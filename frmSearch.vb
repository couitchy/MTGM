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
'| Auteur         |                          Couitchy |
'|----------------------------------------------------|
'| Modifications :                                    |
'| - affichage de la carte numérisée       23/09/2008 |
'| - nouveaux critères de recherches	   15/08/2009 |
'| - nouveaux critères de recherches	   20/12/2009 |
'------------------------------------------------------
Imports System.IO
Public Partial Class frmSearch
	Private VmOwner As MainForm				'Formulaire parent
	Private VmFormMove As Boolean = False	'Formulaire en déplacement
	Private VmMousePos As Point				'Position initiale de la souris sur la barre de titre
	Private VmCanClose As Boolean = False   'Formulaire peut être fermé
	Private VmSource As String
	Private VmRestriction As String
	Private VmPrevSearchs As New ArrayList
	Private VmKeyChange As Boolean = False
	Public Sub New(VpOwner As MainForm)
		Me.InitializeComponent()
		VmSource = If(VpOwner.chkClassement.GetItemChecked(0), clsModule.CgSDecks, clsModule.CgSCollection)
		VmRestriction = VpOwner.Restriction
		VmOwner = VpOwner
		Me.cboSearchType.SelectedIndex = CInt(VgOptions.VgSettings.DefaultSearchCriterion)
	End Sub
	#Region "Méthodes"
	Private Function BuildSplitSearch(VpField As String, VpValue As String) As String
	'-----------------------------------------------
	'Gère le cas d'une recherche avec plusieurs mots
	'-----------------------------------------------
	Dim VpValues() As String
	Dim VpQuery As String = ""
		If VpValue.StartsWith("'""") And VpValue.EndsWith("""'") Then
			Return "InStr(" + VpField + ", " + VpValue.Substring(1, VpValue.Length - 2).Replace("' '", " ") + ") > 0"
		Else
			VpValues = VpValue.Split(" ")
			For Each VpStr As String In VpValues
				VpQuery = "InStr(" + VpField + ", " + VpStr + ") > 0 And " + VpQuery
			Next VpStr
			Return VpQuery.Substring(0, VpQuery.Length - 4)
		End If
	End Function
	Private Function Search(VpField As String, VpValue As String, VpSource As String, Optional VpIsCreature As Boolean = False, Optional VpMode As clsModule.eSearchType = clsModule.eSearchType.Alpha) As String
	'------------------------------------------------------------
	'Effectue la requête de l'utilisateur dans la base de données
	'------------------------------------------------------------
	Dim VpSQL As String
	Dim VpEntry As String
	Dim VpCriteria As String
		Select Case VpMode
			Case clsModule.eSearchType.Num
				VpCriteria = VpField + " = " + VpValue.Replace(",", ".")
			Case clsModule.eSearchType.NumOverAlpha
				VpCriteria = "Val(" + VpField + ") = " + VpValue
			Case clsModule.eSearchType.Alpha
				If Not VpValue.Contains(" ") Then
					VpCriteria = "InStr(" + VpField + ", " + VpValue + ") > 0"
				Else
					VpCriteria = Me.BuildSplitSearch(VpField, VpValue)
				End If
			Case Else
				VpCriteria = ""
		End Select
		'Recherche restreinte aux cartes présentes dans l'arborescence
		If Me.chkRestriction.Checked Then
			VpSQL = "Select Card.Title, CardFR.TitleFR, Card.EncNbr From ((((Card Inner Join CardFR On Card.EncNbr = CardFR.EncNbr) Inner Join Spell On Card.Title = Spell.Title) Inner Join TextesFR On Card.Title = TextesFR.CardName) Inner Join " + VpSource + " On " + VpSource + ".EncNbr = Card.EncNbr) " + If(VpIsCreature, "Inner Join Creature On Creature.Title = Card.Title ", "") + "Where " + VpCriteria + " And "
			VpSQL = VpSQL + VmRestriction
			VpSQL = clsModule.TrimQuery(VpSQL)
		'Recherche étendue (toutes les cartes de la base de données)
		Else
			VpSQL = "Select Card.Title, CardFR.TitleFR, Card.EncNbr From (((Card Inner Join CardFR On Card.EncNbr = CardFR.EncNbr) Inner Join Spell On Card.Title = Spell.Title) Inner Join TextesFR On Card.Title = TextesFR.CardName) " + If(VpIsCreature, "Inner Join Creature On Creature.Title = Card.Title ", "") + "Where " + VpCriteria + ";"
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
	#Region " Evènements "
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
			Case 0, 1, 2, 3, 9
				VpSQL = Me.Search(clsModule.CgSearchFields(VpType), "'" + VpReq.Replace(" ", "' '") + "'", VmSource)
			'Recherche type nombre / sur créatures
			Case 4, 5
				VpSQL = Me.Search(clsModule.CgSearchFields(VpType), VpReq, VmSource, True, clsModule.eSearchType.NumOverAlpha)
			'Recherche type nombre / égalité
			Case 6, 8
				VpSQL = Me.Search(clsModule.CgSearchFields(VpType), VpReq, VmSource, , clsModule.eSearchType.Num)
			'Recherche type string / sur éditions
			Case 7
				VpSQL = Me.Search(clsModule.CgSearchFields(VpType), "'" + clsModule.GetSerieCodeFromName(VpReq, True) + "'", VmSource)
			Case Else
		End Select
		'Nombre de réponses
		Me.lblOccur.Text = Me.lstResult.Items.Count.ToString + " occurence(s) trouvée(s)"
		'Chargement éventuel dans le treeview
		If Me.chkShowExternal.Checked Then
			Call VmOwner.LoadTvw(Me.GetSearchRequests(VpSQL), Me.chkClearPrev.Checked, clsModule.CgFromSearch + " (" + Me.cboFind.Tag +")")
		End If
	End Sub
	Private Function GetLastSearchNode As TreeNode
	'----------------------------------------------------------------------------
	'Retourne le noeud correspondant à la dernière recherche (ie. celle en cours)
	'----------------------------------------------------------------------------
	Dim VpNode As TreeNode = VmOwner.tvwExplore.Nodes.Item(0)
		While Not VpNode.NextNode Is Nothing
			VpNode = VpNode.NextNode
		End While
		Return VpNode
	End Function
	Private Sub LstResultDoubleClick(sender As System.Object, e As System.EventArgs)
	'------------------------------------------------------------------------------------------------------------------------------------
	'Affiche le détail de la carte sélectionnée après la recherche, soit dans l'arborescence principale, soit dans l'onglet des résultats
	'------------------------------------------------------------------------------------------------------------------------------------
	Dim VpTitle As String
	Dim VpSource As String
		If Me.lstResult.SelectedItem Is Nothing Then Exit Sub
		VpTitle = clsModule.ExtractENName(Me.lstResult.SelectedItem.ToString)
		If Not Me.chkRestriction.Checked Then
			VpSource = ""
		Else
			VpSource = If(VmOwner.chkClassement.GetItemChecked(0), clsModule.CgSDecks, clsModule.CgSCollection)
		End If
		Me.lstResult.Tag = VpTitle
		Me.grpSerie.Tag = VpSource
		If Me.chkShowExternal.Checked Then
			Call Me.FindCardNode(VpTitle, Me.GetLastSearchNode)
			Me.btResult.Enabled = False
			'VmOwner.tvwExplore.Focus
		Else
			Me.SuspendLayout
			Call clsModule.LoadCarac(VmOwner, Me, VpTitle, False, VpSource)
			Call clsModule.LoadScanCard(VpTitle, Me.picScanCard)
			Me.btResult.Enabled = True
			Call Me.BtResultActivate(sender, e)
			Me.ResumeLayout
		End If
	End Sub
	Sub CboEditionSelectedValueChanged(ByVal sender As Object, ByVal e As EventArgs)
		'Astuce un peu crade : nom de la carte sauvé dans lstresult.tag et source dans grpserie.tag
		Me.SuspendLayout
		Call clsModule.LoadCarac(VmOwner, Me, Me.lstResult.Tag, False, Me.grpSerie.Tag, clsModule.GetSerieCodeFromName(Me.cboEdition.Text))
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
		Me.grpSerie.Visible = False
	End Sub
	Sub BtResultActivate(ByVal sender As Object, ByVal e As EventArgs)
		Me.grpSerie.Visible = True
		Me.grpSearch.Visible = False
	End Sub
	Sub ChkRestrictionCheckedChanged(sender As Object, e As EventArgs)
		Me.lstResult.Items.Clear
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
	#End Region
End Class