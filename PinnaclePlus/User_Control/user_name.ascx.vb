Public Class user_name
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        litUN.Text = String.Format("{0}", PinnaclePlus.Security.CurrentUser.Full_Name)
    End Sub

End Class