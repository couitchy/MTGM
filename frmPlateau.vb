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
'------------------------------------------------------
Imports System.IO
Public Partial Class frmPlateau
	Private VmSource As String
	Private VmRestriction As String
	Private VmRestrictionTXT As String
	Private VmPlateauPartie As clsPlateauPartie
	Private VmPictures As New List(Of PictureBox)
	Private VmCurrentPicture As PictureBox
	Public Sub New(VpOwner As MainForm)
	Dim VpPath As String = Path.GetTempPath + clsModule.CgTemp
		Me.InitializeComponent()
		VmSource = If(VpOwner.FilterCriteria.DeckMode, clsModule.CgSDecks, clsModule.CgSCollection)
		VmRestriction = VpOwner.Restriction
		VmRestrictionTXT = VpOwner.Restriction(True)
		If VmRestrictionTXT.Length > 31 Then
			VmRestrictionTXT = VmRestrictionTXT.Substring(0, 31)
		End If
		'Extraction des images en répertoire temporaire
		If Not Directory.Exists(VpPath) Then
			Directory.CreateDirectory(VpPath)
		End If
		Call clsModule.ExtractPictures(VpPath.Replace("\\", "\"), VmSource, VmRestriction)
		'Nouvelle partie
		VmPlateauPartie = New clsPlateauPartie(VmSource, VmRestriction)
		Call VmPlateauPartie.BeginPlateauPartie
	End Sub
	Private Sub ManageReDraw
	'-------------------------------
	'Actualisation du plateau de jeu
	'-------------------------------
		'Efface les anciennes images
		For Each VpPictureBox As PictureBox In VmPictures
			VpPictureBox.Parent.Controls.Remove(VpPictureBox)
			VpPictureBox.Dispose
		Next VpPictureBox
		VmPictures.Clear
		With VmPlateauPartie
			'Bibliothèque
			.BibliTop.Hidden = Not Me.btBibliReveal.Checked
			Call Me.DrawPicture(.BibliTop, Me.panelBibli, New EventHandler(AddressOf Me.CardBibliDoubleClick), New MouseEventHandler(AddressOf Me.CardBibliMouseUp))
			'Cimetière
			Call Me.DrawPicture(.GraveyardTop, Me.panelGraveyard, New EventHandler(AddressOf Me.CardGraveyardDoubleClick), New MouseEventHandler(AddressOf Me.CardGraveyardMouseUp))
			'Exil
			Call Me.DrawPictures(.Exil, Me.panelExil, New EventHandler(AddressOf Me.CardExilDoubleClick), New MouseEventHandler(AddressOf Me.CardExilMouseUp))
			'Regard
			Call Me.DrawPictures(.Regard, Me.panelRegard, New EventHandler(AddressOf Me.CardRegardDoubleClick), New MouseEventHandler(AddressOf Me.CardRegardMouseUp))
			'Main
			Call Me.DrawPictures(.Main, Me.panelMain, New EventHandler(AddressOf Me.CardMainDoubleClick), New MouseEventHandler(AddressOf Me.CardMainMouseUp))
			'Champ de bataille
			Call Me.DrawPictures(.Field, Me.panelField, New EventHandler(AddressOf Me.CardFieldDoubleClick), New MouseEventHandler(AddressOf Me.CardFieldMouseUp))
		End With
	End Sub
	Private Sub DrawPicture(VpCard As clsPlateauCard, VpParent As Control, VpIndex As Integer, VpCount As Integer, VpDoubleClickHandler As EventHandler, VpMouseUpHandler As MouseEventHandler)
	'---------------------------------
	'Dessin d'une carte sur le plateau
	'---------------------------------
	Dim VpPicture As PictureBox
	Dim VpW As Integer
	Dim VpH As Integer
		If Not VpCard Is Nothing Then
			VpPicture = New PictureBox
			'Détermine le côté limitant pour l'affichage optimal avec respect des proportions
			If CgMTGCardHeight_px - VpParent.Height > CgMTGCardWidth_px - VpParent.Width Then
				VpH = Math.Min(VpParent.Height, CgMTGCardHeight_px)
				VpW = CgMTGCardWidth_px * VpH / CgMTGCardHeight_px
			Else
				VpW = Math.Min(VpParent.Width, CgMTGCardWidth_px)
				VpH = CgMTGCardHeight_px * VpW / CgMTGCardWidth_px
			End If
			'Ajustement dynamique du contrôle PictureBox
			With VpPicture
				.Location = New System.Drawing.Point(VpParent.Width * VpIndex / VpCount, 0)
				.Size = New System.Drawing.Size(VpW, VpH)
				.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
				.Image = Image.FromFile(VpCard.PicturePath)
				.Tag = VpCard
				AddHandler .DoubleClick, VpDoubleClickHandler
				AddHandler .MouseUp, VpMouseUpHandler
				AddHandler .Paint, New PaintEventHandler(AddressOf Me.PictureBoxPaint)
			End With
			'Gestion carte engagée / dégagée
			Call Me.ManageTap(VpPicture, True)
			VpParent.Controls.Add(VpPicture)
			VmPictures.Add(VpPicture)		'conserve la référence
		End If
	End Sub
	Private Sub DrawPicture(VpCard As clsPlateauCard, VpParent As Control, VpDoubleClickHandler As EventHandler, VpMouseUpHandler As MouseEventHandler)
		Call Me.DrawPicture(VpCard, VpParent, 0, 1, VpDoubleClickHandler, VpMouseUpHandler)
	End Sub
	Private Sub DrawPictures(VpCards As List(Of clsPlateauCard), VpParent As Control, VpDoubleClickHandler As EventHandler, VpMouseUpHandler As MouseEventHandler)
	Dim VpCount As Integer = VpCards.Count
		For VpI As Integer = 0 To VpCount - 1
			Call Me.DrawPicture(VpCards.Item(VpI), VpParent, VpI, VpCount, VpDoubleClickHandler, VpMouseUpHandler)
		Next VpI
	End Sub
	Private Sub ManageTap(VpPicture As PictureBox, Optional VpStatic As Boolean = False)
	Dim VpCard As clsPlateauCard = VpPicture.Tag
		If VpCard.Tapped And Not VpStatic Then
			VpPicture.Image.RotateFlip(RotateFlipType.Rotate90FlipNone)
			VpPicture.Size = New Size(VpPicture.Height, VpPicture.Width)
			VpPicture.Location = New Point(VpPicture.Location.X, VpPicture.Location.Y - Math.Abs(VpPicture.Height - VpPicture.Width))
		ElseIf Not ( VpCard.Tapped Xor VpStatic )
			VpPicture.Image.RotateFlip(RotateFlipType.Rotate270FlipNone)
			VpPicture.Size = New Size(VpPicture.Height, VpPicture.Width)
			VpPicture.Location = New Point(VpPicture.Location.X, VpPicture.Location.Y + Math.Abs(VpPicture.Height - VpPicture.Width))
		End If
		If Not VpStatic Then
			VpCard.Tapped = Not VpCard.Tapped
		End If
	End Sub
	Private Sub ShowContextMenu(VpPictureBox As PictureBox, VpPoint As Point)
	Dim VpCurCard As clsPlateauCard
		VmCurrentPicture = VpPictureBox
		VpCurCard = VmCurrentPicture.Tag
		Me.cmnuName.Text = VpCurCard.NameVF
		Me.cmnuAttachTo.DropDownItems.Clear
		For Each VpCard As clsPlateauCard In VmPlateauPartie.Field
			If Not VpCard Is VpCurCard Then
				Me.cmnuAttachTo.DropDownItems.Add(VpCard.NameVF, Nothing, AddressOf CmnuAttachToClick)
			End If
		Next VpCard
		Me.cmnuCardContext.Show(VpPictureBox, VpPoint)
	End Sub
	Private Sub ManageResize
		Me.splitV1.SplitterDistance = Me.splitV1.Width / 6
		Me.splitH1.SplitterDistance = Me.splitH1.Height / 3
		Me.splitH2.SplitterDistance = Me.splitH2.Height / 2
		Me.splitH3.SplitterDistance = Me.splitH3.Height / 3
		Me.splitH4.SplitterDistance = Me.splitH4.Height / 2
		Call Me.ManageReDraw
	End Sub
	Sub PictureBoxPaint(sender As Object, e As PaintEventArgs)
	Dim VpCard As clsPlateauCard = sender.Tag
	Dim VpDiameter As Single
	Dim VpLeftToDraw As Integer
	Dim VpLevel As Integer
	Dim VpItem As Integer
	Dim VpX As Single
	Dim VpY As Single
		'Gestion d'éventuels marqueurs sur la carte
		If VpCard.Counters > 0 Then
			VpDiameter = clsModule.CgCounterDiametr_px * Math.Max(sender.Width, sender.Height) / clsModule.CgMTGCardHeight_px
			VpLeftToDraw = VpCard.Counters
			VpLevel = 0
			VpItem = 0
			While VpLeftToDraw > 0
				VpX = sender.Width  / 2 + (VpLevel + 1) * VpDiameter * Math.Cos(2 * VpItem * Math.PI / 2 ^ (VpLevel + 1))
				VpY = sender.Height / 2 - (VpLevel + 1) * VpDiameter * Math.Sin(2 * VpItem * Math.PI / 2 ^ (VpLevel + 1))
				e.Graphics.FillEllipse(Brushes.DarkBlue,        CInt(VpX), CInt(VpY), VpDiameter, VpDiameter)
				e.Graphics.DrawEllipse(New Pen(Color.Black, 2), CInt(VpX), CInt(VpY), VpDiameter, VpDiameter)
				If VpItem = 2 ^ (VpLevel + 1) - 1 Then
					VpLevel += 1
					VpItem = 0
				Else
					VpItem += 1
				End If
				VpLeftToDraw -= 1
			End While
		End If
	End Sub
	Sub FrmPlateauLoad(sender As Object, e As EventArgs)
		Call Me.ManageResize
		Me.Text = clsModule.CgPlateau + VmRestrictionTXT
	End Sub
	Sub FrmPlateauResizeEnd(sender As Object, e As EventArgs)
		Call Me.ManageResize
	End Sub
	Sub BtNewPartieClick(sender As Object, e As EventArgs)
		Me.btLives.Text = "Vies"
		Me.btPoisons.Text = "Poisons"
		VmPlateauPartie.Mulligan = 0
		Call VmPlateauPartie.BeginPlateauPartie
		Call Me.ManageReDraw
	End Sub
	Sub BtMulliganClick(sender As Object, e As EventArgs)
		Me.btLives.Text = "Vies"
		Me.btPoisons.Text = "Poisons"		
		VmPlateauPartie.Mulligan = Math.Min(VmPlateauPartie.Mulligan + 1, clsModule.CgNMain - 1)
		Call VmPlateauPartie.BeginPlateauPartie
		Call Me.ManageReDraw
	End Sub
	Sub BtLivesMouseUp(sender As Object, e As MouseEventArgs)
		If e.Button = MouseButtons.Left Then
			VmPlateauPartie.Lives -= 1
		Else
			VmPlateauPartie.Lives += 1
		End If
		Me.btLives.Text = VmPlateauPartie.Lives.ToString
	End Sub
	Sub BtPoisonsMouseUp(sender As Object, e As MouseEventArgs)
		If e.Button = MouseButtons.Left Then
			VmPlateauPartie.Poisons += 1
		Else
			VmPlateauPartie.Poisons -= 1
		End If
		Me.btPoisons.Text = VmPlateauPartie.Poisons.ToString		
	End Sub	
	Sub BtBibliRevealClick(sender As Object, e As EventArgs)
		Me.btBibliReveal.Checked = Not Me.btBibliReveal.Checked
		Call Me.ManageReDraw
	End Sub
	Sub BtFieldUntapAllClick(sender As Object, e As EventArgs)
		For Each VpCard As clsPlateauCard In VmPlateauPartie.Field
			VpCard.Tapped = False
		Next VpCard
		Call Me.ManageReDraw
	End Sub
	Sub BtBibliSearchClick(sender As Object, e As EventArgs)
	Dim VpStr As String = InputBox("Rechercher dans la bibliothèque (VO ou VF) :", "Recherche", clsModule.CgCard)
	Dim VpFound As Boolean = False
	Dim VpTmp As clsPlateauCard = Nothing
	Dim VpIndex As Integer
		If VpStr.Trim <> "" Then
			With VmPlateauPartie
				'Cherche dans la bibliothèque la carte spécifiée par l'utilisateur
				For Each VpCard As clsPlateauCard In .Bibli
					If VpCard.NameVF.ToLower.Contains(VpStr.ToLower) Or VpCard.NameVO.ToLower.Contains(VpStr.ToLower) Then
						VpIndex = .Bibli.IndexOf(VpCard)
						VpTmp = VpCard
						VpFound = True
						Exit For
					End If
				Next VpCard
				'Si on l'a trouvée, on la permute avec la carte actuellement au-dessus de la bibliothèque
				If VpFound Then
					.Bibli.Item(VpIndex) = .Bibli.Item(0)
					.Bibli.Item(0) = VpTmp
					Call Me.ManageReDraw
					Call clsModule.ShowInformation(.BibliTop.ToString + " a été placé(e) sur le dessus de la bibliothèque.")
				End If
			End With
		End If
	End Sub
	Sub CmnuCountersAddClick(sender As Object, e As EventArgs)
	Dim VpCard As clsPlateauCard = VmCurrentPicture.Tag
		VpCard.Counters = VpCard.Counters + 1
		VmCurrentPicture.Invalidate
	End Sub
	Sub CmnuCountersSubClick(sender As Object, e As EventArgs)
	Dim VpCard As clsPlateauCard = VmCurrentPicture.Tag
		VpCard.Counters = Math.Max(0, VpCard.Counters - 1)
		VmCurrentPicture.Invalidate
	End Sub
	Sub CmnuCountersRemoveClick(sender As Object, e As EventArgs)
	Dim VpCard As clsPlateauCard = VmCurrentPicture.Tag
		VpCard.Counters = 0
		VmCurrentPicture.Invalidate
	End Sub
	Sub CmnuTapUntapClick(sender As Object, e As EventArgs)
		Call Me.ManageTap(VmCurrentPicture)
		VmCurrentPicture.BringToFront
	End Sub
	Sub CmnuSendToClick(sender As Object, e As EventArgs)
		
	End Sub
	Sub CmnuAttachToClick(sender As Object, e As EventArgs)
	
	End Sub
	Sub BtBibliShuffleClick(sender As Object, e As EventArgs)
		Call clsPlateauPartie.Shuffle(VmPlateauPartie.Bibli)
	End Sub
	Sub BtMainShuffleClick(sender As Object, e As EventArgs)
		Call clsPlateauPartie.Shuffle(VmPlateauPartie.Main)
		Call Me.ManageReDraw
	End Sub
	Sub CardBibliDoubleClick(sender As Object, e As EventArgs)
	Dim VpCard As clsPlateauCard = sender.Tag
		VpCard.Hidden = False
		VmPlateauPartie.Main.Add(VpCard)
		VmPlateauPartie.Bibli.Remove(VpCard)
		Call VmPlateauPartie.SortAll
		Call Me.ManageReDraw
	End Sub
	Sub CardGraveyardDoubleClick(sender As Object, e As EventArgs)
		
	End Sub
	Sub CardExilDoubleClick(sender As Object, e As EventArgs)
		
	End Sub
	Sub CardRegardDoubleClick(sender As Object, e As EventArgs)
	Dim VpCard As clsPlateauCard = sender.Tag
		VmPlateauPartie.Main.Add(VpCard)
		VmPlateauPartie.Regard.Remove(VpCard)
		Call VmPlateauPartie.SortAll
		Call Me.ManageReDraw
	End Sub
	Sub CardMainDoubleClick(sender As Object, e As EventArgs)
	Dim VpCard As clsPlateauCard = sender.Tag
		If VpCard.IsAPermanent Then
			VmPlateauPartie.Field.Add(VpCard)
		Else
			VmPlateauPartie.Graveyard.Add(VpCard)
		End If
		VmPlateauPartie.Main.Remove(VpCard)
		Call VmPlateauPartie.SortAll
		Call Me.ManageReDraw
	End Sub
	Sub CardFieldDoubleClick(sender As Object, e As EventArgs)
	Dim VpCard As clsPlateauCard = sender.Tag
		VpCard.Tapped = False
		VmPlateauPartie.Graveyard.Add(VpCard)
		VmPlateauPartie.Field.Remove(VpCard)
		Call VmPlateauPartie.SortAll
		Call Me.ManageReDraw
	End Sub
	Sub CardBibliMouseUp(sender As Object, e As MouseEventArgs)
		
	End Sub
	Sub CardGraveyardMouseUp(sender As Object, e As MouseEventArgs)

	End Sub
	Sub CardExilMouseUp(sender As Object, e As MouseEventArgs)

	End Sub
	Sub CardRegardMouseUp(sender As Object, e As MouseEventArgs)
	Dim VpPicture As PictureBox = sender
	Dim VpCard As clsPlateauCard = VpPicture.Tag
		If e.Button = MouseButtons.Left Then
			If VpCard.Hidden Then
				VpCard.Hidden = False
				Call Me.ManageReDraw
			Else
				VpPicture.BringToFront
			End If
		End If
	End Sub
	Sub CardMainMouseUp(sender As Object, e As MouseEventArgs)
	Dim VpPicture As PictureBox = sender
		If e.Button = MouseButtons.Left Then
			VpPicture.BringToFront
		End If
	End Sub
	Sub CardFieldMouseUp(sender As Object, e As MouseEventArgs)
	Dim VpPicture As PictureBox = sender
		If e.Button = MouseButtons.Left Then
			Call Me.ManageTap(VpPicture)
			VpPicture.BringToFront
		Else
			Call Me.ShowContextMenu(sender, e.Location)
		End If
	End Sub
End Class
Public Class clsPlateauPartie
	Private VmDeck As New List(Of clsPlateauCard)
	Private VmBibli As New List(Of clsPlateauCard)
	Private VmRegard As New List(Of clsPlateauCard)
	Private VmMain As New List(Of clsPlateauCard)
	Private VmField As New List(Of clsPlateauCard)
	Private VmGraveyard As New List(Of clsPlateauCard)
	Private VmExil As New List(Of clsPlateauCard)
	Private VmMulligan As Integer = 0
	Private VmLives As Integer
	Private VmPoisons As Integer
	Public Sub New(VpSource As String, VpRestriction As String)
	'-------------------
	'Construction du jeu
	'-------------------
	Dim VpSQL As String
		VpSQL = "Select Card.Title, " + VpSource + ".Items, CardFR.TitleFR, Card.Type From (Card Inner Join " + VpSource + " On " + VpSource + ".EncNbr = Card.EncNbr) Inner Join CardFR On Card.EncNbr = CardFR.EncNbr Where "
		VpSQL = VpSQL + VpRestriction
		VpSQL = clsModule.TrimQuery(VpSQL)
		VgDBCommand.CommandText = VpSQL
		VgDBReader = VgDBCommand.ExecuteReader
		With VgDBReader
			While .Read
				Me.AddCard(.GetString(0), .GetString(2), .GetInt32(1), .GetString(3))
			End While
			.Close
		End With
	End Sub
	Public Sub BeginPlateauPartie
	'-------------------------------
	'Démarrage d'une nouvelle partie
	'-------------------------------
		VmBibli.Clear
		VmRegard.Clear
		VmMain.Clear
		VmField.Clear
		VmGraveyard.Clear
		VmExil.Clear
		For Each VpCard As clsPlateauCard In VmDeck
			Call VpCard.ReInit
			VmBibli.Add(VpCard)
		Next VpCard
		Call Shuffle(VmBibli)
		VmMain.AddRange(VmBibli.GetRange(0, clsModule.CgNMain - VmMulligan))
		VmBibli.RemoveRange(0, clsModule.CgNMain - VmMulligan)
		For Each VpCard As clsPlateauCard In VmMain
			VpCard.Hidden = False
		Next VpCard
		Call Me.SortAll
		VmLives = clsModule.CgNLives
		VmPoisons = 0
	End Sub
	Public Sub SortAll
	'-----------------------------------------
	'Tri les cartes des listes selon leur type
	'-----------------------------------------
	Dim VpComparer As New clsPlateauCardComparer
		VmRegard.Sort(VpComparer)
		'VmMain.Sort(VpComparer)
		VmField.Sort(VpComparer)
	End Sub
	Private Sub AddCard(VpName As String, VpNameFR As String, VpCount As Integer, VpType As String)
	'--------------------
	'Construction du deck
	'--------------------
		For VpI As Integer = 1 To VpCount
			VmDeck.Add(New clsPlateauCard(VpName, VpNameFR, VpType))
		Next VpI
	End Sub
	Public Shared Sub Shuffle(ByRef VpListe As List(Of clsPlateauCard))
	'----------------------------------------
	'Mélange la sélection passée en paramètre
	'----------------------------------------
	Dim VpI As Integer
	Dim VpRandomPos As New SortedList(VpListe.Count)
	Dim VpShuffled As New List(Of clsPlateauCard)(VpListe.Count)
		'Génère un tableau trié de nombres aléatoires
		For VpI = 0 To VpListe.Count - 1
			VpRandomPos.Add(clsModule.VgRandom.NextDouble, VpI)
		Next VpI
		'Réordonne les cartes en conséquence
		VpI = 0
		For Each VpPos As Integer In VpRandomPos.Values
			VpShuffled.Insert(VpI, VpListe.Item(VpPos))
			VpI = VpI + 1
		Next VpPos
		VpListe = VpShuffled
	End Sub
	Public Property Bibli As List(Of clsPlateauCard)
		Get
			Return VmBibli
		End Get
		Set (VpBibli As List(Of clsPlateauCard))
			VmBibli = VpBibli
		End Set
	End Property
	Public Property Regard As List(Of clsPlateauCard)
		Get
			Return VmRegard
		End Get
		Set (VpRegard As List(Of clsPlateauCard))
			VmRegard = VpRegard
		End Set
	End Property
	Public Property Main As List(Of clsPlateauCard)
		Get
			Return VmMain
		End Get
		Set (VpMain As List(Of clsPlateauCard))
			VmMain = VpMain
		End Set
	End Property
	Public Property Field As List(Of clsPlateauCard)
		Get
			Return VmField
		End Get
		Set (VpField As List(Of clsPlateauCard))
			VmField = VpField
		End Set
	End Property
	Public Property Graveyard As List(Of clsPlateauCard)
		Get
			Return VmGraveyard
		End Get
		Set (VpGraveyard As List(Of clsPlateauCard))
			VmGraveyard = VpGraveyard
		End Set
	End Property
	Public Property Exil As List(Of clsPlateauCard)
		Get
			Return VmExil
		End Get
		Set (VpExil As List(Of clsPlateauCard))
			VmExil = VpExil
		End Set
	End Property
	Public ReadOnly Property BibliTop As clsPlateauCard
		Get
			If VmBibli.Count > 0 Then
				Return VmBibli.Item(0)
			Else
				Return Nothing
			End If
		End Get
	End Property
	Public ReadOnly Property GraveyardTop As clsPlateauCard
		Get
			If VmGraveyard.Count > 0 Then
				Return VmGraveyard.Item(VmGraveyard.Count - 1)
			Else
				Return Nothing
			End If
		End Get
	End Property
	Public Property Mulligan As Integer
		Get
			Return VmMulligan
		End Get
		Set (VpMulligan As Integer)
			VmMulligan = VpMulligan
		End Set
	End Property
	Public Property Lives As Integer
		Get
			Return VmLives
		End Get
		Set (VpLives As Integer)
			VmLives = VpLives
		End Set
	End Property
	Public Property Poisons As Integer
		Get
			Return VmPoisons
		End Get
		Set (VpPoisons As Integer)
			VmPoisons = VpPoisons
		End Set
	End Property
	Private Class clsPlateauCardComparer
		Implements IComparer(Of clsPlateauCard)
		Public Function Compare(ByVal x As clsPlateauCard, ByVal y As clsPlateauCard) As Integer Implements IComparer(Of clsPlateauCard).Compare
			Return String.Compare(x.MyType, y.MyType)
		End Function
	End Class
End Class
Public Class clsPlateauCard
	Private VmCardName As String
	Private VmCardNameFR As String
	Private VmCardType As String
	Private VmTapped As Boolean
	Private VmHidden As Boolean
	Private VmCounters As Integer
	Private VmAttachedTo As clsPlateauCard
	Private VmAttachments As New List(Of clsPlateauCard)
	Public Sub New(VpName As String, VpNameFR As String, VpType As String)
		VmCardName = VpName
		VmCardNameFR = VpNameFR
		VmCardType = VpType
		Call Me.ReInit
	End Sub
	Public Sub ReInit
		VmTapped = False
		VmHidden = True
		VmCounters = 0
		VmAttachedTo = Nothing
		VmAttachments.Clear
	End Sub
	Public Overrides Function ToString() As String
		Return VmCardNameFR + " (" + VmCardName + ")"
	End Function
	Public ReadOnly Property NameVF As String
		Get
			Return VmCardNameFR
		End Get
	End Property
	Public ReadOnly Property NameVO As String
		Get
			Return VmCardName
		End Get
	End Property
	Public ReadOnly Property PicturePath As String
		Get
			If Not VmHidden Then
				Return Path.GetTempPath + clsModule.CgTemp + "\" + clsModule.AvoidForbiddenChr(VmCardName) + ".jpg"
			Else
				Return Application.StartupPath + clsModule.CgMagicBack
			End If
		End Get
	End Property
	Public ReadOnly Property IsAPermanent As Boolean
		Get
			Return Not ( VmCardType = "I" Or VmCardType = "N" Or VmCardType = "S" )
		End Get
	End Property
	Public ReadOnly Property MyType As String
		Get
			Return VmCardType
		End Get
	End Property
	Public Property Tapped As Boolean
		Get
			Return VmTapped
		End Get
		Set (VpTapped As Boolean)
			VmTapped = VpTapped
		End Set
	End Property
	Public Property Hidden As Boolean
		Get
			Return VmHidden
		End Get
		Set (VpHidden As Boolean)
			VmHidden = VpHidden
		End Set
	End Property
	Public Property Counters As Integer
		Get
			Return VmCounters
		End Get
		Set (VpCounters As Integer)
			VmCounters = VpCounters
		End Set
	End Property
End Class