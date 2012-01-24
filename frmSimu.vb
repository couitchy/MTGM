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
'| - esp�rance de d�ploiement			   01/08/2009 |
'| - lev�e de l'ambiguit� sur les sources  03/10/2009 |
'| - d�ploiement - effets sp�ciaux		   08/11/2009 |
'| - suggestions de cartes adapt�es		   25/02/2010 |
'| - cartes suivantes apr�s tirage mains   09/05/2011 |
'------------------------------------------------------
Imports SourceGrid2
Imports Cells = SourceGrid2.Cells.Real
Imports System.IO
Public Partial Class frmSimu
	Private Shared VmGrapher As New frmGrapher
	Private Shared VmGraphCount As Integer = 0
	Private VmSource As String
	Private VmRestrictionSQL As String
	Private VmRestrictionTXT As String
	Private VmSimuOut As StreamWriter
	Private VmExpr As List(Of clsCorrelation)
	#Region "M�thodes"
	Public Sub New(VpOwner As MainForm)
		Me.InitializeComponent()
		VmSource = If(VpOwner.FilterCriteria.DeckMode, clsModule.CgSDecks, clsModule.CgSCollection)
		VmRestrictionSQL = VpOwner.Restriction
		VmRestrictionTXT = VpOwner.Restriction(True)
		Me.Text = clsModule.CgSimus + VmRestrictionTXT
	End Sub
	Private Sub LoadSpecialUses
	'----------------------------------------------------------------------
	'Charge la liste des cartes poss�dant une utilisation sp�ciale associ�e
	'----------------------------------------------------------------------
	Dim VpPartie As New clsPartie(VmSource, VmRestrictionSQL)
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
	Private Sub LoadForCombos
	'--------------------------------------------------------
	'Charge la liste des cartes disponibles pour l'estimation
	'--------------------------------------------------------
	Dim VpSQL As String
		Me.lstCombosDispos.Items.Clear
		'VpSQL = "Select Card.Title, CardFR.TitleFR, " + VmSource + ".Items From ((Card Inner Join CardFR On Card.EncNbr = CardFR.EncNbr) Inner Join " + VmSource + " On " + VmSource + ".EncNbr = Card.EncNbr) Where "
		VpSQL = "Select Card.Title, " + VmSource + ".Items From (Card Inner Join " + VmSource + " On " + VmSource + ".EncNbr = Card.EncNbr) Where "
		VpSQL = VpSQL + VmRestrictionSQL
		VpSQL = clsModule.TrimQuery(VpSQL)
		VgDBCommand.CommandText = VpSQL
		VgDBReader = VgDBCommand.ExecuteReader
		With VgDBReader
			While .Read
				'Ajoute les doublons pour permettre de cr�er des s�quences avec des cartes en plusieurs exemplaires
				For VpI As Integer = 1 To .GetInt32(1)
					Me.lstCombosDispos.Items.Add(.GetString(0))
				Next VpI
			End While
			.Close
		End With
		Me.lstCombosDispos.Sorted = True
		Me.lstCombosDispos.Tag = New List(Of Integer)
	End Sub
	Private Sub CombosSimu
	'----------------------------------------------------------------------------------------------
	'Estime les probabilit�s simple et combin�e d'apparition des cartes s�lectionn�es au ni�me tour
	'----------------------------------------------------------------------------------------------
	Dim VpPartie As New clsPartie(VmSource, VmRestrictionSQL)
	Dim VpEspCumul As New clsEsperance(Me.txtN.Text)
		Me.prgSimu.Maximum = CInt(Me.txtN.Text)
		Me.prgSimu.Value = 0
		'Simulation des N parties
		For VpI As Integer = 1 To CInt(Me.txtN.Text)
			'M�lange le jeu
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
				'V�rifie si les s�quences demand�es sont pr�sentes
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
		'M�morisation esp�rances
		Me.cboTourCumul.Tag = VpEspCumul.GetEsp
		'S�lection par d�faut
		If Me.cboTourCumul.Items.Count > 0 Then Me.cboTourCumul.SelectedIndex = 0
	End Sub
	Private Sub ShowMain
	'------------------------
	'Affiche la main courante
	'------------------------
	Dim VpPartie As clsPartie
	Dim VpCell As Cells.Cell
	Dim VpCellBehavior As BehaviorModels.CustomEvents
		VpCellBehavior = New BehaviorModels.CustomEvents
		AddHandler VpCellBehavior.Click, AddressOf CellMouseClick
		AddHandler VpCellBehavior.DoubleClick, AddressOf CellMouseDoubleClick
		AddHandler VpCellBehavior.KeyUp, AddressOf CellKeyUp	
		With Me.grdMainsTirage
			VpPartie = .Tag
			'Nettoyage
			If .Rows.Count > 0 Then
				.Rows.RemoveRange(0, .Rows.Count)
			End If
			.ColumnsCount = 1
			.FixedRows = 0
			For Each VpCard As clsCard In VpPartie.CardsDrawn
				.Rows.Insert(.RowsCount)
				VpCell = New Cells.Cell(VpCard.CardNameFR + " (" + VpCard.CardName + ")")
				VpCell.Behaviors.Add(VpCellBehavior)
				VpCell.Tag = VpCard
				Me.grdMainsTirage(.RowsCount - 1, 0) = VpCell
			Next VpCard
			.Columns(0).Width = .Width
		End With	
	End Sub
	Private Sub MainsSimu
	'---------------------------------------
	'Tire al�atoirement une main de 7 cartes
	'---------------------------------------
	Dim VpPartie As New clsPartie(VmSource, VmRestrictionSQL)	
		If VpPartie.CardsCount < clsModule.CgNMain Then
			Call clsModule.ShowWarning("Il faut avoir au moins " + clsModule.CgNMain.ToString + " cartes saisies pour tirer une main...")
		Else
			'M�lange le jeu
			Call VpPartie.DeckShuffle
			'Tire les 7 cartes
			Call VpPartie.Draw(clsModule.CgNMain)
			'Les inscrit dans la grille
			Me.grdMainsTirage.Tag = VpPartie
			Call Me.ShowMain
			'Force l'affichage de la premi�re carte
			Call Me.CellMouseClick(Nothing, New PositionEventArgs(New Position(0, 0), Me.grdMainsTirage(0, 0)))
		End If
	End Sub
	Private Sub DeploySimu
	'---------------------------------------------------------------
	'Estime l'esp�rance du nombre de manas disponibles au ni�me tour
	'---------------------------------------------------------------
	Dim VpVerbose As Boolean = Me.chkVerbosity.Checked
	Dim VpPartie As New clsPartie(VmSource, VmRestrictionSQL, True, VpVerbose, VmSimuOut)	'Partie en simulation
	Dim VpEspDeploy As New clsEsperance(Me.txtN2.Text)										'R�sultats
	Dim VpEspInvoc As New clsEsperance(Me.txtN2.Text)										'R�f�rences
	Dim VpTmpInPlay As New List(Of clsCard)													'Support liste temporaire 1
	Dim VpTmpInRound As New List(Of clsCard)												'Support liste temporaire 2
	Dim VpSomething As Boolean																'Au moins une action sp�ciale ex�cut�e
	Dim VpPrevious As Integer																'R�serve de manas au tour pr�c�dent
		If VpPartie.CardsCount < clsModule.CgNMain Then Exit Sub
		Me.prgSimu2.Maximum = CInt(Me.txtN2.Text)
		Me.prgSimu2.Value = 0
		'Retire la sp�cification automatique pour les cartes sp�ciales
		Call Me.ManualSpec(VpPartie, Me.lstUserCombos.Items, False)
		'Sp�cification manuelle des cartes sp�ciales coch�es
		Call Me.ManualSpec(VpPartie, Me.lstUserCombos.CheckedItems, True)
		'Simulation des N parties
		For VpI As Integer = 1 To CInt(Me.txtN2.Text)
			VpPrevious = 0
			'M�lange le jeu
			Call VpPartie.DeckShuffle
			'Gestion des n tours
			For VpJ As Integer = 0 To VpPartie.CardsCount - clsModule.CgNMain
				Call clsModule.VerboseSimu(VpVerbose, "Tour " + VpJ.ToString, VmSimuOut)
				'Phase de d�gagement
				Call VpPartie.UntapAll
				'Au premier tour on pioche 7 cartes
				If VpJ = 0 Then
					Call VpPartie.Draw(clsModule.CgNMain)
				'Les suivants une seule
				Else
					Call VpPartie.Draw
				End If
				'Calcule et conserve le nombre de manas dont on manque pour le tour pr�c�dent
				Call VpEspInvoc.AddForRound(VpJ, VpPartie.GetMissingCost(VpPartie.CardsDrawn, VpPrevious))
				'Joue un terrain s'il y en a un (de la couleur la plus astucieuse)
				Call VpPartie.AddToInPlay(VpPartie.PickLand)
				'D�termine la r�serve totale de manas courante
				Call VpPartie.PrepNewPlayRound
				'Invoque les cartes g�n�ratrices de manas que l'on peut (en commen�ant par celles ayant le potentiel le plus �lev� et/ou les cartes sp�ciales)
				'- les permanents restent en jeu
				'- les �ph�m�res doivent partir � la fin du tour
				VpPartie.CardsDrawn.Sort(New clsManasPotComparer(Me.lstUserCombos))
				VpTmpInPlay.Clear
				VpTmpInRound.Clear
				For Each VpCard As clsCard In VpPartie.CardsDrawn
					If (VpCard.ManaAble Or VpCard.IsSpecial) AndAlso (Not VpCard.IsALand) AndAlso VpPartie.Reserve.ContainsEnoughFor(VpCard.ManasInvoc) Then
						Call VpPartie.Reserve.AddSubManas(VpCard.ManasInvoc, -1)
						If VpCard.CardType = "I" Or VpCard.CardType = "N" Or VpCard.CardType = "S" Then
							VpTmpInRound.Add(VpCard)
							Call clsModule.VerboseSimu(VpVerbose, "Sort jou� : " + VpCard.CardName, VmSimuOut)
						Else
							'Si la carte arrive en jeu engag�e ou est soumise au mal d'invocation
							If VpCard.IsSpecial AndAlso VpCard.Speciality.InvocTapped Then
								VpCard.Tapped = True
								Call clsModule.VerboseSimu(VpVerbose, "Carte pos�e (engag�e) : " + VpCard.CardName, VmSimuOut)
							Else
								Call clsModule.VerboseSimu(VpVerbose, "Carte pos�e : " + VpCard.CardName, VmSimuOut)
							End If
							VpTmpInPlay.Add(VpCard)
						End If
					End If
				Next VpCard
				Call VpPartie.CommitChange(VpTmpInPlay, VpPartie.CardsInPlay)
				Call VpPartie.CommitChange(VpTmpInRound, VpPartie.CardsInRound)
				Call VpPartie.FollowRound
				'Active les effets des cartes sp�ciales (tant qu'il y a quelque chose � faire)
				Call VpPartie.DoSpecialEffects(VpPartie.CardsInRound)
				Do
					VpSomething = VpPartie.DoSpecialEffects(VpPartie.CardsInPlay)
				Loop Until VpSomething = False
				'Calcule le potentiel fourni par les cartes pos�es (permanents et �ph�m�res) non engag�es : l'esp�rance du tour courant vaut :
				'- ce qu'il reste dans la r�serve
				'- ce qui a �t� apport� par les nouveaux permanents
				'- ce qui a �t� apport� par les �ph�m�res du tour courant)
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
		'M�morisation esp�rances
		Me.cboTourDeploy.Tag = VpEspDeploy.GetEsp(False)
		Me.cboTourDeploy2.Tag = VpEspInvoc.GetEsp(False)
		'S�lection par d�faut
		If Me.cboTourDeploy.Items.Count > 0 Then Me.cboTourDeploy.SelectedIndex = 0
		If Me.cboTourDeploy2.Items.Count > 0 Then Me.cboTourDeploy2.SelectedIndex = 0
		'Finalisation verbosit�
		If Me.chkVerbosity.Checked Then
			Call clsModule.VerboseSimu(True, "", VmSimuOut, True)
			Process.Start(Me.dlgVerbose.FileName)
		End If
	End Sub
	Private Sub ManualSpec(VpPartie As clsPartie, VpCollection As ICollection, VpAdd As Boolean)
	'--------------------------------------------
	'Gestion de l'attribut d'effet sp�cial manuel
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
	'Requ�te ponctuelle
	'------------------
	Dim VpSQL As String
		VpSQL = "Select " + VpQuery + " From (" + VmSource + " Inner Join Card On " + VmSource + ".EncNbr = Card.EncNbr) Inner Join Spell On Card.Title = Spell.Title" + VpClause
		VpSQL = VpSQL + VmRestrictionSQL
		VgDBCommand.CommandText = clsModule.TrimQuery(VpSQL)
		Return VgDBCommand.ExecuteScalar
	End Function
	Private Function DetectTheme As Boolean
	'------------------------------------------
	'Essaie d'analyser le th�me du jeu en cours
	'------------------------------------------
	Dim VpPartie As New clsPartie(VmSource, VmRestrictionSQL, True)
	Dim VpCards As List(Of clsCard)
	Dim VpX() As String
	Dim VpY() As String
	Dim VpS As New List(Of clsCorrelation)
	Dim VpSQL As String
	Dim VpM As Single
	Dim VpV As Single
		If VpPartie.CardsCount < clsModule.CgNMain Then
			Call clsModule.ShowWarning("Il faut avoir au moins 2 cartes saisies pour d�terminer des suggestions...")
			Return False
		End If
		'Extraction des param�tres primaires du jeu (couleurs, prix moyen, co�ts d'invocation, �ditions)
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
				' - �ditions
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
		Me.txtPrix.Text = Format(VpM, "0.00") + " � " + Format(VpV, "0.00") + " �"
		Me.txtPrix.Tag = VpV
		' - co�ts d'invocation
		VpM = Me.QueryInfo("Sum(myCost * Items) / Sum(Items)", " Where ( Cost <> Null ) And ")
		VpV = Me.QueryInfo("Sum(((myCost - " + VpM.ToString.Replace(",", ".") + ") ^ 2) * Items) / Sum(Items)", " Where ( Cost <> Null ) And ")
		VpV = VpV ^ 0.5
		Me.txtInvoc.Text = Format(VpM, "0.0") + " � " + Format(VpV, "0.0")
		Me.txtInvoc.Tag = VpV
		'Extraction des s�quences communes entre les textes de toutes les cartes
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
	'Restriction de la requ�te par crit�re
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
	'Corr�lation entre les s�quences extraites lors de la d�tection pr�c�dente et les textes des cartes de la base de donn�es
	'------------------------------------------------------------------------------------------------------------------------
	Dim VpX() As String
	Dim VpSQL As String
	Dim VpCorrCoeff As Single
	Dim VpSuggest As New List(Of clsCorrelation)
	Dim VpAlready As New List(Of clsCorrelation)
	Dim VpN As Integer
	Dim VpSeq As String
		'Croisement avec les cartes de la base de donn�es restreintes aux crit�res s�lectionn�s par l'utilisateur
		VpSQL = "Select Card.Title, Card.CardText, Card.EncNbr From Card Inner Join Spell On Card.Title = Spell.Title Where "
		' - couleurs
		If Me.chkColors.Checked Then
			Call Me.RestrCorr(VpSQL, Me.txtColors.Tag.ToString, "Spell.Color")
		End If
		' - �ditions
		If Me.chkEditions.Checked Then
			Call Me.RestrCorr(VpSQL, Me.txtEditions.Tag.ToString, "Card.Series")
		End If
		' - prix moyen
		If Me.chkPrix.Checked Then
			VpSQL = VpSQL + "(Card.Price > " + (Val(Me.txtPrix.Text) - CSng(Me.txtPrix.Tag)).ToString.Replace(",", ".") + " And Card.Price < " + (Val(Me.txtPrix.Text) + CSng(Me.txtPrix.Tag)).ToString.Replace(",", ".") + ") And "
		End If
		' - co�ts d'invocation
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
					'Si la s�quence commune entre la carte courante et la s�quence courante vaut la taille de cette derni�re (� 1 pr�s), on conserve cette carte
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
		'Supprime les cartes d�j� pr�sentes dans le jeu
		For Each VpCard As clsCard In (New clsPartie(VmSource, VmRestrictionSQL, True)).GetDistinctCards
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
		Call clsModule.ShowInformation(VpSuggest.Count.ToString + " occurence(s) trouv�e(s).")
		MainForm.VgMe.Suggestions = VpSuggest
		Call MainForm.VgMe.LoadTvw("(" + VpSQL + ") As " + clsModule.CgSFromSearch)
	End Sub
	Private Sub GestVisible(Optional VpMains As Boolean = False, Optional VpCombos As Boolean = False, Optional VpDeploy As Boolean = False, Optional VpSuggest As Boolean = False)
	'------------------
	'Gestion des panels
	'------------------
		'Groupe visible
		Me.grpMains.Visible = VpMains
		Me.grpCombos.Visible = VpCombos
		Me.grpDeploy.Visible = VpDeploy
		Me.grpSuggest.Visible = VpSuggest
		'Bouton coch�
		Me.btMains.Checked = VpMains
		Me.btCombos.Checked = VpCombos
		Me.btDeploy.Checked = VpDeploy
		Me.btSuggest.Checked = VpSuggest
	End Sub
	Private Sub GestPriority(VpSens As Short)
	'------------------------------------------------
	'Gestions des priorit�s pour les cartes sp�ciales
	'------------------------------------------------
	Dim VpItem As Integer = Me.cmnuUserCombos.Tag
	Dim VpTmp As String
		VpTmp = Me.lstUserCombos.Items(VpItem + VpSens)
		Me.lstUserCombos.Items(VpItem + VpSens) = Me.lstUserCombos.Items(VpItem)
		Me.lstUserCombos.Items(VpItem) = VpTmp
	End Sub
	Private Sub Clear(Optional VpAll As Boolean = False)
		Me.lstCombosDispos.ClearSelected
		CType(Me.lstCombosDispos.Tag, List(Of Integer)).Clear	
		If VpAll Then
			Me.chklstSequencesDispos.Items.Clear
		End If
	End Sub
	#End Region
	#Region "Ev�nements"
	Sub ChklstSequencesDisposSelectedIndexChanged(sender As Object, e As EventArgs)
	'----------------------------------------------------------------------------------------------------
	'Res�lectionne dans le listbox les �l�ments composant la s�quence s�lectionn�e dans le checkedlistbox
	'----------------------------------------------------------------------------------------------------
		If Me.chklstSequencesDispos.SelectedItems.Count > 0 Then
			Call Me.Clear
			For Each VpCard As String In CType(Me.chklstSequencesDispos.SelectedItem, clsComboSequence).CardsNames
				For VpI As Integer = 0 To Me.lstCombosDispos.Items.Count - 1
					If VpCard = Me.lstCombosDispos.Items.Item(VpI) And Not Me.lstCombosDispos.SelectedIndices.Contains(VpI) Then
						Me.lstCombosDispos.SelectedIndices.Add(VpI)
						Exit For
					End If
				Next VpI
			Next VpCard
		End If
	End Sub
	Sub BtSaveClick(sender As Object, e As EventArgs)
	'----------------------------------------------
	'Sauvegarde les s�quences dans un fichier texte
	'----------------------------------------------
	Dim VpSeqFile As StreamWriter
		Me.dlgSave.FileName = ""
		Me.dlgSave.ShowDialog
		If Me.dlgSave.FileName <> "" Then
			VpSeqFile = New StreamWriter(Me.dlgSave.FileName)
			For VpI As Integer = 0 To Me.chklstSequencesDispos.Items.Count - 1
				VpSeqFile.WriteLine(CType(Me.chklstSequencesDispos.Items.Item(VpI), clsComboSequence).ToString + "#" + Me.chklstSequencesDispos.GetItemChecked(VpI).ToString)
			Next VpI
			VpSeqFile.Close
		End If		
	End Sub
	Sub BtOpenClick(sender As Object, e As EventArgs)
	'----------------------------------------------
	'Recharge les s�quences depuis un fichier texte
	'----------------------------------------------
	Dim VpSeqFile As StreamReader
	Dim VpCur(0 To 1) As String
		Me.dlgOpen.FileName = ""
		Me.dlgOpen.ShowDialog
		If Me.dlgOpen.FileName <> "" Then
			VpSeqFile = New StreamReader(Me.dlgOpen.FileName)
			Call Me.Clear(True)
			While Not VpSeqFile.EndOfStream
				VpCur = VpSeqFile.ReadLine.Split("#")
				Me.chklstSequencesDispos.Items.Add(clsComboSequence.FromString(VpCur(0)), clsModule.Matching(VpCur(1)))			
			End While
			VpSeqFile.Close
		End If
	End Sub
	Sub LstCombosDisposSelectedIndexChanged(sender As Object, e As EventArgs)
	Dim VpPrev As List(Of Integer) = Me.lstCombosDispos.Tag
		'El�ment nouvellement s�lectionn� ?
		For Each VpItem As Integer In Me.lstCombosDispos.SelectedIndices
			If Not VpPrev.Contains(VpItem) Then
				VpPrev.Add(VpItem)
				Call clsModule.LoadScanCard(Me.lstCombosDispos.Items.Item(VpItem), Me.picScanCard2)
				Exit For
			End If
		Next VpItem
		'El�ment d�selectionn� ?
		For Each VpItem As Integer In VpPrev
			If Not Me.lstCombosDispos.SelectedIndices.Contains(VpItem) Then
				Call clsModule.LoadScanCard(Me.lstCombosDispos.Items.Item(VpItem), Me.picScanCard2)
				VpPrev.Remove(VpItem)
				Exit For
			End If
		Next VpItem		
	End Sub
	Sub BtClearClick(sender As Object, e As EventArgs)
		Call Me.Clear
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
		If Me.lstCombosDispos.SelectedItems.Count > 0 Then
			For Each VpItem As String In Me.lstCombosDispos.SelectedItems
				VpSequence.Add(VpItem)
			Next VpItem
			Me.chklstSequencesDispos.Items.Add(VpSequence, True)
			Call Me.Clear
		Else
			Call clsModule.ShowWarning("Une s�quence doit contenir au moins une carte.")
		End If						
	End Sub	
	Sub BtMainsActivate(ByVal sender As Object, ByVal e As EventArgs)
		Call Me.GestVisible(True)
	End Sub
	Sub BtCombosActivate(ByVal sender As Object, ByVal e As EventArgs)
		Call Me.GestVisible(, True)
		Call Me.LoadForCombos
	End Sub
	Sub BtDeployActivate(ByVal sender As Object, ByVal e As EventArgs)
		Call Me.GestVisible(, , True)
		Call Me.LoadSpecialUses
	End Sub
	Sub BtSuggestActivate(sender As Object, e As EventArgs)
		Call Me.GestVisible(, , , True)
	End Sub
	Sub BtSimusClick(sender As Object, e As EventArgs)
		Me.cboTourCumul.Text = ""
		Me.txtEspCumul.Text = ""
		If Me.chklstSequencesDispos.CheckedItems.Count > 0 Then
			Call Me.CombosSimu
		Else
			Call clsModule.ShowWarning("Cr�er au moins une s�quence avant de lancer le calcul.")
		End If		
	End Sub	
	Sub CboTourCumulSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
	Dim VpEspCumul As SortedList = Me.cboTourCumul.Tag
		Me.txtEspCumul.Text = Format(VpEspCumul.Item(CInt(Me.cboTourCumul.Text) - 1), "0.00")
	End Sub
	Sub CmdMainClick(ByVal sender As Object, ByVal e As EventArgs)
		Call Me.MainsSimu
	End Sub
	Sub CellMouseClick(sender As Object, e As SourceGrid2.PositionEventArgs)
		Call clsModule.LoadScanCard(clsModule.ExtractENName(e.Cell.GetValue(e.Position)), Me.picScanCard)
	End Sub
	Sub CellMouseDoubleClick(sender As Object, e As SourceGrid2.PositionEventArgs)
	Dim VpPartie As clsPartie = Me.grdMainsTirage.Tag
	Dim VpCell As Cells.Cell = e.Cell
	Dim VpCard As clsCard = VpCell.Tag
		Call VpPartie.AddToInPlay(VpCard)
		Call VpPartie.Draw
		Call Me.ShowMain
	End Sub
	Sub CellKeyUp(sender As Object, e As SourceGrid2.PositionKeyEventArgs)
		Call clsModule.LoadScanCard(clsModule.ExtractENName(e.Cell.GetValue(e.Position)), Me.picScanCard)
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
		Call Me.MainsSimu
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
		If ShowQuestion("L'arborescence de la fen�tre principale va �tre remplac�e par le r�sultat des suggestions..." + vbCrlf + "Continuer ?") = DialogResult.Yes Then
			Call Me.FullCorrelation(Me.Expressions)
		End If
	End Sub
	#End Region
	#Region "Propri�t�s"
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
Public Class clsPartie
	Private VmVerbose As Boolean					'Verbosit�
	Private VmSimuOut As StreamWriter				'Sortie de verbosit�
	Private VmDeck As New List(Of clsCard)			'Biblioth�que
	Private VmDeckCopy As New List(Of clsCard)		'Copie de la biblioth�que (restaur�e � chaque nouvelle partie)
	Private VmDrawn As New List(Of clsCard)			'Cartes pioch�es / en main
	Private VmInPlay As New List(Of clsCard)		'Cartes en jeu (permanents)
	Private VmInRound As New List(Of clsCard)		'Cartes en jeu pour le tour courant (�ph�m�res)
	Private VmReserve As clsManas					'R�serve de manas pour le tour courant
	Private VmLives As Integer = clsModule.CgNLives	'Nombre de points de vie
	Public Sub New(VpSource As String, VpRestriction As String, Optional VpGestDeploy As Boolean = False, Optional VpVerbose As Boolean = False, Optional VpSimuOut As StreamWriter = Nothing)
	'-------------------
	'Construction du jeu
	'-------------------
	Dim VpSQL As String
		If Not VpGestDeploy Then
			VpSQL = "Select Card.Title, " + VpSource + ".Items, CardFR.TitleFR From (Card Inner Join " + VpSource + " On " + VpSource + ".EncNbr = Card.EncNbr) Inner Join CardFR On Card.EncNbr = CardFR.EncNbr Where "
		Else
			VpSQL = "Select Card.Title, Card.CardText, Card.Type, Spell.Cost, " + VpSource + ".Items, CardFR.TitleFR From ((Card Inner Join " + VpSource + " On " + VpSource + ".EncNbr = Card.EncNbr) Inner Join Spell On Card.Title = Spell.Title) Inner Join CardFR On Card.EncNbr = CardFR.EncNbr Where "
		End If
		VpSQL = VpSQL + VpRestriction
		VpSQL = clsModule.TrimQuery(VpSQL)
		VgDBCommand.CommandText = VpSQL
		VgDBReader = VgDBCommand.ExecuteReader
		With VgDBReader
			While .Read
				If Not VpGestDeploy Then
					Me.AddCard(.GetString(0), .GetString(2), .GetInt32(1))
				Else
					Me.AddCard(.GetString(0), .GetString(5), .GetInt32(4), .GetValue(1).ToString.Replace(vbCrLf, " "), .GetValue(3).ToString, .GetValue(2).ToString, True)
				End If
			End While
			.Close
		End With
		VmVerbose = VpVerbose
		VmSimuOut = VpSimuOut
	End Sub
	Private Sub AddCard(VpName As String, VpNameFR As String, VpCount As Integer, Optional VpCardText As String = "", Optional VpCost As String = "", Optional VpType As String = "", Optional VpGestDeploy As Boolean = False)
	'--------------------
	'Construction du deck
	'--------------------
		For VpI As Integer = 1 To VpCount
			VmDeckCopy.Add(New clsCard(VpName, VpNameFR, VpCardText, VpCost, VpType, VpGestDeploy))
		Next VpI
	End Sub
	Public Sub DeckShuffle
	'--------------
	'M�lange le jeu
	'--------------
	Dim VpI As Integer
	Dim VpRandomPos As New SortedList(Me.CardsCount)
		VmDrawn.Clear
		VmInPlay.Clear
		VmDeck = New List(Of clsCard)(VmDeckCopy)
		'G�n�re un tableau tri� de nombres al�atoires
		For VpI = 0 To Me.CardsCount - 1
			VpRandomPos.Add(clsModule.VgRandom.NextDouble, VpI)
		Next VpI
		'R�ordonne les cartes en cons�quence
		VpI = 0
		For Each VpPos As Integer In VpRandomPos.Values
			VmDeck.Item(VpI) = VmDeckCopy.Item(VpPos)
			VpI = VpI + 1
		Next VpPos
	End Sub
	Public Sub UntapAll
	'---------------------------
	'D�sengage toutes les cartes
	'---------------------------
		For Each VpCard As clsCard In VmDeckCopy		'Le faire sur VmDeckCopy permet de n'oublier aucune carte
			If Not (VpCard.IsSpecial AndAlso VpCard.Speciality.DoesntUntap) Then
				VpCard.Tapped = False
			End If
		Next VpCard
	End Sub
	Public Sub UntagAll
	'--------------------------
	'D�marque toutes les cartes
	'--------------------------
		For Each VpCard As clsCard In VmDeckCopy		'Le faire sur VmDeckCopy permet de n'oublier aucune carte
			VpCard.Tagged = False
		Next VpCard
	End Sub
	Public Sub Draw(Optional VpTirages As Integer = 1)
	'----------------
	'Pioche une carte
	'----------------
		If VmDeck.Count > 0 Then
			For VpI As Integer = 1 To VpTirages
				VmDrawn.Add(VmDeck.Item(0))
				VmDeck.RemoveAt(0)
				If VmDeck.Count = 0 Then Exit Sub
			Next VpI
		End If
	End Sub
	Public Function IsSequencePresent(VpSequence As clsComboSequence) As Boolean
	'-------------------------------------------------------------
	'Renvoie vrai si toutes les cartes sp�cifi�es ont �t� pioch�es
	'-------------------------------------------------------------
		For Each VpItem As String In VpSequence.CardsNames
			If Not Me.IsDrawn(VpItem) Then
				Return False
			End If
		Next VpItem
		Return True
	End Function
	Public Function IsInFullDeck(VpCardName As String) As Boolean
	'-------------------------------------------------------------------------------
	'Renvoie vrai si le deck contient la carte dont le nom est sp�cifi� en param�tre
	'-------------------------------------------------------------------------------
		Return Me.IsInList(VpCardName, VmDeckCopy)
	End Function
	Public Function IsInPlay(VpCardName As String) As Boolean
	'--------------------------------------------------------------------------------------------
	'Renvoie vrai si le champ de bataille contient la carte dont le nom est sp�cifi� en param�tre
	'--------------------------------------------------------------------------------------------
		Return Me.IsInList(VpCardName, VmInPlay)
	End Function
	Private Function IsInList(VpCardName As String, VpList As List(Of clsCard)) As Boolean
		For Each VpCard As clsCard In VpList
			If VpCard.CardName = VpCardName Then
				Return True
			End If
		Next VpCard
		Return False
	End Function
	Public Sub PrepNewPlayRound
	'------------------------------------
	'Pr�paration d'un nouveau tour de jeu
	'------------------------------------
		VmInRound.Clear
		VmReserve = New clsManas
		Call Me.FollowRound
	End Sub
	Public Sub FollowRound
	'--------------------------------------------------------------------------------------------------------------
	'G�n�ration �ventuelle de mana sur des cartes arriv�es post�rieurement par rapport � la pose du dernier terrain
	'--------------------------------------------------------------------------------------------------------------
		For Each VpCard As clsCard In Me.CardsInPlay
			If VpCard.ManaAble And Not VpCard.Tapped Then
				Call VmReserve.AddSubManas(VpCard.ManasGen)
				Call clsModule.VerboseSimu(VmVerbose, VpCard.ManasGen.Potentiel.ToString + " manas ajout�s � la r�serve (" + VpCard.CardName + ")", VmSimuOut)
				VpCard.Tapped = True
			End If
		Next VpCard
	End Sub
	Public Sub AddToInPlay(VpCard As clsCard)
	'-----------------------------------
	'Pose en jeu un permanent de la main
	'-----------------------------------
		If Not VpCard Is Nothing Then
			VmInPlay.Add(VpCard)
			VmDrawn.Remove(VpCard)
		End If
	End Sub
	Public Sub AddToInRound(VpCard As clsCard)
	'----------------------------------
	'Pose en jeu un �ph�m�re de la main
	'----------------------------------
		VmInRound.Add(VpCard)
		VmDrawn.Remove(VpCard)
	End Sub
	Public Sub CommitChange(VpSrc As List(Of clsCard), VpDest As List(Of clsCard), Optional VpRemove As Boolean = False, Optional VpRemoveDrawn As Boolean = True)
	'--------------------------------------------------------------------------------------------------
	'Synth�se des deux routines pr�c�dentes utile pour ne pas perturber lors de l'�num�ration des items
	'--------------------------------------------------------------------------------------------------
		For Each VpCard As clsCard In VpSrc
			If VpRemove Then
				VpDest.Remove(VpCard)
			Else
				VpDest.Add(VpCard)
			End If
			If VpRemoveDrawn Then
				VmDrawn.Remove(VpCard)
			End If
		Next VpCard
	End Sub
	Public Function DoSpecialEffects(VpSrc As List(Of clsCard)) As Boolean
	'------------------------------------------------------------------------------------------------------------------------------------------------------------
	'Si des cartes poss�dent des propri�t�s particuli�res sp�cifi�es par l'utilisateur permettant de g�n�rer directement ou indirectement du mana, l'effectue ici
	'(renvoie vrai si un effet sp�cial a effectivement �t� utilis�)
	'------------------------------------------------------------------------------------------------------------------------------------------------------------
	Dim VpSomething As Boolean = False			'Passe � vrai si au moins un effet a �t� utilis�
	Dim VpNext As Boolean = False				'Passe � vrai si l'effet courant ne peut pas �tre utilis�
	Dim VpAbort As Boolean = False				'Passe � vrai si l'effet a �t� avort� (cible ill�gale etc...) auquel cas il faut annuler l'effort
	Dim VpManasInvoc As clsManas = Nothing		'Manas n�cessaires pour activer l'effet sp�cial
	Dim VpMyTarget As clsCard = Nothing			'Carte support
	Dim VpInt As Integer						'Entier support
	Dim VpStrs() As String						'Cha�ne support
	Dim VpTmpInPlay1 As New List(Of clsCard)	'Liste support ajout
	Dim VpTmpInPlay2 As New List(Of clsCard)	'Liste support suppression
	Dim VpTmpInPlay3 As New List(Of clsCard)	'Liste support d�fausse
	Dim VpTmpInPlay4 As New List(Of clsCard)	'Liste support pioche
		For Each VpCard As clsCard In VpSrc
			If VpCard.IsSpecial Then
				VpNext = False
				VpAbort = False
				'Commence par regarder si l'effort � fournir est r�alisable, en termes de :
				Select Case VpCard.Speciality.EffortID
					'- engagement de la carte + manas n�cessaires
					Case 0
						VpManasInvoc = New clsManas(VpCard.Speciality.Effort)
						If VpCard.Tapped Or Not Me.Reserve.ContainsEnoughFor(VpManasInvoc) Then
							VpNext = True
							Exit Select
						Else
							Call Me.Reserve.AddSubManas(VpManasInvoc, -1)
							VpCard.Tapped = True
						End If
					'- manas n�cessaires ou manas n�cessaires + sacrifice de cette carte
					Case 1, 13
						VpManasInvoc = New clsManas(VpCard.Speciality.Effort)
						If Not Me.Reserve.ContainsEnoughFor(VpManasInvoc) Then
							VpNext = True
							Exit Select
						Else
							Call Me.Reserve.AddSubManas(VpManasInvoc, -1)
							If VpCard.Speciality.EffortID = 13 Then
								VpTmpInPlay2.Add(VpCard)
							End If
						End If
					'- engagement de la carte ou engagement de la carte + points de vie � perdre ou engagement de la carte + engagement de la carte sp�cifi�e
					Case 2, 22, 3
						If VpCard.Tapped Then
							VpNext = True
							Exit Select
						Else
							'Perte de point de vie
							If VpCard.Speciality.EffortID = 22 Then
								Call clsModule.VerboseSimu(VmVerbose, "Joueur a " + VmLives.ToString + " point(s) de vie", VmSimuOut)
								VpInt = CInt(VpCard.Speciality.Effort)
								If VmLives <= VpInt Then
									VpNext = True
									Exit Select
								Else
									VmLives = VmLives - VpInt
									VpCard.Tapped = True
								End If
							'Engagement suppl�mentaire de la carte sp�cifi�e ou d'une des cartes sp�cifi�es
							ElseIf VpCard.Speciality.EffortID = 3 Then
								VpStrs = VpCard.Speciality.Effort.Split(";")
								For Each VpTarget As clsCard In Me.CardsInPlay
									For VpWanted As Integer = 0 To VpStrs.Length - 1
										If VpTarget.CardName = VpStrs(VpWanted) And Not VpTarget.Tapped Then
											VpMyTarget = VpTarget
											Exit For
										End If
									Next VpWanted
									If Not VpMyTarget Is Nothing Then
										VpMyTarget.Tapped = True
										VpCard.Tapped = True
									Else
										VpNext = True
										Exit Select
									End If
								Next VpTarget
							Else
								VpCard.Tapped = True
							End If
						End If
					'- sacrifice de cette carte (aucun pr�requis)
					Case 10
						VpTmpInPlay2.Add(VpCard)
					'- engagement de la carte + cartes � sacrifier ou cartes � sacrifier ou engagement de la carte + carte � sacrifier ou carte � sacrifier
					Case 11, 12, 14, 15, 16
						If VpCard.Speciality.EffortID = 12 Or VpCard.Speciality.EffortID = 15 Or VpCard.Speciality.EffortID = 16 Then
							If VpCard.Tapped Then
								VpNext = True
								Exit Select
							Else
								VpCard.Tapped = True
							End If
						End If
						'A part dans le cas 16, on n'est pas s�r de pouvoir assurer les sacrifices
						If VpCard.Speciality.EffortID <> 16 Then
							VpInt = 0
							VpStrs = VpCard.Speciality.Effort.Split(";")
							For Each VpTarget As clsCard In Me.CardsInPlay
								For VpSacrifice As Integer = 0 To VpStrs.Length - 1
									If VpTarget.CardName = VpStrs(VpSacrifice) And Not VpTmpInPlay2.Contains(VpTarget) Then
										VpTmpInPlay2.Add(VpTarget)
										VpInt = VpInt + 1
										VpStrs(VpSacrifice) = ""	'Evite de repasser plusieurs fois sur la m�me carte (ou plut�t une carte du m�me nom)
										Exit For
									End If
								Next VpSacrifice
								'Dans les cas 14 et 15, il n'y a qu'une carte � sacrifier parmi celles de la liste
								If VpInt = 1 And ( VpCard.Speciality.EffortID = 14 Or VpCard.Speciality.EffortID = 15 ) Then
									Exit For
								End If
							Next VpTarget
							'Si on n'a pas pu sacrifier toutes les cartes requises, on sort apr�s avoir annul� les sacrifices d�j� faits ainsi que l'engagement de la carte
							If VpInt < If(VpCard.Speciality.EffortID = 14 Or VpCard.Speciality.EffortID = 15, 1, VpStrs.Length) Then
								VpTmpInPlay2.RemoveRange(VpTmpInPlay2.Count - VpInt, VpInt)
								VpCard.Tapped = False
								VpNext = True
								Exit Select
							End If
						End If
					'- cartes � d�fausser
					Case 40
						VpInt = CInt(VpCard.Speciality.Effort)
						If Me.CardsDrawn.Count < (VpInt + VpTmpInPlay3.Count) Then		'attention il faut prendre en compte les cartes d�fauss�es pas encore committ�es
							VpNext = True
							Exit Select
						Else
							'On d�fausse les derni�res cartes (� priori les moins favoris�es par le dernier classement)
							VpTmpInPlay3.AddRange(Me.CardsDrawn.GetRange(Me.CardsDrawn.Count - VpInt - VpTmpInPlay3.Count, VpInt))
						End If
					'- points de vie � perdre
					Case 21
						Call clsModule.VerboseSimu(VmVerbose, "Joueur a : " + VmLives.ToString + " point(s) de vie", VmSimuOut)
						VpInt = CInt(VpCard.Speciality.Effort)
						If VmLives <= VpInt Then
							VpNext = True
							Exit Select
						Else
							VmLives = VmLives - VpInt
						End If
					'- pr�sence en jeu
					Case 30
						If Not Me.IsInPlay(VpCard.Speciality.Effort) Then
							VpNext = True
							Exit Select
						End If
					'- pr�sence dans le cimeti�re
					Case 31

					'- quantit� de pr�sence dans le cimeti�re
					Case 32

					Case Else
				End Select
				'Si l'effort a �t� fourni
				If Not VpNext Then
					'Obtient la r�compense, parmi :
					Select Case VpCard.Speciality.EffetID
						'- g�n�ration de manas
						Case 100
							Call Me.Reserve.AddSubManas(New clsManas(VpCard.Speciality.Effet))
							VpSomething = True
							Call clsModule.VerboseSimu(VmVerbose, "Effet utilis� : " + VpCard.CardName, VmSimuOut)
						'- d�gagement d'un artefact
						Case 110
							'Liste des artefacts potentiellement vis�s
							VpStrs = VpCard.Speciality.Effet.Split(";")
							VpNext = False
							For Each VpArtefact As String In VpStrs
								If VpNext Then Exit For
								For Each VpTarget As clsCard In Me.CardsInPlay
									If (Not VpCard Is VpTarget) And VpTarget.CardName = VpArtefact And VpTarget.Tapped = True Then
										VpTarget.Tapped = False
										VpNext = True
										Call clsModule.VerboseSimu(VmVerbose, "Effet utilis� : " + VpCard.CardName + " pour d�gager " + VpTarget.CardName, VmSimuOut)
										VpSomething = True
										Exit For
									End If
								Next VpTarget
							Next VpArtefact
							VpAbort = Not VpNext
						'- mise en jeu de l'artefact ou du terrain sp�cifi�
						Case 120
							'Liste des cartes potentiellement vis�es
							VpStrs = VpCard.Speciality.Effet.Split(";")
							VpNext = False
							For Each VpWanted As String In VpStrs
								If VpNext Then Exit For
								For Each VpTarget As clsCard In Me.CardsInDeck
									If VpTarget.CardName = VpWanted And Not VpTmpInPlay1.Contains(VpTarget) Then
										VpTmpInPlay1.Add(VpTarget)
										Me.CardsInDeck.Remove(VpTarget)		'Normalement on n'a pas le droit de toucher � la liste que l'on est en train d'�num�rer (ou celle qui la contient) mais l� on quitte la boucle imm�diatement apr�s...
										VpNext = True
										Call clsModule.VerboseSimu(VmVerbose, "Effet utilis� : " + VpCard.CardName + " pour mettre en jeu " + VpTarget.CardName, VmSimuOut)
										VpSomething = True
										Exit For
									End If
								Next VpTarget
							Next VpWanted
							VpAbort = Not VpNext
						'- pioche de cartes
						Case 130
							Call Me.Draw(CInt(VpCard.Speciality.Effet))
							Call clsModule.VerboseSimu(VmVerbose, "Effet utilis� : " + VpCard.CardName, VmSimuOut)
							VpSomething = True
						'- piocher la carte sp�cifi�e
						Case 131
							'Liste des cartes potentiellement vis�es
							VpStrs = VpCard.Speciality.Effet.Split(";")
							VpNext = False
							For Each VpWanted As String In VpStrs
								If VpNext Then Exit For
								For Each VpTarget As clsCard In Me.CardsInDeck
									If VpTarget.CardName = VpWanted And Not VpTmpInPlay4.Contains(VpTarget) Then
										VpTmpInPlay4.Add(VpTarget)
										Me.CardsInDeck.Remove(VpTarget)		'Normalement on n'a pas le droit de toucher � la liste que l'on est en train d'�num�rer (ou celle qui la contient) mais l� on quitte la boucle imm�diatement apr�s...
										VpNext = True
										Call clsModule.VerboseSimu(VmVerbose, "Effet utilis� : " + VpCard.CardName + " pour piocher " + VpTarget.CardName, VmSimuOut)
										VpSomething = True
										Exit For
									End If
								Next VpTarget
							Next VpWanted
							VpAbort = Not VpNext
						Case Else
					End Select
				End If
				'S'il faut annuler l'effort
				If VpAbort Then
					Select Case VpCard.Speciality.EffortID
						'- engagement de la carte + manas n�cessaires
						Case 0
							VpCard.Tapped = False
							Call Me.Reserve.AddSubManas(VpManasInvoc)
						'- manas n�cessaires
						Case 1
							Call Me.Reserve.AddSubManas(VpManasInvoc)
						'- engagement de la carte
						Case 2
							VpCard.Tapped = False
						'- engagement de la carte + engagement de la carte sp�cifi�e
						Case 3
							VpCard.Tapped = False
							VpMyTarget.Tapped = False
						'- sacrifice de cette carte
						Case 10
							VpTmpInPlay2.RemoveAt(VpTmpInPlay2.Count - 1)
						'- cartes � sacrifier
						Case 11, 14
							VpTmpInPlay2.RemoveRange(VpTmpInPlay2.Count - VpInt, VpInt)
						'- engagement de la carte + cartes � sacrifier
						Case 12, 15
							VpCard.Tapped = False
							VpTmpInPlay2.RemoveRange(VpTmpInPlay2.Count - VpInt, VpInt)
						'- manas n�cessaires + sacrifice de cette carte
						Case 13
							Call Me.Reserve.AddSubManas(VpManasInvoc)
							VpTmpInPlay2.RemoveAt(VpTmpInPlay2.Count - 1)
						'- points de vie � perdre
						Case 21
							VmLives = VmLives + CInt(VpCard.Speciality.Effort)
						'engagement de la carte + points de vie � perdre
						Case 22
							VmLives = VmLives + CInt(VpCard.Speciality.Effort)
							VpCard.Tapped = False
						'- cartes � d�fausser
						Case 40
							VpTmpInPlay3.RemoveRange(VpTmpInPlay3.Count - VpInt, VpInt)
						Case Else
					End Select
				End If
			End If
		Next VpCard
		'Si on a mis en jeu ou retirer des cartes par un effet, c'est maintenant qu'il faut le commiter
		Call Me.CommitChange(VpTmpInPlay1, Me.CardsInPlay, False, False)
		Call Me.CommitChange(VpTmpInPlay2, Me.CardsInPlay, True, False)
		Call Me.CommitChange(VpTmpInPlay3, Me.CardsDrawn, True, False)
		Call Me.CommitChange(VpTmpInPlay4, Me.CardsDrawn, False, False)
		'Les cartes nouvellement arriv�es peuvent �tre susceptibles de produire du mana imm�diatement
		Call Me.FollowRound
		Return VpSomething
	End Function
	Public Function PickLand As clsCard
	'-----------------------------------------------------------------------------------------------------------------------
	'D�termine la couleur dominante de la main en cours pour retourner le terrain (s'il y en a un) le plus appropri� � jouer
	'-----------------------------------------------------------------------------------------------------------------------
	Dim VpMaxPot As Short = 0
	Dim VpPot As Short
	Dim VpPicked As clsCard = Nothing
		'Parcourt les cartes de la main
		For Each VpCard As clsCard In VmDrawn
			'Si on a un terrain sp�cial, le s�lectionne d'office
			If VpCard.IsALand AndAlso VpCard.IsSpecial Then
				If VpCard.Speciality.InvocTapped Then
					VpCard.Tapped = True
					Call clsModule.VerboseSimu(VmVerbose, "Terrain sp�cial pos� (engag�) : " + VpCard.CardName, VmSimuOut)
				Else
					Call clsModule.VerboseSimu(VmVerbose, "Terrain sp�cial pos� : " + VpCard.CardName, VmSimuOut)
				End If
				Return VpCard
			'Si on a un terrain producteur de mana
			ElseIf VpCard.IsALand AndAlso VpCard.ManaAble Then
				'D�termine le potentiel sous-jacent (direct ou indirect) � sa mise en jeu
				VpPot = Me.GetPotFor(VpCard)
				'Retient le meilleur choix
				If VpPot > VpMaxPot Then
					VpMaxPot = VpPot
					VpPicked = VpCard
				ElseIf VpPicked Is Nothing Then
					VpPicked = VpCard
				End If
			End If
		Next VpCard
		If Not VpPicked Is Nothing Then
			Call clsModule.VerboseSimu(VmVerbose, "Terrain pos� : " + VpPicked.CardName, VmSimuOut)
		End If
		Return VpPicked
	End Function
	Private Function GetPotFor(VpLand As clsCard) As Short
	'--------------------------------------------------------------------------------------------------
	'Retourne le potentiel g�n�rable par les cartes en main qui n�cessite le terrain pass� en param�tre
	'--------------------------------------------------------------------------------------------------
	Dim VpPot As Short = 0
		For Each VpCard As clsCard In VmDrawn
			If ( Not ( VpCard Is VpLand ) ) AndAlso VpCard.ManaAble AndAlso VpCard.Requires(VpLand) Then
				VpPot = VpPot + VpCard.ManasPot
			End If
		Next VpCard
		Return VpPot
	End Function
	Private Function IsDrawn(VpName As String) As Boolean
	'-------------------------------------------------
	'Renvoie vrai si la cartes sp�cifi�e a �t� pioch�e
	'-------------------------------------------------
		For Each VpCard As clsCard In VmDrawn
			If VpCard.CardName = VpName And Not VpCard.Tagged Then
				VpCard.Tagged = True
				Return True
			End If
		Next VpCard
		Return False
	End Function
	Public Function ManasPotentielIn(VpList As List(Of clsCard)) As Integer
	'-----------------------------------------------------------------------------
	'Retourne le nombre de manas potentiellement g�n�rables avec les cartes en jeu
	'-----------------------------------------------------------------------------
	Dim VpPot As Integer = 0
		For Each VpCard As clsCard In VpList
			If (Not VpCard.Tapped) AndAlso VpCard.ManaAble Then
				VpPot = VpPot + VpCard.ManasPot
			End If
		Next VpCard
		Return VpPot
	End Function
	Public Function GetMissingCost(VpList As List(Of clsCard), VpPrev As Integer) As Integer
	'-----------------------------------------------------------------
	'Retourne le nombre de manas dont on manque pour le tour pr�c�dent
	'-----------------------------------------------------------------
	Dim VpSum As Integer = 0
	Dim VpCard As clsCard
	Dim VpJ As Integer
		VpJ = Math.Max(VpList.Count - clsModule.CgNMain, 0)
		For VpI As Integer = VpJ To VpList.Count - 1
			VpCard = VpList.Item(VpI)
			If Not ( VpCard.ManaAble Or VpCard.IsSpecial ) Then
				VpSum = VpSum + VpCard.ManasInvoc.Potentiel
			End If
		Next VpI
		Return Math.Max(VpSum - VpPrev, 0)
	End Function
	Public Function GetDistinctCards As List(Of clsCard)
	'-----------------------------------------------------
	'Retourne une liste des cartes distinctes dans le deck
	'-----------------------------------------------------
	Dim VpDistincts As New List(Of clsCard)
	Dim VpAlready As Boolean
		For Each VpCard As clsCard In Me.CardsInFullDeck
			VpAlready = False
			For Each VpIn As clsCard In VpDistincts
				If VpCard.CardName = VpIn.CardName Then
					VpAlready = True
					Exit For
				End If
			Next VpIn
			If Not VpAlready Then
				VpDistincts.Add(VpCard)
			End If
		Next VpCard
		Return VpDistincts
	End Function
	Public ReadOnly Property CardsCount As Integer
		Get
			Return VmDeckCopy.Count
		End Get
	End Property
	Public ReadOnly Property CardsDrawn As List(Of clsCard)
		Get
			Return VmDrawn
		End Get
	End Property
	Public ReadOnly Property CardsInDeck As List(Of clsCard)
		Get
			Return VmDeck
		End Get
	End Property
	Public ReadOnly Property CardsInFullDeck As List(Of clsCard)
		Get
			Return VmDeckCopy
		End Get
	End Property
	Public ReadOnly Property CardsInPlay As List(Of clsCard)
		Get
			Return VmInPlay
		End Get
	End Property
	Public ReadOnly Property CardsInRound As List(Of clsCard)
		Get
			Return VmInRound
		End Get
	End Property
	Public ReadOnly Property Reserve As clsManas
		Get
			Return VmReserve
		End Get
	End Property
End Class
Public Class clsCard
	Private VmCardName As String					'Nom de la carte (VO)
	Private VmCardNameFR As String					'Nom de la carte (VF)
	Private VmCardType As String					'Type de la carte (C,I,A,E,L,N,S,T,U,P)
	Private VmManasInvoc As clsManas				'Co�t d'invocation de la carte
	Private VmManasGen As clsManas = Nothing		'Manas g�n�rables par la carte
	Private VmTapped As Boolean = False				'Carte engag�e ?
	Private VmTagged As Boolean = False				'Carte marqu�e ? (utilisation interne)
	Private VmSpeciality As clsSpeciality = Nothing	'Carte destin�e � une utilisation sp�ciale (pour la simulation de d�ploiement uniquement) ?
	Private VmCardText As String
	Public Sub New(VpCardName As String, VpNameFR As String, Optional VpCardText As String = "", Optional VpCost As String = "", Optional VpType As String = "", Optional VpGestDeploy As Boolean = False)
	Dim VpGCost As String
		VmCardName = VpCardName
		VmCardNameFR = VpNameFR
		VmCardText = VpCardText
		If VpGestDeploy Then
			VmCardType = VpType
			VmManasInvoc = New clsManas(VpCost)
			'On ne s'occupe pas des arpenteurs
			If VpType = "P" Then Exit Sub
			VpCardText = VpCardText.ToLower.Replace(vbCrLf, " ").Replace(".", "")
			'Carte parsable
			If VpCardText.Contains(clsModule.CgManaParsing(0)) Then
				VpGCost = VpCardText.Substring(0, VpCardText.IndexOf(clsModule.CgManaParsing(0)) - 1)
				'Carte dont le mana g�n�rable d�pend d'autres param�tres non contr�lables (dans ce cas on devrait plut�t se trouver dans la situation de myspecialuses)
				If VpGCost.EndsWith(clsModule.CgManaParsing(1)) Then
					'On affecte un mana incolore par d�faut
					VpGCost = "A"
				Else
					VpGCost = VpGCost.Substring(VpGCost.LastIndexOf(clsModule.CgManaParsing(1)) + clsModule.CgManaParsing(1).Length + 1)
				End If
				VmManasGen = New clsManas(VpGCost)
			'Terrain sans texte explicite
			ElseIf VpCardText.StartsWith("[") Then
				VmManasGen = New clsManas(VpCardText.Replace("[", "").Replace("]", ""))
			ElseIf VpCardText.Length = 1 Or VpCardText.Length = 3 Then
				VmManasGen = New clsManas(VpCardText)
			End If
		End If
	End Sub
	Public Function Requires(VpCard As clsCard) As Boolean
	'-----------------------------------------------------------------------------------------------------------
	'Renvoie vrai si l'invocation de la carte courante requiert le mana produit par la carte pass�e en param�tre
	'-----------------------------------------------------------------------------------------------------------
		Return VmManasInvoc.IsBetterWith(VpCard.ManasGen)
	End Function
	Public ReadOnly Property CardName As String
		Get
			Return VmCardName
		End Get
	End Property
	Public ReadOnly Property CardNameFR As String
		Get
			Return VmCardNameFR
		End Get
	End Property
	Public ReadOnly Property CardType As String
		Get
			Return VmCardType
		End Get
	End Property
	Public ReadOnly Property ManaAble As Boolean
		Get
			Return Not ( VmManasGen Is Nothing )
		End Get
	End Property
	Public ReadOnly Property ManasPot As Integer
		Get
			Return VmManasGen.Potentiel
		End Get
	End Property
	Public ReadOnly Property ManasGen As clsManas
		Get
			Return VmManasGen
		End Get
	End Property
	Public ReadOnly Property ManasInvoc As clsManas
		Get
			Return VmManasInvoc
		End Get
	End Property
	Public ReadOnly Property IsALand As Boolean
		Get
			Return VmCardType = "L"
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
	Public Property Tagged As Boolean
		Get
			Return VmTagged
		End Get
		Set (VpTagged As Boolean)
			VmTagged = VpTagged
		End Set
	End Property
	Public Property Speciality As clsSpeciality
		Get
			Return VmSpeciality
		End Get
		Set (VpSpeciality As clsSpeciality)
			VmSpeciality = VpSpeciality
			VmManasGen = Nothing			'Si la carte a une sp�cialit�, on efface le parsing par d�faut qui avait eu lieu pr�c�demment
		End Set
	End Property
	Public ReadOnly Property IsSpecial As Boolean
		Get
			Return Not ( VmSpeciality Is Nothing )
		End Get
	End Property
	Public ReadOnly Property CardText As String
		Get
			Return VmCardText
		End Get
	End Property
End Class
Public Class clsEsperance
	Private VmEsp As New SortedList
	Private VmOccurences As Integer
	Public Sub New(VpOccurences As String)
		VmOccurences = CInt(VpOccurences)
	End Sub
	Public Sub AddForRound(VpI As Integer, Optional VpJ As Integer = 1)
		If VmEsp.ContainsKey(VpI) Then
			VmEsp.Item(VpI) = VmEsp.Item(VpI) + VpJ
		Else
			VmEsp.Add(VpI, VpJ)
		End If
	End Sub
	Public Function GetEsp(Optional VpPercent As Boolean = True) As SortedList
	Dim VpEsp As New SortedList(VmEsp.Count)
		For Each VpKey As Integer In VmEsp.Keys
			VpEsp.Add(VpKey, If(VpPercent, 100, 1) * VmEsp.Item(VpKey) / VmOccurences)
		Next VpKey
		Return VpEsp
	End Function
	Public Sub AddRoundsDispos(VpCbo As ComboBox)
		VpCbo.Items.Clear
		For Each VpI As Integer In VmEsp.Keys
			VpCbo.Items.Add(Format(VpI + 1, "00"))
		Next VpI
		VpCbo.Sorted = True
	End Sub
End Class
Public Class clsCorrelation
	Private VmCard1 As String
	Private VmCard2 As String
	Private VmSeq As String
	Public Sub New(VpC1 As String, VpC2 As String, VpS As String)
		VmCard1 = VpC1
		VmCard2 = VpC2
		VmSeq = VpS
	End Sub
	Public Shared Function LongestSequence(VpX() As String, VpY() As String) As String
	'---------------------------------------------------------------------------------------
	'Calcul par programmation dynamique de la plus longue sous-s�quence commune entre X et Y
	'---------------------------------------------------------------------------------------
	Dim VpC(,) As Integer
		ReDim VpC(0 To VpX.Length, 0 To VpY.Length)
		For VpI As Integer = 1 To VpX.Length - 1
			For VpJ As Integer = 1 To VpY.Length - 1
				If VpX(VpI) = VpY(VpJ) Then
					VpC(VpI, VpJ) = VpC(VpI - 1, VpJ - 1) + 1
				Else
					VpC(VpI, VpJ) = Math.Max(VpC(VpI, VpJ - 1), VpC(VpI - 1, VpJ))
				End If
			Next VpJ
		Next VpI
		'Traceback
		Return ReadOutSequence(VpC, VpX, VpY, VpX.Length - 1, VpY.Length - 1).Trim
	End Function
	Private Shared Function ReadOutSequence(VpC(,) As Integer, VpX() As String, VpY() As String, VpI As Integer, VpJ As Integer) As String
		If VpI = 0 Or VpJ = 0 Then
			Return ""
		ElseIf VpX(VpI) = VpY(VpJ) Then
			Return ReadOutSequence(VpC, VpX, VpY, VpI - 1, VpJ - 1) + " " + VpX(VpI)
		Else
			If VpC(VpI, VpJ - 1) > VpC(VpI - 1, VpJ) Then
				Return ReadOutSequence(VpC, VpX, VpY, VpI, VpJ - 1)
			Else
				Return ReadOutSequence(VpC, VpX, VpY, VpI - 1, VpJ)
			End If
		End If
	End Function
	Public Shared Function GetMean(VpS As List(Of clsCorrelation)) As Single
	'----------------------------------------------------
	'Retourne la longueur moyenne des s�quences non vides
	'----------------------------------------------------
	Dim VpTot As Integer = 0
	Dim VpN As Integer = 0
		For Each VpCorr As clsCorrelation In VpS
			If VpCorr.Seq.Trim <> "" Then
				VpTot = VpTot + VpCorr.Seq.Split(" ").Length
				VpN = VpN + 1
			End If
		Next VpCorr
		Return VpTot / VpN
	End Function
	Public Shared Function MyContains(VpS As List(Of clsCorrelation), VpCard As String) As Boolean
	'--------------------------------------------------------------------------------------------------------------------------
	'Retourne vrai si la carte pass�e en param�tre est r�f�renc�e dans au moins un des �l�ments de la liste pass�e en param�tre
	'--------------------------------------------------------------------------------------------------------------------------
		For Each VpCorr As clsCorrelation In VpS
			If VpCorr.Card1 = VpCard Or VpCorr.Card2 = VpCard Then
				Return True
			End If
		Next VpCorr
		Return False
	End Function
	Public ReadOnly Property Card1 As String
		Get
			Return VmCard1
		End Get
	End Property
	Public ReadOnly Property Card2 As String
		Get
			Return VmCard2
		End Get
	End Property
	Public ReadOnly Property Seq As String
		Get
			Return VmSeq
		End Get
	End Property
End Class
Public Class clsCorrelationComparer
	Implements IComparer(Of clsCorrelation)
	Public Function Compare(ByVal x As clsCorrelation, ByVal y As clsCorrelation) As Integer Implements IComparer(Of clsCorrelation).Compare
		Return x.Seq.Length - y.Seq.Length
	End Function
End Class
Public Class clsComboSequence
	Private VmCardsNames As List(Of String)
	'Private VmCardsNamesFR As List(Of String)
	Public Sub New
		VmCardsNames = New List(Of String)
	'	VmCardsNamesFR = New List(Of String)
	End Sub
	Public Sub Add(VpItem As String)
	'Dim VpVO As String
	'Dim VpVF As String
		VmCardsNames.Add(VpItem)
	'	VmCardsNamesFR.Add(VpVF)
	End Sub
	Public Shared Function FromString(VpStr As String) As clsComboSequence
	Dim VpSequence As New clsComboSequence
		VpStr = VpStr.Substring(VpStr.IndexOf("(") + 1)
		VpStr = VpStr.Substring(0, VpStr.Length - 1)
		For Each VpItem As String In VpStr.Split(New String() {" et "}, StringSplitOptions.None)
			VpSequence.Add(VpItem)
		Next VpItem
		Return VpSequence
	End Function
	Public Overrides Function ToString As String
	Dim VpStr As String = "ou ("
		'For Each VpCard As String In If(VpVF, VmCardsNamesFR, VmCardsNames)
		For Each VpCard As String In VmCardsNames
			VpStr = VpStr + VpCard + " et "
		Next VpCard
		Return VpStr.Substring(0, VpStr.Length - 4) + ")"
	End Function
	Public ReadOnly Property CardsNames As List(Of String)
		Get
			Return VmCardsNames
		End Get
	End Property
End Class