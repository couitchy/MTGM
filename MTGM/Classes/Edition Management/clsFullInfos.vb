<Serializable> _
Public Class clsFullInfos
    Public meta As clsFullMetaInfos
    Public data As clsFullDataInfos
    Public Class clsFullMetaInfos
        Public [date] As String
        Public version As String
    End Class
    Public Class clsFullDataInfos
        Public special As Boolean
        Public name As String
        Public code As String
        Public gathererCode As String
        Public oldCode As String
        Public magicCardsInfoCode As String
        Public releaseDate As String
        Public border As String
        Public type As String
        Public block As String
        Public onlineOnly As Boolean
        Public translations As Dictionary(Of String, String)
        Public cards As List(Of clsFullCardInfos)
        Public tokens As List(Of clsFullCardInfos)
        Public Class clsFullCardInfos
            Public linkedTo As clsFullCardInfos
            Public linkedFrom As clsFullCardInfos
            Public id As String
            Public layout As String
            Public name As String
            Public names As List(Of String)
            Public manaCost As String
            Public cmc As Single
            Public colors As List(Of String)
            Public colorIdentity As List(Of String)
            Public type As String
            Public supertypes As List(Of String)
            Public types As List(Of String)
            Public subtypes As List(Of String)
            Public rarity As String
            Public [text] As String
            Public artist As String
            Public number As String
            Public power As String
            Public toughness As String
            Public loyalty As Object
            Public variations As List(Of String)
            Public imageName As String
            Public watermark As String
            Public border As String
            Public timeshifted As Boolean
            Public hand As Integer
            Public life As Integer
            Public reserved As Boolean
            Public releaseDate As String
            Public starter As Boolean
            Public rulings As List(Of clsRulingsInfos)
            Public foreignData As List(Of clsForeignInfos)
            Public identifiers As clsIdentifierInfos
            Public printings As List(Of String)
            Public originalText As String
            Public originalType As String
            Public legalities As List(Of clsLegalityInfos)
            Public source As String
            Public uuid As String
            Public Class clsRulingsInfos
                Public [date] As String
                Public [text] As String
            End Class
            Public Class clsForeignInfos
                Public language As String
                Public name As String
                Public multiverseId As Long
            End Class
            Public Class clsIdentifierInfos
                Public mtgoId As String
                Public multiverseId As String
                Public scryfallId As String
                Public tcgplayerProductId As String
            End Class
            Public Class clsLegalityInfos
                Public format As String
                Public legality As String
            End Class
            Public Function getForeignName(language As String) As String
                For Each foreign As clsForeignInfos In foreignData
                    If foreign.language = language Then
                        Return foreign.name
                    End If
                Next foreign
                Return name
            End Function
            Public Function getMergedColors As List(Of String)
            Dim mergedColors As List(Of String)
                If linkedTo Is Nothing Then
                    Return colors
                Else
                    mergedColors = New List(Of String)
                    For Each color As String In colors
                        mergedColors.Add(color)
                    Next color
                    For Each color As String In linkedTo.colors
                        If Not mergedColors.Contains(color) Then
                            mergedColors.Add(color)
                        End If
                    Next color
                    Return mergedColors
                End If
            End Function
            Public Function getCost As String
            Dim subCosts() As String
            Dim cost As String
                cost = ""
                If manaCost IsNot Nothing Then
                    subCosts = manaCost.Split("{")
                    If subCosts.Length > 1 Then
                        For i As Integer = 1 To subCosts.Length - 1
                            subCosts(i) = subCosts(i).Replace("}", "")
                            If subCosts(i).Contains("/") Then
                                cost += "(" + subCosts(i) + ")"
                            Else
                                cost += subCosts(i)
                            End If
                        Next i
                    Else
                        cost = manaCost.Replace("{", "").Replace("}", "")
                    End If
                End If
                Return cost
            End Function
            Public Function getRules As String
                Return If([text] Is Nothing, "", [text].Replace("\n", vbCrLf))
            End Function
        End Class
        Public Class clsFullCardInfosComparer
            Implements IComparer(Of clsFullCardInfos)
            Public Function Compare(ByVal x As clsFullCardInfos, ByVal y As clsFullCardInfos) As Integer Implements IComparer(Of clsFullCardInfos).Compare
                If x.name = y.name Then
                    If x.foreignData Is Nothing And y.foreignData IsNot Nothing Then
                        Return 1
                    ElseIf x.foreignData IsNot Nothing And y.foreignData Is Nothing Then
                        Return -1
                    ElseIf x.foreignData IsNot Nothing And y.foreignData IsNot Nothing Then
                        Return y.foreignData.Count.CompareTo(x.foreignData.Count)
                    Else
                        Return x.name.CompareTo(y.name)
                    End If
                Else
                    Return x.name.CompareTo(y.name)
                End If
            End Function
        End Class
    End Class
End Class
