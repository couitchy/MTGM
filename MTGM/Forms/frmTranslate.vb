Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Data
Imports System.Data.OleDb
Public Partial Class frmTranslate
    Private Const CmURL     As String  = "http://www.magiccorporation.com/mc.php?rub=cartes&op=search&word=#cardname#&search=2"
    Private Const CmId      As String  = "#cardname#"
    Private Const CmKey     As String  = "gathering-cartes-view"
    Private Const CmKey2    As String  = "title"
    Private Const CmFrench  As Integer = 2
    Private Const CmFrAlter As Integer = 1
    Private VmOwner As MainForm
    Public Sub New(VpOwner As MainForm)
        Call Me.InitializeComponent
        VmOwner = VpOwner
        CheckForIllegalCrossThreadCalls = False         'Fortement d�conseill� en temps normal mais permet d'�viter un plantage lorsque l'utilisateur ferme sauvagement
    End Sub
    Private Function Translate(VpIn As String) As String
    '---------------------------------------------------------------------
    'Cherche la traduction en ligne du nom de la carte pass�e en param�tre
    '---------------------------------------------------------------------
    Dim VpRequest As HttpWebRequest     'Requ�te HTTP
    Dim VpAnswer As Stream              'Flux de r�ponse
    Dim VpStr As String = ""            'Cha�ne support
    Dim VpStrs() As String              'S�parations de la cha�ne
    Dim VpCurByte As Integer            'Octet courant
        VpRequest = Nothing
        Try
            VpRequest = WebRequest.Create(CmURL.Replace(CmId, VpIn.Replace(" ", "+")))
            VpRequest.KeepAlive = False
            VpRequest.Timeout = 5000
            VpRequest.ServicePoint.ConnectionLeaseTimeout = 5000
            VpRequest.ServicePoint.MaxIdleTime = 5000
            VpAnswer = VpRequest.GetResponse().GetResponseStream()
            VpCurByte = VpAnswer.ReadByte
            While VpCurByte <> -1
                VpStr = VpStr + (Encoding.Default.GetString(New Byte() {VpCurByte}))
                VpCurByte = VpAnswer.ReadByte
                Application.DoEvents
            End While
            VpStrs = VpStr.Split(New String() {CmKey}, StringSplitOptions.None)
            VpStr = VpStrs(CmFrench)
            If Not VpStr.Contains(CmKey2) Then
                VpStr = VpStrs(CmFrAlter)
            End If
            VpStrs = VpStr.Split("""")
            VpStr = VpStrs(CmFrench)
            VpStr = VpStr.Replace("&#039;", "'")    'Petite feinte entre guillemet simple et apostrophe
            Return VpStr
        Catch
            If Me.chkAlert.Checked Then
                Call mdlToolbox.ShowWarning("Un probl�me est survenu lors de la traduction de la carte " + VpIn + "...")
            End If
        Finally
            If VpRequest IsNot Nothing Then
                VpRequest.Abort
            End If
        End Try
        Return VpIn
    End Function
    Private Sub Launch(VpSerie As String)
    '-------------------------------------------------------------------------------------------
    'Parcours les cartes de l'�dition pass�e en param�tre et demande leur traduction une par une
    '-------------------------------------------------------------------------------------------
    Dim VpDBCommand As New OleDbCommand
        MainForm.VgMe.IsMainReaderBusy = True
        VpDBCommand.Connection = VgDB
        VpDBCommand.CommandType = CommandType.Text
        VgDBCommand.CommandText = "Select Title, EncNbr From Card Where Series = '" + VpSerie + "';"
        VgDBReader = VgDBCommand.ExecuteReader
        With VgDBReader
            While .Read
                'Traduction de la carte
                Me.txtEN.Text = .GetString(0)
                Me.txtFR.Text = Me.Translate(Me.txtEN.Text)
                'Mise � jour dans la base de donn�es
                Try
                    VpDBCommand.CommandText = "Update CardFR Set TitleFR = '" + Me.txtFR.Text.Replace("'", "''") + "' Where EncNbr = " + .GetValue(1).ToString + ";"
                    VpDBCommand.ExecuteNonQuery
                    Me.txtCount.Text = (Val(Me.txtCount.Text) + 1).ToString
                Catch
                End Try
                'D�monopolisation des ressources
                Application.DoEvents
                If .IsClosed Then
                    MainForm.VgMe.IsMainReaderBusy = False
                    Exit Sub
                End If
            End While
            .Close
        End With
        MainForm.VgMe.IsMainReaderBusy = False
        Call mdlToolbox.ShowInformation("Traduction termin�e !")
    End Sub
    Private Sub LaunchLocal(VpSerie As String, VpPath As String)
    Dim VpFile As New StreamReader(VpPath, Encoding.Default)
    Dim VpStrs() As String
        While Not VpFile.EndOfStream
            VpStrs = VpFile.ReadLine.Split("#")
            VgDBCommand.CommandText = "Update CardFR Inner Join Card On CardFR.EncNbr = Card.EncNbr Set CardFR.TitleFR = '" + VpStrs(1).Replace("'", "''") + "' Where Card.Title = '" + VpStrs(0).Replace("'", "''") + "' And Card.Series = '" + VpSerie + "';"
            VgDBCommand.ExecuteNonQuery
            Me.txtCount.Text = (Val(Me.txtCount.Text) + 1).ToString
            Application.DoEvents
        End While
        VpFile.Close
        Call mdlToolbox.ShowInformation("Traduction termin�e !")
    End Sub
    Sub CmdGoClick(ByVal sender As Object, ByVal e As EventArgs)
    Dim VpSerie As String = Me.cboSerie.Text.Substring(1, 2)
        If Not Me.cboSerie.SelectedItem Is Nothing Then
            Me.txtCount.Text = "0"
            Me.cmdGo.Enabled = False
            If mdlToolbox.ShowQuestion("Se connecter � Internet pour r�cup�rer les traductions ?" + vbCrLf + "Cliquer sur 'Non' pour mettre � jour depuis un fichier sur le disque dur...") = DialogResult.Yes Then
                Call Me.Launch(VpSerie)
            ElseIf Me.dlgOpen.ShowDialog <> DialogResult.Cancel Then
                Call Me.LaunchLocal(VpSerie, Me.dlgOpen.FileName)
            End If
            Me.cmdGo.Enabled = True
        Else
            Call mdlToolbox.ShowWarning("Aucune �dition n'a �t� s�lectionn�e...")
        End If
    End Sub
    Sub FrmTranslateLoad(ByVal sender As Object, ByVal e As EventArgs)
        Call mdlToolbox.LoadEditions(Me.cboSerie)
    End Sub
    Sub CboSerieSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
    '-----------------------------------------
    'Affiche le logo de l'�dition s�lectionn�e
    '-----------------------------------------
    Dim VpKey As Integer = mdlConstGlob.VgImgSeries.Images.IndexOfKey("_e" + Me.cboSerie.Text.Substring(1, 2) + CgIconsExt)
        If VpKey <> -1 Then
            Me.picSerie.Image = mdlConstGlob.VgImgSeries.Images(VpKey)
        Else
            Me.picSerie.Image = Nothing
        End If
    End Sub
    Sub FrmTranslateFormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs)
        VgDBReader.Close
    End Sub
End Class
