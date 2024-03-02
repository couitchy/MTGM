Imports System.Globalization
Public Class clsUpdateContent
    Private VmType As EgMAJContenu
    Private VmLocale As String
    Private VmServeur As String
    Private VmSize As Integer
    Public Enum EgMAJContenu
        NewPict = 0
        NewPrix
        NewAut
        NewSimu
        NewTxtVF
        NewRulings
        PatchPict
        PatchTrad
        PatchSubTypes
        PatchSubTypesVF
        PatchCosts
        PatchMultiverseId
        NewSerie
        NewTrad
    End Enum
    Public Sub New(VpType As EgMAJContenu, VpLocale As String, VpServeur As String, VpSize As Integer)
    Dim VpLocaleDate As Date
    Dim VpServeurDate As Date
        VmType = VpType
        'Si on reconnait une date, il faut passer par une double conversion pour s'affranchir des problèmes de format
        If Date.TryParseExact(VpLocale, "dd/MM/yyyy", New CultureInfo("fr-FR"), DateTimeStyles.None, VpLocaleDate) AndAlso Date.TryParseExact(VpServeur, "dd/MM/yyyy", New CultureInfo("fr-FR"), DateTimeStyles.None, VpServeurDate) Then
            VmLocale = VpLocaleDate.ToShortDateString
            VmServeur = VpServeurDate.ToShortDateString
        ElseIf Date.TryParse(VpLocale, VpLocaleDate) AndAlso Date.TryParseExact(VpServeur, "dd/MM/yyyy", New CultureInfo("fr-FR"), DateTimeStyles.None, VpServeurDate) Then
            VmLocale = VpLocaleDate.ToShortDateString
            VmServeur = VpServeurDate.ToShortDateString
        ElseIf VpLocale = "" AndAlso Date.TryParseExact(VpServeur, "dd/MM/yyyy", New CultureInfo("fr-FR"), DateTimeStyles.None, VpServeurDate) Then
            VmLocale = VpLocale
            VmServeur = VpServeurDate.ToShortDateString
        Else
            VmLocale = VpLocale
            VmServeur = VpServeur
        End If
        VmSize = VpSize
    End Sub
    Public ReadOnly Property TypeContenu As EgMAJContenu
        Get
            Return VmType
        End Get
    End Property
    Public ReadOnly Property TypeContenuStr As String
        Get
            Select Case VmType
                Case clsUpdateContent.EgMAJContenu.NewPrix
                    Return "Mise à jour des prix"
                Case clsUpdateContent.EgMAJContenu.NewAut
                    Return "Mise à jour des autorisations tournois"
                Case clsUpdateContent.EgMAJContenu.NewSimu
                    Return "Mise à jour des modèles et/ou historiques"
                Case clsUpdateContent.EgMAJContenu.NewTxtVF
                    Return "Mise à jour des textes des cartes en français"
                Case clsUpdateContent.EgMAJContenu.NewRulings
                    Return "Mise à jour des règles spécifiques des cartes"
                Case clsUpdateContent.EgMAJContenu.PatchPict
                    Return "Correctif d'images de cartes"
                Case clsUpdateContent.EgMAJContenu.PatchTrad
                    Return "Correctif des libellés de cartes en français"
                Case clsUpdateContent.EgMAJContenu.PatchSubTypes
                    Return "Correctif des sous-types de cartes"
                Case clsUpdateContent.EgMAJContenu.PatchSubTypesVF
                    Return "Correctif des traductions des sous-types"
                Case clsUpdateContent.EgMAJContenu.PatchCosts
                    Return "Correctif des coûts d'invocation"
                Case clsUpdateContent.EgMAJContenu.PatchMultiverseId
                    Return "Mise à jour des identifiants Multiverse"
                Case clsUpdateContent.EgMAJContenu.NewPict
                    Return "Service pack d'images de cartes"
                Case clsUpdateContent.EgMAJContenu.NewSerie
                    Return "Nouvelle édition Magic The Gathering"
                Case clsUpdateContent.EgMAJContenu.NewTrad
                    Return "Mise à jour des libellés de cartes en français"
                Case Else
                    Return ""
            End Select
        End Get
    End Property
    Public ReadOnly Property Locale As String
        Get
            If VmLocale = "" Then
                Return "N/A"
            Else
                Return VmLocale
            End If
        End Get
    End Property
    Public ReadOnly Property Serveur As String
        Get
            Return VmServeur
        End Get
    End Property
    Public ReadOnly Property SizeDL As Integer
        Get
            Return VmSize
        End Get
    End Property
End Class
