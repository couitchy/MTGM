Imports System.ComponentModel
Public Class clsCaracEdition
    Inherits clsCaracOther
    Private VmSerieDate As String
    Public Sub New(VpCaracOther As clsCaracOther)
        With VpCaracOther
            MyBase.MyTotalCards = .MyTotalCards
            MyBase.MyTotalDistinctCards = .MyTotalDistinctCards
            MyBase.TotalCards = .TotalCards
            MyBase.TotalPricing = .TotalPricing
        End With
    End Sub
    <DisplayName("Date de sortie"), Description("Date de parution de l'édition sélectionnée dans l'arborescence")> _
    Public Property SerieDate As String
        Get
            Return VmSerieDate
        End Get
        Set (VpSerieDate As String)
            VmSerieDate = VpSerieDate
        End Set
    End Property
End Class
