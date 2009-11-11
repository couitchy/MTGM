'
' Created by SharpDevelop.
' User: ${USER}
' Date: ${DATE}
' Time: ${TIME}
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Partial Class About
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(About))
		Me.lnkMail = New System.Windows.Forms.LinkLabel
		Me.lbl2 = New System.Windows.Forms.Label
		Me.label1 = New System.Windows.Forms.Label
		Me.button1 = New System.Windows.Forms.Button
		Me.lbl3 = New System.Windows.Forms.Label
		Me.groupBox1 = New System.Windows.Forms.GroupBox
		Me.tabControl1 = New System.Windows.Forms.TabControl
		Me.tabPage2 = New System.Windows.Forms.TabPage
		Me.txtCodeLines = New System.Windows.Forms.TextBox
		Me.txtDateCompile = New System.Windows.Forms.TextBox
		Me.txtVer = New System.Windows.Forms.TextBox
		Me.label4 = New System.Windows.Forms.Label
		Me.label3 = New System.Windows.Forms.Label
		Me.label2 = New System.Windows.Forms.Label
		Me.tabPage3 = New System.Windows.Forms.TabPage
		Me.label5 = New System.Windows.Forms.Label
		Me.tabPage1 = New System.Windows.Forms.TabPage
		Me.tabPage4 = New System.Windows.Forms.TabPage
		Me.txtVersions = New System.Windows.Forms.TextBox
		Me.groupBox1.SuspendLayout
		Me.tabControl1.SuspendLayout
		Me.tabPage2.SuspendLayout
		Me.tabPage3.SuspendLayout
		Me.tabPage1.SuspendLayout
		Me.tabPage4.SuspendLayout
		Me.SuspendLayout
		'
		'lnkMail
		'
		Me.lnkMail.BackColor = System.Drawing.Color.White
		Me.lnkMail.LinkColor = System.Drawing.Color.FromArgb(CType(CType(255,Byte),Integer), CType(CType(128,Byte),Integer), CType(CType(128,Byte),Integer))
		Me.lnkMail.Location = New System.Drawing.Point(238, 109)
		Me.lnkMail.Name = "lnkMail"
		Me.lnkMail.Size = New System.Drawing.Size(100, 17)
		Me.lnkMail.TabIndex = 15
		Me.lnkMail.TabStop = true
		Me.lnkMail.Text = "couitchy@free.fr"
		AddHandler Me.lnkMail.LinkClicked, AddressOf Me.LnkMailLinkClicked
		'
		'lbl2
		'
		Me.lbl2.BackColor = System.Drawing.Color.White
		Me.lbl2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lbl2.Font = New System.Drawing.Font("Tahoma", 11!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World, CType(0,Byte))
		Me.lbl2.ForeColor = System.Drawing.Color.Black
		Me.lbl2.Location = New System.Drawing.Point(6, 15)
		Me.lbl2.Name = "lbl2"
		Me.lbl2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lbl2.Size = New System.Drawing.Size(424, 141)
		Me.lbl2.TabIndex = 14
		Me.lbl2.Text = resources.GetString("lbl2.Text")
		Me.lbl2.UseCompatibleTextRendering = true
		'
		'label1
		'
		Me.label1.BackColor = System.Drawing.Color.Transparent
		Me.label1.Font = New System.Drawing.Font("Tahoma", 11!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World, CType(0,Byte))
		Me.label1.ForeColor = System.Drawing.Color.Black
		Me.label1.Location = New System.Drawing.Point(6, 46)
		Me.label1.Name = "label1"
		Me.label1.Size = New System.Drawing.Size(424, 22)
		Me.label1.TabIndex = 13
		Me.label1.Text = "Copyright 2005-2008 Couitchy Corporation. Tous droits réservés."
		Me.label1.UseCompatibleTextRendering = true
		'
		'button1
		'
		Me.button1.Location = New System.Drawing.Point(397, 63)
		Me.button1.Name = "button1"
		Me.button1.Size = New System.Drawing.Size(53, 22)
		Me.button1.TabIndex = 12
		Me.button1.Text = "OK"
		Me.button1.UseVisualStyleBackColor = true
		AddHandler Me.button1.Click, AddressOf Me.Button1Click
		'
		'lbl3
		'
		Me.lbl3.AutoSize = true
		Me.lbl3.BackColor = System.Drawing.Color.Transparent
		Me.lbl3.Font = New System.Drawing.Font("Tahoma", 15!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.lbl3.ForeColor = System.Drawing.Color.Black
		Me.lbl3.Location = New System.Drawing.Point(6, 16)
		Me.lbl3.Name = "lbl3"
		Me.lbl3.Size = New System.Drawing.Size(281, 30)
		Me.lbl3.TabIndex = 11
		Me.lbl3.Text = "Magic The Gathering Manager"
		Me.lbl3.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me.lbl3.UseCompatibleTextRendering = true
		'
		'groupBox1
		'
		Me.groupBox1.Controls.Add(Me.lbl3)
		Me.groupBox1.Controls.Add(Me.label1)
		Me.groupBox1.Location = New System.Drawing.Point(12, 12)
		Me.groupBox1.Name = "groupBox1"
		Me.groupBox1.Size = New System.Drawing.Size(342, 73)
		Me.groupBox1.TabIndex = 16
		Me.groupBox1.TabStop = false
		'
		'tabControl1
		'
		Me.tabControl1.Controls.Add(Me.tabPage2)
		Me.tabControl1.Controls.Add(Me.tabPage3)
		Me.tabControl1.Controls.Add(Me.tabPage1)
		Me.tabControl1.Controls.Add(Me.tabPage4)
		Me.tabControl1.Location = New System.Drawing.Point(12, 91)
		Me.tabControl1.Name = "tabControl1"
		Me.tabControl1.SelectedIndex = 0
		Me.tabControl1.Size = New System.Drawing.Size(442, 188)
		Me.tabControl1.TabIndex = 17
		'
		'tabPage2
		'
		Me.tabPage2.BackColor = System.Drawing.Color.Transparent
		Me.tabPage2.Controls.Add(Me.txtCodeLines)
		Me.tabPage2.Controls.Add(Me.txtDateCompile)
		Me.tabPage2.Controls.Add(Me.txtVer)
		Me.tabPage2.Controls.Add(Me.label4)
		Me.tabPage2.Controls.Add(Me.label3)
		Me.tabPage2.Controls.Add(Me.label2)
		Me.tabPage2.Location = New System.Drawing.Point(4, 22)
		Me.tabPage2.Name = "tabPage2"
		Me.tabPage2.Padding = New System.Windows.Forms.Padding(3)
		Me.tabPage2.Size = New System.Drawing.Size(434, 162)
		Me.tabPage2.TabIndex = 1
		Me.tabPage2.Text = "Version"
		Me.tabPage2.UseVisualStyleBackColor = true
		'
		'txtCodeLines
		'
		Me.txtCodeLines.Location = New System.Drawing.Point(269, 121)
		Me.txtCodeLines.Name = "txtCodeLines"
		Me.txtCodeLines.ReadOnly = true
		Me.txtCodeLines.Size = New System.Drawing.Size(69, 20)
		Me.txtCodeLines.TabIndex = 5
		Me.txtCodeLines.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		'
		'txtDateCompile
		'
		Me.txtDateCompile.Location = New System.Drawing.Point(269, 76)
		Me.txtDateCompile.Name = "txtDateCompile"
		Me.txtDateCompile.ReadOnly = true
		Me.txtDateCompile.Size = New System.Drawing.Size(69, 20)
		Me.txtDateCompile.TabIndex = 4
		Me.txtDateCompile.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		'
		'txtVer
		'
		Me.txtVer.Location = New System.Drawing.Point(269, 31)
		Me.txtVer.Name = "txtVer"
		Me.txtVer.ReadOnly = true
		Me.txtVer.Size = New System.Drawing.Size(69, 20)
		Me.txtVer.TabIndex = 3
		Me.txtVer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		'
		'label4
		'
		Me.label4.AutoSize = true
		Me.label4.Location = New System.Drawing.Point(32, 124)
		Me.label4.Name = "label4"
		Me.label4.Size = New System.Drawing.Size(131, 13)
		Me.label4.TabIndex = 2
		Me.label4.Text = "Nombre de lignes de code"
		'
		'label3
		'
		Me.label3.AutoSize = true
		Me.label3.Location = New System.Drawing.Point(32, 79)
		Me.label3.Name = "label3"
		Me.label3.Size = New System.Drawing.Size(101, 13)
		Me.label3.TabIndex = 1
		Me.label3.Text = "Date de compilation"
		'
		'label2
		'
		Me.label2.AutoSize = true
		Me.label2.Location = New System.Drawing.Point(32, 34)
		Me.label2.Name = "label2"
		Me.label2.Size = New System.Drawing.Size(96, 13)
		Me.label2.TabIndex = 0
		Me.label2.Text = "Numéro de version"
		'
		'tabPage3
		'
		Me.tabPage3.Controls.Add(Me.lnkMail)
		Me.tabPage3.Controls.Add(Me.label5)
		Me.tabPage3.Location = New System.Drawing.Point(4, 22)
		Me.tabPage3.Name = "tabPage3"
		Me.tabPage3.Size = New System.Drawing.Size(434, 162)
		Me.tabPage3.TabIndex = 2
		Me.tabPage3.Text = "Contact"
		Me.tabPage3.UseVisualStyleBackColor = true
		'
		'label5
		'
		Me.label5.BackColor = System.Drawing.Color.White
		Me.label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.label5.Font = New System.Drawing.Font("Tahoma", 11!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World, CType(0,Byte))
		Me.label5.ForeColor = System.Drawing.Color.Black
		Me.label5.Location = New System.Drawing.Point(6, 15)
		Me.label5.Name = "label5"
		Me.label5.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.label5.Size = New System.Drawing.Size(424, 141)
		Me.label5.TabIndex = 16
		Me.label5.Text = resources.GetString("label5.Text")
		Me.label5.UseCompatibleTextRendering = true
		'
		'tabPage1
		'
		Me.tabPage1.Controls.Add(Me.lbl2)
		Me.tabPage1.Location = New System.Drawing.Point(4, 22)
		Me.tabPage1.Name = "tabPage1"
		Me.tabPage1.Padding = New System.Windows.Forms.Padding(3)
		Me.tabPage1.Size = New System.Drawing.Size(434, 162)
		Me.tabPage1.TabIndex = 0
		Me.tabPage1.Text = "Informations"
		Me.tabPage1.UseVisualStyleBackColor = true
		'
		'tabPage4
		'
		Me.tabPage4.Controls.Add(Me.txtVersions)
		Me.tabPage4.Location = New System.Drawing.Point(4, 22)
		Me.tabPage4.Name = "tabPage4"
		Me.tabPage4.Padding = New System.Windows.Forms.Padding(3)
		Me.tabPage4.Size = New System.Drawing.Size(434, 162)
		Me.tabPage4.TabIndex = 3
		Me.tabPage4.Text = "Historique"
		Me.tabPage4.UseVisualStyleBackColor = true
		'
		'txtVersions
		'
		Me.txtVersions.Dock = System.Windows.Forms.DockStyle.Fill
		Me.txtVersions.Location = New System.Drawing.Point(3, 3)
		Me.txtVersions.Multiline = true
		Me.txtVersions.Name = "txtVersions"
		Me.txtVersions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
		Me.txtVersions.Size = New System.Drawing.Size(428, 156)
		Me.txtVersions.TabIndex = 0
		'
		'About
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
		Me.ClientSize = New System.Drawing.Size(464, 290)
		Me.Controls.Add(Me.tabControl1)
		Me.Controls.Add(Me.groupBox1)
		Me.Controls.Add(Me.button1)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
		Me.Name = "About"
		Me.ShowIcon = false
		Me.ShowInTaskbar = false
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "A propos..."
		AddHandler Load, AddressOf Me.AboutLoad
		Me.groupBox1.ResumeLayout(false)
		Me.groupBox1.PerformLayout
		Me.tabControl1.ResumeLayout(false)
		Me.tabPage2.ResumeLayout(false)
		Me.tabPage2.PerformLayout
		Me.tabPage3.ResumeLayout(false)
		Me.tabPage1.ResumeLayout(false)
		Me.tabPage4.ResumeLayout(false)
		Me.tabPage4.PerformLayout
		Me.ResumeLayout(false)
	End Sub
	Private txtVersions As System.Windows.Forms.TextBox
	Private tabPage4 As System.Windows.Forms.TabPage
	Private label5 As System.Windows.Forms.Label
	Private tabPage3 As System.Windows.Forms.TabPage
	Private txtCodeLines As System.Windows.Forms.TextBox
	Private txtDateCompile As System.Windows.Forms.TextBox
	Private txtVer As System.Windows.Forms.TextBox
	Private label2 As System.Windows.Forms.Label
	Private label3 As System.Windows.Forms.Label
	Private label4 As System.Windows.Forms.Label
	Private tabPage2 As System.Windows.Forms.TabPage
	Private tabPage1 As System.Windows.Forms.TabPage
	Private tabControl1 As System.Windows.Forms.TabControl
	Private groupBox1 As System.Windows.Forms.GroupBox
	Private label1 As System.Windows.Forms.Label
	Private lbl2 As System.Windows.Forms.Label
	Private lnkMail As System.Windows.Forms.LinkLabel
	Private button1 As System.Windows.Forms.Button
	Private lbl3 As System.Windows.Forms.Label
End Class
