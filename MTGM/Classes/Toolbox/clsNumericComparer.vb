Public Class clsNumericComparer
    Implements IComparer(Of String)
    Public Function Compare(ByVal x As String, ByVal y As String) As Integer Implements IComparer(Of String).Compare
        Return CInt(x) - CInt(y)
    End Function
End Class
