Public Class clsPlateauDrawings
    Private VmPictures As New List(Of PictureBox)
    Private VmCurrentPicture As PictureBox
    Private Shared VmDragMode As Boolean
    Private Shared VmDraggedPicture As PictureBox
    Private Shared VmAttrib As New Imaging.ImageAttributes
    Public Sub New
    Dim VpColorMatrix As New Imaging.ColorMatrix
        VmDraggedPicture = New PictureBox
        VmDraggedPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        VmDraggedPicture.BackColor = Color.Transparent
        With VpColorMatrix
            .Matrix00 = 1
            .Matrix11 = 1
            .Matrix22 = 1
            .Matrix33 = 0.5
            .Matrix44 = 1
        End With
        VmAttrib.SetColorMatrix(VpColorMatrix)
    End Sub
    Public Function GetRightBorder(VpCard As clsPlateauCard) As Integer
        For Each VpPicture As PictureBox In VmPictures
            If CType(VpPicture.Tag, clsPlateauCard) Is VpCard Then
                Return VpPicture.Left + VpPicture.Width
            End If
        Next VpPicture
        Return Nothing
    End Function
    Public Sub StopDragging
        VmDraggedPicture.Visible = False
        VmDraggedPicture.Image = Nothing
    End Sub
    Public Property Pictures As List(Of PictureBox)
        Get
            Return VmPictures
        End Get
        Set (VpPictures As List(Of PictureBox))
            VmPictures = VpPictures
        End Set
    End Property
    Public Property CurrentPicture As PictureBox
        Get
            Return VmCurrentPicture
        End Get
        Set (VpCurrentPicture As PictureBox)
            VmCurrentPicture = VpCurrentPicture
        End Set
    End Property
    Public ReadOnly Property CurrentCard As clsPlateauCard
        Get
            Return VmCurrentPicture.Tag
        End Get
    End Property
    Public Property DragMode As Boolean
        Get
            Return VmDragMode
        End Get
        Set (VpDragMode As Boolean)
            VmDragMode = VpDragMode
            If VmDragMode Then
                VmDraggedPicture.Visible = True
            End If
        End Set
    End Property
    Public Shared ReadOnly Property DraggedPicture As PictureBox
        Get
            Return VmDraggedPicture
        End Get
    End Property
    Public ReadOnly Property Opacity As Imaging.ImageAttributes
        Get
            Return VmAttrib
        End Get
    End Property
End Class
