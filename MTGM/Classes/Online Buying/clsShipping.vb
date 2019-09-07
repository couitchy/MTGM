Public Class clsShipping
    Private VmCountry As String
    Private VmCountryCode As String
    Private VmValueMax As Single
    Private VmQuantityMax As Integer
    Private VmCost As Single
    Public Sub New(VpCountry As String, VpCountryCode As String, VpValueMax As Single, VpQuantityMax As Integer, VpCost As Single)
        VmCountry = VpCountry
        VmCountryCode = VpCountryCode
        VmValueMax = VpValueMax
        VmQuantityMax = VpQuantityMax
        VmCost = VpCost
    End Sub
    Public Shared Function CreateListFrom(VpShipping As clsShipping) As List(Of clsShipping)
    Dim VpList As New List(Of clsShipping)
        VpList.Add(VpShipping)
        Return VpList
    End Function
    Public Shared Function GetShippingCostFor(VpQuantity As Integer, VpValue As Single, VpShippingPolicy As List(Of clsShipping)) As Single
    '-----------------------------------------------------------------------------------------------
    'Détermine les frais de port pour la quantité de cartes demandées et la valeur totale de l'envoi
    '-----------------------------------------------------------------------------------------------
    Dim VpShippingCost As Single = mdlConstGlob.CgWorstShippingCost 'fait en sorte que si la livraison est impossible, le prix de la carte soit tellement élevé qu'elle ne sera jamais retenue
        For Each VpShipping As clsShipping In VpShippingPolicy
            If VpQuantity <= VpShipping.QuantityMax AndAlso VpValue <= VpShipping.ValueMax Then
                VpShippingCost = Math.Min(VpShippingCost, VpShipping.Cost)
            End If
        Next VpShipping
        Return VpShippingCost
    End Function
    Public Shared Function GetShippingCostMin(VpShippingPolicy As List(Of clsShipping)) As Single
    '-------------------------------------------------------------------------
    'Retourne les frais de port les plus bas qu'on puisse espérer pour l'envoi
    '-------------------------------------------------------------------------
    Dim VpShippingCost As Single = mdlConstGlob.CgWorstShippingCost
        For Each VpShipping As clsShipping In VpShippingPolicy
            VpShippingCost = Math.Min(VpShippingCost, VpShipping.Cost)
        Next VpShipping
        Return VpShippingCost
    End Function
    Public Property Country As String
        Get
            Return VmCountry
        End Get
        Set (VpCountry As String)
            VmCountry = VpCountry
        End Set
    End Property
    Public Property CountryCode As String
        Get
            Return VmCountryCode
        End Get
        Set (VpCountryCode As String)
            VmCountryCode = VpCountryCode
        End Set
    End Property
    Public Property ValueMax As Single
        Get
            Return VmValueMax
        End Get
        Set (VpValueMax As Single)
            VmValueMax = VpValueMax
        End Set
    End Property
    Public Property QuantityMax As Integer
        Get
            Return VmQuantityMax
        End Get
        Set (VpQuantityMax As Integer)
            VmQuantityMax = VpQuantityMax
        End Set
    End Property
    Public Property Cost As Single
        Get
            Return VmCost
        End Get
        Set (VpCost As Single)
            VmCost = VpCost
        End Set
    End Property
End Class
