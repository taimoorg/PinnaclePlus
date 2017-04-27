Imports Newtonsoft.Json
Imports System.Net
Imports System.IO
Imports OfficeOpenXml
Imports PinnacleFunction
Public Class nv_export
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
        ExporttonuVizz()

        FillGrid(PinnaclePlus.OrderBatch.P_Order_Get_ByDate(txtDate.Text, "", False))
    End Sub

    
    Private Sub ExporttonuVizz()
        
        'Dim OrderXml As String
        'OrderXml = PinnaclePlus.nuVizz.nuVizzAPI.GetXml(txtDate.Text, "")
        'PinnaclePlus.nuVizz.nuVizzAPI.ExportStopsToNuvizz(OrderXml)


    End Sub
    Private Sub SnycnuVizz()
        Dim loadId As String
        Dim loadNbr As String
        Dim DT As DataTable
        Dim WORKER_ID As String
        Dim nvStop As PinnaclePlus.nuVizz.nuVizzStop
        DT = PinnaclePlus.OrderBatch.P_Batch_Orders_Get_ByDate(txtDate.Text, "")
        For i = 0 To DT.Rows.Count - 1
            nvStop = PinnaclePlus.nuVizz.nuVizzAPI.GetStop(DT.Rows(i).Item("MO_ID"))
            If nvStop IsNot Nothing Then
                If nvStop.driverInfo IsNot Nothing Then
                    WORKER_ID = PinnaclePlus.WorkerOperations.P_Worker_IU(nvStop.driverInfo.firstName, nvStop.driverInfo.lastName, nvStop.driverInfo.phoneNumber, "")
                Else
                    WORKER_ID = "null"
                End If
                If nvStop.loadId Is Nothing Then
                    loadId = "null"
                    loadNbr = ""
                    
                Else
                    loadId = nvStop.loadId.loadHeader.loadId
                    loadNbr = nvStop.loadId.loadHeader.loadNbr
                End If

                PinnaclePlus.SQLData.GeneralOperations.ExecuteNONQuery(String.Format("update T_Manifest_Order set of_id='{0}',of_error='',of_State={2},worker_id={3},of_stop={4},load_id={5}, load_number='{6}' where MO_ID={1}", nvStop.stopId, DT.Rows(i).Item("MO_ID"), "0", WORKER_ID, nvStop.stopSeq, loadId, loadNbr), "", "")
            End If
        Next
        PinnaclePlus.OrderBatch.P_Manifest_Create(txtDate.Text, "")
        PinnaclePlus.OrderBatch.P_Manifest_Create(txtDate.Text, "")
        UpdateTimeonOF()
        ExporttonuVizz()
        FillGrid(PinnaclePlus.OrderBatch.P_Order_Get_ByDate(txtDate.Text, "", False))
    End Sub

    Private Sub lbGetAssignment_Click(sender As Object, e As EventArgs) Handles lbGetAssignment.Click
        'CreateMinifest()
        SnycnuVizz()
    End Sub
    'Private Sub CreateMinifest()
    '    Dim DT As DataTable
    '    Dim ReAddas As String = "", PreZip As String = ""
    '    DT = PinnaclePlus.OrderBatch.P_Batch_Orders_Get_ByDate_NotExported(txtDate.Text, "")
    '    If DT.Rows.Count = 1 Then
    '        litError.Text = "There is a task that has not been exported to Onfleet, first export that task to Onfleet!"
    '        litError.Visible = True
    '        Exit Sub
    '    ElseIf DT.Rows.Count > 1 Then
    '        litError.Text = String.Format("There are {0} tasks that have not been exported to Onfleet, first export all tasks to Onfleet!", DT.Rows.Count)
    '        litError.Visible = True
    '        Exit Sub
    '    End If
    '    Dim Wr As PinnaclePlus.onfleet.Worker
    '    Dim Task As PinnaclePlus.onfleet.OnfleetTask
    '    Dim stopNo As Integer
    '    Dim MO_ID As Integer = 0
    '    Dim cnt As Integer = 0
    '    DT = PinnaclePlus.OrderBatch.P_Order_Get_ByDate(txtDate.Text, "", False)
    '    For i = 0 To DT.Rows.Count - 1
    '        Task = PinnaclePlus.onfleet.OnfleetAPI.GetTask(DT.Rows(i).Item("of_id"))
    '        If Task.worker <> "" Then
    '            Wr = PinnaclePlus.onfleet.OnfleetAPI.GetWorker(Task.worker)
    '            PinnaclePlus.WorkerOperations.P_Worker_IU(Wr)
    '            stopNo = 1
    '            For Each taskid In Wr.tasks
    '                If taskid = Task.id Then
    '                    PinnaclePlus.SQLData.GeneralOperations.ExecuteNONQuery(String.Format("update T_Manifest_Order set worker_id='{0}',of_stop={1},of_State={2} where MO_ID={3}", Task.worker, stopNo, Task.state, DT.Rows(i).Item("MO_ID")), "", "")
    '                End If

    '                stopNo = stopNo + 1

    '            Next
    '        Else
    '            MO_ID = DT.Rows(i).Item("MO_ID")
    '            cnt = cnt + 1
    '        End If
    '    Next
    '    If MO_ID <> 0 Then
    '        PinnaclePlus.OrderBatch.P_Manifest_Clear_Stops(MO_ID)
    '        If cnt = 1 Then
    '            FillGrid(PinnaclePlus.OrderBatch.P_Order_Get_ByDate(txtDate.Text, "", False))
    '            litError.Text = "There is an unassigned task on Onfleet, first assign that task on Onfleet!"
    '            litError.Visible = True
    '        Else
    '            FillGrid(PinnaclePlus.OrderBatch.P_Order_Get_ByDate(txtDate.Text, "", False))
    '            litError.Text = String.Format("There are {0} unassigned tasks on Onfleet, first assign all tasks on Onfleet!", cnt)
    '            litError.Visible = True
    '        End If
    '    Else
    '        PinnaclePlus.OrderBatch.P_Manifest_Create(txtDate.Text, "")
    '        PinnaclePlus.OrderBatch.P_Manifest_Create(txtDate.Text, "")
    '        'UpdateTimeonOF()
    '        FillGrid(PinnaclePlus.OrderBatch.P_Order_Get_ByDate(txtDate.Text, "", False))
    '    End If


    'End Sub
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
            'UpdateTaskTime(DT.Rows(i).Item("of_id"), JsonTime)
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