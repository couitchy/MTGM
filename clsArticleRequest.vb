<Serializable> _
Public Class clsArticleRequest
    Public article As List(Of clsArticle)
    Public Class clsArticle
        Public idArticle As Long
        Public idProduct As Long
        Public language As clsLanguage
        Public comments As String
        Public price As Single
        Public count As Integer
        Public inShoppingCart As Boolean
        Public seller As clsSeller
        Public condition As String
        Public isFoil As Boolean
        Public isSigned As Boolean
        Public isPlayset As Boolean
        Public isAltered As Boolean
        Public Class clsLanguage
            Public idLanguage As Integer
            Public languageName As String
        End Class
        Public Class clsSeller
            Public idUser As Long
            Public username As String
            Public country As String
            Public isCommercial As Integer
            Public riskGroup As Integer
            Public reputation As Integer
            Public shipsFast As Integer
            Public sellCount As Integer
            Public onVacation As Boolean
            Public idDisplayLanguage As Integer
        End Class
    End Class
End Class
