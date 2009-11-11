'
' Created by SharpDevelop.
' User: Couitchy
' Date: 22/03/2008
' Time: 14:38
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
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
		Me.components = New System.ComponentModel.Container
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
		Me.mnuMenuStrip = New TD.SandBar.MenuBar
		Me.mnuFile = New TD.SandBar.MenuBarItem
		Me.mnuDBSelect = New TD.SandBar.MenuButtonItem
		Me.mnuExport = New TD.SandBar.MenuButtonItem
		Me.mnuNewEdition = New TD.SandBar.MenuButtonItem
		Me.mnuRemEdition = New TD.SandBar.MenuButtonItem
		Me.mnuTranslate = New TD.SandBar.MenuButtonItem
		Me.mnuUpdatePrices = New TD.SandBar.MenuButtonItem
		Me.mnuUpdatePictures = New TD.SandBar.MenuButtonItem
		Me.mnuUpdateSimu = New TD.SandBar.MenuButtonItem
		Me.mnuExit = New TD.SandBar.MenuButtonItem
		Me.mnuDisp = New TD.SandBar.MenuBarItem
		Me.mnuRefresh = New TD.SandBar.MenuButtonItem
		Me.mnuShowImage = New TD.SandBar.MenuButtonItem
		Me.mnuDispCollection = New TD.SandBar.MenuButtonItem
		Me.mnuTools = New TD.SandBar.MenuBarItem
		Me.mnuGestDecks = New TD.SandBar.MenuButtonItem
		Me.mnuAddCards = New TD.SandBar.MenuButtonItem
		Me.mnuRemCards = New TD.SandBar.MenuButtonItem
		Me.mnuRemScores = New TD.SandBar.MenuButtonItem
		Me.mnuRemCollec = New TD.SandBar.MenuButtonItem
		Me.mnuRemGames = New TD.SandBar.MenuButtonItem
		Me.mnuFixTable = New TD.SandBar.MenuButtonItem
		Me.mnuFixPrices = New TD.SandBar.MenuButtonItem
		Me.mnuFixFR = New TD.SandBar.MenuButtonItem
		Me.mnuFixSerie = New TD.SandBar.MenuButtonItem
		Me.mnuFixCollec = New TD.SandBar.MenuButtonItem
		Me.mnuFixGames = New TD.SandBar.MenuButtonItem
		Me.mnuPrefs = New TD.SandBar.MenuButtonItem
		Me.mnuBigSearch = New TD.SandBar.MenuBarItem
		Me.mnuStdSearch = New TD.SandBar.MenuButtonItem
		Me.mnuAdvancedSearch = New TD.SandBar.MenuButtonItem
		Me.mnuExcelGen = New TD.SandBar.MenuButtonItem
		Me.mnuPerfs = New TD.SandBar.MenuButtonItem
		Me.mnuSimu = New TD.SandBar.MenuButtonItem
		Me.mnuStats = New TD.SandBar.MenuButtonItem
		Me.mnuInfo = New TD.SandBar.MenuBarItem
		Me.mnuCheckForUpdates = New TD.SandBar.MenuButtonItem
		Me.mnuCheckForBetas = New TD.SandBar.MenuButtonItem
		Me.mnuHelp = New TD.SandBar.MenuButtonItem
		Me.mnuAbout = New TD.SandBar.MenuButtonItem
		Me.statusStrip = New System.Windows.Forms.StatusStrip
		Me.lblDB = New System.Windows.Forms.ToolStripStatusLabel
		Me.lblNCards = New System.Windows.Forms.ToolStripStatusLabel
		Me.prgAvance = New System.Windows.Forms.ToolStripProgressBar
		Me.dlgOpen = New System.Windows.Forms.OpenFileDialog
		Me.splitV = New System.Windows.Forms.SplitContainer
		Me.CBarTvw = New TD.SandBar.ContainerBar
		Me.pnlTvw = New TD.SandBar.ContainerBarClientPanel
		Me.tvwExplore = New TreeViewMS.TreeViewMS
		Me.imglstTvw = New System.Windows.Forms.ImageList(Me.components)
		Me.chkClassement = New System.Windows.Forms.CheckedListBox
		Me.btUp = New TD.SandBar.ButtonItem
		Me.btDown = New TD.SandBar.ButtonItem
		Me.btRefresh = New TD.SandBar.ButtonItem
		Me.splitV2 = New System.Windows.Forms.SplitContainer
		Me.CBarProperties = New TD.SandBar.ContainerBar
		Me.pnlProperties = New TD.SandBar.ContainerBarClientPanel
		Me.grpCarac = New System.Windows.Forms.GroupBox
		Me.txtCardText = New System.Windows.Forms.TextBox
		Me.lblProp7 = New System.Windows.Forms.Label
		Me.grpSerie = New System.Windows.Forms.GroupBox
		Me.cboEdition = New System.Windows.Forms.ComboBox
		Me.picEdition = New System.Windows.Forms.PictureBox
		Me.lblAD = New System.Windows.Forms.Label
		Me.lblStock = New System.Windows.Forms.Label
		Me.lblPrix = New System.Windows.Forms.Label
		Me.lblRarete = New System.Windows.Forms.Label
		Me.lblProp6 = New System.Windows.Forms.Label
		Me.lblProp1 = New System.Windows.Forms.Label
		Me.lblProp2 = New System.Windows.Forms.Label
		Me.lblProp5 = New System.Windows.Forms.Label
		Me.lblProp4 = New System.Windows.Forms.Label
		Me.lblProp3 = New System.Windows.Forms.Label
		Me.CBarImage = New TD.SandBar.ContainerBar
		Me.pnlImage = New TD.SandBar.ContainerBarClientPanel
		Me.picScanCard = New System.Windows.Forms.PictureBox
		Me.imglstCarac = New System.Windows.Forms.ImageList(Me.components)
		Me.cmnuTvw = New System.Windows.Forms.ContextMenuStrip(Me.components)
		Me.mnuCardsFR = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuSort = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuSearchCard = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuSearchText = New System.Windows.Forms.ToolStripTextBox
		Me.mnuFindNext = New System.Windows.Forms.ToolStripMenuItem
		Me.mnucAddCards = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuSeparator = New System.Windows.Forms.ToolStripSeparator
		Me.mnuMoveACard = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuMoveToCollection = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuDeleteACard = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuSeparator2 = New System.Windows.Forms.ToolStripSeparator
		Me.mnuBuy = New System.Windows.Forms.ToolStripMenuItem
		Me.dlgOpen2 = New System.Windows.Forms.OpenFileDialog
		Me.dlgOpen3 = New System.Windows.Forms.OpenFileDialog
		Me.dlgOpen4 = New System.Windows.Forms.OpenFileDialog
		Me.statusStrip.SuspendLayout
		Me.splitV.Panel1.SuspendLayout
		Me.splitV.Panel2.SuspendLayout
		Me.splitV.SuspendLayout
		Me.CBarTvw.SuspendLayout
		Me.pnlTvw.SuspendLayout
		Me.splitV2.Panel1.SuspendLayout
		Me.splitV2.Panel2.SuspendLayout
		Me.splitV2.SuspendLayout
		Me.CBarProperties.SuspendLayout
		Me.pnlProperties.SuspendLayout
		Me.grpCarac.SuspendLayout
		Me.grpSerie.SuspendLayout
		CType(Me.picEdition,System.ComponentModel.ISupportInitialize).BeginInit
		Me.CBarImage.SuspendLayout
		Me.pnlImage.SuspendLayout
		CType(Me.picScanCard,System.ComponentModel.ISupportInitialize).BeginInit
		Me.cmnuTvw.SuspendLayout
		Me.SuspendLayout
		'
		'mnuMenuStrip
		'
		Me.mnuMenuStrip.Guid = New System.Guid("b9df5efd-99ef-4d6d-9498-2535e688efee")
		Me.mnuMenuStrip.Items.AddRange(New TD.SandBar.ToolbarItemBase() {Me.mnuFile, Me.mnuDisp, Me.mnuTools, Me.mnuBigSearch, Me.mnuInfo})
		Me.mnuMenuStrip.Location = New System.Drawing.Point(0, 0)
		Me.mnuMenuStrip.Name = "mnuMenuStrip"
		Me.mnuMenuStrip.OwnerForm = Me
		Me.mnuMenuStrip.Size = New System.Drawing.Size(757, 21)
		Me.mnuMenuStrip.TabIndex = 0
		Me.mnuMenuStrip.Text = "menuBar1"
		'
		'mnuFile
		'
		Me.mnuFile.Items.AddRange(New TD.SandBar.ToolbarItemBase() {Me.mnuDBSelect, Me.mnuExport, Me.mnuNewEdition, Me.mnuRemEdition, Me.mnuTranslate, Me.mnuUpdatePrices, Me.mnuUpdatePictures, Me.mnuUpdateSimu, Me.mnuExit})
		Me.mnuFile.Text = "Fichier"
		'
		'mnuDBSelect
		'
		Me.mnuDBSelect.Icon = CType(resources.GetObject("mnuDBSelect.Icon"),System.Drawing.Icon)
		Me.mnuDBSelect.Text = "Ouvrir une base de données"
		AddHandler Me.mnuDBSelect.Activate, AddressOf Me.MnuDBSelectActivate
		'
		'mnuExport
		'
		Me.mnuExport.Icon = CType(resources.GetObject("mnuExport.Icon"),System.Drawing.Icon)
		Me.mnuExport.Text = "Import / Export"
		AddHandler Me.mnuExport.Activate, AddressOf Me.MnuExportActivate
		'
		'mnuNewEdition
		'
		Me.mnuNewEdition.BeginGroup = true
		Me.mnuNewEdition.Icon = CType(resources.GetObject("mnuNewEdition.Icon"),System.Drawing.Icon)
		Me.mnuNewEdition.Text = "Ajouter une série..."
		AddHandler Me.mnuNewEdition.Activate, AddressOf Me.MnuNewEditionActivate
		'
		'mnuRemEdition
		'
		Me.mnuRemEdition.Icon = CType(resources.GetObject("mnuRemEdition.Icon"),System.Drawing.Icon)
		Me.mnuRemEdition.Text = "Supprimer une série"
		AddHandler Me.mnuRemEdition.Activate, AddressOf Me.MnuRemEditionActivate
		'
		'mnuTranslate
		'
		Me.mnuTranslate.Icon = CType(resources.GetObject("mnuTranslate.Icon"),System.Drawing.Icon)
		Me.mnuTranslate.Text = "Traduire une série"
		AddHandler Me.mnuTranslate.Activate, AddressOf Me.MnuTranslateActivate
		'
		'mnuUpdatePrices
		'
		Me.mnuUpdatePrices.BeginGroup = true
		Me.mnuUpdatePrices.Icon = CType(resources.GetObject("mnuUpdatePrices.Icon"),System.Drawing.Icon)
		Me.mnuUpdatePrices.Text = "Mettre à jour les prix"
		AddHandler Me.mnuUpdatePrices.Activate, AddressOf Me.MnuUpdatePricesActivate
		'
		'mnuUpdatePictures
		'
		Me.mnuUpdatePictures.Icon = CType(resources.GetObject("mnuUpdatePictures.Icon"),System.Drawing.Icon)
		Me.mnuUpdatePictures.Text = "Mettre à jour les images"
		AddHandler Me.mnuUpdatePictures.Activate, AddressOf Me.MnuUpdatePicturesActivate
		'
		'mnuUpdateSimu
		'
		Me.mnuUpdateSimu.Icon = CType(resources.GetObject("mnuUpdateSimu.Icon"),System.Drawing.Icon)
		Me.mnuUpdateSimu.Text = "Mettre à jour les modèles de simulation"
		AddHandler Me.mnuUpdateSimu.Activate, AddressOf Me.MnuUpdateSimuActivate
		'
		'mnuExit
		'
		Me.mnuExit.BeginGroup = true
		Me.mnuExit.Icon = CType(resources.GetObject("mnuExit.Icon"),System.Drawing.Icon)
		Me.mnuExit.Text = "Quitter"
		AddHandler Me.mnuExit.Activate, AddressOf Me.MnuExitActivate
		'
		'mnuDisp
		'
		Me.mnuDisp.Items.AddRange(New TD.SandBar.ToolbarItemBase() {Me.mnuRefresh, Me.mnuShowImage, Me.mnuDispCollection})
		Me.mnuDisp.Text = "Affichage"
		'
		'mnuRefresh
		'
		Me.mnuRefresh.Icon = CType(resources.GetObject("mnuRefresh.Icon"),System.Drawing.Icon)
		Me.mnuRefresh.Text = "Rafraîchir"
		AddHandler Me.mnuRefresh.Activate, AddressOf Me.MnuRefreshActivate
		'
		'mnuShowImage
		'
		Me.mnuShowImage.Icon = CType(resources.GetObject("mnuShowImage.Icon"),System.Drawing.Icon)
		Me.mnuShowImage.Text = "Ouvrir / fermer panneau image"
		AddHandler Me.mnuShowImage.Activate, AddressOf Me.MnuShowImageActivate
		'
		'mnuDispCollection
		'
		Me.mnuDispCollection.BeginGroup = true
		Me.mnuDispCollection.Checked = true
		Me.mnuDispCollection.Text = "Collection"
		AddHandler Me.mnuDispCollection.Activate, AddressOf Me.MnuDispCollectionActivate
		'
		'mnuTools
		'
		Me.mnuTools.Items.AddRange(New TD.SandBar.ToolbarItemBase() {Me.mnuGestDecks, Me.mnuAddCards, Me.mnuRemCards, Me.mnuFixTable, Me.mnuPrefs})
		Me.mnuTools.Text = "Gestion"
		'
		'mnuGestDecks
		'
		Me.mnuGestDecks.Icon = CType(resources.GetObject("mnuGestDecks.Icon"),System.Drawing.Icon)
		Me.mnuGestDecks.Text = "Liste des decks"
		AddHandler Me.mnuGestDecks.Activate, AddressOf Me.MnuGestDecksActivate
		'
		'mnuAddCards
		'
		Me.mnuAddCards.Icon = CType(resources.GetObject("mnuAddCards.Icon"),System.Drawing.Icon)
		Me.mnuAddCards.Text = "Ajouter / supprimer des cartes"
		AddHandler Me.mnuAddCards.Activate, AddressOf Me.MnuAddCardsActivate
		'
		'mnuRemCards
		'
		Me.mnuRemCards.Icon = CType(resources.GetObject("mnuRemCards.Icon"),System.Drawing.Icon)
		Me.mnuRemCards.Items.AddRange(New TD.SandBar.ToolbarItemBase() {Me.mnuRemScores, Me.mnuRemCollec, Me.mnuRemGames})
		Me.mnuRemCards.Text = "Purger la table..."
		'
		'mnuRemScores
		'
		Me.mnuRemScores.Text = "Victoires / Défaites"
		AddHandler Me.mnuRemScores.Activate, AddressOf Me.MnuRemScoresActivate
		'
		'mnuRemCollec
		'
		Me.mnuRemCollec.Text = "Collection"
		AddHandler Me.mnuRemCollec.Activate, AddressOf Me.MnuRemCollecActivate
		'
		'mnuRemGames
		'
		Me.mnuRemGames.Text = "Jeux..."
		'
		'mnuFixTable
		'
		Me.mnuFixTable.Icon = CType(resources.GetObject("mnuFixTable.Icon"),System.Drawing.Icon)
		Me.mnuFixTable.Items.AddRange(New TD.SandBar.ToolbarItemBase() {Me.mnuFixPrices, Me.mnuFixFR, Me.mnuFixSerie, Me.mnuFixCollec, Me.mnuFixGames})
		Me.mnuFixTable.Text = "Réparer la table..."
		'
		'mnuFixPrices
		'
		Me.mnuFixPrices.Text = "Liste des prix"
		AddHandler Me.mnuFixPrices.Activate, AddressOf Me.MnuFixPricesActivate
		'
		'mnuFixFR
		'
		Me.mnuFixFR.Text = "Traductions"
		AddHandler Me.mnuFixFR.Activate, AddressOf Me.MnuFixFRActivate
		'
		'mnuFixSerie
		'
		Me.mnuFixSerie.Text = "Edition"
		'
		'mnuFixCollec
		'
		Me.mnuFixCollec.BeginGroup = true
		Me.mnuFixCollec.Text = "Collection"
		AddHandler Me.mnuFixCollec.Activate, AddressOf Me.MnuFixCollecActivate
		'
		'mnuFixGames
		'
		Me.mnuFixGames.Text = "Jeux..."
		'
		'mnuPrefs
		'
		Me.mnuPrefs.BeginGroup = true
		Me.mnuPrefs.Icon = CType(resources.GetObject("mnuPrefs.Icon"),System.Drawing.Icon)
		Me.mnuPrefs.Text = "Préférences"
		AddHandler Me.mnuPrefs.Activate, AddressOf Me.MnuPrefsActivate
		'
		'mnuBigSearch
		'
		Me.mnuBigSearch.Items.AddRange(New TD.SandBar.ToolbarItemBase() {Me.mnuStdSearch, Me.mnuAdvancedSearch, Me.mnuExcelGen, Me.mnuPerfs, Me.mnuSimu, Me.mnuStats})
		Me.mnuBigSearch.Text = "Outils"
		'
		'mnuStdSearch
		'
		Me.mnuStdSearch.Icon = CType(resources.GetObject("mnuStdSearch.Icon"),System.Drawing.Icon)
		Me.mnuStdSearch.Text = "Rechercher dans l'explorateur"
		AddHandler Me.mnuStdSearch.Activate, AddressOf Me.MnuStdSearchActivate
		'
		'mnuAdvancedSearch
		'
		Me.mnuAdvancedSearch.Icon = CType(resources.GetObject("mnuAdvancedSearch.Icon"),System.Drawing.Icon)
		Me.mnuAdvancedSearch.Text = "Recherche avancée"
		AddHandler Me.mnuAdvancedSearch.Activate, AddressOf Me.MnuAdvancedSearchActivate
		'
		'mnuExcelGen
		'
		Me.mnuExcelGen.BeginGroup = true
		Me.mnuExcelGen.Icon = CType(resources.GetObject("mnuExcelGen.Icon"),System.Drawing.Icon)
		Me.mnuExcelGen.Text = "Génération liste Excel"
		AddHandler Me.mnuExcelGen.Activate, AddressOf Me.MnuExcelGenActivate
		'
		'mnuPerfs
		'
		Me.mnuPerfs.Icon = CType(resources.GetObject("mnuPerfs.Icon"),System.Drawing.Icon)
		Me.mnuPerfs.Text = "Comptage victoires / défaites"
		AddHandler Me.mnuPerfs.Activate, AddressOf Me.MnuPerfsActivate
		'
		'mnuSimu
		'
		Me.mnuSimu.Icon = CType(resources.GetObject("mnuSimu.Icon"),System.Drawing.Icon)
		Me.mnuSimu.Text = "Simulations sur la sélection"
		AddHandler Me.mnuSimu.Activate, AddressOf Me.MnuSimuActivate
		'
		'mnuStats
		'
		Me.mnuStats.Icon = CType(resources.GetObject("mnuStats.Icon"),System.Drawing.Icon)
		Me.mnuStats.Text = "Statistiques sur la sélection"
		AddHandler Me.mnuStats.Activate, AddressOf Me.MnuStatsActivate
		'
		'mnuInfo
		'
		Me.mnuInfo.Items.AddRange(New TD.SandBar.ToolbarItemBase() {Me.mnuCheckForUpdates, Me.mnuCheckForBetas, Me.mnuHelp, Me.mnuAbout})
		Me.mnuInfo.Text = "?"
		'
		'mnuCheckForUpdates
		'
		Me.mnuCheckForUpdates.Icon = CType(resources.GetObject("mnuCheckForUpdates.Icon"),System.Drawing.Icon)
		Me.mnuCheckForUpdates.Text = "Vérifier les mises à jour"
		AddHandler Me.mnuCheckForUpdates.Activate, AddressOf Me.MnuCheckForUpdatesActivate
		'
		'mnuCheckForBetas
		'
		Me.mnuCheckForBetas.Icon = CType(resources.GetObject("mnuCheckForBetas.Icon"),System.Drawing.Icon)
		Me.mnuCheckForBetas.Text = "Vérifier les mises à jour bêta"
		AddHandler Me.mnuCheckForBetas.Activate, AddressOf Me.MnuCheckForBetasActivate
		'
		'mnuHelp
		'
		Me.mnuHelp.BeginGroup = true
		Me.mnuHelp.Icon = CType(resources.GetObject("mnuHelp.Icon"),System.Drawing.Icon)
		Me.mnuHelp.Text = "Aide"
		AddHandler Me.mnuHelp.Activate, AddressOf Me.MnuHelpActivate
		'
		'mnuAbout
		'
		Me.mnuAbout.Icon = CType(resources.GetObject("mnuAbout.Icon"),System.Drawing.Icon)
		Me.mnuAbout.Text = "A propos"
		AddHandler Me.mnuAbout.Activate, AddressOf Me.MnuAboutActivate
		'
		'statusStrip
		'
		Me.statusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblDB, Me.lblNCards, Me.prgAvance})
		Me.statusStrip.Location = New System.Drawing.Point(0, 350)
		Me.statusStrip.Name = "statusStrip"
		Me.statusStrip.Size = New System.Drawing.Size(757, 22)
		Me.statusStrip.TabIndex = 2
		Me.statusStrip.Text = "statusStrip1"
		'
		'lblDB
		'
		Me.lblDB.Name = "lblDB"
		Me.lblDB.Size = New System.Drawing.Size(37, 17)
		Me.lblDB.Text = "Base :"
		'
		'lblNCards
		'
		Me.lblNCards.Name = "lblNCards"
		Me.lblNCards.Size = New System.Drawing.Size(0, 17)
		'
		'prgAvance
		'
		Me.prgAvance.Name = "prgAvance"
		Me.prgAvance.Size = New System.Drawing.Size(100, 16)
		Me.prgAvance.Visible = false
		'
		'dlgOpen
		'
		Me.dlgOpen.DefaultExt = "mdb"
		Me.dlgOpen.Filter = "Access Database File (*.mdb) | *.mdb"
		Me.dlgOpen.Title = "Sélection de la base de données"
		'
		'splitV
		'
		Me.splitV.Dock = System.Windows.Forms.DockStyle.Fill
		Me.splitV.Location = New System.Drawing.Point(0, 21)
		Me.splitV.Name = "splitV"
		'
		'splitV.Panel1
		'
		Me.splitV.Panel1.Controls.Add(Me.CBarTvw)
		'
		'splitV.Panel2
		'
		Me.splitV.Panel2.Controls.Add(Me.splitV2)
		Me.splitV.Size = New System.Drawing.Size(757, 329)
		Me.splitV.SplitterDistance = 299
		Me.splitV.TabIndex = 3
		Me.splitV.TabStop = false
		'
		'CBarTvw
		'
		Me.CBarTvw.Closable = false
		Me.CBarTvw.Controls.Add(Me.pnlTvw)
		Me.CBarTvw.Dock = System.Windows.Forms.DockStyle.Fill
		Me.CBarTvw.DrawActionsButton = false
		Me.CBarTvw.Flow = TD.SandBar.ToolBarLayout.Horizontal
		Me.CBarTvw.Guid = New System.Guid("219cb30a-3b04-4474-8157-17accfec97d2")
		Me.CBarTvw.Items.AddRange(New TD.SandBar.ToolbarItemBase() {Me.btUp, Me.btDown, Me.btRefresh})
		Me.CBarTvw.Location = New System.Drawing.Point(0, 0)
		Me.CBarTvw.Movable = false
		Me.CBarTvw.Name = "CBarTvw"
		Me.CBarTvw.Size = New System.Drawing.Size(299, 329)
		Me.CBarTvw.TabIndex = 0
		Me.CBarTvw.Text = "Explorateur"
		'
		'pnlTvw
		'
		Me.pnlTvw.Controls.Add(Me.tvwExplore)
		Me.pnlTvw.Controls.Add(Me.chkClassement)
		Me.pnlTvw.Location = New System.Drawing.Point(2, 46)
		Me.pnlTvw.Name = "pnlTvw"
		Me.pnlTvw.Size = New System.Drawing.Size(295, 281)
		Me.pnlTvw.TabIndex = 0
		'
		'tvwExplore
		'
		Me.tvwExplore.BackColor = System.Drawing.SystemColors.Window
		Me.tvwExplore.Dock = System.Windows.Forms.DockStyle.Fill
		Me.tvwExplore.ImageIndex = 0
		Me.tvwExplore.ImageList = Me.imglstTvw
		Me.tvwExplore.Location = New System.Drawing.Point(0, 64)
		Me.tvwExplore.Name = "tvwExplore"
		Me.tvwExplore.SelectedImageIndex = 0
		Me.tvwExplore.SelectedNodes = CType(resources.GetObject("tvwExplore.SelectedNodes"),System.Collections.ArrayList)
		Me.tvwExplore.Size = New System.Drawing.Size(295, 217)
		Me.tvwExplore.TabIndex = 3
		AddHandler Me.tvwExplore.MouseUp, AddressOf Me.TvwExploreMouseUp
		AddHandler Me.tvwExplore.AfterSelect, AddressOf Me.TvwExploreAfterSelect
		AddHandler Me.tvwExplore.KeyUp, AddressOf Me.TvwExploreKeyUp
		'
		'imglstTvw
		'
		Me.imglstTvw.ImageStream = CType(resources.GetObject("imglstTvw.ImageStream"),System.Windows.Forms.ImageListStreamer)
		Me.imglstTvw.TransparentColor = System.Drawing.Color.Transparent
		Me.imglstTvw.Images.SetKeyName(0, "Blank.ico")
		Me.imglstTvw.Images.SetKeyName(1, "MTG.ico")
		Me.imglstTvw.Images.SetKeyName(2, "Deck.ico")
		Me.imglstTvw.Images.SetKeyName(3, "_twhite.ico")
		Me.imglstTvw.Images.SetKeyName(4, "_tartifact.ico")
		Me.imglstTvw.Images.SetKeyName(5, "_tblack.ico")
		Me.imglstTvw.Images.SetKeyName(6, "_tblue.ico")
		Me.imglstTvw.Images.SetKeyName(7, "_tgreen.ico")
		Me.imglstTvw.Images.SetKeyName(8, "_tland.ico")
		Me.imglstTvw.Images.SetKeyName(9, "_tmulticolor.ico")
		Me.imglstTvw.Images.SetKeyName(10, "_tred.ico")
		Me.imglstTvw.Images.SetKeyName(11, "_mx.gif")
		Me.imglstTvw.Images.SetKeyName(12, "_m0.gif")
		Me.imglstTvw.Images.SetKeyName(13, "_m1.gif")
		Me.imglstTvw.Images.SetKeyName(14, "_m2.gif")
		Me.imglstTvw.Images.SetKeyName(15, "_m3.gif")
		Me.imglstTvw.Images.SetKeyName(16, "_m4.gif")
		Me.imglstTvw.Images.SetKeyName(17, "_m5.gif")
		Me.imglstTvw.Images.SetKeyName(18, "_m6.gif")
		Me.imglstTvw.Images.SetKeyName(19, "_m7.gif")
		Me.imglstTvw.Images.SetKeyName(20, "_m8.gif")
		Me.imglstTvw.Images.SetKeyName(21, "_m9.gif")
		Me.imglstTvw.Images.SetKeyName(22, "_m10.gif")
		Me.imglstTvw.Images.SetKeyName(23, "_m11.gif")
		Me.imglstTvw.Images.SetKeyName(24, "_m12.gif")
		Me.imglstTvw.Images.SetKeyName(25, "_m13.gif")
		Me.imglstTvw.Images.SetKeyName(26, "_m14.gif")
		Me.imglstTvw.Images.SetKeyName(27, "_m15.gif")
		Me.imglstTvw.Images.SetKeyName(28, "_m16.gif")
		'
		'chkClassement
		'
		Me.chkClassement.CheckOnClick = true
		Me.chkClassement.Dock = System.Windows.Forms.DockStyle.Top
		Me.chkClassement.FormattingEnabled = true
		Me.chkClassement.Items.AddRange(New Object() {"Decks", "Type", "Couleur", "Edition", "Coût d'invocation", "Rareté", "Prix", "Carte"})
		Me.chkClassement.Location = New System.Drawing.Point(0, 0)
		Me.chkClassement.Name = "chkClassement"
		Me.chkClassement.Size = New System.Drawing.Size(295, 64)
		Me.chkClassement.TabIndex = 2
		AddHandler Me.chkClassement.SelectedIndexChanged, AddressOf Me.ChkClassementSelectedIndexChanged
		AddHandler Me.chkClassement.ItemCheck, AddressOf Me.ChkClassementItemCheck
		'
		'btUp
		'
		Me.btUp.Enabled = false
		Me.btUp.Image = CType(resources.GetObject("btUp.Image"),System.Drawing.Image)
		Me.btUp.Text = "Monter"
		AddHandler Me.btUp.Activate, AddressOf Me.BtUpActivate
		'
		'btDown
		'
		Me.btDown.Enabled = false
		Me.btDown.Image = CType(resources.GetObject("btDown.Image"),System.Drawing.Image)
		Me.btDown.Text = "Descendre"
		AddHandler Me.btDown.Activate, AddressOf Me.BtDownActivate
		'
		'btRefresh
		'
		Me.btRefresh.Icon = CType(resources.GetObject("btRefresh.Icon"),System.Drawing.Icon)
		Me.btRefresh.IconSize = New System.Drawing.Size(13, 13)
		Me.btRefresh.Text = "Rafraîchir"
		AddHandler Me.btRefresh.Activate, AddressOf Me.BtRefreshActivate
		'
		'splitV2
		'
		Me.splitV2.Dock = System.Windows.Forms.DockStyle.Fill
		Me.splitV2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
		Me.splitV2.IsSplitterFixed = true
		Me.splitV2.Location = New System.Drawing.Point(0, 0)
		Me.splitV2.Name = "splitV2"
		'
		'splitV2.Panel1
		'
		Me.splitV2.Panel1.Controls.Add(Me.CBarProperties)
		'
		'splitV2.Panel2
		'
		Me.splitV2.Panel2.Controls.Add(Me.CBarImage)
		Me.splitV2.Size = New System.Drawing.Size(454, 329)
		Me.splitV2.SplitterDistance = 231
		Me.splitV2.TabIndex = 0
		Me.splitV2.TabStop = false
		'
		'CBarProperties
		'
		Me.CBarProperties.Closable = false
		Me.CBarProperties.Controls.Add(Me.pnlProperties)
		Me.CBarProperties.Dock = System.Windows.Forms.DockStyle.Fill
		Me.CBarProperties.DrawActionsButton = false
		Me.CBarProperties.Flow = TD.SandBar.ToolBarLayout.Horizontal
		Me.CBarProperties.Guid = New System.Guid("ea1edb50-d1b7-4eab-b136-020bcdc24f2d")
		Me.CBarProperties.Location = New System.Drawing.Point(0, 0)
		Me.CBarProperties.Movable = false
		Me.CBarProperties.Name = "CBarProperties"
		Me.CBarProperties.Size = New System.Drawing.Size(231, 329)
		Me.CBarProperties.TabIndex = 0
		Me.CBarProperties.Text = "Propriétés"
		'
		'pnlProperties
		'
		Me.pnlProperties.Controls.Add(Me.grpCarac)
		Me.pnlProperties.Controls.Add(Me.grpSerie)
		Me.pnlProperties.Location = New System.Drawing.Point(2, 27)
		Me.pnlProperties.Name = "pnlProperties"
		Me.pnlProperties.Size = New System.Drawing.Size(227, 300)
		Me.pnlProperties.TabIndex = 0
		'
		'grpCarac
		'
		Me.grpCarac.BackColor = System.Drawing.Color.Transparent
		Me.grpCarac.Controls.Add(Me.txtCardText)
		Me.grpCarac.Controls.Add(Me.lblProp7)
		Me.grpCarac.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grpCarac.Location = New System.Drawing.Point(0, 138)
		Me.grpCarac.Name = "grpCarac"
		Me.grpCarac.Size = New System.Drawing.Size(227, 162)
		Me.grpCarac.TabIndex = 9
		Me.grpCarac.TabStop = false
		'
		'txtCardText
		'
		Me.txtCardText.Dock = System.Windows.Forms.DockStyle.Fill
		Me.txtCardText.Location = New System.Drawing.Point(3, 29)
		Me.txtCardText.Multiline = true
		Me.txtCardText.Name = "txtCardText"
		Me.txtCardText.Size = New System.Drawing.Size(221, 130)
		Me.txtCardText.TabIndex = 11
		'
		'lblProp7
		'
		Me.lblProp7.AutoSize = true
		Me.lblProp7.BackColor = System.Drawing.Color.Transparent
		Me.lblProp7.Dock = System.Windows.Forms.DockStyle.Top
		Me.lblProp7.Location = New System.Drawing.Point(3, 16)
		Me.lblProp7.Name = "lblProp7"
		Me.lblProp7.Size = New System.Drawing.Size(40, 13)
		Me.lblProp7.TabIndex = 10
		Me.lblProp7.Text = "Texte :"
		'
		'grpSerie
		'
		Me.grpSerie.BackColor = System.Drawing.Color.Transparent
		Me.grpSerie.Controls.Add(Me.cboEdition)
		Me.grpSerie.Controls.Add(Me.picEdition)
		Me.grpSerie.Controls.Add(Me.lblAD)
		Me.grpSerie.Controls.Add(Me.lblStock)
		Me.grpSerie.Controls.Add(Me.lblPrix)
		Me.grpSerie.Controls.Add(Me.lblRarete)
		Me.grpSerie.Controls.Add(Me.lblProp6)
		Me.grpSerie.Controls.Add(Me.lblProp1)
		Me.grpSerie.Controls.Add(Me.lblProp2)
		Me.grpSerie.Controls.Add(Me.lblProp5)
		Me.grpSerie.Controls.Add(Me.lblProp4)
		Me.grpSerie.Controls.Add(Me.lblProp3)
		Me.grpSerie.Dock = System.Windows.Forms.DockStyle.Top
		Me.grpSerie.Location = New System.Drawing.Point(0, 0)
		Me.grpSerie.Name = "grpSerie"
		Me.grpSerie.Size = New System.Drawing.Size(227, 138)
		Me.grpSerie.TabIndex = 8
		Me.grpSerie.TabStop = false
		'
		'cboEdition
		'
		Me.cboEdition.FormattingEnabled = true
		Me.cboEdition.Location = New System.Drawing.Point(163, 12)
		Me.cboEdition.Name = "cboEdition"
		Me.cboEdition.Size = New System.Drawing.Size(56, 21)
		Me.cboEdition.TabIndex = 19
		AddHandler Me.cboEdition.SelectedValueChanged, AddressOf Me.CboEditionSelectedValueChanged
		'
		'picEdition
		'
		Me.picEdition.Location = New System.Drawing.Point(119, 13)
		Me.picEdition.Name = "picEdition"
		Me.picEdition.Size = New System.Drawing.Size(18, 18)
		Me.picEdition.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me.picEdition.TabIndex = 18
		Me.picEdition.TabStop = false
		'
		'lblAD
		'
		Me.lblAD.BackColor = System.Drawing.Color.Transparent
		Me.lblAD.Location = New System.Drawing.Point(174, 95)
		Me.lblAD.Name = "lblAD"
		Me.lblAD.Size = New System.Drawing.Size(45, 13)
		Me.lblAD.TabIndex = 15
		Me.lblAD.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblStock
		'
		Me.lblStock.BackColor = System.Drawing.Color.Transparent
		Me.lblStock.Location = New System.Drawing.Point(174, 75)
		Me.lblStock.Name = "lblStock"
		Me.lblStock.Size = New System.Drawing.Size(45, 13)
		Me.lblStock.TabIndex = 14
		Me.lblStock.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblPrix
		'
		Me.lblPrix.BackColor = System.Drawing.Color.Transparent
		Me.lblPrix.Location = New System.Drawing.Point(174, 55)
		Me.lblPrix.Name = "lblPrix"
		Me.lblPrix.Size = New System.Drawing.Size(45, 13)
		Me.lblPrix.TabIndex = 13
		Me.lblPrix.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblRarete
		'
		Me.lblRarete.BackColor = System.Drawing.Color.Transparent
		Me.lblRarete.Location = New System.Drawing.Point(120, 35)
		Me.lblRarete.Name = "lblRarete"
		Me.lblRarete.Size = New System.Drawing.Size(99, 13)
		Me.lblRarete.TabIndex = 12
		Me.lblRarete.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblProp6
		'
		Me.lblProp6.AutoSize = true
		Me.lblProp6.BackColor = System.Drawing.Color.Transparent
		Me.lblProp6.Location = New System.Drawing.Point(5, 95)
		Me.lblProp6.Name = "lblProp6"
		Me.lblProp6.Size = New System.Drawing.Size(101, 13)
		Me.lblProp6.TabIndex = 10
		Me.lblProp6.Text = "Attaque / Défense :"
		'
		'lblProp1
		'
		Me.lblProp1.AutoSize = true
		Me.lblProp1.BackColor = System.Drawing.Color.Transparent
		Me.lblProp1.Location = New System.Drawing.Point(5, 115)
		Me.lblProp1.Name = "lblProp1"
		Me.lblProp1.Size = New System.Drawing.Size(63, 13)
		Me.lblProp1.TabIndex = 9
		Me.lblProp1.Text = "Invocation :"
		'
		'lblProp2
		'
		Me.lblProp2.AutoSize = true
		Me.lblProp2.BackColor = System.Drawing.Color.Transparent
		Me.lblProp2.Location = New System.Drawing.Point(5, 75)
		Me.lblProp2.Name = "lblProp2"
		Me.lblProp2.Size = New System.Drawing.Size(41, 13)
		Me.lblProp2.TabIndex = 8
		Me.lblProp2.Text = "Stock :"
		'
		'lblProp5
		'
		Me.lblProp5.AutoSize = true
		Me.lblProp5.BackColor = System.Drawing.Color.Transparent
		Me.lblProp5.Location = New System.Drawing.Point(5, 55)
		Me.lblProp5.Name = "lblProp5"
		Me.lblProp5.Size = New System.Drawing.Size(30, 13)
		Me.lblProp5.TabIndex = 7
		Me.lblProp5.Text = "Prix :"
		'
		'lblProp4
		'
		Me.lblProp4.AutoSize = true
		Me.lblProp4.BackColor = System.Drawing.Color.Transparent
		Me.lblProp4.Location = New System.Drawing.Point(5, 35)
		Me.lblProp4.Name = "lblProp4"
		Me.lblProp4.Size = New System.Drawing.Size(45, 13)
		Me.lblProp4.TabIndex = 6
		Me.lblProp4.Text = "Rareté :"
		'
		'lblProp3
		'
		Me.lblProp3.AutoSize = true
		Me.lblProp3.BackColor = System.Drawing.Color.Transparent
		Me.lblProp3.Location = New System.Drawing.Point(5, 15)
		Me.lblProp3.Name = "lblProp3"
		Me.lblProp3.Size = New System.Drawing.Size(45, 13)
		Me.lblProp3.TabIndex = 5
		Me.lblProp3.Text = "Edition :"
		'
		'CBarImage
		'
		Me.CBarImage.Closable = false
		Me.CBarImage.Controls.Add(Me.pnlImage)
		Me.CBarImage.Dock = System.Windows.Forms.DockStyle.Fill
		Me.CBarImage.DrawActionsButton = false
		Me.CBarImage.Flow = TD.SandBar.ToolBarLayout.Horizontal
		Me.CBarImage.Guid = New System.Guid("ea1edb50-d1b7-4eab-b136-020bcdc24f2d")
		Me.CBarImage.Location = New System.Drawing.Point(0, 0)
		Me.CBarImage.Movable = false
		Me.CBarImage.Name = "CBarImage"
		Me.CBarImage.Size = New System.Drawing.Size(219, 329)
		Me.CBarImage.TabIndex = 1
		Me.CBarImage.Text = "Image"
		'
		'pnlImage
		'
		Me.pnlImage.Controls.Add(Me.picScanCard)
		Me.pnlImage.Location = New System.Drawing.Point(2, 27)
		Me.pnlImage.Name = "pnlImage"
		Me.pnlImage.Size = New System.Drawing.Size(215, 300)
		Me.pnlImage.TabIndex = 0
		'
		'picScanCard
		'
		Me.picScanCard.Dock = System.Windows.Forms.DockStyle.Fill
		Me.picScanCard.Location = New System.Drawing.Point(0, 0)
		Me.picScanCard.Name = "picScanCard"
		Me.picScanCard.Size = New System.Drawing.Size(215, 300)
		Me.picScanCard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me.picScanCard.TabIndex = 0
		Me.picScanCard.TabStop = false
		'
		'imglstCarac
		'
		Me.imglstCarac.ImageStream = CType(resources.GetObject("imglstCarac.ImageStream"),System.Windows.Forms.ImageListStreamer)
		Me.imglstCarac.TransparentColor = System.Drawing.Color.Transparent
		Me.imglstCarac.Images.SetKeyName(0, "_lb.gif")
		Me.imglstCarac.Images.SetKeyName(1, "_lbg.gif")
		Me.imglstCarac.Images.SetKeyName(2, "_lbr.gif")
		Me.imglstCarac.Images.SetKeyName(3, "_lg.gif")
		Me.imglstCarac.Images.SetKeyName(4, "_lgu.gif")
		Me.imglstCarac.Images.SetKeyName(5, "_lgw.gif")
		Me.imglstCarac.Images.SetKeyName(6, "_lr.gif")
		Me.imglstCarac.Images.SetKeyName(7, "_lrg.gif")
		Me.imglstCarac.Images.SetKeyName(8, "_lrw.gif")
		Me.imglstCarac.Images.SetKeyName(9, "_lu.gif")
		Me.imglstCarac.Images.SetKeyName(10, "_lub.gif")
		Me.imglstCarac.Images.SetKeyName(11, "_lur.gif")
		Me.imglstCarac.Images.SetKeyName(12, "_lw.gif")
		Me.imglstCarac.Images.SetKeyName(13, "_lwb.gif")
		Me.imglstCarac.Images.SetKeyName(14, "_lwu.gif")
		Me.imglstCarac.Images.SetKeyName(15, "_mx.gif")
		Me.imglstCarac.Images.SetKeyName(16, "_m0.gif")
		Me.imglstCarac.Images.SetKeyName(17, "_m1.gif")
		Me.imglstCarac.Images.SetKeyName(18, "_m2.gif")
		Me.imglstCarac.Images.SetKeyName(19, "_m3.gif")
		Me.imglstCarac.Images.SetKeyName(20, "_m4.gif")
		Me.imglstCarac.Images.SetKeyName(21, "_m5.gif")
		Me.imglstCarac.Images.SetKeyName(22, "_m6.gif")
		Me.imglstCarac.Images.SetKeyName(23, "_m7.gif")
		Me.imglstCarac.Images.SetKeyName(24, "_m8.gif")
		Me.imglstCarac.Images.SetKeyName(25, "_m9.gif")
		Me.imglstCarac.Images.SetKeyName(26, "_m10.gif")
		Me.imglstCarac.Images.SetKeyName(27, "_m11.gif")
		Me.imglstCarac.Images.SetKeyName(28, "_m12.gif")
		Me.imglstCarac.Images.SetKeyName(29, "_m13.gif")
		Me.imglstCarac.Images.SetKeyName(30, "_m14.gif")
		Me.imglstCarac.Images.SetKeyName(31, "_m15.gif")
		Me.imglstCarac.Images.SetKeyName(32, "_m16.gif")
		'
		'cmnuTvw
		'
		Me.cmnuTvw.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuCardsFR, Me.mnuSort, Me.mnuSearchCard, Me.mnucAddCards, Me.mnuSeparator, Me.mnuMoveACard, Me.mnuDeleteACard, Me.mnuSeparator2, Me.mnuBuy})
		Me.cmnuTvw.Name = "cmnuTvw"
		Me.cmnuTvw.Size = New System.Drawing.Size(232, 170)
		'
		'mnuCardsFR
		'
		Me.mnuCardsFR.Enabled = false
		Me.mnuCardsFR.Image = CType(resources.GetObject("mnuCardsFR.Image"),System.Drawing.Image)
		Me.mnuCardsFR.Name = "mnuCardsFR"
		Me.mnuCardsFR.Size = New System.Drawing.Size(231, 22)
		Me.mnuCardsFR.Text = "Titre des cartes en français"
		AddHandler Me.mnuCardsFR.MouseUp, AddressOf Me.MnuCardsFRActivate
		'
		'mnuSort
		'
		Me.mnuSort.Image = CType(resources.GetObject("mnuSort.Image"),System.Drawing.Image)
		Me.mnuSort.Name = "mnuSort"
		Me.mnuSort.Size = New System.Drawing.Size(231, 22)
		Me.mnuSort.Text = "Trier par ordre alphabétique"
		AddHandler Me.mnuSort.Click, AddressOf Me.MnuSortClick
		'
		'mnuSearchCard
		'
		Me.mnuSearchCard.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSearchText, Me.mnuFindNext})
		Me.mnuSearchCard.Image = CType(resources.GetObject("mnuSearchCard.Image"),System.Drawing.Image)
		Me.mnuSearchCard.Name = "mnuSearchCard"
		Me.mnuSearchCard.Size = New System.Drawing.Size(231, 22)
		Me.mnuSearchCard.Text = "Rechercher"
		AddHandler Me.mnuSearchCard.Click, AddressOf Me.MnuSearchCardClick
		'
		'mnuSearchText
		'
		Me.mnuSearchText.Name = "mnuSearchText"
		Me.mnuSearchText.Size = New System.Drawing.Size(100, 21)
		Me.mnuSearchText.Text = "(carte)"
		AddHandler Me.mnuSearchText.KeyUp, AddressOf Me.MnuSearchTextKeyUp
		'
		'mnuFindNext
		'
		Me.mnuFindNext.Enabled = false
		Me.mnuFindNext.Name = "mnuFindNext"
		Me.mnuFindNext.ShortcutKeys = System.Windows.Forms.Keys.F3
		Me.mnuFindNext.Size = New System.Drawing.Size(214, 22)
		Me.mnuFindNext.Text = "Rechercher la suivante"
		AddHandler Me.mnuFindNext.Click, AddressOf Me.MnuFindNextClick
		'
		'mnucAddCards
		'
		Me.mnucAddCards.Image = CType(resources.GetObject("mnucAddCards.Image"),System.Drawing.Image)
		Me.mnucAddCards.Name = "mnucAddCards"
		Me.mnucAddCards.Size = New System.Drawing.Size(231, 22)
		Me.mnucAddCards.Text = "Ajouter / supprimer des cartes"
		AddHandler Me.mnucAddCards.Click, AddressOf Me.MnuAddCardsActivate
		'
		'mnuSeparator
		'
		Me.mnuSeparator.Name = "mnuSeparator"
		Me.mnuSeparator.Size = New System.Drawing.Size(228, 6)
		'
		'mnuMoveACard
		'
		Me.mnuMoveACard.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuMoveToCollection})
		Me.mnuMoveACard.Enabled = false
		Me.mnuMoveACard.Image = CType(resources.GetObject("mnuMoveACard.Image"),System.Drawing.Image)
		Me.mnuMoveACard.Name = "mnuMoveACard"
		Me.mnuMoveACard.Size = New System.Drawing.Size(231, 22)
		Me.mnuMoveACard.Text = "Déplacer vers..."
		'
		'mnuMoveToCollection
		'
		Me.mnuMoveToCollection.Name = "mnuMoveToCollection"
		Me.mnuMoveToCollection.Size = New System.Drawing.Size(131, 22)
		Me.mnuMoveToCollection.Text = "Collection"
		AddHandler Me.mnuMoveToCollection.Click, AddressOf Me.MnuMoveACardActivate
		'
		'mnuDeleteACard
		'
		Me.mnuDeleteACard.Enabled = false
		Me.mnuDeleteACard.Image = CType(resources.GetObject("mnuDeleteACard.Image"),System.Drawing.Image)
		Me.mnuDeleteACard.Name = "mnuDeleteACard"
		Me.mnuDeleteACard.Size = New System.Drawing.Size(231, 22)
		Me.mnuDeleteACard.Text = "Supprimer..."
		AddHandler Me.mnuDeleteACard.Click, AddressOf Me.MnuDeleteACardClick
		'
		'mnuSeparator2
		'
		Me.mnuSeparator2.Name = "mnuSeparator2"
		Me.mnuSeparator2.Size = New System.Drawing.Size(228, 6)
		'
		'mnuBuy
		'
		Me.mnuBuy.Enabled = false
		Me.mnuBuy.Image = CType(resources.GetObject("mnuBuy.Image"),System.Drawing.Image)
		Me.mnuBuy.Name = "mnuBuy"
		Me.mnuBuy.Size = New System.Drawing.Size(231, 22)
		Me.mnuBuy.Text = "Acheter sur Magic-Ville"
		AddHandler Me.mnuBuy.Click, AddressOf Me.MnuBuyClick
		'
		'dlgOpen2
		'
		Me.dlgOpen2.DefaultExt = "txt"
		Me.dlgOpen2.Filter = "Text File (*.txt) | *.txt"
		Me.dlgOpen2.Title = "Sélection du fichier de cotation"
		'
		'dlgOpen3
		'
		Me.dlgOpen3.DefaultExt = "dat"
		Me.dlgOpen3.Filter = "Data File (*.dat) | *.dat"
		Me.dlgOpen3.Title = "Sélection du fichier de données d'images"
		'
		'dlgOpen4
		'
		Me.dlgOpen4.DefaultExt = "txt"
		Me.dlgOpen4.Filter = "Fichiers texte (*.txt) | *.txt"
		Me.dlgOpen4.Title = "Sélection du fichier spoiler"
		'
		'MainForm
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(757, 372)
		Me.Controls.Add(Me.splitV)
		Me.Controls.Add(Me.statusStrip)
		Me.Controls.Add(Me.mnuMenuStrip)
		Me.HelpButton = true
		Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
		Me.Name = "MainForm"
		Me.Text = "Magic The Gathering Manager"
		AddHandler Load, AddressOf Me.MainFormLoad
		AddHandler FormClosing, AddressOf Me.MainFormFormClosing
		Me.statusStrip.ResumeLayout(false)
		Me.statusStrip.PerformLayout
		Me.splitV.Panel1.ResumeLayout(false)
		Me.splitV.Panel2.ResumeLayout(false)
		Me.splitV.ResumeLayout(false)
		Me.CBarTvw.ResumeLayout(false)
		Me.pnlTvw.ResumeLayout(false)
		Me.splitV2.Panel1.ResumeLayout(false)
		Me.splitV2.Panel2.ResumeLayout(false)
		Me.splitV2.ResumeLayout(false)
		Me.CBarProperties.ResumeLayout(false)
		Me.pnlProperties.ResumeLayout(false)
		Me.grpCarac.ResumeLayout(false)
		Me.grpCarac.PerformLayout
		Me.grpSerie.ResumeLayout(false)
		Me.grpSerie.PerformLayout
		CType(Me.picEdition,System.ComponentModel.ISupportInitialize).EndInit
		Me.CBarImage.ResumeLayout(false)
		Me.pnlImage.ResumeLayout(false)
		CType(Me.picScanCard,System.ComponentModel.ISupportInitialize).EndInit
		Me.cmnuTvw.ResumeLayout(false)
		Me.ResumeLayout(false)
		Me.PerformLayout
	End Sub
	Private mnuUpdateSimu As TD.SandBar.MenuButtonItem
	Private mnuBuy As System.Windows.Forms.ToolStripMenuItem
	Private mnuSeparator2 As System.Windows.Forms.ToolStripSeparator
	Private imglstTvw As System.Windows.Forms.ImageList
	Private dlgOpen4 As System.Windows.Forms.OpenFileDialog
	Private mnuFixSerie As TD.SandBar.MenuButtonItem
	Private mnuCheckForBetas As TD.SandBar.MenuButtonItem
	Private mnuShowImage As TD.SandBar.MenuButtonItem
	Private pnlImage As TD.SandBar.ContainerBarClientPanel
	Private CBarImage As TD.SandBar.ContainerBar
	Private mnuMoveToCollection As System.Windows.Forms.ToolStripMenuItem
	Private picScanCard As System.Windows.Forms.PictureBox
	Private splitV2 As System.Windows.Forms.SplitContainer
	Private dlgOpen3 As System.Windows.Forms.OpenFileDialog
	Private mnuUpdatePictures As TD.SandBar.MenuButtonItem
	Private mnuGestDecks As TD.SandBar.MenuButtonItem
	Private mnuExport As TD.SandBar.MenuButtonItem
	Private mnuSimu As TD.SandBar.MenuButtonItem
	Private mnuRemEdition As TD.SandBar.MenuButtonItem
	Private mnuExcelGen As TD.SandBar.MenuButtonItem
	Private mnuDeleteACard As System.Windows.Forms.ToolStripMenuItem
	Private mnuMoveACard As System.Windows.Forms.ToolStripMenuItem
	Private mnuSeparator As System.Windows.Forms.ToolStripSeparator
	Private mnuRemScores As TD.SandBar.MenuButtonItem
	Public mnuPerfs As TD.SandBar.MenuButtonItem
	Private mnucAddCards As System.Windows.Forms.ToolStripMenuItem
	Private mnuCheckForUpdates As TD.SandBar.MenuButtonItem
	Public prgAvance As System.Windows.Forms.ToolStripProgressBar
	Private mnuFixFR As TD.SandBar.MenuButtonItem
	Private mnuDBSelect As TD.SandBar.MenuButtonItem
	Private mnuTranslate As TD.SandBar.MenuButtonItem
	Private mnuFixPrices As TD.SandBar.MenuButtonItem
	Private mnuNewEdition As TD.SandBar.MenuButtonItem
	Private mnuFixGames As TD.SandBar.MenuButtonItem
	Private mnuFixCollec As TD.SandBar.MenuButtonItem
	Private mnuFixTable As TD.SandBar.MenuButtonItem
	Private mnuSort As System.Windows.Forms.ToolStripMenuItem
	Private mnuHelp As TD.SandBar.MenuButtonItem
	Private mnuStats As TD.SandBar.MenuButtonItem
	Private dlgOpen2 As System.Windows.Forms.OpenFileDialog
	Private mnuAdvancedSearch As TD.SandBar.MenuButtonItem
	Private mnuStdSearch As TD.SandBar.MenuButtonItem
	Private mnuBigSearch As TD.SandBar.MenuBarItem
	Private mnuUpdatePrices As TD.SandBar.MenuButtonItem
	Private mnuFindNext As System.Windows.Forms.ToolStripMenuItem
	Private mnuSearchText As System.Windows.Forms.ToolStripTextBox
	Private mnuSearchCard As System.Windows.Forms.ToolStripMenuItem
	Public cboEdition As System.Windows.Forms.ComboBox
	Private lblNCards As System.Windows.Forms.ToolStripStatusLabel
	Private btRefresh As TD.SandBar.ButtonItem
	Public picEdition As System.Windows.Forms.PictureBox
	Public imglstCarac As System.Windows.Forms.ImageList
	Public lblRarete As System.Windows.Forms.Label
	Public lblPrix As System.Windows.Forms.Label
	Public lblStock As System.Windows.Forms.Label
	Public lblAD As System.Windows.Forms.Label
	Public grpSerie As System.Windows.Forms.GroupBox
	Private grpCarac As System.Windows.Forms.GroupBox
	Public txtCardText As System.Windows.Forms.TextBox
	Public lblProp1 As System.Windows.Forms.Label
	Private lblProp2 As System.Windows.Forms.Label
	Private lblProp3 As System.Windows.Forms.Label
	Private lblProp4 As System.Windows.Forms.Label
	Public lblProp5 As System.Windows.Forms.Label
	Public lblProp6 As System.Windows.Forms.Label
	Private lblProp7 As System.Windows.Forms.Label
	Public mnuCardsFR As System.Windows.Forms.ToolStripMenuItem
	Private cmnuTvw As System.Windows.Forms.ContextMenuStrip
	Private btDown As TD.SandBar.ButtonItem
	Private btUp As TD.SandBar.ButtonItem
	Public chkClassement As System.Windows.Forms.CheckedListBox
	Private mnuRefresh As TD.SandBar.MenuButtonItem
	Private mnuDispCollection As TD.SandBar.MenuButtonItem
	Public mnuDisp As TD.SandBar.MenuBarItem
	Public tvwExplore As TreeViewMS.TreeViewMS
	Private pnlProperties As TD.SandBar.ContainerBarClientPanel
	Private CBarProperties As TD.SandBar.ContainerBar
	Private pnlTvw As TD.SandBar.ContainerBarClientPanel
	Private CBarTvw As TD.SandBar.ContainerBar
	Private splitV As System.Windows.Forms.SplitContainer
	Private mnuRemGames As TD.SandBar.MenuButtonItem
	Private mnuRemCollec As TD.SandBar.MenuButtonItem
	Private mnuRemCards As TD.SandBar.MenuButtonItem
	Private mnuPrefs As TD.SandBar.MenuButtonItem
	Private dlgOpen As System.Windows.Forms.OpenFileDialog
	Private mnuMenuStrip As TD.SandBar.MenuBar
	Private mnuTools As TD.SandBar.MenuBarItem
	Private lblDB As System.Windows.Forms.ToolStripStatusLabel
	Private statusStrip As System.Windows.Forms.StatusStrip
	Private mnuAddCards As TD.SandBar.MenuButtonItem
	Private mnuAbout As TD.SandBar.MenuButtonItem
	Private mnuInfo As TD.SandBar.MenuBarItem
	Private mnuExit As TD.SandBar.MenuButtonItem
	Private mnuFile As TD.SandBar.MenuBarItem
End Class
