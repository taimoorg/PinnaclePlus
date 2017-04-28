<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/PinnaclePlus_master.Master" CodeBehind="report_Rights.aspx.vb" Inherits=".report_Rights" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Pinnacle Plus: Report Rights</title>
    <link href="Styles/data_pages.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="pnlEdit" runat="server">
        <div class="repDDL">
            <%--<label>
                Report:
               
            </label>--%>
            <asp:DropDownList ID="ddlReports" runat="server" AutoPostBack="true" Width="700px"> 
            </asp:DropDownList>

        </div>
      
        <asp:GridView ID="gvPara" runat="server" CssClass="AppLabel" AutoGenerateColumns="False" DataKeyNames="User_ID,REP_ID" AllowPaging="True" OnPageIndexChanging="gvData_PageIndexChanging" PageSize="25" HeaderStyle-BackColor="#4789c4">
            <RowStyle />
            <Columns>
                <asp:TemplateField HeaderText="S#">
                    <ItemTemplate>
                        <%# CType(Container, GridViewRow).RowIndex + 1%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="User_ID" HeaderText="User">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                </asp:BoundField>

                <asp:ButtonField CommandName="CmdDel" ButtonType="Image" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Assign/Revoke" />
            </Columns>
        </asp:GridView>
         
    </asp:Panel>
    <style>
        .checkboxgroup label {
            display: inline;
        }

        .hiddencol {
            display: none;
        }

        #ContentPlaceHolder1_gvPara {
            border-collapse: collapse;
            margin-top: 20px;
            margin-left: 534px;
            width: 600px;
            border-color: #c0c0c0;
        }

        th {
            background-color: #5DB2FF;
            color: white;
        }

        #ContentPlaceHolder1_ddlReports {
            width: 600px !important;
            margin-left: 534px;
        }

        .gvPara:hover {
            background-color: #5db2ff;
            color: #ffffff;
        }

        .AppLabel {
            font-family: verdana;
            font-size: smaller;
            font-weight: normal;
            color: #000000;
            vertical-align: middle;
            text-align: left;
            white-space: nowrap;
        }
    </style>
    <script>


        $("#ddlReports").val($("#target option:first").val('Please Select Report'));


    </script>
</asp:Content>
