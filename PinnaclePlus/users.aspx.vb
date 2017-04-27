Imports System.DirectoryServices.AccountManagement

Public Class users
    Inherits System.Web.UI.Page
    Private Enum Page_Options
        View = 90101
        Add_New = 90102
        Edit = 90103
        Delete = 90104
    End Enum
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not PinnaclePlus.Security.P_Has_Rights(Page_Options.View) Then
            Response.Redirect("no_rights.aspx")
        End If
        lbAddNew.Visible = PinnaclePlus.Security.P_Has_Rights(Page_Options.Add_New)

        If Not Page.IsPostBack Then
            litCap.Text = "Users Management"
        End If

        FillGrid()

    End Sub
    Private Sub FillGrid()
        Dim DT As DataTable
        Dim Tr As TableRow
        Dim Thr As TableHeaderRow
        Dim Thc As TableHeaderCell
        Dim TC As TableCell
        Dim Lb As LinkButton
        DT = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect("select * from T_Users ")
        tblData.Rows.Clear()

        Thr = New TableHeaderRow

        Thc = New TableHeaderCell
        Thc.CssClass = "table_header"
        Thc.Text = "User ID"
        Thc.Width = "90"
        Thc.HorizontalAlign = HorizontalAlign.Center
        Thr.Cells.Add(Thc)

        Thc = New TableHeaderCell
        Thc.CssClass = "table_header"
        Thc.Text = "Full Name"
        Thc.HorizontalAlign = HorizontalAlign.Left
        Thr.Cells.Add(Thc)

        Thc = New TableHeaderCell
        Thc.CssClass = "table_header"
        Thc.Text = "Is Admin"
        Thc.HorizontalAlign = HorizontalAlign.Left
        Thr.Cells.Add(Thc)

        If PinnaclePlus.Security.P_Has_Rights(Page_Options.Edit) Then
            Thc = New TableHeaderCell
            Thc.CssClass = "table_header"
            Thc.Text = "Edit"
            Thc.HorizontalAlign = HorizontalAlign.Center
            Thr.Cells.Add(Thc)
        End If
        If PinnaclePlus.Security.P_Has_Rights(Page_Options.Delete) Then
            Thc = New TableHeaderCell
            Thc.CssClass = "table_header"
            Thc.Text = "Delete"
            Thc.HorizontalAlign = HorizontalAlign.Center
            Thr.Cells.Add(Thc)
        End If
        tblData.Rows.Add(Thr)
        For i = 0 To DT.Rows.Count - 1
            Tr = New TableRow

            TC = New TableCell
            TC.Text = DT.Rows(i).Item("USER_ID")
            TC.CssClass = "table_cell_center"
            Tr.Cells.Add(TC)

            TC = New TableCell
            TC.Text = DT.Rows(i).Item("Full_Name")
            TC.CssClass = "table_cell"
            Tr.Cells.Add(TC)

            TC = New TableCell
            TC.Text = IIf(DT.Rows(i).Item("Is_Admin"), "Yes", "No")
            TC.CssClass = "table_cell"
            Tr.Cells.Add(TC)

            If PinnaclePlus.Security.P_Has_Rights(Page_Options.Edit) Then
                Lb = New LinkButton
                Lb.ID = String.Format("LB_{0}", DT.Rows(i).Item("USER_ID"))
                Lb.Text = "Edit"
                AddHandler Lb.Click, AddressOf EditRecord
                TC = New TableCell
                TC.CssClass = "table_edit"
                TC.Controls.Add(Lb)
                Tr.Cells.Add(TC)
            End If

            If PinnaclePlus.Security.P_Has_Rights(Page_Options.Delete) Then
                Lb = New LinkButton
                Lb.ID = String.Format("LB_Del_{0}", DT.Rows(i).Item("USER_ID"))
                Lb.Text = "Delete"
                Lb.OnClientClick = "return confirm('Are you sure you want delete this record');"
                AddHandler Lb.Click, AddressOf DelRecord
                TC = New TableCell
                TC.CssClass = "table_delete"
                TC.Controls.Add(Lb)
                Tr.Cells.Add(TC)
            End If
            tblData.Rows.Add(Tr)
        Next
    End Sub

    Private Sub EditRecord(sender As Object, e As EventArgs)
        Dim USER_ID As String = CType(sender, LinkButton).ID.Replace("LB_", "")
        Dim DR As DataRow
        Dim DT As DataTable
        DR = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleRow(String.Format("select * from T_Users where USER_ID='{0}'", USER_ID))
        txttxtUn.Text = DR.Item("USER_ID")
        txttxtUn.Enabled = False
        If PinnaclePlus.Security.CurrentUser.Is_Admin Then
            chkIsAdmin.Enabled = True
        Else
            chkIsAdmin.Enabled = False
        End If
        chkIsAdmin.Checked = DR.Item("Is_Admin")
        
        litCap.Text = "Edit User"
        pnlData.Visible = False
        pnlEdit.Visible = True

    End Sub
    Private Sub DelRecord(sender As Object, e As EventArgs)
        Dim USER_ID As String = CType(sender, LinkButton).ID.Replace("LB_Del_", "")
        Dim Ret As Integer

        Ret = PinnaclePlus.Security.P_Users_Del(USER_ID)
        If Ret = 1 Then
            FillGrid()
        Else
            litError.Text = "Cannot delete, child records found, first delete them!"
            litError.Visible = True
        End If

    End Sub

    Private Sub lbAddNew_Click(sender As Object, e As EventArgs) Handles lbAddNew.Click

        litCap.Text = "Add New User"
        txttxtUn.Text = ""
        If PinnaclePlus.Security.CurrentUser.Is_Admin Then
            chkIsAdmin.Enabled = True
            chkIsAdmin.Checked = False
        Else
            chkIsAdmin.Enabled = False
            chkIsAdmin.Checked = False
        End If
        txttxtUn.Enabled = True
        pnlData.Visible = False
        pnlEdit.Visible = True

    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If User Is Nothing Then
            litError.Text = "User not found on the domain."
            litError.Visible = True
            Return
        End If
        PinnaclePlus.Security.P_Users_IU(txttxtUn.Text, txttxtUn.Text, chkIsAdmin.Checked)
        FillGrid()
        pnlData.Visible = True
        pnlEdit.Visible = False
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        pnlData.Visible = True
        pnlEdit.Visible = False
        litCap.Text = "Users Management"
    End Sub

End Class