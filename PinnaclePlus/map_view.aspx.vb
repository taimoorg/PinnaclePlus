Public Class map_view
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            rptMarkers.ViewStateMode = UI.ViewStateMode.Disabled
            FillMarkers()

        End If
    End Sub
    Private Sub FillMarkers()
        Dim DT As DataTable
        DT = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect("select  top 1000 ID as Description, Name , cast(lat_ as DECIMAL(12,9)) as Latitude, cast(Long_ as DECIMAL(12,9)) as Longitude from test ")
        rptMarkers.DataSource = DT
        rptMarkers.DataBind()
    End Sub
End Class