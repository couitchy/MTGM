Public Class clsGame
    Public LocaleVersion As Date
    Public AdverseVersion As Date
    Public VicLocale As Boolean
    Public Sub New(VpLocaleVersion As Date, VpAdverseVersion As Date, VpVicLocale As Boolean)
        Me.LocaleVersion = VpLocaleVersion
        Me.AdverseVersion = VpAdverseVersion
        Me.VicLocale = VpVicLocale
    End Sub
End Class
