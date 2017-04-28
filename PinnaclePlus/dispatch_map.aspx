<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/PinnaclePlus_master.Master" CodeBehind="dispatch_map.aspx.vb" Inherits=".dispatch_map" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Pinnacle Plus: Dispatch Map</title>
    <style>



        /*Dropdown Start*/
            .dropbtn {

}

/* The container <div> - needed to position the dropdown content */
.dropdown {
    position: relative;
    display: inline-block;
}

/* Dropdown Content (Hidden by Default) */
.dropdown-content {
    display: none;
    position: absolute;
    background-color: #f9f9f9;
    min-width: 160px;
    box-shadow: 0px 8px 16px 0px rgba(0,0,0,0.2);
    z-index: 1;
}

/* Links inside the dropdown */
.dropdown-content a {
    color: black;
    padding: 12px 16px;
    text-decoration: none;
    display: block;
}

/* Change color of dropdown links on hover */
.dropdown-content a:hover {background-color: #f1f1f1}

/* Show the dropdown menu on hover */
.dropdown:hover .dropdown-content {
    display: block;
}

/* Change the background color of the dropdown button when the dropdown content is shown */
.dropdown:hover .dropbtn {

}
            /*DropDown End*/
          #sortable { list-style-type: none; margin: 0; padding: 0; width: 100%; }
  #sortable li { margin: 0 3px 3px 3px; padding: 4px;  font-size: 10pt; height: 22px; }
  
.hidden
{
    visibility:hidden;
}
#loading-img {
    background: url(icon/ajax.gif) center center no-repeat;
    height: 100%;
    z-index: 20;
    opacity: 1;
}
#overlay {
        background: #ffffff;
        display: none;
    position: absolute;
    top: 0;
    right: 0;
    bottom: 0;
    left: 0;
    opacity: 0.5;
    
}
    .numberCircle {

    display:inline-block;
    height: 24px;
    width: 24px;
    line-height: 24px;

    -moz-border-radius: 12px;
    border-radius: 12px;

    background-color: #3358ac;
    color: #ffffff;
    text-align: center;
    font-size: 10pt;
    }
    .numberCirclemi {
         display:inline-block;
    height: 24px;
    width: 24px;
    line-height: 24px;

    -moz-border-radius: 12px;
    border-radius: 12px;

    background-color: #ff6a00;
    color: #ffffff;
    text-align: center;
    font-size: 9pt;
        }

    #dvMap {
    height: calc(100vh - 115px);
    width: calc(100% - 450px);
    display:block;
    float:left ;
    }
    #pin_tools{
    width: 450px;
    height: calc(100vh - 115px);
    float:left ;
    display:block;
    
    }
    #tools_minifest{
    border:solid 1px #e4e4e4; 
    padding:2px;
    }
    #fresh_sel
    {
    border:solid 0px #e4e4e4; 
    padding:0px;
    }
    #man_display
    {
    border:none; 
    padding:2px;
    height: calc(100vh - 175px);
    }
     
     
  #selectable { list-style-type: none; margin: 0; padding: 0; width: 100%; }
  #selectable li { margin: 1px; padding: 1px; font-size: 10pt;  display:block; }
 .fresh_del {display:block; width:20px; float: left; }
 .fresh_txt {display:block;margin-left:20px;cursor:pointer;}
 .fresh_txt:hover { background: #FECA40;}
  
.selectable_old { display:block;  width:100%;}
.selectable_old:hover {background: #FECA40;  width:100%;}
  
    </style>
    <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?key=AIzaSyDM7-igdY9_vzj7-1ASLXQmLHPxNDB3OHE&libraries=drawing,geometry"></script>
    



    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table>
                <tr>
                    <td>
                        <asp:DropDownList ID="ddlHub" runat="server"></asp:DropDownList>
                    </td>
                    <td>
                        From:
                    </td>
                    <td >
                        <asp:TextBox ID="txtDate" runat="server" Width="70px"  ></asp:TextBox> 
                    </td>
                    <td>
                        To:
                    </td>
                    <td >
                        <asp:TextBox ID="txtDateTo" runat="server"  Width="70px"   ></asp:TextBox> 
                    </td>
                    <td>
                        <input id="Button1" type="button" value="Refresh" onclick="LoadManifest(0,true ); return false;" />
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlFilterAllStops" runat="server">
                            <asp:ListItem Text="All Stops" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Only UnAttached" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Only This Manifest" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Only This Manifest + UnAttached" Value="4"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        States:
                    </td>
                    <td>
                        <input id="txtFilterState" type="text" style="width:100px;"/>
                    </td>
                    <td>
                        Zips:
                    </td>
                    <td>
                        <input id="txtFilterZips" type="text" style="width:100px;"/>
                    </td>
                    <td>
                        Order Nos:
                    </td>
                    <td>
                        <input id="txtOrderNo" type="text" style="width:100px;"/>
                    </td>
                    
                </tr>
            </table>
    </div>

    
                <script type="text/javascript">
                    var dialog_Insert_Stop, dialog, dialog_alert, PrintDialog, dialog_Man, dialogtimeframe, dialog_Service_Time, OldStartTime, OldServiceTime, OldCascade, g_stop_id, OldTimeWindow, dialog_Route_Optimization
                    function iniDialog()
                    {
                        dialog_Insert_Stop = $("#dialog_Insert_Stop").dialog({
                            autoOpen: false,

                            width: 750,
                            modal: true,
                            buttons: {
                                "Save": InsertStop,
                                Cancel: function () {
                                    dialog_Insert_Stop.dialog("close");
                                }
                            }
                        });
                        dialog_alert = $("#dialog_alert").dialog({
                            autoOpen: false,

                            width: 550,
                            modal: true,
                            buttons: {

                                "OK": function () {
                                    dialog_alert.dialog("close");
                                }
                            }
                        });
                        PrintDialog = $("#dialog_Print").dialog({
                            autoOpen: false,

                            width: 550,
                            modal: true,
                            buttons: {
                                
                                "Close": function () {
                                    PrintDialog.dialog("close");
                                }
                            }
                        });
                        dialog = $("#dialog_form_Seq").dialog({
                            autoOpen: false,

                            width: 550,
                            modal: true,
                            buttons: {
                                "Save": changeOrderAjax,
                                Cancel: function () {
                                    dialog.dialog("close");
                                }
                            },
                            close: function () {


                            }
                        });
                        dialog_Man = $("#dialog_Man").dialog({
                            autoOpen: false,

                            width: 880,
                            modal: true,
                            buttons: {
                                "Save": SaveMan,
                                Cancel: function () {
                                    dialog_Man.dialog("close");
                                }
                            },
                            close: function () {


                            }
                        });
    
                        function InsertStop()
                        {
                            dialog_Insert_Stop.dialog("close");
                            $.ajax({
                                type: "POST",
                                url: "man_api.aspx/InsertStop",
                                data: '{MIN_ID:' + $("#ddlManifest").val() + ',MS_ID:' + $("#ddlAllStops").val() + ',Seq:' + $("select[id='ddlAllStops'] option:selected").index() + 1 + ',BeforeAfter:' + $("#ddlBeforeAfter").val() + ',StopStr:"' + StopStr + '"}',
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                beforeSend: function () { $('#overlay').show(); },
                                success: function (response) {
                                    $('#overlay').hide();
                                    
                                    FillManData($("#ddlManifest").val(), true)

                                },
                                failure: function (response) {
                                    $('#overlay').hide();
                                    dialog_Insert_Stop.dialog("close");
                                    alert(response.d);
                                },
                                error: function (response) {
                                    $('#overlay').hide();
                                    dialog_Insert_Stop.dialog("close");
                                    alert(response.d);
                                }
                            });
                        }
                        function SaveMan()
                        {
                            var objMan = {};
                            objMan.MIN_ID = parseInt($("#ddlManifest").val());
                            objMan.Hub = $("[id$=ddlHub]").val();
                            objMan.DRIVER_ID = parseInt($("#ddl_Man_Driver").val());
                            objMan.CO_DRIVER_ID1 = parseInt($("#ddl_Man_Co1").val());
                            objMan.CO_DRIVER_ID2 = parseInt($("#ddl_Man_Co2").val());
                            objMan.TRUCK_ID = parseInt($("#ddl_Man_Truck").val());
                            objMan.StartDate = $("[id$=txtDate]").val() +' ' + $("#ddlManStartTime").val();

                            objMan.ActualStartDate = $("#ddl_Man_StartDate").val();
                            objMan.Running_Hours = parseInt($("#ddl_Man_Running_Hours").val());
                            objMan.Delivery_Time = parseInt($("#txt_Man_DeliveryTime").val());
                            objMan.TimeWindow = parseInt($("#ddl_Man_TimeWindow").val());
                            objMan.Notes = $("#txt_Man_Notes").val();
                            objMan.Is_Multiday = $("#chk_Man_MultiDay").is(":checked");
                            objMan.Color = $("#Color_Man").val();
                            objMan.MultidayList =""
                            objMan.Drivers =""
                            objMan.Trucks  =""
                            objMan.CODrivers1 =""
                            objMan.CODrivers2 =""
                            objMan.Stops = StopsSelected
                            dialog_Man.dialog("close");
                            $.ajax({
                                type: "POST",
                                url: "man_api.aspx/SaveMan",
                                data: '{Man:' + JSON.stringify(objMan) + '}',
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                beforeSend: function () { $('#overlay').show(); },
                                success: function (response) {
                                    $('#overlay').hide();
                                    
                                    if (response.d.Err == "")
                                    {
                                        
                                        LoadManifest(response.d.MIN_ID,true);
                                    }
                                    else
                                    {
                                        alert(response.d.Err);
                                    }
                                    
                                },
                                failure: function (response) {
                                    $('#overlay').hide();
                                    alert(response.d);
                                },
                                error: function (response) {
                                    $('#overlay').hide();
                                    alert(response.d);
                                }
                            });
                            

                        }
                        dialog_Service_Time = $("#dialog_Service_Time").dialog({
                            autoOpen: false,
                            width: 550,
                            modal: true,
                            buttons: {
                                "Save": UpdateServiceTime,
                                Cancel: function () {
                                    dialog_Service_Time.dialog("close");
                                }
                            },
                            close: function () {
                            }
                        });
                        dialogtimeframe = $("#dialog_time_window").dialog({
                            autoOpen: false,
                            width: 550,
                            modal: true,
                            buttons: {
                                "Save": UpdateTimeWindow,
                                Cancel: function () {
                                    dialogtimeframe.dialog("close");
                                }
                            },
                            close: function () {
                            }
                        });
                        dialog_Route_Optimization = $("#dialog_Route_Optimization").dialog({
                            autoOpen: false,
                            width: 550,
                            modal: true,
                            buttons: {
                                "Save": Route_Optimization,
                                Cancel: function () {
                                    dialog_Route_Optimization.dialog("close");
                                }
                            },
                            close: function () {
                            }
                        });
                    }
                    $(function () {
                        iniDialog();
                    });
                    function imgChangeSeqOpen() {
                        if ($("#sortable").sortable('serialize').toString() != "[object Object]")
                        { dialog.dialog("open"); }
                    }

                    function changeServiceTime(Stop_ID,  ServiceTime) {
                        
                        $.ajax({
                            type: "POST",
                            url: "WebFunction.aspx/GetStopDelivery_Time",
                            data: '{MSO_ID: ' + Stop_ID + '}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            beforeSend: function () { $('#overlay').show(); },
                            success: function (response) {
                                $('#overlay').hide();
                                g_stop_id = Stop_ID;
                                OldServiceTime = response.d;
                                $("#txtServiceTime").val(response.d);
                                if (response.d == null)
                                {
                                    $("#chkServiceTime").prop('checked', true);
                                    $("#txtServiceTime").prop("disabled", true);
                                }
                                else
                                    {
                                $("#chkServiceTime").prop('checked', false);
                                $("#txtServiceTime").prop("disabled", false);
                            }
                                $("#chkServiceTime").click(function () {
                                    if ($(this).is(":checked")) {
                                        $("#txtServiceTime").val(null);
                                        $("#txtServiceTime").prop("disabled", true);
                                    } else {
                                        $("#txtServiceTime").val(OldServiceTime);
                                        $("#txtServiceTime").prop("disabled", false);
                                    }
                                });
                                dialog_Service_Time.dialog("open");
                            },
                            failure: function (response) {
                                $('#overlay').hide();
                                alert(response.d);
                            },
                            error: function (response) {
                                $('#overlay').hide();
                                alert(response.d);
                            }
                        });
                    }

                    
                    function Open_Route_Optimization()
                    {
                        if (confirm('This will override the current stop sequence, continue?')) { 
                            dialog_Route_Optimization.dialog("open");
                            
                        }
                    }

                    function Export_NV() {
                        if ($("#ddlManifest").val()==0)
                        {
                            return;
                        }
                        if (!confirm('Sure you want to export to nuVizz?')) {
                            return;

                        }
                        $.ajax({
                            type: "POST",
                            url: "man_api.aspx/Export_NV",
                            data: '{MIN_ID:' + $("#ddlManifest").val() + '}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            beforeSend: function () { $('#overlay').show(); },
                            success: function (response) {
                                $('#overlay').hide();
                                if (response.d!="")
                                {
                                    alert(response.d);
                                }
                                else {
                                    $('#imgExport').attr("src", "icon/nvdone.png");
                                }

                            },
                            failure: function (response) {
                                $('#overlay').hide();
                                alert(response.d);
                            },
                            error: function (response) {
                                $('#overlay').hide();
                                alert(response.d);
                            }
                        });


                    }
                    function DelStop() {
                        if (!confirm("Are you sure you want to delete the selected stop?")) {
                            return;
                        }
                                                
                        var checkedVals = $('.MS_ID_DEL:checkbox:checked').map(function () {
                            return this.value;
                        }).get();
                        if (checkedVals.join(",") == "")
                        {
                            alert("No Stop Selected");
                            return;
                        }
                        $.ajax({
                            type: "POST",
                            url: "man_api.aspx/DelStop",
                            data: '{MS_ID:"' + checkedVals.join(",") + '",MIN_ID:' + $("#ddlManifest").val() + '}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            beforeSend: function () { $('#overlay').show(); },
                            success: function (response) {
                                $('#overlay').hide();
                                LoadManifest($("#ddlManifest").val(),true);
                            },
                            failure: function (response) {
                                $('#overlay').hide();
                                alert(response.d);
                            },
                            error: function (response) {
                                $('#overlay').hide();
                                alert(response.d);
                            }
                        });


                    }
                    function Delete_Manifest() {
                        if ($("#ddlManifest").val()==0)
                        {
                            return;
                        }
                        if (!confirm("Are you sure you want to delete this manifest?")) {
                            return;
                        }
                        $.ajax({
                            type: "POST",
                            url: "man_api.aspx/DelMan",
                            data: '{MIN_ID:' + $("#ddlManifest").val() + '}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            beforeSend: function () { $('#overlay').show(); },
                            success: function (response) {
                                $('#overlay').hide();
                                if (response.d == "")
                                { LoadManifest(0, true); }
                                else
                                {
                                    $('#dialog_alert_msg').html(response.d);
                                    dialog_alert.dialog("open")
                                }
                                

                            },
                            failure: function (response) {
                                $('#overlay').hide();
                                alert(response.d);
                            },
                            error: function (response) {
                                $('#overlay').hide();
                                alert(response.d);
                            }
                        });


                    }
                    function InvertSeq() {
                        if ($("#ddlManifest").val() == 0) {
                            return;
                        }
                        if (!confirm("Are you sure you want to turn sequence upside down ?")) {
                            return;
                        }
                        $.ajax({
                            type: "POST",
                            url: "man_api.aspx/InvertSeq",
                            data: '{MIN_ID:' + $("#ddlManifest").val() + '}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            beforeSend: function () { $('#overlay').show(); },
                            success: function (response) {
                                $('#overlay').hide();
                                LoadManifest($("#ddlManifest").val(),true);

                            },
                            failure: function (response) {
                                $('#overlay').hide();
                                alert(response.d);
                            },
                            error: function (response) {
                                $('#overlay').hide();
                                alert(response.d);
                            }
                        });


                    }
                    function FillMan()
                    {
                        $.ajax({
                            type: "POST",
                            url: "man_api.aspx/GetMan",
                            data: '{Start_Date: "' + $("[id$=txtDate]").val() + '", Hub:"' + $("[id$=ddlHub]").val() + '", MIN_ID: ' + $("#ddlManifest").val() + '}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            beforeSend: function () { $('#overlay').show(); },
                            success: function (response) {
                                $("#ddl_Man_Driver").html(response.d.Drivers);
                                $("#ddl_Man_Truck").html(response.d.Trucks);
                                $("#ddl_Man_Co1").html(response.d.CODrivers1);
                                $("#ddl_Man_Co2").html(response.d.CODrivers2);
                                $("#ddl_Man_StartDate").html(response.d.MultidayList);

                                $("#ddlManStartTime").val(response.d.StartDate);
                                $("#ddl_Man_TimeWindow").val(response.d.TimeWindow);
                                $("#txt_Man_DeliveryTime").val(response.d.Delivery_Time).change();
                                $("#ddl_Man_Running_Hours").val(response.d.Running_Hours);
                                $("#Color_Man").val(response.d.Color.toString());
                                $("#txt_Man_Notes").val(response.d.Notes);
                                $("#chk_Man_MultiDay").prop('checked', response.d.Is_Multiday);
                                if ($("#chk_Man_MultiDay").is(":checked")) {
                                    $("#ddl_Man_StartDate").prop("disabled", false);
                                } else {
                                    $("#ddl_Man_StartDate").val("0");
                                    $("#ddl_Man_StartDate").prop("disabled", true);
                                }
                                $("#chk_Man_MultiDay").click(function () {
                                    if ($(this).is(":checked")) {
                                        $("#ddl_Man_StartDate").prop("disabled", false); 
                                    } else {
                                        $("#ddl_Man_StartDate").val("0");
                                        $("#ddl_Man_StartDate").prop("disabled", true);
                                    }
                                });
                                $('#overlay').hide();
                                dialog_Man.dialog("open");
                            },
                            failure: function (response) {
                                $('#overlay').hide();
                                alert(response.d);
                            },
                            error: function (response) {
                                $('#overlay').hide();
                                alert(response.d);
                            }
                        });
                    }
                    function OpenPrintDialog()
                    {
                        PrintDialog.dialog("open");
                    }
                    function LockMan()
                    {
                        $.ajax({
                            type: "POST",
                            url: "man_api.aspx/LockMan",
                            data: '{MIN_ID: ' + $("#ddlManifest").val() + '}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (response) {
                                $('#overlay').hide();
                                if (response.d) {
                                    $('#imgLock').attr("src", "icon/lock-close.png");
                                }
                                else {
                                    $('#imgLock').attr("src", "icon/lock-open.png");
                                }
                                
                            },
                            failure: function (response) {
                                $('#overlay').hide();
                                alert(response.d);
                            },
                            error: function (response) {
                                $('#overlay').hide();
                                alert(response.d);
                            }
                        });
                    }
                    function Route_Optimization() {

                        dialog_Route_Optimization.dialog("close");

                        $.ajax({
                            type: "POST",
                            url: "WebFunction.aspx/Route_Optimization",
                            data: '{MIN_ID: ' + $("#ddlManifest").val() + ', Route_Optimization_Type:' + $("#ddlOptimizationType").val() + ' }',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            beforeSend: function () { $('#overlay').show(); },
                            success: function (response) {
                                $('#overlay').hide();
                                LoadManifest($("#ddlManifest").val(),true);
                            },
                            failure: function (response) {
                                $('#overlay').hide();
                                alert(response.d);
                            },
                            error: function (response) {
                                $('#overlay').hide();
                                alert(response.d);
                            }
                        });

                    }
                    function UpdateServiceTime() {

                        var tim
                        dialog_Service_Time.dialog("close");
                        if ($("#txtServiceTime").val() == OldServiceTime)
                        { return; }
                        if ($("#txtServiceTime").val() == "")
                        { tim =null}
                        else
                        { tim=$("#txtServiceTime").val() }

                        $.ajax({
                            type: "POST",
                            url: "WebFunction.aspx/UpdateServiceTime",
                            data: '{MSO_ID: ' + g_stop_id + ', ServiceTime:' + tim + ' }',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            beforeSend: function () { $('#overlay').show(); },
                            success: function (response) {
                                $('#overlay').hide();
                                FillManData($("#ddlManifest").val(), false)

                            },
                            failure: function (response) {
                                $('#overlay').hide();
                                alert(response.d);
                            },
                            error: function (response) {
                                $('#overlay').hide();
                                alert(response.d);
                            }
                        });

                    }
                    function ToggleDayBreak(Stop_ID)
                    {
                        $.ajax({
                            type: "POST",
                            url: "man_api.aspx/ToggleDayBreak",
                            data: '{MS_ID:' + Stop_ID + ',MIN_ID:'+ $("#ddlManifest").val() +'}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            beforeSend: function () { $('#overlay').show(); },
                            success: function (response) {
                                $('#overlay').hide();
                                FillManData($("#ddlManifest").val(), false);
                            },
                            failure: function (response) {
                                $('#overlay').hide();
                                alert(response.d);
                            },
                            error: function (response) {
                                $('#overlay').hide();
                                alert(response.d);
                            }
                        });
                    }
                    function changeTimeFrame(Stop_ID ) {
                        $.ajax({
                            type: "POST",
                            url: "WebFunction.aspx/GetStopTimeFrame",
                            data: '{MS_ID: ' + Stop_ID + '}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            beforeSend: function () { $('#overlay').show(); },
                            success: function (response) {
                                $('#overlay').hide();
                                OldTimeWindow = response.d.TimeWindow;
                                OldStartTime = response.d.Start_Time;
                                OldCascade = response.d.User_Value_Cascade;
                                g_stop_id = response.d.MS_ID;
                                $("#ddlNewTimeFrame").val(response.d.TimeWindow);
                                $("#ddlNewStartTime").val(response.d.Start_Time);

                                $('#ddlNewStartTime').change(function () {
                                    if ($("#ddlNewStartTime").val()=="")
                                    {
                                        $("#chkCascade").attr("disabled", true);
                                        $("#chkCascade").prop('checked', false);
                                    }
                                    else
                                    {
                                        $("#chkCascade").removeAttr("disabled");
                                    }
                                });


                                $("#chkCascade").val(response.d.User_Value_Cascade);
                                $("#div_start_time").html(response.d.Start_Date);
                                dialogtimeframe.dialog("open");

                            },
                            failure: function (response) {
                                $('#overlay').hide();
                                alert(response.d);
                            },
                            error: function (response) {
                                $('#overlay').hide();
                                alert(response.d);
                            }
                        });
                    }
                    function UpdateTimeWindow() {
                        
                        dialogtimeframe.dialog("close");
                        if ($("#ddlNewTimeFrame").val() == OldTimeWindow && $("#ddlNewStartTime").val() == OldStartTime && $("#chkCascade").val()==OldCascade)
                        { return; }
                        var start_time, time_win, User_Value_Cascade;
                        if ($("#chkCascade").is(':disabled'))
                        { User_Value_Cascade = null}
                        else
                        { User_Value_Cascade = $("#chkCascade").is(':checked'); }
                        if ($("#ddlNewTimeFrame").val()=="")
                            {time_win =null}
                        else
                        { time_win = $("#ddlNewTimeFrame").val() }
                        if ($("#ddlNewStartTime").val() == "")
                        { start_time = null }
                        else
                        { start_time = '"' + $("#div_start_time").html() + ' ' + $("#ddlNewStartTime").val() +'"'; }

                        $.ajax({
                            type: "POST",
                            url: "WebFunction.aspx/UpdateTimeWindow",
                            data: '{MS_ID: ' + g_stop_id + ', TimeFrame:' + time_win + ',StartTime:' + start_time + ',User_Value_Cascade:' + User_Value_Cascade + ' }',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            beforeSend: function () { $('#overlay').show(); },
                            success: function (response) {
                                     $('#overlay').hide();
                                FillManData($("#ddlManifest").val(), false)
                           
                            },
                            failure: function (response) {
                                $('#overlay').hide();
                                alert(response.d);
                            },
                            error: function (response) {
                                $('#overlay').hide();
                                alert(response.d);
                            }
                        });
                        
                    }
                    function changeOrderAjax() {
                        
                        dialog.dialog("close");
                        $.ajax({
                            type: "POST",
                            url: "WebFunction.aspx/updateSort",
                            data: '{MIN_ID: ' + $("#ddlManifest").val() + ', SortOrder:"' + $("#sortable").sortable('serialize') + '" }',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            beforeSend: function () { $('#overlay').show(); },
                            success: function (response) {
                                $('#overlay').hide();
                                FillManData($("#ddlManifest").val(),true)
                                
                            },
                            failure: function (response) {
                                $('#overlay').hide();
                                alert(response.d);
                            },
                            error: function (response) {
                                $('#overlay').hide();
                                alert(response.d);
                            }
                        });
                        
                    }
                    $(function () {
                        
                        $("[id$=txtDate]").datepicker({
                            changeMonth: true,
                            changeYear: true,
                            beforeShowDay: function (date) {
                                var day = date.getDay();
                                return [day != 0, ''];
                            }
                        });
                        $("[id$=txtDateTo]").datepicker({
                            changeMonth: true,
                            changeYear: true,
                            beforeShowDay: function (date) {
                                var day = date.getDay();
                                return [day != 0, ''];
                            }
                        });
                        $('#ddlManifest').change(function () {
                            FillManData($(this).val(), false);
                            
                        });
                        

                    });
                    $(document).ready(function () {
                        $("[id$=txtDate]").change(function () {
                            LoadManifest(0,true);
                        });
                        $("[id$=ddlFilterAllStops]").change(function () {
                            FilterPinns();
                        });
                        $("[id$=ddlHub]").change(function () {
                            ChangeHub(); 
                        });
                        $("#txtFilterState").on('input propertychange paste', function () {
                            FilterPinns();
                        });
                        
                        $("#txtFilterZips").on('input propertychange paste', function () {
                            FilterPinns();
                        });
                        $("#txtOrderNo").on('input propertychange paste', function () {
                            FilterPinns();
                        });

                        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                        //    if ($("[id$=hfError]").val() != "") {
                        //        alert($("[id$=hfError]").val());
                        //        $("[id$=hfError]").val("");
                        //    }
                        //    if ($("[id$=hfLoadPins]").val() != "0") {
                        //        $("[id$=hfLoadPins]").val("1");
                        //        GetPins();
                                
                        //    }

                           
                            
                        //    iniDialog();
                        //});


                    });



        var selMarkers=[];
        var allMarkers=[];
        var HomeMarkers = [];
        var drawingManager ;
        var map, Poly;
                    
        var StopStr;
        var StopsSelected ;

                    function FillManData(MIN_ID, Load_Pins)
        {
                        
                        
                        if (Poly != null) {
                            Poly.setMap(null);
                        }
                        $("[id$=MIN_ID]").val(MIN_ID);
                        if (MIN_ID == 0)
                        {
                            $("#man_display_div").html("");
                            $("#man_Info_div").html("");
                            $("#dialog_form_Seq").html("");
                            if (Load_Pins) {
                                GetPins();
                            }
                            $('#imgSaveManifest').show();
                            $('#imgSeqUpSideDown').hide();
                            $('#imgLock').hide();
                            $('#imgSeq').hide();
                            $('#imgDel').hide();
                            $('#imgOptimize').hide();
                            $('#imgExport').hide();
                            $('#imgPath').hide();
                            return;
                        }
                        $.ajax({
                            type: "POST",
                            url: "man_api.aspx/GetManDisplayData",
                            data: '{MIN_ID:' + MIN_ID  + '}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            beforeSend: function () { $('#overlay').show(); },
                            success: function (response) {

                                $('#overlay').hide();
                                if (response.d.Err!="")
                                {
                                    $('#dialog_alert_msg').html(response.d.Err);
                                    dialog_alert.dialog("open")
                                }
                                if (response.d.nv_Done)
                                {
                                    $('#imgExport').attr("src", "icon/nvdone.png");
                                }
                                else
                                {
                                    $('#imgExport').attr("src", "icon/nv.png");
                                }
                                if (response.d.Man_Locked)
                                {
                                    $('#imgLock').attr("src", "icon/lock-close.png");
                                }
                                else {
                                    $('#imgLock').attr("src", "icon/lock-open.png");
                                }
                                $('#imgPath').show();
                                if (response.d.NotEditable == true)
                                {
                                    $('#imgSaveManifest').hide();
                                    $('#imgSeqUpSideDown').hide();
                                    $('#imgLock').hide();
                                    $('#imgSeq').hide();
                                    $('#imgDel').hide();
                                    $('#imgOptimize').hide();
                                    $('#imgExport').hide();
                                }
                                else
                                {
                                    $('#imgSaveManifest').show();
                                    $('#imgSeqUpSideDown').show();
                                    $('#imgLock').show();
                                    $('#imgSeq').show();
                                    $('#imgDel').show();
                                    $('#imgOptimize').show();
                                    $('#imgExport').show();
                                }
                                $("#man_display_div").html(response.d.Man_Str);
                                $("#man_Info_div").html(response.d.Man_info);
                                $("#dialog_form_Seq").html(response.d.Seq_Str);
                                $("#ddlAllStops").html(response.d.Insert_Str);
                                //$("#sortable" + $("[id$=hfdialog_form_Seq]").val()).sortable();
                                //$("#sortable" + $("[id$=hfdialog_form_Seq]").val()).disableSelection();
                                $("#sortable").sortable();
                                $("#sortable").disableSelection();
                                if (Load_Pins)
                                {
                                    GetPins();
                                }
                               
                                
                            },
                            failure: function (response) {
                                $('#overlay').hide();
                                alert(response.d);
                            },
                            error: function (response) {
                                $('#overlay').hide();
                                alert(response.d);
                            }
                        });
                    }
                    function LoadManifest(Man_ToSelect, Load_Pins) {
            $.ajax({
                type: "POST",
                url: "man_api.aspx/FillManList",
                data: '{Date_: "' + $("[id$=txtDate]").val() + '", Hub:"' + $("[id$=ddlHub]").val() + '", Man_ToSelect: ' + Man_ToSelect + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () { $('#overlay').show(); },
                success: function (response) {
                    $('#overlay').hide();
                    $("#ddlManifest").html(response.d);
                    FillManData(Man_ToSelect,Load_Pins);
                    
                },
                failure: function (response) {
                    $('#overlay').hide();
                    alert(response.d);
                },
                error: function (response) {
                    $('#overlay').hide();
                    alert(response.d);
                }
            });

        }
        function GoTo(gt_lat,gt_lng)
        {
            var center = new google.maps.LatLng(gt_lat,gt_lng);
            map.panTo(center);
            map.setZoom(15);
        }
        function Insert_Stop_dialog(vM_ID) {
            StopStr = vM_ID;
            dialog_Insert_Stop.dialog("open");
        }
        function sel_manifest(vMIN_ID)
        {
            if ($("#ddlManifest").val() != vMIN_ID)
                {
                $("#ddlManifest option").each(function (i) {
                    if ($(this).val()==vMIN_ID)
                    {
                        $("#ddlManifest").val(vMIN_ID);
                        FillManData(vMIN_ID, false)
                    }
                });
            }
        }
        function Ignore_Address_Check(Marker_ID)
        {
            if (!confirm("Are you sure you want to Ignore Address Check?"))
            {
                return;
            }
            $.ajax({
                type: "POST",
                url: "WebFunction.aspx/Ignore_Address_Check",
                data: '{M_ID:"' + Marker_ID + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () { $('#overlay').show(); },
                success: function (response) {
                    LoadManifest($("#ddlManifest").val(), true)
                     $('#overlay').hide();
                },
                failure: function (response) {
                    $('#overlay').hide();
                    alert(response.d);
                },
                error: function (response) {
                    $('#overlay').hide();
                    alert(response.d);
                }
            });
        }
        function Add_Missing_Stop(AM_Order_ID, AM_MS_ID, AM_Is_Pickup)
        {
            $.ajax({
                type: "POST",
                url: "WebFunction.aspx/Add_Missing_Stop",
                data: '{AM_Order_ID: ' + AM_Order_ID + ', AM_MS_ID:' + AM_MS_ID + ',AM_Is_Pickup:' + AM_Is_Pickup + ' }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () { $('#overlay').show(); },
                success: function (response) {
                    $('#overlay').hide();
                    FillManData($("#ddlManifest").val(), true)
                   
                },
                failure: function (response) {
                    $('#overlay').hide();
                    alert(response.d);
                },
                error: function (response) {
                    $('#overlay').hide();
                    alert(response.d);
                }
            });
        }
        function ChangeHub() {
            for (var i = 0; i < HomeMarkers.length; i++) {
                if ($("[id$=ddlHub]").val() == HomeMarkers[i].M_ID) {
                    var center = new google.maps.LatLng(HomeMarkers[i].lat, HomeMarkers[i].lng);
                    map.panTo(center);
                    map.setZoom(7);
                    LoadManifest(0,true);
                }
            }
        }
        function FilterPinns()
        {
            var ShowPin = true;
            var States_ = $("#txtFilterState").val().split(',')
            var Zips = $("#txtFilterZips").val().split(',')
            var Orders = $("#txtOrderNo").val().split(',')
            for (var i = 0; i < allMarkers.length; i++) {
                ShowPin = false;
                if (allMarkers[i].pin_type != 0) {
                    if ($("[id$=ddlFilterAllStops]").val() == "1") {
                        ShowPin = true;
                    }
                    else if ($("[id$=ddlFilterAllStops]").val() == "2" && allMarkers[i].MIN_ID == 0) {
                        ShowPin = true;
                    }
                    else if ($("[id$=ddlFilterAllStops]").val() == "3" && allMarkers[i].MIN_ID == $("#ddlManifest").val()) {
                        ShowPin = true;
                    }
                    else if ($("[id$=ddlFilterAllStops]").val() == "4" && (allMarkers[i].MIN_ID == $("#ddlManifest").val() || allMarkers[i].MIN_ID == 0)) {
                        ShowPin = true;
                    }
                    if ($("#txtFilterState").val().trim() != '')
                        {
                    for (j=0; j<States_.length;j++)
                    {
                        if (allMarkers[i].State_.trim().toLowerCase()==States_[j].trim().toLowerCase())
                        {
                            ShowPin = true;
                            break;
                        }
                        else
                        {
                            ShowPin = false;
                        }
                    }
                    }
                    if ($("#txtFilterZips").val().trim() != '')
                        {
                    for (j=0; j<Zips.length;j++)
                    {
                        if (allMarkers[i].Zip.trim().toLowerCase()==Zips[j].trim().toLowerCase())
                        {
                            ShowPin = true;
                            break;
                        }
                        else
                        {
                            ShowPin = false;
                        }
                    }
                    }
                    if ($("#txtOrderNo").val().trim() != '')
                    {
                        for (j = 0; j < Orders.length; j++)
                    {
                            if (allMarkers[i].M_ID.trim().toLowerCase().indexOf(Orders[j].trim()) > 0)
                        {
                            ShowPin = true;
                            break;
                        }
                        else
                        {
                            ShowPin = false;
                        }
                    }
                        
                    }
                }
                else {
                    ShowPin = true;
                }
                if (ShowPin == true)
                    allMarkers[i].setMap(map);
                else
                    allMarkers[i].setMap(null);
            }
        }
        function DisplayList()
        {
            var txthtml="";
            var tbl = "";
            var OrderCount_pic = 0;
            var ItemCount_pic = 0;
            var Wt_pic = 0;
            var Cubes_pic = 0;
            var OrderCount_del = 0;
            var ItemCount_del = 0;
            var Wt_del = 0;
            var Cubes_del = 0;
            StopsSelected = "";
            for (i = 0; i<selMarkers.length;i++)
            {
                txthtml = txthtml + "<li class='ui-widget-content'>" 
                txthtml = txthtml +"<span class='fresh_del'><img onclick=\"DelMarker('"+ selMarkers[i].M_ID +"');\" src='icon/cross.gif' style='cursor: pointer;' /></span>";
                txthtml = txthtml + "<span class='fresh_txt' onclick='GoTo("+ selMarkers[i].position.lat() +","+ selMarkers[i].position.lng() +")'>";
                txthtml = txthtml + selMarkers[i].getTitle().replace('\n','<br/>') +"</span>";
                
                txthtml = txthtml +"</li>";
                OrderCount_pic = OrderCount_pic + selMarkers[i].OrderCount_pic;
                ItemCount_pic = ItemCount_pic + selMarkers[i].ItemCount_pic;
                Wt_pic = Wt_pic + selMarkers[i].Wt_pic;
                Cubes_pic = Cubes_pic + selMarkers[i].Cubes_pic;
                OrderCount_del = OrderCount_del + selMarkers[i].OrderCount_del;
                ItemCount_del = ItemCount_del + selMarkers[i].ItemCount_del;
                Wt_del = Wt_del + selMarkers[i].Wt_del;
                Cubes_del = Cubes_del + selMarkers[i].Cubes_del;
                
                StopsSelected = StopsSelected + selMarkers[i].M_ID + ":";
            }
            if (selMarkers.length > 0)
            {
                tbl = "<table style='width:100%;font-weight:none;font-size:8pt;border-spacing: 0;border-collapse:collapse;'>";
                tbl = tbl + "<tr><td style='border: solid 1px #000000;text-align: center;'>Stops: " + selMarkers.length + "</td><td style='text-align: center;border: solid 1px #000000;'>Orders</td><td style='text-align: center;border: solid 1px #000000;'>Items</td><td style='text-align: center;border: solid 1px #000000;'>Wt</td><td style='text-align: center;border: solid 1px #000000;'>CuFts</td><tr>";
                tbl = tbl + "<tr><td style='border: solid 1px #000000;'>Pickup</td><td style='text-align: center;border: solid 1px #000000;'>" + OrderCount_pic + "</td><td style='text-align: center;border: solid 1px #000000;'>" + ItemCount_pic + "</td><td style='text-align: center;border: solid 1px #000000;'>" + Wt_pic + "</td><td style='text-align: center;border: solid 1px #000000;'>" + Cubes_pic + "</td><tr>";
                tbl = tbl + "<tr><td style='border: solid 1px #000000;'>Delivery</td><td style='text-align: center;border: solid 1px #000000;'>" + OrderCount_del + "</td><td style='text-align: center;border: solid 1px #000000;'>" + ItemCount_del + "</td><td style='text-align: center;border: solid 1px #000000;'>" + Wt_del + "</td><td style='text-align: center;border: solid 1px #000000;'>" + Cubes_del + "</td><tr>";
                tbl = tbl + "<tr><td style='border: solid 1px #000000;'>Total</td><td style='text-align: center;border: solid 1px #000000;'>" + (OrderCount_del + OrderCount_pic) + "</td><td style='text-align: center;border: solid 1px #000000;'>" + (ItemCount_del + ItemCount_pic) + "</td><td style='text-align: center;border: solid 1px #000000;'>" + (Wt_del + Wt_pic) + "</td><td style='text-align: center;border: solid 1px #000000;'>" + (Cubes_del + Cubes_pic) + "</td><tr>";
                tbl = tbl + "</table>";
                txthtml = "<li class='ui-widget-content'>" + tbl + "</li>" + txthtml;
                //txthtml = txthtml;
            }
            else
            { txthtml =""}
            
            $("[id$=hfStops]").val(StopsSelected);
            $("#selectable").html(txthtml);
         
        }
        function PushMarker(l_marker){
            var found
            found=false;
            for (i = 0; i<selMarkers.length;i++){
                if (l_marker.M_ID==selMarkers[i].M_ID){
                    l_marker.setIcon(l_marker.My_Icon);
                    selMarkers.splice(i,1);
                    found=true;
                    break;
                }
            }
            if (!found){
                l_marker.setIcon(l_marker.Sel_Icon);
                selMarkers.push(l_marker);
            }
            DisplayList();
        }
        function DelMarker(vM_ID){
            for (i = 0; i<selMarkers.length;i++){
                if (vM_ID==selMarkers[i].M_ID){
                    selMarkers[i].setIcon(selMarkers[i].My_Icon);
                    selMarkers.splice(i,1);
                    break;
                }
            }
            DisplayList();
        }
    </script>
<script type="text/javascript">
    
    window.onload = function (){loadMap();}
    
    function loadMap() {

        $.ajax({
            type: "POST",
            url: "WebFunction.aspx/GetHomes",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                    LoadHome(response.d)
            },
            failure: function (response) {
                alert(response.d);
            },
            error: function (response) {
                alert(response.d);
            }
        });
    }
    function LoadHome(HomePins)
    {
        var uluru = { lat: parseFloat(HomePins[0].lat), lng: parseFloat(HomePins[0].lng) };
        drawingManager = new google.maps.drawing.DrawingManager();
        map = new google.maps.Map(document.getElementById('dvMap'), {
            zoom: 7,
            center: uluru
        });
        for (i = 0; i < HomePins.length; i++) {
            var data = HomePins[i]
            var myLatlng = new google.maps.LatLng(data.lat, data.lng);
            var marker = new google.maps.Marker({
                optimized: false,
                position: myLatlng,
                map: map,
                title: data.title,
                label: data.label,
                M_ID: data.M_ID,
                Icon: data.icon,
                My_Icon: data.icon,
                Sel_Icon: data.Sel_Icon,
                clickable1: data.clickable,
                MIN_ID: data.MIN_ID,
                pin_type: data.pin_type,
                Zip: data.Zip,
                State_: data.State_
            });
            HomeMarkers.push(data);

        }//End For
        drawingManager.setOptions({
            //drawingMode : google.maps.drawing.OverlayType.RECTANGLE,
            drawingControl: true,
            drawingControlOptions: {
                position: google.maps.ControlPosition.TOP_CENTER,
                drawingModes: [google.maps.drawing.OverlayType.RECTANGLE, google.maps.drawing.OverlayType.CIRCLE]
            },
            rectangleOptions: {
                strokeColor: '#a3ccff',
                strokeWeight: 1,
                fillColor: '#b8b8b8',
                fillOpacity: 0.6
                //,
                //editable: true,
                //draggable: true
            }
        });
        // Loading the drawing Tool in the Map.
        drawingManager.setMap(map);
        google.maps.event.addListener(drawingManager, 'overlaycomplete', function (e) {
            var bounds = new google.maps.LatLngBounds(e.overlay.getBounds().getSouthWest(), e.overlay.getBounds().getNorthEast());
            for (a = 0; a < allMarkers.length; a++) {
                if (allMarkers[a].clickable1 && bounds.contains(new google.maps.LatLng(allMarkers[a].position.lat(), allMarkers[a].position.lng()))) {
                    if (allMarkers[a].getMap() != null) {
                        PushMarker(allMarkers[a]);
                    }

                }
            }
            e.overlay.setMap(null);
        });
    }
    function GetPins()
    {
        $.ajax({
            type: "POST",
            url: "WebFunction.aspx/GetMarkers",
            data: '{Date_: "' + $("[id$=txtDate]").val() + '",DateTo:"' + $("[id$=txtDateTo]").val() + '", Hub:"' + $("[id$=ddlHub]").val() + '" }',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {$('#overlay').show();},
            success: function (response) {
                $('#overlay').hide();
                for (var i = 0; i < allMarkers.length; i++) {
                    allMarkers[i].setMap(null);
                }
                selMarkers = [];
                allMarkers = [];
                DisplayList();
                loadPins(response.d);
                
               
            },
            failure: function (response) {
                $('#overlay').hide();
                alert(response.d);
            },
            error: function (response) {
                $('#overlay').hide();
                alert(response.responseJSON.Message);
            }
        });
    }
    function pinSymbol(color) {
        return {
            //path: 'M 0,0 C -2,-20 -10,-22 -10,-30 A 10,10 0 1,1 10,-30 C 10,-22 2,-20 0,0 z M -2,-30 a 2,2 0 1,1 4,0 2,2 0 1,1 -4,0z', with dot
            //path: 'M 0,0 C -2,-20 -10,-22 -10,-30 A 10,10 0 1,1 10,-30 C 10,-22 2,-20 0,0 z', without dot
            path: 'M 0,0 C -2,-20 -10,-22 -10,-30 A 10,10 0 1,1 10,-30 C 10,-22 2,-20 0,0 z',
            fillColor: color,
            fillOpacity: 1,
            labelOrigin: new google.maps.Point(0, -28),
            strokeColor: '#000',
            strokeWeight: 1,
            scale:1,
        };
    }
    function loadPins(markers)
    {
        
        if (markers==null)
        {
            return;
        }
var infoWindow = new google.maps.InfoWindow();

for (i = 0; i < markers.length; i++) {
    var data = markers[i]
    var myLatlng = new google.maps.LatLng(data.lat, data.lng);
    var marker = new google.maps.Marker({
        optimized:false,
        position: myLatlng,
        map: map,
        title: data.title,
        label: data.label,
        M_ID:data.M_ID,
        Icon: data.icon,//pinSymbol('#fb7468')
        My_Icon:data.icon,
        Sel_Icon:data.Sel_Icon,
        clickable1:data.clickable,
        OrderCount_pic: data.OrderCount_pic,
        ItemCount_pic: data.ItemCount_pic,
        Wt_pic: data.Wt_pic,
        Cubes_pic: data.Cubes_pic,
        OrderCount_del: data.OrderCount_del,
        ItemCount_del: data.ItemCount_del,
        Wt_del: data.Wt_del,
        Cubes_del: data.Cubes_del,
        MIN_ID: data.MIN_ID,
        pin_type: data.pin_type,
        Zip: data.Zip,
        State_:data.State_ 
    });
    
    allMarkers.push(marker);
    (function (marker) {
        google.maps.event.addListener(marker, "rightclick", function (e) {
            ///////////////
            $.ajax({
                type: "POST",
                url: "WebFunction.aspx/GetInfo",
                data: '{M_ID: "' + marker.M_ID + '",MIN_ID:' + marker.MIN_ID + ',SEL_MIN_ID:' + $("#ddlManifest").val() + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                
                success: function (response) {
                    infoWindow.setContent(response.d);
                    infoWindow.open(map, marker);
                },
                failure: function (response) {
                    alert(response.d);
                }
            });
        });
        google.maps.event.addListener(marker, "click", function (e) {
            if (marker.clickable1) {
                PushMarker(marker);
            }
            else
            {
                $.ajax({
                    type: "POST",
                    url: "WebFunction.aspx/GetInfo",
                    data: '{M_ID: "' + marker.M_ID + '",MIN_ID:' + marker.MIN_ID + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",

                    success: function (response) {
                        infoWindow.setContent(response.d);
                        infoWindow.open(map, marker);
                    },
                    failure: function (response) {
                        alert(response.d);
                    }
                });
            }
        });
    })(marker);

}//End For
if (Poly != null) {
    Poly.setMap(null);
}
FilterPinns();
    }
    function DrawPoly()
    {
        if (Poly != null) {
            Poly.setMap(null);
            Poly = null;
            return;
        }
        if ($("#ddlManifest").val() == 0 || $("#ddlManifest").val() == null)
        {
            return;
        }
        
            $.ajax({
                type: "POST",
                url: "WebFunction.aspx/GetPoly",
                data: '{MIN_ID:' + $("#ddlManifest").val()+ '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () { $('#overlay').show(); },
                success: function (response) {
                    var paths
                    var all_paths=[]
                    for (i = 0; i <= response.d.PolyList.length - 1; i++)
                    {
                        paths = google.maps.geometry.encoding.decodePath(response.d.PolyList[i])
                        for (j=0 ; j<=paths.length-1;j++)
                        {
                            all_paths.push(paths[j]);
                        }
                    }
                    var myPoly = new google.maps.Polyline({
                        path: all_paths,
                        strokeColor: response.d.Color ,
                        strokeOpacity: 1.0,
                        strokeWeight: response.d.LineWidth
                    });
                    Poly = myPoly;
                    myPoly.setMap(map);
                    $('#overlay').hide();
                },
                failure: function (response) {
                    $('#overlay').hide();
                    alert(response.d);
                }
            });
    }
</script>
    <div id="dvMap">
    </div>
        <div id="pin_tools">
            <div id="tools_minifest">
                <img id="imgSaveManifest" src="icon/save.png" onclick="FillMan();" style="cursor: pointer;" title="Save Manifest"/>
                <img id="imgPrint" src="icon/print.png"  onclick="OpenPrintDialog();" title="Open Print Dialog" style="cursor: pointer;"/>
                <img id="imgLock" src="icon/lock-close.png"  title="Lock This Manifest" onclick="LockMan();" style="cursor: pointer;"/>
                <img id="imgSeqUpSideDown" src="icon/reorder.png"  title="Turn sequence upside down" onclick="InvertSeq();" style="cursor: pointer;"/>
                <img id="imgSeq" src="icon/order.png" title="Change the Stop Order" onclick="imgChangeSeqOpen();" style="cursor: pointer;"/>
                <div class="dropdown">
                    <img id="imgDel" src="icon/del.png" style="cursor: pointer;" class="dropbtn" />
                    <div class="dropdown-content">
                        <a href="javascript:Delete_Manifest();">Delete Manifest</a>
                        <a href="javascript:DelStop();">Delete Stops</a>
                    </div>
                </div>
                
                <img id="imgOptimize" src="icon/Optimize.png" title="Optimize Manifest" onclick="Open_Route_Optimization();" style="cursor: pointer;"/>
                <img id="imgExport" src="icon/nv.png" title="Export to nuVizz" onclick="Export_NV();" style="cursor: pointer;"/>
                <img id="imgPath" src="icon/path.png" title="Show Path" onclick="DrawPoly();" style="cursor: pointer;"/>
                <select id="ddlManifest" style="width: 100%" title="Select an existing manifest or create a new">
                 </select>
                
            </div>
            <div id="man_display" style="text-align:left; font-size:10pt;overflow-y: scroll ;">
                <div id="fresh_sel">
                    <ul id="selectable">
                    </ul>
                </div>
                <div id="man_Info_div">

                </div>
                <div id="man_display_div">

                </div>
            </div>
                <asp:HiddenField ID="MIN_ID" runat="server" Value="" />

            

        </div>
<div id="overlay"><div id="loading-img"></div></div>   
<div id="dialog_form_Seq" title="Change Stop Order">

</div>
<div id="dialog_time_window" title="Change Stop Timeframe">
    <table>
        <tr>
            <td>
                Time Window:
            </td>
            <td colspan="2" style="width:300px">
                <select id="ddlNewTimeFrame" style="width:300px">
                    <option value="">Inherit from Manifest</option>
                    <option value="30">30 Mins</option>
                    <option value="60">1 Hour</option>
                    <option value="90">1 Hour 30 Mins</option>
                    <option value="120">2 Hours</option>
                    <option value="150">2 Hours 30 Mins</option>
                    <option value="180">3 Hours</option>
                    <option value="210">3 Hours 30 Mins</option>
                    <option value="240">4 Hours</option>
                    <option value="270">4 Hours 30 Mins</option>
                    <option value="300">5 Hours</option>
                    <option value="330">5 Hours 30 Mins</option>
                    <option value="360">6 Hours</option>
                    <option value="390">6 Hours 30 Mins</option>
                    <option value="420">7 hours</option>
                </select>
            </td>
        </tr>
        <tr>
            <td>
                Start Time:
            </td>
            <td style="width:200px">
                <select id="ddlNewStartTime" style="width:100%">
                    <option value="">Auto</option>
                    <option value="00:00">12:00 AM</option>
                    <option value="00:30">12:30 AM</option>
                    <option value="01:00">01:00 AM</option>
                    <option value="01:30">01:30 AM</option>
                    <option value="02:00">02:00 AM</option>
                    <option value="02:30">02:30 AM</option>
                    <option value="03:00">03:00 AM</option>
                    <option value="03:30">03:30 AM</option>
                    <option value="04:00">04:00 AM</option>
                    <option value="04:30">04:30 AM</option>
                    <option value="05:00">05:00 AM</option>
                    <option value="05:30">05:30 AM</option>
                    <option value="06:00">06:00 AM</option>
                    <option value="06:30">06:30 AM</option>
                    <option value="07:00">07:00 AM</option>
                    <option value="07:30">07:30 AM</option>
                    <option value="08:00">08:00 AM</option>
                    <option value="08:30">08:30 AM</option>
                    <option value="09:00">09:00 AM</option>
                    <option value="09:30">09:30 AM</option>
                    <option value="10:00">10:00 AM</option>
                    <option value="10:30">10:30 AM</option>
                    <option value="11:00">11:00 AM</option>
                    <option value="11:30">11:30 AM</option>
                    <option value="12:00">12:00 PM</option>
                    <option value="12:30">12:30 PM</option>
                    <option value="13:00">01:00 PM</option>
                    <option value="13:30">01:30 PM</option>
                    <option value="14:00">02:00 PM</option>
                    <option value="14:30">02:30 PM</option>
                    <option value="15:00">03:00 PM</option>
                    <option value="15:30">03:30 PM</option>
                    <option value="16:00">04:00 PM</option>
                    <option value="16:30">04:30 PM</option>
                    <option value="17:00">05:00 PM</option>
                    <option value="17:30">05:30 PM</option>
                    <option value="18:00">06:00 PM</option>
                    <option value="18:30">06:30 PM</option>
                    <option value="19:00">07:00 PM</option>
                    <option value="19:30">07:30 PM</option>
                    <option value="20:00">08:00 PM</option>
                    <option value="20:30">08:30 PM</option>
                    <option value="21:00">09:00 PM</option>
                    <option value="21:30">09:30 PM</option>
                    <option value="22:00">10:00 PM</option>
                    <option value="22:30">10:30 PM</option>
                    <option value="23:00">11:00 PM</option>
                    <option value="23:30">11:30 PM</option>
                </select>
                
            </td>
            <td style="width:100px;">
                <div id="div_start_time"></div>
            </td>
        </tr>
        <tr>
            <td></td>
            <td >
                <input id="chkCascade" type="checkbox"  /> <label for="chkCascade">Cascade</label></td>
        </tr>
    </table>
    
</div>

<div id="dialog_Service_Time" title="Change Stop Service Time">
    <table>
        <tr>
            <td>
                
            </td>
            <td>
                <input id="chkServiceTime" type="checkbox"  /> <label for="chkServiceTime">Inherit from Manifest</label>
            </td>
        </tr>
        <tr>
            <td>
                Service Time:
            </td>
            <td>
                <input id="txtServiceTime" type="text" style="width:300px" />
            </td>
        </tr>
    </table>
</div>
<div id="dialog_Insert_Stop" title="Insert Stop">
    <table style="width:100%">
        <tr>
            <td  style="width:70px">
                <select id="ddlBeforeAfter" style="width:100%">
                    <option value="1">Before</option>
                    <option value="2">After</option>
                    </select>
            </td>
            <td>
                <select id="ddlAllStops" style="width:100%">
                </select>
            </td>
        </tr>
    </table>
</div>
<div id="dialog_Route_Optimization" title="Optimize Route">
    <p style="color:#ff0000;">This will override the current stop sequence</p>
    <table>
        <tr>
            <td>
                Optimization Type:
            </td>
            <td>
                <select id="ddlOptimizationType">
                    <option value="1">Google Route Optimization (Max 23 Stops)</option>
                    <option value="2">Shortest Distance (Unlimited Stops)</option>
                </select>
            </td>
        </tr>
    </table>
</div>
<div id="dialog_Print" title="Print/Export Options">
    <ul style="list-style:none">
        <li><asp:LinkButton ID="lbTaskSheet" runat="server">Print Manifest Sheet With Tickets - Whole Day</asp:LinkButton></li>
        <li><asp:LinkButton ID="lbTaskSheetWithoutTickets" runat="server">Print Manifest Sheet Without Tickets - Whole Day</asp:LinkButton></li>
        <li><asp:LinkButton ID="lbTaskSheetXL" runat="server">Export Manifest Sheet XL - Whole Day</asp:LinkButton></li>
        <li><asp:LinkButton ID="lbPullSheet" runat="server">Print Pull Sheet - Whole Day</asp:LinkButton></li>
        <li><hr /></li>
        <li><asp:LinkButton ID="lbTaskSheetThis" runat="server">Print Manifest Sheet With Tickets - This Manifest</asp:LinkButton></li>
        <li><asp:LinkButton ID="lbTaskSheetWithoutTicketsThis" runat="server">Print Manifest Sheet Without Tickets - This Manifest</asp:LinkButton></li>
        <li><asp:LinkButton ID="lbPullSheetThis" runat="server">Print Pull Sheet - This Manifest</asp:LinkButton></li>

    </ul>
</div>
      <div id="dialog_Man" title="Save Manifest">
                        <table style="border-spacing: 5px; width: 100%">
                            <tr>
                                <td>Driver:</td>
                                <td>
                                    <select id="ddl_Man_Driver" style="width: 300px"></select>
                                </td>
                                <td>Truck:</td>
                                <td>
                                    <select id="ddl_Man_Truck" style="width: 300px"></select>
                                </td>
                            </tr>
                            <tr>
                                <td>Co Driver 1:</td>
                                <td>
                                    <select id="ddl_Man_Co1" style="width: 300px"></select>
                                </td>
                                <td>Co Driver 2:</td>
                                <td>
                                    <select id="ddl_Man_Co2" style="width: 300px"></select>
                                </td>
                            </tr>
                            <tr>
                                <td>Start Time:</td>
                                <td>
                                    <select id="ddlManStartTime" style="width: 300px">
                                        <option value="12:00">12:00 AM</option>
                                        <option value="12:30">12:30 AM</option>
                                        <option value="01:00">01:00 AM</option>
                                        <option value="01:30">01:30 AM</option>
                                        <option value="02:00">02:00 AM</option>
                                        <option value="02:30">02:30 AM</option>
                                        <option value="03:00">03:00 AM</option>
                                        <option value="03:30">03:30 AM</option>
                                        <option value="04:00">04:00 AM</option>
                                        <option value="04:30">04:30 AM</option>
                                        <option value="05:00">05:00 AM</option>
                                        <option value="05:30">05:30 AM</option>
                                        <option value="06:00">06:00 AM</option>
                                        <option value="06:30">06:30 AM</option>
                                        <option value="07:00">07:00 AM</option>
                                        <option value="07:30">07:30 AM</option>
                                        <option value="08:00">08:00 AM</option>
                                        <option value="08:30">08:30 AM</option>
                                        <option value="09:00">09:00 AM</option>
                                        <option value="09:30">09:30 AM</option>
                                        <option value="10:00">10:00 AM</option>
                                        <option value="10:30">10:30 AM</option>
                                        <option value="11:00">11:00 AM</option>
                                        <option value="11:30">11:30 AM</option>
                                        <option value="12:00">12:00 PM</option>
                                        <option value="12:30">12:30 PM</option>
                                        <option value="01:00">01:00 PM</option>
                                        <option value="01:30">01:30 PM</option>
                                        <option value="02:00">02:00 PM</option>
                                        <option value="02:30">02:30 PM</option>
                                        <option value="03:00">03:00 PM</option>
                                        <option value="03:30">03:30 PM</option>
                                        <option value="04:00">04:00 PM</option>
                                        <option value="04:30">04:30 PM</option>
                                        <option value="05:00">05:00 PM</option>
                                        <option value="05:30">05:30 PM</option>
                                        <option value="06:00">06:00 PM</option>
                                        <option value="06:30">06:30 PM</option>
                                        <option value="07:00">07:00 PM</option>
                                        <option value="07:30">07:30 PM</option>
                                        <option value="08:00">08:00 PM</option>
                                        <option value="08:30">08:30 PM</option>
                                        <option value="09:00">09:00 PM</option>
                                        <option value="09:30">09:30 PM</option>
                                        <option value="10:00">10:00 PM</option>
                                        <option value="10:30">10:30 PM</option>
                                        <option value="11:00">11:00 PM</option>
                                        <option value="11:30">11:30 PM</option>
                                    </select>
                                </td>
                                <td>Delivery Time:</td>
                                <td>
                                    <input id="txt_Man_DeliveryTime" type="number" min="15" max="240" value="15" placeholder="Approx. Delivery Time" style="width: 297px" />
                                </td>
                            </tr>
                            <tr>
                                <td>Time Window:</td>
                                <td>
                                    <select id="ddl_Man_TimeWindow" style="width: 300px">
                                        <option value="30">30 Mins</option>
                                        <option value="60">1 Hour</option>
                                        <option value="90">1 Hour 30 Mins</option>
                                        <option value="120">2 Hours</option>
                                        <option value="150">2 Hours 30 Mins</option>
                                        <option value="180">3 Hours</option>
                                        <option value="210">3 Hours 30 Mins</option>
                                        <option value="240">4 Hours</option>
                                        <option value="270">4 Hours 30 Mins</option>
                                        <option value="300">5 Hours</option>
                                        <option value="330">5 Hours 30 Mins</option>
                                        <option value="360">6 Hours</option>
                                        <option value="390">6 Hours 30 Mins</option>
                                        <option value="420">7 hours</option>
                                    </select>
                                </td>
                                <td>Running Hours:</td>
                                <td>
                                    <select id="ddl_Man_Running_Hours" style="width: 300px">
                                        <option value="4">4 Hours</option>
                                        <option value="5">5 Hours</option>
                                        <option value="6">6 Hours</option>
                                        <option value="7">7 Hours</option>
                                        <option value="8">8 Hours</option>
                                        <option value="9">9 Hours</option>
                                        <option value="10">10 Hours</option>
                                        <option value="11">11 Hours</option>
                                        <option value="12">12 Hours</option>
                                        <option value="13">13 Hours</option>
                                        <option value="14">14 Hours</option>
                                        <option value="15">15 Hours</option>
                                        <option value="16">16 Hours</option>
                                        <option value="17">17 Hours</option>
                                        <option value="18">18 Hours</option>
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Color:
                                </td>
                                <td>
                                     <input type="color" id="Color_Man" name="Color_Man"/>
                                </td>
                                <td>
                                    <label for="chk_Man_MultiDay"><input id="chk_Man_MultiDay" type="checkbox"  /> Is Multiday</label> 
                                </td>
                                <td>
                                    
                                    <select id="ddl_Man_StartDate" style="width: 300px"></select>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Notes:
                                </td>
                                <td colspan="3">
                                    <textarea id="txt_Man_Notes" style="width:720px;height:70px;resize: none;" ></textarea>
                                </td>
                            </tr>
                        </table>
                    </div>
<div id="dialog_alert" title="Warning">
<table>
    <tr>
        <td>
            <img src="icon/alert.gif" />
        </td>
        <td>
            <div id="dialog_alert_msg"></div>
        </td>
    </tr>
</table>    
</div>
</asp:Content>