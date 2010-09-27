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
Imports NPlot
Imports System.Drawing
Public Partial Class frmGrapher
	Private VmColors() As String
	Private VmPlots As New ArrayList
	Private Const CmColorBase As Integer = 46
	Public Sub New()
		Me.InitializeComponent()
		VmColors = [Enum].GetNames(GetType(KnownColor))
		For Each VpStyle As String In [Enum].GetNames(GetType(System.Drawing.Drawing2D.DashStyle))
			Me.cboStyle.Items.Add(VpStyle)
		Next VpStyle
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
		VmPlots.Add(VpPlot)
		Call Me.RefreshAllPlots
	End Sub
	Private Sub RefreshAllPlots
	'---------------------------------------------------------------------------
	'Rafraîchit l'affichage de l'ensemble des graphiques de la collection locale
	'---------------------------------------------------------------------------
		Me.plotMain.Clear
		Me.cboPlots.Items.Clear
		For Each VpPlot As LinePlot In VmPlots
			Me.plotMain.Add(VpPlot)
			Me.cboPlots.Items.Add(VpPlot.Label)	
		Next VpPlot
		If VmPlots.Count > 0 Then
			Me.plotMain.AddInteraction(New Windows.PlotSurface2D.Interactions.RubberBandSelection)
			Me.plotMain.Legend = New Legend
			Me.plotMain.Legend.AttachTo(PlotSurface2D.XAxisPosition.Top, PlotSurface2D.YAxisPosition.Left)
			Me.plotMain.Legend.VerticalEdgePlacement = Legend.Placement.Inside
			Me.plotMain.Legend.HorizontalEdgePlacement = Legend.Placement.Inside
			Me.cboPlots.SelectedIndex = 0
		End If
		Me.plotMain.Refresh
	End Sub
	Private Function GetSelectedPlot As LinePlot
	'--------------------------------------------------
	'Retourne l'objet graphique actuellemen sélectionné
	'--------------------------------------------------
		For Each VpPlot As LinePlot In VmPlots
			If VpPlot.Label = Me.cboPlots.SelectedItem Then
				Return VpPlot
			End If
		Next VpPlot
		Return Nothing
	End Function
	Public Function SaveBMP As Bitmap
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
		Call Me.RefreshAllPlots
	End Sub
	Sub CmdOkClick(sender As Object, e As EventArgs)
	'-----------------------------------------------
	'Application des nouveaux paramètres d'affichage
	'-----------------------------------------------
	Dim VpPlot As LinePlot = Me.GetSelectedPlot
	Dim VpIndex As Integer = Me.cboPlots.SelectedIndex
		If Not VpPlot Is Nothing Then
			VpPlot.Label = Me.txtLegend.Text
			VpPlot.Color = Me.lblColorPick.BackColor
			VpPlot.Pen.DashStyle = Me.cboStyle.SelectedIndex
			VpPlot.Pen.Width = Val(Me.txtWidth.Text.Replace(",", "."))
			Call Me.RefreshAllPlots
			Me.cboPlots.SelectedIndex = VpIndex
		End If
	End Sub
	Sub CboPlotsSelectedIndexChanged(sender As Object, e As EventArgs)
	Dim VpPlot As LinePlot = Me.GetSelectedPlot
		Me.txtLegend.Text = VpPlot.Label
		Me.lblColorPick.BackColor = VpPlot.Color
		Me.cboStyle.Text = VpPlot.Pen.DashStyle.ToString
		Me.txtWidth.Text = VpPlot.Pen.Width.ToString
	End Sub
	Sub LblColorPickClick(sender As Object, e As EventArgs)
		Me.dlgColor.ShowDialog
		Me.lblColorPick.BackColor = Me.dlgColor.Color
	End Sub
	Sub CmdDelClick(sender As Object, e As EventArgs)
		VmPlots.Remove(Me.GetSelectedPlot)
		Call Me.RefreshAllPlots
	End Sub
End Class