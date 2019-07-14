''' <summary>
''' Custom property class
''' </summary>
Public Class CustomProperty
    Private sName As String = String.Empty
    Private sCategory As String = String.Empty
    Private bReadOnly As Boolean = False
    Private bVisible As Boolean = True
    Private objValue As Object = Nothing

    Public Sub New(sName As String, sCategory As String, value As Object, type As Type, bReadOnly As Boolean, bVisible As Boolean)
        Me.sName = sName
        Me.sCategory = sCategory
        Me.objValue = value
        Me.m_type = type
        Me.bReadOnly = bReadOnly
        Me.bVisible = bVisible
    End Sub

    Private m_type As Type
    Public ReadOnly Property Type() As Type
        Get
            Return m_type
        End Get
    End Property

    Public ReadOnly Property [ReadOnly]() As Boolean
        Get
            Return bReadOnly
        End Get
    End Property

    Public ReadOnly Property Name() As String
        Get
            Return sName
        End Get
    End Property

    Public Property Category() As String
        Get
            Return sCategory
        End Get
        Set
            sCategory = value
        End Set
    End Property

    Public ReadOnly Property Visible() As Boolean
        Get
            Return bVisible
        End Get
    End Property

    Public Property Value() As Object
        Get
            Return objValue
        End Get
        Set
            objValue = value
        End Set
    End Property

End Class
