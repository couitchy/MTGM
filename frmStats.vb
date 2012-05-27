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
'| - levée de l'ambiguité sur les sources  03/10/2009 |
'| - gestion des autorisations tournois	   17/04/2010 |
'| - gestion cartes foils				   19/12/2010 |
'| - considère nb. rééditions pour raretés 27/05/2012 |
'------------------------------------------------------
Imports SourceGrid2
Imports Cells = SourceGrid2.Cells.Real
Imports SoftwareFX.ChartFX.Lite
Imports NPlot
Public Partial Class frmStats
	Private VmSource As String
	Private VmRestriction As String
	Private VmOwnerChildren As clsChildren
	Public Sub New(VpOwner As MainForm)
		Me.InitializeComponent()
		VmSource = If(VpOwner.FilterCriteria.DeckMode, clsModule.CgSDecks, clsModule.CgSCollection)
		VmRestriction = VpOwner.Restriction
		VmOwnerChildren = VpOwner.MyChildren
		Me.Text = clsModule.CgStats + VpOwner.Restriction(True)
		AddHandler Me.cboCriterion.ComboBox.SelectedIndexChanged, AddressOf CboCriterionSelectedIndexChanged
	End Sub
	Private Sub LoadGrid
	'-----------------------------------------------------------------------------------------------------------
	'Récupère les différentes valeurs possibles du critère demandé ainsi que le nombre de cartes y correspondant
	'-----------------------------------------------------------------------------------------------------------
	Dim VpSQL As String
	Dim VpValues As New List(Of String)
		'Préparation de la grille
		With Me.grdDetails
			'Nettoyage
			If .Rows.Count > 0 Then
				.Rows.RemoveRange(0, .Rows.Count)
			End If
			'Nombre de colonnes et d'en-tête
			.ColumnsCount = 2
			.FixedRows = 1
			.Rows.Insert(0)
			Me.grdDetails(0, 0) = New Cells.ColumnHeader(Me.cboCriterion.ControlText)
			Me.grdDetails(0, 1) = New Cells.ColumnHeader("Quantité")
		End With
		'Récupération des valeurs possibles
		VpSQL = "Select Distinct " + clsModule.CgCriteres.Item(Me.cboCriterion.ControlText) + " From (" + VmSource + " Inner Join Card On " + VmSource + ".EncNbr = Card.EncNbr) Inner Join Spell On Card.Title = Spell.Title Where "
		VpSQL = VpSQL + VmRestriction
		VpSQL = clsModule.TrimQuery(VpSQL)
		VgDBCommand.CommandText = VpSQL
		VgDBReader = VgDBcommand.ExecuteReader
		With VgDBReader
			While .Read
				VpValues.Add(.GetValue(0).ToString)
			End While
			.Close
		End With
		If Me.cboCriterion.ComboBox.SelectedIndex = 3 Then	'Mana curve (un peu crade à cet endroit)
			VpValues.Sort(New clsNumComparer)
		End If
		'Nombre d'items correspondant
		With Me.grdDetails
			For Each VpValue As String In VpValues
				Select Case Me.cboCriterion.ComboBox.SelectedIndex
					'Requête numérique
					Case 3, 5
						VpSQL = "Select Sum(" + VmSource + ".Items) From (" + VmSource + " Inner Join Card On " + VmSource + ".EncNbr = Card.EncNbr) Inner Join Spell On Card.Title = Spell.Title Where " + clsModule.CgCriteres.Item(Me.cboCriterion.ControlText) + " = " + VpValue.Replace(",", ".") + " And " + VmRestriction
					Case Else
						VpSQL = "Select Sum(" + VmSource + ".Items) From (" + VmSource + " Inner Join Card On " + VmSource + ".EncNbr = Card.EncNbr) Inner Join Spell On Card.Title = Spell.Title Where " + clsModule.CgCriteres.Item(Me.cboCriterion.ControlText) + " = '" + VpValue + "' And " + VmRestriction
				End Select
				VgDBCommand.CommandText = clsModule.TrimQuery(VpSQL)
				.Rows.Insert(.RowsCount)
				Me.grdDetails(.RowsCount - 1, 1) = New Cells.Cell(VgDBCommand.ExecuteScalar)
				Me.grdDetails(.RowsCount - 1, 0) = New Cells.Cell(clsModule.FormatTitle(clsModule.CgCriteres.Item(Me.cboCriterion.ControlText), VpValue, , False))
			Next VpValue
			.AutoSize
		End With
	End Sub
	Private Sub LoadGraph(Optional VpDrawManaCurve As Boolean = False)
	'-------------------------------------------------------------------------------------------------------
	'Dessine le diagramme de répartition correspondant aux données présentes dans la grille ou la mana curve
	'-------------------------------------------------------------------------------------------------------
	Dim VpI As Integer
	Dim VpManaCurve As HistogramPlot
	Dim VpQuants() As Integer
	Dim VpCosts() As Integer
	Dim VpY() As Integer
	Dim VpC As Integer = 0
		If Not VpDrawManaCurve Then
			Me.chartManaCurve.Visible = False
			With Me.chartBreakDown
				.Visible = False
				.ClearData(ClearDataFlag.Data)
				.SerLeg.Clear
				.OpenData(COD.Values, 1, Me.grdDetails.RowsCount - 1)
				For VpI = 1 To Me.grdDetails.RowsCount - 1
					.Value(0, VpI - 1) = Me.grdDetails(VpI, 1).Value
					.SerLeg(VpI - 1) = Me.grdDetails(VpI, 0).Value
				Next VpI
				.SerLegBox = True
				.Chart3D = True
				.Gallery = Gallery.Pie
				.CloseData(COD.Values)
				.Visible = True
			End With
		Else
			Me.chartBreakDown.Visible = False
			ReDim VpCosts(0 To Me.grdDetails.RowsCount - 2)
			ReDim VpQuants(0 To Me.grdDetails.RowsCount - 2)
			For VpI = 1 To Me.grdDetails.RowsCount - 1
				VpCosts(VpI - 1) = Me.grdDetails(VpI, 0).Value
				VpQuants(VpI - 1) = Me.grdDetails(VpI, 1).Value
				If VpCosts(VpI - 1) > VpC Then
					VpC = VpCosts(VpI - 1)
				End If
			Next VpI
			ReDim VpY(0 To VpC)
			For VpI = 0 To VpCosts.Length - 1
				VpY(VpCosts(VpI)) = VpQuants(VpI)
			Next VpI
			VpManaCurve = New HistogramPlot
			VpManaCurve.DataSource = VpY
			VpManaCurve.Color = Color.Blue
			VpManaCurve.RectangleBrush = New RectangleBrushes.HorizontalCenterFade(Color.Blue, Color.White)
			VpManaCurve.BaseWidth = 0.5
			VpManaCurve.Filled = True
			Me.chartManaCurve.Clear
			Me.chartManaCurve.Add(VpManaCurve)
			Me.chartManaCurve.Visible = True
		End If
	End Sub
	Private Sub LoadAutorisations
	'-------------------------------------------------------
	'Chargement des autorisations tournois pour la sélection
	'-------------------------------------------------------
	Dim VpHas1ItemRestr As Boolean = False
		'Autorisations T1
		If Not Me.GetAutorisation("T1", VpHas1ItemRestr) Then
			Me.picAutT1.Image = Me.imglstAutorisations.Images.Item(3)
		ElseIf VpHas1ItemRestr Then
			Me.picAutT1.Image = Me.imglstAutorisations.Images.Item(4)
		Else
			Me.picAutT1.Image = Me.imglstAutorisations.Images.Item(2)
		End If
		'Autorisations T1.5
		If Me.GetAutorisation("T15") Then
			Me.picAutT15.Image = Me.imglstAutorisations.Images.Item(9)
		Else
			Me.picAutT15.Image = Me.imglstAutorisations.Images.Item(10)
		End If
		'Autorisations M
		If Me.GetAutorisation("M") Then
			Me.picAutM.Image = Me.imglstAutorisations.Images.Item(11)
		Else
			Me.picAutM.Image = Me.imglstAutorisations.Images.Item(12)
		End If
		'Autorisations T1x
		If Me.GetAutorisation("T1x") Then
			Me.picAutT1x.Image = Me.imglstAutorisations.Images.Item(5)
		Else
			Me.picAutT1x.Image = Me.imglstAutorisations.Images.Item(6)
		End If
		'Autorisations T2
		If Me.GetAutorisation("T2") Then
			Me.picAutT2.Image = Me.imglstAutorisations.Images.Item(7)
		Else
			Me.picAutT2.Image = Me.imglstAutorisations.Images.Item(8)
		End If
		'Autorisations Bloc
		If Me.GetAutorisation("Bloc") Then
			Me.picAutBloc.Image = Me.imglstAutorisations.Images.Item(0)
		Else
			Me.picAutBloc.Image = Me.imglstAutorisations.Images.Item(1)
		End If
	End Sub
	Private Function GetAutorisation(VpTournoiType As String, Optional ByRef VpHas1ItemRestr As Boolean = False) As Boolean
	'------------------------------------------------------------------------------------------------------------------------
	'Indique si la sélection de cartes courante est compatible avec les restrictions tournoi d'identifiant passé en paramètre
	'------------------------------------------------------------------------------------------------------------------------
	Dim VpGranted As Boolean = True
	Dim VpSQL As String
	Dim VpControl1Item As New List(Of String)
		VpSQL = "Select " + VpTournoiType + ", T1r, Items, Card.Title From (" + VmSource + " Inner Join Card On " + VmSource + ".EncNbr = Card.EncNbr) Inner Join Autorisations On Card.Title = Autorisations.Title Where "
		VpSQL = VpSQL + VmRestriction
		VpSQL = clsModule.TrimQuery(VpSQL)
		VgDBCommand.CommandText = VpSQL
		VgDBReader = VgDBCommand.ExecuteReader
		With VgDBReader
			While .Read
				If Not .GetBoolean(0) Then
					VpGranted = False
					Exit While
				End If
				'Vérification pour les cartes restreintes à un exemplaire
				If .GetBoolean(1) Then
					VpHas1ItemRestr = True
					If .GetInt32(2) > 1 Or VpControl1Item.Contains(.GetString(3)) Then
						VpGranted = False
						Exit While
					Else
						VpControl1Item.Add(.GetString(3))
					End If
				End If
			End While
			.Close
		End With
		Return VpGranted
	End Function
	Private Sub LoadConflictingCards(VpTournoiType As String)
	'-----------------------------------------------------------------------------
	'Affiche les cartes non compatibles avec le type de tournoi passé en paramètre
	'-----------------------------------------------------------------------------
	Dim VpSQL As String
		Me.lstTournoiForbid.Items.Clear
		'Cas général
		VpSQL = "Select Card.Title From (" + VmSource + " Inner Join Card On " + VmSource + ".EncNbr = Card.EncNbr) Inner Join Autorisations On Card.Title = Autorisations.Title Where " + VpTournoiType + " = False And "
		VpSQL = VpSQL + VmRestriction
		VpSQL = clsModule.TrimQuery(VpSQL)
		VgDBCommand.CommandText = VpSQL
		VgDBReader = VgDBCommand.ExecuteReader
		With VgDBReader
			While .Read
				If Not Me.lstTournoiForbid.Items.Contains(.GetString(0)) Then
					Me.lstTournoiForbid.Items.Add(.GetString(0))
				End If
			End While
			.Close
		End With
		'Cas des cartes restreintes à un exemplaire
		If VpTournoiType = "T1" Then
			VpSQL = "Select Card.Title, Sum(Items) From (" + VmSource + " Inner Join Card On " + VmSource + ".EncNbr = Card.EncNbr) Inner Join Autorisations On Card.Title = Autorisations.Title Where T1r = True And "
			VpSQL = VpSQL + VmRestriction
			VpSQL = clsModule.TrimQuery(VpSQL, , " Group By Card.Title")
			VgDBCommand.CommandText = VpSQL
			VgDBReader = VgDBCommand.ExecuteReader
			With VgDBReader
				While .Read
					If CInt(.GetValue(1)) > 1 And Not Me.lstTournoiForbid.Items.Contains(.GetString(0)) Then
						Me.lstTournoiForbid.Items.Add(.GetString(0) + " (1 exemplaire max.)")
					End If
				End While
				.Close
			End With
		End If
		If Me.lstTournoiForbid.Items.Count = 0 Then
			Me.lstTournoiForbid.Items.Add("N/C")
		End If
		Me.lstTournoiForbid.Sorted = True
	End Sub
	Private Function GetPriceHistory As SortedList
	'---------------------------------------------------------
	'Retourne l'historique des prix pour la sélection courante
	'---------------------------------------------------------
	Dim VpHist As New SortedList
	Dim VpSQL As String
		'VpSQL = "Select PriceDate, Sum(Price * Items) From (SELECT PricesHistory.EncNbr, PricesHistory.Price, DatesToUse.PriceDate FROM PricesHistory INNER JOIN (SELECT PricesHistory.EncNbr, Max(PricesHistory.PriceDate) AS DLAST, AllDates.PriceDate FROM PricesHistory, (SELECT Distinct PricesHistory.PriceDate FROM PricesHistory) As AllDates WHERE (((PricesHistory.PriceDate)<=[AllDates].[PriceDate])) GROUP BY PricesHistory.EncNbr, AllDates.PriceDate) AS DatesToUse ON (PricesHistory.EncNbr = DatesToUse.EncNbr) AND (PricesHistory.PriceDate = DatesToUse.DLAST)) As GlobalHisto Inner Join " + VmSource + " On " + VmSource + ".EncNbr = GlobalHisto.EncNbr Where "
		VpSQL = "Select PriceDate, Sum(Price * Items) From (SELECT PricesHistory.EncNbr, PricesHistory.Price, DatesToUse.PriceDate, DatesToUse.Foil FROM PricesHistory INNER JOIN (SELECT PricesHistory.EncNbr, Max(PricesHistory.PriceDate) AS DLAST, AllDates.PriceDate, PricesHistory.Foil FROM PricesHistory, (SELECT Distinct PricesHistory.PriceDate FROM PricesHistory) As AllDates WHERE (((PricesHistory.PriceDate)<=[AllDates].[PriceDate])) GROUP BY PricesHistory.EncNbr, AllDates.PriceDate, PricesHistory.Foil) AS DatesToUse ON (PricesHistory.EncNbr = DatesToUse.EncNbr) AND (PricesHistory.PriceDate = DatesToUse.DLAST) AND (PricesHistory.Foil = DatesToUse.Foil)) As GlobalHisto Inner Join " + VmSource + " On (" + VmSource + ".EncNbr = GlobalHisto.EncNbr) And (" + VmSource + ".Foil = GlobalHisto.Foil) Where "
		VpSQL = VpSQL + VmRestriction
		VpSQL = clsModule.TrimQuery(VpSQL, , " Group By PriceDate")
		VgDBCommand.CommandText = VpSQL
		VgDBReader = VgDBCommand.ExecuteReader
		With VgDBReader
			While .Read
				VpHist.Add(.GetDateTime(0), .GetValue(1))
			End While
			.Close
		End With
		Return VpHist
	End Function
	Private Sub ShowCardsPrices(VpGrapher As frmGrapher, Optional VpMyPriceCriteria As String = "")
	'--------------------------------------------------------------------------------------------
	'Retourne l'historique des prix pour la sélection courante restreinte à la catégorie demandée
	'--------------------------------------------------------------------------------------------
	Dim VpHist As New SortedList
	Dim VpSQL As String
	Dim VpLastTitle As String = ""
	Dim VpTitle As String
		If clsModule.HasPriceHistory Then
			'VpSQL = "Select Card.Title, Card.Series, GlobalHisto.PriceDate, GlobalHisto.Price From ((SELECT PricesHistory.EncNbr, PricesHistory.Price, DatesToUse.PriceDate FROM PricesHistory INNER JOIN (SELECT PricesHistory.EncNbr, Max(PricesHistory.PriceDate) AS DLAST, AllDates.PriceDate FROM PricesHistory, (SELECT Distinct PricesHistory.PriceDate FROM PricesHistory) As AllDates WHERE (((PricesHistory.PriceDate)<=[AllDates].[PriceDate])) GROUP BY PricesHistory.EncNbr, AllDates.PriceDate) AS DatesToUse ON (PricesHistory.EncNbr = DatesToUse.EncNbr) AND (PricesHistory.PriceDate = DatesToUse.DLAST)) As GlobalHisto Inner Join " + VmSource + " On " + VmSource + ".EncNbr = GlobalHisto.EncNbr) Inner Join Card On Card.EncNbr = " + VmSource + ".EncNbr Where "
			VpSQL = "Select Card.Title, Card.Series, GlobalHisto.PriceDate, GlobalHisto.Price, " + VmSource + ".Foil From ((SELECT PricesHistory.EncNbr, PricesHistory.Price, DatesToUse.PriceDate, DatesToUse.Foil FROM PricesHistory INNER JOIN (SELECT PricesHistory.EncNbr, Max(PricesHistory.PriceDate) AS DLAST, AllDates.PriceDate, PricesHistory.Foil FROM PricesHistory, (SELECT Distinct PricesHistory.PriceDate FROM PricesHistory) As AllDates WHERE (((PricesHistory.PriceDate)<=[AllDates].[PriceDate])) GROUP BY PricesHistory.EncNbr, AllDates.PriceDate, PricesHistory.Foil) AS DatesToUse ON (PricesHistory.EncNbr = DatesToUse.EncNbr) AND (PricesHistory.PriceDate = DatesToUse.DLAST) AND (PricesHistory.Foil = DatesToUse.Foil)) As GlobalHisto Inner Join " + VmSource + " On (" + VmSource + ".EncNbr = GlobalHisto.EncNbr) And (" + VmSource + ".Foil = GlobalHisto.Foil)) Inner Join Card On Card.EncNbr = " + VmSource + ".EncNbr Where "
			If VpMyPriceCriteria <> "" Then
				VpSQL = VpSQL + "Card.myPrice = " + VpMyPriceCriteria + " And "
			End If
			VpSQL = VpSQL + VmRestriction
			VpSQL = clsModule.TrimQuery(VpSQL, , " Order By Card.EncNbr")
			VgDBCommand.CommandText = VpSQL
			VgDBReader = VgDBCommand.ExecuteReader
			With VgDBReader
				While .Read
					VpTitle = .GetString(0) + " (" + .GetString(1) + If(.GetBoolean(4), " foil)", ")")
					If VpTitle <> VpLastTitle Then
						If VpHist.Count > 0 Then
							If VpGrapher.GraphsCount >= clsModule.CgMaxGraphs Then
								Call clsModule.ShowWarning(clsModule.CgErr4)
								.Close
								Exit Sub
							Else
								VpGrapher.AddNewPlot(VpHist, VpLastTitle)
								VpHist.Clear
							End If
						End If
						VpLastTitle = VpTitle
					End If
					If Not VpHist.Contains(.GetDateTime(2)) Then
						VpHist.Add(.GetDateTime(2), .GetValue(3))
					End If
				End While
				.Close
				If VpHist.Count > 0 Then	'dernier élément non traité
					VpGrapher.AddNewPlot(VpHist, VpLastTitle)
					VpHist.Clear
				End If
			End With
		Else
			Call clsModule.ShowWarning(clsModule.CgErr2)
		End If
	End Sub
	Private Function GetRarest(VpLevel As Integer) As String
	'---------------------------------------------------------------------------------------------------------
	'Retourne la carte la plus rare (si on ne trouve pas de mythique, on passe aux rares, et ainsi de suite)
	'en prenant en compte le nombre de rééditions des cartes (plus une carte est rééditée, moins elle est rare
	'---------------------------------------------------------------------------------------------------------
	Dim VpSQL As String
	Dim VpO As Object	
		VpSQL = "Select Rares.Title From (" + VmSource + " Inner Join Card On " + VmSource + ".EncNbr = Card.EncNbr) Inner Join (Select Card.Title, Count(Card.Title) As Nb From Card Where InStr(UCase(Rarity), " + clsModule.CgRarities(VpLevel) + ") > 0 Group By Card.Title) As Rares On Card.Title = Rares.Title Where "
		VpSQL = VpSQL + VmRestriction
		VgDBCommand.CommandText = clsModule.TrimQuery(VpSQL, False) + " Order By Rares.Nb;"
		VpO = VgDBCommand.ExecuteScalar	
		If VpO Is Nothing Then
			Return Me.GetRarest(VpLevel + 1)
		Else
			Return VpO.ToString
		End If
	End Function
	Private Sub LoadInfos
	'------------------------------------------------------------------------------------------------------------------------------------------------------------
	'Récupère dans la base les informations :
	'- coûts d'invocations minimal, maximal et moyen
	'- prix total, moyen et plus élevé
	'- nombre de cartes, la plus vieille, la plus rare, créature la plus forte
	'- attaque moyenne, défense moyenne et ratio d'efficacité (attaque / cout moyen)
	'------------------------------------------------------------------------------------------------------------------------------------------------------------
	Dim VpC As Double
	Dim VpP As Double
	Dim VpT As Double
	Dim VpMaxNoFoil As Double
	Dim VpMaxFoil As Double		
		'Trappe d'erreur (si pas de créatures dans la sélection ou pas de cartes invocables)
		Try
			VpC = Me.QueryInfo("Sum(myCost * Items) / Sum(Items)", "Where ( Cost <> Null ) And ")
			VpP = Me.QueryInfo("Sum(Val(Power) * Items) / Sum(Items)", "Inner Join Creature On Card.Title = Creature.Title Where ( InStr(Power, '*') = 0 And InStr(Tough, '*') = 0 And (Power <> '0' Or Tough <> '0') ) And ")
			VpT = Me.QueryInfo("Sum(Val(Tough) * Items) / Sum(Items)", "Inner Join Creature On Card.Title = Creature.Title Where ( InStr(Power, '*') = 0 And InStr(Tough, '*') = 0 And (Power <> '0' Or Tough <> '0') ) And ")
		Catch
		End Try
		Me.txtMaxCost.Text = Me.QueryInfo("Card.Title", , " Order By myCost Desc;") + " : " + Me.QueryInfo("Max(myCost)").ToString
		'Me.txtMeanPrice2.Text = Format(Me.QueryInfo("Avg(Price)"), "0.00") + " €"
		Me.txtMeanPrice2.Text = Format(Me.QueryInfo("Avg(IIf(Foil, FoilPrice, Price))"), "0.00") + " €"
		Me.txtMeanCost.Text = Format(Me.QueryInfo("Sum(myCost * Items) / Sum(Items)"), "0.0")
		'Me.txtMeanPrice.Text = Format(Me.QueryInfo("Sum(Price * Items) / Sum(Items)"), "0.00") + " €"
		Me.txtMeanPrice.Text = Format(Me.QueryInfo("Sum(IIf(Foil, FoilPrice, Price) * Items) / Sum(Items)"), "0.00") + " €"
		Me.txtMinCost.Text = Me.QueryInfo("Card.Title", "Where Type <> 'L' And ", " Order By myCost Asc;") + " : " + Me.QueryInfo("Min(myCost)", "Where Type <> 'L' And ").ToString
		'Me.txtMostExpensive.Text = Me.QueryInfo("Card.Title", , " Order By Price Desc;") + " : " + Format(Me.QueryInfo("Max(Price)"), "0.00") + " €"
		VpMaxNoFoil = Me.QueryInfo("Max(Price)")
		'Trappe d'erreur (si aucune carte foil)
		Try
			VpMaxFoil = Me.QueryInfo("Max(FoilPrice)", "Where Foil = True And ")
		Catch
			VpMaxFoil = 0
		End Try
		If VpMaxFoil > VpMaxNoFoil Then
			Me.txtMostExpensive.Text = Me.QueryInfo("Card.Title", , " Order By FoilPrice Desc;") + clsModule.CgFoil2 + " : " + Format(VpMaxFoil, "0.00") + " €"
		Else
			Me.txtMostExpensive.Text = Me.QueryInfo("Card.Title", , " Order By Price Desc;") + " : " + Format(VpMaxNoFoil, "0.00") + " €"
		End If
		Me.txtNCartes.Text = Me.QueryInfo("Sum(Items)").ToString
		Me.txtOldest.Text = Me.QueryInfo("Card.Title", , " Order By Release Asc;")
		Me.txtRarest.Text = Me.GetRarest(0)
		'Me.txtTotPrice.Text = Format(Me.QueryInfo("Sum(Price * Items)"), "0.00") + " €"
		Me.txtTotPrice.Text = Format(Me.QueryInfo("Sum(IIf(Foil, FoilPrice, Price) * Items)"), "0.00") + " €"
		Me.txtTougher.Text = Me.QueryInfo("Card.Title", "Inner Join Creature On Card.Title = Creature.Title ", " Order By Val(Power) Desc;")
		Me.txtMeanCost2.Text = Format(VpC, "0.0")
		Me.txtMeanPower.Text = Format(VpP, "0.0")
		Me.txtMeanTough.Text = Format(VpT, "0.0")
		Me.txtRAD.Text = Format(VpP / VpT, "0.0")
		Me.txtRAC.Text = Format(VpP / VpC, "0.0")
	End Sub
	Private Function QueryInfo(VpQuery As String, Optional VpTblCreature As String = "", Optional VpSort As String = "") As Object
	'------------------
	'Requête ponctuelle
	'------------------
	Dim VpSQL As String
		VpSQL = "Select " + VpQuery + " From (((" + VmSource + " Inner Join Card On " + VmSource + ".EncNbr = Card.EncNbr) Inner Join Spell On Card.Title = Spell.Title) Inner Join Series On Card.Series = Series.SeriesCD) " + VpTblCreature
		VpSQL = VpSQL + If(VpSQL.EndsWith("And "), "", "Where ")
		VpSQL = VpSQL + VmRestriction
		VgDBCommand.CommandText = clsModule.TrimQuery(VpSQL, False) + VpSort
		Return VgDBCommand.ExecuteScalar
	End Function
	Private Function GetGrapher As frmGrapher
	Dim VpPricesHistory As frmGrapher
		If VmOwnerChildren.DoesntExist(VmOwnerChildren.PricesHistory) Then
			VpPricesHistory = New frmGrapher
			VmOwnerChildren.PricesHistory = VpPricesHistory
		Else
			VpPricesHistory = VmOwnerChildren.PricesHistory
		End If
		VpPricesHistory.Show
		VpPricesHistory.BringToFront
		Application.DoEvents
		Return VpPricesHistory
	End Function
	Sub CboCriterionSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
		Call Me.LoadGrid
		Call Me.LoadGraph
		Me.cmnuCurve.Enabled = ( Me.cboCriterion.ComboBox.SelectedIndex = 3 )
		If Me.cmnuCurve.Enabled = False And Me.cmnuCurve.Checked = True Then
			Me.cmnuCurve.Checked = False
			Me.cmnuBreakDown.Checked = True
		End If
	End Sub
	Sub FrmStatsLoad(ByVal sender As Object, ByVal e As EventArgs)
		Call Me.CboCriterionSelectedIndexChanged(sender, e)
		Call Me.LoadInfos
		Call Me.LoadAutorisations
	End Sub
	Sub FrmStatsPaint(ByVal sender As Object, ByVal e As PaintEventArgs)
		'Contourne un bug de rafraîchissement sur le graphique
		If Me.chartBreakDown.Visible Then
			Me.chartBreakDown.Visible = False
			Me.chartBreakDown.Visible = True
		End If
	End Sub
	Sub FrmStatsActivated(ByVal sender As Object, ByVal e As EventArgs)
		Call Me.FrmStatsPaint(sender, Nothing)
	End Sub
	Sub CmnuBreakDownClick(ByVal sender As Object, ByVal e As EventArgs)
		Me.cmnuBreakDown.Checked = True
		Me.cmnuCurve.Checked = False
		Me.chartManaCurve.Visible = False
		Me.chartBreakDown.Visible = True
	End Sub
	Sub CmnuCurveClick(ByVal sender As Object, ByVal e As EventArgs)
		Me.cmnuBreakDown.Checked = False
		Me.cmnuCurve.Checked = True
		Call Me.LoadGraph(True)
	End Sub
	Sub PicAutClick(sender As Object, e As EventArgs)
		Call Me.LoadConflictingCards(sender.Tag)
	End Sub
	Sub CmnuHistDeckClick(sender As Object, e As EventArgs)
		If clsModule.HasPriceHistory Then
			Me.GetGrapher.AddNewPlot(Me.GetPriceHistory, Me.Text.Replace(clsModule.CgStats, ""))
		Else
			Call clsModule.ShowWarning(clsModule.CgErr2)
		End If
	End Sub
	Sub CmdHistPricesMouseDown(sender As Object, e As MouseEventArgs)
		Me.cmnuHisto.Show(sender, New Point(e.X, e.Y))
	End Sub
	Sub CmnuHistAllCardsClick(sender As Object, e As EventArgs)
		Call Me.ShowCardsPrices(Me.GetGrapher)
	End Sub
	Sub CmnuHistCardsPriceClick(sender As Object, e As EventArgs)
		Call Me.ShowCardsPrices(Me.GetGrapher, sender.Tag)
	End Sub
End Class
Public Class clsNumComparer
	Implements IComparer(Of String)
	Public Function Compare(ByVal x As String, ByVal y As String) As Integer Implements IComparer(Of String).Compare
		Return CInt(x) - CInt(y)
	End Function
End Class