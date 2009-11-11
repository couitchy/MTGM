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
		Me.optApprentice = New System.Windows.Forms.RadioButton
		Me.optNormal = New System.Windows.Forms.RadioButton
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
		Me.btExport = New TD.SandBar.ButtonItem
		Me.btImport = New TD.SandBar.ButtonItem
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
		Me.cbarImpExp.Items.AddRange(New TD.SandBar.ToolbarItemBase() {Me.btExport, Me.btImport})
		Me.cbarImpExp.Location = New System.Drawing.Point(0, 0)
		Me.cbarImpExp.Movable = false
		Me.cbarImpExp.Name = "cbarImpExp"
		Me.cbarImpExp.Size = New System.Drawing.Size(328, 224)
		Me.cbarImpExp.TabIndex = 0
		Me.cbarImpExp.Text = "Module d'importation / exportation"
		AddHandler Me.cbarImpExp.VisibleChanged, AddressOf Me.CbarImpExpVisibleChanged
		AddHandler Me.cbarImpExp.MouseDown, AddressOf Me.CbarImpExpMouseDown
		AddHandler Me.cbarImpExp.MouseMove, AddressOf Me.CbarImpExpMouseMove
		AddHandler Me.cbarImpExp.MouseUp, AddressOf Me.CbarImpExpMouseUp
		'
		'pnlImpExp
		'
		Me.pnlImpExp.Controls.Add(Me.grpExport)
		Me.pnlImpExp.Controls.Add(Me.grpImport)
		Me.pnlImpExp.Location = New System.Drawing.Point(2, 49)
		Me.pnlImpExp.Name = "pnlImpExp"
		Me.pnlImpExp.Size = New System.Drawing.Size(324, 173)
		Me.pnlImpExp.TabIndex = 0
		'
		'grpExport
		'
		Me.grpExport.BackColor = System.Drawing.Color.Transparent
		Me.grpExport.Controls.Add(Me.optApprentice)
		Me.grpExport.Controls.Add(Me.optNormal)
		Me.grpExport.Controls.Add(Me.cmdExport)
		Me.grpExport.Controls.Add(Me.lstchkSources)
		Me.grpExport.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grpExport.Location = New System.Drawing.Point(0, 0)
		Me.grpExport.Name = "grpExport"
		Me.grpExport.Size = New System.Drawing.Size(324, 173)
		Me.grpExport.TabIndex = 0
		Me.grpExport.TabStop = false
		'
		'optApprentice
		'
		Me.optApprentice.AutoSize = true
		Me.optApprentice.Dock = System.Windows.Forms.DockStyle.Right
		Me.optApprentice.Location = New System.Drawing.Point(210, 125)
		Me.optApprentice.Name = "optApprentice"
		Me.optApprentice.Size = New System.Drawing.Size(111, 22)
		Me.optApprentice.TabIndex = 8
		Me.optApprentice.Text = "Format Apprentice"
		Me.optApprentice.UseVisualStyleBackColor = true
		'
		'optNormal
		'
		Me.optNormal.AutoSize = true
		Me.optNormal.Checked = true
		Me.optNormal.Dock = System.Windows.Forms.DockStyle.Left
		Me.optNormal.Location = New System.Drawing.Point(3, 125)
		Me.optNormal.Name = "optNormal"
		Me.optNormal.Size = New System.Drawing.Size(127, 22)
		Me.optNormal.TabIndex = 7
		Me.optNormal.TabStop = true
		Me.optNormal.Text = "Format MTGM normal"
		Me.optNormal.UseVisualStyleBackColor = true
		'
		'cmdExport
		'
		Me.cmdExport.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.cmdExport.Location = New System.Drawing.Point(3, 147)
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
		Me.grpImport.Size = New System.Drawing.Size(324, 173)
		Me.grpImport.TabIndex = 1
		Me.grpImport.TabStop = false
		Me.grpImport.Visible = false
		'
		'lstImp
		'
		Me.lstImp.Enabled = false
		Me.lstImp.FormattingEnabled = true
		Me.lstImp.Location = New System.Drawing.Point(90, 82)
		Me.lstImp.Name = "lstImp"
		Me.lstImp.ScrollAlwaysVisible = true
		Me.lstImp.Size = New System.Drawing.Size(204, 56)
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
		Me.cmdImport.Location = New System.Drawing.Point(3, 147)
		Me.cmdImport.Name = "cmdImport"
		Me.cmdImport.Size = New System.Drawing.Size(318, 23)
		Me.cmdImport.TabIndex = 7
		Me.cmdImport.Text = "Importer"
		Me.cmdImport.UseVisualStyleBackColor = true
		AddHandler Me.cmdImport.Click, AddressOf Me.CmdImportClick
		'
		'btExport
		'
		Me.btExport.Icon = CType(resources.GetObject("btExport.Icon"),System.Drawing.Icon)
		Me.btExport.Text = "Exporter"
		AddHandler Me.btExport.Activate, AddressOf Me.BtExportActivate
		'
		'btImport
		'
		Me.btImport.Icon = CType(resources.GetObject("btImport.Icon"),System.Drawing.Icon)
		Me.btImport.Text = "Importer"
		AddHandler Me.btImport.Activate, AddressOf Me.BtImportActivate
		'
		'dlgFileBrowser
		'
		Me.dlgFileBrowser.Filter = "Fichiers de deck (*.dck) | *.dck"
		Me.dlgFileBrowser.Title = "Sélectionner le fichier à importer"
		'
		'frmExport
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(328, 224)
		Me.Controls.Add(Me.cbarImpExp)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
		Me.Name = "frmExport"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		AddHandler FormClosing, AddressOf Me.FrmExportFormClosing
		AddHandler Load, AddressOf Me.FrmExportLoad
		Me.cbarImpExp.ResumeLayout(false)
		Me.pnlImpExp.ResumeLayout(false)
		Me.grpExport.ResumeLayout(false)
		Me.grpExport.PerformLayout
		Me.grpImport.ResumeLayout(false)
		Me.grpImport.PerformLayout
		Me.ResumeLayout(false)
	End Sub
	Private optApprentice As System.Windows.Forms.RadioButton
	Private optNormal As System.Windows.Forms.RadioButton
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
