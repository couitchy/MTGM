Public Class clsRemoteCard
    Private VmName As String
    Private VmVendeur As clsSeller
    Private VmEdition As String
    Private VmLanguage As String
    Private VmEtat As clsModule.eQuality
    Private VmQuant As Integer
    Private VmBought As Integer
    Private VmPrix As Single
    Public Sub New(VpName As String, VpVendeur As clsSeller, VpEdition As String, VpLanguage As String, VpEtat As clsModule.eQuality, VpQuant As Integer, VpBought As Integer, VpPrix As Single)
        VmName = VpName
        VmVendeur = VpVendeur
        VmEdition = VpEdition
        VmLanguage = VpLanguage
        VmEtat = VpEtat
        VmQuant = VpQuant
        VmBought = VpBought
        VmPrix = VpPrix
    End Sub
    Public Sub New(VpName As String)
        Me.New(VpName, New clsSeller, "", "", clsModule.eQuality.Mint, 0, 0, 0)
    End Sub
    Public Sub New
        Me.New("", New clsSeller, "", "", clsModule.eQuality.Mint, 0, 0, 0)
    End Sub
    Public Shared Function GetClone(VpA As List(Of clsRemoteCard)) As List(Of clsRemoteCard)
    '-------------------------------------------
    'Duplication de la liste des cartes en vente
    '-------------------------------------------
    Dim VpB As New List(Of clsRemoteCard)
    Dim VpSellers As New List(Of clsSeller)
        For Each VpRemoteCard As clsRemoteCard In VpA
            With VpRemoteCard
                VpB.Add(New clsRemoteCard(.Name, clsSeller.AddOrGet(.Vendeur.Name, .Vendeur.Coverage, .Vendeur.Bought, VpSellers) , .Edition, .Language, .Etat, .Quantity, .Bought, .Prix))
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
            VpTotal += VpRemoteCard.Bought * VpRemoteCard.Prix
        Next VpRemoteCard
        Return VpTotal
    End Function
    Public Shared Function GetSubTotal(VpToSell As List(Of clsRemoteCard), VpSeller As clsSeller) As Single
    '-------------------------------------------------------------------------------------------------------------
    'Retourne le total des coûts d'acquisition (hors frais de port) auprès du vendeur sollicité passé en paramètre
    '-------------------------------------------------------------------------------------------------------------
    Dim VpTotal As Single = 0
        For Each VpRemoteCard As clsRemoteCard In VpToSell
            If VpRemoteCard.Vendeur Is VpSeller Then
                VpTotal += VpRemoteCard.Bought * VpRemoteCard.Prix
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
    Public Property Vendeur As clsSeller
        Get
            Return VmVendeur
        End Get
        Set (VpVendeur As clsSeller)
            VmVendeur = VpVendeur
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
    Public Property Etat As clsModule.eQuality
        Get
            Return VmEtat
        End Get
        Set (VpEtat As clsModule.eQuality)
            VmEtat = VpEtat
        End Set
    End Property
    Public Property Quantity As Integer
        Get
            Return VmQuant
        End Get
        Set (VpQuant As Integer)
            VmQuant = VpQuant
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
    Public Property Prix As Single
        Get
            Return VmPrix
        End Get
        Set (VpPrix As Single)
            VmPrix = VpPrix
        End Set
    End Property
    Public Class clsRemoteCardComparer
        Implements IComparer(Of clsRemoteCard)
        Public Function Compare(ByVal x As clsRemoteCard, ByVal y As clsRemoteCard) As Integer Implements IComparer(Of clsRemoteCard).Compare
            'Si 2 cartes sont au même prix, on favorise le vendeur qui a la plus grande couverture
            If Math.Abs(x.Prix - y.Prix) <= 0.1 Then
                Return y.Vendeur.Coverage.CompareTo(x.Vendeur.Coverage)
            Else
                Return x.Prix.CompareTo(y.Prix)
            End If
        End Function
    End Class
End Class
