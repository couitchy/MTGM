Imports System.IO
Imports SourceGrid2
Imports Cells = SourceGrid2.Cells.Real
Public Partial Class frmSimu
    Private Shared VmGrapher As New frmGrapher
    Private Shared VmGraphCount As Integer = 0
    Private VmSource As String
    Private VmRestrictionSQL As String
    Private VmRestrictionTXT As String
    Private VmRestrictionReserve As String
    Private VmSimuOut As StreamWriter
    Private VmExpr As List(Of clsCorrelation)
    #Region "Méthodes"
    Public Sub New(VpOwner As MainForm)
        Call Me.InitializeComponent
        VmSource = VpOwner.MySource
        VmRestrictionReserve = If(VpOwner.IsInDeckMode, " Reserve = False And ", " ")
        VmRestrictionSQL = VpOwner.Restriction
        VmRestrictionTXT = VpOwner.Restriction(True)
        Me.Text = clsModule.CgSimus + VmRestrictionTXT
    End Sub
    Private Sub LoadSpecialUses
    '----------------------------------------------------------------------
    'Charge la liste des cartes possédant une utilisation spéciale associée
    '----------------------------------------------------------------------
    Dim VpPartie As New clsPartie(VmSource, VmRestrictionSQL + VmRestrictionReserve)
        Me.lstUserCombos.Items.Clear
        VgDBCommand.CommandText = "Select Card From MySpecialUses;"
        VgDBReader = VgDBCommand.ExecuteReader
        With VgDBReader
            While .Read
                If VpPartie.IsInFullDeck(.GetString(0)) Then
                    Me.lstUserCombos.Items.Add(.GetString(0), True)
                End If
            End While
            .Close
        End With
    End Sub
    Private Sub AddToSequence(VpSequence As clsComboSequence, VpGrid As Grid, VpElementType As clsElement.eElementType)
        For VpI As Integer = 1 To VpGrid.RowsCount - 1
            For VpJ As Integer = 1 To CInt(VpGrid(VpI, 2).Value)
                VpSequence.Add(New clsElement(VpElementType, VpGrid(VpI, 0).Tag))
            Next VpJ
        Next VpI
    End Sub
    Private Sub IncrementRow(VpGrid As Grid, VpItem As String)
        For VpI As Integer = 1 To VpGrid.RowsCount - 1
            If VpGrid(VpI, 0).Tag = VpItem Then
                VpGrid(VpI, 2).Value += 1
            End If
        Next VpI
    End Sub
    Private Sub InsertRow(VpGrid As Grid, VpS As String, VpSTag As String, VpN As Integer, VpCellModel As DataModels.IDataModel, VpCellBehavior As BehaviorModels.CustomEvents)
    '----------------------------------------------------------------------------
    'Insère la ligne spécifiée en paramètre dans la grille spécifiée en paramètre
    '----------------------------------------------------------------------------
        If VpSTag <> "" Then
            With VpGrid
                .Rows.Insert(.RowsCount)
                VpGrid(.RowsCount - 1, 0) = New Cells.Cell(VpS)
                VpGrid(.RowsCount - 1, 0).Tag = VpSTag
                VpGrid(.RowsCount - 1, 0).Behaviors.Add(VpCellBehavior)
                VpGrid(.RowsCount - 1, 1) = New Cells.Cell(VpN)
                VpGrid(.RowsCount - 1, 2) = New Cells.Cell(0)
                VpGrid(.RowsCount - 1, 2).DataModel = VpCellModel
            End With
        End If
    End Sub
    Private Sub LoadForGrid(VpGrid As Grid, VpRequete As String, VpCategorie As String, VpGroupBy As String)
    '----------------------------------------------------------
    'Charge la liste des éléments disponibles pour l'estimation
    '----------------------------------------------------------
    Dim VpSQL As String = VpRequete
    Dim VpCellModel As DataModels.IDataModel = Utility.CreateDataModel(Type.GetType("System.Int32"))
    Dim VpCellBehavior As New BehaviorModels.CustomEvents
        VpCellModel.EditableMode = EditableMode.AnyKey Or EditableMode.DoubleClick
        AddHandler VpCellModel.Validated, AddressOf CellValidated
        AddHandler VpCellBehavior.Click, AddressOf CellMouseClick
        AddHandler VpCellBehavior.KeyUp, AddressOf CellMouseClick
        'Préparation de la grille
        With VpGrid
            'Nettoyage
            If .Rows.Count > 0 Then
                .Rows.RemoveRange(0, .Rows.Count)
            End If
            'Nombre de colonnes et d'en-têtes
            .ColumnsCount = 3
            .FixedRows = 1
            .Rows.Insert(0)
            VpGrid(0, 0) = New Cells.ColumnHeader("Elément")
            VpGrid(0, 1) = New Cells.ColumnHeader("Disponibles")
            VpGrid(0, 2) = New Cells.ColumnHeader("Demandé(s)")
            VpSQL = VpSQL + VmRestrictionSQL
            VpSQL = clsModule.TrimQuery(VpSQL, , VpGroupBy)
            VgDBCommand.CommandText = VpSQL
            VgDBReader = VgDBCommand.ExecuteReader
            With VgDBReader
                While .Read
                    Call Me.InsertRow(VpGrid, clsModule.FormatTitle(VpCategorie, .GetValue(0).ToString, , False), .GetValue(0).ToString, CInt(.GetValue(1)), VpCellModel, VpCellBehavior)
                End While
                .Close
            End With
            .AutoSize
        End With
    End Sub
    Private Sub LoadForGrids
        Call Me.LoadForGrid(Me.grdCardsDispos, "Select Card.Title, Sum(" + VmSource + ".Items) From (Card Inner Join " + VmSource + " On " + VmSource + ".EncNbr = Card.EncNbr) Where" + VmRestrictionReserve, "Card.Title", " Group By Card.Title")
        Call Me.LoadForGrid(Me.grdTypesDispos, "Select Card.Type, Sum(" + VmSource + ".Items) From (Card Inner Join " + VmSource + " On " + VmSource + ".EncNbr = Card.EncNbr) Where" + VmRestrictionReserve, "Card.Type", " Group By Card.Type")
        Call Me.LoadForGrid(Me.grdSubTypesDispos, "Select Card.SubType, Sum(" + VmSource + ".Items) From (Card Inner Join " + VmSource + " On " + VmSource + ".EncNbr = Card.EncNbr) Where" + VmRestrictionReserve, "Card.SubType", " Group By Card.SubType")
        Call Me.LoadForGrid(Me.grdCostsDispos, "Select Spell.myCost, Sum(" + VmSource + ".Items) From (Card Inner Join " + VmSource + " On " + VmSource + ".EncNbr = Card.EncNbr) Inner Join Spell On Card.Title = Spell.Title Where Card.Type <> 'L' And" + VmRestrictionReserve, "Spell.myCost", " Group By Spell.myCost")
        Call Me.LoadForGrid(Me.grdColorsDispos, "Select Spell.Color, Sum(" + VmSource + ".Items) From (Card Inner Join " + VmSource + " On " + VmSource + ".EncNbr = Card.EncNbr) Inner Join Spell On Card.Title = Spell.Title Where" + VmRestrictionReserve, "Spell.Color", " Group By Spell.Color")
    End Sub
    Private Sub CombosSimu
    '----------------------------------------------------------------------------------------------
    'Estime les probabilités simple et combinée d'apparition des cartes sélectionnées au nième tour
    '----------------------------------------------------------------------------------------------
    Dim VpPartie As New clsPartie(VmSource, VmRestrictionSQL + VmRestrictionReserve, True)
    Dim VpEspCumul As New clsEsperance(Me.txtN.Text)
        Me.prgSimu.Maximum = CInt(Me.txtN.Text)
        Me.prgSimu.Value = 0
        'Simulation des N parties
        For VpI As Integer = 1 To CInt(Me.txtN.Text)
            'Mélange le jeu
            Call VpPartie.DeckShuffle
            'Gestion des n tours
            For VpJ As Integer = 0 To VpPartie.CardsCount - clsModule.CgNMain
                'Au premier tour on pioche 7 cartes
                If VpJ = 0 Then
                    Call VpPartie.Draw(clsModule.CgNMain)
                'Les suivants une seule
                Else
                    Call VpPartie.Draw
                End If
                'Vérifie si les séquences demandées sont présentes
                For Each VpSequence As clsComboSequence In Me.chklstSequencesDispos.CheckedItems
                    Call VpPartie.UntagAll
                    If VpPartie.IsSequencePresent(VpSequence) Then
                        VpEspCumul.AddForRound(VpJ)
                        Exit For
                    End If
                Next VpSequence
            Next VpJ
            Me.prgSimu.Value = VpI
            Application.DoEvents
        Next VpI
        Me.btAddPlot.Enabled = True
        'Tours disponibles
        VpEspCumul.AddRoundsDispos(Me.cboTourCumul)
        'Mémorisation espérances
        Me.cboTourCumul.Tag = VpEspCumul.GetEsp
        'Sélection par défaut
        If Me.cboTourCumul.Items.Count > 0 Then Me.cboTourCumul.SelectedIndex = 0
    End Sub
    Private Sub DeploySimu
    '---------------------------------------------------------------
    'Estime l'espérance du nombre de manas disponibles au nième tour
    '---------------------------------------------------------------
    Dim VpVerbose As Boolean = Me.chkVerbosity.Checked
    Dim VpPartie As New clsPartie(VmSource, VmRestrictionSQL + VmRestrictionReserve, True, VpVerbose, VmSimuOut)    'Partie en simulation
    Dim VpEspDeploy As New clsEsperance(Me.txtN2.Text)                                      'Résultats
    Dim VpEspInvoc As New clsEsperance(Me.txtN2.Text)                                       'Références
    Dim VpTmpInPlay As New List(Of clsCard)                                                 'Support liste temporaire 1
    Dim VpTmpInRound As New List(Of clsCard)                                                'Support liste temporaire 2
    Dim VpSomething As Boolean                                                              'Au moins une action spéciale exécutée
    Dim VpPrevious As Integer                                                               'Réserve de manas au tour précédent
        If VpPartie.CardsCount < clsModule.CgNMain Then Exit Sub
        Me.prgSimu2.Maximum = CInt(Me.txtN2.Text)
        Me.prgSimu2.Value = 0
        'Retire la spécification automatique pour les cartes spéciales
        Call Me.ManualSpec(VpPartie, Me.lstUserCombos.Items, False)
        'Spécification manuelle des cartes spéciales cochées
        Call Me.ManualSpec(VpPartie, Me.lstUserCombos.CheckedItems, True)
        'Simulation des N parties
        For VpI As Integer = 1 To CInt(Me.txtN2.Text)
            VpPrevious = 0
            'Mélange le jeu
            Call VpPartie.DeckShuffle
            'Gestion des n tours
            For VpJ As Integer = 0 To VpPartie.CardsCount - clsModule.CgNMain
                Call clsModule.VerboseSimu(VpVerbose, "Tour " + VpJ.ToString, VmSimuOut)
                'Phase de dégagement
                Call VpPartie.UntapAll
                'Au premier tour on pioche 7 cartes
                If VpJ = 0 Then
                    Call VpPartie.Draw(clsModule.CgNMain)
                'Les suivants une seule
                Else
                    Call VpPartie.Draw
                End If
                'Calcule et conserve le nombre de manas dont on manque pour le tour précédent
                Call VpEspInvoc.AddForRound(VpJ, VpPartie.GetMissingCost(VpPartie.CardsDrawn, VpPrevious))
                'Joue un terrain s'il y en a un (de la couleur la plus astucieuse)
                Call VpPartie.AddToInPlay(VpPartie.PickLand)
                'Détermine la réserve totale de manas courante
                Call VpPartie.PrepNewPlayRound
                'Invoque les cartes génératrices de manas que l'on peut (en commençant par celles ayant le potentiel le plus élevé et/ou les cartes spéciales)
                '- les permanents restent en jeu
                '- les éphémères doivent partir à la fin du tour
                VpPartie.CardsDrawn.Sort(New clsCard.clsManasPotComparer(Me.lstUserCombos))
                VpTmpInPlay.Clear
                VpTmpInRound.Clear
                For Each VpCard As clsCard In VpPartie.CardsDrawn
                    If (VpCard.ManaAble Or VpCard.IsSpecial) AndAlso (Not VpCard.IsALand) AndAlso VpPartie.Reserve.ContainsEnoughFor(VpCard.ManasInvoc) Then
                        Call VpPartie.Reserve.AddSubManas(VpCard.ManasInvoc, -1)
                        If VpCard.CardType = "I" Or VpCard.CardType = "N" Or VpCard.CardType = "S" Then
                            VpTmpInRound.Add(VpCard)
                            Call clsModule.VerboseSimu(VpVerbose, "Sort joué : " + VpCard.CardName, VmSimuOut)
                        Else
                            'Si la carte arrive en jeu engagée ou est soumise au mal d'invocation
                            If VpCard.IsSpecial AndAlso VpCard.Speciality.InvocTapped Then
                                VpCard.Tapped = True
                                Call clsModule.VerboseSimu(VpVerbose, "Carte posée (engagée) : " + VpCard.CardName, VmSimuOut)
                            Else
                                Call clsModule.VerboseSimu(VpVerbose, "Carte posée : " + VpCard.CardName, VmSimuOut)
                            End If
                            VpTmpInPlay.Add(VpCard)
                        End If
                    End If
                Next VpCard
                Call VpPartie.CommitChange(VpTmpInPlay, VpPartie.CardsInPlay)
                Call VpPartie.CommitChange(VpTmpInRound, VpPartie.CardsInRound)
                Call VpPartie.FollowRound
                'Active les effets des cartes spéciales (tant qu'il y a quelque chose à faire)
                Call VpPartie.DoSpecialEffects(VpPartie.CardsInRound)
                Do
                    VpSomething = VpPartie.DoSpecialEffects(VpPartie.CardsInPlay)
                Loop Until VpSomething = False
                'Calcule le potentiel fourni par les cartes posées (permanents et éphémères) non engagées : l'espérance du tour courant vaut :
                '- ce qu'il reste dans la réserve
                '- ce qui a été apporté par les nouveaux permanents
                '- ce qui a été apporté par les éphémères du tour courant)
                VpPrevious = VpPartie.Reserve.Potentiel + VpPartie.ManasPotentielIn(VpPartie.CardsInPlay) + VpPartie.ManasPotentielIn(VpPartie.CardsInRound)
                Call VpEspDeploy.AddForRound(VpJ, VpPrevious)
                Call clsModule.VerboseSimu(VpVerbose, "Fin du tour, manas disponibles : " + VpPrevious.ToString, VmSimuOut)
            Next VpJ
            Me.prgSimu2.Value = VpI
            Application.DoEvents
        Next VpI
        Me.cmdAddPlot2.Enabled = True
        'Tours disponibles
        Call VpEspDeploy.AddRoundsDispos(Me.cboTourDeploy)
        Call VpEspInvoc.AddRoundsDispos(Me.cboTourDeploy2)
        'Mémorisation espérances
        Me.cboTourDeploy.Tag = VpEspDeploy.GetEsp(False)
        Me.cboTourDeploy2.Tag = VpEspInvoc.GetEsp(False)
        'Sélection par défaut
        If Me.cboTourDeploy.Items.Count > 0 Then Me.cboTourDeploy.SelectedIndex = 0
        If Me.cboTourDeploy2.Items.Count > 0 Then Me.cboTourDeploy2.SelectedIndex = 0
        'Finalisation verbosité
        If Me.chkVerbosity.Checked Then
            Call clsModule.VerboseSimu(True, "", VmSimuOut, True)
            Process.Start(Me.dlgVerbose.FileName)
        End If
    End Sub
    Private Sub ManualSpec(VpPartie As clsPartie, VpCollection As ICollection, VpAdd As Boolean)
    '--------------------------------------------
    'Gestion de l'attribut d'effet spécial manuel
    '--------------------------------------------
        For Each VpCard As clsCard In VpPartie.CardsInFullDeck
            For Each VpSpecial As String In VpCollection
                If VpCard.CardName = VpSpecial Then
                    If VpAdd Then
                        VpCard.Speciality = New clsSpeciality(VpSpecial)
                    Else
                        VpCard.Speciality = Nothing
                    End If
                End If
            Next VpSpecial
        Next VpCard
    End Sub
    Private Function QueryInfo(VpQuery As String, Optional VpClause As String = " Where ") As Object
    '------------------
    'Requête ponctuelle
    '------------------
    Dim VpSQL As String
        VpSQL = "Select " + VpQuery + " From (" + VmSource + " Inner Join Card On " + VmSource + ".EncNbr = Card.EncNbr) Inner Join Spell On Card.Title = Spell.Title" + VpClause
        VpSQL = VpSQL + VmRestrictionSQL
        VgDBCommand.CommandText = clsModule.TrimQuery(VpSQL)
        Return VgDBCommand.ExecuteScalar
    End Function
    Private Function DetectTheme As Boolean
    '------------------------------------------
    'Essaie d'analyser le thème du jeu en cours
    '------------------------------------------
    Dim VpPartie As New clsPartie(VmSource, VmRestrictionSQL + VmRestrictionReserve, True)
    Dim VpCards As List(Of clsCard)
    Dim VpX() As String
    Dim VpY() As String
    Dim VpS As New List(Of clsCorrelation)
    Dim VpSQL As String
    Dim VpM As Single
    Dim VpV As Single
        If VpPartie.CardsCount < clsModule.CgNMain Then
            Call clsModule.ShowWarning("Il faut avoir au moins 2 cartes saisies pour déterminer des suggestions...")
            Return False
        End If
        'Extraction des paramètres primaires du jeu (couleurs, prix moyen, coûts d'invocation, éditions)
        Me.txtColors.Text = ""
        Me.txtColors.Tag = ""
        Me.txtEditions.Text = ""
        Me.txtEditions.Tag = ""
        VpSQL = "Select Card.Series, Spell.Color From (Card Inner Join " + VmSource + " On " + VmSource + ".EncNbr = Card.EncNbr) Inner Join Spell On Card.Title = Spell.Title Where "
        VpSQL = VpSQL + VmRestrictionSQL
        VpSQL = clsModule.TrimQuery(VpSQL)
        VgDBCommand.CommandText = VpSQL
        VgDBReader = VgDBCommand.ExecuteReader
        With VgDBReader
            While .Read
                ' - couleurs
                If Not Me.txtColors.Tag.Contains(.GetValue(1).ToString) Then
                    Me.txtColors.Tag = Me.txtColors.Tag + .GetValue(1).ToString + ";"
                    Me.txtColors.Text = Me.txtColors.Text + clsModule.FormatTitle("Spell.Color", .GetValue(1)) + ";"
                End If
                ' - éditions
                If Not Me.txtEditions.Tag.Contains(.GetValue(0).ToString) Then
                    Me.txtEditions.Tag = Me.txtEditions.Tag + .GetValue(0).ToString + ";"
                    Me.txtEditions.Text = Me.txtEditions.Text + clsModule.FormatTitle("Card.Series", .GetValue(0)) + ";"
                End If
            End While
            .Close
        End With
        ' - prix moyen
        VpM = Me.QueryInfo("Sum(Price * Items) / Sum(Items)")
        VpV = Me.QueryInfo("Sum(((Price - " + VpM.ToString.Replace(",", ".") + ") ^ 2) * Items) / Sum(Items)")
        VpV = VpV ^ 0.5
        Me.txtPrix.Text = Format(VpM, "0.00") + " ± " + Format(VpV, "0.00") + " €"
        Me.txtPrix.Tag = VpV
        ' - coûts d'invocation
        VpM = Me.QueryInfo("Sum(myCost * Items) / Sum(Items)", " Where ( Cost <> Null ) And ")
        VpV = Me.QueryInfo("Sum(((myCost - " + VpM.ToString.Replace(",", ".") + ") ^ 2) * Items) / Sum(Items)", " Where ( Cost <> Null ) And ")
        VpV = VpV ^ 0.5
        Me.txtInvoc.Text = Format(VpM, "0.0") + " ± " + Format(VpV, "0.0")
        Me.txtInvoc.Tag = VpV
        'Extraction des séquences communes entre les textes de toutes les cartes
        VpCards = VpPartie.GetDistinctCards
        Me.prgSuggest.Maximum = VpCards.Count
        For VpI As Integer = 0 To VpCards.Count - 1
            For VpJ As Integer = VpI + 1 To VpCards.Count - 1
                VpX = VpCards(VpI).CardText.ToLower.Replace(".", "").Replace(",", "").Replace(";", "").Replace(":", "").Replace("!", "").Replace("?", "").Replace("  ", "").Split(" ")
                VpY = VpCards(VpJ).CardText.ToLower.Replace(".", "").Replace(",", "").Replace(";", "").Replace(":", "").Replace("!", "").Replace("?", "").Replace("  ", "").Split(" ")
                VpS.Add(New clsCorrelation(VpCards(VpI).CardName, VpCards(VpJ).CardName, clsCorrelation.LongestSequence(VpX, VpY)))
            Next VpJ
            Me.prgSuggest.Increment(1)
            Application.DoEvents
        Next VpI
        Me.Expressions = VpS
        Return True
    End Function
    Private Sub RestrCorr(ByRef VpSQL As String, VpR As String, VpField As String)
    '-------------------------------------
    'Restriction de la requête par critère
    '-------------------------------------
    Dim VpStr As String
        VpSQL = VpSQL + "("
        For Each VpStr In VpR.Split(";")
            If VpStr.Trim <> "" Then
                VpSQL = VpSQL + VpField + " = '" + VpStr + "' Or "
            End If
        Next VpStr
        VpSQL = VpSQL.Substring(0, VpSQL.Length - 4) + ") And "
    End Sub
    Private Sub FullCorrelation(VpS As List(Of clsCorrelation))
    '------------------------------------------------------------------------------------------------------------------------
    'Corrélation entre les séquences extraites lors de la détection précédente et les textes des cartes de la base de données
    '------------------------------------------------------------------------------------------------------------------------
    Dim VpX() As String
    Dim VpSQL As String
    Dim VpCorrCoeff As Single
    Dim VpSuggest As New List(Of clsCorrelation)
    Dim VpAlready As New List(Of clsCorrelation)
    Dim VpN As Integer
    Dim VpSeq As String
        'Croisement avec les cartes de la base de données restreintes aux critères sélectionnés par l'utilisateur
        VpSQL = "Select Card.Title, Card.CardText, Card.EncNbr From Card Inner Join Spell On Card.Title = Spell.Title Where "
        ' - couleurs
        If Me.chkColors.Checked Then
            Call Me.RestrCorr(VpSQL, Me.txtColors.Tag.ToString, "Spell.Color")
        End If
        ' - éditions
        If Me.chkEditions.Checked Then
            Call Me.RestrCorr(VpSQL, Me.txtEditions.Tag.ToString, "Card.Series")
        End If
        ' - prix moyen
        If Me.chkPrix.Checked Then
            VpSQL = VpSQL + "(Card.Price > " + (Val(Me.txtPrix.Text) - CSng(Me.txtPrix.Tag)).ToString.Replace(",", ".") + " And Card.Price < " + (Val(Me.txtPrix.Text) + CSng(Me.txtPrix.Tag)).ToString.Replace(",", ".") + ") And "
        End If
        ' - coûts d'invocation
        If Me.chkInvoc.Checked Then
            VpSQL = VpSQL + "((Spell.myCost > " + (Val(Me.txtInvoc.Text) - CSng(Me.txtInvoc.Tag)).ToString.Replace(",", ".") + " And Spell.myCost < " + (Val(Me.txtInvoc.Text) + CSng(Me.txtInvoc.Tag)).ToString.Replace(",", ".") + ") Or Spell.myCost = 0) And "
        End If
        VpSQL = clsModule.TrimQuery(VpSQL, False)
        VgDBCommand.CommandText = VpSQL
        VgDBReader = VgDBCommand.ExecuteReader
        VpCorrCoeff = clsCorrelation.GetMean(VpS)
        With VgDBReader
            While .Read
                VpX = .GetValue(1).ToString.ToLower.Replace(".", "").Replace(",", "").Replace(";", "").Replace(":", "").Replace("!", "").Replace("?", "").Replace("  ", "").Split(" ")
                For Each VpCorr As clsCorrelation In VpS
                    VpN = VpCorr.Seq.Split(" ").Length
                    'Si la séquence commune entre la carte courante et la séquence courante vaut la taille de cette dernière (à 1 près), on conserve cette carte
                    If CSng(VpN) > VpCorrCoeff * (Me.sldPertin.Value / clsModule.CgPertinCoeff) Then
                        VpSeq = clsCorrelation.LongestSequence(VpX, VpCorr.Seq.Split(" "))
                        If VpN - VpSeq.Split(" ").Length <= 1 Then
                            If Not clsCorrelation.MyContains(VpSuggest, .GetString(0)) Then
                                VpSuggest.Add(New clsCorrelation(.GetString(0), "", VpSeq))
                            End If
                        End If
                    End If
                Next VpCorr
            End While
            .Close
        End With
        'Supprime les cartes déjà présentes dans le jeu
        For Each VpCard As clsCard In (New clsPartie(VmSource, VmRestrictionSQL + VmRestrictionReserve, True)).GetDistinctCards
            For Each VpSuggested As clsCorrelation In VpSuggest
                If VpCard.CardName = VpSuggested.Card1 Then
                    VpAlready.Add(VpSuggested)
                End If
            Next VpSuggested
        Next VpCard
        For Each VpToRemove As clsCorrelation In VpAlready
            VpSuggest.Remove(VpToRemove)
        Next VpToRemove
        'Affichage dans le treeview
        Call clsModule.ShowInformation(VpSuggest.Count.ToString + " occurence(s) trouvée(s).")
        With MainForm.VgMe
            .Suggestions = VpSuggest
            .mnuDispAdvSearch.Enabled = True
            Call .ManageDispMenu(.mnuDispAdvSearch.Text, False)
            Call .LoadTvw("(" + VpSQL + ") As " + clsModule.CgSFromSearch)
        End With
    End Sub
    Private Sub GestVisible(Optional VpCombos As Boolean = False, Optional VpDeploy As Boolean = False, Optional VpSuggest As Boolean = False)
    '------------------
    'Gestion des panels
    '------------------
        'Groupe visible
        Me.grpCombos.Visible = VpCombos
        Me.grpDeploy.Visible = VpDeploy
        Me.grpSuggest.Visible = VpSuggest
        'Bouton coché
        Me.btCombos.Checked = VpCombos
        Me.btDeploy.Checked = VpDeploy
        Me.btSuggest.Checked = VpSuggest
    End Sub
    Private Sub GestPriority(VpSens As Short)
    '------------------------------------------------
    'Gestions des priorités pour les cartes spéciales
    '------------------------------------------------
    Dim VpItem As Integer = Me.cmnuUserCombos.Tag
    Dim VpTmp As String
        VpTmp = Me.lstUserCombos.Items(VpItem + VpSens)
        Me.lstUserCombos.Items(VpItem + VpSens) = Me.lstUserCombos.Items(VpItem)
        Me.lstUserCombos.Items(VpItem) = VpTmp
    End Sub
    Private Sub Clear1(VpGrid As Grid)
        For VpI As Integer = 1 To VpGrid.RowsCount - 1
            VpGrid(VpI, 2).Value = 0
        Next VpI
    End Sub
    Private Sub Clear(Optional VpAll As Boolean = False)
        Call Me.Clear1(Me.grdCardsDispos)
        Call Me.Clear1(Me.grdTypesDispos)
        Call Me.Clear1(Me.grdSubTypesDispos)
        Call Me.Clear1(Me.grdCostsDispos)
        Call Me.Clear1(Me.grdColorsDispos)
        If VpAll Then
            Me.chklstSequencesDispos.Items.Clear
        End If
    End Sub
    #End Region
    #Region "Evènements"
    Sub CellValidated(sender As Object, e As CellEventArgs)
    Dim VpCell As Cells.Cell = e.Cell
    Dim VpGrid As Grid = VpCell.Grid
    Dim VpMax As Integer = CInt(VpGrid(VpCell.Row, VpCell.Column - 1).Value)
        If CInt(VpCell.Value) < 0 Then
            VpCell.Value = 0
        ElseIf CInt(VpCell.Value) > VpMax Then
            VpCell.Value = VpMax
        End If
    End Sub
    Sub CellMouseClick(sender As Object, e As PositionEventArgs)
        Call clsModule.LoadScanCard(e.Cell.GetValue(e.Position), 0, Me.picScanCard2)
    End Sub
    Sub ChklstSequencesDisposSelectedIndexChanged(sender As Object, e As EventArgs)
    Dim VpSequence As clsComboSequence
        If Me.chklstSequencesDispos.SelectedIndex >= 0 Then
            VpSequence = Me.chklstSequencesDispos.SelectedItem
            Call Me.Clear
            For Each VpElement As clsElement In VpSequence.Elements
                Select Case VpElement.ElementType
                    Case clsElement.eElementType.Card
                        Call Me.IncrementRow(Me.grdCardsDispos, VpElement.ElementValue)
                    Case clsElement.eElementType.Color
                        Call Me.IncrementRow(Me.grdColorsDispos, VpElement.ElementValue)
                    Case clsElement.eElementType.Cost
                        Call Me.IncrementRow(Me.grdCostsDispos, VpElement.ElementValue)
                    Case clsElement.eElementType.SubType
                        Call Me.IncrementRow(Me.grdSubTypesDispos, VpElement.ElementValue)
                    Case clsElement.eElementType.Type
                        Call Me.IncrementRow(Me.grdTypesDispos, VpElement.ElementValue)
                    Case Else
                End Select
            Next VpElement
        End If
    End Sub
    Sub BtClearAllClick(sender As Object, e As EventArgs)
        Call Me.Clear(True)
    End Sub
    Sub BtRemoveClick(sender As Object, e As EventArgs)
        If Me.chklstSequencesDispos.SelectedIndex >= 0 Then
            Me.chklstSequencesDispos.Items.Remove(Me.chklstSequencesDispos.SelectedItem)
            Call Me.Clear
        End If
    End Sub
    Sub BtAddSequenceClick(sender As Object, e As EventArgs)
    Dim VpSequence As New clsComboSequence
        Call Me.AddToSequence(VpSequence, Me.grdCardsDispos, clsElement.eElementType.Card)
        Call Me.AddToSequence(VpSequence, Me.grdTypesDispos, clsElement.eElementType.Type)
        Call Me.AddToSequence(VpSequence, Me.grdSubTypesDispos, clsElement.eElementType.SubType)
        Call Me.AddToSequence(VpSequence, Me.grdCostsDispos, clsElement.eElementType.Cost)
        Call Me.AddToSequence(VpSequence, Me.grdColorsDispos, clsElement.eElementType.Color)
        If VpSequence.Elements.Count > 0 Then
            Me.chklstSequencesDispos.Items.Add(VpSequence, True)
            Call Me.Clear
        Else
            Call clsModule.ShowWarning("Une séquence doit contenir au moins une carte.")
        End If
    End Sub
    Sub BtCombosActivate(ByVal sender As Object, ByVal e As EventArgs)
        Call Me.GestVisible(True)
        Call Me.LoadForGrids
    End Sub
    Sub BtDeployActivate(ByVal sender As Object, ByVal e As EventArgs)
        Call Me.GestVisible(, True)
        Call Me.LoadSpecialUses
    End Sub
    Sub BtSuggestActivate(sender As Object, e As EventArgs)
        Call Me.GestVisible(, , True)
    End Sub
    Sub BtSimusClick(sender As Object, e As EventArgs)
        Me.cboTourCumul.Text = ""
        Me.txtEspCumul.Text = ""
        If Me.chklstSequencesDispos.CheckedItems.Count > 0 Then
            Call Me.CombosSimu
        Else
            Call clsModule.ShowWarning("Créer au moins une séquence avant de lancer le calcul.")
        End If
    End Sub
    Sub CboTourCumulSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
    Dim VpEspCumul As SortedList = Me.cboTourCumul.Tag
        Me.txtEspCumul.Text = Format(VpEspCumul.Item(CInt(Me.cboTourCumul.Text) - 1), "0.00")
    End Sub
    Sub CmdSimu2Click(ByVal sender As Object, ByVal e As EventArgs)
        If Me.chkVerbosity.Checked Then
            Me.dlgVerbose.FileName = ""
            Me.dlgVerbose.ShowDialog
            If Me.dlgVerbose.FileName <> "" Then
                VmSimuOut = New StreamWriter(Me.dlgVerbose.FileName)
            Else
                Exit Sub
            End If
        End If
        Call Me.DeploySimu
    End Sub
    Sub CboTourDeploySelectedIndexChanged(sender As Object, e As EventArgs)
    Dim VpEspDeploy As SortedList = Me.cboTourDeploy.Tag
        Me.txtEspManas.Text = Format(VpEspDeploy.Item(CInt(Me.cboTourDeploy.Text) - 1), "0.00")
    End Sub
    Sub CboTourDeploy2SelectedIndexChanged(sender As Object, e As EventArgs)
    Dim VpEspInvoc As SortedList = Me.cboTourDeploy2.Tag
        Me.txtEspDefaut.Text = Format(VpEspInvoc.Item(CInt(Me.cboTourDeploy2.Text) - 1), "0.00")
    End Sub
    Sub BtAddPlotClick(sender As Object, e As EventArgs)
    Dim VpEspCumul As SortedList = Me.cboTourCumul.Tag
        VmGraphCount = VmGraphCount + 1
        VmGrapher.AddNewPlot(VpEspCumul, "(" + VmGraphCount.ToString + ") " + clsModule.CgSimus3 + VmRestrictionTXT)
        VmGrapher.Show
        VmGrapher.BringToFront
    End Sub
    Sub CmdAddPlot2Click(sender As Object, e As EventArgs)
    Dim VpEspDeploy As SortedList = Me.cboTourDeploy.Tag
    Dim VpEspInvoc As SortedList = Me.cboTourDeploy2.Tag
        VmGrapher.AddNewPlot(VpEspDeploy, clsModule.CgSimus4 + VmRestrictionTXT)
        If Me.chkDefaut.Checked Then
            VmGrapher.AddNewPlot(VpEspInvoc, clsModule.CgSimus5 + VmRestrictionTXT)
        End If
        VmGrapher.Show
        VmGrapher.BringToFront
    End Sub
    Sub FrmSimuLoad(sender As Object, e As EventArgs)
        Call Me.LoadForGrids
    End Sub
    Sub LstUserCombosMouseUp(sender As Object, e As MouseEventArgs)
    Dim VpItem As Integer = Me.lstUserCombos.IndexFromPoint(e.Location)
        If e.Button = System.Windows.Forms.MouseButtons.Right Then
            Me.cmnuDelete.Enabled = ( VpItem >= 0 )
            Me.cmnuUp.Enabled = ( VpItem > 0 )
            Me.cmnuDown.Enabled = ( VpItem < Me.lstUserCombos.Items.Count - 1 )
            Me.cmnuUserCombos.Tag = VpItem
            Me.cmnuUserCombos.Show(sender, e.Location)
        End If
    End Sub
    Sub CmnuAddNewClick(sender As Object, e As EventArgs)
    Dim VpSpecialCard As New frmSpecialCardUse(VmSource, VmRestrictionSQL)
        VpSpecialCard.ShowDialog
        Call Me.LoadSpecialUses
    End Sub
    Sub CmnuDeleteClick(ByVal sender As Object, ByVal e As EventArgs)
        VgDBCommand.CommandText = "Delete * From MySpecialUses Where Card = '" + Me.lstUserCombos.SelectedItem.Replace("'", "''") + "';"
        VgDBCommand.ExecuteNonQuery
        Call Me.LoadSpecialUses
    End Sub
    Sub TxtN2TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Me.chkVerbosity.Checked = ( Val(Me.txtN2.Text) = 1 )
    End Sub
    Sub ChkVerbosityCheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Me.chkVerbosity.Checked Then
            Me.txtN2.Text = "1"
        End If
    End Sub
    Sub CmnuUpMouseClick(ByVal sender As Object, ByVal e As EventArgs)
        Call Me.GestPriority(-1)
    End Sub
    Sub CmnuDownMouseClick(ByVal sender As Object, ByVal e As EventArgs)
        Call Me.GestPriority(1)
    End Sub
    Sub CmdDetectClick(sender As Object, e As EventArgs)
        If Me.DetectTheme Then
            Me.lbl16.Enabled = True
            Me.lbl17.Enabled = True
            Me.sldPertin.Enabled = True
            Me.cmdCorrExpr.Enabled = True
            Me.chkColors.Enabled = True
            Me.chkInvoc.Enabled = True
            Me.chkEditions.Enabled = True
            Me.chkPrix.Enabled = True
            Me.cmdSuggest.Enabled = True
            Me.txtColors.Enabled = True
            Me.txtInvoc.Enabled = True
            Me.txtEditions.Enabled = True
            Me.txtPrix.Enabled = True
        End If
    End Sub
    Sub CmdCorrExprClick(sender As Object, e As EventArgs)
    Dim VpCorrExpr As New frmCorrExpr(Me)
        VpCorrExpr.ShowDialog
    End Sub
    Sub CmdSuggestClick(sender As Object, e As EventArgs)
        If ShowQuestion("L'arborescence de la fenêtre principale va être remplacée par le résultat des suggestions..." + vbCrlf + "Continuer ?") = DialogResult.Yes Then
            Call Me.FullCorrelation(Me.Expressions)
        End If
    End Sub
    #End Region
    #Region "Propriétés"
    Public Property Expressions As List(Of clsCorrelation)
        Get
            Return VmExpr
        End Get
        Set (VpExpr As List(Of clsCorrelation))
            VmExpr = VpExpr
        End Set
    End Property
    #End Region
End Class
