Imports System.IO
Public Partial Class frmBoard
    #Region "Déclarations"
    Private VmSource As String
    Private VmRestriction As String
    Private VmRestrictionTXT As String
    Private VmPlateauPartie As clsBoardGame
    Private VmPlateau As New clsBoardDrawings
    #End Region
    #Region "Méthodes"
    Public Sub New(VpOwner As MainForm)
    Dim VpPath As String = Path.GetTempPath + mdlConstGlob.CgTemp
        Call Me.InitializeComponent
        VmSource = VpOwner.MySource
        VmRestriction = VpOwner.Restriction
        VmRestrictionTXT = VpOwner.Restriction(True)
        If VmRestrictionTXT.Length > 31 Then
            VmRestrictionTXT = VmRestrictionTXT.Substring(0, 31)
        End If
        'Extraction des images en répertoire temporaire
        If Not Directory.Exists(VpPath) Then
            Directory.CreateDirectory(VpPath)
        End If
        Call mdlToolbox.ExtractPictures(VpPath.Replace("\\", "\"), VmSource, VmRestriction, True)
        'Nouvelle partie
        VmPlateauPartie = New clsBoardGame(VmSource, VmRestriction)
        Call VmPlateauPartie.BeginPlateauPartie
    End Sub
    Private Sub ManageReDraw(VpBibli As Boolean, VpGraveyard As Boolean, VpExil As Boolean, VpRegard As Boolean, VpMain As Boolean, VpField As Boolean)
    '-------------------------------
    'Actualisation du plateau de jeu
    '-------------------------------
    Dim VpToRemove As New List(Of PictureBox)
    Dim VpCard As clsBoardCard
        If VmPlateauPartie Is Nothing Then Exit Sub
        With VmPlateauPartie
            'Efface les anciennes images (celles à redessiner)
            For Each VpPictureBox As PictureBox In VmPlateau.Pictures
                VpCard = VpPictureBox.Tag
                If (VpCard.Owner Is .Bibli And VpBibli) Or (VpCard.Owner Is .Graveyard And VpGraveyard) Or (VpCard.Owner Is .Exil And VpExil) Or (VpCard.Owner Is .Regard And VpRegard) Or (VpCard.Owner Is .Main And VpMain) Or (VpCard.Owner Is .Field And VpField) Or (VpCard.InReserve And Not VpCard.PlayedFromReserve And Not Me.btReserve.Checked) Then
                    VpToRemove.Add(VpPictureBox)
                    VpPictureBox.Parent.Controls.Remove(VpPictureBox)
                    VpPictureBox.Dispose
                End If
            Next VpPictureBox
            For Each VpPictureBox As PictureBox In VpToRemove   'interdiction de supprimer des éléments d'une collection en cours d'énumération, d'où cette 2nde boucle
                VmPlateau.Pictures.Remove(VpPictureBox)
            Next VpPictureBox
            'Bibliothèque
            If VpBibli Then
                If .BibliTop IsNot Nothing Then
                    .BibliTop.Hidden = Not Me.btBibliReveal.Checked
                End If
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
    Private Sub ManageReDraw(VpSource As List(Of clsBoardCard), VpDestination As List(Of clsBoardCard))
        With VmPlateauPartie
            Call Me.ManageReDraw(VpSource Is .Bibli Or VpDestination Is .Bibli, VpSource Is .Graveyard Or VpDestination Is .Graveyard, VpSource Is .Exil Or VpDestination Is .Exil, VpSource Is .Regard Or VpDestination Is .Regard, VpSource Is .Main Or VpDestination Is .Main, VpSource Is .Field Or VpDestination Is .Field)
        End With
    End Sub
    Private Sub ManageReDraw(VpDestination As List(Of clsBoardCard))
        With VmPlateauPartie
            Call Me.ManageReDraw(VpDestination Is .Bibli, VpDestination Is .Graveyard, VpDestination Is .Exil, VpDestination Is .Regard, VpDestination Is .Main, VpDestination Is .Field)
        End With
    End Sub
    Private Sub DrawPicture(VpCard As clsBoardCard, VpUntap As Boolean, VpParent As Control, VpIndexH As Integer, VpCount As Integer, VpDoubleClickHandler As EventHandler, VpMouseUpHandler As MouseEventHandler)
    '---------------------------------
    'Dessin d'une carte sur le plateau
    '---------------------------------
    Dim VpW As Integer
    Dim VpH As Integer
    Dim VpW0 As Integer
    Dim VpH0 As Integer
    Dim VpEffectiveCardHeight_px As Integer
        If Not VpCard Is Nothing Then
            If VpUntap Then
                VpCard.Tapped = False
            End If
            'Détermine le côté limitant pour l'affichage optimal avec respect des proportions
            VpEffectiveCardHeight_px = CInt(CgMTGCardHeight_px * (1 + VpCard.Attachments.Count * CgChevauchFactor)) '/!\ il faut prendre en compte les cartes attachées pour avoir la hauteur totale
            'Si c'est la hauteur qui limite
            If VpEffectiveCardHeight_px / VpParent.Height > CgMTGCardWidth_px / VpParent.Width Then
                VpH = Math.Min(VpParent.Height, VpEffectiveCardHeight_px)               'ici VpH vaut la hauteur cumulée (avec les cartes attachées)
                VpH0 = Math.Min(VpParent.Height, CgMTGCardHeight_px)
                VpH = CInt(VpH / (1 + VpCard.Attachments.Count * CgChevauchFactor))     'on divise pour retrouver la hauteur que devra avoir la carte hôte seule
                VpW = CgMTGCardWidth_px * VpH / CgMTGCardHeight_px                      'en déduit la largeur convenable pour conserver le ratio d'aspect
                VpW0 = CgMTGCardWidth_px * VpH0 / CgMTGCardHeight_px
            'Si c'est la largeur qui limite
            Else
                VpW = Math.Min(VpParent.Width, CgMTGCardWidth_px)
                VpH = CgMTGCardHeight_px * VpW / CgMTGCardWidth_px
            End If
            'Dessin carte hôte
            Call Me.EffectiveDraw(VpCard, VpW, VpW0, VpH, VpIndexH, VpCount, VpCard.Attachments.Count, VpParent, VpDoubleClickHandler, VpMouseUpHandler)
            'Dessin cartes attachées éventuelles
            If VpCard.Attachments.Count > 0 Then
                For VpIndexV As Integer = VpCard.Attachments.Count - 1 To 0 Step - 1    'on parcourt la liste à l'envers pour avoir l'affichage visuel dans le bon ordre (zorder)
                    Call Me.EffectiveDraw(VpCard.Attachments.Item(VpIndexV), VpW, VpW0, VpH, VpIndexH, VpCount, VpIndexV, VpParent, VpDoubleClickHandler, VpMouseUpHandler)
                Next VpIndexV
            End If
        End If
    End Sub
    Private Sub EffectiveDraw(VpCard As clsBoardCard, VpW As Integer, VpW0 As Integer, VpH As Integer, VpIndexH As Integer, VpCount As Integer, VpIndexV As Integer, VpParent As Control, VpDoubleClickHandler As EventHandler, VpMouseUpHandler As MouseEventHandler)
    Dim VpPicture As PictureBox
        VpPicture = New PictureBox
        With VpPicture
            .Location = New System.Drawing.Point(If(VpIndexH = 0, 0, Math.Min(VpIndexH * (VpParent.Width - VpW) / (VpCount - 1), VpIndexH * VpW0 * CgSpacingFactor)), CgChevauchFactor * VpH * VpIndexV)
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
        VmPlateau.Pictures.Add(VpPicture)       'conserve la référence
    End Sub
    Private Sub DrawPicture(VpCard As clsBoardCard, VpUntap As Boolean, VpParent As Control, VpDoubleClickHandler As EventHandler, VpMouseUpHandler As MouseEventHandler)
        Call Me.DrawPicture(VpCard, VpUntap, VpParent, 0, 1, VpDoubleClickHandler, VpMouseUpHandler)
    End Sub
    Private Sub DrawPictures(VpCards As List(Of clsBoardCard), VpUntap As Boolean, VpParent As Control, VpDoubleClickHandler As EventHandler, VpMouseUpHandler As MouseEventHandler)
    Dim VpCount As Integer = 0
    Dim VpIndex As Integer = 0
        'On est obligé de faire une première passe pour déterminer VpCount
        For Each VpCard As clsBoardCard In VpCards
            If Not VpCard.IsAttached Then
                VpCount += 1
            End If
        Next VpCard
        For Each VpCard As clsBoardCard In VpCards
            If Not VpCard.IsAttached Then
                Call Me.DrawPicture(VpCard, VpUntap, VpParent, VpIndex, VpCount, VpDoubleClickHandler, VpMouseUpHandler)
                VpIndex += 1
            End If
        Next VpCard
    End Sub
    Private Sub SearchIn(VpListe As List(Of clsBoardCard), VpPosition As Integer)
    '-----------------------------------------------------------------------------------------------------------------------------
    'Recherche une carte demandée par l'utilisateur afin de la placer au sommet de la collection (zone) à laquelle elle appartient
    '-----------------------------------------------------------------------------------------------------------------------------
    Dim VpStr As String = InputBox("Rechercher dans la zone (VO ou VF) :", "Recherche", mdlConstGlob.CgCard)
    Dim VpFound As Boolean = False
    Dim VpTmp As clsBoardCard = Nothing
    Dim VpIndex As Integer
        If VpStr.Trim <> "" Then
            'Cherche dans la zone la carte spécifiée par l'utilisateur
            For Each VpCard As clsBoardCard In VpListe
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
                Call mdlToolbox.ShowInformation(VpTmp.ToString + " a été placé(e) sur le dessus de la zone.")
            End If
        End If
    End Sub
    Private Sub UntapAll
        For Each VpCard As clsBoardCard In VmPlateauPartie.Field
            VpCard.Tapped = False
        Next VpCard
        Call Me.ManageReDraw(VmPlateauPartie.Field)
    End Sub
    Private Sub ManageTap(VpPicture As PictureBox, Optional VpStatic As Boolean = False)
    '------------------------------------------------------------------
    'Gestion de l'orientation de la carte (engagée @ 90°, dégagée @ 0°)
    '------------------------------------------------------------------
    Dim VpCard As clsBoardCard = VpPicture.Tag
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
            If .Attachments.Count = 0 Then                                  'on peut s'attacher à une autre carte que si personne n'est attaché à soi
                For Each VpCard As clsBoardCard In VmPlateauPartie.Field
                    If Not VpCard Is VmPlateau.CurrentCard AndAlso Not VpCard.IsAttached Then   'on ne peut ni s'attacher à soi-même ni à une carte déjà attachée à une autre carte
                        VpDropDown = Me.cmnuAttachTo.DropDownItems.Add(VpCard.NameVF, Nothing, AddressOf CmnuAttachToClick)
                        VpDropDown.Tag = VpCard                                     'conserve la référence de l'hôte potentiel
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
    Private Function HalfSize As Rectangle
    Dim VpScreen As Rectangle
        VpScreen = Screen.GetBounds(Me.Location)
        Me.WindowState = FormWindowState.Normal
        Me.Size = New Size(VpScreen.Width, VpScreen.Height / 2)
        Return VpScreen
    End Function
    Private Function CalcNewPosition(VpMouseLocation As Point, VpDestinationPanel As Panel, VpDestination As List(Of clsBoardCard)) As Integer
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
    Private Sub ManageDrop(VpEventArgs As DragEventArgs, VpDestinationPanel As Panel, VpDestination As List(Of clsBoardCard))
    '---------------------------------------------
    'Gestion de la fin de l'opération de drag&drop
    '---------------------------------------------
    Dim VpCard As clsBoardCard = VpEventArgs.Data.GetData(GetType(PictureBox)).Tag
    Dim VpSource As List(Of clsBoardCard) = VpCard.Owner
        If Me.btReserve.Checked AndAlso VpDestination Is VmPlateauPartie.Regard Then
            Call mdlToolbox.ShowWarning(mdlConstGlob.CgErr9)
        Else
            If VpCard.SendTo(VpDestination, Me.CalcNewPosition(New Point(VpEventArgs.X, VpEventArgs.Y), VpDestinationPanel, VpDestination)) Then
                Call Me.ManageReDraw(VpSource, VpDestination)
            End If
        End If
        Call VmPlateau.StopDragging
    End Sub
    Private Sub NewPartie
    '------------------------------
    'Réinitialise le plateau de jeu
    '------------------------------
        Me.btLives.Text = "Vies"
        Me.btPoisons.Text = "Poisons"
        Me.btTurns.Text = "Tours"
        VmPlateauPartie.Mulligan = 0
        Me.btReserve.Checked = False
        Call VmPlateauPartie.BeginPlateauPartie
        Call Me.ManageReDraw
    End Sub
    #End Region
    #Region "Evènements"
    Sub PictureBoxPaint(sender As Object, e As PaintEventArgs)
    '--------------------------------------------------------------
    'Dessine les éventuels marqueurs présents sur la carte courante
    '--------------------------------------------------------------
    Dim VpCard As clsBoardCard = sender.Tag
    Dim VpDiameter As Single
    Dim VpLeftToDraw As Integer
    Dim VpLevel As Integer
    Dim VpItem As Integer
    Dim VpX As Single
    Dim VpY As Single
        'Gestion d'éventuels marqueurs sur la carte
        If VpCard.Counters > 0 Then
            VpDiameter = mdlConstGlob.CgCounterDiametr_px * Math.Max(sender.Width, sender.Height) / mdlConstGlob.CgMTGCardHeight_px
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
        Me.Text = mdlConstGlob.CgPlateau + VmRestrictionTXT
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
        Call Me.NewPartie
    End Sub
    Sub BtMulliganClick(sender As Object, e As EventArgs)
        Me.btLives.Text = "Vies"
        Me.btPoisons.Text = "Poisons"
        Me.btTurns.Text = "Tours"
        VmPlateauPartie.Mulligan = Math.Min(VmPlateauPartie.Mulligan + 1, mdlConstGlob.CgNMain - 1)
        Me.btReserve.Checked = False
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
        Call Me.UntapAll
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
    Dim VpSource As List(Of clsBoardCard)
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
                    If Not Me.btReserve.Checked Then
                        VpRedraw = .SendTo(VmPlateauPartie.Regard)
                    Else
                        Call mdlToolbox.ShowWarning(mdlConstGlob.CgErr9)
                    End If
                Case Else
            End Select
            If VpRedraw Then
                Call Me.ManageReDraw(VpSource, .Owner)
            End If
        End With
    End Sub
    Sub CmnuAttachToClick(sender As Object, e As EventArgs)
    Dim VpHost As clsBoardCard = sender.Tag
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
        Call clsBoardGame.Shuffle(VmPlateauPartie.Bibli)
        Call Me.ManageReDraw(VmPlateauPartie.Bibli)
    End Sub
    Sub BtMainShuffleClick(sender As Object, e As EventArgs)
        Call clsBoardGame.Shuffle(VmPlateauPartie.Main)
        Call Me.ManageReDraw(VmPlateauPartie.Main)
    End Sub
    Sub BtRegardShuffleClick(sender As Object, e As EventArgs)
        Call clsBoardGame.Shuffle(VmPlateauPartie.Regard)
        Call Me.ManageReDraw(VmPlateauPartie.Regard)
    End Sub
    Sub CardBibliDoubleClick(sender As Object, e As EventArgs)
    Dim VpCard As clsBoardCard = sender.Tag
        If VpCard.SendTo(VmPlateauPartie.Main) Then
            Call Me.ManageReDraw(VmPlateauPartie.Bibli, VmPlateauPartie.Main)
        End If
    End Sub
    Sub CardGraveyardDoubleClick(sender As Object, e As EventArgs)
    Dim VpCard As clsBoardCard = sender.Tag
        If VpCard.SendTo(VmPlateauPartie.Exil) Then
            Call Me.ManageReDraw(VmPlateauPartie.Graveyard, VmPlateauPartie.Exil)
        End If
    End Sub
    Sub CardExilDoubleClick(sender As Object, e As EventArgs)
    Dim VpCard As clsBoardCard = sender.Tag
        If Not Me.btReserve.Checked Then
            If VpCard.SendTo(VmPlateauPartie.Regard) Then
                Call Me.ManageReDraw(VmPlateauPartie.Exil, VmPlateauPartie.Regard)
            End If
        Else
            Call mdlToolbox.ShowWarning(mdlConstGlob.CgErr9)
        End If
    End Sub
    Sub CardRegardDoubleClick(sender As Object, e As EventArgs)
    Dim VpCard As clsBoardCard = sender.Tag
        If VpCard.SendTo(VmPlateauPartie.Main) Then
            Call Me.ManageReDraw(VmPlateauPartie.Regard, VmPlateauPartie.Main)
        End If
    End Sub
    Sub CardMainDoubleClick(sender As Object, e As EventArgs)
    Dim VpCard As clsBoardCard = sender.Tag
    Dim VpRedraw As Boolean
    Dim VpSource As List(Of clsBoardCard) = VpCard.Owner
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
    Dim VpCard As clsBoardCard = sender.Tag
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
    Dim VpCard As clsBoardCard = VpPicture.Tag
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
        With clsBoardDrawings.DraggedPicture
            If .Image Is Nothing Then
                VpImg = VpOrigPicture.Image
                .Image = New Bitmap(VpImg.Width, VpImg.Height)
                Using VpGraphics As Graphics = Graphics.FromImage(.Image)
                    VpGraphics.DrawImage(VpImg, New Rectangle(0, 0, .Image.Width, .Image.Height), 0, 0, .Image.Width, .Image.Height, GraphicsUnit.Pixel, VmPlateau.Opacity)
                End Using
                .Size = VpOrigPicture.Size
            End If
            sender.Controls.Add(clsBoardDrawings.DraggedPicture)
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
    Sub BtAnchorUpClick(sender As Object, e As EventArgs)
    Dim VpScreen As Rectangle = Me.HalfSize
        Me.Location = New Point(VpScreen.Location.X, VpScreen.Location.Y)
    End Sub
    Sub BtAnchorDownClick(sender As Object, e As EventArgs)
    Dim VpScreen As Rectangle = Me.HalfSize
        Me.Location = New Point(VpScreen.Location.X, VpScreen.Location.Y + VpScreen.Height / 2)
    End Sub
    Sub BtNextRoundClick(sender As Object, e As EventArgs)
        Call Me.UntapAll
        With VmPlateauPartie
            If .BibliTop IsNot Nothing AndAlso .BibliTop.SendTo(.Main) Then
                Call Me.ManageReDraw(.Bibli, .Main)
            End If
            .Tours += 1
            Me.btTurns.Text = .Tours.ToString
        End With
    End Sub
    Sub BtReserveClick(sender As Object, e As EventArgs)
        If Not Me.btReserve.Checked AndAlso VmPlateauPartie.Regard.Count > 0 Then
            Call mdlToolbox.ShowWarning(mdlConstGlob.CgErr10)
        Else
            Me.btReserve.Checked = Not Me.btReserve.Checked
            With VmPlateauPartie
                For Each VpCard As clsBoardCard In .GetReserve
                    If Not VpCard.PlayedFromReserve And Me.btReserve.Checked Then
                        VpCard.Hidden = False
                        .Regard.Add(VpCard)
                    ElseIf VpCard.Owner IsNot .Regard Then
                        .Regard.Remove(VpCard)
                    End If
                Next VpCard
                Call Me.ManageReDraw(.Regard)
            End With
        End If
    End Sub
    Sub BtReserveSwapClick(sender As Object, e As EventArgs)
    Dim VpReserveManager As frmSide
        If mdlToolbox.ShowQuestion("Cette opération va interrompre la partie en cours..." + vbCrLf + "Continuer ?") = System.Windows.Forms.DialogResult.Yes Then
            VpReserveManager = New frmSide(Me)
            VpReserveManager.ShowDialog
            Call Me.NewPartie
        End If
    End Sub
    Sub BtDeClick(sender As Object, e As EventArgs)
        Call mdlToolbox.ShowInformation("Le lancer de dé a donné : " + mdlConstGlob.VgRandom.Next(1, 7).ToString)
    End Sub
    Sub BtPieceClick(sender As Object, e As EventArgs)
        Call mdlToolbox.ShowInformation("Le lancer de pièce a donné : " + If(mdlConstGlob.VgRandom.Next(1, 3) = 1, "pile", "face"))
    End Sub
    #End Region
    #Region "Propriétés"
    Public ReadOnly Property PlateauPartie As clsBoardGame
        Get
            Return VmPlateauPartie
        End Get
    End Property
    #End Region
End Class
