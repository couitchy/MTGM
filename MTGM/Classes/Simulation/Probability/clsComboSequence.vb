Public Class clsComboSequence
    Private VmElements As List(Of clsItem)
    Public Sub New
        VmElements = New List(Of clsItem)
    End Sub
    Public Sub Add(VpItem As clsItem)
        VmElements.Add(VpItem)
    End Sub
    Public Overrides Function ToString As String
    Dim VpPaires As New Hashtable
    Dim VpStr As String = "ou ("
        For Each VpElement As clsItem In VmElements
            If Not VpPaires.Contains(VpElement.ToString) Then
                VpPaires.Add(VpElement.ToString, 1)
            Else
                VpPaires.Item(VpElement.ToString) += 1
            End If
        Next VpElement
        For Each VpItem As String In VpPaires.Keys
            VpStr = VpStr + VpPaires.Item(VpItem).ToString + " " + VpItem + " et "
        Next VpItem
        Return VpStr.Substring(0, VpStr.Length - 4) + ")"
    End Function
    Public ReadOnly Property Elements As List(Of clsItem)
        Get
            Return VmElements
        End Get
    End Property
End Class
