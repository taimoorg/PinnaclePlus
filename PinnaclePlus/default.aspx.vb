Imports System.DirectoryServices.AccountManagement
Imports System.DirectoryServices
Public Class _default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Dim ctx As New PrincipalContext(ContextType.Domain)

        ' find a user
        'TxtUserID.Text = UserPrincipal.Current().UserPrincipalName
        'Dim NV As New PinnaclePlus.nuVizz.NuvizzAletrs
        'NV.DO_PODs()
        ' GitHib Change
    End Sub
    

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim DR As DataRow
        Dim PasswordHash As String
        Try
            DR = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleRowPinnacle(String.Format("select [Password Hash],[Password Salt] from [Metropolitan$Web User Login] where Username='{0}'", TxtUserID.Text.ToUpper))
            If DR Is Nothing Then
                lblerror.Visible = True
                lblerror.Text = "You do not have any rights in this system"
                Exit Sub
            End If
            PasswordHash = PinnaclePlus.Security.EncodePassword(1, txtPassword.Text, Convert.ToString(DR.Item("Password Salt")))
            If DR.Item("Password Hash") = PasswordHash Then
                Session("CurrentUserAdmin") = PinnaclePlus.Security.GetUserByID(TxtUserID.Text)
                DR = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleRow(String.Format("select top 1 Url from T_Page where page_ID in (select page_ID from T_Page_Option where Is_Page=1 and PO_ID in(select PO_ID from T_Users_Page_Option where [user_ID]='{0}'))", PinnaclePlus.Security.CurrentUser.ID))
                If DR Is Nothing Then
                    If PinnaclePlus.Security.CurrentUser.Is_Admin Then
                        Response.Redirect("home.aspx")
                    Else
                        lblerror.Visible = True
                        lblerror.Text = "You do not have any rights in this system"
                    End If
                Else
                    If PinnaclePlus.Security.CurrentUser.Is_Blocked Then
                        lblerror.Visible = True
                        lblerror.Text = "You do not have any rights in this system"
                    Else
                        Response.Redirect(DR.Item("url"))
                    End If
                End If
            Else
                lblerror.Visible = True
                lblerror.Text = "The Login Failed"
            End If
        Catch ex As Exception
            lblerror.Visible = True
            lblerror.Text = String.Format("The Login Failed:{0}", Replace(ex.Message, "'", "''"))
        End Try
    End Sub
End Class