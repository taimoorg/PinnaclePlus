Public Class top_menu
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim DT As DataTable
        Dim Dr As DataRow
        Dim LitText As String
        DT = PinnaclePlus.Security.P_Page_Group_By_User(PinnaclePlus.Security.CurrentUser.ID)
        LitText = "<ul>"
        Dr = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleRow(String.Format("select PG_ID from T_Page where url='{0}' order by P_Order", HttpContext.Current.Request.RawUrl.Replace("/", "").ToLower))
        For i = 0 To DT.Rows.Count - 1
            If Dr.Item("PG_ID") = DT.Rows(i).Item("PG_ID") Then
                LitText = String.Format("{0}<li><a href=""{1}"" class=""selected"">{2}</a></li>", LitText, DT.Rows(i).Item("url"), DT.Rows(i).Item("Name"))
            Else
                LitText = String.Format("{0}<li><a href=""{1}"">{2}</a></li>", LitText, DT.Rows(i).Item("url"), DT.Rows(i).Item("Name"))
            End If



        Next
        LitText = String.Format("{0}</ul>", LitText)
        LitTopMenu.Text = LitText
    End Sub

End Class