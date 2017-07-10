Namespace InspectionLibrary
    Public Class InspectionScatterPlot
        Public Property DATEVAL As String
        Public Property DHU_1 As Double
        Public Property DHU_2 As Double
        Public Property DHU_3 As Double
        Public Property DHU_4 As Double
    End Class

    Public Class BubbleChart1
        Public Property ID As String
        Public Property DHU As Double
        Public Property Units As Integer
        Public Property UnitCost As Double
        Public Property Name As String
    End Class

    Public Class InspectionSummary
        Public Property id As Integer
        Public Property JobType As String
        Public Property JobNumber As String
        Public Property UnitDesc As String
        Public Property CID As String
        Public Property Technical_PassFail As String
        Public Property FINISHED As String
        Public Property DHU As Double
        Public Property UpdatedDHU As Boolean
        Public Property UpdatedInspectionStarted As Boolean
    End Class
    Public Class LineChart1
        Public Property DATEBEGIN As String
        Public Property Major As Integer
        Public Property Minor As Integer
        Public Property Repairs As Integer
        Public Property Scraps As Integer
    End Class
End Namespace

Public Class ApplicationLog
    Public Property date_added As DateTime
    Public Property type As String
    Public Property Target As String
    Public Property Message As String
    Public Property application_name As String
End Class

Public Class SingleObject
    Public Property Object1 As Object
    Public Property Object2 As Integer
    Public Property Object3 As Object
End Class