'
' Created by SharpDevelop.
' User: Couitchy
' Date: 20/11/2008
' Time: 18:10
'
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Partial Class frmGestDecks
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmGestDecks))
        Me.cbarDecksManager = New TD.SandBar.ContainerBar()
        Me.pnlDecksManager = New TD.SandBar.ContainerBarClientPanel()
        Me.splitV = New System.Windows.Forms.SplitContainer()
        Me.tvwDecks = New System.Windows.Forms.TreeView()
        Me.imgLstTvw = New System.Windows.Forms.ImageList(Me.components)
        Me.txtMemo = New System.Windows.Forms.TextBox()
        Me.lblMemo = New System.Windows.Forms.Label()
        Me.lblFormat = New System.Windows.Forms.Label()
        Me.cboFormat = New System.Windows.Forms.ComboBox()
        Me.pickDate = New System.Windows.Forms.DateTimePicker()
        Me.lblDate = New System.Windows.Forms.Label()
        Me.btAdd = New TD.SandBar.ButtonItem()
        Me.btAddFolder = New TD.SandBar.ButtonItem()
        Me.btRemove = New TD.SandBar.ButtonItem()
        Me.btRename = New TD.SandBar.ButtonItem()
        Me.btUp = New TD.SandBar.ButtonItem()
        Me.btDown = New TD.SandBar.ButtonItem()
        Me.cbarDecksManager.SuspendLayout
        Me.pnlDecksManager.SuspendLayout
        Me.splitV.Panel1.SuspendLayout
        Me.splitV.Panel2.SuspendLayout
        Me.splitV.SuspendLayout
        Me.SuspendLayout
        '
        'cbarDecksManager
        '
        Me.cbarDecksManager.AddRemoveButtonsVisible = false
        Me.cbarDecksManager.Controls.Add(Me.pnlDecksManager)
        Me.cbarDecksManager.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cbarDecksManager.DrawActionsButton = false
        Me.cbarDecksManager.Flow = TD.SandBar.ToolBarLayout.Horizontal
        Me.cbarDecksManager.Guid = New System.Guid("091e31c8-a054-4ec4-a212-c0bc96ca3063")
        Me.cbarDecksManager.Items.AddRange(New TD.SandBar.ToolbarItemBase() {Me.btAdd, Me.btAddFolder, Me.btRemove, Me.btRename, Me.btUp, Me.btDown})
        Me.cbarDecksManager.Location = New System.Drawing.Point(0, 0)
        Me.cbarDecksManager.Movable = false
        Me.cbarDecksManager.Name = "cbarDecksManager"
        Me.cbarDecksManager.Size = New System.Drawing.Size(646, 278)
        Me.cbarDecksManager.TabIndex = 0
        Me.cbarDecksManager.Text = "Gestion des decks"
        AddHandler Me.cbarDecksManager.VisibleChanged, AddressOf Me.CbarDecksManagerVisibleChanged
        AddHandler Me.cbarDecksManager.MouseDown, AddressOf Me.CbarDecksManagerMouseDown
        AddHandler Me.cbarDecksManager.MouseMove, AddressOf Me.CbarDecksManagerMouseMove
        AddHandler Me.cbarDecksManager.MouseUp, AddressOf Me.CbarDecksManagerMouseUp
        '
        'pnlDecksManager
        '
        Me.pnlDecksManager.Controls.Add(Me.splitV)
        Me.pnlDecksManager.Location = New System.Drawing.Point(2, 49)
        Me.pnlDecksManager.Name = "pnlDecksManager"
        Me.pnlDecksManager.Size = New System.Drawing.Size(642, 227)
        Me.pnlDecksManager.TabIndex = 0
        '
        'splitV
        '
        Me.splitV.Dock = System.Windows.Forms.DockStyle.Fill
        Me.splitV.IsSplitterFixed = true
        Me.splitV.Location = New System.Drawing.Point(0, 0)
        Me.splitV.Name = "splitV"
        '
        'splitV.Panel1
        '
        Me.splitV.Panel1.Controls.Add(Me.tvwDecks)
        '
        'splitV.Panel2
        '
        Me.splitV.Panel2.Controls.Add(Me.txtMemo)
        Me.splitV.Panel2.Controls.Add(Me.lblMemo)
        Me.splitV.Panel2.Controls.Add(Me.lblFormat)
        Me.splitV.Panel2.Controls.Add(Me.cboFormat)
        Me.splitV.Panel2.Controls.Add(Me.pickDate)
        Me.splitV.Panel2.Controls.Add(Me.lblDate)
        Me.splitV.Size = New System.Drawing.Size(642, 227)
        Me.splitV.SplitterDistance = 330
        Me.splitV.TabIndex = 2
        '
        'tvwDecks
        '
        Me.tvwDecks.AllowDrop = true
        Me.tvwDecks.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tvwDecks.HideSelection = false
        Me.tvwDecks.ImageIndex = 0
        Me.tvwDecks.ImageList = Me.imgLstTvw
        Me.tvwDecks.Location = New System.Drawing.Point(0, 0)
        Me.tvwDecks.Name = "tvwDecks"
        Me.tvwDecks.SelectedImageIndex = 0
        Me.tvwDecks.ShowLines = false
        Me.tvwDecks.ShowRootLines = false
        Me.tvwDecks.Size = New System.Drawing.Size(330, 227)
        Me.tvwDecks.TabIndex = 0
        AddHandler Me.tvwDecks.ItemDrag, AddressOf Me.TvwDecksItemDrag
        AddHandler Me.tvwDecks.AfterSelect, AddressOf Me.TvwDecksAfterSelect
        AddHandler Me.tvwDecks.DragDrop, AddressOf Me.TvwDecksDragDrop
        AddHandler Me.tvwDecks.DragEnter, AddressOf Me.TvwDecksDragEnter
        AddHandler Me.tvwDecks.DragOver, AddressOf Me.TvwDecksDragOver
        '
        'imgLstTvw
        '
        Me.imgLstTvw.ImageStream = CType(resources.GetObject("imgLstTvw.ImageStream"),System.Windows.Forms.ImageListStreamer)
        Me.imgLstTvw.TransparentColor = System.Drawing.Color.Transparent
        Me.imgLstTvw.Images.SetKeyName(0, "Deck.ico")
        Me.imgLstTvw.Images.SetKeyName(1, "Folder.ico")
        Me.imgLstTvw.Images.SetKeyName(2, "_tartifact.ico")
        '
        'txtMemo
        '
        Me.txtMemo.Location = New System.Drawing.Point(10, 81)
        Me.txtMemo.Multiline = true
        Me.txtMemo.Name = "txtMemo"
        Me.txtMemo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtMemo.Size = New System.Drawing.Size(280, 136)
        Me.txtMemo.TabIndex = 5
        AddHandler Me.txtMemo.Leave, AddressOf Me.TxtMemoLeave
        '
        'lblMemo
        '
        Me.lblMemo.AutoSize = true
        Me.lblMemo.Location = New System.Drawing.Point(10, 65)
        Me.lblMemo.Name = "lblMemo"
        Me.lblMemo.Size = New System.Drawing.Size(115, 13)
        Me.lblMemo.TabIndex = 4
        Me.lblMemo.Text = "Description et stratégie"
        '
        'lblFormat
        '
        Me.lblFormat.AutoSize = true
        Me.lblFormat.Location = New System.Drawing.Point(10, 39)
        Me.lblFormat.Name = "lblFormat"
        Me.lblFormat.Size = New System.Drawing.Size(71, 13)
        Me.lblFormat.TabIndex = 3
        Me.lblFormat.Text = "Format de jeu"
        '
        'cboFormat
        '
        Me.cboFormat.FormattingEnabled = true
        Me.cboFormat.Location = New System.Drawing.Point(90, 36)
        Me.cboFormat.Name = "cboFormat"
        Me.cboFormat.Size = New System.Drawing.Size(200, 21)
        Me.cboFormat.TabIndex = 2
        Me.cboFormat.Text = "Classique"
        AddHandler Me.cboFormat.Leave, AddressOf Me.CboFormatLeave
        '
        'pickDate
        '
        Me.pickDate.Location = New System.Drawing.Point(90, 10)
        Me.pickDate.Name = "pickDate"
        Me.pickDate.Size = New System.Drawing.Size(200, 20)
        Me.pickDate.TabIndex = 1
        AddHandler Me.pickDate.Leave, AddressOf Me.PickDateLeave
        '
        'lblDate
        '
        Me.lblDate.AutoSize = true
        Me.lblDate.Location = New System.Drawing.Point(10, 13)
        Me.lblDate.Name = "lblDate"
        Me.lblDate.Size = New System.Drawing.Size(75, 13)
        Me.lblDate.TabIndex = 0
        Me.lblDate.Text = "Date associée"
        '
        'btAdd
        '
        Me.btAdd.Icon = CType(resources.GetObject("btAdd.Icon"),System.Drawing.Icon)
        Me.btAdd.Text = "Nouveau deck"
        AddHandler Me.btAdd.Activate, AddressOf Me.BtAddActivate
        '
        'btAddFolder
        '
        Me.btAddFolder.Image = CType(resources.GetObject("btAddFolder.Image"),System.Drawing.Image)
        Me.btAddFolder.Text = "Nouveau dossier"
        AddHandler Me.btAddFolder.Activate, AddressOf Me.BtAddFolderActivate
        '
        'btRemove
        '
        Me.btRemove.Enabled = false
        Me.btRemove.Icon = CType(resources.GetObject("btRemove.Icon"),System.Drawing.Icon)
        Me.btRemove.Text = "Supprimer"
        AddHandler Me.btRemove.Activate, AddressOf Me.BtRemoveActivate
        '
        'btRename
        '
        Me.btRename.Icon = CType(resources.GetObject("btRename.Icon"),System.Drawing.Icon)
        Me.btRename.Text = "Renommer"
        AddHandler Me.btRename.Activate, AddressOf Me.BtRenameActivate
        '
        'btUp
        '
        Me.btUp.BeginGroup = true
        Me.btUp.Enabled = false
        Me.btUp.Image = CType(resources.GetObject("btUp.Image"),System.Drawing.Image)
        Me.btUp.Text = "Monter"
        AddHandler Me.btUp.Activate, AddressOf Me.BtUpActivate
        '
        'btDown
        '
        Me.btDown.Enabled = false
        Me.btDown.Image = CType(resources.GetObject("btDown.Image"),System.Drawing.Image)
        Me.btDown.Text = "Descendre"
        AddHandler Me.btDown.Activate, AddressOf Me.BtDownActivate
        '
        'frmGestDecks
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(646, 278)
        Me.Controls.Add(Me.cbarDecksManager)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
        Me.Name = "frmGestDecks"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Gestion des decks"
        AddHandler FormClosing, AddressOf Me.FrmGestDecksFormClosing
        AddHandler Load, AddressOf Me.FrmGestDecksLoad
        Me.cbarDecksManager.ResumeLayout(false)
        Me.pnlDecksManager.ResumeLayout(false)
        Me.splitV.Panel1.ResumeLayout(false)
        Me.splitV.Panel2.ResumeLayout(false)
        Me.splitV.Panel2.PerformLayout
        Me.splitV.ResumeLayout(false)
        Me.ResumeLayout(false)
    End Sub
    Private btAddFolder As TD.SandBar.ButtonItem
    Private imgLstTvw As System.Windows.Forms.ImageList
    Private tvwDecks As System.Windows.Forms.TreeView
    Private lblDate As System.Windows.Forms.Label
    Private pickDate As System.Windows.Forms.DateTimePicker
    Private cboFormat As System.Windows.Forms.ComboBox
    Private lblFormat As System.Windows.Forms.Label
    Private lblMemo As System.Windows.Forms.Label
    Private txtMemo As System.Windows.Forms.TextBox
    Private splitV As System.Windows.Forms.SplitContainer
    Private btDown As TD.SandBar.ButtonItem
    Private btUp As TD.SandBar.ButtonItem
    Private btRename As TD.SandBar.ButtonItem
    Private btRemove As TD.SandBar.ButtonItem
    Private btAdd As TD.SandBar.ButtonItem
    Private pnlDecksManager As TD.SandBar.ContainerBarClientPanel
    Private cbarDecksManager As TD.SandBar.ContainerBar
End Class
