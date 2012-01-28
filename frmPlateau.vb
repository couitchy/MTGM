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
'| Auteur         |                          Couitchy |
'|----------------------------------------------------|
'| Modifications :                                    |
'| - gestion de la position d'insertion	   29/01/2012 |
'------------------------------------------------------
Imports System.IO
Public Partial Class frmPlateau
	#Region "Déclarations"
	Private VmSource As String
	Private VmRestriction As String
	Private VmRestrictionTXT As String
	Private VmPlateauPartie As clsPlateauPartie
	Private VmPlateau As New clsPlateauDrawings
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
		Call clsModule.ExtractPictures(VpPath.Replace("\\", "\"), VmSource, VmRestriction, True)
		'Nouvelle partie
		VmPlateauPartie = New clsPlateauPartie(VmSource, VmRestriction)
		Call VmPlateauPartie.BeginPlateauPartie
	End Sub
	Private Sub ManageReDraw(VpBibli As Boolean, VpGraveyard As Boolean, VpExil As Boolean, VpRegard As Boolean, VpMain As Boolean, VpField As Boolean)
	'-------------------------------
	'Actualisation du plateau de jeu
	'-------------------------------
	Dim VpToRemove As New List(Of PictureBox)
	Dim VpCard As clsPlateauCard
		With VmPlateauPartie
			'Efface les anciennes images (celles à redessiner)
			For Each VpPictureBox As PictureBox In VmPlateau.Pictures
				VpCard = VpPictureBox.Tag
				If (VpCard.Owner Is .Bibli And VpBibli) Or (VpCard.Owner Is .Graveyard And VpGraveyard) Or (VpCard.Owner Is .Exil And VpExil) Or (VpCard.Owner Is .Regard And VpRegard) Or (VpCard.Owner Is .Main And VpMain) Or (VpCard.Owner Is .Field And VpField) Then
					VpToRemove.Add(VpPictureBox)
					VpPictureBox.Parent.Controls.Remove(VpPictureBox)
					VpPictureBox.Dispose
				End If
			Next VpPictureBox
			For Each VpPictureBox As PictureBox In VpToRemove	'interdiction de supprimer des éléments d'une collection en cours d'énumération, d'où cette 2nde boucle
				VmPlateau.Pictures.Remove(VpPictureBox)
			Next VpPictureBox
			'Bibliothèque
			If VpBibli Then
				.BibliTop.Hidden = Not Me.btBibliReveal.Checked
				Call Me.DrawPicture(.BibliTop, True, Me.panelBibli, New EventHandler(AddressOf Me.CardBibliDoubleClick), New MouseEventHandler(AddressOf Me.CardBibliMouseUp))
			End If
			'Cimetière
			If VpGraveyard Then
				Call Me.DrawPicture(.GraveyardTop, True, Me.panelGraveyard, New EventHandler(AddressOf Me.CardGraveyardDoubleClick), New MouseEventHandler(AddressOf Me.CardGraveyardMouseUp))
			End If
			'Exil
			If VpExil Then
				Call Me.DrawPicture(.ExilTop, True, Me.panelExil, New EventHandler(AddressOf Me.CardExilDoubleClick), New MouseEventHandler(AddressOf Me.CardExilMouseUp))
			End If
			'Regard
			If VpRegard Then
				Call Me.DrawPictures(.Regard, True, Me.panelRegard, New EventHandler(AddressOf Me.CardRegardDoubleClick), New MouseEventHandler(AddressOf Me.CardRegardMouseUp))
			End If
			'Main
			If VpMain Then
				Call Me.DrawPictures(.Main, True, Me.panelMain, New EventHandler(AddressOf Me.CardMainDoubleClick), New MouseEventHandler(AddressOf Me.CardMainMouseUp))
			End If
			'Champ de bataille
			If VpField Then
				Call Me.DrawPictures(.Field, False, Me.panelField, New EventHandler(AddressOf Me.CardFieldDoubleClick), New MouseEventHandler(AddressOf Me.CardFieldMouseUp))
			End If
		End With
	End Sub
	Private Sub ManageReDraw
		Call Me.ManageReDraw(True, True, True, True, True, True)
	End Sub
	Private Sub ManageReDraw(VpSource As List(Of clsPlateauCard), VpDestination As List(Of clsPlateauCard))
		With VmPlateauPartie
			Call Me.ManageReDraw(VpSource Is .Bibli Or VpDestination Is .Bibli, VpSource Is .Graveyard Or VpDestination Is .Graveyard, VpSource Is .Exil Or VpDestination Is .Exil, VpSource Is .Regard Or VpDestination Is .Regard, VpSource Is .Main Or VpDestination Is .Main, VpSource Is .Field Or VpDestination Is .Field)
		End With
	End Sub
	Private Sub ManageReDraw(VpDestination As List(Of clsPlateauCard))
		With VmPlateauPartie
			Call Me.ManageReDraw(VpDestination Is .Bibli, VpDestination Is .Graveyard, VpDestination Is .Exil, VpDestination Is .Regard, VpDestination Is .Main, VpDestination Is .Field)
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
			.Location = New System.Drawing.Point(If(VpIndexH = 0, 0, Math.Min(VpIndexH * (VpParent.Width - VpW) / (VpCount - 1), VpIndexH * VpW * CgSpacingFactor)), CgChevauchFactor * VpH * VpIndexV)
			.Size = New System.Drawing.Size(VpW, VpH)
			.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
			.Image = Image.FromFile(VpCard.PicturePath)
			.Tag = VpCard
			AddHandler .DoubleClick, VpDoubleClickHandler
			AddHandler .MouseUp, VpMouseUpHandler
			AddHandler .MouseMove, New MouseEventHandler(AddressOf Me.CardMouseMove)
			AddHandler .MouseLeave, New EventHandler(AddressOf Me.CardMouseLeave)
			AddHandler .Paint, New PaintEventHandler(AddressOf Me.PictureBoxPaint)
		End With
		'Gestion carte engagée / dégagée
		Call Me.ManageTap(VpPicture, True)
		VpParent.Controls.Add(VpPicture)
		VmPlateau.Pictures.Add(VpPicture)		'conserve la référence
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
				Call Me.ManageReDraw(VpListe)
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
	Dim VpDropDown As ToolStripItem
		VmPlateau.CurrentPicture = VpPictureBox
		With VmPlateau.CurrentCard
			'Nom de la carte (titre du menu contextuel)
			If .Owner Is VmPlateauPartie.Bibli And Not Me.btBibliReveal.Checked Then
				Me.cmnuName.Text = "(Face cachée)"
			Else
				Me.cmnuName.Text = .NameVF
			End If
			'Carte éventuellement transformable
			Me.cmnuTransform.Enabled = .Transformable
			Me.cmnuTransform.Checked = .Transformed
			'Liste des autres cartes auxquelles on pourrait potentiellement attacher la carte courante
			Me.cmnuAttachTo.DropDownItems.Clear
			If .Attachments.Count = 0 Then									'on peut s'attacher à une autre carte que si personne n'est attaché à soi
				For Each VpCard As clsPlateauCard In VmPlateauPartie.Field
					If Not VpCard Is VmPlateau.CurrentCard AndAlso Not VpCard.IsAttached Then	'on ne peut ni s'attacher à soi-même ni à une carte déjà attachée à une autre carte
						VpDropDown = Me.cmnuAttachTo.DropDownItems.Add(VpCard.NameVF, Nothing, AddressOf CmnuAttachToClick)
						VpDropDown.Tag = VpCard										'conserve la référence de l'hôte potentiel
					End If
				Next VpCard
			End If
		End With
		'Affichage effectif
		Me.cmnuCardContext.Show(VpPictureBox, VpPoint)
	End Sub
	Private Sub ManageResize
	'----------------------------------------------------------------------------
	'Recalcule la position des splitters après un redimensionnement de la fenêtre
	'----------------------------------------------------------------------------
		If Me.WindowState <> FormWindowState.Minimized Then
			Me.splitV1.SplitterDistance = Me.splitV1.Width / 6
			Me.splitH1.SplitterDistance = Me.splitH1.Height / 3
			Me.splitH2.SplitterDistance = Me.splitH2.Height / 2
			Me.splitH3.SplitterDistance = Me.splitH3.Height / 3
			Me.splitH4.SplitterDistance = Me.splitH4.Height / 2
			Call Me.ManageReDraw
		End If
	End Sub
	Private Function CalcNewPosition(VpMouseLocation As Point, VpDestinationPanel As Panel, VpDestination As List(Of clsPlateauCard)) As Integer
	Dim VpPos As Integer
		If VpDestination.Count > 0 And ( VpDestination Is VmPlateauPartie.Regard Or VpDestination Is VmPlateauPartie.Main Or VpDestination Is VmPlateauPartie.Field ) Then
			VpPos = VpDestination.Count
			While VpPos > 0 AndAlso VpDestinationPanel.PointToClient(VpMouseLocation).X < VmPlateau.GetRightBorder(VpDestination.Item(VpPos - 1))
				VpPos -= 1
			End While
			Return VpPos
		Else
			Return -1
		End If
	End Function
	Private Sub ManageDrop(VpEventArgs As DragEventArgs, VpDestinationPanel As Panel, VpDestination As List(Of clsPlateauCard))
	'---------------------------------------------
	'Gestion de la fin de l'opération de drag&drop
	'---------------------------------------------
	Dim VpCard As clsPlateauCard = VpEventArgs.Data.GetData(GetType(PictureBox)).Tag
	Dim VpSource As List(Of clsPlateauCard) = VpCard.Owner
		If VpCard.SendTo(VpDestination, Me.CalcNewPosition(New Point(VpEventArgs.X, VpEventArgs.Y), VpDestinationPanel, VpDestination)) Then
			Call Me.ManageReDraw(VpSource, VpDestination)
		End If
		Call VmPlateau.StopDragging
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
		'Si la carte n'a pas d'image, écrit au moins le texte
		If VpCard.MissingPicture Then
			Using VpFont As Font = New Font("Arial", 14)
	        	e.Graphics.DrawString(If(VpCard.Transformed, VpCard.TransformedName, VpCard.NameVF), VpFont, Brushes.Green, New Point(5, 5))
	    	End Using
	    End If
	End Sub
	Sub FrmPlateauLoad(sender As Object, e As EventArgs)
		Call Me.ManageResize
		Me.Text = clsModule.CgPlateau + VmRestrictionTXT
		VmPlateau.DragMode = False
	End Sub
	Sub FrmPlateauResizeEnd(sender As Object, e As EventArgs)
		Call Me.ManageResize
	End Sub
	Sub FrmPlateauResize(sender As Object, e As EventArgs)
		If Control.MouseButtons = MouseButtons.None Then
			Call Me.ManageResize
		End If
	End Sub
	Sub BtNewPartieClick(sender As Object, e As EventArgs)
		Me.btLives.Text = "Vies"
		Me.btPoisons.Text = "Poisons"
		Me.btTurns.Text = "Tours"
		VmPlateauPartie.Mulligan = 0
		Call VmPlateauPartie.BeginPlateauPartie
		Call Me.ManageReDraw
	End Sub
	Sub BtMulliganClick(sender As Object, e As EventArgs)
		Me.btLives.Text = "Vies"
		Me.btPoisons.Text = "Poisons"
		Me.btTurns.Text = "Tours"
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
	Sub BtTurnsMouseUp(sender As Object, e As MouseEventArgs)
		If e.Button = MouseButtons.Left Then
			VmPlateauPartie.Tours += 1
		Else
			VmPlateauPartie.Tours -= 1
		End If
		Me.btTurns.Text = VmPlateauPartie.Tours.ToString
	End Sub
	Sub BtBibliRevealClick(sender As Object, e As EventArgs)
		Me.btBibliReveal.Checked = Not Me.btBibliReveal.Checked
		Call Me.ManageReDraw(VmPlateauPartie.Bibli)
	End Sub
	Sub BtFieldUntapAllClick(sender As Object, e As EventArgs)
		For Each VpCard As clsPlateauCard In VmPlateauPartie.Field
			VpCard.Tapped = False
		Next VpCard
		Call Me.ManageReDraw(VmPlateauPartie.Field)
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
		VmPlateau.CurrentCard.Counters = VmPlateau.CurrentCard.Counters + 1
		VmPlateau.CurrentPicture.Invalidate
	End Sub
	Sub CmnuCountersSubClick(sender As Object, e As EventArgs)
		VmPlateau.CurrentCard.Counters = Math.Max(0, VmPlateau.CurrentCard.Counters - 1)
		VmPlateau.CurrentPicture.Invalidate
	End Sub
	Sub CmnuCountersRemoveClick(sender As Object, e As EventArgs)
		VmPlateau.CurrentCard.Counters = 0
		VmPlateau.CurrentPicture.Invalidate
	End Sub
	Sub CmnuTapUntapClick(sender As Object, e As EventArgs)
		Call Me.ManageTap(VmPlateau.CurrentPicture)
		VmPlateau.CurrentPicture.BringToFront
	End Sub
	Sub CmnuSendToClick(sender As Object, e As EventArgs)
	Dim VpRedraw As Boolean
	Dim VpSource As List(Of clsPlateauCard)
		With VmPlateau.CurrentCard
			VpSource = .Owner
			Select Case CType(sender, ToolStripMenuItem).Name
				Case Me.cmnuSendToBibliBottom.Name
					VpRedraw = .SendTo(VmPlateauPartie.Bibli)
					If VpRedraw Then
						.Hidden = True
					End If
				Case Me.cmnuSendToBibliTop.Name
					VpRedraw = .SendTo(VmPlateauPartie.Bibli, 0)
					If VpRedraw Then
						.Hidden = True
					End If
				Case Me.cmnuSendToExil.Name
					VpRedraw = .SendTo(VmPlateauPartie.Exil)
				Case Me.cmnuSendToField.Name
					VpRedraw = .SendTo(VmPlateauPartie.Field)
				Case Me.cmnuSendToGraveyard.Name
					VpRedraw = .SendTo(VmPlateauPartie.Graveyard)
				Case Me.cmnuSendToMain.Name
					VpRedraw = .SendTo(VmPlateauPartie.Main)
				Case Me.cmnuSendToRegard.Name
					VpRedraw = .SendTo(VmPlateauPartie.Regard)
				Case Else
			End Select
			If VpRedraw Then
				Call Me.ManageReDraw(VpSource, .Owner)
			End If
		End With
	End Sub
	Sub CmnuAttachToClick(sender As Object, e As EventArgs)
	Dim VpHost As clsPlateauCard = sender.Tag
		Call VmPlateau.CurrentCard.AttachTo(VpHost)
		Call Me.ManageReDraw(VmPlateauPartie.Field)
	End Sub
	Sub CmnuDetachFromClick(sender As Object, e As EventArgs)
		Call VmPlateau.CurrentCard.AttachTo(Nothing)
		Call Me.ManageReDraw(VmPlateauPartie.Field)
	End Sub
	Sub CmnuTransformClick(sender As Object, e As EventArgs)
		With VmPlateau.CurrentCard
			.Transformed = Not .Transformed
			VmPlateau.CurrentPicture.Image = Image.FromFile(.PicturePath)
		End With
	End Sub
	Sub BtBibliShuffleClick(sender As Object, e As EventArgs)
		Call clsPlateauPartie.Shuffle(VmPlateauPartie.Bibli)
	End Sub
	Sub BtMainShuffleClick(sender As Object, e As EventArgs)
		Call clsPlateauPartie.Shuffle(VmPlateauPartie.Main)
		Call Me.ManageReDraw(VmPlateauPartie.Main)
	End Sub
	Sub CardBibliDoubleClick(sender As Object, e As EventArgs)
	Dim VpCard As clsPlateauCard = sender.Tag
		If VpCard.SendTo(VmPlateauPartie.Main) Then
			Call Me.ManageReDraw(VmPlateauPartie.Bibli, VmPlateauPartie.Main)
		End If
		VmPlateauPartie.Tours += 1
		Me.btTurns.Text = VmPlateauPartie.Tours.ToString
	End Sub
	Sub CardGraveyardDoubleClick(sender As Object, e As EventArgs)
		
	End Sub
	Sub CardExilDoubleClick(sender As Object, e As EventArgs)
		
	End Sub
	Sub CardRegardDoubleClick(sender As Object, e As EventArgs)
	Dim VpCard As clsPlateauCard = sender.Tag
		If VpCard.SendTo(VmPlateauPartie.Main) Then
			Call Me.ManageReDraw(VmPlateauPartie.Regard, VmPlateauPartie.Main)
		End If
	End Sub
	Sub CardMainDoubleClick(sender As Object, e As EventArgs)
	Dim VpCard As clsPlateauCard = sender.Tag
	Dim VpRedraw As Boolean
	Dim VpSource As List(Of clsPlateauCard) = VpCard.Owner
		If VpCard.IsAPermanent Then
			VpRedraw = VpCard.SendTo(VmPlateauPartie.Field)
		Else
			VpRedraw = VpCard.SendTo(VmPlateauPartie.Graveyard)
		End If
		If VpRedraw Then
			Call Me.ManageReDraw(VpSource, VpCard.Owner)
		End If
	End Sub
	Sub CardFieldDoubleClick(sender As Object, e As EventArgs)
	Dim VpCard As clsPlateauCard = sender.Tag
		If VpCard.SendTo(VmPlateauPartie.Graveyard) Then
			Call Me.ManageReDraw(VmPlateauPartie.Field, VmPlateauPartie.Graveyard)
		End If
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
	Sub CardMouseMove(sender As Object, e As MouseEventArgs)
	Dim VpPicture As PictureBox = sender
		If e.Button = MouseButtons.Left Then
			If Not VmPlateau.DragMode Then
				VmPlateau.DragMode = True
				VpPicture.DoDragDrop(VpPicture, DragDropEffects.Move)
			End If
		End If
	End Sub
	Sub CardMouseLeave(sender As Object, e As EventArgs)
		VmPlateau.DragMode = False
	End Sub
	Sub PanelDragEnter(sender As Object, e As DragEventArgs)
		If e.Data.GetFormats()(0) = GetType(PictureBox).ToString Then
			e.Effect = DragDropEffects.Move
		Else
			e.Effect = DragDropEffects.None
		End If
	End Sub
	Sub PanelDragOver(sender As Object, e As DragEventArgs)
	Dim VpOrigPicture As PictureBox = e.Data.GetData(GetType(PictureBox))
	Dim VpImg As Image
		With clsPlateauDrawings.DraggedPicture
			If .Image Is Nothing Then
				VpImg = VpOrigPicture.Image
				.Image = New Bitmap(VpImg.Width, VpImg.Height)
				Using VpGraphics As Graphics = Graphics.FromImage(.Image)
					VpGraphics.DrawImage(VpImg, New Rectangle(0, 0, .Image.Width, .Image.Height), 0, 0, .Image.Width, .Image.Height, GraphicsUnit.Pixel, VmPlateau.Opacity)
				End Using
				.Size = VpOrigPicture.Size
			End If
			sender.Controls.Add(clsPlateauDrawings.DraggedPicture)
			.Location = sender.PointToClient(New Point(e.X - .Width / 2, e.Y - .Height / 2))
		End With
	End Sub
	Sub PanelFieldDragDrop(sender As Object, e As DragEventArgs)
		Call Me.ManageDrop(e, Me.panelField, VmPlateauPartie.Field)
	End Sub
	Sub PanelMainDragDrop(sender As Object, e As DragEventArgs)
		Call Me.ManageDrop(e, Me.panelMain, VmPlateauPartie.Main)
	End Sub
	Sub PanelRegardDragDrop(sender As Object, e As DragEventArgs)
		Call Me.ManageDrop(e, Me.panelRegard, VmPlateauPartie.Regard)
	End Sub
	Sub PanelBibliDragDrop(sender As Object, e As DragEventArgs)
		Call Me.ManageDrop(e, Me.PanelBibli, VmPlateauPartie.Bibli)
	End Sub
	Sub PanelGraveyardDragDrop(sender As Object, e As DragEventArgs)
		Call Me.ManageDrop(e, Me.panelGraveyard, VmPlateauPartie.Graveyard)
	End Sub
	Sub PanelExilDragDrop(sender As Object, e As DragEventArgs)
		Call Me.ManageDrop(e, Me.panelExil, VmPlateauPartie.Exil)
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
	Private VmTours As Integer
	Public Sub New(VpSource As String, VpRestriction As String)
	'-------------------
	'Construction du jeu
	'-------------------
	Dim VpSQL As String
		VpSQL = "Select Card.Title, " + VpSource + ".Items, CardFR.TitleFR, Card.Type, Card.SpecialDoubleCard From (Card Inner Join " + VpSource + " On " + VpSource + ".EncNbr = Card.EncNbr) Inner Join CardFR On Card.EncNbr = CardFR.EncNbr Where "
		VpSQL = VpSQL + VpRestriction
		VpSQL = clsModule.TrimQuery(VpSQL)
		VgDBCommand.CommandText = VpSQL
		VgDBReader = VgDBCommand.ExecuteReader
		With VgDBReader
			While .Read
				'Carte normale
				If Not .GetBoolean(4) Then
					Call Me.AddCard(.GetString(0), .GetString(2), .GetInt32(1), .GetString(3), False, .GetString(0))
				'Carte transformable
				Else
					Call Me.AddCard(.GetString(0), .GetString(2), .GetInt32(1), .GetString(3), True, clsModule.GetTransformedName(.GetString(0)))
				End If
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
		VmLives = clsModule.CgNLives
		VmPoisons = 0
		VmTours = 0
	End Sub
	Private Sub AddCard(VpName As String, VpNameFR As String, VpCount As Integer, VpType As String, VpTransformable As Boolean, VpTransformedCardName As String)
	'--------------------
	'Construction du deck
	'--------------------
		For VpI As Integer = 1 To VpCount
			VmDeck.Add(New clsPlateauCard(VmDeck, VpName, VpNameFR, VpType, VpTransformable, VpTransformedCardName))
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
	Public Property Tours As Integer
		Get
			Return VmTours
		End Get
		Set (VpTours As Integer)
			VmTours = VpTours
		End Set
	End Property
	#End Region
End Class
Public Class clsPlateauCard
	Private VmOwner As List(Of clsPlateauCard)
	Private VmCardName As String
	Private VmTransformedCardName As String
	Private VmCardNameFR As String
	Private VmCardType As String
	Private VmTapped As Boolean
	Private VmHidden As Boolean
	Private VmTransformable As Boolean
	Private VmTransformed As Boolean
	Private VmCounters As Integer
	Private VmAttachedTo As clsPlateauCard
	Private VmAttachments As New List(Of clsPlateauCard)
	Private VmMissingImg As Boolean
	Public Sub New(VpOwner As List(Of clsPlateauCard), VpName As String, VpNameFR As String, VpType As String, VpTransformable As Boolean, VpTransformedCardName As String)
		VmCardName = VpName
		VmCardNameFR = VpNameFR
		VmCardType = VpType
		VmTransformable = VpTransformable
		VmTransformedCardName = VpTransformedCardName
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
	Public Function SendTo(VpNewOwner As List(Of clsPlateauCard), Optional VpIndex As Integer = -1) As Boolean
	'-----------------------
	'Change la carte de zone
	'-----------------------
		If Not VmOwner Is VpNewOwner Then
			'On doit tout d'abord enlever tout ce qui était attaché / ce à quoi on était attaché
			For Each VpAttachment As clsPlateauCard In VmAttachments
				Call VpAttachment.AttachTo(Nothing, True)
			Next VpAttachment
			VmAttachments.Clear
			If Not VmAttachedTo Is Nothing Then
				VmAttachedTo.Attachments.Remove(Me)
			End If
			VmAttachedTo = Nothing
			VmHidden = False
			VmOwner.Remove(Me)
			VmOwner = VpNewOwner
			If VpIndex <> -1 Then
				VmOwner.Insert(VpIndex, Me)
			Else
				VmOwner.Add(Me)
			End If
			Return True
		End If
		Return False
	End Function
	Public Sub AttachTo(VpHost As clsPlateauCard, Optional VpHostReadOnly As Boolean = False)
	'---------------------------------------------------------------------
	'Attache la carte à une autre (équipement, enchantement, empreinte...)
	'---------------------------------------------------------------------
		If Not VpHostReadOnly Then		'interdiction de supprimer des éléments d'une collection en cours d'énumération
			If Not VmAttachedTo Is Nothing Then
				VmAttachedTo.Attachments.Remove(Me)
			End If
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
		Dim VpFile As String
		Dim VpCardName As String
			VpCardName = If(VmTransformed, VmTransformedCardName, VmCardName)
			VmMissingImg = False
			If Not VmHidden Then
				VpFile = Path.GetTempPath + clsModule.CgTemp + "\" + clsModule.AvoidForbiddenChr(VpCardName) + ".jpg"
				If File.Exists(VpFile) Then
					Return VpFile
				Else
					VmMissingImg = True
					Return Application.StartupPath + clsModule.CgMagicBack
				End If
			Else
				Return Application.StartupPath + clsModule.CgMagicBack
			End If
		End Get
	End Property
	Public ReadOnly Property MissingPicture As Boolean
		Get
			Return VmMissingImg
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
	Public ReadOnly Property Transformable As Boolean
		Get
			Return VmTransformable
		End Get
	End Property
	Public Property Transformed As Boolean
		Get
			Return VmTransformed
		End Get
		Set (VpTransformed As Boolean)
			VmTransformed = VpTransformed
		End Set
	End Property
	Public ReadOnly Property TransformedName As String
		Get
			Return VmTransformedCardName
		End Get
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
Public Class clsPlateauDrawings
	Private VmPictures As New List(Of PictureBox)
	Private VmCurrentPicture As PictureBox
	Private Shared VmDragMode As Boolean
	Private Shared VmDraggedPicture As PictureBox
	Private Shared VmAttrib As New Imaging.ImageAttributes
	Public Sub New
	Dim VpColorMatrix As New Imaging.ColorMatrix
		VmDraggedPicture = New PictureBox
		VmDraggedPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		VmDraggedPicture.BackColor = Color.Transparent
		With VpColorMatrix
			.Matrix00 = 1
			.Matrix11 = 1
			.Matrix22 = 1
			.Matrix33 = 0.5
			.Matrix44 = 1
		End With
		VmAttrib.SetColorMatrix(VpColorMatrix)
	End Sub
	Public Function GetRightBorder(VpCard As clsPlateauCard) As Integer
		For Each VpPicture As PictureBox In VmPictures
			If CType(VpPicture.Tag, clsPlateauCard) Is VpCard Then
				Return VpPicture.Left + VpPicture.Width
			End If
		Next VpPicture
		Return Nothing
	End Function
	Public Sub StopDragging
		VmDraggedPicture.Visible = False
		VmDraggedPicture.Image = Nothing
	End Sub
	Public Property Pictures As List(Of PictureBox)
		Get
			Return VmPictures
		End Get
		Set (VpPictures As List(Of PictureBox))
			VmPictures = VpPictures
		End Set
	End Property
	Public Property CurrentPicture As PictureBox
		Get
			Return VmCurrentPicture
		End Get
		Set (VpCurrentPicture As PictureBox)
			VmCurrentPicture = VpCurrentPicture
		End Set
	End Property
	Public ReadOnly Property CurrentCard As clsPlateauCard
		Get
			Return VmCurrentPicture.Tag
		End Get
	End Property
	Public Property DragMode As Boolean
		Get
			Return VmDragMode
		End Get
		Set (VpDragMode As Boolean)
			VmDragMode = VpDragMode
			If VmDragMode Then
				VmDraggedPicture.Visible = True
			End If
		End Set
	End Property
	Public Shared ReadOnly Property DraggedPicture As PictureBox
		Get
			Return VmDraggedPicture
		End Get
	End Property
	Public ReadOnly Property Opacity As Imaging.ImageAttributes
		Get
			Return VmAttrib
		End Get
	End Property
End Class