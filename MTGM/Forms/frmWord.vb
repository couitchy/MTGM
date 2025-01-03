Imports System.IO
Public Partial Class frmWord
    Private VmFormMove As Boolean = False   'Formulaire en d�placement
    Private VmMousePos As Point             'Position initiale de la souris sur la barre de titre
    Private VmCanClose As Boolean = False   'Formulaire peut �tre ferm�
    Private VmSource As String
    Private VmRestriction As String
    Private VmRestrictionTXT As String
    Private VmBusy As Boolean = False
    Private Const CmImgPerPage As Integer = 9
    Private Const CmImgPerRow As Integer = 3
    Private Const CmTxtPerRowNoFullTxt As Integer = 6
    Private Const CmTxtPerRowWithFullTxt As Integer = 4
    Public Sub New(VpOwner As MainForm)
    Dim VpPath As String = Path.GetTempPath + mdlConstGlob.CgTemp
        Call Me.InitializeComponent
        VmSource = VpOwner.MySource
        VmRestriction = VpOwner.Restriction
        VmRestrictionTXT = VpOwner.Restriction(True)
        If VmRestrictionTXT.Length > 31 Then
            VmRestrictionTXT = VmRestrictionTXT.Substring(0, 31)
        End If
        If Not Directory.Exists(VpPath) Then
            Directory.CreateDirectory(VpPath)
        End If
        Me.txtSaveImg.Text = VpPath.Replace("\\", "\")
    End Sub
    Private Sub WordGen(VpTextOnly As Boolean)
    '-----------------------------------------------------------------------------------------------
    'G�n�ration de planches de vignettes sous Word correspondant aux cartes de la s�lection courante
    '-----------------------------------------------------------------------------------------------
    Dim VpWordApp As Object         'Objet Word par OLE
    Dim VpDocument As Object
    Dim VpPicture As Object
    Dim VpPath As String
    Dim VpTable As Object
    Dim VpSQL As String
    Dim VpTop As Integer
    Dim VpLeft As Integer
    Dim VpCount As Integer
    Dim VpTotal As Integer
    Dim VpTxtPerRow As Integer
    Dim VpItems As New List(Of clsWordItem)
        Try
            VpWordApp = CreateObject("Word.Application")
        Catch
            Call mdlToolbox.ShowWarning("Aucune installation de Microsoft Word n'a �t� d�tect�e sur votre syst�me." + vbCrLf + "Impossible de continuer...")
            Exit Sub
        End Try
        'Pr�-extraction des images n�cessaires
        If Not VpTextOnly Then
            Call mdlToolbox.ExtractPictures(Me.txtSaveImg.Text, VmSource, VmRestriction)
        End If
        'Nouveau document
        VpWordApp.DisplayAlerts = False
        VpDocument = VpWordApp.Documents.Add
        VpWordApp.Visible = False
        MainForm.VgMe.IsMainReaderBusy = True
        'R�cup�ration de la liste
        VpSQL = If(Me.chkVF.Checked, "TextesFR.TexteFR", "Card.CardText")
        VpSQL = "Select " + If(Me.chkVF.Checked, "CardFR.TitleFR", "Card.Title") + ", Sum(Items), Card.Title, Spell.Cost, Creature.Power, Creature.Tough, IIf(IsNull(" + VpSQL + "), '', " + VpSQL + ") From ((((Card Inner Join CardFR On Card.EncNbr = CardFR.EncNbr) Inner Join " + VmSource + " On " + VmSource + ".EncNbr = Card.EncNbr) Inner Join Spell On Spell.Title = Card.Title) Inner Join TextesFR On Card.Title = TextesFR.CardName) Left Join Creature On Card.Title = Creature.Title Where "
        VpSQL = VpSQL + VmRestriction
        VpSQL = mdlToolbox.TrimQuery(VpSQL, , " Group By " + If(Me.chkVF.Checked, "CardFR.TitleFR", "Card.Title") + ", Card.Title, Spell.Cost, Creature.Power, Creature.Tough, CardText, " + If(Me.chkVF.Checked, "TextesFR.TexteFR", "Card.CardText"))
        VgDBCommand.CommandText = VpSQL
        VgDBReader = VgDBcommand.ExecuteReader
        With VgDBReader
            VpTotal = 0
            While .Read
                Try
                    If Me.chklstWord.CheckedItems.Contains(.GetString(2)) Then  'V�rifie que la carte courante fait partie de celles � ajouter
                        VpCount = If(Me.chkSingle.Checked, 1, .GetValue(1))
                        VpItems.Add(New clsWordItem(.GetString(0), .GetString(2), .GetValue(3).ToString, .GetValue(4).ToString, .GetValue(5).ToString, .GetString(6), VpCount))
                        VpTotal = VpTotal + VpCount
                    End If
                Catch
                End Try
            End While
            .Close
        End With
        Me.prgAvance.Maximum = VpTotal
        Me.prgAvance.Value = 0
        '1. G�n�ration mode image
        If Not VpTextOnly Then
            'Marges minimales
            With VpDocument.PageSetup
                .LeftMargin = VpWordApp.MillimetersToPoints(mdlConstGlob.CgXMargin)
                .RightMargin = VpWordApp.MillimetersToPoints(mdlConstGlob.CgXMargin)
                .TopMargin = VpWordApp.MillimetersToPoints(mdlConstGlob.CgYMargin)
                .BottomMargin = VpWordApp.MillimetersToPoints(mdlConstGlob.CgYMargin)
            End With
            'Remplissage
            VpTop = 0
            VpLeft = 0
            VpCount = 0
            For Each VpItem As clsWordItem In VpItems
                Try
                    For VpI As Integer = 1 To VpItem.Quant
                        VpPath = Me.txtSaveImg.Text + "\" + mdlToolbox.AvoidForbiddenChr(VpItem.TitleVO) + ".jpg"
                        VpPicture = VpDocument.Shapes.AddPicture(VpPath, False, True, 1, 1, 1, 1)
                        If Not Me.chkManageBorder.Checked OrElse mdlToolbox.HasBorder(VpPath) Then
                            VpPicture.Width = VpWordApp.MillimetersToPoints(mdlConstGlob.CgMTGCardWidth_mm)
                            VpPicture.Height = VpWordApp.MillimetersToPoints(mdlConstGlob.CgMTGCardHeight_mm)
                        Else
                            VpPicture.Width = VpWordApp.MillimetersToPoints(mdlConstGlob.CgMTGCardWidth_crop_mm)
                            VpPicture.Height = VpWordApp.MillimetersToPoints(mdlConstGlob.CgMTGCardHeight_crop_mm)
                        End If
                        VpPicture.Top = VpTop
                        VpPicture.Left = VpLeft
                        VpCount = VpCount + 1
                        If VpCount Mod CmImgPerPage = 0 Then        '9 vignettes par page
                            VpWordApp.Selection.GoTo(What := 1, Which := 1)
                            VpWordApp.Selection.InsertBreak
                            VpWordApp.Selection.GoTo(What := 1, Which := 1)
                            VpWordApp.Selection.GoTo(What := 3, Which := 1)
                            VpLeft = 0
                            VpTop = 0
                        ElseIf VpCount Mod CmImgPerRow = 0 Then     '3 vignettes par ligne
                            VpLeft = 0
                            VpTop = VpTop + VpWordApp.MillimetersToPoints(mdlConstGlob.CgMTGCardHeight_mm + mdlConstGlob.CgYMargin)
                        Else
                            VpLeft = VpLeft + VpWordApp.MillimetersToPoints(mdlConstGlob.CgMTGCardWidth_mm + mdlConstGlob.CgXMargin)
                        End If
                        Me.prgAvance.Increment(1)
                        Application.DoEvents
                    Next VpI
                Catch
                    Call mdlToolbox.ShowWarning("Un probl�me est survenu lors de la cr�ation de la vignette de la carte " + VpItem.Title + "...")
                End Try
            Next VpItem
        '2. G�n�ration mode texte
        Else
            VpTxtPerRow = If(Me.chkPrintText.Checked, CmTxtPerRowWithFullTxt, CmTxtPerRowNoFullTxt)
            'Cr�ation du tableau
            VpTable = VpDocument.Tables.Add(VpDocument.Range(0, 0), Math.Ceiling(VpTotal / VpTxtPerRow), VpTxtPerRow, 1, 0)
            'Remplissage
            VpCount = 0
            For Each VpItem As clsWordItem In VpItems
                For VpI As Integer = 1 To VpItem.Quant
                    '6 (ou 4) vignettes par ligne
                    VpTable.Cell(1 + (VpCount \ VpTxtPerRow), 1 + (VpCount Mod VpTxtPerRow)).Range.Text = If(Me.chkPrintCost.Checked, VpItem.Cost + vbCrLf, "") + VpItem.Title + If(Me.chkPrintText.Checked, vbCrLf + vbCrLf + VpItem.FullText + vbCrLf, "") + If(Me.chkPrintAD.Checked, vbCrLf + VpItem.AD, "") + If(Me.chkPrintText.Checked, vbCrLf, "")
                    VpCount = VpCount + 1
                    Me.prgAvance.Increment(1)
                    Application.DoEvents
                Next VpI
            Next VpItem
            'Formatage
            For Each VpBorder As Object In VpTable.Borders
                VpBorder.LineStyle = 0
            Next VpBorder
            VpTable.Borders.Shadow = False
            VpTable.TopPadding = VpWordApp.CentimetersToPoints(0.6)
            VpTable.LeftPadding = VpWordApp.CentimetersToPoints(0.3)
            VpTable.RightPadding = VpWordApp.CentimetersToPoints(0.3)
        End If
        Me.prgAvance.Value = 0
        MainForm.VgMe.IsMainReaderBusy = False
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
        VpSQL = mdlToolbox.TrimQuery(VpSQL)
        VgDBCommand.CommandText = VpSQL
        VgDBReader = VgDBcommand.ExecuteReader
        With VgDBReader
            Me.chklstWord.Items.Clear
            While .Read
                Me.chklstWord.Items.Add(.GetString(0), True)
            End While
            .Close
        End With
        'Astuce horrible pour contourner un bug de mise � l'�chelle automatique en fonction de la densit� de pixels
        If Me.CreateGraphics().DpiX <> 96 Then
            Me.grpVignettes.Dock = DockStyle.None
            Me.grpOptions.Dock = DockStyle.None
            Me.chklstWord.Dock = DockStyle.None
            Me.chkAllNone.Dock = DockStyle.None
            Me.chkAllNone.BringToFront
            Me.cmdWord.Dock = DockStyle.None
            Me.cmdWord.BringToFront
        End If
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
        If Me.chklstWord.CheckedItems.Count > mdlConstGlob.CgMaxVignettes Then
            Call mdlToolbox.ShowWarning("Le nombre de vignettes � g�n�rer est trop important..." + vbCrLf + "Maximum autoris� : " + mdlConstGlob.CgMaxVignettes.ToString + ".")
        Else
            Me.cmdWord.Enabled = False
            Application.UseWaitCursor = True
            If Me.optSaveImg.Checked Then
                Call Me.WordGen(False)
                Process.Start(mdlConstGlob.CgShell, Me.txtSaveImg.Text)
            Else
                Call Me.WordGen(True)
            End If
            Application.UseWaitCursor = False
            Me.cmdWord.Enabled = True
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
