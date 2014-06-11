Imports System.Data
Imports System.Data.OleDb
Imports System.Web.Script.Serialization
Imports System.IO
Imports System.Reflection
Imports ICSharpCode.SharpZipLib.Zip
Public Partial Class MainForm
	Private Const CmStrConn As String		= "Provider=Microsoft.Jet.OLEDB.4.0;OLE DB Services=-1;Data Source="
	Private Const CmTemp As String			= "\mtgmgr"
	Private Const CmZipRes As String		= "\CollectionViewer.zip"	
	Private Const CmCollection As String	= "Collection"	
	Private VmDB As OleDbConnection
	Private VmDBCommand As New OleDbCommand
	Private VmDBReader As OleDbDataReader
	Private VmSerializer As New JavaScriptSerializer
	Public Sub New()
		Me.InitializeComponent()
	End Sub
	Private Sub ShowWarning(VpStr As String)
		MessageBox.Show(VpStr, "Problème", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1)
	End Sub	
	Private Sub LoadDecks
		Me.chklstDecksDispos.Items.Clear
		Me.chklstDecksDispos.Items.Add(CmCollection)
		VmDBCommand.CommandText = "Select GameName From MyGamesID Where IsFolder = False;"
		VmDBReader = VmDBCommand.ExecuteReader
		With VmDBReader
			While .Read
				Me.chklstDecksDispos.Items.Add(.GetString(0))
			End While
			.Close
		End With
	End Sub
	Private Function MatchColor(VpColor As String) As String
		Select Case VpColor.ToUpper
			Case "W"
				Return "White"
			Case "U"
				Return "Blue"
			Case "R"
				Return "Red"
			Case "G"
				Return "Green"
			Case "B"
				Return "Black"
			Case "M"
				Return "Multicolor"
			Case "A"
				Return "Colorless"
			Case Else
				Return VpColor
		End Select
	End Function
	Private Sub JSONExport
	Dim VpContent As New List(Of clsCardInfos)
	Dim VpCur As clsCardInfos
	Dim VpJSON As String
	Dim VpOut As StreamWriter
		For Each VpDeck As String In Me.chklstDecksDispos.CheckedItems
			If VpDeck = CmCollection Then
				VmDBCommand.CommandText = "Select * From ((((((MyCollection Inner Join Card On MyCollection.EncNbr = Card.EncNbr) Inner Join Spell On Card.Title = Spell.Title) Inner Join Series On Card.Series = Series.SeriesCD) Inner Join CardFR On Card.EncNbr = CardFR.EncNbr) Inner Join TextesFR On TextesFR.CardName = Card.Title) Left Join Creature On Card.Title = Creature.Title) Left Join SubTypes On Card.SubType = SubTypes.SubTypeVO;"
			Else
				VmDBCommand.CommandText = "Select * From (((((((MyGames Inner Join MyGamesID On MyGames.GameID = MyGamesID.GameID) Inner Join Card On MyGames.EncNbr = Card.EncNbr) Inner Join Spell On Card.Title = Spell.Title) Inner Join Series On Card.Series = Series.SeriesCD) Inner Join CardFR On Card.EncNbr = CardFR.EncNbr) Inner Join TextesFR On TextesFR.CardName = Card.Title) Left Join Creature On Card.Title = Creature.Title) Left Join SubTypes On Card.SubType = SubTypes.SubTypeVO Where MyGamesID.GameName = '" + VpDeck.Replace("'", "''") + "';"				
			End If
			VmDBReader = VmDBCommand.ExecuteReader
			With VmDBReader
				While .Read
					VpCur = New clsCardInfos
					VpCur.Color = Me.MatchColor(.GetValue(.GetOrdinal("Color")).ToString)
					VpCur.Cost = .GetValue(.GetOrdinal("Cost")).ToString
					VpCur.EncNbr = CLng(.GetValue(.GetOrdinal("Card.EncNbr")))
					VpCur.Items = CInt(.GetValue(.GetOrdinal("Items")))
					VpCur.Power = .GetValue(.GetOrdinal("Power")).ToString
					VpCur.Price = .GetValue(.GetOrdinal("Price"))
					VpCur.Rarity = .GetValue(.GetOrdinal("Rarity")).ToString
					VpCur.Series = .GetValue(.GetOrdinal("SeriesCD")).ToString
					VpCur.SeriesNM_FR = .GetValue(.GetOrdinal("SeriesNM_FR")).ToString
					VpCur.SeriesNM_MtG = .GetValue(.GetOrdinal("SeriesNM_MtG")).ToString
					VpCur.SubTypeVF =.GetValue(.GetOrdinal("SubTypeVF")).ToString
					VpCur.TexteFR = .GetValue(.GetOrdinal("TexteFR")).ToString.Trim
					VpCur.Title = .GetString(.GetOrdinal("Card.Title"))
					VpCur.TitleFR = .GetString(.GetOrdinal("TitleFR"))
					VpCur.Tough = .GetValue(.GetOrdinal("Tough")).ToString
					VpContent.Add(VpCur)
				End While
				.Close
			End With
			VpJSON = VmSerializer.Serialize(VpContent)
			VpOut = New StreamWriter(Me.dlgBrowser.SelectedPath + "\data\collection.json")
			VpOut.Write(VpJSON)
			VpOut.Flush
			VpOut.Close
		Next VpDeck	
	End Sub
	Private Sub CopyStream(VpIn As Stream, VpOut As Stream)
	Dim VpBuffer() As Byte = New Byte(8 * 1024 - 1) {}
	Dim VpLen As Integer
		Do
			VpLen = VpIn.Read(VpBuffer, 0, VpBuffer.Length)
			If VpLen > 0 Then
				VpOut.Write(VpBuffer, 0, VpLen)	
			Else
				Exit Do
			End If
		Loop
		VpOut.Flush
		VpOut.Close
	End Sub
	Sub MnuExitClick(sender As Object, e As EventArgs)
		Application.Exit
	End Sub
	Sub MnuDBOpenClick(sender As Object, e As EventArgs)
		Me.dlgOpen.FileName = ""
		Me.dlgOpen.ShowDialog
		If Me.dlgOpen.FileName <> "" Then
			Try
				VmDB = New OleDbConnection(CmStrConn + Me.dlgOpen.FileName)
		    	VmDB.Open
		    	VmDBCommand.Connection = VmDB
		    	VmDBCommand.CommandType = CommandType.Text
		    	Call Me.LoadDecks
			Catch
			End Try
	    End If
	End Sub
	Sub MnuJSONExportClick(sender As Object, e As EventArgs)
		If VmDB IsNot Nothing Then
			If Me.chklstDecksDispos.CheckedItems.Count > 0 Then
				Me.dlgBrowser.SelectedPath = ""
				Me.dlgBrowser.ShowDialog
				If Me.dlgBrowser.SelectedPath <> "" Then
					Call Me.JSONExport
					MessageBox.Show("Exportation terminée.", "Information", MessageBoxbuttons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
					Me.Close
				End If
			End If
		Else
			Call ShowWarning("Aucune base de données n'a été sélectionnée...")
		End If
	End Sub
	Sub MnuHTMLExportClick(sender As Object, e As EventArgs)
	Dim VpMyHTML As Stream = Assembly.GetExecutingAssembly.GetManifestResourceStream("MyHTML")
	Dim VpZipStream As ZipInputStream	
	Dim VpZipEntry As ZipEntry
		If VmDB IsNot Nothing Then
			Me.dlgBrowser.SelectedPath = ""
			Me.dlgBrowser.ShowDialog
			If Me.dlgBrowser.SelectedPath <> "" Then
				If Not Directory.Exists(Path.GetTempPath + CmTemp) Then
					Directory.CreateDirectory(Path.GetTempPath + CmTemp)
				End If
				'Extrait le fichier ZIP des ressources dans le répertoire temporaire
				Using VpFile As Stream = File.OpenWrite(Path.GetTempPath + CmTemp + CmZipRes)
					Call Me.CopyStream(VpMyHTML, VpFile)
				End Using	
				'Puis le décompresse dans le répertoire final
				VpZipStream = New ZipInputStream(File.OpenRead(Path.GetTempPath + CmTemp + CmZipRes))
				Do
					VpZipEntry = VpZipStream.GetNextEntry
					If VpZipEntry IsNot Nothing Then
						If VpZipEntry.IsFile Then
							If VpZipEntry.Name <> "" Then
								Using VpFile As Stream = File.OpenWrite(Me.dlgBrowser.SelectedPath + "\" + VpZipEntry.Name)
									Call Me.CopyStream(VpZipStream, VpFile)
								End Using									
							End If
						ElseIf VpZipEntry.IsDirectory Then
							If Not Directory.Exists(Me.dlgBrowser.SelectedPath + "\" + VpZipEntry.Name) Then
								Directory.CreateDirectory(Me.dlgBrowser.SelectedPath + "\" + VpZipEntry.Name)
							End If
						End If
					Else
						Exit Do
					End If
				Loop
				VpZipStream.Close
				'Génère pour finir le fichier de contenu
				Cursor.Current = Cursors.WaitCursor
				Call Me.JSONExport
				'Affichage de la page HTML
				Process.Start(Me.dlgBrowser.SelectedPath + "\index.html")
			End If
		Else
			Call ShowWarning("Aucune base de données n'a été sélectionnée...")
		End If
	End Sub	
	Sub MnuAboutClick(sender As Object, e As EventArgs)
	Dim VpAbout As New About
		VpAbout.ShowDialog		
	End Sub
End Class
<Serializable> _
Public Class clsCardInfos
	Public EncNbr As Long
	Public Title As String
	Public TitleFR As String
	Public SubTypeVF As String
	Public TexteFR As String
	Public Rarity As String
	Public Series As String
	Public SeriesNM_MtG As String
	Public SeriesNM_FR As String
	Public Items As Integer
	Public Cost As String
	Public Color As String
	Public Power As String
	Public Tough As String
	Public Price As Single
End Class