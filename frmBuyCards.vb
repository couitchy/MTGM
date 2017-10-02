'------------------------------------------------------
'| Projet         |  Magic The Gathering Manager      |
'| Contexte       |  		Perso                     |
'| Date           |                        30/03/2008 |
'| Release 1      |                        12/04/2008 |
'| Release 2      |                        30/08/2008 |
'| Release 3      |                        08/11/2008 |
'| Release 4      |                        29/08/2009 |
'| Release 5      |                        21/03/2010 |
'| Release 6      |                        17/04/2010 |
'| Release 7      |                        29/07/2010 |
'| Release 8      |                        03/10/2010 |
'| Release 9      |                        05/02/2011 |
'| Release 10     |                        10/09/2011 |
'| Release 11     |                        24/01/2012 |
'| Release 12     |                        01/10/2012 |
'| Release 13     |                        09/05/2014 |
'| Release 14     |                        09/05/2015 |
'| Release 15     |                        15/01/2017 |
'| Auteur         |                          Couitchy |
'|----------------------------------------------------|
'| Modifications :                                    |
'| - watchdog sur la connexion Magic-Ville 20/02/2010 |
'| - gestion des requêtes MagicCardMarket  15/01/2017 |
'| - optimisation des achats (heuristique) 23/07/2017 |
'| - restauration des requêtes Magic-Ville 29/07/2017 |
'------------------------------------------------------
#Region "Importations"
Imports SourceGrid2
Imports Cells = SourceGrid2.Cells.Real
Imports System.IO
Imports System.Text
Imports System.Security.Cryptography
Imports System.Net
Imports System.Web.Script.Serialization
Imports System.Xml
Imports System.Xml.Serialization
#End Region
Public Partial Class frmBuyCards
	#Region "Déclarations"	
	Private VmServer As clsModule.eMarketServer			'Market place choisie par l'utilisateur pour effectuer ses transactions
	Private VmToBuy As New List(Of clsLocalCard)		'Collection des cartes souhaitées à l'achat
	Private VmToSell As New List(Of clsRemoteCard)		'Collection des cartes disponibles à la vente
	Private VmEditions As List(Of String)				'Liste des éditions disponibles pour une même carte dans le cas où il y en a trop et qu'il faut potentiellement restreindre le volume de recherches
	Private VmSplitterDistance As Integer				'Position du séparateur
	Private VmBrowser As New WebBrowser					'Navigateur web
	Private VmIsComplete As Boolean = False				'Page complètement affichée dans le navigateur
	#End Region
	#Region "Méthodes"
	Public Sub New(VpServer As clsModule.eMarketServer)
		Me.InitializeComponent()
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
			If Now.Subtract(VpStart).TotalSeconds > clsModule.CgTimeOut Then
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
	Private Sub MVFetch(VpCard As String, VpQualities() As clsModule.eQuality, VpBannedSellers() As String)
	'--------------------------------------------------------------------------------------------------------------
	'Se connecte sur Magic-Ville pour récupérer les informations de ventes relatives à la carte passée en paramètre
	'--------------------------------------------------------------------------------------------------------------
	Dim VpElement As HtmlElement
	Dim VpLastId As Integer = 0
	Dim VpCurId As Integer
	Dim VpRemoteCard As clsRemoteCard = Nothing
	Dim VpToRemove As New List(Of clsRemoteCard)
		'Connexion au site de Magic-Ville
		Call Me.BrowseAndWait(clsModule.CgURL26)
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
						.Language = clsModule.MyLanguage(VpElement.GetAttribute("value"))
					ElseIf VpElement.Name.Contains("etat") Then
						.Etat = VpElement.GetAttribute("value")
					ElseIf VpElement.Name.Contains("seller") Then
						.Vendeur = New clsSeller(VpElement.GetAttribute("value"))
					End If
				End With
			ElseIf VpElement.GetAttribute("color") = "#3333ff" Then
				VpRemoteCard.Prix = clsModule.MyVal(VpElement.InnerText)
			End If
		Next VpElement
		'Supprime de la collection tout ce qui ne respecte pas les critères actifs
		For Each VpRemoteCard In VmToSell
			With VpRemoteCard
				If Not ( Array.IndexOf(clsModule.CgBuyLanguage, .Language.ToLower) >= 0 AndAlso Array.IndexOf(VpQualities, .Etat) >= 0 AndAlso (VpBannedSellers Is Nothing OrElse VpBannedSellers.Length = 0 OrElse Array.IndexOf(VpBannedSellers, .Vendeur.Name.ToLower) < 0) ) Then
					VpToRemove.Add(VpRemoteCard)
				End If
			End With
		Next VpRemoteCard
		For Each VpDelete As clsRemoteCard In VpToRemove
			VmToSell.Remove(VpDelete)
		Next VpDelete
	End Sub
	Private Sub MKMFetch(VpCard As String, VpQualities() As clsModule.eQuality, VpBannedSellers() As String)
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
		If VpProducts.product.Count > clsModule.CgMaxEditionsMarket Then
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
						If Array.IndexOf(clsModule.CgBuyLanguage, .language.languageName.ToLower) >= 0 AndAlso Array.IndexOf(VpQualities, clsModule.MyQuality(.condition)) >= 0 AndAlso (VpBannedSellers Is Nothing OrElse VpBannedSellers.Length = 0 OrElse Array.IndexOf(VpBannedSellers, .seller.username.ToLower) < 0) Then
							VmToSell.Add(New clsRemoteCard(VpCard, New clsSeller(.seller.username), VpProduct.expansion, .language.languageName, clsModule.MyQuality(.condition), .count, 0, .price))
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
		VpURL = VpURL.Replace("'", "%27")	'cas non gérés par EscapeUriString
		VpSerializer.MaxJsonLength = Integer.MaxValue
		VpRequest = HttpWebRequest.Create(VpURL)
		VpRequest.Method = "GET"
		VpRequest.Headers.Clear
		VpRequest.Headers.Add(HttpRequestHeader.Authorization, (New OAuthHeader).GetAuthorizationHeader(VpRequest.Method, VpURL))
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
					VpPrice = VpRemoteCard.Prix
					VpExtracted.Add(VpRemoteCard)
				ElseIf Math.Abs(VpRemoteCard.Prix - VpPrice) <= 0.1 Then
					VpExtracted.Add(VpRemoteCard)
				End If
			End If
		Next VpRemoteCard
		Return VpExtracted
	End Function
	Private Sub CalcTransactions(VpPath As String)
	'------------------------------------------------------------------------------------------------------------------------------
	'Détermine les transactions nécessaires pour acheter toutes les cartes désirées de la bonne manière (heuristique sous-optimale)
	'------------------------------------------------------------------------------------------------------------------------------
	Dim VpOutput As New StreamWriter(VpPath)										'Fichier contenant le résultat de l'optimisation
	Dim VpToBuy As List(Of clsLocalCard) = clsLocalCard.GetClone(VmToBuy)			'Liste des cartes demandées à l'achat
	Dim VpToSell As List(Of clsRemoteCard) = clsRemoteCard.GetClone(VmToSell)		'Liste des cartes disponibles à la vente
	Dim VpSellers As New List(Of clsSeller)											'Liste des vendeurs sollicités
	Dim VpBackups As New List(Of clsBackupTransaction)								'Liste des modifications des transactions effectuées par rapport à la sélection initiale
	Dim VpGroup As List(Of clsRemoteCard)											'Ensemble au même prix pour la carte courante
	Dim VpFound As clsRemoteCard													'Carte envisagée à l'achat en respect des critères
	Dim VpNewDeal As Boolean														'Au moins une modification de transaction a eu lieu sur la sous-itération courante
	Dim VpN As Integer																'Nombre de cartes dans la transaction courante
	Dim VpV, VpV1 As Integer														'Nombre de vendeurs sollicités pour l'ensemble des transactions
		'Calcule la disponibilité des cartes et la couverture des vendeurs
		For Each VpLocalCard As clsLocalCard In VpToBuy
			For Each VpRemoteCard As clsRemoteCard In VpToSell
				If VpLocalCard.Name = VpRemoteCard.Name Then
					VpLocalCard.Dispo += VpRemoteCard.Quantity
					VpN = Math.Min(VpLocalCard.Quantity, VpRemoteCard.Quantity)
					VpRemoteCard.Vendeur.Coverage += VpN
				End If
			Next VpRemoteCard
		Next VpLocalCard
		'Trie les cartes à acheter de la moins disponible (va impacter le choix du vendeur le plus nécessaire) à la plus disponible
		VpToBuy.Sort(New clsLocalCardComparer)
		'Trie les cartes à vendre de la moins chère à la plus chère
		VpToSell.Sort(New clsRemoteCardComparer)
		'Parcourt la liste des cartes à acheter
		For Each VpLocalCard As clsLocalCard In VpToBuy
			While VpLocalCard.Quantity > 0
				'Cherche le groupe de cartes à vendre correspondant à la carte courante désirée pour le moins cher possible
				VpGroup = Me.Extract(VpLocalCard.Name, VpToSell)
				'Avertissement si aucun exemplaire de cette carte n'est disponible à la vente
				If VpGroup.Count = 0 Then
					Call clsModule.ShowWarning("Impossible d'acquérir la carte " + VpLocalCard.Name + "...")
					Exit While
				End If
				VpFound = Nothing
				'Privilégie un vendeur déjà utilisé
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
		'Si on cherche à minimiser le nombre de transactions
		If Me.chkTransactions.Checked Then
			'Ajoute tous les vendeurs, même ceux pas encore sollicités
			For Each VpRemoteCard As clsRemoteCard In VpToSell
				If Not VpSellers.Contains(VpRemoteCard.Vendeur) Then
					VpSellers.Add(VpRemoteCard.Vendeur)
				End If
			Next VpRemoteCard
			'Trie les vendeurs par couverture, de la plus petite à la plus grande
			VpSellers.Sort(New clsSellerComparer)
			VpV = clsSeller.GetCount(VpSellers)
			'Optimisation itérative par réduction du nombre de vendeurs à solliciter
			While True
				'Parcourt chaque vendeur (en commençant par ceux dont on souhaite se passer)
				For Each VpSeller1 As clsSeller In VpSellers
					VpBackups.Clear
					VpNewDeal = True
					'Tant qu'on arrive à réduire la sollicitation de ce vendeur
					While VpNewDeal
						VpNewDeal = False
						'Parcourt les cartes qu'on lui a acheté
						For Each VpCard1 As clsRemoteCard In VpSeller1.GetCardsOfInterest(VpToSell, True)
							VpFound = New clsRemoteCard(VpCard1.Name, Nothing, VpCard1.Edition, VpCard1.Language, VpCard1.Etat, VpCard1.Quantity, VpCard1.Bought, Single.PositiveInfinity)
							'Parcourt les vendeurs susceptibles de proposer davantage de cartes que lui
							For Each VpSeller2 As clsSeller In VpSellers
								If VpSeller1 IsNot VpSeller2 AndAlso VpSeller2.Coverage > VpSeller1.Coverage Then
									'Recherche à qui il vaudrait mieux confier la transaction initiale pour minimiser le surcoût en se passant du vendeur dont on ne veut plus (dans tous les cas, le surcoût doit être inférieur à la valeur maximale estimée pour les frais de port d'une carte)
									For Each VpCard2 As clsRemoteCard In VpSeller2.GetCardsOfInterest(VpToSell, False)
										If VpCard2.Name = VpFound.Name AndAlso VpCard2.Quantity > 0 AndAlso VpCard2.Prix <= VpFound.Prix AndAlso VpCard2.Prix - VpCard1.Prix < clsModule.CgWorstShippingCost Then
											VpFound = VpCard2
										End If
									Next VpCard2
								End If
							Next VpSeller2
							'Effectue la modification de la transaction si on a trouvé un nouveau vendeur
							If VpFound.Vendeur IsNot Nothing Then
								VpN = Math.Min(VpCard1.Bought, VpFound.Quantity)
								'Restitution au vendeur initial
								VpCard1.Quantity += VpN
								VpCard1.Bought -= VpN
								VpCard1.Vendeur.Bought -= VpN
								'Acquisition auprès du nouveau vendeur
								VpFound.Quantity -= VpN
								VpFound.Bought += VpN
								VpFound.Vendeur.Bought += VpN
								'Sauvegarde en cas d'annulation
								VpBackups.Add(New clsBackupTransaction(VpCard1, VpFound, VpN))
								VpNewDeal = True
							End If
						Next VpCard1
						Application.DoEvents
					End While
					'Si on n'a pas réussi à transférer l'intégralité des transactions avec ce vendeur et qu'on est donc obligé de le garder, on restaure toutes les transactions dans leur état initial puisqu'il était moins cher que les autres
					If VpSeller1.Bought > 0 Then
						For Each VpBackup As clsBackupTransaction In VpBackups
							With VpBackup
								'Réacquisition auprès du vendeur initial
								.Before.Quantity -= .N
								.Before.Bought += .N
								.Before.Vendeur.Bought += .N
								'Restitution à l'ex-nouveau vendeur
								.After.Quantity += .N
								.After.Bought -= .N
								.After.Vendeur.Bought -= .N
							End With
						Next VpBackup
					End If
					'Si on a déjà atteint le nombre de transactions souhaité, pas la peine de continuer
					If clsSeller.GetCount(VpSellers) <= Me.sldTransactions.Value Then
						Exit For
					End If
				Next VpSeller1
				'On arrête d'optimiser dès qu'on atteint le nombre de transactions souhaité ou qu'on ne peut plus le diminuer par rapport à l'itération précédente
				VpV1 = clsSeller.GetCount(VpSellers)
				If VpV1 <= Me.sldTransactions.Value Or Not VpV1 < VpV Then
					Exit While
				End If
				VpV = VpV1
			End While
			'Avertissement si le nombre maximal de transactions autorisées est dépassé
			If clsSeller.GetCount(VpSellers) > Me.sldTransactions.Value Then
				Call clsModule.ShowWarning("Impossible d'effectuer tous les achats avec seulement " + Me.sldTransactions.Value.ToString + " transaction(s)...")
			End If
		End If
		'Récapitulatif des transactions par vendeur
		VpOutput.WriteLine("------------------------------------")
		VpOutput.WriteLine("Total (hors frais de port) : " + clsRemoteCard.GetTotal(VpToSell).ToString + "€")
		VpOutput.WriteLine("Nombre de vendeurs : " + clsSeller.GetCount(VpSellers).ToString)
		VpOutput.WriteLine("------------------------------------")
		For Each VpSeller As clsSeller In VpSellers
			If VpSeller.Bought > 0 Then
				VpOutput.WriteLine(VpSeller.Name + " [" + VpSeller.Bought.ToString + " cartes pour " + clsRemoteCard.GetSubTotal(VpToSell, VpSeller).ToString + "€]")
				For Each VpRemoteCard As clsRemoteCard In VpSeller.GetCardsOfInterest(VpToSell, True)
					VpOutput.WriteLine(vbTab + VpRemoteCard.Name + " - " + VpRemoteCard.Edition + " - " + VpRemoteCard.Bought.ToString + "x " + VpRemoteCard.Prix.ToString + "€")
				Next VpRemoteCard
			End If
		Next VpSeller
		VpOutput.Flush
		VpOutput.Close
		Me.txtTot.Text = clsRemoteCard.GetTotal(VpToSell).ToString + "€"
		Call clsModule.ShowInformation("Optimisation terminée !" + vbCrLf + "Les résultats vont s'afficher automatiquement dans le bloc-notes...")
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
	Public Sub LoadGrid(VpMode As clsModule.eBasketMode)
	Dim VpCellModel As DataModels.IDataModel
		Me.grdBasket.SuspendLayout
		'Mode 1 : Résultats de la recherche sur Internet
		If VpMode = clsModule.eBasketMode.Remote Then
			Application.UseWaitCursor = True
			'Préparation de la grille
			With Me.grdBasket
				Me.prgRefresh.Value = 0
				Me.prgRefresh.Maximum = VmToSell.Count
				'Nettoyage
				If .Rows.Count > 0 Then
					.Rows.RemoveRange(0, .Rows.Count)
				End If
				'Nombre de colonnes et d'en-têtes
				.ColumnsCount = 7
				.FixedRows = 1
				.Rows.Insert(0)
				Me.grdBasket(0, 0) = New Cells.ColumnHeader("Nom de la carte")
				Me.grdBasket(0, 1) = New Cells.ColumnHeader("Vendeur")
				Me.grdBasket(0, 2) = New Cells.ColumnHeader("Edition")
				Me.grdBasket(0, 3) = New Cells.ColumnHeader("Langue")
				Me.grdBasket(0, 4) = New Cells.ColumnHeader("Etat")
				Me.grdBasket(0, 5) = New Cells.ColumnHeader("Quantité")
				Me.grdBasket(0, 6) = New Cells.ColumnHeader("Prix unitaire")
				CType(Me.grdBasket(0, 5), Cells.ColumnHeader).Comparer = New clsNumericComparer
				CType(Me.grdBasket(0, 6), Cells.ColumnHeader).Comparer = New clsNumericComparer
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
					Me.grdBasket(VpI + 1, 6) = New Cells.Cell(VmToSell.Item(VpI).Prix.ToString + " €")
					Me.prgRefresh.Increment(1)
				Next VpI
				.AutoSize
			End With
			Me.prgRefresh.Value = 0
			Application.UseWaitCursor = False
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
		Me.grdBasket.ResumeLayout
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
			Call Me.LoadGrid(clsModule.eBasketMode.Remote)
			Me.cmdCalc.Enabled = True
		End If
	End Sub
	#End Region
	#Region "Evènements"
	Private Sub CellValidated(sender As Object, e As CellEventArgs)
	Dim VpQ As Short
		With clsModule.VgSessionSettings
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
				Call Me.LoadGrid(clsModule.eBasketMode.Local)
				 .GridClearing = False
			End If
		End With
	End Sub
	Sub BtLocalBasketActivate(ByVal sender As Object, ByVal e As EventArgs)
		Call Me.LoadGrid(clsModule.eBasketMode.Local)
	End Sub
	Sub BtRemoteBasketActivate(ByVal sender As Object, ByVal e As EventArgs)
		Call Me.LoadGrid(clsModule.eBasketMode.Remote)
	End Sub
	Sub CmdRefreshClick(ByVal sender As Object, ByVal e As EventArgs)
	Dim VpQualities As New List(Of clsModule.eQuality)
	Dim VpBannedSellers() As String = Nothing
		'Information utilisateur
		If VmToBuy.Count = 0 Then
			Call clsModule.ShowInformation("Choisissez des cartes à acheter via clic droit dans l'explorateur !")
			Exit Sub
		End If
		'Blocage d'actions indésirables
		Me.cmdRefresh.Visible = False
		Me.cmdCancel.Tag = False
		Me.cmdCancel.Visible = True
		Me.prgRefresh.Maximum = VmToBuy.Count
		Me.prgRefresh.Value = 0
		'Qualités autorisées pour les cartes à acheter
		If Me.chkMint.Checked Then VpQualities.Add(clsModule.eQuality.Mint)
		If Me.chkNearMint.Checked Then VpQualities.Add(clsModule.eQuality.NearMint)
		If Me.chkExcellent.Checked Then VpQualities.Add(clsModule.eQuality.Excellent)
		If Me.chkGood.Checked Then VpQualities.Add(clsModule.eQuality.Good)
		'Vendeurs exclus
		If Me.chkSeller.Checked Then
			VpBannedSellers = VgOptions.VgSettings.BannedSellers.ToLower.Split("#")
		End If
		'Efface les résultats précédents
		VmToSell.Clear
		Try
			'Récupère les informations pour chaque carte
			For Each VpCard As clsLocalCard In VmToBuy
				Application.DoEvents
				'Vérifie qu'on n'a pas demandé à annuler la recherche
				If Me.cmdCancel.Tag = True Then
					Exit For
				End If
				'Choix de la market place
				Select Case VmServer
					Case clsModule.eMarketServer.MagicVille
						Call Me.MVFetch(VpCard.Name, VpQualities.ToArray, VpBannedSellers)
					Case clsModule.eMarketServer.MagicCardMarket
						Call Me.MKMFetch(VpCard.Name, VpQualities.ToArray, VpBannedSellers)
					Case Else
				End Select
				Me.prgRefresh.Increment(1)
			Next VpCard
			'Finalisation et affichage des résultats dans la grid
			Me.prgRefresh.Value = 0
			Application.DoEvents
			Call Me.ClearSellList
			Call Me.LoadGrid(clsModule.eBasketMode.Remote)
		Catch
			Call clsModule.ShowWarning(clsModule.CgDL3b)
		End Try
		'Restauration des actions bloquées
		Me.cmdRefresh.Visible = True
		Me.cmdCancel.Visible = False
		Me.cmdCalc.Enabled = True
	End Sub
	Sub CmdCalcClick(ByVal sender As Object, ByVal e As EventArgs)
	Dim VpPath As String = clsModule.GetFreeTempFile(".txt")
		Me.prgRefresh.Style = ProgressBarStyle.Marquee
		Cursor.Current = Cursors.WaitCursor
		Call Me.CalcTransactions(VpPath)
		If File.Exists(VpPath) Then
			Process.Start(VpPath)
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
	Sub ChkSellerCheckedChanged(sender As Object, e As EventArgs)
		If Me.chkSeller.Checked Then
			If VmToSell.Count > 0 Then
				Application.DoEvents
				Call Me.ExcludeSellers
				Cursor.Current = Cursors.WaitCursor
				Call Me.LoadGrid(clsModule.eBasketMode.Remote)
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
				Call Me.LoadGrid(clsModule.eBasketMode.Remote)
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
	#Region "Propriétés"
	Public WriteOnly Property Editions As List(Of String)
		Set (VpEditions As List(Of String))
			VmEditions = VpEditions
		End Set
	End Property
	#End Region
End Class
Public Class clsCapsuleCards
	Private VmToBuy As List(Of clsLocalCard)
	Private VmToSell As List(Of clsRemoteCard)
	Public Property ToBuy As List(Of clsLocalCard)
		Get
			Return VmToBuy
		End Get
		Set (VpToBuy As List(Of clsLocalCard))
			VmToBuy = VpToBuy
		End Set
	End Property
	Public Property ToSell As List(Of clsRemoteCard)
		Get
			Return VmToSell
		End Get
		Set (VpToSell As List(Of clsRemoteCard))
			VmToSell = VpToSell
		End Set
	End Property
End Class
Public Class clsSeller
	Private VmName As String
	Private VmCoverage As Integer
	Private VmBought As Integer
	Public Sub New(VpName As String, VpCoverage As Integer, VpBought As Integer)
		VmName = VpName
		VmCoverage = VpCoverage
		VmBought = VpBought
	End Sub
	Public Sub New(VpName As String)
		VmName = VpName
		VmCoverage = 0
	End Sub
	Public Sub New
		VmName = ""
		VmCoverage = 0
	End Sub
	Public Function GetBoughtCards(VpToSell As List(Of clsRemoteCard)) As List(Of clsRemoteCard)
	'--------------------------------------------------
	'Retourne la liste des cartes achetées à ce vendeur
	'--------------------------------------------------
	Dim VpBought As New List(Of clsRemoteCard)
		For Each VpRemoteCard As clsRemoteCard In VpToSell
			If VpRemoteCard.Vendeur Is Me Then
				VpBought.Add(VpRemoteCard)
			End If
		Next VpRemoteCard
		Return VpBought
	End Function
	Public Function GetCardsOfInterest(VpToSell As List(Of clsRemoteCard), VpBought As Boolean) As List(Of clsRemoteCard)
	'-------------------------------------------------------------
	'Retourne la liste des cartes achetables/achetées à ce vendeur
	'-------------------------------------------------------------
	Dim VpCards As New List(Of clsRemoteCard)
		For Each VpRemoteCard As clsRemoteCard In VpToSell
			If VpRemoteCard.Vendeur Is Me And (VpRemoteCard.Bought > 0 Or Not VpBought) Then
				VpCards.Add(VpRemoteCard)
			End If
		Next VpRemoteCard
		Return VpCards
	End Function
	Public Shared Function AddOrGet(VpName As String, VpCoverage As Integer, VpBought As Integer, VpSellers As List(Of clsSeller)) As clsSeller
	'--------------------------------------------------------------------------------------------------------------------------------
	'Crée un nouveau vendeur dont le nom est passé en paramètre et retourne sa référence, ou, s'il existe déjà, retourne sa référence
	'--------------------------------------------------------------------------------------------------------------------------------
	Dim VpSeller As clsSeller
		For Each VpSeller In VpSellers
			If VpSeller.Name = VpName Then
				Return VpSeller
			End If
		Next VpSeller
		VpSeller = New clsSeller(VpName, VpCoverage, VpBought)
		VpSellers.Add(VpSeller)
		Return VpSeller
	End Function
	Public Shared Function GetCount(VpSellers As List(Of clsSeller)) As Integer
	'-------------------------------------------------------
	'Retourne le nombre de vendeurs effectivement sollicités
	'-------------------------------------------------------
	Dim VpCount As Integer = 0
		For Each VpSeller As clsSeller In VpSellers
			'If Not VpSeller.Canceled Then
			If VpSeller.Bought > 0 Then
				VpCount += 1
			End If
		Next VpSeller
		Return VpCount
	End Function
	Public Property Name As String
		Get
			Return VmName
		End Get
		Set (VpName As String)
			VmName = VpName
		End Set
	End Property
	Public Property Coverage As Integer
		Get
			Return VmCoverage
		End Get
		Set (VpCoverage As Integer)
			VmCoverage = VpCoverage
		End Set
	End Property
	Public Property Bought As Integer
		Get
			Return VmBought
		End Get
		Set (VpBought As Integer)
			VmBought = VpBought
		End Set
	End Property
End Class
Public Class clsLocalCard
	Private VmName As String
	Private VmQuant As Integer
	Private VmDispo As Integer
	Public Sub New(VpName As String, VpQuant As Integer, VpDispo As Integer)
		VmName = VpName
		VmQuant = VpQuant
		VmDispo = VpDispo
	End Sub
	Public Sub New
		VmName = ""
		VmQuant = 0
		VmDispo = 0
	End Sub
	Public Shared Function GetClone(VpA As List(Of clsLocalCard)) As List(Of clsLocalCard)
	'-------------------------------------------
	'Duplication de la liste des cartes désirées
	'-------------------------------------------
	Dim VpB As New List(Of clsLocalCard)
		For Each VpLocalCard As clsLocalCard In VpA
			With VpLocalCard
				If .Quantity > 0 Then
					VpB.Add(New clsLocalCard(.Name, .Quantity, .Dispo))
				End If
			End With
		Next VpLocalCard
		Return VpB
	End Function
	Public Property Name As String
		Get
			Return VmName
		End Get
		Set (VpName As String)
			VmName = VpName
		End Set
	End Property
	Public Property Quantity As Integer
		Get
			Return VmQuant
		End Get
		Set (VpQuant As Integer)
			VmQuant = VpQuant
		End Set
	End Property
	Public Property Dispo As Integer
		Get
			Return VmDispo
		End Get
		Set (VpDispo As Integer)
			VmDispo = VpDispo
		End Set
	End Property
End Class
Public Class clsRemoteCard
	Private VmName As String
	Private VmVendeur As clsSeller
	Private VmEdition As String
	Private VmLanguage As String
	Private VmEtat As clsModule.eQuality
	Private VmQuant As Integer
	Private VmBought As Integer
	Private VmPrix As Single
	Public Sub New(VpName As String, VpVendeur As clsSeller, VpEdition As String, VpLanguage As String, VpEtat As clsModule.eQuality, VpQuant As Integer, VpBought As Integer, VpPrix As Single)
		VmName = VpName
		VmVendeur = VpVendeur
		VmEdition = VpEdition
		VmLanguage = VpLanguage
		VmEtat = VpEtat
		VmQuant = VpQuant
		VmBought = VpBought
		VmPrix = VpPrix
	End Sub
	Public Sub New(VpName As String)
		Me.New(VpName, New clsSeller, "", "", clsModule.eQuality.Mint, 0, 0, 0)
	End Sub
	Public Sub New
		Me.New("", New clsSeller, "", "", clsModule.eQuality.Mint, 0, 0, 0)
	End Sub
	Public Shared Function GetClone(VpA As List(Of clsRemoteCard)) As List(Of clsRemoteCard)
	'-------------------------------------------
	'Duplication de la liste des cartes en vente
	'-------------------------------------------
	Dim VpB As New List(Of clsRemoteCard)
	Dim VpSellers As New List(Of clsSeller)
		For Each VpRemoteCard As clsRemoteCard In VpA
			With VpRemoteCard
				VpB.Add(New clsRemoteCard(.Name, clsSeller.AddOrGet(.Vendeur.Name, .Vendeur.Coverage, .Vendeur.Bought, VpSellers) , .Edition, .Language, .Etat, .Quantity, .Bought, .Prix))
			End With
		Next VpRemoteCard
		Return VpB
	End Function
	Public Shared Function GetTotal(VpToSell As List(Of clsRemoteCard)) As Single
	'---------------------------------------------------------------------------------------------
	'Retourne le total des coûts d'acquisition (hors frais de port) auprès des vendeurs sollicités 
	'---------------------------------------------------------------------------------------------
	Dim VpTotal As Single = 0
		For Each VpRemoteCard As clsRemoteCard In VpToSell
			VpTotal += VpRemoteCard.Bought * VpRemoteCard.Prix
		Next VpRemoteCard
		Return VpTotal
	End Function
	Public Shared Function GetSubTotal(VpToSell As List(Of clsRemoteCard), VpSeller As clsSeller) As Single
	'---------------------------------------------------------------------------------------------
	'Retourne le total des coûts d'acquisition (hors frais de port) auprès des vendeurs sollicités 
	'---------------------------------------------------------------------------------------------
	Dim VpTotal As Single = 0
		For Each VpRemoteCard As clsRemoteCard In VpToSell
			If VpRemoteCard.Vendeur Is VpSeller Then
				VpTotal += VpRemoteCard.Bought * VpRemoteCard.Prix
			End If
		Next VpRemoteCard
		Return VpTotal
	End Function
	Public Property Name As String
		Get
			Return VmName
		End Get
		Set (VpName As String)
			VmName = VpName
		End Set
	End Property
	Public Property Vendeur As clsSeller
		Get
			Return VmVendeur
		End Get
		Set (VpVendeur As clsSeller)
			VmVendeur = VpVendeur
		End Set
	End Property
	Public Property Edition As String
		Get
			Return VmEdition
		End Get
		Set (VpEdition As String)
			VmEdition = VpEdition
		End Set
	End Property
	Public Property Language As String
		Get
			Return VmLanguage
		End Get
		Set (VpLanguage As String)
			VmLanguage = VpLanguage
		End Set
	End Property
	Public Property Etat As clsModule.eQuality
		Get
			Return VmEtat
		End Get
		Set (VpEtat As clsModule.eQuality)
			VmEtat = VpEtat
		End Set
	End Property
	Public Property Quantity As Integer
		Get
			Return VmQuant
		End Get
		Set (VpQuant As Integer)
			VmQuant = VpQuant
		End Set
	End Property
	Public Property Bought As Integer
		Get
			Return VmBought
		End Get
		Set (VpBought As Integer)
			VmBought = VpBought
		End Set
	End Property
	Public Property Prix As Single
		Get
			Return VmPrix
		End Get
		Set (VpPrix As Single)
			VmPrix = VpPrix
		End Set
	End Property
End Class
Public Class clsBackupTransaction
	Private VmBefore As clsRemoteCard
	Private VmAfter As clsRemoteCard
	Private VmN As Integer
	Public Sub New(VpBefore As clsRemoteCard, VpAfter As clsRemoteCard, VpN As Integer)
		VmBefore = VpBefore
		VmAfter = VpAfter
		VmN = VpN
	End Sub
	Public Property Before As clsRemoteCard
		Get
			Return VmBefore
		End Get
		Set (VpBefore As clsRemoteCard)
			VmBefore = VpBefore
		End Set
	End Property
	Public Property After As clsRemoteCard
		Get
			Return VmAfter
		End Get
		Set (VpAfter As clsRemoteCard)
			VmAfter = VpAfter
		End Set
	End Property
	Public Property N As Integer
		Get
			Return VmN
		End Get
		Set (VpN As Integer)
			VmN = VpN
		End Set
	End Property
End Class
Public Class clsSellerComparer
	Implements IComparer(Of clsSeller)
	Public Function Compare(ByVal x As clsSeller, ByVal y As clsSeller) As Integer Implements IComparer(Of clsSeller).Compare
		Return x.Coverage.CompareTo(y.Coverage)
	End Function
End Class
Public Class clsLocalCardComparer
	Implements IComparer(Of clsLocalCard)
	Public Function Compare(ByVal x As clsLocalCard, ByVal y As clsLocalCard) As Integer Implements IComparer(Of clsLocalCard).Compare
		Return x.Dispo.CompareTo(y.Dispo)
	End Function
End Class
Public Class clsRemoteCardComparer
	Implements IComparer(Of clsRemoteCard)
	Public Function Compare(ByVal x As clsRemoteCard, ByVal y As clsRemoteCard) As Integer Implements IComparer(Of clsRemoteCard).Compare
		'Si 2 cartes sont au même prix, on favorise le vendeur qui a la plus grande couverture
		If Math.Abs(x.Prix - y.Prix) <= 0.1 Then
			Return y.Vendeur.Coverage.CompareTo(x.Vendeur.Coverage)
		Else
			Return x.Prix.CompareTo(y.Prix)
		End If
	End Function
End Class
Public Class clsNumericComparer
	Implements IComparer
	Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
	Dim VpX As Cells.Cell = x
	Dim VpY As Cells.Cell = y
		Return Val(VpX.Value.ToString.Replace(",", ".")).CompareTo(Val(VpY.Value.ToString.Replace(",", ".")))
	End Function
End Class
Public Class OAuthHeader
	Private VmAppToken As String = "NYbYnf8ZTmqIOofE"						'Clé publique
	Private VmAppSecret As String = "mviFO4ULHXSsPjMxV4KPqCiiP4pHv99r"		'Clé privée
	Private VmAccessToken As String = ""									'Non utilisé
	Private VmAccessSecret As String = ""									'Non utilisé
	Private VmSignatureMethod As String = "HMAC-SHA1"						'Méthode de hashage
	Private VmVersion As String = "1.0"										'Version OAuth
	Private VmHeaderParams As IDictionary(Of String, String)				'En-têtes pour la requête HTTP
	Public Sub New
	'------------------------------------------------
	'Construction d'une authentification OAuth unique
	'------------------------------------------------
	Dim VpNonce As String = Guid.NewGuid.ToString("n")
	Dim VpTimestamp As String = CInt((DateTime.UtcNow.Subtract(New DateTime(1970, 1, 1))).TotalSeconds).ToString
		VmHeaderParams = New Dictionary(Of String, String)
		VmHeaderParams.Add("oauth_consumer_key", VmAppToken)
		VmHeaderParams.Add("oauth_token", VmAccessToken)
		VmHeaderParams.Add("oauth_nonce", VpNonce)
		VmHeaderParams.Add("oauth_timestamp", VpTimestamp)
		VmHeaderParams.Add("oauth_signature_method", VmSignatureMethod)
		VmHeaderParams.Add("oauth_version", VmVersion)
	End Sub
	Public Function GetAuthorizationHeader(VpMethod As String, VpURL As String) As String
	'--------------------------
	'Calcul des en-têtes signés
	'--------------------------
	Dim VpBaseString As String
	Dim VpEncodedParams As New SortedDictionary(Of String, String)
	Dim VpParamStrings As New List(Of String)
	Dim VpParamString As String
	Dim VpSignatureKey As String
	Dim VpHasher As HMAC
	Dim VpOAuthSignature As String
	Dim VpHeaderParamStrings As New List(Of String)
		'Add the realm parameter to the header params
		VmHeaderParams.Add("realm", VpURL)
		'Start composing the base string from the method and request URI
		VpBaseString = VpMethod.ToUpper + "&" + Uri.EscapeDataString(VpURL) + "&"
		'Gather, encode, and sort the base string parameters
		For Each VpParameter As KeyValuePair(Of String, String) In VmHeaderParams
			If Not VpParameter.Key.Equals("realm") Then
				VpEncodedParams.Add(Uri.EscapeDataString(VpParameter.Key), Uri.EscapeDataString(VpParameter.Value))
			End If
		Next VpParameter
		'Expand the base string by the encoded parameter=value pairs
		For Each VpParameter As KeyValuePair(Of String, String) In VpEncodedParams
			VpParamStrings.Add(VpParameter.Key + "=" + VpParameter.Value)
		Next VpParameter
		VpParamString = Uri.EscapeDataString(String.Join("&", VpParamStrings.ToArray))
		VpBaseString += VpParamString
		'Create the OAuth signature
		VpSignatureKey = Uri.EscapeDataString(VmAppSecret) + "&" + Uri.EscapeDataString(VmAccessSecret)
		VpHasher = HMACSHA1.Create
		VpHasher.Key = Encoding.UTF8.GetBytes(VpSignatureKey)
		VpOAuthSignature = Convert.ToBase64String(VpHasher.ComputeHash(Encoding.UTF8.GetBytes(VpBaseString)))
		'Include the OAuth signature parameter in the header parameters array
		VmHeaderParams.Add("oauth_signature", VpOAuthSignature)
		'Construct the header string
		For Each VpParameter As KeyValuePair(Of String, String) In VmHeaderParams
			VpHeaderParamStrings.Add(VpParameter.Key + "=""" + VpParameter.Value + """")
		Next VpParameter
		Return "OAuth " + String.Join(", ", VpHeaderParamStrings.ToArray)
	End Function
End Class