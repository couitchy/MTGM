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
'| Auteur         |                          Couitchy |
'|----------------------------------------------------|
'| Modifications :                                    |
'------------------------------------------------------
Public Partial Class frmReserve
	Private VmFormMove As Boolean = False	'Formulaire en déplacement
	Private VmMousePos As Point				'Position initiale de la souris sur la barre de titre
	Private VmCanClose As Boolean = False   'Formulaire peut être fermé	
	Public Sub New()
		Me.InitializeComponent()
	End Sub
	Sub CbarReserveMouseDown(sender As Object, e As MouseEventArgs)
		VmFormMove = True
		VmMousePos = New Point(e.X, e.Y)		
	End Sub
	Sub CbarReserveMouseMove(sender As Object, e As MouseEventArgs)
		If VmFormMove Then
			Me.Location = New Point(MousePosition.X - VmMousePos.X, MousePosition.Y - VmMousePos.Y)
		End If		
	End Sub
	Sub CbarReserveMouseUp(sender As Object, e As MouseEventArgs)
		VmFormMove = False		
	End Sub
	Sub CbarReserveVisibleChanged(sender As Object, e As EventArgs)
		If VmCanClose AndAlso Not Me.cbarReserve.Visible Then
			Me.Close
		End If		
	End Sub
End Class
