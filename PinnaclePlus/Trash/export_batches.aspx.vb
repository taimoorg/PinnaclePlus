Imports Newtonsoft.Json
Imports System.Net
Imports System.IO
Imports OfficeOpenXml
Imports PinnacleFunction
Public Class export_batches
    Inherits System.Web.UI.Page
    Private Enum Page_Options
        View = 30101
        Delete = 30105

    End Enum
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not PinnaclePlus.Security.P_Has_Rights(Page_Options.View) Then
            Response.Redirect("no_rights.aspx")
        End If

        If Not Page.IsPostBack Then
            litCap.Text = String.Format("Export Batch [{0}]", "")
            'FillGrid()
        End If
    End Sub
    Private Sub SetInOrder()
        'lbAddDate.Text = String.Format("Update From Pinnacle ({0})", PinnaclePlus.OrderBatch.P_Order_Get_ByUnAttached_Count(txtDate.Text, ""))
    End Sub


    Private Sub FillGrid(DT As DataTable)
        If Not IsDate(txtDate.Text) Then
            Exit Sub
        End If

        gvData.DataSource = DT
        gvData.DataBind()
        SetInOrder()
        litError.Visible = False
    End Sub


    'Private Sub FillGrid(Date_ As Object)
    '    If Date_ Is Nothing Then
    '        Exit Sub
    '    End If
    '    Dim DT As DataTable
    '    Dim DT_pin As DataTable
    '    Dim Dr As DataRow
    '    Dim Tr As TableRow
    '    Dim Thr As TableHeaderRow
    '    Dim Thc As TableHeaderCell
    '    Dim TC As TableCell
    '    Dim Lb As LinkButton
    '    Dim imgBut As ImageButton
    '    Dim Img As Image
    '    Dim HL As HyperLink
    '    Dim Pic_Del As String
    '    Dim OrderStr, UrlStr As String
    '    Dim HasLatLong As Boolean
    '    Dim LL As PinnaclePlus.Google.Lat_long
    '    Dim Lt As Literal
    '    Dim Err As String = ""

    '    DT = PinnaclePlus.OrderBatch.P_Batch_Orders_Get_ByDate(Date_, "")
    '    'OrderStr = "'0'"
    '    'For i = 0 To DT.Rows.Count - 1
    '    'OrderStr = String.Format("{0},'{1}'", OrderStr, DT.Rows(i).Item("Order_ID"))
    '    'Next
    '    tblData.Rows.Clear()

    '    Thr = New TableHeaderRow

    '    Thc = New TableHeaderCell
    '    Thc.CssClass = "table_header"
    '    Thc.Text = "S#"
    '    Thc.HorizontalAlign = HorizontalAlign.Center
    '    Thr.Cells.Add(Thc)

    '    Thc = New TableHeaderCell
    '    Thc.CssClass = "table_header"
    '    Thc.Text = "Order#"
    '    Thc.HorizontalAlign = HorizontalAlign.Center
    '    Thr.Cells.Add(Thc)

    '    Thc = New TableHeaderCell
    '    Thc.CssClass = "table_header"
    '    Thc.Text = " "
    '    Thc.HorizontalAlign = HorizontalAlign.Center
    '    Thr.Cells.Add(Thc)

    '    Thc = New TableHeaderCell
    '    Thc.CssClass = "table_header"
    '    Thc.Text = "Name"
    '    Thc.HorizontalAlign = HorizontalAlign.Center
    '    Thr.Cells.Add(Thc)


    '    Thc = New TableHeaderCell
    '    Thc.CssClass = "table_header"
    '    Thc.Text = "Address"
    '    Thc.HorizontalAlign = HorizontalAlign.Center
    '    Thr.Cells.Add(Thc)


    '    Thc = New TableHeaderCell
    '    Thc.CssClass = "table_header"
    '    Thc.Text = "Zip"
    '    Thc.HorizontalAlign = HorizontalAlign.Center
    '    Thr.Cells.Add(Thc)

    '    Thc = New TableHeaderCell
    '    Thc.CssClass = "table_header"
    '    Thc.Text = "City"
    '    Thc.HorizontalAlign = HorizontalAlign.Center
    '    Thr.Cells.Add(Thc)

    '    Thc = New TableHeaderCell
    '    Thc.CssClass = "table_header"
    '    Thc.Text = "State"
    '    Thc.HorizontalAlign = HorizontalAlign.Center
    '    Thr.Cells.Add(Thc)

    '    Thc = New TableHeaderCell
    '    Thc.CssClass = "table_header"
    '    Thc.Text = "Phone"
    '    Thc.HorizontalAlign = HorizontalAlign.Center
    '    Thr.Cells.Add(Thc)

    '    Thc = New TableHeaderCell
    '    Thc.CssClass = "table_header"
    '    Thc.Text = "Sch Date"
    '    Thc.HorizontalAlign = HorizontalAlign.Center
    '    Thr.Cells.Add(Thc)

    '    'If gAdmin.Security.P_Has_Rights(Page_Options.Edit) Then
    '    '    Thc = New TableHeaderCell
    '    '    Thc.CssClass = "table_header"
    '    '    Thc.Text = "Edit"
    '    '    Thc.HorizontalAlign = HorizontalAlign.Center
    '    '    Thr.Cells.Add(Thc)
    '    'End If
    '    Thc = New TableHeaderCell
    '    Thc.CssClass = "table_header"
    '    Thc.Text = "OF Status"
    '    Thc.HorizontalAlign = HorizontalAlign.Center
    '    Thr.Cells.Add(Thc)
    '    If PinnaclePlus.Security.P_Has_Rights(Page_Options.Delete) Then
    '        Thc = New TableHeaderCell
    '        Thc.CssClass = "table_header"
    '        Thc.Text = "Delete"
    '        Thc.HorizontalAlign = HorizontalAlign.Center
    '        Thr.Cells.Add(Thc)
    '    End If
    '    tblData.Rows.Add(Thr)
    '    'DT_pin = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectPinnacle(String.Format("select * from V_Order_Date where OrderNo in ({0})", OrderStr))
    '    For i = 0 To DT.Rows.Count - 1
    '        Err = ""

    '        Dr = DT.Rows(i) 'DT_pin.Select(String.Format("[OrderNo]='{0}'", DT.Rows(i).Item("Order_ID")))(0)

    '        If DT.Rows(i).Item("Pickup") Then
    '            Pic_Del = "Pic"
    '        Else
    '            Pic_Del = "Del"
    '        End If
    '        'Get Lat Long if not available 
    '        HasLatLong = True

    '        'LL = New PinnaclePlus.Google.Lat_long
    '        'If DT.Rows(i).Item("Address1").ToString.ToLower.Trim <> Dr.Item(String.Format("Address1_{0}", Pic_Del)).ToString.ToLower.Trim Then
    '        'HasLatLong = False
    '        'ElseIf IsDBNull(DT.Rows(i).Item("Lati")) Or IsDBNull(DT.Rows(i).Item("Longi")) Then
    '        'HasLatLong = False
    '        'End If
    '        'If HasLatLong = False Then

    '        'LL = PinnaclePlus.Google.GoogleApi.FindCoordinates(Dr.Item(String.Format("Address1_{0}", Pic_Del)), Dr.Item(String.Format("Zip_{0}", Pic_Del)), Dr.Item(String.Format("State_{0}", Pic_Del)))
    '        'If Not (LL Is Nothing) Then
    '        'PinnaclePlus.OrderBatch.P_Batch_Orders_Update(DT.Rows(i).Item("BO_ID"), Dr.Item(String.Format("Address1_{0}", Pic_Del)), Dr.Item(String.Format("Zip_{0}", Pic_Del)), LL.Lati, LL.Longi)
    '        'End If
    '        'Else
    '        'LL.Lati = DT.Rows(i).Item("Lati")
    '        'LL.Longi = DT.Rows(i).Item("Longi")
    '        'End If
    '        'If DT.Rows(i).Item("Hub").ToString.ToLower.Trim <> Dr.Item(String.Format("Hub_{0}", Pic_Del)).ToString.ToLower.Trim Then
    '        'PinnaclePlus.OrderBatch.P_Batch_Orders_Del(DT.Rows(i).Item("BO_ID"))
    '        'Continue For
    '        'End If
    '        'End Get Lat Long if not available 

    '        Tr = New TableRow

    '        TC = New TableCell
    '        TC.Text = i + 1
    '        TC.CssClass = "table_cell_cen"
    '        Tr.Cells.Add(TC)


    '        If Dr.Item("Is_Manual") Then
    '            Lb = New LinkButton
    '            Lb.ID = String.Format("LB_Order_{0}", DT.Rows(i).Item("BO_ID"))
    '            Lb.Text = String.Format("M-{0}", DT.Rows(i).Item("Order_ID"))
    '            AddHandler Lb.Click, AddressOf EditRecord
    '            TC = New TableCell
    '            TC.CssClass = "table_cell_cen"
    '            TC.Controls.Add(Lb)
    '            Tr.Cells.Add(TC)
    '        Else
    '            HL = New HyperLink
    '            HL.Text = DT.Rows(i).Item("Order_ID")
    '            UrlStr = String.Format("OrderId={0}", Dr.Item("Order_ID"))
    '            HL.NavigateUrl = "http://main.metropolitanwarehouse.com/Order/OrderDetail?eqs=" + PinnaclePlus.Security.EncriptURL(UrlStr, "MetroCryptoUSA07306#?+")
    '            HL.Target = "_blank"
    '            TC = New TableCell
    '            TC.CssClass = "table_cell_cen"
    '            TC.Controls.Add(HL)
    '            Tr.Cells.Add(TC)
    '        End If

    '        TC = New TableCell
    '        Img = New Image
    '        Img.ImageUrl = String.Format("~/Styles/images/{0}.png", Pic_Del)
    '        TC.Controls.Add(Img)
    '        TC.Width = 10
    '        TC.CssClass = "table_cell_cen"
    '        Tr.Cells.Add(TC)


    '        TC = New TableCell
    '        TC.Text = Dr.Item("Name")
    '        TC.CssClass = "table_cell"
    '        Tr.Cells.Add(TC)
    '        TC = New TableCell
    '        If Not IsDBNull((DT.Rows(i).Item("Lati"))) Then

    '            TC.Text = String.Format("<a href='https://maps.google.com/maps?q={1},{2}' target='_blank'>{0}</a>", String.Format("{0}{1}{2}", Dr.Item("Address1"), IIf(Dr.Item("Address2") = "", "", "</br>"), Dr.Item("Address2")), _
    '            Dr.Item("Lati"), Dr.Item("Longi"))
    '        Else
    '            Err = "Invalid Address"
    '            TC.Text = String.Format("{0}{1}{2}", Dr.Item("Address1"), IIf(Dr.Item("Address2") = "", "", "</br>"), Dr.Item("Address2"))
    '        End If
    '        '

    '        TC.CssClass = "table_cell"
    '        Tr.Cells.Add(TC)

    '        TC = New TableCell
    '        TC.Text = Dr.Item("Zip")
    '        TC.CssClass = "table_cell_cen"
    '        Tr.Cells.Add(TC)

    '        TC = New TableCell
    '        TC.Text = Dr.Item("City")
    '        TC.CssClass = "table_cell"
    '        Tr.Cells.Add(TC)

    '        TC = New TableCell
    '        TC.Text = Dr.Item("State_")
    '        TC.CssClass = "table_cell_cen"
    '        Tr.Cells.Add(TC)

    '        TC = New TableCell
    '        TC.Text = Dr.Item("Phone")
    '        TC.CssClass = "table_cell_cen"
    '        Tr.Cells.Add(TC)

    '        TC = New TableCell
    '        TC.Text = Dr.Item("Date_")
    '        TC.CssClass = "table_cell_cen"
    '        Tr.Cells.Add(TC)

    '        imgBut = New ImageButton
    '        imgBut.ID = String.Format("IB_Of_{0}", DT.Rows(i).Item("BO_ID"))
    '        If IsDBNull(DT.Rows(i).Item("of_id")) Then
    '            imgBut.ImageUrl = "~/Styles/images/cross.gif"
    '            'imgBut. = "Edit"
    '            AddHandler imgBut.Click, AddressOf ExporttoOnfleetSingle
    '        Else
    '            imgBut.ImageUrl = "~/Styles/images/tick.gif"
    '        End If

    '        TC = New TableCell
    '        TC.CssClass = "table_edit"
    '        TC.Controls.Add(imgBut)
    '        Lt = New Literal
    '        If Not IsDBNull(DT.Rows(i).Item("of_error")) Then
    '            Err = DT.Rows(i).Item("of_error")
    '        End If
    '        If Err <> "" Then
    '            Lt.Text = String.Format("<span class='error_small' style='color:#ff0000;'>{0}</span>", Err)
    '            TC.Controls.Add(Lt)
    '        End If
    '        Tr.Cells.Add(TC)


    '        If PinnaclePlus.Security.P_Has_Rights(Page_Options.Delete) Then
    '            Lb = New LinkButton
    '            Lb.ID = String.Format("LB_Del_{0}", DT.Rows(i).Item("BO_ID"))
    '            Lb.Text = "Delete"
    '            Lb.OnClientClick = "return confirm('Are you sure you want delete this record');"
    '            AddHandler Lb.Click, AddressOf DelRecord
    '            TC = New TableCell
    '            TC.CssClass = "table_delete"
    '            TC.Controls.Add(Lb)
    '            Tr.Cells.Add(TC)
    '        End If
    '        tblData.Rows.Add(Tr)
    '    Next
    'End Sub
    Private Sub EditRecord(sender As Object, e As EventArgs)
        Dim ID As Integer
        ID = CType(sender, LinkButton).ID.Replace("LB_Order_", "")
        BO_ID.Value = ID
        EditManual()
    End Sub
    Private Sub EditManual()
        Dim DR As DataRow
        DR = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleRow(String.Format("select * from T_Batch_Orders where BO_ID={0}", BO_ID.Value))

        txtOrderNo.Text = DR.Item("Order_ID")
        chkPickUp.Checked = DR.Item("Pickup")
        If DR.Item("Service_Type") = "WG" Then
            ddlServiceTYpe.SelectedIndex = 0
        Else
            ddlServiceTYpe.SelectedIndex = 1
        End If
        txtClientName.Text = DR.Item("Client")
        TxtQty.Text = DR.Item("Qty")
        txtWt.Text = DR.Item("Wt")
        TxtCfts.Text = DR.Item("CFts")
        txtName.Text = DR.Item("Name")
        txtAddress1.Text = DR.Item("Address1")
        txtAddress2.Text = DR.Item("Address2")
        TxtZip.Text = DR.Item("Zip")
        TxtCity.Text = DR.Item("City")
        txtState.Text = DR.Item("State_")
        txtphone.Text = DR.Item("Phone")
        txtSI.Text = DR.Item("SpInstructions")
        txtTaskDetails.Text = DR.Item("Task_Details")
        pnlAddManual.Visible = True
        pnlData.Visible = False
    End Sub
    Private Sub lbAddDate_Click(sender As Object, e As EventArgs) Handles lbAddDate.Click
        PinnaclePlus.OrderBatch.P_Order_Sync(txtDate.Text, "")
        FillGrid(PinnaclePlus.OrderBatch.P_Order_Get_ByDate(txtDate.Text, "", True))
    End Sub

    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnReferesh.Click
        FillGrid(PinnaclePlus.OrderBatch.P_Order_Get_ByDate(txtDate.Text, "", True))
    End Sub

    Private Sub txtDate_TextChanged(sender As Object, e As EventArgs) Handles txtDate.TextChanged
        Dim Date_ As String = Request.Form(txtDate.UniqueID)
        FillGrid(PinnaclePlus.OrderBatch.P_Order_Get_ByDate(txtDate.Text, "", False))
    End Sub

    Private Sub lbLatLong_Click(sender As Object, e As EventArgs) Handles lbExporttoOnfleet.Click
        ExporttoOnfleet()
        FillGrid(PinnaclePlus.OrderBatch.P_Order_Get_ByDate(txtDate.Text, "", False))
    End Sub

    'Private Sub lbExcel_Click(sender As Object, e As EventArgs) Handles lbExcel.Click
    '    Dim DT As DataTable
    '    Dim DT_pin As DataTable
    '    Dim DT_Export As DataTable
    '    Dim DD As Date
    '    Dim Dr As DataRow
    '    Dim DrExport As DataRow
    '    Dim Pic_Del As String
    '    Dim OrderStr As String
    '    'Phone format='###-###-####'
    '    DT_Export = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect("select 0 as OrderNo,'' as Client,'' as [State],'' as Zip,'US' as Country,'' as Name,'' as Address1, '' as Address2,'' as longitude,'' as latitude,'' as SpecialInstructions,'' as Phone,	'' as City,'' as DeliveryHub,'' as Qty,'' as ItemDetail,'' as Time_After,'' as Time_To,0 as pickupTask ")
    '    DT_Export.Rows(0).Delete()
    '    DT_Export.AcceptChanges()
    '    DT = PinnaclePlus.OrderBatch.P_Batch_Orders_Get_ByDate(txtDate.Text, "")
    '    OrderStr = "'0'"
    '    For i = 0 To DT.Rows.Count - 1
    '        OrderStr = String.Format("{0},'{1}'", OrderStr, DT.Rows(i).Item("Order_ID"))
    '    Next
    '    DT_pin = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectPinnacle(String.Format("select * from V_Order_Date where OrderNo in ({0})", OrderStr))
    '    For i = 0 To DT.Rows.Count - 1
    '        If Not IsDBNull(DT.Rows(i).Item("OF_Task_ID")) Then
    '            Continue For
    '        End If
    '        Dr = DT_pin.Select(String.Format("[OrderNo]='{0}'", DT.Rows(i).Item("Order_ID")))(0)
    '        DrExport = DT_Export.NewRow
    '        If DT.Rows(i).Item("Pickup") Then
    '            Pic_Del = "Pic"
    '            DrExport.Item("pickupTask") = True
    '        Else
    '            Pic_Del = "Del"
    '            DrExport.Item("pickupTask") = False
    '        End If
    '        DrExport.Item("OrderNo") = DT.Rows(i).Item("Order_ID")
    '        DrExport.Item("Client") = Dr.Item("Client") 'Client
    '        DrExport.Item("State") = Dr.Item(String.Format("State_{0}", Pic_Del)) 'State
    '        DrExport.Item("Zip") = Dr.Item(String.Format("Zip_{0}", Pic_Del)) 'Zip
    '        DrExport.Item("Country") = "US" 'Country
    '        DrExport.Item("Name") = Dr.Item(String.Format("Name_{0}", Pic_Del)) 'Name
    '        DrExport.Item("Address1") = Dr.Item(String.Format("Address1_{0}", Pic_Del)) 'Address1
    '        DrExport.Item("Address2") = Dr.Item(String.Format("Address2_{0}", Pic_Del)) 'Address2
    '        DrExport.Item("longitude") = DT.Rows(i).Item("Longi") 'longitude
    '        DrExport.Item("latitude") = DT.Rows(i).Item("Lati") 'latitude
    '        DrExport.Item("SpecialInstructions") = Dr.Item("SpecialInstructions") 'SpecialInstructions
    '        If IsNumeric(Dr.Item(String.Format("Phone_{0}", Pic_Del))) Then
    '            DrExport.Item("Phone") = String.Format("{0:###-###-####}", Long.Parse(Dr.Item(String.Format("Phone_{0}", Pic_Del)))) 'Phone

    '        Else
    '            DrExport.Item("Phone") = "" 'Phone
    '        End If
    '        DrExport.Item("City") = Dr.Item(String.Format("City_{0}", Pic_Del)) 'City
    '        DrExport.Item("DeliveryHub") = "" 'DeliveryHub
    '        DrExport.Item("Qty") = Dr.Item("Qty") 'Qty
    '        DrExport.Item("ItemDetail") = Dr.Item("ItemDetail") 'ItemDetail,
    '        DD = CDate(Dr.Item(String.Format("Sch_Date_{0}", Pic_Del))).AddHours(8)
    '        DrExport.Item("Time_After") = Format(DD, "MM/dd/yyyy HH:mm") 'Time_After,
    '        DrExport.Item("Time_To") = Format(DD, "MM/dd/yyyy HH:mm") 'Time_To,
    '        DT_Export.Rows.Add(DrExport)
    '    Next
    '    ExportToExcel(txtDate.Text.Replace("/", "-"), "Sheet1", DT_Export)
    'End Sub
    Private Sub ExporttoOnfleet()


        Dim DT As DataTable
        Dim DT_pin As DataTable
        Dim Dr As DataRow
        Dim OrderStr As String
        Dim Team As String
        'Phone format='###-###-####'
        Dr = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleRow(String.Format("select Default_Team from T_HUB where Pinnacle_ID='{0}'", ""))
        If IsDBNull(Dr.Item(0)) Then
            litError.Text = "No Default Team Defined!"
            Exit Sub
        End If
        Team = Dr.Item(0)
        DT = PinnaclePlus.OrderBatch.P_Batch_Orders_Get_ByDate_NotExported(txtDate.Text, "")
        'OrderStr = "'0'"
        'For i = 0 To DT.Rows.Count - 1
        ' OrderStr = String.Format("{0},'{1}'", OrderStr, DT.Rows(i).Item("Order_ID"))
        'Next
        'DT_pin = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectPinnacle(String.Format("select * from V_Order_Date where OrderNo in ({0})", OrderStr))
        For i = 0 To DT.Rows.Count - 1

            ExporttoOnfleetSingle(DT.Rows(i), Team)
        Next

    End Sub

    Public Shared Function ExportToExcel(FileName As String, SheetName As String, data As DataTable) As Boolean
        Dim pck As ExcelPackage
        pck = New ExcelPackage()

        Try

            Dim ws = pck.Workbook.Worksheets.Add(SheetName)
            ws.Cells("A1").LoadFromDataTable(data, True)
            Dim excel = pck.GetAsByteArray()

            HttpContext.Current.Response.ClearHeaders()
            HttpContext.Current.Response.ClearContent()
            HttpContext.Current.Response.Clear() 'really clear it :-p
            HttpContext.Current.Response.BufferOutput = False
            HttpContext.Current.Response.ContentType = "application/octet-stream"
            HttpContext.Current.Response.AddHeader("cache-control", "max-age=0")
            HttpContext.Current.Response.AddHeader("Pragma", "public")
            HttpContext.Current.Response.AddHeader("Content-Length", excel.Length.ToString())
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;  filename=""" & FileName & ".xlsx""")

            HttpContext.Current.Response.BinaryWrite(excel)

            HttpContext.Current.Response.[End]()

            Return True
        Catch
            Return False
        Finally
            pck.Dispose()
        End Try
    End Function
    Private Sub ExporttoOnfleetSingle(DrLocal As DataRow, DefTeamID As String)
        Dim ObjTask As PinnaclePlus.onfleet.OnfleetTask
        Dim Meta As PinnaclePlus.onfleet.Metadata
        Dim MetaList As List(Of PinnaclePlus.onfleet.Metadata)
        Dim Addrs As New PinnaclePlus.onfleet.Address
        Dim Recipient As PinnaclePlus.onfleet.Recipient
        Dim Recipients As List(Of PinnaclePlus.onfleet.Recipient)
        Dim containr As PinnaclePlus.onfleet.container
        'Dim Loc As List(Of Double)
        Dim DestinationId As String
        DestinationId = PinnaclePlus.onfleet.OnfleetAPI.CreateDestination(String.Format("{0}, {1}, USA", DrLocal.Item("Address1"), DrLocal.Item("Zip")))
        If DestinationId.Substring(0, 5) = "ERROR" Then
            PinnaclePlus.SQLData.GeneralOperations.ExecuteNONQuery(String.Format("update T_Manifest_Order set of_error='{0}' where MO_ID={1}", "Address Invalid", DrLocal.Item("MO_ID")), "", "")
            Exit Sub
        End If
        Dim Pic_Del As String
        Dim JsonStr As String

        Dim DD As Date
        ObjTask = New PinnaclePlus.onfleet.OnfleetTask
        With ObjTask
            If DrLocal.Item("Is_Pickup") Then
                Pic_Del = "Pic"
                .pickupTask = True
            Else
                Pic_Del = "Del"
                .pickupTask = False
            End If
            Meta = New PinnaclePlus.onfleet.Metadata
            With Meta
                .name = "OrderNo"
                .type = "string"
                .value = DrLocal.Item("Order_ID")
            End With
            MetaList = New List(Of PinnaclePlus.onfleet.Metadata)
            .metadata = MetaList
            .metadata.Add(Meta)
            Meta = New PinnaclePlus.onfleet.Metadata
            With Meta
                .name = "Client"
                .type = "string"
                .value = DrLocal.Item("Company_Bill")
            End With
            .metadata.Add(Meta)
            Addrs = New PinnaclePlus.onfleet.Address

            'With Destination
            '    .address = Addrs
            '    .address.number = DrLocal.Item("Address1")
            '    '.address.street =
            '    .address.apartment = DrLocal.Item("Address2")
            '    .address.city = DrLocal.Item("City")
            '    .address.postalCode = DrLocal.Item("Zip")
            '    .address.state = DrLocal.Item("State_")
            '    .address.country = "US"
            '    'Loc = New List(Of Double)
            '    '.location = Loc
            '    '.location.Add(DrLocal.Item("Longi"))
            '    '.location.Add(DrLocal.Item("Lati"))
            'End With
            .destination = DestinationId
            Recipient = New PinnaclePlus.onfleet.Recipient
            With Recipient
                .name = DrLocal.Item("Name")
                If IsNumeric(DrLocal.Item("Phone")) Then
                    .phone = String.Format("{0:###-###-####}", Long.Parse(DrLocal.Item("Phone"))) 'Phone
                Else
                    .phone = "" 'Phone
                End If


                .notes = DrLocal.Item("SpInstructions")
            End With
            Recipients = New List(Of PinnaclePlus.onfleet.Recipient)
            .recipients = Recipients
            .recipients.Add(Recipient)
            .quantity = DrLocal.Item("Qty")
            .notes = DrLocal.Item("Task_Details")
            DD = CDate(DrLocal.Item("Date_")).AddHours(11).AddMinutes(59).AddSeconds(59).AddMilliseconds(1)
            .completeAfter = PinnaclePlusGlobals.GetUnixTime(DD)
            .completeBefore = .completeAfter
            'DrExport.Item("Time_After") = Format(DD, "MM/dd/yyyy HH:mm") 'Time_After,
            'DrExport.Item("Time_To") = Format(DD, "MM/dd/yyyy HH:mm") 'Time_To,
            containr = New PinnaclePlus.onfleet.container
            containr.type = "TEAM"
            containr.team = DefTeamID
            .container = containr
        End With
        JsonStr = JsonConvert.SerializeObject(ObjTask, New JsonSerializerSettings With {.NullValueHandling = NullValueHandling.Ignore})

        Dim strURL As String
        Dim myWebReq As HttpWebRequest
        Dim myWebResp As HttpWebResponse
        Dim encoding As New System.Text.UTF8Encoding
        Dim objStream As Stream
        Dim sr As StreamReader
        'getData__1 = getData__1 & strJSON


        strURL = "https://onfleet.com/api/v2/tasks"

        'myWebReq.GetResponse.ToString()

        myWebReq = DirectCast(WebRequest.Create(strURL), HttpWebRequest)
        myWebReq.ContentType = "application/json; charset=utf-8"

        'myWebReq.ContentLength = data.Le
        myWebReq.Method = "POST"
        myWebReq.KeepAlive = True
        Dim autorization As String = "" 'My.Settings.nv_xml_path
        Dim binaryAuthorization As Byte() = System.Text.Encoding.UTF8.GetBytes(autorization)
        autorization = Convert.ToBase64String(binaryAuthorization)
        autorization = "Basic " + autorization
        myWebReq.Headers.Add("AUTHORIZATION", autorization)


        If myWebReq.Proxy IsNot Nothing Then
            myWebReq.Proxy.Credentials = CredentialCache.DefaultCredentials
        End If
        Using myStream As Stream = myWebReq.GetRequestStream()

            'myStream = myWebReq.GetRequestStream()
            Dim data As Byte() = encoding.GetBytes(JsonStr)
            If data.Length > 0 Then
                myStream.Write(data, 0, data.Length)
                myStream.Close()
            End If
            Try
                myWebResp = DirectCast(myWebReq.GetResponse(), HttpWebResponse)
                objStream = myWebResp.GetResponseStream()
                sr = New StreamReader(objStream)
                JsonStr = sr.ReadToEnd()
                If myWebResp.StatusCode = HttpStatusCode.OK Then
                    ObjTask = JsonConvert.DeserializeObject(Of PinnaclePlus.onfleet.OnfleetTask)(JsonStr)
                    PinnaclePlus.SQLData.GeneralOperations.ExecuteNONQuery(String.Format("update T_Manifest_Order set of_id='{0}',of_error='',of_State={2} where MO_ID={1}", ObjTask.id, DrLocal.Item("MO_ID"), ObjTask.state), "", "")
                End If
                myWebResp.Close()
                myWebReq = Nothing
            Catch wex As WebException
                If wex.Response IsNot Nothing Then
                    myWebResp = DirectCast(wex.Response, HttpWebResponse)
                    objStream = myWebResp.GetResponseStream()
                    sr = New StreamReader(objStream)
                    JsonStr = sr.ReadToEnd()
                    Dim Objerr As PinnaclePlus.onfleet.ErrorResponse = JsonConvert.DeserializeObject(Of PinnaclePlus.onfleet.ErrorResponse)(JsonStr)
                    PinnaclePlus.SQLData.GeneralOperations.ExecuteNONQuery(String.Format("update T_Manifest_Order set of_error='{0}' where MO_ID={1}", Objerr.message.message, DrLocal.Item("MO_ID")), "", "")
                End If
            Catch ex As Exception

            End Try
        End Using
        System.Threading.Thread.Sleep(75)
    End Sub

    Private Sub UpdateTaskTime(TaskID As String, TimeJson As String)
        Dim strURL As String
        Dim myWebReq As HttpWebRequest
        Dim myWebResp As HttpWebResponse
        Dim encoding As New System.Text.UTF8Encoding
        Dim objStream As Stream
        Dim sr As StreamReader
        Dim retJson As String
        'getData__1 = getData__1 & strJSON


        strURL = String.Format("https://onfleet.com/api/v2/tasks/{0}", TaskID)

        'myWebReq.GetResponse.ToString()

        myWebReq = DirectCast(WebRequest.Create(strURL), HttpWebRequest)
        myWebReq.ContentType = "application/json; charset=utf-8"

        'myWebReq.ContentLength = data.Le
        myWebReq.Method = "PUT"
        myWebReq.KeepAlive = True
        Dim autorization As String = "" ' My.Settings.nv_xml_path
        Dim binaryAuthorization As Byte() = System.Text.Encoding.UTF8.GetBytes(autorization)
        autorization = Convert.ToBase64String(binaryAuthorization)
        autorization = "Basic " + autorization
        myWebReq.Headers.Add("AUTHORIZATION", autorization)


        If myWebReq.Proxy IsNot Nothing Then
            myWebReq.Proxy.Credentials = CredentialCache.DefaultCredentials
        End If
        Using myStream As Stream = myWebReq.GetRequestStream()

            'myStream = myWebReq.GetRequestStream()
            Dim data As Byte() = encoding.GetBytes(TimeJson)
            If data.Length > 0 Then
                myStream.Write(data, 0, data.Length)
                myStream.Close()
            End If
            Try
                myWebResp = DirectCast(myWebReq.GetResponse(), HttpWebResponse)
                objStream = myWebResp.GetResponseStream()
                sr = New StreamReader(objStream)
                retJson = sr.ReadToEnd()
                If myWebResp.StatusCode = HttpStatusCode.OK Then

                End If
                myWebResp.Close()
                myWebReq = Nothing
            Catch wex As WebException
                If wex.Response IsNot Nothing Then
                    'myWebResp = DirectCast(wex.Response, HttpWebResponse)
                    'objStream = myWebResp.GetResponseStream()
                    'sr = New StreamReader(objStream)
                    'retJson = sr.ReadToEnd()
                    'Dim Objerr As PinnaclePlus.onfleet.ErrorResponse = JsonConvert.DeserializeObject(Of PinnaclePlus.onfleet.ErrorResponse)(retJson)
                    litError.Text = "Error Updating Time on Onfleet"
                    litError.ValidateRequestMode = True
                    Exit Sub
                End If
            Catch ex As Exception

            End Try
        End Using
        System.Threading.Thread.Sleep(75)
    End Sub
    Private Sub lbGetAssignment_Click(sender As Object, e As EventArgs) Handles lbGetAssignment.Click
        CreateMinifest()
        
    End Sub
    Private Sub CreateMinifest()
        Dim DT As DataTable
        Dim ReAddas As String = "", PreZip As String = ""
        DT = PinnaclePlus.OrderBatch.P_Batch_Orders_Get_ByDate_NotExported(txtDate.Text, "")
        If DT.Rows.Count = 1 Then
            litError.Text = "There is a task that has not been exported to Onfleet, first export that task to Onfleet!"
            litError.Visible = True
            Exit Sub
        ElseIf DT.Rows.Count > 1 Then
            litError.Text = String.Format("There are {0} tasks that have not been exported to Onfleet, first export all tasks to Onfleet!", DT.Rows.Count)
            litError.Visible = True
            Exit Sub
        End If
        Dim Wr As PinnaclePlus.onfleet.Worker
        Dim Task As PinnaclePlus.onfleet.OnfleetTask
        Dim stopNo As Integer
        Dim MO_ID As Integer = 0
        Dim cnt As Integer = 0
        DT = PinnaclePlus.OrderBatch.P_Order_Get_ByDate(txtDate.Text, "", False)
        For i = 0 To DT.Rows.Count - 1
            Task = PinnaclePlus.onfleet.OnfleetAPI.GetTask(DT.Rows(i).Item("of_id"))
            If Task.worker <> "" Then
                Wr = PinnaclePlus.onfleet.OnfleetAPI.GetWorker(Task.worker)
                'PinnaclePlus.WorkerOperations.P_Worker_IU(Wr)
                stopNo = 1
                For Each taskid In Wr.tasks
                    If taskid = Task.id Then
                        PinnaclePlus.SQLData.GeneralOperations.ExecuteNONQuery(String.Format("update T_Manifest_Order set worker_id='{0}',of_stop={1},of_State={2} where MO_ID={3}", Task.worker, stopNo, Task.state, DT.Rows(i).Item("MO_ID")), "", "")
                    End If

                    stopNo = stopNo + 1

                Next
            Else
                MO_ID = DT.Rows(i).Item("MO_ID")
                cnt = cnt + 1
            End If
        Next
        If MO_ID <> 0 Then
            PinnaclePlus.OrderBatch.P_Manifest_Clear_Stops(MO_ID)
            If cnt = 1 Then
                FillGrid(PinnaclePlus.OrderBatch.P_Order_Get_ByDate(txtDate.Text, "", False))
                litError.Text = "There is an unassigned task on Onfleet, first assign that task on Onfleet!"
                litError.Visible = True
            Else
                FillGrid(PinnaclePlus.OrderBatch.P_Order_Get_ByDate(txtDate.Text, "", False))
                litError.Text = String.Format("There are {0} unassigned tasks on Onfleet, first assign all tasks on Onfleet!", cnt)
                litError.Visible = True
            End If
        Else
            PinnaclePlus.OrderBatch.P_Manifest_Create(txtDate.Text, "")
            PinnaclePlus.OrderBatch.P_Manifest_Create(txtDate.Text, "")
            UpdateTimeonOF()
            FillGrid(PinnaclePlus.OrderBatch.P_Order_Get_ByDate(txtDate.Text, "", False))
        End If


    End Sub
    Private Sub UpdateTimeonOF()
        Dim DT As DataTable
        Dim DD As DateTime
        Dim St, En As Long
        Dim JsonTime As String
        DT = PinnaclePlus.OrderBatch.P_Order_Get_ByDate(txtDate.Text, "", False)
        For i = 0 To DT.Rows.Count - 1
            DD = CDate(DT.Rows(i).Item("Time_from")).AddHours(4)
            St = PinnaclePlusGlobals.GetUnixTime(DD)
            DD = CDate(DT.Rows(i).Item("Time_to")).AddHours(4)
            En = PinnaclePlusGlobals.GetUnixTime(DD)
            JsonTime = String.Format("{{""completeAfter"": {0},  ""completeBefore"": {1}}}", St, En)
            UpdateTaskTime(DT.Rows(i).Item("of_id"), JsonTime)
        Next
    End Sub

    Private Sub lbTaskSheet_Click(sender As Object, e As EventArgs) Handles lbTaskSheet.Click
        PinnaclePlusGlobals.GetTaskSheet(txtDate.Text, "")
    End Sub

    Private Sub lbPullSheet_Click(sender As Object, e As EventArgs) Handles lbPullSheet.Click
        PinnaclePlusGlobals.GetPullSheet(txtDate.Text, "")
    End Sub

    'Private Sub lbManualOrder_Click(sender As Object, e As EventArgs) Handles lbManualOrder.Click
    '    pnlAddManual.Visible = True
    '    pnlData.Visible = False
    '    BO_ID.Value = 0
    'End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        pnlAddManual.Visible = False
        pnlData.Visible = True
    End Sub

    'Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
    '    Dim Lati, Longi As Object
    '    Dim DR As DataRow
    '    Dim State, Zip, Address1 As String
    '    Dim LL As PinnaclePlus.Google.GooglePlace
    '    If BO_ID.Value <> 0 Then
    '        DR = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleRow(String.Format("select Address1,State_,Zip,Lati,Longi from T_Batch_Orders where BO_ID={0}", BO_ID.Value))
    '        Address1 = DR.Item("Address1")
    '        Zip = DR.Item("Zip")
    '        State = DR.Item("State_")
    '        If Not (Address1.Trim.ToLower = txtAddress1.Text.Trim.ToLower And Zip = TxtZip.Text And State.Trim.ToLower = txtState.Text.Trim.ToLower) Then
    '            LL = Nothing 'PinnaclePlus.Google.GoogleApi.FindCoordinates(txtAddress1.Text, TxtZip.Text, txtState.Text)
    '        Else
    '            LL = New PinnaclePlus.Google.GooglePlace
    '            LL.Lati = DR.Item("Lati")
    '            LL.Longi = DR.Item("Longi")
    '        End If
    '    Else
    '        LL = Nothing 'PinnaclePlus.Google.GoogleApi.FindCoordinates(txtAddress1.Text, TxtZip.Text, txtState.Text)
    '    End If


    '    If (LL Is Nothing) Then
    '        Lati = DBNull.Value
    '        Longi = DBNull.Value
    '        litError.Text = "Invalid Address!"
    '        litError.Visible = True
    '        Exit Sub
    '    Else
    '        Lati = LL.Lati
    '        Longi = LL.Longi
    '    End If
    '    PinnaclePlus.OrderBatch.P_Batch_Orders_IU(BO_ID.Value, True, _
    '                                                  txtClientName.Text, _
    '                                                  txtOrderNo.Text, _
    '                                                  chkPickUp.Checked, txtDate.Text, _
    '                                                  "", _
    '                                                  ddlServiceTYpe.SelectedValue, _
    '                                                  TxtQty.Text, _
    '                                                  txtWt.Text, _
    '                                                  TxtCfts.Text, _
    '                                                  txtName.Text, _
    '                                                  txtAddress1.Text, _
    '                                                  txtAddress2.Text, _
    '                                                  TxtZip.Text, _
    '                                                  TxtCity.Text, _
    '                                                  txtState.Text, _
    '                                                  txtphone.Text, _
    '                                                  txtSI.Text, _
    '                                                  txtTaskDetails.Text, _
    '                                                  Lati, Longi)

    '    pnlAddManual.Visible = False
    '    pnlData.Visible = True
    '    FillGrid(PinnaclePlus.OrderBatch.P_Order_Get_ByDate(txtDate.Text, "", False))
    'End Sub

    Private Sub gvData_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvData.RowCommand
        If e.CommandName = "CmdDel" Then
            Dim Ts As PinnaclePlus.onfleet.OnfleetTask
            Dim DR As DataRow
            Dim L_MO_ID As Integer = Convert.ToInt32(gvData.DataKeys(e.CommandArgument).Values(0))
            DR = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleRow(String.Format("select * from T_Manifest_Order where MO_ID={0}", L_MO_ID))
            If Not IsDBNull(DR.Item("of_id")) Then
                Ts = PinnaclePlus.onfleet.OnfleetAPI.GetTask(DR.Item("of_id"))
                If Ts IsNot Nothing Then
                    litError.Text = "Task exists on Onfleet, first delete from Onfleet!"
                    litError.Visible = True
                    Exit Sub
                End If
            End If
            Dim Ret As Integer = PinnaclePlus.OrderBatch.T_Manifest_Order_Del(L_MO_ID)
            If Ret = 1 Then
                FillGrid(PinnaclePlus.OrderBatch.P_Order_Get_ByDate(txtDate.Text, "", False))
            Else
                litError.Text = "Cannot delete, child records found, first delete them!"
                litError.Visible = True
            End If

        End If

    End Sub

    Private Sub gvData_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvData.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim rowView As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim DR As DataRow

            Dim L_MO_ID As Integer = rowView("MO_ID")
            DR = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleRow(String.Format("select * from T_Manifest_Order where MO_ID={0}", L_MO_ID))
            If DR.Item("Source_") <> 1 Then
                Dim Lb As LinkButton
                Lb = e.Row.Cells(2).FindControl("lbOrderNo")
                Lb.ID = String.Format("LB_Order_{0}", DR.Item("BO_ID"))
                Lb.Text = String.Format("M-{0}", DR.Item("Order_ID"))
                'AddHandler Lb.Click, AddressOf EditRecord
                Lb.Visible = True
            Else
                Dim HL As HyperLink
                HL = e.Row.Cells(2).FindControl("hlOrderNo")
                HL.Text = DR.Item("Order_ID")
                Dim UrlStr As String
                UrlStr = String.Format("OrderId={0}", DR.Item("Order_ID"))
                HL.NavigateUrl = "http://main.metropolitanwarehouse.com/Order/OrderDetail?eqs=" + PinnacleFunction.URLEncrypt.EncryptDesToHex(UrlStr, "MetroCryptoUSA07306#?+")
                HL.Target = "_blank"
                HL.Visible = True
            End If
            Dim Img As Image = e.Row.Cells(2).FindControl("imgPicDel")
            If DR.Item("Is_Pickup") Then
                Img.ImageUrl = "~/Styles/images/Pic.png"
            Else
                Img.ImageUrl = "~/Styles/images/Del.png"
            End If
            Dim DelLink As LinkButton = e.Row.Cells(10).Controls(0)
            DelLink.OnClientClick = "return confirm('Are you sure you want to delete this record?');"

        End If
    End Sub
End Class