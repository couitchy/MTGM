'
' Crée par SharpDevelop.
' Utilisateur: Couitchy
' Date: 09/07/2011
' Heure: 22:58
' 
' Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
'
Partial Class frmExploSettings
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmExploSettings))
		Me.toolStrip = New System.Windows.Forms.ToolStrip
		Me.btUp = New System.Windows.Forms.ToolStripButton
		Me.btDown = New System.Windows.Forms.ToolStripButton
		Me.btRefresh = New System.Windows.Forms.ToolStripButton
		Me.chklstClassement = New System.Windows.Forms.CheckedListBox
		Me.toolStrip.SuspendLayout
		Me.SuspendLayout
		'
		'toolStrip
		'
		Me.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
		Me.toolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btUp, Me.btDown, Me.btRefresh})
		Me.toolStrip.Location = New System.Drawing.Point(0, 0)
		Me.toolStrip.Name = "toolStrip"
		Me.toolStrip.Size = New System.Drawing.Size(116, 25)
		Me.toolStrip.TabIndex = 0
		'
		'btUp
		'
		Me.btUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.btUp.Enabled = false
		Me.btUp.Image = CType(resources.GetObject("btUp.Image"),System.Drawing.Image)
		Me.btUp.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btUp.Name = "btUp"
		Me.btUp.Size = New System.Drawing.Size(23, 22)
		Me.btUp.Text = "Monter"
		AddHandler Me.btUp.Click, AddressOf Me.BtUpClick
		'
		'btDown
		'
		Me.btDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.btDown.Enabled = false
		Me.btDown.Image = CType(resources.GetObject("btDown.Image"),System.Drawing.Image)
		Me.btDown.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btDown.Name = "btDown"
		Me.btDown.Size = New System.Drawing.Size(23, 22)
		Me.btDown.Text = "Descendre"
		AddHandler Me.btDown.Click, AddressOf Me.BtDownClick
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
		'chklstClassement
		'
		Me.chklstClassement.CheckOnClick = true
		Me.chklstClassement.Dock = System.Windows.Forms.DockStyle.Fill
		Me.chklstClassement.FormattingEnabled = true
		Me.chklstClassement.Items.AddRange(New Object() {"Decks", "Type", "Couleur", "Edition", "Coût d'invocation", "Rareté", "Prix", "Carte"})
		Me.chklstClassement.Location = New System.Drawing.Point(0, 25)
		Me.chklstClassement.Name = "chklstClassement"
		Me.chklstClassement.Size = New System.Drawing.Size(116, 124)
		Me.chklstClassement.TabIndex = 5
		AddHandler Me.chklstClassement.SelectedIndexChanged, AddressOf Me.ChklstClassementSelectedIndexChanged
		AddHandler Me.chklstClassement.ItemCheck, AddressOf Me.ChklstClassementItemCheck
		'
		'frmExploSettings
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(116, 151)
		Me.Controls.Add(Me.chklstClassement)
		Me.Controls.Add(Me.toolStrip)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
		Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
		Me.KeyPreview = true
		Me.Name = "frmExploSettings"
		Me.ShowInTaskbar = false
		Me.Text = "Filtres d'affichage"
		Me.TopMost = true
		AddHandler KeyUp, AddressOf Me.FrmExploSettingsKeyUp
		AddHandler FormClosing, AddressOf Me.FrmExploSettingsFormClosing
		Me.toolStrip.ResumeLayout(false)
		Me.toolStrip.PerformLayout
		Me.ResumeLayout(false)
		Me.PerformLayout
	End Sub
	Public chklstClassement As System.Windows.Forms.CheckedListBox
	Private btRefresh As System.Windows.Forms.ToolStripButton
	Private btDown As System.Windows.Forms.ToolStripButton
	Private btUp As System.Windows.Forms.ToolStripButton
	Private toolStrip As System.Windows.Forms.ToolStrip
End Class
