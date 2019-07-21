Imports System.Text
Imports System.Net
Imports System.IO
Public Partial Class frmUpdateContent
    Private VmFormMove As Boolean = False               'Formulaire en déplacement
    Private VmMousePos As Point                         'Position initiale de la souris sur la barre de titre
    Private VmCanClose As Boolean = False               'Formulaire peut être fermé
    Private VmNewContenu As List(Of clsUpdateContent)   'Eléments de mise à jour
    Private VmBusy As Boolean
    Private VmCancel As Boolean
    Private VmAnswered As Boolean
    Private VmPassiveUpdate As mdlConstGlob.ePassiveUpdate = mdlConstGlob.ePassiveUpdate.NotNow
    Public Sub New
        Call Me.InitializeComponent
    End Sub
    Private Function GetStamps As String()
    '----------------------------
    'Récupération des horodatages
    '----------------------------
    Dim VpStamps(0 To 12) As String     '0 = images, 1 = prix, 2 = aut. tournois, 3 = modèles, 4 = textes vf, 5 = rulings, 6 = patch images, 7 = patch trad, 8 = patch sous-types, 9 = patch sous-types vf, 10 = patch multiverse id, 11 = series, 12 = trad
    Dim VpRequest As HttpWebRequest
    Dim VpAnswer As Stream
    Dim VpBuf(0 To 117) As Byte
    DIm VpLength As Integer
        Try
            'Gestion cas 0
            VpStamps(0) = mdlToolbox.GetPictSP
            'Gestion cas 1 à 10
            VpRequest = WebRequest.Create(mdlConstGlob.VgOptions.VgSettings.DownloadServer + CgURL1D)
            VpAnswer = VpRequest.GetResponse.GetResponseStream
            VpAnswer.Read(VpBuf, 0, VpBuf.Length)
            VpLength = If(VpBuf(VpBuf.Length - 1) = 0, 11, 12)  'gestion cr ou cr+lf
            For VpI As Integer = 0 To 9
                VpStamps(VpI + 1) = New ASCIIEncoding().GetString(VpBuf, VpI * VpLength, 10)
            Next VpI
            'Gestion cas 11

            'Gestion cas 12

            Return VpStamps
        Catch
            Call mdlToolbox.ShowWarning(mdlConstGlob.CgDL3b)
            Return Nothing
        End Try
    End Function
    Private Function GetSizes As Integer()
    '------------------------------------
    'Récupération des tailles des patches
    '------------------------------------
    Dim VpSizes(0 To 12) As Integer     '0 = images, 1 = prix, 2 = aut. tournois, 3 = modèles, 4 = textes vf, 5 = rulings, 6 = patch images, 7 = patch trad, 8 = patch sous-types, 9 = patch sous-types vf, 10 = patch multiverse id, 11 = series, 12 = trad
    Dim VpRequest As HttpWebRequest
    Dim VpAnswer As Stream
    Dim VpBuf(0 To 74) As Byte
    DIm VpLength As Integer
        Try
            VpRequest = WebRequest.Create(mdlConstGlob.VgOptions.VgSettings.DownloadServer + CgURL1E)
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
            Call mdlToolbox.ShowWarning(mdlConstGlob.CgDL3b)
            Return Nothing
        End Try
    End Function
    Private Function CompareStamp(VpType As clsUpdateContent.EgMAJContenu, VpStamp As String, VpSize As Integer) As clsUpdateContent
    '-----------------------------------------
    'Comparaison des versions locale / serveur
    '-----------------------------------------
    Dim VpMAJContenu As clsUpdateContent = Nothing
    Dim VpPictFileSize As Long
    Dim VpCur As String
        Select Case VpType
            Case clsUpdateContent.EgMAJContenu.NewAut
                VpMAJContenu = New clsUpdateContent(VpType, VgOptions.VgSettings.LastUpdateAut, VpStamp, VpSize)
            Case clsUpdateContent.EgMAJContenu.PatchPict
                If File.Exists(VgOptions.VgSettings.PicturesFile) Then
                    VpMAJContenu = New clsUpdateContent(VpType, VgOptions.VgSettings.LastUpdatePictPatch, VpStamp, VpSize)
                End If
            Case clsUpdateContent.EgMAJContenu.NewSimu
                VpMAJContenu = New clsUpdateContent(VpType, VgOptions.VgSettings.LastUpdateSimu, VpStamp, VpSize)
            Case clsUpdateContent.EgMAJContenu.PatchTrad
                VpMAJContenu = New clsUpdateContent(VpType, VgOptions.VgSettings.LastUpdateTradPatch, VpStamp, VpSize)
            Case clsUpdateContent.EgMAJContenu.PatchSubTypes
                VpMAJContenu = New clsUpdateContent(VpType, VgOptions.VgSettings.LastUpdateSubTypesPatch, VpStamp, VpSize)
            Case clsUpdateContent.EgMAJContenu.PatchSubTypesVF
                VpMAJContenu = New clsUpdateContent(VpType, VgOptions.VgSettings.LastUpdateSubTypesVFPatch, VpStamp, VpSize)
            Case clsUpdateContent.EgMAJContenu.PatchMultiverseId
                VpMAJContenu = New clsUpdateContent(VpType, VgOptions.VgSettings.LastUpdateMultiverseIdPatch, VpStamp, VpSize)
            Case clsUpdateContent.EgMAJContenu.NewTxtVF
                VpMAJContenu = New clsUpdateContent(VpType, VgOptions.VgSettings.LastUpdateTxtVF, VpStamp, VpSize)
            Case clsUpdateContent.EgMAJContenu.NewRulings
                VpMAJContenu = New clsUpdateContent(VpType, VgOptions.VgSettings.LastUpdateRulings, VpStamp, VpSize)
            Case clsUpdateContent.EgMAJContenu.NewPrix
                VpMAJContenu = New clsUpdateContent(VpType, mdlToolbox.GetLastPricesDate.ToShortDateString, VpStamp, VpSize)
            Case clsUpdateContent.EgMAJContenu.NewPict
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
                        VpMAJContenu = New clsUpdateContent(VpType, VpCur, VpStamp, VpSize)
                    End If
                End If
            Case clsUpdateContent.EgMAJContenu.NewSerie
            Case clsUpdateContent.EgMAJContenu.NewTrad
            Case Else
        End Select
        If VpMAJContenu Is Nothing Then Return Nothing
        If VpMAJContenu.Locale = VpMAJContenu.Serveur Then
            Return Nothing
        Else
            Return VpMAJContenu
        End If
    End Function
    Private Function UpdateItem(VpElement As clsUpdateContent) As Boolean
    '--------------------------------------------------------
    'Procède à la mise à jour de l'élément passé en paramètre
    '--------------------------------------------------------
        Select Case VpElement.TypeContenu
            Case clsUpdateContent.EgMAJContenu.NewAut
                'Appel silencieux pour mise à jour autorisations en tournoi
                Call mdlToolbox.DownloadNow(New Uri(mdlConstGlob.VgOptions.VgSettings.DownloadServer + CgURL15), mdlConstGlob.CgUpAutorisations)
                If File.Exists(Application.StartupPath + mdlConstGlob.CgUpAutorisations) Then
                    Call MainForm.VgMe.UpdateAutorisations(Application.StartupPath + mdlConstGlob.CgUpAutorisations, True)
                    Call mdlToolbox.SecureDelete(Application.StartupPath + mdlConstGlob.CgUpAutorisations)
                    VgOptions.VgSettings.LastUpdateAut = VpElement.Serveur
                Else
                    Return False
                End If
            Case clsUpdateContent.EgMAJContenu.PatchPict
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
            Case clsUpdateContent.EgMAJContenu.NewSimu
                'Appel silencieux pour mise à jour modèles et historiques
                Call mdlToolbox.DownloadNow(New Uri(mdlConstGlob.VgOptions.VgSettings.DownloadServer + CgURL3B), mdlConstGlob.CgUpDDBb)
                If File.Exists(Application.StartupPath + mdlConstGlob.CgUpDDBb) Then
                    Call mdlToolbox.DBImport(Application.StartupPath + mdlConstGlob.CgUpDDBb, True)
                    Call mdlToolbox.DBAdaptEncNbr
                    VgOptions.VgSettings.LastUpdateSimu = VpElement.Serveur
                Else
                    Return False
                End If
            Case clsUpdateContent.EgMAJContenu.PatchTrad
                'Appel silencieux pour correctif de titres de cartes
                If MainForm.VgMe.FixFR2 Then
                    VgOptions.VgSettings.LastUpdateTradPatch = VpElement.Serveur
                Else
                    Return False
                End If
            Case clsUpdateContent.EgMAJContenu.PatchSubTypes
                'Appel silencieux pour correctif des sous-types de cartes
                If MainForm.VgMe.FixSubTypes Then
                    VgOptions.VgSettings.LastUpdateSubTypesPatch = VpElement.Serveur
                Else
                    Return False
                End If
            Case clsUpdateContent.EgMAJContenu.PatchSubTypesVF
                'Appel silencieux pour correctif des traductions des sous-types de cartes
                If MainForm.VgMe.FixSubTypesVF Then
                    VgOptions.VgSettings.LastUpdateSubTypesVFPatch = VpElement.Serveur
                Else
                    Return False
                End If
            Case clsUpdateContent.EgMAJContenu.PatchMultiverseId
                'Appel silencieux pour mise à jour des identifiants Multiverse des cartes
                If MainForm.VgMe.FixMultiverse2 Then
                    VgOptions.VgSettings.LastUpdateMultiverseIdPatch = VpElement.Serveur
                Else
                    Return False
                End If
            Case clsUpdateContent.EgMAJContenu.NewTxtVF
                'Appel silencieux pour mise à jour texte des cartes en français
                Call mdlToolbox.DownloadNow(New Uri(mdlConstGlob.VgOptions.VgSettings.DownloadServer + CgURL11), mdlConstGlob.CgUpTXTFR)
                If File.Exists(Application.StartupPath + mdlConstGlob.CgUpTXTFR) Then
                    Call MainForm.VgMe.UpdateTxtFR(True)
                    VgOptions.VgSettings.LastUpdateTxtVF = VpElement.Serveur
                Else
                    Return False
                End If
            Case clsUpdateContent.EgMAJContenu.NewRulings
                'Appel silencieux pour mise à jour texte des règles spécifiques
                VmPassiveUpdate = mdlConstGlob.ePassiveUpdate.InProgress
                Call mdlToolbox.DownloadUpdate(New Uri(mdlConstGlob.VgOptions.VgSettings.DownloadServer + CgURL19), mdlConstGlob.CgUpRulings)
                While VmPassiveUpdate = mdlConstGlob.ePassiveUpdate.InProgress
                    Application.DoEvents
                End While
                If VmPassiveUpdate = mdlConstGlob.ePassiveUpdate.Failed Then
                    Return False
                Else
                    VgOptions.VgSettings.LastUpdateRulings = VpElement.Serveur
                End If
                VmPassiveUpdate = mdlConstGlob.ePassiveUpdate.NotNow
            Case clsUpdateContent.EgMAJContenu.NewPrix
                'Appel silencieux pour mise à jour prix
                Call mdlToolbox.DownloadNow(New Uri(mdlConstGlob.VgOptions.VgSettings.DownloadServer + CgURL9), mdlConstGlob.CgUpPrices)
                If File.Exists(Application.StartupPath + mdlConstGlob.CgUpPrices) Then
                    Call MainForm.VgMe.UpdatePrices(Application.StartupPath + mdlConstGlob.CgUpPrices, False, True)
                    Call mdlToolbox.SecureDelete(Application.StartupPath + mdlConstGlob.CgUpPrices)
                Else
                    Return False
                End If
            Case clsUpdateContent.EgMAJContenu.NewPict
                'Appel silencieux (multiple) pour mise(s) à jour d'images
                If MainForm.VgMe.IsInImgDL Then Return False
                For VpI As Integer = 1 + CInt(VpElement.Locale.Replace("SP", "")) To CInt(VpElement.Serveur.Replace("SP", ""))
                    VmPassiveUpdate = mdlConstGlob.ePassiveUpdate.InProgress
                    'Téléchargement du fichier accompagnateur
                    Call mdlToolbox.DownloadNow(New Uri(mdlConstGlob.VgOptions.VgSettings.DownloadServer + CgURL10 + "SP" + VpI.ToString + mdlConstGlob.CgPicLogExt), mdlConstGlob.CgUpPic + mdlConstGlob.CgPicLogExt)
                    Application.DoEvents
                    'Téléchargement du service pack d'images
                    MainForm.VgMe.IsInImgDL = True
                    Call mdlToolbox.DownloadUpdate(New Uri(mdlConstGlob.VgOptions.VgSettings.DownloadServer + CgURL10 + "SP" + VpI.ToString + mdlConstGlob.CgPicUpExt), mdlConstGlob.CgUpPic + mdlConstGlob.CgPicUpExt, , True)
                    While VmPassiveUpdate = mdlConstGlob.ePassiveUpdate.InProgress
                        Application.DoEvents
                    End While
                    If VmPassiveUpdate = mdlConstGlob.ePassiveUpdate.Failed Then    'dès que l'application d'un service pack a merdé, il faut sortir sans poursuivre avec les suivants
                        Return False
                    End If
                Next VpI
                VmPassiveUpdate = mdlConstGlob.ePassiveUpdate.NotNow
            Case clsUpdateContent.EgMAJContenu.NewSerie

            Case clsUpdateContent.EgMAJContenu.NewTrad

            Case Else
        End Select
        Return True
    End Function
    Public Function CheckForContenu(ByRef VpNewContenu As List(Of clsUpdateContent)) As Boolean
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
            For Each VpMAJContenu As clsUpdateContent In VpNewContenu
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
            Call mdlToolbox.ShowInformation("Opération terminée.")
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
                    VmCancel = ( mdlToolbox.ShowQuestion("Voulez-vous annuler les mises à jour de contenu ?" + vbCrLf + "L'annulation aura lieu à la fin de l'opération en cours...") = System.Windows.Forms.DialogResult.Yes )
                    VmAnswered = True
                End If
            Else
                Me.Close
            End If
        End If
    End Sub
    Sub FrmUpdateContenuActivated(sender As Object, e As EventArgs)
    Dim VpNewContenu As New List(Of clsUpdateContent)
    Dim VpItem As ListViewItem
        If Me.chklstContenus.Items.Count = 0 Then
            Me.chklstContenus.Items.Add("Vérification en cours...")
            Application.DoEvents
            If Me.CheckForContenu(VpNewContenu) Then
                Me.chklstContenus.Items.Clear
                Me.chklstContenus.CheckBoxes = True
                For Each VpMAJContenu As clsUpdateContent In VpNewContenu
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
    Public Property PassiveUpdate As mdlConstGlob.ePassiveUpdate
        Get
            Return VmPassiveUpdate
        End Get
        Set (VpPassiveUpdate As mdlConstGlob.ePassiveUpdate)
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
