Public Class clsManas
    Private VmX As Short = 0                        'Mana variable
    Private VmM As Short = 0                        'Mana de n'importe quelle couleur
    Private VmA As Short = 0                        'Mana sans couleur
    Private VmC As Short = 0                        'Mana seulement sans couleur
    Private VmS As Short = 0                        'Mana neigeux
    Private VmB As Short = 0                        'Mana noir
    Private VmG As Short = 0                        'Mana vert
    Private VmU As Short = 0                        'Mana bleu
    Private VmR As Short = 0                        'Mana rouge
    Private VmW As Short = 0                        'Mana blanc
    Private VmPB As Short = 0                       'Mana noir ou 2 points de vie
    Private VmPG As Short = 0                       'Mana vert ou 2 points de vie
    Private VmPU As Short = 0                       'Mana bleu ou 2 points de vie
    Private VmPR As Short = 0                       'Mana rouge ou 2 points de vie
    Private VmPW As Short = 0                       'Mana blanc ou 2 points de vie
    Private Vm2W As Short = 0                       'Mana blanc ou 2 de n'importe quelle couleur
    Private Vm2R As Short = 0                       'Mana rouge ou 2 de n'importe quelle couleur
    Private Vm2G As Short = 0                       'Mana vert ou 2 de n'importe quelle couleur
    Private Vm2B As Short = 0                       'Mana noir ou 2 de n'importe quelle couleur
    Private Vm2U As Short = 0                       'Mana bleu ou 2 de n'importe quelle couleur
    Private VmBG As Short = 0                       'Mana noir ou vert
    Private VmBR As Short = 0                       'Mana noir ou rouge
    Private VmGU As Short = 0                       'Mana vert ou bleu
    Private VmGW As Short = 0                       'Mana vert ou blanc
    Private VmRG As Short = 0                       'Mana rouge ou vert
    Private VmRW As Short = 0                       'Mana rouge ou blanc
    Private VmUB As Short = 0                       'Mana bleu ou noir
    Private VmUR As Short = 0                       'Mana bleu ou rouge
    Private VmWB As Short = 0                       'Mana blanc ou noir
    Private VmWU As Short = 0                       'Mana blanc ou bleu
    Private VmEffectiveLength As Short = 0          'Longueur effective (~coût converti)
    Private VmImgIndexes As New List(Of Integer)    'Repères icônes (1=BG,2=BR,3=G,4=GU,5=GW,6=R,7=RG,8=RW,9=U,10=UB,11=UR,12=W,13=WB,14=WU,15=X,16=....)
    Public Sub New(Optional VpCostDB As String = "")
    '----------------------------------------------
    'Effectue un parsing du coût passé en paramètre
    '----------------------------------------------
    Dim VpCost As String
    Dim VpX As String                       'Unité de coût courant
    Dim VpY As String                       'Unité de dual-coût courant
    Dim VpI As Integer                      'Compteur
    Dim VpColorless As Integer              'Coût en manas sans couleur
    Dim VpStrs(0 To 1) As String
        If VpCostDB.Trim = "" Then Exit Sub
        'Gestion des cas spéciaux :
        VpCost = VpCostDB.ToLower
        VpCost = VpCost.Replace("{", "").Replace("}", "")
        '- either or (ex. either b or u)
        If VpCost.Contains(CgManaParsing(2)) Then
            VpCost = VpCost.Replace(CgManaParsing(2), "(").Replace(CgManaParsing(3), "/") + ")"
        '- colorless mana (ex. one colorless mana)
        ElseIf VpCost.Contains(CgManaParsing(4)) Then
            VpCost = VpCost.Replace(CgManaParsing(4), "")
            VpCost = StrBuild("A", FindNumber(VpCost))
        '- colorless mana d'un nombre indéterminé (on en met 1 par défaut)
        ElseIf VpCost = CgManaParsing(4).Trim Then
            VpCost = "A"
        '- mana of any color (ex. one mana of any color)
        ElseIf VpCost.Contains(CgManaParsing(5)) Then
            VpCost = VpCost.Replace(CgManaParsing(5), "")
            VpCost = StrBuild("M", FindNumber(VpCost))
        '- mana (ex. two blue mana)
        ElseIf VpCost.Contains(CgManaParsing(6)) Then
            VpCost = VpCost.Replace(CgManaParsing(6), "")
            VpStrs = VpCost.Split(" ")
            Try
                Select Case VpStrs(1)
                    Case "black"
                        VpStrs(1) = "B"
                    Case "blue"
                        VpStrs(1) = "U"
                    Case "red"
                        VpStrs(1) = "R"
                    Case "white"
                        VpStrs(1) = "W"
                    Case "green"
                        VpStrs(1) = "G"
                    Case Else
                        VpStrs(1) = "A"
                End Select
            'Si la couleur n'a pas été précisée, assume par défaut de l'incolore
            Catch
                ReDim Preserve VpStrs(0 To 1)
                VpStrs(1) = "A"
            End Try
            VpCost = StrBuild(VpStrs(1), FindNumber(VpStrs(0)))
        '- XA... (ex. X2U)
        ElseIf VpCost.StartsWith("x") AndAlso Val(VpCost.Substring(1)) <> 0 Then
            VmX = 1
            VmImgIndexes.Add(0)
            VmEffectiveLength = 1
            VpCost = VpCost.Substring(1)
        End If
        'Gestion des cas classiques :
        VmEffectiveLength = VmEffectiveLength + VpCost.Length - 4 * StrCount(VpCost, "(")
        VpColorless = Val(VpCost)
        If VpColorless >= 10 Then
            VmEffectiveLength = VmEffectiveLength - 1
        End If
        For VpI = 0 To VpCost.Length - 1
            VpX = VpCost.Substring(VpI, 1)
            'Pas de couleur
            If VpI = 0 And VpColorless <> 0 Then
                VmImgIndexes.Add(1 + CInt(VpColorless))
                If VpColorless >= 10 Then
                    VpI = VpI + 1   'Très impropre en programmation structurée mais permet de gérer directement le cas des nombres à 2 chiffres
                End If
                VmA = VpColorless
            'Dual couleur
            ElseIf VpX = "(" Then
                VpY = VpCost.Substring(VpI + 1, 3)
                Select Case VpY.ToUpper
                    Case "P/R", "R/P"
                        VmImgIndexes.Add(35)
                        VmPR = VmPR + 1
                    Case "P/B", "B/P"
                        VmImgIndexes.Add(33)
                        VmPB = VmPB + 1
                    Case "P/G", "G/P"
                        VmImgIndexes.Add(34)
                        VmPG = VmPG + 1
                    Case "P/U", "U/P"
                        VmImgIndexes.Add(36)
                        VmPU = VmPU + 1
                    Case "P/W", "W/P"
                        VmImgIndexes.Add(37)
                        VmPW = VmPW + 1
                    Case "G/B"
                        VmImgIndexes.Add(29)
                        VmBG = VmBG + 1
                    Case "R/B"
                        VmImgIndexes.Add(39)
                        VmBR = VmBR + 1
                    Case "U/G"
                        VmImgIndexes.Add(45)
                        VmGU = VmGU + 1
                    Case "W/G"
                        VmImgIndexes.Add(50)
                        VmGW = VmGW + 1
                    Case "G/R"
                        VmImgIndexes.Add(30)
                        VmRG = VmRG + 1
                    Case "W/R"
                        VmImgIndexes.Add(51)
                        VmRW = VmRW + 1
                    Case "B/U"
                        VmImgIndexes.Add(26)
                        VmUB = VmUB + 1
                    Case "R/U"
                        VmImgIndexes.Add(41)
                        VmUR = VmUR + 1
                    Case "B/W"
                        VmImgIndexes.Add(27)
                        VmWB = VmWB + 1
                    Case "U/W"
                        VmImgIndexes.Add(47)
                        VmWU = VmWU + 1
                    Case "B/G"
                        VmImgIndexes.Add(24)
                        VmBG = VmBG + 1
                    Case "B/R"
                        VmImgIndexes.Add(25)
                        VmBR = VmBR + 1
                    Case "G/U"
                        VmImgIndexes.Add(31)
                        VmGU = VmGU + 1
                    Case "G/W"
                        VmImgIndexes.Add(32)
                        VmGW = VmGW + 1
                    Case "R/G"
                        VmImgIndexes.Add(40)
                        VmRG = VmRG + 1
                    Case "R/W"
                        VmImgIndexes.Add(42)
                        VmRW = VmRW + 1
                    Case "U/B"
                        VmImgIndexes.Add(44)
                        VmUB = VmUB + 1
                    Case "U/R"
                        VmImgIndexes.Add(46)
                        VmUR = VmUR + 1
                    Case "W/B"
                        VmImgIndexes.Add(49)
                        VmWB = VmWB + 1
                    Case "W/U"
                        VmImgIndexes.Add(52)
                        VmWU = VmWU + 1
                    Case "2/W", "W/2"
                        VmImgIndexes.Add(22)
                        Vm2W = Vm2W + 1
                    Case "2/B", "B/2"
                        VmImgIndexes.Add(18)
                        Vm2B = Vm2B + 1
                    Case "2/G", "G/2"
                        VmImgIndexes.Add(19)
                        Vm2G = Vm2G + 1
                    Case "2/R", "R/2"
                        VmImgIndexes.Add(20)
                        Vm2R = Vm2R + 1
                    Case "2/U", "U/2"
                        VmImgIndexes.Add(21)
                        Vm2U = Vm2U + 1
                    Case Else
                        VmImgIndexes.Add(0)
                End Select
                VpI = VpI + 4 'encore impropre mais permet de passer directement à la fin de la parenthèse
            'Couleur simple
            Else
                Select Case VpX.ToUpper
                    Case "X"
                        VmImgIndexes.Add(0)
                        VmX = VmX + 1
                    Case "B"
                        VmImgIndexes.Add(23)
                        VmB = VmB + 1
                    Case "G"
                        VmImgIndexes.Add(28)
                        VmG = VmG + 1
                    Case "R"
                        VmImgIndexes.Add(38)
                        VmR = VmR + 1
                    Case "U"
                        VmImgIndexes.Add(43)
                        VmU = VmU + 1
                    Case "W"
                        VmImgIndexes.Add(48)
                        VmW = VmW + 1
                    Case "M"
                        VmImgIndexes.Add(1)
                        VmM = VmM + 1
                    Case "A"
                        VmImgIndexes.Add(1)
                        VmA = VmA + 1
                    Case "C"
                        VmImgIndexes.Add(56)
                        VmC = VmC + 1
                    Case "S"
                        VmImgIndexes.Add(55)
                        VmS = VmS + 1
                    Case Else
                        VmImgIndexes.Add(1)
                End Select
            End If
        Next VpI
    End Sub
    Public Function IsBetterWith(VpMana As clsManas) As Boolean
    '-----------------------------------------------------------------------------------------------------------
    'Renvoie vrai si l'invocation de la carte courante requiert le mana produit par la carte passée en paramètre
    '-----------------------------------------------------------------------------------------------------------
        Return ( ( Me.HasBlack And VpMana.HasBlack ) Or ( Me.HasBlue And VpMana.HasBlue ) Or ( Me.HasGreen And VpMana.HasGreen ) Or ( Me.HasRed And VpMana.HasRed ) Or ( Me.HasWhite And VpMana.HasWhite ) )
    End Function
    Public Sub AddSubManas(VpManas As clsManas, Optional VpSub As Short = 1)
    '----------------------------------------------------------------------------------
    'Ajoute (ou soustrait) la description de manas passée en paramètre à celle courante
    '----------------------------------------------------------------------------------
        VmM = VmM + VpManas.cM * VpSub
        VmA = VmA + VpManas.cA * VpSub
        VmC = VmC + VpManas.cC * VpSub
        VmS = VmS + VpManas.cS * VpSub
        VmB = VmB + VpManas.cB * VpSub
        VmG = VmG + VpManas.cG * VpSub
        VmU = VmU + VpManas.cU * VpSub
        VmR = VmR + VpManas.cR * VpSub
        VmW = VmW + VpManas.cW * VpSub
        VmBG = VmBG + VpManas.cBG * VpSub
        VmBR = VmBR + VpManas.cBR * VpSub
        VmGU = VmGU + VpManas.cGU * VpSub
        VmGW = VmGW + VpManas.cGW * VpSub
        VmRG = VmRG + VpManas.cRG * VpSub
        VmRW = VmRW + VpManas.cRW * VpSub
        VmUB = VmUB + VpManas.cUB * VpSub
        VmUR = VmUR + VpManas.cUR * VpSub
        VmWB = VmWB + VpManas.cWB * VpSub
        VmWU = VmWU + VpManas.cWU * VpSub
        VmPB = VmPB + VpManas.cPB * VpSub
        VmPG = VmPG + VpManas.cPG * VpSub
        VmPU = VmPU + VpManas.cPU * VpSub
        VmPR = VmPR + VpManas.cPR * VpSub
        VmPW = VmPW + VpManas.cPW * VpSub
        Vm2B = Vm2B + VpManas.c2B * VpSub
        Vm2G = Vm2G + VpManas.c2G * VpSub
        Vm2U = Vm2U + VpManas.c2U * VpSub
        Vm2R = Vm2R + VpManas.c2R * VpSub
        Vm2W = Vm2W + VpManas.c2W * VpSub
    End Sub
    Public Function ContainsEnoughFor(VpManas As clsManas) As Boolean
    '-----------------------------------------------------------------------------------------------------------------------
    'Renvoie vrai si la description de mana courante contient (en terme de qualité et de quantité) celle passée en paramètre
    '-----------------------------------------------------------------------------------------------------------------------
    Dim VpEnoughColor As Boolean
    Dim VpMe As clsManas
        VpEnoughColor = (   VmB >= VpManas.cB And _
                            VmG >= VpManas.cG And _
                            VmU >= VpManas.cU And _
                            VmR >= VpManas.cR And _
                            VmW >= VpManas.cW And _
                            VmC >= VpManas.cC And _
                            VmS >= VpManas.cS And _
                            VmBG >= VpManas.cBG And _
                            VmBR >= VpManas.cBR And _
                            VmGU >= VpManas.cGU And _
                            VmGW >= VpManas.cGW And _
                            VmRG >= VpManas.cRG And _
                            VmRW >= VpManas.cRW And _
                            VmUB >= VpManas.cUB And _
                            VmUR >= VpManas.cUR And _
                            VmWB >= VpManas.cWB And _
                            VmWU >= VpManas.cWU And _
                            Vm2W >= VpManas.c2W And _
                            Vm2B >= VpManas.c2B And _
                            Vm2R >= VpManas.c2R And _
                            Vm2G >= VpManas.c2G And _
                            Vm2U >= VpManas.c2U And _
                            VmPW >= VpManas.cPW And _
                            VmPB >= VpManas.cPB And _
                            VmPR >= VpManas.cPR And _
                            VmPG >= VpManas.cPG And _
                            VmPU >= VpManas.cPU     )
        'Si les manas de couleurs ne peuvent être payés, c'est mort
        If Not VpEnoughColor Then
            Return False
        Else
            'S'ils le peuvent et qu'en plus les multi/in-colores peuvent être payés, c'est bon
            If (VmA + VmM) >= (VpManas.cA + VpManas.cM) Then
                Return True
            'Sinon, il faut regarder si les multi/in-colores peuvent êtres payés avec ceux restant de couleurs
            Else
                'Copie temporaire de la description de manas courante
                VpMe = New clsManas
                Call VpMe.AddSubManas(Me)
                'Soustraction des coûts colorés uniquement
                Call VpMe.AddSubManas(VpManas, -1)
                VpMe.cA = Me.cA
                VpMe.cM = Me.cM
                'Reste-t-il assez pour payer les multi/in-colores de la description de manas passée en paramètre
                Return ( VpMe.Potentiel >= (VpManas.cA + VpManas.cM) )
            End If
        End If
    End Function
    Public ReadOnly Property ImgIndexes As List(Of Integer)
        Get
            Return VmImgIndexes
        End Get
    End Property
    Public ReadOnly Property EffectiveLength As Integer
        Get
            Return VmEffectiveLength
        End Get
    End Property
    Public ReadOnly Property Potentiel As Integer
        Get
            Return ( VmX + VmM + VmA + VmC + VmS + VmB + VmG + VmU + VmR + VmW + VmBG + VmBR + VmGU + VmGW + VmRG + VmRW + VmUB + VmUR + VmWB + VmWU + VmPR + VmPG + VmPU + VmPW + VmPB + Vm2R + Vm2G + Vm2U + Vm2W + Vm2B)
        End Get
    End Property
    Public ReadOnly Property HasBlack As Boolean
        Get
            Return ( VmB > 0 Or VmBG > 0 Or VmBR > 0 Or VmUB > 0 Or VmWB > 0 Or VmPB > 0 Or Vm2B > 0 )
        End Get
    End Property
    Public ReadOnly Property HasGreen As Boolean
        Get
            Return ( VmG > 0 Or VmBG > 0 Or VmGU > 0 Or VmGW > 0 Or VmRG > 0 Or VmPG > 0 Or Vm2G > 0 )
        End Get
    End Property
    Public ReadOnly Property HasBlue As Boolean
        Get
            Return ( VmU > 0 Or VmGU > 0 Or VmUB > 0 Or VmUR > 0 Or VmWU > 0 Or VmPU > 0 Or Vm2U > 0 )
        End Get
    End Property
    Public ReadOnly Property HasRed As Boolean
        Get
            Return ( VmR > 0 Or VmBR > 0 Or VmRG > 0 Or VmRW > 0 Or VmUR > 0 Or VmPR > 0 Or Vm2R > 0 )
        End Get
    End Property
    Public ReadOnly Property HasWhite As Boolean
        Get
            Return ( VmW > 0 Or VmGW > 0 Or VmRW > 0 Or VmWB > 0 Or VmWU > 0 Or VmPW > 0 Or Vm2W > 0 )
        End Get
    End Property
    Public Property cM As Short
        Get
            Return VmM
        End Get
        Set (VpM As Short)
            VmM = VpM
        End Set
    End Property
    Public Property cA As Short
        Get
            Return VmA
        End Get
        Set (VpA As Short)
            VmA = VpA
        End Set
    End Property
    Public Property cC As Short
        Get
            Return VmC
        End Get
        Set (VpC As Short)
            VmC = VpC
        End Set
    End Property
    Public Property cS As Short
        Get
            Return VmS
        End Get
        Set (VpS As Short)
            VmS = VpS
        End Set
    End Property
    Public ReadOnly Property cB As Short
        Get
            Return VmB
        End Get
    End Property
    Public ReadOnly Property cG As Short
        Get
            Return VmG
        End Get
    End Property
    Public ReadOnly Property cU As Short
        Get
            Return VmU
        End Get
    End Property
    Public ReadOnly Property cR As Short
        Get
            Return VmR
        End Get
    End Property
    Public ReadOnly Property cW As Short
        Get
            Return VmW
        End Get
    End Property
    Public ReadOnly Property cBG As Short
        Get
            Return VmBG
        End Get
    End Property
    Public ReadOnly Property cBR As Short
        Get
            Return VmBR
        End Get
    End Property
    Public ReadOnly Property cGU As Short
        Get
            Return VmGU
        End Get
    End Property
    Public ReadOnly Property cGW As Short
        Get
            Return VmGW
        End Get
    End Property
    Public ReadOnly Property cRG As Short
        Get
            Return VmRG
        End Get
    End Property
    Public ReadOnly Property cRW As Short
        Get
            Return VmRW
        End Get
    End Property
    Public ReadOnly Property cUB As Short
        Get
            Return VmUB
        End Get
    End Property
    Public ReadOnly Property cUR As Short
        Get
            Return VmUR
        End Get
    End Property
    Public ReadOnly Property cWB As Short
        Get
            Return VmWB
        End Get
    End Property
    Public ReadOnly Property cWU As Short
        Get
            Return VmWU
        End Get
    End Property
    Public ReadOnly Property cPR As Short
        Get
            Return VmPR
        End Get
    End Property
    Public ReadOnly Property cPW As Short
        Get
            Return VmPW
        End Get
    End Property
    Public ReadOnly Property cPU As Short
        Get
            Return VmPU
        End Get
    End Property
    Public ReadOnly Property cPG As Short
        Get
            Return VmPG
        End Get
    End Property
    Public ReadOnly Property cPB As Short
        Get
            Return VmPB
        End Get
    End Property
    Public ReadOnly Property c2R As Short
        Get
            Return VmPR
        End Get
    End Property
    Public ReadOnly Property c2W As Short
        Get
            Return VmPW
        End Get
    End Property
    Public ReadOnly Property c2U As Short
        Get
            Return VmPU
        End Get
    End Property
    Public ReadOnly Property c2G As Short
        Get
            Return VmPG
        End Get
    End Property
    Public ReadOnly Property c2B As Short
        Get
            Return VmPB
        End Get
    End Property
End Class
