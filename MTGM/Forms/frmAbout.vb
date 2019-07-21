Imports System.IO
Imports System.Text
Public Partial Class frmAbout
    Public Sub New
        Call Me.InitializeComponent
    End Sub
    Sub Button1Click(sender As Object, e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
    End Sub
    Sub AboutLoad(ByVal sender As Object, ByVal e As EventArgs)
    Dim VpHisto As StreamReader
        Me.txtVer.Text = Application.ProductVersion.Substring(0, 8)
        Me.txtDateCompile.Text = System.IO.File.GetLastWriteTimeUtc(Process.GetCurrentProcess().MainModule.FileName).ToShortDateString
        Me.txtCodeLines.Text = mdlConstGlob.CgCodeLines.ToString
        Me.txtNClasses.Text = mdlConstGlob.CgNClasses.ToString
        If File.Exists(Windows.Forms.Application.StartupPath + mdlConstGlob.CgHSTFile) Then
            VpHisto = New StreamReader(Windows.Forms.Application.StartupPath + mdlConstGlob.CgHSTFile, Encoding.Default)
            Me.txtVersions.Text = VpHisto.ReadToEnd().Replace(vbLf, vbCrLf)
            VpHisto.Close
        End If
    End Sub
    Sub LnkMailLinkClicked(ByVal sender As Object, ByVal e As LinkLabelLinkClickedEventArgs)
        Process.Start(mdlConstGlob.CgURL18)
    End Sub
    Sub PicPaypalClick(ByVal sender As Object, ByVal e As EventArgs)
        Process.Start(mdlConstGlob.CgURL16)
    End Sub
End Class
