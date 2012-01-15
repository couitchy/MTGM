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
'| Auteur         |                     Mav Northwind |
'|----------------------------------------------------|
'| Modifications :                                    |
'------------------------------------------------------

Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Drawing
Imports System.Data
Imports System.Windows.Forms
Imports System.Runtime.InteropServices

''' <summary>
''' Event Args for SubItemClicked event
''' </summary>
Public Class SubItemEventArgs
	Inherits EventArgs
	Public Sub New(item As ListViewItem, subItem As Integer)
		_subItemIndex = subItem
		_item = item
	End Sub
	Private _subItemIndex As Integer = -1
	Private _item As ListViewItem = Nothing
	Public ReadOnly Property SubItem() As Integer
		Get
			Return _subItemIndex
		End Get
	End Property
	Public ReadOnly Property Item() As ListViewItem
		Get
			Return _item
		End Get
	End Property
End Class


''' <summary>
''' Event Args for SubItemEndEditingClicked event
''' </summary>
Public Class SubItemEndEditingEventArgs
	Inherits SubItemEventArgs
	Private _text As String = String.Empty
	Private _cancel As Boolean = True

	Public Sub New(item As ListViewItem, subItem As Integer, display As String, cancel As Boolean)
		MyBase.New(item, subItem)
		_text = display
		_cancel = cancel
	End Sub
	Public Property DisplayText() As String
		Get
			Return _text
		End Get
		Set
			_text = value
		End Set
	End Property
	Public Property Cancel() As Boolean
		Get
			Return _cancel
		End Get
		Set
			_cancel = value
		End Set
	End Property
End Class


'''	<summary>
'''	Inherited ListView to allow in-place editing of subitems
'''	</summary>
Public Class ExListView
	Inherits System.Windows.Forms.ListView
	#Region "Interop structs, imports and constants"
	''' <summary>
	''' MessageHeader for WM_NOTIFY
	''' </summary>
	Private Structure NMHDR
		Public hwndFrom As IntPtr
		Public idFrom As Int32
		Public code As Int32
	End Structure


	<DllImport("user32.dll")> _
	Private Shared Function SendMessage(hWnd As IntPtr, msg As Integer, wPar As IntPtr, lPar As IntPtr) As IntPtr
	End Function
	<DllImport("user32.dll", CharSet := CharSet.Ansi)> _
	Private Shared Function SendMessage(hWnd As IntPtr, msg As Integer, len As Integer, ByRef order As Integer()) As IntPtr
	End Function

	' ListView messages
	Private Const LVM_FIRST As Integer = &H1000
	Private Const LVM_GETCOLUMNORDERARRAY As Integer = (LVM_FIRST + 59)

	' Windows Messages that will abort editing
	Private Const WM_HSCROLL As Integer = &H114
	Private Const WM_VSCROLL As Integer = &H115
	Private Const WM_SIZE As Integer = &H5
	Private Const WM_NOTIFY As Integer = &H4e

	Private Const HDN_FIRST As Integer = -300
	Private Const HDN_BEGINDRAG As Integer = (HDN_FIRST - 10)
	Private Const HDN_ITEMCHANGINGA As Integer = (HDN_FIRST - 0)
	Private Const HDN_ITEMCHANGINGW As Integer = (HDN_FIRST - 20)
	#End Region

	'''	<summary>
	'''	Required designer variable.
	'''	</summary>
	Private components As System.ComponentModel.Container = Nothing
	
	''' <summary>
	''' Event Handler for SubItem events
	''' </summary>
	Public Delegate Sub SubItemEventHandler(sender As Object, e As SubItemEventArgs)
	''' <summary>
	''' Event Handler for SubItemEndEditing events
	''' </summary>
	Public Delegate Sub SubItemEndEditingEventHandler(sender As Object, e As SubItemEndEditingEventArgs)

	Public Event SubItemClicked As SubItemEventHandler
	Public Event SubItemBeginEditing As SubItemEventHandler
	Public Event SubItemEndEditing As SubItemEndEditingEventHandler

	Public Sub New()
		' This	call is	required by	the	Windows.Forms Form Designer.
		InitializeComponent()

		MyBase.FullRowSelect = True
		MyBase.View = View.Details
		MyBase.AllowColumnReorder = True
	End Sub

	'''	<summary>
	'''	Clean up any resources being used.
	'''	</summary>
	Protected Overloads Overrides Sub Dispose(disposing As Boolean)
		If disposing Then
			If components IsNot Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(disposing)
	End Sub

	#Region "Component	Designer generated code"
	'''	<summary>
	'''	Required method	for	Designer support - do not modify 
	'''	the	contents of	this method	with the code editor.
	'''	</summary>
	Private Sub InitializeComponent()
		components = New System.ComponentModel.Container()
	End Sub
	#End Region

	Private _doubleClickActivation As Boolean = False
	''' <summary>
	''' Is a double click required to start editing a cell?
	''' </summary>
	Public Property DoubleClickActivation() As Boolean
		Get
			Return _doubleClickActivation
		End Get
		Set
			_doubleClickActivation = value
		End Set
	End Property


	''' <summary>
	''' Retrieve the order in which columns appear
	''' </summary>
	''' <returns>Current display order of column indices</returns>
	Public Function GetColumnOrder() As Integer()
		Dim lPar As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(Integer)) * Columns.Count)

		Dim res As IntPtr = SendMessage(Handle, LVM_GETCOLUMNORDERARRAY, New IntPtr(Columns.Count), lPar)
		If res.ToInt32() = 0 Then
			' Something went wrong
			Marshal.FreeHGlobal(lPar)
			Return Nothing
		End If

		Dim order As Integer() = New Integer(Columns.Count - 1) {}
		Marshal.Copy(lPar, order, 0, Columns.Count)

		Marshal.FreeHGlobal(lPar)

		Return order
	End Function


	''' <summary>
	''' Find ListViewItem and SubItem Index at position (x,y)
	''' </summary>
	''' <param name="x">relative to ListView</param>
	''' <param name="y">relative to ListView</param>
	''' <param name="item">Item at position (x,y)</param>
	''' <returns>SubItem index</returns>
	Public Function GetSubItemAt(x As Integer, y As Integer, ByRef item As ListViewItem) As Integer
		item = Me.GetItemAt(x, y)

		If item IsNot Nothing Then
			Dim order As Integer() = GetColumnOrder()
			Dim lviBounds As Rectangle
			Dim subItemX As Integer

			lviBounds = item.GetBounds(ItemBoundsPortion.Entire)
			subItemX = lviBounds.Left
			For i As Integer = 0 To order.Length - 1
				Dim h As ColumnHeader = Me.Columns(order(i))
				If x < subItemX + h.Width Then
					Return h.Index
				End If
				subItemX += h.Width
			Next
		End If

		Return -1
	End Function


	''' <summary>
	''' Get bounds for a SubItem
	''' </summary>
	''' <param name="Item">Target ListViewItem</param>
	''' <param name="SubItem">Target SubItem index</param>
	''' <returns>Bounds of SubItem (relative to ListView)</returns>
	Public Function GetSubItemBounds(Item As ListViewItem, SubItem As Integer) As Rectangle
		Dim order As Integer() = GetColumnOrder()

		Dim subItemRect As Rectangle = Rectangle.Empty
		If SubItem >= order.Length Then
			Throw New IndexOutOfRangeException("SubItem " & SubItem & " out of range")
		End If

		If Item Is Nothing Then
			Throw New ArgumentNullException("Item")
		End If

		Dim lviBounds As Rectangle = Item.GetBounds(ItemBoundsPortion.Entire)
		Dim subItemX As Integer = lviBounds.Left

		Dim col As ColumnHeader
		Dim i As Integer
		For i = 0 To order.Length - 1
			col = Me.Columns(order(i))
			If col.Index = SubItem Then
				Exit For
			End If
			subItemX += col.Width
		Next
		subItemRect = New Rectangle(subItemX, lviBounds.Top, Me.Columns(order(i)).Width, lviBounds.Height)
		Return subItemRect
	End Function


	Protected Overloads Overrides Sub WndProc(ByRef msg As Message)
		Select Case msg.Msg
			' Look	for	WM_VSCROLL,WM_HSCROLL or WM_SIZE messages.
			Case WM_VSCROLL, WM_HSCROLL, WM_SIZE
				EndEditing(False)
				Exit Select
			Case WM_NOTIFY
				' Look for WM_NOTIFY of events that might also change the
				' editor's position/size: Column reordering or resizing
				Dim h As NMHDR = CType(Marshal.PtrToStructure(msg.LParam, GetType(NMHDR)), NMHDR)
				If h.code = HDN_BEGINDRAG OrElse h.code = HDN_ITEMCHANGINGA OrElse h.code = HDN_ITEMCHANGINGW Then
					EndEditing(False)
				End If
				Exit Select
		End Select

		MyBase.WndProc(msg)
	End Sub


	#Region "Initialize editing depending of DoubleClickActivation property"
	Protected Overloads Overrides Sub OnMouseUp(e As System.Windows.Forms.MouseEventArgs)
		MyBase.OnMouseUp(e)

		If DoubleClickActivation Then
			Return
		End If

		EditSubitemAt(New Point(e.X, e.Y))
	End Sub

	Protected Overloads Overrides Sub OnDoubleClick(e As EventArgs)
		MyBase.OnDoubleClick(e)

		If Not DoubleClickActivation Then
			Return
		End If

		Dim pt As Point = Me.PointToClient(Cursor.Position)

		EditSubitemAt(pt)
	End Sub

	'''<summary>
	''' Fire SubItemClicked
	'''</summary>
	'''<param name="p">Point of click/doubleclick</param>
	Private Sub EditSubitemAt(p As Point)
		Dim item As ListViewItem = Nothing
		Dim idx As Integer = GetSubItemAt(p.X, p.Y, item)
		If idx >= 0 Then
			OnSubItemClicked(New SubItemEventArgs(item, idx))
		End If
	End Sub

	#End Region

	#Region "In-place editing functions"
	' The control performing the actual editing
	Private _editingControl As Control
	' The LVI being edited
	Private _editItem As ListViewItem
	' The SubItem being edited
	Private _editSubItem As Integer

	Protected Sub OnSubItemBeginEditing(e As SubItemEventArgs)
		RaiseEvent SubItemBeginEditing(Me, e)
	End Sub
	Protected Sub OnSubItemEndEditing(e As SubItemEndEditingEventArgs)
		RaiseEvent SubItemEndEditing(Me, e)
	End Sub
	Protected Sub OnSubItemClicked(e As SubItemEventArgs)
		RaiseEvent SubItemClicked(Me, e)
	End Sub


	''' <summary>
	''' Begin in-place editing of given cell
	''' </summary>
	''' <param name="c">Control used as cell editor</param>
	''' <param name="Item">ListViewItem to edit</param>
	''' <param name="SubItem">SubItem index to edit</param>
	Public Sub StartEditing(c As Control, Item As ListViewItem, SubItem As Integer)
		OnSubItemBeginEditing(New SubItemEventArgs(Item, SubItem))

		Dim rcSubItem As Rectangle = GetSubItemBounds(Item, SubItem)

		If rcSubItem.X < 0 Then
			' Left edge of SubItem not visible - adjust rectangle position and width
			rcSubItem.Width += rcSubItem.X
			rcSubItem.X = 0
		End If
		If rcSubItem.X + rcSubItem.Width > Me.Width Then
			' Right edge of SubItem not visible - adjust rectangle width
			rcSubItem.Width = Me.Width - rcSubItem.Left
		End If

		' Subitem bounds are relative to the location of the ListView!
		rcSubItem.Offset(Left, Top)

		' In case the editing control and the listview are on different parents,
		' account for different origins
		Dim origin As New Point(0, 0)
		Dim lvOrigin As Point = Me.Parent.PointToScreen(origin)
		Dim ctlOrigin As Point = c.Parent.PointToScreen(origin)

		rcSubItem.Offset(lvOrigin.X - ctlOrigin.X, lvOrigin.Y - ctlOrigin.Y)

		' Position and show editor
		c.Bounds = rcSubItem
		c.Text = Item.SubItems(SubItem).Text
		c.Visible = True
		c.BringToFront()
		c.Focus()

		_editingControl = c
		AddHandler _editingControl.Leave, New EventHandler(AddressOf _editControl_Leave)
		AddHandler _editingControl.KeyPress, New KeyPressEventHandler(AddressOf _editControl_KeyPress)

		_editItem = Item
		_editSubItem = SubItem
	End Sub


	Private Sub _editControl_Leave(sender As Object, e As EventArgs)
		' cell editor losing focus
		EndEditing(True)
	End Sub

	Private Sub _editControl_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs)
		Select Case e.KeyChar
			Case ChrW(CInt(Keys.Escape))
				If True Then
					EndEditing(False)
					Exit Select
				End If

			Case ChrW(CInt(Keys.Enter))
				If True Then
					EndEditing(True)
					Exit Select
				End If
		End Select
	End Sub

	''' <summary>
	''' Accept or discard current value of cell editor control
	''' </summary>
	''' <param name="AcceptChanges">Use the _editingControl's Text as new SubItem text or discard changes?</param>
	Public Sub EndEditing(AcceptChanges As Boolean)
		If _editingControl Is Nothing Then
			Return
		End If

		' The item being edited
		' The subitem index being edited
		' Use editControl text if changes are accepted
		' or the original subitem's text, if changes are discarded
			' Cancel?
		Dim e As New SubItemEndEditingEventArgs(_editItem, _editSubItem, If(AcceptChanges, _editingControl.Text, _editItem.SubItems(_editSubItem).Text), Not AcceptChanges)

		OnSubItemEndEditing(e)

		_editItem.SubItems(_editSubItem).Text = e.DisplayText

		RemoveHandler _editingControl.Leave, New EventHandler(AddressOf _editControl_Leave)
		RemoveHandler _editingControl.KeyPress, New KeyPressEventHandler(AddressOf _editControl_KeyPress)

		_editingControl.Visible = False

		_editingControl = Nothing
		_editItem = Nothing
		_editSubItem = -1
	End Sub
	#End Region
End Class