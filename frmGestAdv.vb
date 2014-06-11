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
'| Auteur         |                          Couitchy |
'|----------------------------------------------------|
'| Modifications :                                    |
'------------------------------------------------------
Public Partial Class frmGestAdv
	Public Sub New()
		Me.InitializeComponent()
	End Sub
	Private VmFormMove As Boolean = False	'Formulaire en déplacement
	Private VmMousePos As Point				'Position initiale de la souris sur la barre de titre
	Private VmCanClose As Boolean = False   'Formulaire peut être fermé
	Private Sub ReloadListes
	Dim VpOwner As String
	Dim VpItem As ListViewItem
		Me.lvwAdv.Items.Clear
		Me.cboOwner.Items.Clear
		Me.cboDeck.Items.Clear
		Me.cboOwner.Text = ""
		Me.cboDeck.Text = ""
		'Liste des propriétaires
		For VpI As Integer = 1 To clsModule.GetAdvCount
			VpOwner = clsModule.GetAdvName(VpI)
			VpItem = New ListViewItem(VpOwner)
			VpItem.SubItems.Add(clsModule.GetAdvDecksCount(clsModule.GetAdvId(VpOwner)).ToString)
			Me.lvwAdv.Items.Add(VpItem)
			Me.cboOwner.Items.Add(VpOwner)
		Next VpI
		'Liste des decks
		For VpI As Integer = 1 To clsModule.GetDeckCount
			Me.cboDeck.Items.Add(clsModule.GetDeckNameFromIndex(VpI))
		Next VpI
	End Sub
	Private Sub CbarAdvManagerMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
		VmFormMove = True
		VmMousePos = New Point(e.X, e.Y)
	End Sub
	Private Sub CbarAdvManagerMouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
		If VmFormMove Then
			Me.Location = New Point(MousePosition.X - VmMousePos.X, MousePosition.Y - VmMousePos.Y)
		End If
	End Sub
	Private Sub CbarAdvManagerMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs)
		VmFormMove = False
	End Sub
	Private Sub CbarAdvManagerVisibleChanged(ByVal sender As Object, ByVal e As EventArgs)
		If VmCanClose AndAlso Not Me.cbarAdvManager.Visible Then
			Me.Close
		End If
	End Sub
	Sub FrmGestAdvLoad(sender As Object, e As EventArgs)
		Call Me.ReloadListes
		VmCanClose = True
	End Sub
	Sub BtAddActivate(sender As Object, e As EventArgs)
	Dim VpAdvName As String
	Dim VpAdvId As Integer
	Dim VpItem As ListViewItem
		VpAdvName = InputBox("Entrer le nom de l'adversaire:", "Nouvel adversaire", "(Adversaire)")
		If VpAdvName <> "" Then
			For Each VpD As ListViewItem In Me.lvwAdv.Items
				If VpD.Text.ToLower = VpAdvName.ToLower Then
					Call clsModule.ShowWarning("Un adversaire portant ce nom existe déjà...")
					Exit Sub
				End If
			Next VpD
			VpAdvId = clsModule.GetNewAdvId
			VgDBCommand.CommandText = "Insert Into MyAdversairesID Values (" + VpAdvId.ToString + ", '" + VpAdvName.Replace("'", "''") + "');"
			VgDBCommand.ExecuteNonQuery
			VpItem = New ListViewItem(VpAdvName)
			VpItem.SubItems.Add("0")
			Me.lvwAdv.Items.Add(VpItem)
			Me.cboOwner.Items.Add(VpAdvName)
		End If
	End Sub
	Sub CmdAffectClick(sender As Object, e As EventArgs)
	Dim VpAdvId As Integer
		If Me.cboOwner.Items.Contains(Me.cboOwner.Text) And Me.cboDeck.Items.Contains(Me.cboDeck.Text) Then
			VpAdvId = clsModule.GetAdvId(Me.cboOwner.Text)
			VgDBCommand.CommandText = "Update MyGamesID Set AdvID = " + VpAdvId.ToString + " Where GameName = '" + Me.cboDeck.Text.Replace("'", "''") + "';"
			VgDBCommand.ExecuteNonQuery
			Call Me.ReloadListes
		End If
	End Sub
	Sub CboDeckSelectedIndexChanged(sender As Object, e As EventArgs)
		Me.cboOwner.Text = clsModule.GetOwner(Me.cboDeck.Text)
	End Sub
	Sub LvwAdvSelectedIndexChanged(sender As Object, e As EventArgs)
		If Me.lvwAdv.SelectedIndices.Count > 0 Then
			Me.btRemove.Enabled = ( Me.lvwAdv.SelectedIndices(0) > 0 )
		Else
			Me.btRemove.Enabled = False			
		End If
		Me.btRename.Enabled = Me.btRemove.Enabled
	End Sub
	Sub BtRemoveActivate(sender As Object, e As EventArgs)
	Dim VpAdvId As Integer
		If clsModule.ShowQuestion("Êtes-vous sûr de vouloir supprimer l'adversaire " + Me.lvwAdv.SelectedItems(0).Text + " ?" + vbCrLf + "Tous ses decks vous seront réattribués...")  = System.Windows.Forms.DialogResult.Yes Then
			VpAdvId = clsModule.GetAdvId(Me.lvwAdv.SelectedItems(0).Text)
			'Réattribue tous les decks concernés au propriétaire 0 avant suppression
			VgDBCommand.CommandText = "Update MyGamesID Set AdvID = 0 Where AdvId = " + VpAdvId.ToString + ";"
			VgDBCommand.ExecuteNonQuery
			VgDBCommand.CommandText = "Delete * From MyAdversairesID Where AdvID = " + VpAdvId.ToString + ";"
			VgDBCommand.ExecuteNonQuery		
			Call Me.ReloadListes
		End If
	End Sub
	Sub BtRenameActivate(sender As Object, e As EventArgs)
	Dim VpAdvName As String
	Dim VpOldName As String = Me.lvwAdv.SelectedItems(0).Text
		VpAdvName = InputBox("Entrer le nom de l'adversaire :", "Renommer un adversaire", VpOldName)
		VpAdvName = VpAdvName.Replace("'", "''")
		VpOldName = VpOldName.Replace("'", "''")
		If VpAdvName <> "" Then
			For Each VpD As ListViewItem In Me.lvwAdv.Items
				If VpD.Text.ToLower = VpAdvName.ToLower Then
					Call clsModule.ShowWarning("Un adversaire portant ce nom existe déjà...")
					Exit Sub
				End If
			Next VpD
			VgDBCommand.CommandText = "Update MyAdversairesID Set AdvName = '" + VpAdvName + "' Where AdvName = '" + VpOldName + "';"
			VgDBCommand.ExecuteNonQuery
			Call Me.ReloadListes
		End If
	End Sub
End Class