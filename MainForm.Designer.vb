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
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
		Me.statusStrip = New System.Windows.Forms.StatusStrip()
		Me.lblDB = New System.Windows.Forms.ToolStripStatusLabel()
		Me.lblNCards = New System.Windows.Forms.ToolStripStatusLabel()
		Me.prgAvance = New System.Windows.Forms.ToolStripProgressBar()
		Me.btDownload = New System.Windows.Forms.ToolStripSplitButton()
		Me.mnuInfosDL = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuCancel = New System.Windows.Forms.ToolStripMenuItem()
		Me.dlgOpen = New System.Windows.Forms.OpenFileDialog()
		Me.imglstTvw = New System.Windows.Forms.ImageList(Me.components)
		Me.imglstCarac = New System.Windows.Forms.ImageList(Me.components)
		Me.cmnuTvw = New System.Windows.Forms.ContextMenuStrip(Me.components)
		Me.mnuCardsFR = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuSort = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuSearchCard = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuSearchText = New System.Windows.Forms.ToolStripTextBox()
		Me.mnuFindNext = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnucAddCards = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuSeparator1 = New System.Windows.Forms.ToolStripSeparator()
		Me.mnuTransform = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuMoveACard = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuMoveToCollection = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuCopyACard = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuCopyToCollection = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuSwapSerie = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuDeleteACard = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuSeparator2 = New System.Windows.Forms.ToolStripSeparator()
		Me.mnuBuy = New System.Windows.Forms.ToolStripMenuItem()
		Me.dlgOpen2 = New System.Windows.Forms.OpenFileDialog()
		Me.dlgOpen3 = New System.Windows.Forms.OpenFileDialog()
		Me.dlgOpen4 = New System.Windows.Forms.OpenFileDialog()
		Me.mnu = New System.Windows.Forms.MenuStrip()
		Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuDBSelect = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuDBOpen = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuDBSave = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuExport = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuSeparator3 = New System.Windows.Forms.ToolStripSeparator()
		Me.mnuNewEdition = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuRemEdition = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuTranslate = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuSeparator4 = New System.Windows.Forms.ToolStripSeparator()
		Me.mnuUpdatePrices = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuUpdatePictures = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuUpdateAutorisations = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuUpdateRulings = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuUpdateSimu = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuUpdateTxtFR = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuSeparator5 = New System.Windows.Forms.ToolStripSeparator()
		Me.mnuExit = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuDisp = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuRefresh = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuShowImage = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuSeparator6 = New System.Windows.Forms.ToolStripSeparator()
		Me.mnuDispAdvSearch = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuDispCollection = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuTools = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuGestDecks = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuGestAdv = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuAddCards = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuRemCards = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuRemScores = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuRemCollec = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuRemGames = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuFixTable = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuFixPrices = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuFixFR = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuFixCreatures = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuFixSerie = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuFixSerie2 = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuSeparator10 = New System.Windows.Forms.ToolStripSeparator()
		Me.mnuFixCollec = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuFixGames = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuFixDivers = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuFixAssoc = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuFixPic = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuFixFR2 = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuCollapseRarete = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuSeparator7 = New System.Windows.Forms.ToolStripSeparator()
		Me.mnuPrefs = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuBigSearch = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuStdSearch = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuAdvancedSearch = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuSeparator8 = New System.Windows.Forms.ToolStripSeparator()
		Me.mnuExcelGen = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuWordGen = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuPerfs = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuPlateau = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuSimu = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuStats = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuMV = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuPlugins = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuPlugResourcer = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuInfo = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuCheckForUpdates = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuApplicationUpdate = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuContenuUpdate = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuCheckForBetas = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuRestorePrev = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuSeparator9 = New System.Windows.Forms.ToolStripSeparator()
		Me.mnuWebsite = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuAbout = New System.Windows.Forms.ToolStripMenuItem()
		Me.toolStrip = New System.Windows.Forms.ToolStrip()
		Me.btExport = New System.Windows.Forms.ToolStripButton()
		Me.btSeparator1 = New System.Windows.Forms.ToolStripSeparator()
		Me.btGestDecks = New System.Windows.Forms.ToolStripButton()
		Me.btAddCards = New System.Windows.Forms.ToolStripButton()
		Me.btAdvancedSearch = New System.Windows.Forms.ToolStripButton()
		Me.btSeparator2 = New System.Windows.Forms.ToolStripSeparator()
		Me.btExcelGen = New System.Windows.Forms.ToolStripButton()
		Me.btWordGen = New System.Windows.Forms.ToolStripButton()
		Me.btPlateau = New System.Windows.Forms.ToolStripButton()
		Me.btStats = New System.Windows.Forms.ToolStripButton()
		Me.btSimu = New System.Windows.Forms.ToolStripButton()
		Me.btCheckForUpdates = New System.Windows.Forms.ToolStripButton()
		Me.btWebsite = New System.Windows.Forms.ToolStripButton()
		Me.splitV = New System.Windows.Forms.SplitContainer()
		Me.cbarTvw = New TD.SandBar.ContainerBar()
		Me.pnlTvw = New TD.SandBar.ContainerBarClientPanel()
		Me.tvwExplore = New TreeViewMS.TreeViewMS()
		Me.toolSubStrip = New System.Windows.Forms.ToolStrip()
		Me.btCriteria = New System.Windows.Forms.ToolStripButton()
		Me.btExpand = New System.Windows.Forms.ToolStripButton()
		Me.btSeparator = New System.Windows.Forms.ToolStripSeparator()
		Me.btCardsFR = New System.Windows.Forms.ToolStripButton()
		Me.btSort = New System.Windows.Forms.ToolStripButton()
		Me.splitV2 = New System.Windows.Forms.SplitContainer()
		Me.cbarProperties = New TD.SandBar.ContainerBar()
		Me.pnlProperties = New TD.SandBar.ContainerBarClientPanel()
		Me.splitH = New System.Windows.Forms.SplitContainer()
		Me.pnlCard = New System.Windows.Forms.Panel()
		Me.splitH2 = New System.Windows.Forms.SplitContainer()
		Me.pnlCard1 = New System.Windows.Forms.Panel()
		Me.grdPropCard = New SourceGrid2.Grid()
		Me.splitH3 = New System.Windows.Forms.SplitContainer()
		Me.splitV3 = New System.Windows.Forms.SplitContainer()
		Me.pnlCard2 = New System.Windows.Forms.Panel()
		Me.picCost = New System.Windows.Forms.PictureBox()
		Me.pnlCard3 = New System.Windows.Forms.Panel()
		Me.lblPowerTough = New System.Windows.Forms.Label()
		Me.picPowerTough = New System.Windows.Forms.PictureBox()
		Me.txtRichCard = New Magic_The_Gathering_Manager.ExRichTextBox()
		Me.pnlAlternate = New System.Windows.Forms.Panel()
		Me.propAlternate = New System.Windows.Forms.PropertyGrid()
		Me.txtRichOther = New Magic_The_Gathering_Manager.ExRichTextBox()
		Me.grpAutorisations = New System.Windows.Forms.GroupBox()
		Me.picAutT1 = New System.Windows.Forms.PictureBox()
		Me.picAutT15 = New System.Windows.Forms.PictureBox()
		Me.picAutM = New System.Windows.Forms.PictureBox()
		Me.picAutT1x = New System.Windows.Forms.PictureBox()
		Me.picAutT2 = New System.Windows.Forms.PictureBox()
		Me.picAutBloc = New System.Windows.Forms.PictureBox()
		Me.btShowAll = New TD.SandBar.ButtonItem()
		Me.btCardUse = New TD.SandBar.ButtonItem()
		Me.btHistPrices = New TD.SandBar.ButtonItem()
		Me.cbarImage = New TD.SandBar.ContainerBar()
		Me.pnlImage = New TD.SandBar.ContainerBarClientPanel()
		Me.splitH4 = New System.Windows.Forms.SplitContainer()
		Me.picScanCard = New System.Windows.Forms.PictureBox()
		Me.grdPropPicture = New SourceGrid2.Grid()
		Me.dlgSave = New System.Windows.Forms.SaveFileDialog()
		Me.imglstAutorisations = New System.Windows.Forms.ImageList(Me.components)
		Me.cmnuCbar = New System.Windows.Forms.ContextMenuStrip(Me.components)
		Me.btHistPricesSimple = New System.Windows.Forms.ToolStripMenuItem()
		Me.btHistPricesFoil = New System.Windows.Forms.ToolStripMenuItem()
		Me.mnuPlugHTML = New System.Windows.Forms.ToolStripMenuItem()
		Me.statusStrip.SuspendLayout
		Me.cmnuTvw.SuspendLayout
		Me.mnu.SuspendLayout
		Me.toolStrip.SuspendLayout
		Me.splitV.Panel1.SuspendLayout
		Me.splitV.Panel2.SuspendLayout
		Me.splitV.SuspendLayout
		Me.cbarTvw.SuspendLayout
		Me.pnlTvw.SuspendLayout
		Me.toolSubStrip.SuspendLayout
		Me.splitV2.Panel1.SuspendLayout
		Me.splitV2.Panel2.SuspendLayout
		Me.splitV2.SuspendLayout
		Me.cbarProperties.SuspendLayout
		Me.pnlProperties.SuspendLayout
		Me.splitH.Panel1.SuspendLayout
		Me.splitH.Panel2.SuspendLayout
		Me.splitH.SuspendLayout
		Me.pnlCard.SuspendLayout
		Me.splitH2.Panel1.SuspendLayout
		Me.splitH2.Panel2.SuspendLayout
		Me.splitH2.SuspendLayout
		Me.pnlCard1.SuspendLayout
		Me.splitH3.Panel1.SuspendLayout
		Me.splitH3.Panel2.SuspendLayout
		Me.splitH3.SuspendLayout
		Me.splitV3.Panel1.SuspendLayout
		Me.splitV3.Panel2.SuspendLayout
		Me.splitV3.SuspendLayout
		Me.pnlCard2.SuspendLayout
		CType(Me.picCost,System.ComponentModel.ISupportInitialize).BeginInit
		Me.pnlCard3.SuspendLayout
		CType(Me.picPowerTough,System.ComponentModel.ISupportInitialize).BeginInit
		Me.pnlAlternate.SuspendLayout
		Me.grpAutorisations.SuspendLayout
		CType(Me.picAutT1,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.picAutT15,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.picAutM,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.picAutT1x,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.picAutT2,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.picAutBloc,System.ComponentModel.ISupportInitialize).BeginInit
		Me.cbarImage.SuspendLayout
		Me.pnlImage.SuspendLayout
		Me.splitH4.Panel1.SuspendLayout
		Me.splitH4.Panel2.SuspendLayout
		Me.splitH4.SuspendLayout
		CType(Me.picScanCard,System.ComponentModel.ISupportInitialize).BeginInit
		Me.cmnuCbar.SuspendLayout
		Me.SuspendLayout
		'
		'statusStrip
		'
		Me.statusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblDB, Me.lblNCards, Me.prgAvance, Me.btDownload})
		Me.statusStrip.Location = New System.Drawing.Point(0, 544)
		Me.statusStrip.Name = "statusStrip"
		Me.statusStrip.Size = New System.Drawing.Size(992, 22)
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
		Me.imglstTvw.Images.SetKeyName(1, "MTGM_bleu_ld.ico")
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
		Me.imglstTvw.Images.SetKeyName(12, "_mx.png")
		Me.imglstTvw.Images.SetKeyName(13, "_m0.png")
		Me.imglstTvw.Images.SetKeyName(14, "_m1.png")
		Me.imglstTvw.Images.SetKeyName(15, "_m2.png")
		Me.imglstTvw.Images.SetKeyName(16, "_m3.png")
		Me.imglstTvw.Images.SetKeyName(17, "_m4.png")
		Me.imglstTvw.Images.SetKeyName(18, "_m5.png")
		Me.imglstTvw.Images.SetKeyName(19, "_m6.png")
		Me.imglstTvw.Images.SetKeyName(20, "_m7.png")
		Me.imglstTvw.Images.SetKeyName(21, "_m8.png")
		Me.imglstTvw.Images.SetKeyName(22, "_m9.png")
		Me.imglstTvw.Images.SetKeyName(23, "_m10.png")
		Me.imglstTvw.Images.SetKeyName(24, "_m11.png")
		Me.imglstTvw.Images.SetKeyName(25, "_m12.png")
		Me.imglstTvw.Images.SetKeyName(26, "_m13.png")
		Me.imglstTvw.Images.SetKeyName(27, "_m14.png")
		Me.imglstTvw.Images.SetKeyName(28, "_m15.png")
		Me.imglstTvw.Images.SetKeyName(29, "_m16.png")
		Me.imglstTvw.Images.SetKeyName(30, "_rdirt.ico")
		Me.imglstTvw.Images.SetKeyName(31, "_rco.ico")
		Me.imglstTvw.Images.SetKeyName(32, "_runco.ico")
		Me.imglstTvw.Images.SetKeyName(33, "_rrare.ico")
		Me.imglstTvw.Images.SetKeyName(34, "_rmystic.ico")
		Me.imglstTvw.Images.SetKeyName(35, "_p0.png")
		Me.imglstTvw.Images.SetKeyName(36, "_p1.png")
		Me.imglstTvw.Images.SetKeyName(37, "_p2.png")
		Me.imglstTvw.Images.SetKeyName(38, "_p3.png")
		Me.imglstTvw.Images.SetKeyName(39, "_p4.png")
		Me.imglstTvw.Images.SetKeyName(40, "_p5.png")
		Me.imglstTvw.Images.SetKeyName(41, "_p6.png")
		Me.imglstTvw.Images.SetKeyName(42, "_p7.png")
		Me.imglstTvw.Images.SetKeyName(43, "_startifact.png")
		Me.imglstTvw.Images.SetKeyName(44, "_staura.png")
		Me.imglstTvw.Images.SetKeyName(45, "_stcreature.png")
		Me.imglstTvw.Images.SetKeyName(46, "_stinstant.png")
		Me.imglstTvw.Images.SetKeyName(47, "_stland.png")
		Me.imglstTvw.Images.SetKeyName(48, "_stsorcery.png")
		Me.imglstTvw.Images.SetKeyName(49, "_stphenomenon.png")
		Me.imglstTvw.Images.SetKeyName(50, "_stplane.png")
		'
		'imglstCarac
		'
		Me.imglstCarac.ImageStream = CType(resources.GetObject("imglstCarac.ImageStream"),System.Windows.Forms.ImageListStreamer)
		Me.imglstCarac.TransparentColor = System.Drawing.Color.Transparent
		Me.imglstCarac.Images.SetKeyName(0, "_mx.png")
		Me.imglstCarac.Images.SetKeyName(1, "_m0.png")
		Me.imglstCarac.Images.SetKeyName(2, "_m1.png")
		Me.imglstCarac.Images.SetKeyName(3, "_m2.png")
		Me.imglstCarac.Images.SetKeyName(4, "_m3.png")
		Me.imglstCarac.Images.SetKeyName(5, "_m4.png")
		Me.imglstCarac.Images.SetKeyName(6, "_m5.png")
		Me.imglstCarac.Images.SetKeyName(7, "_m6.png")
		Me.imglstCarac.Images.SetKeyName(8, "_m7.png")
		Me.imglstCarac.Images.SetKeyName(9, "_m8.png")
		Me.imglstCarac.Images.SetKeyName(10, "_m9.png")
		Me.imglstCarac.Images.SetKeyName(11, "_m10.png")
		Me.imglstCarac.Images.SetKeyName(12, "_m11.png")
		Me.imglstCarac.Images.SetKeyName(13, "_m12.png")
		Me.imglstCarac.Images.SetKeyName(14, "_m13.png")
		Me.imglstCarac.Images.SetKeyName(15, "_m14.png")
		Me.imglstCarac.Images.SetKeyName(16, "_m15.png")
		Me.imglstCarac.Images.SetKeyName(17, "_m16.png")
		Me.imglstCarac.Images.SetKeyName(18, "_l2b.png")
		Me.imglstCarac.Images.SetKeyName(19, "_l2g.png")
		Me.imglstCarac.Images.SetKeyName(20, "_l2r.png")
		Me.imglstCarac.Images.SetKeyName(21, "_l2u.png")
		Me.imglstCarac.Images.SetKeyName(22, "_l2w.png")
		Me.imglstCarac.Images.SetKeyName(23, "_lb.png")
		Me.imglstCarac.Images.SetKeyName(24, "_lbg.png")
		Me.imglstCarac.Images.SetKeyName(25, "_lbr.png")
		Me.imglstCarac.Images.SetKeyName(26, "_lbu.png")
		Me.imglstCarac.Images.SetKeyName(27, "_lbw.png")
		Me.imglstCarac.Images.SetKeyName(28, "_lg.png")
		Me.imglstCarac.Images.SetKeyName(29, "_lgb.png")
		Me.imglstCarac.Images.SetKeyName(30, "_lgr.png")
		Me.imglstCarac.Images.SetKeyName(31, "_lgu.png")
		Me.imglstCarac.Images.SetKeyName(32, "_lgw.png")
		Me.imglstCarac.Images.SetKeyName(33, "_lpb.png")
		Me.imglstCarac.Images.SetKeyName(34, "_lpg.png")
		Me.imglstCarac.Images.SetKeyName(35, "_lpr.png")
		Me.imglstCarac.Images.SetKeyName(36, "_lpu.png")
		Me.imglstCarac.Images.SetKeyName(37, "_lpw.png")
		Me.imglstCarac.Images.SetKeyName(38, "_lr.png")
		Me.imglstCarac.Images.SetKeyName(39, "_lrb.png")
		Me.imglstCarac.Images.SetKeyName(40, "_lrg.png")
		Me.imglstCarac.Images.SetKeyName(41, "_lru.png")
		Me.imglstCarac.Images.SetKeyName(42, "_lrw.png")
		Me.imglstCarac.Images.SetKeyName(43, "_lu.png")
		Me.imglstCarac.Images.SetKeyName(44, "_lub.png")
		Me.imglstCarac.Images.SetKeyName(45, "_lug.png")
		Me.imglstCarac.Images.SetKeyName(46, "_lur.png")
		Me.imglstCarac.Images.SetKeyName(47, "_luw.png")
		Me.imglstCarac.Images.SetKeyName(48, "_lw.png")
		Me.imglstCarac.Images.SetKeyName(49, "_lwb.png")
		Me.imglstCarac.Images.SetKeyName(50, "_lwg.png")
		Me.imglstCarac.Images.SetKeyName(51, "_lwr.png")
		Me.imglstCarac.Images.SetKeyName(52, "_lwu.png")
		Me.imglstCarac.Images.SetKeyName(53, "_lq.png")
		Me.imglstCarac.Images.SetKeyName(54, "_lt.png")
		Me.imglstCarac.Images.SetKeyName(55, "_ls.png")
		'
		'cmnuTvw
		'
		Me.cmnuTvw.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuCardsFR, Me.mnuSort, Me.mnuSearchCard, Me.mnucAddCards, Me.mnuSeparator1, Me.mnuTransform, Me.mnuMoveACard, Me.mnuCopyACard, Me.mnuSwapSerie, Me.mnuDeleteACard, Me.mnuSeparator2, Me.mnuBuy})
		Me.cmnuTvw.Name = "cmnuTvw"
		Me.cmnuTvw.Size = New System.Drawing.Size(234, 236)
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
		'mnuTransform
		'
		Me.mnuTransform.Enabled = false
		Me.mnuTransform.Image = CType(resources.GetObject("mnuTransform.Image"),System.Drawing.Image)
		Me.mnuTransform.Name = "mnuTransform"
		Me.mnuTransform.Size = New System.Drawing.Size(233, 22)
		Me.mnuTransform.Text = "Transformer"
		AddHandler Me.mnuTransform.Click, AddressOf Me.MnuTransformClick
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
		Me.mnuMoveToCollection.Size = New System.Drawing.Size(128, 22)
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
		Me.mnuCopyToCollection.Size = New System.Drawing.Size(128, 22)
		Me.mnuCopyToCollection.Text = "Collection"
		AddHandler Me.mnuCopyToCollection.Click, AddressOf Me.MnuCopyACardActivate
		'
		'mnuSwapSerie
		'
		Me.mnuSwapSerie.Enabled = false
		Me.mnuSwapSerie.Image = CType(resources.GetObject("mnuSwapSerie.Image"),System.Drawing.Image)
		Me.mnuSwapSerie.Name = "mnuSwapSerie"
		Me.mnuSwapSerie.Size = New System.Drawing.Size(233, 22)
		Me.mnuSwapSerie.Text = "Modifier édition, foil, réserve"
		AddHandler Me.mnuSwapSerie.Click, AddressOf Me.MnuSwapSerieClick
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
		Me.mnu.Size = New System.Drawing.Size(992, 24)
		Me.mnu.TabIndex = 4
		Me.mnu.Text = "menuStrip1"
		'
		'mnuFile
		'
		Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuDBSelect, Me.mnuExport, Me.mnuSeparator3, Me.mnuNewEdition, Me.mnuRemEdition, Me.mnuTranslate, Me.mnuSeparator4, Me.mnuUpdatePrices, Me.mnuUpdatePictures, Me.mnuUpdateAutorisations, Me.mnuUpdateRulings, Me.mnuUpdateSimu, Me.mnuUpdateTxtFR, Me.mnuSeparator5, Me.mnuExit})
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
		Me.mnuNewEdition.Text = "Ajouter des séries..."
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
		'mnuUpdateRulings
		'
		Me.mnuUpdateRulings.Image = CType(resources.GetObject("mnuUpdateRulings.Image"),System.Drawing.Image)
		Me.mnuUpdateRulings.Name = "mnuUpdateRulings"
		Me.mnuUpdateRulings.Size = New System.Drawing.Size(279, 22)
		Me.mnuUpdateRulings.Text = "Mettre à jour les règles spécifiques"
		AddHandler Me.mnuUpdateRulings.Click, AddressOf Me.MnuUpdateRulingsClick
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
		Me.mnuDisp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuRefresh, Me.mnuShowImage, Me.mnuSeparator6, Me.mnuDispAdvSearch, Me.mnuDispCollection})
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
		'mnuDispAdvSearch
		'
		Me.mnuDispAdvSearch.Enabled = false
		Me.mnuDispAdvSearch.Name = "mnuDispAdvSearch"
		Me.mnuDispAdvSearch.Size = New System.Drawing.Size(257, 22)
		Me.mnuDispAdvSearch.Text = "Résultats de recherche"
		AddHandler Me.mnuDispAdvSearch.Click, AddressOf Me.MnuDispCollectionActivate
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
		Me.mnuFixDivers.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFixAssoc, Me.mnuFixPic, Me.mnuFixFR2, Me.mnuCollapseRarete})
		Me.mnuFixDivers.Image = CType(resources.GetObject("mnuFixDivers.Image"),System.Drawing.Image)
		Me.mnuFixDivers.Name = "mnuFixDivers"
		Me.mnuFixDivers.Size = New System.Drawing.Size(277, 22)
		Me.mnuFixDivers.Text = "Corrections..."
		'
		'mnuFixAssoc
		'
		Me.mnuFixAssoc.Name = "mnuFixAssoc"
		Me.mnuFixAssoc.Size = New System.Drawing.Size(233, 22)
		Me.mnuFixAssoc.Text = "Associations"
		AddHandler Me.mnuFixAssoc.Click, AddressOf Me.MnuFixAssocClick
		'
		'mnuFixPic
		'
		Me.mnuFixPic.Name = "mnuFixPic"
		Me.mnuFixPic.Size = New System.Drawing.Size(233, 22)
		Me.mnuFixPic.Text = "Images"
		AddHandler Me.mnuFixPic.Click, AddressOf Me.MnuFixPicClick
		'
		'mnuFixFR2
		'
		Me.mnuFixFR2.Name = "mnuFixFR2"
		Me.mnuFixFR2.Size = New System.Drawing.Size(233, 22)
		Me.mnuFixFR2.Text = "Traductions"
		AddHandler Me.mnuFixFR2.Click, AddressOf Me.MnuFixFR2Click
		'
		'mnuCollapseRarete
		'
		Me.mnuCollapseRarete.Name = "mnuCollapseRarete"
		Me.mnuCollapseRarete.Size = New System.Drawing.Size(233, 22)
		Me.mnuCollapseRarete.Text = "Supprimer les degrés de rareté"
		AddHandler Me.mnuCollapseRarete.Click, AddressOf Me.MnuCollapseRareteClick
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
		Me.mnuPrefs.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift)  _
						Or System.Windows.Forms.Keys.P),System.Windows.Forms.Keys)
		Me.mnuPrefs.Size = New System.Drawing.Size(277, 22)
		Me.mnuPrefs.Text = "Préférences"
		AddHandler Me.mnuPrefs.Click, AddressOf Me.MnuPrefsActivate
		'
		'mnuBigSearch
		'
		Me.mnuBigSearch.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuStdSearch, Me.mnuAdvancedSearch, Me.mnuSeparator8, Me.mnuExcelGen, Me.mnuWordGen, Me.mnuPerfs, Me.mnuPlateau, Me.mnuSimu, Me.mnuStats, Me.mnuMV})
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
		'mnuPlateau
		'
		Me.mnuPlateau.Image = CType(resources.GetObject("mnuPlateau.Image"),System.Drawing.Image)
		Me.mnuPlateau.Name = "mnuPlateau"
		Me.mnuPlateau.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.P),System.Windows.Forms.Keys)
		Me.mnuPlateau.Size = New System.Drawing.Size(303, 22)
		Me.mnuPlateau.Text = "Plateau de jeu sur la sélection"
		AddHandler Me.mnuPlateau.Click, AddressOf Me.MnuPlateauClick
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
		Me.mnuPlugins.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuPlugResourcer, Me.mnuPlugHTML})
		Me.mnuPlugins.Name = "mnuPlugins"
		Me.mnuPlugins.Size = New System.Drawing.Size(63, 20)
		Me.mnuPlugins.Text = "Plug-ins"
		'
		'mnuPlugResourcer
		'
		Me.mnuPlugResourcer.Image = CType(resources.GetObject("mnuPlugResourcer.Image"),System.Drawing.Image)
		Me.mnuPlugResourcer.Name = "mnuPlugResourcer"
		Me.mnuPlugResourcer.Size = New System.Drawing.Size(199, 22)
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
		Me.toolStrip.ImageScalingSize = New System.Drawing.Size(32, 32)
		Me.toolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btExport, Me.btSeparator1, Me.btGestDecks, Me.btAddCards, Me.btAdvancedSearch, Me.btSeparator2, Me.btExcelGen, Me.btWordGen, Me.btPlateau, Me.btStats, Me.btSimu, Me.btCheckForUpdates, Me.btWebsite})
		Me.toolStrip.Location = New System.Drawing.Point(0, 24)
		Me.toolStrip.Name = "toolStrip"
		Me.toolStrip.Size = New System.Drawing.Size(992, 54)
		Me.toolStrip.TabIndex = 7
		'
		'btExport
		'
		Me.btExport.Image = CType(resources.GetObject("btExport.Image"),System.Drawing.Image)
		Me.btExport.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btExport.Name = "btExport"
		Me.btExport.Size = New System.Drawing.Size(57, 51)
		Me.btExport.Text = "Importer"
		Me.btExport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me.btExport.ToolTipText = "Importer des cartes depuis divers formats"
		AddHandler Me.btExport.Click, AddressOf Me.MnuExportActivate
		'
		'btSeparator1
		'
		Me.btSeparator1.Name = "btSeparator1"
		Me.btSeparator1.Size = New System.Drawing.Size(6, 54)
		'
		'btGestDecks
		'
		Me.btGestDecks.AutoSize = false
		Me.btGestDecks.Image = CType(resources.GetObject("btGestDecks.Image"),System.Drawing.Image)
		Me.btGestDecks.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btGestDecks.Name = "btGestDecks"
		Me.btGestDecks.Size = New System.Drawing.Size(60, 51)
		Me.btGestDecks.Text = "Decks"
		Me.btGestDecks.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me.btGestDecks.ToolTipText = "Liste de mes decks"
		AddHandler Me.btGestDecks.Click, AddressOf Me.MnuGestDecksActivate
		'
		'btAddCards
		'
		Me.btAddCards.AutoSize = false
		Me.btAddCards.Image = CType(resources.GetObject("btAddCards.Image"),System.Drawing.Image)
		Me.btAddCards.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btAddCards.Name = "btAddCards"
		Me.btAddCards.Size = New System.Drawing.Size(60, 51)
		Me.btAddCards.Text = "Saisie"
		Me.btAddCards.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me.btAddCards.ToolTipText = "Ajouter des cartes à la collection ou aux decks"
		AddHandler Me.btAddCards.Click, AddressOf Me.MnuAddCardsActivate
		'
		'btAdvancedSearch
		'
		Me.btAdvancedSearch.Image = CType(resources.GetObject("btAdvancedSearch.Image"),System.Drawing.Image)
		Me.btAdvancedSearch.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btAdvancedSearch.Name = "btAdvancedSearch"
		Me.btAdvancedSearch.Size = New System.Drawing.Size(66, 51)
		Me.btAdvancedSearch.Text = "Recherche"
		Me.btAdvancedSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me.btAdvancedSearch.ToolTipText = "Recherche avancée dans la base des cartes"
		AddHandler Me.btAdvancedSearch.Click, AddressOf Me.MnuAdvancedSearchActivate
		'
		'btSeparator2
		'
		Me.btSeparator2.Name = "btSeparator2"
		Me.btSeparator2.Size = New System.Drawing.Size(6, 54)
		'
		'btExcelGen
		'
		Me.btExcelGen.AutoSize = false
		Me.btExcelGen.Image = CType(resources.GetObject("btExcelGen.Image"),System.Drawing.Image)
		Me.btExcelGen.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btExcelGen.Name = "btExcelGen"
		Me.btExcelGen.Size = New System.Drawing.Size(60, 51)
		Me.btExcelGen.Text = "Listing"
		Me.btExcelGen.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me.btExcelGen.ToolTipText = "Générer un listing de cartes sous Excel"
		AddHandler Me.btExcelGen.Click, AddressOf Me.MnuExcelGenActivate
		'
		'btWordGen
		'
		Me.btWordGen.Image = CType(resources.GetObject("btWordGen.Image"),System.Drawing.Image)
		Me.btWordGen.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btWordGen.Name = "btWordGen"
		Me.btWordGen.Size = New System.Drawing.Size(60, 51)
		Me.btWordGen.Text = "Vignettes"
		Me.btWordGen.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me.btWordGen.ToolTipText = "Préparer des pages de cartes à imprimer sous Word"
		AddHandler Me.btWordGen.Click, AddressOf Me.MnuWordGenClick
		'
		'btPlateau
		'
		Me.btPlateau.AutoSize = false
		Me.btPlateau.Image = CType(resources.GetObject("btPlateau.Image"),System.Drawing.Image)
		Me.btPlateau.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btPlateau.Name = "btPlateau"
		Me.btPlateau.Size = New System.Drawing.Size(60, 51)
		Me.btPlateau.Text = "Plateau"
		Me.btPlateau.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me.btPlateau.ToolTipText = "Simuler une partie complète avec ma sélection"
		AddHandler Me.btPlateau.Click, AddressOf Me.MnuPlateauClick
		'
		'btStats
		'
		Me.btStats.Image = CType(resources.GetObject("btStats.Image"),System.Drawing.Image)
		Me.btStats.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btStats.Name = "btStats"
		Me.btStats.Size = New System.Drawing.Size(71, 51)
		Me.btStats.Text = "Statistiques"
		Me.btStats.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me.btStats.ToolTipText = "Calculer les statistiques sur ma sélection"
		AddHandler Me.btStats.Click, AddressOf Me.MnuStatsActivate
		'
		'btSimu
		'
		Me.btSimu.Image = CType(resources.GetObject("btSimu.Image"),System.Drawing.Image)
		Me.btSimu.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btSimu.Name = "btSimu"
		Me.btSimu.Size = New System.Drawing.Size(73, 51)
		Me.btSimu.Text = "Simulations"
		Me.btSimu.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me.btSimu.ToolTipText = "Obtenir des estimations ou des optimisations sur ma sélection"
		AddHandler Me.btSimu.Click, AddressOf Me.MnuSimuActivate
		'
		'btCheckForUpdates
		'
		Me.btCheckForUpdates.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
		Me.btCheckForUpdates.Image = CType(resources.GetObject("btCheckForUpdates.Image"),System.Drawing.Image)
		Me.btCheckForUpdates.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btCheckForUpdates.Name = "btCheckForUpdates"
		Me.btCheckForUpdates.Size = New System.Drawing.Size(74, 51)
		Me.btCheckForUpdates.Text = "Mises à jour"
		Me.btCheckForUpdates.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me.btCheckForUpdates.ToolTipText = "Mises à jour de l'application et de son contenu"
		AddHandler Me.btCheckForUpdates.Click, AddressOf Me.BtCheckForUpdatesClick
		'
		'btWebsite
		'
		Me.btWebsite.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
		Me.btWebsite.Image = CType(resources.GetObject("btWebsite.Image"),System.Drawing.Image)
		Me.btWebsite.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btWebsite.Name = "btWebsite"
		Me.btWebsite.Size = New System.Drawing.Size(67, 51)
		Me.btWebsite.Text = "Sur le Web"
		Me.btWebsite.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me.btWebsite.ToolTipText = "Site Internet de l'application"
		AddHandler Me.btWebsite.Click, AddressOf Me.MnuWebsiteClick
		'
		'splitV
		'
		Me.splitV.Dock = System.Windows.Forms.DockStyle.Fill
		Me.splitV.Location = New System.Drawing.Point(0, 78)
		Me.splitV.Name = "splitV"
		'
		'splitV.Panel1
		'
		Me.splitV.Panel1.Controls.Add(Me.cbarTvw)
		'
		'splitV.Panel2
		'
		Me.splitV.Panel2.Controls.Add(Me.splitV2)
		Me.splitV.Size = New System.Drawing.Size(992, 466)
		Me.splitV.SplitterDistance = 300
		Me.splitV.TabIndex = 8
		Me.splitV.TabStop = false
		'
		'cbarTvw
		'
		Me.cbarTvw.Closable = false
		Me.cbarTvw.Controls.Add(Me.pnlTvw)
		Me.cbarTvw.Dock = System.Windows.Forms.DockStyle.Fill
		Me.cbarTvw.DrawActionsButton = false
		Me.cbarTvw.Flow = TD.SandBar.ToolBarLayout.Horizontal
		Me.cbarTvw.Guid = New System.Guid("219cb30a-3b04-4474-8157-17accfec97d2")
		Me.cbarTvw.Location = New System.Drawing.Point(0, 0)
		Me.cbarTvw.Movable = false
		Me.cbarTvw.Name = "cbarTvw"
		Me.cbarTvw.Size = New System.Drawing.Size(300, 466)
		Me.cbarTvw.TabIndex = 0
		Me.cbarTvw.Text = "Explorateur"
		'
		'pnlTvw
		'
		Me.pnlTvw.Controls.Add(Me.tvwExplore)
		Me.pnlTvw.Controls.Add(Me.toolSubStrip)
		Me.pnlTvw.Location = New System.Drawing.Point(2, 27)
		Me.pnlTvw.Name = "pnlTvw"
		Me.pnlTvw.Size = New System.Drawing.Size(296, 437)
		Me.pnlTvw.TabIndex = 0
		'
		'tvwExplore
		'
		Me.tvwExplore.AllowDrop = true
		Me.tvwExplore.BackColor = System.Drawing.SystemColors.Window
		Me.tvwExplore.Dock = System.Windows.Forms.DockStyle.Fill
		Me.tvwExplore.HideSelection = false
		Me.tvwExplore.ImageIndex = 0
		Me.tvwExplore.ImageList = Me.imglstTvw
		Me.tvwExplore.Location = New System.Drawing.Point(24, 0)
		Me.tvwExplore.Name = "tvwExplore"
		Me.tvwExplore.SelectedImageIndex = 0
		Me.tvwExplore.SelectedNodes = CType(resources.GetObject("tvwExplore.SelectedNodes"),System.Collections.ArrayList)
		Me.tvwExplore.Size = New System.Drawing.Size(272, 437)
		Me.tvwExplore.TabIndex = 6
		AddHandler Me.tvwExplore.AfterSelect, AddressOf Me.TvwExploreAfterSelect
		AddHandler Me.tvwExplore.DragDrop, AddressOf Me.TvwExploreDragDrop
		AddHandler Me.tvwExplore.DragEnter, AddressOf Me.TvwExploreDragEnter
		AddHandler Me.tvwExplore.KeyUp, AddressOf Me.TvwExploreKeyUp
		AddHandler Me.tvwExplore.MouseUp, AddressOf Me.TvwExploreMouseUp
		'
		'toolSubStrip
		'
		Me.toolSubStrip.Dock = System.Windows.Forms.DockStyle.Left
		Me.toolSubStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
		Me.toolSubStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btCriteria, Me.btExpand, Me.btSeparator, Me.btCardsFR, Me.btSort})
		Me.toolSubStrip.Location = New System.Drawing.Point(0, 0)
		Me.toolSubStrip.Name = "toolSubStrip"
		Me.toolSubStrip.Size = New System.Drawing.Size(24, 437)
		Me.toolSubStrip.TabIndex = 0
		'
		'btCriteria
		'
		Me.btCriteria.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.btCriteria.Image = CType(resources.GetObject("btCriteria.Image"),System.Drawing.Image)
		Me.btCriteria.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btCriteria.Name = "btCriteria"
		Me.btCriteria.Size = New System.Drawing.Size(21, 20)
		Me.btCriteria.Text = "Filtres d'affichage"
		AddHandler Me.btCriteria.Click, AddressOf Me.BtCriteriaClick
		'
		'btExpand
		'
		Me.btExpand.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.btExpand.Image = CType(resources.GetObject("btExpand.Image"),System.Drawing.Image)
		Me.btExpand.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btExpand.Name = "btExpand"
		Me.btExpand.Size = New System.Drawing.Size(21, 20)
		Me.btExpand.Text = "Déplier l'arborescence"
		AddHandler Me.btExpand.Click, AddressOf Me.BtExpandClick
		'
		'btSeparator
		'
		Me.btSeparator.Name = "btSeparator"
		Me.btSeparator.Size = New System.Drawing.Size(21, 6)
		'
		'btCardsFR
		'
		Me.btCardsFR.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.btCardsFR.Image = CType(resources.GetObject("btCardsFR.Image"),System.Drawing.Image)
		Me.btCardsFR.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btCardsFR.Name = "btCardsFR"
		Me.btCardsFR.Size = New System.Drawing.Size(21, 20)
		Me.btCardsFR.Text = "Titre des cartes en français"
		AddHandler Me.btCardsFR.MouseUp, AddressOf Me.MnuCardsFRActivate
		'
		'btSort
		'
		Me.btSort.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.btSort.Image = CType(resources.GetObject("btSort.Image"),System.Drawing.Image)
		Me.btSort.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btSort.Name = "btSort"
		Me.btSort.Size = New System.Drawing.Size(21, 20)
		Me.btSort.Text = "Trier par ordre alphabétique"
		AddHandler Me.btSort.Click, AddressOf Me.MnuSortClick
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
		Me.splitV2.Panel1.Controls.Add(Me.cbarProperties)
		'
		'splitV2.Panel2
		'
		Me.splitV2.Panel2.Controls.Add(Me.cbarImage)
		Me.splitV2.Size = New System.Drawing.Size(688, 466)
		Me.splitV2.SplitterDistance = 473
		Me.splitV2.TabIndex = 0
		Me.splitV2.TabStop = false
		'
		'cbarProperties
		'
		Me.cbarProperties.Closable = false
		Me.cbarProperties.Controls.Add(Me.pnlProperties)
		Me.cbarProperties.Dock = System.Windows.Forms.DockStyle.Fill
		Me.cbarProperties.DrawActionsButton = false
		Me.cbarProperties.Flow = TD.SandBar.ToolBarLayout.Horizontal
		Me.cbarProperties.Guid = New System.Guid("ea1edb50-d1b7-4eab-b136-020bcdc24f2d")
		Me.cbarProperties.Items.AddRange(New TD.SandBar.ToolbarItemBase() {Me.btShowAll, Me.btCardUse, Me.btHistPrices})
		Me.cbarProperties.Location = New System.Drawing.Point(0, 0)
		Me.cbarProperties.Movable = false
		Me.cbarProperties.Name = "cbarProperties"
		Me.cbarProperties.Size = New System.Drawing.Size(473, 466)
		Me.cbarProperties.TabIndex = 0
		Me.cbarProperties.Text = "Propriétés"
		'
		'pnlProperties
		'
		Me.pnlProperties.Controls.Add(Me.splitH)
		Me.pnlProperties.Controls.Add(Me.grpAutorisations)
		Me.pnlProperties.Location = New System.Drawing.Point(2, 49)
		Me.pnlProperties.Name = "pnlProperties"
		Me.pnlProperties.Size = New System.Drawing.Size(469, 415)
		Me.pnlProperties.TabIndex = 0
		'
		'splitH
		'
		Me.splitH.BackColor = System.Drawing.Color.Transparent
		Me.splitH.Dock = System.Windows.Forms.DockStyle.Fill
		Me.splitH.Location = New System.Drawing.Point(0, 0)
		Me.splitH.Name = "splitH"
		Me.splitH.Orientation = System.Windows.Forms.Orientation.Horizontal
		'
		'splitH.Panel1
		'
		Me.splitH.Panel1.Controls.Add(Me.pnlCard)
		Me.splitH.Panel1.Controls.Add(Me.pnlAlternate)
		'
		'splitH.Panel2
		'
		Me.splitH.Panel2.Controls.Add(Me.txtRichOther)
		Me.splitH.Size = New System.Drawing.Size(469, 376)
		Me.splitH.SplitterDistance = 290
		Me.splitH.SplitterWidth = 10
		Me.splitH.TabIndex = 19
		'
		'pnlCard
		'
		Me.pnlCard.Controls.Add(Me.splitH2)
		Me.pnlCard.Dock = System.Windows.Forms.DockStyle.Fill
		Me.pnlCard.Location = New System.Drawing.Point(0, 0)
		Me.pnlCard.Name = "pnlCard"
		Me.pnlCard.Size = New System.Drawing.Size(469, 290)
		Me.pnlCard.TabIndex = 1
		'
		'splitH2
		'
		Me.splitH2.Dock = System.Windows.Forms.DockStyle.Fill
		Me.splitH2.Location = New System.Drawing.Point(0, 0)
		Me.splitH2.Name = "splitH2"
		Me.splitH2.Orientation = System.Windows.Forms.Orientation.Horizontal
		'
		'splitH2.Panel1
		'
		Me.splitH2.Panel1.Controls.Add(Me.pnlCard1)
		'
		'splitH2.Panel2
		'
		Me.splitH2.Panel2.Controls.Add(Me.splitH3)
		Me.splitH2.Size = New System.Drawing.Size(469, 290)
		Me.splitH2.SplitterDistance = 150
		Me.splitH2.TabIndex = 0
		'
		'pnlCard1
		'
		Me.pnlCard1.Controls.Add(Me.grdPropCard)
		Me.pnlCard1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.pnlCard1.Location = New System.Drawing.Point(0, 0)
		Me.pnlCard1.Name = "pnlCard1"
		Me.pnlCard1.Size = New System.Drawing.Size(469, 150)
		Me.pnlCard1.TabIndex = 3
		'
		'grdPropCard
		'
		Me.grdPropCard.AutoSizeMinHeight = 10
		Me.grdPropCard.AutoSizeMinWidth = 10
		Me.grdPropCard.AutoStretchColumnsToFitWidth = false
		Me.grdPropCard.AutoStretchRowsToFitHeight = false
		Me.grdPropCard.BackColor = System.Drawing.Color.Transparent
		Me.grdPropCard.ContextMenuStyle = SourceGrid2.ContextMenuStyle.None
		Me.grdPropCard.CustomSort = false
		Me.grdPropCard.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdPropCard.GridToolTipActive = true
		Me.grdPropCard.Location = New System.Drawing.Point(0, 0)
		Me.grdPropCard.Name = "grdPropCard"
		Me.grdPropCard.Size = New System.Drawing.Size(469, 150)
		Me.grdPropCard.SpecialKeys = CType(((((((((((SourceGrid2.GridSpecialKeys.Ctrl_C Or SourceGrid2.GridSpecialKeys.Ctrl_V)  _
						Or SourceGrid2.GridSpecialKeys.Ctrl_X)  _
						Or SourceGrid2.GridSpecialKeys.Delete)  _
						Or SourceGrid2.GridSpecialKeys.Arrows)  _
						Or SourceGrid2.GridSpecialKeys.Tab)  _
						Or SourceGrid2.GridSpecialKeys.PageDownUp)  _
						Or SourceGrid2.GridSpecialKeys.Enter)  _
						Or SourceGrid2.GridSpecialKeys.Escape)  _
						Or SourceGrid2.GridSpecialKeys.Control)  _
						Or SourceGrid2.GridSpecialKeys.Shift),SourceGrid2.GridSpecialKeys)
		Me.grdPropCard.TabIndex = 2
		'
		'splitH3
		'
		Me.splitH3.Dock = System.Windows.Forms.DockStyle.Fill
		Me.splitH3.IsSplitterFixed = true
		Me.splitH3.Location = New System.Drawing.Point(0, 0)
		Me.splitH3.Name = "splitH3"
		Me.splitH3.Orientation = System.Windows.Forms.Orientation.Horizontal
		'
		'splitH3.Panel1
		'
		Me.splitH3.Panel1.Controls.Add(Me.splitV3)
		'
		'splitH3.Panel2
		'
		Me.splitH3.Panel2.Controls.Add(Me.txtRichCard)
		Me.splitH3.Size = New System.Drawing.Size(469, 136)
		Me.splitH3.SplitterDistance = 42
		Me.splitH3.TabIndex = 0
		'
		'splitV3
		'
		Me.splitV3.Dock = System.Windows.Forms.DockStyle.Fill
		Me.splitV3.Location = New System.Drawing.Point(0, 0)
		Me.splitV3.Name = "splitV3"
		'
		'splitV3.Panel1
		'
		Me.splitV3.Panel1.Controls.Add(Me.pnlCard2)
		'
		'splitV3.Panel2
		'
		Me.splitV3.Panel2.Controls.Add(Me.pnlCard3)
		Me.splitV3.Size = New System.Drawing.Size(469, 42)
		Me.splitV3.SplitterDistance = 234
		Me.splitV3.TabIndex = 3
		'
		'pnlCard2
		'
		Me.pnlCard2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.pnlCard2.Controls.Add(Me.picCost)
		Me.pnlCard2.Dock = System.Windows.Forms.DockStyle.Fill
		Me.pnlCard2.Location = New System.Drawing.Point(0, 0)
		Me.pnlCard2.Name = "pnlCard2"
		Me.pnlCard2.Size = New System.Drawing.Size(234, 42)
		Me.pnlCard2.TabIndex = 3
		'
		'picCost
		'
		Me.picCost.Image = CType(resources.GetObject("picCost.Image"),System.Drawing.Image)
		Me.picCost.Location = New System.Drawing.Point(3, 3)
		Me.picCost.Name = "picCost"
		Me.picCost.Size = New System.Drawing.Size(32, 32)
		Me.picCost.TabIndex = 1
		Me.picCost.TabStop = false
		'
		'pnlCard3
		'
		Me.pnlCard3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.pnlCard3.Controls.Add(Me.lblPowerTough)
		Me.pnlCard3.Controls.Add(Me.picPowerTough)
		Me.pnlCard3.Dock = System.Windows.Forms.DockStyle.Fill
		Me.pnlCard3.Location = New System.Drawing.Point(0, 0)
		Me.pnlCard3.Name = "pnlCard3"
		Me.pnlCard3.Size = New System.Drawing.Size(231, 42)
		Me.pnlCard3.TabIndex = 4
		'
		'lblPowerTough
		'
		Me.lblPowerTough.AutoSize = true
		Me.lblPowerTough.Font = New System.Drawing.Font("Arial", 15!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic),System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.lblPowerTough.Location = New System.Drawing.Point(55, 8)
		Me.lblPowerTough.Name = "lblPowerTough"
		Me.lblPowerTough.Size = New System.Drawing.Size(50, 24)
		Me.lblPowerTough.TabIndex = 10
		Me.lblPowerTough.Text = "0 / 0"
		'
		'picPowerTough
		'
		Me.picPowerTough.Image = CType(resources.GetObject("picPowerTough.Image"),System.Drawing.Image)
		Me.picPowerTough.Location = New System.Drawing.Point(3, 3)
		Me.picPowerTough.Name = "picPowerTough"
		Me.picPowerTough.Size = New System.Drawing.Size(32, 32)
		Me.picPowerTough.TabIndex = 9
		Me.picPowerTough.TabStop = false
		'
		'txtRichCard
		'
		Me.txtRichCard.AcceptsTab = true
		Me.txtRichCard.BackColor = System.Drawing.Color.White
		Me.txtRichCard.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.txtRichCard.Dock = System.Windows.Forms.DockStyle.Fill
		Me.txtRichCard.HiglightColor = Magic_The_Gathering_Manager.ExRichTextBox.eRtfColor.White
		Me.txtRichCard.Location = New System.Drawing.Point(0, 0)
		Me.txtRichCard.Name = "txtRichCard"
		Me.txtRichCard.ReadOnly = true
		Me.txtRichCard.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical
		Me.txtRichCard.Size = New System.Drawing.Size(469, 90)
		Me.txtRichCard.TabIndex = 17
		Me.txtRichCard.Text = ""
		Me.txtRichCard.TextColor = Magic_The_Gathering_Manager.ExRichTextBox.eRtfColor.Black
		'
		'pnlAlternate
		'
		Me.pnlAlternate.Controls.Add(Me.propAlternate)
		Me.pnlAlternate.Dock = System.Windows.Forms.DockStyle.Fill
		Me.pnlAlternate.Location = New System.Drawing.Point(0, 0)
		Me.pnlAlternate.Name = "pnlAlternate"
		Me.pnlAlternate.Size = New System.Drawing.Size(469, 290)
		Me.pnlAlternate.TabIndex = 0
		'
		'propAlternate
		'
		Me.propAlternate.Dock = System.Windows.Forms.DockStyle.Fill
		Me.propAlternate.Location = New System.Drawing.Point(0, 0)
		Me.propAlternate.Name = "propAlternate"
		Me.propAlternate.PropertySort = System.Windows.Forms.PropertySort.NoSort
		Me.propAlternate.Size = New System.Drawing.Size(469, 290)
		Me.propAlternate.TabIndex = 0
		Me.propAlternate.ToolbarVisible = false
		'
		'txtRichOther
		'
		Me.txtRichOther.AcceptsTab = true
		Me.txtRichOther.BackColor = System.Drawing.Color.White
		Me.txtRichOther.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.txtRichOther.Dock = System.Windows.Forms.DockStyle.Fill
		Me.txtRichOther.HiglightColor = Magic_The_Gathering_Manager.ExRichTextBox.eRtfColor.White
		Me.txtRichOther.Location = New System.Drawing.Point(0, 0)
		Me.txtRichOther.Name = "txtRichOther"
		Me.txtRichOther.ReadOnly = true
		Me.txtRichOther.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical
		Me.txtRichOther.Size = New System.Drawing.Size(469, 76)
		Me.txtRichOther.TabIndex = 18
		Me.txtRichOther.Text = ""
		Me.txtRichOther.TextColor = Magic_The_Gathering_Manager.ExRichTextBox.eRtfColor.Black
		'
		'grpAutorisations
		'
		Me.grpAutorisations.BackColor = System.Drawing.Color.Transparent
		Me.grpAutorisations.Controls.Add(Me.picAutT1)
		Me.grpAutorisations.Controls.Add(Me.picAutT15)
		Me.grpAutorisations.Controls.Add(Me.picAutM)
		Me.grpAutorisations.Controls.Add(Me.picAutT1x)
		Me.grpAutorisations.Controls.Add(Me.picAutT2)
		Me.grpAutorisations.Controls.Add(Me.picAutBloc)
		Me.grpAutorisations.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.grpAutorisations.Location = New System.Drawing.Point(0, 376)
		Me.grpAutorisations.Name = "grpAutorisations"
		Me.grpAutorisations.Size = New System.Drawing.Size(469, 39)
		Me.grpAutorisations.TabIndex = 18
		Me.grpAutorisations.TabStop = false
		'
		'picAutT1
		'
		Me.picAutT1.Location = New System.Drawing.Point(3, 10)
		Me.picAutT1.Name = "picAutT1"
		Me.picAutT1.Size = New System.Drawing.Size(35, 25)
		Me.picAutT1.TabIndex = 4
		Me.picAutT1.TabStop = false
		'
		'picAutT15
		'
		Me.picAutT15.Location = New System.Drawing.Point(44, 10)
		Me.picAutT15.Name = "picAutT15"
		Me.picAutT15.Size = New System.Drawing.Size(35, 25)
		Me.picAutT15.TabIndex = 3
		Me.picAutT15.TabStop = false
		'
		'picAutM
		'
		Me.picAutM.Location = New System.Drawing.Point(85, 10)
		Me.picAutM.Name = "picAutM"
		Me.picAutM.Size = New System.Drawing.Size(35, 25)
		Me.picAutM.TabIndex = 3
		Me.picAutM.TabStop = false
		'
		'picAutT1x
		'
		Me.picAutT1x.Location = New System.Drawing.Point(126, 10)
		Me.picAutT1x.Name = "picAutT1x"
		Me.picAutT1x.Size = New System.Drawing.Size(35, 25)
		Me.picAutT1x.TabIndex = 2
		Me.picAutT1x.TabStop = false
		'
		'picAutT2
		'
		Me.picAutT2.Location = New System.Drawing.Point(167, 10)
		Me.picAutT2.Name = "picAutT2"
		Me.picAutT2.Size = New System.Drawing.Size(35, 25)
		Me.picAutT2.TabIndex = 1
		Me.picAutT2.TabStop = false
		'
		'picAutBloc
		'
		Me.picAutBloc.Location = New System.Drawing.Point(208, 10)
		Me.picAutBloc.Name = "picAutBloc"
		Me.picAutBloc.Size = New System.Drawing.Size(35, 25)
		Me.picAutBloc.TabIndex = 0
		Me.picAutBloc.TabStop = false
		'
		'btShowAll
		'
		Me.btShowAll.Checked = true
		Me.btShowAll.Icon = CType(resources.GetObject("btShowAll.Icon"),System.Drawing.Icon)
		Me.btShowAll.Text = "Tout afficher"
		AddHandler Me.btShowAll.Activate, AddressOf Me.BtShowAllActivate
		'
		'btCardUse
		'
		Me.btCardUse.Icon = CType(resources.GetObject("btCardUse.Icon"),System.Drawing.Icon)
		Me.btCardUse.Text = "Utilisation"
		AddHandler Me.btCardUse.Activate, AddressOf Me.BtCardUseActivate
		'
		'btHistPrices
		'
		Me.btHistPrices.Enabled = false
		Me.btHistPrices.Icon = CType(resources.GetObject("btHistPrices.Icon"),System.Drawing.Icon)
		Me.btHistPrices.Text = "Historique"
		AddHandler Me.btHistPrices.Activate, AddressOf Me.BtHistPricesActivate
		'
		'cbarImage
		'
		Me.cbarImage.Closable = false
		Me.cbarImage.Controls.Add(Me.pnlImage)
		Me.cbarImage.Dock = System.Windows.Forms.DockStyle.Fill
		Me.cbarImage.DrawActionsButton = false
		Me.cbarImage.Flow = TD.SandBar.ToolBarLayout.Horizontal
		Me.cbarImage.Guid = New System.Guid("ea1edb50-d1b7-4eab-b136-020bcdc24f2d")
		Me.cbarImage.Location = New System.Drawing.Point(0, 0)
		Me.cbarImage.Movable = false
		Me.cbarImage.Name = "cbarImage"
		Me.cbarImage.Size = New System.Drawing.Size(211, 466)
		Me.cbarImage.TabIndex = 1
		Me.cbarImage.Text = "Image"
		'
		'pnlImage
		'
		Me.pnlImage.Controls.Add(Me.splitH4)
		Me.pnlImage.Location = New System.Drawing.Point(2, 27)
		Me.pnlImage.Name = "pnlImage"
		Me.pnlImage.Size = New System.Drawing.Size(207, 437)
		Me.pnlImage.TabIndex = 0
		'
		'splitH4
		'
		Me.splitH4.Dock = System.Windows.Forms.DockStyle.Fill
		Me.splitH4.IsSplitterFixed = true
		Me.splitH4.Location = New System.Drawing.Point(0, 0)
		Me.splitH4.Name = "splitH4"
		Me.splitH4.Orientation = System.Windows.Forms.Orientation.Horizontal
		'
		'splitH4.Panel1
		'
		Me.splitH4.Panel1.Controls.Add(Me.picScanCard)
		'
		'splitH4.Panel2
		'
		Me.splitH4.Panel2.Controls.Add(Me.grdPropPicture)
		Me.splitH4.Size = New System.Drawing.Size(207, 437)
		Me.splitH4.SplitterDistance = 295
		Me.splitH4.TabIndex = 1
		'
		'picScanCard
		'
		Me.picScanCard.Dock = System.Windows.Forms.DockStyle.Fill
		Me.picScanCard.Location = New System.Drawing.Point(0, 0)
		Me.picScanCard.Name = "picScanCard"
		Me.picScanCard.Size = New System.Drawing.Size(207, 295)
		Me.picScanCard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
		Me.picScanCard.TabIndex = 2
		Me.picScanCard.TabStop = false
		AddHandler Me.picScanCard.MouseUp, AddressOf Me.PicScanCardMouseUp
		'
		'grdPropPicture
		'
		Me.grdPropPicture.AutoSizeMinHeight = 10
		Me.grdPropPicture.AutoSizeMinWidth = 10
		Me.grdPropPicture.AutoStretchColumnsToFitWidth = false
		Me.grdPropPicture.AutoStretchRowsToFitHeight = false
		Me.grdPropPicture.BackColor = System.Drawing.Color.Transparent
		Me.grdPropPicture.ContextMenuStyle = SourceGrid2.ContextMenuStyle.None
		Me.grdPropPicture.CustomSort = false
		Me.grdPropPicture.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdPropPicture.GridToolTipActive = true
		Me.grdPropPicture.Location = New System.Drawing.Point(0, 0)
		Me.grdPropPicture.Name = "grdPropPicture"
		Me.grdPropPicture.Size = New System.Drawing.Size(207, 138)
		Me.grdPropPicture.SpecialKeys = CType(((((((((((SourceGrid2.GridSpecialKeys.Ctrl_C Or SourceGrid2.GridSpecialKeys.Ctrl_V)  _
						Or SourceGrid2.GridSpecialKeys.Ctrl_X)  _
						Or SourceGrid2.GridSpecialKeys.Delete)  _
						Or SourceGrid2.GridSpecialKeys.Arrows)  _
						Or SourceGrid2.GridSpecialKeys.Tab)  _
						Or SourceGrid2.GridSpecialKeys.PageDownUp)  _
						Or SourceGrid2.GridSpecialKeys.Enter)  _
						Or SourceGrid2.GridSpecialKeys.Escape)  _
						Or SourceGrid2.GridSpecialKeys.Control)  _
						Or SourceGrid2.GridSpecialKeys.Shift),SourceGrid2.GridSpecialKeys)
		Me.grdPropPicture.TabIndex = 4
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
		Me.imglstAutorisations.Images.SetKeyName(16, "_aM.gif")
		Me.imglstAutorisations.Images.SetKeyName(17, "_aMno.gif")
		Me.imglstAutorisations.Images.SetKeyName(18, "_aMoff.gif")
		'
		'cmnuCbar
		'
		Me.cmnuCbar.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btHistPricesSimple, Me.btHistPricesFoil})
		Me.cmnuCbar.Name = "cmnuCbar"
		Me.cmnuCbar.Size = New System.Drawing.Size(153, 70)
		AddHandler Me.cmnuCbar.Closed, AddressOf Me.CmnuCbarClosed
		'
		'btHistPricesSimple
		'
		Me.btHistPricesSimple.Name = "btHistPricesSimple"
		Me.btHistPricesSimple.Size = New System.Drawing.Size(152, 22)
		Me.btHistPricesSimple.Text = "Prix"
		AddHandler Me.btHistPricesSimple.Click, AddressOf Me.BtHistPricesSimpleClick
		'
		'btHistPricesFoil
		'
		Me.btHistPricesFoil.Name = "btHistPricesFoil"
		Me.btHistPricesFoil.Size = New System.Drawing.Size(152, 22)
		Me.btHistPricesFoil.Text = "Prix foil"
		AddHandler Me.btHistPricesFoil.Click, AddressOf Me.BtHistPricesFoilClick
		'
		'mnuPlugHTML
		'
		Me.mnuPlugHTML.Image = CType(resources.GetObject("mnuPlugHTML.Image"),System.Drawing.Image)
		Me.mnuPlugHTML.Name = "mnuPlugHTML"
		Me.mnuPlugHTML.Size = New System.Drawing.Size(199, 22)
		Me.mnuPlugHTML.Text = "HTML CollectionViewer"
		AddHandler Me.mnuPlugHTML.Click, AddressOf Me.MnuPlugHTMLClick
		'
		'MainForm
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(992, 566)
		Me.Controls.Add(Me.splitV)
		Me.Controls.Add(Me.toolStrip)
		Me.Controls.Add(Me.statusStrip)
		Me.Controls.Add(Me.mnu)
		Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
		Me.MainMenuStrip = Me.mnu
		Me.Name = "MainForm"
		Me.Text = "agic The Gathering Manager"
		AddHandler FormClosing, AddressOf Me.MainFormFormClosing
		AddHandler Load, AddressOf Me.MainFormLoad
		AddHandler ResizeEnd, AddressOf Me.MainFormResizeEnd
		AddHandler Resize, AddressOf Me.MainFormResize
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
		Me.cbarTvw.ResumeLayout(false)
		Me.pnlTvw.ResumeLayout(false)
		Me.pnlTvw.PerformLayout
		Me.toolSubStrip.ResumeLayout(false)
		Me.toolSubStrip.PerformLayout
		Me.splitV2.Panel1.ResumeLayout(false)
		Me.splitV2.Panel2.ResumeLayout(false)
		Me.splitV2.ResumeLayout(false)
		Me.cbarProperties.ResumeLayout(false)
		Me.pnlProperties.ResumeLayout(false)
		Me.splitH.Panel1.ResumeLayout(false)
		Me.splitH.Panel2.ResumeLayout(false)
		Me.splitH.ResumeLayout(false)
		Me.pnlCard.ResumeLayout(false)
		Me.splitH2.Panel1.ResumeLayout(false)
		Me.splitH2.Panel2.ResumeLayout(false)
		Me.splitH2.ResumeLayout(false)
		Me.pnlCard1.ResumeLayout(false)
		Me.splitH3.Panel1.ResumeLayout(false)
		Me.splitH3.Panel2.ResumeLayout(false)
		Me.splitH3.ResumeLayout(false)
		Me.splitV3.Panel1.ResumeLayout(false)
		Me.splitV3.Panel2.ResumeLayout(false)
		Me.splitV3.ResumeLayout(false)
		Me.pnlCard2.ResumeLayout(false)
		CType(Me.picCost,System.ComponentModel.ISupportInitialize).EndInit
		Me.pnlCard3.ResumeLayout(false)
		Me.pnlCard3.PerformLayout
		CType(Me.picPowerTough,System.ComponentModel.ISupportInitialize).EndInit
		Me.pnlAlternate.ResumeLayout(false)
		Me.grpAutorisations.ResumeLayout(false)
		CType(Me.picAutT1,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.picAutT15,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.picAutM,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.picAutT1x,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.picAutT2,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.picAutBloc,System.ComponentModel.ISupportInitialize).EndInit
		Me.cbarImage.ResumeLayout(false)
		Me.pnlImage.ResumeLayout(false)
		Me.splitH4.Panel1.ResumeLayout(false)
		Me.splitH4.Panel2.ResumeLayout(false)
		Me.splitH4.ResumeLayout(false)
		CType(Me.picScanCard,System.ComponentModel.ISupportInitialize).EndInit
		Me.cmnuCbar.ResumeLayout(false)
		Me.ResumeLayout(false)
		Me.PerformLayout
	End Sub
	Private mnuPlugHTML As System.Windows.Forms.ToolStripMenuItem
	Private btExpand As System.Windows.Forms.ToolStripButton
	Private btCardUse As TD.SandBar.ButtonItem
	Public mnuDispAdvSearch As System.Windows.Forms.ToolStripMenuItem
	Private btHistPricesSimple As System.Windows.Forms.ToolStripMenuItem
	Private btHistPricesFoil As System.Windows.Forms.ToolStripMenuItem
	Private cmnuCbar As System.Windows.Forms.ContextMenuStrip
	Private btSimu As System.Windows.Forms.ToolStripButton
	Private txtRichCard As Magic_The_Gathering_Manager.ExRichTextBox
	Private txtRichOther As Magic_The_Gathering_Manager.ExRichTextBox
	Private pnlCard3 As System.Windows.Forms.Panel
	Private lblPowerTough As System.Windows.Forms.Label
	Private picPowerTough As System.Windows.Forms.PictureBox
	Private splitV3 As System.Windows.Forms.SplitContainer
	Private btHistPrices As TD.SandBar.ButtonItem
	Private btShowAll As TD.SandBar.ButtonItem
	Private propAlternate As System.Windows.Forms.PropertyGrid
	Private pnlAlternate As System.Windows.Forms.Panel
	Private pnlCard1 As System.Windows.Forms.Panel
	Private picCost As System.Windows.Forms.PictureBox
	Private pnlCard2 As System.Windows.Forms.Panel
	Private splitH4 As System.Windows.Forms.SplitContainer
	Private grdPropPicture As SourceGrid2.Grid
	Private splitH3 As System.Windows.Forms.SplitContainer
	Private splitH2 As System.Windows.Forms.SplitContainer
	Private grdPropCard As SourceGrid2.Grid
	Private pnlCard As System.Windows.Forms.Panel
	Private splitH As System.Windows.Forms.SplitContainer
	Private mnuUpdateRulings As System.Windows.Forms.ToolStripMenuItem
	Private btPlateau As System.Windows.Forms.ToolStripButton
	Private mnuPlateau As System.Windows.Forms.ToolStripMenuItem
	Private mnuTransform As System.Windows.Forms.ToolStripMenuItem
	Private btSort As System.Windows.Forms.ToolStripButton
	Private btSeparator As System.Windows.Forms.ToolStripSeparator
	Private toolSubStrip As System.Windows.Forms.ToolStrip
	Private cbarTvw As TD.SandBar.ContainerBar
	Private btCriteria As System.Windows.Forms.ToolStripButton
	Private btCardsFR As System.Windows.Forms.ToolStripButton
	Private mnuCollapseRarete As System.Windows.Forms.ToolStripMenuItem
	Private mnuSwapSerie As System.Windows.Forms.ToolStripMenuItem
	Private btWordGen As System.Windows.Forms.ToolStripButton
	Private mnuWordGen As System.Windows.Forms.ToolStripMenuItem
	Private mnuCopyToCollection As System.Windows.Forms.ToolStripMenuItem
	Private mnuCopyACard As System.Windows.Forms.ToolStripMenuItem
	Private mnuPlugResourcer As System.Windows.Forms.ToolStripMenuItem
	Private mnuPlugins As System.Windows.Forms.ToolStripMenuItem
	Private mnuGestAdv As System.Windows.Forms.ToolStripMenuItem
	Private btWebsite As System.Windows.Forms.ToolStripButton
	Private mnuWebsite As System.Windows.Forms.ToolStripMenuItem
	Private mnuMV As System.Windows.Forms.ToolStripMenuItem
	Private mnuContenuUpdate As System.Windows.Forms.ToolStripMenuItem
	Private mnuApplicationUpdate As System.Windows.Forms.ToolStripMenuItem
	Private mnuUpdateAutorisations As System.Windows.Forms.ToolStripMenuItem
	Private imglstAutorisations As System.Windows.Forms.ImageList
	Private picAutBloc As System.Windows.Forms.PictureBox
	Private picAutT2 As System.Windows.Forms.PictureBox
	Private picAutT1x As System.Windows.Forms.PictureBox
	Private picAutT15 As System.Windows.Forms.PictureBox
	Private picAutM As System.Windows.Forms.PictureBox
	Private picAutT1 As System.Windows.Forms.PictureBox
	Private grpAutorisations As System.Windows.Forms.GroupBox
	Private mnuFixSerie2 As System.Windows.Forms.ToolStripMenuItem
	Private mnuFixFR2 As System.Windows.Forms.ToolStripMenuItem
	Private mnuFixPic As System.Windows.Forms.ToolStripMenuItem
	Private mnuFixDivers As System.Windows.Forms.ToolStripMenuItem
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
	Private cbarImage As TD.SandBar.ContainerBar
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
	Private mnuPerfs As System.Windows.Forms.ToolStripMenuItem
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
	Private lblNCards As System.Windows.Forms.ToolStripStatusLabel
	Public imglstCarac As System.Windows.Forms.ImageList
	Private mnuCardsFR As System.Windows.Forms.ToolStripMenuItem
	Private cmnuTvw As System.Windows.Forms.ContextMenuStrip
	Private mnuRefresh As System.Windows.Forms.ToolStripMenuItem
	Private mnuDispCollection As System.Windows.Forms.ToolStripMenuItem
	Public mnuDisp As System.Windows.Forms.ToolStripMenuItem
	Public tvwExplore As TreeViewMS.TreeViewMS
	Private pnlProperties As TD.SandBar.ContainerBarClientPanel
	Private cbarProperties As TD.SandBar.ContainerBar
	Private pnlTvw As TD.SandBar.ContainerBarClientPanel
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
