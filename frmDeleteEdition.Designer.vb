'
' Created by SharpDevelop.
' User: Couitchy
' Date: 03/09/2008
' Time: 18:09
'
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Partial Class frmDeleteEdition
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDeleteEdition))
        Me.grpSerie = New System.Windows.Forms.GroupBox
        Me.cmdGo = New System.Windows.Forms.Button
        Me.lblSerie = New System.Windows.Forms.Label
        Me.picSerie = New System.Windows.Forms.PictureBox
        Me.cboSerie = New System.Windows.Forms.ComboBox
        Me.grpAdvance = New System.Windows.Forms.GroupBox
        Me.chkHeader = New System.Windows.Forms.CheckBox
        Me.chkCards = New System.Windows.Forms.CheckBox
        Me.grpSerie.SuspendLayout
        CType(Me.picSerie,System.ComponentModel.ISupportInitialize).BeginInit
        Me.grpAdvance.SuspendLayout
        Me.SuspendLayout
        '
        'grpSerie
        '
        Me.grpSerie.Controls.Add(Me.cmdGo)
        Me.grpSerie.Controls.Add(Me.lblSerie)
        Me.grpSerie.Controls.Add(Me.picSerie)
        Me.grpSerie.Controls.Add(Me.cboSerie)
        Me.grpSerie.Dock = System.Windows.Forms.DockStyle.Top
        Me.grpSerie.Location = New System.Drawing.Point(0, 0)
        Me.grpSerie.Name = "grpSerie"
        Me.grpSerie.Size = New System.Drawing.Size(292, 97)
        Me.grpSerie.TabIndex = 1
        Me.grpSerie.TabStop = false
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
        Me.lblSerie.Size = New System.Drawing.Size(102, 13)
        Me.lblSerie.TabIndex = 2
        Me.lblSerie.Text = "Edition à supprimer :"
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
        'grpAdvance
        '
        Me.grpAdvance.Controls.Add(Me.chkHeader)
        Me.grpAdvance.Controls.Add(Me.chkCards)
        Me.grpAdvance.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpAdvance.Location = New System.Drawing.Point(0, 97)
        Me.grpAdvance.Name = "grpAdvance"
        Me.grpAdvance.Size = New System.Drawing.Size(292, 97)
        Me.grpAdvance.TabIndex = 2
        Me.grpAdvance.TabStop = false
        '
        'chkHeader
        '
        Me.chkHeader.AutoSize = true
        Me.chkHeader.Checked = true
        Me.chkHeader.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkHeader.Location = New System.Drawing.Point(12, 55)
        Me.chkHeader.Name = "chkHeader"
        Me.chkHeader.Size = New System.Drawing.Size(191, 17)
        Me.chkHeader.TabIndex = 1
        Me.chkHeader.Text = "Supprimer aussi l'en-tête de l'édition"
        Me.chkHeader.UseVisualStyleBackColor = true
        '
        'chkCards
        '
        Me.chkCards.Checked = true
        Me.chkCards.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkCards.Enabled = false
        Me.chkCards.Location = New System.Drawing.Point(12, 19)
        Me.chkCards.Name = "chkCards"
        Me.chkCards.Size = New System.Drawing.Size(269, 30)
        Me.chkCards.TabIndex = 0
        Me.chkCards.Text = "Supprimer aussi les cartes saisies dans cette édition (0 cartes concernées)"
        Me.chkCards.UseVisualStyleBackColor = true
        '
        'frmDeleteEdition
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(292, 194)
        Me.Controls.Add(Me.grpAdvance)
        Me.Controls.Add(Me.grpSerie)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
        Me.Name = "frmDeleteEdition"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Suppression d'une édition"
        AddHandler Load, AddressOf Me.FrmDeleteEditionLoad
        Me.grpSerie.ResumeLayout(false)
        Me.grpSerie.PerformLayout
        CType(Me.picSerie,System.ComponentModel.ISupportInitialize).EndInit
        Me.grpAdvance.ResumeLayout(false)
        Me.grpAdvance.PerformLayout
        Me.ResumeLayout(false)
    End Sub
    Private chkCards As System.Windows.Forms.CheckBox
    Private chkHeader As System.Windows.Forms.CheckBox
    Private grpAdvance As System.Windows.Forms.GroupBox
    Private cboSerie As System.Windows.Forms.ComboBox
    Private picSerie As System.Windows.Forms.PictureBox
    Private lblSerie As System.Windows.Forms.Label
    Private cmdGo As System.Windows.Forms.Button
    Private grpSerie As System.Windows.Forms.GroupBox
End Class
