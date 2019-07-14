Public Class clsCard
    Private VmCardName As String                    'Nom de la carte (VO)
    Private VmCardNameFR As String                  'Nom de la carte (VF)
    Private VmCardType As String                    'Type de la carte (C,I,A,E,L,N,S,T,U,P)
    Private VmManasInvoc As clsManas                'Coût d'invocation de la carte
    Private VmManasGen As clsManas = Nothing        'Manas générables par la carte
    Private VmTapped As Boolean = False             'Carte engagée ?
    Private VmTagged As Boolean = False             'Carte marquée ? (utilisation interne)
    Private VmSpeciality As clsSpeciality = Nothing 'Carte destinée à une utilisation spéciale (pour la simulation de déploiement uniquement) ?
    Private VmCardText As String
    Private VmSubType As String
    Private VmMyCost As String
    Private VmColor As String
    Public Sub New(VpCardName As String, VpNameFR As String, VpCardText As String, VpCost As String, VpMyCost As String, VpColor As String, VpType As String, VpSubType As String, VpGestDeploy As Boolean)
    Dim VpGCost As String
        VmCardName = VpCardName
        VmCardNameFR = VpNameFR
        VmCardText = VpCardText
        VmSubType = VpSubType
        VmMyCost = VpMyCost
        VmColor = VpColor
        If VpGestDeploy Then
            VmCardType = VpType
            VmManasInvoc = New clsManas(VpCost)
            'On ne s'occupe pas des arpenteurs
            If VpType = "P" Then Exit Sub
            VpCardText = VpCardText.ToLower.Replace(vbCrLf, " ").Replace(".", "")
            'Carte parsable
            If VpCardText.Contains(clsModule.CgManaParsing(0)) Then
                VpGCost = VpCardText.Substring(0, VpCardText.IndexOf(clsModule.CgManaParsing(0)) - 1)
                'Carte dont le mana générable dépend d'autres paramètres non contrôlables (dans ce cas on devrait plutôt se trouver dans la situation de myspecialuses)
                If VpGCost.EndsWith(clsModule.CgManaParsing(1)) Then
                    'On affecte un mana incolore par défaut
                    VpGCost = "A"
                Else
                    VpGCost = VpGCost.Substring(VpGCost.LastIndexOf(clsModule.CgManaParsing(1)) + clsModule.CgManaParsing(1).Length + 1)
                End If
                VmManasGen = New clsManas(VpGCost)
            'Terrain sans texte explicite
            ElseIf VpCardText.StartsWith("[") Then
                VmManasGen = New clsManas(VpCardText.Replace("[", "").Replace("]", ""))
            ElseIf VpCardText.Length = 1 Or VpCardText.Length = 3 Then
                VmManasGen = New clsManas(VpCardText)
            End If
        End If
    End Sub
    Public Function Requires(VpCard As clsCard) As Boolean
    '-----------------------------------------------------------------------------------------------------------
    'Renvoie vrai si l'invocation de la carte courante requiert le mana produit par la carte passée en paramètre
    '-----------------------------------------------------------------------------------------------------------
        Return VmManasInvoc.IsBetterWith(VpCard.ManasGen)
    End Function
    Public ReadOnly Property CardName As String
        Get
            Return VmCardName
        End Get
    End Property
    Public ReadOnly Property CardNameFR As String
        Get
            Return VmCardNameFR
        End Get
    End Property
    Public ReadOnly Property CardType As String
        Get
            Return VmCardType
        End Get
    End Property
    Public ReadOnly Property CardSubType As String
        Get
            Return VmSubType
        End Get
    End Property
    Public ReadOnly Property CardMyCost As String
        Get
            Return VmMyCost
        End Get
    End Property
    Public ReadOnly Property CardColor As String
        Get
            Return VmColor
        End Get
    End Property
    Public ReadOnly Property ManaAble As Boolean
        Get
            Return Not ( VmManasGen Is Nothing )
        End Get
    End Property
    Public ReadOnly Property ManasPot As Integer
        Get
            Return VmManasGen.Potentiel
        End Get
    End Property
    Public ReadOnly Property ManasGen As clsManas
        Get
            Return VmManasGen
        End Get
    End Property
    Public ReadOnly Property ManasInvoc As clsManas
        Get
            Return VmManasInvoc
        End Get
    End Property
    Public ReadOnly Property IsALand As Boolean
        Get
            Return VmCardType = "L"
        End Get
    End Property
    Public Property Tapped As Boolean
        Get
            Return VmTapped
        End Get
        Set (VpTapped As Boolean)
            VmTapped = VpTapped
        End Set
    End Property
    Public Property Tagged As Boolean
        Get
            Return VmTagged
        End Get
        Set (VpTagged As Boolean)
            VmTagged = VpTagged
        End Set
    End Property
    Public Property Speciality As clsSpeciality
        Get
            Return VmSpeciality
        End Get
        Set (VpSpeciality As clsSpeciality)
            VmSpeciality = VpSpeciality
            VmManasGen = Nothing            'Si la carte a une spécialité, on efface le parsing par défaut qui avait eu lieu précédemment
        End Set
    End Property
    Public ReadOnly Property IsSpecial As Boolean
        Get
            Return Not ( VmSpeciality Is Nothing )
        End Get
    End Property
    Public ReadOnly Property CardText As String
        Get
            Return VmCardText
        End Get
    End Property
    Public Class clsManasPotComparer
        Implements IComparer(Of clsCard)
        Private VmUserList As CheckedListBox
        Public Sub New(VpUserList As CheckedListBox)
            VmUserList = VpUserList
        End Sub
        Public Function Compare(ByVal x As clsCard, ByVal y As clsCard) As Integer Implements IComparer(Of clsCard).Compare
        '---------------------------------------------------------------------------------------------------------------------
        'Permet le tri des cartes dans l'ordre de préférence d'invocation, selon le mana qu'elles sont susceptibles de générer
        'Favorise également l'invocation des cartes spéciales selon l'ordre spécifié par l'utilisateur
        '---------------------------------------------------------------------------------------------------------------------
        Dim VpPot1 As Integer = Me.GetMiniPot(x)
        Dim VpPot2 As Integer = Me.GetMiniPot(y)
            Return VpPot2 - VpPot1
        End Function
        Private Function GetMiniPot(VpCard As clsCard) As Integer
        Dim VpPot As Integer
            If VpCard.ManaAble Then
                VpPot = VpCard.ManasPot
            ElseIf VpCard.IsSpecial Then
                VpPot = CgMaxPot - VmUserList.CheckedItems.IndexOf(VpCard.CardName)
            End If
            Return VpPot
        End Function
    End Class
End Class
