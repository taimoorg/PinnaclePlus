Public Class Campus
    Inherits System.Web.UI.Page
    Private Enum Page_Options
        View = 2001
        Add_New = 2002
        Edit = 2003
        Delete = 2004
    End Enum
    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '    If Not gAdmin.Security.P_Has_Rights(Page_Options.View) Then
    '        Response.Redirect("no_rights.aspx")
    '    End If
    '    lbAddNew.Visible = gAdmin.Security.P_Has_Rights(Page_Options.Add_New)

    '    If Not Page.IsPostBack Then
    '        litCap.Text = "Campus Management"
    '    End If
    '    FillGrid()
    'End Sub
    'Private Sub FillGrid()
    '    Dim DT As DataTable
    '    Dim Tr As TableRow
    '    Dim Thr As TableHeaderRow
    '    Dim Thc As TableHeaderCell
    '    Dim TC As TableCell
    '    Dim Lb As LinkButton
    '    DT = gAdmin.SQLData.GeneralOperations.ExecuteSelect("select * from T_Campus")
    '    tblData.Rows.Clear()

    '    Thr = New TableHeaderRow

    '    Thc = New TableHeaderCell
    '    Thc.CssClass = "table_header"
    '    Thc.Text = "Name"
    '    Thc.Style.Add("width", "80%")
    '    Thc.HorizontalAlign = HorizontalAlign.Center
    '    Thr.Cells.Add(Thc)


    '    If gAdmin.Security.P_Has_Rights(Page_Options.Edit) Then
    '        Thc = New TableHeaderCell
    '        Thc.CssClass = "table_header"
    '        Thc.Text = "Edit"
    '        Thc.HorizontalAlign = HorizontalAlign.Center
    '        Thr.Cells.Add(Thc)
    '    End If
    '    If gAdmin.Security.P_Has_Rights(Page_Options.Delete) Then
    '        Thc = New TableHeaderCell
    '        Thc.CssClass = "table_header"
    '        Thc.Text = "Delete"
    '        Thc.HorizontalAlign = HorizontalAlign.Center
    '        Thr.Cells.Add(Thc)
    '    End If
    '    tblData.Rows.Add(Thr)
    '    For i = 0 To DT.Rows.Count - 1
    '        Tr = New TableRow

    '        TC = New TableCell
    '        TC.Text = DT.Rows(i).Item("Name")
    '        TC.CssClass = "table_cell"
    '        Tr.Cells.Add(TC)

    '        If gAdmin.Security.P_Has_Rights(Page_Options.Edit) Then
    '            Lb = New LinkButton
    '            Lb.ID = String.Format("LB_{0}", DT.Rows(i).Item("CAM_ID"))
    '            Lb.Text = "Edit"
    '            AddHandler Lb.Click, AddressOf EditRecord
    '            TC = New TableCell
    '            TC.CssClass = "table_edit"
    '            TC.Controls.Add(Lb)
    '            Tr.Cells.Add(TC)
    '        End If

    '        If gAdmin.Security.P_Has_Rights(Page_Options.Delete) Then
    '            Lb = New LinkButton
    '            Lb.ID = String.Format("LB_Del_{0}", DT.Rows(i).Item("CAM_ID"))
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

    'Private Sub EditRecord(sender As Object, e As EventArgs)
    '    CAM_ID.Value = CType(sender, LinkButton).ID.Replace("LB_", "")
    '    Dim DR As DataRow
    '    DR = gAdmin.SQLData.GeneralOperations.ExecuteSelectSingleRow(String.Format("select * from T_Campus where CAM_ID={0}", CAM_ID.Value))
    '    txtName.Text = DR.Item("Name")

    '    litCap.Text = "Edit Campus"
    '    pnlData.Visible = False
    '    pnlEdit.Visible = True

    'End Sub
    'Private Sub DelRecord(sender As Object, e As EventArgs)
    '    Dim ID, Ret As Integer

    '    ID = CType(sender, LinkButton).ID.Replace("LB_Del_", "")

    '    Ret = gAdmin.SQLData.Campus.P_Campus_Del(ID)
    '    If Ret = 1 Then
    '        FillGrid()
    '    Else
    '        litError.Text = "Cannot delete, child records found, first delete them!"
    '        litError.Visible = True
    '    End If
    'End Sub

    'Private Sub lbAddNew_Click(sender As Object, e As EventArgs) Handles lbAddNew.Click
    '    CAM_ID.Value = 0
    '    txtName.Text = ""

    '    litCap.Text = "Add New Campus"
    '    pnlData.Visible = False
    '    pnlEdit.Visible = True
    'End Sub

    'Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
    '    gAdmin.SQLData.Campus.P_Campus_IU(CAM_ID.Value, txtName.Text)
    '    FillGrid()
    '    pnlData.Visible = True
    '    pnlEdit.Visible = False
    'End Sub

    'Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
    '    pnlData.Visible = True
    '    pnlEdit.Visible = False
    '    litCap.Text = "Campus Management"
    'End Sub
End Class