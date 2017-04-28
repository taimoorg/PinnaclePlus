<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/PinnaclePlus_master.Master" CodeBehind="user_rights.aspx.vb" Inherits=".user_rights" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Pinnacle Plus: Users Rights</title>
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
        <asp:TreeView ID="TreeView1" runat="server" ShowCheckBoxes="Leaf" ImageSet="Arrows" ShowLines="True">
            <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD"></HoverNodeStyle>

            <NodeStyle HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" Font-Names="Tahoma" Font-Size="10pt" ForeColor="Black"></NodeStyle>

            <ParentNodeStyle Font-Bold="False"></ParentNodeStyle>

            <SelectedNodeStyle HorizontalPadding="0px" VerticalPadding="0px" Font-Underline="True" ForeColor="#5555DD"></SelectedNodeStyle>
        </asp:TreeView>
        </div> 
    
    
        <div style="text-align:center;">
            <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px" />
        </div>
    
    <asp:HiddenField ID="MS_ID" runat="server" />
</asp:Content>
