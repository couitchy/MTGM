Public Class clsItem
    Public Enum eElementType
        Card
        Type
        SubType
        Cost
        Color
    End Enum
    Private VmElementType As eElementType
    Private VmElementValue As String
    Public Sub New(VpElementType As eElementType, VpElementValue As String)
        VmElementType = VpElementType
        VmElementValue = VpElementValue
    End Sub
    Public Overrides Function ToString As String
        Select Case VmElementType
            Case eElementType.Card, eElementType.SubType, eElementType.Cost
                Return VmElementValue
            Case eElementType.Type
                Return clsModule.FormatTitle("Card.Type", VmElementValue)
            Case eElementType.Color
                Return clsModule.FormatTitle("Spell.Color", VmElementValue)
            Case Else
                Return ""
        End Select
    End Function
    Public ReadOnly Property ElementType As eElementType
        Get
            Return VmElementType
        End Get
    End Property
    Public ReadOnly Property ElementValue As String
        Get
            Return VmElementValue
        End Get
    End Property
End Class
