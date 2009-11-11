'
' Created by SharpDevelop.
' User: Couitchy
' Date: 13/10/2008
' Time: 19:25
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Partial Class frmCalendar
	Inherits System.Windows.Forms.Form
	
	''' <summary>
	''' Designer variable used to keep track of non-visual components.
	''' </summary>
	Private components As System.ComponentModel.IContainer
	
	''' <summary>
	''' Disposes resources used by the form.
	''' </summary>
	''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	Protected Overrides Sub Dispose(ByVal disposing As Boolean)
		If disposing Then
			If components IsNot Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(disposing)
	End Sub
	
	''' <summary>
	''' This method is required for Windows Forms designer support.
	''' Do not change the method contents inside the source code editor. The Forms designer might
	''' not be able to load this method if it was changed manually.
	''' </summary>
	Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container
		Me.cal = New System.Windows.Forms.MonthCalendar
		Me.tmrAffiche = New System.Windows.Forms.Timer(Me.components)
		Me.SuspendLayout
		'
		'cal
		'
		Me.cal.Dock = System.Windows.Forms.DockStyle.Fill
		Me.cal.Location = New System.Drawing.Point(0, 0)
		Me.cal.Name = "cal"
		Me.cal.TabIndex = 0
		AddHandler Me.cal.DateSelected, AddressOf Me.CalDateSelected
		'
		'tmrAffiche
		'
		Me.tmrAffiche.Interval = 5
		AddHandler Me.tmrAffiche.Tick, AddressOf Me.TmrAfficheTick
		'
		'frmCalendar
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(180, 154)
		Me.Controls.Add(Me.cal)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
		Me.KeyPreview = true
		Me.Name = "frmCalendar"
		Me.Opacity = 0
		Me.Text = "frmCalendar"
		AddHandler KeyPress, AddressOf Me.FrmCalendarKeyPress
		AddHandler Load, AddressOf Me.FrmCalendarLoad
		Me.ResumeLayout(false)
	End Sub
	Private tmrAffiche As System.Windows.Forms.Timer
	Public cal As System.Windows.Forms.MonthCalendar
End Class
