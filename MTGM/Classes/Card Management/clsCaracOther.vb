Imports System.ComponentModel
Public Class clsCaracOther
    Private VmTotalCards As String
    Private VmMyTotalCards As String
    Private VmMyTotalDistinctCards As String
    Private VmTotalPricing As String
    <DisplayName("Total cartes existantes"), Description("Nombre total de cartes existantes répondant aux critères de filtrage pour le niveau courant dans l'arborescence")> _
    Public Property TotalCards As String
        Get
            Return VmTotalCards
        End Get
        Set (VpTotalCards As String)
            VmTotalCards = VpTotalCards
        End Set
    End Property
    <DisplayName("Total cartes possédées"), Description("Nombre total de cartes possédées répondant aux critères de filtrage pour le niveau courant dans l'arborescence")> _
    Public Property MyTotalCards As String
        Get
            Return VmMyTotalCards
        End Get
        Set (VpMyTotalCards As String)
            VmMyTotalCards = VpMyTotalCards
        End Set
    End Property
    <DisplayName("Total cartes possédées (distinctes)"), Description("Nombre total de cartes possédées et distinctes répondant aux critères de filtrage pour le niveau courant dans l'arborescence")> _
    Public Property MyTotalDistinctCards As String
        Get
            Return VmMyTotalDistinctCards
        End Get
        Set (VpMyTotalDistinctCards As String)
            VmMyTotalDistinctCards = VpMyTotalDistinctCards
        End Set
    End Property
    <DisplayName("Cote totale des cartes possédées"), Description("Coût estimé de toutes les cartes possédées répondant aux critères de filtrage pour le niveau courant dans l'arborescence")> _
    Public Property TotalPricing As String
        Get
            Return VmTotalPricing
        End Get
        Set (VpTotalPricing As String)
            VmTotalPricing = VpTotalPricing
        End Set
    End Property
End Class
