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
Imports System.IO
Imports System.Text
Public Partial Class About
	Public Sub New()		
		Me.InitializeComponent()
	End Sub	
	Sub Button1Click(sender As Object, e As System.EventArgs)
		Me.DialogResult = System.Windows.Forms.DialogResult.OK		
	End Sub	
	Sub AboutLoad(ByVal sender As Object, ByVal e As EventArgs)
	Dim VpHisto As StreamReader
		Me.txtVer.Text = Application.ProductVersion.Substring(0, 8)
		Me.txtDateCompile.Text = System.IO.File.GetLastWriteTimeUtc(Process.GetCurrentProcess().MainModule.FileName).ToShortDateString
		Me.txtCodeLines.Text = clsModule.CgCodeLines.ToString	
		If File.Exists(Windows.Forms.Application.StartupPath + clsModule.CgHSTFile) Then
			VpHisto = New StreamReader(Windows.Forms.Application.StartupPath + clsModule.CgHSTFile, Encoding.Default)
			Me.txtVersions.Text = VpHisto.ReadToEnd().Replace(vbLf, vbCrLf)
			VpHisto.Close
		End If
	End Sub
	Sub LnkMailLinkClicked(ByVal sender As Object, ByVal e As LinkLabelLinkClickedEventArgs)
		Diagnostics.Process.Start("mailto:couitchy@free.fr?subject=Magic The Gathering Manager&body=Votre message ici")
	End Sub
End Class
