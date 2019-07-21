#Region "Importations"
Imports TD.SandBar
Imports TreeViewMS
Imports System.IO
'Imports Win7Taskbar
Imports System.Resources
Imports System.Reflection
Imports System.Data
Imports System.Data.OleDb
Imports System.Xml
Imports SourceGrid2
Imports Cells = SourceGrid2.Cells.Real
Imports System.Text
Imports System.Web.Script.Serialization
#End Region
Public Partial Class MainForm
    #Region "Déclarations"
    Private VmMyChildren As New clsChildren
    Private VmFilterCriteria As New frmExploSettings(Me)
    Private VmDeckMode As Boolean = False
    Private VmSearch As New clsSearch
    Private VmAdvSearch As String = ""
    Private VmAdvSearchLabel As String = ""
    Private VmSuggestions As List(Of clsCorrelation) = Nothing
    Private VmDownloadInProgress As Boolean = False
    Private VmMustReload As Boolean = False
    Private VmImgDL As Boolean = False
    Private VmMainReaderBusy As Boolean = False
    Private VmStartup As String = ""
'   Public  VgBar As Windows7ProgressBar
    Public Shared VgMe As MainForm
    #End Region
    #Region "Méthodes"
    Public Sub New(VpArgs() As String)
        VgMe = Me
        'Intégrité de l'application
        If Not mdlMain.CheckIntegrity Then
            Process.GetCurrentProcess.Kill
            Exit Sub
        Else
            Call Me.InitializeComponent
            mdlToolbox.VgTray = New NotifyIcon(Me.components)
            mdlToolbox.VgTray.Icon = Me.Icon
            mdlToolbox.VgTray.Text = "Magic The Gathering Manager"
            mdlToolbox.VgTimer = New Timer
            mdlToolbox.VgTimer.Interval = 1000 * 10 'recherche d'une mise à jour 10 sec après le démarrage
            'Anciens fichiers temporaires éventuels
            Call mdlToolbox.DeleteTempFiles
        End If
        'Arguments au lancement
        If VpArgs.Length > 0 Then
            If File.Exists(VpArgs(0)) And VpArgs(0).EndsWith(mdlConstGlob.CgFExtN) Or VpArgs(0).EndsWith(mdlConstGlob.CgFExtO) Or VpArgs(0).EndsWith(mdlConstGlob.CgFExtD) Then
                VmStartup = VpArgs(0)
            End If
        End If
    End Sub
    Public Sub MyRefresh
        Call Me.LoadTvw(VmAdvSearch, , VmAdvSearchLabel)
    End Sub
    Public Function StatusTextGet As String
        If Not Me.lblDB Is Nothing Then
            Return Me.lblDB.Text
        Else
            Return ""
        End If
    End Function
    Public Sub StatusText(VpStr As String, Optional VpClearN As Boolean = False)
        If Not Me.lblDB Is Nothing Then
            Me.lblDB.Text = VpStr
            If VpClearN Then
                Me.lblNCards.Text = ""
            End If
            Application.DoEvents
        End If
    End Sub
    Private Sub DBOpenInit(VpFile As String)
    '-------------------------------------------------
    'Vérification fichier / base de données par défaut
    '-------------------------------------------------
    Dim VpCur As FileInfo
    Dim VpDefault As FileInfo = Nothing
        'Si le fichier sélectionné est différent de celui par défaut ou que celui par défaut n'existe pas, propose de l'y mettre
        If File.Exists(VpFile) Then
            VpCur = New FileInfo(VpFile)
            If File.Exists(VgOptions.VgSettings.DefaultBase) Then
                VpDefault = New FileInfo(VgOptions.VgSettings.DefaultBase)
            End If
            If VpDefault Is Nothing OrElse VpCur.FullName <> VpDefault.FullName Then
                If mdlToolbox.ShowQuestion("Voulez-vous définir la base de données que vous tentez d'ouvrir comme étant celle par défaut ?" + vbCrLf + "Choisissez 'Oui' pour ouvrir automatiquement cette base à chaque démarrage du logiciel...") = DialogResult.Yes Then
                    VgOptions.VgSettings.DefaultBase = VpCur.FullName
                    Call VgOptions.SaveSettings
                End If
            End If
        End If
        'Ouverture de la base sélectionnée
        If mdlToolbox.DBOpen(VpFile) Then
            Me.lblDB.Text = VgDB.DataSource
            Call Me.LoadMnu
            Call Me.LoadTvw
        End If
    End Sub
    Public Sub UpdatePrices(VpFile As String, VpFromDisk As Boolean, Optional VpSilent As Boolean = False)
    '---------------------------------------------------------------------------------------------------------
    'Met à jour le prix des cartes dans la base de données à partir du fichier de cotation fourni en paramètre
    '---------------------------------------------------------------------------------------------------------
    Dim VpPrices As New StreamReader(VpFile)
    Dim VpCardData() As String
    Dim VpPrice As String
    Dim VpEdition As String
    Dim VpEditionsOK As New List(Of String)
    Dim VpCardName As String
    Dim VpDate As String
        'Vérifie que le fichier contient des prix à jour
        VpDate = VpPrices.ReadLine
        If IsDate(VpDate) Then
            'On ne met pas à jour si plus ancien et que le fichier ne vient pas du disque dur
            If mdlToolbox.GetLastPricesDate.Subtract(VpDate).Days >= 0 And Not VpFromDisk Then
                VpPrices.Close
                If Not VpSilent Then
                    Call mdlToolbox.ShowInformation("Les prix sont déjà à jour...")
                End If
                Exit Sub
            End If
        Else
            VpDate = File.GetLastWriteTimeUtc(VpFile).ToShortDateString
            VpPrices.BaseStream.Seek(0, SeekOrigin.Begin)
        End If
        If Not VpSilent Then
            If Not mdlToolbox.ShowQuestion("Les prix vont être mis à jour avec la liste suivante :" + vbCrLf + VpFile + vbCrLf + "L'opération pourra durer plusieurs secondes, patienter jusqu'à la notification. Continuer ?") = System.Windows.Forms.DialogResult.Yes Then
                VpPrices.Close
                Exit Sub
            End If
        End If
        Call Me.InitBars(File.ReadAllLines(VpFile).Length)
        While Not VpPrices.EndOfStream
            VpCardData = VpPrices.ReadLine.Split("#")
            VpCardName = ""
            VpEdition = ""
            VpEditionsOK.Clear
            VpPrice = ""
            For Each VpStr As String In VpCardData
                If VpStr.IndexOf("^") <> -1 Then
                    VpEdition = VpStr.Substring(0, VpStr.IndexOf("^")).Replace("'", "''")
                    If Not VpEditionsOK.Contains(VpEdition) Then    'fait en sorte de ne prendre que le premier prix par édition (correspondant à la qualité de carte NM/MT)
                        VpEditionsOK.Add(VpEdition)
                        VpPrice = VpStr.Substring(VpStr.IndexOf("^") + 1).Replace("€", "").Trim
                        'Prix foil
                        If VpEdition.EndsWith(mdlConstGlob.CgFoil) Then
                            VpEdition = VpEdition.Replace(mdlConstGlob.CgFoil, "")
                            VgDBCommand.CommandText = "Update Card Inner Join Series On Card.Series = Series.SeriesCD Set Card.FoilPrice = " + VpPrice + ", FoilDate = " + mdlToolbox.GetDate(VpDate) + " Where Series.SeriesNM_MtG = '" + VpEdition + "' And Card.Title = '" + VpCardName + "';"
                            VgDBCommand.ExecuteNonQuery
                        'Prix standard
                        Else
                            VgDBCommand.CommandText = "Update Card Inner Join Series On Card.Series = Series.SeriesCD Set Card.Price = " + VpPrice + ", Card.myPrice = " + mdlToolbox.MyPrice(VpPrice).ToString + ", PriceDate = " + mdlToolbox.GetDate(VpDate) + " Where Series.SeriesNM_MtG = '" + VpEdition + "' And Card.Title = '" + VpCardName + "';"
                            VgDBCommand.ExecuteNonQuery
                        End If
                    End If
                Else
                    VpCardName = VpStr.Replace("'", "''")
                End If
            Next VpStr
            Me.prgAvance.Increment(1)
'           VgBar.Increment(1)
            Application.DoEvents
        End While
        Call Me.FixPrices
        VpPrices.Close
        If Not VpSilent Then
            Call mdlToolbox.ShowInformation("Mise à jour des prix terminée !")
        End If
        Me.prgAvance.Visible = False
'       VgBar.ShowInTaskbar = False
    End Sub
    Public Sub UpdateAutorisations(VpFile As String, Optional VpSilent As Boolean = False)
    '----------------------------------------------------------------------------------------------------
    'Met à jour la liste des autorisations des cartes en tournois à partir du fichier fourni en paramètre
    '----------------------------------------------------------------------------------------------------
    Dim VpTournois As New StreamReader(VpFile)
    Dim VpCardData() As String
        VgDBCommand.CommandText = "Delete * From Autorisations;"
        VgDBCommand.ExecuteNonQuery
        Call Me.InitBars(File.ReadAllLines(VpFile).Length)
        While Not VpTournois.EndOfStream
            VpCardData = VpTournois.ReadLine.Split("#")
            If VpCardData.Length = 8 Then
                VgDBCommand.CommandText = "Insert Into Autorisations (Title, T1, T1r, T15, M, T2, Bloc, 1V1, Multi) Values ('" + VpCardData(0).Replace("'", "''") + "', " + (Not VpCardData(1).EndsWith("no")).ToString + ", " + (VpCardData(1).EndsWith("r")).ToString + ", " + (Not VpCardData(2).EndsWith("no")).ToString + ", " + (Not VpCardData(3).EndsWith("no")).ToString + ", " + (Not VpCardData(4).EndsWith("no")).ToString + ", " + (Not VpCardData(5).EndsWith("no")).ToString + ", " + (Not VpCardData(6).EndsWith("no")).ToString + ", " + (Not VpCardData(7).EndsWith("no")).ToString + ");"
                VgDBCommand.ExecuteNonQuery
            ElseIf VpCardData.Length = 9 Then
                Try
                    VgDBCommand.CommandText = "Insert Into Autorisations (Title, T1, T1r, T15, M, T2, Bloc, Blocoff, 1V1, Multi, MTGO, MTGOoff) Values ('" + VpCardData(0).Replace("'", "''") + "', " + (Not VpCardData(1).EndsWith("no")).ToString + ", " + (VpCardData(1).EndsWith("r")).ToString + ", " + (Not VpCardData(2).EndsWith("no")).ToString + ", " + (Not VpCardData(3).EndsWith("no")).ToString + ", " + (Not VpCardData(4).EndsWith("no")).ToString + ", " + (Not VpCardData(5).EndsWith("no")).ToString + ", " + (VpCardData(5).EndsWith("dk")).ToString + ", " + (Not VpCardData(6).EndsWith("no")).ToString + ", " + (Not VpCardData(7).EndsWith("no")).ToString + ", " + (Not VpCardData(8).EndsWith("no")).ToString + ", " + (VpCardData(8).EndsWith("dk")).ToString + ");"
                    VgDBCommand.ExecuteNonQuery
                Catch
                    Call mdlToolbox.ShowWarning("Autorisation en doublon pour la carte : " + VpCardData(0))
                End Try
            End If
            Me.prgAvance.Increment(1)
            Application.DoEvents
        End While
        Call Me.FixPrices
        VpTournois.Close
        If Not VpSilent Then
            Call mdlToolbox.ShowInformation("Mise à jour des autorisations terminée !")
        End If
        Me.prgAvance.Visible = False
    End Sub
    Private Sub MarkAs(VpTrad As List(Of clsTxtFR), VpState As clsTxtFR.eTxtState)
    '------------------------------------------------
    'Détermine l'état des traductions VO/VF courantes
    '------------------------------------------------
        Call Me.InitBars(VpTrad.Count)
        With VgDBReader
            While .Read
                For Each VpTxtFR As clsTxtFR In VpTrad
                    If VpTxtFR.CardName = .GetString(0) AndAlso VpTxtFR.Already = clsTxtFR.eTxtState.Neww Then
                        VpTxtFR.Already = VpState
                        Me.prgAvance.Increment(1)
'                       VgBar.Increment(1)
                        Application.DoEvents
                    End If
                Next VpTxtFR
            End While
            .Close
        End With
    End Sub
    Public Sub UpdateRulings(Optional VpSilent As Boolean = False)
    '--------------------------------------------
    'Met à jour les règles spécifiques des cartes
    '--------------------------------------------
    Dim VpIn As New StreamReader(Application.StartupPath + mdlConstGlob.CgUpRulings)
    Dim VpReader As New XmlTextReader(VpIn)
    Dim VpName As String
    Dim VpRuling As String
    Dim VpRulings As New Hashtable
        If Not VpSilent Then
            If Not mdlToolbox.ShowQuestion("Les règles spécifiques vont être mises à jour..." + vbCrLf + "L'opération pourra durer plusieurs secondes, patienter jusqu'à la notification. Continuer ?") = System.Windows.Forms.DialogResult.Yes Then
                Exit Sub
            End If
        End If
        With VpReader
            While VpReader.Read
                If VpReader.Name = "name" Then
                    VpName = VpReader.ReadElementContentAsString
                    VpReader.ReadToFollowing("ruling")
                    VpRuling = VpReader.ReadElementContentAsString
                    If VpRuling.Trim <> "" Then
                        If Not VpRulings.Contains(VpName) Then
                            VpRulings.Add(VpName, VpRuling)
                        End If
                    End If
                End If
            End While
        End With
        VpReader.Close
        VpIn.Close
        Me.prgAvance.Visible = True
        Call Me.InitBars(VpRulings.Count)
        For Each VpCard As String In VpRulings.Keys
            VgDBCommand.CommandText = "Update Spell Set Rulings = '" + VpRulings.Item(VpCard).ToString.Replace("'", "''") + "' Where Title = '" + VpCard.Replace("'", "''") + "';"
            VgDBCommand.ExecuteNonQuery
            Me.prgAvance.Increment(1)
            Application.DoEvents
        Next VpCard
        If Not VpSilent Then
            Call mdlToolbox.ShowInformation("Mise à jour des règles spécifiques terminée !")
        Else
            VmMyChildren.ContenuUpdater.PassiveUpdate = mdlConstGlob.ePassiveUpdate.Done
        End If
        Me.prgAvance.Visible = False
        Call mdlToolbox.SecureDelete(Application.StartupPath + mdlConstGlob.CgUpRulings)
    End Sub
    Public Sub UpdateTxtFR(Optional VpSilent As Boolean = False)
    '-----------------------------------------------------
    'Met à jour la version française des textes des cartes
    '-----------------------------------------------------
    Dim VpTxt As New StreamReader(Application.StartupPath + mdlConstGlob.CgUpTXTFR)
    Dim VpStrs() As String = VpTxt.ReadToEnd.Split(New String() {"##"}, StringSplitOptions.None)
    Dim VpItem() As String
    Dim VpTrad As New List(Of clsTxtFR)
    Dim VpCountN As Integer = 0
    Dim VpCountU As Integer = 0
        'Parse le contenu du fichier
        For VpI As Integer = 1 To VpStrs.Length - 1
            VpItem = VpStrs(VpI).Split(New String() {"^^"}, StringSplitOptions.None)
            VpTrad.Add(New clsTxtFR(VpItem(0), VpItem(1)))
        Next VpI
        VpTxt.Close
        If Not VpSilent Then
            If Not mdlToolbox.ShowQuestion("Les textes vont être mis à jour..." + vbCrLf + "L'opération pourra durer plusieurs secondes, patienter jusqu'à la notification. Continuer ?") = System.Windows.Forms.DialogResult.Yes Then
                Exit Sub
            End If
        End If
        Me.IsMainReaderBusy = True
        Me.prgAvance.Visible = True
'       VgBar.ShowInTaskbar = True
        'Par défaut on considère qu'on a rien
        'Regarde celles qu'on a déjà et qu'il faut mettre à jour
        VgDBCommand.CommandText = "Select TextesFR.CardName From (TextesFR Inner Join Card On Card.Title = TextesFR.CardName) Where TextesFR.TexteFR = Card.CardText Or Trim(TextesFR.TexteFR) = '' Or TextesFR.TexteFR Is Null;"
        VgDBReader = VgDBCommand.ExecuteReader
        Call Me.MarkAs(VpTrad, clsTxtFR.eTxtState.Update)
        'Regarde celles qu'on a déjà et qui sont déjà traduites
        VgDBCommand.CommandText = "Select TextesFR.CardName From (TextesFR Inner Join Card On Card.Title = TextesFR.CardName) Where TextesFR.TexteFR <> Card.CardText And Trim(TextesFR.TexteFR) <> '' And Not TextesFR.TexteFR Is Null;"
        VgDBReader = VgDBCommand.ExecuteReader
        Call Me.MarkAs(VpTrad, clsTxtFR.eTxtState.Ok)
        Call Me.InitBars(VpTrad.Count)
        'Effectue les modifications
        For Each VpTxtFR As clsTxtFR In VpTrad
            If VpTxtFR.Texte.Trim <> "" Then
                If VpTxtFR.Already = clsTxtFR.eTxtState.Neww Then   'cas où le correctif porte sur des cartes d'une édition pas encore présente dans la base courante
                    Try
                        VgDBCommand.CommandText = "Insert Into TextesFR (CardName, TexteFR) Values ('" + VpTxtFR.CardName.Replace("'", "''") + "', '" + VpTxtFR.Texte.Replace("'", "''") + "');"
                        VgDBCommand.ExecuteNonQuery
                        VpCountN = VpCountN + 1
                    Catch VpErr As Exception
                        'Call mdlToolbox.ShowWarning("Erreur lors de l'insertion de la traduction de " + VpTxtFR.CardName + "..." + vbCrLf + vbCrLf + "Détails : " + VpErr.Message)
                    End Try
                ElseIf VpTxtFR.Already = clsTxtFR.eTxtState.Update Then
                    VgDBCommand.CommandText = "Update TextesFR Set TexteFR = '" + VpTxtFR.Texte.Replace("'", "''") + "' Where CardName = '" + VpTxtFR.CardName.Replace("'", "''") + "';"
                    VgDBCommand.ExecuteNonQuery
                    VpCountU = VpCountU + 1
                End If
            End If
            Me.prgAvance.Increment(1)
'           VgBar.Increment(1)
            Application.DoEvents
        Next VpTxtFR
        Me.IsMainReaderBusy = False
        If Not VpSilent Then
            Call mdlToolbox.ShowInformation("Mise à jour des textes terminée !" + vbCrLf + VpCountN.ToString + " nouvelle(s) insertion(s)" + vbCrLf + VpCountU.ToString + " nouvelle(s) traduction(s)" + vbCrLf + (VpTrad.Count - VpCountN - VpCountU).ToString + " traduction(s) déjà à jour")
        End If
        Me.prgAvance.Visible = False
'       VgBar.ShowInTaskbar = False
        Call mdlToolbox.SecureDelete(Application.StartupPath + mdlConstGlob.CgUpTXTFR)
    End Sub
    Public Sub UpdatePictures(VpFile As String, VpLogFile As String, Optional VpKillThem As Boolean = False)
    '----------------------------------------------------------------------------------------------------------------------------------
    'Met à jour la base d'images des cartes en concaténant le nouveau fichier à l'ancien et en calculant les nouveaux index dans la BDD
    '----------------------------------------------------------------------------------------------------------------------------------
    Dim VpLog As New StreamReader(VpLogFile)
    Dim VpIn As New StreamReader(VpFile)
    Dim VpInB As New BinaryReader(VpIn.BaseStream)
    Dim VpOut As New StreamWriter(VgOptions.VgSettings.PicturesFile, True)
    Dim VpOutB As New BinaryWriter(VpOut.BaseStream)
    Dim VpFileInfo As New FileInfo(VpFile)
    Dim VpStrs() As String
    Dim VpLogInfos() As String
    Dim VpSizeCheck As String
    Dim VpOffsetBase As Long
    Dim VpCurOffset As Long
    Dim VpCurEnd As Long
    Dim VpCount As Integer = 0
        Try
            'Vérification de l'intégrité du patch
            VpLogInfos = File.ReadAllLines(VpLogFile)
            VpSizeCheck = VpLogInfos(VpLogInfos.Length - 1)
            VpSizeCheck = VpSizeCheck.Substring(VpSizeCheck.LastIndexOf("#") + 1)
            If CLng(VpSizeCheck) <> VpFileInfo.Length - 1 Then Throw New Exception
            Call Me.InitBars(VpLogInfos.Length)
            VpOffsetBase = (New FileInfo(VgOptions.VgSettings.PicturesFile)).Length
            'Concaténation des données brutes
            VpOutB.Write(VpInB.ReadBytes(VpFileInfo.Length))
            Me.prgAvance.Value = Me.prgAvance.Maximum / 2
'           VgBar.Value = VgBar.Maximum / 2
            VpIn.Close
            VpOutB.Flush
            VpOutB.Close
            'Mise à jour des index (seulement si CardPictures est obsolète)
            VgDBCommand.CommandText = "Select Max([End]) From CardPictures;"
            While Not VpLog.EndOfStream
                VpStrs = VpLog.ReadLine.Split("#")
                VpCurOffset = VpOffsetBase + CLng(VpStrs(1))
                VpCurEnd = VpOffsetBase + CLng(VpStrs(2))
                Try
                    VgDBCommand.CommandText = "Insert Into CardPictures Values ('" + VpStrs(0).Replace(".jpg", "").Replace("'", "''") + "', " + VpCurOffset.ToString + ", " + VpCurEnd.ToString + ");"
                    VgDBCommand.ExecuteNonQuery
                Catch
                    VgDBCommand.CommandText = "Update CardPictures Set Offset = " + VpCurOffset.ToString + ", [End] = " + VpCurEnd.ToString + " Where Title = '" + VpStrs(0).Replace(".jpg", "").Replace("'", "''") + "';"
                    VgDBCommand.ExecuteNonQuery
                End Try
                VpCount = VpCount + 1
                Me.prgAvance.Increment(1)
'                   VgBar.Increment(1)
                Application.DoEvents
            End While
            VpLog.Close
            'Suppression éventuelle des fichiers d'update
            If VpKillThem Then
                Call mdlToolbox.SecureDelete(VpFile)
                Call mdlToolbox.SecureDelete(VpLogFile)
            End If
            If Not VmMyChildren.DoesntExist(VmMyChildren.ContenuUpdater) Then
                VmMyChildren.ContenuUpdater.PassiveUpdate = mdlConstGlob.ePassiveUpdate.Done
            Else
                Call mdlToolbox.ShowInformation("Mise à jour des images des cartes terminée !" + vbCrLf + "(" + VpCount.ToString + " cartes ajoutées)" + vbCrLf + vbCrLf + "Il se peut qu'il y ait besoin de relancer la mise à jour encore une fois pour compléter...")
            End If
            Me.prgAvance.Visible = False
'           VgBar.ShowInTaskbar = False
        Catch
            If Not VmMyChildren.DoesntExist(VmMyChildren.ContenuUpdater) Then
                If VmMyChildren.ContenuUpdater.PassiveUpdate = mdlConstGlob.ePassiveUpdate.InProgress Then
                    VmMyChildren.ContenuUpdater.PassiveUpdate = mdlConstGlob.ePassiveUpdate.Failed
                Else
                    Call mdlToolbox.ShowWarning("Une erreur est survenue pendant la mise à jour des images...")
                End If
            Else
                Call mdlToolbox.ShowWarning("Une erreur est survenue pendant la mise à jour des images...")
            End If
        End Try
    End Sub
    Public Function FixPictures As Boolean
    '-------------------------------------------------------------
    'Télécharge et installe un correctif pour les images en défaut
    '-------------------------------------------------------------
    Dim VpOut As New FileStream(VgOptions.VgSettings.PicturesFile, FileMode.OpenOrCreate)
    Dim VpOutB As New BinaryWriter(VpOut)
    Dim VpLog As StreamReader
    Dim VpLogPath As String = Application.StartupPath + "\" + mdlConstGlob.CgMdPic + mdlConstGlob.CgPicLogExt
    Dim VpIn As StreamReader
    Dim VpInB As BinaryReader
    Dim VpInPath As String = Application.StartupPath + "\" + mdlConstGlob.CgMdPic + mdlConstGlob.CgPicUpExt
    Dim VpName As String
    Dim VpStrs() As String
    Dim VpNewOffset As Long
    Dim VpNewEnd As Long
    Dim VpOldOffset As Long
        Call mdlToolbox.DownloadNow(New Uri(mdlConstGlob.VgOptions.VgSettings.DownloadServer + CgURL10 + mdlConstGlob.CgMdPic + mdlConstGlob.CgPicUpExt), "\" + mdlConstGlob.CgMdPic + mdlConstGlob.CgPicUpExt)
        Call mdlToolbox.DownloadNow(New Uri(mdlConstGlob.VgOptions.VgSettings.DownloadServer + CgURL10 + mdlConstGlob.CgMdPic + mdlConstGlob.CgPicLogExt), "\" + mdlConstGlob.CgMdPic + mdlConstGlob.CgPicLogExt)
        If File.Exists(VpLogPath) And File.Exists(VpInPath) Then
            VpLog = New StreamReader(VpLogPath)
            VpIn = New StreamReader(VpInPath)
            VpInB = New BinaryReader(VpIn.BaseStream)
            While Not VpLog.EndOfStream
                VpStrs = VpLog.ReadLine.Split("#")
                VpNewOffset = CLng(VpStrs(1))
                VpNewEnd = CLng(VpStrs(2))
                VpName = VpStrs(0).Replace(".jpg", "")
                VgDBCommand.CommandText = "Select Offset From CardPictures Where Title = '" + VpName.Replace("'", "''") + "';"
                VpOldOffset = VgDBCommand.ExecuteScalar
                If VpOldOffset > 0 Then
                    VpOutB.Seek(VpOldOffset, SeekOrigin.Begin)
                    VpOutB.Write(VpInB.ReadBytes(VpNewEnd - VpNewOffset + 1))
                    VgDBCommand.CommandText = "Update CardPictures Set [End] = " + (VpNewEnd - VpNewOffset + VpOldOffset).ToString + " Where Title = '" + VpName.Replace("'", "''") + "';"
                    VgDBCommand.ExecuteNonQuery
                End If
            End While
            VpOutB.Flush
            VpOutB.Close
            VpLog.Close
            VpInB.Close
            Call mdlToolbox.SecureDelete(VpLogPath)
            Call mdlToolbox.SecureDelete(VpInPath)
        Else
            Return False
        End If
        Return True
    End Function
    Private Sub FixCreatures(VpPattern As String)
    '----------------------------------------------
    'Enlève les parenthèses sur l'attaque / défense
    '----------------------------------------------
        VgDBCommand.CommandText = "Update Creature Set Power = '" + VpPattern + "' Where Creature.Power = '(" + VpPattern + "';"
        VgDBCommand.ExecuteNonQuery
        VgDBCommand.CommandText = "Update Creature Set Tough = '" + VpPattern + "' Where Creature.Tough = '" + VpPattern + ")';"
        VgDBCommand.ExecuteNonQuery
    End Sub
    Private Sub FixPrices
    '-----------------------------------------
    'Remplace tous les prix non indiqués par 0
    '-----------------------------------------
        VgDBCommand.CommandText = "Update Card Set Price = 0, myPrice = 1 Where Card.EncNbr In (Select CardDouble.EncNbrDownFace From CardDouble);"
        VgDBCommand.ExecuteNonQuery
        VgDBCommand.CommandText = "Update Card Set FoilPrice = 0 Where Card.EncNbr In (Select CardDouble.EncNbrDownFace From CardDouble);"
        VgDBCommand.ExecuteNonQuery
        VgDBCommand.CommandText = "Update Card Set Price = 0, myPrice = 1 Where Card.Price In (0, Null);"
        VgDBCommand.ExecuteNonQuery
        VgDBCommand.CommandText = "Update Card Set FoilPrice = 0 Where Card.FoilPrice Is Null;"
        VgDBCommand.ExecuteNonQuery
        VgDBCommand.CommandText = "Update Card Set PriceDate = #01/01/01# Where PriceDate > Date();"
        VgDBCommand.ExecuteNonQuery
    End Sub
    Private Sub FixOnlyVO
    '----------------------------------------------------------------------------------------------------
    'Remplace les titres de cartes VF par les VO pour les éditions qui ne sont pas censées être traduites
    '----------------------------------------------------------------------------------------------------
    Dim VpSeries As New Hashtable
        'Détermine le taux de traduction des éditions
        VgDBCommand.CommandText = "Select Card.Series, Count(*) From Card Inner Join CardFR On Card.EncNbr = CardFR.EncNbr Where Card.Title <> CardFR.TitleFR Group By Card.Series;"
        VgDBReader = VgDBCommand.ExecuteReader
        With VgDBReader
            While .Read
                VpSeries.Add(.GetString(0), CSng(.GetValue(1)))
            End While
            .Close
        End With
        VgDBCommand.CommandText = "Select Card.Series, Count(*) From Card Group By Card.Series;"
        VgDBReader = VgDBCommand.ExecuteReader
        With VgDBReader
            While .Read
                If VpSeries.Contains(.GetString(0)) Then
                    VpSeries.Item(.GetString(0)) /= CSng(.GetValue(1))
                End If
            End While
            .Close
        End With
        'Si le pourcentage de cartes traduites n'atteint pas 90%, c'est sans doute que l'édition ne devait pas exister en VF : on remet les cartes traduites en VO
        For Each VpSerie As String In VpSeries.Keys
            If VpSeries.Item(VpSerie) < 0.9 Then
                VgDBCommand.CommandText = "Update CardFR Inner Join Card On CardFR.EncNbr = Card.EncNbr Set CardFR.TitleFR = Card.Title Where Card.Series = '" + VpSerie + "';"
                VgDBCommand.ExecuteNonQuery
            End If
        Next VpSerie
    End Sub
    Private Sub FixSubTypesDB(VpFull As Boolean)
    '--------------------------------------------
    'Correction des sous-types et des super-types
    '--------------------------------------------
    Dim VpSerializer As JavaScriptSerializer
    Dim VpJSONInfos As Dictionary(Of String, clsFullInfos) = Nothing
    Dim VpSubType As String
    Dim VpSubTypeVF As String
    Dim VpNewItems As List(Of String)
        'Remplace la majorité des sous-types d'enchantement par 'Aura'
        VgDBCommand.CommandText = "Update Card Set SubType = 'Aura' Where Type = 'E' And (SubType = 'Artifact' Or SubType = 'Artifact Creature' Or SubType = 'Creature' Or SubType = 'Dead Creature' Or SubType = 'Equipment' Or SubType = 'Land' Or SubType = 'Leg.Land' Or SubType = 'Mountain'  Or SubType = 'Permanent' Or SubType = 'Player' Or SubType = 'Wall');"
        VgDBCommand.ExecuteNonQuery
        'Traitements complémentaires
        If VpFull Then
            'Remplace 'Leg.' par ' Legend' à la fin
            VgDBCommand.CommandText = "Update Card Set SubType = Mid(SubType, 6) & ' Legend' Where Left(SubType, 5) = 'Leg. ';"
            VgDBCommand.ExecuteNonQuery
            VgDBCommand.CommandText = "Update Card Set SubType = Mid(SubType, 5) & ' Legend' Where Left(SubType, 4) = 'Leg.';"
            VgDBCommand.ExecuteNonQuery
            VgDBCommand.CommandText = "Update Card Set SubType = 'Legend' Where SubType = ' Legend';"
            VgDBCommand.ExecuteNonQuery
            'Essaie de télécharger le fichier JSON général depuis Internet
            If Not File.Exists(Application.StartupPath + mdlConstGlob.CgUpMultiverse) Then
                Call mdlToolbox.DownloadUnzip(New Uri(CgURL23), mdlConstGlob.CgUpMultiverse2, mdlConstGlob.CgUpMultiverse)
            End If
            'Si ça a réussi on peut passer à la correction du flag légendaire
            If File.Exists(Application.StartupPath + mdlConstGlob.CgUpMultiverse) Then
                VpNewItems = New List(Of String)
                VpSerializer = New JavaScriptSerializer
                VpSerializer.MaxJsonLength = Integer.MaxValue
                VpJSONInfos = VpSerializer.Deserialize(Of Dictionary(Of String, clsFullInfos))((New StreamReader(Application.StartupPath + mdlConstGlob.CgUpMultiverse)).ReadToEnd)
                For Each VpSerie As String In VpJSONInfos.Keys
                    For Each VpCard As clsFullInfos.clsFullCardInfos In VpJSONInfos.Item(VpSerie).cards
                        'Pour chaque carte légendaire trouvée
                        If VpCard.supertypes IsNot Nothing AndAlso VpCard.supertypes.Contains("Legendary") Then
                            VgDBCommand.CommandText = "Select Card.SubType From Card Inner Join Series On Card.Series = Series.SeriesCD Where Card.Title = '" + VpCard.name.Replace("'", "''") + "' And Series.SeriesCD_MO = '" + VpSerie + "';"
                            VpSubType = mdlToolbox.SafeGetScalarText
                            'si leur sous-type dans la base ne contient pas déjà 'Legend'
                            If Not VpSubType.Contains("Legend") Then
                                'si le sous-type est vide
                                If VpSubType = "" Then
                                    'on le met à 'Legendary'
                                    VgDBCommand.CommandText = "Update Card Inner Join Series On Card.Series = Series.SeriesCD Set Card.SubType = 'Legendary' Where Card.Title = '" + VpCard.name.Replace("'", "''") + "' And Series.SeriesCD_MO = '" + VpSerie + "';"
                                    VgDBCommand.ExecuteNonQuery
                                Else
                                    'si le sous-type existe dans la table de traduction des sous-types
                                    VgDBCommand.CommandText = "Select SubTypeVF From SubTypes Where SubTypeVO = '" + VpSubType.Replace("'", "''") + "';"
                                    VpSubTypeVF = mdlToolbox.SafeGetScalarText
                                    If VpSubTypeVF <> "" Then
                                        'on crée une nouvelle entrée identique mais avec ' Legend' à la fin en VO et ' légendaire' à la fin en VF (si ce n'est pas déjà fait)
                                        If Not VpNewItems.Contains(VpSubType) Then
                                            VgDBCommand.CommandText = "Insert Into SubTypes(SubTypeVO, SubTypeVF) Values ('" + VpSubType.Replace("'", "''") + " Legend', '" + VpSubTypeVF.Replace("'", "''") + " légendaire');"
                                            VgDBCommand.ExecuteNonQuery
                                            VpNewItems.Add(VpSubType)
                                        End If
                                    End If
                                    'on met à jour l'entrée en rajoutant ' Legend' à la fin
                                    VgDBCommand.CommandText = "Update Card Inner Join Series On Card.Series = Series.SeriesCD Set Card.SubType = '" + VpSubType.Replace("'", "''") + " Legend' Where Card.Title = '" + VpCard.name.Replace("'", "''") + "' And Series.SeriesCD_MO = '" + VpSerie + "';"
                                    VgDBCommand.ExecuteNonQuery
                                End If
                            End If
                        End If
                    Next VpCard
                Next VpSerie
                Call SecureDelete(Application.StartupPath + CgUpMultiverse)
                Call SecureDelete(Application.StartupPath + CgUpMultiverse2)
            Else
                Call mdlToolbox.ShowWarning("Impossible de trouver le fichier '" + mdlConstGlob.CgUpMultiverse + "'")
            End If
        End If
    End Sub
    Private Sub FixFR
    '--------------------------------------------------------------------------------------------
    'Remplace une traduction vide par son original anglais
    'Remplace ensuite les libellés non traduits par une traduction trouvée dans une autre édition
    '--------------------------------------------------------------------------------------------
    Dim VpMissing As New Hashtable
    Dim VpNewName As String
        VgDBCommand.CommandText = "Update CardFR Inner Join Card On Card.EncNbr = CardFR.EncNbr Set CardFR.TitleFR = Card.Title Where CardFR.TitleFR In (Null, '');"
        VgDBCommand.ExecuteNonQuery
        VgDBCommand.CommandText = "SELECT Title, EncNbr FROM (Select Card.Title, Card.Series, Card.EncNbr From Card Inner Join CardFR On Card.EncNbr = CardFR.EncNbr Where Title = TitleFR And Not Series In ('CH', 'AL', 'BE', 'UN', 'AN', 'AQ', 'LE', 'DK', 'FE')) WHERE Title In (Select Card.Title From Card Inner Join CardFR On Card.EncNbr = CardFR.EncNbr Where Title <> TitleFR) ORDER BY Series;"
        VgDBReader = VgDBCommand.ExecuteReader
        With VgDBReader
            While .Read
                VpMissing.Add(CLng(.GetValue(1)), .GetString(0))
            End While
            .Close
        End With
        For Each VpCard As Long In VpMissing.Keys
            VgDBCommand.CommandText = "Select TitleFR From Card Inner Join CardFR On Card.EncNbr = CardFR.EncNbr Where Title <> TitleFR And Title = '" + VpMissing.Item(VpCard).ToString.Replace("'", "''") + "';"
            VpNewName = VgDBCommand.ExecuteScalar.ToString
            VgDBCommand.CommandText = "Update CardFR Set TitleFR = '" + VpNewName.Replace("'", "''") + "' Where EncNbr = " + VpCard.ToString + ";"
            VgDBCommand.ExecuteNonQuery
        Next VpCard
    End Sub
    Public Function FixFR2 As Boolean
    '------------------------------------------------------------------
    'Télécharge et installe un correctif pour les traductions en défaut
    '------------------------------------------------------------------
    Dim VpLog As StreamReader
    Dim VpStrs() As String
        Call mdlToolbox.DownloadNow(New Uri(mdlConstGlob.VgOptions.VgSettings.DownloadServer + CgURL14), mdlConstGlob.CgMdTrad)
        If File.Exists(Application.StartupPath + mdlConstGlob.CgMdTrad) Then
            VpLog = New StreamReader(Application.StartupPath + mdlConstGlob.CgMdTrad)
            While Not VpLog.EndOfStream
                VpStrs = VpLog.ReadLine.Split("#")
                VgDBCommand.CommandText = "Update CardFR Inner Join Card On CardFR.EncNbr = Card.EncNbr Set CardFR.TitleFR = '" + VpStrs(1).Replace("'", "''") + "' Where Card.Title = '" + VpStrs(0).Replace("'", "''") + "';"
                VgDBCommand.ExecuteNonQuery
            End While
            VpLog.Close
            Call mdlToolbox.SecureDelete(Application.StartupPath + mdlConstGlob.CgMdTrad)
        Else
            Return False
        End If
        Return True
    End Function
    Private Sub FixFR3
    '------------------------------------------
    'Remplace les textes VF par leur version VO
    '------------------------------------------
        VgDBCommand.CommandText = "Update TextesFR Inner Join Card On Card.Title = TextesFR.CardName Set TextesFR.TexteFR = Card.CardText;"
        'VgDBCommand.CommandText = "Update TextesFR Inner Join Card On Card.Title = TextesFR.CardName Set TextesFR.TexteFR = Card.CardText Where Card.Series = 'KD';"
        VgDBCommand.ExecuteNonQuery
        VgDBCommand.CommandText = "Delete * From TextesFR Where CardName Not In (Select Distinct Card.Title From Card);"    'requête pas utile si on utilise la clause Where Card.Series =
        VgDBCommand.ExecuteNonQuery
    End Sub
    Public Function FixSubTypes As Boolean
    '-----------------------------------------------------------------
    'Télécharge et installe un correctif pour les sous-types en défaut
    '-----------------------------------------------------------------
    Dim VpLog As StreamReader
    Dim VpStrs() As String
        Call mdlToolbox.DownloadNow(New Uri(mdlConstGlob.VgOptions.VgSettings.DownloadServer + CgURL20), mdlConstGlob.CgMdSubTypes)
        If File.Exists(Application.StartupPath + mdlConstGlob.CgMdSubTypes) Then
            VpLog = New StreamReader(Application.StartupPath + mdlConstGlob.CgMdSubTypes, Encoding.Default)
            While Not VpLog.EndOfStream
                VpStrs = VpLog.ReadLine.Split("#")
                VgDBCommand.CommandText = "Update Card Set SubType = '" + VpStrs(2).Replace("'", "''") + "' Where Title = '" + VpStrs(1).Replace("'", "''") + "' And Series = '" + VpStrs(0) + "';"
                VgDBCommand.ExecuteNonQuery
            End While
            VpLog.Close
            Call mdlToolbox.SecureDelete(Application.StartupPath + mdlConstGlob.CgMdSubTypes)
        Else
            Return False
        End If
        Return True
    End Function
    Public Function FixSubTypesVF As Boolean
    '----------------------------------------------------------------------------------
    'Télécharge et installe un correctif pour les traductions des sous-types manquantes
    '----------------------------------------------------------------------------------
    Dim VpLog As StreamReader
    Dim VpStrs() As String
    Dim VpSQL As String
        Call mdlToolbox.DownloadNow(New Uri(mdlConstGlob.VgOptions.VgSettings.DownloadServer + CgURL21), mdlConstGlob.CgMdSubTypesVF)
        If File.Exists(Application.StartupPath + mdlConstGlob.CgMdSubTypesVF) Then
            VpLog = New StreamReader(Application.StartupPath + mdlConstGlob.CgMdSubTypesVF, Encoding.Default)
            While Not VpLog.EndOfStream
                VpStrs = VpLog.ReadLine.Split("#")
                VgDBCommand.CommandText = "Select Count(*) From SubTypes Where SubTypeVO = '" + VpStrs(0).Replace("'", "''") + "';"
                If CInt(VgDBCommand.ExecuteScalar) > 0 Then
                    VgDBCommand.CommandText = "Update SubTypes Set SubTypeVF = '" + VpStrs(1).Replace("'", "''") + "' Where SubTypeVO = '" + VpStrs(0).Replace("'", "''") + "';"
                Else
                    VgDBCommand.CommandText = "Insert Into SubTypes(SubTypeVO, SubTypeVF) Values ('" + VpStrs(0).Replace("'", "''") + "', '" + VpStrs(1).Replace("'", "''") + "');"
                End If
                Try
                    VgDBCommand.ExecuteNonQuery
                Catch
                    VpSQL = VgDBCommand.CommandText
                    VgDBCommand.CommandText = "Alter Table SubTypes Alter Column SubTypeVF Text(64) With Compression;"
                    VgDBCommand.ExecuteNonQuery
                    VgDBCommand.CommandText = VpSQL
                    VgDBCommand.ExecuteNonQuery
                End Try
            End While
            VpLog.Close
            Call mdlToolbox.SecureDelete(Application.StartupPath + mdlConstGlob.CgMdSubTypesVF)
        Else
            Return False
        End If
        Return True
    End Function
    Private Sub FixSerie(VpSerie As String)
    '-----------------------------------------------------------------------------------------------------------------------------------------------------
    'Correction a posteriori d'un bug initial lors de l'ajout dans la base de nouvelles cartes de créatures-artefacts dont le sous-type n'est pas Creature
    '-----------------------------------------------------------------------------------------------------------------------------------------------------
    Dim VpArtefacts As New List(Of String)
    Dim VpFile As StreamReader
    Dim VpCheckCard As clsMyCard
    Dim VpCounter As Integer = 0
        'Cherche dans la base tous les artefacts de l'édition concernée
        VgDBCommand.CommandText = "Select Title From Card Inner Join Series On Card.Series = Series.SeriesCD Where SeriesNM = '" + VpSerie.Replace("'", "''") + "' And Type = 'A';"
        VgDBReader = VgDBCommand.ExecuteReader
        With VgDBReader
            While .Read
                VpArtefacts.Add(.GetString(0))
            End While
            .Close
        End With
        'Vérifie dans la spoilerlist s'il faut les ajouter dans la table Creature (Title, Power, Tough, Nullx37)
        Me.dlgOpen4.FileName = ""
        Me.dlgOpen4.ShowDialog
        If Me.dlgOpen4.FileName <> "" Then
            VpFile = New StreamReader(Me.dlgOpen4.FileName)
            Do While Not VpFile.EndOfStream
                VpCheckCard = New clsMyCard(frmNewEdition.ParseNewCard(VpFile))
                With VpCheckCard
                    If Not .Title Is Nothing AndAlso ( .MyType = "A" And .MySubType.Contains("Creature") ) Then
                        Try
                            VgDBCommand.CommandText = "Insert Into Creature Values ('" + .Title.Replace("'", "''") + "', " + .MyPower + ", " + .MyTough + ", Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null);"
                            VgDBCommand.ExecuteNonQuery
                            VpCounter = VpCounter + 1
                        Catch
                        End Try
                    End If
                End With
            Loop
            VpFile.Close
            Call mdlToolbox.ShowInformation(VpCounter.ToString + " carte(s) ont été corrigée(s) dans la base de données...")
        End If
    End Sub
    Private Sub FixSerie2
    '--------------------------------------------------
    'Supprime les en-têtes orphelins de la table Series
    '--------------------------------------------------
        VgDBCommand.CommandText = "Delete * From Series Where SeriesCD Not In (Select Distinct Series From Card);"
        VgDBCommand.ExecuteNonQuery
    End Sub
    Private Sub FixAssoc
    '------------------------------------------------------------------------------------------------------------------------------------------
    'Correction a posteriori d'un bug initial lors de l'ajout dans la base de nouvelles cartes d'artefacts dont la couleur n'est pas référencée
    ' + créatures / créatures avec capacité
    ' + jetons
    ' + enchantements non-auras
    '------------------------------------------------------------------------------------------------------------------------------------------
        VgDBCommand.CommandText = "Update ((Select Spell.Color From Card Inner Join Spell On Card.Title = Spell.Title Where Card.Type = 'A' And Spell.Color = 'L') As MyFix) Set MyFix.Color = 'A';"
        VgDBCommand.ExecuteNonQuery
        VgDBCommand.CommandText = "Update Card Set Type = 'C' Where Type = 'U' And Trim(CardText) = '';"
        VgDBCommand.ExecuteNonQuery
        VgDBCommand.CommandText = "Update Card Set Type = 'U' Where Type = 'C' And Trim(CardText) <> '';"
        VgDBCommand.ExecuteNonQuery
        VgDBCommand.CommandText = "Update Card Inner Join Spell On Card.Title = Spell.Title Set Card.Type = 'K' Where Spell.Color = 'C';"
        VgDBCommand.ExecuteNonQuery
        VgDBCommand.CommandText = "Update Spell Set Color = 'T' Where Color = 'C';"
        VgDBCommand.ExecuteNonQuery
        VgDBCommand.CommandText = "Update Card Inner Join TextesFR On Card.Title = TextesFR.CardName Set Card.Type = 'E' Where InStr(TexteFR, 'enchanter ') > 0 And Card.Title <> 'Necromancy' And Card.Type = 'T';"
        VgDBCommand.ExecuteNonQuery
        VgDBCommand.CommandText = "Update Card Inner Join TextesFR On Card.Title = TextesFR.CardName Set Card.Type = 'E' Where InStr(TexteFR, 'enchantez ') > 0 And Card.Title <> 'Necromancy' And Card.Type = 'T';"
        VgDBCommand.ExecuteNonQuery
    End Sub
    Private Sub FixRarete
    '--------------------------------
    'Suppression des degrés de rareté
    '--------------------------------
        VgDBCommand.CommandText = "Update Card Set Rarity = Left(Rarity, 1);"
        VgDBCommand.ExecuteNonQuery
        VgDBCommand.CommandText = "Update Card Set Rarity = 'D' Where Rarity = 'L' Or Rarity = 'S';"
        VgDBCommand.ExecuteNonQuery
    End Sub
    Private Sub FixMultiverse(Optional VpCode As String = "")
    '---------------------------------------
    'Mise à jour des identifiants Multiverse
    '---------------------------------------
    Dim VpSerializer As JavaScriptSerializer
    Dim VpJSONInfos As Dictionary(Of String, clsFullInfos) = Nothing
    Dim VpJSONInfo As clsFullInfos = Nothing
    Dim VpSerieCD As String
        VgDBCommand.CommandText = "Update Card Set MultiverseId = EncNbr Where MultiverseId Is Null;"
        VgDBCommand.ExecuteNonQuery
        VgDBCommand.CommandText = "Update Card Set CardNbr = 0 Where CardNbr Is Null;"
        VgDBCommand.ExecuteNonQuery
        'Essaie de télécharger le fichier JSON général depuis Internet
        If Not File.Exists(Application.StartupPath + mdlConstGlob.CgUpMultiverse) Then
            Call mdlToolbox.DownloadUnzip(New Uri(If(VpCode = "", CgURL23, CgURL23.Replace("AllSets", VpCode))), mdlConstGlob.CgUpMultiverse2, mdlConstGlob.CgUpMultiverse)
        End If
        'Si ça a réussi on peut passer à la mise à jour effective
        If File.Exists(Application.StartupPath + mdlConstGlob.CgUpMultiverse) Then
            VpSerializer = New JavaScriptSerializer
            VpSerializer.MaxJsonLength = Integer.MaxValue
            If VpCode <> "" Then
                VpJSONInfo = VpSerializer.Deserialize(Of clsFullInfos)((New StreamReader(Application.StartupPath + mdlConstGlob.CgUpMultiverse)).ReadToEnd)
                VpJSONInfos = New Dictionary(Of String, clsFullInfos)
                VpJSONInfos.Add(VpCode, VpJSONInfo)
            Else
                VpJSONInfos = VpSerializer.Deserialize(Of Dictionary(Of String, clsFullInfos))((New StreamReader(Application.StartupPath + mdlConstGlob.CgUpMultiverse)).ReadToEnd)
            End If
            For Each VpSerie As String In VpJSONInfos.Keys
                VpSerieCD = VpSerie
                If VpSerie.Length = 4 Then  'code d'add-on d'extension, seules les 3 dernières lettres comptent
                    VpSerieCD = VpSerie.Substring(1)
                End If
                For Each VpCard As clsFullInfos.clsFullCardInfos In VpJSONInfos.Item(VpSerie).cards
                    Try
                        If VpCard.multiverseid <> 0 Then
                            VgDBCommand.CommandText = "Update Card Inner Join Series On Card.Series = Series.SeriesCD Set Card.MultiverseId = " + VpCard.multiverseid.ToString + " Where Card.Title = '" + VpCard.name.Replace("'", "''") + "' And Series.SeriesCD_MO = '" + VpSerieCD + "';"
                            VgDBCommand.ExecuteNonQuery
                        End If
                        If VpCard.number <> 0 Then
                            VgDBCommand.CommandText = "Update Card Inner Join Series On Card.Series = Series.SeriesCD Set Card.CardNbr = " + VpCard.number.ToString + " Where Card.Title = '" + VpCard.name.Replace("'", "''") + "' And Series.SeriesCD_MO = '" + VpSerieCD + "';"
                            VgDBCommand.ExecuteNonQuery
                        End If
                    Catch
                    End Try
                Next VpCard
            Next VpSerie
            Call SecureDelete(Application.StartupPath + CgUpMultiverse)
            Call SecureDelete(Application.StartupPath + CgUpMultiverse2)
        Else
            Call mdlToolbox.ShowWarning("Impossible de trouver le fichier '" + mdlConstGlob.CgUpMultiverse + "'")
        End If
    End Sub
    Public Function FixMultiverse2 As Boolean
    '-----------------------------------------------------------------------
    'Télécharge et installe une mise à jour pour les identifiants Multiverse
    '-----------------------------------------------------------------------
    Dim VpLog As StreamReader
    Dim VpStrs() As String
        Call mdlToolbox.DownloadNow(New Uri(mdlConstGlob.VgOptions.VgSettings.DownloadServer + CgURL22), mdlConstGlob.CgMdMultiverse)
        If File.Exists(Application.StartupPath + mdlConstGlob.CgMdMultiverse) Then
            VpLog = New StreamReader(Application.StartupPath + mdlConstGlob.CgMdMultiverse, Encoding.UTF8)
            While Not VpLog.EndOfStream
                VpStrs = VpLog.ReadLine.Split("#")
                VgDBCommand.CommandText = "Update Card Set MultiverseId = " + VpStrs(2) + " Where Title = '" + VpStrs(0).Replace("'", "''") + "' And Series = '" + VpStrs(1) + "';"
                VgDBCommand.ExecuteNonQuery
                VgDBCommand.CommandText = "Update Card Set CardNbr = " + VpStrs(3) + " Where Title = '" + VpStrs(0).Replace("'", "''") + "' And Series = '" + VpStrs(1) + "';"
                VgDBCommand.ExecuteNonQuery
            End While
            VpLog.Close
            Call mdlToolbox.SecureDelete(Application.StartupPath + mdlConstGlob.CgMdMultiverse)
        Else
            Return False
        End If
        Return True
    End Function
    Private Sub GoFind
    '------------------------------------------
    'Déclenche la procédure de recherche simple
    '------------------------------------------
        If Me.tvwExplore.Nodes.Count > 0 Then
            Me.mnuFindNext.Enabled = True
            VmSearch.ItemsFound.Clear
            Call Me.RecurFindCard
            VmSearch.CurItem = -1
            Call Me.FindNextCard
        End If
    End Sub
    Private Sub RecurFindCard
        For Each VpNode As TreeNode In Me.tvwExplore.Nodes
            Call Me.FindCard(VpNode)
        Next VpNode
    End Sub
    Private Sub FindCard(VpNode As TreeNode)
    '-------------------------------------------------------------------------------
    'Mémorise les cartes dont le titre contient le texte recherché par l'utilisateur
    '-------------------------------------------------------------------------------
    Dim VpChild As TreeNode
    Dim VpQuery As String = Me.mnuSearchText.Text.ToUpper
        Try
            If VpNode.Parent.Tag.Key = "Card.Title" And ( VpNode.Tag.Value.ToUpper.IndexOf(VpQuery) <> -1 Or VpNode.Tag.Value2.ToUpper.IndexOf(VpQuery) <> -1) Then
                VmSearch.ItemsFound.Add(VpNode)
            End If
        Catch
        End Try
        For Each VpChild In VpNode.Nodes
            Call Me.FindCard(VpChild)
        Next VpChild
    End Sub
    Private Sub FindNextCard
    '---------------------
    'Poursuit la recherche
    '---------------------
    Dim VpNode As TreeNode
        With VmSearch
            .CurItem = .CurItem + 1
            If .CurItem < .ItemsFound.Count Then
                Me.tvwExplore.BeginUpdate
                Me.tvwExplore.CollapseAll
                VpNode = .ItemsFound.Item(.CurItem)
                VpNode.Expand
                VpNode.EnsureVisible
                Me.tvwExplore.SelectedNode = VpNode
                Me.tvwExplore.EndUpdate
            Else
                Call mdlToolbox.ShowInformation("Aucune autre occurence trouvée !")
                Me.mnuFindNext.Enabled = False
            End If
        End With
    End Sub
    Private Sub ClearCarac
    '--------------------------------------------------------
    'Efface les détails de la carte précédemment sélectionnée
    '--------------------------------------------------------
        Me.propAlternate.SelectedObject = Nothing
        Call Me.DeBuildCost
        Me.lblPowerTough.Text = ""
        Me.txtRichCard.Clear
        Me.txtRichOther.Clear
        Me.btHistPrices.Enabled = False
        Me.btShowAll.Enabled = (Not Me.IsInAdvSearch) And (Not Me.btCardUse.Checked)
        Call Me.InitGrids
        Me.picScanCard.Image = Image.FromFile(VgOptions.VgSettings.MagicBack)
        Call Me.LoadAutorisations("")
    End Sub
    Private Sub ClearAll
        VmAdvSearch = ""
        VmAdvSearchLabel = ""
        Me.tvwExplore.Nodes.Clear
        Me.mnuFindNext.Enabled = False
        Me.mnuSearchText.Text = mdlConstGlob.CgCard
        Me.lblDB.Text = "Base -"
        Me.lblNCards.Text = ""
        Call Me.ClearCarac
    End Sub
    Private Function GetNCards(VpSource As String, VpCountMode As mdlConstGlob.eCountMode) As Integer
    '-------------------------------------------------------------------------
    'Retourne le nombre de cartes présentes dans la source passée en paramètre
    '-------------------------------------------------------------------------
    Dim VpSQL As String = ""
        Try
            Select Case VpCountMode
                Case mdlConstGlob.eCountMode.All
                    VpSQL = "Select Sum(Items) From " + VpSource + " Where " + Me.Restriction
                Case mdlConstGlob.eCountMode.Distinct
                    VpSQL = "Select Count(*) From " + VpSource + " Where " + Me.Restriction
                Case mdlConstGlob.eCountMode.NoReserve
                    VpSQL = "Select Sum(Items) From " + VpSource + " Where " + Me.Restriction + "Reserve = False"
                Case mdlConstGlob.eCountMode.OnlyReserve
                    VpSQL = "Select Sum(Items) From " + VpSource + " Where " + Me.Restriction + "Reserve = True"
                Case Else
            End Select
            VgDBCommand.CommandText = mdlToolbox.TrimQuery(VpSQL)
            Return VgDBCommand.ExecuteScalar
        Catch
            Return 0
        End Try
    End Function
    Private Sub ShowNCards(VpSource As String)
        If VpSource = mdlConstGlob.CgSDecks Then
            Me.lblNCards.Text = "|  " + Me.GetNCards(VpSource, mdlConstGlob.eCountMode.NoReserve).ToString + " cartes dans le deck, " + Me.GetNCards(VpSource, mdlConstGlob.eCountMode.OnlyReserve).ToString + " dans la réserve"
        Else
            Me.lblNCards.Text = "|  " + Me.GetNCards(VpSource, mdlConstGlob.eCountMode.All).ToString + " cartes, " + Me.GetNCards(VpSource, mdlConstGlob.eCountMode.Distinct).ToString + " distinctes"
        End If
    End Sub
    Private Function GetRootIndex(VpRoot As TreeNode) As Integer
    '---------------------------------------------------
    'Retourne le numéro de la racine passée en paramètre
    '---------------------------------------------------
    Dim VpNode As TreeNode = Me.FirstRoot
    Dim VpIndex As Integer = 0
        While VpNode IsNot VpRoot
            VpIndex += 1
            VpNode = VpNode.NextNode
        End While
        Return VpIndex
    End Function
    Private Function GetNthRoot(VpIndex As Integer) As TreeNode
    '----------------------------------------------
    'Retourne la nième racine demandée en paramètre
    '----------------------------------------------
    Dim VpNode As TreeNode = Me.FirstRoot
        For VpI As Integer = 0 To VpIndex - 1
            VpNode = VpNode.NextNode
        Next VpI
        Return VpNode
    End Function
    Private Function SaveNode(VpNode As TreeNode) As String
    '----------------------------------------------------
    'Sauvegarde la généalogie du noeud passé en paramètre
    '----------------------------------------------------
    Dim VpHistory As String = ""
    Dim VpStr As String
        If VpNode.Parent Is Nothing Then
            Return ""
        Else
            Do
                VpStr = VpNode.Text
                VpHistory = "#" + VpStr + VpHistory
                VpNode = VpNode.Parent
            Loop Until VpNode.Parent Is Nothing
            Return VpHistory.Substring(1)
        End If
    End Function
    Private Sub RecurRestoreNode(VpHistory As String, VpNode As TreeNode)
    '-------------------------------------------------------------------
    'Réouvre la généalogie passée en paramètre aussi loin qu'elle existe
    '-------------------------------------------------------------------
    Dim VpCur As String
    Dim VpLeft As String
        If VpHistory <> "" Then
            If Not VpHistory.IndexOf("#") < 0 Then
                VpCur = VpHistory.Substring(0, VpHistory.IndexOf("#"))
                VpLeft = VpHistory.Substring(VpHistory.IndexOf("#") + 1)
            Else
                VpCur = VpHistory
                VpLeft = ""
            End If
            For Each VpChild As TreeNode In VpNode.Nodes
                If VpChild.Text = VpCur Then
                    Me.tvwExplore.SelectedNode = VpChild
                    VpChild.Expand
                    Call Me.RecurRestoreNode(VpLeft, VpChild)
                End If
            Next VpChild
        End If
    End Sub
    Private Sub RestoreNode(VpHistory As String, VpNode As TreeNode)
        Call Me.RecurRestoreNode(VpHistory, VpNode)
        If Not Me.tvwExplore.SelectedNode Is Nothing Then
            Me.tvwExplore.SelectedNode.EnsureVisible
        End If
    End Sub
    Private Sub ReloadWithHistory
    Dim VpHistory As String = Me.SaveNode(Me.tvwExplore.SelectedNode)
    Dim VpMyRoot As Integer = Me.GetRootIndex(Me.MyRoot)
        Call Me.LoadTvw
        Me.tvwExplore.BeginUpdate
        Call Me.RestoreNode(VpHistory, Me.GetNthRoot(VpMyRoot))
        Me.tvwExplore.EndUpdate
    End Sub
    Private Sub InitBars(VpMax As Integer)
    '----------------------------------------
    'Initialisation des barres de progression
    '----------------------------------------
        Me.prgAvance.Style = ProgressBarStyle.Blocks
'       VgBar.Style = ProgressBarStyle.Blocks
        Me.prgAvance.Maximum = VpMax
'       VgBar.Maximum = VpMax
        Me.prgAvance.Value = 0
'       VgBar.Value = 0
        Me.prgAvance.Visible = True
'       VgBar.ShowInTaskbar = True
    End Sub
    Private Sub InitGrids
        If Me.btCardUse.Checked Then
            Call mdlToolbox.InitGrid(Me.grdPropCard, New String() {"Source", "Stock total"})
        Else
            If Me.IsInAdvSearch Then
                Call mdlToolbox.InitGrid(Me.grdPropCard, New String() {"Edition", "Rareté", "Prix (€)", "Prix foil (€)"})
            Else
                Call mdlToolbox.InitGrid(Me.grdPropCard, New String() {"Edition", "Rareté", "Prix (€)", "Prix foil (€)", "Stock", "Stock foil"})
            End If
            Call mdlToolbox.InitGrid(Me.grdPropPicture, New String() {"Edition", "Illustrateur"})
        End If
    End Sub
    Private Sub CheckGridBusy
    '----------------------------------------------------------
    'S'assure que les opérations sur la grille soient terminées
    '----------------------------------------------------------
    Dim VpBusy As Boolean
        With Me.grdPropCard
            If .Selection.Count = 0 OrElse .Selection.GetCells(0).DataModel Is Nothing Then
                VpBusy = False
            Else
                VpBusy = .Selection.GetCells(0).DataModel.IsEditing
            End If
            If VpBusy Then
                SendKeys.Send("{ENTER}")        'crade mais force à valider la cellule en cours d'édition dans la grille si elle est cohérente...
                Application.DoEvents
                SendKeys.Send("{ESC}")          '...ou à l'annuler si elle ne l'est pas (auquel cas le {ENTER} précédent n'aura eu aucun effet)
                Application.DoEvents
            End If
        End With
    End Sub
    Public Sub LoadMnu
    '------------------------------
    'Chargement des menus variables
    '------------------------------
    Dim VpI As Integer
    Dim VpN As Integer = Me.mnuDisp.DropDownItems.Count - 1
        'Nettoyage
        For VpI = 1 + mdlConstGlob.CgNDispMenuBase To VpN
            Me.mnuDisp.DropDownItems.RemoveAt(Me.mnuDisp.DropDownItems.Count - 1)
            Me.mnuFixGames.DropDownItems.RemoveAt(Me.mnuFixGames.DropDownItems.Count - 1)
            Me.mnuRemGames.DropDownItems.RemoveAt(Me.mnuRemGames.DropDownItems.Count - 1)
            Me.mnuMoveACard.DropDownItems.RemoveAt(Me.mnuMoveACard.DropDownItems.Count - 1)
            Me.mnuCopyACard.DropDownItems.RemoveAt(Me.mnuCopyACard.DropDownItems.Count - 1)
        Next VpI
        'Reconstruction
        Call Me.RecurLoadMnu(Me.mnuDisp, "Is Null", AddressOf MnuDispCollectionActivate)
        Call Me.RecurLoadMnu(Me.mnuFixGames, "Is Null", AddressOf MnuFixSubGamesActivate)
        Call Me.RecurLoadMnu(Me.mnuRemGames, "Is Null", AddressOf MnuRemSubGamesActivate)
        Call Me.RecurLoadMnu(Me.mnuMoveACard, "Is Null", AddressOf mnuMoveACardActivate)
        Call Me.RecurLoadMnu(Me.mnuCopyACard, "Is Null", AddressOf mnuCopyACardActivate)
        'Pour les éditions
        Me.mnuFixSerie.DropDownItems.Clear
        VgDBCommand.CommandText = "Select SeriesNM From Series Order By Release Desc;"
        VgDBReader = VgDBCommand.ExecuteReader
        With VgDBReader
            While .Read
                Me.mnuFixSerie.DropDownItems.Add(.GetString(0), Nothing, AddressOf MnuFixSerieActivate)
            End While
            .Close
        End With
        Me.mnuDispAdvSearch.Checked = False
        Me.mnuDispCollection.Checked = True
        VmDeckMode = False
    End Sub
    Public Sub RecurLoadMnu(VpCur As ToolStripMenuItem, VpParent As String, VpHandler As EventHandler)
        For Each VpChild As Integer In mdlToolbox.GetChildrenDecksIds(VpParent)
            Call Me.RecurLoadMnu(VpCur.DropDownItems.Add(mdlToolbox.GetDeckNameFromId(VpChild), Nothing, If(mdlToolbox.IsDeckFolder(VpChild), Nothing, VpHandler)), " = " + VpChild.ToString, VpHandler)
        Next VpChild
    End Sub
    Private Sub SortTvw
    '-----------------------------------------
    'Tri le treeview dans l'ordre alphabétique
    '-----------------------------------------
    Dim VpNode As TreeNode = Me.tvwExplore.SelectedNode
        Me.tvwExplore.Sort
        If Not VpNode Is Nothing Then
            Me.tvwExplore.SelectedNode = VpNode
            VpNode.EnsureVisible
        End If
    End Sub
    Public Sub LoadTvw(Optional VpLoadFromSearch As String = "", Optional VpClear As Boolean = True, Optional VpSearchName As String = mdlConstGlob.CgFromSearch)
        Cursor.Current = Cursors.WaitCursor
        Call Me.LoadDualTvw(VpLoadFromSearch, VpClear, VpSearchName)
        If VmDeckMode Then
            Call Me.LoadDualTvw(VpLoadFromSearch, False, VpSearchName, True)
        End If
        If Me.btExpand.Checked Then
            Me.tvwExplore.ExpandAll
        End If
        Me.FirstRoot.EnsureVisible
    End Sub
    Private Sub LoadDualTvw(VpLoadFromSearch As String, VpClear As Boolean, VpSearchName As String, Optional VpReserve As Boolean = False)
    '-----------------------------------------------------------------------------------------------------------------------------------
    'Chargement du treeview avec les sélections spécifiées dans le menu Affichage ou bien les résultats de la recherche de l'utilisateur
    '-----------------------------------------------------------------------------------------------------------------------------------
    Dim VpNode As TreeNode
    Dim VpSource As String = Me.MySource
        If Not mdlToolbox.DBOK Then Exit Sub
        If VpLoadFromSearch <> "" Then
            VmAdvSearch = VpLoadFromSearch
            VmAdvSearchLabel = VpSearchName
        End If
        Me.tvwExplore.SelectedNodes.Clear
        If VpClear Then
            Me.tvwExplore.Nodes.Clear
            Me.tvwExplore.ShowLines = VgOptions.VgSettings.ShowLines
        End If
        Me.mnuFindNext.Enabled = False
        Me.mnuSearchText.Text = mdlConstGlob.CgCard
        Me.lblDB.Text = VgDB.DataSource
        Call Me.ClearCarac
        If Not Me.IsSourcePresent And Not Me.IsInAdvSearch Then
            Call mdlToolbox.ShowWarning("Aucune source de cartes n'a été sélectionnée...")
        Else
            VpNode = New TreeNode
            VpNode.Tag = New clsTag
            VpNode.ImageIndex = 1
            VpNode.SelectedImageIndex = 1
            'Cas 1 : chargement des résultats d'une recherche de l'utilisateur
            If Me.IsInAdvSearch Then
                VmFilterCriteria.MyList.SetItemChecked(VmFilterCriteria.MyList.Items.IndexOf("Quantité"), False)
                VpNode.Text = VmAdvSearchLabel
                Try
                    VpNode.Tag.Key = CgCriteres.Item(VmFilterCriteria.MyList.CheckedItems(0))
                Catch
                    Call mdlToolbox.ShowWarning(mdlConstGlob.CgErr7)
                End Try
                Call Me.RecurLoadTvw(VmAdvSearch, mdlConstGlob.CgSFromSearch, VpNode, 1, Me.Restriction, VpReserve)
                Me.lblNCards.Text = ""
                VmSuggestions = Nothing
            'Cas 2 : chargement des cartes de la collection ou d'un deck
            Else
                VpNode.Text = If(Not VpReserve, Me.GetSelectedSource, mdlConstGlob.CgSide)
                Try
                    VpNode.Tag.Key = CgCriteres.Item(VmFilterCriteria.MyList.CheckedItems(0))
                Catch
                    Call mdlToolbox.ShowWarning(mdlConstGlob.CgErr7)
                End Try
                Call Me.RecurLoadTvw(VpSource, VpSource, VpNode, 1, Me.Restriction, VpReserve)
                Call Me.ShowNCards(VpSource)
            End If
            Me.tvwExplore.Nodes.Add(VpNode)
            VpNode.Expand
            Me.mnuCardsFR.Enabled = True
            'Restauration des paramètres langue / tri (NB. Si on est en VO on est toujours en ordre alphabétique)
            If Me.IsInVFMode Then
                Me.tvwExplore.BeginUpdate
                Call Me.RecurChangeLanguage(True)
                Call Me.SortTvw
                Me.tvwExplore.EndUpdate
            End If
        End If
        VmMustReload = False
    End Sub
    Private Function CanAdd(VpNodeTag As clsTag, VpNodeTitle As String) As Boolean
    '--------------------------------------------------------------------------
    'Si l'on est en mode suggestions, empêche l'insertion d'un noeud non validé
    '--------------------------------------------------------------------------
        If VpNodeTag.Key = "Card.Title" And Not VmSuggestions Is Nothing Then
            For Each VpSugg As clsCorrelation In VmSuggestions
                If VpSugg.Card1 = VpNodeTitle Then
                    Return True
                End If
            Next VpSugg
            Return False
        Else
            Return ( VpNodeTitle <> "" )
        End If
    End Function
    Private Sub RecurLoadTvw(VpSource1 As String, VpSource2 As String, VpNode As TreeNode, VpRecurLevel As Integer, VpRestriction As String, VpReserve As Boolean)
    '---------------------------------------------------------------------------------------------------------------------
    'Méthode de chargement récursive du treeview : à chaque niveau i, sélectionne les cartes correspondant au ième critère
    'remplissant également les critères de la branche courante de i-1 jusqu'à 1
    '---------------------------------------------------------------------------------------------------------------------
    Dim VpChild As TreeNode             'Enfant du noeud courant
    Dim VpParent As TreeNode = VpNode   'Ancêtres du noeud courant
    Dim VpSQL As String                 'Requête construite adaptativement
    Dim VpStr As String
    Dim VpCurTag As clsTag = VpNode.Tag 'Limite les liaisons tardives
    Dim VpChildTag As clsTag
        'Lorsque le niveau de récursivité courant dépasse le nombre de critères sélectionnés, c'est que l'arborescence est complète
        If VpRecurLevel > VmFilterCriteria.NSelectedCriteria Then Exit Sub
        'La requête s'effectue dans les deux tables Card et Spell mises en correspondances sur le nom de la carte, elles-mêmes mises en correspondance avec MyGames ou MyCollection sur le numéro encyclopédique
        If VpCurTag.Key = "Card.Title" Then
            VpSQL = "Select Distinct Card.Title, Spell.Color, CardFR.TitleFR, Card.SpecialDoubleCard, Max(Card.MultiverseId) From ((" + VpSource1 + " Inner Join Card On " + VpSource2 + ".EncNbr = Card.EncNbr) Inner Join Spell On Card.Title = Spell.Title) Inner Join CardFR On CardFR.EncNbr = Card.EncNbr Where "
            VpStr = " Group By Card.Title, Spell.Color, CardFR.TitleFR, Card.SpecialDoubleCard"
        Else
            VpSQL = "Select Distinct " + VpCurTag.Key + " From (" + VpSource1 + " Inner Join Card On " + VpSource2 + ".EncNbr = Card.EncNbr) Inner Join Spell On Card.Title = Spell.Title Where "
            VpStr = ""
        End If
        'Ajoute les conditions sur les identifiants des jeux
        VpSQL = VpSQL + VpRestriction
        'Ajoute éventuellement la condition sur le flag réserve
        If VpSource1 = mdlConstGlob.CgSDecks Then
            VpSQL = VpSQL + "Reserve = " + VpReserve.ToString + " And "
        End If
        'Ajoute les conditions sur les critères des ancêtres
        While Not VpParent.Parent Is Nothing
            VpSQL = VpSQL + Me.ElderCriteria(CType(VpParent.Tag, clsTag).Value, CType(VpParent.Parent.Tag, clsTag).Key)
            VpParent = VpParent.Parent
        End While
        'Suppression des mots-clés inutiles et agrégation
        VpSQL = mdlToolbox.TrimQuery(VpSQL, , VpStr)
        'Mémorise la requête
        CType(VpNode.Tag, clsTag).Descendance = VpSQL
        'Exécution de la requête
        VgDBCommand.CommandText = VpSQL
        VgDBReader = VgDBcommand.ExecuteReader
        With VgDBReader
            While .Read
                'Ajoute un enfant par enregistrement trouvé
                VpStr = .GetValue(0).ToString
                If Me.CanAdd(VpCurTag, VpStr) Then
                    VpChild = New TreeNode
                    VpChildTag = New clsTag(VpStr)
                    'Si on est au niveau du nom des cartes, l'icône est celle de la couleur de la carte
                    If VpCurTag.Key = "Card.Title" Then
                        VpChild.ImageIndex = Me.FindImageIndex(VpCurTag.Key, .GetString(1))
                    'Sinon l'icône est celle du critère associé
                    Else
                        VpChild.ImageIndex = Me.FindImageIndex(VpCurTag.Key, VpChildTag.Value)
                    End If
                    'Icône identique lorsque l'élément est mis en surbrillance
                    VpChild.SelectedImageIndex = VpChild.ImageIndex
                    'Tant que l'on est pas au dernier niveau, on tag les éléments avec le critère requis pour leur descendance
                    If VpRecurLevel < VmFilterCriteria.NSelectedCriteria Then
                        VpChildTag.Key = mdlConstGlob.CgCriteres.Item(VmFilterCriteria.MyList.CheckedItems(VpRecurLevel))
                        'Caption explicite
                        VpChild.Text = mdlToolbox.FormatTitle(VpCurTag.Key, VpChildTag.Value, Me.IsInVFMode)
                    'Si on est au niveau du nom des cartes, il faut mémoriser dans le tag des paramètres supplémentaires
                    ElseIf VpCurTag.Key = "Card.Title" Then
                        'Identifiant universel
                        If Not .IsDBNull(4) Then
                            VpChildTag.MultiverseId = CLng(.GetValue(4))
                        Else
                            VpChildTag.MultiverseId = 0
                        End If
                        'Traduction
                        VpChildTag.Value2 = .GetString(2)
                        'Flag double carte
                        VpChildTag.Value3 = .GetBoolean(3)
                        If .GetBoolean(3) Then
                            VpChild.NodeFont = New Font(Me.tvwExplore.Font, FontStyle.Bold)
                        End If
                        VpChild.Text = VpChildTag.Value
                    End If
                    'Ajout effectif
                    VpChild.Tag = VpChildTag
                    VpNode.Nodes.Add(VpChild)
                End If
            End While
            .Close
        End With
        'Appel récursif sur chaque enfant au niveau supérieur
        For Each VpChild In VpNode.Nodes
            Call Me.RecurLoadTvw(VpSource1, VpSource2, VpChild, VpRecurLevel + 1, VpRestriction, VpReserve)
        Next VpChild
    End Sub
    Private Function ElderCriteria(VpValue As String, VpField As String) As String
        Select Case VpField
            'Champs numériques
            Case "Card.myPrice", "Spell.myCost", "Items"
                Return VpField + " = " + VpValue + " And "
            'Champs textuels
            Case Else
                Return VpField + " = '" + VpValue + "' And "
        End Select
    End Function
    Public Function Restriction(Optional VpTextMode As Boolean = False) As String
    '------------------------------------------------------------------------
    'Retourne une clause de restriction pour n'afficher que les jeux demandés
    '------------------------------------------------------------------------
    Dim VpStr As String = ""
        If Not VmDeckMode Or Me.IsInAdvSearch Then
            Return If(VpTextMode, mdlConstGlob.CgCollection, "")
        End If
        Call Me.RecurRestriction(Me.mnuDisp, VpStr, VpTextMode)
        'Retourne la restriction de manière textuelle (non pour une utilisation dans une requête)
        If VpTextMode Then
            Return VpStr
        'Retourne la clause de restriction en mode SQL
        ElseIf VpStr.Length > 4 Then
            Return "(" + VpStr.Substring(0, VpStr.Length - 4) + ") And "
        'Cas où aucune source n'est sélectionnée
        Else
            Return "GameID = -1"
        End If
    End Function
    Public Sub RecurRestriction(VpToolStripMenuItem As ToolStripMenuItem, ByRef VpStr As String, VpTextMode As Boolean)
        For Each VpItem As ToolStripItem In VpToolStripMenuItem.DropDownItems
            If TypeOf VpItem Is ToolStripMenuItem Then
                If CType(VpItem, ToolStripMenuItem).Checked Then
                    If VpTextMode Then
                        VpStr = VpStr + VpItem.Text + " "
                    Else
                        VpStr = VpStr + "GameID = " + mdlToolbox.GetDeckIdFromName(VpItem.Text) + " Or "
                    End If
                End If
                Call Me.RecurRestriction(VpItem, VpStr, VpTextMode)
            End If
        Next VpItem
    End Sub
    Private Function QueryInfo(VpQuery As String, VpElderCriteria As String, Optional VpAddendum As String = "") As String
    '------------------
    'Requête ponctuelle
    '------------------
    Dim VpO As Object
    Dim VpSQL As String
        VpSQL = VpQuery
        VpSQL = VpSQL + Me.Restriction + VpElderCriteria
        VpSQL = mdlToolbox.TrimQuery(VpSQL, , VpAddendum)
        VgDBCommand.CommandText = VpSQL
        VpO = VgDBCommand.ExecuteScalar
        If Not VpO Is Nothing Then
            Return VpO.ToString
        Else
            Return "0"
        End If
    End Function
    Private Sub LoadCaracOther(VpCritere As String, VpModeCarac As mdlConstGlob.eModeCarac, VpPartialElderCriteria As String)
    '------------------------------------------------------------------------------------
    'Charge des informations sur {édition, couleur, type} sélectionné(e) dans le treeview
    '------------------------------------------------------------------------------------
    Dim VpCaracOther As New clsCaracOther
    Dim VpCaracSerie As clsCaracEdition
    Dim VpO As Object
    Dim VpSource As String = Me.MySource
    Dim VpElderCriteria As String
        If Me.IsMainReaderBusy Then
            Exit Sub
        End If
        If VpSource = mdlConstGlob.CgSDecks Then
            VpElderCriteria = VpPartialElderCriteria + "Reserve = " + Me.IsReserveSelected.ToString
        Else
            VpElderCriteria = VpPartialElderCriteria
        End If
        With VpCaracOther
            'Nombre de cartes total répondant aux critères
            VgDBCommand.CommandText = "Select Count(*) From (Select Distinct Card.Title From Card Inner Join Spell On Card.Title = Spell.Title Where Card.EncNbr Not In (Select EncNbrDownFace From CardDouble) And " + mdlToolbox.TrimQuery(VpPartialElderCriteria, False) + ");"
            .TotalCards = VgDBCommand.ExecuteScalar
            'Nombre de carte possédées répondant aux critères
            .MyTotalCards = Me.QueryInfo("Select Sum(Items) From (" + VpSource + " Inner Join Card On " + VpSource + ".EncNbr = Card.EncNbr) Inner Join Spell On Card.Title = Spell.Title Where ", VpElderCriteria)
            'Nombre de carte possédées distinctes répondant aux critères
            .MyTotalDistinctCards = Me.QueryInfo("Select Count(*) From (Select Distinct " + VpSource + ".EncNbr From (" + VpSource + " Inner Join Card On " + VpSource + ".EncNbr = Card.EncNbr) Inner Join Spell On Card.Title = Spell.Title Where ", VpElderCriteria, ")")
            .MyTotalDistinctCards += " (" + Format(100 * CInt(.MyTotalDistinctCards) / CInt(.TotalCards), "0") + "%)"
            'Cote de toutes les cartes répondant aux critères
            VgDBCommand.CommandText = "Select Sum(IIf(Foil, FoilPrice, Price) * Items) From (" + VpSource + " Inner Join Card On " + VpSource + ".EncNbr = Card.EncNbr) Inner Join Spell On Card.Title = Spell.Title Where " + Me.Restriction + mdlToolbox.TrimQuery(VpElderCriteria)
            VpO = VgDBCommand.ExecuteScalar
            If Not VpO Is Nothing AndAlso IsNumeric(VpO) Then
                .TotalPricing = Format(VpO, "0.00") + " €"
            Else
                .TotalPricing = "N/A"
            End If
        End With
        If Me.propAlternate.PropertySort <> PropertySort.NoSort Then
            Me.propAlternate.PropertySort = PropertySort.NoSort
            Me.tvwExplore.Focus
        End If
        'Cas particuliers
        Select Case VpModeCarac
            Case mdlConstGlob.eModeCarac.Serie
                VpCaracSerie = New clsCaracEdition(VpCaracOther)
                'Date de sortie
                VgDBCommand.CommandText = "Select Release From Series Where SeriesCD = '" + VpCritere + "';"
                VpCaracSerie.SerieDate = CDate(VgDBCommand.ExecuteScalar).ToShortDateString
                'Notes
                VgDBCommand.CommandText = "Select Notes From Series Where SeriesCD = '" + VpCritere + "';"
                VpO = VgDBCommand.ExecuteScalar
                Me.txtRichOther.Clear
                If Not VpO Is Nothing Then
                    Me.txtRichOther.Text = VpO.ToString
                End If
                Me.propAlternate.SelectedObject = VpCaracSerie
            Case Else
                Me.txtRichOther.Text = (New ResourceManager(mdlConstGlob.CgProject, Assembly.GetExecutingAssembly)).GetString(VpCritere)
                Me.propAlternate.SelectedObject = VpCaracOther
        End Select
    End Sub
    Private Sub ManageMode(VpCardMode As Boolean)
        With mdlConstGlob.VgSessionSettings
            If VpCardMode Then
                Me.grdPropCard.Visible = True
                Me.grdPropPicture.Visible = True
                Me.btHistPrices.Enabled = True
                Me.btHistPrices.Visible = True
                Me.btShowAll.Visible = True
                Me.btCardUse.Visible = True
                Me.pnlCard.BringToFront
            Else
                Me.grdPropCard.Visible = False
                Me.grdPropPicture.Visible = False
                Me.pnlCard.SendToBack
                Me.btHistPrices.Visible = False
                Me.btShowAll.Visible = False
                Me.btCardUse.Visible = False
            End If
        End With
    End Sub
    Public Sub ManageDispMenu(VpToolStripMenuItem As ToolStripMenuItem, VpMenuTitle As String)
        For Each VpItem As ToolStripItem In VpToolStripMenuItem.DropDownItems
            If TypeOf VpItem Is ToolStripMenuItem Then
                'On sélectionne les résultats de recherche
                If VpItem.Text = VpMenuTitle Then
                    CType(VpItem, ToolStripMenuItem).Checked = True
                ElseIf VpItem.Text = mdlConstGlob.CgRefresh Or VpItem.Text = mdlConstGlob.CgPanel Or VpItem.Text = "" Then
                'mais on déselectionne tous les decks ainsi que la collection
                Else
                    CType(VpItem, ToolStripMenuItem).Checked = False
                End If
                Call Me.ManageDispMenu(VpItem, VpMenuTitle)
            End If
        Next VpItem
    End Sub
    Public Sub ManageDispMenu(VpMenuTitle As String, VpDeckMode As Boolean)
        Call Me.ManageDispMenu(Me.mnuDisp, VpMenuTitle)
        VmDeckMode = VpDeckMode
    End Sub
    Private Function ShowCard(VpTitle As String, VpMultiverseId As Long, VpDownFace As Boolean, VpTransformed As Boolean, VpReserve As Boolean) As Boolean
    '-------------------------------------------------------------------
    'Affiche les infos d'une carte après sa sélection dans l'explorateur
    '-------------------------------------------------------------------
        If Not VpTitle Is Nothing AndAlso VpTitle <> "" Then
            Call Me.LoadCarac(VpTitle, VpDownFace, VpTransformed, VpReserve)
            If Not Me.splitV2.Panel2Collapsed Then
                Call mdlToolbox.LoadScanCard(VpTitle, VpMultiverseId, Me.picScanCard)
            End If
            If Me.grpAutorisations.Visible Then
                Call Me.LoadAutorisations(VpTitle)
            End If
            Call Me.ManageMode(True)
            Return True
        End If
        Return False
    End Function
    Private Sub ClearGrid(VpGrid As Grid)
        With VpGrid
            If .Rows.Count > 1 Then
                .Rows.RemoveRange(1, .Rows.Count - 1)
            End If
        End With
    End Sub
    Private Sub LoadCarac(VpCard As String, VpDownFace As Boolean, VpTransformed As Boolean, VpReserve As Boolean)
        Call Me.LoadCaracDetails(VpCard, VpDownFace, VpTransformed, VpReserve, Me.btCardUse.Checked)
        If Me.btCardUse.Checked Then
            Call Me.LoadCaracUse(VpCard)
        End If
    End Sub
    Private Sub LoadCaracUse(VpCard As String)
    '--------------------------------------------------------------------
    'Affiche où est disponible (decks / collection) la carte sélectionnée
    '--------------------------------------------------------------------
    Dim VpMatrix As New Hashtable
    Dim VpUsage As Hashtable
    Dim VpColumns As New List(Of String)
    Dim VpRow As Integer
    Dim VpTotal As Integer
        'Calcul des présences dans la collection
        VpMatrix.Add(mdlConstGlob.CgCollection, Me.CalcUse(VpCard, mdlConstGlob.CgSCollection, mdlConstGlob.CgCollection, False))
        'Calcul des présences dans tous les decks
        For Each VpSource As String In Me.GetAllSources
            If Not VpMatrix.ContainsKey(VpSource) Then
                VpMatrix.Add(VpSource, Me.CalcUse(VpCard, mdlConstGlob.CgSDecks, VpSource, True))
            End If
        Next VpSource
        'Détermine les colonnes de la grille
        VpColumns.Add("Source")
        VpColumns.Add("Stock total")
        For Each VpUsage In VpMatrix.Values
            For Each VpSerie As String In VpUsage.Keys
                If Not VpColumns.Contains(VpSerie) Then
                    VpColumns.Add(VpSerie)
                End If
            Next VpSerie
        Next VpUsage
        'Création de la grille
        Call mdlToolbox.InitGrid(Me.grdPropCard, VpColumns.ToArray)
        'Remplissage
        For Each VpSource As String In VpMatrix.Keys
            VpUsage = VpMatrix.Item(VpSource)
            'Calcul du sous-total
            VpTotal = 0
            For Each VpItem As Integer In VpUsage.Values
                VpTotal += VpItem
            Next VpItem
            'S'il est > 0, on affiche sa ventilation
            If VpTotal > 0 Then
                VpRow = Me.grdPropCard.RowsCount
                Me.grdPropCard.Rows.Insert(VpRow)
                Me.grdPropCard(VpRow, 0) = New Cells.Cell(VpSource)
                Me.grdPropCard(VpRow, 1) = New Cells.Cell(VpTotal)
                For VpI As Integer = 2 To VpColumns.Count - 1
                    If VpUsage.ContainsKey(VpColumns.Item(VpI)) Then
                        Me.grdPropCard(VpRow, VpI) = New Cells.Cell(VpUsage.Item(VpColumns.Item(VpI)))
                    End If
                Next VpI
            End If
        Next VpSource
        Me.grdPropCard.AutoSize
    End Sub
    Private Function CalcUse(VpCard As String, VpSource As String, VpSource2 As String, VpDeckMode As Boolean) As Hashtable
    Dim VpUsage As New Hashtable
        'Sélectionne le stock par édition pour la source passée en paramètre
        VgDBCommand.CommandText = "Select Card.Series, Sum(" + VpSource + ".Items) From (Card Inner Join " + VpSource + " On " + VpSource + ".EncNbr = Card.EncNbr) Where Card.Title = '" + VpCard.Replace("'", "''") + "'" + If(VpDeckMode, " And GameID = " + mdlToolbox.GetDeckIdFromName(VpSource2), "") + " Group By Card.Series;"
        VgDBReader = VgDBCommand.ExecuteReader
        With VgDBReader
            While .Read
                VpUsage.Add(mdlToolbox.GetSerieNameFromCode(.GetString(0), Me.IsInVFMode), CInt(.GetValue(1)))
            End While
            .Close
        End With
        Return VpUsage
    End Function
    Private Sub LoadCaracDeck(VpDeck As String)
    '------------------------------------------
    'Chargement des détails du deck sélectionné
    '------------------------------------------
    Dim VpProps As clsCustomClass
    Dim VpOwned As Integer
    Dim VpRequired As Integer
    Dim VpOwnedTot As Integer = 0
    Dim VpRequiredTot As Integer = 0
        'Texte descriptif associé au deck
        Call Me.PutInRichText(Me.txtRichOther, Me.imglstCarac, mdlToolbox.GetDeckDescription(mdlToolbox.GetDeckIdFromName(Me.GetSelectedSource)), "")
        'Création dynamique d'un property grid pour voir si les cartes du deck sont aussi en collection
        VpProps = New clsCustomClass
        VgDBCommand.CommandText = "Select TGames.Title, IIf(TCollection.Total Is Null, 0, TCollection.Total), TGames.Total From (Select Card.Title, Sum(MyGames.Items) As Total From Card Inner Join MyGames On Card.EncNbr = MyGames.EncNbr Where MyGames.GameID = " + mdlToolbox.GetDeckIdFromName(VpDeck) + " Group By Card.Title) As TGames Left Join (Select Card.Title, Sum(MyCollection.Items) As Total From Card Inner Join MyCollection On Card.EncNbr = MyCollection.EncNbr Group By Card.Title) As TCollection On TGames.Title = TCollection.Title"
        VgDBReader = VgDBCommand.ExecuteReader
        With VgDBReader
            'Pour chaque carte
            While .Read
                VpOwned = CInt(.GetValue(1))
                VpRequired = CInt(.GetValue(2))
                'Cas 1 : on possède tous les exemplaires requis
                If VpOwned >= VpRequired Then
                    VpProps.Add(New clsCustomProperty(.GetString(0), "Cartes possédées", VpRequired.ToString + " / " + VpRequired.ToString, GetType(String), False, True))
                    VpOwnedTot += VpRequired
                'Cas 2 : on n'en possède aucun
                ElseIf VpOwned = 0 Then
                    VpProps.Add(New clsCustomProperty(.GetString(0), "Cartes manquantes", "0 / " + VpRequired.ToString, GetType(String), False, True))
                'Cas 3 : possession partielle
                Else
                    VpProps.Add(New clsCustomProperty(.GetString(0), "Cartes possédées", VpOwned.ToString + " / " + VpRequired.ToString, GetType(String), False, True))
                    VpProps.Add(New clsCustomProperty(.GetString(0), "Cartes manquantes", VpOwned.ToString + " / " + VpRequired.ToString, GetType(String), False, True))
                    VpOwnedTot += VpOwned
                End If
                VpRequiredTot += VpRequired
            End While
            .Close
        End With
        'Synthèse en %
        For Each VpProp As clsCustomProperty In VpProps
            If VpProp.Category = "Cartes possédées" Then
                VpProp.Category += " (" + Format(100 * VpOwnedTot / VpRequiredTot, "0") + " %)"
            Else
                VpProp.Category += " (" + Format(100 * (VpRequiredTot - VpOwnedTot) / VpRequiredTot, "0") + " %)"
            End If
        Next VpProp
        Me.propAlternate.PropertySort = PropertySort.CategorizedAlphabetical
        Me.propAlternate.SelectedObject = VpProps
        If VpProps.Count > 0 AndAlso Me.propAlternate.SelectedGridItem.Parent.Parent IsNot Nothing Then
            Me.propAlternate.SelectedGridItem.Parent.Parent.GridItems(0).Select
        End If
    End Sub
    Private Sub LoadCaracDetails(VpCard As String, VpDownFace As Boolean, VpTransformed As Boolean, VpReserve As Boolean, VpSkipGrid As Boolean)
    '-----------------------------------------------
    'Chargement des détails de la carte sélectionnée
    '-----------------------------------------------
    Dim VpSQLForCreatures As String         'Requête complète générée pour demander des infos dans toutes les tables, y compris celle spécifique aux créatures
    Dim VpSQLForAll As String               'Requête complète générée pour demander des infos dans toutes les tables sauf celle spécifique aux créatures
    Dim VpSQLGeneralCreatures As String     'Sous-table d'infos générales, y compris celle spécifique aux créatures
    Dim VpSQLGeneralAll As String           'Sous-table d'infos générales sauf celle spécifique aux créatures
    Dim VpSQLStockInfosDecksFoil As String  'Sous-table d'infos liées au stock dans les decks, en dissociant les cartes foils
    Dim VpSQLStockInfosCollecFoil As String 'Sous-table d'infos liées au stock dans la collection, en dissociant les cartes foils
    Dim VpSQLColumnsRequired As String      'Colonnes communes demandées dans le résultat final
    Dim VpSQLTitleCriteria As String        'Clause sur le nom de la carte
    Dim VpSQLSorting As String              'Critère de tri
    Dim VpSQLJointure As String             'Type de jointure (Inner ou Left, selon qu'on veut tout afficher ou pas)
    Dim VpIsCreature As Boolean
    Dim VpSubType As String
    Dim VpRow As Integer
    Dim VpPrice As Single
    Dim VpCellVisual As VisualModels.Common
    Dim VpCellModel As DataModels.IDataModel
    Dim VpCellBehavior As New BehaviorModels.CustomEvents
        If MainForm.VgMe.IsMainReaderBusy Then
            Call ShowWarning(CgErr3)
        Else
            'Préparation des requêtes
            VpSQLGeneralCreatures = "(Select Card.EncNbr, Card.Title, Card.Series, Card.Price, Card.PriceDate, Card.Rarity, Card.CardText, Card.SubType, SubTypes.SubTypeVF, Creature.Tough, Creature.Power, Spell.Cost, Series.SeriesNM, Series.SeriesNM_FR, Card.FoilPrice, Card.FoilDate, Spell.Rulings, Series.Release, Card.Artist From ((((Card Inner Join Creature On Card.Title = Creature.Title) Inner Join Spell On Card.Title = Spell.Title) Inner Join Series On Card.Series = Series.SeriesCD) Left Join SubTypes On Card.SubType = SubTypes.SubTypeVO)" + If(Me.IsInAdvSearch And Not VpTransformed, " Inner Join " + VmAdvSearch + " On Card.EncNbr = " + mdlConstGlob.CgSFromSearch + ".EncNbr", "") + ") As T1"
            VpSQLGeneralAll = "(Select Card.EncNbr, Card.Title, Card.Series, Card.Price, Card.PriceDate, Card.Rarity, Card.CardText, Spell.Cost, Series.SeriesNM, Series.SeriesNM_FR, Card.FoilPrice, Card.FoilDate, Spell.Rulings, Series.Release, Card.Artist From ((Card Inner Join Spell On Card.Title = Spell.Title) Inner Join Series On Card.Series = Series.SeriesCD)" + If(Me.IsInAdvSearch And Not VpTransformed, " Inner Join " + VmAdvSearch + " On Card.EncNbr = " + mdlConstGlob.CgSFromSearch + ".EncNbr", "") + ") As T1"
            VpSQLStockInfosDecksFoil = "(Select GameID, EncNbr, Sum(IIf(Foil, Null, Items)) As MyItems, Sum(IIf(Foil, Items, Null)) As MyItemsFoil From MyGames Where Reserve = " + VpReserve.ToString + " And " + Me.Restriction + "True Group By GameID, EncNbr) As T2"
            VpSQLStockInfosCollecFoil = "(Select EncNbr, Sum(IIf(Foil, Null, Items)) As MyItems, Sum(IIf(Foil, Items, Null)) As MyItemsFoil From MyCollection Group By EncNbr) As T2"
            VpSQLColumnsRequired = "Select T1.Series, T1.Price, T1.PriceDate, T1.Rarity, T1.CardText, T1.Cost, T1.SeriesNM, T1.SeriesNM_FR, T1.FoilPrice, T1.FoilDate, T1.Rulings, T1.Release, T1.Artist"
            VpSQLTitleCriteria = " Where T1.Title = '" + VpCard.Replace("'", "''") + "'"
            VpSQLSorting = " Order By T1.Release"
            VpSQLJointure = If((Me.btShowAll.Enabled And Me.btShowAll.Checked) Or VpTransformed, " Left", " Inner")
            'Finalisation de la construction des requêtes ; 12 possibilités selon que :
            '- type de source :
            '   * recherche
            '   * decks
            '   * collection
            '- affichage ou non de toutes les éditions disponibles pour une carte, y compris celles hors stock
            '- carte créature ou non
            If Me.IsInAdvSearch Then
                VpSQLForCreatures = VpSQLColumnsRequired + ", T1.SubType, T1.SubTypeVF, T1.Tough, T1.Power From " + VpSQLGeneralCreatures + VpSQLTitleCriteria
                VpSQLForAll = VpSQLColumnsRequired + " From " + VpSQLGeneralAll + VpSQLTitleCriteria
            Else
                VpSQLForCreatures = VpSQLColumnsRequired + ", T1.SubType, T1.SubTypeVF, T1.Tough, T1.Power, T2.MyItems, T2.MyItemsFoil From " + VpSQLGeneralCreatures + VpSQLJointure + " Join " + If(VmDeckMode, VpSQLStockInfosDecksFoil, VpSQLStockInfosCollecFoil) +  " On T1.EncNbr = T2.EncNbr" + VpSQLTitleCriteria
                VpSQLForAll = VpSQLColumnsRequired + ", T2.MyItems, T2.MyItemsFoil From " + VpSQLGeneralAll + VpSQLJointure + " Join " + If(VmDeckMode, VpSQLStockInfosDecksFoil, VpSQLStockInfosCollecFoil) +  " On T1.EncNbr = T2.EncNbr" + VpSQLTitleCriteria
            End If
            VgDBCommand.CommandText = mdlToolbox.TrimQuery(VpSQLForCreatures, , VpSQLSorting)
            VgDBReader = VgDBCommand.ExecuteReader
            'S'il n'y a pas de réponse, c'est que ce n'est pas une créature, on supprime donc les champs force et endurance et on suppose que c'est un sort
            If Not VgDBReader.HasRows Then
                VpIsCreature = False
                VgDBReader.Close
                VgDBCommand.CommandText = mdlToolbox.TrimQuery(VpSQLForAll, , VpSQLSorting)
                VgDBReader = VgDBCommand.ExecuteReader
                'S'il n'y a encore pas de réponse, c'est que la carte a été supprimée dans l'intervalle
                If Not VgDBReader.HasRows Then
                    Call ShowWarning("Impossible d'afficher les détails de cette carte...")
                    VgDBReader.Close
                    Exit Sub
                End If
            Else
                VpIsCreature = True
            End If
            'Nettoyage grilles
            If Not VpSkipGrid Then
                Call Me.ClearGrid(Me.grdPropCard)
            End If
            Call Me.ClearGrid(Me.grdPropPicture)
            AddHandler VpCellBehavior.FocusEntered, AddressOf CellFocusEntered
            'Remplissage grilles
            With VgDBReader
                While .Read
                    If Not VpSkipGrid Then
                        'Insertion nouvelle ligne
                        VpRow = Me.grdPropCard.RowsCount
                        Me.grdPropCard.Rows.Insert(VpRow)
                        Me.grdPropPicture.Rows.Insert(VpRow)
                        'Colonne édition
                        Me.grdPropCard(VpRow, 0) = New Cells.Cell(.GetString(.GetOrdinal(If(Me.IsInVFMode, "SeriesNM_FR", "SeriesNM"))))
                        Me.grdPropCard(VpRow, 0).Tag = .GetString(.GetOrdinal("Series"))
                        Me.grdPropPicture(VpRow, 0) = New Cells.Cell(CDate(.GetValue(.GetOrdinal("Release"))).Year)
                        VpCellVisual = New VisualModels.Common
                        Try
                            VpCellVisual.Image = VgImgSeries.Images(VgImgSeries.Images.IndexOfKey("_e" + .GetString(.GetOrdinal("Series")) + CgIconsExt))
                        Catch
                            VpCellVisual.Image = Nothing
                        End Try
                        Me.grdPropCard(VpRow, 0).VisualModel = VpCellVisual
                        Me.grdPropCard(VpRow, 0).Behaviors.Add(VpCellBehavior)
                        Me.grdPropPicture(VpRow, 0).VisualModel = VpCellVisual
                        'Colonne illustrateur
                        Try
                            Me.grdPropPicture(VpRow, 1) = New Cells.Cell(.GetString(.GetOrdinal("Artist")))
                        Catch
                        End Try
                        'Colonne rareté
                        Me.grdPropCard(VpRow, 1) = New Cells.Cell(mdlToolbox.FormatTitle("Card.Rarity", .GetString(.GetOrdinal("Rarity"))))
                        'Colonne prix
                        VpPrice = mdlToolbox.SafeGetNonZeroVal("Price")
                        If VpPrice <> 0 Then
                            Me.grdPropCard(VpRow, 2) = New Cells.Cell(VpPrice)
                        End If
                        'Colonne prix foil
                        VpPrice = mdlToolbox.SafeGetNonZeroVal("FoilPrice")
                        If VpPrice <> 0 Then
                            Me.grdPropCard(VpRow, 3) = New Cells.Cell(VpPrice)
                        End If
                        'Colonnes stock et stock foil, qu'on ne gère que si on est pas en mode recherche avancée
                        If Not Me.IsInAdvSearch Then
                            Me.grdPropCard(VpRow, 4) = New Cells.Cell(CInt(mdlToolbox.SafeGetNonZeroVal("MyItems")))
                            Me.grdPropCard(VpRow, 5) = New Cells.Cell(CInt(mdlToolbox.SafeGetNonZeroVal("MyItemsFoil")))
                            VpCellModel = Utility.CreateDataModel(Type.GetType("System.Int32"))
                            VpCellModel.EditableMode = EditableMode.AnyKey Or EditableMode.SingleClick
                            AddHandler VpCellModel.Validated, AddressOf CellValidated
                            Me.grdPropCard(VpRow, 4).DataModel = VpCellModel
                            Me.grdPropCard(VpRow, 5).DataModel = VpCellModel
                        End If
                    End If
                    'Partie commune quelle que soit l'édition (à n'exécuter qu'une fois donc...)
                    If VpRow = 1 Or VpSkipGrid Then
                        'Sous-partie spécifique aux créatures
                        If VpIsCreature Then
                            Me.lblPowerTough.Text = .GetValue(.GetOrdinal("Power")).ToString + " / " + .GetValue(.GetOrdinal("Tough")).ToString
                            VpSubType = .GetValue(.GetOrdinal(If(Me.IsInVFMode, "SubTypeVF", "SubType"))).ToString
                            Me.splitV3.Panel2Collapsed = False
                        Else
                            Me.lblPowerTough.Text = ""
                            VpSubType = ""
                            Me.splitV3.Panel2Collapsed = True
                        End If
                        'Sous-partie commune à toutes les cartes
                        Call Me.BuildCost(.GetValue(.GetOrdinal("Cost")).ToString)
                        If Me.IsInVFMode Then
                            Call Me.PutInRichText(Me.txtRichCard, Me.imglstCarac, mdlToolbox.MyTxt(VpCard, True, VpDownFace), VpSubType)
                        Else
                            Call Me.PutInRichText(Me.txtRichCard, Me.imglstCarac, .GetValue(.GetOrdinal("CardText")).ToString, VpSubType)
                        End If
                        Me.txtRichOther.Clear
                        Me.txtRichOther.Text = .GetValue(.GetOrdinal("Rulings")).ToString
                        With Me.txtRichOther
                            If .Text.Trim <> "" Then
                                .Text = .Text.Substring(1).Replace("£", vbCrLf + vbCrLf)
                                .InsertTextAsRtf(mdlConstGlob.CgRulings + vbCrLf + vbCrLf, New Font(.Font, FontStyle.Bold))
                            End If
                        End With
                    End If
                    If VpSkipGrid Then Exit While
                End While
                .Close
            End With
            Me.grdPropCard.AutoSize
            Me.grdPropPicture.AutoSize
        End If
    End Sub
    Private Sub ReloadCarac
    '------------------------------------------------------------------------
    'Recharge les caractéristiques courantes si une carte était déjà affichée
    '------------------------------------------------------------------------
    Dim VpNode As TreeNode = Me.tvwExplore.SelectedNode
    Dim VpTitle As String
    Dim VpTransformed As Boolean
        If VpNode IsNot Nothing AndAlso VpNode.Parent IsNot Nothing Then
            If VpNode.Parent.Tag.Key = "Card.Title" Then
                VpTransformed = Me.IsTransformed(VpNode)
                VpTitle = If(VpTransformed, Me.picScanCard.Tag, VpNode.Tag.Value)
                If VpTitle <> "" Then
                    Call Me.LoadCarac(VpTitle, Me.IsDownFace(VpNode) Xor VpTransformed, VpTransformed, Me.IsReserveSelected)
                End If
            End If
        End If
    End Sub
    Private Sub BuildCost(VpCost As String)
    '--------------------------------------------------------------------
    'Affiche le coût d'invocation du sort sélectioné de manière graphique
    '--------------------------------------------------------------------
    Dim VpPictureBox As PictureBox              'Objet image icône courante
    Dim VpOffset As Integer                     'Décalage courant pour présenter à l'endroit le coût d'invocation
    Dim VpInvocListe As New List(Of clsManas)   'Liste d'instance classe formatée correspondant au coût passé en paramètre
    Dim VpCosts() As String
        Call Me.DeBuildCost
        If VpCost.Trim <> "" Then
            If Not VpCost.Contains("//") Then
                VpInvocListe.Add(New clsManas(VpCost))
            'Cas des multi-cartes
            Else
                VpCosts = VpCost.Split(New String() {"//"}, StringSplitOptions.None)
                For VpI As Integer = 0 To VpCosts.Length - 1
                    VpInvocListe.Add(New clsManas(VpCosts(VpI).Trim))
                Next VpI
            End If
            VpOffset = 0
            For Each VpInvoc As clsManas In VpInvocListe
                For Each VpImg As Integer In VpInvoc.ImgIndexes
                    VpPictureBox = New PictureBox
                    Me.pnlCard2.Controls.Add(VpPictureBox)
                    With VpPictureBox
                        .Size = New Size(18, 18)
                        .Top = Me.picCost.Top + (Me.picCost.Height - .Height) / 2
                        .Left = 20 + Me.picCost.Left + Me.picCost.Width + .Width * VpOffset
                        .Image = Me.imglstCarac.Images(VpImg)
                    End With
                    VpOffset = VpOffset + 1
                Next VpImg
                VpOffset = VpOffset + 1
            Next VpInvoc
        End If
    End Sub
    Private Sub DeBuildCost
    '------------------------------------------------
    'Efface le coût d'invocation précédemment affiché
    '------------------------------------------------
    Dim VpPictureBox As PictureBox
    Dim VpToRemove As New List(Of Control)
        For Each VpControl As Control In Me.pnlCard2.Controls
            Try
                VpPictureBox = CType(VpControl, PictureBox)
                If VpPictureBox IsNot Me.picCost Then
                    VpToRemove.Add(VpControl)
                End If
            Catch
            End Try
        Next VpControl
        For Each VpControl As Control In VpToRemove
            Me.pnlCard2.Controls.Remove(VpControl)
        Next VpControl
    End Sub
    Private Sub PutInRichText(VpRich As ucExRichTextBox, VpImg As ImageList, VpTxt As String, VpSubType As String)
    '--------------------------------------------------------------------------------------------------
    'Inscrit en RTF (avec images) le texte passé en paramètre dans la zone de texte passée en paramètre
    '--------------------------------------------------------------------------------------------------
    Dim VpStr As String = VpTxt.Replace("{", "!").Replace("}", "!")
    Dim VpSymbole As String
    Dim VpImgIndex As Integer
    Dim VpImg18 As Image
    Dim VpImg12 As Bitmap
        VpRich.Clear
        'Gestion des sous-types
        If VpSubType <> "" Then
            VpRich.AppendTextAsRtf(VpSubType + vbCrLf + vbCrLf, New Font(VpRich.Font, FontStyle.Bold))
        End If
        'Gestion des symboles non usuels
        VpStr = VpStr.Replace("[chaos]", "!o!")
        VpStr = VpStr.Replace("[plane]", "!p!")
        'Gestion de tous les symboles
        While VpStr.IndexOf("!") <> VpStr.LastIndexOf("!")  'tant qu'il reste 2 '!', il reste un symbole à convertir
            VpRich.AppendText(VpStr.Substring(0, VpStr.IndexOf("!")))
            VpStr = VpStr.Substring(VpStr.IndexOf("!") + 1)
            VpSymbole = VpStr.Substring(0, VpStr.IndexOf("!"))
            If IsNumeric(VpSymbole) Then
                VpImgIndex = 1 + CInt(VpSymbole)
            Else
                Select Case VpSymbole.Replace("/", "").Replace("(", "").Replace(")", "").ToLower
                    Case "pr", "rp"
                        VpImgIndex = 35
                    Case "pb", "bp"
                        VpImgIndex = 33
                    Case "pg", "gp"
                        VpImgIndex = 34
                    Case "pu", "up"
                        VpImgIndex = 36
                    Case "pw", "wp"
                        VpImgIndex = 37
                    Case "gb"
                        VpImgIndex = 29
                    Case "rb"
                        VpImgIndex = 39
                    Case "ug"
                        VpImgIndex = 45
                    Case "wg"
                        VpImgIndex = 50
                    Case "gr"
                        VpImgIndex = 30
                    Case "wr"
                        VpImgIndex = 51
                    Case "bu"
                        VpImgIndex = 26
                    Case "ru"
                        VpImgIndex = 41
                    Case "bw"
                        VpImgIndex = 27
                    Case "uw"
                        VpImgIndex = 47
                    Case "bg"
                        VpImgIndex = 24
                    Case "br"
                        VpImgIndex = 25
                    Case "gu"
                        VpImgIndex = 31
                    Case "gw"
                        VpImgIndex = 32
                    Case "rg"
                        VpImgIndex = 40
                    Case "rw"
                        VpImgIndex = 42
                    Case "ub"
                        VpImgIndex = 44
                    Case "ur"
                        VpImgIndex = 46
                    Case "wb"
                        VpImgIndex = 49
                    Case "wu"
                        VpImgIndex = 52
                    Case "2w", "w2"
                        VpImgIndex = 22
                    Case "2b", "b2"
                        VpImgIndex = 18
                    Case "2g", "g2"
                        VpImgIndex = 19
                    Case "2r", "r2"
                        VpImgIndex = 20
                    Case "2u", "u2"
                        VpImgIndex = 21
                    Case "t"
                        VpImgIndex = 54
                    Case "q"
                        VpImgIndex = 53
                    Case "x"
                        VpImgIndex = 0
                    Case "b"
                        VpImgIndex = 23
                    Case "g"
                        VpImgIndex = 28
                    Case "r"
                        VpImgIndex = 38
                    Case "u"
                        VpImgIndex = 43
                    Case "w"
                        VpImgIndex = 48
                    Case "m"
                        VpImgIndex = 1
                    Case "a"
                        VpImgIndex = 1
                    Case "s"
                        VpImgIndex = 55
                    Case "c"
                        VpImgIndex = 56
                    Case "e"
                        VpImgIndex = 57
                    Case "o"
                        VpImgIndex = 58
                    Case "p"
                        VpImgIndex = 59
                    Case Else
                        VpImgIndex = -1
                End Select
            End If
            If VpImgIndex <> -1 Then
                VpImg18 = VpImg.Images.Item(VpImgIndex)
                VpImg12 = New Bitmap(12, 12)
                Using VpGraphics As Graphics = Graphics.FromImage(VpImg12)
                    VpGraphics.DrawImage(VpImg18, New Rectangle(0, 0, VpImg12.Width , VpImg12.Height), New Rectangle(0, 0, VpImg18.Width, VpImg18.Height), GraphicsUnit.Pixel)
                End Using
                VpRich.InsertImage(If(VpRich.Font.Size < 13, VpImg12, VpImg18))
            End If
            VpStr = VpStr.Substring(VpStr.IndexOf("!") + 1)
        End While
        VpRich.AppendText(VpStr)
        'Gestion des descriptions en italique
        If VpStr.Contains(" &lt;i&gt;") Then
            VpRich.SelectionStart = VpRich.Text.IndexOf(" &lt;i&gt;")
            VpRich.SelectionLength = VpRich.Text.IndexOf(" &lt;/i&gt;") - VpRich.SelectionStart + 11
            VpRich.SelectionFont = New Font(VpRich.Font, FontStyle.Italic)
            VpRich.SelectedRtf = VpRich.SelectedRtf.Replace(" &lt;i&gt;", "").Replace(" &lt;/i&gt;", "")
            VpRich.DeselectAll
        End If
    End Sub
    Private Sub LoadAutorisations(VpCard As String)
    '-----------------------------------------------------------------------
    'Affiche les autorisations de tournois pour la carte passée en paramètre
    '-----------------------------------------------------------------------
        If VpCard = "" Or Me.IsMainReaderBusy Then
            'Autorisations vierges
            Me.picAutT1.Image = Me.imglstAutorisations.Images.Item(11)
            Me.picAutT15.Image = Me.imglstAutorisations.Images.Item(18)
            Me.picAutT2.Image = Me.imglstAutorisations.Images.Item(15)
            Me.picAutM.Image = Me.imglstAutorisations.Images.Item(21)
            Me.picAutBloc.Image = Me.imglstAutorisations.Images.Item(5)
            Me.picAut1V1.Image = Me.imglstAutorisations.Images.Item(2)
            Me.picAutMulti.Image = Me.imglstAutorisations.Images.Item(8)
            Me.picAutMTGO.Image = Me.imglstAutorisations.Images.Item(24)
        Else
            VgDBCommand.CommandText = "Select T1, T1r, T15, T2, M, Bloc, Blocoff, [1V1], Multi, MTGO, MTGOoff From Autorisations Where Title = '" + VpCard.Replace("'", "''") + "';"
            VgDBReader = VgDBCommand.ExecuteReader
            With VgDBReader
                If .Read Then
                    'Autorisations T1 (Vintage)
                    If .GetBoolean(1) Then
                        Me.picAutT1.Image = Me.imglstAutorisations.Images.Item(12)      'Restriction à 1 item
                    ElseIf .GetBoolean(0) Then
                        Me.picAutT1.Image = Me.imglstAutorisations.Images.Item(9)
                    Else
                        Me.picAutT1.Image = Me.imglstAutorisations.Images.Item(10)
                    End If
                    'Autorisations T1.5 (Legacy)
                    If .GetBoolean(2) Then
                        Me.picAutT15.Image = Me.imglstAutorisations.Images.Item(16)
                    Else
                        Me.picAutT15.Image = Me.imglstAutorisations.Images.Item(17)
                    End If
                    'Autorisations T2 (Standard)
                    If .GetBoolean(3) Then
                        Me.picAutT2.Image = Me.imglstAutorisations.Images.Item(13)
                    Else
                        Me.picAutT2.Image = Me.imglstAutorisations.Images.Item(14)
                    End If
                    'Autorisations M (Modern)
                    If .GetBoolean(4) Then
                        Me.picAutM.Image = Me.imglstAutorisations.Images.Item(19)
                    Else
                        Me.picAutM.Image = Me.imglstAutorisations.Images.Item(20)
                    End If
                    'Autorisations Bloc
                    If .GetBoolean(6) Then
                        Me.picAutBloc.Image = Me.imglstAutorisations.Images.Item(5)
                    ElseIf .GetBoolean(5) Then
                        Me.picAutBloc.Image = Me.imglstAutorisations.Images.Item(3)
                    Else
                        Me.picAutBloc.Image = Me.imglstAutorisations.Images.Item(4)
                    End If
                    'Autorisations 1 vs 1
                    If .GetBoolean(7) Then
                        Me.picAut1V1.Image = Me.imglstAutorisations.Images.Item(0)
                    Else
                        Me.picAut1V1.Image = Me.imglstAutorisations.Images.Item(1)
                    End If
                    'Autorisations Multi
                    If .GetBoolean(8) Then
                        Me.picAutMulti.Image = Me.imglstAutorisations.Images.Item(6)
                    Else
                        Me.picAutMulti.Image = Me.imglstAutorisations.Images.Item(7)
                    End If
                    'Autorisations MTGO
                    If .GetBoolean(10) Then
                        Me.picAutMTGO.Image = Me.imglstAutorisations.Images.Item(24)
                    ElseIf .GetBoolean(9) Then
                        Me.picAutMTGO.Image = Me.imglstAutorisations.Images.Item(22)
                    Else
                        Me.picAutMTGO.Image = Me.imglstAutorisations.Images.Item(23)
                    End If
                Else
                    Call Me.LoadAutorisations("")
                End If
                .Close
            End With
        End If
    End Sub
    Private Sub LoadPricesHistory(VpFoil As Boolean)
    Dim VpPricesHistory As frmGrapher
    Dim VpCardName As String = Me.tvwExplore.SelectedNode.Tag.Value
    Dim VpHist As Hashtable
        Application.DoEvents
        If mdlToolbox.DBOK Then
            If mdlToolbox.HasPriceHistory Then
                If VmMyChildren.DoesntExist(VmMyChildren.PricesHistory) Then
                    VpPricesHistory = New frmGrapher
                    VmMyChildren.PricesHistory = VpPricesHistory
                Else
                    VpPricesHistory = VmMyChildren.PricesHistory
                End If
                VpHist = mdlToolbox.GetPriceHistory(VpCardName, VpFoil)
                For Each VpEdition As String In VpHist.Keys
                    VpPricesHistory.AddNewPlot(VpHist.Item(VpEdition), mdlToolbox.GetSerieNameFromCode(VpEdition, Me.IsInVFMode))
                Next VpEdition
                VpPricesHistory.Show
                VpPricesHistory.BringToFront
            Else
                Call mdlToolbox.ShowWarning(mdlConstGlob.CgErr2)
            End If
        End If
    End Sub
    Private Function IsReserveSelected As Boolean
    '---------------------------------------------------------------------
    'Regarde si la carte sélectionnée dans un deck appartient à la réserve
    '---------------------------------------------------------------------
        Return ( Me.MyRoot.Text = mdlConstGlob.CgSide )
    End Function
    Private Function IsTransformed(VpNode As TreeNode) As Boolean
    '-----------------------------------
    'Renvoie si la carte est transformée
    '-----------------------------------
        Return Not ( VpNode.Text = VpNode.Tag.Value Or VpNode.Text = VpNode.Tag.Value2 )
    End Function
    Private Function IsDownFace(VpNode As TreeNode) As Boolean
    '--------------------------------------
    'Renvoie si la carte est face retournée
    '--------------------------------------
        If MainForm.VgMe.IsMainReaderBusy Then
            Call ShowWarning(CgErr3)
            Return False
        Else
            VgDBCommand.CommandText = "Select Count(*) From CardDouble Inner Join Card On CardDouble.EncNbrDownFace = Card.EncNbr Where Card.Title = '" + VpNode.Tag.Value.Replace("'", "''") + "';"
            Return ( CInt(VgDBCommand.ExecuteScalar) > 0 )
        End If
    End Function
    Private Function GetTransformedTag(VpTitle As String, VpReverse As Boolean, VpDownFace As Boolean, VpOnlyVO As Boolean) As clsTag
    '----------------------------------------------------------
    'Retourne les noms (VO/VF) de la carte transformée associée
    '----------------------------------------------------------
    Dim VpEncNbr As Long
        If VpDownFace Then
            If Not VpReverse Then
                VgDBCommand.CommandText = "Select CardDouble.EncNbrDownFace From Card Inner Join CardDouble On Card.EncNbr = CardDouble.EncNbrDownFace Where Card.Title = '" + VpTitle.Replace("'", "''") + "';"
            Else
                VgDBCommand.CommandText = "Select CardDouble.EncNbrTopFace From Card Inner Join CardDouble On Card.EncNbr = CardDouble.EncNbrDownFace Where Card.Title = '" + VpTitle.Replace("'", "''") + "';"
            End If
        Else
            If VpReverse Then
                VgDBCommand.CommandText = "Select CardDouble.EncNbrTopFace From Card Inner Join CardDouble On Card.EncNbr = CardDouble.EncNbrTopFace Where Card.Title = '" + VpTitle.Replace("'", "''") + "';"
            Else
                VgDBCommand.CommandText = "Select CardDouble.EncNbrDownFace From Card Inner Join CardDouble On Card.EncNbr = CardDouble.EncNbrTopFace Where Card.Title = '" + VpTitle.Replace("'", "''") + "';"
            End If
        End If
        VpEncNbr = VgDBCommand.ExecuteScalar
        Return Me.GetTag(VpEncNbr, VpOnlyVO)
    End Function
    Private Function GetTag(VpEncNbr As Long, VpOnlyVO As Boolean) As clsTag
    Dim VpNames As New clsTag
        If VpEncNbr <> 0 Then
            VgDBCommand.CommandText = "Select Card.Title From Card Where Card.EncNbr = " + VpEncNbr.ToString + ";"
            VpNames.Value = VgDBCommand.ExecuteScalar.ToString
            VgDBCommand.CommandText = "Select CardFR.TitleFR From CardFR Where CardFR.EncNbr = " + VpEncNbr.ToString + ";"
            VpNames.Value2 = If(VpOnlyVO, VpNames.Value, VgDBCommand.ExecuteScalar.ToString)
            VgDBCommand.CommandText = "Select Card.MultiverseId From Card Where Card.EncNbr = " + VpEncNbr.ToString + ";"
            VpNames.MultiverseId = CLng(VgDBCommand.ExecuteScalar)
        End If
        Return VpNames
    End Function
    Public Function GetSelectedSource(VpToolStripMenuItem As ToolStripMenuItem) As String
    '--------------------------------------
    'Retourne le nom de source sélectionnée
    '--------------------------------------
    Dim VpStr As String
        For Each VpItem As ToolStripItem In VpToolStripMenuItem.DropDownItems
            If TypeOf VpItem Is ToolStripMenuItem Then
                If CType(VpItem, ToolStripMenuItem).Checked Then
                    Return VpItem.Text
                Else
                    VpStr = Me.GetSelectedSource(VpItem)
                    If VpStr <> "" Then
                        Return VpStr
                    End If
                End If
            End If
        Next VpItem
        Return ""
    End Function
    Public Function GetSelectedSource() As String
        Return Me.GetSelectedSource(Me.mnuDisp)
    End Function
    Public Function IsSourcePresent As Boolean
    '--------------------------------------------------------------
    'Vérifie qu'il y a bien au moins une source de cartes à traiter
    '--------------------------------------------------------------
        Return ( Me.GetSelectedSource <> "" )
    End Function
    Private Function GetAllSources As List(Of String)
    '---------------------------------------
    'Retourne toutes les sources disponibles
    '---------------------------------------
    Dim VpSources As New List(Of String)
        VgDBCommand.CommandText = "Select GameName From MyGamesID Where IsFolder = False;"
        VgDBReader = VgDBCommand.ExecuteReader
        With VgDBReader
            While .Read
                VpSources.Add(.GetString(0))
            End While
            .Close
        End With
        Return VpSources
    End Function
    Private Sub ChangeLanguage(VpNode As TreeNode, VpSeriesAldreadyDone As Boolean)
    '-----------------------------------------------------
    'Commute la langue du titre des cartes et des éditions
    '-----------------------------------------------------
    Dim VpChild As TreeNode
        '1. Gestion titres des éditions
        If Not VpSeriesAldreadyDone Then
            If VpNode.Parent IsNot Nothing AndAlso VpNode.Parent.Tag.Key = "Card.Series" Then
                VpNode.Text = mdlToolbox.FormatTitle("Card.Series", mdlToolbox.GetSerieCodeFromName(VpNode.Text, , Not Me.IsInVFMode), Me.IsInVFMode)
            End If
        End If
        '2. Gestion titres des cartes
        If VpNode.Tag.Key = "Card.Title" Then
            If Me.IsInVFMode Then
                For Each VpChild In VpNode.Nodes
                    VpChild.Text = VpChild.Tag.Value2
                Next VpChild
            Else
                For Each VpChild In VpNode.Nodes
                    VpChild.Text = VpChild.Tag.Value
                Next VpChild
            End If
        Else
            For Each VpChild In VpNode.Nodes
                Call Me.ChangeLanguage(VpChild, VpSeriesAldreadyDone)
            Next VpChild
        End If
    End Sub
    Private Sub RecurChangeLanguage(VpSeriesAldreadyDone As Boolean)
        For Each VpNode As TreeNode In Me.tvwExplore.Nodes
            Call Me.ChangeLanguage(VpNode, VpSeriesAldreadyDone)
        Next VpNode
    End Sub
    Private Function FindImageIndex(VpTag As String, VpStr As String) As Integer
    '-------------------------------------------------------------------------
    'Retourne les numéros d'icônes à utiliser comme symboles dans le treeview)
    '-------------------------------------------------------------------------
        Select Case VpTag
            Case "Card.myPrice"
                Return (34 + CInt(VpStr))
            Case "Card.Series"
                Return Me.imglstTvw.Images.IndexOfKey("_e" + VpStr + CgIconsExt)
            Case "Spell.Color", "Card.Title"
                Return FindImageIndexColor(VpStr)
            Case "Spell.myCost"
                If VpStr.Trim <> "" Then
                    Return (13 + CInt(VpStr))
                Else
                    Return 12
                End If
            Case "Card.Rarity"
                Return FindImageIndexRarity(VpStr)
            Case "Card.Type"
                Return FindImageIndexType(VpStr)
            Case Else
                Return 0
        End Select
    End Function
    Private Function FindImageIndexColor(VpColor As String) As Integer
        Select Case VpColor
            Case "A"
                Return 4
            Case "B"
                Return 5
            Case "G"
                Return 7
            Case "L"
                Return 8
            Case "M"
                Return 9
            Case "R"
                Return 10
            Case "U"
                Return 6
            Case "W"
                Return 3
            Case "T", "K"
                Return 11
            Case Else
                Return 0
        End Select
    End Function
    Private Function FindImageIndexRarity(VpRarity As String) As Integer
        Select Case VpRarity.Substring(0, 1).ToUpper
            Case "D", "L", "S"
                Return 30
            Case "C"
                Return 31
            Case "U"
                Return 32
            Case "R"
                Return 33
            Case "M"
                Return 34
            Case Else
                Return 0
        End Select
    End Function
    Private Function FindImageIndexType(VpType As String) As Integer
        Select Case VpType.Substring(0, 1).ToUpper
            Case "C", "U"
                Return 45
            Case "I", "N"
                Return 46
            Case "A"
                Return 43
            Case "E", "T"
                Return 44
            Case "L"
                Return 47
            Case "S"
                Return 48
            Case "H"
                Return 49
            Case "Q"
                Return 50
            Case "Y", "Z"
                Return 51
            Case "P"
                Return 52
            Case Else
                Return 0
        End Select
    End Function
    Private Function ManageTransfert(VpNode As TreeNode, VpTransfertType As clsTransferResult.EgTransfertType, Optional VpTo As String = "") As Boolean
    '---------------------------------------------------------------------------------------------------------------------------------------------
    'Gère la suppression d'une carte ou son transfert dans un autre deck/collection, ou encore son simple ajout, ou enfin son changement d'édition
    '---------------------------------------------------------------------------------------------------------------------------------------------
    Dim VpPreciseTransfert As frmTransfer
    Dim VpTransfertResult As New clsTransferResult
    Dim VpCardName As String = VpNode.Tag.Value
    Dim VpSource As String
    Dim VpSource2 As String
    Dim VpFoil As Boolean
        'Source
        If Me.IsInAdvSearch Then
            VpSource = mdlConstGlob.CgSFromSearch
            VpSource2 = VmAdvSearch
        ElseIf VmDeckMode Then
            VpSource = mdlConstGlob.CgSDecks
            VpSource2 = mdlConstGlob.CgSDecks
        Else
            VpSource = mdlConstGlob.CgSCollection
            VpSource2 = mdlConstGlob.CgSCollection
        End If
        With VpTransfertResult
            'Type d'opération
            .TransfertType = VpTransfertType
            'Présence réserve éventuelle
            .ReserveFrom = Me.IsReserveSelected
            'Gestion des cas multiples / foils
            If frmTransfer.NeedsPrecision(Me, VpCardName, VpSource, VpSource2, VpTransfertType, VpFoil) Then
                VpPreciseTransfert = New frmTransfer(Me, VpCardName, VpSource, VpSource2, VpTransfertResult)
                VpPreciseTransfert.ShowDialog
            Else
                .NCartes = 1
                .IDSerieFrom = frmTransfer.GetMatchingEdition(Me, VpCardName, VpSource, VpSource2)
                .IDSerieTo = .IDSerieFrom
                .FoilFrom = VpFoil
                .FoilTo = VpFoil
            End If
            'Si pas d'annulation utilisateur
            If .NCartes <> 0 Then
                'Récupération du numéro encyclopédique de la carte concernée
                .EncNbrFrom = mdlToolbox.GetEncNbr(VpCardName, .IDSerieFrom)
                .EncNbrTo = mdlToolbox.GetEncNbr(VpCardName, .IDSerieTo)
                'Lieux des modifications
                .TFrom = Me.GetSelectedSource
                .SFrom = VpSource
                If .TransfertType = clsTransferResult.EgTransfertType.Swap Then
                    .TTo = .TFrom
                    .STo = .SFrom
                ElseIf .TransfertType = clsTransferResult.EgTransfertType.Move Or .TransfertType = clsTransferResult.EgTransfertType.Copy Then
                    .TTo = VpTo
                    .STo = If(VpTo = mdlConstGlob.CgCollection, mdlConstGlob.CgSCollection, mdlConstGlob.CgSDecks)
                End If
                'Opération effective
                If .TFrom <> .TTo And .TransfertType = clsTransferResult.EgTransfertType.Copy Then
                    Call frmTransfer.CommitAction(VpTransfertResult)
                    Return Me.IsInAdvSearch AndAlso Me.IsInRestrictedAdvSearch
                ElseIf (.TFrom <> .TTo Or (.TFrom = .TTo And .TransfertType = clsTransferResult.EgTransfertType.Copy)) Or (.TransfertType = clsTransferResult.EgTransfertType.Swap And (.EncNbrFrom <> .EncNbrTo Or .ReserveFrom <> .ReserveTo Or .FoilFrom <> .FoilTo)) Then
                    Call frmTransfer.CommitAction(VpTransfertResult)
                    Return True
                Else
                    Call mdlToolbox.ShowWarning("La source et la destination sont identiques !")
                    Return False
                End If
            End If
        End With
    End Function
    Private Sub ManageMultipleTransferts(VpTransfertType As clsTransferResult.EgTransfertType, Optional VpTo As String = "")
    '-------------------------------------------------------------------------------------------------------------
    'Gère les transferts / suppression en ajoutant le cas où plusieurs éléments sont sélectionnés dans le treeview
    '-------------------------------------------------------------------------------------------------------------
    Dim VpMustReload As Boolean = False
    Dim VpNode As TreeNode = Me.tvwExplore.SelectedNode
    Dim VpTag As clsTag
        If Not VpNode Is Nothing Then
            VpTag = VpNode.Tag
            If VpTag.Descendance = "" Then
                For Each VpItem As TreeNode In Me.tvwExplore.SelectedNodes
                    VpMustReload = VpMustReload Or Me.ManageTransfert(VpItem, VpTransfertType, VpTo)    'Si au moins un des cas multiples ne s'est pas terminé par une annulation, c'est qu'il faut recharger le treeview
                Next VpItem
                If VpMustReload Then
                    Call Me.ReloadWithHistory
                End If
            Else
                Call Me.ManageDescendanceTransferts(VpTransfertType, VpTo)
            End If
        End If
    End Sub
    Private Sub ManageDescendanceTransferts(VpTransfertType As clsTransferResult.EgTransfertType, Optional VpTo As String = "")
    '------------------------------------------------------------------------------------------------------------------------------
    'Gère les transferts / suppression dans le cas où l'utilisateur n'a pas choisi des cartes mais un niveau hiérarchique supérieur
    '------------------------------------------------------------------------------------------------------------------------------
    Dim VpNode As TreeNode = Me.tvwExplore.SelectedNode
    Dim VpTag As clsTag = VpNode.Tag
    Dim VpSQL As String = VpTag.Descendance
    Dim VpSource As String = Me.MySource
    Dim VpTransfertResult As clsTransferResult
    Dim VpDBCommand As New OleDbCommand
    Dim VpDBReader As OleDbDataReader
    Dim VpMustReload As Boolean = False
    Dim VpQuant As Integer
        'La requête permettant de construire les fils du noeud sélectionné a été mémorisée dans un tag => on en récupère la clause From
        VpSQL = VpSQL.Substring(VpSQL.IndexOf("From"))
        If VpSQL.Contains("Group By") Then
            VpSQL = VpSQL.Substring(0, VpSQL.LastIndexOf("Group By") - 1)
        End If
        If Not Me.IsInAdvSearch Then
            VpSQL = "Select " + VpSource + ".EncNbr, Items, Foil " + VpSQL
        Else
            'Gestion particulière lorsque l'on est en mode recherche avancée
            VpSQL = "Select Card.EncNbr " + VpSQL
            If VgOptions.VgSettings.CopyRange > 1 Then
                VpQuant = Math.Min(VgOptions.VgSettings.CopyRange, Val(InputBox("En combien d'exemplaire(s) souhaitez-vous copier ces cartes ?" + vbCrLf + "(" + VgOptions.VgSettings.CopyRange.ToString + " max.)", "Nombre d'éléments", "1")))
            Else
                VpQuant = 1
            End If
        End If
        'Rejoue cette requête
        VpDBCommand.Connection = VgDB
        VpDBCommand.CommandType = CommandType.Text
        VpDBCommand.CommandText = VpSQL
        VpDBReader = VpDBcommand.ExecuteReader
        While VpDBReader.Read
            'On crée un nouvel objet de transfert pour chaque ligne retournée
            VpTransfertResult = New clsTransferResult
            With VpTransfertResult
                .TransfertType = VpTransfertType
                .EncNbrFrom = VpDBReader.GetInt32(0)
                .EncNbrTo = .EncNbrFrom
                .FoilFrom = If(Me.IsInAdvSearch, False, VpDBReader.GetBoolean(2))
                .FoilTo = .FoilFrom
                .ReserveFrom = Me.IsReserveSelected
                .ReserveTo = .ReserveFrom
                .NCartes = If(Me.IsInAdvSearch, VpQuant, VpDBReader.GetInt32(1))
                .TFrom = Me.GetSelectedSource
                .SFrom = VpSource
                If .TransfertType = clsTransferResult.EgTransfertType.Move Or .TransfertType = clsTransferResult.EgTransfertType.Copy Then
                    .TTo = VpTo
                    .STo = If(VpTo = mdlConstGlob.CgCollection, mdlConstGlob.CgSCollection, mdlConstGlob.CgSDecks)
                End If
                'Opération effective
                If .NCartes > 0 Then
                    If .TFrom <> .TTo And .TransfertType = clsTransferResult.EgTransfertType.Copy Then
                        Call frmTransfer.CommitAction(VpTransfertResult)
                    ElseIf (.TFrom <> .TTo Or (.TFrom = .TTo And .TransfertType = clsTransferResult.EgTransfertType.Copy)) Or (.TransfertType = clsTransferResult.EgTransfertType.Swap And (.EncNbrFrom <> .EncNbrTo Or .ReserveFrom <> .ReserveTo Or .FoilFrom <> .FoilTo)) Then
                        Call frmTransfer.CommitAction(VpTransfertResult)
                        VpMustReload = True
                    Else
                        Call mdlToolbox.ShowWarning("La source et la destination sont identiques !")
                        Exit While
                    End If
                End If
            End With
        End While
        VpDBReader.Close
        'Actualisation de l'arborescence si nécessaire
        If VpMustReload Then
            Call Me.ReloadWithHistory
        End If
    End Sub
    Private Sub OnlineBuy(VpLoad As Boolean, VpFullLoad As Boolean)
    '----------------------------------------------
    'Chargement du formulaire d'achats sur Internet
    '----------------------------------------------
    Dim VpBuyer As frmBuyCards
    Dim VpSource As String = Me.MySource
    Dim VpSQL As String
        If VmMyChildren.DoesntExist(VmMyChildren.CardsBuyer) Then
            VpBuyer = New frmBuyCards(VgOptions.VgSettings.MarketServerEnum)
            VmMyChildren.CardsBuyer = VpBuyer
        Else
            VpBuyer = VmMyChildren.CardsBuyer
        End If
        VpBuyer.Show
        VpBuyer.BringToFront
        If VpFullLoad Then
            VpSQL = "Select Title, Items From " + VpSource + " Inner Join Card On Card.EncNbr = " + VpSource + ".EncNbr Where Reserve = " + Me.IsReserveSelected.ToString + " And "
            VpSQL = VpSQL + Me.Restriction
            VgDBCommand.CommandText = mdlToolbox.TrimQuery(VpSQL)
            VgDBReader = VgDBCommand.ExecuteReader
            With VgDBReader
                While .Read
                    VpBuyer.AddToBasket(.GetString(0), .GetInt32(1))
                End While
                .Close
            End With
            Call VpBuyer.LoadGrid(mdlConstGlob.eBasketMode.Local)
        ElseIf VpLoad Then
            For Each VpNode As TreeNode In Me.tvwExplore.SelectedNodes
                Call VpBuyer.AddToBasket(VpNode.Tag.Value)
            Next VpNode
            Call VpBuyer.LoadGrid(mdlConstGlob.eBasketMode.Local)
        End If
    End Sub
    #End Region
    #Region "Propriétés"
    Protected Overrides ReadOnly Property CreateParams As CreateParams
        Get
        Dim VpCP As CreateParams = MyBase.CreateParams
            If Environment.OSVersion.Version.Major >= 6 Then
                VpCP.ExStyle = VpCP.ExStyle Or &H2000000
            End If
            Return VpCP
        End Get
    End Property
    Public WriteOnly Property Suggestions As List(Of clsCorrelation)
        Set (VpSuggestions As List(Of clsCorrelation))
            VmSuggestions = VpSuggestions
        End Set
    End Property
    Public Property IsDownloadInProgress As Boolean
        Get
            Return VmDownloadInProgress
        End Get
        Set (VpDownloadInProgress As Boolean)
            VmDownloadInProgress = VpDownloadInProgress
        End Set
    End Property
    Public Property IsInImgDL As Boolean
        Get
            Return VmImgDL
        End Get
        Set (VpImgDL As Boolean)
            VmImgDL = VpImgDL
        End Set
    End Property
    Public Property IsMainReaderBusy As Boolean
        Get
            Return VmMainReaderBusy
        End Get
        Set (VpMainReaderBusy As Boolean)
            VmMainReaderBusy = VpMainReaderBusy
        End Set
    End Property
    Public ReadOnly Property IsInAdvSearch As Boolean
    '--------------------------------------------------------------------------------
    'Retourne si l'on est actuellement en affichage de résultats de recherche avancée
    '--------------------------------------------------------------------------------
        Get
            Return Me.mnuDispAdvSearch.Checked
        End Get
    End Property
    Public ReadOnly Property IsInVFMode As Boolean
        Get
            Return Me.mnuCardsFR.Checked
        End Get
    End Property
    Public ReadOnly Property IsInRestrictedAdvSearch As Boolean
        Get
            Return VmAdvSearch.Contains("Not In (Select")   'crade, mais permet de savoir si la requête comprend un critère sur la non-possession
        End Get
    End Property
    Public ReadOnly Property MySource As String
    '----------------------------------------------------------------------------------------
    'Retourne le nom de la table appropriée suivant que l'on affiche la collection ou un deck
    '----------------------------------------------------------------------------------------
        Get
            Return If(VmDeckMode, mdlConstGlob.CgSDecks, mdlConstGlob.CgSCollection)
        End Get
    End Property
    Public ReadOnly Property IsInDeckMode As Boolean
        Get
            Return VmDeckMode
        End Get
    End Property
    Public Property MyChildren As clsChildren
    '----------------------
    'Collection des enfants
    '----------------------
        Get
            Return VmMyChildren
        End Get
        Set (VpMyChildren As clsChildren)
            VmMyChildren = VpMyChildren
        End Set
    End Property
    Public ReadOnly Property FilterCriteria As frmExploSettings
        Get
            Return VmFilterCriteria
        End Get
    End Property
    Private ReadOnly Property FirstRoot As TreeNode
        Get
            Return Me.tvwExplore.Nodes.Item(0)
        End Get
    End Property
    Public ReadOnly Property LastRoot As TreeNode
        Get
        Dim VpNode As TreeNode = Me.tvwExplore.Nodes.Item(0)
            While Not VpNode.NextNode Is Nothing
                VpNode = VpNode.NextNode
            End While
            Return VpNode
        End Get
    End Property
    Private ReadOnly Property MyRoot As TreeNode
        Get
        Dim VpNode As TreeNode = Me.tvwExplore.SelectedNode
            If VpNode.Parent IsNot Nothing Then
                Do
                    VpNode = VpNode.Parent
                Loop Until VpNode.Parent Is Nothing
            End If
            Return VpNode
        End Get
    End Property
    Private ReadOnly Property CurrentCardTitle As String
        Get
        Dim VpNode As TreeNode = Me.tvwExplore.SelectedNode
            Return If(Me.IsTransformed(VpNode), Me.picScanCard.Tag, VpNode.Tag.Value)
        End Get
    End Property
    #End Region
    #Region "Evènements"
    Sub MnuExitActivate(ByVal sender As Object, ByVal e As EventArgs)
    Dim VpDefaultActivatedCriteria As String = ""
    Dim VpDefaultCriteriaOrder As String = ""
        If Not VgDB Is Nothing Then
            VgDB.Close
            VgDB.Dispose
        End If
        'Mémorisation des paramètres
        If VgOptions.VgSettings.RestoreSize Then
            VgOptions.VgSettings.RestoredWidth = Me.Width
            VgOptions.VgSettings.RestoredHeight = Me.Height
            VgOptions.VgSettings.RestoredState = Me.WindowState
            Call VgOptions.SaveSettings
        End If
        If VgOptions.VgSettings.RestoreCriteria Then
            For VpI As Integer = 0 To VmFilterCriteria.NCriteria - 1
                VpDefaultCriteriaOrder = VpDefaultCriteriaOrder + "#" + VmFilterCriteria.MyList.Items.Item(VpI).ToString
                If VmFilterCriteria.MyList.GetItemChecked(VpI) Then
                    VpDefaultActivatedCriteria = VpDefaultActivatedCriteria + "#" + VpI.ToString
                End If
            Next VpI
            VgOptions.VgSettings.DefaultCriteriaOrder = VpDefaultCriteriaOrder.Substring(1)
            If VpDefaultActivatedCriteria.Length > 1 Then
                VgOptions.VgSettings.DefaultActivatedCriteria = VpDefaultActivatedCriteria
            Else
                VgOptions.VgSettings.DefaultActivatedCriteria = ""
            End If
            Call VgOptions.SaveSettings
        End If
        Application.Exit
    End Sub
    Sub MnuAddCardsActivate(ByVal sender As Object, ByVal e As EventArgs)
    Dim VpAddCards As frmAddCards
        If mdlToolbox.DBOK Then
            If Not Me.IsSourcePresent Then
                Call mdlToolbox.ShowWarning("Aucune source de cartes n'a été sélectionnée...")
            Else
                Call Me.CheckGridBusy
                Cursor.Current = Cursors.WaitCursor
                VpAddCards = New frmAddCards(Me)
                VpAddCards.ShowDialog
            End If
        End If
    End Sub
    Sub MnuPrefsActivate(ByVal sender As Object, ByVal e As EventArgs)
        VgOptions.ShowDialog
        Me.picScanCard.SizeMode = VgOptions.VgSettings.ImageMode
        Me.grpAutorisations.Visible = Not VgOptions.VgSettings.AutoHideAutorisations
        Me.mnuUpdateAutorisations.Visible = VgOptions.VgSettings.ShowUpdateMenus
        Me.mnuUpdateRulings.Visible = VgOptions.VgSettings.ShowUpdateMenus
        Me.mnuUpdatePrices.Visible = VgOptions.VgSettings.ShowUpdateMenus
        Me.mnuUpdateSimu.Visible = VgOptions.VgSettings.ShowUpdateMenus
        Me.mnuUpdateTxtFR.Visible = VgOptions.VgSettings.ShowUpdateMenus
        Me.mnuUpdateMultiverseId.Visible = VgOptions.VgSettings.ShowUpdateMenus
        Me.mnuFixFR2.Visible = VgOptions.VgSettings.ShowUpdateMenus
        Me.mnuFixPic.Visible = VgOptions.VgSettings.ShowUpdateMenus
        Me.mnuTranslate.Visible = VgOptions.VgSettings.ShowUpdateMenus
    End Sub
    Sub MainFormLoad(ByVal sender As Object, ByVal e As EventArgs)
    '----------------------------------
    'Chargement du formulaire principal
    '----------------------------------
    Dim VpOpen As String = ""
        'Chargement des options
        Call VgOptions.LoadSettings
        With VgOptions.VgSettings
            'Taille par défaut
            If .RestoreSize Then
                Try
                    Me.Size = New Size(.RestoredWidth, .RestoredHeight)
                    Me.WindowState = .RestoredState
                Catch
                End Try
            End If
            'Polices par défaut
            If .FontSize = -1 Then
                .FontSize = Me.txtRichCard.Font.Size
                Call VgOptions.SaveSettings
            Else
                Me.txtRichCard.Font = New Font(Me.txtRichCard.Font.Name, .FontSize)
                Me.txtRichOther.Font = New Font(Me.txtRichOther.Font.Name, .FontSize)
            End If
            'Anciens menus de mises à jour
            If Not .ShowUpdateMenus Then
                Me.mnuUpdateAutorisations.Visible = False
                Me.mnuUpdateRulings.Visible = False
                Me.mnuUpdatePrices.Visible = False
                Me.mnuUpdateSimu.Visible = False
                Me.mnuUpdateTxtFR.Visible = False
                Me.mnuUpdateMultiverseId.Visible = False
                Me.mnuFixFR2.Visible = False
                Me.mnuFixPic.Visible = False
                Me.mnuTranslate.Visible = False
            End If
            'Panneau des images
            Me.picScanCard.SizeMode = .ImageMode
            If .AutoHideImage Then
                Call Me.MnuShowImageActivate(sender, e)
            End If
            'Images
            If (Not File.Exists(VgOptions.VgSettings.PicturesFile)) AndAlso VgOptions.VgSettings.PicturesFile <> "" Then
                Call mdlToolbox.ShowInformation("Vous devriez maintenant télécharger les images des cartes via le menu Fichier / Mettre à jour les images !")
            End If
            If (Not File.Exists(.MagicBack)) OrElse .MagicBack.StartsWith(".") Then
                .MagicBack = Application.StartupPath + mdlConstGlob.CgMagicBack
            End If
            Me.picScanCard.Image = Image.FromFile(.MagicBack)
            'Restriction d'affichage des éditions
            Me.btShowAll.Checked = .ShowAllSeries
            'Groupe des logos des autorisations
            Me.grpAutorisations.Visible = Not .AutoHideAutorisations
            'Critères de classement
            Call mdlToolbox.InitCriteres(Me) 'comme l'ordre des critères est amené à changer et qu'il serait fastidieux de conserver les bons indices, mieux vaut passer par une table de hachage
            If .RestoreCriteria Then
                '-------------------------
                '/!\ Rétro-compatitibilité
                '-------------------------
                If .DefaultCriteriaOrder.Contains("Decks") Then
                    .DefaultActivatedCriteria = "1#7"
                    .DefaultCriteriaOrder = "Type#Couleur#Edition#Coût d'invocation#Rareté#Prix#Quantité#Carte"
                End If
                '-------------------------
                VmFilterCriteria.MyList.Items.Clear
                VmFilterCriteria.MyList.Items.AddRange(.DefaultCriteriaOrder.Split("#"))
            End If
            'Langue par défaut
            Me.mnuCardsFR.Checked = .VFDefault
            Me.btCardsFR.Checked = Me.IsInVFMode
            'Chargement de la base par défaut
            If mdlMain.LoadIcons(Me.imglstTvw) Then
                Call VmFilterCriteria.ValidateCriteria
                VpOpen = If(VmStartup.EndsWith(mdlConstGlob.CgFExtD), VmStartup, .DefaultBase)
                If VpOpen <> "" Then
                    If mdlToolbox.DBOpen(VpOpen) Then
                        Call Me.LoadMnu
                        Call Me.LoadTvw
                    End If
                End If
            Else
                Application.Exit
            End If
            'MAJ auto
            If .CheckForUpdate Then
                mdlToolbox.VgTimer.Start
            End If
        End With
        'Divers
        Call Me.LoadAutorisations("")
'       Me.VgBar = New Windows7ProgressBar(Me)
        'Argument éventuel
        If VmStartup.EndsWith(mdlConstGlob.CgFExtN) Or VmStartup.EndsWith(mdlConstGlob.CgFExtO) Then
            Call Me.MnuExportActivate(sender, Nothing)
            If Not VmMyChildren.DoesntExist(VmMyChildren.ImporterExporter) Then
                VmMyChildren.ImporterExporter.InitImport(VmStartup)
            End If
        End If
    End Sub
    Private Sub ManageDelete(VpSender As Object, VpSQL As String, Optional VpAlternateCaption As String = "")
    Dim VpQuestion As String
        If mdlToolbox.DBOK Then
            If VpAlternateCaption = "" Then
                VpQuestion = "Êtes-vous sûr de vouloir supprimer de manière permanente l'ensemble des cartes saisies dans " + VpSender.Text + " ?"
            Else
                VpQuestion = VpAlternateCaption
            End If
            If mdlToolbox.ShowQuestion(VpQuestion) = System.Windows.Forms.DialogResult.Yes Then
                VgDBCommand.CommandText = VpSQL
                VgDBCommand.ExecuteNonQuery
            End If
        End If
    End Sub
    Sub MnuRemCollecActivate(ByVal sender As Object, ByVal e As EventArgs)
        Call Me.ManageDelete(sender, "Delete * From MyCollection")
    End Sub
    Sub MnuRemSubGamesActivate(ByVal sender As Object, ByVal e As EventArgs)
        Call Me.ManageDelete(sender, "Delete * From MyGames Where GameID = " + mdlToolbox.GetDeckIdFromName(sender.Text))
    End Sub
    Sub MnuRemScoresActivate(ByVal sender As Object, ByVal e As EventArgs)
        Call Me.ManageDelete(sender, "Delete * From MyScores", "Êtes-vous sûr de vouloir supprimer de manière permanente l'ensemble des scores comptés jusqu'à présent ?")
    End Sub
    Sub MnuFixCollecActivate(ByVal sender As Object, ByVal e As EventArgs)
        VgDBCommand.CommandText = "Drop Index EncNbr On MyCollection;"
        VgDBCommand.ExecuteNonQuery
        VgDBCommand.CommandText = "Create Index EncNbr On MyCollection (EncNbr);"
        VgDBCommand.ExecuteNonQuery
        VgDBCommand.CommandText = "Delete * From MyCollection Where Items = 0"
        Try
            VgDBCommand.ExecuteNonQuery
            Call Me.LoadTvw
            Call mdlToolbox.ShowInformation("Terminé !")
        Catch
        End Try
    End Sub
    Sub MnuFixSubGamesActivate(ByVal sender As Object, ByVal e As EventArgs)
        VgDBCommand.CommandText = "Delete * From MyGames Where Items = 0 And GameID = " + mdlToolbox.GetDeckIdFromName(sender.Text)
        VgDBCommand.ExecuteNonQuery
        Call Me.LoadTvw
        Call mdlToolbox.ShowInformation("Terminé !")
    End Sub
    Sub MnuFixSerieActivate(ByVal sender As Object, ByVal e As EventArgs)
        If mdlToolbox.DBOK Then
            Call Me.FixSerie(sender.Text)
        End If
    End Sub
    Sub MnuFixSerie2Click(sender As Object, e As EventArgs)
        If mdlToolbox.DBOK Then
            Call Me.FixSerie2
            Call mdlToolbox.ShowInformation("Terminé !")
        End If
    End Sub
    Sub MnuAboutActivate(ByVal sender As Object, ByVal e As EventArgs)
    Dim VpAbout As New frmAbout
        VpAbout.ShowDialog
    End Sub
    Sub MnuWebsiteClick(sender As Object, e As EventArgs)
        Process.Start(mdlConstGlob.CgURL17)
    End Sub
    Sub MnuDispCollectionActivate(ByVal sender As Object, ByVal e As EventArgs)
    '---------------------------------------------
    'Changement de la sélection active d'affichage
    '---------------------------------------------
    Dim VpDeckMode As Boolean = Not (sender Is Me.mnuDispCollection Or sender Is Me.mnuDispAdvSearch)
        Call Me.CheckGridBusy
        Call Me.ManageDispMenu(sender.Text, VpDeckMode)
        If Not e Is Nothing Then
            Call Me.LoadTvw
        End If
    End Sub
    Sub MnuRefreshActivate(ByVal sender As Object, ByVal e As EventArgs)
        Call Me.MyRefresh
    End Sub
    Sub MnuCardsFRActivate(ByVal sender As Object, ByVal e As MouseEventArgs)
        Me.mnuCardsFR.Checked = Not Me.mnuCardsFR.Checked
        Me.btCardsFR.Checked = Me.IsInVFMode
        If Not Me.tvwExplore.SelectedNode Is Nothing Then
            Call Me.PutInRichText(Me.txtRichCard, Me.imglstCarac, mdlToolbox.MyTxt(Me.tvwExplore.SelectedNode.Tag.Value, Me.IsInVFMode, Me.IsDownFace(Me.tvwExplore.SelectedNode)), "")  'change de suite la traduction de la carte courante
        End If
        Me.tvwExplore.BeginUpdate
        Call Me.RecurChangeLanguage(False)
        Me.tvwExplore.EndUpdate
        Call Me.ReloadCarac
    End Sub
    Sub BtExpandClick(sender As Object, e As EventArgs)
        Call Me.CheckGridBusy
        Me.btExpand.Checked = Not Me.btExpand.Checked
        Call Me.MyRefresh
    End Sub
    Sub TvwExploreMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs)
    '---------------------------------------------------------------
    'Gère l'état du menu contextuel (éléments grisés, cochés, etc...
    '---------------------------------------------------------------
    Dim VpNode As TreeNode = Me.tvwExplore.GetNodeAt(e.Location)
    Dim VpEn As Boolean = False
    Dim VpDouble As Boolean = False
        'Seulement sur clic droit
        If e.Button = System.Windows.Forms.MouseButtons.Right Then
            If Not VpNode Is Nothing AndAlso Not VpNode.Parent Is Nothing Then
                VpEn = ( VpNode.Parent.Tag.Key = "Card.Title" )
                VpDouble = VpNode.Tag.Value3
            End If
            'Modification d'édition
            Me.mnuSwapSerie.Enabled = VpEn And Not Me.IsInAdvSearch
            'Transformation (si carte double)
            If VpEn And VpDouble Then
                Me.mnuTransform.Enabled = True
                Me.mnuTransform.Checked = Me.IsTransformed(VpNode)
            Else
                Me.mnuTransform.Checked = False
                Me.mnuTransform.Enabled = False
            End If
            'Suppression
            Me.mnuDeleteACard.Enabled = VpNode IsNot Nothing And Not Me.IsInAdvSearch
            'Déplacement
            Me.mnuMoveACard.Enabled = VpNode IsNot Nothing And Not Me.IsInAdvSearch
            'Copie
            Me.mnuCopyACard.Enabled = VpNode IsNot Nothing
            Me.mnuClipTitle.Enabled = VpNode IsNot Nothing
            'Achat
            If Not VpNode Is Nothing Then
                Me.mnuBuy.Enabled = VpEn Or ( VpNode.Parent Is Nothing And Not Me.IsInAdvSearch )
            Else
                Me.mnuBuy.Enabled = False
            End If
            'Affichage effectif du menu contextuel
            Me.cmnuTvw.Show(Me.tvwExplore, e.Location)
            Application.DoEvents
            Me.tvwExplore.SelectedNode = VpNode
        End If
    End Sub
    Sub TvwExploreAfterSelect(ByVal sender As Object, ByVal e As TreeViewEventArgs)
    Dim VpParent As TreeNode
    Dim VpElderCriteria As String
    Dim VpTransformed As Boolean
        Call Me.CheckGridBusy
        If Not e.Node.Parent Is Nothing Then
            'Sélection d'un élément de type 'carte'
            If e.Node.Parent.Tag.Key = "Card.Title" Then
                VpTransformed = Me.IsTransformed(e.Node)
                Call Me.ShowCard(If(VpTransformed, Me.picScanCard.Tag, e.Node.Tag.Value), e.Node.Tag.MultiverseId, Me.IsDownFace(e.Node) Xor VpTransformed, VpTransformed, Me.IsReserveSelected)
            Else
                'Préconstruction de la requête avec les conditions sur les critères des ancêtres
                VpParent = e.Node
                VpElderCriteria = ""
                While Not VpParent.Parent Is Nothing
                    VpElderCriteria = VpElderCriteria + Me.ElderCriteria(VpParent.Tag.Value, VpParent.Parent.Tag.Key)
                    VpParent = VpParent.Parent
                End While
                'Sélection d'un élément de type 'édition'
                If e.Node.Parent.Tag.Key = "Card.Series" Then
                    Call Me.ManageMode(False)
                    Call Me.LoadCaracOther(e.Node.Tag.Value, mdlConstGlob.eModeCarac.Serie, VpElderCriteria)
                    If Not Me.splitV2.Panel2Collapsed Then
                        Call mdlToolbox.LoadScanCard(mdlConstGlob.CgImgSeries + mdlToolbox.GetSerieCodeFromName(e.Node.Text, , Me.IsInVFMode), 0, Me.picScanCard)
                    End If
                    Call Me.LoadAutorisations("")
                'Sélection d'un élément de type 'couleur'
                ElseIf e.Node.Parent.Tag.Key = "Spell.Color" Then
                    Call Me.ManageMode(False)
                    Call Me.LoadCaracOther(mdlConstGlob.CgImgColors + e.Node.Text, mdlConstGlob.eModeCarac.Couleur, VpElderCriteria)
                    If Not Me.splitV2.Panel2Collapsed Then
                        Call mdlToolbox.LoadScanCard(mdlConstGlob.CgImgColors + e.Node.Text, 0, Me.picScanCard)
                    End If
                    Call Me.LoadAutorisations("")
                'Sélection d'un élément de type 'type'
                ElseIf e.Node.Parent.Tag.Key = "Card.Type" Then
                    Call Me.ManageMode(False)
                    Call Me.LoadCaracOther(e.Node.Text, mdlConstGlob.eModeCarac.Type, VpElderCriteria)
                    Call Me.LoadAutorisations("")
                End If
            End If
        Else
            Call Me.ClearCarac
            'Sélection d'un élément de type 'deck'
            If VmDeckMode Then
                Call Me.LoadCaracDeck(Me.GetSelectedSource)
                Me.tvwExplore.Focus
            End If
            Call Me.ManageMode(False)
        End If
    End Sub
    Sub MnuSearchCardClick(ByVal sender As Object, ByVal e As EventArgs)
        Me.mnuSearchText.Focus
        Me.mnuSearchText.SelectionStart = 0
        Me.mnuSearchText.SelectionLength = Me.mnuSearchText.Text.Length
    End Sub
    Sub MnuSearchTextKeyUp(ByVal sender As Object, ByVal e As KeyEventArgs)
        If e.KeyCode = 13 Then
            Me.cmnuTvw.Close(ToolStripDropDownCloseReason.Keyboard)
            Call Me.GoFind
        End If
    End Sub
    Sub MnuFindNextClick(ByVal sender As Object, ByVal e As EventArgs)
        Call Me.FindNextCard
    End Sub
    Sub TvwExploreKeyUp(ByVal sender As Object, ByVal e As KeyEventArgs)
        If e.KeyCode = Keys.F3 And Me.mnuFindNext.Enabled Then
            Call Me.FindNextCard
        ElseIf e.KeyCode = Keys.Delete And Me.tvwExplore.SelectedNode IsNot Nothing And Not Me.IsInAdvSearch Then
            If mdlToolbox.ShowQuestion("Êtes-vous sûr de vouloir supprimer '" + Me.tvwExplore.SelectedNode.Text + "' ?")  = System.Windows.Forms.DialogResult.Yes Then
                Call Me.ManageMultipleTransferts(clsTransferResult.EgTransfertType.Deletion)
            End If
        End If
    End Sub
    Sub MnuUpdateSimuActivate(ByVal sender As Object, ByVal e As EventArgs)
        If mdlToolbox.DBOK Then
            If mdlToolbox.ShowQuestion("Se connecter à Internet pour récupérer les derniers modèles et/ou historiques ?") = System.Windows.Forms.DialogResult.Yes Then
                Call mdlToolbox.DownloadNow(New Uri(mdlConstGlob.VgOptions.VgSettings.DownloadServer + CgURL3B), mdlConstGlob.CgUpDDBb)
                If File.Exists(Application.StartupPath + mdlConstGlob.CgUpDDBb) Then
                    Call mdlToolbox.DBImport(Application.StartupPath + mdlConstGlob.CgUpDDBb)
                    Call mdlToolbox.DBAdaptEncNbr
                Else
                    Call mdlToolbox.ShowWarning(mdlConstGlob.CgDL3b)
                End If
            End If
        End If
    End Sub
    Sub MnuUpdateRulingsClick(sender As Object, e As EventArgs)
        If mdlToolbox.DBOK Then
            If mdlToolbox.ShowQuestion("Se connecter à Internet pour récupérer la liste des règles spécifiques pour les cartes ?" + vbCrLf + "Cliquer sur 'Non' pour mettre à jour depuis le fichier sur le disque dur...") = System.Windows.Forms.DialogResult.Yes Then
                Call mdlToolbox.DownloadUpdate(New Uri(VgOptions.VgSettings.DownloadServer + mdlConstGlob.CgURL19), mdlConstGlob.CgUpRulings)
            Else
                If File.Exists(Application.StartupPath + mdlConstGlob.CgUpRulings) Then
                    Call Me.UpdateRulings
                Else
                    Call mdlToolbox.ShowWarning("Fichier des règles spécifiques introuvable...")
                End If
            End If
        End If
    End Sub
    Sub MnuUpdateAutorisationsClick(sender As Object, e As EventArgs)
        If mdlToolbox.DBOK Then
            If mdlToolbox.ShowQuestion("Se connecter à Internet pour récupérer la liste des autorisations des cartes en tournois ?") = System.Windows.Forms.DialogResult.Yes Then
                Call mdlToolbox.DownloadNow(New Uri(mdlConstGlob.VgOptions.VgSettings.DownloadServer + CgURL15), mdlConstGlob.CgUpAutorisations)
                If File.Exists(Application.StartupPath + mdlConstGlob.CgUpAutorisations) Then
                    Call Me.UpdateAutorisations(Application.StartupPath + mdlConstGlob.CgUpAutorisations)
                    Call mdlToolbox.SecureDelete(Application.StartupPath + mdlConstGlob.CgUpAutorisations)
                Else
                    Call mdlToolbox.ShowWarning(mdlConstGlob.CgDL3b)
                End If
            End If
        End If
    End Sub
    Sub MnuUpdatePricesActivate(ByVal sender As Object, ByVal e As EventArgs)
        If mdlToolbox.DBOK Then
            If mdlToolbox.ShowQuestion("Se connecter à Internet pour récupérer la dernière liste des prix ?" + vbCrLf + "Cliquer sur 'Non' pour mettre à jour depuis un fichier sur le disque dur...") = System.Windows.Forms.DialogResult.Yes Then
                Call mdlToolbox.DownloadNow(New Uri(mdlConstGlob.VgOptions.VgSettings.DownloadServer + CgURL9), mdlConstGlob.CgUpPrices)
                If File.Exists(Application.StartupPath + mdlConstGlob.CgUpPrices) Then
                    Call Me.UpdatePrices(Application.StartupPath + mdlConstGlob.CgUpPrices, False)
                    Call mdlToolbox.SecureDelete(Application.StartupPath + mdlConstGlob.CgUpPrices)
                Else
                    Call mdlToolbox.ShowWarning(mdlConstGlob.CgDL3b)
                End If
            Else
                Me.dlgOpen2.ShowDialog
                If Me.dlgOpen2.FileName.Trim <> "" Then
                    Call Me.UpdatePrices(Me.dlgOpen2.FileName, True)
                End If
            End If
        End If
    End Sub
    Sub MnuUpdatePicturesActivate(ByVal sender As Object, ByVal e As EventArgs)
    Dim VpStr As String
        If mdlToolbox.DBOK Then
            If File.Exists(VgOptions.VgSettings.PicturesFile) Then
                If (New FileInfo(VgOptions.VgSettings.PicturesFile)).Length < mdlConstGlob.CgImgMinLength Then
                    If mdlToolbox.ShowQuestion("La base d'images semble être corrompue." + vbCrLf + "Voulez-vous la re-télécharger maintenant ?") = System.Windows.Forms.DialogResult.Yes Then
                        'Re-téléchargement complet de la base principale
                        Me.IsInImgDL = True
                        Call mdlToolbox.DownloadUpdate(New Uri(mdlConstGlob.VgOptions.VgSettings.DownloadServer + CgURL10 + mdlConstGlob.CgUpDDBd), VgOptions.VgSettings.PicturesFile, False)
                    End If
                    Exit Sub
                End If
                If mdlToolbox.ShowQuestion("Se connecter à Internet pour récupérer les dernières images ?" + vbCrLf + "Cliquer sur 'Non' pour mettre à jour depuis un fichier sur le disque dur...") = System.Windows.Forms.DialogResult.Yes Then
                    Call mdlToolbox.CheckForPicUpdates
                Else
                    Me.dlgOpen3.ShowDialog
                    VpStr = Me.dlgOpen3.FileName.Trim
                    If VpStr <> "" Then
                        If mdlToolbox.ShowQuestion("Les images des cartes vont être mises à jour avec les données suivantes :" + vbCrLf + VpStr + vbCrLf + "L'opération pourra durer plusieurs secondes, patienter jusqu'à la notification. Continuer ?") = System.Windows.Forms.DialogResult.Yes Then
                            VpStr = VpStr.Substring(0, VpStr.Length - 4) + ".log"
                            If File.Exists(VpStr) Then
                                Call Me.UpdatePictures(Me.dlgOpen3.FileName, VpStr)
                            Else
                                Call mdlToolbox.ShowWarning("Impossible de trouver le fichier journal accompagnateur...")
                            End If
                        End If
                    End If
                End If
            Else
                If mdlToolbox.ShowQuestion("La base d'images est introuvable." + vbCrLf + "Voulez-vous la télécharger maintenant ?") = System.Windows.Forms.DialogResult.Yes Then
                    'Téléchargement complet de la base principale
                    Me.IsInImgDL = True
                    Call mdlToolbox.DownloadUpdate(New Uri(mdlConstGlob.VgOptions.VgSettings.DownloadServer + CgURL10 + mdlConstGlob.CgUpDDBd), VgOptions.VgSettings.PicturesFile, False)
                End If
            End If
        End If
    End Sub
    Sub MnuUpdateTxtFRClick(sender As Object, e As EventArgs)
        If mdlToolbox.DBOK Then
            If mdlToolbox.ShowQuestion("Se connecter à Internet pour récupérer les textes des cartes en français ?" + vbCrLf + "Cliquer sur 'Non' pour mettre à jour depuis le fichier sur le disque dur...") = System.Windows.Forms.DialogResult.Yes Then
                Call mdlToolbox.DownloadUpdate(New Uri(mdlConstGlob.VgOptions.VgSettings.DownloadServer + mdlConstGlob.CgURL11), mdlConstGlob.CgUpTXTFR)
            Else
                If File.Exists(Application.StartupPath + mdlConstGlob.CgUpTXTFR) Then
                    Call Me.UpdateTxtFR
                Else
                    Call mdlToolbox.ShowWarning("Fichier des traductions introuvable...")
                End If
            End If
        End If
    End Sub
    Sub MnuUpdateMultiverseIdClick(sender As Object, e As EventArgs)
        If mdlToolbox.DBOK Then
            If mdlToolbox.ShowQuestion("Se connecter à Internet pour récupérer la liste des identifiants Multiverse ?") = System.Windows.Forms.DialogResult.Yes Then
                If Me.FixMultiverse2 Then
                    Call mdlToolbox.ShowInformation("Mise à jour des identifiants terminée !")
                Else
                    Call mdlToolbox.ShowWarning(mdlConstGlob.CgDL3b)
                End If
            End If
        End If
    End Sub
    Sub MainFormFormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs)
        Call Me.MnuExitActivate(sender, e)
    End Sub
    Sub MnuStatsActivate(ByVal sender As Object, ByVal e As EventArgs)
    Dim VpStats As frmStats
        If mdlToolbox.DBOK Then
            If Me.GetNCards(Me.MySource, mdlConstGlob.eCountMode.Distinct) > 1 Then
                VpStats = New frmStats(Me)
                VpStats.Show
            Else
                Call mdlToolbox.ShowWarning("Il faut au minimum une sélection de 2 cartes distinctes pour pouvoir lancer l'affichage des statistiques...")
            End If
        End If
    End Sub
    Sub MnuHelpActivate(ByVal sender As Object, ByVal e As EventArgs)
        If File.Exists(Application.StartupPath + mdlConstGlob.CgHLPFile) Then
            Try
                If File.Exists(mdlConstGlob.CgVirtualPath + mdlConstGlob.CgHLPFile) Then
                    Process.Start(mdlConstGlob.CgVirtualPath + mdlConstGlob.CgHLPFile)
                Else
                    Process.Start(Application.StartupPath + mdlConstGlob.CgHLPFile)
                End If
            Catch
                Call mdlToolbox.ShowWarning("Aucune installation d'Adobe Reader n'a été détectée sur votre système..." + vbCrLf + "Impossible de continuer.")
            End Try
        Else
            If mdlToolbox.ShowQuestion("Le fichier d'aide est introuvable." + vbCrLf + "Voulez-vous le télécharger maintenant ?") = System.Windows.Forms.DialogResult.Yes Then
                Call mdlToolbox.DownloadUpdate(New Uri(mdlConstGlob.VgOptions.VgSettings.DownloadServer + CgURL13), mdlConstGlob.CgHLPFile)
            End If
        End If
    End Sub
    Sub MnuSortClick(ByVal sender As Object, ByVal e As EventArgs)
        Call Me.SortTvw
    End Sub
    Sub MnuStdSearchActivate(ByVal sender As Object, ByVal e As EventArgs)
    Dim VpStr As String
        If mdlToolbox.DBOK Then
            VpStr = InputBox("Rechercher dans le titre des cartes présentes dans l'explorateur :", "Recherche", mdlConstGlob.CgCard)
            Call Me.CheckGridBusy
            If VpStr.Trim <> "" Then
                Me.mnuSearchText.Text = VpStr
                Call Me.GoFind
            End If
        End If
    End Sub
    Sub MnuAdvancedSearchActivate(ByVal sender As Object, ByVal e As EventArgs)
    Dim VpSearch As frmSearch
        If mdlToolbox.DBOK Then
            Call Me.CheckGridBusy
            If VmMyChildren.DoesntExist(VmMyChildren.Searcher) Then
                VpSearch = New frmSearch(Me)
                VmMyChildren.Searcher = VpSearch
            Else
                VpSearch = VmMyChildren.Searcher
            End If
            VpSearch.Show
            VpSearch.BringToFront
        End If
    End Sub
    Sub MnuNewEditionActivate(ByVal sender As Object, ByVal e As EventArgs)
    Dim VpNewEdition As frmNewEdition
        If mdlToolbox.DBOK Then
            VpNewEdition = New frmNewEdition
            VpNewEdition.ShowDialog
            Call mdlMain.LoadIcons(Me.imglstTvw)    'recharge les icônes au cas où de nouveaux logos auraient été ajoutés lors de la procédure
        End If
    End Sub
    Sub MnuFixTxtVFActivate(ByVal sender As Object, ByVal e As EventArgs)
        If mdlToolbox.DBOK Then
            If MessageBox.Show("Cette opération peut prendre beaucoup de temps..." + vbCrLf + "Continuer ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = System.Windows.Forms.DialogResult.Yes Then
                Call Me.FixFR3
                Call mdlToolbox.ShowInformation("Les textes des cartes en français ont été réinitialisés..." + vbCrLf + "Relancer la mise à jour des textes des cartes en VF pour terminer.")
            End If
        End If
    End Sub
    Sub MnuFixSubTypesActivate(ByVal sender As Object, ByVal e As EventArgs)
    Dim VpFull As Boolean
        If mdlToolbox.DBOK Then
            VpFull = ( MessageBox.Show("Corriger également les super-types (cette opération peut prendre beaucoup de temps) ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = System.Windows.Forms.DialogResult.Yes )
            Call Me.FixSubTypesDB(VpFull)
            Call mdlToolbox.ShowInformation("Terminé !")
        End If
    End Sub
    Sub MnuFixPricesActivate(ByVal sender As Object, ByVal e As EventArgs)
        If mdlToolbox.DBOK Then
            Call Me.FixPrices
            Call mdlToolbox.ShowInformation("Terminé !")
        End If
    End Sub
    Sub MnuFixOnlyVOActivate(ByVal sender As Object, ByVal e As EventArgs)
        If mdlToolbox.DBOK Then
            Call Me.FixOnlyVO
            Call mdlToolbox.ShowInformation("Terminé !")
        End If
    End Sub
    Sub MnuFixCreaturesClick(sender As Object, e As EventArgs)
        If mdlToolbox.DBOK Then
            Call Me.FixCreatures("*")
            For VpI As Integer = 0 To 10
                Call Me.FixCreatures(VpI.ToString)
            Next VpI
            Call mdlToolbox.ShowInformation("Terminé !")
        End If
    End Sub
    Sub MnuFixAssocClick(sender As Object, e As EventArgs)
        If mdlToolbox.DBOK Then
            Call Me.FixAssoc
            Call mdlToolbox.ShowInformation("Terminé !")
        End If
    End Sub
    Sub MnuTranslateActivate(ByVal sender As Object, ByVal e As EventArgs)
    Dim VpTranslator As frmTranslate
        If mdlToolbox.DBOK Then
            VpTranslator = New frmTranslate(Me)
            VpTranslator.ShowDialog
        End If
    End Sub
    Sub MnuFixFRActivate(ByVal sender As Object, ByVal e As EventArgs)
        If mdlToolbox.DBOK Then
            Call Me.FixFR
            Call mdlToolbox.ShowInformation("Terminé !")
        End If
    End Sub
    Sub MnuFixMultiverseIdAllClick(ByVal sender As Object, ByVal e As EventArgs)
        If mdlToolbox.DBOK Then
            If MessageBox.Show("Cette opération peut prendre beaucoup de temps..." + vbCrLf + "Continuer ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = System.Windows.Forms.DialogResult.Yes Then
                Call Me.FixMultiverse
                Call mdlToolbox.ShowInformation("Terminé !")
            End If
        End If
    End Sub
    Sub MnuFixMultiverseIdOneClick(sender As Object, e As EventArgs)
    Dim VpCode As String
        If mdlToolbox.DBOK Then
            VpCode = InputBox("Quel est le code de l'édition dont il manque les identifiants Multiverse ?", "Récupération des identifiants Multiverse", "(code)")
            If VpCode <> "" Then
                Call Me.FixMultiverse(VpCode.ToUpper)
                Call mdlToolbox.ShowInformation("Terminé !")
            End If
        End If
    End Sub
    Sub MnuFixPicClick(sender As Object, e As EventArgs)
        If mdlToolbox.DBOK And File.Exists(VgOptions.VgSettings.PicturesFile) Then
            If mdlToolbox.ShowQuestion("Se connecter à Internet pour récupérer le correctif ?") = System.Windows.Forms.DialogResult.Yes Then
                If Not Me.IsInImgDL Then
                    Call Me.FixPictures
                    Call mdlToolbox.ShowInformation("Terminé !")
                Else
                    Call mdlToolbox.ShowWarning(mdlConstGlob.CgDL2c)
                End If
            End If
        End If
    End Sub
    Sub MnuFixFR2Click(sender As Object, e As EventArgs)
        If mdlToolbox.DBOK Then
            If mdlToolbox.ShowQuestion("Se connecter à Internet pour récupérer le correctif ?") = System.Windows.Forms.DialogResult.Yes Then
                Call Me.FixFR2
            End If
            Call mdlToolbox.ShowInformation("Terminé !")
        End If
    End Sub
    Sub MnuCheckForUpdatesActivate(ByVal sender As Object, ByVal e As EventArgs)
        Call mdlToolbox.CheckForUpdates(True)
    End Sub
    Sub BtCheckForUpdatesClick(ByVal sender As Object, ByVal e As EventArgs)
        Call mdlToolbox.CheckForUpdates(True, ( mdlToolbox.ShowQuestion("Souhaitez-vous plutôt rechercher les mises à jour bêta (moins stables) ?") = System.Windows.Forms.DialogResult.Yes ), True)
    End Sub
    Public Sub MnuContenuUpdateClick(sender As Object, e As EventArgs)
    Dim VpContenuUpdate As frmUpdateContent
        If mdlToolbox.DBOK Then
            If VmMyChildren.DoesntExist(VmMyChildren.ContenuUpdater) Then
                VpContenuUpdate = New frmUpdateContent
                VmMyChildren.ContenuUpdater = VpContenuUpdate
            Else
                VpContenuUpdate = VmMyChildren.ContenuUpdater
            End If
            VpContenuUpdate.Show
            VpContenuUpdate.BringToFront
        End If
    End Sub
    Sub MnuPerfsActivate(ByVal sender As Object, ByVal e As EventArgs)
    Dim VpPerfs As frmPerfs
        If mdlToolbox.DBOK Then
            If VmMyChildren.DoesntExist(VmMyChildren.PerfsCounter) Then
                VpPerfs = New frmPerfs(Me)
                VmMyChildren.PerfsCounter = VpPerfs
            Else
                VpPerfs = VmMyChildren.PerfsCounter
            End If
            VpPerfs.Show
            VpPerfs.BringToFront
        End If
    End Sub
    Sub MnuClipTitleClick(sender As Object, e As EventArgs)
        Clipboard.SetDataObject(Me.tvwExplore.SelectedNode.Text)
    End Sub
    Sub MnuMoveACardActivate(ByVal sender As Object, ByVal e As EventArgs)
        Call Me.ManageMultipleTransferts(clsTransferResult.EgTransfertType.Move, sender.Text)
    End Sub
    Sub MnuDeleteACardClick(ByVal sender As Object, ByVal e As EventArgs)
        Call Me.ManageMultipleTransferts(clsTransferResult.EgTransfertType.Deletion)
    End Sub
    Sub MnuCopyACardActivate(sender As Object, e As EventArgs)
        Call Me.ManageMultipleTransferts(clsTransferResult.EgTransfertType.Copy, sender.Text)
    End Sub
    Sub MnuSwapSerieClick(sender As Object, e As EventArgs)
        Call Me.ManageMultipleTransferts(clsTransferResult.EgTransfertType.Swap)
    End Sub
    Sub MnuTransformClick(sender As Object, e As EventArgs)
    Dim VpNode As TreeNode = Me.tvwExplore.SelectedNode
    Dim VpDownFace As Boolean
    Dim VpTransformed As Boolean
    Dim VpNames As clsTag
        If Not VpNode Is Nothing AndAlso Not VpNode.Parent Is Nothing Then
            If VpNode.Parent.Tag.Key = "Card.Title" AndAlso VpNode.Tag.Value3 Then      'On doit refaire la vérif. au cas où l'évènement aurait été triggé par un clic sur l'image
                VpDownFace = Me.IsDownFace(VpNode)
                VpTransformed = Me.IsTransformed(VpNode)
                VpNames = Me.GetTransformedTag(VpNode.Tag.Value, VpDownFace Xor VpTransformed, VpDownFace, Me.IsInVFMode And (VpNode.Tag.Value = VpNode.Tag.Value2))
                If Me.ShowCard(VpNames.Value, VpNames.MultiverseId, Not (VpDownFace Xor VpTransformed), Not VpTransformed, Me.IsReserveSelected) Then
                    'Met à jour le noeud de l'arbre
                    VpNode.Text = If(Me.IsInVFMode, VpNames.Value2, VpNames.Value)
                    'Mémorise la référence de l'image
                    Me.picScanCard.Tag = VpNames.Value
                End If
            End If
        End If
    End Sub
    Sub MnuExcelGenActivate(ByVal sender As Object, ByVal e As EventArgs)
    Dim VpXL As frmXL
        If mdlToolbox.DBOK Then
            VpXL = New frmXL(Me)
            VpXL.ShowDialog
        End If
    End Sub
    Sub MnuWordGenClick(sender As Object, e As EventArgs)
    Dim VpWord As frmWord
        If mdlToolbox.DBOK Then
            VpWord = New frmWord(Me)
            VpWord.ShowDialog
        End If
    End Sub
    Sub MnuRemEditionActivate(ByVal sender As Object, ByVal e As EventArgs)
    Dim VpDeletor As frmDeleteEdition
        If mdlToolbox.DBOK Then
            If VmMyChildren.DoesntExist(VmMyChildren.EditionDeleter) Then
                VpDeletor = New frmDeleteEdition(Me)
                VmMyChildren.EditionDeleter = VpDeletor
            Else
                VpDeletor = VmMyChildren.EditionDeleter
            End If
            VpDeletor.Show
            VpDeletor.BringToFront
        End If
    End Sub
    Sub MnuSimuActivate(ByVal sender As Object, ByVal e As EventArgs)
    Dim VpSimu As frmSimu
        If mdlToolbox.DBOK Then
            VpSimu = New frmSimu(Me)
            VpSimu.Show
        End If
    End Sub
    Sub MnuPlateauClick(sender As Object, e As EventArgs)
    Dim VpPlateau As frmBoard
    Dim VpN As Integer
        If mdlToolbox.DBOK Then
            VpN = Me.GetNCards(Me.MySource, mdlConstGlob.eCountMode.All)
            If VpN <= mdlConstGlob.CgMaxVignettes Then
                If VpN >= mdlConstGlob.CgNMain Then
                    VpPlateau = New frmBoard(Me)
                    VpPlateau.Show
                Else
                    Call mdlToolbox.ShowWarning("Il faut avoir au moins " + mdlConstGlob.CgNMain.ToString + " cartes saisies pour tirer une main...")
                End If
            Else
                Call mdlToolbox.ShowWarning("Le nombre de vignettes à générer est trop important..." + vbCrLf + "Maximum autorisé : " + mdlConstGlob.CgMaxVignettes.ToString + ".")
            End If
        End If
    End Sub
    Sub MnuExportActivate(ByVal sender As Object, ByVal e As EventArgs)
    Dim VpImporterExporter As frmExport
        If mdlToolbox.DBOK Then
            If VmMyChildren.DoesntExist(VmMyChildren.ImporterExporter) Then
                VpImporterExporter = New frmExport(Me)
                VmMyChildren.ImporterExporter = VpImporterExporter
            Else
                VpImporterExporter = VmMyChildren.ImporterExporter
            End If
            VpImporterExporter.Show
            VpImporterExporter.BringToFront
        End If
    End Sub
    Sub MnuGestDecksActivate(ByVal sender As Object, ByVal e As EventArgs)
    Dim VpGestDecks As frmManageDecks
        If mdlToolbox.DBOK Then
            If VmMyChildren.DoesntExist(VmMyChildren.DecksManager) Then
                VpGestDecks = New frmManageDecks(Me)
                VmMyChildren.DecksManager = VpGestDecks
            Else
                VpGestDecks = VmMyChildren.DecksManager
            End If
            VpGestDecks.Show
            VpGestDecks.BringToFront
        End If
    End Sub
    Sub MnuGestAdvClick(sender As Object, e As EventArgs)
    Dim VpGestAdv As frmManageAdv
        If mdlToolbox.DBOK Then
            If VmMyChildren.DoesntExist(VmMyChildren.AdversairesManager) Then
                VpGestAdv = New frmManageAdv
                VmMyChildren.AdversairesManager = VpGestAdv
            Else
                VpGestAdv = VmMyChildren.AdversairesManager
            End If
            VpGestAdv.Show
            VpGestAdv.BringToFront
        End If
    End Sub
    Sub MnuShowImageActivate(ByVal sender As Object, ByVal e As EventArgs)
    Dim VpDistance As Integer = Me.splitV.SplitterDistance
        With mdlConstGlob.VgSessionSettings
            Me.SuspendLayout
            'Si le panneau est affiché, le masque
            If Not Me.splitV2.Panel2Collapsed Then
                .SplitterDistance = Me.splitV2.SplitterDistance
                .FormSubWidth = Me.splitV2.Panel2.Width
                Me.Width -= .FormSubWidth
                Me.splitV2.Panel2Collapsed = True
            'Sinon l'affiche
            Else
                Me.splitV2.Panel2Collapsed = False
                Me.Width += .FormSubWidth
            End If
            Me.ResumeLayout
            Me.splitV.SplitterDistance = VpDistance
            If .SplitterDistance <> 0 Then
                Me.splitV2.SplitterDistance = .SplitterDistance
            End If
        End With
    End Sub
    Sub MnuCheckForBetasActivate(ByVal sender As Object, ByVal e As EventArgs)
        Call mdlToolbox.CheckForUpdates(True, True)
    End Sub
    Sub MnuBuyFormClick(sender As Object, e As EventArgs)
        Call Me.OnlineBuy(False, False)
    End Sub
    Sub MnuBuyItemClick(ByVal sender As Object, ByVal e As EventArgs)
        Call Me.OnlineBuy(True, ( Me.tvwExplore.SelectedNode.Parent Is Nothing ))
    End Sub
    Sub MnuDBOpenClick(sender As Object, e As EventArgs)
        Call Me.ClearAll
        If Me.dlgOpen.ShowDialog <> System.Windows.Forms.DialogResult.Cancel Then
            Call Me.DBOpenInit(Me.dlgOpen.FileName)
        End If
    End Sub
    Sub MnuDBSaveClick(sender As Object, e As EventArgs)
    Dim VpSource As FileInfo
        If mdlToolbox.DBOK Then
            VpSource = New FileInfo(VgDB.DataSource)
            If Me.dlgSave.ShowDialog <> System.Windows.Forms.DialogResult.Cancel Then
                If Not VpSource.FullName = Me.dlgSave.FileName Then
                    Call mdlToolbox.SecureDelete(Me.dlgSave.FileName)
                    If Not File.Exists(Me.dlgSave.FileName) Then
                        File.Copy(VpSource.FullName, Me.dlgSave.FileName)
                        If mdlToolbox.DBOpen(Me.dlgSave.FileName) Then
                            Me.lblDB.Text = VgDB.DataSource
                            Call Me.LoadMnu
                            Call Me.LoadTvw
                        End If
                    Else
                        Call mdlToolbox.ShowWarning("Impossible d'écraser " + Me.dlgSave.FileName + "..." + vbCrLf + "Le fichier est en cours d'utilisation.")
                    End If
                Else
                    Call mdlToolbox.ShowWarning("Inutile de sélectionner le fichier actuellement ouvert, il est déjà sauvegardé automatiquement !")
                End If
            End If
        End If
    End Sub
    Sub MnuCancelClick(sender As Object, e As EventArgs)
        If mdlToolbox.VgClient.IsBusy Then
            If ShowQuestion("Êtes-vous sûr de vouloir annuler le téléchargement en cours ?") = DialogResult.Yes Then
                mdlToolbox.VgClient.CancelAsync
                If Not VmMyChildren.DoesntExist(VmMyChildren.ContenuUpdater) Then
                    If VmMyChildren.ContenuUpdater.PassiveUpdate = mdlConstGlob.ePassiveUpdate.InProgress Then
                        VmMyChildren.ContenuUpdater.PassiveUpdate = mdlConstGlob.ePassiveUpdate.Failed
                    End If
                End If
                Me.IsDownloadInProgress = False
                Me.IsInImgDL = False
            End If
        End If
    End Sub
    Sub MnuInfosDLClick(sender As Object, e As EventArgs)
    '------------------------------------------------------------
    'Estimation de l'heure de fin pour le téléchargement en cours
    '------------------------------------------------------------
    Dim VpETA As TimeSpan
    Dim VpDebut As Date = CDate(Me.btDownload.Tag)
        If Me.prgAvance.Value > 1 Then
            VpETA = New TimeSpan(0, 0, Now.Subtract(VpDebut).TotalSeconds * (Me.prgAvance.Maximum / Me.prgAvance.Value - 1))
            Call mdlToolbox.ShowInformation("Le téléchargement a débuté à " + VpDebut.ToLongTimeString + "..." + vbCrLf + vbCrLf + "Estimation de l'heure de fin : " + Now.Add(VpETA).ToLongTimeString + ".")
        End If
    End Sub
    Sub MnuRestorePrevClick(sender As Object, e As EventArgs)
    '-------------------------------------
    'Rétrogade la version de l'application
    '-------------------------------------
        If File.Exists(Application.StartupPath + mdlConstGlob.CgDownDFile) Then
            If mdlToolbox.ShowQuestion("Êtes-vous sûr de vouloir restaurer la précédente version sauvegardée de l'application ?")  = System.Windows.Forms.DialogResult.Yes Then
                File.Move(Application.StartupPath + mdlConstGlob.CgDownDFile, Application.StartupPath + mdlConstGlob.CgUpDFile)
                Try
                    Process.Start(New ProcessStartInfo(Application.StartupPath + CgUpdater))
                Catch
                    Call mdlToolbox.ShowInformation("Opération annulée..." + vbCrLf + "Aucun changement n'a été effectué.")
                End Try
            End If
        Else
            Call mdlToolbox.ShowWarning("Aucune version antérieure n'a été trouvée dans le répertoire d'installation...")
        End If
    End Sub
    Sub CellFocusEntered(sender As Object, e As PositionEventArgs)
    '--------------------------------------------------------------------------------------
    'Ajuste l'édition courante (celle qui a été sélectionnée dans la grid par l'utilisteur)
    '--------------------------------------------------------------------------------------
    Dim VpCell As Cells.Cell = e.Cell
    Dim VpEdition As String = VpCell.Tag
    Dim VpId As Long
        'Si on est en VO, il peut arriver que le texte change selon l'édition
        If Not Me.IsInVFMode Then
            VgDBCommand.CommandText = "Select CardText From Card Where Title = '" + Me.CurrentCardTitle.Replace("'", "''") + "' And Series = '" + VpEdition + "';"
            Call Me.PutInRichText(Me.txtRichCard, Me.imglstCarac, VgDBCommand.ExecuteScalar.ToString, "")
        End If
        'Si l'utilisateur a choisi de récupérer l'image en ligne, il faut la changer selon l'édition
        If VgOptions.VgSettings.PicturesSource = mdlConstGlob.ePicturesSource.Online AndAlso Not Me.splitV2.Panel2Collapsed Then
            VgDBCommand.CommandText = "Select MultiverseId From Card Where Title = '" + Me.CurrentCardTitle.Replace("'", "''") + "' And Series = '" + VpEdition + "';"
            VpId = CLng(VgDBCommand.ExecuteScalar)
            If VpId <> 0 Then Me.picScanCard.LoadAsync(mdlConstGlob.CgURL0.Replace("#", VpId.ToString))
        End If
    End Sub
    Sub CellValidated(sender As Object, e As CellEventArgs)
    '-----------------------------------------------------------
    'Ajuste le nombre de cartes en stock dans la base de données
    '-----------------------------------------------------------
    Dim VpCell As Cells.Cell = e.Cell
    Dim VpGrid As Grid = VpCell.Grid
    Dim VpSource As String = Me.MySource
    Dim VpEncNbr As Long
    Dim VpFoil As Boolean
    Dim VpReserve As Boolean
    Dim VpItems As Integer
        'Cohérence de la valeur
        If CInt(VpCell.Value) < 0 Then
            VpCell.Value = 0
        End If
        'Récupération du numéro encyclopédique, du flag foil et du flag réserve
        VpEncNbr = mdlToolbox.GetEncNbr(CType(Me.tvwExplore.SelectedNode.Tag, clsTag).Value, mdlToolbox.GetSerieCodeFromName(VpGrid(VpCell.Row, 0).Value, , Me.IsInVFMode))
        VpFoil = (VpCell.Column = 5) 'un peu crade, mais une modification sur la dernière colonne indique qu'on a touché au stock foil
        VpReserve = Me.IsReserveSelected
        'Cas 1 : si la valeur est nulle, il s'agit d'une suppression
        With VgDBCommand
            If VpCell.Value = 0 Then
                .CommandText = "Delete * From " + VpSource + " Where Foil = " + VpFoil.ToString + " And EncNbr = " + VpEncNbr.ToString + If(VmDeckMode, " And GameID = " + mdlToolbox.GetDeckIdFromName(Me.GetSelectedSource) + " And Reserve = " + VpReserve.ToString + ";", ";")
                .ExecuteNonQuery
            Else
                .CommandText = "Select Items From " + VpSource + " Where EncNbr = " + VpEncNbr.ToString + " And Foil = " + VpFoil.ToString + If(VmDeckMode, " And GameID = " + mdlToolbox.GetDeckIdFromName(Me.GetSelectedSource) + " And Reserve = " + VpReserve.ToString + ";", ";")
                VpItems = .ExecuteScalar
                'Cas 2 : il s'agit d'une mise à jour du stock
                If VpItems > 0 Then
                    .CommandText = "Update " + VpSource + " Set Items = " + VpCell.Value.ToString + " Where EncNbr = " + VpEncNbr.ToString + " And Foil = " + VpFoil.ToString + If(VmDeckMode, " And GameID = " + mdlToolbox.GetDeckIdFromName(Me.GetSelectedSource) + " And Reserve = " + VpReserve.ToString + ";", ";")
                    .ExecuteNonQuery
                'Cas 3 : il s'agit d'une insertion
                Else
                    .CommandText = "Insert Into " + VpSource + " Values (" + If(VmDeckMode, mdlToolbox.GetDeckIdFromName(Me.GetSelectedSource).ToString + ", ", "") + VpEncNbr.ToString + ", " + VpCell.Value.ToString + ", " + VpFoil.ToString + If(VmDeckMode, ", " + VpReserve.ToString, "") + ");"
                    .ExecuteNonQuery
                End If
            End If
        End With
        'Mise à jour total cartes attachées
        Call Me.ShowNCards(VpSource)
    End Sub
    Sub BtHistPricesFoilClick(sender As Object, e As EventArgs)
        Application.UseWaitCursor = True
        Call Me.LoadPricesHistory(True)
        Application.UseWaitCursor = False
    End Sub
    Sub BtHistPricesSimpleClick(sender As Object, e As EventArgs)
        Application.UseWaitCursor = True
        Call Me.LoadPricesHistory(False)
        Application.UseWaitCursor = False
    End Sub
    Sub BtHistPricesActivate(sender As Object, e As EventArgs)
        Me.btHistPrices.Checked = True
        Application.DoEvents
        With Me.btHistPrices.ButtonBounds
            'un peu crade mais le dropdownmenu ne fonctionne plus sous Windows 7 et oblige à passer par un menu contextuel
            Me.cmnuCbar.Show(Me.cbarProperties, New Point(.Left, .Bottom))
        End With
    End Sub
    Sub BtShowAllActivate(sender As Object, e As EventArgs)
        Me.btShowAll.Checked = Not Me.btShowAll.Checked
        Call Me.CheckGridBusy
        Call Me.ReloadCarac
    End Sub
    Sub BtCardUseActivate(sender As Object, e As EventArgs)
        Me.btCardUse.Checked = Not Me.btCardUse.Checked
        Me.btShowAll.Enabled = (Not Me.IsInAdvSearch) And (Not Me.btCardUse.Checked)
        Call Me.CheckGridBusy
        Call Me.InitGrids
        Call Me.ReloadCarac
    End Sub
    Sub TvwExploreDragEnter(sender As Object, e As DragEventArgs)
    Dim VpStr As String
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            VpStr = e.Data.GetData(DataFormats.FileDrop)(0)
            If VpStr.EndsWith(mdlConstGlob.CgFExtN) Or VpStr.EndsWith(mdlConstGlob.CgFExtO) Or VpStr.EndsWith(mdlConstGlob.CgFExtD) Then
                e.Effect = DragDropEffects.Copy
                Exit Sub
            End If
        End If
        e.Effect = DragDropEffects.None
    End Sub
    Sub TvwExploreDragDrop(sender As Object, e As DragEventArgs)
    Dim VpStr As String = e.Data.GetData(DataFormats.FileDrop)(0)
        If VpStr.EndsWith(mdlConstGlob.CgFExtN) Or VpStr.EndsWith(mdlConstGlob.CgFExtO) Then
            Call Me.MnuExportActivate(sender, Nothing)
            If Not VmMyChildren.DoesntExist(VmMyChildren.ImporterExporter) Then
                VmMyChildren.ImporterExporter.InitImport(VpStr)
            End If
        ElseIf VpStr.EndsWith(mdlConstGlob.CgFExtD) Then
            Call Me.DBOpenInit(VpStr)
        End If
    End Sub
    Sub MnuPlugResourcerClick(sender As Object, e As EventArgs)
        If File.Exists(VgOptions.VgSettings.Plugins + mdlConstGlob.CgMTGMWebResourcer) Then
            Process.Start(VgOptions.VgSettings.Plugins + mdlConstGlob.CgMTGMWebResourcer)
        Else
            Call mdlToolbox.ShowWarning(mdlConstGlob.CgErr6)
        End If
    End Sub
    Sub MnuPlugHTMLClick(sender As Object, e As EventArgs)
        If File.Exists(VgOptions.VgSettings.Plugins + mdlConstGlob.CgHTMLCollectionViewer) Then
            Process.Start(VgOptions.VgSettings.Plugins + mdlConstGlob.CgHTMLCollectionViewer)
        Else
            Call mdlToolbox.ShowWarning(mdlConstGlob.CgErr6)
        End If
    End Sub
    Sub MnuCollapseRareteClick(sender As Object, e As EventArgs)
        Call Me.FixRarete
        Call mdlToolbox.ShowInformation("Terminé !")
    End Sub
    Sub BtCriteriaClick(sender As Object, e As EventArgs)
        VmFilterCriteria.Show
        VmFilterCriteria.Location = MousePosition
    End Sub
    Sub PicScanCardMouseUp(sender As Object, e As MouseEventArgs)
        If e.Button = MouseButtons.Left Then
            Call Me.MnuTransformClick(sender, e)
        End If
    End Sub
    Sub MainFormResizeEnd(sender As Object, e As EventArgs)
        Me.splitH3.SplitterDistance = 42 * Me.CreateGraphics().DpiX / 96        'Astuce horrible pour contourner un bug de mise à l'échelle automatique en fonction de la densité de pixels
    End Sub
    Sub MainFormResize(sender As Object, e As EventArgs)
        If Control.MouseButtons = MouseButtons.None Then
            Me.splitH3.SplitterDistance = 42 * Me.CreateGraphics().DpiX / 96    'Astuce horrible pour contourner un bug de mise à l'échelle automatique en fonction de la densité de pixels
        End If
    End Sub
    Sub SplitH3Resize(sender As Object, e As EventArgs)
        Me.splitH3.SplitterDistance = 42 * Me.CreateGraphics().DpiX / 96        'Astuce horrible pour contourner un bug de mise à l'échelle automatique en fonction de la densité de pixels
    End Sub
    Sub CmnuCbarClosed(sender As Object, e As ToolStripDropDownClosedEventArgs)
        Me.btHistPrices.Checked = False
    End Sub
    #End Region
End Class
