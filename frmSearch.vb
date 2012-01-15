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
'| - affichage de la carte numérisée       23/09/2008 |
'| - nouveaux critères de recherche	   	   15/08/2009 |
'| - nouveaux critères de recherche		   20/12/2009 |
'| - nouveau critère de recherche		   02/09/2010 |
'------------------------------------------------------
Imports System.IO
Public Partial Class frmSearch
	Private VmOwner As MainForm				'Formulaire parent
	Private VmFormMove As Boolean = False	'Formulaire en déplacement
	Private VmMousePos As Point				'Position initiale de la souris sur la barre de titre
	Private VmCanClose As Boolean = False   'Formulaire peut être fermé
	Private VmPrevSearchs As New List(Of String)
	Private VmKeyChange As Boolean = False
	Public Sub New(VpOwner As MainForm)
		Me.InitializeComponent()
		VmOwner = VpOwner
		Me.cboSearchType.SelectedIndex = CInt(VgOptions.VgSettings.DefaultSearchCriterion)
	End Sub
	#Region "Méthodes"
	Private Function BuildSplitSearch(VpField As String, VpValue As String) As String
	'-----------------------------------------------
	'Gère le cas d'une recherche avec plusieurs mots
	'-----------------------------------------------
	Dim VpValues() As String
	Dim VpBlocs As New List(Of String)
	Dim VpQuery As String = ""
	Dim VpStart As Boolean = False
	Dim VpIndex As Integer
		'Gestion recherche par morceaux (avec guillemets)
		If VpValue.Contains("""") Then
			For VpI As Integer = 0 To VpValue.Length - 1
				If VpValue.Substring(VpI, 1) = """" Then
					If VpStart Then
						VpBlocs.Add(VpValue.Substring(VpIndex, VpI - VpIndex))
					Else
						VpIndex = VpI + 1
					End If
					VpStart = Not VpStart
				End If
			Next VpI
			For Each VpBloc As String In VpBlocs
				VpValue = VpValue.Replace("""" + VpBloc + """", "")
			Next VpBloc
			VpValue = VpValue.Trim
		End If
		VpValues = VpValue.Split(" ")
		'Mots indépendants
		Call Me.ConcatSearch(VpField, VpQuery, VpValues)
		'Morceaux
		Call Me.ConcatSearch(VpField, VpQuery, VpBlocs)
		'Troncature finale
		If VpQuery.Length >= 4 Then
			Return VpQuery.Substring(0, VpQuery.Length - 4)
		Else
			Return ""
		End If
	End Function
	Private Sub ConcatSearch(VpField As String, ByRef VpQuery As String, VpValues As Object)
		For Each VpStr As String In VpValues
			If VpStr.Trim <> "" Then
				VpQuery = "InStr(" + VpField + ", '" + VpStr + "') > 0 And " + VpQuery
			End If
		Next VpStr
	End Sub
	Private Function FindNumOperator As String
		If Me.chkInf.Checked And Me.chkEq.Checked Then
			Return " <= "
		ElseIf Me.chkSup.Checked And Me.chkEq.Checked Then
			Return " >= "
		ElseIf Me.chkSup.Checked Then
			Return " > "
		ElseIf Me.chkInf.Checked Then
			Return " < "
		Else
			Return " = "
		End If
	End Function
	Private Function Search(VpField As String, VpValue As String, Optional VpIsCreature As Boolean = False, Optional VpMode As clsModule.eSearchType = clsModule.eSearchType.Alpha) As String
	'------------------------------------------------------------
	'Effectue la requête de l'utilisateur dans la base de données
	'------------------------------------------------------------
	Dim VpSQL As String
	Dim VpEntry As String
	Dim VpCriteria As String
		'Gestion des différents modes de recherche
		Select Case VpMode
			Case clsModule.eSearchType.Num
				VpCriteria = VpField + Me.FindNumOperator + VpValue.Replace(",", ".")
			Case clsModule.eSearchType.NumOverAlpha
				VpCriteria = "Val(" + VpField + ")" + Me.FindNumOperator + VpValue
			Case clsModule.eSearchType.Alpha
				If Not VpValue.Contains(" ") And Not VpValue.Contains("""") Then
					VpCriteria = "InStr(" + VpField + ", '" + VpValue + "') > 0"	'cas simple
				Else
					VpCriteria = Me.BuildSplitSearch(VpField, VpValue)				'cas composé
				End If
			Case Else
				VpCriteria = ""
		End Select
		'Recherche restreinte aux cartes possédées
		If Me.chkRestriction.Checked Then
			'Possédées dans la collection
			VpSQL = "Select Card.Title, CardFR.TitleFR, Card.EncNbr From ((((Card Inner Join CardFR On Card.EncNbr = CardFR.EncNbr) Inner Join Spell On Card.Title = Spell.Title) Inner Join TextesFR On Card.Title = TextesFR.CardName) Inner Join MyCollection On MyCollection.EncNbr = Card.EncNbr) " + If(VpIsCreature, "Inner Join Creature On Creature.Title = Card.Title ", "") + "Where " + VpCriteria + " Union "
			'Possédées dans les decks
			VpSQL = VpSQL + "Select Card.Title, CardFR.TitleFR, Card.EncNbr From ((((Card Inner Join CardFR On Card.EncNbr = CardFR.EncNbr) Inner Join Spell On Card.Title = Spell.Title) Inner Join TextesFR On Card.Title = TextesFR.CardName) Inner Join MyGames On MyGames.EncNbr = Card.EncNbr) " + If(VpIsCreature, "Inner Join Creature On Creature.Title = Card.Title ", "") + "Where " + VpCriteria
			VpSQL = clsModule.TrimQuery(VpSQL)
		'Recherche restreinte aux cartes non possédées
		ElseIf Me.chkRestrictionInv.Checked Then
			VpSQL = "Select Card.Title, CardFR.TitleFR, Card.EncNbr From (((Card Inner Join CardFR On Card.EncNbr = CardFR.EncNbr) Inner Join Spell On Card.Title = Spell.Title) Inner Join TextesFR On Card.Title = TextesFR.CardName) " + If(VpIsCreature, "Inner Join Creature On Creature.Title = Card.Title ", "") + "Where " + VpCriteria + " And Card.EncNbr Not In (Select EncNbr From MyCollection) And Card.EncNbr Not In (Select EncNbr From MyGames)"
			VpSQL = clsModule.TrimQuery(VpSQL)
		'Recherche étendue (toutes les cartes de la base de données)
		Else
			VpSQL = "Select Card.Title, CardFR.TitleFR, Card.EncNbr From ((((Card Inner Join CardFR On Card.EncNbr = CardFR.EncNbr) Inner Join Spell On Card.Title = Spell.Title) Inner Join TextesFR On Card.Title = TextesFR.CardName) Left Join SubTypes On Card.SubType = SubTypes.SubTypeVO) " + If(VpIsCreature, "Inner Join Creature On Creature.Title = Card.Title ", "") + "Where " + VpCriteria + ";"
		End If
		Try
			'Requête effective
			VgDBCommand.CommandText = VpSQL
			VgDBReader = VgDBCommand.ExecuteReader
			With VgDBReader
				'Parcourt la liste des résultats en évitant les doublons
				While .Read
					VpEntry = .GetString(1) + " (" + .GetString(0) + ")"
					If Not Me.lstResult.Items.Contains(VpEntry) Then
						Me.lstResult.Items.Add(VpEntry)
					End If
				End While
				.Close
			End With
			'Retourne le résultat de la requête en tant que sous-sélection (table virtuelle) pour une utilisation ultérieure éventuelle (chargement dans le treeview)
			Return VpSQL.Substring(0, VpSQL.Length - 1)
		Catch
			Call clsModule.ShowWarning("Une erreur s'est produite lors de la recherche..." + vbCrLf + "Vérifier les informations saisies et recommencer.")
		End Try
		Return ""
	End Function
	Private Function FindCardNode(VpTitle As String, VpNode As TreeNode) As Boolean
	'---------------------------------------------------------------------
	'Recherche dans les noeuds du treeview la carte spécifiée en paramètre
	'---------------------------------------------------------------------
		If VpNode.Tag.Value = VpTitle Then
			VpNode.TreeView.SelectedNode = VpNode
			VpNode.EnsureVisible
			Return True
		Else
			For Each VpChild As TreeNode In VpNode.Nodes
				If Me.FindCardNode(VpTitle, VpChild) Then
					Return True
				End If
			Next VpChild
		End If
		Return False
	End Function
	Private Function GetSearchRequests(VpSQL As String) As String
	'----------------------------------------------------------------------------------
	'Retourne la requête globale (simple ou fusionnée) associée à la recherche courante
	'----------------------------------------------------------------------------------
	Dim VpSQLs As String = ""
		If Not Me.chkMerge.Checked Then
			VmPrevSearchs.Clear
		End If
		If VpSQL <> "" Then
			VmPrevSearchs.Add(VpSQL)
		End If
		If VmPrevSearchs.Count = 0 Then
			Return ""
		ElseIf VmPrevSearchs.Count = 1 Then
			Me.cboFind.Tag = Me.cboFind.Text
			Return "(" + VpSQL + ") As " + clsModule.CgSFromSearch
		Else
			Me.cboFind.Tag = Me.cboFind.Tag + ", " + Me.cboFind.Text
			For Each VpSQLi As String In VmPrevSearchs
				VpSQLs = VpSQLs + VpSQLi + " Union "
			Next VpSQLi
			Return "(" + VpSQLs.Substring(0, VpSQLs.Length - 7) + ") As " + clsModule.CgSFromSearch
		End If
	End Function
	#End Region
	#Region "Evènements"
	Private Sub CmdGoClick(sender As System.Object, e As System.EventArgs)
	Dim VpSQL As String = ""
	Dim VpReq As String = Me.cboFind.Text.Replace("'", "''")
	Dim VpType As Integer = Me.cboSearchType.SelectedIndex
		Me.lstResult.Items.Clear
		If Me.chkRestriction.Checked And Not VmOwner.IsSourcePresent Then
			Call clsModule.ShowWarning("Aucune source de cartes n'a été sélectionnée...")
			Exit Sub
		End If
		'Mémorisation requête
		If Not Me.cboFind.Items.Contains(Me.cboFind.Text) AndAlso Me.cboFind.Text.Trim <> "" Then
			Me.cboFind.Items.Add(Me.cboFind.Text)
		End If
		Select Case VpType
			'Recherche type string simple
			Case 0, 1, 2, 3, 10, 11
				VpSQL = Me.Search(clsModule.CgSearchFields(VpType), VpReq)
			'Recherche type nombre / sur créatures
			Case 4, 5
				VpSQL = Me.Search(clsModule.CgSearchFields(VpType), VpReq, True, clsModule.eSearchType.NumOverAlpha)
			'Recherche type nombre / égalité
			Case 6, 9
				VpSQL = Me.Search(clsModule.CgSearchFields(VpType), VpReq, , clsModule.eSearchType.Num)
			'Recherche type string / sur éditions VO
			Case 7
				VpSQL = Me.Search(clsModule.CgSearchFields(VpType), clsModule.GetSerieCodeFromName(Me.cboFind.Text, True))
			'Recherche type string / sur éditions VF
			Case 8
				VpSQL = Me.Search(clsModule.CgSearchFields(VpType), clsModule.GetSerieCodeFromName(Me.cboFind.Text, True, True))
			Case Else
		End Select
		'Nombre de réponses
		Me.lblOccur.Text = Me.lstResult.Items.Count.ToString + " occurence(s) trouvée(s)"
		'Chargement éventuel dans le treeview
		If Me.chkShowExternal.Checked Then
			Call VmOwner.LoadTvw(Me.GetSearchRequests(VpSQL), Me.chkClearPrev.Checked, clsModule.CgFromSearch + " (" + Me.cboFind.Tag +")")
		End If
		Me.cboFind.Focus
	End Sub
	Private Sub LstResultDoubleClick(sender As System.Object, e As System.EventArgs)
	'------------------------------------------------------------------------------------------------------------------------------------
	'Affiche le détail de la carte sélectionnée après la recherche, soit dans l'arborescence principale, soit dans l'onglet des résultats
	'------------------------------------------------------------------------------------------------------------------------------------
	Dim VpTitle As String
		If Me.lstResult.SelectedItem Is Nothing Then Exit Sub
		VpTitle = clsModule.ExtractENName(Me.lstResult.SelectedItem.ToString)
		If Me.chkShowExternal.Checked Then
			Call Me.FindCardNode(VpTitle, VmOwner.LastRoot)
			Me.btResult.Enabled = False
			'VmOwner.tvwExplore.Focus
		Else
			Me.SuspendLayout
			Call clsModule.LoadCarac(VmOwner, Me, VpTitle, False, False)
			Call clsModule.LoadScanCard(VpTitle, Me.picScanCard)
			Me.btResult.Enabled = True
			Call Me.BtResultActivate(sender, e)
			Me.ResumeLayout
		End If
	End Sub
	Sub CboEditionSelectedValueChanged(ByVal sender As Object, ByVal e As EventArgs)
		Me.SuspendLayout
		Call clsModule.LoadCarac(VmOwner, Me, clsModule.ExtractENName(Me.lstResult.SelectedItem.ToString), False, False, , clsModule.GetSerieCodeFromName(Me.cboEdition.Text))
		Me.ResumeLayout
	End Sub
	Private Sub CbarSearchMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
		VmFormMove = True
		VmMousePos = New Point(e.X, e.Y)
	End Sub
	Private Sub CbarSearchMouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
		If VmFormMove Then
			Me.Location = New Point(MousePosition.X - VmMousePos.X, MousePosition.Y - VmMousePos.Y)
		End If
	End Sub
	Private Sub CbarSearchMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs)
		VmFormMove = False
	End Sub
	Private Sub CbarSearchVisibleChanged(ByVal sender As Object, ByVal e As EventArgs)
		If VmCanClose AndAlso Not Me.cbarSearch.Visible Then
			Me.Close
		End If
	End Sub
	Sub BtSearchActivate(ByVal sender As Object, ByVal e As EventArgs)
		Me.grpSearch.Visible = True
		'Me.btResult.Checked = False
		Me.grpSerie.Visible = False
		'Me.btSearch.Checked = True
		Me.cboFind.Focus
	End Sub
	Sub BtResultActivate(ByVal sender As Object, ByVal e As EventArgs)
		Me.grpSerie.Visible = True
		'Me.btSearch.Checked = False
		Me.grpSearch.Visible = False
		'Me.btResult.Checked = True
	End Sub
	Sub ChkRestrictionCheckedChanged(sender As Object, e As EventArgs)
		Me.lstResult.Items.Clear
		If Me.chkRestriction.Checked Then
			Me.chkRestrictionInv.Checked = False
		End If
	End Sub
	Sub ChkRestrictionInvCheckedChanged(sender As Object, e As EventArgs)
		Me.lstResult.Items.Clear
		If Me.chkRestrictionInv.Checked Then
			Me.chkRestriction.Checked = False
		End If
	End Sub
	Sub ChkShowExternalCheckedChanged(sender As Object, e As EventArgs)
		Me.lstResult.Items.Clear
		Me.chkClearPrev.Enabled = ( Me.chkShowExternal.Checked )
		Me.chkMerge.Enabled = ( Me.chkShowExternal.Checked )
	End Sub
	Sub CmdClearSearchesClick(sender As Object, e As EventArgs)
		Me.cboFind.Items.Clear
	End Sub
	Sub FrmSearchFormClosing(sender As Object, e As FormClosingEventArgs)
	Dim VpSearches As String = ""
		If Me.cboFind.Items.Count > 0 Then
			For Each VpSearch As String In Me.cboFind.Items
				VpSearches = VpSearches + VpSearch + "#"
			Next VpSearch
			VpSearches = VpSearches.Substring(0, VpSearches.Length - 1)
		End If
		VgOptions.VgSettings.PrevSearches = VpSearches
		Call VgOptions.SaveSettings
	End Sub
	Sub FrmSearchLoad(sender As Object, e As EventArgs)
		VmCanClose = True
		For Each VpSearch As String In VgOptions.VgSettings.PrevSearches.Split("#")
			If VpSearch.Trim <> "" Then
				Me.cboFind.Items.Add(VpSearch)
			End If
		Next VpSearch
	End Sub
	Function GetRefText(sender As Object) As String
		If sender.SelectionLength > 0 Then
			Return sender.Text.Replace(sender.SelectedText, "").ToLower
		Else
			Return sender.Text.ToLower
		End If
	End Function
	Sub CboFindKeyDown(sender As Object, e As KeyEventArgs)
	Dim VpRef As String = Me.GetRefText(sender)
		If e.KeyCode = Keys.Back And VpRef = "æ" Then
			sender.Text = "Ae "
			sender.SelectionStart = 3
			VmKeyChange = True
		End If
	End Sub
	Sub CboFindKeyUp(sender As Object, e As KeyEventArgs)
	Dim VpRef As String = Me.GetRefText(sender)
		If VpRef = "ae" And Not VmKeyChange Then
			sender.Text = "Æ"
			sender.SelectionStart = 1
		End If
		VmKeyChange = False
	End Sub
	Sub ChkSupCheckedChanged(sender As Object, e As EventArgs)
		If Me.chkSup.Checked Then
			Me.chkInf.Checked = False
		End If
	End Sub
	Sub ChkInfCheckedChanged(sender As Object, e As EventArgs)
		If Me.chkInf.Checked Then
			Me.chkSup.Checked = False
		End If
	End Sub
	Sub CboSearchTypeSelectedIndexChanged(sender As Object, e As EventArgs)
		Select Case Me.cboSearchType.SelectedIndex
			Case 4, 5, 6, 9
				Me.chkInf.Visible = True
				Me.chkEq.Visible = True
				Me.chkSup.Visible = True
			Case Else
				Me.chkInf.Visible = False
				Me.chkEq.Visible = False
				Me.chkSup.Visible = False
		End Select
	End Sub
	#End Region
End Class