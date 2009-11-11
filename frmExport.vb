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
Imports System.IO
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
	Private Sub GoExport(VpPath As String, VpSource As String)
	'-------------------------------------------------------
	'Exporte la table spécifiée dans le répertoire spécifiée
	'-------------------------------------------------------
	Dim VpOut As New StreamWriter(VpPath + "\" + VpSource + IIf(Me.optApprentice.Checked, clsModule.CgFExtA, clsModule.CgFExtN).ToString)
		If Me.optApprentice.Checked Then
			VpOut.WriteLine("// NAME : " + VpSource)
			VpOut.WriteLine("// CREATOR : " + Environment.UserName)
			VpOut.WriteLine("// FORMAT :")
		End If
		VgDBCommand.CommandText = "Select Card.EncNbr, Items, Card.Title From " + IIf(VpSource = clsModule.CgCollection, "MyCollection Inner Join Card On MyCollection.EncNbr = Card.EncNbr;", "MyGames Inner Join Card On MyGames.EncNbr = Card.EncNbr Where GameID = " + VgOptions.GetDeckIndex(VpSource) + ";")
		VgDBReader = VgDBCommand.ExecuteReader
		With VgDBReader
			While .Read
				If Me.optApprentice.Checked Then
					VpOut.WriteLine("        " + .GetValue(1).ToString + " " + .GetString(2))
				Else
					VpOut.WriteLine(.GetValue(0).ToString + "#" + .GetValue(1).ToString)
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
	Dim VpStrs(0 To 1) As String
	Dim VpId As Integer
	Dim VpSQL As String
	Dim VpO As Object
		'S'il s'agit d'un nouveau deck, l'inscrit en BDD
		If VpIsNew Then
			VpId = VgOptions.GetDeckCount
			VgDBCommand.CommandText = "Insert Into MyGamesID Values (" + VpId.ToString + ", '" + VpSource.Replace("'", "''") + "');"
			VgDBCommand.ExecuteNonQuery
		End If
		'Lecture du fichier d'entrée et ajout dans la base de données
		While Not VpIn.EndOfStream
			VpStrs = VpIn.ReadLine.Split("#")
			'Cas 1 : nouveau deck
			If VpIsNew Then
				VpSQL = "Insert Into MyGames(EncNbr, Items, GameID) Values (" + VpStrs(0) + ", " + VpStrs(1) + ", " + VpId.ToString + ");"
			Else
				'Cas 2 : complément collection
				If VpSource = clsModule.CgCollection Then
					VgDBCommand.CommandText = "Select Items From MyCollection Where EncNbr = " + VpStrs(0) + ";"
					VpO = VgDBCommand.ExecuteScalar
					'Cas 2.1 : la carte a ajouté existe déjà => mise à jour de la quantité somme
					If Not VpO Is Nothing AndAlso CInt(VpO) > 0 Then
						VpSQL = "Update MyCollection Set Items = " + (CInt(VpO) + CInt(VpStrs(1))).ToString + " Where EncNbr = " + VpStrs(0) + ";"
					'Cas 2.2 : nouvelle carte => insertion
					Else
						VpSQL = "Insert Into MyCollection(EncNbr, Items) Values (" + VpStrs(0) + ", " + VpStrs(1) + ");"	
					End If
				'Cas 3 : complément ancien deck
				Else
					VgDBCommand.CommandText = "Select Items From MyGames Where EncNbr = " + VpStrs(0) + " And GameID = " + VgOptions.GetDeckIndex(VpSource) + ";"
					VpO = VgDBCommand.ExecuteScalar	
					'Cas 3.1 : la carte a ajouté existe déjà => mise à jour de la quantité somme
					If Not VpO Is Nothing AndAlso CInt(VpO) > 0 Then
						VpSQL = "Update MyGames Set Items = " + (CInt(VpO) + CInt(VpStrs(1))).ToString + " Where EncNbr = " + VpStrs(0) + " And GameID = " + VgOptions.GetDeckIndex(VpSource) + ";"
					'Cas 3.2 : nouvelle carte => insertion
					Else
						VpSQL = "Insert Into MyGames(EncNbr, Items, GameID) Values (" + VpStrs(0) + ", " + VpStrs(1) + ", " + VgOptions.GetDeckIndex(VpSource) + ");"	
					End If					
				End If
			End If			
			VgDBCommand.CommandText = VpSQL
			VgDBCommand.ExecuteNonQuery
		End While
		VpIn.Close
		'Information utilisateur
		Call clsModule.ShowInformation("Importation terminée.")
		VmMustReload = True
	End Sub
	Sub CmdExportClick(ByVal sender As Object, ByVal e As EventArgs)
		Me.dlgBrowser.ShowDialog
		If Me.dlgBrowser.SelectedPath <> "" Then
			For VpI As Integer = 0 To Me.lstchkSources.Items.Count - 1
				If Me.lstchkSources.GetItemChecked(VpI) Then
					Call Me.GoExport(Me.dlgBrowser.SelectedPath, Me.lstchkSources.Items(VpI).ToString)
				End If
			Next VpI
			Call clsModule.ShowInformation("Exportation terminée.")
			Me.Close
		End If
	End Sub
	Sub FrmExportLoad(ByVal sender As Object, ByVal e As EventArgs)
	'-------------------------------
	'Affiche les sources exportables
	'-------------------------------
		Me.lstchkSources.Items.Clear
		Me.lstImp.Items.Clear
		Me.lstchkSources.Items.Add(clsModule.CgCollection)
		Me.lstImp.Items.Add(clsModule.CgCollection)
		VgDBCommand.CommandText = "Select GameName From MyGamesID;"
		VgDBReader = VgDBCommand.ExecuteReader
		With VgDBReader
			While .Read
				Me.lstchkSources.Items.Add(.GetString(0))
				Me.lstImp.Items.Add(.GetString(0))
			End While
			.Close
		End With
	End Sub
	Sub BtExportActivate(ByVal sender As Object, ByVal e As EventArgs)
		Me.grpExport.Visible = True
		Me.grpImport.Visible = False
	End Sub
	Sub BtImportActivate(ByVal sender As Object, ByVal e As EventArgs)
		Me.grpImport.Visible = True
		Me.grpExport.Visible = False
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
		Me.txtFileImp.Text = Me.dlgFileBrowser.FileName
		Me.txtSourceImp.Text = Me.txtFileImp.Text.Substring(Me.txtFileImp.Text.LastIndexOf("\") + 1)
		Me.txtSourceImp.Text = Me.txtSourceImp.Text.Replace(clsModule.CgFExtN, "")
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
				Call Me.GoImport(Me.txtFileImp.Text, Me.txtSourceImp.Text, True)
			Else
				Call Me.GoImport(Me.txtFileImp.Text, Me.lstImp.SelectedItem.ToString, False)
			End If
		Else
			Call clsModule.ShowWarning("Toutes les informations n'ont pas été saisies...")
		End If
	End Sub
	Sub FrmExportFormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs)
		If VmMustReload Then
			Call VmOwner.LoadMnu
			Call VmOwner.LoadTvw	
		End If
	End Sub
End Class
