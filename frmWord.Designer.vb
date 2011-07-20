'
' Crée par SharpDevelop.
' Utilisateur: Couitchy
' Date: 14/12/2009
' Heure: 19:26
' 
' Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
'
Partial Class frmWord
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmWord))
		Me.cbarWord = New TD.SandBar.ContainerBar
		Me.pnlWord = New TD.SandBar.ContainerBarClientPanel
		Me.grpVignettes = New System.Windows.Forms.GroupBox
		Me.chklstWord = New System.Windows.Forms.CheckedListBox
		Me.chkAllNone = New System.Windows.Forms.CheckBox
		Me.lblWord = New System.Windows.Forms.Label
		Me.cmdWord = New System.Windows.Forms.Button
		Me.grpOptions = New System.Windows.Forms.GroupBox
		Me.prgAvance = New System.Windows.Forms.ProgressBar
		Me.cmdSaveImg = New System.Windows.Forms.Button
		Me.txtSaveImg = New System.Windows.Forms.TextBox
		Me.chkSaveImg = New System.Windows.Forms.CheckBox
		Me.chkWordShow = New System.Windows.Forms.CheckBox
		Me.chkSingle = New System.Windows.Forms.CheckBox
		Me.btVignettes = New TD.SandBar.ButtonItem
		Me.btAdvance = New TD.SandBar.ButtonItem
		Me.dlgBrowse = New System.Windows.Forms.FolderBrowserDialog
		Me.cbarWord.SuspendLayout
		Me.pnlWord.SuspendLayout
		Me.grpVignettes.SuspendLayout
		Me.grpOptions.SuspendLayout
		Me.SuspendLayout
		'
		'cbarWord
		'
		Me.cbarWord.AddRemoveButtonsVisible = false
		Me.cbarWord.Controls.Add(Me.pnlWord)
		Me.cbarWord.Dock = System.Windows.Forms.DockStyle.Fill
		Me.cbarWord.DrawActionsButton = false
		Me.cbarWord.Flow = TD.SandBar.ToolBarLayout.Horizontal
		Me.cbarWord.Guid = New System.Guid("87fcc225-afe9-4c7f-8043-5238449392a6")
		Me.cbarWord.Items.AddRange(New TD.SandBar.ToolbarItemBase() {Me.btVignettes, Me.btAdvance})
		Me.cbarWord.Location = New System.Drawing.Point(0, 0)
		Me.cbarWord.Movable = false
		Me.cbarWord.Name = "cbarWord"
		Me.cbarWord.Size = New System.Drawing.Size(284, 275)
		Me.cbarWord.TabIndex = 0
		Me.cbarWord.Text = "Génération Word"
		AddHandler Me.cbarWord.VisibleChanged, AddressOf Me.CbarWordVisibleChanged
		AddHandler Me.cbarWord.MouseMove, AddressOf Me.CbarWordMouseMove
		AddHandler Me.cbarWord.MouseDown, AddressOf Me.CbarWordMouseDown
		AddHandler Me.cbarWord.MouseUp, AddressOf Me.CbarWordMouseUp
		'
		'pnlWord
		'
		Me.pnlWord.Controls.Add(Me.grpVignettes)
		Me.pnlWord.Controls.Add(Me.cmdWord)
		Me.pnlWord.Controls.Add(Me.grpOptions)
		Me.pnlWord.Location = New System.Drawing.Point(2, 49)
		Me.pnlWord.Name = "pnlWord"
		Me.pnlWord.Size = New System.Drawing.Size(280, 224)
		Me.pnlWord.TabIndex = 0
		'
		'grpVignettes
		'
		Me.grpVignettes.Controls.Add(Me.chklstWord)
		Me.grpVignettes.Controls.Add(Me.chkAllNone)
		Me.grpVignettes.Controls.Add(Me.lblWord)
		Me.grpVignettes.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grpVignettes.Location = New System.Drawing.Point(0, 0)
		Me.grpVignettes.Name = "grpVignettes"
		Me.grpVignettes.Size = New System.Drawing.Size(280, 201)
		Me.grpVignettes.TabIndex = 19
		Me.grpVignettes.TabStop = false
		'
		'chklstWord
		'
		Me.chklstWord.CheckOnClick = true
		Me.chklstWord.Dock = System.Windows.Forms.DockStyle.Fill
		Me.chklstWord.FormattingEnabled = true
		Me.chklstWord.Location = New System.Drawing.Point(3, 49)
		Me.chklstWord.Name = "chklstWord"
		Me.chklstWord.Size = New System.Drawing.Size(274, 124)
		Me.chklstWord.TabIndex = 13
		AddHandler Me.chklstWord.SelectedValueChanged, AddressOf Me.ChklstWordSelectedValueChanged
		'
		'chkAllNone
		'
		Me.chkAllNone.Checked = true
		Me.chkAllNone.CheckState = System.Windows.Forms.CheckState.Checked
		Me.chkAllNone.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.chkAllNone.Location = New System.Drawing.Point(3, 173)
		Me.chkAllNone.Name = "chkAllNone"
		Me.chkAllNone.Padding = New System.Windows.Forms.Padding(3, 0, 0, 0)
		Me.chkAllNone.Size = New System.Drawing.Size(274, 25)
		Me.chkAllNone.TabIndex = 12
		Me.chkAllNone.Text = "Sélectionner tout"
		Me.chkAllNone.UseVisualStyleBackColor = true
		AddHandler Me.chkAllNone.CheckedChanged, AddressOf Me.ChkAllNoneCheckedChanged
		'
		'lblWord
		'
		Me.lblWord.BackColor = System.Drawing.Color.Transparent
		Me.lblWord.Dock = System.Windows.Forms.DockStyle.Top
		Me.lblWord.Location = New System.Drawing.Point(3, 16)
		Me.lblWord.Name = "lblWord"
		Me.lblWord.Size = New System.Drawing.Size(274, 33)
		Me.lblWord.TabIndex = 5
		Me.lblWord.Text = "Sélectionnez les cartes que vous souhaitez faire apparaître en vignettes sous Wor"& _ 
		"d :"
		'
		'cmdWord
		'
		Me.cmdWord.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.cmdWord.Location = New System.Drawing.Point(0, 201)
		Me.cmdWord.Name = "cmdWord"
		Me.cmdWord.Size = New System.Drawing.Size(280, 23)
		Me.cmdWord.TabIndex = 13
		Me.cmdWord.Text = "Générer"
		Me.cmdWord.UseVisualStyleBackColor = true
		AddHandler Me.cmdWord.Click, AddressOf Me.CmdWordClick
		'
		'grpOptions
		'
		Me.grpOptions.Controls.Add(Me.prgAvance)
		Me.grpOptions.Controls.Add(Me.cmdSaveImg)
		Me.grpOptions.Controls.Add(Me.txtSaveImg)
		Me.grpOptions.Controls.Add(Me.chkSaveImg)
		Me.grpOptions.Controls.Add(Me.chkWordShow)
		Me.grpOptions.Controls.Add(Me.chkSingle)
		Me.grpOptions.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grpOptions.Location = New System.Drawing.Point(0, 0)
		Me.grpOptions.Name = "grpOptions"
		Me.grpOptions.Size = New System.Drawing.Size(280, 224)
		Me.grpOptions.TabIndex = 17
		Me.grpOptions.TabStop = false
		Me.grpOptions.Visible = false
		'
		'prgAvance
		'
		Me.prgAvance.Location = New System.Drawing.Point(39, 36)
		Me.prgAvance.Name = "prgAvance"
		Me.prgAvance.Size = New System.Drawing.Size(212, 23)
		Me.prgAvance.TabIndex = 7
		'
		'cmdSaveImg
		'
		Me.cmdSaveImg.Location = New System.Drawing.Point(231, 154)
		Me.cmdSaveImg.Name = "cmdSaveImg"
		Me.cmdSaveImg.Size = New System.Drawing.Size(20, 20)
		Me.cmdSaveImg.TabIndex = 6
		Me.cmdSaveImg.Text = "."
		Me.cmdSaveImg.UseVisualStyleBackColor = true
		AddHandler Me.cmdSaveImg.Click, AddressOf Me.CmdSaveImgClick
		'
		'txtSaveImg
		'
		Me.txtSaveImg.Location = New System.Drawing.Point(59, 154)
		Me.txtSaveImg.Name = "txtSaveImg"
		Me.txtSaveImg.ReadOnly = true
		Me.txtSaveImg.Size = New System.Drawing.Size(166, 20)
		Me.txtSaveImg.TabIndex = 5
		'
		'chkSaveImg
		'
		Me.chkSaveImg.AutoSize = true
		Me.chkSaveImg.Checked = true
		Me.chkSaveImg.CheckState = System.Windows.Forms.CheckState.Checked
		Me.chkSaveImg.Enabled = false
		Me.chkSaveImg.Location = New System.Drawing.Point(39, 131)
		Me.chkSaveImg.Name = "chkSaveImg"
		Me.chkSaveImg.Size = New System.Drawing.Size(204, 17)
		Me.chkSaveImg.TabIndex = 4
		Me.chkSaveImg.Text = "Extraire les fichiers d'image des cartes"
		Me.chkSaveImg.UseVisualStyleBackColor = true
		'
		'chkWordShow
		'
		Me.chkWordShow.AutoSize = true
		Me.chkWordShow.Location = New System.Drawing.Point(39, 108)
		Me.chkWordShow.Name = "chkWordShow"
		Me.chkWordShow.Size = New System.Drawing.Size(197, 17)
		Me.chkWordShow.TabIndex = 3
		Me.chkWordShow.Text = "Afficher Word pendant la génération"
		Me.chkWordShow.UseVisualStyleBackColor = true
		'
		'chkSingle
		'
		Me.chkSingle.AutoSize = true
		Me.chkSingle.Location = New System.Drawing.Point(39, 85)
		Me.chkSingle.Name = "chkSingle"
		Me.chkSingle.Size = New System.Drawing.Size(145, 17)
		Me.chkSingle.TabIndex = 2
		Me.chkSingle.Text = "Vignette unique par carte"
		Me.chkSingle.UseVisualStyleBackColor = true
		'
		'btVignettes
		'
		Me.btVignettes.Icon = CType(resources.GetObject("btVignettes.Icon"),System.Drawing.Icon)
		Me.btVignettes.Text = "Vignettes"
		AddHandler Me.btVignettes.Activate, AddressOf Me.BtVignettesActivate
		'
		'btAdvance
		'
		Me.btAdvance.Icon = CType(resources.GetObject("btAdvance.Icon"),System.Drawing.Icon)
		Me.btAdvance.Text = "Options"
		AddHandler Me.btAdvance.Activate, AddressOf Me.BtAdvanceActivate
		'
		'dlgBrowse
		'
		Me.dlgBrowse.Description = "Répertoire d'extraction des images"
		'
		'frmWord
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(284, 275)
		Me.Controls.Add(Me.cbarWord)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
		Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
		Me.Name = "frmWord"
		Me.ShowInTaskbar = false
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Génération Excel"
		AddHandler Load, AddressOf Me.FrmWordLoad
		Me.cbarWord.ResumeLayout(false)
		Me.pnlWord.ResumeLayout(false)
		Me.grpVignettes.ResumeLayout(false)
		Me.grpOptions.ResumeLayout(false)
		Me.grpOptions.PerformLayout
		Me.ResumeLayout(false)
	End Sub
	Private prgAvance As System.Windows.Forms.ProgressBar
	Private btVignettes As TD.SandBar.ButtonItem
	Private cbarWord As TD.SandBar.ContainerBar
	Private chklstWord As System.Windows.Forms.CheckedListBox
	Private cmdWord As System.Windows.Forms.Button
	Private lblWord As System.Windows.Forms.Label
	Private grpVignettes As System.Windows.Forms.GroupBox
	Private chkWordShow As System.Windows.Forms.CheckBox
	Private chkSingle As System.Windows.Forms.CheckBox
	Private dlgBrowse As System.Windows.Forms.FolderBrowserDialog
	Private chkSaveImg As System.Windows.Forms.CheckBox
	Private txtSaveImg As System.Windows.Forms.TextBox
	Private cmdSaveImg As System.Windows.Forms.Button
	Private btAdvance As TD.SandBar.ButtonItem
	Private grpOptions As System.Windows.Forms.GroupBox
	Private chkAllNone As System.Windows.Forms.CheckBox
	Private pnlWord As TD.SandBar.ContainerBarClientPanel
End Class
