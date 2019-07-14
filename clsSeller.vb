Public Class clsSeller
    Private VmName As String
    Private VmCoverage As Integer
    Private VmBought As Integer
    Public Sub New(VpName As String, VpCoverage As Integer, VpBought As Integer)
        VmName = VpName
        VmCoverage = VpCoverage
        VmBought = VpBought
    End Sub
    Public Sub New(VpName As String)
        VmName = VpName
        VmCoverage = 0
    End Sub
    Public Sub New
        VmName = ""
        VmCoverage = 0
    End Sub
    Public Function GetBoughtCards(VpToSell As List(Of clsRemoteCard)) As List(Of clsRemoteCard)
    '--------------------------------------------------
    'Retourne la liste des cartes achetées à ce vendeur
    '--------------------------------------------------
    Dim VpBought As New List(Of clsRemoteCard)
        For Each VpRemoteCard As clsRemoteCard In VpToSell
            If VpRemoteCard.Vendeur Is Me Then
                VpBought.Add(VpRemoteCard)
            End If
        Next VpRemoteCard
        Return VpBought
    End Function
    Public Function GetCardsOfInterest(VpToSell As List(Of clsRemoteCard), VpBought As Boolean) As List(Of clsRemoteCard)
    '-------------------------------------------------------------
    'Retourne la liste des cartes achetables/achetées à ce vendeur
    '-------------------------------------------------------------
    Dim VpCards As New List(Of clsRemoteCard)
        For Each VpRemoteCard As clsRemoteCard In VpToSell
            If VpRemoteCard.Vendeur Is Me And (VpRemoteCard.Bought > 0 Or Not VpBought) Then
                VpCards.Add(VpRemoteCard)
            End If
        Next VpRemoteCard
        Return VpCards
    End Function
    Public Shared Function AddOrGet(VpName As String, VpCoverage As Integer, VpBought As Integer, VpSellers As List(Of clsSeller)) As clsSeller
    '--------------------------------------------------------------------------------------------------------------------------------
    'Crée un nouveau vendeur dont le nom est passé en paramètre et retourne sa référence, ou, s'il existe déjà, retourne sa référence
    '--------------------------------------------------------------------------------------------------------------------------------
    Dim VpSeller As clsSeller
        For Each VpSeller In VpSellers
            If VpSeller.Name = VpName Then
                Return VpSeller
            End If
        Next VpSeller
        VpSeller = New clsSeller(VpName, VpCoverage, VpBought)
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
    Public Class clsSellerComparer
        Implements IComparer(Of clsSeller)
        Public Function Compare(ByVal x As clsSeller, ByVal y As clsSeller) As Integer Implements IComparer(Of clsSeller).Compare
            Return x.Coverage.CompareTo(y.Coverage)
        End Function
    End Class
End Class
