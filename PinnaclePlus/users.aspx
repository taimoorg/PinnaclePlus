<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/PinnaclePlus_master.Master" CodeBehind="users.aspx.vb" Inherits=".users" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Pinnacle Plus: Users </title>
    <link href="Styles/data_pages.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="top_cap">
        <asp:Literal ID="litCap" runat="server"></asp:Literal>
    </div>
    <div class="error">
        <asp:Literal ID="litError" runat="server" Visible="false" ></asp:Literal>
    </div>
    
    <asp:Panel ID="pnlData" runat="server" Visible="true">
        <div style="margin-left: auto; margin-right: auto; text-align: center;">
            <asp:LinkButton ID="lbAddNew" runat="server">Add New</asp:LinkButton></div>
        <br />
        <asp:Table ID="tblData" runat="server" CssClass="table_class" Width="85%">
        </asp:Table>
    </asp:Panel>
    <asp:Panel ID="pnlEdit" runat="server" Visible="false">
        <div>
            <table style="margin-left: auto; margin-right: auto;">
                <tr>
                    <td>User Name: </td>
                    <td>
                        <asp:TextBox ID="txttxtUn" runat="server" Width="500"></asp:TextBox></td>
                </tr>
                
                <tr>
                    <td> </td>
                    <td>
                        <asp:CheckBox ID="chkIsAdmin" runat="server" Text="Is Admin" /></td>
                </tr>
            </table>
        </div>
        <div style="margin-left: auto; margin-right: auto; text-align: center;">
            <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px" />&nbsp;&nbsp;&nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="100px" />
        </div>
    </asp:Panel>
    
</asp:Content>
