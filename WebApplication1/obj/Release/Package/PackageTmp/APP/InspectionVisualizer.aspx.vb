Imports System.Web.Script.Serialization
Imports System.Reflection
Imports System.Web.Configuration

Public Class InspectionVisualizer
    Inherits System.Web.UI.Page
    Public fromdate As String
    Public todate As String
    Public ListChart1Data As String
    Public ListChartFields As String
    Public CurrentSessionID As String
    Public LastDefectID As String
    Public costFlagServer As String = "false"
    Public uristring As String = ""
    Dim jser As New JavaScriptSerializer()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        fromdate = Date.Now.AddDays(-21).ToString("MM/dd/yyyy")
        todate = Date.Now.ToString("MM/dd/yyyy")
        LoadBaseuri()
        CurrentSessionID = Me.Session.SessionID
        Dim Inspect As New APRDashboard.Inspection
        Dim listds As New List(Of APRDashboard.InspectionLibrary.LineChart1)

        listds = Inspect.GetLineChart1(todate, fromdate)
        Dim listso As New List(Of SingleObject)
        Dim bmapso As New BMappers(Of SingleObject)

        listso = bmapso.GetSpcLiveObject("SELECT TOP(1) DefectID as Object1 FROM DefectMaster ORDER BY DefectID DESC")
        If listso.Count > 0 Then
            LastDefectID = listso.ToArray()(0).Object1
        End If
        ListChart1Data = jser.Serialize(listds)
        ListChartFields = jser.Serialize(GetChartField(New APRDashboard.InspectionLibrary.LineChart1))

        Dim authCookie = Request.Cookies("UID")

        If IsNothing(authCookie) = False Then

            Dim cypher As New cypher()
            Dim userval As Object = cypher.MD5Decrypt(authCookie.Value, "8P?5x1d9KIO")
            If userval <> "" Then
                Dim bmap_so As New BMappers(Of SingleObject)
                Dim list_so As New List(Of SingleObject)
                Dim sql As String = "SELECT Address AS Object1 FROM EmailMaster WHERE ADMIN = 1"
                list_so = bmap_so.GetAprMangObject(sql)
                costFlagServer = "false"
                If list_so.Count > 0 Then
                    Dim soarray = list_so.ToArray()

                    For Each item In soarray
                        Dim splitar = item.Object1.ToString().Split("@")
                        If splitar.Count > 1 Then
                            If userval = splitar(0) Or userval = "textile\" + splitar(0) Then
                                costFlagServer = "true"
                            End If
                        End If
                    Next
                End If
            End If
        End If

    End Sub

    Private Sub LoadBaseuri()
        Dim fullurl As String = HttpContext.Current.Request.Url.AbsoluteUri

        Dim DOMAINFLAG As Boolean = False
        Dim CNTTEST As Integer = 0
        If Not fullurl Is Nothing Then
            Dim fullurlarray = fullurl.Split("/")
            If fullurlarray.Length > 3 Then
                For Each item In fullurlarray
                    CNTTEST = item.ToUpper().IndexOf("STANDARDTEXTILE")
                    If CNTTEST >= 0 Then
                        DOMAINFLAG = True
                    End If
                Next
                If DOMAINFLAG = True Then
                    uristring = fullurlarray(0).ToString() + "//" + fullurlarray(2).ToString() + "/" + fullurlarray(3).ToString()

                Else
                    uristring = fullurlarray(0).ToString() + "//" + fullurlarray(2).ToString()

                End If
            End If
        End If
    End Sub

    Public Function GetChartField(ByVal classobj As Object) As String

        Dim bustype As Type = classobj.GetType()
        Dim ReturnList As New List(Of String)
        Dim Properties As PropertyInfo()
        Properties = bustype.GetProperties()
        For Each info As PropertyInfo In Properties
            ReturnList.Add(info.Name.ToUpper())
        Next

        Return jser.Serialize(ReturnList)
    End Function

End Class