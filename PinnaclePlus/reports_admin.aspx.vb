Public Class reports_admin
    Inherits System.Web.UI.Page
    Private Enum Page_Options
        View = 40101
        Add_New = 40102
        Edit = 40103
        Delete = 40104
        Copy = 40105
    End Enum
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not PinnaclePlus.Security.P_Has_Rights(Page_Options.View) Then
            Response.Redirect("no_rights.aspx")
        End If
        lbAddNew.Visible = PinnaclePlus.Security.P_Has_Rights(Page_Options.Add_New)

        If Not Page.IsPostBack Then
            litCap.Text = "Reports Management"
            FillGrid()
        End If

    End Sub
    Private Sub FillGrid()
        Dim DT As DataTable
        Dim query As String
        query = String.Format("select * from T_Report  where 1=1 ")
        If Rep_Val.Value <> "" Then
            query = String.Format("{0} and [REP_ID]={1}", query, Rep_Val.Value)
        End If
        DT = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect(query)
        If Not PinnaclePlus.Security.P_Has_Rights(Page_Options.Edit) Then
            gvData.Columns(2).Visible = False
        Else
            gvData.Columns(2).Visible = True
        End If
        If Not PinnaclePlus.Security.P_Has_Rights(Page_Options.Copy) Then
            gvData.Columns(3).Visible = False
        Else
            gvData.Columns(3).Visible = True
        End If
        If Not PinnaclePlus.Security.P_Has_Rights(Page_Options.Delete) Then
            gvData.Columns(4).Visible = False
        Else
            gvData.Columns(4).Visible = True
        End If
        gvData.DataSource = DT
        gvData.DataBind()
        If Rep_Val.Value <> "" Then
            txtSearch.Text = DT.Rows.Item(0).Item("Name")
        End If

    End Sub

    Private Sub EditRecord()

        Fillvalues()
        litCap.Text = "Edit Report"
        pnlData.Visible = False
        pnlEdit.Visible = True

    End Sub
    Private Sub CopyRecord()

        Fillvalues()
        REP_ID.Value = 0
        litCap.Text = "Edit Report"
        pnlData.Visible = False
        pnlEdit.Visible = True

    End Sub
    Private Sub Fillvalues()
        Dim DR As DataRow
        Dim DT As DataTable
        Dim LI As ListItem
        DR = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleRow(String.Format("select [REP_ID],[Name],[Query],isnull(Description,'') as Description,isnull(Is_PP,0) as Is_PP from T_Report where REP_ID={0}", REP_ID.Value))
        If DR IsNot Nothing Then
            txtName.Text = DR.Item("Name")
            txtQuery.Text = DR.Item("Query")
            txtDes.Text = DR.Item("Description")
            chkIsPP.Checked = DR.Item("Is_PP")
        Else
            txtName.Text = ""
            txtQuery.Text = ""
            txtDes.Text = ""
            chkIsPP.Checked = False
        End If
        DT = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect(String.Format("select u.[USER_ID],(select count([USER_ID]) from T_Users_Report where REP_ID={0} and u.[USER_ID]=[USER_ID]) Has from  [dbo].[T_Users] u where Is_Admin=0", REP_ID.Value))
        lstUsers.Items.Clear()
        For i = 0 To DT.Rows.Count - 1
            LI = New ListItem
            LI.Text = DT.Rows(i).Item("USER_ID")
            If DT.Rows(i).Item("Has") = 1 Then
                LI.Selected = True
            Else
                LI.Selected = False
            End If
            lstUsers.Items.Add(LI)

        Next
        FillGridPara()
    End Sub
    Private Sub FillGridPara()
        Dim DT As DataTable
        DT = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect(String.Format("select * from T_Report_Para where REP_ID={0}", REP_ID.Value))
        gvPara.DataSource = DT
        gvPara.DataBind()
    End Sub

    Private Sub DelRecord(REP_ID As Integer)


        'Ret = 0 'PinnaclePlus.SQLData.Campus.P_Campus_Del(ID)
        'If Ret = 1 Then
        '    FillGrid()
        'Else
        '    litError.Text = "Cannot delete, child records found, first delete them!"
        '    litError.Visible = True
        'End If
    End Sub

    Private Sub lbAddNew_Click(sender As Object, e As EventArgs) Handles lbAddNew.Click
        REP_ID.Value = 0
        Fillvalues()
        litCap.Text = "Add New Report"
        pnlData.Visible = False
        pnlEdit.Visible = True
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim ID As Integer
        Dim Has As Boolean
        Dim gvRow As GridViewRow
        Dim Txt As TextBox
        Dim DDl As DropDownList
        Dim Name, Desc As String
        Dim ParaType, Width, RP_ID As Integer
        Dim Rows As Object
        ID = PinnaclePlus.SQLData.Reports.P_Report_IU(REP_ID.Value, txtName.Text, txtQuery.Text, txtDes.Text, chkIsPP.Checked)

        For i = 0 To gvPara.Rows.Count - 1
            gvRow = gvPara.Rows(i)
            If gvRow.RowType = DataControlRowType.DataRow Then
                RP_ID = Convert.ToInt32(gvPara.DataKeys(i).Values(0))
                Txt = gvRow.FindControl("txtParaName")
                Name = Txt.Text
                Txt = gvRow.FindControl("txtParaDescription")
                Desc = Txt.Text
                DDl = gvRow.FindControl("ddlParaType")
                ParaType = DDl.SelectedValue
                Txt = gvRow.FindControl("txtParaWidth")
                If IsNumeric(Txt.Text) Then
                    Width = Txt.Text
                Else
                    Width = 1
                End If
                Txt = gvRow.FindControl("txtParaRows")
                If IsNumeric(Txt.Text) Then
                    Rows = Txt.Text
                Else
                    Rows = DBNull.Value
                End If
                If REP_ID.Value = 0 Then
                    RP_ID = 0
                End If
                PinnaclePlus.SQLData.Reports.P_Report_Para_IU(RP_ID, ID, Name, Desc, 0, Width, Rows, ParaType)
            End If
        Next


        For i = 0 To lstUsers.Items.Count - 1
            Has = lstUsers.Items(i).Selected
            PinnaclePlus.SQLData.Reports.P_Users_Report_ID(lstUsers.Items(i).Text, ID, Has)
        Next
        FillGrid()
        pnlData.Visible = True
        pnlEdit.Visible = False
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        pnlData.Visible = True
        pnlEdit.Visible = False
        litCap.Text = "Report Management"
    End Sub

    Private Sub gvPara_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvPara.RowCommand
        If e.CommandName = "CmdDel" Then
            PinnaclePlus.SQLData.Reports.P_Report_Para_Del(Convert.ToInt32(gvPara.DataKeys(e.CommandArgument).Values(0)))
            FillGridPara()
        End If
    End Sub

    Private Sub gvPara_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvPara.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim rowView As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim DR As DataRow
            Dim L_RP_ID As Integer = rowView("RP_ID")
            DR = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleRow(String.Format("select * from T_Report_Para where RP_ID={0}", L_RP_ID))
            Dim ddl As DropDownList

            ddl = e.Row.Cells(3).FindControl("ddlParaType")
            For i = 0 To ddl.Items.Count - 1
                If ddl.Items(i).Value = DR.Item("Para_Type") Then
                    ddl.SelectedIndex = i
                    Exit For
                End If
            Next
            Dim DelLink As LinkButton = e.Row.Cells(6).Controls(0)
            DelLink.OnClientClick = "return confirm('Are you sure you want to delete this record?');"
        End If

    End Sub
    Private Sub gvData_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvData.RowCommand
        
        If e.CommandName = "CmdDel" Then
        ElseIf e.CommandName = "CmdEdit" Then
            REP_ID.Value = Convert.ToInt32(gvData.DataKeys(e.CommandArgument).Values(0))
            EditRecord()
        ElseIf e.CommandName = "CmdCopy" Then
            REP_ID.Value = Convert.ToInt32(gvData.DataKeys(e.CommandArgument).Values(0))
            CopyRecord()
        End If
    End Sub
    Private Sub btnAddPara_Click(sender As Object, e As EventArgs) Handles btnAddPara.Click
        PinnaclePlus.SQLData.Reports.P_Report_Para_IU(0, REP_ID.Value, "", "", 0, 150, DBNull.Value, 10)
        FillGridPara()
    End Sub
    'PAGE NUMBER
    Protected Sub gvData_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvData.PageIndexChanging
        gvData.PageIndex = e.NewPageIndex
        Me.FillGrid()
    End Sub

    'PAGE SIZE [NO.OF ROWS SHOW ON PER PAGE]
    Protected Sub DropDownList1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList1.SelectedIndexChanged
        Dim size As Integer = 0
        If DropDownList1.SelectedItem.Text <> "--Select--" Then
            size = Integer.Parse(DropDownList1.SelectedItem.Value.ToString())
            gvData.PageSize = size
            FillGrid()
        End If
    End Sub

    Private Sub btnGo_Click(sender As Object, e As EventArgs) Handles btnGo.Click
        FillGrid()
    End Sub

End Class