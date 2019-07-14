'
' Created by SharpDevelop.
' User: Couitchy
' Date: 06/05/2008
' Time: 18:49
'
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Partial Class frmNewEdition
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmNewEdition))
        Me.grpData = New System.Windows.Forms.GroupBox
        Me.cmdOK = New System.Windows.Forms.Button
        Me.splitH = New System.Windows.Forms.SplitContainer
        Me.cmdBrowsetxtSpoilerList = New System.Windows.Forms.Button
        Me.cmdBrowsetxtCheckList = New System.Windows.Forms.Button
        Me.txtSpoilerList = New System.Windows.Forms.TextBox
        Me.txtCheckList = New System.Windows.Forms.TextBox
        Me.lblSpoilerList = New System.Windows.Forms.Label
        Me.lblCheckList = New System.Windows.Forms.Label
        Me.lblData = New System.Windows.Forms.Label
        Me.chkNewEdition = New System.Windows.Forms.CheckedListBox
        Me.dlgOpen = New System.Windows.Forms.OpenFileDialog
        Me.grpAssist = New System.Windows.Forms.GroupBox
        Me.optUpdate = New System.Windows.Forms.RadioButton
        Me.optManual = New System.Windows.Forms.RadioButton
        Me.optAuto = New System.Windows.Forms.RadioButton
        Me.lnklblAssist = New System.Windows.Forms.LinkLabel
        Me.lblAssist3 = New System.Windows.Forms.Label
        Me.picMagic = New System.Windows.Forms.PictureBox
        Me.cmdAssistCancel = New System.Windows.Forms.Button
        Me.cmdAssistNext = New System.Windows.Forms.Button
        Me.lblAssist2 = New System.Windows.Forms.Label
        Me.lblAssist1 = New System.Windows.Forms.Label
        Me.grpHeader = New System.Windows.Forms.GroupBox
        Me.chkHeaderAlready = New System.Windows.Forms.CheckBox
        Me.cmdHeaderPrevious = New System.Windows.Forms.Button
        Me.cmdHeaderNext = New System.Windows.Forms.Button
        Me.propEdition = New System.Windows.Forms.PropertyGrid
        Me.lblHeader = New System.Windows.Forms.Label
        Me.grpAuto = New System.Windows.Forms.GroupBox
        Me.lblStatus = New System.Windows.Forms.Label
        Me.lblAuto2 = New System.Windows.Forms.Label
        Me.lblAuto1 = New System.Windows.Forms.Label
        Me.chkNewEditionAuto = New System.Windows.Forms.CheckedListBox
        Me.cmdAutoPrevious = New System.Windows.Forms.Button
        Me.cmdAutoNext = New System.Windows.Forms.Button
        Me.grpData.SuspendLayout
        Me.splitH.Panel1.SuspendLayout
        Me.splitH.Panel2.SuspendLayout
        Me.splitH.SuspendLayout
        Me.grpAssist.SuspendLayout
        CType(Me.picMagic,System.ComponentModel.ISupportInitialize).BeginInit
        Me.grpHeader.SuspendLayout
        Me.grpAuto.SuspendLayout
        Me.SuspendLayout
        '
        'grpData
        '
        Me.grpData.Controls.Add(Me.cmdOK)
        Me.grpData.Controls.Add(Me.splitH)
        Me.grpData.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpData.Location = New System.Drawing.Point(0, 0)
        Me.grpData.Name = "grpData"
        Me.grpData.Size = New System.Drawing.Size(439, 355)
        Me.grpData.TabIndex = 0
        Me.grpData.TabStop = false
        Me.grpData.Visible = false
        '
        'cmdOK
        '
        Me.cmdOK.Location = New System.Drawing.Point(352, 320)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(75, 23)
        Me.cmdOK.TabIndex = 7
        Me.cmdOK.Text = "Valider"
        Me.cmdOK.UseVisualStyleBackColor = true
        AddHandler Me.cmdOK.Click, AddressOf Me.CmdOKClick
        '
        'splitH
        '
        Me.splitH.IsSplitterFixed = true
        Me.splitH.Location = New System.Drawing.Point(3, 26)
        Me.splitH.Name = "splitH"
        Me.splitH.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'splitH.Panel1
        '
        Me.splitH.Panel1.Controls.Add(Me.cmdBrowsetxtSpoilerList)
        Me.splitH.Panel1.Controls.Add(Me.cmdBrowsetxtCheckList)
        Me.splitH.Panel1.Controls.Add(Me.txtSpoilerList)
        Me.splitH.Panel1.Controls.Add(Me.txtCheckList)
        Me.splitH.Panel1.Controls.Add(Me.lblSpoilerList)
        Me.splitH.Panel1.Controls.Add(Me.lblCheckList)
        Me.splitH.Panel1.Controls.Add(Me.lblData)
        '
        'splitH.Panel2
        '
        Me.splitH.Panel2.Controls.Add(Me.chkNewEdition)
        Me.splitH.Size = New System.Drawing.Size(433, 280)
        Me.splitH.SplitterDistance = 142
        Me.splitH.TabIndex = 1
        '
        'cmdBrowsetxtSpoilerList
        '
        Me.cmdBrowsetxtSpoilerList.Location = New System.Drawing.Point(406, 90)
        Me.cmdBrowsetxtSpoilerList.Name = "cmdBrowsetxtSpoilerList"
        Me.cmdBrowsetxtSpoilerList.Size = New System.Drawing.Size(20, 20)
        Me.cmdBrowsetxtSpoilerList.TabIndex = 6
        Me.cmdBrowsetxtSpoilerList.Text = "."
        Me.cmdBrowsetxtSpoilerList.UseVisualStyleBackColor = true
        AddHandler Me.cmdBrowsetxtSpoilerList.Click, AddressOf Me.CmdBrowseClick
        '
        'cmdBrowsetxtCheckList
        '
        Me.cmdBrowsetxtCheckList.Location = New System.Drawing.Point(406, 53)
        Me.cmdBrowsetxtCheckList.Name = "cmdBrowsetxtCheckList"
        Me.cmdBrowsetxtCheckList.Size = New System.Drawing.Size(20, 20)
        Me.cmdBrowsetxtCheckList.TabIndex = 5
        Me.cmdBrowsetxtCheckList.Text = "."
        Me.cmdBrowsetxtCheckList.UseVisualStyleBackColor = true
        AddHandler Me.cmdBrowsetxtCheckList.Click, AddressOf Me.CmdBrowseClick
        '
        'txtSpoilerList
        '
        Me.txtSpoilerList.Location = New System.Drawing.Point(80, 90)
        Me.txtSpoilerList.Name = "txtSpoilerList"
        Me.txtSpoilerList.Size = New System.Drawing.Size(320, 20)
        Me.txtSpoilerList.TabIndex = 4
        '
        'txtCheckList
        '
        Me.txtCheckList.Location = New System.Drawing.Point(80, 53)
        Me.txtCheckList.Name = "txtCheckList"
        Me.txtCheckList.Size = New System.Drawing.Size(320, 20)
        Me.txtCheckList.TabIndex = 3
        '
        'lblSpoilerList
        '
        Me.lblSpoilerList.AutoSize = true
        Me.lblSpoilerList.Location = New System.Drawing.Point(12, 93)
        Me.lblSpoilerList.Name = "lblSpoilerList"
        Me.lblSpoilerList.Size = New System.Drawing.Size(63, 13)
        Me.lblSpoilerList.TabIndex = 2
        Me.lblSpoilerList.Text = "Spoiler list - "
        '
        'lblCheckList
        '
        Me.lblCheckList.AutoSize = true
        Me.lblCheckList.Location = New System.Drawing.Point(12, 56)
        Me.lblCheckList.Name = "lblCheckList"
        Me.lblCheckList.Size = New System.Drawing.Size(62, 13)
        Me.lblCheckList.TabIndex = 1
        Me.lblCheckList.Text = "Check list - "
        '
        'lblData
        '
        Me.lblData.BackColor = System.Drawing.SystemColors.Control
        Me.lblData.Location = New System.Drawing.Point(12, 9)
        Me.lblData.Name = "lblData"
        Me.lblData.Size = New System.Drawing.Size(388, 29)
        Me.lblData.TabIndex = 0
        Me.lblData.Text = "Choisissez une édition dont les cartes ne sont pas encore référencées ainsi que l"& _
        "es deux fichiers Wizard of The Coast les accompagnant :"
        '
        'chkNewEdition
        '
        Me.chkNewEdition.CheckOnClick = true
        Me.chkNewEdition.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chkNewEdition.FormattingEnabled = true
        Me.chkNewEdition.Location = New System.Drawing.Point(0, 0)
        Me.chkNewEdition.MultiColumn = true
        Me.chkNewEdition.Name = "chkNewEdition"
        Me.chkNewEdition.Size = New System.Drawing.Size(433, 124)
        Me.chkNewEdition.TabIndex = 0
        AddHandler Me.chkNewEdition.ItemCheck, AddressOf Me.ChkNewEditionItemCheck
        '
        'dlgOpen
        '
        Me.dlgOpen.DefaultExt = "txt"
        Me.dlgOpen.Filter = "Fichiers texte (*.txt) | *.txt"
        Me.dlgOpen.Title = "Sélection de fichier"
        '
        'grpAssist
        '
        Me.grpAssist.Controls.Add(Me.optUpdate)
        Me.grpAssist.Controls.Add(Me.optManual)
        Me.grpAssist.Controls.Add(Me.optAuto)
        Me.grpAssist.Controls.Add(Me.lnklblAssist)
        Me.grpAssist.Controls.Add(Me.lblAssist3)
        Me.grpAssist.Controls.Add(Me.picMagic)
        Me.grpAssist.Controls.Add(Me.cmdAssistCancel)
        Me.grpAssist.Controls.Add(Me.cmdAssistNext)
        Me.grpAssist.Controls.Add(Me.lblAssist2)
        Me.grpAssist.Controls.Add(Me.lblAssist1)
        Me.grpAssist.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpAssist.Location = New System.Drawing.Point(0, 0)
        Me.grpAssist.Name = "grpAssist"
        Me.grpAssist.Size = New System.Drawing.Size(439, 355)
        Me.grpAssist.TabIndex = 1
        Me.grpAssist.TabStop = false
        '
        'optUpdate
        '
        Me.optUpdate.AutoSize = true
        Me.optUpdate.Location = New System.Drawing.Point(15, 297)
        Me.optUpdate.Name = "optUpdate"
        Me.optUpdate.Size = New System.Drawing.Size(191, 17)
        Me.optUpdate.TabIndex = 10
        Me.optUpdate.Text = "Mettre à jour les en-têtes des éditions"
        Me.optUpdate.UseVisualStyleBackColor = true
        '
        'optManual
        '
        Me.optManual.AutoSize = true
        Me.optManual.Location = New System.Drawing.Point(15, 274)
        Me.optManual.Name = "optManual"
        Me.optManual.Size = New System.Drawing.Size(89, 17)
        Me.optManual.TabIndex = 9
        Me.optManual.Text = "Mode manuel"
        Me.optManual.UseVisualStyleBackColor = true
        AddHandler Me.optManual.CheckedChanged, AddressOf Me.OptManualCheckedChanged
        '
        'optAuto
        '
        Me.optAuto.AutoSize = true
        Me.optAuto.Checked = true
        Me.optAuto.Location = New System.Drawing.Point(15, 251)
        Me.optAuto.Name = "optAuto"
        Me.optAuto.Size = New System.Drawing.Size(183, 17)
        Me.optAuto.TabIndex = 8
        Me.optAuto.TabStop = true
        Me.optAuto.Text = "Mode automatique (recommandé)"
        Me.optAuto.UseVisualStyleBackColor = true
        '
        'lnklblAssist
        '
        Me.lnklblAssist.AutoSize = true
        Me.lnklblAssist.Enabled = false
        Me.lnklblAssist.Location = New System.Drawing.Point(110, 276)
        Me.lnklblAssist.Name = "lnklblAssist"
        Me.lnklblAssist.Size = New System.Drawing.Size(297, 13)
        Me.lnklblAssist.TabIndex = 7
        Me.lnklblAssist.TabStop = true
        Me.lnklblAssist.Text = "Fichiers de description des éditions"
        AddHandler Me.lnklblAssist.LinkClicked, AddressOf Me.LnklblAssistLinkClicked
        '
        'lblAssist3
        '
        Me.lblAssist3.Location = New System.Drawing.Point(179, 187)
        Me.lblAssist3.Name = "lblAssist3"
        Me.lblAssist3.Size = New System.Drawing.Size(248, 61)
        Me.lblAssist3.TabIndex = 5
        Me.lblAssist3.Text = "Avant de commencer, choisissez le mode désiré. L'option automatique est recommand"& _
        "ée, mais si vous ne trouvez pas l'édition recherchée, il faudra passer en mode m"& _
        "anuel..."
        '
        'picMagic
        '
        Me.picMagic.Location = New System.Drawing.Point(12, 19)
        Me.picMagic.Name = "picMagic"
        Me.picMagic.Size = New System.Drawing.Size(158, 219)
        Me.picMagic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picMagic.TabIndex = 4
        Me.picMagic.TabStop = false
        '
        'cmdAssistCancel
        '
        Me.cmdAssistCancel.Location = New System.Drawing.Point(271, 322)
        Me.cmdAssistCancel.Name = "cmdAssistCancel"
        Me.cmdAssistCancel.Size = New System.Drawing.Size(75, 23)
        Me.cmdAssistCancel.TabIndex = 3
        Me.cmdAssistCancel.Text = "Fermer"
        Me.cmdAssistCancel.UseVisualStyleBackColor = true
        AddHandler Me.cmdAssistCancel.Click, AddressOf Me.CmdAssistCancelClick
        '
        'cmdAssistNext
        '
        Me.cmdAssistNext.Location = New System.Drawing.Point(352, 322)
        Me.cmdAssistNext.Name = "cmdAssistNext"
        Me.cmdAssistNext.Size = New System.Drawing.Size(75, 23)
        Me.cmdAssistNext.TabIndex = 2
        Me.cmdAssistNext.Text = "Suivant >"
        Me.cmdAssistNext.UseVisualStyleBackColor = true
        AddHandler Me.cmdAssistNext.Click, AddressOf Me.CmdAssistNextClick
        '
        'lblAssist2
        '
        Me.lblAssist2.Location = New System.Drawing.Point(179, 136)
        Me.lblAssist2.Name = "lblAssist2"
        Me.lblAssist2.Size = New System.Drawing.Size(248, 51)
        Me.lblAssist2.TabIndex = 1
        Me.lblAssist2.Text = "Cliquez sur 'Suivant' pour ajouter de nouvelles éditions à votre logiciel Magic T"& _
        "he Gathering Manager..."
        '
        'lblAssist1
        '
        Me.lblAssist1.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblAssist1.Location = New System.Drawing.Point(176, 19)
        Me.lblAssist1.Name = "lblAssist1"
        Me.lblAssist1.Size = New System.Drawing.Size(263, 102)
        Me.lblAssist1.TabIndex = 0
        Me.lblAssist1.Text = "Bienvenue dans l'assistant d'ajout de nouvelles éditions !"
        '
        'grpHeader
        '
        Me.grpHeader.Controls.Add(Me.chkHeaderAlready)
        Me.grpHeader.Controls.Add(Me.cmdHeaderPrevious)
        Me.grpHeader.Controls.Add(Me.cmdHeaderNext)
        Me.grpHeader.Controls.Add(Me.propEdition)
        Me.grpHeader.Controls.Add(Me.lblHeader)
        Me.grpHeader.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpHeader.Location = New System.Drawing.Point(0, 0)
        Me.grpHeader.Name = "grpHeader"
        Me.grpHeader.Size = New System.Drawing.Size(439, 355)
        Me.grpHeader.TabIndex = 2
        Me.grpHeader.TabStop = false
        Me.grpHeader.Visible = false
        '
        'chkHeaderAlready
        '
        Me.chkHeaderAlready.AutoSize = true
        Me.chkHeaderAlready.Location = New System.Drawing.Point(15, 297)
        Me.chkHeaderAlready.Name = "chkHeaderAlready"
        Me.chkHeaderAlready.Size = New System.Drawing.Size(346, 17)
        Me.chkHeaderAlready.TabIndex = 6
        Me.chkHeaderAlready.Text = "Les caractéristiques de l'édition sont déjà saisies [sauter cette étape]"
        Me.chkHeaderAlready.UseVisualStyleBackColor = true
        AddHandler Me.chkHeaderAlready.CheckedChanged, AddressOf Me.ChkHeaderAlreadyCheckedChanged
        '
        'cmdHeaderPrevious
        '
        Me.cmdHeaderPrevious.Location = New System.Drawing.Point(271, 322)
        Me.cmdHeaderPrevious.Name = "cmdHeaderPrevious"
        Me.cmdHeaderPrevious.Size = New System.Drawing.Size(75, 23)
        Me.cmdHeaderPrevious.TabIndex = 5
        Me.cmdHeaderPrevious.Text = "< Précédent"
        Me.cmdHeaderPrevious.UseVisualStyleBackColor = true
        AddHandler Me.cmdHeaderPrevious.Click, AddressOf Me.CmdHeaderPreviousClick
        '
        'cmdHeaderNext
        '
        Me.cmdHeaderNext.Location = New System.Drawing.Point(352, 322)
        Me.cmdHeaderNext.Name = "cmdHeaderNext"
        Me.cmdHeaderNext.Size = New System.Drawing.Size(75, 23)
        Me.cmdHeaderNext.TabIndex = 4
        Me.cmdHeaderNext.Text = "Suivant >"
        Me.cmdHeaderNext.UseVisualStyleBackColor = true
        AddHandler Me.cmdHeaderNext.Click, AddressOf Me.CmdHeaderNextClick
        '
        'propEdition
        '
        Me.propEdition.Location = New System.Drawing.Point(15, 46)
        Me.propEdition.Name = "propEdition"
        Me.propEdition.Size = New System.Drawing.Size(403, 246)
        Me.propEdition.TabIndex = 2
        '
        'lblHeader
        '
        Me.lblHeader.AutoSize = true
        Me.lblHeader.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblHeader.Location = New System.Drawing.Point(6, 12)
        Me.lblHeader.Name = "lblHeader"
        Me.lblHeader.Size = New System.Drawing.Size(347, 31)
        Me.lblHeader.TabIndex = 0
        Me.lblHeader.Text = "Caractéristiques de l'édition"
        '
        'grpAuto
        '
        Me.grpAuto.Controls.Add(Me.lblStatus)
        Me.grpAuto.Controls.Add(Me.lblAuto2)
        Me.grpAuto.Controls.Add(Me.lblAuto1)
        Me.grpAuto.Controls.Add(Me.chkNewEditionAuto)
        Me.grpAuto.Controls.Add(Me.cmdAutoPrevious)
        Me.grpAuto.Controls.Add(Me.cmdAutoNext)
        Me.grpAuto.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpAuto.Location = New System.Drawing.Point(0, 0)
        Me.grpAuto.Name = "grpAuto"
        Me.grpAuto.Size = New System.Drawing.Size(439, 355)
        Me.grpAuto.TabIndex = 3
        Me.grpAuto.TabStop = false
        Me.grpAuto.Visible = false
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = true
        Me.lblStatus.Location = New System.Drawing.Point(76, 276)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(50, 13)
        Me.lblStatus.TabIndex = 11
        Me.lblStatus.Text = "Attente..."
        '
        'lblAuto2
        '
        Me.lblAuto2.Location = New System.Drawing.Point(76, 71)
        Me.lblAuto2.Name = "lblAuto2"
        Me.lblAuto2.Size = New System.Drawing.Size(270, 50)
        Me.lblAuto2.TabIndex = 10
        Me.lblAuto2.Text = "Les éditions suivantes sont disponibles sur le site de l'éditeur. Choisissez cell"& _
        "e que vous souhaitez inclure à votre base de données :"
        '
        'lblAuto1
        '
        Me.lblAuto1.AutoSize = true
        Me.lblAuto1.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblAuto1.Location = New System.Drawing.Point(6, 12)
        Me.lblAuto1.Name = "lblAuto1"
        Me.lblAuto1.Size = New System.Drawing.Size(254, 31)
        Me.lblAuto1.TabIndex = 9
        Me.lblAuto1.Text = "Editions disponibles"
        '
        'chkNewEditionAuto
        '
        Me.chkNewEditionAuto.CheckOnClick = true
        Me.chkNewEditionAuto.FormattingEnabled = true
        Me.chkNewEditionAuto.Location = New System.Drawing.Point(76, 129)
        Me.chkNewEditionAuto.MultiColumn = true
        Me.chkNewEditionAuto.Name = "chkNewEditionAuto"
        Me.chkNewEditionAuto.Size = New System.Drawing.Size(270, 139)
        Me.chkNewEditionAuto.TabIndex = 8
        AddHandler Me.chkNewEditionAuto.ItemCheck, AddressOf Me.ChkNewEditionItemCheck
        '
        'cmdAutoPrevious
        '
        Me.cmdAutoPrevious.Enabled = false
        Me.cmdAutoPrevious.Location = New System.Drawing.Point(271, 322)
        Me.cmdAutoPrevious.Name = "cmdAutoPrevious"
        Me.cmdAutoPrevious.Size = New System.Drawing.Size(75, 23)
        Me.cmdAutoPrevious.TabIndex = 7
        Me.cmdAutoPrevious.Text = "< Précédent"
        Me.cmdAutoPrevious.UseVisualStyleBackColor = true
        AddHandler Me.cmdAutoPrevious.Click, AddressOf Me.CmdAutoPreviousClick
        '
        'cmdAutoNext
        '
        Me.cmdAutoNext.Enabled = false
        Me.cmdAutoNext.Location = New System.Drawing.Point(352, 322)
        Me.cmdAutoNext.Name = "cmdAutoNext"
        Me.cmdAutoNext.Size = New System.Drawing.Size(75, 23)
        Me.cmdAutoNext.TabIndex = 6
        Me.cmdAutoNext.Text = "Suivant >"
        Me.cmdAutoNext.UseVisualStyleBackColor = true
        AddHandler Me.cmdAutoNext.Click, AddressOf Me.CmdAutoNextClick
        '
        'frmNewEdition
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(439, 355)
        Me.Controls.Add(Me.grpAssist)
        Me.Controls.Add(Me.grpAuto)
        Me.Controls.Add(Me.grpData)
        Me.Controls.Add(Me.grpHeader)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
        Me.MaximizeBox = false
        Me.Name = "frmNewEdition"
        Me.ShowInTaskbar = false
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Ajout d'une nouvelle édition"
        AddHandler Load, AddressOf Me.FrmNewEditionLoad
        Me.grpData.ResumeLayout(false)
        Me.splitH.Panel1.ResumeLayout(false)
        Me.splitH.Panel1.PerformLayout
        Me.splitH.Panel2.ResumeLayout(false)
        Me.splitH.ResumeLayout(false)
        Me.grpAssist.ResumeLayout(false)
        Me.grpAssist.PerformLayout
        CType(Me.picMagic,System.ComponentModel.ISupportInitialize).EndInit
        Me.grpHeader.ResumeLayout(false)
        Me.grpHeader.PerformLayout
        Me.grpAuto.ResumeLayout(false)
        Me.grpAuto.PerformLayout
        Me.ResumeLayout(false)
    End Sub
    Private optUpdate As System.Windows.Forms.RadioButton
    Private lblStatus As System.Windows.Forms.Label
    Private lblData As System.Windows.Forms.Label
    Private chkNewEditionAuto As System.Windows.Forms.CheckedListBox
    Private lblAuto1 As System.Windows.Forms.Label
    Private lblAuto2 As System.Windows.Forms.Label
    Private cmdAutoNext As System.Windows.Forms.Button
    Private cmdAutoPrevious As System.Windows.Forms.Button
    Private lnklblAssist As System.Windows.Forms.LinkLabel
    Private grpAuto As System.Windows.Forms.GroupBox
    Private optAuto As System.Windows.Forms.RadioButton
    Private optManual As System.Windows.Forms.RadioButton
    Private picMagic As System.Windows.Forms.PictureBox
    Private chkHeaderAlready As System.Windows.Forms.CheckBox
    Private lblAssist3 As System.Windows.Forms.Label
    Private cmdHeaderNext As System.Windows.Forms.Button
    Private cmdHeaderPrevious As System.Windows.Forms.Button
    Private lblHeader As System.Windows.Forms.Label
    Private propEdition As System.Windows.Forms.PropertyGrid
    Private lblAssist1 As System.Windows.Forms.Label
    Private lblAssist2 As System.Windows.Forms.Label
    Private cmdAssistNext As System.Windows.Forms.Button
    Private cmdAssistCancel As System.Windows.Forms.Button
    Private grpHeader As System.Windows.Forms.GroupBox
    Private grpAssist As System.Windows.Forms.GroupBox
    Private grpData As System.Windows.Forms.GroupBox
    Private dlgOpen As System.Windows.Forms.OpenFileDialog
    Private cmdOK As System.Windows.Forms.Button
    Private lblCheckList As System.Windows.Forms.Label
    Private lblSpoilerList As System.Windows.Forms.Label
    Private txtCheckList As System.Windows.Forms.TextBox
    Private txtSpoilerList As System.Windows.Forms.TextBox
    Private cmdBrowsetxtCheckList As System.Windows.Forms.Button
    Private cmdBrowsetxtSpoilerList As System.Windows.Forms.Button
    Private chkNewEdition As System.Windows.Forms.CheckedListBox
    Private splitH As System.Windows.Forms.SplitContainer
End Class
