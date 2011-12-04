'------------------------------------------------------
'| Projet         |  Magic The Gathering Manager      |
'| Contexte       |  		Perso                     |
'| Date           |                        30/03/2008 |
'| Release 1      |                        12/04/2008 |
'| Release 2      |                        30/08/2008 |
'| Release 3      |                        08/11/2008 |
'| Release 4      |                        29/08/2009 |
'| Release 5      |                        21/03/2010 |
'| Release 6      |                        17/04/2010 |
'| Release 7      |                        29/07/2010 |
'| Release 8      |                        03/10/2010 |
'| Release 9      |                        05/02/2011 |
'| Release 10     |                        10/09/2011 |
'| Auteur         |                          Couitchy |
'|----------------------------------------------------|
'| Modifications :                                    |
'| - watchdog sur la connexion Magic-Ville 20/02/2010 |
'------------------------------------------------------
Imports SourceGrid2
Imports Cells = SourceGrid2.Cells.Real
Imports System.IO
Public Partial Class frmBuyMV
	Private Const CmURL As String = "http://magic-ville.fr/fr/"
	Private VmIsComplete As Boolean = False
	Private VmToBuy As New List(Of clsLocalCard)
	Private VmToSell As New List(Of clsMVCard)	
	Public Sub New()
		Me.InitializeComponent()
	End Sub
	Public Sub AddToBasket(VpName As String)
	'------------------------------------------------------------------
	'Incr�mente la quantit� du panier pour la carte pass�e en param�tre
	'------------------------------------------------------------------
		'Carte d�j� mise au panier
		For Each VpCard As clsLocalCard In VmToBuy
			If VpCard.Name = VpName Then
				VpCard.Quantite = VpCard.Quantite + 1
				Return
			End If
		Next
		'Nouvelle carte
		VmToBuy.Add(New clsLocalCard(VpName))
	End Sub
	Public Sub AddToBasket(VpName As String, VpQuant As Integer)
		For VpI As Integer = 1 To VpQuant
			Call Me.AddToBasket(VpName)
		Next VpI
	End Sub	
	Private Sub BrowseAndWait(Optional VpURL As String = "")
	'---------------------------------------------------------------------------
	'Navigue sur la page pass�e en param�tre en respectant le d�lai d'expiration
	'---------------------------------------------------------------------------
	Dim VpStart As Date = Now
		VmIsComplete = False
		If VpURL <> "" Then
			Me.wbMV.Navigate(VpURL)		
		End If
		While Not VmIsComplete
			If Now.Subtract(VpStart).TotalSeconds > clsModule.CgTimeOut Then
				Me.wbMV.Stop
				VmIsComplete = True
			End If
			Application.DoEvents
		End While
	End Sub
	Private Sub ClearSellList
	'--------------------------------------------------------
	'Supprime les entr�es de la liste dont la quantit� vaut 0
	'--------------------------------------------------------
	Dim VpToRemove As New List(Of clsMVCard)
		'R�cup�ration des �l�ments � supprimer
		For Each VpProposition As clsMVCard In VmToSell
			If VpProposition.Quantite = 0 Then
				VpToRemove.Add(VpProposition)
			End If
		Next VpProposition
		'Suppression effective
		For Each VpDelete As clsMVCard In VpToRemove
			VmToSell.Remove(VpDelete)
		Next VpDelete
	End Sub
	Private Sub DownloadMVInfos(VpCard As String)
	'---------------------------------------------------------------------------------------------------------------------
	'Se connecte au site de Magic-Ville pour r�cup�rer les informations de ventes relatives � la carte pass�e en param�tre
	'---------------------------------------------------------------------------------------------------------------------
	Dim VpElement As HtmlElement
	Dim VpLastId As Integer = 0
	Dim VpCurId As Integer
	Dim VpProposition As clsMVCard = Nothing
		'Site de Magic-Ville
		Call Me.BrowseAndWait(CmURL)
		'Saisie de la carte dans la zone de recherche
		VpElement = Me.wbMV.Document.All.GetElementsByName("recherche_titre").Item(0)
		VpElement.SetAttribute("value", VpCard)
		For Each VpElement In Me.wbMV.Document.All
			If VpElement.GetAttribute("src").ToLower.Contains("/go.gif") Then
				'Validation
				VpElement.InvokeMember("click")
				Call Me.BrowseAndWait
				Exit For
			End If
		Next VpElement		
		'Page interm�diaire (ne s'affiche qu'en cas d'ambiguit�)
		For Each VpElement In Me.wbMV.Document.All
			If VpElement.GetAttribute("href") <> "" AndAlso Not VpElement.InnerText Is Nothing Then
				If VpElement.InnerText.ToLower.Trim = VpCard.ToLower Then
					'Validation
					VpElement.InvokeMember("click")
					Call Me.BrowseAndWait
					Exit For
				End If
			End If
		Next VpElement
		'Page des achats
		For Each VpElement In Me.wbMV.Document.All
			If VpElement.InnerText = "Achetez cette carte � un magicvillois" Then
				'Validation
				VpElement.InvokeMember("click")
				Call Me.BrowseAndWait
				Exit For
			End If
		Next VpElement
		'Parsing
		For Each VpElement In Me.wbMV.Document.All
			If VpElement.Name.Contains("[") Then
				VpCurId = Val(VpElement.Name.Substring(VpElement.Name.IndexOf("[") + 1))
				If VpCurId <> VpLastId Then
					VpProposition = New clsMVCard(VpCard)
					VmToSell.Add(VpProposition)
					VpLastId = VpCurId
				End If
				With VpProposition
					If VpElement.Name.Contains("qte") Then
						.Quantite = CInt(VpElement.InnerText.Substring(VpElement.InnerText.Length - 1))
					ElseIf VpElement.Name.Contains("ref") Then
						.Edition = VpElement.GetAttribute("value")
					ElseIf VpElement.Name.Contains("lang") Then
						.Edition = .Edition + " " + VpElement.GetAttribute("value")
					ElseIf VpElement.Name.Contains("etat") Then
						.Etat = VpElement.GetAttribute("value")
					ElseIf VpElement.Name.Contains("seller") Then
						.Vendeur = VpElement.GetAttribute("value")						
					End If
				End With
			ElseIf VpElement.GetAttribute("color") = "#3333ff" Then
				VpProposition.Prix = clsModule.MyVal(VpElement.InnerText)
			End If
		Next VpElement
	End Sub
	Private Function SortBySellers(VpToSell As List(Of clsMVCard)) As ArrayList
	'------------------------------------------------------------------------
	'Retourne une List(Of ) d'List(Of ) des cartes vendues par chaque vendeur
	'------------------------------------------------------------------------
	Dim VpTable As New ArrayList
	Dim VpSellers As New List(Of String)
	Dim VpL As List(Of clsMVCard)
		'Identifie les vendeurs en pr�sence
		For Each VpCard As clsMVCard In VpToSell
			If Not VpSellers.Contains(VpCard.Vendeur) Then
				VpSellers.Add(VpCard.Vendeur)
			End If
		Next VpCard
		'Pour chaque vendeur
		For Each VpSeller As String In VpSellers
			VpL = New List(Of clsMVCard)
			'Lui associe les cartes qu'il vend
			For Each VpCard As clsMVCard In VpToSell
				If VpCard.Vendeur = VpSeller Then
					VpL.Add(VpCard)
				End If
			Next VpCard
			'On fait un pr�-tri (dans lequel le crit�re de vendeur pr�f�r� ne compte pas puisque toutes les cartes de la liste pass�e en param�tre sont issues du m�me vendeur
			VpL.Sort(New clsMVCardComparer(Me.pnlGestion.Tag))			
			VpTable.Add(VpL)
		Next VpSeller
		'Tri selon le crit�re s�lectionn� par l'utilisateur
		VpTable.Sort(New clsSellerComparer(VmToBuy, Me.pnlGestion.Tag))
		Return VpTable
	End Function	
	Private Function CalcTransactions As String
	'--------------------------------------------------------------------------------------------------
	'D�termine les transactions n�cessaires pour acheter toutes les cartes d�sir�es de mani�re optimale
	'--------------------------------------------------------------------------------------------------
	Dim VpToBuy As List(Of clsLocalCard) = clsModule.MyClone(VmToBuy)
	Dim VpToSell As List(Of clsMVCard) = clsModule.MyClone2(VmToSell)
	Dim VpToRemove As New List(Of clsLocalCard)
	Dim VpToRemove2 As New List(Of clsMVCard)
	Dim VpTable As ArrayList
	Dim VpN As Integer
	Dim VpStr As String = clsModule.CgTransactionsMV + vbCrLf
	Dim VpCout As Single = 0
	Dim VpCount As Short = 1
		While VpToBuy.Count > 0
			'D�termine la meilleure transaction sur les cartes restant � acheter
			VpTable = Me.SortBySellers(VpToSell)
			If VpTable.Count > 0 Then
				'M�morise la transaction
				VpToRemove.Clear
				VpStr = VpStr + vbCrLf + "Transaction n�" + VpCount.ToString
				For Each VpMVCard As clsMVCard In VpTable.Item(0)
					For Each VpLocalCard As clsLocalCard In VpToBuy
						If VpLocalCard.Name = VpMVCard.Name And VpLocalCard.Quantite > 0 Then
							VpN = Math.Min(VpLocalCard.Quantite, VpMVCard.Quantite)
							VpLocalCard.Quantite = VpLocalCard.Quantite - VpN
							If VpLocalCard.Quantite = 0 Then
								VpToRemove.Add(VpLocalCard)
							End If
							VpStr = VpStr + vbCrLf + VpMVCard.Vendeur + " - " + VpMVCard.Name + " - " + VpMVCard.Etat.ToString + " - " + VpN.ToString + " � " + VpMVCard.Prix.ToString + " �"
							VpCout = VpCout + VpN * VpMVCard.Prix
						End If
					Next VpLocalCard					
				Next VpMVCard
				'Supprime les cartes pr�sentes dans la transaction pour pr�parer la suivante
				VpToRemove2.Clear
				For Each VpLocalCard As clsLocalCard In VpToRemove
					'Suppression dans la liste des cartes � acheter
					VpToBuy.Remove(VpLocalCard)
					For Each VpMVCard As clsMVCard In VpToSell
						If VpLocalCard.Name = VpMVCard.Name Then
							VpToRemove2.Add(VpMVCard)
						End If
					Next VpMVCard
				Next VpLocalCard	
				For Each VpMVCard As clsMVCard In VpToRemove2
					'Suppression dans la liste des cartes � vendre
					VpToSell.Remove(VpMVCard)
				Next VpMVCard
			Else
				Call clsModule.ShowWarning("Toutes les cartes ne sont pas disponibles." + vbCrLf + "Essayez d'actualiser les offres ou de modifier les quantit�s...")
				Exit While
			End If
			VpCount = VpCount + 1
		End While
		Me.txtTot.Text = VpCout.ToString + " �"
		Return VpStr
	End Function
	Private Sub InsertRow(VpS As String, VpN As Integer, VpCellModel As DataModels.IDataModel)
		With Me.grdBasket
			.Rows.Insert(.RowsCount)
			Me.grdBasket(.RowsCount - 1, 0) = New Cells.Cell(VpS)
			Me.grdBasket(.RowsCount - 1, 1) = New Cells.Cell(VpN)	
			Me.grdBasket(.RowsCount - 1, 1).DataModel = VpCellModel
		End With
	End Sub
	Public Sub LoadGrid(VpMode As clsModule.eBasketMode)
	Dim VpCellModel As DataModels.IDataModel
		'Mode 1 : R�sultats de la recherche sur Magic-Ville
		If VpMode = clsModule.eBasketMode.MV Then
			'Pr�paration de la grille
			With Me.grdBasket
				'Nettoyage
				If .Rows.Count > 0 Then					
					.Rows.RemoveRange(0, .Rows.Count)
				End If
				'Nombre de colonnes et d'en-t�tes
				.ColumnsCount = 6
				.FixedRows = 1
				.Rows.Insert(0)
				Me.grdBasket(0, 0) = New Cells.ColumnHeader("Nom de la carte")
				Me.grdBasket(0, 1) = New Cells.ColumnHeader("Vendeur")
				Me.grdBasket(0, 2) = New Cells.ColumnHeader("Edition")
				Me.grdBasket(0, 3) = New Cells.ColumnHeader("Etat")
				Me.grdBasket(0, 4) = New Cells.ColumnHeader("Quantit�")
				Me.grdBasket(0, 5) = New Cells.ColumnHeader("Prix unitaire")
				'Remplissage offres
				For VpI As Integer = 0 To VmToSell.Count - 1
					.Rows.Insert(VpI + 1)
					Me.grdBasket(VpI + 1, 0) = New Cells.Cell(VmToSell.Item(VpI).Name)
					Me.grdBasket(VpI + 1, 1) = New Cells.Cell(VmToSell.Item(VpI).Vendeur)
					Me.grdBasket(VpI + 1, 2) = New Cells.Cell(VmToSell.Item(VpI).Edition)
					Me.grdBasket(VpI + 1, 3) = New Cells.Cell(VmToSell.Item(VpI).Etat)
					Me.grdBasket(VpI + 1, 4) = New Cells.Cell(VmToSell.Item(VpI).Quantite)
					Me.grdBasket(VpI + 1, 5) = New Cells.Cell(VmToSell.Item(VpI).Prix.ToString + " �")
				Next VpI
				.AutoSize
			End With			
		'Mode 2 : Cartes � acheter
		Else
			VpCellModel = Utility.CreateDataModel(Type.GetType("System.Int32"))	
			VpCellModel.EditableMode = EditableMode.AnyKey Or EditableMode.DoubleClick
			AddHandler VpCellModel.Validated, AddressOf CellValidated	
			'Pr�paration de la grille
			With Me.grdBasket
				'Nettoyage
				If .Rows.Count > 0 Then					
					.Rows.RemoveRange(0, .Rows.Count)
				End If
				'Nombre de colonnes et d'en-t�tes
				.ColumnsCount = 2
				.FixedRows = 1
				.Rows.Insert(0)
				Me.grdBasket(0, 0) = New Cells.ColumnHeader("Nom de la carte")
				Me.grdBasket(0, 1) = New Cells.ColumnHeader("Quantit�")
				'Remplissage panier
				For Each VpCard As clsLocalCard In VmToBuy
					Call Me.InsertRow(VpCard.Name, VpCard.Quantite, VpCellModel)
				Next VpCard
				.AutoSize
			End With			
		End If
	End Sub
	Private Sub BasketSave(VpFile As String)
	'--------------------
	'Sauvegarde du panier
	'--------------------
	Dim VpBasket As New StreamWriter(VpFile)
		For Each VpCard As clsLocalCard In VmToBuy
			VpBasket.WriteLine(VpCard.Name + "#" + VpCard.Quantite.ToString)
		Next VpCard
		VpBasket.Close
	End Sub
	Private Sub BasketLoad(VpFile As String)
	'----------------------
	'Chargement d'un panier
	'----------------------
	Dim VpBasket As New StreamReader(VpFile)
	Dim VpItem(0 To 1) As String
		While Not VpBasket.EndOfStream
			VpItem = VpBasket.ReadLine.Split("#")
			Call Me.AddToBasket(VpItem(0), Val(VpItem(1)))
		End While
		VpBasket.Close
		Call Me.LoadGrid(eBasketMode.Local)
	End Sub
	Private Sub CellValidated(sender As Object, e As CellEventArgs)	
	Static VsClearing As Boolean = False	'Crade mais �vite une boucle infinie due au fait que la suppression d'un row d�clenche l'�v�nement de validation de mani�re indiscernable par rapport � une fin d'�dition de cellule
	Dim VpQ As Short
		If Not VsClearing Then
			VsClearing = True
			VmToBuy.Clear
			'Mise � jour de la quantit� dans la grille et dans l'List(Of )
			For VpI As Integer = 1 To Me.grdBasket.RowsCount - 1
				VpQ = Me.grdBasket(VpI, 1).Value
				If VpQ > 0 Then
					VmToBuy.Add(New clsLocalCard(Me.grdBasket(VpI, 0).Value, VpQ))
				End If
			Next VpI
			Call Me.LoadGrid(clsModule.eBasketMode.Local)
			VsClearing = False
		End If
	End Sub
	Sub BtLocalBasketActivate(ByVal sender As Object, ByVal e As EventArgs)
		Call Me.LoadGrid(eBasketMode.Local)
	End Sub
	Sub BtMVBasketActivate(ByVal sender As Object, ByVal e As EventArgs)
		Call Me.LoadGrid(eBasketMode.MV)
	End Sub
	Sub CmdRefreshClick(ByVal sender As Object, ByVal e As EventArgs)
		Me.cmdRefresh.Visible = False
		Me.cmdCancelMV.Tag = False
		Me.cmdCancelMV.Visible = True
		Me.prgRefresh.Maximum = VmToBuy.Count
		Me.prgRefresh.Value = 0
		'R�cup�re les informations pour chaque carte
		Try
			For Each VpCard As clsLocalCard In VmToBuy
				If Me.cmdCancelMV.Tag = True Then Exit For
				Call Me.DownloadMVInfos(VpCard.Name)
				Me.prgRefresh.Increment(1)
			Next VpCard
			Me.prgRefresh.Value = 0
			Call Me.ClearSellList
			Call Me.LoadGrid(clsModule.eBasketMode.MV)
		Catch
			Call clsModule.ShowWarning(clsModule.CgDL3b) 
		End Try
		Me.cmdRefresh.Visible = True
		Me.cmdCancelMV.Visible = False
	End Sub	
	Sub WbMVDocumentCompleted(ByVal sender As Object, ByVal e As WebBrowserDocumentCompletedEventArgs)
		VmIsComplete = True
	End Sub
	Sub CmdCalcClick(ByVal sender As Object, ByVal e As EventArgs)
	Dim VpStr As String = Me.CalcTransactions
		If VpStr.Trim <> clsModule.CgTransactionsMV Then
			Call clsModule.ShowInformation(VpStr)
		End If
	End Sub
	Sub OptGestionCheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
		If sender.Checked = True
			Select Case sender.Name.ToString
				Case "optPrice"
					Me.pnlGestion.Tag = clsModule.eSortCriteria.Price
				Case "optQuality"
					Me.pnlGestion.Tag = clsModule.eSortCriteria.Quality
				Case "optSeller"
					Me.pnlGestion.Tag = clsModule.eSortCriteria.Seller
			End Select
		End If
	End Sub
	Sub MnuAddSellerClick(ByVal sender As Object, ByVal e As EventArgs)
	Dim VpStr As String = InputBox("Quel est le nom du vendeur de confiance � ajouter ?", "Ajout d'un vendeur pr�f�r�", "(Vendeur)")
		If VpStr <> "" Then
			With VgOptions
				.VgSettings.PreferredSellers = VpStr + "#" + .VgSettings.PreferredSellers
				Call .SaveSettings
			End With
			Me.lstSeller.Items.Add(VpStr)
		End If
	End Sub
	Sub FrmBuyMVLoad(ByVal sender As Object, ByVal e As EventArgs)
		'Crit�re par d�faut
		Me.pnlGestion.Tag = clsModule.eSortCriteria.Price
		'Liste des vendeurs pr�f�r�s
		For Each VpSeller As String In VgOptions.VgSettings.PreferredSellers.Split("#")
			If VpSeller <> "" Then
				Me.lstSeller.Items.Add(VpSeller)
			End If
		Next VpSeller
	End Sub
	Sub LstSellerMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs)
	Dim VpItem As Integer = Me.lstSeller.IndexFromPoint(e.Location)
		'Menu contextuel ajout / suppression de vendeurs pr�f�r�s
		If e.Button = System.Windows.Forms.MouseButtons.Right Then
			Me.mnuRemoveSeller.Enabled = ( VpItem >= 0 )
			Me.cmnuSeller.Show(sender, e.Location)
		End If		
	End Sub
	Sub MnuRemoveSellerClick(ByVal sender As Object, ByVal e As EventArgs)
		If Me.lstSeller.SelectedIndex >= 0 Then
			With VgOptions
				.VgSettings.PreferredSellers = .VgSettings.PreferredSellers.Replace(Me.lstSeller.SelectedItem + "#", "")
				Call .SaveSettings
			End With
			Me.lstSeller.Items.RemoveAt(Me.lstSeller.SelectedIndex)
		End If
	End Sub
	Sub BtSaveBasketActivate(sender As Object, e As EventArgs)
		Me.dlgSave.FileName = ""
		Me.dlgSave.ShowDialog
		If Me.dlgSave.FileName <> "" Then
			Call Me.BasketSave(Me.dlgSave.FileName)
		End If
	End Sub
	Sub BtLoadBasketActivate(sender As Object, e As EventArgs)
		Me.dlgOpen.FileName = ""
		Me.dlgOpen.ShowDialog
		If Me.dlgOpen.FileName <> "" Then
			Call Me.BasketLoad(Me.dlgOpen.FileName)
		End If		
	End Sub
	Sub CmdCancelMVClick(sender As Object, e As EventArgs)
		Me.cmdCancelMV.Tag = True
		Me.cmdCancelMV.Visible = False
		Me.cmdRefresh.Visible = True
	End Sub
End Class
Public Class clsLocalCard
	Private VmName As String
	Private VmQuant As Integer
	Public Sub New(VpName As String, Optional VpQuant As Integer = 1)
		VmName = VpName
		VmQuant = VpQuant
	End Sub
	Public ReadOnly Property Name As String
		Get
			Return VmName
		End Get
	End Property
	Public Property Quantite As Integer
		Get
			Return VmQuant
		End Get
		Set (VpQuant As Integer)
			VmQuant = VpQuant
		End Set
	End Property
End Class
Public Class clsMVCard
	Private VmName As String
	Private VmVendeur As String
	Private VmEdition As String
	Private VmEtat As clsModule.eQuality
	Private VmQuant As Integer
	Private VmPrix As Single
	Public Sub New(VpName As String, Optional VpVendeur As String = "", Optional VpEdition As String = "", Optional VpEtat As clsModule.eQuality = clsModule.eQuality.Mint, Optional VpQuant As Integer = 0, Optional VpPrix As Single = 0)
		VmName = VpName
		VmVendeur = VpVendeur
		VmEdition = VpEdition
		VmEtat = VpEtat
		VmQuant = VpQuant
		VmPrix = VpPrix
	End Sub
	Public ReadOnly Property Name As String
		Get
			Return VmName
		End Get
	End Property
	Public Property Vendeur As String
		Get
			Return VmVendeur
		End Get
		Set (VpVendeur As String)
			VmVendeur = VpVendeur
		End Set
	End Property
	Public Property Edition As String
		Get
			Return VmEdition
		End Get
		Set (VpEdition As String)
			VmEdition = VpEdition
		End Set
	End Property
	Public Property Etat As clsModule.eQuality
		Get
			Return VmEtat
		End Get
		Set (VpEtat As clsModule.eQuality)
			VmEtat = VpEtat
		End Set
	End Property
	Public Property Quantite As Integer
		Get
			Return VmQuant
		End Get
		Set (VpQuant As Integer)
			VmQuant = VpQuant
		End Set
	End Property
	Public Property Prix As Single
		Get
			Return VmPrix
		End Get
		Set (VpPrix As Single)
			VmPrix = VpPrix
		End Set
	End Property	
End Class
Public Class clsSellerComparer 
	Implements IComparer
	Private VmToBuy As List(Of clsLocalCard)	
	Private VmCriterion As clsModule.eSortCriteria
	Private Function GetCommon(VpToSell As List(Of clsMVCard)) As List(Of clsMVCard)
	'--------------------------------------------------------------------------------------------------------------------------------------------------------------------
	'Retourne les cartes communes entre les cartes � acheter et celles � vendre dont la liste est pass�e en param�tre en privil�giant le crit�re choisi par l'utilisateur
	'--------------------------------------------------------------------------------------------------------------------------------------------------------------------
	Dim VpCommon As New List(Of clsMVCard)
	Dim VpToBuy As List(Of clsLocalCard) = clsModule.MyClone(VmToBuy)
	Dim VpN As Integer
		'D�termine les cartes en commun
		For Each VpMVCard As clsMVCard In VpToSell
			For Each VpLocalCard As clsLocalCard In VpToBuy
				If VpLocalCard.Name = VpMVCard.Name And VpLocalCard.Quantite > 0 Then
					VpN = Math.Min(VpLocalCard.Quantite, VpMVCard.Quantite)
					VpLocalCard.Quantite = VpLocalCard.Quantite - VpN
					VpCommon.Add(New clsMVCard(VpLocalCard.Name, VpMVCard.Vendeur, VpMVCard.Edition, VpMVCard.Etat, VpN, VpMVCard.Prix))
				End If
			Next VpLocalCard			
		Next VpMVCard
		Return VpCommon
	End Function
	Private Function GetNCommon(VpToSell As List(Of clsMVCard)) As Integer
	'--------------------------------------------------------------------------------------------------------------------------
	'Retourne le nombre de cartes en commun entre les cartes � acheter et celles � vendre dont la liste est pass�e en param�tre
	'--------------------------------------------------------------------------------------------------------------------------
	Dim VpN As Integer = 0
		For Each VpCard As clsMVCard In Me.GetCommon(VpToSell)
			VpN = VpN + VpCard.Quantite
		Next VpCard
		Return VpN
	End Function	
	Private Function PackPrice(VpToSell As List(Of clsMVCard)) As Single
	'--------------------------------------------------------------------------------------------------------------------------
	'Retourne le prix total des cartes communes entre les cartes d�sir�es et celles propos�es dans la liste pass�e en param�tre
	'- � minimiser -
	'--------------------------------------------------------------------------------------------------------------------------
	Dim VpP As Single = 0
		For Each VpCard As clsMVCard In Me.GetCommon(VpToSell)
			VpP = VpP + VpCard.Prix
		Next VpCard	
		Return VpP
	End Function
	Private Function PackQuality(VpToSell As List(Of clsMVCard)) As Integer
	'-------------------------------------------------------------------------------------------------------------------------
	'Retourne l'�tat total des cartes communes entre les cartes d�sir�es et celles propos�es dans la liste pass�e en param�tre
	'- � minimiser (car Mint->0) -
	'-------------------------------------------------------------------------------------------------------------------------
	Dim VpQ As Integer = 0
		For Each VpCard As clsMVCard In Me.GetCommon(VpToSell)
			VpQ = VpQ + CInt(VpCard.Etat)
		Next VpCard	
		Return VpQ
	End Function
	Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
	'-------------------------------------------------------------------------------------------------
	'Tri selon le nombre de cartes propos�es par les vendeurs (minimisation du nombre de transactions)
	'- en cas d'�galit�, utilise les crit�res sp�cifi�s par l'utilisateur -
	'La fonction doit retourner < 0 pour faire monter x, > 0 pour faire monter y
	'-------------------------------------------------------------------------------------------------
	Dim VpN1 As Integer
	Dim VpN2 As Integer
		If x Is y Then Return 0
		VpN1 = Me.GetNCommon(x)
		VpN2 = Me.GetNCommon(y)		
		If VpN1 > VpN2 Then
			Return -1
		ElseIf VpN1 < VpN2 Then
			Return 1
		Else
			Select Case VmCriterion
				Case clsModule.eSortCriteria.Price
					Return If((Me.PackPrice(x) - Me.PackPrice(y)) < 0, -1, 1)
				Case clsModule.eSortCriteria.Quality
					Return ( Me.PackQuality(x) - Me.PackQuality(y) )
				Case clsModule.eSortCriteria.Seller
					Return VgOptions.VgSettings.PreferredSellers.ToLower.IndexOf(y.Item(0).Vendeur.ToLower)	'jolie ruse : si la liste des vendeurs pr�f�r�s ne contient pas celui qui vend y, la fonction retourne -1 faisant monter x
			End Select
		End If
	End Function	
	Public Sub New(VpToBuy As List(Of clsLocalCard), VpCriterion As clsModule.eSortCriteria)
		VmToBuy = VpToBuy
		VmCriterion = VpCriterion
	End Sub
End Class
Public Class clsMVCardComparer
	Implements IComparer(Of clsMVCard)
	Private VmCriterion As clsModule.eSortCriteria	
	Public Function Compare(ByVal x As clsMVCard, ByVal y As clsMVCard) As Integer Implements IComparer(Of clsMVCard).Compare
	'---------------------------------------------------------------------------
	'Tri selon le crit�re pass� au constructeur
	'La fonction doit retourner < 0 pour faire monter x, > 0 pour faire monter y
	'---------------------------------------------------------------------------
		If x Is y Then Return 0
		If VmCriterion = clsModule.eSortCriteria.Quality Then
			If CInt(x.Etat) < CInt(y.Etat) Then
				Return -1
			Else
				Return 1
			End If
		Else
			If x.Prix > y.Prix Then
				Return 1
			Else
				Return -1
			End If
		End If
	End Function
	Public Sub New(VpCriterion As clsModule.eSortCriteria)
		VmCriterion = VpCriterion
	End Sub
End Class