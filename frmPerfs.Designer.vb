'
' Created by SharpDevelop.
' User: Couitchy
' Date: 28/07/2008
' Time: 23:04
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Partial Class frmPerfs
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPerfs))
		Me.pnlGraph = New TD.SandBar.ContainerBarClientPanel
		Me.chartBreakDown = New SoftwareFX.ChartFX.Lite.Chart
		Me.cmnuChart = New System.Windows.Forms.ContextMenuStrip(Me.components)
		Me.mnuBreakDown = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuEvol = New System.Windows.Forms.ToolStripMenuItem
		Me.btDefNok = New TD.SandBar.ButtonItem
		Me.btVicNok = New TD.SandBar.ButtonItem
		Me.btDefOk = New TD.SandBar.ButtonItem
		Me.btVicOk = New TD.SandBar.ButtonItem
		Me.cboJeuAdv = New TD.SandBar.ComboBoxItem
		Me.cboJeuLocal = New TD.SandBar.ComboBoxItem
		Me.cbarGraph = New TD.SandBar.ContainerBar
		Me.cboLocalVersion = New TD.SandBar.ComboBoxItem
		Me.cboAdvVersion = New TD.SandBar.ComboBoxItem
		Me.btAdvice = New TD.SandBar.ButtonItem
		Me.btEfficiency = New TD.SandBar.ButtonItem
		Me.strStatus = New System.Windows.Forms.StatusStrip
		Me.dropAddGames = New System.Windows.Forms.ToolStripDropDownButton
		Me.dropGamesVersions = New System.Windows.Forms.ToolStripMenuItem
		Me.dropSeparator = New System.Windows.Forms.ToolStripSeparator
		Me.dropAddLocal = New System.Windows.Forms.ToolStripMenuItem
		Me.dropAddLocalOther = New System.Windows.Forms.ToolStripMenuItem
		Me.dropAddAdv = New System.Windows.Forms.ToolStripMenuItem
		Me.dropAddMixte = New System.Windows.Forms.ToolStripMenuItem
		Me.lblAllPlayed = New System.Windows.Forms.ToolStripStatusLabel
		Me.pnlGraph.SuspendLayout
		Me.cmnuChart.SuspendLayout
		Me.cbarGraph.SuspendLayout
		Me.strStatus.SuspendLayout
		Me.SuspendLayout
		'
		'pnlGraph
		'
		Me.pnlGraph.Controls.Add(Me.chartBreakDown)
		Me.pnlGraph.Location = New System.Drawing.Point(2, 71)
		Me.pnlGraph.Name = "pnlGraph"
		Me.pnlGraph.Size = New System.Drawing.Size(444, 202)
		Me.pnlGraph.TabIndex = 0
		'
		'chartBreakDown
		'
		Me.chartBreakDown.Chart3D = true
		Me.chartBreakDown.ContextMenuStrip = Me.cmnuChart
		Me.chartBreakDown.Dock = System.Windows.Forms.DockStyle.Fill
		Me.chartBreakDown.Gallery = SoftwareFX.ChartFX.Lite.Gallery.Pie
		Me.chartBreakDown.Location = New System.Drawing.Point(0, 0)
		Me.chartBreakDown.Name = "chartBreakDown"
		Me.chartBreakDown.NSeries = 1
		Me.chartBreakDown.NValues = 1
		Me.chartBreakDown.Size = New System.Drawing.Size(444, 202)
		Me.chartBreakDown.TabIndex = 0
		'
		'cmnuChart
		'
		Me.cmnuChart.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuBreakDown, Me.mnuEvol})
		Me.cmnuChart.Name = "cmnuChart"
		Me.cmnuChart.Size = New System.Drawing.Size(139, 48)
		AddHandler Me.cmnuChart.Opening, AddressOf Me.CmnuChartOpening
		'
		'mnuBreakDown
		'
		Me.mnuBreakDown.Checked = true
		Me.mnuBreakDown.CheckState = System.Windows.Forms.CheckState.Checked
		Me.mnuBreakDown.Name = "mnuBreakDown"
		Me.mnuBreakDown.Size = New System.Drawing.Size(138, 22)
		Me.mnuBreakDown.Text = "Répartition"
		AddHandler Me.mnuBreakDown.Click, AddressOf Me.MnuBreakDownClick
		'
		'mnuEvol
		'
		Me.mnuEvol.Enabled = false
		Me.mnuEvol.Name = "mnuEvol"
		Me.mnuEvol.Size = New System.Drawing.Size(138, 22)
		Me.mnuEvol.Text = "Evolution"
		AddHandler Me.mnuEvol.Click, AddressOf Me.MnuEvolClick
		'
		'btDefNok
		'
		Me.btDefNok.Icon = CType(resources.GetObject("btDefNok.Icon"),System.Drawing.Icon)
		Me.btDefNok.Text = "Défaite -1"
		AddHandler Me.btDefNok.Activate, AddressOf Me.BtDefNokActivate
		'
		'btVicNok
		'
		Me.btVicNok.Icon = CType(resources.GetObject("btVicNok.Icon"),System.Drawing.Icon)
		Me.btVicNok.Text = "Victoire -1"
		AddHandler Me.btVicNok.Activate, AddressOf Me.BtVicNokActivate
		'
		'btDefOk
		'
		Me.btDefOk.Icon = CType(resources.GetObject("btDefOk.Icon"),System.Drawing.Icon)
		Me.btDefOk.Text = "Défaite +1"
		AddHandler Me.btDefOk.Activate, AddressOf Me.BtDefOkActivate
		'
		'btVicOk
		'
		Me.btVicOk.Icon = CType(resources.GetObject("btVicOk.Icon"),System.Drawing.Icon)
		Me.btVicOk.Text = "Victoire +1"
		AddHandler Me.btVicOk.Activate, AddressOf Me.BtVicOkActivate
		'
		'cboJeuAdv
		'
		Me.cboJeuAdv.MinimumControlWidth = 130
		'
		'cboJeuLocal
		'
		Me.cboJeuLocal.MinimumControlWidth = 130
		'
		'cbarGraph
		'
		Me.cbarGraph.AddRemoveButtonsVisible = false
		Me.cbarGraph.Closable = false
		Me.cbarGraph.Controls.Add(Me.pnlGraph)
		Me.cbarGraph.Dock = System.Windows.Forms.DockStyle.Fill
		Me.cbarGraph.DrawActionsButton = false
		Me.cbarGraph.Flow = TD.SandBar.ToolBarLayout.Horizontal
		Me.cbarGraph.Guid = New System.Guid("4060aef7-8a2a-4cab-a9b1-52b8477a7366")
		Me.cbarGraph.Items.AddRange(New TD.SandBar.ToolbarItemBase() {Me.cboJeuLocal, Me.cboLocalVersion, Me.cboJeuAdv, Me.cboAdvVersion, Me.btAdvice, Me.btEfficiency, Me.btVicOk, Me.btDefOk, Me.btVicNok, Me.btDefNok})
		Me.cbarGraph.Location = New System.Drawing.Point(0, 0)
		Me.cbarGraph.Movable = false
		Me.cbarGraph.Name = "cbarGraph"
		Me.cbarGraph.Size = New System.Drawing.Size(448, 275)
		Me.cbarGraph.TabIndex = 1
		Me.cbarGraph.Text = "Résultats"
		'
		'cboLocalVersion
		'
		Me.cboLocalVersion.MinimumControlWidth = 82
		Me.cboLocalVersion.Visible = false
		'
		'cboAdvVersion
		'
		Me.cboAdvVersion.MinimumControlWidth = 82
		Me.cboAdvVersion.Visible = false
		'
		'btAdvice
		'
		Me.btAdvice.Icon = CType(resources.GetObject("btAdvice.Icon"),System.Drawing.Icon)
		Me.btAdvice.Text = "Conseils"
		AddHandler Me.btAdvice.Activate, AddressOf Me.BtAdviceActivate
		'
		'btEfficiency
		'
		Me.btEfficiency.Icon = CType(resources.GetObject("btEfficiency.Icon"),System.Drawing.Icon)
		Me.btEfficiency.Text = "Efficacité"
		AddHandler Me.btEfficiency.Activate, AddressOf Me.BtEfficiencyActivate
		'
		'strStatus
		'
		Me.strStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.dropAddGames, Me.lblAllPlayed})
		Me.strStatus.Location = New System.Drawing.Point(0, 253)
		Me.strStatus.Name = "strStatus"
		Me.strStatus.Size = New System.Drawing.Size(448, 22)
		Me.strStatus.TabIndex = 2
		Me.strStatus.Text = "statusStrip1"
		'
		'dropAddGames
		'
		Me.dropAddGames.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.dropGamesVersions, Me.dropSeparator, Me.dropAddLocal, Me.dropAddAdv, Me.dropAddMixte})
		Me.dropAddGames.Image = CType(resources.GetObject("dropAddGames.Image"),System.Drawing.Image)
		Me.dropAddGames.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.dropAddGames.Name = "dropAddGames"
		Me.dropAddGames.Size = New System.Drawing.Size(62, 20)
		Me.dropAddGames.Text = "Menu"
		'
		'dropGamesVersions
		'
		Me.dropGamesVersions.CheckOnClick = true
		Me.dropGamesVersions.Name = "dropGamesVersions"
		Me.dropGamesVersions.Size = New System.Drawing.Size(215, 22)
		Me.dropGamesVersions.Text = "Gérer les versions des jeux"
		AddHandler Me.dropGamesVersions.Click, AddressOf Me.DropGamesVersionsClick
		'
		'dropSeparator
		'
		Me.dropSeparator.Name = "dropSeparator"
		Me.dropSeparator.Size = New System.Drawing.Size(212, 6)
		'
		'dropAddLocal
		'
		Me.dropAddLocal.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.dropAddLocalOther})
		Me.dropAddLocal.Name = "dropAddLocal"
		Me.dropAddLocal.Size = New System.Drawing.Size(215, 22)
		Me.dropAddLocal.Text = "Ajouter jeu local"
		'
		'dropAddLocalOther
		'
		Me.dropAddLocalOther.Name = "dropAddLocalOther"
		Me.dropAddLocalOther.Size = New System.Drawing.Size(124, 22)
		Me.dropAddLocalOther.Text = "Autre..."
		AddHandler Me.dropAddLocalOther.Click, AddressOf Me.DropAddLocalOtherClick
		'
		'dropAddAdv
		'
		Me.dropAddAdv.Name = "dropAddAdv"
		Me.dropAddAdv.Size = New System.Drawing.Size(215, 22)
		Me.dropAddAdv.Text = "Ajouter jeu adverse"
		AddHandler Me.dropAddAdv.Click, AddressOf Me.DropAddAdvClick
		'
		'dropAddMixte
		'
		Me.dropAddMixte.Name = "dropAddMixte"
		Me.dropAddMixte.Size = New System.Drawing.Size(215, 22)
		Me.dropAddMixte.Text = "Ajouter jeu indépendant"
		AddHandler Me.dropAddMixte.Click, AddressOf Me.DropAddMixteClick
		'
		'lblAllPlayed
		'
		Me.lblAllPlayed.Name = "lblAllPlayed"
		Me.lblAllPlayed.Size = New System.Drawing.Size(0, 17)
		'
		'frmPerfs
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(448, 275)
		Me.Controls.Add(Me.strStatus)
		Me.Controls.Add(Me.cbarGraph)
		Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
		Me.MaximizeBox = false
		Me.Name = "frmPerfs"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Victoires & Défaites"
		AddHandler FormClosing, AddressOf Me.FrmPerfsFormClosing
		AddHandler Load, AddressOf Me.FrmPerfsLoad
		Me.pnlGraph.ResumeLayout(false)
		Me.cmnuChart.ResumeLayout(false)
		Me.cbarGraph.ResumeLayout(false)
		Me.strStatus.ResumeLayout(false)
		Me.strStatus.PerformLayout
		Me.ResumeLayout(false)
		Me.PerformLayout
	End Sub
	Private lblAllPlayed As System.Windows.Forms.ToolStripStatusLabel
	Private dropAddMixte As System.Windows.Forms.ToolStripMenuItem
	Private btEfficiency As TD.SandBar.ButtonItem
	Private mnuEvol As System.Windows.Forms.ToolStripMenuItem
	Private mnuBreakDown As System.Windows.Forms.ToolStripMenuItem
	Private cmnuChart As System.Windows.Forms.ContextMenuStrip
	Private cboLocalVersion As TD.SandBar.ComboBoxItem
	Private cboAdvVersion As TD.SandBar.ComboBoxItem
	Private dropSeparator As System.Windows.Forms.ToolStripSeparator
	Private dropGamesVersions As System.Windows.Forms.ToolStripMenuItem
	Private btAdvice As TD.SandBar.ButtonItem
	Private cboJeuLocal As TD.SandBar.ComboBoxItem
	Private cboJeuAdv As TD.SandBar.ComboBoxItem
	Private dropAddAdv As System.Windows.Forms.ToolStripMenuItem
	Private dropAddLocalOther As System.Windows.Forms.ToolStripMenuItem
	Private dropAddLocal As System.Windows.Forms.ToolStripMenuItem
	Private dropAddGames As System.Windows.Forms.ToolStripDropDownButton
	Private strStatus As System.Windows.Forms.StatusStrip
	Private btDefNok As TD.SandBar.ButtonItem
	Private btVicNok As TD.SandBar.ButtonItem
	Private btDefOk As TD.SandBar.ButtonItem
	Private btVicOk As TD.SandBar.ButtonItem
	Private chartBreakDown As SoftwareFX.ChartFX.Lite.Chart
	Private pnlGraph As TD.SandBar.ContainerBarClientPanel
	Private cbarGraph As TD.SandBar.ContainerBar
End Class
