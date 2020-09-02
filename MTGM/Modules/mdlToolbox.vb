Imports System.Data
Imports System.Data.OleDb
Imports System.Text
Imports System.Net
Imports System.IO
Imports System.ComponentModel
Imports System.Globalization
Imports SourceGrid2
Imports Cells = SourceGrid2.Cells.Real
Imports ICSharpCode.SharpZipLib.Zip
Public Module mdlToolbox
    Public WithEvents VgTray As NotifyIcon
    Public WithEvents VgTimer As Timer
    Public WithEvents VgClient As New WebClient
    #Region "API Win32"    
    Public Declare Function OpenIcon            Lib "user32" (ByVal hwnd As Long) As Long
    Public Declare Function SetForegroundWindow Lib "user32" (ByVal hwnd As Long) As Long
    Public Declare Function SendMessageA        Lib "user32" (ByVal hWnd As IntPtr, ByVal wMsg As UInt32, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
    #End Region
    #Region "Gestion de la base de données"
    Public Function DBOpen(VpPath As String) As Boolean
    '--------------------------------------------------------------------------------
    'Essaie d'ouvrir la base de données spécifiée en paramètre et renvoie le résultat
    '--------------------------------------------------------------------------------
        If Not IO.File.Exists(VpPath) Then
            Return False
        Else
            VgDB = New OleDbConnection(CgStrConn(CInt(VgOptions.VgSettings.DBProvider)) + VpPath)
            Try
                VgDB.Open
                VgDBCommand.Connection = VgDB
                VgDBCommand.CommandType = CommandType.Text
                If Not DBVersion Then
                    VgDB.Close
                    VgDB.Dispose
                    VgDB = Nothing
                    Return False
                Else
                    Return True
                End If
            Catch VpErr As Exception
                Call ShowWarning("Impossible d'ouvrir la base de données sélectionnée..." + vbCrLf + vbCrLf + "Détails : " + VpErr.Message)
            End Try
        End If
        Return False
    End Function
    Private Function DBVersion As Boolean
    '------------------------------------
    'Vérifie la version de la BDD ouverte
    '------------------------------------
    Dim VpDBVersion As eDBVersion = eDBVersion.Unknown
    Dim VpSchemaTable As DataTable = VgDB.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, New Object() {Nothing, Nothing, Nothing, "TABLE"})
    Dim VpTablesCount As Integer = VpSchemaTable.Rows.Count
        'Détermination de la version de la base de données suivant le nombre d'éléments qu'elle contient
        If VpTablesCount >= 8 Then
            VpSchemaTable = VgDB.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, New Object() {Nothing, Nothing, "MyScores", Nothing})
            If Not VpSchemaTable.Rows(1)!COLUMN_NAME.ToString.StartsWith("JeuAdverse") Then
                'Si on est ici, BDD version 2
                VpDBVersion = eDBVersion.BDD_v2
            ElseIf VpTablesCount < 10 Then
                'Si on est ici, BDD version 3
                VpDBVersion = eDBVersion.BDD_v3
            ElseIf VpTablesCount < 12 Then
                'Si on est ici, BDD version 4
                VpDBVersion = eDBVersion.BDD_v4
            ElseIf VpTablesCount < 13 Then
                'Si on est ici, BDD version 5
                VpDBVersion = eDBVersion.BDD_v5
            ElseIf VpTablesCount < 14 Then
                'Si on est ici, BDD version 6
                VpDBVersion = eDBVersion.BDD_v6
            ElseIf VpTablesCount < 15 Then
                'Si on est ici, BDD version 7
                VpDBVersion = eDBVersion.BDD_v7
            ElseIf VpTablesCount < 16 Then
                'Si on est ici, BDD version 8
                VpDBVersion = eDBVersion.BDD_v8
            Else
                VpSchemaTable = VgDB.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, New Object() {Nothing, Nothing, "Card", Nothing})
                If VpSchemaTable.Rows.Count <> 22 And VpSchemaTable.Rows.Count <> 23 And CInt(VpSchemaTable.Rows(11)!DATA_TYPE) <> 4 Then
                    'Si on est ici, BDD version 9
                    VpDBVersion = eDBVersion.BDD_v9
                Else
                    VpSchemaTable = VgDB.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, New Object() {Nothing, Nothing, "Autorisations", Nothing})
                    If VpSchemaTable.Rows.Count <> 8 And VpSchemaTable.Rows.Count <> 9 And VpSchemaTable.Rows.Count <> 12 Then
                        'Si on est ici, BDD version 10
                        VpDBVersion = eDBVersion.BDD_v10
                    Else
                        If VpTablesCount < 17 Then
                            'Si on est ici, BDD version 11
                            VpDBVersion = eDBVersion.BDD_v11
                        ElseIf VpTablesCount < 18 Then
                            'Si on est ici, BDD version 12
                            VpDBVersion = eDBVersion.BDD_v12
                        Else
                            VpSchemaTable = VgDB.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, New Object() {Nothing, Nothing, "MyGamesID", Nothing})
                            If VpSchemaTable.Rows.Count <> 6 And VpSchemaTable.Rows.Count <> 8 Then
                                'Si on est ici, BDD version 13
                                VpDBVersion = eDBVersion.BDD_v13
                            Else
                                VpSchemaTable = VgDB.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, New Object() {Nothing, Nothing, "MyGames", Nothing})
                                If VpSchemaTable.Rows.Count <> 5 Then
                                    'Si on est ici, BDD version 14
                                    VpDBVersion = eDBVersion.BDD_v14
                                Else
                                    VpSchemaTable = VgDB.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, New Object() {Nothing, Nothing, "Series", Nothing})
                                    If VpSchemaTable.Rows.Count <> 33 And VpSchemaTable.Rows.Count <> 34 Then
                                        'Si on est ici, BDD version 15
                                        VpDBVersion = eDBVersion.BDD_v15
                                    Else
                                        VpSchemaTable = VgDB.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, New Object() {Nothing, Nothing, "Autorisations", Nothing})
                                        If VpSchemaTable.Rows.Count <> 9 And VpSchemaTable.Rows.Count <> 12 Then
                                            'Si on est ici, BDD version 16
                                            VpDBVersion = eDBVersion.BDD_v16
                                        Else
                                            VpSchemaTable = VgDB.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, New Object() {Nothing, Nothing, "MyGamesID", Nothing})
                                            If VpSchemaTable.Rows.Count <> 8 Then
                                                'Si on est ici, BDD version 17
                                                VpDBVersion = eDBVersion.BDD_v17
                                            Else
                                                VpSchemaTable = VgDB.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, New Object() {Nothing, Nothing, "Card", Nothing})
                                                If VpSchemaTable.Rows.Count <> 22 And VpSchemaTable.Rows.Count <> 23 Then
                                                    'Si on est ici, BDD version 18
                                                    VpDBVersion = eDBVersion.BDD_v18
                                                Else
                                                    VpSchemaTable = VgDB.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, New Object() {Nothing, Nothing, "Autorisations", Nothing})
                                                    If VpSchemaTable.Rows.Count <> 12 Then
                                                        'Si on est ici, BDD version 19
                                                        VpDBVersion = eDBVersion.BDD_v19
                                                    Else
                                                        VpSchemaTable = VgDB.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, New Object() {Nothing, Nothing, "Card", Nothing})
                                                        If VpSchemaTable.Rows.Count <> 23 Then
                                                            'Si on est ici, BDD version 20
                                                            VpDBVersion = eDBVersion.BDD_v20
                                                        Else
                                                            'Si on est ici, BDD version 21
                                                            VpDBVersion = eDBVersion.BDD_v21
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        Else
            'Si on est ici, BDD version 1
            VpDBVersion = eDBVersion.BDD_v1
        End If
        'Actions à effectuer en conséquence
        If VpDBVersion = eDBVersion.Unknown Then        'Version inconnue
            Return False
        ElseIf VpDBVersion = eDBVersion.BDD_v21 Then    'Dernière version
            Return True
        Else                                            'Versions intermédiaires
            If ShowQuestion("La base de données (v" + CInt(VpDBVersion).ToString + ") doit être mise à jour pour devenir compatible avec la nouvelle version du logiciel (v21)..." + vbCrlf + "Continuer ?") = DialogResult.Yes Then
                Try
                    'Passage version 1 à 2
                    If CInt(VpDBVersion) < 2 Then
                        VgDBCommand.CommandText = "Create Table MyScores (JeuLocal Text(50) With Compression, JeuAdverse Text(50) With Compression, Victoire Bit);"
                        VgDBCommand.ExecuteNonQuery
                    End If
                    'Passage version 2 à 3
                    If CInt(VpDBVersion) < 3 Then
                        VgDBCommand.CommandText = "Alter Table MyScores Add JeuLocalVersion Text(10) With Compression;"
                        VgDBCommand.ExecuteNonQuery
                        VgDBCommand.CommandText = "Alter Table MyScores Add JeuAdverseVersion Text(10) With Compression;"
                        VgDBCommand.ExecuteNonQuery
                    End If
                    'Passage version 3 à 4
                    If CInt(VpDBVersion) < 4 Then
                        Try
                            VgDBCommand.CommandText = "Create Table MyGamesID (GameID Integer, GameName Text(50) With Compression);"
                            VgDBCommand.ExecuteNonQuery
                        Catch
                        End Try
                    End If
                    'Passage version 4 à 5
                    If CInt(VpDBVersion) < 5 Then
                        VgDBCommand.CommandText = "Create Table MySpecialUses (EffortID Integer, EffetID Integer, Card Text(80) With Compression, Effort Text(255) With Compression, Effet Text(255) With Compression);"
                        VgDBCommand.ExecuteNonQuery
                        VgDBCommand.CommandText = "Create Table SpecialUse (SpecID Integer, IsEffort Bit, Description Text(255) With Compression);"
                        VgDBCommand.ExecuteNonQuery
                        VgDBCommand.CommandText = "Alter Table MyScores Add IsMixte Bit;"
                        VgDBCommand.ExecuteNonQuery
                    End If
                    'Passage version 5 à 6
                    If CInt(VpDBVersion) < 6 Then
                        VgDBCommand.CommandText = "Create Table TextesFR (CardName Text(80) With Compression, TexteFR Memo);"
                        VgDBCommand.ExecuteNonQuery
                    End If
                    'Passage version 6 à 7
                    If CInt(VpDBVersion) < 7 Then
                        VgDBCommand.CommandText = "Create Table Autorisations (Title Text(80) With Compression, T1 Bit, T1r Bit, T15 Bit, T1x Bit, T2 Bit, Bloc Bit);"
                        VgDBCommand.ExecuteNonQuery
                    End If
                    'Passage version 7 à 8
                    If CInt(VpDBVersion) < 8 Then
                        VgDBCommand.CommandText = "Create Table PricesHistory (EncNbr Long, PriceDate Date, Price Single);"
                        VgDBCommand.ExecuteNonQuery
                    End If
                    'Passage version 8 à 9
                    If CInt(VpDBVersion) < 9 Then
                        VgDBCommand.CommandText = "Create Table MyAdversairesID (AdvID Long, AdvName Text(255) With Compression);"
                        VgDBCommand.ExecuteNonQuery
                        VgDBCommand.CommandText = "Insert Into MyAdversairesID(AdvID, AdvName) Values (0, '" + CgMe + "');"
                        VgDBCommand.ExecuteNonQuery
                        VgDBCommand.CommandText = "Alter Table MyGamesID Add AdvID Long;"
                        VgDBCommand.ExecuteNonQuery
                        VgDBCommand.CommandText = "Update MyGamesID Set AdvID = 0;"
                        VgDBCommand.ExecuteNonQuery
                        VgDBCommand.CommandText = "Alter Table MyScores Drop Column IsMixte;"
                        VgDBCommand.ExecuteNonQuery
                    End If
                    'Passage version 9 à 10
                    If CInt(VpDBVersion) < 10 Then
                        Call DBChangeType("Card", "Price", "Single", "Val")
                        Call DBChangeType("Card", "myPrice", "Integer", "Int")
                        Call DBChangeType("Spell", "myCost", "Integer", "Int")
                        VgDBCommand.CommandText = "Alter Table MyGames Add Foil Bit;"
                        VgDBCommand.ExecuteNonQuery
                        VgDBCommand.CommandText = "Alter Table MyCollection Add Foil Bit;"
                        VgDBCommand.ExecuteNonQuery
                        VgDBCommand.CommandText = "Alter Table PricesHistory Add Foil Bit;"
                        VgDBCommand.ExecuteNonQuery
                        Try     'normalement la clé primaire n'existe pas (mais on ne sait jamais), d'où la trappe pour éviter une exception
                            VgDBCommand.CommandText = "Drop Index PrimaryKey On MyCollection;"
                            VgDBCommand.ExecuteNonQuery
                        Catch
                        End Try
                        VgDBCommand.CommandText = "Drop Index EncNbr On MyCollection;"
                        VgDBCommand.ExecuteNonQuery
                        VgDBCommand.CommandText = "Create Index EncNbr On MyCollection (EncNbr);"
                        VgDBCommand.ExecuteNonQuery
                    End If
                    'Passage version 10 à 11
                    If CInt(VpDBVersion) < 11 Then
                        VgDBCommand.CommandText = "Alter Table Autorisations Add M Bit;"
                        VgDBCommand.ExecuteNonQuery
                    End If
                    'Passage version 11 à 12
                    If CInt(VpDBVersion) < 12 Then
                        VgDBCommand.CommandText = "Alter Table Card Add SpecialDoubleCard Bit;"
                        VgDBCommand.ExecuteNonQuery
                        VgDBCommand.CommandText = "Update Card Set SpecialDoubleCard = False;"
                        VgDBCommand.ExecuteNonQuery
                        VgDBCommand.CommandText = "Create Table CardDouble (EncNbrDownFace Long, EncNbrTopFace Long);"
                        VgDBCommand.ExecuteNonQuery
                    End If
                    'Passage version 12 à 13
                    If CInt(VpDBVersion) < 13 Then
                        VgDBCommand.CommandText = "Create Table SubTypes (SubTypeVO Text(32) With Compression, SubTypeVF Text(32) With Compression);"
                        VgDBCommand.ExecuteNonQuery
                        VgDBCommand.CommandText = "Alter Table Series Add SeriesNM_FR Text(50) With Compression;"
                        VgDBCommand.ExecuteNonQuery
                        VgDBCommand.CommandText = "Update Series Set SeriesNM_FR = SeriesNM;"
                        VgDBCommand.ExecuteNonQuery
                    End If
                    'Passage version 13 à 14
                    If CInt(VpDBVersion) < 14 Then
                        VgDBCommand.CommandText = "Alter Table MyGamesID Add GameDate Date;"
                        VgDBCommand.ExecuteNonQuery
                        VgDBCommand.CommandText = "Alter Table MyGamesID Add GameFormat Text(63) With Compression;"
                        VgDBCommand.ExecuteNonQuery
                        VgDBCommand.CommandText = "Alter Table MyGamesID Add GameDescription Memo With Compression;"
                        VgDBCommand.ExecuteNonQuery
                        VgDBCommand.CommandText = "Update MyGamesID Set GameDate = '" + Now.ToShortDateString + "';"
                        VgDBCommand.ExecuteNonQuery
                        VgDBCommand.CommandText = "Update MyGamesID Set GameFormat = '" + mdlConstGlob.CgDefaultFormat + "';"
                        VgDBCommand.ExecuteNonQuery
                    End If
                    'Passage version 14 à 15
                    If CInt(VpDBVersion) < 15 Then
                        VgDBCommand.CommandText = "Alter Table MyGames Add Reserve Bit;"
                        VgDBCommand.ExecuteNonQuery
                    End If
                    'Passage version 15 à 16
                    If CInt(VpDBVersion) < 16 Then
                        VgDBCommand.CommandText = "Alter Table Series Add SeriesCD_MO Text(3) With Compression;"
                        VgDBCommand.ExecuteNonQuery
                        VgDBCommand.CommandText = "Alter Table Series Add SeriesCD_MW Text(3) With Compression;"
                        VgDBCommand.ExecuteNonQuery
                        VgDBCommand.CommandText = "Update Series Set SeriesCD_MO = SeriesCD;"
                        VgDBCommand.ExecuteNonQuery
                        VgDBCommand.CommandText = "Update Series Set SeriesCD_MW = SeriesCD;"
                        VgDBCommand.ExecuteNonQuery
                        Call ShowInformation("Vous devriez mettre à jour les en-têtes (Fichier / Ajouter des éditions / Mettre à jour les en-têtes) pour assurer une compatibilité optimale avec les autres formats de logiciels Magic...")
                    End If
                    'Passage version 16 à 17
                    If CInt(VpDBVersion) < 17 Then
                        VgDBCommand.CommandText = "Alter Table Autorisations Drop Column T1x;"
                        VgDBCommand.ExecuteNonQuery
                        VgDBCommand.CommandText = "Alter Table Autorisations Add [1V1] Bit;"
                        VgDBCommand.ExecuteNonQuery
                        VgDBCommand.CommandText = "Alter Table Autorisations Add Multi Bit;"
                        VgDBCommand.ExecuteNonQuery
                    End If
                    'Passage version 17 à 18
                    If CInt(VpDBVersion) < 18 Then
                        VgDBCommand.CommandText = "Alter Table MyGamesID Add Parent Long;"
                        VgDBCommand.ExecuteNonQuery
                        VgDBCommand.CommandText = "Alter Table MyGamesID Add IsFolder Bit;"
                        VgDBCommand.ExecuteNonQuery
                        Call ShowInformation("Vous pouvez maintenant classer vos decks dans des dossiers !")
                    End If
                    'Passage version 18 à 19
                    If CInt(VpDBVersion) < 19 Then
                        VgDBCommand.CommandText = "Alter Table Card Add MultiverseId Long;"
                        VgDBCommand.ExecuteNonQuery
                        VgDBCommand.CommandText = "Update Card Set MultiverseId = 0;"
                        VgDBCommand.ExecuteNonQuery
                        Call ShowInformation("Vous pouvez maintenant avoir l'image de chaque carte selon son édition (à activer dans les Préférences) !")
                    End If
                    'Passage version 19 à 20
                    If CInt(VpDBVersion) < 20 Then
                        VgDBCommand.CommandText = "Alter Table Autorisations Add MTGO Bit;"
                        VgDBCommand.ExecuteNonQuery
                        VgDBCommand.CommandText = "Alter Table Autorisations Add Blocoff Bit;"
                        VgDBCommand.ExecuteNonQuery
                        VgDBCommand.CommandText = "Alter Table Autorisations Add MTGOoff Bit;"
                        VgDBCommand.ExecuteNonQuery
                        VgDBCommand.CommandText = "Update Autorisations Set MTGOoff = 1;"
                        VgDBCommand.ExecuteNonQuery
                    End If
                    'Passage version 20 à 21
                    If CInt(VpDBVersion) < 21 Then
                        VgDBCommand.CommandText = "Alter Table Card Add UrzaId Long;"
                        VgDBCommand.ExecuteNonQuery
                        VgDBCommand.CommandText = "Update Card Set UrzaId = 0;"
                        VgDBCommand.ExecuteNonQuery
                        VgDBCommand.CommandText = "Alter Table Series Add SeriesNM_UG Text(50) With Compression;"
                        VgDBCommand.ExecuteNonQuery
                        VgDBCommand.CommandText = "Update Series Set SeriesNM_UG = SeriesNM;"
                        VgDBCommand.ExecuteNonQuery
                        Call ShowInformation("Vous devriez mettre à jour les en-têtes (Fichier / Ajouter des éditions / Mettre à jour les en-têtes) ainsi que les identifiants Multiverse pour assurer une compatibilité optimale avec Urza Gatherer...")
                    End If
                Catch
                    Call ShowWarning("Un problème est survenu pendant la mise à jour de la base de données...")
                    Return False
                End Try
                Return True
            Else
                Return False
            End If
        End If
    End Function
    Private Sub DBChangeType(VpTable As String, VpField As String, VpType As String, VpCaster As String)
    '-----------------------------------------------------------------------------------------------------------------
    'Jet-SQL ne supporte pas le changement de type d'une colonne avec conversion implicite, d'où la routine ci-dessous
    '-----------------------------------------------------------------------------------------------------------------
        VgDBCommand.CommandText = "Alter Table " + VpTable + " Add " + VpField + "2 " + VpType + ";"
        VgDBCommand.ExecuteNonQuery
        VgDBCommand.CommandText = "Update " + VpTable + " Set " + VpField + "2 = " + VpCaster + "(" + VpField + ");"
        VgDBCommand.ExecuteNonQuery
        VgDBCommand.CommandText = "Alter Table " + VpTable + " Drop Column " + VpField + ";"
        VgDBCommand.ExecuteNonQuery
        VgDBCommand.CommandText = "Alter Table " + VpTable + " Add " + VpField + " " + VpType + ";"
        VgDBCommand.ExecuteNonQuery
        VgDBCommand.CommandText = "Update " + VpTable + " Set " + VpField + " = " + VpField + "2;"
        VgDBCommand.ExecuteNonQuery
        VgDBCommand.CommandText = "Alter Table " + VpTable + " Drop Column " + VpField + "2;"
        VgDBCommand.ExecuteNonQuery
    End Sub
    Public Function DBOK As Boolean
        If VgDB Is Nothing Then
            Call ShowWarning("Aucune base de données n'a été sélectionnée...")
            Return False
        ElseIf MainForm.VgMe.IsMainReaderBusy Then
            Return False
        Else
            Return True
        End If
    End Function
    Public Sub DBImport(VpPath As String, Optional VpSilent As Boolean = False)
    '---------------------------------------------------------------------------------
    'Importe dans la base de données l'ensemble du patch (structure + enregistrements)
    '---------------------------------------------------------------------------------
    'TABLE_CATALOG
    'TABLE_SCHEMA
    'TABLE_NAME
    'COLUMN_NAME
    'COLUMN_GUID
    'COLUMN_PROPID
    'ORDINAL_POSITION
    'COLUMN_HASDEFAULT
    'COLUMN_DEFAULT
    'COLUMN_FLAGS
    'IS_NULLABLE
    'DATA_TYPE
    'TYPE_GUID
    'CHARACTER_MAXIMUM_LENGTH
    'CHARACTER_OCTET_LENGTH
    'NUMERIC_PRECISION
    'NUMERIC_SCALE
    'DATETIME_PRECISION
    'CHARACTER_SET_CATALOG
    'CHARACTER_SET_SCHEMA
    'CHARACTER_SET_NAME
    'COLLATION_CATALOG
    'COLLATION_SCHEMA
    'COLLATION_NAME
    'DOMAIN_CATALOG
    'DOMAIN_SCHEMA
    'DOMAIN_NAME
    'DESCRIPTION
    '---------------------------------------------------------------------------------
    Dim VpDB As New OleDbConnection(CgStrConn(CInt(VgOptions.VgSettings.DBProvider)) + VpPath)
    Dim VpTable As String
    Dim VpTables As DataTable
    Dim VpSchemaTable As DataTable
    Dim VpType As OleDbType
    Dim VpSQL As String
    Dim VpDA1 As New OleDbDataAdapter
    Dim VpDA2 As New OleDbDataAdapter
    Dim VpDS1 As New DataSet
    Dim VpDS2 As New DataSet
    Dim VpDBCommand As New OleDbCommand
    Dim VpBuilder As New OleDbCommandBuilder(VpDA2)
    Dim VpRow As DataRow
    Dim VpCurTable As DataTable
        VpDB.Open
        VpDBCommand.Connection = VpDB
        VpDBCommand.CommandType = CommandType.Text
        VpTables = VpDB.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, New Object() {Nothing, Nothing, Nothing, "TABLE"})
        'Pour chaque table du patch
        For VpI As Integer = 0 To VpTables.Rows.Count - 1
            VpTable = VpTables.Rows(VpI)!TABLE_NAME.ToString
            'Si la table existe déjà dans la base mais qu'elle doit être préalablement détruite
            If PreDelete(VpTable) Then
                Try
                    VgDBCommand.CommandText = "Drop Table " + VpTable + ";"
                    VgDBCommand.ExecuteNonQuery
                Catch
                End Try
            End If
            'Si la table n'existe pas (ou plus) dans la base
            If Not IsInDB(VpTable) Then
                'Prépare la requête de création de table (méthode SQL)
                VpSQL = "Create Table " + VpTable + "("
                VpSchemaTable = VpDB.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, New Object() {Nothing, Nothing, VpTable, Nothing})
                For VpK As Integer = 0 To VpSchemaTable.Rows.Count - 1
                    For VpJ As Integer = 0 To VpSchemaTable.Rows.Count - 1
                        If VpSchemaTable.Rows(VpJ)!ORDINAL_POSITION - 1 = VpK Then          'Respecte la position ordinale des champs
                            VpSQL = VpSQL + VpSchemaTable.Rows(VpJ)!COLUMN_NAME.ToString
                            VpType = VpSchemaTable.Rows(VpJ)!DATA_TYPE
                            If VpType = OleDbType.WChar Then
                                VpSQL = VpSQL + " VarChar(" + VpSchemaTable.Rows(VpJ)!CHARACTER_MAXIMUM_LENGTH.ToString + ") With Compression"
                            Else
                                VpSQL = VpSQL + " " + VpType.ToString.Replace("Boolean", "Bit")
                            End If
                            VpSQL = VpSQL + ", "
                        End If
                    Next VpJ
                Next VpK
                VpSQL = VpSQL.Substring(0, VpSQL.Length - 2)
                VgDBCommand.CommandText = VpSQL + ");"
                VgDBCommand.ExecuteNonQuery
                'La table étant présente, on peut insérer les nouvelles données (méthode OLEDB/DataSet)
                VpDBCommand.CommandText = "Select * From " + VpTable + ";"
                VgDBCommand.CommandText = VpDBCommand.CommandText
                VpDA1.SelectCommand = VpDBCommand
                VpDA2.SelectCommand = VgDBCommand
                VpBuilder.RefreshSchema
                VpDA2.InsertCommand = VpBuilder.GetInsertCommand
                VpDA1.Fill(VpDS1, VpTable)
                VpDA2.Fill(VpDS2, VpTable)
                VpCurTable = VpDS2.Tables.Item(VpDS2.Tables.IndexOf(VpTable))
                For Each VpSrcRow As DataRow In VpDS1.Tables.Item(VpDS1.Tables.IndexOf(VpTable)).Rows
                    VpRow = VpCurTable.NewRow
                    VpRow.ItemArray = VpSrcRow.ItemArray
                    VpCurTable.Rows.Add(VpRow)
                Next VpSrcRow
                VpDA2.Update(VpDS2, VpTable)
            End If
        Next VpI
        'Informe l'utilisateur
        If Not VpSilent Then
            Call ShowInformation("Mise à jour effectuée." + vbCrLf + "Assurez-vous d'avoir également la dernière version du logiciel...")
        End If
        'Supprime le patch
        VpDB.Close
        VpDB.Dispose
        VpDB = Nothing
        Call SecureDelete(VpPath)
    End Sub
    Public Sub DBAdaptEncNbr
    '--------------------------------------------------------------------------------------------------------------------------
    'Toutes les éditions ne sont pas forcément importées dans le même ordre chez les utilisateurs, d'où des EncNbr différents :
    '=> cette procédure détermine les bons EncNbr à partir du nom de la carte et de son édition
    '--------------------------------------------------------------------------------------------------------------------------
        VgDBCommand.CommandText = "Alter Table PricesHistory Add EncNbr Long;"
        VgDBCommand.ExecuteNonQuery
        VgDBCommand.CommandText = "Update Card Inner Join PricesHistory On PricesHistory.Title = Card.Title And PricesHistory.Series = Card.Series Set PricesHistory.EncNbr = Card.EncNbr Where PricesHistory.Title = Card.Title And PricesHistory.Series = Card.Series;"
        VgDBCommand.ExecuteNonQuery
        VgDBCommand.CommandText = "Alter Table PricesHistory Drop Column Series;"
        VgDBCommand.ExecuteNonQuery
        VgDBCommand.CommandText = "Alter Table PricesHistory Drop Column Title;"
        VgDBCommand.ExecuteNonQuery
        VgDBCommand.CommandText = "Create Index EncNbr On PricesHistory (EncNbr);"
        VgDBCommand.ExecuteNonQuery
    End Sub
    Private Function PreDelete(VpTable As String) As Boolean
    '---------------------------------------------------------------------------------------------
    'Indique si la table spécifiée en paramètre doit être supprimée lors de l'application du patch
    '---------------------------------------------------------------------------------------------
        Select Case VpTable
            Case "MySpecialUses"
                'Essaie d'utiliser le champ large ; si erreur => on doit recréer la table avec un champ plus grand
                Try
                    VgDBCommand.CommandText = "Insert Into MySpecialUses Values (-1, -1, 'X', '" + StrBuild("X", 128) + "', '" + StrBuild("X", 128) + "', False, False);"
                    VgDBCommand.ExecuteNonQuery
                    VgDBCommand.CommandText = "Delete * From MySpecialUses Where EffortID = -1;"        'un peu crade mais je n'ai pas trouvé plus simple !
                    VgDBCommand.ExecuteNonQuery
                Catch
                    Return True
                End Try
                Return False
            Case Else
                Return True
        End Select
    End Function
    Private Function IsInDB(VpTable As String) As Boolean
    '------------------------------------------------------------------------------------------------
    'Renvoie faux si la table spécifiée en paramètre n'existe pas ou est vide dans la base de données
    '------------------------------------------------------------------------------------------------
    Dim VpResult As Boolean
        Try
            VgDBCommand.CommandText = "Select * From " + VpTable + ";"
            VgDBReader = VgDBCommand.ExecuteReader
            VgDBReader.Read
            VpResult = True
        Catch
            VpResult = False
        End Try
        If Not VgDBReader.IsClosed Then
            VgDBReader.Close
        End If
        Return VpResult
    End Function
    #End Region
    #Region "Boîtes de dialogue"
    Public Sub ShowWarning(VpStr As String)
        MessageBox.Show(VpStr, "Problème", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1)
    End Sub
    Public Sub ShowInformation(VpStr As String)
        MessageBox.Show(VpStr, "Information", MessageBoxbuttons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
    End Sub
    Public Function ShowQuestion(VpStr As String, Optional VpButtons As MessageBoxButtons = MessageBoxbuttons.YesNo) As DialogResult
        Return MessageBox.Show(VpStr, "Question", VpButtons, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
    End Function
    #End Region
    #Region "Accessoires"
    Public Function Matching(VpStr As String) As Object
    '-------------------------------------------
    'Adapte une chaîne en un type plus approprié
    '-------------------------------------------
        If VpStr = "True" Then
            Return True
        ElseIf VpStr = "False" Then
            Return False
        ElseIf IsNumeric(VpStr)
            Return CInt(VpStr)
        Else
            Return VpStr
        End If
    End Function
    Public Function TrimQuery(VpSQL As String, Optional VpDot As Boolean = True, Optional VpAddendum As String = "") As String
    '----------------------------------
    'Suppression des mots-clés inutiles
    '----------------------------------
    Dim VpTrimSQL As String
        If VpSQL.EndsWith(" Where ") Then
            VpTrimSQL = VpSQL.Substring(0, VpSQL.Length - 7)
        ElseIf VpSQL.EndsWith(" And ") Then
            VpTrimSQL = VpSQL.Substring(0, VpSQL.Length - 5)
        Else
            VpTrimSQL = VpSQL
        End If
        If VpDot Then
            Return VpTrimSQL + VpAddendum + ";"
        Else
            Return VpTrimSQL + VpAddendum
        End If
    End Function
    Public Function FormatTitle(VpTag As String, VpStr As String, Optional VpFR As Boolean = False, Optional VpIsForTvw As Boolean = True) As String
    '-------------------------------------------------------------------
    'Modifie l'expression passée en paramètre en un titre plus explicite
    '-------------------------------------------------------------------
    Dim VpDBCommand As OleDbCommand
        Select Case VpTag
            Case "Card.Series"
                Try
                    VpDBCommand = New OleDbCommand
                    VpDBCommand.Connection = VgDB
                    VpDBCommand.CommandType = CommandType.Text
                    VpDBCommand.CommandText = "Select " + If(VpFR, "SeriesNM_FR", "SeriesNM") + " From Series Where SeriesCD = '" + VpStr + "';"
                    Return VpDBCommand.ExecuteScalar.ToString
                Catch
                    Return VpStr
                End Try
            Case "Card.Type"
                Select Case VpStr.ToUpper
                    Case "C"
                        Return "Créatures"
                    Case "I"
                        Return "Ephémères"
                    Case "A"
                        Return "Artefacts"
                    Case "E"
                        Return "Auras"
                    Case "L"
                        Return "Terrains"
                    Case "N"
                        Return "Interruptions"
                    Case "S"
                        Return "Rituels"
                    Case "T"
                        Return "Enchantements"
                    Case "U"
                        Return "Créatures avec capacité"
                    Case "P"
                        Return "Arpenteurs"
                    Case "Q"
                        Return "Plans"
                    Case "H"
                        Return "Phénomènes"
                    Case "Y"
                        Return "Conspirations"
                    Case "Z"
                        Return "Machinations"
                    Case "K"
                        Return "Jetons"
                    Case Else
                        Return VpStr
                End Select
            Case "Spell.Color"
                Select Case VpStr.ToUpper
                    Case "A"
                        Return "Incolores"
                    Case "B"
                        Return "Noires"
                    Case "G"
                        Return "Vertes"
                    Case "L"
                        Return "Terrains"
                    Case "M"
                        Return "Multicolores"
                    Case "R"
                        Return "Rouges"
                    Case "U"
                        Return "Bleues"
                    Case "W"
                        Return "Blanches"
                    Case "T"
                        Return "Jetons"
                    'Cas mal géré des double cartes
                    Case "X"
                        Return "Double-cartes"
                    Case Else
                        Return VpStr
                End Select
            Case "Spell.myCost"
                If VpIsForTvw Then
                    Return ""
                Else
                    Return VpStr
                End If
            Case "Card.myPrice"
                Select Case VpStr
                    Case "1"
                        Return "Moins de 0,50€"
                    Case "2"
                        Return "Entre 0,50€ et 1€"
                    Case "3"
                        Return "Entre 1€ et 3€"
                    Case "4"
                        Return "Entre 3€ et 5€"
                    Case "5"
                        Return "Entre 5€ et 10€"
                    Case "6"
                        Return "Entre 10€ et 20€"
                    Case "7"
                        Return "Entre 20€ et 50€"
                    Case "8"
                        Return "Plus de 50 €"
                    Case Else
                        Return VpStr
                End Select
            Case "Card.Rarity"
                Select Case VpStr.Substring(0, 1).ToUpper
'                   Case "M"
'                       Return ("Mythiques (" + VpStr.Substring(1) + ")").Replace("()","")
'                   Case "R"
'                       Return ("Rares (" + VpStr.Substring(1) + ")").Replace("()","")
'                   Case "U"
'                       Return ("Peu communes (" + VpStr.Substring(1) + ")").Replace("()","")
'                   Case "C"
'                       Return ("Communes (" + VpStr.Substring(1) + ")").Replace("()","")
'                   Case "D", "L", "S"
'                       Return ("Sans valeur (" + VpStr.Substring(1) + ")").Replace("()","")
                    Case "M"
                        Return "Mythiques"
                    Case "R"
                        Return "Rares"
                    Case "U"
                        Return "Peu communes"
                    Case "C"
                        Return "Communes"
                    Case "D", "L", "S"
                        Return "Sans valeur"
                    Case "X"
                        Return "Spéciales"
                    Case Else
                        Return VpStr
                End Select
            Case Else
                Return VpStr
        End Select
    End Function
    Public Function MatchColor(VpColor As String) As String
        Select Case VpColor.ToUpper
            Case "W"
                Return "White"
            Case "U"
                Return "Blue"
            Case "R"
                Return "Red"
            Case "G"
                Return "Green"
            Case "B"
                Return "Black"
            Case "M"
                Return "Multicolor"
            Case "A"
                Return "Colorless"
            Case Else
                Return VpColor
        End Select
    End Function
    '---------------------
    'Gestion formats dates
    '---------------------
    Public Function MyCDate(VpStr As String) As Date
        If VpStr <> "" Then
            Return CDate(VpStr)
        Else
            Return Nothing
        End If
    End Function
    Public Function MyShortDateString(VpDate As Date) As String
        If VpDate.Year <> 1 Then
            Return VpDate.ToShortDateString
        Else
            Return CgPerfsVFree
        End If
    End Function
    Public Function GetDate(VpDate As Date) As String
        Return "'" + VpDate.ToString + "'"
    End Function
    Public Function MyPrice(VpStr As String) As Integer
    '-------------------------------------------------------
    'Retourne la catégorie du prix correspondant à sa valeur
    '-------------------------------------------------------
        '(1 [0-0.5] 2 [0.5-1] 3 [1-3] 4 [3-5] 5 [5-10] 6 [10-20] 7 [20-50] 8 [50+])
        Select Case MyVal(VpStr)
            Case Is <= 0.5
                Return 1
            Case Is <= 1
                Return 2
            Case Is <= 3
                Return 3
            Case Is <= 5
                Return 4
            Case Is <= 10
                Return 5
            Case Is <= 20
                Return 6
            Case Is <= 50
                Return 7
            Case Else
                Return 8
        End Select
    End Function
    Public Function MyCost(VpStr As String) As Integer
    '---------------------------------------------------------------------
    'Retourne le coût converti de mana de l'invocation passée en paramètre
    '---------------------------------------------------------------------
    Dim VpStrs() As String
        If VpStr = "0" Then
            Return 0
        ElseIf VpStr.Contains(" // ") Then
            VpStrs = VpStr.Split(New String() {" // "}, StringSplitOptions.None)
            Return MyInnerCost(VpStrs(0)) + MyInnerCost(VpStrs(1))
        Else
            Return MyInnerCost(VpStr)
        End If
    End Function
    Private Function MyInnerCost(VpStr As String) As Integer
    Dim VpColorless As Integer
        VpColorless = Val(VpStr)
        If VpColorless <> 0 Then
            Return VpStr.Replace(VpColorless.ToString.Trim, "").Length + VpColorless - 4 * StrCount(VpStr, "(")
        Else
            Return VpStr.Length - 4 * StrCount(VpStr, "(")
        End If
    End Function
    Public Function MyTxt(VpCard As String, VpVF As Boolean, VpDownFace As Boolean) As String
    '----------------------------------------------------
    'Retourne le texte VF de la carte passée en paramètre
    '----------------------------------------------------
    Dim VpDBCommand As New OleDbCommand
    Dim VpO As Object
    Dim VpStr As String
        VpDBCommand.Connection = VgDB
        VpDBCommand.CommandType = CommandType.Text
        If VpVF Then
            VpDBCommand.CommandText = "Select TexteFR From TextesFR Where CardName = '" + VpCard.Replace("'", "''") + "';"
        Else
            VpDBCommand.CommandText = "Select CardText From Card Where Title = '" + VpCard.Replace("'", "''") + "';"
        End If
        VpO = VpDBCommand.ExecuteScalar
        If Not VpO Is Nothing Then
            VpStr = VpO.ToString
            If VpStr.Contains("//") Then
                If VpDownFace Then
                    Return VpStr.Substring(VpStr.IndexOf("//") + 6).Trim
                Else
                    Return VpStr.Substring(0, VpStr.IndexOf("//") - 5).Trim
                End If
            Else
                Return VpStr.Trim
            End If
        Else
            Return ""
        End If
    End Function
    Public Function MyQuality(VpQuality As String) As eQuality
        Select Case VpQuality
            Case "MT", "Mint"
                Return eQuality.Mint
            Case "NM", "Near Mint"
                Return eQuality.NearMint
            Case "EX", "Excellent"
                Return eQuality.Excellent
            Case "GD", "Good"
                Return eQuality.Good
            Case "LP", "Lightly Played", "Light Played"
                Return eQuality.LightPlayed
            Case "PL", "Played"
                Return eQuality.Played
            Case "PO", "Poor"
                Return eQuality.Poor
            Case Else
                Return eQuality.Good
        End Select
    End Function
    Public Function MyLanguage(VpLanguage As String) As String
        Select Case VpLanguage.ToLower
            Case "fra"
                Return "French"
            Case "eng"
                Return "English"
            Case Else
                Return VpLanguage
        End Select
    End Function
    Public Function StrCount(VpStr As String, VpChar As String) As Integer
    '----------------------------------------------------------------------------------------
    'Retourne le nombre d'occurences du caractère spécifié dans la chaîne passée en paramètre
    '----------------------------------------------------------------------------------------
    Dim VpCounter As Integer = 0
        For VpI As Integer = 0 To VpStr.Length - 1
            If VpStr.Substring(VpI, 1) = VpChar Then
                VpCounter = VpCounter + 1
            End If
        Next VpI
        Return VpCounter
    End Function
    Public Function StrBuild(VpChar As String, VpCount As Integer) As String
    '--------------------------
    'Retourne n occurences de c
    '--------------------------
    Dim VpStr As String = ""
        For VpI As Integer = 1 To VpCount
            VpStr = VpStr + VpChar
        Next VpI
        Return VpStr
    End Function
    Public Function StrDiacriticInsensitize(VpStr As String) As String
    '-----------------------------------------------------------------------------------------------------------------------------------------------
    'Rend la requête SQL insensible aux signes diacritiques pour le terme passé en paramètre (NB. elle l'est déjà vis-à-vis de la casse avec JetSQL)
    '-----------------------------------------------------------------------------------------------------------------------------------------------
    Dim VpStrSB As New StringBuilder
    Dim VpCur As Char
        For VpI As Integer = 0 To VpStr.Length - 1
            VpCur = VpStr.Substring(VpI, 1)
            Select Case VpCur
                Case "e", "é", "è", "ê", "ë", "E", "É", "È", "Ê", "Ë"
                    VpStrSB.Append("[eéèêë]")
                Case "a", "à", "â", "ä", "A", "À", "Â", "Ä"
                    VpStrSB.Append("[aàâä]")
                Case "i", "ì", "ï", "î", "I", "Ì", "Ï", "Î"
                    VpStrSB.Append("[iïîì]")
                Case "o", "ô", "ö", "ò", "O", "Ô", "Ö", "Ò"
                    VpStrSB.Append("[oôöò]")
                Case "u", "ù", "û", "ü", "U", "Ù", "Û", "Ü"
                    VpStrSB.Append("[uûüù]")
                Case "c", "ç", "C", "Ç"
                    VpStrSB.Append("[cç]")
                Case Else
                    VpStrSB.Append(VpCur)
            End Select
        Next VpI
        Return VpStrSB.ToString
    End Function
    Public Function MyVal(VpStr As String) As Double
        Return Val(VpStr.Replace(",", "."))
    End Function
    Public Function FindIndex(VpTab() As String, VpValue As String) As Integer
    '--------------------------------------------------------------------------------------------------------------
    'Retourne l'indice de la première occurence de la valeur passée en paramètre dans le tableau passé en paramètre
    '--------------------------------------------------------------------------------------------------------------
        For VpI As Integer = 0 To VpTab.Length - 1
            If VpTab(VpI) = VpValue Then
                Return VpI
            End If
        Next VpI
        Return -1
    End Function
    Public Function FindNumber(VpStr As String) As Integer
    '----------------------------------------------------------------------------------
    'Retourne le nombre correspondant à la chaîne (texte ou nombre) passée en paramètre
    '----------------------------------------------------------------------------------
        If IsNumeric(VpStr) Then
            Return Val(VpStr)
        Else
            Return FindIndex(CgNumbers, VpStr) + 1
        End If
    End Function
    Public Function AvoidForbiddenChr(ByVal VpIn As String, Optional VpChrSet As eForbiddenCharset = eForbiddenCharset.Standard) As String
        Select Case VpChrSet
            Case eForbiddenCharset.Standard
                Return VpIn.Replace(":", "").Replace("/", "").Replace("""", "").Replace("?", "")
            Case eForbiddenCharset.BDD
                Return AvoidForbiddenChr(VpIn.Replace("'", "''"))
            Case eForbiddenCharset.Full
                Return AvoidForbiddenChr(VpIn.Replace("\", "").Replace("*", "").Replace("<", "").Replace(">", "").Replace("|", ""))
            Case Else
                Return VpIn
        End Select
    End Function
    Public Function ExtractENName(VpStr As String) As String
    Dim VpTitle As String = VpStr
        VpTitle = VpTitle.Substring(VpTitle.IndexOf("(") + 1)
        If VpTitle.Contains("(") Then
            VpTitle = VpTitle.Substring(VpTitle.IndexOf("(") + 1)
        End If
        Return VpTitle.Substring(0, VpTitle.Length - 1)
    End Function
    Public Function SafeGetNonZeroVal(VpColumn As String) As Single
        With VgDBReader
            If .GetValue(.GetOrdinal(VpColumn)) Is DBNull.Value Then
                Return 0
            Else
                Return .GetValue(.GetOrdinal(VpColumn))
            End If
        End With
    End Function
    Public Function SafeGetScalarText As String
        Try
            Return VgDBCommand.ExecuteScalar.ToString
        Catch
            Return ""
        End Try
    End Function
    Public Sub InitCriteres(VpMainForm As MainForm)
        For VpI As Integer = 0 To VpMainForm.FilterCriteria.NCriteria - 1
            CgCriteres.Add(VpMainForm.FilterCriteria.MyList.Items(VpI), CgCriterionsFields(VpI))
        Next VpI
    End Sub
    Public Sub InitGrid(VpGrid As Grid, VpColumns() As String)
    '-----------------------------------
    'Initialisation des contrôles grille
    '-----------------------------------
        With VpGrid
            'Nettoyage
            If .Rows.Count > 0 Then
                .Rows.RemoveRange(0, .Rows.Count)
            End If
            'Nombre de colonnes et d'en-têtes
            .ColumnsCount = VpColumns.Length
            .FixedRows = 1
            .Rows.Insert(0)
            For VpI As Integer = 0 To VpColumns.Length - 1
                VpGrid(0, VpI) = New Cells.ColumnHeader(VpColumns(VpI))
            Next VpI
            .AutoSize
        End With
    End Sub
    Public Sub CopyStream(VpIn As Stream, VpOut As Stream)
    Dim VpBuffer() As Byte = New Byte(8 * 1024 - 1) {}
    Dim VpLen As Integer
        Do
            VpLen = VpIn.Read(VpBuffer, 0, VpBuffer.Length)
            If VpLen > 0 Then
                VpOut.Write(VpBuffer, 0, VpLen)
            Else
                Exit Do
            End If
        Loop
        VpOut.Flush
        VpOut.Close
    End Sub
    Public Sub VerboseSimu(VpVerbose As Boolean, VpStr As String, VpSimuOut As StreamWriter, Optional VpEndSimu As Boolean = False)
    '-----------------------------------------------------
    'Gère la verbosité pour les simulations de déploiement
    '-----------------------------------------------------
        If VpVerbose Then
            If VpEndSimu Then
                VpSimuOut.Flush
                VpSimuOut.Close
            Else
                VpSimuOut.WriteLine(VpStr)
            End If
        End If
    End Sub
    #End Region
    #Region "Mises à jour"
    Public Function GetPictSP As String
    Dim VpRequest As HttpWebRequest
    Dim VpResponse As WebResponse
    Dim VpAnswer As Stream
    Dim VpBuf() As Byte
    Dim VpStamp As String
            VpRequest = WebRequest.Create(VgOptions.VgSettings.DownloadServer + CgURL1C)
            VpResponse = VpRequest.GetResponse
            VpAnswer = VpResponse.GetResponseStream
            'Lecture du fichier sur Internet
            ReDim VpBuf(0 To VpResponse.ContentLength - 1)
            VpAnswer.Read(VpBuf, 0, VpBuf.Length)
            VpStamp = New ASCIIEncoding().GetString(VpBuf)
            Return VpStamp
    End Function
    Public Sub CheckForPicUpdates
    '-------------------------------------------------------------------------
    'Vérifie si une mise à jour de la base d'image est disponible sur Internet
    '-------------------------------------------------------------------------
    Dim VpStamp As String
    Dim VpStr As String
    Dim VpOldText As String
        VpOldText = MainForm.VgMe.StatusTextGet
        Call MainForm.VgMe.StatusText(CgDL1, True)
        VgTimer.Stop
        'Vérification par la taille
        Try
            VpStamp = GetPictSP
            VpStr = (New FileInfo(VgOptions.VgSettings.PicturesFile)).Length.ToString
            If VpStamp.Contains(VpStr) Then
                VpStr = VpStamp.Substring(VpStamp.IndexOf(VpStr) + VpStr.Length + 1)
                VpStr = VpStr.Substring(0, VpStr.IndexOf("#"))
                If VpStr = "OK"  Then
                    Call ShowInformation("Les images sont déjà à jour...")
                    Call MainForm.VgMe.StatusText(VpOldText)
                Else
                    'Téléchargement du fichier accompagnateur
                    Call DownloadNow(New Uri(VgOptions.VgSettings.DownloadServer + CgURL10 + VpStr + CgPicLogExt), CgUpPic + CgPicLogExt)
                    Application.DoEvents
                    'Téléchargement du service pack d'images
                    MainForm.VgMe.IsInImgDL = True
                    Call DownloadUpdate(New Uri(VgOptions.VgSettings.DownloadServer + CgURL10 + VpStr + CgPicUpExt), CgUpPic + CgPicUpExt)
                End If
            Else
                If ShowQuestion("La base d'images semble être corrompue." + vbCrLf + "Voulez-vous la re-télécharger maintenant ?") = System.Windows.Forms.DialogResult.Yes Then
                    'Re-téléchargement complet de la base principale
                    MainForm.VgMe.IsInImgDL = True
                    Call DownloadUpdate(New Uri(VgOptions.VgSettings.DownloadServer + CgURL10 + CgUpDDBd), VgOptions.VgSettings.PicturesFile, False)
                Else
                    Call MainForm.VgMe.StatusText(VpOldText)
                End If
            End If
        Catch
            'En cas d'échec de connexion
            Call ShowWarning(CgDL3b)
            Call MainForm.VgMe.StatusText(VpOldText)
        End Try
    End Sub
    Public Sub CheckForUpdates(Optional VpExplicit As Boolean = False, Optional VpBeta As Boolean = False, Optional VpContenu As Boolean = False)
    '------------------------------------------------------------------
    'Vérifie si une mise à jour du logiciel est disponible sur Internet
    '------------------------------------------------------------------
    Dim VpRequest As HttpWebRequest
    Dim VpAnswer As Stream
    Dim VpBuf(0 To 18) As Byte
    Dim VpOldText As String
    Dim VpContenuUpdate As frmUpdateContent
    Dim VpNewContenu As New List(Of clsUpdateContent)
        VpOldText = MainForm.VgMe.StatusTextGet
        Call MainForm.VgMe.StatusText(CgDL1)
        'Fichier d'historique des versions
        Call DownloadNow(New Uri(VgOptions.VgSettings.DownloadServer + CgURL7), CgHSTFile)
        'Vérification horodatage
        Try
            VpRequest = WebRequest.Create(If(VpBeta, VgOptions.VgSettings.DownloadServer + CgURL1B, VgOptions.VgSettings.DownloadServer + CgURL1))
            VpAnswer = VpRequest.GetResponse.GetResponseStream
            'Lecture du fichier horodaté sur Internet
            VpAnswer.Read(VpBuf, 0, 19)
            Call Date.TryParseExact(New ASCIIEncoding().GetString(VpBuf), "dd/MM/yyyy HH:mm:ss", New CultureInfo("fr-FR"), DateTimeStyles.None, VgRemoteDate)
            'Si version plus récente
            If DateTime.Compare(File.GetLastWriteTimeUtc(Application.ExecutablePath), VgRemoteDate) < 0 Then
                VgTray.Visible = True
                VgTray.Tag = If(VpBeta, eUpdateType.Beta, eUpdateType.Release)
                VgTray.ShowBalloonTip(10, "Magic The Gathering Manager" + If(VpBeta, " BETA", ""), "Une mise à jour de l'application est disponible..." + vbCrLf + "Cliquer ici pour la télécharger, quitter Magic The Gathering Manager et l'installer.", ToolTipIcon.Info)
            ElseIf VpExplicit Then
                If VpContenu Then
                    If mdlToolbox.ShowQuestion("L'application est à jour..." + vbCrLf + "Voulez-vous rechercher aussi les mises à jour de contenu (prix, images...) ?") = System.Windows.Forms.DialogResult.Yes Then
                        Call MainForm.VgMe.MnuContenuUpdateClick(Nothing, Nothing)
                    End If
                Else
                    If VpBeta Then
                        Call ShowInformation("Aucune version bêta postérieure à la dernière release n'est disponible pour l'instant...")
                    Else
                        Call ShowInformation("Vous disposez déjà de la dernière version de Magic The Gathering Manager !")
                    End If
                End If
            'Recherche automatique des mises à jour de contenu
            ElseIf DBOK Then
                If MainForm.VgMe.MyChildren.DoesntExist(MainForm.VgMe.MyChildren.ContenuUpdater) Then
                    VpContenuUpdate = New frmUpdateContent
                    MainForm.VgMe.MyChildren.ContenuUpdater = VpContenuUpdate
                Else
                    VpContenuUpdate = MainForm.VgMe.MyChildren.ContenuUpdater
                End If
                If VpContenuUpdate.CheckForContenu(VpNewContenu) Then
                    VgTray.Visible = True
                    VgTray.Tag = eUpdateType.Contenu
                    VgTray.ShowBalloonTip(10, "Magic The Gathering Manager", "Des mises à jour de contenu sont disponibles..." + vbCrLf + "Cliquer ici pour en savoir plus...", ToolTipIcon.Info)
                End If
            End If
        Catch
            'En cas d'échec de connexion, inutile de continuer à checker
            VgTimer.Stop
            If VpExplicit Then
                Call ShowWarning(CgDL3b)
            End If
        End Try
        Call MainForm.VgMe.StatusText(VpOldText)
    End Sub
    Public Sub NotifyIconBalloonTipClosed(ByVal sender As Object, ByVal e As EventArgs) Handles VgTray.BalloonTipClosed
        VgTray.Visible = False
    End Sub
    Public Sub NotifyIconBalloonTipClicked(ByVal sender As Object, ByVal e As EventArgs) Handles VgTray.BalloonTipClicked
    Dim VpType As eUpdateType = VgTray.Tag
        VgTray.Visible = False
        VgTimer.Stop
        Select Case VpType
            Case eUpdateType.Release
                Call DownloadUpdate(New Uri(VgOptions.VgSettings.DownloadServer + CgURL2), CgUpDFile)
            Case eUpdateType.Beta
                Call DownloadUpdate(New Uri(VgOptions.VgSettings.DownloadServer + CgURL2B), CgUpDFile)
            Case eUpdateType.Contenu
                MainForm.VgMe.MyChildren.ContenuUpdater.Show
                MainForm.VgMe.MyChildren.ContenuUpdater.BringToFront
            Case Else
        End Select
    End Sub
    Public Sub TimerTick(ByVal sender As Object, ByVal e As EventArgs) Handles VgTimer.Tick
        Call CheckForUpdates
        VgTimer.Stop
    End Sub
    Public Sub DownloadNow(VpURI As System.Uri, VpOutput As String)
    '----------------------------------------------------------------------------
    'Télécharge immédiatement l'application mise à jour ou une de ses dépendances
    '----------------------------------------------------------------------------
    Dim VpCopy As Process
        Cursor.Current = Cursors.WaitCursor
        Try
            ServicePointManager.SecurityProtocol = &H00000C00   'TLS 1.2
            VgClient.DownloadFile(VpURI, Application.StartupPath + VpOutput)
        Catch
            'Si on arrive là c'est qu'on n'a pas les droits d'écriture => on télécharge dans un dossier temporaire et on lance la copie en demandant les droits d'admin
            Try
                VgClient.DownloadFile(VpURI, Path.GetTempPath + VpOutput)
                VpCopy = New Process
                VpCopy.StartInfo.FileName = "cmd.exe"
                VpCopy.StartInfo.Arguments = "/c copy """ + Path.GetTempPath + VpOutput + """ """ + Application.StartupPath + """"
                VpCopy.StartInfo.UseShellExecute = True
                VpCopy.StartInfo.Verb = "runas"
                VpCopy.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
                VpCopy.Start
                VpCopy.WaitForExit
            Catch
            End Try
        End Try
    End Sub
    Public Sub DownloadUpdate(VpURI As System.Uri, VpOutput As String, Optional VpBaseDir As Boolean = True, Optional VpSilent As Boolean = False)
    '------------------------------------------------------------------------------
    'Télécharge en arrière-plan l'application mise à jour ou une de ses dépendances
    '------------------------------------------------------------------------------
        If MainForm.VgMe.IsDownloadInProgress Then
            If Not MainForm.VgMe.MyChildren.DoesntExist(MainForm.VgMe.MyChildren.ContenuUpdater) Then
                If MainForm.VgMe.MyChildren.ContenuUpdater.PassiveUpdate = mdlConstGlob.ePassiveUpdate.InProgress Then
                    MainForm.VgMe.MyChildren.ContenuUpdater.PassiveUpdate = mdlConstGlob.ePassiveUpdate.Failed
                End If
            End If
            If Not VpSilent Then
                Call ShowWarning(CgDL2b)
            End If
        Else
            MainForm.VgMe.IsDownloadInProgress = True
            Call MainForm.VgMe.StatusText(CgDL2, True)
            Try
                VgClient.DownloadFileAsync(VpURI, If(VpBaseDir, Application.StartupPath + VpOutput, VpOutput))
                MainForm.VgMe.btDownload.Visible = True
                MainForm.VgMe.btDownload.Tag = Now
            Catch
            End Try
        End If
    End Sub
    Public Sub DownloadUnzip(VpURI As System.Uri, VpOutput As String, VpInside As String)
    '-------------------------------------------------------------
    'Télécharge immédiatement et décompresse la ressource demandée
    '-------------------------------------------------------------
    Dim VpZipStream As ZipInputStream
    Dim VpZipEntry As ZipEntry
        Call DownloadNow(VpURI, VpOutput)
        If File.Exists(Application.StartupPath + VpOutput) Then
            VpZipStream = New ZipInputStream(File.OpenRead(Application.StartupPath + VpOutput))
            Do
                VpZipEntry = VpZipStream.GetNextEntry
                If VpZipEntry IsNot Nothing AndAlso VpZipEntry.IsFile AndAlso VpZipEntry.Name <> "" Then
                    Using VpFile As Stream = File.OpenWrite(Application.StartupPath + VpInside)
                        Call mdlToolbox.CopyStream(VpZipStream, VpFile)
                    End Using
                Else
                    Exit Do
                End If
            Loop
            VpZipStream.Close
        End If
    End Sub
    Public Sub ClientDownloadProgressChanged(ByVal sender As Object, ByVal e As DownloadProgressChangedEventArgs) Handles VgClient.DownloadProgressChanged
'       MainForm.VgMe.VgBar.Style = ProgressBarStyle.Blocks
        MainForm.VgMe.prgAvance.Maximum = 100
'       MainForm.VgMe.VgBar.Maximum = 100
        MainForm.VgMe.prgAvance.Value = e.ProgressPercentage
'       MainForm.VgMe.VgBar.Value = e.ProgressPercentage
        MainForm.VgMe.prgAvance.Visible = True
'       MainForm.VgMe.VgBar.ShowInTaskbar = True
    End Sub
    Public Sub ClientDownloadFileCompleted(ByVal sender As Object, ByVal e As AsyncCompletedEventArgs) Handles VgClient.DownloadFileCompleted
    '------------------------------------------------------------
    'Installe l'application mise à jour ou une de des dépendances
    '------------------------------------------------------------
    Dim VpResult As WebException = e.Error
        MainForm.VgMe.IsInImgDL = False
        MainForm.VgMe.IsDownloadInProgress = False
        MainForm.VgMe.btDownload.Visible = False
        MainForm.VgMe.prgAvance.Visible = False
'       MainForm.VgMe.VgBar.ShowInTaskbar = False
        'Gestion des erreurs
        If Not VpResult Is Nothing Then
            If VpResult.Status = WebExceptionStatus.ConnectFailure Then
                Call ShowWarning(CgDL3b)
                Call MainForm.VgMe.StatusText(CgDL3)
                Exit Sub
            ElseIf VpResult.Status = WebExceptionStatus.RequestCanceled Then
                Call MainForm.VgMe.StatusText(CgDL5)
                Call DeleteTempFiles(True)
                Exit Sub
            End If
        End If
        If MainForm.VgMe.StatusTextGet = CgDL2 Then
            Call MainForm.VgMe.StatusText(CgDL4)
        End If
        'Maj EXE
        If File.Exists(Application.StartupPath + CgUpDFile) Then
            Try
                File.SetLastWriteTimeUtc(Application.StartupPath + CgUpDFile, VgRemoteDate)
                Call SecureDelete(Application.StartupPath + CgDownDFile)
                File.Copy(Process.GetCurrentProcess.MainModule.FileName, Application.StartupPath + CgDownDFile)
                Process.Start(New ProcessStartInfo(Application.StartupPath + CgUpdater))
            Catch
                Call ShowWarning(CgErr5)
            End Try
        'Maj Images
        ElseIf File.Exists(Application.StartupPath + CgUpPic + CgPicUpExt) And File.Exists(Application.StartupPath + CgUpPic + CgPicLogExt) Then
            Call MainForm.VgMe.UpdatePictures(Application.StartupPath + CgUpPic + CgPicUpExt, Application.StartupPath + CgUpPic + CgPicLogExt, True)
        'Maj TXTFR
        ElseIf File.Exists(Application.StartupPath + CgUpTXTFR) Then
            Call MainForm.VgMe.UpdateTxtFR
        'Maj Rulings
        ElseIf File.Exists(Application.StartupPath + CgUpRulings) Then
            Call MainForm.VgMe.UpdateRulings(Not MainForm.VgMe.MyChildren.DoesntExist(MainForm.VgMe.MyChildren.ContenuUpdater) AndAlso MainForm.VgMe.MyChildren.ContenuUpdater.PassiveUpdate = mdlConstGlob.ePassiveUpdate.InProgress)
        End If
    End Sub
    #End Region
    #Region "Requêtes ponctuelles"
    Public Function GetEncNbr(VpCardName As String, VpIDSerie As String) As Long
    '-------------------------------------------------------------------------------------------
    'Retourne le numéro unique caractéristique d'une carte à partir de son nom et de son édition
    '-------------------------------------------------------------------------------------------
        VgDBCommand.CommandText = "Select Card.EncNbr From Card Where Card.Title = '" + VpCardName.Replace("'", "''") + "' And Card.Series = '" + VpIDSerie + "';"
        Return CLng(VgDBCommand.ExecuteScalar)
    End Function
    Public Function GetSerieCodeFromName(VpName As String, Optional VpApprox As Boolean = False, Optional VpFR As Boolean = False) As String
    '-------------------------------------------------------------------------------------------
    'Retourne le code id d'une édition à partir de son nom VO ou VF, éventuellement approximatif
    '-------------------------------------------------------------------------------------------
    Dim VpO As Object
        If VpName.Length = 2 Then
            Return VpName
        Else
            VgDBCommand.CommandText = "Select SeriesCD From Series Where " + If(VpFR, "SeriesNM_FR", "SeriesNM") + " = '" + VpName.Replace("'", "''") + "';"
            VpO = VgDBCommand.ExecuteScalar
            If Not VpO Is Nothing Then
                Return VpO.ToString
            Else
                If VpApprox Then
                    VgDBCommand.CommandText = "Select SeriesCD From Series Where InStr(" + If(VpFR, "SeriesNM_FR", "SeriesNM") + ", '" + VpName.Replace("'", "''") + "') > 0;"
                    VpO = VgDBCommand.ExecuteScalar
                    If Not VpO Is Nothing Then
                        Return VpO.ToString
                    Else
                        Return " "
                    End If
                Else
                    Return " "
                End If
            End If
        End If
    End Function
    Public Function GetSerieNameFromCode(VpSerie As String, VpVF As Boolean) As String
    '--------------------------------------------------------
    'Retourne le nom VO d'une édition à partir de son code id
    '--------------------------------------------------------
    Dim VpO As Object
    Dim VpFoil As Boolean = VpSerie.EndsWith(CgFoil2)
    Dim VpDBCommand As New OleDbCommand("", VgDB)
        VpDBCommand.CommandText = "Select " + If(VpVF, "SeriesNM_FR", "SeriesNM") + " From Series Where SeriesCD = '" + VpSerie.Replace(CgFoil2, "") + "';"
        VpO = VpDBCommand.ExecuteScalar
        If Not VpO Is Nothing Then
            Return VpO.ToString + If(VpFoil, CgFoil2, "")
        Else
            Return ""
        End If
    End Function
    Public Function GetTransformedName(VpCard As String) As String
    '--------------------------------------------------------------------
    'Pour les cartes double faces, obtient le nom de la carte transformée
    '--------------------------------------------------------------------
    Dim VpDBCommand As OleDbCommand
        Try
            VpDBCommand = New OleDbCommand
            VpDBCommand.Connection = VgDB
            VpDBCommand.CommandType = CommandType.Text
            VpDBCommand.CommandText = "Select Top 1 Card.Title From Card Where Card.EncNbr In (Select CardDouble.EncNbrDownFace From Card Inner Join CardDouble On Card.EncNbr = CardDouble.EncNbrTopFace Where Card.Title = '" + VpCard.Replace("'", "''") + "');"
            Return VpDBCommand.ExecuteScalar.ToString
        Catch
            Return ""
        End Try
    End Function
    Public Function GetNameVF(VpCardVO As String) As String
    '----------------------------------------------------------------------------
    'Retourne la première traduction disponible pour la carte passée en paramètre
    '----------------------------------------------------------------------------
    Dim VpDBCommand As OleDbCommand
        Try
            VpDBCommand = New OleDbCommand
            VpDBCommand.Connection = VgDB
            VpDBCommand.CommandType = CommandType.Text
            VpDBCommand.CommandText = "Select CardFR.TitleFR From Card Inner Join CardFR On Card.EncNbr = CardFR.EncNbr Where Card.Title <> CardFR.TitleFR And Card.Title = '" + VpCardVO.Replace("'", "''") + "';"
            Return VpDBCommand.ExecuteScalar.ToString
        Catch
            Return VpCardVO
        End Try
    End Function
    Public Function HasPriceHistory As Boolean
    '--------------------------------------------------
    'Vérifie si la base contient un historique des prix
    '--------------------------------------------------
        VgDBCommand.CommandText = "Select Count(*) From PricesHistory;"
        Return ( CInt(VgDBCommand.ExecuteScalar) > 0 )
    End Function
    Public Function GetPriceHistory(VpCardName As String, VpFoil As Boolean) As Hashtable
    '---------------------------------------------------------------------------------------------
    'Retourne l'historique des prix pour la carte passée en paramètre pour chacune de ses éditions
    '---------------------------------------------------------------------------------------------
    Dim VpHist As New Hashtable
    Dim VpLastName As String = ""
    Dim VpCurName As String
    Dim VpCur As SortedList = Nothing
        VgDBCommand.CommandText = "Select Card.Series, GlobalHisto.PriceDate, GlobalHisto.Price From ((SELECT PricesHistory.EncNbr, PricesHistory.Price, DatesToUse.PriceDate, DatesToUse.Foil FROM PricesHistory INNER JOIN (SELECT PricesHistory.EncNbr, Max(PricesHistory.PriceDate) AS DLAST, AllDates.PriceDate, PricesHistory.Foil FROM PricesHistory, (SELECT Distinct PricesHistory.PriceDate FROM PricesHistory) As AllDates WHERE (((PricesHistory.PriceDate)<=[AllDates].[PriceDate])) GROUP BY PricesHistory.EncNbr, AllDates.PriceDate, PricesHistory.Foil) AS DatesToUse ON (PricesHistory.EncNbr = DatesToUse.EncNbr) AND (PricesHistory.PriceDate = DatesToUse.DLAST) AND (PricesHistory.Foil = DatesToUse.Foil)) As GlobalHisto) Inner Join Card On Card.EncNbr = GlobalHisto.EncNbr Where Card.Title = '" + VpCardName.Replace("'", "''") + "' And GlobalHisto.Foil = " + VpFoil.ToString + ";"
        VgDBReader = VgDBCommand.ExecuteReader
        With VgDBReader
            While .Read
                VpCurName = .GetString(0) + If(VpFoil, CgFoil2, "")
                If VpCurName <> VpLastName Then
                    VpCur = New SortedList
                    VpLastName = VpCurName
                    VpHist.Add(VpLastName, VpCur)
                End If
                If Not VpCur.ContainsKey(.GetDateTime(1)) Then
                    VpCur.Add(.GetDateTime(1), .GetFloat(2))
                End If
            End While
            .Close
        End With
        Return VpHist
    End Function
    Public Function GetLastPricesDate As Date
    '-------------------------------------------------
    'Retourne la dernière date de mise à jour des prix
    '-------------------------------------------------
    Dim VpDate As Date
        VgDBCommand.CommandText = "Select Top 1 PriceDate From Card Order By PriceDate Desc;"
        VpDate = VgDBCommand.ExecuteScalar
        If VpDate.Subtract(Date.Now).Days > 0 Then
            'Si on est là c'est que la date n'est pas valide (on ne peut pas avoir des prix dans le futur !) => on force la mise à jour en retournant une date d'il y a un an
            Return New DateTime(Date.Now.Year - 1, Date.Now.Month, Date.Now.Day)
        Else
            Return VpDate
        End If
    End Function
    Public Sub LoadEditions(VpCbo As ComboBox)
    '--------------------------------------------------------------
    'Charge la liste des éditions présentes dans la base de données
    '--------------------------------------------------------------
        VpCbo.Items.Clear
        VpCbo.Text = ""
        VgDBCommand.CommandText = "Select Distinct Series.SeriesCD, Series.SeriesNM From Card Inner Join Series On Card.Series = Series.SeriesCD;"
        VgDBReader = VgDBCommand.ExecuteReader
        With VgDBReader
            While .Read
                VpCbo.Items.Add("(" + .GetString(0) + ") " + .GetString(1))
            End While
            .Close
        End With
        VpCbo.Sorted = True
    End Sub
    Public Function GetAdvCount As Integer
    '------------------------------------------------------
    'Retourne le nombre de propriétaires en base de données
    '------------------------------------------------------
        VgDBCommand.CommandText = "Select Count(*) From MyAdversairesID;"
        Return VgDBCommand.ExecuteScalar
    End Function
    Public Function GetAdvDecksCount(VpI As Integer) As Integer
    '--------------------------------------------------------------------------------------
    'Retourne le nombre de decks possédés par le propriétaire d'index spécifié en paramètre
    '--------------------------------------------------------------------------------------
        VgDBCommand.CommandText = "Select Count(*) From MyGamesID Where AdvID = " + VpI.ToString + " And IsFolder = False;"
        Return VgDBCommand.ExecuteScalar
    End Function
    Public Function GetAdvDecksCount(VpName As String) As Integer
        VgDBCommand.CommandText = "Select Count(*) From MyGamesID Inner Join MyAdversairesID On MyGamesID.AdvID = MyAdversairesID.AdvID Where AdvName = '" + VpName.Replace("'", "''") + "' And IsFolder = False;"
        Return VgDBCommand.ExecuteScalar
    End Function
    Public Function GetAdvId(VpName As String) As Integer
    '-------------------------------------------------------------------------
    'Retourne l'identifiant de l'adversaire dont le nom est passé en paramètre
    '-------------------------------------------------------------------------
        VgDBCommand.CommandText = "Select AdvId From MyAdversairesId Where AdvName = '" + VpName.Replace("'", "''") + "';"
        Return VgDBCommand.ExecuteScalar
    End Function
    Public Function GetOwner(VpDeck As String) As String
    '-------------------------------------------------------------------------
    'Retourne l'identifiant de l'adversaire dont le nom est passé en paramètre
    '-------------------------------------------------------------------------
        VgDBCommand.CommandText = "Select AdvName From MyAdversairesID Inner Join MyGamesID On MyAdversairesID.AdvID = MyGamesID.AdvID Where GameName = '" + VpDeck.Replace("'", "''") + "';"
        Return VgDBCommand.ExecuteScalar
    End Function
    Public Function GetAdvName(VpI As Integer) As String
    '-------------------------------------------------------------
    'Retourne le nom de l'adversaire d'index spécifié en paramètre
    'NB. : 0 = Moi
    '-------------------------------------------------------------
        VgDBCommand.CommandText = "Select Last(AdvName) From (Select Top " + VpI.ToString + " AdvName From MyAdversairesID Order By AdvID);"
        Return VgDBCommand.ExecuteScalar
    End Function
    Public Function GetNewAdvId As Integer
    '-------------------------------------------------
    'Retourne un identifiant pour un nouvel adversaire
    '-------------------------------------------------
        VgDBCommand.CommandText = "Select Max(AdvID) From MyAdversairesID;"
        Return (CInt(VgDBCommand.ExecuteScalar) + 1)
    End Function
    Public Function GetDeckNameFromIndex(VpI As Integer) As String
    '---------------------------------------------------------------------
    'Retourne le nom du deck d'index spécifié en paramètre
    '/!\ retourne le nom du VpI ème deck, et pas le deck dont l'id est VpI
    '---------------------------------------------------------------------
        VgDBCommand.CommandText = "Select Last(GameName) From (Select Top " + VpI.ToString + " GameName From MyGamesID Where IsFolder = False Order By GameID);"
        Try
            Return VgDBCommand.ExecuteScalar
        Catch
            Return "Jeu n°" + VpI.ToString
        End Try
    End Function
    Public Function GetDeckIdFromName(VpStr As String) As String
    '--------------------------------------------------
    'Retourne l'id du deck de nom spécifié en paramètre
    '--------------------------------------------------
    Dim VpO As Object
        VgDBCommand.CommandText = "Select GameID From MyGamesID Where GameName = '" + VpStr.Replace("'", "''") + "';"
        VpO = VgDBCommand.ExecuteScalar
        If Not VpO Is Nothing Then
            Return VpO.ToString
        Else
            Return ""
        End If
    End Function
    Public Function GetDeckNameFromId(VpId As Long) As String
    '--------------------------------------------------
    'Retourne le nom du deck d'id spécifié en paramètre
    '--------------------------------------------------
        VgDBCommand.CommandText = "Select GameName From MyGamesID Where GameID = " + VpId.ToString + ";"
        Return VgDBCommand.ExecuteScalar.ToString
    End Function
    Public Function IsDeckFolder(VpId As Long) As Boolean
    '---------------------------------------------------
    'Retourne si le deck n'est qu'un alias de répertoire
    '---------------------------------------------------
        VgDBCommand.CommandText = "Select IsFolder From MyGamesID Where GameID = " + VpId.ToString + ";"
        Return VgDBCommand.ExecuteScalar.ToString
    End Function
    Public Function GetChildrenDecksIds(VpParent As String) As List(Of Integer)
    '---------------------------------------------------------------
    'Retourne les decks dont le nom du parent est passé en paramètre
    '---------------------------------------------------------------
    Dim VpChildren As New List(Of Integer)
        VgDBCommand.CommandText = "Select GameID From MyGamesID Where Parent " + VpParent + " Order By GameID;"
        VgDBReader = VgDBCommand.ExecuteReader
        With VgDBReader
            While .Read
                VpChildren.Add(CInt(.GetValue(0)))
            End While
            .Close
        End With
        Return VpChildren
    End Function
    Public Function GetDeckFormat(VpStr As String) As String
    '--------------------------------------------------------------
    'Retourne le format de jeu du deck de nom spécifié en paramètre
    '--------------------------------------------------------------
        VgDBCommand.CommandText = "Select GameFormat From MyGamesID Where GameName = '" + VpStr.Replace("'", "''") + "';"
        Return VgDBCommand.ExecuteScalar.ToString
    End Function
    Public Function GetDeckDescription(VpDeckIndex As Integer) As String
    '--------------------------------------------------------------------
    'Retourne le commentaire associé au deck de nom spécifié en paramètre
    '--------------------------------------------------------------------
    Dim VpSQL As String
    Dim VpO As Object
        VpSQL = "Select GameDescription From MyGamesID Inner Join MyGames On MyGamesID.GameID = MyGames.GameID  Where MyGames.GameID = " + VpDeckIndex.ToString
        VgDBCommand.CommandText = mdlToolbox.TrimQuery(VpSQL)
        VpO = VgDBCommand.ExecuteScalar
        If Not VpO Is Nothing Then
            Return VpO.ToString
        Else
            Return ""
        End If
    End Function
    Public Function GetDeckCount As Integer
    '----------------------------------------------
    'Retourne le nombre de decks en base de données
    '----------------------------------------------
        VgDBCommand.CommandText = "Select Count(*) From MyGamesID Where IsFolder = False;"
        Return VgDBCommand.ExecuteScalar
    End Function
    Public Function GetNewDeckId As Integer
    '--------------------------------------------
    'Retourne un identifiant pour un nouveau deck
    '--------------------------------------------
        VgDBCommand.CommandText = "Select Max(GameID) From MyGamesID;"
        Try
            Return (CInt(VgDBCommand.ExecuteScalar) + 1)
        Catch
            'Si pas de deck présent
            Return 0
        End Try
    End Function
    #End Region
    #Region "Gestion des images"
    Public Sub ExtractPictures(VpSaveFolder As String, VpSource As String, VpRestriction As String, Optional VpExtractTransformed As Boolean = False)
    '-------------------------------------------------------------------------------------------------
    'Sauvegarde dans le dossier spécifié par l'utilisateur l'ensembles des images JPEG de la sélection
    '-------------------------------------------------------------------------------------------------
    Dim VpSQL As String
    Dim VpCards As New List(Of String)
        VpSQL = "Select Distinct Card.Title From " + VpSource + " Inner Join Card On " + VpSource + ".EncNbr = Card.EncNbr Where "
        VpSQL = VpSQL + VpRestriction
        VpSQL = TrimQuery(VpSQL, False)
        If VpExtractTransformed Then
            VpSQL = VpSQL + " Union Select Distinct Card.Title From Card Where Card.EncNbr In (Select EncNbrDownFace From (" + VpSource + " Inner Join Card On " + VpSource + ".EncNbr = Card.EncNbr) Inner Join CardDouble On Card.EncNbr = CardDouble.EncNbrTopFace Where "
            VpSQL = VpSQL + VpRestriction
            VpSQL = TrimQuery(VpSQL, True, ")")
        End If
        VgDBCommand.CommandText = VpSQL
        VgDBReader = VgDBcommand.ExecuteReader
        With VgDBReader
            While .Read
                VpCards.Add(.GetString(0))
            End While
            .Close
        End With
        For Each VpCard As String In VpCards
            Call LoadScanCard(VpCard, 0, Nothing, True, VpSaveFolder)
        Next VpCard
    End Sub
    Public Sub LoadScanCard(VpTitle As String, VpMultiverseId As Long, VppicScanCard As PictureBox, Optional VpSave As Boolean = False, Optional VpSaveFolder As String = "")
    '---------------------------------------------------------------------------------
    'Charge l'image scannérisée de la carte recherchée dans la zone prévue à cet effet
    '---------------------------------------------------------------------------------
    Dim VpOffset As Long
    Dim VpEnd As Long
    Dim VpPicturesFile As StreamReader
    Dim VpPicturesFileB As BinaryReader
    Dim VpTmp As String = GetFreeTempFile(".jpg")
    Dim VpTmpFile As StreamWriter
    Dim VpTmpFileB As BinaryWriter
    Dim VpMissingTable As Boolean = False
    Dim VpDest As String
        If MainForm.VgMe.IsInImgDL Then
            Call ShowWarning(CgDL2c)
            Exit Sub
        ElseIf MainForm.VgMe.IsMainReaderBusy Then
            Exit Sub
        ElseIf VpMultiverseId <> 0 AndAlso VgOptions.VgSettings.PicturesSource = ePicturesSource.Online Then
            RemoveHandler VppicScanCard.LoadCompleted, AddressOf LoadScanCardCompleted
            AddHandler VppicScanCard.LoadCompleted, AddressOf LoadScanCardCompleted
            Try
                ServicePointManager.SecurityProtocol = &H00000C00   'TLS 1.2
                VppicScanCard.LoadAsync(mdlConstGlob.CgURL0.Replace("#", VpMultiverseId.ToString))
            Catch
                Call mdlToolbox.ShowWarning(mdlConstGlob.CgDL3b)
            End Try
        ElseIf File.Exists(VgOptions.VgSettings.PicturesFile) Then
            If (New FileInfo(VgOptions.VgSettings.PicturesFile)).Length < CgImgMinLength Then
                If Not VpSave Then
                    VppicScanCard.Image = Nothing
                End If
                Call ShowWarning("La base d'images ne possède pas la taille minimale requise." + vbCrLf + "Vérifiez le fichier spécifié dans les Préférences...")
                Exit Sub
            End If
            VpPicturesFile = New StreamReader(VgOptions.VgSettings.PicturesFile)
            VpPicturesFileB = New BinaryReader(VpPicturesFile.BaseStream)
            VpTmpFile = New StreamWriter(VpTmp)
            VpTmpFileB = New BinaryWriter(VpTmpFile.BaseStream)
            VgDBCommand.CommandText = "Select [Offset], [End] From CardPictures Where Title = '" + AvoidForbiddenChr(VpTitle, eForbiddenCharset.BDD) + "' Order By [End] Desc;"
            Try
                VgDBReader = VgDBCommand.ExecuteReader
                VgDBReader.Read
                If VgDBReader.HasRows Then
                    VpOffset = CLng(VgDBReader.GetValue(0))
                    VpEnd = CLng(VgDBReader.GetValue(1))
                    If VpOffset > 0 Then
                        VpPicturesFileB.BaseStream.Seek(VpOffset, SeekOrigin.Begin)
                    End If
                    VpTmpFileB.Write(VpPicturesFileB.ReadBytes(VpEnd - VpOffset + 1))
                    VpTmpFileB.Flush
                    VpTmpFileB.Close
                    VpPicturesFileB.Close
                    If Not VpSave Then
                        Try
                            VppicScanCard.Image = Image.FromFile(VpTmp)
                        Catch
                            If VgOptions.VgSettings.ShowCorruption Then
                                Call ShowWarning("La base d'images semble être corrompue." + vbCrLf + "Essayez de la mettre à jour ou de la re-télécharger...")
                            End If
                            VppicScanCard.Image = Nothing
                            VgDBReader.Close
                            Exit Sub
                        End Try
                    Else
                        VpDest = VpSaveFolder + "\" + AvoidForbiddenChr(VpTitle) + ".jpg"
                        If (Not File.Exists(VpDest)) OrElse (New FileInfo(VpDest)).Length = 0 Then
                            Try
                                File.Copy(VpTmp, VpDest)
                            Catch
                            End Try
                        End If
                    End If
                Else
                    If Not VpSave Then
                        VppicScanCard.Image = Nothing
                    End If
                End If
            Catch VpEx As OleDbException
                If VpEx.ErrorCode = CgMissingTable Then
                    VpMissingTable = True
                End If
                If Not VpSave Then
                    VppicScanCard.Image = Nothing
                End If
            End Try
            VgDBReader.Close
            'Fichier présent mais table d'index absente
            If VpMissingTable Then
                Select Case ShowQuestion("Cette version du logiciel est capable de gérer les images des cartes mais la base de données n'est pas à jour." + vbCrLf + "Voulez-vous télécharger les informations manquantes maintenant ?" + vbCrLf + "Cliquez sur 'Annuler' pour ne plus afficher ce message...", MessageBoxButtons.YesNoCancel)
                    Case DialogResult.Yes
                        Call DownloadUpdate(New Uri(VgOptions.VgSettings.DownloadServer + CgURL3), CgUpDDB)
                    Case DialogResult.Cancel
                        VgOptions.VgSettings.PicturesFile = ""
                    Case Else
                End Select
                Exit Sub
            End If
        End If
    End Sub
    Private Sub LoadScanCardCompleted(sender As Object, e As AsyncCompletedEventArgs)
    '-------------------------------------------------------------------------------------------
    'Vérifie que l'image retournée par le Gatherer n'est pas trop grande ou s'il faut la réduire
    '-------------------------------------------------------------------------------------------
    Dim VppicScanCard As PictureBox = sender
        With VppicScanCard
            If .Image.Height >= CgMTGCardHeight_px * 1.1 Then
                .Image = New Bitmap(.Image, 223, 310)   '223x310 est déterminé empiriquement de façon à ne pas afficher trop de bordure
            End If
        End With
    End Sub
    Public Function HasBorder(VpPath As String) As Boolean
    '-------------------------------------------------------------------------------------------------------
    'Regarde si l'image dont le chemin est passé en paramètre contient une bordure noire ou blanche continue
    '-------------------------------------------------------------------------------------------------------
    Const CpBorderSize As Integer = 8
    Dim VpBitmap As Bitmap
    Dim VpColor As Color
    Dim VpCount As Integer = 0
    Dim VpW As Integer = 0
    Dim VpB As Integer = 0
        If File.Exists(VpPath) Then
            Try
                VpBitmap = Bitmap.FromFile(VpPath)
            Catch
                Return True
            End Try
            'Regarde le nombre de pixels noirs (ou blancs) sur une bordure de 4 pixels autour de la carte
            For VpX As Integer = 0 To VpBitmap.Width - 1
                For VpY As Integer = 0 To VpBitmap.Height - 1
                    If VpX < CpBorderSize OrElse VpY < CpBorderSize OrElse VpX > (VpBitmap.Width - 1) - CpBorderSize OrElse VpY > (VpBitmap.Height - 1) - CpBorderSize Then
                        VpCount += 1
                        VpColor = VpBitmap.GetPixel(VpX, VpY)
                        'Pixel noir
                        If VpColor.R < 40 AndAlso VpColor.G < 40 AndAlso VpColor.B < 40 Then
                            VpB += 1
                        'Pixel blanc
                        ElseIf VpColor.R > 215 AndAlso VpColor.G > 215 AndAlso VpColor.B > 215 Then
                            VpW += 1
                        End If
                    End If
                Next VpY
            Next VpX
            'En déduit s'il y a une bordure ou pas
            Return ( (VpB / VpCount > 0.5 AndAlso VpW / VpCount < 0.1) OrElse (VpW / VpCount > 0.5 AndAlso VpB / VpCount < 0.1) )
        Else
            Return True
        End If
    End Function
    #End Region
    #Region "Gestion des fichiers temporaires"
    Public Function GetFreeTempFile(VpExt As String) As String
    '--------------------------------------------------
    'Retourne un nom de fichier temporaire image valide
    '--------------------------------------------------
        With VgSessionSettings
            If .FreeTempFileIndex = -1 Then
                Do
                    .FreeTempFileIndex += 1
                Loop While File.Exists(Path.GetTempPath + "\mtgm~" + .FreeTempFileIndex.ToString + VpExt)
            Else
                .FreeTempFileIndex += 1
            End If
            Return Path.GetTempPath + "\mtgm~" + .FreeTempFileIndex.ToString + VpExt
        End With
    End Function
    Public Sub DeleteTempFiles(Optional VpSilent As Boolean = False)
    '------------------------------------
    'Suppression des fichiers temporaires
    '------------------------------------
        'Images
        Try
            For Each VpFile As FileInfo In (New DirectoryInfo(Path.GetTempPath)).GetFiles("mtgm~*.jpg")
                Call SecureDelete(VpFile.FullName)
            Next VpFile
        Catch
            If Not VpSilent Then
                Call ShowWarning("Impossible d'accéder au répertoire temporaire...")
            End If
        End Try
        'Logs
        Try
            For Each VpFile As FileInfo In (New DirectoryInfo(Path.GetTempPath)).GetFiles("mtgm~*.txt")
                Call SecureDelete(VpFile.FullName)
            Next VpFile
        Catch
            If Not VpSilent Then
                Call ShowWarning("Impossible d'accéder au répertoire temporaire...")
            End If
        End Try
        'Updates
        Call SecureDelete(Application.StartupPath + CgUpMultiverse)
        Call SecureDelete(Application.StartupPath + CgUpMultiverse2)
        Call SecureDelete(Application.StartupPath + CgUpRulings)
        Call SecureDelete(Application.StartupPath + CgUpTXTFR)
        Call SecureDelete(Application.StartupPath + CgUpDFile)
        Call SecureDelete(Application.StartupPath + CgUpDDB)
        Call SecureDelete(Application.StartupPath + CgUpDDBb)
        Call SecureDelete(Application.StartupPath + CgUpPrices)
        Call SecureDelete(Application.StartupPath + CgUpSeries)
        Call SecureDelete(Application.StartupPath + CgUpPic + CgPicLogExt)
        Call SecureDelete(Application.StartupPath + CgUpPic + CgPicUpExt)
        Call SecureDelete(Application.StartupPath + CgMdSubTypes)
        Call SecureDelete(Application.StartupPath + CgMdSubTypesVF)
        Call SecureDelete(Application.StartupPath + CgMdShippingCosts)
        For Each VpFile As FileInfo In (New DirectoryInfo(Application.StartupPath)).GetFiles("*_en.txt")
            Call SecureDelete(VpFile.FullName)
        Next VpFile
        For Each VpFile As FileInfo In (New DirectoryInfo(Application.StartupPath)).GetFiles("*_fr.txt")
            Call SecureDelete(VpFile.FullName)
        Next VpFile
    End Sub
    Public Sub SecureDelete(VpFile As String)
    '----------------------------------------------------------------
    'Suppression du fichier passé en paramètre (avec trappe d'erreur)
    '----------------------------------------------------------------
        Try
            File.Delete(VpFile)
        Catch
        End Try
    End Sub
    #End Region
End Module
