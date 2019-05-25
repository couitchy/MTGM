﻿'------------------------------------------------------
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
'| - gestion des règles spécifiques		   18/02/2012 |
'| - gestion formats dates internationaux  04/01/2015 |
'| - gestion mise à jour des sous-types    16/01/2015 |
'| - gestion mise à jour des multiverse id 10/07/2016 |
'------------------------------------------------------
Imports System.Text
Imports System.Net
Imports System.IO
imports System.Globalization
Public Partial Class frmUpdateContenu
	Private VmFormMove As Boolean = False			'Formulaire en déplacement
	Private VmMousePos As Point						'Position initiale de la souris sur la barre de titre
	Private VmCanClose As Boolean = False   		'Formulaire peut être fermé
	Private VmNewContenu As List(Of clsMAJContenu)	'Eléments de mise à jour
	Private VmBusy As Boolean
	Private VmCancel As Boolean
	Private VmAnswered As Boolean
	Private VmPassiveUpdate As EgPassiveUpdate = EgPassiveUpdate.NotNow
	Public Enum EgPassiveUpdate
		NotNow = 0
		InProgress
		Done
		Failed
	End Enum
	Public Sub New()
		Me.InitializeComponent()
	End Sub
	Private Function GetStamps As String()
	'----------------------------
	'Récupération des horodatages
	'----------------------------
	Dim VpStamps(0 To 12) As String		'0 = images, 1 = prix, 2 = aut. tournois, 3 = modèles, 4 = textes vf, 5 = rulings, 6 = patch images, 7 = patch trad, 8 = patch sous-types, 9 = patch sous-types vf, 10 = patch multiverse id, 11 = series, 12 = trad
	Dim VpRequest As HttpWebRequest
	Dim VpAnswer As Stream
	Dim VpBuf(0 To 117) As Byte
	DIm VpLength As Integer
		Try
			'Gestion cas 0
			VpStamps(0) = clsModule.GetPictSP
			'Gestion cas 1 à 10
			VpRequest = WebRequest.Create(clsModule.VgOptions.VgSettings.DownloadServer + CgURL1D)
			VpAnswer = VpRequest.GetResponse.GetResponseStream
			VpAnswer.Read(VpBuf, 0, VpBuf.Length)
			VpLength = If(VpBuf(VpBuf.Length - 1) = 0, 11, 12)	'gestion cr ou cr+lf
			For VpI As Integer = 0 To 9
				VpStamps(VpI + 1) = New ASCIIEncoding().GetString(VpBuf, VpI * VpLength, 10)
			Next VpI
			'Gestion cas 11

			'Gestion cas 12

			Return VpStamps
		Catch
			Call clsModule.ShowWarning(clsModule.CgDL3b)
			Return Nothing
		End Try
	End Function
	Private Function GetSizes As Integer()
	'------------------------------------
	'Récupération des tailles des patches
	'------------------------------------
	Dim VpSizes(0 To 12) As Integer		'0 = images, 1 = prix, 2 = aut. tournois, 3 = modèles, 4 = textes vf, 5 = rulings, 6 = patch images, 7 = patch trad, 8 = patch sous-types, 9 = patch sous-types vf, 10 = patch multiverse id, 11 = series, 12 = trad
	Dim VpRequest As HttpWebRequest
	Dim VpAnswer As Stream
	Dim VpBuf(0 To 74) As Byte
	DIm VpLength As Integer
		Try
			VpRequest = WebRequest.Create(clsModule.VgOptions.VgSettings.DownloadServer + CgURL1E)
			VpAnswer = VpRequest.GetResponse.GetResponseStream
			VpAnswer.Read(VpBuf, 0, VpBuf.Length)
			VpLength = If(VpBuf(VpBuf.Length - 1) = 0, 6, 7)
			'Gestion cas 0 à 10
			For VpI As Integer = 0 To 10
				VpSizes(VpI) = Val(New ASCIIEncoding().GetString(VpBuf, VpI * VpLength, 5))
			Next VpI
			'Gestion cas 11

			'Gestion cas 12

			Return VpSizes
		Catch
			Call clsModule.ShowWarning(clsModule.CgDL3b)
			Return Nothing
		End Try
	End Function
	Private Function CompareStamp(VpType As clsMAJContenu.EgMAJContenu, VpStamp As String, VpSize As Integer) As clsMAJContenu
	'-----------------------------------------
	'Comparaison des versions locale / serveur
	'-----------------------------------------
	Dim VpMAJContenu As clsMAJContenu = Nothing
	Dim VpPictFileSize As Long
	Dim VpCur As String
		Select Case VpType
			Case clsMAJContenu.EgMAJContenu.NewAut
				VpMAJContenu = New clsMAJContenu(VpType, VgOptions.VgSettings.LastUpdateAut, VpStamp, VpSize)
			Case clsMAJContenu.EgMAJContenu.PatchPict
				If File.Exists(VgOptions.VgSettings.PicturesFile) Then
					VpMAJContenu = New clsMAJContenu(VpType, VgOptions.VgSettings.LastUpdatePictPatch, VpStamp, VpSize)
				End If
			Case clsMAJContenu.EgMAJContenu.NewSimu
				VpMAJContenu = New clsMAJContenu(VpType, VgOptions.VgSettings.LastUpdateSimu, VpStamp, VpSize)
			Case clsMAJContenu.EgMAJContenu.PatchTrad
				VpMAJContenu = New clsMAJContenu(VpType, VgOptions.VgSettings.LastUpdateTradPatch, VpStamp, VpSize)
			Case clsMAJContenu.EgMAJContenu.PatchSubTypes
				VpMAJContenu = New clsMAJContenu(VpType, VgOptions.VgSettings.LastUpdateSubTypesPatch, VpStamp, VpSize)
			Case clsMAJContenu.EgMAJContenu.PatchSubTypesVF
				VpMAJContenu = New clsMAJContenu(VpType, VgOptions.VgSettings.LastUpdateSubTypesVFPatch, VpStamp, VpSize)
			Case clsMAJContenu.EgMAJContenu.PatchMultiverseId
				VpMAJContenu = New clsMAJContenu(VpType, VgOptions.VgSettings.LastUpdateMultiverseIdPatch, VpStamp, VpSize)
			Case clsMAJContenu.EgMAJContenu.NewTxtVF
				VpMAJContenu = New clsMAJContenu(VpType, VgOptions.VgSettings.LastUpdateTxtVF, VpStamp, VpSize)
			Case clsMAJContenu.EgMAJContenu.NewRulings
				VpMAJContenu = New clsMAJContenu(VpType, VgOptions.VgSettings.LastUpdateRulings, VpStamp, VpSize)
			Case clsMAJContenu.EgMAJContenu.NewPrix
				VpMAJContenu = New clsMAJContenu(VpType, clsModule.GetLastPricesDate.ToShortDateString, VpStamp, VpSize)
			Case clsMAJContenu.EgMAJContenu.NewPict
				If File.Exists(VgOptions.VgSettings.PicturesFile) Then
					VpPictFileSize = (New FileInfo(VgOptions.VgSettings.PicturesFile)).Length
					If VpPictFileSize <> 0 AndAlso VpStamp.Contains(VpPictFileSize.ToString) Then
						If VpStamp.IndexOf(VpPictFileSize.ToString) > 1 Then
							VpCur = VpStamp.Substring(0, VpStamp.IndexOf(VpPictFileSize.ToString) - 3)
							VpCur = VpCur.Substring(VpCur.LastIndexOf("#") + 1)
						Else
							VpCur = "SP0"
						End If
						VpStamp = VpStamp.Substring(VpStamp.LastIndexOf("SP"))
						VpStamp = VpStamp.Substring(0, VpStamp.IndexOf("#"))
						VpMAJContenu = New clsMAJContenu(VpType, VpCur, VpStamp, VpSize)
					End If
				End If
			Case clsMAJContenu.EgMAJContenu.NewSerie
			Case clsMAJContenu.EgMAJContenu.NewTrad
			Case Else
		End Select
		If VpMAJContenu Is Nothing Then Return Nothing
		If VpMAJContenu.Locale = VpMAJContenu.Serveur Then
			Return Nothing
		Else
			Return VpMAJContenu
		End If
	End Function
	Private Function UpdateItem(VpElement As clsMAJContenu) As Boolean
	'--------------------------------------------------------
	'Procède à la mise à jour de l'élément passé en paramètre
	'--------------------------------------------------------
		Select Case VpElement.TypeContenu
			Case clsMAJContenu.EgMAJContenu.NewAut
				'Appel silencieux pour mise à jour autorisations en tournoi
				Call clsModule.DownloadNow(New Uri(clsModule.VgOptions.VgSettings.DownloadServer + CgURL15), clsModule.CgUpAutorisations)
				If File.Exists(Application.StartupPath + clsModule.CgUpAutorisations) Then
					Call MainForm.VgMe.UpdateAutorisations(Application.StartupPath + clsModule.CgUpAutorisations, True)
					Call clsModule.SecureDelete(Application.StartupPath + clsModule.CgUpAutorisations)
					VgOptions.VgSettings.LastUpdateAut = VpElement.Serveur
				Else
					Return False
				End If
			Case clsMAJContenu.EgMAJContenu.PatchPict
				'Appel silencieux pour correctif d'images
				If File.Exists(VgOptions.VgSettings.PicturesFile) And Not MainForm.VgMe.IsInImgDL Then
					If MainForm.VgMe.FixPictures Then
						VgOptions.VgSettings.LastUpdatePictPatch = VpElement.Serveur
					Else
						Return False
					End If
				Else
					Return False
				End If
			Case clsMAJContenu.EgMAJContenu.NewSimu
				'Appel silencieux pour mise à jour modèles et historiques
				Call clsModule.DownloadNow(New Uri(clsModule.VgOptions.VgSettings.DownloadServer + CgURL3B), clsModule.CgUpDDBb)
				If File.Exists(Application.StartupPath + clsModule.CgUpDDBb) Then
					Call clsModule.DBImport(Application.StartupPath + clsModule.CgUpDDBb, True)
					Call clsModule.DBAdaptEncNbr
					VgOptions.VgSettings.LastUpdateSimu = VpElement.Serveur
				Else
					Return False
				End If
			Case clsMAJContenu.EgMAJContenu.PatchTrad
				'Appel silencieux pour correctif de titres de cartes
				If MainForm.VgMe.FixFR2 Then
					VgOptions.VgSettings.LastUpdateTradPatch = VpElement.Serveur
				Else
					Return False
				End If
			Case clsMAJContenu.EgMAJContenu.PatchSubTypes
				'Appel silencieux pour correctif des sous-types de cartes
				If MainForm.VgMe.FixSubTypes Then
					VgOptions.VgSettings.LastUpdateSubTypesPatch = VpElement.Serveur
				Else
					Return False
				End If
			Case clsMAJContenu.EgMAJContenu.PatchSubTypesVF
				'Appel silencieux pour correctif des traductions des sous-types de cartes
				If MainForm.VgMe.FixSubTypesVF Then
					VgOptions.VgSettings.LastUpdateSubTypesVFPatch = VpElement.Serveur
				Else
					Return False
				End If
			Case clsMAJContenu.EgMAJContenu.PatchMultiverseId
				'Appel silencieux pour mise à jour des identifiants Multiverse des cartes
				If MainForm.VgMe.FixMultiverse2 Then
					VgOptions.VgSettings.LastUpdateMultiverseIdPatch = VpElement.Serveur
				Else
					Return False
				End If
			Case clsMAJContenu.EgMAJContenu.NewTxtVF
				'Appel silencieux pour mise à jour texte des cartes en français
				Call clsModule.DownloadNow(New Uri(clsModule.VgOptions.VgSettings.DownloadServer + CgURL11), clsModule.CgUpTXTFR)
				If File.Exists(Application.StartupPath + clsModule.CgUpTXTFR) Then
					Call MainForm.VgMe.UpdateTxtFR(True)
					VgOptions.VgSettings.LastUpdateTxtVF = VpElement.Serveur
				Else
					Return False
				End If
			Case clsMAJContenu.EgMAJContenu.NewRulings
				'Appel silencieux pour mise à jour texte des règles spécifiques
				VmPassiveUpdate = EgPassiveUpdate.InProgress
				Call clsModule.DownloadUpdate(New Uri(clsModule.VgOptions.VgSettings.DownloadServer + CgURL19), clsModule.CgUpRulings)
				While VmPassiveUpdate = EgPassiveUpdate.InProgress
					Application.DoEvents
				End While
				If VmPassiveUpdate = EgPassiveUpdate.Failed Then
					Return False
				Else
					VgOptions.VgSettings.LastUpdateRulings = VpElement.Serveur
				End If
				VmPassiveUpdate = EgPassiveUpdate.NotNow
			Case clsMAJContenu.EgMAJContenu.NewPrix
				'Appel silencieux pour mise à jour prix
				Call clsModule.DownloadNow(New Uri(clsModule.VgOptions.VgSettings.DownloadServer + CgURL9), clsModule.CgUpPrices)
				If File.Exists(Application.StartupPath + clsModule.CgUpPrices) Then
					Call MainForm.VgMe.UpdatePrices(Application.StartupPath + clsModule.CgUpPrices, False, True)
					Call clsModule.SecureDelete(Application.StartupPath + clsModule.CgUpPrices)
				Else
					Return False
				End If
			Case clsMAJContenu.EgMAJContenu.NewPict
				'Appel silencieux (multiple) pour mise(s) à jour d'images
				If MainForm.VgMe.IsInImgDL Then Return False
				For VpI As Integer = 1 + CInt(VpElement.Locale.Replace("SP", "")) To CInt(VpElement.Serveur.Replace("SP", ""))
					VmPassiveUpdate = EgPassiveUpdate.InProgress
					'Téléchargement du fichier accompagnateur
					Call clsModule.DownloadNow(New Uri(clsModule.VgOptions.VgSettings.DownloadServer + CgURL10 + "SP" + VpI.ToString + clsModule.CgPicLogExt), clsModule.CgUpPic + clsModule.CgPicLogExt)
					Application.DoEvents
					'Téléchargement du service pack d'images
					MainForm.VgMe.IsInImgDL = True
					Call clsModule.DownloadUpdate(New Uri(clsModule.VgOptions.VgSettings.DownloadServer + CgURL10 + "SP" + VpI.ToString + clsModule.CgPicUpExt), clsModule.CgUpPic + clsModule.CgPicUpExt, , True)
					While VmPassiveUpdate = EgPassiveUpdate.InProgress
						Application.DoEvents
					End While
					If VmPassiveUpdate = EgPassiveUpdate.Failed Then	'dès que l'application d'un service pack a merdé, il faut sortir sans poursuivre avec les suivants
						Return False
					End If
				Next VpI
				VmPassiveUpdate = EgPassiveUpdate.NotNow
			Case clsMAJContenu.EgMAJContenu.NewSerie

			Case clsMAJContenu.EgMAJContenu.NewTrad

			Case Else
		End Select
		Return True
	End Function
	Public Function CheckForContenu(ByRef VpNewContenu As List(Of clsMAJContenu)) As Boolean
	'----------------------------------------------------------------------
	'Regarde s'il existe des mises à jour disponibles pour :
	'- les prix (bdd)
	'- les images (taille fichier)
	'- les autorisations de tournoi (.xml)
	'- les modèles et historiques (.xml)
	'- le texte des cartes en vf (.xml)
	'- les régles spécifiques des cartes en vo (.xml)
	'- les éditions (liste serveur | PAS ENCORE GERE)
	'- le titre des cartes en vf (.xml)
	'- les corrections sur les images (.xml)
	'- les corrections sur les titres des cartes (.xml)
	'- les corrections sur les sous-types des cartes (.xml)
	'- les corrections sur les traductions des sous-types des cartes (.xml)
	'----------------------------------------------------------------------
	Dim VpStamps() As String = Me.GetStamps
	Dim VpSizes() As Integer = Me.GetSizes
		If (Not VpStamps Is Nothing) And (Not VpSizes Is Nothing) Then
			For VpI As Integer = 0 To VpStamps.Length - 1
				VpNewContenu.Add(Me.CompareStamp(VpI, VpStamps(VpI), VpSizes(VpI)))
			Next VpI
			For Each VpMAJContenu As clsMAJContenu In VpNewContenu
				If Not VpMAJContenu Is Nothing Then
					Return True
				End If
			Next VpMAJContenu
		End If
		Return False
	End Function
	Sub CmdUpdateClick(sender As Object, e As EventArgs)
		Me.cmdUpdate.Enabled = False
		Me.IsBusy = True
		VmCancel = False
		VmAnswered = False
		For Each VpItem As ListViewItem In Me.chklstContenus.CheckedItems
			VpItem.ForeColor = Color.Blue
			Application.DoEvents
			If Me.UpdateItem(VmNewContenu.Item(VpItem.Tag)) Then
				VpItem.ForeColor = Color.Green
				VpItem.SubItems.Item(1).Text = VpItem.SubItems.Item(2).Text
			Else
				VpItem.ForeColor = Color.Red
			End If
			Application.DoEvents
			If VmCancel Then
				Exit For
			Else
				VmAnswered = False
			End If
		Next VpItem
		If VmCancel Then
			Me.IsBusy = False
			Me.Close
		Else
			Call VgOptions.SaveSettings
			Me.IsBusy = False
			Call clsModule.ShowInformation("Opération terminée.")
		End If
	End Sub
	Sub CbarUpdateMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
		VmFormMove = True
		VmCanClose = True
		VmMousePos = New Point(e.X, e.Y)
	End Sub
	Sub CbarUpdateMouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
		If VmFormMove Then
			Me.Location = New Point(MousePosition.X - VmMousePos.X, MousePosition.Y - VmMousePos.Y)
		End If
	End Sub
	Sub CbarUpdateMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs)
		VmFormMove = False
	End Sub
	Sub CbarUpdateVisibleChanged(ByVal sender As Object, ByVal e As EventArgs)
		If VmCanClose Then
			If Me.IsBusy Then
				Me.cbarUpdate.Show
				If Not VmAnswered Then
					VmCancel = ( clsModule.ShowQuestion("Voulez-vous annuler les mises à jour de contenu ?" + vbCrLf + "L'annulation aura lieu à la fin de l'opération en cours...") = System.Windows.Forms.DialogResult.Yes )
					VmAnswered = True
				End If
			Else
				Me.Close
			End If
		End If
	End Sub
	Sub FrmUpdateContenuActivated(sender As Object, e As EventArgs)
	Dim VpNewContenu As New List(Of clsMAJContenu)
	Dim VpItem As ListViewItem
		If Me.chklstContenus.Items.Count = 0 Then
			Me.chklstContenus.Items.Add("Vérification en cours...")
			Application.DoEvents
			If Me.CheckForContenu(VpNewContenu) Then
				Me.chklstContenus.Items.Clear
				Me.chklstContenus.CheckBoxes = True
				For Each VpMAJContenu As clsMAJContenu In VpNewContenu
					If Not VpMAJContenu Is Nothing Then
						VpItem = New ListViewItem(VpMAJContenu.TypeContenuStr)
						VpItem.SubItems.Add(VpMAJContenu.Locale)
						VpItem.SubItems.Add(VpMAJContenu.Serveur)
						VpItem.SubItems.Add(VpMAJContenu.SizeDL.ToString + " Ko")
						VpItem.Checked = True
						VpItem.Tag = VpNewContenu.IndexOf(VpMAJContenu)
						Me.chklstContenus.Items.Add(VpItem)
					End If
				Next VpMAJContenu
			Else
				Me.chklstContenus.Items.Clear
			End If
			VmNewContenu = VpNewContenu
			'Astuce horrible pour contourner un bug de mise à l'échelle automatique en fonction de la densité de pixels
			If Me.CreateGraphics().DpiX <> 96 Then
				Me.grpUpdate.Dock = DockStyle.None
				Me.chklstContenus.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)
			End If
		End If
	End Sub
	Public Property PassiveUpdate As EgPassiveUpdate
		Get
			Return VmPassiveUpdate
		End Get
		Set (VpPassiveUpdate As EgPassiveUpdate)
			VmPassiveUpdate = VpPassiveUpdate
		End Set
	End Property
	Public Property IsBusy As Boolean
		Get
			Return VmBusy
		End Get
		Set (VpBusy As Boolean)
			VmBusy = VpBusy
			Me.prgWait.Style = If(VpBusy, ProgressBarStyle.Marquee, ProgressBarStyle.Blocks)
		End Set
	End Property
End Class
Public Class clsMAJContenu
	Private VmType As EgMAJContenu
	Private VmLocale As String
	Private VmServeur As String
	Private VmSize As Integer
	Public Enum EgMAJContenu
		NewPict = 0
		NewPrix
		NewAut
		NewSimu
		NewTxtVF
		NewRulings
		PatchPict
		PatchTrad
		PatchSubTypes
		PatchSubTypesVF
		PatchMultiverseId
		NewSerie
		NewTrad
	End Enum
	Public Sub New(VpType As EgMAJContenu, VpLocale As String, VpServeur As String, VpSize As Integer)
	Dim VpLocaleDate As Date
	Dim VpServeurDate As Date
		VmType = VpType
		'Si on reconnait une date, il faut passer par une double conversion pour s'affranchir des problèmes de format
		If Date.TryParseExact(VpLocale, "dd/MM/yyyy", New CultureInfo("fr-FR"), DateTimeStyles.None, VpLocaleDate) AndAlso Date.TryParseExact(VpServeur, "dd/MM/yyyy", New CultureInfo("fr-FR"), DateTimeStyles.None, VpServeurDate) Then
			VmLocale = VpLocaleDate.ToShortDateString
			VmServeur = VpServeurDate.ToShortDateString
		ElseIf Date.TryParse(VpLocale, VpLocaleDate) AndAlso Date.TryParseExact(VpServeur, "dd/MM/yyyy", New CultureInfo("fr-FR"), DateTimeStyles.None, VpServeurDate) Then
			VmLocale = VpLocaleDate.ToShortDateString
			VmServeur = VpServeurDate.ToShortDateString
		ElseIf VpLocale = "" AndAlso Date.TryParseExact(VpServeur, "dd/MM/yyyy", New CultureInfo("fr-FR"), DateTimeStyles.None, VpServeurDate) Then
			VmLocale = VpLocale
			VmServeur = VpServeurDate.ToShortDateString
		Else
			VmLocale = VpLocale
			VmServeur = VpServeur
		End If
		VmSize = VpSize
	End Sub
	Public ReadOnly Property TypeContenu As EgMAJContenu
		Get
			Return VmType
		End Get
	End Property
	Public ReadOnly Property TypeContenuStr As String
		Get
			Select Case VmType
				Case clsMAJContenu.EgMAJContenu.NewPrix
					Return "Mise à jour des prix"
				Case clsMAJContenu.EgMAJContenu.NewAut
					Return "Mise à jour des autorisations tournois"
				Case clsMAJContenu.EgMAJContenu.NewSimu
					Return "Mise à jour des modèles et/ou historiques"
				Case clsMAJContenu.EgMAJContenu.NewTxtVF
					Return "Mise à jour des textes des cartes en français"
				Case clsMAJContenu.EgMAJContenu.NewRulings
					Return "Mise à jour des règles spécifiques des cartes"
				Case clsMAJContenu.EgMAJContenu.PatchPict
					Return "Correctif d'images de cartes"
				Case clsMAJContenu.EgMAJContenu.PatchTrad
					Return "Correctif des libellés de cartes en français"
				Case clsMAJContenu.EgMAJContenu.PatchSubTypes
					Return "Correctif des sous-types de cartes"
				Case clsMAJContenu.EgMAJContenu.PatchSubTypesVF
					Return "Correctif des traductions des sous-types"
				Case clsMAJContenu.EgMAJContenu.PatchMultiverseId
					Return "Mise à jour des identifiants Multiverse"
				Case clsMAJContenu.EgMAJContenu.NewPict
					Return "Service pack d'images de cartes"
				Case clsMAJContenu.EgMAJContenu.NewSerie
					Return "Nouvelle édition Magic The Gathering"
				Case clsMAJContenu.EgMAJContenu.NewTrad
					Return "Mise à jour des libellés de cartes en français"
				Case Else
					Return ""
			End Select
		End Get
	End Property
	Public ReadOnly Property Locale As String
		Get
			If VmLocale = "" Then
				Return "N/C"
			Else
				Return VmLocale
			End If
		End Get
	End Property
	Public ReadOnly Property Serveur As String
		Get
			Return VmServeur
		End Get
	End Property
	Public ReadOnly Property SizeDL As Integer
		Get
			Return VmSize
		End Get
	End Property
End Class