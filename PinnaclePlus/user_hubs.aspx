<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/PinnaclePlus_master.Master" CodeBehind="user_hubs.aspx.vb" Inherits=".user_hubs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Pinnacle Plus: Users Hub</title>
    <link href="Styles/data_pages.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="text-align:center;">
        <table style="margin-left:auto;margin-right:auto;">
            <tr>
                
                <td>User: </td>
                <td>
                    <asp:DropDownList ID="ddlUser" runat="server" AutoPostBack="true"></asp:DropDownList></td>
                
            </tr>
            
        </table>
        </div > 
    
    <div style="text-align:center;">
        <asp:Table ID="tblData" runat="server" CssClass="table_class" Width="30%">
       
        
    </asp:Table>
        </div> 
    
    
        
    
    <asp:HiddenField ID="MS_ID" runat="server" />
</asp:Content>
