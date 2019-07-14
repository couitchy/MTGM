Public Partial Class frmExploSettings
    Private VmOwner As MainForm
    Public Sub New(VpOwner As MainForm)
        Call Me.InitializeComponent
        VmOwner = VpOwner
    End Sub
    Public Sub ValidateCriteria
    '---------------------------------------------------
    'Coche la liste des critères sélectionnés par défaut
    '---------------------------------------------------
    Dim VpCriteria() As String = VgOptions.VgSettings.DefaultActivatedCriteria.Split("#")
        For Each VpCriterion As String In VpCriteria
            Try
                Me.chklstClassement.SetItemChecked(CInt(VpCriterion), True)
            Catch
            End Try
        Next VpCriterion
    End Sub
    Private Sub ManageOrder(VpX As Integer, VpY As Integer, VpZ As Integer)
    Dim VpIndex As Integer = Me.chklstClassement.SelectedIndex
    Dim VpChecked As Boolean = Me.chklstClassement.GetItemChecked(VpIndex)
        Me.chklstClassement.Items.Insert(VpIndex + VpZ, Me.chklstClassement.SelectedItem)
        Me.chklstClassement.Items.RemoveAt(VpIndex + VpX + VpY)
        Me.chklstClassement.SetItemChecked(VpIndex - VpX, VpChecked)
        Me.chklstClassement.SelectedIndex = VpIndex - Vpx
    End Sub
    Sub ChklstClassementSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Me.btUp.Enabled = ( Me.chklstClassement.SelectedIndex <> 0 And Me.chklstClassement.SelectedIndex <> Me.chklstClassement.Items.Count - 1 )
        Me.btDown.Enabled = ( Me.chklstClassement.SelectedIndex < Me.chklstClassement.Items.Count - 2 )
    End Sub
    Sub BtRefreshClick(sender As Object, e As EventArgs)
        Me.Hide
        Call VmOwner.MyRefresh
    End Sub
    Sub FrmExploSettingsKeyUp(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Escape Then
            Me.Hide
        End If
    End Sub
    Sub FrmExploSettingsFormClosing(sender As Object, e As FormClosingEventArgs)
        If e.CloseReason = CloseReason.UserClosing Then
            e.Cancel = True
            Me.Hide
        End If
    End Sub
    Sub FrmExploSettingsLoad(sender As Object, e As EventArgs)
        Me.chklstClassement.SetItemCheckState(Me.chklstClassement.Items.Count - 1, CheckState.Indeterminate)
    End Sub
    Sub ChklstClassementItemCheck(sender As Object, e As ItemCheckEventArgs)
        If e.CurrentValue = CheckState.Indeterminate Then
            e.NewValue = CheckState.Indeterminate
        End If
    End Sub
    Sub BtUpClick(sender As Object, e As EventArgs)
        Call Me.ManageOrder(1, 0, -1)
    End Sub
    Sub BtDownClick(sender As Object, e As EventArgs)
        Call Me.ManageOrder(-1, 1, 2)
    End Sub
    Public ReadOnly Property MyList As CheckedListBox
        Get
            Return Me.chklstClassement
        End Get
    End Property
    Public ReadOnly Property NCriteria As Integer
        Get
            Return Me.chklstClassement.Items.Count
        End Get
    End Property
    Public ReadOnly Property NSelectedCriteria As Integer
        Get
            Return Me.chklstClassement.CheckedItems.Count
        End Get
    End Property
End Class
