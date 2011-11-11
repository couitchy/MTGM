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
'| Auteur         |                          Couitchy |
'|----------------------------------------------------|
'| Modifications :                                    |
'| - traduction des options				   03/04/2010 |
'| - r�flexion .INI => s�rialisation .XML  11/11/2011 |
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
	'Sauvegarde les propri�t�s actuelles du PropertyGrid dans le fichier XML
	'-----------------------------------------------------------------------
    Dim VpXmlSerializer As New XmlSerializer(GetType(clsSettings))
    Dim VpFile As FileStream
    Dim VpWriter As XmlTextWriter
		VpFile = New FileStream(Application.StartupPath + clsModule.CgXMLFile, FileMode.Create)
		VpWriter = New XmlTextWriter(VpFile, Nothing)
        VpXmlSerializer.Serialize(VpWriter, VgSettings)
        VpWriter.Close
        VpFile.Close
	End Sub
	Public Sub LoadSettings
	'----------------------------------------------------------------------------
	'Restaure les propri�t�s sauvegard�es du PropertyGrid � partir du fichier XML
	'----------------------------------------------------------------------------
    Dim VpXmlSerializer As XmlSerializer
    Dim VpFile As FileStream
    Dim VpReader As XmlTextReader
		If File.Exists(Application.StartupPath + clsModule.CgXMLFile) Then
		    VpXmlSerializer = New XmlSerializer(GetType(clsSettings))
		    VpFile = New FileStream(Application.StartupPath + clsModule.CgXMLFile, FileMode.Open)
		    VpReader = New XmlTextReader(VpFile)
	   		VgSettings = CType(VpXmlSerializer.Deserialize(VpReader), clsSettings)
	        VpReader.Close
	   	    VpFile.Close
		ElseIf File.Exists(Application.StartupPath + clsModule.CgINIFile) Then
			Call clsModule.ShowInformation(clsModule.CgErr8)
		End If
	End Sub
End Class
Public Class clsSettings
	Private VmNJeux As Integer = 1
	Private VmNomsJeux As String = ""
	Private VmDBProvider As clsModule.eDBProvider = clsModule.eDBProvider.Jet
	Private VmDefaultBase As String = Application.StartupPath + clsModule.CgMDB
	Private VmPicturesFile As String = Application.StartupPath + clsModule.CgDAT
	Private VmMagicBack As String = Application.StartupPath + clsModule.CgMagicBack
	Private VmPlugins As String = Application.StartupPath
	Private VmPreferredSellers As String = ""
	Private VmDefaultActivatedCriteria As String = "2#7"
	Private VmDefaultCriteriaOrder As String = "Decks#Type#Couleur#Edition#Co�t d'invocation#Raret�#Prix#Carte"
	Private VmRestoreCriteria As Boolean = True
	Private VmDefaultSearchCriterion As clsModule.eSearchCriterion = clsModule.eSearchCriterion.NomVF
	Private VmCheckForUpdate As Boolean = True
	Private VmAutoRefresh As Boolean = True
	Private VmImageMode As PictureBoxSizeMode = PictureBoxSizeMode.CenterImage
	Private VmAutoHideImage As Boolean = False
	Private VmAutoHideAutorisations As Boolean = False
	Private VmRestoreSize As Boolean = False
	Private VmRestoredWidth As Integer = 773
	Private VmRestoredHeight As Integer = 435
	Private VmRestoredSplitterDistance As Integer = 68
	Private VmFormWindowState As FormWindowState = FormWindowState.Normal
	Private VmForceSingleSource As Boolean = False
	Private VmLastUpdateAut As String = ""
	Private VmLastUpdateSimu As String = ""
	Private VmLastUpdateTxtVF As String = ""
	Private VmLastUpdatePictPatch As String = ""
	Private VmLastUpdateTradPatch As String = ""
	Private VmShowUpdateMenus As Boolean = False
	Private VmPrevSearches As String = ""
	Private VmVFDefault As Boolean = True
	Private VmShowCorruption As Boolean = True
	Private VmCopyRange As Integer = 1
	Private VmShowLines As Boolean = False
	Private VmDownloadServer As String = clsModule.CgDefaultServer
	<DisplayName("Crit�re de recherche"), Category("G�n�ral"), DefaultValue(clsModule.eSearchCriterion.NomVF), Description("Crit�re de recherche par d�faut pour la recherche avanc�e")> _
	Public Property DefaultSearchCriterion As clsModule.eSearchCriterion
		Get
			Return VmDefaultSearchCriterion
		End Get
		Set (VpDefaultSearchCriterion As clsModule.eSearchCriterion)
			VmDefaultSearchCriterion = VpDefaultSearchCriterion
		End Set
	End Property
	<DisplayName("Restaurer le fen�trage"), Category("G�n�ral"), DefaultValue(False), Description("M�moriser la taille de la fen�tre principale � la fermeture")> _
	Public Property RestoreSize As Boolean
		Get
			Return VmRestoreSize
		End Get
		Set (VpRestoreSize As Boolean)
			VmRestoreSize = VpRestoreSize
		End Set
	End Property
	<DisplayName("Avertissement de corruption"), Category("G�n�ral"), DefaultValue(True), Description("Affiche un message d'avertissement lorsque le chargement d'une image �choue")> _
	Public Property ShowCorruption As Boolean
		Get
			Return VmShowCorruption
		End Get
		Set (VpShowCorruption As Boolean)
			VmShowCorruption = VpShowCorruption
		End Set
	End Property
	<Browsable(False), Category("G�n�ral"), Description("M�morisation largeur")> _
	Public Property RestoredWidth As Integer
		Get
			Return VmRestoredWidth
		End Get
		Set (VpRestoredWidth As Integer)
			VmRestoredWidth = VpRestoredWidth
		End Set
	End Property
	<Browsable(False), Category("G�n�ral"), Description("M�morisation hauteur")> _
	Public Property RestoredHeight As Integer
		Get
			Return VmRestoredHeight
		End Get
		Set (VpRestoredHeight As Integer)
			VmRestoredHeight = VpRestoredHeight
		End Set
	End Property
	<Browsable(False), Category("G�n�ral"), Description("M�morisation distance splitter")> _
	Public Property RestoredSplitterDistance As Integer
		Get
			Return VmRestoredSplitterDistance
		End Get
		Set (VpRestoredSplitterDistance As Integer)
			VmRestoredSplitterDistance = VpRestoredSplitterDistance
		End Set
	End Property
	<Browsable(False), Category("G�n�ral"), Description("M�morisation fen�trage")> _
	Public Property RestoredState As FormWindowState
		Get
			Return VmFormWindowState
		End Get
		Set (VpFormWindowState As FormWindowState)
			VmFormWindowState = VpFormWindowState
		End Set
	End Property
	<DisplayName("V�rifier les mises � jour"), Category("Mises � jour"), DefaultValue(True), Description("V�rifier r�guli�rement si une mise � jour existe pour l'application")> _
	Public Property CheckForUpdate As Boolean
		Get
			Return VmCheckForUpdate
		End Get
		Set (VpCheckForUpdate As Boolean)
			VmCheckForUpdate = VpCheckForUpdate
		End Set
	End Property
	<Browsable(False), Category("G�n�ral"), Description("Nombre de decks")> _
	Public Property NJeux As Integer
		Get
			Return VmNJeux
		End Get
		Set (VpNJeux As Integer)
			VmNJeux = VpNJeux
		End Set
	End Property
	<Browsable(False), Category("G�n�ral"), Description("Noms des decks (� s�parer par un di�se)")> _
	Public Property NomsJeux As String
		Get
			Return VmNomsJeux
		End Get
		Set (VpNomsJeux As String)
			VmNomsJeux = VpNomsJeux
		End Set
	End Property
	<Browsable(False), Category("G�n�ral"), Description("Noms des vendeurs pr�f�r�s (� s�parer par un di�se)")> _
	Public Property PreferredSellers As String
		Get
			Return VmPreferredSellers
		End Get
		Set (VpPreferredSellers As String)
			VmPreferredSellers = VpPreferredSellers
		End Set
	End Property
	<DisplayName("Base par d�faut"), Category("Emplacements des fichiers"), Description("Base de donn�es � ouvrir par d�faut"), Editor(GetType(UIFilenameEditor), GetType(Drawing.Design.UITypeEditor)), FileDialogFilter("Fichiers de base de donn�es Microsoft Access (*.mdb)|*.mdb")> _
	Public Property DefaultBase As String
		Get
			Return VmDefaultBase
		End Get
		Set (VpDefaultBase As String)
			VmDefaultBase = VpDefaultBase
		End Set
	End Property
	<DisplayName("Restaurer les filtres"), Category("Explorateur"), DefaultValue(True), Description("M�moriser l'�tat des crit�res de classement � la fermeture")> _
	Public Property RestoreCriteria As Boolean
		Get
			Return VmRestoreCriteria
		End Get
		Set (VpRestoreCriteria As Boolean)
			VmRestoreCriteria = VpRestoreCriteria
		End Set
	End Property
	<DisplayName("Afficher les pointill�s"), Category("Explorateur"), DefaultValue(False), Description("Afficher les pointill�s dans l'arborescence")> _
	Public Property ShowLines As Boolean
		Get
			Return VmShowLines
		End Get
		Set (VpShowLines As Boolean)
			VmShowLines = VpShowLines
		End Set
	End Property
	<Browsable(False), Category("Explorateur"), Description("Crit�res activ�s par d�faut (� s�parer par un di�se, Type = 1, Couleur = 2, Edition = 3, Co�t d'invocation = 4 etc.)")> _
	Public Property DefaultActivatedCriteria As String
		Get
			If VmRestoreCriteria Then
				Return VmDefaultActivatedCriteria
			Else
				Return "2#7"
			End If
		End Get
		Set (VpDefaultActivatedCriteria As String)
			VmDefaultActivatedCriteria = VpDefaultActivatedCriteria
		End Set
	End Property
	<Browsable(False), Category("Explorateur"), Description("Ordre par d�faut des crit�res (� s�parer par un di�se)")> _
	Public Property DefaultCriteriaOrder As String
		Get
			Return VmDefaultCriteriaOrder
		End Get
		Set (VpDefaultCriteriaOrder As String)
			VmDefaultCriteriaOrder = VpDefaultCriteriaOrder
		End Set
	End Property
	<DisplayName("Rafra�chissement automatique"), Category("Explorateur"), DefaultValue(True), Description("Rafra�chir automatiquement l'arborescence apr�s avoir ajout� ou supprim� des cartes")> _
	Public Property AutoRefresh As Boolean
		Get
			Return VmAutoRefresh
		End Get
		Set (VpAutoRefresh As Boolean)
			VmAutoRefresh = VpAutoRefresh
		End Set
	End Property
	<DisplayName("Cartes en fran�ais par d�faut"), Category("Explorateur"), DefaultValue(True), Description("Toujours consid�rer le fran�ais comme la langue par d�faut pour le titre et le texte des cartes")> _
	Public Property VFDefault As Boolean
		Get
			Return VmVFDefault
		End Get
		Set (VpVFDefault As Boolean)
			VmVFDefault = VpVFDefault
		End Set
	End Property
	<DisplayName("Masquer les images"), Category("Explorateur"), DefaultValue(False), Description("Toujours fermer le panneau image au d�marrage")> _
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
	<DisplayName("Limite pour les cartes � copier"), Category("Explorateur"), DefaultValue(1), Description("Nombre maximal de cartes que l'on peut copier avec 'Copier vers...'. Si cette valeur est r�gl�e sur 1, la fen�tre permettant d'ajuster la quantit� de cartes ne s'affichera pas syst�matiquement.")> _
	Public Property CopyRange As Integer
		Get
			Return VmCopyRange
		End Get
		Set (VpCopyRange As Integer)
			VmCopyRange = Math.Max(VpCopyRange, 1)
		End Set
	End Property
	<DisplayName("Base des images"), Category("Emplacements des fichiers"), Description("Fichier des images num�ris�es des cartes"), Editor(GetType(UIFilenameEditor), GetType(Drawing.Design.UITypeEditor)), FileDialogFilter("Fichiers de donn�es d'images (*.dat)|*.dat")> _
	Public Property PicturesFile As String
		Get
			Return VmPicturesFile
		End Get
		Set (VpPicturesFile As String)
			VmPicturesFile = VpPicturesFile
		End Set
	End Property
	<DisplayName("Image de fond"), Category("Emplacements des fichiers"), Description("Image de fond � afficher par d�faut dans le panneau lat�ral"), Editor(GetType(UIFilenameEditor), GetType(Drawing.Design.UITypeEditor)), FileDialogFilter("Images JPEG (*.jpg)|*.jpg")> _
	Public Property MagicBack As String
		Get
			Return VmMagicBack
		End Get
		Set (VpMagicBack As String)
			VmMagicBack = VpMagicBack
		End Set
	End Property
	<DisplayName("Emplacement des plug-ins"), Category("Emplacements des fichiers"), Description("Chemin d'acc�s au dossier des plug-ins MTGM"), Editor(GetType(System.Windows.Forms.Design.FolderNameEditor), GetType(System.Drawing.Design.UITypeEditor))> _
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
	<DisplayName("Forcer source unique"), Category("Explorateur"), DefaultValue(True), Description("Limitation � une source unique s�lectionn�e dans le menu affichage (un seul deck ou collection)")> _
	Public Property ForceSingleSource As Boolean
		Get
			Return VmForceSingleSource
		End Get
		Set (VpForceSingleSource As Boolean)
			VmForceSingleSource = VpForceSingleSource
		End Set
	End Property
	<Browsable(False), Category("G�n�ral"), Description("Date de derni�re mise � jour des autorisations de tournoi")> _
	Public Property LastUpdateAut As String
		Get
			Return VmLastUpdateAut
		End Get
		Set (VpLastUpdateAut As String)
			VmLastUpdateAut = VpLastUpdateAut
		End Set
	End Property
	<Browsable(False), Category("G�n�ral"), Description("Date de derni�re mise � jour des mod�les de simulations / historiques")> _
	Public Property LastUpdateSimu As String
		Get
			Return VmLastUpdateSimu
		End Get
		Set (VpLastUpdateSimu As String)
			VmLastUpdateSimu = VpLastUpdateSimu
		End Set
	End Property
	<Browsable(False), Category("G�n�ral"), Description("Date de derni�re mise � jour des textes des cartes en VF")> _
	Public Property LastUpdateTxtVF As String
		Get
			Return VmLastUpdateTxtVF
		End Get
		Set (VpLastUpdateTxtVF As String)
			VmLastUpdateTxtVF = VpLastUpdateTxtVF
		End Set
	End Property
	<Browsable(False), Category("G�n�ral"), Description("Date de derni�re mise � jour du correctif des images")> _
	Public Property LastUpdatePictPatch As String
		Get
			Return VmLastUpdatePictPatch
		End Get
		Set (VpLastUpdatePictPatch As String)
			VmLastUpdatePictPatch = VpLastUpdatePictPatch
		End Set
	End Property
	<Browsable(False), Category("G�n�ral"), Description("Date de derni�re mise � jour du correctif des titres des cartes en VF")> _
	Public Property LastUpdateTradPatch As String
		Get
			Return VmLastUpdateTradPatch
		End Get
		Set (VpLastUpdateTradPatch As String)
			VmLastUpdateTradPatch = VpLastUpdateTradPatch
		End Set
	End Property
	<Browsable(False), Category("G�n�ral"), Description("Liste des derni�res recherches effectu�es")> _
	Public Property PrevSearches As String
		Get
			Return VmPrevSearches
		End Get
		Set (VpPrevSearches As String)
			VmPrevSearches = VpPrevSearches
		End Set
	End Property
	<DisplayName("Afficher les menus d�taill�s des mises � jour"), Category("G�n�ral"), DefaultValue(False), Description("Permet d'afficher l'ensemble des menus des mises � jour (obsol�te)")> _
	Public Property ShowUpdateMenus As Boolean
		Get
			Return VmShowUpdateMenus
		End Get
		Set (VpShowUpdateMenus As Boolean)
			VmShowUpdateMenus = VpShowUpdateMenus
		End Set
	End Property
	<DisplayName("Serveur de t�l�chargement"), Category("Mises � jour"), Description("Serveur de t�l�chargement � contacter pour t�l�charger les mises � jour d'application et de contenu")> _
	Public Property DownloadServer As String
		Get
			Return VmDownloadServer
		End Get
		Set (VpDownloadServer As String)
			VmDownloadServer = VpDownloadServer
		End Set
	End Property
	<DisplayName("Moteur de base de donn�es"), Category("G�n�ral"), Description("Composant logiciel OLEDB permettant d'acc�der aux fichiers de base de donn�es Microsoft Access")> _
	Public Property DBProvider As clsModule.eDBProvider
		Get
			Return VmDBProvider
		End Get
		Set (VpDBProvider As clsModule.eDBProvider)
			VmDBProvider = VpDBProvider
		End Set
	End Property
End Class