Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.IO
Imports System.Net.Mail

Public Class Utilities
    Public Property _DAOFactory As New DAOFactory
    Public Property DL As New dlayer
    Public newlogobj As New ApplicationLog
    Private bmap_log As New BMappers(Of ApplicationLog)

    Public Sub Log()
        Try
            If IsNothing(newlogobj) = False Then
                newlogobj.date_added = Date.Now
                bmap_log.InsertAprManagerObject("INSERT INTO SPC_ApplicationLog (type, Target, Message, application_name) VALUES (@type,@Target,@Message,@application_name)", 578, newlogobj)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Function ConvertType(ByVal Obj As Object, ByVal Typename As String) As Object
        Dim returnobj As Object
        Dim typenamefm As String = Typename.ToUpper()
        Select Case typenamefm
            Case "STRING"
                Try
                    returnobj = CType(Obj, String)
                Catch ex As Exception
                    returnobj = ""
                End Try

            Case "INT32", "INT64", "INTEGER"
                If IsNumeric(Obj) = True And IsDBNull(Obj) = False Then
                    Try
                        returnobj = CType(Obj, Integer)
                    Catch ex As Exception
                        returnobj = 0
                    End Try
                Else
                    returnobj = 0
                End If
            Case "BOOLEAN"
                If IsDBNull(Obj) = False Then
                    Try
                        returnobj = CType(Obj, Boolean)
                    Catch ex As Exception
                        returnobj = False
                    End Try

                Else
                    returnobj = False
                End If
            Case "DECIMAL"
                If IsNumeric(Obj) = True And IsDBNull(Obj) = False Then
                    Try
                        returnobj = CType(Obj, Decimal)
                    Catch ex As Exception
                        returnobj = 0
                    End Try

                Else
                    returnobj = 0
                End If
            Case "DATETIME"
                If IsDate(Obj) = True And IsDBNull(Obj) = False Then
                    Try
                        returnobj = CType(Obj, DateTime)
                    Catch ex As Exception
                        returnobj = New DateTime(1900, 1, 1)
                    End Try

                Else
                    returnobj = New DateTime(1900, 1, 1)
                End If
            Case Else
                returnobj = Obj
        End Select

        Return returnobj
    End Function

    Public Function ConvertStrFractionToDecimal(ByVal stdiv As String) As Decimal
        Dim i, k As Integer, num, den As Object, wholenum As Double = 0, returnnum As Decimal = 0
        Dim opp As Integer = -1
        i = stdiv.IndexOf("/")
        k = stdiv.Length
        If i > 0 Then
            num = stdiv.Substring(i - 1, 1)
            den = stdiv.Substring(i + 1, k - i - 1)
            opp = stdiv.ToString().IndexOf("-")
            Dim wholenumstr As String = stdiv.Substring(0, i - 1)
            If IsNumeric(wholenumstr) And num.ToString().Trim().Length > 0 And den.ToString().Trim().Length > 0 Then
                returnnum = Convert.ToDecimal(wholenumstr) + (CType(num, Decimal) / CType(den, Decimal))
            Else
                returnnum = (CType(num, Decimal) / CType(den, Decimal))
            End If
        Else
            If IsNumeric(stdiv) Then
                returnnum = Convert.ToDecimal(stdiv)
            End If
        End If

        If opp >= 0 Then
            returnnum = returnnum * (-1)
        End If

        Return returnnum

    End Function
End Class


