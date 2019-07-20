Public Class ucReadOnlyPropertyGrid
    Inherits PropertyGrid
    Protected Overrides Function ProcessDialogKey(ByVal keyData As Keys) As Boolean
        Return True
    End Function
    Public Function PreFilterMessage(ByRef m As Message) As Boolean
        If m.Msg = &H204 Then   'WM_RBUTTONDOWN
            Return True
        End If
        Return False
    End Function
    Protected Overrides ReadOnly Property CreateParams As CreateParams
        Get
            Me.DoubleBuffered = True
            Dim VpParams As CreateParams = MyBase.CreateParams
            VpParams.ExStyle = VpParams.ExStyle And Not &H2000000
            Return VpParams
        End Get
    End Property
End Class
