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
'------------------------------------------------------
Public Partial Class frmWord
	Private VmFormMove As Boolean = False	'Formulaire en déplacement
	Private VmMousePos As Point				'Position initiale de la souris sur la barre de titre
	Private VmCanClose As Boolean = False   'Formulaire peut être fermé
	Private VmSource As String
	Private VmRestriction As String
	Private VmRestrictionTXT As String
	Private VmBusy As Boolean = False
	Public Sub New(VpOwner As MainForm)
		Me.InitializeComponent()
		VmSource = If(VpOwner.chkClassement.GetItemChecked(0), clsModule.CgSDecks, clsModule.CgSCollection)
		VmRestriction = VpOwner.Restriction
		VmRestrictionTXT = VpOwner.Restriction(True)
		If VmRestrictionTXT.Length > 31 Then
			VmRestrictionTXT = VmRestrictionTXT.Substring(0, 31)
		End If
		Me.txtSaveImg.Text = Application.StartupPath
	End Sub
	Private Sub WordGen
	'-----------------------------------------------------------------------------------------------
	'Génération de planches de vignettes sous Word correspondant aux cartes de la sélection courante
	'-----------------------------------------------------------------------------------------------
	Dim VpWordApp As Object			'Objet Word par OLE
	Dim VpDocument As Object
	Dim VpPicture As Object
	Dim VpSQL As String
	Dim VpTop As Integer
	Dim VpLeft As Integer
	Dim VpCount As Integer
	Dim VpTotal As Integer
		Try
			VpWordApp = CreateObject("Word.Application")
		Catch
			Call clsModule.ShowWarning("Aucune installation de Microsoft Word n'a été détectée sur votre système." + vbCrLf + "Impossible de continuer...")
			Exit Sub
		End Try
		'Pré-extraction des images nécessaires
		VpTotal = clsModule.ExtractPictures(Me.txtSaveImg.Text, VmSource, VmRestriction)
		Me.prgAvance.Value = 0
		Me.prgAvance.Maximum = VpTotal
		'Nouveau document
		VpWordApp.DisplayAlerts = False
		VpDocument = VpWordApp.Documents.Add
		With VpDocument.PageSetup
			.LeftMargin = VpWordApp.MillimetersToPoints(clsModule.CgXMargin)
			.RightMargin = VpWordApp.MillimetersToPoints(clsModule.CgXMargin)
			.TopMargin = VpWordApp.MillimetersToPoints(clsModule.CgYMargin)
			.BottomMargin = VpWordApp.MillimetersToPoints(clsModule.CgYMargin)
		End With
		VpWordApp.Visible = Me.chkWordShow.Checked
		'Récupération de la liste
		VpSQL = "Select Card.Title, Sum(Items) From " + VmSource + " Inner Join Card On " + VmSource + ".EncNbr = Card.EncNbr Where "
		VpSQL = VpSQL + VmRestriction
		VpSQL = clsModule.TrimQuery(VpSQL, , " Group By Card.Title")
		VgDBCommand.CommandText = VpSQL
		VgDBReader = VgDBcommand.ExecuteReader
		With VgDBReader
			VpTop = 0
			VpLeft = 0
			VpCount = 0
			While .Read
				'Remplissage
				If Me.chklstWord.CheckedItems.Contains(.GetString(0)) Then	'Vérifie que la carte courante fait partie de celles à ajouter
					Try
						For VpI As Integer = 1 To If(Me.chkSingle.Checked, 1, .GetValue(1))
							Application.DoEvents
		 					VpPicture = VpDocument.Shapes.AddPicture(Me.txtSaveImg.Text + "\" + .GetString(0).Replace(":", "").Replace("/", "").Replace("""", "").Replace("?", "") + ".jpg", False, True, 1, 1, 1, 1)
		 					VpPicture.Width = VpWordApp.MillimetersToPoints(clsModule.CgMTGCardWidth)
		 					VpPicture.Height = VpWordApp.MillimetersToPoints(clsModule.CgMTGCardHeight)
		 					VpPicture.Top = VpTop
		 					VpPicture.Left = VpLeft
		 					VpCount = VpCount + 1
		 					If VpCount Mod 9 = 0 Then
		 						VpWordApp.Selection.InsertBreak
		 						VpWordApp.Selection.MoveUp
		 						VpLeft = 0
		 						VpTop = 0
		 					ElseIf VpCount Mod 3 = 0 Then
		 						VpLeft = 0
		 						VpTop = VpTop + VpWordApp.MillimetersToPoints(clsModule.CgMTGCardHeight + clsModule.CgYMargin)
		 					Else
		 						VpLeft = VpLeft + VpWordApp.MillimetersToPoints(clsModule.CgMTGCardWidth + clsModule.CgXMargin)
		 					End If
		 				Next VpI
					Catch
						Call clsModule.ShowWarning("Un problème est survenu lors de la création de la vignette de la carte " + .GetString(0) + "...")
					End Try
	 			End If
 				Me.prgAvance.Increment(1)
			End While
			.Close
		End With
		Me.prgAvance.Value = 0
		VpWordApp.Visible = True
		VpWordApp.DisplayAlerts = True
	End Sub
	Private Sub SetCheckState
	Dim VpAll As Boolean = True
	Dim VpNone As Boolean = True
		If VmBusy Then Exit Sub
		VmBusy = True
		For VpI As Integer = 0 To Me.chklstWord.Items.Count - 1
			VpAll = VpAll And Me.chklstWord.GetItemChecked(VpI)
			VpNone = VpNone And (Not Me.chklstWord.GetItemChecked(VpI))
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
	Sub FrmWordLoad(sender As Object, e As EventArgs)
	'---------------------------
	'Chargement de la checkliste
	'---------------------------
	Dim VpSQL As String
		VpSQL = "Select Distinct Card.Title From " + VmSource + " Inner Join Card On " + VmSource + ".EncNbr = Card.EncNbr Where "
		VpSQL = VpSQL + VmRestriction
		VpSQL = clsModule.TrimQuery(VpSQL)
		VgDBCommand.CommandText = VpSQL
		VgDBReader = VgDBcommand.ExecuteReader
		With VgDBReader
			Me.chklstWord.Items.Clear
			While .Read
				Me.chklstWord.Items.Add(.GetString(0), True)
			End While
			.Close
		End With
	End Sub
	Private Sub CbarWordMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
		VmFormMove = True
		VmCanClose = True
		VmMousePos = New Point(e.X, e.Y)
	End Sub
	Private Sub CbarWordMouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
		If VmFormMove Then
			Me.Location = New Point(MousePosition.X - VmMousePos.X, MousePosition.Y - VmMousePos.Y)
		End If
	End Sub
	Private Sub CbarWordMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs)
		VmFormMove = False
	End Sub
	Private Sub CbarWordVisibleChanged(ByVal sender As Object, ByVal e As EventArgs)
		If VmCanClose Then
			Me.Close
		End If
	End Sub
	Sub CmdWordClick(sender As Object, e As EventArgs)
		Call Me.WordGen
		If Me.chkSaveImg.Checked Then
			Process.Start(clsModule.CgShell, Me.txtSaveImg.Text)
		End If
	End Sub
	Sub ChkAllNoneCheckedChanged(sender As Object, e As EventArgs)
		If VmBusy Then Exit Sub
		VmBusy = True
		For VpI As Integer = 0 To Me.chklstWord.Items.Count - 1
			Me.chklstWord.SetItemChecked(VpI, Me.chkAllNone.Checked)
		Next VpI
		VmBusy = False
	End Sub
	Sub CmdSaveImgClick(sender As Object, e As EventArgs)
		Me.dlgBrowse.SelectedPath = ""
		Me.dlgBrowse.ShowDialog
		If Me.dlgBrowse.SelectedPath <> "" Then
			Me.txtSaveImg.Text = Me.dlgBrowse.SelectedPath
		End If
	End Sub
	Sub ChklstWordSelectedValueChanged(sender As Object, e As EventArgs)
		Call Me.SetCheckState
	End Sub
	Sub BtVignettesActivate(sender As Object, e As EventArgs)
		Me.grpVignettes.Visible = True
		Me.grpOptions.Visible = False
	End Sub
	Sub BtAdvanceActivate(sender As Object, e As EventArgs)
		Me.grpVignettes.Visible = False
		Me.grpOptions.Visible = True
	End Sub
End Class