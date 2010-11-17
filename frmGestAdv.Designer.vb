'
' Crée par SharpDevelop.
' Utilisateur: Couitchy
' Date: 07/11/2010
' Heure: 17:56
' 
' Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
'
Partial Class frmGestAdv
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmGestAdv))
		Me.cbarAdvManager = New TD.SandBar.ContainerBar
		Me.pnlAdvManager = New TD.SandBar.ContainerBarClientPanel
		Me.splitH = New System.Windows.Forms.SplitContainer
		Me.lvwAdv = New System.Windows.Forms.ListView
		Me.colAdv = New System.Windows.Forms.ColumnHeader
		Me.colDecks = New System.Windows.Forms.ColumnHeader
		Me.cmdAffect = New System.Windows.Forms.Button
		Me.cboOwner = New System.Windows.Forms.ComboBox
		Me.cboDeck = New System.Windows.Forms.ComboBox
		Me.lblOwner = New System.Windows.Forms.Label
		Me.lblDeck = New System.Windows.Forms.Label
		Me.btAdd = New TD.SandBar.ButtonItem
		Me.btRemove = New TD.SandBar.ButtonItem
		Me.btRename = New TD.SandBar.ButtonItem
		Me.cbarAdvManager.SuspendLayout
		Me.pnlAdvManager.SuspendLayout
		Me.splitH.Panel1.SuspendLayout
		Me.splitH.Panel2.SuspendLayout
		Me.splitH.SuspendLayout
		Me.SuspendLayout
		'
		'cbarAdvManager
		'
		Me.cbarAdvManager.AddRemoveButtonsVisible = false
		Me.cbarAdvManager.Controls.Add(Me.pnlAdvManager)
		Me.cbarAdvManager.Dock = System.Windows.Forms.DockStyle.Fill
		Me.cbarAdvManager.DrawActionsButton = false
		Me.cbarAdvManager.Flow = TD.SandBar.ToolBarLayout.Horizontal
		Me.cbarAdvManager.Guid = New System.Guid("091e31c8-a054-4ec4-a212-c0bc96ca3063")
		Me.cbarAdvManager.Items.AddRange(New TD.SandBar.ToolbarItemBase() {Me.btAdd, Me.btRemove, Me.btRename})
		Me.cbarAdvManager.Location = New System.Drawing.Point(0, 0)
		Me.cbarAdvManager.Movable = false
		Me.cbarAdvManager.Name = "cbarAdvManager"
		Me.cbarAdvManager.Size = New System.Drawing.Size(326, 340)
		Me.cbarAdvManager.TabIndex = 1
		Me.cbarAdvManager.Text = "Gestion des adversaires"
		AddHandler Me.cbarAdvManager.VisibleChanged, AddressOf Me.CbarAdvManagerVisibleChanged
		AddHandler Me.cbarAdvManager.MouseMove, AddressOf Me.CbarAdvManagerMouseMove
		AddHandler Me.cbarAdvManager.MouseDown, AddressOf Me.CbarAdvManagerMouseDown
		AddHandler Me.cbarAdvManager.MouseUp, AddressOf Me.CbarAdvManagerMouseUp
		'
		'pnlAdvManager
		'
		Me.pnlAdvManager.Controls.Add(Me.splitH)
		Me.pnlAdvManager.Location = New System.Drawing.Point(2, 49)
		Me.pnlAdvManager.Name = "pnlAdvManager"
		Me.pnlAdvManager.Size = New System.Drawing.Size(322, 289)
		Me.pnlAdvManager.TabIndex = 0
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
		Me.splitH.Panel1.Controls.Add(Me.lvwAdv)
		'
		'splitH.Panel2
		'
		Me.splitH.Panel2.Controls.Add(Me.cmdAffect)
		Me.splitH.Panel2.Controls.Add(Me.cboOwner)
		Me.splitH.Panel2.Controls.Add(Me.cboDeck)
		Me.splitH.Panel2.Controls.Add(Me.lblOwner)
		Me.splitH.Panel2.Controls.Add(Me.lblDeck)
		Me.splitH.Panel2MinSize = 95
		Me.splitH.Size = New System.Drawing.Size(322, 289)
		Me.splitH.SplitterDistance = 190
		Me.splitH.TabIndex = 1
		'
		'lvwAdv
		'
		Me.lvwAdv.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colAdv, Me.colDecks})
		Me.lvwAdv.Dock = System.Windows.Forms.DockStyle.Fill
		Me.lvwAdv.Location = New System.Drawing.Point(0, 0)
		Me.lvwAdv.MultiSelect = false
		Me.lvwAdv.Name = "lvwAdv"
		Me.lvwAdv.Size = New System.Drawing.Size(322, 190)
		Me.lvwAdv.TabIndex = 0
		Me.lvwAdv.UseCompatibleStateImageBehavior = false
		Me.lvwAdv.View = System.Windows.Forms.View.Details
		AddHandler Me.lvwAdv.SelectedIndexChanged, AddressOf Me.LvwAdvSelectedIndexChanged
		'
		'colAdv
		'
		Me.colAdv.Text = "Nom"
		Me.colAdv.Width = 183
		'
		'colDecks
		'
		Me.colDecks.Text = "Decks attribués"
		Me.colDecks.Width = 98
		'
		'cmdAffect
		'
		Me.cmdAffect.Location = New System.Drawing.Point(225, 63)
		Me.cmdAffect.Name = "cmdAffect"
		Me.cmdAffect.Size = New System.Drawing.Size(75, 23)
		Me.cmdAffect.TabIndex = 4
		Me.cmdAffect.Text = "Attribuer"
		Me.cmdAffect.UseVisualStyleBackColor = true
		AddHandler Me.cmdAffect.Click, AddressOf Me.CmdAffectClick
		'
		'cboOwner
		'
		Me.cboOwner.FormattingEnabled = true
		Me.cboOwner.Location = New System.Drawing.Point(105, 36)
		Me.cboOwner.Name = "cboOwner"
		Me.cboOwner.Size = New System.Drawing.Size(195, 21)
		Me.cboOwner.TabIndex = 3
		'
		'cboDeck
		'
		Me.cboDeck.FormattingEnabled = true
		Me.cboDeck.Location = New System.Drawing.Point(105, 9)
		Me.cboDeck.Name = "cboDeck"
		Me.cboDeck.Size = New System.Drawing.Size(195, 21)
		Me.cboDeck.TabIndex = 2
		AddHandler Me.cboDeck.SelectedIndexChanged, AddressOf Me.CboDeckSelectedIndexChanged
		'
		'lblOwner
		'
		Me.lblOwner.AutoSize = true
		Me.lblOwner.Location = New System.Drawing.Point(19, 39)
		Me.lblOwner.Name = "lblOwner"
		Me.lblOwner.Size = New System.Drawing.Size(66, 13)
		Me.lblOwner.TabIndex = 1
		Me.lblOwner.Text = "Propriétaire :"
		'
		'lblDeck
		'
		Me.lblDeck.AutoSize = true
		Me.lblDeck.Location = New System.Drawing.Point(19, 12)
		Me.lblDeck.Name = "lblDeck"
		Me.lblDeck.Size = New System.Drawing.Size(39, 13)
		Me.lblDeck.TabIndex = 0
		Me.lblDeck.Text = "Deck :"
		'
		'btAdd
		'
		Me.btAdd.Icon = CType(resources.GetObject("btAdd.Icon"),System.Drawing.Icon)
		Me.btAdd.Text = "Ajouter"
		AddHandler Me.btAdd.Activate, AddressOf Me.BtAddActivate
		'
		'btRemove
		'
		Me.btRemove.Enabled = false
		Me.btRemove.Icon = CType(resources.GetObject("btRemove.Icon"),System.Drawing.Icon)
		Me.btRemove.Text = "Supprimer"
		AddHandler Me.btRemove.Activate, AddressOf Me.BtRemoveActivate
		'
		'btRename
		'
		Me.btRename.Enabled = false
		Me.btRename.Icon = CType(resources.GetObject("btRename.Icon"),System.Drawing.Icon)
		Me.btRename.Text = "Renommer"
		AddHandler Me.btRename.Activate, AddressOf Me.BtRenameActivate
		'
		'frmGestAdv
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(326, 340)
		Me.Controls.Add(Me.cbarAdvManager)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
		Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
		Me.Name = "frmGestAdv"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "frmGestAdv"
		AddHandler Load, AddressOf Me.FrmGestAdvLoad
		Me.cbarAdvManager.ResumeLayout(false)
		Me.pnlAdvManager.ResumeLayout(false)
		Me.splitH.Panel1.ResumeLayout(false)
		Me.splitH.Panel2.ResumeLayout(false)
		Me.splitH.Panel2.PerformLayout
		Me.splitH.ResumeLayout(false)
		Me.ResumeLayout(false)
	End Sub
	Private lblDeck As System.Windows.Forms.Label
	Private lblOwner As System.Windows.Forms.Label
	Private cboDeck As System.Windows.Forms.ComboBox
	Private cboOwner As System.Windows.Forms.ComboBox
	Private cmdAffect As System.Windows.Forms.Button
	Private colDecks As System.Windows.Forms.ColumnHeader
	Private colAdv As System.Windows.Forms.ColumnHeader
	Private lvwAdv As System.Windows.Forms.ListView
	Private splitH As System.Windows.Forms.SplitContainer
	Private btRename As TD.SandBar.ButtonItem
	Private btRemove As TD.SandBar.ButtonItem
	Private btAdd As TD.SandBar.ButtonItem
	Private pnlAdvManager As TD.SandBar.ContainerBarClientPanel
	Private cbarAdvManager As TD.SandBar.ContainerBar
End Class
