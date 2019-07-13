'------------------------------------------------------
'| Projet         |  Magic The Gathering Manager      |
'| Contexte       |         Perso                     |
'| Date           |                        30/03/2008 |
'| Release 1      |                        12/04/2008 |
'| Release 2      |                        30/08/2008 |
'| Release 3      |                        08/11/2008 |
'| Release 4      |                        29/08/2009 |
'| Release 5      |                        21/03/2010 |
'| Release 6      |                        17/04/2010 |
'| Release 7      |                        29/07/2010 |
'| Release 8      |                        03/10/2010 |
'| Release 9      |                        05/02/2011 |
'| Release 10     |                        10/09/2011 |
'| Release 11     |                        24/01/2012 |
'| Release 12     |                        01/10/2012 |
'| Release 13     |                        09/05/2014 |
'| Release 14     |                        09/05/2015 |
'| Release 15     |                        15/01/2017 |
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
        Process.Start("mailto:couitchy@free.fr?subject=[Bug] Magic The Gathering Manager&body=MTGM " + Application.ProductVersion.ToString + " " + Environment.OSVersion.ToString + " " + VmException)
    End Sub
End Class
