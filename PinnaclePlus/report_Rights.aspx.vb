Imports System.Diagnostics.Eventing.Reader
Imports System.DirectoryServices.AccountManagement

'Public Class report_Rights
'    Inherits System.Web.UI.Page
'    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
'        If Not Page.IsPostBack Then
'            FillReports()
'        End If

'    End Sub
'    Private Sub FillReports()
'        Dim DT As DataTable
'        Dim LI As ListItem
'        DT = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect(String.Format("select R.*, Cnt=isnull((select Used_Count from T_Users_Report where REP_ID=R.REP_ID and [USER_ID]='{0}'),0) from T_Report R where REP_ID in (select REP_ID from T_Users_Report where USER_ID='{0}') or {1}", PinnaclePlus.Security.CurrentUser.ID, IIf(PinnaclePlus.Security.CurrentUser.Is_Admin, "1=1", "0=1 order by Cnt Desc")))
'        For i = 0 To DT.Rows.Count - 1
'            LI = New ListItem
'            LI.Text = DT.Rows(i).Item("Name")
'            LI.Value = DT.Rows(i).Item("REP_ID")
'            ddlReports.Items.Add(LI)
'        Next
'        FillPara()
'    End Sub
'    Private Sub ddlReports_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlReports.SelectedIndexChanged
'        FillPara()
'    End Sub
'    Private Sub FillPara()
'        If ddlReports.SelectedIndex < 0 Then
'            Exit Sub
'        End If
'        Dim DT As DataTable
'        Dim count As Integer
'        Dim LI As ListItem
'        DT = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect(String.Format("select u.[USER_ID],(select count([USER_ID]) from T_Users_Report where REP_ID={0} and u.[USER_ID]=[USER_ID]) Has from  [dbo].[T_Users] u where Is_Admin=0", ddlReports.SelectedValue))
'        chkUsers.DataSource = DT
'        chkUsers.Items.Clear()
'        count = DT.Rows.Count
'        For i = 0 To DT.Rows.Count - 1
'            LI = New ListItem
'            LI.Text = DT.Rows(i).Item("USER_ID")
'            If DT.Rows(i).Item("Has") = 1 Then
'                LI.Selected = True
'            Else
'                LI.Selected = False
'            End If
'            chkUsers.Items.Add(LI)

'        Next

'    End Sub
'    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
'        Dim Has As Boolean
'        For i = 0 To chkUsers.Items.Count - 1
'            Has = chkUsers.Items(i).Selected
'            PinnaclePlus.SQLData.Reports.P_Users_Report_ID(chkUsers.Items(i).Text, ddlReports.SelectedValue, Has)
'        Next
'        FillReports()
'        FillPara()
'    End Sub

'End Class

Public Class report_Rights
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            FillReports()
        End If

    End Sub
    Private Sub FillReports()
        Dim DT As DataTable
        Dim LI As ListItem
        DT = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect(String.Format("select R.*, Cnt=isnull((select Used_Count from T_Users_Report where REP_ID=R.REP_ID and [USER_ID]='{0}'),0) from T_Report R where REP_ID in (select REP_ID from T_Users_Report where USER_ID='{0}') or {1}", PinnaclePlus.Security.CurrentUser.ID, IIf(PinnaclePlus.Security.CurrentUser.Is_Admin, "1=1", "0=1 order by Cnt Desc")))
        For i = 0 To DT.Rows.Count - 1
            LI = New ListItem
            LI.Text = DT.Rows(i).Item("Name")
            LI.Value = DT.Rows(i).Item("REP_ID")
            ddlReports.Items.Add(LI)
        Next
        FillPara()
    End Sub
    Private Sub ddlReports_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlReports.SelectedIndexChanged
        FillPara()
    End Sub
    Private Sub FillPara()
        If ddlReports.SelectedIndex < 0 Then
            Exit Sub
        End If
        Dim DT As DataTable
        Dim count As Integer
        Dim LI As ListItem
        'DT = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect(String.Format("select u.[USER_ID],(select count([USER_ID]) from T_Users_Report where REP_ID={0} and u.[USER_ID]=[USER_ID]) Has from  [dbo].[T_Users] u where Is_Admin=0", ddlReports.SelectedValue))
        DT = PinnaclePlus.SQLData.Reports.User_ReportRights(ddlReports.SelectedValue)
        gvPara.DataSource = DT
        gvPara.DataBind()

    End Sub
     

    Private Sub gvData_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvPara.RowCommand
        If e.CommandName = "CmdDel" Then
            Dim Has As Boolean
            Dim User_ID As String = gvPara.DataKeys(e.CommandArgument).Values("User_ID")
            'Dim field As HiddenField = CType(gvPara.Rows(e.CommandArgument).FindControl("hdnHas"), HiddenField)

            PinnaclePlus.SQLData.Reports.P_Users_Report_ID(User_ID, ddlReports.SelectedValue)
        End If
        FillPara()
    End Sub 
    
    Protected Sub gvData_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvPara.PageIndexChanging
        gvPara.PageIndex = e.NewPageIndex
        Me.FillPara()
    End Sub

    Private Sub gvPara_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvPara.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim DT As DataTable
            Dim rowView As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim IB As ImageButton
            IB = e.Row.Cells(2).Controls(0)
            
            If PinnaclePlus.SQLData.Reports.User_ReportRights_Check(ddlReports.SelectedItem.Value, rowView("USER_ID")) Then
                IB.ImageUrl = "~/Styles/images/tick.gif"
            Else
                IB.ImageUrl = "~/Styles/images/cross.gif"
            End If
        End If
    End Sub
    'Private Sub GetCustomersPageWise(ByVal pageIndex As Integer)
    '    PinnaclePlus.SQLData.Reports.GetCustomersPageWise(pageIndex)
    'End Sub
    'Private Sub PopulatePager(ByVal recordCount As Integer, ByVal currentPage As Integer)
    '    Dim dblPageCount As Double = CType((CType(recordCount, Decimal) / Decimal.Parse(ddlPageSize.SelectedValue)), Double)
    '    Dim pageCount As Integer = CType(Math.Ceiling(dblPageCount), Integer)
    '    Dim pages As New List(Of ListItem)
    '    If (pageCount > 0) Then
    '        pages.Add(New ListItem("First", "1", (currentPage > 1)))
    '        Dim i As Integer = 1
    '        Do While (i <= pageCount)
    '            pages.Add(New ListItem(i.ToString, i.ToString, (i <> currentPage)))
    '            i = (i + 1)
    '        Loop
    '        pages.Add(New ListItem("Last", pageCount.ToString, (currentPage < pageCount)))
    '    End If
    '    rptPager.DataSource = pages
    '    rptPager.DataBind()
    'End Sub
    'Protected Sub PageSize_Changed(ByVal sender As Object, ByVal e As EventArgs)
    '    Me.GetCustomersPageWise(1)
    'End Sub
    'Protected Sub Page_Changed(ByVal sender As Object, ByVal e As EventArgs)
    '    Dim pageIndex As Integer = Integer.Parse(CType(sender, LinkButton).CommandArgument)
    '    Me.GetCustomersPageWise(pageIndex)
    'End Sub
End Class