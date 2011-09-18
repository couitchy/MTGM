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
'| Auteur         |                       VadimBerman |
'|----------------------------------------------------|
'| Modifications :                                    |
'------------------------------------------------------

Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Design
Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Windows.Forms

#Region " Filename Editor "

Public Class UIFilenameEditor
    Inherits System.Drawing.Design.UITypeEditor

    Public Overloads Overrides Function GetEditStyle(ByVal context As _
                    ITypeDescriptorContext) As UITypeEditorEditStyle
        If Not context Is Nothing AndAlso Not context.Instance Is Nothing Then
            Return UITypeEditorEditStyle.Modal
        End If
        Return UITypeEditorEditStyle.None
    End Function

    <RefreshProperties(RefreshProperties.All)> _
    Public Overloads Overrides Function EditValue( _
                ByVal context As ITypeDescriptorContext, _
                ByVal provider As System.IServiceProvider, _
                ByVal value As [Object]) As [Object]
        If context Is Nothing OrElse provider Is Nothing _
                OrElse context.Instance Is Nothing Then
            Return MyBase.EditValue(provider, value)
        End If

        Dim fileDlg As FileDialog
        If context.PropertyDescriptor.Attributes(GetType(SaveFileAttribute)) Is Nothing Then
            fileDlg = New OpenFileDialog
        Else
            fileDlg = New SaveFileDialog
        End If
        fileDlg.Title = "Select " & context.PropertyDescriptor.DisplayName
        fileDlg.FileName = value

        Dim filterAtt As FileDialogFilterAttribute = _
            context.PropertyDescriptor.Attributes(GetType(FileDialogFilterAttribute))
        If Not filterAtt Is Nothing Then fileDlg.Filter = filterAtt.Filter
        If fileDlg.ShowDialog() = DialogResult.OK Then
            value = fileDlg.FileName
        End If
        fileDlg.Dispose()
        Return value
    End Function
End Class

#Region " Filter attribute "
<AttributeUsage(AttributeTargets.Property)> _
Public Class FileDialogFilterAttribute
    Inherits Attribute
    Private _filter As String

    '''-----------------------------------------------------------------------------
    ''' <summary>
    ''' The filter to use in the file dialog in UIFilenameEditor.
    ''' </summary>
    ''' <value></value>
    ''' <remarks>The following is an example of a filter string: "Text files (*.txt)|*.txt|All files (*.*)|*.*"
    ''' </remarks>
    ''' <history>
    ''' 	[Vadim] 	30/12/2003	Created
    ''' </history>
    '''-----------------------------------------------------------------------------
    Public ReadOnly Property Filter() As String
        Get
            Return Me._filter
        End Get
    End Property

    '''-----------------------------------------------------------------------------
    ''' <summary>
    ''' Define a filter for the UIFilenameEditor.
    ''' </summary>
    ''' <param name="filter">The filter to use in the file dialog in UIFilenameEditor. The following is an example of a filter string: "Text files (*.txt)|*.txt|All files (*.*)|*.*"</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 	[Vadim] 	30/12/2003	Created
    ''' </history>
    '''-----------------------------------------------------------------------------
    Public Sub New(ByVal filter As String)
        MyBase.New()
        Me._filter = filter
    End Sub
End Class
#End Region

#Region " 'Save file' attribute - indicates that SaveFileDialog must be shown "
<AttributeUsage(AttributeTargets.Property)> _
Public Class SaveFileAttribute
    Inherits Attribute
End Class
#End Region

#End Region


