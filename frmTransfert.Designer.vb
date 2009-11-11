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
		Me.picSerie = New System.Windows.Forms.PictureBox
		Me.cboSerie = New System.Windows.Forms.ComboBox
		Me.lbl = New System.Windows.Forms.Label
		Me.sldQuant = New System.Windows.Forms.TrackBar
		Me.cmdOK = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.lblQuant = New System.Windows.Forms.Label
		Me.lblCard = New System.Windows.Forms.Label
		CType(Me.picSerie,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.sldQuant,System.ComponentModel.ISupportInitialize).BeginInit
		Me.SuspendLayout
		'
		'picSerie
		'
		Me.picSerie.Location = New System.Drawing.Point(10, 30)
		Me.picSerie.Name = "picSerie"
		Me.picSerie.Size = New System.Drawing.Size(21, 21)
		Me.picSerie.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me.picSerie.TabIndex = 3
		Me.picSerie.TabStop = false
		'
		'cboSerie
		'
		Me.cboSerie.FormattingEnabled = true
		Me.cboSerie.Location = New System.Drawing.Point(37, 30)
		Me.cboSerie.Name = "cboSerie"
		Me.cboSerie.Size = New System.Drawing.Size(182, 21)
		Me.cboSerie.TabIndex = 2
		AddHandler Me.cboSerie.SelectedIndexChanged, AddressOf Me.CboSerieSelectedIndexChanged
		'
		'lbl
		'
		Me.lbl.AutoSize = true
		Me.lbl.Location = New System.Drawing.Point(10, 69)
		Me.lbl.Name = "lbl"
		Me.lbl.Size = New System.Drawing.Size(53, 13)
		Me.lbl.TabIndex = 4
		Me.lbl.Text = "Quantité :"
		'
		'sldQuant
		'
		Me.sldQuant.Location = New System.Drawing.Point(69, 58)
		Me.sldQuant.Maximum = 5
		Me.sldQuant.Name = "sldQuant"
		Me.sldQuant.Size = New System.Drawing.Size(150, 45)
		Me.sldQuant.TabIndex = 5
		Me.sldQuant.TickStyle = System.Windows.Forms.TickStyle.TopLeft
		AddHandler Me.sldQuant.Scroll, AddressOf Me.SldQuantScroll
		'
		'cmdOK
		'
		Me.cmdOK.Location = New System.Drawing.Point(144, 109)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.Size = New System.Drawing.Size(75, 23)
		Me.cmdOK.TabIndex = 6
		Me.cmdOK.Text = "OK"
		Me.cmdOK.UseVisualStyleBackColor = true
		AddHandler Me.cmdOK.Click, AddressOf Me.CmdOKClick
		'
		'cmdCancel
		'
		Me.cmdCancel.Location = New System.Drawing.Point(63, 109)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.Size = New System.Drawing.Size(75, 23)
		Me.cmdCancel.TabIndex = 7
		Me.cmdCancel.Text = "Annuler"
		Me.cmdCancel.UseVisualStyleBackColor = true
		AddHandler Me.cmdCancel.Click, AddressOf Me.CmdCancelClick
		'
		'lblQuant
		'
		Me.lblQuant.AutoSize = true
		Me.lblQuant.Location = New System.Drawing.Point(44, 90)
		Me.lblQuant.Name = "lblQuant"
		Me.lblQuant.Size = New System.Drawing.Size(13, 13)
		Me.lblQuant.TabIndex = 8
		Me.lblQuant.Text = "1"
		'
		'lblCard
		'
		Me.lblCard.Location = New System.Drawing.Point(10, 9)
		Me.lblCard.Name = "lblCard"
		Me.lblCard.Size = New System.Drawing.Size(209, 18)
		Me.lblCard.TabIndex = 9
		Me.lblCard.Text = "Nom de la carte"
		Me.lblCard.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'frmTransfert
		'
		Me.AcceptButton = Me.cmdOK
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(231, 142)
		Me.Controls.Add(Me.lblCard)
		Me.Controls.Add(Me.lblQuant)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.sldQuant)
		Me.Controls.Add(Me.lbl)
		Me.Controls.Add(Me.picSerie)
		Me.Controls.Add(Me.cboSerie)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
		Me.Name = "frmTransfert"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Transfert / Suppression"
		CType(Me.picSerie,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.sldQuant,System.ComponentModel.ISupportInitialize).EndInit
		Me.ResumeLayout(false)
		Me.PerformLayout
	End Sub
	Private lblCard As System.Windows.Forms.Label
	Private lbl As System.Windows.Forms.Label
	Private cmdCancel As System.Windows.Forms.Button
	Private cmdOK As System.Windows.Forms.Button
	Private sldQuant As System.Windows.Forms.TrackBar
	Private lblQuant As System.Windows.Forms.Label
	Private cboSerie As System.Windows.Forms.ComboBox
	Private picSerie As System.Windows.Forms.PictureBox
End Class
