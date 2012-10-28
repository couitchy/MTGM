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
'| Release 9      |                        05/02/2011 |
'| Release 10     |                        10/09/2011 |
'| Release 11     |                        24/01/2012 |
'| Release 12     |                        01/10/2012 |
'| Auteur         |                          Couitchy |
'|----------------------------------------------------|
'| Modifications :                                    |
'| - t�l�chargement auto des d�pendances   10/10/2009 |
'| - pr�vention des ex�cutions multiples   31/01/2010 |
'| - gestion cartes foils				   19/12/2010 |
'| - gestion des co�ts partiels en PV	   02/05/2011 |
'------------------------------------------------------
Imports System.Data
Imports System.Data.OleDb
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Net
Imports System.IO
Imports System.ComponentModel
Public Module clsModule
	Public Declare Function OpenIcon 				Lib "user32" (ByVal hwnd As Long) As Long
	Public Declare Function SetForegroundWindow		Lib "user32" (ByVal hwnd As Long) As Long
	Public Const CgCodeLines As Integer   			= 31441
	Public Const CGNClasses As Integer   			= 69
	Public Const CgLastUpdateAut As String			= "30/07/2012"
	Public Const CgLastUpdateSimu As String			= "27/07/2012"
	Public Const CgLastUpdateTxtVF As String		= "24/07/2012"
	Public Const CgLastUpdateRulings As String		= "27/07/2012"
	Public Const CgLastUpdateTradPatch As String	= "20/09/2012"
	Public Const CgProject As String				= "Magic_The_Gathering_Manager.MainForm"
	Public Const CgMe As String						= "Moi"
	Public Const CgNCriterions As Integer 			= 8
	Public Const CgNDispMenuBase As Integer 		= 3
	Public Const CgNMain As Integer					= 7
	Public Const CgNLives As Integer				= 20
	Public Const CgMaxPot As Integer				= 100
	Public Const CgPertinCoeff As Integer			= 4
	Public Const CgGraphsExtraMargin As Single		= 0.2
	Public Const CgMaxGraphs As Integer				= 128
	Public Const CgMaxVignettes As Integer			= 120
	Public Const CgMissingTable As Long				= -2147217865
	Public Const CgImgMinLength As Long				= 296297676
	Public Const CgTimeOut As Integer				= 5
	Public Const CgMTGCardWidth_mm As Integer		= 63
	Public Const CgMTGCardHeight_mm As Integer		= 88
	Public Const CgMTGCardWidth_px As Integer		= 210
	Public Const CgMTGCardHeight_px As Integer		= 300
	Public Const CgCounterDiametr_px As Integer 	= 20
	Public Const CgChevauchFactor As Single			= 0.14
	Public Const CgSpacingFactor As Single			= 1.5
	Public Const CgXMargin As Integer				= 5
	Public Const CgYMargin As Integer				= 8
	Public Const CgTemp As String					= "\mtgmgr"
	Public Const CgIcons As String        			= "\Ressources"
	Public Const CgMagicBack As String      		= "\Ressources\Magic Back.jpg"
	Public Const CgMDB As String					= "\Cartes\Magic DB.mdb"
	Public Const CgDAT As String					= "\Cartes\Images DB.dat"
	Public Const CgINIFile As String      			= "\MTGM.ini"
	Public Const CgXMLFile As String      			= "\MTGM.xml"
	Public Const CgHLPFile As String      			= "\MTGM.pdf"
	Public Const CgHSTFile As String      			= "\Historique.txt"
	Public Const CgUpdater As String      			= "\Updater.exe"
	Public Const CgMTGMWebResourcer As String		= "\WebResourcer.exe"
	Public Const CgUpDFile As String      			= "\Magic The Gathering Manager.new"
	Public Const CgDownDFile As String     			= "\Magic The Gathering Manager.bak"
	Public Const CgUpDDB As String      			= "\Images DB.mdb"
	Public Const CgUpDDBb As String      			= "\Patch.mdb"
	Public Const CgUpDDBd As String      			= "Images%20DB.dat"
	Public Const CgUpTXTFR As String				= "\TextesVF.txt"
	Public Const CgUpSeries As String				= "\Series.txt"
	Public Const CgUpAutorisations As String		= "\Tournois.txt"
	Public Const CgUpRulings As String				= "\Rulings.xml"
	Public Const CgUpPrices As String				= "\LastPrices.txt"
	Public Const CgUpPic As String					= "\SP_Pict"
	Public Const CgMdPic As String					= "MD_Pict"
	Public Const CgMdTrad As String					= "\MD_Trad.log"
	Public Const CgShell As String					= "explorer.exe"
	Public Const CgDefaultServer As String			= "http://couitchy.free.fr/upload/MTGM"
	Public Const CgURL1 As String         			= "/Updates/TimeStamp r4.txt"
	Public Const CgURL1B As String         			= "/Updates/Beta/TimeStamp.txt"
	Public Const CgURL1C As String         			= "/Updates/PicturesStamp.txt"
	Public Const CgURL1D As String         			= "/Updates/ContenuStamp r14.txt"
	Public Const CgURL1E As String         			= "/Updates/ContenuSizes r14.txt"
	Public Const CgURL2 As String         			= "/Updates/Magic The Gathering Manager r4.new"
	Public Const CgURL2B As String         			= "/Updates/Beta/Magic The Gathering Manager.new"
	Public Const CgURL3 As String         			= "/Updates/Images DB.mdb"
	Public Const CgURL3B As String         			= "/Updates/Patch r13.mdb"
	Public Const CgURL4 As String					= "/Listes%20des%20editions/"
	Public Const CgURL5 As String					= "/Logos%20des%20editions/"
	Public Const CgURL6 As String					= "http://gatherer.wizards.com/Pages/Default.aspx"
	Public Const CgURL7 As String         			= "/Updates/Historique.txt"
	Public Const CgURL8 As String         			= "/Lib/"
	Public Const CgURL9 As String         			= "/Updates/LastPrices.txt"
	Public Const CgURL10 As String					= "/Images%20des%20cartes/"
	Public Const CgURL11 As String         			= "/Updates/TextesVF.txt"
	Public Const CgURL12 As String         			= "/Updates/Series r14.txt"
	Public Const CgURL13 As String         			= "/Updates/MTGM.pdf"
	Public Const CgURL14 As String         			= "/Updates/MD_Trad.log"
	Public Const CgURL15 As String         			= "/Updates/Tournois r11.txt"
	Public Const CgURL16 As String					= "https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=couitchy@free.fr&lc=FR&item_name=Magic The Gathering Manager&currency_code=EUR&bn=PP%2dDonationsBF"
	Public Const CgURL17 As String					= "http://mtgm.free.fr"
	Public Const CgURL18 As String					= "mailto:couitchy@free.fr?subject=Magic The Gathering Manager&body=Votre message ici"
	Public Const CgURL19 As String         			= "/Updates/Rulings.xml"
	Public Const CgDL1 As String         			= "V�rification des mises � jour..."
	Public Const CgDL2 As String         			= "T�l�chargement en cours"
	Public Const CgDL2b As String         			= "Un t�l�chargement est d�j� en cours..." + vbCrLf + "Veuillez attendre qu'il se termine avant de r�essayer."
	Public Const CgDL2c As String 					= "Une mise � jour des images est en cours." + vbCrLf + "Veuillez patienter avant d'essayer de les utiliser..."
	Public Const CgDL3 As String         			= "Erreur lors du t�l�chargement"
	Public Const CgDL3b As String					= "La connexion au serveur a �chou�..." + vbCrLf + "V�rifier la connectivit� � Internet et les param�tres du pare-feu."
	Public Const CgDL4 As String         			= "T�l�chargement termin�"
	Public Const CgDL5 As String         			= "T�l�chargement annul�"
	Public Const CgErr0 As String					= "Des fichiers n�cessaires sont manquants..."
	Public Const CgErr1 As String					= "Les mod�les de simulation sont absents ou incomplets..." + vbCrLf + "Proc�dez � la mise � jour depuis le menu 'Fichier' de la fen�tre principale..."
	Public Const CgErr2 As String					= "L'historique des prix est vide..."
	Public Const CgErr3 As String					= "Impossible d'afficher les informations demand�es maintenant..." + vbCrLf + "Si une mise � jour est en cours, merci d'attendre qu'elle se finisse."
	Public Const CgErr4 As String					= "Le nombre maximal de courbes affichables a �t� atteint..." + vbCrLf + "Les suivantes seront ignor�es."
	Public Const CgErr5 As String					= "Le processus de mise � jour a �t� interrompu..."
	Public Const CgErr6 As String					= "Le plug-in sp�cifi� est introuvable..."
	Public Const CgErr7 As String					= "Aucun crit�re de classement n'a �t� s�lectionn�..."
	Public Const CgErr8 As String					= "A la suite d'une mise � jour, vos pr�f�rences ont �t� r�initialis�es." + vbCrLf + "Merci de v�rifier dans Gestion / Pr�f�rences les diff�rents chemins des fichiers. Il est possible que certaines mises � jour de contenu devront �tre re-t�l�charg�es..."
	Public Const CgFExtO As String					= ".dck"
	Public Const CgFExtN As String					= ".dk2"
	Public Const CgFExtA As String					= ".dec"
	Public Const CgFExtW As String					= ".mwDeck"
	Public Const CgFExtM As String					= ".xml"
	Public Const CgFExtD As String					= ".mdb"
	Public Const CgIconsExt As String				= ".png"
	Public Const CgPicUpExt As String				= ".dat"
	Public Const CgPicLogExt As String				= ".log"
	Public Const CgImgSeries As String				= "_series_"
	Public Const CgImgColors As String				= "_colors_"
	Public Const CgDefaultName As String			= "(Deck)"
	Public Const CgDefaultFormat As String			= "Classique"
	Public Const CgRulings As String				= "R�gles sp�cifiques"
	Public Const CgPlateau As String				= "Plateau de jeu : "
	Public Const CgStats As String					= "Statistiques : "
	Public Const CgSimus As String					= "Simulations : "
	Public Const CgSimus3 As String					= "Proba. s�quence(s) pour "
	Public Const CgSimus4 As String					= "Manas productibles pour "
	Public Const CgSimus5 As String					= "D�faut de manas pour "
	Public Const CgRefresh As String				= "Rafra�chir"
	Public Const CgPanel As String					= "Ouvrir / fermer panneau image"
	Public Const CgStock As String					= "Nombre d�j� en stock"
	Public Const CgCollection As String	  			= "Collection"
	Public Const CgSCollection As String  			= "MyCollection"
	Public Const CgSDecks As String		  			= "MyGames"
	Public Const CgFromSearch As String				= "Recherche"
	Public Const CgSFromSearch As String			= "MySearch"
	Public Const CgCard As String		  			= "(carte)"
	Public Const CgPerfsEfficiency As String 		= "Calcul du facteur d'efficacit�" + vbCrLf + "----------------------------------" + vbCrLf + "NB. Ce calcul n'a de sens que si tous les jeux en pr�sence ont �t� saisis dans la base (afin d'en conna�tre leur prix)." + vbCrLf + "~1, le jeu est � la hauteur de son prix (jeu normal)" + vbCrLf + "<1, le jeu gagne plus de parties qu'il n'en devrait compte tenu de son prix (jeu efficient)" + vbCrLf + ">1, le jeu gagne moins de parties qu'il n'en devrait compte tenu de son prix (jeu soit mauvais / soit ""bulldozer"")" + vbCrLf + "(un r�sultat n�gatif signifie qu'il manque des informations pour le calcul : prix du jeu, r�sultats de parties...)" + vbCrLf + vbCrLf
	Public Const CgTransactionsMV As String			= "Transaction(s) � effectuer :"
	Public Const CgPerfsVersion As String 			= "nouv."
	Public Const CgPerfsTotal As String   			= "TOTAL "
	Public Const CgPerfsTotalV As String  			= "toutes"
	Public Const CgPerfsVFree As String   			= "sans version"
	Public Const CgAlternateStart As String 		= "Card Name:"
	Public Const CgAlternateStart2 As String		= "Name:"
	Public Const CgFoil As String					= "PREMIUMFOILVO"
	Public Const CgFoil2 As String					= " (Foil)"
	Public CgBalises() As String 					= {"CardName:", "Cost:", "Type:", "Pow/Tgh:", "Rules Text:", "Set/Rarity:"}
	Public CgManaParsing() As String 				= {"to your mana pool", "add", "either ", " or ", " colorless mana", " mana of any color", " mana"}
	Public CgCriterionsFields() As String 			= {"Card.Type", "Spell.Color", "Card.Series", "Spell.myCost", "Card.Rarity", "Card.myPrice", "Card.Title"}
	Public CgNumbers() As String 					= {"one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten"}
	Public CgRarities() As String					= {"'M'", "'R'", "'U'", "'C'", "'D'", "'L'", "'S'"}
	Public CgSearchFields() As String 				= {"Card.Title", "CardFR.TitleFR", "Card.CardText", "TextesFR.TexteFR", "Creature.Power", "Creature.Tough", "Card.Price", "Card.Series", "Card.Series", "Spell.myCost", "Card.SubType", "SubTypes.SubTypeVF"}
	Public CgRequiredFiles() As String				= {"\TreeViewMS.dll", "\ChartFX.Lite.dll", "\NPlot.dll", "\SandBar.dll", "\SourceGrid2.dll", "\SourceLibrary.dll", CgMagicBack, CgUpdater}
	Public CgStrConn() As String      				= {"Provider=Microsoft.Jet.OLEDB.4.0;OLE DB Services=-1;Data Source=", "Provider=Microsoft.ACE.OLEDB.12.0;Data Source="}
	Public CgCriteres As New Hashtable(CgNCriterions)
	Public CgVirtualPath As String
	Public VgDB As OleDbConnection
	Public VgDBCommand As New OleDbCommand
	Public VgDBReader As OleDbDataReader
	Public VgImgSeries As New ImageList
	Public VgRemoteDate As Date
	Public VgOptions As New Options
	Public VgSessionSettings As New clsSessionSettings
	Public VgRandom As New Random(Now.Millisecond)
	Public WithEvents VgTray As NotifyIcon
	Public WithEvents VgTimer As Timer
	Public WithEvents VgClient As New WebClient
	Public Enum eFormat
		MTGMv2 = 0
		MTGM
		Apprentice
		MWS
	End Enum
	Public Enum eSearchType
		Alpha = 0
		Num
		NumOverAlpha
	End Enum
	Public Enum eForbiddenCharset
		Standard = 0
		BDD
		Full
	End Enum
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
		Type
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
	Public Enum eModeCarac
		Serie = 0
		Couleur
		Type
	End Enum
	Public Enum eDBVersion
		Unknown	= 0	'version inconnue (base corrompue)
		BDD_v1		'manque infos MyGamesID, SubTypes, CardDouble, tournois M, ajustement types num�riques, manque Adversaires, manque Historique prix, Autorisations, TextesFR, jeux ind�pendants dans MyScores, SpecialUse et MySpecialUses, MyGamesID et MyScores (+ �ventuellement CardPictures, mais non g�r�, r�installation par l'utilisateur n�cessaire)
		BDD_v2		'manque infos MyGamesID, SubTypes, CardDouble, tournois M, ajustement types num�riques, manque Adversaires, manque Historique prix, Autorisations, TextesFR, jeux ind�pendants dans MyScores, SpecialUse et MySpecialUses, MyGamesID et les versions dans MyScores
		BDD_v3		'manque infos MyGamesID, SubTypes, CardDouble, tournois M, ajustement types num�riques, manque Adversaires, manque Historique prix, Autorisations, TextesFR, jeux ind�pendants dans MyScores, SpecialUse et MySpecialUses, MyGamesID
		BDD_v4		'manque infos MyGamesID, SubTypes, CardDouble, tournois M, ajustement types num�riques, manque Adversaires, manque Historique prix, Autorisations, TextesFR, jeux ind�pendants dans MyScores, SpecialUse et MySpecialUses
		BDD_v5		'manque infos MyGamesID, SubTypes, CardDouble, tournois M, ajustement types num�riques, manque Adversaires, manque Historique prix, Autorisations, TextesFR
		BDD_v6		'manque infos MyGamesID, SubTypes, CardDouble, tournois M, ajustement types num�riques, manque Adversaires, manque Historique prix, Autorisations
		BDD_v7		'manque infos MyGamesID, SubTypes, CardDouble, tournois M, ajustement types num�riques, manque Adversaires, manque Historique prix
		BDD_v8		'manque infos MyGamesID, SubTypes, CardDouble, tournois M, ajustement types num�riques, manque Adversaires
		BDD_v9		'manque infos MyGamesID, SubTypes, CardDouble, tournois M, ajustement types num�riques
		BDD_v10		'manque infos MyGamesID, SubTypes, CardDouble, tournois M
		BDD_v11		'manque infos MyGamesID, SubTypes, CardDouble
		BDD_v12		'manque infos MyGamesID, SubTypes
		BDD_v13		'manque infos MyGamesID
		BDD_v14		'� jour
	End Enum
	Public Enum eDBProvider
		Jet = 0
		ACE
	End Enum
	Public Sub Main(ByVal VpArgs() As String)
	'-------------------------------
	'Point d'entr�e de l'application
	'-------------------------------
	Dim VpStartup As String = ""
		'Gestion globale des exceptions
		AddHandler Application.ThreadException, AddressOf ThreadExceptionHandler
		AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf DomainExceptionHandler
		'Gestion r�pertoire virtuel UAC (Vista / 7)
		CgVirtualPath = Process.GetCurrentProcess.MainModule.FileName.Replace("Magic The Gathering Manager.exe", "")
		CgVirtualPath = Environment.GetEnvironmentVariable("USERPROFILE") + "\AppData\Local\VirtualStore" + CgVirtualPath.Substring(2)
		'Ex�cution du formulaire de d�marrage
		If Not PreventMultipleInstances Then
			Application.EnableVisualStyles
			Application.Run(New MainForm(VpArgs))
		Else
			Application.Exit
		End If
	End Sub
	Private Function PreventMultipleInstances As Boolean
	'---------------------------------------------------------------------------------------------------
	'V�rifie si une instance de MTGM est d�j� en cours d'ex�cution, auquel cas l'affiche au premier plan
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
	'V�rifie l'int�grit� de l'application
	'------------------------------------
		For Each VpFile As String In CgRequiredFiles
			'Si le fichier n'existe pas
			If Not File.Exists(Application.StartupPath + VpFile) Then
				'Essaie de le t�l�charger
				Call DownloadNow(New Uri(VgOptions.VgSettings.DownloadServer + VgOptions.VgSettings.DownloadServer + CgURL8 + VpFile.Replace("\", "")), VpFile)
				'Si le fichier n'existe toujours pas, on ne d�marre pas
				If Not File.Exists(Application.StartupPath + VpFile) Then
					Call ShowWarning(clsModule.CgErr0)
					Return False
				End If
			End If
		Next VpFile
		Return True
	End Function
	Public Function DBOpen(VpPath As String) As Boolean
	'--------------------------------------------------------------------------------
	'Essaie d'ouvrir la base de donn�es sp�cifi�e en param�tre et renvoie le r�sultat
	'--------------------------------------------------------------------------------
		If Not IO.File.Exists(VpPath) Then
			Return False
		Else
			VgDB = New OleDbConnection(CgStrConn(CInt(VgOptions.VgSettings.DBProvider)) + VpPath)
	    	Try
		    	VgDB.Open
		    	VgDBCommand.Connection = VgDB
		    	VgDBCommand.CommandType = CommandType.Text
		    	If Not DBVersion Then
					VgDB.Close
					VgDB.Dispose
					VgDB = Nothing
		    		Return False
		    	Else
		    		Return True
		    	End If
	    	Catch VpErr As Exception
	    		Call ShowWarning("Impossible d'ouvrir la base de donn�es s�lectionn�e..." + vbCrLf + "D�tails : " + VpErr.Message)
	    	End Try
	    End If
	    Return False
	End Function
	Private Function DBVersion As Boolean
	'------------------------------------
	'V�rifie la version de la BDD ouverte
	'------------------------------------
	Dim VpDBVersion As eDBVersion = eDBVersion.Unknown
	Dim VpSchemaTable As DataTable = VgDB.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, New Object() {Nothing, Nothing, Nothing, "TABLE"})
	Dim VpTablesCount As Integer = VpSchemaTable.Rows.Count
		'D�termination de la version de la base de donn�es suivant le nombre d'�l�ments qu'elle contient
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
				VpSchemaTable = VgDB.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, New Object() {Nothing, Nothing, "Card", Nothing})
				If CInt(VpSchemaTable.Rows(11)!DATA_TYPE) <> 4 Then
					'Si on est ici, BDD version 9
					VpDBVersion = eDBVersion.BDD_v9
				Else
					VpSchemaTable = VgDB.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, New Object() {Nothing, Nothing, "Autorisations", Nothing})
					If VpSchemaTable.Rows.Count <> 8 Then
						'Si on est ici, BDD version 10
						VpDBVersion = eDBVersion.BDD_v10
					Else
						If VpTablesCount < 17 Then
							'Si on est ici, BDD version 11
							VpDBVersion = eDBVersion.BDD_v11
						ElseIf VpTablesCount < 18 Then
							'Si on est ici, BDD version 12
							VpDBVersion = eDBVersion.BDD_v12
						Else
							VpSchemaTable = VgDB.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, New Object() {Nothing, Nothing, "MyGamesID", Nothing})
							If VpSchemaTable.Rows.Count <> 6 Then
								'Si on est ici, BDD version 13
								VpDBVersion = eDBVersion.BDD_v13
							Else
								'Si on est ici, BDD version 14
								VpDBVersion = eDBVersion.BDD_v14
							End If
						End If
					End If
				End If
			End If
		Else
			'Si on est ici, BDD version 1
			VpDBVersion = eDBVersion.BDD_v1
		End If
		'Actions � effectuer en cons�quence
		If VpDBVersion = eDBVersion.Unknown Then		'Version inconnue
			Return False
		ElseIf VpDBVersion = eDBVersion.BDD_v14 Then	'Derni�re version
			Return True
		Else											'Versions interm�diaires
			If ShowQuestion("La base de donn�es (v" + CInt(VpDBVersion).ToString + ") doit �tre mise � jour pour devenir compatible avec la nouvelle version du logiciel (v14)..." + vbCrlf + "Continuer ?") = DialogResult.Yes Then
				Try
					'Passage version 1 � 2
					If CInt(VpDBVersion) < 2 Then
						VgDBCommand.CommandText = "Create Table MyScores (JeuLocal Text(50) With Compression, JeuAdverse Text(50) With Compression, Victoire Bit);"
						VgDBCommand.ExecuteNonQuery
					End If
					'Passage version 2 � 3
					If CInt(VpDBVersion) < 3 Then
						VgDBCommand.CommandText = "Alter Table MyScores Add JeuLocalVersion Text(10) With Compression;"
						VgDBCommand.ExecuteNonQuery
						VgDBCommand.CommandText = "Alter Table MyScores Add JeuAdverseVersion Text(10) With Compression;"
						VgDBCommand.ExecuteNonQuery
					End If
					'Passage version 3 � 4
					If CInt(VpDBVersion) < 4 Then
						Try
							VgDBCommand.CommandText = "Create Table MyGamesID (GameID Integer, GameName Text(50) With Compression);"
							VgDBCommand.ExecuteNonQuery
						Catch
						End Try
					End If
					'Passage version 4 � 5
					If CInt(VpDBVersion) < 5 Then
						VgDBCommand.CommandText = "Create Table MySpecialUses (EffortID Integer, EffetID Integer, Card Text(80) With Compression, Effort Text(255) With Compression, Effet Text(255) With Compression);"
						VgDBCommand.ExecuteNonQuery
						VgDBCommand.CommandText = "Create Table SpecialUse (SpecID Integer, IsEffort Bit, Description Text(255) With Compression);"
						VgDBCommand.ExecuteNonQuery
						VgDBCommand.CommandText = "Alter Table MyScores Add IsMixte Bit;"
						VgDBCommand.ExecuteNonQuery
					End If
					'Passage version 5 � 6
					If CInt(VpDBVersion) < 6 Then
						VgDBCommand.CommandText = "Create Table TextesFR (CardName Text(80) With Compression, TexteFR Memo);"
						VgDBCommand.ExecuteNonQuery
					End If
					'Passage version 6 � 7
					If CInt(VpDBVersion) < 7 Then
						VgDBCommand.CommandText = "Create Table Autorisations (Title Text(80) With Compression, T1 Bit, T1r Bit, T15 Bit, T1x Bit, T2 Bit, Bloc Bit);"
						VgDBCommand.ExecuteNonQuery
					End If
					'Passage version 7 � 8
					If CInt(VpDBVersion) < 8 Then
						VgDBCommand.CommandText = "Create Table PricesHistory (EncNbr Long, PriceDate Date, Price Single);"
						VgDBCommand.ExecuteNonQuery
					End If
					'Passage version 8 � 9
					If CInt(VpDBVersion) < 9 Then
						VgDBCommand.CommandText = "Create Table MyAdversairesID (AdvID Long, AdvName Text(255) With Compression);"
						VgDBCommand.ExecuteNonQuery
						VgDBCommand.CommandText = "Insert Into MyAdversairesID(AdvID, AdvName) Values (0, '" + CgMe + "');"
						VgDBCommand.ExecuteNonQuery
						VgDBCommand.CommandText = "Alter Table MyGamesID Add AdvID Long;"
						VgDBCommand.ExecuteNonQuery
						VgDBCommand.CommandText = "Update MyGamesID Set AdvID = 0;"
						VgDBCommand.ExecuteNonQuery
						VgDBCommand.CommandText = "Alter Table MyScores Drop Column IsMixte;"
						VgDBCommand.ExecuteNonQuery
					End If
					'Passage version 9 � 10
					If CInt(VpDBVersion) < 10 Then
						Call DBChangeType("Card", "Price", "Single", "Val")
						Call DBChangeType("Card", "myPrice", "Integer", "Int")
						Call DBChangeType("Spell", "myCost", "Integer", "Int")
						VgDBCommand.CommandText = "Alter Table MyGames Add Foil Bit;"
						VgDBCommand.ExecuteNonQuery
						VgDBCommand.CommandText = "Alter Table MyCollection Add Foil Bit;"
						VgDBCommand.ExecuteNonQuery
						VgDBCommand.CommandText = "Alter Table PricesHistory Add Foil Bit;"
						VgDBCommand.ExecuteNonQuery
						Try		'normalement la cl� primaire n'existe pas (mais on ne sait jamais), d'o� la trappe pour �viter une exception
							VgDBCommand.CommandText = "Drop Index PrimaryKey On MyCollection;"
							VgDBCommand.ExecuteNonQuery
						Catch
						End Try
						VgDBCommand.CommandText = "Drop Index EncNbr On MyCollection;"
						VgDBCommand.ExecuteNonQuery
						VgDBCommand.CommandText = "Create Index EncNbr On MyCollection (EncNbr);"
						VgDBCommand.ExecuteNonQuery
					End If
					'Passage version 10 � 11
					If CInt(VpDBVersion) < 11 Then
						VgDBCommand.CommandText = "Alter Table Autorisations Add M Bit;"
						VgDBCommand.ExecuteNonQuery
					End If
					'Passage version 11 � 12
					If CInt(VpDBVersion) < 12 Then
						VgDBCommand.CommandText = "Alter Table Card Add SpecialDoubleCard Bit;"
						VgDBCommand.ExecuteNonQuery
						VgDBCommand.CommandText = "Update Card Set SpecialDoubleCard = False;"
						VgDBCommand.ExecuteNonQuery
						VgDBCommand.CommandText = "Create Table CardDouble (EncNbrDownFace Long, EncNbrTopFace Long);"
						VgDBCommand.ExecuteNonQuery
					End If
					'Passage version 12 � 13
					If CInt(VpDBVersion) < 13 Then
						VgDBCommand.CommandText = "Create Table SubTypes (SubTypeVO Text(32) With Compression, SubTypeVF Text(32) With Compression);"
						VgDBCommand.ExecuteNonQuery
						VgDBCommand.CommandText = "Alter Table Series Add SeriesNM_FR Text(50) With Compression;"
						VgDBCommand.ExecuteNonQuery
						VgDBCommand.CommandText = "Update Series Set SeriesNM_FR = SeriesNM;"
						VgDBCommand.ExecuteNonQuery
					End If
					'Passage version 13 � 14
					If CInt(VpDBVersion) < 14 Then
						VgDBCommand.CommandText = "Alter Table MyGamesID Add GameDate Date;"
						VgDBCommand.ExecuteNonQuery
						VgDBCommand.CommandText = "Alter Table MyGamesID Add GameFormat Text(63) With Compression;"
						VgDBCommand.ExecuteNonQuery
						VgDBCommand.CommandText = "Alter Table MyGamesID Add GameDescription Memo With Compression;"
						VgDBCommand.ExecuteNonQuery
						VgDBCommand.CommandText = "Update MyGamesID Set GameDate = '" + Now.ToShortDateString + "';"
						VgDBCommand.ExecuteNonQuery
						VgDBCommand.CommandText = "Update MyGamesID Set GameFormat = '" + clsModule.CgDefaultFormat + "';"
						VgDBCommand.ExecuteNonQuery
					End If
				Catch
					Call ShowWarning("Un probl�me est survenu pendant la mise � jour de la base de donn�es...")
					Return False
				End Try
				Return True
			Else
				Return False
			End If
		End If
	End Function
	Private Sub DBChangeType(VpTable As String, VpField As String, VpType As String, VpCaster As String)
	'-----------------------------------------------------------------------------------------------------------------
	'Jet-SQL ne supporte pas le changement de type d'une colonne avec conversion implicite, d'o� la routine ci-dessous
	'-----------------------------------------------------------------------------------------------------------------
		VgDBCommand.CommandText = "Alter Table " + VpTable + " Add " + VpField + "2 " + VpType + ";"
		VgDBCommand.ExecuteNonQuery
		VgDBCommand.CommandText = "Update " + VpTable + " Set " + VpField + "2 = " + VpCaster + "(" + VpField + ");"
		VgDBCommand.ExecuteNonQuery
		VgDBCommand.CommandText = "Alter Table " + VpTable + " Drop Column " + VpField + ";"
		VgDBCommand.ExecuteNonQuery
		VgDBCommand.CommandText = "Alter Table " + VpTable + " Add " + VpField + " " + VpType + ";"
		VgDBCommand.ExecuteNonQuery
		VgDBCommand.CommandText = "Update " + VpTable + " Set " + VpField + " = " + VpField + "2;"
		VgDBCommand.ExecuteNonQuery
		VgDBCommand.CommandText = "Alter Table " + VpTable + " Drop Column " + VpField + "2;"
		VgDBCommand.ExecuteNonQuery
	End Sub
	Public Function DBOK As Boolean
		If VgDB Is Nothing Then
			Call ShowWarning("Aucune base de donn�es n'a �t� s�lectionn�e...")
			Return False
		ElseIf MainForm.VgMe.IsMainReaderBusy Then
			Return False
		Else
			Return True
		End If
	End Function
	Public Sub DBImport(VpPath As String, Optional VpSilent As Boolean = False)
	'---------------------------------------------------------------------------------
	'Importe dans la base de donn�es l'ensemble du patch (structure + enregistrements)
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
	Dim VpDB As New OleDbConnection(CgStrConn(CInt(VgOptions.VgSettings.DBProvider)) + VpPath)
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
			'Si la table existe d�j� dans la base mais qu'elle doit �tre pr�alablement d�truite
			If PreDelete(VpTable) Then
				Try
					VgDBCommand.CommandText = "Drop Table " + VpTable + ";"
					VgDBCommand.ExecuteNonQuery
				Catch
				End Try
			End If
			'Si la table n'existe pas (ou plus) dans la base
			If Not IsInDB(VpTable) Then
				'Pr�pare la requ�te de cr�ation de table (m�thode SQL)
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
				'La table �tant pr�sente, on peut ins�rer les nouvelles donn�es	(m�thode OLEDB/DataSet)
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
			Call ShowInformation("Mise � jour effectu�e." + vbCrLf + "Assurez-vous d'avoir �galement la derni�re version du logiciel...")
		End If
		'Supprime le patch
		VpDB.Close
		VpDB.Dispose
		VpDB = Nothing
		Call SecureDelete(VpPath)
	End Sub
	Public Sub DBAdaptEncNbr
	'--------------------------------------------------------------------------------------------------------------------------
	'Toutes les �ditions ne sont pas forc�ment import�es dans le m�me ordre chez les utilisateurs, d'o� des EncNbr diff�rents :
	'=> cette proc�dure d�termine les bons EncNbr � partir du nom de la carte et de son �dition
	'--------------------------------------------------------------------------------------------------------------------------
		VgDBCommand.CommandText = "Alter Table PricesHistory Add EncNbr Long;"
		VgDBCommand.ExecuteNonQuery
    	VgDBCommand.CommandText = "Update Card Inner Join PricesHistory On PricesHistory.Title = Card.Title And PricesHistory.Series = Card.Series Set PricesHistory.EncNbr = Card.EncNbr Where PricesHistory.Title = Card.Title And PricesHistory.Series = Card.Series;"
    	VgDBCommand.ExecuteNonQuery
		VgDBCommand.CommandText = "Alter Table PricesHistory Drop Column Series;"
		VgDBCommand.ExecuteNonQuery
		VgDBCommand.CommandText = "Alter Table PricesHistory Drop Column Title;"
		VgDBCommand.ExecuteNonQuery
		VgDBCommand.CommandText = "Create Index EncNbr On PricesHistory (EncNbr);"
		VgDBCommand.ExecuteNonQuery
	End Sub
	Private Function PreDelete(VpTable As String) As Boolean
	'---------------------------------------------------------------------------------------------
	'Indique si la table sp�cifi�e en param�tre doit �tre supprim�e lors de l'application du patch
	'---------------------------------------------------------------------------------------------
		Select Case VpTable
			Case "MySpecialUses"
				'Essaie d'utiliser le champ large ; si erreur => on doit recr�er la table avec un champ plus grand
				Try
					VgDBCommand.CommandText = "Insert Into MySpecialUses Values (-1, -1, 'X', '" + StrBuild("X", 128) + "', '" + StrBuild("X", 128) + "', False, False);"
					VgDBCommand.ExecuteNonQuery
					VgDBCommand.CommandText = "Delete * From MySpecialUses Where EffortID = -1;"		'un peu crade mais je n'ai pas trouv� plus simple !
					VgDBCommand.ExecuteNonQuery
				Catch
					Return True
				End Try
				Return False
			Case Else
				Return True
		End Select
	End Function
	Private Function IsInDB(VpTable As String) As Boolean
	'------------------------------------------------------------------------------------------------
	'Renvoie faux si la table sp�cifi�e en param�tre n'existe pas ou est vide dans la base de donn�es
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
	'Charge en m�moire les ic�nes / ressources de l'application
	'----------------------------------------------------------
	Dim VpHandle As Image
	Dim VpKey As String
		If Not System.IO.Directory.Exists(Application.StartupPath + CgIcons) Then
			Call ShowWarning("Impossible de trouver le r�pertoire des ressources...")
			Return False
		Else
			VgImgSeries.ColorDepth = ColorDepth.Depth32Bit
			VgImgSeries.ImageSize = New Size(21, 21)
			VgImgSeries.TransparentColor = System.Drawing.Color.Transparent
			For Each VpIcon As String In System.IO.Directory.GetFiles(Application.StartupPath + CgIcons, "*" + CgIconsExt)
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
	'Bo�tes de dialogue
	'------------------
	Public Sub ShowWarning(VpStr As String)
		MessageBox.Show(VpStr, "Probl�me", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1)
	End Sub
	Public Sub ShowInformation(VpStr As String)
		MessageBox.Show(VpStr, "Information", MessageBoxbuttons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
	End Sub
	Public Function ShowQuestion(VpStr As String, Optional VpButtons As MessageBoxButtons = MessageBoxbuttons.YesNo) As DialogResult
		Return MessageBox.Show(VpStr, "Question", VpButtons, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
	End Function
	Public Function Matching(VpStr As String) As Object
	'-------------------------------------------
	'Adapte une cha�ne en un type plus appropri�
	'-------------------------------------------
		If VpStr = "True" Then
			Return True
		ElseIf VpStr = "False" Then
			Return False
		ElseIf IsNumeric(VpStr)
			Return CInt(VpStr)
		Else
			Return VpStr
		End If
	End Function
	Public Function TrimQuery(VpSQL As String, Optional VpDot As Boolean = True, Optional VpAddendum As String = "") As String
	'----------------------------------
	'Suppression des mots-cl�s inutiles
	'----------------------------------
	Dim VpTrimSQL As String
		If VpSQL.EndsWith(" Where ") Then
			VpTrimSQL = VpSQL.Substring(0, VpSQL.Length - 7)
		ElseIf VpSQL.EndsWith(" And ") Then
			VpTrimSQL = VpSQL.Substring(0, VpSQL.Length - 5)
		Else
			VpTrimSQL = VpSQL
		End If
		If VpDot Then
			Return VpTrimSQL + VpAddendum + ";"
		Else
			Return VpTrimSQL + VpAddendum
		End If
	End Function
	Public Function FormatTitle(VpTag As String, VpStr As String, Optional VpFR As Boolean = False, Optional VpIsForTvw As Boolean = True) As String
	'-------------------------------------------------------------------
	'Modifie l'expression pass�e en param�tre en un titre plus explicite
	'-------------------------------------------------------------------
	Dim VpDBCommand As OleDbCommand
		Select Case VpTag
			Case "Card.Series"
				Try
					VpDBCommand = New OleDbCommand
    				VpDBCommand.Connection = VgDB
    				VpDBCommand.CommandType = CommandType.Text
					VpDBCommand.CommandText = "Select " + If(VpFR, "SeriesNM_FR", "SeriesNM") + " From Series Where SeriesCD = '" + VpStr + "';"
					Return VpDBCommand.ExecuteScalar.ToString
				Catch
					Return VpStr
				End Try
			Case "Card.Type"
				Select Case VpStr.ToUpper
					Case "C"
						Return "Cr�atures"
					Case "I"
						Return "Eph�m�res"
					Case "A"
						Return "Artefacts"
					Case "E"
						Return "Auras"
					Case "L"
						Return "Terrains"
					Case "N"
						Return "Interruptions"
					Case "S"
						Return "Rituels"
					Case "T"
 						Return "Enchantements"
 					Case "U"
						Return "Cr�atures avec capacit�"
					Case "P"
						Return "Arpenteurs"
					Case "Q"
						Return "Plans"
					Case "H"
						Return "Ph�nom�nes"
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
					'Cas mal g�r� des double cartes
					Case "X"
						Return "Double-cartes"
					Case Else
						Return VpStr
				End Select
			Case "Spell.myCost"
				If VpIsForTvw Then
					Return ""
				Else
					Return VpStr
				End If
			Case "Card.myPrice"
				Select Case VpStr
					Case "1"
						Return "Moins de 0,50�"
					Case "2"
						Return "Entre 0,50� et 1�"
					Case "3"
						Return "Entre 1� et 3�"
					Case "4"
						Return "Entre 3� et 5�"
					Case "5"
						Return "Entre 5� et 10�"
					Case "6"
						Return "Entre 10� et 20�"
					Case "7"
						Return "Entre 20� et 50�"
					Case "8"
						Return "Plus de 50 �"
					Case Else
						Return VpStr
				End Select
			Case "Card.Rarity"
				Select Case VpStr.Substring(0, 1).ToUpper
'					Case "M"
'						Return ("Mythiques (" + VpStr.Substring(1) + ")").Replace("()","")
'					Case "R"
'						Return ("Rares (" + VpStr.Substring(1) + ")").Replace("()","")
'					Case "U"
'						Return ("Peu communes (" + VpStr.Substring(1) + ")").Replace("()","")
'					Case "C"
'						Return ("Communes (" + VpStr.Substring(1) + ")").Replace("()","")
'					Case "D", "L", "S"
'						Return ("Sans valeur (" + VpStr.Substring(1) + ")").Replace("()","")
					Case "M"
						Return "Mythiques"
					Case "R"
						Return "Rares"
					Case "U"
						Return "Peu communes"
					Case "C"
						Return "Communes"
					Case "D", "L", "S"
						Return "Sans valeur"
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
	'Retourne la cat�gorie du prix correspondant � sa valeur
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
	'Retourne le co�t converti de mana de l'invocation pass�e en param�tre
	'---------------------------------------------------------------------
	Dim VpColorless As Integer
	    If VpStr = "0" Then
	    	Return 0
	    Else
	        VpColorless = Val(VpStr)
	        If VpColorless <> 0 Then
	            Return VpStr.Replace(VpColorless.ToString.Trim, "").Length + VpColorless - 4 * StrCount(VpStr, "(")
	        Else
	            Return VpStr.Length - 4 * StrCount(VpStr, "(")
	        End If
	    End If
	End Function
	Public Function MyTxt(VpCard As String, VpVF As Boolean, VpDownFace As Boolean) As String
	'----------------------------------------------------
	'Retourne le texte VF de la carte pass�e en param�tre
	'----------------------------------------------------
	Dim VpDBCommand As New OleDbCommand
	Dim VpO As Object
	Dim VpStr As String
    	VpDBCommand.Connection = VgDB
    	VpDBCommand.CommandType = CommandType.Text
    	If VpVF Then
			VpDBCommand.CommandText = "Select TexteFR From TextesFR Where CardName = '" + VpCard.Replace("'", "''") + "';"
    	Else
    		VpDBCommand.CommandText = "Select CardText From Card Where Title = '" + VpCard.Replace("'", "''") + "';"
    	End If
		VpO = VpDBCommand.ExecuteScalar
		If Not VpO Is Nothing Then
			VpStr = VpO.ToString
			If VpStr.Contains("//") Then
				If VpDownFace Then
					Return VpStr.Substring(VpStr.IndexOf("//") + 6).Trim
				Else
					Return VpStr.Substring(0, VpStr.IndexOf("//") - 5).Trim
				End If
			Else
				Return VpStr.Trim
			End If
		Else
			Return ""
		End If
	End Function
	Public Function MyClone(VpA As List(Of clsLocalCard)) As List(Of clsLocalCard)
	'-------------------------------------------
	'Duplication de la liste des cartes d�sir�es
	'-------------------------------------------
	Dim VpB As New List(Of clsLocalCard)
		For Each VpLocalCard As clsLocalCard In VpA
			VpB.Add(New clsLocalCard(VpLocalCard.Name, VpLocalCard.Quantite))
		Next VpLocalCard
		Return VpB
	End Function
	Public Function MyClone2(VpA As List(Of clsMVCard)) As List(Of clsMVCard)
	'-------------------------------------------
	'Duplication de la liste des cartes d�sir�es
	'-------------------------------------------
	Dim VpB As New List(Of clsMVCard)
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
	'Retourne le nombre d'occurences du caract�re sp�cifi� dans la cha�ne pass�e en param�tre
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
	Public Function StrDiacriticInsensitize(VpStr As String) As String
	'-----------------------------------------------------------------------------------------------------------------------------------------------
	'Rend la requ�te SQL insensible aux signes diacritiques pour le terme pass� en param�tre (NB. elle l'est d�j� vis-�-vis de la casse avec JetSQL)
	'-----------------------------------------------------------------------------------------------------------------------------------------------
	Dim VpStrSB As New StringBuilder
	Dim VpCur As Char
		For VpI As Integer = 0 To VpStr.Length - 1
			VpCur = VpStr.Substring(VpI, 1)
			Select Case VpCur
				Case "e", "�", "�", "�", "�", "E", "�", "�", "�", "�"
					VpStrSB.Append("[e����]")
				Case "a", "�", "�", "�", "A", "�", "�", "�"
					VpStrSB.Append("[a���]")
				Case "i", "�", "�", "�", "I", "�", "�", "�"
					VpStrSB.Append("[i���]")
				Case "o", "�", "�", "�", "O", "�", "�", "�"
					VpStrSB.Append("[o���]")
				Case "u", "�", "�", "�", "U", "�", "�", "�"
					VpStrSB.Append("[u���]")
				Case "c", "�", "C", "�"
					VpStrSB.Append("[c�]")
				Case Else
					VpStrSB.Append(VpCur)
			End Select
		Next VpI
		Return VpStrSB.ToString
	End Function
	Public Function MyVal(VpStr As String) As Double
		Return Val(VpStr.Replace(",", "."))
	End Function
	Public Function FindIndex(VpTab() As String, VpValue As String) As Integer
	'--------------------------------------------------------------------------------------------------------------
	'Retourne l'indice de la premi�re occurence de la valeur pass�e en param�tre dans le tableau pass� en param�tre
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
	'Retourne le nombre correspondant � la cha�ne (texte ou nombre) pass�e en param�tre
	'----------------------------------------------------------------------------------
		If IsNumeric(VpStr) Then
			Return Val(VpStr)
		Else
			Return FindIndex(CgNumbers, VpStr) + 1
		End If
	End Function
	Public Function SafeGetChecked(VpObj As Object) As Boolean
		Try
			Return VpObj.Checked
		Catch
			Return False
		End Try
	End Function
	Public Function AvoidForbiddenChr(ByVal VpIn As String, Optional VpChrSet As eForbiddenCharset = eForbiddenCharset.Standard) As String
		Select Case VpChrSet
			Case eForbiddenCharset.Standard
				Return VpIn.Replace(":", "").Replace("/", "").Replace("""", "").Replace("?", "")
			Case eForbiddenCharset.BDD
				Return AvoidForbiddenChr(VpIn.Replace("'", "''"))
			Case eForbiddenCharset.Full
				Return AvoidForbiddenChr(VpIn.Replace("\", "").Replace("*", "").Replace("<", "").Replace(">", "").Replace("|", ""))
			Case Else
				Return VpIn
		End Select
	End Function
	Public Function SafeGetText(VpObj As Object) As String
		Try
			Return VpObj.Text
		Catch
			Return ""
		End Try
	End Function
	Public Function SafeGetNonZeroVal(VpColumn As String) As Single
		With VgDBReader
			If .GetValue(.GetOrdinal(VpColumn)) Is DBNull.Value Then
				Return 0
			Else
				Return .GetValue(.GetOrdinal(VpColumn))
			End If
		End With
	End Function
	Public Sub InitCriteres(VpMainForm As MainForm)
		For VpI As Integer = 0 To VpMainForm.FilterCriteria.NCriteria - 1
			CgCriteres.Add(VpMainForm.FilterCriteria.MyList.Items(VpI), CgCriterionsFields(VpI))
		Next VpI
	End Sub
	Public Function GetPictSP As String
	Dim VpRequest As HttpWebRequest
	Dim VpResponse As WebResponse
	Dim VpAnswer As Stream
	Dim VpBuf() As Byte
	Dim VpStamp As String
			VpRequest = WebRequest.Create(VgOptions.VgSettings.DownloadServer + CgURL1C)
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
	'V�rifie si une mise � jour de la base d'image est disponible sur Internet
	'-------------------------------------------------------------------------
	Dim VpStamp As String
	Dim VpStr As String
	Dim VpOldText As String
		VpOldText = MainForm.VgMe.StatusTextGet
		Call MainForm.VgMe.StatusText(CgDL1, True)
		VgTimer.Stop
		'V�rification par la taille
		Try
			VpStamp = GetPictSP
			VpStr = (New FileInfo(VgOptions.VgSettings.PicturesFile)).Length.ToString
			If VpStamp.Contains(VpStr) Then
				VpStr = VpStamp.Substring(VpStamp.IndexOf(VpStr) + VpStr.Length + 1)
				VpStr = VpStr.Substring(0, VpStr.IndexOf("#"))
				If VpStr = "OK"  Then
					Call ShowInformation("Les images sont d�j� � jour...")
					Call MainForm.VgMe.StatusText(VpOldText)
				Else
					'T�l�chargement du fichier accompagnateur
					Call DownloadNow(New Uri(VgOptions.VgSettings.DownloadServer + CgURL10 + VpStr + CgPicLogExt), CgUpPic + CgPicLogExt)
					Application.DoEvents
					'T�l�chargement du service pack d'images
					MainForm.VgMe.IsInImgDL = True
					Call DownloadUpdate(New Uri(VgOptions.VgSettings.DownloadServer + CgURL10 + VpStr + CgPicUpExt), CgUpPic + CgPicUpExt)
				End If
			Else
				If ShowQuestion("La base d'images semble �tre corrompue." + vbCrLf + "Voulez-vous la re-t�l�charger maintenant ?") = System.Windows.Forms.DialogResult.Yes Then
					'Re-t�l�chargement complet de la base principale
					MainForm.VgMe.IsInImgDL = True
					Call DownloadUpdate(New Uri(VgOptions.VgSettings.DownloadServer + CgURL10 + CgUpDDBd), VgOptions.VgSettings.PicturesFile, False)
				Else
					Call MainForm.VgMe.StatusText(VpOldText)
				End If
			End If
		Catch
			'En cas d'�chec de connexion
			Call ShowWarning(CgDL3b)
			Call MainForm.VgMe.StatusText(VpOldText)
		End Try
	End Sub
	Public Sub CheckForUpdates(Optional VpExplicit As Boolean = False, Optional VpBeta As Boolean = False, Optional VpContenu As Boolean = False)
	'------------------------------------------------------------------
	'V�rifie si une mise � jour du logiciel est disponible sur Internet
	'------------------------------------------------------------------
	Dim VpRequest As HttpWebRequest
	Dim VpAnswer As Stream
	Dim VpBuf(0 To 18) As Byte
	Dim VpOldText As String
	Dim VpContenuUpdate As frmUpdateContenu
	Dim VpNewContenu As New List(Of clsMAJContenu)
		VpOldText = MainForm.VgMe.StatusTextGet
		Call MainForm.VgMe.StatusText(CgDL1)
		'Fichier d'historique des versions
		Call DownloadNow(New Uri(VgOptions.VgSettings.DownloadServer + CgURL7), CgHSTFile)
		'V�rification horodatage
		Try
			VpRequest = WebRequest.Create(If(VpBeta, VgOptions.VgSettings.DownloadServer + CgURL1B, VgOptions.VgSettings.DownloadServer + CgURL1))
			VpAnswer = VpRequest.GetResponse.GetResponseStream
			'Lecture du fichier horodat� sur Internet
			VpAnswer.Read(VpBuf, 0, 19)
			VgRemoteDate = CDate(New ASCIIEncoding().GetString(VpBuf))
			'Si version plus r�cente
			If DateTime.Compare(File.GetLastWriteTimeUtc(Application.ExecutablePath), VgRemoteDate) < 0 Then
				VgTray.Visible = True
				VgTray.Tag = If(VpBeta, eUpdateType.Beta, eUpdateType.Release)
				VgTray.ShowBalloonTip(10, "Magic The Gathering Manager" + If(VpBeta, " BETA", ""), "Une mise � jour de l'application est disponible..." + vbCrLf + "Cliquer ici pour la t�l�charger, quitter Magic The Gathering Manager et l'installer.", ToolTipIcon.Info)
			ElseIf VpExplicit
				If VpContenu Then
					If clsModule.ShowQuestion("L'application est � jour..." + vbCrLf + "Voulez-vous rechercher aussi les mises � jour de contenu (prix, images...) ?") = System.Windows.Forms.DialogResult.Yes Then
						Call MainForm.VgMe.MnuContenuUpdateClick(Nothing, Nothing)
					End If
				Else
					If VpBeta Then
						Call ShowInformation("Aucune version b�ta post�rieure � la derni�re release n'est disponible pour l'instant...")
					Else
						Call ShowInformation("Vous disposez d�j� de la derni�re version de Magic The Gathering Manager !")
					End If
				End If
			'Recherche automatique des mises � jour de contenu
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
					VgTray.ShowBalloonTip(10, "Magic The Gathering Manager", "Des mises � jour de contenu sont disponibles..." + vbCrLf + "Cliquer ici pour en savoir plus...", ToolTipIcon.Info)
				End If
			End If
		Catch
			'En cas d'�chec de connexion, inutile de continuer � checker
			VgTimer.Stop
			If VpExplicit Then
				Call ShowWarning(CgDL3b)
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
				Call DownloadUpdate(New Uri(VgOptions.VgSettings.DownloadServer + CgURL2), CgUpDFile)
			Case eUpdateType.Beta
				Call DownloadUpdate(New Uri(VgOptions.VgSettings.DownloadServer + CgURL2B), CgUpDFile)
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
	Public Sub DownloadNow(VpURI As System.Uri, VpOutput As String)
	'----------------------------------------------------------------------------
	'T�l�charge imm�diatement l'application mise � jour ou une de ses d�pendances
	'----------------------------------------------------------------------------
		Try
			VgClient.DownloadFile(VpURI, Application.StartupPath + VpOutput)
		Catch
		End Try
	End Sub
	Public Sub DownloadUpdate(VpURI As System.Uri, VpOutput As String, Optional VpBaseDir As Boolean = True, Optional VpSilent As Boolean = False)
	'------------------------------------------------------------------------------
	'T�l�charge en arri�re-plan l'application mise � jour ou une de ses d�pendances
	'------------------------------------------------------------------------------
		If MainForm.VgMe.IsDownloadInProgress Then
			If Not MainForm.VgMe.MyChildren.DoesntExist(MainForm.VgMe.MyChildren.ContenuUpdater) Then
				If MainForm.VgMe.MyChildren.ContenuUpdater.PassiveUpdate = frmUpdateContenu.EgPassiveUpdate.InProgress Then
					MainForm.VgMe.MyChildren.ContenuUpdater.PassiveUpdate = frmUpdateContenu.EgPassiveUpdate.Failed
				End If
			End If
			If Not VpSilent Then
				Call ShowWarning(CgDL2b)
			End If
		Else
			MainForm.VgMe.IsDownloadInProgress = True
			Call MainForm.VgMe.StatusText(CgDL2, True)
			Try
				VgClient.DownloadFileAsync(VpURI, If(VpBaseDir, Application.StartupPath + VpOutput, VpOutput))
				MainForm.VgMe.btDownload.Visible = True
				MainForm.VgMe.btDownload.Tag = Now
			Catch
			End Try
		End If
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
	'Installe l'application mise � jour ou une de des d�pendances
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
				Call ShowWarning(CgDL3b)
				Call MainForm.VgMe.StatusText(CgDL3)
				Exit Sub
			ElseIf VpResult.Status = WebExceptionStatus.RequestCanceled Then
				Call MainForm.VgMe.StatusText(CgDL5)
				Call DeleteTempFiles(True)
				Exit Sub
			End If
		End If
		If MainForm.VgMe.StatusTextGet = CgDL2 Then
			Call MainForm.VgMe.StatusText(CgDL4)
		End If
		'Maj EXE
		If File.Exists(Application.StartupPath + CgUpDFile) Then
			File.SetLastWriteTimeUtc(Application.StartupPath + CgUpDFile, VgRemoteDate)
			Call SecureDelete(Application.StartupPath + CgDownDFile)
			Try
				File.Copy(Process.GetCurrentProcess.MainModule.FileName, Application.StartupPath + CgDownDFile)
				Process.Start(New ProcessStartInfo(Application.StartupPath + CgUpdater))
			Catch
				Call ShowWarning(CgErr5)
			End Try
		'Maj Images
		ElseIf File.Exists(Application.StartupPath + CgUpPic + CgPicUpExt) And File.Exists(Application.StartupPath + CgUpPic + CgPicLogExt) Then
			Call MainForm.VgMe.UpdatePictures(Application.StartupPath + CgUpPic + CgPicUpExt, Application.StartupPath + CgUpPic + CgPicLogExt, True)
		'Maj TXTFR
		ElseIf File.Exists(Application.StartupPath + CgUpTXTFR) Then
			Call MainForm.VgMe.UpdateTxtFR
		'Maj Rulings
		ElseIf File.Exists(Application.StartupPath + CgUpRulings) Then
			Call MainForm.VgMe.UpdateRulings(Not MainForm.VgMe.MyChildren.DoesntExist(MainForm.VgMe.MyChildren.ContenuUpdater) AndAlso MainForm.VgMe.MyChildren.ContenuUpdater.PassiveUpdate = frmUpdateContenu.EgPassiveUpdate.InProgress)
		End If
	End Sub
	Public Function GetEncNbr(VpCardName As String, VpIDSerie As String) As Long
	'-------------------------------------------------------------------------------------------
	'Retourne le num�ro unique caract�ristique d'une carte � partir de son nom et de son �dition
	'-------------------------------------------------------------------------------------------
		VgDBCommand.CommandText = "Select Card.EncNbr From Card Where Card.Title = '" + VpCardName.Replace("'", "''") + "' And Card.Series = '" + VpIDSerie + "';"
		Return CLng(VgDBCommand.ExecuteScalar)
	End Function
	Public Function GetSerieCodeFromName(VpName As String, Optional VpApprox As Boolean = False, Optional VpFR As Boolean = False) As String
	'-----------------------------------------------------------------------------------------
	'Retourne le code id d'une s�rie � partir de son nom VO ou VF, �ventuellement approximatif
	'-----------------------------------------------------------------------------------------
	Dim VpO As Object
		If VpName.Length = 2 Then
			Return VpName
		Else
			VgDBCommand.CommandText = "Select SeriesCD From Series Where " + If(VpFR, "SeriesNM_FR", "SeriesNM") + " = '" + VpName.Replace("'", "''") + "';"
			VpO = VgDBCommand.ExecuteScalar
			If Not VpO Is Nothing Then
				Return VpO.ToString
			Else
				If VpApprox Then
					VgDBCommand.CommandText = "Select SeriesCD From Series Where InStr(" + If(VpFR, "SeriesNM_FR", "SeriesNM") + ", '" + VpName.Replace("'", "''") + "') > 0;"
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
	Public Function GetSerieNameFromCode(VpSerie As String) As String
	'--------------------------------------------------------
	'Retourne le nom VO d'une �dition � partir de son code id
	'--------------------------------------------------------
	Dim VpO As Object
	Dim VpFoil As Boolean = VpSerie.EndsWith(CgFoil2)
		VgDBCommand.CommandText = "Select SeriesNM From Series Where SeriesCD = '" + VpSerie.Replace(CgFoil2, "") + "';"
		VpO = VgDBCommand.ExecuteScalar
		If Not VpO Is Nothing Then
			Return VpO.ToString + If(VpFoil, CgFoil2, "")
		Else
			Return ""
		End If
	End Function
	Public Function GetTransformedName(VpCard As String) As String
	'--------------------------------------------------------------------
	'Pour les cartes double faces, obtient le nom de la carte transform�e
	'--------------------------------------------------------------------
	Dim VpDBCommand As OleDbCommand
		Try
			VpDBCommand = New OleDbCommand
			VpDBCommand.Connection = VgDB
			VpDBCommand.CommandType = CommandType.Text
			VpDBCommand.CommandText = "Select Top 1 Card.Title From Card Where Card.EncNbr In (Select CardDouble.EncNbrDownFace From Card Inner Join CardDouble On Card.EncNbr = CardDouble.EncNbrTopFace Where Card.Title = '" + VpCard.Replace("'", "''") + "');"
			Return VpDBCommand.ExecuteScalar.ToString
		Catch
			Return ""
		End Try
	End Function
	Public Function HasPriceHistory As Boolean
		VgDBCommand.CommandText = "Select Count(*) From PricesHistory;"
		Return ( CInt(VgDBCommand.ExecuteScalar) > 0 )
	End Function
	Public Function GetPriceHistory(VpCardName As String, VpFoil As Boolean) As Hashtable
	'---------------------------------------------------------------------------------------------
	'Retourne l'historique des prix pour la carte pass�e en param�tre pour chacune de ses �ditions
	'---------------------------------------------------------------------------------------------
	Dim VpHist As New Hashtable
	Dim VpLastName As String = ""
	Dim VpCurName As String
	Dim VpCur As SortedList = Nothing
		VgDBCommand.CommandText = "Select Card.Series, GlobalHisto.PriceDate, GlobalHisto.Price From ((SELECT PricesHistory.EncNbr, PricesHistory.Price, DatesToUse.PriceDate, DatesToUse.Foil FROM PricesHistory INNER JOIN (SELECT PricesHistory.EncNbr, Max(PricesHistory.PriceDate) AS DLAST, AllDates.PriceDate, PricesHistory.Foil FROM PricesHistory, (SELECT Distinct PricesHistory.PriceDate FROM PricesHistory) As AllDates WHERE (((PricesHistory.PriceDate)<=[AllDates].[PriceDate])) GROUP BY PricesHistory.EncNbr, AllDates.PriceDate, PricesHistory.Foil) AS DatesToUse ON (PricesHistory.EncNbr = DatesToUse.EncNbr) AND (PricesHistory.PriceDate = DatesToUse.DLAST) AND (PricesHistory.Foil = DatesToUse.Foil)) As GlobalHisto) Inner Join Card On Card.EncNbr = GlobalHisto.EncNbr Where Card.Title = '" + VpCardName.Replace("'", "''") + "' And GlobalHisto.Foil = " + VpFoil.ToString + ";"
		VgDBReader = VgDBCommand.ExecuteReader
		With VgDBReader
			While .Read
				VpCurName = .GetString(0) + If(VpFoil, CgFoil2, "")
				If VpCurName <> VpLastName Then
					VpCur = New SortedList
					VpLastName = VpCurName
					VpHist.Add(VpLastName, VpCur)					
				End If
				VpCur.Add(.GetDateTime(1), .GetFloat(2))
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
	'Charge la liste des �ditions pr�sentes dans la base de donn�es
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
	Public Sub ExtractPictures(VpSaveFolder As String, VpSource As String, VpRestriction As String, Optional VpExtractTransformed As Boolean = False)
	'-------------------------------------------------------------------------------------------------
	'Sauvegarde dans le dossier sp�cifi� par l'utilisateur l'ensembles des images JPEG de la s�lection
	'-------------------------------------------------------------------------------------------------
	Dim VpSQL As String
	Dim VpCards As New List(Of String)
		VpSQL = "Select Distinct Card.Title From " + VpSource + " Inner Join Card On " + VpSource + ".EncNbr = Card.EncNbr Where "
		VpSQL = VpSQL + VpRestriction
		VpSQL = TrimQuery(VpSQL, False)
		If VpExtractTransformed Then
			VpSQL = VpSQL + " Union Select Distinct Card.Title From Card Where Card.EncNbr In (Select EncNbrDownFace From (" + VpSource + " Inner Join Card On " + VpSource + ".EncNbr = Card.EncNbr) Inner Join CardDouble On Card.EncNbr = CardDouble.EncNbrTopFace Where "
			VpSQL = VpSQL + VpRestriction
			VpSQL = TrimQuery(VpSQL, True, ")")
		End If
		VgDBCommand.CommandText = VpSQL
		VgDBReader = VgDBcommand.ExecuteReader
		With VgDBReader
			While .Read
				VpCards.Add(.GetString(0))
			End While
			.Close
		End With
		For Each VpCard As String In VpCards
			Call LoadScanCard(VpCard, Nothing, True, VpSaveFolder)
		Next VpCard
	End Sub
	Public Sub LoadScanCard(VpTitle As String, VppicScanCard As PictureBox, Optional VpSave As Boolean = False, Optional VpSaveFolder As String = "")
	'---------------------------------------------------------------------------------
	'Charge l'image scann�ris�e de la carte recherch�e dans la zone pr�vue � cet effet
	'---------------------------------------------------------------------------------
	Dim VpOffset As Long
	Dim VpEnd As Long
	Dim VpPicturesFile As StreamReader
	Dim VpPicturesFileB As BinaryReader
	Dim VpTmp As String = GetFreeTempFile
	Dim VpTmpFile As StreamWriter
	Dim VpTmpFileB As BinaryWriter
	Dim VpMissingTable As Boolean = False
	Dim VpDest As String
		If MainForm.VgMe.IsInImgDL Then
			Call ShowWarning(CgDL2c)
			Exit Sub
		ElseIf MainForm.VgMe.IsMainReaderBusy Then
			Exit Sub
		ElseIf File.Exists(VgOptions.VgSettings.PicturesFile) Then
			If (New FileInfo(VgOptions.VgSettings.PicturesFile)).Length < CgImgMinLength Then
				If Not VpSave Then
					VppicScanCard.Image = Nothing
				End If
				Call ShowWarning("La base d'images ne poss�de pas la taille minimale requise." + vbCrLf + "V�rifiez le fichier sp�cifi� dans les Pr�f�rences...")
				Exit Sub
			End If
			VpPicturesFile = New StreamReader(VgOptions.VgSettings.PicturesFile)
			VpPicturesFileB = New BinaryReader(VpPicturesFile.BaseStream)
			VpTmpFile = New StreamWriter(VpTmp)
			VpTmpFileB = New BinaryWriter(VpTmpFile.BaseStream)
			VgDBCommand.CommandText = "Select [Offset], [End] From CardPictures Where Title = '" + AvoidForbiddenChr(VpTitle, eForbiddenCharset.BDD) + "' Order By [End] Desc;"
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
							If VgOptions.VgSettings.ShowCorruption Then
								Call ShowWarning("La base d'images semble �tre corrompue." + vbCrLf + "Essayez de la mettre � jour ou de la re-t�l�charger...")
							End If
							VppicScanCard.Image = Nothing
							VgDBReader.Close
							Exit Sub
						End Try
					Else
						VpDest = VpSaveFolder + "\" + AvoidForbiddenChr(VpTitle) + ".jpg"
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
				If VpEx.ErrorCode = CgMissingTable Then
					VpMissingTable = True
				End If
				If Not VpSave Then
					VppicScanCard.Image = Nothing
				End If
			End Try
			VgDBReader.Close
			'Fichier pr�sent mais table d'index absente
			If VpMissingTable Then
				Select Case ShowQuestion("Cette version du logiciel est capable de g�rer les images des cartes mais la base de donn�es n'est pas � jour." + vbCrLf + "Voulez-vous t�l�charger les informations manquantes maintenant ?" + vbCrLf + "Cliquez sur 'Annuler' pour ne plus afficher ce message...", MessageBoxButtons.YesNoCancel)
					Case DialogResult.Yes
						Call DownloadUpdate(New Uri(VgOptions.VgSettings.DownloadServer + CgURL3), CgUpDDB)
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
	'Retourne le nombre de propri�taires en base de donn�es
	'------------------------------------------------------
		VgDBCommand.CommandText = "Select Count(*) From MyAdversairesID;"
		Return VgDBCommand.ExecuteScalar
	End Function
	Public Function GetAdvDecksCount(VpI As Integer) As Integer
	'--------------------------------------------------------------------------------------
	'Retourne le nombre de decks poss�d�s par le propri�taire d'index sp�cifi� en param�tre
	'--------------------------------------------------------------------------------------
		VgDBCommand.CommandText = "Select Count(*) From MyGamesID Where AdvID = " + VpI.ToString + ";"
		Return VgDBCommand.ExecuteScalar
	End Function
	Public Function GetAdvDecksCount(VpName As String) As Integer
		VgDBCommand.CommandText = "Select Count(*) From MyGamesID Inner Join MyAdversairesID On MyGamesID.AdvID = MyAdversairesID.AdvID Where AdvName = '" + VpName.Replace("'", "''") + "';"
		Return VgDBCommand.ExecuteScalar
	End Function
	Public Function GetAdvId(VpName As String) As Integer
	'-------------------------------------------------------------------------
	'Retourne l'identifiant de l'adversaire dont le nom est pass� en param�tre
	'-------------------------------------------------------------------------
		VgDBCommand.CommandText = "Select AdvId From MyAdversairesId Where AdvName = '" + VpName.Replace("'", "''") + "';"
		Return VgDBCommand.ExecuteScalar
	End Function
	Public Function GetOwner(VpDeck As String) As String
	'-------------------------------------------------------------------------
	'Retourne l'identifiant de l'adversaire dont le nom est pass� en param�tre
	'-------------------------------------------------------------------------
		VgDBCommand.CommandText = "Select AdvName From MyAdversairesID Inner Join MyGamesID On MyAdversairesID.AdvID = MyGamesID.AdvID Where GameName = '" + VpDeck.Replace("'", "''") + "';"
		Return VgDBCommand.ExecuteScalar
	End Function
	Public Function GetAdvName(VpI As Integer) As String
	'-------------------------------------------------------------
	'Retourne le nom de l'adversaire d'index sp�cifi� en param�tre
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
	'Retourne le nom du deck d'index sp�cifi� en param�tre
	'/!\ retourne le nom du VpI �me deck, et pas le deck dont l'id est VpI
	'---------------------------------------------------------------------
		VgDBCommand.CommandText = "Select Last(GameName) From (Select Top " + VpI.ToString + " GameName From MyGamesID Order By GameID);"
		Try
			Return VgDBCommand.ExecuteScalar
		Catch
			Return "Jeu n�" + VpI.ToString
		End Try
	End Function
	Public Function GetDeckIndex(VpStr As String) As String
	'-----------------------------------------------------
	'Retourne l'index du deck de nom sp�cifi� en param�tre
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
	Public Function GetDeckFormat(VpStr As String) As String
	'--------------------------------------------------------------
	'Retourne le format de jeu du deck de nom sp�cifi� en param�tre
	'--------------------------------------------------------------
		VgDBCommand.CommandText = "Select GameFormat From MyGamesID Where GameName = '" + VpStr.Replace("'", "''") + "';"
		Return VgDBCommand.ExecuteScalar.ToString
	End Function
	Public Function GetDeckDescription(VpDeckIndex As Integer) As String
	'--------------------------------------------------------------------
	'Retourne le commentaire associ� au deck de nom sp�cifi� en param�tre
	'--------------------------------------------------------------------
	Dim VpSQL As String
	Dim VpO As Object
		VpSQL = "Select GameDescription From MyGamesID Inner Join MyGames On MyGamesID.GameID = MyGames.GameID  Where MyGames.GameID = " + VpDeckIndex.ToString
		VgDBCommand.CommandText = clsModule.TrimQuery(VpSQL)
		VpO = VgDBCommand.ExecuteScalar
		If Not VpO Is Nothing Then
			Return VpO.ToString
		Else
			Return ""
		End If
	End Function
	Public Function GetDeckCount As Integer
	'----------------------------------------------
	'Retourne le nombre de decks en base de donn�es
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
			'Si pas de deck pr�sent
			Return 0
		End Try
	End Function
	Public Function GetFreeTempFile As String
	'--------------------------------------------------
	'Retourne un nom de fichier temporaire image valide
	'--------------------------------------------------
		With VgSessionSettings
			If .FreeTempFileIndex = -1 Then
				Do
					.FreeTempFileIndex += 1
				Loop While File.Exists(Path.GetTempPath + "\mtgm~" + .FreeTempFileIndex.ToString + ".jpg")
			Else
				.FreeTempFileIndex += 1
			End If
			Return Path.GetTempPath + "\mtgm~" + .FreeTempFileIndex.ToString + ".jpg"
		End With
	End Function
	Public Sub DeleteTempFiles(Optional VpSilent As Boolean = False)
	'------------------------------------
	'Suppression des fichiers temporaires
	'------------------------------------
		'Images
		Try
			For Each VpFile As FileInfo In (New DirectoryInfo(Path.GetTempPath)).GetFiles("mtgm~*.jpg")
				Call SecureDelete(VpFile.FullName)
			Next VpFile
		Catch
			If Not VpSilent Then
				Call ShowWarning("Impossible d'acc�der au r�pertoire temporaire...")
			End If
		End Try
		'Updates
		Call SecureDelete(Application.StartupPath + CgUpRulings)
		Call SecureDelete(Application.StartupPath + CgUpTXTFR)
		Call SecureDelete(Application.StartupPath + CgUpDFile)
		Call SecureDelete(Application.StartupPath + CgUpDDB)
		Call SecureDelete(Application.StartupPath + CgUpDDBb)
		Call SecureDelete(Application.StartupPath + CgUpPrices)
		Call SecureDelete(Application.StartupPath + CgUpSeries)
		For Each VpFile As FileInfo In (New DirectoryInfo(Application.StartupPath)).GetFiles("*_en.txt")
			Call SecureDelete(VpFile.FullName)
		Next VpFile
		For Each VpFile As FileInfo In (New DirectoryInfo(Application.StartupPath)).GetFiles("*_fr.txt")
			Call SecureDelete(VpFile.FullName)
		Next VpFile
	End Sub
	Public Sub SecureDelete(VpFile As String)
	'----------------------------------------------------------------
	'Suppression du fichier pass� en param�tre (avec trappe d'erreur)
	'----------------------------------------------------------------
		Try
			File.Delete(VpFile)
		Catch
		End Try
	End Sub
	Public Sub VerboseSimu(VpVerbose As Boolean, VpStr As String, VpSimuOut As StreamWriter, Optional VpEndSimu As Boolean = False)
	'-----------------------------------------------------
	'G�re la verbosit� pour les simulations de d�ploiement
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
	Public ItemsFound As New List(Of TreeNode)
	Public CurItem As Integer
End Class
Public Class clsManas
	Private VmX As Short = 0						'Mana variable
	Private VmM As Short = 0						'Mana de n'importe quelle couleur
	Private VmA As Short = 0						'Mana sans couleur
	Private VmB As Short = 0						'Mana noir
	Private VmG As Short = 0						'Mana vert
	Private VmU As Short = 0						'Mana bleu
	Private VmR As Short = 0						'Mana rouge
	Private VmW As Short = 0						'Mana blanc
	Private VmPB As Short = 0						'Mana noir ou 2 points de vie
	Private VmPG As Short = 0						'Mana vert ou 2 points de vie
	Private VmPU As Short = 0						'Mana bleu ou 2 points de vie
	Private VmPR As Short = 0						'Mana rouge ou 2 points de vie
	Private VmPW As Short = 0						'Mana blanc ou 2 points de vie
	Private Vm2W As Short = 0						'Mana blanc ou 2 de n'importe quelle couleur
	Private Vm2R As Short = 0						'Mana rouge ou 2 de n'importe quelle couleur
	Private Vm2G As Short = 0						'Mana vert ou 2 de n'importe quelle couleur
	Private Vm2B As Short = 0						'Mana noir ou 2 de n'importe quelle couleur
	Private Vm2U As Short = 0						'Mana bleu ou 2 de n'importe quelle couleur
	Private VmBG As Short = 0						'Mana noir ou vert
	Private VmBR As Short = 0						'Mana noir ou rouge
	Private VmGU As Short = 0						'Mana vert ou bleu
	Private VmGW As Short = 0						'Mana vert ou blanc
	Private VmRG As Short = 0						'Mana rouge ou vert
	Private VmRW As Short = 0						'Mana rouge ou blanc
	Private VmUB As Short = 0						'Mana bleu ou noir
	Private VmUR As Short = 0						'Mana bleu ou rouge
	Private VmWB As Short = 0						'Mana blanc ou noir
	Private VmWU As Short = 0						'Mana blanc ou bleu
	Private VmEffectiveLength As Short = 0			'Longueur effective (~co�t converti)
	Private VmImgIndexes As New List(Of Integer)	'Rep�res ic�nes (1=BG,2=BR,3=G,4=GU,5=GW,6=R,7=RG,8=RW,9=U,10=UB,11=UR,12=W,13=WB,14=WU,15=X,16=)
	Public Sub New(Optional VpCostDB As String = "")
	'----------------------------------------------
	'Effectue un parsing du co�t pass� en param�tre
	'----------------------------------------------
	Dim VpCost As String
	Dim VpX As String						'Unit� de co�t courant
	Dim VpY As String						'Unit� de dual-co�t courant
	Dim VpI As Integer						'Compteur
	Dim VpColorless As Integer				'Co�t en manas sans couleur
	Dim VpStrs(0 To 1) As String
		If VpCostDB.Trim = "" Then Exit Sub
		'Gestion des cas sp�ciaux :
		VpCost = VpCostDB.ToLower
		VpCost = VpCost.Replace("{", "").Replace("}", "")
		'- either or (ex. either b or u)
		If VpCost.Contains(CgManaParsing(2)) Then
			VpCost = VpCost.Replace(CgManaParsing(2), "(").Replace(CgManaParsing(3), "/") + ")"
		'- colorless mana (ex. one colorless mana)
		ElseIf VpCost.Contains(CgManaParsing(4)) Then
			VpCost = VpCost.Replace(CgManaParsing(4), "")
			VpCost = StrBuild("A", FindNumber(VpCost))
		'- colorless mana d'un nombre ind�termin� (on en met 1 par d�faut)
		ElseIf VpCost = CgManaParsing(4).Trim Then
			VpCost = "A"
		'- mana of any color (ex. one mana of any color)
		ElseIf VpCost.Contains(CgManaParsing(5)) Then
			VpCost = VpCost.Replace(CgManaParsing(5), "")
			VpCost = StrBuild("M", FindNumber(VpCost))
		'- mana (ex. two blue mana)
		ElseIf VpCost.Contains(CgManaParsing(6)) Then
			VpCost = VpCost.Replace(CgManaParsing(6), "")
			VpStrs = VpCost.Split(" ")
			Try
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
			'Si la couleur n'a pas �t� pr�cis�e, assume par d�faut de l'incolore
			Catch
				ReDim Preserve VpStrs(0 To 1)
				VpStrs(1) = "A"
			End Try
			VpCost = StrBuild(VpStrs(1), FindNumber(VpStrs(0)))
		'- XA... (ex. X2U)
		ElseIf VpCost.StartsWith("x") AndAlso Val(VpCost.Substring(1)) <> 0 Then
			VmX = 1
			VmImgIndexes.Add(0)
			VmEffectiveLength = 1
			VpCost = VpCost.Substring(1)
		End If
		'Gestion des cas classiques :
		VmEffectiveLength = VmEffectiveLength + VpCost.Length - 4 * StrCount(VpCost, "(")
		VpColorless = Val(VpCost)
		If VpColorless >= 10 Then
			VmEffectiveLength = VmEffectiveLength - 1
		End If
		For VpI = 0 To VpCost.Length - 1
			VpX = VpCost.Substring(VpI, 1)
			'Pas de couleur
			If VpI = 0 And VpColorless <> 0 Then
				VmImgIndexes.Add(1 + CInt(VpColorless))
				If VpColorless >= 10 Then
					VpI = VpI + 1 	'Tr�s impropre en programmation structur�e mais permet de g�rer directement le cas des nombres � 2 chiffres
				End If
				VmA = VpColorless
			'Dual couleur
			ElseIf VpX = "(" Then
				VpY = VpCost.Substring(VpI + 1, 3)
				Select Case VpY.ToUpper
					Case "P/R", "R/P"
						VmImgIndexes.Add(35)
						VmPR = VmPR + 1
					Case "P/B", "B/P"
						VmImgIndexes.Add(33)
						VmPB = VmPB + 1
					Case "P/G", "G/P"
						VmImgIndexes.Add(34)
						VmPG = VmPG + 1
					Case "P/U", "U/P"
						VmImgIndexes.Add(36)
						VmPU = VmPU + 1
					Case "P/W", "W/P"
						VmImgIndexes.Add(37)
						VmPW = VmPW + 1
					Case "G/B"
						VmImgIndexes.Add(29)
						VmBG = VmBG + 1
					Case "R/B"
						VmImgIndexes.Add(39)
						VmBR = VmBR + 1
					Case "U/G"
						VmImgIndexes.Add(45)
						VmGU = VmGU + 1
					Case "W/G"
						VmImgIndexes.Add(50)
						VmGW = VmGW + 1
					Case "G/R"
						VmImgIndexes.Add(30)
						VmRG = VmRG + 1
					Case "W/R"
						VmImgIndexes.Add(51)
						VmRW = VmRW + 1
					Case "B/U"
						VmImgIndexes.Add(26)
						VmUB = VmUB + 1
					Case "R/U"
						VmImgIndexes.Add(41)
						VmUR = VmUR + 1
					Case "B/W"
						VmImgIndexes.Add(27)
						VmWB = VmWB + 1
					Case "U/W"
						VmImgIndexes.Add(47)
						VmWU = VmWU + 1
					Case "B/G"
						VmImgIndexes.Add(24)
						VmBG = VmBG + 1
					Case "B/R"
						VmImgIndexes.Add(25)
						VmBR = VmBR + 1
					Case "G/U"
						VmImgIndexes.Add(31)
						VmGU = VmGU + 1
					Case "G/W"
						VmImgIndexes.Add(32)
						VmGW = VmGW + 1
					Case "R/G"
						VmImgIndexes.Add(40)
						VmRG = VmRG + 1
					Case "R/W"
						VmImgIndexes.Add(42)
						VmRW = VmRW + 1
					Case "U/B"
						VmImgIndexes.Add(44)
						VmUB = VmUB + 1
					Case "U/R"
						VmImgIndexes.Add(46)
						VmUR = VmUR + 1
					Case "W/B"
						VmImgIndexes.Add(49)
						VmWB = VmWB + 1
					Case "W/U"
						VmImgIndexes.Add(52)
						VmWU = VmWU + 1
					Case "2/W", "W/2"
						VmImgIndexes.Add(22)
						Vm2W = Vm2W + 1
					Case "2/B", "B/2"
						VmImgIndexes.Add(18)
						Vm2B = Vm2B + 1
					Case "2/G", "G/2"
						VmImgIndexes.Add(19)
						Vm2G = Vm2G + 1
					Case "2/R", "R/2"
						VmImgIndexes.Add(20)
						Vm2R = Vm2R + 1
					Case "2/U", "U/2"
						VmImgIndexes.Add(21)
						Vm2U = Vm2U + 1
					Case Else
						VmImgIndexes.Add(0)
				End Select
				VpI = VpI + 4 'encore impropre mais permet de passer directement � la fin de la parenth�se
			'Couleur simple
			Else
				Select Case VpX.ToUpper
					Case "X"
						VmImgIndexes.Add(0)
						VmX = VmX + 1
					Case "B"
						VmImgIndexes.Add(23)
						VmB = VmB + 1
					Case "G"
						VmImgIndexes.Add(28)
						VmG = VmG + 1
					Case "R"
						VmImgIndexes.Add(38)
						VmR = VmR + 1
					Case "U"
						VmImgIndexes.Add(43)
						VmU = VmU + 1
					Case "W"
						VmImgIndexes.Add(48)
						VmW = VmW + 1
					Case "M"
						VmImgIndexes.Add(1)
						VmM = VmM + 1
					Case "A"
						VmImgIndexes.Add(1)
						VmA = VmA + 1
					Case Else
						VmImgIndexes.Add(1)
				End Select
			End If
		Next VpI
	End Sub
	Public Function IsBetterWith(VpMana As clsManas) As Boolean
	'-----------------------------------------------------------------------------------------------------------
	'Renvoie vrai si l'invocation de la carte courante requiert le mana produit par la carte pass�e en param�tre
	'-----------------------------------------------------------------------------------------------------------
		Return ( ( Me.HasBlack And VpMana.HasBlack ) Or ( Me.HasBlue And VpMana.HasBlue ) Or ( Me.HasGreen And VpMana.HasGreen ) Or ( Me.HasRed And VpMana.HasRed ) Or ( Me.HasWhite And VpMana.HasWhite ) )
	End Function
	Public Sub AddSubManas(VpManas As clsManas, Optional VpSub As Short = 1)
	'----------------------------------------------------------------------------------
	'Ajoute (ou soustrait) la description de manas pass�e en param�tre � celle courante
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
		VmPB = VmPB + VpManas.cPB * VpSub
		VmPG = VmPG + VpManas.cPG * VpSub
		VmPU = VmPU + VpManas.cPU * VpSub
		VmPR = VmPR + VpManas.cPR * VpSub
		VmPW = VmPW + VpManas.cPW * VpSub
		Vm2B = Vm2B + VpManas.c2B * VpSub
		Vm2G = Vm2G + VpManas.c2G * VpSub
		Vm2U = Vm2U + VpManas.c2U * VpSub
		Vm2R = Vm2R + VpManas.c2R * VpSub
		Vm2W = Vm2W + VpManas.c2W * VpSub
	End Sub
	Public Function ContainsEnoughFor(VpManas As clsManas) As Boolean
	'-----------------------------------------------------------------------------------------------------------------------
	'Renvoie vrai si la description de mana courante contient (en terme de qualit� et de quantit�) celle pass�e en param�tre
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
							VmWU >= VpManas.cWU And _
							Vm2W >= VpManas.c2W And _
							Vm2B >= VpManas.c2B And _
							Vm2R >= VpManas.c2R And _
							Vm2G >= VpManas.c2G And _
							Vm2U >= VpManas.c2U	And _
							VmPW >= VpManas.cPW And _
							VmPB >= VpManas.cPB And _
							VmPR >= VpManas.cPR And _
							VmPG >= VpManas.cPG And _
							VmPU >= VpManas.cPU		)
		'Si les manas de couleurs ne peuvent �tre pay�s, c'est mort
		If Not VpEnoughColor Then
			Return False
		Else
			'S'ils le peuvent et qu'en plus les multi/in-colores peuvent �tre pay�s, c'est bon
			If (VmA + VmM) >= (VpManas.cA + VpManas.cM) Then
				Return True
			'Sinon, il faut regarder si les multi/in-colores peuvent �tres pay�s avec ceux restant de couleurs
			Else
				'Copie temporaire de la description de manas courante
				VpMe = New clsManas
				Call VpMe.AddSubManas(Me)
				'Soustraction des co�ts color�s uniquement
				Call VpMe.AddSubManas(VpManas, -1)
				VpMe.cA = Me.cA
				VpMe.cM = Me.cM
				'Reste-t-il assez pour payer les multi/in-colores de la description de manas pass�e en param�tre
				Return ( VpMe.Potentiel >= (VpManas.cA + VpManas.cM) )
			End If
		End If
	End Function
	Public ReadOnly Property ImgIndexes As List(Of Integer)
		Get
			Return VmImgIndexes
		End Get
	End Property
	Public ReadOnly Property EffectiveLength As Integer
		Get
			Return VmEffectiveLength
		End Get
	End Property
	Public ReadOnly Property Potentiel As Integer
		Get
			Return ( VmX + VmM + VmA + VmB + VmG + VmU + VmR + VmW + VmBG + VmBR + VmGU + VmGW + VmRG + VmRW + VmUB + VmUR + VmWB + VmWU + VmPR + VmPG + VmPU + VmPW + VmPB + Vm2R + Vm2G + Vm2U + Vm2W + Vm2B)
		End Get
	End Property
	Public ReadOnly Property HasBlack As Boolean
		Get
			Return ( VmB > 0 Or VmBG > 0 Or VmBR > 0 Or VmUB > 0 Or VmWB > 0 Or VmPB > 0 Or Vm2B > 0 )
		End Get
	End Property
	Public ReadOnly Property HasGreen As Boolean
		Get
			Return ( VmG > 0 Or VmBG > 0 Or VmGU > 0 Or VmGW > 0 Or VmRG > 0 Or VmPG > 0 Or Vm2G > 0 )
		End Get
	End Property
	Public ReadOnly Property HasBlue As Boolean
		Get
			Return ( VmU > 0 Or VmGU > 0 Or VmUB > 0 Or VmUR > 0 Or VmWU > 0 Or VmPU > 0 Or Vm2U > 0 )
		End Get
	End Property
	Public ReadOnly Property HasRed As Boolean
		Get
			Return ( VmR > 0 Or VmBR > 0 Or VmRG > 0 Or VmRW > 0 Or VmUR > 0 Or VmPR > 0 Or Vm2R > 0 )
		End Get
	End Property
	Public ReadOnly Property HasWhite As Boolean
		Get
			Return ( VmW > 0 Or VmGW > 0 Or VmRW > 0 Or VmWB > 0 Or VmWU > 0 Or VmPW > 0 Or Vm2W > 0 )
		End Get
	End Property
	Public Property cM As Short
		Get
			Return VmM
		End Get
		Set (VpM As Short)
			VmM = VpM
		End Set
	End Property
	Public Property cA As Short
		Get
			Return VmA
		End Get
		Set (VpA As Short)
			VmA = VpA
		End Set
	End Property
	Public ReadOnly Property cB As Short
		Get
			Return VmB
		End Get
	End Property
	Public ReadOnly Property cG As Short
		Get
			Return VmG
		End Get
	End Property
	Public ReadOnly Property cU As Short
		Get
			Return VmU
		End Get
	End Property
	Public ReadOnly Property cR As Short
		Get
			Return VmR
		End Get
	End Property
	Public ReadOnly Property cW As Short
		Get
			Return VmW
		End Get
	End Property
	Public ReadOnly Property cBG As Short
		Get
			Return VmBG
		End Get
	End Property
	Public ReadOnly Property cBR As Short
		Get
			Return VmBR
		End Get
	End Property
	Public ReadOnly Property cGU As Short
		Get
			Return VmGU
		End Get
	End Property
	Public ReadOnly Property cGW As Short
		Get
			Return VmGW
		End Get
	End Property
	Public ReadOnly Property cRG As Short
		Get
			Return VmRG
		End Get
	End Property
	Public ReadOnly Property cRW As Short
		Get
			Return VmRW
		End Get
	End Property
	Public ReadOnly Property cUB As Short
		Get
			Return VmUB
		End Get
	End Property
	Public ReadOnly Property cUR As Short
		Get
			Return VmUR
		End Get
	End Property
	Public ReadOnly Property cWB As Short
		Get
			Return VmWB
		End Get
	End Property
	Public ReadOnly Property cWU As Short
		Get
			Return VmWU
		End Get
	End Property
	Public ReadOnly Property cPR As Short
		Get
			Return VmPR
		End Get
	End Property
	Public ReadOnly Property cPW As Short
		Get
			Return VmPW
		End Get
	End Property
	Public ReadOnly Property cPU As Short
		Get
			Return VmPU
		End Get
	End Property
	Public ReadOnly Property cPG As Short
		Get
			Return VmPG
		End Get
	End Property
	Public ReadOnly Property cPB As Short
		Get
			Return VmPB
		End Get
	End Property
	Public ReadOnly Property c2R As Short
		Get
			Return VmPR
		End Get
	End Property
	Public ReadOnly Property c2W As Short
		Get
			Return VmPW
		End Get
	End Property
	Public ReadOnly Property c2U As Short
		Get
			Return VmPU
		End Get
	End Property
	Public ReadOnly Property c2G As Short
		Get
			Return VmPG
		End Get
	End Property
	Public ReadOnly Property c2B As Short
		Get
			Return VmPB
		End Get
	End Property
End Class
Public Class clsManasPotComparer
	Implements IComparer(Of clsCard)
	Private VmUserList As CheckedListBox
	Public Sub New(VpUserList As CheckedListBox)
		VmUserList = VpUserList
	End Sub
	Public Function Compare(ByVal x As clsCard, ByVal y As clsCard) As Integer Implements IComparer(Of clsCard).Compare
	'---------------------------------------------------------------------------------------------------------------------
	'Permet le tri des cartes dans l'ordre de pr�f�rence d'invocation, selon le mana qu'elles sont susceptibles de g�n�rer
	'Favorise �galement l'invocation des cartes sp�ciales selon l'ordre sp�cifi� par l'utilisateur
	'---------------------------------------------------------------------------------------------------------------------
	Dim VpPot1 As Integer = Me.GetMiniPot(x)
	Dim VpPot2 As Integer = Me.GetMiniPot(y)
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