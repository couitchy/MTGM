Public Class clsGameCounter
    Private VmGames As New List(Of clsGame)
    Public Sub AddGame(VpLocaleVersion As Date, VpAdverseVersion As Date, VpVicLocale As Boolean)
        VmGames.Add(New clsGame(VpLocaleVersion, VpAdverseVersion, VpVicLocale))
    End Sub
    Public Function GetDistinctLocalVersions As Date()
    Dim VpDates As New List(Of Date)
        For Each VpGame As clsGame In VmGames
            If Not VpDates.Contains(VpGame.LocaleVersion) Then
                VpDates.Add(VpGame.LocaleVersion)
            End If
        Next VpGame
        VpDates.Sort(New clsDateComparer)
        Return VpDates.ToArray
    End Function
    Public Function GetDistinctAdverseVersions As Date()
    Dim VpDates As New List(Of Date)
        For Each VpGame As clsGame In VmGames
            If Not VpDates.Contains(VpGame.AdverseVersion) Then
                VpDates.Add(VpGame.AdverseVersion)
            End If
        Next VpGame
        VpDates.Sort(New clsDateComparer)
        Return VpDates.ToArray
    End Function
    Public Function GetNLocal(VpLocalVersion As Date) As Integer
    Dim VpN As Integer = 0
        For Each VpGame As clsGame In VmGames
            If VpGame.LocaleVersion = VpLocalVersion Then
                VpN = VpN + 1
            End If
        Next VpGame
        Return VpN
    End Function
    Public Function GetNVicLocal(VpLocalVersion As Date) As Integer
    Dim VpN As Integer = 0
        For Each VpGame As clsGame In VmGames
            If VpGame.LocaleVersion = VpLocalVersion And VpGame.VicLocale = True Then
                VpN = VpN + 1
            End If
        Next VpGame
        Return VpN
    End Function
    Public Function GetNAdverse(VpAdverseVersion As Date) As Integer
    Dim VpN As Integer = 0
        For Each VpGame As clsGame In VmGames
            If VpGame.AdverseVersion = VpAdverseVersion Then
                VpN = VpN + 1
            End If
        Next VpGame
        Return VpN
    End Function
    Public Function GetNVicAdverse(VpAdverseVersion As Date) As Integer
    Dim VpN As Integer = 0
        For Each VpGame As clsGame In VmGames
            If VpGame.AdverseVersion = VpAdverseVersion And VpGame.VicLocale = False Then
                VpN = VpN + 1
            End If
        Next VpGame
        Return VpN
    End Function
End Class
