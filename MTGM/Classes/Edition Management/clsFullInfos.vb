<Serializable> _
Public Class clsFullInfos
    Public meta As clsFullMetaInfos
    Public data As clsFullDataInfos
    Public Class clsFullMetaInfos
        Public [date] As String
        Public version As String
    End Class
    Public Class clsFullDataInfos
        Public baseSetSize As Integer
        Public block As String
        Public code As String
        Public isFoilOnly As Boolean
        Public isOnlineOnly As Boolean
        Public keyruneCode As String
        Public mcmId As Integer
        Public mcmIdExtras As Integer
        Public mcmName As String
        Public mtgoCode As String
        Public name As String
        Public parentCode As String
        Public releaseDate As String
        Public tcgplayerGroupId As Integer
        Public totalSetSize As Integer
        Public type As String
        Public translations As Dictionary(Of String, String)
        Public cards As List(Of clsFullCardInfos)
        Public tokens As List(Of clsFullCardInfos)
        Public Class clsFullCardInfos
            Public linkedTo As clsFullCardInfos
            Public linkedFrom As clsFullCardInfos
            Public artist As String
            Public availability As List(Of String)
            Public borderColor As String
            Public colorIdentity As List(Of String)
            Public colorIndicator As List(Of String)
            Public colors As List(Of String)
            Public convertedManaCost As Single
            Public defense As Object
            Public edhrecRank As Integer
            Public faceConvertedManaCost As Single
            Public faceManaValue As Single
            Public faceName As String
            Public finishes As List(Of String)
            Public foreignData As List(Of clsForeignInfos)
            Public frameEffects As List(Of String)
            Public frameVersion As String
            Public hasFoil As Boolean
            Public hasNonFoil As Boolean
            Public identifiers As clsIdentifierInfos
            Public isPromo As Boolean
            Public isReprint As Boolean
            Public isStarter As Boolean
            Public isTimeshifted As Boolean
            Public keywords As List(Of String)
            Public layout As String
            Public legalities As List(Of clsLegalityInfos)
            Public loyalty As Object
            Public manaCost As String
            Public manaValue As Single
            Public name As String
            Public names As List(Of String)
            Public number As String
            Public originalText As String
            Public originalType As String
            Public otherFaceIds As List(Of String)
            Public power As String
            Public printings As List(Of String)
            Public promoTypes As List(Of String)
            Public rarity As String
            Public rulings As List(Of clsRulingsInfos)
            Public setCode As String
            Public side As String
            Public subtypes As List(Of String)
            Public supertypes As List(Of String)
            Public [text] As String
            Public toughness As String
            Public type As String
            Public types As List(Of String)
            Public uuid As String
            Public variations As List(Of String)
            Public watermark As String
            Public Class clsRulingsInfos
                Public [date] As String
                Public [text] As String
            End Class
            Public Class clsForeignInfos
                Public language As String
                Public multiverseId As Long
                Public name As String
                Public [text] As String
                Public type As String
            End Class
            Public Class clsIdentifierInfos
                Public cardKingdomFoilId As String
                Public cardKingdomId As String
                Public mcmId As String
                Public mcmMetaId As String
                Public mtgArenaId As String
                Public mtgjsonV4Id As String
                Public mtgoId As String
                Public multiverseId As String
                Public scryfallId As String
                Public scryfallIllustrationId As String
                Public scryfallOracleId As String
                Public tcgplayerProductId As String
            End Class
            Public Class clsLegalityInfos
                Public brawl As String
                Public commander As String
                Public duel As String
                Public future As String
                Public historic As String
                Public historicbrawl As String
                Public legacy As String
                Public modern As String
                Public pioneer As String
                Public standard As String
                Public vintage As String
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
