Imports System.IO
Public Class clsPlateauCard
    Private VmOwner As List(Of clsPlateauCard)
    Private VmCardName As String
    Private VmTransformedCardName As String
    Private VmCardNameFR As String
    Private VmCardType As String
    Private VmTapped As Boolean
    Private VmHidden As Boolean
    Private VmTransformable As Boolean
    Private VmTransformed As Boolean
    Private VmCounters As Integer
    Private VmAttachedTo As clsPlateauCard
    Private VmAttachments As New List(Of clsPlateauCard)
    Private VmMissingImg As Boolean
    Private VmReserve As Boolean
    Private VmReserveInPlay As Boolean
    Public Sub New(VpOwner As List(Of clsPlateauCard), VpName As String, VpNameFR As String, VpType As String, VpTransformable As Boolean, VpTransformedCardName As String, VpReserve As Boolean)
        VmCardName = VpName
        VmCardNameFR = VpNameFR
        VmCardType = VpType
        VmTransformable = VpTransformable
        VmTransformedCardName = VpTransformedCardName
        VmReserve = VpReserve
        Call Me.ReInit(VpOwner)
    End Sub
    Public Sub ReInit(VpOwner As List(Of clsPlateauCard))
        VmOwner = VpOwner
        VmTapped = False
        VmTransformed = False
        VmHidden = True
        VmReserveInPlay = False
        VmCounters = 0
        VmAttachedTo = Nothing
        VmAttachments.Clear
    End Sub
    Public Function SendTo(VpNewOwner As List(Of clsPlateauCard), Optional VpIndex As Integer = -1) As Boolean
    '-----------------------
    'Change la carte de zone
    '-----------------------
        If Not VmOwner Is VpNewOwner Then
            'On doit tout d'abord enlever tout ce qui était attaché / ce à quoi on était attaché
            For Each VpAttachment As clsPlateauCard In VmAttachments
                Call VpAttachment.AttachTo(Nothing, True)
            Next VpAttachment
            VmAttachments.Clear
            If Not VmAttachedTo Is Nothing Then
                VmAttachedTo.Attachments.Remove(Me)
            End If
            VmAttachedTo = Nothing
            VmHidden = False
            If VmReserve And Not VmReserveInPlay Then
                VmReserveInPlay = True
            Else
                VmOwner.Remove(Me)
            End If
            VmOwner = VpNewOwner
            If VpIndex <> -1 Then
                VmOwner.Insert(VpIndex, Me)
            Else
                VmOwner.Add(Me)
            End If
            Return True
        End If
        Return False
    End Function
    Public Sub AttachTo(VpHost As clsPlateauCard, Optional VpHostReadOnly As Boolean = False)
    '---------------------------------------------------------------------
    'Attache la carte à une autre (équipement, enchantement, empreinte...)
    '---------------------------------------------------------------------
        If Not VpHostReadOnly Then      'interdiction de supprimer des éléments d'une collection en cours d'énumération
            If Not VmAttachedTo Is Nothing Then
                VmAttachedTo.Attachments.Remove(Me)
            End If
        End If
        VmAttachedTo = VpHost
        If Not VmAttachedTo Is Nothing Then
            VmAttachedTo.Attachments.Add(Me)
        End If
    End Sub
    Public Overrides Function ToString() As String
        Return VmCardNameFR + " (" + VmCardName + ")"
    End Function
    Public Property Owner As List(Of clsPlateauCard)
        Get
            Return VmOwner
        End Get
        Set (VpOwner As List(Of clsPlateauCard))
            VmOwner = VpOwner
        End Set
    End Property
    Public ReadOnly Property NameVF As String
        Get
            Return VmCardNameFR
        End Get
    End Property
    Public ReadOnly Property NameVO As String
        Get
            Return VmCardName
        End Get
    End Property
    Public ReadOnly Property PicturePath As String
        Get
        Dim VpFile As String
        Dim VpCardName As String
            VpCardName = If(VmTransformed, VmTransformedCardName, VmCardName)
            VmMissingImg = False
            If Not VmHidden Then
                VpFile = Path.GetTempPath + clsModule.CgTemp + "\" + clsModule.AvoidForbiddenChr(VpCardName) + ".jpg"
                If File.Exists(VpFile) Then
                    Return VpFile
                Else
                    VmMissingImg = True
                    Return Application.StartupPath + clsModule.CgMagicBack
                End If
            Else
                Return Application.StartupPath + clsModule.CgMagicBack
            End If
        End Get
    End Property
    Public ReadOnly Property MissingPicture As Boolean
        Get
            Return VmMissingImg
        End Get
    End Property
    Public Property InReserve As Boolean
        Get
            Return VmReserve
        End Get
        Set (VpReserve As Boolean)
            VmReserve = VpReserve
        End Set
    End Property
    Public ReadOnly Property PlayedFromReserve As Boolean
        Get
            Return VmReserveInPlay
        End Get
    End Property
    Public ReadOnly Property IsAPermanent As Boolean
        Get
            Return Not ( VmCardType = "I" Or VmCardType = "N" Or VmCardType = "S" )
        End Get
    End Property
    Public ReadOnly Property MyType As String
        Get
            Return VmCardType
        End Get
    End Property
    Public Property Tapped As Boolean
        Get
            Return VmTapped
        End Get
        Set (VpTapped As Boolean)
            VmTapped = VpTapped
        End Set
    End Property
    Public ReadOnly Property Transformable As Boolean
        Get
            Return VmTransformable
        End Get
    End Property
    Public Property Transformed As Boolean
        Get
            Return VmTransformed
        End Get
        Set (VpTransformed As Boolean)
            VmTransformed = VpTransformed
        End Set
    End Property
    Public ReadOnly Property TransformedName As String
        Get
            Return VmTransformedCardName
        End Get
    End Property
    Public Property Hidden As Boolean
        Get
            Return VmHidden
        End Get
        Set (VpHidden As Boolean)
            VmHidden = VpHidden
        End Set
    End Property
    Public Property Counters As Integer
        Get
            Return VmCounters
        End Get
        Set (VpCounters As Integer)
            VmCounters = VpCounters
        End Set
    End Property
    Public ReadOnly Property IsAttached As Boolean
        Get
            Return VmAttachedTo IsNot Nothing
        End Get
    End Property
    Public Property Attachments As List(Of clsPlateauCard)
        Get
            Return VmAttachments
        End Get
        Set (VpAttachments As List(Of clsPlateauCard))
            VmAttachments = VpAttachments
        End Set
    End Property
End Class
