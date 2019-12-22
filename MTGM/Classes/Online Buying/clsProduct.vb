Public Class clsProduct
    Private VmName As String                    'Nom de la carte
    Private VmEditions As List(Of String)       'Editions disponibles
    Private VmSellers As List(Of clsSeller)     'Vendeurs la proposant
    Private VmQuantity As Integer               'Quantité totale
    Private VmPrices As List(Of Single)         'Prix
    Private VmMedianPrice As Single             'Prix médian moyen
    Public Sub New(VpName As String)
        VmName = VpName
        VmEditions = New List(Of String)
        VmSellers = New List(Of clsSeller)
        VmQuantity = 0
        VmPrices = New List(Of Single)
    End Sub
    Public Sub AddOffer(VpRemoteCard As clsRemoteCard)
    Dim VpPos As Integer = 0
        With VpRemoteCard
            If Not VmSellers.Contains(.Seller) Then
                VmSellers.Add(.Seller)
            End If
            If Not VmEditions.Contains(.Edition) Then
                VmEditions.Add(.Edition)
            End If
            VmQuantity += .Quantity
            If VmPrices.Count = 0 OrElse .Price >= VmPrices.Item(VmPrices.Count - 1) Then
                For VpI As Integer = 0 To .Quantity - 1
                    VmPrices.Add(.Price)
                Next VpI
            Else
                For Each VpPrice As Single In VmPrices
                    If VpPrice > .Price Then
                        Exit For
                    End If
                    VpPos += 1
                Next VpPrice
                For VpI As Integer = 0 To .Quantity - 1
                    VmPrices.Insert(VpPos, .Price)
                Next VpI
            End If
        End With
    End Sub
    Public ReadOnly Property Name As String
        Get
            Return VmName
        End Get
    End Property
    Public ReadOnly Property Editions As Integer
        Get
            Return VmEditions.Count
        End Get
    End Property
    Public ReadOnly Property Sellers As Integer
        Get
            Return VmSellers.Count
        End Get
    End Property
    Public ReadOnly Property Quantity As Integer
        Get
            Return VmQuantity
        End Get
    End Property
    Public ReadOnly Property MeanQuantity As Single
        Get
            Return VmQuantity / VmSellers.Count
        End Get
    End Property
    Public ReadOnly Property MedianPrice As Single
        Get
            Return VmPrices(VmPrices.Count \ 2)
        End Get
    End Property
    Public ReadOnly Property OffersCount As Integer
        Get
            Return VmPrices.Count
        End Get
    End Property
End Class
