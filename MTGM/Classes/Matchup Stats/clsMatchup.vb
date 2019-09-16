Public Class clsMatchup
    Private m_Deck1 As String
    Private m_Deck2 As String
    Private m_Deck1Victory As Integer
    Private m_Deck2Victory As Integer
    Private m_ProbaBase As Integer
    Private m_ProbaModif As Single
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
    Public Property ProbaModif() As Single
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
