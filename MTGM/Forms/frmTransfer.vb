Public Partial Class frmTransfer
    Private VmCardName As String
    Private VmOwner As MainForm
    Private VmSource As String
    Private VmSource2 As String
    Private VmTransfertResult As clsTransferResult
    Public Sub New(VpOwner As MainForm, VpCardName As String, VpSource As String, VpSource2 As String, VpTransfertResult As clsTransferResult)
        Call Me.InitializeComponent
        VmCardName = VpCardName
        VmSource = VpSource
        VmSource2 = VpSource2
        VmOwner = VpOwner
        VmTransfertResult = VpTransfertResult
        Call Me.GetEditionsDispo(Me.cboSerie)
        If VpTransfertResult.TransfertType = clsTransferResult.EgTransfertType.Swap Then
            Call Me.GetEditionsDispo(Me.cboSerie2, True)
            Me.cboSerie2.Text = Me.cboSerie2.Items(0)
            For Each VpEdition As String In Me.cboSerie2.Items
                If Me.cboSerie.Items(0).StartsWith(VpEdition) Then
                    Me.cboSerie2.SelectedItem = VpEdition
                End If
            Next VpEdition
            Me.grpDest.Visible = True
        Else
            Me.grpDest.Visible = False
            Me.cmdCancel.Top = Me.cmdCancel.Top - Me.grpDest.Height
            Me.cmdOK.Top = Me.cmdOK.Top - Me.grpDest.Height
            Me.Height = Me.Height - Me.grpDest.Height
        End If
        Me.chkReserve.Visible = ( VpSource = mdlConstGlob.CgSDecks )
        Me.chkReserve.Checked = VpTransfertResult.ReserveFrom
        Me.cboSerie.Text = Me.cboSerie.Items(0)
        Me.sldQuant.Minimum = 1
        Me.sldQuant.Value = 1
        Me.lblQuant.Text = Me.sldQuant.Value.ToString
        Me.lblCard.Text = VpCardName
    End Sub
    Private Sub GetEditionsDispo(VpCboSerie As ComboBox, Optional SwapTo As Boolean = False)
    '--------------------------------------------------------------------------------------
    'Charge la liste des �ditions disponibles pour la carte courante sur la source courante
    '--------------------------------------------------------------------------------------
    Dim VpSQL As String
        If SwapTo Then
            VpSQL = "Select SeriesNM From Card Inner Join Series On Card.Series = Series.SeriesCD Where Card.Title = '" + VmCardName.Replace("'", "''") + "'"
        ElseIf VmSource = mdlConstGlob.CgSFromSearch Then
            VpSQL = "Select SeriesNM From ((" + VmSource2 + " Inner Join Card On " + VmSource + ".EncNbr = Card.EncNbr) Inner Join Series On Card.Series = Series.SeriesCD) Where Card.Title = '" + VmCardName.Replace("'", "''") + "' And "
            VpSQL = VpSQL + VmOwner.Restriction
        Else
            VpSQL = "Select SeriesNM, Foil From ((" + VmSource + " Inner Join Card On " + VmSource + ".EncNbr = Card.EncNbr) Inner Join Series On Card.Series = Series.SeriesCD) Where " + If(VmSource = mdlConstGlob.CgSDecks, " Reserve = " + VmTransfertResult.ReserveFrom.ToString + " And ", "") + "Card.Title = '" + VmCardName.Replace("'", "''") + "' And "
            VpSQL = VpSQL + VmOwner.Restriction
        End If
        VpSQL = mdlToolbox.TrimQuery(VpSQL)
        VgDBCommand.CommandText = VpSQL
        VgDBReader = VgDBCommand.ExecuteReader
        With VgDBReader
            If VmSource = mdlConstGlob.CgSFromSearch Or SwapTo Then
                While .Read
                    VpCboSerie.Items.Add(.GetString(0))
                End While
            Else
                While .Read
                    VpCboSerie.Items.Add(.GetString(0) + If(.GetBoolean(1), mdlConstGlob.CgFoil2, ""))
                End While
            End If
            .Close
        End With
    End Sub
    Public Shared Function NeedsPrecision(VpOwner As MainForm, VpCardName As String, VpSource As String, VpSource2 As String, VpTransfertType As clsTransferResult.EgTransfertType, ByRef VpFoil As Boolean) As Boolean
    '----------------------------------------------------------------------------------
    'V�rifie si l'op�ration demand�e n�cessit� des pr�cisions (�dition, foil, quantit�)
    '----------------------------------------------------------------------------------
    Dim VpSQL As String
    Dim VpRet As Boolean
        'Si on fait un �change d'�dition, on a syst�matiquement besoin d'afficher le formulaire de transfert
        If VpTransfertType = clsTransferResult.EgTransfertType.Swap Then
            Return True
        End If
        'Cas 1 : plusieurs �ditions disponibles ou bien m�me �dition mais avec foil(s) et non foil(s)
        VpSQL = "Select Count(*) From ((" + VpSource2 + " Inner Join Card On " + VpSource + ".EncNbr = Card.EncNbr) Inner Join Series On Card.Series = Series.SeriesCD) Where Card.Title = '" + VpCardName.Replace("'", "''") + "' And "
        VpSQL = VpSQL + VpOwner.Restriction
        VpSQL = mdlToolbox.TrimQuery(VpSQL)
        VgDBCommand.CommandText = VpSQL
        VpRet = ( VgDBCommand.ExecuteScalar > 1 )
        If Not VpRet Then   's'il n'y a pas d'ambiguit�, on veut quand m�me savoir si la carte qu'on veut transf�rer est foil ou non
            If VpOwner.IsInAdvSearch Then
                VpFoil = False
            Else
                VpSQL = "Select Foil From (" + VpSource2 + " Inner Join Card On " + VpSource + ".EncNbr = Card.EncNbr) Where Card.Title = '" + VpCardName.Replace("'", "''") + "' And "
                VpSQL = VpSQL + VpOwner.Restriction
                VpSQL = mdlToolbox.TrimQuery(VpSQL)
                VgDBCommand.CommandText = VpSQL
                VpFoil = VgDBCommand.ExecuteScalar
            End If
        End If
        'Si c'est une copie que l'on fait, on n'a pas besoin de savoir combien d'items il y a (ie. pas besoin d'�valuer le cas 2 ci-dessous), cela d�pend si l'utilisateur a choisi dans les options de pouvoir r�gler manuellement le nombre de cartes � copier
        If VpTransfertType = clsTransferResult.EgTransfertType.Copy Then
            Return If(VgOptions.VgSettings.CopyRange > 1, True, VpRet)
        End If
        'Cas 2 : Si une seule �dition mais plusieurs items
        VpSQL = "Select Items From ((" + VpSource + " Inner Join Card On " + VpSource + ".EncNbr = Card.EncNbr) Inner Join Series On Card.Series = Series.SeriesCD) Where Card.Title = '" + VpCardName.Replace("'", "''") + "' And "
        VpSQL = VpSQL + VpOwner.Restriction
        VpSQL = mdlToolbox.TrimQuery(VpSQL)
        VgDBCommand.CommandText = VpSQL
        Return VpRet Or ( VgDBCommand.ExecuteScalar > 1 )
    End Function
    Public Shared Function GetMatchingEdition(VpOwner As MainForm, VpCardName As String, VpSource As String, VpSource2 As String) As String
    '-------------------------------------------------------------------------------------------------
    'Dans le cas o� NeedsPrecision a retourn� faux, trouve l'�dition correspondant � la carte courante
    '-------------------------------------------------------------------------------------------------
    Dim VpSQL As String
        VpSQL = "Select Card.Series From ((" + VpSource2 + " Inner Join Card On " + VpSource + ".EncNbr = Card.EncNbr) Inner Join Series On Card.Series = Series.SeriesCD) Where Card.Title = '" + VpCardName.Replace("'", "''") + "' And "
        VpSQL = VpSQL + VpOwner.Restriction
        VpSQL = mdlToolbox.TrimQuery(VpSQL)
        VgDBCommand.CommandText = VpSQL
        Return VgDBCOmmand.ExecuteScalar
    End Function
    Public Shared Sub CommitAction(VpTransfertResult As clsTransferResult)
    '-----------------------------------------------------------------------------------
    'Modifie les enregistrements de la BDD pour assurer le transfert d�crit en param�tre
    '-----------------------------------------------------------------------------------
    Dim VpNItemsAtSource As Integer
    Dim VpNItemsAtDest As Integer
        With VpTransfertResult
            'Dans le cas d'un d�placement ou d'une suppression ou d'un swap : suppression � la source
            If .TransfertType = clsTransferResult.EgTransfertType.Move Or .TransfertType = clsTransferResult.EgTransfertType.Deletion Or .TransfertType = clsTransferResult.EgTransfertType.Swap Then
                'Nombre d'items d�j� pr�sents � la source
                VgDBCommand.CommandText = "Select Items From " + .SFrom + " Where Foil = " + .FoilFrom.ToString + " And EncNbr = " + .EncNbrFrom.ToString + If(.SFrom = mdlConstGlob.CgSDecks, " And GameID = " + mdlToolbox.GetDeckIdFromName(.TFrom) + " And Reserve = " + .ReserveFrom.ToString + ";", ";")
                VpNItemsAtSource = VgDBCommand.ExecuteScalar
                '-NCartes � la source
                If VpNItemsAtSource - .NCartes > 0 Then
                    VgDBCommand.CommandText = "Update " + .SFrom + " Set Items = " + (VpNItemsAtSource - .NCartes).ToString + " Where Foil = " + .FoilFrom.ToString + " And EncNbr = " + .EncNbrFrom.ToString + If(.SFrom = mdlConstGlob.CgSDecks, " And GameID = " + mdlToolbox.GetDeckIdFromName(.TFrom) + " And Reserve = " + .ReserveFrom.ToString + ";", ";")
                    VgDBCommand.ExecuteNonQuery
                Else
                    VgDBCommand.CommandText = "Delete * From " + .SFrom + " Where Foil = " + .FoilFrom.ToString + " And EncNbr = " + .EncNbrFrom.ToString + If(.SFrom = mdlConstGlob.CgSDecks, " And GameID = " + mdlToolbox.GetDeckIdFromName(.TFrom) + " And Reserve = " + .ReserveFrom.ToString + ";", ";")
                    VgDBCommand.ExecuteNonQuery
                End If
            End If
            'Dans le cas d'un d�placement ou d'une copie ou d'un swap : ajout � destination
            If .TransfertType = clsTransferResult.EgTransfertType.Move Or .TransfertType = clsTransferResult.EgTransfertType.Copy Or .TransfertType = clsTransferResult.EgTransfertType.Swap Then
                'Nombre d'items d�j� pr�sents � la destination
                VgDBCommand.CommandText = "Select Items From " + .STo + " Where Foil = " + .FoilTo.ToString + " And EncNbr = " + .EncNbrTo.ToString + If(.STo = mdlConstGlob.CgSDecks, " And GameID = " + mdlToolbox.GetDeckIdFromName(.TTo) + " And Reserve = " + .ReserveTo.ToString + ";", ";")
                VpNItemsAtDest = VgDBCommand.ExecuteScalar
                '+NCartes � la destination
                If VpNItemsAtDest > 0 Then
                    VgDBCommand.CommandText = "Update " + .STo + " Set Items = " + (VpNItemsAtDest + .NCartes).ToString + " Where Foil = " + .FoilTo.ToString + " And EncNbr = " + .EncNbrTo.ToString + If(.STo = mdlConstGlob.CgSDecks, " And GameID = " + mdlToolbox.GetDeckIdFromName(.TTo) + " And Reserve = " + .ReserveTo.ToString + ";", ";")
                    VgDBCommand.ExecuteNonQuery
                Else
                    VgDBCommand.CommandText = "Insert Into " + .STo + " Values (" + If(.STo = mdlConstGlob.CgSDecks, mdlToolbox.GetDeckIdFromName(.TTo) + ", ", "") + .EncNbrTo.ToString + ", " + .NCartes.ToString + ", " + .FoilTo.ToString + If(.STo = mdlConstGlob.CgSDecks, ", " + .ReserveTo.ToString, "") + ");"
                    VgDBCommand.ExecuteNonQuery
                End If
            End If
        End With
    End Sub
    Private Sub ChangeLogo(VpCboSerie As ComboBox, VpPicSerie As PictureBox, Optional ByRef VpEdition As String = "", Optional ByRef VpFoil As Boolean = False)
    '-----------------------------------------
    'Affiche le logo de l'�dition s�lectionn�e
    '-----------------------------------------
    Dim VpKey As Integer
        If VpCboSerie.Text.EndsWith(mdlConstGlob.CgFoil2) Then
            VpEdition = mdlToolbox.GetSerieCodeFromName(VpCboSerie.Text.Replace(mdlConstGlob.CgFoil2, ""))
            VpFoil = True
        Else
            VpEdition = mdlToolbox.GetSerieCodeFromName(VpCboSerie.Text)
            VpFoil = False
        End If
        VpKey = mdlConstGlob.VgImgSeries.Images.IndexOfKey("_e" + VpEdition + CgIconsExt)
        If VpKey <> -1 Then
            VpPicSerie.Image = mdlConstGlob.VgImgSeries.Images(VpKey)
        Else
            VpPicSerie.Image = Nothing
        End If
    End Sub
    Sub CboSerieSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
    '-----------------------------------------------------------
    'R�ajustement des param�tres en fonction de l'�dition source
    '-----------------------------------------------------------
    Dim VpSQL As String
    Dim VpEdition As String = ""
    Dim VpFoil As Boolean = False
        'Logo �dition source
        Call Me.ChangeLogo(Me.cboSerie, Me.picSerie, VpEdition, VpFoil)
        Me.chkFoil.Checked = VpFoil
        'R�ajuste le nombre de cartes disponibles dans l'�dition s�lectionn�e
        If VmTransfertResult.TransfertType = clsTransferResult.EgTransfertType.Copy Then
            Me.sldQuant.Maximum = VgOptions.VgSettings.CopyRange
        Else
            VpSQL = "Select " + VmSource + ".Items From " + VmSource + " Inner Join Card On " + VmSource + ".EncNbr = Card.EncNbr Where " + If(VmSource = mdlConstGlob.CgSDecks, " Reserve = " + VmTransfertResult.ReserveFrom.ToString + " And ", "") + "Card.Title = '" + VmCardName.Replace("'", "''") + "' And Foil = " + VpFoil.ToString + " And Card.Series = '" + VpEdition + "' And "
            VpSQL = VpSQL + VmOwner.Restriction
            VpSQL = mdlToolbox.TrimQuery(VpSQL)
            VgDBCommand.CommandText = VpSQL
            Me.sldQuant.Maximum = CInt(VgDBCommand.ExecuteScalar)
            Me.lblQuant.Text = Me.sldQuant.Value.ToString
        End If
        'Correspondance �ventuelle avec le combobox de destination
        If Me.grpDest.Visible Then
            If Me.cboSerie2.Items.Contains(Me.cboSerie.Text) Then
                Me.cboSerie2.SelectedItem = Me.cboSerie.Text
            End If
        End If
    End Sub
    Sub CboSerie2SelectedIndexChanged(sender As Object, e As EventArgs)
        'Logo �dition destination
        Call Me.ChangeLogo(Me.cboSerie2, Me.picSerie2)
    End Sub
    Sub CmdCancelClick(ByVal sender As Object, ByVal e As EventArgs)
        VmTransfertResult.NCartes = 0
        Me.Close
    End Sub
    Sub CmdOKClick(ByVal sender As Object, ByVal e As EventArgs)
        If Me.cboSerie.Items.Contains(Me.cboSerie.Text) AndAlso (Me.cboSerie2.Items.Contains(Me.cboSerie2.Text) OrElse Me.cboSerie2.Text.Trim = "") Then
            With VmTransfertResult
                .NCartes = Me.sldQuant.Value
                If Me.cboSerie.Text.EndsWith(mdlConstGlob.CgFoil2) Then
                    .IDSerieFrom = mdlToolbox.GetSerieCodeFromName(Me.cboSerie.Text.Replace(mdlConstGlob.CgFoil2, ""))
                    .FoilFrom = True
                Else
                    .IDSerieFrom = mdlToolbox.GetSerieCodeFromName(Me.cboSerie.Text)
                    .FoilFrom = False
                End If
                If .TransfertType = clsTransferResult.EgTransfertType.Swap Then
                    .IDSerieTo = mdlToolbox.GetSerieCodeFromName(Me.cboSerie2.Text)
                    .FoilTo = Me.chkFoil.Checked
                    .ReserveTo = Me.chkReserve.Checked
                Else
                    .IDSerieTo = .IDSerieFrom
                    .FoilTo = .FoilFrom
                    .ReserveTo = .ReserveFrom
                End If
            End With
        End If
        Me.Close
    End Sub
    Sub SldQuantScroll(ByVal sender As Object, ByVal e As EventArgs)
        Me.lblQuant.Text = Me.sldQuant.Value.ToString
    End Sub
End Class
