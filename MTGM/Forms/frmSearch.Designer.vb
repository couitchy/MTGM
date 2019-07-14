'
' Created by SharpDevelop.
' User: Couitchy
' Date: 24/04/2008
' Time: 22:12
'
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Partial Class frmSearch
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSearch))
        Me.cbarSearch = New TD.SandBar.ContainerBar()
        Me.pnlSearch = New TD.SandBar.ContainerBarClientPanel()
        Me.grpSearch = New System.Windows.Forms.GroupBox()
        Me.grpRestriction = New System.Windows.Forms.GroupBox()
        Me.chkRestrictionMyGames = New System.Windows.Forms.CheckBox()
        Me.chkRestrictionMyCollection = New System.Windows.Forms.CheckBox()
        Me.chkRestriction = New System.Windows.Forms.CheckBox()
        Me.chkRestrictionInv = New System.Windows.Forms.CheckBox()
        Me.grpOutput = New System.Windows.Forms.GroupBox()
        Me.optMergeAnd = New System.Windows.Forms.RadioButton()
        Me.optMergeOr = New System.Windows.Forms.RadioButton()
        Me.chkShowExternal = New System.Windows.Forms.CheckBox()
        Me.chkClearPrev = New System.Windows.Forms.CheckBox()
        Me.chkMerge = New System.Windows.Forms.CheckBox()
        Me.picScanCard = New System.Windows.Forms.PictureBox()
        Me.chkSup = New System.Windows.Forms.CheckBox()
        Me.chkEq = New System.Windows.Forms.CheckBox()
        Me.chkInf = New System.Windows.Forms.CheckBox()
        Me.cmdClearSearches = New System.Windows.Forms.Button()
        Me.cboFind = New System.Windows.Forms.ComboBox()
        Me.lblOccur = New System.Windows.Forms.Label()
        Me.cboSearchType = New System.Windows.Forms.ComboBox()
        Me.cmdGo = New System.Windows.Forms.Button()
        Me.lstResult = New System.Windows.Forms.ListBox()
        Me.imgSearch = New System.Windows.Forms.PictureBox()
        Me.cbarSearch.SuspendLayout
        Me.pnlSearch.SuspendLayout
        Me.grpSearch.SuspendLayout
        Me.grpRestriction.SuspendLayout
        Me.grpOutput.SuspendLayout
        CType(Me.picScanCard,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.imgSearch,System.ComponentModel.ISupportInitialize).BeginInit
        Me.SuspendLayout
        '
        'cbarSearch
        '
        Me.cbarSearch.AddRemoveButtonsVisible = false
        Me.cbarSearch.Controls.Add(Me.pnlSearch)
        Me.cbarSearch.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cbarSearch.DrawActionsButton = false
        Me.cbarSearch.Flow = TD.SandBar.ToolBarLayout.Horizontal
        Me.cbarSearch.Guid = New System.Guid("e1afccf7-b082-451d-9dae-2142b22dc603")
        Me.cbarSearch.Location = New System.Drawing.Point(0, 0)
        Me.cbarSearch.Movable = false
        Me.cbarSearch.Name = "cbarSearch"
        Me.cbarSearch.Size = New System.Drawing.Size(618, 423)
        Me.cbarSearch.TabIndex = 7
        Me.cbarSearch.Text = "Recherche avancée"
        AddHandler Me.cbarSearch.VisibleChanged, AddressOf Me.CbarSearchVisibleChanged
        AddHandler Me.cbarSearch.MouseDown, AddressOf Me.CbarSearchMouseDown
        AddHandler Me.cbarSearch.MouseMove, AddressOf Me.CbarSearchMouseMove
        AddHandler Me.cbarSearch.MouseUp, AddressOf Me.CbarSearchMouseUp
        '
        'pnlSearch
        '
        Me.pnlSearch.Controls.Add(Me.grpSearch)
        Me.pnlSearch.Location = New System.Drawing.Point(2, 27)
        Me.pnlSearch.Name = "pnlSearch"
        Me.pnlSearch.Size = New System.Drawing.Size(614, 394)
        Me.pnlSearch.TabIndex = 0
        '
        'grpSearch
        '
        Me.grpSearch.BackColor = System.Drawing.Color.Transparent
        Me.grpSearch.Controls.Add(Me.grpRestriction)
        Me.grpSearch.Controls.Add(Me.grpOutput)
        Me.grpSearch.Controls.Add(Me.picScanCard)
        Me.grpSearch.Controls.Add(Me.chkSup)
        Me.grpSearch.Controls.Add(Me.chkEq)
        Me.grpSearch.Controls.Add(Me.chkInf)
        Me.grpSearch.Controls.Add(Me.cmdClearSearches)
        Me.grpSearch.Controls.Add(Me.cboFind)
        Me.grpSearch.Controls.Add(Me.lblOccur)
        Me.grpSearch.Controls.Add(Me.cboSearchType)
        Me.grpSearch.Controls.Add(Me.cmdGo)
        Me.grpSearch.Controls.Add(Me.lstResult)
        Me.grpSearch.Controls.Add(Me.imgSearch)
        Me.grpSearch.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpSearch.Location = New System.Drawing.Point(0, 0)
        Me.grpSearch.Name = "grpSearch"
        Me.grpSearch.Size = New System.Drawing.Size(614, 394)
        Me.grpSearch.TabIndex = 0
        Me.grpSearch.TabStop = false
        '
        'grpRestriction
        '
        Me.grpRestriction.Controls.Add(Me.chkRestrictionMyGames)
        Me.grpRestriction.Controls.Add(Me.chkRestrictionMyCollection)
        Me.grpRestriction.Controls.Add(Me.chkRestriction)
        Me.grpRestriction.Controls.Add(Me.chkRestrictionInv)
        Me.grpRestriction.Location = New System.Drawing.Point(31, 189)
        Me.grpRestriction.Name = "grpRestriction"
        Me.grpRestriction.Size = New System.Drawing.Size(328, 102)
        Me.grpRestriction.TabIndex = 40
        Me.grpRestriction.TabStop = false
        '
        'chkRestrictionMyGames
        '
        Me.chkRestrictionMyGames.AutoSize = true
        Me.chkRestrictionMyGames.BackColor = System.Drawing.Color.Transparent
        Me.chkRestrictionMyGames.Checked = true
        Me.chkRestrictionMyGames.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkRestrictionMyGames.Enabled = false
        Me.chkRestrictionMyGames.Location = New System.Drawing.Point(23, 82)
        Me.chkRestrictionMyGames.Name = "chkRestrictionMyGames"
        Me.chkRestrictionMyGames.Size = New System.Drawing.Size(99, 17)
        Me.chkRestrictionMyGames.TabIndex = 29
        Me.chkRestrictionMyGames.Text = "parmi les decks"
        Me.chkRestrictionMyGames.UseVisualStyleBackColor = false
        AddHandler Me.chkRestrictionMyGames.CheckedChanged, AddressOf Me.ChkRestrictionMyGamesCheckedChanged
        '
        'chkRestrictionMyCollection
        '
        Me.chkRestrictionMyCollection.AutoSize = true
        Me.chkRestrictionMyCollection.BackColor = System.Drawing.Color.Transparent
        Me.chkRestrictionMyCollection.Checked = true
        Me.chkRestrictionMyCollection.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkRestrictionMyCollection.Enabled = false
        Me.chkRestrictionMyCollection.Location = New System.Drawing.Point(23, 59)
        Me.chkRestrictionMyCollection.Name = "chkRestrictionMyCollection"
        Me.chkRestrictionMyCollection.Size = New System.Drawing.Size(110, 17)
        Me.chkRestrictionMyCollection.TabIndex = 28
        Me.chkRestrictionMyCollection.Text = "parmi la collection"
        Me.chkRestrictionMyCollection.UseVisualStyleBackColor = false
        AddHandler Me.chkRestrictionMyCollection.CheckedChanged, AddressOf Me.ChkRestrictionMyCollectionCheckedChanged
        '
        'chkRestriction
        '
        Me.chkRestriction.AutoSize = true
        Me.chkRestriction.BackColor = System.Drawing.Color.Transparent
        Me.chkRestriction.Location = New System.Drawing.Point(6, 13)
        Me.chkRestriction.Name = "chkRestriction"
        Me.chkRestriction.Size = New System.Drawing.Size(261, 17)
        Me.chkRestriction.TabIndex = 19
        Me.chkRestriction.Text = "Rechercher seulement dans les cartes possédées"
        Me.chkRestriction.UseVisualStyleBackColor = false
        AddHandler Me.chkRestriction.CheckedChanged, AddressOf Me.ChkRestrictionCheckedChanged
        '
        'chkRestrictionInv
        '
        Me.chkRestrictionInv.AutoSize = true
        Me.chkRestrictionInv.BackColor = System.Drawing.Color.Transparent
        Me.chkRestrictionInv.Location = New System.Drawing.Point(6, 36)
        Me.chkRestrictionInv.Name = "chkRestrictionInv"
        Me.chkRestrictionInv.Size = New System.Drawing.Size(282, 17)
        Me.chkRestrictionInv.TabIndex = 27
        Me.chkRestrictionInv.Text = "Rechercher seulement dans les cartes non possédées"
        Me.chkRestrictionInv.UseVisualStyleBackColor = false
        AddHandler Me.chkRestrictionInv.CheckedChanged, AddressOf Me.ChkRestrictionInvCheckedChanged
        '
        'grpOutput
        '
        Me.grpOutput.Controls.Add(Me.optMergeAnd)
        Me.grpOutput.Controls.Add(Me.optMergeOr)
        Me.grpOutput.Controls.Add(Me.chkShowExternal)
        Me.grpOutput.Controls.Add(Me.chkClearPrev)
        Me.grpOutput.Controls.Add(Me.chkMerge)
        Me.grpOutput.Location = New System.Drawing.Point(31, 288)
        Me.grpOutput.Name = "grpOutput"
        Me.grpOutput.Size = New System.Drawing.Size(328, 100)
        Me.grpOutput.TabIndex = 39
        Me.grpOutput.TabStop = false
        '
        'optMergeAnd
        '
        Me.optMergeAnd.AutoSize = true
        Me.optMergeAnd.Enabled = false
        Me.optMergeAnd.Location = New System.Drawing.Point(108, 78)
        Me.optMergeAnd.Name = "optMergeAnd"
        Me.optMergeAnd.Size = New System.Drawing.Size(90, 17)
        Me.optMergeAnd.TabIndex = 24
        Me.optMergeAnd.Text = "intersection(s)"
        Me.optMergeAnd.UseVisualStyleBackColor = true
        '
        'optMergeOr
        '
        Me.optMergeOr.AutoSize = true
        Me.optMergeOr.Checked = true
        Me.optMergeOr.Enabled = false
        Me.optMergeOr.Location = New System.Drawing.Point(40, 78)
        Me.optMergeOr.Name = "optMergeOr"
        Me.optMergeOr.Size = New System.Drawing.Size(62, 17)
        Me.optMergeOr.TabIndex = 23
        Me.optMergeOr.TabStop = true
        Me.optMergeOr.Text = "union(s)"
        Me.optMergeOr.UseVisualStyleBackColor = true
        '
        'chkShowExternal
        '
        Me.chkShowExternal.AutoSize = true
        Me.chkShowExternal.BackColor = System.Drawing.Color.Transparent
        Me.chkShowExternal.Checked = true
        Me.chkShowExternal.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkShowExternal.Location = New System.Drawing.Point(6, 11)
        Me.chkShowExternal.Name = "chkShowExternal"
        Me.chkShowExternal.Size = New System.Drawing.Size(206, 17)
        Me.chkShowExternal.TabIndex = 20
        Me.chkShowExternal.Text = "Charger les résultats dans l'explorateur"
        Me.chkShowExternal.UseVisualStyleBackColor = false
        AddHandler Me.chkShowExternal.CheckedChanged, AddressOf Me.ChkShowExternalCheckedChanged
        '
        'chkClearPrev
        '
        Me.chkClearPrev.AutoSize = true
        Me.chkClearPrev.BackColor = System.Drawing.Color.Transparent
        Me.chkClearPrev.Checked = true
        Me.chkClearPrev.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkClearPrev.Location = New System.Drawing.Point(23, 34)
        Me.chkClearPrev.Name = "chkClearPrev"
        Me.chkClearPrev.Size = New System.Drawing.Size(164, 17)
        Me.chkClearPrev.TabIndex = 21
        Me.chkClearPrev.Text = "Effacer le contenu précédent"
        Me.chkClearPrev.UseVisualStyleBackColor = false
        '
        'chkMerge
        '
        Me.chkMerge.AutoSize = true
        Me.chkMerge.BackColor = System.Drawing.Color.Transparent
        Me.chkMerge.Location = New System.Drawing.Point(23, 57)
        Me.chkMerge.Name = "chkMerge"
        Me.chkMerge.Size = New System.Drawing.Size(262, 17)
        Me.chkMerge.TabIndex = 22
        Me.chkMerge.Text = "Fusionner avec la (les) recherche(s) précédente(s)"
        Me.chkMerge.UseVisualStyleBackColor = false
        AddHandler Me.chkMerge.CheckedChanged, AddressOf Me.ChkMergeCheckedChanged
        '
        'picScanCard
        '
        Me.picScanCard.Location = New System.Drawing.Point(390, 52)
        Me.picScanCard.Name = "picScanCard"
        Me.picScanCard.Size = New System.Drawing.Size(207, 295)
        Me.picScanCard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.picScanCard.TabIndex = 38
        Me.picScanCard.TabStop = false
        Me.picScanCard.Visible = false
        '
        'chkSup
        '
        Me.chkSup.AutoSize = true
        Me.chkSup.Location = New System.Drawing.Point(294, 18)
        Me.chkSup.Name = "chkSup"
        Me.chkSup.Size = New System.Drawing.Size(32, 17)
        Me.chkSup.TabIndex = 30
        Me.chkSup.Text = ">"
        Me.chkSup.UseVisualStyleBackColor = true
        Me.chkSup.Visible = false
        AddHandler Me.chkSup.CheckedChanged, AddressOf Me.ChkSupCheckedChanged
        '
        'chkEq
        '
        Me.chkEq.AutoSize = true
        Me.chkEq.Checked = true
        Me.chkEq.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkEq.Location = New System.Drawing.Point(256, 18)
        Me.chkEq.Name = "chkEq"
        Me.chkEq.Size = New System.Drawing.Size(32, 17)
        Me.chkEq.TabIndex = 29
        Me.chkEq.Text = "="
        Me.chkEq.UseVisualStyleBackColor = true
        Me.chkEq.Visible = false
        AddHandler Me.chkEq.CheckedChanged, AddressOf Me.ChkEqCheckedChanged
        '
        'chkInf
        '
        Me.chkInf.AutoSize = true
        Me.chkInf.Location = New System.Drawing.Point(218, 18)
        Me.chkInf.Name = "chkInf"
        Me.chkInf.Size = New System.Drawing.Size(32, 17)
        Me.chkInf.TabIndex = 28
        Me.chkInf.Text = "<"
        Me.chkInf.UseVisualStyleBackColor = true
        Me.chkInf.Visible = false
        AddHandler Me.chkInf.CheckedChanged, AddressOf Me.ChkInfCheckedChanged
        '
        'cmdClearSearches
        '
        Me.cmdClearSearches.Image = CType(resources.GetObject("cmdClearSearches.Image"),System.Drawing.Image)
        Me.cmdClearSearches.Location = New System.Drawing.Point(338, 16)
        Me.cmdClearSearches.Name = "cmdClearSearches"
        Me.cmdClearSearches.Size = New System.Drawing.Size(21, 21)
        Me.cmdClearSearches.TabIndex = 26
        Me.cmdClearSearches.UseVisualStyleBackColor = true
        AddHandler Me.cmdClearSearches.Click, AddressOf Me.CmdClearSearchesClick
        '
        'cboFind
        '
        Me.cboFind.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cboFind.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboFind.FormattingEnabled = true
        Me.cboFind.Location = New System.Drawing.Point(62, 45)
        Me.cboFind.Name = "cboFind"
        Me.cboFind.Size = New System.Drawing.Size(264, 21)
        Me.cboFind.TabIndex = 15
        AddHandler Me.cboFind.KeyDown, AddressOf Me.CboFindKeyDown
        AddHandler Me.cboFind.KeyUp, AddressOf Me.CboFindKeyUp
        '
        'lblOccur
        '
        Me.lblOccur.Location = New System.Drawing.Point(31, 171)
        Me.lblOccur.Name = "lblOccur"
        Me.lblOccur.Size = New System.Drawing.Size(328, 17)
        Me.lblOccur.TabIndex = 23
        Me.lblOccur.Text = "0 résultat(s) trouvé(s)"
        Me.lblOccur.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cboSearchType
        '
        Me.cboSearchType.DropDownHeight = 200
        Me.cboSearchType.IntegralHeight = false
        Me.cboSearchType.Items.AddRange(New Object() {"Nom de la carte (VO)", "Nom de la carte (VF)", "Texte détaillé (VO)", "Texte détaillé (VF)", "Force", "Endurance", "Prix", "Edition (VO)", "Edition (VF)", "Coût converti de mana", "Type / Sous-type (VO)", "Type / Sous-type (VF)"})
        Me.cboSearchType.Location = New System.Drawing.Point(62, 16)
        Me.cboSearchType.Name = "cboSearchType"
        Me.cboSearchType.Size = New System.Drawing.Size(150, 21)
        Me.cboSearchType.TabIndex = 16
        Me.cboSearchType.Text = "Nom de la carte (VF)"
        AddHandler Me.cboSearchType.SelectedIndexChanged, AddressOf Me.CboSearchTypeSelectedIndexChanged
        '
        'cmdGo
        '
        Me.cmdGo.Image = CType(resources.GetObject("cmdGo.Image"),System.Drawing.Image)
        Me.cmdGo.Location = New System.Drawing.Point(338, 45)
        Me.cmdGo.Name = "cmdGo"
        Me.cmdGo.Size = New System.Drawing.Size(21, 21)
        Me.cmdGo.TabIndex = 17
        Me.cmdGo.UseCompatibleTextRendering = true
        AddHandler Me.cmdGo.Click, AddressOf Me.CmdGoClick
        '
        'lstResult
        '
        Me.lstResult.Location = New System.Drawing.Point(31, 72)
        Me.lstResult.Name = "lstResult"
        Me.lstResult.Size = New System.Drawing.Size(328, 95)
        Me.lstResult.Sorted = true
        Me.lstResult.TabIndex = 18
        AddHandler Me.lstResult.SelectedIndexChanged, AddressOf Me.LstResultSelectedIndexChanged
        AddHandler Me.lstResult.DoubleClick, AddressOf Me.LstResultDoubleClick
        '
        'imgSearch
        '
        Me.imgSearch.BackColor = System.Drawing.Color.Transparent
        Me.imgSearch.Image = CType(resources.GetObject("imgSearch.Image"),System.Drawing.Image)
        Me.imgSearch.Location = New System.Drawing.Point(16, 23)
        Me.imgSearch.Name = "imgSearch"
        Me.imgSearch.Size = New System.Drawing.Size(32, 32)
        Me.imgSearch.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.imgSearch.TabIndex = 14
        Me.imgSearch.TabStop = false
        '
        'frmSearch
        '
        Me.AcceptButton = Me.cmdGo
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(618, 423)
        Me.Controls.Add(Me.cbarSearch)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
        Me.Name = "frmSearch"
        Me.ShowInTaskbar = false
        Me.Text = "Recherche avancée"
        AddHandler FormClosing, AddressOf Me.FrmSearchFormClosing
        AddHandler Load, AddressOf Me.FrmSearchLoad
        Me.cbarSearch.ResumeLayout(false)
        Me.pnlSearch.ResumeLayout(false)
        Me.grpSearch.ResumeLayout(false)
        Me.grpSearch.PerformLayout
        Me.grpRestriction.ResumeLayout(false)
        Me.grpRestriction.PerformLayout
        Me.grpOutput.ResumeLayout(false)
        Me.grpOutput.PerformLayout
        CType(Me.picScanCard,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.imgSearch,System.ComponentModel.ISupportInitialize).EndInit
        Me.ResumeLayout(false)
    End Sub
    Private optMergeOr As System.Windows.Forms.RadioButton
    Private optMergeAnd As System.Windows.Forms.RadioButton
    Private grpOutput As System.Windows.Forms.GroupBox
    Private chkRestrictionMyCollection As System.Windows.Forms.CheckBox
    Private chkRestrictionMyGames As System.Windows.Forms.CheckBox
    Private grpRestriction As System.Windows.Forms.GroupBox
    Private chkInf As System.Windows.Forms.CheckBox
    Private chkEq As System.Windows.Forms.CheckBox
    Private chkSup As System.Windows.Forms.CheckBox
    Private chkRestrictionInv As System.Windows.Forms.CheckBox
    Private cmdClearSearches As System.Windows.Forms.Button
    Private cboFind As System.Windows.Forms.ComboBox
    Private lblOccur As System.Windows.Forms.Label
    Private imgSearch As System.Windows.Forms.PictureBox
    Private chkMerge As System.Windows.Forms.CheckBox
    Private chkClearPrev As System.Windows.Forms.CheckBox
    Private picScanCard As System.Windows.Forms.PictureBox
    Private grpSearch As System.Windows.Forms.GroupBox
    Private pnlSearch As TD.SandBar.ContainerBarClientPanel
    Private chkShowExternal As System.Windows.Forms.CheckBox
    Private chkRestriction As System.Windows.Forms.CheckBox
    Private cbarSearch As TD.SandBar.ContainerBar
    Private cboSearchType As System.Windows.Forms.ComboBox
    Private lstResult As System.Windows.Forms.ListBox
    Private cmdGo As System.Windows.Forms.Button
End Class
