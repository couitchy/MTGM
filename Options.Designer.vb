'
' Created by SharpDevelop.
' User: ${USER}
' Date: ${DATE}
' Time: ${TIME}
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Partial Class Options
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Options))
		Me.propOptions = New System.Windows.Forms.PropertyGrid
		Me.SuspendLayout
		'
		'propOptions
		'
		Me.propOptions.Dock = System.Windows.Forms.DockStyle.Fill
		Me.propOptions.Location = New System.Drawing.Point(0, 0)
		Me.propOptions.Name = "propOptions"
		Me.propOptions.Size = New System.Drawing.Size(538, 291)
		Me.propOptions.TabIndex = 0
		'
		'Options
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(538, 291)
		Me.Controls.Add(Me.propOptions)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
		Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
		Me.Name = "Options"
		Me.ShowInTaskbar = false
		Me.Text = "Options"
		AddHandler Load, AddressOf Me.OptionsLoad
		AddHandler FormClosing, AddressOf Me.OptionsFormClosing
		Me.ResumeLayout(false)
	End Sub
	Private propOptions As System.Windows.Forms.PropertyGrid
End Class
