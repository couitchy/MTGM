Imports System.IO
Public Class clsPartie
    Private VmVerbose As Boolean                    'Verbosité
    Private VmSimuOut As StreamWriter               'Sortie de verbosité
    Private VmDeck As New List(Of clsCard)          'Bibliothèque
    Private VmDeckCopy As New List(Of clsCard)      'Copie de la bibliothèque (restaurée à chaque nouvelle partie)
    Private VmDrawn As New List(Of clsCard)         'Cartes piochées / en main
    Private VmInPlay As New List(Of clsCard)        'Cartes en jeu (permanents)
    Private VmInRound As New List(Of clsCard)       'Cartes en jeu pour le tour courant (éphémères)
    Private VmReserve As clsManas                   'Réserve de manas pour le tour courant
    Private VmLives As Integer = clsModule.CgNLives 'Nombre de points de vie
    Public Sub New(VpSource As String, VpRestriction As String, Optional VpGestDeploy As Boolean = False, Optional VpVerbose As Boolean = False, Optional VpSimuOut As StreamWriter = Nothing)
    '-------------------
    'Construction du jeu
    '-------------------
    Dim VpSQL As String
        If Not VpGestDeploy Then
            VpSQL = "Select Card.Title, " + VpSource + ".Items, CardFR.TitleFR From (Card Inner Join " + VpSource + " On " + VpSource + ".EncNbr = Card.EncNbr) Inner Join CardFR On Card.EncNbr = CardFR.EncNbr Where"
        Else
            VpSQL = "Select Card.Title, Card.CardText, Card.Type, Spell.Cost, " + VpSource + ".Items, CardFR.TitleFR, Card.SubType, Spell.Color, Spell.myCost From ((Card Inner Join " + VpSource + " On " + VpSource + ".EncNbr = Card.EncNbr) Inner Join Spell On Card.Title = Spell.Title) Inner Join CardFR On Card.EncNbr = CardFR.EncNbr Where"
        End If
        VpSQL = VpSQL + VpRestriction
        VpSQL = clsModule.TrimQuery(VpSQL)
        VgDBCommand.CommandText = VpSQL
        VgDBReader = VgDBCommand.ExecuteReader
        With VgDBReader
            While .Read
                If Not VpGestDeploy Then
                    Me.AddCard(.GetString(0), .GetString(2), .GetInt32(1))
                Else
                    Me.AddCard(.GetString(0), .GetString(5), .GetInt32(4), .GetValue(1).ToString.Replace(vbCrLf, " "), .GetValue(3).ToString, .GetValue(8).ToString, .GetString(7), .GetValue(2).ToString, .GetValue(6).ToString, True)
                End If
            End While
            .Close
        End With
        VmVerbose = VpVerbose
        VmSimuOut = VpSimuOut
    End Sub
    Private Sub AddCard(VpName As String, VpNameFR As String, VpCount As Integer, Optional VpCardText As String = "", Optional VpCost As String = "", Optional VpMyCost As String = "", Optional VpColor As String = "", Optional VpType As String = "", Optional VpSubType As String = "", Optional VpGestDeploy As Boolean = False)
    '--------------------
    'Construction du deck
    '--------------------
        For VpI As Integer = 1 To VpCount
            VmDeckCopy.Add(New clsCard(VpName, VpNameFR, VpCardText, VpCost, VpMyCost, VpColor, VpType, VpSubType, VpGestDeploy))
        Next VpI
    End Sub
    Public Sub DeckShuffle
    '--------------
    'Mélange le jeu
    '--------------
    Dim VpI As Integer
    Dim VpRandomPos As New SortedList(Me.CardsCount)
        VmDrawn.Clear
        VmInPlay.Clear
        VmDeck = New List(Of clsCard)(VmDeckCopy)
        'Génère un tableau trié de nombres aléatoires
        For VpI = 0 To Me.CardsCount - 1
            VpRandomPos.Add(clsModule.VgRandom.NextDouble, VpI)
        Next VpI
        'Réordonne les cartes en conséquence
        VpI = 0
        For Each VpPos As Integer In VpRandomPos.Values
            VmDeck.Item(VpI) = VmDeckCopy.Item(VpPos)
            VpI = VpI + 1
        Next VpPos
    End Sub
    Public Sub UntapAll
    '---------------------------
    'Désengage toutes les cartes
    '---------------------------
        For Each VpCard As clsCard In VmDeckCopy        'Le faire sur VmDeckCopy permet de n'oublier aucune carte
            If Not (VpCard.IsSpecial AndAlso VpCard.Speciality.DoesntUntap) Then
                VpCard.Tapped = False
            End If
        Next VpCard
    End Sub
    Public Sub UntagAll
    '--------------------------
    'Démarque toutes les cartes
    '--------------------------
        For Each VpCard As clsCard In VmDeckCopy        'Le faire sur VmDeckCopy permet de n'oublier aucune carte
            VpCard.Tagged = False
        Next VpCard
    End Sub
    Public Sub Draw(Optional VpTirages As Integer = 1)
    '----------------
    'Pioche une carte
    '----------------
        If VmDeck.Count > 0 Then
            For VpI As Integer = 1 To VpTirages
                VmDrawn.Add(VmDeck.Item(0))
                VmDeck.RemoveAt(0)
                If VmDeck.Count = 0 Then Exit Sub
            Next VpI
        End If
    End Sub
    Public Function IsSequencePresent(VpSequence As clsComboSequence) As Boolean
    '-------------------------------------------------------------
    'Renvoie vrai si toutes les cartes spécifiées ont été piochées
    '-------------------------------------------------------------
        For Each VpItem As clsElement In VpSequence.Elements
            If Not Me.IsDrawn(VpItem) Then
                Return False
            End If
        Next VpItem
        Return True
    End Function
    Public Function IsInFullDeck(VpCardName As String) As Boolean
    '-------------------------------------------------------------------------------
    'Renvoie vrai si le deck contient la carte dont le nom est spécifié en paramètre
    '-------------------------------------------------------------------------------
        Return Me.IsInList(VpCardName, VmDeckCopy)
    End Function
    Public Function IsInPlay(VpCardName As String) As Boolean
    '--------------------------------------------------------------------------------------------
    'Renvoie vrai si le champ de bataille contient la carte dont le nom est spécifié en paramètre
    '--------------------------------------------------------------------------------------------
        Return Me.IsInList(VpCardName, VmInPlay)
    End Function
    Private Function IsInList(VpCardName As String, VpList As List(Of clsCard)) As Boolean
        For Each VpCard As clsCard In VpList
            If VpCard.CardName = VpCardName Then
                Return True
            End If
        Next VpCard
        Return False
    End Function
    Public Sub PrepNewPlayRound
    '------------------------------------
    'Préparation d'un nouveau tour de jeu
    '------------------------------------
        VmInRound.Clear
        VmReserve = New clsManas
        Call Me.FollowRound
    End Sub
    Public Sub FollowRound
    '--------------------------------------------------------------------------------------------------------------
    'Génération éventuelle de mana sur des cartes arrivées postérieurement par rapport à la pose du dernier terrain
    '--------------------------------------------------------------------------------------------------------------
        For Each VpCard As clsCard In Me.CardsInPlay
            If VpCard.ManaAble And Not VpCard.Tapped Then
                Call VmReserve.AddSubManas(VpCard.ManasGen)
                Call clsModule.VerboseSimu(VmVerbose, VpCard.ManasGen.Potentiel.ToString + " manas ajoutés à la réserve (" + VpCard.CardName + ")", VmSimuOut)
                VpCard.Tapped = True
            End If
        Next VpCard
    End Sub
    Public Sub AddToInPlay(VpCard As clsCard)
    '-----------------------------------
    'Pose en jeu un permanent de la main
    '-----------------------------------
        If Not VpCard Is Nothing Then
            VmInPlay.Add(VpCard)
            VmDrawn.Remove(VpCard)
        End If
    End Sub
    Public Sub AddToInRound(VpCard As clsCard)
    '----------------------------------
    'Pose en jeu un éphémère de la main
    '----------------------------------
        VmInRound.Add(VpCard)
        VmDrawn.Remove(VpCard)
    End Sub
    Public Sub CommitChange(VpSrc As List(Of clsCard), VpDest As List(Of clsCard), Optional VpRemove As Boolean = False, Optional VpRemoveDrawn As Boolean = True)
    '--------------------------------------------------------------------------------------------------
    'Synthèse des deux routines précédentes utile pour ne pas perturber lors de l'énumération des items
    '--------------------------------------------------------------------------------------------------
        For Each VpCard As clsCard In VpSrc
            If VpRemove Then
                VpDest.Remove(VpCard)
            Else
                VpDest.Add(VpCard)
            End If
            If VpRemoveDrawn Then
                VmDrawn.Remove(VpCard)
            End If
        Next VpCard
    End Sub
    Public Function DoSpecialEffects(VpSrc As List(Of clsCard)) As Boolean
    '------------------------------------------------------------------------------------------------------------------------------------------------------------
    'Si des cartes possèdent des propriétés particulières spécifiées par l'utilisateur permettant de générer directement ou indirectement du mana, l'effectue ici
    '(renvoie vrai si un effet spécial a effectivement été utilisé)
    '------------------------------------------------------------------------------------------------------------------------------------------------------------
    Dim VpSomething As Boolean = False          'Passe à vrai si au moins un effet a été utilisé
    Dim VpNext As Boolean = False               'Passe à vrai si l'effet courant ne peut pas être utilisé
    Dim VpAbort As Boolean = False              'Passe à vrai si l'effet a été avorté (cible illégale etc...) auquel cas il faut annuler l'effort
    Dim VpManasInvoc As clsManas = Nothing      'Manas nécessaires pour activer l'effet spécial
    Dim VpMyTarget As clsCard = Nothing         'Carte support
    Dim VpInt As Integer                        'Entier support
    Dim VpStrs() As String                      'Chaîne support
    Dim VpTmpInPlay1 As New List(Of clsCard)    'Liste support ajout
    Dim VpTmpInPlay2 As New List(Of clsCard)    'Liste support suppression
    Dim VpTmpInPlay3 As New List(Of clsCard)    'Liste support défausse
    Dim VpTmpInPlay4 As New List(Of clsCard)    'Liste support pioche
        For Each VpCard As clsCard In VpSrc
            If VpCard.IsSpecial Then
                VpNext = False
                VpAbort = False
                'Commence par regarder si l'effort à fournir est réalisable, en termes de :
                Select Case VpCard.Speciality.EffortID
                    '- engagement de la carte + manas nécessaires
                    Case 0
                        VpManasInvoc = New clsManas(VpCard.Speciality.Effort)
                        If VpCard.Tapped Or Not Me.Reserve.ContainsEnoughFor(VpManasInvoc) Then
                            VpNext = True
                            Exit Select
                        Else
                            Call Me.Reserve.AddSubManas(VpManasInvoc, -1)
                            VpCard.Tapped = True
                        End If
                    '- manas nécessaires ou manas nécessaires + sacrifice de cette carte
                    Case 1, 13
                        VpManasInvoc = New clsManas(VpCard.Speciality.Effort)
                        If Not Me.Reserve.ContainsEnoughFor(VpManasInvoc) Then
                            VpNext = True
                            Exit Select
                        Else
                            Call Me.Reserve.AddSubManas(VpManasInvoc, -1)
                            If VpCard.Speciality.EffortID = 13 Then
                                VpTmpInPlay2.Add(VpCard)
                            End If
                        End If
                    '- engagement de la carte ou engagement de la carte + points de vie à perdre ou engagement de la carte + engagement de la carte spécifiée
                    Case 2, 22, 3
                        If VpCard.Tapped Then
                            VpNext = True
                            Exit Select
                        Else
                            'Perte de point de vie
                            If VpCard.Speciality.EffortID = 22 Then
                                Call clsModule.VerboseSimu(VmVerbose, "Joueur a " + VmLives.ToString + " point(s) de vie", VmSimuOut)
                                VpInt = CInt(VpCard.Speciality.Effort)
                                If VmLives <= VpInt Then
                                    VpNext = True
                                    Exit Select
                                Else
                                    VmLives = VmLives - VpInt
                                    VpCard.Tapped = True
                                End If
                            'Engagement supplémentaire de la carte spécifiée ou d'une des cartes spécifiées
                            ElseIf VpCard.Speciality.EffortID = 3 Then
                                VpStrs = VpCard.Speciality.Effort.Split(";")
                                For Each VpTarget As clsCard In Me.CardsInPlay
                                    For VpWanted As Integer = 0 To VpStrs.Length - 1
                                        If VpTarget.CardName = VpStrs(VpWanted) And Not VpTarget.Tapped Then
                                            VpMyTarget = VpTarget
                                            Exit For
                                        End If
                                    Next VpWanted
                                    If Not VpMyTarget Is Nothing Then
                                        VpMyTarget.Tapped = True
                                        VpCard.Tapped = True
                                    Else
                                        VpNext = True
                                        Exit Select
                                    End If
                                Next VpTarget
                            Else
                                VpCard.Tapped = True
                            End If
                        End If
                    '- sacrifice de cette carte (aucun prérequis)
                    Case 10
                        VpTmpInPlay2.Add(VpCard)
                    '- engagement de la carte + cartes à sacrifier ou cartes à sacrifier ou engagement de la carte + carte à sacrifier ou carte à sacrifier
                    Case 11, 12, 14, 15, 16
                        If VpCard.Speciality.EffortID = 12 Or VpCard.Speciality.EffortID = 15 Or VpCard.Speciality.EffortID = 16 Then
                            If VpCard.Tapped Then
                                VpNext = True
                                Exit Select
                            Else
                                VpCard.Tapped = True
                            End If
                        End If
                        'A part dans le cas 16, on n'est pas sûr de pouvoir assurer les sacrifices
                        If VpCard.Speciality.EffortID <> 16 Then
                            VpInt = 0
                            VpStrs = VpCard.Speciality.Effort.Split(";")
                            For Each VpTarget As clsCard In Me.CardsInPlay
                                For VpSacrifice As Integer = 0 To VpStrs.Length - 1
                                    If VpTarget.CardName = VpStrs(VpSacrifice) And Not VpTmpInPlay2.Contains(VpTarget) Then
                                        VpTmpInPlay2.Add(VpTarget)
                                        VpInt = VpInt + 1
                                        VpStrs(VpSacrifice) = ""    'Evite de repasser plusieurs fois sur la même carte (ou plutôt une carte du même nom)
                                        Exit For
                                    End If
                                Next VpSacrifice
                                'Dans les cas 14 et 15, il n'y a qu'une carte à sacrifier parmi celles de la liste
                                If VpInt = 1 And ( VpCard.Speciality.EffortID = 14 Or VpCard.Speciality.EffortID = 15 ) Then
                                    Exit For
                                End If
                            Next VpTarget
                            'Si on n'a pas pu sacrifier toutes les cartes requises, on sort après avoir annulé les sacrifices déjà faits ainsi que l'engagement de la carte
                            If VpInt < If(VpCard.Speciality.EffortID = 14 Or VpCard.Speciality.EffortID = 15, 1, VpStrs.Length) Then
                                VpTmpInPlay2.RemoveRange(VpTmpInPlay2.Count - VpInt, VpInt)
                                VpCard.Tapped = False
                                VpNext = True
                                Exit Select
                            End If
                        End If
                    '- cartes à défausser
                    Case 40
                        VpInt = CInt(VpCard.Speciality.Effort)
                        If Me.CardsDrawn.Count < (VpInt + VpTmpInPlay3.Count) Then      'attention il faut prendre en compte les cartes défaussées pas encore committées
                            VpNext = True
                            Exit Select
                        Else
                            'On défausse les dernières cartes (à priori les moins favorisées par le dernier classement)
                            VpTmpInPlay3.AddRange(Me.CardsDrawn.GetRange(Me.CardsDrawn.Count - VpInt - VpTmpInPlay3.Count, VpInt))
                        End If
                    '- points de vie à perdre
                    Case 21
                        Call clsModule.VerboseSimu(VmVerbose, "Joueur a : " + VmLives.ToString + " point(s) de vie", VmSimuOut)
                        VpInt = CInt(VpCard.Speciality.Effort)
                        If VmLives <= VpInt Then
                            VpNext = True
                            Exit Select
                        Else
                            VmLives = VmLives - VpInt
                        End If
                    '- présence en jeu
                    Case 30
                        If Not Me.IsInPlay(VpCard.Speciality.Effort) Then
                            VpNext = True
                            Exit Select
                        End If
                    '- présence dans le cimetière
                    Case 31

                    '- quantité de présence dans le cimetière
                    Case 32

                    Case Else
                End Select
                'Si l'effort a été fourni
                If Not VpNext Then
                    'Obtient la récompense, parmi :
                    Select Case VpCard.Speciality.EffetID
                        '- génération de manas
                        Case 100
                            Call Me.Reserve.AddSubManas(New clsManas(VpCard.Speciality.Effet))
                            VpSomething = True
                            Call clsModule.VerboseSimu(VmVerbose, "Effet utilisé : " + VpCard.CardName, VmSimuOut)
                        '- dégagement d'un artefact
                        Case 110
                            'Liste des artefacts potentiellement visés
                            VpStrs = VpCard.Speciality.Effet.Split(";")
                            VpNext = False
                            For Each VpArtefact As String In VpStrs
                                If VpNext Then Exit For
                                For Each VpTarget As clsCard In Me.CardsInPlay
                                    If (Not VpCard Is VpTarget) And VpTarget.CardName = VpArtefact And VpTarget.Tapped = True Then
                                        VpTarget.Tapped = False
                                        VpNext = True
                                        Call clsModule.VerboseSimu(VmVerbose, "Effet utilisé : " + VpCard.CardName + " pour dégager " + VpTarget.CardName, VmSimuOut)
                                        VpSomething = True
                                        Exit For
                                    End If
                                Next VpTarget
                            Next VpArtefact
                            VpAbort = Not VpNext
                        '- mise en jeu de l'artefact ou du terrain spécifié
                        Case 120
                            'Liste des cartes potentiellement visées
                            VpStrs = VpCard.Speciality.Effet.Split(";")
                            VpNext = False
                            For Each VpWanted As String In VpStrs
                                If VpNext Then Exit For
                                For Each VpTarget As clsCard In Me.CardsInDeck
                                    If VpTarget.CardName = VpWanted And Not VpTmpInPlay1.Contains(VpTarget) Then
                                        VpTmpInPlay1.Add(VpTarget)
                                        Me.CardsInDeck.Remove(VpTarget)     'Normalement on n'a pas le droit de toucher à la liste que l'on est en train d'énumérer (ou celle qui la contient) mais là on quitte la boucle immédiatement après...
                                        VpNext = True
                                        Call clsModule.VerboseSimu(VmVerbose, "Effet utilisé : " + VpCard.CardName + " pour mettre en jeu " + VpTarget.CardName, VmSimuOut)
                                        VpSomething = True
                                        Exit For
                                    End If
                                Next VpTarget
                            Next VpWanted
                            VpAbort = Not VpNext
                        '- pioche de cartes
                        Case 130
                            Call Me.Draw(CInt(VpCard.Speciality.Effet))
                            Call clsModule.VerboseSimu(VmVerbose, "Effet utilisé : " + VpCard.CardName, VmSimuOut)
                            VpSomething = True
                        '- piocher la carte spécifiée
                        Case 131
                            'Liste des cartes potentiellement visées
                            VpStrs = VpCard.Speciality.Effet.Split(";")
                            VpNext = False
                            For Each VpWanted As String In VpStrs
                                If VpNext Then Exit For
                                For Each VpTarget As clsCard In Me.CardsInDeck
                                    If VpTarget.CardName = VpWanted And Not VpTmpInPlay4.Contains(VpTarget) Then
                                        VpTmpInPlay4.Add(VpTarget)
                                        Me.CardsInDeck.Remove(VpTarget)     'Normalement on n'a pas le droit de toucher à la liste que l'on est en train d'énumérer (ou celle qui la contient) mais là on quitte la boucle immédiatement après...
                                        VpNext = True
                                        Call clsModule.VerboseSimu(VmVerbose, "Effet utilisé : " + VpCard.CardName + " pour piocher " + VpTarget.CardName, VmSimuOut)
                                        VpSomething = True
                                        Exit For
                                    End If
                                Next VpTarget
                            Next VpWanted
                            VpAbort = Not VpNext
                        Case Else
                    End Select
                End If
                'S'il faut annuler l'effort
                If VpAbort Then
                    Select Case VpCard.Speciality.EffortID
                        '- engagement de la carte + manas nécessaires
                        Case 0
                            VpCard.Tapped = False
                            Call Me.Reserve.AddSubManas(VpManasInvoc)
                        '- manas nécessaires
                        Case 1
                            Call Me.Reserve.AddSubManas(VpManasInvoc)
                        '- engagement de la carte
                        Case 2
                            VpCard.Tapped = False
                        '- engagement de la carte + engagement de la carte spécifiée
                        Case 3
                            VpCard.Tapped = False
                            VpMyTarget.Tapped = False
                        '- sacrifice de cette carte
                        Case 10
                            VpTmpInPlay2.RemoveAt(VpTmpInPlay2.Count - 1)
                        '- cartes à sacrifier
                        Case 11, 14
                            VpTmpInPlay2.RemoveRange(VpTmpInPlay2.Count - VpInt, VpInt)
                        '- engagement de la carte + cartes à sacrifier
                        Case 12, 15
                            VpCard.Tapped = False
                            VpTmpInPlay2.RemoveRange(VpTmpInPlay2.Count - VpInt, VpInt)
                        '- manas nécessaires + sacrifice de cette carte
                        Case 13
                            Call Me.Reserve.AddSubManas(VpManasInvoc)
                            VpTmpInPlay2.RemoveAt(VpTmpInPlay2.Count - 1)
                        '- points de vie à perdre
                        Case 21
                            VmLives = VmLives + CInt(VpCard.Speciality.Effort)
                        'engagement de la carte + points de vie à perdre
                        Case 22
                            VmLives = VmLives + CInt(VpCard.Speciality.Effort)
                            VpCard.Tapped = False
                        '- cartes à défausser
                        Case 40
                            VpTmpInPlay3.RemoveRange(VpTmpInPlay3.Count - VpInt, VpInt)
                        Case Else
                    End Select
                End If
            End If
        Next VpCard
        'Si on a mis en jeu ou retirer des cartes par un effet, c'est maintenant qu'il faut le commiter
        Call Me.CommitChange(VpTmpInPlay1, Me.CardsInPlay, False, False)
        Call Me.CommitChange(VpTmpInPlay2, Me.CardsInPlay, True, False)
        Call Me.CommitChange(VpTmpInPlay3, Me.CardsDrawn, True, False)
        Call Me.CommitChange(VpTmpInPlay4, Me.CardsDrawn, False, False)
        'Les cartes nouvellement arrivées peuvent être susceptibles de produire du mana immédiatement
        Call Me.FollowRound
        Return VpSomething
    End Function
    Public Function PickLand As clsCard
    '-----------------------------------------------------------------------------------------------------------------------
    'Détermine la couleur dominante de la main en cours pour retourner le terrain (s'il y en a un) le plus approprié à jouer
    '-----------------------------------------------------------------------------------------------------------------------
    Dim VpMaxPot As Short = 0
    Dim VpPot As Short
    Dim VpPicked As clsCard = Nothing
        'Parcourt les cartes de la main
        For Each VpCard As clsCard In VmDrawn
            'Si on a un terrain spécial, le sélectionne d'office
            If VpCard.IsALand AndAlso VpCard.IsSpecial Then
                If VpCard.Speciality.InvocTapped Then
                    VpCard.Tapped = True
                    Call clsModule.VerboseSimu(VmVerbose, "Terrain spécial posé (engagé) : " + VpCard.CardName, VmSimuOut)
                Else
                    Call clsModule.VerboseSimu(VmVerbose, "Terrain spécial posé : " + VpCard.CardName, VmSimuOut)
                End If
                Return VpCard
            'Si on a un terrain producteur de mana
            ElseIf VpCard.IsALand AndAlso VpCard.ManaAble Then
                'Détermine le potentiel sous-jacent (direct ou indirect) à sa mise en jeu
                VpPot = Me.GetPotFor(VpCard)
                'Retient le meilleur choix
                If VpPot > VpMaxPot Then
                    VpMaxPot = VpPot
                    VpPicked = VpCard
                ElseIf VpPicked Is Nothing Then
                    VpPicked = VpCard
                End If
            End If
        Next VpCard
        If Not VpPicked Is Nothing Then
            Call clsModule.VerboseSimu(VmVerbose, "Terrain posé : " + VpPicked.CardName, VmSimuOut)
        End If
        Return VpPicked
    End Function
    Private Function GetPotFor(VpLand As clsCard) As Short
    '--------------------------------------------------------------------------------------------------
    'Retourne le potentiel générable par les cartes en main qui nécessite le terrain passé en paramètre
    '--------------------------------------------------------------------------------------------------
    Dim VpPot As Short = 0
        For Each VpCard As clsCard In VmDrawn
            If ( Not ( VpCard Is VpLand ) ) AndAlso VpCard.ManaAble AndAlso VpCard.Requires(VpLand) Then
                VpPot = VpPot + VpCard.ManasPot
            End If
        Next VpCard
        Return VpPot
    End Function
    Private Function IsDrawnMatching(VpCard As clsCard, VpElement As clsElement) As Boolean
        Select Case VpElement.ElementType
            Case clsElement.eElementType.Card
                Return VpCard.CardName = VpElement.ElementValue
            Case clsElement.eElementType.Type
                Return VpCard.CardType = VpElement.ElementValue
            Case clsElement.eElementType.SubType
                Return VpCard.CardSubType = VpElement.ElementValue
            Case clsElement.eElementType.Cost
                Return VpCard.CardMyCost = VpElement.ElementValue
            Case clsElement.eElementType.Color
                Return VpCard.CardColor = VpElement.ElementValue
            Case Else
        End Select
    End Function
    Private Function IsDrawn(VpElement As clsElement) As Boolean
    '-----------------------------------------------
    'Renvoie vrai si l'élément spécifié a été pioché
    '-----------------------------------------------
        For Each VpCard As clsCard In VmDrawn
            If Me.IsDrawnMatching(VpCard, VpElement) And Not VpCard.Tagged Then
                VpCard.Tagged = True
                Return True
            End If
        Next VpCard
        Return False
    End Function
    Public Function ManasPotentielIn(VpList As List(Of clsCard)) As Integer
    '-----------------------------------------------------------------------------
    'Retourne le nombre de manas potentiellement générables avec les cartes en jeu
    '-----------------------------------------------------------------------------
    Dim VpPot As Integer = 0
        For Each VpCard As clsCard In VpList
            If (Not VpCard.Tapped) AndAlso VpCard.ManaAble Then
                VpPot = VpPot + VpCard.ManasPot
            End If
        Next VpCard
        Return VpPot
    End Function
    Public Function GetMissingCost(VpList As List(Of clsCard), VpPrev As Integer) As Integer
    '-----------------------------------------------------------------
    'Retourne le nombre de manas dont on manque pour le tour précédent
    '-----------------------------------------------------------------
    Dim VpSum As Integer = 0
    Dim VpCard As clsCard
    Dim VpJ As Integer
        VpJ = Math.Max(VpList.Count - clsModule.CgNMain, 0)
        For VpI As Integer = VpJ To VpList.Count - 1
            VpCard = VpList.Item(VpI)
            If Not ( VpCard.ManaAble Or VpCard.IsSpecial ) Then
                VpSum = VpSum + VpCard.ManasInvoc.Potentiel
            End If
        Next VpI
        Return Math.Max(VpSum - VpPrev, 0)
    End Function
    Public Function GetDistinctCards As List(Of clsCard)
    '-----------------------------------------------------
    'Retourne une liste des cartes distinctes dans le deck
    '-----------------------------------------------------
    Dim VpDistincts As New List(Of clsCard)
    Dim VpAlready As Boolean
        For Each VpCard As clsCard In Me.CardsInFullDeck
            VpAlready = False
            For Each VpIn As clsCard In VpDistincts
                If VpCard.CardName = VpIn.CardName Then
                    VpAlready = True
                    Exit For
                End If
            Next VpIn
            If Not VpAlready Then
                VpDistincts.Add(VpCard)
            End If
        Next VpCard
        Return VpDistincts
    End Function
    Public ReadOnly Property CardsCount As Integer
        Get
            Return VmDeckCopy.Count
        End Get
    End Property
    Public ReadOnly Property CardsDrawn As List(Of clsCard)
        Get
            Return VmDrawn
        End Get
    End Property
    Public ReadOnly Property CardsInDeck As List(Of clsCard)
        Get
            Return VmDeck
        End Get
    End Property
    Public ReadOnly Property CardsInFullDeck As List(Of clsCard)
        Get
            Return VmDeckCopy
        End Get
    End Property
    Public ReadOnly Property CardsInPlay As List(Of clsCard)
        Get
            Return VmInPlay
        End Get
    End Property
    Public ReadOnly Property CardsInRound As List(Of clsCard)
        Get
            Return VmInRound
        End Get
    End Property
    Public ReadOnly Property Reserve As clsManas
        Get
            Return VmReserve
        End Get
    End Property
End Class
