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
Imports NPlot
Public Partial Class frmGrapher
	Private VmColors() As String
	Public Sub New()
		Me.InitializeComponent()
		VmColors = [Enum].GetNames(GetType(KnownColor))
		Me.plotMain.Tag = 0		
	End Sub
	Public Sub AddNewPlot(VpTable As Hashtable, VpLabel As String)
	Dim VpX As New ArrayList
	Dim VpY As New ArrayList
	Dim VpPlot As New LinePlot
		For Each VpI As Integer In VpTable.Keys
			VpX.Add(VpI)
			VpY.Add(VpTable.Item(VpI))
		Next VpI
		VpPlot.Label = VpLabel
		VpPlot.DataSource = VpY
		VpPlot.AbscissaData = VpX
		VpPlot.Color = Color.FromName(VmColors(46 + Me.plotMain.Tag))
		Me.plotMain.Add(VpPlot)
		Me.plotMain.Tag = Me.plotMain.Tag + 1
		If Me.plotMain.Tag = 1 Then
			Me.plotMain.AddInteraction(New Windows.PlotSurface2D.Interactions.RubberBandSelection)
		End If
		Me.plotMain.Legend = New Legend
		Me.plotMain.Legend.AttachTo(PlotSurface2D.XAxisPosition.Bottom, PlotSurface2D.YAxisPosition.Right)
		Me.plotMain.Legend.VerticalEdgePlacement = Legend.Placement.Inside
		Me.plotMain.Legend.HorizontalEdgePlacement = Legend.Placement.Inside
		Me.plotMain.Refresh	
	End Sub
	Sub PlotMainMouseDown(sender As Object, e As MouseEventArgs)
		If e.Button = System.Windows.Forms.MouseButtons.Right Then
			Me.plotMain.OriginalDimensions
		End If		
	End Sub
	Sub FrmGrapherFormClosing(sender As Object, e As FormClosingEventArgs)
		If e.CloseReason = CloseReason.UserClosing Then
			e.Cancel = True
			Me.Hide
			Me.plotMain.Clear
			Me.plotMain.Tag = 0	
		End If
	End Sub
End Class
