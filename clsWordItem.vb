Public Class clsWordItem
    Private VmTitleVO As String
    Private VmTitle As String
    Private VmCost As String
    Private VmQuant As Integer
    Private VmAD As String
    Private VmFullText As String
    Public Sub New(VpTitle As String, VpTitleVO As String, VpCost As String, VpA As String, VpD As String, VpFullText As String, VpQuant As Integer)
        VmTitle = VpTitle
        VmTitleVO = VpTitleVO
        VmCost = VpCost
        VmQuant = VpQuant
        VmAD = VpA + "/" + VpD
        VmFullText = VpFullText
    End Sub
    Public ReadOnly Property Title As String
        Get
            Return VmTitle
        End Get
    End Property
    Public ReadOnly Property TitleVO As String
        Get
            Return VmTitleVO
        End Get
    End Property
    Public ReadOnly Property Cost As String
        Get
            Return VmCost
        End Get
    End Property
    Public ReadOnly Property AD As String
        Get
            Return If(VmAD = "/", "", VmAD)
        End Get
    End Property
    Public ReadOnly Property FullText As String
        Get
            Return VmFullText
        End Get
    End Property
    Public ReadOnly Property Quant As Integer
        Get
            Return VmQuant
        End Get
    End Property
End Class
