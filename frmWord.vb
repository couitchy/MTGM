'------------------------------------------------------
'| Projet         |  Magic The Gathering Manager      |
'| Contexte       |         Perso                     |
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
'| - intégration possible des coûts invoc. 26/05/2012 |
'| - intégration possible A/D et texte     03/08/2013 |
'| - gestion présence bordure / taille     01/10/2017 |
'------------------------------------------------------
Imports System.IO
Public Partial Class frmWord
    Private VmFormMove As Boolean = False   'Formulaire en déplacement
    Private VmMousePos As Point             'Position initiale de la souris sur la barre de titre
    Private VmCanClose As Boolean = False   'Formulaire peut être fermé
    Private VmSource As String
    Private VmRestriction As String
    Private VmRestrictionTXT As String
    Private VmBusy As Boolean = False
    Private Const CmImgPerPage As Integer = 9
    Private Const CmImgPerRow As Integer = 3
    Private Const CmTxtPerRowNoFullTxt As Integer = 6
    Private Const CmTxtPerRowWithFullTxt As Integer = 4
    Public Sub New(VpOwner As MainForm)
    Dim VpPath As String = Path.GetTempPath + clsModule.CgTemp
        Me.InitializeComponent()
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
    'Génération de planches de vignettes sous Word correspondant aux cartes de la sélection courante
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
            Call clsModule.ShowWarning("Aucune installation de Microsoft Word n'a été détectée sur votre système." + vbCrLf + "Impossible de continuer...")
            Exit Sub
        End Try
        'Pré-extraction des images nécessaires
        If Not VpTextOnly Then
            Call clsModule.ExtractPictures(Me.txtSaveImg.Text, VmSource, VmRestriction)
        End If
        'Nouveau document
        VpWordApp.DisplayAlerts = False
        VpDocument = VpWordApp.Documents.Add
        VpWordApp.Visible = False
        MainForm.VgMe.IsMainReaderBusy = True
        'Récupération de la liste
        VpSQL = If(Me.chkVF.Checked, "TextesFR.TexteFR", "Card.CardText")
        VpSQL = "Select " + If(Me.chkVF.Checked, "CardFR.TitleFR", "Card.Title") + ", Sum(Items), Card.Title, Spell.Cost, Creature.Power, Creature.Tough, IIf(IsNull(" + VpSQL + "), '', " + VpSQL + ") From ((((Card Inner Join CardFR On Card.EncNbr = CardFR.EncNbr) Inner Join " + VmSource + " On " + VmSource + ".EncNbr = Card.EncNbr) Inner Join Spell On Spell.Title = Card.Title) Inner Join TextesFR On Card.Title = TextesFR.CardName) Left Join Creature On Card.Title = Creature.Title Where "
        VpSQL = VpSQL + VmRestriction
        VpSQL = clsModule.TrimQuery(VpSQL, , " Group By " + If(Me.chkVF.Checked, "CardFR.TitleFR", "Card.Title") + ", Card.Title, Spell.Cost, Creature.Power, Creature.Tough, CardText, " + If(Me.chkVF.Checked, "TextesFR.TexteFR", "Card.CardText"))
        VgDBCommand.CommandText = VpSQL
        VgDBReader = VgDBcommand.ExecuteReader
        With VgDBReader
            VpTotal = 0
            While .Read
                Try
                    If Me.chklstWord.CheckedItems.Contains(.GetString(2)) Then  'Vérifie que la carte courante fait partie de celles à ajouter
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
        '1. Génération mode image
        If Not VpTextOnly Then
            'Marges minimales
            With VpDocument.PageSetup
                .LeftMargin = VpWordApp.MillimetersToPoints(clsModule.CgXMargin)
                .RightMargin = VpWordApp.MillimetersToPoints(clsModule.CgXMargin)
                .TopMargin = VpWordApp.MillimetersToPoints(clsModule.CgYMargin)
                .BottomMargin = VpWordApp.MillimetersToPoints(clsModule.CgYMargin)
            End With
            'Remplissage
            VpTop = 0
            VpLeft = 0
            VpCount = 0
            For Each VpItem As clsWordItem In VpItems
                Try
                    For VpI As Integer = 1 To VpItem.Quant
                        VpPath = Me.txtSaveImg.Text + "\" + clsModule.AvoidForbiddenChr(VpItem.TitleVO) + ".jpg"
                        VpPicture = VpDocument.Shapes.AddPicture(VpPath, False, True, 1, 1, 1, 1)
                        If Not Me.chkManageBorder.Checked OrElse clsModule.HasBorder(VpPath) Then
                            VpPicture.Width = VpWordApp.MillimetersToPoints(clsModule.CgMTGCardWidth_mm)
                            VpPicture.Height = VpWordApp.MillimetersToPoints(clsModule.CgMTGCardHeight_mm)
                        Else
                            VpPicture.Width = VpWordApp.MillimetersToPoints(clsModule.CgMTGCardWidth_crop_mm)
                            VpPicture.Height = VpWordApp.MillimetersToPoints(clsModule.CgMTGCardHeight_crop_mm)
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
                            VpTop = VpTop + VpWordApp.MillimetersToPoints(clsModule.CgMTGCardHeight_mm + clsModule.CgYMargin)
                        Else
                            VpLeft = VpLeft + VpWordApp.MillimetersToPoints(clsModule.CgMTGCardWidth_mm + clsModule.CgXMargin)
                        End If
                        Me.prgAvance.Increment(1)
                        Application.DoEvents
                    Next VpI
                Catch
                    Call clsModule.ShowWarning("Un problème est survenu lors de la création de la vignette de la carte " + VpItem.Title + "...")
                End Try
            Next VpItem
        '2. Génération mode texte
        Else
            VpTxtPerRow = If(Me.chkPrintText.Checked, CmTxtPerRowWithFullTxt, CmTxtPerRowNoFullTxt)
            'Création du tableau
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
        'Astuce horrible pour contourner un bug de mise à l'échelle automatique en fonction de la densité de pixels
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
        If Me.chklstWord.CheckedItems.Count > clsModule.CgMaxVignettes Then
            Call clsModule.ShowWarning("Le nombre de vignettes à générer est trop important..." + vbCrLf + "Maximum autorisé : " + clsModule.CgMaxVignettes.ToString + ".")
        Else
            Me.cmdWord.Enabled = False
            Application.UseWaitCursor = True
            If Me.optSaveImg.Checked Then
                Call Me.WordGen(False)
                Process.Start(clsModule.CgShell, Me.txtSaveImg.Text)
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
Public Class clsWordItem
    Private VmTitleVO As String
    Private VmTitle As String
    Private VmCost As String
    Private VmQuant As Integer
    Private VmAD As String
    Private VmFullText As String
    Public Sub New(VpTitle As String, VpTitleVO As String, VpCost As String, VpA As String, VpD As String, VpFullText As String, VpQuant As Integer)
        VmTitle = VpTitle
        VmTitleVO = VpTitleVO
        VmCost = VpCost
        VmQuant = VpQuant
        VmAD = VpA + "/" + VpD
        VmFullText = VpFullText
    End Sub
    Public ReadOnly Property Title As String
        Get
            Return VmTitle
        End Get
    End Property
    Public ReadOnly Property TitleVO As String
        Get
            Return VmTitleVO
        End Get
    End Property
    Public ReadOnly Property Cost As String
        Get
            Return VmCost
        End Get
    End Property
    Public ReadOnly Property AD As String
        Get
            Return If(VmAD = "/", "", VmAD)
        End Get
    End Property
    Public ReadOnly Property FullText As String
        Get
            Return VmFullText
        End Get
    End Property
    Public ReadOnly Property Quant As Integer
        Get
            Return VmQuant
        End Get
    End Property
End Class
