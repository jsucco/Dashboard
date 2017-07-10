Imports System.Data.SqlClient

Public Class DAOFactory
    Public Function Getparameter(ByVal dbParameterName As String, ByVal dbParameterValue As DbType) As IDbDataParameter

        Select Case dbParameterValue
            Case DbType.String
                Return New SqlParameter(dbParameterName, SqlDbType.NVarChar)
            Case DbType.Int16
                Return New SqlParameter(dbParameterName, SqlDbType.SmallInt)
            Case DbType.Int32
                Return New SqlParameter(dbParameterName, SqlDbType.Int)
            Case DbType.Boolean
                Return New SqlParameter(dbParameterName, SqlDbType.Bit)
            Case DbType.DateTime
                Return New SqlParameter(dbParameterName, SqlDbType.DateTime)
            Case DbType.Single                                                      'bms 11/4/04
                Return New SqlParameter(dbParameterName, SqlDbType.Real)            'bms 11/4/04
            Case DbType.Currency                                                    'bms 11/17/04
                Return New SqlParameter(dbParameterName, SqlDbType.Money)           'bms 11/17/04
            Case DbType.Binary                                              'SER 12/9/2008
                Return New SqlParameter(dbParameterName, SqlDbType.Binary)  'SER 12/9/2008
            Case DbType.Decimal                                                 ' DJP 8/30/12
                Return New SqlParameter(dbParameterName, SqlDbType.Decimal)     ' DJP 8/30/12
            Case Else
                Return Nothing
        End Select

    End Function

    Public Function GetRefparameter(ByVal dbParameterName As String, ByVal systype As String) As IDbDataParameter

        Select Case systype.ToString()
            Case "System.String"
                Return New SqlParameter(dbParameterName, SqlDbType.NVarChar)
            Case "System.Int16"
                Return New SqlParameter(dbParameterName, SqlDbType.SmallInt)
            Case "System.Int32"
                Return New SqlParameter(dbParameterName, SqlDbType.Int)
            Case "System.Boolean"
                Return New SqlParameter(dbParameterName, SqlDbType.Bit)
            Case "System.DateTime"
                Return New SqlParameter(dbParameterName, SqlDbType.DateTime)
            Case "System.Single"                                                      'bms 11/4/04
                Return New SqlParameter(dbParameterName, SqlDbType.Real)            'bms 11/4/04
            Case "System.Currency"                                                    'bms 11/17/04
                Return New SqlParameter(dbParameterName, SqlDbType.Money)           'bms 11/17/04
            Case "System.Binary"                                              'SER 12/9/2008
                Return New SqlParameter(dbParameterName, SqlDbType.Binary)  'SER 12/9/2008
            Case "System.Decimal"                                                 ' DJP 8/30/12"
                Return New SqlParameter(dbParameterName, SqlDbType.Decimal)     ' DJP 8/30/12
            Case Else
                Return Nothing
        End Select

    End Function
End Class
