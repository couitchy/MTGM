Public Class clsLocalCard
    Private VmName As String            'Nom de la carte à acquérir (remarque : l'édition n'a pas d'importance puisque l'utilisateur a déjà coché celle(s) qui lui convenai(en)t lors de l'actualisation des offres
    Private VmQuantity As Integer       'Nombre d'exemplaires à acquérir
    Private VmAvailability As Integer   'Disponibilité de la carte (nombre total d'exemplaires disponibles à la vente parmi tous les vendeurs)
    Public Sub New(VpName As String, VpQuantity As Integer, VpAvailability As Integer)
        VmName = VpName
        VmQuantity = VpQuantity
        VmAvailability = VpAvailability
    End Sub
    Public Sub New
        Me.New("", 0, 0)
    End Sub
    Public Shared Function GetClone(VpA As List(Of clsLocalCard)) As List(Of clsLocalCard)
    '-------------------------------------------
    'Duplication de la liste des cartes désirées
    '-------------------------------------------
    Dim VpB As New List(Of clsLocalCard)
        For Each VpLocalCard As clsLocalCard In VpA
            With VpLocalCard
                If .Quantity > 0 Then
                    VpB.Add(New clsLocalCard(.Name, .Quantity, .Availability))
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
            Return VmQuantity
        End Get
        Set (VpQuantity As Integer)
            VmQuantity = VpQuantity
        End Set
    End Property
    Public Property Availability As Integer
        Get
            Return VmAvailability
        End Get
        Set (VpAvailability As Integer)
            VmAvailability = VpAvailability
        End Set
    End Property
    Public Class clsLocalCardComparer
        Implements IComparer(Of clsLocalCard)
        Public Function Compare(ByVal x As clsLocalCard, ByVal y As clsLocalCard) As Integer Implements IComparer(Of clsLocalCard).Compare
            Return x.Availability.CompareTo(y.Availability)
        End Function
    End Class
End Class
