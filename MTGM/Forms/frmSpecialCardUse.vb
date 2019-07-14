Public Partial Class frmSpecialCardUse
    Private VmFormMove As Boolean = False   'Formulaire en déplacement
    Private VmMousePos As Point             'Position initiale de la souris sur la barre de titre
    Private VmCanClose As Boolean = False   'Formulaire peut être fermé
    Private VmHelp As ToolTip               'Info-bulles d'aide à la saisie
    Public Sub New(VpSource As String, VpRestriction As String)
    Dim VpPartie As clsSimulGame
        Call Me.InitializeComponent
        VpPartie = New clsSimulGame(VpSource, VpRestriction)
        For Each VpCard As clsCard In VpPartie.CardsInFullDeck
            If Not Me.cboCard.Items.Contains(VpCard.CardName) Then
                Me.cboCard.Items.Add(VpCard.CardName)
            End If
        Next VpCard
        Call Me.LoadSpecials
        Me.cboCard.SelectedIndex = 0
    End Sub
    Private Sub LoadSpecials
        VgDBCommand.CommandText = "Select Description, IsEffort From SpecialUse Order By Description;"
        VgDBReader = VgDBCommand.ExecuteReader
        With VgDBReader
            While .Read
                If .GetBoolean(1) Then
                    Me.cboEffort.Items.Add(.GetString(0))
                Else
                    Me.cboEffet.Items.Add(.GetString(0))
                End If
            End While
            .Close
        End With
    End Sub
    Private Sub CbarSpecialCardMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
        VmFormMove = True
        VmMousePos = New Point(e.X, e.Y)
    End Sub
    Private Sub CbarSpecialCardMouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
        If VmFormMove Then
            Me.Location = New Point(MousePosition.X - VmMousePos.X, MousePosition.Y - VmMousePos.Y)
        End If
    End Sub
    Private Sub CbarSpecialCardMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs)
        VmFormMove = False
    End Sub
    Private Sub CbarSpecialCardVisibleChanged(ByVal sender As Object, ByVal e As EventArgs)
        If VmCanClose AndAlso Not Me.cbarSpecialCard.Visible Then
            Me.Close
        End If
    End Sub
    Sub ChkDefineSpecialCheckedChanged(sender As Object, e As EventArgs)
        Me.lbl1.Enabled = Me.chkDefineSpecial.Checked
        Me.lbl11.Enabled = Me.chkDefineSpecial.Checked
        Me.lbl2.Enabled = Me.chkDefineSpecial.Checked
        Me.lbl21.Enabled = Me.chkDefineSpecial.Checked
        Me.cboEffet.Enabled = Me.chkDefineSpecial.Checked
        Me.cboEffort.Enabled = Me.chkDefineSpecial.Checked
        Me.txtEffet.Enabled = Me.chkDefineSpecial.Checked
        Me.txtEffort.Enabled = Me.chkDefineSpecial.Checked
        Me.chkInvocTapped.Enabled = Me.chkDefineSpecial.Checked
        Me.chkDoesntUntap.Enabled = Me.chkDefineSpecial.Checked
    End Sub
    Sub CmdSaveClick(sender As Object, e As EventArgs)
        VgDBCommand.CommandText = "Delete * From MySpecialUses Where Card = '" + Me.cboCard.Text.Replace("'", "''") + "';"
        VgDBCommand.ExecuteNonQuery
        If Me.chkDefineSpecial.Checked And Me.cboEffet.Text <> "" And Me.cboEffort.Text <> "" Then
            VgDBCommand.CommandText = "Insert Into MySpecialUses Values (" + clsSpecialty.GetSpecId(Me.cboEffort.Text).ToString + ", " + clsSpecialty.GetSpecId(Me.cboEffet.Text).ToString + ", '" + Me.cboCard.Text.Replace("'", "''") + "', '" + Me.txtEffort.Text + "', '" + Me.txtEffet.Text + "', " + Me.chkInvocTapped.Checked.ToString + ", " + Me.chkDoesntUntap.Checked.ToString + ");"
            Try
                VgDBCommand.ExecuteNonQuery
                Call clsModule.ShowInformation("Cet effet sera pris en compte dès la prochaine simulation.")
            Catch
                Call clsModule.ShowWarning(clsModule.CgErr1)
            End Try
        Else
            Call clsModule.ShowWarning("L'effet a été défini de manière incomplète...")
        End If
    End Sub
    Sub CboCardSelectedIndexChanged(sender As Object, e As EventArgs)
    Dim VpSpeciality As New clsSpecialty(Me.cboCard.Text)
        If VpSpeciality.IsSpecial Then
            Me.chkDefineSpecial.Checked = True
            Me.txtEffort.Text = VpSpeciality.Effort
            Me.txtEffet.Text = VpSpeciality.Effet
            Me.cboEffort.Text = clsSpecialty.GetSpecTxt(VpSpeciality.EffortID)
            Me.cboEffet.Text = clsSpecialty.GetSpecTxt(VpSpeciality.EffetID)
            Me.chkInvocTapped.Checked = VpSpeciality.InvocTapped
            Me.chkDoesntUntap.Checked = VpSpeciality.DoesntUntap
        Else
            Me.chkDefineSpecial.Checked = False
        End If
    End Sub
    Sub TxtEffortEnter(ByVal sender As Object, ByVal e As EventArgs)
        VmHelp = New ToolTip
        VmHelp.Show(clsSpecialty.GetSpecHlp(clsSpecialty.GetSpecId(Me.cboEffort.Text)), Me, Me.txtEffort.Left, Me.txtEffort.Top + Me.grpUse.Top - Me.txtEffort.Height)
    End Sub
    Sub TxtEffetEnter(ByVal sender As Object, ByVal e As EventArgs)
        VmHelp = New ToolTip
        VmHelp.Show(clsSpecialty.GetSpecHlp(clsSpecialty.GetSpecId(Me.cboEffet.Text)), Me, Me.txtEffet.Left, Me.txtEffet.Top + Me.grpUse.Top - Me.txtEffet.Height)
    End Sub
    Sub TxtEffortLeave(ByVal sender As Object, ByVal e As EventArgs)
        VmHelp.RemoveAll
    End Sub
    Sub TxtEffetLeave(ByVal sender As Object, ByVal e As EventArgs)
        VmHelp.RemoveAll
    End Sub
    Sub CmdCancelClick(sender As Object, e As EventArgs)
        Me.Close
    End Sub
    Sub FrmSpecialCardUseLoad(sender As Object, e As EventArgs)
        VmCanClose = True
    End Sub
End Class
