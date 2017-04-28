
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.IO
Imports System.Web.Services
Imports CrystalDecisions.Shared.Json
Imports PinnaclePlus.SQLData

Public Class dispatch_data
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            FillHubs()
        End If
    End Sub
    Private Sub FillHubs()
        Dim DT As DataTable
        Dim LI As ListItem
        Dim LIE As ListItem
        DT = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect(String.Format("Select * from T_Hub where HUB_CODE in (select HUB_CODE from T_Users_Hub where USER_ID='{0}')", PinnaclePlus.Security.CurrentUser.ID))
        LI = New ListItem
        LI.Text = "All"
        LI.Value = ""
        ddlHub.Items.Add(LI)
        For i = 0 To DT.Rows.Count - 1
            LI = New ListItem
            LI.Text = DT.Rows(i).Item("Name")
            LI.Value = DT.Rows(i).Item("HUB_CODE")
            ddlHub.Items.Add(LI)
        Next

    End Sub
#Region "Filters"
    Private Sub FillGrid()
        Dim StrSQL As String
        Dim startDate As String
        Dim enddate As String
        startDate = txtDateFrom.Text
        enddate = txtDateTo.Text
        Dim dt As DataTable
        'StrSQL = "select *,dbo.F_StopHasException(MSO_ID) as HasException,(select [Company_Bill] FROM [dbo].[V_Order]WHERE[OrderNo] = CAST(Order_ID AS NVARCHAR(20))) as Client,( SELECT  CASE WHEN is_pickup = 1 THEN [Email_Pic] ELSE [Email_Del] End FROM [dbo].[V_Order] WHERE [OrderNo] = CAST(Order_ID AS NVARCHAR(20)))AS Email from V_Manifest_Stop_Order mso LEFT OUTER JOIN dbo.T_Manifest_Stop_Order_Event msoe  ON  mso.MSO_ID=msoe.MSO_ID LEFT OUTER JOIN dbo.T_Manifest_Stop_Order_Event_Doc msoed ON msoe.MSOE_ID=msoed.MSOE_ID where 1=1 "
        StrSQL = "select  *,CASE is_Pickup WHEN 1 THEN 'Yes'  ELSE 'NO' End as Pickup,dbo.F_StopHasException(MSO_ID) as HasException,(select [Company_Bill] FROM [dbo].[V_Order]WHERE[OrderNo] = CAST(Order_ID AS NVARCHAR(20))) as Client,( SELECT  CASE WHEN is_pickup = 1 THEN [Email_Pic] ELSE [Email_Del] End FROM [dbo].[V_Order] WHERE [OrderNo] = CAST(Order_ID AS NVARCHAR(20)))AS Email from V_Manifest_Stop_Order  where 1=1 "
        If txtLoadNo.Text.Trim <> "" Then
            StrSQL = String.Format("{0} and [MIN_ID]={1}", StrSQL, txtLoadNo.Text.Trim)
        End If
        If ddlHub.SelectedItem.Value <> "" Then
            StrSQL = String.Format("{0} and HUB='{1}'", StrSQL, ddlHub.SelectedItem.Value)
        End If
        If txtPoMNo.Text.Trim <> "" Then
            StrSQL = String.Format("{0} and Order_ID={1}", StrSQL, txtPoMNo.Text.Trim)
        End If
        If txtDateFrom.Text.Trim <> "" Then
            StrSQL = String.Format("{0} and CAST(StartDate AS DATE)=CAST('{1}' as Date)", StrSQL, startDate)
        End If
        If txtDateTo.Text.Trim <> "" Then
            StrSQL = String.Format("{0} and CAST(StartDate AS DATE)=CAST('{1}' as Date)", StrSQL, enddate)
        End If
        If ddlException.Text.Trim <> "" Then
            StrSQL = String.Format("{0} and dbo.F_StopHasException(MSO_ID)= {1}", StrSQL, ddlException.SelectedItem.Value)
        End If
        dt = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect(StrSQL)
        gvPara.DataSource = dt
        gvPara.DataBind()

    End Sub
    Protected Sub btnfilter_Click(sender As Object, e As EventArgs)
        FillGrid()
    End Sub
#End Region
#Region "Export to Excel"
    Protected Sub btntoExcel_OnClick(sender As Object, e As EventArgs)
        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"
        Using sw As New StringWriter()
            Dim hw As New HtmlTextWriter(sw)

            'To Export all pages
            gvPara.AllowPaging = False
            Me.FillGrid()

            gvPara.HeaderRow.BackColor = Color.White
            For Each cell As TableCell In gvPara.HeaderRow.Cells
                cell.BackColor = gvPara.HeaderStyle.BackColor
            Next
            For Each row As GridViewRow In gvPara.Rows
                row.BackColor = Color.White
                For Each cell As TableCell In row.Cells
                    If row.RowIndex Mod 2 = 0 Then
                        cell.BackColor = gvPara.AlternatingRowStyle.BackColor
                    Else
                        cell.BackColor = gvPara.RowStyle.BackColor
                    End If
                    cell.CssClass = "textmode"
                Next
            Next

            gvPara.RenderControl(hw)
            'style to format numbers to string
            Dim style As String = "<style> .textmode { } </style>"
            Response.Write(style)
            Response.Output.Write(sw.ToString())
            Response.Flush()
            Response.[End]()
        End Using
    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        ' Verifies that the control is rendered
    End Sub
#End Region
#Region "Row DataBound and Row Command"
    Private Sub gvPara_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvPara.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            'Dim DT As DataTable
            Dim rowView As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim LB, LB1 As HyperLink
            LB = e.Row.Cells(5).FindControl("hl_Exception")


            If rowView("HasException") Then
                LB.Text = "Exception"
                LB.ForeColor = Color.White
                LB.NavigateUrl = String.Format("javascript:ShowException({0});", rowView("MSO_ID"))
                e.Row.BackColor = Color.Red
                e.Row.ForeColor = Color.White
            Else
                LB.Text = "No Exception"
                LB.NavigateUrl = "#"
            End If
            Dim HL As HyperLink
            HL = e.Row.Cells(2).FindControl("hl_POM")
            HL.Text = rowView.Item("Order_ID")
            Dim UrlStr As String
            UrlStr = String.Format("OrderId={0}", rowView.Item("Order_ID"))
            HL.NavigateUrl = "http://main.metropolitanwarehouse.com/Order/OrderDetail?eqs=" + PinnacleFunction.URLEncrypt.EncryptDesToHex(UrlStr, "MetroCryptoUSA07306#?+")
            HL.Target = "_blank"
            HL.Visible = True

            Dim HLID As HyperLink
            HLID = e.Row.Cells(2).FindControl("HL_MSOED_ID")
            HLID.Text = "Doc. Details"
            HLID.NavigateUrl = String.Format("javascript:ShowManifestFiles({0});", rowView("MSO_ID"))
        End If
    End Sub
    <WebMethod()> _
    Public Shared Function P_ExceptionAgainst_Menifest(MSO_ID As Integer) As String
        Dim DR As DataTable
        Dim wholeException As String
        DR = Dispatch.P_ExceptionAgainst_Menifest(MSO_ID)
        If DR.Rows.Count >= 1 Then
            wholeException = DR.Rows.Item(0).Item("Exception_Text") + " " + DR.Rows.Item(0).Item("Comments")
            Return wholeException

        Else
            wholeException = "Exception not defined"
            Return wholeException
        End If




    End Function
    <WebMethod()> _
    Public Shared Function P_DetailsAgainst_Menifest(MSO_ID As Integer) As String
        Dim DT As DataTable
        Dim HtmlStr As String
        DT = Dispatch.P_DetailsAgainst_Menifest(MSO_ID)
        If DT.Rows.Count >= 1 Then
            HtmlStr = "<ul>"
            For i = 0 To DT.Rows.Count - 1
                'HtmlStr = String.Format("{0}<li><a href=""getdoc.aspx?ID={2}""  target=""_blank"">{1}</a></li>", HtmlStr, DT.Rows(i).Item("StopSigneeName"), DT.Rows(i).Item("MSOED_ID"))
                HtmlStr = String.Format("{0}<li><a href=""javascript:P_DownloadFilesAgainst_Menifest({2})"" >{1}</a></li>", HtmlStr, DT.Rows(i).Item("documentname"), DT.Rows(i).Item("MSOED_ID"))
                'HtmlStr = String.Format("<li>{0}</li>", HtmlStr, DT.Rows(i).Item("documentname"))

            Next
            HtmlStr = String.Format("{0}</ul>", HtmlStr)
            Return HtmlStr
        Else
            HtmlStr = "No Document"
        End If


    End Function
    <WebMethod()> _
    Public Shared Function P_DownloadFilesAgainst_Menifest(MSOED_ID As Integer) As String

        PinnaclePlusGlobals.GetDocument(MSOED_ID)
        Return "Executed"
    End Function
    Protected Sub GridView1_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvPara.PageIndexChanging
        gvPara.PageIndex = e.NewPageIndex
        FillGrid()
    End Sub
#End Region
End Class