'
' Created by SharpDevelop.
' User: Couitchy
' Date: 20/11/2008
' Time: 18:10
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Partial Class frmGestDecks
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmGestDecks))
		Me.cbarDecksManager = New TD.SandBar.ContainerBar
		Me.pnlDecksManager = New TD.SandBar.ContainerBarClientPanel
		Me.lstDecks = New System.Windows.Forms.ListBox
		Me.btAdd = New TD.SandBar.ButtonItem
		Me.btRemove = New TD.SandBar.ButtonItem
		Me.btRename = New TD.SandBar.ButtonItem
		Me.btUp = New TD.SandBar.ButtonItem
		Me.btDown = New TD.SandBar.ButtonItem
		Me.cbarDecksManager.SuspendLayout
		Me.pnlDecksManager.SuspendLayout
		Me.SuspendLayout
		'
		'cbarDecksManager
		'
		Me.cbarDecksManager.AddRemoveButtonsVisible = false
		Me.cbarDecksManager.Controls.Add(Me.pnlDecksManager)
		Me.cbarDecksManager.Dock = System.Windows.Forms.DockStyle.Fill
		Me.cbarDecksManager.DrawActionsButton = false
		Me.cbarDecksManager.Flow = TD.SandBar.ToolBarLayout.Horizontal
		Me.cbarDecksManager.Guid = New System.Guid("091e31c8-a054-4ec4-a212-c0bc96ca3063")
		Me.cbarDecksManager.Items.AddRange(New TD.SandBar.ToolbarItemBase() {Me.btAdd, Me.btRemove, Me.btRename, Me.btUp, Me.btDown})
		Me.cbarDecksManager.Location = New System.Drawing.Point(0, 0)
		Me.cbarDecksManager.Movable = false
		Me.cbarDecksManager.Name = "cbarDecksManager"
		Me.cbarDecksManager.Size = New System.Drawing.Size(358, 212)
		Me.cbarDecksManager.TabIndex = 0
		Me.cbarDecksManager.Text = "Gestion des decks"
		AddHandler Me.cbarDecksManager.VisibleChanged, AddressOf Me.CbarDecksManagerVisibleChanged
		AddHandler Me.cbarDecksManager.MouseMove, AddressOf Me.CbarDecksManagerMouseMove
		AddHandler Me.cbarDecksManager.MouseDown, AddressOf Me.CbarDecksManagerMouseDown
		AddHandler Me.cbarDecksManager.MouseUp, AddressOf Me.CbarDecksManagerMouseUp
		'
		'pnlDecksManager
		'
		Me.pnlDecksManager.Controls.Add(Me.lstDecks)
		Me.pnlDecksManager.Location = New System.Drawing.Point(2, 49)
		Me.pnlDecksManager.Name = "pnlDecksManager"
		Me.pnlDecksManager.Size = New System.Drawing.Size(354, 161)
		Me.pnlDecksManager.TabIndex = 0
		'
		'lstDecks
		'
		Me.lstDecks.Dock = System.Windows.Forms.DockStyle.Fill
		Me.lstDecks.FormattingEnabled = true
		Me.lstDecks.HorizontalScrollbar = true
		Me.lstDecks.Location = New System.Drawing.Point(0, 0)
		Me.lstDecks.Name = "lstDecks"
		Me.lstDecks.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
		Me.lstDecks.Size = New System.Drawing.Size(354, 160)
		Me.lstDecks.TabIndex = 0
		AddHandler Me.lstDecks.SelectedIndexChanged, AddressOf Me.LstDecksSelectedIndexChanged
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
		'btUp
		'
		Me.btUp.BeginGroup = true
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
		'frmGestDecks
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(358, 212)
		Me.Controls.Add(Me.cbarDecksManager)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
		Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
		Me.Name = "frmGestDecks"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Gestion des decks"
		AddHandler Load, AddressOf Me.FrmGestDecksLoad
		AddHandler FormClosing, AddressOf Me.FrmGestDecksFormClosing
		Me.cbarDecksManager.ResumeLayout(false)
		Me.pnlDecksManager.ResumeLayout(false)
		Me.ResumeLayout(false)
	End Sub
	Private btDown As TD.SandBar.ButtonItem
	Private btUp As TD.SandBar.ButtonItem
	Private btRename As TD.SandBar.ButtonItem
	Private btRemove As TD.SandBar.ButtonItem
	Private btAdd As TD.SandBar.ButtonItem
	Private lstDecks As System.Windows.Forms.ListBox
	Private pnlDecksManager As TD.SandBar.ContainerBarClientPanel
	Private cbarDecksManager As TD.SandBar.ContainerBar
End Class
