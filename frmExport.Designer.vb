'
' Created by SharpDevelop.
' User: Couitchy
' Date: 08/11/2008
' Time: 23:08
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Partial Class frmExport
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmExport))
		Me.dlgBrowser = New System.Windows.Forms.FolderBrowserDialog
		Me.cbarImpExp = New TD.SandBar.ContainerBar
		Me.pnlImpExp = New TD.SandBar.ContainerBarClientPanel
		Me.grpExport = New System.Windows.Forms.GroupBox
		Me.cboFormat = New System.Windows.Forms.ComboBox
		Me.lblFormat = New System.Windows.Forms.Label
		Me.cmdExport = New System.Windows.Forms.Button
		Me.lstchkSources = New System.Windows.Forms.CheckedListBox
		Me.grpImport = New System.Windows.Forms.GroupBox
		Me.lstImp = New System.Windows.Forms.ListBox
		Me.optImpAdd = New System.Windows.Forms.RadioButton
		Me.txtSourceImp = New System.Windows.Forms.TextBox
		Me.optImpNew = New System.Windows.Forms.RadioButton
		Me.cmdBrowse = New System.Windows.Forms.Button
		Me.txtFileImp = New System.Windows.Forms.TextBox
		Me.lblImp = New System.Windows.Forms.Label
		Me.cmdImport = New System.Windows.Forms.Button
		Me.btImport = New TD.SandBar.ButtonItem
		Me.btExport = New TD.SandBar.ButtonItem
		Me.dlgFileBrowser = New System.Windows.Forms.OpenFileDialog
		Me.cbarImpExp.SuspendLayout
		Me.pnlImpExp.SuspendLayout
		Me.grpExport.SuspendLayout
		Me.grpImport.SuspendLayout
		Me.SuspendLayout
		'
		'cbarImpExp
		'
		Me.cbarImpExp.Controls.Add(Me.pnlImpExp)
		Me.cbarImpExp.Dock = System.Windows.Forms.DockStyle.Fill
		Me.cbarImpExp.DrawActionsButton = false
		Me.cbarImpExp.Flow = TD.SandBar.ToolBarLayout.Horizontal
		Me.cbarImpExp.Guid = New System.Guid("f988187e-b847-4ec4-a421-bf66cadf6d4d")
		Me.cbarImpExp.Items.AddRange(New TD.SandBar.ToolbarItemBase() {Me.btImport, Me.btExport})
		Me.cbarImpExp.Location = New System.Drawing.Point(0, 0)
		Me.cbarImpExp.Movable = false
		Me.cbarImpExp.Name = "cbarImpExp"
		Me.cbarImpExp.Size = New System.Drawing.Size(328, 237)
		Me.cbarImpExp.TabIndex = 0
		Me.cbarImpExp.Text = "Module d'importation / exportation"
		AddHandler Me.cbarImpExp.VisibleChanged, AddressOf Me.CbarImpExpVisibleChanged
		AddHandler Me.cbarImpExp.MouseMove, AddressOf Me.CbarImpExpMouseMove
		AddHandler Me.cbarImpExp.MouseDown, AddressOf Me.CbarImpExpMouseDown
		AddHandler Me.cbarImpExp.MouseUp, AddressOf Me.CbarImpExpMouseUp
		'
		'pnlImpExp
		'
		Me.pnlImpExp.Controls.Add(Me.grpExport)
		Me.pnlImpExp.Controls.Add(Me.grpImport)
		Me.pnlImpExp.Location = New System.Drawing.Point(2, 49)
		Me.pnlImpExp.Name = "pnlImpExp"
		Me.pnlImpExp.Size = New System.Drawing.Size(324, 186)
		Me.pnlImpExp.TabIndex = 0
		'
		'grpExport
		'
		Me.grpExport.BackColor = System.Drawing.Color.Transparent
		Me.grpExport.Controls.Add(Me.cboFormat)
		Me.grpExport.Controls.Add(Me.lblFormat)
		Me.grpExport.Controls.Add(Me.cmdExport)
		Me.grpExport.Controls.Add(Me.lstchkSources)
		Me.grpExport.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grpExport.Location = New System.Drawing.Point(0, 0)
		Me.grpExport.Name = "grpExport"
		Me.grpExport.Size = New System.Drawing.Size(324, 186)
		Me.grpExport.TabIndex = 0
		Me.grpExport.TabStop = false
		Me.grpExport.Visible = false
		'
		'cboFormat
		'
		Me.cboFormat.FormattingEnabled = true
		Me.cboFormat.Items.AddRange(New Object() {"MTGM v2 (*.dk2)", "MTGM (*.dck)", "Apprentice (*.dec)", "Magic Workstation (*.mwDeck)", "Site Web (*.html)"})
		Me.cboFormat.Location = New System.Drawing.Point(109, 133)
		Me.cboFormat.Name = "cboFormat"
		Me.cboFormat.Size = New System.Drawing.Size(212, 21)
		Me.cboFormat.TabIndex = 11
		Me.cboFormat.Text = "MTGM v2 (*.dk2)"
		'
		'lblFormat
		'
		Me.lblFormat.Dock = System.Windows.Forms.DockStyle.Left
		Me.lblFormat.Location = New System.Drawing.Point(3, 125)
		Me.lblFormat.Name = "lblFormat"
		Me.lblFormat.Size = New System.Drawing.Size(100, 35)
		Me.lblFormat.TabIndex = 10
		Me.lblFormat.Text = "Format de sortie :"
		Me.lblFormat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		'
		'cmdExport
		'
		Me.cmdExport.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.cmdExport.Location = New System.Drawing.Point(3, 160)
		Me.cmdExport.Name = "cmdExport"
		Me.cmdExport.Size = New System.Drawing.Size(318, 23)
		Me.cmdExport.TabIndex = 6
		Me.cmdExport.Text = "Exporter"
		Me.cmdExport.UseVisualStyleBackColor = true
		AddHandler Me.cmdExport.Click, AddressOf Me.CmdExportClick
		'
		'lstchkSources
		'
		Me.lstchkSources.CheckOnClick = true
		Me.lstchkSources.Dock = System.Windows.Forms.DockStyle.Top
		Me.lstchkSources.FormattingEnabled = true
		Me.lstchkSources.Location = New System.Drawing.Point(3, 16)
		Me.lstchkSources.Name = "lstchkSources"
		Me.lstchkSources.Size = New System.Drawing.Size(318, 109)
		Me.lstchkSources.TabIndex = 5
		'
		'grpImport
		'
		Me.grpImport.BackColor = System.Drawing.Color.Transparent
		Me.grpImport.Controls.Add(Me.lstImp)
		Me.grpImport.Controls.Add(Me.optImpAdd)
		Me.grpImport.Controls.Add(Me.txtSourceImp)
		Me.grpImport.Controls.Add(Me.optImpNew)
		Me.grpImport.Controls.Add(Me.cmdBrowse)
		Me.grpImport.Controls.Add(Me.txtFileImp)
		Me.grpImport.Controls.Add(Me.lblImp)
		Me.grpImport.Controls.Add(Me.cmdImport)
		Me.grpImport.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grpImport.Location = New System.Drawing.Point(0, 0)
		Me.grpImport.Name = "grpImport"
		Me.grpImport.Size = New System.Drawing.Size(324, 186)
		Me.grpImport.TabIndex = 1
		Me.grpImport.TabStop = false
		'
		'lstImp
		'
		Me.lstImp.Enabled = false
		Me.lstImp.FormattingEnabled = true
		Me.lstImp.Location = New System.Drawing.Point(90, 82)
		Me.lstImp.Name = "lstImp"
		Me.lstImp.ScrollAlwaysVisible = true
		Me.lstImp.Size = New System.Drawing.Size(204, 69)
		Me.lstImp.TabIndex = 14
		'
		'optImpAdd
		'
		Me.optImpAdd.AutoSize = true
		Me.optImpAdd.Location = New System.Drawing.Point(11, 82)
		Me.optImpAdd.Name = "optImpAdd"
		Me.optImpAdd.Size = New System.Drawing.Size(73, 17)
		Me.optImpAdd.TabIndex = 13
		Me.optImpAdd.Text = "Ajouter à :"
		Me.optImpAdd.UseVisualStyleBackColor = true
		AddHandler Me.optImpAdd.CheckedChanged, AddressOf Me.OptImpAddCheckedChanged
		'
		'txtSourceImp
		'
		Me.txtSourceImp.Location = New System.Drawing.Point(194, 58)
		Me.txtSourceImp.Name = "txtSourceImp"
		Me.txtSourceImp.Size = New System.Drawing.Size(100, 20)
		Me.txtSourceImp.TabIndex = 12
		Me.txtSourceImp.MaxLength = 50
		'
		'optImpNew
		'
		Me.optImpNew.AutoSize = true
		Me.optImpNew.Checked = true
		Me.optImpNew.Location = New System.Drawing.Point(11, 59)
		Me.optImpNew.Name = "optImpNew"
		Me.optImpNew.Size = New System.Drawing.Size(182, 17)
		Me.optImpNew.TabIndex = 11
		Me.optImpNew.TabStop = true
		Me.optImpNew.Text = "Importer dans un nouveau deck :"
		Me.optImpNew.UseVisualStyleBackColor = true
		AddHandler Me.optImpNew.CheckedChanged, AddressOf Me.OptImpNewCheckedChanged
		'
		'cmdBrowse
		'
		Me.cmdBrowse.Location = New System.Drawing.Point(300, 32)
		Me.cmdBrowse.Name = "cmdBrowse"
		Me.cmdBrowse.Size = New System.Drawing.Size(21, 21)
		Me.cmdBrowse.TabIndex = 10
		Me.cmdBrowse.Text = "."
		Me.cmdBrowse.UseVisualStyleBackColor = true
		AddHandler Me.cmdBrowse.Click, AddressOf Me.CmdBrowseClick
		'
		'txtFileImp
		'
		Me.txtFileImp.Location = New System.Drawing.Point(10, 32)
		Me.txtFileImp.Name = "txtFileImp"
		Me.txtFileImp.Size = New System.Drawing.Size(284, 20)
		Me.txtFileImp.TabIndex = 9
		'
		'lblImp
		'
		Me.lblImp.AutoSize = true
		Me.lblImp.Location = New System.Drawing.Point(10, 16)
		Me.lblImp.Name = "lblImp"
		Me.lblImp.Size = New System.Drawing.Size(93, 13)
		Me.lblImp.TabIndex = 8
		Me.lblImp.Text = "Fichier à importer :"
		'
		'cmdImport
		'
		Me.cmdImport.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.cmdImport.Location = New System.Drawing.Point(3, 160)
		Me.cmdImport.Name = "cmdImport"
		Me.cmdImport.Size = New System.Drawing.Size(318, 23)
		Me.cmdImport.TabIndex = 7
		Me.cmdImport.Text = "Importer"
		Me.cmdImport.UseVisualStyleBackColor = true
		AddHandler Me.cmdImport.Click, AddressOf Me.CmdImportClick
		'
		'btImport
		'
		Me.btImport.Checked = true
		Me.btImport.Icon = CType(resources.GetObject("btImport.Icon"),System.Drawing.Icon)
		Me.btImport.Text = "Import"
		AddHandler Me.btImport.Activate, AddressOf Me.BtImportActivate
		'
		'btExport
		'
		Me.btExport.Icon = CType(resources.GetObject("btExport.Icon"),System.Drawing.Icon)
		Me.btExport.Text = "Export"
		AddHandler Me.btExport.Activate, AddressOf Me.BtExportActivate
		'
		'dlgFileBrowser
		'
		Me.dlgFileBrowser.Filter = "Fichiers MTGM v2 (*.dk2) | *.dk2|Fichiers MTGM (*.dck) | *.dck|Fichiers Magic Master (*.xml) | *.xml|Fichiers Magic Workstation (*.mwDeck) | *.mwDeck|Fichiers Magic Collection (*.txt) | *.txt|Fichiers Magic Online (*.csv) | *.csv|Fichiers XMage (*.dck) | *.dck|Fichiers Urza Gatherer (*.ugs) | *.ugs|Fichiers Urza Gatherer (*.csv) | *.csv"
		Me.dlgFileBrowser.Title = "Sélectionner le fichier à importer"
		'
		'frmExport
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(328, 237)
		Me.Controls.Add(Me.cbarImpExp)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
		Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
		Me.Name = "frmExport"
		AddHandler Load, AddressOf Me.FrmExportLoad
		AddHandler FormClosing, AddressOf Me.FrmExportFormClosing
		Me.cbarImpExp.ResumeLayout(false)
		Me.pnlImpExp.ResumeLayout(false)
		Me.grpExport.ResumeLayout(false)
		Me.grpImport.ResumeLayout(false)
		Me.grpImport.PerformLayout
		Me.ResumeLayout(false)
	End Sub
	Private cboFormat As System.Windows.Forms.ComboBox
	Private lblFormat As System.Windows.Forms.Label
	Private dlgFileBrowser As System.Windows.Forms.OpenFileDialog
	Private optImpNew As System.Windows.Forms.RadioButton
	Private txtSourceImp As System.Windows.Forms.TextBox
	Private optImpAdd As System.Windows.Forms.RadioButton
	Private lstImp As System.Windows.Forms.ListBox
	Private lblImp As System.Windows.Forms.Label
	Private txtFileImp As System.Windows.Forms.TextBox
	Private cmdBrowse As System.Windows.Forms.Button
	Private cmdImport As System.Windows.Forms.Button
	Private grpImport As System.Windows.Forms.GroupBox
	Private btImport As TD.SandBar.ButtonItem
	Private btExport As TD.SandBar.ButtonItem
	Private grpExport As System.Windows.Forms.GroupBox
	Private pnlImpExp As TD.SandBar.ContainerBarClientPanel
	Private cbarImpExp As TD.SandBar.ContainerBar
	Private dlgBrowser As System.Windows.Forms.FolderBrowserDialog
	Private cmdExport As System.Windows.Forms.Button
	Private lstchkSources As System.Windows.Forms.CheckedListBox
End Class
