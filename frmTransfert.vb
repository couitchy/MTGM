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
'| Auteur         |                          Couitchy |
'|----------------------------------------------------|
'| Modifications :                                    |
'| - gestion cartes foils				   19/12/2010 |
'| - gestion transferts type 'copie'	   26/02/2011 |
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
		Call Me.GetEditionsDispo
		Me.cboSerie.Text = Me.cboSerie.Items(0)
		Me.sldQuant.Minimum = 1
		Me.lblCard.Text = VpCardName
	End Sub
	Private Sub GetEditionsDispo
	'--------------------------------------------------------------------------------------
	'Charge la liste des éditions disponibles pour la carte courante sur la source courante
	'--------------------------------------------------------------------------------------
	Dim VpSQL As String
		If VmSource = clsModule.CgSFromSearch Then
			VpSQL = "Select SeriesNM From ((" + VmSource2 + " Inner Join Card On " + VmSource + ".EncNbr = Card.EncNbr) Inner Join Series On Card.Series = Series.SeriesCD) Where Card.Title = '" + VmCardName.Replace("'", "''") + "' And "
		Else
			VpSQL = "Select SeriesNM, Foil From ((" + VmSource + " Inner Join Card On " + VmSource + ".EncNbr = Card.EncNbr) Inner Join Series On Card.Series = Series.SeriesCD) Where Card.Title = '" + VmCardName.Replace("'", "''") + "' And "
		End If
		VpSQL = VpSQL + VmOwner.Restriction
		VpSQL = clsModule.TrimQuery(VpSQL)
		VgDBCommand.CommandText = VpSQL
		VgDBReader = VgDBCommand.ExecuteReader
		With VgDBReader
			If VmSource = clsModule.CgSFromSearch Then
				While .Read
					Me.cboSerie.Items.Add(.GetString(0))
				End While
			Else
				While .Read
					Me.cboSerie.Items.Add(.GetString(0) + If(.GetBoolean(1), clsModule.CgFoil2, ""))
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
		'Cas 1 : plusieurs éditions disponibles ou bien même édition mais avec foil(s) et non foil(s)
		VpSQL = "Select Count(*) From ((" + VpSource2 + " Inner Join Card On " + VpSource + ".EncNbr = Card.EncNbr) Inner Join Series On Card.Series = Series.SeriesCD) Where Card.Title = '" + VpCardName.Replace("'", "''") + "' And "
		VpSQL = VpSQL + VpOwner.Restriction
		VpSQL = clsModule.TrimQuery(VpSQL)
		VgDBCommand.CommandText = VpSQL
		VpRet = ( VgDBCommand.ExecuteScalar > 1 )
		If VpTransfertType = clsTransfertResult.EgTransfertType.Copy Then
			Return VpRet	'dans le cas de la copie, on n'a pas besoin de savoir combien d'items il y a
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
			'Dans le cas d'un déplacement ou d'une suppression : suppression à la source
			If .TransfertType = clsTransfertResult.EgTransfertType.Move Or .TransfertType = clsTransfertResult.EgTransfertType.Deletion Then
				'Nombre d'items déjà présents à la source
				VgDBCommand.CommandText = "Select Items From " + .SFrom + " Where Foil = " + .Foil.ToString + " And EncNbr = " + .EncNbr.ToString + If(.SFrom = clsModule.CgSDecks, " And GameID = " + clsModule.GetDeckIndex(.TFrom) + ";", ";")
				VpNItemsAtSource = VgDBCommand.ExecuteScalar
				'-NCartes à la source
				If VpNItemsAtSource - .NCartes > 0 Then
					VgDBCommand.CommandText = "Update " + .SFrom + " Set Items = " + (VpNItemsAtSource - .NCartes).ToString + " Where Foil = " + .Foil.ToString + " And EncNbr = " + .EncNbr.ToString + If(.SFrom = clsModule.CgSDecks, " And GameID = " + clsModule.GetDeckIndex(.TFrom) + ";", ";")
					VgDBCommand.ExecuteNonQuery
				Else
					VgDBCommand.CommandText = "Delete * From " + .SFrom + " Where Foil = " + .Foil.ToString + " And EncNbr = " + .EncNbr.ToString + If(.SFrom = clsModule.CgSDecks, " And GameID = " + clsModule.GetDeckIndex(.TFrom) + ";", ";")
					VgDBCommand.ExecuteNonQuery
				End If
			End If
			'Dans le cas d'un déplacement ou d'une copie : ajout à destination
			If .TransfertType = clsTransfertResult.EgTransfertType.Move Or .TransfertType = clsTransfertResult.EgTransfertType.Copy Then
				'Nombre d'items déjà présents à la destination
				VgDBCommand.CommandText = "Select Items From " + .STo + " Where Foil = " + .Foil.ToString + " And EncNbr = " + .EncNbr.ToString + If(.STo = clsModule.CgSDecks, " And GameID = " + clsModule.GetDeckIndex(.TTo) + ";", ";")
				VpNItemsAtDest = VgDBCommand.ExecuteScalar
				'+NCartes à la destination
				If VpNItemsAtDest > 0 Then
					VgDBCommand.CommandText = "Update " + .STo + " Set Items = " + (VpNItemsAtDest + .NCartes).ToString + " Where Foil = " + .Foil.ToString + " And EncNbr = " + .EncNbr.ToString + If(.STo = clsModule.CgSDecks, " And GameID = " + clsModule.GetDeckIndex(.TTo) + ";", ";")
					VgDBCommand.ExecuteNonQuery
				Else
					VgDBCommand.CommandText = "Insert Into " + .STo + " Values (" + If(.STo = clsModule.CgSDecks, clsModule.GetDeckIndex(.TTo) + ", ", "") + .EncNbr.ToString + ", " + .NCartes.ToString + ", " + .Foil.ToString + ");"
					VgDBCommand.ExecuteNonQuery
				End If
			End If
		End With
	End Sub
	Sub CboSerieSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
	'-----------------------------------------
	'Affiche le logo de l'édition sélectionnée
	'-----------------------------------------
	Dim VpEdition As String
	Dim VpKey As Integer
	Dim VpSQL As String
	Dim VpFoil As Boolean
		If Me.cboSerie.Text.EndsWith(clsModule.CgFoil2) Then
			VpEdition = clsModule.GetSerieCodeFromName(Me.cboSerie.Text.Replace(clsModule.CgFoil2, ""))
			VpFoil = True
		Else
			VpEdition = clsModule.GetSerieCodeFromName(Me.cboSerie.Text)
			VpFoil = False
		End If
		VpKey = clsModule.VgImgSeries.Images.IndexOfKey("_e" + VpEdition + CgIconsExt)
		If VpKey <> -1 Then
			Me.picSerie.Image = clsModule.VgImgSeries.Images(VpKey)
		Else
			Me.picSerie.Image = Nothing
		End If
		'Réajuste le nombre de cartes disponibles dans l'édition sélectionnée
		If VmTransfertResult.TransfertType = clsTransfertResult.EgTransfertType.Copy Then
			Me.sldQuant.Maximum = 1
		Else
			VpSQL = "Select " + VmSource + ".Items From " + VmSource + " Inner Join Card On " + VmSource + ".EncNbr = Card.EncNbr Where Card.Title = '" + VmCardName.Replace("'", "''") + "' And Foil = " + VpFoil.ToString + " And Card.Series = '" + VpEdition + "' And "
			VpSQL = VpSQL + VmOwner.Restriction
			VpSQL = clsModule.TrimQuery(VpSQL)
			VgDBCommand.CommandText = VpSQL
			Me.sldQuant.Maximum = CInt(VgDBCommand.ExecuteScalar)
		End If
	End Sub
	Sub CmdCancelClick(ByVal sender As Object, ByVal e As EventArgs)
		VmTransfertResult.NCartes = 0
		Me.Close
	End Sub
	Sub CmdOKClick(ByVal sender As Object, ByVal e As EventArgs)
		If Me.cboSerie.Items.Contains(Me.cboSerie.Text) Then
			VmTransfertResult.NCartes = Me.sldQuant.Value
			If Me.cboSerie.Text.EndsWith(clsModule.CgFoil2) Then
				VmTransfertResult.IDSerie = clsModule.GetSerieCodeFromName(Me.cboSerie.Text.Replace(clsModule.CgFoil2, ""))
				VmTransfertResult.Foil = True
			Else
				VmTransfertResult.IDSerie = clsModule.GetSerieCodeFromName(Me.cboSerie.Text)
				VmTransfertResult.Foil = False
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
	End Enum
	Public NCartes As Integer = 0										'Nombre de cartes concernées
	Public IDSerie As String = ""										'Edition correspondante
	Public EncNbr As Integer = 0										'Numéro encyclopédique des cartes concernées
	Public Foil As Boolean = False										'Mention éventuelle foil pour les cartes concernées
	Public TransfertType As EgTransfertType = EgTransfertType.Move		'Type d'opération
	Public TFrom As String = ""											'Deck source
	Public TTo As String = ""											'Deck destination
	Public SFrom As String = ""											'Nom de la table source
	Public STo As String = ""											'Nom de la table destination
End Class