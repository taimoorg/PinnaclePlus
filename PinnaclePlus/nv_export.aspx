<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/PinnaclePlus_master.Master" CodeBehind="nv_export.aspx.vb" Inherits=".nv_export" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Pinnacle Plus: Export</title>
    <link href="Styles/data_pages.css" rel="stylesheet" type="text/css" />
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.6/jquery.min.js" type="text/javascript"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js" type="text/javascript"></script>
    <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            $("[id$=txtDate]").datepicker({
                changeMonth: true,
                changeYear: true,
                beforeShowDay: $.datepicker.noWeekends
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="top_cap">
        <asp:Literal ID="litCap" runat="server"></asp:Literal>
    </div>
    
    
        <div style="margin-left: auto; margin-right: auto; text-align: center;">
            <table>
                <tr>
                    <td>
                        Date:
                    </td>
                    <td>
                        <asp:TextBox ID="txtDate" runat="server" AutoPostBack="true"  ></asp:TextBox> 
                    </td>
                    <td>
                        <asp:Button ID="btnReferesh" runat="server" Text="Refresh" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
            <ProgressTemplate>
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    Working please wait....<br />
                    <img src="Styles/images/ajax-loader.gif" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>

                
        
        <div style="margin-left: auto; margin-right: auto; text-align: left;">
            <table style="width :60%;" >
                <tr>
                    <td >
                        Step 1:</td>
                    <td>
                        <asp:LinkButton ID="lbAddDate" runat="server">Update From Pinnacle</asp:LinkButton>
                    <%--    <br />
                        <asp:LinkButton ID="lbManualOrder" runat="server">Add Manual Order</asp:LinkButton>--%>
                        
                        
                    </td>
                    <td>
                        Step 2:</td>
                    <td>
                        <asp:LinkButton ID="lbExporttoOnfleet" runat="server">Export to nuVizz</asp:LinkButton>
                        
                    </td>
                    <td>Step 3:</td>
                    <td>
                        <asp:LinkButton ID="lbGetAssignment" runat="server">Sync With nuVizz</asp:LinkButton>
                    </td>
                    <td>
                        Step 4:</td>
                    <td>
                        <asp:LinkButton ID="lbTaskSheet" runat="server">Print Manifest Sheet</asp:LinkButton>
                        <br />
                        <asp:LinkButton ID="lbPullSheet" runat="server">Print Pull Sheet</asp:LinkButton>
                        
                    </td>
                </tr>
            </table>
        </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlData" runat="server" Visible="true">
                <div class="error">
            <asp:Literal ID="litError" runat="server" Visible="false"></asp:Literal>
        </div>
        <asp:GridView id="gvData" runat="server" CssClass="AppLabel" Width="100%" AutoGenerateColumns="False" DataKeyNames="MO_ID">
            <RowStyle />
            <Columns>
                <asp:TemplateField HeaderText="S#">
                    <HeaderStyle HorizontalAlign="Center" Width="10px"/>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <%# CType(Container, GridViewRow).RowIndex + 1%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="nv Status" HeaderStyle-Width="10px" >
                    <HeaderStyle HorizontalAlign="Center"/>
                    <ItemStyle  HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <asp:Literal ID="Literal1" runat="server" Text='<%# Bind("of_stop")%>'> </asp:Literal>
                        <asp:Image ID="imgOFStatus" runat="server" ImageUrl='<%# Bind("Of_Url")%>'  />
                        <asp:Literal ID="litofError" runat="server" Text='<%# Bind("of_error")%>' > </asp:Literal>
                        <asp:Literal ID="litOfStatus" runat="server" Text='<%# Bind("WorkerName")%> '> </asp:Literal>&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Order#">
                    <HeaderStyle HorizontalAlign="Center"  Width="10px"/>
                    <ItemStyle HorizontalAlign="Center" ></ItemStyle>
                    <ItemTemplate>
                        <asp:Image ID="imgPicDel" runat="server" />
                        <asp:LinkButton ID="lbOrderNo" runat="server" Visible="false"></asp:LinkButton>
                        <asp:HyperLink  ID="hlOrderNo" runat="server" Visible="false" ></asp:HyperLink>
                        &nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Name" HeaderText="Name" >
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                </asp:BoundField>   
                <asp:BoundField DataField="Address"  HeaderText="Address" >
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Left" CssClass="WrappedColumnText"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="City"  HeaderText="City" >
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="State_"  HeaderText="State" >
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Zip"  HeaderText="Zip" >
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Phone"  HeaderText="Phone" DataFormatString="{0:(###) ###-####}" >
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Date_"  HeaderText="Sch Date" DataFormatString="{0:d}">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
                <asp:ButtonField CommandName="CmdDel" ButtonType="Link" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Delete" Text="Delete"  />

                
            </Columns>
        </asp:GridView>

    </asp:Panel>
    <asp:Panel ID="pnlAddManual" runat="server" Visible="false">
        <div>
            <table style="margin-left: auto; margin-right: auto;">
                <tr>
                    <td>
                        Client Name:
                    </td>
                    <td>
                        <asp:TextBox ID="txtClientName" runat="server" Width="280px"></asp:TextBox></td>
                    <td>Order No:</td>
                    <td>
                        <asp:TextBox ID="txtOrderNo" runat="server" Width="280px"></asp:TextBox></td>
                    <td>Service Type:</td>
                    <td>
                        <asp:DropDownList ID="ddlServiceTYpe" runat="server">
                            <asp:ListItem Text="White Glove" Value="WG"></asp:ListItem>
                            <asp:ListItem Text="Threshold" Value="TRHD"></asp:ListItem>
                        </asp:DropDownList><asp:CheckBox ID="chkPickUp" runat="server" text="Is Pickup"/>
                    </td>
                </tr>
                <tr>
                    <td>Qty:</td>
                    <td>
                        <asp:TextBox ID="TxtQty" runat="server" Width="280px" TextMode="Number" ></asp:TextBox></td>
                    <td>Weight: </td>
                    <td><asp:TextBox ID="txtWt" runat="server" Width="280px" TextMode="Number" ></asp:TextBox></td>
                    <td>Cubic feet: </td>
                    <td><asp:TextBox ID="TxtCfts" runat="server" Width="280px" TextMode="Number" ></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Name:</td>
                    <td>
                        <asp:TextBox ID="txtName" runat="server" Width="280px"></asp:TextBox></td>
                    <td >Address 1:</td>
                    <td ><asp:TextBox ID="txtAddress1" runat="server" Width="280px"  ></asp:TextBox></td>
                    <td >Address 2:</td>
                    <td ><asp:TextBox ID="txtAddress2" runat="server" Width="280px"  ></asp:TextBox></td>
                </tr>
                <tr>
                    <td>City:</td>
                    <td>
                        <asp:TextBox ID="TxtCity" runat="server" Width="280px"  ></asp:TextBox></td>
                    <td>State: </td>
                    <td><asp:TextBox ID="txtState" runat="server" Width="280px"  ></asp:TextBox></td>
                    <td>Zip: </td>
                    <td><asp:TextBox ID="TxtZip" runat="server" Width="280px" TextMode="Number" ></asp:TextBox></td>
                </tr>
                 <tr>
                    <td>Phone:</td>
                    <td>
                        <asp:TextBox ID="txtphone" runat="server" Width="280px"  TextMode="Number"></asp:TextBox></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="3">Special Instructions:</td>
                    <td colspan="3">Task Details:</td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:TextBox ID="txtSI" runat="server" Width="560px" TextMode="MultiLine" Height="80px" ></asp:TextBox>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtTaskDetails" runat="server" Width="560px" TextMode="MultiLine" Height="78px" ></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
        <div style="margin-left: auto; margin-right: auto; text-align: center;">
            <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px" />&nbsp;&nbsp;&nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="100px" />
        </div>
    </asp:Panel>
                    </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger  ControlID="txtDate" EventName="TextChanged"/>
                <asp:AsyncPostBackTrigger  ControlID="btnReferesh" EventName="Click"/>
                <asp:AsyncPostBackTrigger  ControlID="lbAddDate" EventName="Click"/>
                <asp:AsyncPostBackTrigger  ControlID="lbExporttoOnfleet" EventName="Click"/>
                <asp:AsyncPostBackTrigger  ControlID="lbGetAssignment" EventName="Click"/>
                
            </Triggers>
        </asp:UpdatePanel>
    <asp:HiddenField ID="BO_ID" runat="server" />
</asp:Content>



