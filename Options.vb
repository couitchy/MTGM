'------------------------------------------------------
'| Projet         |  Magic The Gathering Manager      |
'| Contexte       |  		Perso                     |
'| Date           |                        30/03/2008 |
'| Release 1      |                        12/04/2008 |
'| Release 2      |                        30/08/2008 |
'| Release 3      |                        08/11/2008 |
'| Release 4      |                        29/08/2009 |
'| Auteur         |                          Couitchy |
'|----------------------------------------------------|
'| Modifications :                                    |
'------------------------------------------------------
#Region "Importations"
Imports System.Data
Imports System.Data.OleDb
Imports System.ComponentModel
Imports System.Reflection
Imports System.Text
#End Region
Public Partial Class Options	
	Public VgSettings As New clsSettings
	Private VmIsInitializing As Boolean
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
	'------------------------------------------------------------------------------------------------------------
	'Sauvegarde les propri�t�s actuelles du PropertyGrid dans le fichier INI avant de fermer la bo�te de dialogue
	'------------------------------------------------------------------------------------------------------------
	Dim VpSettingsType As Type = GetType(clsSettings)
		'Parcourt les membres de la classe clsSettings
		For Each VpProperty As MemberInfo In VpSettingsType.GetMembers				
			'Si on tombe sur une propri�t�
			If VpProperty.MemberType = MemberTypes.Property Then
				'On la sauvegarde dans le fichier INI
				Call clsModule.WritePrivateProfileString("Properties", VpProperty.Name, VpSettingsType.InvokeMember(VpProperty.Name, BindingFlags.GetProperty, Nothing, VgSettings, Nothing).ToString, Application.StartupPath + clsModule.CgINIFile)					
			End If
		Next VpProperty
	End Sub
	Public Sub LoadSettings
	'------------------------------------------------------------------------------------------------------------------
	'Restaure les propri�t�s sauvegard�es du PropertyGrid � partir du fichier INI � l'ouverture de la bo�te de dialogue
	'------------------------------------------------------------------------------------------------------------------
	Dim VpSettingsType As Type = GetType(clsSettings)
	Dim VpStr As New StringBuilder(512)		
		VmIsInitializing = True
		For Each VpProperty As MemberInfo In VpSettingsType.GetMembers
			If VpProperty.MemberType = MemberTypes.Property Then
				Call clsModule.GetPrivateProfileString("Properties", VpProperty.Name, "", VpStr, VpStr.Capacity, Application.StartupPath + clsModule.CgINIFile)
				If VpStr.ToString <> "" Then
					'Attention, les propri�t�s ne sont pas toutes du m�me type, il faut une fonction d'adaptation
					VpSettingsType.InvokeMember(VpProperty.Name, BindingFlags.SetProperty, Nothing, VgSettings, New Object() {clsModule.Matching(VpStr.ToString)})
				End If
			End If
		Next VpProperty	
		VmIsInitializing = False
	End Sub		
	Public Function GetDeckName_INI(VpI As Integer) As String
	'-----------------------------------------------------
	'Retourne le nom du deck d'index sp�cifi� en param�tre
	'				/!\ OBSOLETE /!\
	'-----------------------------------------------------
	Dim VpNames() As String
		Try 
			VpNames = VgSettings.NomsJeux.Split("#")
			Return VpNames(VpI - 1)
		Catch
			Return "Jeu n�" + VpI.ToString
		End Try
	End Function
	Public Function GetDeckName(VpI As Integer) As String
	'-----------------------------------------------------
	'Retourne le nom du deck d'index sp�cifi� en param�tre
	'-----------------------------------------------------
		VgDBCommand.CommandText = "Select Last(GameName) From (Select Top " + VpI.ToString + " GameName From MyGamesID);"
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
	Public Function GetDeckCount As Integer
	'----------------------------------------------
	'Retourne le nombre de decks en base de donn�es
	'----------------------------------------------
		VgDBCommand.CommandText = "Select Count(*) From MyGamesID;"		
		Return VgDBCommand.ExecuteScalar
	End Function
	Public ReadOnly Property IsInitializing As Boolean
		Get
			Return VmIsInitializing
		End Get
	End Property
End Class
Public Class clsSettings		
	Private VmNJeux As Integer = 1
	Private VmNomsJeux As String = ""
	Private VmDefaultBase As String = ".\Cartes\Magic DB.mdb"
	Private VmPicturesFile As String = ".\Cartes\Images DB.dat"
	Private VmPreferredSellers As String = ""
	Private VmDefaultCriteria As String = "1#2#7"
	Private VmCheckForUpdate As Boolean = True
	Private VmAutoRefresh As Boolean = True
	Private VmImageMode As PictureBoxSizeMode = PictureBoxSizeMode.CenterImage
	Private VmAutoHideImage As Boolean = False
	Private VmRestoreSize As Boolean = False
	Private VmRestoredWidth As Integer = 757
	Private VmRestoredHeight As Integer = 372
	Private VmFormWindowState As FormWindowState = FormWindowState.Normal	
	<Category("G�n�ral"), Description("M�moriser la taille de la fen�tre principale � la fermeture")> _
	Public Property RestoreSize As Boolean
		Get
			Return VmRestoreSize
		End Get
		Set (VpRestoreSize As Boolean)
			VmRestoreSize = VpRestoreSize
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
	<Browsable(False), Category("G�n�ral"), Description("M�morisation fen�trage")> _
	Public Property RestoredState As FormWindowState
		Get
			Return VmFormWindowState
		End Get
		Set (VpFormWindowState As FormWindowState)
			VmFormWindowState = VpFormWindowState
		End Set
	End Property	
	<Category("G�n�ral"), Description("V�rifier r�guli�rement si une mise � jour existe pour l'application")> _	
	Public Property CheckForUpdate As Boolean
		Get
			Return VmCheckForUpdate
		End Get
		Set(VpCheckForUpdate As Boolean)
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
	<Category("G�n�ral"), Description("Base de donn�es � ouvrir par d�faut"), Editor(GetType(System.Windows.Forms.Design.FileNameEditor), GetType(System.Drawing.Design.UITypeEditor))> _
	Public Property DefaultBase As String
		Get
			Return VmDefaultBase
		End Get
		Set(VpDefaultBase As String)
			VmDefaultBase = VpDefaultBase
		End Set
	End Property		
	<Category("Explorateur"), Description("Crit�res par d�faut (� s�parer par un di�se, Type = 1, Couleur = 2, Edition = 3, Co�t d'invocation = 4 etc.)")> _
	Public Property DefaultCriteria As String
		Get
			Return VmDefaultCriteria
		End Get
		Set(VpDefaultCriteria As String)
			VmDefaultCriteria = VpDefaultCriteria
		End Set
	End Property
	<Category("Explorateur"), Description("Rafra�chir automatiquement l'arborescence apr�s avoir ajout� ou supprim� des cartes.")> _
	Public Property AutoRefresh As Boolean
		Get
			Return VmAutoRefresh
		End Get
		Set(VpAutoRefresh As Boolean)
			VmAutoRefresh = VpAutoRefresh
		End Set
	End Property	
	<Category("Explorateur"), Description("Toujours fermer le panneau image au d�marrage.")> _
	Public Property AutoHideImage As Boolean
		Get
			Return VmAutoHideImage
		End Get
		Set(VpAutoHideImage As Boolean)
			VmAutoHideImage = VpAutoHideImage
		End Set
	End Property	
	<Category("Explorateur"), Description("Fichier des images num�ris�es des cartes."), Editor(GetType(System.Windows.Forms.Design.FileNameEditor), GetType(System.Drawing.Design.UITypeEditor))> _
	Public Property PicturesFile As String
		Get
			Return VmPicturesFile
		End Get
		Set(VpPicturesFile As String)
			VmPicturesFile = VpPicturesFile
		End Set
	End Property	
	<Category("Explorateur"), Description("Mode d'affichage des images des cartes.")> _
	Public Property ImageMode As PictureBoxSizeMode
		Get
			Return VmImageMode
		End Get
		Set(VpImageMode As PictureBoxSizeMode)
			VmImageMode = VpImageMode
		End Set
	End Property	
End Class
