Imports System.Data
Imports System.Data.OleDb
Imports SoftwareFX.ChartFX.Lite
Public Partial Class frmPerfs
    Private VmOwner As MainForm
    #Region "M�thodes"
    Public Sub New(VpOwner As MainForm)
    '------------
    'Constructeur
    '------------
        Call Me.InitializeComponent
        VmOwner = VpOwner
        For VpI As Integer = 1 To mdlToolbox.GetDeckCount
            Me.dropAddGame.DropDownItems.Add(mdlToolbox.GetDeckNameFromIndex(VpI), Nothing, AddressOf AddGameClick)
        Next VpI
        Call AddKnownGames("JeuLocal", Me.cboJeuLocal)
        Call AddKnownGames("JeuAdverse", Me.cboJeuAdv)
        AddHandler Me.cboJeuLocal.ComboBox.SelectedIndexChanged, AddressOf CboJeuLocalSelectedIndexChanged
        AddHandler Me.cboJeuAdv.ComboBox.SelectedIndexChanged, AddressOf CboJeuAdvSelectedIndexChanged
        AddHandler Me.cboLocalVersion.ComboBox.SelectedIndexChanged, AddressOf CboLocalVersionSelectedIndexChanged
        AddHandler Me.cboAdvVersion.ComboBox.SelectedIndexChanged, AddressOf CboAdvVersionSelectedIndexChanged
        Me.cboLocalVersion.ComboBox.BackColor = Color.LightBlue
        Me.cboAdvVersion.ComboBox.BackColor = Color.LightBlue
        For VpI As Integer = 1 To mdlToolbox.GetAdvCount
            Me.cboJeuLocal.Items.Add(mdlConstGlob.CgPerfsTotal + mdlToolbox.GetAdvName(VpI))
            Me.cboJeuAdv.Items.Add(mdlConstGlob.CgPerfsTotal + mdlToolbox.GetAdvName(VpI))
        Next VpI
        Me.cboLocalVersion.Items.Add(mdlConstGlob.CgPerfsVersion)
        Me.cboAdvVersion.Items.Add(mdlConstGlob.CgPerfsVersion)
        Me.cboLocalVersion.Items.Add(mdlConstGlob.CgPerfsTotalV)
        Me.cboAdvVersion.Items.Add(mdlConstGlob.CgPerfsTotalV)
    End Sub
    Private Sub MyRefresh
    '---------------------------------------------------------------------------
    'Mise � jour du graphique lorsque les deux items des comboboxes sont valides
    '---------------------------------------------------------------------------
        If Me.cboJeuLocal.Items.Contains(Me.cboJeuLocal.ComboBox.Text) And Me.cboJeuAdv.Items.Contains(Me.cboJeuAdv.ComboBox.Text) Then
            Call Me.UpdateGraph
        End If
    End Sub
    Private Sub LoadVersions(VpCbo1 As TD.SandBar.ComboBoxItem, VpCbo2 As TD.SandBar.ComboBoxItem, VpField1 As String, VpField2 As String)
    '---------------------------------------------------------------------------------
    'Ajoute au combobox les versions disponibles pour le jeu venant d'�tre s�lectionn�
    '---------------------------------------------------------------------------------
    Dim VpSQL As String
        VpCbo2.ComboBox.Text = mdlConstGlob.CgPerfsTotalV
        VpCbo2.Items.Clear
        VpCbo2.Items.Add(mdlConstGlob.CgPerfsVersion)
        VpCbo2.Items.Add(mdlConstGlob.CgPerfsTotalV)
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
    'V�rifie que la version (ie. la date) s�lectionn�e pour le jeu est correcte
    '--------------------------------------------------------------------------
        If VpCbo.Items.Contains(VpCbo.ComboBox.Text) And VpCbo.ComboBox.Text <> mdlConstGlob.CgPerfsVersion And VpCbo.ComboBox.Text <> mdlConstGlob.CgPerfsTotalV Then
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
            'Comptage victoires / d�faites
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
    'Affiche le diagramme de r�partitions des victoires sur les deux jeux s�lectionn�s
    'ou le diagramme d'�volution suivant les versions (selon l'�tat du menu contextuel)
    '----------------------------------------------------------------------------------
    Dim VpSQL As String
    Dim VpJeuLocal As String = Me.cboJeuLocal.ComboBox.Text.Replace("'", "''")
    Dim VpJeuAdv As String = Me.cboJeuAdv.ComboBox.Text.Replace("'", "''")
    Dim VpVicLocal As Integer = 0
    Dim VpVicAdv As Integer = 0
    Dim VpGameCounter As New clsGameCounter                     'Matrice contenant {version locale, version adverse, nombre victoires locales, nombre victoires adverses)
    Dim VpDistinctAdverseVersions() As Date
    Dim VpDistinctLocalVersions() As Date
    Dim VpHandleGamesVersions As Boolean = (Not (VpJeuLocal.StartsWith(mdlConstGlob.CgPerfsTotal) Or VpJeuAdv.StartsWith(mdlConstGlob.CgPerfsTotal))) And Me.dropGamesVersions.Checked
        'Si on ne tient pas compte des versions des jeux, le diagramme est bipolaire
        If Not VpHandleGamesVersions Then
            'Cas 1 : jeu 1 contre jeu 2
            If Not ( VpJeuLocal.StartsWith(mdlConstGlob.CgPerfsTotal) Or VpJeuAdv.StartsWith(mdlConstGlob.CgPerfsTotal) ) Then
                'Cas 1.1 : 1 contre 2
                Call Me.GetNVictoires(VpJeuLocal, VpJeuAdv, VpVicLocal, VpVicAdv, True, "", "")
                'Cas 1.2 : 2 contre 1
                Call Me.GetNVictoires(VpJeuAdv, VpJeuLocal, VpVicLocal, VpVicAdv, False, "", "")
            'Cas 2 : adversaire 1 contre adversaire 2
            ElseIf VpJeuLocal.StartsWith(mdlConstGlob.CgPerfsTotal) And VpJeuAdv.StartsWith(mdlConstGlob.CgPerfsTotal) Then
                VpJeuLocal = VpJeuLocal.Replace(mdlConstGlob.CgPerfsTotal, "")
                VpJeuAdv = VpJeuAdv.Replace(mdlConstGlob.CgPerfsTotal, "")
                'Cas 2.1 : 1 contre 2
                Call Me.GetNVictoires("", "", VpVicLocal, VpVicAdv, True, VpJeuLocal, VpJeuAdv)
                'Cas 2.2 : 2 contre 1
                Call Me.GetNVictoires("", "", VpVicLocal, VpVicAdv, False, VpJeuLocal, VpJeuAdv)
            'Cas 3 : adversaire 1 contre jeu 2
            ElseIf VpJeuLocal.StartsWith(mdlConstGlob.CgPerfsTotal)
                VpJeuLocal = VpJeuLocal.Replace(mdlConstGlob.CgPerfsTotal, "")
                'Cas 3.1 : 1 contre 2
                Call Me.GetNVictoires("", VpJeuAdv, VpVicLocal, VpVicAdv, True, VpJeuLocal, "")
                'Cas 3.2 : 2 contre 1
                Call Me.GetNVictoires("", VpJeuAdv, VpVicLocal, VpVicAdv, False, VpJeuLocal, "")
            'Cas 4 : jeu 1 contre adversaire 2
            ElseIf VpJeuAdv.StartsWith(mdlConstGlob.CgPerfsTotal) Then
                VpJeuAdv = VpJeuAdv.Replace(mdlConstGlob.CgPerfsTotal, "")
                'Cas 4.1 : 1 contre 2
                Call Me.GetNVictoires(VpJeuLocal, "", VpVicLocal, VpVicAdv, True, "", VpJeuAdv)
                'Cas 4.2 : 2 contre 1
                Call Me.GetNVictoires(VpJeuLocal, "", VpVicLocal, VpVicAdv, False, "", VpJeuAdv)
            End If
            'Diagramme de r�partition
            With Me.chartBreakDown
                .Visible = False
                .ClearData(ClearDataFlag.Data)
                .SerLeg.Clear
                .OpenData(COD.Values, 1, 2)
                .Value(0, 0) = VpVicLocal
                .Value(0, 1) = VpVicAdv
                'Inclusion des chiffres explicites dans la l�gende
                .SerLeg(0) = "Victoires " + VpJeuLocal + " (" + VpVicLocal.ToString + "/" + (VpVicLocal + VpVicAdv).ToString + ")"
                .SerLeg(1) = "Victoires " + VpJeuAdv + " (" + VpVicAdv.ToString + "/" + (VpVicLocal + VpVicAdv).ToString + ")"
                .SerLegBox = True
                .Chart3D = True
                .Gallery = Gallery.Pie
                .CloseData(COD.Values)
                .Visible = True
            End With
        'Si on tient compte des versions des jeux :
        '- en mode diagramme, il y a autant de portions que de versions de jeux confront�es
        '- en mode �volution, il y a autant de points que de versions de jeux confront�es
        Else
            'Mode diagramme
            If Me.mnuBreakDown.Checked Then
                VpSQL = "Select Victoire, JeuLocalVersion, JeuAdverseVersion From MyScores Where JeuLocal = '" + VpJeuLocal.Replace("'", "''") + "' And JeuAdverse = '" + VpJeuAdv.Replace("'", "''") + "' "
                'Cas 1 : toutes les versions du jeu local contre 1 version du jeu adverse
                If Me.cboLocalVersion.ComboBox.Text = mdlConstGlob.CgPerfsTotalV And Me.cboAdvVersion.ComboBox.Text <> mdlConstGlob.CgPerfsTotalV Then
                    VpSQL = VpSQL + "And JeuAdverseVersion = '" + Me.cboAdvVersion.ComboBox.Text + "';"
                'Cas 2 : 1 version du jeu local contre toutes les versions du jeu adverse
                ElseIf Me.cboLocalVersion.ComboBox.Text <> mdlConstGlob.CgPerfsTotalV And Me.cboAdvVersion.ComboBox.Text = mdlConstGlob.CgPerfsTotalV Then
                    VpSQL = VpSQL + "And JeuLocalVersion = '" + Me.cboLocalVersion.ComboBox.Text + "';"
                'Cas 3 : toutes les versions du jeu local contre toutes les versions du jeu adverse
                ElseIf Me.cboLocalVersion.ComboBox.Text = mdlConstGlob.CgPerfsTotalV And Me.cboAdvVersion.ComboBox.Text = mdlConstGlob.CgPerfsTotalV Then
                    VpSQL = VpSQL + ";"
                'Cas 4 : 1 version du jeu local contre 1 version du jeu adverse
                ElseIf Me.cboLocalVersion.ComboBox.Text <> mdlConstGlob.CgPerfsTotalV And Me.cboAdvVersion.ComboBox.Text <> mdlConstGlob.CgPerfsTotalV Then
                    VpSQL = VpSQL + "And JeuLocalVersion = '" + Me.cboLocalVersion.ComboBox.Text + "' And JeuAdverseVersion = '" + Me.cboAdvVersion.ComboBox.Text + "';"
                End If
                VgDBCommand.CommandText = VpSQL
                VgDBReader = VgDBCommand.ExecuteReader
                With VgDBReader
                    While .Read
                        VpGameCounter.AddGame(mdlToolbox.MyCDate(.GetValue(1).ToString), mdlToolbox.MyCDate(.GetValue(2).ToString), .GetBoolean(0))
                    End While
                    .Close
                End With
                VpDistinctLocalVersions = VpGameCounter.GetDistinctLocalVersions
                VpDistinctAdverseVersions = VpGameCounter.GetDistinctAdverseVersions
                'Diagramme de r�partition
                With Me.chartBreakDown
                    .Visible = False
                    .ClearData(ClearDataFlag.Data)
                    .SerLeg.Clear
                    .OpenData(COD.Values, 1, VpDistinctLocalVersions.Length + VpDistinctAdverseVersions.Length)
                    'Ajout des victoires locales
                    For VpI As Integer = 0 To VpDistinctLocalVersions.Length - 1
                        .Value(0, VpI) = VpGameCounter.GetNVicLocal(VpDistinctLocalVersions(VpI))
                        .SerLeg(VpI) = "Victoires " + VpJeuLocal + " (" + mdlToolbox.MyShortDateString(VpDistinctLocalVersions(VpI)) + ") : " + .Value(0, VpI).ToString
                    Next VpI
                    'Ajout des victoires adverses
                    For VpI As Integer = 0 To VpDistinctAdverseVersions.Length - 1
                        .Value(0, VpI + VpDistinctLocalVersions.Length) = VpGameCounter.GetNVicAdverse(VpDistinctAdverseVersions(VpI))
                        .SerLeg(VpI + VpDistinctLocalVersions.Length) = "Victoires " + VpJeuAdv + " (" + mdlToolbox.MyShortDateString(VpDistinctAdverseVersions(VpI)) + ") : " + .Value(0, VpI + VpDistinctLocalVersions.Length).ToString
                    Next VpI
                    .SerLegBox = True
                    .Chart3D = True
                    .Gallery = Gallery.Pie
                    .CloseData(COD.Values)
                    .Visible = True
                End With
            'Mode �volution
            Else
                VgDBCommand.CommandText = "Select Victoire, JeuLocalVersion, JeuAdverseVersion From MyScores Where JeuLocal = '" + VpJeuLocal.Replace("'", "''") + "' And JeuAdverse = '" + VpJeuAdv.Replace("'", "''") + "';"
                VgDBReader = VgDBCommand.ExecuteReader
                With VgDBReader
                    While .Read
                        VpGameCounter.AddGame(mdlToolbox.MyCDate(.GetValue(1).ToString), mdlToolbox.MyCDate(.GetValue(2).ToString), .GetBoolean(0))
                    End While
                    .Close
                End With
                VpDistinctLocalVersions = VpGameCounter.GetDistinctLocalVersions
                VpDistinctAdverseVersions = VpGameCounter.GetDistinctAdverseVersions
                'Diagramme d'�volution
                With Me.chartBreakDown
                    .Visible = False
                    .ClearData(ClearDataFlag.Data)
                    .SerLeg.Clear
                    .OpenData(COD.Values, 2, Math.Max(VpDistinctLocalVersions.Length, VpDistinctAdverseVersions.Length))
                    'Ajoute du ratio victoires/d�faites local
                    For VpI As Integer = 0 To VpDistinctLocalVersions.Length - 1
                        .Value(0, VpI) = VpGameCounter.GetNVicLocal(VpDistinctLocalVersions(VpI)) / VpGameCounter.GetNLocal(VpDistinctLocalVersions(VpI))
                    Next VpI
                    .SerLeg(0) = "Ratio local"
                    'Ajout du ratio victoires/d�faites adverse
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
    'Retourne le jeu le plus efficace � jouer contre celui pass� en param�tre
    '------------------------------------------------------------------------
    Dim VpBest As String = ""
    Dim VpMaxRatio As Single = 0
    Dim VpCur As Integer
    Dim VpTotal As Integer
        For Each VpGame As String In VpCbo.Items
            'Nombre total de parties jou�es avec le jeu courant
            VgDBCommand.CommandText = "Select Count(*) From MyScores Where " + VpField + " = '" + VpJeu.Replace("'", "''") + "' And " + VpOtherField + " = '" + VpGame.Replace("'", "''") + "';"
            VpTotal = VgDBCommand.ExecuteScalar
            'Nombre de parties gagn�es avec le jeu courant
            VgDBCommand.CommandText = "Select Count(*) From MyScores Where " + VpField + " = '" + VpJeu.Replace("'", "''") + "' And " + VpOtherField + " = '" + VpGame.Replace("'", "''") + "' And Victoire = " + If(VpVic, "True", "False") + ";"
            VpCur = VgDBCommand.ExecuteScalar
            'Si meilleur rendement, conserve le jeu courant comme le meilleur pr�tendant
            If (VpCur / VpTotal) > VpMaxRatio Then
                VpMaxRatio = VpCur / VpTotal
                VpBest = VpGame
            End If
        Next VpGame
        Return VpBest
    End Function
    Private Function GetLeastPlayed As String
    '------------------------------------
    'Retourne la rencontre la moins jou�e
    '------------------------------------
    Dim VpMin As Integer = Integer.MaxValue
    Dim VpCur As Integer
    Dim VpLeastPlayed As String = ""
        For Each VpLocal As String In Me.cboJeuLocal.Items
            For Each VpAdv As String In Me.cboJeuAdv.Items
                If Not VpLocal.StartsWith(mdlConstGlob.CgPerfsTotal) And Not VpAdv.StartsWith(mdlConstGlob.CgPerfsTotal) And VpLocal <> VpAdv And mdlToolbox.GetOwner(VpLocal) <> mdlToolbox.GetOwner(VpAdv) Then
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
    'Ajoute les jeux d�j� connus (de par une saisie ant�rieure) dans les listes d�roulantes
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
    'G�n�re un rapport Excel sur l'efficacit� des jeux saisis dans la base de donn�es
    '--------------------------------------------------------------------------------
    Dim VpExcelApp As Object
    Dim VpRow As Integer = 3
    Dim VpK As Integer
    Dim VpL As Integer
    Dim VpM As Integer
    Dim VpJ1 As String
    Dim VpJ2 As String
    Dim VpJn1 As Integer
    Dim VpJn2 As Integer
    Dim VpJv1 As Integer
    Dim VpJv2 As Integer
    Dim VpDecks1() As String = clsPerformances.GetAllDecks
    Dim VpDecks2() As String = clsPerformances.GetActiveDecks
    Dim VpMat1() As Single = clsPerformances.GetSimpleRatioMatrix(VpDecks1)
    Dim VpMat2() As Single = clsPerformances.Reshape(clsPerformances.GetRatioMatrix(VpDecks2))
    Dim VpRef As Single = clsPerformances.GetMeanPrice(VpDecks2) / 50
    Dim VpPrice As Single
    Dim VpVersusMatV1() As Integer
    Dim VpVersusMatD1() As Integer
    Dim VpVersusMatV2() As Integer
    Dim VpVersusMatD2() As Integer
    Dim VpTotV1 As Integer
    Dim VpTotV2 As Integer
    Dim VpTotMat As New Dictionary(Of String, Single)
    Dim VpEfficiencies As New List(Of clsEfficiency)
    Dim VpTournoi As clsMagicTournament
        Try
            VpExcelApp = CreateObject("Excel.Application")
        Catch
            Call mdlToolbox.ShowWarning("Aucune installation de Microsoft Excel n'a �t� d�tect�e sur votre syst�me..." + vbCrLf + "Impossible de continuer.")
            Exit Sub
        End Try
        With VpExcelApp
            .Workbooks.Add
            While .Sheets.Count < 4
                .Sheets.Add
            End While
            'Partie 1 : efficacit� dans l'absolu
            With .Sheets(1)
                .Name = "Efficience"
                .Cells(1, 1) = "Nom du deck"
                .Cells(1, 2) = "Prix du deck"
                .Cells(1, 3) = "Performance (mode 1)"
                .Cells(1, 4) = "Performance (mode 2)"
                .Cells(1, 5) = "Esp�rance de prix (pour cette performance)"
                .Cells(1, 6) = "Esp�rance de performance (pour ce prix)"
                .Cells(1, 7) = "Facteur d'efficacit� (meilleur vers 0)"
                .Rows(1).EntireRow.Font.Bold = True
                For VpI As Integer = 0 To VpDecks1.Length - 1
                    For VpJ As Integer = 0 To VpDecks2.Length - 1
                        If VpDecks1(VpI) = VpDecks2(VpJ) Then
                            VpPrice = clsPerformances.GetPrice(VpDecks1(VpI))
                            If VpPrice <> -1 Then
                                VpEfficiencies.Add(New clsEfficiency(VpDecks1(VpI), VpPrice, VpMat1(VpI), VpMat2(VpJ), VpRef * VpMat2(VpJ), Math.Min(VpPrice / VpRef, 100), (VpPrice / VpMat2(VpJ)) / VpRef))
                            End If
                        End If
                    Next VpJ
                Next VpI
                VpEfficiencies.Sort(New clsEfficiency.clsEfficiencyComparer)
                For Each VpEfficiency As clsEfficiency In VpEfficiencies
                    .Cells(VpRow, 1) = VpEfficiency.Name
                    .Cells(VpRow, 2) = VpEfficiency.Price
                    .Cells(VpRow, 3) = Format(VpEfficiency.Perfs1, "0.0") + "%"
                    .Cells(VpRow, 4) = Format(VpEfficiency.Perfs2, "0.0") + "%"
                    .Cells(VpRow, 5) = VpEfficiency.EspPrice
                    .Cells(VpRow, 6) = Format(VpEfficiency.EspPerfs, "0.0") + "%"
                    .Cells(VpRow, 7) = Format(VpEfficiency.Efficiency, "0.00")
                    VpRow = VpRow + 1
                Next VpEfficiency
                'Formatage particulier
                For VpI As Integer = 1 To 7
                    .Columns(VpI).EntireColumn.AutoFit
                    If VpI = 2 Or VpI = 5 Then
                        .Columns(VpI).EntireColumn.NumberFormat = "0,00 �"
                    End If
                Next VpI
            End With
            'Partie 2 : r�sultats matches versus
            With .Sheets(2)
                .Name = "Matches vs."
                VpRow = 1
                VpM = mdlToolbox.GetAdvCount
                For VpI As Integer = 1 To VpM
                    For VpJ As Integer = VpI + 1 To VpM
                        VpJ1 = mdlToolbox.GetAdvName(VpI)
                        VpJ2 = mdlToolbox.GetAdvName(VpJ)
                        VpJn1 = mdlToolbox.GetAdvDecksCount(VpJ1)
                        VpJn2 = mdlToolbox.GetAdvDecksCount(VpJ2)
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
                            VpTotMat.Add(VpJ1 + VpJ2, VpTotV1 / (VpTotV1 + VpTotV2))
                            VpTotMat.Add(VpJ2 + VpJ1, VpTotV2 / (VpTotV1 + VpTotV2))
                            .Cells(VpK + VpRow + 1, VpVersusMatV2.Length + 2) = VpTotV1.ToString + "V / " + VpTotV2.ToString + "D"
                            .Cells(VpK + VpRow + 1, VpVersusMatV2.Length + 2).Interior.ColorIndex = 48
                            .Cells(VpK + VpRow + 1, VpVersusMatV2.Length + 2).Font.Bold = True
                            VpRow = VpRow + VpK + 3
                        End If
                    Next VpJ
                Next VpI
                'Formatage particulier
                For VpI As Integer = 1 To VpDecks1.Length + 1
                    .Columns(VpI).EntireColumn.AutoFit
                Next VpI
            End With
            'Partie 3 : r�sultats joueurs versus
            With .Sheets(3)
                .Name = "Joueurs vs."
                .Rows(1).EntireRow.Font.Bold = True
                .Columns(1).EntireColumn.Font.Bold = True
                For VpI As Integer = 1 To VpM
                    For VpJ As Integer = 1 To VpM
                        If VpI <> VpJ Then
                            VpJ1 = mdlToolbox.GetAdvName(VpI)
                            VpJ2 = mdlToolbox.GetAdvName(VpJ)
                            .Cells(1 + VpI, 1) = VpJ1
                            .Cells(1, 1 + VpJ) = VpJ2
                            If VpTotMat.ContainsKey(VpJ1 + VpJ2) Then
                                .Cells(1 + VpI, 1 + VpJ) = VpTotMat.Item(VpJ1 + VpJ2)
                                .Cells(1 + VpI, 1 + VpJ).NumberFormat = "0%"
                            Else
                                .Cells(1 + VpI, 1 + VpJ).Interior.ColorIndex = 48
                            End If
                        Else
                            .Cells(1 + VpI, 1 + VpJ).Interior.ColorIndex = 48
                        End If
                    Next VpJ
                Next VpI
                For VpI As Integer = 1 To VpM
                    .Columns(VpI).EntireColumn.AutoFit
                Next VpI
            End With
            'Partie 4 : fr�quence des matches
            With .Sheets(4)
                .Name = "Fr�quences"
                VpRow = 1
                For VpI As Integer = 1 To VpM
                    For VpJ As Integer = VpI + 1 To VpM
                        VpJ1 = mdlToolbox.GetAdvName(VpI)
                        VpJ2 = mdlToolbox.GetAdvName(VpJ)
                        VpTournoi = New clsMagicTournament
                        If mdlToolbox.GetAdvDecksCount(VpJ1) > 0 And mdlToolbox.GetAdvDecksCount(VpJ2) > 0 Then
                            .Cells(VpRow, 1) = VpJ1 + " vs. " + VpJ2
                            .Rows(VpRow).EntireRow.Font.Bold = True
                            VpRow = VpRow + 1
                            For Each VpDeck1 As String In clsPerformances.GetAdvDecks(VpJ1)
                                For Each VpDeck2 As String In clsPerformances.GetAdvDecks(VpJ2)
                                    If mdlToolbox.GetDeckFormat(VpDeck1) = mdlToolbox.GetDeckFormat(VpDeck2) Then
                                        VpTournoi.CreateMatchup(VpDeck1, VpDeck2, clsPerformances.GetNVictoires(VpDeck1, VpDeck2), clsPerformances.GetNVictoires(VpDeck2, VpDeck1))
                                    End If
                                Next VpDeck2
                            Next VpDeck1
                            For Each VpDuel As clsMatchup In VpTournoi.GetDuelList
                                .Cells(VpRow, 1) = VpDuel.Deck1 + " / " + VpDuel.Deck2
                                .Cells(VpRow, 2) = VpDuel.DeckConfrontations
                                .Cells(VpRow, 3) = VpDuel.Deck1Victory
                                .Cells(VpRow, 4) = VpDuel.Deck2Victory
                                VpRow += 1
                            Next VpDuel
                            VpRow += 1
                        End If
                    Next VpJ
                Next VpI
                'Formatage particulier
                For VpI As Integer = 1 To 4
                    .Columns(VpI).EntireColumn.AutoFit
                Next VpI
            End With
            Call mdlToolbox.ShowInformation("G�n�ration termin�e." + vbCrLf + "NB. Ces calculs n'ont de sens que si tous les jeux en pr�sence ont �t� saisis dans la base (afin d'en conna�tre leur prix) et si suffisamment de parties entre tous les decks ont �t� disput�es.")
            .Visible = True
        End With
    End Sub
    Private Sub GetAllPlayed
    '-----------------------------------
    'Affiche le nombre de parties jou�es
    '-----------------------------------
    Dim VpStr As String
        VgDBCommand.CommandText = "Select Count(*) From MyScores;"
        VpStr = "| " + VgDBCommand.ExecuteScalar.ToString + " parties ("
        VgDBCommand.CommandText = "Select Count(*) From MyScores Where JeuLocal Not In (Select GameName From MyGamesID) Or JeuAdverse Not In (Select GameName From MyGamesID);"
        Me.lblAllPlayed.Text = VpStr + VgDBCommand.ExecuteScalar.ToString + " ind�pendantes)"
    End Sub
    #End Region
    #Region "Ev�nements"
    Private Sub AddGameClick(ByVal sender As Object, ByVal e As EventArgs)
        If Not Me.cboJeuLocal.Items.Contains(sender.Text) Then
            Me.cboJeuLocal.Items.Add(sender.Text)
        End If
        If Not Me.cboJeuAdv.Items.Contains(sender.Text) Then
            Me.cboJeuAdv.Items.Add(sender.Text)
        End If
    End Sub
    Private Sub AddResult(VpJeuLocal As String, VpJeuAdv As String, VpDate As Date, VpVictoire As Boolean)
        If VpJeuLocal.StartsWith(mdlConstGlob.CgPerfsTotal) Or VpJeuAdv.StartsWith(mdlConstGlob.CgPerfsTotal) Then Exit Sub
        If Me.cboJeuLocal.Items.Contains(VpJeuLocal) And Me.cboJeuAdv.Items.Contains(VpJeuAdv) Then
            If Me.dropGamesVersions.Checked Then
                VgDBCommand.CommandText = "Insert Into MyScores(JeuLocal, JeuLocalVersion, JeuAdverse, JeuAdverseVersion, DuelDate, Victoire) Values ('" + VpJeuLocal.Replace("'", "''") + "', " + Me.ValidateVersion(Me.cboLocalVersion) + ", '" + VpJeuAdv.Replace("'", "''") + "', " + Me.ValidateVersion(Me.cboAdvVersion) + ", ', Null, '" + VpDate.ToShortDateString + "', " + If(VpVictoire, "True", "False") + ");"
            Else
                VgDBCommand.CommandText = "Insert Into MyScores(JeuLocal, JeuLocalVersion, JeuAdverse, JeuAdverseVersion, DuelDate, Victoire) Values ('" + VpJeuLocal.Replace("'", "''") + "', Null, '" + VpJeuAdv.Replace("'", "''") + "', Null, '" + VpDate.ToShortDateString + "', " + If(VpVictoire, "True", "False") + ");"
            End If
            VgDBCommand.ExecuteNonQuery
            Call Me.UpdateGraph
        Else
            Call mdlToolbox.ShowWarning("S�lectionner deux jeux pr�sents dans les listes d�roulantes avant d'en enregistrer le r�sultat...")
        End If
        Call Me.GetAllPlayed
    End Sub
    Private Sub RemoveResult(VpJeuLocal As String, VpJeuAdv As String, VpVictoire As Boolean)
        If VpJeuLocal.StartsWith(mdlConstGlob.CgPerfsTotal) Or VpJeuAdv.StartsWith(mdlConstGlob.CgPerfsTotal) Then Exit Sub
        If Me.cboJeuLocal.Items.Contains(VpJeuLocal) And Me.cboJeuAdv.Items.Contains(VpJeuAdv) Then
            If Me.dropGamesVersions.Checked Then
                VgDBCommand.CommandText = "Delete * From (Select Top 1 * From (Select * From MyScores Where JeuLocal = '" + VpJeuLocal.Replace("'", "''") + "' And JeuLocalVersion = " + Me.ValidateVersion(Me.cboLocalVersion) + " And JeuAdverse = '" + VpJeuAdv.Replace("'", "''") + "' And JeuAdverseVersion = " + Me.ValidateVersion(Me.cboAdvVersion) + " And Victoire = " + If(VpVictoire, "True", "False") + " Order By DuelDate Desc));"
            Else
                VgDBCommand.CommandText = "Delete * From (Select Top 1 * From (Select * From MyScores Where JeuLocal = '" + VpJeuLocal.Replace("'", "''") + "' And JeuAdverse = '" + VpJeuAdv.Replace("'", "''") + "' And Victoire = " + If(VpVictoire, "True", "False") + " Order By DuelDate Desc));"
            End If
            Try
                VgDBCommand.ExecuteNonQuery
            Catch
                Call mdlToolbox.ShowWarning("Impossible d'accomplir l'op�ration...")
            End Try
            Call Me.UpdateGraph
        Else
            Call mdlToolbox.ShowWarning("S�lectionner deux jeux pr�sents dans les listes d�roulantes avant d'en enregistrer le r�sultat...")
        End If
        Call Me.GetAllPlayed
    End Sub
    Sub DropAddGameOtherClick(ByVal sender As Object, ByVal e As EventArgs)
    Dim VpGameName As String = InputBox("Quel est le nom du jeu ind�pendant (disponible dans les deux colonnes) � ajouter ?", "Nouveau jeu", mdlConstGlob.CgDefaultDeckName)
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
        Call Me.AddResult(Me.cboJeuLocal.ComboBox.Text, Me.cboJeuAdv.ComboBox.Text, Me.calGames.Value, True)
    End Sub
    Sub BtDefOkActivate(ByVal sender As Object, ByVal e As EventArgs)
        Call Me.AddResult(Me.cboJeuLocal.ComboBox.Text, Me.cboJeuAdv.ComboBox.Text, Me.calGames.Value, False)
    End Sub
    Sub MySelectedIndexChanged(VpCbo1 As TD.SandBar.ComboBoxItem, VpCbo2 As TD.SandBar.ComboBoxItem)
    Dim VpPickDate As New frmCalendar
        If VpCbo1.Items.Contains(VpCbo1.ComboBox.Text) And VpCbo2.ComboBox.Text = mdlConstGlob.CgPerfsVersion Then
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
        'Astuce horrible pour contourner un bug de mise � l'�chelle automatique en fonction de la densit� de pixels
        Me.Width = 461 * Me.CreateGraphics().DpiX / 96
        Me.cboJeuAdv.MinimumControlWidth = 130 * Me.CreateGraphics().DpiX / 96
        Me.cboJeuLocal.MinimumControlWidth = 130 * Me.CreateGraphics().DpiX / 96
    End Sub
    Sub FrmPerfsResize(sender As Object, e As EventArgs)
        Me.calGames.Top = Me.strStatus.Top + 3
    End Sub
    Sub BtEfficiencyActivate(ByVal sender As Object, ByVal e As EventArgs)
        If mdlToolbox.ShowQuestion("G�n�rer un rapport complet sous Excel ?" + vbCrLf + "Ceci peut prendre plusieurs secondes...")= System.Windows.Forms.DialogResult.Yes Then
            Cursor.Current = Cursors.WaitCursor
            Call Me.ExcelEfficiency
        End If
    End Sub
    #End Region
End Class
