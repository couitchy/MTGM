'
' Created by SharpDevelop.
' User: Couitchy
' Date: 27/07/2009
' Time: 15:21
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Partial Class frmGrapher
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmGrapher))
		Me.toolStrip = New System.Windows.Forms.ToolStrip
		Me.btCapture = New System.Windows.Forms.ToolStripButton
		Me.btSeparator = New System.Windows.Forms.ToolStripSeparator
		Me.btEdit = New System.Windows.Forms.ToolStripButton
		Me.btClear = New System.Windows.Forms.ToolStripButton
		Me.splitH = New System.Windows.Forms.SplitContainer
		Me.plotMain = New NPlot.Windows.PlotSurface2D
		Me.cmdDel = New System.Windows.Forms.Button
		Me.cmdOk = New System.Windows.Forms.Button
		Me.cboStyle = New System.Windows.Forms.ComboBox
		Me.lblColorPick = New System.Windows.Forms.Label
		Me.txtLegend = New System.Windows.Forms.TextBox
		Me.cboPlots = New System.Windows.Forms.ComboBox
		Me.lblStyle = New System.Windows.Forms.Label
		Me.lblColor = New System.Windows.Forms.Label
		Me.lblLegend = New System.Windows.Forms.Label
		Me.dlgColor = New System.Windows.Forms.ColorDialog
		Me.dlgCapture = New System.Windows.Forms.SaveFileDialog
		Me.lblWidth = New System.Windows.Forms.Label
		Me.txtWidth = New System.Windows.Forms.TextBox
		Me.toolStrip.SuspendLayout
		Me.splitH.Panel1.SuspendLayout
		Me.splitH.Panel2.SuspendLayout
		Me.splitH.SuspendLayout
		Me.SuspendLayout
		'
		'toolStrip
		'
		Me.toolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btCapture, Me.btSeparator, Me.btEdit, Me.btClear})
		Me.toolStrip.Location = New System.Drawing.Point(0, 0)
		Me.toolStrip.Name = "toolStrip"
		Me.toolStrip.Size = New System.Drawing.Size(458, 25)
		Me.toolStrip.TabIndex = 1
		Me.toolStrip.Text = "toolStrip1"
		'
		'btCapture
		'
		Me.btCapture.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.btCapture.Image = CType(resources.GetObject("btCapture.Image"),System.Drawing.Image)
		Me.btCapture.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btCapture.Name = "btCapture"
		Me.btCapture.Size = New System.Drawing.Size(23, 22)
		Me.btCapture.Text = "Enregistrer l'image..."
		AddHandler Me.btCapture.Click, AddressOf Me.BtCaptureClick
		'
		'btSeparator
		'
		Me.btSeparator.Name = "btSeparator"
		Me.btSeparator.Size = New System.Drawing.Size(6, 25)
		'
		'btEdit
		'
		Me.btEdit.CheckOnClick = true
		Me.btEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.btEdit.Image = CType(resources.GetObject("btEdit.Image"),System.Drawing.Image)
		Me.btEdit.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btEdit.Name = "btEdit"
		Me.btEdit.Size = New System.Drawing.Size(23, 22)
		Me.btEdit.Text = "Editer les paramètres d'affichage"
		AddHandler Me.btEdit.Click, AddressOf Me.BtEditClick
		'
		'btClear
		'
		Me.btClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.btClear.Image = CType(resources.GetObject("btClear.Image"),System.Drawing.Image)
		Me.btClear.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btClear.Name = "btClear"
		Me.btClear.Size = New System.Drawing.Size(23, 22)
		Me.btClear.Text = "Effacer toutes les courbes"
		AddHandler Me.btClear.Click, AddressOf Me.BtClearClick
		'
		'splitH
		'
		Me.splitH.Dock = System.Windows.Forms.DockStyle.Fill
		Me.splitH.Location = New System.Drawing.Point(0, 25)
		Me.splitH.Name = "splitH"
		Me.splitH.Orientation = System.Windows.Forms.Orientation.Horizontal
		'
		'splitH.Panel1
		'
		Me.splitH.Panel1.Controls.Add(Me.plotMain)
		'
		'splitH.Panel2
		'
		Me.splitH.Panel2.Controls.Add(Me.txtWidth)
		Me.splitH.Panel2.Controls.Add(Me.lblWidth)
		Me.splitH.Panel2.Controls.Add(Me.cmdDel)
		Me.splitH.Panel2.Controls.Add(Me.cmdOk)
		Me.splitH.Panel2.Controls.Add(Me.cboStyle)
		Me.splitH.Panel2.Controls.Add(Me.lblColorPick)
		Me.splitH.Panel2.Controls.Add(Me.txtLegend)
		Me.splitH.Panel2.Controls.Add(Me.cboPlots)
		Me.splitH.Panel2.Controls.Add(Me.lblStyle)
		Me.splitH.Panel2.Controls.Add(Me.lblColor)
		Me.splitH.Panel2.Controls.Add(Me.lblLegend)
		Me.splitH.Panel2MinSize = 101
		Me.splitH.Size = New System.Drawing.Size(458, 260)
		Me.splitH.SplitterDistance = 155
		Me.splitH.TabIndex = 2
		'
		'plotMain
		'
		Me.plotMain.AutoScaleAutoGeneratedAxes = false
		Me.plotMain.AutoScaleTitle = false
		Me.plotMain.BackColor = System.Drawing.SystemColors.ControlLightLight
		Me.plotMain.DateTimeToolTip = false
		Me.plotMain.Dock = System.Windows.Forms.DockStyle.Fill
		Me.plotMain.Legend = Nothing
		Me.plotMain.LegendZOrder = -1
		Me.plotMain.Location = New System.Drawing.Point(0, 0)
		Me.plotMain.Name = "plotMain"
		Me.plotMain.RightMenu = Nothing
		Me.plotMain.ShowCoordinates = false
		Me.plotMain.Size = New System.Drawing.Size(458, 155)
		Me.plotMain.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None
		Me.plotMain.TabIndex = 1
		Me.plotMain.Title = ""
		Me.plotMain.TitleFont = New System.Drawing.Font("Arial", 14!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel)
		Me.plotMain.XAxis1 = Nothing
		Me.plotMain.XAxis2 = Nothing
		Me.plotMain.YAxis1 = Nothing
		Me.plotMain.YAxis2 = Nothing
		AddHandler Me.plotMain.MouseDown, AddressOf Me.PlotMainMouseDown
		'
		'cmdDel
		'
		Me.cmdDel.Location = New System.Drawing.Point(371, 38)
		Me.cmdDel.Name = "cmdDel"
		Me.cmdDel.Size = New System.Drawing.Size(75, 23)
		Me.cmdDel.TabIndex = 9
		Me.cmdDel.Text = "Supprimer"
		Me.cmdDel.UseVisualStyleBackColor = true
		AddHandler Me.cmdDel.Click, AddressOf Me.CmdDelClick
		'
		'cmdOk
		'
		Me.cmdOk.Location = New System.Drawing.Point(371, 67)
		Me.cmdOk.Name = "cmdOk"
		Me.cmdOk.Size = New System.Drawing.Size(75, 23)
		Me.cmdOk.TabIndex = 8
		Me.cmdOk.Text = "Appliquer"
		Me.cmdOk.UseVisualStyleBackColor = true
		AddHandler Me.cmdOk.Click, AddressOf Me.CmdOkClick
		'
		'cboStyle
		'
		Me.cboStyle.FormattingEnabled = true
		Me.cboStyle.Location = New System.Drawing.Point(86, 69)
		Me.cboStyle.Name = "cboStyle"
		Me.cboStyle.Size = New System.Drawing.Size(121, 21)
		Me.cboStyle.TabIndex = 7
		'
		'lblColorPick
		'
		Me.lblColorPick.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lblColorPick.Location = New System.Drawing.Point(86, 44)
		Me.lblColorPick.Name = "lblColorPick"
		Me.lblColorPick.Size = New System.Drawing.Size(121, 23)
		Me.lblColorPick.TabIndex = 6
		AddHandler Me.lblColorPick.Click, AddressOf Me.LblColorPickClick
		'
		'txtLegend
		'
		Me.txtLegend.Location = New System.Drawing.Point(86, 21)
		Me.txtLegend.Name = "txtLegend"
		Me.txtLegend.Size = New System.Drawing.Size(236, 20)
		Me.txtLegend.TabIndex = 5
		'
		'cboPlots
		'
		Me.cboPlots.Dock = System.Windows.Forms.DockStyle.Top
		Me.cboPlots.FormattingEnabled = true
		Me.cboPlots.Location = New System.Drawing.Point(0, 0)
		Me.cboPlots.Name = "cboPlots"
		Me.cboPlots.Size = New System.Drawing.Size(458, 21)
		Me.cboPlots.TabIndex = 4
		AddHandler Me.cboPlots.SelectedIndexChanged, AddressOf Me.CboPlotsSelectedIndexChanged
		'
		'lblStyle
		'
		Me.lblStyle.AutoSize = true
		Me.lblStyle.Location = New System.Drawing.Point(12, 72)
		Me.lblStyle.Name = "lblStyle"
		Me.lblStyle.Size = New System.Drawing.Size(30, 13)
		Me.lblStyle.TabIndex = 3
		Me.lblStyle.Text = "Style"
		'
		'lblColor
		'
		Me.lblColor.AutoSize = true
		Me.lblColor.Location = New System.Drawing.Point(12, 48)
		Me.lblColor.Name = "lblColor"
		Me.lblColor.Size = New System.Drawing.Size(43, 13)
		Me.lblColor.TabIndex = 2
		Me.lblColor.Text = "Couleur"
		'
		'lblLegend
		'
		Me.lblLegend.AutoSize = true
		Me.lblLegend.Location = New System.Drawing.Point(12, 24)
		Me.lblLegend.Name = "lblLegend"
		Me.lblLegend.Size = New System.Drawing.Size(49, 13)
		Me.lblLegend.TabIndex = 1
		Me.lblLegend.Text = "Légende"
		'
		'dlgColor
		'
		Me.dlgColor.AnyColor = true
		Me.dlgColor.FullOpen = true
		'
		'dlgCapture
		'
		Me.dlgCapture.DefaultExt = "png"
		Me.dlgCapture.Filter = "Fichiers d'image (*.png)|*.png"
		Me.dlgCapture.Title = "Sélection du fichier de sauvegarde"
		'
		'lblWidth
		'
		Me.lblWidth.AutoSize = true
		Me.lblWidth.Location = New System.Drawing.Point(228, 72)
		Me.lblWidth.Name = "lblWidth"
		Me.lblWidth.Size = New System.Drawing.Size(53, 13)
		Me.lblWidth.TabIndex = 10
		Me.lblWidth.Text = "Epaisseur"
		'
		'txtWidth
		'
		Me.txtWidth.Location = New System.Drawing.Point(287, 69)
		Me.txtWidth.Name = "txtWidth"
		Me.txtWidth.Size = New System.Drawing.Size(45, 20)
		Me.txtWidth.TabIndex = 11
		Me.txtWidth.Text = "1"
		Me.txtWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		'
		'frmGrapher
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(458, 285)
		Me.Controls.Add(Me.splitH)
		Me.Controls.Add(Me.toolStrip)
		Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
		Me.Name = "frmGrapher"
		Me.Text = "Tracés graphiques"
		AddHandler FormClosing, AddressOf Me.FrmGrapherFormClosing
		Me.toolStrip.ResumeLayout(false)
		Me.toolStrip.PerformLayout
		Me.splitH.Panel1.ResumeLayout(false)
		Me.splitH.Panel2.ResumeLayout(false)
		Me.splitH.Panel2.PerformLayout
		Me.splitH.ResumeLayout(false)
		Me.ResumeLayout(false)
		Me.PerformLayout
	End Sub
	Private lblWidth As System.Windows.Forms.Label
	Private txtWidth As System.Windows.Forms.TextBox
	Private cmdDel As System.Windows.Forms.Button
	Private dlgCapture As System.Windows.Forms.SaveFileDialog
	Private txtLegend As System.Windows.Forms.TextBox
	Private dlgColor As System.Windows.Forms.ColorDialog
	Private lblLegend As System.Windows.Forms.Label
	Private lblColor As System.Windows.Forms.Label
	Private lblStyle As System.Windows.Forms.Label
	Private cboPlots As System.Windows.Forms.ComboBox
	Private lblColorPick As System.Windows.Forms.Label
	Private cboStyle As System.Windows.Forms.ComboBox
	Private cmdOk As System.Windows.Forms.Button
	Private btSeparator As System.Windows.Forms.ToolStripSeparator
	Private btClear As System.Windows.Forms.ToolStripButton
	Private btEdit As System.Windows.Forms.ToolStripButton
	Private btCapture As System.Windows.Forms.ToolStripButton
	Private toolStrip As System.Windows.Forms.ToolStrip
	Private splitH As System.Windows.Forms.SplitContainer
	Private plotMain As NPlot.Windows.PlotSurface2D
End Class
