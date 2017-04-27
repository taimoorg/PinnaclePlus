Public Class left_menu
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim DT As DataTable
        Dim LitText As String
        DT = PinnaclePlus.Security.P_Page_Get_All_Sibling(PinnaclePlus.Security.CurrentUser.ID, HttpContext.Current.Request.RawUrl)
        LitText = "<ul>"
        For i = 0 To DT.Rows.Count - 1
            If CStr(DT.Rows(i).Item("url")).ToLower = HttpContext.Current.Request.RawUrl.Replace("/", "").ToLower Then
                LitText = String.Format("{0}<li><a href=""{1}"" class=""selected"">{2}</a></li>", LitText, DT.Rows(i).Item("url"), DT.Rows(i).Item("Name"))
            Else
                LitText = String.Format("{0}<li><a href=""{1}"">{2}</a></li>", LitText, DT.Rows(i).Item("url"), DT.Rows(i).Item("Name"))
            End If


        Next
        LitText = String.Format("{0}</ul>", LitText)
        LitLeftMenu.Text = LitText
    End Sub

End Class