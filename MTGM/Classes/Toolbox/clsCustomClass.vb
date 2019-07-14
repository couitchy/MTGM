Imports System.ComponentModel
''' <summary>
''' CustomClass (Which is binding to property grid)
''' </summary>
Public Class clsCustomClass
    Inherits CollectionBase
    Implements ICustomTypeDescriptor
    ''' <summary>
    ''' Add CustomProperty to Collectionbase List
    ''' </summary>
    ''' <param name="Value"></param>
    Public Sub Add(Value As clsCustomProperty)
        MyBase.List.Add(Value)
    End Sub

    ''' <summary>
    ''' Remove item from List
    ''' </summary>
    ''' <param name="Name"></param>
    Public Sub Remove(Name As String)
        For Each prop As clsCustomProperty In MyBase.List
            If prop.Name = Name Then
                MyBase.List.Remove(prop)
                Return
            End If
        Next
    End Sub

    ''' <summary>
    ''' Indexer
    ''' </summary>
    Public Default Property Item(index As Integer) As clsCustomProperty
        Get
            Return DirectCast(MyBase.List(index), clsCustomProperty)
        End Get
        Set
            MyBase.List(index) = DirectCast(value, clsCustomProperty)
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
            Dim prop As clsCustomProperty = DirectCast(Me(i), clsCustomProperty)
            newProps(i) = New clsCustomPropertyDescriptor(prop, attributes)
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
