'------------------------------------------------------
'| Projet         |  Magic The Gathering Manager      |
'| Contexte       |  		Perso                     |
'| Date           |                        30/03/2008 |
'| Release 1      |                        12/04/2008 |
'| Release 2      |                        30/08/2008 |
'| Release 3      |                        08/11/2008 |
'| Release 4      |                        29/08/2009 |
'| Release 5      |                        21/03/2010 |
'| Release 6      |                        17/04/2010 |
'| Release 7      |                        29/07/2010 |
'| Release 8      |                        03/10/2010 |
'| Auteur         |                          Couitchy |
'|----------------------------------------------------|
'| Modifications :                                    |
'| - téléchargement auto des dépendances   10/10/2009 |
'| - prévention des exécutions multiples   31/01/2010 |
'------------------------------------------------------
Imports System.Data
Imports System.Data.OleDb
Imports System.Text
Imports System.Net
Imports System.IO
Imports System.ComponentModel
Public Module clsModule
	Public Declare Function GetPrivateProfileString   Lib "kernel32" Alias "GetPrivateProfileStringA" (lpApplicationName As String, lpKeyName As String, lpDefault As String, lpReturnedString As StringBuilder, nSize As Integer, lpFileName As String) As Integer
	Public Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA"(lpApplicationName As String, lpKeyName As String, lpString As String, ByVal lpFileName As String) As Integer
	Public Declare Function OpenIcon 				  Lib "user32" (ByVal hwnd As Long) As Long
	Public Declare Function SetForegroundWindow 	  Lib "user32" (ByVal hwnd As Long) As Long
	Public Const CgProject As String			= "Magic_The_Gathering_Manager.MainForm"
	Public Const CgMe As String					= "Moi"
	Public Const CgStrConn As String      		= "Provider=Microsoft.Jet.OLEDB.4.0;OLE DB Services=-1;Data Source="
	Public Const CgCodeLines As Integer   		= 21390
	Public Const CgNCriterions As Integer 		= 8
	Public Const CgNDispMenuBase As Integer 	= 3
	Public Const CgShuffleDepth As Integer		= 4
	Public Const CgNMain As Integer				= 7
	Public Const CgNLives As Integer			= 20
	Public Const CgMaxPot As Integer			= 100
	Public Const CgPertinCoeff As Integer		= 4
	Public Const CgMaxGraphs As Integer			= 128
	Public Const CgMissingTable As Long			= -2147217865
	Public Const CgImgMinLength As Long			= 296297676
	Public Const CgTimeOut As Integer			= 5
	Public Const CgIcons As String        		= "\Ressources"
	Public Const CgMagicBack As String      	= "\Ressources\Magic Back.jpg"
	Public Const CgMDB As String				= "\Cartes\Magic DB.mdb"
	Public Const CgDAT As String				= "\Cartes\Images DB.dat"
	Public Const CgINIFile As String      		= "\MTGM.ini"
	Public Const CgHLPFile As String      		= "\MTGM.pdf"
	Public Const CgHSTFile As String      		= "\Historique.txt"
	Public Const CgUpdater As String      		= "\Updater.exe"
	Public Const CgUpDFile As String      		= "\Magic The Gathering Manager.new"
	Public Const CgDownDFile As String     		= "\Magic The Gathering Manager.bak"
	Public Const CgUpDDB As String      		= "\Images DB.mdb"
	Public Const CgUpDDBb As String      		= "\Patch.mdb"
	Public Const CgUpDDBd As String      		= "Images%20DB.dat"
	Public Const CgUpTXTFR As String			= "\TextesVF.txt"
	Public Const CgUpSeries As String			= "\Series.txt"
	Public Const CgUpAutorisations As String	= "\Tournois.txt"
	Public Const CgUpPrices As String			= "\LastPrices.txt"
	Public Const CgUpPic As String				= "\SP_Pict"
	Public Const CgMdPic As String				= "MD_Pict"
	Public Const CgMdTrad As String				= "\MD_Trad.log"
	Public Const CgShell As String				= "explorer.exe"
	Public Const CgURL1 As String         		= "http://couitchy.free.fr/upload/MTGM/Updates/TimeStamp r4.txt"
	Public Const CgURL1B As String         		= "http://couitchy.free.fr/upload/MTGM/Updates/Beta/TimeStamp.txt"
	Public Const CgURL1C As String         		= "http://couitchy.free.fr/upload/MTGM/Updates/PicturesStamp.txt"
	Public Const CgURL1D As String         		= "http://couitchy.free.fr/upload/MTGM/Updates/ContenuStamp.txt"
	Public Const CgURL1E As String         		= "http://couitchy.free.fr/upload/MTGM/Updates/ContenuSizes.txt"
	Public Const CgURL2 As String         		= "http://couitchy.free.fr/upload/MTGM/Updates/Magic The Gathering Manager r4.new"
	Public Const CgURL2B As String         		= "http://couitchy.free.fr/upload/MTGM/Updates/Beta/Magic The Gathering Manager.new"
	Public Const CgURL3 As String         		= "http://couitchy.free.fr/upload/MTGM/Updates/Images DB.mdb"
	Public Const CgURL3B As String         		= "http://couitchy.free.fr/upload/MTGM/Updates/Patch.mdb"
	Public Const CgURL4 As String				= "http://couitchy.free.fr/upload/MTGM/Listes%20des%20editions/"
	Public Const CgURL5 As String				= "http://couitchy.free.fr/upload/MTGM/Logos%20des%20editions/"
	Public Const CgURL6 As String				= "http://gatherer.wizards.com/Pages/Default.aspx"
	Public Const CgURL7 As String         		= "http://couitchy.free.fr/upload/MTGM/Updates/Historique.txt"
	Public Const CgURL8 As String         		= "http://couitchy.free.fr/upload/MTGM/Lib/"
	Public Const CgURL9 As String         		= "http://couitchy.free.fr/upload/MTGM/Updates/LastPrices.txt"
	Public Const CgURL10 As String				= "http://couitchy.free.fr/upload/MTGM/Images%20des%20cartes/"
	Public Const CgURL11 As String         		= "http://couitchy.free.fr/upload/MTGM/Updates/TextesVF.txt"
	Public Const CgURL12 As String         		= "http://couitchy.free.fr/upload/MTGM/Updates/Series r8.txt"
	Public Const CgURL13 As String         		= "http://couitchy.free.fr/upload/MTGM/Updates/MTGM.pdf"
	Public Const CgURL14 As String         		= "http://couitchy.free.fr/upload/MTGM/Updates/MD_Trad.log"
	Public Const CgURL15 As String         		= "http://couitchy.free.fr/upload/MTGM/Updates/Tournois.txt"
	Public Const CgURL16 As String				= "https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=couitchy@free.fr&lc=FR&item_name=Magic The Gathering Manager&currency_code=EUR&bn=PP%2dDonationsBF"
	Public Const CgURL17 As String				= "http://mtgm.free.fr"
	Public Const CgURL18 As String				= "mailto:couitchy@free.fr?subject=Magic The Gathering Manager&body=Votre message ici"
	Public Const CgDL1 As String         		= "Vérification des mises à jour..."
	Public Const CgDL2 As String         		= "Téléchargement en cours"
	Public Const CgDL2b As String         		= "Un téléchargement est déjà en cours..." + vbCrLf + "Veuillez attendre qu'il se termine avant de réessayer."
	Public Const CgDL2c As String 				= "Une mise à jour des images est en cours." + vbCrLf + "Veuillez patienter avant d'essayer de les utiliser..."
	Public Const CgDL3 As String         		= "Erreur lors du téléchargement"
	Public Const CgDL3b As String				= "La connexion au serveur a échoué..." + vbCrLf + "Vérifier la connectivité à Internet et les paramètres du pare-feu."
	Public Const CgDL4 As String         		= "Téléchargement terminé"
	Public Const CgDL5 As String         		= "Téléchargement annulé"
	Public Const CgErr1 As String				= "Les modèles de simulation sont absents ou incomplets..." + vbCrLf + "Procédez à la mise à jour depuis le menu 'Fichier' de la fenêtre principale..."
	Public Const CgErr2 As String				= "L'historique des prix est vide..."
	Public Const CgErr3 As String				= "Impossible d'afficher les informations demandées maintenant..." + vbCrLf + "Si une mise à jour est en cours, merci d'attendre qu'elle se finisse."
	Public Const CgErr4 As String				= "Le nombre maximal de courbes affichables a été atteint..." + vbCrLf + "Les suivantes seront ignorées."
	Public Const CgErr5 As String				= "Le processus de mise à jour a été interrompu..."
	Public Const CgFExtO As String				= ".dck"
	Public Const CgFExtN As String				= ".dk2"
	Public Const CgFExtA As String				= ".dec"
	Public Const CgFExtD As String				= ".mdb"
	Public Const CgIconsExt As String			= ".png"
	Public Const CgPicUpExt As String			= ".dat"
	Public Const CgPicLogExt As String			= ".log"
	Public Const CgImgSeries As String			= "_series_"
	Public Const CgImgColors As String			= "_colors_"
	Public Const CgDefaultName As String		= "MonJeu"
	Public Const CgStats As String				= "Statistiques : "
	Public Const CgSimus As String				= "Simulations : "
	Public Const CgSimus2 As String				= "Proba. partielle pour "
	Public Const CgSimus3 As String				= "Proba. du combo pour "
	Public Const CgSimus4 As String				= "Manas productibles pour "
	Public Const CgSimus5 As String				= "Défaut de manas pour "
	Public Const CgRefresh As String			= "Rafraîchir"
	Public Const CgPanel As String				= "Ouvrir / fermer panneau image"
	Public Const CgStock As String				= "Nombre déjà en stock"
	Public Const CgCollection As String	  		= "Collection"
	Public Const CgSCollection As String  		= "MyCollection"
	Public Const CgDecks As String		  		= "Decks"
	Public Const CgSDecks As String		  		= "MyGames"
	Public Const CgFromSearch As String			= "Recherche"
	Public Const CgSFromSearch As String		= "MySearch"
	Public Const CgCard As String		  		= "(carte)"
	Public Const CgPerfsEfficiency As String 	= "Calcul du facteur d'efficacité" + vbCrLf + "----------------------------------" + vbCrLf + "NB. Ce calcul n'a de sens que si tous les jeux en présence ont été saisis dans la base (afin d'en connaître leur prix)." + vbCrLf + "~1, le jeu est à la hauteur de son prix (jeu normal)" + vbCrLf + "<1, le jeu gagne plus de parties qu'il n'en devrait compte tenu de son prix (jeu efficient)" + vbCrLf + ">1, le jeu gagne moins de parties qu'il n'en devrait compte tenu de son prix (jeu soit mauvais / soit ""bulldozer"")" + vbCrLf + "(un résultat négatif signifie qu'il manque des informations pour le calcul : prix du jeu, résultats de parties...)" + vbCrLf + vbCrLf
	Public Const CgTransactionsMV As String		= "Transaction(s) à effectuer :"
	Public Const CgPerfsVersion As String 		= "nouv."
	Public Const CgPerfsTotal As String   		= "TOTAL "
	Public Const CgPerfsTotalV As String  		= "toutes"
	Public Const CgPerfsVFree As String   		= "sans version"
	Public Const CgAlternateStart As String 	= "Card Name:"
	Public Const CgAlternateStart2 As String	= "Name:"
	Public CgBalises() As String 				= {"CardName:", "Cost:", "Type:", "Pow/Tgh:", "Rules Text:"}
	Public CgManaParsing() As String 			= {"to your mana pool", "add", "either ", " or ", " colorless mana", " mana of any color", " mana"}
	Public CgCriterionsFields() As String 		= {"", "Card.Type", "Spell.Color", "Card.Series", "Spell.myCost", "Card.Rarity", "Card.myPrice", "Card.Title"}
	Public CgNumbers() As String 				= {"one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten"}
	Public CgSearchFields() As String 			= {"Card.Title", "CardFR.TitleFR", "Card.CardText", "TextesFR.TexteFR", "Creature.Power", "Creature.Tough", "Card.Price", "Card.Series", "Spell.myCost", "Card.SubType"}
	Public CgRequiredFiles() As String			= {"\TreeViewMS.dll", "\ChartFX.Lite.dll", "\NPlot.dll", "\SandBar.dll", "\SourceGrid2.dll", "\SourceLibrary.dll", clsModule.CgINIFile, clsModule.CgMagicBack, clsModule.CgUpdater}
	Public CgCriteres As New Hashtable(clsModule.CgNCriterions)
	Public CgVirtualPath As String
	Public VgDB As OleDbConnection
	Public VgDBCommand As New OleDbCommand
	Public VgDBReader As OleDbDataReader
	Public VgImgSeries As New ImageList
	Public VgRemoteDate As Date
	Public VgOptions As New Options
	Public WithEvents VgTray As NotifyIcon
	Public WithEvents VgTimer As Timer
	Public WithEvents VgClient As New WebClient
	Public Enum eUpdateType
		Release = 0
		Beta
		Contenu
	End Enum
	Public Enum eSearchCriterion
		NomVO = 0
		NomVF
		TexteVO
		TexteVF
		Force
		Endurance
		Prix
		Edition
		Cout
	End Enum
	Public Enum eSortCriteria
		Price
		Quality
		Seller
	End Enum
	Public Enum eBasketMode
		Local
		MV
	End Enum
	Public Enum eQuality
		Mint = 0
		NearMint
		Excellent
		Played
		Poor
	End Enum
	Public Enum eDBVersion
		Unknown	= 0	'version inconnue (base corrompue)
		BDD_v1		'manque Adversaires, manque Historique prix, Autorisations, TextesFR, jeux indépendants dans MyScores, SpecialUse et MySpecialUses, MyGamesID et MyScores (+ éventuellement CardPictures, mais non géré, réinstallation par l'utilisateur nécessaire)
		BDD_v2		'manque Adversaires, manque Historique prix, Autorisations, TextesFR, jeux indépendants dans MyScores, SpecialUse et MySpecialUses, MyGamesID et les versions dans MyScores
		BDD_v3		'manque Adversaires, manque Historique prix, Autorisations, TextesFR, jeux indépendants dans MyScores, SpecialUse et MySpecialUses, MyGamesID
		BDD_v4		'manque Adversaires, manque Historique prix, Autorisations, TextesFR, jeux indépendants dans MyScores, SpecialUse et MySpecialUses
		BDD_v5		'manque Adversaires, manque Historique prix, Autorisations, TextesFR
		BDD_v6		'manque Adversaires, manque Historique prix, Autorisations
		BDD_v7		'manque Adversaires, manque Historique prix
		BDD_v8		'manque Adversaires
		BDD_v9		'à jour
	End Enum
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
		For Each VpFile As String In clsModule.CgRequiredFiles
			'Si le fichier n'existe pas
			If Not File.Exists(Application.StartupPath + VpFile) Then
				'Essaie de le télécharger
				Call clsModule.DownloadNow(New Uri(CgURL8 + VpFile.Replace("\", "")), VpFile)
				'Si le fichier n'existe toujours pas, on ne démarre pas
				If Not File.Exists(Application.StartupPath + VpFile) Then
					Call clsModule.ShowWarning("Des fichiers nécessaires à l'exécution sont manquants...")
					Return False
				End If
			End If
		Next VpFile
		Return True
	End Function
	Public Function DBOpen(VpPath As String) As Boolean
	'--------------------------------------------------------------------------------
	'Essaie d'ouvrir la base de données spécifiée en paramètre et renvoie le résultat
	'--------------------------------------------------------------------------------
		If Not IO.File.Exists(VpPath) Then
			Return False
		Else
			VgDB = New OleDbConnection(CgStrConn + VpPath)
	    	Try
		    	VgDB.Open
		    	VgDBCommand.Connection = VgDB
		    	VgDBCommand.CommandType = CommandType.Text
		    	If Not clsModule.DBVersion Then
					VgDB.Close
					VgDB.Dispose
					VgDB = Nothing
		    		Return False
		    	Else
		    		Return True
		    	End If
	    	Catch
	    		clsModule.ShowWarning("Impossible d'ouvrir la base de données sélectionnée...")
	    	End Try
	    End If
	    Return False
	End Function
	Private Function DBVersion As Boolean
	'------------------------------------
	'Vérifie la version de la BDD ouverte
	'------------------------------------
	Dim VpDBVersion As eDBVersion = eDBVersion.Unknown
	Dim VpSchemaTable As DataTable = VgDB.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, New Object() {Nothing, Nothing, Nothing, "TABLE"})
	Dim VpTablesCount As Integer = VpSchemaTable.Rows.Count
		'Détermination de la version de la base de données suivant le nombre d'éléments qu'elle contient
		If VpTablesCount >= 8 Then
			VpSchemaTable = VgDB.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, New Object() {Nothing, Nothing, "MyScores", Nothing})
			If Not VpSchemaTable.Rows(1)!COLUMN_NAME.ToString.StartsWith("JeuAdverse") Then
				'Si on est ici, BDD version 2
				VpDBVersion = eDBVersion.BDD_v2
			ElseIf VpTablesCount < 10 Then
				'Si on est ici, BDD version 3
				VpDBVersion = eDBVersion.BDD_v3
			ElseIf VpTablesCount < 12 Then
				'Si on est ici, BDD version 4
				VpDBVersion = eDBVersion.BDD_v4
			ElseIf VpTablesCount < 13 Then
				'Si on est ici, BDD version 5
				VpDBVersion = eDBVersion.BDD_v5
			ElseIf VpTablesCount < 14 Then
				'Si on est ici, BDD version 6
				VpDBVersion = eDBVersion.BDD_v6
			ElseIf VpTablesCount < 15 Then
				'Si on est ici, BDD version 7
				VpDBVersion = eDBVersion.BDD_v7
			ElseIf VpTablesCount < 16 Then
				'Si on est ici, BDD version 8
				VpDBVersion = eDBVersion.BDD_v8
			Else
				'Si on est ici, BDD version 9
				VpDBVersion = eDBVersion.BDD_v9
			End If
		Else
			'Si on est ici, BDD version 1
			VpDBVersion = eDBVersion.BDD_v1
		End If
		'Actions à effectuer en conséquence
		If VpDBVersion = eDBVersion.Unknown Then		'Version inconnue
			Return False
		ElseIf VpDBVersion = eDBVersion.BDD_v9 Then		'Dernière version
			Return True
		Else											'Versions intermédiaires
			If ShowQuestion("La base de données (v" + CInt(VpDBVersion).ToString + ") doit être mise à jour pour devenir compatible avec la nouvelle version du logiciel (v9)..." + vbCrlf + "Continuer ?") = DialogResult.Yes Then
				Try
					'Passage version 1 à 2
					If CInt(VpDBVersion) < 2 Then
						VgDBCommand.CommandText = "Create Table MyScores (JeuLocal Text(50) With Compression, JeuAdverse Text(50) With Compression, Victoire Bit);"
						VgDBCommand.ExecuteNonQuery
					End If
					'Passage version 2 à 3
					If CInt(VpDBVersion) < 3 Then
						VgDBCommand.CommandText = "Alter Table MyScores Add JeuLocalVersion Text(10) With Compression;"
						VgDBCommand.ExecuteNonQuery
						VgDBCommand.CommandText = "Alter Table MyScores Add JeuAdverseVersion Text(10) With Compression;"
						VgDBCommand.ExecuteNonQuery
					End If
					'Passage version 3 à 4
					If CInt(VpDBVersion) < 4 Then
						Try
							VgDBCommand.CommandText = "Create Table MyGamesID (GameID Integer, GameName Text(50) With Compression);"
							VgDBCommand.ExecuteNonQuery
							For VpI As Integer = 0 To VgOptions.VgSettings.NJeux - 1
								VgDBCommand.CommandText = "Insert Into MyGamesID Values (" + VpI.ToString + ", '" + VgOptions.GetDeckName_INI(VpI + 1).Replace("'", "''") + "');"
								VgDBCommand.ExecuteNonQuery
							Next VpI
						Catch
						End Try
					End If
					'Passage version 4 à 5
					If CInt(VpDBVersion) < 5 Then
						VgDBCommand.CommandText = "Create Table MySpecialUses (EffortID Integer, EffetID Integer, Card Text(80) With Compression, Effort Text(255) With Compression, Effet Text(255) With Compression);"
						VgDBCommand.ExecuteNonQuery
						VgDBCommand.CommandText = "Create Table SpecialUse (SpecID Integer, IsEffort Bit, Description Text(255) With Compression);"
						VgDBCommand.ExecuteNonQuery
						VgDBCommand.CommandText = "Alter Table MyScores Add IsMixte Bit;"
						VgDBCommand.ExecuteNonQuery
					End If
					'Passage version 5 à 6
					If CInt(VpDBVersion) < 6 Then
						VgDBCommand.CommandText = "Create Table TextesFR (CardName Text(80) With Compression, TexteFR Memo);"
						VgDBCommand.ExecuteNonQuery
					End If
					'Passage version 6 à 7
					If CInt(VpDBVersion) < 7 Then
						VgDBCommand.CommandText = "Create Table Autorisations (Title Text(80) With Compression, T1 Bit, T1r Bit, T15 Bit, T1x Bit, T2 Bit, Bloc Bit);"
						VgDBCommand.ExecuteNonQuery
					End If
					'Passage version 7 à 8
					If CInt(VpDBVersion) < 8 Then
						VgDBCommand.CommandText = "Create Table PricesHistory (EncNbr Long, PriceDate Date, Price Single);"
						VgDBCommand.ExecuteNonQuery
					End If
					'Passage version 8 à 9
					If CInt(VpDBVersion) < 9 Then
						VgDBCommand.CommandText = "Create Table MyAdversairesID (AdvID Long, AdvName Text(255) With Compression);"
						VgDBCommand.ExecuteNonQuery						
						VgDBCommand.CommandText = "Insert Into MyAdversairesID(AdvID, AdvName) Values (0, '" + clsModule.CgMe + "');"
						VgDBCommand.ExecuteNonQuery						
						VgDBCommand.CommandText = "Alter Table MyGamesID Add AdvID Long;"
						VgDBCommand.ExecuteNonQuery						
						VgDBCommand.CommandText = "Update MyGamesID Set AdvID = 0;"
						VgDBCommand.ExecuteNonQuery			
						VgDBCommand.CommandText = "Alter Table MyScores Drop Column IsMixte;"
						VgDBCommand.ExecuteNonQuery
					End If					
				Catch
					Call clsModule.ShowWarning("Un problème est survenu pendant la mise à jour de la base de données...")
					Return False
				End Try
				Return True
			Else
				Return False
			End If
		End If
	End Function
	Public Function DBOK As Boolean
		If VgDB Is Nothing Then
			Call clsModule.ShowWarning("Aucune base de données n'a été sélectionnée...")
			Return False
		ElseIf MainForm.VgMe.IsMainReaderBusy Then
			Return False
		Else
			Return True
		End If
	End Function
	Public Sub DBImport(VpPath As String, Optional VpSilent As Boolean = False)
	'---------------------------------------------------------------------------------
	'Importe dans la base de données l'ensemble du patch (structure + enregistrements)
	'---------------------------------------------------------------------------------
	'TABLE_CATALOG
	'TABLE_SCHEMA
	'TABLE_NAME
	'COLUMN_NAME
	'COLUMN_GUID
	'COLUMN_PROPID
	'ORDINAL_POSITION
	'COLUMN_HASDEFAULT
	'COLUMN_DEFAULT
	'COLUMN_FLAGS
	'IS_NULLABLE
	'DATA_TYPE
	'TYPE_GUID
	'CHARACTER_MAXIMUM_LENGTH
	'CHARACTER_OCTET_LENGTH
	'NUMERIC_PRECISION
	'NUMERIC_SCALE
	'DATETIME_PRECISION
	'CHARACTER_SET_CATALOG
	'CHARACTER_SET_SCHEMA
	'CHARACTER_SET_NAME
	'COLLATION_CATALOG
	'COLLATION_SCHEMA
	'COLLATION_NAME
	'DOMAIN_CATALOG
	'DOMAIN_SCHEMA
	'DOMAIN_NAME
	'DESCRIPTION
	'---------------------------------------------------------------------------------
	Dim VpDB As New OleDbConnection(CgStrConn + VpPath)
	Dim VpTable As String
	Dim VpTables As DataTable
	Dim VpSchemaTable As DataTable
	Dim VpType As OleDbType
	Dim VpSQL As String
	Dim VpDA1 As New OleDbDataAdapter
	Dim VpDA2 As New OleDbDataAdapter
	Dim VpDS1 As New DataSet
	Dim VpDS2 As New DataSet
	Dim VpDBCommand As New OleDbCommand
	Dim VpBuilder As New OleDbCommandBuilder(VpDA2)
	Dim VpRow As DataRow
	Dim VpCurTable As DataTable
		VpDB.Open
		VpDBCommand.Connection = VpDB
		VpDBCommand.CommandType = CommandType.Text
		VpTables = VpDB.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, New Object() {Nothing, Nothing, Nothing, "TABLE"})
		'Pour chaque table du patch
		For VpI As Integer = 0 To VpTables.Rows.Count - 1
			VpTable = VpTables.Rows(VpI)!TABLE_NAME.ToString
			'Si la table existe déjà dans la base mais qu'elle doit être préalablement détruite
			If clsModule.PreDelete(VpTable) Then
				Try
					VgDBCommand.CommandText = "Drop Table " + VpTable + ";"
					VgDBCommand.ExecuteNonQuery
				Catch
				End Try
			End If
			'Si la table n'existe pas (ou plus) dans la base
			If Not clsModule.IsInDB(VpTable) Then
				'Prépare la requête de création de table (méthode SQL)
				VpSQL = "Create Table " + VpTable + "("
				VpSchemaTable = VpDB.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, New Object() {Nothing, Nothing, VpTable, Nothing})
				For VpK As Integer = 0 To VpSchemaTable.Rows.Count - 1
					For VpJ As Integer = 0 To VpSchemaTable.Rows.Count - 1
						If VpSchemaTable.Rows(VpJ)!ORDINAL_POSITION - 1 = VpK Then			'Respecte la position ordinale des champs
							VpSQL = VpSQL + VpSchemaTable.Rows(VpJ)!COLUMN_NAME.ToString
							VpType = VpSchemaTable.Rows(VpJ)!DATA_TYPE
							If VpType = OleDbType.WChar Then
								VpSQL = VpSQL + " VarChar(" + VpSchemaTable.Rows(VpJ)!CHARACTER_MAXIMUM_LENGTH.ToString + ") With Compression"
							Else
								VpSQL = VpSQL + " " + VpType.ToString.Replace("Boolean", "Bit")
							End If
							VpSQL = VpSQL + ", "
						End If
					Next VpJ
				Next VpK
				VpSQL = VpSQL.Substring(0, VpSQL.Length - 2)
				VgDBCommand.CommandText = VpSQL + ");"
				VgDBCommand.ExecuteNonQuery
				'La table étant présente, on peut insérer les nouvelles données	(méthode OLEDB/DataSet)
				VpDBCommand.CommandText = "Select * From " + VpTable + ";"
				VgDBCommand.CommandText = VpDBCommand.CommandText
				VpDA1.SelectCommand = VpDBCommand
				VpDA2.SelectCommand = VgDBCommand
				VpBuilder.RefreshSchema
				VpDA2.InsertCommand = VpBuilder.GetInsertCommand
				VpDA1.Fill(VpDS1, VpTable)
				VpDA2.Fill(VpDS2, VpTable)
				VpCurTable = VpDS2.Tables.Item(VpDS2.Tables.IndexOf(VpTable))
				For Each VpSrcRow As DataRow In VpDS1.Tables.Item(VpDS1.Tables.IndexOf(VpTable)).Rows
					VpRow = VpCurTable.NewRow
					VpRow.ItemArray = VpSrcRow.ItemArray
					VpCurTable.Rows.Add(VpRow)
				Next VpSrcRow
				VpDA2.Update(VpDS2, VpTable)
			End If
		Next VpI
		'Informe l'utilisateur
		If Not VpSilent Then
			Call clsModule.ShowInformation("Mise à jour effectuée." + vbCrLf + "Assurez-vous d'avoir également la dernière version du logiciel...")
		End If
		'Supprime le patch
		VpDB.Close
		VpDB.Dispose
		VpDB = Nothing
		Call clsModule.SecureDelete(VpPath)
	End Sub
	Private Function PreDelete(VpTable As String) As Boolean
	'---------------------------------------------------------------------------------------------
	'Indique si la table spécifiée en paramètre doit être supprimée lors de l'application du patch
	'---------------------------------------------------------------------------------------------
		Select Case VpTable
			Case "MySpecialUses"
				'Essaie d'utiliser le champ large ; si erreur => on doit recréer la table avec un champ plus grand
				Try
					VgDBCommand.CommandText = "Insert Into MySpecialUses Values (-1, -1, 'X', '" + clsModule.StrBuild("X", 128) + "', '" + clsModule.StrBuild("X", 128) + "', False, False);"
					VgDBCommand.ExecuteNonQuery
					VgDBCommand.CommandText = "Delete * From MySpecialUses Where EffortID = -1;"		'un peu crade mais je n'ai pas trouvé plus simple !
					VgDBCommand.ExecuteNonQuery
				Catch
					Return True
				End Try
				Return False
			Case "SpecialUse"
				Return True
			Case Else
				Return True
		End Select
	End Function
	Private Function IsInDB(VpTable As String) As Boolean
	'------------------------------------------------------------------------------------------------
	'Renvoie faux si la table spécifiée en paramètre n'existe pas ou est vide dans la base de données
	'------------------------------------------------------------------------------------------------
	Dim VpResult As Boolean
		Try
			VgDBCommand.CommandText = "Select * From " + VpTable + ";"
			VgDBReader = VgDBCommand.ExecuteReader
			VgDBReader.Read
			VpResult = True
		Catch
			VpResult = False
		End Try
		If Not VgDBReader.IsClosed Then
			VgDBReader.Close
		End If
		Return VpResult
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
			For Each VpIcon As String In System.IO.Directory.GetFiles(Application.StartupPath + CgIcons, "*" + clsModule.CgIconsExt)
				VpHandle = Image.FromFile(VpIcon)
				VpKey = VpIcon.Substring(VpIcon.LastIndexOf("\") + 1)
				If Not VgImgSeries.Images.Keys.Contains(VpKey) Then
					VgImgSeries.Images.Add(VpKey, VpHandle)
				End If
				If Not VpImgSeries.Images.Keys.Contains(VpKey) Then
					VpImgSeries.Images.Add(VpKey, VpHandle)
				End If
			Next VpIcon
		End If
		Return True
	End Function
	'------------------
	'Boîtes de dialogue
	'------------------
	Public Sub ShowWarning(VpStr As String)
		MessageBox.Show(VpStr, "Problème", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1)
	End Sub
	Public Sub ShowInformation(VpStr As String)
		MessageBox.Show(VpStr, "Information", MessageBoxbuttons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
	End Sub
	Public Function ShowQuestion(VpStr As String, Optional VpButtons As MessageBoxButtons = MessageBoxbuttons.YesNo) As DialogResult
		Return MessageBox.Show(VpStr, "Question", VpButtons, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
	End Function
	Public Function Matching(VpStr As String) As Object
	'-------------------------------------------
	'Adapte une chaîne en un type plus approprié
	'-------------------------------------------
		If VpStr = "True" Then
			Return True
		ElseIf VpStr = "False" Then
			Return False
		ElseIf VpStr = "Normal" Then
			Return FormWindowState.Normal
		ElseIf VpStr = "Maximized" Then
			Return FormWindowState.Maximized
		ElseIf VpStr = "Minimized" Then
			Return FormWindowState.Minimized
		ElseIf VpStr = "AutoSize" Then
			Return PictureBoxSizeMode.AutoSize
		ElseIf VpStr = "CenterImage" Then
			Return PictureBoxSizeMode.CenterImage
		ElseIf VpStr = "Normal" Then
			Return PictureBoxSizeMode.Normal
		ElseIf VpStr = "StretchImage" Then
			Return PictureBoxSizeMode.StretchImage
		ElseIf VpStr = "Zoom" Then
			Return PictureBoxSizeMode.Zoom
		ElseIf VpStr = "NomVO" Then
			Return eSearchCriterion.NomVO
		ElseIf VpStr = "NomVF" Then
			Return eSearchCriterion.NomVF
		ElseIf VpStr = "TexteVO" Then
			Return eSearchCriterion.TexteVO
		ElseIf VpStr = "TexteVF" Then
			Return eSearchCriterion.TexteVF
		ElseIf VpStr = "Force" Then
			Return eSearchCriterion.Force
		ElseIf VpStr = "Endurance" Then
			Return eSearchCriterion.Endurance
		ElseIf VpStr = "Prix" Then
			Return eSearchCriterion.Prix
		ElseIf VpStr = "Edition" Then
			Return eSearchCriterion.Edition
		ElseIf VpStr = "Cout" Then
			Return eSearchCriterion.Cout
		ElseIf IsNumeric(VpStr)
			Return CInt(VpStr)
		Else
			Return VpStr
		End If
	End Function
	Public Function TrimQuery(VpSQL As String, Optional VpDot As Boolean = True, Optional VpAddendum As String = "") As String
	'----------------------------------
	'Suppression des mots-clés inutiles
	'----------------------------------
		If VpSQL.EndsWith(" Where ") Then
			Return VpSQL.Substring(0, VpSQL.Length - 7) + VpAddendum + IIf(VpDot, ";", "")
		ElseIf VpSQL.EndsWith(" And ") Then
			Return VpSQL.Substring(0, VpSQL.Length - 5) + VpAddendum + IIf(VpDot, ";", "")
		Else
			Return VpSQL + VpAddendum + IIf(VpDot, ";", "")
		End If
	End Function
	Public Function ColorTo1(VpColor As String) As String
	'----------------------------------------
	'Description de la couleur en 1 caractère
	'----------------------------------------
		VpColor = VpColor.Replace(CgImgColors, "")
		Select Case VpColor
			Case "Noires"
				Return "B"
			Case "Vertes"
				Return "G"
			Case "Rouges"
				Return "R"
			Case "Bleues"
				Return "U"
			Case "Blanches"
				Return "W"
			Case "Terrains"
				Return "L"
			Case "Multicolores"
				Return "M"
			Case "Jetons"
				Return "T"
			Case "Incolores"
				Return "A"
			Case Else
				Return ""
		End Select
	End Function
	Public Function FormatTitle(VpTag As String, VpStr As String, Optional VpIsForTvw As Boolean = True) As String
	'-------------------------------------------------------------------
	'Modifie l'expression passée en paramètre en un titre plus explicite
	'-------------------------------------------------------------------
	Dim VpDBCommand As OleDbCommand
		Select Case VpTag
			Case "Card.Series"
				Try
					VpDBCommand = New OleDbCommand
    				VpDBCommand.Connection = VgDB
    				VpDBCommand.CommandType = CommandType.Text
					VpDBCommand.CommandText = "Select SeriesNM From Series Where SeriesCD = '" + VpStr + "';"
					Return VpDBCommand.ExecuteScalar.ToString
				Catch
					Return VpStr
				End Try
			Case "Card.Type"
				Select Case VpStr.ToUpper
					Case "C"
						Return "Créatures"
					Case "I"
						Return "Ephémères"
					Case "A"
						Return "Artefacts"
					Case "E"
						Return "Enchanter-permanents"
					Case "L"
						Return "Terrains"
					Case "N"
						Return "Interruptions"
					Case "S"
						Return "Rituels"
					Case "T"
 						Return "Enchantements"
 					Case "U"
						Return "Créatures avec capacité"
					Case "P"
						Return "Arpenteurs"
					Case "K"
						Return "Jetons"
					Case Else
						Return VpStr
				End Select
			Case "Spell.Color"
				Select Case VpStr.ToUpper
					Case "A"
						Return "Incolores"
					Case "B"
						Return "Noires"
					Case "G"
						Return "Vertes"
					Case "L"
						Return "Terrains"
					Case "M"
						Return "Multicolores"
					Case "R"
						Return "Rouges"
					Case "U"
						Return "Bleues"
					Case "W"
						Return "Blanches"
					Case "T"
						Return "Jetons"
					'Cas mal géré des double cartes
					Case "X"
						Return "Double-cartes"
					Case Else
						Return VpStr
				End Select
			Case "Spell.myCost"
				Return IIf(VpIsForTvw, "", VpStr)
			Case "Card.myPrice"
				Select Case VpStr
					Case "1"
						Return "Moins de 0,50€"
					Case "2"
						Return "Entre 0,50€ et 1€"
					Case "3"
						Return "Entre 1€ et 3€"
					Case "4"
						Return "Entre 3€ et 5€"
					Case "5"
						Return "Entre 5€ et 10€"
					Case "6"
						Return "Entre 10€ et 20€"
					Case "7"
						Return "Entre 20€ et 50€"
					Case "8"
						Return "Plus de 50 €"
					Case Else
						Return VpStr
				End Select
			Case "Card.Rarity"
				Select Case VpStr.Substring(0, 1).ToUpper
					Case "M"
						Return ("Mythiques (" + VpStr.Substring(1) + ")").Replace("()","")
					Case "R"
						Return ("Rares (" + VpStr.Substring(1) + ")").Replace("()","")
					Case "U"
						Return ("Peu communes (" + VpStr.Substring(1) + ")").Replace("()","")
					Case "C"
						Return ("Communes (" + VpStr.Substring(1) + ")").Replace("()","")
					Case "D", "L", "S"
						Return ("Sans valeur (" + VpStr.Substring(1) + ")").Replace("()","")
					Case Else
						Return VpStr
				End Select
			Case Else
				Return VpStr
		End Select
	End Function
	Public Function ExtractENName(VpStr As String) As String
	Dim VpTitle As String = VpStr
		VpTitle = VpTitle.Substring(VpTitle.IndexOf("(") + 1)
		If VpTitle.Contains("(") Then
			VpTitle = VpTitle.Substring(VpTitle.IndexOf("(") + 1)
		End If
		Return VpTitle.Substring(0, VpTitle.Length - 1)
	End Function
	'---------------------
	'Gestion formats dates
	'---------------------
	Public Function MyCDate(VpStr As String) As Date
		If VpStr <> "" Then
			Return CDate(VpStr)
		Else
			Return Nothing
		End If
	End Function
	Public Function MyShortDateString(VpDate As Date) As String
		If VpDate.Year <> 1 Then
			Return VpDate.ToShortDateString
		Else
			Return CgPerfsVFree
		End If
	End Function
	Public Function MyPrice(VpStr As String) As Integer
	'-------------------------------------------------------
	'Retourne la catégorie du prix correspondant à sa valeur
	'-------------------------------------------------------
    	'(1 [0-0.5] 2 [0.5-1] 3 [1-3] 4 [3-5] 5 [5-10] 6 [10-20] 7 [20-50] 8 [50+])
		Select Case MyVal(VpStr)
			Case Is <= 0.5
				Return 1
			Case Is <= 1
				Return 2
			Case Is <= 3
				Return 3
			Case Is <= 5
				Return 4
			Case Is <= 10
				Return 5
			Case Is <= 20
				Return 6
			Case Is <= 50
				Return 7
			Case Else
				Return 8
		End Select
	End Function
	Public Function MyCost(VpStr As String) As Integer
	'---------------------------------------------------------------------
	'Retourne le coût converti de mana de l'invocation passée en paramètre
	'---------------------------------------------------------------------
	Dim VpColorless As Integer
	    If VpStr = "0" Then
	    	Return 0
	    Else
	        VpColorless = Val(VpStr)
	        If VpColorless <> 0 Then
	            Return VpStr.Replace(VpColorless.ToString.Trim, "").Length + VpColorless - 4 * clsModule.StrCount(VpStr, "(")
	        Else
	            Return VpStr.Length - 4 * clsModule.StrCount(VpStr, "(")
	        End If
	    End If
	End Function
	Public Function MyTxt(VpCard As String, VpVF As Boolean) As String
	'----------------------------------------------------
	'Retourne le texte VF de la carte passée en paramètre
	'----------------------------------------------------
	Dim VpDBCommand As New OleDbCommand
	Dim VpO As Object
    	VpDBCommand.Connection = VgDB
    	VpDBCommand.CommandType = CommandType.Text
    	If VpVF Then
			VpDBCommand.CommandText = "Select TexteFR From TextesFR Where CardName = '" + VpCard.Replace("'", "''") + "';"
    	Else
    		VpDBCommand.CommandText = "Select CardText From Card Where Title = '" + VpCard.Replace("'", "''") + "';"
    	End If
		VpO = VpDBCommand.ExecuteScalar
		If Not VpO Is Nothing Then
			Return VpO.ToString
		Else
			Return ""
		End If
	End Function
	Public Function MyClone(VpA As ArrayList) As ArrayList
	'-------------------------------------------
	'Duplication de la liste des cartes désirées
	'-------------------------------------------
	Dim VpB As New ArrayList
		For Each VpLocalCard As clsLocalCard In VpA
			VpB.Add(New clsLocalCard(VpLocalCard.Name, VpLocalCard.Quantite))
		Next VpLocalCard
		Return VpB
	End Function
	Public Function MyClone2(VpA As ArrayList) As ArrayList
	'-------------------------------------------
	'Duplication de la liste des cartes désirées
	'-------------------------------------------
	Dim VpB As New ArrayList
		For Each VpMVCard As clsMVCard In VpA
			VpB.Add(New clsMVCard(VpMVCard.Name, VpMVCard.Vendeur, VpMVCard.Edition, VpMVCard.Etat, VpMVCard.Quantite, VpMVCard.Prix))
		Next VpMVCard
		Return VpB
	End Function
	Public Function GetDate(VpDate As Date) As String
		Return "'" + VpDate.Day.ToString + "/" + VpDate.Month.ToString + "/" + VpDate.Year.ToString.Substring(2, 2) + "'"
	End Function
	Public Function StrCount(VpStr As String, VpChar As String) As Integer
	'----------------------------------------------------------------------------------------
	'Retourne le nombre d'occurences du caractère spécifié dans la chaîne passée en paramètre
	'----------------------------------------------------------------------------------------
	Dim VpCounter As Integer = 0
		For VpI As Integer = 0 To VpStr.Length - 1
			If VpStr.Substring(VpI, 1) = VpChar Then
				VpCounter = VpCounter + 1
			End If
		Next VpI
		Return VpCounter
	End Function
	Public Function StrBuild(VpChar As String, VpCount As Integer) As String
	'--------------------------
	'Retourne n occurences de c
	'--------------------------
	Dim VpStr As String = ""
		For VpI As Integer = 1 To VpCount
			VpStr = VpStr + VpChar
		Next VpI
		Return VpStr
	End Function
	Public Function MyVal(VpStr As String) As Double
		Return Val(VpStr.Replace(",", "."))
	End Function
	Public Function FindIndex(VpTab() As String, VpValue As String) As Integer
	'--------------------------------------------------------------------------------------------------------------
	'Retourne l'indice de la première occurence de la valeur passée en paramètre dans le tableau passé en paramètre
	'--------------------------------------------------------------------------------------------------------------
		For VpI As Integer = 0 To VpTab.Length - 1
			If VpTab(VpI) = VpValue Then
				Return VpI
			End If
		Next VpI
		Return -1
	End Function
	Public Function FindNumber(VpStr As String) As Integer
	'----------------------------------------------------------------------------------
	'Retourne le nombre correspondant à la chaîne (texte ou nombre) passée en paramètre
	'----------------------------------------------------------------------------------
		If IsNumeric(VpStr) Then
			Return Val(VpStr)
		Else
			Return clsModule.FindIndex(clsModule.CgNumbers, VpStr) + 1
		End If
	End Function
	Public Function SafeGetChecked(VpObj As Object) As Boolean
		Try
			Return VpObj.Checked
		Catch
			Return False
		End Try
	End Function
	Public Function SafeGetText(VpObj As Object) As String
		Try
			Return VpObj.Text
		Catch
			Return ""
		End Try
	End Function
	Public Sub InitCriteres(VpMainForm as MainForm)
		For VpI As Integer = 0 To VpMainForm.chkClassement.Items.Count - 1
			CgCriteres.Add(VpMainForm.chkClassement.Items(VpI), clsModule.CgCriterionsFields(VpI))
		Next VpI
	End Sub
	Public Function GetPictSP As String
	Dim VpRequest As HttpWebRequest
	Dim VpResponse As WebResponse
	Dim VpAnswer As Stream
	Dim VpBuf() As Byte
	Dim VpStamp As String
			VpRequest = WebRequest.Create(clsModule.CgURL1C)
			VpResponse = VpRequest.GetResponse
			VpAnswer = VpResponse.GetResponseStream
			'Lecture du fichier sur Internet
			ReDim VpBuf(0 To VpResponse.ContentLength - 1)
			VpAnswer.Read(VpBuf, 0, VpBuf.Length)
			VpStamp = New ASCIIEncoding().GetString(VpBuf)
			Return VpStamp
	End Function
	Public Sub CheckForPicUpdates
	'-------------------------------------------------------------------------
	'Vérifie si une mise à jour de la base d'image est disponible sur Internet
	'-------------------------------------------------------------------------
	Dim VpStamp As String
	Dim VpStr As String
	Dim VpOldText As String
		VpOldText = MainForm.VgMe.StatusTextGet
		Call MainForm.VgMe.StatusText(clsModule.CgDL1, True)
		VgTimer.Stop
		'Vérification par la taille
		Try
			VpStamp = clsModule.GetPictSP
			VpStr = (New FileInfo(VgOptions.VgSettings.PicturesFile)).Length.ToString
			If VpStamp.Contains(VpStr) Then
				VpStr = VpStamp.Substring(VpStamp.IndexOf(VpStr) + VpStr.Length + 1)
				VpStr = VpStr.Substring(0, VpStr.IndexOf("#"))
				If VpStr = "OK"  Then
					Call clsModule.ShowInformation("Les images sont déjà à jour...")
					Call MainForm.VgMe.StatusText(VpOldText)
				Else
					'Téléchargement du fichier accompagnateur
					Call clsModule.DownloadNow(New Uri(clsModule.CgURL10 + VpStr + clsModule.CgPicLogExt), clsModule.CgUpPic + clsModule.CgPicLogExt)
					Application.DoEvents
					'Téléchargement du service pack d'images
					MainForm.VgMe.IsInImgDL = True
					Call clsModule.DownloadUpdate(New Uri(clsModule.CgURL10 + VpStr + clsModule.CgPicUpExt), clsModule.CgUpPic + clsModule.CgPicUpExt)
				End If
			Else
				If clsModule.ShowQuestion("La base d'images semble être corrompue." + vbCrLf + "Voulez-vous la re-télécharger maintenant (~300 Mo) ?") = System.Windows.Forms.DialogResult.Yes Then
					'Re-téléchargement complet de la base principale
					MainForm.VgMe.IsInImgDL = True
					Call clsModule.DownloadUpdate(New Uri(clsModule.CgURL10 + clsModule.CgUpDDBd), VgOptions.VgSettings.PicturesFile, False)
				Else
					Call MainForm.VgMe.StatusText(VpOldText)
				End If
			End If
		Catch
			'En cas d'échec de connexion
			Call clsModule.ShowWarning(clsModule.CgDL3b)
			Call MainForm.VgMe.StatusText(VpOldText)
		End Try
	End Sub
	Public Sub CheckForUpdates(Optional VpExplicit As Boolean = False, Optional VpBeta As Boolean = False)
	'------------------------------------------------------------------
	'Vérifie si une mise à jour du logiciel est disponible sur Internet
	'------------------------------------------------------------------
	Dim VpRequest As HttpWebRequest
	Dim VpAnswer As Stream
	Dim VpBuf(0 To 18) As Byte
	Dim VpOldText As String
	Dim VpContenuUpdate As frmUpdateContenu
	Dim VpNewContenu As New ArrayList
		VpOldText = MainForm.VgMe.StatusTextGet
		Call MainForm.VgMe.StatusText(clsModule.CgDL1)
		'Fichier d'historique des versions
		Call clsModule.DownloadNow(New Uri(clsModule.CgURL7), clsModule.CgHSTFile)
		'Vérification horodatage
		Try
			VpRequest = WebRequest.Create(IIf(VpBeta, clsModule.CgURL1B, clsModule.CgURL1))
			VpAnswer = VpRequest.GetResponse.GetResponseStream
			'Lecture du fichier horodaté sur Internet
			VpAnswer.Read(VpBuf, 0, 19)
			VgRemoteDate = CDate(New ASCIIEncoding().GetString(VpBuf))
			'Si version plus récente
			If DateTime.Compare(File.GetLastWriteTimeUtc(Application.ExecutablePath), VgRemoteDate) < 0 Then
				VgTray.Visible = True
				VgTray.Tag = IIf(VpBeta, eUpdateType.Beta, eUpdateType.Release)
				VgTray.ShowBalloonTip(10, "Magic The Gathering Manager" + IIf(VpBeta, " BETA", ""), "Une mise à jour de l'application est disponible..." + vbCrLf + "Cliquer ici pour la télécharger, quitter Magic The Gathering Manager et l'installer.", ToolTipIcon.Info)
			ElseIf VpExplicit
				If VpBeta Then
					Call clsModule.ShowInformation("Aucune version bêta postérieure à la dernière release n'est disponible pour l'instant...")
				Else
					Call clsModule.ShowInformation("Vous disposez déjà de la dernière version de Magic The Gathering Manager !")
				End If
			'Recherche automatique des mises à jour de contenu
			ElseIf DBOK Then
				If MainForm.VgMe.MyChildren.DoesntExist(MainForm.VgMe.MyChildren.ContenuUpdater) Then
					VpContenuUpdate = New frmUpdateContenu
					MainForm.VgMe.MyChildren.ContenuUpdater = VpContenuUpdate
				Else
					VpContenuUpdate = MainForm.VgMe.MyChildren.ContenuUpdater
				End If
				If VpContenuUpdate.CheckForContenu(VpNewContenu) Then
					VgTray.Visible = True
					VgTray.Tag = eUpdateType.Contenu
					VgTray.ShowBalloonTip(10, "Magic The Gathering Manager", "Des mises à jour de contenu sont disponibles..." + vbCrLf + "Cliquer ici pour en savoir plus...", ToolTipIcon.Info)
				End If
			End If
		Catch
			'En cas d'échec de connexion, inutile de continuer à checker
			VgTimer.Stop
			If VpExplicit Then
				Call clsModule.ShowWarning(clsModule.CgDL3b)
			End If
		End Try
		Call MainForm.VgMe.StatusText(VpOldText)
	End Sub
	Public Sub NotifyIconBalloonTipClosed(ByVal sender As Object, ByVal e As EventArgs) Handles VgTray.BalloonTipClosed
		VgTray.Visible = False
	End Sub
	Public Sub NotifyIconBalloonTipClicked(ByVal sender As Object, ByVal e As EventArgs) Handles VgTray.BalloonTipClicked
	Dim VpType As eUpdateType = VgTray.Tag
		VgTray.Visible = False
		VgTimer.Stop
		Select Case VpType
			Case eUpdateType.Release
				Call clsModule.DownloadUpdate(New Uri(CgURL2), clsModule.CgUpDFile)
			Case eUpdateType.Beta
				Call clsModule.DownloadUpdate(New Uri(CgURL2B), clsModule.CgUpDFile)
			Case eUpdateType.Contenu
				MainForm.VgMe.MyChildren.ContenuUpdater.Show
				MainForm.VgMe.MyChildren.ContenuUpdater.BringToFront
			Case Else
		End Select
	End Sub
	Public Sub TimerTick(ByVal sender As Object, ByVal e As EventArgs) Handles VgTimer.Tick
		Call CheckForUpdates
		VgTimer.Stop
	End Sub
	Public Sub DownloadUpdate(VpURI As System.Uri, VpOutput As String, Optional VpBaseDir As Boolean = True, Optional VpSilent As Boolean = False)
	'------------------------------------------------------------------------------
	'Télécharge en arrière-plan l'application mise à jour ou une de ses dépendances
	'------------------------------------------------------------------------------
		If MainForm.VgMe.IsDownloadInProgress Then
			If Not MainForm.VgMe.MyChildren.DoesntExist(MainForm.VgMe.MyChildren.ContenuUpdater) Then
				If MainForm.VgMe.MyChildren.ContenuUpdater.PassiveUpdate = frmUpdateContenu.EgPassiveUpdate.InProgress Then
					MainForm.VgMe.MyChildren.ContenuUpdater.PassiveUpdate = frmUpdateContenu.EgPassiveUpdate.Failed
				End If
			End If
			If Not VpSilent Then
				Call clsModule.ShowWarning(clsModule.CgDL2b)
			End If
		Else
			MainForm.VgMe.IsDownloadInProgress = True
			Call MainForm.VgMe.StatusText(clsModule.CgDL2, True)
			Try
				VgClient.DownloadFileAsync(VpURI, IIf(VpBaseDir, Application.StartupPath + VpOutput, VpOutput))
				MainForm.VgMe.btDownload.Visible = True
				MainForm.VgMe.btDownload.Tag = Now
			Catch
			End Try
		End If
	End Sub
	Public Sub DownloadNow(VpURI As System.Uri, VpOutput As String)
	'----------------------------------------------------------------------------
	'Télécharge immédiatement l'application mise à jour ou une de ses dépendances
	'----------------------------------------------------------------------------
		Try
			VgClient.DownloadFile(VpURI, Application.StartupPath + VpOutput)
		Catch' VpEx As Exception
			'MessageBox.Show(VpEx.GetType.FullName)
			'VgClient.DownloadFile(VpURI, clsModule.CgVirtualPath + VpOutput)
		End Try
	End Sub
	Public Sub ClientDownloadProgressChanged(ByVal sender As Object, ByVal e As DownloadProgressChangedEventArgs) Handles VgClient.DownloadProgressChanged
'		MainForm.VgMe.VgBar.Style = ProgressBarStyle.Blocks
		MainForm.VgMe.prgAvance.Maximum = 100
'		MainForm.VgMe.VgBar.Maximum = 100
		MainForm.VgMe.prgAvance.Value = e.ProgressPercentage
'		MainForm.VgMe.VgBar.Value = e.ProgressPercentage
		MainForm.VgMe.prgAvance.Visible = True
'		MainForm.VgMe.VgBar.ShowInTaskbar = True
	End Sub
	Public Sub ClientDownloadFileCompleted(ByVal sender As Object, ByVal e As AsyncCompletedEventArgs) Handles VgClient.DownloadFileCompleted
	'------------------------------------------------------------
	'Installe l'application mise à jour ou une de des dépendances
	'------------------------------------------------------------
	Dim VpResult As WebException = e.Error
		MainForm.VgMe.IsInImgDL = False
		MainForm.VgMe.IsDownloadInProgress = False
		MainForm.VgMe.btDownload.Visible = False
		MainForm.VgMe.prgAvance.Visible = False
'		MainForm.VgMe.VgBar.ShowInTaskbar = False
		'Gestion des erreurs
		If Not VpResult Is Nothing Then
			If VpResult.Status = WebExceptionStatus.ConnectFailure Then
				Call clsModule.ShowWarning(clsModule.CgDL3b)
				Call MainForm.VgMe.StatusText(clsModule.CgDL3)
				Exit Sub
			ElseIf VpResult.Status = WebExceptionStatus.RequestCanceled Then
				Call MainForm.VgMe.StatusText(clsModule.CgDL5)
				Exit Sub
			End If
		End If
		If MainForm.VgMe.StatusTextGet = clsModule.CgDL2 Then
			Call MainForm.VgMe.StatusText(clsModule.CgDL4)
		End If
		'Maj EXE
		If File.Exists(Application.StartupPath + clsModule.CgUpDFile) Then
			File.SetLastWriteTimeUtc(Application.StartupPath + CgUpDFile, VgRemoteDate)
			Call clsModule.SecureDelete(Application.StartupPath + clsModule.CgDownDFile)
			File.Copy(Process.GetCurrentProcess.MainModule.FileName, Application.StartupPath + clsModule.CgDownDFile)
			Try
				Process.Start(New ProcessStartInfo(Application.StartupPath + clsModule.CgUpdater))
			Catch
				Call clsModule.ShowWarning(clsModule.CgErr5)
			End Try
		'Maj MDB
		ElseIf File.Exists(Application.StartupPath + clsModule.CgUpDDB) Then
			Call clsModule.DBImport(Application.StartupPath + clsModule.CgUpDDB)
		'Maj Images
		ElseIf File.Exists(Application.StartupPath + clsModule.CgUpPic + clsModule.CgPicUpExt) And File.Exists(Application.StartupPath + clsModule.CgUpPic + clsModule.CgPicLogExt) Then
			Call MainForm.VgMe.UpdatePictures(Application.StartupPath + clsModule.CgUpPic + clsModule.CgPicUpExt, Application.StartupPath + clsModule.CgUpPic + clsModule.CgPicLogExt, True)
		'Maj TXTFR
		ElseIf File.Exists(Application.StartupPath + clsModule.CgUpTXTFR) Then
			Call MainForm.VgMe.UpdateTxtFR
		End If
	End Sub
	Public Sub LoadCarac(VpMainForm As MainForm, VpForm As Object, VpCard As String, Optional VpSource As String = "", Optional VpEdition As String = "")
	'-----------------------------------------------
	'Chargement des détails de la carte sélectionnée
	'-----------------------------------------------
	Dim VpOpened As Boolean = False
	Dim VpSQL As String
		If MainForm.VgMe.IsMainReaderBusy Then
			Call clsModule.ShowWarning(clsModule.CgErr3)
		Else
			'Le type de la carte est inconnu à priori, on suppose par défaut que c'est une créature
			If VpSource <> "" Then
				VpSQL = "Select Card.Series, Card.Price, Card.PriceDate, Card.Rarity, Card.CardText, " + VpSource + ".Items, Creature.Tough, Creature.Power, Spell.Cost, Series.SeriesNM From ((((Card Inner Join Creature On Card.Title = Creature.Title) Inner Join Spell On Card.Title = Spell.Title) Inner Join " + VpSource + " On Card.EncNbr = " + VpSource + ".EncNbr) Inner Join Series On Card.Series = Series.SeriesCD) Where Card.Title = '" + VpCard.Replace("'", "''") + "'" + clsModule.CaracEdition(VpEdition)
				VpSQL = VpSQL + VpMainForm.Restriction
			Else
				VpSQL = "Select Card.Series, Card.Price, Card.PriceDate, Card.Rarity, Card.CardText, Creature.Tough, Creature.Power, Spell.Cost, Series.SeriesNM From (((Card Inner Join Creature On Card.Title = Creature.Title) Inner Join Spell On Card.Title = Spell.Title) Inner Join Series On Card.Series = Series.SeriesCD) Where Card.Title = '" + VpCard.Replace("'", "''") + "'" + clsModule.CaracEdition(VpEdition)
			End If
			VgDBCommand.CommandText = clsModule.TrimQuery(VpSQL)
			VgDBReader = VgDBCommand.ExecuteReader
			'S'il n'y a pas de réponse, c'est que ce n'est pas une créature, on supprime donc les champs force et endurance et on suppose que c'est un sort
			If Not VgDBReader.HasRows Then
				VgDBReader.Close
				If VpSource <> "" Then
					VpSQL = "Select Card.Series, Card.Price, Card.PriceDate, Card.Rarity, Card.CardText, " + VpSource + ".Items, Spell.Cost, Series.SeriesNM From (((Card Inner Join Spell On Card.Title = Spell.Title) Inner Join " + VpSource + " On " + VpSource + ".EncNbr = Card.EncNbr) Inner Join Series On Card.Series = Series.SeriesCD) Where Card.Title = '" + VpCard.Replace("'", "''") + "'" + clsModule.CaracEdition(VpEdition)
					VpSQL = VpSQL + VpMainForm.Restriction
				Else
					VpSQL = "Select Card.Series, Card.Price, Card.PriceDate, Card.Rarity, Card.CardText, Spell.Cost, Series.SeriesNM From ((Card Inner Join Spell On Card.Title = Spell.Title) Inner Join Series On Card.Series = Series.SeriesCD) Where Card.Title = '" + VpCard.Replace("'", "''") + "'" + clsModule.CaracEdition(VpEdition)
				End If
				VgDBCommand.CommandText = clsModule.TrimQuery(VpSQL)
				VgDBReader = VgDBCommand.ExecuteReader
				VpForm.lblProp6.Enabled = False
				VpForm.lblAD.Text = ""
				VgDBReader.Read
				VpOpened = True
				Call clsModule.BuildCost(VpMainForm, VpForm, VgDBReader.GetValue(VgDBReader.GetOrdinal("Cost")).ToString)
			'S'il y a des réponses c'était bien une créature
			Else
				VgDBReader.Read
				VpOpened = True
				VpForm.lblAD.Text = VgDBReader.GetValue(VgDBReader.GetOrdinal("Power")).ToString + "/" + VgDBReader.GetValue(VgDBReader.GetOrdinal("Tough")).ToString
				VpForm.lblProp6.Enabled = True
				Call clsModule.BuildCost(VpMainForm, VpForm, VgDBReader.GetValue(VgDBReader.GetOrdinal("Cost")).ToString)
			End If
			'Points communs quel que soit le type de carte
			With VgDBReader
				If Not VpOpened Then
					.Read
				End If
				If VpEdition = "" Then
					VpForm.cboEdition.Items.Clear
					VpForm.cboEdition.Text = .GetValue(VgDBReader.GetOrdinal("SeriesNM")).ToString
					VpForm.cboEdition.Items.Add(VpForm.cboEdition.Text)
				End If
				Try
					VpForm.picEdition.Image = clsModule.VgImgSeries.Images(clsModule.VgImgSeries.Images.IndexOfKey("_e" + .GetValue(VgDBReader.GetOrdinal("Series")).ToString + CgIconsExt))
				Catch
					VpForm.picEdition.Image = Nothing
				End Try
				If VpSource <> "" Then
'					Try
'						VpForm.picEdition.Image = clsModule.VgImgSeries.Images(clsModule.VgImgSeries.Images.IndexOfKey("_e" + .GetValue(VgDBReader.GetOrdinal("Series")).ToString + CgIconsExt))
'					Catch
'						VpForm.picEdition.Image = Nothing
'					End Try
					VpForm.lblStock.Text = .GetValue(VgDBReader.GetOrdinal("Items")).ToString
				Else
					VpForm.lblStock.Text = ""
				End If
				If .GetValue(VgDBReader.GetOrdinal("Price")) Is DBNull.Value OrElse MyVal(.GetValue(VgDBReader.GetOrdinal("Price"))) = 0 Then
					VpForm.lblPrix.Text = "N/C"
				Else
					'VpForm.lblProp5.Text = "Prix (" + .GetDateTime(VgDBReader.GetOrdinal("PriceDate")).ToShortDateString + ") :"
					VpForm.lblPrix.Text = Format(MyVal(.GetValue(VgDBReader.GetOrdinal("Price"))), "0.00") + " €"
				End If
				VpForm.lblRarete.Text = clsModule.FormatTitle("Card.Rarity", .GetValue(VgDBReader.GetOrdinal("Rarity")).ToString)
				If VpMainForm.mnuCardsFR.Checked Then
					VpForm.txtCardText.Text = clsModule.MyTxt(VpCard, True)
				Else
					VpForm.txtCardText.Text = .GetValue(VgDBReader.GetOrdinal("CardText")).ToString
				End If
				If VpEdition = "" Then
					'Il peut y avoir trois cas si jamais la requête a rapporté plus d'un enregistrement :
					'- 1 - soit la carte existe dans une autre édition et dans ce cas il faut inclure cette dernière dans la liste déroulante
					'- 2 - soit dans la même édition mais d'un autre deck (cas où la source vaut MyGames) et dans ce cas il faut sommer les items en stock
					'- 3 - soit dans la même édition et c'est alors un doublon (cas d'une recherche avancée sur une carte doublonnée)
					While .Read
						'Gestion cas 2
						If VpForm.cboEdition.Items.Contains(.GetValue(VgDBReader.GetOrdinal("SeriesNM")).ToString) Then
							'Gestion cas 3
							If VpSource <> "" Then
								VpForm.lblStock.Text = (CInt(VpForm.lblStock.Text) + .GetValue(VgDBReader.GetOrdinal("Items"))).ToString
							End If
						'Gestion cas 1
						Else
							VpForm.cboEdition.Items.Add(.GetValue(VgDBReader.GetOrdinal("SeriesNM")).ToString)
						End If
					End While
				End If
				.Close
			End With
		End If
	End Sub
	Public Sub BuildCost(VpMainForm As MainForm, VpForm As Object, VpCost As String)
	'--------------------------------------------------------------------
	'Affiche le coût d'invocation du sort sélectioné de manière graphique
	'--------------------------------------------------------------------
	Dim VpPictureBox As PictureBox			'Objet image icône courante
	Dim VpOffset As Integer					'Décalage courant pour présenter à l'endroit le coût d'invocation
	Dim VpInvoc As New clsManas(VpCost)		'Classe formatée correspondant au coût passé en paramètre
		Call clsModule.DeBuildCost(VpMainForm, VpForm)
		If VpCost.Trim = "" Then
			VpForm.lblProp1.Enabled = False
		Else
			VpForm.lblProp1.Enabled = True
			VpOffset = 0
			For Each VpImg As Integer In VpInvoc.ImgIndexes
				VpPictureBox = New PictureBox
				VpForm.grpSerie.Controls.Add(VpPictureBox)
				With VpPictureBox
					.Top = VpForm.lblProp1.Top
					.Size = New Size(18, 18)
					.Left = VpForm.cboEdition.Left + VpForm.cboEdition.Width - .Width * (VpInvoc.EffectiveLength - VpOffset)
					.Anchor = AnchorStyles.Top Or AnchorStyles.Right
					.Image = VpMainForm.imglstCarac.Images(VpImg)
				End With
				VpOffset = VpOffset + 1
			Next VpImg
		End If
	End Sub
	Public Sub DeBuildCost(VpMainForm As MainForm, VpForm As Object)
	'------------------------------------------------
	'Efface le coût d'invocation précédemment affiché
	'------------------------------------------------
	Dim VpPictureBox As PictureBox
	Dim VpToRemove As New ArrayList
		For Each VpControl As Control In VpForm.grpSerie.Controls
			Try
				VpPictureBox = CType(VpControl, PictureBox)
				If VpControl.Name <> "picEdition" And VpControl.Name <> "picScanCard" Then		'n'enlève pas le picturebox présentant l'icône de l'édition ainsi que celui la carte numérisée
					VpToRemove.Add(VpControl)
				End If
			Catch
			End Try
		Next VpControl
		For Each VpControl As Control In VpToRemove
			VpForm.grpSerie.Controls.Remove(VpControl)
		Next VpControl
	End Sub
	Public Function CaracEdition(VpEdition As String) As String
		If VpEdition <> "" Then
			Return " And Card.Series = '" + VpEdition + "' And "
		Else
			Return " And "
		End If
	End Function
	Public Function GetEncNbr(VpOwner As MainForm, VpSource As String, VpCardName As String, VpIDSerie As String) As Integer
	Dim VpSQL As String
		VpSQL = "Select Card.EncNbr From " + VpSource + " Inner Join Card On " + VpSource + ".EncNbr = Card.EncNbr Where Card.Title = '" + VpCardName.Replace("'", "''") + "' And Card.Series = '" + VpIDSerie + "' And "
		VpSQL = VpSQL + VpOwner.Restriction
		VpSQL = clsModule.TrimQuery(VpSQL)
		VgDBCommand.CommandText = VpSQL
		Return CInt(VgDBCommand.ExecuteScalar)
	End Function
	Public Function GetSerieCodeFromName(VpName As String, Optional VpApprox As Boolean = False) As String
	Dim VpO As Object
		If VpName.Length = 2 Then
			Return VpName
		Else
			VgDBCommand.CommandText = "Select SeriesCD From Series Where SeriesNM = '" + VpName + "';"
			VpO = VgDBCommand.ExecuteScalar
			If Not VpO Is Nothing Then
				Return VpO.ToString
			Else
				If VpApprox Then
					VgDBCommand.CommandText = "Select SeriesCD From Series Where InStr(SeriesNM, '" + VpName + "') > 0;"
					VpO = VgDBCommand.ExecuteScalar
					If Not VpO Is Nothing Then
						Return VpO.ToString
					Else
						Return " "
					End If
				Else
					Return " "
				End If
			End If
		End If
	End Function
	Public Function HasPriceHistory As Boolean
		VgDBCommand.CommandText = "Select Count(*) From PricesHistory;"
		Return ( CInt(VgDBCommand.ExecuteScalar) > 0 )
	End Function
	Public Function GetPriceHistory(VpCardName As String, VpSerie As String) As SortedList
	'----------------------------------------------------------------
	'Retourne l'historique des prix pour la carte passée en paramètre
	'----------------------------------------------------------------
	Dim VpHist As New SortedList
		VgDBCommand.CommandText = "Select PricesHistory.PriceDate, PricesHistory.Price From PricesHistory Inner Join Card On PricesHistory.EncNbr = Card.EncNbr Where Card.Title = '" + VpCardName + "' And Card.Series = '" + VpSerie + "' Order By PricesHistory.PriceDate Asc;"
		VgDBReader = VgDBCommand.ExecuteReader
		With VgDBReader
			While .Read
				VpHist.Add(.GetDateTime(0), .GetFloat(1))
			End While
			.Close
		End With
		Return VpHist
	End Function
	Public Function GetLastPricesDate As Date
		VgDBCommand.CommandText = "Select Top 1 PriceDate From Card Order By PriceDate Desc;"
		Return VgDBCommand.ExecuteScalar
	End Function
	Public Sub LoadEditions(VpCbo As ComboBox)
	'--------------------------------------------------------------
	'Charge la liste des éditions présentes dans la base de données
	'--------------------------------------------------------------
		VpCbo.Items.Clear
		VpCbo.Text = ""
		VgDBCommand.CommandText = "Select Distinct Series.SeriesCD, Series.SeriesNM From Card Inner Join Series On Card.Series = Series.SeriesCD;"
		VgDBReader = VgDBCommand.ExecuteReader
		With VgDBReader
			While .Read
				VpCbo.Items.Add("(" + .GetString(0) + ") " + .GetString(1))
			End While
			.Close
		End With
		VpCbo.Sorted = True
	End Sub
	Public Sub LoadScanCard(VpTitle As String, VppicScanCard As PictureBox, Optional VpSave As Boolean = False, Optional VpSaveFolder As String = "")
	'---------------------------------------------------------------------------------
	'Charge l'image scannérisée de la carte recherchée dans la zone prévue à cet effet
	'---------------------------------------------------------------------------------
	Dim VpOffset As Long
	Dim VpEnd As Long
	Dim VpPicturesFile As StreamReader
	Dim VpPicturesFileB As BinaryReader
	Dim VpTmp As String = clsModule.GetFreeTempFile
	Dim VpTmpFile As StreamWriter
	Dim VpTmpFileB As BinaryWriter
	Dim VpMissingTable As Boolean = False
	Dim VpDest As String
		If MainForm.VgMe.IsInImgDL Then
			Call clsModule.ShowWarning(clsModule.CgDL2c)
			Exit Sub
		ElseIf MainForm.VgMe.IsMainReaderBusy Then
			Exit Sub
		ElseIf File.Exists(VgOptions.VgSettings.PicturesFile) Then
			If (New FileInfo(VgOptions.VgSettings.PicturesFile)).Length < clsModule.CgImgMinLength Then
				If Not VpSave Then
					VppicScanCard.Image = Nothing
				End If
				Call clsModule.ShowWarning("La base d'images ne possède pas la taille minimale requise." + vbCrLf + "Vérifiez le fichier spécifié dans les Préférences...")
				Exit Sub
			End If
			VpPicturesFile = New StreamReader(VgOptions.VgSettings.PicturesFile)
			VpPicturesFileB = New BinaryReader(VpPicturesFile.BaseStream)
			VpTmpFile = New StreamWriter(VpTmp)
			VpTmpFileB = New BinaryWriter(VpTmpFile.BaseStream)
			VgDBCommand.CommandText = "Select [Offset], [End] From CardPictures Where Title = '" + VpTitle.Replace("'", "''").Replace(":", "").Replace("/", "").Replace("""", "").Replace("?", "") + "' Order By [End] Desc;"
			Try
				VgDBReader = VgDBCommand.ExecuteReader
				VgDBReader.Read
				If VgDBReader.HasRows Then
					VpOffset = CLng(VgDBReader.GetValue(0))
					VpEnd = CLng(VgDBReader.GetValue(1))
					If VpOffset > 0 Then
						VpPicturesFileB.BaseStream.Seek(VpOffset, SeekOrigin.Begin)
					End If
					VpTmpFileB.Write(VpPicturesFileB.ReadBytes(VpEnd - VpOffset + 1))
					VpTmpFileB.Flush
					VpTmpFileB.Close
					VpPicturesFileB.Close
					If Not VpSave Then
						Try
							VppicScanCard.Image = Image.FromFile(VpTmp)
						Catch
							Call clsModule.ShowWarning("La base d'images semble être corrompue." + vbCrLf + "Essayez de la mettre à jour ou de la re-télécharger...")
							VppicScanCard.Image = Nothing
							VgDBReader.Close
							Exit Sub
						End Try
					Else
						VpDest = VpSaveFolder + "\" + VpTitle + ".jpg"
						If Not File.Exists(VpDest) Then
							File.Copy(VpTmp, VpDest)
						End If
					End If
				Else
					If Not VpSave Then
						VppicScanCard.Image = Nothing
					End If
				End If
			Catch VpEx As OleDbException
				If VpEx.ErrorCode = clsModule.CgMissingTable Then
					VpMissingTable = True
				End If
				If Not VpSave Then
					VppicScanCard.Image = Nothing
				End If
			End Try
			VgDBReader.Close
			'Fichier présent mais table d'index absente
			If VpMissingTable Then
				Select Case clsModule.ShowQuestion("Cette version du logiciel est capable de gérer les images des cartes mais la base de données n'est pas à jour." + vbCrLf + "Voulez-vous télécharger les informations manquantes maintenant ?" + vbCrLf + "Cliquez sur 'Annuler' pour ne plus afficher ce message...", MessageBoxButtons.YesNoCancel)
					Case DialogResult.Yes
						Call clsModule.DownloadUpdate(New Uri(CgURL3), clsModule.CgUpDDB)
					Case DialogResult.Cancel
						VgOptions.VgSettings.PicturesFile = ""
				 	Case Else
				End Select
				Exit Sub
			End If
		End If
	End Sub
	Public Function GetAdvCount As Integer
	'------------------------------------------------------
	'Retourne le nombre de propriétaires en base de données
	'------------------------------------------------------
		VgDBCommand.CommandText = "Select Count(*) From MyAdversairesID;"
		Return VgDBCommand.ExecuteScalar
	End Function
	Public Function GetAdvDecksCount(VpI As Integer) As Integer
	'--------------------------------------------------------------------------------------
	'Retourne le nombre de decks possédés par le propriétaire d'index spécifié en paramètre
	'--------------------------------------------------------------------------------------
		VgDBCommand.CommandText = "Select Count(*) From MyGamesID Where AdvID = " + VpI.ToString + ";"
		Return VgDBCommand.ExecuteScalar
	End Function
	Public Function GetAdvId(VpName As String) As Integer
	'-------------------------------------------------------------------------
	'Retourne l'identifiant de l'adversaire dont le nom est passé en paramètre
	'-------------------------------------------------------------------------
		VgDBCommand.CommandText = "Select AdvId From MyAdversairesId Where AdvName = '" + VpName.Replace("'", "''") + "';"
		Return VgDBCommand.ExecuteScalar	
	End Function
	Public Function GetOwner(VpDeck As String) As String
	'-------------------------------------------------------------------------
	'Retourne l'identifiant de l'adversaire dont le nom est passé en paramètre
	'-------------------------------------------------------------------------
		VgDBCommand.CommandText = "Select AdvName From MyAdversairesID Inner Join MyGamesID On MyAdversairesID.AdvID = MyGamesID.AdvID Where GameName = '" + VpDeck.Replace("'", "''") + "';"
		Return VgDBCommand.ExecuteScalar	
	End Function
	Public Function GetAdvName(VpI As Integer) As String
	'-------------------------------------------------------------
	'Retourne le nom de l'adversaire d'index spécifié en paramètre
	'N.B. : 0 = Moi
	'-------------------------------------------------------------
		VgDBCommand.CommandText = "Select Last(AdvName) From (Select Top " + VpI.ToString + " AdvName From MyAdversairesID Order By AdvID);"
		Return VgDBCommand.ExecuteScalar
	End Function
	Public Function GetNewAdvId As Integer
	'-------------------------------------------------
	'Retourne un identifiant pour un nouvel adversaire
	'-------------------------------------------------
		VgDBCommand.CommandText = "Select Max(AdvID) From MyAdversairesID;"
		Return (CInt(VgDBCommand.ExecuteScalar) + 1)
	End Function
	Public Function GetDeckName(VpI As Integer) As String
	'---------------------------------------------------------------------
	'Retourne le nom du deck d'index spécifié en paramètre
	'/!\ retourne le nom du VpI ème deck, et pas le deck dont l'id est VpI
	'---------------------------------------------------------------------
		VgDBCommand.CommandText = "Select Last(GameName) From (Select Top " + VpI.ToString + " GameName From MyGamesID Order By GameID);"
		Try
			Return VgDBCommand.ExecuteScalar
		Catch
			Return "Jeu n°" + VpI.ToString
		End Try
	End Function
	Public Function GetDeckIndex(VpStr As String) As String
	'-----------------------------------------------------
	'Retourne l'index du deck de nom spécifié en paramètre
	'-----------------------------------------------------
	Dim VpO As Object
		VgDBCommand.CommandText = "Select GameID From MyGamesID Where GameName = '" + VpStr.Replace("'", "''") + "';"
		VpO = VgDBCommand.ExecuteScalar
		If Not VpO Is Nothing Then
			Return VpO.ToString
		Else
			Return ""
		End If
	End Function
	Public Function GetDeckCount As Integer
	'----------------------------------------------
	'Retourne le nombre de decks en base de données
	'----------------------------------------------
		VgDBCommand.CommandText = "Select Count(*) From MyGamesID;"
		Return VgDBCommand.ExecuteScalar
	End Function
	Public Function GetNewDeckId As Integer
	'--------------------------------------------
	'Retourne un identifiant pour un nouveau deck
	'--------------------------------------------
		VgDBCommand.CommandText = "Select Max(GameID) From MyGamesID;"
		Try
			Return (CInt(VgDBCommand.ExecuteScalar) + 1)
		Catch
			'Si pas de deck présent
			Return 0
		End Try
	End Function
	Public Function GetFreeTempFile As String
	'--------------------------------------------------
	'Retourne un nom de fichier temporaire image valide
	'--------------------------------------------------
	Dim VpI As Integer = 0
		Do
			VpI = VpI + 1
		Loop While File.Exists(Path.GetTempPath + "\mtgm~" + VpI.ToString + ".jpg")
		Return Path.GetTempPath + "\mtgm~" + VpI.ToString + ".jpg"
	End Function
	Public Sub DeleteTempFiles
	'------------------------------------
	'Suppression des fichiers temporaires
	'------------------------------------
		'Images
		Try
			For Each VpFile As FileInfo In (New DirectoryInfo(Path.GetTempPath)).GetFiles("mtgm~*.jpg")
				Call clsModule.SecureDelete(VpFile.FullName)
			Next VpFile
		Catch
			Call clsModule.ShowWarning("Impossible d'accéder au répertoire temporaire...")
		End Try
		'Updates
		Call clsModule.SecureDelete(Application.StartupPath + clsModule.CgUpTXTFR)
		Call clsModule.SecureDelete(Application.StartupPath + clsModule.CgUpDFile)
		Call clsModule.SecureDelete(Application.StartupPath + clsModule.CgUpDDB)
		Call clsModule.SecureDelete(Application.StartupPath + clsModule.CgUpDDBb)
		Call clsModule.SecureDelete(Application.StartupPath + clsModule.CgUpPrices)
		Call clsModule.SecureDelete(Application.StartupPath + clsModule.CgUpSeries)
		For Each VpFile As FileInfo In (New DirectoryInfo(Application.StartupPath)).GetFiles("*_en.txt")
			Call clsModule.SecureDelete(VpFile.FullName)
		Next VpFile
		For Each VpFile As FileInfo In (New DirectoryInfo(Application.StartupPath)).GetFiles("*_fr.txt")
			Call clsModule.SecureDelete(VpFile.FullName)
		Next VpFile
	End Sub
	Public Sub SecureDelete(VpFile As String)
	'----------------------------------------------------------------
	'Suppression du fichier passé en paramètre (avec trappe d'erreur)
	'----------------------------------------------------------------
		Try
			File.Delete(VpFile)
		Catch
		End Try
	End Sub
	Public Sub VerboseSimu(VpVerbose As Boolean, VpStr As String, VpSimuOut As StreamWriter, Optional VpEndSimu As Boolean = False)
	'-----------------------------------------------------
	'Gère la verbosité pour les simulations de déploiement
	'-----------------------------------------------------
		If VpVerbose Then
			If VpEndSimu Then
				VpSimuOut.Flush
				VpSimuOut.Close
			Else
				VpSimuOut.WriteLine(VpStr)
			End If
		End If
	End Sub
End Module
Public Class clsChildren
	Private VmDeleteEdition As frmDeleteEdition = Nothing
	Private VmGestDecks As frmGestDecks = Nothing
	Private VmGestAdv As frmGestAdv = Nothing
	Private VmBuyMV As frmBuyMV = Nothing
	Private VmSearcher As frmSearch = Nothing
	Private VmPerfs As frmPerfs = Nothing
	Private VmUpdateContenu As frmUpdateContenu = Nothing
	Private VmPricesHistory As frmGrapher = Nothing
	Private VmImportExport As frmExport = Nothing
	Public Function DoesntExist(VpForm As Form) As Boolean
		Return VpForm Is Nothing OrElse VpForm.IsDisposed
	End Function
	Public Property EditionDeleter As frmDeleteEdition
		Get
			Return VmDeleteEdition
		End Get
		Set (VpDeleteEdition As frmDeleteEdition)
			VmDeleteEdition = VpDeleteEdition
		End Set
	End Property
	Public Property DecksManager As frmGestDecks
		Get
			Return VmGestDecks
		End Get
		Set (VpGestDecks As frmGestDecks)
			VmGestDecks = VpGestDecks
		End Set
	End Property
	Public Property AdversairesManager As frmGestAdv
		Get
			Return VmGestAdv
		End Get
		Set (VpGestAdv As frmGestAdv)
			VmGestAdv = VpGestAdv
		End Set
	End Property
	Public Property MVBuyer As frmBuyMV
		Get
			Return VmBuyMV
		End Get
		Set (VpBuyMV As frmBuyMV)
			VmBuyMV = VpBuyMV
		End Set
	End Property
	Public Property Searcher As frmSearch
		Get
			Return VmSearcher
		End Get
		Set (VpSearcher As frmSearch)
			VmSearcher = VpSearcher
		End Set
	End Property
	Public Property PerfsCounter As frmPerfs
		Get
			Return VmPerfs
		End Get
		Set (VpPerfs As frmPerfs)
			VmPerfs = VpPerfs
		End Set
	End Property
	Public Property ContenuUpdater As frmUpdateContenu
		Get
			Return VmUpdateContenu
		End Get
		Set (VpUpdateContenu As frmUpdateContenu)
			VmUpdateContenu = VpUpdateContenu
		End Set
	End Property
	Public Property PricesHistory As frmGrapher
		Get
			Return VmPricesHistory
		End Get
		Set (VpPricesHistory As frmGrapher)
			VmPricesHistory = VpPricesHistory
		End Set
	End Property
	Public Property ImporterExporter As frmExport
		Get
			Return VmImportExport
		End Get
		Set (VpImportExport As frmExport)
			VmImportExport = VpImportExport
		End Set
	End Property
End Class
Public Class clsSearch
	Public ItemsFound As New ArrayList
	Public CurItem As Integer
End Class
Public Class clsManas
	Private VmX As Short = 0				'Mana variable
	Private VmM As Short = 0				'Mana de n'importe quelle couleur
	Private VmA As Short = 0				'Mana sans couleur
	Private VmB As Short = 0				'Mana noir
	Private VmG As Short = 0				'Mana vert
	Private VmU As Short = 0				'Mana bleu
	Private VmR As Short = 0				'Mana rouge
	Private VmW As Short = 0				'Mana blanc
	Private VmBG As Short = 0				'Mana noir ou vert
	Private VmBR As Short = 0				'Mana noir ou rouge
	Private VmGU As Short = 0				'Mana vert ou bleu
	Private VmGW As Short = 0				'Mana vert ou blanc
	Private VmRG As Short = 0				'Mana rouge ou vert
	Private VmRW As Short = 0				'Mana rouge ou blanc
	Private VmUB As Short = 0				'Mana bleu ou noir
	Private VmUR As Short = 0				'Mana bleu ou rouge
	Private VmWB As Short = 0				'Mana blanc ou noir
	Private VmWU As Short = 0				'Mana blanc ou bleu
	Private VmEffectiveLength As Short = 0	'Longueur effective (~coût converti)
	Private VmImgIndexes As New ArrayList	'Repères icônes (1=BG,2=BR,3=G,4=GU,5=GW,6=R,7=RG,8=RW,9=U,10=UB,11=UR,12=W,13=WB,14=WU,15=X,16=)
	Public Sub New(Optional VpCostDB As String = "")
	'----------------------------------------------
	'Effectue un parsing du coût passé en paramètre
	'----------------------------------------------
	Dim VpCost As String
	Dim VpX As String						'Unité de coût courant
	Dim VpY As String						'Unité de dual-coût courant
	Dim VpI As Integer						'Compteur
	Dim VpColorless As Integer				'Coût en manas sans couleur
	Dim VpStrs(0 To 1) As String
		If VpCostDB.Trim = "" Then Exit Sub
		'Gestion des cas spéciaux :
		VpCost = VpCostDB.ToLower
		VpCost = VpCost.Replace("{", "").Replace("}", "")
		'- either or (ex. either b or u)
		If VpCost.Contains(clsModule.CgManaParsing(2)) Then
			VpCost = VpCost.Replace(clsModule.CgManaParsing(2), "(").Replace(clsModule.CgManaParsing(3), "/") + ")"
		'- colorless mana (ex. one colorless mana)
		ElseIf VpCost.Contains(clsModule.CgManaParsing(4)) Then
			VpCost = VpCost.Replace(clsModule.CgManaParsing(4), "")
			VpCost = clsModule.StrBuild("A", clsModule.FindNumber(VpCost))
		'- colorless mana d'un nombre indéterminé (on en met 1 par défaut)
		ElseIf VpCost = clsModule.CgManaParsing(4).Trim Then
			VpCost = "A"
		'- mana of any color (ex. one mana of any color)
		ElseIf VpCost.Contains(clsModule.CgManaParsing(5)) Then
			VpCost = VpCost.Replace(clsModule.CgManaParsing(5), "")
			VpCost = clsModule.StrBuild("M", clsModule.FindNumber(VpCost))
		'- mana (ex. two blue mana)
		ElseIf VpCost.Contains(clsModule.CgManaParsing(6)) Then
			VpCost = VpCost.Replace(clsModule.CgManaParsing(6), "")
			VpStrs = VpCost.Split(" ")
			Select Case VpStrs(1)
				Case "black"
					VpStrs(1) = "B"
				Case "blue"
					VpStrs(1) = "U"
				Case "red"
					VpStrs(1) = "R"
				Case "white"
					VpStrs(1) = "W"
				Case "green"
					VpStrs(1) = "G"
				Case Else
					VpStrs(1) = "A"
			End Select
			VpCost = clsModule.StrBuild(VpStrs(1), clsModule.FindNumber(VpStrs(0)))
		'- XA... (ex. X2U)
		ElseIf VpCost.StartsWith("x") AndAlso Val(VpCost.Substring(1)) <> 0 Then
			VmX = 1
			VmImgIndexes.Add(15)
			VmEffectiveLength = 1
			VpCost = VpCost.Substring(1)
		End If
		'Gestion des cas classiques :
		VmEffectiveLength = VmEffectiveLength + VpCost.Length - 4 * clsModule.StrCount(VpCost, "(")
		VpColorless = Val(VpCost)
		If VpColorless >= 10 Then
			VmEffectiveLength = VmEffectiveLength - 1
		End If
		For VpI = 0 To VpCost.Length - 1
			VpX = VpCost.Substring(VpI, 1)
			'Pas de couleur
			If VpI = 0 And VpColorless <> 0 Then
				VmImgIndexes.Add(16 + CInt(VpColorless))
				If VpColorless >= 10 Then
					VpI = VpI + 1 	'Très impropre en programmation structurée mais permet de gérer directement le cas des nombres à 2 chiffres
				End If
				VmA = VpColorless
			'Dual couleur
			ElseIf VpX = "(" Then
				VpY = VpCost.Substring(VpI + 1, 3)
				Select Case VpY.ToUpper
					Case "B/G", "G/B"
						VmImgIndexes.Add(1)
						VmBG = VmBG + 1
					Case "B/R", "R/B"
						VmImgIndexes.Add(2)
						VmBR = VmBR + 1
					Case "G/U", "U/G"
						VmImgIndexes.Add(4)
						VmGU = VmGU + 1
					Case "G/W", "W/G"
						VmImgIndexes.Add(5)
						VmGW = VmGW + 1
					Case "R/G", "G/R"
						VmImgIndexes.Add(7)
						VmRG = VmRG + 1
					Case "R/W", "W/R"
						VmImgIndexes.Add(8)
						VmRW = VmRW + 1
					Case "U/B", "B/U"
						VmImgIndexes.Add(10)
						VmUB = VmUB + 1
					Case "U/R", "R/U"
						VmImgIndexes.Add(11)
						VmUR = VmUR + 1
					Case "W/B", "B/W"
						VmImgIndexes.Add(13)
						VmWB = VmWB + 1
					Case "W/U", "U/W"
						VmImgIndexes.Add(14)
						VmWU = VmWU + 1
					Case Else
						VmImgIndexes.Add(16)
				End Select
				VpI = VpI + 4 'encore impropre mais permet de passer directement à la fin de la parenthèse
			'Couleur simple
			Else
				Select Case VpX.ToUpper
					Case "X"
						VmImgIndexes.Add(15)
						VmX = VmX + 1
					Case "B"
						VmImgIndexes.Add(0)
						VmB = VmB + 1
					Case "G"
						VmImgIndexes.Add(3)
						VmG = VmG + 1
					Case "R"
						VmImgIndexes.Add(6)
						VmR = VmR + 1
					Case "U"
						VmImgIndexes.Add(9)
						VmU = VmU + 1
					Case "W"
						VmImgIndexes.Add(12)
						VmW = VmW + 1
					Case "M"
						VmImgIndexes.Add(16)
						VmM = VmM + 1
					Case "A"
						VmImgIndexes.Add(16)
						VmA = VmA + 1
					Case Else
						VmImgIndexes.Add(16)
				End Select
			End If
		Next VpI
	End Sub
	Public Function IsBetterWith(VpMana As clsManas) As Boolean
	'-----------------------------------------------------------------------------------------------------------
	'Renvoie vrai si l'invocation de la carte courante requiert le mana produit par la carte passée en paramètre
	'-----------------------------------------------------------------------------------------------------------
		Return ( ( Me.HasBlack And VpMana.HasBlack ) Or ( Me.HasBlue And VpMana.HasBlue ) Or ( Me.HasGreen And VpMana.HasGreen ) Or ( Me.HasRed And VpMana.HasRed ) Or ( Me.HasWhite And VpMana.HasWhite ) )
	End Function
	Public Sub AddSubManas(VpManas As clsManas, Optional VpSub As Short = 1)
	'----------------------------------------------------------------------------------
	'Ajoute (ou soustrait) la description de manas passée en paramètre à celle courante
	'----------------------------------------------------------------------------------
		VmM = VmM + VpManas.cM * VpSub
		VmA = VmA + VpManas.cA * VpSub
		VmB = VmB + VpManas.cB * VpSub
		VmG = VmG + VpManas.cG * VpSub
		VmU = VmU + VpManas.cU * VpSub
		VmR = VmR + VpManas.cR * VpSub
		VmW = VmW + VpManas.cW * VpSub
		VmBG = VmBG + VpManas.cBG * VpSub
		VmBR = VmBR + VpManas.cBR * VpSub
		VmGU = VmGU + VpManas.cGU * VpSub
		VmGW = VmGW + VpManas.cGW * VpSub
		VmRG = VmRG + VpManas.cRG * VpSub
		VmRW = VmRW + VpManas.cRW * VpSub
		VmUB = VmUB + VpManas.cUB * VpSub
		VmUR = VmUR + VpManas.cUR * VpSub
		VmWB = VmWB + VpManas.cWB * VpSub
		VmWU = VmWU	+ VpManas.cWU * VpSub
	End Sub
	Public Function ContainsEnoughFor(VpManas As clsManas) As Boolean
	'-----------------------------------------------------------------------------------------------------------------------
	'Renvoie vrai si la description de mana courante contient (en terme de qualité et de quantité) celle passée en paramètre
	'-----------------------------------------------------------------------------------------------------------------------
	Dim VpEnoughColor As Boolean
	Dim VpMe As clsManas
		VpEnoughColor = ( 	VmB >= VpManas.cB And _
							VmG >= VpManas.cG And _
							VmU >= VpManas.cU And _
							VmR >= VpManas.cR And _
							VmW >= VpManas.cW And _
							VmBG >= VpManas.cBG And _
							VmBR >= VpManas.cBR And _
							VmGU >= VpManas.cGU And _
							VmGW >= VpManas.cGW And _
							VmRG >= VpManas.cRG And _
							VmRW >= VpManas.cRW And _
							VmUB >= VpManas.cUB And _
							VmUR >= VpManas.cUR And _
							VmWB >= VpManas.cWB And _
							VmWU >= VpManas.cWU		)
		'Si les manas de couleurs ne peuvent être payés, c'est mort
		If Not VpEnoughColor Then
			Return False
		Else
			'S'ils le peuvent et qu'en plus les multi/in-colores peuvent être payés, c'est bon
			If (VmA + VmM) >= (VpManas.cA + VpManas.cM) Then
				Return True
			'Sinon, il faut regarder si les multi/in-colores peuvent êtres payés avec ceux restant de couleurs
			Else
				'Copie temporaire de la description de manas courante
				VpMe = New clsManas
				Call VpMe.AddSubManas(Me)
				'Soustraction des coûts colorés uniquement
				Call VpMe.AddSubManas(VpManas, -1)
				VpMe.cA = Me.cA
				VpMe.cM = Me.cM
				'Reste-t-il assez pour payer les multi/in-colores de la description de manas passée en paramètre
				Return ( VpMe.Potentiel >= (VpManas.cA + VpManas.cM) )
			End If
		End If
	End Function
	Public ReadOnly Property ImgIndexes As ArrayList
		Get
			Return Me.VmImgIndexes
		End Get
	End Property
	Public ReadOnly Property EffectiveLength As Integer
		Get
			Return Me.VmEffectiveLength
		End Get
	End Property
	Public ReadOnly Property Potentiel As Integer
		Get
			Return ( VmX + VmM + VmA + VmB + VmG + VmU + VmR + VmW + VmBG + VmBR + VmGU + VmGW + VmRG + VmRW + VmUB + VmUR + VmWB + VmWU )
		End Get
	End Property
	Public ReadOnly Property HasBlack As Boolean
		Get
			Return ( Me.VmB > 0 Or Me.VmBG > 0 Or Me.VmBR > 0 Or Me.VmUB > 0 Or Me.VmWB > 0 )
		End Get
	End Property
	Public ReadOnly Property HasGreen As Boolean
		Get
			Return ( Me.VmG > 0 Or Me.VmBG > 0 Or Me.VmGU > 0 Or Me.VmGW > 0 Or Me.VmRG > 0 )
		End Get
	End Property
	Public ReadOnly Property HasBlue As Boolean
		Get
			Return ( Me.VmU > 0 Or Me.VmGU > 0 Or Me.VmUB > 0 Or Me.VmUR > 0 Or Me.VmWU > 0 )
		End Get
	End Property
	Public ReadOnly Property HasRed As Boolean
		Get
			Return ( Me.VmR > 0 Or Me.VmBR > 0 Or Me.VmRG > 0 Or Me.VmRW > 0 Or Me.VmUR > 0 )
		End Get
	End Property
	Public ReadOnly Property HasWhite As Boolean
		Get
			Return ( Me.VmW > 0 Or Me.VmGW > 0 Or Me.VmRW > 0 Or Me.VmWB > 0 Or Me.VmWU > 0 )
		End Get
	End Property
	Public Property cM As Short
		Get
			Return Me.VmM
		End Get
		Set (VpM As Short)
			VmM = VpM
		End Set
	End Property
	Public Property cA As Short
		Get
			Return Me.VmA
		End Get
		Set (VpA As Short)
			VmA = VpA
		End Set
	End Property
	Public ReadOnly Property cB As Short
		Get
			Return Me.VmB
		End Get
	End Property
	Public ReadOnly Property cG As Short
		Get
			Return Me.VmG
		End Get
	End Property
	Public ReadOnly Property cU As Short
		Get
			Return Me.VmU
		End Get
	End Property
	Public ReadOnly Property cR As Short
		Get
			Return Me.VmR
		End Get
	End Property
	Public ReadOnly Property cW As Short
		Get
			Return Me.VmW
		End Get
	End Property
	Public ReadOnly Property cBG As Short
		Get
			Return Me.VmBG
		End Get
	End Property
	Public ReadOnly Property cBR As Short
		Get
			Return Me.VmBR
		End Get
	End Property
	Public ReadOnly Property cGU As Short
		Get
			Return Me.VmGU
		End Get
	End Property
	Public ReadOnly Property cGW As Short
		Get
			Return Me.VmGW
		End Get
	End Property
	Public ReadOnly Property cRG As Short
		Get
			Return Me.VmRG
		End Get
	End Property
	Public ReadOnly Property cRW As Short
		Get
			Return Me.VmRW
		End Get
	End Property
	Public ReadOnly Property cUB As Short
		Get
			Return Me.VmUB
		End Get
	End Property
	Public ReadOnly Property cUR As Short
		Get
			Return Me.VmUR
		End Get
	End Property
	Public ReadOnly Property cWB As Short
		Get
			Return Me.VmWB
		End Get
	End Property
	Public ReadOnly Property cWU As Short
		Get
			Return Me.VmWU
		End Get
	End Property
End Class
Public Class clsManasPotComparer
	Implements IComparer
	Private VmUserList As CheckedListBox
	Public Sub New(VpUserList As CheckedListBox)
		VmUserList = VpUserList
	End Sub
	Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
	'---------------------------------------------------------------------------------------------------------------------
	'Permet le tri des cartes dans l'ordre de préférence d'invocation, selon le mana qu'elles sont susceptibles de générer
	'Favorise également l'invocation des cartes spéciales selon l'ordre spécifié par l'utilisateur
	'---------------------------------------------------------------------------------------------------------------------
	Dim VpCard1 As clsCard = x
	Dim VpCard2 As clsCard = y
	Dim VpPot1 As Integer = Me.GetMiniPot(VpCard1)
	Dim VpPot2 As Integer = Me.GetMiniPot(VpCard2)
		Return VpPot2 - VpPot1
	End Function
	Private Function GetMiniPot(VpCard As clsCard) As Integer
	Dim VpPot As Integer
		If VpCard.ManaAble Then
			VpPot = VpCard.ManasPot
		ElseIf VpCard.IsSpecial Then
			VpPot = CgMaxPot - VmUserList.CheckedItems.IndexOf(VpCard.CardName)
		End If
		Return VpPot
	End Function
End Class
Public Class clsTxtFR
	Private VmCard As String
	Private VmTexte As String
	Private VmAlready As eTxtState
	Public Enum eTxtState
		Neww = 0
		Update
		Ok
	End Enum
	Public Sub New (VpCard As String, VpTexte As String)
		VmCard = VpCard
		VmTexte = VpTexte
		VmAlready = eTxtState.Neww
	End Sub
	Public ReadOnly Property CardName As String
		Get
			Return VmCard
		End Get
	End Property
	Public ReadOnly Property Texte As String
		Get
			Return VmTexte
		End Get
	End Property
	Public Property Already As eTxtState
		Get
			Return VmAlready
		End Get
		Set (VpAlready As eTxtState)
			VmAlready = VpAlready
		End Set
	End Property
End Class