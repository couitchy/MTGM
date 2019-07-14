#Region "Importations"
Imports System.Data
Imports System.Data.OleDb
Imports System.Xml
Imports System.Xml.Serialization
Imports System.IO
Imports System.Text
#End Region
Public Partial Class frmOptions
    Public VgSettings As New clsSettings
    Public Sub New
        Call Me.InitializeComponent
    End Sub
    Private Sub OptionsLoad(sender As Object, e As System.EventArgs)
        Me.propOptions.SelectedObject = VgSettings
    End Sub
    Private Sub OptionsFormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs)
        Call SaveSettings
    End Sub
    Public Sub SaveSettings
    '-----------------------------------------------------------------------
    'Sauvegarde les propriétés actuelles du PropertyGrid dans le fichier XML
    '-----------------------------------------------------------------------
    Dim VpXmlSerializer As New XmlSerializer(GetType(clsSettings))
    Dim VpFile As FileStream
    Dim VpWriter As XmlTextWriter
        Try
            VpFile = New FileStream(Application.StartupPath + clsModule.CgXMLFile, FileMode.Create)
            VpWriter = New XmlTextWriter(VpFile, Nothing)
            VpXmlSerializer.Serialize(VpWriter, VgSettings)
            VpWriter.Close
            VpFile.Close
        Catch
            Call clsModule.ShowWarning(clsModule.CgErr11)
        End Try
    End Sub
    Public Sub LoadSettings
    '----------------------------------------------------------------------------
    'Restaure les propriétés sauvegardées du PropertyGrid à partir du fichier XML
    '----------------------------------------------------------------------------
    Dim VpXmlSerializer As XmlSerializer
    Dim VpFile As FileStream
    Dim VpReader As XmlTextReader
        If File.Exists(Application.StartupPath + clsModule.CgXMLFile) Then
            Try
                VpXmlSerializer = New XmlSerializer(GetType(clsSettings))
                VpFile = New FileStream(Application.StartupPath + clsModule.CgXMLFile, FileMode.Open)
                VpReader = New XmlTextReader(VpFile)
                VgSettings = CType(VpXmlSerializer.Deserialize(VpReader), clsSettings)
                VpReader.Close
                VpFile.Close
            Catch
                Call clsModule.ShowWarning(clsModule.CgErr11)
            End Try
        ElseIf File.Exists(Application.StartupPath + clsModule.CgINIFile) Then
            Call clsModule.ShowInformation(clsModule.CgErr8)
        End If
    End Sub
End Class
