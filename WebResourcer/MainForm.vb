'---------------------------------------------------------------
'| Projet         | Magic The Gathering Manager - WebResourcer |
'| Contexte       |         Perso                              |
'| Date           |                                 30/03/2008 |
'| Release 1      |                                 12/04/2008 |
'| Release 2      |                                 30/08/2008 |
'| Release 3      |                                 08/11/2008 |
'| Release 4      |                                 29/08/2009 |
'| Release 5      |                                 21/03/2010 |
'| Release 6      |                                 17/04/2010 |
'| Release 7      |                                 29/07/2010 |
'| Release 8      |                                 03/10/2010 |
'| Release 9      |                                 05/02/2011 |
'| Release 10     |                                 10/09/2011 |
'| Release 11     |                                 24/01/2012 |
'| Release 12     |                                 01/10/2012 |
'| Release 13     |                                 09/05/2014 |
'| Release 14     |                                 09/05/2015 |
'| Release 15     |                                 15/01/2017 |
'| Auteur         |                                   Couitchy |
'|-------------------------------------------------------------|
'| Modifications :                                             |
'---------------------------------------------------------------
Imports System.Data
Imports System.Data.OleDb
Imports System.Net
Imports System.IO
Imports System.Text
Imports System.Threading
Imports System.Web.Script.Serialization
Public Partial Class MainForm
    Private Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA"(lpApplicationName As String, lpKeyName As String, lpString As String, ByVal lpFileName As String) As Integer
    Private Const CmStrConn As String       = "Provider=Microsoft.Jet.OLEDB.4.0;OLE DB Services=-1;Data Source="
    Private Const CmURL0 As String          = "http://magic-ville.fr/fr/"
    Private Const CmURL1 As String          = "http://www.magiccorporation.com/mc.php?rub=cartes&op=search&word=#cardname#&search=2"
    Private Const CmURL2 As String          = "http://www.magiccorporation.com/gathering-cartes-view"
    Private Const CmURL3 As String          = "http://www.magiccorporation.com/scan/"
    Private Const CmURL4 As String          = "http://www.magiccorporation.com"
    Private Const CmURL5 As String          = "http://magiccards.info/###/^^.html"
    Private Const CmURL6 As String          = "http://magiccards.info/query?q=%2B%2Be%3A###%2Fen&v=spoiler&s=issue"
    Private Const CmURL7 As String          = "https://mtgjson.com/json/###.json"
    Private Const CmURL8 As String          = "https://www.cardmarket.com/en/Help/ShippingCosts?origin=#country#&destination=FR#Show_shipping_costs"
    Private Const CmId1 As String           = "#cardname#"
    Private Const CmId2 As String           = "#country#"
    Private Const CmKey0 As String          = "recherche_titre"
    Private Const CmKey1 As String          = "gathering-cartes-view"
    Private Const CmKey2 As String          = "NM/MT"
    Private Const CmKey2A As String         = "Nm</td><td>VF"
    Private Const CmKey2B As String         = "Premium"
    Private Const CmKey2C As String         = ">VO<"
    Private Const CmKey3 As String          = "src=""/scan/"
    Private Const CmKey4 As String          = "src=""http://www.wizards.com/global/images/magic"
    Private Const CmKey4B As String         = "href=""/images/cartes/illustrations"
    Private Const CmKey5 As String          = "Texte Fran�ais"
    Private Const CmKey6 As String          = "/###/^^/"
    Private Const CmKey6B As String         = ".html"">"
    Private Const CmKey7 As String          = "<td>"
    Private Const CmKey7B As String         = "</td>"
    Private Const CmKey7C As String         = "<img src=""/images/en.gif"" alt=""English"" width=""16"" height=""11"" class=""flag2""> "
    Private Const CmKey8 As String          = "<p"
    Private Const CmKey8B As String         = "<b>"
    Private Const CmKey8C As String         = "</b></p>"
    Private Const CmKey8D As String         = "<br><br>"
    Private Const CmKey8E As String         = ", <i>"
    Private Const CmKey8F As String         = "</i></p>"
    Private Const CmKey9 As String          = "<tbody><tr><td>"
    Private Const CmKey9B As String         = "</td><td>"
    Private Const CmKey9C As String         = "</td></tr><tr><td>"
    Private Const CmFrench  As Integer      = 2
    Private Const CmMe As String            = "Moi"
    Private Const CmStamp As String         = "ContenuStamp r14.txt"
    Private Const CmCategory As String      = "Properties"
    Private CmFields() As String            = {"LastUpdateAut", "LastUpdateSimu", "LastUpdateTxtVF", "LastUpdateTradPatch"}
    Private CmIndexes() As Integer          = {2, 3, 4, 7}
    Private CmIgnoredEditions() As String   = {"Premium Deck Seri...", "Archenemy Decks", "Duels of the Plan...", "Battle Royale", "Anthologies", "Promotional Card", "Friday Night Magic", "Prerelease Cards", "Magic Player Rewards"}
    Private CmCountriesName() As String     = {"Austria", "Belgium", "Bulgaria", "Croatia", "Cyprus", "Czech Republic", "Denmark", "Estonia", "Finland", "France", "Germany", "Greece", "Hungary", "Iceland", "Ireland", "Italy", "Latvia", "Liechtenstein", "Lithuania", "Luxembourg", "Malta", "Netherlands", "Norway", "Poland", "Portugal", "Romania", "Slovakia", "Slovenia", "Spain", "Sweden", "Switzerland", "United Kingdom"}
    Private CmCountriesId() As String       = {"AT", "BE", "BG", "HR", "CY", "CZ", "DK", "EE", "FI", "FR", "D", "GR", "HU", "IS", "IE", "IT", "LV", "LI", "LT", "LU", "MT", "NL", "NO", "PL", "PT", "RO", "SK", "SI", "ES", "SE", "CH", "GB"}
    Private VmDB As OleDbConnection
    Private VmDBCommand As New OleDbCommand
    Private VmDBReader As OleDbDataReader
    Private VmStart As Date
    Private VmIsComplete As Boolean = False
    Private Enum eLogType
        Information = 0
        Warning
        MyError
    End Enum
    Public Sub New
        Call Me.InitializeComponent
        Me.wbMV.ScriptErrorsSuppressed = True
    End Sub
    Private Sub AddToLog(VpText As String, VpType As eLogType, Optional VpNewOp As Boolean = False, Optional VpEndOp As Boolean = False)
    '-----------------------------
    'Ajout d'une entr�e au journal
    '-----------------------------
    Dim VpLog As ListViewItem
        If VpNewOp Then
            VmStart = Now
            Me.btCancel.Enabled = True
            Me.btCancel.Tag = False
        ElseIf VpEndOp Then
            Me.btCancel.Tag = False
            Me.btCancel.Enabled = False
            Me.tabMain.SelectedIndex = 0
            Me.prgAvance.Style = ProgressBarStyle.Blocks
            Me.prgAvance.Value = 0
        End If
        VpLog = New ListViewItem(Now.ToLongTimeString, CInt(VpType))
        VpLog.SubItems.Add(VpText)
        Me.lvwLog.Items.Add(VpLog)
        VpLog.EnsureVisible
    End Sub
    Private Sub ETA
    '-------------------------------------------------------
    'Estimation du temps restant pour le traitement en cours
    '-------------------------------------------------------
    Dim VpETA As TimeSpan
        If Me.prgAvance.Value > 0 Then
            VpETA = New TimeSpan(0, 0, Now.Subtract(VmStart).TotalSeconds * (Me.prgAvance.Maximum / Me.prgAvance.Value - 1))
            Me.txtETA.Text = Format(VpETA.Hours, "00") + ":" + Format(VpETA.Minutes, "00") + ":" + Format(VpETA.Seconds, "00")
        End If
    End Sub
    Private Sub GetShippingCosts
    '--------------------------------------------------------------------
    'R�cup�re les frais de port indiqu�s sur le site de Magic Card Market
    '--------------------------------------------------------------------
    Dim VpStr As String = ""
    Dim VpStrs() As String
    Dim VpStrs2() As String
    Dim VpOut As StreamWriter
        Me.dlgSave.FileName = ""
        Me.dlgSave.ShowDialog
        If Me.dlgSave.FileName <> "" Then
            VpOut = New StreamWriter(Me.dlgSave.FileName)
            Call Me.AddToLog("La r�cup�ration des frais de port a commenc�...", eLogType.Information, True)
            Application.DoEvents
            VpOut.WriteLine("#Country;Code;Value max.;Quantity max.;Cost")
            For Each VpCountry As String In CmCountriesId
                VpStr = Me.HTMLfromRequest(CmURL8.Replace(CmId2, VpCountry))
                VpStr = VpStr.Substring(VpStr.IndexOf(CmKey9) + 15).Replace("&#x20AC;", "�").Replace(CmKey9B, ";")
                VpStrs = VpStr.Split(New String() {CmKey9C}, StringSplitOptions.None)
                For Each VpStr2 As String In VpStrs
                    VpStrs2 = VpStr2.Split(";")
                    If VpStrs2.Length = 6 AndAlso Val(VpStrs2(5)) <> 0 AndAlso Val(VpStrs2(3)) <= 100000 Then
                        VpOut.WriteLine(CmCountriesName(Array.IndexOf(CmCountriesId, VpCountry)) + ";" + VpCountry + ";" + Val(VpStrs2(2).Replace(",", ".")).ToString + ";" + Me.WeightToCards(Val(VpStrs2(3))).ToString + ";" + Val(VpStrs2(5).Replace(",", ".")).ToString)
                    End If
                Next VpStr2
                Application.DoEvents
            Next VpCountry
            VpOut.Flush
            VpOut.Close
            Call Me.AddToLog("La r�cup�ration des frais de port est termin�e.", eLogType.Information, , True)
        End If
    End Sub
    Private Function WeightToCards(VpWeight As Integer) As Integer
        Select Case VpWeight
            Case Is <= 20
                Return 4
            Case Is <= 50
                Return 17
            Case Is <= 100
                Return 40
            Case Is <= 100000
                Return 50000
            Case Else
                Return Integer.MaxValue
        End Select
    End Function
    Private Function HTMLfromRequest(VpURL As String) As String
    '------------------------------------------------------------------
    'R�cup�re le code HTML en r�ponse de la requ�te pass�e en param�tre
    '------------------------------------------------------------------
    Dim VpRequest As HttpWebRequest
    Dim VpAnswer As Stream
    Dim VpCurByte As Integer
    Dim VpStr As String = ""
        VpRequest = WebRequest.Create(VpURL)
        Try
            VpRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:47.0) Gecko/20100101 Firefox/47.0"
            VpAnswer = VpRequest.GetResponse().GetResponseStream()
            VpCurByte = VpAnswer.ReadByte
            While VpCurByte <> -1
                VpStr = VpStr + (Encoding.Default.GetString(New Byte() {VpCurByte}))
                VpCurByte = VpAnswer.ReadByte
                Application.DoEvents
            End While
        Catch
            Call Me.AddToLog("Erreur de connexion. Recherche du HTML source pour " + VpURL + "...", eLogType.Warning)
            Me.dlgOpen4.FileName = ""
            Me.dlgOpen4.ShowDialog
            If Me.dlgOpen4.FileName <> "" Then
                VpStr = (New StreamReader(Me.dlgOpen4.FileName)).ReadToEnd
            End If
        End Try
        Return VpStr
    End Function
    Private Function GetPrice(VpIn As String) As String
    '---------------------------------------------------
    'Retourne les prix pour la carte pass�e en param�tre
    '---------------------------------------------------
    Dim VpStr As String = ""
    Dim VpStrs() As String
    Dim VpPrices As String = ""
    Dim VpIr As Integer
    Dim VpVoid As String
    Dim VpIn2 As String = VpIn.Replace(" ", "+").Replace("""", "").Replace("�", "u").Replace("�", "a").Replace("�", "a")
    Dim VpItem As clsPriceInfos
    Dim VpFoil As Boolean
    Dim VpBigList As New SortedDictionary(Of String, List(Of clsPriceInfos))
    Dim VpBigListFoil As New SortedDictionary(Of String, List(Of clsPriceInfos))
    Dim VpList As List(Of clsPriceInfos)
    Dim VpListFoil As List(Of clsPriceInfos)
        Try
            If VpIn2.StartsWith("�") Then
                VpIn2 = VpIn2.Substring(1)
            ElseIf VpIn2.Contains("�") Then
                VpVoid = VpIn2.Substring(0, VpIn2.IndexOf("�") + 1)
                VpIn2 = VpIn2.Replace(VpVoid, "")
            End If
            VpStr = Me.HTMLfromRequest(CmURL1.Replace(CmId1, VpIn2))
            VpStrs = VpStr.Split(New String() {CmKey1}, StringSplitOptions.None)
            VpIr = Me.FindRightIndex(VpStrs, VpIn.Replace("�", "u"))
            VpStr = CmURL2 + VpStrs(VpIr).Substring(0, VpStrs(VpIr).IndexOf(""""))
            VpStr = Me.HTMLfromRequest(VpStr)
            VpStr = VpStr.Substring(VpStr.IndexOf("Disponibilit�s"))
            VpStrs = VpStr.Split(New String() {"/sigles/"}, StringSplitOptions.None)
            For Each VpStr In VpStrs
                VpItem = New clsPriceInfos
                If VpStr.Contains(".png") Then
                    VpStr = VpStr.Substring(VpStr.IndexOf(".png"))
                    VpStr = VpStr.Substring(VpStr.IndexOf(">") + 1)
                    VpItem.Serie = VpStr.Substring(0, VpStr.IndexOf("<"))
                    VpStr = VpStr.Substring(VpStr.IndexOf("<td>") + 4)
                    VpItem.Condition = VpStr.Substring(0, VpStr.IndexOf("<"))
                    If VpStr.Contains("lang/vf.gif") Then
                        VpItem.Language = "VF"
                    ElseIf VpStr.Contains("lang/vo.gif") Then
                        VpItem.Language = "VO"
                    End If
                    VpFoil = VpStr.Contains("/logo_foil.png")
                    VpStr = VpStr.Substring(0, VpStr.IndexOf("�") + 1)
                    VpItem.Price = VpStr.Substring(VpStr.LastIndexOf(">") + 1)
                    If VpFoil Then
                        If VpBigListFoil.ContainsKey(VpItem.Serie) Then
                            VpListFoil = VpBigListFoil.Item(VpItem.Serie)
                        Else
                            VpListFoil = New List(Of clsPriceInfos)
                            VpBigListFoil.Add(VpItem.Serie, VpListFoil)
                        End If
                        VpListFoil.Add(VpItem)
                    Else
                        If VpBigList.ContainsKey(VpItem.Serie) Then
                            VpList = VpBigList.Item(VpItem.Serie)
                        Else
                            VpList = New List(Of clsPriceInfos)
                            VpBigList.Add(VpItem.Serie, VpList)
                        End If
                        VpList.Add(VpItem)
                    End If
                End If
            Next VpStr
            For Each VpList In VpBigList.Values
                VpList.Sort(New clsPriceInfos.clsPriceInfosComparer)
            Next VpList
            For Each VpListFoil In VpBigListFoil.Values
                VpListFoil.Sort(New clsPriceInfos.clsPriceInfosComparer)
            Next VpListFoil
            For Each VpList In VpBigList.Values
                If VpList.Count > 0 Then
                    VpItem = VpList.Item(0)
                    VpPrices = VpPrices + VpItem.Serie + "^" + VpItem.Price + "#"
                End If
            Next VpList
            For Each VpListFoil In VpBigListFoil.Values
                If VpListFoil.Count > 0 Then
                    VpItem = VpListFoil.Item(0)
                    VpPrices = VpPrices + VpItem.Serie + "PREMIUMFOILVO^" + VpItem.Price + "#"
                End If
            Next VpListFoil
        Catch
        End Try
        VpPrices = VpPrices.Trim
        If VpPrices.EndsWith("#") Then
            Return VpPrices
        Else
            Return VpPrices + "#"
        End If
    End Function
    Private Function FindRightIndex(VpT() As String, VpIn As String) As Integer
    Dim VpA As String = VpIn.ToLower.Trim
    Dim VpB As String = ""
        For VpI As Integer = 0 To VpT.Length - 1
            Try
                VpB = VpT(VpI).Substring(VpT(VpI).IndexOf(">") + 1)
                VpB = VpB.Substring(0, VpB.IndexOf("<"))
            Catch
            End Try
            If VpB.ToLower.Trim = VpA Then
                Return VpI
            End If
        Next VpI
        Return 1
    End Function
    Private Function BuildListeFromDB(Optional VpStartAfter As String = "") As List(Of String)
    Dim VpCards As New List(Of String)
    Dim VpCanAdd As Boolean = ( VpStartAfter = "" )
        Me.prgAvance.Style = ProgressBarStyle.Marquee
        VmDBCommand.CommandText = "Select Distinct Title From Card Order By Title Asc;"
        VmDBReader = VmDBCommand.ExecuteReader
        With VmDBReader
            While .Read
                If VpCanAdd Then
                    VpCards.Add(.GetString(0))
                Else
                    VpCanAdd = ( .GetString(0) = VpStartAfter )
                End If
                Application.DoEvents
            End While
            .Close
        End With
        Return VpCards
    End Function
    Private Function BuildListeFromFile(Optional VpCut As Boolean = False) As List(Of String)
    Dim VpCards As New StreamReader(Me.dlgOpen4.FileName)
    Dim VpListe As New List(Of String)
    Dim VpStr As String
        Me.prgAvance.Style = ProgressBarStyle.Marquee
        While Not VpCards.EndOfStream
            If Not VpCut Then
                VpListe.Add(VpCards.ReadLine)
            Else
                VpStr = VpCards.ReadLine
                If VpStr.Contains("#") Then
                    VpListe.Add(VpStr.Split("#")(0))
                End If
            End If
            Application.DoEvents
        End While
        VpCards.Close
        Return VpListe
    End Function
    Private Sub UpdatePrices(VpAll As Boolean)
    '------------------------------------------------
    'Mise � jour de la liste des prix depuis Internet
    '------------------------------------------------
    Dim VpCards As List(Of String)
    Dim VpOut As StreamWriter
    Dim VpPrices As String
    Dim VpAppend As Boolean
    Dim VpAlready() As String
    Dim VpLast As String = ""
        Me.dlgSave.FileName = ""
        Me.dlgSave.ShowDialog
        If Me.dlgSave.FileName <> "" Then
            VpAppend = File.Exists(Me.dlgSave.FileName)
            If VpAppend Then
                'Si le fichier existe d�j�, regarde la derni�re carte qui a �t� trait�e
                VpAlready = File.ReadAllLines(Me.dlgSave.FileName)
                If VpAlready.Length > 2 Then
                    If VpAlready(VpAlready.Length - 1).Contains("#") Then
                        VpLast = VpAlready(VpAlready.Length - 1)
                    Else
                        VpLast = VpAlready(VpAlready.Length - 2)
                    End If
                    VpLast = VpLast.Substring(0, VpLast.IndexOf("#"))
                Else
                    VpAppend = False
                End If
            End If
            VpOut = New StreamWriter(Me.dlgSave.FileName, VpAppend)
            If VpAppend Then
                Call Me.AddToLog("La r�cup�ration des prix se poursuit...", eLogType.Information, True)
            Else
                Call Me.AddToLog("La r�cup�ration des prix a commenc�...", eLogType.Information, True)
                'Inscription de la date
                VpOut.WriteLine(Now.ToShortDateString)
            End If
            'R�cup�re la liste des cartes
            If VpAll Then
                VpCards = Me.BuildListeFromDB(VpLast)
            Else
                Me.dlgOpen4.FileName = ""
                Me.dlgOpen4.ShowDialog
                If Me.dlgOpen4.FileName <> "" Then
                    VpCards = Me.BuildListeFromFile
                Else
                    VpOut.Close
                    Exit Sub
                End If
            End If
            'R�cup�re le prix pour chaque carte
            Me.prgAvance.Maximum = VpCards.Count
            Me.prgAvance.Value = 0
            Me.prgAvance.Style = ProgressBarStyle.Blocks
            For Each VpCard As String In VpCards
                If Not VpCard.StartsWith("_") Then
                    Me.txtCur.Text = VpCard
                    Application.DoEvents
                    VpPrices = Me.GetPrice(VpCard)
                    If VpPrices = "#" Then
                        Call Me.AddToLog("Impossible de r�cup�rer les prix pour la carte : " + VpCard, eLogType.Warning)
                    Else
                        VpOut.WriteLine(VpCard + "#" + VpPrices)
                    End If
                End If
                Me.prgAvance.Increment(1)
                Call Me.ETA
                If Me.btCancel.Tag Then Exit For
            Next VpCard
            VpOut.Flush
            VpOut.Close
            If Me.btCancel.Tag Then
                Call Me.AddToLog("La r�cup�ration des prix a �t� annul�e.", eLogType.Warning, , True)
            Else
                Call Me.AddToLog("La r�cup�ration des prix est termin�e.", eLogType.Information, , True)
            End If
        End If
    End Sub
    Private Sub FillPricesHistory(VpPath As String)
    '----------------------------------------------------------------------------------------------------------------------------------------------------------------------
    'Actualisation de la table de l'historique des prix : si un prix a chang� depuis son dernier enregistrement, ajoute une entr�e avec la nouvelle date et le nouveau prix
    '----------------------------------------------------------------------------------------------------------------------------------------------------------------------
    Dim VpPrices As StreamReader
    Dim VpCardData() As String
    Dim VpPrice As String
    Dim VpEdition As String
    Dim VpEdition0 As String
    Dim VpEditionsOK As New List(Of String)
    Dim VpEditionsKO As New Hashtable
    Dim VpCardName As String
    Dim VpDate As String
    Dim VpEncNbr As Long
    Dim VpO As Object
    Dim VpFoilTarget As Boolean = False
        VpPrices = New StreamReader(VpPath)
        Call Me.AddToLog("La mise � jour de l'historique des prix a commenc�...", eLogType.Information, True)
        Me.prgAvance.Style = ProgressBarStyle.Marquee
        VpDate = VpPrices.ReadLine
        If Not IsDate(VpDate) Then
            VpDate = File.GetLastWriteTimeUtc(VpPath).ToShortDateString
            VpPrices.BaseStream.Seek(0, SeekOrigin.Begin)
        End If
        VpDate = "'" + CDate(VpDate).Day.ToString + "/" + CDate(VpDate).Month.ToString + "/" + CDate(VpDate).Year.ToString.Substring(2, 2) + "'"
        While Not VpPrices.EndOfStream
            Application.DoEvents
            VpCardData = VpPrices.ReadLine.Split("#")
            VpCardName = ""
            VpEdition = ""
            VpEditionsOK.Clear
            VpPrice = ""
            For Each VpStr As String In VpCardData
                If VpStr.IndexOf("^") <> -1 Then
                    VpEdition = VpStr.Substring(0, VpStr.IndexOf("^")).Replace("'", "''")
                    VpEdition0 = VpEdition.Replace("PREMIUMFOILVO", "")
                    If Not VpEditionsOK.Contains(VpEdition) Then    'fait en sorte de ne prendre que le premier prix par �dition (correspondant � la qualit� de carte NM/MT)
                        VpEditionsOK.Add(VpEdition)
                        VpPrice = VpStr.Substring(VpStr.IndexOf("^") + 1).Replace("�", "").Trim
                        If VpEdition.EndsWith("PREMIUMFOILVO") Then
                            VpEdition = VpEdition.Replace("PREMIUMFOILVO", "")
                            VpFoilTarget = True
                        Else
                            VpFoilTarget = False
                        End If
                        If VpEdition.Contains("...") Then
                            VpEdition = VpEdition.Replace("...", "")
                            VmDBCommand.CommandText = "Select EncNbr From Card Inner Join Series On Card.Series = Series.SeriesCD Where InStr(Series.SeriesNM_MtG, '" + VpEdition + "') > 0 And Card.Title = '" + VpCardName + "';"
                        Else
                            VmDBCommand.CommandText = "Select EncNbr From Card Inner Join Series On Card.Series = Series.SeriesCD Where Series.SeriesNM_MtG = '" + VpEdition + "' And Card.Title = '" + VpCardName + "';"
                        End If
                        VpEncNbr = VmDBCommand.ExecuteScalar
                        If VpEncNbr <> 0 Then
                            If VpFoilTarget Then
                                VmDBCommand.CommandText = "Select Price From PricesHistory Where EncNbr = " + VpEncNbr.ToString + " And Foil = True Order By PriceDate Desc;"
                                VpO = VmDBCommand.ExecuteScalar
                                If Not VpO Is Nothing Then
                                    If Format(VpO, "0.00").Replace(",", ".") <> VpPrice Then
                                        VmDBCommand.CommandText = "Insert Into PricesHistory(EncNbr, PriceDate, Price, Foil) Values (" + VpEncNbr.ToString + ", " + VpDate + ", " + VpPrice + ", True);"
                                        VmDBCommand.ExecuteNonQuery
                                        Call Me.AddToLog("Changement de prix pour la carte " + VpCardName + " - " + VpEdition + " (foil) : " + VpPrice + "�" , eLogType.Information)
                                    End If
                                Else
                                    VmDBCommand.CommandText = "Insert Into PricesHistory(EncNbr, PriceDate, Price, Foil) Values (" + VpEncNbr.ToString + ", " + VpDate + ", " + VpPrice + ", True);"
                                    VmDBCommand.ExecuteNonQuery
                                    Call Me.AddToLog("Nouveau prix pour la carte " + VpCardName + " - " + VpEdition + " (foil) : " + VpPrice + "�" , eLogType.Information)
                                End If
                            Else
                                VmDBCommand.CommandText = "Select Price From PricesHistory Where EncNbr = " + VpEncNbr.ToString + " And Foil = False Order By PriceDate Desc;"
                                VpO = VmDBCommand.ExecuteScalar
                                If Not VpO Is Nothing Then
                                    If Format(VpO, "0.00").Replace(",", ".") <> VpPrice Then
                                        VmDBCommand.CommandText = "Insert Into PricesHistory(EncNbr, PriceDate, Price, Foil) Values (" + VpEncNbr.ToString + ", " + VpDate + ", " + VpPrice + ", False);"
                                        VmDBCommand.ExecuteNonQuery
                                        Call Me.AddToLog("Changement de prix pour la carte " + VpCardName + " - " + VpEdition + " : " + VpPrice + "�" , eLogType.Information)
                                    End If
                                Else
                                    VmDBCommand.CommandText = "Insert Into PricesHistory(EncNbr, PriceDate, Price, Foil) Values (" + VpEncNbr.ToString + ", " + VpDate + ", " + VpPrice + ", False);"
                                    VmDBCommand.ExecuteNonQuery
                                    Call Me.AddToLog("Nouveau prix pour la carte " + VpCardName + " - " + VpEdition + " : " + VpPrice + "�" , eLogType.Information)
                                End If
                            End If
                        Else
                            If Array.IndexOf(CmIgnoredEditions, VpEdition) < 0 Then
                                If VpEditionsKO.Contains(VpEdition0) Then
                                    VpEditionsKO.Item(VpEdition0) += 1
                                Else
                                    VpEditionsKO.Add(VpEdition0, 1)
                                End If
                                Call Me.AddToLog("Pas de correspondance de prix trouv�e pour la carte " + VpCardName + " - " + VpEdition, eLogType.Warning)
                            End If
                        End If
                    End If
                Else
                    VpCardName = VpStr.Replace("'", "''")
                    Me.txtCur.Text = VpStr
                End If
            Next VpStr
            If Me.btCancel.Tag Then Exit While
        End While
        VpPrices.Close
        If Me.btCancel.Tag Then
            Call Me.AddToLog("La mise � jour de l'historique des prix a �t� annul�e.", eLogType.Warning, , True)
        Else
            For Each VpEdition0 In VpEditionsKO.Keys
                Call Me.AddToLog("Edition non trouv�e : " + VpEdition0 + " (" + VpEditionsKO.Item(VpEdition0).ToString + ")", eLogType.Warning)
            Next VpEdition0
            Call Me.AddToLog("La mise � jour de l'historique des prix est termin�e.", eLogType.Information, , True)
        End If
    End Sub
    Private Sub FixTxtVO
    '----------------------------------------------------------------------------------------------------------------
    'Corrige les textes des cartes en anglais dont seule la premi�re ligne aurait �t� prise depuis le fichier spoiler
    '----------------------------------------------------------------------------------------------------------------
    Dim VpCurSpoiler As StreamReader
    Dim VpCategory As Boolean = False
    Dim VpStr As String
    Dim VpCard As String = ""
    Dim VpSeriesCD As String = ""
    Dim VpNewTxt As String = ""
        Me.dlgBrowse.SelectedPath = ""
        Me.dlgBrowse.ShowDialog
        If Me.dlgBrowse.SelectedPath <> "" Then
            Call Me.AddToLog("La mise � jour des textes VO multilignes a commenc�...", eLogType.Information, True)
            For Each VpFile As String In Directory.GetFiles(Me.dlgBrowse.SelectedPath, "*spoiler_en.txt")
                VpSeriesCD = VpFile.Substring(VpFile.LastIndexOf("\") + 1)
                VpSeriesCD = Me.SerieCode(VpSeriesCD.Substring(0, VpSeriesCD.IndexOf("_")))
                VpCurSpoiler = New StreamReader(VpFile, Encoding.Default)
                While Not VpCurSpoiler.EndOfStream
                    Application.DoEvents
                    VpStr = VpCurSpoiler.ReadLine
                    If VpStr.StartsWith("CardName:") Or VpStr.StartsWith("Card Name:") Or VpStr.StartsWith("Name:") Then
                        VpCard = VpStr.Substring(VpStr.IndexOf(":") + 1).Trim
                    ElseIf VpStr.StartsWith("Rules Text:") Then
                        VpCategory = True
                        VpNewTxt = VpStr.Substring(11).Trim
                    ElseIf VpStr.StartsWith("Set/Rarity:") Then
                        VpCategory = False
                        'Mise � jour du texte si n�cessaire
                        If VpNewTxt.Contains(vbCrLf) Then
                            VmDBCommand.CommandText = "Update Card Set CardText = '" + VpNewTxt.Replace("'", "''") + "' Where Title = '" + VpCard.Replace("'", "''") + "' And Series = '" + VpSeriesCD + "';"
                            VmDBCommand.ExecuteNonQuery
                            Call Me.AddToLog("Nouveau texte VO pour la carte " + VpCard + " - " + VpSeriesCD, eLogType.Information)
                        End If
                        VpNewTxt = ""
                        VpCard = ""
                    ElseIf VpCategory Then
                        VpNewTxt = VpNewTxt + vbCrLf + VpStr
                    End If
                    If Me.btCancel.Tag Then Exit While
                End While
                VpCurSpoiler.Close
            Next VpFile
            If Me.btCancel.Tag Then
                Call Me.AddToLog("La mise � jour des textes VO multilignes a �t� annul�e.", eLogType.Warning, , True)
            Else
                Call Me.AddToLog("La mise � jour des textes VO multilignes est termin�e.", eLogType.Information, , True)
            End If
        End If
    End Sub
    Private Sub FixPictures
    '------------------------------------------------------
    'Correction des images erron�es dans la base de donn�es
    '------------------------------------------------------
    Dim VpOut As FileStream
    Dim VpOutB As BinaryWriter
    Dim VpIn As StreamReader
    Dim VpInB As BinaryReader
    Dim VpFileInfo As FileInfo
    Dim VpName As String
    Dim VpOffset As Long
    Dim VpEnd As Long
        Me.dlgOpen3.FileName = ""
        Me.dlgOpen3.ShowDialog
        If Me.dlgOpen3.FileName <> "" Then
            Me.dlgBrowse.SelectedPath = ""
            Me.dlgBrowse.ShowDialog
            If Me.dlgBrowse.SelectedPath <> "" Then
                VpOut = New FileStream(Me.dlgOpen3.FileName, FileMode.OpenOrCreate)
                VpOutB = New BinaryWriter(VpOut)
                Call Me.AddToLog("La correction des images a commenc�...", eLogType.Information, True)
                Me.prgAvance.Maximum = System.IO.Directory.GetFiles(Me.dlgBrowse.SelectedPath, "*.jpg").Length
                Me.prgAvance.Value = 0
                Me.prgAvance.Style = ProgressBarStyle.Blocks
                For Each VpImg As String In System.IO.Directory.GetFiles(Me.dlgBrowse.SelectedPath, "*.jpg")
                    Application.DoEvents
                    VpFileInfo = New FileInfo(VpImg)
                    VpName = VpFileInfo.Name.Replace(".jpg", "")
                    VmDBCommand.CommandText = "Select Offset From CardPictures Where Title = '" + VpName.Replace("'", "''") + "';"
                    VpOffset = VmDBCommand.ExecuteScalar
                    VmDBCommand.CommandText = "Select [End] From CardPictures Where Title = '" + VpName.Replace("'", "''") + "';"
                    VpEnd = VmDBCommand.ExecuteScalar
                    If VpEnd - VpOffset + 1 >= VpFileInfo.Length Then
                        VpIn = New StreamReader(VpImg)
                        VpInB = New BinaryReader(VpIn.BaseStream)
                        VpOutB.Seek(VpOffset, SeekOrigin.Begin)
                        VpOutB.Write(VpInB.ReadBytes(VpFileInfo.Length))
                        VpIn.Close
                        VmDBCommand.CommandText = "Update CardPictures Set [End] = " + (VpOffset + VpFileInfo.Length - 1).ToString + " Where Title = '" + VpName.Replace("'", "''") + "';"
                        VmDBCommand.ExecuteNonQuery
                        Call Me.AddToLog("Nouvelle image pour la carte " + VpName, eLogType.Information)
                    Else
                        Call Me.AddToLog("Place insuffisante pour la nouvelle image de la carte " + VpName, eLogType.Warning)
                    End If
                    Me.prgAvance.Increment(1)
                    If Me.btCancel.Tag Then Exit For
                Next VpImg
                VpOutB.Flush
                VpOutB.Close
                If Me.btCancel.Tag Then
                    Call Me.AddToLog("La correction des images a �t� annul�e.", eLogType.Warning, , True)
                Else
                    Call Me.AddToLog("La correction des images est termin�e.", eLogType.Information, , True)
                End If
            End If
        End If
    End Sub
    Private Sub RemoveDoublePictures
    '---------------------------------------------------------------------------
    'Supprime du dossier des nouvelles images celles d�j� pr�sentes dans la base
    '---------------------------------------------------------------------------
    Dim VpMissingPicts As StreamReader
    Dim VpNewPicts As DirectoryInfo
    Dim VpTagger As Hashtable
    Dim VpMissing As String
        Me.dlgBrowse.SelectedPath = ""
        Me.dlgBrowse.ShowDialog
        If Me.dlgBrowse.SelectedPath <> "" Then
            VpNewPicts = New DirectoryInfo(Me.dlgBrowse.SelectedPath)
            Me.dlgOpen4.FileName = ""
            Me.dlgOpen4.ShowDialog
            If Me.dlgOpen4.FileName <> "" Then
                Call Me.AddToLog("La suppression des images doublons a commenc�...", eLogType.Information, True)
                VpTagger = New Hashtable
                For Each VpNew As FileInfo In VpNewPicts.GetFiles("*.jpg")
                    VpTagger.Add(VpNew.Name.Replace(".jpg", ""), False)
                Next VpNew
                VpMissingPicts = New StreamReader(Me.dlgOpen4.FileName)
                While Not VpMissingPicts.EndOfStream
                    Application.DoEvents
                    VpMissing = VpMissingPicts.ReadLine
                    If VpTagger.Contains(VpMissing) Then
                        VpTagger.Item(VpMissing) = True
                    End If
                End While
                For Each VpChecker As String In VpTagger.Keys
                    If Not VpTagger.Item(VpChecker) Then
                        If Me.btCancel.Tag Then Exit For
                        File.Delete(Me.dlgBrowse.SelectedPath + "\" + VpChecker + ".jpg")
                        Call Me.AddToLog("Doublon supprim� : " + VpChecker + ".jpg", eLogType.Warning)
                        Application.DoEvents
                    End If
                Next VpChecker
            End If
            If Me.btCancel.Tag Then
                Call Me.AddToLog("La suppression des images doublons a �t� annul�e.", eLogType.Warning, , True)
            Else
                Call Me.AddToLog("La suppression des images doublons est termin�e.", eLogType.Information, , True)
            End If
        End If
    End Sub
    Private Sub ReplaceTitle
    '--------------------------------------------------------------------------------------------------------------------
    'Correction d'un titre de carte erron� dans les tables Autorisations, Card, CardPictures, Creature, Spell et TextesFR
    '--------------------------------------------------------------------------------------------------------------------
    Dim VpOldTitle As String
    Dim VpNewTitle As String
    Dim VpO As Object
        Call Me.AddToLog("La mise � jour d'un nom de carte a commenc�...", eLogType.Information, True)
        VpOldTitle = InputBox("Quel est le nom de la carte � remplacer ?", "Mise � jour de nom", "(carte)")
        If VpOldTitle <> "" Then
            VmDBCommand.CommandText = "Select Title From Card Where InStr(UCase(Title), '" + VpOldTitle.Replace("'", "''").ToUpper + "') > 0;"
            VpO = VmDBCommand.ExecuteScalar
            If Not VpO Is Nothing
                VpOldTitle = VpO.ToString
                If MessageBox.Show("Carte correspondante trouv�e : " + VpOldTitle + vbCrLf + "Voulez-vous changer son nom ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = System.Windows.Forms.DialogResult.Yes Then
                    VpNewTitle = InputBox("Quel est le nouveau nom pour cette carte ?", "Mise � jour de nom", VpOldTitle)
                    If VpNewTitle <> "" Then
                        VpOldTitle = VpOldTitle.Replace("'", "''")
                        VpNewTitle = VpNewTitle.Replace("'", "''")
                        'Autorisations
                        Try
                            VmDBCommand.CommandText = "Update Autorisations Set Title = '" + VpNewTitle + "' Where Title = '" + VpOldTitle + "';"
                            VmDBCommand.ExecuteNonQuery
                            Call Me.AddToLog(VpOldTitle + " remplac�e par " + VpNewTitle + " dans la table Autorisations", eLogType.Information)
                        Catch
                            VmDBCommand.CommandText = "Delete * From Autorisations Where Title = '" + VpOldTitle + "';"
                            VmDBCommand.ExecuteNonQuery
                            Call Me.AddToLog(VpOldTitle + " supprim�e dans la table Autorisations afin d'�viter un doublon", eLogType.Information)
                        End Try
                        'Card
                        VmDBCommand.CommandText = "Update Card Set Title = '" + VpNewTitle + "' Where Title = '" + VpOldTitle + "';"
                        VmDBCommand.ExecuteNonQuery
                        Call Me.AddToLog(VpOldTitle + " remplac�e par " + VpNewTitle + " dans la table Card", eLogType.Information)
                        'CardFR (pour le cas o� la carte n'a pas de traduction VF)
                        VmDBCommand.CommandText = "Update CardFR Set TitleFR = '" + VpNewTitle + "' Where TitleFR = '" + VpOldTitle + "';"
                        VmDBCommand.ExecuteNonQuery
                        Call Me.AddToLog(VpOldTitle + " remplac�e par " + VpNewTitle + " dans la table CardFR", eLogType.Information)
                        'CardPictures
                        Try
                            VmDBCommand.CommandText = "Update CardPictures Set Title = '" + VpNewTitle + "' Where Title = '" + VpOldTitle + "';"
                            VmDBCommand.ExecuteNonQuery
                            Call Me.AddToLog(VpOldTitle + " remplac�e par " + VpNewTitle + " dans la table CardPictures", eLogType.Information)
                        Catch
                            VmDBCommand.CommandText = "Delete * From CardPictures Where Title = '" + VpOldTitle + "';"
                            VmDBCommand.ExecuteNonQuery
                            Call Me.AddToLog(VpOldTitle + " supprim�e dans la table CardPictures afin d'�viter un doublon", eLogType.Information)
                        End Try
                        'Creature
                        Try
                            VmDBCommand.CommandText = "Update Creature Set Title = '" + VpNewTitle + "' Where Title = '" + VpOldTitle + "';"
                            VmDBCommand.ExecuteNonQuery
                            Call Me.AddToLog(VpOldTitle + " remplac�e par " + VpNewTitle + " dans la table Creature", eLogType.Information)
                        Catch
                            Call Me.AddToLog("Impossible de mettre � jour dans la table Creature : il faut supprimer temporairement l'int�grit� r�f�rentielle", eLogType.Warning)
                        End Try
                        'Spell
                        Try
                            VmDBCommand.CommandText = "Update Spell Set Title = '" + VpNewTitle + "' Where Title = '" + VpOldTitle + "';"
                            VmDBCommand.ExecuteNonQuery
                            Call Me.AddToLog(VpOldTitle + " remplac�e par " + VpNewTitle + " dans la table Spell", eLogType.Information)
                        Catch
                            Call Me.AddToLog("Impossible de mettre � jour dans la table Spell : il faut supprimer temporairement l'int�grit� r�f�rentielle", eLogType.Warning)
                        End Try
                        'TextesFR
                        Try
                            VmDBCommand.CommandText = "Update TextesFR Set CardName = '" + VpNewTitle + "' Where CardName = '" + VpOldTitle + "';"
                            VmDBCommand.ExecuteNonQuery
                            Call Me.AddToLog(VpOldTitle + " remplac�e par " + VpNewTitle + " dans la table TextesFR", eLogType.Information)
                        Catch
                            VmDBCommand.CommandText = "Delete * From TextesFR Where CardName = '" + VpOldTitle + "';"
                            VmDBCommand.ExecuteNonQuery
                            Call Me.AddToLog(VpOldTitle + " supprim�e dans la table TextesFR afin d'�viter un doublon", eLogType.Information)
                        End Try
                        Call Me.AddToLog("La mise � jour d'un nom de carte est termin�e.", eLogType.Information, , True)
                    Else
                        Call Me.AddToLog("La mise � jour d'un nom de carte a �t� annul�e.", eLogType.Warning, , True)
                    End If
                Else
                    Call Me.AddToLog("La mise � jour d'un nom de carte a �t� annul�e.", eLogType.Warning, , True)
                End If
            Else
                Call Me.AddToLog("La mise � jour d'un nom de carte a �t� annul�e (aucune correspondance trouv�e).", eLogType.Warning, , True)
            End If
        Else
            Call Me.AddToLog("La mise � jour d'un nom de carte a �t� annul�e.", eLogType.Warning, , True)
        End If
    End Sub
    Private Sub ExtractModIm
    '---------------------------------------------------------------------------------------
    'Extrait et concat�ne dans un fichier .dat les images pr�alablement corrig�es de la base
    '---------------------------------------------------------------------------------------
    Dim VpIn As FileStream
    Dim VpInB As BinaryReader
    Dim VpOut As StreamWriter
    Dim VpOutB As BinaryWriter
    Dim VpPrev As Long = -1
    Dim VpPrev2 As Long = -1
    Dim VpTitle As String = ""
    Dim VpCurIn As StreamReader
    Dim VpCurInB As BinaryReader
    Dim VpOutFull As StreamWriter
    Dim VpOutFullB As BinaryWriter
    Dim VpOutLog As StreamWriter
    Dim VpDirIn As DirectoryInfo
    Dim VpCumul As Long = 0
        Me.dlgOpen3.FileName = ""
        Me.dlgOpen3.ShowDialog
        If Me.dlgOpen3.FileName <> "" Then
            Me.dlgBrowse.SelectedPath = ""
            Me.dlgBrowse.ShowDialog
            If Me.dlgBrowse.SelectedPath <> "" Then
                VpIn = New FileStream(Me.dlgOpen3.FileName, FileMode.Open)
                VpInB = New BinaryReader(VpIn)
                If Directory.Exists(Path.GetTempPath + "\mtgmwbr") Then
                    Directory.Delete(Path.GetTempPath + "\mtgmwbr", True)
                End If
                Directory.CreateDirectory(Path.GetTempPath + "\mtgmwbr")
                Call Me.AddToLog("L'extraction des images corrig�es a commenc�...", eLogType.Information, True)
                Me.prgAvance.Style = ProgressBarStyle.Marquee
                VmDBCommand.CommandText = "Select Title, Offset, [End] From CardPictures Order By Offset Asc;"
                VmDBReader = VmDBCommand.ExecuteReader
                'Extraction
                With VmDBReader
                    While .Read
                        Application.DoEvents
                        If VpPrev <> -1 Then
                            If CDbl(.GetValue(1)) - VpPrev <> 1 Then
                                VpInB.BaseStream.Seek(VpPrev2, SeekOrigin.Begin)
                                VpOut = New StreamWriter(Path.GetTempPath + "\mtgmwbr\" + VpTitle + ".jpg")
                                VpOutB = New BinaryWriter(VpOut.BaseStream)
                                VpOutB.Write(VpInB.ReadBytes(VpPrev - VpPrev2 + 1))
                                VpOutB.Flush
                                VpOutB.Close
                            End If
                        End If
                        VpPrev = CDbl(.GetValue(2))
                        VpPrev2 = CDbl(.GetValue(1))
                        VpTitle = .GetString(0)
                        If Me.btCancel.Tag Then Exit While
                    End While
                    .Close
                End With
                'Concat�nation
                If Not Me.btCancel.Tag Then
                    VpOutFull = New StreamWriter(Me.dlgBrowse.SelectedPath + "\MD_Pict.dat")
                    VpOutFullB = New BinaryWriter(VpOutFull.BaseStream)
                    VpOutLog = New StreamWriter(Me.dlgBrowse.SelectedPath + "\MD_Pict.log")
                    VpDirIn = New DirectoryInfo(Path.GetTempPath + "\mtgmwbr")
                    Me.prgAvance.Style = ProgressBarStyle.Blocks
                    Me.prgAvance.Maximum = VpDirIn.GetFiles().Length
                    Me.prgAvance.Value = 0
                    For Each VpFile As FileInfo In VpDirIn.GetFiles
                        Application.DoEvents
                        VpOutLog.WriteLine(VpFile.Name + "#" + VpCumul.ToString + "#" + (VpCumul + VpFile.Length - 1).ToString)
                        VpCumul = VpCumul + VpFile.Length
                        VpCurIn = New StreamReader(VpFile.FullName)
                        VpCurInB = New BinaryReader(VpCurIn.BaseStream)
                        VpOutFullB.Write(VpCurInB.ReadBytes(VpFile.Length))
                        VpCurIn.Close
                        Me.prgAvance.Increment(1)
                        If Me.btCancel.Tag Then Exit For
                    Next VpFile
                    VpOutLog.Flush
                    VpOutLog.Close
                    VpOutFull.Flush
                    VpOutFull.Close
                    Directory.Delete(Path.GetTempPath + "\mtgmwbr", True)
                End If
                If Me.btCancel.Tag Then
                    Call Me.AddToLog("L'extraction des images corrig�es a �t� annul�e.", eLogType.Warning, , True)
                Else
                    Call Me.AddToLog("L'extraction des images corrig�es est termin�e.", eLogType.Information, , True)
                End If
            End If
        End If
    End Sub
    Sub FilterRulings
    '----------------------------------------------------------------------------------------------------------------
    'Remplace les r�f�rences � des URLs de symboles dans le fichier des r�gles sp�cifiques par les symboles effectifs
    '----------------------------------------------------------------------------------------------------------------
    Dim VpIn As StreamReader
    Dim VpOut As StreamWriter
    Dim VpStr As String
        Me.dlgOpen6.FileName = ""
        Me.dlgOpen6.ShowDialog
        If Me.dlgOpen6.FileName <> "" Then
            Me.dlgSave3.FileName = ""
            Me.dlgSave3.ShowDialog
            If Me.dlgSave3.FileName <> "" Then
                Call Me.AddToLog("Filtrage des rulings...", eLogType.Information)
                Application.DoEvents
                VpIn = New StreamReader(Me.dlgOpen6.FileName)
                VpOut = New StreamWriter(Me.dlgSave3.FileName)
                VpStr = VpIn.ReadToEnd
                VpStr = VpStr.Replace("<img src=""/Handlers/Image.ashx?size=small&amp;name=chaos&amp;type=symbol"" alt=""Chaos"" align=""absbottom"" />", "{O}")
                VpStr = VpStr.Replace("<img src=""/Handlers/Image.ashx?size=small&amp;name=pw&amp;type=symbol"" alt=""Planeswalk"" align=""absbottom"" />", "{P}")
                VpStr = VpStr.Replace("<img src=""/Handlers/Image.ashx?size=small&amp;name=e&amp;type=symbol"" alt=""Energy"" align=""absbottom"" />", "{E}")
                VpStr = VpStr.Replace("<img src=""/Handlers/Image.ashx?size=small&amp;name=R&amp;type=symbol"" alt=""Red"" align=""absbottom"" />", "{R}")
                VpStr = VpStr.Replace("<img src=""/Handlers/Image.ashx?size=small&amp;name=U&amp;type=symbol"" alt=""Blue"" align=""absbottom"" />", "{U}")
                VpStr = VpStr.Replace("<img src=""/Handlers/Image.ashx?size=small&amp;name=G&amp;type=symbol"" alt=""Green"" align=""absbottom"" />", "{G}")
                VpStr = VpStr.Replace("<img src=""/Handlers/Image.ashx?size=small&amp;name=W&amp;type=symbol"" alt=""White"" align=""absbottom"" />", "{W}")
                VpStr = VpStr.Replace("<img src=""/Handlers/Image.ashx?size=small&amp;name=B&amp;type=symbol"" alt=""Black"" align=""absbottom"" />", "{B}")
                VpStr = VpStr.Replace("<img src=""/Handlers/Image.ashx?size=small&amp;name=WB&amp;type=symbol"" alt=""White or Black"" align=""absbottom"" />", "{W/B}")
                VpStr = VpStr.Replace("<img src=""/Handlers/Image.ashx?size=small&amp;name=BG&amp;type=symbol"" alt=""Black or Green"" align=""absbottom"" />", "{B/G}")
                VpStr = VpStr.Replace("<img src=""/Handlers/Image.ashx?size=small&amp;name=C&amp;type=symbol"" alt=""Colorless"" align=""absbottom"" />", "{C}")
                VpStr = VpStr.Replace("<img src=""/Handlers/Image.ashx?size=small&amp;name=X&amp;type=symbol"" alt=""Variable Colorless"" align=""absbottom"" />", "{X}")
                VpStr = VpStr.Replace("<img src=""/Handlers/Image.ashx?size=small&amp;name=6&amp;type=symbol"" alt=""6"" align=""absbottom"" />", "{6}")
                VpStr = VpStr.Replace("<img src=""/Handlers/Image.ashx?size=small&amp;name=5&amp;type=symbol"" alt=""5"" align=""absbottom"" />", "{5}")
                VpStr = VpStr.Replace("<img src=""/Handlers/Image.ashx?size=small&amp;name=4&amp;type=symbol"" alt=""4"" align=""absbottom"" />", "{4}")
                VpStr = VpStr.Replace("<img src=""/Handlers/Image.ashx?size=small&amp;name=3&amp;type=symbol"" alt=""3"" align=""absbottom"" />", "{3}")
                VpStr = VpStr.Replace("<img src=""/Handlers/Image.ashx?size=small&amp;name=2&amp;type=symbol"" alt=""2"" align=""absbottom"" />", "{2}")
                VpStr = VpStr.Replace("<img src=""/Handlers/Image.ashx?size=small&amp;name=1&amp;type=symbol"" alt=""1"" align=""absbottom"" />", "{1}")
                VpStr = VpStr.Replace("<img src=""/Handlers/Image.ashx?size=small&amp;name=0&amp;type=symbol"" alt=""0"" align=""absbottom"" />", "{0}")
                VpStr = VpStr.Replace("<img src=""/Handlers/Image.ashx?size=small&amp;name=tap&amp;type=symbol"" alt=""Tap"" align=""absbottom"" />", "{T}")
                VpStr = VpStr.Replace("<tr class=""post oddItem"" style=""background-color: rgba(255,255,366, 0.08);"">", "")
                VpOut.Write(VpStr)
                VpOut.Flush
                VpOut.Close
                VpIn.Close
                Call Me.AddToLog("Le filtrage des rulings est termin�.", eLogType.Information)
            End If
        End If
    End Sub
    Sub ExtractCardsMultiverseId
    '-----------------------------------
    'Listing des identifiants Multiverse
    '-----------------------------------
    Dim VpOut As StreamWriter
        Me.dlgSave.FileName = ""
        Me.dlgSave.ShowDialog
        If Me.dlgSave.FileName <> "" Then
            VpOut = New StreamWriter(Me.dlgSave.FileName)
            Call Me.AddToLog("L'extraction des identifiants Multiverse des cartes a commenc�...", eLogType.Information, True)
            Me.prgAvance.Style = ProgressBarStyle.Marquee
            VmDBCommand.CommandText = "Select Title, Series, MultiverseId, CardNbr From Card Order By Title;"
            VmDBReader = VmDBCommand.ExecuteReader
            With VmDBReader
                While .Read
                    Application.DoEvents
                    VpOut.WriteLine(.GetString(0) + "#" + .GetString(1) + "#" + CLng(.GetValue(2)).ToString + "#" + CLng(.GetValue(3)).ToString)
                    Me.txtCur.Text = .GetString(0)
                    If Me.btCancel.Tag Then Exit While
                End While
                .Close
            End With
            VpOut.Flush
            VpOut.Close
            If Me.btCancel.Tag Then
                Call Me.AddToLog("L'extraction des identifiants Multiverse des cartes a �t� annul�e.", eLogType.Warning, , True)
            Else
                Call Me.AddToLog("L'extraction des identifiants Multiverse des cartes est termin�e.", eLogType.Information, , True)
            End If
        End If
    End Sub
    Private Sub ExtractCards(VpReq As String)
    '--------------------------------------------------
    'Listing des titres distincts des cartes de la base
    '--------------------------------------------------
    Dim VpOut As StreamWriter
        Me.dlgSave.FileName = ""
        Me.dlgSave.ShowDialog
        If Me.dlgSave.FileName <> "" Then
            VpOut = New StreamWriter(Me.dlgSave.FileName)
            Call Me.AddToLog("L'extraction des titres des cartes a commenc�...", eLogType.Information, True)
            Me.prgAvance.Style = ProgressBarStyle.Marquee
            VmDBCommand.CommandText = VpReq
            VmDBReader = VmDBCommand.ExecuteReader
            With VmDBReader
                While .Read
                    Application.DoEvents
                    VpOut.WriteLine(.GetString(0))
                    Me.txtCur.Text = .GetString(0)
                    If Me.btCancel.Tag Then Exit While
                End While
                .Close
            End With
            VpOut.Flush
            VpOut.Close
            If Me.btCancel.Tag Then
                Call Me.AddToLog("L'extraction des titres des cartes a �t� annul�e.", eLogType.Warning, , True)
            Else
                Call Me.AddToLog("L'extraction des titres des cartes est termin�e.", eLogType.Information, , True)
            End If
        End If
    End Sub
    Private Sub ExtractCardsPricesAborted
    '-----------------------------------------------------------------------------------
    'Listing des cartes dont la mise � jour des prix ne semble pas avoir �t� t�l�charg�e
    '-----------------------------------------------------------------------------------
    Dim VpOut As StreamWriter
    Dim VpCards As List(Of String)
    Dim VpCardsDB As List(Of String)
    Dim VpFound As Boolean
        Me.dlgOpen4.FileName = ""
        Me.dlgOpen4.ShowDialog
        If Me.dlgOpen4.FileName <> "" Then
            VpCards = Me.BuildListeFromFile(True)
        Else
            Exit Sub
        End If
        Me.dlgSave.FileName = ""
        Me.dlgSave.ShowDialog
        If Me.dlgSave.FileName <> "" Then
            VpOut = New StreamWriter(Me.dlgSave.FileName)
            Call Me.AddToLog("L'extraction des titres des cartes a commenc�...", eLogType.Information, True)
            Me.prgAvance.Style = ProgressBarStyle.Marquee
            VpCardsDB = Me.BuildListeFromDB
            For Each VpCardDB As String In VpCardsDB
                Application.DoEvents
                VpFound = False
                For Each VpCard As String In VpCards
                    If VpCard = VpCardDB Then
                        VpFound = True
                        Exit For
                    End If
                Next VpCard
                If Not VpFound Then
                    VpOut.WriteLine(VpCardDB)
                    Me.txtCur.Text = VpCardDB
                End If
                If Me.btCancel.Tag Then Exit For
            Next VpCardDB
            VpOut.Flush
            VpOut.Close
            If Me.btCancel.Tag Then
                Call Me.AddToLog("L'extraction des titres des cartes a �t� annul�e.", eLogType.Warning, , True)
            Else
                Call Me.AddToLog("L'extraction des titres des cartes est termin�e.", eLogType.Information, , True)
            End If
        End If
    End Sub
    Private Function GetTextVF(VpIn As String) As String
    '------------------------------------------------------------
    'Retourne le texte fran�ais pour la carte pass�e en param�tre
    '------------------------------------------------------------
    Dim VpClient As New WebClient
    Dim VpStr As String = ""
    Dim VpE As String
    Dim VpStrs() As String
    Dim VpIr As Integer
        Try
            VpStr = Me.HTMLfromRequest(CmURL1.Replace(CmId1, VpIn.Replace(" ", "+")))
            VpStrs = VpStr.Split(New String() {CmKey1}, StringSplitOptions.None)
            VpIr = Me.FindRightIndex(VpStrs, VpIn)
            VpStr = CmURL2 + VpStrs(VpIr).Substring(0, VpStrs(VpIr).IndexOf(""""))
            VpStr = Me.HTMLfromRequest(VpStr)
            If VpStr.Contains(CmKey5) Then
                VpStr = VpStr.Substring(VpStr.IndexOf(CmKey5))
                VpStr = VpStr.Substring(VpStr.IndexOf("<br /><br />") + 12)
                If VpStr.IndexOf("<div align=""right""><b>") <> -1 Then
                    VpStr = VpStr.Substring(0, Math.Min(VpStr.IndexOf("</"), VpStr.IndexOf("<div align=""right""><b>")))
                Else
                    VpStr = VpStr.Substring(0, VpStr.IndexOf("</"))
                End If
                VpStr = VpStr.Replace("&#039;", "'")
                VpStr = VpStr.Replace("&#8212;", "-")
                VpStr = VpStr.Replace("&#8217;", "'")
                VpStr = VpStr.Replace("&quot;", """")
                VpStr = VpStr.Replace("&lt;&gt;","!c!")
                VpStr = VpStr.Replace("<img src=""/images/magic/manas/micro/", "!")
                VpStr = VpStr.Replace("<img src=""images/smileys/", "!")
                VpStr = VpStr.Replace("<img src=""/images/smileys/", "!")
                VpStr = VpStr.Replace(".gif""  border=""0"" style=""vertical-align: text-bottom;"" alt="""" />", "!")
                VpStr = VpStr.Replace("<br />" + vbCrLf, vbCrLf)
                VpStr = VpStr.Replace(vbCrLf + "<br />", vbCrLf)
                VpStr = VpStr.Replace("<br />", vbCrLf)
                For Each VpEnder As String In ( New String() { " ", ")", "!", ":", ";", ".", ",", "?" } )
                    For VpI As Integer = 8 To 1 Step -1
                        VpE = " " + ( New String("E", VpI) ) + VpEnder
                        VpStr = VpStr.Replace(VpE, VpE.Replace("E", "!e!"))
                    Next VpI
                Next VpEnder
                Return VpStr
            End If
        Catch
        End Try
        Return ""
    End Function
    Private Sub TranslateTexts
    '---------------------------------------------------------------
    'Cherche le texte VF des cartes list�es dans le fichier sp�cifi�
    '---------------------------------------------------------------
    Dim VpOut As StreamWriter
    Dim VpStr As String
    Dim VpListe As List(Of String)
        Me.dlgOpen4.FileName = ""
        Me.dlgOpen4.ShowDialog
        If Me.dlgOpen4.FileName <> "" Then
            Me.dlgSave.FileName = ""
            Me.dlgSave.ShowDialog
            If Me.dlgSave.FileName <> "" Then
                VpOut = New StreamWriter(Me.dlgSave.FileName)
                Call Me.AddToLog("La r�cup�ration des textes des cartes en fran�ais a commenc�...", eLogType.Information, True)
                'Construction de la liste
                VpListe = Me.BuildListeFromFile
                'Traduction
                Me.prgAvance.Maximum = VpListe.Count
                Me.prgAvance.Value = 0
                Me.prgAvance.Style = ProgressBarStyle.Blocks
                For Each VpCard As String In VpListe
                    Application.DoEvents
                    If VpCard.Trim <> "" Then
                        Me.txtCur.Text = VpCard
                        VpStr = Me.GetTextVF(VpCard)
                        If VpStr.Trim <> "" Then
                            VpOut.Write("##" + VpCard + "^^" + VpStr)
                        Else
                            Call Me.AddToLog("Impossible de r�cup�rer le texte en fran�ais pour la carte : " + VpCard, eLogType.Warning)
                        End If
                    End If
                    Me.prgAvance.Increment(1)
                    Call Me.ETA
                    If Me.btCancel.Tag Then Exit For
                Next VpCard
                VpOut.Flush
                VpOut.Close
                If Me.btCancel.Tag Then
                    Call Me.AddToLog("La r�cup�ration des textes des cartes en fran�ais a �t� annul�e.", eLogType.Warning, , True)
                Else
                    Call Me.AddToLog("La r�cup�ration des textes des cartes en fran�ais est termin�e.", eLogType.Information, , True)
                End If
            End If
        End If
    End Sub
    Private Sub BrowseAndWait(Optional VpURL As String = "", Optional VpExplicitWaitFor As String = "")
    '---------------------------------------------------------------------------
    'Navigue sur la page pass�e en param�tre en respectant le d�lai d'expiration
    '---------------------------------------------------------------------------
    Dim VpStart As Date = Now
        VmIsComplete = False
        If VpURL <> "" Then
            Me.wbMV.Navigate(VpURL)
        End If
        While Not VmIsComplete
            If Now.Subtract(VpStart).TotalSeconds > 10 Then
                If VpExplicitWaitFor <> "" Then
                    Try
                        If Not Me.wbMV.Document.All Is Nothing Then
                            For Each VpElement As HtmlElement In Me.wbMV.Document.All
                                If VpElement.Name = VpExplicitWaitFor Then
                                    Me.wbMV.Stop
                                    VmIsComplete = True
                                End If
                            Next VpElement
                        End If
                    Catch
                    End Try
                Else
                    Me.wbMV.Stop
                    VmIsComplete = True
                End If
            End If
            Application.DoEvents
        End While
    End Sub
    Public Function GetAutorisations(VpCard As String) As String
    '------------------------------------------------------------
    'Retourne les autorisations pour la carte pass�e en param�tre
    '------------------------------------------------------------
    Dim VpElement As HtmlElement
    Dim VpLastId As Integer = 0
        Try
            'Site de Magic-Ville
            Call Me.BrowseAndWait(CmURL0, CmKey0)
            'Saisie de la carte dans la zone de recherche
            VpElement = Me.wbMV.Document.All.GetElementsByName(CmKey0).Item(0)
            VpElement.SetAttribute("value", VpCard)
            For Each VpElement In Me.wbMV.Document.All
                If VpElement.GetAttribute("src").ToLower.Contains("/go.png") Then
                    'Validation
                    VpElement.InvokeMember("click")
                    Call Me.BrowseAndWait
                    Exit For
                End If
            Next VpElement
            'Page interm�diaire (ne s'affiche qu'en cas d'ambiguit�)
            For Each VpElement In Me.wbMV.Document.All
                If VpElement.GetAttribute("href") <> "" AndAlso Not VpElement.InnerText Is Nothing Then
                    If VpElement.InnerText.ToLower.Trim = VpCard.ToLower Then
                        'Validation
                        VpElement.InvokeMember("click")
                        Call Me.BrowseAndWait
                        Exit For
                    End If
                End If
            Next VpElement
            For Each VpElement In Me.wbMV.Document.All
                If VpElement.InnerText = "Autorisations en Tournois" Then
                    Return Me.TournoiFormat(VpElement.NextSibling.InnerHtml + VpElement.NextSibling.NextSibling.NextSibling.InnerHtml)
                End If
            Next VpElement
        Catch
        End Try
        Thread.Sleep(2000)
        Return ""
    End Function
    Private Function TournoiFormat(VpStr As String) As String
    Dim VpStrs() As String
    Dim VpAut As String = ""
        Try
            If VpStr.Contains(".png") Then
                VpStrs = VpStr.ToLower.Split(New String() {".png"}, StringSplitOptions.None)
                For Each VpA As String In VpStrs
                    If VpA.Contains("/") Then
                        VpAut = VpAut + VpA.Substring(VpA.LastIndexOf("/") + 1) + "#"
                    End If
                Next VpA
                VpAut = VpAut.Replace("#1vs1", "#blocdk#1vs1")
                Return VpAut.Substring(0, VpAut.Length - 1)
            End If
        Catch
        End Try
        Return ""
    End Function
    Private Sub UpdateAutorisations(VpAll As Boolean)
    '----------------------------------------------------
    'Mise � jour des autorisations des cartes en tournois
    '----------------------------------------------------
    Dim VpOut As StreamWriter
    Dim VpCards As List(Of String)
    Dim VpAut As String
    Dim VpAppend As Boolean
    Dim VpAlready() As String
    Dim VpLast As String = ""
    Dim VpCount As Integer
        Me.dlgSave.FileName = ""
        Me.dlgSave.ShowDialog
        If Me.dlgSave.FileName <> "" Then
            VpAppend = File.Exists(Me.dlgSave.FileName)
            If VpAppend Then
                'Si le fichier existe d�j�, regarde la derni�re carte qui a �t� trait�e
                VpAlready = File.ReadAllLines(Me.dlgSave.FileName)
                If VpAlready.Length > 2 Then
                    If VpAlready(VpAlready.Length - 1).Contains("#") Then
                        VpLast = VpAlready(VpAlready.Length - 1)
                    Else
                        VpLast = VpAlready(VpAlready.Length - 2)
                    End If
                    VpLast = VpLast.Substring(0, VpLast.IndexOf("#"))
                Else
                    VpAppend = False
                End If
            End If
            VpOut = New StreamWriter(Me.dlgSave.FileName, VpAppend)
            If VpAppend Then
                Call Me.AddToLog("La r�cup�ration des autorisations de tournois se poursuit...", eLogType.Information, True)
            Else
                Call Me.AddToLog("La r�cup�ration des autorisations de tournois a commenc�...", eLogType.Information, True)
                'Inscription de la date
                VpOut.WriteLine(Now.ToShortDateString)
            End If
            'R�cup�re la liste des cartes
            If VpAll Then
                VpCards = Me.BuildListeFromDB(VpLast)
            Else
                Me.dlgOpen4.FileName = ""
                Me.dlgOpen4.ShowDialog
                If Me.dlgOpen4.FileName <> "" Then
                    VpCards = Me.BuildListeFromFile
                Else
                    VpOut.Close
                    Exit Sub
                End If
            End If
            Me.prgAvance.Maximum = VpCards.Count
            Me.prgAvance.Value = 0
            Me.prgAvance.Style = ProgressBarStyle.Blocks
            For Each VpCard As String In VpCards
                Me.txtCur.Text = VpCard
                Application.DoEvents
                VpAut = ""
                VpCount = 0
                While VpAut = "" And VpCount < 10
                    VpAut = Me.GetAutorisations(VpCard)
                    VpCount += 1
                End While
                If VpAut <> "" Then
                    VpOut.WriteLine(VpCard + "#" + VpAut)
                Else
                    Call Me.AddToLog("Impossible de r�cup�rer les autorisations de tournois pour la carte : " + VpCard, eLogType.Warning)
                End If
                Me.prgAvance.Increment(1)
                Call Me.ETA
                If Me.btCancel.Tag Then Exit For
            Next VpCard
            VpOut.Flush
            VpOut.Close
            If Me.btCancel.Tag Then
                Call Me.AddToLog("La r�cup�ration des autorisations de tournois a �t� annul�e.", eLogType.Warning, , True)
            Else
                Call Me.AddToLog("La r�cup�ration des autorisations de tournois est termin�e.", eLogType.Information, , True)
            End If
        End If
    End Sub
    Private Function ReadAutorisations(VpPath As String) As Dictionary(Of String, String)
    Dim VpFile As StreamReader
    Dim VpData As New Dictionary(Of String, String)
    Dim VpStr As String
    Dim VpName As String
    Dim VpAut As String
        VpFile = New StreamReader(VpPath)
        While Not VpFile.EndOfStream
            VpStr = VpFile.ReadLine
            If VpStr.Contains("#") Then
                VpName = VpStr.Substring(0, VpStr.IndexOf("#"))
                VpAut = VpStr.Substring(VpStr.IndexOf("#"))
                If Not VpAut.Contains("#mtgo") Then
                    VpAut += "#mtgodk"
                End If
                VpData.Add(VpName, VpAut)
            End If
        End While
        VpFile.Close
        Return VpData
    End Function
    Private Sub MergeAutorisations
    '------------------------------------
    'Fusion de 2 fichiers d'autorisations
    '------------------------------------
    Dim VpPathCur As String
    Dim VpPathOld As String
    Dim VpDataCur As Dictionary(Of String, String)
    Dim VpDataOld As Dictionary(Of String, String)
    Dim VpKeysCur() As String
    Dim VpKeysOld() As String
    Dim VpMerge As StreamWriter
        Call Me.AddToLog("Attente saisie listing r�cent...", eLogType.Information)
        Application.DoEvents
        Me.dlgOpen4.FileName = ""
        Me.dlgOpen4.ShowDialog
        If Me.dlgOpen4.FileName <> "" Then
            VpPathCur = Me.dlgOpen4.FileName
            Call Me.AddToLog("Attente saisie listing ancien...", eLogType.Information)
            Application.DoEvents
            Me.dlgOpen4.FileName = ""
            Me.dlgOpen4.ShowDialog
            If Me.dlgOpen4.FileName <> "" Then
                VpPathOld = Me.dlgOpen4.FileName
                Me.dlgSave.FileName = ""
                Me.dlgSave.ShowDialog
                If Me.dlgSave.FileName <> "" Then
                    Call Me.AddToLog("La fusion des autorisations de tournois a commenc�...", eLogType.Information, True)
                    Application.DoEvents
                    VpDataCur = Me.ReadAutorisations(VpPathCur)
                    VpDataOld = Me.ReadAutorisations(VpPathOld)
                    ReDim VpKeysCur(0 To VpDataCur.Keys.Count - 1)
                    ReDim VpKeysOld(0 To VpDataOld.Keys.Count - 1)
                    Call VpDataCur.Keys.CopyTo(VpKeysCur, 0)
                    Call VpDataOld.Keys.CopyTo(VpKeysOld, 0)
                    Call Me.AddToLog("*** Reprise des anciennes autorisations 'bloc' :", eLogType.Information)
                    For Each VpCard As String In VpKeysCur
                        If VpDataOld.ContainsKey(VpCard) AndAlso VpDataCur.Item(VpCard).Contains("#blocdk#") AndAlso Not VpDataOld.Item(VpCard).Contains("#blocdk#") Then
                            VpDataCur.Item(VpCard) = VpDataCur.Item(VpCard).Replace("#blocdk#", If(VpDataOld.Item(VpCard).Contains("#blocno"), "#blocno#", "#bloc#"))
                        Else
                            Call Me.AddToLog("Pas de correspondance trouv�e pour : " + VpCard, eLogType.Information)
                            Application.DoEvents
                        End If
                    Next VpCard
                    Call Me.AddToLog("*** Autorisations de cartes manquantes reprises :", eLogType.Information)
                    For Each VpCard As String In VpKeysOld
                        If Not VpDataCur.ContainsKey(VpCard) Then
                            VpDataCur.Add(VpCard, VpDataOld.Item(VpCard))
                            Call Me.AddToLog(VpCard + " = " + VpDataCur.Item(VpCard), eLogType.Information)
                            Application.DoEvents
                        End If
                    Next VpCard
                    ReDim VpKeysCur(0 To VpDataCur.Keys.Count - 1)
                    Call VpDataCur.Keys.CopyTo(VpKeysCur, 0)
                    VpMerge = New StreamWriter(Me.dlgSave.FileName)
                    VpMerge.WriteLine(Now.ToShortDateString)
                    For Each VpCard As String In VpKeysCur
                        VpMerge.WriteLine(VpCard + VpDataCur.Item(VpCard))
                    Next VpCard
                    VpMerge.Flush
                    VpMerge.Close
                    Call Me.AddToLog("La fusion des autorisations de tournois est termin�e.", eLogType.Information, , True)
                End If
            End If
        End If
    End Sub
    Private Function Matching(VpStr As String) As String
        Select Case VpStr
            Case "True"
                Return 1
            Case "False"
                Return 0
            Case Else
                Return VpStr.Replace(vbCrLf, " ")
        End Select
    End Function
    Private Function SerieShortcut(VpStr As String) As String
        Select Case VpStr.Substring(0, 2)
            Case "1E"
                Return "10th#" + VpStr
            Case "3B"
                Return "3rdBB#" + VpStr
            Case "3W"
                Return "3rdWB#" + VpStr
            Case "CT"
                Return "coldsnapthemedecks#" + VpStr
            Case "DM"
                Return "deckmasters#" + VpStr
            Case "PH"
                Return "planechase#" + VpStr
            Case "PI"
                Return "planechase2012#" + VpStr
            Case "PG"
                Return "pegase#" + VpStr
            Case "AR"
                Return "alarareborn#" + VpStr
            Case "BT"
                Return "beatdown#" + VpStr
            Case "CF"
                Return "conflux#" + VpStr
            Case "CS"
                Return "coldsnap#" + VpStr
            Case "DI"
                Return "dissension#" + VpStr
            Case "ET"
                Return "eventide#" + VpStr
            Case "FD"
                Return "fifthdawn#" + VpStr
            Case "FS"
                Return "futuresight#" + VpStr
            Case "GP"
                Return "guildpact#" + VpStr
            Case "LW"
                Return "lorwyn#" + VpStr
            Case "M1"
                Return "magic2010#" + VpStr
            Case "MT"
                Return "morningtide#" + VpStr
            Case "RA"
                Return "ravnica#" + VpStr
            Case "RE"
                Return "renaissance#" + VpStr
            Case "SL"
                Return "shardsofalara#" + VpStr
            Case "SM"
                Return "shadowmoor#" + VpStr
            Case "TS"
                Return "timespiral#" + VpStr
            Case "UH"
                Return "unhinged#" + VpStr
            Case "WW"
                Return "worldwake#" + VpStr
            Case "ZK"
                Return "zendikar#" + VpStr
            Case "RI"
                Return "riseoftheeldrazi#" + VpStr
            Case "M2"
                Return "magic2011#" + VpStr
            Case "TD"
                Return "timeshifted#" + VpStr
            Case "D1"
                Return "DuelDecksDivinevsDemonic#" + VpStr
            Case "D2"
                Return "DuelDecksElspethvsTezzeret#" + VpStr
            Case "D3"
                Return "DuelDecksElvesvsGoblins#" + VpStr
            Case "D4"
                Return "DuelDecksGarrukvsLiliana#" + VpStr
            Case "D5"
                Return "DuelDecksJacevsChandra#" + VpStr
            Case "D6"
                Return "DuelDecksPhyrexiavstheCoalition#" + VpStr
            Case "D7"
                Return "DuelDecksKnightsvsDragons#" + VpStr
            Case "D8"
                Return "DuelDecksAjanivsNicolBolas#" + VpStr
            Case "V1"
                Return "FromtheVaultDragons#" + VpStr
            Case "V2"
                Return "FromtheVaultExiled#" + VpStr
            Case "V3"
                Return "FromtheVaultRelics#" + VpStr
            Case "SD"
                Return "scarsofmirrodin#" + VpStr
            Case "R1"
                Return "fireandlightining#" + VpStr
            Case "R2"
                Return "slivers#" + VpStr
            Case "MB"
                Return "mirrodinbesieged#" + VpStr
            Case "DS"
                Return "darksteel#" + VpStr
            Case "PC"
                Return "planarchaos#" + VpStr
            Case "NP"
                Return "newphyrexia#" + VpStr
            Case "M3"
                Return "magic2012#" + VpStr
            Case "CD"
                Return "commander#" + VpStr
            Case "V4"
                Return "FromtheVaultLegends#" + VpStr
            Case "IN"
                Return "innistrad#" + VpStr
            Case "DA"
                Return "darkascension#" + VpStr
            Case "S2"
                Return "starter2000#" + VpStr
            Case "R3"
                Return "graveborn#" + VpStr
            Case "YR"
                Return "avacynrestored#" + VpStr
            Case "D9"
                Return "DuelDecksVenservsKoth#" + VpStr
            Case "M4"
                Return "magic2013#" + VpStr
            Case "RR"
                Return "returntoravnica#" + VpStr
            Case "DG"
                Return "DuelDecksIzzetvsGolgari#" + VpStr
            Case "V5"
                Return "FromtheVaultRealms#" + VpStr
            Case "GC"
                Return "gatecrash#" + VpStr
            Case "D0"
                Return "DuelDecksSorinvsTibalt#" + VpStr
            Case "DZ"
                Return "dragonsmaze#" + VpStr
            Case "MS"
                Return "modernmasters#" + VpStr
            Case "M5"
                Return "magic2014#" + VpStr
            Case "JG"
                Return "DCIJudgeGift#" + VpStr
            Case "AH"
                Return "archenemy#" + VpStr
            Case "V6"
                Return "FromtheVaultTwenty#" + VpStr
            Case "DD"
                Return "DuelDecksHeroesvsMonsters#" + VpStr
            Case "TH"
                Return "theros#" + VpStr
            Case "C3"
                Return "commander2013#" + VpStr
            Case "BG"
                Return "bornofthegods#" + VpStr
            Case "JN"
                Return "journeyintonyx#" + VpStr
            Case "DB"
                Return "DuelDecksJacevsVraska#" + VpStr
            Case "CY"
                Return "conspiracy#" + VpStr
            Case "M6"
                Return "magic2015#" + VpStr
            Case "DP"
                Return "duelsoftheplansewalkers#" + VpStr
            Case "FM"
                Return "fridaynightmagic#" + VpStr
            Case "IS"
                Return "introductory2pset#" + VpStr
            Case "MP"
                Return "magicplayerrewards#" + VpStr
            Case "ME"
                Return "mastersedition#" + VpStr
            Case "MV"
                Return "moderneventdeck2014#" + VpStr
            Case "PR"
                Return "prereleaseevents#" + VpStr
            Case "RT"
                Return "releaseevents#" + VpStr
            Case "GX"
                Return "grandprix#" + VpStr
            Case "KT"
                Return "khansoftarkir#" + VpStr
            Case "DH"
                Return "DuelDecksSpeedvsCunning#" + VpStr
            Case "C4"
                Return "commander2014#" + VpStr
            Case "V7"
                Return "FromtheVaultAnnihilation#" + VpStr
            Case "FR"
                Return "fatereforged#" + VpStr
            Case "DT"
                Return "dragonsoftarkir#" + VpStr
            Case "DJ"
                Return "DuelDecksKioravsElspeth#" + VpStr
            Case "MU"
                Return "modernmasters2015#" + VpStr
            Case "OR"
                Return "origins#" + VpStr
            Case "DC"
                Return "DuelDecksZendikarvsEldrazi#" + VpStr
            Case "V8"
                Return "FromtheVaultAngels#" + VpStr
            Case "BZ"
                Return "battleforzendikar#" + VpStr
            Case "C5"
                Return "commander2015#" + VpStr
            Case "OG"
                Return "oathofthegatewatch#" + VpStr
            Case "SI"
                Return "shadowsoverinnistrad#" + VpStr
            Case "DQ"
                Return "DuelDecksBlessedvsCursed#" + VpStr
            Case "MG"
                Return "magicgamedaycards#" + VpStr
            Case "ZX"
                Return "zendikarexpeditions#" + VpStr
            Case "TC"
                Return "takethecrown#" + VpStr
            Case "MA"
                Return "eternalmasters#" + VpStr
            Case "EM"
                Return "eldritchmoon#" + VpStr
            Case "V9"
                Return "FromtheVaultLore#" + VpStr
            Case "KD"
                Return "kaladesh#" + VpStr
            Case "KI"
                Return "kaladeshinventions#" + VpStr
            Case "DR"
                Return "DuelDecksNissavsObNixilis#" + VpStr
            Case "C6"
                Return "commander2016#" + VpStr
            Case "WD"
                Return "welcomedeck2016#" + VpStr
            Case "ER"
                Return "aetherrevolt#" + VpStr
            Case "C1"
                Return "commandersarsenal#" + VpStr
            Case "AK"
                Return "amonkhet#" + VpStr
            Case "MW"
                Return "modernmasters2017#" + VpStr
            Case "HD"
                Return "hourofdevastation#" + VpStr
            Case "AI"
                Return "archenemyNicolBolas#" + VpStr
            Case "CP"
                Return "clashpack#" + VpStr
            Case "CA"
                Return "commanderanthology#" + VpStr
            Case "A1"
                Return "DuelDecksAnthologyDivinevsDemonic#" + VpStr
            Case "A3"
                Return "DuelDecksAnthologyElvesvsGoblins#" + VpStr
            Case "A4"
                Return "DuelDecksAnthologyGarrukvsLiliana#" + VpStr
            Case "A5"
                Return "DuelDecksAnthologyJacevsChandra#" + VpStr
            Case "PA"
                Return "planechaseanthology#" + VpStr
            Case "C7"
                Return "commander2017#" + VpStr
            Case "DL"
                Return "DuelDecksMindvsMight#" + VpStr
            Case "WE"
                Return "welcomedeck2017#" + VpStr
            Case "AJ"
                Return "amonkhetinvocations#" + VpStr
            Case "GW"
                Return "wpngateway#" + VpStr
            Case "XL"
                Return "ixalan#" + VpStr
            Case "DU"
                Return "DuelDecksMerfolkvsGoblins#" + VpStr
            Case "VA"
                Return "FromtheVaultTransform#" + VpStr
            Case "IM"
                Return "iconicmasters#" + VpStr
            Case "UB"
                Return "unstable#" + VpStr
            Case "RX"
                Return "rivalsofixalan#" + VpStr
            Case "A2"
                Return "masters25#" + VpStr
            Case "DO"
                Return "dominaria#" + VpStr
            Case "DV"
                Return "DuelDecksElvesvsInventors#" + VpStr
            Case "BB"
                Return "battlebond#" + VpStr
            Case "M9"
                Return "magic2019#" + VpStr
            Case "GS"
                Return "GlobalSeriesJiangYangguMuYanling#" + VpStr
            Case "SS"
                Return "SignatureSpellbookJace#" + VpStr
            Case "C8"
                Return "commander2018#" + VpStr
            Case "GR"
                Return "guildsofravnica#" + VpStr
            Case "XP"
                Return "explorersofixalan#" + VpStr
            Case "UM"
                Return "ultimatemasters#" + VpStr
            Case "RG"
                Return "ravnicaallegiance#" + VpStr
            Case "UT"
                Return "ultimateboxtopper#" + VpStr
            Case "MY"
                Return "mythicedition#" + VpStr
            Case "PL"
                Return "asiapacificlandprogram#" + VpStr
            Case "EL"
                Return "europeanlandprogram#" + VpStr
            Case "GU"
                Return "guru#" + VpStr
            Case "CB"
                Return "commanderanthologyvolumeii#" + VpStr
            Case "WS"
                Return "warofthespark#" + VpStr
            Case "GK"
                Return "GuildsofRavnicaGuildKits#" + VpStr
            Case "MH"
                Return "modernhorizons#" + VpStr
            Case "ST"
                Return "SignatureSpellbookGideon#" + VpStr
            Case "M0"
                Return "magic2020#" + VpStr
            Case "C9"
                Return "commander2019#" + VpStr
            Case "TE"
                Return "throneofeldraine#" + VpStr
            Case Else
                Return "#" + VpStr
        End Select
    End Function
    Private Function SerieCode(VpStr As String) As String
        Select Case VpStr
            Case "10th"
                Return "1E"
            Case "3rdBB"
                Return "3B"
            Case "3rdWB"
                Return "3W"
            Case "coldsnapthemedecks"
                Return "CT"
            Case "deckmasters"
                Return "DM"
            Case "planechase"
                Return "PH"
            Case "planechase2012"
                Return "PI"
            Case "pegase"
                Return "PG"
            Case "alarareborn"
                Return "AR"
            Case "beatdown"
                Return "BT"
            Case "conflux"
                Return "CF"
            Case "coldsnap"
                Return "CS"
            Case "dissension"
                Return "DI"
            Case "eventide"
                Return "ET"
            Case "fifthdawn"
                Return "FD"
            Case "futuresight"
                Return "FS"
            Case "guildpact"
                Return "GP"
            Case "lorwyn"
                Return "LW"
            Case "magic2010"
                Return "M1"
            Case "morningtide"
                Return "MT"
            Case "ravnica"
                Return "RA"
            Case "renaissance"
                Return "RE"
            Case "shardsofalara"
                Return "SL"
            Case "shadowmoor"
                Return "SM"
            Case "timespiral"
                Return "TS"
            Case "unhinged"
                Return "UH"
            Case "worldwake"
                Return "WW"
            Case "zendikar"
                Return "ZK"
            Case "riseoftheeldrazi"
                Return "RI"
            Case "magic2011"
                Return "M2"
            Case "timeshifted"
                Return "TD"
            Case "DuelDecksDivinevsDemonic"
                Return "D1"
            Case "DuelDecksElspethvsTezzeret"
                Return "D2"
            Case "DuelDecksElvesvsGoblins"
                Return "D3"
            Case "DuelDecksGarrukvsLiliana"
                Return "D4"
            Case "DuelDecksJacevsChandra"
                Return "D5"
            Case "DuelDecksPhyrexiavstheCoalition"
                Return "D6"
            Case "DuelDecksKnightsvsDragons"
                Return "D7"
            Case "DuelDecksAjanivsNicolBolas"
                Return "D8"
            Case "FromtheVaultDragons"
                Return "V1"
            Case "FromtheVaultExiled"
                Return "V2"
            Case "FromtheVaultRelics"
                Return "V3"
            Case "scarsofmirrodin"
                Return "SD"
            Case "fireandlightining"
                Return "R1"
            Case "slivers"
                Return "R2"
            Case "mirrodinbesieged"
                Return "MB"
            Case "darksteel"
                Return "DS"
            Case "planarchaos"
                Return "PC"
            Case "newphyrexia"
                Return "NP"
            Case "magic2012"
                Return "M3"
            Case "commander"
                Return "CD"
            Case "FromtheVaultLegends"
                Return "V4"
            Case "innistrad"
                Return "IN"
            Case "darkascension"
                Return "DA"
            Case "starter2000"
                Return "S2"
            Case "graveborn"
                Return "R3"
            Case "avacynrestored"
                Return "YR"
            Case "DuelDecksVenservsKoth"
                Return "D9"
            Case "magic2013"
                Return "M4"
            Case "returntoravnica"
                Return "RR"
            Case "DuelDecksIzzetvsGolgari"
                Return "DG"
            Case "FromtheVaultRealms"
                Return "V5"
            Case "gatecrash"
                Return "GC"
            Case "DuelDecksSorinvsTibalt"
                Return "D0"
            Case "dragonsmaze"
                Return "DZ"
            Case "modernmasters"
                Return "MS"
            Case "magic2014"
                Return "M5"
            Case "DCIJudgeGift"
                Return "JG"
            Case "archenemy"
                Return "AH"
            Case "FromtheVaultTwenty"
                Return "V6"
            Case "DuelDecksHeroesvsMonsters"
                Return "DD"
            Case "theros"
                Return "TH"
            Case "commander2013"
                Return "C3"
            Case "bornofthegods"
                Return "BG"
            Case "journeyintonyx"
                Return "JN"
            Case "DuelDecksJacevsVraska"
                Return "DB"
            Case "conspiracy"
                Return "CY"
            Case "magic2015"
                Return "M6"
            Case "duelsoftheplansewalkers"
                Return "DP"
            Case "fridaynightmagic"
                Return "FM"
            Case "introductory2pset"
                Return "IS"
            Case "magicplayerrewards"
                Return "MP"
            Case "mastersedition"
                Return "ME"
            Case "moderneventdeck2014"
                Return "MV"
            Case "prereleaseevents"
                Return "PR"
            Case "releaseevents"
                Return "RT"
            Case "grandprix"
                Return "GX"
            Case "khansoftarkir"
                Return "KT"
            Case "DuelDecksSpeedvsCunning"
                Return "DH"
            Case "commander2014"
                Return "C4"
            Case "FromtheVaultAnnihilation"
                Return "V7"
            Case "fatereforged"
                Return "FR"
            Case "dragonsoftarkir"
                Return "DT"
            Case "DuelDecksKioravsElspeth"
                Return "DJ"
            Case "modernmasters2015"
                Return "MU"
            Case "origins"
                Return "OR"
            Case "DuelDecksZendikarvsEldrazi"
                Return "DC"
            Case "FromtheVaultAngels"
                Return "V8"
            Case "battleforzendikar"
                Return "BZ"
            Case "commander2015"
                Return "C5"
            Case "oathofthegatewatch"
                Return "OG"
            Case "shadowsoverinnistrad"
                Return "SI"
            Case "DuelDecksBlessedvsCursed"
                Return "DQ"
            Case "magicgamedaycards"
                Return "MG"
            Case "zendikarexpeditions"
                Return "ZX"
            Case "takethecrown"
                Return "TC"
            Case "eternalmasters"
                Return "MA"
            Case "eldritchmoon"
                Return "EM"
            Case "kaladesh"
                Return "KD"
            Case "kaladeshinventions"
                Return "KI"
            Case "DuelDecksNissavsObNixilis"
                Return "DR"
            Case "commander2016"
                Return "C6"
            Case "welcomedeck2016"
                Return "WD"
            Case "aetherrevolt"
                Return "ER"
            Case "commandersarsenal"
                Return "C1"
            Case "amonkhet"
                Return "AK"
            Case "modernmasters2017"
                Return "MW"
            Case "hourofdevastation"
                Return "HD"
            Case "archenemyNicolBolas"
                Return "AI"
            Case "clashpack"
                Return "CP"
            Case "commanderanthology"
                Return "CA"
            Case "DuelDecksAnthologyDivinevsDemonic"
                Return "A1"
            Case "DuelDecksAnthologyElvesvsGoblins"
                Return "A3"
            Case "DuelDecksAnthologyGarrukvsLiliana"
                Return "A4"
            Case "DuelDecksAnthologyJacevsChandra"
                Return "A5"
            Case "planechaseanthology"
                Return "PA"
            Case "commander2017"
                Return "C7"
            Case "DuelDecksMindvsMight"
                Return "DL"
            Case "welcomedeck2017"
                Return "WE"
            Case "amonkhetinvocations"
                Return "AJ"
            Case "wpngateway"
                Return "GW"
            Case "ixalan"
                Return "XL"
            Case "DuelDecksMerfolkvsGoblins"
                Return "DU"
            Case "FromtheVaultTransform"
                Return "VA"
            Case "iconicmasters"
                Return "IM"
            Case "unstable"
                Return "UB"
            Case "rivalsofixalan"
                Return "RX"
            Case "masters25"
                Return "A2"
            Case "dominaria"
                Return "DO"
            Case "DuelDecksElvesvsInventors"
                Return "DV"
            Case "battlebond"
                Return "BB"
            Case "magic2019"
                Return "M9"
            Case "GlobalSeriesJiangYangguMuYanling"
                Return "GS"
            Case "SignatureSpellbookJace"
                Return "SS"
            Case "commander2018"
                Return "C8"
            Case "guildsofravnica"
                Return "GR"
            Case "explorersofixalan"
                Return "XP"
            Case "ultimatemasters"
                Return "UM"
            Case "ravnicaallegiance"
                Return "RG"
            Case "ultimateboxtopper"
                Return "UT"
            Case "mythicedition"
                Return "MY"
            Case "asiapacificlandprogram"
                Return "PL"
            Case "europeanlandprogram"
                Return "EL"
            Case "guru"
                Return "GU"
            Case "commanderanthologyvolumeii"
                Return "CB"
            Case "warofthespark"
                Return "WS"
            Case "GuildsofRavnicaGuildKits"
                Return "GK"
            Case "modernhorizons"
                Return "MH"
            Case "SignatureSpellbookGideon"
                Return "ST"
            Case "magic2020"
                Return "M0"
            Case "commander2019"
                Return "C9"
            Case "throneofeldraine"
                Return "TE"
            Case Else
                Return ""
        End Select
    End Function
    Private Sub BuildHeaders(VpR14 As Boolean)
    '---------------------------------------------
    'G�n�ration du fichier d'en-t�tes des �ditions
    '---------------------------------------------
    Dim VpTxt As StreamWriter
    Dim VpStr As String
        Me.dlgSave.FileName = ""
        Me.dlgSave.ShowDialog
        If Me.dlgSave.FileName <> "" Then
            VpTxt = New StreamWriter(Me.dlgSave.FileName)
            Call Me.AddToLog("La construction du fichier d'en-t�tes a commenc�...", eLogType.Information, True)
            VmDBCommand.CommandText = "Select SeriesCD, SeriesNM, SeriesNM_MtG, MIESetID, Cycle, LegalE, LegalS, Border, Release, RunSize, TotCards, UqCards, UqRare, UqUncom, UqComm, UqBLand, Foils, StartRare, StartUncom, StartComm, StartLand, BoostRare, BoostUncom, BoostComm, BoostLand, Decks, Starters, Boosters, Boxes, Notes, SeriesNM_FR, SeriesCD_MO, SeriesCD_MW From Series;"
            VmDBReader = VmDBCommand.ExecuteReader
            With VmDBReader
                While .Read
                    Application.DoEvents
                    VpStr = ""
                    For VpI As Integer = 0 To If(VpR14, 30, 32)
                        VpStr = VpStr + Me.Matching(.GetValue(VpI).ToString) + "#"
                    Next VpI
                    VpStr = Me.SerieShortcut(VpStr.Substring(0, VpStr.Length - 1))
                    VpTxt.WriteLine(VpStr)
                    If Me.btCancel.Tag Then Exit While
                End While
                .Close
            End With
            VpTxt.Flush
            VpTxt.Close
            If Me.btCancel.Tag Then
                Call Me.AddToLog("La construction du fichier d'en-t�tes a �t� annul�e.", eLogType.Warning, , True)
            Else
                Call Me.AddToLog("La construction du fichier d'en-t�tes est termin�e.", eLogType.Information, , True)
            End If
        End If
    End Sub
    Private Sub GetScan(VpIn As String)
    '----------------------------------------------------------
    'T�l�charge l'image associ�e � la carte pass�e en param�tre
    '----------------------------------------------------------
    Dim VpClient As New WebClient
    Dim VpStr As String = ""
    Dim VpStrs() As String
    Dim VpIr As Integer
        Try
            VpStr = Me.HTMLfromRequest(CmURL1.Replace(CmId1, VpIn.Replace(" ", "+")))
            VpStrs = VpStr.Split(New String() {CmKey1}, StringSplitOptions.None)
            VpIr = Me.FindRightIndex(VpStrs, VpIn)
            VpStr = CmURL2 + VpStrs(VpIr).Substring(0, VpStrs(VpIr).IndexOf(""""))
            VpStr = Me.HTMLfromRequest(VpStr)
            If VpStr.Contains(CmKey3) Then
                VpStr = VpStr.Substring(VpStr.IndexOf(CmKey3))
                VpStr = VpStr.Substring(CmKey3.Length)
                VpStr = VpStr.Substring(0, VpStr.IndexOf(""""))
                VpClient.DownloadFile(CmURL3 + VpStr, Me.dlgBrowse.SelectedPath + "\" + VpIn.Replace(":", "").Replace("/", "").Replace("""", "").Replace("?", "") + ".jpg")
            ElseIf VpStr.Contains(CmKey4) Then
                VpStr = VpStr.Substring(VpStr.IndexOf(CmKey4) + 5)
                VpStr = VpStr.Substring(0, VpStr.IndexOf(""""))
                VpClient.DownloadFile(VpStr, Me.dlgBrowse.SelectedPath + "\" + VpIn.Replace(":", "").Replace("/", "").Replace("""", "").Replace("?", "") + ".jpg")
            ElseIf VpStr.Contains(CmKey4B) Then
                VpStr = VpStr.Substring(VpStr.IndexOf(CmKey4B) + 6)
                VpStr = VpStr.Substring(0, VpStr.IndexOf(""""))
                VpClient.DownloadFile(CmURL4 + VpStr, Me.dlgBrowse.SelectedPath + "\" + VpIn.Replace(":", "").Replace("/", "").Replace("""", "").Replace("?", "") + ".jpg")
            End If
        Catch
        End Try
    End Sub
    Private Sub DownloadSymbolsOrThumbs(VpURL As String, VpDesc As String, VpDesc1 As String)
    '----------------------------------------------------------------
    'T�l�charge les symboles ou les miniatures de toutes les �ditions
    '----------------------------------------------------------------
    Dim VpClient As New WebClient
        Me.dlgBrowse.SelectedPath = ""
        Me.dlgBrowse.ShowDialog
        If Me.dlgBrowse.SelectedPath <> "" Then
            Call Me.AddToLog("La r�cup�ration des " + VpDesc + " des �ditions a commenc�...", eLogType.Information, True)
            VmDBCommand.CommandText = "Select SeriesCD_MO, SeriesNM From Series;"
            VmDBReader = VmDBCommand.ExecuteReader
            With VmDBReader
                While .Read
                    Try
                        ServicePointManager.SecurityProtocol = &H00000C00
                        VpClient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:67.0) Gecko/20100101 Firefox/67.0")
                        VpClient.DownloadFile(VpURL.Replace("#id#", .GetString(0).ToLower), Me.dlgBrowse.SelectedPath + "\" + .GetString(0) + ".png")
                    Catch
                        Call Me.AddToLog("Impossible de r�cup�rer " + VpDesc1 + " de l'�dition : " + .GetString(1), eLogType.Warning)
                    End Try
                    Application.DoEvents
                    If Me.btCancel.Tag Then Exit While
                End While
                .Close
            End With
            If Me.btCancel.Tag Then
                Call Me.AddToLog("La r�cup�ration des " + VpDesc + " des �ditions a �t� annul�e.", eLogType.Warning, , True)
            Else
                Call Me.AddToLog("La r�cup�ration des " + VpDesc + " des �ditions est termin�e.", eLogType.Information, , True)
            End If
        End If
    End Sub
    Private Sub DownloadPictures
    '---------------------------------------------------------------------------
    'T�l�charge les images associ�es aux cartes list�es dans le fichier sp�cifi�
    '---------------------------------------------------------------------------
    Dim VpListe As List(Of String)
        Me.dlgOpen4.FileName = ""
        Me.dlgOpen4.ShowDialog
        If Me.dlgOpen4.FileName <> "" Then
            Me.dlgBrowse.SelectedPath = ""
            Me.dlgBrowse.ShowDialog
            If Me.dlgBrowse.SelectedPath <> "" Then
                Call Me.AddToLog("La r�cup�ration des images des cartes a commenc�...", eLogType.Information, True)
                'Construction de la liste
                VpListe = Me.BuildListeFromFile
                'R�cup�ration des images
                Me.prgAvance.Maximum = VpListe.Count
                Me.prgAvance.Value = 0
                Me.prgAvance.Style = ProgressBarStyle.Blocks
                For Each VpCard As String In VpListe
                    Application.DoEvents
                    If VpCard.Trim <> "" Then
                        Me.txtCur.Text = VpCard
                        Call Me.GetScan(VpCard)
                        If Not File.Exists(Me.dlgBrowse.SelectedPath + "\" + VpCard + ".jpg") Then
                            Call Me.AddToLog("Impossible de r�cup�rer l'image de la carte : " + VpCard, eLogType.Warning)
                        End If
                    End If
                    Me.prgAvance.Increment(1)
                    Call Me.ETA
                    If Me.btCancel.Tag Then Exit For
                Next VpCard
                If Me.btCancel.Tag Then
                    Call Me.AddToLog("La r�cup�ration des images des cartes a �t� annul�e.", eLogType.Warning, , True)
                Else
                    Call Me.AddToLog("La r�cup�ration des images des cartes est termin�e.", eLogType.Information, , True)
                End If
            End If
        End If
    End Sub
    Private Function GetListing(VpCode As String, VpLanguage As String) As List(Of String)
    '----------------------------------------------------------------------------
    'R�cup�re la liste des cartes dans la langue demand�e pour l'�dition demand�e
    '----------------------------------------------------------------------------
    Dim VpStr As String = ""
    Dim VpKey As String
    Dim VpItem As String
    Dim VpListe As New List(Of String)
        VpStr = Me.HTMLfromRequest(CmURL5.Replace("###", VpCode).Replace("^^", VpLanguage))
        VpKey = CmKey6.Replace("###", VpCode).Replace("^^", VpLanguage)
        If VpStr.Contains(VpKey) Then
            VpStr = VpStr.Substring(VpStr.IndexOf(VpKey))
            While VpStr.Contains(CmKey6B)
                VpStr = VpStr.Substring(VpStr.IndexOf(CmKey6B) + CmKey6B.Length)
                VpItem = VpStr.Substring(0, VpStr.IndexOf("</"))
                VpListe.Add(VpItem)
            End While
        End If
        Return VpListe
    End Function
    Private Sub DownloadSpoilers(VpCode As String)
    '-----------------------------------------------------------------------------------------------------------
    'Construit les fichiers (listing avec traduction, checklist, spoilerlist) n�cessaires � l'ajout de l'�dition
    '-----------------------------------------------------------------------------------------------------------
    Dim VpClient As New WebClient
    Dim VpSerializer As JavaScriptSerializer
    Dim VpJSONInfos As clsFullInfos = Nothing
        'R�cup�ration du json
        Call Me.AddToLog("T�l�chargement des informations depuis MTG JSON...", eLogType.Information)
        Application.DoEvents
        VpClient.DownloadFile(CmURL7.Replace("###", VpCode.ToUpper), Me.dlgBrowse.SelectedPath + "\" + VpCode + ".json")
        VpSerializer = New JavaScriptSerializer
        VpSerializer.MaxJsonLength = Integer.MaxValue
        VpJSONInfos = VpSerializer.Deserialize(Of clsFullInfos)((New StreamReader(Me.dlgBrowse.SelectedPath + "\" + VpCode + ".json")).ReadToEnd)
        'Infos sur l'�dition
        Call Me.AddToLog("*** Nom VO : " + VpJSONInfos.name, eLogType.Information)
        If VpJSONInfos.translations IsNot Nothing AndAlso VpJSONInfos.translations.ContainsKey("fr") Then
            Call Me.AddToLog("*** Nom VF : " + VpJSONInfos.translations.Item("fr"), eLogType.Information)
        End If
        Call Me.AddToLog("*** Date de sortie : " + VpJSONInfos.releaseDate, eLogType.Information)
        Call Me.AddToLog("*** Bordure des cartes : " + VpJSONInfos.border, eLogType.Information)
        Call Me.AddToLog("*** Extension : " + VpJSONInfos.block, eLogType.Information)
        Call Me.AddToLog("*** Code : " + VpJSONInfos.code.ToUpper, eLogType.Information)
        'Construction des listings
        Call Me.AddToLog("Construction du listing VO/VF...", eLogType.Information)
        Application.DoEvents
        Call Me.BuildAllTitles(VpJSONInfos, "_titles_fr.txt")
        Call Me.AddToLog("Construction de la checklist...", eLogType.Information)
        Application.DoEvents
        Call Me.BuildCheckList(VpJSONInfos, "_checklist_en.txt")
        Call Me.AddToLog("Construction de la spoilerlist...", eLogType.Information)
        Application.DoEvents
        Call Me.BuildSpoilerList(VpJSONInfos, "_spoiler_en.txt")
        Call Me.AddToLog("Construction du listing des doubles cartes...", eLogType.Information)
        Application.DoEvents
        Call Me.BuildDoubles(VpJSONInfos, "_doubles_en.txt")
        'Listings alternatifs (�ventuellement)
        If VpJSONInfos.special Then
            If MessageBox.Show("L'�dition semble contenir des cartes sp�ciales (double face ou double carte ou aventure)" + vbCrLf + "Voulez-vous g�n�rer les listings alternatifs ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = System.Windows.Forms.DialogResult.Yes Then
                Call Me.AddToLog("Construction des listings alternatifs...", eLogType.Information)
                Application.DoEvents
                Call Me.BuildAllTitles(VpJSONInfos, "_titles_fr_special.txt")
                Call Me.BuildCheckList(VpJSONInfos, "_checklist_en_special.txt")
                Call Me.BuildSpoilerList(VpJSONInfos, "_spoiler_en_special.txt")
            End If
        End If
        Call Me.AddToLog("La construction des fichiers spoilers est termin�e.", eLogType.Information)
    End Sub
    Private Sub BuildAllTitles(VpJSONInfos As clsFullInfos, VpSuffix As String)
    Dim VpOut As New StreamWriter(Me.dlgBrowse.SelectedPath + "\" + VpJSONInfos.name.ToLower.Replace(":", "").Replace(" ", "") + VpSuffix)
    Dim VpAlready As New List(Of String)
        For Each VpCard As clsFullInfos.clsFullCardInfos In VpJSONInfos.cards
            With VpCard
                If Not VpAlready.Contains(.name) AndAlso .foreignData IsNot Nothing AndAlso .foreignData.Count > 0 Then
                    If .linkedTo IsNot Nothing Then
                        VpOut.WriteLine(.name + " // " + .linkedTo.name + "#" + .getForeignName("French") + " // " + .linkedTo.getForeignName("French"))
                    ElseIf .linkedFrom Is Nothing Then
                        VpOut.WriteLine(.name + "#" + .getForeignName("French"))
                    End If
                    VpAlready.Add(.name)
                End If
            End With
        Next VpCard
        VpOut.Flush
        VpOut.Close
    End Sub
    Private Sub BuildCheckList(VpJSONInfos As clsFullInfos, VpSuffix As String)
    Dim VpOut As New StreamWriter(Me.dlgBrowse.SelectedPath + "\" + VpJSONInfos.name.ToLower.Replace(":", "").Replace(" ", "") + VpSuffix)
    Dim VpColors As String
    Dim VpDone As New List(Of String)
        VpOut.WriteLine("#" + vbTab + "Name" + vbTab + "Artist" + vbTab + "Color" + vbTab + "Rarity" + vbTab + "Set")
        VpJSONInfos.cards.Sort(New clsFullInfos.clsFullCardInfosComparer)
        For Each VpCard As clsFullInfos.clsFullCardInfos In VpJSONInfos.cards
            With VpCard
                If Not VpDone.Contains(.name) Then
                    If .colors Is Nothing OrElse .colors.Count = 0 Then
                        If .type <> "Land" AndAlso Not .type.StartsWith("Basic Land") Then
                            If Not VpSuffix.Contains("special") Then
                                Call Me.AddToLog("V�rifier la couleur de la carte : " + .name, eLogType.Warning)
                            End If
                        End If
                        VpColors = "/"
                    Else
                        VpColors = ""
                        For Each VpColor As String In .getMergedColors
                            Select Case VpColor.ToUpper
                                Case "W"
                                    VpColor = "White"
                                Case "U"
                                    VpColor = "Blue"
                                Case "R"
                                    VpColor = "Red"
                                Case "G"
                                    VpColor = "Green"
                                Case "B"
                                    VpColor = "Black"
                                Case Else
                            End Select
                            VpColors += "/" + VpColor
                        Next VpColor
                    End If
                    If .linkedTo IsNot Nothing Then
                        VpOut.WriteLine(.number.ToString.Replace("a", "").Replace("b", "") + vbTab + .name + " // " + .linkedTo.name + vbTab + .artist + vbTab + VpColors.Substring(1) + vbTab + .rarity.Substring(0, 1).ToUpper.Replace("B", "L") + vbTab + VpJSONInfos.name)
                    ElseIf .linkedFrom Is Nothing Then
                        VpOut.WriteLine(.number.ToString.Replace("a", "").Replace("b", "") + vbTab + .name + vbTab + .artist + vbTab + VpColors.Substring(1) + vbTab + .rarity.Substring(0, 1).ToUpper.Replace("B", "L") + vbTab + VpJSONInfos.name)
                    End If
                    VpDone.Add(.name)
                End If
            End With
        Next VpCard
        VpOut.Flush
        VpOut.Close
    End Sub
    Private Sub BuildSpoilerList(VpJSONInfos As clsFullInfos, VpSuffix As String)
    Dim VpOut As New StreamWriter(Me.dlgBrowse.SelectedPath + "\" + VpJSONInfos.name.ToLower.Replace(":", "").Replace(" ", "") + VpSuffix)
    Dim VpRarity As String
    Dim VpDone As New List(Of String)
        For Each VpCard As clsFullInfos.clsFullCardInfos In VpJSONInfos.cards
            With VpCard
                If Not VpDone.Contains(.name) Then
                    If .linkedTo IsNot Nothing Then
                        VpOut.WriteLine("Name: " + vbTab + .name + " // " + .linkedTo.name)
                        VpOut.WriteLine("Cost: " + vbTab + .getCost + " // " + .linkedTo.getCost)
                    ElseIf .linkedFrom Is Nothing Then
                        VpOut.WriteLine("Name: " + vbTab + .name)
                        VpOut.WriteLine("Cost: " + vbTab + .getCost)
                    End If
                    VpOut.WriteLine("Type: " + vbTab + .type)
                    If .types.Contains("Creature") Then
                        VpOut.WriteLine("Pow/Tgh: " + vbTab + "(" + .power + "/" + .toughness +")")
                    ElseIf .types.Contains("Planeswalker") Then
                        VpOut.WriteLine("Pow/Tgh: " + vbTab + "(0/" + .loyalty.ToString +")")
                    Else
                        VpOut.WriteLine("Pow/Tgh: " + vbTab)
                    End If
                    If .linkedTo IsNot Nothing Then
                        VpOut.WriteLine("Rules Text: " + vbTab + .getRules + "/#/" + .linkedTo.getRules)
                    ElseIf .linkedFrom Is Nothing Then
                        VpOut.WriteLine("Rules Text: " + vbTab + .getRules)
                    End If
                    Select Case .rarity
                        Case "land", "basic land"
                            VpRarity = "Land"
                        Case "common"
                            VpRarity = "Common"
                        Case "uncommon"
                            VpRarity = "Uncommon"
                        Case "rare"
                            VpRarity = "Rare"
                        Case "mythic", "mythic rare"
                            VpRarity = "Mythic Rare"
                        Case Else
                            VpRarity = .rarity
                    End Select
                    VpOut.WriteLine("Set/Rarity: " + vbTab + VpJSONInfos.name + " " + VpRarity)
                    VpOut.WriteLine("")
                    VpDone.Add(.name)
                End If
            End With
        Next VpCard
        VpOut.Flush
        VpOut.Close
    End Sub
    Private Sub BuildDoubles(VpJSONInfos As clsFullInfos, VpSuffix As String)
    Dim VpOut As New StreamWriter(Me.dlgBrowse.SelectedPath + "\" + VpJSONInfos.name.ToLower.Replace(":", "").Replace(" ", "") + VpSuffix)
    Dim VpDone As New List(Of String)
        For Each VpCard As clsFullInfos.clsFullCardInfos In VpJSONInfos.cards
            With VpCard
                If Not VpDone.Contains(.name) Then
                    If .names IsNot Nothing AndAlso .names.Count = 2 AndAlso .names.Item(0) = .name Then
                        VpOut.WriteLine(.names.Item(1) + "#" + .names.Item(0))
                        'association crois�e
                        For Each VpMate As clsFullInfos.clsFullCardInfos In VpJSONInfos.cards
                            If VpMate.name = .names.Item(1) Then
                                VpCard.linkedTo = VpMate
                                VpMate.linkedFrom = VpCard
                                Exit For
                            End If
                        Next VpMate
                        VpJSONInfos.special = True
                    End If
                    VpDone.Add(.name)
                End If
            End With
        Next VpCard
        VpOut.Flush
        VpOut.Close
    End Sub
    Private Sub ListSubtypes
    '---------------------------------------------------------------------------------------------------
    'Liste les sous-types non traduits et les trie par ordre d'occurrences, du plus r�pandu au plus rare
    '---------------------------------------------------------------------------------------------------
        Call Me.AddToLog("L'analyse des sous-types a commenc�...", eLogType.Information)
        Application.DoEvents
        VmDBCommand.CommandText = "Select SubType, Count(*) From Card Where Not Exists (Select SubTypeVO From SubTypes Where Card.SubType = SubTypes.SubTypeVO) Group By SubType Order By 2 Desc;"
        VmDBReader = VmDBCommand.ExecuteReader
        With VmDBReader
            While .Read
                If Not .IsDBNull(0) AndAlso .GetString(0).Trim <> "" Then
                    Call Me.AddToLog("Sous-type non traduit : " + .GetString(0) + " (" + .GetInt32(1).ToString + ")", eLogType.Warning)
                    Application.DoEvents
                End If
            End While
            .Close
        End With
        Call Me.AddToLog("L'analyse des sous-types est termin�e.", eLogType.Information)
    End Sub
    Private Sub ExtractTexts
    '-----------------------------------------
    'Extrait les textes des cartes en fran�ais
    '-----------------------------------------
    Dim VpOut As StreamWriter
        Me.dlgSave.FileName = ""
        Me.dlgSave.ShowDialog
        If Me.dlgSave.FileName <> "" Then
            VpOut = New StreamWriter(Me.dlgSave.FileName)
            Call Me.AddToLog("L'extraction des textes des cartes en fran�ais a commenc�...", eLogType.Information, True)
            Me.prgAvance.Style = ProgressBarStyle.Marquee
            VmDBCommand.CommandText = "Select Distinct TextesFR.CardName, TextesFR.TexteFR From TextesFR Inner Join Card On TextesFR.CardName = Card.Title Where Card.CardText <> TextesFR.TexteFR;"
            VmDBReader = VmDBCommand.ExecuteReader
            With VmDBReader
                While .Read
                    Me.txtCur.Text = .GetString(0)
                    Application.DoEvents
                    Try
                        If .GetString(1).Trim <> "" Then
                            VpOut.Write("##" + .GetString(0) + "^^" + .GetString(1))
                        End If
                    Catch
                        Call Me.AddToLog("Impossible de r�cup�rer le texte de la carte : " + .GetString(0), eLogType.Warning)
                    End Try
                    If Me.btCancel.Tag Then Exit While
                End While
                .Close
            End With
            VpOut.Flush
            VpOut.Close
            If Me.btCancel.Tag Then
                Call Me.AddToLog("L'extraction des textes des cartes en fran�ais a �t� annul�e.", eLogType.Warning, , True)
            Else
                Call Me.AddToLog("L'extraction des textes des cartes en fran�ais est termin�e.", eLogType.Information, , True)
            End If
        End If
    End Sub
    Private Sub CompareTexts
    '------------------------------------------------------------------------------------------------------------------------
    'Comparaison interactive d'un fichier de textes des cartes en fran�ais avec leur version courante dans la base de donn�es
    '------------------------------------------------------------------------------------------------------------------------
    Dim VpTxt As StreamReader
    Dim VpStrs() As String
    Dim VpItem() As String
    Dim VpTrad As New Hashtable
    Dim VpTradDB As New Hashtable
    Dim VpTitle As String
    Dim VpO As Object
        Me.dlgOpen2.FileName = ""
        Me.dlgOpen2.ShowDialog
        If Me.dlgOpen2.FileName <> "" Then
            VpTxt = New StreamReader(Me.dlgOpen2.FileName)
            VpStrs = VpTxt.ReadToEnd.Split(New String() {"##"}, StringSplitOptions.None)
            'Parse le contenu du fichier
            For VpI As Integer = 1 To VpStrs.Length - 1
                VpItem = VpStrs(VpI).Split(New String() {"^^"}, StringSplitOptions.None)
                VpTrad.Add(VpItem(0), VpItem(1))
            Next VpI
            VpTxt.Close
            'Pareil pour la base de donn�es
            VmDBCommand.CommandText = "Select Card.Title, TextesFR.TexteFR From Card Inner Join TextesFR On Card.Title = TextesFR.CardName"
            VmDBReader = VmDBCommand.ExecuteReader
            With VmDBReader
                While .Read
                    If Not VpTradDB.ContainsKey(.GetString(0)) And Not .IsDBNull(1) Then
                        VpTradDB.Add(.GetString(0), .GetString(1))
                    End If
                End While
                .Close
            End With
            'Changement interactif
            Call Me.AddToLog("La modification interactive des traductions a commenc�...", eLogType.Information, True)
            While MessageBox.Show("Voulez-vous changer une traduction ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = System.Windows.Forms.DialogResult.Yes
                VpTitle = InputBox("Quel est le nom de la carte dont la traduction � remplacer ?", "Mise � jour de texte", "(carte)")
                If VpTitle <> "" Then
                    VmDBCommand.CommandText = "Select Title From Card Where InStr(UCase(Title), '" + VpTitle.Replace("'", "''").ToUpper + "') > 0;"
                    VpO = VmDBCommand.ExecuteScalar
                    If Not VpO Is Nothing
                        VpTitle = VpO.ToString
                        If MessageBox.Show("Carte correspondante trouv�e : " + VpTitle + vbCrLf + "Voulez-vous changer son texte traduit VF ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = System.Windows.Forms.DialogResult.Yes Then
                            If VpTrad.ContainsKey(VpTitle) Then
                                VmDBCommand.CommandText = "Update TextesFR Set TexteFR = '" + VpTrad.Item(VpTitle).Replace("'", "''")  + "' Where CardName = '" + VpTitle.Replace("'", "''") + "';"
                                VmDBCommand.ExecuteNonQuery
                                Call Me.AddToLog("Texte VF mis � jour pour la carte : " + VpTitle, eLogType.Information)
                            Else
                                Call Me.AddToLog("Texte VF non trouv� dans le fichier pour la carte : " + VpTitle, eLogType.Warning)
                            End If
                        End If
                    End If
                End If
            End While
            Call Me.AddToLog("La modification interactive des traductions est termin�e.", eLogType.Information, , True)
        End If
    End Sub
    Private Sub CheckTexts
    '------------------------------------------------------------------
    'V�rifie la coh�rence d'un fichier de textes des cartes en fran�ais
    '------------------------------------------------------------------
    Dim VpTxt As StreamReader
    Dim VpStrs() As String
    Dim VpItem() As String
    Dim VpTrad As New SortedList(Of String, String)
    Dim VpFR As String
    Dim VpToRemove As New List(Of String)
    Dim VpOut As StreamWriter
        Me.dlgOpen2.FileName = ""
        Me.dlgOpen2.ShowDialog
        If Me.dlgOpen2.FileName <> "" Then
            Me.dlgSave.FileName = ""
            Me.dlgSave.ShowDialog
            If Me.dlgSave.FileName <> "" Then
                Call Me.AddToLog("La v�rification du fichier des traductions a commenc�...", eLogType.Information, True)
                VpTxt = New StreamReader(Me.dlgOpen2.FileName)
                VpStrs = VpTxt.ReadToEnd.Split(New String() {"##"}, StringSplitOptions.None)
                'Parse le contenu du fichier
                For VpI As Integer = 1 To VpStrs.Length - 1
                    VpItem = VpStrs(VpI).Split(New String() {"^^"}, StringSplitOptions.None)
                    VpTrad.Add(VpItem(0), VpItem(1))
                Next VpI
                VpTxt.Close
                'Regarde s'il existe des traductions identiques correspondant � des noms similaires de cartes diff�rentes
                For Each VpFR1 As String In VpTrad.Keys
                    For Each VpFR2 As String In VpTrad.Keys
                        Application.DoEvents
                        VpFR = ""
                        If VpTrad.Item(VpFR1) = VpTrad.Item(VpFR2) Then
                            If VpFR1 <> VpFR2 Then
                                If VpFR1.Contains(VpFR2) Then
                                    VpFR = VpFR2
                                ElseIf VpFR2.Contains(VpFR1) Then
                                    VpFR = VpFR1
                                End If
                                If VpFR <> "" Then
                                    If Not VpToRemove.Contains(VpFR) Then
                                        Call Me.AddToLog("Suppression de la traduction pour la carte : " + VpFR, eLogType.Warning)
                                        VpToRemove.Add(VpFR)
                                    End If
                                End If
                            End If
                        End If
                        If Me.btCancel.Tag Then Exit For
                    Next VpFR2
                    If Me.btCancel.Tag Then Exit For
                Next VpFR1
                'Supprime les traductions erron�es
                For Each VpFR In VpToRemove
                    VpTrad.Remove(VpFR)
                Next VpFR
                'Reconstruit le fichier corrig�
                VpOut = New StreamWriter(Me.dlgSave.FileName)
                For Each VpFR In VpTrad.Keys
                    VpOut.Write("##" + VpFR + "^^" + VpTrad.Item(VpFR))
                Next VpFR
                VpOut.Flush
                VpOut.Close
                If Me.btCancel.Tag Then
                    Call Me.AddToLog("La v�rification du fichier des traductions a �t� annul�e.", eLogType.Warning, , True)
                Else
                    Call Me.AddToLog("La v�rification du fichier des traductions est termin�e.", eLogType.Information, , True)
                End If
            End If
        End If
    End Sub
    Private Sub BuildTitles
    '---------------------------------------------------------------------------------------------------------------
    'Construit un fichier de titres des cartes en fran�ais pour l'�dition demand�e, avec les infos d�j� dans la base
    '---------------------------------------------------------------------------------------------------------------
    Dim VpSerie As String = InputBox("Entrer le code de l'�dition")
    Dim VpOut As StreamWriter
        If VpSerie.Length = 2 Then
            Me.dlgSave.FileName = ""
            Me.dlgSave.ShowDialog
            If Me.dlgSave.FileName <> "" Then
                VpOut = New StreamWriter(Me.dlgSave.FileName)
                Call Me.AddToLog("La construction du fichier des titres traduits a commenc�...", eLogType.Information, True)
                Me.prgAvance.Style = ProgressBarStyle.Marquee
                VmDBCommand.CommandText = "Select Card.Title, CardFR.TitleFR From (Card Inner Join Series On Card.Series = Series.SeriesCD) Inner Join CardFR On Card.EncNbr = CardFR.EncNbr Where UCase(Series.SeriesCD) = '" + VpSerie.ToUpper + "';"
                VmDBReader = VmDBCommand.ExecuteReader
                With VmDBReader
                    While .Read
                        Me.txtCur.Text = .GetString(0)
                        Application.DoEvents
                        Try
                            If .GetString(1).Trim <> "" And .GetString(1) <> .GetString(0) Then
                                VpOut.WriteLine(.GetString(0) + "#" + .GetString(1))
                            End If
                        Catch
                            Call Me.AddToLog("Impossible de r�cup�rer le titre de la carte : " + .GetString(0), eLogType.Warning)
                        End Try
                        If Me.btCancel.Tag Then Exit While
                    End While
                    .Close
                End With
                VpOut.Flush
                VpOut.Close
                If Me.btCancel.Tag Then
                    Call Me.AddToLog("La construction du fichier des titres traduits a �t� annul�e.", eLogType.Warning, , True)
                Else
                    Call Me.AddToLog("La construction du fichier des titres traduits est termin�e.", eLogType.Information, , True)
                End If
            End If
        End If
    End Sub
    Private Sub BuildDouble
    '----------------------------------------------------------------------------------------------------------------------------
    'Construit le fichier des doubles cartes pour l'�dition demand�e, avec les infos d�j� dans la base (associations recto-verso)
    '----------------------------------------------------------------------------------------------------------------------------
    Dim VpSerie As String = InputBox("Entrer le code de l'�dition")
    Dim VpOut As StreamWriter
    Dim VpDown As Hashtable
    Dim VpTop As Hashtable
        If VpSerie.Length = 2 Then
            Me.dlgSave.FileName = ""
            Me.dlgSave.ShowDialog
            If Me.dlgSave.FileName <> "" Then
                VpOut = New StreamWriter(Me.dlgSave.FileName)
                Call Me.AddToLog("La construction du fichier des doubles cartes a commenc�...", eLogType.Information, True)
                Me.prgAvance.Style = ProgressBarStyle.Marquee
                VpDown = Me.BuildDoubleHash("EncNbrDownFace", VpSerie)
                VpTop = Me.BuildDoubleHash("EncNbrTopFace", VpSerie)
                For Each VpEncNbr1 As Integer In VpDown.Keys
                    Application.DoEvents
                    For Each VpEncNbr2 As Integer In VpTop.Keys
                        Application.DoEvents
                        If VpEncNbr1 = VpEncNbr2 Then
                            VpOut.WriteLine(VpDown.Item(VpEncNbr1) + "#" + VpTop.Item(VpEncNbr1))
                            Exit For
                        End If
                    Next VpEncNbr2
                    If Me.btCancel.Tag Then Exit For
                Next VpEncNbr1
                VpOut.Flush
                VpOut.Close
                If Me.btCancel.Tag Then
                    Call Me.AddToLog("La construction du fichier des doubles cartes a �t� annul�e.", eLogType.Warning, , True)
                Else
                    Call Me.AddToLog("La construction du fichier des doubles cartes est termin�e.", eLogType.Information, , True)
                End If
            End If
        End If
    End Sub
    Private Function BuildDoubleHash(VpField As String, VpSerie As String) As Hashtable
    Dim VpHash As New Hashtable
        VmDBCommand.CommandText = "Select Card.Title, CardDouble.EncNbrTopFace From Card Inner Join CardDouble On CardDouble." + VpField + " = Card.EncNbr Where UCase(Card.Series) = '" + VpSerie.ToUpper + "';"
        VmDBReader = VmDBCommand.ExecuteReader
        With VmDBReader
            While .Read
                Application.DoEvents
                VpHash.Add(.GetInt32(1), .GetString(0))
                If Me.btCancel.Tag Then Exit While
            End While
            .Close
        End With
        Return VpHash
    End Function
    Private Sub ExtractTitles
    '-----------------------------------------
    'Extrait les titres des cartes en fran�ais
    '-----------------------------------------
    Dim VpIn As StreamReader
    Dim VpOut As StreamWriter
    Dim VpStr As String
    Dim VpStr2 As String
        Me.dlgOpen4.FileName = ""
        Me.dlgOpen4.ShowDialog
        If Me.dlgOpen4.FileName <> "" Then
            Me.dlgSave.FileName = ""
            Me.dlgSave.ShowDialog
            If Me.dlgSave.FileName <> "" Then
                VpIn = New StreamReader(Me.dlgOpen4.FileName, Encoding.Default)
                VpOut = New StreamWriter(Me.dlgSave.FileName, False, VpIn.CurrentEncoding)
                Call Me.AddToLog("Le filtrage des titres des cartes en fran�ais a commenc�...", eLogType.Information, True)
                Me.prgAvance.Style = ProgressBarStyle.Marquee
                Application.DoEvents
                While Not VpIn.EndOfStream
                    VpStr = VpIn.ReadLine
                    If VpStr.StartsWith("Name:") Then
                        If VpStr.Contains("(") Then
                            VpStr2 = VpStr.Substring(VpStr.IndexOf("(") + 1)
                            If VpStr2.Contains("(") Then
                                VpStr2 = VpStr2.Substring(VpStr2.IndexOf("(") + 1)
                            End If
                            VpStr2 = VpStr2.Substring(0, VpStr2.Length - 1)
                            VpStr = VpStr.Replace("Name:", "").Replace("(" + VpStr2 + ")", "").Trim
                        Else
                            VpStr = VpStr.Replace("Name:", "").Trim
                            VpStr2 = VpStr
                        End If
                        VpOut.WriteLine(VpStr2 + "#" + VpStr)
                    End If
                End While
                Call Me.AddToLog("Le filtrage des titres des cartes en fran�ais est termin�.", eLogType.Information, , True)
                VpOut.Flush
                VpOut.Close
                VpIn.Close
            End If
        End If
    End Sub
    Private Sub BuildPatch
    '------------------------------------------------
    'Construction du Patch r9 (mod�les et historique)
    '------------------------------------------------
        Call Me.AddToLog("La construction du patch a commenc�...", eLogType.Information, True)
        Me.prgAvance.Style = ProgressBarStyle.Marquee
        Application.DoEvents
        With VmDBCommand
            Try
                '.CommandText = "Delete * From MSysRelationships;"
                '.ExecuteNonQuery
                .CommandText = "Drop Table Series;"
                .ExecuteNonQuery
                .CommandText = "Drop Table Autorisations;"
                .ExecuteNonQuery
                .CommandText = "Drop Table CardDouble;"
                .ExecuteNonQuery
                .CommandText = "Drop Table CardFR;"
                .ExecuteNonQuery
                .CommandText = "Drop Table CardPictures;"
                .ExecuteNonQuery
                .CommandText = "Drop Table Creature;"
                .ExecuteNonQuery
                .CommandText = "Drop Table MyAdversairesID;"
                .ExecuteNonQuery
                .CommandText = "Drop Table MyCollection;"
                .ExecuteNonQuery
                .CommandText = "Drop Table MyGames;"
                .ExecuteNonQuery
                .CommandText = "Drop Table MyGamesID;"
                .ExecuteNonQuery
                .CommandText = "Drop Table MyScores;"
                .ExecuteNonQuery
                .CommandText = "Delete * From MySpecialUses;"
                .ExecuteNonQuery
                .CommandText = "Drop Table Spell;"
                .ExecuteNonQuery
                .CommandText = "Drop Table TextesFR;"
                .ExecuteNonQuery
                .CommandText = "Alter Table PricesHistory Add Title Text(80) With Compression;"
                .ExecuteNonQuery
                .CommandText = "Alter Table PricesHistory Add Series Text(2) With Compression;"
                .ExecuteNonQuery
                .CommandText = "Update Card Inner Join PricesHistory On Card.EncNbr = PricesHistory.EncNbr Set PricesHistory.Title = Card.Title, PricesHistory.Series = Card.Series Where PricesHistory.EncNbr = Card.EncNbr;"
                .ExecuteNonQuery
                .CommandText = "Drop Index EncNbr On PricesHistory;"
                .ExecuteNonQuery
                .CommandText = "Alter Table PricesHistory Drop Column EncNbr;"
                .ExecuteNonQuery
                .CommandText = "Drop Table Card;"
                .ExecuteNonQuery
            Catch
                Call Me.AddToLog("La construction du patch a �chou� car la base contient encore des relations.", eLogType.MyError, , True)
                Exit Sub
            End Try
        End With
        Call Me.AddToLog("La construction du patch est termin�e.", eLogType.Information, , True)
    End Sub
    Private Sub ReadyForRelease
    '-------------------------------------------
    'Pr�pare la base pour l'int�gration au setup
    '-------------------------------------------
        Call Me.AddToLog("La pr�paration de la base a commenc�...", eLogType.Information, True)
        Me.prgAvance.Style = ProgressBarStyle.Marquee
        Application.DoEvents
        With VmDBCommand
            .CommandText = "Delete * From MyScores;"
            .ExecuteNonQuery
            .CommandText = "Delete * From MySpecialUses;"
            .ExecuteNonQuery
            .CommandText = "Delete * From MyCollection;"
            .ExecuteNonQuery
            .CommandText = "Delete * From MyGames;"
            .ExecuteNonQuery
            .CommandText = "Delete * From MyGamesID;"
            .ExecuteNonQuery
            .CommandText = "Delete * From MyAdversairesID;"
            .ExecuteNonQuery
            .CommandText = "Insert Into MyAdversairesID(AdvID, AdvName) Values (0, '" + CmMe + "');"
            .ExecuteNonQuery
        End With
        Call Me.AddToLog("La pr�paration de la base est termin�e.", eLogType.Information, , True)
    End Sub
    Private Sub ReadyINI
    '--------------------------------------------------
    'Pr�pare le fichier INI pour l'int�gration au setup
    '--------------------------------------------------
    Dim VpDir As String
    Dim VpINIPath As String
    Dim VpStampFile() As String
        Me.dlgOpen5.FileName = ""
        Me.dlgOpen5.ShowDialog
        VpINIPath = Me.dlgOpen5.FileName
        If VpINIPath <> "" Then
            VpDir = VpINIPath.Substring(0, VpINIPath.LastIndexOf("\") + 1)
            Call Me.AddToLog("La pr�paration du fichier de configuration a commenc�...", eLogType.Information, True)
            Application.DoEvents
            If File.Exists(VpDir + CmStamp) Then
                VpStampFile = File.ReadAllLines(VpDir + CmStamp)
                For VpI As Integer = 0 To CmIndexes.Length - 1
                    Call WritePrivateProfileString(CmCategory, CmFields(VpI), VpStampFile(CmIndexes(VpI) - 1), VpINIPath)
                Next VpI
            Else
                Call Me.AddToLog("Impossible de trouver le fichier de r�f�rence " + CmStamp + ".", eLogType.MyError, , True)
                Exit Sub
            End If
            Call Me.AddToLog("La pr�paration du fichier de configuration est termin�e.", eLogType.Information, , True)
        End If
    End Sub
    Private Sub BuildSP
    '---------------------------------------------------------------------------------------
    'Construit un nouveau Service Pack d'images en concat�nant celles du r�pertoire sp�cifi�
    '---------------------------------------------------------------------------------------
    Dim VpCurIn As StreamReader
    Dim VpCurInB As BinaryReader
    Dim VpOutFull As StreamWriter
    Dim VpOutFullB As BinaryWriter
    Dim VpOutLog As StreamWriter
    Dim VpDirIn As DirectoryInfo
    Dim VpCumul As Long = 0
        Me.dlgBrowse.SelectedPath = ""
        Me.dlgBrowse.ShowDialog
        If Me.dlgBrowse.SelectedPath <> "" Then
            Me.dlgSave2.FileName = ""
            Me.dlgSave2.ShowDialog
            If Me.dlgSave2.FileName <> "" Then
                Call Me.AddToLog("La construction du Service Pack a commenc�...", eLogType.Information, True)
                VpOutFull = New StreamWriter(Me.dlgSave2.FileName)
                VpOutFullB = New BinaryWriter(VpOutFull.BaseStream)
                VpOutLog = New StreamWriter(Me.dlgSave2.FileName.Replace(".dat", ".log"))
                VpDirIn = New DirectoryInfo(Me.dlgBrowse.SelectedPath)
                Me.prgAvance.Style = ProgressBarStyle.Blocks
                Me.prgAvance.Maximum = VpDirIn.GetFiles().Length
                Me.prgAvance.Value = 0
                For Each VpFile As FileInfo In VpDirIn.GetFiles
                    Application.DoEvents
                    VpOutLog.WriteLine(VpFile.Name + "#" + VpCumul.ToString + "#" + (VpCumul + VpFile.Length - 1).ToString)
                    VpCumul = VpCumul + VpFile.Length
                    VpCurIn = New StreamReader(VpFile.FullName)
                    VpCurInB = New BinaryReader(VpCurIn.BaseStream)
                    VpOutFullB.Write(VpCurInB.ReadBytes(VpFile.Length))
                    VpCurIn.Close
                    Me.prgAvance.Increment(1)
                    If Me.btCancel.Tag Then Exit For
                Next VpFile
                VpOutLog.Flush
                VpOutLog.Close
                VpOutFull.Flush
                VpOutFull.Close
            End If
            If Me.btCancel.Tag Then
                Call Me.AddToLog("La construction du Service Pack a �t� annul�e.", eLogType.Warning, , True)
            Else
                Call Me.AddToLog("La construction du Service Pack est termin�e.", eLogType.Information, , True)
            End If
        End If
    End Sub
    Private Sub RevertSP
    '-----------------------------------------------------
    'Reconstruit la base des images dans un �tat ant�rieur
    '-----------------------------------------------------
    Dim VpOut As FileStream
    Dim VpOutB As BinaryWriter
    Dim VpIn As StreamReader
    Dim VpInB As BinaryReader
    Dim VpSize As Integer
        Me.dlgOpen3.FileName = ""
        Me.dlgOpen3.ShowDialog
        If Me.dlgOpen3.FileName <> "" Then
            Me.dlgSave2.FileName = ""
            Me.dlgSave2.ShowDialog
            If Me.dlgSave2.FileName <> "" Then
                Call Me.AddToLog("La reconstruction des images dans un �tat ant�rieur a commenc�...", eLogType.Information, True)
                VpSize = CInt(Val(InputBox("Taille du fichier de sortie ?")))
                If VpSize <> 0 Then
                    VpOut = New FileStream(Me.dlgSave2.FileName, FileMode.OpenOrCreate)
                    VpOutB = New BinaryWriter(VpOut)
                    VpIn = New StreamReader(Me.dlgOpen3.FileName)
                    VpInB = New BinaryReader(VpIn.BaseStream)
                    VpOutB.Seek(0, SeekOrigin.Begin)
                    VpOutB.Write(VpInB.ReadBytes(VpSize))
                    VpIn.Close
                    VpOutB.Flush
                    VpOutB.Close
                    Call Me.AddToLog("La reconstruction des images dans un �tat ant�rieur est termin�e.", eLogType.Information, , True)
                Else
                    Call Me.AddToLog("La reconstruction des images dans un �tat ant�rieur a �t� annul�e.", eLogType.Warning, , True)
                End If
            End If
        End If
    End Sub
    Private Sub FindHoles
    Dim VpEncNbrs() As Long
    Dim VpMin As Long
    Dim VpMax As Long
    Dim VpBestFit As Long
    Dim VpBestFitDelta As Long = Long.MaxValue
    Dim VpSerie As String = InputBox("Entrer le code de l'�dition")
        If VpSerie.Length = 2 Then
            VmDBCommand.CommandText = "Select Min(EncNbr) From Card Where UCase(Series) = '" + VpSerie.ToUpper + "';"
            VpMin = VmDBCommand.ExecuteScalar
            VmDBCommand.CommandText = "Select Max(EncNbr) From Card Where UCase(Series) = '" + VpSerie.ToUpper + "';"
            VpMax = VmDBCommand.ExecuteScalar
            Call Me.AddToLog("L'intervalle pour cette �dition est [" + VpMin.ToString + ";" + VpMax.ToString + "]", eLogType.Information, , True)
            VmDBCommand.CommandText = "Select EncNbr From Card Order By EncNbr;"
            VmDBReader = VmDBCommand.ExecuteReader
            ReDim VpEncNbrs(0)
            With VmDBReader
                While .Read
                    VpEncNbrs(VpEncNbrs.Length - 1) = .GetValue(0)
                    ReDim Preserve VpEncNbrs(0 To VpEncNbrs.Length)
                End While
                .Close
            End With
            ReDim Preserve VpEncNbrs(0 To VpEncNbrs.Length - 2)
            For VpI As Integer = 1 To VpEncNbrs.Length - 1
                If VpEncNbrs(VpI) > 1 + VpEncNbrs(VpI - 1) Then
                    'Debug.Print("Emplacement dispo. entre : " + VpEncNbrs(VpI - 1).ToString + " et " + VpEncNbrs(VpI).ToString)
                    If Math.Abs(VpEncNbrs(VpI - 1) - VpMin) < VpBestFitDelta Then
                        VpBestFitDelta = Math.Abs(VpEncNbrs(VpI - 1) - VpMin)
                        VpBestFit = VpEncNbrs(VpI - 1)
                    End If
                    If Math.Abs(VpEncNbrs(VpI) - VpMin) < VpBestFitDelta Then
                        VpBestFitDelta = Math.Abs(VpEncNbrs(VpI - 1) - VpMin)
                        VpBestFit = VpEncNbrs(VpI)
                    End If
                    If Math.Abs(VpEncNbrs(VpI - 1) - VpMax) < VpBestFitDelta Then
                        VpBestFitDelta = Math.Abs(VpEncNbrs(VpI - 1) - VpMin)
                        VpBestFit = VpEncNbrs(VpI - 1)
                    End If
                    If Math.Abs(VpEncNbrs(VpI) - VpMax) < VpBestFitDelta Then
                        VpBestFitDelta = Math.Abs(VpEncNbrs(VpI - 1) - VpMin)
                        VpBestFit = VpEncNbrs(VpI)
                    End If
                End If
            Next VpI
            Call Me.AddToLog("Emplacement le plus adapt� : " + VpBestFit.ToString, eLogType.Information, , True)
            VmDBCommand.CommandText = "Select Series.SeriesNM From Series Inner Join Card On Series.SeriesCD = Card.Series Where Card.EncNbr = " + VpBestFit.ToString + ";"
            Call Me.AddToLog("Edition normalement � cet endroit : " + VmDBCommand.ExecuteScalar.ToString, eLogType.Information, , True)
        End If
    End Sub
    Sub MnuExitClick(sender As Object, e As EventArgs)
        Application.Exit
    End Sub
    Sub MnuDBOpenClick(sender As Object, e As EventArgs)
        Me.dlgOpen.FileName = ""
        Me.dlgOpen.ShowDialog
        If Me.dlgOpen.FileName <> "" Then
            Try
                VmDB = New OleDbConnection(CmStrConn + Me.dlgOpen.FileName)
                VmDB.Open
                VmDBCommand.Connection = VmDB
                VmDBCommand.CommandType = CommandType.Text
                Call Me.AddToLog("Base de donn�es ouverte avec succ�s.", eLogType.Information)
            Catch
                Call Me.AddToLog("Impossible d'ouvrir la base de donn�es...", eLogType.Warning)
            End Try
        End If
    End Sub
    Sub MainFormFormClosing(sender As Object, e As FormClosingEventArgs)
        If Me.btCancel.Enabled = True Then
            MessageBox.Show("Une op�ration est en cours..." + vbCrLf + "Vous devez d'abord l'annuler avant de quitter l'application.", "Probl�me", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1)
            e.Cancel = True
        ElseIf Not VmDB Is Nothing Then
            VmDB.Close
            VmDB.Dispose
        End If
    End Sub
    Sub BtCancelClick(sender As Object, e As EventArgs)
        If MessageBox.Show("Voulez-vous vraiment annuler l'op�ration en cours ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = System.Windows.Forms.DialogResult.Yes Then
            Me.btCancel.Tag = True
        End If
    End Sub
    Sub MnuAboutClick(sender As Object, e As EventArgs)
    Dim VpAbout As New About
        VpAbout.ShowDialog
    End Sub
    Sub MnuPricesUpdateClick(sender As Object, e As EventArgs)
        If Not VmDB Is Nothing Then
            Call Me.UpdatePrices(True)
        End If
    End Sub
    Sub MnuPricesUpdateListeClick(sender As Object, e As EventArgs)
        If Not VmDB Is Nothing Then
            Call Me.UpdatePrices(False)
        End If
    End Sub
    Sub MnuPricesHistoryAddClick(sender As Object, e As EventArgs)
        If Not VmDB Is Nothing Then
            Me.dlgOpen2.FileName = ""
            Me.dlgOpen2.ShowDialog
            If Me.dlgOpen2.FileName <> "" Then
                Call Me.FillPricesHistory(Me.dlgOpen2.FileName)
            End If
        End If
    End Sub
    Sub MnuPricesHistoryRebuildClick(sender As Object, e As EventArgs)
    Dim VpDir As DirectoryInfo
    Dim VpFiles As FileSystemInfo()
        If Not VmDB Is Nothing Then
            Me.dlgBrowse.SelectedPath = ""
            Me.dlgBrowse.ShowDialog
            If Me.dlgBrowse.SelectedPath <> "" Then
                VmDBCommand.CommandText = "Delete * From PricesHistory;"
                VmDBCommand.ExecuteNonQuery
                VpDir = New DirectoryInfo(Me.dlgBrowse.SelectedPath)
                VpFiles = VpDir.GetFileSystemInfos("Prices (*.txt")
                Array.Sort(VpFiles, New clsPriceFilesComparer)
                For Each VpFile As FileSystemInfo In VpFiles
                    Call Me.FillPricesHistory(VpFile.FullName)
                Next VpFile
            End If
        End If
    End Sub
    Sub MnuPicturesFixClick(sender As Object, e As EventArgs)
        If Not VmDB Is Nothing Then
            Call Me.FixPictures
        End If
    End Sub
    Sub MnuPicturesDeltaClick(sender As Object, e As EventArgs)
        If Not VmDB Is Nothing Then
            Call Me.ExtractModIm
        End If
    End Sub
    Sub MnuCardsTradTxtClick(sender As Object, e As EventArgs)
        Call Me.TranslateTexts
    End Sub
    Sub MnuCardsExtractAllClick(sender As Object, e As EventArgs)
        If Not VmDB Is Nothing Then
            Call Me.ExtractCards("Select Distinct Title From Card Order By Title Asc;")
        End If
    End Sub
    Sub MnuCardsExtractDiffClick(sender As Object, e As EventArgs)
        If Not VmDB Is Nothing Then
            'Select Distinct Card.Title From Card Where Not Exists (Select CardPictures.Title From CardPictures Where CardPictures.Title = Replace(Replace(Replace(Replace(Card.Title, ':', ''), '/', ''), '""', ''), '?', '')) Order By Card.Title Asc;
            Call Me.ExtractCards("Select Distinct Card.Title From Card Where Not Exists (Select CardPictures.Title From CardPictures Where CardPictures.Title = Card.Title) Order By Card.Title Asc;")
            Call Me.AddToLog("Utiliser la requ�te Access pour �viter les doublons...", eLogType.Warning)
        End If
    End Sub
    Sub MnuCardsExtractDiff2Click(sender As Object, e As EventArgs)
        If Not VmDB Is Nothing Then
            Call Me.ExtractCards("Select Distinct Card.Title From Card Inner Join CardFR On Card.EncNbr = CardFR.EncNbr Where Card.Title = CardFR.TitleFR And Not Card.Series In ('UG', 'UH', 'V1', 'V2', 'V3', 'D1', 'D2', 'D3','D4', 'D5', 'D6', 'TD', 'CH', 'AL', 'BE', 'UN', 'AN', 'AQ', 'LE', 'DK', 'FE');")
        End If
    End Sub
    Sub MnuCardsExtractDiff3Click(sender As Object, e As EventArgs)
        If Not VmDB Is Nothing Then
            Call Me.ExtractCards("Select Distinct Card.Title From Card Inner Join TextesFR On Card.Title = TextesFR.CardName Where Card.CardText = TextesFR.TexteFR And Trim(Card.CardText) <> """" And Card.CardText <> Null And Card.Title <> 'Forest' And Card.Title <> 'Plains' And Card.Title <> 'Mountain' And Card.Title <> 'Swamp' And Card.Title <> 'Island';")
        End If
    End Sub
    Sub MnuCardsExtractDiff4Click(sender As Object, e As EventArgs)
        If Not VmDB Is Nothing Then
            Call Me.ExtractCards("Select Distinct Card.Title From Card Where Not Exists (Select Autorisations.Title From Autorisations Where Card.Title = Autorisations.Title) And Card.Type <> 'K' And Card.SpecialDoubleCard = False Order By Card.Title Asc;")
        End If
    End Sub
    Sub MnuCardsExtractDiff5Click(sender As Object, e As EventArgs)
        If Not VmDB Is Nothing Then
            Call Me.ExtractCardsPricesAborted
        End If
    End Sub
    Sub MnuCardsRulingsFilterClick(sender As Object, e As EventArgs)
        Call Me.FilterRulings
    End Sub
    Sub MnuCardsExtractMultiverseIdClick(sender As Object, e As EventArgs)
        If Not VmDB Is Nothing Then
            Call Me.ExtractCardsMultiverseId
        End If
    End Sub
    Sub MnuCardsAutClick(sender As Object, e As EventArgs)
        If Not VmDB Is Nothing Then
            Call Me.UpdateAutorisations(True)
        End If
    End Sub
    Sub MnuCardsAutListeClick(sender As Object, e As EventArgs)
        If Not VmDB Is Nothing Then
            Call Me.UpdateAutorisations(False)
        End If
    End Sub
    Sub MnuCardsAutMergeClick(sender As Object, e As EventArgs)
        Call Me.MergeAutorisations
    End Sub
    Sub WbMVDocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs)
        VmIsComplete = True
    End Sub
    Sub MnuSeriesGenR14Click(sender As Object, e As EventArgs)
        If Not VmDB Is Nothing Then
            Call Me.BuildHeaders(True)
        End If
    End Sub
    Sub MnuSeriesGenR16Click(sender As Object, e As EventArgs)
        If Not VmDB Is Nothing Then
            Call Me.BuildHeaders(False)
        End If
    End Sub
    Sub MnuPicturesSymbolsClick(sender As Object, e As EventArgs)
        If Not VmDB Is Nothing Then
            Call Me.DownloadSymbolsOrThumbs("https://www.mtgpics.com/graph/sets/symbols/#id#-c.png", "symboles", "le symbole")
        End If
    End Sub
    Sub MnuPicturesThumbsClick(sender As Object, e As EventArgs)
        If Not VmDB Is Nothing Then
            Call Me.DownloadSymbolsOrThumbs("https://www.mtgpics.com/graph/sets/logos/#id#.png", "miniatures", "la miniature")
        End If
    End Sub
    Sub MnuPicturesUpdateClick(sender As Object, e As EventArgs)
        Call Me.DownloadPictures
    End Sub
    Sub MnuPicturesRemoveClick(sender As Object, e As EventArgs)
        Call Me.RemoveDoublePictures
    End Sub
    Sub MnuExtractTextsClick(sender As Object, e As EventArgs)
        If Not VmDB Is Nothing Then
            Call Me.ExtractTexts
        End If
    End Sub
    Sub MnuBuildPatchClick(sender As Object, e As EventArgs)
        If Not VmDB Is Nothing Then
            If MessageBox.Show("Cette op�ration va d�truire des tables dans la base en cours..." + vbCrLf + "Continuer ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = System.Windows.Forms.DialogResult.Yes Then
                Call Me.BuildPatch
            End If
        End If
    End Sub
    Sub MnuGetShippingCostsClick(sender As Object, e As EventArgs)
        Call Me.GetShippingCosts
    End Sub
    Sub MnuDBReadyClick(sender As Object, e As EventArgs)
        If Not VmDB Is Nothing Then
            If MessageBox.Show("Cette op�ration va d�truire des champs dans la base en cours..." + vbCrLf + "Continuer ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = System.Windows.Forms.DialogResult.Yes Then
                Call Me.ReadyForRelease
            End If
        End If
    End Sub
    Sub MnuINIReadyClick(sender As Object, e As EventArgs)
        Call Me.ReadyINI
    End Sub
    Sub MnuFilterTitlesClick(sender As Object, e As EventArgs)
        Call Me.ExtractTitles
    End Sub
    Sub MnuPicturesNewSPClick(sender As Object, e As EventArgs)
        Call Me.BuildSP
    End Sub
    Sub MnuPicturesRevertSPClick(sender As Object, e As EventArgs)
        Call Me.RevertSP
    End Sub
    Sub MnuBuildTitlesClick(sender As Object, e As EventArgs)
        If Not VmDB Is Nothing Then
            Call Me.BuildTitles
        End If
    End Sub
    Sub MnuCheckTradClick(sender As Object, e As EventArgs)
        Call Me.CheckTexts
    End Sub
    Sub MnuCompareTradClick(sender As Object, e As EventArgs)
        If Not VmDB Is Nothing Then
            Call Me.CompareTexts
        End If
    End Sub
    Sub MnuListSubtypesClick(sender As Object, e As EventArgs)
        If Not VmDB Is Nothing Then
            Call Me.ListSubtypes
        End If
    End Sub
    Sub MnuBuildDoubleClick(sender As Object, e As EventArgs)
        If Not VmDB Is Nothing Then
            Call Me.BuildDouble
        End If
    End Sub
    Sub MnuFindHolesClick(sender As Object, e As EventArgs)
        If Not VmDB Is Nothing Then
            Call Me.FindHoles
        End If
    End Sub
    Sub MnuFixTxtVOClick(sender As Object, e As EventArgs)
        If Not VmDB Is Nothing Then
            Call Me.FixTxtVO
        End If
    End Sub
    Sub MnuCardReplaceTitleClick(sender As Object, e As EventArgs)
        If Not VmDB Is Nothing Then
            Call Me.ReplaceTitle
        End If
    End Sub
    Sub MnuSeriesSpoilersClick(sender As Object, e As EventArgs)
    Dim VpCode As String
        VpCode = InputBox("Quel est le code de la nouvelle �dition � ajouter ?", "R�cup�ration des spoilers", "(code)")
        If VpCode <> "" Then
            Me.dlgBrowse.SelectedPath = ""
            Me.dlgBrowse.ShowDialog
            If Me.dlgBrowse.SelectedPath <> "" Then
                Call Me.DownloadSpoilers(VpCode)
            End If
        End If
    End Sub
    Public Class clsPriceFilesComparer
        Implements IComparer(Of FileSystemInfo)
        Public Function Compare(ByVal x As FileSystemInfo, ByVal y As FileSystemInfo) As Integer Implements IComparer(Of FileSystemInfo).Compare
            Return x.LastWriteTime.CompareTo(y.LastWriteTime)
        End Function
    End Class
    Public Class clsPriceInfos
        Public Serie As String
        Public Condition As String
        Public Language As String
        Public Price As String
        Public Class clsPriceInfosComparer
            Implements IComparer(Of clsPriceInfos)
            Private Function GetPoints(ByVal x As clsPriceInfos) As Integer
                If x.Condition = "NM/MT" And x.Language = "VO" Then
                    Return 11
                ElseIf x.Condition = "NM/MT" And x.Language = "VF" Then
                    Return 10
                ElseIf x.Condition = "NM/MT" Then
                    Return 9
                ElseIf x.Condition = "NM" And x.Language = "VO" Then
                    Return 8
                ElseIf x.Condition = "NM" And x.Language = "VF" Then
                    Return 7
                ElseIf x.Condition = "NM" Then
                    Return 6
                ElseIf x.Condition = "EX" And x.Language = "VO" Then
                    Return 5
                ElseIf x.Condition = "EX" And x.Language = "VF" Then
                    Return 4
                ElseIf x.Condition = "EX" Then
                    Return 3
                ElseIf x.Language = "VO" Then
                    Return 2
                ElseIf x.Language = "VF" Then
                    Return 1
                Else
                    Return 0
                End If
            End Function
            Public Function Compare(ByVal x As clsPriceInfos, ByVal y As clsPriceInfos) As Integer Implements IComparer(Of clsPriceInfos).Compare
                'Ordre de pr�f�rence :
                '1) NM/MT VO
                '2) NM/MT VF
                '3) NM/MT
                '4) NM VO
                '5) NM VF
                '6) NM
                '7) EX VO
                '8) EX VF
                '9) EX
                '10) VO
                '11) VF
                Return GetPoints(y).CompareTo(GetPoints(x))
            End Function
        End Class
    End Class
    <Serializable> _
    Public Class clsFullInfos
        Public special As Boolean
        Public name As String
        Public code As String
        Public gathererCode As String
        Public oldCode As String
        Public magicCardsInfoCode As String
        Public releaseDate As String
        Public border As String
        Public type As String
        Public block As String
        Public onlineOnly As Boolean
        Public translations As Dictionary(Of String, String)
        Public cards As List(Of clsFullCardInfos)
        Public Class clsFullCardInfos
            Public linkedTo As clsFullCardInfos
            Public linkedFrom As clsFullCardInfos
            Public id As String
            Public layout As String
            Public name As String
            Public names As List(Of String)
            Public manaCost As String
            Public cmc As Single
            Public colors As List(Of String)
            Public colorIdentity As List(Of String)
            Public type As String
            Public supertypes As List(Of String)
            Public types As List(Of String)
            Public subtypes As List(Of String)
            Public rarity As String
            Public [text] As String
            Public artist As String
            Public number As String
            Public power As String
            Public toughness As String
            Public loyalty As Object
            Public multiverseid As Long
            Public variations As List(Of String)
            Public imageName As String
            Public watermark As String
            Public border As String
            Public timeshifted As Boolean
            Public hand As Integer
            Public life As Integer
            Public reserved As Boolean
            Public releaseDate As String
            Public starter As Boolean
            Public rulings As List(Of clsRulingsInfos)
            Public foreignData As List(Of clsForeignInfos)
            Public printings As List(Of String)
            Public originalText As String
            Public originalType As String
            Public legalities As List(Of clsLegalityInfos)
            Public source As String
            Public uuid As String
            Public Class clsRulingsInfos
                Public [date] As String
                Public [text] As String
            End Class
            Public Class clsForeignInfos
                Public language As String
                Public name As String
                Public multiverseid As Long
            End Class
            Public Class clsLegalityInfos
                Public format As String
                Public legality As String
            End Class
            Public Function getForeignName(language As String) As String
                For Each foreign As clsForeignInfos In foreignData
                    If foreign.language = language Then
                        Return foreign.name
                    End If
                Next foreign
                Return name
            End Function
            Public Function getMergedColors As List(Of String)
            Dim mergedColors As List(Of String)
                If linkedTo Is Nothing Then
                    Return colors
                Else
                    mergedColors = New List(Of String)
                    For Each color As String In colors
                        mergedColors.Add(color)
                    Next color
                    For Each color As String In linkedTo.colors
                        If Not mergedColors.Contains(color) Then
                            mergedColors.Add(color)
                        End If
                    Next color
                    Return mergedColors
                End If
            End Function
            Public Function getCost As String
            Dim subCosts() As String
            Dim cost As String
                cost = ""
                If manaCost IsNot Nothing Then
                    subCosts = manaCost.Split("{")
                    If subCosts.Length > 1 Then
                        For i As Integer = 1 To subCosts.Length - 1
                            subCosts(i) = subCosts(i).Replace("}", "")
                            If subCosts(i).Contains("/") Then
                                cost += "(" + subCosts(i) + ")"
                            Else
                                cost += subCosts(i)
                            End If
                        Next i
                    Else
                        cost = manaCost.Replace("{", "").Replace("}", "")
                    End If
                End If
                Return cost
            End Function
            Public Function getRules As String
                Return If([text] Is Nothing, "", [text].Replace("\n", vbCrLf))
            End Function
        End Class
        Public Class clsFullCardInfosComparer
            Implements IComparer(Of clsFullCardInfos)
            Public Function Compare(ByVal x As clsFullCardInfos, ByVal y As clsFullCardInfos) As Integer Implements IComparer(Of clsFullCardInfos).Compare
                If x.name = y.name Then
                    If x.foreignData Is Nothing And y.foreignData IsNot Nothing Then
                        Return 1
                    ElseIf x.foreignData IsNot Nothing And y.foreignData Is Nothing Then
                        Return -1
                    ElseIf x.foreignData IsNot Nothing And y.foreignData IsNot Nothing Then
                        Return y.foreignData.Count.CompareTo(x.foreignData.Count)
                    Else
                        Return x.name.CompareTo(y.name)
                    End If
                Else
                    Return x.name.CompareTo(y.name)
                End If
            End Function
        End Class
    End Class
End Class
