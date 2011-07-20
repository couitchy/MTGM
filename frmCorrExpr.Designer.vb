'
' Crée par SharpDevelop.
' Utilisateur: Couitchy
' Date: 20/03/2010
' Heure: 19:22
' 
' Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
'
Partial Class frmCorrExpr
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCorrExpr))
		Me.cbarExpr = New TD.SandBar.ContainerBar
		Me.pnlExpr = New TD.SandBar.ContainerBarClientPanel
		Me.chklstExpr = New System.Windows.Forms.CheckedListBox
		Me.chkAllNone = New System.Windows.Forms.CheckBox
		Me.cmdKeep = New System.Windows.Forms.Button
		Me.cmdRemove = New System.Windows.Forms.Button
		Me.cmdOk = New System.Windows.Forms.Button
		Me.cbarExpr.SuspendLayout
		Me.pnlExpr.SuspendLayout
		Me.SuspendLayout
		'
		'cbarExpr
		'
		Me.cbarExpr.AddRemoveButtonsVisible = false
		Me.cbarExpr.Controls.Add(Me.pnlExpr)
		Me.cbarExpr.Dock = System.Windows.Forms.DockStyle.Fill
		Me.cbarExpr.DrawActionsButton = false
		Me.cbarExpr.Flow = TD.SandBar.ToolBarLayout.Horizontal
		Me.cbarExpr.Guid = New System.Guid("e1afccf7-b082-451d-9dae-2142b22dc603")
		Me.cbarExpr.Location = New System.Drawing.Point(0, 0)
		Me.cbarExpr.Movable = false
		Me.cbarExpr.Name = "cbarExpr"
		Me.cbarExpr.Size = New System.Drawing.Size(267, 283)
		Me.cbarExpr.TabIndex = 8
		Me.cbarExpr.Text = "Ajustement des expressions de corrélation"
		AddHandler Me.cbarExpr.VisibleChanged, AddressOf Me.CbarExprVisibleChanged
		AddHandler Me.cbarExpr.MouseMove, AddressOf Me.CbarExprMouseMove
		AddHandler Me.cbarExpr.MouseDown, AddressOf Me.CbarExprMouseDown
		AddHandler Me.cbarExpr.MouseUp, AddressOf Me.CbarExprMouseUp
		'
		'pnlExpr
		'
		Me.pnlExpr.Controls.Add(Me.chklstExpr)
		Me.pnlExpr.Controls.Add(Me.chkAllNone)
		Me.pnlExpr.Controls.Add(Me.cmdKeep)
		Me.pnlExpr.Controls.Add(Me.cmdRemove)
		Me.pnlExpr.Controls.Add(Me.cmdOk)
		Me.pnlExpr.Location = New System.Drawing.Point(2, 27)
		Me.pnlExpr.Name = "pnlExpr"
		Me.pnlExpr.Size = New System.Drawing.Size(263, 254)
		Me.pnlExpr.TabIndex = 0
		'
		'chklstExpr
		'
		Me.chklstExpr.CheckOnClick = true
		Me.chklstExpr.Dock = System.Windows.Forms.DockStyle.Fill
		Me.chklstExpr.FormattingEnabled = true
		Me.chklstExpr.HorizontalScrollbar = true
		Me.chklstExpr.Location = New System.Drawing.Point(0, 0)
		Me.chklstExpr.Name = "chklstExpr"
		Me.chklstExpr.Size = New System.Drawing.Size(263, 154)
		Me.chklstExpr.TabIndex = 12
		'
		'chkAllNone
		'
		Me.chkAllNone.Checked = true
		Me.chkAllNone.CheckState = System.Windows.Forms.CheckState.Checked
		Me.chkAllNone.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.chkAllNone.Location = New System.Drawing.Point(0, 161)
		Me.chkAllNone.Name = "chkAllNone"
		Me.chkAllNone.Size = New System.Drawing.Size(263, 24)
		Me.chkAllNone.TabIndex = 11
		Me.chkAllNone.Text = "Sélectionner tout / rien"
		Me.chkAllNone.UseVisualStyleBackColor = true
		AddHandler Me.chkAllNone.CheckedChanged, AddressOf Me.ChkAllNoneCheckedChanged
		'
		'cmdKeep
		'
		Me.cmdKeep.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.cmdKeep.Location = New System.Drawing.Point(0, 185)
		Me.cmdKeep.Name = "cmdKeep"
		Me.cmdKeep.Size = New System.Drawing.Size(263, 23)
		Me.cmdKeep.TabIndex = 10
		Me.cmdKeep.Text = "Inclure..."
		Me.cmdKeep.UseVisualStyleBackColor = true
		AddHandler Me.cmdKeep.Click, AddressOf Me.CmdKeepClick
		'
		'cmdRemove
		'
		Me.cmdRemove.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.cmdRemove.Location = New System.Drawing.Point(0, 208)
		Me.cmdRemove.Name = "cmdRemove"
		Me.cmdRemove.Size = New System.Drawing.Size(263, 23)
		Me.cmdRemove.TabIndex = 6
		Me.cmdRemove.Text = "Exclure..."
		Me.cmdRemove.UseVisualStyleBackColor = true
		AddHandler Me.cmdRemove.Click, AddressOf Me.CmdRemoveClick
		'
		'cmdOk
		'
		Me.cmdOk.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.cmdOk.Location = New System.Drawing.Point(0, 231)
		Me.cmdOk.Name = "cmdOk"
		Me.cmdOk.Size = New System.Drawing.Size(263, 23)
		Me.cmdOk.TabIndex = 2
		Me.cmdOk.Text = "Valider"
		Me.cmdOk.UseVisualStyleBackColor = true
		AddHandler Me.cmdOk.Click, AddressOf Me.CmdOkClick
		'
		'frmCorrExpr
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(267, 283)
		Me.Controls.Add(Me.cbarExpr)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
		Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
		Me.Name = "frmCorrExpr"
		Me.ShowInTaskbar = false
		Me.Text = "Expressions de corrélation"
		AddHandler Load, AddressOf Me.FrmCorrExprLoad
		Me.cbarExpr.ResumeLayout(false)
		Me.pnlExpr.ResumeLayout(false)
		Me.ResumeLayout(false)
	End Sub
	Private cmdKeep As System.Windows.Forms.Button
	Private cmdRemove As System.Windows.Forms.Button
	Private cmdOk As System.Windows.Forms.Button
	Private chkAllNone As System.Windows.Forms.CheckBox
	Private chklstExpr As System.Windows.Forms.CheckedListBox
	Private pnlExpr As TD.SandBar.ContainerBarClientPanel
	Private cbarExpr As TD.SandBar.ContainerBar
End Class
