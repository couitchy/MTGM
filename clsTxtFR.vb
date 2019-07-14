Public Class clsTxtFR
    Private VmCard As String
    Private VmTexte As String
    Private VmAlready As eTxtState
    Public Enum eTxtState
        Neww = 0
        Update
        Ok
    End Enum
    Public Sub New (VpCard As String, VpTexte As String)
        VmCard = VpCard
        VmTexte = VpTexte
        VmAlready = eTxtState.Neww
    End Sub
    Public ReadOnly Property CardName As String
        Get
            Return VmCard
        End Get
    End Property
    Public ReadOnly Property Texte As String
        Get
            Return VmTexte
        End Get
    End Property
    Public Property Already As eTxtState
        Get
            Return VmAlready
        End Get
        Set (VpAlready As eTxtState)
            VmAlready = VpAlready
        End Set
    End Property
End Class
