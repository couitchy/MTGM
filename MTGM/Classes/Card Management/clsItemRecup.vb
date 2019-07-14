Public Class clsItemRecup
    Private VmEncNbr As Long
    Private VmQuant As Integer
    Private VmFoil As Boolean
    Public Sub New(VpEncNbr As Long, VpQuant As Integer, VpFoil As Boolean)
        VmEncNbr = VpEncNbr
        VmQuant = VpQuant
        VmFoil = VpFoil
    End Sub
    Public ReadOnly Property EncNbr As Long
        Get
            Return VmEncNbr
        End Get
    End Property
    Public ReadOnly Property Quant As Integer
        Get
            Return VmQuant
        End Get
    End Property
    Public ReadOnly Property Foil As Boolean
        Get
            Return VmFoil
        End Get
    End Property
End Class
