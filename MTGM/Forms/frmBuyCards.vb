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
    #Region "Déclarations"
    Private VmServer As mdlConstGlob.eMarketServer      'Market place choisie par l'utilisateur pour effectuer ses transactions
    Private VmToBuy As New List(Of clsLocalCard)        'Collection des cartes souhaitées à l'achat
    Private VmToSell As New List(Of clsRemoteCard)      'Collection des cartes disponibles à la vente
    Private VmProducts As New List(Of clsProduct)       'Collection des offres exploitables sous forme statistique
    Private VmEditions As List(Of String)               'Liste des éditions disponibles pour une même carte dans le cas où il y en a trop et qu'il faut potentiellement restreindre le volume de recherches
    Private VmSplitterDistance As Integer               'Position du séparateur
    Private VmBrowser As New WebBrowser                 'Navigateur web
    Private VmIsComplete As Boolean = False             'Page complètement affichée dans le navigateur
    #End Region
    #Region "Méthodes"
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
    'Navigue sur la page passée en paramètre en respectant le délai d'expiration
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
    'Incrémente la quantité du panier pour la carte passée en paramètre
    '------------------------------------------------------------------
        'Carte déjà mise au panier
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
    'Supprime les entrées de la liste dont la quantité vaut 0
    '--------------------------------------------------------
    Dim VpToRemove As New List(Of clsRemoteCard)
        'Récupération des éléments à supprimer
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
    'Supprime les cartes proposées par des vendeurs bannis
    '-----------------------------------------------------
    Dim VpToRemove As New List(Of clsRemoteCard)
    Dim VpBannedSellers() As String = VgOptions.VgSettings.BannedSellers.ToLower.Split("#")
        If VpBannedSellers IsNot Nothing AndAlso VpBannedSellers.Length > 0 Then
            'Récupération des éléments à supprimer
            For Each VpRemoteCard As clsRemoteCard In VmToSell
                If Array.IndexOf(VpBannedSellers, VpRemoteCard.Seller.Name.ToLower) >= 0 Then
                    VpToRemove.Add(VpRemoteCard)
                End If
            Next VpRemoteCard
            'Suppression effective
            For Each VpDelete As clsRemoteCard In VpToRemove
                VmToSell.Remove(VpDelete)
            Next VpDelete
        End If
    End Sub
    Private Sub Classify
    '---------------------------------------------------------------------------
    'Réorganise les cartes disponibles à la vente sous forme de produits groupés
    '---------------------------------------------------------------------------
    Dim VpProduct As clsProduct
        VmProducts.Clear
        For Each VpLocalCard As clsLocalCard In VmToBuy
            VpProduct = New clsProduct(VpLocalCard.Name)
            For Each VpRemoteCard As clsRemoteCard In VmToSell
                If VpRemoteCard.Name = VpProduct.Name Then
                    VpProduct.AddOffer(VpRemoteCard)
                End If
            Next VpRemoteCard
            VmProducts.Add(VpProduct)
        Next VpLocalCard
    End Sub
    Private Sub MVFetch(VpCard As String, VpQualities() As mdlConstGlob.eQuality, VpBannedSellers() As String)
    '--------------------------------------------------------------------------------------------------------------
    'Se connecte sur Magic-Ville pour récupérer les informations de ventes relatives à la carte passée en paramètre
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
        'Page intermédiaire (ne s'affiche qu'en cas d'ambiguité)
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
            If VpElement.InnerText = "Achetez cette carte à un magicvillois" Then
                'Validation
                VpElement.InvokeMember("click")
                Call Me.BrowseAndWait
                Exit For
            End If
        Next VpElement
        'Parsing des propriétés
        For Each VpElement In VmBrowser.Document.All
            If VpElement.Name.Contains("[") Then
                VpCurId = Val(VpElement.Name.Substring(VpElement.Name.IndexOf("[") + 1))
                'Si l'identifiant a changé, c'est qu'on est sur une nouvelle entrée
                If VpCurId <> VpLastId Then
                    VpRemoteCard = New clsRemoteCard(VpCard)
                    VmToSell.Add(VpRemoteCard)
                    VpLastId = VpCurId
                End If
                'Remplissage des propriétés au fur et à mesure
                With VpRemoteCard
                    If VpElement.Name.Contains("qte") Then
                        .Quantity = CInt(VpElement.InnerText.Substring(VpElement.InnerText.Length - 1))
                    ElseIf VpElement.Name.Contains("ref") Then
                        .Edition = VpElement.GetAttribute("value")
                    ElseIf VpElement.Name.Contains("lang") Then
                        .Language = mdlToolbox.MyLanguage(VpElement.GetAttribute("value"))
                    ElseIf VpElement.Name.Contains("etat") Then
                        .Condition = VpElement.GetAttribute("value")
                    ElseIf VpElement.Name.Contains("seller") Then
                        .Seller = New clsSeller(VpElement.GetAttribute("value"))
                    End If
                End With
            ElseIf VpElement.GetAttribute("color") = "#3333ff" Then
                VpRemoteCard.Price = mdlToolbox.MyVal(VpElement.InnerText)
            End If
        Next VpElement
        'Supprime de la collection tout ce qui ne respecte pas les critères actifs
        For Each VpRemoteCard In VmToSell
            With VpRemoteCard
                If Not ( Array.IndexOf(mdlConstGlob.CgBuyLanguage, .Language.ToLower) >= 0 AndAlso Array.IndexOf(VpQualities, .Condition) >= 0 AndAlso (VpBannedSellers Is Nothing OrElse VpBannedSellers.Length = 0 OrElse Array.IndexOf(VpBannedSellers, .Seller.Name.ToLower) < 0) ) Then
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
    'Se connecte sur MagicCardMarket pour récupérer les informations de ventes relatives à la carte passée en paramètre
    '------------------------------------------------------------------------------------------------------------------
    Dim VpProducts As clsProductRequest = Nothing
    Dim VpArticles As clsArticleRequest = Nothing
    Dim VpFilterEditions As frmBuySettings
        'Commence par effectuer une recherche sur le nom pour récupérer l'id produit
        VpProducts = MKMRequest(Of clsProductRequest)(CgURL24.Replace("card-name", Uri.EscapeDataString(VpCard.ToLower)))
        'S'il y a trop d'id produits disponibles (trop d'éditions pour une même carte), on propose de restreindre les requêtes
        VmEditions = Nothing
        If VpProducts.product.Count > mdlConstGlob.CgMaxEditionsMarket Then
            VpFilterEditions = New frmBuySettings(VpCard, Me, VpProducts.product)
            VpFilterEditions.ShowDialog
        End If
        'Récupération des disponibilités du produit sur les éditions retenues
        For Each VpProduct As clsProductRequest.clsProduct In VpProducts.product
            If VmEditions Is Nothing OrElse VmEditions.Contains(VpProduct.expansion) Then
                VpArticles = MKMRequest(Of clsArticleRequest)(Uri.EscapeUriString(CgURL25 + VpProduct.idProduct.ToString))
                'Ajout à la collection si respect des critères actifs
                For Each VpArticle As clsArticleRequest.clsArticle In VpArticles.article
                    With VpArticle
                        If Array.IndexOf(mdlConstGlob.CgBuyLanguage, .language.languageName.ToLower) >= 0 AndAlso Array.IndexOf(VpQualities, mdlToolbox.MyQuality(.condition)) >= 0 AndAlso (VpBannedSellers Is Nothing OrElse VpBannedSellers.Length = 0 OrElse Array.IndexOf(VpBannedSellers, .seller.username.ToLower) < 0) Then
                            VmToSell.Add(New clsRemoteCard(VpCard, New clsSeller(.seller.username, .seller.country), VpProduct.expansion, .language.languageName, mdlToolbox.MyQuality(.condition), .count, 0, .price, Single.NaN))
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
        VpURL = VpURL.Replace("'", "%27")   'cas non gérés par EscapeUriString
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
    'Renvoie la sous-liste de toutes les cartes de même prix non encore achetées correspondant au nom de carte demandé (la liste est déjà triée)
    '-------------------------------------------------------------------------------------------------------------------------------------------
    Dim VpExtracted As New List(Of clsRemoteCard)
    Dim VpPrice As Single = -1
        For Each VpRemoteCard As clsRemoteCard In VpToSell
            If VpRemoteCard.Name = VpCard AndAlso VpRemoteCard.Quantity > 0 Then
                If VpPrice = -1 Then
                    VpPrice = VpRemoteCard.PriceWithShipping
                    VpExtracted.Add(VpRemoteCard)
                ElseIf Math.Abs(VpRemoteCard.PriceWithShipping - VpPrice) <= 0.1 Then   'pour simplifier, jusqu'à 10 centimes d'écart, on considère que c'est le même prix
                    VpExtracted.Add(VpRemoteCard)
                End If
            End If
        Next VpRemoteCard
        Return VpExtracted
    End Function
    Private Sub ChangeTransaction(VpFound As clsRemoteCard, VpCard As clsRemoteCard)
    '------------------------------------------------------------------------------------------------------------------------------
    'Modification d'une transaction au profit d'un vendeur pour un autre, et sauvegarde des informations pour annulation éventuelle
    '------------------------------------------------------------------------------------------------------------------------------
    Dim VpN As Integer
        If VpFound.Seller IsNot Nothing Then
            VpN = Math.Min(VpCard.Bought, VpFound.Quantity)
            'Restitution au vendeur initial
            VpCard.Quantity += VpN
            VpCard.Bought -= VpN
            VpCard.Seller.Bought -= VpN
            VpCard.Seller.BoughtValue -= VpCard.Price * VpN
            'Acquisition auprès du nouveau vendeur
            VpFound.Quantity -= VpN
            VpFound.Bought += VpN
            VpFound.Seller.Bought += VpN
            VpFound.Seller.BoughtValue += VpFound.Price * VpN
        End If
    End Sub
    Private Sub CalcTransactions(VpShippingPolicies As Dictionary(Of String, List(Of clsShipping)), VpMinParcelValue As Single, VpPath As String)
    '------------------------------------------------------------------------------------------------------------------------------
    'Détermine les transactions nécessaires pour acheter toutes les cartes désirées de la bonne manière (heuristique sous-optimale)
    '------------------------------------------------------------------------------------------------------------------------------
    Dim VpOpti As New clsOptiBuyResult                                              'Résultat synthétique de l'optimisation
    Dim VpOutput As New StreamWriter(VpPath)                                        'Fichier contenant le résultat détaillé de l'optimisation
    Dim VpAllSellers As New List(Of clsSeller)                                      'Liste de tous les vendeurs
    Dim VpSellers As New List(Of clsSeller)                                         'Liste des vendeurs sollicités
    Dim VpToBuy As List(Of clsLocalCard) = clsLocalCard.GetClone(VmToBuy)           'Liste des cartes demandées à l'achat
    Dim VpToSell As List(Of clsRemoteCard) = clsRemoteCard.GetClone(VmToSell)       'Liste des cartes disponibles à la vente
    Dim VpGroup As List(Of clsRemoteCard)                                           'Ensemble au même prix pour la carte courante
    Dim VpFound As clsRemoteCard                                                    'Carte envisagée à l'achat en respectant les critères
    Dim VpN As Integer                                                              'Nombre de cartes dans la transaction courante
        'Associe chaque carte disponible à la vente à son vendeur et calcule les frais de port de chaque carte
        For Each VpRemoteCard As clsRemoteCard In VpToSell
            'Association croisée
            VpRemoteCard.Seller.Cards.Add(VpRemoteCard)
            If Not VpAllSellers.Contains(VpRemoteCard.Seller) Then
                VpAllSellers.Add(VpRemoteCard.Seller)
            End If
            'Frais de port
            If VpShippingPolicies.ContainsKey(VpRemoteCard.Seller.Country) Then
                'Calcule les frais de port de chaque carte comme si elle était achetée seule
                VpRemoteCard.ShippingCost = clsShipping.GetShippingCostFor(1, VpRemoteCard.Price, VpShippingPolicies.Item(VpRemoteCard.Seller.Country))
            Else
                Call mdlToolbox.ShowWarning("Frais de port inconnus depuis " + VpRemoteCard.Seller.Country + "...")
                'Ajoute une info (volontairement fausse) sur les frais de port manquants afin d'éviter tout crash dans la suite de la méthode
                'De toute façon, le montant du port sera tellement élevé que normalement aucune carte n'y sera associée
                VpShippingPolicies.Add(VpRemoteCard.Seller.Country, clsShipping.CreateListFrom(New clsShipping("", VpRemoteCard.Seller.Country, Single.MaxValue, Integer.MaxValue, mdlConstGlob.CgWorstShippingCost)))
                VpRemoteCard.ShippingCost = mdlConstGlob.CgWorstShippingCost
            End If
            If VpRemoteCard.Seller.ShippingPolicy Is Nothing Then
                'Conserve la politique en matière de frais de port pour le vendeur de la carte courante si ce n'est pas déjà fait
                VpRemoteCard.Seller.ShippingPolicy = VpShippingPolicies.Item(VpRemoteCard.Seller.Country)
            End If
        Next VpRemoteCard
        'Calcule la disponibilité des cartes et la couverture des vendeurs
        For Each VpSeller As clsSeller In VpAllSellers
            For Each VpLocalCard As clsLocalCard In VpToBuy
                VpN = 0
                For Each VpRemoteCard As clsRemoteCard In VpSeller.Cards
                    If VpLocalCard.Name = VpRemoteCard.Name Then
                        VpLocalCard.Availability += VpRemoteCard.Quantity
                        VpN += VpRemoteCard.Quantity
                    End If
                Next VpRemoteCard
                VpSeller.Coverage += Math.Min(VpLocalCard.Quantity, VpN)
            Next VpLocalCard
        Next VpSeller
        'Trie les cartes à acheter de la moins disponible (va impacter le choix du vendeur le plus nécessaire) à la plus disponible
        VpToBuy.Sort(New clsLocalCard.clsLocalCardComparer)
        'Trie les cartes à vendre de la moins chère à la plus chère (en intégrant les frais de port unitaires)
        VpToSell.Sort(New clsRemoteCard.clsRemoteCardComparer)
        'Trie les vendeurs par couverture, de la plus grande à la plus petite
        VpAllSellers.Sort(New clsSeller.clsSellerCoverageComparer)
        'Parcourt la liste des cartes à acheter
        For Each VpLocalCard As clsLocalCard In VpToBuy
            While VpLocalCard.Quantity > 0
                'Cherche le groupe de cartes à vendre correspondant à la carte courante désirée pour le moins cher possible
                VpGroup = Me.Extract(VpLocalCard.Name, VpToSell)
                'Avertissement si aucun exemplaire de cette carte n'est disponible à la vente
                If VpGroup.Count = 0 Then
                    Call mdlToolbox.ShowWarning("Impossible d'acquérir la carte " + VpLocalCard.Name + "...")
                    Exit While
                End If
                VpFound = Nothing
                'Privilégie un vendeur déjà sollicité
                For Each VpRemoteCard As clsRemoteCard In VpGroup
                    If VpSellers.Contains(VpRemoteCard.Seller) Then
                        'Cherche celui qui propose la carte au prix le plus bas (en intégrant le surcoût de frais de port)
                        If VpFound Is Nothing OrElse VpRemoteCard.Price + VpRemoteCard.Seller.GetMarginalShippingCostFor(Math.Min(VpLocalCard.Quantity, VpRemoteCard.Quantity), VpRemoteCard.Price) < VpFound.Price + VpFound.Seller.GetMarginalShippingCostFor(Math.Min(VpLocalCard.Quantity, VpFound.Quantity), VpFound.Price) Then
                            VpFound = VpRemoteCard
                        End If
                    End If
                Next VpRemoteCard
                'Si aucun vendeur parmi ceux déjà sollicités ne dispose de la carte, on en sollicite un nouveau (celui qui propose la carte au prix le plus bas, y compris avec ses frais de port)
                If VpFound Is Nothing Then
                    VpFound = VpGroup.Item(0)
                    VpSellers.Add(VpFound.Seller)
                End If
                'Acquisition
                VpN = Math.Min(VpLocalCard.Quantity, VpFound.Quantity)
                VpLocalCard.Quantity -= VpN
                VpFound.Quantity -= VpN
                VpFound.Bought += VpN
                VpFound.Seller.Bought += VpN
                VpFound.Seller.BoughtValue += VpFound.Price * VpN
            End While
        Next VpLocalCard
        'Trie les vendeurs par montant de transaction, du plus petit au plus grand
        VpSellers.Sort(New clsSeller.clsSellerBoughtValueComparer)
        'Il faut à présent dispatcher les cartes achetées auprès des vendeurs pour lesquels le montant minimal de transaction n'est pas atteint
        If VpMinParcelValue > 0 Then
            For Each VpSeller As clsSeller In VpSellers
                If VpSeller.BoughtValue < VpMinParcelValue Then
                    For Each VpRemoteCard As clsRemoteCard In VpSeller.GetBoughtCards
                        'Tant qu'il reste des cartes à acquérir auprès de ce vendeur pour lequel la valeur achetée n'atteint pas le seuil
                        While VpRemoteCard.Bought > 0
                            VpFound = Nothing
                            For Each VpSeller1 As clsSeller In VpSellers
                                'Parmi les vendeurs déjà sollicités (et pour lesquels ce seuil est bien atteint)                            
                                If VpSeller1.BoughtValue >= VpMinParcelValue Then
                                    'Regarde les cartes qu'ils proposent
                                    For Each VpRemoteCard1 As clsRemoteCard In VpSeller1.Cards
                                        'Si on trouve parmi elles celle(s) qu'on voulait redispatcher
                                        If VpRemoteCard.Name = VpRemoteCard1.Name AndAlso VpRemoteCard1.Quantity > 0 Then
                                            'On choisit celle au prix le plus bas (en intégrant le surcoût de frais de port)
                                            If VpFound Is Nothing OrElse VpRemoteCard1.Price + VpRemoteCard1.Seller.GetMarginalShippingCostFor(Math.Min(VpRemoteCard.Quantity, VpRemoteCard1.Quantity), VpRemoteCard1.Price) < VpFound.Price + VpFound.Seller.GetMarginalShippingCostFor(Math.Min(VpRemoteCard.Quantity, VpFound.Quantity), VpFound.Price) Then
                                                VpFound = VpRemoteCard1
                                            End If
                                        End If
                                    Next VpRemoteCard1
                                End If
                            Next VpSeller1
                            'Si on a trouvé, effectue la modification de transaction
                            If VpFound IsNot Nothing Then
                                Call Me.ChangeTransaction(VpFound, VpRemoteCard)
                            'Note si on n'a pas réussi à se passer du vendeur
                            Else
                                Call VpOpti.AddSmallTransaction(VpRemoteCard.Seller)
                                Exit While
                            End If
                        End While
                    Next VpRemoteCard
                End If
            Next VpSeller
        End If
        'Récapitulatif des transactions vendeur par vendeur
        For Each VpSeller As clsSeller In VpSellers
            If VpSeller.Bought > 0 Then
                VpOutput.WriteLine(VpSeller.Name + " (" + VpSeller.Country + ")" + " [" + VpSeller.Bought.ToString + " cartes pour " + VpSeller.BoughtValue.ToString + "€ + " + VpSeller.ShippingCost.ToString + "€ de port]")
                For Each VpRemoteCard As clsRemoteCard In VpSeller.GetBoughtCards
                    VpOutput.WriteLine(vbTab + VpRemoteCard.Name + " - " + VpRemoteCard.Edition + " - " + VpRemoteCard.Bought.ToString + "x " + VpRemoteCard.Price.ToString + "€")
                Next VpRemoteCard
                VpOpti.Bought += VpSeller.Bought
                VpOpti.SellersCount += 1
                VpOpti.CardsCost += VpSeller.BoughtValue
                VpOpti.ShippingCost += VpSeller.ShippingCost
            End If
        Next VpSeller
        VpOutput.WriteLine("")
        VpOutput.WriteLine("------------------------------------")
        VpOutput.WriteLine("Nombre de cartes          : " + VpOpti.Bought.ToString)
        VpOutput.WriteLine("Nombre de vendeurs        : " + VpOpti.SellersCount.ToString)
        VpOutput.WriteLine("Montant des cartes        : " + VpOpti.CardsCost.ToString + "€")
        VpOutput.WriteLine("Montant des frais de port : " + VpOpti.ShippingCost.ToString + "€")
        VpOutput.WriteLine("Total                     : " + VpOpti.GrandTotal.ToString + "€")
        VpOutput.WriteLine("------------------------------------")
        VpOutput.Flush
        VpOutput.Close
        Me.txtTot.Text = VpOpti.CardsCost.ToString + "€"
        Call mdlToolbox.ShowInformation("Optimisation terminée !" + vbCrLf + VpOpti.Warning(VpMinParcelValue) + "Les résultats vont s'afficher automatiquement dans le bloc-notes...")
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
        'Mode 1 : Résultats de la recherche sur Internet
        If VpMode = mdlConstGlob.eBasketMode.Remote Then
            'Préparation de la grille
            Me.grdBasket.SuspendLayout
            With Me.grdBasket
                'Nettoyage
                If .Rows.Count > 0 Then
                    .Rows.RemoveRange(0, .Rows.Count)
                End If
                'Nombre de colonnes et d'en-têtes
                .ColumnsCount = 6
                .FixedRows = 1
                .Rows.Insert(0)
                Me.grdBasket(0, 0) = New Cells.ColumnHeader("Nom")
                Me.grdBasket(0, 1) = New Cells.ColumnHeader("Editions")
                Me.grdBasket(0, 2) = New Cells.ColumnHeader("Vendeurs")
                Me.grdBasket(0, 3) = New Cells.ColumnHeader("Quantité")
                Me.grdBasket(0, 4) = New Cells.ColumnHeader("Moy./vendeur")
                Me.grdBasket(0, 5) = New Cells.ColumnHeader("Prix médian")
                For VpI As Integer = 1 To 5
                    CType(Me.grdBasket(0, VpI), Cells.ColumnHeader).Comparer = New clsGridNumericComparer
                Next VpI
                'Remplissage offres
                For VpI As Integer = 0 To VmProducts.Count - 1
                    Application.DoEvents
                    .Rows.Insert(VpI + 1)
                    Me.grdBasket(VpI + 1, 0) = New Cells.Cell(VmProducts(VpI).Name)
                    Me.grdBasket(VpI + 1, 1) = New Cells.Cell(VmProducts(VpI).Editions.ToString)
                    Me.grdBasket(VpI + 1, 2) = New Cells.Cell(VmProducts(VpI).Sellers.ToString)
                    Me.grdBasket(VpI + 1, 3) = New Cells.Cell(VmProducts(VpI).Quantity.ToString)
                    Me.grdBasket(VpI + 1, 4) = New Cells.Cell(VmProducts(VpI).MeanQuantity.ToString("F2"))
                    Me.grdBasket(VpI + 1, 5) = New Cells.Cell(VmProducts(VpI).MedianPrice.ToString("F2") + " €")
                Next VpI
                .AutoSize
            End With
            Me.grdBasket.ResumeLayout
        'Mode 2 : Cartes à acheter
        Else
            VpCellModel = Utility.CreateDataModel(Type.GetType("System.Int32"))
            VpCellModel.EditableMode = EditableMode.AnyKey Or EditableMode.DoubleClick
            AddHandler VpCellModel.Validated, AddressOf CellValidated
            'Préparation de la grille
            With Me.grdBasket
                'Nettoyage
                If .Rows.Count > 0 Then
                    .Rows.RemoveRange(0, .Rows.Count)
                End If
                'Nombre de colonnes et d'en-têtes
                .ColumnsCount = 2
                .FixedRows = 1
                .Rows.Insert(0)
                Me.grdBasket(0, 0) = New Cells.ColumnHeader("Nom de la carte")
                Me.grdBasket(0, 1) = New Cells.ColumnHeader("Quantité")
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
        Call Me.LoadGrid(mdlConstGlob.eBasketMode.Local)
        If VmToSell.Count > 0 Then
            If Me.chkSeller.Checked Then
                Call Me.ExcludeSellers
            End If
            Call Me.ClearSellList
            Call Me.Classify
            Call Me.LoadGrid(mdlConstGlob.eBasketMode.Remote)
            Me.cmdCalc.Enabled = True
        End If
    End Sub
    #End Region
    #Region "Evènements"
    Private Sub CellValidated(sender As Object, e As CellEventArgs)
    Dim VpQ As Short
        With mdlConstGlob.VgSessionSettings
            If Not .GridClearing Then
                 .GridClearing = True
                VmToBuy.Clear
                'Mise à jour de la quantité dans la grille et dans le List(Of )
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
            Call mdlToolbox.ShowInformation("Choisissez des cartes à acheter via clic droit dans l'explorateur !")
            Exit Sub
        ElseIf VmToSell.Count > 0 Then
            If mdlToolbox.ShowQuestion("Des offres pour les cartes demandées sont déjà disponibles..." + vbCrLf + "Voulez-vous à nouveau télécharger ces informations depuis " + VmServer.ToString + " ?") = DialogResult.No Then
                Exit Sub
            End If
        End If
        'Blocage d'actions indésirables
        Me.cmdRefresh.Visible = False
        Me.cmdCancel.Tag = False
        Me.cmdCancel.Visible = True
        Me.prgRefresh.Maximum = VmToBuy.Count
        Me.prgRefresh.Value = 0
        'Qualités autorisées pour les cartes à acheter
        If Me.chkMint.Checked Then VpQualities.Add(mdlConstGlob.eQuality.Mint)
        If Me.chkNearMint.Checked Then VpQualities.Add(mdlConstGlob.eQuality.NearMint)
        If Me.chkExcellent.Checked Then VpQualities.Add(mdlConstGlob.eQuality.Excellent)
        If Me.chkGood.Checked Then VpQualities.Add(mdlConstGlob.eQuality.Good)
        'Vendeurs exclus
        If Me.chkSeller.Checked Then
            VpBannedSellers = VgOptions.VgSettings.BannedSellers.ToLower.Split("#")
        End If
        'Efface les résultats précédents
        VmToSell.Clear
        Try
            'Récupère les informations pour les frais de port
            Call mdlToolbox.DownloadNow(New Uri(mdlConstGlob.VgOptions.VgSettings.DownloadServer + CgURL27), mdlConstGlob.CgMdShippingCosts)
            'Récupère les informations pour chaque carte
            For Each VpCard As clsLocalCard In VmToBuy
                Application.DoEvents
                'Vérifie qu'on n'a pas demandé à annuler la recherche
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
            'Finalisation et affichage des résultats dans la grid
            Me.prgRefresh.Value = 0
            Application.DoEvents
            Call Me.ClearSellList
            Call Me.Classify
            Call Me.LoadGrid(mdlConstGlob.eBasketMode.Remote)
        Catch
            Call mdlToolbox.ShowWarning(mdlConstGlob.CgDL3b)
        End Try
        'Restauration des actions bloquées
        Me.cmdRefresh.Visible = True
        Me.cmdCancel.Visible = False
        Me.cmdCalc.Enabled = True
    End Sub
    Sub CmdCalcClick(ByVal sender As Object, ByVal e As EventArgs)
    Dim VpStr As String
    Dim VpCountry As String
    Dim VpInfos() As String
    Dim VpShippingData As StreamReader
    Dim VpShippingPolicy As List(Of clsShipping)
    Dim VpShippingPolicies As New Dictionary(Of String, List(Of clsShipping))
    Dim VpMinParcelValue As Single = 0
    Dim VpPath As String = mdlToolbox.GetFreeTempFile(".txt")
        Me.prgRefresh.Style = ProgressBarStyle.Marquee
        Cursor.Current = Cursors.WaitCursor
        'On stocke les frais de port en mémoire pour un accès le plus rapide possible
        If File.Exists(Application.StartupPath + mdlConstGlob.CgMdShippingCosts) Then
            VpShippingData = New StreamReader(Application.StartupPath + mdlConstGlob.CgMdShippingCosts)
            While Not VpShippingData.EndOfStream
                VpStr = VpShippingData.ReadLine
                If Not VpStr.StartsWith("#") Then
                    VpInfos = VpStr.Split(";")
                    If VpInfos.Length = 5 Then
                        VpCountry = VpInfos(1)
                        If Not VpShippingPolicies.ContainsKey(VpCountry) Then
                            VpShippingPolicy = New List(Of clsShipping)
                            VpShippingPolicies.Add(VpCountry, VpShippingPolicy)
                        Else
                            VpShippingPolicy = VpShippingPolicies.Item(VpCountry)
                        End If
                        VpShippingPolicy.Add(New clsShipping(VpInfos(0), VpCountry, Val(VpInfos(2).Replace(",", ".")), CInt(Val(VpInfos(3).Replace(",", "."))), Val(VpInfos(4).Replace(",", "."))))
                    End If
                End If
            End While
            VpShippingData.Close
            'Essaie de ne pas effectuer de transaction dont le montant des produits serait strictement inférieur à la valeur saisie par l'utilisateur (optionnel)
            If Me.chkTransactions.Checked Then
                VpMinParcelValue = Val(Me.txtTransaction.Text)
            End If
            'Calcul effectif des transactions
            Call Me.CalcTransactions(VpShippingPolicies, VpMinParcelValue, VpPath)
            If File.Exists(VpPath) Then
                Process.Start(VpPath)
            End If
        Else
            Call mdlToolbox.ShowWarning("Les informations sur les frais de port sont indisponibles...")
        End If
        Me.prgRefresh.Style = ProgressBarStyle.Blocks
        Me.prgRefresh.Value = 0
    End Sub
    Sub FrmBuyCardsLoad(ByVal sender As Object, ByVal e As EventArgs)
        'Astuce horrible pour contourner un bug de mise à l'échelle automatique en fonction de la densité de pixels
        If Me.CreateGraphics().DpiX <> 96 Then
            Me.splitH.SplitterDistance = 140 * Me.CreateGraphics().DpiX / 96
        End If
        'Fixe la position du séparateur
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
    Sub ChkTransactionsCheckedChanged(sender As Object, e As EventArgs)
        Me.txtTransaction.Enabled = Me.chkTransactions.Checked
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
    Dim VpStr As String = InputBox("Quel est le nom du vendeur à exclure ?", "Ajout d'un vendeur dans la liste noire", "(Vendeur)")
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
            Cursor.Current = Cursors.WaitCursor
            Application.UseWaitCursor = True
            Application.DoEvents
            Call Me.BasketLoad(Me.dlgOpen.FileName)
            Call mdlToolbox.DownloadNow(New Uri(mdlConstGlob.VgOptions.VgSettings.DownloadServer + CgURL27), mdlConstGlob.CgMdShippingCosts)
            Application.UseWaitCursor = False
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
    #Region "Propriétés"
    Public WriteOnly Property Editions As List(Of String)
        Set (VpEditions As List(Of String))
            VmEditions = VpEditions
        End Set
    End Property
    #End Region
End Class
