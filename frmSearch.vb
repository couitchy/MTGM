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
'| - affichage de la carte numérisée       23/09/2008 |
'| - nouveaux critères de recherches	   15/08/2009 |
'------------------------------------------------------
Imports System.IO
Public Partial Class frmSearch
	Private VmOwner As MainForm				'Formulaire parent
	Private VmFormMove As Boolean = False	'Formulaire en déplacement
	Private VmMousePos As Point				'Position initiale de la souris sur la barre de titre
	Private VmCanClose As Boolean = False   'Formulaire peut être fermé	
	Public Sub New(VpOwner As MainForm)
		Me.InitializeComponent()
		VmOwner = VpOwner
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
	Private Function Search(VpField As String, VpValue As String, VpSource As String, Optional VpIsCreature As Boolean = False, Optional VpEquals As Boolean = False) As String
	'------------------------------------------------------------
	'Effectue la requête de l'utilisateur dans la base de données
	'------------------------------------------------------------
	Dim VpSQL As String
	Dim VpEntry As String
	Dim VpCriteria As String
		'Si on effectue une recherche numérique, demande l'égalité et sinon, la simple présence dans la chaîne
		If VpEquals Then
			VpCriteria = "Val(" + VpField + ") = " + VpValue
		Else
			If Not VpValue.Contains(" ") Then
				VpCriteria = "InStr(" + VpField + ", " + VpValue + ") > 0"			
			Else
				VpCriteria = Me.BuildSplitSearch(VpField, VpValue)
			End If
		End If
		'Recherche restreinte aux cartes présentes dans l'arborescence
		If Me.chkRestriction.Checked Then
			VpSQL = "Select Card.Title, CardFR.TitleFR, Card.EncNbr From (((Card Inner Join CardFR On Card.EncNbr = CardFR.EncNbr) Inner Join Spell On Card.Title = Spell.Title) Inner Join " + VpSource + " On " + VpSource + ".EncNbr = Card.EncNbr) " + IIf(VpIsCreature, "Inner Join Creature On Creature.Title = Card.Title ", "") + "Where " + VpCriteria + " And "
			VpSQL = VpSQL + VmOwner.Restriction
			VpSQL = clsModule.TrimQuery(VpSQL)
		'Recherche étendue (toutes les cartes de la base de données)
		Else		
			VpSQL = "Select Card.Title, CardFR.TitleFR, Card.EncNbr From ((Card Inner Join CardFR On Card.EncNbr = CardFR.EncNbr) Inner Join Spell On Card.Title = Spell.Title) " + IIf(VpIsCreature, "Inner Join Creature On Creature.Title = Card.Title ", "") + "Where " + VpCriteria + ";"
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
		Catch
			Call clsModule.ShowWarning("Une erreur s'est produite lors de la recherche..." + vbCrLf + "Vérifier les informations saisies et recommencer.")
		End Try
		'Retourne le résultat de la requête en tant que sous-sélection (table virtuelle) pour une utilisation ultérieure éventuelle (chargement dans le treeview)
		Return "(" + VpSQL.Substring(0, VpSQL.Length - 1) + ") As " + clsModule.CgSFromSearch
	End Function
	Private Function FindCardNode(VpTitle As String, VpNode As TreeNode) As Boolean
	'---------------------------------------------------------------------
	'Recherche dans les noeuds du treeview la carte spécifiée en paramètre
	'---------------------------------------------------------------------
		If VpNode.Text = VpTitle Or VpNode.Tag = VpTitle Then
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
	#End Region
	#Region " Evènements "
	Private Sub CmdGoClick(sender As System.Object, e As System.EventArgs)
	Dim VpSource As String
	Dim VpSQL As String = ""
	Dim VpReq As String = Me.txtFind.Text.Replace("'", "''")
	Dim VpType As Integer = Me.cboSearchType.SelectedIndex
		Me.lstResult.Items.Clear
		If Me.chkRestriction.Checked And Not VmOwner.IsSourcePresent Then
			Call clsModule.ShowWarning("Aucune source de cartes n'a été sélectionnée...")
			Exit Sub
		End If
		VpSource = IIf(VmOwner.chkClassement.GetItemChecked(0), clsModule.CgSDecks, clsModule.CgSCollection)
		Select Case VpType
			'Recherche type string
			Case 0, 1, 2
				VpSQL = Me.Search(clsModule.CgSearchFields(VpType), "'" + VpReq.Replace(" ", "' '") + "'", VpSource)
			'Recherche type nombre / sur créatures
			Case 3, 4
				VpSQL = Me.Search(clsModule.CgSearchFields(VpType), VpReq, VpSource, True)
			'Recherche type nombre / égalité
			Case 5, 7
				VpSQL = Me.Search(clsModule.CgSearchFields(VpType), VpReq, VpSource, , True)
			'Recherche type string / sur éditions
			Case 6
				VpSQL = Me.Search(clsModule.CgSearchFields(VpType), "'" + clsModule.GetSerieCodeFromName(VpReq) + "'", VpSource)
			Case Else
		End Select
		'Nombre de réponses
		Call clsModule.ShowInformation(Me.lstResult.Items.Count.ToString + " occurence(s) trouvée(s).")
		'Chargement éventuel dans le treeview
		If Me.chkShowExternal.Checked Then
			Call VmOwner.LoadTvw(VpSQL)
		End If
	End Sub		
	Private Sub LstResultDoubleClick(sender As System.Object, e As System.EventArgs)			
	'------------------------------------------------------------------------------------------------------------------------------------
	'Affiche le détail de la carte sélectionnée après la recherche, soit dans l'arborescence principale, soit dans l'onglet des résultats
	'------------------------------------------------------------------------------------------------------------------------------------
	Dim VpTitle As String
	Dim VpSource As String
		If Me.lstResult.SelectedItem Is Nothing Then Exit Sub
		VpTitle = Me.lstResult.SelectedItem.ToString
		VpTitle = VpTitle.Substring(VpTitle.IndexOf("(") + 1)
		VpTitle = VpTitle.Substring(0, VpTitle.Length - 1)
		If Not Me.chkRestriction.Checked Then
			VpSource = ""
		Else
			VpSource = IIf(VmOwner.chkClassement.GetItemChecked(0), clsModule.CgSDecks, clsModule.CgSCollection)
		End If
		Me.lstResult.Tag = VpTitle
		Me.grpSerie.Tag = VpSource
		If Me.chkShowExternal.Checked Then	
			Call Me.FindCardNode(VpTitle, VmOwner.tvwExplore.Nodes.Item(0))
			Me.btResult.Enabled = False
		Else
			Me.SuspendLayout
			Call clsModule.LoadCarac(VmOwner, Me, VpTitle, VpSource)
			Call clsModule.LoadScanCard(VpTitle, Me.picScanCard)
			Me.btResult.Enabled = True
			Call Me.BtResultActivate(sender, e)
			Me.ResumeLayout		
		End If
	End Sub	
	Sub CboEditionSelectedValueChanged(ByVal sender As Object, ByVal e As EventArgs)
		'Astuce un peu crade : nom de la carte sauvé dans lstresult.tag et source dans grpserie.tag
		Me.SuspendLayout
		Call clsModule.LoadCarac(VmOwner, Me, Me.lstResult.Tag, Me.grpSerie.Tag, Me.cboEdition.Text)
		Me.ResumeLayout
	End Sub	
	Private Sub CbarSearchMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
		VmFormMove = True
		VmCanClose = True
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
		If VmCanClose Then
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
	End Sub	
	#End Region	
End Class
