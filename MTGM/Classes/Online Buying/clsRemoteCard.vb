Imports System.Xml.Serialization
Public Class clsRemoteCard
    Private VmName As String                        'Nom de la carte à vendre
    Private VmSeller As clsSeller                   'Nom du vendeur
    Private VmEdition As String                     'Edition dans laquelle la carte est imprimée (l'utilisateur a pu éventuellement en indiquer plusieurs comme lui convenant)
    Private VmLanguage As String                    'Langue dans laquelle la carte est imprimée (VF ou VO)
    Private VmCondition As mdlConstGlob.eQuality    'Etat dans lequel se trouve la carte (bonne ou mauvaise qualité)
    Private VmQuantity As Integer                   'Nombre d'exemplaires disponibles à la vente
    Private VmBought As Integer                     'Nombre d'exemplaires achetés
    Private VmPrice As Single                       'Prix de vente de la carte
    Private VmShippingCost As Single                'Montant des frais de porte pour cette carte comme si elle était achetée seule
    Public Sub New(VpName As String, VpSeller As clsSeller, VpEdition As String, VpLanguage As String, VpCondition As mdlConstGlob.eQuality, VpQuantity As Integer, VpBought As Integer, VpPrice As Single, VpShippingCost As Single)
        VmName = VpName
        VmSeller = VpSeller
        VmEdition = VpEdition
        VmLanguage = VpLanguage
        VmCondition = VpCondition
        VmQuantity = VpQuantity
        VmBought = VpBought
        VmPrice = VpPrice
        VmShippingCost = VpShippingCost
    End Sub
    Public Sub New(VpName As String)
        Me.New(VpName, New clsSeller, "", "", mdlConstGlob.eQuality.Mint, 0, 0, 0, 0)
    End Sub
    Public Sub New
        Me.New("")
    End Sub
    Public Shared Function GetClone(VpA As List(Of clsRemoteCard)) As List(Of clsRemoteCard)
    '-------------------------------------------
    'Duplication de la liste des cartes en vente
    '-------------------------------------------
    Dim VpB As New List(Of clsRemoteCard)
    Dim VpSellers As New List(Of clsSeller)
        For Each VpRemoteCard As clsRemoteCard In VpA
            With VpRemoteCard
                VpB.Add(New clsRemoteCard(.Name, clsSeller.AddOrGet(.Seller.Name, .Seller.Country, .Seller.Coverage, .Seller.Bought, .Seller.BoughtValue, VpSellers) , .Edition, .Language, .Condition, .Quantity, .Bought, .Price, .ShippingCost))
            End With
        Next VpRemoteCard
        Return VpB
    End Function
    Public Shared Function GetTotal(VpToSell As List(Of clsRemoteCard)) As Single
    '---------------------------------------------------------------------------------------------
    'Retourne le total des coûts d'acquisition (hors frais de port) auprès des vendeurs sollicités
    '---------------------------------------------------------------------------------------------
    Dim VpTotal As Single = 0
        For Each VpRemoteCard As clsRemoteCard In VpToSell
            VpTotal += VpRemoteCard.Bought * VpRemoteCard.Price
        Next VpRemoteCard
        Return VpTotal
    End Function
    Public Shared Function GetSubTotal(VpToSell As List(Of clsRemoteCard), VpSeller As clsSeller) As Single
    '-------------------------------------------------------------------------------------------------------------
    'Retourne le total des coûts d'acquisition (hors frais de port) auprès du vendeur sollicité passé en paramètre
    '-------------------------------------------------------------------------------------------------------------
    Dim VpTotal As Single = 0
        For Each VpRemoteCard As clsRemoteCard In VpToSell
            If VpRemoteCard.Seller Is VpSeller Then
                VpTotal += VpRemoteCard.Bought * VpRemoteCard.Price
            End If
        Next VpRemoteCard
        Return VpTotal
    End Function
    Public Property Name As String
        Get
            Return VmName
        End Get
        Set (VpName As String)
            VmName = VpName
        End Set
    End Property
    Public Property Seller As clsSeller
        Get
            Return VmSeller
        End Get
        Set (VpSeller As clsSeller)
            VmSeller = VpSeller
        End Set
    End Property
    Public Property Edition As String
        Get
            Return VmEdition
        End Get
        Set (VpEdition As String)
            VmEdition = VpEdition
        End Set
    End Property
    Public Property Language As String
        Get
            Return VmLanguage
        End Get
        Set (VpLanguage As String)
            VmLanguage = VpLanguage
        End Set
    End Property
    Public Property Condition As mdlConstGlob.eQuality
        Get
            Return VmCondition
        End Get
        Set (VpCondition As mdlConstGlob.eQuality)
            VmCondition = VpCondition
        End Set
    End Property
    Public Property Quantity As Integer
        Get
            Return VmQuantity
        End Get
        Set (VpQuantity As Integer)
            VmQuantity = VpQuantity
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
    Public Property Price As Single
        Get
            Return VmPrice
        End Get
        Set (VpPrice As Single)
            VmPrice = VpPrice
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
    <XmlIgnore> _
    Public ReadOnly Property PriceWithShipping As Single
        Get
            Return VmPrice + VmShippingCost
        End Get
    End Property
    Public Class clsRemoteCardComparer
        Implements IComparer(Of clsRemoteCard)
        Public Function Compare(ByVal x As clsRemoteCard, ByVal y As clsRemoteCard) As Integer Implements IComparer(Of clsRemoteCard).Compare
            'Si 2 cartes sont au même prix, on favorise le vendeur qui a la plus grande couverture
            If Math.Abs(x.PriceWithShipping - y.PriceWithShipping) <= 0.1 Then  'pour simplifier, jusqu'à 10 centimes d'écart, on considère que c'est le même prix
                Return y.Seller.Coverage.CompareTo(x.Seller.Coverage)
            Else
                Return x.PriceWithShipping.CompareTo(y.PriceWithShipping)
            End If
        End Function
    End Class
End Class
