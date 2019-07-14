Public Partial Class frmExceptionBox
    Private VmException As String
    Public Sub New(VpException As String)
        Call Me.InitializeComponent
        VmException = VpException
    End Sub
    Sub CmdContinueClick(ByVal sender As Object, ByVal e As EventArgs)
        Me.Close
    End Sub
    Sub CmdExitClick(ByVal sender As Object, ByVal e As EventArgs)
        Application.Exit
    End Sub
    Sub CmdSendClick(ByVal sender As Object, ByVal e As EventArgs)
        Process.Start("mailto:couitchy@free.fr?subject=[Bug] Magic The Gathering Manager&body=MTGM " + Application.ProductVersion.ToString + " " + Environment.OSVersion.ToString + " " + VmException)
    End Sub
End Class
