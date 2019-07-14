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
    Public Property Name As String
        Get
            Return m_Name
        End Get
        Set
            m_Name = Value
        End Set
    End Property
    Public Property NbGamesHisto As Integer
        Get
            Return m_NbGamesHisto
        End Get
        Set
            m_NbGamesHisto = Value
        End Set
    End Property
    Public Property NbGames As Integer
        Get
            Return m_NbGames
        End Get
        Set
            m_NbGames = Value
        End Set
    End Property
    Public Property Favor As Integer
        Get
            Return m_Favor
        End Get
        Set
            m_Favor = Value
        End Set
    End Property
End Class
