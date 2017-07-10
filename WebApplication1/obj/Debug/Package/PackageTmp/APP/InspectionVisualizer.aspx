<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/STCDashboard.master" CodeBehind="InspectionVisualizer.aspx.vb" Inherits="APRDashboard.InspectionVisualizer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="Titlediv" style="position:relative; float:right; top:-65px;">
        <label style="font-family:Arial; font-size:30px; font-weight:bold; position:relative; color: #646D6D">Inspection Dashboard</label>
    </div>
    <div id="DHU_Scat_Div" style="position:absolute; top:75px; left: 10px;">
        <div id="DHU_Scat_Holder"></div>
    </div>
    <div id="Bubble_Scat_Div" style="position:absolute; top:56%; left: 10px; width: 54%; height: 500px;">
        <div id="Bubble_Scat_Holder"></div>
    </div>
    <div id="LineChart1_Div" style="position:absolute; top:60px; left: 40%; width: 58%; height: 47%;">
        <div id="dashboard_div">
            <div id="LineChart1_Holder"></div>
            <div id="filter_div" style="position:absolute; left:15%; top: 98%;"></div>
           </div>
    </div>
    <div id="SummaryGrid_Div" style="position:absolute; top:62%; left: 46%; width: 49%;">
        <table id="ijsgrid" style=" font-size:medium; Z-INDEX: 104; font-weight:800; ">
                </table>
        <div id="gridpager"></div>

    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ControlOptions" runat="server">
    <style>
        /*circle {
            r:3.5px; !important
        }*/
        .ui-jqgrid tr.jqgrow td { font-size: 13px !important;  font-weight:bolder  !important; }
    </style>
     

    <script type="text/javascript" src="../Scripts/jquery-1.11.1.js"></script>

    <script type="text/javascript" src="../Scripts/jsapi.js"></script>
    <script type="text/javascript" src="../Scripts/jquery.jqGrid.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery.jqGrid.js"></script>
    <link href="../Content/ui.jqgrid.css" rel="stylesheet" type="text/css" />
   <link rel="stylesheet" href="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css">

    <script type="text/javascript">
        setInterval('window.location.reload()', 60000);
        var DTScatData;
        var DTScatFields;
        var BBScatData;
        var BBScatFields;
        var LineChart1Data = <%=ListChart1Data%>;
        var LineChart1Fields =<%=ListChartFields%>;
        var fromdate = '<%=fromdate%>'; 
        var todate = '<%=todate%>';
        var SessionID = '<%=CurrentSessionID%>';
        var LastDefectID = '<%=LastDefectID%>';
        var RangeSliderLeftDate = fromdate;
        var RangeSliderRightDate = todate; 
        var handlerstr = '<%=uristring%>';
        var costFlagIn = '<%=costFlagServer%>';
       // google.load('visualization', '1.1', { packages: ['scatter'] });
        window.alert = function() { };
        google.load("visualization", "1.0", { 'packages': ["corechart"] });
        //google.load("visualization", "1.1");
        google.load('visualization', '1.0', { 'packages': ['controls'] });
        google.setOnLoadCallback(drawDashboard2);
        
        $(function () {
            document.body.style.zoom = "100%"
            var htstr = $(window).height()*1.7 + "px"
            $("#sitebody").css("height", htstr);
            datahandler.DHUBYTEMP_Scat();
            //google.load("visualization", "1", { packages: ["corechart"] });
            datahandler.GetBubbleChart1();

            charthandler.RenderSummaryGrid();
            var ua = window.navigator.userAgent
            var msie = ua.indexOf ( "MSIE " )

            if (navigator.sayswho == "IE") { 
                charthandler.GetIELineChart();
            }
           // setInterval(function () { 
                datahandler.DHUBYTEMP_Scat();//}, 15000);
            //setInterval(function () { 
                datahandler.GetUpdatedLineChart1();//}, 180000);
     

        });
        navigator.sayswho= (function(){
            var ua= navigator.userAgent, tem,
            M= ua.match(/(opera|chrome|safari|firefox|msie|trident(?=\/))\/?\s*(\d+)/i) || [];
            if(/trident/i.test(M[1])){
                tem=  /\brv[ :]+(\d+)/g.exec(ua) || [];
                return 'IE';
            }
            if(M[1]=== 'Chrome'){
                tem= ua.match(/\b(OPR|Edge)\/(\d+)/);
                if(tem!= null) return tem.slice(1).join(' ').replace('OPR', 'Opera');
            }
            M= M[2]? [M[1], M[2]]: [navigator.appName, navigator.appVersion, '-?'];
            if((tem= ua.match(/version\/(\d+)/i))!= null) M.splice(1, 1, tem[1]);
            return M.join(' ');
        })();
        var datahandler = {
            DHUBYTEMP_Scat: function () {
                $.ajax({
                    url: handlerstr + '/handlers/SPC_InspectionVisualizer.ashx',
                    //url: 'http://apr.standardtextile.com/APRDashboard/handlers/SPC_InspectionVisualizer.ashx',
                    type: 'GET',
                    data: { method: 'GetDHU_Scat', args: {CID: '999'} },
                    success: function (data) {

                        if (data) {
                            var dataarr = data.split('%%%');
                            if (dataarr.length == 2) {
                                var titlestr = dataarr.split
                                var parseArr = $.parseJSON(dataarr[0]);
                                var titleArr = $.parseJSON(dataarr[1]);
                                var newarray = [];
                                var FieldCount = titleArr.length;
                                
                                $.each(parseArr, function (index, value) {

                                    
                                    switch (FieldCount) {
                                        case 1:
                                            newarray.push([value.DATEVAL, value.DHU_1]);
                                            break;
                                        case 2:
                                            newarray.push([value.DATEVAL, value.DHU_1, value.DHU_2]);
                                            break;
                                        case 3:
                                            newarray.push([value.DATEVAL, value.DHU_1, value.DHU_2, value.DHU_3]);
                                            break;
                                        case 4:
                                            newarray.push([value.DATEVAL, value.DHU_1, value.DHU_2, value.DHU_3, value.DHU_4]);
                                            break;
                                    }
                                    
                                });
     
                                DTScatData = newarray;
                                DTScatFields = titleArr;
                                charthandler.DHU_Scat();
                                //drawChart()
                            }
                            window.setTimeout(datahandler.DHUBYTEMP_Scat(), 610000);
                            //google.load('visualization', '1.1', { packages: ['scatter'] });

                            //google.setOnLoadCallback(charthandler.DHU_Scat);
                            //charthandler.DHU_Scat
                        }
                    },
                    error: function (a, b, c) {
                        alert(c);

                    }
                });
            },
            GetBubbleChart1: function () {
                $.ajax({
                    url: handlerstr + '/handlers/SPC_InspectionVisualizer.ashx',
                    //url: 'http://apr.standardtextile.com/APRDashboard/handlers/SPC_InspectionVisualizer.ashx',
                    type: 'GET',
                    data: { method: 'GetBubbleChart1', args: { fromdate: fromdate, todate: todate, costFlag: costFlagIn } },
                    success: function (data) {
           
                        if (data) {
                            var dataarr = data.split('%%%');
                            //console.log(dataarr);
                            if (dataarr.length == 2) {
                                var titlestr = dataarr.split
                                var parseArr = $.parseJSON(dataarr[0]);
                                var titleArr = $.parseJSON(dataarr[1]);
                                var newarray = [];
                                $.each(parseArr, function (index, value) {

                                    //console.log(value);
                                    newarray.push([value.ID, value.DHU, value.UnitCost, value.Name, value.Units]);
                                });
                                //console.log(newarray);
                                BBScatData = newarray;
                                BBScatFields = titleArr;
       
                                charthandler.Bubble_Scat();
                                //drawChart()
                            }
                            //google.load('visualization', '1.1', { packages: ['scatter'] });

                            //google.setOnLoadCallback(charthandler.DHU_Scat);
                            //charthandler.DHU_Scat
                        }
                    },
                    error: function (a, b, c) {
                        

                    }
                });

            },
            GetLineChart1: function () {
                $.ajax({
                    url: handlerstr +  '/handlers/SPC_InspectionVisualizer.ashx',
                    //url: 'http://apr.standardtextile.com/APRDashboard/handlers/SPC_InspectionVisualizer.ashx',
                    type: 'GET',
                    data: { method: 'GetLineChart1', args: { fromdate: fromdate, todate: todate } },
                    success: function (data) {
                        if (data) {
                            var dataarr = data.split('%%%');
                           
                            if (dataarr.length == 2) {
                                var titlestr = dataarr.split
                                var parseArr = $.parseJSON(dataarr[0]);
                                var titleArr = $.parseJSON(dataarr[1]);
                                var newarray = [];
                                $.each(parseArr, function (index, value) {

                                    //console.log(value);
                                    newarray.push([new Date(value.DATEBEGIN), value.Major, value.Minor, value.Repairs, value.Scraps]);
                                });
                                //console.log(newarray);
                                LineChart1Data = newarray;
                                LineChart1Fields = titleArr;
         
                                //charthandler.LineChart1_Binded;
                                
                                //drawChart()
                            }
                            //google.load('visualization', '1.1', { packages: ['scatter'] });

                            //google.setOnLoadCallback(charthandler.DHU_Scat);
                            //charthandler.DHU_Scat
                        }
                    },
                    error: function (a, b, c) {
                        alert(c);

                    }
                });
            },
            GetUpdatedLineChart1: function () {
                if (LineChart1Data && LineChart1Data.length > 0) {
 
                    var LastDate = new Date(LineChart1Data[LineChart1Data.length - 1].DATEBEGIN);
 
                    $.ajax({
                        url: handlerstr +  '/handlers/SPC_InspectionVisualizer.ashx',
                        //url: 'http://apr.standardtextile.com/APRDashboard/handlers/SPC_InspectionVisualizer.ashx',
                        type: 'GET',
                        data: { method: 'GetUpdatedLineChart1', args: { DefectID: LastDefectID, LASTDATE: LineChart1Data[LineChart1Data.length - 1].DATEBEGIN } },
                        success: function (data) {
         
                            if (data && data != '0') {
                                var dataarr = data.split('%%%');
                           
                                if (dataarr.length == 3) {
                                    var titlestr = dataarr.split
                                    var parseArr = $.parseJSON(dataarr[0]);
                                    var titleArr = $.parseJSON(dataarr[1]);
                                    var LastDefect = $.parseJSON(dataarr[2]);
                                    var newarray = [];
                                    var counter = 0;
       
                                    $.each(parseArr, function (index, value) {

                                        //console.log(value);
                                        if (counter == 0) { 
  
                                            //LineChart1Data[LineChart1Data.length - 1] = [value.DATEBEGIN, value.Major, value.Minor, value.Repairs, value.Scraps];
                                            LineChart1Data[LineChart1Data.length - 1].Major = LineChart1Data[LineChart1Data.length - 1].Major + value.Major
                                            LineChart1Data[LineChart1Data.length - 1].Minor = LineChart1Data[LineChart1Data.length - 1].Minor + value.Minor
                                            LineChart1Data[LineChart1Data.length - 1].Repairs = LineChart1Data[LineChart1Data.length - 1].Repairs + value.Repairs
                                            LineChart1Data[LineChart1Data.length - 1].Scraps = LineChart1Data[LineChart1Data.length - 1].Scraps + value.Scraps

                                        } else { 
                                            LineCart1Data.remove(0);
            
                                            LineChart1Data.push([value.DATEBEGIN, value.Major, value.Minor, value.Repairs, value.Scraps]);
                                        }
                                        
                                        counter = counter + 1;
                                    });
                                    //console.log(newarray);
                                    LineChart1Fields = titleArr;
                                    if (LineChart1Data.length > 0 ){ 
                                        drawDashboard2()
                                    }
                                    //window.setTimeout(datahandler.GetUpdatedLineChart1(), 180000);
                                    //charthandler.LineChart1_Binded;
                                    
                                    //drawChart()
                                }
                                //google.load('visualization', '1.1', { packages: ['scatter'] });

                                //google.setOnLoadCallback(charthandler.DHU_Scat);
                                //charthandler.DHU_Scat
                            }
                        },
                        error: function (a, b, c) {
                            alert(c);

                        }
                    });
                }
        }
        }; 
        var charthandler = {
            DHU_Scat: function () {
                var newarray = [];
                // Create the data table.
                var data = new google.visualization.DataTable();
                data.addColumn('string', 'DATEVAL');
                $.each(DTScatFields, function (index, value) {
                    data.addColumn('number', value.Object1);
                });

                data.addRows(DTScatData);

                var options = {
                    title: 'DHU BY DAY AND TEMPLATE',
                    fontSize:23,
                    subtitle: '(MAJORS ONLY)',
                    pointSize: 5,
                    titleTextStyle: {
                        color: 'black',
                        fontName: 'Arial',
                        bold: true
                    },
                    titlePosition: 'out',
                    width: $(window).width() * .42,
                    height: $(window).height() * .47,
                    colors: ['#6496c8', '#B0B579', '#FBB040', '#D31245'],
                    hAxis: { 
                        textStyle: {
                            fontSize: 10
                        }
                    },
                    vAxis: {
                        textStyle: {
                            fontSize: 9,
                        },
                        title: 'DHU%'
                    },
                    legend: {
                        textStyle: {
                            fontSize: 10
                        }
                    },
                    backgroundColor: 'transparent'
                };


                //var chart = new google.charts.Scatter(document.getElementById('DHU_Scat_Holder'));
                var chart = new google.visualization.ScatterChart(document.getElementById('DHU_Scat_Holder'));
                //chart.draw(data, google.charts.Scatter.convertOptions(options));
                chart.draw(data, options);
            },
            Bubble_Scat: function () {
                var data = new google.visualization.DataTable();
                var unittype;
                data.addColumn('string', 'ID');
                data.addColumn('number', 'DHU');
                if (costFlagIn == "true") { 
                    data.addColumn('number', 'UNITCOST');
                    unittype = " REAL COST";
                } else { 
                    data.addColumn('number', 'UNITVALUE');
                    unittype = "VALUE";
                }
                data.addColumn('string', 'NAME');
                data.addColumn('number', 'UNITS');

                data.addRows(BBScatData);

                var options = {
                    title: 'Correlation between DHU, UNIT ' + unittype + ' and Work Order Units',
                    hAxis: { title: 'DHU' },
                    vAxis: { title: 'COST MAGNITUDE' },
                    bubble: { textStyle: { fontSize: 11 } },
                    backgroundColor: 'transparent',
                    legend: {position: 'top', textStyle: {fontSize: 14}},
                    colors: ['#6496c8', '#B0B579', '#FBB040', '#D31245'],
                    height: $(window).height()*.44,
                    width: $(window).width() * .48,
                    fontSize:20
                };
                
                var chart = new google.visualization.BubbleChart(document.getElementById('Bubble_Scat_Holder'));
                $('#Bubble_Scat_Holder').empty();
                chart.draw(data, options);
            },
            RenderSummaryGrid: function () {
                $("#ijsgrid").jqGrid({
                    datatype: "json",
                    url: handlerstr +  '/handlers/InspectionSummary_Load.ashx',
                    //url: 'http://apr.standardtextile.com/APRDashboard/handlers/InspectionSummary_Load.ashx',
                    colNames: ['id', 'JobType', 'JobNumber', 'UnitDesc', 'Location', 'Pass/Fail', 'Finished', 'DHU', 'UpdatedDHU', 'UpdatedInspectionStarted'],
                    colModel: [
                            { name: 'id', index: 'id', hidden: false, editable: false, formatter: charthandler.formatijsGrid, width: 56 },
                            { name: 'JobType', index: 'JobType', hidden: true },
                            { name: 'JobNumber', index: 'JobNumber', editable: false, formatter: charthandler.formatijsGrid },
                            { name: 'UnitDesc', index: 'UnitDesc', width: 255, editable: false, formatter: charthandler.formatijsGrid },
                            { name: 'CID', index: 'CID', hidden: false, width: 85, formatter: charthandler.formatijsGrid },
                            { name: 'Technical_PassFail', index: 'Technical_PassFail',  editable: false, formatter: charthandler.formatijsGrid },
                            { name: 'FINISHED', index: 'FINISHED', editable: false, formatter: charthandler.formatijsGrid },
                            { name: 'DHU', index: 'DHU', editable: false, formatter: 'number', formatoptions: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' }},
                            { name: 'UpdatedDHU', index: 'UpdatedDHU', hidden: true, editable: false },
                            { name: 'UpdatedInspectionStarted', index: 'UpdatedInspectionStarted', hidden: true, editable: false }
                    ],
                    width: new Number($(window).width() *.49),
                    height: 'auto',
                    viewrecords: true,
                    gridview: true,
                    loadonce: true,
                    postData: {
                        CID: function () {
                            return '485';
                        },
                        SessionID: function () { 
                            return SessionID;
                        }
                    },
                    gridComplete: function () { 
                        //setInterval(function(){ jQuery("#ijsgrid").jqGrid('clearGridData', true).trigger('reloadGrid'); }, 15000);
                        setTimeout(function(){ $("#ijsgrid").jqGrid('setGridParam', 
                          { datatype: 'json' }).trigger('reloadGrid'); 
                            datahandler.GetBubbleChart1();}, 30000);
                        var griddata = $("#ijsgrid").jqGrid('getGridParam', 'data');
                        $.each(griddata, function(index, value) { 
                            if (value.UpdatedDHU == true) { 
                                if (value.Technical_PassFail == 'PASS') { 
                                    $("#" + value.id ).css("background","rgba(0, 128, 0, 0.38) 50% 50% repeat-x");
                                    setTimeout(function(){  $("#" + value.id ).css("background","none") }, 3000);
                                } else {
                                    $("#" + value.id ).css("background","rgba(241, 10, 10, 0.46) 50% 50% repeat-x");
                                    setTimeout(function(){  $("#" + value.id ).css("background","none") }, 3000);
                                }
                            }
                            if (value.UpdatedInspectionStarted == true) { 
 
                                setTimeout(function(){$("#" + value.id ).css("background","rgba(0, 128, 0, 0.38); 50% 50% repeat-x");  }, 1000);
                                
                                setTimeout(function(){  $("#" + value.id ).css("background","none") }, 6000);
                            }
                        });
                      

                    }

                });
               // jQuery("#ijsgrid").jqGrid('navGrid','#gridpager',{edit:false,add:false,del:false});
                //$('#ijsgrid').jqGrid('navGrid', '#gridpager',
                //{
                //    edit: false,
                //    add: false,
                //    del: false,
                //    search: false
                //}
                //    );
            },
            formatijsGrid: function (cellvalue, options, rowobject) {

 
                if (cellvalue == "01/01/00") { 
                    return "<span style='color:red; font-weight:bolder; font-size: 11px;'></span>"
                } else {
                    if (rowobject.Technical_PassFail == 'FAIL') {

                        return "<span style='color:red; font-weight:bolder; font-size: 13px;'>" + cellvalue + "</span>";

                    } else {
                        return "<span style='color:black; font-weight:800; font-size: 12px;'>" + cellvalue + "</span>";
                    }
                }
            },
            GetIELineChart: function () { 
                var data = new google.visualization.DataTable();

                data.addColumn('datetime', 'DATEBEGIN');
                data.addColumn('number', 'Majors');
                data.addColumn('number', 'Minors');
                data.addColumn('number', 'Repairs');
                data.addColumn('number', 'Scraps');
                var newarray = [];

                var counter = 0;
                $.each(LineChart1Data, function (index, value) {
                    if (counter > 300) {
                    var newdatestr = value.DATEBEGIN.split(" "); 
                    var newdate; 
                    if (newdatestr.length == 2) { 

                        var firstpart = newdatestr[0].split("-");
                        var secondpart = newdatestr[1].split(":"); 

                        newdate = new Date(firstpart[0], firstpart[1] - 1, firstpart[2], secondpart[0], 0, 0);

                    }
                    //console.log(value);
                    
                        newarray.push([newdate, value.Major, value.Minor, value.Repairs, value.Scraps]);
                    }
                    counter++;
                });
                data.addRows(newarray);
                console.log(newarray);
                var options = {
                    chart: {
                        title: 'Hourly Defects By Type',
                        fontSize:23
                    },
                    width: $(window).width() * .57,
                    height: $(window).height() * .47,
                    backgroundColor: 'transparent',
                    colors: ['#6496c8', '#B0B579', '#FBB040', '#D31245'],
                    
                };

                var chart = new google.visualization.LineChart(document.getElementById('LineChart1_Holder'));

                chart.draw(data, options);

            }
        }
        function drawDashboard2() {

            // Create our data table.
            console.log(navigator.sayswho);
            if (navigator.sayswho != "IE") { 
                var data = new google.visualization.DataTable();

                data.addColumn('datetime', 'DATEBEGIN');
                data.addColumn('number', 'Majors');
                data.addColumn('number', 'Minors');
                data.addColumn('number', 'Repairs');
                data.addColumn('number', 'Scraps');
                var newarray = [];
                $.each(LineChart1Data, function (index, value) {
                    
                    //console.log(value);
                    newarray.push([new Date(value.DATEBEGIN), value.Major, value.Minor, value.Repairs, value.Scraps]);
                });
                data.addRows(newarray);

                // Create a dashboard.
                var dashboard = new google.visualization.Dashboard(
                    document.getElementById('dashboard_div'));

                // Create a range slider, passing some options
                console.log(RangeSliderLeftDate);
                console.log(RangeSliderRightDate);
                var LineRangeSlider = new google.visualization.ControlWrapper({
                    'controlType': 'DateRangeFilter',
                    'containerId': 'filter_div',
                    'options': {
                        'filterColumnLabel': 'DATEBEGIN'
                    }
                });

                LineRangeSlider.setState({range: { start: RangeSliderLeftDate, end: RangeSliderRightDate }});
                var options = {
                    chart: {
                        title: 'Hourly Defects By Type'
                    },
                    width: $(window).width() * .57,
                    height: $(window).height() * .47,
                    backgroundColor: 'transparent',
                    colors: ['#6496c8', '#B0B579', '#FBB040', '#D31245']
                };

                var LineChart = new google.visualization.ChartWrapper({
                    'chartType': 'LineChart',
                    'containerId': 'LineChart1_Holder',
                    'options': {
                        'width': $(window).width() * .57,
                        'height': $(window).height() * .47,
                        'backgroundColor': 'transparent',
                        'colors': ['#6496c8', '#B0B579', '#FBB040', '#D31245'],
                        'title': 'Hourly Defects By Type',
                        'fontSize':'20'
                    }
                });

                dashboard.bind(LineRangeSlider, LineChart);

                dashboard.draw(data);

                google.visualization.events.addListener(LineRangeSlider, 'statechange', selectHandler);

                setTimeout(function () { 
                    $(".google-visualization-controls-rangefilter-thumblabel").each(function (index, value) { 

                    });
                
                }, 2000);
            }
        }
        function selectHandler (e) { 
 
            $(".google-visualization-controls-rangefilter-thumblabel").each(function (index, value) { 
                console.log(index);
                console.log($(value).text());
                if (index == 0) { 
                    var parsestr = $(value).text().split("-"); 
                    RangeSliderLeftDate = new Date(parsestr[0], parsestr[1] - 1, parsestr[2]);
                }
                if (index == 1) { 
                    var parsestr = $(value).text().split("-"); 
                    RangeSliderRightDate = new Date(parsestr[0], parsestr[1] - 1, parsestr[2]);
                }
            });

        }
    </script>
</asp:Content>
