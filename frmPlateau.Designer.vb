'
' Crée par SharpDevelop.
' Utilisateur: Couitchy
' Date: 17/11/2011
' Heure: 18:51
' 
' Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
'
Partial Class frmPlateau
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPlateau))
		Me.toolStrip = New System.Windows.Forms.ToolStrip
		Me.btNewPartie = New System.Windows.Forms.ToolStripButton
		Me.btMulligan = New System.Windows.Forms.ToolStripButton
		Me.btSeparator = New System.Windows.Forms.ToolStripSeparator
		Me.btInfos = New System.Windows.Forms.ToolStripButton
		Me.splitV1 = New System.Windows.Forms.SplitContainer
		Me.splitH3 = New System.Windows.Forms.SplitContainer
		Me.cbarBibli = New TD.SandBar.ContainerBar
		Me.cbarpanelBibli = New TD.SandBar.ContainerBarClientPanel
		Me.panelBibli = New System.Windows.Forms.Panel
		Me.toolStripBibli = New System.Windows.Forms.ToolStrip
		Me.btBibliShuffle = New System.Windows.Forms.ToolStripButton
		Me.btBibliReveal = New System.Windows.Forms.ToolStripButton
		Me.btBibliSearch = New System.Windows.Forms.ToolStripButton
		Me.splitH4 = New System.Windows.Forms.SplitContainer
		Me.cbarGraveyard = New TD.SandBar.ContainerBar
		Me.cbarpanelGraveyard = New TD.SandBar.ContainerBarClientPanel
		Me.panelGraveyard = New System.Windows.Forms.Panel
		Me.toolStripGraveyard = New System.Windows.Forms.ToolStrip
		Me.btGraveyardSearch = New System.Windows.Forms.ToolStripButton
		Me.cbarExil = New TD.SandBar.ContainerBar
		Me.cbarpanelExil = New TD.SandBar.ContainerBarClientPanel
		Me.panelExil = New System.Windows.Forms.Panel
		Me.toolStripExil = New System.Windows.Forms.ToolStrip
		Me.btExilSearch = New System.Windows.Forms.ToolStripButton
		Me.splitH1 = New System.Windows.Forms.SplitContainer
		Me.cbarRegard = New TD.SandBar.ContainerBar
		Me.cbarpanelRegard = New TD.SandBar.ContainerBarClientPanel
		Me.panelRegard = New System.Windows.Forms.Panel
		Me.toolStripRegard = New System.Windows.Forms.ToolStrip
		Me.btRegardShuffle = New System.Windows.Forms.ToolStripButton
		Me.splitH2 = New System.Windows.Forms.SplitContainer
		Me.cbarMain = New TD.SandBar.ContainerBar
		Me.cbarpanelMain = New TD.SandBar.ContainerBarClientPanel
		Me.panelMain = New System.Windows.Forms.Panel
		Me.toolStripMain = New System.Windows.Forms.ToolStrip
		Me.btMainShuffle = New System.Windows.Forms.ToolStripButton
		Me.cbarField = New TD.SandBar.ContainerBar
		Me.cbarpanelField = New TD.SandBar.ContainerBarClientPanel
		Me.panelField = New System.Windows.Forms.Panel
		Me.toolStripField = New System.Windows.Forms.ToolStrip
		Me.btFieldUntapAll = New System.Windows.Forms.ToolStripButton
		Me.cmnuCardContext = New System.Windows.Forms.ContextMenuStrip(Me.components)
		Me.cmnuSendTo = New System.Windows.Forms.ToolStripMenuItem
		Me.cmnuSendToBibliTop = New System.Windows.Forms.ToolStripMenuItem
		Me.cmnuSendToBibliBottom = New System.Windows.Forms.ToolStripMenuItem
		Me.cmnuSendToRegard = New System.Windows.Forms.ToolStripMenuItem
		Me.cmnuSendToMain = New System.Windows.Forms.ToolStripMenuItem
		Me.cmnuSendToField = New System.Windows.Forms.ToolStripMenuItem
		Me.cmnuSendToGraveyard = New System.Windows.Forms.ToolStripMenuItem
		Me.cmnuSendToExil = New System.Windows.Forms.ToolStripMenuItem
		Me.cmnuCounters = New System.Windows.Forms.ToolStripMenuItem
		Me.cmnuCountersAdd = New System.Windows.Forms.ToolStripMenuItem
		Me.cmnuCountersSub = New System.Windows.Forms.ToolStripMenuItem
		Me.cmnuTapUntap = New System.Windows.Forms.ToolStripMenuItem
		Me.toolStrip.SuspendLayout
		Me.splitV1.Panel1.SuspendLayout
		Me.splitV1.Panel2.SuspendLayout
		Me.splitV1.SuspendLayout
		Me.splitH3.Panel1.SuspendLayout
		Me.splitH3.Panel2.SuspendLayout
		Me.splitH3.SuspendLayout
		Me.cbarBibli.SuspendLayout
		Me.cbarpanelBibli.SuspendLayout
		Me.toolStripBibli.SuspendLayout
		Me.splitH4.Panel1.SuspendLayout
		Me.splitH4.Panel2.SuspendLayout
		Me.splitH4.SuspendLayout
		Me.cbarGraveyard.SuspendLayout
		Me.cbarpanelGraveyard.SuspendLayout
		Me.toolStripGraveyard.SuspendLayout
		Me.cbarExil.SuspendLayout
		Me.cbarpanelExil.SuspendLayout
		Me.toolStripExil.SuspendLayout
		Me.splitH1.Panel1.SuspendLayout
		Me.splitH1.Panel2.SuspendLayout
		Me.splitH1.SuspendLayout
		Me.cbarRegard.SuspendLayout
		Me.cbarpanelRegard.SuspendLayout
		Me.toolStripRegard.SuspendLayout
		Me.splitH2.Panel1.SuspendLayout
		Me.splitH2.Panel2.SuspendLayout
		Me.splitH2.SuspendLayout
		Me.cbarMain.SuspendLayout
		Me.cbarpanelMain.SuspendLayout
		Me.toolStripMain.SuspendLayout
		Me.cbarField.SuspendLayout
		Me.cbarpanelField.SuspendLayout
		Me.toolStripField.SuspendLayout
		Me.cmnuCardContext.SuspendLayout
		Me.SuspendLayout
		'
		'toolStrip
		'
		Me.toolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btNewPartie, Me.btMulligan, Me.btSeparator, Me.btInfos})
		Me.toolStrip.Location = New System.Drawing.Point(0, 0)
		Me.toolStrip.Name = "toolStrip"
		Me.toolStrip.Size = New System.Drawing.Size(727, 54)
		Me.toolStrip.TabIndex = 1
		'
		'btNewPartie
		'
		Me.btNewPartie.Image = CType(resources.GetObject("btNewPartie.Image"),System.Drawing.Image)
		Me.btNewPartie.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me.btNewPartie.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btNewPartie.Name = "btNewPartie"
		Me.btNewPartie.Size = New System.Drawing.Size(74, 51)
		Me.btNewPartie.Text = "Redistribuer"
		Me.btNewPartie.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		AddHandler Me.btNewPartie.Click, AddressOf Me.BtNewPartieClick
		'
		'btMulligan
		'
		Me.btMulligan.AutoSize = false
		Me.btMulligan.Image = CType(resources.GetObject("btMulligan.Image"),System.Drawing.Image)
		Me.btMulligan.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me.btMulligan.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btMulligan.Name = "btMulligan"
		Me.btMulligan.Size = New System.Drawing.Size(74, 51)
		Me.btMulligan.Text = "Mulligan"
		Me.btMulligan.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		'
		'btSeparator
		'
		Me.btSeparator.Name = "btSeparator"
		Me.btSeparator.Size = New System.Drawing.Size(6, 54)
		'
		'btInfos
		'
		Me.btInfos.AutoSize = false
		Me.btInfos.Image = CType(resources.GetObject("btInfos.Image"),System.Drawing.Image)
		Me.btInfos.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
		Me.btInfos.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btInfos.Name = "btInfos"
		Me.btInfos.Size = New System.Drawing.Size(74, 51)
		Me.btInfos.Text = "Infos."
		Me.btInfos.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		'
		'splitV1
		'
		Me.splitV1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.splitV1.Location = New System.Drawing.Point(0, 54)
		Me.splitV1.Name = "splitV1"
		'
		'splitV1.Panel1
		'
		Me.splitV1.Panel1.Controls.Add(Me.splitH3)
		'
		'splitV1.Panel2
		'
		Me.splitV1.Panel2.Controls.Add(Me.splitH1)
		Me.splitV1.Size = New System.Drawing.Size(727, 474)
		Me.splitV1.SplitterDistance = 243
		Me.splitV1.TabIndex = 2
		Me.splitV1.TabStop = false
		'
		'splitH3
		'
		Me.splitH3.Dock = System.Windows.Forms.DockStyle.Fill
		Me.splitH3.Location = New System.Drawing.Point(0, 0)
		Me.splitH3.Name = "splitH3"
		Me.splitH3.Orientation = System.Windows.Forms.Orientation.Horizontal
		'
		'splitH3.Panel1
		'
		Me.splitH3.Panel1.Controls.Add(Me.cbarBibli)
		'
		'splitH3.Panel2
		'
		Me.splitH3.Panel2.Controls.Add(Me.splitH4)
		Me.splitH3.Size = New System.Drawing.Size(243, 474)
		Me.splitH3.SplitterDistance = 132
		Me.splitH3.TabIndex = 0
		Me.splitH3.TabStop = false
		'
		'cbarBibli
		'
		Me.cbarBibli.Closable = false
		Me.cbarBibli.Controls.Add(Me.cbarpanelBibli)
		Me.cbarBibli.Dock = System.Windows.Forms.DockStyle.Fill
		Me.cbarBibli.DrawActionsButton = false
		Me.cbarBibli.Flow = TD.SandBar.ToolBarLayout.Horizontal
		Me.cbarBibli.Guid = New System.Guid("e32163aa-c429-40d7-a6ea-f4e8715c9ed8")
		Me.cbarBibli.Location = New System.Drawing.Point(0, 0)
		Me.cbarBibli.Movable = false
		Me.cbarBibli.Name = "cbarBibli"
		Me.cbarBibli.Size = New System.Drawing.Size(243, 132)
		Me.cbarBibli.TabIndex = 1
		Me.cbarBibli.Text = "Bibliothèque"
		'
		'cbarpanelBibli
		'
		Me.cbarpanelBibli.Controls.Add(Me.panelBibli)
		Me.cbarpanelBibli.Controls.Add(Me.toolStripBibli)
		Me.cbarpanelBibli.Location = New System.Drawing.Point(2, 27)
		Me.cbarpanelBibli.Name = "cbarpanelBibli"
		Me.cbarpanelBibli.Size = New System.Drawing.Size(239, 103)
		Me.cbarpanelBibli.TabIndex = 0
		'
		'panelBibli
		'
		Me.panelBibli.Dock = System.Windows.Forms.DockStyle.Fill
		Me.panelBibli.Location = New System.Drawing.Point(24, 0)
		Me.panelBibli.Name = "panelBibli"
		Me.panelBibli.Size = New System.Drawing.Size(215, 103)
		Me.panelBibli.TabIndex = 2
		'
		'toolStripBibli
		'
		Me.toolStripBibli.Dock = System.Windows.Forms.DockStyle.Left
		Me.toolStripBibli.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
		Me.toolStripBibli.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btBibliShuffle, Me.btBibliReveal, Me.btBibliSearch})
		Me.toolStripBibli.Location = New System.Drawing.Point(0, 0)
		Me.toolStripBibli.Name = "toolStripBibli"
		Me.toolStripBibli.Size = New System.Drawing.Size(24, 103)
		Me.toolStripBibli.TabIndex = 1
		'
		'btBibliShuffle
		'
		Me.btBibliShuffle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.btBibliShuffle.Image = CType(resources.GetObject("btBibliShuffle.Image"),System.Drawing.Image)
		Me.btBibliShuffle.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btBibliShuffle.Name = "btBibliShuffle"
		Me.btBibliShuffle.Size = New System.Drawing.Size(21, 20)
		Me.btBibliShuffle.Text = "Mélanger"
		AddHandler Me.btBibliShuffle.Click, AddressOf Me.BtBibliShuffleClick
		'
		'btBibliReveal
		'
		Me.btBibliReveal.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.btBibliReveal.Image = CType(resources.GetObject("btBibliReveal.Image"),System.Drawing.Image)
		Me.btBibliReveal.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btBibliReveal.Name = "btBibliReveal"
		Me.btBibliReveal.Size = New System.Drawing.Size(21, 20)
		Me.btBibliReveal.Text = "Révéler carte sommet"
		AddHandler Me.btBibliReveal.Click, AddressOf Me.BtBibliRevealClick
		'
		'btBibliSearch
		'
		Me.btBibliSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.btBibliSearch.Image = CType(resources.GetObject("btBibliSearch.Image"),System.Drawing.Image)
		Me.btBibliSearch.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btBibliSearch.Name = "btBibliSearch"
		Me.btBibliSearch.Size = New System.Drawing.Size(21, 20)
		Me.btBibliSearch.Text = "Rechercher"
		AddHandler Me.btBibliSearch.Click, AddressOf Me.BtBibliSearchClick
		'
		'splitH4
		'
		Me.splitH4.Dock = System.Windows.Forms.DockStyle.Fill
		Me.splitH4.Location = New System.Drawing.Point(0, 0)
		Me.splitH4.Name = "splitH4"
		Me.splitH4.Orientation = System.Windows.Forms.Orientation.Horizontal
		'
		'splitH4.Panel1
		'
		Me.splitH4.Panel1.Controls.Add(Me.cbarGraveyard)
		'
		'splitH4.Panel2
		'
		Me.splitH4.Panel2.Controls.Add(Me.cbarExil)
		Me.splitH4.Size = New System.Drawing.Size(243, 338)
		Me.splitH4.SplitterDistance = 163
		Me.splitH4.TabIndex = 0
		Me.splitH4.TabStop = false
		'
		'cbarGraveyard
		'
		Me.cbarGraveyard.Closable = false
		Me.cbarGraveyard.Controls.Add(Me.cbarpanelGraveyard)
		Me.cbarGraveyard.Dock = System.Windows.Forms.DockStyle.Fill
		Me.cbarGraveyard.DrawActionsButton = false
		Me.cbarGraveyard.Flow = TD.SandBar.ToolBarLayout.Horizontal
		Me.cbarGraveyard.Guid = New System.Guid("e32163aa-c429-40d7-a6ea-f4e8715c9ed8")
		Me.cbarGraveyard.Location = New System.Drawing.Point(0, 0)
		Me.cbarGraveyard.Movable = false
		Me.cbarGraveyard.Name = "cbarGraveyard"
		Me.cbarGraveyard.Size = New System.Drawing.Size(243, 163)
		Me.cbarGraveyard.TabIndex = 1
		Me.cbarGraveyard.Text = "Cimetière"
		'
		'cbarpanelGraveyard
		'
		Me.cbarpanelGraveyard.Controls.Add(Me.panelGraveyard)
		Me.cbarpanelGraveyard.Controls.Add(Me.toolStripGraveyard)
		Me.cbarpanelGraveyard.Location = New System.Drawing.Point(2, 27)
		Me.cbarpanelGraveyard.Name = "cbarpanelGraveyard"
		Me.cbarpanelGraveyard.Size = New System.Drawing.Size(239, 134)
		Me.cbarpanelGraveyard.TabIndex = 0
		'
		'panelGraveyard
		'
		Me.panelGraveyard.Dock = System.Windows.Forms.DockStyle.Fill
		Me.panelGraveyard.Location = New System.Drawing.Point(24, 0)
		Me.panelGraveyard.Name = "panelGraveyard"
		Me.panelGraveyard.Size = New System.Drawing.Size(215, 134)
		Me.panelGraveyard.TabIndex = 3
		'
		'toolStripGraveyard
		'
		Me.toolStripGraveyard.Dock = System.Windows.Forms.DockStyle.Left
		Me.toolStripGraveyard.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
		Me.toolStripGraveyard.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btGraveyardSearch})
		Me.toolStripGraveyard.Location = New System.Drawing.Point(0, 0)
		Me.toolStripGraveyard.Name = "toolStripGraveyard"
		Me.toolStripGraveyard.Size = New System.Drawing.Size(24, 134)
		Me.toolStripGraveyard.TabIndex = 2
		'
		'btGraveyardSearch
		'
		Me.btGraveyardSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.btGraveyardSearch.Image = CType(resources.GetObject("btGraveyardSearch.Image"),System.Drawing.Image)
		Me.btGraveyardSearch.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btGraveyardSearch.Name = "btGraveyardSearch"
		Me.btGraveyardSearch.Size = New System.Drawing.Size(21, 20)
		Me.btGraveyardSearch.Text = "Rechercher"
		'
		'cbarExil
		'
		Me.cbarExil.Closable = false
		Me.cbarExil.Controls.Add(Me.cbarpanelExil)
		Me.cbarExil.Dock = System.Windows.Forms.DockStyle.Fill
		Me.cbarExil.DrawActionsButton = false
		Me.cbarExil.Flow = TD.SandBar.ToolBarLayout.Horizontal
		Me.cbarExil.Guid = New System.Guid("e32163aa-c429-40d7-a6ea-f4e8715c9ed8")
		Me.cbarExil.Location = New System.Drawing.Point(0, 0)
		Me.cbarExil.Movable = false
		Me.cbarExil.Name = "cbarExil"
		Me.cbarExil.Size = New System.Drawing.Size(243, 171)
		Me.cbarExil.TabIndex = 1
		Me.cbarExil.Text = "Exil"
		'
		'cbarpanelExil
		'
		Me.cbarpanelExil.Controls.Add(Me.panelExil)
		Me.cbarpanelExil.Controls.Add(Me.toolStripExil)
		Me.cbarpanelExil.Location = New System.Drawing.Point(2, 27)
		Me.cbarpanelExil.Name = "cbarpanelExil"
		Me.cbarpanelExil.Size = New System.Drawing.Size(239, 142)
		Me.cbarpanelExil.TabIndex = 0
		'
		'panelExil
		'
		Me.panelExil.Dock = System.Windows.Forms.DockStyle.Fill
		Me.panelExil.Location = New System.Drawing.Point(24, 0)
		Me.panelExil.Name = "panelExil"
		Me.panelExil.Size = New System.Drawing.Size(215, 142)
		Me.panelExil.TabIndex = 3
		'
		'toolStripExil
		'
		Me.toolStripExil.Dock = System.Windows.Forms.DockStyle.Left
		Me.toolStripExil.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
		Me.toolStripExil.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btExilSearch})
		Me.toolStripExil.Location = New System.Drawing.Point(0, 0)
		Me.toolStripExil.Name = "toolStripExil"
		Me.toolStripExil.Size = New System.Drawing.Size(24, 142)
		Me.toolStripExil.TabIndex = 2
		'
		'btExilSearch
		'
		Me.btExilSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.btExilSearch.Image = CType(resources.GetObject("btExilSearch.Image"),System.Drawing.Image)
		Me.btExilSearch.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btExilSearch.Name = "btExilSearch"
		Me.btExilSearch.Size = New System.Drawing.Size(21, 20)
		Me.btExilSearch.Text = "Rechercher"
		'
		'splitH1
		'
		Me.splitH1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.splitH1.Location = New System.Drawing.Point(0, 0)
		Me.splitH1.Name = "splitH1"
		Me.splitH1.Orientation = System.Windows.Forms.Orientation.Horizontal
		'
		'splitH1.Panel1
		'
		Me.splitH1.Panel1.Controls.Add(Me.cbarRegard)
		'
		'splitH1.Panel2
		'
		Me.splitH1.Panel2.Controls.Add(Me.splitH2)
		Me.splitH1.Size = New System.Drawing.Size(480, 474)
		Me.splitH1.SplitterDistance = 130
		Me.splitH1.TabIndex = 0
		Me.splitH1.TabStop = false
		'
		'cbarRegard
		'
		Me.cbarRegard.Closable = false
		Me.cbarRegard.Controls.Add(Me.cbarpanelRegard)
		Me.cbarRegard.Dock = System.Windows.Forms.DockStyle.Fill
		Me.cbarRegard.DrawActionsButton = false
		Me.cbarRegard.Flow = TD.SandBar.ToolBarLayout.Horizontal
		Me.cbarRegard.Guid = New System.Guid("e32163aa-c429-40d7-a6ea-f4e8715c9ed8")
		Me.cbarRegard.Location = New System.Drawing.Point(0, 0)
		Me.cbarRegard.Movable = false
		Me.cbarRegard.Name = "cbarRegard"
		Me.cbarRegard.Size = New System.Drawing.Size(480, 130)
		Me.cbarRegard.TabIndex = 0
		Me.cbarRegard.Text = "Regard"
		'
		'cbarpanelRegard
		'
		Me.cbarpanelRegard.Controls.Add(Me.panelRegard)
		Me.cbarpanelRegard.Controls.Add(Me.toolStripRegard)
		Me.cbarpanelRegard.Location = New System.Drawing.Point(2, 27)
		Me.cbarpanelRegard.Name = "cbarpanelRegard"
		Me.cbarpanelRegard.Size = New System.Drawing.Size(476, 101)
		Me.cbarpanelRegard.TabIndex = 0
		'
		'panelRegard
		'
		Me.panelRegard.Dock = System.Windows.Forms.DockStyle.Fill
		Me.panelRegard.Location = New System.Drawing.Point(24, 0)
		Me.panelRegard.Name = "panelRegard"
		Me.panelRegard.Size = New System.Drawing.Size(452, 101)
		Me.panelRegard.TabIndex = 3
		'
		'toolStripRegard
		'
		Me.toolStripRegard.Dock = System.Windows.Forms.DockStyle.Left
		Me.toolStripRegard.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
		Me.toolStripRegard.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btRegardShuffle})
		Me.toolStripRegard.Location = New System.Drawing.Point(0, 0)
		Me.toolStripRegard.Name = "toolStripRegard"
		Me.toolStripRegard.Size = New System.Drawing.Size(24, 101)
		Me.toolStripRegard.TabIndex = 2
		'
		'btRegardShuffle
		'
		Me.btRegardShuffle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.btRegardShuffle.Image = CType(resources.GetObject("btRegardShuffle.Image"),System.Drawing.Image)
		Me.btRegardShuffle.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btRegardShuffle.Name = "btRegardShuffle"
		Me.btRegardShuffle.Size = New System.Drawing.Size(21, 20)
		Me.btRegardShuffle.Text = "Mélanger"
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
		Me.splitH2.Panel1.Controls.Add(Me.cbarMain)
		'
		'splitH2.Panel2
		'
		Me.splitH2.Panel2.Controls.Add(Me.cbarField)
		Me.splitH2.Size = New System.Drawing.Size(480, 340)
		Me.splitH2.SplitterDistance = 131
		Me.splitH2.TabIndex = 0
		Me.splitH2.TabStop = false
		'
		'cbarMain
		'
		Me.cbarMain.Closable = false
		Me.cbarMain.Controls.Add(Me.cbarpanelMain)
		Me.cbarMain.Dock = System.Windows.Forms.DockStyle.Fill
		Me.cbarMain.DrawActionsButton = false
		Me.cbarMain.Flow = TD.SandBar.ToolBarLayout.Horizontal
		Me.cbarMain.Guid = New System.Guid("e32163aa-c429-40d7-a6ea-f4e8715c9ed8")
		Me.cbarMain.Location = New System.Drawing.Point(0, 0)
		Me.cbarMain.Movable = false
		Me.cbarMain.Name = "cbarMain"
		Me.cbarMain.Size = New System.Drawing.Size(480, 131)
		Me.cbarMain.TabIndex = 1
		Me.cbarMain.Text = "Main"
		'
		'cbarpanelMain
		'
		Me.cbarpanelMain.Controls.Add(Me.panelMain)
		Me.cbarpanelMain.Controls.Add(Me.toolStripMain)
		Me.cbarpanelMain.Location = New System.Drawing.Point(2, 27)
		Me.cbarpanelMain.Name = "cbarpanelMain"
		Me.cbarpanelMain.Size = New System.Drawing.Size(476, 102)
		Me.cbarpanelMain.TabIndex = 0
		'
		'panelMain
		'
		Me.panelMain.Dock = System.Windows.Forms.DockStyle.Fill
		Me.panelMain.Location = New System.Drawing.Point(24, 0)
		Me.panelMain.Name = "panelMain"
		Me.panelMain.Size = New System.Drawing.Size(452, 102)
		Me.panelMain.TabIndex = 3
		'
		'toolStripMain
		'
		Me.toolStripMain.Dock = System.Windows.Forms.DockStyle.Left
		Me.toolStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
		Me.toolStripMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btMainShuffle})
		Me.toolStripMain.Location = New System.Drawing.Point(0, 0)
		Me.toolStripMain.Name = "toolStripMain"
		Me.toolStripMain.Size = New System.Drawing.Size(24, 102)
		Me.toolStripMain.TabIndex = 2
		'
		'btMainShuffle
		'
		Me.btMainShuffle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.btMainShuffle.Image = CType(resources.GetObject("btMainShuffle.Image"),System.Drawing.Image)
		Me.btMainShuffle.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btMainShuffle.Name = "btMainShuffle"
		Me.btMainShuffle.Size = New System.Drawing.Size(21, 20)
		Me.btMainShuffle.Text = "Mélanger"
		AddHandler Me.btMainShuffle.Click, AddressOf Me.BtMainShuffleClick
		'
		'cbarField
		'
		Me.cbarField.Closable = false
		Me.cbarField.Controls.Add(Me.cbarpanelField)
		Me.cbarField.Dock = System.Windows.Forms.DockStyle.Fill
		Me.cbarField.DrawActionsButton = false
		Me.cbarField.Flow = TD.SandBar.ToolBarLayout.Horizontal
		Me.cbarField.Guid = New System.Guid("e32163aa-c429-40d7-a6ea-f4e8715c9ed8")
		Me.cbarField.Location = New System.Drawing.Point(0, 0)
		Me.cbarField.Movable = false
		Me.cbarField.Name = "cbarField"
		Me.cbarField.Size = New System.Drawing.Size(480, 205)
		Me.cbarField.TabIndex = 1
		Me.cbarField.Text = "Champ de bataille"
		'
		'cbarpanelField
		'
		Me.cbarpanelField.Controls.Add(Me.panelField)
		Me.cbarpanelField.Controls.Add(Me.toolStripField)
		Me.cbarpanelField.Location = New System.Drawing.Point(2, 27)
		Me.cbarpanelField.Name = "cbarpanelField"
		Me.cbarpanelField.Size = New System.Drawing.Size(476, 176)
		Me.cbarpanelField.TabIndex = 0
		'
		'panelField
		'
		Me.panelField.Dock = System.Windows.Forms.DockStyle.Fill
		Me.panelField.Location = New System.Drawing.Point(24, 0)
		Me.panelField.Name = "panelField"
		Me.panelField.Size = New System.Drawing.Size(452, 176)
		Me.panelField.TabIndex = 3
		'
		'toolStripField
		'
		Me.toolStripField.Dock = System.Windows.Forms.DockStyle.Left
		Me.toolStripField.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
		Me.toolStripField.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btFieldUntapAll})
		Me.toolStripField.Location = New System.Drawing.Point(0, 0)
		Me.toolStripField.Name = "toolStripField"
		Me.toolStripField.Size = New System.Drawing.Size(24, 176)
		Me.toolStripField.TabIndex = 2
		'
		'btFieldUntapAll
		'
		Me.btFieldUntapAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.btFieldUntapAll.Image = CType(resources.GetObject("btFieldUntapAll.Image"),System.Drawing.Image)
		Me.btFieldUntapAll.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btFieldUntapAll.Name = "btFieldUntapAll"
		Me.btFieldUntapAll.Size = New System.Drawing.Size(21, 20)
		Me.btFieldUntapAll.Text = "Tout dégager"
		AddHandler Me.btFieldUntapAll.Click, AddressOf Me.BtFieldUntapAllClick
		'
		'cmnuCardContext
		'
		Me.cmnuCardContext.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmnuSendTo, Me.cmnuCounters, Me.cmnuTapUntap})
		Me.cmnuCardContext.Name = "cmnuCardContext"
		Me.cmnuCardContext.Size = New System.Drawing.Size(173, 92)
		'
		'cmnuSendTo
		'
		Me.cmnuSendTo.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmnuSendToBibliTop, Me.cmnuSendToBibliBottom, Me.cmnuSendToRegard, Me.cmnuSendToMain, Me.cmnuSendToField, Me.cmnuSendToGraveyard, Me.cmnuSendToExil})
		Me.cmnuSendTo.Name = "cmnuSendTo"
		Me.cmnuSendTo.Size = New System.Drawing.Size(172, 22)
		Me.cmnuSendTo.Text = "Envoyer vers..."
		'
		'cmnuSendToBibliTop
		'
		Me.cmnuSendToBibliTop.Name = "cmnuSendToBibliTop"
		Me.cmnuSendToBibliTop.Size = New System.Drawing.Size(194, 22)
		Me.cmnuSendToBibliTop.Text = "Bibliothèque (dessus)"
		'
		'cmnuSendToBibliBottom
		'
		Me.cmnuSendToBibliBottom.Name = "cmnuSendToBibliBottom"
		Me.cmnuSendToBibliBottom.Size = New System.Drawing.Size(194, 22)
		Me.cmnuSendToBibliBottom.Text = "Bibliothèque (dessous)"
		'
		'cmnuSendToRegard
		'
		Me.cmnuSendToRegard.Name = "cmnuSendToRegard"
		Me.cmnuSendToRegard.Size = New System.Drawing.Size(194, 22)
		Me.cmnuSendToRegard.Text = "Regard"
		'
		'cmnuSendToMain
		'
		Me.cmnuSendToMain.Name = "cmnuSendToMain"
		Me.cmnuSendToMain.Size = New System.Drawing.Size(194, 22)
		Me.cmnuSendToMain.Text = "Main"
		'
		'cmnuSendToField
		'
		Me.cmnuSendToField.Name = "cmnuSendToField"
		Me.cmnuSendToField.Size = New System.Drawing.Size(194, 22)
		Me.cmnuSendToField.Text = "Champ de bataille"
		'
		'cmnuSendToGraveyard
		'
		Me.cmnuSendToGraveyard.Name = "cmnuSendToGraveyard"
		Me.cmnuSendToGraveyard.Size = New System.Drawing.Size(194, 22)
		Me.cmnuSendToGraveyard.Text = "Cimetière"
		'
		'cmnuSendToExil
		'
		Me.cmnuSendToExil.Name = "cmnuSendToExil"
		Me.cmnuSendToExil.Size = New System.Drawing.Size(194, 22)
		Me.cmnuSendToExil.Text = "Exil"
		'
		'cmnuCounters
		'
		Me.cmnuCounters.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmnuCountersAdd, Me.cmnuCountersSub})
		Me.cmnuCounters.Name = "cmnuCounters"
		Me.cmnuCounters.Size = New System.Drawing.Size(172, 22)
		Me.cmnuCounters.Text = "Marqueurs..."
		'
		'cmnuCountersAdd
		'
		Me.cmnuCountersAdd.Name = "cmnuCountersAdd"
		Me.cmnuCountersAdd.Size = New System.Drawing.Size(152, 22)
		Me.cmnuCountersAdd.Text = "Ajouter"
		AddHandler Me.cmnuCountersAdd.Click, AddressOf Me.CmnuCountersAddClick
		'
		'cmnuCountersSub
		'
		Me.cmnuCountersSub.Name = "cmnuCountersSub"
		Me.cmnuCountersSub.Size = New System.Drawing.Size(152, 22)
		Me.cmnuCountersSub.Text = "Retirer"
		AddHandler Me.cmnuCountersSub.Click, AddressOf Me.CmnuCountersSubClick
		'
		'cmnuTapUntap
		'
		Me.cmnuTapUntap.Name = "cmnuTapUntap"
		Me.cmnuTapUntap.Size = New System.Drawing.Size(172, 22)
		Me.cmnuTapUntap.Text = "Engager / Dégager"
		'
		'frmPlateau
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(727, 528)
		Me.Controls.Add(Me.splitV1)
		Me.Controls.Add(Me.toolStrip)
		Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
		Me.Name = "frmPlateau"
		Me.Text = "Plateau de jeu"
		Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
		AddHandler Load, AddressOf Me.FrmPlateauLoad
		AddHandler ResizeEnd, AddressOf Me.FrmPlateauResizeEnd
		Me.toolStrip.ResumeLayout(false)
		Me.toolStrip.PerformLayout
		Me.splitV1.Panel1.ResumeLayout(false)
		Me.splitV1.Panel2.ResumeLayout(false)
		Me.splitV1.ResumeLayout(false)
		Me.splitH3.Panel1.ResumeLayout(false)
		Me.splitH3.Panel2.ResumeLayout(false)
		Me.splitH3.ResumeLayout(false)
		Me.cbarBibli.ResumeLayout(false)
		Me.cbarpanelBibli.ResumeLayout(false)
		Me.cbarpanelBibli.PerformLayout
		Me.toolStripBibli.ResumeLayout(false)
		Me.toolStripBibli.PerformLayout
		Me.splitH4.Panel1.ResumeLayout(false)
		Me.splitH4.Panel2.ResumeLayout(false)
		Me.splitH4.ResumeLayout(false)
		Me.cbarGraveyard.ResumeLayout(false)
		Me.cbarpanelGraveyard.ResumeLayout(false)
		Me.cbarpanelGraveyard.PerformLayout
		Me.toolStripGraveyard.ResumeLayout(false)
		Me.toolStripGraveyard.PerformLayout
		Me.cbarExil.ResumeLayout(false)
		Me.cbarpanelExil.ResumeLayout(false)
		Me.cbarpanelExil.PerformLayout
		Me.toolStripExil.ResumeLayout(false)
		Me.toolStripExil.PerformLayout
		Me.splitH1.Panel1.ResumeLayout(false)
		Me.splitH1.Panel2.ResumeLayout(false)
		Me.splitH1.ResumeLayout(false)
		Me.cbarRegard.ResumeLayout(false)
		Me.cbarpanelRegard.ResumeLayout(false)
		Me.cbarpanelRegard.PerformLayout
		Me.toolStripRegard.ResumeLayout(false)
		Me.toolStripRegard.PerformLayout
		Me.splitH2.Panel1.ResumeLayout(false)
		Me.splitH2.Panel2.ResumeLayout(false)
		Me.splitH2.ResumeLayout(false)
		Me.cbarMain.ResumeLayout(false)
		Me.cbarpanelMain.ResumeLayout(false)
		Me.cbarpanelMain.PerformLayout
		Me.toolStripMain.ResumeLayout(false)
		Me.toolStripMain.PerformLayout
		Me.cbarField.ResumeLayout(false)
		Me.cbarpanelField.ResumeLayout(false)
		Me.cbarpanelField.PerformLayout
		Me.toolStripField.ResumeLayout(false)
		Me.toolStripField.PerformLayout
		Me.cmnuCardContext.ResumeLayout(false)
		Me.ResumeLayout(false)
		Me.PerformLayout
	End Sub
	Private cmnuTapUntap As System.Windows.Forms.ToolStripMenuItem
	Private cmnuCountersSub As System.Windows.Forms.ToolStripMenuItem
	Private cmnuCountersAdd As System.Windows.Forms.ToolStripMenuItem
	Private cmnuCounters As System.Windows.Forms.ToolStripMenuItem
	Private cmnuSendToExil As System.Windows.Forms.ToolStripMenuItem
	Private cmnuSendToGraveyard As System.Windows.Forms.ToolStripMenuItem
	Private cmnuSendToField As System.Windows.Forms.ToolStripMenuItem
	Private cmnuSendToMain As System.Windows.Forms.ToolStripMenuItem
	Private cmnuSendToRegard As System.Windows.Forms.ToolStripMenuItem
	Private cmnuSendToBibliBottom As System.Windows.Forms.ToolStripMenuItem
	Private cmnuSendToBibliTop As System.Windows.Forms.ToolStripMenuItem
	Private cmnuSendTo As System.Windows.Forms.ToolStripMenuItem
	Private cmnuCardContext As System.Windows.Forms.ContextMenuStrip
	Private btBibliSearch As System.Windows.Forms.ToolStripButton
	Private btBibliReveal As System.Windows.Forms.ToolStripButton
	Private toolStripBibli As System.Windows.Forms.ToolStrip
	Private btFieldUntapAll As System.Windows.Forms.ToolStripButton
	Private toolStripField As System.Windows.Forms.ToolStrip
	Private panelField As System.Windows.Forms.Panel
	Private btMainShuffle As System.Windows.Forms.ToolStripButton
	Private toolStripMain As System.Windows.Forms.ToolStrip
	Private panelMain As System.Windows.Forms.Panel
	Private btRegardShuffle As System.Windows.Forms.ToolStripButton
	Private toolStripRegard As System.Windows.Forms.ToolStrip
	Private panelRegard As System.Windows.Forms.Panel
	Private btExilSearch As System.Windows.Forms.ToolStripButton
	Private toolStripExil As System.Windows.Forms.ToolStrip
	Private panelExil As System.Windows.Forms.Panel
	Private btGraveyardSearch As System.Windows.Forms.ToolStripButton
	Private toolStripGraveyard As System.Windows.Forms.ToolStrip
	Private panelGraveyard As System.Windows.Forms.Panel
	Private panelBibli As System.Windows.Forms.Panel
	Private btBibliShuffle As System.Windows.Forms.ToolStripButton
	Private btInfos As System.Windows.Forms.ToolStripButton
	Private btSeparator As System.Windows.Forms.ToolStripSeparator
	Private btMulligan As System.Windows.Forms.ToolStripButton
	Private toolStrip As System.Windows.Forms.ToolStrip
	Private btNewPartie As System.Windows.Forms.ToolStripButton
	Private cbarBibli As TD.SandBar.ContainerBar
	Private cbarpanelBibli As TD.SandBar.ContainerBarClientPanel
	Private splitH2 As System.Windows.Forms.SplitContainer
	Private splitH1 As System.Windows.Forms.SplitContainer
	Private splitH4 As System.Windows.Forms.SplitContainer
	Private splitH3 As System.Windows.Forms.SplitContainer
	Private splitV1 As System.Windows.Forms.SplitContainer
	Private cbarpanelField As TD.SandBar.ContainerBarClientPanel
	Private cbarField As TD.SandBar.ContainerBar
	Private cbarpanelMain As TD.SandBar.ContainerBarClientPanel
	Private cbarMain As TD.SandBar.ContainerBar
	Private cbarpanelRegard As TD.SandBar.ContainerBarClientPanel
	Private cbarpanelExil As TD.SandBar.ContainerBarClientPanel
	Private cbarExil As TD.SandBar.ContainerBar
	Private cbarpanelGraveyard As TD.SandBar.ContainerBarClientPanel
	Private cbarGraveyard As TD.SandBar.ContainerBar
	Private cbarRegard As TD.SandBar.ContainerBar
End Class
