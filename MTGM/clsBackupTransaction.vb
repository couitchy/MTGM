Public Class clsBackupTransaction
    Private VmBefore As clsRemoteCard
    Private VmAfter As clsRemoteCard
    Private VmN As Integer
    Public Sub New(VpBefore As clsRemoteCard, VpAfter As clsRemoteCard, VpN As Integer)
        VmBefore = VpBefore
        VmAfter = VpAfter
        VmN = VpN
    End Sub
    Public Property Before As clsRemoteCard
        Get
            Return VmBefore
        End Get
        Set (VpBefore As clsRemoteCard)
            VmBefore = VpBefore
        End Set
    End Property
    Public Property After As clsRemoteCard
        Get
            Return VmAfter
        End Get
        Set (VpAfter As clsRemoteCard)
            VmAfter = VpAfter
        End Set
    End Property
    Public Property N As Integer
        Get
            Return VmN
        End Get
        Set (VpN As Integer)
            VmN = VpN
        End Set
    End Property
End Class
