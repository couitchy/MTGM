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
        Me.splitV = New System.Windows.Forms.SplitContainer
        Me.chklstCurves = New System.Windows.Forms.CheckedListBox
        Me.propCurves = New System.Windows.Forms.PropertyGrid
        Me.dlgCapture = New System.Windows.Forms.SaveFileDialog
        Me.btCoords = New System.Windows.Forms.ToolStripButton
        Me.toolStrip.SuspendLayout
        Me.splitH.Panel1.SuspendLayout
        Me.splitH.Panel2.SuspendLayout
        Me.splitH.SuspendLayout
        Me.splitV.Panel1.SuspendLayout
        Me.splitV.Panel2.SuspendLayout
        Me.splitV.SuspendLayout
        Me.SuspendLayout
        '
        'toolStrip
        '
        Me.toolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btCapture, Me.btSeparator, Me.btEdit, Me.btClear, Me.btCoords})
        Me.toolStrip.Location = New System.Drawing.Point(0, 0)
        Me.toolStrip.Name = "toolStrip"
        Me.toolStrip.Size = New System.Drawing.Size(553, 25)
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
        Me.btEdit.Text = "Editer les param�tres d'affichage"
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
        Me.splitH.Panel2.Controls.Add(Me.splitV)
        Me.splitH.Panel2MinSize = 101
        Me.splitH.Size = New System.Drawing.Size(553, 457)
        Me.splitH.SplitterDistance = 236
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
        Me.plotMain.Size = New System.Drawing.Size(553, 236)
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
        'splitV
        '
        Me.splitV.Dock = System.Windows.Forms.DockStyle.Fill
        Me.splitV.Location = New System.Drawing.Point(0, 0)
        Me.splitV.Name = "splitV"
        '
        'splitV.Panel1
        '
        Me.splitV.Panel1.Controls.Add(Me.chklstCurves)
        '
        'splitV.Panel2
        '
        Me.splitV.Panel2.Controls.Add(Me.propCurves)
        Me.splitV.Size = New System.Drawing.Size(553, 217)
        Me.splitV.SplitterDistance = 251
        Me.splitV.TabIndex = 0
        '
        'chklstCurves
        '
        Me.chklstCurves.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chklstCurves.FormattingEnabled = true
        Me.chklstCurves.Location = New System.Drawing.Point(0, 0)
        Me.chklstCurves.Name = "chklstCurves"
        Me.chklstCurves.Size = New System.Drawing.Size(251, 214)
        Me.chklstCurves.TabIndex = 0
        AddHandler Me.chklstCurves.SelectedIndexChanged, AddressOf Me.ChklstCurvesSelectedIndexChanged
        AddHandler Me.chklstCurves.ItemCheck, AddressOf Me.ChklstCurvesItemCheck
        '
        'propCurves
        '
        Me.propCurves.Dock = System.Windows.Forms.DockStyle.Fill
        Me.propCurves.Location = New System.Drawing.Point(0, 0)
        Me.propCurves.Name = "propCurves"
        Me.propCurves.PropertySort = System.Windows.Forms.PropertySort.NoSort
        Me.propCurves.Size = New System.Drawing.Size(298, 217)
        Me.propCurves.TabIndex = 0
        Me.propCurves.ToolbarVisible = false
        '
        'dlgCapture
        '
        Me.dlgCapture.DefaultExt = "png"
        Me.dlgCapture.Filter = "Fichiers d'image (*.png)|*.png"
        Me.dlgCapture.Title = "S�lection du fichier de sauvegarde"
        '
        'btCoords
        '
        Me.btCoords.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.btCoords.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btCoords.Image = CType(resources.GetObject("btCoords.Image"),System.Drawing.Image)
        Me.btCoords.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btCoords.Name = "btCoords"
        Me.btCoords.Size = New System.Drawing.Size(23, 22)
        Me.btCoords.Text = "Afficher les coordonn�es"
        AddHandler Me.btCoords.Click, AddressOf Me.BtCoordsClick
        '
        'frmGrapher
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(553, 482)
        Me.Controls.Add(Me.splitH)
        Me.Controls.Add(Me.toolStrip)
        Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
        Me.Name = "frmGrapher"
        Me.Text = "Trac�s graphiques"
        AddHandler FormClosing, AddressOf Me.FrmGrapherFormClosing
        Me.toolStrip.ResumeLayout(false)
        Me.toolStrip.PerformLayout
        Me.splitH.Panel1.ResumeLayout(false)
        Me.splitH.Panel2.ResumeLayout(false)
        Me.splitH.ResumeLayout(false)
        Me.splitV.Panel1.ResumeLayout(false)
        Me.splitV.Panel2.ResumeLayout(false)
        Me.splitV.ResumeLayout(false)
        Me.ResumeLayout(false)
        Me.PerformLayout
    End Sub
    Private btCoords As System.Windows.Forms.ToolStripButton
    Private propCurves As System.Windows.Forms.PropertyGrid
    Private chklstCurves As System.Windows.Forms.CheckedListBox
    Private splitV As System.Windows.Forms.SplitContainer
    Private dlgCapture As System.Windows.Forms.SaveFileDialog
    Private btSeparator As System.Windows.Forms.ToolStripSeparator
    Private btClear As System.Windows.Forms.ToolStripButton
    Private btEdit As System.Windows.Forms.ToolStripButton
    Private btCapture As System.Windows.Forms.ToolStripButton
    Private toolStrip As System.Windows.Forms.ToolStrip
    Private splitH As System.Windows.Forms.SplitContainer
    Private plotMain As NPlot.Windows.PlotSurface2D
End Class
