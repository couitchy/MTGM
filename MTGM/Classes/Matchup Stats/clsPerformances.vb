Public Class clsPerformances
    Public Shared Function GetAllDecks As String()
    '-----------------------------------------------------------------
    'Retourne le nom de tous les jeux en présence (locaux et adverses)
    '-----------------------------------------------------------------
    Dim VpGames As New List(Of String)
        VgDBCommand.CommandText = "Select JeuLocal, JeuAdverse From MyScores;"
        VgDBReader = VgDBCommand.ExecuteReader
        With VgDBReader
            While .Read
                'Ajoute distinctement les jeux locaux
                If Not VpGames.Contains(.GetString(0)) Then
                    VpGames.Add(.GetString(0))
                End If
                'Ajoute distinctement les jeux adverses
                If Not VpGames.Contains(.GetString(1)) Then
                    VpGames.Add(.GetString(1))
                End If
            End While
            .Close
        End With
        Return VpGames.ToArray
    End Function
    Public Shared Function GetActiveDecks As String()
    '------------------------------------------------------------------
    'Retourne le nom de tous les jeux effectivement saisis dans la base
    '------------------------------------------------------------------
    Dim VpGames As New List(Of String)
        VgDBCommand.CommandText = "Select GameName From MyGamesID Where IsFolder = False;"
        VgDBReader = VgDBCommand.ExecuteReader
        With VgDBReader
            While .Read
                VpGames.Add(.GetString(0))
            End While
            .Close
        End With
        Return VpGames.ToArray
    End Function
    Public Shared Function GetAdvDecks(VpAdvName As String) As String()
    '---------------------------------------------------------
    'Retourne le nom des decks du joueur spécifié en paramètre
    '---------------------------------------------------------
    Dim VpGames As New List(Of String)
        VgDBCommand.CommandText = "Select GameName From MyGamesID Inner Join MyAdversairesID On MyGamesID.AdvID = MyAdversairesID.AdvID Where AdvName = '" + VpAdvName.Replace("'", "''") + "' And IsFolder = False;"
        VgDBReader = VgDBCommand.ExecuteReader
        With VgDBReader
            While .Read
                VpGames.Add(.GetString(0))
            End While
            .Close
        End With
        Return VpGames.ToArray
    End Function
    Public Shared Function GetNPlayed(VpGame1 As String, Optional VpGame2 As String = "") As Integer
    '---------------------------------------------------------------------------------------------------------------------
    'Retourne le nombre de parties jouées par le jeu passé en paramètre, ou dans l'absolu, ou contre l'adversaire spécifié
    '---------------------------------------------------------------------------------------------------------------------
    Dim VpP As Integer
        'Cas 1 : suppose que le jeu 1 est local et le jeu 2 est adverse
        VgDBCommand.CommandText = "Select Count(*) From MyScores Where JeuLocal = '" + VpGame1.Replace("'", "''") + "'" + If(VpGame2 <> "", " And JeuAdverse = '" + VpGame2.Replace("'", "''") + "';", ";")
        VpP = VgDBCommand.ExecuteScalar
        'Cas 2 : suppose que le jeu 1 est adverse et le jeu 2 est local
        VgDBCommand.CommandText = "Select Count(*) From MyScores Where JeuAdverse = '" + VpGame1.Replace("'", "''") + "'" + If(VpGame2 <> "", " And JeuLocal = '" + VpGame2.Replace("'", "''") + "';", ";")
        Return VpP + VgDBCommand.ExecuteScalar
    End Function
    Public Shared Function GetNVictoires(VpGame1 As String, Optional VpGame2 As String = "") As Integer
    '----------------------------------------------------------------------------------------------------------------------
    'Retourne le nombre de parties gagnées par le jeu passé en paramètre, ou dans l'absolu, ou contre l'adversaire spécifié
    '----------------------------------------------------------------------------------------------------------------------
    Dim VpP As Integer
        'Cas 1 : suppose que le jeu 1 est local et le jeu 2 est adverse
        VgDBCommand.CommandText = "Select Count(*) From MyScores Where JeuLocal = '" + VpGame1.Replace("'", "''") + "'" + If(VpGame2 <> "", " And JeuAdverse = '" + VpGame2.Replace("'", "''") + "'", "") + " And Victoire = True;"
        VpP = VgDBCommand.ExecuteScalar
        'Cas 2 : suppose que le jeu 1 est adverse et le jeu 2 est local
        VgDBCommand.CommandText = "Select Count(*) From MyScores Where JeuAdverse = '" + VpGame1.Replace("'", "''") + "'" + If(VpGame2 <> "", " And JeuLocal = '" + VpGame2.Replace("'", "''") + "'", "") + " And Victoire = False;"
        Return VpP + VgDBCommand.ExecuteScalar
    End Function
    Public Shared Function GetNDefaites(VpGame1 As String, Optional VpGame2 As String = "") As Integer
    '----------------------------------------------------------------------------------------------------------------------
    'Retourne le nombre de parties perdues par le jeu passé en paramètre, ou dans l'absolu, ou contre l'adversaire spécifié
    '----------------------------------------------------------------------------------------------------------------------
    Dim VpP As Integer
        'Cas 1 : suppose que le jeu 1 est local et le jeu 2 est adverse
        VgDBCommand.CommandText = "Select Count(*) From MyScores Where JeuLocal = '" + VpGame1.Replace("'", "''") + "'" + If(VpGame2 <> "", " And JeuAdverse = '" + VpGame2.Replace("'", "''") + "'", "") + " And Victoire = False;"
        VpP = VgDBCommand.ExecuteScalar
        'Cas 2 : suppose que le jeu 1 est adverse et le jeu 2 est local
        VgDBCommand.CommandText = "Select Count(*) From MyScores Where JeuAdverse = '" + VpGame1.Replace("'", "''") + "'" + If(VpGame2 <> "", " And JeuLocal = '" + VpGame2.Replace("'", "''") + "'", "") + " And Victoire = True;"
        Return VpP + VgDBCommand.ExecuteScalar
    End Function
    Public Shared Function GetRatio(VpGame1 As String, Optional VpGame2 As String = "") As Single
    '------------------------------------------------------------------------------------------------------------------------
    'Retourne la fraction de parties gagnées par le jeu passé en paramètre, ou dans l'absolu, ou contre l'adversaire spécifié
    '------------------------------------------------------------------------------------------------------------------------
    Dim VpT As Integer = clsPerformances.GetNPlayed(VpGame1, VpGame2)
        If VpT > 0 Then
            Return clsPerformances.GetNVictoires(VpGame1, VpGame2) / VpT
        Else
            Return -1
        End If
    End Function
    Public Shared Function GetSimpleRatioMatrix(VpGames As String()) As Single()
    '-----------------------------------------------------------------------------------------------
    'Retourne la fraction de parties gagnées par le jeu i, 1<i<N, N nombre total de jeux en présence
    '-----------------------------------------------------------------------------------------------
    Dim VpMat() As Single
    Dim VpN As Integer
        VpN = VpGames.Length
        ReDim VpMat(0 To VpN - 1)
        For VpI As Integer = 0 To VpN - 1
            VpMat(VpI) = 100 * clsPerformances.GetRatio(VpGames(VpI))
        Next VpI
        Return VpMat
    End Function
    Public Shared Function GetRatioMatrix(VpGames As String()) As Single(,)
    '-------------------------------------------------------------------------------------------------------------------
    'Retourne la fraction de parties gagnées par le jeu i contre le jeu j, 1<(i,j)<N, N nombre total de jeux en présence
    '-------------------------------------------------------------------------------------------------------------------
    Dim VpMat(,) As Single
    Dim VpN As Integer
        VpN = VpGames.Length
        ReDim VpMat(0 To VpN - 1, 0 To VpN - 1)
        For VpI As Integer = 0 To VpN - 1
            For VpJ As Integer = 0 To VpN - 1
                If VpI = VpJ Then
                    VpMat(VpI, VpJ) = -1
                ElseIf VpJ > VpI Then
                    VpMat(VpI, VpJ) = clsPerformances.GetRatio(VpGames(VpI), VpGames(VpJ))
                Else
                    VpMat(VpI, VpJ) = If(VpMat(VpJ, VpI) = -1, -1, 1 - VpMat(VpJ, VpI))
                End If
            Next VpJ
        Next VpI
        Return VpMat
    End Function
    Public Shared Function Reshape(VpMatrix(,) As Single) As Single()
    '--------------------------------------------------------
    'Rapporte la fraction de victoires par couple en base 100
    '--------------------------------------------------------
    Dim VpN As Integer
    Dim VpNN As Integer
    Dim VpNewShape() As Single
        VpN = 1 + VpMatrix.GetUpperBound(0)
        ReDim VpNewShape(0 To VpN - 1)
        For VpI As Integer = 0 To VpN - 1
            VpNN = 0
            For VpJ As Integer = 0 To VpN - 1
                If VpMatrix(VpI, VpJ) <> - 1 Then
                    VpNewShape(VpI) = VpNewShape(VpI) + VpMatrix(VpI, VpJ)
                    VpNN = VpNN + 1
                End If
            Next VpJ
            VpNewShape(VpI) = 100 * VpNewShape(VpI) / VpNN
        Next VpI
        Return VpNewShape
    End Function
    Public Shared Function GetPrice(VpGame As String) As Single
    '------------------------------------------
    'Retourne le prix du jeu passé en paramètre
    '------------------------------------------
    Dim VpId As String
        VpId = mdlToolbox.GetDeckIdFromName(VpGame)
        If VpId <> "" Then
            Try
                VgDBCommand.CommandText = "Select Sum(Price * Items) From Card Inner Join MyGames On Card.EncNbr = MyGames.EncNbr Where GameID = " + VpId + ";"
                Return VgDBCommand.ExecuteScalar
            Catch
                Return -1
            End Try
        Else
            Return -1
        End If
    End Function
    Public Shared Function GetMeanPrice(VpGames As String()) As Single
    '-------------------------------------------
    'Retourne le prix moyen des jeux en présence
    '-------------------------------------------
    Dim VpSum As Single = 0
    Dim VpCur As Single
    Dim VpN As Integer = 0
        For Each VpGame As String In VpGames
            VpCur = clsPerformances.GetPrice(VpGame)
            If VpCur <> -1 Then
                VpSum = VpSum + VpCur
                VpN = VpN + 1
            End If
        Next VpGame
        If VpN <> 0 Then
            Return VpSum / VpN
        Else
            Return -1
        End If
    End Function
End Class
