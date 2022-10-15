'
' Crée par SharpDevelop.
' Utilisateur: Couitchy
' Date: 18/01/2011
' Heure: 20:32
'
' Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
'
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    ''' <summary>
    ''' Designer variable used to keep track of non-visual components.
    ''' </summary>
    Private components As System.ComponentModel.IContainer

    ''' <summary>
    ''' Disposes resources used by the form.
    ''' </summary>
    ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If components IsNot Nothing Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    ''' <summary>
    ''' This method is required for Windows Forms designer support.
    ''' Do not change the method contents inside the source code editor. The Forms designer might
    ''' not be able to load this method if it was changed manually.
    ''' </summary>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.menuStrip = New System.Windows.Forms.MenuStrip()
        Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuDBOpen = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuDBReady = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuINIReady = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSeparator = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTools = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCards = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCardsExtract = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCardsExtractAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCardsExtractDiff = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCardsExtractDiff3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCardsExtractDiff4 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCardsExtractDiff2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCardsExtractDiff5 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTrad = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCardsTradTxt = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuExtractTexts = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFilterTitles = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuBuildTitles = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCheckTrad = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCompareTrad = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuListSubtypes = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCompareTitles = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCardsTradTitles = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCardsTradTitlesFilter = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCardsAut = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCardsAutAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCardsAutListe = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCardsAutMerge = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCardReplaceTitle = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFindHoles = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSeries = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSeriesSpoilers = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSeriesMerge = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSeriesVirtualAdd = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSeriesGen = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSeriesGenR14 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSeriesGenR16 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuBuildStamps = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuBuildDouble = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFixTxtVO = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPrices = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPricesUpdate = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPricesUpdateAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPricesUpdateListe = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPricesHistoryAdd = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuBuildPatch = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuGetShippingCosts = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPictures = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPicturesSymbols = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPicturesThumbs = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPicturesUpdate = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPicturesRemove = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPicturesFix = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPicturesDelta = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPicturesNewSP = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPicturesRevertSP = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAbout = New System.Windows.Forms.ToolStripMenuItem()
        Me.toolStrip = New System.Windows.Forms.ToolStrip()
        Me.btDBOpen = New System.Windows.Forms.ToolStripButton()
        Me.btPricesUpdate = New System.Windows.Forms.ToolStripButton()
        Me.btPricesHistoryAdd = New System.Windows.Forms.ToolStripButton()
        Me.btPicturesFix = New System.Windows.Forms.ToolStripButton()
        Me.btCancel = New System.Windows.Forms.ToolStripButton()
        Me.btReplaceTitle = New System.Windows.Forms.ToolStripButton()
        Me.statusStrip = New System.Windows.Forms.StatusStrip()
        Me.prgAvance = New System.Windows.Forms.ProgressBar()
        Me.tabMain = New System.Windows.Forms.TabControl()
        Me.tabStatus = New System.Windows.Forms.TabPage()
        Me.lvwLog = New System.Windows.Forms.ListView()
        Me.colDate = New System.Windows.Forms.ColumnHeader()
        Me.colEvent = New System.Windows.Forms.ColumnHeader()
        Me.imgLst = New System.Windows.Forms.ImageList(Me.components)
        Me.tabBrowser = New System.Windows.Forms.TabPage()
        Me.wbMV = New System.Windows.Forms.WebBrowser()
        Me.tabInfo = New System.Windows.Forms.TabPage()
        Me.txtETA = New System.Windows.Forms.TextBox()
        Me.lbl2 = New System.Windows.Forms.Label()
        Me.txtCur = New System.Windows.Forms.TextBox()
        Me.lbl1 = New System.Windows.Forms.Label()
        Me.dlgOpen = New System.Windows.Forms.OpenFileDialog()
        Me.dlgSave = New System.Windows.Forms.SaveFileDialog()
        Me.dlgOpen2 = New System.Windows.Forms.OpenFileDialog()
        Me.dlgOpen3 = New System.Windows.Forms.OpenFileDialog()
        Me.dlgBrowse = New System.Windows.Forms.FolderBrowserDialog()
        Me.dlgOpen4 = New System.Windows.Forms.OpenFileDialog()
        Me.dlgOpen5 = New System.Windows.Forms.OpenFileDialog()
        Me.dlgOpen6 = New System.Windows.Forms.OpenFileDialog()
        Me.dlgSave2 = New System.Windows.Forms.SaveFileDialog()
        Me.dlgSave3 = New System.Windows.Forms.SaveFileDialog()
        Me.mnuPricesHistoryRebuild = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCardsRulingsFilter = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCardsExtractMultiverseId = New System.Windows.Forms.ToolStripMenuItem()
        Me.menuStrip.SuspendLayout
        Me.toolStrip.SuspendLayout
        Me.tabMain.SuspendLayout
        Me.tabStatus.SuspendLayout
        Me.tabBrowser.SuspendLayout
        Me.tabInfo.SuspendLayout
        Me.SuspendLayout
        '
        'menuStrip
        '
        Me.menuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile, Me.mnuTools, Me.mnuHelp})
        Me.menuStrip.Location = New System.Drawing.Point(0, 0)
        Me.menuStrip.Name = "menuStrip"
        Me.menuStrip.Size = New System.Drawing.Size(670, 24)
        Me.menuStrip.TabIndex = 0
        Me.menuStrip.Text = "menuStrip1"
        '
        'mnuFile
        '
        Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuDBOpen, Me.mnuDBReady, Me.mnuINIReady, Me.mnuBuildStamps, Me.mnuSeparator, Me.mnuExit})
        Me.mnuFile.Name = "mnuFile"
        Me.mnuFile.Size = New System.Drawing.Size(54, 20)
        Me.mnuFile.Text = "Fichier"
        '
        'mnuDBOpen
        '
        Me.mnuDBOpen.Image = CType(resources.GetObject("mnuDBOpen.Image"),System.Drawing.Image)
        Me.mnuDBOpen.Name = "mnuDBOpen"
        Me.mnuDBOpen.Size = New System.Drawing.Size(239, 22)
        Me.mnuDBOpen.Text = "Base de données source"
        AddHandler Me.mnuDBOpen.Click, AddressOf Me.MnuDBOpenClick
        '
        'mnuDBReady
        '
        Me.mnuDBReady.Image = CType(resources.GetObject("mnuDBReady.Image"),System.Drawing.Image)
        Me.mnuDBReady.Name = "mnuDBReady"
        Me.mnuDBReady.Size = New System.Drawing.Size(239, 22)
        Me.mnuDBReady.Text = "Préparer la base pour la Release"
        AddHandler Me.mnuDBReady.Click, AddressOf Me.MnuDBReadyClick
        '
        'mnuINIReady
        '
        Me.mnuINIReady.Image = CType(resources.GetObject("mnuINIReady.Image"),System.Drawing.Image)
        Me.mnuINIReady.Name = "mnuINIReady"
        Me.mnuINIReady.Size = New System.Drawing.Size(239, 22)
        Me.mnuINIReady.Text = "Préparer le .INI pour la Release"
        AddHandler Me.mnuINIReady.Click, AddressOf Me.MnuINIReadyClick
        '
        'mnuSeparator
        '
        Me.mnuSeparator.Name = "mnuSeparator"
        Me.mnuSeparator.Size = New System.Drawing.Size(236, 6)
        '
        'mnuExit
        '
        Me.mnuExit.Image = CType(resources.GetObject("mnuExit.Image"),System.Drawing.Image)
        Me.mnuExit.Name = "mnuExit"
        Me.mnuExit.Size = New System.Drawing.Size(239, 22)
        Me.mnuExit.Text = "Quitter"
        AddHandler Me.mnuExit.Click, AddressOf Me.MnuExitClick
        '
        'mnuTools
        '
        Me.mnuTools.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuCards, Me.mnuSeries, Me.mnuPrices, Me.mnuPictures})
        Me.mnuTools.Name = "mnuTools"
        Me.mnuTools.Size = New System.Drawing.Size(50, 20)
        Me.mnuTools.Text = "Outils"
        '
        'mnuCards
        '
        Me.mnuCards.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuCardsExtract, Me.mnuTrad, Me.mnuCardsAut, Me.mnuCardReplaceTitle, Me.mnuCardsRulingsFilter, Me.mnuCardsExtractMultiverseId})
        Me.mnuCards.Name = "mnuCards"
        Me.mnuCards.Size = New System.Drawing.Size(152, 22)
        Me.mnuCards.Text = "Cartes"
        '
        'mnuCardsExtract
        '
        Me.mnuCardsExtract.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuCardsExtractAll, Me.mnuCardsExtractDiff, Me.mnuCardsExtractDiff3, Me.mnuCardsExtractDiff4, Me.mnuCardsExtractDiff2, Me.mnuCardsExtractDiff5})
        Me.mnuCardsExtract.Image = CType(resources.GetObject("mnuCardsExtract.Image"),System.Drawing.Image)
        Me.mnuCardsExtract.Name = "mnuCardsExtract"
        Me.mnuCardsExtract.Size = New System.Drawing.Size(278, 22)
        Me.mnuCardsExtract.Text = "Extraire la liste des noms des cartes"
        '
        'mnuCardsExtractAll
        '
        Me.mnuCardsExtractAll.Name = "mnuCardsExtractAll"
        Me.mnuCardsExtractAll.Size = New System.Drawing.Size(406, 22)
        Me.mnuCardsExtractAll.Text = "toutes"
        AddHandler Me.mnuCardsExtractAll.Click, AddressOf Me.MnuCardsExtractAllClick
        '
        'mnuCardsExtractDiff
        '
        Me.mnuCardsExtractDiff.Name = "mnuCardsExtractDiff"
        Me.mnuCardsExtractDiff.Size = New System.Drawing.Size(406, 22)
        Me.mnuCardsExtractDiff.Text = "celles sans image associée"
        AddHandler Me.mnuCardsExtractDiff.Click, AddressOf Me.MnuCardsExtractDiffClick
        '
        'mnuCardsExtractDiff3
        '
        Me.mnuCardsExtractDiff3.Name = "mnuCardsExtractDiff3"
        Me.mnuCardsExtractDiff3.Size = New System.Drawing.Size(406, 22)
        Me.mnuCardsExtractDiff3.Text = "celles sans traduction de leur texte en VF (hors texte vide)"
        AddHandler Me.mnuCardsExtractDiff3.Click, AddressOf Me.MnuCardsExtractDiff3Click
        '
        'mnuCardsExtractDiff4
        '
        Me.mnuCardsExtractDiff4.Name = "mnuCardsExtractDiff4"
        Me.mnuCardsExtractDiff4.Size = New System.Drawing.Size(406, 22)
        Me.mnuCardsExtractDiff4.Text = "celles sans autorisations tournois indiquées"
        AddHandler Me.mnuCardsExtractDiff4.Click, AddressOf Me.MnuCardsExtractDiff4Click
        '
        'mnuCardsExtractDiff2
        '
        Me.mnuCardsExtractDiff2.Name = "mnuCardsExtractDiff2"
        Me.mnuCardsExtractDiff2.Size = New System.Drawing.Size(406, 22)
        Me.mnuCardsExtractDiff2.Text = "celles ayant un titre VO/VF identique (hors éditions VO exclusives)"
        AddHandler Me.mnuCardsExtractDiff2.Click, AddressOf Me.MnuCardsExtractDiff2Click
        '
        'mnuCardsExtractDiff5
        '
        Me.mnuCardsExtractDiff5.Name = "mnuCardsExtractDiff5"
        Me.mnuCardsExtractDiff5.Size = New System.Drawing.Size(406, 22)
        Me.mnuCardsExtractDiff5.Text = "celles sans mise à jour de prix (qui a échoué)"
        AddHandler Me.mnuCardsExtractDiff5.Click, AddressOf Me.MnuCardsExtractDiff5Click
        '
        'mnuTrad
        '
        Me.mnuTrad.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuCardsTradTxt, Me.mnuExtractTexts, Me.mnuFilterTitles, Me.mnuBuildTitles, Me.mnuCheckTrad, Me.mnuCompareTrad, Me.mnuListSubtypes, Me.mnuCompareTitles, Me.mnuCardsTradTitles, Me.mnuCardsTradTitlesFilter})
        Me.mnuTrad.Image = CType(resources.GetObject("mnuTrad.Image"),System.Drawing.Image)
        Me.mnuTrad.Name = "mnuTrad"
        Me.mnuTrad.Size = New System.Drawing.Size(278, 22)
        Me.mnuTrad.Text = "Traduction"
        '
        'mnuCardsTradTxt
        '
        Me.mnuCardsTradTxt.Name = "mnuCardsTradTxt"
        Me.mnuCardsTradTxt.Size = New System.Drawing.Size(398, 22)
        Me.mnuCardsTradTxt.Text = "Récupération des textes VF pour les nouvelles cartes"
        AddHandler Me.mnuCardsTradTxt.Click, AddressOf Me.MnuCardsTradTxtClick
        '
        'mnuExtractTexts
        '
        Me.mnuExtractTexts.Name = "mnuExtractTexts"
        Me.mnuExtractTexts.Size = New System.Drawing.Size(398, 22)
        Me.mnuExtractTexts.Text = "Extraction de tous les textes VF"
        AddHandler Me.mnuExtractTexts.Click, AddressOf Me.MnuExtractTextsClick
        '
        'mnuFilterTitles
        '
        Me.mnuFilterTitles.Name = "mnuFilterTitles"
        Me.mnuFilterTitles.Size = New System.Drawing.Size(398, 22)
        Me.mnuFilterTitles.Text = "Extraction des titres VF pour une nouvelle édition"
        AddHandler Me.mnuFilterTitles.Click, AddressOf Me.MnuFilterTitlesClick
        '
        'mnuBuildTitles
        '
        Me.mnuBuildTitles.Name = "mnuBuildTitles"
        Me.mnuBuildTitles.Size = New System.Drawing.Size(398, 22)
        Me.mnuBuildTitles.Text = "Construire un fichier de titres VF pour une édition depuis la base"
        AddHandler Me.mnuBuildTitles.Click, AddressOf Me.MnuBuildTitlesClick
        '
        'mnuCheckTrad
        '
        Me.mnuCheckTrad.Name = "mnuCheckTrad"
        Me.mnuCheckTrad.Size = New System.Drawing.Size(398, 22)
        Me.mnuCheckTrad.Text = "Vérifier la cohérence d'un fichier de traduction"
        AddHandler Me.mnuCheckTrad.Click, AddressOf Me.MnuCheckTradClick
        '
        'mnuCompareTrad
        '
        Me.mnuCompareTrad.Name = "mnuCompareTrad"
        Me.mnuCompareTrad.Size = New System.Drawing.Size(398, 22)
        Me.mnuCompareTrad.Text = "Comparaison interactive de traductions"
        AddHandler Me.mnuCompareTrad.Click, AddressOf Me.MnuCompareTradClick
        '
        'mnuListSubtypes
        '
        Me.mnuListSubtypes.Name = "mnuListSubtypes"
        Me.mnuListSubtypes.Size = New System.Drawing.Size(398, 22)
        Me.mnuListSubtypes.Text = "Lister les sous-types non traduits"
        AddHandler Me.mnuListSubtypes.Click, AddressOf Me.MnuListSubtypesClick
        '
        'mnuCompareTitles
        '
        Me.mnuCompareTitles.Name = "mnuCompareTitles"
        Me.mnuCompareTitles.Size = New System.Drawing.Size(398, 22)
        Me.mnuCompareTitles.Text = "Comparaison différentielle de titres VF"
        AddHandler Me.mnuCompareTitles.Click, AddressOf Me.MnuCompareTitlesClick
        '
        'mnuCardsTradTitles
        '
        Me.mnuCardsTradTitles.Name = "mnuCardsTradTitles"
        Me.mnuCardsTradTitles.Size = New System.Drawing.Size(398, 22)
        Me.mnuCardsTradTitles.Text = "Traductions de titres VF"
        AddHandler Me.mnuCardsTradTitles.Click, AddressOf Me.MnuCardsTradTitlesClick
        '
        'mnuCardsTradTitlesFilter
        '
        Me.mnuCardsTradTitlesFilter.Name = "mnuCardsTradTitlesFilter"
        Me.mnuCardsTradTitlesFilter.Size = New System.Drawing.Size(398, 22)
        Me.mnuCardsTradTitlesFilter.Text = "Filtrer un fichier de traductions (suppression des titres VO/VF identiques)"
        AddHandler Me.mnuCardsTradTitlesFilter.Click, AddressOf Me.MnuCardsTradTitlesFilterClick
        '
        'mnuCardsAut
        '
        Me.mnuCardsAut.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuCardsAutAll, Me.mnuCardsAutListe, Me.mnuCardsAutMerge})
        Me.mnuCardsAut.Image = CType(resources.GetObject("mnuCardsAut.Image"),System.Drawing.Image)
        Me.mnuCardsAut.Name = "mnuCardsAut"
        Me.mnuCardsAut.Size = New System.Drawing.Size(278, 22)
        Me.mnuCardsAut.Text = "Autorisations en tournois"
        '
        'mnuCardsAutAll
        '
        Me.mnuCardsAutAll.Name = "mnuCardsAutAll"
        Me.mnuCardsAutAll.Size = New System.Drawing.Size(110, 22)
        Me.mnuCardsAutAll.Text = "Récupérer Toutes"
        AddHandler Me.mnuCardsAutAll.Click, AddressOf Me.MnuCardsAutClick
        '
        'mnuCardsAutListe
        '
        Me.mnuCardsAutListe.Name = "mnuCardsAutListe"
        Me.mnuCardsAutListe.Size = New System.Drawing.Size(110, 22)
        Me.mnuCardsAutListe.Text = "Récupérer Liste"
        AddHandler Me.mnuCardsAutListe.Click, AddressOf Me.MnuCardsAutListeClick
        '
        'mnuCardsAutMerge
        '
        Me.mnuCardsAutMerge.Name = "mnuCardsAutMerge"
        Me.mnuCardsAutMerge.Size = New System.Drawing.Size(110, 22)
        Me.mnuCardsAutMerge.Text = "Fusionner 2 fichiers d'autorisations"
        AddHandler Me.mnuCardsAutMerge.Click, AddressOf Me.MnuCardsAutMergeClick
        '
        'mnuCardReplaceTitle
        '
        Me.mnuCardReplaceTitle.Image = CType(resources.GetObject("mnuCardReplaceTitle.Image"),System.Drawing.Image)
        Me.mnuCardReplaceTitle.Name = "mnuCardReplaceTitle"
        Me.mnuCardReplaceTitle.Size = New System.Drawing.Size(278, 22)
        Me.mnuCardReplaceTitle.Text = "Remplacer un nom"
        AddHandler Me.mnuCardReplaceTitle.Click, AddressOf Me.MnuCardReplaceTitleClick
        '
        'mnuFindHoles
        '
        Me.mnuFindHoles.Name = "mnuFindHoles"
        Me.mnuFindHoles.Size = New System.Drawing.Size(278, 22)
        Me.mnuFindHoles.Text = "Trouve-trous (expérimental)"
        AddHandler Me.mnuFindHoles.Click, AddressOf Me.MnuFindHolesClick
        '
        'mnuSeries
        '
        Me.mnuSeries.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSeriesSpoilers, Me.mnuSeriesMerge, Me.mnuSeriesVirtualAdd, Me.mnuSeriesGen, Me.mnuBuildDouble, Me.mnuFixTxtVO, Me.mnuFindHoles})
        Me.mnuSeries.Name = "mnuSeries"
        Me.mnuSeries.Size = New System.Drawing.Size(152, 22)
        Me.mnuSeries.Text = "Editions"
        '
        'mnuSeriesSpoilers
        '
        Me.mnuSeriesSpoilers.Name = "mnuSeriesSpoilers"
        Me.mnuSeriesSpoilers.Size = New System.Drawing.Size(432, 22)
        Me.mnuSeriesSpoilers.Text = "Récupérer les spoilers"
        AddHandler Me.mnuSeriesSpoilers.Click, AddressOf Me.MnuSeriesSpoilersClick
        '
        'mnuSeriesMerge
        '
        Me.mnuSeriesMerge.Name = "mnuSeriesMerge"
        Me.mnuSeriesMerge.Size = New System.Drawing.Size(432, 22)
        Me.mnuSeriesMerge.Text = "Fusionner des spoilers"
        AddHandler Me.mnuSeriesMerge.Click, AddressOf Me.MnuSeriesMergeClick
        '
        'mnuSeriesVirtualAdd
        '
        Me.mnuSeriesVirtualAdd.Name = "mnuSeriesVirtualAdd"
        Me.mnuSeriesVirtualAdd.Size = New System.Drawing.Size(432, 22)
        Me.mnuSeriesVirtualAdd.Text = "Simuler l'ajout d'éditions"
        AddHandler Me.mnuSeriesVirtualAdd.Click, AddressOf Me.MnuSeriesVirtualAddClick
        '
        'mnuSeriesGen
        '
        Me.mnuSeriesGen.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSeriesGenR14, Me.mnuSeriesGenR16})
        Me.mnuSeriesGen.Image = CType(resources.GetObject("mnuSeriesGen.Image"),System.Drawing.Image)
        Me.mnuSeriesGen.Name = "mnuSeriesGen"
        Me.mnuSeriesGen.Size = New System.Drawing.Size(432, 22)
        Me.mnuSeriesGen.Text = "Générer le fichier d'en-têtes"
        '
        'mnuSeriesGenR14
        '
        Me.mnuSeriesGenR14.Name = "mnuSeriesGenR14"
        Me.mnuSeriesGenR14.Size = New System.Drawing.Size(93, 22)
        Me.mnuSeriesGenR14.Text = "R14"
        AddHandler Me.mnuSeriesGenR14.Click, AddressOf Me.MnuSeriesGenR14Click
        '
        'mnuSeriesGenR16
        '
        Me.mnuSeriesGenR16.Name = "mnuSeriesGenR16"
        Me.mnuSeriesGenR16.Size = New System.Drawing.Size(93, 22)
        Me.mnuSeriesGenR16.Text = "R20"
        AddHandler Me.mnuSeriesGenR16.Click, AddressOf Me.MnuSeriesGenR16Click
        '
        'mnuBuildStamps
        '
        Me.mnuBuildStamps.Image = CType(resources.GetObject("mnuINIReady.Image"),System.Drawing.Image)
        Me.mnuBuildStamps.Name = "mnuBuildStamps"
        Me.mnuBuildStamps.Size = New System.Drawing.Size(432, 22)
        Me.mnuBuildStamps.Text = "Préparer les .TXT d'horodatage"
        AddHandler Me.mnuBuildStamps.Click, AddressOf Me.MnuBuildStampsClick
        '
        'mnuBuildDouble
        '
        Me.mnuBuildDouble.Name = "mnuBuildDouble"
        Me.mnuBuildDouble.Size = New System.Drawing.Size(432, 22)
        Me.mnuBuildDouble.Text = "Construire le fichier des doubles cartes pour une édition depuis la base"
        AddHandler Me.mnuBuildDouble.Click, AddressOf Me.MnuBuildDoubleClick
        '
        'mnuFixTxtVO
        '
        Me.mnuFixTxtVO.Name = "mnuFixTxtVO"
        Me.mnuFixTxtVO.Size = New System.Drawing.Size(432, 22)
        Me.mnuFixTxtVO.Text = "Corriger les textes VO multilignes"
        AddHandler Me.mnuFixTxtVO.Click, AddressOf Me.MnuFixTxtVOClick
        '
        'mnuPrices
        '
        Me.mnuPrices.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuPricesUpdate, Me.mnuPricesHistoryAdd, Me.mnuPricesHistoryRebuild, Me.mnuBuildPatch, Me.mnuGetShippingCosts})
        Me.mnuPrices.Name = "mnuPrices"
        Me.mnuPrices.Size = New System.Drawing.Size(152, 22)
        Me.mnuPrices.Text = "Prix"
        '
        'mnuPricesUpdate
        '
        Me.mnuPricesUpdate.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuPricesUpdateAll, Me.mnuPricesUpdateListe})
        Me.mnuPricesUpdate.Image = CType(resources.GetObject("mnuPricesUpdate.Image"),System.Drawing.Image)
        Me.mnuPricesUpdate.Name = "mnuPricesUpdate"
        Me.mnuPricesUpdate.Size = New System.Drawing.Size(283, 22)
        Me.mnuPricesUpdate.Text = "Récupérer les prix"
        '
        'mnuPricesUpdateAll
        '
        Me.mnuPricesUpdateAll.Name = "mnuPricesUpdateAll"
        Me.mnuPricesUpdateAll.Size = New System.Drawing.Size(100, 22)
        Me.mnuPricesUpdateAll.Text = "Tous"
        AddHandler Me.mnuPricesUpdateAll.Click, AddressOf Me.MnuPricesUpdateClick
        '
        'mnuPricesUpdateListe
        '
        Me.mnuPricesUpdateListe.Name = "mnuPricesUpdateListe"
        Me.mnuPricesUpdateListe.Size = New System.Drawing.Size(100, 22)
        Me.mnuPricesUpdateListe.Text = "Liste"
        AddHandler Me.mnuPricesUpdateListe.Click, AddressOf Me.MnuPricesUpdateListeClick
        '
        'mnuPricesHistoryAdd
        '
        Me.mnuPricesHistoryAdd.Image = CType(resources.GetObject("mnuPricesHistoryAdd.Image"),System.Drawing.Image)
        Me.mnuPricesHistoryAdd.Name = "mnuPricesHistoryAdd"
        Me.mnuPricesHistoryAdd.Size = New System.Drawing.Size(283, 22)
        Me.mnuPricesHistoryAdd.Text = "Mettre à jour l'historique de la base"
        AddHandler Me.mnuPricesHistoryAdd.Click, AddressOf Me.MnuPricesHistoryAddClick
        '
        'mnuBuildPatch
        '
        Me.mnuBuildPatch.Image = CType(resources.GetObject("mnuBuildPatch.Image"),System.Drawing.Image)
        Me.mnuBuildPatch.Name = "mnuBuildPatch"
        Me.mnuBuildPatch.Size = New System.Drawing.Size(283, 22)
        Me.mnuBuildPatch.Text = "Construction du patch r13"
        AddHandler Me.mnuBuildPatch.Click, AddressOf Me.MnuBuildPatchClick
        '
        'mnuGetShippingCosts
        '
        Me.mnuGetShippingCosts.Name = "mnuGetShippingCosts"
        Me.mnuGetShippingCosts.Size = New System.Drawing.Size(283, 22)
        Me.mnuGetShippingCosts.Text = "Récupération des frais de port (MKM)"
        AddHandler Me.mnuGetShippingCosts.Click, AddressOf Me.MnuGetShippingCostsClick
        '
        'mnuPictures
        '
        Me.mnuPictures.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuPicturesSymbols, Me.mnuPicturesThumbs, Me.mnuPicturesUpdate, Me.mnuPicturesRemove, Me.mnuPicturesFix, Me.mnuPicturesDelta, Me.mnuPicturesNewSP, Me.mnuPicturesRevertSP})
        Me.mnuPictures.Name = "mnuPictures"
        Me.mnuPictures.Size = New System.Drawing.Size(152, 22)
        Me.mnuPictures.Text = "Images"
        '
        'mnuPicturesSymbols
        '
        Me.mnuPicturesSymbols.Name = "mnuPicturesSymbols"
        Me.mnuPicturesSymbols.Size = New System.Drawing.Size(316, 22)
        Me.mnuPicturesSymbols.Text = "Récupérer les symboles des éditions"
        AddHandler Me.mnuPicturesSymbols.Click, AddressOf Me.MnuPicturesSymbolsClick
        '
        'mnuPicturesThumbs
        '
        Me.mnuPicturesThumbs.Name = "mnuPicturesThumbs"
        Me.mnuPicturesThumbs.Size = New System.Drawing.Size(316, 22)
        Me.mnuPicturesThumbs.Text = "Récupérer les miniatures des éditions"
        AddHandler Me.mnuPicturesThumbs.Click, AddressOf Me.MnuPicturesThumbsClick
        '
        'mnuPicturesUpdate
        '
        Me.mnuPicturesUpdate.Image = CType(resources.GetObject("mnuPicturesUpdate.Image"),System.Drawing.Image)
        Me.mnuPicturesUpdate.Name = "mnuPicturesUpdate"
        Me.mnuPicturesUpdate.Size = New System.Drawing.Size(316, 22)
        Me.mnuPicturesUpdate.Text = "Récupérer les images"
        AddHandler Me.mnuPicturesUpdate.Click, AddressOf Me.MnuPicturesUpdateClick
        '
        'mnuPicturesRemove
        '
        Me.mnuPicturesRemove.Name = "mnuPicturesRemove"
        Me.mnuPicturesRemove.Size = New System.Drawing.Size(316, 22)
        Me.mnuPicturesRemove.Text = "Supprimer les images doublons"
        AddHandler Me.mnuPicturesRemove.Click, AddressOf Me.MnuPicturesRemoveClick
        '
        'mnuPicturesFix
        '
        Me.mnuPicturesFix.Image = CType(resources.GetObject("mnuPicturesFix.Image"),System.Drawing.Image)
        Me.mnuPicturesFix.Name = "mnuPicturesFix"
        Me.mnuPicturesFix.Size = New System.Drawing.Size(316, 22)
        Me.mnuPicturesFix.Text = "Corriger les images"
        AddHandler Me.mnuPicturesFix.Click, AddressOf Me.MnuPicturesFixClick
        '
        'mnuPicturesDelta
        '
        Me.mnuPicturesDelta.Image = CType(resources.GetObject("mnuPicturesDelta.Image"),System.Drawing.Image)
        Me.mnuPicturesDelta.Name = "mnuPicturesDelta"
        Me.mnuPicturesDelta.Size = New System.Drawing.Size(316, 22)
        Me.mnuPicturesDelta.Text = "Extraire les images modifiées"
        AddHandler Me.mnuPicturesDelta.Click, AddressOf Me.MnuPicturesDeltaClick
        '
        'mnuPicturesNewSP
        '
        Me.mnuPicturesNewSP.Image = CType(resources.GetObject("mnuPicturesNewSP.Image"),System.Drawing.Image)
        Me.mnuPicturesNewSP.Name = "mnuPicturesNewSP"
        Me.mnuPicturesNewSP.Size = New System.Drawing.Size(316, 22)
        Me.mnuPicturesNewSP.Text = "Assembler un nouveau Service Pack"
        AddHandler Me.mnuPicturesNewSP.Click, AddressOf Me.MnuPicturesNewSPClick
        '
        'mnuPicturesRevertSP
        '
        Me.mnuPicturesRevertSP.Name = "mnuPicturesRevertSP"
        Me.mnuPicturesRevertSP.Size = New System.Drawing.Size(316, 22)
        Me.mnuPicturesRevertSP.Text = "Reconstruire jusqu'à un Service Pack antérieur"
        AddHandler Me.mnuPicturesRevertSP.Click, AddressOf Me.MnuPicturesRevertSPClick
        '
        'mnuHelp
        '
        Me.mnuHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAbout})
        Me.mnuHelp.Name = "mnuHelp"
        Me.mnuHelp.Size = New System.Drawing.Size(24, 20)
        Me.mnuHelp.Text = "?"
        '
        'mnuAbout
        '
        Me.mnuAbout.Image = CType(resources.GetObject("mnuAbout.Image"),System.Drawing.Image)
        Me.mnuAbout.Name = "mnuAbout"
        Me.mnuAbout.Size = New System.Drawing.Size(122, 22)
        Me.mnuAbout.Text = "A propos"
        AddHandler Me.mnuAbout.Click, AddressOf Me.MnuAboutClick
        '
        'toolStrip
        '
        Me.toolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btDBOpen, Me.btPricesUpdate, Me.btPricesHistoryAdd, Me.btPicturesFix, Me.btCancel, Me.btReplaceTitle})
        Me.toolStrip.Location = New System.Drawing.Point(0, 24)
        Me.toolStrip.Name = "toolStrip"
        Me.toolStrip.Size = New System.Drawing.Size(670, 25)
        Me.toolStrip.TabIndex = 1
        Me.toolStrip.Text = "toolStrip1"
        '
        'btDBOpen
        '
        Me.btDBOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btDBOpen.Image = CType(resources.GetObject("btDBOpen.Image"),System.Drawing.Image)
        Me.btDBOpen.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btDBOpen.Name = "btDBOpen"
        Me.btDBOpen.Size = New System.Drawing.Size(23, 22)
        Me.btDBOpen.Text = "Base de données source"
        AddHandler Me.btDBOpen.Click, AddressOf Me.MnuDBOpenClick
        '
        'btPricesUpdate
        '
        Me.btPricesUpdate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btPricesUpdate.Image = CType(resources.GetObject("btPricesUpdate.Image"),System.Drawing.Image)
        Me.btPricesUpdate.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btPricesUpdate.Name = "btPricesUpdate"
        Me.btPricesUpdate.Size = New System.Drawing.Size(23, 22)
        Me.btPricesUpdate.Text = "Récupérer les prix"
        AddHandler Me.btPricesUpdate.Click, AddressOf Me.MnuPricesUpdateClick
        '
        'btPricesHistoryAdd
        '
        Me.btPricesHistoryAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btPricesHistoryAdd.Image = CType(resources.GetObject("btPricesHistoryAdd.Image"),System.Drawing.Image)
        Me.btPricesHistoryAdd.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btPricesHistoryAdd.Name = "btPricesHistoryAdd"
        Me.btPricesHistoryAdd.Size = New System.Drawing.Size(23, 22)
        Me.btPricesHistoryAdd.Text = "Mettre à jour l'historique de la base"
        AddHandler Me.btPricesHistoryAdd.Click, AddressOf Me.MnuPricesHistoryAddClick
        '
        'btPicturesFix
        '
        Me.btPicturesFix.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btPicturesFix.Image = CType(resources.GetObject("btPicturesFix.Image"),System.Drawing.Image)
        Me.btPicturesFix.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btPicturesFix.Name = "btPicturesFix"
        Me.btPicturesFix.Size = New System.Drawing.Size(23, 22)
        Me.btPicturesFix.Text = "Corriger les images"
        AddHandler Me.btPicturesFix.Click, AddressOf Me.MnuPicturesFixClick
        '
        'btCancel
        '
        Me.btCancel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.btCancel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btCancel.Enabled = false
        Me.btCancel.Image = CType(resources.GetObject("btCancel.Image"),System.Drawing.Image)
        Me.btCancel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btCancel.Name = "btCancel"
        Me.btCancel.Size = New System.Drawing.Size(23, 22)
        Me.btCancel.Text = "Annuler"
        AddHandler Me.btCancel.Click, AddressOf Me.BtCancelClick
        '
        'btReplaceTitle
        '
        Me.btReplaceTitle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btReplaceTitle.Image = CType(resources.GetObject("btReplaceTitle.Image"),System.Drawing.Image)
        Me.btReplaceTitle.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btReplaceTitle.Name = "btReplaceTitle"
        Me.btReplaceTitle.Size = New System.Drawing.Size(23, 22)
        Me.btReplaceTitle.Text = "Remplacer un nom"
        AddHandler Me.btReplaceTitle.Click, AddressOf Me.MnuCardReplaceTitleClick
        '
        'statusStrip
        '
        Me.statusStrip.Location = New System.Drawing.Point(0, 328)
        Me.statusStrip.Name = "statusStrip"
        Me.statusStrip.Size = New System.Drawing.Size(670, 22)
        Me.statusStrip.TabIndex = 2
        Me.statusStrip.Text = "statusStrip1"
        '
        'prgAvance
        '
        Me.prgAvance.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.prgAvance.Location = New System.Drawing.Point(0, 305)
        Me.prgAvance.Name = "prgAvance"
        Me.prgAvance.Size = New System.Drawing.Size(670, 23)
        Me.prgAvance.TabIndex = 4
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.tabStatus)
        Me.tabMain.Controls.Add(Me.tabBrowser)
        Me.tabMain.Controls.Add(Me.tabInfo)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 49)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(670, 256)
        Me.tabMain.TabIndex = 5
        '
        'tabStatus
        '
        Me.tabStatus.Controls.Add(Me.lvwLog)
        Me.tabStatus.Location = New System.Drawing.Point(4, 22)
        Me.tabStatus.Name = "tabStatus"
        Me.tabStatus.Padding = New System.Windows.Forms.Padding(3)
        Me.tabStatus.Size = New System.Drawing.Size(662, 230)
        Me.tabStatus.TabIndex = 0
        Me.tabStatus.Text = "Statut"
        Me.tabStatus.UseVisualStyleBackColor = true
        '
        'lvwLog
        '
        Me.lvwLog.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colDate, Me.colEvent})
        Me.lvwLog.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwLog.Location = New System.Drawing.Point(3, 3)
        Me.lvwLog.Name = "lvwLog"
        Me.lvwLog.Size = New System.Drawing.Size(656, 224)
        Me.lvwLog.SmallImageList = Me.imgLst
        Me.lvwLog.TabIndex = 0
        Me.lvwLog.UseCompatibleStateImageBehavior = false
        Me.lvwLog.View = System.Windows.Forms.View.Details
        '
        'colDate
        '
        Me.colDate.Text = "Date"
        Me.colDate.Width = 156
        '
        'colEvent
        '
        Me.colEvent.Text = "Evènement"
        Me.colEvent.Width = 435
        '
        'imgLst
        '
        Me.imgLst.ImageStream = CType(resources.GetObject("imgLst.ImageStream"),System.Windows.Forms.ImageListStreamer)
        Me.imgLst.TransparentColor = System.Drawing.Color.Transparent
        Me.imgLst.Images.SetKeyName(0, "Info.ico")
        Me.imgLst.Images.SetKeyName(1, "Warning.ico")
        Me.imgLst.Images.SetKeyName(2, "Exit.ico")
        '
        'tabBrowser
        '
        Me.tabBrowser.Controls.Add(Me.wbMV)
        Me.tabBrowser.Location = New System.Drawing.Point(4, 22)
        Me.tabBrowser.Name = "tabBrowser"
        Me.tabBrowser.Padding = New System.Windows.Forms.Padding(3)
        Me.tabBrowser.Size = New System.Drawing.Size(662, 230)
        Me.tabBrowser.TabIndex = 1
        Me.tabBrowser.Text = "Navigateur"
        Me.tabBrowser.UseVisualStyleBackColor = true
        '
        'wbMV
        '
        Me.wbMV.Dock = System.Windows.Forms.DockStyle.Fill
        Me.wbMV.Location = New System.Drawing.Point(3, 3)
        Me.wbMV.MinimumSize = New System.Drawing.Size(20, 20)
        Me.wbMV.Name = "wbMV"
        Me.wbMV.Size = New System.Drawing.Size(656, 224)
        Me.wbMV.TabIndex = 15
        AddHandler Me.wbMV.DocumentCompleted, AddressOf Me.WbMVDocumentCompleted
        AddHandler Me.wbMV.NewWindow, AddressOf Me.WbMVNewWindow
        '
        'tabInfo
        '
        Me.tabInfo.Controls.Add(Me.txtETA)
        Me.tabInfo.Controls.Add(Me.lbl2)
        Me.tabInfo.Controls.Add(Me.txtCur)
        Me.tabInfo.Controls.Add(Me.lbl1)
        Me.tabInfo.Location = New System.Drawing.Point(4, 22)
        Me.tabInfo.Name = "tabInfo"
        Me.tabInfo.Size = New System.Drawing.Size(662, 230)
        Me.tabInfo.TabIndex = 2
        Me.tabInfo.Text = "Informations"
        Me.tabInfo.UseVisualStyleBackColor = true
        '
        'txtETA
        '
        Me.txtETA.Location = New System.Drawing.Point(224, 75)
        Me.txtETA.Name = "txtETA"
        Me.txtETA.ReadOnly = true
        Me.txtETA.Size = New System.Drawing.Size(211, 20)
        Me.txtETA.TabIndex = 3
        '
        'lbl2
        '
        Me.lbl2.AutoSize = true
        Me.lbl2.Location = New System.Drawing.Point(52, 78)
        Me.lbl2.Name = "lbl2"
        Me.lbl2.Size = New System.Drawing.Size(142, 13)
        Me.lbl2.TabIndex = 2
        Me.lbl2.Text = "Estimation du temps restant :"
        '
        'txtCur
        '
        Me.txtCur.Location = New System.Drawing.Point(224, 49)
        Me.txtCur.Name = "txtCur"
        Me.txtCur.ReadOnly = true
        Me.txtCur.Size = New System.Drawing.Size(211, 20)
        Me.txtCur.TabIndex = 1
        '
        'lbl1
        '
        Me.lbl1.AutoSize = true
        Me.lbl1.Location = New System.Drawing.Point(52, 52)
        Me.lbl1.Name = "lbl1"
        Me.lbl1.Size = New System.Drawing.Size(107, 13)
        Me.lbl1.TabIndex = 0
        Me.lbl1.Text = "Traitement en cours :"
        '
        'dlgOpen
        '
        Me.dlgOpen.DefaultExt = "mdb"
        Me.dlgOpen.Filter = "Fichiers de base de données Microsoft Access (*.mdb)|*.mdb"
        Me.dlgOpen.Title = "Sélection de la base de données"
        '
        'dlgSave
        '
        Me.dlgSave.DefaultExt = "txt"
        Me.dlgSave.Filter = "Fichiers texte (*.txt)|*.txt"
        Me.dlgSave.Title = "Sélection du fichier de sortie"
        '
        'dlgOpen2
        '
        Me.dlgOpen2.DefaultExt = "txt"
        Me.dlgOpen2.Filter = "Text files (*.txt)|*.txt"
        Me.dlgOpen2.Title = "Sélection du fichier de prix"
        '
        'dlgOpen3
        '
        Me.dlgOpen3.DefaultExt = "dat"
        Me.dlgOpen3.Filter = "Data files (*.dat) | *.dat"
        Me.dlgOpen3.Title = "Sélection du fichier de données d'images"
        '
        'dlgBrowse
        '
        Me.dlgBrowse.Description = "Choix du dossier"
        '
        'dlgOpen4
        '
        Me.dlgOpen4.DefaultExt = "txt"
        Me.dlgOpen4.Filter = "Text files (*.txt)|*.txt"
        Me.dlgOpen4.Title = "Sélection du listing"
        '
        'dlgOpen5
        '
        Me.dlgOpen5.DefaultExt = "ini"
        Me.dlgOpen5.Filter = "Fichiers de configuration (*.ini)|*.ini"
        Me.dlgOpen5.Title = "Sélection du fichier à préparer"
        '
        'dlgOpen6
        '
        Me.dlgOpen6.DefaultExt = "xml"
        Me.dlgOpen6.Filter = "Fichiers XML (*.xml)|*.xml"
        Me.dlgOpen6.Title = "Sélection du fichier à préparer"
        '
        'dlgSave2
        '
        Me.dlgSave2.DefaultExt = "dat"
        Me.dlgSave2.Filter = "Data files (*.dat) | *.dat"
        Me.dlgSave2.Title = "Sélection du fichier de sortie"
        '
        'dlgSave3
        '
        Me.dlgSave3.DefaultExt = "xml"
        Me.dlgSave3.Filter = "Fichiers XML (*.xml) | *.xml"
        Me.dlgSave3.Title = "Sélection du fichier de sortie"
        '
        'mnuPricesHistoryRebuild
        '
        Me.mnuPricesHistoryRebuild.Name = "mnuPricesHistoryRebuild"
        Me.mnuPricesHistoryRebuild.Size = New System.Drawing.Size(283, 22)
        Me.mnuPricesHistoryRebuild.Text = "Reconstruire complètement l'historique"
        AddHandler Me.mnuPricesHistoryRebuild.Click, AddressOf Me.MnuPricesHistoryRebuildClick
        '
        '
        'mnuCardsRulingsFilter
        '
        Me.mnuCardsRulingsFilter.Name = "mnuCardsRulingsFilter"
        Me.mnuCardsRulingsFilter.Size = New System.Drawing.Size(278, 22)
        Me.mnuCardsRulingsFilter.Text = "Filtrer les rulings"
        AddHandler Me.mnuCardsRulingsFilter.Click, AddressOf Me.MnuCardsRulingsFilterClick
        '
        'mnuCardsExtractMultiverseId
        '
        Me.mnuCardsExtractMultiverseId.Image = CType(resources.GetObject("mnuCardsExtractMultiverseId.Image"),System.Drawing.Image)
        Me.mnuCardsExtractMultiverseId.Name = "mnuCardsExtractMultiverseId"
        Me.mnuCardsExtractMultiverseId.Size = New System.Drawing.Size(278, 22)
        Me.mnuCardsExtractMultiverseId.Text = "Extraire les identifiants Multiverse et autres"
        AddHandler Me.mnuCardsExtractMultiverseId.Click, AddressOf Me.MnuCardsExtractMultiverseIdClick
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(670, 350)
        Me.Controls.Add(Me.tabMain)
        Me.Controls.Add(Me.prgAvance)
        Me.Controls.Add(Me.statusStrip)
        Me.Controls.Add(Me.toolStrip)
        Me.Controls.Add(Me.menuStrip)
        Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
        Me.MainMenuStrip = Me.menuStrip
        Me.Name = "MainForm"
        Me.Text = "MTGM Web Resourcer"
        AddHandler FormClosing, AddressOf Me.MainFormFormClosing
        Me.menuStrip.ResumeLayout(false)
        Me.menuStrip.PerformLayout
        Me.toolStrip.ResumeLayout(false)
        Me.toolStrip.PerformLayout
        Me.tabMain.ResumeLayout(false)
        Me.tabStatus.ResumeLayout(false)
        Me.tabBrowser.ResumeLayout(false)
        Me.tabInfo.ResumeLayout(false)
        Me.tabInfo.PerformLayout
        Me.ResumeLayout(false)
        Me.PerformLayout
    End Sub
    Private mnuPicturesThumbs As System.Windows.Forms.ToolStripMenuItem
    Private mnuPicturesSymbols As System.Windows.Forms.ToolStripMenuItem
    Private mnuCardsRulingsFilter As System.Windows.Forms.ToolStripMenuItem
    Private mnuCardsExtractMultiverseId As System.Windows.Forms.ToolStripMenuItem
    Private mnuPricesHistoryRebuild As System.Windows.Forms.ToolStripMenuItem
    Private mnuSeriesSpoilers As System.Windows.Forms.ToolStripMenuItem
    Private mnuSeriesVirtualAdd As System.Windows.Forms.ToolStripMenuItem
    Private mnuSeriesMerge As System.Windows.Forms.ToolStripMenuItem
    Private mnuSeriesGenR16 As System.Windows.Forms.ToolStripMenuItem
    Private mnuSeriesGenR14 As System.Windows.Forms.ToolStripMenuItem
    Private mnuCardsExtractDiff5 As System.Windows.Forms.ToolStripMenuItem
    Private mnuPricesUpdateListe As System.Windows.Forms.ToolStripMenuItem
    Private mnuPricesUpdateAll As System.Windows.Forms.ToolStripMenuItem
    Private mnuCardsAutMerge As System.Windows.Forms.ToolStripMenuItem
    Private mnuCardsAutListe As System.Windows.Forms.ToolStripMenuItem
    Private mnuCardsAutAll As System.Windows.Forms.ToolStripMenuItem
    Private mnuPicturesRevertSP As System.Windows.Forms.ToolStripMenuItem
    Private btReplaceTitle As System.Windows.Forms.ToolStripButton
    Private mnuCardReplaceTitle As System.Windows.Forms.ToolStripMenuItem
    Private mnuFixTxtVO As System.Windows.Forms.ToolStripMenuItem
    Private mnuFindHoles As System.Windows.Forms.ToolStripMenuItem
    Private mnuBuildStamps As System.Windows.Forms.ToolStripMenuItem
    Private mnuBuildDouble As System.Windows.Forms.ToolStripMenuItem
    Private mnuBuildTitles As System.Windows.Forms.ToolStripMenuItem
    Private mnuCheckTrad As System.Windows.Forms.ToolStripMenuItem
    Private mnuCardsTradTitles As System.Windows.Forms.ToolStripMenuItem
    Private mnuCardsTradTitlesFilter As System.Windows.Forms.ToolStripMenuItem
    Private mnuCompareTitles As System.Windows.Forms.ToolStripMenuItem
    Private mnuListSubtypes As System.Windows.Forms.ToolStripMenuItem
    Private mnuCompareTrad As System.Windows.Forms.ToolStripMenuItem
    Private mnuCardsExtractDiff4 As System.Windows.Forms.ToolStripMenuItem
    Private mnuCardsExtractDiff3 As System.Windows.Forms.ToolStripMenuItem
    Private mnuCardsExtractDiff As System.Windows.Forms.ToolStripMenuItem
    Private dlgSave2 As System.Windows.Forms.SaveFileDialog
    Private dlgSave3 As System.Windows.Forms.SaveFileDialog
    Private mnuPicturesNewSP As System.Windows.Forms.ToolStripMenuItem
    Private mnuFilterTitles As System.Windows.Forms.ToolStripMenuItem
    Private dlgOpen5 As System.Windows.Forms.OpenFileDialog
    Private dlgOpen6 As System.Windows.Forms.OpenFileDialog
    Private mnuINIReady As System.Windows.Forms.ToolStripMenuItem
    Private mnuDBReady As System.Windows.Forms.ToolStripMenuItem
    Private mnuBuildPatch As System.Windows.Forms.ToolStripMenuItem
    Private mnuGetShippingCosts As System.Windows.Forms.ToolStripMenuItem
    Private mnuTrad As System.Windows.Forms.ToolStripMenuItem
    Private mnuCardsExtractDiff2 As System.Windows.Forms.ToolStripMenuItem
    Private mnuExtractTexts As System.Windows.Forms.ToolStripMenuItem
    Private wbMV As System.Windows.Forms.WebBrowser
    Private dlgOpen4 As System.Windows.Forms.OpenFileDialog
    Private mnuCardsExtractAll As System.Windows.Forms.ToolStripMenuItem
    Private dlgBrowse As System.Windows.Forms.FolderBrowserDialog
    Private dlgOpen3 As System.Windows.Forms.OpenFileDialog
    Private dlgOpen2 As System.Windows.Forms.OpenFileDialog
    Private btCancel As System.Windows.Forms.ToolStripButton
    Private lbl1 As System.Windows.Forms.Label
    Private txtCur As System.Windows.Forms.TextBox
    Private lbl2 As System.Windows.Forms.Label
    Private txtETA As System.Windows.Forms.TextBox
    Private tabInfo As System.Windows.Forms.TabPage
    Private dlgSave As System.Windows.Forms.SaveFileDialog
    Private dlgOpen As System.Windows.Forms.OpenFileDialog
    Private btPicturesFix As System.Windows.Forms.ToolStripButton
    Private btPricesHistoryAdd As System.Windows.Forms.ToolStripButton
    Private btPricesUpdate As System.Windows.Forms.ToolStripButton
    Private btDBOpen As System.Windows.Forms.ToolStripButton
    Private mnuPicturesDelta As System.Windows.Forms.ToolStripMenuItem
    Private mnuPicturesFix As System.Windows.Forms.ToolStripMenuItem
    Private mnuPicturesUpdate As System.Windows.Forms.ToolStripMenuItem
    Private mnuPicturesRemove As System.Windows.Forms.ToolStripMenuItem
    Private mnuPictures As System.Windows.Forms.ToolStripMenuItem
    Private mnuPricesHistoryAdd As System.Windows.Forms.ToolStripMenuItem
    Private mnuPricesUpdate As System.Windows.Forms.ToolStripMenuItem
    Private mnuPrices As System.Windows.Forms.ToolStripMenuItem
    Private mnuSeriesGen As System.Windows.Forms.ToolStripMenuItem
    Private mnuSeries As System.Windows.Forms.ToolStripMenuItem
    Private mnuCardsAut As System.Windows.Forms.ToolStripMenuItem
    Private mnuCardsTradTxt As System.Windows.Forms.ToolStripMenuItem
    Private mnuCardsExtract As System.Windows.Forms.ToolStripMenuItem
    Private mnuCards As System.Windows.Forms.ToolStripMenuItem
    Private mnuSeparator As System.Windows.Forms.ToolStripSeparator
    Private mnuDBOpen As System.Windows.Forms.ToolStripMenuItem
    Private imgLst As System.Windows.Forms.ImageList
    Private colEvent As System.Windows.Forms.ColumnHeader
    Private colDate As System.Windows.Forms.ColumnHeader
    Private lvwLog As System.Windows.Forms.ListView
    Private menuStrip As System.Windows.Forms.MenuStrip
    Private mnuAbout As System.Windows.Forms.ToolStripMenuItem
    Private mnuExit As System.Windows.Forms.ToolStripMenuItem
    Private mnuHelp As System.Windows.Forms.ToolStripMenuItem
    Private toolStrip As System.Windows.Forms.ToolStrip
    Private statusStrip As System.Windows.Forms.StatusStrip
    Private prgAvance As System.Windows.Forms.ProgressBar
    Private tabMain As System.Windows.Forms.TabControl
    Private tabBrowser As System.Windows.Forms.TabPage
    Private tabStatus As System.Windows.Forms.TabPage
    Private mnuTools As System.Windows.Forms.ToolStripMenuItem
    Private mnuFile As System.Windows.Forms.ToolStripMenuItem
End Class
