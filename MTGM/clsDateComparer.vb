Public Class clsDateComparer
    Implements IComparer(Of Date)
    Public Function Compare(ByVal x As Date, ByVal y As Date) As Integer Implements IComparer(Of Date).Compare
        Return Date.Compare(x, y)
    End Function
End Class
