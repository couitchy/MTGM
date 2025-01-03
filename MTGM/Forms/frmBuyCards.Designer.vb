'
' Created by SharpDevelop.
' User: Couitchy
' Date: 02/09/2009
' Time: 20:38
'
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Partial Class frmBuyCards
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBuyCards))
        Me.splitH = New System.Windows.Forms.SplitContainer()
        Me.cbarGestion = New TD.SandBar.ContainerBar()
        Me.pnlGestion = New TD.SandBar.ContainerBarClientPanel()
        Me.splitV1 = New System.Windows.Forms.SplitContainer()
        Me.lblTransactions = New System.Windows.Forms.Label()
        Me.chkSeller = New System.Windows.Forms.CheckBox()
        Me.chkGood = New System.Windows.Forms.CheckBox()
        Me.chkExcellent = New System.Windows.Forms.CheckBox()
        Me.chkNearMint = New System.Windows.Forms.CheckBox()
        Me.chkMint = New System.Windows.Forms.CheckBox()
        Me.chkTransactions = New System.Windows.Forms.CheckBox()
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.splitV2 = New System.Windows.Forms.SplitContainer()
        Me.lblSeller = New System.Windows.Forms.Label()
        Me.lstSeller = New System.Windows.Forms.ListBox()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdCalc = New System.Windows.Forms.Button()
        Me.prgRefresh = New System.Windows.Forms.ProgressBar()
        Me.cmdRefresh = New System.Windows.Forms.Button()
        Me.txtTot = New System.Windows.Forms.TextBox()
        Me.lblTot = New System.Windows.Forms.Label()
        Me.cbarBasket = New TD.SandBar.ContainerBar()
        Me.pnlBasket = New TD.SandBar.ContainerBarClientPanel()
        Me.grdBasket = New SourceGrid2.Grid()
        Me.btLocalBasket = New TD.SandBar.ButtonItem()
        Me.btRemoteBasket = New TD.SandBar.ButtonItem()
        Me.btSaveBasket = New TD.SandBar.ButtonItem()
        Me.btLoadBasket = New TD.SandBar.ButtonItem()
        Me.cmnuSeller = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuAddSeller = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuRemoveSeller = New System.Windows.Forms.ToolStripMenuItem()
        Me.dlgSave = New System.Windows.Forms.SaveFileDialog()
        Me.dlgOpen = New System.Windows.Forms.OpenFileDialog()
        Me.txtTransaction = New System.Windows.Forms.TextBox()
        Me.splitH.Panel1.SuspendLayout
        Me.splitH.Panel2.SuspendLayout
        Me.splitH.SuspendLayout
        Me.cbarGestion.SuspendLayout
        Me.pnlGestion.SuspendLayout
        Me.splitV1.Panel1.SuspendLayout
        Me.splitV1.Panel2.SuspendLayout
        Me.splitV1.SuspendLayout
        Me.splitV2.Panel1.SuspendLayout
        Me.splitV2.Panel2.SuspendLayout
        Me.splitV2.SuspendLayout
        Me.cbarBasket.SuspendLayout
        Me.pnlBasket.SuspendLayout
        Me.cmnuSeller.SuspendLayout
        Me.SuspendLayout
        '
        'splitH
        '
        Me.splitH.Dock = System.Windows.Forms.DockStyle.Fill
        Me.splitH.IsSplitterFixed = true
        Me.splitH.Location = New System.Drawing.Point(0, 0)
        Me.splitH.Name = "splitH"
        Me.splitH.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'splitH.Panel1
        '
        Me.splitH.Panel1.Controls.Add(Me.cbarGestion)
        '
        'splitH.Panel2
        '
        Me.splitH.Panel2.Controls.Add(Me.cbarBasket)
        Me.splitH.Size = New System.Drawing.Size(588, 355)
        Me.splitH.SplitterDistance = 140
        Me.splitH.TabIndex = 0
        '
        'cbarGestion
        '
        Me.cbarGestion.Closable = false
        Me.cbarGestion.Controls.Add(Me.pnlGestion)
        Me.cbarGestion.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cbarGestion.DrawActionsButton = false
        Me.cbarGestion.Flow = TD.SandBar.ToolBarLayout.Horizontal
        Me.cbarGestion.Guid = New System.Guid("506f1fca-6690-4897-b4e1-9ee0febd6deb")
        Me.cbarGestion.Location = New System.Drawing.Point(0, 0)
        Me.cbarGestion.Movable = false
        Me.cbarGestion.Name = "cbarGestion"
        Me.cbarGestion.Size = New System.Drawing.Size(588, 140)
        Me.cbarGestion.TabIndex = 0
        Me.cbarGestion.Text = "Gestion"
        '
        'pnlGestion
        '
        Me.pnlGestion.Controls.Add(Me.splitV1)
        Me.pnlGestion.Location = New System.Drawing.Point(2, 27)
        Me.pnlGestion.Name = "pnlGestion"
        Me.pnlGestion.Size = New System.Drawing.Size(584, 111)
        Me.pnlGestion.TabIndex = 0
        '
        'splitV1
        '
        Me.splitV1.BackColor = System.Drawing.Color.Transparent
        Me.splitV1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.splitV1.Location = New System.Drawing.Point(0, 0)
        Me.splitV1.Name = "splitV1"
        '
        'splitV1.Panel1
        '
        Me.splitV1.Panel1.Controls.Add(Me.txtTransaction)
        Me.splitV1.Panel1.Controls.Add(Me.lblTransactions)
        Me.splitV1.Panel1.Controls.Add(Me.chkSeller)
        Me.splitV1.Panel1.Controls.Add(Me.chkGood)
        Me.splitV1.Panel1.Controls.Add(Me.chkExcellent)
        Me.splitV1.Panel1.Controls.Add(Me.chkNearMint)
        Me.splitV1.Panel1.Controls.Add(Me.chkMint)
        Me.splitV1.Panel1.Controls.Add(Me.chkTransactions)
        Me.splitV1.Panel1.Controls.Add(Me.lblInfo)
        '
        'splitV1.Panel2
        '
        Me.splitV1.Panel2.Controls.Add(Me.splitV2)
        Me.splitV1.Size = New System.Drawing.Size(584, 111)
        Me.splitV1.SplitterDistance = 235
        Me.splitV1.TabIndex = 0
        '
        'lblTransactions
        '
        Me.lblTransactions.AutoSize = true
        Me.lblTransactions.BackColor = System.Drawing.Color.Transparent
        Me.lblTransactions.Location = New System.Drawing.Point(203, 32)
        Me.lblTransactions.Name = "lblTransactions"
        Me.lblTransactions.Size = New System.Drawing.Size(13, 13)
        Me.lblTransactions.TabIndex = 15
        Me.lblTransactions.Text = "�"
        '
        'chkSeller
        '
        Me.chkSeller.AutoSize = true
        Me.chkSeller.Location = New System.Drawing.Point(10, 88)
        Me.chkSeller.Name = "chkSeller"
        Me.chkSeller.Size = New System.Drawing.Size(150, 17)
        Me.chkSeller.TabIndex = 14
        Me.chkSeller.Text = "Exclure les vendeurs list�s"
        Me.chkSeller.UseVisualStyleBackColor = true
        AddHandler Me.chkSeller.CheckedChanged, AddressOf Me.ChkSellerCheckedChanged
        '
        'chkGood
        '
        Me.chkGood.AutoSize = true
        Me.chkGood.Location = New System.Drawing.Point(164, 60)
        Me.chkGood.Name = "chkGood"
        Me.chkGood.Size = New System.Drawing.Size(52, 17)
        Me.chkGood.TabIndex = 13
        Me.chkGood.Text = "Good"
        Me.chkGood.UseVisualStyleBackColor = true
        '
        'chkExcellent
        '
        Me.chkExcellent.AutoSize = true
        Me.chkExcellent.Location = New System.Drawing.Point(111, 60)
        Me.chkExcellent.Name = "chkExcellent"
        Me.chkExcellent.Size = New System.Drawing.Size(47, 17)
        Me.chkExcellent.TabIndex = 12
        Me.chkExcellent.Text = "Exc."
        Me.chkExcellent.UseVisualStyleBackColor = true
        '
        'chkNearMint
        '
        Me.chkNearMint.AutoSize = true
        Me.chkNearMint.Checked = true
        Me.chkNearMint.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkNearMint.Location = New System.Drawing.Point(62, 60)
        Me.chkNearMint.Name = "chkNearMint"
        Me.chkNearMint.Size = New System.Drawing.Size(43, 17)
        Me.chkNearMint.TabIndex = 11
        Me.chkNearMint.Text = "NM"
        Me.chkNearMint.UseVisualStyleBackColor = true
        '
        'chkMint
        '
        Me.chkMint.AutoSize = true
        Me.chkMint.Checked = true
        Me.chkMint.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkMint.Location = New System.Drawing.Point(10, 60)
        Me.chkMint.Name = "chkMint"
        Me.chkMint.Size = New System.Drawing.Size(46, 17)
        Me.chkMint.TabIndex = 10
        Me.chkMint.Text = "Mint"
        Me.chkMint.UseVisualStyleBackColor = true
        '
        'chkTransactions
        '
        Me.chkTransactions.AutoSize = true
        Me.chkTransactions.Location = New System.Drawing.Point(10, 32)
        Me.chkTransactions.Name = "chkTransactions"
        Me.chkTransactions.Size = New System.Drawing.Size(138, 17)
        Me.chkTransactions.TabIndex = 9
        Me.chkTransactions.Text = "Eviter les transactions <"
        Me.chkTransactions.UseVisualStyleBackColor = true
        AddHandler Me.chkTransactions.CheckedChanged, AddressOf Me.ChkTransactionsCheckedChanged
        '
        'lblInfo
        '
        Me.lblInfo.AutoSize = true
        Me.lblInfo.BackColor = System.Drawing.Color.Transparent
        Me.lblInfo.Location = New System.Drawing.Point(9, 7)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(188, 13)
        Me.lblInfo.TabIndex = 4
        Me.lblInfo.Text = "Optimiser les achats selon les crit�res :"
        '
        'splitV2
        '
        Me.splitV2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.splitV2.Location = New System.Drawing.Point(0, 0)
        Me.splitV2.Name = "splitV2"
        '
        'splitV2.Panel1
        '
        Me.splitV2.Panel1.Controls.Add(Me.lblSeller)
        Me.splitV2.Panel1.Controls.Add(Me.lstSeller)
        '
        'splitV2.Panel2
        '
        Me.splitV2.Panel2.Controls.Add(Me.cmdCancel)
        Me.splitV2.Panel2.Controls.Add(Me.cmdCalc)
        Me.splitV2.Panel2.Controls.Add(Me.prgRefresh)
        Me.splitV2.Panel2.Controls.Add(Me.cmdRefresh)
        Me.splitV2.Panel2.Controls.Add(Me.txtTot)
        Me.splitV2.Panel2.Controls.Add(Me.lblTot)
        Me.splitV2.Size = New System.Drawing.Size(345, 111)
        Me.splitV2.SplitterDistance = 187
        Me.splitV2.TabIndex = 0
        '
        'lblSeller
        '
        Me.lblSeller.BackColor = System.Drawing.Color.Transparent
        Me.lblSeller.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblSeller.Location = New System.Drawing.Point(0, 0)
        Me.lblSeller.Name = "lblSeller"
        Me.lblSeller.Size = New System.Drawing.Size(187, 29)
        Me.lblSeller.TabIndex = 1
        Me.lblSeller.Text = "Liste des vendeurs exclus :"
        Me.lblSeller.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lstSeller
        '
        Me.lstSeller.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lstSeller.FormattingEnabled = true
        Me.lstSeller.Location = New System.Drawing.Point(0, 29)
        Me.lstSeller.Name = "lstSeller"
        Me.lstSeller.Size = New System.Drawing.Size(187, 82)
        Me.lstSeller.TabIndex = 0
        AddHandler Me.lstSeller.MouseUp, AddressOf Me.LstSellerMouseUp
        '
        'cmdCancel
        '
        Me.cmdCancel.Location = New System.Drawing.Point(8, 26)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(141, 23)
        Me.cmdCancel.TabIndex = 3
        Me.cmdCancel.Text = "Annuler"
        Me.cmdCancel.UseVisualStyleBackColor = true
        Me.cmdCancel.Visible = false
        AddHandler Me.cmdCancel.Click, AddressOf Me.CmdCancelClick
        '
        'cmdCalc
        '
        Me.cmdCalc.Enabled = false
        Me.cmdCalc.Location = New System.Drawing.Point(8, 51)
        Me.cmdCalc.Name = "cmdCalc"
        Me.cmdCalc.Size = New System.Drawing.Size(141, 23)
        Me.cmdCalc.TabIndex = 4
        Me.cmdCalc.Text = "Calculer les transactions"
        Me.cmdCalc.UseVisualStyleBackColor = true
        AddHandler Me.cmdCalc.Click, AddressOf Me.CmdCalcClick
        '
        'prgRefresh
        '
        Me.prgRefresh.Dock = System.Windows.Forms.DockStyle.Top
        Me.prgRefresh.Location = New System.Drawing.Point(0, 0)
        Me.prgRefresh.Name = "prgRefresh"
        Me.prgRefresh.Size = New System.Drawing.Size(154, 23)
        Me.prgRefresh.TabIndex = 3
        '
        'cmdRefresh
        '
        Me.cmdRefresh.Location = New System.Drawing.Point(8, 26)
        Me.cmdRefresh.Name = "cmdRefresh"
        Me.cmdRefresh.Size = New System.Drawing.Size(141, 23)
        Me.cmdRefresh.TabIndex = 2
        Me.cmdRefresh.Text = "Actualiser les offres"
        Me.cmdRefresh.UseVisualStyleBackColor = true
        AddHandler Me.cmdRefresh.Click, AddressOf Me.CmdRefreshClick
        '
        'txtTot
        '
        Me.txtTot.Enabled = false
        Me.txtTot.Location = New System.Drawing.Point(70, 82)
        Me.txtTot.Name = "txtTot"
        Me.txtTot.Size = New System.Drawing.Size(79, 20)
        Me.txtTot.TabIndex = 1
        Me.txtTot.Text = "N/A"
        Me.txtTot.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblTot
        '
        Me.lblTot.Location = New System.Drawing.Point(8, 77)
        Me.lblTot.Name = "lblTot"
        Me.lblTot.Size = New System.Drawing.Size(63, 29)
        Me.lblTot.TabIndex = 0
        Me.lblTot.Text = "Prix total (hors port) :"
        Me.lblTot.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbarBasket
        '
        Me.cbarBasket.Closable = false
        Me.cbarBasket.Controls.Add(Me.pnlBasket)
        Me.cbarBasket.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cbarBasket.DrawActionsButton = false
        Me.cbarBasket.Flow = TD.SandBar.ToolBarLayout.Horizontal
        Me.cbarBasket.Guid = New System.Guid("0201851e-8b6d-4b50-81e7-8ea9ba9ea4d9")
        Me.cbarBasket.Items.AddRange(New TD.SandBar.ToolbarItemBase() {Me.btLocalBasket, Me.btRemoteBasket, Me.btSaveBasket, Me.btLoadBasket})
        Me.cbarBasket.Location = New System.Drawing.Point(0, 0)
        Me.cbarBasket.Movable = false
        Me.cbarBasket.Name = "cbarBasket"
        Me.cbarBasket.Size = New System.Drawing.Size(588, 211)
        Me.cbarBasket.TabIndex = 0
        Me.cbarBasket.Text = "Panier"
        '
        'pnlBasket
        '
        Me.pnlBasket.Controls.Add(Me.grdBasket)
        Me.pnlBasket.Location = New System.Drawing.Point(2, 49)
        Me.pnlBasket.Name = "pnlBasket"
        Me.pnlBasket.Size = New System.Drawing.Size(584, 160)
        Me.pnlBasket.TabIndex = 0
        '
        'grdBasket
        '
        Me.grdBasket.AutoSizeMinHeight = 10
        Me.grdBasket.AutoSizeMinWidth = 10
        Me.grdBasket.AutoStretchColumnsToFitWidth = false
        Me.grdBasket.AutoStretchRowsToFitHeight = false
        Me.grdBasket.BackColor = System.Drawing.Color.Transparent
        Me.grdBasket.ContextMenuStyle = SourceGrid2.ContextMenuStyle.None
        Me.grdBasket.CustomSort = false
        Me.grdBasket.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grdBasket.GridToolTipActive = true
        Me.grdBasket.Location = New System.Drawing.Point(0, 0)
        Me.grdBasket.Name = "grdBasket"
        Me.grdBasket.Size = New System.Drawing.Size(584, 160)
        Me.grdBasket.SpecialKeys = CType(((((((((((SourceGrid2.GridSpecialKeys.Ctrl_C Or SourceGrid2.GridSpecialKeys.Ctrl_V)  _
                        Or SourceGrid2.GridSpecialKeys.Ctrl_X)  _
                        Or SourceGrid2.GridSpecialKeys.Delete)  _
                        Or SourceGrid2.GridSpecialKeys.Arrows)  _
                        Or SourceGrid2.GridSpecialKeys.Tab)  _
                        Or SourceGrid2.GridSpecialKeys.PageDownUp)  _
                        Or SourceGrid2.GridSpecialKeys.Enter)  _
                        Or SourceGrid2.GridSpecialKeys.Escape)  _
                        Or SourceGrid2.GridSpecialKeys.Control)  _
                        Or SourceGrid2.GridSpecialKeys.Shift),SourceGrid2.GridSpecialKeys)
        Me.grdBasket.TabIndex = 0
        '
        'btLocalBasket
        '
        Me.btLocalBasket.Icon = CType(resources.GetObject("btLocalBasket.Icon"),System.Drawing.Icon)
        Me.btLocalBasket.Text = "Cartes � acheter"
        AddHandler Me.btLocalBasket.Activate, AddressOf Me.BtLocalBasketActivate
        '
        'btRemoteBasket
        '
        Me.btRemoteBasket.Icon = CType(resources.GetObject("btRemoteBasket.Icon"),System.Drawing.Icon)
        Me.btRemoteBasket.Text = "R�sultat de la recherche"
        AddHandler Me.btRemoteBasket.Activate, AddressOf Me.BtRemoteBasketActivate
        '
        'btSaveBasket
        '
        Me.btSaveBasket.Icon = CType(resources.GetObject("btSaveBasket.Icon"),System.Drawing.Icon)
        Me.btSaveBasket.Text = "Sauvegarder le panier"
        AddHandler Me.btSaveBasket.Activate, AddressOf Me.BtSaveBasketActivate
        '
        'btLoadBasket
        '
        Me.btLoadBasket.Icon = CType(resources.GetObject("btLoadBasket.Icon"),System.Drawing.Icon)
        Me.btLoadBasket.Text = "Charger un panier"
        AddHandler Me.btLoadBasket.Activate, AddressOf Me.BtLoadBasketActivate
        '
        'cmnuSeller
        '
        Me.cmnuSeller.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAddSeller, Me.mnuRemoveSeller})
        Me.cmnuSeller.Name = "cmnuSeller"
        Me.cmnuSeller.Size = New System.Drawing.Size(130, 48)
        '
        'mnuAddSeller
        '
        Me.mnuAddSeller.Name = "mnuAddSeller"
        Me.mnuAddSeller.Size = New System.Drawing.Size(129, 22)
        Me.mnuAddSeller.Text = "Ajouter"
        AddHandler Me.mnuAddSeller.Click, AddressOf Me.MnuAddSellerClick
        '
        'mnuRemoveSeller
        '
        Me.mnuRemoveSeller.Name = "mnuRemoveSeller"
        Me.mnuRemoveSeller.Size = New System.Drawing.Size(129, 22)
        Me.mnuRemoveSeller.Text = "Supprimer"
        AddHandler Me.mnuRemoveSeller.Click, AddressOf Me.MnuRemoveSellerClick
        '
        'dlgSave
        '
        Me.dlgSave.DefaultExt = "mcb"
        Me.dlgSave.Filter = "Magic Cards Baskets (*.mcb) | *.mcb"
        Me.dlgSave.Title = "Enregistrement du panier"
        '
        'dlgOpen
        '
        Me.dlgOpen.DefaultExt = "mcb"
        Me.dlgOpen.Filter = "Magic Cards Baskets (*.mcb) | *.mcb"
        Me.dlgOpen.Title = "Chargement du panier"
        '
        'txtTransaction
        '
        Me.txtTransaction.Enabled = false
        Me.txtTransaction.Location = New System.Drawing.Point(154, 30)
        Me.txtTransaction.Name = "txtTransaction"
        Me.txtTransaction.Size = New System.Drawing.Size(43, 20)
        Me.txtTransaction.TabIndex = 16
        Me.txtTransaction.Text = "2"
        Me.txtTransaction.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'frmBuyCards
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(588, 355)
        Me.Controls.Add(Me.splitH)
        Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
        Me.Name = "frmBuyCards"
        Me.Text = "Achats de cartes sur Internet"
        AddHandler Load, AddressOf Me.FrmBuyCardsLoad
        AddHandler Resize, AddressOf Me.FrmBuyCardsResize
        Me.splitH.Panel1.ResumeLayout(false)
        Me.splitH.Panel2.ResumeLayout(false)
        Me.splitH.ResumeLayout(false)
        Me.cbarGestion.ResumeLayout(false)
        Me.pnlGestion.ResumeLayout(false)
        Me.splitV1.Panel1.ResumeLayout(false)
        Me.splitV1.Panel1.PerformLayout
        Me.splitV1.Panel2.ResumeLayout(false)
        Me.splitV1.ResumeLayout(false)
        Me.splitV2.Panel1.ResumeLayout(false)
        Me.splitV2.Panel2.ResumeLayout(false)
        Me.splitV2.Panel2.PerformLayout
        Me.splitV2.ResumeLayout(false)
        Me.cbarBasket.ResumeLayout(false)
        Me.pnlBasket.ResumeLayout(false)
        Me.cmnuSeller.ResumeLayout(false)
        Me.ResumeLayout(false)
    End Sub
    Private txtTransaction As System.Windows.Forms.TextBox
    Private lblTransactions As System.Windows.Forms.Label
    Private chkNearMint As System.Windows.Forms.CheckBox
    Private chkExcellent As System.Windows.Forms.CheckBox
    Private chkGood As System.Windows.Forms.CheckBox
    Private chkSeller As System.Windows.Forms.CheckBox
    Private chkTransactions As System.Windows.Forms.CheckBox
    Private chkMint As System.Windows.Forms.CheckBox
    Private cmdCancel As System.Windows.Forms.Button
    Private dlgOpen As System.Windows.Forms.OpenFileDialog
    Private dlgSave As System.Windows.Forms.SaveFileDialog
    Private btLoadBasket As TD.SandBar.ButtonItem
    Private btSaveBasket As TD.SandBar.ButtonItem
    Private mnuRemoveSeller As System.Windows.Forms.ToolStripMenuItem
    Private mnuAddSeller As System.Windows.Forms.ToolStripMenuItem
    Private cmnuSeller As System.Windows.Forms.ContextMenuStrip
    Private cmdCalc As System.Windows.Forms.Button
    Private btRemoteBasket As TD.SandBar.ButtonItem
    Private btLocalBasket As TD.SandBar.ButtonItem
    Private prgRefresh As System.Windows.Forms.ProgressBar
    Private txtTot As System.Windows.Forms.TextBox
    Private cmdRefresh As System.Windows.Forms.Button
    Private lblTot As System.Windows.Forms.Label
    Private lstSeller As System.Windows.Forms.ListBox
    Private lblSeller As System.Windows.Forms.Label
    Private splitV2 As System.Windows.Forms.SplitContainer
    Private lblInfo As System.Windows.Forms.Label
    Private splitV1 As System.Windows.Forms.SplitContainer
    Private cbarGestion As TD.SandBar.ContainerBar
    Private pnlGestion As TD.SandBar.ContainerBarClientPanel
    Private splitH As System.Windows.Forms.SplitContainer
    Private cbarBasket As TD.SandBar.ContainerBar
    Private pnlBasket As TD.SandBar.ContainerBarClientPanel
    Private grdBasket As SourceGrid2.Grid
End Class
