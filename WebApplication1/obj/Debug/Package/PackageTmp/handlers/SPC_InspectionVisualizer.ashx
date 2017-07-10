<%@ WebHandler Language="VB" Class="SPC_InspectionVisualizer" %>

Imports System
Imports System.Web
Imports App.Utilities.Web.Handlers
Imports System.Web.Script.Serialization
Imports System.Data.SqlClient
Imports System.Data
Imports System.Globalization
Imports System.Reflection
    
Public Class SPC_InspectionVisualizer
    Inherits BaseHandler
    
    Dim jser As New JavaScriptSerializer()
    
    Public Function GetDHU_Scat(ByVal CID As String) As String
        Dim _todate As DateTime = DateTime.Now
        Dim _formdate As DateTime = DateTime.Now.AddDays(-14)
        'Dim _todate As DateTime = DateTime.Parse(Todate)
        'Dim _formdate As DateTime = DateTime.Parse(Fromdate)
        Dim Inspect As New APRDashboard.Inspection
        Dim listds As New List(Of APRDashboard.InspectionLibrary.InspectionScatterPlot)
            
        listds = Inspect.GetScatterPlotData(_todate, _formdate, "999")
        
        
        Return jser.Serialize(listds) + "%%%" + GetScatterChartFields(_formdate, _todate, CID)
       
        
    End Function
    Public Function GetBubbleChart1(ByVal fromdate As String, todate As String, ByVal costFlag As String) As String
        'Dim _todate As DateTime = DateTime.Now
        'Dim _formdate As DateTime = DateTime.Now.AddDays(-14)
        Dim _todate As DateTime = DateTime.Parse(todate)
        Dim _formdate As DateTime = DateTime.Parse(fromdate)
        
        Dim Inspect As New APRDashboard.Inspection
        Dim listds As New List(Of APRDashboard.InspectionLibrary.BubbleChart1)
            
        listds = Inspect.GetBubbleChart1(_todate, _formdate)
        If listds.Count > 0 And costFlag = "false" Then
            Dim listdsmax = (From v In listds Select v.UnitCost).Max()
            For Each item In listds
                item.UnitCost = item.UnitCost / CType(listdsmax, Double)
            Next
        End If
        Return jser.Serialize(listds) + "%%%" + GetChartField(New APRDashboard.InspectionLibrary.BubbleChart1)
        
        
    End Function
    
    Public Function GetLineChart1(ByVal fromdate As String, todate As String) As String
        'Dim _todate As DateTime = DateTime.Now
        'Dim _formdate As DateTime = DateTime.Now.AddDays(-14)
        Dim _todate As DateTime = DateTime.Parse(todate)
        Dim _formdate As DateTime = DateTime.Parse(fromdate)
        
        Dim Inspect As New APRDashboard.Inspection
        Dim listds As New List(Of APRDashboard.InspectionLibrary.LineChart1)
            
        listds = Inspect.GetLineChart1(_todate, _formdate)
                                                
        Return jser.Serialize(listds) + "%%%" + GetChartField(New APRDashboard.InspectionLibrary.LineChart1)
        
        
    End Function
    Public Function GetUpdatedLineChart1(ByVal DefectID As String, ByVal LASTDATE As String) As String
        
        Dim Inspect As New APRDashboard.Inspection
        Dim lastdateparsed As DateTime = DateTime.Parse(LASTDATE)
        Dim listds As New List(Of APRDashboard.InspectionLibrary.LineChart1)
        Dim LastDefectID As Integer
        listds = Inspect.GetUpdatedLineChart1(lastdateparsed, CType(DefectID, Integer))
        
        If listds.Count > 0 Then
            Dim listso As New List(Of APRDashboard.SingleObject)
            Dim bmapso As New APRDashboard.BMappers(Of APRDashboard.SingleObject)

            listso = bmapso.GetSpcLiveObject("SELECT TOP(1) DefectID as Object1 FROM DefectMaster ORDER BY DefectID DESC")
            If listso.Count > 0 Then
                LastDefectID = listso.ToArray()(0).Object1
            End If
                                                
            Return jser.Serialize(listds) + "%%%" + GetChartField(New APRDashboard.InspectionLibrary.LineChart1) + "%%%" + LastDefectID.ToString()
        Else
            Return "0"
            
        End If
        
        
    End Function
    
    
    Private Function GetScatterChartFields(ByVal fromdate As DateTime, ByVal todate As DateTime, ByVal CID As String) As String
        Dim sql As String
        
        Dim fromdatestring As String = fromdate.ToString("MM/dd/yyyy") + " 00:00:00"
        Dim todatestring As String = todate.ToString("MM/dd/yyyy") + " 23:59:59"
        
        If CID <> "999" Then
            sql = "SELECT DISTINCT TOP(4) tm.Name as Object1, js.TemplateId as Object2, COUNT(js.id) as Object3" & vbCrLf &
                      "FROM InspectionJobSummary js INNER JOIN TemplateName tm ON js.TemplateId = tm.TemplateId" & vbCrLf &
                      "WHERE js.CID = '" & CID & "' and tm.LineType <> 'ROLL' and js.Inspection_Started > = '" & fromdatestring & "' and js.Inspection_Started <= '" & todatestring & "'" & vbCrLf &
                      "GROUP BY js.TemplateId, tm.Name" & vbCrLf &
                      "ORDER BY Object3 desc"
        Else
            sql = "SELECT DISTINCT TOP(4) tm.Name as Object1, js.TemplateId as Object2, COUNT(js.id) as Object3" & vbCrLf &
                 "FROM InspectionJobSummary js INNER JOIN TemplateName tm ON js.TemplateId = tm.TemplateId" & vbCrLf &
                 "WHERE tm.LineType <> 'ROLL' and js.Inspection_Started > = '" & fromdatestring & "' and js.Inspection_Started <= '" & todatestring & "'" & vbCrLf &
                 "GROUP BY js.TemplateId, tm.Name" & vbCrLf &
                 "ORDER BY Object3 desc"
        End If
        
        Dim bmapso As New APRDashboard.BMappers(Of APRDashboard.SingleObject)
        Dim listso As New List(Of APRDashboard.SingleObject)

        listso = bmapso.GetSpcLiveObject(sql)

        Return jser.Serialize(listso)
        
    End Function
    
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
