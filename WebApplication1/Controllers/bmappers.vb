Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Reflection
Imports System.Data
Imports System.Drawing


Public Class BMappers(Of t As {Class, New})
    Sub New()
        Dim obj As New t()
        Properties = obj.GetType().GetProperties()
    End Sub
    Private Property dl As New dlayer
    Private Property dreader As SqlDataReader
    Private Property Properties As PropertyInfo()
    Private Property _DAOFactory As New DAOFactory
    Public Property ErrorMessage As String
    Public RowReturnIdentity As Integer = 0

  

   
    Public Function GetAprMangObject(ByVal Sql As String) As List(Of t)
        Dim returnlist As New List(Of t)
        Try
            Using con As New SqlConnection(dl.APRConnectionString(2))
                con.Open()
                Dim cmd As New SqlCommand(Sql, con)
                dreader = cmd.ExecuteReader(CommandBehavior.CloseConnection)

                If dreader.FieldCount > 0 Then
                    returnlist = Start()
                End If
            End Using

        Catch ex As Exception
            ErrorMessage = ex.Message
        End Try

        Return returnlist

    End Function
    Public Function GetSpcObject(ByVal Sql As String) As List(Of t)
        Dim returnlist As New List(Of t)
        Try
            Using con As New SqlConnection(dl.SPCConnectionString)
                con.Open()
                Dim cmd As New SqlCommand(Sql, con)
                dreader = cmd.ExecuteReader(CommandBehavior.CloseConnection)

                If dreader.FieldCount > 0 Then
                    returnlist = Start()
                End If
            End Using

        Catch ex As Exception
            ErrorMessage = ex.Message
        End Try

        Return returnlist

    End Function

    Public Function GetSpcLiveObject(ByVal Sql As String) As List(Of t)
        Dim returnlist As New List(Of t)
        Try
            Using con As New SqlConnection(dl.SPCLiveConnectionString)
                con.Open()
                Dim cmd As New SqlCommand(Sql, con)
                dreader = cmd.ExecuteReader(CommandBehavior.CloseConnection)

                If dreader.FieldCount > 0 Then
                    returnlist = Start()
                End If
            End Using

        Catch ex As Exception
            ErrorMessage = ex.Message
        End Try

        Return returnlist

    End Function
   
    Public Function GetSpcSP(ByVal cmd As SqlCommand) As List(Of t)
        Dim returnlist As New List(Of t)
        Try
            dreader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            If dreader.FieldCount > 0 Then
                returnlist = Start()
            End If
        Catch ex As Exception
            ErrorMessage = ex.Message
        End Try

        Return returnlist

    End Function

    Public Function GetReqParamAsObject(ByVal RequestParams As NameValueCollection) As t
        Dim hashtable As New Hashtable()
        Dim Obj As New t()
        Dim bustype As Type = Obj.GetType()
        Dim Util As New Utilities

        Properties = bustype.GetProperties()

        If Not Properties Is Nothing Then
            For Each info As PropertyInfo In Properties
                hashtable(info.Name.ToUpper()) = info
            Next

            Dim keys As ICollection = hashtable.Keys
            If hashtable.Count > 0 Then
                For Each key In keys
                    Dim info As PropertyInfo = hashtable(key)
                    If IsNothing(info) = False Then
                        If info.CanWrite Then
                            If IsNothing(RequestParams.GetValues(key)) = False And IsDBNull(RequestParams.GetValues(key)) = False Then
                                Dim rpvalue As Object = Util.ConvertType(RequestParams.GetValues(key)(0), info.PropertyType.Name)
                                info.SetValue(Obj, rpvalue, Nothing)
                            End If

                        End If
                    End If

                Next


            End If
        End If
        Return Obj
    End Function
    Private Function Start() As List(Of t)
        Dim hashtable As New Hashtable()
        Dim Obj As New t()
        Dim bustype As Type = Obj.GetType()
        Dim ReturnList As New List(Of t)
        Properties = bustype.GetProperties()
        Dim record As IDataRecord
        For Each info As PropertyInfo In Properties
            hashtable(info.Name.ToUpper()) = info
        Next

        While dreader.Read
            Dim newObj As New t()
            record = CType(dreader, IDataRecord)
            For i = 0 To record.FieldCount - 1
                Dim info As PropertyInfo = hashtable(dreader.GetName(i).ToUpper())
                If IsNothing(info) = False Then
                    If info.CanWrite And IsDBNull(record(i)) = False Then
                        Dim testobj As Object = record(i)
                        info.SetValue(newObj, record(i), Nothing)
                    End If
                End If
            Next
            ReturnList.Add(ProcessRecordSet(newObj))
        End While

        Return ReturnList
    End Function
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")> Public Function InsertSpcObject(ByVal sql As String, Optional ByVal obj As t = Nothing, Optional ByVal RetId As Boolean = False) As Boolean
        Dim returnint As Integer = 0
        Dim cnt As Integer = 0
        'If ParaStringArray.Length > 0 Then
        Using conn As New SqlConnection(dl.SPCConnectionString())
            If RetId = True Then
                sql = sql + " SELECT @@IDENTITY;"
            End If
            Dim cmd As New SqlCommand(sql, conn)
            If IsNothing(obj) = False Then
                Properties = obj.GetType().GetProperties()
                Dim ParaStringArray As String() = GetQueryStringParam(sql)
                Dim newobj As Object = ProcessRecordSet(obj)

                For Each item In ParaStringArray
                    Dim fieldprop As PropertyInfo = obj.GetType.GetProperty(item)
                    If Not fieldprop Is Nothing Then
                        cmd.Parameters.Add(_DAOFactory.GetRefparameter("@" + item, fieldprop.PropertyType().FullName()))
                        cmd.Parameters("@" + item).Value = fieldprop.GetValue(newobj, Nothing)
                    End If
                Next
            End If
            Try
                cmd.Connection.Open()
                If RetId = True Then
                    returnint = Convert.ToInt32(cmd.ExecuteScalar())
                Else
                    returnint = Convert.ToInt32(cmd.ExecuteNonQuery())
                End If


            Catch ex As Exception

            End Try
        End Using
        'End If

        If returnint = 1 Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Function InsertSpcObject_RetNum(ByVal sql As String, Optional ByVal obj As t = Nothing, Optional ByVal RetId As Boolean = False) As Boolean
        Dim returnint As Boolean = False
        Dim cnt As Integer = 0
        'If ParaStringArray.Length > 0 Then
        Using conn As New SqlConnection(dl.SPCConnectionString())
            If RetId = True And sql.IndexOf("INSERT ") >= 0 Then
                sql = sql + " SELECT @@IDENTITY;"
            End If
            Dim cmd As New SqlCommand(sql, conn)
            If IsNothing(obj) = False Then
                Properties = obj.GetType().GetProperties()
                Dim ParaStringArray As String() = GetQueryStringParam(sql)
                Dim newobj As Object = ProcessRecordSet(obj)

                For Each item In ParaStringArray
                    Dim fieldprop As PropertyInfo = obj.GetType.GetProperty(item)
                    cmd.Parameters.Add(_DAOFactory.GetRefparameter("@" + item, fieldprop.PropertyType().FullName()))
                    cmd.Parameters("@" + item).Value = fieldprop.GetValue(newobj, Nothing)
                Next
            End If
            Try
                cmd.Connection.Open()
                If RetId = True Then
                    RowReturnIdentity = Convert.ToInt32(cmd.ExecuteScalar())
                    If RowReturnIdentity > 0 Then
                        returnint = True
                    End If
                Else
                    If Convert.ToInt32(cmd.ExecuteNonQuery()) > 0 Then
                        returnint = True
                    End If

                End If


            Catch ex As Exception

            End Try
        End Using
        'End If


        Return returnint


    End Function

    Public Function InsertAprManagerObject(ByVal sql As String, ByVal CID As Integer, Optional ByVal obj As t = Nothing) As Boolean
        Dim returnint As Integer = 0
        Dim cnt As Integer = 0
        'If ParaStringArray.Length > 0 Then
        Using conn As New SqlConnection(dl.AprManagerConnectionString())
            Dim cmd As New SqlCommand(sql, conn)
            If IsNothing(obj) = False Then
                Properties = obj.GetType().GetProperties()
                Dim ParaStringArray As String() = GetQueryStringParam(sql)
                Dim newobj As Object = ProcessRecordSet(obj)

                For Each item In ParaStringArray
                    Dim fieldprop As PropertyInfo = obj.GetType.GetProperty(item)
                    cmd.Parameters.Add(_DAOFactory.GetRefparameter("@" + item, fieldprop.PropertyType().FullName()))
                    cmd.Parameters("@" + item).Value = fieldprop.GetValue(newobj, Nothing)
                Next
            End If
            Try
                cmd.Connection.Open()
                returnint = Convert.ToInt32(cmd.ExecuteNonQuery())

            Catch ex As Exception

            End Try
        End Using
        'End If

        If returnint = 1 Then
            Return True
        Else
            Return False
        End If


    End Function

    Private Function GetQueryStringParam(ByVal Sql As String) As String()
        Dim returnParam As New List(Of String)

        If Sql <> "" Then
            Dim stringsep() As String = Sql.Split("@")
            For i = 0 To stringsep.Length - 1
                If i > 0 Then
                    If Not String.IsNullOrWhiteSpace(stringsep(i).Split(",")(0)) And stringsep(i).Split(",").Length > 1 Then
                        returnParam.Add(stringsep(i).Split(",")(0))
                    ElseIf Not String.IsNullOrWhiteSpace(stringsep(i).Split(" ")(0)) And stringsep(i).Split(" ").Length > 1 Then
                        returnParam.Add(stringsep(i).Split(" ")(0))
                    ElseIf Not String.IsNullOrWhiteSpace(stringsep(i).Split(")")(0)) And stringsep(i).Split(")").Length > 1 Then
                        returnParam.Add(stringsep(i).Split(")")(0))
                    End If
                End If
            Next
            Return returnParam.ToArray()
        Else
            Return Nothing
        End If

    End Function

    Private Function ProcessRecordSet(ByRef obj As Object) As Object
        Dim returndict As New Dictionary(Of String, Object)

        If IsNothing(obj) = False Then

            For Each mfield As PropertyInfo In Properties
                If IsNothing(mfield.GetValue(obj, Nothing)) = True Then
                    Dim fdatatype = mfield.PropertyType.ToString()
                    Select Case fdatatype
                        Case "System.String"
                            mfield.SetValue(obj, "", Nothing)
                        Case "System.Int32", "System.Int64"
                            mfield.SetValue(obj, 0, Nothing)
                        Case "System.Boolean"
                            mfield.SetValue(obj, True, Nothing)
                        Case "System.Object"
                            mfield.SetValue(obj, "", Nothing)
                        Case Else
                            mfield.SetValue(obj, Nothing, Nothing)
                    End Select
                End If
            Next
        End If

        Return obj
    End Function

End Class
