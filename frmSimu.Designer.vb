'
' Created by SharpDevelop.
' User: Couitchy
' Date: 29/10/2008
' Time: 21:35
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Partial Class frmSimu
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSimu))
		Me.cbarSimus = New TD.SandBar.ContainerBar
		Me.pnlSimus = New TD.SandBar.ContainerBarClientPanel
		Me.grpCombos = New System.Windows.Forms.GroupBox
		Me.splitCombosV = New System.Windows.Forms.SplitContainer
		Me.splitCombosH1 = New System.Windows.Forms.SplitContainer
		Me.tabControl = New System.Windows.Forms.TabControl
		Me.tabCarte = New System.Windows.Forms.TabPage
		Me.grdCardsDispos = New SourceGrid2.Grid
		Me.tabType = New System.Windows.Forms.TabPage
		Me.grdTypesDispos = New SourceGrid2.Grid
		Me.tabSubType = New System.Windows.Forms.TabPage
		Me.grdSubTypesDispos = New SourceGrid2.Grid
		Me.tabCost = New System.Windows.Forms.TabPage
		Me.grdCostsDispos = New SourceGrid2.Grid
		Me.tabColor = New System.Windows.Forms.TabPage
		Me.grdColorsDispos = New SourceGrid2.Grid
		Me.toolStripCombos = New System.Windows.Forms.ToolStrip
		Me.btAddSequence = New System.Windows.Forms.ToolStripButton
		Me.btRemove = New System.Windows.Forms.ToolStripButton
		Me.btClearAll = New System.Windows.Forms.ToolStripButton
		Me.chklstSequencesDispos = New System.Windows.Forms.CheckedListBox
		Me.splitCombosH2 = New System.Windows.Forms.SplitContainer
		Me.picScanCard2 = New System.Windows.Forms.PictureBox
		Me.prgSimu = New System.Windows.Forms.ProgressBar
		Me.toolStripCombos2 = New System.Windows.Forms.ToolStrip
		Me.txtN = New System.Windows.Forms.ToolStripTextBox
		Me.lbl3 = New System.Windows.Forms.ToolStripLabel
		Me.btSeparator = New System.Windows.Forms.ToolStripSeparator
		Me.btSimu = New System.Windows.Forms.ToolStripButton
		Me.btAddPlot = New System.Windows.Forms.ToolStripButton
		Me.lbl8 = New System.Windows.Forms.Label
		Me.txtEspCumul = New System.Windows.Forms.TextBox
		Me.cboTourCumul = New System.Windows.Forms.ComboBox
		Me.lbl4 = New System.Windows.Forms.Label
		Me.lbl1 = New System.Windows.Forms.Label
		Me.grpSuggest = New System.Windows.Forms.GroupBox
		Me.cmdCorrExpr = New System.Windows.Forms.Button
		Me.sldPertin = New System.Windows.Forms.TrackBar
		Me.lbl17 = New System.Windows.Forms.Label
		Me.txtPrix = New System.Windows.Forms.TextBox
		Me.txtEditions = New System.Windows.Forms.TextBox
		Me.txtInvoc = New System.Windows.Forms.TextBox
		Me.cmdSuggest = New System.Windows.Forms.Button
		Me.txtColors = New System.Windows.Forms.TextBox
		Me.chkPrix = New System.Windows.Forms.CheckBox
		Me.chkEditions = New System.Windows.Forms.CheckBox
		Me.chkInvoc = New System.Windows.Forms.CheckBox
		Me.chkColors = New System.Windows.Forms.CheckBox
		Me.lbl16 = New System.Windows.Forms.Label
		Me.lbl15 = New System.Windows.Forms.Label
		Me.cmdDetect = New System.Windows.Forms.Button
		Me.prgSuggest = New System.Windows.Forms.ProgressBar
		Me.grpDeploy = New System.Windows.Forms.GroupBox
		Me.splitDeployH = New System.Windows.Forms.SplitContainer
		Me.splitDeployV = New System.Windows.Forms.SplitContainer
		Me.lstUserCombos = New System.Windows.Forms.CheckedListBox
		Me.lbl12 = New System.Windows.Forms.Label
		Me.lbl14 = New System.Windows.Forms.Label
		Me.lbl13 = New System.Windows.Forms.Label
		Me.txtEspManas = New System.Windows.Forms.TextBox
		Me.cboTourDeploy = New System.Windows.Forms.ComboBox
		Me.chkDefaut = New System.Windows.Forms.CheckBox
		Me.txtEspDefaut = New System.Windows.Forms.TextBox
		Me.chkVerbosity = New System.Windows.Forms.CheckBox
		Me.cmdAddPlot2 = New System.Windows.Forms.Button
		Me.cboTourDeploy2 = New System.Windows.Forms.ComboBox
		Me.prgSimu2 = New System.Windows.Forms.ProgressBar
		Me.lbl11 = New System.Windows.Forms.Label
		Me.cmdSimu2 = New System.Windows.Forms.Button
		Me.lbl9 = New System.Windows.Forms.Label
		Me.txtN2 = New System.Windows.Forms.TextBox
		Me.lbl10 = New System.Windows.Forms.Label
		Me.btCombos = New TD.SandBar.ButtonItem
		Me.btDeploy = New TD.SandBar.ButtonItem
		Me.btSuggest = New TD.SandBar.ButtonItem
		Me.cmnuUserCombos = New System.Windows.Forms.ContextMenuStrip(Me.components)
		Me.cmnuAddNew = New System.Windows.Forms.ToolStripMenuItem
		Me.cmnuDelete = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuSeparator = New System.Windows.Forms.ToolStripSeparator
		Me.cmnuUp = New System.Windows.Forms.ToolStripMenuItem
		Me.cmnuDown = New System.Windows.Forms.ToolStripMenuItem
		Me.dlgVerbose = New System.Windows.Forms.SaveFileDialog
		Me.dlgSave = New System.Windows.Forms.SaveFileDialog
		Me.dlgOpen = New System.Windows.Forms.OpenFileDialog
		Me.cbarSimus.SuspendLayout
		Me.pnlSimus.SuspendLayout
		Me.grpCombos.SuspendLayout
		Me.splitCombosV.Panel1.SuspendLayout
		Me.splitCombosV.Panel2.SuspendLayout
		Me.splitCombosV.SuspendLayout
		Me.splitCombosH1.Panel1.SuspendLayout
		Me.splitCombosH1.Panel2.SuspendLayout
		Me.splitCombosH1.SuspendLayout
		Me.tabControl.SuspendLayout
		Me.tabCarte.SuspendLayout
		Me.grdCardsDispos.SuspendLayout
		Me.tabType.SuspendLayout
		Me.grdTypesDispos.SuspendLayout
		Me.tabSubType.SuspendLayout
		Me.grdSubTypesDispos.SuspendLayout
		Me.tabCost.SuspendLayout
		Me.grdCostsDispos.SuspendLayout
		Me.tabColor.SuspendLayout
		Me.grdColorsDispos.SuspendLayout
		Me.toolStripCombos.SuspendLayout
		Me.splitCombosH2.Panel1.SuspendLayout
		Me.splitCombosH2.Panel2.SuspendLayout
		Me.splitCombosH2.SuspendLayout
		CType(Me.picScanCard2,System.ComponentModel.ISupportInitialize).BeginInit
		Me.toolStripCombos2.SuspendLayout
		Me.grpSuggest.SuspendLayout
		CType(Me.sldPertin,System.ComponentModel.ISupportInitialize).BeginInit
		Me.grpDeploy.SuspendLayout
		Me.splitDeployH.Panel1.SuspendLayout
		Me.splitDeployH.Panel2.SuspendLayout
		Me.splitDeployH.SuspendLayout
		Me.splitDeployV.Panel1.SuspendLayout
		Me.splitDeployV.Panel2.SuspendLayout
		Me.splitDeployV.SuspendLayout
		Me.cmnuUserCombos.SuspendLayout
		Me.SuspendLayout
		'
		'cbarSimus
		'
		Me.cbarSimus.AddRemoveButtonsVisible = false
		Me.cbarSimus.Closable = false
		Me.cbarSimus.Controls.Add(Me.pnlSimus)
		Me.cbarSimus.Dock = System.Windows.Forms.DockStyle.Fill
		Me.cbarSimus.DrawActionsButton = false
		Me.cbarSimus.Flow = TD.SandBar.ToolBarLayout.Horizontal
		Me.cbarSimus.Guid = New System.Guid("b15afb33-5835-4d55-9f3e-42d79c5c6722")
		Me.cbarSimus.Items.AddRange(New TD.SandBar.ToolbarItemBase() {Me.btCombos, Me.btDeploy, Me.btSuggest})
		Me.cbarSimus.Location = New System.Drawing.Point(0, 0)
		Me.cbarSimus.Movable = false
		Me.cbarSimus.Name = "cbarSimus"
		Me.cbarSimus.Size = New System.Drawing.Size(627, 424)
		Me.cbarSimus.TabIndex = 0
		Me.cbarSimus.Text = "Modes de simulation"
		'
		'pnlSimus
		'
		Me.pnlSimus.Controls.Add(Me.grpCombos)
		Me.pnlSimus.Controls.Add(Me.grpSuggest)
		Me.pnlSimus.Controls.Add(Me.grpDeploy)
		Me.pnlSimus.Location = New System.Drawing.Point(2, 49)
		Me.pnlSimus.Name = "pnlSimus"
		Me.pnlSimus.Size = New System.Drawing.Size(623, 373)
		Me.pnlSimus.TabIndex = 0
		'
		'grpCombos
		'
		Me.grpCombos.BackColor = System.Drawing.Color.Transparent
		Me.grpCombos.Controls.Add(Me.splitCombosV)
		Me.grpCombos.Controls.Add(Me.lbl1)
		Me.grpCombos.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grpCombos.Location = New System.Drawing.Point(0, 0)
		Me.grpCombos.Name = "grpCombos"
		Me.grpCombos.Size = New System.Drawing.Size(623, 373)
		Me.grpCombos.TabIndex = 1
		Me.grpCombos.TabStop = false
		'
		'splitCombosV
		'
		Me.splitCombosV.Dock = System.Windows.Forms.DockStyle.Fill
		Me.splitCombosV.IsSplitterFixed = true
		Me.splitCombosV.Location = New System.Drawing.Point(3, 33)
		Me.splitCombosV.Name = "splitCombosV"
		'
		'splitCombosV.Panel1
		'
		Me.splitCombosV.Panel1.Controls.Add(Me.splitCombosH1)
		'
		'splitCombosV.Panel2
		'
		Me.splitCombosV.Panel2.Controls.Add(Me.splitCombosH2)
		Me.splitCombosV.Size = New System.Drawing.Size(617, 337)
		Me.splitCombosV.SplitterDistance = 346
		Me.splitCombosV.TabIndex = 3
		'
		'splitCombosH1
		'
		Me.splitCombosH1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.splitCombosH1.IsSplitterFixed = true
		Me.splitCombosH1.Location = New System.Drawing.Point(0, 0)
		Me.splitCombosH1.Name = "splitCombosH1"
		Me.splitCombosH1.Orientation = System.Windows.Forms.Orientation.Horizontal
		'
		'splitCombosH1.Panel1
		'
		Me.splitCombosH1.Panel1.Controls.Add(Me.tabControl)
		Me.splitCombosH1.Panel1.Controls.Add(Me.toolStripCombos)
		'
		'splitCombosH1.Panel2
		'
		Me.splitCombosH1.Panel2.Controls.Add(Me.chklstSequencesDispos)
		Me.splitCombosH1.Size = New System.Drawing.Size(346, 337)
		Me.splitCombosH1.SplitterDistance = 200
		Me.splitCombosH1.TabIndex = 0
		'
		'tabControl
		'
		Me.tabControl.Controls.Add(Me.tabCarte)
		Me.tabControl.Controls.Add(Me.tabType)
		Me.tabControl.Controls.Add(Me.tabSubType)
		Me.tabControl.Controls.Add(Me.tabCost)
		Me.tabControl.Controls.Add(Me.tabColor)
		Me.tabControl.Dock = System.Windows.Forms.DockStyle.Fill
		Me.tabControl.Location = New System.Drawing.Point(0, 25)
		Me.tabControl.Name = "tabControl"
		Me.tabControl.SelectedIndex = 0
		Me.tabControl.Size = New System.Drawing.Size(346, 175)
		Me.tabControl.TabIndex = 8
		'
		'tabCarte
		'
		Me.tabCarte.Controls.Add(Me.grdCardsDispos)
		Me.tabCarte.Location = New System.Drawing.Point(4, 22)
		Me.tabCarte.Name = "tabCarte"
		Me.tabCarte.Padding = New System.Windows.Forms.Padding(3)
		Me.tabCarte.Size = New System.Drawing.Size(338, 149)
		Me.tabCarte.TabIndex = 0
		Me.tabCarte.Text = "Carte"
		Me.tabCarte.UseVisualStyleBackColor = true
		'
		'grdCardsDispos
		'
		Me.grdCardsDispos.AutoSizeMinHeight = 10
		Me.grdCardsDispos.AutoSizeMinWidth = 10
		Me.grdCardsDispos.AutoStretchColumnsToFitWidth = false
		Me.grdCardsDispos.AutoStretchRowsToFitHeight = false
		Me.grdCardsDispos.BackColor = System.Drawing.Color.Transparent
		Me.grdCardsDispos.ContextMenuStyle = SourceGrid2.ContextMenuStyle.None
		Me.grdCardsDispos.CustomSort = false
		Me.grdCardsDispos.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdCardsDispos.GridToolTipActive = true
		Me.grdCardsDispos.Location = New System.Drawing.Point(3, 3)
		Me.grdCardsDispos.Name = "grdCardsDispos"
		Me.grdCardsDispos.Size = New System.Drawing.Size(332, 143)
		Me.grdCardsDispos.SpecialKeys = CType(((((((((((SourceGrid2.GridSpecialKeys.Ctrl_C Or SourceGrid2.GridSpecialKeys.Ctrl_V)  _
						Or SourceGrid2.GridSpecialKeys.Ctrl_X)  _
						Or SourceGrid2.GridSpecialKeys.Delete)  _
						Or SourceGrid2.GridSpecialKeys.Arrows)  _
						Or SourceGrid2.GridSpecialKeys.Tab)  _
						Or SourceGrid2.GridSpecialKeys.PageDownUp)  _
						Or SourceGrid2.GridSpecialKeys.Enter)  _
						Or SourceGrid2.GridSpecialKeys.Escape)  _
						Or SourceGrid2.GridSpecialKeys.Control)  _
						Or SourceGrid2.GridSpecialKeys.Shift),SourceGrid2.GridSpecialKeys)
		Me.grdCardsDispos.TabIndex = 1
		'
		'tabType
		'
		Me.tabType.Controls.Add(Me.grdTypesDispos)
		Me.tabType.Location = New System.Drawing.Point(4, 22)
		Me.tabType.Name = "tabType"
		Me.tabType.Padding = New System.Windows.Forms.Padding(3)
		Me.tabType.Size = New System.Drawing.Size(421, 149)
		Me.tabType.TabIndex = 1
		Me.tabType.Text = "Type"
		Me.tabType.UseVisualStyleBackColor = true
		'
		'grdTypesDispos
		'
		Me.grdTypesDispos.AutoSizeMinHeight = 10
		Me.grdTypesDispos.AutoSizeMinWidth = 10
		Me.grdTypesDispos.AutoStretchColumnsToFitWidth = false
		Me.grdTypesDispos.AutoStretchRowsToFitHeight = false
		Me.grdTypesDispos.BackColor = System.Drawing.Color.Transparent
		Me.grdTypesDispos.ContextMenuStyle = SourceGrid2.ContextMenuStyle.None
		Me.grdTypesDispos.CustomSort = false
		Me.grdTypesDispos.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdTypesDispos.GridToolTipActive = true
		Me.grdTypesDispos.Location = New System.Drawing.Point(3, 3)
		Me.grdTypesDispos.Name = "grdTypesDispos"
		Me.grdTypesDispos.Size = New System.Drawing.Size(415, 143)
		Me.grdTypesDispos.SpecialKeys = CType(((((((((((SourceGrid2.GridSpecialKeys.Ctrl_C Or SourceGrid2.GridSpecialKeys.Ctrl_V)  _
						Or SourceGrid2.GridSpecialKeys.Ctrl_X)  _
						Or SourceGrid2.GridSpecialKeys.Delete)  _
						Or SourceGrid2.GridSpecialKeys.Arrows)  _
						Or SourceGrid2.GridSpecialKeys.Tab)  _
						Or SourceGrid2.GridSpecialKeys.PageDownUp)  _
						Or SourceGrid2.GridSpecialKeys.Enter)  _
						Or SourceGrid2.GridSpecialKeys.Escape)  _
						Or SourceGrid2.GridSpecialKeys.Control)  _
						Or SourceGrid2.GridSpecialKeys.Shift),SourceGrid2.GridSpecialKeys)
		Me.grdTypesDispos.TabIndex = 1
		'
		'tabSubType
		'
		Me.tabSubType.Controls.Add(Me.grdSubTypesDispos)
		Me.tabSubType.Location = New System.Drawing.Point(4, 22)
		Me.tabSubType.Name = "tabSubType"
		Me.tabSubType.Padding = New System.Windows.Forms.Padding(3)
		Me.tabSubType.Size = New System.Drawing.Size(421, 149)
		Me.tabSubType.TabIndex = 2
		Me.tabSubType.Text = "Sous-type"
		Me.tabSubType.UseVisualStyleBackColor = true
		'
		'grdSubTypesDispos
		'
		Me.grdSubTypesDispos.AutoSizeMinHeight = 10
		Me.grdSubTypesDispos.AutoSizeMinWidth = 10
		Me.grdSubTypesDispos.AutoStretchColumnsToFitWidth = false
		Me.grdSubTypesDispos.AutoStretchRowsToFitHeight = false
		Me.grdSubTypesDispos.BackColor = System.Drawing.Color.Transparent
		Me.grdSubTypesDispos.ContextMenuStyle = SourceGrid2.ContextMenuStyle.None
		Me.grdSubTypesDispos.CustomSort = false
		Me.grdSubTypesDispos.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdSubTypesDispos.GridToolTipActive = true
		Me.grdSubTypesDispos.Location = New System.Drawing.Point(3, 3)
		Me.grdSubTypesDispos.Name = "grdSubTypesDispos"
		Me.grdSubTypesDispos.Size = New System.Drawing.Size(415, 143)
		Me.grdSubTypesDispos.SpecialKeys = CType(((((((((((SourceGrid2.GridSpecialKeys.Ctrl_C Or SourceGrid2.GridSpecialKeys.Ctrl_V)  _
						Or SourceGrid2.GridSpecialKeys.Ctrl_X)  _
						Or SourceGrid2.GridSpecialKeys.Delete)  _
						Or SourceGrid2.GridSpecialKeys.Arrows)  _
						Or SourceGrid2.GridSpecialKeys.Tab)  _
						Or SourceGrid2.GridSpecialKeys.PageDownUp)  _
						Or SourceGrid2.GridSpecialKeys.Enter)  _
						Or SourceGrid2.GridSpecialKeys.Escape)  _
						Or SourceGrid2.GridSpecialKeys.Control)  _
						Or SourceGrid2.GridSpecialKeys.Shift),SourceGrid2.GridSpecialKeys)
		Me.grdSubTypesDispos.TabIndex = 1
		'
		'tabCost
		'
		Me.tabCost.Controls.Add(Me.grdCostsDispos)
		Me.tabCost.Location = New System.Drawing.Point(4, 22)
		Me.tabCost.Name = "tabCost"
		Me.tabCost.Padding = New System.Windows.Forms.Padding(3)
		Me.tabCost.Size = New System.Drawing.Size(421, 149)
		Me.tabCost.TabIndex = 3
		Me.tabCost.Text = "Invocation"
		Me.tabCost.UseVisualStyleBackColor = true
		'
		'grdCostsDispos
		'
		Me.grdCostsDispos.AutoSizeMinHeight = 10
		Me.grdCostsDispos.AutoSizeMinWidth = 10
		Me.grdCostsDispos.AutoStretchColumnsToFitWidth = false
		Me.grdCostsDispos.AutoStretchRowsToFitHeight = false
		Me.grdCostsDispos.BackColor = System.Drawing.Color.Transparent
		Me.grdCostsDispos.ContextMenuStyle = SourceGrid2.ContextMenuStyle.None
		Me.grdCostsDispos.CustomSort = false
		Me.grdCostsDispos.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdCostsDispos.GridToolTipActive = true
		Me.grdCostsDispos.Location = New System.Drawing.Point(3, 3)
		Me.grdCostsDispos.Name = "grdCostsDispos"
		Me.grdCostsDispos.Size = New System.Drawing.Size(415, 143)
		Me.grdCostsDispos.SpecialKeys = CType(((((((((((SourceGrid2.GridSpecialKeys.Ctrl_C Or SourceGrid2.GridSpecialKeys.Ctrl_V)  _
						Or SourceGrid2.GridSpecialKeys.Ctrl_X)  _
						Or SourceGrid2.GridSpecialKeys.Delete)  _
						Or SourceGrid2.GridSpecialKeys.Arrows)  _
						Or SourceGrid2.GridSpecialKeys.Tab)  _
						Or SourceGrid2.GridSpecialKeys.PageDownUp)  _
						Or SourceGrid2.GridSpecialKeys.Enter)  _
						Or SourceGrid2.GridSpecialKeys.Escape)  _
						Or SourceGrid2.GridSpecialKeys.Control)  _
						Or SourceGrid2.GridSpecialKeys.Shift),SourceGrid2.GridSpecialKeys)
		Me.grdCostsDispos.TabIndex = 1
		'
		'tabColor
		'
		Me.tabColor.Controls.Add(Me.grdColorsDispos)
		Me.tabColor.Location = New System.Drawing.Point(4, 22)
		Me.tabColor.Name = "tabColor"
		Me.tabColor.Padding = New System.Windows.Forms.Padding(3)
		Me.tabColor.Size = New System.Drawing.Size(421, 149)
		Me.tabColor.TabIndex = 4
		Me.tabColor.Text = "Couleur"
		Me.tabColor.UseVisualStyleBackColor = true
		'
		'grdColorsDispos
		'
		Me.grdColorsDispos.AutoSizeMinHeight = 10
		Me.grdColorsDispos.AutoSizeMinWidth = 10
		Me.grdColorsDispos.AutoStretchColumnsToFitWidth = false
		Me.grdColorsDispos.AutoStretchRowsToFitHeight = false
		Me.grdColorsDispos.BackColor = System.Drawing.Color.Transparent
		Me.grdColorsDispos.ContextMenuStyle = SourceGrid2.ContextMenuStyle.None
		Me.grdColorsDispos.CustomSort = false
		Me.grdColorsDispos.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdColorsDispos.GridToolTipActive = true
		Me.grdColorsDispos.Location = New System.Drawing.Point(3, 3)
		Me.grdColorsDispos.Name = "grdColorsDispos"
		Me.grdColorsDispos.Size = New System.Drawing.Size(415, 143)
		Me.grdColorsDispos.SpecialKeys = CType(((((((((((SourceGrid2.GridSpecialKeys.Ctrl_C Or SourceGrid2.GridSpecialKeys.Ctrl_V)  _
						Or SourceGrid2.GridSpecialKeys.Ctrl_X)  _
						Or SourceGrid2.GridSpecialKeys.Delete)  _
						Or SourceGrid2.GridSpecialKeys.Arrows)  _
						Or SourceGrid2.GridSpecialKeys.Tab)  _
						Or SourceGrid2.GridSpecialKeys.PageDownUp)  _
						Or SourceGrid2.GridSpecialKeys.Enter)  _
						Or SourceGrid2.GridSpecialKeys.Escape)  _
						Or SourceGrid2.GridSpecialKeys.Control)  _
						Or SourceGrid2.GridSpecialKeys.Shift),SourceGrid2.GridSpecialKeys)
		Me.grdColorsDispos.TabIndex = 1
		'
		'toolStripCombos
		'
		Me.toolStripCombos.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
		Me.toolStripCombos.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btAddSequence, Me.btRemove, Me.btClearAll})
		Me.toolStripCombos.Location = New System.Drawing.Point(0, 0)
		Me.toolStripCombos.Name = "toolStripCombos"
		Me.toolStripCombos.Size = New System.Drawing.Size(346, 25)
		Me.toolStripCombos.TabIndex = 7
		Me.toolStripCombos.Text = "toolStrip1"
		'
		'btAddSequence
		'
		Me.btAddSequence.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.btAddSequence.Image = CType(resources.GetObject("btAddSequence.Image"),System.Drawing.Image)
		Me.btAddSequence.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btAddSequence.Name = "btAddSequence"
		Me.btAddSequence.Size = New System.Drawing.Size(23, 22)
		Me.btAddSequence.Text = "Ajouter la séquence"
		AddHandler Me.btAddSequence.Click, AddressOf Me.BtAddSequenceClick
		'
		'btRemove
		'
		Me.btRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.btRemove.Image = CType(resources.GetObject("btRemove.Image"),System.Drawing.Image)
		Me.btRemove.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btRemove.Name = "btRemove"
		Me.btRemove.Size = New System.Drawing.Size(23, 22)
		Me.btRemove.Text = "Supprimer la séquence sélectionnée"
		AddHandler Me.btRemove.Click, AddressOf Me.BtRemoveClick
		'
		'btClearAll
		'
		Me.btClearAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.btClearAll.Image = CType(resources.GetObject("btClearAll.Image"),System.Drawing.Image)
		Me.btClearAll.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btClearAll.Name = "btClearAll"
		Me.btClearAll.Size = New System.Drawing.Size(23, 22)
		Me.btClearAll.Text = "Supprimer toutes les séquences"
		AddHandler Me.btClearAll.Click, AddressOf Me.BtClearAllClick
		'
		'chklstSequencesDispos
		'
		Me.chklstSequencesDispos.Dock = System.Windows.Forms.DockStyle.Fill
		Me.chklstSequencesDispos.FormattingEnabled = true
		Me.chklstSequencesDispos.Location = New System.Drawing.Point(0, 0)
		Me.chklstSequencesDispos.Name = "chklstSequencesDispos"
		Me.chklstSequencesDispos.Size = New System.Drawing.Size(346, 124)
		Me.chklstSequencesDispos.TabIndex = 0
		AddHandler Me.chklstSequencesDispos.SelectedIndexChanged, AddressOf Me.ChklstSequencesDisposSelectedIndexChanged
		'
		'splitCombosH2
		'
		Me.splitCombosH2.Dock = System.Windows.Forms.DockStyle.Fill
		Me.splitCombosH2.IsSplitterFixed = true
		Me.splitCombosH2.Location = New System.Drawing.Point(0, 0)
		Me.splitCombosH2.Name = "splitCombosH2"
		Me.splitCombosH2.Orientation = System.Windows.Forms.Orientation.Horizontal
		'
		'splitCombosH2.Panel1
		'
		Me.splitCombosH2.Panel1.Controls.Add(Me.picScanCard2)
		'
		'splitCombosH2.Panel2
		'
		Me.splitCombosH2.Panel2.Controls.Add(Me.prgSimu)
		Me.splitCombosH2.Panel2.Controls.Add(Me.toolStripCombos2)
		Me.splitCombosH2.Panel2.Controls.Add(Me.lbl8)
		Me.splitCombosH2.Panel2.Controls.Add(Me.txtEspCumul)
		Me.splitCombosH2.Panel2.Controls.Add(Me.cboTourCumul)
		Me.splitCombosH2.Panel2.Controls.Add(Me.lbl4)
		Me.splitCombosH2.Size = New System.Drawing.Size(267, 337)
		Me.splitCombosH2.SplitterDistance = 200
		Me.splitCombosH2.TabIndex = 41
		'
		'picScanCard2
		'
		Me.picScanCard2.Dock = System.Windows.Forms.DockStyle.Fill
		Me.picScanCard2.Location = New System.Drawing.Point(0, 0)
		Me.picScanCard2.Name = "picScanCard2"
		Me.picScanCard2.Size = New System.Drawing.Size(267, 200)
		Me.picScanCard2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
		Me.picScanCard2.TabIndex = 41
		Me.picScanCard2.TabStop = false
		'
		'prgSimu
		'
		Me.prgSimu.Dock = System.Windows.Forms.DockStyle.Top
		Me.prgSimu.Location = New System.Drawing.Point(0, 25)
		Me.prgSimu.Name = "prgSimu"
		Me.prgSimu.Size = New System.Drawing.Size(267, 14)
		Me.prgSimu.TabIndex = 16
		'
		'toolStripCombos2
		'
		Me.toolStripCombos2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
		Me.toolStripCombos2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.txtN, Me.lbl3, Me.btSeparator, Me.btSimu, Me.btAddPlot})
		Me.toolStripCombos2.Location = New System.Drawing.Point(0, 0)
		Me.toolStripCombos2.Name = "toolStripCombos2"
		Me.toolStripCombos2.Size = New System.Drawing.Size(267, 25)
		Me.toolStripCombos2.TabIndex = 15
		Me.toolStripCombos2.Text = "toolStrip1"
		'
		'txtN
		'
		Me.txtN.Name = "txtN"
		Me.txtN.Size = New System.Drawing.Size(40, 25)
		Me.txtN.Text = "1000"
		Me.txtN.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right
		'
		'lbl3
		'
		Me.lbl3.Name = "lbl3"
		Me.lbl3.Size = New System.Drawing.Size(40, 22)
		Me.lbl3.Text = "parties"
		'
		'btSeparator
		'
		Me.btSeparator.Name = "btSeparator"
		Me.btSeparator.Size = New System.Drawing.Size(6, 25)
		'
		'btSimu
		'
		Me.btSimu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.btSimu.Image = CType(resources.GetObject("btSimu.Image"),System.Drawing.Image)
		Me.btSimu.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btSimu.Name = "btSimu"
		Me.btSimu.Size = New System.Drawing.Size(23, 22)
		Me.btSimu.Text = "Lancer le calcul"
		AddHandler Me.btSimu.Click, AddressOf Me.BtSimusClick
		'
		'btAddPlot
		'
		Me.btAddPlot.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.btAddPlot.Enabled = false
		Me.btAddPlot.Image = CType(resources.GetObject("btAddPlot.Image"),System.Drawing.Image)
		Me.btAddPlot.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btAddPlot.Name = "btAddPlot"
		Me.btAddPlot.Size = New System.Drawing.Size(23, 22)
		Me.btAddPlot.Text = "Graphe résultat"
		AddHandler Me.btAddPlot.Click, AddressOf Me.BtAddPlotClick
		'
		'lbl8
		'
		Me.lbl8.AutoSize = true
		Me.lbl8.Location = New System.Drawing.Point(141, 63)
		Me.lbl8.Name = "lbl8"
		Me.lbl8.Size = New System.Drawing.Size(15, 13)
		Me.lbl8.TabIndex = 14
		Me.lbl8.Text = "%"
		'
		'txtEspCumul
		'
		Me.txtEspCumul.Enabled = false
		Me.txtEspCumul.Location = New System.Drawing.Point(74, 60)
		Me.txtEspCumul.Name = "txtEspCumul"
		Me.txtEspCumul.Size = New System.Drawing.Size(61, 20)
		Me.txtEspCumul.TabIndex = 13
		'
		'cboTourCumul
		'
		Me.cboTourCumul.FormattingEnabled = true
		Me.cboTourCumul.Location = New System.Drawing.Point(7, 60)
		Me.cboTourCumul.Name = "cboTourCumul"
		Me.cboTourCumul.Size = New System.Drawing.Size(61, 21)
		Me.cboTourCumul.TabIndex = 12
		AddHandler Me.cboTourCumul.SelectedIndexChanged, AddressOf Me.CboTourCumulSelectedIndexChanged
		'
		'lbl4
		'
		Me.lbl4.AutoSize = true
		Me.lbl4.Location = New System.Drawing.Point(7, 44)
		Me.lbl4.Name = "lbl4"
		Me.lbl4.Size = New System.Drawing.Size(107, 13)
		Me.lbl4.TabIndex = 11
		Me.lbl4.Text = "Probabilité au tour n :"
		'
		'lbl1
		'
		Me.lbl1.Dock = System.Windows.Forms.DockStyle.Top
		Me.lbl1.Location = New System.Drawing.Point(3, 16)
		Me.lbl1.Name = "lbl1"
		Me.lbl1.Size = New System.Drawing.Size(617, 17)
		Me.lbl1.TabIndex = 2
		Me.lbl1.Text = "Sélectionner des éléments pour en estimer la probabilité d'apparition :"
		'
		'grpSuggest
		'
		Me.grpSuggest.BackColor = System.Drawing.Color.Transparent
		Me.grpSuggest.Controls.Add(Me.cmdCorrExpr)
		Me.grpSuggest.Controls.Add(Me.sldPertin)
		Me.grpSuggest.Controls.Add(Me.lbl17)
		Me.grpSuggest.Controls.Add(Me.txtPrix)
		Me.grpSuggest.Controls.Add(Me.txtEditions)
		Me.grpSuggest.Controls.Add(Me.txtInvoc)
		Me.grpSuggest.Controls.Add(Me.cmdSuggest)
		Me.grpSuggest.Controls.Add(Me.txtColors)
		Me.grpSuggest.Controls.Add(Me.chkPrix)
		Me.grpSuggest.Controls.Add(Me.chkEditions)
		Me.grpSuggest.Controls.Add(Me.chkInvoc)
		Me.grpSuggest.Controls.Add(Me.chkColors)
		Me.grpSuggest.Controls.Add(Me.lbl16)
		Me.grpSuggest.Controls.Add(Me.lbl15)
		Me.grpSuggest.Controls.Add(Me.cmdDetect)
		Me.grpSuggest.Controls.Add(Me.prgSuggest)
		Me.grpSuggest.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grpSuggest.Location = New System.Drawing.Point(0, 0)
		Me.grpSuggest.Name = "grpSuggest"
		Me.grpSuggest.Size = New System.Drawing.Size(623, 373)
		Me.grpSuggest.TabIndex = 3
		Me.grpSuggest.TabStop = false
		Me.grpSuggest.Visible = false
		'
		'cmdCorrExpr
		'
		Me.cmdCorrExpr.Enabled = false
		Me.cmdCorrExpr.Location = New System.Drawing.Point(129, 89)
		Me.cmdCorrExpr.Name = "cmdCorrExpr"
		Me.cmdCorrExpr.Size = New System.Drawing.Size(193, 23)
		Me.cmdCorrExpr.TabIndex = 38
		Me.cmdCorrExpr.Text = "Personnaliser les expressions"
		Me.cmdCorrExpr.UseVisualStyleBackColor = true
		AddHandler Me.cmdCorrExpr.Click, AddressOf Me.CmdCorrExprClick
		'
		'sldPertin
		'
		Me.sldPertin.BackColor = System.Drawing.SystemColors.Control
		Me.sldPertin.Enabled = false
		Me.sldPertin.Location = New System.Drawing.Point(129, 297)
		Me.sldPertin.Minimum = 1
		Me.sldPertin.Name = "sldPertin"
		Me.sldPertin.Size = New System.Drawing.Size(193, 45)
		Me.sldPertin.TabIndex = 37
		Me.sldPertin.Value = 4
		'
		'lbl17
		'
		Me.lbl17.AutoSize = true
		Me.lbl17.Enabled = false
		Me.lbl17.Location = New System.Drawing.Point(3, 272)
		Me.lbl17.Name = "lbl17"
		Me.lbl17.Size = New System.Drawing.Size(141, 13)
		Me.lbl17.TabIndex = 36
		Me.lbl17.Text = "Degré de pertinence désiré :"
		'
		'txtPrix
		'
		Me.txtPrix.Enabled = false
		Me.txtPrix.Location = New System.Drawing.Point(189, 232)
		Me.txtPrix.Name = "txtPrix"
		Me.txtPrix.ReadOnly = true
		Me.txtPrix.Size = New System.Drawing.Size(133, 20)
		Me.txtPrix.TabIndex = 35
		Me.txtPrix.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		'
		'txtEditions
		'
		Me.txtEditions.Enabled = false
		Me.txtEditions.Location = New System.Drawing.Point(189, 208)
		Me.txtEditions.Name = "txtEditions"
		Me.txtEditions.ReadOnly = true
		Me.txtEditions.Size = New System.Drawing.Size(133, 20)
		Me.txtEditions.TabIndex = 34
		Me.txtEditions.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		'
		'txtInvoc
		'
		Me.txtInvoc.Enabled = false
		Me.txtInvoc.Location = New System.Drawing.Point(189, 185)
		Me.txtInvoc.Name = "txtInvoc"
		Me.txtInvoc.ReadOnly = true
		Me.txtInvoc.Size = New System.Drawing.Size(133, 20)
		Me.txtInvoc.TabIndex = 33
		Me.txtInvoc.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		'
		'cmdSuggest
		'
		Me.cmdSuggest.Enabled = false
		Me.cmdSuggest.Location = New System.Drawing.Point(391, 300)
		Me.cmdSuggest.Name = "cmdSuggest"
		Me.cmdSuggest.Size = New System.Drawing.Size(193, 23)
		Me.cmdSuggest.TabIndex = 32
		Me.cmdSuggest.Text = "Suggérer des cartes !"
		Me.cmdSuggest.UseVisualStyleBackColor = true
		AddHandler Me.cmdSuggest.Click, AddressOf Me.CmdSuggestClick
		'
		'txtColors
		'
		Me.txtColors.Enabled = false
		Me.txtColors.Location = New System.Drawing.Point(189, 163)
		Me.txtColors.Name = "txtColors"
		Me.txtColors.ReadOnly = true
		Me.txtColors.Size = New System.Drawing.Size(133, 20)
		Me.txtColors.TabIndex = 31
		Me.txtColors.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		'
		'chkPrix
		'
		Me.chkPrix.AutoSize = true
		Me.chkPrix.Checked = true
		Me.chkPrix.CheckState = System.Windows.Forms.CheckState.Checked
		Me.chkPrix.Enabled = false
		Me.chkPrix.Location = New System.Drawing.Point(70, 234)
		Me.chkPrix.Name = "chkPrix"
		Me.chkPrix.Size = New System.Drawing.Size(96, 17)
		Me.chkPrix.TabIndex = 30
		Me.chkPrix.Text = "Gamme de prix"
		Me.chkPrix.UseVisualStyleBackColor = true
		'
		'chkEditions
		'
		Me.chkEditions.AutoSize = true
		Me.chkEditions.Checked = true
		Me.chkEditions.CheckState = System.Windows.Forms.CheckState.Checked
		Me.chkEditions.Enabled = false
		Me.chkEditions.Location = New System.Drawing.Point(70, 211)
		Me.chkEditions.Name = "chkEditions"
		Me.chkEditions.Size = New System.Drawing.Size(63, 17)
		Me.chkEditions.TabIndex = 29
		Me.chkEditions.Text = "Editions"
		Me.chkEditions.UseVisualStyleBackColor = true
		'
		'chkInvoc
		'
		Me.chkInvoc.AutoSize = true
		Me.chkInvoc.Checked = true
		Me.chkInvoc.CheckState = System.Windows.Forms.CheckState.Checked
		Me.chkInvoc.Enabled = false
		Me.chkInvoc.Location = New System.Drawing.Point(70, 188)
		Me.chkInvoc.Name = "chkInvoc"
		Me.chkInvoc.Size = New System.Drawing.Size(113, 17)
		Me.chkInvoc.TabIndex = 28
		Me.chkInvoc.Text = "Coûts d'invocation"
		Me.chkInvoc.UseVisualStyleBackColor = true
		'
		'chkColors
		'
		Me.chkColors.AutoSize = true
		Me.chkColors.Checked = true
		Me.chkColors.CheckState = System.Windows.Forms.CheckState.Checked
		Me.chkColors.Enabled = false
		Me.chkColors.Location = New System.Drawing.Point(70, 165)
		Me.chkColors.Name = "chkColors"
		Me.chkColors.Size = New System.Drawing.Size(67, 17)
		Me.chkColors.TabIndex = 27
		Me.chkColors.Text = "Couleurs"
		Me.chkColors.UseVisualStyleBackColor = true
		'
		'lbl16
		'
		Me.lbl16.AutoSize = true
		Me.lbl16.Enabled = false
		Me.lbl16.Location = New System.Drawing.Point(3, 137)
		Me.lbl16.Name = "lbl16"
		Me.lbl16.Size = New System.Drawing.Size(353, 13)
		Me.lbl16.TabIndex = 26
		Me.lbl16.Text = "Sélectionner ensuite des critères pour restreindre la recherche (facultatif) :"
		'
		'lbl15
		'
		Me.lbl15.Dock = System.Windows.Forms.DockStyle.Top
		Me.lbl15.Location = New System.Drawing.Point(3, 16)
		Me.lbl15.Name = "lbl15"
		Me.lbl15.Size = New System.Drawing.Size(617, 43)
		Me.lbl15.TabIndex = 24
		Me.lbl15.Text = "Pour commencer, cliquer ci-dessous pour estimer les informations de corrélation ("& _ 
		"cela peut prendre un certain temps s'il y a beaucoup de cartes dans le deck) :"
		'
		'cmdDetect
		'
		Me.cmdDetect.Location = New System.Drawing.Point(129, 60)
		Me.cmdDetect.Name = "cmdDetect"
		Me.cmdDetect.Size = New System.Drawing.Size(193, 23)
		Me.cmdDetect.TabIndex = 23
		Me.cmdDetect.Text = "Déterminer les paramètres du jeu"
		Me.cmdDetect.UseVisualStyleBackColor = true
		AddHandler Me.cmdDetect.Click, AddressOf Me.CmdDetectClick
		'
		'prgSuggest
		'
		Me.prgSuggest.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.prgSuggest.Location = New System.Drawing.Point(3, 356)
		Me.prgSuggest.Name = "prgSuggest"
		Me.prgSuggest.Size = New System.Drawing.Size(617, 14)
		Me.prgSuggest.TabIndex = 22
		'
		'grpDeploy
		'
		Me.grpDeploy.BackColor = System.Drawing.Color.Transparent
		Me.grpDeploy.Controls.Add(Me.splitDeployH)
		Me.grpDeploy.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grpDeploy.Location = New System.Drawing.Point(0, 0)
		Me.grpDeploy.Name = "grpDeploy"
		Me.grpDeploy.Size = New System.Drawing.Size(623, 373)
		Me.grpDeploy.TabIndex = 2
		Me.grpDeploy.TabStop = false
		Me.grpDeploy.Visible = false
		'
		'splitDeployH
		'
		Me.splitDeployH.Dock = System.Windows.Forms.DockStyle.Fill
		Me.splitDeployH.IsSplitterFixed = true
		Me.splitDeployH.Location = New System.Drawing.Point(3, 16)
		Me.splitDeployH.Name = "splitDeployH"
		Me.splitDeployH.Orientation = System.Windows.Forms.Orientation.Horizontal
		'
		'splitDeployH.Panel1
		'
		Me.splitDeployH.Panel1.Controls.Add(Me.splitDeployV)
		'
		'splitDeployH.Panel2
		'
		Me.splitDeployH.Panel2.Controls.Add(Me.chkDefaut)
		Me.splitDeployH.Panel2.Controls.Add(Me.txtEspDefaut)
		Me.splitDeployH.Panel2.Controls.Add(Me.chkVerbosity)
		Me.splitDeployH.Panel2.Controls.Add(Me.cmdAddPlot2)
		Me.splitDeployH.Panel2.Controls.Add(Me.cboTourDeploy2)
		Me.splitDeployH.Panel2.Controls.Add(Me.prgSimu2)
		Me.splitDeployH.Panel2.Controls.Add(Me.lbl11)
		Me.splitDeployH.Panel2.Controls.Add(Me.cmdSimu2)
		Me.splitDeployH.Panel2.Controls.Add(Me.lbl9)
		Me.splitDeployH.Panel2.Controls.Add(Me.txtN2)
		Me.splitDeployH.Panel2.Controls.Add(Me.lbl10)
		Me.splitDeployH.Size = New System.Drawing.Size(617, 354)
		Me.splitDeployH.SplitterDistance = 175
		Me.splitDeployH.TabIndex = 0
		'
		'splitDeployV
		'
		Me.splitDeployV.Dock = System.Windows.Forms.DockStyle.Fill
		Me.splitDeployV.IsSplitterFixed = true
		Me.splitDeployV.Location = New System.Drawing.Point(0, 0)
		Me.splitDeployV.Name = "splitDeployV"
		'
		'splitDeployV.Panel1
		'
		Me.splitDeployV.Panel1.Controls.Add(Me.lstUserCombos)
		Me.splitDeployV.Panel1.Controls.Add(Me.lbl12)
		'
		'splitDeployV.Panel2
		'
		Me.splitDeployV.Panel2.Controls.Add(Me.lbl14)
		Me.splitDeployV.Panel2.Controls.Add(Me.lbl13)
		Me.splitDeployV.Panel2.Controls.Add(Me.txtEspManas)
		Me.splitDeployV.Panel2.Controls.Add(Me.cboTourDeploy)
		Me.splitDeployV.Size = New System.Drawing.Size(617, 175)
		Me.splitDeployV.SplitterDistance = 323
		Me.splitDeployV.TabIndex = 24
		'
		'lstUserCombos
		'
		Me.lstUserCombos.CheckOnClick = true
		Me.lstUserCombos.Dock = System.Windows.Forms.DockStyle.Fill
		Me.lstUserCombos.FormattingEnabled = true
		Me.lstUserCombos.Location = New System.Drawing.Point(0, 28)
		Me.lstUserCombos.Name = "lstUserCombos"
		Me.lstUserCombos.Size = New System.Drawing.Size(323, 139)
		Me.lstUserCombos.TabIndex = 25
		AddHandler Me.lstUserCombos.MouseUp, AddressOf Me.LstUserCombosMouseUp
		'
		'lbl12
		'
		Me.lbl12.Dock = System.Windows.Forms.DockStyle.Top
		Me.lbl12.Location = New System.Drawing.Point(0, 0)
		Me.lbl12.Name = "lbl12"
		Me.lbl12.Size = New System.Drawing.Size(323, 28)
		Me.lbl12.TabIndex = 24
		Me.lbl12.Text = "Cartes décrites manuellement (clic droit pour éditer, décocher pour les ignorer) "& _ 
		":"
		'
		'lbl14
		'
		Me.lbl14.AutoSize = true
		Me.lbl14.Location = New System.Drawing.Point(5, 49)
		Me.lbl14.Name = "lbl14"
		Me.lbl14.Size = New System.Drawing.Size(29, 13)
		Me.lbl14.TabIndex = 14
		Me.lbl14.Text = "Tour"
		'
		'lbl13
		'
		Me.lbl13.Location = New System.Drawing.Point(5, 14)
		Me.lbl13.Name = "lbl13"
		Me.lbl13.Size = New System.Drawing.Size(160, 32)
		Me.lbl13.TabIndex = 12
		Me.lbl13.Text = "Espérance du nombre de manas productibles au tour n :"
		'
		'txtEspManas
		'
		Me.txtEspManas.Enabled = false
		Me.txtEspManas.Location = New System.Drawing.Point(104, 70)
		Me.txtEspManas.Name = "txtEspManas"
		Me.txtEspManas.Size = New System.Drawing.Size(61, 20)
		Me.txtEspManas.TabIndex = 11
		'
		'cboTourDeploy
		'
		Me.cboTourDeploy.FormattingEnabled = true
		Me.cboTourDeploy.Location = New System.Drawing.Point(104, 46)
		Me.cboTourDeploy.Name = "cboTourDeploy"
		Me.cboTourDeploy.Size = New System.Drawing.Size(61, 21)
		Me.cboTourDeploy.TabIndex = 9
		AddHandler Me.cboTourDeploy.SelectedIndexChanged, AddressOf Me.CboTourDeploySelectedIndexChanged
		'
		'chkDefaut
		'
		Me.chkDefaut.Checked = true
		Me.chkDefaut.CheckState = System.Windows.Forms.CheckState.Checked
		Me.chkDefaut.Location = New System.Drawing.Point(30, 78)
		Me.chkDefaut.Name = "chkDefaut"
		Me.chkDefaut.Size = New System.Drawing.Size(114, 30)
		Me.chkDefaut.TabIndex = 24
		Me.chkDefaut.Text = "Afficher le défaut de manas"
		Me.chkDefaut.UseVisualStyleBackColor = true
		'
		'txtEspDefaut
		'
		Me.txtEspDefaut.Enabled = false
		Me.txtEspDefaut.Location = New System.Drawing.Point(431, 83)
		Me.txtEspDefaut.Name = "txtEspDefaut"
		Me.txtEspDefaut.Size = New System.Drawing.Size(61, 20)
		Me.txtEspDefaut.TabIndex = 15
		'
		'chkVerbosity
		'
		Me.chkVerbosity.AutoSize = true
		Me.chkVerbosity.Location = New System.Drawing.Point(30, 60)
		Me.chkVerbosity.Name = "chkVerbosity"
		Me.chkVerbosity.Size = New System.Drawing.Size(70, 17)
		Me.chkVerbosity.TabIndex = 23
		Me.chkVerbosity.Text = "Verbosité"
		Me.chkVerbosity.UseVisualStyleBackColor = true
		AddHandler Me.chkVerbosity.CheckedChanged, AddressOf Me.ChkVerbosityCheckedChanged
		'
		'cmdAddPlot2
		'
		Me.cmdAddPlot2.Enabled = false
		Me.cmdAddPlot2.Location = New System.Drawing.Point(393, 124)
		Me.cmdAddPlot2.Name = "cmdAddPlot2"
		Me.cmdAddPlot2.Size = New System.Drawing.Size(100, 23)
		Me.cmdAddPlot2.TabIndex = 22
		Me.cmdAddPlot2.Text = "Graphe"
		Me.cmdAddPlot2.UseVisualStyleBackColor = true
		AddHandler Me.cmdAddPlot2.Click, AddressOf Me.CmdAddPlot2Click
		'
		'cboTourDeploy2
		'
		Me.cboTourDeploy2.FormattingEnabled = true
		Me.cboTourDeploy2.Location = New System.Drawing.Point(432, 59)
		Me.cboTourDeploy2.Name = "cboTourDeploy2"
		Me.cboTourDeploy2.Size = New System.Drawing.Size(61, 21)
		Me.cboTourDeploy2.TabIndex = 13
		AddHandler Me.cboTourDeploy2.SelectedIndexChanged, AddressOf Me.CboTourDeploy2SelectedIndexChanged
		'
		'prgSimu2
		'
		Me.prgSimu2.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.prgSimu2.Location = New System.Drawing.Point(0, 161)
		Me.prgSimu2.Name = "prgSimu2"
		Me.prgSimu2.Size = New System.Drawing.Size(617, 14)
		Me.prgSimu2.TabIndex = 21
		'
		'lbl11
		'
		Me.lbl11.AutoSize = true
		Me.lbl11.Location = New System.Drawing.Point(333, 62)
		Me.lbl11.Name = "lbl11"
		Me.lbl11.Size = New System.Drawing.Size(29, 13)
		Me.lbl11.TabIndex = 10
		Me.lbl11.Text = "Tour"
		'
		'cmdSimu2
		'
		Me.cmdSimu2.Location = New System.Drawing.Point(30, 124)
		Me.cmdSimu2.Name = "cmdSimu2"
		Me.cmdSimu2.Size = New System.Drawing.Size(100, 23)
		Me.cmdSimu2.TabIndex = 20
		Me.cmdSimu2.Text = "Simuler"
		Me.cmdSimu2.UseVisualStyleBackColor = true
		AddHandler Me.cmdSimu2.Click, AddressOf Me.CmdSimu2Click
		'
		'lbl9
		'
		Me.lbl9.AutoSize = true
		Me.lbl9.Location = New System.Drawing.Point(30, 38)
		Me.lbl9.Name = "lbl9"
		Me.lbl9.Size = New System.Drawing.Size(99, 13)
		Me.lbl9.TabIndex = 19
		Me.lbl9.Text = "Nombre de parties :"
		'
		'txtN2
		'
		Me.txtN2.Location = New System.Drawing.Point(147, 36)
		Me.txtN2.Name = "txtN2"
		Me.txtN2.Size = New System.Drawing.Size(56, 20)
		Me.txtN2.TabIndex = 18
		Me.txtN2.Text = "1000"
		Me.txtN2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		AddHandler Me.txtN2.TextChanged, AddressOf Me.TxtN2TextChanged
		'
		'lbl10
		'
		Me.lbl10.Location = New System.Drawing.Point(332, 33)
		Me.lbl10.Name = "lbl10"
		Me.lbl10.Size = New System.Drawing.Size(160, 32)
		Me.lbl10.TabIndex = 8
		Me.lbl10.Text = "Défaut de manas au tour n :"
		'
		'btCombos
		'
		Me.btCombos.Checked = true
		Me.btCombos.Icon = CType(resources.GetObject("btCombos.Icon"),System.Drawing.Icon)
		Me.btCombos.Text = "Probabilités"
		Me.btCombos.ToolTipText = "Probabilités d'apparition de combos"
		AddHandler Me.btCombos.Activate, AddressOf Me.BtCombosActivate
		'
		'btDeploy
		'
		Me.btDeploy.Icon = CType(resources.GetObject("btDeploy.Icon"),System.Drawing.Icon)
		Me.btDeploy.Text = "Déploiement"
		Me.btDeploy.ToolTipText = "Espérance de déploiement (manas productibles)"
		AddHandler Me.btDeploy.Activate, AddressOf Me.BtDeployActivate
		'
		'btSuggest
		'
		Me.btSuggest.Icon = CType(resources.GetObject("btSuggest.Icon"),System.Drawing.Icon)
		Me.btSuggest.Text = "Suggestions"
		Me.btSuggest.ToolTipText = "Suggestions de nouvelles cartes en accord avec le thème"
		AddHandler Me.btSuggest.Activate, AddressOf Me.BtSuggestActivate
		'
		'cmnuUserCombos
		'
		Me.cmnuUserCombos.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmnuAddNew, Me.cmnuDelete, Me.mnuSeparator, Me.cmnuUp, Me.cmnuDown})
		Me.cmnuUserCombos.Name = "cmnuUserCombos"
		Me.cmnuUserCombos.Size = New System.Drawing.Size(226, 98)
		'
		'cmnuAddNew
		'
		Me.cmnuAddNew.Image = CType(resources.GetObject("cmnuAddNew.Image"),System.Drawing.Image)
		Me.cmnuAddNew.Name = "cmnuAddNew"
		Me.cmnuAddNew.Size = New System.Drawing.Size(225, 22)
		Me.cmnuAddNew.Text = "Ajouter / Modifier un élément"
		AddHandler Me.cmnuAddNew.Click, AddressOf Me.CmnuAddNewClick
		'
		'cmnuDelete
		'
		Me.cmnuDelete.Enabled = false
		Me.cmnuDelete.Image = CType(resources.GetObject("cmnuDelete.Image"),System.Drawing.Image)
		Me.cmnuDelete.Name = "cmnuDelete"
		Me.cmnuDelete.Size = New System.Drawing.Size(225, 22)
		Me.cmnuDelete.Text = "Supprimer cet élément"
		AddHandler Me.cmnuDelete.Click, AddressOf Me.CmnuDeleteClick
		'
		'mnuSeparator
		'
		Me.mnuSeparator.Name = "mnuSeparator"
		Me.mnuSeparator.Size = New System.Drawing.Size(222, 6)
		'
		'cmnuUp
		'
		Me.cmnuUp.Image = CType(resources.GetObject("cmnuUp.Image"),System.Drawing.Image)
		Me.cmnuUp.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me.cmnuUp.Name = "cmnuUp"
		Me.cmnuUp.Size = New System.Drawing.Size(225, 22)
		Me.cmnuUp.Text = "Augmenter la priorité"
		AddHandler Me.cmnuUp.Click, AddressOf Me.CmnuUpMouseClick
		'
		'cmnuDown
		'
		Me.cmnuDown.Image = CType(resources.GetObject("cmnuDown.Image"),System.Drawing.Image)
		Me.cmnuDown.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me.cmnuDown.Name = "cmnuDown"
		Me.cmnuDown.Size = New System.Drawing.Size(225, 22)
		Me.cmnuDown.Text = "Diminuer la priorité"
		AddHandler Me.cmnuDown.Click, AddressOf Me.CmnuDownMouseClick
		'
		'dlgVerbose
		'
		Me.dlgVerbose.DefaultExt = "txt"
		Me.dlgVerbose.Filter = "Fichiers texte (*.txt) | *.txt"
		Me.dlgVerbose.Title = "Enregistrer la simulation sous"
		'
		'dlgSave
		'
		Me.dlgSave.DefaultExt = "txt"
		Me.dlgSave.Filter = "Fichiers texte (*.txt)|*.txt"
		Me.dlgSave.Title = "Enregistrer sous..."
		'
		'dlgOpen
		'
		Me.dlgOpen.DefaultExt = "txt"
		Me.dlgOpen.Filter = "Fichiers texte (*.txt)|*.txt"
		Me.dlgOpen.Title = "Ouvrir"
		'
		'frmSimu
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(627, 424)
		Me.Controls.Add(Me.cbarSimus)
		Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
		Me.Name = "frmSimu"
		Me.Text = "Simulations"
		AddHandler Load, AddressOf Me.FrmSimuLoad
		Me.cbarSimus.ResumeLayout(false)
		Me.pnlSimus.ResumeLayout(false)
		Me.grpCombos.ResumeLayout(false)
		Me.splitCombosV.Panel1.ResumeLayout(false)
		Me.splitCombosV.Panel2.ResumeLayout(false)
		Me.splitCombosV.ResumeLayout(false)
		Me.splitCombosH1.Panel1.ResumeLayout(false)
		Me.splitCombosH1.Panel1.PerformLayout
		Me.splitCombosH1.Panel2.ResumeLayout(false)
		Me.splitCombosH1.ResumeLayout(false)
		Me.tabControl.ResumeLayout(false)
		Me.tabCarte.ResumeLayout(false)
		Me.grdCardsDispos.ResumeLayout(false)
		Me.tabType.ResumeLayout(false)
		Me.grdTypesDispos.ResumeLayout(false)
		Me.tabSubType.ResumeLayout(false)
		Me.grdSubTypesDispos.ResumeLayout(false)
		Me.tabCost.ResumeLayout(false)
		Me.grdCostsDispos.ResumeLayout(false)
		Me.tabColor.ResumeLayout(false)
		Me.grdColorsDispos.ResumeLayout(false)
		Me.toolStripCombos.ResumeLayout(false)
		Me.toolStripCombos.PerformLayout
		Me.splitCombosH2.Panel1.ResumeLayout(false)
		Me.splitCombosH2.Panel2.ResumeLayout(false)
		Me.splitCombosH2.Panel2.PerformLayout
		Me.splitCombosH2.ResumeLayout(false)
		CType(Me.picScanCard2,System.ComponentModel.ISupportInitialize).EndInit
		Me.toolStripCombos2.ResumeLayout(false)
		Me.toolStripCombos2.PerformLayout
		Me.grpSuggest.ResumeLayout(false)
		Me.grpSuggest.PerformLayout
		CType(Me.sldPertin,System.ComponentModel.ISupportInitialize).EndInit
		Me.grpDeploy.ResumeLayout(false)
		Me.splitDeployH.Panel1.ResumeLayout(false)
		Me.splitDeployH.Panel2.ResumeLayout(false)
		Me.splitDeployH.Panel2.PerformLayout
		Me.splitDeployH.ResumeLayout(false)
		Me.splitDeployV.Panel1.ResumeLayout(false)
		Me.splitDeployV.Panel2.ResumeLayout(false)
		Me.splitDeployV.Panel2.PerformLayout
		Me.splitDeployV.ResumeLayout(false)
		Me.cmnuUserCombos.ResumeLayout(false)
		Me.ResumeLayout(false)
	End Sub
	Private grdColorsDispos As SourceGrid2.Grid
	Private grdCostsDispos As SourceGrid2.Grid
	Private grdSubTypesDispos As SourceGrid2.Grid
	Private grdTypesDispos As SourceGrid2.Grid
	Private grdCardsDispos As SourceGrid2.Grid
	Private tabColor As System.Windows.Forms.TabPage
	Private tabCost As System.Windows.Forms.TabPage
	Private tabSubType As System.Windows.Forms.TabPage
	Private tabType As System.Windows.Forms.TabPage
	Private tabCarte As System.Windows.Forms.TabPage
	Private tabControl As System.Windows.Forms.TabControl
	Private btRemove As System.Windows.Forms.ToolStripButton
	Private dlgOpen As System.Windows.Forms.OpenFileDialog
	Private dlgSave As System.Windows.Forms.SaveFileDialog
	Private lbl3 As System.Windows.Forms.ToolStripLabel
	Private picScanCard2 As System.Windows.Forms.PictureBox
	Private btClearAll As System.Windows.Forms.ToolStripButton
	Private btAddSequence As System.Windows.Forms.ToolStripButton
	Private toolStripCombos As System.Windows.Forms.ToolStrip
	Private toolStripCombos2 As System.Windows.Forms.ToolStrip
	Private chklstSequencesDispos As System.Windows.Forms.CheckedListBox
	Private btAddPlot As System.Windows.Forms.ToolStripButton
	Private btSimu As System.Windows.Forms.ToolStripButton
	Private btSeparator As System.Windows.Forms.ToolStripSeparator
	Private splitCombosH1 As System.Windows.Forms.SplitContainer
	Private splitCombosH2 As System.Windows.Forms.SplitContainer
	Private cmdCorrExpr As System.Windows.Forms.Button
	Private lbl17 As System.Windows.Forms.Label
	Private sldPertin As System.Windows.Forms.TrackBar
	Private txtColors As System.Windows.Forms.TextBox
	Private cmdSuggest As System.Windows.Forms.Button
	Private txtInvoc As System.Windows.Forms.TextBox
	Private txtEditions As System.Windows.Forms.TextBox
	Private txtPrix As System.Windows.Forms.TextBox
	Private prgSuggest As System.Windows.Forms.ProgressBar
	Private chkEditions As System.Windows.Forms.CheckBox
	Private chkPrix As System.Windows.Forms.CheckBox
	Private cmdDetect As System.Windows.Forms.Button
	Private lbl15 As System.Windows.Forms.Label
	Private lbl16 As System.Windows.Forms.Label
	Private chkColors As System.Windows.Forms.CheckBox
	Private chkInvoc As System.Windows.Forms.CheckBox
	Private btSuggest As TD.SandBar.ButtonItem
	Private grpSuggest As System.Windows.Forms.GroupBox
	Private txtEspDefaut As System.Windows.Forms.TextBox
	Private chkDefaut As System.Windows.Forms.CheckBox
	Private lbl13 As System.Windows.Forms.Label
	Private cboTourDeploy2 As System.Windows.Forms.ComboBox
	Private lbl14 As System.Windows.Forms.Label
	Private splitDeployV As System.Windows.Forms.SplitContainer
	Private cmnuUp As System.Windows.Forms.ToolStripMenuItem
	Private cmnuDown As System.Windows.Forms.ToolStripMenuItem
	Private dlgVerbose As System.Windows.Forms.SaveFileDialog
	Private chkVerbosity As System.Windows.Forms.CheckBox
	Private mnuSeparator As System.Windows.Forms.ToolStripSeparator
	Private cmnuDelete As System.Windows.Forms.ToolStripMenuItem
	Private cmnuAddNew As System.Windows.Forms.ToolStripMenuItem
	Private cmnuUserCombos As System.Windows.Forms.ContextMenuStrip
	Private lbl12 As System.Windows.Forms.Label
	Private lstUserCombos As System.Windows.Forms.CheckedListBox
	Private cmdAddPlot2 As System.Windows.Forms.Button
	Private lbl10 As System.Windows.Forms.Label
	Private cboTourDeploy As System.Windows.Forms.ComboBox
	Private lbl11 As System.Windows.Forms.Label
	Private txtEspManas As System.Windows.Forms.TextBox
	Private txtN2 As System.Windows.Forms.TextBox
	Private lbl9 As System.Windows.Forms.Label
	Private cmdSimu2 As System.Windows.Forms.Button
	Private prgSimu2 As System.Windows.Forms.ProgressBar
	Private splitDeployH As System.Windows.Forms.SplitContainer
	Private prgSimu As System.Windows.Forms.ProgressBar
	Private lbl4 As System.Windows.Forms.Label
	Private cboTourCumul As System.Windows.Forms.ComboBox
	Private txtEspCumul As System.Windows.Forms.TextBox
	Private lbl8 As System.Windows.Forms.Label
	Private txtN As System.Windows.Forms.ToolStripTextBox
	Private lbl1 As System.Windows.Forms.Label
	Private splitCombosV As System.Windows.Forms.SplitContainer
	Private grpCombos As System.Windows.Forms.GroupBox
	Private grpDeploy As System.Windows.Forms.GroupBox
	Private btDeploy As TD.SandBar.ButtonItem
	Private btCombos As TD.SandBar.ButtonItem
	Private pnlSimus As TD.SandBar.ContainerBarClientPanel
	Private cbarSimus As TD.SandBar.ContainerBar
End Class
