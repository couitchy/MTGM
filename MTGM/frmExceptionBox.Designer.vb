'
' Created by SharpDevelop.
' User: Couitchy
' Date: 05/07/2009
' Time: 14:16
'
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Partial Class frmExceptionBox
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmExceptionBox))
        Me.picBug = New System.Windows.Forms.PictureBox
        Me.lblInfo = New System.Windows.Forms.Label
        Me.cmdSend = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdContinue = New System.Windows.Forms.Button
        CType(Me.picBug,System.ComponentModel.ISupportInitialize).BeginInit
        Me.SuspendLayout
        '
        'picBug
        '
        Me.picBug.Image = CType(resources.GetObject("picBug.Image"),System.Drawing.Image)
        Me.picBug.Location = New System.Drawing.Point(12, 12)
        Me.picBug.Name = "picBug"
        Me.picBug.Size = New System.Drawing.Size(150, 175)
        Me.picBug.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.picBug.TabIndex = 0
        Me.picBug.TabStop = false
        '
        'lblInfo
        '
        Me.lblInfo.AutoSize = true
        Me.lblInfo.Location = New System.Drawing.Point(173, 21)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(282, 117)
        Me.lblInfo.TabIndex = 1
        Me.lblInfo.Text = resources.GetString("lblInfo.Text")
        '
        'cmdSend
        '
        Me.cmdSend.Location = New System.Drawing.Point(173, 164)
        Me.cmdSend.Name = "cmdSend"
        Me.cmdSend.Size = New System.Drawing.Size(75, 23)
        Me.cmdSend.TabIndex = 2
        Me.cmdSend.Text = "Envoyer"
        Me.cmdSend.UseVisualStyleBackColor = true
        AddHandler Me.cmdSend.Click, AddressOf Me.CmdSendClick
        '
        'cmdExit
        '
        Me.cmdExit.Location = New System.Drawing.Point(281, 164)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.Size = New System.Drawing.Size(75, 23)
        Me.cmdExit.TabIndex = 3
        Me.cmdExit.Text = "Quitter"
        Me.cmdExit.UseVisualStyleBackColor = true
        AddHandler Me.cmdExit.Click, AddressOf Me.CmdExitClick
        '
        'cmdContinue
        '
        Me.cmdContinue.Location = New System.Drawing.Point(389, 164)
        Me.cmdContinue.Name = "cmdContinue"
        Me.cmdContinue.Size = New System.Drawing.Size(75, 23)
        Me.cmdContinue.TabIndex = 4
        Me.cmdContinue.Text = "Continuer"
        Me.cmdContinue.UseVisualStyleBackColor = true
        AddHandler Me.cmdContinue.Click, AddressOf Me.CmdContinueClick
        '
        'frmExceptionBox
        '
        Me.AcceptButton = Me.cmdContinue
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(476, 205)
        Me.Controls.Add(Me.cmdContinue)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdSend)
        Me.Controls.Add(Me.lblInfo)
        Me.Controls.Add(Me.picBug)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
        Me.MaximizeBox = false
        Me.Name = "frmExceptionBox"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Ooooooooooops !"
        CType(Me.picBug,System.ComponentModel.ISupportInitialize).EndInit
        Me.ResumeLayout(false)
        Me.PerformLayout
    End Sub
    Private cmdContinue As System.Windows.Forms.Button
    Private cmdExit As System.Windows.Forms.Button
    Private cmdSend As System.Windows.Forms.Button
    Private lblInfo As System.Windows.Forms.Label
    Private picBug As System.Windows.Forms.PictureBox
End Class
