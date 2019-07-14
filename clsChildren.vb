Public Class clsChildren
    Private VmDeleteEdition As frmDeleteEdition = Nothing
    Private VmGestDecks As frmGestDecks = Nothing
    Private VmGestAdv As frmGestAdv = Nothing
    Private VmCardsBuyer As frmBuyCards = Nothing
    Private VmSearcher As frmSearch = Nothing
    Private VmPerfs As frmPerfs = Nothing
    Private VmUpdateContenu As frmUpdateContenu = Nothing
    Private VmPricesHistory As frmGrapher = Nothing
    Private VmImportExport As frmExport = Nothing
    Public Function DoesntExist(VpForm As Form) As Boolean
        Return VpForm Is Nothing OrElse VpForm.IsDisposed
    End Function
    Public Property EditionDeleter As frmDeleteEdition
        Get
            Return VmDeleteEdition
        End Get
        Set (VpDeleteEdition As frmDeleteEdition)
            VmDeleteEdition = VpDeleteEdition
        End Set
    End Property
    Public Property DecksManager As frmGestDecks
        Get
            Return VmGestDecks
        End Get
        Set (VpGestDecks As frmGestDecks)
            VmGestDecks = VpGestDecks
        End Set
    End Property
    Public Property AdversairesManager As frmGestAdv
        Get
            Return VmGestAdv
        End Get
        Set (VpGestAdv As frmGestAdv)
            VmGestAdv = VpGestAdv
        End Set
    End Property
    Public Property CardsBuyer As frmBuyCards
        Get
            Return VmCardsBuyer
        End Get
        Set (VpCardsBuyer As frmBuyCards)
            VmCardsBuyer = VpCardsBuyer
        End Set
    End Property
    Public Property Searcher As frmSearch
        Get
            Return VmSearcher
        End Get
        Set (VpSearcher As frmSearch)
            VmSearcher = VpSearcher
        End Set
    End Property
    Public Property PerfsCounter As frmPerfs
        Get
            Return VmPerfs
        End Get
        Set (VpPerfs As frmPerfs)
            VmPerfs = VpPerfs
        End Set
    End Property
    Public Property ContenuUpdater As frmUpdateContenu
        Get
            Return VmUpdateContenu
        End Get
        Set (VpUpdateContenu As frmUpdateContenu)
            VmUpdateContenu = VpUpdateContenu
        End Set
    End Property
    Public Property PricesHistory As frmGrapher
        Get
            Return VmPricesHistory
        End Get
        Set (VpPricesHistory As frmGrapher)
            VmPricesHistory = VpPricesHistory
        End Set
    End Property
    Public Property ImporterExporter As frmExport
        Get
            Return VmImportExport
        End Get
        Set (VpImportExport As frmExport)
            VmImportExport = VpImportExport
        End Set
    End Property
End Class
