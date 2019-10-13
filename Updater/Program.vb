'----------------------------------------------------------
'| Projet         | Magic The Gathering Manager - Updater |
'| Contexte       |         Perso                         |
'| Date           |                            30/03/2008 |
'| Release 1      |                            12/04/2008 |
'| Release 2      |                            30/08/2008 |
'| Release 3      |                            08/11/2008 |
'| Release 4      |                            29/08/2009 |
'| Release 5      |                            21/03/2010 |
'| Release 6      |                            17/04/2010 |
'| Release 7      |                            29/07/2010 |
'| Release 8      |                            03/10/2010 |
'| Release 9      |                            05/02/2011 |
'| Release 10     |                            10/09/2011 |
'| Release 11     |                            24/01/2012 |
'| Release 12     |                            01/10/2012 |
'| Release 13     |                            09/05/2014 |
'| Release 14     |                            09/05/2015 |
'| Release 15     |                            15/01/2017 |
'| Auteur         |                              Couitchy |
'|--------------------------------------------------------|
'| Modifications :                                        |
'| - UAC-aware                                 24/12/2009 |
'----------------------------------------------------------
Imports System.Diagnostics
Imports System.IO
Module Program
    Sub Main()
    Dim VpStr As String
    Dim VpPath As String = Process.GetCurrentProcess.MainModule.FileName.Replace("Updater.exe", "")
    Dim VpVirtualPath As String = Environment.GetEnvironmentVariable("USERPROFILE") + "\AppData\Local\VirtualStore" + VpPath.Substring(2)
        'Commence par terminer l'application Magic The Gathering Manager.exe si elle est lancée
        For Each VpProcess As Process In Process.GetProcesses
            Try
                VpStr = VpProcess.MainModule.FileName
                If VpStr.IndexOf("Magic The Gathering Manager.exe") >= 0 Then
                    VpProcess.Kill
                    Try
                        Do
                            Console.WriteLine("Waiting for process to terminate...")
                        Loop While Not VpProcess.HasExited
                    Catch
                        Console.WriteLine("Process seems to be terminated...")
                    End Try
                End If
            Catch
            End Try
        Next VpProcess
        'S'il existe une mise à jour, détruit l'ancien exécutable et renomme la mise à jour en .exe
        Try
            If File.Exists(VpPath + "Magic The Gathering Manager.new") Then
                File.Delete(VpPath + "Magic The Gathering Manager.exe")
                File.Move(VpPath + "Magic The Gathering Manager.new", VpPath + "Magic The Gathering Manager.exe")
            ElseIf File.Exists(VpVirtualPath + "Magic The Gathering Manager.new") Then
                Console.WriteLine("UAC virtualization detected...")
                Console.WriteLine("Real path : " + VpPath)
                Console.WriteLine("Virtual path : " + VpVirtualPath)
                File.Delete(VpPath + "Magic The Gathering Manager.exe")
                File.Move(VpVirtualPath + "Magic The Gathering Manager.new", VpPath + "Magic The Gathering Manager.exe")
                'Ne redémarre pas l'application en cas d'élévation de privilèges
                Exit Sub
            End If
        Catch VpEx As Exception
            Console.WriteLine("Unable to update file (are you logged as Administrator ?)." + vbCrLf + "Older version will be restarted...")
            Console.WriteLine("Error details :")
            Console.WriteLine(VpEx.Message)
            Console.ReadKey
            'Ne redémarre pas l'application en cas d'élévation de privilèges
            If File.Exists(VpVirtualPath + "Magic The Gathering Manager.new") Then Exit Sub
        End Try
        Process.Start(New ProcessStartInfo(VpPath + "Magic The Gathering Manager.exe"))
    End Sub
End Module
