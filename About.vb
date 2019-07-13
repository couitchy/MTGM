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
        Me.txtNClasses.Text = clsModule.CGNClasses.ToString
        If File.Exists(Windows.Forms.Application.StartupPath + clsModule.CgHSTFile) Then
            VpHisto = New StreamReader(Windows.Forms.Application.StartupPath + clsModule.CgHSTFile, Encoding.Default)
            Me.txtVersions.Text = VpHisto.ReadToEnd().Replace(vbLf, vbCrLf)
            VpHisto.Close
        End If
    End Sub
    Sub LnkMailLinkClicked(ByVal sender As Object, ByVal e As LinkLabelLinkClickedEventArgs)
        Process.Start(clsModule.CgURL18)
    End Sub
    Sub PicPaypalClick(ByVal sender As Object, ByVal e As EventArgs)
        Process.Start(clsModule.CgURL16)
    End Sub
End Class
