'
' Crée par SharpDevelop.
' Utilisateur: Couitchy
' Date: 18/01/2013
' Heure: 19:39
'
' Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
'
Partial Class MainForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.statusStrip = New System.Windows.Forms.StatusStrip()
        Me.toolStrip = New System.Windows.Forms.ToolStrip()
        Me.btDBOpen = New System.Windows.Forms.ToolStripButton()
        Me.btHTMLExport = New System.Windows.Forms.ToolStripButton()
        Me.menuStrip = New System.Windows.Forms.MenuStrip()
        Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuDBOpen = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSeparator = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTools = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuJSONExport = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuHTMLExport = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAbout = New System.Windows.Forms.ToolStripMenuItem()
        Me.dlgOpen = New System.Windows.Forms.OpenFileDialog()
        Me.chklstDecksDispos = New System.Windows.Forms.CheckedListBox()
        Me.dlgBrowser = New System.Windows.Forms.FolderBrowserDialog()
        Me.toolStrip.SuspendLayout
        Me.menuStrip.SuspendLayout
        Me.SuspendLayout
        '
        'statusStrip
        '
        Me.statusStrip.Location = New System.Drawing.Point(0, 240)
        Me.statusStrip.Name = "statusStrip"
        Me.statusStrip.Size = New System.Drawing.Size(281, 22)
        Me.statusStrip.TabIndex = 5
        Me.statusStrip.Text = "statusStrip1"
        '
        'toolStrip
        '
        Me.toolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btDBOpen, Me.btHTMLExport})
        Me.toolStrip.Location = New System.Drawing.Point(0, 24)
        Me.toolStrip.Name = "toolStrip"
        Me.toolStrip.Size = New System.Drawing.Size(281, 25)
        Me.toolStrip.TabIndex = 4
        Me.toolStrip.Text = "toolStrip1"
        '
        'btDBOpen
        '
        Me.btDBOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btDBOpen.Image = CType(resources.GetObject("btDBOpen.Image"),System.Drawing.Image)
        Me.btDBOpen.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btDBOpen.Name = "btDBOpen"
        Me.btDBOpen.Size = New System.Drawing.Size(23, 22)
        Me.btDBOpen.Text = "Base de données source"
        AddHandler Me.btDBOpen.Click, AddressOf Me.MnuDBOpenClick
        '
        'btHTMLExport
        '
        Me.btHTMLExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btHTMLExport.Image = CType(resources.GetObject("btHTMLExport.Image"),System.Drawing.Image)
        Me.btHTMLExport.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btHTMLExport.Name = "btHTMLExport"
        Me.btHTMLExport.Size = New System.Drawing.Size(23, 22)
        Me.btHTMLExport.Text = "HTML Generator"
        AddHandler Me.btHTMLExport.Click, AddressOf Me.MnuHTMLExportClick
        '
        'menuStrip
        '
        Me.menuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile, Me.mnuTools, Me.mnuHelp})
        Me.menuStrip.Location = New System.Drawing.Point(0, 0)
        Me.menuStrip.Name = "menuStrip"
        Me.menuStrip.Size = New System.Drawing.Size(281, 24)
        Me.menuStrip.TabIndex = 3
        Me.menuStrip.Text = "menuStrip1"
        '
        'mnuFile
        '
        Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuDBOpen, Me.mnuSeparator, Me.mnuExit})
        Me.mnuFile.Name = "mnuFile"
        Me.mnuFile.Size = New System.Drawing.Size(54, 20)
        Me.mnuFile.Text = "Fichier"
        '
        'mnuDBOpen
        '
        Me.mnuDBOpen.Image = CType(resources.GetObject("mnuDBOpen.Image"),System.Drawing.Image)
        Me.mnuDBOpen.Name = "mnuDBOpen"
        Me.mnuDBOpen.Size = New System.Drawing.Size(200, 22)
        Me.mnuDBOpen.Text = "Base de données source"
        AddHandler Me.mnuDBOpen.Click, AddressOf Me.MnuDBOpenClick
        '
        'mnuSeparator
        '
        Me.mnuSeparator.Name = "mnuSeparator"
        Me.mnuSeparator.Size = New System.Drawing.Size(197, 6)
        '
        'mnuExit
        '
        Me.mnuExit.Image = CType(resources.GetObject("mnuExit.Image"),System.Drawing.Image)
        Me.mnuExit.Name = "mnuExit"
        Me.mnuExit.Size = New System.Drawing.Size(200, 22)
        Me.mnuExit.Text = "Quitter"
        AddHandler Me.mnuExit.Click, AddressOf Me.MnuExitClick
        '
        'mnuTools
        '
        Me.mnuTools.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuJSONExport, Me.mnuHTMLExport})
        Me.mnuTools.Name = "mnuTools"
        Me.mnuTools.Size = New System.Drawing.Size(50, 20)
        Me.mnuTools.Text = "Outils"
        '
        'mnuJSONExport
        '
        Me.mnuJSONExport.Image = CType(resources.GetObject("mnuJSONExport.Image"),System.Drawing.Image)
        Me.mnuJSONExport.Name = "mnuJSONExport"
        Me.mnuJSONExport.Size = New System.Drawing.Size(162, 22)
        Me.mnuJSONExport.Text = "JSON Exporter"
        AddHandler Me.mnuJSONExport.Click, AddressOf Me.MnuJSONExportClick
        '
        'mnuHTMLExport
        '
        Me.mnuHTMLExport.Image = CType(resources.GetObject("mnuHTMLExport.Image"),System.Drawing.Image)
        Me.mnuHTMLExport.Name = "mnuHTMLExport"
        Me.mnuHTMLExport.Size = New System.Drawing.Size(162, 22)
        Me.mnuHTMLExport.Text = "HTML Generator"
        AddHandler Me.mnuHTMLExport.Click, AddressOf Me.MnuHTMLExportClick
        '
        'mnuHelp
        '
        Me.mnuHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAbout})
        Me.mnuHelp.Name = "mnuHelp"
        Me.mnuHelp.Size = New System.Drawing.Size(24, 20)
        Me.mnuHelp.Text = "?"
        '
        'mnuAbout
        '
        Me.mnuAbout.Image = CType(resources.GetObject("mnuAbout.Image"),System.Drawing.Image)
        Me.mnuAbout.Name = "mnuAbout"
        Me.mnuAbout.Size = New System.Drawing.Size(122, 22)
        Me.mnuAbout.Text = "A propos"
        AddHandler Me.mnuAbout.Click, AddressOf Me.MnuAboutClick
        '
        'dlgOpen
        '
        Me.dlgOpen.DefaultExt = "mdb"
        Me.dlgOpen.Filter = "Fichiers de base de données Microsoft Access (*.mdb)|*.mdb"
        Me.dlgOpen.Title = "Sélection de la base de données"
        '
        'chklstDecksDispos
        '
        Me.chklstDecksDispos.CheckOnClick = true
        Me.chklstDecksDispos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chklstDecksDispos.FormattingEnabled = true
        Me.chklstDecksDispos.Location = New System.Drawing.Point(0, 49)
        Me.chklstDecksDispos.Name = "chklstDecksDispos"
        Me.chklstDecksDispos.Size = New System.Drawing.Size(281, 191)
        Me.chklstDecksDispos.TabIndex = 7
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(281, 262)
        Me.Controls.Add(Me.chklstDecksDispos)
        Me.Controls.Add(Me.statusStrip)
        Me.Controls.Add(Me.toolStrip)
        Me.Controls.Add(Me.menuStrip)
        Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
        Me.Name = "MainForm"
        Me.Text = "CollectionViewer"
        Me.toolStrip.ResumeLayout(false)
        Me.toolStrip.PerformLayout
        Me.menuStrip.ResumeLayout(false)
        Me.menuStrip.PerformLayout
        Me.ResumeLayout(false)
        Me.PerformLayout
    End Sub
    Private btHTMLExport As System.Windows.Forms.ToolStripButton
    Private mnuHTMLExport As System.Windows.Forms.ToolStripMenuItem
    Private dlgBrowser As System.Windows.Forms.FolderBrowserDialog
    Private chklstDecksDispos As System.Windows.Forms.CheckedListBox
    Private mnuJSONExport As System.Windows.Forms.ToolStripMenuItem
    Private dlgOpen As System.Windows.Forms.OpenFileDialog
    Private mnuAbout As System.Windows.Forms.ToolStripMenuItem
    Private mnuHelp As System.Windows.Forms.ToolStripMenuItem
    Private mnuTools As System.Windows.Forms.ToolStripMenuItem
    Private mnuExit As System.Windows.Forms.ToolStripMenuItem
    Private mnuSeparator As System.Windows.Forms.ToolStripSeparator
    Private mnuDBOpen As System.Windows.Forms.ToolStripMenuItem
    Private mnuFile As System.Windows.Forms.ToolStripMenuItem
    Private menuStrip As System.Windows.Forms.MenuStrip
    Private btDBOpen As System.Windows.Forms.ToolStripButton
    Private toolStrip As System.Windows.Forms.ToolStrip
    Private statusStrip As System.Windows.Forms.StatusStrip
End Class
