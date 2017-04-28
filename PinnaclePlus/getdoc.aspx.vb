Public Class getdoc
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        PinnaclePlusGlobals.GetDocument(Request.QueryString("ID"))
    End Sub

End Class