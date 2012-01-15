'
' Created by SharpDevelop.
' User: Couitchy
' Date: 22/03/2008
' Time: 14:41
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Partial Class frmAddCards
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
		Me.components = New System.ComponentModel.Container
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAddCards))
		Me.cbarAjout = New TD.SandBar.ContainerBar
		Me.pnlAjout = New TD.SandBar.ContainerBarClientPanel
		Me.grpQuality = New System.Windows.Forms.GroupBox
		Me.chkFoil = New System.Windows.Forms.CheckBox
		Me.cmdClose = New System.Windows.Forms.Button
		Me.cmdDestination = New System.Windows.Forms.Button
		Me.cmdAdd = New System.Windows.Forms.Button
		Me.grpQuantite = New System.Windows.Forms.GroupBox
		Me.imgAdd = New System.Windows.Forms.PictureBox
		Me.lblNbItems = New System.Windows.Forms.Label
		Me.txtNbItems = New System.Windows.Forms.TextBox
		Me.grpEdition = New System.Windows.Forms.GroupBox
		Me.imgEdition = New System.Windows.Forms.PictureBox
		Me.lblYear = New System.Windows.Forms.Label
		Me.lblEncNbr = New System.Windows.Forms.Label
		Me.cboSerie = New System.Windows.Forms.ComboBox
		Me.grpId = New System.Windows.Forms.GroupBox
		Me.pictureBox2 = New System.Windows.Forms.PictureBox
		Me.imgVF = New System.Windows.Forms.PictureBox
		Me.cboTitleEN = New System.Windows.Forms.ComboBox
		Me.cboTitleFR = New System.Windows.Forms.ComboBox
		Me.cmnuDestination = New System.Windows.Forms.ContextMenuStrip(Me.components)
		Me.mnuDropToCollection = New System.Windows.Forms.ToolStripMenuItem
		Me.cbarAjout.SuspendLayout
		Me.pnlAjout.SuspendLayout
		Me.grpQuality.SuspendLayout
		Me.grpQuantite.SuspendLayout
		CType(Me.imgAdd,System.ComponentModel.ISupportInitialize).BeginInit
		Me.grpEdition.SuspendLayout
		CType(Me.imgEdition,System.ComponentModel.ISupportInitialize).BeginInit
		Me.grpId.SuspendLayout
		CType(Me.pictureBox2,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.imgVF,System.ComponentModel.ISupportInitialize).BeginInit
		Me.cmnuDestination.SuspendLayout
		Me.SuspendLayout
		'
		'cbarAjout
		'
		Me.cbarAjout.Controls.Add(Me.pnlAjout)
		Me.cbarAjout.Dock = System.Windows.Forms.DockStyle.Fill
		Me.cbarAjout.DrawActionsButton = false
		Me.cbarAjout.Flow = TD.SandBar.ToolBarLayout.Horizontal
		Me.cbarAjout.Guid = New System.Guid("e9c7c112-d3ac-45da-8c08-d20335fc4e9e")
		Me.cbarAjout.Location = New System.Drawing.Point(0, 0)
		Me.cbarAjout.Movable = false
		Me.cbarAjout.Name = "cbarAjout"
		Me.cbarAjout.Size = New System.Drawing.Size(365, 345)
		Me.cbarAjout.TabIndex = 1
		Me.cbarAjout.Text = "Ajout de cartes"
		AddHandler Me.cbarAjout.VisibleChanged, AddressOf Me.CBarAjoutVisibleChanged
		AddHandler Me.cbarAjout.MouseMove, AddressOf Me.CBarAjoutMouseMove
		AddHandler Me.cbarAjout.MouseDown, AddressOf Me.CBarAjoutMouseDown
		AddHandler Me.cbarAjout.MouseUp, AddressOf Me.CBarAjoutMouseUp
		'
		'pnlAjout
		'
		Me.pnlAjout.Controls.Add(Me.grpQuality)
		Me.pnlAjout.Controls.Add(Me.cmdClose)
		Me.pnlAjout.Controls.Add(Me.cmdDestination)
		Me.pnlAjout.Controls.Add(Me.cmdAdd)
		Me.pnlAjout.Controls.Add(Me.grpQuantite)
		Me.pnlAjout.Controls.Add(Me.grpEdition)
		Me.pnlAjout.Controls.Add(Me.grpId)
		Me.pnlAjout.Location = New System.Drawing.Point(2, 27)
		Me.pnlAjout.Name = "pnlAjout"
		Me.pnlAjout.Size = New System.Drawing.Size(361, 316)
		Me.pnlAjout.TabIndex = 0
		'
		'grpQuality
		'
		Me.grpQuality.BackColor = System.Drawing.Color.Transparent
		Me.grpQuality.Controls.Add(Me.chkFoil)
		Me.grpQuality.Location = New System.Drawing.Point(181, 207)
		Me.grpQuality.Name = "grpQuality"
		Me.grpQuality.Size = New System.Drawing.Size(162, 69)
		Me.grpQuality.TabIndex = 103
		Me.grpQuality.TabStop = false
		Me.grpQuality.Text = "Qualité"
		'
		'chkFoil
		'
		Me.chkFoil.AutoSize = true
		Me.chkFoil.Location = New System.Drawing.Point(26, 30)
		Me.chkFoil.Name = "chkFoil"
		Me.chkFoil.Size = New System.Drawing.Size(91, 17)
		Me.chkFoil.TabIndex = 5
		Me.chkFoil.Text = "Premium (Foil)"
		Me.chkFoil.UseVisualStyleBackColor = true
		AddHandler Me.chkFoil.CheckedChanged, AddressOf Me.ChkFoilCheckedChanged
		'
		'cmdClose
		'
		Me.cmdClose.Location = New System.Drawing.Point(187, 282)
		Me.cmdClose.Name = "cmdClose"
		Me.cmdClose.Size = New System.Drawing.Size(75, 23)
		Me.cmdClose.TabIndex = 8
		Me.cmdClose.Text = "Fermer"
		Me.cmdClose.UseVisualStyleBackColor = true
		AddHandler Me.cmdClose.Click, AddressOf Me.CmdCloseClick
		'
		'cmdDestination
		'
		Me.cmdDestination.Location = New System.Drawing.Point(10, 282)
		Me.cmdDestination.Name = "cmdDestination"
		Me.cmdDestination.Size = New System.Drawing.Size(75, 23)
		Me.cmdDestination.TabIndex = 6
		Me.cmdDestination.Text = "Ajouter à >"
		Me.cmdDestination.UseVisualStyleBackColor = true
		AddHandler Me.cmdDestination.MouseDown, AddressOf Me.CmdDestinationMouseDown
		'
		'cmdAdd
		'
		Me.cmdAdd.Location = New System.Drawing.Point(268, 282)
		Me.cmdAdd.Name = "cmdAdd"
		Me.cmdAdd.Size = New System.Drawing.Size(75, 23)
		Me.cmdAdd.TabIndex = 7
		Me.cmdAdd.Text = "Valider"
		Me.cmdAdd.UseVisualStyleBackColor = true
		AddHandler Me.cmdAdd.Click, AddressOf Me.CmdAddClick
		'
		'grpQuantite
		'
		Me.grpQuantite.BackColor = System.Drawing.Color.Transparent
		Me.grpQuantite.Controls.Add(Me.imgAdd)
		Me.grpQuantite.Controls.Add(Me.lblNbItems)
		Me.grpQuantite.Controls.Add(Me.txtNbItems)
		Me.grpQuantite.Location = New System.Drawing.Point(10, 207)
		Me.grpQuantite.Name = "grpQuantite"
		Me.grpQuantite.Size = New System.Drawing.Size(165, 69)
		Me.grpQuantite.TabIndex = 102
		Me.grpQuantite.TabStop = false
		Me.grpQuantite.Text = "Quantité"
		'
		'imgAdd
		'
		Me.imgAdd.BackColor = System.Drawing.Color.Transparent
		Me.imgAdd.Image = CType(resources.GetObject("imgAdd.Image"),System.Drawing.Image)
		Me.imgAdd.Location = New System.Drawing.Point(16, 23)
		Me.imgAdd.Name = "imgAdd"
		Me.imgAdd.Size = New System.Drawing.Size(16, 16)
		Me.imgAdd.TabIndex = 15
		Me.imgAdd.TabStop = false
		'
		'lblNbItems
		'
		Me.lblNbItems.AutoSize = true
		Me.lblNbItems.Location = New System.Drawing.Point(38, 46)
		Me.lblNbItems.Name = "lblNbItems"
		Me.lblNbItems.Size = New System.Drawing.Size(111, 13)
		Me.lblNbItems.TabIndex = 106
		Me.lblNbItems.Text = "Nombre déjà en stock"
		'
		'txtNbItems
		'
		Me.txtNbItems.Location = New System.Drawing.Point(38, 23)
		Me.txtNbItems.Name = "txtNbItems"
		Me.txtNbItems.Size = New System.Drawing.Size(61, 20)
		Me.txtNbItems.TabIndex = 3
		Me.txtNbItems.Text = "+1"
		Me.txtNbItems.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
		'
		'grpEdition
		'
		Me.grpEdition.BackColor = System.Drawing.Color.Transparent
		Me.grpEdition.Controls.Add(Me.imgEdition)
		Me.grpEdition.Controls.Add(Me.lblYear)
		Me.grpEdition.Controls.Add(Me.lblEncNbr)
		Me.grpEdition.Controls.Add(Me.cboSerie)
		Me.grpEdition.Location = New System.Drawing.Point(10, 118)
		Me.grpEdition.Name = "grpEdition"
		Me.grpEdition.Size = New System.Drawing.Size(333, 83)
		Me.grpEdition.TabIndex = 101
		Me.grpEdition.TabStop = false
		Me.grpEdition.Text = "Edition"
		'
		'imgEdition
		'
		Me.imgEdition.Location = New System.Drawing.Point(16, 32)
		Me.imgEdition.Name = "imgEdition"
		Me.imgEdition.Size = New System.Drawing.Size(16, 16)
		Me.imgEdition.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me.imgEdition.TabIndex = 3
		Me.imgEdition.TabStop = false
		'
		'lblYear
		'
		Me.lblYear.AutoSize = true
		Me.lblYear.Location = New System.Drawing.Point(269, 59)
		Me.lblYear.Name = "lblYear"
		Me.lblYear.Size = New System.Drawing.Size(38, 13)
		Me.lblYear.TabIndex = 2
		Me.lblYear.Text = "Année"
		Me.lblYear.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblEncNbr
		'
		Me.lblEncNbr.AutoSize = true
		Me.lblEncNbr.Location = New System.Drawing.Point(38, 59)
		Me.lblEncNbr.Name = "lblEncNbr"
		Me.lblEncNbr.Size = New System.Drawing.Size(85, 13)
		Me.lblEncNbr.TabIndex = 104
		Me.lblEncNbr.Text = "ID Encyclopédie"
		'
		'cboSerie
		'
		Me.cboSerie.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
		Me.cboSerie.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
		Me.cboSerie.FormattingEnabled = true
		Me.cboSerie.Location = New System.Drawing.Point(38, 29)
		Me.cboSerie.Name = "cboSerie"
		Me.cboSerie.Size = New System.Drawing.Size(277, 21)
		Me.cboSerie.TabIndex = 2
		AddHandler Me.cboSerie.SelectedIndexChanged, AddressOf Me.CboSerieSelectedIndexChanged
		'
		'grpId
		'
		Me.grpId.BackColor = System.Drawing.Color.Transparent
		Me.grpId.Controls.Add(Me.pictureBox2)
		Me.grpId.Controls.Add(Me.imgVF)
		Me.grpId.Controls.Add(Me.cboTitleEN)
		Me.grpId.Controls.Add(Me.cboTitleFR)
		Me.grpId.Location = New System.Drawing.Point(10, 12)
		Me.grpId.Name = "grpId"
		Me.grpId.Size = New System.Drawing.Size(333, 100)
		Me.grpId.TabIndex = 100
		Me.grpId.TabStop = false
		Me.grpId.Text = "Identification"
		'
		'pictureBox2
		'
		Me.pictureBox2.Image = CType(resources.GetObject("pictureBox2.Image"),System.Drawing.Image)
		Me.pictureBox2.Location = New System.Drawing.Point(16, 58)
		Me.pictureBox2.Name = "pictureBox2"
		Me.pictureBox2.Size = New System.Drawing.Size(16, 16)
		Me.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me.pictureBox2.TabIndex = 5
		Me.pictureBox2.TabStop = false
		'
		'imgVF
		'
		Me.imgVF.Image = CType(resources.GetObject("imgVF.Image"),System.Drawing.Image)
		Me.imgVF.Location = New System.Drawing.Point(16, 31)
		Me.imgVF.Name = "imgVF"
		Me.imgVF.Size = New System.Drawing.Size(16, 16)
		Me.imgVF.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me.imgVF.TabIndex = 4
		Me.imgVF.TabStop = false
		'
		'cboTitleEN
		'
		Me.cboTitleEN.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
		Me.cboTitleEN.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
		Me.cboTitleEN.FormattingEnabled = true
		Me.cboTitleEN.Location = New System.Drawing.Point(38, 56)
		Me.cboTitleEN.Name = "cboTitleEN"
		Me.cboTitleEN.Size = New System.Drawing.Size(277, 21)
		Me.cboTitleEN.TabIndex = 1
		AddHandler Me.cboTitleEN.SelectedIndexChanged, AddressOf Me.CboTitleENSelectedIndexChanged
		AddHandler Me.cboTitleEN.Enter, AddressOf Me.CboTitleENEnter
		AddHandler Me.cboTitleEN.KeyUp, AddressOf Me.CboTitleKeyUp
		AddHandler Me.cboTitleEN.KeyDown, AddressOf Me.CboTitleKeyDown
		'
		'cboTitleFR
		'
		Me.cboTitleFR.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
		Me.cboTitleFR.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
		Me.cboTitleFR.FormattingEnabled = true
		Me.cboTitleFR.Location = New System.Drawing.Point(38, 29)
		Me.cboTitleFR.Name = "cboTitleFR"
		Me.cboTitleFR.Size = New System.Drawing.Size(277, 21)
		Me.cboTitleFR.TabIndex = 0
		AddHandler Me.cboTitleFR.SelectedIndexChanged, AddressOf Me.CboTitleFRSelectedIndexChanged
		AddHandler Me.cboTitleFR.Enter, AddressOf Me.CboTitleFREnter
		AddHandler Me.cboTitleFR.KeyUp, AddressOf Me.CboTitleKeyUp
		AddHandler Me.cboTitleFR.KeyDown, AddressOf Me.CboTitleKeyDown
		'
		'cmnuDestination
		'
		Me.cmnuDestination.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuDropToCollection})
		Me.cmnuDestination.Name = "cmnuDestination"
		Me.cmnuDestination.Size = New System.Drawing.Size(129, 26)
		'
		'mnuDropToCollection
		'
		Me.mnuDropToCollection.Name = "mnuDropToCollection"
		Me.mnuDropToCollection.Size = New System.Drawing.Size(128, 22)
		Me.mnuDropToCollection.Text = "Collection"
		AddHandler Me.mnuDropToCollection.Click, AddressOf Me.DropTo
		'
		'frmAddCards
		'
		Me.AcceptButton = Me.cmdAdd
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(365, 345)
		Me.Controls.Add(Me.cbarAjout)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
		Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
		Me.Name = "frmAddCards"
		Me.ShowInTaskbar = false
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Ajouter des cartes"
		AddHandler Load, AddressOf Me.FrmAddCardsLoad
		AddHandler Activated, AddressOf Me.FrmAddCardsActivated
		AddHandler FormClosing, AddressOf Me.FrmAddCardsFormClosing
		Me.cbarAjout.ResumeLayout(false)
		Me.pnlAjout.ResumeLayout(false)
		Me.grpQuality.ResumeLayout(false)
		Me.grpQuality.PerformLayout
		Me.grpQuantite.ResumeLayout(false)
		Me.grpQuantite.PerformLayout
		CType(Me.imgAdd,System.ComponentModel.ISupportInitialize).EndInit
		Me.grpEdition.ResumeLayout(false)
		Me.grpEdition.PerformLayout
		CType(Me.imgEdition,System.ComponentModel.ISupportInitialize).EndInit
		Me.grpId.ResumeLayout(false)
		CType(Me.pictureBox2,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.imgVF,System.ComponentModel.ISupportInitialize).EndInit
		Me.cmnuDestination.ResumeLayout(false)
		Me.ResumeLayout(false)
	End Sub
	Private chkFoil As System.Windows.Forms.CheckBox
	Private grpQuality As System.Windows.Forms.GroupBox
	Private cbarAjout As TD.SandBar.ContainerBar
	Private cmdClose As System.Windows.Forms.Button
	Private imgAdd As System.Windows.Forms.PictureBox
	Private imgVF As System.Windows.Forms.PictureBox
	Private pictureBox2 As System.Windows.Forms.PictureBox
	Private mnuDropToCollection As System.Windows.Forms.ToolStripMenuItem
	Private cmnuDestination As System.Windows.Forms.ContextMenuStrip
	Private cmdDestination As System.Windows.Forms.Button
	Private imgEdition As System.Windows.Forms.PictureBox
	Private lblYear As System.Windows.Forms.Label
	Private pnlAjout As TD.SandBar.ContainerBarClientPanel
	Private lblNbItems As System.Windows.Forms.Label
	Private cmdAdd As System.Windows.Forms.Button
	Private txtNbItems As System.Windows.Forms.TextBox
	Private lblEncNbr As System.Windows.Forms.Label
	Private cboSerie As System.Windows.Forms.ComboBox
	Private grpQuantite As System.Windows.Forms.GroupBox
	Private cboTitleFR As System.Windows.Forms.ComboBox
	Private cboTitleEN As System.Windows.Forms.ComboBox
	Private grpId As System.Windows.Forms.GroupBox
	Private grpEdition As System.Windows.Forms.GroupBox
End Class
