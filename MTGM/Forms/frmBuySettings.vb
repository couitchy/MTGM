Public Partial Class frmBuySettings
    Private VmOwner As frmBuyCards
    Public Sub New(VpCard As String, VpOwner As frmBuyCards, VpProducts As List(Of clsProductRequest.clsProduct))
        Call Me.InitializeComponent
        Me.lblTitle.Text = VpCard
        VmOwner = VpOwner
        If VpProducts IsNot Nothing Then
            For Each VpProduct As clsProductRequest.clsProduct In VpProducts
                If VpProduct IsNot Nothing AndAlso VpProduct.expansion IsNot Nothing AndAlso Not Me.chklstEditions.Items.Contains(VpProduct.expansion) Then
                    Me.chklstEditions.Items.Add(VpProduct.expansion)
                End If
            Next VpProduct
        End If
        Me.chklstEditions.Sorted = True
    End Sub
    Private Function GetCheckedItems As List(Of String)
    Dim VpEditions As New List(Of String)
        For Each VpEdition As String In Me.chklstEditions.CheckedItems
            VpEditions.Add(VpEdition)
        Next VpEdition
        Return VpEditions
    End Function
    Sub BtRefreshClick(sender As Object, e As EventArgs)
        VmOwner.Editions = Me.GetCheckedItems
        Me.Close
    End Sub
    Sub frmBuySettingsKeyUp(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Escape Then
            VmOwner.Editions = Me.GetCheckedItems
            Me.Close
        End If
    End Sub
    Sub ChklstClassementItemCheck(sender As Object, e As ItemCheckEventArgs)
        If e.CurrentValue = CheckState.Indeterminate Then
            e.NewValue = CheckState.Indeterminate
        End If
    End Sub
End Class
