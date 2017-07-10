Imports System.Data.SqlClient

Public Class Inspection
    Private DL As New dlayer

    Public Function GetScatterPlotData(ByVal todate As DateTime, ByVal fromdate As DateTime, ByVal CID As String) As List(Of InspectionLibrary.InspectionScatterPlot)
        Dim con As New SqlConnection(DL.SPCLiveConnectionString())
        Dim cmd As SqlCommand = con.CreateCommand()
        Dim rglist As New List(Of InspectionLibrary.InspectionScatterPlot)
        Dim retlist As New List(Of InspectionLibrary.InspectionScatterPlot)

        Try
            Using con
                con.Open()
                Using cmd
                    cmd = New SqlCommand("GetDHUByTemplate", con)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@TODATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                    cmd.Parameters.Add("@FROMDATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                    cmd.Parameters.Add("@CID", SqlDbType.VarChar).Direction = ParameterDirection.Input
                    cmd.Parameters("@TODATE").Value = todate
                    cmd.Parameters("@FROMDATE").Value = fromdate
                    cmd.Parameters("@CID").Value = CID

                    cmd.CommandTimeout = 5000

                    Dim bmap_rg As New BMappers(Of InspectionLibrary.InspectionScatterPlot)
                    rglist = bmap_rg.GetSpcSP(cmd)

                End Using
            End Using
        Catch ex As Exception
            Return Nothing
        End Try

        Return rglist
    End Function

    Public Function GetBubbleChart1(ByVal todate As DateTime, ByVal fromdate As DateTime) As List(Of InspectionLibrary.BubbleChart1)
        Dim con As New SqlConnection(DL.SPCConnectionString())
        Dim cmd As SqlCommand = con.CreateCommand()
        Dim rglist As New List(Of InspectionLibrary.BubbleChart1)
        Dim retlist As New List(Of InspectionLibrary.BubbleChart1)

        Try
            Using con
                con.Open()
                Using cmd
                    cmd = New SqlCommand("SP_GetDBBubbleChart2", con)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@TODATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                    cmd.Parameters.Add("@FROMDATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                    cmd.Parameters("@TODATE").Value = Date.Now
                    cmd.Parameters("@FROMDATE").Value = Date.Now.AddDays(-21)

                    cmd.CommandTimeout = 5000

                    Dim bmap_rg As New BMappers(Of InspectionLibrary.BubbleChart1)
                    rglist = bmap_rg.GetSpcSP(cmd)

                End Using
            End Using
        Catch ex As Exception
            Return Nothing
        End Try

        Return rglist
    End Function
    Public Function GetLineChart1(ByVal todate As DateTime, ByVal fromdate As DateTime) As List(Of InspectionLibrary.LineChart1)
        Dim con As New SqlConnection(DL.SPCLiveConnectionString())
        Dim cmd As SqlCommand = con.CreateCommand()
        Dim rglist As New List(Of InspectionLibrary.LineChart1)
        Dim retlist As New List(Of InspectionLibrary.LineChart1)

        Try
            Using con
                con.Open()
                Using cmd
                    cmd = New SqlCommand("SP_GetDefectsByTypeRange", con)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@TODATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                    cmd.Parameters.Add("@FROMDATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                    cmd.Parameters.Add("@CID", SqlDbType.VarChar).Direction = ParameterDirection.Input
                    cmd.Parameters("@TODATE").Value = Date.Now
                    cmd.Parameters("@FROMDATE").Value = fromdate
                    cmd.Parameters("@CID").Value = "999"

                    cmd.CommandTimeout = 5000

                    Dim bmap_rg As New BMappers(Of InspectionLibrary.LineChart1)
                    rglist = bmap_rg.GetSpcSP(cmd)

                End Using
            End Using
        Catch ex As Exception
            Return Nothing
        End Try

        Return rglist
    End Function
    Public Function GetUpdatedLineChart1(ByVal fromdate As DateTime, ByVal LastDefectID As Integer) As List(Of InspectionLibrary.LineChart1)
        Dim con As New SqlConnection(DL.SPCLiveConnectionString())
        Dim cmd As SqlCommand = con.CreateCommand()
        Dim rglist As New List(Of InspectionLibrary.LineChart1)
        Dim retlist As New List(Of InspectionLibrary.LineChart1)

        Try
            Using con
                con.Open()
                Using cmd
                    cmd = New SqlCommand("SP_GetUpdatedDefectsByTypeRange", con)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@LASTDATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                    cmd.Parameters.Add("@LASTDEFECTID", SqlDbType.Int).Direction = ParameterDirection.Input
                    cmd.Parameters.Add("@CID", SqlDbType.VarChar).Direction = ParameterDirection.Input
                    cmd.Parameters("@LASTDATE").Value = fromdate
                    cmd.Parameters("@LASTDEFECTID").Value = LastDefectID
                    cmd.Parameters("@CID").Value = "999"

                    cmd.CommandTimeout = 5000

                    Dim bmap_rg As New BMappers(Of InspectionLibrary.LineChart1)
                    rglist = bmap_rg.GetSpcSP(cmd)

                End Using
            End Using
        Catch ex As Exception
            Return Nothing
        End Try

        Return rglist
    End Function
    Public Function GetInspectionSummary() As Object

        Dim listis As New List(Of InspectionLibrary.InspectionSummary)
        Dim bmapis As New BMappers(Of InspectionLibrary.InspectionSummary)
        Dim sql As String = "SELECT TOP(14) ijs.id, ijs.JobType, ijs.JobNumber, ijs.UnitDesc, lm.Abreviation as CID, CASE WHEN ijs.Technical_PassFail = 0 THEN  'FAIL' WHEN ijs.Technical_PassFail = 1 THEN  'PASS' END AS Technical_PassFail, CONVERT(VARCHAR(20), ISNULL(ijs.Inspection_Finished, ''), 1) AS FINISHED, CASE WHEN ijs.SampleSize > 0 THEN (SELECT cast(count(*) as float) FROM DefectMaster WHERE InspectionJobSummaryId = ijs.id) * 100/ijs.SampleSize ELSE 0 END AS DHU FROM InspectionJobSummary ijs LEFT OUTER JOIN AprManager.dbo.LocationMaster lm on ijs.CID = SUBSTRING(lm.CID, 4, 3) WHERE ijs.Inspection_Started > getdate() - 21 ORDER BY ijs.id DESC"

        listis = bmapis.GetSpcObject(sql)

        Return listis
    End Function
End Class
