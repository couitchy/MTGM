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
		Me.grpDeploy = New System.Windows.Forms.GroupBox
		Me.splitDeployH = New System.Windows.Forms.SplitContainer
		Me.lstUserCombos = New System.Windows.Forms.CheckedListBox
		Me.lbl12 = New System.Windows.Forms.Label
		Me.chkVerbosity = New System.Windows.Forms.CheckBox
		Me.cmdAddPlot2 = New System.Windows.Forms.Button
		Me.txtEspManas = New System.Windows.Forms.TextBox
		Me.prgSimu2 = New System.Windows.Forms.ProgressBar
		Me.lbl11 = New System.Windows.Forms.Label
		Me.cmdSimu2 = New System.Windows.Forms.Button
		Me.cboTourDeploy = New System.Windows.Forms.ComboBox
		Me.lbl9 = New System.Windows.Forms.Label
		Me.txtN2 = New System.Windows.Forms.TextBox
		Me.lbl10 = New System.Windows.Forms.Label
		Me.grpCombos = New System.Windows.Forms.GroupBox
		Me.splitCombosV = New System.Windows.Forms.SplitContainer
		Me.splitCombosH = New System.Windows.Forms.SplitContainer
		Me.lstCombosListe = New System.Windows.Forms.ListBox
		Me.cmdAddPlot = New System.Windows.Forms.Button
		Me.prgSimu = New System.Windows.Forms.ProgressBar
		Me.cmdSimu = New System.Windows.Forms.Button
		Me.lbl2 = New System.Windows.Forms.Label
		Me.txtN = New System.Windows.Forms.TextBox
		Me.lbl8 = New System.Windows.Forms.Label
		Me.lbl7 = New System.Windows.Forms.Label
		Me.txtEspCumul = New System.Windows.Forms.TextBox
		Me.txtEspSimple = New System.Windows.Forms.TextBox
		Me.cboTourCumul = New System.Windows.Forms.ComboBox
		Me.lbl6 = New System.Windows.Forms.Label
		Me.lbl5 = New System.Windows.Forms.Label
		Me.cboTourSimple = New System.Windows.Forms.ComboBox
		Me.lbl4 = New System.Windows.Forms.Label
		Me.lbl3 = New System.Windows.Forms.Label
		Me.lbl1 = New System.Windows.Forms.Label
		Me.grpMains = New System.Windows.Forms.GroupBox
		Me.splitMainsV = New System.Windows.Forms.SplitContainer
		Me.grdMainsTirage = New SourceGrid2.Grid
		Me.cmdMain = New System.Windows.Forms.Button
		Me.picScanCard = New System.Windows.Forms.PictureBox
		Me.btMains = New TD.SandBar.ButtonItem
		Me.btCombos = New TD.SandBar.ButtonItem
		Me.btDeploy = New TD.SandBar.ButtonItem
		Me.cmnuUserCombos = New System.Windows.Forms.ContextMenuStrip(Me.components)
		Me.cmnuAddNew = New System.Windows.Forms.ToolStripMenuItem
		Me.cmnuDelete = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuSeparator = New System.Windows.Forms.ToolStripSeparator
		Me.cmnuUp = New System.Windows.Forms.ToolStripMenuItem
		Me.cmnuDown = New System.Windows.Forms.ToolStripMenuItem
		Me.dlgVerbose = New System.Windows.Forms.SaveFileDialog
		Me.cbarSimus.SuspendLayout
		Me.pnlSimus.SuspendLayout
		Me.grpDeploy.SuspendLayout
		Me.splitDeployH.Panel1.SuspendLayout
		Me.splitDeployH.Panel2.SuspendLayout
		Me.splitDeployH.SuspendLayout
		Me.grpCombos.SuspendLayout
		Me.splitCombosV.Panel1.SuspendLayout
		Me.splitCombosV.Panel2.SuspendLayout
		Me.splitCombosV.SuspendLayout
		Me.splitCombosH.Panel1.SuspendLayout
		Me.splitCombosH.Panel2.SuspendLayout
		Me.splitCombosH.SuspendLayout
		Me.grpMains.SuspendLayout
		Me.splitMainsV.Panel1.SuspendLayout
		Me.splitMainsV.Panel2.SuspendLayout
		Me.splitMainsV.SuspendLayout
		CType(Me.picScanCard,System.ComponentModel.ISupportInitialize).BeginInit
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
		Me.cbarSimus.Items.AddRange(New TD.SandBar.ToolbarItemBase() {Me.btMains, Me.btCombos, Me.btDeploy})
		Me.cbarSimus.Location = New System.Drawing.Point(0, 0)
		Me.cbarSimus.Movable = false
		Me.cbarSimus.Name = "cbarSimus"
		Me.cbarSimus.Size = New System.Drawing.Size(408, 272)
		Me.cbarSimus.TabIndex = 0
		Me.cbarSimus.Text = "Modes de simulation"
		'
		'pnlSimus
		'
		Me.pnlSimus.Controls.Add(Me.grpDeploy)
		Me.pnlSimus.Controls.Add(Me.grpCombos)
		Me.pnlSimus.Controls.Add(Me.grpMains)
		Me.pnlSimus.Location = New System.Drawing.Point(2, 49)
		Me.pnlSimus.Name = "pnlSimus"
		Me.pnlSimus.Size = New System.Drawing.Size(404, 221)
		Me.pnlSimus.TabIndex = 0
		'
		'grpDeploy
		'
		Me.grpDeploy.BackColor = System.Drawing.Color.Transparent
		Me.grpDeploy.Controls.Add(Me.splitDeployH)
		Me.grpDeploy.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grpDeploy.Location = New System.Drawing.Point(0, 0)
		Me.grpDeploy.Name = "grpDeploy"
		Me.grpDeploy.Size = New System.Drawing.Size(404, 221)
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
		Me.splitDeployH.Panel1.Controls.Add(Me.lstUserCombos)
		Me.splitDeployH.Panel1.Controls.Add(Me.lbl12)
		'
		'splitDeployH.Panel2
		'
		Me.splitDeployH.Panel2.Controls.Add(Me.chkVerbosity)
		Me.splitDeployH.Panel2.Controls.Add(Me.cmdAddPlot2)
		Me.splitDeployH.Panel2.Controls.Add(Me.txtEspManas)
		Me.splitDeployH.Panel2.Controls.Add(Me.prgSimu2)
		Me.splitDeployH.Panel2.Controls.Add(Me.lbl11)
		Me.splitDeployH.Panel2.Controls.Add(Me.cmdSimu2)
		Me.splitDeployH.Panel2.Controls.Add(Me.cboTourDeploy)
		Me.splitDeployH.Panel2.Controls.Add(Me.lbl9)
		Me.splitDeployH.Panel2.Controls.Add(Me.txtN2)
		Me.splitDeployH.Panel2.Controls.Add(Me.lbl10)
		Me.splitDeployH.Size = New System.Drawing.Size(398, 202)
		Me.splitDeployH.SplitterDistance = 100
		Me.splitDeployH.TabIndex = 0
		'
		'lstUserCombos
		'
		Me.lstUserCombos.CheckOnClick = true
		Me.lstUserCombos.Dock = System.Windows.Forms.DockStyle.Fill
		Me.lstUserCombos.FormattingEnabled = true
		Me.lstUserCombos.Location = New System.Drawing.Point(0, 23)
		Me.lstUserCombos.Name = "lstUserCombos"
		Me.lstUserCombos.Size = New System.Drawing.Size(398, 64)
		Me.lstUserCombos.TabIndex = 23
		AddHandler Me.lstUserCombos.MouseUp, AddressOf Me.LstUserCombosMouseUp
		'
		'lbl12
		'
		Me.lbl12.Dock = System.Windows.Forms.DockStyle.Top
		Me.lbl12.Location = New System.Drawing.Point(0, 0)
		Me.lbl12.Name = "lbl12"
		Me.lbl12.Size = New System.Drawing.Size(398, 23)
		Me.lbl12.TabIndex = 22
		Me.lbl12.Text = "Cartes à effet spécifié manuellement (décocher pour les ignorer) :"
		'
		'chkVerbosity
		'
		Me.chkVerbosity.AutoSize = true
		Me.chkVerbosity.Location = New System.Drawing.Point(84, 30)
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
		Me.cmdAddPlot2.Location = New System.Drawing.Point(82, 57)
		Me.cmdAddPlot2.Name = "cmdAddPlot2"
		Me.cmdAddPlot2.Size = New System.Drawing.Size(56, 23)
		Me.cmdAddPlot2.TabIndex = 22
		Me.cmdAddPlot2.Text = "Graphe"
		Me.cmdAddPlot2.UseVisualStyleBackColor = true
		AddHandler Me.cmdAddPlot2.Click, AddressOf Me.CmdAddPlot2Click
		'
		'txtEspManas
		'
		Me.txtEspManas.Enabled = false
		Me.txtEspManas.Location = New System.Drawing.Point(316, 61)
		Me.txtEspManas.Name = "txtEspManas"
		Me.txtEspManas.Size = New System.Drawing.Size(61, 20)
		Me.txtEspManas.TabIndex = 11
		'
		'prgSimu2
		'
		Me.prgSimu2.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.prgSimu2.Location = New System.Drawing.Point(0, 84)
		Me.prgSimu2.Name = "prgSimu2"
		Me.prgSimu2.Size = New System.Drawing.Size(398, 14)
		Me.prgSimu2.TabIndex = 21
		Me.prgSimu2.Visible = false
		'
		'lbl11
		'
		Me.lbl11.AutoSize = true
		Me.lbl11.Location = New System.Drawing.Point(217, 40)
		Me.lbl11.Name = "lbl11"
		Me.lbl11.Size = New System.Drawing.Size(29, 13)
		Me.lbl11.TabIndex = 10
		Me.lbl11.Text = "Tour"
		'
		'cmdSimu2
		'
		Me.cmdSimu2.Location = New System.Drawing.Point(20, 57)
		Me.cmdSimu2.Name = "cmdSimu2"
		Me.cmdSimu2.Size = New System.Drawing.Size(56, 23)
		Me.cmdSimu2.TabIndex = 20
		Me.cmdSimu2.Text = "Simuler"
		Me.cmdSimu2.UseVisualStyleBackColor = true
		AddHandler Me.cmdSimu2.Click, AddressOf Me.CmdSimu2Click
		'
		'cboTourDeploy
		'
		Me.cboTourDeploy.FormattingEnabled = true
		Me.cboTourDeploy.Location = New System.Drawing.Point(316, 37)
		Me.cboTourDeploy.Name = "cboTourDeploy"
		Me.cboTourDeploy.Size = New System.Drawing.Size(61, 21)
		Me.cboTourDeploy.TabIndex = 9
		AddHandler Me.cboTourDeploy.SelectedIndexChanged, AddressOf Me.CboTourDeploySelectedIndexChanged
		'
		'lbl9
		'
		Me.lbl9.AutoSize = true
		Me.lbl9.Location = New System.Drawing.Point(20, 8)
		Me.lbl9.Name = "lbl9"
		Me.lbl9.Size = New System.Drawing.Size(99, 13)
		Me.lbl9.TabIndex = 19
		Me.lbl9.Text = "Nombre de parties :"
		'
		'txtN2
		'
		Me.txtN2.Location = New System.Drawing.Point(20, 28)
		Me.txtN2.Name = "txtN2"
		Me.txtN2.Size = New System.Drawing.Size(38, 20)
		Me.txtN2.TabIndex = 18
		Me.txtN2.Text = "1000"
		Me.txtN2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		AddHandler Me.txtN2.TextChanged, AddressOf Me.TxtN2TextChanged
		'
		'lbl10
		'
		Me.lbl10.Location = New System.Drawing.Point(217, 8)
		Me.lbl10.Name = "lbl10"
		Me.lbl10.Size = New System.Drawing.Size(160, 32)
		Me.lbl10.TabIndex = 8
		Me.lbl10.Text = "Espérance du nombre de manas productibles au tour n :"
		'
		'grpCombos
		'
		Me.grpCombos.BackColor = System.Drawing.Color.Transparent
		Me.grpCombos.Controls.Add(Me.splitCombosV)
		Me.grpCombos.Controls.Add(Me.lbl1)
		Me.grpCombos.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grpCombos.Location = New System.Drawing.Point(0, 0)
		Me.grpCombos.Name = "grpCombos"
		Me.grpCombos.Size = New System.Drawing.Size(404, 221)
		Me.grpCombos.TabIndex = 1
		Me.grpCombos.TabStop = false
		Me.grpCombos.Visible = false
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
		Me.splitCombosV.Panel1.Controls.Add(Me.splitCombosH)
		'
		'splitCombosV.Panel2
		'
		Me.splitCombosV.Panel2.Controls.Add(Me.lbl8)
		Me.splitCombosV.Panel2.Controls.Add(Me.lbl7)
		Me.splitCombosV.Panel2.Controls.Add(Me.txtEspCumul)
		Me.splitCombosV.Panel2.Controls.Add(Me.txtEspSimple)
		Me.splitCombosV.Panel2.Controls.Add(Me.cboTourCumul)
		Me.splitCombosV.Panel2.Controls.Add(Me.lbl6)
		Me.splitCombosV.Panel2.Controls.Add(Me.lbl5)
		Me.splitCombosV.Panel2.Controls.Add(Me.cboTourSimple)
		Me.splitCombosV.Panel2.Controls.Add(Me.lbl4)
		Me.splitCombosV.Panel2.Controls.Add(Me.lbl3)
		Me.splitCombosV.Size = New System.Drawing.Size(398, 185)
		Me.splitCombosV.SplitterDistance = 224
		Me.splitCombosV.TabIndex = 3
		'
		'splitCombosH
		'
		Me.splitCombosH.Dock = System.Windows.Forms.DockStyle.Fill
		Me.splitCombosH.IsSplitterFixed = true
		Me.splitCombosH.Location = New System.Drawing.Point(0, 0)
		Me.splitCombosH.Name = "splitCombosH"
		Me.splitCombosH.Orientation = System.Windows.Forms.Orientation.Horizontal
		'
		'splitCombosH.Panel1
		'
		Me.splitCombosH.Panel1.Controls.Add(Me.lstCombosListe)
		'
		'splitCombosH.Panel2
		'
		Me.splitCombosH.Panel2.Controls.Add(Me.cmdAddPlot)
		Me.splitCombosH.Panel2.Controls.Add(Me.prgSimu)
		Me.splitCombosH.Panel2.Controls.Add(Me.cmdSimu)
		Me.splitCombosH.Panel2.Controls.Add(Me.lbl2)
		Me.splitCombosH.Panel2.Controls.Add(Me.txtN)
		Me.splitCombosH.Size = New System.Drawing.Size(224, 185)
		Me.splitCombosH.SplitterDistance = 119
		Me.splitCombosH.TabIndex = 0
		'
		'lstCombosListe
		'
		Me.lstCombosListe.Dock = System.Windows.Forms.DockStyle.Fill
		Me.lstCombosListe.FormattingEnabled = true
		Me.lstCombosListe.Location = New System.Drawing.Point(0, 0)
		Me.lstCombosListe.Name = "lstCombosListe"
		Me.lstCombosListe.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
		Me.lstCombosListe.Size = New System.Drawing.Size(224, 108)
		Me.lstCombosListe.TabIndex = 1
		AddHandler Me.lstCombosListe.MouseUp, AddressOf Me.LstCombosListeMouseUp
		'
		'cmdAddPlot
		'
		Me.cmdAddPlot.Enabled = false
		Me.cmdAddPlot.Location = New System.Drawing.Point(164, 25)
		Me.cmdAddPlot.Name = "cmdAddPlot"
		Me.cmdAddPlot.Size = New System.Drawing.Size(56, 23)
		Me.cmdAddPlot.TabIndex = 14
		Me.cmdAddPlot.Text = "Graphes"
		Me.cmdAddPlot.UseVisualStyleBackColor = true
		AddHandler Me.cmdAddPlot.Click, AddressOf Me.CmdAddPlotClick
		'
		'prgSimu
		'
		Me.prgSimu.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.prgSimu.Location = New System.Drawing.Point(0, 48)
		Me.prgSimu.Name = "prgSimu"
		Me.prgSimu.Size = New System.Drawing.Size(224, 14)
		Me.prgSimu.TabIndex = 13
		Me.prgSimu.Visible = false
		'
		'cmdSimu
		'
		Me.cmdSimu.Location = New System.Drawing.Point(164, 2)
		Me.cmdSimu.Name = "cmdSimu"
		Me.cmdSimu.Size = New System.Drawing.Size(56, 23)
		Me.cmdSimu.TabIndex = 4
		Me.cmdSimu.Text = "Simuler"
		Me.cmdSimu.UseVisualStyleBackColor = true
		AddHandler Me.cmdSimu.Click, AddressOf Me.CmdSimuClick
		'
		'lbl2
		'
		Me.lbl2.AutoSize = true
		Me.lbl2.Location = New System.Drawing.Point(7, 19)
		Me.lbl2.Name = "lbl2"
		Me.lbl2.Size = New System.Drawing.Size(99, 13)
		Me.lbl2.TabIndex = 3
		Me.lbl2.Text = "Nombre de parties :"
		'
		'txtN
		'
		Me.txtN.Location = New System.Drawing.Point(112, 16)
		Me.txtN.Name = "txtN"
		Me.txtN.Size = New System.Drawing.Size(38, 20)
		Me.txtN.TabIndex = 1
		Me.txtN.Text = "1000"
		Me.txtN.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		'
		'lbl8
		'
		Me.lbl8.AutoSize = true
		Me.lbl8.Location = New System.Drawing.Point(115, 160)
		Me.lbl8.Name = "lbl8"
		Me.lbl8.Size = New System.Drawing.Size(15, 13)
		Me.lbl8.TabIndex = 10
		Me.lbl8.Text = "%"
		'
		'lbl7
		'
		Me.lbl7.AutoSize = true
		Me.lbl7.Location = New System.Drawing.Point(115, 74)
		Me.lbl7.Name = "lbl7"
		Me.lbl7.Size = New System.Drawing.Size(15, 13)
		Me.lbl7.TabIndex = 9
		Me.lbl7.Text = "%"
		'
		'txtEspCumul
		'
		Me.txtEspCumul.Enabled = false
		Me.txtEspCumul.Location = New System.Drawing.Point(48, 157)
		Me.txtEspCumul.Name = "txtEspCumul"
		Me.txtEspCumul.Size = New System.Drawing.Size(61, 20)
		Me.txtEspCumul.TabIndex = 8
		'
		'txtEspSimple
		'
		Me.txtEspSimple.Enabled = false
		Me.txtEspSimple.Location = New System.Drawing.Point(48, 71)
		Me.txtEspSimple.Name = "txtEspSimple"
		Me.txtEspSimple.Size = New System.Drawing.Size(61, 20)
		Me.txtEspSimple.TabIndex = 7
		'
		'cboTourCumul
		'
		Me.cboTourCumul.FormattingEnabled = true
		Me.cboTourCumul.Location = New System.Drawing.Point(48, 130)
		Me.cboTourCumul.Name = "cboTourCumul"
		Me.cboTourCumul.Size = New System.Drawing.Size(61, 21)
		Me.cboTourCumul.TabIndex = 6
		AddHandler Me.cboTourCumul.SelectedIndexChanged, AddressOf Me.CboTourCumulSelectedIndexChanged
		'
		'lbl6
		'
		Me.lbl6.AutoSize = true
		Me.lbl6.Location = New System.Drawing.Point(13, 133)
		Me.lbl6.Name = "lbl6"
		Me.lbl6.Size = New System.Drawing.Size(29, 13)
		Me.lbl6.TabIndex = 5
		Me.lbl6.Text = "Tour"
		'
		'lbl5
		'
		Me.lbl5.AutoSize = true
		Me.lbl5.Location = New System.Drawing.Point(13, 47)
		Me.lbl5.Name = "lbl5"
		Me.lbl5.Size = New System.Drawing.Size(29, 13)
		Me.lbl5.TabIndex = 4
		Me.lbl5.Text = "Tour"
		'
		'cboTourSimple
		'
		Me.cboTourSimple.FormattingEnabled = true
		Me.cboTourSimple.Location = New System.Drawing.Point(48, 44)
		Me.cboTourSimple.Name = "cboTourSimple"
		Me.cboTourSimple.Size = New System.Drawing.Size(61, 21)
		Me.cboTourSimple.TabIndex = 3
		AddHandler Me.cboTourSimple.SelectedIndexChanged, AddressOf Me.CboTourSimpleSelectedIndexChanged
		'
		'lbl4
		'
		Me.lbl4.Location = New System.Drawing.Point(13, 99)
		Me.lbl4.Name = "lbl4"
		Me.lbl4.Size = New System.Drawing.Size(152, 38)
		Me.lbl4.TabIndex = 2
		Me.lbl4.Text = "Probabilité de la combinaison au tour n :"
		'
		'lbl3
		'
		Me.lbl3.Location = New System.Drawing.Point(13, 11)
		Me.lbl3.Name = "lbl3"
		Me.lbl3.Size = New System.Drawing.Size(141, 38)
		Me.lbl3.TabIndex = 1
		Me.lbl3.Text = "Probabilité d'au moins une des cartes au tour n :"
		'
		'lbl1
		'
		Me.lbl1.Dock = System.Windows.Forms.DockStyle.Top
		Me.lbl1.Location = New System.Drawing.Point(3, 16)
		Me.lbl1.Name = "lbl1"
		Me.lbl1.Size = New System.Drawing.Size(398, 17)
		Me.lbl1.TabIndex = 2
		Me.lbl1.Text = "Sélectionner des cartes pour en estimer la probabilité d'apparition :"
		'
		'grpMains
		'
		Me.grpMains.BackColor = System.Drawing.Color.Transparent
		Me.grpMains.Controls.Add(Me.splitMainsV)
		Me.grpMains.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grpMains.Location = New System.Drawing.Point(0, 0)
		Me.grpMains.Name = "grpMains"
		Me.grpMains.Size = New System.Drawing.Size(404, 221)
		Me.grpMains.TabIndex = 0
		Me.grpMains.TabStop = false
		'
		'splitMainsV
		'
		Me.splitMainsV.Dock = System.Windows.Forms.DockStyle.Fill
		Me.splitMainsV.IsSplitterFixed = true
		Me.splitMainsV.Location = New System.Drawing.Point(3, 16)
		Me.splitMainsV.Name = "splitMainsV"
		'
		'splitMainsV.Panel1
		'
		Me.splitMainsV.Panel1.Controls.Add(Me.grdMainsTirage)
		Me.splitMainsV.Panel1.Controls.Add(Me.cmdMain)
		'
		'splitMainsV.Panel2
		'
		Me.splitMainsV.Panel2.Controls.Add(Me.picScanCard)
		Me.splitMainsV.Size = New System.Drawing.Size(398, 202)
		Me.splitMainsV.SplitterDistance = 256
		Me.splitMainsV.TabIndex = 0
		'
		'grdMainsTirage
		'
		Me.grdMainsTirage.AutoSizeMinHeight = 10
		Me.grdMainsTirage.AutoSizeMinWidth = 10
		Me.grdMainsTirage.AutoStretchColumnsToFitWidth = false
		Me.grdMainsTirage.AutoStretchRowsToFitHeight = false
		Me.grdMainsTirage.ContextMenuStyle = SourceGrid2.ContextMenuStyle.None
		Me.grdMainsTirage.CustomSort = false
		Me.grdMainsTirage.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grdMainsTirage.GridToolTipActive = true
		Me.grdMainsTirage.Location = New System.Drawing.Point(0, 23)
		Me.grdMainsTirage.Name = "grdMainsTirage"
		Me.grdMainsTirage.Size = New System.Drawing.Size(256, 179)
		Me.grdMainsTirage.SpecialKeys = CType(((((((((((SourceGrid2.GridSpecialKeys.Ctrl_C Or SourceGrid2.GridSpecialKeys.Ctrl_V)  _
						Or SourceGrid2.GridSpecialKeys.Ctrl_X)  _
						Or SourceGrid2.GridSpecialKeys.Delete)  _
						Or SourceGrid2.GridSpecialKeys.Arrows)  _
						Or SourceGrid2.GridSpecialKeys.Tab)  _
						Or SourceGrid2.GridSpecialKeys.PageDownUp)  _
						Or SourceGrid2.GridSpecialKeys.Enter)  _
						Or SourceGrid2.GridSpecialKeys.Escape)  _
						Or SourceGrid2.GridSpecialKeys.Control)  _
						Or SourceGrid2.GridSpecialKeys.Shift),SourceGrid2.GridSpecialKeys)
		Me.grdMainsTirage.TabIndex = 3
		'
		'cmdMain
		'
		Me.cmdMain.Dock = System.Windows.Forms.DockStyle.Top
		Me.cmdMain.Location = New System.Drawing.Point(0, 0)
		Me.cmdMain.Name = "cmdMain"
		Me.cmdMain.Size = New System.Drawing.Size(256, 23)
		Me.cmdMain.TabIndex = 2
		Me.cmdMain.Text = "Tirer une main !"
		Me.cmdMain.UseVisualStyleBackColor = true
		AddHandler Me.cmdMain.Click, AddressOf Me.CmdMainClick
		'
		'picScanCard
		'
		Me.picScanCard.Dock = System.Windows.Forms.DockStyle.Fill
		Me.picScanCard.Location = New System.Drawing.Point(0, 0)
		Me.picScanCard.Name = "picScanCard"
		Me.picScanCard.Size = New System.Drawing.Size(138, 202)
		Me.picScanCard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me.picScanCard.TabIndex = 39
		Me.picScanCard.TabStop = false
		'
		'btMains
		'
		Me.btMains.Icon = CType(resources.GetObject("btMains.Icon"),System.Drawing.Icon)
		Me.btMains.Text = "Mains aléatoires"
		AddHandler Me.btMains.Activate, AddressOf Me.BtMainsActivate
		'
		'btCombos
		'
		Me.btCombos.Icon = CType(resources.GetObject("btCombos.Icon"),System.Drawing.Icon)
		Me.btCombos.Text = "Probabilités de combos"
		AddHandler Me.btCombos.Activate, AddressOf Me.BtCombosActivate
		'
		'btDeploy
		'
		Me.btDeploy.Icon = CType(resources.GetObject("btDeploy.Icon"),System.Drawing.Icon)
		Me.btDeploy.Text = "Espérance de déploiement"
		AddHandler Me.btDeploy.Activate, AddressOf Me.BtDeployActivate
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
		'frmSimu
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(408, 272)
		Me.Controls.Add(Me.cbarSimus)
		Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
		Me.Name = "frmSimu"
		Me.Text = "Simulations"
		AddHandler Load, AddressOf Me.FrmSimuLoad
		Me.cbarSimus.ResumeLayout(false)
		Me.pnlSimus.ResumeLayout(false)
		Me.grpDeploy.ResumeLayout(false)
		Me.splitDeployH.Panel1.ResumeLayout(false)
		Me.splitDeployH.Panel2.ResumeLayout(false)
		Me.splitDeployH.Panel2.PerformLayout
		Me.splitDeployH.ResumeLayout(false)
		Me.grpCombos.ResumeLayout(false)
		Me.splitCombosV.Panel1.ResumeLayout(false)
		Me.splitCombosV.Panel2.ResumeLayout(false)
		Me.splitCombosV.Panel2.PerformLayout
		Me.splitCombosV.ResumeLayout(false)
		Me.splitCombosH.Panel1.ResumeLayout(false)
		Me.splitCombosH.Panel2.ResumeLayout(false)
		Me.splitCombosH.Panel2.PerformLayout
		Me.splitCombosH.ResumeLayout(false)
		Me.grpMains.ResumeLayout(false)
		Me.splitMainsV.Panel1.ResumeLayout(false)
		Me.splitMainsV.Panel2.ResumeLayout(false)
		Me.splitMainsV.ResumeLayout(false)
		CType(Me.picScanCard,System.ComponentModel.ISupportInitialize).EndInit
		Me.cmnuUserCombos.ResumeLayout(false)
		Me.ResumeLayout(false)
	End Sub
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
	Private cmdAddPlot As System.Windows.Forms.Button
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
	Private cmdMain As System.Windows.Forms.Button
	Private prgSimu As System.Windows.Forms.ProgressBar
	Private lbl3 As System.Windows.Forms.Label
	Private lbl4 As System.Windows.Forms.Label
	Private cboTourSimple As System.Windows.Forms.ComboBox
	Private lbl5 As System.Windows.Forms.Label
	Private lbl6 As System.Windows.Forms.Label
	Private cboTourCumul As System.Windows.Forms.ComboBox
	Private txtEspSimple As System.Windows.Forms.TextBox
	Private txtEspCumul As System.Windows.Forms.TextBox
	Private lbl7 As System.Windows.Forms.Label
	Private lbl8 As System.Windows.Forms.Label
	Private cmdSimu As System.Windows.Forms.Button
	Private txtN As System.Windows.Forms.TextBox
	Private lbl2 As System.Windows.Forms.Label
	Private splitCombosH As System.Windows.Forms.SplitContainer
	Private lbl1 As System.Windows.Forms.Label
	Private lstCombosListe As System.Windows.Forms.ListBox
	Private splitCombosV As System.Windows.Forms.SplitContainer
	Private splitMainsV As System.Windows.Forms.SplitContainer
	Private grdMainsTirage As SourceGrid2.Grid
	Private picScanCard As System.Windows.Forms.PictureBox
	Private grpMains As System.Windows.Forms.GroupBox
	Private grpCombos As System.Windows.Forms.GroupBox
	Private grpDeploy As System.Windows.Forms.GroupBox
	Private btDeploy As TD.SandBar.ButtonItem
	Private btCombos As TD.SandBar.ButtonItem
	Private btMains As TD.SandBar.ButtonItem
	Private pnlSimus As TD.SandBar.ContainerBarClientPanel
	Private cbarSimus As TD.SandBar.ContainerBar
End Class
