Imports System.Data.OleDb
Public Module mdlConstGlob
    Public Const CgCodeLines As Integer             = 40026
    Public Const CGNClasses As Integer              = 93
    Public Const CgLastUpdateAut As String          = "26/10/2019"
    Public Const CgLastUpdateSimu As String         = "26/10/2019"
    Public Const CgLastUpdateTxtVF As String        = "17/10/2019"
    Public Const CgLastUpdateRulings As String      = "14/10/2019"
    Public Const CgLastUpdatePictPatch As String    = "27/12/2014"
    Public Const CgLastUpdateTradPatch As String    = "15/12/2019"
    Public Const CgLastUpdateSubsPatch As String    = "04/04/2015"
    Public Const CgLastUpdateSubsVFPatch As String  = "10/05/2018"
    Public Const CgLastUpdateMultiIdPatch As String = "26/10/2019"
    Public Const CgProject As String                = "Magic_The_Gathering_Manager.MainForm"
    Public Const CgMe As String                     = "Moi"
    Public Const CgNCriterions As Integer           = 8
    Public Const CgNDispMenuBase As Integer         = 4
    Public Const CgNMain As Integer                 = 7
    Public Const CgNLives As Integer                = 20
    Public Const CgMaxPot As Integer                = 100
    Public Const CgPertinCoeff As Integer           = 4
    Public Const CgGraphsExtraMargin As Single      = 0.2
    Public Const CgMaxGraphs As Integer             = 128
    Public Const CgMaxVignettes As Integer          = 120
    Public Const CgMaxEditionsMarket As Integer     = 4
    Public Const CgWorstShippingCost As Single      = 100000
    Public Const CgMissingTable As Long             = -2147217865
    Public Const CgImgMinLength As Long             = 296297676
    Public Const CgTimeOut As Integer               = 5
    Public Const CgMTGCardWidth_mm As Integer       = 63
    Public Const CgMTGCardHeight_mm As Integer      = 88
    Public Const CgMTGCardWidth_crop_mm As Integer  = 57
    Public Const CgMTGCardHeight_crop_mm As Integer = 82
    Public Const CgMTGCardWidth_px As Integer       = 210
    Public Const CgMTGCardHeight_px As Integer      = 300
    Public Const CgCounterDiametr_px As Integer     = 20
    Public Const CgChevauchFactor As Single         = 0.14
    Public Const CgSpacingFactor As Single          = 1.5
    Public Const CgXMargin As Integer               = 5
    Public Const CgYMargin As Integer               = 8
    Public Const CgTemp As String                   = "\mtgmgr"
    Public Const CgIcons As String                  = "\Ressources"
    Public Const CgMagicBack As String              = "\Ressources\Magic Back.jpg"
    Public Const CgMDB As String                    = "\Cartes\Magic DB.mdb"
    Public Const CgDAT As String                    = "\Cartes\Images DB.dat"
    Public Const CgINIFile As String                = "\MTGM.ini"
    Public Const CgXMLFile As String                = "\MTGM.xml"
    Public Const CgHLPFile As String                = "\MTGM.pdf"
    Public Const CgHSTFile As String                = "\Historique.txt"
    Public Const CgUpdater As String                = "\Updater.exe"
    Public Const CgMTGMWebResourcer As String       = "\WebResourcer.exe"
    Public Const CgHTMLCollectionViewer As String   = "\CollectionViewer.exe"
    Public Const CgColViewerZipRes As String        = "\CollectionViewer.zip"
    Public Const CgUpDFile As String                = "\Magic The Gathering Manager.new"
    Public Const CgDownDFile As String              = "\Magic The Gathering Manager.bak"
    Public Const CgUpMultiverse As String           = "\AllPrintings.json"
    Public Const CgUpMultiverse2 As String          = "\AllPrintings.json.zip"
    Public Const CgUpDDB As String                  = "\Images DB.mdb"
    Public Const CgUpDDBb As String                 = "\Patch.mdb"
    Public Const CgUpDDBd As String                 = "Images%20DB.dat"
    Public Const CgUpTXTFR As String                = "\TextesVF.txt"
    Public Const CgUpSeries As String               = "\Series.txt"
    Public Const CgUpAutorisations As String        = "\Tournois.txt"
    Public Const CgUpRulings As String              = "\Rulings.xml"
    Public Const CgUpPrices As String               = "\LastPrices.txt"
    Public Const CgUpPic As String                  = "\SP_Pict"
    Public Const CgMdPic As String                  = "MD_Pict"
    Public Const CgMdTrad As String                 = "\MD_Trad.log"
    Public Const CgMdSubTypes As String             = "\MD_SubTypes.log"
    Public Const CgMdSubTypesVF As String           = "\MD_SubTypesVF.log"
    Public Const CgMdMultiverse As String           = "\MD_Multiverse.log"
    Public Const CgMdShippingCosts As String        = "\MD_ShippingCosts.log"
    Public Const CgMdPicturesPointers As String     = "\MD_PicturesPointers.log"
    Public Const CgShell As String                  = "explorer.exe"
    Public Const CgDefaultServer As String          = "http://couitchy.free.fr/upload/MTGM"
    Public Const CgURL0 As String                   = "https://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=#&type=card"
    Public Const CgURL1 As String                   = "/Updates/TimeStamp r4.txt"
    Public Const CgURL1B As String                  = "/Updates/Beta/TimeStamp.txt"
    Public Const CgURL1C As String                  = "/Updates/PicturesStamp.txt"
    Public Const CgURL1D As String                  = "/Updates/ContenuStamp r20.txt"
    Public Const CgURL1E As String                  = "/Updates/ContenuSizes r20.txt"
    Public Const CgURL2 As String                   = "/Updates/Magic The Gathering Manager r4.new"
    Public Const CgURL2B As String                  = "/Updates/Beta/Magic The Gathering Manager.new"
    Public Const CgURL3 As String                   = "/Updates/Images DB.mdb"
    Public Const CgURL3B As String                  = "/Updates/Patch r13.mdb"
    Public Const CgURL4 As String                   = "/Listes%20des%20editions/"
    Public Const CgURL5 As String                   = "/Logos%20des%20editions/"
    Public Const CgURL6 As String                   = "https://mtgjson.com/downloads/all-sets/"
    Public Const CgURL7 As String                   = "/Updates/Historique.txt"
    Public Const CgURL8 As String                   = "/Lib/"
    Public Const CgURL9 As String                   = "/Updates/LastPrices.txt"
    Public Const CgURL10 As String                  = "/Images%20des%20cartes/"
    Public Const CgURL11 As String                  = "/Updates/TextesVF.txt"
    Public Const CgURL12 As String                  = "/Updates/Series r23.txt"
    Public Const CgURL13 As String                  = "/Updates/MTGM.pdf"
    Public Const CgURL14 As String                  = "/Updates/MD_Trad.log"
    Public Const CgURL15 As String                  = "/Updates/Tournois r24.txt"
    Public Const CgURL16 As String                  = "https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=couitchy@free.fr&lc=FR&item_name=Magic The Gathering Manager&currency_code=EUR&bn=PP%2dDonationsBF"
    Public Const CgURL17 As String                  = "http://mtgm.free.fr"
    Public Const CgURL18 As String                  = "mailto:couitchy@free.fr?subject=Magic The Gathering Manager&body=Votre message ici"
    Public Const CgURL19 As String                  = "/Updates/Rulings.xml"
    Public Const CgURL20 As String                  = "/Updates/MD_SubTypes r19.log"
    Public Const CgURL21 As String                  = "/Updates/MD_SubTypesVF r19.log"
    Public Const CgURL22 As String                  = "/Updates/MD_Multiverse r22.log"
    Public Const CgURL23 As String                  = "https://mtgjson.com/api/v5/AllPrintings.json.zip"
    Public Const CgURL24 As String                  = "https://api.cardmarket.com/ws/v1.1/output.json/products/card-name/1/1/true"
    Public Const CgURL25 As String                  = "https://api.cardmarket.com/ws/v1.1/output.json/articles/"
    Public Const CgURL26 As String                  = "https://www.magic-ville.com/fr/"
    Public Const CgURL27 As String                  = "/Updates/MD_ShippingCosts r21.log"
    Public Const CgURL28 As String                  = "/Updates/MD_PicturesPointers.log"
    Public Const CgDL1 As String                    = "Vérification des mises à jour..."
    Public Const CgDL2 As String                    = "Téléchargement en cours"
    Public Const CgDL2b As String                   = "Un téléchargement est déjà en cours..." + vbCrLf + "Veuillez attendre qu'il se termine avant de réessayer."
    Public Const CgDL2c As String                   = "Une mise à jour des images est en cours." + vbCrLf + "Veuillez patienter avant d'essayer de les utiliser..."
    Public Const CgDL3 As String                    = "Erreur lors du téléchargement"
    Public Const CgDL3b As String                   = "La connexion au serveur a échoué..." + vbCrLf + "Vérifier la connectivité à Internet et les paramètres du pare-feu."
    Public Const CgDL4 As String                    = "Téléchargement terminé"
    Public Const CgDL5 As String                    = "Téléchargement annulé"
    Public Const CgErr0 As String                   = "Des fichiers nécessaires sont manquants..."
    Public Const CgErr1 As String                   = "Les modèles de simulation sont absents ou incomplets..." + vbCrLf + "Procédez à la mise à jour depuis le menu 'Fichier' de la fenêtre principale..."
    Public Const CgErr2 As String                   = "L'historique des prix est vide..."
    Public Const CgErr3 As String                   = "Impossible d'afficher les informations demandées maintenant..." + vbCrLf + "Si une mise à jour est en cours, merci d'attendre qu'elle se finisse."
    Public Const CgErr4 As String                   = "Le nombre maximal de courbes affichables a été atteint..." + vbCrLf + "Les suivantes seront ignorées."
    Public Const CgErr5 As String                   = "Le processus de mise à jour a été interrompu..."
    Public Const CgErr6 As String                   = "Le plug-in spécifié est introuvable..."
    Public Const CgErr7 As String                   = "Aucun critère de classement n'a été sélectionné..."
    Public Const CgErr8 As String                   = "A la suite d'une mise à jour, vos préférences ont été réinitialisées." + vbCrLf + "Merci de vérifier dans Gestion / Préférences les différents chemins des fichiers. Il est possible que certaines mises à jour de contenu devront être re-téléchargées..."
    Public Const CgErr9 As String                   = "Vous ne pouvez pas déplacer des cartes dans cette zone quand la Réserve est affichée..."
    Public Const CgErr10 As String                  = "La zone 'Regard' doit être vide pour pouvoir afficher la Réserve..."
    Public Const CgErr11 As String                  = "Impossible de modifier les préférences..."
    Public Const CgFExtH As String                  = ".html"
    Public Const CgFExtO As String                  = ".dck"
    Public Const CgFExtN As String                  = ".dk2"
    Public Const CgFExtA As String                  = ".dec"
    Public Const CgFExtW As String                  = ".mwDeck"
    Public Const CgFExtX As String                  = ".xmg"
    Public Const CgFExtC As String                  = ".txt"
    Public Const CgFExtM As String                  = ".xml"
    Public Const CgFExtL As String                  = ".csv"
    Public Const CgFExtU As String                  = ".ugs"
    Public Const CgFExtD As String                  = ".mdb"
    Public Const CgIconsExt As String               = ".png"
    Public Const CgPicUpExt As String               = ".dat"
    Public Const CgPicLogExt As String              = ".log"
    Public Const CgImgSeries As String              = "_series_"
    Public Const CgImgColors As String              = "_colors_"
    Public Const CgDefaultDeckName As String        = "(Deck)"
    Public Const CgDefaultFolderName As String      = "(Dossier)"
    Public Const CgDefaultFormat As String          = "Classique"
    Public Const CgRulings As String                = "Règles spécifiques"
    Public Const CgPlateau As String                = "Plateau de jeu : "
    Public Const CgStats As String                  = "Statistiques : "
    Public Const CgSimus As String                  = "Simulations : "
    Public Const CgSimus3 As String                 = "Proba. séquence(s) pour "
    Public Const CgSimus4 As String                 = "Manas productibles pour "
    Public Const CgSimus5 As String                 = "Défaut de manas pour "
    Public Const CgRefresh As String                = "Rafraîchir"
    Public Const CgPanel As String                  = "Ouvrir / fermer panneau image"
    Public Const CgStock As String                  = "Nombre déjà en stock"
    Public Const CgSide As String                   = "~ Jeu de réserve ~"
    Public Const CgCollection As String             = "Collection"
    Public Const CgSCollection As String            = "MyCollection"
    Public Const CgSDecks As String                 = "MyGames"
    Public Const CgAdvSearch As String              = "Résultats de recherche"
    Public Const CgFromSearch As String             = "Recherche"
    Public Const CgSFromSearch As String            = "MySearch"
    Public Const CgCard As String                   = "(carte)"
    Public Const CgPerfsEfficiency As String        = "Calcul du facteur d'efficacité" + vbCrLf + "----------------------------------" + vbCrLf + "NB. Ce calcul n'a de sens que si tous les jeux en présence ont été saisis dans la base (afin d'en connaître leur prix)." + vbCrLf + "~1, le jeu est à la hauteur de son prix (jeu normal)" + vbCrLf + "<1, le jeu gagne plus de parties qu'il n'en devrait compte tenu de son prix (jeu efficient)" + vbCrLf + ">1, le jeu gagne moins de parties qu'il n'en devrait compte tenu de son prix (jeu soit mauvais / soit ""bulldozer"")" + vbCrLf + "(un résultat négatif signifie qu'il manque des informations pour le calcul : prix du jeu, résultats de parties...)" + vbCrLf + vbCrLf
    Public Const CgPerfsVersion As String           = "nouv."
    Public Const CgPerfsTotal As String             = "TOTAL "
    Public Const CgPerfsTotalV As String            = "toutes"
    Public Const CgPerfsVFree As String             = "sans version"
    Public Const CgAlternateStart As String         = "Card Name:"
    Public Const CgAlternateStart2 As String        = "Name:"
    Public Const CgFoil As String                   = "PREMIUMFOILVO"
    Public Const CgFoil2 As String                  = " (Foil)"
    Public CgBuyLanguage() As String                = {"french", "english"}
    Public CgBalises() As String                    = {"CardName:", "Cost:", "Type:", "Pow/Tgh:", "Rules Text:", "Set/Rarity:"}
    Public CgManaParsing() As String                = {"to your mana pool", "add", "either ", " or ", " colorless mana", " mana of any color", " mana"}
    Public CgCriterionsFields() As String           = {"Card.Type", "Spell.Color", "Card.Series", "Spell.myCost", "Card.Rarity", "Card.myPrice", "Items", "Card.Title"}
    Public CgNumbers() As String                    = {"one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten"}
    Public CgRarities() As String                   = {"'M'", "'R'", "'U'", "'C'", "'D'", "'L'", "'S'"}
    Public CgSearchFields() As String               = {"Card.Title", "CardFR.TitleFR", "Card.CardText", "TextesFR.TexteFR", "Creature.Power", "Creature.Tough", "Card.Price", "Card.Series", "Card.Series", "Spell.myCost", "Card.SubType", "SubTypes.SubTypeVF"}
    Public CgRequiredFiles() As String              = {"\TreeViewMS.dll", "\ChartFX.Lite.dll", "\NPlot.dll", "\SandBar.dll", "\SourceGrid2.dll", "\SourceLibrary.dll", "\ICSharpCode.SharpZipLib.dll", CgMagicBack, CgUpdater}
    Public CgStrConn() As String                    = {"Provider=Microsoft.Jet.OLEDB.4.0;OLE DB Services=-1;Data Source=", "Provider=Microsoft.ACE.OLEDB.12.0;Data Source="}
    Public CgCriteres As New Hashtable(CgNCriterions)
    Public CgVirtualPath As String
    Public VgDB As OleDbConnection
    Public VgDBCommand As New OleDbCommand
    Public VgDBReader As OleDbDataReader
    Public VgImgSeries As New ImageList
    Public VgRemoteDate As Date
    Public VgOptions As New frmOptions
    Public VgSessionSettings As New clsSessionSettings
    Public VgRandom As New Random(Now.Millisecond)
    Public Enum eFormat
        MTGMv2 = 0
        MTGM
        MTGArena
        Apprentice
        MWS
        UrzaGatherer
        Web
    End Enum
    Public Enum eServer
        FreePagesPerso
        ChromeLightStudio
    End Enum
    Public Enum eMarketServer
        MagicVille
        MagicCardMarket
    End Enum
    Public Enum ePicturesSource
        Local
        Online
    End Enum
    Public Enum eSearchType
        Alpha = 0
        Num
        NumOverAlpha
    End Enum
    Public Enum eCountMode
        All
        Distinct
        OnlyReserve
        NoReserve
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
        EditionVO
        EditionVF
        Cout
        TypeVO
        TypeVF
    End Enum
    Public Enum eSortCriteria
        Price
        Quality
        Seller
    End Enum
    Public Enum eBasketMode
        Local
        Remote
    End Enum
    Public Enum eQuality
        Mint = 0
        NearMint
        Excellent
        Good
        LightPlayed
        Played
        Poor
    End Enum
    Public Enum eModeCarac
        Serie = 0
        Couleur
        Type
    End Enum
    Public Enum ePassiveUpdate
        NotNow = 0
        InProgress
        Done
        Failed
    End Enum
    Public Enum eDBVersion
        Unknown = 0 'version inconnue (base corrompue)
        BDD_v1      'manque date confrontation dans MyScores, tournois Explorer&Alchemy, tournois Pioneer&Historic, UrzaId, tournois MTGO, MultiverseId, dossiers decks, tournois 1V1&Multi, codes éditions, infos Réserve, MyGamesID, SubTypes, CardDouble, tournois M, ajustement types numériques, manque Adversaires, manque Historique prix, Autorisations, TextesFR, jeux indépendants dans MyScores, SpecialUse et MySpecialUses, MyGamesID et MyScores (+ éventuellement CardPictures, mais non géré, réinstallation par l'utilisateur nécessaire)
        BDD_v2      'manque date confrontation dans MyScores, tournois Explorer&Alchemy, tournois Pioneer&Historic, UrzaId, tournois MTGO, MultiverseId, dossiers decks, tournois 1V1&Multi, codes éditions, infos Réserve, MyGamesID, SubTypes, CardDouble, tournois M, ajustement types numériques, manque Adversaires, manque Historique prix, Autorisations, TextesFR, jeux indépendants dans MyScores, SpecialUse et MySpecialUses, MyGamesID et les versions dans MyScores
        BDD_v3      'manque date confrontation dans MyScores, tournois Explorer&Alchemy, tournois Pioneer&Historic, UrzaId, tournois MTGO, MultiverseId, dossiers decks, tournois 1V1&Multi, codes éditions, infos Réserve, MyGamesID, SubTypes, CardDouble, tournois M, ajustement types numériques, manque Adversaires, manque Historique prix, Autorisations, TextesFR, jeux indépendants dans MyScores, SpecialUse et MySpecialUses, MyGamesID
        BDD_v4      'manque date confrontation dans MyScores, tournois Explorer&Alchemy, tournois Pioneer&Historic, UrzaId, tournois MTGO, MultiverseId, dossiers decks, tournois 1V1&Multi, codes éditions, infos Réserve, MyGamesID, SubTypes, CardDouble, tournois M, ajustement types numériques, manque Adversaires, manque Historique prix, Autorisations, TextesFR, jeux indépendants dans MyScores, SpecialUse et MySpecialUses
        BDD_v5      'manque date confrontation dans MyScores, tournois Explorer&Alchemy, tournois Pioneer&Historic, UrzaId, tournois MTGO, MultiverseId, dossiers decks, tournois 1V1&Multi, codes éditions, infos Réserve, MyGamesID, SubTypes, CardDouble, tournois M, ajustement types numériques, manque Adversaires, manque Historique prix, Autorisations, TextesFR
        BDD_v6      'manque date confrontation dans MyScores, tournois Explorer&Alchemy, tournois Pioneer&Historic, UrzaId, tournois MTGO, MultiverseId, dossiers decks, tournois 1V1&Multi, codes éditions, infos Réserve, MyGamesID, SubTypes, CardDouble, tournois M, ajustement types numériques, manque Adversaires, manque Historique prix, Autorisations
        BDD_v7      'manque date confrontation dans MyScores, tournois Explorer&Alchemy, tournois Pioneer&Historic, UrzaId, tournois MTGO, MultiverseId, dossiers decks, tournois 1V1&Multi, codes éditions, infos Réserve, MyGamesID, SubTypes, CardDouble, tournois M, ajustement types numériques, manque Adversaires, manque Historique prix
        BDD_v8      'manque date confrontation dans MyScores, tournois Explorer&Alchemy, tournois Pioneer&Historic, UrzaId, tournois MTGO, MultiverseId, dossiers decks, tournois 1V1&Multi, codes éditions, infos Réserve, MyGamesID, SubTypes, CardDouble, tournois M, ajustement types numériques, manque Adversaires
        BDD_v9      'manque date confrontation dans MyScores, tournois Explorer&Alchemy, tournois Pioneer&Historic, UrzaId, tournois MTGO, MultiverseId, dossiers decks, tournois 1V1&Multi, codes éditions, infos Réserve, MyGamesID, SubTypes, CardDouble, tournois M, ajustement types numériques
        BDD_v10     'manque date confrontation dans MyScores, tournois Explorer&Alchemy, tournois Pioneer&Historic, UrzaId, tournois MTGO, MultiverseId, dossiers decks, tournois 1V1&Multi, codes éditions, infos Réserve, MyGamesID, SubTypes, CardDouble, tournois M
        BDD_v11     'manque date confrontation dans MyScores, tournois Explorer&Alchemy, tournois Pioneer&Historic, UrzaId, tournois MTGO, MultiverseId, dossiers decks, tournois 1V1&Multi, codes éditions, infos Réserve, MyGamesID, SubTypes, CardDouble
        BDD_v12     'manque date confrontation dans MyScores, tournois Explorer&Alchemy, tournois Pioneer&Historic, UrzaId, tournois MTGO, MultiverseId, dossiers decks, tournois 1V1&Multi, codes éditions, infos Réserve, MyGamesID, SubTypes
        BDD_v13     'manque date confrontation dans MyScores, tournois Explorer&Alchemy, tournois Pioneer&Historic, UrzaId, tournois MTGO, MultiverseId, dossiers decks, tournois 1V1&Multi, codes éditions, infos Réserve, MyGamesID
        BDD_v14     'manque date confrontation dans MyScores, tournois Explorer&Alchemy, tournois Pioneer&Historic, UrzaId, tournois MTGO, MultiverseId, dossiers decks, tournois 1V1&Multi, codes éditions, infos Réserve
        BDD_v15     'manque date confrontation dans MyScores, tournois Explorer&Alchemy, tournois Pioneer&Historic, UrzaId, tournois MTGO, MultiverseId, dossiers decks, tournois 1V1&Multi, codes éditions
        BDD_v16     'manque date confrontation dans MyScores, tournois Explorer&Alchemy, tournois Pioneer&Historic, UrzaId, tournois MTGO, MultiverseId, dossiers decks, tournois 1V1&Multi
        BDD_v17     'manque date confrontation dans MyScores, tournois Explorer&Alchemy, tournois Pioneer&Historic, UrzaId, tournois MTGO, MultiverseId, dossiers decks
        BDD_v18     'manque date confrontation dans MyScores, tournois Explorer&Alchemy, tournois Pioneer&Historic, UrzaId, tournois MTGO, MultiverseId
        BDD_v19     'manque date confrontation dans MyScores, tournois Explorer&Alchemy, tournois Pioneer&Historic, UrzaId, tournois MTGO
        BDD_v20     'manque date confrontation dans MyScores, tournois Explorer&Alchemy, tournois Pioneer&Historic, UrzaId
        BDD_v21     'manque date confrontation dans MyScores, tournois Explorer&Alchemy, tournois Pioneer&Historic
        BDD_v22     'manque date confrontation dans MyScores, tournois Explorer&Alchemy
        BDD_v23     'manque date confrontation dans MyScores
        BDD_v24     'à jour
    End Enum
    Public Enum eDBProvider
        Jet = 0
        ACE
    End Enum
End Module
