Public Class clsMatchCounter
    Private VmVersus As String
    Private VmCount As Integer
    Private VmProba As Single
    Public Sub New(VpVersus As String, VpCount As Integer)
        VmVersus = VpVersus
        VmCount = VpCount
        VmProba = 0
    End Sub
    Public ReadOnly Property Versus As String
        Get
            Return VmVersus
        End Get
    End Property
    Public ReadOnly Property Count As Integer
        Get
            Return VmCount
        End Get
    End Property
    Public Property Proba As Single
        Get
            Return VmProba
        End Get
        Set (VpProba As Single)
            VmProba = VpProba
        End Set
    End Property
    Public Class clsMatchCounterComparer
        Implements IComparer(Of clsMatchCounter)
        Public Function Compare(ByVal x As clsMatchCounter, ByVal y As clsMatchCounter) As Integer Implements IComparer(Of clsMatchCounter).Compare
            Return x.Proba.CompareTo(y.Proba)
        End Function
    End Class
End Class
