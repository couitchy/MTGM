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
'| Auteur         |                          Couitchy |
'|----------------------------------------------------|
'| Modifications :                                    |
'| - gestion de la suppression			   09/01/2009 |
'| - renvoi cartes vers collection		   29/08/2010 |
'| - gestion suppressions multiples		   09/05/2011 |
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
	Private Sub LoadDecks(Optional VpDirection As Integer = 0)
	Dim VpOldSelect As Integer = -1
		If Me.lstDecks.SelectedIndex >=0 Then
			VpOldSelect = Me.lstDecks.SelectedIndex
		End If
		Me.lstDecks.Items.Clear
		For VpI As Integer = 1 To clsModule.GetDeckCount
			Me.lstDecks.Items.Add(clsModule.GetDeckName(VpI))
		Next VpI
		Me.btDown.Enabled = False
		Me.btUp.Enabled = False
		Me.btRename.Enabled = False
		Me.btRemove.Enabled = False
		If VpOldSelect <> -1 Then
			Me.lstDecks.SelectedIndex = VpOldSelect + VpDirection
		End If
	End Sub
	Private Sub SwapDeckId(VpTable As String, VpId1 As Integer, VpId2 As Integer)
	'----------------------------------------------------------------
	'Intervertit les identifiants des deux decks passés en paramètres
	'----------------------------------------------------------------
		VgDBCommand.CommandText = "Update " + VpTable + " Set GameID = -1 Where GameID = " + VpId1.ToString + ";"
		VgDBCommand.ExecuteNonQuery
		VgDBCommand.CommandText = "Update " + VpTable + " Set GameID = " + VpId1.ToString + " Where GameID = " + VpId2.ToString + ";"
		VgDBCommand.ExecuteNonQuery
		VgDBCommand.CommandText = "Update " + VpTable + " Set GameID = " + VpId2.ToString + " Where GameID = -1;"
		VgDBCommand.ExecuteNonQuery
	End Sub
	Private Sub ManageSwap(VpDirection As Integer)
	Dim VpId1 As Integer = clsModule.GetDeckIndex(Me.lstDecks.SelectedItem)
	Dim VpId2 As Integer = clsModule.GetDeckIndex(Me.lstDecks.Items(Me.lstDecks.SelectedIndex + VpDirection))
		Call Me.SwapDeckId("MyGamesID", VpId1, VpId2)
		Call Me.SwapDeckId("MyGames", VpId1, VpId2)
		Call Me.LoadDecks(VpDirection)
	End Sub
	Private Sub RemoveDeck(VpDeckName As String)
	'-----------------------------------
	'Gestion de la suppression d'un deck
	'-----------------------------------
	Dim VpDeckId As Integer = clsModule.GetDeckIndex(VpDeckName)
	Dim VpQuestion As DialogResult = clsModule.ShowQuestion("Le deck " + VpDeckName + " va être supprimé." + vbCrLf + "Souhaitez-vous déplacer les cartes qu'il contenait vers la collection ?", MessageBoxButtons.YesNoCancel)
	Dim VpContenu As Hashtable
	Dim VpO As Object
		If VpQuestion = System.Windows.Forms.DialogResult.Cancel Then Exit Sub
		'Recopie avant suppression
		If VpQuestion = System.Windows.Forms.DialogResult.Yes Then
			'Récupération du contenu du deck
			VpContenu = New Hashtable
			VgDBCommand.CommandText = "Select EncNbr, Items From MyGames Where GameID = " + VpDeckId.ToString + ";"
			VgDBReader = VgDBCommand.ExecuteReader
			With VgDBReader
				While .Read
					VpContenu.Add(.GetInt32(0), .GetInt32(1))
				End While
				.Close
			End With
			'Insertion ou mise à jour dans la collection
			For Each VpEncNbr As Integer In VpContenu.Keys
				VgDBCommand.CommandText = "Select Items From MyCollection Where EncNbr = " + VpEncNbr.ToString + ";"
				VpO = VgDBCommand.ExecuteScalar
				'Cas 1 : la carte a ajouté existe déjà => mise à jour de la quantité somme
				If Not VpO Is Nothing AndAlso CInt(VpO) > 0 Then
					VgDBCommand.CommandText = "Update MyCollection Set Items = " + (CInt(VpO) + VpContenu.Item(VpEncNbr)).ToString + " Where EncNbr = " + VpEncNbr.ToString + ";"
				'Cas 2 : nouvelle carte => insertion
				Else
					VgDBCommand.CommandText = "Insert Into MyCollection(EncNbr, Items) Values (" + VpEncNbr.ToString + ", " + VpContenu.Item(VpEncNbr).ToString + ");"
				End If
				VgDBCommand.ExecuteNonQuery
			Next VpEncNbr
		End If
		VgDBCommand.CommandText = "Delete * From MyGames Where GameID = " + VpDeckId.ToString + ";"
		VgDBCommand.ExecuteNonQuery
		VgDBCommand.CommandText = "Delete * From MyGamesId Where GameID = " + VpDeckId.ToString + ";"
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
	End Sub
	Sub LstDecksSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
		Me.btRemove.Enabled = True
		If Me.lstDecks.SelectedIndices.Count > 1 Then
			Me.btRename.Enabled = False
			Me.btUp.Enabled = False
			Me.btDown.Enabled = False
		Else			
			Me.btRename.Enabled = True
			Me.btUp.Enabled = ( Me.lstDecks.SelectedIndex > 0 )
			Me.btDown.Enabled = ( Me.lstDecks.SelectedIndex < Me.lstDecks.Items.Count - 1 )
		End If
	End Sub
	Sub FrmGestDecksFormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs)
		If e.CloseReason = CloseReason.UserClosing Then
			Me.Visible = False
			Call VmOwner.LoadMnu
			Call VmOwner.LoadTvw
		End If
	End Sub
	Sub BtAddActivate(sender As Object, e As EventArgs)
	Dim VpDeckName As String
	Dim VpId As Integer
		VpDeckName = InputBox("Entrer le nom du deck :", "Nouveau deck", "(Deck)")
		If VpDeckName <> "" Then
			For Each VpD As String In Me.lstDecks.Items
				If VpD.ToLower = VpDeckName.ToLower Then
					Call clsModule.ShowWarning("Un deck portant ce nom existe déjà...")
					Exit Sub
				End If
			Next VpD
			VpId = clsModule.GetNewDeckId
			VgDBCommand.CommandText = "Insert Into MyGamesID Values (" + VpId.ToString + ", '" + VpDeckName.Replace("'", "''") + "', 0);"
			VgDBCommand.ExecuteNonQuery
			Me.lstDecks.Items.Add(clsModule.GetDeckName(VpId + 1))
		End If
	End Sub
	Sub BtRemoveActivate(sender As Object, e As EventArgs)
	Dim VpToRemove As New ArrayList
		For Each VpI As Integer In Me.lstDecks.SelectedIndices	
			Call Me.RemoveDeck(clsModule.GetDeckName(VpI + 1))
			VpToRemove.Add(Me.lstDecks.Items.Item(VpI))		'liste temporaire car on ne peut pas toucher à la collection qu'on est en train d'énumérer
		Next VpI
		For Each VpItem As String In VpToRemove
			Me.lstDecks.Items.Remove(VpItem)
		Next VpItem
		Me.btRemove.Enabled = False
		Me.btRename.Enabled = False
		Me.btUp.Enabled = False
		Me.btDown.Enabled = False
	End Sub
	Sub BtRenameActivate(sender As Object, e As EventArgs)
	Dim VpDeckName As String
	Dim VpOldName As String = Me.lstDecks.SelectedItem 'clsModule.GetDeckName(Me.lstDecks.SelectedIndex + 1)
		VpDeckName = InputBox("Entrer le nom du deck :", "Renommer un deck", VpOldName)
		VpDeckName = VpDeckName.Replace("'", "''")
		VpOldName = VpOldName.Replace("'", "''")
		If VpDeckName <> "" Then
			For Each VpD As String In Me.lstDecks.Items
				If VpD.ToLower = VpDeckName.ToLower Then
					Call clsModule.ShowWarning("Un deck portant ce nom existe déjà...")
					Exit Sub
				End If
			Next VpD
			VgDBCommand.CommandText = "Select * From MyScores Where JeuLocal = '" + VpOldName + "' Or JeuAdverse = '" + VpOldName + "';"
			VgDBReader = VgDBCommand.ExecuteReader
			VgDBReader.Read
			If VgDBReader.HasRows Then
				VgDBReader.Close
				If clsModule.ShowQuestion("Ce deck est lié à des parties saisies dans le comptage Victoires / Défaites." + vbCrlf + "Renommer également le label des parties en question ?") = System.Windows.Forms.DialogResult.Yes Then
					VgDBCommand.CommandText = "Update MyScores Set JeuLocal = '" + VpDeckName + "' Where JeuLocal = '" + VpOldName + "';"
					VgDBCommand.ExecuteNonQuery
					VgDBCommand.CommandText = "Update MyScores Set JeuAdverse = '" + VpDeckName + "' Where JeuAdverse = '" + VpOldName + "';"
					VgDBCommand.ExecuteNonQuery
				End If
			Else
				VgDBReader.Close
			End If
			VgDBCommand.CommandText = "Update MyGamesID Set GameName = '" + VpDeckName + "' Where GameName = '" + VpOldName + "';"
			VgDBCommand.ExecuteNonQuery
			Me.lstDecks.Items(Me.lstDecks.SelectedIndex) = clsModule.GetDeckName(Me.lstDecks.SelectedIndex + 1)
		End If
	End Sub
	Sub BtDownActivate(sender As Object, e As EventArgs)
		Call Me.ManageSwap(1)
	End Sub
	Sub BtUpActivate(sender As Object, e As EventArgs)
		Call Me.ManageSwap(-1)
	End Sub
End Class