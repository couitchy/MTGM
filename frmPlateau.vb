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
	#Region "Déclarations"
	Private VmSource As String
	Private VmRestriction As String
	Private VmRestrictionTXT As String
	Private VmPlateauPartie As clsPlateauPartie
	Private VmPictures As New List(Of PictureBox)
	Private VmCurrentPicture As PictureBox
	#End Region
	#Region "Méthodes"
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
			Call Me.DrawPicture(.BibliTop, True, Me.panelBibli, New EventHandler(AddressOf Me.CardBibliDoubleClick), New MouseEventHandler(AddressOf Me.CardBibliMouseUp))
			'Cimetière
			Call Me.DrawPicture(.GraveyardTop, True, Me.panelGraveyard, New EventHandler(AddressOf Me.CardGraveyardDoubleClick), New MouseEventHandler(AddressOf Me.CardGraveyardMouseUp))
			'Exil
			Call Me.DrawPicture(.ExilTop, True, Me.panelExil, New EventHandler(AddressOf Me.CardExilDoubleClick), New MouseEventHandler(AddressOf Me.CardExilMouseUp))
			'Regard
			Call Me.DrawPictures(.Regard, True, Me.panelRegard, New EventHandler(AddressOf Me.CardRegardDoubleClick), New MouseEventHandler(AddressOf Me.CardRegardMouseUp))
			'Main
			Call Me.DrawPictures(.Main, True, Me.panelMain, New EventHandler(AddressOf Me.CardMainDoubleClick), New MouseEventHandler(AddressOf Me.CardMainMouseUp))
			'Champ de bataille
			Call Me.DrawPictures(.Field, False, Me.panelField, New EventHandler(AddressOf Me.CardFieldDoubleClick), New MouseEventHandler(AddressOf Me.CardFieldMouseUp))
		End With
	End Sub
	Private Sub DrawPicture(VpCard As clsPlateauCard, VpUntap As Boolean, VpParent As Control, VpIndexH As Integer, VpCount As Integer, VpDoubleClickHandler As EventHandler, VpMouseUpHandler As MouseEventHandler)
	'---------------------------------
	'Dessin d'une carte sur le plateau
	'---------------------------------
	Dim VpW As Integer
	Dim VpH As Integer
	Dim VpEffectiveCardHeight_px As Integer
		If Not VpCard Is Nothing Then
			If VpUntap Then
				VpCard.Tapped = False
			End If
			'Détermine le côté limitant pour l'affichage optimal avec respect des proportions
			VpEffectiveCardHeight_px = CInt(CgMTGCardHeight_px * (1 + VpCard.Attachments.Count * CgChevauchFactor))	'/!\ il faut prendre en compte les cartes attachées pour avoir la hauteur totale
			'Si c'est la hauteur qui limite
			If VpEffectiveCardHeight_px - VpParent.Height > CgMTGCardWidth_px - VpParent.Width Then
				VpH = Math.Min(VpParent.Height, VpEffectiveCardHeight_px)				'ici VpH vaut la hauteur cumulée (avec les cartes attachées)
				VpH = CInt(VpH / (1 + VpCard.Attachments.Count * CgChevauchFactor))		'on divise pour trouver la hauteur de la carte hôte seule
				VpW = CgMTGCardWidth_px * VpH / CgMTGCardHeight_px
			'Si c'est la largeur qui limite
			Else
				VpW = Math.Min(VpParent.Width, CgMTGCardWidth_px)
				VpH = CgMTGCardHeight_px * VpW / CgMTGCardWidth_px
			End If
			'Dessin carte hôte
			Call Me.EffectiveDraw(VpCard, VpW, VpH, VpIndexH, VpCount, VpCard.Attachments.Count, VpParent, VpDoubleClickHandler, VpMouseUpHandler)
			'Dessin cartes attachées éventuelles
			If VpCard.Attachments.Count > 0 Then
				For VpIndexV As Integer = VpCard.Attachments.Count - 1 To 0 Step - 1	'on parcourt la liste à l'envers pour avoir l'affichage visuel dans le bon ordre (zorder)
					Call Me.EffectiveDraw(VpCard.Attachments.Item(VpIndexV), VpW, VpH, VpIndexH, VpCount, VpIndexV, VpParent, VpDoubleClickHandler, VpMouseUpHandler)
				Next VpIndexV
			End If
		End If
	End Sub
	Private Sub EffectiveDraw(VpCard As clsPlateauCard, VpW As Integer, VpH As Integer, VpIndexH As Integer, VpCount As Integer, VpIndexV As Integer, VpParent As Control, VpDoubleClickHandler As EventHandler, VpMouseUpHandler As MouseEventHandler)
	Dim VpPicture As PictureBox
		VpPicture = New PictureBox
		With VpPicture
			.Location = New System.Drawing.Point(If(VpIndexH = 0, 0, VpIndexH * (VpParent.Width - VpW) / (VpCount - 1)), CgChevauchFactor * VpH * VpIndexV)
			.Size = New System.Drawing.Size(VpW, VpH)
			.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
			.Image = Image.FromFile(VpCard.PicturePath)
			.Tag = VpCard
			AddHandler .DoubleClick, VpDoubleClickHandler
			AddHandler .MouseUp, VpMouseUpHandler
			AddHandler .MouseDown, New MouseEventHandler(AddressOf Me.CardMouseDown)
			AddHandler .Paint, New PaintEventHandler(AddressOf Me.PictureBoxPaint)
		End With
		'Gestion carte engagée / dégagée
		Call Me.ManageTap(VpPicture, True)
		VpParent.Controls.Add(VpPicture)
		VmPictures.Add(VpPicture)		'conserve la référence
	End Sub
	Private Sub DrawPicture(VpCard As clsPlateauCard, VpUntap As Boolean, VpParent As Control, VpDoubleClickHandler As EventHandler, VpMouseUpHandler As MouseEventHandler)
		Call Me.DrawPicture(VpCard, VpUntap, VpParent, 0, 1, VpDoubleClickHandler, VpMouseUpHandler)
	End Sub
	Private Sub DrawPictures(VpCards As List(Of clsPlateauCard), VpUntap As Boolean, VpParent As Control, VpDoubleClickHandler As EventHandler, VpMouseUpHandler As MouseEventHandler)
	Dim VpCount As Integer = 0
	Dim VpIndex As Integer = 0
		'On est obligé de faire une première passe pour déterminer VpCount
		For Each VpCard As clsPlateauCard In VpCards
			If Not VpCard.IsAttached Then
				VpCount += 1
			End If
		Next VpCard
		For Each VpCard As clsPlateauCard In VpCards
			If Not VpCard.IsAttached Then
				Call Me.DrawPicture(VpCard, VpUntap, VpParent, VpIndex, VpCount, VpDoubleClickHandler, VpMouseUpHandler)
				VpIndex += 1
			End If
		Next VpCard
	End Sub
	Private Sub SearchIn(VpListe As List(Of clsPlateauCard), VpPosition As Integer)
	'-----------------------------------------------------------------------------------------------------------------------------
	'Recherche une carte demandée par l'utilisateur afin de la placer au sommet de la collection (zone) à laquelle elle appartient
	'-----------------------------------------------------------------------------------------------------------------------------
	Dim VpStr As String = InputBox("Rechercher dans la zone (VO ou VF) :", "Recherche", clsModule.CgCard)
	Dim VpFound As Boolean = False
	Dim VpTmp As clsPlateauCard = Nothing
	Dim VpIndex As Integer
		If VpStr.Trim <> "" Then
			'Cherche dans la zone la carte spécifiée par l'utilisateur
			For Each VpCard As clsPlateauCard In VpListe
				If VpCard.NameVF.ToLower.Contains(VpStr.ToLower) Or VpCard.NameVO.ToLower.Contains(VpStr.ToLower) Then
					VpIndex = VpListe.IndexOf(VpCard)
					VpTmp = VpCard
					VpFound = True
					Exit For
				End If
			Next VpCard
			'Si on l'a trouvée, on la permute avec la carte actuellement au-dessus de la zone
			If VpFound Then
				VpListe.Item(VpIndex) = VpListe.Item(VpPosition)
				VpListe.Item(VpPosition) = VpTmp
				Call Me.ManageReDraw
				Call clsModule.ShowInformation(VpTmp.ToString + " a été placé(e) sur le dessus de la zone.")
			End If
		End If
	End Sub
	Private Sub ManageTap(VpPicture As PictureBox, Optional VpStatic As Boolean = False)
	'------------------------------------------------------------------
	'Gestion de l'orientation de la carte (engagée @ 90°, dégagée @ 0°)
	'------------------------------------------------------------------
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
	Private Sub ManageContextMenu(VpField As Boolean)
	'-----------------------------------------------------------------------------------------------------------------
	'Gestion des éléments visibles du menu contextuel en fonction de la zone dans laquelle se trouve la carte courante
	'-----------------------------------------------------------------------------------------------------------------
		Me.cmnuSeparator0.Visible = VpField
		Me.cmnuAttachTo.Visible   = VpField
		Me.cmnuDetachFrom.Visible = VpField
		Me.cmnuCounters.Visible   = VpField
		Me.cmnuTapUntap.Visible   = VpField
	End Sub
	Private Sub ShowContextMenu(VpPictureBox As PictureBox, VpPoint As Point)
	'-------------------------------------
	'Affichage effectif du menu contextuel
	'-------------------------------------
	Dim VpCurCard As clsPlateauCard
	Dim VpDropDown As ToolStripItem
		VmCurrentPicture = VpPictureBox
		VpCurCard = VmCurrentPicture.Tag
		'Nom de la carte (titre du menu contextuel)
		If VpCurCard.Owner Is VmPlateauPartie.Bibli And Not Me.btBibliReveal.Checked Then
			Me.cmnuName.Text = "(Face cachée)"
		Else
			Me.cmnuName.Text = VpCurCard.NameVF
		End If
		'Liste des autres cartes auxquelles on pourrait potentiellement attacher la carte courante
		Me.cmnuAttachTo.DropDownItems.Clear
		If VpCurCard.Attachments.Count = 0 Then									'on peut s'attacher à une autre carte que si personne n'est attaché à soi
			For Each VpCard As clsPlateauCard In VmPlateauPartie.Field
				If Not VpCard Is VpCurCard AndAlso Not VpCard.IsAttached Then	'on ne peut ni s'attacher à soi-même ni à une carte déjà attachée à une autre carte
					VpDropDown = Me.cmnuAttachTo.DropDownItems.Add(VpCard.NameVF, Nothing, AddressOf CmnuAttachToClick)
					VpDropDown.Tag = VpCard										'conserve la référence de l'hôte potentiel
				End If
			Next VpCard
		End If
		'Affichage effectif
		Me.cmnuCardContext.Show(VpPictureBox, VpPoint)
	End Sub
	Private Sub ManageResize
	'----------------------------------------------------------------------------
	'Recalcule la position des splitters après un redimensionnement de la fenêtre
	'----------------------------------------------------------------------------
		Me.splitV1.SplitterDistance = Me.splitV1.Width / 6
		Me.splitH1.SplitterDistance = Me.splitH1.Height / 3
		Me.splitH2.SplitterDistance = Me.splitH2.Height / 2
		Me.splitH3.SplitterDistance = Me.splitH3.Height / 3
		Me.splitH4.SplitterDistance = Me.splitH4.Height / 2
		Call Me.ManageReDraw
	End Sub
	#End Region
	#Region "Evènements"
	Sub PictureBoxPaint(sender As Object, e As PaintEventArgs)
	'--------------------------------------------------------------
	'Dessine les éventuels marqueurs présents sur la carte courante
	'--------------------------------------------------------------
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
		Call Me.SearchIn(VmPlateauPartie.Bibli, 0)
	End Sub
	Sub BtGraveyardSearchClick(sender As Object, e As EventArgs)
		Call Me.SearchIn(VmPlateauPartie.Graveyard, VmPlateauPartie.Graveyard.Count - 1)
	End Sub
	Sub BtExilSearchClick(sender As Object, e As EventArgs)
		Call Me.SearchIn(VmPlateauPartie.Exil, VmPlateauPartie.Exil.Count - 1)
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
	Dim VpCurCard As clsPlateauCard = VmCurrentPicture.Tag
		Select Case CType(sender, ToolStripMenuItem).Name
			Case Me.cmnuSendToBibliBottom.Name
				Call VpCurCard.SendTo(VmPlateauPartie.Bibli)
				VpCurCard.Hidden = True
			Case Me.cmnuSendToBibliTop.Name
				Call VpCurCard.SendTo(VmPlateauPartie.Bibli, True)
				VpCurCard.Hidden = True
			Case Me.cmnuSendToExil.Name
				Call VpCurCard.SendTo(VmPlateauPartie.Exil)
			Case Me.cmnuSendToField.Name
				Call VpCurCard.SendTo(VmPlateauPartie.Field)
			Case Me.cmnuSendToGraveyard.Name
				Call VpCurCard.SendTo(VmPlateauPartie.Graveyard)
			Case Me.cmnuSendToMain.Name
				Call VpCurCard.SendTo(VmPlateauPartie.Main)
			Case Me.cmnuSendToRegard.Name
				Call VpCurCard.SendTo(VmPlateauPartie.Regard)
			Case Else
		End Select
		Call VmPlateauPartie.SortAll
		Call Me.ManageReDraw
	End Sub
	Sub CmnuAttachToClick(sender As Object, e As EventArgs)
	Dim VpHost As clsPlateauCard = sender.Tag
	Dim VpCurCard As clsPlateauCard = VmCurrentPicture.Tag
		Call VpCurCard.AttachTo(VpHost)
		Call VmPlateauPartie.SortAll
		Call Me.ManageReDraw
	End Sub
	Sub CmnuDetachFromClick(sender As Object, e As EventArgs)
	Dim VpCurCard As clsPlateauCard = VmCurrentPicture.Tag
		Call VpCurCard.AttachTo(Nothing)
		Call VmPlateauPartie.SortAll
		Call Me.ManageReDraw
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
		Call VpCard.SendTo(VmPlateauPartie.Main)
		Call VmPlateauPartie.SortAll
		Call Me.ManageReDraw
	End Sub
	Sub CardGraveyardDoubleClick(sender As Object, e As EventArgs)
		
	End Sub
	Sub CardExilDoubleClick(sender As Object, e As EventArgs)
		
	End Sub
	Sub CardRegardDoubleClick(sender As Object, e As EventArgs)
	Dim VpCard As clsPlateauCard = sender.Tag
		Call VpCard.SendTo(VmPlateauPartie.Main)
		Call VmPlateauPartie.SortAll
		Call Me.ManageReDraw
	End Sub
	Sub CardMainDoubleClick(sender As Object, e As EventArgs)
	Dim VpCard As clsPlateauCard = sender.Tag
		If VpCard.IsAPermanent Then
			Call VpCard.SendTo(VmPlateauPartie.Field)
		Else
			Call VpCard.SendTo(VmPlateauPartie.Graveyard)
		End If
		Call VmPlateauPartie.SortAll
		Call Me.ManageReDraw
	End Sub
	Sub CardFieldDoubleClick(sender As Object, e As EventArgs)
	Dim VpCard As clsPlateauCard = sender.Tag
		Call VpCard.SendTo(VmPlateauPartie.Graveyard)
		Call VmPlateauPartie.SortAll
		Call Me.ManageReDraw
	End Sub
	Sub CardBibliMouseUp(sender As Object, e As MouseEventArgs)
		If e.Button = MouseButtons.Right Then
			Call Me.ManageContextMenu(False)
			Call Me.ShowContextMenu(sender, e.Location)
		End If
	End Sub
	Sub CardGraveyardMouseUp(sender As Object, e As MouseEventArgs)
		If e.Button = MouseButtons.Right Then
			Call Me.ManageContextMenu(False)
			Call Me.ShowContextMenu(sender, e.Location)
		End If
	End Sub
	Sub CardExilMouseUp(sender As Object, e As MouseEventArgs)
		If e.Button = MouseButtons.Right Then
			Call Me.ManageContextMenu(False)
			Call Me.ShowContextMenu(sender, e.Location)
		End If
	End Sub
	Sub CardRegardMouseUp(sender As Object, e As MouseEventArgs)
	Dim VpPicture As PictureBox = sender
	Dim VpCard As clsPlateauCard = VpPicture.Tag
		If e.Button = MouseButtons.Left Then
			VpPicture.BringToFront
		Else
			Call Me.ManageContextMenu(False)
			Call Me.ShowContextMenu(sender, e.Location)
		End If
	End Sub
	Sub CardMainMouseUp(sender As Object, e As MouseEventArgs)
	Dim VpPicture As PictureBox = sender
		If e.Button = MouseButtons.Left Then
			VpPicture.BringToFront
		Else
			Call Me.ManageContextMenu(False)
			Call Me.ShowContextMenu(sender, e.Location)
		End If
	End Sub
	Sub CardFieldMouseUp(sender As Object, e As MouseEventArgs)
	Dim VpPicture As PictureBox = sender
		If e.Button = MouseButtons.Left Then
			Call Me.ManageTap(VpPicture)
			VpPicture.BringToFront
		Else
			Call Me.ManageContextMenu(True)
			Call Me.ShowContextMenu(sender, e.Location)
		End If
	End Sub
	Sub CardMouseDown(sender As Object, e As MouseEventArgs)
'	Dim VpPicture As PictureBox = sender
'		VpPicture.DoDragDrop(VpPicture, DragDropEffects.Move)
	End Sub
	Sub PanelFieldDragEnter(sender As Object, e As DragEventArgs)
		If e.Data.GetFormats()(0) = GetType(PictureBox).ToString Then
			e.Effect = DragDropEffects.Move
		Else
			e.Effect = DragDropEffects.None
		End If
	End Sub	
	Sub PanelFieldDragDrop(sender As Object, e As DragEventArgs)
	Dim VpCard As clsPlateauCard = e.Data.GetData(GetType(PictureBox)).Tag
		Call VpCard.SendTo(VmPlateauPartie.Field)
		Call VmPlateauPartie.SortAll
		Call Me.ManageReDraw
	End Sub
	#End Region
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
				Call Me.AddCard(.GetString(0), .GetString(2), .GetInt32(1), .GetString(3))
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
			Call VpCard.ReInit(VmBibli)
			VmBibli.Add(VpCard)
		Next VpCard
		Call Shuffle(VmBibli)
		VmMain.AddRange(VmBibli.GetRange(0, clsModule.CgNMain - VmMulligan))
		VmBibli.RemoveRange(0, clsModule.CgNMain - VmMulligan)
		For Each VpCard As clsPlateauCard In VmMain
			VpCard.Hidden = False
			VpCard.Owner = VmMain
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
			VmDeck.Add(New clsPlateauCard(VmDeck, VpName, VpNameFR, VpType))
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
			VpListe.Item(VpPos).Owner = VpShuffled			'eh oui : quand on mélange la liste, on change de handle, et il faut pas perdre le nouveau !
			VpShuffled.Insert(VpI, VpListe.Item(VpPos))
			VpI = VpI + 1
		Next VpPos
		VpListe = VpShuffled
	End Sub
	#Region "Propriétés"
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
	Public ReadOnly Property ExilTop As clsPlateauCard
		Get
			If VmExil.Count > 0 Then
				Return VmExil.Item(VmExil.Count - 1)
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
	#End Region
	Private Class clsPlateauCardComparer
		Implements IComparer(Of clsPlateauCard)
		Public Function Compare(ByVal x As clsPlateauCard, ByVal y As clsPlateauCard) As Integer Implements IComparer(Of clsPlateauCard).Compare
			Return String.Compare(x.MyType, y.MyType)
		End Function
	End Class
End Class
Public Class clsPlateauCard
	Private VmOwner As List(Of clsPlateauCard)
	Private VmCardName As String
	Private VmCardNameFR As String
	Private VmCardType As String
	Private VmTapped As Boolean
	Private VmHidden As Boolean
	Private VmCounters As Integer
	Private VmAttachedTo As clsPlateauCard
	Private VmAttachments As New List(Of clsPlateauCard)
	Public Sub New(VpOwner As List(Of clsPlateauCard), VpName As String, VpNameFR As String, VpType As String)
		VmCardName = VpName
		VmCardNameFR = VpNameFR
		VmCardType = VpType
		Call Me.ReInit(VpOwner)
	End Sub
	Public Sub ReInit(VpOwner As List(Of clsPlateauCard))
		VmOwner = VpOwner
		VmTapped = False
		VmHidden = True
		VmCounters = 0
		VmAttachedTo = Nothing
		VmAttachments.Clear
	End Sub
	Public Sub SendTo(VpNewOwner As List(Of clsPlateauCard), Optional VpTop As Boolean = False)
	'-----------------------
	'Change la carte de zone
	'-----------------------
		If Not VmOwner Is VpNewOwner Then
			VmHidden = False
			VmOwner.Remove(Me)
			VmOwner = VpNewOwner
			If VpTop Then
				VmOwner.Insert(0, Me)
			Else
				VmOwner.Add(Me)
			End If
			'On doit aussi enlever tout ce qui était attaché / ce à quoi on était attaché
			For Each VpAttachment As clsPlateauCard In VmAttachments
				Call VpAttachment.AttachTo(Nothing)
			Next VpAttachment
			VmAttachedTo = Nothing
			VmAttachments.Clear
		End If
	End Sub
	Public Sub AttachTo(VpHost As clsPlateauCard)
	'---------------------------------------------------------------------
	'Attache la carte à une autre (équipement, enchantement, empreinte...)
	'---------------------------------------------------------------------
		If Not VmAttachedTo Is Nothing Then
			VmAttachedTo.Attachments.Remove(Me)
		End If
		VmAttachedTo = VpHost
		If Not VmAttachedTo Is Nothing Then
			VmAttachedTo.Attachments.Add(Me)
		End If
	End Sub
	Public Overrides Function ToString() As String
		Return VmCardNameFR + " (" + VmCardName + ")"
	End Function
	Public Property Owner As List(Of clsPlateauCard)
		Get
			Return VmOwner
		End Get
		Set (VpOwner As List(Of clsPlateauCard))
			VmOwner = VpOwner
		End Set
	End Property
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
	Public ReadOnly Property IsAttached As Boolean
		Get
			Return VmAttachedTo IsNot Nothing
		End Get
	End Property
	Public Property Attachments As List(Of clsPlateauCard)
		Get
			Return VmAttachments
		End Get
		Set (VpAttachments As List(Of clsPlateauCard))
			VmAttachments = VpAttachments
		End Set
	End Property
End Class