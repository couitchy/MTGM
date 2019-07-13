'
' Created by SharpDevelop.
' User: Couitchy
' Date: 26/04/2008
' Time: 15:05
'
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Partial Class frmStats
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmStats))
        Me.splitH = New System.Windows.Forms.SplitContainer()
        Me.cbarChart = New TD.SandBar.ContainerBar()
        Me.pnlChart = New TD.SandBar.ContainerBarClientPanel()
        Me.chartManaCurve = New NPlot.Windows.PlotSurface2D()
        Me.cmnuChart = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmnuBreakDown = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuCurve = New System.Windows.Forms.ToolStripMenuItem()
        Me.chartBreakDown = New SoftwareFX.ChartFX.Lite.Chart()
        Me.cboCriterion = New TD.SandBar.ComboBoxItem()
        Me.cbarInfos = New TD.SandBar.ContainerBar()
        Me.pnlInfos = New TD.SandBar.ContainerBarClientPanel()
        Me.splitV = New System.Windows.Forms.SplitContainer()
        Me.tabInfos = New System.Windows.Forms.TabControl()
        Me.tabGeneral = New System.Windows.Forms.TabPage()
        Me.grpGeneral = New System.Windows.Forms.GroupBox()
        Me.txtOldest = New System.Windows.Forms.TextBox()
        Me.txtRarest = New System.Windows.Forms.TextBox()
        Me.txtTougher = New System.Windows.Forms.TextBox()
        Me.txtNCartes = New System.Windows.Forms.TextBox()
        Me.lblOldest = New System.Windows.Forms.Label()
        Me.lblRarest = New System.Windows.Forms.Label()
        Me.lblTougher = New System.Windows.Forms.Label()
        Me.lblNCartes = New System.Windows.Forms.Label()
        Me.tabPrix = New System.Windows.Forms.TabPage()
        Me.grpPrix = New System.Windows.Forms.GroupBox()
        Me.cmdHistPrices = New System.Windows.Forms.Button()
        Me.txtMeanPrice2 = New System.Windows.Forms.TextBox()
        Me.lblMeanPrice2 = New System.Windows.Forms.Label()
        Me.txtMostExpensive = New System.Windows.Forms.TextBox()
        Me.lblMostExpensive = New System.Windows.Forms.Label()
        Me.txtMeanPrice = New System.Windows.Forms.TextBox()
        Me.lblMeanPrice = New System.Windows.Forms.Label()
        Me.txtTotPrice = New System.Windows.Forms.TextBox()
        Me.lblTotPrice = New System.Windows.Forms.Label()
        Me.tabEfficacite = New System.Windows.Forms.TabPage()
        Me.grpInvocation = New System.Windows.Forms.GroupBox()
        Me.txtMeanCost2 = New System.Windows.Forms.TextBox()
        Me.lblMeanCost2 = New System.Windows.Forms.Label()
        Me.txtMaxCost = New System.Windows.Forms.TextBox()
        Me.lblMaxCost = New System.Windows.Forms.Label()
        Me.txtMinCost = New System.Windows.Forms.TextBox()
        Me.lblMinCost = New System.Windows.Forms.Label()
        Me.txtMeanCost = New System.Windows.Forms.TextBox()
        Me.lblMeanCost = New System.Windows.Forms.Label()
        Me.tabCreatures = New System.Windows.Forms.TabPage()
        Me.grpCreatures = New System.Windows.Forms.GroupBox()
        Me.txtMeanTough = New System.Windows.Forms.TextBox()
        Me.lblMeanTough = New System.Windows.Forms.Label()
        Me.txtRAC = New System.Windows.Forms.TextBox()
        Me.lblRAC = New System.Windows.Forms.Label()
        Me.txtRAD = New System.Windows.Forms.TextBox()
        Me.lblRAD = New System.Windows.Forms.Label()
        Me.txtMeanPower = New System.Windows.Forms.TextBox()
        Me.lblMeanPower = New System.Windows.Forms.Label()
        Me.tabAutorisations = New System.Windows.Forms.TabPage()
        Me.lstTournoiForbid = New System.Windows.Forms.ListBox()
        Me.grpAutorisations = New System.Windows.Forms.GroupBox()
        Me.picAutMulti = New System.Windows.Forms.PictureBox()
        Me.picAutM = New System.Windows.Forms.PictureBox()
        Me.picAutT1 = New System.Windows.Forms.PictureBox()
        Me.picAutT15 = New System.Windows.Forms.PictureBox()
        Me.picAutT2 = New System.Windows.Forms.PictureBox()
        Me.picAutBloc = New System.Windows.Forms.PictureBox()
        Me.picAut1V1 = New System.Windows.Forms.PictureBox()
        Me.lblAutorisations = New System.Windows.Forms.Label()
        Me.grdDetails = New SourceGrid2.Grid()
        Me.cmnuHisto = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmnuHistDeck = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuHistCards = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuHistAllCards = New System.Windows.Forms.ToolStripMenuItem()
        Me.toolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.cmnuHistCardsPrice1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuHistCardsPrice2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuHistCardsPrice3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuHistCardsPrice4 = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuHistCardsPrice5 = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuHistCardsPrice6 = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuHistCardsPrice7 = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuHistCardsPrice8 = New System.Windows.Forms.ToolStripMenuItem()
        Me.imglstAutorisations = New System.Windows.Forms.ImageList(Me.components)
        Me.splitH.Panel1.SuspendLayout
        Me.splitH.Panel2.SuspendLayout
        Me.splitH.SuspendLayout
        Me.cbarChart.SuspendLayout
        Me.pnlChart.SuspendLayout
        Me.cmnuChart.SuspendLayout
        Me.cbarInfos.SuspendLayout
        Me.pnlInfos.SuspendLayout
        Me.splitV.Panel1.SuspendLayout
        Me.splitV.Panel2.SuspendLayout
        Me.splitV.SuspendLayout
        Me.tabInfos.SuspendLayout
        Me.tabGeneral.SuspendLayout
        Me.grpGeneral.SuspendLayout
        Me.tabPrix.SuspendLayout
        Me.grpPrix.SuspendLayout
        Me.tabEfficacite.SuspendLayout
        Me.grpInvocation.SuspendLayout
        Me.tabCreatures.SuspendLayout
        Me.grpCreatures.SuspendLayout
        Me.tabAutorisations.SuspendLayout
        Me.grpAutorisations.SuspendLayout
        CType(Me.picAutMulti,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.picAutM,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.picAutT1,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.picAutT15,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.picAutT2,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.picAutBloc,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.picAut1V1,System.ComponentModel.ISupportInitialize).BeginInit
        Me.cmnuHisto.SuspendLayout
        Me.SuspendLayout
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
        Me.splitH.Panel1.Controls.Add(Me.cbarChart)
        '
        'splitH.Panel2
        '
        Me.splitH.Panel2.Controls.Add(Me.cbarInfos)
        Me.splitH.Size = New System.Drawing.Size(579, 415)
        Me.splitH.SplitterDistance = 220
        Me.splitH.TabIndex = 0
        '
        'cbarChart
        '
        Me.cbarChart.Closable = false
        Me.cbarChart.Controls.Add(Me.pnlChart)
        Me.cbarChart.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cbarChart.DrawActionsButton = false
        Me.cbarChart.Flow = TD.SandBar.ToolBarLayout.Horizontal
        Me.cbarChart.Guid = New System.Guid("c298d149-b8ec-4c9d-a8c7-dd350bf6317b")
        Me.cbarChart.Items.AddRange(New TD.SandBar.ToolbarItemBase() {Me.cboCriterion})
        Me.cbarChart.Location = New System.Drawing.Point(0, 0)
        Me.cbarChart.Movable = false
        Me.cbarChart.Name = "cbarChart"
        Me.cbarChart.Size = New System.Drawing.Size(579, 220)
        Me.cbarChart.TabIndex = 2
        Me.cbarChart.Text = "Répartitions"
        '
        'pnlChart
        '
        Me.pnlChart.Controls.Add(Me.chartManaCurve)
        Me.pnlChart.Controls.Add(Me.chartBreakDown)
        Me.pnlChart.Location = New System.Drawing.Point(2, 49)
        Me.pnlChart.Name = "pnlChart"
        Me.pnlChart.Size = New System.Drawing.Size(575, 169)
        Me.pnlChart.TabIndex = 0
        '
        'chartManaCurve
        '
        Me.chartManaCurve.AutoScaleAutoGeneratedAxes = false
        Me.chartManaCurve.AutoScaleTitle = false
        Me.chartManaCurve.BackColor = System.Drawing.SystemColors.Control
        Me.chartManaCurve.ContextMenuStrip = Me.cmnuChart
        Me.chartManaCurve.DateTimeToolTip = false
        Me.chartManaCurve.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chartManaCurve.Legend = Nothing
        Me.chartManaCurve.LegendZOrder = -1
        Me.chartManaCurve.Location = New System.Drawing.Point(0, 0)
        Me.chartManaCurve.Name = "chartManaCurve"
        Me.chartManaCurve.RightMenu = Nothing
        Me.chartManaCurve.ShowCoordinates = false
        Me.chartManaCurve.Size = New System.Drawing.Size(575, 169)
        Me.chartManaCurve.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None
        Me.chartManaCurve.TabIndex = 2
        Me.chartManaCurve.Title = ""
        Me.chartManaCurve.TitleFont = New System.Drawing.Font("Arial", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
        Me.chartManaCurve.Visible = false
        Me.chartManaCurve.XAxis1 = Nothing
        Me.chartManaCurve.XAxis2 = Nothing
        Me.chartManaCurve.YAxis1 = Nothing
        Me.chartManaCurve.YAxis2 = Nothing
        '
        'cmnuChart
        '
        Me.cmnuChart.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmnuBreakDown, Me.cmnuCurve})
        Me.cmnuChart.Name = "cmnuChart"
        Me.cmnuChart.Size = New System.Drawing.Size(137, 48)
        '
        'cmnuBreakDown
        '
        Me.cmnuBreakDown.Checked = true
        Me.cmnuBreakDown.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cmnuBreakDown.Name = "cmnuBreakDown"
        Me.cmnuBreakDown.Size = New System.Drawing.Size(136, 22)
        Me.cmnuBreakDown.Text = "Diagramme"
        AddHandler Me.cmnuBreakDown.Click, AddressOf Me.CmnuBreakDownClick
        '
        'cmnuCurve
        '
        Me.cmnuCurve.Enabled = false
        Me.cmnuCurve.Name = "cmnuCurve"
        Me.cmnuCurve.Size = New System.Drawing.Size(136, 22)
        Me.cmnuCurve.Text = "Mana curve"
        AddHandler Me.cmnuCurve.Click, AddressOf Me.CmnuCurveClick
        '
        'chartBreakDown
        '
        Me.chartBreakDown.BackColor = System.Drawing.Color.Transparent
        Me.chartBreakDown.Chart3D = true
        Me.chartBreakDown.ContextMenuStrip = Me.cmnuChart
        Me.chartBreakDown.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chartBreakDown.Font = New System.Drawing.Font("Arial", 7!)
        Me.chartBreakDown.Gallery = SoftwareFX.ChartFX.Lite.Gallery.Pie
        Me.chartBreakDown.Location = New System.Drawing.Point(0, 0)
        Me.chartBreakDown.Name = "chartBreakDown"
        Me.chartBreakDown.NSeries = 1
        Me.chartBreakDown.NValues = 1
        Me.chartBreakDown.Size = New System.Drawing.Size(575, 169)
        Me.chartBreakDown.TabIndex = 1
        '
        'cboCriterion
        '
        Me.cboCriterion.ControlText = "Type"
        Me.cboCriterion.Items.AddRange(New Object() {"Type", "Couleur", "Edition", "Coût d'invocation", "Rareté", "Prix"})
        Me.cboCriterion.MinimumControlWidth = 50
        Me.cboCriterion.MinimumSize = 150
        '
        'cbarInfos
        '
        Me.cbarInfos.Closable = false
        Me.cbarInfos.Controls.Add(Me.pnlInfos)
        Me.cbarInfos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cbarInfos.DrawActionsButton = false
        Me.cbarInfos.Flow = TD.SandBar.ToolBarLayout.Horizontal
        Me.cbarInfos.Guid = New System.Guid("c298d149-b8ec-4c9d-a8c7-dd350bf6317b")
        Me.cbarInfos.Location = New System.Drawing.Point(0, 0)
        Me.cbarInfos.Movable = false
        Me.cbarInfos.Name = "cbarInfos"
        Me.cbarInfos.Size = New System.Drawing.Size(579, 191)
        Me.cbarInfos.TabIndex = 1
        Me.cbarInfos.Text = "Informations"
        '
        'pnlInfos
        '
        Me.pnlInfos.Controls.Add(Me.splitV)
        Me.pnlInfos.Location = New System.Drawing.Point(2, 27)
        Me.pnlInfos.Name = "pnlInfos"
        Me.pnlInfos.Size = New System.Drawing.Size(575, 162)
        Me.pnlInfos.TabIndex = 0
        '
        'splitV
        '
        Me.splitV.Dock = System.Windows.Forms.DockStyle.Fill
        Me.splitV.Location = New System.Drawing.Point(0, 0)
        Me.splitV.Name = "splitV"
        '
        'splitV.Panel1
        '
        Me.splitV.Panel1.Controls.Add(Me.tabInfos)
        '
        'splitV.Panel2
        '
        Me.splitV.Panel2.Controls.Add(Me.grdDetails)
        Me.splitV.Size = New System.Drawing.Size(575, 162)
        Me.splitV.SplitterDistance = 356
        Me.splitV.TabIndex = 0
        '
        'tabInfos
        '
        Me.tabInfos.Controls.Add(Me.tabGeneral)
        Me.tabInfos.Controls.Add(Me.tabPrix)
        Me.tabInfos.Controls.Add(Me.tabEfficacite)
        Me.tabInfos.Controls.Add(Me.tabCreatures)
        Me.tabInfos.Controls.Add(Me.tabAutorisations)
        Me.tabInfos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabInfos.Location = New System.Drawing.Point(0, 0)
        Me.tabInfos.Name = "tabInfos"
        Me.tabInfos.SelectedIndex = 0
        Me.tabInfos.Size = New System.Drawing.Size(356, 162)
        Me.tabInfos.TabIndex = 25
        '
        'tabGeneral
        '
        Me.tabGeneral.Controls.Add(Me.grpGeneral)
        Me.tabGeneral.Location = New System.Drawing.Point(4, 22)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGeneral.Size = New System.Drawing.Size(348, 136)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "Général"
        Me.tabGeneral.UseVisualStyleBackColor = true
        '
        'grpGeneral
        '
        Me.grpGeneral.Controls.Add(Me.txtOldest)
        Me.grpGeneral.Controls.Add(Me.txtRarest)
        Me.grpGeneral.Controls.Add(Me.txtTougher)
        Me.grpGeneral.Controls.Add(Me.txtNCartes)
        Me.grpGeneral.Controls.Add(Me.lblOldest)
        Me.grpGeneral.Controls.Add(Me.lblRarest)
        Me.grpGeneral.Controls.Add(Me.lblTougher)
        Me.grpGeneral.Controls.Add(Me.lblNCartes)
        Me.grpGeneral.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpGeneral.Location = New System.Drawing.Point(3, 3)
        Me.grpGeneral.Name = "grpGeneral"
        Me.grpGeneral.Size = New System.Drawing.Size(342, 130)
        Me.grpGeneral.TabIndex = 24
        Me.grpGeneral.TabStop = false
        '
        'txtOldest
        '
        Me.txtOldest.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.txtOldest.Location = New System.Drawing.Point(150, 98)
        Me.txtOldest.Name = "txtOldest"
        Me.txtOldest.ReadOnly = true
        Me.txtOldest.Size = New System.Drawing.Size(140, 20)
        Me.txtOldest.TabIndex = 16
        Me.txtOldest.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtRarest
        '
        Me.txtRarest.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.txtRarest.Location = New System.Drawing.Point(150, 72)
        Me.txtRarest.Name = "txtRarest"
        Me.txtRarest.ReadOnly = true
        Me.txtRarest.Size = New System.Drawing.Size(140, 20)
        Me.txtRarest.TabIndex = 15
        Me.txtRarest.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtTougher
        '
        Me.txtTougher.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.txtTougher.Location = New System.Drawing.Point(150, 46)
        Me.txtTougher.Name = "txtTougher"
        Me.txtTougher.ReadOnly = true
        Me.txtTougher.Size = New System.Drawing.Size(140, 20)
        Me.txtTougher.TabIndex = 14
        Me.txtTougher.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtNCartes
        '
        Me.txtNCartes.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.txtNCartes.Location = New System.Drawing.Point(150, 20)
        Me.txtNCartes.Name = "txtNCartes"
        Me.txtNCartes.ReadOnly = true
        Me.txtNCartes.Size = New System.Drawing.Size(140, 20)
        Me.txtNCartes.TabIndex = 13
        Me.txtNCartes.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblOldest
        '
        Me.lblOldest.AutoSize = true
        Me.lblOldest.BackColor = System.Drawing.Color.Transparent
        Me.lblOldest.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.lblOldest.Location = New System.Drawing.Point(3, 101)
        Me.lblOldest.Name = "lblOldest"
        Me.lblOldest.Size = New System.Drawing.Size(112, 13)
        Me.lblOldest.TabIndex = 12
        Me.lblOldest.Text = "Carte la plus ancienne"
        '
        'lblRarest
        '
        Me.lblRarest.AutoSize = true
        Me.lblRarest.BackColor = System.Drawing.Color.Transparent
        Me.lblRarest.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.lblRarest.Location = New System.Drawing.Point(3, 75)
        Me.lblRarest.Name = "lblRarest"
        Me.lblRarest.Size = New System.Drawing.Size(86, 13)
        Me.lblRarest.TabIndex = 11
        Me.lblRarest.Text = "Carte la plus rare"
        '
        'lblTougher
        '
        Me.lblTougher.AutoSize = true
        Me.lblTougher.BackColor = System.Drawing.Color.Transparent
        Me.lblTougher.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.lblTougher.Location = New System.Drawing.Point(3, 49)
        Me.lblTougher.Name = "lblTougher"
        Me.lblTougher.Size = New System.Drawing.Size(128, 13)
        Me.lblTougher.TabIndex = 10
        Me.lblTougher.Text = "Créature la plus puissante"
        '
        'lblNCartes
        '
        Me.lblNCartes.AutoSize = true
        Me.lblNCartes.BackColor = System.Drawing.Color.Transparent
        Me.lblNCartes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.lblNCartes.Location = New System.Drawing.Point(3, 23)
        Me.lblNCartes.Name = "lblNCartes"
        Me.lblNCartes.Size = New System.Drawing.Size(91, 13)
        Me.lblNCartes.TabIndex = 9
        Me.lblNCartes.Text = "Nombre de cartes"
        '
        'tabPrix
        '
        Me.tabPrix.Controls.Add(Me.grpPrix)
        Me.tabPrix.Location = New System.Drawing.Point(4, 22)
        Me.tabPrix.Name = "tabPrix"
        Me.tabPrix.Padding = New System.Windows.Forms.Padding(3)
        Me.tabPrix.Size = New System.Drawing.Size(348, 136)
        Me.tabPrix.TabIndex = 1
        Me.tabPrix.Text = "Prix"
        Me.tabPrix.UseVisualStyleBackColor = true
        '
        'grpPrix
        '
        Me.grpPrix.Controls.Add(Me.cmdHistPrices)
        Me.grpPrix.Controls.Add(Me.txtMeanPrice2)
        Me.grpPrix.Controls.Add(Me.lblMeanPrice2)
        Me.grpPrix.Controls.Add(Me.txtMostExpensive)
        Me.grpPrix.Controls.Add(Me.lblMostExpensive)
        Me.grpPrix.Controls.Add(Me.txtMeanPrice)
        Me.grpPrix.Controls.Add(Me.lblMeanPrice)
        Me.grpPrix.Controls.Add(Me.txtTotPrice)
        Me.grpPrix.Controls.Add(Me.lblTotPrice)
        Me.grpPrix.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpPrix.Location = New System.Drawing.Point(3, 3)
        Me.grpPrix.Name = "grpPrix"
        Me.grpPrix.Size = New System.Drawing.Size(342, 130)
        Me.grpPrix.TabIndex = 0
        Me.grpPrix.TabStop = false
        '
        'cmdHistPrices
        '
        Me.cmdHistPrices.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.cmdHistPrices.BackgroundImage = CType(resources.GetObject("cmdHistPrices.BackgroundImage"),System.Drawing.Image)
        Me.cmdHistPrices.Location = New System.Drawing.Point(119, 17)
        Me.cmdHistPrices.Name = "cmdHistPrices"
        Me.cmdHistPrices.Size = New System.Drawing.Size(25, 25)
        Me.cmdHistPrices.TabIndex = 24
        Me.cmdHistPrices.UseVisualStyleBackColor = true
        AddHandler Me.cmdHistPrices.MouseDown, AddressOf Me.CmdHistPricesMouseDown
        '
        'txtMeanPrice2
        '
        Me.txtMeanPrice2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.txtMeanPrice2.Location = New System.Drawing.Point(150, 71)
        Me.txtMeanPrice2.Name = "txtMeanPrice2"
        Me.txtMeanPrice2.ReadOnly = true
        Me.txtMeanPrice2.Size = New System.Drawing.Size(140, 20)
        Me.txtMeanPrice2.TabIndex = 23
        Me.txtMeanPrice2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblMeanPrice2
        '
        Me.lblMeanPrice2.AutoSize = true
        Me.lblMeanPrice2.BackColor = System.Drawing.Color.Transparent
        Me.lblMeanPrice2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.lblMeanPrice2.Location = New System.Drawing.Point(3, 74)
        Me.lblMeanPrice2.Name = "lblMeanPrice2"
        Me.lblMeanPrice2.Size = New System.Drawing.Size(111, 13)
        Me.lblMeanPrice2.TabIndex = 22
        Me.lblMeanPrice2.Text = "Prix moyen (distinctes)"
        '
        'txtMostExpensive
        '
        Me.txtMostExpensive.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.txtMostExpensive.Location = New System.Drawing.Point(150, 98)
        Me.txtMostExpensive.Name = "txtMostExpensive"
        Me.txtMostExpensive.ReadOnly = true
        Me.txtMostExpensive.Size = New System.Drawing.Size(140, 20)
        Me.txtMostExpensive.TabIndex = 21
        Me.txtMostExpensive.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblMostExpensive
        '
        Me.lblMostExpensive.AutoSize = true
        Me.lblMostExpensive.BackColor = System.Drawing.Color.Transparent
        Me.lblMostExpensive.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.lblMostExpensive.Location = New System.Drawing.Point(3, 101)
        Me.lblMostExpensive.Name = "lblMostExpensive"
        Me.lblMostExpensive.Size = New System.Drawing.Size(49, 13)
        Me.lblMostExpensive.TabIndex = 20
        Me.lblMostExpensive.Text = "Prix max."
        '
        'txtMeanPrice
        '
        Me.txtMeanPrice.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.txtMeanPrice.Location = New System.Drawing.Point(150, 46)
        Me.txtMeanPrice.Name = "txtMeanPrice"
        Me.txtMeanPrice.ReadOnly = true
        Me.txtMeanPrice.Size = New System.Drawing.Size(140, 20)
        Me.txtMeanPrice.TabIndex = 19
        Me.txtMeanPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblMeanPrice
        '
        Me.lblMeanPrice.AutoSize = true
        Me.lblMeanPrice.BackColor = System.Drawing.Color.Transparent
        Me.lblMeanPrice.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.lblMeanPrice.Location = New System.Drawing.Point(3, 49)
        Me.lblMeanPrice.Name = "lblMeanPrice"
        Me.lblMeanPrice.Size = New System.Drawing.Size(58, 13)
        Me.lblMeanPrice.TabIndex = 18
        Me.lblMeanPrice.Text = "Prix moyen"
        '
        'txtTotPrice
        '
        Me.txtTotPrice.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.txtTotPrice.Location = New System.Drawing.Point(150, 20)
        Me.txtTotPrice.Name = "txtTotPrice"
        Me.txtTotPrice.ReadOnly = true
        Me.txtTotPrice.Size = New System.Drawing.Size(140, 20)
        Me.txtTotPrice.TabIndex = 17
        Me.txtTotPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblTotPrice
        '
        Me.lblTotPrice.AutoSize = true
        Me.lblTotPrice.BackColor = System.Drawing.Color.Transparent
        Me.lblTotPrice.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.lblTotPrice.Location = New System.Drawing.Point(3, 23)
        Me.lblTotPrice.Name = "lblTotPrice"
        Me.lblTotPrice.Size = New System.Drawing.Size(118, 13)
        Me.lblTotPrice.TabIndex = 16
        Me.lblTotPrice.Text = "Prix total"
        '
        'tabEfficacite
        '
        Me.tabEfficacite.Controls.Add(Me.grpInvocation)
        Me.tabEfficacite.Location = New System.Drawing.Point(4, 22)
        Me.tabEfficacite.Name = "tabEfficacite"
        Me.tabEfficacite.Padding = New System.Windows.Forms.Padding(3)
        Me.tabEfficacite.Size = New System.Drawing.Size(348, 136)
        Me.tabEfficacite.TabIndex = 2
        Me.tabEfficacite.Text = "Invocations"
        Me.tabEfficacite.UseVisualStyleBackColor = true
        '
        'grpInvocation
        '
        Me.grpInvocation.Controls.Add(Me.txtMeanCost2)
        Me.grpInvocation.Controls.Add(Me.lblMeanCost2)
        Me.grpInvocation.Controls.Add(Me.txtMaxCost)
        Me.grpInvocation.Controls.Add(Me.lblMaxCost)
        Me.grpInvocation.Controls.Add(Me.txtMinCost)
        Me.grpInvocation.Controls.Add(Me.lblMinCost)
        Me.grpInvocation.Controls.Add(Me.txtMeanCost)
        Me.grpInvocation.Controls.Add(Me.lblMeanCost)
        Me.grpInvocation.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpInvocation.Location = New System.Drawing.Point(3, 3)
        Me.grpInvocation.Name = "grpInvocation"
        Me.grpInvocation.Size = New System.Drawing.Size(342, 130)
        Me.grpInvocation.TabIndex = 0
        Me.grpInvocation.TabStop = false
        '
        'txtMeanCost2
        '
        Me.txtMeanCost2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.txtMeanCost2.Location = New System.Drawing.Point(150, 45)
        Me.txtMeanCost2.Name = "txtMeanCost2"
        Me.txtMeanCost2.ReadOnly = true
        Me.txtMeanCost2.Size = New System.Drawing.Size(140, 20)
        Me.txtMeanCost2.TabIndex = 30
        Me.txtMeanCost2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblMeanCost2
        '
        Me.lblMeanCost2.AutoSize = true
        Me.lblMeanCost2.BackColor = System.Drawing.Color.Transparent
        Me.lblMeanCost2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.lblMeanCost2.Location = New System.Drawing.Point(3, 48)
        Me.lblMeanCost2.Name = "lblMeanCost2"
        Me.lblMeanCost2.Size = New System.Drawing.Size(123, 13)
        Me.lblMeanCost2.TabIndex = 29
        Me.lblMeanCost2.Text = "Coût moyen (invocables)"
        '
        'txtMaxCost
        '
        Me.txtMaxCost.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.txtMaxCost.Location = New System.Drawing.Point(150, 98)
        Me.txtMaxCost.Name = "txtMaxCost"
        Me.txtMaxCost.ReadOnly = true
        Me.txtMaxCost.Size = New System.Drawing.Size(140, 20)
        Me.txtMaxCost.TabIndex = 28
        Me.txtMaxCost.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblMaxCost
        '
        Me.lblMaxCost.AutoSize = true
        Me.lblMaxCost.BackColor = System.Drawing.Color.Transparent
        Me.lblMaxCost.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.lblMaxCost.Location = New System.Drawing.Point(3, 101)
        Me.lblMaxCost.Name = "lblMaxCost"
        Me.lblMaxCost.Size = New System.Drawing.Size(54, 13)
        Me.lblMaxCost.TabIndex = 27
        Me.lblMaxCost.Text = "Coût max."
        '
        'txtMinCost
        '
        Me.txtMinCost.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.txtMinCost.Location = New System.Drawing.Point(150, 72)
        Me.txtMinCost.Name = "txtMinCost"
        Me.txtMinCost.ReadOnly = true
        Me.txtMinCost.Size = New System.Drawing.Size(140, 20)
        Me.txtMinCost.TabIndex = 26
        Me.txtMinCost.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblMinCost
        '
        Me.lblMinCost.AutoSize = true
        Me.lblMinCost.BackColor = System.Drawing.Color.Transparent
        Me.lblMinCost.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.lblMinCost.Location = New System.Drawing.Point(3, 75)
        Me.lblMinCost.Name = "lblMinCost"
        Me.lblMinCost.Size = New System.Drawing.Size(111, 13)
        Me.lblMinCost.TabIndex = 25
        Me.lblMinCost.Text = "Coût min. (invocables)"
        '
        'txtMeanCost
        '
        Me.txtMeanCost.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.txtMeanCost.Location = New System.Drawing.Point(150, 20)
        Me.txtMeanCost.Name = "txtMeanCost"
        Me.txtMeanCost.ReadOnly = true
        Me.txtMeanCost.Size = New System.Drawing.Size(140, 20)
        Me.txtMeanCost.TabIndex = 24
        Me.txtMeanCost.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblMeanCost
        '
        Me.lblMeanCost.AutoSize = true
        Me.lblMeanCost.BackColor = System.Drawing.Color.Transparent
        Me.lblMeanCost.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.lblMeanCost.Location = New System.Drawing.Point(3, 23)
        Me.lblMeanCost.Name = "lblMeanCost"
        Me.lblMeanCost.Size = New System.Drawing.Size(63, 13)
        Me.lblMeanCost.TabIndex = 23
        Me.lblMeanCost.Text = "Coût moyen"
        '
        'tabCreatures
        '
        Me.tabCreatures.Controls.Add(Me.grpCreatures)
        Me.tabCreatures.Location = New System.Drawing.Point(4, 22)
        Me.tabCreatures.Name = "tabCreatures"
        Me.tabCreatures.Padding = New System.Windows.Forms.Padding(3)
        Me.tabCreatures.Size = New System.Drawing.Size(348, 136)
        Me.tabCreatures.TabIndex = 3
        Me.tabCreatures.Text = "Créatures"
        Me.tabCreatures.UseVisualStyleBackColor = true
        '
        'grpCreatures
        '
        Me.grpCreatures.Controls.Add(Me.txtMeanTough)
        Me.grpCreatures.Controls.Add(Me.lblMeanTough)
        Me.grpCreatures.Controls.Add(Me.txtRAC)
        Me.grpCreatures.Controls.Add(Me.lblRAC)
        Me.grpCreatures.Controls.Add(Me.txtRAD)
        Me.grpCreatures.Controls.Add(Me.lblRAD)
        Me.grpCreatures.Controls.Add(Me.txtMeanPower)
        Me.grpCreatures.Controls.Add(Me.lblMeanPower)
        Me.grpCreatures.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpCreatures.Location = New System.Drawing.Point(3, 3)
        Me.grpCreatures.Name = "grpCreatures"
        Me.grpCreatures.Size = New System.Drawing.Size(342, 130)
        Me.grpCreatures.TabIndex = 2
        Me.grpCreatures.TabStop = false
        '
        'txtMeanTough
        '
        Me.txtMeanTough.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.txtMeanTough.Location = New System.Drawing.Point(150, 45)
        Me.txtMeanTough.Name = "txtMeanTough"
        Me.txtMeanTough.ReadOnly = true
        Me.txtMeanTough.Size = New System.Drawing.Size(140, 20)
        Me.txtMeanTough.TabIndex = 30
        Me.txtMeanTough.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblMeanTough
        '
        Me.lblMeanTough.AutoSize = true
        Me.lblMeanTough.BackColor = System.Drawing.Color.Transparent
        Me.lblMeanTough.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.lblMeanTough.Location = New System.Drawing.Point(3, 48)
        Me.lblMeanTough.Name = "lblMeanTough"
        Me.lblMeanTough.Size = New System.Drawing.Size(93, 13)
        Me.lblMeanTough.TabIndex = 29
        Me.lblMeanTough.Text = "Défense moyenne"
        '
        'txtRAC
        '
        Me.txtRAC.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.txtRAC.Location = New System.Drawing.Point(150, 98)
        Me.txtRAC.Name = "txtRAC"
        Me.txtRAC.ReadOnly = true
        Me.txtRAC.Size = New System.Drawing.Size(140, 20)
        Me.txtRAC.TabIndex = 28
        Me.txtRAC.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblRAC
        '
        Me.lblRAC.AutoSize = true
        Me.lblRAC.BackColor = System.Drawing.Color.Transparent
        Me.lblRAC.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.lblRAC.Location = New System.Drawing.Point(3, 101)
        Me.lblRAC.Name = "lblRAC"
        Me.lblRAC.Size = New System.Drawing.Size(103, 13)
        Me.lblRAC.TabIndex = 27
        Me.lblRAC.Text = "Ratio attaque / coût"
        '
        'txtRAD
        '
        Me.txtRAD.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.txtRAD.Location = New System.Drawing.Point(150, 72)
        Me.txtRAD.Name = "txtRAD"
        Me.txtRAD.ReadOnly = true
        Me.txtRAD.Size = New System.Drawing.Size(140, 20)
        Me.txtRAD.TabIndex = 26
        Me.txtRAD.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblRAD
        '
        Me.lblRAD.AutoSize = true
        Me.lblRAD.BackColor = System.Drawing.Color.Transparent
        Me.lblRAD.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.lblRAD.Location = New System.Drawing.Point(3, 75)
        Me.lblRAD.Name = "lblRAD"
        Me.lblRAD.Size = New System.Drawing.Size(120, 13)
        Me.lblRAD.TabIndex = 25
        Me.lblRAD.Text = "Ratio attaque / défense"
        '
        'txtMeanPower
        '
        Me.txtMeanPower.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.txtMeanPower.Location = New System.Drawing.Point(150, 20)
        Me.txtMeanPower.Name = "txtMeanPower"
        Me.txtMeanPower.ReadOnly = true
        Me.txtMeanPower.Size = New System.Drawing.Size(140, 20)
        Me.txtMeanPower.TabIndex = 24
        Me.txtMeanPower.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblMeanPower
        '
        Me.lblMeanPower.AutoSize = true
        Me.lblMeanPower.BackColor = System.Drawing.Color.Transparent
        Me.lblMeanPower.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.lblMeanPower.Location = New System.Drawing.Point(3, 23)
        Me.lblMeanPower.Name = "lblMeanPower"
        Me.lblMeanPower.Size = New System.Drawing.Size(90, 13)
        Me.lblMeanPower.TabIndex = 23
        Me.lblMeanPower.Text = "Attaque moyenne"
        '
        'tabAutorisations
        '
        Me.tabAutorisations.Controls.Add(Me.lstTournoiForbid)
        Me.tabAutorisations.Controls.Add(Me.grpAutorisations)
        Me.tabAutorisations.Controls.Add(Me.lblAutorisations)
        Me.tabAutorisations.Location = New System.Drawing.Point(4, 22)
        Me.tabAutorisations.Name = "tabAutorisations"
        Me.tabAutorisations.Padding = New System.Windows.Forms.Padding(3)
        Me.tabAutorisations.Size = New System.Drawing.Size(348, 136)
        Me.tabAutorisations.TabIndex = 4
        Me.tabAutorisations.Text = "Autorisations"
        Me.tabAutorisations.UseVisualStyleBackColor = true
        '
        'lstTournoiForbid
        '
        Me.lstTournoiForbid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstTournoiForbid.FormattingEnabled = true
        Me.lstTournoiForbid.Location = New System.Drawing.Point(3, 55)
        Me.lstTournoiForbid.MultiColumn = true
        Me.lstTournoiForbid.Name = "lstTournoiForbid"
        Me.lstTournoiForbid.Size = New System.Drawing.Size(342, 78)
        Me.lstTournoiForbid.TabIndex = 16
        '
        'grpAutorisations
        '
        Me.grpAutorisations.Controls.Add(Me.picAutMulti)
        Me.grpAutorisations.Controls.Add(Me.picAutM)
        Me.grpAutorisations.Controls.Add(Me.picAutT1)
        Me.grpAutorisations.Controls.Add(Me.picAutT15)
        Me.grpAutorisations.Controls.Add(Me.picAutT2)
        Me.grpAutorisations.Controls.Add(Me.picAutBloc)
        Me.grpAutorisations.Controls.Add(Me.picAut1V1)
        Me.grpAutorisations.Dock = System.Windows.Forms.DockStyle.Top
        Me.grpAutorisations.Location = New System.Drawing.Point(3, 16)
        Me.grpAutorisations.Name = "grpAutorisations"
        Me.grpAutorisations.Size = New System.Drawing.Size(342, 39)
        Me.grpAutorisations.TabIndex = 15
        Me.grpAutorisations.TabStop = false
        '
        'picAutMulti
        '
        Me.picAutMulti.Location = New System.Drawing.Point(276, 10)
        Me.picAutMulti.Name = "picAutMulti"
        Me.picAutMulti.Size = New System.Drawing.Size(35, 25)
        Me.picAutMulti.TabIndex = 6
        Me.picAutMulti.TabStop = false
        Me.picAutMulti.Tag = "Multi"
        AddHandler Me.picAutMulti.Click, AddressOf Me.PicAutClick
        '
        'picAutM
        '
        Me.picAutM.Location = New System.Drawing.Point(112, 10)
        Me.picAutM.Name = "picAutM"
        Me.picAutM.Size = New System.Drawing.Size(35, 25)
        Me.picAutM.TabIndex = 5
        Me.picAutM.TabStop = false
        Me.picAutM.Tag = "M"
        AddHandler Me.picAutM.Click, AddressOf Me.PicAutClick
        '
        'picAutT1
        '
        Me.picAutT1.Location = New System.Drawing.Point(30, 10)
        Me.picAutT1.Name = "picAutT1"
        Me.picAutT1.Size = New System.Drawing.Size(35, 25)
        Me.picAutT1.TabIndex = 4
        Me.picAutT1.TabStop = false
        Me.picAutT1.Tag = "T1"
        AddHandler Me.picAutT1.Click, AddressOf Me.PicAutClick
        '
        'picAutT15
        '
        Me.picAutT15.Location = New System.Drawing.Point(71, 10)
        Me.picAutT15.Name = "picAutT15"
        Me.picAutT15.Size = New System.Drawing.Size(35, 25)
        Me.picAutT15.TabIndex = 3
        Me.picAutT15.TabStop = false
        Me.picAutT15.Tag = "T15"
        AddHandler Me.picAutT15.Click, AddressOf Me.PicAutClick
        '
        'picAutT2
        '
        Me.picAutT2.Location = New System.Drawing.Point(153, 10)
        Me.picAutT2.Name = "picAutT2"
        Me.picAutT2.Size = New System.Drawing.Size(35, 25)
        Me.picAutT2.TabIndex = 2
        Me.picAutT2.TabStop = false
        Me.picAutT2.Tag = "T2"
        AddHandler Me.picAutT2.Click, AddressOf Me.PicAutClick
        '
        'picAutBloc
        '
        Me.picAutBloc.Location = New System.Drawing.Point(194, 10)
        Me.picAutBloc.Name = "picAutBloc"
        Me.picAutBloc.Size = New System.Drawing.Size(35, 25)
        Me.picAutBloc.TabIndex = 1
        Me.picAutBloc.TabStop = false
        Me.picAutBloc.Tag = "Bloc"
        AddHandler Me.picAutBloc.Click, AddressOf Me.PicAutClick
        '
        'picAut1V1
        '
        Me.picAut1V1.Location = New System.Drawing.Point(235, 10)
        Me.picAut1V1.Name = "picAut1V1"
        Me.picAut1V1.Size = New System.Drawing.Size(35, 25)
        Me.picAut1V1.TabIndex = 0
        Me.picAut1V1.TabStop = false
        Me.picAut1V1.Tag = "[1V1]"
        AddHandler Me.picAut1V1.Click, AddressOf Me.PicAutClick
        '
        'lblAutorisations
        '
        Me.lblAutorisations.AutoSize = true
        Me.lblAutorisations.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblAutorisations.Location = New System.Drawing.Point(3, 3)
        Me.lblAutorisations.Name = "lblAutorisations"
        Me.lblAutorisations.Size = New System.Drawing.Size(274, 13)
        Me.lblAutorisations.TabIndex = 14
        Me.lblAutorisations.Text = "Cliquez sur un type pour la liste des cartes incompatibles."
        '
        'grdDetails
        '
        Me.grdDetails.AutoSizeMinHeight = 10
        Me.grdDetails.AutoSizeMinWidth = 10
        Me.grdDetails.AutoStretchColumnsToFitWidth = false
        Me.grdDetails.AutoStretchRowsToFitHeight = false
        Me.grdDetails.ContextMenuStyle = SourceGrid2.ContextMenuStyle.None
        Me.grdDetails.CustomSort = false
        Me.grdDetails.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grdDetails.GridToolTipActive = true
        Me.grdDetails.Location = New System.Drawing.Point(0, 0)
        Me.grdDetails.Name = "grdDetails"
        Me.grdDetails.Size = New System.Drawing.Size(215, 162)
        Me.grdDetails.SpecialKeys = CType(((((((((((SourceGrid2.GridSpecialKeys.Ctrl_C Or SourceGrid2.GridSpecialKeys.Ctrl_V)  _
                        Or SourceGrid2.GridSpecialKeys.Ctrl_X)  _
                        Or SourceGrid2.GridSpecialKeys.Delete)  _
                        Or SourceGrid2.GridSpecialKeys.Arrows)  _
                        Or SourceGrid2.GridSpecialKeys.Tab)  _
                        Or SourceGrid2.GridSpecialKeys.PageDownUp)  _
                        Or SourceGrid2.GridSpecialKeys.Enter)  _
                        Or SourceGrid2.GridSpecialKeys.Escape)  _
                        Or SourceGrid2.GridSpecialKeys.Control)  _
                        Or SourceGrid2.GridSpecialKeys.Shift),SourceGrid2.GridSpecialKeys)
        Me.grdDetails.TabIndex = 0
        '
        'cmnuHisto
        '
        Me.cmnuHisto.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmnuHistDeck, Me.cmnuHistCards})
        Me.cmnuHisto.Name = "cmnuChart"
        Me.cmnuHisto.Size = New System.Drawing.Size(242, 48)
        '
        'cmnuHistDeck
        '
        Me.cmnuHistDeck.Name = "cmnuHistDeck"
        Me.cmnuHistDeck.Size = New System.Drawing.Size(241, 22)
        Me.cmnuHistDeck.Text = "Evolution du prix de la sélection"
        AddHandler Me.cmnuHistDeck.Click, AddressOf Me.CmnuHistDeckClick
        '
        'cmnuHistCards
        '
        Me.cmnuHistCards.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmnuHistAllCards, Me.toolStripMenuItem1, Me.cmnuHistCardsPrice1, Me.cmnuHistCardsPrice2, Me.cmnuHistCardsPrice3, Me.cmnuHistCardsPrice4, Me.cmnuHistCardsPrice5, Me.cmnuHistCardsPrice6, Me.cmnuHistCardsPrice7, Me.cmnuHistCardsPrice8})
        Me.cmnuHistCards.Name = "cmnuHistCards"
        Me.cmnuHistCards.Size = New System.Drawing.Size(241, 22)
        Me.cmnuHistCards.Text = "Evolution du prix des cartes..."
        '
        'cmnuHistAllCards
        '
        Me.cmnuHistAllCards.Name = "cmnuHistAllCards"
        Me.cmnuHistAllCards.Size = New System.Drawing.Size(159, 22)
        Me.cmnuHistAllCards.Text = "Toutes"
        AddHandler Me.cmnuHistAllCards.Click, AddressOf Me.CmnuHistAllCardsClick
        '
        'toolStripMenuItem1
        '
        Me.toolStripMenuItem1.Name = "toolStripMenuItem1"
        Me.toolStripMenuItem1.Size = New System.Drawing.Size(156, 6)
        '
        'cmnuHistCardsPrice1
        '
        Me.cmnuHistCardsPrice1.Name = "cmnuHistCardsPrice1"
        Me.cmnuHistCardsPrice1.Size = New System.Drawing.Size(159, 22)
        Me.cmnuHistCardsPrice1.Tag = "1"
        Me.cmnuHistCardsPrice1.Text = "Moins de 0,50€"
        AddHandler Me.cmnuHistCardsPrice1.Click, AddressOf Me.CmnuHistCardsPriceClick
        '
        'cmnuHistCardsPrice2
        '
        Me.cmnuHistCardsPrice2.Name = "cmnuHistCardsPrice2"
        Me.cmnuHistCardsPrice2.Size = New System.Drawing.Size(159, 22)
        Me.cmnuHistCardsPrice2.Tag = "2"
        Me.cmnuHistCardsPrice2.Text = "Entre 0,50€ et 1€"
        AddHandler Me.cmnuHistCardsPrice2.Click, AddressOf Me.CmnuHistCardsPriceClick
        '
        'cmnuHistCardsPrice3
        '
        Me.cmnuHistCardsPrice3.Name = "cmnuHistCardsPrice3"
        Me.cmnuHistCardsPrice3.Size = New System.Drawing.Size(159, 22)
        Me.cmnuHistCardsPrice3.Tag = "3"
        Me.cmnuHistCardsPrice3.Text = "Entre 1€ et 3€"
        AddHandler Me.cmnuHistCardsPrice3.Click, AddressOf Me.CmnuHistCardsPriceClick
        '
        'cmnuHistCardsPrice4
        '
        Me.cmnuHistCardsPrice4.Name = "cmnuHistCardsPrice4"
        Me.cmnuHistCardsPrice4.Size = New System.Drawing.Size(159, 22)
        Me.cmnuHistCardsPrice4.Tag = "4"
        Me.cmnuHistCardsPrice4.Text = "Entre 3€ et 5€"
        AddHandler Me.cmnuHistCardsPrice4.Click, AddressOf Me.CmnuHistCardsPriceClick
        '
        'cmnuHistCardsPrice5
        '
        Me.cmnuHistCardsPrice5.Name = "cmnuHistCardsPrice5"
        Me.cmnuHistCardsPrice5.Size = New System.Drawing.Size(159, 22)
        Me.cmnuHistCardsPrice5.Tag = "5"
        Me.cmnuHistCardsPrice5.Text = "Entre 5€ et 10€"
        AddHandler Me.cmnuHistCardsPrice5.Click, AddressOf Me.CmnuHistCardsPriceClick
        '
        'cmnuHistCardsPrice6
        '
        Me.cmnuHistCardsPrice6.Name = "cmnuHistCardsPrice6"
        Me.cmnuHistCardsPrice6.Size = New System.Drawing.Size(159, 22)
        Me.cmnuHistCardsPrice6.Tag = "6"
        Me.cmnuHistCardsPrice6.Text = "Entre 10€ et 20€"
        AddHandler Me.cmnuHistCardsPrice6.Click, AddressOf Me.CmnuHistCardsPriceClick
        '
        'cmnuHistCardsPrice7
        '
        Me.cmnuHistCardsPrice7.Name = "cmnuHistCardsPrice7"
        Me.cmnuHistCardsPrice7.Size = New System.Drawing.Size(159, 22)
        Me.cmnuHistCardsPrice7.Tag = "7"
        Me.cmnuHistCardsPrice7.Text = "Entre 20€ et 50€"
        AddHandler Me.cmnuHistCardsPrice7.Click, AddressOf Me.CmnuHistCardsPriceClick
        '
        'cmnuHistCardsPrice8
        '
        Me.cmnuHistCardsPrice8.Name = "cmnuHistCardsPrice8"
        Me.cmnuHistCardsPrice8.Size = New System.Drawing.Size(159, 22)
        Me.cmnuHistCardsPrice8.Tag = "8"
        Me.cmnuHistCardsPrice8.Text = "Plus de 50 €"
        AddHandler Me.cmnuHistCardsPrice8.Click, AddressOf Me.CmnuHistCardsPriceClick
        '
        'imglstAutorisations
        '
        Me.imglstAutorisations.ImageStream = CType(resources.GetObject("imglstAutorisations.ImageStream"),System.Windows.Forms.ImageListStreamer)
        Me.imglstAutorisations.TransparentColor = System.Drawing.Color.Transparent
        Me.imglstAutorisations.Images.SetKeyName(0, "_a1vs1.png")
        Me.imglstAutorisations.Images.SetKeyName(1, "_a1vs1no.png")
        Me.imglstAutorisations.Images.SetKeyName(2, "_aBloc.png")
        Me.imglstAutorisations.Images.SetKeyName(3, "_aBlocno.png")
        Me.imglstAutorisations.Images.SetKeyName(4, "_aMulti.png")
        Me.imglstAutorisations.Images.SetKeyName(5, "_aMultino.png")
        Me.imglstAutorisations.Images.SetKeyName(6, "_aT1.png")
        Me.imglstAutorisations.Images.SetKeyName(7, "_aT1no.png")
        Me.imglstAutorisations.Images.SetKeyName(8, "_aT1r.png")
        Me.imglstAutorisations.Images.SetKeyName(9, "_aT2.png")
        Me.imglstAutorisations.Images.SetKeyName(10, "_aT2no.png")
        Me.imglstAutorisations.Images.SetKeyName(11, "_aT15.png")
        Me.imglstAutorisations.Images.SetKeyName(12, "_aT15no.png")
        Me.imglstAutorisations.Images.SetKeyName(13, "_aTM.png")
        Me.imglstAutorisations.Images.SetKeyName(14, "_aTMno.png")
        '
        'frmStats
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(600, 415)
        Me.Controls.Add(Me.splitH)
        Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
        Me.Name = "frmStats"
        Me.Text = "Statistiques"
        AddHandler Activated, AddressOf Me.FrmStatsActivated
        AddHandler Load, AddressOf Me.FrmStatsLoad
        AddHandler Paint, AddressOf Me.FrmStatsPaint
        Me.splitH.Panel1.ResumeLayout(false)
        Me.splitH.Panel2.ResumeLayout(false)
        Me.splitH.ResumeLayout(false)
        Me.cbarChart.ResumeLayout(false)
        Me.pnlChart.ResumeLayout(false)
        Me.cmnuChart.ResumeLayout(false)
        Me.cbarInfos.ResumeLayout(false)
        Me.pnlInfos.ResumeLayout(false)
        Me.splitV.Panel1.ResumeLayout(false)
        Me.splitV.Panel2.ResumeLayout(false)
        Me.splitV.ResumeLayout(false)
        Me.tabInfos.ResumeLayout(false)
        Me.tabGeneral.ResumeLayout(false)
        Me.grpGeneral.ResumeLayout(false)
        Me.grpGeneral.PerformLayout
        Me.tabPrix.ResumeLayout(false)
        Me.grpPrix.ResumeLayout(false)
        Me.grpPrix.PerformLayout
        Me.tabEfficacite.ResumeLayout(false)
        Me.grpInvocation.ResumeLayout(false)
        Me.grpInvocation.PerformLayout
        Me.tabCreatures.ResumeLayout(false)
        Me.grpCreatures.ResumeLayout(false)
        Me.grpCreatures.PerformLayout
        Me.tabAutorisations.ResumeLayout(false)
        Me.tabAutorisations.PerformLayout
        Me.grpAutorisations.ResumeLayout(false)
        CType(Me.picAutMulti,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.picAutM,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.picAutT1,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.picAutT15,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.picAutT2,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.picAutBloc,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.picAut1V1,System.ComponentModel.ISupportInitialize).EndInit
        Me.cmnuHisto.ResumeLayout(false)
        Me.ResumeLayout(false)
    End Sub
    Private imglstAutorisations As System.Windows.Forms.ImageList
    Private picAutMulti As System.Windows.Forms.PictureBox
    Private picAutM As System.Windows.Forms.PictureBox
    Private chartManaCurve As NPlot.Windows.PlotSurface2D
    Private cmnuHistCardsPrice8 As System.Windows.Forms.ToolStripMenuItem
    Private cmnuHistCardsPrice7 As System.Windows.Forms.ToolStripMenuItem
    Private cmnuHistCardsPrice6 As System.Windows.Forms.ToolStripMenuItem
    Private cmnuHistCardsPrice5 As System.Windows.Forms.ToolStripMenuItem
    Private cmnuHistCardsPrice4 As System.Windows.Forms.ToolStripMenuItem
    Private cmnuHistCardsPrice3 As System.Windows.Forms.ToolStripMenuItem
    Private cmnuHistCardsPrice2 As System.Windows.Forms.ToolStripMenuItem
    Private cmnuHistCardsPrice1 As System.Windows.Forms.ToolStripMenuItem
    Private toolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Private cmnuHistAllCards As System.Windows.Forms.ToolStripMenuItem
    Private cmnuHistCards As System.Windows.Forms.ToolStripMenuItem
    Private cmnuHistDeck As System.Windows.Forms.ToolStripMenuItem
    Private cmnuHisto As System.Windows.Forms.ContextMenuStrip
    Private cmdHistPrices As System.Windows.Forms.Button
    Private lstTournoiForbid As System.Windows.Forms.ListBox
    Private lblAutorisations As System.Windows.Forms.Label
    Private picAut1V1 As System.Windows.Forms.PictureBox
    Private picAutBloc As System.Windows.Forms.PictureBox
    Private picAutT2 As System.Windows.Forms.PictureBox
    Private picAutT15 As System.Windows.Forms.PictureBox
    Private picAutT1 As System.Windows.Forms.PictureBox
    Private grpAutorisations As System.Windows.Forms.GroupBox
    Private tabAutorisations As System.Windows.Forms.TabPage
    Private cmnuCurve As System.Windows.Forms.ToolStripMenuItem
    Private cmnuBreakDown As System.Windows.Forms.ToolStripMenuItem
    Private cmnuChart As System.Windows.Forms.ContextMenuStrip
    Private lblMeanPower As System.Windows.Forms.Label
    Private txtMeanPower As System.Windows.Forms.TextBox
    Private lblRAD As System.Windows.Forms.Label
    Private txtRAD As System.Windows.Forms.TextBox
    Private lblRAC As System.Windows.Forms.Label
    Private txtRAC As System.Windows.Forms.TextBox
    Private lblMeanTough As System.Windows.Forms.Label
    Private txtMeanTough As System.Windows.Forms.TextBox
    Private grpCreatures As System.Windows.Forms.GroupBox
    Private tabCreatures As System.Windows.Forms.TabPage
    Private grpInvocation As System.Windows.Forms.GroupBox
    Private lblMeanCost2 As System.Windows.Forms.Label
    Private txtMeanCost2 As System.Windows.Forms.TextBox
    Private lblMeanPrice2 As System.Windows.Forms.Label
    Private txtMeanPrice2 As System.Windows.Forms.TextBox
    Private pnlInfos As TD.SandBar.ContainerBarClientPanel
    Private splitV As System.Windows.Forms.SplitContainer
    Private tabGeneral As System.Windows.Forms.TabPage
    Private tabPrix As System.Windows.Forms.TabPage
    Private tabEfficacite As System.Windows.Forms.TabPage
    Private cbarInfos As TD.SandBar.ContainerBar
    Private tabInfos As System.Windows.Forms.TabControl
    Private grpGeneral As System.Windows.Forms.GroupBox
    Private txtOldest As System.Windows.Forms.TextBox
    Private txtRarest As System.Windows.Forms.TextBox
    Private txtTougher As System.Windows.Forms.TextBox
    Private txtNCartes As System.Windows.Forms.TextBox
    Private lblOldest As System.Windows.Forms.Label
    Private lblRarest As System.Windows.Forms.Label
    Private lblTougher As System.Windows.Forms.Label
    Private lblNCartes As System.Windows.Forms.Label
    Private grpPrix As System.Windows.Forms.GroupBox
    Private txtMostExpensive As System.Windows.Forms.TextBox
    Private lblMostExpensive As System.Windows.Forms.Label
    Private txtMeanPrice As System.Windows.Forms.TextBox
    Private lblMeanPrice As System.Windows.Forms.Label
    Private txtTotPrice As System.Windows.Forms.TextBox
    Private lblTotPrice As System.Windows.Forms.Label
    Private txtMaxCost As System.Windows.Forms.TextBox
    Private lblMaxCost As System.Windows.Forms.Label
    Private txtMinCost As System.Windows.Forms.TextBox
    Private lblMinCost As System.Windows.Forms.Label
    Private txtMeanCost As System.Windows.Forms.TextBox
    Private lblMeanCost As System.Windows.Forms.Label
    Private grdDetails As SourceGrid2.Grid
    Private chartBreakDown As SoftwareFX.ChartFX.Lite.Chart
    Private cboCriterion As TD.SandBar.ComboBoxItem
    Private pnlChart As TD.SandBar.ContainerBarClientPanel
    Private cbarChart As TD.SandBar.ContainerBar
    Private splitH As System.Windows.Forms.SplitContainer
End Class
