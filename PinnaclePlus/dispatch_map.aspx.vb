Imports OfficeOpenXml
Public Class dispatch_map
    Inherits System.Web.UI.Page
    '[MetroPolitanNavProduction].[dbo].[Metropolitan$Truck Location]
    '[MetroPolitanNavProduction].[dbo].[Metropolitan$Driver] 1000 driver 2000 co-driver
    '[MetroPolitanNavProduction].[dbo].[Metropolitan$TR Warehouse Hub]
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            FillHubs()
        End If
    End Sub
    Private Sub FillHubs()
        Dim DT As DataTable
        Dim LI As ListItem
        DT = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect(String.Format("Select * from T_Hub where HUB_CODE in (select HUB_CODE from T_Users_Hub where USER_ID='{0}')", PinnaclePlus.Security.CurrentUser.ID))
        For i = 0 To DT.Rows.Count - 1
            LI = New ListItem
            LI.Text = String.Format("{0} ({1})", DT.Rows(i).Item("Name"), DT.Rows(i).Item("HUB_CODE"))
            LI.Value = DT.Rows(i).Item("HUB_CODE")
            ddlHub.Items.Add(LI)
        Next
    End Sub

    Private Sub lbPullSheetThis_Click(sender As Object, e As EventArgs) Handles lbPullSheetThis.Click
        PinnaclePlusGlobals.GetPullSheetMin(MIN_ID.Value, ddlHub.SelectedValue)
    End Sub

    Private Sub lbTaskSheetThis_Click(sender As Object, e As EventArgs) Handles lbTaskSheetThis.Click
        PinnaclePlusGlobals.GetTaskSheetForMin(MIN_ID.Value, ddlHub.SelectedValue)
    End Sub

    Private Sub lbTaskSheetWithoutTicketsThis_Click(sender As Object, e As EventArgs) Handles lbTaskSheetWithoutTicketsThis.Click
        PinnaclePlusGlobals.GetTaskSheetForMin(MIN_ID.Value, ddlHub.SelectedValue, False)
    End Sub
    Private Sub lbPullSheet_Click(sender As Object, e As EventArgs) Handles lbPullSheet.Click
        PinnaclePlusGlobals.GetPullSheet(txtDate.Text, ddlHub.SelectedValue)
    End Sub

    Private Sub lbTaskSheetXL_Click(sender As Object, e As EventArgs) Handles lbTaskSheetXL.Click
        Dim DT As DataTable
        DT = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect(String.Format("SELECT         [MIN_ID],[Hub],[StartDate],[Tot_Mils],[Order_ID],[Seq],[Name]      ,[Zip]      ,[City]      ,[State]      ,[Phone]	  ,[Type]      ,[Type_Code]      ,[Sub_Type_Code]      ,[Qty]      ,[Wt]      ,[CuFts]      ,[Is_Pickup]      ,[Hub_Name]      ,[TruckName]      ,[CoDriver2Name]      ,[CoDriver1Name]      ,[DriverName]      ,[Notes]      ,[ETA]      ,[Run_Date]      ,[Tot_CuTfs]      ,[Tot_Qty]      ,[Tot_Stops]      ,[Tot_Wt]  FROM [dbo].[V_RPT_ManSheet] where Hub='{1}' and cast(StartDate as date)=cast('{0}' as date) order by  MIN_ID,SEQ", txtDate.Text, ddlHub.SelectedValue))
        Dim pck As ExcelPackage
        pck = New ExcelPackage()

        Try

            Dim ws = pck.Workbook.Worksheets.Add("Sheet 1")
            ws.Cells("A1").LoadFromDataTable(DT, True)
            Dim excel = pck.GetAsByteArray()

            HttpContext.Current.Response.ClearHeaders()
            HttpContext.Current.Response.ClearContent()
            HttpContext.Current.Response.Clear() 'really clear it :-p
            HttpContext.Current.Response.BufferOutput = False
            HttpContext.Current.Response.ContentType = "application/octet-stream"
            HttpContext.Current.Response.AddHeader("cache-control", "max-age=0")
            HttpContext.Current.Response.AddHeader("Pragma", "public")
            HttpContext.Current.Response.AddHeader("Content-Length", excel.Length.ToString())
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;  filename=""" & String.Format("Manifest-{0}-{1}-{2}-{3}", ddlHub.SelectedValue, CDate(txtDate.Text).Year, CDate(txtDate.Text).Month, CDate(txtDate.Text).Day) & ".xlsx""")

            HttpContext.Current.Response.BinaryWrite(excel)

            HttpContext.Current.Response.[End]()


        Catch
        Finally
            pck.Dispose()

        End Try
        'PinnaclePlusGlobals.GetTaskSheetXL()
    End Sub
    Private Sub lbTaskSheet_Click(sender As Object, e As EventArgs) Handles lbTaskSheet.Click

        PinnaclePlusGlobals.GetTaskSheetNew(txtDate.Text, ddlHub.SelectedValue)
    End Sub
    Private Sub lbTaskSheetWithoutTickets_Click(sender As Object, e As EventArgs) Handles lbTaskSheetWithoutTickets.Click
        PinnaclePlusGlobals.GetTaskSheetNew(txtDate.Text, ddlHub.SelectedValue, False)
    End Sub
End Class