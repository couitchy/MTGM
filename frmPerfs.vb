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
'| - gestion des versions de jeux		   13/10/2008 |
'| - gestion de parties 'hors total'	   10/09/2009 |
'| - gestion des adversaires multiples	   12/11/2010 |
'| - diversification des informations	   08/10/2011 |
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
		For VpI As Integer = 1 To clsModule.GetDeckCount
			Me.dropAddGame.DropDownItems.Add(clsModule.GetDeckName(VpI), Nothing, AddressOf AddGameClick)
		Next VpI
		Call AddKnownGames("JeuLocal", Me.cboJeuLocal)
		Call AddKnownGames("JeuAdverse", Me.cboJeuAdv)
		AddHandler Me.cboJeuLocal.ComboBox.SelectedIndexChanged, AddressOf CboJeuLocalSelectedIndexChanged
		AddHandler Me.cboJeuAdv.ComboBox.SelectedIndexChanged, AddressOf CboJeuAdvSelectedIndexChanged
		AddHandler Me.cboLocalVersion.ComboBox.SelectedIndexChanged, AddressOf CboLocalVersionSelectedIndexChanged
		AddHandler Me.cboAdvVersion.ComboBox.SelectedIndexChanged, AddressOf CboAdvVersionSelectedIndexChanged
		Me.cboLocalVersion.ComboBox.BackColor = Color.LightBlue
		Me.cboAdvVersion.ComboBox.BackColor = Color.LightBlue
		For VpI As Integer = 1 To clsModule.GetAdvCount
			Me.cboJeuLocal.Items.Add(clsModule.CgPerfsTotal + clsModule.GetAdvName(VpI))
			Me.cboJeuAdv.Items.Add(clsModule.CgPerfsTotal + clsModule.GetAdvName(VpI))
		Next VpI
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
	Private Sub GetNVictoires(VpJeuLocal As String, VpJeuAdv As String, ByRef VpVicLocal As Integer, ByRef VpVicAdv As Integer, VpCritere As Boolean, VpOwnerLocal As String, VpOwnerAdv As String)
	Dim VpSQL As String
		If VpOwnerLocal <> "" And VpOwnerAdv <> "" Then
			If VpCritere Then
				VpSQL = "Select Victoire From ((Select * From (MyScores Inner Join MyGamesID On MyScores.JeuLocal = MyGamesID.GameName) Inner Join MyAdversairesID On MyAdversairesID.AdvID = MyGamesID.AdvID) As T1 Inner Join MyGamesID On T1.JeuAdverse = MyGamesID.GameName) Inner Join MyAdversairesID On MyAdversairesID.AdvID = MyGamesID.AdvID Where T1.AdvName = '" + VpOwnerLocal + "' And MyAdversairesID.AdvName = '" + VpOwnerAdv + "';"
			Else
				VpSQL = "Select Victoire From ((Select * From (MyScores Inner Join MyGamesID On MyScores.JeuLocal = MyGamesID.GameName) Inner Join MyAdversairesID On MyAdversairesID.AdvID = MyGamesID.AdvID) As T1 Inner Join MyGamesID On T1.JeuAdverse = MyGamesID.GameName) Inner Join MyAdversairesID On MyAdversairesID.AdvID = MyGamesID.AdvID Where T1.AdvName = '" + VpOwnerAdv + "' And MyAdversairesID.AdvName = '" + VpOwnerLocal + "';"
			End If
		ElseIf VpOwnerLocal <> "" Then
			If VpCritere Then
				VpSQL = "Select Victoire From (MyScores Inner Join MyGamesID On MyScores.JeuLocal = MyGamesID.GameName) Inner Join MyAdversairesID On MyAdversairesID.AdvID = MyGamesID.AdvID Where AdvName = '" + VpOwnerLocal + "' And JeuAdverse = '" + VpJeuAdv + "';"
			Else
				VpSQL = "Select Victoire From (MyScores Inner Join MyGamesID On MyScores.JeuAdverse = MyGamesID.GameName) Inner Join MyAdversairesID On MyAdversairesID.AdvID = MyGamesID.AdvID Where AdvName = '" + VpOwnerLocal + "' And JeuLocal = '" + VpJeuAdv + "';"
			End If
		ElseIf VpOwnerAdv <> "" Then
			If Not VpCritere Then
				VpSQL = "Select Victoire From (MyScores Inner Join MyGamesID On MyScores.JeuLocal = MyGamesID.GameName) Inner Join MyAdversairesID On MyAdversairesID.AdvID = MyGamesID.AdvID Where AdvName = '" + VpOwnerAdv + "' And JeuAdverse = '" + VpJeuLocal + "';"
			Else
				VpSQL = "Select Victoire From (MyScores Inner Join MyGamesID On MyScores.JeuAdverse = MyGamesID.GameName) Inner Join MyAdversairesID On MyAdversairesID.AdvID = MyGamesID.AdvID Where AdvName = '" + VpOwnerAdv + "' And JeuLocal = '" + VpJeuLocal + "';"
			End If
		Else
			VpSQL = "Select Victoire From MyScores Where JeuLocal = '" + VpJeuLocal + "' And JeuAdverse = '" + VpJeuAdv + "';"
		End If
		VgDBCommand.CommandText = VpSQL
		VgDBReader = VgDBCommand.ExecuteReader
		With VgDBReader
			'Comptage victoires / défaites
			While .Read
				If .GetBoolean(0) = VpCritere Then
					VpVicLocal = VpVicLocal + 1
				Else
					VpVicAdv = VpVicAdv + 1
				End If
			End While
			.Close
		End With
	End Sub
	Private Sub UpdateGraph
	'----------------------------------------------------------------------------------
	'Affiche le diagramme de répartitions des victoires sur les deux jeux sélectionnés
	'ou le diagramme d'évolution suivant les versions (selon l'état du menu contextuel)
	'----------------------------------------------------------------------------------
	Dim VpSQL As String
	Dim VpJeuLocal As String = Me.cboJeuLocal.ComboBox.Text.Replace("'", "''")
	Dim VpJeuAdv As String = Me.cboJeuAdv.ComboBox.Text.Replace("'", "''")
	Dim VpVicLocal As Integer = 0
	Dim VpVicAdv As Integer = 0
	Dim VpGameCounter As New clsGameCounter						'Matrice contenant {version locale, version adverse, nombre victoires locales, nombre victoires adverses)
	Dim VpDistinctAdverseVersions() As Date
	Dim VpDistinctLocalVersions() As Date
	Dim VpHandleGamesVersions As Boolean = (Not (VpJeuLocal.StartsWith(clsModule.CgPerfsTotal) Or VpJeuAdv.StartsWith(clsModule.CgPerfsTotal))) And Me.dropGamesVersions.Checked
		'Si on ne tient pas compte des versions des jeux, le diagramme est bipolaire
		If Not VpHandleGamesVersions Then
			'Cas 1 : jeu 1 contre jeu 2
			If Not (VpJeuLocal.StartsWith(clsModule.CgPerfsTotal) Or VpJeuAdv.StartsWith(clsModule.CgPerfsTotal)) Then
				'Cas 1.1 : 1 contre 2
				Call Me.GetNVictoires(VpJeuLocal, VpJeuAdv, VpVicLocal, VpVicAdv, True, "", "")
				'Cas 1.2 : 2 contre 1
				Call Me.GetNVictoires(VpJeuAdv, VpJeuLocal, VpVicLocal, VpVicAdv, False, "", "")
			'Cas 2 : adversaire 1 contre adversaire 2
			ElseIf VpJeuLocal.StartsWith(clsModule.CgPerfsTotal) And VpJeuAdv.StartsWith(clsModule.CgPerfsTotal) Then
				VpJeuLocal = VpJeuLocal.Replace(clsModule.CgPerfsTotal, "")
				VpJeuAdv = VpJeuAdv.Replace(clsModule.CgPerfsTotal, "")
				'Cas 2.1 : 1 contre 2
				Call Me.GetNVictoires("", "", VpVicLocal, VpVicAdv, True, VpJeuLocal, VpJeuAdv)
				'Cas 2.2 : 2 contre 1
				Call Me.GetNVictoires("", "", VpVicLocal, VpVicAdv, False, VpJeuLocal, VpJeuAdv)
			'Cas 3 : adversaire 1 contre jeu 2
			ElseIf VpJeuLocal.StartsWith(clsModule.CgPerfsTotal)
				VpJeuLocal = VpJeuLocal.Replace(clsModule.CgPerfsTotal, "")
				'Cas 3.1 : 1 contre 2
				Call Me.GetNVictoires("", VpJeuAdv, VpVicLocal, VpVicAdv, True, VpJeuLocal, "")
				'Cas 3.2 : 2 contre 1
				Call Me.GetNVictoires("", VpJeuAdv, VpVicLocal, VpVicAdv, False, VpJeuLocal, "")
			'Cas 4 : jeu 1 contre adversaire 2
			ElseIf VpJeuAdv.StartsWith(clsModule.CgPerfsTotal) Then
				VpJeuAdv = VpJeuAdv.Replace(clsModule.CgPerfsTotal, "")
				'Cas 4.1 : 1 contre 2
				Call Me.GetNVictoires(VpJeuLocal, "", VpVicLocal, VpVicAdv, True, "", VpJeuAdv)
				'Cas 4.2 : 2 contre 1
				Call Me.GetNVictoires(VpJeuLocal, "", VpVicLocal, VpVicAdv, False, "", VpJeuAdv)
			End If
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
					'Ajout des victoires locales
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
			VgDBCommand.CommandText = "Select Count(*) From MyScores Where " + VpField + " = '" + VpJeu.Replace("'", "''") + "' And " + VpOtherField + " = '" + VpGame.Replace("'", "''") + "' And Victoire = " + If(VpVic, "True", "False") + ";"
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
				If Not VpLocal.StartsWith(clsModule.CgPerfsTotal) And Not VpAdv.StartsWith(clsModule.CgPerfsTotal) And VpLocal <> VpAdv And clsModule.GetOwner(VpLocal) <> clsModule.GetOwner(VpAdv) Then
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
	Dim VpRow As Integer = 3
	Dim VpI As Integer = 0
	Dim VpJ As Integer
	Dim VpK As Integer
	Dim VpL As Integer
	Dim VpM As Integer
	Dim VpJ1 As String
	Dim VpJ2 As String
	Dim VpJn1 As Integer
	Dim VpJn2 As Integer	
	Dim VpJv1 As Integer
	Dim VpJv2 As Integer		
	Dim VpDecks() As String = clsPerformances.GetAllDecks
	Dim VpMat(,) As Single = clsPerformances.GetRatioMatrix(VpDecks)
	Dim VpMatT() As Single = clsPerformances.Reshape(VpMat)
	Dim VpRef As Single = clsPerformances.GetMeanPrice(VpDecks) / 50
	Dim VpPrice As Single
	Dim VpVersusMatV1() As Integer
	Dim VpVersusMatD1() As Integer
	Dim VpVersusMatV2() As Integer
	Dim VpVersusMatD2() As Integer
	Dim VpTotV1 As Integer
	Dim VpTotV2 As Integer
	Dim VpEfficiencies As New List(Of clsEfficiency)
	Dim VpFrequences As New List(Of clsMatchCounter)
		Try
			VpExcelApp = CreateObject("Excel.Application")
		Catch
			Call clsModule.ShowWarning("Aucune installation de Microsoft Excel n'a été détectée sur votre système..." + vbCrLf + "Impossible de continuer.")
			Exit Sub
		End Try
		With VpExcelApp
			.Workbooks.Add
			'Partie 1 : efficacité dans l'absolu
			With .Sheets(1)
				.Name = "Efficience"
				.Cells(1, 1) = "Nom du deck"
				.Cells(1, 2) = "Prix du deck"
				.Cells(1, 3) = "Performance (proportions de victoires)"
				.Cells(1, 4) = "Espérance de prix compte-tenu de la performance"
				.Cells(1, 5) = "Espérance de la performance compte-tenu du prix"
				.Cells(1, 6) = "Facteur d'efficacité (meilleur si tend vers 0)"
				.Rows(1).EntireRow.Font.Bold = True
				For Each VpDeck As String In VpDecks
					VpPrice = clsPerformances.GetPrice(VpDeck)
					If VpPrice <> -1 Then
						VpEfficiencies.Add(New clsEfficiency(VpDeck, VpPrice, VpMatT(VpI), VpRef * VpMatT(VpI), Math.Min(VpPrice / VpRef, 100), (VpPrice / VpMatT(VpI)) / VpRef))
					End If
					VpI = VpI + 1
				Next VpDeck
				VpEfficiencies.Sort(New clsEfficiency.clsEfficiencyComparer)
				For Each VpEfficiency As clsEfficiency In VpEfficiencies
					.Cells(VpRow, 1) = VpEfficiency.Name
					.Cells(VpRow, 2) = VpEfficiency.Price
					.Cells(VpRow, 3) = Format(VpEfficiency.Perfs, "0.0") + "%"
					.Cells(VpRow, 4) = VpEfficiency.EspPrice
					.Cells(VpRow, 5) = Format(VpEfficiency.EspPerfs, "0.0") + "%"
					.Cells(VpRow, 6) = Format(VpEfficiency.Efficiency, "0.00")
					VpRow = VpRow + 1
				Next VpEfficiency
				'Formatage particulier
				For VpI = 1 To 6
					.Columns(VpI).EntireColumn.AutoFit
					If VpI = 2 Or VpI = 4 Then
						.Columns(VpI).EntireColumn.NumberFormat = "0,00 €"
					End If
				Next VpI
			End With
			VpRow = 1
			'Partie 2 : résultats matches versus
			With .Sheets(2)
				.Name = "Matches vs."
				VpM = clsModule.GetAdvCount
				For VpI = 1 To VpM
					For VpJ = VpI + 1 To VpM
						VpJ1 = clsModule.GetAdvName(VpI)
						VpJ2 = clsModule.GetAdvName(VpJ)
						VpJn1 = clsModule.GetAdvDecksCount(VpJ1)
						VpJn2 = clsModule.GetAdvDecksCount(VpJ2)
						If VpJn1 > 0 And VpJn2 > 0 Then
							ReDim VpVersusMatV1(0 To VpJn1 - 1)
							ReDim VpVersusMatD1(0 To VpJn1 - 1)
							ReDim VpVersusMatV2(0 To VpJn2 - 1)
							ReDim VpVersusMatD2(0 To VpJn2 - 1)
							VpTotV1 = 0
							VpTotV2 = 0
							.Cells(VpRow, 1) = VpJ1 + " vs. " + VpJ2
							.Rows(VpRow).EntireRow.Font.Bold = True
							VpRow = VpRow + 1
							VpK = 1
							For Each VpDeck1 As String In clsPerformances.GetAdvDecks(VpJ1)
								.Cells(VpK + VpRow + 1, 1) = VpDeck1
								VpL = 1
								For Each VpDeck2 As String In clsPerformances.GetAdvDecks(VpJ2)
									.Cells(VpRow, VpL + 1) = VpDeck2
									VpJv1 = clsPerformances.GetNVictoires(VpDeck1, VpDeck2)
									VpJv2 = clsPerformances.GetNDefaites(VpDeck1, VpDeck2)
									VpVersusMatV1(VpK - 1) += VpJv1
									VpVersusMatD1(VpK - 1) += VpJv2
									VpVersusMatV2(VpL - 1) += VpJv1
									VpVersusMatD2(VpL - 1) += VpJv2
									.Cells(VpK + VpRow + 1, VpL + 1) = VpJv1.ToString + "V / " + VpJv2.ToString + "D"
									VpL = VpL + 1
								Next VpDeck2
								.Cells(VpK + VpRow + 1, VpL + 1) = VpVersusMatV1(VpK - 1).ToString + "V / " + VpVersusMatD1(VpK - 1).ToString + "D"
								.Cells(VpK + VpRow + 1, VpL + 1).Interior.ColorIndex = 48
								VpK = VpK + 1
							Next VpDeck1
							For VpF As Integer = 0 To VpVersusMatV2.Length - 1
								VpTotV1 += VpVersusMatV2(VpF)
								VpTotV2 += VpVersusMatD2(VpF)
								.Cells(VpK + VpRow + 1, VpF + 2) = VpVersusMatV2(VpF).ToString + "V / " + VpVersusMatD2(VpF).ToString + "D"
								.Cells(VpK + VpRow + 1, VpF + 2).Interior.ColorIndex = 48
							Next VpF
							.Cells(VpK + VpRow + 1, VpVersusMatV2.Length + 2) = VpTotV1.ToString + "V / " + VpTotV2.ToString + "D"
							.Cells(VpK + VpRow + 1, VpVersusMatV2.Length + 2).Interior.ColorIndex = 48
							.Cells(VpK + VpRow + 1, VpVersusMatV2.Length + 2).Font.Bold = True
							VpRow = VpRow + VpK + 3
						End If
					Next VpJ
				Next VpI
				'Formatage particulier
				For VpI = 1 To VpDecks.Length + 1
					.Columns(VpI).EntireColumn.AutoFit
				Next VpI
			End With
			VpRow = 1
			'Partie 3 : fréquence des matches
			With .Sheets(3)
				.Name = "Fréquences"
				For VpI = 1 To VpM
					For VpJ = VpI + 1 To VpM
						VpJ1 = clsModule.GetAdvName(VpI)
						VpJ2 = clsModule.GetAdvName(VpJ)
						If clsModule.GetAdvDecksCount(VpJ1) > 0 And clsModule.GetAdvDecksCount(VpJ2) > 0 Then
							.Cells(VpRow, 1) = VpJ1 + " vs. " + VpJ2
							.Rows(VpRow).EntireRow.Font.Bold = True
							VpRow = VpRow + 1
							VpFrequences.Clear
							For Each VpDeck1 As String In clsPerformances.GetAdvDecks(VpJ1)
								For Each VpDeck2 As String In clsPerformances.GetAdvDecks(VpJ2)
									VpFrequences.Add(New clsMatchCounter(VpDeck1 + " / " + VpDeck2, clsPerformances.GetNPlayed(VpDeck1, VpDeck2)))
								Next VpDeck2
							Next VpDeck1
							VpFrequences.Sort(New clsMatchCounter.clsMatchCounterComparer)
							For Each VpMatchCounter As clsMatchCounter In VpFrequences
								.Cells(VpRow, 1) = VpMatchCounter.Versus
								.Cells(VpRow, 2) = VpMatchCounter.Count
								VpRow = VpRow + 1
							Next VpMatchCounter
							VpRow = VpRow + 1
						End If
					Next VpJ
				Next VpI
				'Formatage particulier
				For VpI = 1 To VpDecks.Length + 1
					.Columns(VpI).EntireColumn.AutoFit
				Next VpI
			End With
			Call clsModule.ShowInformation("Génération terminée." + vbCrLf + "NB. Ces calculs n'ont de sens que si tous les jeux en présence ont été saisis dans la base (afin d'en connaître leur prix) et si suffisamment de parties entre tous les decks ont été disputées.")
			.Visible = True
		End With
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
	Private Sub GetAllPlayed
	'-----------------------------------
	'Affiche le nombre de parties jouées
	'-----------------------------------
	Dim VpStr As String
		VgDBCommand.CommandText = "Select Count(*) From MyScores;"
		VpStr = VgDBCommand.ExecuteScalar.ToString + " parties enregistrées (dont "
		VgDBCommand.CommandText = "Select Count(*) From MyScores Where JeuLocal Not In (Select GameName From MyGamesID) Or JeuAdverse Not In (Select GameName From MyGamesID);"
		Me.lblAllPlayed.Text = VpStr + VgDBCommand.ExecuteScalar.ToString + " indépendantes)."
	End Sub
	#End Region
	#Region "Evènements"
	Private Sub AddGameClick(ByVal sender As Object, ByVal e As EventArgs)
		If Not Me.cboJeuLocal.Items.Contains(sender.Text) Then
			Me.cboJeuLocal.Items.Add(sender.Text)
		End If
		If Not Me.cboJeuAdv.Items.Contains(sender.Text) Then
			Me.cboJeuAdv.Items.Add(sender.Text)
		End If
	End Sub
	Private Sub AddResult(VpJeuLocal As String, VpJeuAdv As String, VpVictoire As Boolean)
		If VpJeuLocal.StartsWith(clsModule.CgPerfsTotal) Or VpJeuAdv.StartsWith(clsModule.CgPerfsTotal) Then Exit Sub
		If Me.cboJeuLocal.Items.Contains(VpJeuLocal) And Me.cboJeuAdv.Items.Contains(VpJeuAdv) Then
			If Me.dropGamesVersions.Checked Then
				VgDBCommand.CommandText = "Insert Into MyScores(JeuLocal, JeuLocalVersion, JeuAdverse, JeuAdverseVersion, Victoire) Values ('" + VpJeuLocal.Replace("'", "''") + "', " + Me.ValidateVersion(Me.cboLocalVersion) + ", '" + VpJeuAdv.Replace("'", "''") + "', " + Me.ValidateVersion(Me.cboAdvVersion) + ", " + If(VpVictoire, "True", "False") + ");"
			Else
				VgDBCommand.CommandText = "Insert Into MyScores(JeuLocal, JeuLocalVersion, JeuAdverse, JeuAdverseVersion, Victoire) Values ('" + VpJeuLocal.Replace("'", "''") + "', Null, '" + VpJeuAdv.Replace("'", "''") + "', Null, " + If(VpVictoire, "True", "False") + ");"
			End If
			VgDBCommand.ExecuteNonQuery
			Call Me.UpdateGraph
		Else
			Call clsModule.ShowWarning("Sélectionner deux jeux présents dans les listes déroulantes avant d'en enregistrer le résultat...")
		End If
		Call Me.GetAllPlayed
	End Sub
	Private Sub RemoveResult(VpJeuLocal As String, VpJeuAdv As String, VpVictoire As Boolean)
		If VpJeuLocal.StartsWith(clsModule.CgPerfsTotal) Or VpJeuAdv.StartsWith(clsModule.CgPerfsTotal) Then Exit Sub
		If Me.cboJeuLocal.Items.Contains(VpJeuLocal) And Me.cboJeuAdv.Items.Contains(VpJeuAdv) Then
			If Me.dropGamesVersions.Checked Then
				VgDBCommand.CommandText = "Delete * From (Select Top 1 * From MyScores Where JeuLocal = '" + VpJeuLocal.Replace("'", "''") + "' And JeuLocalVersion = " + Me.ValidateVersion(Me.cboLocalVersion) + " And JeuAdverse = '" + VpJeuAdv.Replace("'", "''") + "' And JeuAdverseVersion = " + Me.ValidateVersion(Me.cboAdvVersion) + " And Victoire = " + If(VpVictoire, "True", "False") + ");"
			Else
				VgDBCommand.CommandText = "Delete * From (Select Top 1 * From MyScores Where JeuLocal = '" + VpJeuLocal.Replace("'", "''") + "' And JeuAdverse = '" + VpJeuAdv.Replace("'", "''") + "' And Victoire = " + If(VpVictoire, "True", "False") + ");"
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
	Sub DropAddGameOtherClick(ByVal sender As Object, ByVal e As EventArgs)
	Dim VpGameName As String = InputBox("Quel est le nom du jeu indépendant (disponible dans les deux colonnes) à ajouter ?", "Nouveau jeu", "(Deck)")
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
		Call Me.GetAllPlayed
	End Sub
	Sub BtEfficiencyActivate(ByVal sender As Object, ByVal e As EventArgs)
		If clsModule.ShowQuestion("Générer un rapport complet sous Excel ?" + vbCrLf + "Ceci peut prendre plusieurs secondes...")= System.Windows.Forms.DialogResult.Yes Then
			Call Me.ExcelEfficiency
		End If
	End Sub
	#End Region
End Class
Public Class clsGameCounter
	Private VmGames As New List(Of clsGame)
	Public Sub AddGame(VpLocaleVersion As Date, VpAdverseVersion As Date, VpVicLocale As Boolean)
		VmGames.Add(New clsGame(VpLocaleVersion, VpAdverseVersion, VpVicLocale))
	End Sub
	Public Function GetDistinctLocalVersions As Date()
	Dim VpDates As New List(Of Date)
		For Each VpGame As clsGame In VmGames
			If Not VpDates.Contains(VpGame.LocaleVersion) Then
				VpDates.Add(VpGame.LocaleVersion)
			End If
		Next VpGame
		VpDates.Sort(New clsDateComparer)
		Return VpDates.ToArray
	End Function
	Public Function GetDistinctAdverseVersions As Date()
	Dim VpDates As New List(Of Date)
		For Each VpGame As clsGame In VmGames
			If Not VpDates.Contains(VpGame.AdverseVersion) Then
				VpDates.Add(VpGame.AdverseVersion)
			End If
		Next VpGame
		VpDates.Sort(New clsDateComparer)
		Return VpDates.ToArray
	End Function
	Public Function GetNLocal(VpLocalVersion As Date) As Integer
	Dim VpN As Integer = 0
		For Each VpGame As clsGame In VmGames
			If VpGame.LocaleVersion = VpLocalVersion Then
				VpN = VpN + 1
			End If
		Next VpGame
		Return VpN
	End Function
	Public Function GetNVicLocal(VpLocalVersion As Date) As Integer
	Dim VpN As Integer = 0
		For Each VpGame As clsGame In VmGames
			If VpGame.LocaleVersion = VpLocalVersion And VpGame.VicLocale = True Then
				VpN = VpN + 1
			End If
		Next VpGame
		Return VpN
	End Function
	Public Function GetNAdverse(VpAdverseVersion As Date) As Integer
	Dim VpN As Integer = 0
		For Each VpGame As clsGame In VmGames
			If VpGame.AdverseVersion = VpAdverseVersion Then
				VpN = VpN + 1
			End If
		Next VpGame
		Return VpN
	End Function
	Public Function GetNVicAdverse(VpAdverseVersion As Date) As Integer
	Dim VpN As Integer = 0
		For Each VpGame As clsGame In VmGames
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
	Implements IComparer(Of Date)
	Public Function Compare(ByVal x As Date, ByVal y As Date) As Integer Implements IComparer(Of Date).Compare
		Return Date.Compare(x, y)
	End Function
End Class
Public Class clsPerformances
	Public Shared Function GetAllDecks As String()
	'-----------------------------------------------------------------
	'Retourne le nom de tous les jeux en présence (locaux et adverses)
	'-----------------------------------------------------------------
	Dim VpGames As New List(Of String)
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
		Return VpGames.ToArray
	End Function
	Public Shared Function GetAdvDecks(VpAdvName As String) As String()
	'---------------------------------------------------------
	'Retourne le nom des decks du joueur spécifié en paramètre
	'---------------------------------------------------------
	Dim VpGames As New List(Of String)
		VgDBCommand.CommandText = "Select GameName From MyGamesID Inner Join MyAdversairesID On MyGamesID.AdvID = MyAdversairesID.AdvID Where AdvName = '" + VpAdvName.Replace("'", "''") + "';"
		VgDBReader = VgDBCommand.ExecuteReader
		With VgDBReader
			While .Read
				VpGames.Add(.GetString(0))
			End While
			.Close
		End With
		Return VpGames.ToArray
	End Function
	Public Shared Function GetNPlayed(VpGame1 As String, Optional VpGame2 As String = "") As Integer
	'---------------------------------------------------------------------------------------------------------------------
	'Retourne le nombre de parties jouées par le jeu passé en paramètre, ou dans l'absolu, ou contre l'adversaire spécifié
	'---------------------------------------------------------------------------------------------------------------------
	Dim VpP As Integer
		'Cas 1 : suppose que le jeu 1 est local et le jeu 2 est adverse
		VgDBCommand.CommandText = "Select Count(*) From MyScores Where JeuLocal = '" + VpGame1.Replace("'", "''") + "'" + If(VpGame2 <> "", " And JeuAdverse = '" + VpGame2.Replace("'", "''") + "';", ";")
		VpP = VgDBCommand.ExecuteScalar
		'Cas 2 : suppose que le jeu 1 est adverse et le jeu 2 est local
		VgDBCommand.CommandText = "Select Count(*) From MyScores Where JeuAdverse = '" + VpGame1.Replace("'", "''") + "'" + If(VpGame2 <> "", " And JeuLocal = '" + VpGame2.Replace("'", "''") + "';", ";")
		Return VpP + VgDBCommand.ExecuteScalar
	End Function
	Public Shared Function GetNVictoires(VpGame1 As String, Optional VpGame2 As String = "") As Integer
	'----------------------------------------------------------------------------------------------------------------------
	'Retourne le nombre de parties gagnées par le jeu passé en paramètre, ou dans l'absolu, ou contre l'adversaire spécifié
	'----------------------------------------------------------------------------------------------------------------------
	Dim VpP As Integer
		'Cas 1 : suppose que le jeu 1 est local et le jeu 2 est adverse
		VgDBCommand.CommandText = "Select Count(*) From MyScores Where JeuLocal = '" + VpGame1.Replace("'", "''") + "'" + If(VpGame2 <> "", " And JeuAdverse = '" + VpGame2.Replace("'", "''") + "'", "") + " And Victoire = True;"
		VpP = VgDBCommand.ExecuteScalar
		'Cas 2 : suppose que le jeu 1 est adverse et le jeu 2 est local
		VgDBCommand.CommandText = "Select Count(*) From MyScores Where JeuAdverse = '" + VpGame1.Replace("'", "''") + "'" + If(VpGame2 <> "", " And JeuLocal = '" + VpGame2.Replace("'", "''") + "'", "") + " And Victoire = False;"
		Return VpP + VgDBCommand.ExecuteScalar
	End Function
	Public Shared Function GetNDefaites(VpGame1 As String, Optional VpGame2 As String = "") As Integer
	'----------------------------------------------------------------------------------------------------------------------
	'Retourne le nombre de parties perdues par le jeu passé en paramètre, ou dans l'absolu, ou contre l'adversaire spécifié
	'----------------------------------------------------------------------------------------------------------------------
	Dim VpP As Integer
		'Cas 1 : suppose que le jeu 1 est local et le jeu 2 est adverse
		VgDBCommand.CommandText = "Select Count(*) From MyScores Where JeuLocal = '" + VpGame1.Replace("'", "''") + "'" + If(VpGame2 <> "", " And JeuAdverse = '" + VpGame2.Replace("'", "''") + "'", "") + " And Victoire = False;"
		VpP = VgDBCommand.ExecuteScalar
		'Cas 2 : suppose que le jeu 1 est adverse et le jeu 2 est local
		VgDBCommand.CommandText = "Select Count(*) From MyScores Where JeuAdverse = '" + VpGame1.Replace("'", "''") + "'" + If(VpGame2 <> "", " And JeuLocal = '" + VpGame2.Replace("'", "''") + "'", "") + " And Victoire = True;"
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
		VpId = clsModule.GetDeckIndex(VpGame)
		If VpId <> "" Then
			Try
				VgDBCommand.CommandText = "Select Sum(Price * Items) From Card Inner Join MyGames On Card.EncNbr = MyGames.EncNbr Where GameID = " + VpId + ";"
				Return VgDBCommand.ExecuteScalar
			Catch
				Return -1
			End Try
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
Public Class clsEfficiency
	Public Class clsEfficiencyComparer
		Implements IComparer(Of clsEfficiency)
		Public Function Compare(ByVal x As clsEfficiency, ByVal y As clsEfficiency) As Integer Implements IComparer(Of clsEfficiency).Compare
			If x.Efficiency > y.Efficiency Then
				Return 1
			ElseIf x.Efficiency < y.Efficiency Then
				Return -1
			Else
				Return 0
			End If
		End Function
	End Class
	Private VmName As String
	Private VmPrice As Single
	Private VmPerfs As Single
	Private VmEspPrice As Single
	Private VmEspPerfs As Single
	Private VmEfficiency As Single
	Public Sub New(VpName As String, VpPrice As Single, VpPerfs As Single, VpEspPrice As Single, VpEspPerfs As Single, VpEfficiency As Single)
		VmName = VpName
		VmPrice = VpPrice
		VmPerfs = VpPerfs
		VmEspPrice = VpEspPrice
		VmEspPerfs = VpEspPerfs
		VmEfficiency = VpEfficiency
	End Sub
	Public ReadOnly Property Name As String
		Get
			Return VmName
		End Get
	End Property
	Public ReadOnly Property Price As Single
		Get
			Return VmPrice
		End Get
	End Property
	Public ReadOnly Property Perfs As Single
		Get
			Return VmPerfs
		End Get
	End Property
	Public ReadOnly Property EspPrice As Single
		Get
			Return VmEspPrice
		End Get
	End Property
	Public ReadOnly Property EspPerfs As Single
		Get
			Return VmEspPerfs
		End Get
	End Property
	Public ReadOnly Property Efficiency As Single
		Get
			Return VmEfficiency
		End Get
	End Property
End Class
Public Class clsMatchCounter
	Public Class clsMatchCounterComparer
		Implements IComparer(Of clsMatchCounter)
		Public Function Compare(ByVal x As clsMatchCounter, ByVal y As clsMatchCounter) As Integer Implements IComparer(Of clsMatchCounter).Compare
			Return y.Count - x.Count
		End Function
	End Class
	Private VmVersus As String
	Private VmCount As Integer
	Public Sub New(VpVersus As String, VpCount As Integer)
		VmVersus = VpVersus
		VmCount = VpCount
	End Sub
	Public ReadOnly Property Versus As String
		Get
			Return VmVersus
		End Get
	End Property
	Public ReadOnly Property Count As Integer
		Get
			Return VmCount
		End Get
	End Property
End Class