'------------------------------------------------------
'| Projet         |  Magic The Gathering Manager      |
'| Contexte       |  		Perso                     |
'| Date           |                        30/03/2008 |
'| Release 1      |                        12/04/2008 |
'| Release 2      |                        30/08/2008 |
'| Release 3      |                        08/11/2008 |
'| Release 4      |                        29/08/2009 |
'| Auteur         |                          Couitchy |
'|----------------------------------------------------|
'| Modifications :                                    |
'------------------------------------------------------
Public Partial Class frmExceptionBox
	Private VmException As String
	Public Sub New(VpException As String)
		Me.InitializeComponent()
		VmException = VpException
	End Sub
	Sub CmdContinueClick(ByVal sender As Object, ByVal e As EventArgs)
		Me.Close
	End Sub
	Sub CmdExitClick(ByVal sender As Object, ByVal e As EventArgs)
		Application.Exit
	End Sub
	Sub CmdSendClick(ByVal sender As Object, ByVal e As EventArgs)
		Diagnostics.Process.Start("mailto:couitchy@free.fr?subject=[Bug] Magic The Gathering Manager&body=" + VmException)
	End Sub
End Class
