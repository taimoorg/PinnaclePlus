<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/gAdmin_master.Master" CodeBehind="config_change.aspx.vb" Inherits=".config_change" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Styles/data_pages.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="top_cap">
        <asp:Literal ID="litCap" runat="server"></asp:Literal>
    </div>
    <div class="error">
        <asp:Literal ID="litError" runat="server" Visible="false" ></asp:Literal>
    </div>
    <br />
    <asp:Panel ID="pnlData" runat="server" Visible="true">
        
        <asp:Table ID="tblData" runat="server" CssClass="table_class" Width="60%">
        </asp:Table>
    </asp:Panel>
    <asp:Panel ID="pnlEdit" runat="server" Visible="false">
        <div>
            <table style="margin-left: auto; margin-right: auto;">
                <tr>
                    <td>Setting: </td>
                    <td>
                        <asp:TextBox ID="txtName" runat="server" Width="500" Enabled="false" ></asp:TextBox></td>
                </tr>
                <tr>
                    <td></td>
                    <td class="app_label" >
                        <asp:Literal ID="litDescription" runat="server"></asp:Literal></td>
                    
                </tr>
                <tr>
                    <td>Value: </td>
                    <td>
                        <asp:TextBox ID="txtValue" runat="server" Width="500"></asp:TextBox></td>
                </tr>
            </table>
        </div>
        <div style="margin-left: auto; margin-right: auto; text-align: center;">
            <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px" />&nbsp;&nbsp;&nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="100px" />
        </div>
    </asp:Panel>
    <asp:HiddenField ID="CONFIG_ID" runat="server" />
</asp:Content>
