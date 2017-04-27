Public Class PinnaclePlus_master
    Inherits System.Web.UI.MasterPage
    Public Delegate Sub MyAsyncDelegate()
    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        If My.Settings.nuVizzIntegrationCompanyCode = "metro" Then
            testdiv.Visible = True
        Else
            testdiv.Visible = False
        End If
        If Session("CurrentUserAdmin") Is Nothing Then
            Response.Redirect(".")
        End If

    End Sub
    Private Sub ImDone(ByVal ar As System.IAsyncResult)
        
    End Sub

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Dim del As MyAsyncDelegate

        'del = New MyAsyncDelegate(AddressOf StartFTP)
        'Dim cb As AsyncCallback = New AsyncCallback(AddressOf ImDone)
        'Dim oState As New Object
        'del.BeginInvoke(cb, oState)
    End Sub

End Class