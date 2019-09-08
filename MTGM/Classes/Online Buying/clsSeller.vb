Imports System.Xml.Serialization
Public Class clsSeller
    Private VmName As String                            'Nom du vendeur
    Private VmCountry As String                         'Code pays
    Private VmCoverage As Integer                       'Nombre total de cartes proposé par ce vendeur qui nous intéressent (non, il n'y a pas de fautes d'orthographe ni de grammaire dans ce commentaire)
    Private VmBought As Integer                         'Quantité de cartes acquises auprès de ce vendeur
    Private VmBoughtValue As Single                     'Montant total des cartes acquises auprès de ce vendeur
    Private VmShippingCost As Single                    'Montant total des frais de port engagés auprès de ce vendeur
    Private VmShippingPolicy As List(Of clsShipping)    'Politique du vendeur en matière de frais de port
    Private VmToSell As List(Of clsRemoteCard)          'Collection des cartes disponibles à la vente pour ce vendeur
    Public Sub New(VpName As String, VpCountry As String, VpCoverage As Integer, VpBought As Integer, VpBoughtValue As Single)
        VmName = VpName
        VmCountry = VpCountry
        VmCoverage = VpCoverage
        VmBought = VpBought
        VmBoughtValue = VpBoughtValue
        VmToSell = New List(Of clsRemoteCard)
    End Sub
    Public Sub New(VpName As String, VpCountry As String)
        Me.New(VpName, VpCountry, 0, 0, 0)
    End Sub
    Public Sub New(VpName As String)
        Me.New(VpName, "FR")
    End Sub
    Public Sub New
        Me.New("")
    End Sub
    Public Function GetMarginalShippingCostFor(VpQuantity As Integer, VpUnitPrice As Single) As Single
    '-------------------------------------------------------------------------------------------------------------
    'Retourne le coût marginal sur les frais de port qui serait engendré par l'acquisition détaillée en paramètres
    '-------------------------------------------------------------------------------------------------------------
        Return clsShipping.GetShippingCostFor(VmBought + VpQuantity, VmBoughtValue + VpUnitPrice * VpQuantity, VmShippingPolicy) - Me.ShippingCost
    End Function
    Public Function GetBoughtCards As List(Of clsRemoteCard)
    '--------------------------------------------------
    'Retourne la liste des cartes achetées à ce vendeur
    '--------------------------------------------------
    Dim VpCards As New List(Of clsRemoteCard)
        For Each VpRemoteCard As clsRemoteCard In VmToSell
            If VpRemoteCard.Bought > 0 Then
                VpCards.Add(VpRemoteCard)
            End If
        Next VpRemoteCard
        Return VpCards
    End Function
    Public Shared Function AddOrGet(VpName As String, VpCountry As String, VpCoverage As Integer, VpBought As Integer, VpBoughtValue As Single, VpSellers As List(Of clsSeller)) As clsSeller
    '--------------------------------------------------------------------------------------------------------------------------------
    'Crée un nouveau vendeur dont le nom est passé en paramètre et retourne sa référence, ou, s'il existe déjà, retourne sa référence
    '--------------------------------------------------------------------------------------------------------------------------------
    Dim VpSeller As clsSeller
        For Each VpSeller In VpSellers
            If VpSeller.Name = VpName Then
                Return VpSeller
            End If
        Next VpSeller
        VpSeller = New clsSeller(VpName, VpCountry, VpCoverage, VpBought, VpBoughtValue)
        VpSellers.Add(VpSeller)
        Return VpSeller
    End Function
    Public Shared Function GetCount(VpSellers As List(Of clsSeller)) As Integer
    '-------------------------------------------------------
    'Retourne le nombre de vendeurs effectivement sollicités
    '-------------------------------------------------------
    Dim VpCount As Integer = 0
        For Each VpSeller As clsSeller In VpSellers
            'If Not VpSeller.Canceled Then
            If VpSeller.Bought > 0 Then
                VpCount += 1
            End If
        Next VpSeller
        Return VpCount
    End Function
    Public Property Name As String
        Get
            Return VmName
        End Get
        Set (VpName As String)
            VmName = VpName
        End Set
    End Property
    Public Property Country As String
        Get
            Return VmCountry
        End Get
        Set (VpCountry As String)
            VmCountry = VpCountry
        End Set
    End Property
    Public Property Coverage As Integer
        Get
            Return VmCoverage
        End Get
        Set (VpCoverage As Integer)
            VmCoverage = VpCoverage
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
    Public Property BoughtValue As Single
        Get
            Return VmBoughtValue
        End Get
        Set (VpBoughtValue As Single)
            VmBoughtValue = VpBoughtValue
        End Set
    End Property
    <XmlIgnore> _
    Public ReadOnly Property ShippingCost As Single
        Get
            Return clsShipping.GetShippingCostFor(VmBought, VmBoughtValue, VmShippingPolicy)
        End Get
    End Property
    <XmlIgnore> _
    Public Property ShippingPolicy As List(Of clsShipping)
        Get
            Return VmShippingPolicy
        End Get
        Set (VpShippingPolicy As List(Of clsShipping))
            VmShippingPolicy = VpShippingPolicy
        End Set
    End Property
    <XmlIgnore> _
    Public ReadOnly Property Cards As List(Of clsRemoteCard)
        Get
            Return VmToSell
        End Get
    End Property
    Public Class clsSellerCoverageComparer
        Implements IComparer(Of clsSeller)
        Public Function Compare(ByVal x As clsSeller, ByVal y As clsSeller) As Integer Implements IComparer(Of clsSeller).Compare
            Return y.Coverage.CompareTo(x.Coverage)
        End Function
    End Class
    Public Class clsSellerBoughtValueComparer
        Implements IComparer(Of clsSeller)
        Public Function Compare(ByVal x As clsSeller, ByVal y As clsSeller) As Integer Implements IComparer(Of clsSeller).Compare
            Return x.BoughtValue.CompareTo(y.BoughtValue)
        End Function
    End Class
End Class
