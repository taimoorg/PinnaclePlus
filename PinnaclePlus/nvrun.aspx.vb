Public Class nvrun
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        HttpContext.Current.Application.Remove("GetnuVizzRunning")
    End Sub

End Class