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
'| - support passif depuis drag&drop	   27/09/2010 |
'| - format v2 avec ref. éditions		   01/10/2010 |
'| - gestion cartes foils				   19/12/2010 |
'------------------------------------------------------
Imports System.IO
Imports System.Xml
Public Partial Class frmExport
	Private VmFormMove As Boolean = False	'Formulaire en déplacement
	Private VmMousePos As Point				'Position initiale de la souris sur la barre de titre
	Private VmCanClose As Boolean = False   'Formulaire peut être fermé
	Private VmMustReload As Boolean = False	'Rechargement des menus obligatoires dans le père
	Private VmOwner As MainForm
	Public Sub New(VpOwner As MainForm)
		Me.InitializeComponent()
		VmOwner = VpOwner
	End Sub
	Private Sub SourcesLoad
	'-------------------------------
	'Affiche les sources exportables
	'-------------------------------
		Me.lstchkSources.Items.Clear
		Me.lstImp.Items.Clear
		Me.lstchkSources.Items.Add(clsModule.CgCollection)
		Me.lstImp.Items.Add(clsModule.CgCollection)
		VgDBCommand.CommandText = "Select GameName From MyGamesID Order By GameID;"
		VgDBReader = VgDBCommand.ExecuteReader
		With VgDBReader
			While .Read
				Me.lstchkSources.Items.Add(.GetString(0))
				Me.lstImp.Items.Add(.GetString(0))
			End While
			.Close
		End With
	End Sub
	Private Sub GoExport(VpPath As String, VpSource As String)
	'-------------------------------------------------------
	'Exporte la table spécifiée dans le répertoire spécifiée
	'-------------------------------------------------------
	Dim VpOut As New StreamWriter(VpPath + "\" + clsModule.AvoidForbiddenChr(VpSource, clsModule.eForbiddenCharset.Full) + If(Me.optApprentice.Checked, clsModule.CgFExtA, If(Me.optNormal.Checked, clsModule.CgFExtN, clsModule.CgFExtO)).ToString)
		If Me.optApprentice.Checked Then
			VpOut.WriteLine("// NAME : " + VpSource)
			VpOut.WriteLine("// CREATOR : " + Environment.UserName)
			VpOut.WriteLine("// FORMAT :")
		End If
		VgDBCommand.CommandText = "Select Card.EncNbr, Items, Card.Title, Card.Series, Foil From " + If(VpSource = clsModule.CgCollection, "MyCollection Inner Join Card On MyCollection.EncNbr = Card.EncNbr;", "MyGames Inner Join Card On MyGames.EncNbr = Card.EncNbr Where GameID = " + clsModule.GetDeckIndex(VpSource) + ";")
		VgDBReader = VgDBCommand.ExecuteReader
		With VgDBReader
			While .Read
				If Me.optApprentice.Checked Then
					VpOut.WriteLine("        " + .GetValue(1).ToString + " " + .GetString(2))
				ElseIf Me.optOld.Checked Then
					VpOut.WriteLine(.GetValue(0).ToString + "#" + .GetValue(1).ToString)
				ElseIf Me.optNormal.Checked Then
					VpOut.WriteLine(.GetValue(2).ToString + "#" + .GetValue(3).ToString + "#" + .GetValue(1).ToString + "#" + .GetValue(4).ToString)
				End If
			End While
			.Close
		End With
		VpOut.Flush
		VpOut.Close
	End Sub
	Private Sub GoImport(VpPath As String, VpSource As String, VpIsNew As Boolean)
	'--------------------------------------------------------------------------------------------------
	'Importe le fichier spécifié à la destination spécifiée (collection ou nouveau deck ou ancien deck)
	'--------------------------------------------------------------------------------------------------
	Dim VpIn As New StreamReader(VpPath)
	Dim VpReader As XmlTextReader
	Dim VpLog As StreamWriter
	Dim VpConverted As StreamWriter
	Dim VpStrs(0 To 1) As String
	Dim VpId As Integer
	Dim VpSQL As String
	Dim VpO As Object
	Dim VpFoil As Boolean
	Dim VpName As String
	Dim VpEdition As String
	Dim VpQte As Integer
	Dim VpNeedLog As Boolean = False
		'S'il s'agit d'un nouveau deck, l'inscrit en BDD
		If VpIsNew Then
			VpId = clsModule.GetNewDeckId
			VgDBCommand.CommandText = "Insert Into MyGamesID Values (" + VpId.ToString + ", '" + VpSource.Replace("'", "''") + "', 0);"
			VgDBCommand.ExecuteNonQuery
		End If
		'** Gestion format MTGM **
		Select Case VpPath.Substring(VpPath.LastIndexOf(".")).ToLower
			Case clsModule.CgFExtO, clsModule.CgFExtN
				'Lecture du fichier d'entrée et ajout dans la base de données
				While Not VpIn.EndOfStream
					VpStrs = VpIn.ReadLine.Split("#")
					'Pré-traitement 1 : dans le cas du nouveau format d'exportation v2, il faut d'abord retrouver le numéro encyclopédique correspondant au nom de la carte et sa série
					If VpStrs.Length > 2 AndAlso VpStrs(2) <> "" Then
						VpStrs(0) = clsModule.GetEncNbr(VpStrs(0), VpStrs(1))
						VpStrs(1) = VpStrs(2)
					End If
					'Pré-traitement 2 : gestion éventuelle de la mention foil
					If VpStrs.Length > 3 Then
						VpFoil = clsModule.Matching(VpStrs(3))
					Else
						VpFoil = False
					End If
					If IsNumeric(VpStrs(0)) AndAlso CInt(VpStrs(0)) <> 0 Then
						'Cas 1 : nouveau deck
						If VpIsNew Then
							VpSQL = "Insert Into MyGames(EncNbr, Items, GameID, Foil) Values (" + VpStrs(0) + ", " + VpStrs(1) + ", " + VpId.ToString + ", " + VpFoil.ToString + ");"
						Else
							'Cas 2 : complément collection
							If VpSource = clsModule.CgCollection Then
								VgDBCommand.CommandText = "Select Items From MyCollection Where EncNbr = " + VpStrs(0) + " And Foil = " + VpFoil.ToString + ";"
								VpO = VgDBCommand.ExecuteScalar
								'Cas 2.1 : la carte a ajouté existe déjà => mise à jour de la quantité somme
								If Not VpO Is Nothing AndAlso CInt(VpO) > 0 Then
									VpSQL = "Update MyCollection Set Items = " + (CInt(VpO) + CInt(VpStrs(1))).ToString + " Where EncNbr = " + VpStrs(0) + " And Foil = " + VpFoil.ToString + ";"
								'Cas 2.2 : nouvelle carte => insertion
								Else
									VpSQL = "Insert Into MyCollection(EncNbr, Items, Foil) Values (" + VpStrs(0) + ", " + VpStrs(1) + ", " + VpFoil.ToString + ");"
								End If
							'Cas 3 : complément ancien deck
							Else
								VgDBCommand.CommandText = "Select Items From MyGames Where EncNbr = " + VpStrs(0) + " And Foil = " + VpFoil.ToString + " And GameID = " + clsModule.GetDeckIndex(VpSource) + ";"
								VpO = VgDBCommand.ExecuteScalar
								'Cas 3.1 : la carte a ajouté existe déjà => mise à jour de la quantité somme
								If Not VpO Is Nothing AndAlso CInt(VpO) > 0 Then
									VpSQL = "Update MyGames Set Items = " + (CInt(VpO) + CInt(VpStrs(1))).ToString + " Where EncNbr = " + VpStrs(0) + " And Foil = " + VpFoil.ToString + " And GameID = " + clsModule.GetDeckIndex(VpSource) + ";"
								'Cas 3.2 : nouvelle carte => insertion
								Else
									VpSQL = "Insert Into MyGames(EncNbr, Items, GameID, Foil) Values (" + VpStrs(0) + ", " + VpStrs(1) + ", " + clsModule.GetDeckIndex(VpSource) + ", " + VpFoil.ToString + ");"
								End If
							End If
						End If
						VgDBCommand.CommandText = VpSQL
						VgDBCommand.ExecuteNonQuery
					End If
				End While
				VpIn.Close
			'** Gestion format Magic Master **
			Case clsModule.CgFExtM
				VpReader = New XmlTextReader(VpIn)
				VpLog = New StreamWriter(VpPath.ToLower.Replace(clsModule.CgFExtM, clsModule.CgPicLogExt))
				VpConverted = New StreamWriter(VpPath.ToLower.Replace(clsModule.CgFExtM, clsModule.CgFExtO))
				While VpReader.Read
					If VpReader.Name = "NOM" Then
						VpName = VpReader.ReadElementContentAsString
						VpReader.ReadToFollowing("EDITION")
						VpEdition = VpReader.ReadElementContentAsString
						VpReader.ReadToFollowing("QTE")
						VpQte =  VpReader.ReadElementContentAsInt
						VpReader.ReadToFollowing("FOIL")
						VpFoil = VpReader.ReadElementContentAsBoolean
						'Exact match
						VgDBCommand.CommandText = "Select EncNbr From Card Inner Join Series On Card.Series = Series.SeriesCD Where LCase(Card.Title) = '" + VpName.ToLower.Replace("'", "''") + "' And (LCase(Series.SeriesNM) = '" + VpEdition.ToLower.Replace("'", "''") + "' Or LCase(Series.SeriesNM_MtG) = '" + VpEdition.ToLower.Replace("'", "''") + "');"
						VpO = VgDBCommand.ExecuteScalar
						If Not VpO Is Nothing Then
							VpConverted.WriteLine(VpO.ToString + "#" + VpQte.ToString + "##" + VpFoil.ToString)
						Else
							VpNeedLog = True
							'Partial match
							VgDBCommand.CommandText = "Select EncNbr From Card Inner Join Series On Card.Series = Series.SeriesCD Where InStr('" + VpName.ToLower.Replace("'", "''") + "', LCase(Card.Title)) > 0 And (InStr('" + VpEdition.ToLower.Replace("'", "''") + "', LCase(Series.SeriesNM)) > 0 Or InStr('" + VpEdition.ToLower.Replace("'", "''") + "', LCase(Series.SeriesNM_MtG)) > 0);"
							VpO = VgDBCommand.ExecuteScalar
							If Not VpO Is Nothing Then
								VpLog.WriteLine("Partial math for card: " + VpName.ToString + " - " + VpEdition.ToString)
								VpConverted.WriteLine(VpO.ToString + "#" + VpQte.ToString + "##" + VpFoil.ToString)
							Else
								VpLog.WriteLine("No math for card: " + VpName.ToString + " - " + VpEdition.ToString)
							End If
						End If
					End If
				End While
				VpConverted.Close
				VpLog.Close
				VpReader.Close
				VpIn.Close
				If VpNeedLog Then
					If clsModule.ShowQuestion("Certaines cartes n'ont pas été trouvées..." + vbCrLf + "Voulez-vous afficher le journal ?") = System.Windows.Forms.DialogResult.Yes Then
						Process.Start(VpPath.ToLower.Replace(clsModule.CgFExtM, clsModule.CgPicLogExt))
					End If
				End If
				'Une fois la conversion effectuée, on rappelle l'importation sur le fichier au bon format
				Call Me.GoImport(VpPath.ToLower.Replace(clsModule.CgFExtM, clsModule.CgFExtO), VpSource, False)
			Case Else
				Call clsModule.ShowWarning("Format non pris en charge...")
		End Select
	End Sub
	Public Sub InitImport(VpFile As String)
		Me.grpImport.Visible = True
		Me.grpExport.Visible = False
		Me.txtFileImp.Text = VpFile
		Me.txtSourceImp.Text = Me.txtFileImp.Text.Substring(Me.txtFileImp.Text.LastIndexOf("\") + 1)
		Me.txtSourceImp.Text = Me.txtSourceImp.Text.Replace(clsModule.CgFExtN, "").Replace(clsModule.CgFExtO, "")
	End Sub
	Sub CmdExportClick(ByVal sender As Object, ByVal e As EventArgs)
		Me.dlgBrowser.ShowDialog
		If Me.dlgBrowser.SelectedPath <> "" Then
			For Each VpSource As String In Me.lstchkSources.CheckedItems
				Call Me.GoExport(Me.dlgBrowser.SelectedPath, VpSource)
			Next VpSource
			Call clsModule.ShowInformation("Exportation terminée.")
			Me.Close
		End If
	End Sub
	Sub FrmExportLoad(ByVal sender As Object, ByVal e As EventArgs)
		Call Me.SourcesLoad
		Me.TopMost = True
	End Sub
	Sub BtExportActivate(ByVal sender As Object, ByVal e As EventArgs)
		Me.grpExport.Visible = True
		Me.btImport.Checked = False
		Me.grpImport.Visible = False
		Me.btExport.Checked = True
	End Sub
	Sub BtImportActivate(ByVal sender As Object, ByVal e As EventArgs)
		Me.grpImport.Visible = True
		Me.btExport.Checked = False
		Me.grpExport.Visible = False
		Me.btImport.Checked = True
	End Sub
	Sub OptImpAddCheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
		Me.lstImp.Enabled = Me.optImpAdd.Checked
		Me.txtSourceImp.Enabled = Me.optImpNew.Checked
	End Sub
	Sub OptImpNewCheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
		Me.lstImp.Enabled = Me.optImpAdd.Checked
		Me.txtSourceImp.Enabled = Me.optImpNew.Checked
	End Sub
	Sub CbarImpExpMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
		VmFormMove = True
		VmCanClose = True
		VmMousePos = New Point(e.X, e.Y)
	End Sub
	Sub CbarImpExpMouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
		If VmFormMove Then
			Me.Location = New Point(MousePosition.X - VmMousePos.X, MousePosition.Y - VmMousePos.Y)
		End If
	End Sub
	Sub CbarImpExpMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs)
		VmFormMove = False
	End Sub
	Sub CbarImpExpVisibleChanged(ByVal sender As Object, ByVal e As EventArgs)
		If VmCanClose Then
			Me.Close
		End If
	End Sub
	Sub CmdBrowseClick(ByVal sender As Object, ByVal e As EventArgs)
		Me.dlgFileBrowser.ShowDialog
		Call Me.InitImport(Me.dlgFileBrowser.FileName)
	End Sub
	Sub CmdImportClick(ByVal sender As Object, ByVal e As EventArgs)
	Dim VpOK As Boolean
		'Vérification de la validité de la demande
		VpOK = File.Exists(Me.txtFileImp.Text)
		If Me.optImpNew.Checked Then
			If Me.txtSourceImp.Text.Trim = "" Then
				VpOK = False
			End If
		Else
			If Me.lstImp.SelectedIndex < 0 Then
				VpOK = False
			End If
		End If
		If VpOK Then
			'Importation effective
			If Me.optImpNew.Checked Then
				For Each VpD As String In Me.lstImp.Items
					If VpD.ToLower = Me.txtSourceImp.Text.ToLower Then
						Call clsModule.ShowWarning("Un deck portant ce nom existe déjà...")
						Exit Sub
					End If
				Next VpD
				Call Me.GoImport(Me.txtFileImp.Text, Me.txtSourceImp.Text, True)
			Else
				Call Me.GoImport(Me.txtFileImp.Text, Me.lstImp.SelectedItem.ToString, False)
			End If
			Call Me.SourcesLoad
			'Information utilisateur
			Call clsModule.ShowInformation("Importation terminée.")
			VmMustReload = True
		Else
			Call clsModule.ShowWarning("Toutes les informations n'ont pas été saisies...")
		End If
	End Sub
	Sub FrmExportFormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs)
		If VmMustReload And e.CloseReason = CloseReason.UserClosing Then
			Me.Visible = False
			Call VmOwner.LoadMnu
			Call VmOwner.LoadTvw
		End If
	End Sub
End Class
