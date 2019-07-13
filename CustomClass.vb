'------------------------------------------------------
'| Projet         |  Magic The Gathering Manager      |
'| Contexte       |         Perso                     |
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
'| Auteur         |                         Sreejumon |
'|----------------------------------------------------|
'| Modifications :                                    |
'| - gestion des catégories                14/02/2015 |
'------------------------------------------------------
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.ComponentModel
''' <summary>
''' CustomClass (Which is binding to property grid)
''' </summary>
Public Class CustomClass
    Inherits CollectionBase
    Implements ICustomTypeDescriptor
    ''' <summary>
    ''' Add CustomProperty to Collectionbase List
    ''' </summary>
    ''' <param name="Value"></param>
    Public Sub Add(Value As CustomProperty)
        MyBase.List.Add(Value)
    End Sub

    ''' <summary>
    ''' Remove item from List
    ''' </summary>
    ''' <param name="Name"></param>
    Public Sub Remove(Name As String)
        For Each prop As CustomProperty In MyBase.List
            If prop.Name = Name Then
                MyBase.List.Remove(prop)
                Return
            End If
        Next
    End Sub

    ''' <summary>
    ''' Indexer
    ''' </summary>
    Public Default Property Item(index As Integer) As CustomProperty
        Get
            Return DirectCast(MyBase.List(index), CustomProperty)
        End Get
        Set
            MyBase.List(index) = DirectCast(value, CustomProperty)
        End Set
    End Property


    #Region "TypeDescriptor Implementation"
    ''' <summary>
    ''' Get Class Name
    ''' </summary>
    ''' <returns>String</returns>
    Public Function GetClassName() As [String] Implements ICustomTypeDescriptor.GetClassName
        Return TypeDescriptor.GetClassName(Me, True)
    End Function

    ''' <summary>
    ''' GetAttributes
    ''' </summary>
    ''' <returns>AttributeCollection</returns>
    Public Function GetAttributes() As AttributeCollection Implements ICustomTypeDescriptor.GetAttributes
        Return TypeDescriptor.GetAttributes(Me, True)
    End Function

    ''' <summary>
    ''' GetComponentName
    ''' </summary>
    ''' <returns>String</returns>
    Public Function GetComponentName() As [String] Implements ICustomTypeDescriptor.GetComponentName
        Return TypeDescriptor.GetComponentName(Me, True)
    End Function

    ''' <summary>
    ''' GetConverter
    ''' </summary>
    ''' <returns>TypeConverter</returns>
    Public Function GetConverter() As TypeConverter Implements ICustomTypeDescriptor.GetConverter
        Return TypeDescriptor.GetConverter(Me, True)
    End Function

    ''' <summary>
    ''' GetDefaultEvent
    ''' </summary>
    ''' <returns>EventDescriptor</returns>
    Public Function GetDefaultEvent() As EventDescriptor Implements ICustomTypeDescriptor.GetDefaultEvent
        Return TypeDescriptor.GetDefaultEvent(Me, True)
    End Function

    ''' <summary>
    ''' GetDefaultProperty
    ''' </summary>
    ''' <returns>PropertyDescriptor</returns>
    Public Function GetDefaultProperty() As PropertyDescriptor Implements ICustomTypeDescriptor.GetDefaultProperty
        Return TypeDescriptor.GetDefaultProperty(Me, True)
    End Function

    ''' <summary>
    ''' GetEditor
    ''' </summary>
    ''' <param name="editorBaseType">editorBaseType</param>
    ''' <returns>object</returns>
    Public Function GetEditor(editorBaseType As Type) As Object Implements ICustomTypeDescriptor.GetEditor
        Return TypeDescriptor.GetEditor(Me, editorBaseType, True)
    End Function

    Public Function GetEvents(attributes As Attribute()) As EventDescriptorCollection Implements ICustomTypeDescriptor.GetEvents
        Return TypeDescriptor.GetEvents(Me, attributes, True)
    End Function

    Public Function GetEvents() As EventDescriptorCollection Implements ICustomTypeDescriptor.GetEvents
        Return TypeDescriptor.GetEvents(Me, True)
    End Function

    Public Function GetProperties(attributes As Attribute()) As PropertyDescriptorCollection Implements ICustomTypeDescriptor.GetProperties
        Dim newProps As PropertyDescriptor() = New PropertyDescriptor(Me.Count - 1) {}
        For i As Integer = 0 To Me.Count - 1
            Dim prop As CustomProperty = DirectCast(Me(i), CustomProperty)
            newProps(i) = New CustomPropertyDescriptor(prop, attributes)
        Next

        Return New PropertyDescriptorCollection(newProps)
    End Function

    Public Function GetProperties() As PropertyDescriptorCollection Implements ICustomTypeDescriptor.GetProperties
        Return TypeDescriptor.GetProperties(Me, True)
    End Function

    Public Function GetPropertyOwner(pd As PropertyDescriptor) As Object Implements ICustomTypeDescriptor.GetPropertyOwner
        Return Me
    End Function
    #End Region

End Class

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


''' <summary>
''' Custom PropertyDescriptor
''' </summary>
Public Class CustomPropertyDescriptor
    Inherits PropertyDescriptor
    Private m_Property As CustomProperty
    Public Sub New(ByRef myProperty As CustomProperty, attrs As Attribute())
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
