Partial Public Class menu_action
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim DR As DataRow

        DR = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleRow(String.Format("select * from T_Command_Menu where CM_ID={0}", Request.QueryString("CM_ID")))
        If DR IsNot Nothing Then
            Dim userControl As ICommandMenuAction = DirectCast(LoadControl(String.Format("~/User_Control/{0}", DR.Item("User_Control"))), ICommandMenuAction)
            userControl.A_ID = Request.QueryString("A_ID")
            ControlHolder.Controls.Add(userControl)
        End If
    End Sub
    
End Class