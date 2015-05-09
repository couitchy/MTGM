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
'| Release 11     |                        24/01/2012 |
'| Release 12     |                        01/10/2012 |
'| Release 13     |                        09/05/2014 |
'| Release 14     |                        09/05/2015 |
'| Auteur         |                          Couitchy |
'|----------------------------------------------------|
'| Modifications :                                    |
'| - /!\ nouveau site MagicCorporation     01/10/2010 |
'------------------------------------------------------
Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Data
Imports System.Data.OleDb
Public Partial Class frmTranslate
	Private Const CmURL     As String  = "http://www.magiccorporation.com/mc.php?rub=cartes&op=search&word=#cardname#&search=2"
	Private Const CmId      As String  = "#cardname#"
	Private Const CmKey     As String  = "gathering-cartes-view"
	Private Const CmFrench  As Integer = 2	
	Private VmOwner As MainForm
	Public Sub New(VpOwner As MainForm)
		Me.InitializeComponent()
		VmOwner = VpOwner
		CheckForIllegalCrossThreadCalls = False			'Fortement déconseillé en temps normal mais permet d'éviter un plantage lorsque l'utilisateur ferme sauvagement
	End Sub	
	Private Function Translate(VpIn As String) As String
	'---------------------------------------------------------------------
	'Cherche la traduction en ligne du nom de la carte passée en paramètre
	'---------------------------------------------------------------------
	Dim VpRequest As HttpWebRequest		'Requête HTTP
	Dim VpAnswer As Stream				'Flux de réponse
	Dim VpStr As String = ""			'Chaîne support
	Dim VpStrs() As String				'Séparations de la chaîne
	Dim VpCurByte As Integer			'Octet courant
		Try	
			VpRequest = WebRequest.Create(CmURL.Replace(CmId, VpIn.Replace(" ", "+")))
			VpAnswer = VpRequest.GetResponse().GetResponseStream()
			VpCurByte = VpAnswer.ReadByte
			While VpCurByte <> -1
				VpStr = VpStr + (Encoding.Default.GetString(New Byte() {VpCurByte}))
				VpCurByte = VpAnswer.ReadByte
				Application.DoEvents
			End While
			VpStrs = VpStr.Split(New String() {CmKey}, StringSplitOptions.None)
			VpStr = VpStrs(CmFrench)
			VpStr = VpStr.Substring(VpStr.IndexOf("""") + 2)
			VpStr = VpStr.Substring(0, VpStr.IndexOf("<"))
			VpStr = VpStr.Replace("&#039;", "'")	'Petite feinte entre guillemet simple et apostrophe
			Return VpStr
		Catch
			If Me.chkAlert.Checked Then
				Call clsModule.ShowWarning("Un problème est survenu lors de la traduction de la carte " + VpIn + "...")
			End If
		End Try		
		Return VpIn
	End Function	
	Private Sub Launch(VpSerie As String)
	'------------------------------------------------------------------------------------------
	'Parcours les cartes de la série passée en paramètre et demande leur traduction une par une
	'------------------------------------------------------------------------------------------
	Dim VpDBCommand As New OleDbCommand
		MainForm.VgMe.IsMainReaderBusy = True
    	VpDBCommand.Connection = VgDB
    	VpDBCommand.CommandType = CommandType.Text	
		VgDBCommand.CommandText = "Select Title, EncNbr From Card Where Series = '" + VpSerie + "';"
		VgDBReader = VgDBCommand.ExecuteReader
		With VgDBReader
			While .Read
				'Traduction de la carte
				Me.txtEN.Text = .GetString(0)
				Me.txtFR.Text = Me.Translate(Me.txtEN.Text)
				'Mise à jour dans la base de données
				Try
					VpDBCommand.CommandText = "Update CardFR Set TitleFR = '" + Me.txtFR.Text.Replace("'", "''") + "' Where EncNbr = " + .GetValue(1).ToString + ";"
					VpDBCommand.ExecuteNonQuery
					Me.txtCount.Text = (Val(Me.txtCount.Text) + 1).ToString
				Catch
				End Try
				'Démonopolisation des ressources
				Application.DoEvents
				If .IsClosed Then
					MainForm.VgMe.IsMainReaderBusy = False
					Exit Sub
				End If
			End While
			.Close
		End With
		MainForm.VgMe.IsMainReaderBusy = False
		Call clsModule.ShowInformation("Traduction terminée !")
	End Sub
	Sub CmdGoClick(ByVal sender As Object, ByVal e As EventArgs)
		If Not Me.cboSerie.SelectedItem Is Nothing Then
			Me.txtCount.Text = "0"
			Me.cmdGo.Enabled = False
			Call Me.Launch(Me.cboSerie.Text.Substring(1, 2))
			Me.cmdGo.Enabled = True
		Else
			Call clsModule.ShowWarning("Aucune édition n'a été sélectionnée...")
		End If
	End Sub	
	Sub FrmTranslateLoad(ByVal sender As Object, ByVal e As EventArgs)
		Call clsModule.LoadEditions(Me.cboSerie)
	End Sub	
	Sub CboSerieSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
	'-----------------------------------------
	'Affiche le logo de l'édition sélectionnée
	'-----------------------------------------
	Dim VpKey As Integer = clsModule.VgImgSeries.Images.IndexOfKey("_e" + Me.cboSerie.Text.Substring(1, 2) + CgIconsExt)
		If VpKey <> -1 Then
			Me.picSerie.Image = clsModule.VgImgSeries.Images(VpKey)
		Else
			Me.picSerie.Image = Nothing
		End If
	End Sub	
	Sub FrmTranslateFormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs)
		VgDBReader.Close
	End Sub
End Class
