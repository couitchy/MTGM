'----------------------------------------------------------
'| Projet         | Magic The Gathering Manager - Updater |
'| Contexte       |  		Perso                         |
'| Date           |                            30/03/2008 |
'| Release 1      |                            12/04/2008 |
'| Release 2      |                    		   30/08/2008 |
'| Release 3      |                        	   08/11/2008 |
'| Auteur         |                              Couitchy |
'|--------------------------------------------------------|
'| Modifications :                                        |
'----------------------------------------------------------
Imports System.Diagnostics
Imports System.IO
Module Program
	Sub Main()	
	Dim VpStr As String
	Dim VpPath As String = Process.GetCurrentProcess().MainModule.FileName.Replace("Updater.exe", "")
		'Commence par terminer l'application Magic The Gathering Manager.exe si elle est lancée
		For Each VpProcess As Process In Process.GetProcesses
			Try
				VpStr = VpProcess.MainModule.FileName
				If VpStr.IndexOf("Magic The Gathering Manager.exe") >= 0 Then
					VpProcess.Kill
					Try
						Do
							Console.WriteLine("Waiting thread to terminate...")
						Loop While Not VpProcess.HasExited
					Catch
						Console.WriteLine("Thread seems to be terminated...")
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
			End If
		Catch
			Console.WriteLine("Unable to update file (are you logged as Administrator ?)." + vbCrLf + "Older version will be restarted...")
			Console.ReadKey
		End Try
		Process.Start(New ProcessStartInfo(VpPath + "Magic The Gathering Manager.exe"))
	End Sub
End Module
