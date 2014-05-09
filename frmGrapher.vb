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
'| Auteur         |                          Couitchy |
'|----------------------------------------------------|
'| Modifications :                                    |
'| - checkedlistbox et propertygrid		   23/10/2010 |
'------------------------------------------------------
Imports NPlot
Imports System.Drawing
Imports System.ComponentModel
Public Partial Class frmGrapher
	Private VmColors() As String
	Private VmPlots As New List(Of clsGrapherSettings)
	Private VmBusy As Boolean = False
	Private Const CmColorBase As Integer = 46
	Public Sub New()
		Me.InitializeComponent()
		VmColors = [Enum].GetNames(GetType(KnownColor))
		Me.splitH.Panel2Collapsed = True
	End Sub
	Public Sub AddNewPlot(VpTable As SortedList, VpLabel As String)
	'----------------------------------------------------------
	'Ajoute le graphe passé en paramètre à la collection locale
	'----------------------------------------------------------
	Dim VpX As New ArrayList
	Dim VpY As New ArrayList
	Dim VpPlot As New LinePlot
		For Each VpI As Object In VpTable.Keys
			VpX.Add(VpI)
			VpY.Add(VpTable.Item(VpI))
		Next VpI
		VpPlot.Label = VpLabel
		VpPlot.DataSource = VpY
		VpPlot.AbscissaData = VpX
		VpPlot.Color = Color.FromName(VmColors(CmColorBase + VmPlots.Count))
		VmPlots.Add(New clsGrapherSettings(Me, VpLabel, VpPlot.Color, Drawing2D.DashStyle.Solid, 2, True, VpPlot))
		Call Me.RefreshAllPlots
	End Sub
	Public Sub RefreshAllPlots(Optional VpList As Boolean = True)
	'---------------------------------------------------------------------------
	'Rafraîchit l'affichage de l'ensemble des graphiques de la collection locale
	'---------------------------------------------------------------------------
	Dim VpSel As Integer = Me.chklstCurves.SelectedIndex
	Dim VpOneAtLeast As Boolean = False
		VmBusy = True
		With Me.plotMain
			.Clear
			If VpList Then
				Me.chklstCurves.Items.Clear
			End If
			For Each VpPlot As clsGrapherSettings In VmPlots
				If VpPlot.myVisible Then
					.Add(VpPlot.RefPlot)
					VpOneAtLeast = True
				End If
				If VpList Then
					Me.chklstCurves.Items.Add(VpPlot.Legende, VpPlot.myVisible)
				End If
			Next VpPlot
			If VpOneAtLeast Then
				.AddInteraction(New Windows.PlotSurface2D.Interactions.RubberBandSelection)
				.Legend = New Legend
				.Legend.AttachTo(PlotSurface2D.XAxisPosition.Top, PlotSurface2D.YAxisPosition.Left)
				.Legend.VerticalEdgePlacement = Legend.Placement.Inside
				.Legend.HorizontalEdgePlacement = Legend.Placement.Inside
				.YAxis1.WorldMin = CDbl(CInt(Me.GetExtremum(False) * (1 - clsModule.CgGraphsExtraMargin) - 0.5))
				.YAxis1.WorldMax = CDbl(CInt(Me.GetExtremum(True)  * (1 + clsModule.CgGraphsExtraMargin) + 0.5))
				Me.chklstCurves.SelectedIndex = Math.Max(0, VpSel)
			End If
			.Refresh
		End With
		VmBusy = False
	End Sub
	Private Function GetExtremum(VpMaximum As Boolean) As Single
	Dim VpExtremum As Single = If(VpMaximum, Single.MinValue, Single.MaxValue)
		For Each VpPlot As clsGrapherSettings In VmPlots
			If VpPlot.myVisible
				For Each VpY As Single In VpPlot.RefPlot.DataSource
					If (VpY > VpExtremum And VpMaximum) Or (VpY < VpExtremum And Not VpMaximum) Then
						VpExtremum = VpY
					End If
				Next VpY
			End If
		Next VpPlot		
		Return VpExtremum
	End Function
	Private Function SaveBMP As Bitmap
	'---------------
	'Capture d'écran
	'---------------
	Dim VpBMP As Bitmap
	Dim VpGR As Graphics 
	Dim VpOrig As Point = Me.plotMain.PointToScreen(Me.plotMain.Location)
		VpBMP = New Bitmap(Me.plotMain.Width, Me.plotMain.Height, Imaging.PixelFormat.Format32bppArgb)
		VpGR = Graphics.FromImage(VpBMP)              
		Application.DoEvents
		VpGR.CopyFromScreen(VpOrig, New Point(0, 0), Me.Size, CopyPixelOperation.SourceCopy)
		Return VpBMP
	End Function	
	Public ReadOnly Property GraphsCount As Integer
		Get
			Return VmPlots.Count
		End Get
	End Property
	Sub PlotMainMouseDown(sender As Object, e As MouseEventArgs)
		If e.Button = System.Windows.Forms.MouseButtons.Right Then
			Me.plotMain.OriginalDimensions
		End If		
	End Sub
	Sub FrmGrapherFormClosing(sender As Object, e As FormClosingEventArgs)
		If e.CloseReason = CloseReason.UserClosing Then
			e.Cancel = True
			Me.Hide
			If VmPlots.Count > 0 Then
				Me.chklstCurves.SelectedIndex = 0
			End If
			Me.plotMain.Clear
			VmPlots.Clear
		End If
	End Sub	
	Sub BtCaptureClick(sender As Object, e As EventArgs)
	Dim VpBMP As Bitmap = Me.SaveBMP
		Me.dlgCapture.FileName = ""
		Me.dlgCapture.ShowDialog
		If Me.dlgCapture.FileName <> "" Then
			VpBMP.Save(Me.dlgCapture.FileName, Imaging.ImageFormat.Png)
		End If
	End Sub
	Sub BtEditClick(sender As Object, e As EventArgs)
		Me.splitH.Panel2Collapsed = Not Me.btEdit.Checked
	End Sub	
	Sub BtClearClick(sender As Object, e As EventArgs)
		VmPlots.Clear
		Me.propCurves.SelectedObject = Nothing
		Call Me.RefreshAllPlots
	End Sub
	Sub ChklstCurvesSelectedIndexChanged(sender As Object, e As EventArgs)
		If Me.chklstCurves.Items.Count > 0 AndAlso Me.chklstCurves.SelectedIndex >= 0 Then
			Me.propCurves.SelectedObject = VmPlots.Item(Me.chklstCurves.SelectedIndex)
		Else
			Me.propCurves.SelectedObject = Nothing
		End If		
	End Sub
	Sub ChklstCurvesItemCheck(sender As Object, e As ItemCheckEventArgs)
		If Not VmBusy AndAlso Me.chklstCurves.SelectedIndex >= 0 Then
			VmPlots.Item(Me.chklstCurves.SelectedIndex).myVisible = e.NewValue
			Call Me.RefreshAllPlots(False)
		End If		
	End Sub
	Sub BtCoordsClick(sender As Object, e As EventArgs)
		Me.btCoords.Checked = Not Me.btCoords.Checked
		Me.plotMain.ShowCoordinates = Me.btCoords.Checked
	End Sub
End Class
Public Class clsGrapherSettings
	Private VmOwner As frmGrapher
	Private VmLegende As String
	Private VmColor As Color
	Private VmStyle As Drawing2D.DashStyle
	Private VmThickness As Single
	Private VmRefPlot As LinePlot
	Private VmVisible As Boolean
	Public Sub New(VpOwner As frmGrapher, VpLegende As String, VpColor As Color, VpStyle As Drawing2D.DashStyle, VpThickness As Single, VpVisible As Boolean, VpPlot As LinePlot)
		VmOwner = VpOwner
		VmLegende = VpLegende
		VmColor = VpColor
		VmStyle = VpStyle
		VmThickness = VpThickness
		VmVisible = VpVisible
		VmRefPlot = VpPlot
	End Sub
	<DisplayName("Légende"), Description("Texte apparaissant dans la légende du graphique")> _
	Public Property Legende As String
		Get
			Return VmLegende
		End Get
		Set (VpLegende As String)
			VmLegende = VpLegende
			VmRefPlot.Label = VpLegende
			Call VmOwner.RefreshAllPlots(False)
		End Set
	End Property
	<DisplayName("Couleur"), Description("Couleur de la courbe")> _
	Public Property myColor As Color
		Get
			Return VmColor
		End Get
		Set (VpColor As Color)
			VmColor = VpColor
			VmRefPlot.Color = VpColor
			Call VmOwner.RefreshAllPlots(False)
		End Set
	End Property
	<DisplayName("Style"), DefaultValue(Drawing2D.DashStyle.Solid), Description("Style du tracé de la courbe")> _
	Public Property myStyle As Drawing2D.DashStyle
		Get
			Return VmStyle
		End Get
		Set (VpStyle As Drawing2D.DashStyle)
			VmStyle = VpStyle
			VmRefPlot.Pen.DashStyle = VpStyle
			Call VmOwner.RefreshAllPlots(False)
		End Set
	End Property
	<DisplayName("Epaisseur"), DefaultValue(1), Description("Epaisseur de la courbe")> _
	Public Property Thickness As Single
		Get
			Return VmThickness
		End Get
		Set (VpThickness As Single)
			VmThickness = VpThickness
			VmRefPlot.Pen.Width = VpThickness
			Call VmOwner.RefreshAllPlots(False)
		End Set
	End Property
	<Browsable(False)> _
	Public Property myVisible As Boolean
		Get
			Return VmVisible
		End Get
		Set (VpVisible As Boolean)
			VmVisible = VpVisible
		End Set
	End Property
	<Browsable(False)> _
	Public ReadOnly Property RefPlot As LinePlot
		Get
			Return VmRefPlot
		End Get
	End Property
End Class