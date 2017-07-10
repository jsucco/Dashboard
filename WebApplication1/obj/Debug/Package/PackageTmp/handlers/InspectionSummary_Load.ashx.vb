Imports System.Web
Imports System.Web.Services
Imports System.Web.Script.Serialization

Public Class InspectionSummary_Load
    Implements System.Web.IHttpHandler, IRequiresSessionState
    Dim InspectionSummary As New List(Of InspectionLibrary.InspectionSummary)
    Dim Inspect As New Inspection
    Dim CurrentSessionID As String

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim RequestParams As NameValueCollection = context.Request.Params
        Dim jser As New JavaScriptSerializer
        CurrentSessionID = RequestParams.GetValues("SessionID")(0).ToString()
        InspectionSummary = Inspect.GetInspectionSummary()

        Dim lastCachedObj = context.Cache("SummaryGrid_" + CurrentSessionID)

        If Not lastCachedObj Is Nothing Then
            Dim Cacheijs As New List(Of InspectionLibrary.InspectionSummary)
            Cacheijs = lastCachedObj

            For Each item As InspectionLibrary.InspectionSummary In InspectionSummary
                Dim objrow = (From v In Cacheijs Where v.id = item.id Select v).ToArray()
                If Not objrow Is Nothing Then
                    If objrow.Length = 0 Then
                        context.Cache.Insert("SummaryGrid_" + CurrentSessionID, InspectionSummary)
                        lastCachedObj = InspectionSummary
                        item.UpdatedInspectionStarted = True
                    Else
                        If objrow(0).DHU <> item.DHU Then
                            item.UpdatedDHU = True
                        End If
                        If objrow(0).Technical_PassFail <> item.Technical_PassFail Then
                            item.UpdatedDHU = True
                        End If
                    End If
                End If

            Next
        End If

        If InspectionSummary.Count > 0 Then
            context.Cache.Insert("SummaryGrid_" + CurrentSessionID, InspectionSummary)
        End If
        context.Response.Write(jser.Serialize(InspectionSummary))
    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class