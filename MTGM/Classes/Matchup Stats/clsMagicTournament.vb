Imports System
Imports System.Collections.Generic
Imports System.Linq
Public Class clsMagicTournament
    ' Variables priv�es de la classe
    Private _matchups As New List(Of clsMatchup)()
    ' M�thodes publiques
    Public Function CreateMatchup(deck1 As String, deck2 As String, victory1 As Integer, victory2 As Integer) As clsMatchup
        Dim mu As New clsMatchup(deck1, deck2, victory1, victory2)
        _matchups.Add(mu)
        Return mu
    End Function
    Public Function GetDuelList() As List(Of clsMatchup)
        ' G�n�rateur de nombres al�atoires
        Dim rnd As New Random()
        ' Facteur de r�duction de la probabilit� de jouer un deck en fonction du nombre de fois o� il a d�j� �t� jou�
        Dim factReduc As Integer = 5
        ' => Concr�tement, si un jeu a d�j� �t� s�lectionn� deux fois, il aura 5^2 = 25 fois moins de chance de sortir
        Dim factFavor As Integer = 1
        ' Les decks � favoriser ont un facteur puissance suppl�mentaire de sortir
        Dim coefFavor As Single = 0.6
        ' Si le deck a jou� moins de 60% du nombre de parties jou�es en moyenne par les decks, il faut le favoriser
        ' Initialise un dictionnaire permettant de suivre le nombre de duel impliquant chaque jeu (avec pour l'instant 0 partie par deck)
        Dim Decks As New Dictionary(Of String, clsDeck)()
        ' Commence par calculer la probabilit� de base d'apparition de chaque matchup dans le tirage
        ' En pratique, c'est (le nombre maximal de confrontations rencontr� sur les matchups) + 1 - (nombre de confrontations du matchup)
        ' Par exemple, si le matchup le plus jou� a donn� lieu � 30 manches :
        ' - ce matchup le plus jou� aura 1 chance de sortir
        ' - un matchup jou� 10 fois aura 21 chances de sortir (30 + 1 - 10)
        Dim maxConfront As Integer = _matchups.[Select](Function(x) x.Deck1Victory + x.Deck2Victory).Max()
        ' Traitement d'initialisation des matchups
        For Each mu As clsMatchup In _matchups
            ' Initialisation du statut "jou�"
            mu.Played = False
            ' Probabilit� de base
            mu.ProbaBase = maxConfront + 1 - mu.Deck1Victory - mu.Deck2Victory
            ' Profite de l'occasion pour recenser les noms des decks
            If Not Decks.ContainsKey(mu.Deck1) Then
                Decks.Add(mu.Deck1, New clsDeck())
            End If
            If Not Decks.ContainsKey(mu.Deck2) Then
                Decks.Add(mu.Deck2, New clsDeck())
            End If
            ' ... et comptabiliser le nombre historique de parties jou�es par chaque deck
            Decks(mu.Deck1).NbGamesHisto += mu.Deck1Victory + mu.Deck2Victory
            Decks(mu.Deck2).NbGamesHisto += mu.Deck1Victory + mu.Deck2Victory
        Next
        ' D�termine la liste des decks � favoriser
        Dim avgPlayed As Single = Decks.Values.[Select](Function(x) x.NbGamesHisto).Average()
        For Each deck As clsDeck In Decks.Values.Where(Function(x) x.NbGamesHisto < (coefFavor * avgPlayed))
            deck.Favor = factFavor
        Next
        ' Cr�e la liste des duels � jouer
        Dim duels As New List(Of clsMatchup)()
        ' Effectue une boucle jusqu'� ce que tous les matchups aient �t� jou�s
        While _matchups.Where(Function(x) Not x.Played).Count() > 0
            ' Quel jeu a �t� le plus jou� ?
            Dim maxPlayed As Integer = Decks.Values.[Select](Function(x) x.NbGames).Max()
            ' Le nombre de chances de jouer un jeu �gale
            ' (proba de base x factreduc^retard) o� retard indique combien ce jeu a de parties de retard sur le jeu le plus jou�
            ' Par exemple, si la premi�re partie jou�e est "Death & Taxes vs Pilotage", alors pour le second tirage tous les matchups n'int�grant pas
            ' Pilotage auront 5 fois plus de chance de sortir. De m�me pour les matchups n'int�grant pas Death & Taxes. Donc les matchups ne faisant
            ' intervenir ni l'un ni l'autre ont concr�tement 25 fois plus de chance de sortir
            For Each mu As clsMatchup In _matchups
                ' NB : les matchups d�j� jou�s n'ont aucune chance de sortir
                mu.ProbaModif = (If(mu.Played, 0, 1)) * mu.ProbaBase * Math.Pow(factReduc, (2 * maxPlayed + Decks(mu.Deck1).Favor + Decks(mu.Deck2).Favor - Decks(mu.Deck1).NbGames - Decks(mu.Deck2).NbGames))
            Next
            ' Ensuite on d�termine le total de chances de sortir
            Dim nb As Single = _matchups.[Select](Function(x) x.ProbaModif).Sum()
            ' Puis on tire au sort un chiffre entier allant de 1 � nb
            Dim tirage As Single = Math.Truncate(rnd.NextDouble() * nb) + 1
            Dim offset As Integer = 0
            ' Et on d�termine � quel matchup il correspond
            While True
                tirage -= _matchups(offset).ProbaModif
                If tirage <= 0 Then
                    ' Le matchup a �t� trouv�
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
