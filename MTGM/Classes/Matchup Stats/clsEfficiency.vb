Public Class clsEfficiency
    Public Class clsEfficiencyComparer
        Implements IComparer(Of clsEfficiency)
        Public Function Compare(ByVal x As clsEfficiency, ByVal y As clsEfficiency) As Integer Implements IComparer(Of clsEfficiency).Compare
            If x.Efficiency > y.Efficiency Then
                Return 1
            ElseIf x.Efficiency < y.Efficiency Then
                Return -1
            Else
                Return 0
            End If
        End Function
    End Class
    Private VmName As String
    Private VmPrice As Single
    Private VmPerfs1 As Single
    Private VmPerfs2 As Single
    Private VmEspPrice As Single
    Private VmEspPerfs As Single
    Private VmEfficiency As Single
    Public Sub New(VpName As String, VpPrice As Single, VpPerfs1 As Single, VpPerfs2 As Single, VpEspPrice As Single, VpEspPerfs As Single, VpEfficiency As Single)
        VmName = VpName
        VmPrice = VpPrice
        VmPerfs1 = VpPerfs1
        VmPerfs2 = VpPerfs2
        VmEspPrice = VpEspPrice
        VmEspPerfs = VpEspPerfs
        VmEfficiency = VpEfficiency
    End Sub
    Public ReadOnly Property Name As String
        Get
            Return VmName
        End Get
    End Property
    Public ReadOnly Property Price As Single
        Get
            Return VmPrice
        End Get
    End Property
    Public ReadOnly Property Perfs1 As Single
        Get
            Return VmPerfs1
        End Get
    End Property
    Public ReadOnly Property Perfs2 As Single
        Get
            Return VmPerfs2
        End Get
    End Property
    Public ReadOnly Property EspPrice As Single
        Get
            Return VmEspPrice
        End Get
    End Property
    Public ReadOnly Property EspPerfs As Single
        Get
            Return VmEspPerfs
        End Get
    End Property
    Public ReadOnly Property Efficiency As Single
        Get
            Return VmEfficiency
        End Get
    End Property
End Class
