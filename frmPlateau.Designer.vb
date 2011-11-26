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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPlateau))
		Me.splitV1 = New System.Windows.Forms.SplitContainer
		Me.splitH3 = New System.Windows.Forms.SplitContainer
		Me.cbarBibli = New TD.SandBar.ContainerBar
		Me.cbarpanelBibli = New TD.SandBar.ContainerBarClientPanel
		Me.btBibliShuffle = New TD.SandBar.ButtonItem
		Me.btBibliReveal = New TD.SandBar.ButtonItem
		Me.btBibliSearch = New TD.SandBar.ButtonItem
		Me.splitH4 = New System.Windows.Forms.SplitContainer
		Me.cbarGraveyard = New TD.SandBar.ContainerBar
		Me.cbarpanelGraveyard = New TD.SandBar.ContainerBarClientPanel
		Me.btGraveyardSearch = New TD.SandBar.ButtonItem
		Me.btGraveyardShuffle = New TD.SandBar.ButtonItem
		Me.cbarExil = New TD.SandBar.ContainerBar
		Me.cbarpanelExil = New TD.SandBar.ContainerBarClientPanel
		Me.btExilSearch = New TD.SandBar.ButtonItem
		Me.splitH1 = New System.Windows.Forms.SplitContainer
		Me.cbarRegard = New TD.SandBar.ContainerBar
		Me.cbarpanelRegard = New TD.SandBar.ContainerBarClientPanel
		Me.btRegardShuffle = New TD.SandBar.ButtonItem
		Me.splitH2 = New System.Windows.Forms.SplitContainer
		Me.cbarMain = New TD.SandBar.ContainerBar
		Me.cbarpanelMain = New TD.SandBar.ContainerBarClientPanel
		Me.btMainShuffle = New TD.SandBar.ButtonItem
		Me.cbarField = New TD.SandBar.ContainerBar
		Me.cbarpanelField = New TD.SandBar.ContainerBarClientPanel
		Me.btFieldUntapAll = New TD.SandBar.ButtonItem
		Me.splitV1.Panel1.SuspendLayout
		Me.splitV1.Panel2.SuspendLayout
		Me.splitV1.SuspendLayout
		Me.splitH3.Panel1.SuspendLayout
		Me.splitH3.Panel2.SuspendLayout
		Me.splitH3.SuspendLayout
		Me.cbarBibli.SuspendLayout
		Me.splitH4.Panel1.SuspendLayout
		Me.splitH4.Panel2.SuspendLayout
		Me.splitH4.SuspendLayout
		Me.cbarGraveyard.SuspendLayout
		Me.cbarExil.SuspendLayout
		Me.splitH1.Panel1.SuspendLayout
		Me.splitH1.Panel2.SuspendLayout
		Me.splitH1.SuspendLayout
		Me.cbarRegard.SuspendLayout
		Me.splitH2.Panel1.SuspendLayout
		Me.splitH2.Panel2.SuspendLayout
		Me.splitH2.SuspendLayout
		Me.cbarMain.SuspendLayout
		Me.cbarField.SuspendLayout
		Me.SuspendLayout
		'
		'splitV1
		'
		Me.splitV1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.splitV1.Location = New System.Drawing.Point(0, 0)
		Me.splitV1.Name = "splitV1"
		'
		'splitV1.Panel1
		'
		Me.splitV1.Panel1.Controls.Add(Me.splitH3)
		'
		'splitV1.Panel2
		'
		Me.splitV1.Panel2.Controls.Add(Me.splitH1)
		Me.splitV1.Size = New System.Drawing.Size(727, 528)
		Me.splitV1.SplitterDistance = 243
		Me.splitV1.TabIndex = 0
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
		Me.splitH3.Size = New System.Drawing.Size(243, 528)
		Me.splitH3.SplitterDistance = 149
		Me.splitH3.TabIndex = 0
		'
		'cbarBibli
		'
		Me.cbarBibli.Closable = false
		Me.cbarBibli.Controls.Add(Me.cbarpanelBibli)
		Me.cbarBibli.Dock = System.Windows.Forms.DockStyle.Fill
		Me.cbarBibli.DrawActionsButton = false
		Me.cbarBibli.Flow = TD.SandBar.ToolBarLayout.Horizontal
		Me.cbarBibli.Guid = New System.Guid("e32163aa-c429-40d7-a6ea-f4e8715c9ed8")
		Me.cbarBibli.Items.AddRange(New TD.SandBar.ToolbarItemBase() {Me.btBibliShuffle, Me.btBibliReveal, Me.btBibliSearch})
		Me.cbarBibli.Location = New System.Drawing.Point(0, 0)
		Me.cbarBibli.Movable = false
		Me.cbarBibli.Name = "cbarBibli"
		Me.cbarBibli.Size = New System.Drawing.Size(243, 149)
		Me.cbarBibli.TabIndex = 1
		Me.cbarBibli.Text = "Bibliothèque"
		'
		'cbarpanelBibli
		'
		Me.cbarpanelBibli.Location = New System.Drawing.Point(2, 49)
		Me.cbarpanelBibli.Name = "cbarpanelBibli"
		Me.cbarpanelBibli.Size = New System.Drawing.Size(239, 98)
		Me.cbarpanelBibli.TabIndex = 0
		'
		'btBibliShuffle
		'
		Me.btBibliShuffle.Image = CType(resources.GetObject("btBibliShuffle.Image"),System.Drawing.Image)
		Me.btBibliShuffle.Text = "Mélanger"
		AddHandler Me.btBibliShuffle.Activate, AddressOf Me.BtBibliShuffleActivate
		'
		'btBibliReveal
		'
		Me.btBibliReveal.Image = CType(resources.GetObject("btBibliReveal.Image"),System.Drawing.Image)
		Me.btBibliReveal.Text = "Découvrir"
		'
		'btBibliSearch
		'
		Me.btBibliSearch.Image = CType(resources.GetObject("btBibliSearch.Image"),System.Drawing.Image)
		Me.btBibliSearch.Text = "Rechercher"
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
		Me.splitH4.Size = New System.Drawing.Size(243, 375)
		Me.splitH4.SplitterDistance = 182
		Me.splitH4.TabIndex = 0
		'
		'cbarGraveyard
		'
		Me.cbarGraveyard.Closable = false
		Me.cbarGraveyard.Controls.Add(Me.cbarpanelGraveyard)
		Me.cbarGraveyard.Dock = System.Windows.Forms.DockStyle.Fill
		Me.cbarGraveyard.DrawActionsButton = false
		Me.cbarGraveyard.Flow = TD.SandBar.ToolBarLayout.Horizontal
		Me.cbarGraveyard.Guid = New System.Guid("e32163aa-c429-40d7-a6ea-f4e8715c9ed8")
		Me.cbarGraveyard.Items.AddRange(New TD.SandBar.ToolbarItemBase() {Me.btGraveyardSearch, Me.btGraveyardShuffle})
		Me.cbarGraveyard.Location = New System.Drawing.Point(0, 0)
		Me.cbarGraveyard.Movable = false
		Me.cbarGraveyard.Name = "cbarGraveyard"
		Me.cbarGraveyard.Size = New System.Drawing.Size(243, 182)
		Me.cbarGraveyard.TabIndex = 1
		Me.cbarGraveyard.Text = "Cimetière"
		'
		'cbarpanelGraveyard
		'
		Me.cbarpanelGraveyard.Location = New System.Drawing.Point(2, 49)
		Me.cbarpanelGraveyard.Name = "cbarpanelGraveyard"
		Me.cbarpanelGraveyard.Size = New System.Drawing.Size(239, 131)
		Me.cbarpanelGraveyard.TabIndex = 0
		'
		'btGraveyardSearch
		'
		Me.btGraveyardSearch.Icon = CType(resources.GetObject("btGraveyardSearch.Icon"),System.Drawing.Icon)
		Me.btGraveyardSearch.Text = "Rechercher"
		'
		'btGraveyardShuffle
		'
		Me.btGraveyardShuffle.Image = CType(resources.GetObject("btGraveyardShuffle.Image"),System.Drawing.Image)
		Me.btGraveyardShuffle.Text = "Mélanger"
		'
		'cbarExil
		'
		Me.cbarExil.Closable = false
		Me.cbarExil.Controls.Add(Me.cbarpanelExil)
		Me.cbarExil.Dock = System.Windows.Forms.DockStyle.Fill
		Me.cbarExil.DrawActionsButton = false
		Me.cbarExil.Flow = TD.SandBar.ToolBarLayout.Horizontal
		Me.cbarExil.Guid = New System.Guid("e32163aa-c429-40d7-a6ea-f4e8715c9ed8")
		Me.cbarExil.Items.AddRange(New TD.SandBar.ToolbarItemBase() {Me.btExilSearch})
		Me.cbarExil.Location = New System.Drawing.Point(0, 0)
		Me.cbarExil.Movable = false
		Me.cbarExil.Name = "cbarExil"
		Me.cbarExil.Size = New System.Drawing.Size(243, 189)
		Me.cbarExil.TabIndex = 1
		Me.cbarExil.Text = "Exil"
		'
		'cbarpanelExil
		'
		Me.cbarpanelExil.Location = New System.Drawing.Point(2, 49)
		Me.cbarpanelExil.Name = "cbarpanelExil"
		Me.cbarpanelExil.Size = New System.Drawing.Size(239, 138)
		Me.cbarpanelExil.TabIndex = 0
		'
		'btExilSearch
		'
		Me.btExilSearch.Image = CType(resources.GetObject("btExilSearch.Image"),System.Drawing.Image)
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
		Me.splitH1.Size = New System.Drawing.Size(480, 528)
		Me.splitH1.SplitterDistance = 146
		Me.splitH1.TabIndex = 0
		'
		'cbarRegard
		'
		Me.cbarRegard.Closable = false
		Me.cbarRegard.Controls.Add(Me.cbarpanelRegard)
		Me.cbarRegard.Dock = System.Windows.Forms.DockStyle.Fill
		Me.cbarRegard.DrawActionsButton = false
		Me.cbarRegard.Flow = TD.SandBar.ToolBarLayout.Horizontal
		Me.cbarRegard.Guid = New System.Guid("e32163aa-c429-40d7-a6ea-f4e8715c9ed8")
		Me.cbarRegard.Items.AddRange(New TD.SandBar.ToolbarItemBase() {Me.btRegardShuffle})
		Me.cbarRegard.Location = New System.Drawing.Point(0, 0)
		Me.cbarRegard.Movable = false
		Me.cbarRegard.Name = "cbarRegard"
		Me.cbarRegard.Size = New System.Drawing.Size(480, 146)
		Me.cbarRegard.TabIndex = 0
		Me.cbarRegard.Text = "Regard"
		'
		'cbarpanelRegard
		'
		Me.cbarpanelRegard.Location = New System.Drawing.Point(2, 49)
		Me.cbarpanelRegard.Name = "cbarpanelRegard"
		Me.cbarpanelRegard.Size = New System.Drawing.Size(476, 95)
		Me.cbarpanelRegard.TabIndex = 0
		'
		'btRegardShuffle
		'
		Me.btRegardShuffle.Image = CType(resources.GetObject("btRegardShuffle.Image"),System.Drawing.Image)
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
		Me.splitH2.Size = New System.Drawing.Size(480, 378)
		Me.splitH2.SplitterDistance = 146
		Me.splitH2.TabIndex = 0
		'
		'cbarMain
		'
		Me.cbarMain.Closable = false
		Me.cbarMain.Controls.Add(Me.cbarpanelMain)
		Me.cbarMain.Dock = System.Windows.Forms.DockStyle.Fill
		Me.cbarMain.DrawActionsButton = false
		Me.cbarMain.Flow = TD.SandBar.ToolBarLayout.Horizontal
		Me.cbarMain.Guid = New System.Guid("e32163aa-c429-40d7-a6ea-f4e8715c9ed8")
		Me.cbarMain.Items.AddRange(New TD.SandBar.ToolbarItemBase() {Me.btMainShuffle})
		Me.cbarMain.Location = New System.Drawing.Point(0, 0)
		Me.cbarMain.Movable = false
		Me.cbarMain.Name = "cbarMain"
		Me.cbarMain.Size = New System.Drawing.Size(480, 146)
		Me.cbarMain.TabIndex = 1
		Me.cbarMain.Text = "Main"
		'
		'cbarpanelMain
		'
		Me.cbarpanelMain.Location = New System.Drawing.Point(2, 49)
		Me.cbarpanelMain.Name = "cbarpanelMain"
		Me.cbarpanelMain.Size = New System.Drawing.Size(476, 95)
		Me.cbarpanelMain.TabIndex = 0
		'
		'btMainShuffle
		'
		Me.btMainShuffle.Image = CType(resources.GetObject("btMainShuffle.Image"),System.Drawing.Image)
		Me.btMainShuffle.Text = "Mélanger"
		'
		'cbarField
		'
		Me.cbarField.Closable = false
		Me.cbarField.Controls.Add(Me.cbarpanelField)
		Me.cbarField.Dock = System.Windows.Forms.DockStyle.Fill
		Me.cbarField.DrawActionsButton = false
		Me.cbarField.Flow = TD.SandBar.ToolBarLayout.Horizontal
		Me.cbarField.Guid = New System.Guid("e32163aa-c429-40d7-a6ea-f4e8715c9ed8")
		Me.cbarField.Items.AddRange(New TD.SandBar.ToolbarItemBase() {Me.btFieldUntapAll})
		Me.cbarField.Location = New System.Drawing.Point(0, 0)
		Me.cbarField.Movable = false
		Me.cbarField.Name = "cbarField"
		Me.cbarField.Size = New System.Drawing.Size(480, 228)
		Me.cbarField.TabIndex = 1
		Me.cbarField.Text = "Champ de bataille"
		'
		'cbarpanelField
		'
		Me.cbarpanelField.Location = New System.Drawing.Point(2, 49)
		Me.cbarpanelField.Name = "cbarpanelField"
		Me.cbarpanelField.Size = New System.Drawing.Size(476, 177)
		Me.cbarpanelField.TabIndex = 0
		'
		'btFieldUntapAll
		'
		Me.btFieldUntapAll.Image = CType(resources.GetObject("btFieldUntapAll.Image"),System.Drawing.Image)
		Me.btFieldUntapAll.Text = "Tout dégager"
		'
		'frmPlateau
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(727, 528)
		Me.Controls.Add(Me.splitV1)
		Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
		Me.Name = "frmPlateau"
		Me.Text = "Plateau de jeu"
		Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
		AddHandler Load, AddressOf Me.FrmPlateauLoad
		AddHandler ResizeEnd, AddressOf Me.FrmPlateauResizeEnd
		Me.splitV1.Panel1.ResumeLayout(false)
		Me.splitV1.Panel2.ResumeLayout(false)
		Me.splitV1.ResumeLayout(false)
		Me.splitH3.Panel1.ResumeLayout(false)
		Me.splitH3.Panel2.ResumeLayout(false)
		Me.splitH3.ResumeLayout(false)
		Me.cbarBibli.ResumeLayout(false)
		Me.splitH4.Panel1.ResumeLayout(false)
		Me.splitH4.Panel2.ResumeLayout(false)
		Me.splitH4.ResumeLayout(false)
		Me.cbarGraveyard.ResumeLayout(false)
		Me.cbarExil.ResumeLayout(false)
		Me.splitH1.Panel1.ResumeLayout(false)
		Me.splitH1.Panel2.ResumeLayout(false)
		Me.splitH1.ResumeLayout(false)
		Me.cbarRegard.ResumeLayout(false)
		Me.splitH2.Panel1.ResumeLayout(false)
		Me.splitH2.Panel2.ResumeLayout(false)
		Me.splitH2.ResumeLayout(false)
		Me.cbarMain.ResumeLayout(false)
		Me.cbarField.ResumeLayout(false)
		Me.ResumeLayout(false)
	End Sub
	Private cbarBibli As TD.SandBar.ContainerBar
	Private cbarpanelBibli As TD.SandBar.ContainerBarClientPanel
	Private btBibliSearch As TD.SandBar.ButtonItem
	Private btBibliReveal As TD.SandBar.ButtonItem
	Private btBibliShuffle As TD.SandBar.ButtonItem
	Private btGraveyardSearch As TD.SandBar.ButtonItem
	Private btGraveyardShuffle As TD.SandBar.ButtonItem
	Private btExilSearch As TD.SandBar.ButtonItem
	Private btRegardShuffle As TD.SandBar.ButtonItem
	Private btMainShuffle As TD.SandBar.ButtonItem
	Private btFieldUntapAll As TD.SandBar.ButtonItem
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
