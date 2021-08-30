Public Class clsMyCard
    Private VmTitle As String
    Private VmCost As String
    Private VmType As String
    Private VmSubType As String
    Private VmPower As String
    Private VmTough As String
    Private VmCardText As String
    Private VmAuthor As String
    Private VmColor As String
    Private VmRarity As String
    Public Sub New(VpCarac() As String, Optional VpComplement As List(Of String) = Nothing)
    Dim VpStrs() As String
        If VpCarac Is Nothing Then Exit Sub
        'Titre, coût, type, sous-type, attaque, défense, texte détaillé
        VmTitle = VpCarac(0).Trim
        VmCost = VpCarac(1).Trim
        VpStrs = VpCarac(2).Replace("—", "-").Split(New String() {" - "}, StringSplitOptions.None)
        VmType = VpStrs(0).Trim
        If VpStrs.Length > 1 Then
            VmSubType = VpStrs(1).Trim
        Else
            VmSubType = ""
        End If
        If VpCarac(3).Contains("/") Then
            VpStrs = VpCarac(3).Split("/")
            VmPower = VpStrs(0).Replace("(", "").Trim
            VmTough = VpStrs(1).Replace(")", "").Trim
        Else
            VmPower = ""
            VmTough = ""
        End If
        VmCardText = VpCarac(4)
        'Auteur, couleur, rareté
        If Not VpComplement Is Nothing Then
            VmAuthor = VpComplement.Item(0).ToString.Trim
            VmColor = VpComplement.Item(1).ToString.Trim
            If VmColor.Contains("/") Then
                VmColor = "Multicolor"
            End If
            VmRarity = VpComplement.Item(2).ToString.Trim
        End If
    End Sub
    Public Function GetMyCost As String
        Return mdlToolbox.MyCost(VmCost).ToString
    End Function
    Public Function MyType As String
        '(C = creature, I = instant, A = artefact, E = enchant-creature, K = token, L = land, N = interruption, S = sorcery, T = enchantment, U = abilited creature, P = planeswalker, Q = plane, H = phenomenon, Y = conspiracy, Z = scheme, W = dungeon)
        If VmType.Contains("Artifact") And Not VmType.Contains("Token") Then
            Return "A"
        ElseIf VmType.Contains("Instant") Then
            Return "I"
        ElseIf VmType.Contains("Enchantment") Then
            If VmSubType = "Aura" Then
                Return "E"
            Else
                Return "T"
            End If
        ElseIf VmType.Contains("Token") Or VmType.Contains("Emblem") Or VmType.Contains("Card") Or VmType.Contains("Vanguard") Then
            Return "K"
        ElseIf VmType.Contains("Creature") Or VmType.Contains("Summon") Then
            If VmCardText.Trim = "" Then
                Return "C"      'pas de texte : créature "classique"
            Else
                Return "U"      'texte descriptif : créature avec capacité
            End If
        ElseIf VmType.Contains("Land") Then
            Return "L"
        ElseIf VmType.Contains("Sorcery") Then
            Return "S"
        ElseIf VmType.Contains("Interrupt") Then
            Return "N"
        ElseIf VmType.Contains("Planeswalker") Then
            Return "P"
        ElseIf VmType.Contains("Plane") Then
            Return "Q"
        ElseIf VmType.Contains("Phenomenon") Then
            Return "H"
        ElseIf VmType.Contains("Conspiracy") Then
            Return "Y"
        ElseIf VmType.Contains("Scheme") Then
            Return "Z"
        ElseIf VmType.Contains("Dungeon") Then
            Return "W"
        Else
            Return ""
        End If
    End Function
    Public Function MySubType As String
        If VmSubType = "" Then
            Return "Null"
        ElseIf VmType.Contains("Artifact Creature") Then
            Return "'Creature " + VmSubType.Replace("'", "''") + "'"
        Else
            Return "'" + VmSubType.Replace("'", "''") + "'"
        End If
    End Function
    Public Function MyPower As String
        If VmPower = "" Then
            Return "'0'"
        Else
            Return "'" + VmPower + "'"
        End If
    End Function
    Public Function MyTough As String
        If VmTough = "" Then
            Return "'0'"
        Else
            Return "'" + VmTough + "'"
        End If
    End Function
    Public Function MyCardText As String
        If VmCardText = "" Then
            Return "Null"
        Else
            Return "'" + VmCardText.Replace("'", "''").Replace("/#/", vbCrLf + vbCrLf + "----" + vbCrLf + vbCrLf) + "'"
        End If
    End Function
    Public Function MyColor As String
    Dim VpMyType As String
        If VmType.Contains("Token") OrElse VmColor = "" Then    'dans les dernières versions du gatherer, il n'y a rien lorsqu'il s'agit d'un artefact, d'un terrain, d'un plan, d'un phénomène, d'une machination, d'un donjon, d'un arpenteur incolore ou d'un jeton
            VpMyType = Me.MyType
            Select Case VpMyType
                Case "H", "Q", "Y", "Z", "W", "P"
                    Return "A"
                Case "K"
                    Return "T"
                Case "L"
                    Return "L"
                Case Else
                    Return "A"
            End Select
        Else
            Select Case VmColor
                Case "Colorless (Artifact)", "Colorless", "Artifact", "A"
                    Return "A"
                Case "Black", "B"
                    Return "B"
                Case "Green", "G"
                    Return "G"
                Case "Colorless (Land)", "Land", "L"
                    Return "L"
                Case "Multicolor", "Z"
                    Return "M"
                Case "Red", "R"
                    Return "R"
                Case "Blue", "U"
                    Return "U"
                Case "White", "W"
                    Return "W"
                'Cas mal géré des double cartes
                Case "X"
                    Return "X"
                Case Else
                    Return "A"
            End Select
        End If
    End Function
    Public ReadOnly Property Title As String
        Get
            Return VmTitle
        End Get
    End Property
    Public ReadOnly Property Cost As String
        Get
            If VmCost <> "" Then
                Return "'" + VmCost + "'"
            Else
                Return "Null"
            End If
        End Get
    End Property
    Public ReadOnly Property Type As String
        Get
            Return VmType
        End Get
    End Property
    Public ReadOnly Property SubType As String
        Get
            Return VmSubType
        End Get
    End Property
    Public ReadOnly Property Power As String
        Get
            Return VmPower
        End Get
    End Property
    Public ReadOnly Property Tough As String
        Get
            Return VmTough
        End Get
    End Property
    Public ReadOnly Property CardText As String
        Get
            Return VmCardText
        End Get
    End Property
    Public ReadOnly Property Author As String
        Get
            Return VmAuthor
        End Get
    End Property
    Public ReadOnly Property Rarity As String
        Get
            Return VmRarity
        End Get
    End Property
End Class
