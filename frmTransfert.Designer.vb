'
' Created by SharpDevelop.
' User: Couitchy
' Date: 02/08/2008
' Time: 00:12
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Partial Class frmTransfert
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTransfert))
		Me.cmdOK = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.grpSource = New System.Windows.Forms.GroupBox
		Me.lblCard = New System.Windows.Forms.Label
		Me.lblQuant = New System.Windows.Forms.Label
		Me.sldQuant = New System.Windows.Forms.TrackBar
		Me.lbl = New System.Windows.Forms.Label
		Me.picSerie = New System.Windows.Forms.PictureBox
		Me.cboSerie = New System.Windows.Forms.ComboBox
		Me.grpDest = New System.Windows.Forms.GroupBox
		Me.chkFoil = New System.Windows.Forms.CheckBox
		Me.picSerie2 = New System.Windows.Forms.PictureBox
		Me.cboSerie2 = New System.Windows.Forms.ComboBox
		Me.chkReserve = New System.Windows.Forms.CheckBox
		Me.grpSource.SuspendLayout
		CType(Me.sldQuant,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.picSerie,System.ComponentModel.ISupportInitialize).BeginInit
		Me.grpDest.SuspendLayout
		CType(Me.picSerie2,System.ComponentModel.ISupportInitialize).BeginInit
		Me.SuspendLayout
		'
		'cmdOK
		'
		Me.cmdOK.Location = New System.Drawing.Point(182, 214)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.Size = New System.Drawing.Size(75, 23)
		Me.cmdOK.TabIndex = 6
		Me.cmdOK.Text = "OK"
		Me.cmdOK.UseVisualStyleBackColor = true
		AddHandler Me.cmdOK.Click, AddressOf Me.CmdOKClick
		'
		'cmdCancel
		'
		Me.cmdCancel.Location = New System.Drawing.Point(101, 214)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.Size = New System.Drawing.Size(75, 23)
		Me.cmdCancel.TabIndex = 7
		Me.cmdCancel.Text = "Annuler"
		Me.cmdCancel.UseVisualStyleBackColor = true
		AddHandler Me.cmdCancel.Click, AddressOf Me.CmdCancelClick
		'
		'grpSource
		'
		Me.grpSource.Controls.Add(Me.lblCard)
		Me.grpSource.Controls.Add(Me.lblQuant)
		Me.grpSource.Controls.Add(Me.sldQuant)
		Me.grpSource.Controls.Add(Me.lbl)
		Me.grpSource.Controls.Add(Me.picSerie)
		Me.grpSource.Controls.Add(Me.cboSerie)
		Me.grpSource.Location = New System.Drawing.Point(12, 12)
		Me.grpSource.Name = "grpSource"
		Me.grpSource.Size = New System.Drawing.Size(245, 121)
		Me.grpSource.TabIndex = 13
		Me.grpSource.TabStop = false
		Me.grpSource.Text = "Source"
		'
		'lblCard
		'
		Me.lblCard.Location = New System.Drawing.Point(6, 16)
		Me.lblCard.Name = "lblCard"
		Me.lblCard.Size = New System.Drawing.Size(209, 18)
		Me.lblCard.TabIndex = 15
		Me.lblCard.Text = "Nom de la carte"
		Me.lblCard.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblQuant
		'
		Me.lblQuant.Location = New System.Drawing.Point(184, 100)
		Me.lblQuant.Name = "lblQuant"
		Me.lblQuant.Size = New System.Drawing.Size(31, 13)
		Me.lblQuant.TabIndex = 14
		Me.lblQuant.Text = "1"
		Me.lblQuant.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'sldQuant
		'
		Me.sldQuant.Location = New System.Drawing.Point(65, 64)
		Me.sldQuant.Maximum = 5
		Me.sldQuant.Name = "sldQuant"
		Me.sldQuant.Size = New System.Drawing.Size(150, 45)
		Me.sldQuant.TabIndex = 13
		Me.sldQuant.TickStyle = System.Windows.Forms.TickStyle.TopLeft
		AddHandler Me.sldQuant.Scroll, AddressOf Me.SldQuantScroll
		'
		'lbl
		'
		Me.lbl.AutoSize = true
		Me.lbl.Location = New System.Drawing.Point(6, 75)
		Me.lbl.Name = "lbl"
		Me.lbl.Size = New System.Drawing.Size(53, 13)
		Me.lbl.TabIndex = 12
		Me.lbl.Text = "Quantité :"
		'
		'picSerie
		'
		Me.picSerie.Location = New System.Drawing.Point(6, 37)
		Me.picSerie.Name = "picSerie"
		Me.picSerie.Size = New System.Drawing.Size(21, 21)
		Me.picSerie.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me.picSerie.TabIndex = 11
		Me.picSerie.TabStop = false
		'
		'cboSerie
		'
		Me.cboSerie.FormattingEnabled = true
		Me.cboSerie.Location = New System.Drawing.Point(33, 37)
		Me.cboSerie.Name = "cboSerie"
		Me.cboSerie.Size = New System.Drawing.Size(195, 21)
		Me.cboSerie.TabIndex = 10
		AddHandler Me.cboSerie.SelectedIndexChanged, AddressOf Me.CboSerieSelectedIndexChanged
		'
		'grpDest
		'
		Me.grpDest.Controls.Add(Me.chkReserve)
		Me.grpDest.Controls.Add(Me.chkFoil)
		Me.grpDest.Controls.Add(Me.picSerie2)
		Me.grpDest.Controls.Add(Me.cboSerie2)
		Me.grpDest.Location = New System.Drawing.Point(12, 139)
		Me.grpDest.Name = "grpDest"
		Me.grpDest.Size = New System.Drawing.Size(245, 69)
		Me.grpDest.TabIndex = 14
		Me.grpDest.TabStop = false
		Me.grpDest.Text = "Destination"
		Me.grpDest.Visible = false
		'
		'chkFoil
		'
		Me.chkFoil.AutoSize = true
		Me.chkFoil.Location = New System.Drawing.Point(138, 46)
		Me.chkFoil.Name = "chkFoil"
		Me.chkFoil.Size = New System.Drawing.Size(91, 17)
		Me.chkFoil.TabIndex = 15
		Me.chkFoil.Text = "Premium (Foil)"
		Me.chkFoil.UseVisualStyleBackColor = true
		'
		'picSerie2
		'
		Me.picSerie2.Location = New System.Drawing.Point(6, 19)
		Me.picSerie2.Name = "picSerie2"
		Me.picSerie2.Size = New System.Drawing.Size(21, 21)
		Me.picSerie2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me.picSerie2.TabIndex = 14
		Me.picSerie2.TabStop = false
		'
		'cboSerie2
		'
		Me.cboSerie2.FormattingEnabled = true
		Me.cboSerie2.Location = New System.Drawing.Point(33, 19)
		Me.cboSerie2.Name = "cboSerie2"
		Me.cboSerie2.Size = New System.Drawing.Size(195, 21)
		Me.cboSerie2.TabIndex = 13
		AddHandler Me.cboSerie2.SelectedIndexChanged, AddressOf Me.CboSerie2SelectedIndexChanged
		'
		'chkReserve
		'
		Me.chkReserve.AutoSize = true
		Me.chkReserve.Location = New System.Drawing.Point(33, 46)
		Me.chkReserve.Name = "chkReserve"
		Me.chkReserve.Size = New System.Drawing.Size(96, 17)
		Me.chkReserve.TabIndex = 16
		Me.chkReserve.Text = "Jeu de réserve"
		Me.chkReserve.UseVisualStyleBackColor = true
		'
		'frmTransfert
		'
		Me.AcceptButton = Me.cmdOK
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(269, 245)
		Me.Controls.Add(Me.grpDest)
		Me.Controls.Add(Me.grpSource)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
		Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
		Me.Name = "frmTransfert"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Transfert"
		Me.grpSource.ResumeLayout(false)
		Me.grpSource.PerformLayout
		CType(Me.sldQuant,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.picSerie,System.ComponentModel.ISupportInitialize).EndInit
		Me.grpDest.ResumeLayout(false)
		Me.grpDest.PerformLayout
		CType(Me.picSerie2,System.ComponentModel.ISupportInitialize).EndInit
		Me.ResumeLayout(false)
	End Sub
	Private chkReserve As System.Windows.Forms.CheckBox
	Private grpDest As System.Windows.Forms.GroupBox
	Private grpSource As System.Windows.Forms.GroupBox
	Private chkFoil As System.Windows.Forms.CheckBox
	Private cboSerie2 As System.Windows.Forms.ComboBox
	Private picSerie2 As System.Windows.Forms.PictureBox
	Private lblCard As System.Windows.Forms.Label
	Private lbl As System.Windows.Forms.Label
	Private cmdCancel As System.Windows.Forms.Button
	Private cmdOK As System.Windows.Forms.Button
	Private sldQuant As System.Windows.Forms.TrackBar
	Private lblQuant As System.Windows.Forms.Label
	Private cboSerie As System.Windows.Forms.ComboBox
	Private picSerie As System.Windows.Forms.PictureBox
End Class
