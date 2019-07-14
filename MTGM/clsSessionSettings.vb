Public Class clsSessionSettings
    Private VmFreeTempFileIndex As Integer = -1
    Private VmGridClearing As Boolean = False
    Private VmSplitterDistance As Integer
    Private VmFormSubWidth As Integer
    Public Property FreeTempFileIndex As Integer
        Get
            Return VmFreeTempFileIndex
        End Get
        Set (VpFreeTempFileIndex As Integer)
            VmFreeTempFileIndex = VpFreeTempFileIndex
        End Set
    End Property
    Public Property GridClearing As Boolean
        Get
            Return VmGridClearing
        End Get
        Set (VpGridClearing As Boolean)
            VmGridClearing = VpGridClearing
        End Set
    End Property
    Public Property SplitterDistance As Integer
        Get
            Return VmSplitterDistance
        End Get
        Set (VpSplitterDistance As Integer)
            VmSplitterDistance = VpSplitterDistance
        End Set
    End Property
    Public Property FormSubWidth As Integer
        Get
            Return VmFormSubWidth
        End Get
        Set (VpFormSubWidth As Integer)
            VmFormSubWidth = VpFormSubWidth
        End Set
    End Property
End Class
