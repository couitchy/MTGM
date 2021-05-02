Imports System.IO
Public Module mdlMain
    Public Sub Main(ByVal VpArgs() As String)
    '-------------------------------
    'Point d'entrée de l'application
    '-------------------------------
    Dim VpStartup As String = ""
        'Gestion globale des exceptions
        AddHandler Application.ThreadException, AddressOf ThreadExceptionHandler
        AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf DomainExceptionHandler
        'Gestion répertoire virtuel UAC (Vista / 7)
        CgVirtualPath = Process.GetCurrentProcess.MainModule.FileName.Replace("Magic The Gathering Manager.exe", "")
        CgVirtualPath = Environment.GetEnvironmentVariable("USERPROFILE") + "\AppData\Local\VirtualStore" + CgVirtualPath.Substring(2)
        'Exécution du formulaire de démarrage
        If Not PreventMultipleInstances Then
            Application.EnableVisualStyles
            Application.Run(New MainForm(VpArgs))
        Else
            Application.Exit
        End If
    End Sub
    Private Function PreventMultipleInstances As Boolean
    '---------------------------------------------------------------------------------------------------
    'Vérifie si une instance de MTGM est déjà en cours d'exécution, auquel cas l'affiche au premier plan
    '---------------------------------------------------------------------------------------------------
    Dim VmHandle As Long
    Dim VmProcesses() As Process
        VmProcesses = Process.GetProcessesByName(Process.GetCurrentProcess.ProcessName)
        For Each VpProcess As Process In VmProcesses
            If VpProcess.Id <> Process.GetCurrentProcess.Id Then
                VmHandle = VpProcess.MainWindowHandle.ToInt32
                Call OpenIcon(VmHandle)
                Call SetForegroundWindow(VmHandle)
                Return True
            End If
        Next VpProcess
        Return False
    End Function
    Private Sub ThreadExceptionHandler(sender As Object, ByVal e As Threading.ThreadExceptionEventArgs)
    Dim VpExceptionBox As New frmExceptionBox(e.Exception.ToString)
        VpExceptionBox.ShowDialog
    End Sub
    Private Sub DomainExceptionHandler(sender As Object, ByVal e As UnhandledExceptionEventArgs)
    Dim VpExceptionBox As New frmExceptionBox(e.ExceptionObject.ToString)
        VpExceptionBox.ShowDialog
    End Sub
    Public Function CheckIntegrity As Boolean
    '------------------------------------
    'Vérifie l'intégrité de l'application
    '------------------------------------
        For Each VpFile As String In CgRequiredFiles
            'Si le fichier n'existe pas
            If Not File.Exists(Application.StartupPath + VpFile) Then
                'Essaie de le télécharger
                Call DownloadNow(New Uri(VgOptions.VgSettings.DownloadServer + CgURL8 + VpFile.Replace("\", "")), VpFile)
                'Si le fichier n'existe toujours pas, on ne démarre pas
                If Not File.Exists(Application.StartupPath + VpFile) Then
                    Call ShowWarning(mdlConstGlob.CgErr0)
                    Return False
                End If
            End If
        Next VpFile
        Return True
    End Function
    Public Function LoadIcons(VpImgSeries As ImageList) As Boolean
    '----------------------------------------------------------
    'Charge en mémoire les icônes / ressources de l'application
    '----------------------------------------------------------
    Dim VpHandle As Image
    Dim VpKey As String
        If Not System.IO.Directory.Exists(Application.StartupPath + CgIcons) Then
            Call ShowWarning("Impossible de trouver le répertoire des ressources...")
            Return False
        Else
            VgImgSeries.ColorDepth = ColorDepth.Depth32Bit
            VgImgSeries.ImageSize = New Size(21, 21)
            VgImgSeries.TransparentColor = System.Drawing.Color.Transparent
            For Each VpIcon As String In System.IO.Directory.GetFiles(Application.StartupPath + CgIcons, "*" + CgIconsExt)
                If File.Exists(VpIcon) Then
                    Try
                        VpHandle = Image.FromFile(VpIcon)
                        VpKey = VpIcon.Substring(VpIcon.LastIndexOf("\") + 1)
                        If Not VgImgSeries.Images.Keys.Contains(VpKey) Then
                            VgImgSeries.Images.Add(VpKey, VpHandle)
                        End If
                        If Not VpImgSeries.Images.Keys.Contains(VpKey) Then
                            VpImgSeries.Images.Add(VpKey, VpHandle)
                        End If
                    Catch
                    End Try
                End If
            Next VpIcon
        End If
        Return True
    End Function
End Module
