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
Imports System.IO
Imports System.ComponentModel
Public Partial Class frmNewEdition
	Private VmEditionHeader As New clsEditionHeader
	Public Sub New()
		Me.InitializeComponent()
		Me.picMagic.Image = Image.FromFile(Application.StartupPath + clsModule.CgMagicBack)
	End Sub	
	Private Function AddNewCard(VpCarac() As String) As Boolean
	'-------------------------------------------------------------------------------------------------------------------------------------------------------------
	'Ajoute à la base de données la carte donc les caractéristiques sont passées en paramètre en ayant au prélable complété celles manquantes grâce à la checklist
	'-------------------------------------------------------------------------------------------------------------------------------------------------------------	
	Dim VpFile As StreamReader
	Dim VpLine As String
	Dim VpFLine As String
	Dim VpComplement As ArrayList
	Dim VpMyCard As clsMyCard
	Dim VpSerieCD As String
	Dim VpEncNbr As Long
	Dim VpPrevious As Boolean
	Dim VpType As String
	Dim VpFound As Boolean
	Dim VpIndex As Integer
	Dim VpLen As Integer
		If VpCarac Is Nothing Then Return False
		VpFile = New StreamReader(Me.txtCheckList.Text)
		VpComplement = New ArrayList
		'Code la nouvelle édition
		VpSerieCD = clsModule.GetSerieCodeFromName(Me.chkNewEdition.CheckedItems(0).ToString)
		'Dernier numéro d'identification de carte utilisé
		VgDBCommand.CommandText = "Select Max(EncNbr) From Card;"
		VpEncNbr = CLng(VgDBCommand.ExecuteScalar) + 1 
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
				'à la recherche du nom de l'auteur, de la couleur et de la rareté de la carte (attention, remplacement des tabulations
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
				VgDBCommand.CommandText = "Insert Into Card Values ('" + VpSerieCD + "', '" + VpMyCard.Title.Replace("'", "''") + "', " + VpEncNbr.ToString + ", 1, Null, '" + VpMyCard.Rarity + "', '" + VpMyCard.MyType + "', " + VpMycard.MySubType + ", 1, 0, Null, 'N', Null, Null, '" + VpMyCard.Author.Replace("'", "''") + "', False, 10, 10, " + VpMyCard.MyCardText + ", Null);"
				VgDBCommand.ExecuteNonQuery
				'Insertion dans la table CardFR où par défaut le nom français sera le nom anglais jusqu'à mise à jour (EncNbr, TitleFR)
				VgDBCommand.CommandText = "Insert Into CardFR Values (" + VpEncNbr.ToString + ", '" + VpMyCard.Title.Replace("'", "''") + "');"
				VgDBCommand.ExecuteNonQuery			
				'Insertion (ou mise à jour) dans la table Spell (Title, LastPrint, Color, Null, Null, myCost, Cost, Nullx32)
				If VpPrevious Then
					VgDBCommand.CommandText = "Update Spell Set LastPrint = '" + VpSerieCD + "' Where Title = '" + VpMyCard.Title.Replace("'", "''") + "';"
				Else
					VgDBCommand.CommandText = "Insert Into Spell Values ('" + VpMyCard.Title.Replace("'", "''") + "', '" + VpSerieCD + "', '" + VpMyCard.MyColor + "', Null, Null, '" + VpMyCard.GetMyCost + "', " + VpMyCard.Cost + ", Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null);"
				End If
				VgDBCommand.ExecuteNonQuery
				'Si c'est une nouvelle créature (ou créature-artefact ou arpenteur), insertion dans la table Creature (Title, Power, Tough, Nullx37)
				If Not VpPrevious Then
					VpType = VpMyCard.MyType
					If VpType = "P" Or VpType = "U" Or VpType = "C" Or ( VpType = "A" And VpMyCard.SubType.Contains("Creature") ) Then
						VgDBCommand.CommandText = "Insert Into Creature Values ('" + VpMyCard.Title.Replace("'", "''") + "', " + VpMyCard.MyPower + ", " + VpMyCard.MyTough + ", Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null);"
						VgDBCommand.ExecuteNonQuery
					End If
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
		VpLine = VpFile.ReadLine.Trim
		If VpLine.StartsWith(clsModule.CgBalises(0)) Or VpLine.StartsWith(clsModule.CgAlternateStart) Or VpLine.StartsWith(clsModule.CgAlternateStart2) Then
			For VpI As Integer = 0 To clsModule.CgBalises.Length - 1
				VpCarac(VpI) = VpLine.Replace(clsModule.CgBalises(VpI), "").Replace(clsModule.CgAlternateStart, "").Replace(clsModule.CgAlternateStart2, "").Trim
				VpLine = VpFile.ReadLine.Trim
			Next VpI
			Return VpCarac
		End If
		Return Nothing
	End Function
	Private Sub AddNewEdition
	'---------------------------------------------------------------------------------------
	'Ajoute à la base de données l'ensemble des cartes présentes dans les fichiers spécifiés
	'---------------------------------------------------------------------------------------
	Dim VpFile As New StreamReader(Me.txtSpoilerList.Text)
	Dim VpCounter As Integer = 0
		Do While Not VpFile.EndOfStream
			If Me.AddNewCard(frmNewEdition.ParseNewCard(VpFile)) Then
				VpCounter = VpCounter + 1
			End If
		Loop
		VpFile.Close
		Call clsModule.ShowInformation(VpCounter.ToString + " carte(s) ont été ajoutée(s) à la base de données...")
		Me.txtCheckList.Text = ""
		Me.txtSpoilerList.Text = ""
		Call Me.CheckLoad
	End Sub
	Private Function BuildList(VpSQL As String) As ArrayList
	'-------------------------------------------------------------------------
	'Renvoie une liste des éléments répondant à la requête passée en paramètre
	'-------------------------------------------------------------------------
	Dim VpList As New ArrayList
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
	Dim VpAlready As ArrayList
	Dim VpAll As ArrayList
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
		Me.propEdition.SelectedObject = Me.VmEditionHeader
	End Sub	
	Sub ChkNewEditionItemCheck(ByVal sender As Object, ByVal e As ItemCheckEventArgs)
	'---------------------------------------------------------
	'N'autorise la sélection que d'une unique nouvelle édition
	'---------------------------------------------------------
		For VpI As Integer = 0 To Me.chkNewEdition.Items.Count - 1
			If VpI <> e.Index Then
				Me.chkNewEdition.SetItemChecked(VpI, False)
			End If
		Next VpI
	End Sub	
	Sub CmdOKClick(ByVal sender As Object, ByVal e As EventArgs)
	'-------------------------------------------------------------------------
	'Vérifie la cohérence de la demande avant de lancer la procédure effective
	'-------------------------------------------------------------------------
		If Not File.Exists(Me.txtCheckList.Text) Or Not File.Exists(Me.txtSpoilerList.Text) Then
			Call clsModule.ShowWarning("Au moins un des deux fichiers spécifiés n'existe pas...")
		Else
			If Me.chkNewEdition.CheckedItems.Count > 0 Then
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
			With Me.VmEditionHeader
				Try
					'Insertion dans la table Series (SeriesCD, SeriesNM, SeriesNM_MtG, Null, Null, True, True, Border, Release, Null, TotCards, TotCards, Rare, Uncommon, Common, Land, Nullx14)
					VgDBCommand.CommandText = "Insert Into Series Values ('" + .SeriesCD.Substring(0, 2) + "', '" + .SeriesNM.Replace("'", "''") + "', '" + .SeriesNM_MtG.Replace("'", "''") + "', Null, Null, True, True, " + .GetBorder(.Border) + ", " + clsModule.GetDate(.Release) + ", Null, " + .TotCards.ToString + ", " + .TotCards.ToString + ", " + .Rare.ToString+ ", " + .Uncommon.ToString+ ", " + .Common.ToString+ ", " + .Land.ToString + ", Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null, Null);"			
					VgDBCommand.ExecuteNonQuery
				Catch
					Call clsModule.ShowWarning("Impossible d'ajouter l'en-tête à la base de données..." + vbCrLf + "Peut-être ce nom d'édition existe-t-il déjà ? Vérifier les informations saisies et recommencer.")
					Return
				End Try
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
		Me.grpHeader.Visible = True
		Me.grpAssist.Visible = False
	End Sub
	Sub LnklblAssist1LinkClicked(ByVal sender As Object, ByVal e As LinkLabelLinkClickedEventArgs)
		Diagnostics.Process.Start(clsModule.CgURL4)
	End Sub
	Sub LnklblAssist2LinkClicked(ByVal sender As Object, ByVal e As LinkLabelLinkClickedEventArgs)
		Diagnostics.Process.Start(clsModule.CgURL6)		
	End Sub
	Sub LnklblAssist3LinkClicked(ByVal sender As Object, ByVal e As LinkLabelLinkClickedEventArgs)
		Diagnostics.Process.Start(clsModule.CgURL5)
	End Sub
	Sub ChkHeaderAlreadyCheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
		Me.propEdition.Enabled = Not Me.chkHeaderAlready.Checked	
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
	Public Sub New(VpCarac() As String, Optional VpComplement As ArrayList = Nothing)
	Dim VpStrs() As String
		If VpCarac Is Nothing Then Exit Sub
		'Titre, coût, type, sous-type, attaque, défense, texte détaillé
		VmTitle = VpCarac(0).Trim
		VmCost = VpCarac(1).Trim
		VpStrs = VpCarac(2).Split(New String() {" - "}, StringSplitOptions.None)
		VmType = VpStrs(0).Trim
		If VpStrs.Length > 1 Then
			VmSubType = VpStrs(1).Trim
		Else
			VmSubType = ""
		End If
		If VpCarac(3).Contains("/") Then
			VpStrs = VpCarac(3).Split("/")
			VmPower = VpStrs(0).Trim
			VmTough = VpStrs(1).Trim
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
		'(C = creature, I = instant, A = artefact, E = enchant-creature, L = land, N = interruption, S = sorcery, T = enchantment, U = abilited creature, P = planeswalker)
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
		ElseIf VmType.Contains("Creature") Then
			If VmType = "Creature" Then
				Return "C"
			Else
				Return "U"
			End If
		ElseIf VmType.Contains("Land") Then
			Return "L"
		ElseIf VmType.Contains("Sorcery") Then
			Return "S"
		ElseIf VmType.Contains("Interrupt") Then
			Return "N"
		ElseIf VmType.Contains("Planeswalker") Then
			Return "P"			 
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
			Return "'" + VmCardText.Replace("'", "''") + "'"
		End If
	End Function
	Public Function MyColor As String
		Select Case VmColor
			Case "Artifact", "A"
				Return "A"
			Case "Black", "B"
				Return "B"
			Case "Green", "G"
				Return "G"
			Case "Land", "L", ""
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
	End Function
	Public ReadOnly Property Title As String
		Get
			Return VmTitle
		End Get
	End Property
	Public ReadOnly Property Cost As String
		Get
			Return IIf(VmCost <> "", "'" + VmCost + "'", "Null")
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
	Private VmSeriesNM As String = "Magic Edition"
	Private VmSeriesNM_MtG As String = "Magic Ed..."
	Private VmBorder As eBorder = eBorder.White
	Private VmRelease As Date = Date.Now.ToShortDateString
	Private VmTotCards As Integer = 175
	Private VmRare As Integer = 55
	Private VmUncommon As Integer = 55
	Private VmCommon As Integer = 55
	Private VmLand As Integer = 10
	Private VmLogoEdition As String = ""
	<Category("Identification"), Description("Code la série à 2 chiffres")> _
	Public Property SeriesCD As String
		Get
			Return VmSeriesCD
		End Get
		Set(VpSeriesCD As String)
			VmSeriesCD = VpSeriesCD
		End Set
	End Property
	<Category("Identification"), Description("Nom de la série")> _
	Public Property SeriesNM As String
		Get
			Return VmSeriesNM
		End Get
		Set(VpSeriesNM As String)
			VmSeriesNM = VpSeriesNM
		End Set
	End Property
	<Category("Identification"), Description("Nom raccourci de la série sur magiccorportation.com (12 caractères maxi., correspondance requise pour la mise à jour des prix...)")> _
	Public Property SeriesNM_MtG As String
		Get
			Return VmSeriesNM_MtG
		End Get
		Set(VpSeriesNM_MtG As String)
			VmSeriesNM_MtG = VpSeriesNM_MtG
		End Set
	End Property
	<Category("Détails"), Description("Nombre de cartes de l'édition")> _
	Public Property TotCards As Integer
		Get
			Return VmTotCards
		End Get
		Set(VpTotCards As Integer)
			VmTotCards = VpTotCards
		End Set
	End Property
	<Category("Détails"), Description("Nombre de cartes rares")> _
	Public Property Rare As Integer
		Get
			Return VmRare
		End Get
		Set(VpRare As Integer)
			VmRare = VpRare
		End Set
	End Property	
	<Category("Détails"), Description("Nombre de cartes peu communes")> _
	Public Property Uncommon As Integer
		Get
			Return VmUncommon
		End Get
		Set(VpUncommon As Integer)
			VmUncommon = VpUncommon
		End Set
	End Property
	<Category("Détails"), Description("Nombre de cartes communes")> _
	Public Property Common As Integer
		Get
			Return VmCommon
		End Get
		Set(VpCommon As Integer)
			VmCommon = VpCommon
		End Set
	End Property
	<Category("Détails"), Description("Nombre de terrains")> _
	Public Property Land As Integer
		Get
			Return VmLand
		End Get
		Set(VpLand As Integer)
			VmLand = VpLand
		End Set
	End Property	
	<Category("Divers"), Description("Date de sortie")> _
	Public Property Release As Date
		Get
			Return VmRelease
		End Get
		Set(VpRelease As Date)
			VmRelease = VpRelease
		End Set
	End Property	
	<Category("Divers"), Description("Type de bordure")> _
	Public Property Border As eBorder
		Get
			Return VmBorder
		End Get
		Set(VpBorder As eBorder)
			VmBorder = VpBorder
		End Set
	End Property
	<Category("Divers"), Description("Fichier d'image (PNG 21x21) correspondant au logo de l'édition"), Editor(GetType(System.Windows.Forms.Design.FileNameEditor), GetType(System.Drawing.Design.UITypeEditor))> _
	Public Property LogoEdition As String
		Get
			Return VmLogoEdition
		End Get
		Set(VpLogoEdition As String)
			VmLogoEdition = VpLogoEdition
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
End Class
