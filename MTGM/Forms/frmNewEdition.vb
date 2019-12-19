Imports System.IO
Imports System.Text
Imports System.Globalization
Public Partial Class frmNewEdition
    Private VmEditionHeader As New clsEditionHeader
    Private VmEncNbr0 As Long = -1
    Private VmBusy As Boolean = False
    Public Sub New
        Call Me.InitializeComponent
        Me.picMagic.Image = Image.FromFile(VgOptions.VgSettings.MagicBack)
    End Sub
    Private Function InsertHeader As Boolean
    '----------------------------------------------------------------
    'Inscrit l'en-t�te de la nouvelle �dition dans la base de donn�es
    '----------------------------------------------------------------
        Try
            With VmEditionHeader
                '(SeriesCD, SeriesCD_MO, SeriesCD_MW, SeriesNM, SeriesNM_MtG, SeriesNM_FR, Null, Null, True, True, Border, Release, Null, TotCards, TotCards, Rare, Uncommon, Common, Land, Foils, Nullx12, Notes)
                VgDBCommand.CommandText = "Insert Into Series (SeriesCD, SeriesCD_MO, SeriesCD_MW, SeriesNM, SeriesNM_MtG, SeriesNM_FR, LegalE, LegalS, Border, Release, TotCards, UqCards, UqRare, UqUncom, UqComm, UqBLand, Foils, Notes) Values ('" + .SeriesCD + "', '" + .SeriesCD_MO + "', '" + .SeriesCD_MW + "', '" + .SeriesNM.Replace("'", "''") + "', '" + .SeriesNM_MtG.Replace("'", "''") + "', '" + .SeriesNM_FR.Replace("'", "''") + "', True, True, " + .GetBorder(.Border) + ", " + mdlToolbox.GetDate(.Release) + ", " + .TotCards.ToString + ", " + .TotCards.ToString + ", " + .Rare.ToString + ", " + .Uncommon.ToString + ", " + .Common.ToString + ", " + .Land.ToString + ", True, '" + .NotesEdition.Replace("'", "''") + "');"
                VgDBCommand.ExecuteNonQuery
            End With
        Catch
            Call mdlToolbox.ShowWarning("Impossible d'ajouter l'en-t�te � la base de donn�es..." + vbCrLf + "Peut-�tre ce nom d'�dition existe-t-il d�j� ? V�rifier les informations saisies et recommencer.")
            Return False
        End Try
        Return True
    End Function
    Private Sub DLResource(VpCount As String, VpFrom As String, VpTo As String)
    '--------------------------------------------------------
    'T�l�charge un fichier n�cessaire � l'ajout d'une �dition
    '--------------------------------------------------------
        Me.lblStatus.Text = "T�l�chargement des donn�es en cours... " + VpCount
        Application.DoEvents
        Call mdlToolbox.DownloadNow(New Uri(VpFrom), VpTo)
    End Sub
    Private Sub FillHeader(VpInfos() As String)
    '-------------------------------------------------------------------------
    'Remplit l'objet en-t�te � partir du tableau de cha�nes pass� en param�tre
    '-------------------------------------------------------------------------
        With VmEditionHeader
            .SeriesCD = VpInfos(1)
            .SeriesNM = VpInfos(2)
            .SeriesNM_MtG = VpInfos(3)
            .SeriesNM_FR = VpInfos(31)
            If VpInfos.Length > 32 Then
                .SeriesCD_MO = VpInfos(32)
                .SeriesCD_MW = VpInfos(33)
            Else
                .SeriesCD_MO = VpInfos(1)
                .SeriesCD_MW = VpInfos(1)
            End If
            .Border = .SetBorder(VpInfos(8))
            .Release = Date.Parse(VpInfos(9), New CultureInfo("fr-FR", True), DateTimeStyles.NoCurrentDateDefault)
            .TotCards = Val(VpInfos(11))
            .Rare = Val(VpInfos(13))
            .Uncommon = Val(VpInfos(14))
            .Common = Val(VpInfos(15))
            .Land = Val(VpInfos(16))
            .NotesEdition = VpInfos(30)
        End With
    End Sub
    Private Sub UpdateSerie(VpInfos() As String, VpSilent As Boolean)
    '------------------------------------------------------------------------------------
    'Met � jour automatiquement l'�dition dont les informations sont pass�es en param�tre
    '------------------------------------------------------------------------------------
    Dim VpChecker As String = "\" + VpInfos(0) + "_checklist_en.txt"
    Dim VpSpoiler As String = "\" + VpInfos(0) + "_spoiler_en.txt"
    Dim VpTrad As String = "\" + VpInfos(0) + "_titles_fr.txt"
    Dim VpDouble As String = "\" + VpInfos(0) + "_doubles_en.txt"
        'T�l�chargement des fichiers n�cessaires
        Call Me.DLResource("(1/5)", mdlConstGlob.VgOptions.VgSettings.DownloadServer + CgURL5 + "_e" + VpInfos(1) + ".png", mdlConstGlob.CgIcons + "\_e" + VpInfos(1) + ".png")
        Call Me.DLResource("(2/5)", mdlConstGlob.VgOptions.VgSettings.DownloadServer + CgURL4 + VpInfos(0) + "_checklist_en.txt", VpChecker)
        Call Me.DLResource("(3/5)", mdlConstGlob.VgOptions.VgSettings.DownloadServer + CgURL4 + VpInfos(0) + "_spoiler_en.txt", VpSpoiler)
        Call Me.DLResource("(4/5)", mdlConstGlob.VgOptions.VgSettings.DownloadServer + CgURL4 + VpInfos(0) + "_titles_fr.txt", VpTrad)
        Call Me.DLResource("(5/5)", mdlConstGlob.VgOptions.VgSettings.DownloadServer + CgURL4 + VpInfos(0) + "_doubles_en.txt", VpDouble)
        'Inscription de l'en-t�te
        Me.lblStatus.Text = "Inscription de l'en-t�te..."
        Application.DoEvents
        Call Me.FillHeader(VpInfos)
        If Me.InsertHeader Then
            Me.lblStatus.Text = "Ajout des cartes..."
            Application.DoEvents
            'La suite est comme en mode manuel
            Me.txtCheckList.Text = Application.StartupPath + VpChecker
            Me.txtSpoilerList.Text = Application.StartupPath + VpSpoiler
            Me.txtSpoilerList.Tag = Application.StartupPath + VpTrad
            Me.txtCheckList.Tag = Application.StartupPath + VpDouble
            Me.chkNewEdition.Tag = VpInfos(2)
            If Not File.Exists(Me.txtCheckList.Text) Or Not File.Exists(Me.txtSpoilerList.Text) Then
                Call mdlToolbox.ShowWarning(mdlConstGlob.CgErr0)
            Else
                Call Me.AddNewEdition(VpSilent)
            End If
        End If
        'Suppression des fichiers temporaires
        Call mdlToolbox.SecureDelete(Application.StartupPath + mdlConstGlob.CgUpSeries)
        Call mdlToolbox.SecureDelete(Application.StartupPath + VpChecker)
        Call mdlToolbox.SecureDelete(Application.StartupPath + VpSpoiler)
        Call mdlToolbox.SecureDelete(Application.StartupPath + VpTrad)
        Call mdlToolbox.SecureDelete(Application.StartupPath + VpDouble)
    End Sub
    Private Sub UpdateSeriesHeaders
    '----------------------------------------------------------------
    'Met � jour les en-t�tes des �ditions (table Series) dans la base
    '----------------------------------------------------------------
    Dim VpSeriesInfos As StreamReader
    Dim VpInfos() As String
    Dim VpLine As String
    Dim VpFullUpdate As Boolean
        Call mdlToolbox.DownloadNow(New Uri(mdlConstGlob.VgOptions.VgSettings.DownloadServer + CgURL12), mdlConstGlob.CgUpSeries)
        If File.Exists(Application.StartupPath + mdlConstGlob.CgUpSeries) Then
            VpFullUpdate = ( mdlToolbox.ShowQuestion("Voulez-vous mettre � jour l'int�gralit� des en-t�tes ?" + vbCrLf + "Cliquez sur 'Non' pour mettre uniquement � jour les codes des �ditions (compatibilit� avec les autres formats de logiciels Magic)") = System.Windows.Forms.DialogResult.Yes )
            VpSeriesInfos = New StreamReader(Application.StartupPath + mdlConstGlob.CgUpSeries)
            Do While Not VpSeriesInfos.EndOfStream
                VpLine = VpSeriesInfos.ReadLine
                If VpLine.Contains("#") Then
                    VpInfos = VpLine.Split("#")
                    Call Me.FillHeader(VpInfos)
                    Try
                        With VmEditionHeader
                            If VpFullUpdate Then
                                VgDBCommand.CommandText = "Update Series Set SeriesCD_MO = '" + .SeriesCD_MO + "', SeriesCD_MW = '" + .SeriesCD_MW + "', SeriesNM = '" + .SeriesNM.Replace("'", "''") + "', SeriesNM_FR = '" + .SeriesNM_FR.Replace("'", "''") + "', SeriesNM_MtG = '" + .SeriesNM_MtG.Replace("'", "''") + "', Border = " + .GetBorder(.Border) + ", Release = " + mdlToolbox.GetDate(.Release) + ", TotCards = " + .TotCards.ToString + ", UqRare = " + .Rare.ToString + ", UqUncom = " + .Uncommon.ToString + ", UqComm = " + .Common.ToString + ", UqBLand = " + .Land.ToString + ", Notes = '" + .NotesEdition.Replace("'", "''") + "' Where SeriesCD = '" + .SeriesCD + "';"
                            Else
                                VgDBCommand.CommandText = "Update Series Set SeriesCD_MO = '" + .SeriesCD_MO + "', SeriesCD_MW = '" + .SeriesCD_MW + "' Where SeriesCD = '" + .SeriesCD + "';"
                            End If
                            VgDBCommand.ExecuteNonQuery
                        End With
                    Catch
                        Call mdlToolbox.ShowWarning("Impossible de mettre � jour l'en-t�te " + VmEditionHeader.SeriesNM + " de la base de donn�es...")
                    End Try
                End If
            Loop
            Call mdlToolbox.SecureDelete(Application.StartupPath + mdlConstGlob.CgUpSeries)
            Call mdlToolbox.ShowInformation("Termin� !" + vbCrLf + "Il est recommand� de relancer l'application...")
        Else
            Call mdlToolbox.ShowWarning(mdlConstGlob.CgDL3b)
        End If
    End Sub
    Private Function QuerySeries As List(Of String)
    '------------------------------------------
    'R�cup�re la liste des �ditions disponibles
    '------------------------------------------
    Dim VpSeriesInfos As StreamReader
    Dim VpInfos() As String
    Dim VpLine As String
    Dim VpAlready As List(Of String)
    Dim VpNew As New List(Of String)
    Dim VpMustAdd As Boolean
    Dim VpWidth As Integer
    Dim VpMaxWidth As Integer = Integer.MinValue
        If Not File.Exists(Application.StartupPath + mdlConstGlob.CgUpSeries) Then
            Call mdlToolbox.DownloadNow(New Uri(mdlConstGlob.VgOptions.VgSettings.DownloadServer + CgURL12), mdlConstGlob.CgUpSeries)
        End If
        If File.Exists(Application.StartupPath + mdlConstGlob.CgUpSeries) Then
            VpAlready = Me.BuildList("Select UCase(SeriesNM) From Series;")
            VpSeriesInfos = New StreamReader(Application.StartupPath + mdlConstGlob.CgUpSeries)
            Do While Not VpSeriesInfos.EndOfStream
                VpLine = VpSeriesInfos.ReadLine
                If VpLine.Contains("#") Then
                    VpInfos = VpLine.Split("#")
                    VpMustAdd = True
                    For Each VpStr As String In VpAlready
                        If VpStr = VpInfos(2).ToUpper Then
                            VpMustAdd = False
                            Exit For
                        End If
                    Next VpStr
                    If VpMustAdd Then
                        VpNew.Add(VpLine)
                        Me.chkNewEditionAuto.Items.Add(VpInfos(2), False)
                        VpWidth = TextRenderer.MeasureText(VpInfos(2), Me.chkNewEditionAuto.Font).Width
                        VpMaxWidth = Math.Max(VpWidth, VpMaxWidth)
                    End If
                End If
            Loop
            If VpMaxWidth > 0 Then
                Me.chkNewEditionAuto.ColumnWidth = VpMaxWidth + 20
            End If
            Me.cmdAutoPrevious.Enabled = True
            Return VpNew
        Else
            Call mdlToolbox.ShowWarning(mdlConstGlob.CgDL3b)
            Me.cmdAutoPrevious.Enabled = True
            Return Nothing
        End If
    End Function
    Private Function AddNewCard(VpCarac() As String) As Boolean
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    'Ajoute � la base de donn�es la carte donc les caract�ristiques sont pass�es en param�tre en ayant au pr�lable compl�t� celles manquantes gr�ce � la checklist
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    Dim VpFile As StreamReader
    Dim VpLine As String
    Dim VpFLine As String
    Dim VpComplement As List(Of String)
    Dim VpMyCard As clsMyCard
    Dim VpSerieCD As String
    Dim VpEncNbr As Long
    Dim VpPrevious As Boolean
    Dim VpType As String
    Dim VpFound As Boolean
    Dim VpIndex As Integer
    Dim VpLen As Integer
        If VpCarac Is Nothing Then Return False
        VpFile = New StreamReader(Me.txtCheckList.Text, Encoding.Default)
        VpComplement = New List(Of String)
        'Code la nouvelle �dition
        VpSerieCD = mdlToolbox.GetSerieCodeFromName(Me.chkNewEdition.Tag)
        'Dernier num�ro d'identification de carte utilis�
        VgDBCommand.CommandText = "Select Max(EncNbr) From Card;"
        VpEncNbr = CLng(VgDBCommand.ExecuteScalar) + 1
        If VmEncNbr0 = -1 Then
            VmEncNbr0 = VpEncNbr
        End If
        'V�rifie si la carte a d�j� �t� �dit�e dans une �dition pr�c�dente
        VgDBCommand.CommandText = "Select LastPrint From Spell Where Title = '" + VpCarac(0).Replace("'", "''") + "';"
        VpPrevious = Not ( VgDBCommand.ExecuteScalar Is Nothing )
        'Parcours de la checklist
        Do While Not VpFile.EndOfStream
            VpLine = VpFile.ReadLine.Trim
            VpFLine = VpLine.Replace(vbTab, " ")
            'S'assure que l'on fait bien une recherche sur le mot entier (et pas une sous-cha�ne) en ayant pr�alablement supprim� les tabulations pour la comparaison
            If VpFLine.Contains(" " + VpCarac(0) + " ") Then
                VpIndex = VpLine.IndexOf(VpCarac(0))
                VpLen = VpCarac(0).Length
                VpFound = True
            '(�vite les erreurs dues au caract�re apostrophe dans des charsets exotiques !)
            ElseIf VpFLine.Contains(" " + VpCarac(0).Replace("'", "") + " ") Then
                VpIndex = VpLine.IndexOf(VpCarac(0).Replace("'", ""))
                VpLen = VpCarac(0).Length - 1
                VpFound = True
            Else
                VpFound = False
            End If
            If VpFound Then
                '� la recherche du nom de l'auteur, de la couleur et de la raret� de la carte (attention, remplacement des tabulations)
                VpLine = VpLine.Substring(VpIndex + VpLen).Replace(vbTab, "  ").Trim
                While VpLine.Contains("  ")
                    VpComplement.Add(VpLine.Substring(0, VpLine.IndexOf("  ")))
                    VpLine = VpLine.Substring(VpLine.IndexOf("  ") + 2)
                End While
                VpComplement.Add(VpLine)
                'On sort d�s qu'on a trouv�, inutile de parcourir tout le fichier
                Exit Do
            End If
        Loop
        VpFile.Close
        If VpComplement.Count = 0 Then
            Call mdlToolbox.ShowWarning("Impossible de trouver la correspondance pour la carte " + VpCarac(0) + "...")
            Return False
        Else
            VpMyCard = New clsMyCard(VpCarac, VpComplement)
            Try
                'Insertion dans la table Card (Series, Title, EncNbr, MultiverseId, 1, Null, Rarity, Type, SubType, 1, 0, Null, 'N', Null, Null, Author, False, 10, 10, CardText, Null)
                VgDBCommand.CommandText = "Insert Into Card (Series, Title, EncNbr, MultiverseId, Versions, CardNbr, Rarity, Type, SubType, myPrice, Price, PriceDate, Condition, FoilPrice, FoilDate, Artist, CenterText, TextSize, FlavorSize, CardText, FlavorText, SpecialDoubleCard) Values ('" + VpSerieCD + "', '" + VpMyCard.Title.Replace("'", "''") + "', " + VpEncNbr.ToString + ", 0, 1, Null, '" + VpMyCard.Rarity + "', '" + VpMyCard.MyType + "', " + VpMycard.MySubType + ", 1, 0, Null, 'N', Null, Null, '" + VpMyCard.Author.Replace("'", "''") + "', False, 10, 10, " + VpMyCard.MyCardText + ", Null, False);"
                VgDBCommand.ExecuteNonQuery
                'Insertion dans la table CardFR o� par d�faut le nom fran�ais sera le nom anglais jusqu'� mise � jour (EncNbr, TitleFR)
                VgDBCommand.CommandText = "Insert Into CardFR Values (" + VpEncNbr.ToString + ", '" + VpMyCard.Title.Replace("'", "''") + "');"
                VgDBCommand.ExecuteNonQuery
                'Insertion (ou mise � jour) dans la table Spell (Title, LastPrint, Color, Null, Null, myCost, Cost, Nullx32)
                If VpPrevious Then
                    VgDBCommand.CommandText = "Update Spell Set LastPrint = '" + VpSerieCD + "', Color = '" + VpMyCard.MyColor + "', myCost = " + VpMyCard.GetMyCost + ", Cost = " + VpMyCard.Cost + " Where Title = '" + VpMyCard.Title.Replace("'", "''") + "';"
                Else
                    VgDBCommand.CommandText = "Insert Into Spell (Title, LastPrint, Color, Goal, Rating, myCost, Cost, CostA, CostB, CostU, CostG, CostR, CostW, CostX, ConvCost, CostLife, CostUnsum, CostSac, CostDisc, Kicker, Buyback, Flashback, Cycling, Madness, Upkeep, UpkeepMana, UpkeepLife, UpkeepSac, UpkeepDisc, Cumulative, Echo, Phasing, Fading, Cantrip, Threshold, Legal1, LegalE, LegalB, Rulings) Values ('" + VpMyCard.Title.Replace("'", "''") + "', '" + VpSerieCD + "', '" + VpMyCard.MyColor + "', Null, Null, " + VpMyCard.GetMyCost + ", " + VpMyCard.Cost + ", Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null);"
                End If
                VgDBCommand.ExecuteNonQuery
                If Not VpPrevious Then
                    VpType = VpMyCard.MyType
                    'Si c'est une nouvelle cr�ature (ou cr�ature-artefact ou arpenteur), insertion dans la table Creature (Title, Power, Tough, Nullx37)
                    If VpType = "P" Or VpType = "U" Or VpType = "C" Or ( VpType = "A" And VpMyCard.Power <> "" And VpMyCard.Tough <> "") Then
                        VgDBCommand.CommandText = "Insert Into Creature Values ('" + VpMyCard.Title.Replace("'", "''") + "', " + VpMyCard.MyPower + ", " + VpMyCard.MyTough + ", Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null);"
                        VgDBCommand.ExecuteNonQuery
                    End If
                    'Si c'est une nouvelle carte, insertion dans la table TextesFR o� par d�faut le texte fran�ais sera le texte anglais jusqu'� mise � jour (CardName, TexteFR)
                    Try
                        VgDBCommand.CommandText = "Insert Into TextesFR (CardName, TexteFR) Values ('" + VpMyCard.Title.Replace("'", "''") + "', " + VpMyCard.MyCardText + ");"
                        VgDBCommand.ExecuteNonQuery
                    Catch   'Trappe d'erreur au cas o� une mise � jour de textes VF a �t� faite avant que l'�dition n'ait �t� ajout�e (auquel cas TextesFR est d�j� bon et il n'y a rien de plus � faire)
                    End Try
                End If
            Catch
                Call mdlToolbox.ShowWarning("Erreur lors de l'insertion de la carte " + VpMyCard.Title + "...")
                Return False
            End Try
        End If
        Return True
    End Function
    Public Shared Function ParseNewCard(VpFile As StreamReader) As String()
    '----------------------------------------------------------------------------------------------
    'Regarde � la position courante du flux si des informations sur une nouvelle carte s'y trouvent
    '----------------------------------------------------------------------------------------------
    Dim VpLine As String
    Dim VpCarac(0 To mdlConstGlob.CgBalises.Length - 1) As String
    Dim VpFound As Boolean
    Dim VpMulti As Boolean
        VpLine = VpFile.ReadLine.Trim
        If VpLine.StartsWith(mdlConstGlob.CgBalises(0)) Or VpLine.StartsWith(mdlConstGlob.CgAlternateStart) Or VpLine.StartsWith(mdlConstGlob.CgAlternateStart2) Then
            For VpI As Integer = 0 To mdlConstGlob.CgBalises.Length - 2
                VpFound = False
                VpMulti = False
                Do
                    'Analyse de la ligne selon les balises
                    If VpLine.StartsWith(mdlConstGlob.CgBalises(VpI)) Or VpI = 0 Then
                        VpCarac(VpI) = VpLine.Replace(mdlConstGlob.CgBalises(VpI), "").Replace(mdlConstGlob.CgAlternateStart, "").Replace(mdlConstGlob.CgAlternateStart2, "").Trim
                        VpFound = True
                        If VpI = 4 Then 'La 5�me balise (indic�e 4) "Rules Text:" est une balise dont le contenu peut prendre plusieurs lignes
                            VpMulti = True
                        End If
                    ElseIf VpMulti And VpLine.StartsWith(mdlConstGlob.CgBalises(VpI + 1)) Then 'si on voit la balise suivante, c'est qu'on a fini
                        VpMulti = False
                    ElseIf VpMulti Then
                        VpCarac(VpI) = VpCarac(VpI) + vbCrLf + VpLine
                    End If
                    'Pr�aparation de la ligne suivante
                    If Not VpFile.EndOfStream Then
                        VpLine = VpFile.ReadLine.Trim
                    Else
                        Exit Do 'si tout se passe bien, cette ligne ne devrait jamais �tre ex�cut�e avant l'insertion de la derni�re carte
                    End If
                Loop Until VpFound And Not VpMulti
            Next VpI
            Return VpCarac
        End If
        Return Nothing
    End Function
    Private Sub AddNewEdition(VpSilent As Boolean)
    '---------------------------------------------------------------------------------------
    'Ajoute � la base de donn�es l'ensemble des cartes pr�sentes dans les fichiers sp�cifi�s
    '---------------------------------------------------------------------------------------
    Dim VpFile As New StreamReader(Me.txtSpoilerList.Text, Encoding.Default)
    Dim VpCounter As Integer = 0
    Dim VpStrs() As String
    Dim VpEncNbrDown As Long
    Dim VpEncNbrTop As Long
    Dim VpSerieCD As String
        VmEncNbr0 = -1
        'Ajout des cartes
        Do While Not VpFile.EndOfStream
            If Me.AddNewCard(frmNewEdition.ParseNewCard(VpFile)) Then
                VpCounter = VpCounter + 1
            End If
        Loop
        VpFile.Close
        'Traduction
        If Not Me.txtSpoilerList.Tag Is Nothing AndAlso File.Exists(Me.txtSpoilerList.Tag.ToString) Then
            Me.lblStatus.Text = "Traduction en cours..."
            Application.DoEvents
            VpFile = New StreamReader(Me.txtSpoilerList.Tag.ToString, Encoding.Default)
            While Not VpFile.EndOfStream
                VpStrs = VpFile.ReadLine.Split("#")
                VgDBCommand.CommandText = "Update CardFR Inner Join Card On CardFR.EncNbr = Card.EncNbr Set CardFR.TitleFR = '" + VpStrs(1).Replace("'", "''") + "' Where Card.Title = '" + VpStrs(0).Replace("'", "''") + "' And CardFR.EncNbr >= " + VmEncNbr0.ToString + ";"
                VgDBCommand.ExecuteNonQuery
            End While
            VpFile.Close
        End If
        'Gestion des doubles cartes �ventuelles
        If Not Me.txtCheckList.Tag Is Nothing AndAlso File.Exists(Me.txtCheckList.Tag.ToString) Then
            Me.lblStatus.Text = "Association des doubles cartes en cours..."
            Application.DoEvents
            VpSerieCD = mdlToolbox.GetSerieCodeFromName(Me.chkNewEdition.Tag)
            VpFile = New StreamReader(Me.txtCheckList.Tag.ToString, Encoding.Default)
            While Not VpFile.EndOfStream
                VpStrs = VpFile.ReadLine.Split("#")
                VpEncNbrDown = mdlToolbox.GetEncNbr(VpStrs(0), VpSerieCD)
                VpEncNbrTop = mdlToolbox.GetEncNbr(VpStrs(1), VpSerieCD)
                VgDBCommand.CommandText = "Insert Into CardDouble(EncNbrDownFace, EncNbrTopFace) Values (" + VpEncNbrDown.ToString + ", " + VpEncNbrTop.ToString + ");"
                VgDBCommand.ExecuteNonQuery
                VgDBCommand.CommandText = "Update Card Set SpecialDoubleCard = True Where Card.EncNbr = " + VpEncNbrDown.ToString + ";"
                VgDBCommand.ExecuteNonQuery
                VgDBCommand.CommandText = "Update Card Set SpecialDoubleCard = True Where Card.EncNbr = " + VpEncNbrTop.ToString + ";"
                VgDBCommand.ExecuteNonQuery
            End While
            VpFile.Close
        End If
        Me.lblStatus.Text = "Termin�."
        If Not VpSilent Then
            Call mdlToolbox.ShowInformation(VpCounter.ToString + " carte(s) ont �t� ajout�e(s) � la base de donn�es...")
        End If
        Me.txtCheckList.Text = ""
        Me.txtSpoilerList.Text = ""
        Call Me.CheckLoad
    End Sub
    Private Function BuildList(VpSQL As String) As List(Of String)
    '-------------------------------------------------------------------------
    'Renvoie une liste des �l�ments r�pondant � la requ�te pass�e en param�tre
    '-------------------------------------------------------------------------
    Dim VpList As New List(Of String)
        VgDBCommand.CommandText = VpSQL
        VgDBReader = VgDBCommand.ExecuteReader
        With VgDBReader
            While .Read
                VpList.Add(.GetString(0))
            End While
            .Close
        End With
        Return VpList
    End Function
    Sub CheckLoad
    '----------------------------------------------------------------------------------------------------------------------------------------
    'Ajoute dans la checkboxlist l'ensemble des �ditions pr�sentes dans la table des �ditions mais pas dans celle des cartes d�j� r�f�renc�es
    '----------------------------------------------------------------------------------------------------------------------------------------
    Dim VpAlready As List(Of String)
    Dim VpAll As List(Of String)
        Me.chkNewEdition.Items.Clear
        VpAlready = Me.BuildList("Select Distinct Series.SeriesNM From Card Inner Join Series On Card.Series = Series.SeriesCD;")
        VpAll = Me.BuildList("Select SeriesNM From Series;")
        For Each VpItem As String In VpAll
            If VpAlready.BinarySearch(VpItem) < 0 Then
                Me.chkNewEdition.Items.Add(VpItem)
            End If
        Next VpItem
    End Sub
    Sub FrmNewEditionLoad(ByVal sender As Object, ByVal e As EventArgs)
        Me.propEdition.SelectedObject = VmEditionHeader
    End Sub
    Sub ChkNewEditionItemCheck(ByVal sender As Object, ByVal e As ItemCheckEventArgs)
    Dim VpAll As Boolean = True
    Dim VpNone As Boolean = True
        If VmBusy Then Exit Sub
        VmBusy = True
        For VpI As Integer = 0 To Me.chkNewEditionAuto.Items.Count - 1
            If VpI <> e.Index Then
                VpAll = VpAll And Me.chkNewEditionAuto.GetItemChecked(VpI)
                VpNone = VpNone And (Not Me.chkNewEditionAuto.GetItemChecked(VpI))
            Else
                VpAll = VpAll And e.NewValue = CheckState.Checked
                VpNone = VpNone And (Not e.NewValue = CheckState.Checked)
            End If
        Next VpI
        If VpAll Then
            Me.chkAllNone.CheckState = CheckState.Checked
        ElseIf VpNone Then
            Me.chkAllNone.CheckState = CheckState.Unchecked
        Else
            Me.chkAllNone.CheckState = CheckState.Indeterminate
        End If
        Me.cmdAutoNext.Enabled = Not VpNone
        VmBusy = False
    End Sub
    Sub CmdOKClick(ByVal sender As Object, ByVal e As EventArgs)
    '-------------------------------------------------------------------------
    'V�rifie la coh�rence de la demande avant de lancer la proc�dure effective
    '-------------------------------------------------------------------------
        If Not File.Exists(Me.txtCheckList.Text) Or Not File.Exists(Me.txtSpoilerList.Text) Then
            Call mdlToolbox.ShowWarning(mdlConstGlob.CgErr0)
        Else
            If Me.chkNewEdition.CheckedItems.Count > 0 Then
                Me.chkNewEdition.Tag = Me.chkNewEdition.CheckedItems(0).ToString
                Call Me.AddNewEdition(False)
            Else
                Call mdlToolbox.ShowWarning("Aucune �dition n'a �t� s�lectionn�e dans la liste...")
            End If
        End If
    End Sub
    Sub CmdBrowseClick(ByVal sender As Object, ByVal e As EventArgs)
        Me.dlgOpen.FileName = ""
        Me.dlgOpen.ShowDialog
        Me.Controls.Find(sender.Name.Replace("cmdBrowse", ""), True)(0).Text = Me.dlgOpen.FileName
    End Sub
    Sub CmdHeaderNextClick(ByVal sender As Object, ByVal e As EventArgs)
        If Not Me.chkHeaderAlready.Checked Then
            If Me.InsertHeader Then
                With VmEditionHeader
                    'Copie du fichier logo
                    If File.Exists(.LogoEdition) Then
                        Try
                            File.Copy(.LogoEdition, Application.StartupPath + mdlConstGlob.CgIcons + .LogoEdition.Substring(.LogoEdition.LastIndexOf("\")))
                        Catch
                        End Try
                    Else
                        Call mdlToolbox.ShowWarning("Aucun logo d'�dition n'a �t� sp�cifi�...")
                    End If
                End With
            End If
        End If
        Call Me.CheckLoad
        Me.grpData.Visible = True
        Me.grpHeader.Visible = False
    End Sub
    Sub CmdHeaderPreviousClick(ByVal sender As Object, ByVal e As EventArgs)
        Me.grpAssist.Visible = True
        Me.grpHeader.Visible = False
    End Sub
    Sub CmdAssistCancelClick(ByVal sender As Object, ByVal e As EventArgs)
        Me.Close
    End Sub
    Sub CmdAssistNextClick(ByVal sender As Object, ByVal e As EventArgs)
        If Me.optUpdate.Checked Then
            Call Me.UpdateSeriesHeaders
        Else
            If Me.optAuto.Checked Then
                Me.chkNewEditionAuto.Items.Clear
                Me.cmdAutoPrevious.Enabled = False
                Me.cmdAutoNext.Enabled = False
                Me.grpAuto.Visible = True
                Me.lblStatus.Text = "R�cup�ration des en-t�tes..."
                Cursor.Current = Cursors.WaitCursor
                Application.DoEvents
                Me.chkNewEditionAuto.Tag = Me.QuerySeries
                Me.lblStatus.Text = ""
            Else
                Me.grpHeader.Visible = True
            End If
            Me.grpAssist.Visible = False
        End If
    End Sub
    Sub LnklblAssistLinkClicked(ByVal sender As Object, ByVal e As LinkLabelLinkClickedEventArgs)
        Process.Start(mdlConstGlob.CgURL6)
    End Sub
    Sub ChkHeaderAlreadyCheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Me.propEdition.Enabled = Not Me.chkHeaderAlready.Checked
    End Sub
    Sub OptManualCheckedChanged(sender As Object, e As EventArgs)
        Me.lnklblAssist.Enabled = Me.optManual.Checked
    End Sub
    Sub ChkAllNoneCheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        If VmBusy Then Exit Sub
        VmBusy = True
        For VpI As Integer = 0 To chkNewEditionAuto.Items.Count - 1
            Me.chkNewEditionAuto.SetItemChecked(VpI, Me.chkAllNone.Checked)
        Next VpI
        Me.cmdAutoNext.Enabled = Me.chkAllNone.Checked
        VmBusy = False
    End Sub
    Sub CmdAutoNextClick(sender As Object, e As EventArgs)
    Dim VpInfos() As String
    Dim VpMulti As Boolean
        Me.cmdAutoNext.Enabled = False
        VpMulti = ( Me.chkNewEditionAuto.CheckedItems.Count > 1 )
        For Each VpToAdd As Object In Me.chkNewEditionAuto.CheckedItems
            For Each VpLine As String In Me.chkNewEditionAuto.Tag
                VpInfos = VpLine.Split("#")
                If VpInfos(2) = VpToAdd.ToString Then
                    Call Me.UpdateSerie(VpInfos, VpMulti)
                    Exit For
                End If
            Next VpLine
            'Si l'utilisateur a cliqu� sur 'Pr�c�dent', on interrompt
            If Me.grpAssist.Visible And Not Me.grpAuto.Visible Then
                Exit For
            End If
        Next VpToAdd
        Me.grpAssist.Visible = True
        Me.grpAuto.Visible = False
        Me.chkAllNone.Checked = False
        Me.cmdAutoNext.Enabled = True
    End Sub
    Sub CmdAutoPreviousClick(sender As Object, e As EventArgs)
        Me.grpAssist.Visible = True
        Me.grpAuto.Visible = False
    End Sub
End Class
