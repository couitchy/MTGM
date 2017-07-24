'------------------------------------------------------
'| Projet         |  Magic The Gathering Manager      |
'| Contexte       |  		Perso                     |
'| Date           |                        30/03/2008 |
'| Release 1      |                        12/04/2008 |
'| Release 2      |                        30/08/2008 |
'| Release 3      |                        08/11/2008 |
'| Release 4      |                        29/08/2009 |
'| Release 5      |                        21/03/2010 |
'| Release 6      |                        17/04/2010 |
'| Release 7      |                        29/07/2010 |
'| Release 8      |                        03/10/2010 |
'| Release 9      |                        05/02/2011 |
'| Release 10     |                        10/09/2011 |
'| Release 11     |                        24/01/2012 |
'| Release 12     |                        01/10/2012 |
'| Release 13     |                        09/05/2014 |
'| Release 14     |                        09/05/2015 |
'| Release 15     |                        15/01/2017 |
'| Auteur         |                          Couitchy |
'|----------------------------------------------------|
'| Modifications :                                    |
'------------------------------------------------------
Public Partial Class frmBuySettings
	Private VmOwner As frmBuyCards
	Public Sub New(VpCard As String, VpOwner As frmBuyCards, VpProducts As List(Of clsProductRequest.clsProduct))
		Me.InitializeComponent()
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