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
Public Partial Class frmCalendar
	Public Sub New()
		Me.InitializeComponent()
	End Sub
	Sub FrmCalendarLoad(ByVal sender As Object, ByVal e As EventArgs)
		Me.Left = Control.MousePosition.X
		Me.Top = Control.MousePosition.Y	
		Me.cal.SelectionStart = DateTime.Now
		Me.tmrAffiche.Start
	End Sub
	Sub FrmCalendarKeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs)
		If Asc(e.KeyChar) = Keys.Escape Then				
			Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
		ElseIf Asc(e.KeyChar) = Keys.Enter Then
			Me.DialogResult = System.Windows.Forms.DialogResult.OK
		End If
	End Sub
	Sub CalDateSelected(ByVal sender As Object, ByVal e As DateRangeEventArgs)
		Me.DialogResult = System.Windows.Forms.DialogResult.OK
	End Sub
	Sub TmrAfficheTick(ByVal sender As Object, ByVal e As EventArgs)
		Me.Opacity = Me.Opacity + 0.01
		If Me.Opacity = 1 Then
			Me.tmrAffiche.Stop				
		End If	
	End Sub
End Class
