Public Class clsAppearance
    Private VmEsp As New SortedList
    Private VmOccurences As Integer
    Public Sub New(VpOccurences As String)
        VmOccurences = CInt(VpOccurences)
    End Sub
    Public Sub AddForRound(VpI As Integer, Optional VpJ As Integer = 1)
        If VmEsp.ContainsKey(VpI) Then
            VmEsp.Item(VpI) = VmEsp.Item(VpI) + VpJ
        Else
            VmEsp.Add(VpI, VpJ)
        End If
    End Sub
    Public Function GetEsp(Optional VpPercent As Boolean = True) As SortedList
    Dim VpEsp As New SortedList(VmEsp.Count)
        For Each VpKey As Integer In VmEsp.Keys
            VpEsp.Add(VpKey, If(VpPercent, 100, 1) * VmEsp.Item(VpKey) / VmOccurences)
        Next VpKey
        Return VpEsp
    End Function
    Public Sub AddRoundsDispos(VpCbo As ComboBox)
        VpCbo.Items.Clear
        For Each VpI As Integer In VmEsp.Keys
            VpCbo.Items.Add(Format(VpI + 1, "00"))
        Next VpI
        VpCbo.Sorted = True
    End Sub
End Class
