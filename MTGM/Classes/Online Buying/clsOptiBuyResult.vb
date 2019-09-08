Public Class clsOptiBuyResult
    Private VmWarning As Boolean        'Un avertissement a été affiché
    Private VmBought As Integer         'Nombre de carte achetées
    Private VmSellersCount As Integer   'Nombre de vendeurs sollicités
    Private VmCardsCost As Single       'Montant total des cartes
    Private VmShippingCost As Single    'Montant total des frais de port
    Public Sub New
        VmWarning = False
        VmBought = 0
        VmSellersCount = 0
        VmCardsCost = 0
        VmShippingCost = 0
    End Sub
    Public Property Warning As Boolean
        Get
            Return VmWarning
        End Get
        Set (VpWarning As Boolean)
            VmWarning = VpWarning
        End Set
    End Property
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
End Class
