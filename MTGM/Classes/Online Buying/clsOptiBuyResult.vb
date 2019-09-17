Public Class clsOptiBuyResult
    Private VmBought As Integer                     'Nombre de carte achetées
    Private VmSellersCount As Integer               'Nombre de vendeurs sollicités
    Private VmCardsCost As Single                   'Montant total des cartes
    Private VmShippingCost As Single                'Montant total des frais de port
    Private VmSmallSellers As List(Of clsSeller)    'Vendeur pour lesquels le montant minimal demandé pour les transactions n'est pas atteint
    Public Sub New
        VmBought = 0
        VmSellersCount = 0
        VmCardsCost = 0
        VmShippingCost = 0
        VmSmallSellers = New List(Of clsSeller)
    End Sub
    Public Sub AddSmallTransaction(VpSeller As clsSeller)
        If Not VmSmallSellers.Contains(VpSeller) Then
            VmSmallSellers.Add(VpSeller)
        End If
    End Sub
    Public Property Bought As Integer
        Get
            Return VmBought
        End Get
        Set (VpBought As Integer)
            VmBought = VpBought
        End Set
    End Property
    Public Property SellersCount As Integer
        Get
            Return VmSellersCount
        End Get
        Set (VpSellersCount As Integer)
            VmSellersCount = VpSellersCount
        End Set
    End Property
    Public Property CardsCost As Single
        Get
            Return VmCardsCost
        End Get
        Set (VpCardsCost As Single)
            VmCardsCost = VpCardsCost
        End Set
    End Property
    Public Property ShippingCost As Single
        Get
            Return VmShippingCost
        End Get
        Set (VpShippingCost As Single)
            VmShippingCost = VpShippingCost
        End Set
    End Property
    Public ReadOnly Property GrandTotal As Single
        Get
            Return VmCardsCost + VmShippingCost
        End Get
    End Property
    Public ReadOnly Property Warning(VpMinParcelValue As Single) As String
        Get
            Select Case VmSmallSellers.Count
                Case 0
                    Return ""
                Case 1
                    Return "Il reste malheureusement 1 transaction à moins de " + VpMinParcelValue.ToString + "€" + vbCrLf
                Case Else
                    Return "Il reste malheureusement " + VmSmallSellers.Count.ToString + " transactions à moins de " + VpMinParcelValue.ToString + "€" + vbCrLf
            End Select
        End Get
    End Property
End Class
