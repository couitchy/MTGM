Imports System.ComponentModel
Public Class clsEditionHeader
    Public Enum eBorder
        White
        Black
        Silver
    End Enum
    Private VmSeriesCD As String = "ME"
    Private VmSeriesCD_MO As String = "ME"
    Private VmSeriesCD_MW As String = "ME"
    Private VmSeriesNM As String = "Magic Edition"
    Private VmSeriesNM_FR As String = "Édition Magic"
    Private VmSeriesNM_MtG As String = "Magic Ed..."
    Private VmSeriesNM_UG As String = "Magic Edition"
    Private VmBorder As eBorder = eBorder.White
    Private VmRelease As Date = Date.Now.ToShortDateString
    Private VmTotCards As Integer = 175
    Private VmRare As Integer = 55
    Private VmUncommon As Integer = 55
    Private VmCommon As Integer = 55
    Private VmLand As Integer = 10
    Private VmLogoEdition As String = ""
    Private VmNotesEdition As String = ""
    <Category("Identification"), Description("Code de l'édition à 2 chiffres")> _
    Public Property SeriesCD As String
        Get
            Return VmSeriesCD.Substring(0, 2)
        End Get
        Set (VpSeriesCD As String)
            VmSeriesCD = VpSeriesCD
        End Set
    End Property
    <Category("Identification"), Description("Code de l'édition à 2 chiffres (Magic Online)")> _
    Public Property SeriesCD_MO As String
        Get
            Return VmSeriesCD_MO
        End Get
        Set (VpSeriesCD_MO As String)
            VmSeriesCD_MO = VpSeriesCD_MO
        End Set
    End Property
    <Category("Identification"), Description("Code de l'édition à 2 chiffres (Magic Workstation)")> _
    Public Property SeriesCD_MW As String
        Get
            Return VmSeriesCD_MW
        End Get
        Set (VpSeriesCD_MW As String)
            VmSeriesCD_MW = VpSeriesCD_MW
        End Set
    End Property
    <Category("Identification"), Description("Nom de l'édition (VO)")> _
    Public Property SeriesNM As String
        Get
            Return VmSeriesNM
        End Get
        Set (VpSeriesNM As String)
            VmSeriesNM = VpSeriesNM
        End Set
    End Property
    <Category("Identification"), Description("Nom de l'édition (VF)")> _
    Public Property SeriesNM_FR As String
        Get
            Return VmSeriesNM_FR
        End Get
        Set (VpSeriesNM_FR As String)
            VmSeriesNM_FR = VpSeriesNM_FR
        End Set
    End Property
    <Category("Identification"), Description("Nom raccourci de l'édition sur magiccorportation.com (correspondance requise pour la mise à jour des prix...)")> _
    Public Property SeriesNM_MtG As String
        Get
            Return VmSeriesNM_MtG
        End Get
        Set (VpSeriesNM_MtG As String)
            VmSeriesNM_MtG = VpSeriesNM_MtG
        End Set
    End Property
    <Category("Identification"), Description("Nom de l'édition sur urzagatherer.app")> _
    Public Property SeriesNM_UG As String
        Get
            Return VmSeriesNM_UG
        End Get
        Set (VpSeriesNM_UG As String)
            VmSeriesNM_UG = VpSeriesNM_UG
        End Set
    End Property
    <Category("Détails"), Description("Nombre de cartes de l'édition")> _
    Public Property TotCards As Integer
        Get
            Return VmTotCards
        End Get
        Set (VpTotCards As Integer)
            VmTotCards = VpTotCards
        End Set
    End Property
    <Category("Détails"), Description("Nombre de cartes rares")> _
    Public Property Rare As Integer
        Get
            Return VmRare
        End Get
        Set (VpRare As Integer)
            VmRare = VpRare
        End Set
    End Property
    <Category("Détails"), Description("Nombre de cartes peu communes")> _
    Public Property Uncommon As Integer
        Get
            Return VmUncommon
        End Get
        Set (VpUncommon As Integer)
            VmUncommon = VpUncommon
        End Set
    End Property
    <Category("Détails"), Description("Nombre de cartes communes")> _
    Public Property Common As Integer
        Get
            Return VmCommon
        End Get
        Set (VpCommon As Integer)
            VmCommon = VpCommon
        End Set
    End Property
    <Category("Détails"), Description("Nombre de terrains")> _
    Public Property Land As Integer
        Get
            Return VmLand
        End Get
        Set (VpLand As Integer)
            VmLand = VpLand
        End Set
    End Property
    <Category("Divers"), Description("Date de sortie")> _
    Public Property Release As Date
        Get
            Return VmRelease
        End Get
        Set (VpRelease As Date)
            VmRelease = VpRelease
        End Set
    End Property
    <Category("Divers"), Description("Type de bordure")> _
    Public Property Border As eBorder
        Get
            Return VmBorder
        End Get
        Set (VpBorder As eBorder)
            VmBorder = VpBorder
        End Set
    End Property
    <Category("Divers"), Description("Fichier d'image (PNG 21x21) correspondant au logo de l'édition"), Editor(GetType(System.Windows.Forms.Design.FileNameEditor), GetType(System.Drawing.Design.UITypeEditor))> _
    Public Property LogoEdition As String
        Get
            Return VmLogoEdition
        End Get
        Set (VpLogoEdition As String)
            VmLogoEdition = VpLogoEdition
        End Set
    End Property
    <Category("Divers"), Description("Notes diverses sur l'édition")> _
    Public Property NotesEdition As String
        Get
            If VmNotesEdition = "" Then
                Return "N/A"
            Else
                Return VmNotesEdition
            End If
        End Get
        Set (VpNotesEdition As String)
            VmNotesEdition = VpNotesEdition
        End Set
    End Property
    Public Function GetBorder(VpBorder As eBorder) As String
        Select Case VpBorder
            Case eBorder.Black
                Return "'B'"
            Case eBorder.White
                Return "'W'"
            Case eBorder.Silver
                Return "'S'"
            Case Else
                Return "Null"
        End Select
    End Function
    Public Function SetBorder(VpBorder As String) As eBorder
        Select Case VpBorder
            Case "B"
                Return eBorder.Black
            Case "W"
                Return eBorder.White
            Case "S"
                Return eBorder.Silver
            Case Else
                Return eBorder.White
        End Select
    End Function
End Class
