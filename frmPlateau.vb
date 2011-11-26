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
	Public Sub New(VpOwner As MainForm)
	Dim VpPath As String = Path.GetTempPath + clsModule.CgTemp
		Me.InitializeComponent()
		VmSource = If(VpOwner.FilterCriteria.DeckMode, clsModule.CgSDecks, clsModule.CgSCollection)
		VmRestriction = VpOwner.Restriction
		VmRestrictionTXT = VpOwner.Restriction(True)
		If VmRestrictionTXT.Length > 31 Then
			VmRestrictionTXT = VmRestrictionTXT.Substring(0, 31)
		End If
		If Not Directory.Exists(VpPath) Then
			Directory.CreateDirectory(VpPath)
		End If
		Call clsModule.ExtractPictures(VpPath.Replace("\\", "\"), VmSource, VmRestriction)
		VmPlateauPartie = New clsPlateauPartie(VmSource, VmRestriction)
		Call VmPlateauPartie.BeginPlateauPartie
	End Sub
	Private Sub ManageReDraw
		'Efface les anciennes images
		For Each VpPictureBox As PictureBox In VmPictures
			VpPictureBox.Parent.Controls.Remove(VpPictureBox)
			VpPictureBox.Dispose
		Next VpPictureBox
		VmPictures.Clear
		'Bibliothèque
		Call Me.DrawPicture(VmPlateauPartie.BibliTop, Me.cbarpanelBibli, New EventHandler(AddressOf Me.CardBibliDoubleClick))
		'Cimetière
		Call Me.DrawPicture(VmPlateauPartie.GraveyardTop, Me.cbarpanelGraveyard, New EventHandler(AddressOf Me.CardGraveyardDoubleClick))
		'Exil
		Call Me.DrawPictures(VmPlateauPartie.Exil, Me.cbarpanelExil, New EventHandler(AddressOf Me.CardExilDoubleClick))
		'Regard
		Call Me.DrawPictures(VmPlateauPartie.Regard, Me.cbarpanelRegard, New EventHandler(AddressOf Me.CardRegardDoubleClick))
		'Main
		Call Me.DrawPictures(VmPlateauPartie.Main, Me.cbarpanelMain, New EventHandler(AddressOf Me.CardMainDoubleClick))
		'Champ de bataille
		Call Me.DrawPictures(VmPlateauPartie.Field, Me.cbarpanelField, New EventHandler(AddressOf Me.CardFieldDoubleClick))
	End Sub
	Private Sub DrawPicture(VpCard As clsPlateauCard, VpParent As Control, VpIndex As Integer, VpCount As Integer, VpDoubleClickHandler As EventHandler)
	Dim VpPicture As PictureBox
	Dim VpW As Integer
	Dim VpH As Integer
		If Not VpCard Is Nothing Then
			VpPicture = New PictureBox
			If 300 - VpParent.Height > 210 - VpParent.Width Then
				VpH = Math.Min(VpParent.Height, 300)
				VpW = 210 * VpH / 300
			Else
				VpW = Math.Min(VpParent.Width, 210)
				VpH = 300 * VpW / 210
			End If
			With VpPicture
				.Location = New System.Drawing.Point(VpParent.Width * VpIndex / VpCount, 0)
				.Size = New System.Drawing.Size(VpW, VpH)
				.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
				.Image = Image.FromFile(VpCard.PicturePath)
				.Tag = VpCard
				AddHandler .DoubleClick, VpDoubleClickHandler
			End With
			VpParent.Controls.Add(VpPicture)
			VmPictures.Add(VpPicture)
		End If
	End Sub
	Private Sub DrawPicture(VpCard As clsPlateauCard, VpParent As Control, VpDoubleClickHandler As EventHandler)
		Call Me.DrawPicture(VpCard, VpParent, 0, 1, VpDoubleClickHandler)
	End Sub
	Private Sub DrawPictures(VpCards As List(Of clsPlateauCard), VpParent As Control, VpDoubleClickHandler As EventHandler)
	Dim VpCount As Integer = VpCards.Count
		For VpI As Integer = 0 To VpCount - 1
			Call Me.DrawPicture(VpCards.Item(VpI), VpParent, VpI, VpCount, VpDoubleClickHandler)
		Next VpI
	End Sub
	Private Sub ManageResize
		Me.splitV1.SplitterDistance = Me.splitV1.Width / 6
		Me.splitH1.SplitterDistance = Me.splitH1.Height / 3
		Me.splitH2.SplitterDistance = Me.splitH2.Height / 2
		Me.splitH3.SplitterDistance = Me.splitH3.Height / 3
		Me.splitH4.SplitterDistance = Me.splitH4.Height / 2
		Call Me.ManageReDraw
	End Sub
	Sub FrmPlateauLoad(sender As Object, e As EventArgs)
		Call Me.ManageResize
		Me.Text = clsModule.CgPlateau + VmRestrictionTXT
	End Sub
	Sub FrmPlateauResizeEnd(sender As Object, e As EventArgs)
		Call Me.ManageResize
	End Sub
	Sub BtBibliShuffleActivate(sender As Object, e As EventArgs)
		Call clsPlateauPartie.Shuffle(VmPlateauPartie.Bibli)
	End Sub
	Sub CardBibliDoubleClick(sender As Object, e As EventArgs)
	Dim VpCard As clsPlateauCard = sender.Tag
		VmPlateauPartie.Main.Add(VpCard)
		VmPlateauPartie.Bibli.Remove(VpCard)
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
		Call Me.ManageReDraw
	End Sub
	Sub CardFieldDoubleClick(sender As Object, e As EventArgs)
	Dim VpCard As clsPlateauCard = sender.Tag
		VmPlateauPartie.Graveyard.Add(VpCard)
		VmPlateauPartie.Field.Remove(VpCard)
		Call Me.ManageReDraw
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
			VmBibli.Add(VpCard)
		Next VpCard
		Call Shuffle(VmBibli)
		VmMain.AddRange(VmBibli.GetRange(0, clsModule.CgNMain))
		VmBibli.RemoveRange(0, clsModule.CgNMain)
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
	Public ReadOnly Property Bibli As List(Of clsPlateauCard)
		Get
			Return VmBibli
		End Get
	End Property
	Public ReadOnly Property Regard As List(Of clsPlateauCard)
		Get
			Return VmRegard
		End Get
	End Property
	Public ReadOnly Property Main As List(Of clsPlateauCard)
		Get
			Return VmMain
		End Get
	End Property
	Public ReadOnly Property Field As List(Of clsPlateauCard)
		Get
			Return VmField
		End Get
	End Property
	Public ReadOnly Property Graveyard As List(Of clsPlateauCard)
		Get
			Return VmGraveyard
		End Get
	End Property
	Public ReadOnly Property Exil As List(Of clsPlateauCard)
		Get
			Return VmExil
		End Get
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
				Return VmGraveyard.Item(0)
			Else
				Return Nothing
			End If
		End Get
	End Property
End Class
Public Class clsPlateauCard
	Private VmCardName As String
	Private VmCardNameFR As String
	Private VmCardType As String
	Private VmTapped As Boolean
	Public Sub New(VpName As String, VpNameFR As String, VpType As String)
		VmCardName = VpName
		VmCardNameFR = VpNameFR
		VmCardType = VpType
		VmTapped = False
	End Sub
	Public ReadOnly Property PicturePath As String
		Get
			Return Path.GetTempPath + clsModule.CgTemp + "\" + clsModule.AvoidForbiddenChr(VmCardName) + ".jpg"
		End Get
	End Property
	Public ReadOnly Property IsAPermanent As Boolean
		Get
			Return Not ( VmCardType = "I" Or VmCardType = "N" Or VmCardType = "S" )
		End Get
	End Property
End Class