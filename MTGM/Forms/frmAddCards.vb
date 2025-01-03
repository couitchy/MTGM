Public Partial Class frmAddCards
    #Region "D�clarations"
    Private VmFormMove As Boolean = False           'Formulaire en d�placement
    Private VmMousePos As Point                     'Position initiale de la souris sur la barre de titre
    Private VmCanClose As Boolean = False           'Formulaire peut �tre ferm�
    Private VmChangesCommited As Boolean = False    'Modifications en cours sur ComboBoxes
    Private VmFR As Boolean                         'Vrai si saisie depuis la combobox FR, faux si saisie depuis la combobox EN
    Private VmKeyChange As Boolean = False
    Private VmOwner As MainForm
    #End Region
    #Region "M�thodes"
    Private Sub LoadCombos
    '----------------------------------------------------------------
    'Ajoute aux bo�tes combos les noms de cartes disponibles en VO/VF
    '----------------------------------------------------------------
    Dim VpTitleEN As String
    Dim VpTitleFR As String
        Me.cboTitleFR.Sorted = True
        VgDBCommand.CommandText = "Select Card.Title, CardFR.TitleFR From Card Inner Join CardFR On Card.EncNbr = CardFR.EncNbr Order By Card.Title;"
        VgDBReader = VgDBCommand.ExecuteReader
        With VgDBReader
            While .Read
                VpTitleEN = .GetString(0)
                VpTitleFR = .GetString(1)
                If Not Me.cboTitleEN.Items.Contains(VpTitleEN) Then
                    Me.cboTitleEN.Items.Add(VpTitleEN)
                    Me.cboTitleFR.Items.Add(VpTitleFR)
                ElseIf Not Me.cboTitleFR.Items.Contains(VpTitleFR) Then
                    Me.cboTitleFR.Items.Add(VpTitleFR)
                End If
            End While
            .Close
        End With
    End Sub
    Private Sub ComboChangeManagement(VpField1 As String, VpField2 As String, VpTable1 As String, VpTable2 As String, VpCbo1 As ComboBox, VpCbo2 As ComboBox)
    '-----------------------------------------------------------
    'G�re le changement de la s�lection dans un des comboboxes :
    '- traduction dans l'autre
    '- r�cup�ration du num�ro encyclop�dique
    '- r�cup�ration des �ditions disponibles
    '-----------------------------------------------------------
        If VmChangesCommited Then Exit Sub
        VmChangesCommited = True
        VgDBCommand.CommandText = "Select " + VpField1 + " From " + VpTable1 + " Where EncNbr = " + FindInfos(VpCbo2.Text, VpTable2, VpField2).ToString + ";"
        Try
            VpCbo1.Text = VgDBCommand.ExecuteScalar.ToString
        Catch
            VpCbo1.Text = ""
        End Try
        Me.cboSerie.Items.Clear
        VgDBCommand.CommandText = "Select Card.Series, " + If(VmFR, "Series.SeriesNM_FR", "Series.SeriesNM") + " From Series Inner Join Card On Series.SeriesCD = Card.Series Where Card.Title = '" + Me.cboTitleEN.Text.Replace("'", "''") + "';"
        VgDBReader = VgDBCommand.ExecuteReader
        With VgDBReader
            While .Read
                Me.cboSerie.Items.Add("(" + .GetString(0) + ") " + .GetString(1))
            End While
            .Close
        End With
        If Me.cboSerie.Tag <> "" Then
            If Me.cboSerie.Items.Contains(Me.cboSerie.Tag) Then
                Me.cboSerie.SelectedIndex = Me.cboSerie.Items.IndexOf(Me.cboSerie.Tag)
            Else
                Me.cboSerie.SelectedIndex = 0
            End If
        Else
            Me.cboSerie.SelectedIndex = 0
        End If
        VmChangesCommited = False
    End Sub
    Private Function AdjustEncNbr(VpTitle As String, VpSerie As String) As String
    '------------------------------------------------------------------------------------------
    'Retourne le num�ro encyclop�dique de la carte pass�e en param�tre pour l'�dition sp�cifi�e
    '------------------------------------------------------------------------------------------
    Dim VpO As Object
        VgDBCommand.CommandText = "Select Card.EncNbr From Card Inner Join Series On Card.Series = Series.SeriesCD Where Card.Title = '" + VpTitle.Replace("'", "''") + "' And Series.SeriesCD = '" + VpSerie + "';"
        VpO = VgDBCommand.ExecuteScalar
        If Not VpO Is Nothing Then
            Return VpO.ToString
        Else
            Return Me.lblEncNbr.Text
        End If
    End Function
    Private Function FindDateSerie(VpSerie As String) As String
    '--------------------------------------------------------------
    'Retourne la date d'impression de l'�dition pass�e en param�tre
    '--------------------------------------------------------------
        VgDBCommand.CommandText = "Select Release From Series Where SeriesCD = '" + VpSerie + "';"
        Return CDate(VgDBCommand.ExecuteScalar).Year
    End Function
    Private Function FindQuant(VpEncNbr As String, VpFoil As Boolean, VpReserve As Boolean) As String
    '--------------------------------------------------------------------------------------------------------------
    'Retourne la quantit� enregistr�e d'items dont la r�f�rence est pass�e en param�tre dans la collection courante
    '--------------------------------------------------------------------------------------------------------------
    Dim VpSource As String = If(Me.mnuDropToCollection.Checked, mdlConstGlob.CgSCollection, mdlConstGlob.CgSDecks)
    Dim VpO As Object
    Dim VpStr As String = "0"
        If Not IsNumeric(VpEncNbr) OrElse Me.cboSerie.SelectedIndex < 0 Then Return mdlConstGlob.CgStock
        'Quantit� dans l'�dition courante
        VgDBCommand.CommandText = "Select Items From " + VpSource + " Where EncNbr = " + VpEncNbr + " And Foil = " + VpFoil.ToString + If(Me.mnuDropToCollection.Checked = False, " And Reserve = " + VpReserve.ToString + " And GameID = " + Me.cmdDestination.Tag.ToString + ";", ";")
        VpO = VgDBCommand.ExecuteScalar
        If Not VpO Is Nothing Then
            VpStr = VpO.ToString
        End If
        Me.lblNbItems.Tag = Val(VpStr)
        VpStr = VpStr + " " + Me.cboSerie.Text.Substring(0, 3) + If(VpFoil, " foil) / " , ") / ")
        'Quantit� totale toutes �ditions confondues / foil ou pas / r�serve ou pas
        VgDBCommand.CommandText = "Select Sum(Items) From " + VpSource + " Inner Join Card On Card.EncNbr = " + VpSource + ".EncNbr Where Title = '" + Me.cboTitleEN.Text.Replace("'", "''") + "'" + If(Me.mnuDropToCollection.Checked = False, " And GameID = " + Me.cmdDestination.Tag.ToString + ";", ";")
        VpO = VgDBCommand.ExecuteScalar
        If (Not VpO Is Nothing) AndAlso VpO.ToString <> "" Then
            Return VpStr + VpO.ToString + " (total)"
        Else
            Return VpStr + "0 (total)"
        End If
    End Function
    Private Function FindInfos(VpTitle As String, VpTable As String, VpField As String) As Long
    '-------------------------------------------------------------------------------------------------------------
    'Retourne le num�ro encyclop�dique de la carte sp�cifi�e en param�tre, en plus de l'inscrire sur le formulaire
    '-------------------------------------------------------------------------------------------------------------
        VgDBCommand.CommandText = "Select EncNbr From " + VpTable + " Where " + VpField + " = '" + VpTitle.Replace("'", "''") + "';"
        Try
            Me.lblEncNbr.Text = VgDBCommand.ExecuteScalar.ToString
            Return CLng(Me.lblEncNbr.Text)
        Catch
            Return 0
        End Try
    End Function
    Public Sub New(VpOwner As MainForm)
    Dim VpSource As String
        Call Me.InitializeComponent
        VmOwner = VpOwner
        Me.cboSerie.Tag = ""
        Me.lblNbItems.Tag = 0
        Call Me.LoadCombos  'on le fait dans le constructeur car �a prend une �ternit� si c'est fait dans le Load � cause du rafra�chissement graphique
        If mdlToolbox.GetDeckCount = 0 Then
            Me.cmdDestination.Visible = False
        Else
            'Destinations possibles
            For VpI As Integer = 1 To mdlToolbox.GetDeckCount
                Me.cmnuDestination.Items.Add(mdlToolbox.GetDeckNameFromIndex(VpI), Nothing, AddressOf DropTo)
            Next VpI
        End If
        'Destination par d�faut
        VpSource = VmOwner.GetSelectedSource
        If VpSource <> "" And VpSource <> mdlConstGlob.CgAdvSearch Then
            For Each VpItem As ToolStripMenuItem In Me.cmnuDestination.Items
                If VpItem.Text = VpSource Then
                    VpItem.Checked = True
                    Me.cmdDestination.Tag = mdlToolbox.GetDeckIdFromName(VpSource)
                    Me.lblDest.Text = VpSource
                    Exit For
                End If
            Next VpItem
        Else
            Me.mnuDropToCollection.Checked = True
            Me.lblDest.Text = mdlConstGlob.CgCollection
        End If
        Me.chkReserve.Visible = Not Me.mnuDropToCollection.Checked
    End Sub
    #End Region
    #Region "Ev�nements"
    Sub DropTo(ByVal sender As Object, ByVal e As EventArgs)
        For Each VpItem As ToolStripMenuItem In Me.cmnuDestination.Items
            VpItem.Checked = ( VpItem Is sender )
        Next VpItem
        Me.chkReserve.Visible = Not Me.mnuDropToCollection.Checked
        Me.cmdDestination.Tag = mdlToolbox.GetDeckIdFromName(sender.Text)
        Me.lblDest.Text = sender.Text
        Me.lblNbItems.Text = Me.FindQuant(Me.lblEncNbr.Text, Me.chkFoil.Checked, Me.chkReserve.Checked)
    End Sub
    Sub ChkFoilCheckedChanged(sender As Object, e As EventArgs)
        Me.lblNbItems.Text = Me.FindQuant(Me.lblEncNbr.Text, Me.chkFoil.Checked, Me.chkReserve.Checked)
    End Sub
    Sub ChkReserveCheckedChanged(sender As Object, e As EventArgs)
        Me.lblNbItems.Text = Me.FindQuant(Me.lblEncNbr.Text, Me.chkFoil.Checked, Me.chkReserve.Checked)
    End Sub
    Private Sub CbarAjoutMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
        VmFormMove = True
        VmMousePos = New Point(e.X, e.Y)
    End Sub
    Private Sub CbarAjoutMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs)
        VmFormMove = False
    End Sub
    Sub CbarAjoutMouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
        If VmFormMove Then
            Me.Location = New Point(MousePosition.X - VmMousePos.X, MousePosition.Y - VmMousePos.Y)
        End If
    End Sub
    Sub CboTitleFRSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Call Me.ComboChangeManagement("Title", "TitleFR", "Card", "CardFR", Me.cboTitleEN, Me.cboTitleFR)
        Me.cboTitleFR.Tag = Me.cboTitleFR.Text
    End Sub
    Sub CboTitleENSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Call Me.ComboChangeManagement("TitleFR", "Title", "CardFR", "Card", Me.cboTitleFR, Me.cboTitleEN)
        Me.cboTitleEN.Tag = Me.cboTitleEN.Text
    End Sub
    Sub CboSerieSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
    Dim VpSerieCD As String = Me.cboSerie.Text.Substring(1, 2)
    Dim VpKey As Integer = mdlConstGlob.VgImgSeries.Images.IndexOfKey("_e" + VpSerieCD + CgIconsExt)
        Me.lblYear.Text = Me.FindDateSerie(VpSerieCD)
        Me.lblEncNbr.Text = Me.AdjustEncNbr(Me.cboTitleEN.Text, VpSerieCD)
        If VpKey <> -1 Then
            Me.imgEdition.Image = mdlConstGlob.VgImgSeries.Images(VpKey)
        Else
            Me.imgEdition.Image = Nothing
        End If
        Me.lblNbItems.Text = Me.FindQuant(Me.lblEncNbr.Text, Me.chkFoil.Checked, Me.chkReserve.Checked)
    End Sub
    Sub CmdAddClick(ByVal sender As Object, ByVal e As EventArgs)
        If Val(Me.txtNbItems.Text) <> 0 Then
            Try
                If CInt(Me.lblNbItems.Tag) = 0 Then
                    VgDBCommand.CommandText = "Insert Into " + If(Me.mnuDropToCollection.Checked, mdlConstGlob.CgSCollection, mdlConstGlob.CgSDecks) + " Values (" + If(Not Me.mnuDropToCollection.Checked, Me.cmdDestination.Tag.ToString + ", ", "") + Me.lblEncNbr.Text + ", " + CInt(Me.txtNbItems.Text).ToString + ", " + Me.chkFoil.Checked.ToString + If(Not Me.mnuDropToCollection.Checked, ", " + Me.chkReserve.Checked.ToString, "") + ");"
                    VgDBCommand.ExecuteNonQuery
                ElseIf ( CInt(Me.lblNbItems.Tag) + CInt(Me.txtNbItems.Text) ) <= 0 Then
                    VgDBCommand.CommandText = "Delete * From " + If(Me.mnuDropToCollection.Checked, mdlConstGlob.CgSCollection, mdlConstGlob.CgSDecks) + " Where EncNbr = " + Me.lblEncNbr.Text + " And Foil = " + Me.chkFoil.Checked.ToString + If(Not Me.mnuDropToCollection.Checked, " And GameID = " + Me.cmdDestination.Tag.ToString, "") + ";"
                    VgDBCommand.ExecuteNonQuery
                Else
                    VgDBCommand.CommandText = "Update " + If(Me.mnuDropToCollection.Checked, mdlConstGlob.CgSCollection, mdlConstGlob.CgSDecks) + " Set Items = " + (CInt(Me.lblNbItems.Tag) + CInt(Me.txtNbItems.Text)).ToString + " Where EncNbr = " + Me.lblEncNbr.Text + " And Foil = " + Me.chkFoil.Checked.ToString + If(Not Me.mnuDropToCollection.Checked, " And Reserve = " + Me.chkReserve.Checked.ToString + " And GameID = " + Me.cmdDestination.Tag.ToString, "") + ";"
                    VgDBCommand.ExecuteNonQuery
                End If
                Me.cboSerie.Tag = Me.cboSerie.Text
                Me.cboTitleFR.Text = ""
                Me.cboTitleEN.Text = ""
                Me.cboSerie.Text = ""
                Me.cboSerie.Items.Clear
                Me.lblEncNbr.Text = "ID Encyclop�die"
                Me.lblYear.Text = "Ann�e"
                Me.imgEdition.Image = Nothing
                Me.lblNbItems.Text = mdlConstGlob.CgStock
                Me.lblNbItems.Tag = 0
                Me.txtNbItems.Text = "+1"
                Me.chkFoil.Checked = False
                Me.cboTitleFR.Focus
            Catch
                Call mdlToolbox.ShowWarning("Impossible d'ajouter cette carte dans la base de donn�es." + vbCrLf + "La carte n'est peut-�tre pas (ou incorrectement) r�f�renc�e...")
            End Try
        End If
    End Sub
    Function GetRefText(sender As Object) As String
        If sender.SelectionLength > 0 Then
            Return sender.Text.Replace(sender.SelectedText, "").ToLower
        Else
            Return sender.Text.ToLower
        End If
    End Function
    Sub CboTitleKeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
    Dim VpRef As String = Me.GetRefText(sender)
        If e.KeyCode = Keys.Back And VpRef = "�" Then
            sender.Text = "Ae "
            sender.SelectionStart = 3
            VmKeyChange = True
        ElseIf e.KeyCode = Keys.Back And VpRef.EndsWith("�") And Not VpRef.Replace("oe", "").Length <= VpRef.Length - 2  Then   'lourdingue mais il y a un bug dans .NET : oe et � sont des strings consid�r�es identiques pour un EndsWith par exemple alors que la diff�rence de taille est bien prise en compte (1 contre 2)
            sender.Text = VpRef.Substring(0, VpRef.Length - 1) + "oe "
            sender.SelectionStart = sender.Text.Length
            VmKeyChange = True
        End If
    End Sub
    Sub CboTitleKeyUp(sender As Object, e As KeyEventArgs)
    Dim VpRef As String = Me.GetRefText(sender)
        If e.KeyCode = Keys.Add Then
            sender.Text = sender.Tag
        ElseIf VpRef = "ae" And Not VmKeyChange Then
            sender.Text = "�"
            sender.SelectionStart = 1
        ElseIf sender Is Me.cboTitleFR And VpRef.EndsWith("oe") And VpRef.Replace("oe", "").Length <= VpRef.Length - 2 And Not VmKeyChange Then 'lourdingue mais il y a un bug dans .NET : oe et � sont des strings consid�r�es identiques pour un EndsWith par exemple alors que la diff�rence de taille est bien prise en compte (1 contre 2)
            sender.Text = VpRef.Substring(0, VpRef.Length - 2) + "�"
            sender.SelectionStart = sender.Text.Length
        End If
        VmKeyChange = False
    End Sub
    Sub ScrollStockScroll(sender As Object, e As ScrollEventArgs)
        If e.Type <> ScrollEventType.EndScroll Then
            If e.Type = ScrollEventType.SmallIncrement Then     'attention orientation invers�e : fl�che inf�rieure = incr�ment
                Me.txtNbItems.Text = (Val(Me.txtNbItems.Text) - 1).ToString
            Else
                Me.txtNbItems.Text = (Val(Me.txtNbItems.Text) + 1).ToString
            End If
        End If
    End Sub
    Sub CmdDestinationMouseDown(sender As Object, e As MouseEventArgs)
        Me.cmnuDestination.Show(Me.cmdDestination, e.Location)
    End Sub
    Sub CbarAjoutVisibleChanged(sender As Object, e As EventArgs)
        If VmCanClose AndAlso Not Me.CBarAjout.Visible Then
            Me.Close
        End If
    End Sub
    Sub FrmAddCardsFormClosing(sender As Object, e As FormClosingEventArgs)
        VmOwner.BringToFront
        Me.Hide
        If VgOptions.VgSettings.AutoRefresh Then
            If VmOwner.tvwExplore.SelectedNode IsNot Nothing Then
                Call VmOwner.ReloadWithHistory
            Else
                Call VmOwner.LoadTvw
            End If
        End If
    End Sub
    Sub FrmAddCardsLoad(sender As Object, e As EventArgs)
        VmCanClose = True
    End Sub
    Sub FrmAddCardsActivated(sender As Object, e As EventArgs)
        Me.cboTitleFR.Focus
    End Sub
    Sub CmdCloseClick(sender As Object, e As EventArgs)
        Me.Close
    End Sub
    Sub CboTitleFREnter(sender As Object, e As EventArgs)
        VmFR = True
    End Sub
    Sub CboTitleENEnter(sender As Object, e As EventArgs)
        VmFR = False
    End Sub
    Sub CboSerieLeave(sender As Object, e As EventArgs)
        Me.chkFoil.Enabled = ( Me.cboSerie.Items.Contains(Me.cboSerie.Text) )
    End Sub
    #End Region
End Class
