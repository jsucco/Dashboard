Imports System.Data.SqlClient




Public Class dlayer

    Private ApplicationName As String = "APRDashboard"
    Private ConnectionTimeout As Integer = 30
    Private APRTRNconnection As String = ""
    Public Function SPCConnectionString() As String

        Dim constrg As String = ConfigurationManager.ConnectionStrings("SpcSTCMAIN2connectionstring").ConnectionString
        Dim sb As New SqlConnectionStringBuilder(constrg)

        If String.IsNullOrWhiteSpace(sb.ApplicationName) Then
            sb.ApplicationName = ApplicationName
        Else
            sb.ApplicationName = ApplicationName
        End If

        sb.ConnectTimeout = 25

        Return sb.ToString

    End Function
    Public Function SPCLiveConnectionString() As String

        Dim constrg As String = ConfigurationManager.ConnectionStrings("SpcSTCMAINLIVEconnectionstring").ConnectionString
        Dim sb As New SqlConnectionStringBuilder(constrg)

        If String.IsNullOrWhiteSpace(sb.ApplicationName) Then
            sb.ApplicationName = ApplicationName
        Else
            sb.ApplicationName = ApplicationName
        End If

        sb.ConnectTimeout = 25

        Return sb.ToString

    End Function

    Public Function InspectConnectionString() As String

        'GetCtxConnectionString()

        'Dim constrg As String = ConfigurationManager.ConnectionStrings(Spcconnection).ConnectionString
        Dim constrg As String = ConfigurationManager.ConnectionStrings("Inspectionconnectionstring").ConnectionString
        Dim sb As New SqlConnectionStringBuilder(constrg)

        If String.IsNullOrWhiteSpace(sb.ApplicationName) Then
            sb.ApplicationName = ApplicationName
        Else
            sb.ApplicationName = "SPC"
        End If

        sb.ConnectTimeout = 25

        Return sb.ToString

    End Function

    Public Function APRConnectionString(ByVal Database As Integer) As String

        GetTRN_APRSTTString(Database)

        Dim constrg As String = ConfigurationManager.ConnectionStrings(APRTRNconnection).ConnectionString
        Dim sb As New SqlConnectionStringBuilder(constrg)

        If String.IsNullOrWhiteSpace(sb.ApplicationName) Then
            sb.ApplicationName = ApplicationName
        Else
            sb.ApplicationName = "APR"
        End If

        If ConnectionTimeout > 0 Then
            sb.ConnectTimeout = ConnectionTimeout
        Else
            sb.ConnectTimeout = ConnectionTimeout
        End If

        Return sb.ToString


    End Function

    Public Function AprManagerConnectionString() As String

        Dim constrg As String = ConfigurationManager.ConnectionStrings("AprManager_string").ConnectionString
        Dim sb As New SqlConnectionStringBuilder(constrg)

        If String.IsNullOrWhiteSpace(sb.ApplicationName) Then
            sb.ApplicationName = ApplicationName
        Else
            sb.ApplicationName = "SPC"
        End If

        sb.ConnectTimeout = 25

        Return sb.ToString

    End Function

    Public Sub GetTRN_APRSTTString(ByVal Database As Integer)

        Select Case Database
            Case Is = 1
                APRTRNconnection = "AprSTT_TRNXstring"
            Case Is = 2
                APRTRNconnection = "AprManager_string"
            Case Is = 3
                APRTRNconnection = "SpcInspection_TRNXstring"
            Case Else
                APRTRNconnection = "AprSTT_TRNXstring"
        End Select


    End Sub

End Class
