'
' Crée par SharpDevelop.
' Utilisateur: Couitchy
' Date: 11/11/2012
' Heure: 18:07
'
' Pour changer ce modèle utiliser Outils | Options | Codage | Editer les en-têtes standards.
'
Partial Class frmSide
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSide))
        Me.cbarReserve = New TD.SandBar.ContainerBar()
        Me.pnlReserve = New TD.SandBar.ContainerBarClientPanel()
        Me.grdRepartition = New SourceGrid2.Grid()
        Me.wbMV = New System.Windows.Forms.WebBrowser()
        Me.cbarReserve.SuspendLayout
        Me.pnlReserve.SuspendLayout
        Me.grdRepartition.SuspendLayout
        Me.SuspendLayout
        '
        'cbarReserve
        '
        Me.cbarReserve.AddRemoveButtonsVisible = false
        Me.cbarReserve.Controls.Add(Me.pnlReserve)
        Me.cbarReserve.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cbarReserve.DrawActionsButton = false
        Me.cbarReserve.Flow = TD.SandBar.ToolBarLayout.Horizontal
        Me.cbarReserve.Guid = New System.Guid("e1afccf7-b082-451d-9dae-2142b22dc603")
        Me.cbarReserve.Location = New System.Drawing.Point(0, 0)
        Me.cbarReserve.Movable = false
        Me.cbarReserve.Name = "cbarReserve"
        Me.cbarReserve.Size = New System.Drawing.Size(574, 412)
        Me.cbarReserve.TabIndex = 8
        Me.cbarReserve.Text = "Répartition avec le jeu de réserve"
        AddHandler Me.cbarReserve.VisibleChanged, AddressOf Me.CbarReserveVisibleChanged
        AddHandler Me.cbarReserve.MouseDown, AddressOf Me.CbarReserveMouseDown
        AddHandler Me.cbarReserve.MouseMove, AddressOf Me.CbarReserveMouseMove
        AddHandler Me.cbarReserve.MouseUp, AddressOf Me.CbarReserveMouseUp
        '
        'pnlReserve
        '
        Me.pnlReserve.Controls.Add(Me.grdRepartition)
        Me.pnlReserve.Location = New System.Drawing.Point(2, 27)
        Me.pnlReserve.Name = "pnlReserve"
        Me.pnlReserve.Size = New System.Drawing.Size(570, 383)
        Me.pnlReserve.TabIndex = 0
        '
        'grdRepartition
        '
        Me.grdRepartition.AutoSizeMinHeight = 10
        Me.grdRepartition.AutoSizeMinWidth = 10
        Me.grdRepartition.AutoStretchColumnsToFitWidth = false
        Me.grdRepartition.AutoStretchRowsToFitHeight = false
        Me.grdRepartition.BackColor = System.Drawing.Color.Transparent
        Me.grdRepartition.ContextMenuStyle = SourceGrid2.ContextMenuStyle.None
        Me.grdRepartition.Controls.Add(Me.wbMV)
        Me.grdRepartition.CustomSort = false
        Me.grdRepartition.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grdRepartition.GridToolTipActive = true
        Me.grdRepartition.Location = New System.Drawing.Point(0, 0)
        Me.grdRepartition.Name = "grdRepartition"
        Me.grdRepartition.Size = New System.Drawing.Size(570, 383)
        Me.grdRepartition.SpecialKeys = CType(((((((((((SourceGrid2.GridSpecialKeys.Ctrl_C Or SourceGrid2.GridSpecialKeys.Ctrl_V)  _
                        Or SourceGrid2.GridSpecialKeys.Ctrl_X)  _
                        Or SourceGrid2.GridSpecialKeys.Delete)  _
                        Or SourceGrid2.GridSpecialKeys.Arrows)  _
                        Or SourceGrid2.GridSpecialKeys.Tab)  _
                        Or SourceGrid2.GridSpecialKeys.PageDownUp)  _
                        Or SourceGrid2.GridSpecialKeys.Enter)  _
                        Or SourceGrid2.GridSpecialKeys.Escape)  _
                        Or SourceGrid2.GridSpecialKeys.Control)  _
                        Or SourceGrid2.GridSpecialKeys.Shift),SourceGrid2.GridSpecialKeys)
        Me.grdRepartition.TabIndex = 1
        '
        'wbMV
        '
        Me.wbMV.AllowWebBrowserDrop = false
        Me.wbMV.IsWebBrowserContextMenuEnabled = false
        Me.wbMV.Location = New System.Drawing.Point(10, 35)
        Me.wbMV.MinimumSize = New System.Drawing.Size(20, 20)
        Me.wbMV.Name = "wbMV"
        Me.wbMV.ScriptErrorsSuppressed = true
        Me.wbMV.Size = New System.Drawing.Size(564, 115)
        Me.wbMV.TabIndex = 5
        Me.wbMV.Visible = false
        '
        'frmReserve
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(574, 412)
        Me.Controls.Add(Me.cbarReserve)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
        Me.Name = "frmReserve"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Gestion du jeu de réserve"
        AddHandler FormClosing, AddressOf Me.FrmReserveFormClosing
        AddHandler Load, AddressOf Me.FrmReserveLoad
        Me.cbarReserve.ResumeLayout(false)
        Me.pnlReserve.ResumeLayout(false)
        Me.grdRepartition.ResumeLayout(false)
        Me.ResumeLayout(false)
    End Sub
    Private wbMV As System.Windows.Forms.WebBrowser
    Private grdRepartition As SourceGrid2.Grid
    Private pnlReserve As TD.SandBar.ContainerBarClientPanel
    Private cbarReserve As TD.SandBar.ContainerBar
End Class
