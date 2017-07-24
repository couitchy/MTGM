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
'| Release 13     |                        09/05/2014 |
'| Release 14     |                        09/05/2015 |
'| Release 15     |                        15/01/2017 |
'| Auteur         |                          Couitchy |
'|----------------------------------------------------|
'| Modifications :                                    |
'| - traduction des options				   03/04/2010 |
'| - réflexion .INI => sérialisation .XML  11/11/2011 |
'------------------------------------------------------
#Region "Importations"
Imports System.Data
Imports System.Data.OleDb
Imports System.ComponentModel
Imports System.Xml
Imports System.Xml.Serialization
Imports System.IO
Imports System.Text
#End Region
Public Partial Class Options
	Public VgSettings As New clsSettings
	Public Sub New()
		Me.InitializeComponent()
	End Sub
	Private Sub OptionsLoad(sender As Object, e As System.EventArgs)
		Me.propOptions.SelectedObject = VgSettings
	End Sub
	Private Sub OptionsFormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs)
		Call SaveSettings
	End Sub
	Public Sub SaveSettings
	'-----------------------------------------------------------------------
	'Sauvegarde les propriétés actuelles du PropertyGrid dans le fichier XML
	'-----------------------------------------------------------------------
    Dim VpXmlSerializer As New XmlSerializer(GetType(clsSettings))
    Dim VpFile As FileStream
    Dim VpWriter As XmlTextWriter
		Try
			VpFile = New FileStream(Application.StartupPath + clsModule.CgXMLFile, FileMode.Create)
			VpWriter = New XmlTextWriter(VpFile, Nothing)
	        VpXmlSerializer.Serialize(VpWriter, VgSettings)
	        VpWriter.Close
	        VpFile.Close
		Catch
			Call clsModule.ShowWarning(clsModule.CgErr11)
		End Try
	End Sub
	Public Sub LoadSettings
	'----------------------------------------------------------------------------
	'Restaure les propriétés sauvegardées du PropertyGrid à partir du fichier XML
	'----------------------------------------------------------------------------
    Dim VpXmlSerializer As XmlSerializer
    Dim VpFile As FileStream
    Dim VpReader As XmlTextReader
	    If File.Exists(Application.StartupPath + clsModule.CgXMLFile) Then
	    	Try
			    VpXmlSerializer = New XmlSerializer(GetType(clsSettings))
			    VpFile = New FileStream(Application.StartupPath + clsModule.CgXMLFile, FileMode.Open)
			    VpReader = New XmlTextReader(VpFile)
		   		VgSettings = CType(VpXmlSerializer.Deserialize(VpReader), clsSettings)
		        VpReader.Close
		        VpFile.Close
	    	Catch
	    		Call clsModule.ShowWarning(clsModule.CgErr11)
	    	End Try
		ElseIf File.Exists(Application.StartupPath + clsModule.CgINIFile) Then
			Call clsModule.ShowInformation(clsModule.CgErr8)
		End If
	End Sub
	Public Class clsSettings
		Private VmNJeux As Integer = 1
		Private VmNomsJeux As String = ""
		Private VmDBProvider As clsModule.eDBProvider = clsModule.eDBProvider.Jet
		Private VmDefaultBase As String = Application.StartupPath + clsModule.CgMDB
		Private VmPicturesFile As String = Application.StartupPath + clsModule.CgDAT
		Private VmMagicBack As String = Application.StartupPath + clsModule.CgMagicBack
		Private VmPlugins As String = Application.StartupPath
		Private VmBannedSellers As String = ""
		Private VmDefaultActivatedCriteria As String = "1#7"
		Private VmDefaultCriteriaOrder As String = "Type#Couleur#Edition#Coût d'invocation#Rareté#Prix#Quantité#Carte"
		Private VmRestoreCriteria As Boolean = True
		Private VmDefaultSearchCriterion As clsModule.eSearchCriterion = clsModule.eSearchCriterion.NomVF
		Private VmCheckForUpdate As Boolean = True
		Private VmAutoRefresh As Boolean = True
		Private VmImageMode As PictureBoxSizeMode = PictureBoxSizeMode.CenterImage
		Private VmAutoHideImage As Boolean = False
		Private VmAutoHideAutorisations As Boolean = False
		Private VmRestoreSize As Boolean = False
		Private VmRestoredWidth As Integer = 1008
		Private VmRestoredHeight As Integer = 604
		Private VmFormWindowState As FormWindowState = FormWindowState.Normal
		Private VmLastUpdateAut As String = clsModule.CgLastUpdateAut
		Private VmLastUpdateSimu As String = clsModule.CgLastUpdateSimu
		Private VmLastUpdateTxtVF As String = clsModule.CgLastUpdateTxtVF
		Private VmLastUpdateRulings As String = clsModule.CgLastUpdateRulings
		Private VmLastUpdatePictPatch As String = clsModule.CgLastUpdatePictPatch
		Private VmLastUpdateTradPatch As String = clsModule.CgLastUpdateTradPatch
		Private VmLastUpdateSubTypesPatch As String = clsModule.CgLastUpdateSubsPatch
		Private VmLastUpdateSubTypesVFPatch As String = clsModule.CgLastUpdateSubsVFPatch
		Private VmLastUpdateMultiverseIdPatch As String = clsModule.CgLastUpdateMultiIdPatch
		Private VmShowUpdateMenus As Boolean = False
		Private VmPrevSearches As String = ""
		Private VmVFDefault As Boolean = True
		Private VmShowCorruption As Boolean = True
		Private VmCopyRange As Integer = 1
		Private VmShowLines As Boolean = False
		Private VmDownloadServerEnum As clsModule.eServer = clsModule.eServer.FreePagesPerso
		Private VmMarketServerEnum As clsModule.eMarketServer = clsModule.eMarketServer.MagicCardMarket
		Private VmShowAllSeries As Boolean = False
		Private VmPicturesSource As clsModule.ePicturesSource = clsmodule.ePicturesSource.Local
		Private VmFontSize As Single = -1
		<DisplayName("Critère de recherche"), Category("Général"), DefaultValue(clsModule.eSearchCriterion.NomVF), Description("Critère de recherche par défaut pour la recherche avancée")> _
		Public Property DefaultSearchCriterion As clsModule.eSearchCriterion
			Get
				Return VmDefaultSearchCriterion
			End Get
			Set (VpDefaultSearchCriterion As clsModule.eSearchCriterion)
				VmDefaultSearchCriterion = VpDefaultSearchCriterion
			End Set
		End Property
		<DisplayName("Taille de police"), Category("Général"), Description("Taille par défaut des polices pour les textes dans le panneau des propriétés / détails (redémarrer l'application pour prendre en compte les modifications)")> _
		Public Property FontSize As Single
			Get
				Return VmFontSize
			End Get
			Set (VpFontSize As Single)
				VmFontSize = VpFontSize
			End Set
		End Property
		<DisplayName("Restaurer le fenêtrage"), Category("Général"), DefaultValue(False), Description("Mémoriser la taille de la fenêtre principale à la fermeture")> _
		Public Property RestoreSize As Boolean
			Get
				Return VmRestoreSize
			End Get
			Set (VpRestoreSize As Boolean)
				VmRestoreSize = VpRestoreSize
			End Set
		End Property
		<DisplayName("Avertissement de corruption"), Category("Général"), DefaultValue(True), Description("Affiche un message d'avertissement lorsque le chargement d'une image échoue")> _
		Public Property ShowCorruption As Boolean
			Get
				Return VmShowCorruption
			End Get
			Set (VpShowCorruption As Boolean)
				VmShowCorruption = VpShowCorruption
			End Set
		End Property
		<Browsable(False), Category("Général"), Description("Mémorisation largeur")> _
		Public Property RestoredWidth As Integer
			Get
				Return VmRestoredWidth
			End Get
			Set (VpRestoredWidth As Integer)
				VmRestoredWidth = VpRestoredWidth
			End Set
		End Property
		<Browsable(False), Category("Général"), Description("Mémorisation hauteur")> _
		Public Property RestoredHeight As Integer
			Get
				Return VmRestoredHeight
			End Get
			Set (VpRestoredHeight As Integer)
				VmRestoredHeight = VpRestoredHeight
			End Set
		End Property
		<Browsable(False), Category("Général"), Description("Mémorisation fenêtrage")> _
		Public Property RestoredState As FormWindowState
			Get
				Return VmFormWindowState
			End Get
			Set (VpFormWindowState As FormWindowState)
				VmFormWindowState = VpFormWindowState
			End Set
		End Property
		<DisplayName("Vérifier les mises à jour"), Category("Mises à jour"), DefaultValue(True), Description("Vérifier régulièrement si une mise à jour existe pour l'application")> _
		Public Property CheckForUpdate As Boolean
			Get
				Return VmCheckForUpdate
			End Get
			Set (VpCheckForUpdate As Boolean)
				VmCheckForUpdate = VpCheckForUpdate
			End Set
		End Property
		<Browsable(False), Category("Général"), Description("Nombre de decks")> _
		Public Property NJeux As Integer
			Get
				Return VmNJeux
			End Get
			Set (VpNJeux As Integer)
				VmNJeux = VpNJeux
			End Set
		End Property
		<Browsable(False), Category("Général"), Description("Noms des decks (à séparer par un dièse)")> _
		Public Property NomsJeux As String
			Get
				Return VmNomsJeux
			End Get
			Set (VpNomsJeux As String)
				VmNomsJeux = VpNomsJeux
			End Set
		End Property
		<Browsable(False), Category("Général"), Description("Noms des vendeurs bannis (à séparer par un dièse)")> _
		Public Property BannedSellers As String
			Get
				Return VmBannedSellers
			End Get
			Set (VpBannedSellers As String)
				VmBannedSellers = VpBannedSellers
			End Set
		End Property
		<DisplayName("Base par défaut"), Category("Emplacements des fichiers"), Description("Base de données à ouvrir par défaut"), Editor(GetType(UIFilenameEditor), GetType(Drawing.Design.UITypeEditor)), FileDialogFilter("Fichiers de base de données Microsoft Access (*.mdb)|*.mdb")> _
		Public Property DefaultBase As String
			Get
				Return VmDefaultBase
			End Get
			Set (VpDefaultBase As String)
				VmDefaultBase = VpDefaultBase
			End Set
		End Property
		<DisplayName("Restaurer les filtres"), Category("Explorateur"), DefaultValue(True), Description("Mémoriser l'état des critères de classement à la fermeture")> _
		Public Property RestoreCriteria As Boolean
			Get
				Return VmRestoreCriteria
			End Get
			Set (VpRestoreCriteria As Boolean)
				VmRestoreCriteria = VpRestoreCriteria
			End Set
		End Property
		<DisplayName("Afficher les pointillés"), Category("Explorateur"), DefaultValue(False), Description("Afficher les pointillés dans l'arborescence")> _
		Public Property ShowLines As Boolean
			Get
				Return VmShowLines
			End Get
			Set (VpShowLines As Boolean)
				VmShowLines = VpShowLines
			End Set
		End Property
		<Browsable(False), Category("Explorateur"), Description("Critères activés par défaut (à séparer par un dièse, Type = 1, Couleur = 2, Edition = 3, Coût d'invocation = 4 etc.)")> _
		Public Property DefaultActivatedCriteria As String
			Get
				If VmRestoreCriteria Then
					Return VmDefaultActivatedCriteria
				Else
					Return "1#7"
				End If
			End Get
			Set (VpDefaultActivatedCriteria As String)
				VmDefaultActivatedCriteria = VpDefaultActivatedCriteria
			End Set
		End Property
		<Browsable(False), Category("Explorateur"), Description("Ordre par défaut des critères (à séparer par un dièse)")> _
		Public Property DefaultCriteriaOrder As String
			Get
				Return VmDefaultCriteriaOrder
			End Get
			Set (VpDefaultCriteriaOrder As String)
				VmDefaultCriteriaOrder = VpDefaultCriteriaOrder
			End Set
		End Property
		<DisplayName("Rafraîchissement automatique"), Category("Explorateur"), DefaultValue(True), Description("Rafraîchir automatiquement l'arborescence après avoir ajouté ou supprimé des cartes")> _
		Public Property AutoRefresh As Boolean
			Get
				Return VmAutoRefresh
			End Get
			Set (VpAutoRefresh As Boolean)
				VmAutoRefresh = VpAutoRefresh
			End Set
		End Property
		<DisplayName("Cartes en français par défaut"), Category("Explorateur"), DefaultValue(True), Description("Toujours considérer le français comme la langue par défaut pour le titre et le texte des cartes")> _
		Public Property VFDefault As Boolean
			Get
				Return VmVFDefault
			End Get
			Set (VpVFDefault As Boolean)
				VmVFDefault = VpVFDefault
			End Set
		End Property
		<DisplayName("Masquer les images"), Category("Explorateur"), DefaultValue(False), Description("Toujours fermer le panneau image au démarrage")> _
		Public Property AutoHideImage As Boolean
			Get
				Return VmAutoHideImage
			End Get
			Set (VpAutoHideImage As Boolean)
				VmAutoHideImage = VpAutoHideImage
			End Set
		End Property
		<DisplayName("Masquer les autorisations"), Category("Explorateur"), DefaultValue(False), Description("Toujours fermer le panneau des autorisations en tournois")> _
		Public Property AutoHideAutorisations As Boolean
			Get
				Return VmAutoHideAutorisations
			End Get
			Set (VpAutoHideAutorisations As Boolean)
				VmAutoHideAutorisations = VpAutoHideAutorisations
			End Set
		End Property
		<DisplayName("Limite pour les cartes à copier"), Category("Explorateur"), DefaultValue(1), Description("Nombre maximal de cartes que l'on peut copier avec 'Copier vers...'. Si cette valeur est réglée sur 1, la fenêtre permettant d'ajuster la quantité de cartes ne s'affichera pas systématiquement.")> _
		Public Property CopyRange As Integer
			Get
				Return VmCopyRange
			End Get
			Set (VpCopyRange As Integer)
				VmCopyRange = Math.Max(VpCopyRange, 1)
			End Set
		End Property
		<DisplayName("Afficher toutes les éditions"), Category("Explorateur"), DefaultValue(False), Description("Afficher par défaut toutes les éditions disponibles pour la carte sélectionnée dans l'arborescence")> _
		Public Property ShowAllSeries As Boolean
			Get
				Return VmShowAllSeries
			End Get
			Set (VpShowAllSeries As Boolean)
				VmShowAllSeries = VpShowAllSeries
			End Set
		End Property
		<DisplayName("Base des images"), Category("Emplacements des fichiers"), Description("Fichier des images numérisées des cartes"), Editor(GetType(UIFilenameEditor), GetType(Drawing.Design.UITypeEditor)), FileDialogFilter("Fichiers de données d'images (*.dat)|*.dat")> _
		Public Property PicturesFile As String
			Get
				Return VmPicturesFile
			End Get
			Set (VpPicturesFile As String)
				VmPicturesFile = VpPicturesFile
			End Set
		End Property
		<DisplayName("Image de fond"), Category("Emplacements des fichiers"), Description("Image de fond à afficher par défaut dans le panneau latéral"), Editor(GetType(UIFilenameEditor), GetType(Drawing.Design.UITypeEditor)), FileDialogFilter("Images JPEG (*.jpg)|*.jpg")> _
		Public Property MagicBack As String
			Get
				Return VmMagicBack
			End Get
			Set (VpMagicBack As String)
				VmMagicBack = VpMagicBack
			End Set
		End Property
		<DisplayName("Emplacement des plug-ins"), Category("Emplacements des fichiers"), Description("Chemin d'accès au dossier des plug-ins MTGM"), Editor(GetType(System.Windows.Forms.Design.FolderNameEditor), GetType(System.Drawing.Design.UITypeEditor))> _
		Public Property Plugins As String
			Get
				Return VmPlugins
			End Get
			Set (VpPlugins As String)
				VmPlugins = VpPlugins
			End Set
		End Property
		<DisplayName("Format des images"), Category("Explorateur"), DefaultValue(PictureBoxSizeMode.CenterImage), Description("Mode d'affichage des images des cartes")> _
		Public Property ImageMode As PictureBoxSizeMode
			Get
				Return VmImageMode
			End Get
			Set (VpImageMode As PictureBoxSizeMode)
				VmImageMode = VpImageMode
			End Set
		End Property
		<DisplayName("Source des images"), Category("Explorateur"), DefaultValue(clsModule.ePicturesSource.Local), Description("Emplacement source des images des cartes")> _
		Public Property PicturesSource As clsModule.ePicturesSource
			Get
				Return VmPicturesSource
			End Get
			Set (VpPicturesSource As clsModule.ePicturesSource)
				VmPicturesSource = VpPicturesSource
			End Set
		End Property
		<Browsable(False), Category("Mises à jour"), Description("Date de dernière mise à jour des autorisations de tournoi")> _
		Public Property LastUpdateAut As String
			Get
				Return VmLastUpdateAut
			End Get
			Set (VpLastUpdateAut As String)
				VmLastUpdateAut = VpLastUpdateAut
			End Set
		End Property
		<Browsable(False), Category("Mises à jour"), Description("Date de dernière mise à jour des modèles de simulations / historiques")> _
		Public Property LastUpdateSimu As String
			Get
				Return VmLastUpdateSimu
			End Get
			Set (VpLastUpdateSimu As String)
				VmLastUpdateSimu = VpLastUpdateSimu
			End Set
		End Property
		<Browsable(False), Category("Mises à jour"), Description("Date de dernière mise à jour des textes des cartes en VF")> _
		Public Property LastUpdateTxtVF As String
			Get
				Return VmLastUpdateTxtVF
			End Get
			Set (VpLastUpdateTxtVF As String)
				VmLastUpdateTxtVF = VpLastUpdateTxtVF
			End Set
		End Property
		<Browsable(False), Category("Mises à jour"), Description("Date de dernière mise à jour des règles spécifiques des cartes")> _
		Public Property LastUpdateRulings As String
			Get
				Return VmLastUpdateRulings
			End Get
			Set (VpLastUpdateRulings As String)
				VmLastUpdateRulings = VpLastUpdateRulings
			End Set
		End Property
		<Browsable(False), Category("Mises à jour"), Description("Date de dernière mise à jour du correctif des images")> _
		Public Property LastUpdatePictPatch As String
			Get
				Return VmLastUpdatePictPatch
			End Get
			Set (VpLastUpdatePictPatch As String)
				VmLastUpdatePictPatch = VpLastUpdatePictPatch
			End Set
		End Property
		<Browsable(False), Category("Mises à jour"), Description("Date de dernière mise à jour du correctif des titres des cartes en VF")> _
		Public Property LastUpdateTradPatch As String
			Get
				Return VmLastUpdateTradPatch
			End Get
			Set (VpLastUpdateTradPatch As String)
				VmLastUpdateTradPatch = VpLastUpdateTradPatch
			End Set
		End Property
		<Browsable(False), Category("Mises à jour"), Description("Date de dernière mise à jour du correctif des sous-types des cartes")> _
		Public Property LastUpdateSubTypesPatch As String
			Get
				Return VmLastUpdateSubTypesPatch
			End Get
			Set (VpLastUpdateSubTypesPatch As String)
				VmLastUpdateSubTypesPatch = VpLastUpdateSubTypesPatch
			End Set
		End Property
		<Browsable(False), Category("Mises à jour"), Description("Date de dernière mise à jour du correctif des traductions des sous-types")> _
		Public Property LastUpdateSubTypesVFPatch As String
			Get
				Return VmLastUpdateSubTypesVFPatch
			End Get
			Set (VpLastUpdateSubTypesVFPatch As String)
				VmLastUpdateSubTypesVFPatch = VpLastUpdateSubTypesVFPatch
			End Set
		End Property
		<Browsable(False), Category("Mises à jour"), Description("Date de dernière mise à jour des identifiants Multiverse")> _
		Public Property LastUpdateMultiverseIdPatch As String
			Get
				Return VmLastUpdateMultiverseIdPatch
			End Get
			Set (VpLastUpdateMultiverseIdPatch As String)
				VmLastUpdateMultiverseIdPatch = VpLastUpdateMultiverseIdPatch
			End Set
		End Property
		<Browsable(False), Category("Général"), Description("Liste des dernières recherches effectuées")> _
		Public Property PrevSearches As String
			Get
				Return VmPrevSearches
			End Get
			Set (VpPrevSearches As String)
				VmPrevSearches = VpPrevSearches
			End Set
		End Property
		<DisplayName("Afficher les menus détaillés des mises à jour"), Category("Mises à jour"), DefaultValue(False), Description("Permet d'afficher l'ensemble des menus des mises à jour (obsolète)")> _
		Public Property ShowUpdateMenus As Boolean
			Get
				Return VmShowUpdateMenus
			End Get
			Set (VpShowUpdateMenus As Boolean)
				VmShowUpdateMenus = VpShowUpdateMenus
			End Set
		End Property
		<DisplayName("Serveur de téléchargement"), Category("Mises à jour"), Description("Serveur de téléchargement à contacter pour télécharger les mises à jour d'application et de contenu")> _
		Public Property DownloadServerEnum As clsModule.eServer
			Get
				Return VmDownloadServerEnum
			End Get
			Set (VpDownloadServerEnum As clsModule.eServer)
				VmDownloadServerEnum = VpDownloadServerEnum
			End Set
		End Property
		<DisplayName("Serveur d'achats de cartes"), Category("Mises à jour"), Description("Serveur à contacter pour récupérer les informations d'achats / ventes")> _
		Public Property MarketServerEnum As clsModule.eMarketServer
			Get
				Return VmMarketServerEnum
			End Get
			Set (VpMarketServerEnum As clsModule.eMarketServer)
				VmMarketServerEnum = VpMarketServerEnum
			End Set
		End Property
		<XmlIgnore(), Browsable(False), Category("Mises à jour"), Description("URL explicite du serveur de téléchargement")> _
		Public ReadOnly Property DownloadServer As String
			Get
				Select Case VmDownloadServerEnum
					Case clsModule.eServer.FreePagesPerso
						Return clsModule.CgDefaultServer
					Case clsModule.eServer.ChromeLightStudio
						Return "http://chromelight.brutin.fr/MTGM"
					Case Else
						Return ""
				End Select
			End Get
		End Property
		<DisplayName("Moteur de base de données"), Category("Général"), Description("Composant logiciel OLEDB permettant d'accéder aux fichiers de base de données Microsoft Access")> _
		Public Property DBProvider As clsModule.eDBProvider
			Get
				Return VmDBProvider
			End Get
			Set (VpDBProvider As clsModule.eDBProvider)
				VmDBProvider = VpDBProvider
			End Set
		End Property
	End Class
End Class
Public Class clsSessionSettings
	Private VmFreeTempFileIndex As Integer = -1
	Private VmGridClearing As Boolean = False
	Private VmSplitterDistance As Integer
	Private VmFormSubWidth As Integer
	Public Property FreeTempFileIndex As Integer
		Get
			Return VmFreeTempFileIndex
		End Get
		Set (VpFreeTempFileIndex As Integer)
			VmFreeTempFileIndex = VpFreeTempFileIndex
		End Set
	End Property
	Public Property GridClearing As Boolean
		Get
			Return VmGridClearing
		End Get
		Set (VpGridClearing As Boolean)
			VmGridClearing = VpGridClearing
		End Set
	End Property
	Public Property SplitterDistance As Integer
		Get
			Return VmSplitterDistance
		End Get
		Set (VpSplitterDistance As Integer)
			VmSplitterDistance = VpSplitterDistance
		End Set
	End Property
	Public Property FormSubWidth As Integer
		Get
			Return VmFormSubWidth
		End Get
		Set (VpFormSubWidth As Integer)
			VmFormSubWidth = VpFormSubWidth
		End Set
	End Property
End Class