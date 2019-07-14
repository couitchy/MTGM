<Serializable> _
Public Class clsProductRequest
    Public product As List (Of clsProduct)
    Public Class clsProduct
        Public idProduct As Long
        Public idMetaproduct As Long
        Public idGame As Integer
        'Public countReprints As Integer
        'Public name As Dictionary(Of String, clsName)
        'Public website As String
        'Public image As String
        'Public category As clsCategory
        'Public priceGuide As clsPriceGuide
        Public expansion As String
        'Public expIcon As Integer
        'Public number As Integer
        'Public rarity As String
        'Public Class clsName
        '   Public idLanguage As Integer
        '   Public languageName As String
        '   Public productName As String
        'End Class
        'Public Class clsCategory
        '   Public idCategory As Integer
        '   Public categoryName As String
        'End Class
        'Public Class clsPriceGuide
        '   Public SELL As Single
        '   Public LOW As Single
        '   Public LOWEX As Single
        '   Public LOWFOIL As Single
        '   Public AVG As Single
        '   Public TREND As Single
        'End Class
    End Class
End Class
