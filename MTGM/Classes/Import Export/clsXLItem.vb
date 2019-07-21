Public Class clsXLItem
    Private VmTitle As String
    Private VmFoil As Boolean = False
    Private VmQuant As Integer
    Private VmColor As String = ""
    Private VmForceEndurance As String = ""
    Private VmInvoc As String = ""
    Private VmSerie As String = ""
    Private VmPrice As String = 0
    Private VmRarity As String = ""
    Private VmSubType As String = ""
    Private VmType As String = ""
    Private VmCardText As String = ""
    Private VmReserve As Boolean
    Public Sub New(VpChk As CheckedListBox, VpTitle As String, VpQuant As Integer, VpFoil As Boolean, VpColor As String, VpForce As String, VpEndurance As String, VpInvoc As String, VpSerie As String, VpPrice As String, VpFoilPrice As String, VpRarity As String, VpSubType As String, VpType As String, VpCardText As String)
        VmTitle = VpTitle
        VmQuant = VpQuant
        'Foil ou pas
        If VpChk.GetItemChecked(0) Then
            VmFoil = VpFoil
        End If
        'Type
        If VpChk.GetItemChecked(8) Then
            VmType = mdlToolbox.FormatTitle("Card.Type", VpType)
        End If
        'Sous-type
        If VpChk.GetItemChecked(7) Then
            VmSubType = VpSubType
        End If
        'Couleur
        If VpChk.GetItemChecked(1) Then
            VmColor = mdlToolbox.FormatTitle("Spell.Color", VpColor)
        End If
        'Force / Endurance
        If VpChk.GetItemChecked(2) Then
            VmForceEndurance = If(VpForce = "" And VpEndurance = "", "", "'" + VpForce + " / " + VpEndurance)
        End If
        'Coût d'invocation
        If VpChk.GetItemChecked(3) Then
            VmInvoc = VpInvoc
        End If
        'Edition
        If VpChk.GetItemChecked(4) Then
            VmSerie = VpSerie
        End If
        'Prix
        If VpChk.GetItemChecked(5) Then
            VmPrice = If(VpFoil, VpFoilPrice, VpPrice)
        End If
        'Rareté
        If VpChk.GetItemChecked(6) Then
            VmRarity = mdlToolbox.FormatTitle("Card.Rarity", VpRarity)
        End If
        'Texte
        If VpChk.GetItemChecked(9) Then
            VmCardText = VpCardText
        End If
    End Sub
    Public Shared Function AreAlike(Vp1 As clsXLItem, Vp2 As clsXLItem) As Boolean
        Return ( Vp1.Foil = Vp2.Foil And Vp1.Color = Vp2.Color And Vp1.Invoc = Vp2.Invoc And Vp1.Price = Vp2.Price And Vp1.Rarity = Vp2.Rarity And Vp1.Serie = Vp2.Serie And Vp1.SubType = Vp2.SubType And Vp1.Title = Vp2.Title And Vp1.Type = Vp2.Type And Vp1.CardText = Vp2.CardText )
    End Function
    Public ReadOnly Property Title As String
        Get
            Return VmTitle
        End Get
    End Property
    Public Property Quant As Integer
        Get
            Return VmQuant
        End Get
        Set (VpQuant As Integer)
            VmQuant = VpQuant
        End Set
    End Property
    Public ReadOnly Property Foil As Boolean
        Get
            Return VmFoil
        End Get
    End Property
    Public ReadOnly Property Color As String
        Get
            Return VmColor
        End Get
    End Property
    Public ReadOnly Property ForceEndurance As String
        Get
            Return VmForceEndurance
        End Get
    End Property
    Public ReadOnly Property Invoc As String
        Get
            Return VmInvoc
        End Get
    End Property
    Public ReadOnly Property Serie As String
        Get
            Return VmSerie
        End Get
    End Property
    Public ReadOnly Property Price As String
        Get
            Return VmPrice
        End Get
    End Property
    Public ReadOnly Property Rarity As String
        Get
            Return VmRarity
        End Get
    End Property
    Public ReadOnly Property SubType As String
        Get
            Return VmSubType
        End Get
    End Property
    Public ReadOnly Property Type As String
        Get
            Return VmType
        End Get
    End Property
    Public ReadOnly Property CardText As String
        Get
            Return VmCardText
        End Get
    End Property
    Public Property Reserve As Boolean
        Get
            Return VmReserve
        End Get
        Set (VpReserve As Boolean)
            VmReserve = VpReserve
        End Set
    End Property
End Class
