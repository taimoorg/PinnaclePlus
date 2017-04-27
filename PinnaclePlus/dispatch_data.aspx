<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/PinnaclePlus_master.Master" CodeBehind="dispatch_data.aspx.vb" Inherits=".dispatch_data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?key=AIzaSyDM7-igdY9_vzj7-1ASLXQmLHPxNDB3OHE&libraries=drawing"></script>
    

    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
  <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
     <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css"/>
     <script>
        $(function () {

            $("[id$=txtDate]").datepicker({
                changeMonth: true,
                changeYear: true,
                beforeShowDay: function (date) {
                    var day = date.getDay();
                    return [day != 0, ''];
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div>
        <table>
                <tr>
                    <td>
                        <asp:DropDownList ID="ddlHub" runat="server"></asp:DropDownList>
                    </td>
                    <td>
                        Date:
                    </td>
                    <td >
                        <asp:TextBox ID="txtDate" runat="server"  AutoPostBack="true" ></asp:TextBox> 
                    </td>
                    <td>
                        Manifest:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlManifest" runat="server"></asp:DropDownList>
                    </td>
                </tr>
            </table>
    </div>
   <div>
       <asp:Table ID="tbldata" runat="server"></asp:Table>
   </div>
</asp:Content>
