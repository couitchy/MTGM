'
' Crée par SharpDevelop.
' Utilisateur: Couitchy
' Date: 11/11/2012
' Heure: 18:07
' 
' Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
'
Partial Class frmReserve
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmReserve))
		Me.cbarReserve = New TD.SandBar.ContainerBar()
		Me.pnlReserve = New TD.SandBar.ContainerBarClientPanel()
		Me.cbarReserve.SuspendLayout
		Me.SuspendLayout
		'
		'cbarReserve
		'
		Me.cbarReserve.AddRemoveButtonsVisible = false
		Me.cbarReserve.Controls.Add(Me.pnlReserve)
		Me.cbarReserve.Dock = System.Windows.Forms.DockStyle.Fill
		Me.cbarReserve.DrawActionsButton = false
		Me.cbarReserve.Flow = TD.SandBar.ToolBarLayout.Horizontal
		Me.cbarReserve.Guid = New System.Guid("e1afccf7-b082-451d-9dae-2142b22dc603")
		Me.cbarReserve.Location = New System.Drawing.Point(0, 0)
		Me.cbarReserve.Movable = false
		Me.cbarReserve.Name = "cbarReserve"
		Me.cbarReserve.Size = New System.Drawing.Size(284, 262)
		Me.cbarReserve.TabIndex = 8
		Me.cbarReserve.Text = "Gestion du jeu de réserve"
		AddHandler Me.cbarReserve.VisibleChanged, AddressOf Me.CbarReserveVisibleChanged
		AddHandler Me.cbarReserve.MouseDown, AddressOf Me.CbarReserveMouseDown
		AddHandler Me.cbarReserve.MouseMove, AddressOf Me.CbarReserveMouseMove
		AddHandler Me.cbarReserve.MouseUp, AddressOf Me.CbarReserveMouseUp
		'
		'pnlReserve
		'
		Me.pnlReserve.Location = New System.Drawing.Point(2, 27)
		Me.pnlReserve.Name = "pnlReserve"
		Me.pnlReserve.Size = New System.Drawing.Size(280, 233)
		Me.pnlReserve.TabIndex = 0
		'
		'frmReserve
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(284, 262)
		Me.Controls.Add(Me.cbarReserve)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
		Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
		Me.Name = "frmReserve"
		Me.Text = "Gestion du jeu de réserve"
		Me.cbarReserve.ResumeLayout(false)
		Me.ResumeLayout(false)
	End Sub
	Private pnlReserve As TD.SandBar.ContainerBarClientPanel
	Private cbarReserve As TD.SandBar.ContainerBar
End Class
