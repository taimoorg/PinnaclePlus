Public Class dispatch_data
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            FillHubs()
        End If
    End Sub
    Private Sub FillHubs()
        Dim DT As DataTable
        Dim LI As ListItem
        DT = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect(String.Format("Select * from T_Hub where HUB_CODE in (select HUB_CODE from T_Users_Hub where USER_ID='{0}')", PinnaclePlus.Security.CurrentUser.ID))
        For i = 0 To DT.Rows.Count - 1
            LI = New ListItem
            LI.Text = DT.Rows(i).Item("Name")
            LI.Value = DT.Rows(i).Item("HUB_CODE")
            ddlHub.Items.Add(LI)
        Next
        'ddlHub_SelectedIndexChanged(ddlHub, New System.EventArgs)
    End Sub
    Private Sub FillManifests()
        Dim DT As DataTable
        Dim LI As ListItem
        DT = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect(String.Format("select * from V_Manifest where Hub='{0}' and cast(StartDate as date)='{1}-{2}-{3}'", ddlHub.SelectedValue, CDate(txtDate.Text).Year, CDate(txtDate.Text).Month, CDate(txtDate.Text).Day))
        ddlManifest.Items.Clear()
        LI = New ListItem
        LI.Text = "All Manifests"
        LI.Value = 0
        ddlManifest.Items.Add(LI)
        For i = 0 To DT.Rows.Count - 1
            LI = New ListItem
            LI.Text = String.Format("{0}-{1}-{2}", DT.Rows(i).Item("MIN_ID"), DT.Rows(i).Item("DriverName"), DT.Rows(i).Item("TruckName"))
            LI.Value = DT.Rows(i).Item("MIN_ID")
            ddlManifest.Items.Add(LI)
        Next
        
    End Sub

    Private Sub txtDate_TextChanged(sender As Object, e As EventArgs) Handles txtDate.TextChanged
        FillManifests()
    End Sub
End Class