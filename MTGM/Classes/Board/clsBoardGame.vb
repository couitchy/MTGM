Public Class clsBoardGame
    Private VmDeck As New List(Of clsBoardCard)
    Private VmBibli As New List(Of clsBoardCard)
    Private VmRegard As New List(Of clsBoardCard)
    Private VmMain As New List(Of clsBoardCard)
    Private VmField As New List(Of clsBoardCard)
    Private VmGraveyard As New List(Of clsBoardCard)
    Private VmExil As New List(Of clsBoardCard)
    Private VmMulligan As Integer = 0
    Private VmLives As Integer
    Private VmPoisons As Integer
    Private VmTours As Integer
    Public Sub New(VpSource As String, VpRestriction As String)
    '-------------------
    'Construction du jeu
    '-------------------
    Dim VpSQL As String
    Dim VpReserve As Boolean = False
        VpSQL = "Select Card.Title, " + VpSource + ".Items, CardFR.TitleFR, Card.Type, Card.SpecialDoubleCard" + If(VpSource = clsModule.CgSDecks, ", Reserve", "") + " From (Card Inner Join " + VpSource + " On " + VpSource + ".EncNbr = Card.EncNbr) Inner Join CardFR On Card.EncNbr = CardFR.EncNbr Where "
        VpSQL = VpSQL + VpRestriction
        VpSQL = clsModule.TrimQuery(VpSQL)
        VgDBCommand.CommandText = VpSQL
        VgDBReader = VgDBCommand.ExecuteReader
        With VgDBReader
            While .Read
                If VpSource = clsModule.CgSDecks Then
                    VpReserve = .GetBoolean(5)
                End If
                'Carte normale
                If Not .GetBoolean(4) Then
                    Call Me.AddCard(.GetString(0), .GetString(2), .GetInt32(1), .GetString(3), False, .GetString(0), VpReserve)
                'Carte transformable
                Else
                    Call Me.AddCard(.GetString(0), .GetString(2), .GetInt32(1), .GetString(3), True, clsModule.GetTransformedName(.GetString(0)), VpReserve)
                End If
            End While
            .Close
        End With
    End Sub
    Private Sub AddCard(VpName As String, VpNameFR As String, VpCount As Integer, VpType As String, VpTransformable As Boolean, VpTransformedCardName As String, VpReserve As Boolean)
    '--------------------
    'Construction du deck
    '--------------------
        For VpI As Integer = 1 To VpCount
            VmDeck.Add(New clsBoardCard(VmDeck, VpName, VpNameFR, VpType, VpTransformable, VpTransformedCardName, VpReserve))
        Next VpI
    End Sub
    Public Sub BeginPlateauPartie
    '-------------------------------
    'Démarrage d'une nouvelle partie
    '-------------------------------
        VmBibli.Clear
        VmRegard.Clear
        VmMain.Clear
        VmField.Clear
        VmGraveyard.Clear
        VmExil.Clear
        For Each VpCard As clsBoardCard In VmDeck
            If Not VpCard.InReserve Then
                Call VpCard.ReInit(VmBibli)
                VmBibli.Add(VpCard)
            Else
                Call VpCard.ReInit(VmDeck)
            End If
        Next VpCard
        Call Shuffle(VmBibli)
        VmMain.AddRange(VmBibli.GetRange(0, clsModule.CgNMain - VmMulligan))
        VmBibli.RemoveRange(0, clsModule.CgNMain - VmMulligan)
        For Each VpCard As clsBoardCard In VmMain
            VpCard.Hidden = False
            VpCard.Owner = VmMain
        Next VpCard
        VmLives = clsModule.CgNLives
        VmPoisons = 0
        VmTours = 0
    End Sub
    Public Function GetReserve As List(Of clsBoardCard)
    '---------------------------------
    'Retourne les cartes de la réserve
    '---------------------------------
    Dim VpReserve As New List(Of clsBoardCard)
        For Each VpCard As clsBoardCard In VmDeck
            If VpCard.InReserve Then
                VpReserve.Add(VpCard)
            End If
        Next VpCard
        Return VpReserve
    End Function
    Public Shared Sub Shuffle(ByRef VpListe As List(Of clsBoardCard))
    '----------------------------------------
    'Mélange la sélection passée en paramètre
    '----------------------------------------
    Dim VpI As Integer
    Dim VpRandomPos As New SortedList(VpListe.Count)
    Dim VpShuffled As New List(Of clsBoardCard)(VpListe.Count)
        'Génère un tableau trié de nombres aléatoires
        For VpI = 0 To VpListe.Count - 1
            VpRandomPos.Add(clsModule.VgRandom.NextDouble, VpI)
        Next VpI
        'Réordonne les cartes en conséquence
        VpI = 0
        For Each VpPos As Integer In VpRandomPos.Values
            VpListe.Item(VpPos).Owner = VpShuffled          'eh oui : quand on mélange la liste, on change de handle, et il faut pas perdre le nouveau !
            VpShuffled.Insert(VpI, VpListe.Item(VpPos))
            VpI = VpI + 1
        Next VpPos
        VpListe = VpShuffled
    End Sub
    #Region "Propriétés"
    Public Property Bibli As List(Of clsBoardCard)
        Get
            Return VmBibli
        End Get
        Set (VpBibli As List(Of clsBoardCard))
            VmBibli = VpBibli
        End Set
    End Property
    Public Property Regard As List(Of clsBoardCard)
        Get
            Return VmRegard
        End Get
        Set (VpRegard As List(Of clsBoardCard))
            VmRegard = VpRegard
        End Set
    End Property
    Public Property Main As List(Of clsBoardCard)
        Get
            Return VmMain
        End Get
        Set (VpMain As List(Of clsBoardCard))
            VmMain = VpMain
        End Set
    End Property
    Public Property Field As List(Of clsBoardCard)
        Get
            Return VmField
        End Get
        Set (VpField As List(Of clsBoardCard))
            VmField = VpField
        End Set
    End Property
    Public Property Graveyard As List(Of clsBoardCard)
        Get
            Return VmGraveyard
        End Get
        Set (VpGraveyard As List(Of clsBoardCard))
            VmGraveyard = VpGraveyard
        End Set
    End Property
    Public Property Exil As List(Of clsBoardCard)
        Get
            Return VmExil
        End Get
        Set (VpExil As List(Of clsBoardCard))
            VmExil = VpExil
        End Set
    End Property
    Public ReadOnly Property BibliTop As clsBoardCard
        Get
            If VmBibli.Count > 0 Then
                Return VmBibli.Item(0)
            Else
                Return Nothing
            End If
        End Get
    End Property
    Public ReadOnly Property GraveyardTop As clsBoardCard
        Get
            If VmGraveyard.Count > 0 Then
                Return VmGraveyard.Item(VmGraveyard.Count - 1)
            Else
                Return Nothing
            End If
        End Get
    End Property
    Public ReadOnly Property ExilTop As clsBoardCard
        Get
            If VmExil.Count > 0 Then
                Return VmExil.Item(VmExil.Count - 1)
            Else
                Return Nothing
            End If
        End Get
    End Property
    Public Property Mulligan As Integer
        Get
            Return VmMulligan
        End Get
        Set (VpMulligan As Integer)
            VmMulligan = VpMulligan
        End Set
    End Property
    Public Property Lives As Integer
        Get
            Return VmLives
        End Get
        Set (VpLives As Integer)
            VmLives = VpLives
        End Set
    End Property
    Public Property Poisons As Integer
        Get
            Return VmPoisons
        End Get
        Set (VpPoisons As Integer)
            VmPoisons = VpPoisons
        End Set
    End Property
    Public Property Tours As Integer
        Get
            Return VmTours
        End Get
        Set (VpTours As Integer)
            VmTours = VpTours
        End Set
    End Property
    Public ReadOnly Property CardsInDeck As List(Of clsBoardCard)
        Get
            Return VmDeck
        End Get
    End Property
    #End Region
End Class
