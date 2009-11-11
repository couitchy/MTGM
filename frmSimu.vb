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
'| - espérance de déploiement			   01/08/2009 |
'| - levée de l'ambiguité sur les sources  03/10/2009 |
'| - déploiement - effets spéciaux		   08/11/2009 |
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
	#Region "Méthodes"
	Public Sub New(VpOwner As MainForm)
		Me.InitializeComponent()		
		VmSource = IIf(VpOwner.chkClassement.GetItemChecked(0), clsModule.CgSDecks, clsModule.CgSCollection)			
		VmRestrictionSQL = VpOwner.Restriction
		VmRestrictionTXT = VpOwner.Restriction(True)
		Me.Text = clsModule.CgSimus + VmRestrictionTXT
	End Sub
	Private Function GetNbItems(VpIndex As Integer) As Integer
	'------------------------------------------------------------
	'Retourne la quantité associée à l'élément passé en paramètre
	'------------------------------------------------------------
	Dim VpStr As String = Me.lstCombosListe.Items.Item(VpIndex)
		Return Val(VpStr.Substring(VpStr.IndexOf("[") + 1))
	End Function
	Private Function GetMaxItems(VpIndex As Integer, VpIncrement As Integer) As Integer
	'------------------------------------------------------------------------------
	'Retourne la quantité maximale possible associée à l'élément passé en paramètre
	'------------------------------------------------------------------------------
	Dim VpStr As String = Me.lstCombosListe.Items.Item(VpIndex)
		Return VpIncrement + Val(VpStr.Substring(VpStr.LastIndexOf("/") + 1))
	End Function
	Private Function myIndexOf(VpEntry As String) As Integer
	'--------------------------------------------------------------------------------------
	'Retourne l'index dans la liste de l'élément passé en paramètre en omettant sa quantité
	'--------------------------------------------------------------------------------------
	Dim VpI As Integer
		For VpI = 0 To Me.lstCombosListe.Items.Count - 1
			If Me.lstCombosListe.Items.Item(VpI).StartsWith(VpEntry) Then
				Return VpI
			End If
		Next VpI
		Return -1
	End Function
	Private Sub LoadSpecialUses
	'----------------------------------------------------------------------
	'Charge la liste des cartes possédant une utilisation spéciale associée
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
	Dim VpEntry As String	
	Dim VpIndex As Integer
		Me.lstCombosListe.Items.Clear
		VpSQL = "Select Card.Title, CardFR.TitleFR, " + VmSource + ".Items From ((Card Inner Join CardFR On Card.EncNbr = CardFR.EncNbr) Inner Join " + VmSource + " On " + VmSource + ".EncNbr = Card.EncNbr) Where "
		VpSQL = VpSQL + VmRestrictionSQL
		VpSQL = clsModule.TrimQuery(VpSQL)		
		VgDBCommand.CommandText = VpSQL
		VgDBReader = VgDBCommand.ExecuteReader	
		With VgDBReader			
			While .Read
				VpEntry = .GetString(1) + " (" + .GetString(0) + ")"
				'Si la liste contient déjà la carte, ne la met pas en doublon mais incrémente le nombre d'éléments correspondant
				VpIndex = Me.myIndexOf(VpEntry)			
				If VpIndex < 0 Then
					Me.lstCombosListe.Items.Add(VpEntry + " [0/" + .GetInt32(2).ToString + "]")
				Else
					Me.lstCombosListe.Items.Item(VpIndex) = VpEntry + " [0/" + Me.GetMaxItems(VpIndex, .GetInt32(2)).ToString + "]"
				End If
			End While
			.Close
		End With	
		Me.lstCombosListe.Sorted = True
	End Sub
	Private Sub CombosSimu
	'----------------------------------------------------------------------------------------------
	'Estime les probabilités simple et combinée d'apparition des cartes sélectionnées au nième tour
	'----------------------------------------------------------------------------------------------	
	Dim VpPartie As New clsPartie(VmSource, VmRestrictionSQL)
	Dim VpEspSimple As New clsEsperance(Me.txtN.Text)
	Dim VpEspCumul As New clsEsperance(Me.txtN.Text)
	Dim VpCombo() As String
	Dim VpComboL As New ArrayList
	Dim VpStr As String
		Me.prgSimu.Maximum = CInt(Me.txtN.Text)
		Me.prgSimu.Value = 0
		Me.prgSimu.Visible = True
		'Préparation du combo
		For VpI As Integer = 0 To Me.lstCombosListe.SelectedItems.Count - 1
			VpStr = Me.lstCombosListe.SelectedItems(VpI)
			VpStr = VpStr.Substring(VpStr.IndexOf("(") + 1)
			VpStr = VpStr.Substring(0, VpStr.IndexOf(")"))			
			For VpJ As Integer = 1 To Me.GetNbItems(Me.lstCombosListe.SelectedIndices(VpI))
				VpComboL.Add(VpStr)
			Next VpJ
		Next VpI
		VpCombo = VpComboL.ToArray(System.Type.GetType("System.String"))
		'Simulation des N parties
		For VpI As Integer = 1 To CInt(Me.txtN.Text)
			'Mélange le jeu
			Call VpPartie.DeckShuffle(VpI)
			'Gestion des n tours
			For VpJ As Integer = 0 To VpPartie.CardsCount - clsModule.CgNMain
				Call VpPartie.UntagAll
				'Au premier tour on pioche 7 cartes
				If VpJ = 0 Then
					Call VpPartie.Draw(clsModule.CgNMain)
				'Les suivants une seule
				Else
					Call VpPartie.Draw
				End If
				If VpPartie.IsOnePresent(VpCombo) Then
					VpEspSimple.AddForRound(VpJ)
				End If				
				If VpPartie.IsComboPresent(VpCombo) Then
					VpEspCumul.AddForRound(VpJ)
				End If
			Next VpJ 
			Me.prgSimu.Value = VpI
			Application.DoEvents
		Next VpI
		Me.prgSimu.Visible = False
		Me.cmdAddPlot.Enabled = True
		'Tours disponibles
		VpEspSimple.AddRoundsDispos(Me.cboTourSimple)
		VpEspCumul.AddRoundsDispos(Me.cboTourCumul)
		'Mémorisation espérances
		Me.cboTourSimple.Tag = VpEspSimple.GetEsp
		Me.cboTourCumul.Tag = VpEspCumul.GetEsp
		'Sélection par défaut
		If Me.cboTourSimple.Items.Count > 0 Then Me.cboTourSimple.SelectedIndex = 0
		If Me.cboTourCumul.Items.Count > 0 Then Me.cboTourCumul.SelectedIndex = 0
	End Sub
	Private Sub MainsSimu
	'---------------------------------------
	'Tire aléatoirement une main de 7 cartes
	'---------------------------------------
	Dim VpPartie As New clsPartie(VmSource, VmRestrictionSQL)
	Dim VpCell As Cells.Cell
	Dim VpCellBehavior As New BehaviorModels.CustomEvents
		If VpPartie.CardsCount < clsModule.CgNMain Then
			Call clsModule.ShowWarning("Il faut avoir au moins " + clsModule.CgNMain.ToString + " cartes saisies pour tirer une main...")
		Else
			'Mélange le jeu
			Call VpPartie.DeckShuffle(DateTime.Now.Millisecond)
			'Tire les 7 cartes
			Call VpPartie.Draw(clsModule.CgNMain)
			'Les inscrit dans la grille
			AddHandler VpCellBehavior.Click, AddressOf CellMouseClick
			AddHandler VpCellBehavior.KeyUp, AddressOf CellKeyUp
			With Me.grdMainsTirage
				'Nettoyage
				If .Rows.Count > 0 Then					
					.Rows.RemoveRange(0, .Rows.Count)
				End If			
				.ColumnsCount = 1
				.FixedRows = 0
				For Each VpCard As clsCard In VpPartie.CardsDrawn
					.Rows.Insert(.RowsCount)
					VpCell = New Cells.Cell(VpCard.CardName)
					VpCell.Behaviors.Add(VpCellBehavior)
					Me.grdMainsTirage(.RowsCount - 1, 0) = VpCell
				Next VpCard
				.Columns(0).Width = .Width
			End With
			Call Me.CellMouseClick(Nothing, New PositionEventArgs(New Position(0, 0), Me.grdMainsTirage(0, 0)))
		End If
	End Sub
	Private Sub DeploySimu	
	'---------------------------------------------------------------
	'Estime l'espérance du nombre de manas disponibles au nième tour
	'---------------------------------------------------------------
	Dim VpVerbose As Boolean = Me.chkVerbosity.Checked
	Dim VpPartie As New clsPartie(VmSource, VmRestrictionSQL, True, VpVerbose, VmSimuOut)	'Partie en simulation
	Dim VpEspDeploy As New clsEsperance(Me.txtN2.Text)										'Résultats
	Dim VpTmpInPlay As New ArrayList														'Support liste temporaire 1
	Dim VpTmpInRound As New ArrayList														'Support liste temporaire 2
	Dim VpSomething As Boolean																'Au moins une action spéciale exécutée
		If VpPartie.CardsCount < clsModule.CgNMain Then Exit Sub
		Me.prgSimu2.Maximum = CInt(Me.txtN2.Text)
		Me.prgSimu2.Value = 0
		Me.prgSimu2.Visible = True
		'Retire la spécification automatique pour les cartes spéciales	
		Call Me.ManualSpec(VpPartie, Me.lstUserCombos.Items, False)
		'Spécification manuelle des cartes spéciales cochées
		Call Me.ManualSpec(VpPartie, Me.lstUserCombos.CheckedItems, True)
		'Simulation des N parties
		For VpI As Integer = 1 To CInt(Me.txtN2.Text)
			'Mélange le jeu
			If Me.chkVerbosity.Checked Then
				Call VpPartie.DeckShuffle(DateTime.Now.Millisecond)
			Else
				Call VpPartie.DeckShuffle(VpI)
			End If
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
				'Joue un terrain s'il y en a un (de la couleur la plus astucieuse)
				Call VpPartie.AddToInPlay(VpPartie.PickLand)
				'Détermine la réserve totale de manas courante
				Call VpPartie.PrepNewPlayRound
				'Invoque les cartes génératrices de manas que l'on peut (en commençant par celles ayant le potentiel le plus élevé et/ou les cartes spéciales)
				'- les permanents restent en jeu
				'- les éphémères doivent partir à la fin du tour				
				VpPartie.CardsDrawn.Sort(New clsManasPotComparer(Me.lstUserCombos))
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
				Call VpEspDeploy.AddForRound(VpJ, VpPartie.Reserve.Potentiel + VpPartie.ManasPotentielIn(VpPartie.CardsInPlay) + VpPartie.ManasPotentielIn(VpPartie.CardsInRound))
				Call clsModule.VerboseSimu(VpVerbose, "Fin du tour, manas disponibles : " + (VpPartie.Reserve.Potentiel + VpPartie.ManasPotentielIn(VpPartie.CardsInPlay) + VpPartie.ManasPotentielIn(VpPartie.CardsInRound)).ToString, VmSimuOut)
			Next VpJ
			Me.prgSimu2.Value = VpI
			Application.DoEvents
		Next VpI
		Me.prgSimu2.Visible = False
		Me.cmdAddPlot2.Enabled = True
		'Tours disponibles
		Call VpEspDeploy.AddRoundsDispos(Me.cboTourDeploy)
		'Mémorisation espérances
		Me.cboTourDeploy.Tag = VpEspDeploy.GetEsp(False)
		'Sélection par défaut
		If Me.cboTourDeploy.Items.Count > 0 Then Me.cboTourDeploy.SelectedIndex = 0	
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
	Private Sub GestVisible(Optional VpMains As Boolean = False, Optional VpCombos As Boolean = False, Optional VpDeploy As Boolean = False)
	'------------------
	'Gestion des panels
	'------------------
		Me.grpMains.Visible = VpMains
		Me.grpCombos.Visible = VpCombos
		Me.grpDeploy.Visible = VpDeploy
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
	#End Region
	#Region "Evènements"
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
	Sub CmdSimuClick(ByVal sender As Object, ByVal e As EventArgs)
		Me.cboTourSimple.Text = ""
		Me.cboTourCumul.Text = ""
		Me.txtEspSimple.Text = ""
		Me.txtEspCumul.Text = ""
		If Me.lstCombosListe.SelectedItems.Count > 0 Then
			Call Me.CombosSimu	
		Else
			Call clsModule.ShowWarning("Sélectionner au moins une carte dans la liste.")
		End If
	End Sub
	Sub CboTourSimpleSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
	Dim VpEspSimple As Hashtable = Me.cboTourSimple.Tag
		Me.txtEspSimple.Text = Format(VpEspSimple.Item(CInt(Me.cboTourSimple.Text) - 1), "0.00")
	End Sub
	Sub CboTourCumulSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
	Dim VpEspCumul As Hashtable = Me.cboTourCumul.Tag
		Me.txtEspCumul.Text = Format(VpEspCumul.Item(CInt(Me.cboTourCumul.Text) - 1), "0.00")
	End Sub
	Sub CmdMainClick(ByVal sender As Object, ByVal e As EventArgs)
		Call Me.MainsSimu
	End Sub
	Sub CellMouseClick(sender As Object, e As SourceGrid2.PositionEventArgs)
		Call clsModule.LoadScanCard(e.Cell.GetValue(e.Position), Me.picScanCard)
	End Sub
	Sub CellKeyUp(sender As Object, e As SourceGrid2.PositionKeyEventArgs)
		Call clsModule.LoadScanCard(e.Cell.GetValue(e.Position), Me.picScanCard)
	End Sub
	Sub CmdSimu2Click(ByVal sender As Object, ByVal e As EventArgs)
		If Me.chkVerbosity.Checked Then
			Me.dlgVerbose.ShowDialog
			If Me.dlgVerbose.FileName <> "" Then
				VmSimuOut = New StreamWriter(Me.dlgVerbose.FileName)
			End If
		End If
		Call Me.DeploySimu		
	End Sub
	Sub CboTourDeploySelectedIndexChanged(sender As Object, e As EventArgs)
	Dim VpEspDeploy As Hashtable = Me.cboTourDeploy.Tag
		Me.txtEspManas.Text = Format(VpEspDeploy.Item(CInt(Me.cboTourDeploy.Text) - 1), "0.00")		
	End Sub	
	Sub CmdAddPlotClick(sender As Object, e As EventArgs)
	Dim VpEspSimple As Hashtable = Me.cboTourSimple.Tag
	Dim VpEspCumul As Hashtable = Me.cboTourCumul.Tag	
		VmGraphCount = VmGraphCount + 1
		VmGrapher.AddNewPlot(VpEspSimple, "(" + VmGraphCount.ToString + ") " + clsModule.CgSimus2 + VmRestrictionTXT)
		VmGrapher.AddNewPlot(VpEspCumul, "(" + VmGraphCount.ToString + ") " + clsModule.CgSimus3 + VmRestrictionTXT)
		VmGrapher.Show	
		VmGrapher.BringToFront
	End Sub	
	Sub CmdAddPlot2Click(sender As Object, e As EventArgs)
	Dim VpEspDeploy As Hashtable = Me.cboTourDeploy.Tag		
		VmGrapher.AddNewPlot(VpEspDeploy, clsModule.CgSimus4 + VmRestrictionTXT)
		VmGrapher.Show
		VmGrapher.BringToFront
	End Sub	
	Sub FrmSimuLoad(sender As Object, e As EventArgs)
		Call Me.MainsSimu		
	End Sub	
	Sub LstCombosListeMouseUp(sender As Object, e As MouseEventArgs)
	'------------------------------------------------------------------------------------------------------------------------------------
	'Gère les cas où l'utilisateur souhaite faire une estimation d'apparition sur un combo qui peut contenir plusieurs fois la même carte
	'------------------------------------------------------------------------------------------------------------------------------------
	Dim VpIndex As Integer
	Dim VpQuant As Integer
	Dim VpMax As Integer
		If Me.lstCombosListe.Items.Count > 0 Then
			VpIndex = Me.lstCombosListe.IndexFromPoint(e.Location)
			VpMax = Me.GetMaxItems(VpIndex, 0)
			VpQuant = Val(InputBox("Combien de " + Me.lstCombosListe.Items.Item(VpIndex) + " voulez-vous sélectionner ?", "Nombre d'éléments", "1"))
			VpQuant = Math.Min(VpMax, VpQuant)
			'Actualisation de la quantité
			Me.lstCombosListe.Items.Item(VpIndex) = Me.lstCombosListe.Items.Item(VpIndex).Replace("[" + Me.GetNbItems(VpIndex).ToString, "[" + VpQuant.ToString)
			'Si annulation, désélectionne la carte
			If VpQuant = 0 And Me.lstCombosListe.SelectedIndices.Contains(VpIndex) Then
				Me.lstCombosListe.SelectedIndices.Remove(VpIndex)
			'Sinon, force le maintien
			ElseIf VpQuant > 0 And Not Me.lstCombosListe.SelectedIndices.Contains(VpIndex) Then
				Me.lstCombosListe.SelectedIndices.Add(VpIndex)
			End If
		End If
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
	#End Region
End Class
Public Class clsPartie
	Private VmVerbose As Boolean					'Verbosité
	Private VmSimuOut As StreamWriter				'Sortie de verbosité
	Private VmDeck As New ArrayList					'Bibliothèque
	Private VmDeckCopy As New ArrayList				'Copie de la bibliothèque (restaurée à chaque nouvelle partie)
	Private VmDrawn As New ArrayList				'Cartes piochées / en main
	Private VmInPlay As New ArrayList				'Cartes en jeu (permanents)
	Private VmInRound As New ArrayList				'Cartes en jeu pour le tour courant (éphémères)
	Private VmReserve As clsManas					'Réserve de manas pour le tour courant
	Private VmLives As Integer = clsModule.CgNLives	'Nombre de points de vie
	Public Sub New(VpSource As String, VpRestriction As String, Optional VpGestDeploy As Boolean = False, Optional VpVerbose As Boolean = False, Optional VpSimuOut As StreamWriter = Nothing)
	'-------------------
	'Construction du jeu
	'-------------------
	Dim VpSQL As String	
		If Not VpGestDeploy Then
			VpSQL = "Select Card.Title, " + VpSource + ".Items From Card Inner Join " + VpSource + " On " + VpSource + ".EncNbr = Card.EncNbr Where "
		Else
			VpSQL = "Select Card.Title, Card.CardText, Card.Type, Spell.Cost, " + VpSource + ".Items From (Card Inner Join " + VpSource + " On " + VpSource + ".EncNbr = Card.EncNbr) Inner Join Spell On Card.Title = Spell.Title Where "
		End If
		VpSQL = VpSQL + VpRestriction
		VpSQL = clsModule.TrimQuery(VpSQL)		
		VgDBCommand.CommandText = VpSQL
		VgDBReader = VgDBCommand.ExecuteReader	
		With VgDBReader
			While .Read
				If Not VpGestDeploy Then
					Me.AddCard(.GetString(0), .GetInt32(1))
				Else
					Me.AddCard(.GetString(0), .GetInt32(4), .GetValue(1).ToString.Replace(vbCrLf, " "), .GetValue(3).ToString, .GetValue(2).ToString, True)
				End If
			End While
			.Close
		End With
		VmVerbose = VpVerbose
		VmSimuOut = VpSimuOut
	End Sub
	Private Sub AddCard(VpName As String, VpCount As Integer , Optional VpCardText As String = "", Optional VpCost As String = "", Optional VpType As String = "", Optional VpGestDeploy As Boolean = False)
	'--------------------
	'Construction du deck
	'--------------------
		For VpI As Integer = 1 To VpCount
			VmDeckCopy.Add(New clsCard(VpName, VpCardText, VpCost, VpType, VpGestDeploy))
		Next VpI
	End Sub
	Public Sub DeckShuffle(VpRndSeed As Integer)
	'--------------
	'Mélange le jeu
	'--------------
	Dim VpRnd As New Random(VpRndSeed)
		VmDrawn.Clear
		VmInPlay.Clear
		VmDeck = VmDeckCopy.Clone
		For VpI As Integer = 1 To clsModule.CgShuffleDepth * Me.CardsCount
			Me.DeckSwap(VpRnd.NextDouble * (Me.CardsCount - 1), VpRnd.NextDouble * (Me.CardsCount - 1))
		Next VpI
	End Sub
	Public Sub UntapAll
	'---------------------------
	'Désengage toutes les cartes
	'---------------------------
		For Each VpCard As clsCard In Me.VmDeckCopy		'Le faire sur VmDeckCopy permet de n'oublier aucune carte
			If Not (VpCard.IsSpecial AndAlso VpCard.Speciality.DoesntUntap) Then
				VpCard.Tapped = False
			End If
		Next VpCard
	End Sub
	Public Sub UntagAll
	'--------------------------
	'Démarque toutes les cartes
	'--------------------------
		For Each VpCard As clsCard In Me.VmDeckCopy		'Le faire sur VmDeckCopy permet de n'oublier aucune carte
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
	Private Function ManagePresence(VpCombo() As String, VpMode As Boolean) As Boolean
		For Each VpItem As String In VpCombo		
			If Me.IsDrawn(VpItem, Not VpMode) = VpMode Then
				Return VpMode
			End If
		Next VpItem
		Return Not VpMode
	End Function	
	Public Function IsComboPresent(VpCombo() As String) As Boolean
	'-------------------------------------------------------------
	'Renvoie vrai si toutes les cartes spécifiées ont été piochées
	'-------------------------------------------------------------	
		Return Me.ManagePresence(VpCombo, False)
	End Function
	Public Function IsOnePresent(VpCombo() As String) As Boolean
	'----------------------------------------------------------------
	'Renvoie vrai si au moins une des cartes spécifiées a été piochée
	'----------------------------------------------------------------
		Return Me.ManagePresence(VpCombo, True)
	End Function	
	Public Function IsInFullDeck(VpCardName As String) As Boolean
	'-------------------------------------------------------------------------------
	'Renvoie vrai si le deck contient la carte dont le nom est spécifié en paramètre
	'-------------------------------------------------------------------------------
		Return Me.IsInList(VpCardName, Me.VmDeckCopy)
	End Function
	Public Function IsInPlay(VpCardName As String) As Boolean
	'--------------------------------------------------------------------------------------------
	'Renvoie vrai si le champ de bataille contient la carte dont le nom est spécifié en paramètre
	'--------------------------------------------------------------------------------------------
		Return Me.IsInList(VpCardName, Me.VmInPlay)
	End Function	
	Private Function IsInList(VpCardName As String, VpList As ArrayList) As Boolean
		For Each VpCard As clsCard In VpList
			If VpCard.CardName = VpCardName Then
				Return True
			End If
		Next VpCard
		Return False
	End Function	
	Public Sub PrepNewPlayRound
	'------------------------------------
	'Préparation d'un nouveau tour de jeu
	'------------------------------------
		Me.VmInRound.Clear
		VmReserve = New clsManas
		Call Me.FollowRound	
	End Sub
	Public Sub FollowRound
	'--------------------------------------------------------------------------------------------------------------
	'Génération éventuelle de mana sur des cartes arrivées postérieurement par rapport à la pose du dernier terrain
	'--------------------------------------------------------------------------------------------------------------
		For Each VpCard As clsCard In Me.CardsInPlay
			If VpCard.ManaAble And Not VpCard.Tapped Then
				Call VmReserve.AddSubManas(VpCard.ManasGen)
				Call clsModule.VerboseSimu(VmVerbose, VpCard.ManasGen.Potentiel.ToString + " manas ajoutés à la réserve (" + VpCard.CardName + ")", VmSimuOut)
				VpCard.Tapped = True
			End If
		Next VpCard	
	End Sub
	Public Sub AddToInPlay(VpCard As clsCard)
	'-----------------------------------
	'Pose en jeu un permanent de la main
	'-----------------------------------
		If Not VpCard Is Nothing Then
			Me.VmInPlay.Add(VpCard)
			Me.VmDrawn.Remove(VpCard)		
		End If
	End Sub
	Public Sub AddToInRound(VpCard As clsCard)
	'----------------------------------
	'Pose en jeu un éphémère de la main
	'----------------------------------
		Me.VmInRound.Add(VpCard)
		Me.VmDrawn.Remove(VpCard)		
	End Sub	
	Public Sub CommitChange(VpSrc As ArrayList, VpDest As ArrayList, Optional VpRemove As Boolean = False, Optional VpRemoveDrawn As Boolean = True)
	'--------------------------------------------------------------------------------------------------
	'Synthèse des deux routines précédentes utile pour ne pas perturber lors de l'énumération des items
	'--------------------------------------------------------------------------------------------------
		For Each VpCard As clsCard In VpSrc
			If VpRemove Then
				VpDest.Remove(VpCard)
			Else
				VpDest.Add(VpCard)
			End If
			If VpRemoveDrawn Then
				Me.VmDrawn.Remove(VpCard)
			End If
		Next VpCard
	End Sub
	Public Function DoSpecialEffects(VpSrc As ArrayList) As Boolean
	'---------------------------------------------------------------------------------------------------------------------------------------------
	'Si des cartes possèdent des propriétés particulières spécifiées par l'utilisateur permettant de générer indirectement du mana, l'effectue ici
	'(renvoie vrai si un effet spécial a été utilisé)
	'---------------------------------------------------------------------------------------------------------------------------------------------
	Dim VpSomething As Boolean = False		'Passe à vrai si au moins un effet a été utilisé
	Dim VpNext As Boolean = False			'Passe à vrai si l'effet courant ne peut pas être utilisé
	Dim VpAbort As Boolean = False			'Passe à vrai si l'effet a été avorté (cible illégale etc...) auquel cas il faut annuler l'effort
	Dim VpManasInvoc As clsManas = Nothing	'Manas nécessaires pour activer l'effet spécial
	Dim VpInt As Integer					'Entier support
	Dim VpStrs() As String					'Chaîne support
	Dim VpTmpInPlay1 As New ArrayList		'Liste support ajout
	Dim VpTmpInPlay2 As New ArrayList		'Liste support suppression
	Dim VpTmpInPlay3 As New ArrayList		'Liste support défausse
		For Each VpCard As clsCard In VpSrc
			If VpCard.IsSpecial And Not VpCard.Tapped Then
				VpNext = False
				VpAbort = False
				'Commence par regarder si l'effort à fournir est réalisable, en termes de :
				Select Case VpCard.Speciality.EffortID
					'- engagement de la carte + manas nécessaires
					Case 0
						VpManasInvoc = New clsManas(VpCard.Speciality.Effort)
						If VpCard.Tapped Or Not Me.Reserve.ContainsEnoughFor(VpManasInvoc) Then
							VpNext = True
							Exit Select
						Else
							Call Me.Reserve.AddSubManas(VpManasInvoc, -1)
							VpCard.Tapped = True
						End If
					'- manas nécessaires ou manas nécessaires + sacrifice de cette carte
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
					'- engagement de la carte
					Case 2
						If VpCard.Tapped Then
							VpNext = True
							Exit Select
						Else
							VpCard.Tapped = True
						End If						
					'- sacrifice de cette carte (aucun prérequis)
					Case 10
						VpTmpInPlay2.Add(VpCard)
					'- engagement de la carte + cartes à sacrifier ou cartes à sacrifier
					Case 11, 12
						If VpCard.Speciality.EffortID = 12 Then
							If VpCard.Tapped Then
								VpNext = True
								Exit Select
							Else
								VpCard.Tapped = True
							End If		
						End If
						VpInt = 0
						VpStrs = VpCard.Speciality.Effort.Split(";")
						For Each VpTarget As clsCard In Me.CardsInPlay
							For VpSacrifice As Integer = 0 To VpStrs.Length - 1
								If VpTarget.CardName = VpStrs(VpSacrifice) And Not VpTmpInPlay2.Contains(VpTarget) Then
									VpTmpInPlay2.Add(VpTarget)
									VpInt = VpInt + 1
									VpStrs(VpSacrifice) = ""
									Exit For
								End If
							Next VpSacrifice
						Next VpTarget
						'Si on n'a pas pu sacrifier toutes les cartes requises, on sort
						VpNext = ( VpInt < VpStrs.Length )					
					'- cartes à défausser
					Case 40
						VpInt = CInt(VpCard.Speciality.Effort)
						If Me.CardsDrawn.Count < (VpInt + VpTmpInPlay3.Count) Then		'attention il faut prendre en compte les cartes défaussées pas encore committées
							VpNext = True
							Exit Select
						Else
							'On défausse les dernières cartes (à priori les moins favorisées par le dernier classement)
							VpTmpInPlay3.AddRange(Me.CardsDrawn.GetRange(Me.CardsDrawn.Count - VpInt - 1 - VpTmpInPlay3.Count, VpInt))
						End If
					'- points de vie à perdre
					Case 21
						VpInt = CInt(VpCard.Speciality.Effort)
						If VmLives <= VpInt Then
							VpNext = True
							Exit Select
						Else
							VmLives = VmLives - VpInt
						End If
					'- présence en jeu
					Case 30
						If Not Me.IsInPlay(VpCard.Speciality.Effort) Then
							VpNext = True
							Exit Select
						End If
					'- présence dans le cimetière
					Case 31

					'- quantité de présence dans le cimetière
					Case 32

					Case Else
				End Select
				'Si l'effort a été fourni
				If Not VpNext Then
					'Obtient la récompense, parmi :
					Select Case VpCard.Speciality.EffetID
						'- génération de manas
						Case 100
							Call Me.Reserve.AddSubManas(New clsManas(VpCard.Speciality.Effet))
							VpSomething = True
							Call clsModule.VerboseSimu(VmVerbose, "Effet utilisé : " + VpCard.CardName, VmSimuOut)
						'- dégagement d'un artefact
						Case 110
							'Liste des artefacts potentiellement visés
							VpStrs = VpCard.Speciality.Effet.Split(";")
							VpNext = False
							For Each VpArtefact As String In VpStrs
								If VpNext Then Exit For
								For Each VpTarget As clsCard In Me.CardsInPlay
									If (Not VpCard Is VpTarget) And VpTarget.CardName = VpArtefact And VpTarget.Tapped = True Then
										VpTarget.Tapped = False
										VpNext = True
										Call clsModule.VerboseSimu(VmVerbose, "Effet utilisé : " + VpCard.CardName + " pour dégager " + VpTarget.CardName, VmSimuOut)
										VpSomething = True
										Exit For
									End If
								Next VpTarget
							Next VpArtefact
							VpAbort = Not VpNext
						'- mise en jeu de l'artefact ou du terrain spécifié
						Case 120
							'Liste des cartes potentiellement visés
							VpStrs = VpCard.Speciality.Effet.Split(";")
							VpNext = False
							For Each VpWanted As String In VpStrs
								If VpNext Then Exit For
								For Each VpTarget As clsCard In Me.CardsInDeck
									If VpTarget.CardName = VpWanted And Not VpTmpInPlay1.Contains(VpTarget) Then
										VpTmpInPlay1.Add(VpTarget)
										Me.CardsInDeck.Remove(VpTarget)		'Normalement on n'a pas le droit de toucher à la liste que l'on est en train d'énumérer (ou celle qui la contient) mais là on quitte la boucle immédiatement après...
										VpNext = True
										Call clsModule.VerboseSimu(VmVerbose, "Effet utilisé : " + VpCard.CardName + " pour mettre en jeu " + VpTarget.CardName, VmSimuOut)
										VpSomething = True
										Exit For
									End If
								Next VpTarget
							Next VpWanted	
							VpAbort = Not VpNext
						'- pioche de cartes
						Case 130
							Call Me.Draw(CInt(VpCard.Speciality.Effet))
							Call clsModule.VerboseSimu(VmVerbose, "Effet utilisé : " + VpCard.CardName, VmSimuOut)
							VpSomething = True
						Case Else							
					End Select
				End If	
				'S'il faut annuler l'effort
				If VpAbort Then
					Select Case VpCard.Speciality.EffortID
						'- engagement de la carte + manas nécessaires
						Case 0
							VpCard.Tapped = False
							Call Me.Reserve.AddSubManas(VpManasInvoc)
						'- manas nécessaires
						Case 1
							Call Me.Reserve.AddSubManas(VpManasInvoc)
						'- engagement de la carte
						Case 2
							VpCard.Tapped = False							
						'- sacrifice de cette carte (aucun prérequis)
						Case 10
							VpTmpInPlay2.Clear
						'- cartes à sacrifier
						Case 11
							VpTmpInPlay2.Clear	
						'- engagement de la carte + cartes à sacrifier
						Case 12
							VpCard.Tapped = False	
							VpTmpInPlay2.Clear							
						'- cartes à défausser
						Case 40
							VpTmpInPlay3.Clear
						'- points de vie à perdre
						Case 21
							VmLives = VmLives + CInt(VpCard.Speciality.Effort)
						Case Else
					End Select					
				End If
			End If
		Next VpCard
		'Si on a mis en jeu ou retirer des cartes par un effet, c'est maintenant qu'il faut le commiter
		Call Me.CommitChange(VpTmpInPlay1, Me.CardsInPlay, False, False)
		Call Me.CommitChange(VpTmpInPlay2, Me.CardsInPlay, True, False)
		Call Me.CommitChange(VpTmpInPlay3, Me.CardsDrawn, True, False)
		'Les cartes nouvellement arrivées peuvent être susceptibles de produire du mana immédiatement				
		Call Me.FollowRound
		Return VpSomething
	End Function
	Public Function PickLand As clsCard
	'-----------------------------------------------------------------------------------------------------------------------
	'Détermine la couleur dominante de la main en cours pour retourner le terrain (s'il y en a un) le plus approprié à jouer
	'-----------------------------------------------------------------------------------------------------------------------
	Dim VpMaxPot As Short = 0
	Dim VpPot As Short
	Dim VpPicked As clsCard = Nothing
		'Parcourt les cartes de la main
		For Each VpCard As clsCard In VmDrawn
			'Si on a un terrain spécial, le sélectionne d'office
			If VpCard.IsALand AndAlso VpCard.IsSpecial Then
				Call clsModule.VerboseSimu(VmVerbose, "Terrain spécial posé : " + VpCard.CardName, VmSimuOut)
				Return VpCard
			'Si on a un terrain producteur de mana
			ElseIf VpCard.IsALand AndAlso VpCard.ManaAble Then
				'Détermine le potentiel sous-jacent (direct ou indirect) à sa mise en jeu
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
			Call clsModule.VerboseSimu(VmVerbose, "Terrain posé : " + VpPicked.CardName, VmSimuOut)
		End If
		Return VpPicked
	End Function
	Private Function GetPotFor(VpLand As clsCard) As Short
	'--------------------------------------------------------------------------------------------------
	'Retourne le potentiel générable par les cartes en main qui nécessite le terrain passé en paramètre
	'--------------------------------------------------------------------------------------------------
	Dim VpPot As Short = 0
		For Each VpCard As clsCard In VmDrawn
			If ( Not ( VpCard Is VpLand ) ) AndAlso VpCard.ManaAble AndAlso VpCard.Requires(VpLand) Then
				VpPot = VpPot + VpCard.ManasPot
			End If
		Next VpCard
		Return VpPot
	End Function
	Private Function IsDrawn(VpName As String, VpComboCare As Boolean) As Boolean
	'-------------------------------------------------
	'Renvoie vrai si la cartes spécifiée a été piochée
	'-------------------------------------------------
		For Each VpCard As clsCard In Me.VmDrawn
			If VpCard.CardName = VpName And Not VpCard.Tagged Then
				VpCard.Tagged = VpComboCare
				Return True
			End If
		Next VpCard
		Return False
	End Function
	Private Sub DeckSwap(VpI As Integer, VpJ As Integer)
	'------------------------------------------
	'Permute les deux cartes spécifiées du deck
	'------------------------------------------
	Dim VpTmp As clsCard
		VpTmp = VmDeck.Item(VpI)
		VmDeck.Item(VpI) = VmDeck.Item(VpJ)
		VmDeck.Item(VpJ) = VpTmp
	End Sub
	Public Function ManasPotentielIn(VpList As ArrayList) As Integer
	'-----------------------------------------------------------------------------
	'Retourne le nombre de manas potentiellement générables avec les cartes en jeu
	'-----------------------------------------------------------------------------
	Dim VpPot As Integer = 0
		For Each VpCard As clsCard In VpList
			If (Not VpCard.Tapped) AndAlso VpCard.ManaAble Then
				VpPot = VpPot + VpCard.ManasPot
			End If
		Next VpCard
		Return VpPot
	End Function
	Public ReadOnly Property CardsCount As Integer
		Get
			Return Me.VmDeckCopy.Count
		End Get
	End Property
	Public ReadOnly Property CardsDrawn As ArrayList
		Get
			Return Me.VmDrawn
		End Get
	End Property
	Public ReadOnly Property CardsInDeck As ArrayList
		Get
			Return Me.VmDeck
		End Get
	End Property	
	Public ReadOnly Property CardsInFullDeck As ArrayList
		Get
			Return Me.VmDeckCopy
		End Get
	End Property	
	Public ReadOnly Property CardsInPlay As ArrayList
		Get
			Return Me.VmInPlay
		End Get
	End Property
	Public ReadOnly Property CardsInRound As ArrayList
		Get
			Return Me.VmInRound
		End Get
	End Property	
	Public ReadOnly Property Reserve As clsManas
		Get
			Return Me.VmReserve
		End Get
	End Property	
End Class
Public Class clsCard
	Private VmCardName As String					'Nom de la carte (VO)
	Private VmCardType As String					'Type de la carte (C,I,A,E,L,N,S,T,U,P)
	Private VmManasInvoc As clsManas				'Coût d'invocation de la carte
	Private VmManasGen As clsManas = Nothing		'Manas générables par la carte
	Private VmTapped As Boolean = False				'Carte engagée ?
	Private VmTagged As Boolean = False				'Carte marquée ? (utilisation interne)
	Private VmSpeciality As clsSpeciality = Nothing	'Carte destinée à une utilisation spéciale (pour la simulation de déploiement uniquement) ?
	Public Sub New(VpCardName As String, Optional VpCardText As String = "", Optional VpCost As String = "", Optional VpType As String = "", Optional VpGestDeploy As Boolean = False)
	Dim VpGCost As String		
		VmCardName = VpCardName
		If VpGestDeploy Then
			VmCardType = VpType
			VmManasInvoc = New clsManas(VpCost)
			'On ne s'occupe pas des arpenteurs
			If VpType = "P" Then Exit Sub
			VpCardText = VpCardText.ToLower.Replace(vbCrLf, " ").Replace(".", "")
			'Carte parsable
			If VpCardText.Contains(clsModule.CgManaParsing(0)) Then
				VpGCost = VpCardText.Substring(0, VpCardText.IndexOf(clsModule.CgManaParsing(0)) - 1)
				'Carte dont le mana générable dépend d'autres paramètres non contrôlables (dans ce cas on devrait plutôt se trouver dans la situation de myspecialuses)
				If VpGCost.EndsWith(clsModule.CgManaParsing(1)) Then
					'On affecte un mana incolore par défaut
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
	'Renvoie vrai si l'invocation de la carte courante requiert le mana produit par la carte passée en paramètre
	'-----------------------------------------------------------------------------------------------------------
		Return VmManasInvoc.IsBetterWith(VpCard.ManasGen)
	End Function
	Public ReadOnly Property CardName As String
		Get
			Return Me.VmCardName
		End Get
	End Property
	Public ReadOnly Property CardType As String
		Get
			Return Me.VmCardType
		End Get
	End Property
	Public ReadOnly Property ManaAble As Boolean
		Get
			Return Not ( Me.VmManasGen Is Nothing )
		End Get
	End Property	
	Public ReadOnly Property ManasPot As Integer
		Get
			Return Me.VmManasGen.Potentiel
		End Get
	End Property
	Public ReadOnly Property ManasGen As clsManas
		Get
			Return Me.VmManasGen
		End Get
	End Property
	Public ReadOnly Property ManasInvoc As clsManas
		Get
			Return Me.VmManasInvoc
		End Get
	End Property	
	Public ReadOnly Property IsALand As Boolean
		Get
			Return Me.VmCardType = "L"
		End Get
	End Property
	Public Property Tapped As Boolean
		Get
			Return VmTapped
		End Get
		Set(VpTapped As Boolean)
			VmTapped = VpTapped
		End Set
	End Property
	Public Property Tagged As Boolean
		Get
			Return VmTagged
		End Get
		Set(VpTagged As Boolean)
			VmTagged = VpTagged
		End Set
	End Property	
	Public Property Speciality As clsSpeciality
		Get
			Return VmSpeciality
		End Get
		Set(VpSpeciality As clsSpeciality)
			VmSpeciality = VpSpeciality
			VmManasGen = Nothing			'Si la carte a une spécialité, on efface le parsing par défaut qui avait eu lieu précédemment
		End Set
	End Property
	Public ReadOnly Property IsSpecial As Boolean
		Get
			Return Not ( VmSpeciality Is Nothing )
		End Get
	End Property
End Class
Public Class clsEsperance
	Private VmEsp As New Hashtable
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
	Public Function GetEsp(Optional VpPercent As Boolean = True) As Hashtable
	Dim VpEsp As New Hashtable(VmEsp.Count)
		For Each VpKey As Integer In VmEsp.Keys
			VpEsp.Add(VpKey, IIf(VpPercent, 100, 1) * VmEsp.Item(VpKey) / VmOccurences)
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
