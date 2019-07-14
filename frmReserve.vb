Imports SourceGrid2
Imports Cells = SourceGrid2.Cells.Real
Public Partial Class frmReserve
    Private VmFormMove As Boolean = False   'Formulaire en déplacement
    Private VmMousePos As Point             'Position initiale de la souris sur la barre de titre
    Private VmCanClose As Boolean = False   'Formulaire peut être fermé
    Private VmOwner As frmPlateau
    Private VmRepartition As New Hashtable
    Public Sub New(VpOwner As frmPlateau)
        Call Me.InitializeComponent
        VmOwner = VpOwner
    End Sub
    Private Sub LoadRepartition
    '----------------------------------------------
    'Chargement de la grille des cartes disponibles
    '----------------------------------------------
    Dim VpItem As clsRepartition
    Dim VpRow As Integer
    Dim VpCellModel As DataModels.IDataModel
        'Construction de la grille
        Call clsModule.InitGrid(Me.grdRepartition, New String() {"Nom VF", "Nom VO", "Quantité Deck", "Quantité Réserve"})
        'Enumération
        For Each VpCard As clsPlateauCard In VmOwner.PlateauPartie.CardsInDeck
            If Not VmRepartition.Contains(VpCard.NameVO) Then
                VmRepartition.Add(VpCard.NameVO, New clsRepartition(VpCard.NameVF, VpCard.InReserve))
            Else
                VpItem = VmRepartition.Item(VpCard.NameVO)
                If VpCard.InReserve Then
                    VpItem.SideCount += 1
                Else
                    VpItem.DeckCount += 1
                End If
            End If
        Next VpCard
        'Remplissage
        For Each VpNameVO As String In VmRepartition.Keys
            VpItem = VmRepartition.Item(VpNameVO)
            'Insertion nouvelle ligne
            VpRow = Me.grdRepartition.RowsCount
            Me.grdRepartition.Rows.Insert(VpRow)
            Me.grdRepartition(VpRow, 0) = New Cells.Cell(VpItem.NameVF)
            Me.grdRepartition(VpRow, 0).Tag = VpItem
            Me.grdRepartition(VpRow, 1) = New Cells.Cell(VpNameVO)
            Me.grdRepartition(VpRow, 2) = New Cells.Cell(VpItem.DeckCount)
            Me.grdRepartition(VpRow, 3) = New Cells.Cell(VpItem.SideCount)
            VpCellModel = Utility.CreateDataModel(Type.GetType("System.Int32"))
            VpCellModel.EditableMode = EditableMode.AnyKey Or EditableMode.SingleClick
            AddHandler VpCellModel.Validated, AddressOf CellValidated
            Me.grdRepartition(VpRow, 2).DataModel = VpCellModel
            Me.grdRepartition(VpRow, 3).DataModel = VpCellModel
        Next VpNameVO
        Me.grdRepartition.AutoSize
    End Sub
    Private Sub ValidateRepartition
    '-------------------------------------
    'Validation de la nouvelle répartition
    '-------------------------------------
    Dim VpItem As clsRepartition
    Dim VpDeckCount As Integer
    Dim VpSideCount As Integer
        For Each VpNameVO As String In VmRepartition.Keys
            VpItem = VmRepartition.Item(VpNameVO)
            VpDeckCount = 0
            VpSideCount = 0
            For Each VpCard As clsPlateauCard In VmOwner.PlateauPartie.CardsInDeck
                If VpCard.NameVO = VpNameVO Then
                    If VpDeckCount < VpItem.DeckCount Then
                        VpCard.InReserve = False
                        VpDeckCount += 1
                    ElseIf VpSideCount < VpItem.SideCount Then
                        VpCard.InReserve = True
                        VpSideCount += 1
                    End If
                End If
            Next VpCard
        Next VpNameVO
    End Sub
    Sub FrmReserveLoad(sender As Object, e As EventArgs)
        Call Me.LoadRepartition
    End Sub
    Sub CellValidated(sender As Object, e As CellEventArgs)
    Dim VpCell As Cells.Cell = e.Cell
    Dim VpGrid As Grid = VpCell.Grid
    Dim VpItem As clsRepartition = CType(VpGrid(VpCell.Row, 0).Tag, clsRepartition)
    Dim VpSideCountChanging As Boolean = (VpCell.Column = 3) 'un peu crade, mais une modification sur la dernière colonne indique qu'on a touché au nombre dans le side
    Dim VpValue As Integer = CInt(VpCell.Value)
    Dim VpTotal As Integer
    Dim VpDeckCount As Integer
    Dim VpSideCount As Integer
        'Cohérence de la valeur
        VpTotal = VpItem.DeckCount + VpItem.SideCount
        VpValue = Math.Min(Math.Max(VpValue, 0), VpTotal)
        'Adaptation des quantités
        If VpSideCountChanging Then
            VpSideCount = VpValue
            VpDeckCount = VpTotal - VpSideCount
        Else
            VpDeckCount = VpValue
            VpSideCount = VpTotal - VpDeckCount
        End If
        'Rafraîchissement dans la hashtable et dans la grille
        VpItem.DeckCount = VpDeckCount
        VpItem.SideCount = VpSideCount
        VpGrid(VpCell.Row, 2).Value = VpDeckCount
        VpGrid(VpCell.Row, 3).Value = VpSideCount
    End Sub
    Sub CbarReserveMouseDown(sender As Object, e As MouseEventArgs)
        VmFormMove = True
        VmCanClose = True
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
    Sub FrmReserveFormClosing(sender As Object, e As FormClosingEventArgs)
        Call Me.ValidateRepartition
    End Sub
End Class
