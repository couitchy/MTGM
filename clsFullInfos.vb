<Serializable> _
Public Class clsFullInfos
    Public name As String
    Public code As String
    Public gathererCode As String
    Public oldCode As String
    Public magicCardsInfoCode As String
    Public releaseDate As String
    Public border As String
    Public type As String
    Public block As String
    Public onlineOnly As Boolean
    Public translations As Dictionary(Of String, String)
    Public cards As List(Of clsFullCardInfos)
    Public Class clsFullCardInfos
        Public id As String
        Public layout As String
        Public name As String
        Public names As List(Of String)
        Public manaCost As String
        Public cmc As Single
        Public colors As List(Of String)
        Public colorIdentity As List(Of String)
        Public type As String
        Public supertypes As List(Of String)
        Public types As List(Of String)
        Public subtypes As List(Of String)
        Public rarity As String
        Public [text] As String
        Public artist As String
        Public number As String
        Public power As String
        Public toughness As String
        Public loyalty As Object
        Public multiverseid As Long
        Public variations As List(Of String)
        Public imageName As String
        Public watermark As String
        Public border As String
        Public timeshifted As Boolean
        Public hand As Integer
        Public life As Integer
        Public reserved As Boolean
        Public releaseDate As String
        Public starter As Boolean
        Public rulings As List(Of clsRulingsInfos)
        Public foreignData As List(Of clsForeignInfos)
        Public printings As List(Of String)
        Public originalText As String
        Public originalType As String
        Public legalities As List(Of clsLegalityInfos)
        Public source As String
        Public uuid As String
        Public Class clsRulingsInfos
            Public [date] As String
            Public [text] As String
        End Class
        Public Class clsForeignInfos
            Public language As String
            Public name As String
            Public multiverseid As Long
        End Class
        Public Class clsLegalityInfos
            Public format As String
            Public legality As String
        End Class
    End Class
End Class
