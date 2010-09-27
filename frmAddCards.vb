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
'| Auteur         |                          Couitchy |
'|----------------------------------------------------|
'| Modifications :                                    |
'| - verrouillage de l'édition de saisie   27/03/2010 |
'------------------------------------------------------
Public Partial Class frmAddCards
	#Region "Déclarations"
	Private VmFormMove As Boolean = False			'Formulaire en déplacement
	Private VmMousePos As Point						'Position initiale de la souris sur la barre de titre
	Private VmCanClose As Boolean = False  			'Formulaire peut être fermé
	Private VmChangesCommited As Boolean = False	'Modifications en cours sur ComboBoxes
	Private VmOwner As MainForm
	#End Region
	#Region "Méthodes"
	Private Sub LoadCombo(VpCbo As ComboBox, VpTable As String, VpField As String)
	'---------------------------------------------------------------------------------------------------
	'Ajoute à la boîte combo passée en paramètres la liste des cartes de la base dans la table spécifiée
	'---------------------------------------------------------------------------------------------------
	Dim VpTitle As String
		VgDBCommand.CommandText = "Select Distinct " + VpField + " From " + VpTable + ";"
		VgDBReader = VgDBCommand.ExecuteReader
		With VgDBReader
			While .Read
				VpTitle = .GetString(0)
				If VpTitle.Trim <> "" Then
					VpCbo.Items.Add(VpTitle)
				End If
			End While
			.Close
		End With
	End Sub
	Private Sub ComboChangeManagement(VpField1 As String, VpField2 As String, VpTable1 As String, VpTable2 As String, VpCbo1 As ComboBox, VpCbo2 As ComboBox)
	'-----------------------------------------------------------
	'Gère le changement de la sélection dans un des comboboxes :
	'- traduction dans l'autre
	'- récupération du numéro encyclopédique
	'- récupération des séries disponibles
	'-----------------------------------------------------------
		If VmChangesCommited Then Exit Sub
		VmChangesCommited = True
		VgDBCommand.CommandText = "Select " + VpField1 + " From " + VpTable1 + " Where EncNbr = " + FindInfos(VpCbo2.Text, VpTable2, VpField2).ToString + ";"
		Try
			VpCbo1.Text = VgDBCommand.ExecuteScalar.ToString
		Catch
			VpCbo1.Text = ""
		End Try
		Me.cboSerie.Items.Clear
		VgDBCommand.CommandText = "Select Card.Series, Series.SeriesNM From Series Inner Join Card On Series.SeriesCD = Card.Series Where Card.Title = '" + Me.cboTitleEN.Text.Replace("'", "''") + "';"
		VgDBReader = VgDBCommand.ExecuteReader
		With VgDBReader
			While .Read
				Me.cboSerie.Items.Add("(" + .GetString(0) + ") " + .GetString(1))
			End While
			.Close
		End With
		If Me.cboSerie.Tag <> "" Then
			If Me.cboSerie.Items.Contains(Me.cboSerie.Tag) Then
				Me.cboSerie.SelectedIndex = Me.cboSerie.Items.IndexOf(Me.cboSerie.Tag)
			Else
				Me.cboSerie.SelectedIndex = 0
			End If
		Else
			Me.cboSerie.SelectedIndex = 0
		End If
		VmChangesCommited = False
	End Sub
	Private Function AdjustEncNbr(VpTitle As String, VpSerie As String) As Integer
	'------------------------------------------------------------------------------------------
	'Retourne le numéro encyclopédique de la carte passée en paramètre pour l'édition spécifiée
	'------------------------------------------------------------------------------------------
		VgDBCommand.CommandText = "Select Card.EncNbr From Card Inner Join Series On Card.Series = Series.SeriesCD Where Card.Title = '" + VpTitle.Replace("'", "''") + "' And Series.SeriesNM = '" + VpSerie + "';"
		Return VgDBCommand.ExecuteScalar.ToString
	End Function
	Private Function FindDateSerie(VpSerie As String) As String
	'--------------------------------------------------------------
	'Retourne la date d'impression de l'édition passée en paramètre
	'--------------------------------------------------------------
		VgDBCommand.CommandText = "Select Release From Series Where SeriesNM = '" + VpSerie + "';"
		Return CDate(VgDBCommand.ExecuteScalar).Year
	End Function
	Private Function FindQuant(VpEncNbr As String) As String
	'--------------------------------------------------------------------------------------------------------------
	'Retourne la quantité enregistrée d'items dont la référence est passée en paramètre dans la collection courante
	'--------------------------------------------------------------------------------------------------------------
	Dim VpSource As String = IIf(Me.mnuDropToCollection.Checked, clsModule.CgSCollection, clsModule.CgSDecks)
	Dim VpO As Object
	Dim VpStr As String = "0"
		If Not IsNumeric(VpEncNbr) Then Return clsModule.CgStock
		'Quantité dans l'édition courante
		VgDBCommand.CommandText = "Select Items From " + VpSource + " Where EncNbr = " + VpEncNbr + IIf(Me.mnuDropToCollection.Checked = False, " And GameID = " + Me.cmdDestination.Tag.ToString + ";", ";")
		VpO = VgDBCommand.ExecuteScalar
		If Not VpO Is Nothing Then
			VpStr = VpO.ToString
		End If
		Me.lblNbItems.Tag = Val(VpStr)
		VpStr = VpStr + " " + Me.cboSerie.Text.Substring(0, 4) + " / "
		'Quantité totale toutes éditions confondues
		VgDBCommand.CommandText = "Select Sum(Items) From " + VpSource + " Inner Join Card On Card.EncNbr = " + VpSource + ".EncNbr Where Title = '" + Me.cboTitleEN.Text.Replace("'", "''") + "'" + IIf(Me.mnuDropToCollection.Checked = False, " And GameID = " + Me.cmdDestination.Tag.ToString + ";", ";")
		VpO = VgDBCommand.ExecuteScalar
		If (Not VpO Is Nothing) AndAlso VpO.ToString <> "" Then
			Return VpStr + VpO.ToString + " (total)"
		Else
			Return VpStr + "0 (total)"
		End If
	End Function
	Private Function FindInfos(VpTitle As String, VpTable As String, VpField As String) As Integer
	'-------------------------------------------------------------------------------------------------------------
	'Retourne le numéro encyclopédique de la carte spécifiée en paramètre, en plus de l'inscrire sur le formulaire
	'-------------------------------------------------------------------------------------------------------------
		VgDBCommand.CommandText = "Select EncNbr From " + VpTable + " Where " + VpField + " = '" + VpTitle.Replace("'", "''") + "';"
		Try
			Me.lblEncNbr.Text = VgDBCommand.ExecuteScalar.ToString
			Return CInt(Me.lblEncNbr.Text)
		Catch
			Return 0
		End Try
	End Function
	Public Sub New(VpOwner As MainForm)
		Me.InitializeComponent()
		VmOwner = VpOwner
		Me.cboSerie.Tag = ""
		Me.lblNbItems.Tag = 0
		Call Me.LoadCombo(Me.cboTitleFR, "CardFR", "TitleFR")
		Call Me.LoadCombo(Me.cboTitleEN, "Card", "Title")
		If clsModule.GetDeckCount = 0 Then
			Me.cmdDestination.Visible = False
		Else
			'Destinations possibles
			For VpI As Integer = 1 To clsModule.GetDeckCount
				Me.cmnuDestination.Items.Add(clsModule.GetDeckName(VpI), Nothing, AddressOf DropTo)
			Next VpI
		End If
		'Destination par défaut
		For Each VpItem1 As Object In VpOwner.mnuDisp.DropDownItems
			If clsModule.SafeGetChecked(VpItem1) Then
				For Each VpItem2 As ToolStripMenuItem In Me.cmnuDestination.Items
					If VpItem1.Text = VpItem2.Text Then
						VpItem2.Checked = True
						Me.cmdDestination.Tag = clsModule.GetDeckIndex(VpItem2.Text)
						Exit For
					End If
				Next VpItem2
				Exit For
			End If
		Next VpItem1
	End Sub
	#End Region
	#Region "Evènements"
	Sub DropTo(ByVal sender As Object, ByVal e As EventArgs)
		For Each VpItem As ToolStripMenuItem In Me.cmnuDestination.Items
			VpItem.Checked = ( VpItem Is sender )
		Next VpItem
		Me.cmdDestination.Tag = clsModule.GetDeckIndex(sender.Text)
		Me.lblNbItems.Text = Me.FindQuant(Me.lblEncNbr.Text)
	End Sub
	Private Sub CBarAjoutMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
		VmFormMove = True
		VmMousePos = New Point(e.X, e.Y)
	End Sub
	Private Sub CBarAjoutMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs)
		VmFormMove = False
	End Sub
	Sub CBarAjoutMouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
		If VmFormMove Then
			Me.Location = New Point(MousePosition.X - VmMousePos.X, MousePosition.Y - VmMousePos.Y)
		End If
	End Sub
	Sub CboTitleFRSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
		Call Me.ComboChangeManagement("Title", "TitleFR", "Card", "CardFR", Me.cboTitleEN, Me.cboTitleFR)
		Me.cboTitleFR.Tag = Me.cboTitleFR.Text
	End Sub
	Sub CboTitleENSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
		Call Me.ComboChangeManagement("TitleFR", "Title", "CardFR", "Card", Me.cboTitleFR, Me.cboTitleEN)
		Me.cboTitleEN.Tag = Me.cboTitleEN.Text
	End Sub
	Sub CboSerieSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
	Dim VpKey As Integer = clsModule.VgImgSeries.Images.IndexOfKey("_e" + Me.cboSerie.Text.Substring(1, 2) + CgIconsExt)
		Me.lblYear.Text = Me.FindDateSerie(Me.cboSerie.Text.Substring(5))
		Me.lblEncNbr.Text = Me.AdjustEncNbr(Me.cboTitleEN.Text, Me.cboSerie.Text.Substring(5))
		If VpKey <> -1 Then
			Me.imgEdition.Image = clsModule.VgImgSeries.Images(VpKey)
		Else
			Me.imgEdition.Image = Nothing
		End If
		Me.lblNbItems.Text = Me.FindQuant(Me.lblEncNbr.Text)
	End Sub
	Sub CmdAddClick(ByVal sender As Object, ByVal e As EventArgs)
		Try
			If CInt(Me.lblNbItems.Tag) = 0 Then
				VgDBCommand.CommandText = "Insert Into " + IIf(Me.mnuDropToCollection.Checked, clsModule.CgSCollection, clsModule.CgSDecks) + " Values (" + IIf(Me.mnuDropToCollection.Checked = False, Me.cmdDestination.Tag.ToString + ", ", "") + Me.lblEncNbr.Text + ", " + CInt(Me.txtNbItems.Text).ToString + ");"
				VgDBCommand.ExecuteNonQuery
			ElseIf ( CInt(Me.lblNbItems.Tag) + CInt(Me.txtNbItems.Text) ) <= 0 Then
				VgDBCommand.CommandText = "Delete * From " + IIf(Me.mnuDropToCollection.Checked, clsModule.CgSCollection, clsModule.CgSDecks) + " Where EncNbr = " + Me.lblEncNbr.Text + IIf(Me.mnuDropToCollection.Checked = False, " And GameID = " + Me.cmdDestination.Tag.ToString, "") + ";"
				VgDBCommand.ExecuteNonQuery
			Else
				VgDBCommand.CommandText = "Update " + IIf(Me.mnuDropToCollection.Checked, clsModule.CgSCollection, clsModule.CgSDecks) + " Set Items = " + (CInt(Me.lblNbItems.Tag) + CInt(Me.txtNbItems.Text)).ToString + " Where EncNbr = " + Me.lblEncNbr.Text + IIf(Me.mnuDropToCollection.Checked = False, " And GameID = " + Me.cmdDestination.Tag.ToString, "") + ";"
				VgDBCommand.ExecuteNonQuery
			End If
			Me.cboSerie.Tag = Me.cboSerie.Text
			Me.cboTitleFR.Text = ""
			Me.cboTitleEN.Text = ""
			Me.cboSerie.Text = ""
			Me.cboSerie.Items.Clear
			Me.lblEncNbr.Text = "ID Encyclopédie"
			Me.lblYear.Text = "Année"
			Me.imgEdition.Image = Nothing
			Me.lblNbItems.Text = clsModule.CgStock
			Me.lblNbItems.Tag = 0
			Me.txtNbItems.Text = "+1"
			Me.cboTitleFR.Focus
		Catch
			Call clsModule.ShowWarning("Impossible d'ajouter cette carte dans la base de données." + vbCrLf + "La carte n'est peut-être pas (ou incorrectement) référencée...")
		End Try
	End Sub
	Sub CboTitleKeyUp(ByVal sender As Object, ByVal e As KeyEventArgs)
		If e.KeyCode = Keys.Add Then
			sender.Text = sender.Tag
		End If
	End Sub
	Sub CmdDestinationMouseDown(sender As Object, e As MouseEventArgs)
		Me.cmnuDestination.Show(Me.cmdDestination, e.Location)
	End Sub
	Sub CBarAjoutVisibleChanged(sender As Object, e As EventArgs)
		If VmCanClose AndAlso Not Me.CBarAjout.Visible Then
			Me.Close
		End If		
	End Sub
	Sub FrmAddCardsFormClosing(sender As Object, e As FormClosingEventArgs)
		VmOwner.BringToFront
		Me.Hide
		If VgOptions.VgSettings.AutoRefresh Then
			Call VmOwner.LoadTvw
		End If		
	End Sub
	Sub FrmAddCardsLoad(sender As Object, e As EventArgs)
		VmCanClose = True
	End Sub
	#End Region
End Class
