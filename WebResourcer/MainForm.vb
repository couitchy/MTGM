'---------------------------------------------------------------
'| Projet         | Magic The Gathering Manager - WebResourcer |
'| Contexte       |  		Perso       					   |
'| Date           |      							30/03/2008 |
'| Release 1      |   								12/04/2008 |
'| Release 2      |  								30/08/2008 |
'| Release 3      | 								08/11/2008 |
'| Release 4      |      							29/08/2009 |
'| Release 5      |       							21/03/2010 |
'| Release 6      |       							17/04/2010 |
'| Release 7      |									29/07/2010 |
'| Release 8      |       							03/10/2010 |
'| Release 9      |                      			05/02/2011 |
'| Release 10     |                           		10/09/2011 |
'| Release 11     |                        			24/01/2012 |
'| Release 12     |                        			01/10/2012 |
'| Release 13     |                        			09/05/2014 |
'| Auteur         |      							  Couitchy |
'|-------------------------------------------------------------|
'| Modifications :               							   |
'---------------------------------------------------------------
Imports System.Data
Imports System.Data.OleDb
Imports System.Net
Imports System.IO
Imports System.Text
Public Partial Class MainForm
	Private Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA"(lpApplicationName As String, lpKeyName As String, lpString As String, ByVal lpFileName As String) As Integer
	Private Const CmStrConn As String		= "Provider=Microsoft.Jet.OLEDB.4.0;OLE DB Services=-1;Data Source="
	Private Const CmURL0 As String 			= "http://magic-ville.fr/fr/"
	Private Const CmURL1 As String 			= "http://www.magiccorporation.com/mc.php?rub=cartes&op=search&word=#cardname#&search=2"
	Private Const CmURL2 As String  		= "http://www.magiccorporation.com/gathering-cartes-view"
	Private Const CmURL3 As String  		= "http://www.magiccorporation.com/scan/"
	Private Const CmURL4 As String  		= "http://www.magiccorporation.com"
	Private Const CmURL5 As String  		= "http://magiccards.info/###/^^.html"
	Private Const CmURL6 As String  		= "http://magiccards.info/query?q=%2B%2Be%3A###%2Fen&v=spoiler&s=issue"
	Private Const CmId As String  			= "#cardname#"
	Private Const CmKey0 As String 			= "recherche_titre"
	Private Const CmKey1 As String 			= "gathering-cartes-view"
	Private Const CmKey2 As String  		= "NM/MT"
	Private Const CmKey2A As String  		= "Nm</td><td>VF"
	Private Const CmKey2B As String  		= "Premium"
	Private Const CmKey2C As String  		= ">VO<"
	Private Const CmKey3 As String  		= "src=""/scan/"
	Private Const CmKey4 As String  		= "src=""http://www.wizards.com/global/images/magic"
	Private Const CmKey4B As String			= "href=""/images/cartes/illustrations"
	Private Const CmKey5 As String  		= "Texte Français"
	Private Const CmKey6 As String  		= "/###/^^/"
	Private Const CmKey6B As String  		= ".html"">"
	Private Const CmKey7 As String  		= "<td>"
	Private Const CmKey7B As String  		= "</td>"
	Private Const CmKey7C As String  		= "<img src=""http://magiccards.info/images/en.gif"" alt=""English"" width=""16"" height=""11"" class=""flag2""> "
	Private Const CmKey8 As String  		= "<p"
	Private Const CmKey8B As String  		= "<b>"
	Private Const CmKey8C As String  		= "</b></p>"
	Private Const CmKey8D As String  		= "<br><br>"
	Private Const CmKey8E As String  		= ", <i>"
	Private Const CmKey8F As String  		= "</i></p>"
	Private Const CmFrench  As Integer 		= 2
	Private Const CmMe As String			= "Moi"
	Private Const CmStamp As String			= "ContenuStamp r14.txt"
	Private Const CmCategory As String		= "Properties"
	Private CmFields() As String 			= {"LastUpdateAut", "LastUpdateSimu", "LastUpdateTxtVF", "LastUpdateTradPatch"}
	Private CmIndexes() As Integer 			= {2, 3, 4, 7}
	Private CmIgnoredEditions() As String	= {"Premium Deck Seri...", "Archenemy Decks", "Duels of the Plan...", "Battle Royale", "Anthologies", "Promotional Card", "Friday Night Magic", "Prerelease Cards", "Magic Player Rewards"}
	Private VmDB As OleDbConnection
	Private VmDBCommand As New OleDbCommand
	Private VmDBReader As OleDbDataReader
	Private VmStart As Date
	Private VmIsComplete As Boolean = False
	Private Enum eLogType
		Information = 0
		Warning
		MyError
	End Enum
	Public Sub New()
		Me.InitializeComponent()
		Me.wbMV.ScriptErrorsSuppressed = True
	End Sub
	Private Sub AddToLog(VpText As String, VpType As eLogType, Optional VpNewOp As Boolean = False, Optional VpEndOp As Boolean = False)
	'-----------------------------
	'Ajout d'une entrée au journal
	'-----------------------------
	Dim VpLog As ListViewItem
		If VpNewOp Then
			VmStart = Now
			Me.btCancel.Enabled = True
			Me.btCancel.Tag = False
		ElseIf VpEndOp Then
			Me.btCancel.Tag = False
			Me.btCancel.Enabled = False
			Me.tabMain.SelectedIndex = 0
			Me.prgAvance.Style = ProgressBarStyle.Blocks
			Me.prgAvance.Value = 0
		End If
		VpLog = New ListViewItem(Now.ToLongTimeString, CInt(VpType))
		VpLog.SubItems.Add(VpText)
		Me.lvwLog.Items.Add(VpLog)
		VpLog.EnsureVisible
	End Sub
	Private Sub ETA
	'-------------------------------------------------------
	'Estimation du temps restant pour le traitement en cours
	'-------------------------------------------------------
	Dim VpETA As TimeSpan
		If Me.prgAvance.Value > 0 Then
			VpETA = New TimeSpan(0, 0, Now.Subtract(VmStart).TotalSeconds * (Me.prgAvance.Maximum / Me.prgAvance.Value - 1))
			Me.txtETA.Text = Format(VpETA.Hours, "00") + ":" + Format(VpETA.Minutes, "00") + ":" + Format(VpETA.Seconds, "00")
		End If
	End Sub
	Private Function HTMLfromRequest(VpURL As String) As String
	'------------------------------------------------------------------
	'Récupère le code HTML en réponse de la requête passée en paramètre
	'------------------------------------------------------------------
	Dim VpRequest As HttpWebRequest
	Dim VpAnswer As Stream
	Dim VpCurByte As Integer
	Dim VpStr As String = ""
		VpRequest = WebRequest.Create(VpURL)
		VpAnswer = VpRequest.GetResponse().GetResponseStream()
		VpCurByte = VpAnswer.ReadByte
		While VpCurByte <> -1
			VpStr = VpStr + (Encoding.Default.GetString(New Byte() {VpCurByte}))
			VpCurByte = VpAnswer.ReadByte
			Application.DoEvents
		End While
		Return VpStr
	End Function
	Private Function GetPrice(VpIn As String) As String
	'---------------------------------------------------
	'Retourne les prix pour la carte passée en paramètre
	'---------------------------------------------------
	Dim VpStr As String = ""
	Dim VpStrFoil As String
	Dim VpStrPlane As String
	Dim VpStrs() As String
	Dim VpPrices As String = ""
	Dim VpIr As Integer
	Dim VpVoid As String
	Dim VpIn2 As String = VpIn.Replace(" ", "+").Replace("""", "").Replace("û", "u").Replace("á", "a").Replace("â", "a")
		Try
			If VpIn2.StartsWith("Æ") Then
				VpIn2 = VpIn2.Substring(1)
			ElseIf VpIn2.Contains("Æ") Then
				VpVoid = VpIn2.Substring(0, VpIn2.IndexOf("Æ") + 1)
				VpIn2 = VpIn2.Replace(VpVoid, "")
			End If
			VpStr = Me.HTMLfromRequest(CmURL1.Replace(CmId, VpIn2))
			VpStrs = VpStr.Split(New String() {CmKey1}, StringSplitOptions.None)
			VpIr = Me.FindRightIndex(VpStrs, VpIn.Replace("û", "u"))
			VpStr = CmURL2 + VpStrs(VpIr).Substring(0, VpStrs(VpIr).IndexOf(""""))
			VpStr = Me.HTMLfromRequest(VpStr)
			VpStrFoil = VpStr
			VpStrPlane = VpStr
			VpStrs = VpStr.Split(New String() {CmKey2}, StringSplitOptions.None)
			VpStr = VpStrs(0).Substring(VpStrs(0).LastIndexOf("""") + 2)
			VpStr = VpStr.Substring(0, VpStr.IndexOf("<"))
			VpPrices = VpStr
			For VpI As Integer = 1 To VpStrs.Length - 1
				VpStr = VpStrs(VpI).Substring(0, VpStrs(VpI).IndexOf("€") + 1)
				VpStr = VpStr.Substring(VpStr.LastIndexOf(">") + 1)
				VpPrices = VpPrices + "^" + VpStr
				VpStr = VpStrs(VpI).Substring(VpStrs(VpI).LastIndexOf("""") + 2)
				VpStr = VpStr.Substring(0, VpStr.IndexOf("<"))
				VpPrices = VpPrices + "#" + VpStr
			Next VpI
			VpStrs = VpStrPlane.Split(New String() {CmKey2A}, StringSplitOptions.None)
			VpStr = VpStrs(0).Substring(VpStrs(0).LastIndexOf("""") + 2)
			VpStr = VpStr.Substring(0, VpStr.IndexOf("<"))
			If VpStrs.Length > 1 Then
				VpPrices = VpPrices + VpStr
			End If
			For VpI As Integer = 1 To VpStrs.Length - 1
				VpStr = VpStrs(VpI).Substring(0, VpStrs(VpI).IndexOf("€") + 1)
				VpStr = VpStr.Substring(VpStr.LastIndexOf(">") + 1)
				VpPrices = VpPrices + "^" + VpStr
				VpStr = VpStrs(VpI).Substring(VpStrs(VpI).LastIndexOf("""") + 2)
				VpStr = VpStr.Substring(0, VpStr.IndexOf("<"))
				VpPrices = VpPrices + "#" + VpStr
			Next VpI
			VpStrs = VpStrFoil.Split(New String() {CmKey2B}, StringSplitOptions.None)
			VpStr = VpStrs(0).Substring(VpStrs(0).LastIndexOf("""") + 2)
			VpStr = VpStr.Substring(0, VpStr.IndexOf("<"))
			If VpStrs.Length > 1 AndAlso VpStrs(1).ToUpper.Contains(CmKey2C) Then
				VpPrices = VpPrices + VpStr
			End If
			For VpI As Integer = 1 To VpStrs.Length - 1
				If VpStrs(VpI).ToUpper.Contains(CmKey2C) Then
					VpStr = VpStrs(VpI).Substring(0, VpStrs(VpI).IndexOf("€") + 1)
					VpStr = VpStr.Substring(VpStr.LastIndexOf(">") + 1)
					VpPrices = VpPrices + "PREMIUMFOILVO^" + VpStr
					VpStr = VpStrs(VpI).Substring(VpStrs(VpI).LastIndexOf("""") + 2)
					VpStr = VpStr.Substring(0, VpStr.IndexOf("<"))
				End If
				If VpStrs(VpI + 1).ToUpper.Contains(CmKey2C) Then
					VpPrices = VpPrices + "#" + VpStr
				End If
			Next VpI
		Catch
		End Try
		VpPrices = VpPrices.Replace("Planechase 2012 D...^", "Planechase 2012^")		'très crade mais c'est de la faute du site magiccorporation
		VpPrices = VpPrices.Trim
		If VpPrices.EndsWith("#") Then
			Return VpPrices
		Else
			Return VpPrices + "#"
		End If
	End Function
	Private Function FindRightIndex(VpT() As String, VpIn As String) As Integer
	Dim VpA As String = VpIn.ToLower.Trim
	Dim VpB As String = ""
		For VpI As Integer = 0 To VpT.Length - 1
			Try
				VpB = VpT(VpI).Substring(VpT(VpI).IndexOf(">") + 1)
				VpB = VpB.Substring(0, VpB.IndexOf("<"))
			Catch
			End Try
			If VpB.ToLower.Trim = VpA Then
				Return VpI
			End If
		Next VpI
		Return 1
	End Function
	Private Function BuildListeFromDB(Optional VpStartAfter As String = "") As List(Of String)
	Dim VpCards As New List(Of String)
	Dim VpCanAdd As Boolean = ( VpStartAfter = "" )
		Me.prgAvance.Style = ProgressBarStyle.Marquee
		VmDBCommand.CommandText = "Select Distinct Title From Card Order By Title Asc;"
		VmDBReader = VmDBCommand.ExecuteReader
		With VmDBReader
			While .Read
				If VpCanAdd Then
					VpCards.Add(.GetString(0))
				Else
					VpCanAdd = ( .GetString(0) = VpStartAfter )
				End If
				Application.DoEvents
			End While
			.Close
		End With
		Return VpCards
	End Function
	Private Function BuildListeFromFile(Optional VpCut As Boolean = False) As List(Of String)
	Dim VpCards As New StreamReader(Me.dlgOpen4.FileName)
	Dim VpListe As New List(Of String)
	Dim VpStr As String
		Me.prgAvance.Style = ProgressBarStyle.Marquee
		While Not VpCards.EndOfStream
			If Not VpCut Then
				VpListe.Add(VpCards.ReadLine)
			Else
				VpStr = VpCards.ReadLine
				If VpStr.Contains("#") Then
					VpListe.Add(VpStr.Split("#")(0))
				End If
			End If
			Application.DoEvents
		End While
		VpCards.Close
		Return VpListe
	End Function
	Private Sub UpdatePrices(VpAll As Boolean)
	'------------------------------------------------
	'Mise à jour de la liste des prix depuis Internet
	'------------------------------------------------
	Dim VpCards As List(Of String)
	Dim VpOut As StreamWriter
	Dim VpPrices As String
	Dim VpAppend As Boolean
	Dim VpAlready() As String
	Dim VpLast As String = ""
		Me.dlgSave.FileName = ""
		Me.dlgSave.ShowDialog
		If Me.dlgSave.FileName <> "" Then
			VpAppend = File.Exists(Me.dlgSave.FileName)
			If VpAppend Then
				'Si le fichier existe déjà, regarde la dernière carte qui a été traitée
				VpAlready = File.ReadAllLines(Me.dlgSave.FileName)
				If VpAlready.Length > 2 Then
					If VpAlready(VpAlready.Length - 1).Contains("#") Then
						VpLast = VpAlready(VpAlready.Length - 1)
					Else
						VpLast = VpAlready(VpAlready.Length - 2)
					End If
					VpLast = VpLast.Substring(0, VpLast.IndexOf("#"))
				Else
					VpAppend = False
				End If
			End If
			VpOut = New StreamWriter(Me.dlgSave.FileName, VpAppend)
			If VpAppend Then
				Call Me.AddToLog("La récupération des prix se poursuit...", eLogType.Information, True)
			Else
				Call Me.AddToLog("La récupération des prix a commencé...", eLogType.Information, True)
				'Inscription de la date
				VpOut.WriteLine(Now.ToShortDateString)
			End If
			'Récupère la liste des cartes
			If VpAll Then
				VpCards = Me.BuildListeFromDB(VpLast)
			Else
				Me.dlgOpen4.FileName = ""
				Me.dlgOpen4.ShowDialog
				If Me.dlgOpen4.FileName <> "" Then
					VpCards = Me.BuildListeFromFile
				Else
					VpOut.Close
					Exit Sub
				End If
			End If
			'Récupère le prix pour chaque carte
			Me.prgAvance.Maximum = VpCards.Count
			Me.prgAvance.Value = 0
			Me.prgAvance.Style = ProgressBarStyle.Blocks
			For Each VpCard As String In VpCards
				If Not VpCard.StartsWith("_") Then
					Me.txtCur.Text = VpCard
					Application.DoEvents
					VpPrices = Me.GetPrice(VpCard)
					If VpPrices = "#" Then
						Call Me.AddToLog("Impossible de récupérer les prix pour la carte : " + VpCard, eLogType.Warning)
					Else
						VpOut.WriteLine(VpCard + "#" + VpPrices)
					End If
				End If
				Me.prgAvance.Increment(1)
				Call Me.ETA
				If Me.btCancel.Tag Then Exit For
			Next VpCard
			VpOut.Flush
			VpOut.Close
			If Me.btCancel.Tag Then
				Call Me.AddToLog("La récupération des prix a été annulée.", eLogType.Warning, , True)
			Else
				Call Me.AddToLog("La récupération des prix est terminée.", eLogType.Information, , True)
			End If
		End If
	End Sub
	Private Sub FillPricesHistory
	'----------------------------------------------------------------------------------------------------------------------------------------------------------------------
	'Actualisation de la table de l'historique des prix : si un prix a changé depuis son dernier enregistrement, ajoute une entrée avec la nouvelle date et le nouveau prix
	'----------------------------------------------------------------------------------------------------------------------------------------------------------------------
	Dim VpPrices As StreamReader
	Dim VpCardData() As String
	Dim VpPrice As String
	Dim VpEdition As String
	Dim VpEditionsOK As New List(Of String)
	Dim VpCardName As String
	Dim VpDate As String
	Dim VpEncNbr As Long
	Dim VpO As Object
	Dim VpFoilTarget As Boolean = False
	 	Me.dlgOpen2.FileName = ""
		Me.dlgOpen2.ShowDialog
		If Me.dlgOpen2.FileName <> "" Then
	    	VpPrices = New StreamReader(Me.dlgOpen2.FileName)
	    	Call Me.AddToLog("La mise à jour de l'historique des prix a commencé...", eLogType.Information, True)
	    	Me.prgAvance.Style = ProgressBarStyle.Marquee
			VpDate = VpPrices.ReadLine
			If Not IsDate(VpDate) Then
				VpDate = File.GetLastWriteTimeUtc(Me.dlgOpen2.FileName).ToShortDateString
				VpPrices.BaseStream.Seek(0, SeekOrigin.Begin)
			End If
			VpDate = "'" + CDate(VpDate).Day.ToString + "/" + CDate(VpDate).Month.ToString + "/" + CDate(VpDate).Year.ToString.Substring(2, 2) + "'"
	    	While Not VpPrices.EndOfStream
	    		Application.DoEvents
				VpCardData = VpPrices.ReadLine.Split("#")
				VpCardName = ""
				VpEdition = ""
				VpEditionsOK.Clear
				VpPrice = ""
				For Each VpStr As String In VpCardData
					If VpStr.IndexOf("^") <> -1 Then
						VpEdition = VpStr.Substring(0, VpStr.IndexOf("^")).Replace("'", "''")
						If Not VpEditionsOK.Contains(VpEdition) Then	'fait en sorte de ne prendre que le premier prix par édition (correspondant à la qualité de carte NM/MT)
							VpEditionsOK.Add(VpEdition)
							VpPrice = VpStr.Substring(VpStr.IndexOf("^") + 1).Replace("€", "").Trim
							If VpEdition.EndsWith("PREMIUMFOILVO") Then
								VpEdition = VpEdition.Replace("PREMIUMFOILVO", "")
								VpFoilTarget = True
							Else
								VpFoilTarget = False
							End If
							VmDBCommand.CommandText = "Select EncNbr From Card Inner Join Series On Card.Series = Series.SeriesCD Where Series.SeriesNM_MtG = '" + VpEdition + "' And Card.Title = '" + VpCardName + "';"
							VpEncNbr = VmDBCommand.ExecuteScalar
							If VpEncNbr <> 0 Then
								If VpFoilTarget Then
									VmDBCommand.CommandText = "Select Price From PricesHistory Where EncNbr = " + VpEncNbr.ToString + " And Foil = True Order By PriceDate Desc;"
									VpO = VmDBCommand.ExecuteScalar
									If Not VpO Is Nothing Then
										If Format(VpO, "0.00").Replace(",", ".") <> VpPrice Then
											VmDBCommand.CommandText = "Insert Into PricesHistory(EncNbr, PriceDate, Price, Foil) Values (" + VpEncNbr.ToString + ", " + VpDate + ", " + VpPrice + ", True);"
											VmDBCommand.ExecuteNonQuery
											Call Me.AddToLog("Changement de prix pour la carte " + VpCardName + " - " + VpEdition + " (foil) : " + VpPrice + "€" , eLogType.Information)
										End If
									Else
										VmDBCommand.CommandText = "Insert Into PricesHistory(EncNbr, PriceDate, Price, Foil) Values (" + VpEncNbr.ToString + ", " + VpDate + ", " + VpPrice + ", True);"
										VmDBCommand.ExecuteNonQuery
										Call Me.AddToLog("Nouveau prix pour la carte " + VpCardName + " - " + VpEdition + " (foil) : " + VpPrice + "€" , eLogType.Information)
									End If
								Else
									VmDBCommand.CommandText = "Select Price From PricesHistory Where EncNbr = " + VpEncNbr.ToString + " And Foil = False Order By PriceDate Desc;"
									VpO = VmDBCommand.ExecuteScalar
									If Not VpO Is Nothing Then
										If Format(VpO, "0.00").Replace(",", ".") <> VpPrice Then
											VmDBCommand.CommandText = "Insert Into PricesHistory(EncNbr, PriceDate, Price, Foil) Values (" + VpEncNbr.ToString + ", " + VpDate + ", " + VpPrice + ", False);"
											VmDBCommand.ExecuteNonQuery
											Call Me.AddToLog("Changement de prix pour la carte " + VpCardName + " - " + VpEdition + " : " + VpPrice + "€" , eLogType.Information)
										End If
									Else
										VmDBCommand.CommandText = "Insert Into PricesHistory(EncNbr, PriceDate, Price, Foil) Values (" + VpEncNbr.ToString + ", " + VpDate + ", " + VpPrice + ", False);"
										VmDBCommand.ExecuteNonQuery
										Call Me.AddToLog("Nouveau prix pour la carte " + VpCardName + " - " + VpEdition + " : " + VpPrice + "€" , eLogType.Information)
									End If
								End If
							Else
								If Array.IndexOf(CmIgnoredEditions, VpEdition) < 0 Then
									Call Me.AddToLog("Pas de correspondance de prix trouvée pour la carte " + VpCardName + " - " + VpEdition, eLogType.Warning)
								End If
							End If
						End If
					Else
						VpCardName = VpStr.Replace("'", "''")
						Me.txtCur.Text = VpStr
					End If
				Next VpStr
				If Me.btCancel.Tag Then Exit While
			End While
			VpPrices.Close
			If Me.btCancel.Tag Then
				Call Me.AddToLog("La mise à jour de l'historique des prix a été annulée.", eLogType.Warning, , True)
			Else
				Call Me.AddToLog("La mise à jour de l'historique des prix est terminée.", eLogType.Information, , True)
			End If
		End If
	End Sub
	Private Sub FixTxtVO
	'----------------------------------------------------------------------------------------------------------------
	'Corrige les textes des cartes en anglais dont seule la première ligne aurait été prise depuis le fichier spoiler
	'----------------------------------------------------------------------------------------------------------------
	Dim VpCurSpoiler As StreamReader
	Dim VpCategory As Boolean = False
	Dim VpStr As String
	Dim VpCard As String = ""
	Dim VpSeriesCD As String = ""
	Dim VpNewTxt As String = ""
		Me.dlgBrowse.SelectedPath = ""
		Me.dlgBrowse.ShowDialog
		If Me.dlgBrowse.SelectedPath <> "" Then
			Call Me.AddToLog("La mise à jour des textes VO multilignes a commencé...", eLogType.Information, True)
			For Each VpFile As String In Directory.GetFiles(Me.dlgBrowse.SelectedPath, "*spoiler_en.txt")
				VpSeriesCD = VpFile.Substring(VpFile.LastIndexOf("\") + 1)
				VpSeriesCD = Me.SerieCode(VpSeriesCD.Substring(0, VpSeriesCD.IndexOf("_")))
				VpCurSpoiler = New StreamReader(VpFile, Encoding.Default)
				While Not VpCurSpoiler.EndOfStream
					Application.DoEvents
					VpStr = VpCurSpoiler.ReadLine
					If VpStr.StartsWith("CardName:") Or VpStr.StartsWith("Card Name:") Or VpStr.StartsWith("Name:") Then
						VpCard = VpStr.Substring(VpStr.IndexOf(":") + 1).Trim
					ElseIf VpStr.StartsWith("Rules Text:") Then
						VpCategory = True
						VpNewTxt = VpStr.Substring(11).Trim
					ElseIf VpStr.StartsWith("Set/Rarity:") Then
						VpCategory = False
						'Mise à jour du texte si nécessaire
						If VpNewTxt.Contains(vbCrLf) Then
							VmDBCommand.CommandText = "Update Card Set CardText = '" + VpNewTxt.Replace("'", "''") + "' Where Title = '" + VpCard.Replace("'", "''") + "' And Series = '" + VpSeriesCD + "';"
							VmDBCommand.ExecuteNonQuery
							Call Me.AddToLog("Nouveau texte VO pour la carte " + VpCard + " - " + VpSeriesCD, eLogType.Information)
						End If
						VpNewTxt = ""
						VpCard = ""
					ElseIf VpCategory Then
						VpNewTxt = VpNewTxt + vbCrLf + VpStr
					End If
					If Me.btCancel.Tag Then Exit While
				End While
				VpCurSpoiler.Close
			Next VpFile
			If Me.btCancel.Tag Then
				Call Me.AddToLog("La mise à jour des textes VO multilignes a été annulée.", eLogType.Warning, , True)
			Else
				Call Me.AddToLog("La mise à jour des textes VO multilignes est terminée.", eLogType.Information, , True)
			End If
		End If
	End Sub
	Private Sub FixPictures
	'------------------------------------------------------
	'Correction des images erronées dans la base de données
	'------------------------------------------------------
	Dim VpOut As FileStream
	Dim VpOutB As BinaryWriter
	Dim VpIn As StreamReader
	Dim VpInB As BinaryReader
	Dim VpFileInfo As FileInfo
	Dim VpName As String
	Dim VpOffset As Long
	Dim VpEnd As Long
		Me.dlgOpen3.FileName = ""
		Me.dlgOpen3.ShowDialog
		If Me.dlgOpen3.FileName <> "" Then
			Me.dlgBrowse.SelectedPath = ""
			Me.dlgBrowse.ShowDialog
			If Me.dlgBrowse.SelectedPath <> "" Then
				VpOut = New FileStream(Me.dlgOpen3.FileName, FileMode.OpenOrCreate)
				VpOutB = New BinaryWriter(VpOut)
				Call Me.AddToLog("La correction des images a commencé...", eLogType.Information, True)
				Me.prgAvance.Maximum = System.IO.Directory.GetFiles(Me.dlgBrowse.SelectedPath, "*.jpg").Length
				Me.prgAvance.Value = 0
				Me.prgAvance.Style = ProgressBarStyle.Blocks
		    	For Each VpImg As String In System.IO.Directory.GetFiles(Me.dlgBrowse.SelectedPath, "*.jpg")
		    		Application.DoEvents
		    		VpFileInfo = New FileInfo(VpImg)
		    		VpName = VpFileInfo.Name.Replace(".jpg", "")
		    		VmDBCommand.CommandText = "Select Offset From CardPictures Where Title = '" + VpName.Replace("'", "''") + "';"
		    		VpOffset = VmDBCommand.ExecuteScalar
		    		VmDBCommand.CommandText = "Select [End] From CardPictures Where Title = '" + VpName.Replace("'", "''") + "';"
		    		VpEnd = VmDBCommand.ExecuteScalar
		    		If VpEnd - VpOffset + 1 >= VpFileInfo.Length Then
						VpIn = New StreamReader(VpImg)
						VpInB = New BinaryReader(VpIn.BaseStream)
		    			VpOutB.Seek(VpOffset, SeekOrigin.Begin)
		    			VpOutB.Write(VpInB.ReadBytes(VpFileInfo.Length))
						VpIn.Close
						VmDBCommand.CommandText = "Update CardPictures Set [End] = " + (VpOffset + VpFileInfo.Length - 1).ToString + " Where Title = '" + VpName.Replace("'", "''") + "';"
						VmDBCommand.ExecuteNonQuery
						Call Me.AddToLog("Nouvelle image pour la carte " + VpName, eLogType.Information)
		    		Else
		    			Call Me.AddToLog("Place insuffisante pour la nouvelle image de la carte " + VpName, eLogType.Warning)
		    		End If
					Me.prgAvance.Increment(1)
					If Me.btCancel.Tag Then Exit For
		    	Next VpImg
				VpOutB.Flush
				VpOutB.Close
				If Me.btCancel.Tag Then
					Call Me.AddToLog("La correction des images a été annulée.", eLogType.Warning, , True)
				Else
					Call Me.AddToLog("La correction des images est terminée.", eLogType.Information, , True)
				End If
			End If
		End If
	End Sub
	Private Sub ReplaceTitle
	'--------------------------------------------------------------------------------------------------------------------
	'Correction d'un titre de carte erroné dans les tables Autorisations, Card, CardPictures, Creature, Spell et TextesFR
	'--------------------------------------------------------------------------------------------------------------------
	Dim VpOldTitle As String
	Dim VpNewTitle As String
	Dim VpO As Object
		Call Me.AddToLog("La mise à jour d'un nom de carte a commencé...", eLogType.Information, True)
		VpOldTitle = InputBox("Quel est le nom de la carte à remplacer ?", "Mise à jour de nom", "(carte)")
		If VpOldTitle <> "" Then
			VmDBCommand.CommandText = "Select Title From Card Where InStr(UCase(Title), '" + VpOldTitle.Replace("'", "''").ToUpper + "') > 0;"
			VpO = VmDBCommand.ExecuteScalar
			If Not VpO Is Nothing
				VpOldTitle = VpO.ToString
				If MessageBox.Show("Carte correspondante trouvée : " + VpOldTitle + vbCrLf + "Voulez-vous changer son nom ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = System.Windows.Forms.DialogResult.Yes Then
					VpNewTitle = InputBox("Quel est le nouveau nom pour cette carte ?", "Mise à jour de nom", VpOldTitle)
					If VpNewTitle <> "" Then
						VpOldTitle = VpOldTitle.Replace("'", "''")
						VpNewTitle = VpNewTitle.Replace("'", "''")
						'Autorisations
						Try
					    	VmDBCommand.CommandText = "Update Autorisations Set Title = '" + VpNewTitle + "' Where Title = '" + VpOldTitle + "';"
					    	VmDBCommand.ExecuteNonQuery
							Call Me.AddToLog(VpOldTitle + " remplacée par " + VpNewTitle + " dans la table Autorisations", eLogType.Information)
						Catch
					    	VmDBCommand.CommandText = "Delete * From Autorisations Where Title = '" + VpOldTitle + "';"
					    	VmDBCommand.ExecuteNonQuery
							Call Me.AddToLog(VpOldTitle + " supprimée dans la table Autorisations afin d'éviter un doublon", eLogType.Information)
						End Try
				    	'Card
				    	VmDBCommand.CommandText = "Update Card Set Title = '" + VpNewTitle + "' Where Title = '" + VpOldTitle + "';"
				    	VmDBCommand.ExecuteNonQuery
						Call Me.AddToLog(VpOldTitle + " remplacée par " + VpNewTitle + " dans la table Card", eLogType.Information)
				    	'CardPictures
				    	Try
					    	VmDBCommand.CommandText = "Update CardPictures Set Title = '" + VpNewTitle + "' Where Title = '" + VpOldTitle + "';"
					    	VmDBCommand.ExecuteNonQuery
							Call Me.AddToLog(VpOldTitle + " remplacée par " + VpNewTitle + " dans la table CardPictures", eLogType.Information)
						Catch
					    	VmDBCommand.CommandText = "Delete * From CardPictures Where Title = '" + VpOldTitle + "';"
					    	VmDBCommand.ExecuteNonQuery
							Call Me.AddToLog(VpOldTitle + " supprimée dans la table CardPictures afin d'éviter un doublon", eLogType.Information)
						End Try
				    	'Creature
				    	Try
					    	VmDBCommand.CommandText = "Update Creature Set Title = '" + VpNewTitle + "' Where Title = '" + VpOldTitle + "';"
					    	VmDBCommand.ExecuteNonQuery
							Call Me.AddToLog(VpOldTitle + " remplacée par " + VpNewTitle + " dans la table Creature", eLogType.Information)
						Catch
					    	VmDBCommand.CommandText = "Delete * From Creature Where Title = '" + VpOldTitle + "';"
					    	VmDBCommand.ExecuteNonQuery
							Call Me.AddToLog(VpOldTitle + " supprimée dans la table Creature afin d'éviter un doublon", eLogType.Information)
						End Try
				    	'Spell
				    	Try
					    	VmDBCommand.CommandText = "Update Spell Set Title = '" + VpNewTitle + "' Where Title = '" + VpOldTitle + "';"
					    	VmDBCommand.ExecuteNonQuery
							Call Me.AddToLog(VpOldTitle + " remplacée par " + VpNewTitle + " dans la table Spell", eLogType.Information)
						Catch
					    	VmDBCommand.CommandText = "Delete * From Spell Where Title = '" + VpOldTitle + "';"
					    	VmDBCommand.ExecuteNonQuery
							Call Me.AddToLog(VpOldTitle + " supprimée dans la table Spell afin d'éviter un doublon", eLogType.Information)
						End Try
				    	'TextesFR
				    	Try
					    	VmDBCommand.CommandText = "Update TextesFR Set CardName = '" + VpNewTitle + "' Where CardName = '" + VpOldTitle + "';"
					    	VmDBCommand.ExecuteNonQuery
							Call Me.AddToLog(VpOldTitle + " remplacée par " + VpNewTitle + " dans la table TextesFR", eLogType.Information)
						Catch
					    	VmDBCommand.CommandText = "Delete * From TextesFR Where CardName = '" + VpOldTitle + "';"
					    	VmDBCommand.ExecuteNonQuery
							Call Me.AddToLog(VpOldTitle + " supprimée dans la table TextesFR afin d'éviter un doublon", eLogType.Information)
						End Try
						Call Me.AddToLog("La mise à jour d'un nom de carte est terminée.", eLogType.Information, , True)
					Else
						Call Me.AddToLog("La mise à jour d'un nom de carte a été annulée.", eLogType.Warning, , True)
					End If
				Else
					Call Me.AddToLog("La mise à jour d'un nom de carte a été annulée.", eLogType.Warning, , True)
				End If
			Else
				Call Me.AddToLog("La mise à jour d'un nom de carte a été annulée (aucune correspondance trouvée).", eLogType.Warning, , True)
			End If
		Else
			Call Me.AddToLog("La mise à jour d'un nom de carte a été annulée.", eLogType.Warning, , True)
		End If
	End Sub
	Private Sub ExtractModIm
	'---------------------------------------------------------------------------------------
	'Extrait et concatène dans un fichier .dat les images préalablement corrigées de la base
	'---------------------------------------------------------------------------------------
	Dim VpIn As FileStream
	Dim VpInB As BinaryReader
	Dim VpOut As StreamWriter
	Dim VpOutB As BinaryWriter
	Dim VpPrev As Long = -1
	Dim VpPrev2 As Long = -1
	Dim VpTitle As String = ""
	Dim VpCurIn As StreamReader
	Dim VpCurInB As BinaryReader
	Dim VpOutFull As StreamWriter
	Dim VpOutFullB As BinaryWriter
	Dim VpOutLog As StreamWriter
	Dim VpDirIn As DirectoryInfo
	Dim VpCumul As Long = 0
		Me.dlgOpen3.FileName = ""
		Me.dlgOpen3.ShowDialog
		If Me.dlgOpen3.FileName <> "" Then
			Me.dlgBrowse.SelectedPath = ""
			Me.dlgBrowse.ShowDialog
			If Me.dlgBrowse.SelectedPath <> "" Then
				VpIn = New FileStream(Me.dlgOpen3.FileName, FileMode.Open)
				VpInB = New BinaryReader(VpIn)
				If Directory.Exists(Path.GetTempPath + "\mtgmwbr") Then
					Directory.Delete(Path.GetTempPath + "\mtgmwbr", True)
				End If
				Directory.CreateDirectory(Path.GetTempPath + "\mtgmwbr")
		    	Call Me.AddToLog("L'extraction des images corrigées a commencé...", eLogType.Information, True)
		    	Me.prgAvance.Style = ProgressBarStyle.Marquee
		    	VmDBCommand.CommandText = "Select Title, Offset, [End] From CardPictures Order By Offset Asc;"
		    	VmDBReader = VmDBCommand.ExecuteReader
				'Extraction
				With VmDBReader
					While .Read
						Application.DoEvents
		    			If VpPrev <> -1 Then
		    				If CDbl(.GetValue(1)) - VpPrev <> 1 Then
		    					VpInB.BaseStream.Seek(VpPrev2, SeekOrigin.Begin)
		    					VpOut = New StreamWriter(Path.GetTempPath + "\mtgmwbr\" + VpTitle + ".jpg")
		    					VpOutB = New BinaryWriter(VpOut.BaseStream)
		    					VpOutB.Write(VpInB.ReadBytes(VpPrev - VpPrev2 + 1))
		    					VpOutB.Flush
		    					VpOutB.Close
		    				End If
		    			End If
		    			VpPrev = CDbl(.GetValue(2))
		    			VpPrev2 = CDbl(.GetValue(1))
		    			VpTitle = .GetString(0)
		    			If Me.btCancel.Tag Then Exit While
					End While
					.Close
				End With
				'Concaténation
				If Not Me.btCancel.Tag Then
					VpOutFull = New StreamWriter(Me.dlgBrowse.SelectedPath + "\MD_Pict.dat")
					VpOutFullB = New BinaryWriter(VpOutFull.BaseStream)
					VpOutLog = New StreamWriter(Me.dlgBrowse.SelectedPath + "\MD_Pict.log")
					VpDirIn = New DirectoryInfo(Path.GetTempPath + "\mtgmwbr")
					Me.prgAvance.Style = ProgressBarStyle.Blocks
					Me.prgAvance.Maximum = VpDirIn.GetFiles().Length
					Me.prgAvance.Value = 0
					For Each VpFile As FileInfo In VpDirIn.GetFiles
						Application.DoEvents
						VpOutLog.WriteLine(VpFile.Name + "#" + VpCumul.ToString + "#" + (VpCumul + VpFile.Length - 1).ToString)
						VpCumul = VpCumul + VpFile.Length
						VpCurIn = New StreamReader(VpFile.FullName)
						VpCurInB = New BinaryReader(VpCurIn.BaseStream)
						VpOutFullB.Write(VpCurInB.ReadBytes(VpFile.Length))
						VpCurIn.Close
						Me.prgAvance.Increment(1)
						If Me.btCancel.Tag Then Exit For
					Next VpFile
					VpOutLog.Flush
					VpOutLog.Close
					VpOutFull.Flush
					VpOutFull.Close
					Directory.Delete(Path.GetTempPath + "\mtgmwbr", True)
				End If
				If Me.btCancel.Tag Then
					Call Me.AddToLog("L'extraction des images corrigées a été annulée.", eLogType.Warning, , True)
				Else
					Call Me.AddToLog("L'extraction des images corrigées est terminée.", eLogType.Information, , True)
				End If
			End If
		End If
	End Sub
	Private Sub ExtractCards(VpReq As String)
	'--------------------------------------------------
	'Listing des titres distincts des cartes de la base
	'--------------------------------------------------
	Dim VpOut As StreamWriter
		Me.dlgSave.FileName = ""
		Me.dlgSave.ShowDialog
		If Me.dlgSave.FileName <> "" Then
			VpOut = New StreamWriter(Me.dlgSave.FileName)
	    	Call Me.AddToLog("L'extraction des titres des cartes a commencé...", eLogType.Information, True)
	    	Me.prgAvance.Style = ProgressBarStyle.Marquee
	    	VmDBCommand.CommandText = VpReq
	    	VmDBReader = VmDBCommand.ExecuteReader
			With VmDBReader
				While .Read
					Application.DoEvents
					VpOut.WriteLine(.GetString(0))
					Me.txtCur.Text = .GetString(0)
					If Me.btCancel.Tag Then Exit While
				End While
				.Close
			End With
			VpOut.Flush
			VpOut.Close
			If Me.btCancel.Tag Then
				Call Me.AddToLog("L'extraction des titres des cartes a été annulée.", eLogType.Warning, , True)
			Else
				Call Me.AddToLog("L'extraction des titres des cartes est terminée.", eLogType.Information, , True)
			End If
		End If
	End Sub
	Private Sub ExtractCardsPricesAborted
	'-----------------------------------------------------------------------------------
	'Listing des cartes dont la mise à jour des prix ne semble pas avoir été téléchargée
	'-----------------------------------------------------------------------------------
	Dim VpOut As StreamWriter
	Dim VpCards As List(Of String)
	Dim VpCardsDB As List(Of String)
	Dim VpFound As Boolean
		Me.dlgOpen4.FileName = ""
		Me.dlgOpen4.ShowDialog
		If Me.dlgOpen4.FileName <> "" Then
			VpCards = Me.BuildListeFromFile(True)
		Else
			Exit Sub
		End If
		Me.dlgSave.FileName = ""
		Me.dlgSave.ShowDialog
		If Me.dlgSave.FileName <> "" Then
			VpOut = New StreamWriter(Me.dlgSave.FileName)
	    	Call Me.AddToLog("L'extraction des titres des cartes a commencé...", eLogType.Information, True)
	    	Me.prgAvance.Style = ProgressBarStyle.Marquee
	    	VpCardsDB = Me.BuildListeFromDB
			For Each VpCardDB As String In VpCardsDB
				Application.DoEvents
				VpFound = False
				For Each VpCard As String In VpCards
					If VpCard = VpCardDB Then
						VpFound = True
						Exit For
					End If
				Next VpCard
				If Not VpFound Then
					VpOut.WriteLine(VpCardDB)
					Me.txtCur.Text = VpCardDB
				End If
				If Me.btCancel.Tag Then Exit For
			Next VpCardDB
			VpOut.Flush
			VpOut.Close
			If Me.btCancel.Tag Then
				Call Me.AddToLog("L'extraction des titres des cartes a été annulée.", eLogType.Warning, , True)
			Else
				Call Me.AddToLog("L'extraction des titres des cartes est terminée.", eLogType.Information, , True)
			End If
		End If
	End Sub
	Private Function GetTextVF(VpIn As String) As String
	'------------------------------------------------------------
	'Retourne le texte français pour la carte passée en paramètre
	'------------------------------------------------------------
	Dim VpClient As New WebClient
	Dim VpStr As String = ""
	Dim VpStrs() As String
	Dim VpIr As Integer
		Try
			VpStr = Me.HTMLfromRequest(CmURL1.Replace(CmId, VpIn.Replace(" ", "+")))
			VpStrs = VpStr.Split(New String() {CmKey1}, StringSplitOptions.None)
			VpIr = Me.FindRightIndex(VpStrs, VpIn)
			VpStr = CmURL2 + VpStrs(VpIr).Substring(0, VpStrs(VpIr).IndexOf(""""))
			VpStr = Me.HTMLfromRequest(VpStr)
			If VpStr.Contains(CmKey5) Then
				VpStr = VpStr.Substring(VpStr.IndexOf(CmKey5))
				VpStr = VpStr.Substring(VpStr.IndexOf("<br /><br />") + 12)
				If VpStr.IndexOf("<div align=""right""><b>") <> -1 Then
					VpStr = VpStr.Substring(0, Math.Min(VpStr.IndexOf("</"), VpStr.IndexOf("<div align=""right""><b>")))
				Else
					VpStr = VpStr.Substring(0, VpStr.IndexOf("</"))
				End If
				VpStr = VpStr.Replace("&#039;", "'")
				VpStr = VpStr.Replace("&#8212;", "-")
				VpStr = VpStr.Replace("&#8217;", "'")
				VpStr = VpStr.Replace("&quot;", """")
				VpStr = VpStr.Replace("<img src=""/images/magic/manas/micro/", "!")
				VpStr = VpStr.Replace("<img src=""images/smileys/", "!")
				VpStr = VpStr.Replace("<img src=""/images/smileys/", "!")
				VpStr = VpStr.Replace(".gif""  border=""0"" style=""vertical-align: text-bottom;"" alt="""" />", "!")
				VpStr = VpStr.Replace("<br />", vbCrLf)
				Return VpStr
			End If
		Catch
		End Try
		Return ""
	End Function
	Private Sub TranslateTexts
	'---------------------------------------------------------------
	'Cherche le texte VF des cartes listées dans le fichier spécifié
	'---------------------------------------------------------------
	Dim VpOut As StreamWriter
	Dim VpStr As String
	Dim VpListe As List(Of String)
		Me.dlgOpen4.FileName = ""
		Me.dlgOpen4.ShowDialog
		If Me.dlgOpen4.FileName <> "" Then
			Me.dlgSave.FileName = ""
			Me.dlgSave.ShowDialog
			If Me.dlgSave.FileName <> "" Then
				VpOut = New StreamWriter(Me.dlgSave.FileName)
				Call Me.AddToLog("La récupération des textes des cartes en français a commencé...", eLogType.Information, True)
				'Construction de la liste
				VpListe = Me.BuildListeFromFile
				'Traduction
				Me.prgAvance.Maximum = VpListe.Count
				Me.prgAvance.Value = 0
				Me.prgAvance.Style = ProgressBarStyle.Blocks
				For Each VpCard As String In VpListe
					Application.DoEvents
					If VpCard.Trim <> "" Then
						Me.txtCur.Text = VpCard
						VpStr = Me.GetTextVF(VpCard)
						If VpStr.Trim <> "" Then
							VpOut.Write("##" + VpCard + "^^" + VpStr)
						Else
							Call Me.AddToLog("Impossible de récupérer le texte en français pour la carte : " + VpCard, eLogType.Warning)
						End If
					End If
					Me.prgAvance.Increment(1)
					Call Me.ETA
					If Me.btCancel.Tag Then Exit For
				Next VpCard
				VpOut.Flush
				VpOut.Close
				If Me.btCancel.Tag Then
					Call Me.AddToLog("La récupération des textes des cartes en français a été annulée.", eLogType.Warning, , True)
				Else
					Call Me.AddToLog("La récupération des textes des cartes en français est terminée.", eLogType.Information, , True)
				End If
			End If
		End If
	End Sub
	Private Sub BrowseAndWait(Optional VpURL As String = "", Optional VpExplicitWaitFor As String = "")
	'---------------------------------------------------------------------------
	'Navigue sur la page passée en paramètre en respectant le délai d'expiration
	'---------------------------------------------------------------------------
	Dim VpStart As Date = Now
		VmIsComplete = False
		If VpURL <> "" Then
			Me.wbMV.Navigate(VpURL)
		End If
		While Not VmIsComplete
			If Now.Subtract(VpStart).TotalSeconds > 5 Then
				If VpExplicitWaitFor <> "" Then
					Try
						If Not Me.wbMV.Document.All Is Nothing Then
							For Each VpElement As HtmlElement In Me.wbMV.Document.All
								If VpElement.Name = VpExplicitWaitFor Then
									Me.wbMV.Stop
									VmIsComplete = True
								End If
							Next VpElement
						End If
					Catch
					End Try
				Else
					Me.wbMV.Stop
					VmIsComplete = True
				End If
			End If
			Application.DoEvents
		End While
	End Sub
	Public Function GetAutorisations(VpCard As String) As String
	'------------------------------------------------------------
	'Retourne les autorisations pour la carte passée en paramètre
	'------------------------------------------------------------
	Dim VpElement As HtmlElement
	Dim VpLastId As Integer = 0
		'Site de Magic-Ville
		Call Me.BrowseAndWait(CmURL0, CmKey0)
		'Saisie de la carte dans la zone de recherche
		VpElement = Me.wbMV.Document.All.GetElementsByName(CmKey0).Item(0)
		VpElement.SetAttribute("value", VpCard)
		For Each VpElement In Me.wbMV.Document.All
			If VpElement.GetAttribute("src").ToLower.Contains("/go.gif") Then
				'Validation
				VpElement.InvokeMember("click")
				Call Me.BrowseAndWait
				Exit For
			End If
		Next VpElement
		'Page intermédiaire (ne s'affiche qu'en cas d'ambiguité)
		For Each VpElement In Me.wbMV.Document.All
			If VpElement.GetAttribute("href") <> "" AndAlso Not VpElement.InnerText Is Nothing Then
				If VpElement.InnerText.ToLower.Trim = VpCard.ToLower Then
					'Validation
					VpElement.InvokeMember("click")
					Call Me.BrowseAndWait
					Exit For
				End If
			End If
		Next VpElement
		For Each VpElement In Me.wbMV.Document.All
			If VpElement.InnerText = "Autorisations en Tournois" Then
				Return Me.TournoiFormat(VpElement.NextSibling.InnerHtml)
			End If
		Next VpElement
		Return ""
	End Function
	Private Function TournoiFormat(VpStr As String) As String
	Dim VpStrs() As String
	Dim VpAut As String = ""
		Try
			If VpStr.Contains(".png") Then
				VpStrs = VpStr.ToLower.Split(New String() {".png"}, StringSplitOptions.None)
				For Each VpA As String In VpStrs
					If VpA.Contains("/") Then
						VpAut = VpAut + VpA.Substring(VpA.LastIndexOf("/") + 1) + "#"
					End If
				Next VpA
				Return VpAut.Substring(0, VpAut.Length - 1)
			End If
		Catch
		End Try
		Return ""
	End Function
	Private Sub UpdateAutorisations(VpAll As Boolean)
	'----------------------------------------------------
	'Mise à jour des autorisations des cartes en tournois
	'----------------------------------------------------
	Dim VpOut As StreamWriter
	Dim VpCards As List(Of String)
	Dim VpAut As String
	Dim VpAppend As Boolean
	Dim VpAlready() As String
	Dim VpLast As String = ""
		Me.dlgSave.FileName = ""
		Me.dlgSave.ShowDialog
		If Me.dlgSave.FileName <> "" Then
			VpAppend = File.Exists(Me.dlgSave.FileName)
			If VpAppend Then
				'Si le fichier existe déjà, regarde la dernière carte qui a été traitée
				VpAlready = File.ReadAllLines(Me.dlgSave.FileName)
				If VpAlready.Length > 2 Then
					If VpAlready(VpAlready.Length - 1).Contains("#") Then
						VpLast = VpAlready(VpAlready.Length - 1)
					Else
						VpLast = VpAlready(VpAlready.Length - 2)
					End If
					VpLast = VpLast.Substring(0, VpLast.IndexOf("#"))
				Else
					VpAppend = False
				End If
			End If
			VpOut = New StreamWriter(Me.dlgSave.FileName, VpAppend)
			If VpAppend Then
				Call Me.AddToLog("La récupération des autorisations de tournois se poursuit...", eLogType.Information, True)
			Else
				Call Me.AddToLog("La récupération des autorisations de tournois a commencé...", eLogType.Information, True)
				'Inscription de la date
				VpOut.WriteLine(Now.ToShortDateString)
			End If
			'Récupère la liste des cartes
			If VpAll Then
				VpCards = Me.BuildListeFromDB(VpLast)
			Else
				Me.dlgOpen4.FileName = ""
				Me.dlgOpen4.ShowDialog
				If Me.dlgOpen4.FileName <> "" Then
					VpCards = Me.BuildListeFromFile
				Else
					VpOut.Close
					Exit Sub
				End If
			End If
			Me.prgAvance.Maximum = VpCards.Count
			Me.prgAvance.Value = 0
			Me.prgAvance.Style = ProgressBarStyle.Blocks
			For Each VpCard As String In VpCards
				Me.txtCur.Text = VpCard
				Application.DoEvents
				VpAut = Me.GetAutorisations(VpCard)
				If VpAut <> "" Then
					VpOut.WriteLine(VpCard + "#" + VpAut)
				Else
					Call Me.AddToLog("Impossible de récupérer les autorisations de tournois pour la carte : " + VpCard, eLogType.Warning)
				End If
				Me.prgAvance.Increment(1)
				Call Me.ETA
				If Me.btCancel.Tag Then Exit For
			Next VpCard
			VpOut.Flush
			VpOut.Close
			If Me.btCancel.Tag Then
				Call Me.AddToLog("La récupération des autorisations de tournois a été annulée.", eLogType.Warning, , True)
			Else
				Call Me.AddToLog("La récupération des autorisations de tournois est terminée.", eLogType.Information, , True)
			End If
		End If
	End Sub
	Private Function Matching(VpStr As String) As String
		Select Case VpStr
			Case "True"
				Return 1
			Case "False"
				Return 0
			Case Else
				Return VpStr.Replace(vbCrLf, " ")
		End Select
	End Function
	Private Function SerieShortcut(VpStr As String) As String
		Select Case VpStr.Substring(0, 2)
			Case "1E"
				Return "10th#" + VpStr
			Case "3B"
				Return "3rdBB#" + VpStr
			Case "3W"
				Return "3rdWB#" + VpStr
			Case "CT"
				Return "coldsnapthemedecks#" + VpStr
			Case "DM"
				Return "deckmasters#" + VpStr
			Case "PH"
				Return "planechase#" + VpStr
			Case "PI"
				Return "planechase2012#" + VpStr
			Case "PG"
				Return "pegase#" + VpStr
			Case "AR"
				Return "alarareborn#" + VpStr
			Case "BT"
				Return "beatdown#" + VpStr
			Case "CF"
				Return "conflux#" + VpStr
			Case "CS"
				Return "coldsnap#" + VpStr
			Case "DI"
				Return "dissension#" + VpStr
			Case "ET"
				Return "eventide#" + VpStr
			Case "FD"
				Return "fifthdawn#" + VpStr
			Case "FS"
				Return "futuresight#" + VpStr
			Case "GP"
				Return "guildpact#" + VpStr
			Case "LW"
				Return "lorwyn#" + VpStr
			Case "M1"
				Return "magic2010#" + VpStr
			Case "MT"
				Return "morningtide#" + VpStr
			Case "RA"
				Return "ravnica#" + VpStr
			Case "RE"
				Return "renaissance#" + VpStr
			Case "SL"
				Return "shardsofalara#" + VpStr
			Case "SM"
				Return "shadowmoor#" + VpStr
			Case "TS"
				Return "timespiral#" + VpStr
			Case "UH"
				Return "unhinged#" + VpStr
			Case "WW"
				Return "worldwake#" + VpStr
			Case "ZK"
				Return "zendikar#" + VpStr
			Case "RI"
				Return "riseoftheeldrazi#" + VpStr
			Case "M2"
				Return "magic2011#" + VpStr
			Case "TD"
				Return "timeshifted#" + VpStr
			Case "D1"
				Return "DuelDecksDivinevsDemonic#" + VpStr
			Case "D2"
				Return "DuelDecksElspethvsTezzeret#" + VpStr
			Case "D3"
				Return "DuelDecksElvesvsGoblins#" + VpStr
			Case "D4"
				Return "DuelDecksGarrukvsLiliana#" + VpStr
			Case "D5"
				Return "DuelDecksJacevsChandra#" + VpStr
			Case "D6"
				Return "DuelDecksPhyrexiavstheCoalition#" + VpStr
			Case "D7"
				Return "DuelDecksKnightsvsDragons#" + VpStr
			Case "D8"
				Return "DuelDecksAjanivsNicolBolas#" + VpStr
			Case "V1"
				Return "FromtheVaultDragons#" + VpStr
			Case "V2"
				Return "FromtheVaultExiled#" + VpStr
			Case "V3"
				Return "FromtheVaultRelics#" + VpStr
			Case "SD"
				Return "scarsofmirrodin#" + VpStr
			Case "R1"
				Return "fireandlightining#" + VpStr
			Case "R2"
				Return "slivers#" + VpStr
			Case "MB"
				Return "mirrodinbesieged#" + VpStr
			Case "DS"
				Return "darksteel#" + VpStr
			Case "PC"
				Return "planarchaos#" + VpStr
			Case "NP"
				Return "newphyrexia#" + VpStr
			Case "M3"
				Return "magic2012#" + VpStr
			Case "CD"
				Return "commander#" + VpStr
			Case "V4"
				Return "FromtheVaultLegends#" + VpStr
			Case "IN"
				Return "innistrad#" + VpStr
			Case "DA"
				Return "darkascension#" + VpStr
			Case "S2"
				Return "starter2000#" + VpStr
			Case "R3"
				Return "graveborn#" + VpStr
			Case "YR"
				Return "avacynrestored#" + VpStr
			Case "D9"
				Return "DuelDecksVenservsKoth#" + VpStr
			Case "M4"
				Return "magic2013#" + VpStr
			Case "RR"
				Return "returntoravnica#" + VpStr
			Case "DG"
				Return "DuelDecksIzzetvsGolgari#" + VpStr
			Case "V5"
				Return "FromtheVaultRealms#" + VpStr
			Case "GC"
				Return "gatecrash#" + VpStr
			Case "D0"
				Return "DuelDecksSorinvsTibalt#" + VpStr
			Case "DZ"
				Return "dragonsmaze#" + VpStr
			Case "MS"
				Return "modernmasters#" + VpStr
			Case "M5"
				Return "magic2014#" + VpStr
			Case "JG"
				Return "DCIJudgeGift#" + VpStr
			Case "AH"
				Return "archenemy#" + VpStr
			Case "V6"
				Return "FromtheVaultTwenty#" + VpStr
			Case "DD"
				Return "DuelDecksHeroesvsMonsters#" + VpStr
			Case "TH"
				Return "theros#" + VpStr
			Case "C3"
				Return "commander2013#" + VpStr
			Case "BG"
				Return "bornofthegods#" + VpStr
			Case "JN"
				Return "journeyintonyx#" + VpStr
			Case "DB"
				Return "DuelDecksJacevsVraska#" + VpStr
			Case "CY"
				Return "conspiracy#" + VpStr
			Case "M6"
				Return "magic2015#" + VpStr
			Case "DP"
				Return "duelsoftheplansewalkers#" + VpStr
			Case "FM"
				Return "fridaynightmagic#" + VpStr
			Case "IS"
				Return "introductory2pset#" + VpStr
			Case "MP"
				Return "magicplayerrewards#" + VpStr
			Case "ME"
				Return "mastersedition#" + VpStr
			Case "MV"
				Return "moderneventdeck2014#" + VpStr
			Case "PR"
				Return "prereleaseevents#" + VpStr
			Case "RT"
				Return "releaseevents#" + VpStr
			Case "GX"
				Return "grandprix#" + VpStr
			Case "KT"
				Return "khansoftarkir#" + VpStr
			Case "DH"
				Return "DuelDecksSpeedvsCunning#" + VpStr
			Case "C4"
				Return "commander2014#" + VpStr
			Case "V7"
				Return "FromtheVaultAnnihilation#" + VpStr
			Case "FR"
				Return "fatereforged#" + VpStr
			Case Else
				Return "#" + VpStr
		End Select
	End Function
	Private Function SerieCode(VpStr As String) As String
		Select Case VpStr
			Case "10th"
				Return "1E"
			Case "3rdBB"
				Return "3B"
			Case "3rdWB"
				Return "3W"
			Case "coldsnapthemedecks"
				Return "CT"
			Case "deckmasters"
				Return "DM"
			Case "planechase"
				Return "PH"
			Case "planechase2012"
				Return "PI"
			Case "pegase"
				Return "PG"
			Case "alarareborn"
				Return "AR"
			Case "beatdown"
				Return "BT"
			Case "conflux"
				Return "CF"
			Case "coldsnap"
				Return "CS"
			Case "dissension"
				Return "DI"
			Case "eventide"
				Return "ET"
			Case "fifthdawn"
				Return "FD"
			Case "futuresight"
				Return "FS"
			Case "guildpact"
				Return "GP"
			Case "lorwyn"
				Return "LW"
			Case "magic2010"
				Return "M1"
			Case "morningtide"
				Return "MT"
			Case "ravnica"
				Return "RA"
			Case "renaissance"
				Return "RE"
			Case "shardsofalara"
				Return "SL"
			Case "shadowmoor"
				Return "SM"
			Case "timespiral"
				Return "TS"
			Case "unhinged"
				Return "UH"
			Case "worldwake"
				Return "WW"
			Case "zendikar"
				Return "ZK"
			Case "riseoftheeldrazi"
				Return "RI"
			Case "magic2011"
				Return "M2"
			Case "timeshifted"
				Return "TD"
			Case "DuelDecksDivinevsDemonic"
				Return "D1"
			Case "DuelDecksElspethvsTezzeret"
				Return "D2"
			Case "DuelDecksElvesvsGoblins"
				Return "D3"
			Case "DuelDecksGarrukvsLiliana"
				Return "D4"
			Case "DuelDecksJacevsChandra"
				Return "D5"
			Case "DuelDecksPhyrexiavstheCoalition"
				Return "D6"
			Case "DuelDecksKnightsvsDragons"
				Return "D7"
			Case "DuelDecksAjanivsNicolBolas"
				Return "D8"
			Case "FromtheVaultDragons"
				Return "V1"
			Case "FromtheVaultExiled"
				Return "V2"
			Case "FromtheVaultRelics"
				Return "V3"
			Case "scarsofmirrodin"
				Return "SD"
			Case "fireandlightining"
				Return "R1"
			Case "slivers"
				Return "R2"
			Case "mirrodinbesieged"
				Return "MB"
			Case "darksteel"
				Return "DS"
			Case "planarchaos"
				Return "PC"
			Case "newphyrexia"
				Return "NP"
			Case "magic2012"
				Return "M3"
			Case "commander"
				Return "CD"
			Case "FromtheVaultLegends"
				Return "V4"
			Case "innistrad"
				Return "IN"
			Case "darkascension"
				Return "DA"
			Case "starter2000"
				Return "S2"
			Case "graveborn"
				Return "R3"
			Case "avacynrestored"
				Return "YR"
			Case "DuelDecksVenservsKoth"
				Return "D9"
			Case "magic2013"
				Return "M4"
			Case "returntoravnica"
				Return "RR"
			Case "DuelDecksIzzetvsGolgari"
				Return "DG"
			Case "FromtheVaultRealms"
				Return "V5"
			Case "gatecrash"
				Return "GC"
			Case "DuelDecksSorinvsTibalt"
				Return "D0"
			Case "dragonsmaze"
				Return "DZ"
			Case "modernmasters"
				Return "MS"
			Case "magic2014"
				Return "M5"
			Case "DCIJudgeGift"
				Return "JG"
			Case "archenemy"
				Return "AH"
			Case "FromtheVaultTwenty"
				Return "V6"
			Case "DuelDecksHeroesvsMonsters"
				Return "DD"
			Case "theros"
				Return "TH"
			Case "commander2013"
				Return "C3"
			Case "bornofthegods"
				Return "BG"
			Case "journeyintonyx"
				Return "JN"
			Case "DuelDecksJacevsVraska"
				Return "DB"
			Case "conspiracy"
				Return "CY"
			Case "magic2015"
				Return "M6"
			Case "duelsoftheplansewalkers"
				Return "DP"
			Case "fridaynightmagic"
				Return "FM"
			Case "introductory2pset"
				Return "IS"
			Case "magicplayerrewards"
				Return "MP"
			Case "mastersedition"
				Return "ME"
			Case "moderneventdeck2014"
				Return "MV"
			Case "prereleaseevents"
				Return "PR"
			Case "releaseevents"
				Return "RT"
			Case "grandprix"
				Return "GX"
			Case "khansoftarkir"
				Return "KT"
			Case "DuelDecksSpeedvsCunning"
				Return "DH"
			Case "commander2014"
				Return "C4"
			Case "FromtheVaultAnnihilation"
				Return "V7"
			Case "fatereforged"
				Return "FR"
			Case Else
				Return ""
		End Select
	End Function
	Private Sub BuildHeaders(VpR14 As Boolean)
	'-------------------------------------------
	'Génération du fichier d'en-têtes des séries
	'-------------------------------------------
	Dim VpTxt As StreamWriter
	Dim VpStr As String
		Me.dlgSave.FileName = ""
		Me.dlgSave.ShowDialog
		If Me.dlgSave.FileName <> "" Then
			VpTxt = New StreamWriter(Me.dlgSave.FileName)
			Call Me.AddToLog("La construction du fichier d'en-têtes a commencé...", eLogType.Information, True)
	    	VmDBCommand.CommandText = "Select SeriesCD, SeriesNM, SeriesNM_MtG, MIESetID, Cycle, LegalE, LegalS, Border, Release, RunSize, TotCards, UqCards, UqRare, UqUncom, UqComm, UqBLand, Foils, StartRare, StartUncom, StartComm, StartLand, BoostRare, BoostUncom, BoostComm, BoostLand, Decks, Starters, Boosters, Boxes, Notes, SeriesNM_FR, SeriesCD_MO, SeriesCD_MW From Series;"
	    	VmDBReader = VmDBCommand.ExecuteReader
			With VmDBReader
				While .Read
					Application.DoEvents
					VpStr = ""
					For VpI As Integer = 0 To If(VpR14, 30, 32)
						VpStr = VpStr + Me.Matching(.GetValue(VpI).ToString) + "#"
					Next VpI
					VpStr = Me.SerieShortcut(VpStr.Substring(0, VpStr.Length - 1))
					VpTxt.WriteLine(VpStr)
					If Me.btCancel.Tag Then Exit While
				End While
				.Close
			End With
			VpTxt.Flush
			VpTxt.Close
			If Me.btCancel.Tag Then
				Call Me.AddToLog("La construction du fichier d'en-têtes a été annulée.", eLogType.Warning, , True)
			Else
				Call Me.AddToLog("La construction du fichier d'en-têtes est terminée.", eLogType.Information, , True)
			End If
		End If
	End Sub
	Private Sub GetScan(VpIn As String)
	'----------------------------------------------------------
	'Télécharge l'image associée à la carte passée en paramètre
	'----------------------------------------------------------
	Dim VpClient As New WebClient
	Dim VpStr As String = ""
	Dim VpStrs() As String
	Dim VpIr As Integer
		Try
			VpStr = Me.HTMLfromRequest(CmURL1.Replace(CmId, VpIn.Replace(" ", "+")))
			VpStrs = VpStr.Split(New String() {CmKey1}, StringSplitOptions.None)
			VpIr = Me.FindRightIndex(VpStrs, VpIn)
			VpStr = CmURL2 + VpStrs(VpIr).Substring(0, VpStrs(VpIr).IndexOf(""""))
			VpStr = Me.HTMLfromRequest(VpStr)
			If VpStr.Contains(CmKey3) Then
				VpStr = VpStr.Substring(VpStr.IndexOf(CmKey3))
				VpStr = VpStr.Substring(CmKey3.Length)
				VpStr = VpStr.Substring(0, VpStr.IndexOf(""""))
				Call VpClient.DownloadFile(CmURL3 + VpStr, Me.dlgBrowse.SelectedPath + "\" + VpIn.Replace(":", "").Replace("/", "").Replace("""", "").Replace("?", "") + ".jpg")
			ElseIf VpStr.Contains(CmKey4) Then
				VpStr = VpStr.Substring(VpStr.IndexOf(CmKey4) + 5)
				VpStr = VpStr.Substring(0, VpStr.IndexOf(""""))
				Call VpClient.DownloadFile(VpStr, Me.dlgBrowse.SelectedPath + "\" + VpIn.Replace(":", "").Replace("/", "").Replace("""", "").Replace("?", "") + ".jpg")
			ElseIf VpStr.Contains(CmKey4B) Then
				VpStr = VpStr.Substring(VpStr.IndexOf(CmKey4B) + 6)
				VpStr = VpStr.Substring(0, VpStr.IndexOf(""""))
				Call VpClient.DownloadFile(CmURL4 + VpStr, Me.dlgBrowse.SelectedPath + "\" + VpIn.Replace(":", "").Replace("/", "").Replace("""", "").Replace("?", "") + ".jpg")
			End If
		Catch
		End Try
	End Sub
	Private Sub DownloadPictures
	'----------------------------------------------------------------------------
	'Télécharge les images  associées aux cartes listées dans le fichier spécifié
	'----------------------------------------------------------------------------
	Dim VpListe As List(Of String)
		Me.dlgOpen4.FileName = ""
		Me.dlgOpen4.ShowDialog
		If Me.dlgOpen4.FileName <> "" Then
			Me.dlgBrowse.SelectedPath = ""
			Me.dlgBrowse.ShowDialog
			If Me.dlgBrowse.SelectedPath <> "" Then
				Call Me.AddToLog("La récupération des images des cartes a commencé...", eLogType.Information, True)
				'Construction de la liste
				VpListe = Me.BuildListeFromFile
				'Récupération des images
				Me.prgAvance.Maximum = VpListe.Count
				Me.prgAvance.Value = 0
				Me.prgAvance.Style = ProgressBarStyle.Blocks
				For Each VpCard As String In VpListe
					Application.DoEvents
					If VpCard.Trim <> "" Then
						Me.txtCur.Text = VpCard
						Call Me.GetScan(VpCard)
						If Not File.Exists(Me.dlgBrowse.SelectedPath + "\" + VpCard + ".jpg") Then
							Call Me.AddToLog("Impossible de récupérer l'image de la carte : " + VpCard, eLogType.Warning)
						End If
					End If
					Me.prgAvance.Increment(1)
					Call Me.ETA
					If Me.btCancel.Tag Then Exit For
				Next VpCard
				If Me.btCancel.Tag Then
					Call Me.AddToLog("La récupération des images des cartes a été annulée.", eLogType.Warning, , True)
				Else
					Call Me.AddToLog("La récupération des images des cartes est terminée.", eLogType.Information, , True)
				End If
			End If
		End If
	End Sub
	Private Function GetListing(VpCode As String, VpLanguage As String) As List(Of String)
	'----------------------------------------------------------------------------
	'Récupère la liste des cartes dans la langue demandée pour l'édition demandée
	'----------------------------------------------------------------------------
	Dim VpStr As String = ""
	Dim VpKey As String
	Dim VpItem As String
	Dim VpListe As New List(Of String)
		VpStr = Me.HTMLfromRequest(CmURL5.Replace("###", VpCode).Replace("^^", VpLanguage))
		VpKey = CmKey6.Replace("###", VpCode).Replace("^^", VpLanguage)
		If VpStr.Contains(VpKey) Then
			VpStr = VpStr.Substring(VpStr.IndexOf(VpKey))
			While VpStr.Contains(CmKey6B)
				VpStr = VpStr.Substring(VpStr.IndexOf(CmKey6B) + CmKey6B.Length)
				VpItem = VpStr.Substring(0, VpStr.IndexOf("</"))
				VpListe.Add(VpItem)
			End While
		End If
		Return VpListe
	End Function
	Private Sub DownloadSpoilers(VpCode As String)
	'----------------------------------------------------------------------------------------------------------
	'Récupère les fichiers (listing avec traduction, checklist, spoilerlist) nécessaires à l'ajout de l'édition
	'----------------------------------------------------------------------------------------------------------
	Dim VpStr As String = ""
	Dim VpStrs() As String
	Dim VpListeEN As List(Of String)
	Dim VpListeFR As List(Of String)
	Dim VpOut As StreamWriter
	Dim VpCount As Integer
	Dim VpItem As String
	Dim VpNewItem As String
		'Récupération des traductions
		Call Me.AddToLog("Récupération du listing VO/VF...", eLogType.Information)
		VpListeEN = Me.GetListing(VpCode, "en")
		VpListeFR = Me.GetListing(VpCode, "fr")
		If VpListeFR.Count > 0 AndAlso VpListeFR.Count = VpListeEN.Count Then
			VpOut = New StreamWriter(Me.dlgBrowse.SelectedPath + "\" + VpCode + "_titles_fr.txt", False, Encoding.Default)
			For VpI As Integer = 0 To VpListeEN.Count - 1
				VpOut.WriteLine(VpListeEN.Item(VpI) + "#" + VpListeFR.Item(VpI))
			Next VpI
			VpOut.Flush
			VpOut.Close
		Else
			Call Me.AddToLog("Traduction VF indisponible.", eLogType.Warning)
		End If
		'Récupération de la checklist
		Call Me.AddToLog("Récupération de la checklist...", eLogType.Information)
		VpStr = Me.HTMLfromRequest(CmURL5.Replace("###", VpCode).Replace("^^", "en"))
		VpOut = New StreamWriter(Me.dlgBrowse.SelectedPath + "\" + VpCode + "_checklist_en.txt", False, Encoding.Default)
		VpOut.WriteLine("# 	Name 	Artist 	Color 	Rarity 	Set")
		VpCount = 0
		For Each VpCard As String In VpListeEN
			VpStr = VpStr.Substring(VpStr.IndexOf(VpCard))
			VpStrs = VpStr.Split(New String() {CmKey7}, StringSplitOptions.None)
			For VpI As Integer = 1 To 5
				While VpStrs(VpI).LastIndexOf(CmKey7B) >= 0
					VpStrs(VpI) = VpStrs(VpI).Substring(0, VpStrs(VpI).LastIndexOf(CmKey7B))
				End While
			Next VpI
			VpItem = ""
			For VpI As Integer = 0 To VpStrs(2).Length - 1
				Select Case VpStrs(2).Substring(VpI, 1)
					Case "W"
						VpNewItem = "White"
					Case "B"
						VpNewItem = "Black"
					Case "U"
						VpNewItem = "Blue"
					Case "R"
						VpNewItem = "Red"
					Case "G"
						VpNewItem = "Green"
					Case Else
						VpNewItem = ""
				End Select
				If VpNewItem <> "" AndAlso Not VpItem.Contains(VpNewItem) Then
					VpItem += If(VpItem <> "", "/", "") + VpNewItem
				End If
			Next VpI
			Select Case VpStrs(3)
				Case "Mythic Rare"
					VpStrs(3) = "M"
				Case "Rare"
					VpStrs(3) = "R"
				Case "Uncommon"
					VpStrs(3) = "U"
				Case "Common"
					VpStrs(3) = "C"
				Case "Special"
					VpStrs(3) = "X"
				Case Else
					VpStrs(3) = ""
			End Select
			VpStrs(5) = VpStrs(5).Replace(CmKey7C, "")
			VpCount += 1
			VpOut.WriteLine(VpCount.ToString + vbTab + VpCard + vbTab + VpStrs(4) + vbTab + VpItem + vbTab + VpStrs(3) + vbTab + VpStrs(5))
		Next VpCard
		VpOut.Flush
		VpOut.Close
		'Récupération de la spoilerlist
		Call Me.AddToLog("Récupération de la spoilerlist...", eLogType.Information)
		VpStr = Me.HTMLfromRequest(CmURL6.Replace("###", VpCode))
		VpOut = New StreamWriter(Me.dlgBrowse.SelectedPath + "\" + VpCode + "_spoiler_en.txt", False, Encoding.Default)
		For Each VpCard As String In VpListeEN
			VpStr = VpStr.Substring(VpStr.IndexOf(">" + VpCard))
			VpStrs = VpStr.Split(New String() {CmKey8}, StringSplitOptions.None)
			VpOut.WriteLine("Name: " + vbTab + VpCard)
			VpItem = VpStrs(2).Substring(VpStrs(2).IndexOf(vbLf) + 1)
			If VpItem.Contains("(") Then
				VpItem = VpItem.Substring(0, VpItem.LastIndexOf("("))
			Else
				VpItem = ""
			End If
			VpOut.WriteLine("Cost: " + vbTab + VpItem.Trim)
			VpItem = VpStrs(2).Substring(1)
			VpItem = VpItem.Substring(0, VpItem.IndexOf(","))
			If VpItem.Contains("Loyalty:") Then
				VpNewItem = "(0/" + VpItem.Substring(VpItem.IndexOf("Loyalty:") + 9)
				VpItem = VpItem.Substring(0, VpItem.IndexOf("Loyalty:") - 2)
			ElseIf VpItem.Contains(" ") And VpItem.Contains("/") Then
				VpNewItem = "(" + VpItem.Substring(VpItem.LastIndexOf(" ") + 1) + ")"
				VpItem = VpItem.Substring(0, VpItem.LastIndexOf(" "))
			Else
				VpNewItem = ""
			End If
			VpOut.WriteLine("Type: " + vbTab + VpItem)
			VpOut.WriteLine("Pow/Tgh: " + vbTab + VpNewItem)
			VpItem = VpStrs(3).Substring(VpStrs(3).IndexOf(CmKey8B) + 3)
			VpItem = VpItem.Replace(CmKey8C, "").Replace(CmKey8D, vbCrLf)
			VpOut.WriteLine("Rules Text: " + vbTab + VpItem.Trim)
			VpItem = VpStrs(1).Substring(1).Replace(CmKey7C, "").Replace(CmKey8E, " ").Replace(CmKey8F, "").Trim
			VpOut.WriteLine("Set/Rarity: " + vbTab + VpItem)
			VpOut.WriteLine("")
		Next VpCard
		VpOut.Flush
		VpOut.Close
		Call Me.AddToLog("La récupération des fichiers spoilers est terminée.", eLogType.Information)
	End Sub
	Private Sub ExtractTexts
	'-----------------------------------------
	'Extrait les textes des cartes en français
	'-----------------------------------------
	Dim VpOut As StreamWriter
		Me.dlgSave.FileName = ""
		Me.dlgSave.ShowDialog
		If Me.dlgSave.FileName <> "" Then
			VpOut = New StreamWriter(Me.dlgSave.FileName)
			Call Me.AddToLog("L'extraction des textes des cartes en français a commencé...", eLogType.Information, True)
			Me.prgAvance.Style = ProgressBarStyle.Marquee
	    	VmDBCommand.CommandText = "Select Distinct TextesFR.CardName, TextesFR.TexteFR From TextesFR Inner Join Card On TextesFR.CardName = Card.Title Where Card.CardText <> TextesFR.TexteFR;"
	    	VmDBReader = VmDBCommand.ExecuteReader
			With VmDBReader
				While .Read
					Me.txtCur.Text = .GetString(0)
					Application.DoEvents
					Try
						If .GetString(1).Trim <> "" Then
							VpOut.Write("##" + .GetString(0) + "^^" + .GetString(1))
						End If
					Catch
						Call Me.AddToLog("Impossible de récupérer le texte de la carte : " + .GetString(0), eLogType.Warning)
					End Try
					If Me.btCancel.Tag Then Exit While
				End While
				.Close
			End With
			VpOut.Flush
			VpOut.Close
			If Me.btCancel.Tag Then
				Call Me.AddToLog("L'extraction des textes des cartes en français a été annulée.", eLogType.Warning, , True)
			Else
				Call Me.AddToLog("L'extraction des textes des cartes en français est terminée.", eLogType.Information, , True)
			End If
		End If
	End Sub
	Private Sub BuildTitles
	'---------------------------------------------------------------------------------------------------------------
	'Construit un fichier de titres des cartes en français pour l'édition demandée, avec les infos déjà dans la base
	'---------------------------------------------------------------------------------------------------------------
	Dim VpSerie As String = InputBox("Entrer le code de la série")
	Dim VpOut As StreamWriter
		If VpSerie.Length = 2 Then
			Me.dlgSave.FileName = ""
			Me.dlgSave.ShowDialog
			If Me.dlgSave.FileName <> "" Then
				VpOut = New StreamWriter(Me.dlgSave.FileName)
				Call Me.AddToLog("La construction du fichier des titres traduits a commencé...", eLogType.Information, True)
				Me.prgAvance.Style = ProgressBarStyle.Marquee
		    	VmDBCommand.CommandText = "Select Card.Title, CardFR.TitleFR From (Card Inner Join Series On Card.Series = Series.SeriesCD) Inner Join CardFR On Card.EncNbr = CardFR.EncNbr Where UCase(Series.SeriesCD) = '" + VpSerie.ToUpper + "';"
		    	VmDBReader = VmDBCommand.ExecuteReader
				With VmDBReader
					While .Read
						Me.txtCur.Text = .GetString(0)
						Application.DoEvents
						Try
							If .GetString(1).Trim <> "" And .GetString(1) <> .GetString(0) Then
								VpOut.WriteLine(.GetString(0) + "#" + .GetString(1))
							End If
						Catch
							Call Me.AddToLog("Impossible de récupérer le titre de la carte : " + .GetString(0), eLogType.Warning)
						End Try
						If Me.btCancel.Tag Then Exit While
					End While
					.Close
				End With
				VpOut.Flush
				VpOut.Close
				If Me.btCancel.Tag Then
					Call Me.AddToLog("La construction du fichier des titres traduits a été annulée.", eLogType.Warning, , True)
				Else
					Call Me.AddToLog("La construction du fichier des titres traduits est terminée.", eLogType.Information, , True)
				End If
			End If
		End If
	End Sub
	Private Sub BuildDouble
	'----------------------------------------------------------------------------------------------------------------------------
	'Construit le fichier des doubles cartes pour l'édition demandée, avec les infos déjà dans la base (associations recto-verso)
	'----------------------------------------------------------------------------------------------------------------------------
	Dim VpSerie As String = InputBox("Entrer le code de la série")
	Dim VpOut As StreamWriter
	Dim VpDown As Hashtable
	Dim VpTop As Hashtable
		If VpSerie.Length = 2 Then
			Me.dlgSave.FileName = ""
			Me.dlgSave.ShowDialog
			If Me.dlgSave.FileName <> "" Then
				VpOut = New StreamWriter(Me.dlgSave.FileName)
				Call Me.AddToLog("La construction du fichier des doubles cartes a commencé...", eLogType.Information, True)
				Me.prgAvance.Style = ProgressBarStyle.Marquee
				VpDown = Me.BuildDoubleHash("EncNbrDownFace", VpSerie)
				VpTop = Me.BuildDoubleHash("EncNbrTopFace", VpSerie)
				For Each VpEncNbr1 As Integer In VpDown.Keys
					Application.DoEvents
					For Each VpEncNbr2 As Integer In VpTop.Keys
						Application.DoEvents
						If VpEncNbr1 = VpEncNbr2 Then
							VpOut.WriteLine(VpDown.Item(VpEncNbr1) + "#" + VpTop.Item(VpEncNbr1))
							Exit For
						End If
					Next VpEncNbr2
					If Me.btCancel.Tag Then Exit For
				Next VpEncNbr1
				VpOut.Flush
				VpOut.Close
				If Me.btCancel.Tag Then
					Call Me.AddToLog("La construction du fichier des doubles cartes a été annulée.", eLogType.Warning, , True)
				Else
					Call Me.AddToLog("La construction du fichier des doubles cartes est terminée.", eLogType.Information, , True)
				End If
			End If
		End If
	End Sub
	Private Function BuildDoubleHash(VpField As String, VpSerie As String) As Hashtable
	Dim VpHash As New Hashtable
		VmDBCommand.CommandText = "Select Card.Title, CardDouble.EncNbrTopFace From Card Inner Join CardDouble On CardDouble." + VpField + " = Card.EncNbr Where UCase(Card.Series) = '" + VpSerie.ToUpper + "';"
		VmDBReader = VmDBCommand.ExecuteReader
		With VmDBReader
			While .Read
				Application.DoEvents
				VpHash.Add(.GetInt32(1), .GetString(0))
				If Me.btCancel.Tag Then Exit While
			End While
			.Close
		End With
		Return VpHash
	End Function
	Private Sub ExtractTitles
	'-----------------------------------------
	'Extrait les titres des cartes en français
	'-----------------------------------------
	Dim VpIn As StreamReader
	Dim VpOut As StreamWriter
	Dim VpStr As String
	Dim VpStr2 As String
		Me.dlgOpen4.FileName = ""
		Me.dlgOpen4.ShowDialog
		If Me.dlgOpen4.FileName <> "" Then
			Me.dlgSave.FileName = ""
			Me.dlgSave.ShowDialog
			If Me.dlgSave.FileName <> "" Then
				VpIn = New StreamReader(Me.dlgOpen4.FileName, Encoding.Default)
				VpOut = New StreamWriter(Me.dlgSave.FileName, False, VpIn.CurrentEncoding)
				Call Me.AddToLog("Le filtrage des titres des cartes en français a commencé...", eLogType.Information, True)
				Me.prgAvance.Style = ProgressBarStyle.Marquee
				Application.DoEvents
				While Not VpIn.EndOfStream
					VpStr = VpIn.ReadLine
					If VpStr.StartsWith("Name:") Then
						If VpStr.Contains("(") Then
							VpStr2 = VpStr.Substring(VpStr.IndexOf("(") + 1)
							If VpStr2.Contains("(") Then
								VpStr2 = VpStr2.Substring(VpStr2.IndexOf("(") + 1)
							End If
							VpStr2 = VpStr2.Substring(0, VpStr2.Length - 1)
							VpStr = VpStr.Replace("Name:", "").Replace("(" + VpStr2 + ")", "").Trim
						Else
							VpStr = VpStr.Replace("Name:", "").Trim
							VpStr2 = VpStr
						End If
						VpOut.WriteLine(VpStr2 + "#" + VpStr)
					End If
				End While
				Call Me.AddToLog("Le filtrage des titres des cartes en français est terminé.", eLogType.Information, , True)
				VpOut.Flush
				VpOut.Close
				VpIn.Close
			End If
		End If
	End Sub
	Private Sub BuildPatch
	'------------------------------------------------
	'Construction du Patch r9 (modèles et historique)
	'------------------------------------------------
		Call Me.AddToLog("La construction du patch a commencé...", eLogType.Information, True)
		Me.prgAvance.Style = ProgressBarStyle.Marquee
		Application.DoEvents
		With VmDBCommand
			Try
				'.CommandText = "Delete * From MSysRelationships;"
				'.ExecuteNonQuery
				.CommandText = "Drop Table Series;"
				.ExecuteNonQuery
				.CommandText = "Drop Table Autorisations;"
				.ExecuteNonQuery
				.CommandText = "Drop Table CardDouble;"
				.ExecuteNonQuery
				.CommandText = "Drop Table CardFR;"
				.ExecuteNonQuery
				.CommandText = "Drop Table CardPictures;"
				.ExecuteNonQuery
				.CommandText = "Drop Table Creature;"
				.ExecuteNonQuery
				.CommandText = "Drop Table MyAdversairesID;"
				.ExecuteNonQuery
				.CommandText = "Drop Table MyCollection;"
				.ExecuteNonQuery
				.CommandText = "Drop Table MyGames;"
				.ExecuteNonQuery
				.CommandText = "Drop Table MyGamesID;"
				.ExecuteNonQuery
				.CommandText = "Drop Table MyScores;"
				.ExecuteNonQuery
				.CommandText = "Delete * From MySpecialUses;"
				.ExecuteNonQuery
				.CommandText = "Drop Table Spell;"
				.ExecuteNonQuery
				.CommandText = "Drop Table TextesFR;"
				.ExecuteNonQuery
				.CommandText = "Alter Table PricesHistory Add Title Text(80) With Compression;"
				.ExecuteNonQuery
				.CommandText = "Alter Table PricesHistory Add Series Text(2) With Compression;"
				.ExecuteNonQuery
		    	.CommandText = "Update Card Inner Join PricesHistory On Card.EncNbr = PricesHistory.EncNbr Set PricesHistory.Title = Card.Title, PricesHistory.Series = Card.Series Where PricesHistory.EncNbr = Card.EncNbr;"
		    	.ExecuteNonQuery
				.CommandText = "Drop Index EncNbr On PricesHistory;"
				.ExecuteNonQuery
				.CommandText = "Alter Table PricesHistory Drop Column EncNbr;"
				.ExecuteNonQuery
				.CommandText = "Drop Table Card;"
				.ExecuteNonQuery
			Catch
				Call Me.AddToLog("La construction du patch a échoué car la base contient encore des relations.", eLogType.MyError, , True)
				Exit Sub
			End Try
		End With
		Call Me.AddToLog("La construction du patch est terminée.", eLogType.Information, , True)
	End Sub
	Private Sub ReadyForRelease
	'-------------------------------------------
	'Prépare la base pour l'intégration au setup
	'-------------------------------------------
		Call Me.AddToLog("La préparation de la base a commencé...", eLogType.Information, True)
		Me.prgAvance.Style = ProgressBarStyle.Marquee
		Application.DoEvents
		With VmDBCommand
			.CommandText = "Delete * From MyScores;"
			.ExecuteNonQuery
			.CommandText = "Delete * From MySpecialUses;"
			.ExecuteNonQuery
			.CommandText = "Delete * From MyCollection;"
			.ExecuteNonQuery
			.CommandText = "Delete * From MyGames;"
			.ExecuteNonQuery
			.CommandText = "Delete * From MyGamesID;"
			.ExecuteNonQuery
			.CommandText = "Delete * From MyAdversairesID;"
			.ExecuteNonQuery
			.CommandText = "Insert Into MyAdversairesID(AdvID, AdvName) Values (0, '" + CmMe + "');"
			.ExecuteNonQuery
		End With
		Call Me.AddToLog("La préparation de la base est terminée.", eLogType.Information, , True)
	End Sub
	Private Sub ReadyINI
	'--------------------------------------------------
	'Prépare le fichier INI pour l'intégration au setup
	'--------------------------------------------------
	Dim VpDir As String
	Dim VpINIPath As String
	Dim VpStampFile() As String
		Me.dlgOpen5.FileName = ""
		Me.dlgOpen5.ShowDialog
		VpINIPath = Me.dlgOpen5.FileName
		If VpINIPath <> "" Then
			VpDir = VpINIPath.Substring(0, VpINIPath.LastIndexOf("\") + 1)
			Call Me.AddToLog("La préparation du fichier de configuration a commencé...", eLogType.Information, True)
			Application.DoEvents
			If File.Exists(VpDir + CmStamp) Then
				VpStampFile = File.ReadAllLines(VpDir + CmStamp)
				For VpI As Integer = 0 To CmIndexes.Length - 1
					Call WritePrivateProfileString(CmCategory, CmFields(VpI), VpStampFile(CmIndexes(VpI) - 1), VpINIPath)
				Next VpI
			Else
				Call Me.AddToLog("Impossible de trouver le fichier de référence " + CmStamp + ".", eLogType.MyError, , True)
				Exit Sub
			End If
			Call Me.AddToLog("La préparation du fichier de configuration est terminée.", eLogType.Information, , True)
		End If
	End Sub
	Private Sub BuildSP
	'---------------------------------------------------------------------------------------
	'Construit un nouveau Service Pack d'images en concaténant celles du répertoire spécifié
	'---------------------------------------------------------------------------------------
	Dim VpCurIn As StreamReader
	Dim VpCurInB As BinaryReader
	Dim VpOutFull As StreamWriter
	Dim VpOutFullB As BinaryWriter
	Dim VpOutLog As StreamWriter
	Dim VpDirIn As DirectoryInfo
	Dim VpCumul As Long = 0
		Me.dlgBrowse.SelectedPath = ""
		Me.dlgBrowse.ShowDialog
		If Me.dlgBrowse.SelectedPath <> "" Then
			Me.dlgSave2.FileName = ""
			Me.dlgSave2.ShowDialog
			If Me.dlgSave2.FileName <> "" Then
				Call Me.AddToLog("La construction du Service Pack a commencé...", eLogType.Information, True)
				VpOutFull = New StreamWriter(Me.dlgSave2.FileName)
				VpOutFullB = New BinaryWriter(VpOutFull.BaseStream)
				VpOutLog = New StreamWriter(Me.dlgSave2.FileName.Replace(".dat", ".log"))
				VpDirIn = New DirectoryInfo(Me.dlgBrowse.SelectedPath)
				Me.prgAvance.Style = ProgressBarStyle.Blocks
				Me.prgAvance.Maximum = VpDirIn.GetFiles().Length
				Me.prgAvance.Value = 0
				For Each VpFile As FileInfo In VpDirIn.GetFiles
					Application.DoEvents
					VpOutLog.WriteLine(VpFile.Name + "#" + VpCumul.ToString + "#" + (VpCumul + VpFile.Length - 1).ToString)
					VpCumul = VpCumul + VpFile.Length
					VpCurIn = New StreamReader(VpFile.FullName)
					VpCurInB = New BinaryReader(VpCurIn.BaseStream)
					VpOutFullB.Write(VpCurInB.ReadBytes(VpFile.Length))
					VpCurIn.Close
					Me.prgAvance.Increment(1)
					If Me.btCancel.Tag Then Exit For
				Next VpFile
				VpOutLog.Flush
				VpOutLog.Close
				VpOutFull.Flush
				VpOutFull.Close
			End If
			If Me.btCancel.Tag Then
				Call Me.AddToLog("La construction du Service Pack a été annulée.", eLogType.Warning, , True)
			Else
				Call Me.AddToLog("La construction du Service Pack est terminée.", eLogType.Information, , True)
			End If
		End If
	End Sub
	Private Sub RevertSP
	'-----------------------------------------------------
	'Reconstruit la base des images dans un état antérieur
	'-----------------------------------------------------
	Dim VpOut As FileStream
	Dim VpOutB As BinaryWriter
	Dim VpIn As StreamReader
	Dim VpInB As BinaryReader
	Dim VpSize As Integer
		Me.dlgOpen3.FileName = ""
		Me.dlgOpen3.ShowDialog
		If Me.dlgOpen3.FileName <> "" Then
			Me.dlgSave2.FileName = ""
			Me.dlgSave2.ShowDialog
			If Me.dlgSave2.FileName <> "" Then
				Call Me.AddToLog("La reconstruction des images dans un état antérieur a commencé...", eLogType.Information, True)
				VpSize = CInt(Val(InputBox("Taille du fichier de sortie ?")))
				If VpSize <> 0 Then
					VpOut = New FileStream(Me.dlgSave2.FileName, FileMode.OpenOrCreate)
					VpOutB = New BinaryWriter(VpOut)
					VpIn = New StreamReader(Me.dlgOpen3.FileName)
					VpInB = New BinaryReader(VpIn.BaseStream)
	    			VpOutB.Seek(0, SeekOrigin.Begin)
	    			VpOutB.Write(VpInB.ReadBytes(VpSize))
					VpIn.Close
					VpOutB.Flush
					VpOutB.Close
					Call Me.AddToLog("La reconstruction des images dans un état antérieur est terminée.", eLogType.Information, , True)
				Else
					Call Me.AddToLog("La reconstruction des images dans un état antérieur a été annulée.", eLogType.Warning, , True)
				End If
			End If
		End If
	End Sub
	Private Sub FindHoles
	Dim VpEncNbrs() As Long
	Dim VpMin As Long
	Dim VpMax As Long
	Dim VpBestFit As Long
	Dim VpBestFitDelta As Long = Long.MaxValue
	Dim VpSerie As String = InputBox("Entrer le code de la série")
		If VpSerie.Length = 2 Then
			VmDBCommand.CommandText = "Select Min(EncNbr) From Card Where UCase(Series) = '" + VpSerie.ToUpper + "';"
			VpMin = VmDBCommand.ExecuteScalar
			VmDBCommand.CommandText = "Select Max(EncNbr) From Card Where UCase(Series) = '" + VpSerie.ToUpper + "';"
			VpMax = VmDBCommand.ExecuteScalar
			Call Me.AddToLog("L'intervalle pour cette série est [" + VpMin.ToString + ";" + VpMax.ToString + "]", eLogType.Information, , True)
	    	VmDBCommand.CommandText = "Select EncNbr From Card Order By EncNbr;"
	    	VmDBReader = VmDBCommand.ExecuteReader
	    	ReDim VpEncNbrs(0)
			With VmDBReader
				While .Read
					VpEncNbrs(VpEncNbrs.Length - 1) = .GetValue(0)
					ReDim Preserve VpEncNbrs(0 To VpEncNbrs.Length)
				End While
				.Close
			End With
			ReDim Preserve VpEncNbrs(0 To VpEncNbrs.Length - 2)
			For VpI As Integer = 1 To VpEncNbrs.Length - 1
				If VpEncNbrs(VpI) > 1 + VpEncNbrs(VpI - 1) Then
					'Debug.Print("Emplacement dispo. entre : " + VpEncNbrs(VpI - 1).ToString + " et " + VpEncNbrs(VpI).ToString)
					If Math.Abs(VpEncNbrs(VpI - 1) - VpMin) < VpBestFitDelta Then
						VpBestFitDelta = Math.Abs(VpEncNbrs(VpI - 1) - VpMin)
						VpBestFit = VpEncNbrs(VpI - 1)
					End If
					If Math.Abs(VpEncNbrs(VpI) - VpMin) < VpBestFitDelta Then
						VpBestFitDelta = Math.Abs(VpEncNbrs(VpI - 1) - VpMin)
						VpBestFit = VpEncNbrs(VpI)
					End If
					If Math.Abs(VpEncNbrs(VpI - 1) - VpMax) < VpBestFitDelta Then
						VpBestFitDelta = Math.Abs(VpEncNbrs(VpI - 1) - VpMin)
						VpBestFit = VpEncNbrs(VpI - 1)
					End If
					If Math.Abs(VpEncNbrs(VpI) - VpMax) < VpBestFitDelta Then
						VpBestFitDelta = Math.Abs(VpEncNbrs(VpI - 1) - VpMin)
						VpBestFit = VpEncNbrs(VpI)
					End If
				End If
			Next VpI
			Call Me.AddToLog("Emplacement le plus adapté : " + VpBestFit.ToString, eLogType.Information, , True)
			VmDBCommand.CommandText = "Select Series.SeriesNM From Series Inner Join Card On Series.SeriesCD = Card.Series Where Card.EncNbr = " + VpBestFit.ToString + ";"
			Call Me.AddToLog("Série normalement à cet endroit : " + VmDBCommand.ExecuteScalar.ToString, eLogType.Information, , True)
		End If
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
		    	Call Me.AddToLog("Base de données ouverte avec succès.", eLogType.Information)
			Catch
				Call Me.AddToLog("Impossible d'ouvrir la base de données...", eLogType.Warning)
			End Try
	    End If
	End Sub
	Sub MainFormFormClosing(sender As Object, e As FormClosingEventArgs)
		If Me.btCancel.Enabled = True Then
			MessageBox.Show("Une opération est en cours..." + vbCrLf + "Vous devez d'abord l'annuler avant de quitter l'application.", "Problème", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1)
			e.Cancel = True
		ElseIf Not VmDB Is Nothing Then
			VmDB.Close
			VmDB.Dispose
		End If
	End Sub
	Sub BtCancelClick(sender As Object, e As EventArgs)
		If MessageBox.Show("Voulez-vous vraiment annuler l'opération en cours ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = System.Windows.Forms.DialogResult.Yes Then
			Me.btCancel.Tag = True
		End If
	End Sub
	Sub MnuAboutClick(sender As Object, e As EventArgs)
	Dim VpAbout As New About
		VpAbout.ShowDialog
	End Sub
	Sub MnuPricesUpdateClick(sender As Object, e As EventArgs)
		If Not VmDB Is Nothing Then
			Call Me.UpdatePrices(True)
		End If
	End Sub
	Sub MnuPricesUpdateListeClick(sender As Object, e As EventArgs)
		If Not VmDB Is Nothing Then
			Call Me.UpdatePrices(False)
		End If
	End Sub
	Sub MnuPricesHistoryAddClick(sender As Object, e As EventArgs)
		If Not VmDB Is Nothing Then
			Call Me.FillPricesHistory
		End If
	End Sub
	Sub MnuPicturesFixClick(sender As Object, e As EventArgs)
		If Not VmDB Is Nothing Then
			Call Me.FixPictures
		End If
	End Sub
	Sub MnuPicturesDeltaClick(sender As Object, e As EventArgs)
		If Not VmDB Is Nothing Then
			Call Me.ExtractModIm
		End If
	End Sub
	Sub MnuCardsTradTxtClick(sender As Object, e As EventArgs)
		Call Me.TranslateTexts
	End Sub
	Sub MnuCardsExtractAllClick(sender As Object, e As EventArgs)
		If Not VmDB Is Nothing Then
			Call Me.ExtractCards("Select Distinct Title From Card Order By Title Asc;")
		End If
	End Sub
	Sub MnuCardsExtractDiffClick(sender As Object, e As EventArgs)
		If Not VmDB Is Nothing Then
			'Select Distinct Card.Title From Card Where Not Exists (Select CardPictures.Title From CardPictures Where CardPictures.Title = Replace(Replace(Replace(Replace(Card.Title, ':', ''), '/', ''), '""', ''), '?', '')) Order By Card.Title Asc;
			Call Me.ExtractCards("Select Distinct Card.Title From Card Where Not Exists (Select CardPictures.Title From CardPictures Where CardPictures.Title = Card.Title) Order By Card.Title Asc;")
			Call Me.AddToLog("Utiliser la requête Access pour éviter les doublons...", eLogType.Warning)
		End If
	End Sub
	Sub MnuCardsExtractDiff2Click(sender As Object, e As EventArgs)
		If Not VmDB Is Nothing Then
			Call Me.ExtractCards("Select Distinct Card.Title From Card Inner Join CardFR On Card.EncNbr = CardFR.EncNbr Where Card.Title = CardFR.TitleFR And Not Card.Series In ('UG', 'UH', 'V1', 'V2', 'V3', 'D1', 'D2', 'D3','D4', 'D5', 'D6', 'TD', 'CH', 'AL', 'BE', 'UN', 'AN', 'AQ', 'LE', 'DK', 'FE');")
		End If
	End Sub
	Sub MnuCardsExtractDiff3Click(sender As Object, e As EventArgs)
		If Not VmDB Is Nothing Then
			Call Me.ExtractCards("Select Distinct Card.Title From Card Inner Join TextesFR On Card.Title = TextesFR.CardName Where Card.CardText = TextesFR.TexteFR And Trim(Card.CardText) <> """" And Card.CardText <> Null;")
		End If
	End Sub
	Sub MnuCardsExtractDiff4Click(sender As Object, e As EventArgs)
		If Not VmDB Is Nothing Then
			Call Me.ExtractCards("Select Distinct Card.Title From Card Where Not Exists (Select Autorisations.Title From Autorisations Where Card.Title = Autorisations.Title) Order By Card.Title Asc;")
		End If
	End Sub
	Sub MnuCardsExtractDiff5Click(sender As Object, e As EventArgs)
		If Not VmDB Is Nothing Then
			Call Me.ExtractCardsPricesAborted
		End If
	End Sub
	Sub MnuCardsAutClick(sender As Object, e As EventArgs)
		If Not VmDB Is Nothing Then
			Call Me.UpdateAutorisations(True)
		End If
	End Sub
	Sub MnuCardsAutListeClick(sender As Object, e As EventArgs)
		If Not VmDB Is Nothing Then
			Call Me.UpdateAutorisations(False)
		End If
	End Sub
	Sub WbMVDocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs)
		VmIsComplete = True
	End Sub
	Sub MnuSeriesGenR14Click(sender As Object, e As EventArgs)
		If Not VmDB Is Nothing Then
			Call Me.BuildHeaders(True)
		End If
	End Sub
	Sub MnuSeriesGenR16Click(sender As Object, e As EventArgs)
		If Not VmDB Is Nothing Then
			Call Me.BuildHeaders(False)
		End If
	End Sub
	Sub MnuPicturesUpdateClick(sender As Object, e As EventArgs)
		Call Me.DownloadPictures
	End Sub
	Sub MnuExtractTextsClick(sender As Object, e As EventArgs)
		If Not VmDB Is Nothing Then
			Call Me.ExtractTexts
		End If
	End Sub
	Sub MnuBuildPatchClick(sender As Object, e As EventArgs)
		If Not VmDB Is Nothing Then
			If MessageBox.Show("Cette opération va détruire des tables dans la base en cours..." + vbCrLf + "Continuer ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = System.Windows.Forms.DialogResult.Yes Then
				Call Me.BuildPatch
			End If
		End If
	End Sub
	Sub MnuDBReadyClick(sender As Object, e As EventArgs)
		If Not VmDB Is Nothing Then
			If MessageBox.Show("Cette opération va détruire des champs dans la base en cours..." + vbCrLf + "Continuer ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = System.Windows.Forms.DialogResult.Yes Then
				Call Me.ReadyForRelease
			End If
		End If
	End Sub
	Sub MnuINIReadyClick(sender As Object, e As EventArgs)
		Call Me.ReadyINI
	End Sub
	Sub MnuFilterTitlesClick(sender As Object, e As EventArgs)
		Call Me.ExtractTitles
	End Sub
	Sub MnuPicturesNewSPClick(sender As Object, e As EventArgs)
		Call Me.BuildSP
	End Sub
	Sub MnuPicturesRevertSPClick(sender As Object, e As EventArgs)
		Call Me.RevertSP
	End Sub
	Sub MnuBuildTitlesClick(sender As Object, e As EventArgs)
		If Not VmDB Is Nothing Then
			Call Me.BuildTitles
		End If
	End Sub
	Sub MnuBuildDoubleClick(sender As Object, e As EventArgs)
		If Not VmDB Is Nothing Then
			Call Me.BuildDouble
		End If
	End Sub
	Sub MnuFindHolesClick(sender As Object, e As EventArgs)
		If Not VmDB Is Nothing Then
			Call Me.FindHoles
		End If
	End Sub
	Sub MnuFixTxtVOClick(sender As Object, e As EventArgs)
		If Not VmDB Is Nothing Then
			Call Me.FixTxtVO
		End If
	End Sub
	Sub MnuCardReplaceTitleClick(sender As Object, e As EventArgs)
		If Not VmDB Is Nothing Then
			Call Me.ReplaceTitle
		End If
	End Sub
	Sub MnuSeriesSpoilersClick(sender As Object, e As EventArgs)
	Dim VpCode As String
		VpCode = InputBox("Quel est le code de la nouvelle série à ajouter ?", "Récupération des spoilers", "(code)")
		If VpCode <> "" Then
			Me.dlgBrowse.SelectedPath = ""
			Me.dlgBrowse.ShowDialog
			If Me.dlgBrowse.SelectedPath <> "" Then
				Call Me.DownloadSpoilers(VpCode)
			End If
		End If
	End Sub
End Class