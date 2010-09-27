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
		Me.CBarAjout = New TD.SandBar.ContainerBar
		Me.pnlAjout = New TD.SandBar.ContainerBarClientPanel
		Me.imgAdd = New System.Windows.Forms.PictureBox
		Me.cmdDestination = New System.Windows.Forms.Button
		Me.cmdAdd = New System.Windows.Forms.Button
		Me.grpQuantite = New System.Windows.Forms.GroupBox
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
		Me.CBarAjout.SuspendLayout
		Me.pnlAjout.SuspendLayout
		CType(Me.imgAdd,System.ComponentModel.ISupportInitialize).BeginInit
		Me.grpQuantite.SuspendLayout
		Me.grpEdition.SuspendLayout
		CType(Me.imgEdition,System.ComponentModel.ISupportInitialize).BeginInit
		Me.grpId.SuspendLayout
		CType(Me.pictureBox2,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.imgVF,System.ComponentModel.ISupportInitialize).BeginInit
		Me.cmnuDestination.SuspendLayout
		Me.SuspendLayout
		'
		'CBarAjout
		'
		Me.CBarAjout.Controls.Add(Me.pnlAjout)
		Me.CBarAjout.Dock = System.Windows.Forms.DockStyle.Fill
		Me.CBarAjout.DrawActionsButton = false
		Me.CBarAjout.Flow = TD.SandBar.ToolBarLayout.Horizontal
		Me.CBarAjout.Guid = New System.Guid("e9c7c112-d3ac-45da-8c08-d20335fc4e9e")
		Me.CBarAjout.Location = New System.Drawing.Point(0, 0)
		Me.CBarAjout.Movable = false
		Me.CBarAjout.Name = "CBarAjout"
		Me.CBarAjout.Size = New System.Drawing.Size(365, 328)
		Me.CBarAjout.TabIndex = 1
		Me.CBarAjout.Text = "Ajout de cartes"
		AddHandler Me.CBarAjout.VisibleChanged, AddressOf Me.CBarAjoutVisibleChanged
		AddHandler Me.CBarAjout.MouseMove, AddressOf Me.CBarAjoutMouseMove
		AddHandler Me.CBarAjout.MouseDown, AddressOf Me.CBarAjoutMouseDown
		AddHandler Me.CBarAjout.MouseUp, AddressOf Me.CBarAjoutMouseUp
		'
		'pnlAjout
		'
		Me.pnlAjout.Controls.Add(Me.imgAdd)
		Me.pnlAjout.Controls.Add(Me.cmdDestination)
		Me.pnlAjout.Controls.Add(Me.cmdAdd)
		Me.pnlAjout.Controls.Add(Me.grpQuantite)
		Me.pnlAjout.Controls.Add(Me.grpEdition)
		Me.pnlAjout.Controls.Add(Me.grpId)
		Me.pnlAjout.Location = New System.Drawing.Point(2, 27)
		Me.pnlAjout.Name = "pnlAjout"
		Me.pnlAjout.Size = New System.Drawing.Size(361, 299)
		Me.pnlAjout.TabIndex = 0
		'
		'imgAdd
		'
		Me.imgAdd.BackColor = System.Drawing.Color.Transparent
		Me.imgAdd.Image = CType(resources.GetObject("imgAdd.Image"),System.Drawing.Image)
		Me.imgAdd.Location = New System.Drawing.Point(209, 226)
		Me.imgAdd.Name = "imgAdd"
		Me.imgAdd.Size = New System.Drawing.Size(32, 32)
		Me.imgAdd.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me.imgAdd.TabIndex = 15
		Me.imgAdd.TabStop = false
		'
		'cmdDestination
		'
		Me.cmdDestination.Location = New System.Drawing.Point(187, 264)
		Me.cmdDestination.Name = "cmdDestination"
		Me.cmdDestination.Size = New System.Drawing.Size(75, 23)
		Me.cmdDestination.TabIndex = 8
		Me.cmdDestination.Text = "Ajouter à >"
		Me.cmdDestination.UseVisualStyleBackColor = true
		AddHandler Me.cmdDestination.MouseDown, AddressOf Me.CmdDestinationMouseDown
		'
		'cmdAdd
		'
		Me.cmdAdd.Location = New System.Drawing.Point(268, 263)
		Me.cmdAdd.Name = "cmdAdd"
		Me.cmdAdd.Size = New System.Drawing.Size(75, 23)
		Me.cmdAdd.TabIndex = 4
		Me.cmdAdd.Text = "Valider"
		Me.cmdAdd.UseVisualStyleBackColor = true
		AddHandler Me.cmdAdd.Click, AddressOf Me.CmdAddClick
		'
		'grpQuantite
		'
		Me.grpQuantite.BackColor = System.Drawing.Color.Transparent
		Me.grpQuantite.Controls.Add(Me.lblNbItems)
		Me.grpQuantite.Controls.Add(Me.txtNbItems)
		Me.grpQuantite.Location = New System.Drawing.Point(10, 217)
		Me.grpQuantite.Name = "grpQuantite"
		Me.grpQuantite.Size = New System.Drawing.Size(165, 69)
		Me.grpQuantite.TabIndex = 7
		Me.grpQuantite.TabStop = false
		Me.grpQuantite.Text = "Quantité"
		'
		'lblNbItems
		'
		Me.lblNbItems.AutoSize = true
		Me.lblNbItems.Location = New System.Drawing.Point(28, 43)
		Me.lblNbItems.Name = "lblNbItems"
		Me.lblNbItems.Size = New System.Drawing.Size(111, 13)
		Me.lblNbItems.TabIndex = 1
		Me.lblNbItems.Text = "Nombre déjà en stock"
		'
		'txtNbItems
		'
		Me.txtNbItems.Location = New System.Drawing.Point(28, 19)
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
		Me.grpEdition.Size = New System.Drawing.Size(333, 93)
		Me.grpEdition.TabIndex = 6
		Me.grpEdition.TabStop = false
		Me.grpEdition.Text = "Edition"
		'
		'imgEdition
		'
		Me.imgEdition.Location = New System.Drawing.Point(236, 66)
		Me.imgEdition.Name = "imgEdition"
		Me.imgEdition.Size = New System.Drawing.Size(16, 16)
		Me.imgEdition.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me.imgEdition.TabIndex = 3
		Me.imgEdition.TabStop = false
		'
		'lblYear
		'
		Me.lblYear.AutoSize = true
		Me.lblYear.Location = New System.Drawing.Point(269, 69)
		Me.lblYear.Name = "lblYear"
		Me.lblYear.Size = New System.Drawing.Size(38, 13)
		Me.lblYear.TabIndex = 2
		Me.lblYear.Text = "Année"
		Me.lblYear.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblEncNbr
		'
		Me.lblEncNbr.AutoSize = true
		Me.lblEncNbr.Location = New System.Drawing.Point(28, 66)
		Me.lblEncNbr.Name = "lblEncNbr"
		Me.lblEncNbr.Size = New System.Drawing.Size(85, 13)
		Me.lblEncNbr.TabIndex = 1
		Me.lblEncNbr.Text = "ID Encyclopédie"
		'
		'cboSerie
		'
		Me.cboSerie.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
		Me.cboSerie.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
		Me.cboSerie.FormattingEnabled = true
		Me.cboSerie.Location = New System.Drawing.Point(28, 31)
		Me.cboSerie.Name = "cboSerie"
		Me.cboSerie.Size = New System.Drawing.Size(279, 21)
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
		Me.grpId.TabIndex = 5
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
		Me.cboTitleEN.Size = New System.Drawing.Size(269, 21)
		Me.cboTitleEN.TabIndex = 1
		AddHandler Me.cboTitleEN.SelectedIndexChanged, AddressOf Me.CboTitleENSelectedIndexChanged
		AddHandler Me.cboTitleEN.KeyUp, AddressOf Me.CboTitleKeyUp
		'
		'cboTitleFR
		'
		Me.cboTitleFR.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
		Me.cboTitleFR.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
		Me.cboTitleFR.FormattingEnabled = true
		Me.cboTitleFR.Location = New System.Drawing.Point(38, 29)
		Me.cboTitleFR.Name = "cboTitleFR"
		Me.cboTitleFR.Size = New System.Drawing.Size(269, 21)
		Me.cboTitleFR.TabIndex = 0
		AddHandler Me.cboTitleFR.SelectedIndexChanged, AddressOf Me.CboTitleFRSelectedIndexChanged
		AddHandler Me.cboTitleFR.KeyUp, AddressOf Me.CboTitleKeyUp
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
		Me.ClientSize = New System.Drawing.Size(365, 328)
		Me.Controls.Add(Me.CBarAjout)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
		Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
		Me.Name = "frmAddCards"
		Me.ShowInTaskbar = false
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Ajouter des cartes"
		AddHandler Load, AddressOf Me.FrmAddCardsLoad
		AddHandler FormClosing, AddressOf Me.FrmAddCardsFormClosing
		Me.CBarAjout.ResumeLayout(false)
		Me.pnlAjout.ResumeLayout(false)
		CType(Me.imgAdd,System.ComponentModel.ISupportInitialize).EndInit
		Me.grpQuantite.ResumeLayout(false)
		Me.grpQuantite.PerformLayout
		Me.grpEdition.ResumeLayout(false)
		Me.grpEdition.PerformLayout
		CType(Me.imgEdition,System.ComponentModel.ISupportInitialize).EndInit
		Me.grpId.ResumeLayout(false)
		CType(Me.pictureBox2,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.imgVF,System.ComponentModel.ISupportInitialize).EndInit
		Me.cmnuDestination.ResumeLayout(false)
		Me.ResumeLayout(false)
	End Sub
	Private imgAdd As System.Windows.Forms.PictureBox
	Private imgVF As System.Windows.Forms.PictureBox
	Private pictureBox2 As System.Windows.Forms.PictureBox
	Private mnuDropToCollection As System.Windows.Forms.ToolStripMenuItem
	Private cmnuDestination As System.Windows.Forms.ContextMenuStrip
	Private cmdDestination As System.Windows.Forms.Button
	Private imgEdition As System.Windows.Forms.PictureBox
	Private lblYear As System.Windows.Forms.Label
	Private CBarAjout As TD.SandBar.ContainerBar
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
