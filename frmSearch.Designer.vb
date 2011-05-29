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
		Me.cbarSearch = New TD.SandBar.ContainerBar
		Me.pnlSearch = New TD.SandBar.ContainerBarClientPanel
		Me.grpSerie = New System.Windows.Forms.GroupBox
		Me.lblStock3 = New System.Windows.Forms.Label
		Me.lblStock2 = New System.Windows.Forms.Label
		Me.picScanCard = New System.Windows.Forms.PictureBox
		Me.lblAD = New System.Windows.Forms.Label
		Me.lblStock = New System.Windows.Forms.Label
		Me.lblPrix = New System.Windows.Forms.Label
		Me.lblRarete = New System.Windows.Forms.Label
		Me.txtCardText = New System.Windows.Forms.TextBox
		Me.lblProp7 = New System.Windows.Forms.Label
		Me.cboEdition = New System.Windows.Forms.ComboBox
		Me.picEdition = New System.Windows.Forms.PictureBox
		Me.lblProp6 = New System.Windows.Forms.Label
		Me.lblProp1 = New System.Windows.Forms.Label
		Me.lblProp2 = New System.Windows.Forms.Label
		Me.lblProp5 = New System.Windows.Forms.Label
		Me.lblProp4 = New System.Windows.Forms.Label
		Me.lblProp3 = New System.Windows.Forms.Label
		Me.grpSearch = New System.Windows.Forms.GroupBox
		Me.cmdClearSearches = New System.Windows.Forms.Button
		Me.cboFind = New System.Windows.Forms.ComboBox
		Me.lblOccur = New System.Windows.Forms.Label
		Me.chkMerge = New System.Windows.Forms.CheckBox
		Me.chkClearPrev = New System.Windows.Forms.CheckBox
		Me.chkShowExternal = New System.Windows.Forms.CheckBox
		Me.chkRestriction = New System.Windows.Forms.CheckBox
		Me.cboSearchType = New System.Windows.Forms.ComboBox
		Me.cmdGo = New System.Windows.Forms.Button
		Me.lstResult = New System.Windows.Forms.ListBox
		Me.imgSearch = New System.Windows.Forms.PictureBox
		Me.btSearch = New TD.SandBar.ButtonItem
		Me.btResult = New TD.SandBar.ButtonItem
		Me.cbarSearch.SuspendLayout
		Me.pnlSearch.SuspendLayout
		Me.grpSerie.SuspendLayout
		CType(Me.picScanCard,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.picEdition,System.ComponentModel.ISupportInitialize).BeginInit
		Me.grpSearch.SuspendLayout
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
		Me.cbarSearch.Items.AddRange(New TD.SandBar.ToolbarItemBase() {Me.btSearch, Me.btResult})
		Me.cbarSearch.Location = New System.Drawing.Point(0, 0)
		Me.cbarSearch.Movable = false
		Me.cbarSearch.Name = "cbarSearch"
		Me.cbarSearch.Size = New System.Drawing.Size(379, 335)
		Me.cbarSearch.TabIndex = 7
		Me.cbarSearch.Text = "Recherche avancée"
		AddHandler Me.cbarSearch.VisibleChanged, AddressOf Me.CbarSearchVisibleChanged
		AddHandler Me.cbarSearch.MouseMove, AddressOf Me.CbarSearchMouseMove
		AddHandler Me.cbarSearch.MouseDown, AddressOf Me.CbarSearchMouseDown
		AddHandler Me.cbarSearch.MouseUp, AddressOf Me.CbarSearchMouseUp
		'
		'pnlSearch
		'
		Me.pnlSearch.Controls.Add(Me.grpSerie)
		Me.pnlSearch.Controls.Add(Me.grpSearch)
		Me.pnlSearch.Location = New System.Drawing.Point(2, 46)
		Me.pnlSearch.Name = "pnlSearch"
		Me.pnlSearch.Size = New System.Drawing.Size(375, 287)
		Me.pnlSearch.TabIndex = 0
		'
		'grpSerie
		'
		Me.grpSerie.BackColor = System.Drawing.Color.Transparent
		Me.grpSerie.Controls.Add(Me.lblStock2)
		Me.grpSerie.Controls.Add(Me.lblStock3)
		Me.grpSerie.Controls.Add(Me.picScanCard)
		Me.grpSerie.Controls.Add(Me.lblAD)
		Me.grpSerie.Controls.Add(Me.lblStock)
		Me.grpSerie.Controls.Add(Me.lblPrix)
		Me.grpSerie.Controls.Add(Me.lblRarete)
		Me.grpSerie.Controls.Add(Me.txtCardText)
		Me.grpSerie.Controls.Add(Me.lblProp7)
		Me.grpSerie.Controls.Add(Me.cboEdition)
		Me.grpSerie.Controls.Add(Me.picEdition)
		Me.grpSerie.Controls.Add(Me.lblProp6)
		Me.grpSerie.Controls.Add(Me.lblProp1)
		Me.grpSerie.Controls.Add(Me.lblProp2)
		Me.grpSerie.Controls.Add(Me.lblProp5)
		Me.grpSerie.Controls.Add(Me.lblProp4)
		Me.grpSerie.Controls.Add(Me.lblProp3)
		Me.grpSerie.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grpSerie.Location = New System.Drawing.Point(0, 0)
		Me.grpSerie.Name = "grpSerie"
		Me.grpSerie.Size = New System.Drawing.Size(375, 287)
		Me.grpSerie.TabIndex = 1
		Me.grpSerie.TabStop = false
		Me.grpSerie.Visible = false
		'
		'lblStock3
		'
		Me.lblStock3.BackColor = System.Drawing.Color.Transparent
		Me.lblStock3.Location = New System.Drawing.Point(163, 83)
		Me.lblStock3.Name = "lblStock3"
		Me.lblStock3.Size = New System.Drawing.Size(19, 20)
		Me.lblStock3.TabIndex = 39
		Me.lblStock3.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblStock2
		'
		Me.lblStock2.AutoSize = true
		Me.lblStock2.BackColor = System.Drawing.Color.Transparent
		Me.lblStock2.Location = New System.Drawing.Point(161, 83)
		Me.lblStock2.Name = "lblStock2"
		Me.lblStock2.Size = New System.Drawing.Size(12, 13)
		Me.lblStock2.TabIndex = 38
		Me.lblStock2.Text = "/"
		Me.lblStock2.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'picScanCard
		'
		Me.picScanCard.Location = New System.Drawing.Point(231, 26)
		Me.picScanCard.Name = "picScanCard"
		Me.picScanCard.Size = New System.Drawing.Size(138, 202)
		Me.picScanCard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me.picScanCard.TabIndex = 37
		Me.picScanCard.TabStop = false
		'
		'lblAD
		'
		Me.lblAD.BackColor = System.Drawing.Color.Transparent
		Me.lblAD.Location = New System.Drawing.Point(135, 103)
		Me.lblAD.Name = "lblAD"
		Me.lblAD.Size = New System.Drawing.Size(45, 13)
		Me.lblAD.TabIndex = 35
		Me.lblAD.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblStock
		'
		Me.lblStock.BackColor = System.Drawing.Color.Transparent
		Me.lblStock.Location = New System.Drawing.Point(114, 83)
		Me.lblStock.Name = "lblStock"
		Me.lblStock.Size = New System.Drawing.Size(45, 13)
		Me.lblStock.TabIndex = 34
		Me.lblStock.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblPrix
		'
		Me.lblPrix.BackColor = System.Drawing.Color.Transparent
		Me.lblPrix.Location = New System.Drawing.Point(135, 63)
		Me.lblPrix.Name = "lblPrix"
		Me.lblPrix.Size = New System.Drawing.Size(45, 13)
		Me.lblPrix.TabIndex = 33
		Me.lblPrix.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblRarete
		'
		Me.lblRarete.BackColor = System.Drawing.Color.Transparent
		Me.lblRarete.Location = New System.Drawing.Point(81, 42)
		Me.lblRarete.Name = "lblRarete"
		Me.lblRarete.Size = New System.Drawing.Size(99, 13)
		Me.lblRarete.TabIndex = 32
		Me.lblRarete.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'txtCardText
		'
		Me.txtCardText.Location = New System.Drawing.Point(31, 159)
		Me.txtCardText.Multiline = true
		Me.txtCardText.Name = "txtCardText"
		Me.txtCardText.Size = New System.Drawing.Size(194, 67)
		Me.txtCardText.TabIndex = 31
		'
		'lblProp7
		'
		Me.lblProp7.AutoSize = true
		Me.lblProp7.BackColor = System.Drawing.Color.Transparent
		Me.lblProp7.Location = New System.Drawing.Point(28, 143)
		Me.lblProp7.Name = "lblProp7"
		Me.lblProp7.Size = New System.Drawing.Size(40, 13)
		Me.lblProp7.TabIndex = 30
		Me.lblProp7.Text = "Texte :"
		'
		'cboEdition
		'
		Me.cboEdition.FormattingEnabled = true
		Me.cboEdition.Location = New System.Drawing.Point(98, 20)
		Me.cboEdition.Name = "cboEdition"
		Me.cboEdition.Size = New System.Drawing.Size(80, 21)
		Me.cboEdition.TabIndex = 29
		AddHandler Me.cboEdition.SelectedValueChanged, AddressOf Me.CboEditionSelectedValueChanged
		'
		'picEdition
		'
		Me.picEdition.Location = New System.Drawing.Point(75, 21)
		Me.picEdition.Name = "picEdition"
		Me.picEdition.Size = New System.Drawing.Size(18, 18)
		Me.picEdition.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me.picEdition.TabIndex = 28
		Me.picEdition.TabStop = false
		'
		'lblProp6
		'
		Me.lblProp6.AutoSize = true
		Me.lblProp6.BackColor = System.Drawing.Color.Transparent
		Me.lblProp6.Location = New System.Drawing.Point(28, 103)
		Me.lblProp6.Name = "lblProp6"
		Me.lblProp6.Size = New System.Drawing.Size(101, 13)
		Me.lblProp6.TabIndex = 27
		Me.lblProp6.Text = "Attaque / Défense :"
		'
		'lblProp1
		'
		Me.lblProp1.AutoSize = true
		Me.lblProp1.BackColor = System.Drawing.Color.Transparent
		Me.lblProp1.Location = New System.Drawing.Point(28, 123)
		Me.lblProp1.Name = "lblProp1"
		Me.lblProp1.Size = New System.Drawing.Size(63, 13)
		Me.lblProp1.TabIndex = 26
		Me.lblProp1.Text = "Invocation :"
		'
		'lblProp2
		'
		Me.lblProp2.AutoSize = true
		Me.lblProp2.BackColor = System.Drawing.Color.Transparent
		Me.lblProp2.Location = New System.Drawing.Point(28, 83)
		Me.lblProp2.Name = "lblProp2"
		Me.lblProp2.Size = New System.Drawing.Size(41, 13)
		Me.lblProp2.TabIndex = 25
		Me.lblProp2.Text = "Stock :"
		'
		'lblProp5
		'
		Me.lblProp5.AutoSize = true
		Me.lblProp5.BackColor = System.Drawing.Color.Transparent
		Me.lblProp5.Location = New System.Drawing.Point(28, 63)
		Me.lblProp5.Name = "lblProp5"
		Me.lblProp5.Size = New System.Drawing.Size(30, 13)
		Me.lblProp5.TabIndex = 24
		Me.lblProp5.Text = "Prix :"
		'
		'lblProp4
		'
		Me.lblProp4.AutoSize = true
		Me.lblProp4.BackColor = System.Drawing.Color.Transparent
		Me.lblProp4.Location = New System.Drawing.Point(28, 43)
		Me.lblProp4.Name = "lblProp4"
		Me.lblProp4.Size = New System.Drawing.Size(45, 13)
		Me.lblProp4.TabIndex = 23
		Me.lblProp4.Text = "Rareté :"
		'
		'lblProp3
		'
		Me.lblProp3.AutoSize = true
		Me.lblProp3.BackColor = System.Drawing.Color.Transparent
		Me.lblProp3.Location = New System.Drawing.Point(28, 23)
		Me.lblProp3.Name = "lblProp3"
		Me.lblProp3.Size = New System.Drawing.Size(45, 13)
		Me.lblProp3.TabIndex = 22
		Me.lblProp3.Text = "Edition :"
		'
		'grpSearch
		'
		Me.grpSearch.BackColor = System.Drawing.Color.Transparent
		Me.grpSearch.Controls.Add(Me.cmdClearSearches)
		Me.grpSearch.Controls.Add(Me.cboFind)
		Me.grpSearch.Controls.Add(Me.lblOccur)
		Me.grpSearch.Controls.Add(Me.chkMerge)
		Me.grpSearch.Controls.Add(Me.chkClearPrev)
		Me.grpSearch.Controls.Add(Me.chkShowExternal)
		Me.grpSearch.Controls.Add(Me.chkRestriction)
		Me.grpSearch.Controls.Add(Me.cboSearchType)
		Me.grpSearch.Controls.Add(Me.cmdGo)
		Me.grpSearch.Controls.Add(Me.lstResult)
		Me.grpSearch.Controls.Add(Me.imgSearch)
		Me.grpSearch.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grpSearch.Location = New System.Drawing.Point(0, 0)
		Me.grpSearch.Name = "grpSearch"
		Me.grpSearch.Size = New System.Drawing.Size(375, 287)
		Me.grpSearch.TabIndex = 0
		Me.grpSearch.TabStop = false
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
		Me.cboFind.Location = New System.Drawing.Point(79, 16)
		Me.cboFind.Name = "cboFind"
		Me.cboFind.Size = New System.Drawing.Size(232, 21)
		Me.cboFind.TabIndex = 25
		AddHandler Me.cboFind.KeyUp, AddressOf Me.CboFindKeyUp
		AddHandler Me.cboFind.KeyDown, AddressOf Me.CboFindKeyDown
		'
		'lblOccur
		'
		Me.lblOccur.Location = New System.Drawing.Point(31, 171)
		Me.lblOccur.Name = "lblOccur"
		Me.lblOccur.Size = New System.Drawing.Size(328, 17)
		Me.lblOccur.TabIndex = 23
		Me.lblOccur.Text = "0 occurence(s) trouvée(s)"
		Me.lblOccur.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'chkMerge
		'
		Me.chkMerge.AutoSize = true
		Me.chkMerge.BackColor = System.Drawing.Color.Transparent
		Me.chkMerge.Enabled = false
		Me.chkMerge.Location = New System.Drawing.Point(48, 261)
		Me.chkMerge.Name = "chkMerge"
		Me.chkMerge.Size = New System.Drawing.Size(291, 17)
		Me.chkMerge.TabIndex = 22
		Me.chkMerge.Text = "Fusionner avec les résultats de la recherche précédente"
		Me.chkMerge.UseVisualStyleBackColor = false
		'
		'chkClearPrev
		'
		Me.chkClearPrev.AutoSize = true
		Me.chkClearPrev.BackColor = System.Drawing.Color.Transparent
		Me.chkClearPrev.Checked = true
		Me.chkClearPrev.CheckState = System.Windows.Forms.CheckState.Checked
		Me.chkClearPrev.Enabled = false
		Me.chkClearPrev.Location = New System.Drawing.Point(48, 238)
		Me.chkClearPrev.Name = "chkClearPrev"
		Me.chkClearPrev.Size = New System.Drawing.Size(189, 17)
		Me.chkClearPrev.TabIndex = 21
		Me.chkClearPrev.Text = "Effacer l'arborescence précédente"
		Me.chkClearPrev.UseVisualStyleBackColor = false
		'
		'chkShowExternal
		'
		Me.chkShowExternal.AutoSize = true
		Me.chkShowExternal.BackColor = System.Drawing.Color.Transparent
		Me.chkShowExternal.Location = New System.Drawing.Point(31, 215)
		Me.chkShowExternal.Name = "chkShowExternal"
		Me.chkShowExternal.Size = New System.Drawing.Size(267, 17)
		Me.chkShowExternal.TabIndex = 20
		Me.chkShowExternal.Text = "Charger les résultats dans l'arborescence principale"
		Me.chkShowExternal.UseVisualStyleBackColor = false
		AddHandler Me.chkShowExternal.CheckedChanged, AddressOf Me.ChkShowExternalCheckedChanged
		'
		'chkRestriction
		'
		Me.chkRestriction.AutoSize = true
		Me.chkRestriction.BackColor = System.Drawing.Color.Transparent
		Me.chkRestriction.Location = New System.Drawing.Point(31, 192)
		Me.chkRestriction.Name = "chkRestriction"
		Me.chkRestriction.Size = New System.Drawing.Size(278, 17)
		Me.chkRestriction.TabIndex = 19
		Me.chkRestriction.Text = "Rechercher seulement dans les cartes de la sélection"
		Me.chkRestriction.UseVisualStyleBackColor = false
		AddHandler Me.chkRestriction.CheckedChanged, AddressOf Me.ChkRestrictionCheckedChanged
		'
		'cboSearchType
		'
		Me.cboSearchType.Items.AddRange(New Object() {"Nom de la carte (VO)", "Nom de la carte (VF)", "Texte détaillé (VO)", "Texte détaillé (VF)", "Force", "Endurance", "Prix", "Edition", "Coût converti de mana", "Type / Sous-type"})
		Me.cboSearchType.Location = New System.Drawing.Point(79, 45)
		Me.cboSearchType.Name = "cboSearchType"
		Me.cboSearchType.Size = New System.Drawing.Size(232, 21)
		Me.cboSearchType.TabIndex = 16
		Me.cboSearchType.Text = "Nom de la carte (VF)"
		'
		'cmdGo
		'
		Me.cmdGo.Location = New System.Drawing.Point(338, 45)
		Me.cmdGo.Name = "cmdGo"
		Me.cmdGo.Size = New System.Drawing.Size(21, 21)
		Me.cmdGo.TabIndex = 17
		Me.cmdGo.Text = "."
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
		AddHandler Me.lstResult.DoubleClick, AddressOf Me.LstResultDoubleClick
		'
		'imgSearch
		'
		Me.imgSearch.BackColor = System.Drawing.Color.Transparent
		Me.imgSearch.Image = CType(resources.GetObject("imgSearch.Image"),System.Drawing.Image)
		Me.imgSearch.Location = New System.Drawing.Point(15, 16)
		Me.imgSearch.Name = "imgSearch"
		Me.imgSearch.Size = New System.Drawing.Size(32, 32)
		Me.imgSearch.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me.imgSearch.TabIndex = 14
		Me.imgSearch.TabStop = false
		'
		'btSearch
		'
		Me.btSearch.Text = "Recherche"
		AddHandler Me.btSearch.Activate, AddressOf Me.BtSearchActivate
		'
		'btResult
		'
		Me.btResult.Enabled = false
		Me.btResult.Text = "Résultat"
		AddHandler Me.btResult.Activate, AddressOf Me.BtResultActivate
		'
		'frmSearch
		'
		Me.AcceptButton = Me.cmdGo
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(379, 335)
		Me.Controls.Add(Me.cbarSearch)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
		Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
		Me.Name = "frmSearch"
		Me.ShowInTaskbar = false
		Me.Text = "Recherche avancée"
		AddHandler Load, AddressOf Me.FrmSearchLoad
		AddHandler FormClosing, AddressOf Me.FrmSearchFormClosing
		Me.cbarSearch.ResumeLayout(false)
		Me.pnlSearch.ResumeLayout(false)
		Me.grpSerie.ResumeLayout(false)
		Me.grpSerie.PerformLayout
		CType(Me.picScanCard,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.picEdition,System.ComponentModel.ISupportInitialize).EndInit
		Me.grpSearch.ResumeLayout(false)
		Me.grpSearch.PerformLayout
		CType(Me.imgSearch,System.ComponentModel.ISupportInitialize).EndInit
		Me.ResumeLayout(false)
	End Sub
	Public lblStock2 As System.Windows.Forms.Label
	Public lblStock3 As System.Windows.Forms.Label
	Private cmdClearSearches As System.Windows.Forms.Button
	Private cboFind As System.Windows.Forms.ComboBox
	Private lblOccur As System.Windows.Forms.Label
	Private imgSearch As System.Windows.Forms.PictureBox
	Private chkMerge As System.Windows.Forms.CheckBox
	Private chkClearPrev As System.Windows.Forms.CheckBox
	Private picScanCard As System.Windows.Forms.PictureBox
	Public grpSerie As System.Windows.Forms.GroupBox
	Private lblProp3 As System.Windows.Forms.Label
	Private lblProp4 As System.Windows.Forms.Label
	Public lblProp5 As System.Windows.Forms.Label
	Private lblProp2 As System.Windows.Forms.Label
	Public lblProp1 As System.Windows.Forms.Label
	Public lblProp6 As System.Windows.Forms.Label
	Public picEdition As System.Windows.Forms.PictureBox
	Public cboEdition As System.Windows.Forms.ComboBox
	Private lblProp7 As System.Windows.Forms.Label
	Public txtCardText As System.Windows.Forms.TextBox
	Public lblRarete As System.Windows.Forms.Label
	Public lblPrix As System.Windows.Forms.Label
	Public lblStock As System.Windows.Forms.Label
	Public lblAD As System.Windows.Forms.Label
	Private grpSearch As System.Windows.Forms.GroupBox
	Private pnlSearch As TD.SandBar.ContainerBarClientPanel
	Private btResult As TD.SandBar.ButtonItem
	Private btSearch As TD.SandBar.ButtonItem
	Private chkShowExternal As System.Windows.Forms.CheckBox
	Private chkRestriction As System.Windows.Forms.CheckBox
	Private cbarSearch As TD.SandBar.ContainerBar
	Private cboSearchType As System.Windows.Forms.ComboBox
	Private lstResult As System.Windows.Forms.ListBox
	Private cmdGo As System.Windows.Forms.Button
End Class
