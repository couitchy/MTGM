Public Class clsDistribution
    Private VmNameVF As String
    Private VmDeckCount As Integer
    Private VmSideCount As Integer
    Public Sub New(VpNameVF As String, VpReserve As Boolean)
        VmNameVF = VpNameVF
        VmDeckCount = If(VpReserve, 0, 1)
        VmSideCount = If(VpReserve, 1, 0)
    End Sub
    Public Property DeckCount As Integer
        Get
            Return VmDeckCount
        End Get
        Set (VpDeckCount As Integer)
            VmDeckCount = VpDeckCount
        End Set
    End Property
    Public Property SideCount As Integer
        Get
            Return VmSideCount
        End Get
        Set (VpSideCount As Integer)
            VmSideCount = VpSideCount
        End Set
    End Property
    Public ReadOnly Property NameVF As String
        Get
            Return VmNameVF
        End Get
    End Property
End Class
