Imports System.DirectoryServices.AccountManagement

Public Class _Default
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        Response.Redirect("~/APP/InspectionVisualizer.aspx")

        'Dim authCookie = Request.Cookies("APRKeepMeIn")
        'If IsNothing(authCookie) = False Then
        '    Try
        '        Dim CID_Print = Request.Cookies("APRKeepMeIn")("CID_Print")
        '        Dim User_ActivityKey = Request.Cookies("APR_UserActivityLog")("PrimaryKey")

        '        Response.Redirect("~/APP/InspectionVisualizer.aspx")
        '    Catch ex As Exception

        '    End Try


        'End If


    End Sub
End Class

