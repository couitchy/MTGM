#Region "Importations"
Imports SourceGrid2
Imports Cells = SourceGrid2.Cells.Real
Imports System.IO
Imports System.Net
Imports System.Web.Script.Serialization
Imports System.Xml
Imports System.Xml.Serialization
#End Region
Public Partial Class frmBuyCards
    #Region "D�clarations"
    Private VmServer As mdlConstGlob.eMarketServer      'Market place choisie par l'utilisateur pour effectuer ses transactions
    Private VmToBuy As New List(Of clsLocalCard)        'Collection des cartes souhait�es � l'achat
    Private VmToSell As New List(Of clsRemoteCard)      'Collection des cartes disponibles � la vente
    Private VmEditions As List(Of String)               'Liste des �ditions disponibles pour une m�me carte dans le cas o� il y en a trop et qu'il faut potentiellement restreindre le volume de recherches
    Private VmSplitterDistance As Integer               'Position du s�parateur
    Private VmBrowser As New WebBrowser                 'Navigateur web
    Private VmIsComplete As Boolean = False             'Page compl�tement affich�e dans le navigateur
    #End Region
    #Region "M�thodes"
    Public Sub New(VpServer As mdlConstGlob.eMarketServer)
        Call Me.InitializeComponent
        'Market place
        VmServer = VpServer
        'Initialisation du navigateur
        VmBrowser.AllowWebBrowserDrop = false
        VmBrowser.IsWebBrowserContextMenuEnabled = false
        VmBrowser.ScriptErrorsSuppressed = true
        AddHandler VmBrowser.DocumentCompleted, AddressOf Me.BrowserDocumentCompleted
    End Sub
    Private Sub BrowseAndWait(Optional VpURL As String = "")
    '---------------------------------------------------------------------------
    'Navigue sur la page pass�e en param�tre en respectant le d�lai d'expiration
    '---------------------------------------------------------------------------
    Dim VpStart As Date = Now
        VmIsComplete = False
        If VpURL <> "" Then
            VmBrowser.Navigate(VpURL)
        End If
        While Not VmIsComplete
            If Now.Subtract(VpStart).TotalSeconds > mdlConstGlob.CgTimeOut Then
                VmBrowser.Stop
                VmIsComplete = True
            End If
            Application.DoEvents
        End While
    End Sub
    Public Sub AddToBasket(VpName As String)
    '------------------------------------------------------------------
    'Incr�mente la quantit� du panier pour la carte pass�e en param�tre
    '------------------------------------------------------------------
        'Carte d�j� mise au panier
        For Each VpCard As clsLocalCard In VmToBuy
            If VpCard.Name = VpName Then
                VpCard.Quantity += 1
                Return
            End If
        Next
        'Nouvelle carte
        VmToBuy.Add(New clsLocalCard(VpName, 1, 0))
    End Sub
    Public Sub AddToBasket(VpName As String, VpQuant As Integer)
        For VpI As Integer = 1 To VpQuant
            Call Me.AddToBasket(VpName)
        Next VpI
    End Sub
    Private Sub ClearSellList
    '--------------------------------------------------------
    'Supprime les entr�es de la liste dont la quantit� vaut 0
    '--------------------------------------------------------
    Dim VpToRemove As New List(Of clsRemoteCard)
        'R�cup�ration des �l�ments � supprimer
        For Each VpProposition As clsRemoteCard In VmToSell
            If VpProposition.Quantity = 0 Then
                VpToRemove.Add(VpProposition)
            End If
        Next VpProposition
        'Suppression effective
        For Each VpDelete As clsRemoteCard In VpToRemove
            VmToSell.Remove(VpDelete)
        Next VpDelete
    End Sub
    Private Sub ExcludeSellers
    '-----------------------------------------------------
    'Supprime les cartes propos�es par des vendeurs bannis
    '-----------------------------------------------------
    Dim VpToRemove As New List(Of clsRemoteCard)
    Dim VpBannedSellers() As String = VgOptions.VgSettings.BannedSellers.ToLower.Split("#")
        If VpBannedSellers IsNot Nothing AndAlso VpBannedSellers.Length > 0 Then
            'R�cup�ration des �l�ments � supprimer
            For Each VpRemoteCard As clsRemoteCard In VmToSell
                If Array.IndexOf(VpBannedSellers, VpRemoteCard.Vendeur.Name.ToLower) >= 0 Then
                    VpToRemove.Add(VpRemoteCard)
                End If
            Next VpRemoteCard
            'Suppression effective
            For Each VpDelete As clsRemoteCard In VpToRemove
                VmToSell.Remove(VpDelete)
            Next VpDelete
        End If
    End Sub
    Private Sub MVFetch(VpCard As String, VpQualities() As mdlConstGlob.eQuality, VpBannedSellers() As String)
    '--------------------------------------------------------------------------------------------------------------
    'Se connecte sur Magic-Ville pour r�cup�rer les informations de ventes relatives � la carte pass�e en param�tre
    '--------------------------------------------------------------------------------------------------------------
    Dim VpElement As HtmlElement
    Dim VpLastId As Integer = 0
    Dim VpCurId As Integer
    Dim VpRemoteCard As clsRemoteCard = Nothing
    Dim VpToRemove As New List(Of clsRemoteCard)
        'Connexion au site de Magic-Ville
        Call Me.BrowseAndWait(mdlConstGlob.CgURL26)
        'Saisie de la carte dans la zone de recherche
        VpElement = VmBrowser.Document.All.GetElementsByName("recherche_titre").Item(0)
        VpElement.SetAttribute("value", VpCard)
        For Each VpElement In VmBrowser.Document.All
            If VpElement.GetAttribute("src").ToLower.Contains("/go.png") Then
                'Validation
                VpElement.InvokeMember("click")
                Call Me.BrowseAndWait
                Exit For
            End If
        Next VpElement
        'Page interm�diaire (ne s'affiche qu'en cas d'ambiguit�)
        For Each VpElement In VmBrowser.Document.All
            If VpElement.GetAttribute("href") <> "" AndAlso Not VpElement.InnerText Is Nothing Then
                If VpElement.InnerText.ToLower.Trim = VpCard.ToLower Then
                    'Validation
                    VpElement.InvokeMember("click")
                    Call Me.BrowseAndWait
                    Exit For
                End If
            End If
        Next VpElement
        'Page des achats
        For Each VpElement In VmBrowser.Document.All
            If VpElement.InnerText = "Achetez cette carte � un magicvillois" Then
                'Validation
                VpElement.InvokeMember("click")
                Call Me.BrowseAndWait
                Exit For
            End If
        Next VpElement
        'Parsing des propri�t�s
        For Each VpElement In VmBrowser.Document.All
            If VpElement.Name.Contains("[") Then
                VpCurId = Val(VpElement.Name.Substring(VpElement.Name.IndexOf("[") + 1))
                'Si l'identifiant a chang�, c'est qu'on est sur une nouvelle entr�e
                If VpCurId <> VpLastId Then
                    VpRemoteCard = New clsRemoteCard(VpCard)
                    VmToSell.Add(VpRemoteCard)
                    VpLastId = VpCurId
                End If
                'Remplissage des propri�t�s au fur et � mesure
                With VpRemoteCard
                    If VpElement.Name.Contains("qte") Then
                        .Quantity = CInt(VpElement.InnerText.Substring(VpElement.InnerText.Length - 1))
                    ElseIf VpElement.Name.Contains("ref") Then
                        .Edition = VpElement.GetAttribute("value")
                    ElseIf VpElement.Name.Contains("lang") Then
                        .Language = mdlToolbox.MyLanguage(VpElement.GetAttribute("value"))
                    ElseIf VpElement.Name.Contains("etat") Then
                        .Etat = VpElement.GetAttribute("value")
                    ElseIf VpElement.Name.Contains("seller") Then
                        .Vendeur = New clsSeller(VpElement.GetAttribute("value"))
                    End If
                End With
            ElseIf VpElement.GetAttribute("color") = "#3333ff" Then
                VpRemoteCard.Prix = mdlToolbox.MyVal(VpElement.InnerText)
            End If
        Next VpElement
        'Supprime de la collection tout ce qui ne respecte pas les crit�res actifs
        For Each VpRemoteCard In VmToSell
            With VpRemoteCard
                If Not ( Array.IndexOf(mdlConstGlob.CgBuyLanguage, .Language.ToLower) >= 0 AndAlso Array.IndexOf(VpQualities, .Etat) >= 0 AndAlso (VpBannedSellers Is Nothing OrElse VpBannedSellers.Length = 0 OrElse Array.IndexOf(VpBannedSellers, .Vendeur.Name.ToLower) < 0) ) Then
                    VpToRemove.Add(VpRemoteCard)
                End If
            End With
        Next VpRemoteCard
        For Each VpDelete As clsRemoteCard In VpToRemove
            VmToSell.Remove(VpDelete)
        Next VpDelete
    End Sub
    Private Sub MKMFetch(VpCard As String, VpQualities() As mdlConstGlob.eQuality, VpBannedSellers() As String)
    '------------------------------------------------------------------------------------------------------------------
    'Se connecte sur MagicCardMarket pour r�cup�rer les informations de ventes relatives � la carte pass�e en param�tre
    '------------------------------------------------------------------------------------------------------------------
    Dim VpProducts As clsProductRequest = Nothing
    Dim VpArticles As clsArticleRequest = Nothing
    Dim VpFilterEditions As frmBuySettings
        'Commence par effectuer une recherche sur le nom pour r�cup�rer l'id produit
        VpProducts = MKMRequest(Of clsProductRequest)(CgURL24.Replace("card-name", Uri.EscapeDataString(VpCard.ToLower)))
        'S'il y a trop d'id produits disponibles (trop d'�ditions pour une m�me carte), on propose de restreindre les requ�tes
        VmEditions = Nothing
        If VpProducts.product.Count > mdlConstGlob.CgMaxEditionsMarket Then
            VpFilterEditions = New frmBuySettings(VpCard, Me, VpProducts.product)
            VpFilterEditions.ShowDialog
        End If
        'R�cup�ration des disponibilit�s du produit sur les �ditions retenues
        For Each VpProduct As clsProductRequest.clsProduct In VpProducts.product
            If VmEditions Is Nothing OrElse VmEditions.Contains(VpProduct.expansion) Then
                VpArticles = MKMRequest(Of clsArticleRequest)(Uri.EscapeUriString(CgURL25 + VpProduct.idProduct.ToString))
                'Ajout � la collection si respect des crit�res actifs
                For Each VpArticle As clsArticleRequest.clsArticle In VpArticles.article
                    With VpArticle
                        If Array.IndexOf(mdlConstGlob.CgBuyLanguage, .language.languageName.ToLower) >= 0 AndAlso Array.IndexOf(VpQualities, mdlToolbox.MyQuality(.condition)) >= 0 AndAlso (VpBannedSellers Is Nothing OrElse VpBannedSellers.Length = 0 OrElse Array.IndexOf(VpBannedSellers, .seller.username.ToLower) < 0) Then
                            VmToSell.Add(New clsRemoteCard(VpCard, New clsSeller(.seller.username), VpProduct.expansion, .language.languageName, mdlToolbox.MyQuality(.condition), .count, 0, .price))
                        End If
                    End With
                Next VpArticle
                Application.DoEvents
            End If
        Next VpProduct
    End Sub
    Private Function MKMRequest(Of tRequest)(VpURL As String) As tRequest
    Dim VpRequest As HttpWebRequest
    Dim VpSerializer As New JavaScriptSerializer
    Dim VpWebResponse As HttpWebResponse
        VpURL = VpURL.Replace("'", "%27")   'cas non g�r�s par EscapeUriString
        VpSerializer.MaxJsonLength = Integer.MaxValue
        VpRequest = HttpWebRequest.Create(VpURL)
        VpRequest.Method = "GET"
        VpRequest.Headers.Clear
        VpRequest.Headers.Add(HttpRequestHeader.Authorization, (New clsOAuthHeader).GetAuthorizationHeader(VpRequest.Method, VpURL))
        VpWebResponse = VpRequest.GetResponse
        Return VpSerializer.Deserialize(Of tRequest)((New StreamReader(VpWebResponse.GetResponseStream)).ReadToEnd)
    End Function
    Private Function Extract(VpCard As String, VpToSell As List(Of clsRemoteCard)) As List(Of clsRemoteCard)
    '-------------------------------------------------------------------------------------------------------------------------------------------
    'Renvoie la sous-liste de toutes les cartes de m�me prix non encore achet�es correspondant au nom de carte demand� (la liste est d�j� tri�e)
    '-------------------------------------------------------------------------------------------------------------------------------------------
    Dim VpExtracted As New List(Of clsRemoteCard)
    Dim VpPrice As Single = -1
        For Each VpRemoteCard As clsRemoteCard In VpToSell
            If VpRemoteCard.Name = VpCard AndAlso VpRemoteCard.Quantity > 0 Then
                If VpPrice = -1 Then
                    VpPrice = VpRemoteCard.Prix
                    VpExtracted.Add(VpRemoteCard)
                ElseIf Math.Abs(VpRemoteCard.Prix - VpPrice) <= 0.1 Then
                    VpExtracted.Add(VpRemoteCard)
                End If
            End If
        Next VpRemoteCard
        Return VpExtracted
    End Function
    Private Function ChangeTransaction(VpFound As clsRemoteCard, VpCard As clsRemoteCard, VpBackups As List(Of clsBackupTransaction)) As Boolean
    '------------------------------------------------------------------------------------------------------------------------------
    'Modification d'une transaction au profit d'un vendeur pour un autre, et sauvegarde des informations pour annulation �ventuelle
    '------------------------------------------------------------------------------------------------------------------------------
    Dim VpN As Integer
        If VpFound.Vendeur IsNot Nothing Then
            VpN = Math.Min(VpCard.Bought, VpFound.Quantity)
            'Restitution au vendeur initial
            VpCard.Quantity += VpN
            VpCard.Bought -= VpN
            VpCard.Vendeur.Bought -= VpN
            'Acquisition aupr�s du nouveau vendeur
            VpFound.Quantity -= VpN
            VpFound.Bought += VpN
            VpFound.Vendeur.Bought += VpN
            'Sauvegarde en cas d'annulation
            VpBackups.Add(New clsBackupTransaction(VpCard, VpFound, VpN))
            Return True
        End If
        Return False
    End Function
    Private Sub CancelChanges(VpBackups As List(Of clsBackupTransaction))
    '---------------------------------------------------------------------------
    'Annulation des modifications effectu�es temporairement sur les transactions
    '---------------------------------------------------------------------------
        For Each VpBackup As clsBackupTransaction In VpBackups
            With VpBackup
                'R�acquisition aupr�s du vendeur initial
                .Before.Quantity -= .N
                .Before.Bought += .N
                .Before.Vendeur.Bought += .N
                'Restitution � l'ex-autre vendeur
                .After.Quantity += .N
                .After.Bought -= .N
                .After.Vendeur.Bought -= .N
            End With
        Next VpBackup
    End Sub
    Private Sub CalcTransactions(VpMaxTransactions As Integer, VpPath As String)
    '------------------------------------------------------------------------------------------------------------------------------
    'D�termine les transactions n�cessaires pour acheter toutes les cartes d�sir�es de la bonne mani�re (heuristique sous-optimale)
    '------------------------------------------------------------------------------------------------------------------------------
    Dim VpOutput As New StreamWriter(VpPath)                                        'Fichier contenant le r�sultat de l'optimisation
    Dim VpToBuy As List(Of clsLocalCard) = clsLocalCard.GetClone(VmToBuy)           'Liste des cartes demand�es � l'achat
    Dim VpToSell As List(Of clsRemoteCard) = clsRemoteCard.GetClone(VmToSell)       'Liste des cartes disponibles � la vente
    Dim VpSellers As New List(Of clsSeller)                                         'Liste des vendeurs sollicit�s
    Dim VpBackups As New List(Of clsBackupTransaction)                              'Liste des modifications des transactions effectu�es par rapport � la s�lection initiale
    Dim VpGroup As List(Of clsRemoteCard)                                           'Ensemble au m�me prix pour la carte courante
    Dim VpFound As clsRemoteCard                                                    'Carte envisag�e � l'achat en respect des crit�res
    Dim VpNewDeal As Boolean                                                        'Au moins une modification de transaction a eu lieu sur la sous-it�ration courante
    Dim VpN As Integer                                                              'Nombre de cartes dans la transaction courante
    Dim VpV, VpV1 As Integer                                                        'Nombre de vendeurs sollicit�s pour l'ensemble des transactions
    Dim VpPreTotal As Single                                                        'Co�t total pour l'ensemble des transactions avant optimisation finale par rapport aux frais de port
        'Calcule la disponibilit� des cartes et la couverture des vendeurs
        For Each VpLocalCard As clsLocalCard In VpToBuy
            For Each VpRemoteCard As clsRemoteCard In VpToSell
                If VpLocalCard.Name = VpRemoteCard.Name Then
                    VpLocalCard.Dispo += VpRemoteCard.Quantity
                    VpN = Math.Min(VpLocalCard.Quantity, VpRemoteCard.Quantity)
                    VpRemoteCard.Vendeur.Coverage += VpN
                End If
            Next VpRemoteCard
        Next VpLocalCard
        'Trie les cartes � acheter de la moins disponible (va impacter le choix du vendeur le plus n�cessaire) � la plus disponible
        VpToBuy.Sort(New clsLocalCard.clsLocalCardComparer)
        'Trie les cartes � vendre de la moins ch�re � la plus ch�re
        VpToSell.Sort(New clsRemoteCard.clsRemoteCardComparer)
        'Parcourt la liste des cartes � acheter
        For Each VpLocalCard As clsLocalCard In VpToBuy
            While VpLocalCard.Quantity > 0
                'Cherche le groupe de cartes � vendre correspondant � la carte courante d�sir�e pour le moins cher possible
                VpGroup = Me.Extract(VpLocalCard.Name, VpToSell)
                'Avertissement si aucun exemplaire de cette carte n'est disponible � la vente
                If VpGroup.Count = 0 Then
                    Call mdlToolbox.ShowWarning("Impossible d'acqu�rir la carte " + VpLocalCard.Name + "...")
                    Exit While
                End If
                VpFound = Nothing
                'Privil�gie un vendeur d�j� utilis�
                For Each VpRemoteCard As clsRemoteCard In VpGroup
                    If VpSellers.Contains(VpRemoteCard.Vendeur) Then
                        If VpFound Is Nothing OrElse VpRemoteCard.Prix < VpFound.Prix Then
                            VpFound = VpRemoteCard
                        End If
                    End If
                Next VpRemoteCard
                'Mais sinon prend ce qu'il y a de moins cher
                If VpFound Is Nothing Then
                    VpFound = VpGroup.Item(0)
                    VpSellers.Add(VpFound.Vendeur)
                End If
                'Acquisition
                VpN = Math.Min(VpLocalCard.Quantity, VpFound.Quantity)
                VpLocalCard.Quantity -= VpN
                VpFound.Quantity -= VpN
                VpFound.Bought += VpN
                VpFound.Vendeur.Bought += VpN
            End While
        Next VpLocalCard
        'Si on cherche � minimiser le nombre de transactions
        If Me.chkTransactions.Checked Then
            'Ajoute tous les vendeurs, m�me ceux pas encore sollicit�s
            For Each VpRemoteCard As clsRemoteCard In VpToSell
                If Not VpSellers.Contains(VpRemoteCard.Vendeur) Then
                    VpSellers.Add(VpRemoteCard.Vendeur)
                End If
            Next VpRemoteCard
            'Trie les vendeurs par couverture, de la plus petite � la plus grande
            VpSellers.Sort(New clsSeller.clsSellerComparer)
            VpV = clsSeller.GetCount(VpSellers)
            'Optimisation it�rative par r�duction du nombre de vendeurs � solliciter
            While True
                'Parcourt chaque vendeur en commen�ant par ceux dont on souhaite se passer
                For Each VpSeller1 As clsSeller In VpSellers
                    VpBackups.Clear
                    VpNewDeal = True
                    'Tant qu'on arrive � r�duire la sollicitation de ce vendeur
                    While VpNewDeal
                        VpNewDeal = False
                        'Parcourt les cartes qu'on lui a achet�es
                        For Each VpCard1 As clsRemoteCard In VpSeller1.GetCardsOfInterest(VpToSell, True)
                            VpFound = New clsRemoteCard(VpCard1.Name, Nothing, VpCard1.Edition, VpCard1.Language, VpCard1.Etat, VpCard1.Quantity, VpCard1.Bought, Single.PositiveInfinity)
                            'Parcourt les vendeurs susceptibles de proposer davantage de cartes que lui
                            For Each VpSeller2 As clsSeller In VpSellers
                                If VpSeller1 IsNot VpSeller2 AndAlso VpSeller2.Coverage > VpSeller1.Coverage Then
                                    'Recherche � qui il vaudrait mieux confier la transaction initiale pour minimiser le surco�t en se passant du vendeur dont on ne veut plus (dans tous les cas, le surco�t doit �tre inf�rieur � la valeur maximale estim�e pour les frais de port d'une carte)
                                    For Each VpCard2 As clsRemoteCard In VpSeller2.GetCardsOfInterest(VpToSell, False)
                                        If VpCard2.Name = VpFound.Name AndAlso VpCard2.Quantity > 0 AndAlso VpCard2.Prix <= VpFound.Prix AndAlso VpCard2.Prix - VpCard1.Prix < mdlConstGlob.CgWorstShippingCost Then
                                            VpFound = VpCard2
                                        End If
                                    Next VpCard2
                                End If
                            Next VpSeller2
                            'Effectue la modification de la transaction si on a trouv� un nouveau vendeur
                            VpNewDeal = VpNewDeal Or Me.ChangeTransaction(VpFound, VpCard1, VpBackups)
                        Next VpCard1
                        Application.DoEvents
                    End While
                    'Si on n'a pas r�ussi � transf�rer l'int�gralit� des transactions avec ce vendeur et qu'on est donc oblig� de le garder, on restaure toutes les transactions dans leur �tat initial puisqu'il �tait moins cher que les autres
                    If VpSeller1.Bought > 0 Then
                        Call Me.CancelChanges(VpBackups)
                    End If
                    'Si on a d�j� atteint le nombre de transactions souhait�, pas la peine de continuer
                    If clsSeller.GetCount(VpSellers) <= VpMaxTransactions Then
                        Exit For
                    End If
                Next VpSeller1
                'On arr�te d'optimiser d�s qu'on atteint le nombre de transactions souhait� ou qu'on ne peut plus le diminuer par rapport � l'it�ration pr�c�dente
                VpV1 = clsSeller.GetCount(VpSellers)
                If VpV1 <= VpMaxTransactions Or Not VpV1 < VpV Then
                    Exit While
                End If
                VpV = VpV1
            End While
            VpPreTotal = clsRemoteCard.GetTotal(VpToSell)
            'Optimisation compl�mentaire par rapport aux frais de port
            For Each VpSeller1 As clsSeller In VpSellers
                'Si on sollicite un vendeur pour un montant n'atteignant m�me pas les frais de port max, on va essayer de dispatcher les acquisitions ailleurs
                If clsRemoteCard.GetSubTotal(VpToSell, VpSeller1) <= mdlConstGlob.CgWorstShippingCost Then
                    VpBackups.Clear
                    'Parcourt les cartes qu'on lui a achet�es
                    For Each VpCard1 As clsRemoteCard In VpSeller1.GetCardsOfInterest(VpToSell, True)
                        VpFound = New clsRemoteCard(VpCard1.Name, Nothing, VpCard1.Edition, VpCard1.Language, VpCard1.Etat, VpCard1.Quantity, VpCard1.Bought, Single.PositiveInfinity)
                        'Parcourt les autres vendeurs qu'on a d�j� sollicit�s pour voir s'ils proposent la carte en question
                        For Each VpSeller2 As clsSeller In VpSellers
                            If VpSeller1 IsNot VpSeller2 Then
                                'Recherche � qui il vaudrait mieux acheter cette carte pour minimiser le surco�t en se passant du vendeur dont on ne veut plus
                                For Each VpCard2 As clsRemoteCard In VpSeller2.GetCardsOfInterest(VpToSell, False)
                                    If VpCard2.Name = VpFound.Name AndAlso VpCard2.Quantity > 0 AndAlso VpCard2.Prix <= VpFound.Prix AndAlso VpSeller2.Bought > 0 Then
                                        VpFound = VpCard2
                                    End If
                                Next VpCard2
                            End If
                            'Effectue la modification de la transaction si on a trouv� un autre vendeur
                            Call Me.ChangeTransaction(VpFound, VpCard1, VpBackups)
                        Next VpSeller2
                        Application.DoEvents
                    Next VpCard1
                    'Si on n'a pas r�ussi � transf�rer l'int�gralit� des transactions avec ce vendeur ou bien qu'au final �a co�te plus cher que les frais de port �conomis�s, on restaure toutes les transactions dans leur �tat initial
                    If VpSeller1.Bought > 0 OrElse clsRemoteCard.GetTotal(VpToSell) - VpPreTotal >= mdlConstGlob.CgShippingCost Then
                        Call Me.CancelChanges(VpBackups)
                    End If
                End If
            Next VpSeller1
            'Optimisation finale sur les cartes les plus ch�res si on a r�ussi (en particulier gr�ce � l'�tape pr�c�dente) � solliciter moins de vendeurs que ce que la consigne autorise
            VpBackups.Clear
            VpToSell.Reverse
            If clsSeller.GetCount(VpSellers) < VpMaxTransactions Then
                'Parcourt les cartes achet�es, de la plus ch�re � la moins ch�re
                For Each VpCard1 As clsRemoteCard In VpToSell
                    If VpCard1.Bought > 0 Then
                        VpFound = New clsRemoteCard(VpCard1.Name, Nothing, VpCard1.Edition, VpCard1.Language, VpCard1.Etat, VpCard1.Quantity, VpCard1.Bought, Single.PositiveInfinity)
                        'Recherche si la carte est disponible avec une �conomie sup�rieure aux frais de port engendr�s
                        For Each VpCard2 As clsRemoteCard In VpToSell
                            If VpCard2.Name = VpFound.Name AndAlso VpCard2.Quantity > 0 AndAlso VpCard2.Prix <= VpFound.Prix AndAlso VpCard1.Prix - VpCard2.Prix > mdlConstGlob.CgShippingCost Then
                                VpFound = VpCard2
                            End If
                        Next VpCard2
                        'Effectue la modification de la transaction si on a trouv� un meilleur vendeur
                        Call Me.ChangeTransaction(VpFound, VpCard1, VpBackups)
                        'Si on a atteint le nombre de transactions souhait�, il faut s'arr�ter
                        If clsSeller.GetCount(VpSellers) >= VpMaxTransactions Then
                            Exit For
                        End If
                    End If
                Next VpCard1
            End If
            'Avertissement si le nombre maximal de transactions autoris�es est d�pass�
            If clsSeller.GetCount(VpSellers) > VpMaxTransactions Then
                Call mdlToolbox.ShowWarning("Impossible d'effectuer tous les achats avec seulement " + VpMaxTransactions.ToString + " transaction(s)...")
            End If
        End If
        'R�capitulatif des transactions par vendeur
        VpOutput.WriteLine("------------------------------------")
        VpOutput.WriteLine("Total (hors frais de port) : " + clsRemoteCard.GetTotal(VpToSell).ToString + "�")
        VpOutput.WriteLine("Nombre de vendeurs : " + clsSeller.GetCount(VpSellers).ToString)
        VpOutput.WriteLine("------------------------------------")
        For Each VpSeller As clsSeller In VpSellers
            If VpSeller.Bought > 0 Then
                VpOutput.WriteLine(VpSeller.Name + " [" + VpSeller.Bought.ToString + " cartes pour " + clsRemoteCard.GetSubTotal(VpToSell, VpSeller).ToString + "�]")
                For Each VpRemoteCard As clsRemoteCard In VpSeller.GetCardsOfInterest(VpToSell, True)
                    VpOutput.WriteLine(vbTab + VpRemoteCard.Name + " - " + VpRemoteCard.Edition + " - " + VpRemoteCard.Bought.ToString + "x " + VpRemoteCard.Prix.ToString + "�")
                Next VpRemoteCard
            End If
        Next VpSeller
        VpOutput.Flush
        VpOutput.Close
        Me.txtTot.Text = clsRemoteCard.GetTotal(VpToSell).ToString + "�"
        Call mdlToolbox.ShowInformation("Optimisation termin�e !" + vbCrLf + "Les r�sultats vont s'afficher automatiquement dans le bloc-notes...")
    End Sub
    Private Sub InsertRow(VpS As String, VpN As Integer, VpCellModel As DataModels.IDataModel)
    '--------------------------------------
    'Chargement et insertion dans la grille
    '--------------------------------------
        With Me.grdBasket
            .Rows.Insert(.RowsCount)
            Me.grdBasket(.RowsCount - 1, 0) = New Cells.Cell(VpS)
            Me.grdBasket(.RowsCount - 1, 1) = New Cells.Cell(VpN)
            Me.grdBasket(.RowsCount - 1, 1).DataModel = VpCellModel
        End With
    End Sub
    Public Sub LoadGrid(VpMode As mdlConstGlob.eBasketMode)
    Dim VpCellModel As DataModels.IDataModel
        'Mode 1 : R�sultats de la recherche sur Internet
        If VpMode = mdlConstGlob.eBasketMode.Remote Then
            #If DEBUG Then
                Return
            #End If
            Me.grdBasket.SuspendLayout
            Application.UseWaitCursor = True
            'Pr�paration de la grille
            With Me.grdBasket
                Me.prgRefresh.Value = 0
                Me.prgRefresh.Maximum = VmToSell.Count
                'Nettoyage
                If .Rows.Count > 0 Then
                    .Rows.RemoveRange(0, .Rows.Count)
                End If
                'Nombre de colonnes et d'en-t�tes
                .ColumnsCount = 7
                .FixedRows = 1
                .Rows.Insert(0)
                Me.grdBasket(0, 0) = New Cells.ColumnHeader("Nom de la carte")
                Me.grdBasket(0, 1) = New Cells.ColumnHeader("Vendeur")
                Me.grdBasket(0, 2) = New Cells.ColumnHeader("Edition")
                Me.grdBasket(0, 3) = New Cells.ColumnHeader("Langue")
                Me.grdBasket(0, 4) = New Cells.ColumnHeader("Etat")
                Me.grdBasket(0, 5) = New Cells.ColumnHeader("Quantit�")
                Me.grdBasket(0, 6) = New Cells.ColumnHeader("Prix unitaire")
                CType(Me.grdBasket(0, 5), Cells.ColumnHeader).Comparer = New clsGridNumericComparer
                CType(Me.grdBasket(0, 6), Cells.ColumnHeader).Comparer = New clsGridNumericComparer
                'Remplissage offres
                For VpI As Integer = 0 To VmToSell.Count - 1
                    Application.DoEvents
                    .Rows.Insert(VpI + 1)
                    Me.grdBasket(VpI + 1, 0) = New Cells.Cell(VmToSell.Item(VpI).Name)
                    Me.grdBasket(VpI + 1, 1) = New Cells.Cell(VmToSell.Item(VpI).Vendeur.Name)
                    Me.grdBasket(VpI + 1, 2) = New Cells.Cell(VmToSell.Item(VpI).Edition)
                    Me.grdBasket(VpI + 1, 3) = New Cells.Cell(VmToSell.Item(VpI).Language)
                    Me.grdBasket(VpI + 1, 4) = New Cells.Cell(VmToSell.Item(VpI).Etat)
                    Me.grdBasket(VpI + 1, 5) = New Cells.Cell(VmToSell.Item(VpI).Quantity)
                    Me.grdBasket(VpI + 1, 6) = New Cells.Cell(VmToSell.Item(VpI).Prix.ToString + " �")
                    Me.prgRefresh.Increment(1)
                Next VpI
                .AutoSize
            End With
            Me.prgRefresh.Value = 0
            Application.UseWaitCursor = False
            Me.grdBasket.ResumeLayout
        'Mode 2 : Cartes � acheter
        Else
            VpCellModel = Utility.CreateDataModel(Type.GetType("System.Int32"))
            VpCellModel.EditableMode = EditableMode.AnyKey Or EditableMode.DoubleClick
            AddHandler VpCellModel.Validated, AddressOf CellValidated
            'Pr�paration de la grille
            With Me.grdBasket
                'Nettoyage
                If .Rows.Count > 0 Then
                    .Rows.RemoveRange(0, .Rows.Count)
                End If
                'Nombre de colonnes et d'en-t�tes
                .ColumnsCount = 2
                .FixedRows = 1
                .Rows.Insert(0)
                Me.grdBasket(0, 0) = New Cells.ColumnHeader("Nom de la carte")
                Me.grdBasket(0, 1) = New Cells.ColumnHeader("Quantit�")
                'Remplissage panier
                For Each VpCard As clsLocalCard In VmToBuy
                    Call Me.InsertRow(VpCard.Name, VpCard.Quantity, VpCellModel)
                Next VpCard
                .AutoSize
            End With
        End If
    End Sub
    Private Sub BasketSave(VpPath As String)
    '--------------------
    'Sauvegarde du panier
    '--------------------
    Dim VpCapsule As New clsCapsuleCards
    Dim VpXmlSerializer As New XmlSerializer(GetType(clsCapsuleCards), New Type() {GetType(clsLocalCard), GetType(clsRemoteCard), GetType(clsSeller)})
    Dim VpFile As FileStream
    Dim VpWriter As XmlTextWriter
        VpCapsule.ToBuy = VmToBuy
        VpCapsule.ToSell = VmToSell
        VpFile = New FileStream(VpPath, FileMode.Create)
        VpWriter = New XmlTextWriter(VpFile, Nothing)
        VpXmlSerializer.Serialize(VpWriter, VpCapsule)
        VpWriter.Close
        VpFile.Close
    End Sub
    Private Sub BasketLoad(VpPath As String)
    '----------------------
    'Chargement d'un panier
    '----------------------
    Dim VpCapsule As clsCapsuleCards
    Dim VpXmlSerializer As New XmlSerializer(GetType(clsCapsuleCards), New Type() {GetType(clsLocalCard), GetType(clsRemoteCard), GetType(clsSeller)})
    Dim VpFile As New FileStream(VpPath, FileMode.Open)
    Dim VpReader As New XmlTextReader(VpFile)
        VpCapsule = CType(VpXmlSerializer.Deserialize(VpReader), clsCapsuleCards)
        VpReader.Close
        VpFile.Close
        VmToBuy = VpCapsule.ToBuy
        VmToSell = VpCapsule.ToSell
        Call Me.LoadGrid(eBasketMode.Local)
        If VmToSell.Count > 0 Then
            If Me.chkSeller.Checked Then
                Call Me.ExcludeSellers
            End If
            Call Me.ClearSellList
            Call Me.LoadGrid(mdlConstGlob.eBasketMode.Remote)
            Me.cmdCalc.Enabled = True
        End If
    End Sub
    #End Region
    #Region "Ev�nements"
    Private Sub CellValidated(sender As Object, e As CellEventArgs)
    Dim VpQ As Short
        With mdlConstGlob.VgSessionSettings
            If Not .GridClearing Then
                 .GridClearing = True
                VmToBuy.Clear
                'Mise � jour de la quantit� dans la grille et dans le List(Of )
                For VpI As Integer = 1 To Me.grdBasket.RowsCount - 1
                    VpQ = Me.grdBasket(VpI, 1).Value
                    If VpQ > 0 Then
                        VmToBuy.Add(New clsLocalCard(Me.grdBasket(VpI, 0).Value, VpQ, 0))
                    End If
                Next VpI
                Call Me.LoadGrid(mdlConstGlob.eBasketMode.Local)
                 .GridClearing = False
            End If
        End With
    End Sub
    Sub BtLocalBasketActivate(ByVal sender As Object, ByVal e As EventArgs)
        Call Me.LoadGrid(mdlConstGlob.eBasketMode.Local)
    End Sub
    Sub BtRemoteBasketActivate(ByVal sender As Object, ByVal e As EventArgs)
        Call Me.LoadGrid(mdlConstGlob.eBasketMode.Remote)
    End Sub
    Sub CmdRefreshClick(ByVal sender As Object, ByVal e As EventArgs)
    Dim VpQualities As New List(Of mdlConstGlob.eQuality)
    Dim VpBannedSellers() As String = Nothing
        'Information utilisateur
        If VmToBuy.Count = 0 Then
            Call mdlToolbox.ShowInformation("Choisissez des cartes � acheter via clic droit dans l'explorateur !")
            Exit Sub
        End If
        'Blocage d'actions ind�sirables
        Me.cmdRefresh.Visible = False
        Me.cmdCancel.Tag = False
        Me.cmdCancel.Visible = True
        Me.prgRefresh.Maximum = VmToBuy.Count
        Me.prgRefresh.Value = 0
        'Qualit�s autoris�es pour les cartes � acheter
        If Me.chkMint.Checked Then VpQualities.Add(mdlConstGlob.eQuality.Mint)
        If Me.chkNearMint.Checked Then VpQualities.Add(mdlConstGlob.eQuality.NearMint)
        If Me.chkExcellent.Checked Then VpQualities.Add(mdlConstGlob.eQuality.Excellent)
        If Me.chkGood.Checked Then VpQualities.Add(mdlConstGlob.eQuality.Good)
        'Vendeurs exclus
        If Me.chkSeller.Checked Then
            VpBannedSellers = VgOptions.VgSettings.BannedSellers.ToLower.Split("#")
        End If
        'Efface les r�sultats pr�c�dents
        VmToSell.Clear
        Try
            'R�cup�re les informations pour chaque carte
            For Each VpCard As clsLocalCard In VmToBuy
                Application.DoEvents
                'V�rifie qu'on n'a pas demand� � annuler la recherche
                If Me.cmdCancel.Tag = True Then
                    Exit For
                End If
                'Choix de la market place
                Select Case VmServer
                    Case mdlConstGlob.eMarketServer.MagicVille
                        Call Me.MVFetch(VpCard.Name, VpQualities.ToArray, VpBannedSellers)
                    Case mdlConstGlob.eMarketServer.MagicCardMarket
                        Call Me.MKMFetch(VpCard.Name, VpQualities.ToArray, VpBannedSellers)
                    Case Else
                End Select
                Me.prgRefresh.Increment(1)
            Next VpCard
            'Finalisation et affichage des r�sultats dans la grid
            Me.prgRefresh.Value = 0
            Application.DoEvents
            Call Me.ClearSellList
            Call Me.LoadGrid(mdlConstGlob.eBasketMode.Remote)
        Catch
            Call mdlToolbox.ShowWarning(mdlConstGlob.CgDL3b)
        End Try
        'Restauration des actions bloqu�es
        Me.cmdRefresh.Visible = True
        Me.cmdCancel.Visible = False
        Me.cmdCalc.Enabled = True
    End Sub
    Sub CmdCalcClick(ByVal sender As Object, ByVal e As EventArgs)
    Dim VpPath As String = mdlToolbox.GetFreeTempFile(".txt")
        Me.prgRefresh.Style = ProgressBarStyle.Marquee
        Cursor.Current = Cursors.WaitCursor
        Call Me.CalcTransactions(Me.sldTransactions.Value, VpPath)
        If File.Exists(VpPath) Then
            Process.Start(VpPath)
        End If
        Me.prgRefresh.Style = ProgressBarStyle.Blocks
        Me.prgRefresh.Value = 0
    End Sub
    Sub FrmBuyCardsLoad(ByVal sender As Object, ByVal e As EventArgs)
        'Astuce horrible pour contourner un bug de mise � l'�chelle automatique en fonction de la densit� de pixels
        If Me.CreateGraphics().DpiX <> 96 Then
            Me.splitH.SplitterDistance = 140 * Me.CreateGraphics().DpiX / 96
        End If
        'Fixe la position du s�parateur
        VmSplitterDistance = Me.splitH.SplitterDistance
        'Charge la liste des vendeurs exclus
        For Each VpSeller As String In VgOptions.VgSettings.BannedSellers.Split("#")
            If VpSeller <> "" Then
                Me.lstSeller.Items.Add(VpSeller)
            End If
        Next VpSeller
    End Sub
    Sub FrmBuyCardsResize(sender As Object, e As EventArgs)
        Me.splitH.SplitterDistance = VmSplitterDistance
    End Sub
    Sub LstSellerMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs)
    Dim VpItem As Integer = Me.lstSeller.IndexFromPoint(e.Location)
        'Menu contextuel ajout / suppression de vendeurs exclus
        If e.Button = System.Windows.Forms.MouseButtons.Right Then
            Me.mnuRemoveSeller.Enabled = ( VpItem >= 0 )
            Me.cmnuSeller.Show(sender, e.Location)
        End If
    End Sub
    Sub ChkSellerCheckedChanged(sender As Object, e As EventArgs)
        If Me.chkSeller.Checked Then
            If VmToSell.Count > 0 Then
                Application.DoEvents
                Call Me.ExcludeSellers
                Cursor.Current = Cursors.WaitCursor
                Call Me.LoadGrid(mdlConstGlob.eBasketMode.Remote)
            End If
        End If
    End Sub
    Sub MnuAddSellerClick(ByVal sender As Object, ByVal e As EventArgs)
    Dim VpStr As String = InputBox("Quel est le nom du vendeur � exclure ?", "Ajout d'un vendeur dans la liste noire", "(Vendeur)")
        VpStr = VpStr.Trim
        If VpStr <> "" Then
            With VgOptions
                .VgSettings.BannedSellers = VpStr + "#" + .VgSettings.BannedSellers
                Call .SaveSettings
            End With
            Me.lstSeller.Items.Add(VpStr)
            If VmToSell.Count > 0 And Me.chkSeller.Checked Then
                Application.DoEvents
                Call Me.ExcludeSellers
                Call Me.LoadGrid(mdlConstGlob.eBasketMode.Remote)
            End If
        End If
    End Sub
    Sub MnuRemoveSellerClick(ByVal sender As Object, ByVal e As EventArgs)
        If Me.lstSeller.SelectedIndex >= 0 Then
            With VgOptions
                .VgSettings.BannedSellers = .VgSettings.BannedSellers.Replace(Me.lstSeller.SelectedItem + "#", "")
                Call .SaveSettings
            End With
            Me.lstSeller.Items.RemoveAt(Me.lstSeller.SelectedIndex)
        End If
    End Sub
    Sub BtSaveBasketActivate(sender As Object, e As EventArgs)
        Me.dlgSave.FileName = ""
        Me.dlgSave.ShowDialog
        If Me.dlgSave.FileName <> "" Then
            Call Me.BasketSave(Me.dlgSave.FileName)
        End If
    End Sub
    Sub BtLoadBasketActivate(sender As Object, e As EventArgs)
        Me.dlgOpen.FileName = ""
        Me.dlgOpen.ShowDialog
        If Me.dlgOpen.FileName <> "" Then
            Call Me.BasketLoad(Me.dlgOpen.FileName)
        End If
    End Sub
    Sub CmdCancelClick(sender As Object, e As EventArgs)
        Me.cmdCancel.Tag = True
        Me.cmdCancel.Visible = False
        Me.cmdRefresh.Visible = True
    End Sub
    Sub BrowserDocumentCompleted(ByVal sender As Object, ByVal e As WebBrowserDocumentCompletedEventArgs)
        VmIsComplete = True
    End Sub
    #End Region
    #Region "Propri�t�s"
    Public WriteOnly Property Editions As List(Of String)
        Set (VpEditions As List(Of String))
            VmEditions = VpEditions
        End Set
    End Property
    #End Region
End Class
