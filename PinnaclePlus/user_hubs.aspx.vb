Public Class user_hubs
    Inherits System.Web.UI.Page
    Private Enum Page_Options
        View = 90301
        Edit = 90302
    End Enum
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not PinnaclePlus.Security.P_Has_Rights(Page_Options.View) Then
            Response.Redirect("no_rights.aspx")
        End If
        If Not Page.IsPostBack Then
            FillUsers()

        End If
        FillGrid()
    End Sub
    Private Sub FillUsers()
        Dim DT As DataTable
        Dim LI As ListItem
        DT = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect("select * from T_Users where Is_Admin=0")
        For i = 0 To DT.Rows.Count - 1
            LI = New ListItem
            LI.Text = String.Format("{0} - {1}", DT.Rows(i).Item("USER_ID"), DT.Rows(i).Item("Full_Name"))
            LI.Value = DT.Rows(i).Item("USER_ID")
            ddlUser.Items.Add(LI)
        Next
        If ddlUser.Items.Count > 0 Then
            ddlUser.SelectedIndex = 0
        End If
    End Sub
    Private Sub FillGrid()
        Dim DT As DataTable
        Dim Tr As TableRow
        Dim Thr As TableHeaderRow
        Dim Thc As TableHeaderCell
        Dim TC As TableCell

        Dim Ib As ImageButton
        Dim Img As Image
        DT = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect(String.Format("select *, Is_allowed=(select count(USER_ID) from T_Users_Hub where HUB_CODE=h.HUB_CODE and [USER_ID]='{0}' )  from T_Hub h order by Is_allowed desc", ddlUser.SelectedValue))
        tblData.Rows.Clear()

        Thr = New TableHeaderRow

        Thc = New TableHeaderCell
        Thc.CssClass = "table_header"
        Thc.Text = "Code"
        Thc.HorizontalAlign = HorizontalAlign.Center
        Thr.Cells.Add(Thc)

        Thc = New TableHeaderCell
        Thc.CssClass = "table_header"
        Thc.Text = "Name"
        Thc.HorizontalAlign = HorizontalAlign.Center
        Thr.Cells.Add(Thc)

        Thc = New TableHeaderCell
        Thc.CssClass = "table_header"
        Thc.Text = "Allow"
        Thc.HorizontalAlign = HorizontalAlign.Center
        Thr.Cells.Add(Thc)



        tblData.Rows.Add(Thr)
        For i = 0 To DT.Rows.Count - 1
            Tr = New TableRow

            TC = New TableCell
            TC.Text = DT.Rows(i).Item("HUB_CODE")
            TC.CssClass = "table_cell"
            Tr.Cells.Add(TC)

            TC = New TableCell
            TC.Text = DT.Rows(i).Item("Name")
            TC.CssClass = "table_cell"
            Tr.Cells.Add(TC)


            If PinnaclePlus.Security.P_Has_Rights(Page_Options.Edit) Then
                Ib = New ImageButton
                Ib.ID = String.Format("IB_{0}", DT.Rows(i).Item("HUB_CODE"))
                If DT.Rows(i).Item("Is_allowed") = 1 Then
                    Ib.ImageUrl = "~/Styles/images/tick.gif"
                Else
                    Ib.ImageUrl = "~/Styles/images/cross.gif"
                End If

                AddHandler Ib.Click, AddressOf ToggleAllow
                TC = New TableCell
                TC.CssClass = "table_edit"
                TC.Controls.Add(Ib)
                Tr.Cells.Add(TC)
            Else
                Img = New Image
                If DT.Rows(i).Item("Is_allowed") = 1 Then
                    Img.ImageUrl = "~/Styles/images/tick.gif"
                Else
                    Img.ImageUrl = "~/Styles/images/cross.gif"
                End If


                TC = New TableCell
                TC.CssClass = "table_edit"
                TC.Controls.Add(Img)
                Tr.Cells.Add(TC)
            End If

            tblData.Rows.Add(Tr)
        Next


    End Sub

    Private Sub ToggleAllow(sender As Object, e As EventArgs)
        PinnaclePlus.Security.P_User_Hub_Toggle(CType(sender, ImageButton).ID.Replace("IB_", ""), ddlUser.SelectedValue)
        FillGrid()
    End Sub

    'Private Sub btnRemoveAll_Click(sender As Object, e As EventArgs) Handles btnRemoveAll.Click
    '    'Dim DT As DataTable
    '    'DT = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect(String.Format("select * from T_Users_Programs where UserID='{0}'", ddlUser.SelectedValue))
    '    'For i = 0 To DT.Rows.Count - 1
    '    '    PinnaclePlus.Security.P_Users_Programs_Toggle(DT.Rows(i).Item("PRO_ID"), ddlUser.SelectedValue)
    '    'Next
    '    'FillGrid()
    'End Sub
End Class