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
'------------------------------------------------------
Public Partial Class frmTransfert
	Private VmCardName As String
	Private VmOwner As MainForm
	Private VmSource As String
	Private VmTransfertResult As clsTransfertResult
	Public Sub New(VpOwner As MainForm, VpCardName As String, VpSource As String, VpTransfertResult As clsTransfertResult)
		Me.InitializeComponent()
		VmCardName = VpCardName
		VmSource = VpSource
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
		VpSQL = "Select Series From " + VmSource + " Inner Join Card On " + VmSource + ".EncNbr = Card.EncNbr Where Card.Title = '" + VmCardName.Replace("'", "''") + "' And "
		VpSQL = VpSQL + VmOwner.Restriction
		VpSQL = clsModule.TrimQuery(VpSQL)
		VgDBCommand.CommandText = VpSQL		
		VgDBReader = VgDBCommand.ExecuteReader
		With VgDBReader
			While .Read
				Me.cboSerie.Items.Add(.GetString(0))
			End While
			.Close
		End With		
	End Sub
	Public Shared Sub CommitAction(VpTransfertResult As clsTransfertResult)
	'-----------------------------------------------------------------------------------
	'Modifie les enregistrements de la BDD pour assurer le transfert décrit en paramètre
	'-----------------------------------------------------------------------------------
	Dim VpNItemsAtSource As Integer
	Dim VpNItemsAtDest As Integer
		With VpTransfertResult
			'Si pas d'annulation utilisateur
			If .NCartes <> 0 Then
				'Nombre d'items déjà présents à la source
				VgDBCommand.CommandText = "Select Items From " + .SFrom + " Where EncNbr = " + .EncNbr.ToString + IIf(.SFrom = clsModule.CgSDecks, " And GameID = " + VgOptions.GetDeckIndex(.TFrom) + ";", ";")
				VpNItemsAtSource = VgDBCommand.ExecuteScalar				
				'-NCartes à la source
				If VpNItemsAtSource - .NCartes > 0 Then
					VgDBCommand.CommandText = "Update " + .SFrom + " Set Items = " + (VpNItemsAtSource - .NCartes).ToString + " Where EncNbr = " + .EncNbr.ToString + IIf(.SFrom = clsModule.CgSDecks, " And GameID = " + VgOptions.GetDeckIndex(.TFrom) + ";", ";")
					VgDBCommand.ExecuteNonQuery									
				Else
					VgDBCommand.CommandText = "Delete * From " + .SFrom + " Where EncNbr = " + .EncNbr.ToString + IIf(.SFrom = clsModule.CgSDecks, " And GameID = " + VgOptions.GetDeckIndex(.TFrom) + ";", ";")
					VgDBCommand.ExecuteNonQuery				
				End If
				'Dans le cas d'un déplacement
				If .TransfertType = clsTransfertResult.EgTransfertType.Move Then
					'Nombre d'items déjà présents à la destination
					VgDBCommand.CommandText = "Select Items From " + .STo + " Where EncNbr = " + .EncNbr.ToString + IIf(.STo = clsModule.CgSDecks, " And GameID = " + VgOptions.GetDeckIndex(.TTo) + ";", ";")
					VpNItemsAtDest = VgDBCommand.ExecuteScalar
					'+NCartes à la destination
					If VpNItemsAtDest > 0 Then
						VgDBCommand.CommandText = "Update " + .STo + " Set Items = " + (VpNItemsAtDest + .NCartes).ToString + " Where EncNbr = " + .EncNbr.ToString + IIf(.STo = clsModule.CgSDecks, " And GameID = " + VgOptions.GetDeckIndex(.TTo) + ";", ";")
						VgDBCommand.ExecuteNonQuery						
					Else
						VgDBCommand.CommandText = "Insert Into " + .STo + " Values (" + IIf(.STo = clsModule.CgSDecks, VgOptions.GetDeckIndex(.TTo) + ", ", "") + .EncNbr.ToString + ", " + .NCartes.ToString + ");"
						VgDBCommand.ExecuteNonQuery
					End If
				End If
			End If	
		End With
	End Sub
	Sub CboSerieSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
	'-----------------------------------------
	'Affiche le logo de l'édition sélectionnée
	'-----------------------------------------
	Dim VpKey As Integer = clsModule.VgImgSeries.Images.IndexOfKey("_e" + Me.cboSerie.Text + CgIconsExt)
	Dim VpSQL As String
		If VpKey <> -1 Then
			Me.picSerie.Image = clsModule.VgImgSeries.Images(VpKey)
		Else
			Me.picSerie.Image = Nothing
		End If		
		'Réajuste le nombre de cartes disponibles dans l'édition sélectionnée
		VpSQL = "Select " + VmSource + ".Items From " + VmSource + " Inner Join Card On " + VmSource + ".EncNbr = Card.EncNbr Where Card.Title = '" + VmCardName.Replace("'", "''") + "' And Card.Series = '" + Me.cboSerie.Text + "' And "
		VpSQL = VpSQL + VmOwner.Restriction
		VpSQL = clsModule.TrimQuery(VpSQL)
		VgDBCommand.CommandText = VpSQL		
		Me.sldQuant.Maximum = CInt(VgDBCommand.ExecuteScalar)		
	End Sub	
	Sub CmdCancelClick(ByVal sender As Object, ByVal e As EventArgs)
		VmTransfertResult.NCartes = 0
		Me.Close
	End Sub	
	Sub CmdOKClick(ByVal sender As Object, ByVal e As EventArgs)
		If Me.cboSerie.Items.Contains(Me.cboSerie.Text) Then
			VmTransfertResult.NCartes = Me.sldQuant.Value
			VmTransfertResult.IDSerie = Me.cboSerie.Text
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
	End Enum
	Public NCartes As Integer = 0										'Nombre de cartes concernées
	Public IDSerie As String = ""										'Edition correspondante
	Public EncNbr As Integer = 0										'Numéro encyclopédique des cartes concernées
	Public TransfertType As EgTransfertType = EgTransfertType.Move		'Type d'opération
	Public TFrom As String = ""											'Deck source
	Public TTo As String = ""											'Deck destination
	Public SFrom As String = ""											'Nom de la table source
	Public STo As String = ""											'Nom de la table destination
End Class
