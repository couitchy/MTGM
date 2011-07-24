﻿'---------------------------------------------------------------
'| Projet         | Magic The Gathering Manager - WebResourcer |
'| Contexte       |  		Perso       					   |
'| Date           |      							30/03/2008 |
'| Release 1      |   								12/04/2008 |
'| Release 2      |  								30/08/2008 |
'| Release 3      | 								08/11/2008 |
'| Release 4      |      							29/08/2009 |
'| Release 5      |       							21/03/2010 |
'| Release 6      |       							17/04/2010 |
'| Release 7      |									29/07/2010 |
'| Release 8      |       							03/10/2010 |
'| Release 9      |                       			05/02/2011 |
'| Auteur         |      							  Couitchy |
'|-------------------------------------------------------------|
'| Modifications :               							   |
'---------------------------------------------------------------
Public Partial Class frmExploSettings
	Private VmMustReload As Boolean = False	'Rechargement des menus obligatoires dans le père
	Private VmOwner As MainForm	
	Public Sub New(VpOwner As MainForm)
		Me.InitializeComponent()
		VmOwner = VpOwner
	End Sub
	Public Sub ValidateCriteria
	'---------------------------------------------------
	'Coche la liste des critères sélectionnés par défaut
	'---------------------------------------------------
	Dim VpCriteria() As String = VgOptions.VgSettings.DefaultActivatedCriteria.Split("#")
		For Each VpCriterion As String In VpCriteria
			Try
				Me.chklstClassement.SetItemChecked(CInt(VpCriterion), True)
			Catch
			End Try
		Next VpCriterion
	End Sub	
	Private Sub ManageOrder(VpX As Integer, VpY As Integer, VpZ As Integer)
	Dim VpIndex As Integer = Me.chklstClassement.SelectedIndex
	Dim VpChecked As Boolean = Me.chklstClassement.GetItemChecked(VpIndex)
		Me.chklstClassement.Items.Insert(VpIndex + VpZ, Me.chklstClassement.SelectedItem)
		Me.chklstClassement.Items.RemoveAt(VpIndex + VpX + VpY)
		Me.chklstClassement.SetItemChecked(VpIndex - VpX, VpChecked)
		Me.chklstClassement.SelectedIndex = VpIndex - Vpx
	End Sub	
	Sub ChklstClassementSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
		Me.btUp.Enabled = (Me.chklstClassement.SelectedIndex > 1 And Me.chklstClassement.SelectedIndex <> Me.chklstClassement.Items.Count - 1)
		Me.btDown.Enabled = (Me.chklstClassement.SelectedIndex > 0 And Me.chklstClassement.SelectedIndex < Me.chklstClassement.Items.Count - 2)
		If VmMustReload Then
			Call VmOwner.LoadTvw
		End If
	End Sub
	Sub ChklstClassementItemCheck(ByVal sender As Object, ByVal e As ItemCheckEventArgs)
	Dim VpStr As String
		If Me.chklstClassement.SelectedIndex = 0 Then
			For Each VpItem As Object In VmOwner.mnuDisp.DropDownItems
				VpStr = clsModule.SafeGetText(VpItem)
				If VpStr = clsModule.CgCollection Then
					VpItem.Checked = Not ( e.NewValue = CheckState.Checked )
'				ElseIf VpStr = clsModule.CgRefresh Or VpStr = clsModule.CgPanel Or VpStr = "" Then
'				Else
'					VpItem.Checked = ( e.NewValue = CheckState.Checked )
				End If
			Next VpItem
			Me.chklstClassement.SelectedItems.Clear
			VmMustReload = True	'un peu crade mais l'appel direct à LoadTvw est impossible car les checkboxes ne sont mises à jour qu'à la fin du présent évènement
		End If
	End Sub	
	Sub BtRefreshClick(sender As Object, e As EventArgs)
		Me.Hide
		Call VmOwner.MyRefresh
	End Sub	
	Sub FrmExploSettingsKeyUp(sender As Object, e As KeyEventArgs)
		If e.KeyCode = Keys.Escape Then
			Me.Hide
		End If
	End Sub	
	Sub FrmExploSettingsFormClosing(sender As Object, e As FormClosingEventArgs)
		If e.CloseReason = CloseReason.UserClosing Then
			e.Cancel = True
			Me.Hide
		End If
	End Sub	
	Sub BtUpClick(sender As Object, e As EventArgs)
		Call Me.ManageOrder(1, 0, -1)
	End Sub
	Sub BtDownClick(sender As Object, e As EventArgs)
		Call Me.ManageOrder(-1, 1, 2)
	End Sub	
	Public ReadOnly Property MyList As CheckedListBox
		Get
			Return Me.chklstClassement
		End Get
	End Property
	Public ReadOnly Property NCriteria As Integer
		Get
			Return Me.chklstClassement.Items.Count
		End Get
	End Property	
	Public ReadOnly Property NSelectedCriteria As Integer
		Get
			Return Me.chklstClassement.CheckedItems.Count
		End Get
	End Property
	Public Property DeckMode As Boolean
		Get
			Return Me.chklstClassement.GetItemChecked(0)
		End Get
		Set(VpDeckMode As Boolean)
			Me.chklstClassement.SetItemChecked(0, VpDeckMode)
		End Set
	End Property
End Class