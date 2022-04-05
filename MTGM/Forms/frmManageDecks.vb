Imports System.Collections.Generic
Public Partial Class frmManageDecks
    Private VmOwner As MainForm
    Private VmFormMove As Boolean = False   'Formulaire en déplacement
    Private VmMousePos As Point             'Position initiale de la souris sur la barre de titre
    Private VmCanClose As Boolean = False   'Formulaire peut être fermé
    Private VmMustReload As Boolean = False
    Public Sub New(VpOwner As MainForm)
        Call Me.InitializeComponent
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
        VpRoot.Tag = New clsNodeInfo(-1, True)
        Call Me.RecurLoadDecks(VpRoot, "Is Null")
        Me.tvwDecks.Nodes.Add(VpRoot)
        VpRoot.Expand
        If VpRoot.FirstNode IsNot Nothing Then
            Me.tvwDecks.SelectedNode = VpRoot.FirstNode
        End If
    End Sub
    Private Sub RecurLoadDecks(VpNode As TreeNode, VpParent As String)
    Dim VpCur As TreeNode
        For Each VpChild As Integer In mdlToolbox.GetChildrenDecksIds(VpParent)
            VpCur = New TreeNode(mdlToolbox.GetDeckNameFromId(VpChild))
            VpCur.Tag = New clsNodeInfo(VpChild, mdlToolbox.IsDeckFolder(VpChild))
            VpCur.ImageIndex = If(CType(VpCur.Tag, clsNodeInfo).IsFolder, 1, 2)
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
    '---------------------------------------------------------------
    'Intervertit les identifiants des deux decks passés en paramètre
    '---------------------------------------------------------------
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
    Dim VpId1 As Integer = CType(VpNode1.Tag, clsNodeInfo).Id
    Dim VpId2 As Integer = CType(VpNode2.Tag, clsNodeInfo).Id
    Dim VpIdString1 As String = CType(VpNode1.Tag, clsNodeInfo).IdString
    Dim VpIdString2 As String = CType(VpNode2.Tag, clsNodeInfo).IdString
    Dim VpSelected As TreeNode = Me.tvwDecks.SelectedNode
    Dim VpParent As TreeNode = VpSelected.Parent
        Call Me.SwapDeckId2("MyGamesID", VpIdString1, VpIdString2)
        Call Me.SwapDeckId1("MyGames", VpIdString1, VpIdString2)
        Me.tvwDecks.BeginUpdate
        VpNode1.Tag = New clsNodeInfo(VpId2, CType(VpNode1.Tag, clsNodeInfo).IsFolder)
        VpNode2.Tag = New clsNodeInfo(VpId1, CType(VpNode2.Tag, clsNodeInfo).IsFolder)
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
        VpName = InputBox("Entrer le nom de l'élément :", "Nouvel élément", If(VpFolder, mdlConstGlob.CgDefaultFolderName, mdlConstGlob.CgDefaultDeckName))
        If VpName <> "" Then
            If VpName.Length > 50 Then VpName = VpName.Substring(0, 50)
            VpParent = Me.SelectedParent
            If Me.DeckOrFolderExists(VpName, If(VpFolder, VpParent, Me.RootNode), VpFolder) Then
                Call mdlToolbox.ShowWarning("Un élément portant ce nom existe déjà...")
            Else
                VpId = mdlToolbox.GetNewDeckId
                If VpFolder Then
                    VgDBCommand.CommandText = "Insert Into MyGamesID(GameID, GameName, AdvID, Parent, IsFolder) Values (" + VpId.ToString + ", '" + VpName.Replace("'", "''") + "', 0, " + CType(VpParent.Tag, clsNodeInfo).IdString + ", True);"
                Else
                    VgDBCommand.CommandText = "Insert Into MyGamesID(GameID, GameName, AdvID, GameDate, GameFormat, GameDescription, Parent, IsFolder) Values (" + VpId.ToString + ", '" + VpName.Replace("'", "''") + "', 0, '" + Now.ToShortDateString + "', '" + mdlConstGlob.CgDefaultFormat + "', '', " + CType(VpParent.Tag, clsNodeInfo).IdString + ", False);"
                End If
                VgDBCommand.ExecuteNonQuery
                VpNew = New TreeNode(VpName)
                VpNew.Tag = New clsNodeInfo(VpId, VpFolder)
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
    Dim VpDeckId As Integer = mdlToolbox.GetDeckIdFromName(VpDeckName)
    Dim VpQuestion As DialogResult = mdlToolbox.ShowQuestion("Le deck " + VpDeckName + " va être supprimé." + vbCrLf + "Souhaitez-vous déplacer les cartes qu'il contenait vers la collection ?", MessageBoxButtons.YesNoCancel)
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
    Dim VpFolder As Boolean = CType(Me.tvwDecks.SelectedNode.Tag, clsNodeInfo).IsFolder
    Dim VpDeleted As Boolean = False
        If VpFolder Then
            If Me.tvwDecks.SelectedNode.Nodes.Count = 0 Then
                Call Me.RemoveFolder(CType(Me.tvwDecks.SelectedNode.Tag, clsNodeInfo).IdString)
                VpDeleted = True
            Else
                Call mdlToolbox.ShowWarning("Ce dossier n'est pas vide...")
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
    Dim VpFolder As Boolean = CType(Me.tvwDecks.SelectedNode.Tag, clsNodeInfo).IsFolder
        VpName = InputBox("Entrer le nouveau nom de l'élément :", "Renommer un élément", VpOldName)
        If VpName <> "" Then
            If VpName.Length > 50 Then VpName = VpName.Substring(0, 50)
            VpName = VpName.Replace("'", "''")
            VpOldName = VpOldName.Replace("'", "''")
            If Me.DeckOrFolderExists(VpName, If(VpFolder, Me.tvwDecks.SelectedNode.Parent, Me.RootNode), VpFolder) Then
                Call mdlToolbox.ShowWarning("Un élément portant ce nom existe déjà...")
            Else
                If Not VpFolder Then
                    VgDBCommand.CommandText = "Select * From MyScores Where JeuLocal = '" + VpOldName + "' Or JeuAdverse = '" + VpOldName + "';"
                    VgDBReader = VgDBCommand.ExecuteReader
                    VgDBReader.Read
                    If VgDBReader.HasRows Then
                        VgDBReader.Close
                        If mdlToolbox.ShowQuestion("Ce deck est lié à des parties saisies dans le comptage Victoires / Défaites." + vbCrlf + "Renommer également le label des parties en question ?") = System.Windows.Forms.DialogResult.Yes Then
                            VgDBCommand.CommandText = "Update MyScores Set JeuLocal = '" + VpName + "' Where JeuLocal = '" + VpOldName + "';"
                            VgDBCommand.ExecuteNonQuery
                            VgDBCommand.CommandText = "Update MyScores Set JeuAdverse = '" + VpName + "' Where JeuAdverse = '" + VpOldName + "';"
                            VgDBCommand.ExecuteNonQuery
                        End If
                    Else
                        VgDBReader.Close
                    End If
                End If
                VgDBCommand.CommandText = "Update MyGamesID Set GameName = '" + VpName + "' Where GameID = " + CType(Me.tvwDecks.SelectedNode.Tag, clsNodeInfo).IdString + ";"
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
    Dim VpSourceInfo As clsNodeInfo
    Dim VpDest As TreeNode
    Dim VpDestInfo As clsNodeInfo
    Dim VpPos As Integer
    Dim VpNode As TreeNode
        If e.Data.GetDataPresent("System.Windows.Forms.TreeNode", False) Then
            VpSource = CType(e.Data.GetData("System.Windows.Forms.TreeNode"), TreeNode)
            VpSourceInfo = CType(VpSource.Tag, clsNodeInfo)
            VpDest = Me.tvwDecks.GetNodeAt(Me.tvwDecks.PointToClient(New Point(e.X, e.Y)))
            VpDestInfo = CType(VpDest.Tag, clsNodeInfo)
            If VpDestInfo.IsFolder Then
                VgDBCommand.CommandText = "Update MyGamesID Set Parent = " + VpDestInfo.IdString + " Where GameID = " + VpSourceInfo.IdString + ";"
                VgDBCommand.ExecuteNonQuery
                'Détermine l'endroit où il faut insérer le noeud
                VpPos = 0
                VpNode = VpDest.FirstNode
                While VpNode IsNot Nothing
                    If VpSourceInfo.Id > CType(VpNode.Tag, clsNodeInfo).Id Then
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
            Call mdlToolbox.SendMessageA(Me.tvwDecks.Handle, 277, CType(1, IntPtr), IntPtr.Zero)
        ElseIf VpPoint.Y < 20 Then
            Call mdlToolbox.SendMessageA(Me.tvwDecks.Handle, 277, CType(0, IntPtr), IntPtr.Zero)
        End If
    End Sub
    Private ReadOnly Property IsDeckNode As Boolean
        Get
            Return ( Me.tvwDecks.SelectedNode IsNot Nothing And Not CType(Me.tvwDecks.SelectedNode.Tag, clsNodeInfo).IsFolder )
        End Get
    End Property
    Private ReadOnly Property SelectedParent As TreeNode
        Get
            If CType(Me.tvwDecks.SelectedNode.Tag, clsNodeInfo).IsFolder Then
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
