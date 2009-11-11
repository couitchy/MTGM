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
		Me.cbarDecksManager = New TD.SandBar.ContainerBar
		Me.pnlDecksManager = New TD.SandBar.ContainerBarClientPanel
		Me.cmdClose = New System.Windows.Forms.Button
		Me.cmdRen = New System.Windows.Forms.Button
		Me.cmdRem = New System.Windows.Forms.Button
		Me.cmdAdd = New System.Windows.Forms.Button
		Me.lstDecks = New System.Windows.Forms.ListBox
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
		Me.cbarDecksManager.Location = New System.Drawing.Point(0, 0)
		Me.cbarDecksManager.Movable = false
		Me.cbarDecksManager.Name = "cbarDecksManager"
		Me.cbarDecksManager.Size = New System.Drawing.Size(275, 212)
		Me.cbarDecksManager.TabIndex = 0
		Me.cbarDecksManager.Text = "Gestion des decks"
		AddHandler Me.cbarDecksManager.VisibleChanged, AddressOf Me.CbarDecksManagerVisibleChanged
		AddHandler Me.cbarDecksManager.MouseDown, AddressOf Me.CbarDecksManagerMouseDown
		AddHandler Me.cbarDecksManager.MouseMove, AddressOf Me.CbarDecksManagerMouseMove
		AddHandler Me.cbarDecksManager.MouseUp, AddressOf Me.CbarDecksManagerMouseUp
		'
		'pnlDecksManager
		'
		Me.pnlDecksManager.Controls.Add(Me.cmdClose)
		Me.pnlDecksManager.Controls.Add(Me.cmdRen)
		Me.pnlDecksManager.Controls.Add(Me.cmdRem)
		Me.pnlDecksManager.Controls.Add(Me.cmdAdd)
		Me.pnlDecksManager.Controls.Add(Me.lstDecks)
		Me.pnlDecksManager.Location = New System.Drawing.Point(2, 27)
		Me.pnlDecksManager.Name = "pnlDecksManager"
		Me.pnlDecksManager.Size = New System.Drawing.Size(271, 183)
		Me.pnlDecksManager.TabIndex = 0
		'
		'cmdClose
		'
		Me.cmdClose.Location = New System.Drawing.Point(189, 153)
		Me.cmdClose.Name = "cmdClose"
		Me.cmdClose.Size = New System.Drawing.Size(75, 23)
		Me.cmdClose.TabIndex = 4
		Me.cmdClose.Text = "Fermer"
		Me.cmdClose.UseVisualStyleBackColor = true
		AddHandler Me.cmdClose.Click, AddressOf Me.CmdCloseClick
		'
		'cmdRen
		'
		Me.cmdRen.Enabled = false
		Me.cmdRen.Location = New System.Drawing.Point(189, 61)
		Me.cmdRen.Name = "cmdRen"
		Me.cmdRen.Size = New System.Drawing.Size(75, 23)
		Me.cmdRen.TabIndex = 3
		Me.cmdRen.Text = "Renommer"
		Me.cmdRen.UseVisualStyleBackColor = true
		AddHandler Me.cmdRen.Click, AddressOf Me.CmdRenClick
		'
		'cmdRem
		'
		Me.cmdRem.Enabled = false
		Me.cmdRem.Location = New System.Drawing.Point(189, 32)
		Me.cmdRem.Name = "cmdRem"
		Me.cmdRem.Size = New System.Drawing.Size(75, 23)
		Me.cmdRem.TabIndex = 2
		Me.cmdRem.Text = "Supprimer"
		Me.cmdRem.UseVisualStyleBackColor = true
		AddHandler Me.cmdRem.Click, AddressOf Me.CmdRemClick
		'
		'cmdAdd
		'
		Me.cmdAdd.Location = New System.Drawing.Point(189, 3)
		Me.cmdAdd.Name = "cmdAdd"
		Me.cmdAdd.Size = New System.Drawing.Size(75, 23)
		Me.cmdAdd.TabIndex = 1
		Me.cmdAdd.Text = "Ajouter"
		Me.cmdAdd.UseVisualStyleBackColor = true
		AddHandler Me.cmdAdd.Click, AddressOf Me.CmdAddClick
		'
		'lstDecks
		'
		Me.lstDecks.FormattingEnabled = true
		Me.lstDecks.Location = New System.Drawing.Point(3, 3)
		Me.lstDecks.Name = "lstDecks"
		Me.lstDecks.Size = New System.Drawing.Size(180, 173)
		Me.lstDecks.TabIndex = 0
		AddHandler Me.lstDecks.SelectedIndexChanged, AddressOf Me.LstDecksSelectedIndexChanged
		'
		'frmGestDecks
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(275, 212)
		Me.Controls.Add(Me.cbarDecksManager)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
		Me.Name = "frmGestDecks"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "frmGestDecks"
		AddHandler FormClosing, AddressOf Me.FrmGestDecksFormClosing
		AddHandler Load, AddressOf Me.FrmGestDecksLoad
		Me.cbarDecksManager.ResumeLayout(false)
		Me.pnlDecksManager.ResumeLayout(false)
		Me.ResumeLayout(false)
	End Sub
	Private lstDecks As System.Windows.Forms.ListBox
	Private cmdAdd As System.Windows.Forms.Button
	Private cmdRem As System.Windows.Forms.Button
	Private cmdRen As System.Windows.Forms.Button
	Private cmdClose As System.Windows.Forms.Button
	Private pnlDecksManager As TD.SandBar.ContainerBarClientPanel
	Private cbarDecksManager As TD.SandBar.ContainerBar
End Class
