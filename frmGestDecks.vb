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
'| Release 11     |                        24/01/2012 |
'| Release 12     |                        01/10/2012 |
'| Release 13     |                        09/05/2014 |
'| Release 14     |                        09/05/2015 |
'| Release 15     |                        15/01/2017 |
'| Auteur         |                          Couitchy |
'|----------------------------------------------------|
'| Modifications :                                    |
'| - gestion de la suppression			   09/01/2009 |
'| - renvoi cartes vers collection		   29/08/2010 |
'| - gestion suppressions multiples		   09/05/2011 |
'| - gestion d'infos personnalisées		   08/04/2012 |
'| - gestion d'une arborescence de decks   06/06/2014 |
'------------------------------------------------------
Imports System.Collections.Generic
Public Partial Class frmGestDecks
	Private VmOwner As MainForm
	Private VmFormMove As Boolean = False	'Formulaire en déplacement
	Private VmMousePos As Point				'Position initiale de la souris sur la barre de titre
	Private VmCanClose As Boolean = False   'Formulaire peut être fermé
	Private VmMustReload As Boolean = False
	Public Sub New(VpOwner As MainForm)
		Me.InitializeComponent()
		VmOwner = VpOwner
	End Sub
	Private Sub LoadDecks
	'--------------------------------------------
	'Chargement de la liste des decks disponibles
	'--------------------------------------------
	Dim VpRoot As New TreeNode("Liste des decks", 0, 0)
		Me.tvwDecks.Nodes.Clear
		Me.cboFormat.Items.Clear
		VgDBCommand.CommandText = "Select Distinct GameFormat From MyGamesID Where IsFolder = False;"
		VgDBReader = VgDBCommand.ExecuteReader
		With VgDBReader
			While .Read
				Me.cboFormat.Items.Add(.GetString(0))
			End While
			.Close
		End With
		Me.btDown.Enabled = False
		Me.btUp.Enabled = False
		Me.btRename.Enabled = False
		Me.btRemove.Enabled = False
		VpRoot.Tag = New clsInfoNode(-1, True)
		Call Me.RecurLoadDecks(VpRoot, "Is Null")
		Me.tvwDecks.Nodes.Add(VpRoot)
		VpRoot.Expand
		If VpRoot.FirstNode IsNot Nothing Then
			Me.tvwDecks.SelectedNode = VpRoot.FirstNode
		End If
	End Sub
	Private Sub RecurLoadDecks(VpNode As TreeNode, VpParent As String)
	Dim VpCur As TreeNode
		For Each VpChild As Integer In clsModule.GetChildrenDecksIds(VpParent)
			VpCur = New TreeNode(clsModule.GetDeckNameFromId(VpChild))
			VpCur.Tag = New clsInfoNode(VpChild, clsModule.IsDeckFolder(VpChild))
			VpCur.ImageIndex = If(CType(VpCur.Tag, clsInfoNode).IsFolder, 1, 2)
			VpCur.SelectedImageIndex = VpCur.ImageIndex
			VpNode.Nodes.Add(VpCur)
			Call Me.RecurLoadDecks(VpCur, " = " + VpChild.ToString)
		Next VpChild
	End Sub
	Private Sub LoadDeckInfos
	'----------------------------------------------------------------
	'Charge les infos (date, format, description) du deck sélectionné
	'----------------------------------------------------------------
		If Me.IsDeckNode Then
			VgDBCommand.CommandText = "Select GameDate, GameFormat, GameDescription From MyGamesID Where GameName = '" + Me.tvwDecks.SelectedNode.Text.Replace("'", "''") + "' And IsFolder = False;"
			VgDBReader = VgDBCommand.ExecuteReader
			With VgDBReader
				If .Read Then
					Me.pickDate.Value = .GetDateTime(0)
					Me.cboFormat.Text = .GetString(1)
					Try
						Me.txtMemo.Text = .GetString(2)
					Catch
						Me.txtMemo.Text = ""
					End Try
				End If
				.Close
			End With
		End If
	End Sub
	Private Sub SaveDeckInfos
	'--------------------------------------------------------------------
	'Sauvegarde les infos (date, format, description) du deck sélectionné
	'--------------------------------------------------------------------
		If Me.IsDeckNode Then
			VgDBCommand.CommandText = "Update MyGamesID Set GameDate = '" + Me.pickDate.Value.ToShortDateString + "', GameFormat = '" + Me.cboFormat.Text.Replace("'", "''") + "', GameDescription = '" + Me.txtMemo.Text.Replace("'", "''") + "' Where GameName = '" + Me.tvwDecks.SelectedNode.Text.Replace("'", "''") + "';"
			VgDBCommand.ExecuteNonQuery
		End If
	End Sub
	Private Function DeckExists(VpName As String, VpNode As TreeNode) As Boolean
	'---------------------------------------------------------------------------------------
	'Parcourt récursif de l'arborescence pour savoir si un nom identique de deck existe déjà
	'---------------------------------------------------------------------------------------
		If VpNode.Text.ToLower = VpName.ToLower Then
			Return True
		Else
			For Each VpChild As TreeNode In VpNode.Nodes
				If Me.DeckExists(VpName, VpChild) Then
					Return True
				End If
			Next VpChild
			Return False
		End If
	End Function
	Private Function FolderExists(VpName As String, VpNode As TreeNode) As Boolean
	'--------------------------------------------------------------------------------------------------
	'Parcourt du niveau actuel de l'arborescence pour savoir si un nom identique de dossier existe déjà
	'--------------------------------------------------------------------------------------------------
		For Each VpChild As TreeNode In VpNode.Nodes
			If VpChild.Text.ToLower = VpName.ToLower Then
				Return True
			End If
		Next VpChild
		Return False
	End Function
	Private Function DeckOrFolderExists(VpName As String, VpNode As TreeNode, VpFolder As Boolean) As Boolean
		If VpFolder Then
			Return FolderExists(VpName, VpNode)
		Else
			Return DeckExists(VpName, VpNode)
		End If
	End Function
	Private Sub SwapDeckId2(VpTable As String, VpId1 As String, VpId2 As String)
	'----------------------------------------------------------------
	'Intervertit les identifiants des deux decks passés en paramètres
	'----------------------------------------------------------------
		VgDBCommand.CommandText = "Update " + VpTable + " Set GameID = -1 Where GameID = " + VpId1 + ";"
		VgDBCommand.ExecuteNonQuery
		VgDBCommand.CommandText = "Update " + VpTable + " Set Parent = -1 Where Parent = " + VpId1 + ";"
		VgDBCommand.ExecuteNonQuery
		VgDBCommand.CommandText = "Update " + VpTable + " Set GameID = " + VpId1 + " Where GameID = " + VpId2 + ";"
		VgDBCommand.ExecuteNonQuery
		VgDBCommand.CommandText = "Update " + VpTable + " Set Parent = " + VpId1 + " Where Parent = " + VpId2 + ";"
		VgDBCommand.ExecuteNonQuery
		VgDBCommand.CommandText = "Update " + VpTable + " Set GameID = " + VpId2 + " Where GameID = -1;"
		VgDBCommand.ExecuteNonQuery
		VgDBCommand.CommandText = "Update " + VpTable + " Set Parent = " + VpId2 + " Where Parent = -1;"
		VgDBCommand.ExecuteNonQuery
	End Sub
	Private Sub SwapDeckId1(VpTable As String, VpId1 As String, VpId2 As String)
		VgDBCommand.CommandText = "Update " + VpTable + " Set GameID = -1 Where GameID = " + VpId1 + ";"
		VgDBCommand.ExecuteNonQuery
		VgDBCommand.CommandText = "Update " + VpTable + " Set GameID = " + VpId1 + " Where GameID = " + VpId2 + ";"
		VgDBCommand.ExecuteNonQuery
		VgDBCommand.CommandText = "Update " + VpTable + " Set GameID = " + VpId2 + " Where GameID = -1;"
		VgDBCommand.ExecuteNonQuery
	End Sub
	Private Sub ManageSwap(VpNode1 As TreeNode, VpNode2 As TreeNode)
	Dim VpId1 As Integer = CType(VpNode1.Tag, clsInfoNode).Id
	Dim VpId2 As Integer = CType(VpNode2.Tag, clsInfoNode).Id
	Dim VpIdString1 As String = CType(VpNode1.Tag, clsInfoNode).IdString
	Dim VpIdString2 As String = CType(VpNode2.Tag, clsInfoNode).IdString
	Dim VpSelected As TreeNode = Me.tvwDecks.SelectedNode
	Dim VpParent As TreeNode = VpSelected.Parent
		Call Me.SwapDeckId2("MyGamesID", VpIdString1, VpIdString2)
		Call Me.SwapDeckId1("MyGames", VpIdString1, VpIdString2)
		Me.tvwDecks.BeginUpdate
		VpNode1.Tag = New clsInfoNode(VpId2, CType(VpNode1.Tag, clsInfoNode).IsFolder)
		VpNode2.Tag = New clsInfoNode(VpId1, CType(VpNode2.Tag, clsInfoNode).IsFolder)
		VpParent.Nodes.RemoveAt(VpNode2.Index)
		VpParent.Nodes.RemoveAt(VpNode1.Index)
		VpParent.Nodes.Insert(VpNode2.Index - 1, VpNode1)
		VpParent.Nodes.Insert(VpNode1.Index, VpNode2)
		Me.tvwDecks.EndUpdate
		VpSelected.EnsureVisible
		Me.tvwDecks.SelectedNode = VpSelected
		VmMustReload = True
	End Sub
	Private Sub AddDeckOrFolder(VpFolder As Boolean)
	Dim VpName As String
	Dim VpId As Integer
	Dim VpParent As TreeNode
	Dim VpNew As TreeNode
		If Me.tvwDecks.SelectedNode Is Nothing Then Exit Sub
		VpName = InputBox("Entrer le nom de l'élément :", "Nouvel élément", If(VpFolder, clsModule.CgDefaultFolderName, clsModule.CgDefaultDeckName))
		If VpName <> "" Then
			If VpName.Length > 50 Then VpName = VpName.Substring(0, 50)
			VpParent = Me.SelectedParent
			If Me.DeckOrFolderExists(VpName, If(VpFolder, VpParent, Me.RootNode), VpFolder) Then
				Call clsModule.ShowWarning("Un élément portant ce nom existe déjà...")
			Else
				VpId = clsModule.GetNewDeckId
				If VpFolder Then
					VgDBCommand.CommandText = "Insert Into MyGamesID(GameID, GameName, AdvID, Parent, IsFolder) Values (" + VpId.ToString + ", '" + VpName.Replace("'", "''") + "', 0, " + CType(VpParent.Tag, clsInfoNode).IdString + ", True);"
				Else
					VgDBCommand.CommandText = "Insert Into MyGamesID(GameID, GameName, AdvID, GameDate, GameFormat, GameDescription, Parent, IsFolder) Values (" + VpId.ToString + ", '" + VpName.Replace("'", "''") + "', 0, '" + Now.ToShortDateString + "', '" + clsModule.CgDefaultFormat + "', '', " + CType(VpParent.Tag, clsInfoNode).IdString + ", False);"
				End If
				VgDBCommand.ExecuteNonQuery
				VpNew = New TreeNode(VpName)
				VpNew.Tag = New clsInfoNode(VpId, VpFolder)
				VpNew.ImageIndex = If(VpFolder, 1, 2)
				VpNew.SelectedImageIndex = VpNew.ImageIndex
				VpParent.Nodes.Add(VpNew)
				VpNew.EnsureVisible
				Me.tvwDecks.SelectedNode = VpNew
				VmMustReload = True
			End If
		End If
	End Sub
	Private Function RemoveDeck(VpDeckName As String) As Boolean
	'-----------------------------------
	'Gestion de la suppression d'un deck
	'-----------------------------------
	Dim VpDeckId As Integer = clsModule.GetDeckIdFromName(VpDeckName)
	Dim VpQuestion As DialogResult = clsModule.ShowQuestion("Le deck " + VpDeckName + " va être supprimé." + vbCrLf + "Souhaitez-vous déplacer les cartes qu'il contenait vers la collection ?", MessageBoxButtons.YesNoCancel)
	Dim VpContenu As List(Of clsItemRecup)
	Dim VpO As Object
		If VpQuestion = System.Windows.Forms.DialogResult.Cancel Then Return False
		'Recopie avant suppression
		If VpQuestion = System.Windows.Forms.DialogResult.Yes Then
			'Récupération du contenu du deck
			VpContenu = New List(Of clsItemRecup)
			VgDBCommand.CommandText = "Select EncNbr, Items, Foil From MyGames Where GameID = " + VpDeckId.ToString + ";"
			VgDBReader = VgDBCommand.ExecuteReader
			With VgDBReader
				While .Read
					VpContenu.Add(New clsItemRecup(.GetInt32(0), .GetInt32(1), .GetBoolean(2)))
				End While
				.Close
			End With
			'Insertion ou mise à jour dans la collection
			For Each VpRecup As clsItemRecup In VpContenu
				VgDBCommand.CommandText = "Select Items From MyCollection Where EncNbr = " + VpRecup.EncNbr.ToString + " And Foil = " + VpRecup.Foil.ToString + ";"
				VpO = VgDBCommand.ExecuteScalar
				'Cas 1 : la carte a ajouté existe déjà => mise à jour de la quantité somme
				If Not VpO Is Nothing AndAlso CInt(VpO) > 0 Then
					VgDBCommand.CommandText = "Update MyCollection Set Items = " + (CInt(VpO) + VpRecup.Quant).ToString + " Where EncNbr = " + VpRecup.EncNbr.ToString + " And Foil = " + VpRecup.Foil.ToString + ";"
				'Cas 2 : nouvelle carte => insertion
				Else
					VgDBCommand.CommandText = "Insert Into MyCollection(EncNbr, Items, Foil) Values (" + VpRecup.EncNbr.ToString + ", " + VpRecup.Quant.ToString + ", " + VpRecup.Foil.ToString + ");"
				End If
				VgDBCommand.ExecuteNonQuery
			Next VpRecup
		End If
		VgDBCommand.CommandText = "Delete * From MyGames Where GameID = " + VpDeckId.ToString + ";"
		VgDBCommand.ExecuteNonQuery
		VgDBCommand.CommandText = "Delete * From MyGamesId Where GameID = " + VpDeckId.ToString + ";"
		VgDBCommand.ExecuteNonQuery
		Return True
	End Function
	Private Sub RemoveFolder(VpFolderId As String)
	'--------------------------------------
	'Gestion de la suppression d'un dossier
	'--------------------------------------
		VgDBCommand.CommandText = "Delete * From MyGamesId Where GameID = " + VpFolderId + ";"
		VgDBCommand.ExecuteNonQuery
	End Sub
	Sub CbarDecksManagerMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
		VmFormMove = True
		VmCanClose = True
		VmMousePos = New Point(e.X, e.Y)
	End Sub
	Sub CbarDecksManagerMouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
		If VmFormMove Then
			Me.Location = New Point(MousePosition.X - VmMousePos.X, MousePosition.Y - VmMousePos.Y)
		End If
	End Sub
	Sub CbarDecksManagerMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs)
		VmFormMove = False
	End Sub
	Sub CbarDecksManagerVisibleChanged(ByVal sender As Object, ByVal e As EventArgs)
		If VmCanClose Then
			Me.Close
		End If
	End Sub
	Sub FrmGestDecksLoad(ByVal sender As Object, ByVal e As EventArgs)
		Call Me.LoadDecks
		'Astuce horrible pour contourner un bug de mise à l'échelle automatique en fonction de la densité de pixels
		If Me.CreateGraphics().DpiX <> 96 Then
			Me.Width *= 2
			Me.Width *= 0.5
		End If
	End Sub
	Sub TvwDecksAfterSelect(sender As Object, e As TreeViewEventArgs)
		If e.Node Is Me.RootNode Then
			Me.btRemove.Enabled = False
			Me.btRename.Enabled = False
			Me.btUp.Enabled = False
			Me.btDown.Enabled = False
		Else
			Me.btRemove.Enabled = True
			Me.btRename.Enabled = True
			Me.btUp.Enabled = e.Node IsNot e.Node.Parent.FirstNode
			Me.btDown.Enabled = e.Node IsNot e.Node.Parent.LastNode
		End If
		Call Me.LoadDeckInfos
	End Sub
	Sub FrmGestDecksFormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs)
		If e.CloseReason = CloseReason.UserClosing Then
			Me.Visible = False
			If VmMustReload Then
				Call VmOwner.LoadMnu
				Call VmOwner.LoadTvw
			End If
		End If
	End Sub
	Sub BtAddFolderActivate(sender As Object, e As EventArgs)
		Call Me.AddDeckOrFolder(True)
	End Sub
	Sub BtAddActivate(sender As Object, e As EventArgs)
		Call Me.AddDeckOrFolder(False)
	End Sub
	Sub BtRemoveActivate(sender As Object, e As EventArgs)
	Dim VpParent As TreeNode = Me.tvwDecks.SelectedNode.Parent
	Dim VpFolder As Boolean = CType(Me.tvwDecks.SelectedNode.Tag, clsInfoNode).IsFolder
	Dim VpDeleted As Boolean = False
		If VpFolder Then
			If Me.tvwDecks.SelectedNode.Nodes.Count = 0 Then
				Call Me.RemoveFolder(CType(Me.tvwDecks.SelectedNode.Tag, clsInfoNode).IdString)
				VpDeleted = True
			Else
				Call clsModule.ShowWarning("Ce dossier n'est pas vide...")
			End If
		Else
			If Me.RemoveDeck(Me.tvwDecks.SelectedNode.Text) Then
				VpDeleted = True
			End If
		End If
		If VpDeleted Then
			Me.tvwDecks.SelectedNode.Remove
			VpParent.EnsureVisible
			Me.tvwDecks.SelectedNode = VpParent
			VmMustReload = True
			Call VmOwner.LoadMnu
		End If
	End Sub
	Sub BtRenameActivate(sender As Object, e As EventArgs)
	Dim VpName As String
	Dim VpOldName As String = Me.tvwDecks.SelectedNode.Text
	Dim VpFolder As Boolean = CType(Me.tvwDecks.SelectedNode.Tag, clsInfoNode).IsFolder
		VpName = InputBox("Entrer le nouveau nom de l'élément :", "Renommer un élément", VpOldName)
		If VpName <> "" Then
			If VpName.Length > 50 Then VpName = VpName.Substring(0, 50)
			VpName = VpName.Replace("'", "''")
			VpOldName = VpOldName.Replace("'", "''")
			If Me.DeckOrFolderExists(VpName, If(VpFolder, Me.tvwDecks.SelectedNode.Parent, Me.RootNode), VpFolder) Then
				Call clsModule.ShowWarning("Un élément portant ce nom existe déjà...")
			Else
				If Not VpFolder Then
					VgDBCommand.CommandText = "Select * From MyScores Where JeuLocal = '" + VpOldName + "' Or JeuAdverse = '" + VpOldName + "';"
					VgDBReader = VgDBCommand.ExecuteReader
					VgDBReader.Read
					If VgDBReader.HasRows Then
						VgDBReader.Close
						If clsModule.ShowQuestion("Ce deck est lié à des parties saisies dans le comptage Victoires / Défaites." + vbCrlf + "Renommer également le label des parties en question ?") = System.Windows.Forms.DialogResult.Yes Then
							VgDBCommand.CommandText = "Update MyScores Set JeuLocal = '" + VpName + "' Where JeuLocal = '" + VpOldName + "';"
							VgDBCommand.ExecuteNonQuery
							VgDBCommand.CommandText = "Update MyScores Set JeuAdverse = '" + VpName + "' Where JeuAdverse = '" + VpOldName + "';"
							VgDBCommand.ExecuteNonQuery
						End If
					Else
						VgDBReader.Close
					End If
				End If
				VgDBCommand.CommandText = "Update MyGamesID Set GameName = '" + VpName + "' Where GameID = " + CType(Me.tvwDecks.SelectedNode.Tag, clsInfoNode).IdString + ";"
				VgDBCommand.ExecuteNonQuery
				Me.tvwDecks.SelectedNode.Text = VpName.Replace("''", "'")
				VmMustReload = True
				Call VmOwner.LoadMnu
			End If
		End If
	End Sub
	Sub BtDownActivate(sender As Object, e As EventArgs)
		Call Me.ManageSwap(Me.tvwDecks.SelectedNode, Me.tvwDecks.SelectedNode.NextNode)
	End Sub
	Sub BtUpActivate(sender As Object, e As EventArgs)
		Call Me.ManageSwap(Me.tvwDecks.SelectedNode.PrevNode, Me.tvwDecks.SelectedNode)
	End Sub
	Sub PickDateLeave(sender As Object, e As EventArgs)
		Call Me.SaveDeckInfos
	End Sub
	Sub CboFormatLeave(sender As Object, e As EventArgs)
		Call Me.SaveDeckInfos
	End Sub
	Sub TxtMemoLeave(sender As Object, e As EventArgs)
		Call Me.SaveDeckInfos
	End Sub
	Sub TvwDecksDragDrop(sender As Object, e As DragEventArgs)
	Dim VpSource As TreeNode
	Dim VpSourceInfo As clsInfoNode
	Dim VpDest As TreeNode
	Dim VpDestInfo As clsInfoNode
	Dim VpPos As Integer
	Dim VpNode As TreeNode
		If e.Data.GetDataPresent("System.Windows.Forms.TreeNode", False) Then
			VpSource = CType(e.Data.GetData("System.Windows.Forms.TreeNode"), TreeNode)
			VpSourceInfo = CType(VpSource.Tag, clsInfoNode)
			VpDest = Me.tvwDecks.GetNodeAt(Me.tvwDecks.PointToClient(New Point(e.X, e.Y)))
			VpDestInfo = CType(VpDest.Tag, clsInfoNode)
			If VpDestInfo.IsFolder Then
				VgDBCommand.CommandText = "Update MyGamesID Set Parent = " + VpDestInfo.IdString + " Where GameID = " + VpSourceInfo.IdString + ";"
				VgDBCommand.ExecuteNonQuery
				'Détermine l'endroit où il faut insérer le noeud
				VpPos = 0
				VpNode = VpDest.FirstNode
				While VpNode IsNot Nothing
					If VpSourceInfo.Id > CType(VpNode.Tag, clsInfoNode).Id Then
						VpPos += 1
					Else
						Exit While
					End If
					VpNode = VpNode.NextNode
				End While
				VpNode = VpSource.Clone
				VpDest.Nodes.Insert(VpPos, VpNode)
				VpSource.Remove
				Me.tvwDecks.SelectedNode = VpNode
				VpNode.EnsureVisible
				VmMustReload = True
			End If
		End If
	End Sub
	Sub TvwDecksDragEnter(sender As Object, e As DragEventArgs)
		If e.Data.GetDataPresent("System.Windows.Forms.TreeNode", False) Then
			e.Effect = DragDropEffects.Move
		Else
			e.Effect = DragDropEffects.None
		End If
	End Sub
	Sub TvwDecksItemDrag(sender As Object, e As ItemDragEventArgs)
		If e.Item IsNot Me.RootNode Then
			Me.tvwDecks.SelectedNode = e.Item
			Me.tvwDecks.DoDragDrop(e.Item, DragDropEffects.Move)
		End If
	End Sub
	Sub TvwDecksDragOver(sender As Object, e As DragEventArgs)
	Dim VpPoint As Point = Me.tvwDecks.PointToClient(Cursor.Position)
		If VpPoint.Y + 20 > Me.tvwDecks.Height Then
			Call clsModule.SendMessageA(Me.tvwDecks.Handle, 277, CType(1, IntPtr), IntPtr.Zero)
		ElseIf VpPoint.Y < 20 Then
			Call clsModule.SendMessageA(Me.tvwDecks.Handle, 277, CType(0, IntPtr), IntPtr.Zero)
		End If
    End Sub
	Private ReadOnly Property IsDeckNode As Boolean
		Get
			Return ( Me.tvwDecks.SelectedNode IsNot Nothing And Not CType(Me.tvwDecks.SelectedNode.Tag, clsInfoNode).IsFolder )
		End Get
	End Property
	Private ReadOnly Property SelectedParent As TreeNode
		Get
			If CType(Me.tvwDecks.SelectedNode.Tag, clsInfoNode).IsFolder Then
				Return Me.tvwDecks.SelectedNode
			Else
				Return Me.tvwDecks.SelectedNode.Parent
			End If
		End Get
	End Property
	Private ReadOnly Property RootNode As TreeNode
		Get
			Return tvwDecks.Nodes(0)
		End Get
	End Property
End Class
Public Class clsItemRecup
	Private VmEncNbr As Long
	Private VmQuant As Integer
	Private VmFoil As Boolean
	Public Sub New(VpEncNbr As Long, VpQuant As Integer, VpFoil As Boolean)
		VmEncNbr = VpEncNbr
		VmQuant = VpQuant
		VmFoil = VpFoil
	End Sub
	Public ReadOnly Property EncNbr As Long
		Get
			Return VmEncNbr
		End Get
	End Property
	Public ReadOnly Property Quant As Integer
		Get
			Return VmQuant
		End Get
	End Property
	Public ReadOnly Property Foil As Boolean
		Get
			Return VmFoil
		End Get
	End Property
End Class
Public Class clsInfoNode
	Private VmId As Integer
	Private VmFolder As Boolean
	Public Sub New(VpId As Integer, VpFolder As Boolean)
		VmId = VpId
		VmFolder = VpFolder
	End Sub
	Public ReadOnly Property Id As Integer
		Get
			Return VmId
		End Get
	End Property
	Public ReadOnly Property IdString As String
		Get
			Return If(VmId = -1, "Null", VmId.ToString)
		End Get
	End Property
	Public ReadOnly Property IsFolder As Boolean
		Get
			Return VmFolder
		End Get
	End Property
End Class