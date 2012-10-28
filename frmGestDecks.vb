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
'| Auteur         |                          Couitchy |
'|----------------------------------------------------|
'| Modifications :                                    |
'| - gestion de la suppression			   09/01/2009 |
'| - renvoi cartes vers collection		   29/08/2010 |
'| - gestion suppressions multiples		   09/05/2011 |
'| - gestion d'infos personnalisées		   08/04/2012 |
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
	Private Sub LoadDecks(Optional VpDirection As Integer = 0)
	'--------------------------------------------
	'Chargement de la liste des decks disponibles
	'--------------------------------------------
	Dim VpOldSelect As Integer = -1
		If Me.lstDecks.SelectedIndex >= 0 Then
			VpOldSelect = Me.lstDecks.SelectedIndex
		End If
		Me.lstDecks.Items.Clear
		Me.cboFormat.Items.Clear
		For VpI As Integer = 1 To clsModule.GetDeckCount
			Me.lstDecks.Items.Add(clsModule.GetDeckName(VpI))
		Next VpI
		VgDBCommand.CommandText = "Select Distinct GameFormat From MyGamesID;"
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
		If VpOldSelect <> -1 Then
			Me.lstDecks.SelectedIndex = VpOldSelect + VpDirection
		End If
	End Sub
	Private Sub LoadDeckInfos
	'----------------------------------------------------------------
	'Charge les infos (date, format, description) du deck sélectionné
	'----------------------------------------------------------------
		If Me.lstDecks.SelectedIndices.Count > 0 Then
			VgDBCommand.CommandText = "Select GameDate, GameFormat, GameDescription From MyGamesID Where GameName = '" + Me.lstDecks.SelectedItem.ToString.Replace("'", "''") + "';"
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
		If Me.lstDecks.SelectedIndices.Count > 0 Then
			VgDBCommand.CommandText = "Update MyGamesID Set GameDate = '" + Me.pickDate.Value.ToShortDateString + "', GameFormat = '" + Me.cboFormat.Text.Replace("'", "''") + "', GameDescription = '" + Me.txtMemo.Text.Replace("'", "''") + "' Where GameName = '" + Me.lstDecks.SelectedItem.ToString.Replace("'", "''") + "';"
			VgDBCommand.ExecuteNonQuery
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
		VmMustReload = True
	End Sub
	Private Function RemoveDeck(VpDeckName As String) As Boolean
	'-----------------------------------
	'Gestion de la suppression d'un deck
	'-----------------------------------
	Dim VpDeckId As Integer = clsModule.GetDeckIndex(VpDeckName)
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
			Me.btRename.Enabled = ( Me.lstDecks.SelectedIndices.Count = 1 )
			Me.btUp.Enabled = ( Me.lstDecks.SelectedIndex > 0 )
			Me.btDown.Enabled = ( Me.lstDecks.SelectedIndex < Me.lstDecks.Items.Count - 1 )
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
	Sub BtAddActivate(sender As Object, e As EventArgs)
	Dim VpDeckName As String
	Dim VpId As Integer
		VpDeckName = InputBox("Entrer le nom du deck :", "Nouveau deck", clsModule.CgDefaultName)
		If VpDeckName <> "" Then
			For Each VpD As String In Me.lstDecks.Items
				If VpD.ToLower = VpDeckName.ToLower Then
					Call clsModule.ShowWarning("Un deck portant ce nom existe déjà...")
					Exit Sub
				End If
			Next VpD
			VpId = clsModule.GetNewDeckId
			VgDBCommand.CommandText = "Insert Into MyGamesID(GameID, GameName, AdvID, GameDate, GameFormat, GameDescription) Values (" + VpId.ToString + ", '" + VpDeckName.Replace("'", "''") + "', 0, '" + Now.ToShortDateString + "', '" + clsModule.CgDefaultFormat + "', '');"
			VgDBCommand.ExecuteNonQuery
			Me.lstDecks.Items.Add(clsModule.GetDeckName(VpId + 1))
			VmMustReload = True
		End If
	End Sub
	Sub BtRemoveActivate(sender As Object, e As EventArgs)
	Dim VpDecksToRemove As New Hashtable
	Dim VpToRemove As New List(Of String)
		'Informations sur les decks à supprimer (indice, nom) à mémoriser avant car en cours de suppression les indices se trouveraient décalés
		For Each VpI As Integer In Me.lstDecks.SelectedIndices
			VpDecksToRemove.Add(VpI, clsModule.GetDeckName(VpI + 1))
		Next VpI
		'Suppression unitaire en base de données
		For Each VpKey As Integer In VpDecksToRemove.Keys
			If Me.RemoveDeck(VpDecksToRemove.Item(VpKey)) Then
				VpToRemove.Add(Me.lstDecks.Items.Item(VpKey))		'liste temporaire car on ne peut pas toucher à la collection qu'on est en train d'énumérer
			End If
		Next VpKey
		'Suppression dans la listbox
		For Each VpItem As String In VpToRemove
			Me.lstDecks.Items.Remove(VpItem)
		Next VpItem
		Me.btRemove.Enabled = False
		Me.btRename.Enabled = False
		Me.btUp.Enabled = False
		Me.btDown.Enabled = False
		VmMustReload = True
		Call VmOwner.LoadMnu
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
			VmMustReload = True
		End If
	End Sub
	Sub BtDownActivate(sender As Object, e As EventArgs)
		Call Me.ManageSwap(1)
	End Sub
	Sub BtUpActivate(sender As Object, e As EventArgs)
		Call Me.ManageSwap(-1)
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