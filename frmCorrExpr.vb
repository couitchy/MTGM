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
'| Auteur         |                          Couitchy |
'|----------------------------------------------------|
'| Modifications :                                    |
'------------------------------------------------------
Public Partial Class frmCorrExpr
	Private VmFormMove As Boolean = False	'Formulaire en déplacement
	Private VmMousePos As Point				'Position initiale de la souris sur la barre de titre
	Private VmCanClose As Boolean = False   'Formulaire peut être fermé
	Private VmOwner As frmSimu
	Private VmBusy As Boolean = False
	Public Sub New(VpOwner As frmSimu)
		Me.InitializeComponent()
		VmOwner = VpOwner
	End Sub
	Private Sub SetCheckState
	Dim VpAll As Boolean = True
	Dim VpNone As Boolean = True
		If VmBusy Then Exit Sub
		VmBusy = True
		For VpI As Integer = 0 To Me.chklstExpr.Items.Count - 1
			VpAll = VpAll And Me.chklstExpr.GetItemChecked(VpI)
			VpNone = VpNone And (Not Me.chklstExpr.GetItemChecked(VpI))
		Next VpI
		If VpAll Then
			Me.chkAllNone.CheckState = CheckState.Checked
		ElseIf VpNone Then
			Me.chkAllNone.CheckState = CheckState.Unchecked
		Else
			Me.chkAllNone.CheckState = CheckState.Indeterminate
		End If		
		VmBusy = False				
	End Sub
	Sub CbarExprMouseDown(sender As Object, e As MouseEventArgs)
		VmFormMove = True
		VmCanClose = True
		VmMousePos = New Point(e.X, e.Y)
	End Sub
	Sub CbarExprMouseMove(sender As Object, e As MouseEventArgs)
		If VmFormMove Then
			Me.Location = New Point(MousePosition.X - VmMousePos.X, MousePosition.Y - VmMousePos.Y)
		End If
	End Sub
	Sub CbarExprMouseUp(sender As Object, e As MouseEventArgs)
		VmFormMove = False
	End Sub
	Sub CbarExprVisibleChanged(sender As Object, e As EventArgs)
		If VmCanClose Then
			Me.Close
		End If
	End Sub
	Sub CmdOkClick(sender As Object, e As EventArgs)
	Dim VpToRemove As New ArrayList
		For VpI As Integer = 0 To Me.chklstExpr.Items.Count - 1
			If Not Me.chklstExpr.GetItemChecked(VpI) Then
				For Each VpCorr As clsCorrelation In VmOwner.Expressions
					If Me.chklstExpr.Items.Item(VpI).ToString = VpCorr.Seq Then
						VpToRemove.Add(VpCorr)
					End If
				Next VpCorr
			End If
		Next VpI
		For Each VpCorr As clsCorrelation In VpToRemove
			VmOwner.Expressions.Remove(VpCorr)
		Next VpCorr
		Me.Close
	End Sub
	Sub FrmCorrExprLoad(sender As Object, e As EventArgs)
		VmOwner.Expressions.Sort(New clsCorrelationComparer)
		For Each VpCorr As clsCorrelation In VmOwner.Expressions
			If VpCorr.Seq.Trim <> "" AndAlso Not Me.chklstExpr.Items.Contains(VpCorr.Seq) Then
				Me.chklstExpr.Items.Add(VpCorr.Seq, True)
			End If
		Next VpCorr
	End Sub
	Sub CmdRemoveClick(sender As Object, e As EventArgs)
		Call Me.InclureExclure("Désélectionner les expressions qui contiennent...", False)
	End Sub
	Sub ChkAllNoneCheckedChanged(sender As Object, e As EventArgs)
		If VmBusy Then Exit Sub
		VmBusy = True
		For VpI As Integer = 0 To Me.chklstExpr.Items.Count - 1
			Me.chklstExpr.SetItemChecked(VpI, Me.chkAllNone.Checked)
		Next VpI
		VmBusy = False
	End Sub
	Sub ChklstExprSelectedValueChanged(sender As Object, e As EventArgs)
		Call Me.SetCheckState
	End Sub
	Sub CmdKeepClick(sender As Object, e As EventArgs)
		Call Me.InclureExclure("Sélectionner les expressions qui contiennent...", True)
	End Sub
	Sub InclureExclure(VpCaption As String, VpState As Boolean)
	Dim VpStr As String = InputBox(VpCaption, "Ajustement des expressions", "(Expression)")
		If VpStr.Trim <> "" Then
			For VpI As Integer = 0 To Me.chklstExpr.Items.Count - 1
				If Me.chklstExpr.Items.Item(VpI).ToLower.Contains(VpStr.ToLower) Then
					Me.chklstExpr.SetItemChecked(VpI, VpState)
				End If
			Next VpI
			Call Me.SetCheckState
		End If			
	End Sub
End Class