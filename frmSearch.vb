'------------------------------------------------------
'| Projet         |  Magic The Gathering Manager      |
'| Contexte       |         Perso                     |
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
'| Release 12     |                        01/10/2012 |
'| Release 13     |                        09/05/2014 |
'| Release 14     |                        09/05/2015 |
'| Release 15     |                        15/01/2017 |
'| Auteur         |                          Couitchy |
'|----------------------------------------------------|
'| Modifications :                                    |
'| - affichage de la carte numérisée       23/09/2008 |
'| - nouveaux critères de recherche        15/08/2009 |
'| - nouveaux critères de recherche        20/12/2009 |
'| - nouveau critère de recherche          02/09/2010 |
'| - taille dropdownlist pour vue totale   27/04/2012 |
'| - [recents] par ordre décroissant       13/05/2012 |
'| - insensibilité aux signes diacritiques 26/05/2012 |
'------------------------------------------------------
Imports System.IO
Public Partial Class frmSearch
    Private VmOwner As MainForm             'Formulaire parent
    Private VmFormMove As Boolean = False   'Formulaire en déplacement
    Private VmMousePos As Point             'Position initiale de la souris sur la barre de titre
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
                'VpQuery = "InStr(" + VpField + ", '" + VpStr + "') > 0 And " + VpQuery
                VpQuery = VpField + " Like '%" + clsModule.StrDiacriticInsensitize(VpStr) + "%' And " + VpQuery
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
    Dim VpSQL1 As String
    Dim VpSQL2 As String
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
                    'VpCriteria = "InStr(" + VpField + ", '" + VpValue + "') > 0"   'cas simple
                    VpCriteria = VpField + " Like '%" + clsModule.StrDiacriticInsensitize(VpValue) + "%'"
                Else
                    VpCriteria = Me.BuildSplitSearch(VpField, VpValue)              'cas composé
                End If
            Case Else
                VpCriteria = ""
        End Select
        'Recherche restreinte aux cartes possédées
        If Me.chkRestriction.Checked Then
            'Possédées dans la collection
            VpSQL1 = "Select Card.Title, CardFR.TitleFR, Card.EncNbr From (((((Card Inner Join CardFR On Card.EncNbr = CardFR.EncNbr) Inner Join Spell On Card.Title = Spell.Title) Inner Join TextesFR On Card.Title = TextesFR.CardName) Left Join SubTypes On Card.SubType = SubTypes.SubTypeVO) Inner Join MyCollection On MyCollection.EncNbr = Card.EncNbr) " + If(VpIsCreature, "Inner Join Creature On Creature.Title = Card.Title ", "") + "Where " + VpCriteria + " Group By Card.Title, CardFR.TitleFR, Card.EncNbr"
            'Possédées dans les decks
            VpSQL2 = "Select Card.Title, CardFR.TitleFR, Card.EncNbr From (((((Card Inner Join CardFR On Card.EncNbr = CardFR.EncNbr) Inner Join Spell On Card.Title = Spell.Title) Inner Join TextesFR On Card.Title = TextesFR.CardName) Left Join SubTypes On Card.SubType = SubTypes.SubTypeVO) Inner Join MyGames On MyGames.EncNbr = Card.EncNbr) " + If(VpIsCreature, "Inner Join Creature On Creature.Title = Card.Title ", "") + "Where " + VpCriteria + " Group By Card.Title, CardFR.TitleFR, Card.EncNbr"
            If Me.chkRestrictionMyCollection.Checked And Me.chkRestrictionMyGames.Checked Then
                VpSQL = VpSQL1 + " Union " + VpSQL2
            ElseIf Me.chkRestrictionMyCollection.Checked Then
                VpSQL = VpSQL1
            Else
                VpSQL = VpSQL2
            End If
            VpSQL = clsModule.TrimQuery(VpSQL)
        'Recherche restreinte aux cartes non possédées
        ElseIf Me.chkRestrictionInv.Checked Then
            VpSQL = "Select Card.Title, CardFR.TitleFR, Card.EncNbr From ((((Card Inner Join CardFR On Card.EncNbr = CardFR.EncNbr) Inner Join Spell On Card.Title = Spell.Title) Inner Join TextesFR On Card.Title = TextesFR.CardName) Left Join SubTypes On Card.SubType = SubTypes.SubTypeVO) " + If(VpIsCreature, "Inner Join Creature On Creature.Title = Card.Title ", "") + "Where " + VpCriteria
            VpSQL1 = " And Card.EncNbr Not In (Select EncNbr From MyCollection)"
            VpSQL2 = " And Card.EncNbr Not In (Select EncNbr From MyGames)"
            If Me.chkRestrictionMyCollection.Checked And Me.chkRestrictionMyGames.Checked Then
                VpSQL = VpSQL + VpSQL1 + VpSQL2
            ElseIf Me.chkRestrictionMyCollection.Checked Then
                VpSQL = VpSQL + VpSQL1
            Else
                VpSQL = VpSQL + VpSQL2
            End If
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
    Dim VpI As Integer
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
            'Fusion des requêtes en Union
            If Me.optMergeOr.Checked Then
                For Each VpSQLi As String In VmPrevSearchs
                    VpSQLs = VpSQLs + VpSQLi + " Union "
                Next VpSQLi
                Return "(" + VpSQLs.Substring(0, VpSQLs.Length - 7) + ") As " + clsModule.CgSFromSearch
            Else
                'Fusion des requêtes en Intersection
                VpI = 1
                For Each VpSQLi As String In VmPrevSearchs
                    If VpI = 1 Then
                        VpSQLs = "(" + VpSQLi + ") As T1"
                    Else
                        VpSQLs = "(" + VpSQLs + ") Inner Join (" + VpSQLi + ") As T" + VpI.ToString + " On T" + VpI.ToString + ".EncNbr = T1.EncNbr"
                    End If
                    VpI += 1
                Next VpSQLi
                Return "(Select T1.* From " + VpSQLs + ") As " + clsModule.CgSFromSearch
            End If
        End If
    End Function
    #End Region
    #Region "Evènements"
    Private Sub CmdGoClick(sender As System.Object, e As System.EventArgs)
    Dim VpSQL As String = ""
    Dim VpReq As String = Me.cboFind.Text.Replace("'", "''")
    Dim VpType As Integer = Me.cboSearchType.SelectedIndex
        Me.lstResult.Items.Clear
        Me.picScanCard.Image = Image.FromFile(VgOptions.VgSettings.MagicBack)
        'Mémorisation requête
        If Not Me.cboFind.Items.Contains(Me.cboFind.Text) AndAlso Me.cboFind.Text.Trim <> "" Then
            Me.cboFind.Items.Insert(0, Me.cboFind.Text)
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
        Me.lblOccur.Text = Me.lstResult.Items.Count.ToString + " résultat(s) trouvé(s)"
        'Chargement éventuel dans le treeview
        If Me.chkShowExternal.Checked And VpSQL <> "" Then
            With VmOwner
                .mnuDispAdvSearch.Enabled = True
                Call .ManageDispMenu(.mnuDispAdvSearch.Text, False)
                Call .LoadTvw(Me.GetSearchRequests(VpSQL), Me.chkClearPrev.Checked, clsModule.CgFromSearch + " (" + Me.cboFind.Tag +")")
            End With
        End If
        Me.cboFind.Focus
    End Sub
    Private Sub LstResultDoubleClick(sender As System.Object, e As System.EventArgs)
    '------------------------------------------------------------------------------------------------------------------------------------
    'Affiche le détail de la carte sélectionnée après la recherche, soit dans l'arborescence principale, soit dans l'onglet des résultats
    '------------------------------------------------------------------------------------------------------------------------------------
        If Me.lstResult.SelectedItem IsNot Nothing Then
            If Me.chkShowExternal.Checked Then
                Call Me.FindCardNode(clsModule.ExtractENName(Me.lstResult.SelectedItem.ToString), VmOwner.LastRoot)
                'VmOwner.tvwExplore.Focus
            End If
        End If
    End Sub
    Sub LstResultSelectedIndexChanged(sender As Object, e As EventArgs)
        If Me.lstResult.SelectedItem IsNot Nothing AndAlso Me.picScanCard.Visible Then
            Call clsModule.LoadScanCard(clsModule.ExtractENName(Me.lstResult.SelectedItem.ToString), 0, Me.picScanCard)
        End If
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
    Sub ChkRestrictionCheckedChanged(sender As Object, e As EventArgs)
        Me.lstResult.Items.Clear
        Me.picScanCard.Image = Image.FromFile(VgOptions.VgSettings.MagicBack)
        If Me.chkRestriction.Checked Then
            Me.chkRestrictionInv.Checked = False
            Me.chkRestrictionMyCollection.Enabled = True
            Me.chkRestrictionMyGames.Enabled = True
        ElseIf Not Me.chkRestrictionInv.Checked Then
            Me.chkRestrictionMyCollection.Enabled = False
            Me.chkRestrictionMyGames.Enabled = False
        End If
    End Sub
    Sub ChkRestrictionInvCheckedChanged(sender As Object, e As EventArgs)
        Me.lstResult.Items.Clear
        Me.picScanCard.Image = Image.FromFile(VgOptions.VgSettings.MagicBack)
        If Me.chkRestrictionInv.Checked Then
            Me.chkRestriction.Checked = False
            Me.chkRestrictionMyCollection.Enabled = True
            Me.chkRestrictionMyGames.Enabled = True
        ElseIf Not Me.chkRestriction.Checked Then
            Me.chkRestrictionMyCollection.Enabled = False
            Me.chkRestrictionMyGames.Enabled = False
        End If
    End Sub
    Sub ChkRestrictionMyCollectionCheckedChanged(sender As Object, e As EventArgs)
        If Not Me.chkRestrictionMyCollection.Checked Then
            Me.chkRestrictionMyGames.Checked = True
        End If
    End Sub
    Sub ChkRestrictionMyGamesCheckedChanged(sender As Object, e As EventArgs)
        If Not Me.chkRestrictionMyGames.Checked Then
            Me.chkRestrictionMyCollection.Checked = True
        End If
    End Sub
    Sub ChkShowExternalCheckedChanged(sender As Object, e As EventArgs)
        Me.lstResult.Items.Clear
        Me.picScanCard.Image = Image.FromFile(VgOptions.VgSettings.MagicBack)
        Me.chkClearPrev.Enabled = ( Me.chkShowExternal.Checked )
        Me.chkMerge.Enabled = ( Me.chkShowExternal.Checked )
        Me.optMergeOr.Enabled = Me.chkMerge.Enabled And Me.chkMerge.Checked
        Me.optMergeAnd.Enabled = Me.chkMerge.Enabled And Me.chkMerge.Checked
        If Me.chkShowExternal.Checked Then
            Me.picScanCard.Visible = False
            Me.Width = 390 * Me.CreateGraphics().DpiX / 96  'Astuce horrible pour contourner un bug de mise à l'échelle automatique en fonction de la densité de pixels
        Else
            Me.picScanCard.Visible = True
            Me.Width = 618 * Me.CreateGraphics().DpiX / 96  'Astuce horrible pour contourner un bug de mise à l'échelle automatique en fonction de la densité de pixels
        End If
    End Sub
    Sub CmdClearSearchesClick(sender As Object, e As EventArgs)
        Me.cboFind.Items.Clear
    End Sub
    Sub FrmSearchFormClosing(sender As Object, e As FormClosingEventArgs)
    Dim VpSearches As String = ""
        If Me.cboFind.Items.Count > 0 Then
            For Each VpSearch As String In Me.cboFind.Items
                VpSearches = VpSearch + "#" + VpSearches
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
                Me.cboFind.Items.Insert(0, VpSearch)
            End If
        Next VpSearch
        Me.Width = 390 * Me.CreateGraphics().DpiX / 96  'Astuce horrible pour contourner un bug de mise à l'échelle automatique en fonction de la densité de pixels
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
        ElseIf Not Me.chkInf.Checked And Not Me.chkEq.Checked Then
            Me.chkEq.Checked = True
        End If
    End Sub
    Sub ChkInfCheckedChanged(sender As Object, e As EventArgs)
        If Me.chkInf.Checked Then
            Me.chkSup.Checked = False
        ElseIf Not Me.chkSup.Checked And Not Me.chkEq.Checked Then
            Me.chkEq.Checked = True
        End If
    End Sub
    Sub ChkEqCheckedChanged(sender As Object, e As EventArgs)
        If Not Me.chkInf.Checked And Not Me.chkSup.Checked And Not Me.chkEq.Checked Then
            Me.chkEq.Checked = True
        End If
    End Sub
    Sub ChkMergeCheckedChanged(sender As Object, e As EventArgs)
        Me.optMergeOr.Enabled = Me.chkMerge.Checked
        Me.optMergeAnd.Enabled = Me.chkMerge.Checked
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
