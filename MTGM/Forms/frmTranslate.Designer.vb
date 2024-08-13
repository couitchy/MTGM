'
' Created by SharpDevelop.
' User: Couitchy
' Date: 09/05/2008
' Time: 22:22
'
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Partial Class frmTranslate
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTranslate))
        Me.grpSerie = New System.Windows.Forms.GroupBox
        Me.chkAlert = New System.Windows.Forms.CheckBox
        Me.cmdGo = New System.Windows.Forms.Button
        Me.lblSerie = New System.Windows.Forms.Label
        Me.picSerie = New System.Windows.Forms.PictureBox
        Me.cboSerie = New System.Windows.Forms.ComboBox
        Me.grpTranslate = New System.Windows.Forms.GroupBox
        Me.txtCount = New System.Windows.Forms.TextBox
        Me.txtFR = New System.Windows.Forms.TextBox
        Me.txtEN = New System.Windows.Forms.TextBox
        Me.lblCount = New System.Windows.Forms.Label
        Me.lblFR = New System.Windows.Forms.Label
        Me.lblEN = New System.Windows.Forms.Label
        Me.dlgOpen = New System.Windows.Forms.OpenFileDialog
        Me.grpSerie.SuspendLayout
        CType(Me.picSerie,System.ComponentModel.ISupportInitialize).BeginInit
        Me.grpTranslate.SuspendLayout
        Me.SuspendLayout
        '
        'grpSerie
        '
        Me.grpSerie.Controls.Add(Me.chkAlert)
        Me.grpSerie.Controls.Add(Me.cmdGo)
        Me.grpSerie.Controls.Add(Me.lblSerie)
        Me.grpSerie.Controls.Add(Me.picSerie)
        Me.grpSerie.Controls.Add(Me.cboSerie)
        Me.grpSerie.Dock = System.Windows.Forms.DockStyle.Top
        Me.grpSerie.Location = New System.Drawing.Point(0, 0)
        Me.grpSerie.Name = "grpSerie"
        Me.grpSerie.Size = New System.Drawing.Size(292, 97)
        Me.grpSerie.TabIndex = 0
        Me.grpSerie.TabStop = false
        '
        'chkAlert
        '
        Me.chkAlert.AutoSize = true
        Me.chkAlert.Checked = true
        Me.chkAlert.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAlert.Location = New System.Drawing.Point(142, 70)
        Me.chkAlert.Name = "chkAlert"
        Me.chkAlert.Size = New System.Drawing.Size(138, 17)
        Me.chkAlert.TabIndex = 4
        Me.chkAlert.Text = "Prévenir en cas d'erreur"
        Me.chkAlert.UseVisualStyleBackColor = true
        '
        'cmdGo
        '
        Me.cmdGo.Location = New System.Drawing.Point(227, 41)
        Me.cmdGo.Name = "cmdGo"
        Me.cmdGo.Size = New System.Drawing.Size(53, 23)
        Me.cmdGo.TabIndex = 3
        Me.cmdGo.Text = "OK"
        Me.cmdGo.UseVisualStyleBackColor = true
        AddHandler Me.cmdGo.Click, AddressOf Me.CmdGoClick
        '
        'lblSerie
        '
        Me.lblSerie.AutoSize = true
        Me.lblSerie.Location = New System.Drawing.Point(12, 16)
        Me.lblSerie.Name = "lblSerie"
        Me.lblSerie.Size = New System.Drawing.Size(92, 13)
        Me.lblSerie.TabIndex = 2
        Me.lblSerie.Text = "Edition à traduire :"
        '
        'picSerie
        '
        Me.picSerie.Location = New System.Drawing.Point(12, 41)
        Me.picSerie.Name = "picSerie"
        Me.picSerie.Size = New System.Drawing.Size(21, 21)
        Me.picSerie.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picSerie.TabIndex = 1
        Me.picSerie.TabStop = false
        '
        'cboSerie
        '
        Me.cboSerie.FormattingEnabled = true
        Me.cboSerie.Location = New System.Drawing.Point(39, 41)
        Me.cboSerie.Name = "cboSerie"
        Me.cboSerie.Size = New System.Drawing.Size(182, 21)
        Me.cboSerie.TabIndex = 0
        AddHandler Me.cboSerie.SelectedIndexChanged, AddressOf Me.CboSerieSelectedIndexChanged
        '
        'grpTranslate
        '
        Me.grpTranslate.Controls.Add(Me.txtCount)
        Me.grpTranslate.Controls.Add(Me.txtFR)
        Me.grpTranslate.Controls.Add(Me.txtEN)
        Me.grpTranslate.Controls.Add(Me.lblCount)
        Me.grpTranslate.Controls.Add(Me.lblFR)
        Me.grpTranslate.Controls.Add(Me.lblEN)
        Me.grpTranslate.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpTranslate.Location = New System.Drawing.Point(0, 97)
        Me.grpTranslate.Name = "grpTranslate"
        Me.grpTranslate.Size = New System.Drawing.Size(292, 99)
        Me.grpTranslate.TabIndex = 1
        Me.grpTranslate.TabStop = false
        '
        'txtCount
        '
        Me.txtCount.Enabled = false
        Me.txtCount.Location = New System.Drawing.Point(114, 67)
        Me.txtCount.Name = "txtCount"
        Me.txtCount.Size = New System.Drawing.Size(166, 20)
        Me.txtCount.TabIndex = 5
        Me.txtCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtFR
        '
        Me.txtFR.Enabled = false
        Me.txtFR.Location = New System.Drawing.Point(114, 42)
        Me.txtFR.Name = "txtFR"
        Me.txtFR.Size = New System.Drawing.Size(166, 20)
        Me.txtFR.TabIndex = 4
        Me.txtFR.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtEN
        '
        Me.txtEN.Enabled = false
        Me.txtEN.Location = New System.Drawing.Point(114, 17)
        Me.txtEN.Name = "txtEN"
        Me.txtEN.Size = New System.Drawing.Size(166, 20)
        Me.txtEN.TabIndex = 3
        Me.txtEN.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblCount
        '
        Me.lblCount.AutoSize = true
        Me.lblCount.Location = New System.Drawing.Point(12, 70)
        Me.lblCount.Name = "lblCount"
        Me.lblCount.Size = New System.Drawing.Size(80, 13)
        Me.lblCount.TabIndex = 2
        Me.lblCount.Text = "Cartes traitées :"
        '
        'lblFR
        '
        Me.lblFR.AutoSize = true
        Me.lblFR.Location = New System.Drawing.Point(12, 45)
        Me.lblFR.Name = "lblFR"
        Me.lblFR.Size = New System.Drawing.Size(75, 13)
        Me.lblFR.TabIndex = 1
        Me.lblFR.Text = "Nom français :"
        '
        'lblEN
        '
        Me.lblEN.AutoSize = true
        Me.lblEN.Location = New System.Drawing.Point(12, 20)
        Me.lblEN.Name = "lblEN"
        Me.lblEN.Size = New System.Drawing.Size(71, 13)
        Me.lblEN.TabIndex = 0
        Me.lblEN.Text = "Nom anglais :"
        '
        'dlgOpen
        '
        Me.dlgOpen.DefaultExt = "txt"
        Me.dlgOpen.Filter = "Fichiers texte (*.txt)|*.txt"
        Me.dlgOpen.Title = "Sélection du fichier de traductions"
        '
        'frmTranslate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(292, 196)
        Me.Controls.Add(Me.grpTranslate)
        Me.Controls.Add(Me.grpSerie)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
        Me.Name = "frmTranslate"
        Me.ShowInTaskbar = false
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Traduction des cartes d'une édition"
        AddHandler Load, AddressOf Me.FrmTranslateLoad
        AddHandler FormClosing, AddressOf Me.FrmTranslateFormClosing
        Me.grpSerie.ResumeLayout(false)
        Me.grpSerie.PerformLayout
        CType(Me.picSerie,System.ComponentModel.ISupportInitialize).EndInit
        Me.grpTranslate.ResumeLayout(false)
        Me.grpTranslate.PerformLayout
        Me.ResumeLayout(false)
    End Sub
    Private dlgOpen As System.Windows.Forms.OpenFileDialog
    Private chkAlert As System.Windows.Forms.CheckBox
    Private lblEN As System.Windows.Forms.Label
    Private lblFR As System.Windows.Forms.Label
    Private lblCount As System.Windows.Forms.Label
    Private txtEN As System.Windows.Forms.TextBox
    Private txtFR As System.Windows.Forms.TextBox
    Private txtCount As System.Windows.Forms.TextBox
    Private grpTranslate As System.Windows.Forms.GroupBox
    Private cboSerie As System.Windows.Forms.ComboBox
    Private picSerie As System.Windows.Forms.PictureBox
    Private lblSerie As System.Windows.Forms.Label
    Private cmdGo As System.Windows.Forms.Button
    Private grpSerie As System.Windows.Forms.GroupBox
End Class
