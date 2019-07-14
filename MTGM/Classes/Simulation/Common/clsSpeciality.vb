Public Class clsSpeciality
    Private VmEffortID As Integer = -1
    Private VmEffetID As Integer = -1
    Private VmEffort As String = ""
    Private VmEffet As String = ""
    Private VmInvocTapped As Boolean = False
    Private VmDoesntUntap As Boolean = False
    Private VmSpecial As Boolean = False
    Private Shared VmModelOutOfDateErr As Boolean = False
    Public Sub New(VpCard As String)
        VgDBCommand.CommandText = "Select EffortID, Effort, EffetID, Effet, InvocTapped, DoesntUntap From MySpecialUses Where Card = '" + VpCard.Replace("'", "''") + "';"
        Try
            VgDBReader = VgDBCommand.ExecuteReader
            With VgDBReader
                .Read
                If .HasRows Then
                    VmSpecial = True
                    VmEffortID = .GetValue(0)
                    VmEffetID = .GetValue(2)
                    VmEffort = .GetValue(1).ToString
                    VmEffet = .GetValue(3).ToString
                    VmInvocTapped = .GetBoolean(4)
                    VmDoesntUntap = .GetBoolean(5)
                Else
                    VmSpecial = False
                End If
                .Close
            End With
        Catch
            If Not VmModelOutOfDateErr Then
                Call clsModule.ShowWarning(clsModule.CgErr1)
                VmModelOutOfDateErr = True
            End If
        End Try
    End Sub
    Public Shared Function GetSpecId(VpSpec As String) As Integer
        VgDBCommand.CommandText = "Select SpecID From SpecialUse Where Description = '" + VpSpec.Replace("'", "''") + "';"
        Return VgDBCommand.ExecuteScalar
    End Function
    Public Shared Function GetSpecTxt(VpId As Integer) As String
        VgDBCommand.CommandText = "Select Description From SpecialUse Where SpecID = " + VpId.ToString + ";"
        Return VgDBCommand.ExecuteScalar
    End Function
    Public Shared Function GetSpecHlp(VpId As Integer) As String
        VgDBCommand.CommandText = "Select Aide From SpecialUse Where SpecID = " + VpId.ToString + ";"
        Return VgDBCommand.ExecuteScalar
    End Function
    Public ReadOnly Property EffortID As Integer
        Get
            Return VmEffortID
        End Get
    End Property
    Public ReadOnly Property EffetID As Integer
        Get
            Return VmEffetID
        End Get
    End Property
    Public ReadOnly Property Effort As String
        Get
            Return VmEffort
        End Get
    End Property
    Public ReadOnly Property Effet As String
        Get
            Return VmEffet
        End Get
    End Property
    Public ReadOnly Property InvocTapped As Boolean
        Get
            Return VmInvocTapped
        End Get
    End Property
    Public ReadOnly Property DoesntUntap As Boolean
        Get
            Return VmDoesntUntap
        End Get
    End Property
    Public ReadOnly Property IsSpecial As Boolean
        Get
            Return VmSpecial
        End Get
    End Property
End Class
