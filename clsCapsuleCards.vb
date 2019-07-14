Public Class clsCapsuleCards
    Private VmToBuy As List(Of clsLocalCard)
    Private VmToSell As List(Of clsRemoteCard)
    Public Property ToBuy As List(Of clsLocalCard)
        Get
            Return VmToBuy
        End Get
        Set (VpToBuy As List(Of clsLocalCard))
            VmToBuy = VpToBuy
        End Set
    End Property
    Public Property ToSell As List(Of clsRemoteCard)
        Get
            Return VmToSell
        End Get
        Set (VpToSell As List(Of clsRemoteCard))
            VmToSell = VpToSell
        End Set
    End Property
End Class
