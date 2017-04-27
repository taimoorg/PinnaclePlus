<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="default.aspx.vb" Inherits="._default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PinnaclePlus Login</title>
    <link href="Styles/login.css" rel="stylesheet" type="text/css" />
    
</head>
<body>
    <form id="form1" runat="server">
        <div class="login_box">
            <div class="heading">PinnaclePlus Login</div>
            <div class="content">
                <table style="margin: auto;">
                    <tr>
                        <td colspan="3" style="height: 5px">
                            <asp:Label ID="lblerror" ForeColor="Red" CssClass="AppLabel" runat="server" Text="The Login Failed" Visible="false">The Login Failed</asp:Label></td>
                    </tr>
                    <tr>
                        <td colspan="3" style="height: 2px" class="app_label"></td>
                    </tr>
                    <tr>
                        <td colspan="3" style="height: 5px" class="app_label">Please enter your Pinnacle user name and password.</td>
                    </tr>
                    <tr>
                        <td colspan="3" style="height: 2px" class="app_label"></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="User Name:" CssClass="app_label"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtUserID" runat="server" Width="250" TabIndex="0" CssClass="TxtBox"></asp:TextBox>
                        </td>
                        <td>
                            
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Password:" CssClass="app_label"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="250" CssClass="TxtBox"></asp:TextBox>
                        </td>
                        <td>
                            
                            
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="height: 2px" class="app_label"></td>
                    </tr>

                    <tr>
                        <td colspan="3" style="text-align:center">
                            <asp:Button ID="btnLogin" runat="server" Text="Log In"
                                Style="height: 26px" Width="80px" />
                           

                        </td>

                    </tr>
                <%--    <tr>
                        <td colspan="3" style="height: 5px"></td>
                    </tr>--%>

                </table>

            </div>
            
        </div>
    </form>
</body>
</html>
