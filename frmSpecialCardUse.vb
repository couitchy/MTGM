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
'------------------------------------------------------
Public Partial Class frmSpecialCardUse
	Private VmFormMove As Boolean = False	'Formulaire en déplacement
	Private VmMousePos As Point				'Position initiale de la souris sur la barre de titre
	Private VmCanClose As Boolean = False   'Formulaire peut être fermé		
	Private VmHelp As ToolTip				'Info-bulles d'aide à la saisie
	Public Sub New(VpSource As String, VpRestriction As String)
	Dim VpPartie As clsPartie
		Me.InitializeComponent()
		VpPartie = New clsPartie(VpSource, VpRestriction)
		For Each VpCard As clsCard In VpPartie.CardsInFullDeck
			If Not Me.cboCard.Items.Contains(VpCard.CardName) Then
				Me.cboCard.Items.Add(VpCard.CardName)
			End If
		Next VpCard
		Call Me.LoadSpecials
		Me.cboCard.SelectedIndex = 0
	End Sub
	Private Sub LoadSpecials
		VgDBCommand.CommandText = "Select Description, IsEffort From SpecialUse Order By Description;"
		VgDBReader = VgDBCommand.ExecuteReader
		With VgDBReader
			While .Read
				If .GetBoolean(1) Then
					Me.cboEffort.Items.Add(.GetString(0))
				Else
					Me.cboEffet.Items.Add(.GetString(0))
				End If
			End While
			.Close
		End With
	End Sub
	Private Sub CbarSpecialCardMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
		VmFormMove = True
		VmMousePos = New Point(e.X, e.Y)
	End Sub
	Private Sub CbarSpecialCardMouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
		If VmFormMove Then
			Me.Location = New Point(MousePosition.X - VmMousePos.X, MousePosition.Y - VmMousePos.Y)
		End If
	End Sub
	Private Sub CbarSpecialCardMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs)
		VmFormMove = False
	End Sub		
	Private Sub CbarSpecialCardVisibleChanged(ByVal sender As Object, ByVal e As EventArgs)
		If VmCanClose AndAlso Not Me.cbarSpecialCard.Visible Then
			Me.Close
		End If
	End Sub		
	Sub ChkDefineSpecialCheckedChanged(sender As Object, e As EventArgs)
		Me.lbl1.Enabled = Me.chkDefineSpecial.Checked
		Me.lbl11.Enabled = Me.chkDefineSpecial.Checked
		Me.lbl2.Enabled = Me.chkDefineSpecial.Checked
		Me.lbl21.Enabled = Me.chkDefineSpecial.Checked
		Me.cboEffet.Enabled = Me.chkDefineSpecial.Checked
		Me.cboEffort.Enabled = Me.chkDefineSpecial.Checked
		Me.txtEffet.Enabled = Me.chkDefineSpecial.Checked
		Me.txtEffort.Enabled = Me.chkDefineSpecial.Checked
		Me.chkInvocTapped.Enabled = Me.chkDefineSpecial.Checked
		Me.chkDoesntUntap.Enabled = Me.chkDefineSpecial.Checked
	End Sub
	Sub CmdSaveClick(sender As Object, e As EventArgs)
		VgDBCommand.CommandText = "Delete * From MySpecialUses Where Card = '" + Me.cboCard.Text.Replace("'", "''") + "';"
		VgDBCommand.ExecuteNonQuery
		If Me.chkDefineSpecial.Checked And Me.cboEffet.Text <> "" And Me.cboEffort.Text <> "" Then
			VgDBCommand.CommandText = "Insert Into MySpecialUses Values (" + clsSpeciality.GetSpecId(Me.cboEffort.Text).ToString + ", " + clsSpeciality.GetSpecId(Me.cboEffet.Text).ToString + ", '" + Me.cboCard.Text.Replace("'", "''") + "', '" + Me.txtEffort.Text + "', '" + Me.txtEffet.Text + "', " + Me.chkInvocTapped.Checked.ToString + ", " + Me.chkDoesntUntap.Checked.ToString + ");"
			Try
				VgDBCommand.ExecuteNonQuery
				Call clsModule.ShowInformation("Cet effet sera pris en compte dès la prochaine simulation.")
			Catch
				Call clsModule.ShowWarning(clsModule.CgErr1)
			End Try
		Else
			Call clsModule.ShowWarning("L'effet a été défini de manière incomplète...")
		End If
	End Sub
	Sub CboCardSelectedIndexChanged(sender As Object, e As EventArgs)
	Dim VpSpeciality As New clsSpeciality(Me.cboCard.Text)
		If VpSpeciality.IsSpecial Then
			Me.chkDefineSpecial.Checked = True
			Me.txtEffort.Text = VpSpeciality.Effort
			Me.txtEffet.Text = VpSpeciality.Effet
			Me.cboEffort.Text = clsSpeciality.GetSpecTxt(VpSpeciality.EffortID)
			Me.cboEffet.Text = clsSpeciality.GetSpecTxt(VpSpeciality.EffetID)	
			Me.chkInvocTapped.Checked = VpSpeciality.InvocTapped
			Me.chkDoesntUntap.Checked = VpSpeciality.DoesntUntap
		Else
			Me.chkDefineSpecial.Checked = False			
		End If
	End Sub
	Sub TxtEffortEnter(ByVal sender As Object, ByVal e As EventArgs)
		VmHelp = New ToolTip
		VmHelp.Show(clsSpeciality.GetSpecHlp(clsSpeciality.GetSpecId(Me.cboEffort.Text)), Me, Me.txtEffort.Left, Me.txtEffort.Top + Me.grpUse.Top - Me.txtEffort.Height)
	End Sub
	Sub TxtEffetEnter(ByVal sender As Object, ByVal e As EventArgs)
		VmHelp = New ToolTip
		VmHelp.Show(clsSpeciality.GetSpecHlp(clsSpeciality.GetSpecId(Me.cboEffet.Text)), Me, Me.txtEffet.Left, Me.txtEffet.Top + Me.grpUse.Top - Me.txtEffet.Height)
	End Sub
	Sub TxtEffortLeave(ByVal sender As Object, ByVal e As EventArgs)
		VmHelp.RemoveAll
	End Sub
	Sub TxtEffetLeave(ByVal sender As Object, ByVal e As EventArgs)
		VmHelp.RemoveAll
	End Sub
	Sub CmdCancelClick(sender As Object, e As EventArgs)
		Me.Close	
	End Sub
	Sub FrmSpecialCardUseLoad(sender As Object, e As EventArgs)
		VmCanClose = True
	End Sub
End Class
Public Class clsSpeciality
	Private VmEffortID As Integer = -1
	Private VmEffetID As Integer = -1
	Private VmEffort As String = ""
	Private VmEffet As String = ""
	Private VmInvocTapped As Boolean = False
	Private VmDoesntUntap As Boolean = False
	Private VmSpecial As Boolean = False
	Private Shared VmModelOutOfDateErr As Boolean = False
	Public Sub New(VpCard As String)
		VgDBCommand.CommandText = "Select EffortID, Effort, EffetID, Effet, InvocTapped, DoesntUntap From MySpecialUses Where Card = '" + VpCard.Replace("'", "''") + "';"
		Try
			VgDBReader = VgDBCommand.ExecuteReader
			With VgDBReader
				.Read
				If .HasRows Then
					VmSpecial = True
					VmEffortID = .GetValue(0)
					VmEffetID = .GetValue(2)
					VmEffort = .GetValue(1).ToString
					VmEffet = .GetValue(3).ToString
					VmInvocTapped = .GetBoolean(4)
					VmDoesntUntap = .GetBoolean(5)
				Else
					VmSpecial = False
				End If
				.Close
			End With		
		Catch
			If Not VmModelOutOfDateErr Then
				Call clsModule.ShowWarning(clsModule.CgErr1)
				VmModelOutOfDateErr = True
			End If
		End Try
	End Sub
	Public Shared Function GetSpecId(VpSpec As String) As Integer
		VgDBCommand.CommandText = "Select SpecID From SpecialUse Where Description = '" + VpSpec.Replace("'", "''") + "';"
		Return VgDBCommand.ExecuteScalar
	End Function
	Public Shared Function GetSpecTxt(VpId As Integer) As String
		VgDBCommand.CommandText = "Select Description From SpecialUse Where SpecID = " + VpId.ToString + ";"
		Return VgDBCommand.ExecuteScalar		
	End Function
	Public Shared Function GetSpecHlp(VpId As Integer) As String
		VgDBCommand.CommandText = "Select Aide From SpecialUse Where SpecID = " + VpId.ToString + ";"
		Return VgDBCommand.ExecuteScalar		
	End Function		
	Public ReadOnly Property EffortID As Integer
		Get
			Return VmEffortID
		End Get
	End Property
	Public ReadOnly Property EffetID As Integer
		Get
			Return VmEffetID
		End Get
	End Property	
	Public ReadOnly Property Effort As String
		Get
			Return VmEffort
		End Get
	End Property
	Public ReadOnly Property Effet As String
		Get
			Return VmEffet
		End Get
	End Property	
	Public ReadOnly Property InvocTapped As Boolean
		Get
			Return VmInvocTapped
		End Get	
	End Property
	Public ReadOnly Property DoesntUntap As Boolean
		Get
			Return VmDoesntUntap
		End Get	
	End Property
	Public ReadOnly Property IsSpecial As Boolean
		Get
			Return VmSpecial
		End Get	
	End Property	
End Class
