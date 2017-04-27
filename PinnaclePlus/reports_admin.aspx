<%@ Page Title="" Language="vb" AutoEventWireup="false"  enableEventValidation="false" MasterPageFile="~/PinnaclePlus_master.Master" CodeBehind="reports_admin.aspx.vb" Inherits=".reports_admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" />
<%--    <link rel="stylesheet" href="/resources/demos/style.css" />--%>
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    
    <link href="Styles/data_pages.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">

        $(document).ready(function () {
           AutoComplete();
        });

      function AutoComplete() {
       $("#<%=txtSearch.ClientID %>").autocomplete({
                autoFocus: true,
                source: function (request, response) {
                    $.ajax({
                        url: "man_api.aspx/AutoComplete",
                        data: "{'SearchText': '" + request.term + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.Rep_Name,
                                    val: item.Rep_ID
                                }
                            }))
                        },
                        error: function (response) {
                            alert(response.responseText);
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        }
                    });
                },
                select: function (e, i) {
                    $('#<%=Rep_Val.ClientID %>').val(i.item.val);
                    __doPostBack("<%= btnGo.UniqueID%>", "OnClick");},
                           
                minLength: 1,
            });
         }

    </script>
    
    <style>
        .GvGrid:hover {
            /*background-color: #FFEB9C;   color:#9C6500; */
            background-color: #5db2ff;
            color: #ffffff;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="top_cap">
        <asp:Literal ID="litCap" runat="server"></asp:Literal>
    </div>
    <div class="error">
        <asp:Literal ID="litError" runat="server" Visible="false"></asp:Literal>
    </div>

    <asp:Panel ID="pnlData" runat="server" Visible="true">
        <div style="margin-left: auto; margin-right: auto; text-align: center;">
            <asp:LinkButton ID="lbAddNew" runat="server">Add New</asp:LinkButton>
        </div>
        <br />

    <%--AUTO COMPLETE SEARCH--%> 
        <div style="margin-left: auto; margin-right: auto; text-align: center">
            <b style ="color :#4789c4">Search</b>
            <asp:TextBox ID="txtSearch" placeholder="Search by Name" runat="server" Width="500px" ></asp:TextBox>
               <asp:HiddenField ID="Rep_Val" runat="server" />
                <asp:Button ID="btnGo" runat="server" Text="Button" style="display:none;" />
        </div>
        <br />

        <div style="margin-left: auto; margin-right: auto; width: 80%">
    <%--DROPDOWNLIST FOR PAGING--%>
             <asp:Label ID="Label1" runat="server" Text="Choose Page." Font-Bold="True" ForeColor="#4789c4"></asp:Label>
             <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" Width="80px"  ForeColor="#4789c4" >
                <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                <asp:ListItem Text="20" Value="20"></asp:ListItem>
                <asp:ListItem Text="30" Value="30"></asp:ListItem>
                <asp:ListItem Text="40" Value="40"></asp:ListItem>
                <asp:ListItem Text="50" Value="50"></asp:ListItem>
                <asp:ListItem Text="All" Value="99999"></asp:ListItem>
            </asp:DropDownList>

            <asp:GridView ID="gvData" runat="server" CssClass="AppLabel" Width="100%" AutoGenerateColumns="False" DataKeyNames="REP_ID" RowStyle-CssClass="GvGrid" AllowPaging="True" PageSize="25" HeaderStyle-BackColor="#4789c4" HeaderStyle-Height ="25px">
                <PagerStyle BackColor="#4789c4" ForeColor="White" Font-Size="14px" HorizontalAlign="Center" />

                <RowStyle />
                <Columns>
                    <asp:BoundField DataField="Name" HeaderText="Name">
                        <HeaderStyle HorizontalAlign="Center" Font-Size="Medium" ForeColor="White" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Description" HeaderText="Description">
                        <HeaderStyle HorizontalAlign="Center" Font-Size="Medium" ForeColor="White" />
                        <ItemStyle CssClass="WrappedColumnText" HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:ButtonField ButtonType="Link" CommandName="CmdEdit" HeaderStyle-HorizontalAlign="Center" HeaderText="Edit" ItemStyle-HorizontalAlign="Center" Text="Edit">
                        <HeaderStyle HorizontalAlign="Center" ForeColor="White" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:ButtonField>
                    <asp:ButtonField ButtonType="Link" CommandName="CmdCopy" HeaderStyle-HorizontalAlign="Center" HeaderText="Copy" ItemStyle-HorizontalAlign="Center" Text="Copy">
                        <HeaderStyle HorizontalAlign="Center" ForeColor="White" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:ButtonField>
                    <asp:ButtonField ButtonType="Link" CommandName="CmdDel" HeaderStyle-HorizontalAlign="Center" HeaderText="Delete" ItemStyle-HorizontalAlign="Center" Text="Delete">
                        <HeaderStyle HorizontalAlign="Center" ForeColor="White" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:ButtonField>
                </Columns>
            </asp:GridView>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlEdit" runat="server" Visible="false">
        <div>
            <table style="margin-left: auto; margin-right: auto;">
                <tr>
                    <td>Name: </td>
                    <td>
                        <asp:TextBox ID="txtName" runat="server" Width="1000"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Description: </td>
                    <td>
                        <asp:TextBox ID="txtDes" runat="server" Width="1000"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkIsPP" runat="server" Text="Is Pinnacle Plus" /></td>

                </tr>
                <tr>
                    <td>Query </td>
                    <td>
                        <asp:TextBox ID="txtQuery" runat="server" Width="1000" TextMode="MultiLine" Rows="40"></asp:TextBox></td>
                </tr>
                <tr>
                    <td></td>
                    <td>

                        <asp:GridView ID="gvPara" runat="server" CssClass="AppLabel" AutoGenerateColumns="False" DataKeyNames="RP_ID">
                            <RowStyle />
                            <Columns>
                                <asp:TemplateField HeaderText="S#">
                                    <HeaderStyle HorizontalAlign="Center" Width="10px" />
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <%# CType(Container, GridViewRow).RowIndex + 1%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Name">
                                    <HeaderStyle HorizontalAlign="Center" Width="110px" />
                                    <ItemStyle HorizontalAlign="Left" Width="110px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtParaName" runat="server" Width="110px" Text='<%# Bind("Name")%>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description">
                                    <HeaderStyle HorizontalAlign="Center" Width="200px" />
                                    <ItemStyle HorizontalAlign="Left" Width="200px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtParaDescription" runat="server" Width="200px" Text='<%# Bind("Description")%>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Type">
                                    <HeaderStyle HorizontalAlign="Center" Width="110px" />
                                    <ItemStyle HorizontalAlign="Left" Width="110px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlParaType" runat="server" Width="110px">
                                            <asp:ListItem Text="GeneralText" Value="10"></asp:ListItem>
                                            <asp:ListItem Text="OrderNo" Value="20"></asp:ListItem>
                                            <asp:ListItem Text="OrderNo_Outer" Value="21"></asp:ListItem>
                                            <asp:ListItem Text="TrackingNo" Value="30"></asp:ListItem>
                                            <asp:ListItem Text="TrackingNo_Outer" Value="31"></asp:ListItem>
                                            <asp:ListItem Text="Date_" Value="40"></asp:ListItem>
                                            <asp:ListItem Text="ClientList" Value="50"></asp:ListItem>
                                            <asp:ListItem Text="CheckBox" Value="60"></asp:ListItem>
                                            <asp:ListItem Text="Hub_List" Value="70"></asp:ListItem>
                                            <asp:ListItem Text="State_List" Value="80"></asp:ListItem>
                                            <asp:ListItem Text="Cur_User" Value="90"></asp:ListItem>
                                            <asp:ListItem Text="File" Value="100"></asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Width">
                                    <HeaderStyle HorizontalAlign="Center" Width="40px" />
                                    <ItemStyle HorizontalAlign="Left" Width="40px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtParaWidth" runat="server" Width="40px" Text='<%# Bind("Width")%>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rows">
                                    <HeaderStyle HorizontalAlign="Center" Width="40px" />
                                    <ItemStyle HorizontalAlign="Left" Width="40px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtParaRows" runat="server" Width="40px" Text='<%# Bind("Rows_")%>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:ButtonField CommandName="CmdDel" ButtonType="Link" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Delete" Text="Delete" />
                            </Columns>
                        </asp:GridView>
                        <asp:Button ID="btnAddPara" runat="server" Text="Add Parameter" />
                    </td>
                </tr>
                <tr>
                    <td>Users</td>
                    <td>
                        <asp:CheckBoxList ID="lstUsers" runat="server"></asp:CheckBoxList>

                    </td>
                </tr>
            </table>
        </div>
        <div style="margin-left: auto; margin-right: auto; text-align: center;">
            <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px" />&nbsp;&nbsp;&nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="100px" />
        </div>
    </asp:Panel>
    <asp:HiddenField ID="REP_ID" runat="server" />
</asp:Content>
