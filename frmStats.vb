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
'| - levée de l'ambiguité sur les sources  03/10/2009 |
'------------------------------------------------------
Imports SourceGrid2
Imports Cells = SourceGrid2.Cells.Real
Imports SoftwareFX.ChartFX.Lite
Public Partial Class frmStats
	Private VmSource As String	
	Private VmRestriction As String
	Public Sub New(VpOwner As MainForm)
		Me.InitializeComponent()
		VmSource = IIf(VpOwner.chkClassement.GetItemChecked(0), clsModule.CgSDecks, clsModule.CgSCollection)	
		VmRestriction = VpOwner.Restriction
		Me.Text = clsModule.CgStats + VpOwner.Restriction(True)	
		AddHandler Me.cboCriterion.ComboBox.SelectedIndexChanged, AddressOf CboCriterionSelectedIndexChanged
	End Sub
	Private Sub LoadGrid
	'-----------------------------------------------------------------------------------------------------------
	'Récupère les différentes valeurs possibles du critère demandé ainsi que le nombre de cartes y correspondant
	'-----------------------------------------------------------------------------------------------------------
	Dim VpSQL As String
	Dim VpValues As New ArrayList
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
				VpValues.Add(.GetString(0))
			End While
			.Close
		End With
		If Me.cboCriterion.ComboBox.SelectedIndex = 3 Then	'Mana curve (un peu crade à cet endroit)
			VpValues.Sort(New clsNumComparer)
		End If
		'Nombre d'items correspondant
		With Me.grdDetails
			For Each VpValue As String In VpValues
				VpSQL = "Select Sum(" + VmSource + ".Items) From (" + VmSource + " Inner Join Card On " + VmSource + ".EncNbr = Card.EncNbr) Inner Join Spell On Card.Title = Spell.Title Where " + clsModule.CgCriteres.Item(Me.cboCriterion.ControlText) + " ='" + VpValue + "' And " + VmRestriction
				VgDBCommand.CommandText = clsModule.TrimQuery(VpSQL)
				.Rows.Insert(.RowsCount)
				Me.grdDetails(.RowsCount - 1, 1) = New Cells.Cell(VgDBCommand.ExecuteScalar)
				Me.grdDetails(.RowsCount - 1, 0) = New Cells.Cell(clsModule.FormatTitle(clsModule.CgCriteres.Item(Me.cboCriterion.ControlText), VpValue, False))
			Next VpValue
			.AutoSize
		End With
	End Sub
	Private Sub LoadGraph
	'--------------------------------------------------------------------------------------
	'Dessine le diagramme de répartition correspondant aux données présentes dans la grille
	'--------------------------------------------------------------------------------------
	Dim VpI As Integer
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
	End Sub
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
		VpC = Me.QueryInfo("Sum(Val(myCost) * Items) / Sum(Items)", VmSource, "Where ( Cost <> Null ) And ")
		'Trappe d'erreur si pas de créatures dans la sélection
		Try
			VpP = Me.QueryInfo("Sum(Val(Power) * Items) / Sum(Items)", VmSource, "Inner Join Creature On Card.Title = Creature.Title Where ( InStr(Power, '*') = 0 And Power <> '0' ) And ")
			VpT = Me.QueryInfo("Sum(Val(Tough) * Items) / Sum(Items)", VmSource, "Inner Join Creature On Card.Title = Creature.Title Where ( InStr(Tough, '*') = 0 And Tough <> '0' ) And ")
		Catch
		End Try
		Me.txtMaxCost.Text = Me.QueryInfo("Max(Val(myCost))", VmSource).ToString
		Me.txtMeanPrice2.Text = Format(Me.QueryInfo("Avg(Val(Price))", VmSource), "0.00") + " €"
		Me.txtMeanCost.Text = Format(Me.QueryInfo("Sum(Val(myCost) * Items) / Sum(Items)", VmSource), "0.0")
		Me.txtMeanPrice.Text = Format(Me.QueryInfo("Sum(Val(Price) * Items) / Sum(Items)", VmSource), "0.00") + " €"
		Me.txtMinCost.Text = Me.QueryInfo("Min(Val(myCost))", VmSource, "Where Type <> 'L' And ").ToString
		Me.txtMostExpensive.Text = Format(Me.QueryInfo("Max(Val(Price))", VmSource), "0.00") + " €"
		Me.txtNCartes.Text = Me.QueryInfo("Sum(Items)", VmSource).ToString
		Me.txtOldest.Text = Me.QueryInfo("Card.Title", VmSource, , " Order By Release Asc;")
		Me.txtRarest.Text = Me.QueryInfo("Card.Title", VmSource, "Where InStr(UCase(Rarity), 'R') > 0 And " , " Order By Val(Mid(Rarity, 2)) Desc;")
		Me.txtTotPrice.Text = Format(Me.QueryInfo("Sum(Val(Price) * Items)", VmSource), "0.00") + " €"
		Me.txtTougher.Text = Me.QueryInfo("Card.Title", VmSource, "Inner Join Creature On Card.Title = Creature.Title ", " Order By Val(Power) Desc;")
		Me.txtMeanCost2.Text = Format(VpC, "0.0")
		Me.txtMeanPower.Text = Format(VpP, "0.0")
		Me.txtMeanTough.Text = Format(VpT, "0.0")
		Me.txtRAD.Text = Format(VpP / VpT, "0.0")
		Me.txtRAC.Text = Format(VpP / VpC, "0.0")
	End Sub
	Private Function QueryInfo(VpQuery As String, VmSource As String, Optional VpTblCreature As String = "", Optional VpSort As String = "") As Object
	Dim VpSQL As String
		VpSQL = "Select " + VpQuery + " From (((" + VmSource + " Inner Join Card On " + VmSource + ".EncNbr = Card.EncNbr) Inner Join Spell On Card.Title = Spell.Title) Inner Join Series On Card.Series = Series.SeriesCD) " + VpTblCreature
		VpSQL = VpSQL + IIf(VpSQL.EndsWith("And "), "", "Where ")
		VpSQL = VpSQL + VmRestriction
		VgDBCommand.CommandText = clsModule.TrimQuery(VpSQL, False) + VpSort
		Return VgDBCommand.ExecuteScalar
	End Function
	Sub CboCriterionSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
		Call Me.LoadGrid
		Call Me.LoadGraph
		Call Me.LoadInfos
		Me.cmnuCurve.Enabled = ( Me.cboCriterion.ComboBox.SelectedIndex = 3 )
		If Me.cmnuCurve.Enabled = False And Me.cmnuCurve.Checked = True Then
			Me.cmnuCurve.Checked = False
			Me.cmnuBreakDown.Checked = True
		End If
	End Sub
	Sub FrmStatsLoad(ByVal sender As Object, ByVal e As EventArgs)
		Call Me.CboCriterionSelectedIndexChanged(sender, e)	
	End Sub	
	Sub FrmStatsPaint(ByVal sender As Object, ByVal e As PaintEventArgs)
		'Contourne un bug de rafraîchissement sur le graphique
		Me.chartBreakDown.Visible = False
		Me.chartBreakDown.Visible = True
	End Sub	
	Sub FrmStatsActivated(ByVal sender As Object, ByVal e As EventArgs)
		Call Me.FrmStatsPaint(sender, Nothing)
	End Sub
	Sub CmnuBreakDownClick(ByVal sender As Object, ByVal e As EventArgs)
		Me.cmnuBreakDown.Checked = True
		Me.cmnuCurve.Checked = False
		Me.chartBreakDown.Gallery = Gallery.Pie
		Me.chartBreakDown.Chart3D = True
	End Sub
	Sub CmnuCurveClick(ByVal sender As Object, ByVal e As EventArgs)
		Me.cmnuBreakDown.Checked = False
		Me.cmnuCurve.Checked = True
		Me.chartBreakDown.Chart3D = False
		Me.chartBreakDown.Gallery = Gallery.Lines
	End Sub
End Class
Public Class clsNumComparer 
	Implements IComparer
	Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
		Return CInt(x) - CInt(y)
	End Function	
End Class
