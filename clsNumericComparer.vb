Imports SourceGrid2
Imports Cells = SourceGrid2.Cells.Real
Public Class clsNumericComparer
    Implements IComparer
    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
    Dim VpX As Cells.Cell = x
    Dim VpY As Cells.Cell = y
        Return Val(VpX.Value.ToString.Replace(",", ".")).CompareTo(Val(VpY.Value.ToString.Replace(",", ".")))
    End Function
End Class
