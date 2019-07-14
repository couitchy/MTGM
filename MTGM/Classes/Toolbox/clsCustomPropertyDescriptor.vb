Imports System.ComponentModel
''' <summary>
''' Custom PropertyDescriptor
''' </summary>
Public Class clsCustomPropertyDescriptor
    Inherits PropertyDescriptor
    Private m_Property As clsCustomProperty
    Public Sub New(ByRef myProperty As clsCustomProperty, attrs As Attribute())
        MyBase.New(myProperty.Name, attrs)
        m_Property = myProperty
    End Sub

    #Region "PropertyDescriptor specific"

    Public Overrides Function CanResetValue(component As Object) As Boolean
        Return False
    End Function

    Public Overrides ReadOnly Property ComponentType() As Type
        Get
            Return Nothing
        End Get
    End Property

    Public Overrides Function GetValue(component As Object) As Object
        Return m_Property.Value
    End Function

    Public Overrides ReadOnly Property Description() As String
        Get
            Return m_Property.Name
        End Get
    End Property

    Public Overrides ReadOnly Property Category() As String
        Get
            Return m_Property.Category
        End Get
    End Property

    Public Overrides ReadOnly Property DisplayName() As String
        Get
            Return m_Property.Name
        End Get
    End Property

    Public Overrides ReadOnly Property IsReadOnly() As Boolean
        Get
            Return m_Property.[ReadOnly]
        End Get
    End Property

    Public Overrides Sub ResetValue(component As Object)
        'Have to implement
    End Sub

    Public Overrides Function ShouldSerializeValue(component As Object) As Boolean
        Return False
    End Function

    Public Overrides Sub SetValue(component As Object, value As Object)
        m_Property.Value = value
    End Sub

    Public Overrides ReadOnly Property PropertyType() As Type
        Get
            Return m_Property.Type
        End Get
    End Property

    #End Region


End Class
