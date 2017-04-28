<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/PinnaclePlus_master.Master" CodeBehind="dispatch_data.aspx.vb" Inherits=".dispatch_data" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Pinnacle Plus: Dispatch Data</title>
    <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?key=AIzaSyDM7-igdY9_vzj7-1ASLXQmLHPxNDB3OHE&libraries=drawing"></script>

    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" />

    <script>
        var editdialog;
        var editdialogforDetails;
        $(document).ready(function () {
            SetDialog();
            SetDetailDialog();
        });

        $(function () {

            $("[id$=txtDateFrom]").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'yy-mm-dd',
                beforeShowDay: function (date) {
                    var day = date.getDay();
                    return [day != 0, ''];
                }
            });
            $("[id$=txtDateTo").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'yy-mm-dd',
                beforeShowDay: function (date) {
                    var day = date.getDay();
                    return [day != 0, ''];
                }
            });
        });

        function SetDialog() {
            editdialog = $("#dialog").dialog({
                autoOpen: false,
                title: "Exception Details",
                modal: true,
                buttons: {
                    Cancel: function () {
                        $(this).dialog("close");
                    }
                },

            });
        }
        function SetDetailDialog() {
            editdialogforDetails = $("#DetailDialog").dialog({
                autoOpen: false,
                title: "Document Details",
                modal: true,
                buttons: {
                    Cancel: function () {
                        $(this).dialog("close");
                    }
                },

            });
        }
        function ShowException(MSO_ID) {
            $.ajax({
                type: "POST",
                url: "dispatch_data.aspx/P_ExceptionAgainst_Menifest",
                data: '{MSO_ID: ' + MSO_ID + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    $("#dialog").html(response.d);
                    editdialog.dialog("open");
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
        function ShowManifestFiles(MSO_ID) {

            $.ajax({
                type: "POST",
                url: "dispatch_data.aspx/P_DetailsAgainst_Menifest",
                data: '{MSO_ID: ' + MSO_ID + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    $("#DetailDialog").html(response.d);
                    editdialogforDetails.dialog("open");
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

        function P_DownloadFilesAgainst_Menifest(MSOED_ID) {
            $.ajax({
                type: "POST",
                url: "dispatch_data.aspx/P_DownloadFilesAgainst_Menifest",
                data: '{MSOED_ID: ' + MSOED_ID + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
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
    </script>
    <style>
        .AltRow {
            background-color: #F3F3F3;
        }

        .gridHeader {
            background-color: #5DB2FF;
            color: white;
            height: 30px;
            margin: 0 auto;
        }

        .AppLabel tr.normal:hover, .AppLabel tr.alternate:hover {
            background-color: white;
            color: black;
            font-weight: bold;
        }

        .filterHead {
            border: 1px solid #CDCDCD !important;
            height: 65px;
            display: table;
            margin: 0 auto;
            width: 99%;
        }

        .centerGrid {
            display: table;
            margin: 0 auto;
        }

        .GvGrid:hover {
            background-color: #FFEB9C;
            border-top: solid;
            color: #9C6500;
        }

        .export_button {
            float: right;
            margin-right: 9px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="filterHead">
        Date From:
        <asp:TextBox ID="txtDateFrom" runat="server"></asp:TextBox>
        Date To:
        <asp:TextBox ID="txtDateTo" runat="server"></asp:TextBox>
        Load No:
        <asp:TextBox ID="txtLoadNo" runat="server"></asp:TextBox>
        POM No:
        <asp:TextBox ID="txtPoMNo" runat="server"></asp:TextBox>
        Hubs:
        <asp:DropDownList runat="server" ID="ddlHub" />
        Exception:
        <asp:DropDownList runat="server" ID="ddlException">
            <asp:ListItem Text="All" Value=""></asp:ListItem>
            <asp:ListItem Text="Exception" Value="1"></asp:ListItem>
            <asp:ListItem Text="Without Exception" Value="0"></asp:ListItem>
        </asp:DropDownList>
        <asp:Button ID="btnSearch" Text="Search" CssClass="search" runat="server" OnClick="btnfilter_Click" />
    </div>

    <div id="dialog" style="display: none">
    </div>
    <div id="DetailDialog" style="display: none">
    </div>
    <div class="export_button">
        <asp:Button ID="btntoExcel" runat="server" CssClass="excelButton" Text="Export" OnClick="btntoExcel_OnClick" />
    </div>

    <div class="centerGrid">
        <asp:GridView ID="gvPara" runat="server" CssClass="AppLabel" AutoGenerateColumns="False" AllowPaging="true" DataKeyNames="MSO_ID,HasException,Order_ID" AllowSorting="true" PageSize="50" RowStyle-CssClass="GvGrid">
            <Columns>
                <asp:TemplateField HeaderText="S#">
                    <HeaderStyle HorizontalAlign="Center" Width="10px" CssClass="gridHeader" />
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <%# CType(Container, GridViewRow).RowIndex + 1%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="MIN_ID" HeaderText="Load No">
                    <HeaderStyle HorizontalAlign="Left" CssClass="gridHeader" />
                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="StartDate" HeaderText="Load Date">
                    <HeaderStyle HorizontalAlign="Left" CssClass="gridHeader" />
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                </asp:BoundField>

                <asp:TemplateField HeaderText="POM No">
                    <HeaderStyle HorizontalAlign="Center" Width="10px" CssClass="gridHeader" />
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <asp:HyperLink ID="hl_POM" runat="server"></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="MSO_ID" HeaderText="Stop No">
                    <HeaderStyle HorizontalAlign="Left" CssClass="gridHeader" />
                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Pickup" HeaderText="Pick Up">
                    <HeaderStyle HorizontalAlign="Left" CssClass="gridHeader" />
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="DeliveryStartDTTM" HeaderText="Stop Arrival">
                    <HeaderStyle HorizontalAlign="Left" CssClass="gridHeader" />
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="DeliveryEndDTTM" HeaderText="Stop Departure">
                    <HeaderStyle HorizontalAlign="Left" CssClass="gridHeader" />
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="State" HeaderText="State">
                    <HeaderStyle HorizontalAlign="Left" CssClass="gridHeader" />
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="City" HeaderText="City">
                    <HeaderStyle HorizontalAlign="Left" CssClass="gridHeader" />
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                </asp:BoundField>

                <asp:BoundField DataField="Hub" HeaderText="HUb">
                    <HeaderStyle HorizontalAlign="Left" CssClass="gridHeader" />
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                </asp:BoundField>
                <asp:TemplateField HeaderText="Exception">
                    <HeaderStyle HorizontalAlign="Center" Width="10px" CssClass="gridHeader" />
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <asp:HyperLink ID="hl_Exception" runat="server"></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Details">
                    <HeaderStyle HorizontalAlign="Center" Width="10px" CssClass="gridHeader" />
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <asp:HyperLink ID="HL_MSOED_ID" runat="server"></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Actual_Driver" HeaderText="Driver">
                    <HeaderStyle HorizontalAlign="Left" CssClass="gridHeader" />
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Client" HeaderText="Client">
                    <HeaderStyle HorizontalAlign="Left" CssClass="gridHeader" Width="600px" />
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Email" HeaderText="Email">
                    <HeaderStyle HorizontalAlign="Left" CssClass="gridHeader" Width="200px" />
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Address" HeaderText="Address">
                    <HeaderStyle HorizontalAlign="Left" CssClass="gridHeader" />
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                </asp:BoundField>
            </Columns>
            <RowStyle CssClass="Row" />
            <AlternatingRowStyle CssClass="AltRow" />
            <FooterStyle BackColor="#5DB2FF" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#5DB2FF" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#5DB2FF" Font-Bold="True" ForeColor="#5DB2FF" />
        </asp:GridView>
    </div>
</asp:Content>
