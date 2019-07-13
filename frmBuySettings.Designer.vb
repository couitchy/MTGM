'
' Crée par SharpDevelop.
' Utilisateur: Couitchy
' Date: 09/07/2011
' Heure: 22:58
'
' Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
'
Partial Class frmBuySettings
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBuySettings))
        Me.toolStrip = New System.Windows.Forms.ToolStrip()
        Me.btRefresh = New System.Windows.Forms.ToolStripButton()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.chklstEditions = New System.Windows.Forms.CheckedListBox()
        Me.toolStrip.SuspendLayout
        Me.SuspendLayout
        '
        'toolStrip
        '
        Me.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.toolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btRefresh})
        Me.toolStrip.Location = New System.Drawing.Point(0, 0)
        Me.toolStrip.Name = "toolStrip"
        Me.toolStrip.Size = New System.Drawing.Size(116, 25)
        Me.toolStrip.TabIndex = 0
        '
        'btRefresh
        '
        Me.btRefresh.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.btRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btRefresh.Image = CType(resources.GetObject("btRefresh.Image"),System.Drawing.Image)
        Me.btRefresh.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btRefresh.Name = "btRefresh"
        Me.btRefresh.Size = New System.Drawing.Size(23, 22)
        Me.btRefresh.Text = "Valider"
        AddHandler Me.btRefresh.Click, AddressOf Me.BtRefreshClick
        '
        'lblTitle
        '
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblTitle.Location = New System.Drawing.Point(0, 25)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(116, 23)
        Me.lblTitle.TabIndex = 6
        '
        'chklstEditions
        '
        Me.chklstEditions.CheckOnClick = true
        Me.chklstEditions.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chklstEditions.FormattingEnabled = true
        Me.chklstEditions.Location = New System.Drawing.Point(0, 48)
        Me.chklstEditions.Name = "chklstEditions"
        Me.chklstEditions.Size = New System.Drawing.Size(116, 103)
        Me.chklstEditions.TabIndex = 7
        AddHandler Me.chklstEditions.ItemCheck, AddressOf Me.ChklstClassementItemCheck
        '
        'frmBuySettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(116, 151)
        Me.Controls.Add(Me.chklstEditions)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.toolStrip)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
        Me.KeyPreview = true
        Me.Name = "frmBuySettings"
        Me.ShowInTaskbar = false
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Filtres d'éditions"
        Me.TopMost = true
        AddHandler KeyUp, AddressOf Me.frmBuySettingsKeyUp
        Me.toolStrip.ResumeLayout(false)
        Me.toolStrip.PerformLayout
        Me.ResumeLayout(false)
        Me.PerformLayout
    End Sub
    Private lblTitle As System.Windows.Forms.Label
    Private chklstEditions As System.Windows.Forms.CheckedListBox
    Private btRefresh As System.Windows.Forms.ToolStripButton
    Private toolStrip As System.Windows.Forms.ToolStrip
End Class
