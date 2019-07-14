Public Class clsInfoNode
    Private VmId As Integer
    Private VmFolder As Boolean
    Public Sub New(VpId As Integer, VpFolder As Boolean)
        VmId = VpId
        VmFolder = VpFolder
    End Sub
    Public ReadOnly Property Id As Integer
        Get
            Return VmId
        End Get
    End Property
    Public ReadOnly Property IdString As String
        Get
            Return If(VmId = -1, "Null", VmId.ToString)
        End Get
    End Property
    Public ReadOnly Property IsFolder As Boolean
        Get
            Return VmFolder
        End Get
    End Property
End Class
