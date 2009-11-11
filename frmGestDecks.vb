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
'| - gestion de la suppression			   09/01/2009 |
'------------------------------------------------------
Public Partial Class frmGestDecks
	Private VmFormMove As Boolean = False	'Formulaire en déplacement
	Private VmMousePos As Point				'Position initiale de la souris sur la barre de titre
	Private VmCanClose As Boolean = False   'Formulaire peut être fermé		
	Private VmOwner As MainForm
	Public Sub New(VpOwner As MainForm)
		Me.InitializeComponent()
		VmOwner = VpOwner
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
	Sub CmdCloseClick(ByVal sender As Object, ByVal e As EventArgs)
		Me.Close
	End Sub
	Sub FrmGestDecksLoad(ByVal sender As Object, ByVal e As EventArgs)
		For VpI As Integer = 1 To VgOptions.GetDeckCount
			Me.lstDecks.Items.Add(VgOptions.GetDeckName(VpI))
		Next VpI
	End Sub
	Sub CmdAddClick(ByVal sender As Object, ByVal e As EventArgs)
	Dim VpDeckName As String
	Dim VpId As Integer
		VpDeckName = InputBox("Entrer le nom du deck :", "Nouveau deck", "(nom)")
		If VpDeckName <> "" Then
			VpId = VgOptions.GetDeckCount
			VgDBCommand.CommandText = "Insert Into MyGamesID Values (" + VpId.ToString + ", '" + VpDeckName.Replace("'", "''") + "');"
			VgDBCommand.ExecuteNonQuery	
			Me.lstDecks.Items.Add(VgOptions.GetDeckName(VpId + 1))
		End If
	End Sub
	Sub LstDecksSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
		Me.cmdRen.Enabled = True
		Me.cmdRem.Enabled = True
	End Sub
	Sub CmdRenClick(ByVal sender As Object, ByVal e As EventArgs)
	Dim VpDeckName As String
		VpDeckName = InputBox("Entrer le nom du deck :", "Renommer un deck", VgOptions.GetDeckName(Me.lstDecks.SelectedIndex + 1))
		If VpDeckName <> "" Then
			VgDBCommand.CommandText = "Update MyGamesID Set GameName = '" + VpDeckName.Replace("'", "''") + "' Where GameID = " + Me.lstDecks.SelectedIndex.ToString + ";"
			VgDBCommand.ExecuteNonQuery			
			Me.lstDecks.Items(Me.lstDecks.SelectedIndex) = VgOptions.GetDeckName(Me.lstDecks.SelectedIndex + 1)
		End If		
	End Sub
	Sub CmdRemClick(ByVal sender As Object, ByVal e As EventArgs)
	Dim VpDeckName As String = VgOptions.GetDeckName(Me.lstDecks.SelectedIndex + 1)
	Dim VpDeckId As Integer = VgOptions.GetDeckIndex(VpDeckName)
		If clsModule.ShowQuestion("Êtes vous sûr de vouloir supprimer le deck " + VpDeckName + " ?" + vbCrLf + "Cela supprimera également les cartes saisies associées...") = System.Windows.Forms.DialogResult.Yes Then
			VgDBCommand.CommandText = "Delete * From MyGames Where GameID = " + VpDeckId.ToString + ";"
			VgDBCommand.ExecuteNonQuery
			VgDBCommand.CommandText = "Delete * From MyGamesId Where GameID = " + VpDeckId.ToString + ";"
			VgDBCommand.ExecuteNonQuery
			Me.lstDecks.Items.RemoveAt(Me.lstDecks.SelectedIndex)
		End If
	End Sub
	Sub FrmGestDecksFormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs)
		Call VmOwner.LoadMnu
		Call VmOwner.LoadTvw
	End Sub
End Class
