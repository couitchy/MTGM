'------------------------------------------------------
'| Projet         |  Magic The Gathering Manager      |
'| Contexte       |  		Perso                     |
'| Date           |                        30/03/2008 |
'| Release 1      |                        12/04/2008 |
'| Release 2      |                        30/08/2008 |
'| Release 3      |                        08/11/2008 |
'| Release 4      |                        29/08/2009 |
'| Auteur         |                          Couitchy |
'|----------------------------------------------------|
'| Modifications :                                    |
'| - options gérées en classe instantiable 08/11/2008 |
'| - fonctionnalité import / export		   09/11/2008 |
'| - gestion decks en BDD au lieu d'INI	   07/01/2009 |
'| - MAJ semi-auto base d'images		   18/01/2009 |
'| - images des cartes sur le mainform	   09/02/2009 |
'| - mémorisation de la taille du mainform 03/10/2009 |
'| - sélection multiple dans le treeview   10/10/2009 |
'| - MAJ auto prix						   14/10/2009 |
'| - MAJ auto images					   18/10/2009 |
'------------------------------------------------------
#Region "Importations"
Imports TD.SandBar
Imports TreeViewMS
Imports System.IO
#End Region
Public Partial Class MainForm
	#Region "Déclarations"
	Private VmSearch As New clsSearch
	Private VmAdvSearch As String = ""
	Private VmMustReload As Boolean = False
	Public Shared VgMe As MainForm
	#End Region
	#Region "Méthodes"
	Public Sub New()
		VgMe = Me
		'Intégrité de l'application
		If Not clsModule.CheckIntegrity Then
			Process.GetCurrentProcess.Kill
			Exit Sub				
		End If			
		Me.InitializeComponent()
		clsModule.VgTray = New NotifyIcon(Me.components)
		clsModule.VgTray.Icon = Me.Icon
		clsModule.VgTray.Text = "Magic The Gathering Manager"
		clsModule.VgTimer = New Timer
		clsModule.VgTimer.Interval = 1000 * 10		'recherche d'une mise à jour 10 sec après le démarrage
		'Comme l'ordre des critères est amené à changer et qu'il serait fastidieux de conserver les bons indices, mieux vaut passer par une table de hachage
		Call clsModule.InitCriteres(Me)
		'Anciens fichiers temporaires éventuels
		Call clsModule.DeleteTempFiles
		'Image par défaut
		Me.picScanCard.Image = Image.FromFile(Application.StartupPath + clsModule.CgMagicBack)
	End Sub	
	Public Function StatusTextGet As String
		If Not Me.lblDB Is Nothing Then
			Return Me.lblDB.Text
		Else
			Return ""
		End If
	End Function
	Public Sub StatusText(VpStr As String, Optional VpClearN As Boolean = False)
		If Not Me.lblDB Is Nothing Then
			Me.lblDB.Text = VpStr
			If VpClearN Then
				Me.lblNCards.Text = ""
			End If
			Application.DoEvents
		End If
	End Sub	
	Private Sub UpdatePrices(VpFile As String)
	'---------------------------------------------------------------------------------------------------------
	'Met à jour le prix des cartes dans la base de données à partir du fichier de cotation fourni en paramètre
	'---------------------------------------------------------------------------------------------------------
	Dim VpPrices As New StreamReader(VpFile)
	Dim VpCardData() As String		
	Dim VpPrice As String
	Dim VpEdition As String
	Dim VpCardName As String
	Dim VpDate As String
		'Vérifie que le fichier contient des prix à jour
		VpDate = VpPrices.ReadLine
		If IsDate(VpDate) Then
			If clsModule.GetLastPricesDate.Subtract(VpDate).Days >= 0 Then
				VpPrices.Close
				Call clsModule.ShowInformation("Les prix sont déjà à jour...")
				Exit Sub
			End If
		Else
			VpDate = File.GetLastWriteTimeUtc(VpFile).ToShortDateString
			VpPrices.BaseStream.Seek(0, SeekOrigin.Begin)
		End If
		If Not clsModule.ShowQuestion("Les prix vont être mis à jour avec la liste suivante :" + vbCrLf + VpFile + vbCrLf + "L'opération pourra durer plusieurs secondes, patienter jusqu'à la notification... Continuer ?") = System.Windows.Forms.DialogResult.Yes Then
			VpPrices.Close
			Exit Sub
		End If
		While Not VpPrices.EndOfStream
			VpCardData = VpPrices.ReadLine.Split("#")
			VpCardName = ""
			VpEdition = ""
			VpPrice = ""
			For Each VpStr As String In VpCardData
				If VpStr.IndexOf("^") <> -1 Then
					VpEdition = VpStr.Substring(0, VpStr.IndexOf("^")).Replace("'", "''")
					VpPrice = VpStr.Substring(VpStr.IndexOf("^") + 1).Replace("€", "").Trim
					VgDBCommand.CommandText = "Update Card Inner Join Series On Card.Series = Series.SeriesCD Set Card.Price = '" + VpPrice + "', Card.myPrice = '" + clsModule.MyPrice(VpPrice).ToString + "', PriceDate = " + clsModule.GetDate(VpDate) + " Where Series.SeriesNM_MtG = '" + VpEdition + "' And Card.Title = '" + VpCardName + "';"
					VgDBCommand.ExecuteNonQuery
				Else
					VpCardName = VpStr.Replace("'", "''")
				End If
			Next VpStr
			Application.DoEvents
		End While	
		Call Me.FixPrices
		VpPrices.Close
		Call clsModule.ShowInformation("Mise à jour des prix terminée !")
	End Sub
	Public Sub UpdatePictures(VpFile As String, VpLogFile As String, Optional VpKillThem As Boolean = False)
	'------------------------------------------------------------------------------------------------------------------------------------
	'Met à jour la base d'images des cartes en concaténant le nouveau fichier à l'ancien et en calculant les nouveaux indexes dans la BDD
	'------------------------------------------------------------------------------------------------------------------------------------
	Dim VpLog As New StreamReader(VpLogFile)
	Dim VpIn As New StreamReader(VpFile)
	Dim VpInB As New BinaryReader(VpIn.BaseStream)
	Dim VpOut As New StreamWriter(VgOptions.VgSettings.PicturesFile, True)
	Dim VpOutB As New BinaryWriter(VpOut.BaseStream)	
	Dim VpFileInfo As New FileInfo(VpFile)
	Dim VpStrs() As String
	Dim VpOffsetBase As Long
	Dim VpCurOffset As Long
	Dim VpCurEnd As Long
	Dim VpCount As Integer = 0
		'Concaténation des données brutes
		VpOutB.Write(VpInB.ReadBytes(VpFileInfo.Length))
		VpIn.Close
		VpOutB.Flush
		VpOutB.Close
		'Mise à jour des indexes
		VgDBCommand.CommandText = "Select Max([End]) From CardPictures;"
		VpOffsetBase = CLng(VgDBCommand.ExecuteScalar) + 1 	
		While Not VpLog.EndOfStream
			VpStrs = VpLog.ReadLine.Split("#")
			VpCurOffset = VpOffsetBase + CLng(VpStrs(1))
			VpCurEnd = VpOffsetBase + CLng(VpStrs(2))
			VgDBCommand.CommandText = "Insert Into CardPictures Values ('" + VpStrs(0).Replace(".jpg", "").Replace("'", "''") + "', " + VpCurOffset.ToString + ", " + VpCurEnd.ToString + ");"
			VgDBCommand.ExecuteNonQuery
			VpCount = VpCount + 1
		End While
		VpLog.Close
		'Suppression éventuel des fichiers d'update
		If VpKillThem Then
			Call clsModule.SecureDelete(VpFile)
			Call clsModule.SecureDelete(VpLogFile)
		End If
		Call clsModule.ShowInformation("Mise à jour des images des cartes terminée !" + vbCrLf + "(" + VpCount.ToString + " cartes ajoutées)")
	End Sub
	Private Sub FixPrices
	'-----------------------------------------
	'Remplace tous les prix non indiqués par 0
	'-----------------------------------------
		VgDBCommand.CommandText = "Update Card Set Price = '0', myPrice = '1' Where Card.Price In ('0', Null);"
		VgDBCommand.ExecuteNonQuery
	End Sub
	Private Sub FixFR
	'-----------------------------------------------------
	'Remplace une traduction vide par son original anglais
	'-----------------------------------------------------
		VgDBCommand.CommandText = "Update CardFR Inner Join Card On Card.EncNbr = CardFR.EncNbr Set CardFR.TitleFR = Card.Title Where CardFR.TitleFR In (Null, '');"
		VgDBCommand.ExecuteNonQuery	
	End Sub
	Private Sub FixSerie(VpSerie As String)
	'-----------------------------------------------------------------------------------------------------------------------------------------------------
	'Correction a posteriori d'un bug initial lors de l'ajout dans la base de nouvelles cartes de créatures-artefacts dont le sous-type n'est pas Creature
	'-----------------------------------------------------------------------------------------------------------------------------------------------------
	Dim VpArtefacts As New ArrayList
	Dim VpFile As StreamReader
	Dim VpCheckCard As clsMyCard
	Dim VpCounter As Integer = 0
		'Cherche dans la base tous les artefacts de la série concernée
		VgDBCommand.CommandText = "Select Title From Card Inner Join Series On Card.Series = Series.SeriesCD Where SeriesNM = '" + VpSerie + "' And Type = 'A';"
		VgDBReader = VgDBCommand.ExecuteReader
		With VgDBReader
			While .Read
				VpArtefacts.Add(.GetString(0))
			End While
			.Close
		End With
		'Vérifie dans la spoiler list s'il faut les ajouter dans la table Creature (Title, Power, Tough, Nullx37)
		Me.dlgOpen4.FileName = ""
		Me.dlgOpen4.ShowDialog
		If Me.dlgOpen4.FileName <> "" Then
			VpFile = New StreamReader(Me.dlgOpen4.FileName)
			Do While Not VpFile.EndOfStream
				VpCheckCard = New clsMyCard(frmNewEdition.ParseNewCard(VpFile))
				With VpCheckCard
					If Not .Title Is Nothing AndAlso ( .MyType = "A" And .MySubType.Contains("Creature") ) Then
						Try 
							VgDBCommand.CommandText = "Insert Into Creature Values ('" + .Title.Replace("'", "''") + "', " + .MyPower + ", " + .MyTough + ", Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null);"
							VgDBCommand.ExecuteNonQuery
							VpCounter = VpCounter + 1
						Catch
						End Try
					End If
				End With
			Loop
			VpFile.Close	
			Call clsModule.ShowInformation(VpCounter.ToString + " carte(s) ont été corrigée(s) dans la base de données...")
		End If
	End Sub
	Private Sub ValidateCriteria
	'---------------------------------------------------
	'Coche la liste des critères sélectionnés par défaut
	'---------------------------------------------------
	Dim VpCriteria() As String = VgOptions.VgSettings.DefaultCriteria.Split("#")
		For Each VpCriterion As String In VpCriteria
			Try
				Me.chkClassement.SetItemChecked(CInt(VpCriterion), True)
			Catch
			End Try
		Next VpCriterion		
	End Sub
	Private Sub GoFind
	'------------------------------------------
	'Déclenche la procédure de recherche simple
	'------------------------------------------
		If Me.tvwExplore.Nodes.Count > 0 Then
			Me.mnuFindNext.Enabled = True
			VmSearch.ItemsFound.Clear
			Call Me.FindCard(Me.tvwExplore.Nodes.Item(0))
			VmSearch.CurItem = -1
			Call Me.FindNextCard
		End If		
	End Sub
	Private Sub FindCard(VpNode As TreeNode)
	'-------------------------------------------------------------------------------
	'Mémorise les cartes dont le titre contient le texte recherché par l'utilisateur
	'-------------------------------------------------------------------------------
	Dim VpChild As TreeNode
		Try
			If VpNode.Parent.Tag = "Card.Title" And ( VpNode.Text.ToUpper.IndexOf(Me.mnuSearchText.Text.ToUpper) <> -1 Or VpNode.Tag.ToUpper.IndexOf(Me.mnuSearchText.Text.ToUpper) <> -1) Then
				VmSearch.ItemsFound.Add(VpNode)
			End If
		Catch
		End Try
		For Each VpChild In VpNode.Nodes
			Call Me.FindCard(VpChild)
		Next VpChild
	End Sub
	Private Sub FindNextCard	
	'---------------------
	'Poursuit la recherche
	'---------------------
	Dim VpNode As TreeNode
		With VmSearch
			.CurItem = .CurItem + 1
			If .CurItem < .ItemsFound.Count Then
				Me.tvwExplore.BeginUpdate
				Me.tvwExplore.CollapseAll
				VpNode = .ItemsFound.Item(.CurItem)
				VpNode.Expand
				VpNode.EnsureVisible
				Me.tvwExplore.SelectedNode = VpNode
				Me.tvwExplore.EndUpdate
			Else
				Call clsModule.ShowInformation("Aucune autre occurence trouvée !")
				Me.mnuFindNext.Enabled = False
			End If
		End With
	End Sub
	Private Sub ClearCarac
	'--------------------------------------------------------
	'Efface les détails de la carte précédemment sélectionnée
	'--------------------------------------------------------
		Me.lblProp6.Enabled = True
		Me.lblAD.Text = ""
		Me.lblProp1.Enabled = True
		Call clsModule.DeBuildCost(Me, Me)
		Me.cboEdition.Items.Clear
		Me.cboEdition.Text = ""
		Me.picEdition.Image = Nothing
		Me.lblPrix.Text = ""
		Me.lblRarete.Text = ""
		Me.lblStock.Text = ""
		Me.txtCardText.Text = ""
		Me.picScanCard.Image = Image.FromFile(Application.StartupPath + clsModule.CgMagicBack)
	End Sub
	Private Function GetNCards(VpSource As String) As Integer
	'-------------------------------------------------------------------------
	'Retourne le nombre de cartes présentes dans la source passée en paramètre
	'-------------------------------------------------------------------------
	Dim VpSQL As String
		Try
			VpSQL = "Select Sum(Items) From " + VpSource + " Where " + Me.Restriction
			VgDBCommand.CommandText = clsModule.TrimQuery(VpSQL)
			Return VgDBCommand.ExecuteScalar
		Catch
			Return 0
		End Try
	End Function
	Private Function SaveNode(VpNode As TreeNode) As String
	'-----------------------------------------------------
	'Sauvegarder la généalogie du noeud passé en paramètre
	'-----------------------------------------------------
	Dim VpHistory As String = ""
	Dim VpStr As String
		Do
			VpStr = VpNode.Text
			'If ( Not VpNode.Parent Is Nothing ) AndAlso ( VpNode.Parent.Tag = "Card.Title" ) AndAlso Me.mnuCardsFR.Checked Then
			'	VpStr = VpNode.Tag
			'End If
			VpHistory = "#" + VpStr + VpHistory
			VpNode = VpNode.Parent
		Loop Until VpNode Is Me.tvwExplore.Nodes(0)
		Return VpHistory.Substring(1)
	End Function
	Private Sub RestoreNode(VpHistory As String, VpNode As TreeNode)
	'-------------------------------------------------------------------
	'Réouvre la généalogie passée en paramètre aussi loin qu'elle existe
	'-------------------------------------------------------------------
	Dim VpCur As String
	Dim VpLeft As String
		If VpHistory <> "" Then
			If Not VpHistory.IndexOf("#") < 0 Then
				VpCur = VpHistory.Substring(0, VpHistory.IndexOf("#"))
				VpLeft = VpHistory.Substring(VpHistory.IndexOf("#") + 1)				
			Else
				VpCur = VpHistory
				VpLeft = ""
			End If
			For Each VpChild As TreeNode In VpNode.Nodes
				If VpChild.Text = VpCur Then
					Me.tvwExplore.SelectedNode = VpChild
					VpChild.Expand
					Call Me.RestoreNode(VpLeft, VpChild)
				End If
			Next VpChild
		End If
	End Sub
	Public Sub ExcelGen(VpSimple As Boolean)
	'---------------------------------------------------------------------
	'Génération d'une liste des cartes de la sélection courante sous Excel
	'---------------------------------------------------------------------
	Dim VpExcelApp As Object	'Objet Excel par OLE
	Dim VpSQL As String			'Reqûete SQL
	Dim VpY As Integer = 1		'Numéro de ligne courante	
	Dim VpSource As String		'Decks ou Collection
	Dim VpX As Integer = 1		'Nombre de colonnes
		Try
			VpExcelApp = CreateObject("Excel.Application")
		Catch
			Call clsModule.ShowWarning("Aucune installation de Microsoft Excel n'a été détectée sur votre système." + vbCrLf + "Impossible de continuer...")
			Exit Sub
		End Try	
		With VpExcelApp				
			'Nouveau classeur
			.Workbooks.Add
			.Visible = True		
			'Liste simple
			If VpSimple Then
				VpSource = IIf(Me.chkClassement.GetItemChecked(0), clsModule.CgSDecks, clsModule.CgSCollection)
				VpSQL = "Select Title, Items From " + VpSource + " Inner Join Card On " + VpSource + ".EncNbr = Card.EncNbr Where "		
				VpSQL = VpSQL + Me.Restriction
				VpSQL = clsModule.TrimQuery(VpSQL, True, " Order By Card.Title")
				VgDBCommand.CommandText = VpSQL
				VgDBReader = VgDBcommand.ExecuteReader
				While VgDBReader.Read
					.Cells(VpY, 1).FormulaR1C1 = VgDBReader.GetValue(0).ToString
					.Cells(VpY, 2).FormulaR1C1 = VgDBReader.GetValue(1).ToString
					VpY = VpY + 1
				End While
				VgDBReader.Close
				.Columns(1).EntireColumn.AutoFit
				.Columns(2).EntireColumn.AutoFit
			'Liste basée sur le treeview
			Else
				Call Me.RecurExcelGen(Me.tvwExplore.Nodes(0), VpExcelApp, 1, VpX)
				For VpI As Integer = 1 To VpX + 1
					.Columns(VpI).EntireColumn.AutoFit
				Next VpI
				.Cells(1, 1).Select
			End If				
		End With
	End Sub
	Public Sub RecurExcelGen(VpNode As TreeNode, VpExcelApp As Object, VpX As Integer, ByRef VpMaxX As Integer)
	'------------------------------------------------------
	'Méthode de génération récursive de la liste sous Excel
	'------------------------------------------------------
	Dim VpPic As Object			'Icône courante		
	Static VpY As Integer		'Ligne courante
		If VpX = 1 Then 
			VpY = 1
		ElseIf VpX > VpMaxX Then
			VpMaxX = VpX
		End If
		Clipboard.SetImage(Me.imglstTvw.Images(VpNode.ImageIndex))
		With VpExcelApp
			.Cells(VpY, VpX + 1).FormulaR1C1 = VpNode.Text
			.ActiveSheet.Paste
			VpPic = .ActiveSheet.Pictures(VpY)
			VpPic.Top = .Cells(VpY, VpX).Top		
			VpPic.Left = .Cells(VpY, VpX).Left
    		VpPic.ShapeRange.PictureFormat.TransparentBackground = True
    		VpPic.ShapeRange.PictureFormat.TransparencyColor = RGB(0, 0, 211)
			VpY = VpY + 1
		End With
		For Each VpChild As TreeNode In VpNode.Nodes
			Call Me.RecurExcelGen(VpChild, VpExcelApp, VpX + 1, VpMaxX)
		Next VpChild		
	End Sub
	Public Sub LoadMnu
	'------------------------------
	'Chargement des menus variables
	'------------------------------
	Dim VpI As Integer
	Dim VpN As Integer = Me.mnuDisp.Items.Count - 1
		'Nettoyage
		For VpI = 1 + clsModule.CgNDispMenuBase To VpN
			Me.mnuDisp.Items.RemoveAt(Me.mnuDisp.Items.Count - 1)
			Me.mnuFixGames.Items.RemoveAt(Me.mnuFixGames.Items.Count - 1)
			Me.mnuRemGames.Items.RemoveAt(Me.mnuRemGames.Items.Count - 1)
			Me.mnuMoveACard.DropDownItems.RemoveAt(Me.mnuMoveACard.DropDownItems.Count - 1)
		Next VpI
		'Reconstruction
		For VpI = 1 To VgOptions.GetDeckCount			
			Me.mnuRemGames.Items.Add(VgOptions.GetDeckName(VpI), AddressOf MnuRemSubGamesActivate)
			Me.mnuFixGames.Items.Add(VgOptions.GetDeckName(VpI), AddressOf MnuFixSubGamesActivate)
			Me.mnuDisp.Items.Add(VgOptions.GetDeckName(VpI), AddressOf MnuDispCollectionActivate)
			Me.mnuMoveACard.DropDownItems.Add(VgOptions.GetDeckName(VpI), Nothing, AddressOf mnuMoveACardActivate)
		Next VpI	
		'Pour les éditions
		Me.mnuFixSerie.Items.Clear
		VgDBCommand.CommandText = "Select SeriesNM From Series Order By Release Desc;"
		VgDBReader = VgDBCommand.ExecuteReader
		With VgDBReader
			While .Read
				Me.mnuFixSerie.Items.Add(.GetString(0), AddressOf MnuFixSerieActivate)				
			End While
			.Close
		End With
		Me.mnuDispCollection.Checked = True
		Me.chkClassement.SetItemChecked(0, False)
	End Sub
	Private Sub SortTvw
	'-----------------------------------------
	'Tri le treeview dans l'ordre alphabétique
	'-----------------------------------------
	Dim VpNode As TreeNode = Me.tvwExplore.SelectedNode
		Me.mnuSort.Tag = True
		Me.tvwExplore.Sort
		If Not VpNode Is Nothing Then
			Me.tvwExplore.SelectedNode = VpNode
			VpNode.EnsureVisible	
		End If
	End Sub
	Public Sub LoadTvw(Optional VpLoadFromSearch As String = "")
	'-----------------------------------------------------------------------------------------------------------------------------------		
	'Chargement du treeview avec les sélections spécifiées dans le menu Affichage ou bien les résultats de la recherche de l'utilisateur
	'-----------------------------------------------------------------------------------------------------------------------------------		
	Dim VpNode As TreeNode
		If Not clsModule.DBOK Then Exit Sub
		VmAdvSearch = VpLoadFromSearch
		Me.tvwExplore.SelectedNodes.Clear
		Me.tvwExplore.Nodes.Clear
		Me.mnuFindNext.Enabled = False
		Me.mnuSearchText.Text = clsModule.CgCard
		Me.lblDB.Text = "Base - " + VgDB.DataSource
		Call Me.ClearCarac
		If Not Me.IsSourcePresent And VpLoadFromSearch = "" Then
			Call clsModule.ShowWarning("Aucune source de cartes n'a été sélectionnée...")
		Else
			VpNode = New TreeNode
			VpNode.ImageIndex = 1
			VpNode.SelectedImageIndex = 1
			'Cas 1 : chargement des résultats d'une recherche de l'utilisateur
			If VpLoadFromSearch <> "" Then
				Call Me.MnuDispCollectionActivate(Me.mnuDispCollection, Nothing) 'Un peu crade mais il faut absolument déselectionner les decks avant de vouloir charger la recherche (car sinon le me.restriction va altérer l'expression de la requête)
				VpNode.Text = clsModule.CgFromSearch
				Try
					VpNode.Tag = CgCriteres.Item(Me.chkClassement.CheckedItems(0))
				Catch
					Call clsModule.ShowWarning("Aucun critère de classement n'a été sélectionné...")
				End Try					
				Call Me.RecurLoadTvw(VpLoadFromSearch, clsModule.CgSFromSearch, VpNode, 1)				
				Me.lblNCards.Text = ""
			'Cas 2 : chargement des cartes de deck(s)
			ElseIf Me.chkClassement.GetItemChecked(0) Then
				VpNode.Text = clsModule.CgDecks
				Try
					VpNode.Tag = CgCriteres.Item(Me.chkClassement.CheckedItems(1))
				Catch
					Call clsModule.ShowWarning("Aucun critère de classement n'a été sélectionné...")
				End Try
				Call Me.RecurLoadTvw(clsModule.CgSDecks, clsModule.CgSDecks, VpNode, 2)
				Me.lblNCards.Text = "(" + Me.GetNCards(clsModule.CgSDecks).ToString + " cartes attachées)"
			'Cas 3 : chargement de la collection
			Else
				VpNode.Text = clsModule.CgCollection
				Try
					VpNode.Tag = CgCriteres.Item(Me.chkClassement.CheckedItems(0))
				Catch
					Call clsModule.ShowWarning("Aucun critère de classement n'a été sélectionné...")
				End Try					
				Call Me.RecurLoadTvw(clsModule.CgSCollection, clsModule.CgSCollection, VpNode, 1)
				Me.lblNCards.Text = "(" + Me.GetNCards(clsModule.CgSCollection).ToString + " cartes attachées)"
			End If
			Call Me.RecurFormatTitle(VpNode)
			Me.tvwExplore.Nodes.Add(VpNode)
			VpNode.Expand
			Me.mnuCardsFR.Enabled = True
		End If
		Me.VmMustReload = False
		'Restauration des paramètres langue / tri (NB. Si on est en VO on est toujours en ordre alphabétique)
		If Me.mnuCardsFR.Checked Then
			Me.tvwExplore.BeginUpdate
			Call Me.ChangeLanguage(Me.tvwExplore.Nodes.Item(0))
			Me.tvwExplore.EndUpdate
		End If
		If Me.mnuSort.Tag = True Then
			Call Me.SortTvw
		End If
	End Sub
	Private Sub RecurLoadTvw(VpSource1 As String, VpSource2 As String, VpNode As TreeNode, VpRecurLevel As Integer)
	'---------------------------------------------------------------------------------------------------------------------
	'Méthode de chargement récursive du treeview : à chaque niveau i, sélectionne les cartes correspondant au ième critère
	'remplissant également les critères de la branche courante de i-1 jusqu'à 1
	'---------------------------------------------------------------------------------------------------------------------	
	Dim VpChild As TreeNode				'Enfant du noeud courant
	Dim VpParent As TreeNode = VpNode	'Ancêtres du noeud courant
	Dim VpSQL As String					'Requête construite adaptativement
	Dim VpStr As String
		'Lorsque le niveau de récursivité courant dépasse le nombre de critères sélectionnés, c'est que l'arborescence est complète
		If VpRecurLevel > Me.chkClassement.CheckedItems.Count Then Exit Sub
		'La requête s'effectue dans les deux tables Card et Spell mises en correspondances sur le nom de la carte, elles-mêmes mises en correspondance avec MyGames ou MyCollection sur le numéro encyclopédique
		If VpNode.Tag = "Card.Title" Then
			VpSQL = "Select Distinct Card.Title, Spell.Color, CardFR.TitleFR From ((" + VpSource1 + " Inner Join Card On " + VpSource2 + ".EncNbr = Card.EncNbr) Inner Join Spell On Card.Title = Spell.Title) Inner Join CardFR On CardFR.EncNbr = Card.EncNbr Where "		
		Else
			VpSQL = "Select Distinct " + VpNode.Tag + " From (" + VpSource1 + " Inner Join Card On " + VpSource2 + ".EncNbr = Card.EncNbr) Inner Join Spell On Card.Title = Spell.Title Where "		
		End If	
		'Ajoute les conditions sur les identifiants des jeux
		VpSQL = VpSQL + Me.Restriction
		'Ajoute les conditions sur les critères des ancêtres
		While Not VpParent.Parent Is Nothing						
			VpSQL = VpSQL + VpParent.Parent.Tag + " = '" + VpParent.Text + "' And "
			VpParent = VpParent.Parent
		End While
		'Suppression des mots-clés inutiles
		VpSQL = clsModule.TrimQuery(VpSQL)
		'Exécution de la requête
		VgDBCommand.CommandText = VpSQL
		VgDBReader = VgDBcommand.ExecuteReader
		With VgDBReader
			While .Read
				'Ajoute un enfant par enregistrement trouvé
				VpStr = .GetValue(0).ToString
				If VpStr.Trim <> "" Then
					VpChild = New TreeNode(VpStr)
					'Si on est au niveau du nom des cartes, l'icône est celle de la couleur de la carte
					If VpNode.Tag = "Card.Title" Then
						VpChild.ImageIndex = Me.FindImageIndex(VpNode.Tag, .GetValue(1).ToString)
					'Sinon l'icône est celle du critère associé
					Else
						VpChild.ImageIndex = Me.FindImageIndex(VpNode.Tag, VpChild.Text)
					End If
					'Icône identique lorsque l'élément est mis en surbrillance
					VpChild.SelectedImageIndex = VpChild.ImageIndex
					'Tant que l'on est pas au dernier niveau, on tag les éléments avec le critère requis pour leur descendance
					If VpRecurLevel < Me.chkClassement.CheckedItems.Count Then
						VpChild.Tag = clsModule.CgCriteres.Item(Me.chkClassement.CheckedItems(VpRecurLevel))
					'Si on est au niveau du nom des cartes, le tag est celui de sa traduction (Title = VO, Tag = VF)
					ElseIf VpNode.Tag = "Card.Title"
						VpChild.Tag = .GetValue(2).ToString
					End If	
					'Ajout effectif
					VpNode.Nodes.Add(VpChild)
				End If
			End While
			.Close
		End With
		'Appel récursif sur chaque enfant au niveau supérieur
		For Each VpChild In VpNode.Nodes
			Call Me.RecurLoadTvw(VpSource1, VpSource2, VpChild, VpRecurLevel + 1)
		Next VpChild
	End Sub
	Public Function IsInAdvSearch As Boolean
	'--------------------------------------------------------------------------------
	'Retourne si l'on est actuellement en affichage de résultats de recherche avancée
	'--------------------------------------------------------------------------------
		Return ( VmAdvSearch <> "" )
	End Function
	Public Function IsSourcePresent As Boolean
	'--------------------------------------------------------------
	'Vérifie qu'il y a bien au moins une source de cartes à traiter
	'--------------------------------------------------------------
		For Each VpItem As Object In Me.mnuDisp.Items
			If VpItem.Checked Then
				Return True
			End If
		Next VpItem
		Return False
	End Function
	Public Function Restriction(Optional VpTextMode As Boolean = False) As String
	'------------------------------------------------------------------------
	'Retourne une clause de restriction pour n'afficher que les jeux demandés
	'------------------------------------------------------------------------
	Dim VpStr As String = ""
		For Each VpItem As Object In Me.mnuDisp.Items
			If VpItem.Checked Then
				If VpItem.Text = clsModule.CgCollection Then
					Return IIf(VpTextMode, clsModule.CgCollection, "")
				Else
					VpStr = VpStr + IIf(VpTextMode, VpItem.Text + " ", "MyGames.GameID = " + VgOptions.GetDeckIndex(VpItem.Text) + " Or ")
				End If
			End If
		Next VpItem
		'Retourne la restriction de manière textuelle (non pour une utilisation dans une requête)
		If VpTextMode Then
			Return VpStr
		'Retourne la clause de restriction en mode SQL
		ElseIf VpStr.Length > 4 Then
			Return "(" + VpStr.Substring(0, VpStr.Length - 4) + ") And "
		'Cas où aucune source n'est sélectionnée
		Else
			Return "MyGames.GameID = -1"
		End If
	End Function
	Public Function IsSingleSource As Boolean
	'----------------------------------------------------
	'Retourne si une seule source unique est sélectionnée	
	'----------------------------------------------------
	Dim VpNSources As Integer = 0
		For Each VpItem As Object In Me.mnuDisp.Items
			If VpItem.Checked Then
				VpNSources = VpNSources + 1
			End If
		Next VpItem
		Return ( VpNSources = 1 )
	End Function
	Public Function GetSelectedSource As String
	'--------------------------------------------------
	'Retourne le nom de la première source sélectionnée
	'--------------------------------------------------
		For Each VpItem As Object In Me.mnuDisp.Items
			If VpItem.Checked Then
				Return VpItem.Text
			End If
		Next VpItem	
		Return ""
	End Function
	Private Sub ChangeLanguage(VpNode As TreeNode)
	'-------------------------------------
	'Commute la langue du titre des cartes
	'-------------------------------------
	Dim VpChild As TreeNode
	Dim VpStr As String
		If VpNode.Tag = "Card.Title" Then
			For Each VpChild In VpNode.Nodes
				VpStr = VpChild.Text
				VpChild.Text = VpChild.Tag
				VpChild.Tag = VpStr
			Next VpChild
		Else
			For Each VpChild In VpNode.Nodes
				Call Me.ChangeLanguage(VpChild)
			Next VpChild
		End If	
	End Sub
	Private Sub RecurFormatTitle(VpNode As TreeNode)
	'----------------------------------------------------------
	'Modifie les captions des noeuds en un titre plus explicite
	'----------------------------------------------------------
		If Not VpNode.Parent Is Nothing Then
			VpNode.Text = clsModule.FormatTitle(VpNode.Parent.Tag, VpNode.Text)
		End If
		For Each VpChild As TreeNode In VpNode.Nodes
			Call Me.RecurFormatTitle(VpChild)
		Next VpChild
	End Sub
	Private Function FindImageIndex(VpTag As String, VpStr As String) As Integer
	'-------------------------------------------------------------------------
	'Retourne les numéros d'icônes à utiliser comme symboles dans le treeview)
	'-------------------------------------------------------------------------
		Select Case VpTag
			Case "Card.Type"
			Case "Card.Series"
				Return Me.imglstTvw.Images.IndexOfKey("_e" + VpStr + CgIconsExt)
			Case "Spell.Color", "Card.Title"
				Return FindImageIndexColor(VpStr)
			Case "Spell.myCost"
				If VpStr.Trim <> "" Then
					Return (12 + CInt(VpStr))
				Else
					Return 11
				End If
			Case Else
				Return 0
		End Select	
	End Function
	Private Function FindImageIndexColor(VpColor As String) As Integer
		Select Case VpColor
			Case "A"
				Return 4
			Case "B"
				Return 5
			Case "G"
				Return 7
			Case "L"
				Return 8
			Case "M"
				Return 9
			Case "R"
				Return 10
			Case "U"
				Return 6
			Case "W"
				Return 3
			Case Else
				Return 0
		End Select		
	End Function	
	#End Region
	#Region "Evènements"
	Sub MnuExitActivate(ByVal sender As Object, ByVal e As EventArgs)
		If Not VgDB Is Nothing Then			
			VgDB.Close
			VgDB.Dispose
		End If
		'Mémorisation des paramètres
		If VgOptions.VgSettings.RestoreSize Then
			VgOptions.VgSettings.RestoredWidth = Me.Width
			VgOptions.VgSettings.RestoredHeight = Me.Height
			VgOptions.VgSettings.RestoredState = Me.WindowState
			Call VgOptions.SaveSettings
		End If		
		Application.Exit		
	End Sub	
	Sub MnuDBSelectActivate(ByVal sender As Object, ByVal e As EventArgs)
		If Me.dlgOpen.ShowDialog <> System.Windows.Forms.DialogResult.Cancel Then
			If clsModule.DBOpen(Me.dlgOpen.FileName) Then
				Me.lblDB.Text = "Base - " + VgDB.DataSource
				Call Me.LoadMnu
				Call Me.LoadTvw
			End If
		End If
	End Sub	
	Sub MnuAddCardsActivate(ByVal sender As Object, ByVal e As EventArgs)
	Dim VpAddCards As frmAddCards
		If clsModule.DBOK Then
			VpAddCards = New frmAddCards(Me)
			VpAddCards.Show		
		End If
	End Sub	
	Sub MnuPrefsActivate(ByVal sender As Object, ByVal e As EventArgs)
		VgOptions.ShowDialog	
		Me.picScanCard.SizeMode = VgOptions.VgSettings.ImageMode
	End Sub	
	Sub MainFormLoad(ByVal sender As Object, ByVal e As EventArgs)
	'----------------------------------
	'Chargement du formulaire principal
	'----------------------------------
		'Chargement des options
		Call VgOptions.LoadSettings
		'Taille par défaut
		If VgOptions.VgSettings.RestoreSize Then
			Me.Size = New Size(VgOptions.VgSettings.RestoredWidth, VgOptions.VgSettings.RestoredHeight)
			Me.WindowState = VgOptions.VgSettings.RestoredState
		End If		
		'Panneau des images
		Me.picScanCard.SizeMode = VgOptions.VgSettings.ImageMode
		If VgOptions.VgSettings.AutoHideImage Then
			Call Me.MnuShowImageActivate(sender, e)
		End If
		'Chargement de la base par défaut
		If clsModule.LoadIcons(Me.imglstTvw) Then
			Call Me.ValidateCriteria
			If VgOptions.VgSettings.DefaultBase <> "" Then
				If clsModule.DBOpen(VgOptions.VgSettings.DefaultBase) Then
					Call Me.LoadMnu
					Call Me.LoadTvw
				End If			
			End If
		Else
			Application.Exit
		End If
		'MAJ auto
		If VgOptions.VgSettings.CheckForUpdate Then
			clsModule.VgTimer.Start
		End If	
		'Divers
		Me.mnuSort.Tag = False
	End Sub	
	Private Sub ManageDelete(VpSender As Object, VpSQL As String, Optional VpAlternateCaption As String = "")
	Dim VpQuestion As String
		If clsModule.DBOK Then
			If VpAlternateCaption = "" Then
				VpQuestion = "Êtes-vous sûr de vouloir supprimer de manière permanente l'ensemble des cartes saisies dans " + VpSender.Text + " ?"
			Else
				VpQuestion = VpAlternateCaption
			End If			
			If clsModule.ShowQuestion(VpQuestion) = System.Windows.Forms.DialogResult.Yes Then
				VgDBCommand.CommandText = VpSQL
				VgDBCommand.ExecuteNonQuery
			End If		
		End If
	End Sub
	Private Sub ManageOrder(VpX As Integer, VpY As Integer, VpZ As Integer)
	Dim VpIndex As Integer = Me.chkClassement.SelectedIndex
	Dim VpChecked As Boolean = Me.chkClassement.GetItemChecked(VpIndex)
		Me.chkClassement.Items.Insert(VpIndex + VpZ, Me.chkClassement.SelectedItem)
		Me.chkClassement.Items.RemoveAt(VpIndex + VpX + VpY)
		Me.chkClassement.SetItemChecked(VpIndex - VpX, VpChecked)
		Me.chkClassement.SelectedIndex = VpIndex - Vpx		
	End Sub
	Sub MnuRemCollecActivate(ByVal sender As Object, ByVal e As EventArgs)
		Call Me.ManageDelete(sender, "Delete * From MyCollection")
	End Sub
	Sub MnuRemSubGamesActivate(ByVal sender As Object, ByVal e As EventArgs)
		Call Me.ManageDelete(sender, "Delete * From MyGames Where GameID = " + VgOptions.GetDeckIndex(sender.Text))
	End Sub	
	Sub MnuRemScoresActivate(ByVal sender As Object, ByVal e As EventArgs)
		Call Me.ManageDelete(sender, "Delete * From MyScores", "Êtes-vous sûr de vouloir supprimer de manière permanente l'ensemble des scores comptés jusqu'à présent ?")
	End Sub	
	Sub MnuFixCollecActivate(ByVal sender As Object, ByVal e As EventArgs)
		VgDBCommand.CommandText = "Delete * From MyCollection Where Items = 0"
		VgDBCommand.ExecuteNonQuery		
		Call Me.BtRefreshActivate(sender, e)
	End Sub	
	Sub MnuFixSubGamesActivate(ByVal sender As Object, ByVal e As EventArgs)
		VgDBCommand.CommandText = "Delete * From MyGames Where Items = 0 And GameID = " + VgOptions.GetDeckIndex(sender.Text)
		VgDBCommand.ExecuteNonQuery			
		Call Me.BtRefreshActivate(sender, e)
	End Sub	
	Sub MnuFixSerieActivate(ByVal sender As Object, ByVal e As EventArgs)
		Call Me.FixSerie(sender.Text)
	End Sub
	Sub MnuAboutActivate(ByVal sender As Object, ByVal e As EventArgs)
	Dim VpAbout As New About
		VpAbout.ShowDialog
	End Sub	
	Sub MnuDispCollectionActivate(ByVal sender As Object, ByVal e As EventArgs)
	'---------------------------------------------
	'Changement de la sélection active d'affichage
	'---------------------------------------------
		'Si l'utilisateur a cliqué sur "Collection"
		If sender.Text = clsModule.CgCollection Then
			For Each VpItem As Object In Me.mnuDisp.Items
				'On sélectionne la collection
				If VpItem.Text = clsModule.CgCollection Then
					VpItem.Checked = True
				ElseIf VpItem.Text = clsModule.CgRefresh
				'mais on déselectionne tous les decks
				Else
					VpItem.Checked = False
				End If
			Next VpItem		
			Me.chkClassement.SetItemChecked(0, False)
		'Si l'utilisateur a cliqué sur un deck
		Else
			'On inverse son état de sélection	
			sender.Checked = Not sender.Checked
			'et on déselectionne la collection
			For Each VpItem As Object In Me.mnuDisp.Items
				If VpItem.Text = clsModule.CgCollection Then
					VpItem.Checked = False
				End If				
			Next VpItem
			Me.chkClassement.SetItemChecked(0, True)
		End If
		If Not e Is Nothing Then
			Call Me.LoadTvw
		End If
	End Sub	
	Sub MnuRefreshActivate(ByVal sender As Object, ByVal e As EventArgs)		
		Call Me.LoadTvw
	End Sub	
	Sub BtUpActivate(ByVal sender As Object, ByVal e As EventArgs)
		Call Me.ManageOrder(1, 0, -1)
	End Sub	
	Sub BtDownActivate(ByVal sender As Object, ByVal e As EventArgs)
		Call Me.ManageOrder(-1, 1, 2)
	End Sub
	Sub ChkClassementSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
		Me.btUp.Enabled = (Me.chkClassement.SelectedIndex > 1 And Me.chkClassement.SelectedIndex <> Me.chkClassement.Items.Count - 1)
		Me.btDown.Enabled = (Me.chkClassement.SelectedIndex > 0 And Me.chkClassement.SelectedIndex < Me.chkClassement.Items.Count - 2)
		If Me.VmMustReload Then
			Call Me.LoadTvw
		End If
	End Sub	
	Sub ChkClassementItemCheck(ByVal sender As Object, ByVal e As ItemCheckEventArgs)
		If Me.chkClassement.SelectedIndex = 0 Then			
			For Each VpItem As Object In Me.mnuDisp.Items
				If VpItem.Text = clsModule.CgCollection Then
					VpItem.Checked = Not ( e.NewValue = CheckState.Checked )
				ElseIf VpItem.Text = clsModule.CgRefresh Or VpItem.Text = clsModule.CgPanel Then
				Else
					VpItem.Checked = ( e.NewValue = CheckState.Checked )					
				End If				
			Next VpItem	
			Me.chkClassement.SelectedItems.Clear
			Me.VmMustReload = True	'un peu crade mais l'appel direct à LoadTvw est impossible car les checkboxes ne sont mises à jour qu'à la fin du présent évènement
		End If		
	End Sub	
	Sub MnuCardsFRActivate(ByVal sender As Object, ByVal e As MouseEventArgs)
		Me.mnuCardsFR.Checked = Not Me.mnuCardsFR.Checked
		Me.mnuSort.Tag = False
		Me.tvwExplore.BeginUpdate
		Call Me.ChangeLanguage(Me.tvwExplore.Nodes.Item(0))
		Me.tvwExplore.EndUpdate
	End Sub	
	Sub TvwExploreMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs)
	Dim VpNode As TreeNode = Me.tvwExplore.GetNodeAt(e.Location)
	Dim VpEN As Boolean
	Dim VpSingle As Boolean = Me.IsSingleSource
		If e.Button = System.Windows.Forms.MouseButtons.Right Then
			If Not VpNode Is Nothing AndAlso Not VpNode.Parent Is Nothing Then
				VpEN = ( VpNode.Parent.Tag = "Card.Title" )
			End If
			Me.mnuDeleteACard.Enabled = VpEN And VpSingle And Not Me.IsInAdvSearch
			Me.mnuMoveACard.Enabled = VpEN And VpSingle And Not Me.IsInAdvSearch
			Me.mnuBuy.Enabled = VpEN
			Me.cmnuTvw.Show(Me.tvwExplore, e.Location)
			Application.DoEvents
			Me.tvwExplore.SelectedNode = VpNode
		End If
	End Sub	
	Sub TvwExploreAfterSelect(ByVal sender As Object, ByVal e As TreeViewEventArgs)
	Dim VpTitle As String
		If Not e.Node.Parent Is Nothing Then
			If e.Node.Parent.Tag = "Card.Title" Then
				VpTitle = IIf(Me.mnuCardsFR.Checked, e.Node.Tag, e.Node.Text)
				Me.SuspendLayout
				If Me.IsInAdvSearch Then
					Call clsModule.LoadCarac(Me, Me, VpTitle)
				ElseIf Me.chkClassement.GetItemChecked(0) Then
					Call clsModule.LoadCarac(Me, Me, VpTitle, clsModule.CgSDecks)
				Else
					Call clsModule.LoadCarac(Me, Me, VpTitle, clsModule.CgSCollection)
				End If
				Call clsModule.LoadScanCard(VpTitle, Me.picScanCard)
				Me.ResumeLayout
			End If
		End If
	End Sub	
	Sub BtRefreshActivate(ByVal sender As Object, ByVal e As EventArgs)
		Call Me.MnuRefreshActivate(sender, e)
	End Sub	
	Sub CboEditionSelectedValueChanged(ByVal sender As Object, ByVal e As EventArgs)
	Dim VpTitle As String = IIf(Me.mnuCardsFR.Checked, Me.tvwExplore.SelectedNode.Tag, Me.tvwExplore.SelectedNode.Text)
		Me.SuspendLayout
		If Me.IsInAdvSearch Then
			Call clsModule.LoadCarac(Me, Me, VpTitle, , Me.cboEdition.Text)
		ElseIf Me.chkClassement.GetItemChecked(0) Then
			Call clsModule.LoadCarac(Me, Me, VpTitle, clsModule.CgSDecks, Me.cboEdition.Text)
		Else
			Call clsModule.LoadCarac(Me, Me, VpTitle, clsModule.CgSCollection, Me.cboEdition.Text)
		End If		
		Me.ResumeLayout
	End Sub	
	Sub MnuSearchCardClick(ByVal sender As Object, ByVal e As EventArgs)
		Me.mnuSearchText.Focus
		Me.mnuSearchText.SelectionStart = 0
		Me.mnuSearchText.SelectionLength = Me.mnuSearchText.Text.Length
	End Sub	
	Sub MnuSearchTextKeyUp(ByVal sender As Object, ByVal e As KeyEventArgs)
		If e.KeyCode = 13 Then
			Me.cmnuTvw.Close(ToolStripDropDownCloseReason.Keyboard)
			Call Me.GoFind
		End If
	End Sub	
	Sub MnuFindNextClick(ByVal sender As Object, ByVal e As EventArgs)
		Call Me.FindNextCard
	End Sub
	Sub TvwExploreKeyUp(ByVal sender As Object, ByVal e As KeyEventArgs)
		If e.KeyCode = Keys.F3 And Me.mnuFindNext.Enabled Then
			Call Me.FindNextCard
		End If
	End Sub	
	Sub MnuUpdateSimuActivate(ByVal sender As Object, ByVal e As EventArgs)
		If clsModule.DBOK Then
			If clsModule.ShowQuestion("Se connecter à Internet pour récupérer les derniers modèles de simulation ?") = System.Windows.Forms.DialogResult.Yes Then
				Call clsModule.DownloadNow(New Uri(clsModule.CgURL3B), clsModule.CgUpDDBb)
				If File.Exists(Application.StartupPath + clsModule.CgUpDDBb) Then
					Call clsModule.DBImport(Application.StartupPath + clsModule.CgUpDDBb)
				Else
					Call clsModule.ShowWarning(clsModule.CgDL3b) 
				End If				
			End If
		End If
	End Sub	
	Sub MnuUpdatePricesActivate(ByVal sender As Object, ByVal e As EventArgs)
		If clsModule.DBOK Then
			If clsModule.ShowQuestion("Se connecter à Internet pour récupérer la dernière liste des prix ?" + vbCrLf + "Cliquer sur 'Non' pour mettre à jour depuis un fichier sur le disque dur...") = System.Windows.Forms.DialogResult.Yes Then
				Call clsModule.DownloadNow(New Uri(clsModule.CgURL9), clsModule.CgUpPrices)
				If File.Exists(Application.StartupPath + clsModule.CgUpPrices) Then
					Call Me.UpdatePrices(Application.StartupPath + clsModule.CgUpPrices)
					Call clsModule.SecureDelete(Application.StartupPath + clsModule.CgUpPrices)
				Else
					Call clsModule.ShowWarning(clsModule.CgDL3b) 
				End If				
			Else
				Me.dlgOpen2.ShowDialog
				If Me.dlgOpen2.FileName.Trim <> "" Then
					Call Me.UpdatePrices(Me.dlgOpen2.FileName)
				End If
			End If
		End If		
	End Sub	
	Sub MnuUpdatePicturesActivate(ByVal sender As Object, ByVal e As EventArgs)
	Dim VpStr As String
		If clsModule.DBOK Then
			If File.Exists(VgOptions.VgSettings.PicturesFile) Then
				If (New FileInfo(VgOptions.VgSettings.PicturesFile)).Length < clsModule.CgImgMinLength Then
					If clsModule.ShowQuestion("La base d'images semble être corrompue." + vbCrLf + "Voulez-vous la re-télécharger maintenant (~300 Mo) ?") = System.Windows.Forms.DialogResult.Yes Then
						'Re-téléchargement complet de la base principale
						Call clsModule.DownloadUpdate(New Uri(clsModule.CgURL10 + clsModule.CgUpDDBd), VgOptions.VgSettings.PicturesFile, False)
					Else
						Exit Sub
					End If
				End If				
				If clsModule.ShowQuestion("Se connecter à Internet pour récupérer les dernières images ?" + vbCrLf + "Cliquer sur 'Non' pour mettre à jour depuis un fichier sur le disque dur...") = System.Windows.Forms.DialogResult.Yes Then
					Call clsModule.CheckForPicUpdates
				Else
					Me.dlgOpen3.ShowDialog
					VpStr = Me.dlgOpen3.FileName.Trim
					If VpStr <> "" Then
						If clsModule.ShowQuestion("Les images des cartes vont être mises à jour avec les données suivantes :" + vbCrLf + VpStr + vbCrLf + "L'opération pourra durer plusieurs secondes, patienter jusqu'à la notification... Continuer ?") = System.Windows.Forms.DialogResult.Yes Then
							VpStr = VpStr.Substring(0, VpStr.Length - 4) + ".log"
							If File.Exists(VpStr) Then
								Call Me.UpdatePictures(Me.dlgOpen3.FileName, VpStr)
							Else
								Call clsModule.ShowWarning("Impossible de trouver le fichier journal accompagnateur...")
							End If
						End If
					End If
				End If
			Else
				If clsModule.ShowQuestion("La base d'images est introuvable." + vbCrLf + "Voulez-vous la télécharger maintenant (~300 Mo) ?") = System.Windows.Forms.DialogResult.Yes Then
					'Téléchargement complet de la base principale
					Call clsModule.DownloadUpdate(New Uri(clsModule.CgURL10 + clsModule.CgUpDDBd), VgOptions.VgSettings.PicturesFile, False)
				End If				
			End If
		End If
	End Sub	
	Sub MainFormFormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs)
		Call Me.MnuExitActivate(sender, e)		
	End Sub	
	Sub MnuStatsActivate(ByVal sender As Object, ByVal e As EventArgs)
	Dim VpStats As New frmStats(Me)
		If clsModule.DBOK Then 
			If Me.GetNCards(IIf(Me.chkClassement.GetItemChecked(0), clsModule.CgSDecks, clsModule.CgSCollection)) > 1 Then
				VpStats.Show
			Else
				Call clsModule.ShowWarning("Il faut au minimum une sélection de 2 cartes pour pouvoir lancer l'affichage des statistiques...")
			End If
		End If
	End Sub
	Sub MnuHelpActivate(ByVal sender As Object, ByVal e As EventArgs)
		Call clsModule.ShowWarning("L'aide n'est pas encore rédigée !" + vbCrlf + vbCrLf + "Sachez cependant que :" + vbCrLf + "- la base de données des images des cartes est disponible sur http://couitchy.free.fr/upload/MTGM/Images des cartes/" + vbCrLf + "- les listes de prix sont téléchargeables sur http://couitchy.free.fr/upload/MTGM/Listes des prix/" + vbCrLf + "- notez que la traduction automatique des éditions ajoutées nécessite une libre connexion à Internet")
	End Sub
	Sub MnuSortClick(ByVal sender As Object, ByVal e As EventArgs)
		Call Me.SortTvw
	End Sub	
	Sub MnuStdSearchActivate(ByVal sender As Object, ByVal e As EventArgs)
	Dim VpStr As String
		If clsModule.DBOK Then
			VpStr = InputBox("Rechercher dans le titre des cartes présentes dans l'explorateur :", "Recherche", clsModule.CgCard)
			If VpStr.Trim <> "" Then
				Me.mnuSearchText.Text = VpStr
				Call Me.GoFind
			End If
		End If
	End Sub	
	Sub MnuAdvancedSearchActivate(ByVal sender As Object, ByVal e As EventArgs)
	Dim VpSearch As New frmSearch(Me)		
		If clsModule.DBOK Then
			VpSearch.Show
		End If		
	End Sub
	Sub MnuNewEditionActivate(ByVal sender As Object, ByVal e As EventArgs)
	Dim VpNewEdition As New frmNewEdition
		If clsModule.DBOK Then
			VpNewEdition.ShowDialog
		End If
	End Sub
	Sub MnuFixPricesActivate(ByVal sender As Object, ByVal e As EventArgs)
		If clsModule.DBOK Then
			Call Me.FixPrices		
		End If
	End Sub	
	Sub MnuTranslateActivate(ByVal sender As Object, ByVal e As EventArgs)
	Dim VpTranslator As New frmTranslate(Me)
		If clsModule.DBOK Then
			VpTranslator.ShowDialog
		End If		
	End Sub	
	Sub MnuFixFRActivate(ByVal sender As Object, ByVal e As EventArgs)
		If clsModule.DBOK Then
			Call Me.FixFR		
		End If		
	End Sub	
	Sub MnuCheckForUpdatesActivate(ByVal sender As Object, ByVal e As EventArgs)
		Call clsModule.CheckForUpdates(True)
	End Sub	
	Sub MnuPerfsActivate(ByVal sender As Object, ByVal e As EventArgs)
	Dim VpPerfs As frmPerfs
		If Me.mnuPerfs.Tag Is Nothing Then
			If clsModule.DBOK Then 
				VpPerfs = New frmPerfs(Me)
				VpPerfs.Show
				Me.mnuPerfs.Tag = clsModule.CgDummy
			End If
		End If
	End Sub
	Private Function ManageTransfert(VpNode As TreeNode, VpTransfertType As clsTransfertResult.EgTransfertType, Optional VpTo As String = "") As Boolean
	'------------------------------------------------------------------------------
	'Gère la suppression d'une carte ou son transfert dans un autre deck/collection
	'------------------------------------------------------------------------------
	Dim VpPreciseTransfert As frmTransfert
	Dim VpTransfertResult As New clsTransfertResult
	Dim VpCardName As String = IIf(Me.mnuCardsFR.Checked, VpNode.Tag.ToString, VpNode.Text)
	Dim VpSource As String = IIf(Me.chkClassement.GetItemChecked(0), clsModule.CgSDecks, clsModule.CgSCollection)
		'Gestion des cas multiples
		If Me.cboEdition.Items.Count > 1 Or CInt(Me.lblStock.Text) > 1 Then
			'VpPreciseTransfert = New frmTransfert(Me, Me.cboEdition.Items, VpCardName, VpSource, VpTransfertResult)
			VpPreciseTransfert = New frmTransfert(Me, VpCardName, VpSource, VpTransfertResult)
			VpPreciseTransfert.ShowDialog
		Else
			VpTransfertResult.NCartes = 1
			VpTransfertResult.IDSerie = Me.cboEdition.Text
		End If
		'Récupération du numéro encyclopédique de la carte concernée
		VpTransfertResult.EncNbr = clsModule.GetEncNbr(Me, VpSource, VpCardName, VpTransfertResult.IDSerie)
		'Type d'opération
		VpTransfertResult.TransfertType = VpTransfertType
		'Lieux des modifications
		VpTransfertResult.TFrom = Me.GetSelectedSource
		VpTransfertResult.SFrom = VpSource
		If VpTransfertType = clsTransfertResult.EgTransfertType.Move Then
			VpTransfertResult.TTo = VpTo
			VpTransfertResult.STo = IIf(VpTo = clsModule.CgCollection, clsModule.CgSCollection, clsModule.CgSDecks)
		End If		
		'Opération effective
		If VpTransfertResult.TFrom <> VpTransfertResult.TTo Then
			frmTransfert.CommitAction(VpTransfertResult)
			Return ( VpTransfertResult.NCartes <> 0 )
		ElseIf VpTransfertResult.NCartes <> 0 Then
			Call clsModule.ShowWarning("La source et la destination sont identiques !")
		End If
		Return False
	End Function
	Private Sub ManageMultipleTransferts(VpTransfertType As clsTransfertResult.EgTransfertType, Optional VpTo As String = "")
	'-------------------------------------------------------------------------------------------------------------
	'Gère les transferts / suppression en ajoutant le cas où plusieurs éléments sont sélectionnés dans le treeview
	'-------------------------------------------------------------------------------------------------------------
	Dim VpMustReload As Boolean = False
	Dim VpHistory As String
		For Each VpNode As TreeNode In Me.tvwExplore.SelectedNodes
			VpMustReload = VpMustReload Or Me.ManageTransfert(VpNode, VpTransfertType, VpTo)	'Si au moins un des cas multiples ne s'est pas terminé par une annulation, c'est qu'il faut recharger le treeview
		Next VpNode
		If VpMustReload Then
			VpHistory = Me.SaveNode(Me.tvwExplore.SelectedNode)
			Call Me.LoadTvw
			Call Me.RestoreNode(VpHistory, Me.tvwExplore.Nodes(0))
		End If		
	End Sub
	Sub MnuMoveACardActivate(ByVal sender As Object, ByVal e As EventArgs)
		Call Me.ManageMultipleTransferts(clsTransfertResult.EgTransfertType.Move, sender.Text)		
	End Sub	
	Sub MnuDeleteACardClick(ByVal sender As Object, ByVal e As EventArgs)
		Call Me.ManageMultipleTransferts(clsTransfertResult.EgTransfertType.Deletion)
	End Sub	
	Sub MnuExcelGenActivate(ByVal sender As Object, ByVal e As EventArgs)
		If clsModule.DBOK Then
			Call Me.ExcelGen(clsModule.ShowQuestion("Générer une liste simple ?" + vbCrLf + "Cliquer sur 'Non' pour générer une liste arborescente.") = System.Windows.Forms.DialogResult.Yes)
		End If
	End Sub
	Sub MnuRemEditionActivate(ByVal sender As Object, ByVal e As EventArgs)
	Dim VpDeletor As New frmDeleteEdition(Me)
		If clsModule.DBOK Then
			VpDeletor.Show
		End If			
	End Sub
	Sub MnuSimuActivate(ByVal sender As Object, ByVal e As EventArgs)
	Dim VpSimu As New frmSimu(Me)
		If clsModule.DBOK Then
			VpSimu.Show
		End If
	End Sub
	Sub MnuExportActivate(ByVal sender As Object, ByVal e As EventArgs)
	Dim VpExporter As New frmExport(Me)
		If clsModule.DBOK Then
			VpExporter.Show
		End If
	End Sub
	Sub MnuGestDecksActivate(ByVal sender As Object, ByVal e As EventArgs)
	Dim VpGestDecks As New frmGestDecks(Me)
		If clsModule.DBOK Then
			VpGestDecks.Show
		End If	
	End Sub
	Sub MnuShowImageActivate(ByVal sender As Object, ByVal e As EventArgs)
	Dim VpDistance As Integer = Me.splitV.SplitterDistance
	Static VpDistance2 As Integer
	Static VpWidth2 As Integer
		Me.SuspendLayout
		'Si le panneau est affiché, le masque
		If Not Me.splitV2.Panel2Collapsed Then
			VpDistance2 = Me.splitV2.SplitterDistance
			VpWidth2 = Me.splitV2.Panel2.Width
			Me.Width = Me.Width - VpWidth2
			Me.splitV2.Panel2Collapsed = True
		'Sinon l'affiche
		Else
			Me.splitV2.Panel2Collapsed = False
			Me.Width = Me.Width + VpWidth2
		End If
		Me.ResumeLayout
		Me.splitV.SplitterDistance = VpDistance
		If VpDistance2 <> 0 Then
			Me.splitV2.SplitterDistance = VpDistance2
		End If
	End Sub	
	Sub MnuCheckForBetasActivate(ByVal sender As Object, ByVal e As EventArgs)
		Call clsModule.CheckForUpdates(True, True)
	End Sub
	Sub MnuBuyClick(ByVal sender As Object, ByVal e As EventArgs)
	Dim VpBuy As frmBuyMV		
		If Me.mnuBuy.Tag Is Nothing OrElse Me.mnuBuy.Tag.IsDisposed Then
			VpBuy = New frmBuyMV
			Me.mnuBuy.Tag = VpBuy
		Else
			VpBuy = Me.mnuBuy.Tag
		End If
		VpBuy.Show
		VpBuy.BringToFront
		For Each VpNode As TreeNode In Me.tvwExplore.SelectedNodes
			VpBuy.AddToBasket(VpNode.Text)
		Next VpNode
		VpBuy.LoadGrid(clsModule.eBasketMode.Local)
	End Sub	
	#End Region
End Class
