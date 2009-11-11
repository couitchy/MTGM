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
'| - gestion des versions de jeux		   13/10/2008 |
'| - gestion de parties 'hors total'	   10/09/2009 |
'------------------------------------------------------
Imports System.Data
Imports System.Data.OleDb
Imports SoftwareFX.ChartFX.Lite
Public Partial Class frmPerfs
	Private VmOwner As MainForm
	#Region "Méthodes"
	Public Sub New(VpOwner As MainForm)
	'------------
	'Constructeur
	'------------	
		Me.InitializeComponent()
		VmOwner = VpOwner
		For VpI As Integer = 1 To VgOptions.GetDeckCount
			Me.dropAddLocal.DropDownItems.Add(VgOptions.GetDeckName(VpI), Nothing, AddressOf AddLocalGameClick)
		Next VpI
		Call AddKnownGames("JeuLocal", Me.cboJeuLocal)
		Call AddKnownGames("JeuAdverse", Me.cboJeuAdv)
		AddHandler Me.cboJeuLocal.ComboBox.SelectedIndexChanged, AddressOf CboJeuLocalSelectedIndexChanged
		AddHandler Me.cboJeuAdv.ComboBox.SelectedIndexChanged, AddressOf CboJeuAdvSelectedIndexChanged
		AddHandler Me.cboLocalVersion.ComboBox.SelectedIndexChanged, AddressOf CboLocalVersionSelectedIndexChanged
		AddHandler Me.cboAdvVersion.ComboBox.SelectedIndexChanged, AddressOf CboAdvVersionSelectedIndexChanged
		Me.cboLocalVersion.ComboBox.BackColor = Color.LightBlue
		Me.cboAdvVersion.ComboBox.BackColor = Color.LightBlue
		Me.cboJeuLocal.Items.Add(clsModule.CgPerfsTotal)
		Me.cboJeuAdv.Items.Add(clsModule.CgPerfsTotal)
		Me.cboLocalVersion.Items.Add(clsModule.CgPerfsVersion)
		Me.cboAdvVersion.Items.Add(clsModule.CgPerfsVersion)
		Me.cboLocalVersion.Items.Add(clsModule.CgPerfsTotalV)
		Me.cboAdvVersion.Items.Add(clsModule.CgPerfsTotalV)		
	End Sub	
	Private Sub MyRefresh
	'---------------------------------------------------------------------------
	'Mise à jour du graphique lorsque les deux items des comboboxes sont valides
	'---------------------------------------------------------------------------	
		If Me.cboJeuLocal.Items.Contains(Me.cboJeuLocal.ComboBox.Text) And Me.cboJeuAdv.Items.Contains(Me.cboJeuAdv.ComboBox.Text) Then	
			Call Me.UpdateGraph
		End If
	End Sub
	Private Sub LoadVersions(VpCbo1 As TD.SandBar.ComboBoxItem, VpCbo2 As TD.SandBar.ComboBoxItem, VpField1 As String, VpField2 As String)
	'---------------------------------------------------------------------------------
	'Ajoute au combobox les versions disponibles pour le jeu venant d'être sélectionné
	'---------------------------------------------------------------------------------	
	Dim VpSQL As String
		VpCbo2.ComboBox.Text = clsModule.CgPerfsTotalV
		VpCbo2.Items.Clear
		VpCbo2.Items.Add(clsModule.CgPerfsVersion)
		VpCbo2.Items.Add(clsModule.CgPerfsTotalV)
		VpSQL = "Select " + VpField2 + " From MyScores Where " + VpField1 + " = '" + VpCbo1.ComboBox.Text.Replace("'", "''") + "';"
		VgDBCommand.CommandText = VpSQL
		VgDBReader = VgDBCommand.ExecuteReader
		With VgDBReader
			While .Read
				If Not .GetValue(0) Is DBNull.Value AndAlso Not VpCbo2.Items.Contains(.GetString(0)) Then
					VpCbo2.Items.Add(.GetString(0))
				End If
			End While
			.Close
		End With
	End Sub
	Private Function ValidateVersion(VpCbo As TD.SandBar.ComboBoxItem) As String
	'--------------------------------------------------------------------------
	'Vérifie que la version (ie. la date) sélectionnée pour le jeu est correcte
	'--------------------------------------------------------------------------
		If VpCbo.Items.Contains(VpCbo.ComboBox.Text) And VpCbo.ComboBox.Text <> clsModule.CgPerfsVersion And VpCbo.ComboBox.Text <> clsModule.CgPerfsTotalV Then
			Return "'" + VpCbo.ComboBox.Text + "'"
		Else
			Return "Null"
		End If
	End Function	
	Private Sub UpdateGraph
	'----------------------------------------------------------------------------------
	'Affiche le diagramme de répartitions des victoires sur les deux jeux sélectionnés
	'ou le diagramme d'évolution suivant les versions (selon l'état du menu contextuel)
	'----------------------------------------------------------------------------------
	Dim VpJeuLocal As String = Me.cboJeuLocal.ComboBox.Text
	Dim VpJeuAdv As String = Me.cboJeuAdv.ComboBox.Text
	Dim VpVicLocal As Integer = 0
	Dim VpVicAdv As Integer = 0
	Dim VpGameCounter As New clsGameCounter						'Matrice contenant {version locale, version adverse, nombre victoires locales, nombre victoires adverses)
	Dim VpDistinctAdverseVersions() As Date
	Dim VpDistinctLocalVersions() As Date
	Dim VpSQL As String
	Dim VpHandleGamesVersions As Boolean = True
		'Performances totales (ne tient pas compte des versions des jeux) ou sur jeu unique (en tient compte ou pas)
		If VpJeuLocal = clsModule.CgPerfsTotal Then
			VpJeuLocal = clsModule.CgPerfsLocal
			VpHandleGamesVersions = False
		End If
		If VpJeuAdv = clsModule.CgPerfsTotal Then
			VpJeuAdv = clsModule.CgPerfsAdv
			VpHandleGamesVersions = False
		End If
		VpHandleGamesVersions = VpHandleGamesVersions And Me.dropGamesVersions.Checked
		'Si on ne tient pas compte des versions des jeux, le diagramme est bipolaire
		If Not VpHandleGamesVersions Then
			'Si on s'intéresse de part ou d'autre à un TOTAL, il ne faut pas prendre en compte les parties indépendantes
			If Me.cboJeuLocal.ComboBox.Text = clsModule.CgPerfsTotal Or Me.cboJeuAdv.ComboBox.Text = clsModule.CgPerfsTotal Then
				VpSQL = "Select Victoire From MyScores Where IsMixte = False" + IIf(VpJeuLocal <> clsModule.CgPerfsLocal, " And JeuLocal = '" + VpJeuLocal.Replace("'", "''") + "'", "") + IIf(VpJeuAdv <> clsModule.CgPerfsAdv, " And JeuAdverse = '" + VpJeuAdv.Replace("'", "''") + "'", "")
			Else
				VpSQL = "Select Victoire From MyScores Where (IsMixte = False Or IsMixte = True)" + IIf(VpJeuLocal <> clsModule.CgPerfsLocal, " And JeuLocal = '" + VpJeuLocal.Replace("'", "''") + "'", "") + IIf(VpJeuAdv <> clsModule.CgPerfsAdv, " And JeuAdverse = '" + VpJeuAdv.Replace("'", "''") + "'", "") 'le (IsMixte = False Or IsMixte = True) est un peu crade mais permet d'éviter une mise en forme fastidieuse
			End If
			VgDBCommand.CommandText = VpSQL
			VgDBReader = VgDBCommand.ExecuteReader
			With VgDBReader
				'Comptage victoires / défaites
				While .Read
					If .GetBoolean(0) Then
						VpVicLocal = VpVicLocal + 1
					Else
						VpVicAdv = VpVicAdv + 1
					End If
				End While
				.Close
			End With
			'Diagramme de répartition
			With Me.chartBreakDown
				.Visible = False			
				.ClearData(ClearDataFlag.Data)
				.SerLeg.Clear
				.OpenData(COD.Values, 1, 2)
				.Value(0, 0) = VpVicLocal
				.Value(0, 1) = VpVicAdv
				'Inclusion des chiffres explicites dans la légende
				.SerLeg(0) = "Victoires " + VpJeuLocal + " (" + VpVicLocal.ToString + "/" + (VpVicLocal + VpVicAdv).ToString + ")"
				.SerLeg(1) = "Victoires " + VpJeuAdv + " (" + VpVicAdv.ToString + "/" + (VpVicLocal + VpVicAdv).ToString + ")"
				.SerLegBox = True
				.Chart3D = True
				.Gallery = Gallery.Pie
				.CloseData(COD.Values)
				.Visible = True			
			End With	
		'Si on tient compte des versions des jeux :
		'- en mode diagramme, il y a autant de portions que de versions de jeux confrontées
		'- en mode évolution, il y a autant de points que de versions de jeux confrontées
		Else
			'Mode diagramme
			If Me.mnuBreakDown.Checked Then
				VpSQL = "Select Victoire, JeuLocalVersion, JeuAdverseVersion From MyScores Where JeuLocal = '" + VpJeuLocal.Replace("'", "''") + "' And JeuAdverse = '" + VpJeuAdv.Replace("'", "''") + "' "
				'Cas 1 : toutes les versions du jeu local contre 1 version du jeu adverse
				If Me.cboLocalVersion.ComboBox.Text = clsModule.CgPerfsTotalV And Me.cboAdvVersion.ComboBox.Text <> clsModule.CgPerfsTotalV Then
					VpSQL = VpSQL + "And JeuAdverseVersion = '" + Me.cboAdvVersion.ComboBox.Text + "';"
				'Cas 2 : 1 version du jeu local contre toutes les versions du jeu adverse
				ElseIf Me.cboLocalVersion.ComboBox.Text <> clsModule.CgPerfsTotalV And Me.cboAdvVersion.ComboBox.Text = clsModule.CgPerfsTotalV Then
					VpSQL = VpSQL + "And JeuLocalVersion = '" + Me.cboLocalVersion.ComboBox.Text + "';"
				'Cas 3 : toutes les versions du jeu local contre toutes les versions du jeu adverse
				ElseIf Me.cboLocalVersion.ComboBox.Text = clsModule.CgPerfsTotalV And Me.cboAdvVersion.ComboBox.Text = clsModule.CgPerfsTotalV Then
					VpSQL = VpSQL + ";"
				'Cas 4 : 1 version du jeu local contre 1 version du jeu adverse
				ElseIf Me.cboLocalVersion.ComboBox.Text <> clsModule.CgPerfsTotalV And Me.cboAdvVersion.ComboBox.Text <> clsModule.CgPerfsTotalV Then
					VpSQL = VpSQL + "And JeuLocalVersion = '" + Me.cboLocalVersion.ComboBox.Text + "' And JeuAdverseVersion = '" + Me.cboAdvVersion.ComboBox.Text + "';"				
				End If
				VgDBCommand.CommandText = VpSQL
				VgDBReader = VgDBCommand.ExecuteReader
				With VgDBReader
					While .Read
						VpGameCounter.AddGame(clsModule.MyCDate(.GetValue(1).ToString), clsModule.MyCDate(.GetValue(2).ToString), .GetBoolean(0))
					End While
					.Close
				End With
				VpDistinctLocalVersions = VpGameCounter.GetDistinctLocalVersions
				VpDistinctAdverseVersions = VpGameCounter.GetDistinctAdverseVersions
				'Diagramme de répartition
				With Me.chartBreakDown
					.Visible = False			
					.ClearData(ClearDataFlag.Data)
					.SerLeg.Clear
					.OpenData(COD.Values, 1, VpDistinctLocalVersions.Length + VpDistinctAdverseVersions.Length)
					'Ajoute des victoires locales
					For VpI As Integer = 0 To VpDistinctLocalVersions.Length - 1
						.Value(0, VpI) = VpGameCounter.GetNVicLocal(VpDistinctLocalVersions(VpI))
						.SerLeg(VpI) = "Victoires " + VpJeuLocal + " (" + clsModule.MyShortDateString(VpDistinctLocalVersions(VpI)) + ") : " + .Value(0, VpI).ToString
					Next VpI
					'Ajout des victoires adverses
					For VpI As Integer = 0 To VpDistinctAdverseVersions.Length - 1
						.Value(0, VpI + VpDistinctLocalVersions.Length) = VpGameCounter.GetNVicAdverse(VpDistinctAdverseVersions(VpI))
						.SerLeg(VpI + VpDistinctLocalVersions.Length) = "Victoires " + VpJeuAdv + " (" + clsModule.MyShortDateString(VpDistinctAdverseVersions(VpI)) + ") : " + .Value(0, VpI + VpDistinctLocalVersions.Length).ToString
					Next VpI					
					.SerLegBox = True
					.Chart3D = True
					.Gallery = Gallery.Pie
					.CloseData(COD.Values)
					.Visible = True			
				End With					
			'Mode évolution
			Else
				VgDBCommand.CommandText = "Select Victoire, JeuLocalVersion, JeuAdverseVersion From MyScores Where JeuLocal = '" + VpJeuLocal.Replace("'", "''") + "' And JeuAdverse = '" + VpJeuAdv.Replace("'", "''") + "';"
				VgDBReader = VgDBCommand.ExecuteReader
				With VgDBReader
					While .Read
						VpGameCounter.AddGame(clsModule.MyCDate(.GetValue(1).ToString), clsModule.MyCDate(.GetValue(2).ToString), .GetBoolean(0))
					End While
					.Close
				End With
				VpDistinctLocalVersions = VpGameCounter.GetDistinctLocalVersions
				VpDistinctAdverseVersions = VpGameCounter.GetDistinctAdverseVersions
				'Diagramme d'évolution
				With Me.chartBreakDown
					.Visible = False			
					.ClearData(ClearDataFlag.Data)
					.SerLeg.Clear
					.OpenData(COD.Values, 2, Math.Max(VpDistinctLocalVersions.Length, VpDistinctAdverseVersions.Length))
					'Ajoute du ratio victoires/défaites local
					For VpI As Integer = 0 To VpDistinctLocalVersions.Length - 1
						.Value(0, VpI) = VpGameCounter.GetNVicLocal(VpDistinctLocalVersions(VpI)) / VpGameCounter.GetNLocal(VpDistinctLocalVersions(VpI))
					Next VpI
					.SerLeg(0) = "Ratio local"
					'Ajout du ratio victoires/défaites adverse
					For VpI As Integer = 0 To VpDistinctAdverseVersions.Length - 1
						.Value(1, VpI) = VpGameCounter.GetNVicAdverse(VpDistinctAdverseVersions(VpI)) / VpGameCounter.GetNAdverse(VpDistinctAdverseVersions(VpI))
					Next VpI
					.SerLeg(1) = "Ratio adverse"
					.SerLegBox = True
					.Chart3D = False
					.Gallery = Gallery.Lines
					.CloseData(COD.Values)
					.Visible = True			
				End With				
			End If
		End If
	End Sub
	Private Function GetBestGameAgainst(VpCbo As TD.SandBar.ComboBoxItem, VpField As String, VpOtherField As String, VpJeu As String, VpVic As Boolean) As String
	'------------------------------------------------------------------------
	'Retourne le jeu le plus efficace à jouer contre celui passé en paramètre
	'------------------------------------------------------------------------
	Dim VpBest As String = ""
	Dim VpMaxRatio As Single = 0
	Dim VpCur As Integer
	Dim VpTotal As Integer
		For Each VpGame As String In VpCbo.Items
			'Nombre total de parties jouées avec le jeu courant
			VgDBCommand.CommandText = "Select Count(*) From MyScores Where " + VpField + " = '" + VpJeu.Replace("'", "''") + "' And " + VpOtherField + " = '" + VpGame.Replace("'", "''") + "';"		
			VpTotal = VgDBCommand.ExecuteScalar
			'Nombre de parties gagnées avec le jeu courant
			VgDBCommand.CommandText = "Select Count(*) From MyScores Where " + VpField + " = '" + VpJeu.Replace("'", "''") + "' And " + VpOtherField + " = '" + VpGame.Replace("'", "''") + "' And Victoire = " + IIf(VpVic, "True", "False") + ";"
			VpCur = VgDBCommand.ExecuteScalar
			'Si meilleur rendement, conserve le jeu courant comme le meilleur prétendant
			If (VpCur / VpTotal) > VpMaxRatio Then
				VpMaxRatio = VpCur / VpTotal
				VpBest = VpGame
			End If
		Next VpGame
		Return VpBest
	End Function
	Private Function GetLeastPlayed As String
	'------------------------------------
	'Retourne la rencontre la moins jouée
	'------------------------------------
	Dim VpMin As Integer = Integer.MaxValue
	Dim VpCur As Integer
	Dim VpLeastPlayed As String = ""
		For Each VpLocal As String In Me.cboJeuLocal.Items
			For Each VpAdv As String In Me.cboJeuAdv.Items
				If VpLocal <> clsModule.CgPerfsTotal And VpAdv <> clsModule.CgPerfsTotal Then
					VgDBCommand.CommandText = "Select Count(*) From MyScores Where JeuLocal = '" + VpLocal.Replace("'", "''") + "' And JeuAdverse = '" + VpAdv.Replace("'", "''") + "';"		
					VpCur = VgDBCommand.ExecuteScalar	
					If VpCur < VpMin Then
						VpMin = VpCur
						VpLeastPlayed = VpLocal + " vs. " + VpAdv
					End If
				End If
			Next VpAdv
		Next VpLocal
		Return VpLeastPlayed
	End Function	
	Private Sub AddKnownGames(VpField As String, VpCbo As TD.SandBar.ComboBoxItem)		
	'--------------------------------------------------------------------------------------
	'Ajoute les jeux déjà connus (de par une saisie antérieure) dans les listes déroulantes
	'--------------------------------------------------------------------------------------
		VgDBCommand.CommandText = "Select Distinct " + VpField + " From MyScores;"
		VgDBReader = VgDBCommand.ExecuteReader
		With VgDBReader
			While .Read
				VpCbo.Items.Add(.GetString(0))
			End While
			.Close
		End With		
	End Sub	
	Private Sub ExcelEfficiency
	'--------------------------------------------------------------------------------
	'Génère un rapport Excel sur l'efficacité des jeux saisis dans la base de données
	'--------------------------------------------------------------------------------
	Dim VpExcelApp As Object
	Dim VpRow As Integer = 5
	Dim VpI As Integer = 0
	Dim VpJ As Integer
	Dim VpDecks() As String = clsPerformances.GetAllDecks
	Dim VpMat(,) As Single = clsPerformances.GetRatioMatrix(VpDecks)
	Dim VpMatT() As Single = clsPerformances.Reshape(VpMat)
	Dim VpRef As Single = clsPerformances.GetMeanPrice(VpDecks) / 50	
	Dim VpPrice As Single
		Try
			VpExcelApp = CreateObject("Excel.Application")
		Catch
			Call clsModule.ShowWarning("Aucune installation de Microsoft Excel n'a été détectée sur votre système." + vbCrLf + "Impossible de continuer...")
			Exit Sub
		End Try	
		With VpExcelApp				
			.Workbooks.Add
			.Visible = True	
			'Partie 1 : efficacité dans l'absolu
			.Cells(1, 1) = "Efficacité mode global"
			.Cells(3, 1) = "Nom du deck"
			.Cells(3, 2) = "Prix du deck"
			.Cells(3, 3) = "Performance (proportions de victoires)"
			.Cells(3, 4) = "Espérance de prix compte-tenu de la performance"
			.Cells(3, 5) = "Espérance de la performance compte-tenu du prix"
			.Cells(3, 6) = "Facteur d'efficacité (meilleur si tend vers 0)"
			.Rows(3).EntireRow.Font.Bold = True
			For Each VpDeck As String In VpDecks
				VpPrice = clsPerformances.GetPrice(VpDeck)
				If VpPrice <> -1 Then
					.Cells(VpRow, 1) = VpDeck
					.Cells(VpRow, 2) = Format(VpPrice, "0.0") + " €"
					.Cells(VpRow, 3) = Format(VpMatT(VpI), "0.0") + "%"
					.Cells(VpRow, 4) = Format(VpRef * VpMatT(VpI), "0.0") + " €"
					.Cells(VpRow, 5) = Format(Math.Min(VpPrice / VpRef, 100), "0.0") + "%"
					.Cells(VpRow, 6) = Format((VpPrice / VpMatT(VpI)) / VpRef, "0.00")
					VpRow = VpRow + 1
				End If
				VpI = VpI + 1
			Next VpDeck
			For VpI = 1 To 6
				.Columns(VpI).EntireColumn.AutoFit
			Next VpI			
			'Partie 2 : facteur d'efficacité d'un jeu contre un autre (à lire sur ligne (et non sur colonne) après génération)
			VpRow = VpRow + 1
			.Cells(VpRow, 1) = "Efficacité mode versus"
			For VpI = 0 To VpDecks.Length - 1
				.Cells(VpI + VpRow + 1, 1) = VpDecks(VpI)
				For VpJ = 0 To VpDecks.Length - 1
					.Cells(VpRow, VpJ + 2) = VpDecks(VpJ)
					.Cells(VpI + VpRow + 1, VpJ + 2) = Format(Me.GetEfficiency(New String() {VpDecks(VpI), VpDecks(VpJ)}, VpDecks(VpI)), "0.00").Replace("Non Numérique", "").Replace("-Infini", "").Replace("+Infini", "")
				Next VpJ
			Next VpI
		End With
		Call clsModule.ShowInformation("Génération terminée." + vbCrLf + "NB. Ce calcul n'a de sens que si tous les jeux en présence ont été saisis dans la base (afin d'en connaître leur prix).")
	End Sub	
	Private Function GetEfficiency(VpDecks() As String, VpGame As String) As Single
	'------------------------------------------------------------------------------------------------------------------------
	'Retourne le facteur d'efficacité pour le jeu passé en paramètre calculé selon :
	' r = ( prix jeu / %victoires ) / ( prix moyen / 0.5 )
	' r = 1 => le jeu est à la hauteur de son prix (jeu normal)
	' r < 1 => le jeu gagne plus de parties qu'il n'en devrait compte tenu de son prix (jeu efficient)
	' r > 1 => le jeu gagne moins de parties qu'il n'en devrait compte tenu de son prix (jeu soit mauvais / soit "bulldozer")
	'------------------------------------------------------------------------------------------------------------------------
	Dim VpMat(,) As Single = clsPerformances.GetRatioMatrix(VpDecks)
	Dim VpMatT() As Single = clsPerformances.Reshape(VpMat)
	Dim VpRef As Single = clsPerformances.GetMeanPrice(VpDecks) / 50
	Dim VpI As Integer = 0
		For Each VpDeck As String In VpDecks
			If VpDeck = VpGame Then
				Return ( (clsPerformances.GetPrice(VpDeck) / VpMatT(VpI)) / VpRef )
			End If
			VpI = VpI + 1
		Next VpDeck
		Return -1	
	End Function
	Private Function IsMixte As Boolean
	'--------------------------------------------------------------------------------
	'Renvoie vrai si au moins un enregistrement de la base pour jeu adverse est mixte
	'--------------------------------------------------------------------------------
	Dim VpDBCommand As New OleDbCommand
    	VpDBCommand.Connection = VgDB
    	VpDBCommand.CommandType = CommandType.Text	
		VpDBCommand.CommandText = "Select Count(*) From MyScores Where JeuAdverse = '" + Me.cboJeuAdv.ComboBox.Text.Replace("'", "''") + "' And IsMixte = True;"
		If CInt(VpDBCommand.ExecuteScalar) > 0 Or Me.cboJeuLocal.Items.Contains(Me.cboJeuAdv.ComboBox.Text) Or Me.cboJeuAdv.Items.Contains(Me.cboJeuLocal.ComboBox.Text) Then
			If clsModule.ShowQuestion("S'agit-il d'une partie indépendante ?" + vbCrLf + "Cliquer sur 'Non' pour la considérer comme une partie intégrant les statistiques locale/adverse habituelles...") = System.Windows.Forms.DialogResult.Yes Then
				Return True
			Else
				Return False
			End If
		Else
			Return False
		End If
	End Function
	Private Sub GetAllPlayed
	'-----------------------------------
	'Affiche le nombre de parties jouées
	'-----------------------------------
	Dim VpStr As String
		VgDBCommand.CommandText = "Select Count(*) From MyScores;"
		VpStr = VgDBCommand.ExecuteScalar.ToString + " parties enregistrées (dont "
		VgDBCommand.CommandText = "Select Count(*) From MyScores Where IsMixte = True;"
		Me.lblAllPlayed.Text = VpStr + VgDBCommand.ExecuteScalar.ToString + " indépendantes)."
	End Sub	
	#End Region
	#Region "Evènements"
	Private Sub AddLocalGameClick(ByVal sender As Object, ByVal e As EventArgs)
		If Not Me.cboJeuLocal.Items.Contains(sender.Text) Then
			Me.cboJeuLocal.Items.Add(sender.Text)
		End If
	End Sub
	Private Sub AddResult(VpJeuLocal As String, VpJeuAdv As String, VpVictoire As Boolean)
		If VpJeuLocal = clsModule.CgPerfsTotal Or VpJeuAdv = clsModule.CgPerfsTotal Then Exit Sub
		If Me.cboJeuLocal.Items.Contains(VpJeuLocal) And Me.cboJeuAdv.Items.Contains(VpJeuAdv) Then
			If Me.dropGamesVersions.Checked Then
				VgDBCommand.CommandText = "Insert Into MyScores(JeuLocal, JeuLocalVersion, JeuAdverse, JeuAdverseVersion, Victoire, IsMixte) Values ('" + VpJeuLocal.Replace("'", "''") + "', " + Me.ValidateVersion(Me.cboLocalVersion) + ", '" + VpJeuAdv.Replace("'", "''") + "', " + Me.ValidateVersion(Me.cboAdvVersion) + ", " + IIf(VpVictoire, "True", "False") + ", " + IIf(Me.IsMixte, "True", "False") + ");"
			Else
				VgDBCommand.CommandText = "Insert Into MyScores(JeuLocal, JeuLocalVersion, JeuAdverse, JeuAdverseVersion, Victoire, IsMixte) Values ('" + VpJeuLocal.Replace("'", "''") + "', Null, '" + VpJeuAdv.Replace("'", "''") + "', Null, " + IIf(VpVictoire, "True", "False") + ", " + IIf(Me.IsMixte, "True", "False") + ");"
			End If
			VgDBCommand.ExecuteNonQuery
			Call Me.UpdateGraph
		Else
			Call clsModule.ShowWarning("Sélectionner deux jeux présents dans les listes déroulantes avant d'en enregistrer le résultat...")
		End If	
		Call Me.GetAllPlayed
	End Sub
	Private Sub RemoveResult(VpJeuLocal As String, VpJeuAdv As String, VpVictoire As Boolean)
		If VpJeuLocal = clsModule.CgPerfsTotal Or VpJeuAdv = clsModule.CgPerfsTotal Then Exit Sub
		If Me.cboJeuLocal.Items.Contains(VpJeuLocal) And Me.cboJeuAdv.Items.Contains(VpJeuAdv) Then
			'Le problème avec Top 1 => on ne sait pas si on vire un IsMixte ou pas
			If Me.dropGamesVersions.Checked Then
				VgDBCommand.CommandText = "Delete * From (Select Top 1 * From MyScores Where JeuLocal = '" + VpJeuLocal.Replace("'", "''") + "' And JeuLocalVersion = " + Me.ValidateVersion(Me.cboLocalVersion) + " And JeuAdverse = '" + VpJeuAdv.Replace("'", "''") + "' And JeuAdverseVersion = " + Me.ValidateVersion(Me.cboAdvVersion) + " And Victoire = " + IIf(VpVictoire, "True", "False") + ");"				
			Else
				VgDBCommand.CommandText = "Delete * From (Select Top 1 * From MyScores Where JeuLocal = '" + VpJeuLocal.Replace("'", "''") + "' And JeuAdverse = '" + VpJeuAdv.Replace("'", "''") + "' And Victoire = " + IIf(VpVictoire, "True", "False") + ");"
			End If
			Try
				VgDBCommand.ExecuteNonQuery
			Catch
				Call clsModule.ShowWarning("Impossible d'accomplir l'opération...")
			End Try
			Call Me.UpdateGraph
		Else
			Call clsModule.ShowWarning("Sélectionner deux jeux présents dans les listes déroulantes avant d'en enregistrer le résultat...")
		End If
		Call Me.GetAllPlayed
	End Sub
	Sub FrmPerfsFormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs)
		VmOwner.mnuPerfs.Tag = Nothing
	End Sub	
	Sub DropAddLocalOtherClick(ByVal sender As Object, ByVal e As EventArgs)
	Dim VpGameName As String = InputBox("Quel est le nom du jeu local à ajouter ?", "Nouveau jeu", "(Deck)")
		If VpGameName.Trim <> "" Then
			If Not Me.cboJeuLocal.Items.Contains(VpGameName) Then
				Me.cboJeuLocal.Items.Add(VpGameName)
			End If			
		End If
	End Sub
	Sub DropAddAdvClick(ByVal sender As Object, ByVal e As EventArgs)
	Dim VpGameName As String = InputBox("Quel est le nom du jeu adverse à ajouter ?", "Nouveau jeu", "(Deck)")
		If VpGameName.Trim <> "" Then
			If Not Me.cboJeuAdv.Items.Contains(VpGameName) Then
				Me.cboJeuAdv.Items.Add(VpGameName)
			End If			
		End If		
	End Sub
	Sub DropAddMixteClick(ByVal sender As Object, ByVal e As EventArgs)
	Dim VpGameName As String = InputBox("Quel est le nom du jeu indépendant (mixte, disponible dans les deux colonnes) à ajouter ?", "Nouveau jeu", "(Deck)")
		If VpGameName.Trim <> "" Then
			If Not Me.cboJeuLocal.Items.Contains(VpGameName) Then
				Me.cboJeuLocal.Items.Add(VpGameName)
			End If			
			If Not Me.cboJeuAdv.Items.Contains(VpGameName) Then
				Me.cboJeuAdv.Items.Add(VpGameName)
			End If			
		End If				
	End Sub	
	Sub BtVicOkActivate(ByVal sender As Object, ByVal e As EventArgs)
		Call Me.AddResult(Me.cboJeuLocal.ComboBox.Text, Me.cboJeuAdv.ComboBox.Text, True)
	End Sub	
	Sub BtDefOkActivate(ByVal sender As Object, ByVal e As EventArgs)
		Call Me.AddResult(Me.cboJeuLocal.ComboBox.Text, Me.cboJeuAdv.ComboBox.Text, False)
	End Sub			
	Sub MySelectedIndexChanged(VpCbo1 As TD.SandBar.ComboBoxItem, VpCbo2 As TD.SandBar.ComboBoxItem)
	Dim VpPickDate As New frmCalendar
		If VpCbo1.Items.Contains(VpCbo1.ComboBox.Text) And VpCbo2.ComboBox.Text = clsModule.CgPerfsVersion Then
			VpPickDate.ShowDialog	
			If VpPickDate.DialogResult = System.Windows.Forms.DialogResult.OK Then
				VpCbo2.Items.Add(VpPickDate.cal.SelectionStart.ToShortDateString)
				VpCbo2.ComboBox.SelectedIndex = VpCbo2.Items.Count - 1
			End If
		Else
			Call Me.MyRefresh
		End If		
	End Sub
	Sub BtVicNokActivate(ByVal sender As Object, ByVal e As EventArgs)
		Call Me.RemoveResult(Me.cboJeuLocal.ComboBox.Text, Me.cboJeuAdv.ComboBox.Text, True)
	End Sub	
	Sub BtDefNokActivate(ByVal sender As Object, ByVal e As EventArgs)
		Call Me.RemoveResult(Me.cboJeuLocal.ComboBox.Text, Me.cboJeuAdv.ComboBox.Text, False)
	End Sub
	Sub BtAdviceActivate(ByVal sender As Object, ByVal e As EventArgs)
	Dim VpJeuLocal As String = Me.cboJeuLocal.ComboBox.Text
	Dim VpJeuAdv As String = Me.cboJeuAdv.ComboBox.Text
	Dim VpMsg As Boolean = False
		'Rencontre la moins jouée
		Call clsModule.ShowInformation("La rencontre la moins souvent jouée est " + Me.GetLeastPlayed)
		If VpJeuLocal = clsModule.CgPerfsTotal Or VpJeuAdv = clsModule.CgPerfsTotal Then
			VpMsg = True
		Else
			If Me.cboJeuLocal.Items.Contains(VpJeuLocal) And Me.cboJeuAdv.Items.Contains(VpJeuAdv) Then
				'Meilleur jeu adverse contre jeu local
				Call clsModule.ShowInformation("Le jeu adverse le plus fort contre le " + VpJeuLocal + " est le " + Me.GetBestGameAgainst(Me.cboJeuAdv, "JeuLocal", "JeuAdverse", VpJeuLocal, False) + ".")
				'Meilleur jeu local contre jeu adverse
				Call clsModule.ShowInformation("Le jeu local le plus fort contre le " + VpJeuAdv + " est le " + Me.GetBestGameAgainst(Me.cboJeuLocal, "JeuAdverse", "JeuLocal", VpJeuAdv, True) + ".")
			Else
				VpMsg = True
			End If	
		End If
		If VpMsg Then
			Call clsModule.ShowWarning("Sélectionner deux jeux présents dans les listes pour obtenir plus d'informations dessus...")
		End If
	End Sub	
	Sub DropGamesVersionsClick(ByVal sender As Object, ByVal e As EventArgs)
		Me.cboLocalVersion.Visible = Me.dropGamesVersions.Checked
		Me.cboAdvVersion.Visible = Me.dropGamesVersions.Checked
		If Not Me.dropGamesVersions.Checked Then
			Me.mnuEvol.Checked = False
			Me.mnuBreakDown.Checked = True
		End If
		Call Me.MyRefresh
	End Sub
	Sub CboJeuLocalSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
		Call Me.MyRefresh
		Call Me.LoadVersions(Me.cboJeuLocal, Me.cboLocalVersion, "JeuLocal", "JeuLocalVersion")
	End Sub
	Sub CboJeuAdvSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
		Call Me.MyRefresh
		Call Me.LoadVersions(Me.cboJeuAdv, Me.cboAdvVersion, "JeuAdverse", "JeuAdverseVersion")
	End Sub	
	Sub CboLocalVersionSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
		Call Me.MySelectedIndexChanged(Me.cboJeuLocal, Me.cboLocalVersion)
	End Sub	
	Sub CboAdvVersionSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
		Call Me.MySelectedIndexChanged(Me.cboJeuAdv, Me.cboAdvVersion)
	End Sub	
	Sub MnuBreakDownClick(ByVal sender As Object, ByVal e As EventArgs)
		Me.mnuBreakDown.Checked = True
		Me.mnuEvol.Checked = False
		Call Me.MyRefresh
	End Sub
	Sub MnuEvolClick(ByVal sender As Object, ByVal e As EventArgs)
		Me.mnuBreakDown.Checked = False
		Me.mnuEvol.Checked = True
		Call Me.MyRefresh		
	End Sub	
	Sub CmnuChartOpening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
		Me.mnuEvol.Enabled = ( Me.cboLocalVersion.Items.Count > 2 Or Me.cboAdvVersion.Items.Count > 2 ) And Me.dropGamesVersions.Checked
	End Sub	
	Sub FrmPerfsLoad(ByVal sender As Object, ByVal e As EventArgs)
		Me.cboJeuLocal.ComboBox.Text = clsModule.CgPerfsTotal		
		Me.cboJeuAdv.ComboBox.Text = clsModule.CgPerfsTotal
		Call Me.GetAllPlayed
	End Sub	
	Sub BtEfficiencyActivate(ByVal sender As Object, ByVal e As EventArgs)
	Dim VpDecks() As String
	Dim VpGame As String = ""
	Dim VpStr As String = clsModule.CgPerfsEfficiency
		If clsModule.ShowQuestion("Générer un rapport complet sous Excel ?" + vbCrLf + "Cliquer sur 'Non' pour obtenir l'information ponctuelle...")= System.Windows.Forms.DialogResult.Yes Then
			Call Me.ExcelEfficiency
		Else
			'Cas 0 : aucun jeu sélectionné
			If Me.cboJeuLocal.ComboBox.Text = clsModule.CgPerfsTotal And Me.cboJeuAdv.ComboBox.Text = clsModule.CgPerfsTotal Then
				Call clsModule.ShowWarning("Sélectionner au moins un jeu présent dans les listes déroulantes pour obtenir l'information souhaitée...")
			'Cas 1 : efficacité absolue d'un jeu local / adverse
			ElseIf Me.cboJeuLocal.ComboBox.Text = clsModule.CgPerfsTotal Or Me.cboJeuAdv.ComboBox.Text = clsModule.CgPerfsTotal Then
				VpDecks = clsPerformances.GetAllDecks
				'Cas 1.1 : efficacité absolue d'un jeu adverse
				If Me.cboJeuLocal.Items.Contains(Me.cboJeuLocal.ComboBox.Text) And Me.cboJeuLocal.ComboBox.Text <> clsModule.CgPerfsTotal Then
					VpGame = Me.cboJeuLocal.ComboBox.Text
					VpStr = VpStr + VpGame + " : " + Me.GetEfficiency(VpDecks, VpGame).ToString
				'Cas 1.2 : efficacité absolue d'un jeu adverse
				ElseIf Me.cboJeuAdv.Items.Contains(Me.cboJeuAdv.ComboBox.Text) Then
					VpGame = Me.cboJeuAdv.ComboBox.Text
					VpStr = VpStr + VpGame + " : " + Me.GetEfficiency(VpDecks, VpGame).ToString
				End If
			'Cas 2 : efficacité entre deux jeux
			ElseIf Me.cboJeuLocal.Items.Contains(Me.cboJeuLocal.ComboBox.Text) And Me.cboJeuAdv.Items.Contains(Me.cboJeuAdv.ComboBox.Text) Then
				ReDim VpDecks(0 To 1)
				VpDecks(0) = Me.cboJeuLocal.ComboBox.Text
				VpDecks(1) = Me.cboJeuAdv.ComboBox.Text
				VpStr = VpStr + VpDecks(0) + " : " + Me.GetEfficiency(VpDecks, VpDecks(0)).ToString + vbCrLf
				VpStr = VpStr + VpDecks(1) + " : " + Me.GetEfficiency(VpDecks, VpDecks(1)).ToString
			End If
			'Affichage effectif
			If VpStr <> clsModule.CgPerfsEfficiency Then
				Call clsModule.ShowInformation(VpStr)
			End If
		End If
	End Sub	
	#End Region
End Class
Public Class clsGameCounter
	Private VmGames As New ArrayList
	Public Sub AddGame(VpLocaleVersion As Date, VpAdverseVersion As Date, VpVicLocale As Boolean)
		Me.VmGames.Add(New clsGame(VpLocaleVersion, VpAdverseVersion, VpVicLocale))
	End Sub
	Public Function GetDistinctLocalVersions As Date()
	Dim VpDates As New ArrayList
		For Each VpGame As clsGame In Me.VmGames
			If Not VpDates.Contains(VpGame.LocaleVersion) Then
				VpDates.Add(VpGame.LocaleVersion)
			End If
		Next VpGame
		VpDates.Sort(New clsDateComparer)
		Return VpDates.ToArray(System.Type.GetType("System.DateTime"))
	End Function
	Public Function GetDistinctAdverseVersions As Date()
	Dim VpDates As New ArrayList
		For Each VpGame As clsGame In Me.VmGames
			If Not VpDates.Contains(VpGame.AdverseVersion) Then
				VpDates.Add(VpGame.AdverseVersion)
			End If
		Next VpGame
		VpDates.Sort(New clsDateComparer)
		Return VpDates.ToArray(System.Type.GetType("System.DateTime"))
	End Function
	Public Function GetNLocal(VpLocalVersion As Date) As Integer
	Dim VpN As Integer = 0
		For Each VpGame As clsGame In Me.VmGames
			If VpGame.LocaleVersion = VpLocalVersion Then
				VpN = VpN + 1
			End If
		Next VpGame
		Return VpN
	End Function
	Public Function GetNVicLocal(VpLocalVersion As Date) As Integer
	Dim VpN As Integer = 0
		For Each VpGame As clsGame In Me.VmGames
			If VpGame.LocaleVersion = VpLocalVersion And VpGame.VicLocale = True Then
				VpN = VpN + 1
			End If
		Next VpGame
		Return VpN
	End Function
	Public Function GetNAdverse(VpAdverseVersion As Date) As Integer
	Dim VpN As Integer = 0
		For Each VpGame As clsGame In Me.VmGames
			If VpGame.AdverseVersion = VpAdverseVersion Then
				VpN = VpN + 1
			End If
		Next VpGame
		Return VpN		
	End Function	
	Public Function GetNVicAdverse(VpAdverseVersion As Date) As Integer
	Dim VpN As Integer = 0
		For Each VpGame As clsGame In Me.VmGames
			If VpGame.AdverseVersion = VpAdverseVersion And VpGame.VicLocale = False Then
				VpN = VpN + 1
			End If
		Next VpGame
		Return VpN		
	End Function	
End Class
Public Class clsGame
	Public LocaleVersion As Date
	Public AdverseVersion As Date
	Public VicLocale As Boolean
	Public Sub New(VpLocaleVersion As Date, VpAdverseVersion As Date, VpVicLocale As Boolean)
		Me.LocaleVersion = VpLocaleVersion
		Me.AdverseVersion = VpAdverseVersion
		Me.VicLocale = VpVicLocale
	End Sub
End Class
Public Class clsDateComparer 
	Implements IComparer
	Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
		Return Date.Compare(CDate(x), CDate(y))
	End Function	
End Class
Public Class clsPerformances
	Public Shared Function GetAllDecks As String()
	'-----------------------------------------------------------------
	'Retourne le nom de tous les jeux en présence (locaux et adverses)
	'-----------------------------------------------------------------
	Dim VpGames As New ArrayList
		VgDBCommand.CommandText = "Select JeuLocal, JeuAdverse From MyScores;"
		VgDBReader = VgDBCommand.ExecuteReader
		With VgDBReader
			While .Read
				'Ajoute distinctement les jeux locaux
				If Not VpGames.Contains(.GetString(0)) Then
					VpGames.Add(.GetString(0))
				End If
				'Ajoute distinctement les jeux adverses
				If Not VpGames.Contains(.GetString(1)) Then
					VpGames.Add(.GetString(1))
				End If				
			End While
			.Close
		End With
		Return VpGames.ToArray(System.Type.GetType("System.String"))
	End Function	
	Public Shared Function GetNPlayed(VpGame1 As String, Optional VpGame2 As String = "") As Integer
	'---------------------------------------------------------------------------------------------------------------------
	'Retourne le nombre de parties jouées par le jeu passé en paramètre, ou dans l'absolu, ou contre l'adversaire spécifié
	'---------------------------------------------------------------------------------------------------------------------
	Dim VpP As Integer
		'Cas 1 : suppose que le jeu 1 est local et le jeu 2 est adverse
		VgDBCommand.CommandText = "Select Count(*) From MyScores Where JeuLocal = '" + VpGame1.Replace("'", "''") + "'" + IIf(VpGame2 <> "", " And JeuAdverse = '" + VpGame2.Replace("'", "''") + "';", ";")
		VpP = VgDBCommand.ExecuteScalar
		'Cas 2 : suppose que le jeu 1 est adverse et le jeu 2 est local
		VgDBCommand.CommandText = "Select Count(*) From MyScores Where JeuAdverse = '" + VpGame1.Replace("'", "''") + "'" + IIf(VpGame2 <> "", " And JeuLocal = '" + VpGame2.Replace("'", "''") + "';", ";")
		Return VpP + VgDBCommand.ExecuteScalar		
	End Function
	Public Shared Function GetNVictoires(VpGame1 As String, Optional VpGame2 As String = "") As Integer
	'----------------------------------------------------------------------------------------------------------------------
	'Retourne le nombre de parties gagnées par le jeu passé en paramètre, ou dans l'absolu, ou contre l'adversaire spécifié
	'----------------------------------------------------------------------------------------------------------------------
	Dim VpP As Integer
		'Cas 1 : suppose que le jeu 1 est local et le jeu 2 est adverse
		VgDBCommand.CommandText = "Select Count(*) From MyScores Where JeuLocal = '" + VpGame1.Replace("'", "''") + "'" + IIf(VpGame2 <> "", " And JeuAdverse = '" + VpGame2.Replace("'", "''") + "'", "") + " And Victoire = True;"
		VpP = VgDBCommand.ExecuteScalar
		'Cas 2 : suppose que le jeu 1 est adverse et le jeu 2 est local
		VgDBCommand.CommandText = "Select Count(*) From MyScores Where JeuAdverse = '" + VpGame1.Replace("'", "''") + "'" + IIf(VpGame2 <> "", " And JeuLocal = '" + VpGame2.Replace("'", "''") + "'", "") + " And Victoire = False;"
		Return VpP + VgDBCommand.ExecuteScalar		
	End Function	
	Public Shared Function GetRatio(VpGame1 As String, Optional VpGame2 As String = "") As Single
	'------------------------------------------------------------------------------------------------------------------------
	'Retourne la fraction de parties gagnées par le jeu passé en paramètre, ou dans l'absolu, ou contre l'adversaire spécifié
	'------------------------------------------------------------------------------------------------------------------------
	Dim VpT As Integer = clsPerformances.GetNPlayed(VpGame1, VpGame2)
		If VpT > 0 Then
			Return clsPerformances.GetNVictoires(VpGame1, VpGame2) / VpT
		Else
			Return -1
		End If
	End Function
	Public Shared Function GetRatioMatrix(VpGames As String()) As Single(,)
	'-------------------------------------------------------------------------------------------------------------------
	'Retourne la fraction de parties gagnées par le jeu i contre le jeu j, 1<(i,j)<N, N nombre total de jeux en présence
	'-------------------------------------------------------------------------------------------------------------------
	Dim VpMat(,) As Single
	Dim VpN As Integer
		VpN =  VpGames.Length
		ReDim VpMat(0 To VpN - 1, 0 To VpN - 1)
		For VpI As Integer = 0 To VpN - 1
			For VpJ As Integer = 0 To VpN - 1
				VpMat(VpI, VpJ) = clsPerformances.GetRatio(VpGames(VpI), VpGames(VpJ))
			Next VpJ
		Next VpI
		Return VpMat
	End Function
	Public Shared Function Reshape(VpMatrix(,) As Single) As Single()
	'--------------------------------------------------------
	'Rapporte la fraction de victoires par couple en base 100
	'--------------------------------------------------------
	Dim VpN As Integer
	Dim VpNN As Integer
	Dim VpNewShape() As Single
		VpN = 1 + VpMatrix.GetUpperBound(0)
		ReDim VpNewShape(0 To VpN - 1)
		For VpI As Integer = 0 To VpN - 1
			VpNN = 0
			For VpJ As Integer = 0 To VpN - 1
				If VpMatrix(VpI, VpJ) <> - 1 Then
					VpNewShape(VpI) = VpNewShape(VpI) + VpMatrix(VpI, VpJ)
					VpNN = VpNN + 1
				End If
			Next VpJ
			VpNewShape(VpI) = 100 * VpNewShape(VpI) / VpNN
		Next VpI
		Return VpNewShape
	End Function
	Public Shared Function GetPrice(VpGame As String) As Single
	'------------------------------------------
	'Retourne le prix du jeu passé en paramètre
	'------------------------------------------
	Dim VpId As String
		VpId = VgOptions.GetDeckIndex(VpGame)
		If VpId <> "" Then
			VgDBCommand.CommandText = "Select Sum(Val(Price) * Items) From Card Inner Join MyGames On Card.EncNbr = MyGames.EncNbr Where GameID = " + VpId + ";"
			Return VgDBCommand.ExecuteScalar
		Else
			Return -1
		End If
	End Function
	Public Shared Function GetMeanPrice(VpGames As String()) As Single
	'-------------------------------------------
	'Retourne le prix moyen des jeux en présence
	'-------------------------------------------
	Dim VpSum As Single = 0
	Dim VpCur As Single
	Dim VpN As Integer = 0
		For Each VpGame As String In VpGames
			VpCur = clsPerformances.GetPrice(VpGame)
			If VpCur <> -1 Then
				VpSum = VpSum + VpCur
				VpN = VpN + 1
			End If
		Next VpGame
		If VpN <> 0 Then
			Return VpSum / VpN
		Else
			Return -1
		End If
	End Function
End Class
