Public Class config_change
    Inherits System.Web.UI.Page
    Private Enum Page_Options
        View = 2001
        Add_New = 2002
        Edit = 2003
        Delete = 2004
    End Enum
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not PinnaclePlus.Security.P_Has_Rights(Page_Options.View) Then
            Response.Redirect("no_rights.aspx")
        End If


        If Not Page.IsPostBack Then
            litCap.Text = "System Settings"
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
        DT = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect("select * from T_Config")
        tblData.Rows.Clear()

        Thr = New TableHeaderRow

        Thc = New TableHeaderCell
        Thc.CssClass = "table_header"
        Thc.Text = "Name"
        Thc.Width = "150"
        Thc.HorizontalAlign = HorizontalAlign.Center
        Thr.Cells.Add(Thc)

        Thc = New TableHeaderCell
        Thc.CssClass = "table_header"
        Thc.Text = "Description"
        Thc.HorizontalAlign = HorizontalAlign.Center
        Thr.Cells.Add(Thc)

        Thc = New TableHeaderCell
        Thc.CssClass = "table_header"
        Thc.Text = "Value"
        Thc.HorizontalAlign = HorizontalAlign.Center
        Thr.Cells.Add(Thc)
        If PinnaclePlus.Security.P_Has_Rights(Page_Options.Edit) Then
            Thc = New TableHeaderCell
            Thc.CssClass = "table_header"
            Thc.Text = "Edit"
            Thc.HorizontalAlign = HorizontalAlign.Center
            Thr.Cells.Add(Thc)
        End If
        
        tblData.Rows.Add(Thr)
        For i = 0 To DT.Rows.Count - 1
            Tr = New TableRow

            TC = New TableCell
            TC.Text = DT.Rows(i).Item("Name")
            TC.CssClass = "table_cell"
            Tr.Cells.Add(TC)

            TC = New TableCell
            TC.Text = DT.Rows(i).Item("Description")
            TC.CssClass = "table_cell"
            Tr.Cells.Add(TC)

            TC = New TableCell
            TC.Text = DT.Rows(i).Item("C_Value")
            TC.CssClass = "table_cell_center"
            Tr.Cells.Add(TC)

            If PinnaclePlus.Security.P_Has_Rights(Page_Options.Edit) Then
                Lb = New LinkButton
                Lb.ID = String.Format("LB_{0}", DT.Rows(i).Item("CONFIG_ID"))
                Lb.Text = "Edit"
                AddHandler Lb.Click, AddressOf EditRecord
                TC = New TableCell
                TC.CssClass = "table_edit"
                TC.Controls.Add(Lb)
                Tr.Cells.Add(TC)
            End If
            tblData.Rows.Add(Tr)
        Next
    End Sub

    Private Sub EditRecord(sender As Object, e As EventArgs)
        CONFIG_ID.Value = CType(sender, LinkButton).ID.Replace("LB_", "")
        Dim DR As DataRow
        DR = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleRow(String.Format("select * from T_Config where CONFIG_ID={0}", CONFIG_ID.Value))
        txtName.Text = DR.Item("Name")
        txtValue.Text = DR.Item("C_Value")
        litDescription.Text = DR.Item("Description")
        litCap.Text = "System Settings"
        pnlData.Visible = False
        pnlEdit.Visible = True

    End Sub
    Private Sub DelRecord(sender As Object, e As EventArgs)
        Dim ID, Ret As Integer

        ID = CType(sender, LinkButton).ID.Replace("LB_Del_", "")

        'Ret = PinnaclePlus.SQLData.Modules.P_Module_Del(ID)
        If Ret = 1 Then
            FillGrid()
        Else
            litError.Text = "Cannot delete, child records found, first delete them!"
            litError.Visible = True
        End If
    End Sub

   

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        'PinnaclePlus.Config.P_Config_Update(CONFIG_ID.Value, txtValue.Text)

        FillGrid()
        pnlData.Visible = True
        pnlEdit.Visible = False
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        pnlData.Visible = True
        pnlEdit.Visible = False
        litCap.Text = "System Settings"
    End Sub
End Class