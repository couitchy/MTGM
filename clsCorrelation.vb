Public Class clsCorrelation
    Private VmCard1 As String
    Private VmCard2 As String
    Private VmSeq As String
    Public Sub New(VpC1 As String, VpC2 As String, VpS As String)
        VmCard1 = VpC1
        VmCard2 = VpC2
        VmSeq = VpS
    End Sub
    Public Shared Function LongestSequence(VpX() As String, VpY() As String) As String
    '---------------------------------------------------------------------------------------
    'Calcul par programmation dynamique de la plus longue sous-séquence commune entre X et Y
    '---------------------------------------------------------------------------------------
    Dim VpC(,) As Integer
        ReDim VpC(0 To VpX.Length, 0 To VpY.Length)
        For VpI As Integer = 1 To VpX.Length - 1
            For VpJ As Integer = 1 To VpY.Length - 1
                If VpX(VpI) = VpY(VpJ) Then
                    VpC(VpI, VpJ) = VpC(VpI - 1, VpJ - 1) + 1
                Else
                    VpC(VpI, VpJ) = Math.Max(VpC(VpI, VpJ - 1), VpC(VpI - 1, VpJ))
                End If
            Next VpJ
        Next VpI
        'Traceback
        Return ReadOutSequence(VpC, VpX, VpY, VpX.Length - 1, VpY.Length - 1).Trim
    End Function
    Private Shared Function ReadOutSequence(VpC(,) As Integer, VpX() As String, VpY() As String, VpI As Integer, VpJ As Integer) As String
        If VpI = 0 Or VpJ = 0 Then
            Return ""
        ElseIf VpX(VpI) = VpY(VpJ) Then
            Return ReadOutSequence(VpC, VpX, VpY, VpI - 1, VpJ - 1) + " " + VpX(VpI)
        Else
            If VpC(VpI, VpJ - 1) > VpC(VpI - 1, VpJ) Then
                Return ReadOutSequence(VpC, VpX, VpY, VpI, VpJ - 1)
            Else
                Return ReadOutSequence(VpC, VpX, VpY, VpI - 1, VpJ)
            End If
        End If
    End Function
    Public Shared Function GetMean(VpS As List(Of clsCorrelation)) As Single
    '----------------------------------------------------
    'Retourne la longueur moyenne des séquences non vides
    '----------------------------------------------------
    Dim VpTot As Integer = 0
    Dim VpN As Integer = 0
        For Each VpCorr As clsCorrelation In VpS
            If VpCorr.Seq.Trim <> "" Then
                VpTot = VpTot + VpCorr.Seq.Split(" ").Length
                VpN = VpN + 1
            End If
        Next VpCorr
        Return VpTot / VpN
    End Function
    Public Shared Function MyContains(VpS As List(Of clsCorrelation), VpCard As String) As Boolean
    '--------------------------------------------------------------------------------------------------------------------------
    'Retourne vrai si la carte passée en paramètre est référencée dans au moins un des éléments de la liste passée en paramètre
    '--------------------------------------------------------------------------------------------------------------------------
        For Each VpCorr As clsCorrelation In VpS
            If VpCorr.Card1 = VpCard Or VpCorr.Card2 = VpCard Then
                Return True
            End If
        Next VpCorr
        Return False
    End Function
    Public ReadOnly Property Card1 As String
        Get
            Return VmCard1
        End Get
    End Property
    Public ReadOnly Property Card2 As String
        Get
            Return VmCard2
        End Get
    End Property
    Public ReadOnly Property Seq As String
        Get
            Return VmSeq
        End Get
    End Property
    Public Class clsCorrelationComparer
        Implements IComparer(Of clsCorrelation)
        Public Function Compare(ByVal x As clsCorrelation, ByVal y As clsCorrelation) As Integer Implements IComparer(Of clsCorrelation).Compare
            Return x.Seq.Length - y.Seq.Length
        End Function
    End Class
End Class
