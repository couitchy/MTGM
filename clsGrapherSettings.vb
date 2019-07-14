Imports NPlot
Imports System.ComponentModel
Public Class clsGrapherSettings
    Private VmOwner As frmGrapher
    Private VmLegende As String
    Private VmColor As Color
    Private VmStyle As Drawing2D.DashStyle
    Private VmThickness As Single
    Private VmRefPlot As LinePlot
    Private VmVisible As Boolean
    Public Sub New(VpOwner As frmGrapher, VpLegende As String, VpColor As Color, VpStyle As Drawing2D.DashStyle, VpThickness As Single, VpVisible As Boolean, VpPlot As LinePlot)
        VmOwner = VpOwner
        VmLegende = VpLegende
        VmColor = VpColor
        VmStyle = VpStyle
        VmThickness = VpThickness
        VmVisible = VpVisible
        VmRefPlot = VpPlot
    End Sub
    <DisplayName("Légende"), Description("Texte apparaissant dans la légende du graphique")> _
    Public Property Legende As String
        Get
            Return VmLegende
        End Get
        Set (VpLegende As String)
            VmLegende = VpLegende
            VmRefPlot.Label = VpLegende
            Call VmOwner.RefreshAllPlots(False)
        End Set
    End Property
    <DisplayName("Couleur"), Description("Couleur de la courbe")> _
    Public Property myColor As Color
        Get
            Return VmColor
        End Get
        Set (VpColor As Color)
            VmColor = VpColor
            VmRefPlot.Color = VpColor
            Call VmOwner.RefreshAllPlots(False)
        End Set
    End Property
    <DisplayName("Style"), DefaultValue(Drawing2D.DashStyle.Solid), Description("Style du tracé de la courbe")> _
    Public Property myStyle As Drawing2D.DashStyle
        Get
            Return VmStyle
        End Get
        Set (VpStyle As Drawing2D.DashStyle)
            VmStyle = VpStyle
            VmRefPlot.Pen.DashStyle = VpStyle
            Call VmOwner.RefreshAllPlots(False)
        End Set
    End Property
    <DisplayName("Epaisseur"), DefaultValue(1), Description("Epaisseur de la courbe")> _
    Public Property Thickness As Single
        Get
            Return VmThickness
        End Get
        Set (VpThickness As Single)
            VmThickness = VpThickness
            VmRefPlot.Pen.Width = VpThickness
            Call VmOwner.RefreshAllPlots(False)
        End Set
    End Property
    <Browsable(False)> _
    Public Property myVisible As Boolean
        Get
            Return VmVisible
        End Get
        Set (VpVisible As Boolean)
            VmVisible = VpVisible
        End Set
    End Property
    <Browsable(False)> _
    Public ReadOnly Property RefPlot As LinePlot
        Get
            Return VmRefPlot
        End Get
    End Property
End Class
