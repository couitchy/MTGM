Public Partial Class frmDeleteEdition
    Private VmOwner As MainForm
    Public Sub New(VpOwner As MainForm)
        Call Me.InitializeComponent
        VmOwner = VpOwner
    End Sub
    Private Sub GoDelete
    '-------------------------------------------------
    'Suppression d'une édition dans la base de données
    '-------------------------------------------------
    Dim VpCD As String = Me.cboSerie.Text.Substring(1, 2)
        VgDBCommand.CommandText = "Delete CardDouble.* From CardDouble Inner Join Card On CardDouble.EncNbrDownFace = Card.EncNbr Where Card.Series = '" + VpCD + "';"
        VgDBCommand.ExecuteNonQuery
        VgDBCommand.CommandText = "Delete CardDouble.* From CardDouble Inner Join Card On CardDouble.EncNbrTopFace = Card.EncNbr Where Card.Series = '" + VpCD + "';"
        VgDBCommand.ExecuteNonQuery
        VgDBCommand.CommandText = "Delete PricesHistory.* From PricesHistory Inner Join Card On PricesHistory.EncNbr = Card.EncNbr Where Card.Series = '" + VpCD + "';"
        VgDBCommand.ExecuteNonQuery
        VgDBCommand.CommandText = "Delete CardFR.* From CardFR Inner Join Card On CardFR.EncNbr = Card.EncNbr Where Card.Series = '" + VpCD + "';"
        VgDBCommand.ExecuteNonQuery
        VgDBCommand.CommandText = "Delete MyCollection.* From MyCollection Inner Join Card On MyCollection.EncNbr = Card.EncNbr Where Card.Series = '" + VpCD + "';"
        VgDBCommand.ExecuteNonQuery
        VgDBCommand.CommandText = "Delete MyGames.* From MyGames Inner Join Card On MyGames.EncNbr = Card.EncNbr Where Card.Series = '" + VpCD + "';"
        VgDBCommand.ExecuteNonQuery
        VgDBCommand.CommandText = "Delete * From Card Where Series = '" + VpCD + "';"
        VgDBCommand.ExecuteNonQuery
        If Me.chkHeader.Checked Then
            VgDBCommand.CommandText = "Delete * From Series Where SeriesCD = '" + VpCD + "';"
            VgDBCommand.ExecuteNonQuery
            VgDBCommand.CommandText = "Update Spell Set LastPrint = 'XX' Where LastPrint = '" + VpCD + "';"
            VgDBCommand.ExecuteNonQuery
        End If
    End Sub
    Sub FrmDeleteEditionLoad(ByVal sender As Object, ByVal e As EventArgs)
        Call mdlToolbox.LoadEditions(Me.cboSerie)
    End Sub
    Sub CboSerieSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
    Dim VpCD As String = Me.cboSerie.Text.Substring(1, 2)
    Dim VpKey As Integer = mdlConstGlob.VgImgSeries.Images.IndexOfKey("_e" + VpCD + CgIconsExt)
    Dim VpCount As Integer
        'Affiche le logo de l'édition sélectionnée
        If VpKey <> -1 Then
            Me.picSerie.Image = mdlConstGlob.VgImgSeries.Images(VpKey)
        Else
            Me.picSerie.Image = Nothing
        End If
        'Compte le nombre de cartes déjà saisies dans cette édition
        VgDBCommand.CommandText = "Select Count(*) From (Card Inner Join CardFR On Card.EncNbr = CardFR.EncNbr) Inner Join MyCollection On Card.EncNbr = MyCollection.EncNbr Where Card.Series = '" + VpCD + "';"
        VpCount = CInt(VgDBCommand.ExecuteScalar)
        VgDBCommand.CommandText = "Select Count(*) From (Card Inner Join CardFR On Card.EncNbr = CardFR.EncNbr) Inner Join MyGames On Card.EncNbr = MyGames.EncNbr Where Card.Series = '" + VpCD + "';"
        VpCount = VpCount + CInt(VgDBCommand.ExecuteScalar)
        Me.chkCards.Text = "Supprimer aussi les cartes saisies dans cette édition (" + VpCount.ToString + " cartes distinctes concernées)"
    End Sub
    Sub CmdGoClick(ByVal sender As Object, ByVal e As EventArgs)
        If Me.cboSerie.Text.Trim <> "" Then
            Call Me.GoDelete
            VmOwner.LoadTvw
            Call mdlToolbox.ShowInformation("Suppression effectuée avec succès !")
            Call mdlToolbox.LoadEditions(Me.cboSerie)
            Me.picSerie.Image = Nothing
        End If
    End Sub
End Class
