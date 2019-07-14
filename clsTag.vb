Public Class clsTag
    Private VmKey As String = ""            'Champ référent en base de données
    Private VmValue As String = ""          'Valeur de ce champ
    Private VmValue2 As String = ""         'Titre VF
    Private VmValue3 As Boolean = False     'Double carte
    Private VmMultiverseId As Long = 0      'Identifiant universel
    Private VmDescendance As String = ""    'Requête SQL permettant de générer la descendance du noeud courant
    Public Sub New
    End Sub
    Public Sub New(VpValue As String)
        Value = VpValue
    End Sub
    Public Property Key As String
        Get
            Return VmKey
        End Get
        Set (VpKey As String)
            VmKey = VpKey
        End Set
    End Property
    Public Property Value As String
        Get
            Return VmValue
        End Get
        Set (VpValue As String)
            VmValue = VpValue
        End Set
    End Property
    Public Property Value2 As String
        Get
            Return VmValue2
        End Get
        Set (VpValue2 As String)
            VmValue2 = VpValue2
        End Set
    End Property
    Public Property Value3 As Boolean
        Get
            Return VmValue3
        End Get
        Set (VpValue3 As Boolean)
            VmValue3 = VpValue3
        End Set
    End Property
    Public Property MultiverseId As Long
        Get
            Return VmMultiverseId
        End Get
        Set (VpMultiverseId As Long)
            VmMultiverseId = VpMultiverseId
        End Set
    End Property
    Public Property Descendance As String
        Get
            Return VmDescendance
        End Get
        Set (VpDescendance As String)
            VmDescendance = VpDescendance
        End Set
    End Property
End Class
