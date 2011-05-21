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
		Me.statusStrip = New System.Windows.Forms.StatusStrip
		Me.lblDB = New System.Windows.Forms.ToolStripStatusLabel
		Me.lblNCards = New System.Windows.Forms.ToolStripStatusLabel
		Me.prgAvance = New System.Windows.Forms.ToolStripProgressBar
		Me.btDownload = New System.Windows.Forms.ToolStripSplitButton
		Me.mnuInfosDL = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuCancel = New System.Windows.Forms.ToolStripMenuItem
		Me.dlgOpen = New System.Windows.Forms.OpenFileDialog
		Me.imglstTvw = New System.Windows.Forms.ImageList(Me.components)
		Me.imglstCarac = New System.Windows.Forms.ImageList(Me.components)
		Me.cmnuTvw = New System.Windows.Forms.ContextMenuStrip(Me.components)
		Me.mnuCardsFR = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuSort = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuDegroupFoils = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuSearchCard = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuSearchText = New System.Windows.Forms.ToolStripTextBox
		Me.mnuFindNext = New System.Windows.Forms.ToolStripMenuItem
		Me.mnucAddCards = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuSeparator1 = New System.Windows.Forms.ToolStripSeparator
		Me.mnuMoveACard = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuMoveToCollection = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuCopyACard = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuCopyToCollection = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuDeleteACard = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuSeparator2 = New System.Windows.Forms.ToolStripSeparator
		Me.mnuBuy = New System.Windows.Forms.ToolStripMenuItem
		Me.dlgOpen2 = New System.Windows.Forms.OpenFileDialog
		Me.dlgOpen3 = New System.Windows.Forms.OpenFileDialog
		Me.dlgOpen4 = New System.Windows.Forms.OpenFileDialog
		Me.mnu = New System.Windows.Forms.MenuStrip
		Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuDBSelect = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuDBOpen = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuDBSave = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuExport = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuSeparator3 = New System.Windows.Forms.ToolStripSeparator
		Me.mnuNewEdition = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuRemEdition = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuTranslate = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuSeparator4 = New System.Windows.Forms.ToolStripSeparator
		Me.mnuUpdatePrices = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuUpdatePictures = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuUpdateAutorisations = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuUpdateSimu = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuUpdateTxtFR = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuSeparator5 = New System.Windows.Forms.ToolStripSeparator
		Me.mnuExit = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuDisp = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuRefresh = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuShowImage = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuSeparator6 = New System.Windows.Forms.ToolStripSeparator
		Me.mnuDispCollection = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuTools = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuGestDecks = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuGestAdv = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuAddCards = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuRemCards = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuRemScores = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuRemCollec = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuRemGames = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuFixTable = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuFixPrices = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuFixFR = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuFixCreatures = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuFixSerie = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuFixSerie2 = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuSeparator10 = New System.Windows.Forms.ToolStripSeparator
		Me.mnuFixCollec = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuFixGames = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuFixDivers = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuFixAssoc = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuFixPic = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuFixFR2 = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuSeparator7 = New System.Windows.Forms.ToolStripSeparator
		Me.mnuPrefs = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuBigSearch = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuStdSearch = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuAdvancedSearch = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuSeparator8 = New System.Windows.Forms.ToolStripSeparator
		Me.mnuExcelGen = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuWordGen = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuPerfs = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuSimu = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuStats = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuMV = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuPlugins = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuPlugResourcer = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuInfo = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuCheckForUpdates = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuApplicationUpdate = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuContenuUpdate = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuCheckForBetas = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuRestorePrev = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuSeparator9 = New System.Windows.Forms.ToolStripSeparator
		Me.mnuWebsite = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuAbout = New System.Windows.Forms.ToolStripMenuItem
		Me.toolStrip = New System.Windows.Forms.ToolStrip
		Me.btDBSelect = New System.Windows.Forms.ToolStripButton
		Me.btExport = New System.Windows.Forms.ToolStripButton
		Me.btSeparator1 = New System.Windows.Forms.ToolStripSeparator
		Me.btGestDecks = New System.Windows.Forms.ToolStripButton
		Me.btAddCards = New System.Windows.Forms.ToolStripButton
		Me.btAdvancedSearch = New System.Windows.Forms.ToolStripButton
		Me.btSeparator2 = New System.Windows.Forms.ToolStripSeparator
		Me.btExcelGen = New System.Windows.Forms.ToolStripButton
		Me.btWordGen = New System.Windows.Forms.ToolStripButton
		Me.btSimu = New System.Windows.Forms.ToolStripButton
		Me.btStats = New System.Windows.Forms.ToolStripButton
		Me.btCheckForUpdates = New System.Windows.Forms.ToolStripButton
		Me.btWebsite = New System.Windows.Forms.ToolStripButton
		Me.splitV = New System.Windows.Forms.SplitContainer
		Me.CBarTvw = New TD.SandBar.ContainerBar
		Me.pnlTvw = New TD.SandBar.ContainerBarClientPanel
		Me.splitH = New System.Windows.Forms.SplitContainer
		Me.chkClassement = New System.Windows.Forms.CheckedListBox
		Me.tvwExplore = New TreeViewMS.TreeViewMS
		Me.btUp = New TD.SandBar.ButtonItem
		Me.btDown = New TD.SandBar.ButtonItem
		Me.btRefresh = New TD.SandBar.ButtonItem
		Me.splitV2 = New System.Windows.Forms.SplitContainer
		Me.CBarProperties = New TD.SandBar.ContainerBar
		Me.pnlProperties = New TD.SandBar.ContainerBarClientPanel
		Me.grpCarac = New System.Windows.Forms.GroupBox
		Me.txtCardText = New System.Windows.Forms.TextBox
		Me.grpAutorisations = New System.Windows.Forms.GroupBox
		Me.picAutT1 = New System.Windows.Forms.PictureBox
		Me.picAutT15 = New System.Windows.Forms.PictureBox
		Me.picAutT1x = New System.Windows.Forms.PictureBox
		Me.picAutT2 = New System.Windows.Forms.PictureBox
		Me.picAutBloc = New System.Windows.Forms.PictureBox
		Me.grpSerie = New System.Windows.Forms.GroupBox
		Me.cmdHistPrices = New System.Windows.Forms.Button
		Me.scrollStock = New System.Windows.Forms.VScrollBar
		Me.cboEdition = New System.Windows.Forms.ComboBox
		Me.picEdition = New System.Windows.Forms.PictureBox
		Me.lblAD = New System.Windows.Forms.Label
		Me.lblPrix = New System.Windows.Forms.Label
		Me.lblRarete = New System.Windows.Forms.Label
		Me.lblProp6 = New System.Windows.Forms.Label
		Me.lblProp1 = New System.Windows.Forms.Label
		Me.lblProp2 = New System.Windows.Forms.Label
		Me.lblProp5 = New System.Windows.Forms.Label
		Me.lblProp4 = New System.Windows.Forms.Label
		Me.lblProp3 = New System.Windows.Forms.Label
		Me.lblStock = New System.Windows.Forms.Label
		Me.grpSerie2 = New System.Windows.Forms.GroupBox
		Me.lblSerieDate = New System.Windows.Forms.Label
		Me.lblSerieCote = New System.Windows.Forms.Label
		Me.lblSerieMyTotDist = New System.Windows.Forms.Label
		Me.lblSerieMyTot = New System.Windows.Forms.Label
		Me.lblSerieTot = New System.Windows.Forms.Label
		Me.lblProp12 = New System.Windows.Forms.Label
		Me.lblProp11 = New System.Windows.Forms.Label
		Me.lblProp10 = New System.Windows.Forms.Label
		Me.lblProp9 = New System.Windows.Forms.Label
		Me.lblProp8 = New System.Windows.Forms.Label
		Me.CBarImage = New TD.SandBar.ContainerBar
		Me.pnlImage = New TD.SandBar.ContainerBarClientPanel
		Me.picScanCard = New System.Windows.Forms.PictureBox
		Me.dlgSave = New System.Windows.Forms.SaveFileDialog
		Me.imglstAutorisations = New System.Windows.Forms.ImageList(Me.components)
		Me.mnuSwapSerie = New System.Windows.Forms.ToolStripMenuItem
		Me.statusStrip.SuspendLayout
		Me.cmnuTvw.SuspendLayout
		Me.mnu.SuspendLayout
		Me.toolStrip.SuspendLayout
		Me.splitV.Panel1.SuspendLayout
		Me.splitV.Panel2.SuspendLayout
		Me.splitV.SuspendLayout
		Me.CBarTvw.SuspendLayout
		Me.pnlTvw.SuspendLayout
		Me.splitH.Panel1.SuspendLayout
		Me.splitH.Panel2.SuspendLayout
		Me.splitH.SuspendLayout
		Me.splitV2.Panel1.SuspendLayout
		Me.splitV2.Panel2.SuspendLayout
		Me.splitV2.SuspendLayout
		Me.CBarProperties.SuspendLayout
		Me.pnlProperties.SuspendLayout
		Me.grpCarac.SuspendLayout
		Me.grpAutorisations.SuspendLayout
		CType(Me.picAutT1,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.picAutT15,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.picAutT1x,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.picAutT2,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.picAutBloc,System.ComponentModel.ISupportInitialize).BeginInit
		Me.grpSerie.SuspendLayout
		CType(Me.picEdition,System.ComponentModel.ISupportInitialize).BeginInit
		Me.grpSerie2.SuspendLayout
		Me.CBarImage.SuspendLayout
		Me.pnlImage.SuspendLayout
		CType(Me.picScanCard,System.ComponentModel.ISupportInitialize).BeginInit
		Me.SuspendLayout
		'
		'statusStrip
		'
		Me.statusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblDB, Me.lblNCards, Me.prgAvance, Me.btDownload})
		Me.statusStrip.Location = New System.Drawing.Point(0, 375)
		Me.statusStrip.Name = "statusStrip"
		Me.statusStrip.Size = New System.Drawing.Size(757, 22)
		Me.statusStrip.TabIndex = 2
		Me.statusStrip.Text = "statusStrip1"
		'
		'lblDB
		'
		Me.lblDB.Name = "lblDB"
		Me.lblDB.Size = New System.Drawing.Size(39, 17)
		Me.lblDB.Text = "Base -"
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
		'btDownload
		'
		Me.btDownload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.btDownload.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuInfosDL, Me.mnuCancel})
		Me.btDownload.Image = CType(resources.GetObject("btDownload.Image"),System.Drawing.Image)
		Me.btDownload.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btDownload.Name = "btDownload"
		Me.btDownload.Size = New System.Drawing.Size(32, 20)
		Me.btDownload.Visible = false
		AddHandler Me.btDownload.ButtonClick, AddressOf Me.MnuCancelClick
		'
		'mnuInfosDL
		'
		Me.mnuInfosDL.Name = "mnuInfosDL"
		Me.mnuInfosDL.Size = New System.Drawing.Size(142, 22)
		Me.mnuInfosDL.Text = "Informations"
		AddHandler Me.mnuInfosDL.Click, AddressOf Me.MnuInfosDLClick
		'
		'mnuCancel
		'
		Me.mnuCancel.Name = "mnuCancel"
		Me.mnuCancel.Size = New System.Drawing.Size(142, 22)
		Me.mnuCancel.Text = "Annuler"
		AddHandler Me.mnuCancel.Click, AddressOf Me.MnuCancelClick
		'
		'dlgOpen
		'
		Me.dlgOpen.DefaultExt = "mdb"
		Me.dlgOpen.Filter = "Fichiers de base de données Microsoft Access (*.mdb)|*.mdb"
		Me.dlgOpen.Title = "Sélection de la base de données"
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
		Me.imglstTvw.Images.SetKeyName(11, "_ttocken.ico")
		Me.imglstTvw.Images.SetKeyName(12, "_mx.gif")
		Me.imglstTvw.Images.SetKeyName(13, "_m0.gif")
		Me.imglstTvw.Images.SetKeyName(14, "_m1.gif")
		Me.imglstTvw.Images.SetKeyName(15, "_m2.gif")
		Me.imglstTvw.Images.SetKeyName(16, "_m3.gif")
		Me.imglstTvw.Images.SetKeyName(17, "_m4.gif")
		Me.imglstTvw.Images.SetKeyName(18, "_m5.gif")
		Me.imglstTvw.Images.SetKeyName(19, "_m6.gif")
		Me.imglstTvw.Images.SetKeyName(20, "_m7.gif")
		Me.imglstTvw.Images.SetKeyName(21, "_m8.gif")
		Me.imglstTvw.Images.SetKeyName(22, "_m9.gif")
		Me.imglstTvw.Images.SetKeyName(23, "_m10.gif")
		Me.imglstTvw.Images.SetKeyName(24, "_m11.gif")
		Me.imglstTvw.Images.SetKeyName(25, "_m12.gif")
		Me.imglstTvw.Images.SetKeyName(26, "_m13.gif")
		Me.imglstTvw.Images.SetKeyName(27, "_m14.gif")
		Me.imglstTvw.Images.SetKeyName(28, "_m15.gif")
		Me.imglstTvw.Images.SetKeyName(29, "_m16.gif")
		Me.imglstTvw.Images.SetKeyName(30, "_rco.gif")
		Me.imglstTvw.Images.SetKeyName(31, "_runco.gif")
		Me.imglstTvw.Images.SetKeyName(32, "_rrare.gif")
		Me.imglstTvw.Images.SetKeyName(33, "_p0.png")
		Me.imglstTvw.Images.SetKeyName(34, "_p1.png")
		Me.imglstTvw.Images.SetKeyName(35, "_p2.png")
		Me.imglstTvw.Images.SetKeyName(36, "_p3.png")
		Me.imglstTvw.Images.SetKeyName(37, "_p4.png")
		Me.imglstTvw.Images.SetKeyName(38, "_p5.png")
		Me.imglstTvw.Images.SetKeyName(39, "_p6.png")
		Me.imglstTvw.Images.SetKeyName(40, "_p7.png")
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
		Me.imglstCarac.Images.SetKeyName(15, "_lpb.gif")
		Me.imglstCarac.Images.SetKeyName(16, "_lpg.gif")
		Me.imglstCarac.Images.SetKeyName(17, "_lpr.gif")
		Me.imglstCarac.Images.SetKeyName(18, "_lpu.gif")
		Me.imglstCarac.Images.SetKeyName(19, "_lpw.gif")
		Me.imglstCarac.Images.SetKeyName(20, "_mx.gif")
		Me.imglstCarac.Images.SetKeyName(21, "_m0.gif")
		Me.imglstCarac.Images.SetKeyName(22, "_m1.gif")
		Me.imglstCarac.Images.SetKeyName(23, "_m2.gif")
		Me.imglstCarac.Images.SetKeyName(24, "_m3.gif")
		Me.imglstCarac.Images.SetKeyName(25, "_m4.gif")
		Me.imglstCarac.Images.SetKeyName(26, "_m5.gif")
		Me.imglstCarac.Images.SetKeyName(27, "_m6.gif")
		Me.imglstCarac.Images.SetKeyName(28, "_m7.gif")
		Me.imglstCarac.Images.SetKeyName(29, "_m8.gif")
		Me.imglstCarac.Images.SetKeyName(30, "_m9.gif")
		Me.imglstCarac.Images.SetKeyName(31, "_m10.gif")
		Me.imglstCarac.Images.SetKeyName(32, "_m11.gif")
		Me.imglstCarac.Images.SetKeyName(33, "_m12.gif")
		Me.imglstCarac.Images.SetKeyName(34, "_m13.gif")
		Me.imglstCarac.Images.SetKeyName(35, "_m14.gif")
		Me.imglstCarac.Images.SetKeyName(36, "_m15.gif")
		Me.imglstCarac.Images.SetKeyName(37, "_m16.gif")
		'
		'cmnuTvw
		'
		Me.cmnuTvw.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuCardsFR, Me.mnuSort, Me.mnuDegroupFoils, Me.mnuSearchCard, Me.mnucAddCards, Me.mnuSeparator1, Me.mnuMoveACard, Me.mnuCopyACard, Me.mnuSwapSerie, Me.mnuDeleteACard, Me.mnuSeparator2, Me.mnuBuy})
		Me.cmnuTvw.Name = "cmnuTvw"
		Me.cmnuTvw.Size = New System.Drawing.Size(234, 258)
		'
		'mnuCardsFR
		'
		Me.mnuCardsFR.Enabled = false
		Me.mnuCardsFR.Image = CType(resources.GetObject("mnuCardsFR.Image"),System.Drawing.Image)
		Me.mnuCardsFR.Name = "mnuCardsFR"
		Me.mnuCardsFR.Size = New System.Drawing.Size(233, 22)
		Me.mnuCardsFR.Text = "Titre des cartes en français"
		AddHandler Me.mnuCardsFR.MouseUp, AddressOf Me.MnuCardsFRActivate
		'
		'mnuSort
		'
		Me.mnuSort.Image = CType(resources.GetObject("mnuSort.Image"),System.Drawing.Image)
		Me.mnuSort.Name = "mnuSort"
		Me.mnuSort.Size = New System.Drawing.Size(233, 22)
		Me.mnuSort.Text = "Trier par ordre alphabétique"
		AddHandler Me.mnuSort.Click, AddressOf Me.MnuSortClick
		'
		'mnuDegroupFoils
		'
		Me.mnuDegroupFoils.Checked = true
		Me.mnuDegroupFoils.CheckState = System.Windows.Forms.CheckState.Checked
		Me.mnuDegroupFoils.Image = CType(resources.GetObject("mnuDegroupFoils.Image"),System.Drawing.Image)
		Me.mnuDegroupFoils.Name = "mnuDegroupFoils"
		Me.mnuDegroupFoils.Size = New System.Drawing.Size(233, 22)
		Me.mnuDegroupFoils.Text = "Dissocier les cartes foils"
		AddHandler Me.mnuDegroupFoils.Click, AddressOf Me.MnuDegroupFoilsClick
		'
		'mnuSearchCard
		'
		Me.mnuSearchCard.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSearchText, Me.mnuFindNext})
		Me.mnuSearchCard.Image = CType(resources.GetObject("mnuSearchCard.Image"),System.Drawing.Image)
		Me.mnuSearchCard.Name = "mnuSearchCard"
		Me.mnuSearchCard.Size = New System.Drawing.Size(233, 22)
		Me.mnuSearchCard.Text = "Rechercher"
		AddHandler Me.mnuSearchCard.Click, AddressOf Me.MnuSearchCardClick
		'
		'mnuSearchText
		'
		Me.mnuSearchText.Name = "mnuSearchText"
		Me.mnuSearchText.Size = New System.Drawing.Size(100, 23)
		Me.mnuSearchText.Text = "(carte)"
		AddHandler Me.mnuSearchText.KeyUp, AddressOf Me.MnuSearchTextKeyUp
		'
		'mnuFindNext
		'
		Me.mnuFindNext.Enabled = false
		Me.mnuFindNext.Name = "mnuFindNext"
		Me.mnuFindNext.ShortcutKeys = System.Windows.Forms.Keys.F3
		Me.mnuFindNext.Size = New System.Drawing.Size(211, 22)
		Me.mnuFindNext.Text = "Rechercher la suivante"
		AddHandler Me.mnuFindNext.Click, AddressOf Me.MnuFindNextClick
		'
		'mnucAddCards
		'
		Me.mnucAddCards.Image = CType(resources.GetObject("mnucAddCards.Image"),System.Drawing.Image)
		Me.mnucAddCards.Name = "mnucAddCards"
		Me.mnucAddCards.Size = New System.Drawing.Size(233, 22)
		Me.mnucAddCards.Text = "Ajouter / supprimer des cartes"
		AddHandler Me.mnucAddCards.Click, AddressOf Me.MnuAddCardsActivate
		'
		'mnuSeparator1
		'
		Me.mnuSeparator1.Name = "mnuSeparator1"
		Me.mnuSeparator1.Size = New System.Drawing.Size(230, 6)
		'
		'mnuMoveACard
		'
		Me.mnuMoveACard.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuMoveToCollection})
		Me.mnuMoveACard.Enabled = false
		Me.mnuMoveACard.Image = CType(resources.GetObject("mnuMoveACard.Image"),System.Drawing.Image)
		Me.mnuMoveACard.Name = "mnuMoveACard"
		Me.mnuMoveACard.Size = New System.Drawing.Size(233, 22)
		Me.mnuMoveACard.Text = "Déplacer vers..."
		'
		'mnuMoveToCollection
		'
		Me.mnuMoveToCollection.Name = "mnuMoveToCollection"
		Me.mnuMoveToCollection.Size = New System.Drawing.Size(152, 22)
		Me.mnuMoveToCollection.Text = "Collection"
		AddHandler Me.mnuMoveToCollection.Click, AddressOf Me.MnuMoveACardActivate
		'
		'mnuCopyACard
		'
		Me.mnuCopyACard.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuCopyToCollection})
		Me.mnuCopyACard.Enabled = false
		Me.mnuCopyACard.Image = CType(resources.GetObject("mnuCopyACard.Image"),System.Drawing.Image)
		Me.mnuCopyACard.Name = "mnuCopyACard"
		Me.mnuCopyACard.Size = New System.Drawing.Size(233, 22)
		Me.mnuCopyACard.Text = "Copier vers..."
		'
		'mnuCopyToCollection
		'
		Me.mnuCopyToCollection.Name = "mnuCopyToCollection"
		Me.mnuCopyToCollection.Size = New System.Drawing.Size(152, 22)
		Me.mnuCopyToCollection.Text = "Collection"
		AddHandler Me.mnuCopyToCollection.Click, AddressOf Me.MnuCopyACardActivate
		'
		'mnuDeleteACard
		'
		Me.mnuDeleteACard.Enabled = false
		Me.mnuDeleteACard.Image = CType(resources.GetObject("mnuDeleteACard.Image"),System.Drawing.Image)
		Me.mnuDeleteACard.Name = "mnuDeleteACard"
		Me.mnuDeleteACard.Size = New System.Drawing.Size(233, 22)
		Me.mnuDeleteACard.Text = "Supprimer"
		AddHandler Me.mnuDeleteACard.Click, AddressOf Me.MnuDeleteACardClick
		'
		'mnuSeparator2
		'
		Me.mnuSeparator2.Name = "mnuSeparator2"
		Me.mnuSeparator2.Size = New System.Drawing.Size(230, 6)
		'
		'mnuBuy
		'
		Me.mnuBuy.Enabled = false
		Me.mnuBuy.Image = CType(resources.GetObject("mnuBuy.Image"),System.Drawing.Image)
		Me.mnuBuy.Name = "mnuBuy"
		Me.mnuBuy.Size = New System.Drawing.Size(233, 22)
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
		'mnu
		'
		Me.mnu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile, Me.mnuDisp, Me.mnuTools, Me.mnuBigSearch, Me.mnuPlugins, Me.mnuInfo})
		Me.mnu.Location = New System.Drawing.Point(0, 0)
		Me.mnu.Name = "mnu"
		Me.mnu.Size = New System.Drawing.Size(757, 24)
		Me.mnu.TabIndex = 4
		Me.mnu.Text = "menuStrip1"
		'
		'mnuFile
		'
		Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuDBSelect, Me.mnuExport, Me.mnuSeparator3, Me.mnuNewEdition, Me.mnuRemEdition, Me.mnuTranslate, Me.mnuSeparator4, Me.mnuUpdatePrices, Me.mnuUpdatePictures, Me.mnuUpdateAutorisations, Me.mnuUpdateSimu, Me.mnuUpdateTxtFR, Me.mnuSeparator5, Me.mnuExit})
		Me.mnuFile.Name = "mnuFile"
		Me.mnuFile.Size = New System.Drawing.Size(54, 20)
		Me.mnuFile.Text = "Fichier"
		'
		'mnuDBSelect
		'
		Me.mnuDBSelect.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuDBOpen, Me.mnuDBSave})
		Me.mnuDBSelect.Image = CType(resources.GetObject("mnuDBSelect.Image"),System.Drawing.Image)
		Me.mnuDBSelect.Name = "mnuDBSelect"
		Me.mnuDBSelect.Size = New System.Drawing.Size(279, 22)
		Me.mnuDBSelect.Text = "Base de données"
		'
		'mnuDBOpen
		'
		Me.mnuDBOpen.Image = CType(resources.GetObject("mnuDBOpen.Image"),System.Drawing.Image)
		Me.mnuDBOpen.Name = "mnuDBOpen"
		Me.mnuDBOpen.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O),System.Windows.Forms.Keys)
		Me.mnuDBOpen.Size = New System.Drawing.Size(206, 22)
		Me.mnuDBOpen.Text = "Ouvrir..."
		AddHandler Me.mnuDBOpen.Click, AddressOf Me.MnuDBOpenClick
		'
		'mnuDBSave
		'
		Me.mnuDBSave.Image = CType(resources.GetObject("mnuDBSave.Image"),System.Drawing.Image)
		Me.mnuDBSave.Name = "mnuDBSave"
		Me.mnuDBSave.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S),System.Windows.Forms.Keys)
		Me.mnuDBSave.Size = New System.Drawing.Size(206, 22)
		Me.mnuDBSave.Text = "Enregistrer sous..."
		AddHandler Me.mnuDBSave.Click, AddressOf Me.MnuDBSaveClick
		'
		'mnuExport
		'
		Me.mnuExport.Image = CType(resources.GetObject("mnuExport.Image"),System.Drawing.Image)
		Me.mnuExport.Name = "mnuExport"
		Me.mnuExport.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.R),System.Windows.Forms.Keys)
		Me.mnuExport.Size = New System.Drawing.Size(279, 22)
		Me.mnuExport.Text = "Import / Export"
		AddHandler Me.mnuExport.Click, AddressOf Me.MnuExportActivate
		'
		'mnuSeparator3
		'
		Me.mnuSeparator3.Name = "mnuSeparator3"
		Me.mnuSeparator3.Size = New System.Drawing.Size(276, 6)
		'
		'mnuNewEdition
		'
		Me.mnuNewEdition.Image = CType(resources.GetObject("mnuNewEdition.Image"),System.Drawing.Image)
		Me.mnuNewEdition.Name = "mnuNewEdition"
		Me.mnuNewEdition.Size = New System.Drawing.Size(279, 22)
		Me.mnuNewEdition.Text = "Ajouter une série..."
		AddHandler Me.mnuNewEdition.Click, AddressOf Me.MnuNewEditionActivate
		'
		'mnuRemEdition
		'
		Me.mnuRemEdition.Image = CType(resources.GetObject("mnuRemEdition.Image"),System.Drawing.Image)
		Me.mnuRemEdition.Name = "mnuRemEdition"
		Me.mnuRemEdition.Size = New System.Drawing.Size(279, 22)
		Me.mnuRemEdition.Text = "Supprimer une série"
		AddHandler Me.mnuRemEdition.Click, AddressOf Me.MnuRemEditionActivate
		'
		'mnuTranslate
		'
		Me.mnuTranslate.Image = CType(resources.GetObject("mnuTranslate.Image"),System.Drawing.Image)
		Me.mnuTranslate.Name = "mnuTranslate"
		Me.mnuTranslate.Size = New System.Drawing.Size(279, 22)
		Me.mnuTranslate.Text = "Traduire une série"
		AddHandler Me.mnuTranslate.Click, AddressOf Me.MnuTranslateActivate
		'
		'mnuSeparator4
		'
		Me.mnuSeparator4.Name = "mnuSeparator4"
		Me.mnuSeparator4.Size = New System.Drawing.Size(276, 6)
		'
		'mnuUpdatePrices
		'
		Me.mnuUpdatePrices.Image = CType(resources.GetObject("mnuUpdatePrices.Image"),System.Drawing.Image)
		Me.mnuUpdatePrices.Name = "mnuUpdatePrices"
		Me.mnuUpdatePrices.Size = New System.Drawing.Size(279, 22)
		Me.mnuUpdatePrices.Text = "Mettre à jour les prix"
		AddHandler Me.mnuUpdatePrices.Click, AddressOf Me.MnuUpdatePricesActivate
		'
		'mnuUpdatePictures
		'
		Me.mnuUpdatePictures.Image = CType(resources.GetObject("mnuUpdatePictures.Image"),System.Drawing.Image)
		Me.mnuUpdatePictures.Name = "mnuUpdatePictures"
		Me.mnuUpdatePictures.Size = New System.Drawing.Size(279, 22)
		Me.mnuUpdatePictures.Text = "Mettre à jour les images"
		AddHandler Me.mnuUpdatePictures.Click, AddressOf Me.MnuUpdatePicturesActivate
		'
		'mnuUpdateAutorisations
		'
		Me.mnuUpdateAutorisations.Image = CType(resources.GetObject("mnuUpdateAutorisations.Image"),System.Drawing.Image)
		Me.mnuUpdateAutorisations.Name = "mnuUpdateAutorisations"
		Me.mnuUpdateAutorisations.Size = New System.Drawing.Size(279, 22)
		Me.mnuUpdateAutorisations.Text = "Mettre à jour les autorisations tournois"
		AddHandler Me.mnuUpdateAutorisations.Click, AddressOf Me.MnuUpdateAutorisationsClick
		'
		'mnuUpdateSimu
		'
		Me.mnuUpdateSimu.Image = CType(resources.GetObject("mnuUpdateSimu.Image"),System.Drawing.Image)
		Me.mnuUpdateSimu.Name = "mnuUpdateSimu"
		Me.mnuUpdateSimu.Size = New System.Drawing.Size(279, 22)
		Me.mnuUpdateSimu.Text = "Mettre à jour les modèles / historiques"
		AddHandler Me.mnuUpdateSimu.Click, AddressOf Me.MnuUpdateSimuActivate
		'
		'mnuUpdateTxtFR
		'
		Me.mnuUpdateTxtFR.Image = CType(resources.GetObject("mnuUpdateTxtFR.Image"),System.Drawing.Image)
		Me.mnuUpdateTxtFR.Name = "mnuUpdateTxtFR"
		Me.mnuUpdateTxtFR.Size = New System.Drawing.Size(279, 22)
		Me.mnuUpdateTxtFR.Text = "Mettre à jour les textes des cartes en VF"
		AddHandler Me.mnuUpdateTxtFR.Click, AddressOf Me.MnuUpdateTxtFRClick
		'
		'mnuSeparator5
		'
		Me.mnuSeparator5.Name = "mnuSeparator5"
		Me.mnuSeparator5.Size = New System.Drawing.Size(276, 6)
		'
		'mnuExit
		'
		Me.mnuExit.Image = CType(resources.GetObject("mnuExit.Image"),System.Drawing.Image)
		Me.mnuExit.Name = "mnuExit"
		Me.mnuExit.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Q),System.Windows.Forms.Keys)
		Me.mnuExit.Size = New System.Drawing.Size(279, 22)
		Me.mnuExit.Text = "Quitter"
		AddHandler Me.mnuExit.Click, AddressOf Me.MnuExitActivate
		'
		'mnuDisp
		'
		Me.mnuDisp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuRefresh, Me.mnuShowImage, Me.mnuSeparator6, Me.mnuDispCollection})
		Me.mnuDisp.Name = "mnuDisp"
		Me.mnuDisp.Size = New System.Drawing.Size(70, 20)
		Me.mnuDisp.Text = "Affichage"
		'
		'mnuRefresh
		'
		Me.mnuRefresh.Image = CType(resources.GetObject("mnuRefresh.Image"),System.Drawing.Image)
		Me.mnuRefresh.Name = "mnuRefresh"
		Me.mnuRefresh.ShortcutKeys = System.Windows.Forms.Keys.F5
		Me.mnuRefresh.Size = New System.Drawing.Size(257, 22)
		Me.mnuRefresh.Text = "Rafraîchir"
		AddHandler Me.mnuRefresh.Click, AddressOf Me.MnuRefreshActivate
		'
		'mnuShowImage
		'
		Me.mnuShowImage.Image = CType(resources.GetObject("mnuShowImage.Image"),System.Drawing.Image)
		Me.mnuShowImage.Name = "mnuShowImage"
		Me.mnuShowImage.ShortcutKeys = System.Windows.Forms.Keys.F4
		Me.mnuShowImage.Size = New System.Drawing.Size(257, 22)
		Me.mnuShowImage.Text = "Ouvrir / fermer panneau image"
		AddHandler Me.mnuShowImage.Click, AddressOf Me.MnuShowImageActivate
		'
		'mnuSeparator6
		'
		Me.mnuSeparator6.Name = "mnuSeparator6"
		Me.mnuSeparator6.Size = New System.Drawing.Size(254, 6)
		'
		'mnuDispCollection
		'
		Me.mnuDispCollection.Checked = true
		Me.mnuDispCollection.CheckState = System.Windows.Forms.CheckState.Checked
		Me.mnuDispCollection.Name = "mnuDispCollection"
		Me.mnuDispCollection.Size = New System.Drawing.Size(257, 22)
		Me.mnuDispCollection.Text = "Collection"
		AddHandler Me.mnuDispCollection.Click, AddressOf Me.MnuDispCollectionActivate
		'
		'mnuTools
		'
		Me.mnuTools.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuGestDecks, Me.mnuGestAdv, Me.mnuAddCards, Me.mnuRemCards, Me.mnuFixTable, Me.mnuFixDivers, Me.mnuSeparator7, Me.mnuPrefs})
		Me.mnuTools.Name = "mnuTools"
		Me.mnuTools.Size = New System.Drawing.Size(59, 20)
		Me.mnuTools.Text = "Gestion"
		'
		'mnuGestDecks
		'
		Me.mnuGestDecks.Image = CType(resources.GetObject("mnuGestDecks.Image"),System.Drawing.Image)
		Me.mnuGestDecks.Name = "mnuGestDecks"
		Me.mnuGestDecks.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.L),System.Windows.Forms.Keys)
		Me.mnuGestDecks.Size = New System.Drawing.Size(277, 22)
		Me.mnuGestDecks.Text = "Liste des decks"
		AddHandler Me.mnuGestDecks.Click, AddressOf Me.MnuGestDecksActivate
		'
		'mnuGestAdv
		'
		Me.mnuGestAdv.Image = CType(resources.GetObject("mnuGestAdv.Image"),System.Drawing.Image)
		Me.mnuGestAdv.Name = "mnuGestAdv"
		Me.mnuGestAdv.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.V),System.Windows.Forms.Keys)
		Me.mnuGestAdv.Size = New System.Drawing.Size(277, 22)
		Me.mnuGestAdv.Text = "Liste des adversaires"
		AddHandler Me.mnuGestAdv.Click, AddressOf Me.MnuGestAdvClick
		'
		'mnuAddCards
		'
		Me.mnuAddCards.Image = CType(resources.GetObject("mnuAddCards.Image"),System.Drawing.Image)
		Me.mnuAddCards.Name = "mnuAddCards"
		Me.mnuAddCards.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.N),System.Windows.Forms.Keys)
		Me.mnuAddCards.Size = New System.Drawing.Size(277, 22)
		Me.mnuAddCards.Text = "Ajouter / Supprimer des cartes"
		AddHandler Me.mnuAddCards.Click, AddressOf Me.MnuAddCardsActivate
		'
		'mnuRemCards
		'
		Me.mnuRemCards.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuRemScores, Me.mnuRemCollec, Me.mnuRemGames})
		Me.mnuRemCards.Image = CType(resources.GetObject("mnuRemCards.Image"),System.Drawing.Image)
		Me.mnuRemCards.Name = "mnuRemCards"
		Me.mnuRemCards.Size = New System.Drawing.Size(277, 22)
		Me.mnuRemCards.Text = "Purger la table..."
		'
		'mnuRemScores
		'
		Me.mnuRemScores.Name = "mnuRemScores"
		Me.mnuRemScores.Size = New System.Drawing.Size(172, 22)
		Me.mnuRemScores.Text = "Victoires / Défaites"
		AddHandler Me.mnuRemScores.Click, AddressOf Me.MnuRemScoresActivate
		'
		'mnuRemCollec
		'
		Me.mnuRemCollec.Name = "mnuRemCollec"
		Me.mnuRemCollec.Size = New System.Drawing.Size(172, 22)
		Me.mnuRemCollec.Text = "Collection"
		AddHandler Me.mnuRemCollec.Click, AddressOf Me.MnuRemCollecActivate
		'
		'mnuRemGames
		'
		Me.mnuRemGames.Name = "mnuRemGames"
		Me.mnuRemGames.Size = New System.Drawing.Size(172, 22)
		Me.mnuRemGames.Text = "Jeu..."
		'
		'mnuFixTable
		'
		Me.mnuFixTable.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFixPrices, Me.mnuFixFR, Me.mnuFixCreatures, Me.mnuFixSerie, Me.mnuFixSerie2, Me.mnuSeparator10, Me.mnuFixCollec, Me.mnuFixGames})
		Me.mnuFixTable.Image = CType(resources.GetObject("mnuFixTable.Image"),System.Drawing.Image)
		Me.mnuFixTable.Name = "mnuFixTable"
		Me.mnuFixTable.Size = New System.Drawing.Size(277, 22)
		Me.mnuFixTable.Text = "Réparer la table..."
		'
		'mnuFixPrices
		'
		Me.mnuFixPrices.Name = "mnuFixPrices"
		Me.mnuFixPrices.Size = New System.Drawing.Size(206, 22)
		Me.mnuFixPrices.Text = "Liste des prix"
		AddHandler Me.mnuFixPrices.Click, AddressOf Me.MnuFixPricesActivate
		'
		'mnuFixFR
		'
		Me.mnuFixFR.Name = "mnuFixFR"
		Me.mnuFixFR.Size = New System.Drawing.Size(206, 22)
		Me.mnuFixFR.Text = "Traductions manquantes"
		AddHandler Me.mnuFixFR.Click, AddressOf Me.MnuFixFRActivate
		'
		'mnuFixCreatures
		'
		Me.mnuFixCreatures.Name = "mnuFixCreatures"
		Me.mnuFixCreatures.Size = New System.Drawing.Size(206, 22)
		Me.mnuFixCreatures.Text = "Créatures"
		AddHandler Me.mnuFixCreatures.Click, AddressOf Me.MnuFixCreaturesClick
		'
		'mnuFixSerie
		'
		Me.mnuFixSerie.Name = "mnuFixSerie"
		Me.mnuFixSerie.Size = New System.Drawing.Size(206, 22)
		Me.mnuFixSerie.Text = "Edition..."
		'
		'mnuFixSerie2
		'
		Me.mnuFixSerie2.Name = "mnuFixSerie2"
		Me.mnuFixSerie2.Size = New System.Drawing.Size(206, 22)
		Me.mnuFixSerie2.Text = "En-têtes orphelins"
		AddHandler Me.mnuFixSerie2.Click, AddressOf Me.MnuFixSerie2Click
		'
		'mnuSeparator10
		'
		Me.mnuSeparator10.Name = "mnuSeparator10"
		Me.mnuSeparator10.Size = New System.Drawing.Size(203, 6)
		'
		'mnuFixCollec
		'
		Me.mnuFixCollec.Name = "mnuFixCollec"
		Me.mnuFixCollec.Size = New System.Drawing.Size(206, 22)
		Me.mnuFixCollec.Text = "Collection"
		AddHandler Me.mnuFixCollec.Click, AddressOf Me.MnuFixCollecActivate
		'
		'mnuFixGames
		'
		Me.mnuFixGames.Name = "mnuFixGames"
		Me.mnuFixGames.Size = New System.Drawing.Size(206, 22)
		Me.mnuFixGames.Text = "Jeu..."
		'
		'mnuFixDivers
		'
		Me.mnuFixDivers.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFixAssoc, Me.mnuFixPic, Me.mnuFixFR2})
		Me.mnuFixDivers.Image = CType(resources.GetObject("mnuFixDivers.Image"),System.Drawing.Image)
		Me.mnuFixDivers.Name = "mnuFixDivers"
		Me.mnuFixDivers.Size = New System.Drawing.Size(277, 22)
		Me.mnuFixDivers.Text = "Corrections..."
		'
		'mnuFixAssoc
		'
		Me.mnuFixAssoc.Name = "mnuFixAssoc"
		Me.mnuFixAssoc.Size = New System.Drawing.Size(140, 22)
		Me.mnuFixAssoc.Text = "Associations"
		AddHandler Me.mnuFixAssoc.Click, AddressOf Me.MnuFixAssocClick
		'
		'mnuFixPic
		'
		Me.mnuFixPic.Name = "mnuFixPic"
		Me.mnuFixPic.Size = New System.Drawing.Size(140, 22)
		Me.mnuFixPic.Text = "Images"
		AddHandler Me.mnuFixPic.Click, AddressOf Me.MnuFixPicClick
		'
		'mnuFixFR2
		'
		Me.mnuFixFR2.Name = "mnuFixFR2"
		Me.mnuFixFR2.Size = New System.Drawing.Size(140, 22)
		Me.mnuFixFR2.Text = "Traductions"
		AddHandler Me.mnuFixFR2.Click, AddressOf Me.MnuFixFR2Click
		'
		'mnuSeparator7
		'
		Me.mnuSeparator7.Name = "mnuSeparator7"
		Me.mnuSeparator7.Size = New System.Drawing.Size(274, 6)
		'
		'mnuPrefs
		'
		Me.mnuPrefs.Image = CType(resources.GetObject("mnuPrefs.Image"),System.Drawing.Image)
		Me.mnuPrefs.Name = "mnuPrefs"
		Me.mnuPrefs.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.P),System.Windows.Forms.Keys)
		Me.mnuPrefs.Size = New System.Drawing.Size(277, 22)
		Me.mnuPrefs.Text = "Préférences"
		AddHandler Me.mnuPrefs.Click, AddressOf Me.MnuPrefsActivate
		'
		'mnuBigSearch
		'
		Me.mnuBigSearch.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuStdSearch, Me.mnuAdvancedSearch, Me.mnuSeparator8, Me.mnuExcelGen, Me.mnuWordGen, Me.mnuPerfs, Me.mnuSimu, Me.mnuStats, Me.mnuMV})
		Me.mnuBigSearch.Name = "mnuBigSearch"
		Me.mnuBigSearch.Size = New System.Drawing.Size(50, 20)
		Me.mnuBigSearch.Text = "Outils"
		'
		'mnuStdSearch
		'
		Me.mnuStdSearch.Image = CType(resources.GetObject("mnuStdSearch.Image"),System.Drawing.Image)
		Me.mnuStdSearch.Name = "mnuStdSearch"
		Me.mnuStdSearch.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F),System.Windows.Forms.Keys)
		Me.mnuStdSearch.Size = New System.Drawing.Size(303, 22)
		Me.mnuStdSearch.Text = "Rechercher dans l'explorateur"
		AddHandler Me.mnuStdSearch.Click, AddressOf Me.MnuStdSearchActivate
		'
		'mnuAdvancedSearch
		'
		Me.mnuAdvancedSearch.Image = CType(resources.GetObject("mnuAdvancedSearch.Image"),System.Drawing.Image)
		Me.mnuAdvancedSearch.Name = "mnuAdvancedSearch"
		Me.mnuAdvancedSearch.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift)  _
						Or System.Windows.Forms.Keys.F),System.Windows.Forms.Keys)
		Me.mnuAdvancedSearch.Size = New System.Drawing.Size(303, 22)
		Me.mnuAdvancedSearch.Text = "Recherche avancée"
		AddHandler Me.mnuAdvancedSearch.Click, AddressOf Me.MnuAdvancedSearchActivate
		'
		'mnuSeparator8
		'
		Me.mnuSeparator8.Name = "mnuSeparator8"
		Me.mnuSeparator8.Size = New System.Drawing.Size(300, 6)
		'
		'mnuExcelGen
		'
		Me.mnuExcelGen.Image = CType(resources.GetObject("mnuExcelGen.Image"),System.Drawing.Image)
		Me.mnuExcelGen.Name = "mnuExcelGen"
		Me.mnuExcelGen.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.E),System.Windows.Forms.Keys)
		Me.mnuExcelGen.Size = New System.Drawing.Size(303, 22)
		Me.mnuExcelGen.Text = "Génération d'une liste sous Excel"
		AddHandler Me.mnuExcelGen.Click, AddressOf Me.MnuExcelGenActivate
		'
		'mnuWordGen
		'
		Me.mnuWordGen.Image = CType(resources.GetObject("mnuWordGen.Image"),System.Drawing.Image)
		Me.mnuWordGen.Name = "mnuWordGen"
		Me.mnuWordGen.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.W),System.Windows.Forms.Keys)
		Me.mnuWordGen.Size = New System.Drawing.Size(303, 22)
		Me.mnuWordGen.Text = "Génération de vignettes sous Word"
		AddHandler Me.mnuWordGen.Click, AddressOf Me.MnuWordGenClick
		'
		'mnuPerfs
		'
		Me.mnuPerfs.Image = CType(resources.GetObject("mnuPerfs.Image"),System.Drawing.Image)
		Me.mnuPerfs.Name = "mnuPerfs"
		Me.mnuPerfs.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.J),System.Windows.Forms.Keys)
		Me.mnuPerfs.Size = New System.Drawing.Size(303, 22)
		Me.mnuPerfs.Text = "Comptage Victoires / Défaites"
		AddHandler Me.mnuPerfs.Click, AddressOf Me.MnuPerfsActivate
		'
		'mnuSimu
		'
		Me.mnuSimu.Image = CType(resources.GetObject("mnuSimu.Image"),System.Drawing.Image)
		Me.mnuSimu.Name = "mnuSimu"
		Me.mnuSimu.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.U),System.Windows.Forms.Keys)
		Me.mnuSimu.Size = New System.Drawing.Size(303, 22)
		Me.mnuSimu.Text = "Simulations sur la sélection"
		AddHandler Me.mnuSimu.Click, AddressOf Me.MnuSimuActivate
		'
		'mnuStats
		'
		Me.mnuStats.Image = CType(resources.GetObject("mnuStats.Image"),System.Drawing.Image)
		Me.mnuStats.Name = "mnuStats"
		Me.mnuStats.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.T),System.Windows.Forms.Keys)
		Me.mnuStats.Size = New System.Drawing.Size(303, 22)
		Me.mnuStats.Text = "Statistiques sur la sélection"
		AddHandler Me.mnuStats.Click, AddressOf Me.MnuStatsActivate
		'
		'mnuMV
		'
		Me.mnuMV.Image = CType(resources.GetObject("mnuMV.Image"),System.Drawing.Image)
		Me.mnuMV.Name = "mnuMV"
		Me.mnuMV.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.B),System.Windows.Forms.Keys)
		Me.mnuMV.Size = New System.Drawing.Size(303, 22)
		Me.mnuMV.Text = "Achats sur Magic-Ville"
		AddHandler Me.mnuMV.Click, AddressOf Me.MnuMVClick
		'
		'mnuPlugins
		'
		Me.mnuPlugins.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuPlugResourcer})
		Me.mnuPlugins.Name = "mnuPlugins"
		Me.mnuPlugins.Size = New System.Drawing.Size(63, 20)
		Me.mnuPlugins.Text = "Plug-ins"
		'
		'mnuPlugResourcer
		'
		Me.mnuPlugResourcer.Image = CType(resources.GetObject("mnuPlugResourcer.Image"),System.Drawing.Image)
		Me.mnuPlugResourcer.Name = "mnuPlugResourcer"
		Me.mnuPlugResourcer.Size = New System.Drawing.Size(190, 22)
		Me.mnuPlugResourcer.Text = "MTGM WebResourcer"
		AddHandler Me.mnuPlugResourcer.Click, AddressOf Me.MnuPlugResourcerClick
		'
		'mnuInfo
		'
		Me.mnuInfo.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuCheckForUpdates, Me.mnuCheckForBetas, Me.mnuRestorePrev, Me.mnuSeparator9, Me.mnuWebsite, Me.mnuHelp, Me.mnuAbout})
		Me.mnuInfo.Name = "mnuInfo"
		Me.mnuInfo.Size = New System.Drawing.Size(24, 20)
		Me.mnuInfo.Text = "?"
		'
		'mnuCheckForUpdates
		'
		Me.mnuCheckForUpdates.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuApplicationUpdate, Me.mnuContenuUpdate})
		Me.mnuCheckForUpdates.Image = CType(resources.GetObject("mnuCheckForUpdates.Image"),System.Drawing.Image)
		Me.mnuCheckForUpdates.Name = "mnuCheckForUpdates"
		Me.mnuCheckForUpdates.Size = New System.Drawing.Size(237, 22)
		Me.mnuCheckForUpdates.Text = "Vérifier les mises à jour..."
		'
		'mnuApplicationUpdate
		'
		Me.mnuApplicationUpdate.Name = "mnuApplicationUpdate"
		Me.mnuApplicationUpdate.Size = New System.Drawing.Size(135, 22)
		Me.mnuApplicationUpdate.Text = "Application"
		AddHandler Me.mnuApplicationUpdate.Click, AddressOf Me.MnuCheckForUpdatesActivate
		'
		'mnuContenuUpdate
		'
		Me.mnuContenuUpdate.Name = "mnuContenuUpdate"
		Me.mnuContenuUpdate.Size = New System.Drawing.Size(135, 22)
		Me.mnuContenuUpdate.Text = "Contenu"
		AddHandler Me.mnuContenuUpdate.Click, AddressOf Me.MnuContenuUpdateClick
		'
		'mnuCheckForBetas
		'
		Me.mnuCheckForBetas.Image = CType(resources.GetObject("mnuCheckForBetas.Image"),System.Drawing.Image)
		Me.mnuCheckForBetas.Name = "mnuCheckForBetas"
		Me.mnuCheckForBetas.Size = New System.Drawing.Size(237, 22)
		Me.mnuCheckForBetas.Text = "Vérifier les mises à jour bêta"
		AddHandler Me.mnuCheckForBetas.Click, AddressOf Me.MnuCheckForBetasActivate
		'
		'mnuRestorePrev
		'
		Me.mnuRestorePrev.Image = CType(resources.GetObject("mnuRestorePrev.Image"),System.Drawing.Image)
		Me.mnuRestorePrev.Name = "mnuRestorePrev"
		Me.mnuRestorePrev.Size = New System.Drawing.Size(237, 22)
		Me.mnuRestorePrev.Text = "Revenir à la version précédente"
		AddHandler Me.mnuRestorePrev.Click, AddressOf Me.MnuRestorePrevClick
		'
		'mnuSeparator9
		'
		Me.mnuSeparator9.Name = "mnuSeparator9"
		Me.mnuSeparator9.Size = New System.Drawing.Size(234, 6)
		'
		'mnuWebsite
		'
		Me.mnuWebsite.Image = CType(resources.GetObject("mnuWebsite.Image"),System.Drawing.Image)
		Me.mnuWebsite.Name = "mnuWebsite"
		Me.mnuWebsite.ShortcutKeys = System.Windows.Forms.Keys.F9
		Me.mnuWebsite.Size = New System.Drawing.Size(237, 22)
		Me.mnuWebsite.Text = "Site Web de MTGM"
		AddHandler Me.mnuWebsite.Click, AddressOf Me.MnuWebsiteClick
		'
		'mnuHelp
		'
		Me.mnuHelp.Image = CType(resources.GetObject("mnuHelp.Image"),System.Drawing.Image)
		Me.mnuHelp.Name = "mnuHelp"
		Me.mnuHelp.ShortcutKeys = System.Windows.Forms.Keys.F1
		Me.mnuHelp.Size = New System.Drawing.Size(237, 22)
		Me.mnuHelp.Text = "Aide"
		AddHandler Me.mnuHelp.Click, AddressOf Me.MnuHelpActivate
		'
		'mnuAbout
		'
		Me.mnuAbout.Image = CType(resources.GetObject("mnuAbout.Image"),System.Drawing.Image)
		Me.mnuAbout.Name = "mnuAbout"
		Me.mnuAbout.ShortcutKeys = System.Windows.Forms.Keys.F10
		Me.mnuAbout.Size = New System.Drawing.Size(237, 22)
		Me.mnuAbout.Text = "A propos"
		AddHandler Me.mnuAbout.Click, AddressOf Me.MnuAboutActivate
		'
		'toolStrip
		'
		Me.toolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btDBSelect, Me.btExport, Me.btSeparator1, Me.btGestDecks, Me.btAddCards, Me.btAdvancedSearch, Me.btSeparator2, Me.btExcelGen, Me.btWordGen, Me.btSimu, Me.btStats, Me.btCheckForUpdates, Me.btWebsite})
		Me.toolStrip.Location = New System.Drawing.Point(0, 24)
		Me.toolStrip.Name = "toolStrip"
		Me.toolStrip.Size = New System.Drawing.Size(757, 25)
		Me.toolStrip.TabIndex = 7
		Me.toolStrip.Text = "toolStrip1"
		'
		'btDBSelect
		'
		Me.btDBSelect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.btDBSelect.Image = CType(resources.GetObject("btDBSelect.Image"),System.Drawing.Image)
		Me.btDBSelect.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btDBSelect.Name = "btDBSelect"
		Me.btDBSelect.Size = New System.Drawing.Size(23, 22)
		Me.btDBSelect.Text = "Ouvrir une base de données"
		AddHandler Me.btDBSelect.Click, AddressOf Me.MnuDBOpenClick
		'
		'btExport
		'
		Me.btExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.btExport.Image = CType(resources.GetObject("btExport.Image"),System.Drawing.Image)
		Me.btExport.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btExport.Name = "btExport"
		Me.btExport.Size = New System.Drawing.Size(23, 22)
		Me.btExport.Text = "Import / Export"
		AddHandler Me.btExport.Click, AddressOf Me.MnuExportActivate
		'
		'btSeparator1
		'
		Me.btSeparator1.Name = "btSeparator1"
		Me.btSeparator1.Size = New System.Drawing.Size(6, 25)
		'
		'btGestDecks
		'
		Me.btGestDecks.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.btGestDecks.Image = CType(resources.GetObject("btGestDecks.Image"),System.Drawing.Image)
		Me.btGestDecks.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btGestDecks.Name = "btGestDecks"
		Me.btGestDecks.Size = New System.Drawing.Size(23, 22)
		Me.btGestDecks.Text = "Liste des decks"
		AddHandler Me.btGestDecks.Click, AddressOf Me.MnuGestDecksActivate
		'
		'btAddCards
		'
		Me.btAddCards.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.btAddCards.Image = CType(resources.GetObject("btAddCards.Image"),System.Drawing.Image)
		Me.btAddCards.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btAddCards.Name = "btAddCards"
		Me.btAddCards.Size = New System.Drawing.Size(23, 22)
		Me.btAddCards.Text = "Ajouter / Supprimer des cartes"
		AddHandler Me.btAddCards.Click, AddressOf Me.MnuAddCardsActivate
		'
		'btAdvancedSearch
		'
		Me.btAdvancedSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.btAdvancedSearch.Image = CType(resources.GetObject("btAdvancedSearch.Image"),System.Drawing.Image)
		Me.btAdvancedSearch.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btAdvancedSearch.Name = "btAdvancedSearch"
		Me.btAdvancedSearch.Size = New System.Drawing.Size(23, 22)
		Me.btAdvancedSearch.Text = "Recherche avancée"
		AddHandler Me.btAdvancedSearch.Click, AddressOf Me.MnuAdvancedSearchActivate
		'
		'btSeparator2
		'
		Me.btSeparator2.Name = "btSeparator2"
		Me.btSeparator2.Size = New System.Drawing.Size(6, 25)
		'
		'btExcelGen
		'
		Me.btExcelGen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.btExcelGen.Image = CType(resources.GetObject("btExcelGen.Image"),System.Drawing.Image)
		Me.btExcelGen.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btExcelGen.Name = "btExcelGen"
		Me.btExcelGen.Size = New System.Drawing.Size(23, 22)
		Me.btExcelGen.Text = "Génération d'une liste sous Excel"
		AddHandler Me.btExcelGen.Click, AddressOf Me.MnuExcelGenActivate
		'
		'btWordGen
		'
		Me.btWordGen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.btWordGen.Image = CType(resources.GetObject("btWordGen.Image"),System.Drawing.Image)
		Me.btWordGen.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btWordGen.Name = "btWordGen"
		Me.btWordGen.Size = New System.Drawing.Size(23, 22)
		Me.btWordGen.Text = "Génération de vignettes sous Word"
		AddHandler Me.btWordGen.Click, AddressOf Me.MnuWordGenClick
		'
		'btSimu
		'
		Me.btSimu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.btSimu.Image = CType(resources.GetObject("btSimu.Image"),System.Drawing.Image)
		Me.btSimu.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btSimu.Name = "btSimu"
		Me.btSimu.Size = New System.Drawing.Size(23, 22)
		Me.btSimu.Text = "Simulations sur la sélection"
		AddHandler Me.btSimu.Click, AddressOf Me.MnuSimuActivate
		'
		'btStats
		'
		Me.btStats.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.btStats.Image = CType(resources.GetObject("btStats.Image"),System.Drawing.Image)
		Me.btStats.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btStats.Name = "btStats"
		Me.btStats.Size = New System.Drawing.Size(23, 22)
		Me.btStats.Text = "Statistiques sur la sélection"
		AddHandler Me.btStats.Click, AddressOf Me.MnuStatsActivate
		'
		'btCheckForUpdates
		'
		Me.btCheckForUpdates.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
		Me.btCheckForUpdates.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.btCheckForUpdates.Image = CType(resources.GetObject("btCheckForUpdates.Image"),System.Drawing.Image)
		Me.btCheckForUpdates.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btCheckForUpdates.Name = "btCheckForUpdates"
		Me.btCheckForUpdates.Size = New System.Drawing.Size(23, 22)
		Me.btCheckForUpdates.Text = "Vérifier les mises à jour"
		AddHandler Me.btCheckForUpdates.Click, AddressOf Me.MnuCheckForUpdatesActivate
		'
		'btWebsite
		'
		Me.btWebsite.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
		Me.btWebsite.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.btWebsite.Image = CType(resources.GetObject("btWebsite.Image"),System.Drawing.Image)
		Me.btWebsite.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btWebsite.Name = "btWebsite"
		Me.btWebsite.Size = New System.Drawing.Size(23, 22)
		Me.btWebsite.Text = "Site Web de MTGM"
		AddHandler Me.btWebsite.Click, AddressOf Me.MnuWebsiteClick
		'
		'splitV
		'
		Me.splitV.Dock = System.Windows.Forms.DockStyle.Fill
		Me.splitV.Location = New System.Drawing.Point(0, 49)
		Me.splitV.Name = "splitV"
		'
		'splitV.Panel1
		'
		Me.splitV.Panel1.Controls.Add(Me.CBarTvw)
		'
		'splitV.Panel2
		'
		Me.splitV.Panel2.Controls.Add(Me.splitV2)
		Me.splitV.Size = New System.Drawing.Size(757, 326)
		Me.splitV.SplitterDistance = 294
		Me.splitV.TabIndex = 8
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
		Me.CBarTvw.Size = New System.Drawing.Size(294, 326)
		Me.CBarTvw.TabIndex = 0
		Me.CBarTvw.Text = "Explorateur"
		'
		'pnlTvw
		'
		Me.pnlTvw.Controls.Add(Me.splitH)
		Me.pnlTvw.Location = New System.Drawing.Point(2, 46)
		Me.pnlTvw.Name = "pnlTvw"
		Me.pnlTvw.Size = New System.Drawing.Size(290, 278)
		Me.pnlTvw.TabIndex = 0
		'
		'splitH
		'
		Me.splitH.Dock = System.Windows.Forms.DockStyle.Fill
		Me.splitH.Location = New System.Drawing.Point(0, 0)
		Me.splitH.Name = "splitH"
		Me.splitH.Orientation = System.Windows.Forms.Orientation.Horizontal
		'
		'splitH.Panel1
		'
		Me.splitH.Panel1.Controls.Add(Me.chkClassement)
		'
		'splitH.Panel2
		'
		Me.splitH.Panel2.Controls.Add(Me.tvwExplore)
		Me.splitH.Size = New System.Drawing.Size(290, 278)
		Me.splitH.SplitterDistance = 68
		Me.splitH.TabIndex = 3
		'
		'chkClassement
		'
		Me.chkClassement.CheckOnClick = true
		Me.chkClassement.Dock = System.Windows.Forms.DockStyle.Fill
		Me.chkClassement.FormattingEnabled = true
		Me.chkClassement.Items.AddRange(New Object() {"Decks", "Type", "Couleur", "Edition", "Coût d'invocation", "Rareté", "Prix", "Carte"})
		Me.chkClassement.Location = New System.Drawing.Point(0, 0)
		Me.chkClassement.Name = "chkClassement"
		Me.chkClassement.Size = New System.Drawing.Size(290, 64)
		Me.chkClassement.TabIndex = 3
		AddHandler Me.chkClassement.SelectedIndexChanged, AddressOf Me.ChkClassementSelectedIndexChanged
		AddHandler Me.chkClassement.ItemCheck, AddressOf Me.ChkClassementItemCheck
		'
		'tvwExplore
		'
		Me.tvwExplore.AllowDrop = true
		Me.tvwExplore.BackColor = System.Drawing.SystemColors.Window
		Me.tvwExplore.Dock = System.Windows.Forms.DockStyle.Fill
		Me.tvwExplore.ImageIndex = 0
		Me.tvwExplore.ImageList = Me.imglstTvw
		Me.tvwExplore.Location = New System.Drawing.Point(0, 0)
		Me.tvwExplore.Name = "tvwExplore"
		Me.tvwExplore.SelectedImageIndex = 0
		Me.tvwExplore.SelectedNodes = CType(resources.GetObject("tvwExplore.SelectedNodes"),System.Collections.ArrayList)
		Me.tvwExplore.Size = New System.Drawing.Size(290, 206)
		Me.tvwExplore.TabIndex = 4
		AddHandler Me.tvwExplore.MouseUp, AddressOf Me.TvwExploreMouseUp
		AddHandler Me.tvwExplore.DragDrop, AddressOf Me.TvwExploreDragDrop
		AddHandler Me.tvwExplore.AfterSelect, AddressOf Me.TvwExploreAfterSelect
		AddHandler Me.tvwExplore.DragEnter, AddressOf Me.TvwExploreDragEnter
		AddHandler Me.tvwExplore.KeyUp, AddressOf Me.TvwExploreKeyUp
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
		Me.splitV2.Size = New System.Drawing.Size(459, 326)
		Me.splitV2.SplitterDistance = 245
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
		Me.CBarProperties.Size = New System.Drawing.Size(245, 326)
		Me.CBarProperties.TabIndex = 0
		Me.CBarProperties.Text = "Propriétés"
		'
		'pnlProperties
		'
		Me.pnlProperties.Controls.Add(Me.grpCarac)
		Me.pnlProperties.Controls.Add(Me.grpSerie)
		Me.pnlProperties.Controls.Add(Me.grpSerie2)
		Me.pnlProperties.Location = New System.Drawing.Point(2, 27)
		Me.pnlProperties.Name = "pnlProperties"
		Me.pnlProperties.Size = New System.Drawing.Size(241, 297)
		Me.pnlProperties.TabIndex = 0
		'
		'grpCarac
		'
		Me.grpCarac.BackColor = System.Drawing.Color.Transparent
		Me.grpCarac.Controls.Add(Me.txtCardText)
		Me.grpCarac.Controls.Add(Me.grpAutorisations)
		Me.grpCarac.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grpCarac.Location = New System.Drawing.Point(0, 138)
		Me.grpCarac.Name = "grpCarac"
		Me.grpCarac.Size = New System.Drawing.Size(241, 159)
		Me.grpCarac.TabIndex = 16
		Me.grpCarac.TabStop = false
		'
		'txtCardText
		'
		Me.txtCardText.AcceptsReturn = true
		Me.txtCardText.AcceptsTab = true
		Me.txtCardText.Dock = System.Windows.Forms.DockStyle.Fill
		Me.txtCardText.Location = New System.Drawing.Point(3, 16)
		Me.txtCardText.Multiline = true
		Me.txtCardText.Name = "txtCardText"
		Me.txtCardText.ReadOnly = true
		Me.txtCardText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
		Me.txtCardText.Size = New System.Drawing.Size(235, 101)
		Me.txtCardText.TabIndex = 13
		'
		'grpAutorisations
		'
		Me.grpAutorisations.Controls.Add(Me.picAutT1)
		Me.grpAutorisations.Controls.Add(Me.picAutT15)
		Me.grpAutorisations.Controls.Add(Me.picAutT1x)
		Me.grpAutorisations.Controls.Add(Me.picAutT2)
		Me.grpAutorisations.Controls.Add(Me.picAutBloc)
		Me.grpAutorisations.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.grpAutorisations.Location = New System.Drawing.Point(3, 117)
		Me.grpAutorisations.Name = "grpAutorisations"
		Me.grpAutorisations.Size = New System.Drawing.Size(235, 39)
		Me.grpAutorisations.TabIndex = 12
		Me.grpAutorisations.TabStop = false
		'
		'picAutT1
		'
		Me.picAutT1.Location = New System.Drawing.Point(33, 10)
		Me.picAutT1.Name = "picAutT1"
		Me.picAutT1.Size = New System.Drawing.Size(35, 25)
		Me.picAutT1.TabIndex = 4
		Me.picAutT1.TabStop = false
		'
		'picAutT15
		'
		Me.picAutT15.Location = New System.Drawing.Point(74, 10)
		Me.picAutT15.Name = "picAutT15"
		Me.picAutT15.Size = New System.Drawing.Size(35, 25)
		Me.picAutT15.TabIndex = 3
		Me.picAutT15.TabStop = false
		'
		'picAutT1x
		'
		Me.picAutT1x.Location = New System.Drawing.Point(115, 10)
		Me.picAutT1x.Name = "picAutT1x"
		Me.picAutT1x.Size = New System.Drawing.Size(35, 25)
		Me.picAutT1x.TabIndex = 2
		Me.picAutT1x.TabStop = false
		'
		'picAutT2
		'
		Me.picAutT2.Location = New System.Drawing.Point(156, 10)
		Me.picAutT2.Name = "picAutT2"
		Me.picAutT2.Size = New System.Drawing.Size(35, 25)
		Me.picAutT2.TabIndex = 1
		Me.picAutT2.TabStop = false
		'
		'picAutBloc
		'
		Me.picAutBloc.Location = New System.Drawing.Point(197, 10)
		Me.picAutBloc.Name = "picAutBloc"
		Me.picAutBloc.Size = New System.Drawing.Size(35, 25)
		Me.picAutBloc.TabIndex = 0
		Me.picAutBloc.TabStop = false
		'
		'grpSerie
		'
		Me.grpSerie.BackColor = System.Drawing.Color.Transparent
		Me.grpSerie.Controls.Add(Me.cmdHistPrices)
		Me.grpSerie.Controls.Add(Me.scrollStock)
		Me.grpSerie.Controls.Add(Me.cboEdition)
		Me.grpSerie.Controls.Add(Me.picEdition)
		Me.grpSerie.Controls.Add(Me.lblAD)
		Me.grpSerie.Controls.Add(Me.lblPrix)
		Me.grpSerie.Controls.Add(Me.lblRarete)
		Me.grpSerie.Controls.Add(Me.lblProp6)
		Me.grpSerie.Controls.Add(Me.lblProp1)
		Me.grpSerie.Controls.Add(Me.lblProp2)
		Me.grpSerie.Controls.Add(Me.lblProp5)
		Me.grpSerie.Controls.Add(Me.lblProp4)
		Me.grpSerie.Controls.Add(Me.lblProp3)
		Me.grpSerie.Controls.Add(Me.lblStock)
		Me.grpSerie.Dock = System.Windows.Forms.DockStyle.Top
		Me.grpSerie.Location = New System.Drawing.Point(0, 0)
		Me.grpSerie.Name = "grpSerie"
		Me.grpSerie.Size = New System.Drawing.Size(241, 138)
		Me.grpSerie.TabIndex = 8
		Me.grpSerie.TabStop = false
		'
		'cmdHistPrices
		'
		Me.cmdHistPrices.BackgroundImage = CType(resources.GetObject("cmdHistPrices.BackgroundImage"),System.Drawing.Image)
		Me.cmdHistPrices.Enabled = false
		Me.cmdHistPrices.Location = New System.Drawing.Point(59, 49)
		Me.cmdHistPrices.Name = "cmdHistPrices"
		Me.cmdHistPrices.Size = New System.Drawing.Size(25, 25)
		Me.cmdHistPrices.TabIndex = 22
		Me.cmdHistPrices.UseVisualStyleBackColor = true
		AddHandler Me.cmdHistPrices.Click, AddressOf Me.CmdHistPricesClick
		'
		'scrollStock
		'
		Me.scrollStock.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.scrollStock.LargeChange = 1
		Me.scrollStock.Location = New System.Drawing.Point(180, 70)
		Me.scrollStock.Maximum = 0
		Me.scrollStock.Name = "scrollStock"
		Me.scrollStock.Size = New System.Drawing.Size(17, 25)
		Me.scrollStock.TabIndex = 20
		Me.scrollStock.Visible = false
		AddHandler Me.scrollStock.Scroll, AddressOf Me.ScrollStockScroll
		'
		'cboEdition
		'
		Me.cboEdition.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.cboEdition.FormattingEnabled = true
		Me.cboEdition.Location = New System.Drawing.Point(115, 12)
		Me.cboEdition.Name = "cboEdition"
		Me.cboEdition.RightToLeft = System.Windows.Forms.RightToLeft.Yes
		Me.cboEdition.Size = New System.Drawing.Size(104, 21)
		Me.cboEdition.TabIndex = 19
		AddHandler Me.cboEdition.SelectedValueChanged, AddressOf Me.CboEditionSelectedValueChanged
		'
		'picEdition
		'
		Me.picEdition.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.picEdition.Location = New System.Drawing.Point(91, 13)
		Me.picEdition.Name = "picEdition"
		Me.picEdition.Size = New System.Drawing.Size(18, 18)
		Me.picEdition.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me.picEdition.TabIndex = 18
		Me.picEdition.TabStop = false
		'
		'lblAD
		'
		Me.lblAD.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.lblAD.BackColor = System.Drawing.Color.Transparent
		Me.lblAD.Location = New System.Drawing.Point(174, 95)
		Me.lblAD.Name = "lblAD"
		Me.lblAD.Size = New System.Drawing.Size(45, 13)
		Me.lblAD.TabIndex = 15
		Me.lblAD.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblPrix
		'
		Me.lblPrix.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.lblPrix.BackColor = System.Drawing.Color.Transparent
		Me.lblPrix.Location = New System.Drawing.Point(174, 55)
		Me.lblPrix.Name = "lblPrix"
		Me.lblPrix.Size = New System.Drawing.Size(45, 13)
		Me.lblPrix.TabIndex = 13
		Me.lblPrix.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblRarete
		'
		Me.lblRarete.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
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
		'lblStock
		'
		Me.lblStock.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.lblStock.BackColor = System.Drawing.Color.Transparent
		Me.lblStock.Location = New System.Drawing.Point(174, 75)
		Me.lblStock.Name = "lblStock"
		Me.lblStock.Size = New System.Drawing.Size(45, 13)
		Me.lblStock.TabIndex = 14
		Me.lblStock.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'grpSerie2
		'
		Me.grpSerie2.BackColor = System.Drawing.Color.Transparent
		Me.grpSerie2.Controls.Add(Me.lblSerieDate)
		Me.grpSerie2.Controls.Add(Me.lblSerieCote)
		Me.grpSerie2.Controls.Add(Me.lblSerieMyTotDist)
		Me.grpSerie2.Controls.Add(Me.lblSerieMyTot)
		Me.grpSerie2.Controls.Add(Me.lblSerieTot)
		Me.grpSerie2.Controls.Add(Me.lblProp12)
		Me.grpSerie2.Controls.Add(Me.lblProp11)
		Me.grpSerie2.Controls.Add(Me.lblProp10)
		Me.grpSerie2.Controls.Add(Me.lblProp9)
		Me.grpSerie2.Controls.Add(Me.lblProp8)
		Me.grpSerie2.Location = New System.Drawing.Point(0, 0)
		Me.grpSerie2.Name = "grpSerie2"
		Me.grpSerie2.Size = New System.Drawing.Size(241, 138)
		Me.grpSerie2.TabIndex = 13
		Me.grpSerie2.TabStop = false
		Me.grpSerie2.Visible = false
		'
		'lblSerieDate
		'
		Me.lblSerieDate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.lblSerieDate.BackColor = System.Drawing.Color.Transparent
		Me.lblSerieDate.Location = New System.Drawing.Point(149, 15)
		Me.lblSerieDate.Name = "lblSerieDate"
		Me.lblSerieDate.Size = New System.Drawing.Size(70, 13)
		Me.lblSerieDate.TabIndex = 16
		Me.lblSerieDate.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblSerieCote
		'
		Me.lblSerieCote.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.lblSerieCote.BackColor = System.Drawing.Color.Transparent
		Me.lblSerieCote.Location = New System.Drawing.Point(149, 55)
		Me.lblSerieCote.Name = "lblSerieCote"
		Me.lblSerieCote.Size = New System.Drawing.Size(70, 13)
		Me.lblSerieCote.TabIndex = 15
		Me.lblSerieCote.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblSerieMyTotDist
		'
		Me.lblSerieMyTotDist.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.lblSerieMyTotDist.BackColor = System.Drawing.Color.Transparent
		Me.lblSerieMyTotDist.Location = New System.Drawing.Point(154, 95)
		Me.lblSerieMyTotDist.Name = "lblSerieMyTotDist"
		Me.lblSerieMyTotDist.Size = New System.Drawing.Size(65, 13)
		Me.lblSerieMyTotDist.TabIndex = 14
		Me.lblSerieMyTotDist.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblSerieMyTot
		'
		Me.lblSerieMyTot.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.lblSerieMyTot.BackColor = System.Drawing.Color.Transparent
		Me.lblSerieMyTot.Location = New System.Drawing.Point(174, 75)
		Me.lblSerieMyTot.Name = "lblSerieMyTot"
		Me.lblSerieMyTot.Size = New System.Drawing.Size(45, 13)
		Me.lblSerieMyTot.TabIndex = 13
		Me.lblSerieMyTot.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblSerieTot
		'
		Me.lblSerieTot.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.lblSerieTot.BackColor = System.Drawing.Color.Transparent
		Me.lblSerieTot.Location = New System.Drawing.Point(174, 35)
		Me.lblSerieTot.Name = "lblSerieTot"
		Me.lblSerieTot.Size = New System.Drawing.Size(45, 13)
		Me.lblSerieTot.TabIndex = 12
		Me.lblSerieTot.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblProp12
		'
		Me.lblProp12.AutoSize = true
		Me.lblProp12.BackColor = System.Drawing.Color.Transparent
		Me.lblProp12.Location = New System.Drawing.Point(5, 55)
		Me.lblProp12.Name = "lblProp12"
		Me.lblProp12.Size = New System.Drawing.Size(64, 13)
		Me.lblProp12.TabIndex = 10
		Me.lblProp12.Text = "Cote totale :"
		'
		'lblProp11
		'
		Me.lblProp11.AutoSize = true
		Me.lblProp11.BackColor = System.Drawing.Color.Transparent
		Me.lblProp11.Location = New System.Drawing.Point(5, 95)
		Me.lblProp11.Name = "lblProp11"
		Me.lblProp11.Size = New System.Drawing.Size(144, 13)
		Me.lblProp11.TabIndex = 8
		Me.lblProp11.Text = "Total possédées (distinctes) :"
		'
		'lblProp10
		'
		Me.lblProp10.AutoSize = true
		Me.lblProp10.BackColor = System.Drawing.Color.Transparent
		Me.lblProp10.Location = New System.Drawing.Point(5, 75)
		Me.lblProp10.Name = "lblProp10"
		Me.lblProp10.Size = New System.Drawing.Size(91, 13)
		Me.lblProp10.TabIndex = 7
		Me.lblProp10.Text = "Total possédées :"
		'
		'lblProp9
		'
		Me.lblProp9.AutoSize = true
		Me.lblProp9.BackColor = System.Drawing.Color.Transparent
		Me.lblProp9.Location = New System.Drawing.Point(5, 35)
		Me.lblProp9.Name = "lblProp9"
		Me.lblProp9.Size = New System.Drawing.Size(69, 13)
		Me.lblProp9.TabIndex = 6
		Me.lblProp9.Text = "Total cartes :"
		'
		'lblProp8
		'
		Me.lblProp8.AutoSize = true
		Me.lblProp8.BackColor = System.Drawing.Color.Transparent
		Me.lblProp8.Location = New System.Drawing.Point(5, 15)
		Me.lblProp8.Name = "lblProp8"
		Me.lblProp8.Size = New System.Drawing.Size(79, 13)
		Me.lblProp8.TabIndex = 5
		Me.lblProp8.Text = "Date de sortie :"
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
		Me.CBarImage.Size = New System.Drawing.Size(210, 326)
		Me.CBarImage.TabIndex = 1
		Me.CBarImage.Text = "Image"
		'
		'pnlImage
		'
		Me.pnlImage.Controls.Add(Me.picScanCard)
		Me.pnlImage.Location = New System.Drawing.Point(2, 27)
		Me.pnlImage.Name = "pnlImage"
		Me.pnlImage.Size = New System.Drawing.Size(206, 297)
		Me.pnlImage.TabIndex = 0
		'
		'picScanCard
		'
		Me.picScanCard.Dock = System.Windows.Forms.DockStyle.Fill
		Me.picScanCard.Location = New System.Drawing.Point(0, 0)
		Me.picScanCard.Name = "picScanCard"
		Me.picScanCard.Size = New System.Drawing.Size(206, 297)
		Me.picScanCard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me.picScanCard.TabIndex = 0
		Me.picScanCard.TabStop = false
		'
		'dlgSave
		'
		Me.dlgSave.DefaultExt = "mdb"
		Me.dlgSave.Filter = "Fichiers de base de données Microsoft Access (*.mdb)|*.mdb"
		Me.dlgSave.Title = "Emplacement de la copie"
		'
		'imglstAutorisations
		'
		Me.imglstAutorisations.ImageStream = CType(resources.GetObject("imglstAutorisations.ImageStream"),System.Windows.Forms.ImageListStreamer)
		Me.imglstAutorisations.TransparentColor = System.Drawing.Color.Transparent
		Me.imglstAutorisations.Images.SetKeyName(0, "_aBloc.gif")
		Me.imglstAutorisations.Images.SetKeyName(1, "_aBlocno.gif")
		Me.imglstAutorisations.Images.SetKeyName(2, "_aBlocoff.gif")
		Me.imglstAutorisations.Images.SetKeyName(3, "_aT1.gif")
		Me.imglstAutorisations.Images.SetKeyName(4, "_aT1no.gif")
		Me.imglstAutorisations.Images.SetKeyName(5, "_aT1off.gif")
		Me.imglstAutorisations.Images.SetKeyName(6, "_aT1r.gif")
		Me.imglstAutorisations.Images.SetKeyName(7, "_aT1X.gif")
		Me.imglstAutorisations.Images.SetKeyName(8, "_aT1Xno.gif")
		Me.imglstAutorisations.Images.SetKeyName(9, "_aT1Xoff.gif")
		Me.imglstAutorisations.Images.SetKeyName(10, "_aT2.gif")
		Me.imglstAutorisations.Images.SetKeyName(11, "_aT2no.gif")
		Me.imglstAutorisations.Images.SetKeyName(12, "_aT2off.gif")
		Me.imglstAutorisations.Images.SetKeyName(13, "_aT15.gif")
		Me.imglstAutorisations.Images.SetKeyName(14, "_aT15no.gif")
		Me.imglstAutorisations.Images.SetKeyName(15, "_aT15off.gif")
		'
		'mnuSwapSerie
		'
		Me.mnuSwapSerie.Enabled = false
		Me.mnuSwapSerie.Image = CType(resources.GetObject("mnuSwapSerie.Image"),System.Drawing.Image)
		Me.mnuSwapSerie.Name = "mnuSwapSerie"
		Me.mnuSwapSerie.Size = New System.Drawing.Size(233, 22)
		Me.mnuSwapSerie.Text = "Modifier l'édition..."
		AddHandler Me.mnuSwapSerie.Click, AddressOf Me.MnuSwapSerieClick
		'
		'MainForm
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(757, 397)
		Me.Controls.Add(Me.splitV)
		Me.Controls.Add(Me.toolStrip)
		Me.Controls.Add(Me.statusStrip)
		Me.Controls.Add(Me.mnu)
		Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
		Me.MainMenuStrip = Me.mnu
		Me.Name = "MainForm"
		Me.Text = "Magic The Gathering Manager"
		AddHandler Load, AddressOf Me.MainFormLoad
		AddHandler FormClosing, AddressOf Me.MainFormFormClosing
		Me.statusStrip.ResumeLayout(false)
		Me.statusStrip.PerformLayout
		Me.cmnuTvw.ResumeLayout(false)
		Me.mnu.ResumeLayout(false)
		Me.mnu.PerformLayout
		Me.toolStrip.ResumeLayout(false)
		Me.toolStrip.PerformLayout
		Me.splitV.Panel1.ResumeLayout(false)
		Me.splitV.Panel2.ResumeLayout(false)
		Me.splitV.ResumeLayout(false)
		Me.CBarTvw.ResumeLayout(false)
		Me.pnlTvw.ResumeLayout(false)
		Me.splitH.Panel1.ResumeLayout(false)
		Me.splitH.Panel2.ResumeLayout(false)
		Me.splitH.ResumeLayout(false)
		Me.splitV2.Panel1.ResumeLayout(false)
		Me.splitV2.Panel2.ResumeLayout(false)
		Me.splitV2.ResumeLayout(false)
		Me.CBarProperties.ResumeLayout(false)
		Me.pnlProperties.ResumeLayout(false)
		Me.grpCarac.ResumeLayout(false)
		Me.grpCarac.PerformLayout
		Me.grpAutorisations.ResumeLayout(false)
		CType(Me.picAutT1,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.picAutT15,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.picAutT1x,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.picAutT2,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.picAutBloc,System.ComponentModel.ISupportInitialize).EndInit
		Me.grpSerie.ResumeLayout(false)
		Me.grpSerie.PerformLayout
		CType(Me.picEdition,System.ComponentModel.ISupportInitialize).EndInit
		Me.grpSerie2.ResumeLayout(false)
		Me.grpSerie2.PerformLayout
		Me.CBarImage.ResumeLayout(false)
		Me.pnlImage.ResumeLayout(false)
		CType(Me.picScanCard,System.ComponentModel.ISupportInitialize).EndInit
		Me.ResumeLayout(false)
		Me.PerformLayout
	End Sub
	Private mnuSwapSerie As System.Windows.Forms.ToolStripMenuItem
	Private btWordGen As System.Windows.Forms.ToolStripButton
	Private mnuWordGen As System.Windows.Forms.ToolStripMenuItem
	Private mnuCopyToCollection As System.Windows.Forms.ToolStripMenuItem
	Private mnuCopyACard As System.Windows.Forms.ToolStripMenuItem
	Private mnuPlugResourcer As System.Windows.Forms.ToolStripMenuItem
	Private mnuPlugins As System.Windows.Forms.ToolStripMenuItem
	Private mnuDegroupFoils As System.Windows.Forms.ToolStripMenuItem
	Private mnuGestAdv As System.Windows.Forms.ToolStripMenuItem
	Private btWebsite As System.Windows.Forms.ToolStripButton
	Private mnuWebsite As System.Windows.Forms.ToolStripMenuItem
	Private mnuMV As System.Windows.Forms.ToolStripMenuItem
	Private cmdHistPrices As System.Windows.Forms.Button
	Private mnuContenuUpdate As System.Windows.Forms.ToolStripMenuItem
	Private mnuApplicationUpdate As System.Windows.Forms.ToolStripMenuItem
	Private scrollStock As System.Windows.Forms.VScrollBar
	Private mnuUpdateAutorisations As System.Windows.Forms.ToolStripMenuItem
	Private imglstAutorisations As System.Windows.Forms.ImageList
	Private picAutBloc As System.Windows.Forms.PictureBox
	Private picAutT2 As System.Windows.Forms.PictureBox
	Private picAutT1x As System.Windows.Forms.PictureBox
	Private picAutT15 As System.Windows.Forms.PictureBox
	Private picAutT1 As System.Windows.Forms.PictureBox
	Private grpAutorisations As System.Windows.Forms.GroupBox
	Private mnuFixSerie2 As System.Windows.Forms.ToolStripMenuItem
	Private mnuFixFR2 As System.Windows.Forms.ToolStripMenuItem
	Private mnuFixPic As System.Windows.Forms.ToolStripMenuItem
	Private mnuFixDivers As System.Windows.Forms.ToolStripMenuItem
	Public lblSerieCote As System.Windows.Forms.Label
	Public lblSerieMyTotDist As System.Windows.Forms.Label
	Public lblSerieMyTot As System.Windows.Forms.Label
	Public lblSerieTot As System.Windows.Forms.Label
	Public lblProp12 As System.Windows.Forms.Label
	Private lblProp11 As System.Windows.Forms.Label
	Public lblProp10 As System.Windows.Forms.Label
	Private lblProp9 As System.Windows.Forms.Label
	Private lblProp8 As System.Windows.Forms.Label
	Public lblSerieDate As System.Windows.Forms.Label
	Public grpSerie2 As System.Windows.Forms.GroupBox
	Private splitH As System.Windows.Forms.SplitContainer
	Private mnuRestorePrev As System.Windows.Forms.ToolStripMenuItem
	Private mnuSeparator10 As System.Windows.Forms.ToolStripSeparator
	Private mnuFixAssoc As System.Windows.Forms.ToolStripMenuItem
	Private mnuInfosDL As System.Windows.Forms.ToolStripMenuItem
	Private mnuCancel As System.Windows.Forms.ToolStripMenuItem
	Public btDownload As System.Windows.Forms.ToolStripSplitButton
	Private dlgSave As System.Windows.Forms.SaveFileDialog
	Private mnuDBSave As System.Windows.Forms.ToolStripMenuItem
	Private mnuDBOpen As System.Windows.Forms.ToolStripMenuItem
	Private btExcelGen As System.Windows.Forms.ToolStripButton
	Private mnuUpdateTxtFR As System.Windows.Forms.ToolStripMenuItem
	Private mnuFixCreatures As System.Windows.Forms.ToolStripMenuItem
	Private toolStrip As System.Windows.Forms.ToolStrip
	Private btCheckForUpdates As System.Windows.Forms.ToolStripButton
	Private btStats As System.Windows.Forms.ToolStripButton
	Private btSimu As System.Windows.Forms.ToolStripButton
	Private btDBSelect As System.Windows.Forms.ToolStripButton
	Private btExport As System.Windows.Forms.ToolStripButton
	Private btGestDecks As System.Windows.Forms.ToolStripButton
	Private btSeparator2 As System.Windows.Forms.ToolStripSeparator
	Private btAdvancedSearch As System.Windows.Forms.ToolStripButton
	Private btAddCards As System.Windows.Forms.ToolStripButton
	Private btSeparator1 As System.Windows.Forms.ToolStripSeparator
	Private mnu As System.Windows.Forms.MenuStrip
	Private mnuSeparator1 As System.Windows.Forms.ToolStripSeparator
	Private mnuSeparator3 As System.Windows.Forms.ToolStripSeparator
	Private mnuSeparator4 As System.Windows.Forms.ToolStripSeparator
	Private mnuSeparator5 As System.Windows.Forms.ToolStripSeparator
	Private mnuSeparator6 As System.Windows.Forms.ToolStripSeparator
	Private mnuSeparator7 As System.Windows.Forms.ToolStripSeparator
	Private mnuSeparator8 As System.Windows.Forms.ToolStripSeparator
	Private mnuSeparator9 As System.Windows.Forms.ToolStripSeparator
	Private mnuUpdateSimu As System.Windows.Forms.ToolStripMenuItem
	Private mnuBuy As System.Windows.Forms.ToolStripMenuItem
	Private mnuSeparator2 As System.Windows.Forms.ToolStripSeparator
	Private imglstTvw As System.Windows.Forms.ImageList
	Private dlgOpen4 As System.Windows.Forms.OpenFileDialog
	Private mnuFixSerie As System.Windows.Forms.ToolStripMenuItem
	Private mnuCheckForBetas As System.Windows.Forms.ToolStripMenuItem
	Private mnuShowImage As System.Windows.Forms.ToolStripMenuItem
	Private pnlImage As TD.SandBar.ContainerBarClientPanel
	Private CBarImage As TD.SandBar.ContainerBar
	Private mnuMoveToCollection As System.Windows.Forms.ToolStripMenuItem
	Private picScanCard As System.Windows.Forms.PictureBox
	Private splitV2 As System.Windows.Forms.SplitContainer
	Private dlgOpen3 As System.Windows.Forms.OpenFileDialog
	Private mnuUpdatePictures As System.Windows.Forms.ToolStripMenuItem
	Private mnuGestDecks As System.Windows.Forms.ToolStripMenuItem
	Private mnuExport As System.Windows.Forms.ToolStripMenuItem
	Private mnuSimu As System.Windows.Forms.ToolStripMenuItem
	Private mnuRemEdition As System.Windows.Forms.ToolStripMenuItem
	Private mnuExcelGen As System.Windows.Forms.ToolStripMenuItem
	Private mnuDeleteACard As System.Windows.Forms.ToolStripMenuItem
	Private mnuMoveACard As System.Windows.Forms.ToolStripMenuItem
	Private mnuRemScores As System.Windows.Forms.ToolStripMenuItem
	Public mnuPerfs As System.Windows.Forms.ToolStripMenuItem
	Private mnucAddCards As System.Windows.Forms.ToolStripMenuItem
	Private mnuCheckForUpdates As System.Windows.Forms.ToolStripMenuItem
	Public prgAvance As System.Windows.Forms.ToolStripProgressBar
	Private mnuFixFR As System.Windows.Forms.ToolStripMenuItem
	Private mnuDBSelect As System.Windows.Forms.ToolStripMenuItem
	Private mnuTranslate As System.Windows.Forms.ToolStripMenuItem
	Private mnuFixPrices As System.Windows.Forms.ToolStripMenuItem
	Private mnuNewEdition As System.Windows.Forms.ToolStripMenuItem
	Private mnuFixGames As System.Windows.Forms.ToolStripMenuItem
	Private mnuFixCollec As System.Windows.Forms.ToolStripMenuItem
	Private mnuFixTable As System.Windows.Forms.ToolStripMenuItem
	Private mnuSort As System.Windows.Forms.ToolStripMenuItem
	Private mnuHelp As System.Windows.Forms.ToolStripMenuItem
	Private mnuStats As System.Windows.Forms.ToolStripMenuItem
	Private dlgOpen2 As System.Windows.Forms.OpenFileDialog
	Private mnuAdvancedSearch As System.Windows.Forms.ToolStripMenuItem
	Private mnuStdSearch As System.Windows.Forms.ToolStripMenuItem
	Private mnuBigSearch As System.Windows.Forms.ToolStripMenuItem
	Private mnuUpdatePrices As System.Windows.Forms.ToolStripMenuItem
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
	Public mnuCardsFR As System.Windows.Forms.ToolStripMenuItem
	Private cmnuTvw As System.Windows.Forms.ContextMenuStrip
	Private btDown As TD.SandBar.ButtonItem
	Private btUp As TD.SandBar.ButtonItem
	Public chkClassement As System.Windows.Forms.CheckedListBox
	Private mnuRefresh As System.Windows.Forms.ToolStripMenuItem
	Private mnuDispCollection As System.Windows.Forms.ToolStripMenuItem
	Public mnuDisp As System.Windows.Forms.ToolStripMenuItem
	Public tvwExplore As TreeViewMS.TreeViewMS
	Private pnlProperties As TD.SandBar.ContainerBarClientPanel
	Private CBarProperties As TD.SandBar.ContainerBar
	Private pnlTvw As TD.SandBar.ContainerBarClientPanel
	Private CBarTvw As TD.SandBar.ContainerBar
	Private splitV As System.Windows.Forms.SplitContainer
	Private mnuRemGames As System.Windows.Forms.ToolStripMenuItem
	Private mnuRemCollec As System.Windows.Forms.ToolStripMenuItem
	Private mnuRemCards As System.Windows.Forms.ToolStripMenuItem
	Private mnuPrefs As System.Windows.Forms.ToolStripMenuItem
	Private dlgOpen As System.Windows.Forms.OpenFileDialog
	Private mnuTools As System.Windows.Forms.ToolStripMenuItem
	Private lblDB As System.Windows.Forms.ToolStripStatusLabel
	Private statusStrip As System.Windows.Forms.StatusStrip
	Private mnuAddCards As System.Windows.Forms.ToolStripMenuItem
	Private mnuAbout As System.Windows.Forms.ToolStripMenuItem
	Private mnuInfo As System.Windows.Forms.ToolStripMenuItem
	Private mnuExit As System.Windows.Forms.ToolStripMenuItem
	Private mnuFile As System.Windows.Forms.ToolStripMenuItem
End Class
