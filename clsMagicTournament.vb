'------------------------------------------------------
'| Projet         |  Magic The Gathering Manager      |
'| Contexte       |  		Perso                     |
'| Date           |                        30/03/2008 |
'| Release 1      |                        12/04/2008 |
'| Release 2      |                        30/08/2008 |
'| Release 3      |                        08/11/2008 |
'| Release 4      |                        29/08/2009 |
'| Release 5      |                        21/03/2010 |
'| Release 6      |                        17/04/2010 |
'| Release 7      |                        29/07/2010 |
'| Release 8      |                        03/10/2010 |
'| Release 9      |                        05/02/2011 |
'| Release 10     |                        10/09/2011 |
'| Release 11     |                        24/01/2012 |
'| Release 12     |                        01/10/2012 |
'| Release 13     |                        09/05/2014 |
'| Release 14     |                        09/05/2015 |
'| Release 15     |                        15/01/2017 |
'| Auteur         |         				     Ghis |
'|----------------------------------------------------|
'| Modifications :                                    |
'| -								       28/09/2018 |
'------------------------------------------------------
Imports System
Imports System.Collections.Generic
Imports System.Linq
Public Class clsMagicTournament
	' Variables privées de la classe
	Private _matchups As New List(Of clsMatchup)()
	' Méthodes publiques
	Public Function CreateMatchup(deck1 As String, deck2 As String, victory1 As Integer, victory2 As Integer) As clsMatchup
		Dim mu As New clsMatchup(deck1, deck2, victory1, victory2)
		_matchups.Add(mu)
		Return mu
	End Function
	Public Function GetDuelList() As List(Of clsMatchup)
		' Générateur de nombres aléatoires
		Dim rnd As New Random()
		' Facteur de réduction de la probabilité de jouer un deck en fonction du nombre de fois où il a déjà été joué
		Dim factReduc As Integer = 5
		' => Concrètement, si un jeu a déjà été sélectionné deux fois, il aura 5^2 = 25 fois moins de chance de sortir
		Dim factFavor As Integer = 1
		' Les decks à favoriser ont un facteur puissance supplémentaire de sortir
		Dim coefFavor As Double = 0.6
		' Si le deck a joué moins de 60% du nombre de parties jouées en moyenne par les decks, il faut le favoriser
		' Initialise un dictionnaire permettant de suivre le nombre de duel impliquant chaque jeu (avec pour l'instant 0 partie par deck)
		Dim Decks As New Dictionary(Of String, clsDeck)()
		' Commence par calculer la probabilité de base d'apparition de chaque matchup dans le tirage
		' En pratique, c'est (le nombre maximal de confrontations rencontré sur les matchups) + 1 - (nombre de confrontations du matchup)
		' Par exemple, si le matchup le plus joué a donné lieu à 30 manches :
		' - ce matchup le plus joué aura 1 chance de sortir
		' - un matchup joué 10 fois aura 21 chances de sortir (30 + 1 - 10)
		Dim maxConfront As Integer = _matchups.[Select](Function(x) x.Deck1Victory + x.Deck2Victory).Max()
		' Traitement d'initialisation des matchups
		For Each mu As clsMatchup In _matchups
			' Initialisation du statut "joué"
			mu.Played = False
			' Probabilité de base
			mu.ProbaBase = maxConfront + 1 - mu.Deck1Victory - mu.Deck2Victory
			' Profite de l'occasion pour recenser les noms des decks
			If Not Decks.ContainsKey(mu.Deck1) Then
				Decks.Add(mu.Deck1, New clsDeck())
			End If
			If Not Decks.ContainsKey(mu.Deck2) Then
				Decks.Add(mu.Deck2, New clsDeck())
			End If
			' ... et comptabiliser le nombre historique de parties jouées par chaque deck
			Decks(mu.Deck1).NbGamesHisto += mu.Deck1Victory + mu.Deck2Victory
			Decks(mu.Deck2).NbGamesHisto += mu.Deck1Victory + mu.Deck2Victory
		Next
		' Détermine la liste des decks à favoriser
		Dim avgPlayed As Double = Decks.Values.[Select](Function(x) x.NbGamesHisto).Average()
		For Each deck As clsDeck In Decks.Values.Where(Function(x) x.NbGamesHisto < (coefFavor * avgPlayed))
			deck.Favor = factFavor
		Next
		' Crée la liste des duels à jouer
		Dim duels As New List(Of clsMatchup)()
		' Effectue une boucle jusqu'à ce que tous les matchups aient été joués
		While _matchups.Where(Function(x) Not x.Played).Count() > 0
			' Quel jeu a été le plus joué ?
			Dim maxPlayed As Integer = Decks.Values.[Select](Function(x) x.NbGames).Max()
			' Le nombre de chances de jouer un jeu égale
			' (proba de base x factreduc^retard) où retard indique combien ce jeu a de parties de retard sur le jeu le plus joué
			' Par exemple, si la première partie jouée est "Death & Taxes vs Pilotage", alors pour le second tirage tous les matchups n'intégrant pas
			' Pilotage auront 5 fois plus de chance de sortir. De même pour les matchups n'intégrant pas Death & Taxes. Donc les matchups ne faisant
			' intervenir ni l'un ni l'autre ont concrètement 25 fois plus de chance de sortir
			For Each mu As clsMatchup In _matchups
				' NB : les matchups déjà joués n'ont aucune chance de sortir
				mu.ProbaModif = (If(mu.Played, 0, 1)) * mu.ProbaBase * Math.Pow(factReduc, (2 * maxPlayed + Decks(mu.Deck1).Favor + Decks(mu.Deck2).Favor - Decks(mu.Deck1).NbGames - Decks(mu.Deck2).NbGames))
			Next
			' Ensuite on détermine le total de chances de sortir
			Dim nb As Double = _matchups.[Select](Function(x) x.ProbaModif).Sum()
			' Puis on tire au sort un chiffre entier allant de 1 à nb
			Dim tirage As Double = Math.Truncate(rnd.NextDouble() * nb) + 1
			Dim offset As Integer = 0
			' Et on détermine à quel matchup il correspond
			While True
				tirage -= _matchups(offset).ProbaModif
				If tirage <= 0 Then
					' Le matchup a été trouvé
					duels.Add(_matchups(offset))
					Decks(_matchups(offset).Deck1).NbGames += 1
					Decks(_matchups(offset).Deck2).NbGames += 1
					_matchups(offset).Played = True
					Exit While
				End If
				offset += 1
			End While
		End While
		' Renvoie les duels
		Return duels
	End Function
End Class
Public Class clsMatchup
	Private m_Deck1 As String
	Private m_Deck2 As String
	Private m_Deck1Victory As Integer
	Private m_Deck2Victory As Integer
	Private m_ProbaBase As Integer
	Private m_ProbaModif As Double
	Private m_Played As Boolean
	Public Sub New(deck1 As String, deck2 As String, victory1 As Integer, victory2 As Integer)
		m_Deck1 = deck1
		m_Deck2 = deck2
		m_Deck1Victory = victory1
		m_Deck2Victory = victory2
	End Sub
	Public Property Deck1() As String
		Get
			Return m_Deck1
		End Get
		Set
			m_Deck1 = Value
		End Set
	End Property
	Public Property Deck2() As String
		Get
			Return m_Deck2
		End Get
		Set
			m_Deck2 = Value
		End Set
	End Property
	Public Property Deck1Victory() As Integer
		Get
			Return m_Deck1Victory
		End Get
		Set
			m_Deck1Victory = Value
		End Set
	End Property
	Public Property Deck2Victory() As Integer
		Get
			Return m_Deck2Victory
		End Get
		Set
			m_Deck2Victory = Value
		End Set
	End Property
	Public ReadOnly Property DeckConfrontations() As Integer
		Get
			Return m_Deck1Victory + m_Deck2Victory
		End Get
	End Property
	Public Property ProbaBase() As Integer
		Get
			Return m_ProbaBase
		End Get
		Set
			m_ProbaBase = Value
		End Set
	End Property
	Public Property ProbaModif() As Double
		Get
			Return m_ProbaModif
		End Get
		Set
			m_ProbaModif = Value
		End Set
	End Property
	Public Property Played() As Boolean
		Get
			Return m_Played
		End Get
		Set
			m_Played = Value
		End Set
	End Property
End Class
Public Class clsDeck
	Private m_Name As String
	Private m_NbGamesHisto As Integer
	Private m_NbGames As Integer
	Private m_Favor As Integer
	Public Sub New
		m_NbGamesHisto = 0
		m_NbGames = 0
		m_Favor = 0
	End Sub
	Public Property Name() As String
		Get
			Return m_Name
		End Get
		Set
			m_Name = Value
		End Set
	End Property
	Public Property NbGamesHisto() As Integer
		Get
			Return m_NbGamesHisto
		End Get
		Set
			m_NbGamesHisto = Value
		End Set
	End Property
	Public Property NbGames() As Integer
		Get
			Return m_NbGames
		End Get
		Set
			m_NbGames = Value
		End Set
	End Property
	Public Property Favor() As Integer
		Get
			Return m_Favor
		End Get
		Set
			m_Favor = Value
		End Set
	End Property
End Class
