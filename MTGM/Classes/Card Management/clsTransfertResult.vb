Public Class clsTransfertResult
    Public Enum EgTransfertType
        Deletion
        Move
        Copy
        Swap
    End Enum
    Public TransfertType As EgTransfertType = EgTransfertType.Move      'Type d'opération
    Public NCartes As Integer = 0                                       'Nombre de cartes concernées
    Public IDSerieFrom As String = ""                                   'Edition source
    Public IDSerieTo As String = ""                                     'Edition destination
    Public EncNbrFrom As Integer = 0                                    'Numéro encyclopédique source
    Public EncNbrTo As Integer = 0                                      'Numéro encyclopédique destination
    Public FoilFrom As Boolean = False                                  'Mention éventuelle foil source
    Public FoilTo As Boolean = False                                    'Mention éventuelle foil destination
    Public ReserveFrom As Boolean = False                               'Mention éventuelle réserve source
    Public ReserveTo As Boolean = False                                 'Mention éventuelle réserve destination
    Public TFrom As String = ""                                         'Deck source
    Public TTo As String = ""                                           'Deck destination
    Public SFrom As String = ""                                         'Nom de la table source
    Public STo As String = ""                                           'Nom de la table destination
End Class
