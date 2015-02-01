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
'| Auteur         |                          Couitchy |
'|----------------------------------------------------|
'| Modifications :                                    |
'| - mode automatique					   13/12/2009 |
'| - mise à jour auto des headers		   21/04/2010 |
'------------------------------------------------------
Imports System.IO
Imports System.ComponentModel
Imports System.Text
Imports System.Globalization
Public Partial Class frmNewEdition
	Private VmEditionHeader As New clsEditionHeader
	Private VmEncNbr0 As Long = -1
	Public Sub New()
		Me.InitializeComponent()
		Me.picMagic.Image = Image.FromFile(VgOptions.VgSettings.MagicBack)
	End Sub
	Private Function InsertHeader As Boolean
	'--------------------------------------------------------------
	'Inscrit l'en-tête de la nouvelle série dans la base de données
	'--------------------------------------------------------------
		Try
			With VmEditionHeader
				'(SeriesCD, SeriesCD_MO, SeriesCD_MW, SeriesNM, SeriesNM_MtG, SeriesNM_FR, Null, Null, True, True, Border, Release, Null, TotCards, TotCards, Rare, Uncommon, Common, Land, Foils, Nullx12, Notes)
				VgDBCommand.CommandText = "Insert Into Series (SeriesCD, SeriesCD_MO, SeriesCD_MW, SeriesNM, SeriesNM_MtG, SeriesNM_FR, LegalE, LegalS, Border, Release, TotCards, UqCards, UqRare, UqUncom, UqComm, UqBLand, Foils, Notes) Values ('" + .SeriesCD + "', '" + .SeriesCD_MO + "', '" + .SeriesCD_MW + "', '" + .SeriesNM.Replace("'", "''") + "', '" + .SeriesNM_MtG.Replace("'", "''") + "', '" + .SeriesNM_FR.Replace("'", "''") + "', True, True, " + .GetBorder(.Border) + ", " + clsModule.GetDate(.Release) + ", " + .TotCards.ToString + ", " + .TotCards.ToString + ", " + .Rare.ToString + ", " + .Uncommon.ToString + ", " + .Common.ToString + ", " + .Land.ToString + ", True, '" + .NotesEdition.Replace("'", "''") + "');"
				VgDBCommand.ExecuteNonQuery
			End With
		Catch
			Call clsModule.ShowWarning("Impossible d'ajouter l'en-tête à la base de données..." + vbCrLf + "Peut-être ce nom d'édition existe-t-il déjà ? Vérifier les informations saisies et recommencer.")
			Return False
		End Try
		Return True
	End Function
	Private Sub DLResource(VpCount As String, VpFrom As String, VpTo As String)
	'------------------------------------------------------
	'Télécharge un fichier nécessaire à l'ajout d'une série
	'------------------------------------------------------
		Me.lblStatus.Text = "Téléchargement des données en cours... " + VpCount
		Application.DoEvents
		Call clsModule.DownloadNow(New Uri(VpFrom), VpTo)
	End Sub
	Private Sub FillHeader(VpInfos() As String)
	'-------------------------------------------------------------------------
	'Remplit l'objet en-tête à partir du tableau de chaînes passé en paramètre
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
	Private Sub UpdateSerie(VpInfos() As String)
	'------------------------------------------------------------------------------------
	'Met à jour automatiquement l'édition dont les informations sont passées en paramètre
	'------------------------------------------------------------------------------------
	Dim VpChecker As String = "\" + VpInfos(0) + "_checklist_en.txt"
	Dim VpSpoiler As String = "\" + VpInfos(0) + "_spoiler_en.txt"
	Dim VpTrad As String = "\" + VpInfos(0) + "_titles_fr.txt"
	Dim VpDouble As String = "\" + VpInfos(0) + "_doubles_en.txt"
		'Téléchargement des fichiers nécessaires
		Call Me.DLResource("(1/5)", clsModule.VgOptions.VgSettings.DownloadServer + CgURL5 + "_e" + VpInfos(1) + ".png", clsModule.CgIcons + "\_e" + VpInfos(1) + ".png")
		Call Me.DLResource("(2/5)", clsModule.VgOptions.VgSettings.DownloadServer + CgURL4 + VpInfos(0) + "_checklist_en.txt", VpChecker)
		Call Me.DLResource("(3/5)", clsModule.VgOptions.VgSettings.DownloadServer + CgURL4 + VpInfos(0) + "_spoiler_en.txt", VpSpoiler)
		Call Me.DLResource("(4/5)", clsModule.VgOptions.VgSettings.DownloadServer + CgURL4 + VpInfos(0) + "_titles_fr.txt", VpTrad)
		Call Me.DLResource("(5/5)", clsModule.VgOptions.VgSettings.DownloadServer + CgURL4 + VpInfos(0) + "_doubles_en.txt", VpDouble)
		'Inscription de l'en-tête
		Me.lblStatus.Text = "Inscription de l'en-tête..."
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
				Call clsModule.ShowWarning(clsModule.CgErr0)
			Else
				Call Me.AddNewEdition
			End If
		End If
		'Suppression des fichiers temporaires
		Call clsModule.SecureDelete(Application.StartupPath + clsModule.CgUpSeries)
		Call clsModule.SecureDelete(Application.StartupPath + VpChecker)
		Call clsModule.SecureDelete(Application.StartupPath + VpSpoiler)
		Call clsModule.SecureDelete(Application.StartupPath + VpTrad)
		Call clsModule.SecureDelete(Application.StartupPath + VpDouble)
	End Sub
	Private Sub UpdateSeriesHeaders
	'--------------------------------------------------------------
	'Met à jour les en-têtes des séries (table Series) dans la base
	'--------------------------------------------------------------
	Dim VpSeriesInfos As StreamReader
	Dim VpInfos() As String
	Dim VpLine As String
	Dim VpFullUpdate As Boolean
		Call clsModule.DownloadNow(New Uri(clsModule.VgOptions.VgSettings.DownloadServer + CgURL12), clsModule.CgUpSeries)
		If File.Exists(Application.StartupPath + clsModule.CgUpSeries) Then
			VpFullUpdate = ( clsModule.ShowQuestion("Voulez-vous mettre à jour l'intégralité des en-têtes ?" + vbCrLf + "Cliquez sur 'Non' pour mettre uniquement à jour les codes des éditions (compatibilité avec les autres formats de logiciels Magic)") = System.Windows.Forms.DialogResult.Yes )
			VpSeriesInfos = New StreamReader(Application.StartupPath + clsModule.CgUpSeries)
			Do While Not VpSeriesInfos.EndOfStream
				VpLine = VpSeriesInfos.ReadLine
				If VpLine.Contains("#") Then
					VpInfos = VpLine.Split("#")
					Call Me.FillHeader(VpInfos)
					Try
						With VmEditionHeader
							If VpFullUpdate Then
								VgDBCommand.CommandText = "Update Series Set SeriesCD_MO = '" + .SeriesCD_MO + "', SeriesCD_MW = '" + .SeriesCD_MW + "', SeriesNM_FR = '" + .SeriesNM_FR.Replace("'", "''") + "', SeriesNM_MtG = '" + .SeriesNM_MtG.Replace("'", "''") + "', Border = " + .GetBorder(.Border) + ", Release = " + clsModule.GetDate(.Release) + ", TotCards = " + .TotCards.ToString + ", UqRare = " + .Rare.ToString + ", UqUncom = " + .Uncommon.ToString + ", UqComm = " + .Common.ToString + ", UqBLand = " + .Land.ToString + ", Notes = '" + .NotesEdition.Replace("'", "''") + "' Where SeriesCD = '" + .SeriesCD + "';"
							Else
								VgDBCommand.CommandText = "Update Series Set SeriesCD_MO = '" + .SeriesCD_MO + "', SeriesCD_MW = '" + .SeriesCD_MW + "' Where SeriesCD = '" + .SeriesCD + "';"
							End If
							VgDBCommand.ExecuteNonQuery
						End With
					Catch
						Call clsModule.ShowWarning("Impossible de mettre à jour l'en-tête " + VmEditionHeader.SeriesNM + " de la base de données...")
					End Try
				End If
			Loop
			Call clsModule.SecureDelete(Application.StartupPath + clsModule.CgUpSeries)
			Call clsModule.ShowInformation("Terminé !" + vbCrLf + "Il est recommandé de relancer l'application...")
		Else
			Call clsModule.ShowWarning(clsModule.CgDL3b)
		End If
	End Sub
	Private Function QuerySeries As List(Of String)
	'------------------------------------------
	'Récupère la liste des éditions disponibles
	'------------------------------------------
	Dim VpSeriesInfos As StreamReader
	Dim VpInfos() As String
	Dim VpLine As String
	Dim VpAlready As List(Of String)
	Dim VpNew As New List(Of String)
	Dim VpMustAdd As Boolean
		Call clsModule.DownloadNow(New Uri(clsModule.VgOptions.VgSettings.DownloadServer + CgURL12), clsModule.CgUpSeries)
		If File.Exists(Application.StartupPath + clsModule.CgUpSeries) Then
			VpAlready = Me.BuildList("Select UCase(SeriesNM) From Series;")
			VpSeriesInfos = New StreamReader(Application.StartupPath + clsModule.CgUpSeries)
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
					End If
				End If
			Loop
			Me.cmdAutoPrevious.Enabled = True
			Return VpNew
		Else
			Call clsModule.ShowWarning(clsModule.CgDL3b)
			Me.cmdAutoPrevious.Enabled = True
			Return Nothing
		End If
	End Function
	Private Function AddNewCard(VpCarac() As String) As Boolean
	'-------------------------------------------------------------------------------------------------------------------------------------------------------------
	'Ajoute à la base de données la carte donc les caractéristiques sont passées en paramètre en ayant au prélable complété celles manquantes grâce à la checklist
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
		'Code la nouvelle édition
		VpSerieCD = clsModule.GetSerieCodeFromName(Me.chkNewEdition.Tag)
		'Dernier numéro d'identification de carte utilisé
		VgDBCommand.CommandText = "Select Max(EncNbr) From Card;"
		VpEncNbr = CLng(VgDBCommand.ExecuteScalar) + 1
		If VmEncNbr0 = -1 Then
			VmEncNbr0 = VpEncNbr
		End If
		'Vérifie si la carte a déjà été éditée dans une édition précédente
		VgDBCommand.CommandText = "Select LastPrint From Spell Where Title = '" + VpCarac(0).Replace("'", "''") + "';"
		VpPrevious = Not ( VgDBCommand.ExecuteScalar Is Nothing )
		'Parcours de la checklist
		Do While Not VpFile.EndOfStream
			VpLine = VpFile.ReadLine.Trim
			VpFLine = VpLine.Replace("	", " ")
			'S'assure que l'on fait bien une recherche sur le mot entier (et pas une sous-chaîne) en ayant préalablement supprimé les tabulations pour la comparaison
			If VpFLine.Contains(" " + VpCarac(0) + " ") Then
				VpIndex = VpLine.IndexOf(VpCarac(0))
				VpLen = VpCarac(0).Length
				VpFound = True
			'(évite les erreurs dues au caractère apostrophe dans des charsets exotiques !)
			ElseIf VpFLine.Contains(" " + VpCarac(0).Replace("'", "") + " ") Then
				VpIndex = VpLine.IndexOf(VpCarac(0).Replace("'", ""))
				VpLen = VpCarac(0).Length - 1
				VpFound = True
			Else
				VpFound = False
			End If
			If VpFound Then
				'à la recherche du nom de l'auteur, de la couleur et de la rareté de la carte (attention, remplacement des tabulations)
				VpLine = VpLine.Substring(VpIndex + VpLen).Replace("	", "  ").Trim
				While VpLine.Contains("  ")
					VpComplement.Add(VpLine.Substring(0, VpLine.IndexOf("  ")))
					VpLine = VpLine.Substring(VpLine.IndexOf("  ") + 2)
				End While
				VpComplement.Add(VpLine)
				'On sort dès qu'on a trouvé, inutile de parcourir tout le fichier
				Exit Do
			End If
		Loop
		VpFile.Close
		If VpComplement.Count = 0 Then
			Call clsModule.ShowWarning("Impossible de trouver la correspondance pour la carte " + VpCarac(0) + "...")
			Return False
		Else
			VpMyCard = New clsMyCard(VpCarac, VpComplement)
			Try
				'Insertion dans la table Card (Series, Title, EncNbr, 1, Null, Rarity, Type, SubType, 1, 0, Null, 'N', Null, Null, Author, False, 10, 10, CardText, Null)
				VgDBCommand.CommandText = "Insert Into Card (Series, Title, EncNbr, Versions, CardNbr, Rarity, Type, SubType, myPrice, Price, PriceDate, Condition, FoilPrice, FoilDate, Artist, CenterText, TextSize, FlavorSize, CardText, FlavorText, SpecialDoubleCard) Values ('" + VpSerieCD + "', '" + VpMyCard.Title.Replace("'", "''") + "', " + VpEncNbr.ToString + ", 1, Null, '" + VpMyCard.Rarity + "', '" + VpMyCard.MyType + "', " + VpMycard.MySubType + ", 1, 0, Null, 'N', Null, Null, '" + VpMyCard.Author.Replace("'", "''") + "', False, 10, 10, " + VpMyCard.MyCardText + ", Null, False);"
				VgDBCommand.ExecuteNonQuery
				'Insertion dans la table CardFR où par défaut le nom français sera le nom anglais jusqu'à mise à jour (EncNbr, TitleFR)
				VgDBCommand.CommandText = "Insert Into CardFR Values (" + VpEncNbr.ToString + ", '" + VpMyCard.Title.Replace("'", "''") + "');"
				VgDBCommand.ExecuteNonQuery
				'Insertion (ou mise à jour) dans la table Spell (Title, LastPrint, Color, Null, Null, myCost, Cost, Nullx32)
				If VpPrevious Then
					VgDBCommand.CommandText = "Update Spell Set LastPrint = '" + VpSerieCD + "' Where Title = '" + VpMyCard.Title.Replace("'", "''") + "';"
				Else
					VgDBCommand.CommandText = "Insert Into Spell (Title, LastPrint, Color, Goal, Rating, myCost, Cost, CostA, CostB, CostU, CostG, CostR, CostW, CostX, ConvCost, CostLife, CostUnsum, CostSac, CostDisc, Kicker, Buyback, Flashback, Cycling, Madness, Upkeep, UpkeepMana, UpkeepLife, UpkeepSac, UpkeepDisc, Cumulative, Echo, Phasing, Fading, Cantrip, Threshold, Legal1, LegalE, LegalB, Rulings) Values ('" + VpMyCard.Title.Replace("'", "''") + "', '" + VpSerieCD + "', '" + VpMyCard.MyColor + "', Null, Null, " + VpMyCard.GetMyCost + ", " + VpMyCard.Cost + ", Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null);"
				End If
				VgDBCommand.ExecuteNonQuery
				If Not VpPrevious Then
					VpType = VpMyCard.MyType
					'Si c'est une nouvelle créature (ou créature-artefact ou arpenteur), insertion dans la table Creature (Title, Power, Tough, Nullx37)
					If VpType = "P" Or VpType = "U" Or VpType = "C" Or ( VpType = "A" And VpMyCard.Power <> "" And VpMyCard.Tough <> "") Then
						VgDBCommand.CommandText = "Insert Into Creature Values ('" + VpMyCard.Title.Replace("'", "''") + "', " + VpMyCard.MyPower + ", " + VpMyCard.MyTough + ", Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null);"
						VgDBCommand.ExecuteNonQuery
					End If
					'Si c'est une nouvelle carte, insertion dans la table TextesFR où par défaut le texte français sera le texte anglais jusqu'à mise à jour (CardName, TexteFR)
					Try
						VgDBCommand.CommandText = "Insert Into TextesFR (CardName, TexteFR) Values ('" + VpMyCard.Title.Replace("'", "''") + "', " + VpMyCard.MyCardText + ");"
						VgDBCommand.ExecuteNonQuery
					Catch	'Trappe d'erreur au cas où une mise à jour de textes VF a été faite avant que la série n'ait été ajoutée (auquel cas TextesFR est déjà bon et il n'y a rien de plus à faire)
					End Try
				End If
			Catch
				Call clsModule.ShowWarning("Erreur lors de l'insertion de la carte " + VpMyCard.Title + "...")
				Return False
			End Try
		End If
		Return True
	End Function
	Public Shared Function ParseNewCard(VpFile As StreamReader) As String()
	'----------------------------------------------------------------------------------------------
	'Regarde à la position courante du flux si des informations sur une nouvelle carte s'y trouvent
	'----------------------------------------------------------------------------------------------
	Dim VpLine As String
	Dim VpCarac(0 To clsModule.CgBalises.Length - 1) As String
	Dim VpFound As Boolean
	Dim VpMulti As Boolean
		VpLine = VpFile.ReadLine.Trim
		If VpLine.StartsWith(clsModule.CgBalises(0)) Or VpLine.StartsWith(clsModule.CgAlternateStart) Or VpLine.StartsWith(clsModule.CgAlternateStart2) Then
			For VpI As Integer = 0 To clsModule.CgBalises.Length - 2
				VpFound = False
				VpMulti = False
				Do
					'Analyse de la ligne selon les balises
					If VpLine.StartsWith(clsModule.CgBalises(VpI)) Or VpI = 0 Then
						VpCarac(VpI) = VpLine.Replace(clsModule.CgBalises(VpI), "").Replace(clsModule.CgAlternateStart, "").Replace(clsModule.CgAlternateStart2, "").Trim
						VpFound = True
						If VpI = 4 Then	'La 5ème balise (indicée 4) "Rules Text:" est une balise dont le contenu peut prendre plusieurs lignes
							VpMulti = True
						End If
					ElseIf VpMulti And VpLine.StartsWith(clsModule.CgBalises(VpI + 1)) Then	'si on voit la balise suivante, c'est qu'on a fini
						VpMulti = False
					ElseIf VpMulti Then
						VpCarac(VpI) = VpCarac(VpI) + vbCrLf + VpLine
					End If
					'Préaparation de la ligne suivante
					If Not VpFile.EndOfStream Then
						VpLine = VpFile.ReadLine.Trim
					Else
						Exit Do	'si tout se passe bien, cette ligne ne devrait jamais être exécutée avant l'insertion de la dernière carte
					End If
				Loop Until VpFound And Not VpMulti
			Next VpI
			Return VpCarac
		End If
		Return Nothing
	End Function
	Private Sub AddNewEdition
	'---------------------------------------------------------------------------------------
	'Ajoute à la base de données l'ensemble des cartes présentes dans les fichiers spécifiés
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
		'Gestion des doubles cartes éventuelles
		If Not Me.txtCheckList.Tag Is Nothing AndAlso File.Exists(Me.txtCheckList.Tag.ToString) Then
			Me.lblStatus.Text = "Association des doubles cartes en cours..."
			Application.DoEvents
			VpSerieCD = clsModule.GetSerieCodeFromName(Me.chkNewEdition.Tag)
			VpFile = New StreamReader(Me.txtCheckList.Tag.ToString, Encoding.Default)
			While Not VpFile.EndOfStream
				VpStrs = VpFile.ReadLine.Split("#")
				VpEncNbrDown = clsModule.GetEncNbr(VpStrs(0), VpSerieCD)
				VpEncNbrTop = clsModule.GetEncNbr(VpStrs(1), VpSerieCD)
				VgDBCommand.CommandText = "Insert Into CardDouble(EncNbrDownFace, EncNbrTopFace) Values (" + VpEncNbrDown.ToString + ", " + VpEncNbrTop.ToString + ");"
				VgDBCommand.ExecuteNonQuery
				VgDBCommand.CommandText = "Update Card Set SpecialDoubleCard = True Where Card.EncNbr = " + VpEncNbrDown.ToString + ";"
				VgDBCommand.ExecuteNonQuery
				VgDBCommand.CommandText = "Update Card Set SpecialDoubleCard = True Where Card.EncNbr = " + VpEncNbrTop.ToString + ";"
				VgDBCommand.ExecuteNonQuery
	    	End While
			VpFile.Close
		End If
		Me.lblStatus.Text = "Terminé."
		Call clsModule.ShowInformation(VpCounter.ToString + " carte(s) ont été ajoutée(s) à la base de données...")
		Me.txtCheckList.Text = ""
		Me.txtSpoilerList.Text = ""
		Call Me.CheckLoad
	End Sub
	Private Function BuildList(VpSQL As String) As List(Of String)
	'-------------------------------------------------------------------------
	'Renvoie une liste des éléments répondant à la requête passée en paramètre
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
	'--------------------------------------------------------------------------------------------------------------------------------------
	'Ajoute dans la checkboxlist l'ensemble des séries présentes dans la table des éditions mais pas dans celle des cartes déjà référencées
	'--------------------------------------------------------------------------------------------------------------------------------------
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
	'---------------------------------------------------------
	'N'autorise la sélection que d'une unique nouvelle édition
	'---------------------------------------------------------
		For VpI As Integer = 0 To sender.Items.Count - 1
			If VpI <> e.Index Then
				sender.SetItemChecked(VpI, False)
			End If
		Next VpI
		Me.cmdAutoNext.Enabled = ( Me.chkNewEditionAuto.CheckedItems.Count = 0 )
	End Sub
	Sub CmdOKClick(ByVal sender As Object, ByVal e As EventArgs)
	'-------------------------------------------------------------------------
	'Vérifie la cohérence de la demande avant de lancer la procédure effective
	'-------------------------------------------------------------------------
		If Not File.Exists(Me.txtCheckList.Text) Or Not File.Exists(Me.txtSpoilerList.Text) Then
			Call clsModule.ShowWarning(clsModule.CgErr0)
		Else
			If Me.chkNewEdition.CheckedItems.Count > 0 Then
				Me.chkNewEdition.Tag = Me.chkNewEdition.CheckedItems(0).ToString
				Call Me.AddNewEdition
			Else
				Call clsModule.ShowWarning("Aucune série n'a été sélectionnée dans la liste...")
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
							File.Copy(.LogoEdition, Application.StartupPath + clsModule.CgIcons + .LogoEdition.Substring(.LogoEdition.LastIndexOf("\")))
						Catch
						End Try
					Else
						Call clsModule.ShowWarning("Aucun logo d'édition n'a été spécifié...")
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
				Me.lblStatus.Text = "Récupération des en-têtes..."
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
		Process.Start(clsModule.CgURL6)
	End Sub
	Sub ChkHeaderAlreadyCheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
		Me.propEdition.Enabled = Not Me.chkHeaderAlready.Checked
	End Sub
	Sub OptManualCheckedChanged(sender As Object, e As EventArgs)
		Me.lnklblAssist.Enabled = Me.optManual.Checked
	End Sub
	Sub CmdAutoNextClick(sender As Object, e As EventArgs)
	Dim VpInfos() As String
		Me.cmdAutoNext.Enabled = False
		For Each VpLine As String In Me.chkNewEditionAuto.Tag
			VpInfos = VpLine.Split("#")
			If VpInfos(2) = Me.chkNewEditionAuto.CheckedItems.Item(0).ToString Then
				Call Me.UpdateSerie(VpInfos)
				Exit For
			End If
		Next VpLine
		Me.grpAssist.Visible = True
		Me.grpAuto.Visible = False
		Me.cmdAutoNext.Enabled = True
	End Sub
	Sub CmdAutoPreviousClick(sender As Object, e As EventArgs)
		Me.grpAssist.Visible = True
		Me.grpAuto.Visible = False
	End Sub
End Class
Public Class clsMyCard
	Private VmTitle As String
	Private VmCost As String
	Private VmType As String
	Private VmSubType As String
	Private VmPower As String
	Private VmTough As String
	Private VmCardText As String
	Private VmAuthor As String
	Private VmColor As String
	Private VmRarity As String
	Public Sub New(VpCarac() As String, Optional VpComplement As List(Of String) = Nothing)
	Dim VpStrs() As String
		If VpCarac Is Nothing Then Exit Sub
		'Titre, coût, type, sous-type, attaque, défense, texte détaillé
		VmTitle = VpCarac(0).Trim
		VmCost = VpCarac(1).Trim
		VpStrs = VpCarac(2).Replace("—", "-").Split(New String() {" - "}, StringSplitOptions.None)
		VmType = VpStrs(0).Trim
		If VpStrs.Length > 1 Then
			VmSubType = VpStrs(1).Trim
		Else
			VmSubType = ""
		End If
		If VpCarac(3).Contains("/") Then
			VpStrs = VpCarac(3).Split("/")
			VmPower = VpStrs(0).Replace("(", "").Trim
			VmTough = VpStrs(1).Replace(")", "").Trim
		Else
			VmPower = ""
			VmTough = ""
		End If
		VmCardText = VpCarac(4)
		'Auteur, couleur, rareté
		If Not VpComplement Is Nothing Then
			VmAuthor = VpComplement.Item(0).ToString.Trim
			VmColor = VpComplement.Item(1).ToString.Trim
			If VmColor.Contains("/") Then
				VmColor = "Multicolor"
			End If
			VmRarity = VpComplement.Item(2).ToString.Trim
		End If
	End Sub
	Public Function GetMyCost As String
		Return clsModule.MyCost(VmCost).ToString
	End Function
	Public Function MyType As String
		'(C = creature, I = instant, A = artefact, E = enchant-creature, L = land, N = interruption, S = sorcery, T = enchantment, U = abilited creature, P = planeswalker, Q = plane, H = phenomenon, Y = conspiracy)
		If VmType.Contains("Artifact") Then
			Return "A"
		ElseIf VmType.Contains("Instant") Then
			Return "I"
		ElseIf VmType.Contains("Enchantment") Then
			If VmSubType = "Aura" Then
				Return "E"
			Else
				Return "T"
			End If
		ElseIf VmType.Contains("Creature") Or VmType.Contains("Summon") Then
			If VmCardText.Trim = "" Then
				Return "C"		'pas de texte : créature "classique"
			Else
				Return "U"		'texte descriptif : créature avec capacité
			End If
		ElseIf VmType.Contains("Land") Then
			Return "L"
		ElseIf VmType.Contains("Sorcery") Then
			Return "S"
		ElseIf VmType.Contains("Interrupt") Then
			Return "N"
		ElseIf VmType.Contains("Planeswalker") Then
			Return "P"
		ElseIf VmType.Contains("Plane") Then
			Return "Q"
		ElseIf VmType.Contains("Phenomenon") Then
			Return "H"
		ElseIf VmType.Contains("Conspiracy") Then
			Return "Y"
		Else
			Return ""
		End If
	End Function
	Public Function MySubType As String
		If VmSubType = "" Then
			Return "Null"
		ElseIf VmType.Contains("Artifact Creature") Then
			Return "'Creature " + VmSubType.Replace("'", "''") + "'"
		Else
			Return "'" + VmSubType.Replace("'", "''") + "'"
		End If
	End Function
	Public Function MyPower As String
		If VmPower = "" Then
			Return "'0'"
		Else
			Return "'" + VmPower + "'"
		End If
	End Function
	Public Function MyTough As String
		If VmTough = "" Then
			Return "'0'"
		Else
			Return "'" + VmTough + "'"
		End If
	End Function
	Public Function MyCardText As String
		If VmCardText = "" Then
			Return "Null"
		Else
			Return "'" + VmCardText.Replace("'", "''").Replace("/#/", vbCrLf + vbCrLf + "----" + vbCrLf + vbCrLf) + "'"
		End If
	End Function
	Public Function MyColor As String
	Dim VpMyType As String
		If VmColor = "" Then	'dans les dernières versions du gatherer, il n'y a rien lorsqu'il s'agit d'un artefact, d'un terrain, d'un plan, d'un phénomène, d'un jeton ou d'un arpenteur incolore
			VpMyType = Me.MyType
			Select Case VpMyType
				Case "H", "Q", "Y", "P"
					Return "A"
				Case Else
					Return VpMyType
			End Select
		Else
			Select Case VmColor
				Case "Colorless (Artifact)", "Colorless", "Artifact", "A"
					Return "A"
				Case "Black", "B"
					Return "B"
				Case "Green", "G"
					Return "G"
				Case "Colorless (Land)", "Land", "L"
					Return "L"
				Case "Multicolor", "Z"
					Return "M"
				Case "Red", "R"
					Return "R"
				Case "Blue", "U"
					Return "U"
				Case "White", "W"
					Return "W"
				'Cas mal géré des double cartes
				Case "X"
					Return "X"
				Case Else
					Return ""
			End Select
		End If
	End Function
	Public ReadOnly Property Title As String
		Get
			Return VmTitle
		End Get
	End Property
	Public ReadOnly Property Cost As String
		Get
			If VmCost <> "" Then
				Return "'" + VmCost + "'"
			Else
				Return "Null"
			End If
		End Get
	End Property
	Public ReadOnly Property Type As String
		Get
			Return VmType
		End Get
	End Property
	Public ReadOnly Property SubType As String
		Get
			Return VmSubType
		End Get
	End Property
	Public ReadOnly Property Power As String
		Get
			Return VmPower
		End Get
	End Property
	Public ReadOnly Property Tough As String
		Get
			Return VmTough
		End Get
	End Property
	Public ReadOnly Property CardText As String
		Get
			Return VmCardText
		End Get
	End Property
	Public ReadOnly Property Author As String
		Get
			Return VmAuthor
		End Get
	End Property
	Public ReadOnly Property Rarity As String
		Get
			Return VmRarity
		End Get
	End Property
End Class
Public Class clsEditionHeader
	Public Enum eBorder
		White
		Black
		Silver
	End Enum
	Private VmSeriesCD As String = "ME"
	Private VmSeriesCD_MO As String = "ME"
	Private VmSeriesCD_MW As String = "ME"
	Private VmSeriesNM As String = "Magic Edition"
	Private VmSeriesNM_FR As String = "Édition Magic"
	Private VmSeriesNM_MtG As String = "Magic Ed..."
	Private VmBorder As eBorder = eBorder.White
	Private VmRelease As Date = Date.Now.ToShortDateString
	Private VmTotCards As Integer = 175
	Private VmRare As Integer = 55
	Private VmUncommon As Integer = 55
	Private VmCommon As Integer = 55
	Private VmLand As Integer = 10
	Private VmLogoEdition As String = ""
	Private VmNotesEdition As String = ""
	<Category("Identification"), Description("Code de la série à 2 chiffres")> _
	Public Property SeriesCD As String
		Get
			Return VmSeriesCD.Substring(0, 2)
		End Get
		Set (VpSeriesCD As String)
			VmSeriesCD = VpSeriesCD
		End Set
	End Property
	<Category("Identification"), Description("Code de la série à 2 chiffres (Magic Online)")> _
	Public Property SeriesCD_MO As String
		Get
			Return VmSeriesCD_MO
		End Get
		Set (VpSeriesCD_MO As String)
			VmSeriesCD_MO = VpSeriesCD_MO
		End Set
	End Property
	<Category("Identification"), Description("Code de la série à 2 chiffres (Magic Workstation)")> _
	Public Property SeriesCD_MW As String
		Get
			Return VmSeriesCD_MW
		End Get
		Set (VpSeriesCD_MW As String)
			VmSeriesCD_MW = VpSeriesCD_MW
		End Set
	End Property
	<Category("Identification"), Description("Nom de la série (VO)")> _
	Public Property SeriesNM As String
		Get
			Return VmSeriesNM
		End Get
		Set (VpSeriesNM As String)
			VmSeriesNM = VpSeriesNM
		End Set
	End Property
	<Category("Identification"), Description("Nom de la série (VF)")> _
	Public Property SeriesNM_FR As String
		Get
			Return VmSeriesNM_FR
		End Get
		Set (VpSeriesNM_FR As String)
			VmSeriesNM_FR = VpSeriesNM_FR
		End Set
	End Property
	<Category("Identification"), Description("Nom raccourci de la série sur magiccorportation.com (correspondance requise pour la mise à jour des prix...)")> _
	Public Property SeriesNM_MtG As String
		Get
			Return VmSeriesNM_MtG
		End Get
		Set (VpSeriesNM_MtG As String)
			VmSeriesNM_MtG = VpSeriesNM_MtG
		End Set
	End Property
	<Category("Détails"), Description("Nombre de cartes de l'édition")> _
	Public Property TotCards As Integer
		Get
			Return VmTotCards
		End Get
		Set (VpTotCards As Integer)
			VmTotCards = VpTotCards
		End Set
	End Property
	<Category("Détails"), Description("Nombre de cartes rares")> _
	Public Property Rare As Integer
		Get
			Return VmRare
		End Get
		Set (VpRare As Integer)
			VmRare = VpRare
		End Set
	End Property
	<Category("Détails"), Description("Nombre de cartes peu communes")> _
	Public Property Uncommon As Integer
		Get
			Return VmUncommon
		End Get
		Set (VpUncommon As Integer)
			VmUncommon = VpUncommon
		End Set
	End Property
	<Category("Détails"), Description("Nombre de cartes communes")> _
	Public Property Common As Integer
		Get
			Return VmCommon
		End Get
		Set (VpCommon As Integer)
			VmCommon = VpCommon
		End Set
	End Property
	<Category("Détails"), Description("Nombre de terrains")> _
	Public Property Land As Integer
		Get
			Return VmLand
		End Get
		Set (VpLand As Integer)
			VmLand = VpLand
		End Set
	End Property
	<Category("Divers"), Description("Date de sortie")> _
	Public Property Release As Date
		Get
			Return VmRelease
		End Get
		Set (VpRelease As Date)
			VmRelease = VpRelease
		End Set
	End Property
	<Category("Divers"), Description("Type de bordure")> _
	Public Property Border As eBorder
		Get
			Return VmBorder
		End Get
		Set (VpBorder As eBorder)
			VmBorder = VpBorder
		End Set
	End Property
	<Category("Divers"), Description("Fichier d'image (PNG 21x21) correspondant au logo de l'édition"), Editor(GetType(System.Windows.Forms.Design.FileNameEditor), GetType(System.Drawing.Design.UITypeEditor))> _
	Public Property LogoEdition As String
		Get
			Return VmLogoEdition
		End Get
		Set (VpLogoEdition As String)
			VmLogoEdition = VpLogoEdition
		End Set
	End Property
	<Category("Divers"), Description("Notes diverses sur l'édition")> _
	Public Property NotesEdition As String
		Get
			If VmNotesEdition = "" Then
				Return "N/C"
			Else
				Return VmNotesEdition
			End If
		End Get
		Set (VpNotesEdition As String)
			VmNotesEdition = VpNotesEdition
		End Set
	End Property
	Public Function GetBorder(VpBorder As eBorder) As String
		Select Case VpBorder
			Case eBorder.Black
				Return "'B'"
			Case eBorder.White
				Return "'W'"
			Case eBorder.Silver
				Return "'S'"
			Case Else
				Return "Null"
		End Select
	End Function
	Public Function SetBorder(VpBorder As String) As eBorder
		Select Case VpBorder
			Case "B"
				Return eBorder.Black
			Case "W"
				Return eBorder.White
			Case "S"
				Return eBorder.Silver
			Case Else
				Return eBorder.White
		End Select
	End Function
End Class