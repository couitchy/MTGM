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
'| Auteur         |                          Couitchy |
'|----------------------------------------------------|
'| Modifications :                                    |
'| - formatage des colonnes spéciales	   20/03/2010 |
'| - extraction des images de la sélection 24/07/2010 |
'| - ajout de la colonne opt. force / end. 26/01/2012 |
'------------------------------------------------------
Public Partial Class frmXL
	Private VmFormMove As Boolean = False	'Formulaire en déplacement
	Private VmMousePos As Point				'Position initiale de la souris sur la barre de titre
	Private VmCanClose As Boolean = False   'Formulaire peut être fermé
	Private VmSource As String
	Private VmRestriction As String
	Private VmRestrictionTXT As String
	Private VmBusy As Boolean = False
	Public Sub New(VpOwner As MainForm)
		Me.InitializeComponent()
		VmSource = VpOwner.MySource
		VmRestriction = VpOwner.Restriction
		VmRestrictionTXT = VpOwner.Restriction(True)
		If VmRestrictionTXT.Length > 31 Then
			VmRestrictionTXT = VmRestrictionTXT.Substring(0, 31)
		End If
		Me.txtSaveImg.Text = Application.StartupPath
	End Sub
	Private Sub ExcelGen
	'---------------------------------------------------------------------
	'Génération d'une liste des cartes de la sélection courante sous Excel
	'---------------------------------------------------------------------
	Dim VpExcelApp As Object			'Objet Excel par OLE
	Dim VpBook As Object
	Dim VpSheet As Object
	Dim VpSQL As String					'Reqûete SQL
	Dim VpY As Integer = 1				'Numéro de ligne courante
	Dim VpX As Integer					'Numéro de colonne courante
	Dim VpForceCurrency As Integer = -1
	Dim VpForceText As Integer = -1
	Dim VpElements As New List(Of clsXLItem)
	Dim VpElementsGroupes As New List(Of clsXLItem)
	Dim VpCur As clsXLItem
		System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US")
		Try
			VpExcelApp = CreateObject("Excel.Application")
		Catch
			Call clsModule.ShowWarning("Aucune installation de Microsoft Excel n'a été détectée sur votre système." + vbCrLf + "Impossible de continuer...")
			System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("fr-FR")
			Exit Sub
		End Try
		'Récupération de la liste
		VpSQL = "Select * From (((((" + VmSource + " Inner Join Card On " + VmSource + ".EncNbr = Card.EncNbr) Inner Join Spell On Card.Title = Spell.Title) Inner Join Series On Card.Series = Series.SeriesCD) Inner Join CardFR On Card.EncNbr = CardFR.EncNbr) Inner Join TextesFR On TextesFR.CardName = Card.Title) Left Join Creature On Card.Title = Creature.Title Where "
		VpSQL = VpSQL + VmRestriction
		VpSQL = clsModule.TrimQuery(VpSQL, True, " Order By Card.Title")
		VgDBCommand.CommandText = VpSQL
		VgDBReader = VgDBcommand.ExecuteReader
		With VgDBReader
			While .Read
				VpElements.Add(New clsXLItem(Me.chklstXL, If(Me.chkVF.Checked, .GetString(.GetOrdinal("TitleFR")), .GetString(.GetOrdinal("Card.Title"))), CInt(.GetValue(.GetOrdinal("Items"))), CBool(.GetValue(.GetOrdinal("Foil"))), .GetValue(.GetOrdinal("Color")).ToString, .GetValue(.GetOrdinal("Power")).ToString, .GetValue(.GetOrdinal("Tough")).ToString, .GetValue(.GetOrdinal("Cost")).ToString, .GetValue(.GetOrdinal(If(Me.chkVF.Checked, "SeriesNM_FR", "SeriesNM"))).ToString, .GetValue(.GetOrdinal("Price")).ToString, .GetValue(.GetOrdinal("Rarity")).ToString, .GetValue(.GetOrdinal("SubType")).ToString, .GetValue(.GetOrdinal("Type")).ToString, .GetValue(.GetOrdinal(If(Me.chkVF.Checked, "TexteFR", "CardText"))).ToString.Trim))
			End While
			.Close
		End With
		'Nouveau classeur
		VpExcelApp.DisplayAlerts = False
		VpBook = VpExcelApp.Workbooks.Add
		For Each VpSheet In VpBook.WorkSheets
			If VpBook.WorkSheets.Count > 1 Then
				VpSheet.Delete
			End If
		Next VpSheet
		VpSheet = VpBook.ActiveSheet
		VpExcelApp.Visible = Me.chkXLShow.Checked
		If VpElements.Count > 0 Then
			'Groupement pour éviter les doublons sur les critères non utilisés
			VpCur = VpElements.Item(0)
			For VpI As Integer = 1 To VpElements.Count - 1
				If clsXLItem.AreAlike(VpCur, VpElements.Item(VpI)) Then
					VpCur.Quant = VpCur.Quant + VpElements.Item(VpI).Quant
				Else
					VpElementsGroupes.Add(VpCur)
					VpCur = VpElements.Item(VpI)
				End If
			Next VpI
			VpElementsGroupes.Add(VpCur)
			'Remplissage
			With VpSheet
				'Nom de la feuille
				.Name = VmRestrictionTXT.Replace("/", " ").Replace("\", " ").Replace("?", " ").Replace("*", " ").Replace("[", " ").Replace("]", " ")
				'En-têtes
				If Me.chkHeaders.Checked Then
					VpX = 1
					'Quantité
					.Cells(VpY, VpX) = "Quantité"
					VpX = VpX + 1
					'Nom
					.Cells(VpY, VpX) = "Nom"
					VpX = VpX + 1
					'Foil ou pas
					.Cells(VpY, VpX) = "Foil"
					VpX = VpX + 1					
					'Type
					If Me.chklstXL.GetItemChecked(7) Then
						.Cells(VpY, VpX) = "Type"
						VpX = VpX + 1
					End If
					'Sous-type
					If Me.chklstXL.GetItemChecked(6) Then
						.Cells(VpY, VpX) = "Sous-type"
						VpX = VpX + 1
					End If
					'Couleur
					If Me.chklstXL.GetItemChecked(0) Then
						.Cells(VpY, VpX) = "Couleur"
						VpX = VpX + 1
					End If
					'Force / Endurance
					If Me.chklstXL.GetItemChecked(1) Then
						.Cells(VpY, VpX) = "Force / Endurance"
						VpX = VpX + 1
					End If
					'Coût d'invocation
					If Me.chklstXL.GetItemChecked(2) Then
						.Cells(VpY, VpX) = "Coût d'invocation"
						VpX = VpX + 1
					End If
					'Edition
					If Me.chklstXL.GetItemChecked(3) Then
						.Cells(VpY, VpX) = "Edition"
						VpX = VpX + 1
					End If
					'Prix
					If Me.chklstXL.GetItemChecked(4) Then
						.Cells(VpY, VpX) = "Prix unitaire"
						VpX = VpX + 1
					End If
					'Rareté
					If Me.chklstXL.GetItemChecked(5) Then
						.Cells(VpY, VpX) = "Rareté"
						VpX = VpX + 1
					End If
					'Texte
					If Me.chklstXL.GetItemChecked(8) Then
						.Cells(VpY, VpX) = "Texte descriptif"
						VpX = VpX + 1
					End If
					.Cells(VpY, VpX).EntireRow.Font.Bold = True
					VpY = VpY + 1
				End If
				'Eléments
				For Each VpCur In VpElementsGroupes
					VpX = 1
					'Quantité
					.Cells(VpY, VpX) = VpCur.Quant
					VpX = VpX + 1
					'Nom
					.Cells(VpY, VpX) = VpCur.Title
					VpX = VpX + 1
					'Foil ou pas
					.Cells(VpY, VpX) = If(VpCur.Foil, "Premium", "")
					VpX = VpX + 1					
					'Type
					If Me.chklstXL.GetItemChecked(7) Then
						.Cells(VpY, VpX) = VpCur.Type
						VpX = VpX + 1
					End If
					'Sous-type
					If Me.chklstXL.GetItemChecked(6) Then
						.Cells(VpY, VpX) = VpCur.SubType
						VpX = VpX + 1
					End If
					'Couleur
					If Me.chklstXL.GetItemChecked(0) Then
						.Cells(VpY, VpX) = VpCur.Color
						VpX = VpX + 1
					End If
					'Force / Endurance
					If Me.chklstXL.GetItemChecked(1) Then
						.Cells(VpY, VpX) = VpCur.ForceEndurance
						VpX = VpX + 1
					End If
					'Coût d'invocation
					If Me.chklstXL.GetItemChecked(2) Then
						.Cells(VpY, VpX) = VpCur.Invoc
						VpForceText = VpX
						VpX = VpX + 1
					End If
					'Edition
					If Me.chklstXL.GetItemChecked(3) Then
						.Cells(VpY, VpX) = VpCur.Serie
						VpX = VpX + 1
					End If
					'Prix
					If Me.chklstXL.GetItemChecked(4) Then
						.Cells(VpY, VpX) = VpCur.Price
						VpForceCurrency = VpX
						VpX = VpX + 1
					End If
					'Rareté
					If Me.chklstXL.GetItemChecked(5) Then
						.Cells(VpY, VpX) = VpCur.Rarity
						VpX = VpX + 1
					End If
					'Texte
					If Me.chklstXL.GetItemChecked(8) Then
						.Cells(VpY, VpX) = VpCur.CardText
						VpX = VpX + 1
					End If
					VpY = VpY + 1
				Next VpCur
				'Ajustement largeur colonnes
				For VpI As Integer = 1 To VpX - 1
					.Columns(VpI).WrapText = False
					.Columns(VpI).EntireColumn.AutoFit
				Next VpI
				'Formatage particulier
				If VpForceCurrency <> -1 Then
					.Columns(VpForceCurrency).EntireColumn.NumberFormat = "0.00 €"
				End If
				If VpForceText <> -1 Then
					.Columns(VpForceText).EntireColumn.NumberFormat = "@"
				End If
			End With
		End If
		VpExcelApp.Visible = True
		VpExcelApp.DisplayAlerts = True
		System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("fr-FR")
	End Sub
	Private Sub SetCheckState
	Dim VpAll As Boolean = True
	Dim VpNone As Boolean = True
		If VmBusy Then Exit Sub
		VmBusy = True
		For VpI As Integer = 0 To Me.chklstXL.Items.Count - 1
			VpAll = VpAll And Me.chklstXL.GetItemChecked(VpI)
			VpNone = VpNone And (Not Me.chklstXL.GetItemChecked(VpI))
		Next VpI
		If VpAll Then
			Me.chkAllNone.CheckState = CheckState.Checked
		ElseIf VpNone Then
			Me.chkAllNone.CheckState = CheckState.Unchecked
		Else
			Me.chkAllNone.CheckState = CheckState.Indeterminate
		End If
		VmBusy = False
	End Sub
	Private Sub CbarXLMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
		VmFormMove = True
		VmCanClose = True
		VmMousePos = New Point(e.X, e.Y)
	End Sub
	Private Sub CbarXLMouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
		If VmFormMove Then
			Me.Location = New Point(MousePosition.X - VmMousePos.X, MousePosition.Y - VmMousePos.Y)
		End If
	End Sub
	Private Sub CbarXLMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs)
		VmFormMove = False
	End Sub
	Private Sub CbarXLVisibleChanged(ByVal sender As Object, ByVal e As EventArgs)
		If VmCanClose Then
			Me.Close
		End If
	End Sub
	Sub CmdXLClick(sender As Object, e As EventArgs)
		Me.cmdXL.Enabled = False
		Call Me.ExcelGen
		If Me.chkSaveImg.Checked Then
			Call clsModule.ExtractPictures(Me.txtSaveImg.Text, VmSource, VmRestriction)
			Process.Start(clsModule.CgShell, Me.txtSaveImg.Text)
		End If
		Me.cmdXL.Enabled = True
	End Sub
	Sub BtColumnsActivate(sender As Object, e As EventArgs)
		Me.grpColumns.Visible = True
		Me.grpOptions.Visible = False
	End Sub
	Sub BtAdvanceActivate(sender As Object, e As EventArgs)
		Me.grpColumns.Visible = False
		Me.grpOptions.Visible = True
	End Sub
	Sub ChkAllNoneCheckedChanged(sender As Object, e As EventArgs)
		If VmBusy Then Exit Sub
		VmBusy = True
		For VpI As Integer = 0 To Me.chklstXL.Items.Count - 1
			Me.chklstXL.SetItemChecked(VpI, Me.chkAllNone.Checked)
		Next VpI
		VmBusy = False
	End Sub
	Sub ChkSaveImgCheckedChanged(sender As Object, e As EventArgs)
		Me.txtSaveImg.Enabled = Me.chkSaveImg.Checked
		Me.cmdSaveImg.Enabled = Me.chkSaveImg.Checked
	End Sub
	Sub CmdSaveImgClick(sender As Object, e As EventArgs)
		Me.dlgBrowse.SelectedPath = ""
		Me.dlgBrowse.ShowDialog
		If Me.dlgBrowse.SelectedPath <> "" Then
			Me.txtSaveImg.Text = Me.dlgBrowse.SelectedPath
		End If
	End Sub
	Sub ChklstXLSelectedValueChanged(sender As Object, e As EventArgs)
		Call Me.SetCheckState
	End Sub
	Sub FrmExcelLoad(sender As Object, e As EventArgs)
		'Astuce horrible pour contourner un bug de mise à l'échelle automatique en fonction de la densité de pixels
		If Me.CreateGraphics().DpiX <> 96 Then
			Me.grpColumns.Dock = DockStyle.None
			Me.grpOptions.Dock = DockStyle.None
			Me.cmdXL.Dock = DockStyle.None
		End If
	End Sub
End Class
Public Class clsXLItem
	Private VmTitle As String
	Private VmFoil As Boolean = False
	Private VmQuant As Integer
	Private VmColor As String = ""
	Private VmForceEndurance As String = ""
	Private VmInvoc As String = ""
	Private VmSerie As String = ""
	Private VmPrice As String = 0
	Private VmRarity As String = ""
	Private VmSubType As String = ""
	Private VmType As String = ""
	Private VmCardText As String = ""
	Public Sub New(VpChk As CheckedListBox, VpTitle As String, VpQuant As Integer, VpFoil As Boolean, VpColor As String, VpForce As String, VpEndurance As String, VpInvoc As String, VpSerie As String, VpPrice As String, VpRarity As String, VpSubType As String, VpType As String, VpCardText As String)
		VmTitle = VpTitle
		VmQuant = VpQuant
		'Foil ou pas
		If VpChk.GetItemChecked(0) Then
			VmFoil = VpFoil
		End If
		'Type
		If VpChk.GetItemChecked(8) Then
			VmType = clsModule.FormatTitle("Card.Type", VpType)
		End If
		'Sous-type
		If VpChk.GetItemChecked(7) Then
			VmSubType = VpSubType
		End If
		'Couleur
		If VpChk.GetItemChecked(1) Then
			VmColor = clsModule.FormatTitle("Spell.Color", VpColor)
		End If
		'Force / Endurance
		If VpChk.GetItemChecked(2) Then
			VmForceEndurance = If(VpForce = "" And VpEndurance = "", "", "'" + VpForce + " / " + VpEndurance)
		End If		
		'Coût d'invocation
		If VpChk.GetItemChecked(3) Then
			VmInvoc = VpInvoc
		End If
		'Edition
		If VpChk.GetItemChecked(4) Then
			VmSerie = VpSerie
		End If
		'Prix
		If VpChk.GetItemChecked(5) Then
			VmPrice = VpPrice
		End If
		'Rareté
		If VpChk.GetItemChecked(6) Then
			VmRarity = clsModule.FormatTitle("Card.Rarity", VpRarity)
		End If
		'Texte
		If VpChk.GetItemChecked(9) Then
			VmCardText = VpCardText
		End If
	End Sub
	Public Shared Function AreAlike(Vp1 As clsXLItem, Vp2 As clsXLItem) As Boolean
		Return ( Vp1.Foil = Vp2.Foil And Vp1.Color = Vp2.Color And Vp1.Invoc = Vp2.Invoc And Vp1.Price = Vp2.Price And Vp1.Rarity = Vp2.Rarity And Vp1.Serie = Vp2.Serie And Vp1.SubType = Vp2.SubType And Vp1.Title = Vp2.Title And Vp1.Type = Vp2.Type And Vp1.CardText = Vp2.CardText )
	End Function
	Public ReadOnly Property Title As String
		Get
			Return VmTitle
		End Get
	End Property
	Public Property Quant As Integer
		Get
			Return VmQuant
		End Get
		Set (VpQuant As Integer)
			VmQuant = VpQuant
		End Set
	End Property
	Public ReadOnly Property Foil As Boolean
		Get
			Return VmFoil
		End Get
	End Property	
	Public ReadOnly Property Color As String
		Get
			Return VmColor
		End Get
	End Property
	Public ReadOnly Property ForceEndurance As String
		Get
			Return VmForceEndurance
		End Get
	End Property
	Public ReadOnly Property Invoc As String
		Get
			Return VmInvoc
		End Get
	End Property
	Public ReadOnly Property Serie As String
		Get
			Return VmSerie
		End Get
	End Property
	Public ReadOnly Property Price As String
		Get
			Return VmPrice
		End Get
	End Property
	Public ReadOnly Property Rarity As String
		Get
			Return VmRarity
		End Get
	End Property
	Public ReadOnly Property SubType As String
		Get
			Return VmSubType
		End Get
	End Property
	Public ReadOnly Property Type As String
		Get
			Return VmType
		End Get
	End Property
	Public ReadOnly Property CardText As String
		Get
			Return VmCardText
		End Get
	End Property
End Class