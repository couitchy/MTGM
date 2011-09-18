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
'| - gestion cartes foils				   19/12/2010 |
'| - gestion transferts type 'copie'	   26/02/2011 |
'| - quantité à copier ajustable		   09/05/2011 |
'| - gestion transferts type 'swap'		   21/05/2011 |
'------------------------------------------------------
Public Partial Class frmTransfert
	Private VmCardName As String
	Private VmOwner As MainForm
	Private VmSource As String
	Private VmSource2 As String
	Private VmTransfertResult As clsTransfertResult
	Public Sub New(VpOwner As MainForm, VpCardName As String, VpSource As String, VpSource2 As String, VpTransfertResult As clsTransfertResult)
		Me.InitializeComponent()
		VmCardName = VpCardName
		VmSource = VpSource
		VmSource2 = VpSource2
		VmOwner = VpOwner
		VmTransfertResult = VpTransfertResult
		Call Me.GetEditionsDispo(Me.cboSerie)
		If VpTransfertResult.TransfertType = clsTransfertResult.EgTransfertType.Swap Then
			Call Me.GetEditionsDispo(Me.cboSerie2, True)
			Me.cboSerie2.Text = Me.cboSerie2.Items(0)
			Me.grpDest.Visible = True
		Else
			Me.grpDest.Visible = False
			Me.cmdCancel.Top = Me.cmdCancel.Top - Me.grpDest.Height
			Me.cmdOK.Top = Me.cmdOK.Top - Me.grpDest.Height
			Me.Height = Me.Height - Me.grpDest.Height
		End If
		Me.cboSerie.Text = Me.cboSerie.Items(0)
		Me.sldQuant.Minimum = 1
		Me.lblCard.Text = VpCardName
	End Sub
	Private Sub GetEditionsDispo(VpCboSerie As ComboBox, Optional SwapTo As Boolean = False)
	'--------------------------------------------------------------------------------------
	'Charge la liste des éditions disponibles pour la carte courante sur la source courante
	'--------------------------------------------------------------------------------------
	Dim VpSQL As String
		If SwapTo Then
			VpSQL = "Select SeriesNM From Card Inner Join Series On Card.Series = Series.SeriesCD Where Card.Title = '" + VmCardName.Replace("'", "''") + "'"
		ElseIf VmSource = clsModule.CgSFromSearch Then
			VpSQL = "Select SeriesNM From ((" + VmSource2 + " Inner Join Card On " + VmSource + ".EncNbr = Card.EncNbr) Inner Join Series On Card.Series = Series.SeriesCD) Where Card.Title = '" + VmCardName.Replace("'", "''") + "' And "
			VpSQL = VpSQL + VmOwner.Restriction
		Else
			VpSQL = "Select SeriesNM, Foil From ((" + VmSource + " Inner Join Card On " + VmSource + ".EncNbr = Card.EncNbr) Inner Join Series On Card.Series = Series.SeriesCD) Where Card.Title = '" + VmCardName.Replace("'", "''") + "' And "
			VpSQL = VpSQL + VmOwner.Restriction
		End If
		VpSQL = clsModule.TrimQuery(VpSQL)
		VgDBCommand.CommandText = VpSQL
		VgDBReader = VgDBCommand.ExecuteReader
		With VgDBReader
			If VmSource = clsModule.CgSFromSearch Or SwapTo Then
				While .Read
					VpCboSerie.Items.Add(.GetString(0))
				End While
			Else
				While .Read
					VpCboSerie.Items.Add(.GetString(0) + If(.GetBoolean(1), clsModule.CgFoil2, ""))
				End While
			End If
			.Close
		End With
	End Sub
	Public Shared Function NeedsPrecision(VpOwner As MainForm, VpCardName As String, VpSource As String, VpSource2 As String, VpTransfertType As clsTransfertResult.EgTransfertType) As Boolean
	'----------------------------------------------------------------------------------
	'Vérifie si l'opération demandée nécessité des précisions (édition, foil, quantité)
	'----------------------------------------------------------------------------------
	Dim VpSQL As String
	Dim VpRet As Boolean
		'Si on fait un échange d'édition, on a systématiquement besoin d'afficher le formulaire de transfert
		If VpTransfertType = clsTransfertResult.EgTransfertType.Swap Then
			Return True
		End If
		'Cas 1 : plusieurs éditions disponibles ou bien même édition mais avec foil(s) et non foil(s)
		VpSQL = "Select Count(*) From ((" + VpSource2 + " Inner Join Card On " + VpSource + ".EncNbr = Card.EncNbr) Inner Join Series On Card.Series = Series.SeriesCD) Where Card.Title = '" + VpCardName.Replace("'", "''") + "' And "
		VpSQL = VpSQL + VpOwner.Restriction
		VpSQL = clsModule.TrimQuery(VpSQL)
		VgDBCommand.CommandText = VpSQL
		VpRet = ( VgDBCommand.ExecuteScalar > 1 )
		'Si c'est une copie que l'on fait, on n'a pas besoin de savoir combien d'items il y a (ie. pas besoin d'évaluer le cas 2 ci-dessous), cela dépend si l'utilisateur a choisi dans les options de pouvoir régler manuellement le nombre de cartes à copier
		If VpTransfertType = clsTransfertResult.EgTransfertType.Copy Then
			Return If(VgOptions.VgSettings.CopyRange > 1, True, VpRet)
		End If
		'Cas 2 : Si une seule édition mais plusieurs items
		VpSQL = "Select Items From ((" + VpSource + " Inner Join Card On " + VpSource + ".EncNbr = Card.EncNbr) Inner Join Series On Card.Series = Series.SeriesCD) Where Card.Title = '" + VpCardName.Replace("'", "''") + "' And "
		VpSQL = VpSQL + VpOwner.Restriction
		VpSQL = clsModule.TrimQuery(VpSQL)
		VgDBCommand.CommandText = VpSQL
		Return VpRet Or ( VgDBCommand.ExecuteScalar > 1 )
	End Function
	Public Shared Function GetMatchingEdition(VpOwner As MainForm, VpCardName As String, VpSource As String, VpSource2 As String) As String
	'------------------------------------------------------------------------------------------------
	'Dans le cas où NeedsPrecision a retourné faux, trouve la série correspondant à la carte courante
	'------------------------------------------------------------------------------------------------
	Dim VpSQL As String
		VpSQL = "Select Card.Series From ((" + VpSource2 + " Inner Join Card On " + VpSource + ".EncNbr = Card.EncNbr) Inner Join Series On Card.Series = Series.SeriesCD) Where Card.Title = '" + VpCardName.Replace("'", "''") + "' And "
		VpSQL = VpSQL + VpOwner.Restriction
		VpSQL = clsModule.TrimQuery(VpSQL)
		VgDBCommand.CommandText = VpSQL
		Return VgDBCOmmand.ExecuteScalar
	End Function
	Public Shared Sub CommitAction(VpTransfertResult As clsTransfertResult)
	'-----------------------------------------------------------------------------------
	'Modifie les enregistrements de la BDD pour assurer le transfert décrit en paramètre
	'-----------------------------------------------------------------------------------
	Dim VpNItemsAtSource As Integer
	Dim VpNItemsAtDest As Integer
		With VpTransfertResult
			'Dans le cas d'un déplacement ou d'une suppression ou d'un swap : suppression à la source
			If .TransfertType = clsTransfertResult.EgTransfertType.Move Or .TransfertType = clsTransfertResult.EgTransfertType.Deletion Or .TransfertType = clsTransfertResult.EgTransfertType.Swap Then
				'Nombre d'items déjà présents à la source
				VgDBCommand.CommandText = "Select Items From " + .SFrom + " Where Foil = " + .FoilFrom.ToString + " And EncNbr = " + .EncNbrFrom.ToString + If(.SFrom = clsModule.CgSDecks, " And GameID = " + clsModule.GetDeckIndex(.TFrom) + ";", ";")
				VpNItemsAtSource = VgDBCommand.ExecuteScalar
				'-NCartes à la source
				If VpNItemsAtSource - .NCartes > 0 Then
					VgDBCommand.CommandText = "Update " + .SFrom + " Set Items = " + (VpNItemsAtSource - .NCartes).ToString + " Where Foil = " + .FoilFrom.ToString + " And EncNbr = " + .EncNbrFrom.ToString + If(.SFrom = clsModule.CgSDecks, " And GameID = " + clsModule.GetDeckIndex(.TFrom) + ";", ";")
					VgDBCommand.ExecuteNonQuery
				Else
					VgDBCommand.CommandText = "Delete * From " + .SFrom + " Where Foil = " + .FoilFrom.ToString + " And EncNbr = " + .EncNbrFrom.ToString + If(.SFrom = clsModule.CgSDecks, " And GameID = " + clsModule.GetDeckIndex(.TFrom) + ";", ";")
					VgDBCommand.ExecuteNonQuery
				End If
			End If
			'Dans le cas d'un déplacement ou d'une copie ou d'un swap : ajout à destination
			If .TransfertType = clsTransfertResult.EgTransfertType.Move Or .TransfertType = clsTransfertResult.EgTransfertType.Copy Or .TransfertType = clsTransfertResult.EgTransfertType.Swap Then
				'Nombre d'items déjà présents à la destination
				VgDBCommand.CommandText = "Select Items From " + .STo + " Where Foil = " + .FoilTo.ToString + " And EncNbr = " + .EncNbrTo.ToString + If(.STo = clsModule.CgSDecks, " And GameID = " + clsModule.GetDeckIndex(.TTo) + ";", ";")
				VpNItemsAtDest = VgDBCommand.ExecuteScalar
				'+NCartes à la destination
				If VpNItemsAtDest > 0 Then
					VgDBCommand.CommandText = "Update " + .STo + " Set Items = " + (VpNItemsAtDest + .NCartes).ToString + " Where Foil = " + .FoilTo.ToString + " And EncNbr = " + .EncNbrTo.ToString + If(.STo = clsModule.CgSDecks, " And GameID = " + clsModule.GetDeckIndex(.TTo) + ";", ";")
					VgDBCommand.ExecuteNonQuery
				Else
					VgDBCommand.CommandText = "Insert Into " + .STo + " Values (" + If(.STo = clsModule.CgSDecks, clsModule.GetDeckIndex(.TTo) + ", ", "") + .EncNbrTo.ToString + ", " + .NCartes.ToString + ", " + .FoilTo.ToString + ");"
					VgDBCommand.ExecuteNonQuery
				End If
			End If
		End With
	End Sub
	Private Sub ChangeLogo(VpCboSerie As ComboBox, VpPicSerie As PictureBox, Optional ByRef VpEdition As String = "", Optional ByRef VpFoil As Boolean = False)
	'-----------------------------------------
	'Affiche le logo de l'édition sélectionnée
	'-----------------------------------------
	Dim VpKey As Integer
		If VpCboSerie.Text.EndsWith(clsModule.CgFoil2) Then
			VpEdition = clsModule.GetSerieCodeFromName(VpCboSerie.Text.Replace(clsModule.CgFoil2, ""))
			VpFoil = True
		Else
			VpEdition = clsModule.GetSerieCodeFromName(VpCboSerie.Text)
			VpFoil = False
		End If
		VpKey = clsModule.VgImgSeries.Images.IndexOfKey("_e" + VpEdition + CgIconsExt)
		If VpKey <> -1 Then
			VpPicSerie.Image = clsModule.VgImgSeries.Images(VpKey)
		Else
			VpPicSerie.Image = Nothing
		End If
	End Sub
	Sub CboSerieSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
	'-----------------------------------------------------------
	'Réajustement des paramètres en fonction de l'édition source
	'-----------------------------------------------------------
	Dim VpSQL As String
	Dim VpEdition As String = ""
	Dim VpFoil As Boolean = False
		'Logo édition source
		Call Me.ChangeLogo(Me.cboSerie, Me.picSerie, VpEdition, VpFoil)
		Me.chkFoil.Checked = VpFoil
		'Réajuste le nombre de cartes disponibles dans l'édition sélectionnée
		If VmTransfertResult.TransfertType = clsTransfertResult.EgTransfertType.Copy Then
			Me.sldQuant.Maximum = VgOptions.VgSettings.CopyRange
		Else
			VpSQL = "Select " + VmSource + ".Items From " + VmSource + " Inner Join Card On " + VmSource + ".EncNbr = Card.EncNbr Where Card.Title = '" + VmCardName.Replace("'", "''") + "' And Foil = " + VpFoil.ToString + " And Card.Series = '" + VpEdition + "' And "
			VpSQL = VpSQL + VmOwner.Restriction
			VpSQL = clsModule.TrimQuery(VpSQL)
			VgDBCommand.CommandText = VpSQL
			Me.sldQuant.Maximum = CInt(VgDBCommand.ExecuteScalar)
		End If
	End Sub
	Sub CboSerie2SelectedIndexChanged(sender As Object, e As EventArgs)
		'Logo édition destination
		Call Me.ChangeLogo(Me.cboSerie2, Me.picSerie2)
	End Sub
	Sub CmdCancelClick(ByVal sender As Object, ByVal e As EventArgs)
		VmTransfertResult.NCartes = 0
		Me.Close
	End Sub
	Sub CmdOKClick(ByVal sender As Object, ByVal e As EventArgs)
		If Me.cboSerie.Items.Contains(Me.cboSerie.Text) Then
			VmTransfertResult.NCartes = Me.sldQuant.Value
			If Me.cboSerie.Text.EndsWith(clsModule.CgFoil2) Then
				VmTransfertResult.IDSerieFrom = clsModule.GetSerieCodeFromName(Me.cboSerie.Text.Replace(clsModule.CgFoil2, ""))
				VmTransfertResult.FoilFrom = True
			Else
				VmTransfertResult.IDSerieFrom = clsModule.GetSerieCodeFromName(Me.cboSerie.Text)
				VmTransfertResult.FoilFrom = False
			End If
			If VmTransfertResult.TransfertType = clsTransfertResult.EgTransfertType.Swap Then
				VmTransfertResult.IDSerieTo = clsModule.GetSerieCodeFromName(Me.cboSerie2.Text)
				VmTransfertResult.FoilTo = Me.chkFoil.Checked
			Else
				VmTransfertResult.IDSerieTo = VmTransfertResult.IDSerieFrom
				VmTransfertResult.FoilTo = VmTransfertResult.FoilFrom
			End If
		End If
		Me.Close
	End Sub
	Sub SldQuantScroll(ByVal sender As Object, ByVal e As EventArgs)
		Me.lblQuant.Text = Me.sldQuant.Value.ToString
	End Sub
End Class
Public Class clsTransfertResult
	Public Enum EgTransfertType
		Deletion
		Move
		Copy
		Swap
	End Enum
	Public TransfertType As EgTransfertType = EgTransfertType.Move		'Type d'opération
	Public NCartes As Integer = 0										'Nombre de cartes concernées
	Public IDSerieFrom As String = ""									'Edition source
	Public IDSerieTo As String = ""										'Edition destination
	Public EncNbrFrom As Integer = 0									'Numéro encyclopédique source
	Public EncNbrTo As Integer = 0										'Numéro encyclopédique destination
	Public FoilFrom As Boolean = False									'Mention éventuelle foil source
	Public FoilTo As Boolean = False									'Mention éventuelle foil destination
	Public TFrom As String = ""											'Deck source
	Public TTo As String = ""											'Deck destination
	Public SFrom As String = ""											'Nom de la table source
	Public STo As String = ""											'Nom de la table destination
End Class