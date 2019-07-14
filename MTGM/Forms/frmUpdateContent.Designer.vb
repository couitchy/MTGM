'
' Crée par SharpDevelop.
' Utilisateur: Couitchy
' Date: 09/06/2010
' Heure: 22:24
'
' Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
'
Partial Class frmUpdateContent
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUpdateContent))
        Me.cbarUpdate = New TD.SandBar.ContainerBar
        Me.pnlUpdate = New TD.SandBar.ContainerBarClientPanel
        Me.grpUpdate = New System.Windows.Forms.GroupBox
        Me.prgWait = New System.Windows.Forms.ProgressBar
        Me.chklstContenus = New System.Windows.Forms.ListView
        Me.colContenu = New System.Windows.Forms.ColumnHeader
        Me.colLocal = New System.Windows.Forms.ColumnHeader
        Me.colServeur = New System.Windows.Forms.ColumnHeader
        Me.colSize = New System.Windows.Forms.ColumnHeader
        Me.cmdUpdate = New System.Windows.Forms.Button
        Me.cbarUpdate.SuspendLayout
        Me.pnlUpdate.SuspendLayout
        Me.grpUpdate.SuspendLayout
        Me.SuspendLayout
        '
        'cbarUpdate
        '
        Me.cbarUpdate.Controls.Add(Me.pnlUpdate)
        Me.cbarUpdate.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cbarUpdate.DrawActionsButton = false
        Me.cbarUpdate.Flow = TD.SandBar.ToolBarLayout.Horizontal
        Me.cbarUpdate.Guid = New System.Guid("f988187e-b847-4ec4-a421-bf66cadf6d4d")
        Me.cbarUpdate.Location = New System.Drawing.Point(0, 0)
        Me.cbarUpdate.Movable = false
        Me.cbarUpdate.Name = "cbarUpdate"
        Me.cbarUpdate.Size = New System.Drawing.Size(514, 262)
        Me.cbarUpdate.TabIndex = 1
        Me.cbarUpdate.Text = "Mises à jour de contenu"
        AddHandler Me.cbarUpdate.VisibleChanged, AddressOf Me.CbarUpdateVisibleChanged
        AddHandler Me.cbarUpdate.MouseMove, AddressOf Me.CbarUpdateMouseMove
        AddHandler Me.cbarUpdate.MouseDown, AddressOf Me.CbarUpdateMouseDown
        AddHandler Me.cbarUpdate.MouseUp, AddressOf Me.CbarUpdateMouseUp
        '
        'pnlUpdate
        '
        Me.pnlUpdate.Controls.Add(Me.grpUpdate)
        Me.pnlUpdate.Location = New System.Drawing.Point(2, 27)
        Me.pnlUpdate.Name = "pnlUpdate"
        Me.pnlUpdate.Size = New System.Drawing.Size(510, 233)
        Me.pnlUpdate.TabIndex = 0
        '
        'grpUpdate
        '
        Me.grpUpdate.BackColor = System.Drawing.Color.Transparent
        Me.grpUpdate.Controls.Add(Me.prgWait)
        Me.grpUpdate.Controls.Add(Me.chklstContenus)
        Me.grpUpdate.Controls.Add(Me.cmdUpdate)
        Me.grpUpdate.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpUpdate.Location = New System.Drawing.Point(0, 0)
        Me.grpUpdate.Name = "grpUpdate"
        Me.grpUpdate.Size = New System.Drawing.Size(510, 233)
        Me.grpUpdate.TabIndex = 0
        Me.grpUpdate.TabStop = false
        '
        'prgWait
        '
        Me.prgWait.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.prgWait.Location = New System.Drawing.Point(3, 197)
        Me.prgWait.Name = "prgWait"
        Me.prgWait.Size = New System.Drawing.Size(504, 10)
        Me.prgWait.TabIndex = 8
        '
        'chklstContenus
        '
        Me.chklstContenus.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colContenu, Me.colLocal, Me.colServeur, Me.colSize})
        Me.chklstContenus.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chklstContenus.Location = New System.Drawing.Point(3, 16)
        Me.chklstContenus.Name = "chklstContenus"
        Me.chklstContenus.Size = New System.Drawing.Size(504, 191)
        Me.chklstContenus.TabIndex = 7
        Me.chklstContenus.UseCompatibleStateImageBehavior = false
        Me.chklstContenus.View = System.Windows.Forms.View.Details
        '
        'colContenu
        '
        Me.colContenu.Text = "Contenu"
        Me.colContenu.Width = 203
        '
        'colLocal
        '
        Me.colLocal.Text = "Version locale"
        Me.colLocal.Width = 90
        '
        'colServeur
        '
        Me.colServeur.Text = "Version serveur"
        Me.colServeur.Width = 90
        '
        'colSize
        '
        Me.colSize.Text = "Taille"
        Me.colSize.Width = 78
        '
        'cmdUpdate
        '
        Me.cmdUpdate.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.cmdUpdate.Location = New System.Drawing.Point(3, 207)
        Me.cmdUpdate.Name = "cmdUpdate"
        Me.cmdUpdate.Size = New System.Drawing.Size(504, 23)
        Me.cmdUpdate.TabIndex = 6
        Me.cmdUpdate.Text = "Mettre à jour les éléments sélectionnés"
        Me.cmdUpdate.UseVisualStyleBackColor = true
        AddHandler Me.cmdUpdate.Click, AddressOf Me.CmdUpdateClick
        '
        'frmUpdateContenu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(514, 262)
        Me.Controls.Add(Me.cbarUpdate)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
        Me.Name = "frmUpdateContenu"
        Me.ShowInTaskbar = false
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Mises à jour de contenu"
        AddHandler Activated, AddressOf Me.FrmUpdateContenuActivated
        Me.cbarUpdate.ResumeLayout(false)
        Me.pnlUpdate.ResumeLayout(false)
        Me.grpUpdate.ResumeLayout(false)
        Me.ResumeLayout(false)
    End Sub
    Private colSize As System.Windows.Forms.ColumnHeader
    Private prgWait As System.Windows.Forms.ProgressBar
    Private pnlUpdate As TD.SandBar.ContainerBarClientPanel
    Private grpUpdate As System.Windows.Forms.GroupBox
    Private colServeur As System.Windows.Forms.ColumnHeader
    Private colLocal As System.Windows.Forms.ColumnHeader
    Private colContenu As System.Windows.Forms.ColumnHeader
    Private chklstContenus As System.Windows.Forms.ListView
    Private cmdUpdate As System.Windows.Forms.Button
    Private cbarUpdate As TD.SandBar.ContainerBar
End Class
