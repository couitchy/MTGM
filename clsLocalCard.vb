Public Class clsLocalCard
    Private VmName As String
    Private VmQuant As Integer
    Private VmDispo As Integer
    Public Sub New(VpName As String, VpQuant As Integer, VpDispo As Integer)
        VmName = VpName
        VmQuant = VpQuant
        VmDispo = VpDispo
    End Sub
    Public Sub New
        VmName = ""
        VmQuant = 0
        VmDispo = 0
    End Sub
    Public Shared Function GetClone(VpA As List(Of clsLocalCard)) As List(Of clsLocalCard)
    '-------------------------------------------
    'Duplication de la liste des cartes désirées
    '-------------------------------------------
    Dim VpB As New List(Of clsLocalCard)
        For Each VpLocalCard As clsLocalCard In VpA
            With VpLocalCard
                If .Quantity > 0 Then
                    VpB.Add(New clsLocalCard(.Name, .Quantity, .Dispo))
                End If
            End With
        Next VpLocalCard
        Return VpB
    End Function
    Public Property Name As String
        Get
            Return VmName
        End Get
        Set (VpName As String)
            VmName = VpName
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
    Public Property Dispo As Integer
        Get
            Return VmDispo
        End Get
        Set (VpDispo As Integer)
            VmDispo = VpDispo
        End Set
    End Property
    Public Class clsLocalCardComparer
        Implements IComparer(Of clsLocalCard)
        Public Function Compare(ByVal x As clsLocalCard, ByVal y As clsLocalCard) As Integer Implements IComparer(Of clsLocalCard).Compare
            Return x.Dispo.CompareTo(y.Dispo)
        End Function
    End Class
End Class
