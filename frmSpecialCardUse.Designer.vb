'
' Created by SharpDevelop.
' User: Couitchy
' Date: 20/08/2009
' Time: 16:36
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Partial Class frmSpecialCardUse
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSpecialCardUse))
		Me.cbarSpecialCard = New TD.SandBar.ContainerBar
		Me.pnlSpecialCard = New TD.SandBar.ContainerBarClientPanel
		Me.grpUse = New System.Windows.Forms.GroupBox
		Me.chkDoesntUntap = New System.Windows.Forms.CheckBox
		Me.chkInvocTapped = New System.Windows.Forms.CheckBox
		Me.txtEffet = New System.Windows.Forms.TextBox
		Me.txtEffort = New System.Windows.Forms.TextBox
		Me.lbl21 = New System.Windows.Forms.Label
		Me.lbl11 = New System.Windows.Forms.Label
		Me.cboEffet = New System.Windows.Forms.ComboBox
		Me.cboEffort = New System.Windows.Forms.ComboBox
		Me.lbl2 = New System.Windows.Forms.Label
		Me.lbl1 = New System.Windows.Forms.Label
		Me.cmdSave = New System.Windows.Forms.Button
		Me.chkDefineSpecial = New System.Windows.Forms.CheckBox
		Me.grpCard = New System.Windows.Forms.GroupBox
		Me.lbl0 = New System.Windows.Forms.Label
		Me.cboCard = New System.Windows.Forms.ComboBox
		Me.cbarSpecialCard.SuspendLayout
		Me.pnlSpecialCard.SuspendLayout
		Me.grpUse.SuspendLayout
		Me.grpCard.SuspendLayout
		Me.SuspendLayout
		'
		'cbarSpecialCard
		'
		Me.cbarSpecialCard.AddRemoveButtonsVisible = false
		Me.cbarSpecialCard.Controls.Add(Me.pnlSpecialCard)
		Me.cbarSpecialCard.Dock = System.Windows.Forms.DockStyle.Fill
		Me.cbarSpecialCard.DrawActionsButton = false
		Me.cbarSpecialCard.Flow = TD.SandBar.ToolBarLayout.Horizontal
		Me.cbarSpecialCard.Guid = New System.Guid("d1da9cf1-da52-45df-81be-02ab25d7428d")
		Me.cbarSpecialCard.Location = New System.Drawing.Point(0, 0)
		Me.cbarSpecialCard.Movable = false
		Me.cbarSpecialCard.Name = "cbarSpecialCard"
		Me.cbarSpecialCard.Size = New System.Drawing.Size(378, 345)
		Me.cbarSpecialCard.TabIndex = 0
		Me.cbarSpecialCard.Text = "Utilisation spéciale d'une carte"
		AddHandler Me.cbarSpecialCard.VisibleChanged, AddressOf Me.CbarSpecialCardVisibleChanged
		AddHandler Me.cbarSpecialCard.MouseDown, AddressOf Me.CbarSpecialCardMouseDown
		AddHandler Me.cbarSpecialCard.MouseMove, AddressOf Me.CbarSpecialCardMouseMove
		AddHandler Me.cbarSpecialCard.MouseUp, AddressOf Me.CbarSpecialCardMouseUp
		'
		'pnlSpecialCard
		'
		Me.pnlSpecialCard.Controls.Add(Me.grpUse)
		Me.pnlSpecialCard.Controls.Add(Me.grpCard)
		Me.pnlSpecialCard.Location = New System.Drawing.Point(2, 27)
		Me.pnlSpecialCard.Name = "pnlSpecialCard"
		Me.pnlSpecialCard.Size = New System.Drawing.Size(374, 316)
		Me.pnlSpecialCard.TabIndex = 0
		'
		'grpUse
		'
		Me.grpUse.Controls.Add(Me.chkDoesntUntap)
		Me.grpUse.Controls.Add(Me.chkInvocTapped)
		Me.grpUse.Controls.Add(Me.txtEffet)
		Me.grpUse.Controls.Add(Me.txtEffort)
		Me.grpUse.Controls.Add(Me.lbl21)
		Me.grpUse.Controls.Add(Me.lbl11)
		Me.grpUse.Controls.Add(Me.cboEffet)
		Me.grpUse.Controls.Add(Me.cboEffort)
		Me.grpUse.Controls.Add(Me.lbl2)
		Me.grpUse.Controls.Add(Me.lbl1)
		Me.grpUse.Controls.Add(Me.cmdSave)
		Me.grpUse.Controls.Add(Me.chkDefineSpecial)
		Me.grpUse.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grpUse.Location = New System.Drawing.Point(0, 91)
		Me.grpUse.Name = "grpUse"
		Me.grpUse.Size = New System.Drawing.Size(374, 225)
		Me.grpUse.TabIndex = 1
		Me.grpUse.TabStop = false
		'
		'chkDoesntUntap
		'
		Me.chkDoesntUntap.AutoSize = true
		Me.chkDoesntUntap.Enabled = false
		Me.chkDoesntUntap.Location = New System.Drawing.Point(20, 166)
		Me.chkDoesntUntap.Name = "chkDoesntUntap"
		Me.chkDoesntUntap.Size = New System.Drawing.Size(315, 17)
		Me.chkDoesntUntap.TabIndex = 11
		Me.chkDoesntUntap.Text = "La carte ne se dégage pas pendant la phase de dégagement"
		Me.chkDoesntUntap.UseVisualStyleBackColor = true
		'
		'chkInvocTapped
		'
		Me.chkInvocTapped.AutoSize = true
		Me.chkInvocTapped.Enabled = false
		Me.chkInvocTapped.Location = New System.Drawing.Point(20, 143)
		Me.chkInvocTapped.Name = "chkInvocTapped"
		Me.chkInvocTapped.Size = New System.Drawing.Size(171, 17)
		Me.chkInvocTapped.TabIndex = 10
		Me.chkInvocTapped.Text = "La carte arrive en jeu engagée (ou mal d'invocation)"
		Me.chkInvocTapped.UseVisualStyleBackColor = true
		'
		'txtEffet
		'
		Me.txtEffet.Enabled = false
		Me.txtEffet.Location = New System.Drawing.Point(146, 117)
		Me.txtEffet.Name = "txtEffet"
		Me.txtEffet.Size = New System.Drawing.Size(218, 20)
		Me.txtEffet.TabIndex = 9
		Me.txtEffet.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		AddHandler Me.txtEffet.Enter, AddressOf Me.TxtEffetEnter
		AddHandler Me.txtEffet.Leave, AddressOf Me.TxtEffetLeave
		'
		'txtEffort
		'
		Me.txtEffort.Enabled = false
		Me.txtEffort.Location = New System.Drawing.Point(105, 69)
		Me.txtEffort.Name = "txtEffort"
		Me.txtEffort.Size = New System.Drawing.Size(259, 20)
		Me.txtEffort.TabIndex = 8
		Me.txtEffort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		AddHandler Me.txtEffort.Enter, AddressOf Me.TxtEffortEnter
		AddHandler Me.txtEffort.Leave, AddressOf Me.TxtEffortLeave
		'
		'lbl21
		'
		Me.lbl21.AutoSize = true
		Me.lbl21.Enabled = false
		Me.lbl21.Location = New System.Drawing.Point(64, 120)
		Me.lbl21.Name = "lbl21"
		Me.lbl21.Size = New System.Drawing.Size(76, 13)
		Me.lbl21.TabIndex = 7
		Me.lbl21.Text = "Récompense :"
		'
		'lbl11
		'
		Me.lbl11.AutoSize = true
		Me.lbl11.Enabled = false
		Me.lbl11.Location = New System.Drawing.Point(64, 72)
		Me.lbl11.Name = "lbl11"
		Me.lbl11.Size = New System.Drawing.Size(35, 13)
		Me.lbl11.TabIndex = 6
		Me.lbl11.Text = "Coût :"
		'
		'cboEffet
		'
		Me.cboEffet.Enabled = false
		Me.cboEffet.FormattingEnabled = true
		Me.cboEffet.Location = New System.Drawing.Point(64, 96)
		Me.cboEffet.Name = "cboEffet"
		Me.cboEffet.Size = New System.Drawing.Size(300, 21)
		Me.cboEffet.TabIndex = 5
		'
		'cboEffort
		'
		Me.cboEffort.Enabled = false
		Me.cboEffort.FormattingEnabled = true
		Me.cboEffort.Location = New System.Drawing.Point(64, 48)
		Me.cboEffort.Name = "cboEffort"
		Me.cboEffort.Size = New System.Drawing.Size(300, 21)
		Me.cboEffort.TabIndex = 4
		'
		'lbl2
		'
		Me.lbl2.AutoSize = true
		Me.lbl2.Enabled = false
		Me.lbl2.Location = New System.Drawing.Point(20, 99)
		Me.lbl2.Name = "lbl2"
		Me.lbl2.Size = New System.Drawing.Size(35, 13)
		Me.lbl2.TabIndex = 3
		Me.lbl2.Text = "Effet :"
		'
		'lbl1
		'
		Me.lbl1.AutoSize = true
		Me.lbl1.Enabled = false
		Me.lbl1.Location = New System.Drawing.Point(20, 51)
		Me.lbl1.Name = "lbl1"
		Me.lbl1.Size = New System.Drawing.Size(38, 13)
		Me.lbl1.TabIndex = 2
		Me.lbl1.Text = "Effort :"
		'
		'cmdSave
		'
		Me.cmdSave.Location = New System.Drawing.Point(289, 192)
		Me.cmdSave.Name = "cmdSave"
		Me.cmdSave.Size = New System.Drawing.Size(75, 23)
		Me.cmdSave.TabIndex = 1
		Me.cmdSave.Text = "Enregistrer"
		Me.cmdSave.UseVisualStyleBackColor = true
		AddHandler Me.cmdSave.Click, AddressOf Me.CmdSaveClick
		'
		'chkDefineSpecial
		'
		Me.chkDefineSpecial.AutoSize = true
		Me.chkDefineSpecial.Location = New System.Drawing.Point(10, 19)
		Me.chkDefineSpecial.Name = "chkDefineSpecial"
		Me.chkDefineSpecial.Size = New System.Drawing.Size(205, 17)
		Me.chkDefineSpecial.TabIndex = 0
		Me.chkDefineSpecial.Text = "Définir une spécificité pour cette carte"
		Me.chkDefineSpecial.UseVisualStyleBackColor = true
		AddHandler Me.chkDefineSpecial.CheckedChanged, AddressOf Me.ChkDefineSpecialCheckedChanged
		'
		'grpCard
		'
		Me.grpCard.Controls.Add(Me.lbl0)
		Me.grpCard.Controls.Add(Me.cboCard)
		Me.grpCard.Dock = System.Windows.Forms.DockStyle.Top
		Me.grpCard.Location = New System.Drawing.Point(0, 0)
		Me.grpCard.Name = "grpCard"
		Me.grpCard.Size = New System.Drawing.Size(374, 91)
		Me.grpCard.TabIndex = 0
		Me.grpCard.TabStop = false
		'
		'lbl0
		'
		Me.lbl0.Location = New System.Drawing.Point(10, 16)
		Me.lbl0.Name = "lbl0"
		Me.lbl0.Size = New System.Drawing.Size(354, 42)
		Me.lbl0.TabIndex = 3
		Me.lbl0.Text = "Si la carte ci-dessous est mal (ou pas du tout) interprétée dans la simulation de"& _ 
		" déploiement, il est possible de caractériser son utilisation de manière plus sp"& _ 
		"écifique :"
		'
		'cboCard
		'
		Me.cboCard.FormattingEnabled = true
		Me.cboCard.Location = New System.Drawing.Point(10, 61)
		Me.cboCard.Name = "cboCard"
		Me.cboCard.Size = New System.Drawing.Size(354, 21)
		Me.cboCard.Sorted = true
		Me.cboCard.TabIndex = 0
		AddHandler Me.cboCard.SelectedIndexChanged, AddressOf Me.CboCardSelectedIndexChanged
		'
		'frmSpecialCardUse
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(378, 345)
		Me.Controls.Add(Me.cbarSpecialCard)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
		Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
		Me.Name = "frmSpecialCardUse"
		Me.Text = "Utilisation spéciale d'une carte"
		Me.cbarSpecialCard.ResumeLayout(false)
		Me.pnlSpecialCard.ResumeLayout(false)
		Me.grpUse.ResumeLayout(false)
		Me.grpUse.PerformLayout
		Me.grpCard.ResumeLayout(false)
		Me.ResumeLayout(false)
	End Sub
	Private lbl0 As System.Windows.Forms.Label
	Private chkInvocTapped As System.Windows.Forms.CheckBox
	Private chkDoesntUntap As System.Windows.Forms.CheckBox
	Private lbl11 As System.Windows.Forms.Label
	Private lbl21 As System.Windows.Forms.Label
	Private txtEffort As System.Windows.Forms.TextBox
	Private txtEffet As System.Windows.Forms.TextBox
	Private cboCard As System.Windows.Forms.ComboBox
	Private grpCard As System.Windows.Forms.GroupBox
	Private chkDefineSpecial As System.Windows.Forms.CheckBox
	Private cmdSave As System.Windows.Forms.Button
	Private lbl1 As System.Windows.Forms.Label
	Private lbl2 As System.Windows.Forms.Label
	Private cboEffort As System.Windows.Forms.ComboBox
	Private cboEffet As System.Windows.Forms.ComboBox
	Private grpUse As System.Windows.Forms.GroupBox
	Private pnlSpecialCard As TD.SandBar.ContainerBarClientPanel
	Private cbarSpecialCard As TD.SandBar.ContainerBar
End Class
