'
' Crée par SharpDevelop.
' Utilisateur: Couitchy
' Date: 14/12/2009
' Heure: 19:26
' 
' Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
'
Partial Class frmXL
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmXL))
		Me.cbarXL = New TD.SandBar.ContainerBar()
		Me.pnlXL = New TD.SandBar.ContainerBarClientPanel()
		Me.grpColumns = New System.Windows.Forms.GroupBox()
		Me.chklstXL = New System.Windows.Forms.CheckedListBox()
		Me.chkAllNone = New System.Windows.Forms.CheckBox()
		Me.lblXL = New System.Windows.Forms.Label()
		Me.cmdXL = New System.Windows.Forms.Button()
		Me.grpOptions = New System.Windows.Forms.GroupBox()
		Me.cmdSaveImg = New System.Windows.Forms.Button()
		Me.txtSaveImg = New System.Windows.Forms.TextBox()
		Me.chkSaveImg = New System.Windows.Forms.CheckBox()
		Me.chkXLShow = New System.Windows.Forms.CheckBox()
		Me.chkVF = New System.Windows.Forms.CheckBox()
		Me.chkTextMode = New System.Windows.Forms.CheckBox()
		Me.chkHeaders = New System.Windows.Forms.CheckBox()
		Me.btColumns = New TD.SandBar.ButtonItem()
		Me.btAdvance = New TD.SandBar.ButtonItem()
		Me.dlgBrowse = New System.Windows.Forms.FolderBrowserDialog()
		Me.cbarXL.SuspendLayout
		Me.pnlXL.SuspendLayout
		Me.grpColumns.SuspendLayout
		Me.grpOptions.SuspendLayout
		Me.SuspendLayout
		'
		'cbarXL
		'
		Me.cbarXL.AddRemoveButtonsVisible = false
		Me.cbarXL.Controls.Add(Me.pnlXL)
		Me.cbarXL.Dock = System.Windows.Forms.DockStyle.Fill
		Me.cbarXL.DrawActionsButton = false
		Me.cbarXL.Flow = TD.SandBar.ToolBarLayout.Horizontal
		Me.cbarXL.Guid = New System.Guid("87fcc225-afe9-4c7f-8043-5238449392a6")
		Me.cbarXL.Items.AddRange(New TD.SandBar.ToolbarItemBase() {Me.btColumns, Me.btAdvance})
		Me.cbarXL.Location = New System.Drawing.Point(0, 0)
		Me.cbarXL.Movable = false
		Me.cbarXL.Name = "cbarXL"
		Me.cbarXL.Size = New System.Drawing.Size(284, 305)
		Me.cbarXL.TabIndex = 0
		Me.cbarXL.Text = "Génération Excel"
		AddHandler Me.cbarXL.VisibleChanged, AddressOf Me.CbarXLVisibleChanged
		AddHandler Me.cbarXL.MouseDown, AddressOf Me.CbarXLMouseDown
		AddHandler Me.cbarXL.MouseMove, AddressOf Me.CbarXLMouseMove
		AddHandler Me.cbarXL.MouseUp, AddressOf Me.CbarXLMouseUp
		'
		'pnlXL
		'
		Me.pnlXL.Controls.Add(Me.grpColumns)
		Me.pnlXL.Controls.Add(Me.cmdXL)
		Me.pnlXL.Controls.Add(Me.grpOptions)
		Me.pnlXL.Location = New System.Drawing.Point(2, 49)
		Me.pnlXL.Name = "pnlXL"
		Me.pnlXL.Size = New System.Drawing.Size(280, 254)
		Me.pnlXL.TabIndex = 0
		'
		'grpColumns
		'
		Me.grpColumns.Controls.Add(Me.chklstXL)
		Me.grpColumns.Controls.Add(Me.chkAllNone)
		Me.grpColumns.Controls.Add(Me.lblXL)
		Me.grpColumns.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grpColumns.Location = New System.Drawing.Point(0, 0)
		Me.grpColumns.Name = "grpColumns"
		Me.grpColumns.Size = New System.Drawing.Size(280, 231)
		Me.grpColumns.TabIndex = 13
		Me.grpColumns.TabStop = false
		'
		'chklstXL
		'
		Me.chklstXL.CheckOnClick = true
		Me.chklstXL.Dock = System.Windows.Forms.DockStyle.Fill
		Me.chklstXL.FormattingEnabled = true
		Me.chklstXL.Items.AddRange(New Object() {"Foil ou pas", "Couleur", "Force / Endurance", "Coût d'invocation", "Edition", "Prix", "Rareté", "Sous-type", "Type", "Texte"})
		Me.chklstXL.Location = New System.Drawing.Point(3, 49)
		Me.chklstXL.Name = "chklstXL"
		Me.chklstXL.Size = New System.Drawing.Size(274, 154)
		Me.chklstXL.TabIndex = 8
		AddHandler Me.chklstXL.SelectedValueChanged, AddressOf Me.ChklstXLSelectedValueChanged
		'
		'chkAllNone
		'
		Me.chkAllNone.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.chkAllNone.Location = New System.Drawing.Point(3, 203)
		Me.chkAllNone.Name = "chkAllNone"
		Me.chkAllNone.Padding = New System.Windows.Forms.Padding(3, 0, 0, 0)
		Me.chkAllNone.Size = New System.Drawing.Size(274, 25)
		Me.chkAllNone.TabIndex = 7
		Me.chkAllNone.Text = "Sélectionner tout"
		Me.chkAllNone.UseVisualStyleBackColor = true
		AddHandler Me.chkAllNone.CheckedChanged, AddressOf Me.ChkAllNoneCheckedChanged
		'
		'lblXL
		'
		Me.lblXL.BackColor = System.Drawing.Color.Transparent
		Me.lblXL.Dock = System.Windows.Forms.DockStyle.Top
		Me.lblXL.Location = New System.Drawing.Point(3, 16)
		Me.lblXL.Name = "lblXL"
		Me.lblXL.Size = New System.Drawing.Size(274, 33)
		Me.lblXL.TabIndex = 5
		Me.lblXL.Text = "Sélectionnez les colonnes optionnelles que vous souhaitez faire apparaître dans l"& _ 
		"a feuille Excel :"
		'
		'cmdXL
		'
		Me.cmdXL.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.cmdXL.Location = New System.Drawing.Point(0, 231)
		Me.cmdXL.Name = "cmdXL"
		Me.cmdXL.Size = New System.Drawing.Size(280, 23)
		Me.cmdXL.TabIndex = 7
		Me.cmdXL.Text = "Générer"
		Me.cmdXL.UseVisualStyleBackColor = true
		AddHandler Me.cmdXL.Click, AddressOf Me.CmdXLClick
		'
		'grpOptions
		'
		Me.grpOptions.Controls.Add(Me.cmdSaveImg)
		Me.grpOptions.Controls.Add(Me.txtSaveImg)
		Me.grpOptions.Controls.Add(Me.chkSaveImg)
		Me.grpOptions.Controls.Add(Me.chkXLShow)
		Me.grpOptions.Controls.Add(Me.chkVF)
		Me.grpOptions.Controls.Add(Me.chkTextMode)
		Me.grpOptions.Controls.Add(Me.chkHeaders)
		Me.grpOptions.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grpOptions.Location = New System.Drawing.Point(0, 0)
		Me.grpOptions.Name = "grpOptions"
		Me.grpOptions.Size = New System.Drawing.Size(280, 254)
		Me.grpOptions.TabIndex = 10
		Me.grpOptions.TabStop = false
		Me.grpOptions.Visible = false
		'
		'cmdSaveImg
		'
		Me.cmdSaveImg.Enabled = false
		Me.cmdSaveImg.Location = New System.Drawing.Point(231, 156)
		Me.cmdSaveImg.Name = "cmdSaveImg"
		Me.cmdSaveImg.Size = New System.Drawing.Size(20, 20)
		Me.cmdSaveImg.TabIndex = 6
		Me.cmdSaveImg.Text = "."
		Me.cmdSaveImg.UseVisualStyleBackColor = true
		AddHandler Me.cmdSaveImg.Click, AddressOf Me.CmdSaveImgClick
		'
		'txtSaveImg
		'
		Me.txtSaveImg.Enabled = false
		Me.txtSaveImg.Location = New System.Drawing.Point(59, 156)
		Me.txtSaveImg.Name = "txtSaveImg"
		Me.txtSaveImg.ReadOnly = true
		Me.txtSaveImg.Size = New System.Drawing.Size(166, 20)
		Me.txtSaveImg.TabIndex = 5
		'
		'chkSaveImg
		'
		Me.chkSaveImg.AutoSize = true
		Me.chkSaveImg.Location = New System.Drawing.Point(39, 133)
		Me.chkSaveImg.Name = "chkSaveImg"
		Me.chkSaveImg.Size = New System.Drawing.Size(204, 17)
		Me.chkSaveImg.TabIndex = 4
		Me.chkSaveImg.Text = "Extraire les fichiers d'image des cartes"
		Me.chkSaveImg.UseVisualStyleBackColor = true
		AddHandler Me.chkSaveImg.CheckedChanged, AddressOf Me.ChkSaveImgCheckedChanged
		'
		'chkXLShow
		'
		Me.chkXLShow.AutoSize = true
		Me.chkXLShow.Location = New System.Drawing.Point(39, 110)
		Me.chkXLShow.Name = "chkXLShow"
		Me.chkXLShow.Size = New System.Drawing.Size(197, 17)
		Me.chkXLShow.TabIndex = 3
		Me.chkXLShow.Text = "Afficher Excel pendant la génération"
		Me.chkXLShow.UseVisualStyleBackColor = true
		'
		'chkVF
		'
		Me.chkVF.AutoSize = true
		Me.chkVF.Checked = true
		Me.chkVF.CheckState = System.Windows.Forms.CheckState.Checked
		Me.chkVF.Location = New System.Drawing.Point(39, 87)
		Me.chkVF.Name = "chkVF"
		Me.chkVF.Size = New System.Drawing.Size(154, 17)
		Me.chkVF.TabIndex = 2
		Me.chkVF.Text = "Titre des cartes en français"
		Me.chkVF.UseVisualStyleBackColor = true
		'
		'chkTextMode
		'
		Me.chkTextMode.AutoSize = true
		Me.chkTextMode.Checked = true
		Me.chkTextMode.CheckState = System.Windows.Forms.CheckState.Checked
		Me.chkTextMode.Enabled = false
		Me.chkTextMode.Location = New System.Drawing.Point(39, 64)
		Me.chkTextMode.Name = "chkTextMode"
		Me.chkTextMode.Size = New System.Drawing.Size(149, 17)
		Me.chkTextMode.TabIndex = 1
		Me.chkTextMode.Text = "Mode texte exclusivement"
		Me.chkTextMode.UseVisualStyleBackColor = true
		'
		'chkHeaders
		'
		Me.chkHeaders.AutoSize = true
		Me.chkHeaders.Location = New System.Drawing.Point(39, 41)
		Me.chkHeaders.Name = "chkHeaders"
		Me.chkHeaders.Size = New System.Drawing.Size(126, 17)
		Me.chkHeaders.TabIndex = 0
		Me.chkHeaders.Text = "En-tête des colonnes"
		Me.chkHeaders.UseVisualStyleBackColor = true
		'
		'btColumns
		'
		Me.btColumns.Icon = CType(resources.GetObject("btColumns.Icon"),System.Drawing.Icon)
		Me.btColumns.Text = "Colonnes"
		AddHandler Me.btColumns.Activate, AddressOf Me.BtColumnsActivate
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
		'frmXL
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(284, 305)
		Me.Controls.Add(Me.cbarXL)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
		Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
		Me.Name = "frmXL"
		Me.ShowInTaskbar = false
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Génération Excel"
		AddHandler Load, AddressOf Me.FrmExcelLoad
		Me.cbarXL.ResumeLayout(false)
		Me.pnlXL.ResumeLayout(false)
		Me.grpColumns.ResumeLayout(false)
		Me.grpOptions.ResumeLayout(false)
		Me.grpOptions.PerformLayout
		Me.ResumeLayout(false)
	End Sub
	Private dlgBrowse As System.Windows.Forms.FolderBrowserDialog
	Private chkSaveImg As System.Windows.Forms.CheckBox
	Private txtSaveImg As System.Windows.Forms.TextBox
	Private cmdSaveImg As System.Windows.Forms.Button
	Private chkHeaders As System.Windows.Forms.CheckBox
	Private chkTextMode As System.Windows.Forms.CheckBox
	Private chkVF As System.Windows.Forms.CheckBox
	Private chkXLShow As System.Windows.Forms.CheckBox
	Private btAdvance As TD.SandBar.ButtonItem
	Private btColumns As TD.SandBar.ButtonItem
	Private grpOptions As System.Windows.Forms.GroupBox
	Private chkAllNone As System.Windows.Forms.CheckBox
	Private grpColumns As System.Windows.Forms.GroupBox
	Private lblXL As System.Windows.Forms.Label
	Private cmdXL As System.Windows.Forms.Button
	Private chklstXL As System.Windows.Forms.CheckedListBox
	Private pnlXL As TD.SandBar.ContainerBarClientPanel
	Private cbarXL As TD.SandBar.ContainerBar
End Class
