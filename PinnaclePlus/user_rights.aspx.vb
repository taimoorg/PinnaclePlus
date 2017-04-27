Public Class user_rights
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            FillUsers()
            FillTree()
        End If
    End Sub
    Private Sub FillTree()
        Dim DTG, DTP, DTO As DataTable
        Dim DR As DataRow
        Dim TN, TN_G, TN_P, TN_O As TreeNode
        TreeView1.Nodes.Clear()
        TN = New TreeNode
        TN.Text = "PinnaclePlus"
        TN.Value = "0"
        TN.SelectAction = TreeNodeSelectAction.None
        TreeView1.Nodes.Add(TN)
        DTG = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect("Select * from T_Page_Group")
        For i = 0 To DTG.Rows.Count - 1
            TN_G = New TreeNode
            TN_G.Text = DTG.Rows(i).Item("Name")
            TN_G.Value = DTG.Rows(i).Item("PG_ID")
            TN_G.SelectAction = TreeNodeSelectAction.None
            TN.ChildNodes.Add(TN_G)
            DTP = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect(String.Format("select * from T_Page where PG_ID={0}", DTG.Rows(i).Item("PG_ID")))
            For j = 0 To DTP.Rows.Count - 1
                TN_P = New TreeNode
                TN_P.Text = DTP.Rows(j).Item("Name")
                TN_P.Value = DTP.Rows(j).Item("PAGE_ID")
                TN_P.SelectAction = TreeNodeSelectAction.None
                TN_G.ChildNodes.Add(TN_P)
                DTO = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect(String.Format("select * from T_Page_Option where PAGE_ID={0}", DTP.Rows(j).Item("PAGE_ID")))
                For k = 0 To DTO.Rows.Count - 1
                    TN_O = New TreeNode
                    TN_O.Text = DTO.Rows(k).Item("Name")
                    TN_O.Value = DTO.Rows(k).Item("PO_ID")
                    TN_O.SelectAction = TreeNodeSelectAction.None
                    TN_P.ChildNodes.Add(TN_O)
                    DR = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleRow(String.Format("select * from T_Users_Page_Option where USER_ID='{0}' and PO_ID={1}", ddlUser.SelectedItem.Value, DTO.Rows(k).Item("PO_ID")))
                    If DR Is Nothing Then
                        TN_O.Checked = False
                    Else
                        TN_O.Checked = True
                    End If
                Next
            Next
        Next
        TN.ExpandAll()
    End Sub
    Private Sub FillUsers()
        Dim DT As DataTable
        Dim LI As ListItem
        DT = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect("select * from T_Users where Is_Admin=0")
        For i = 0 To DT.Rows.Count - 1
            LI = New ListItem
            LI.Text = String.Format("{0} - {1}", DT.Rows(i).Item("USER_ID"), DT.Rows(i).Item("Full_Name"))
            LI.Value = DT.Rows(i).Item("USER_ID")
            ddlUser.Items.Add(LI)
        Next
        If ddlUser.Items.Count > 0 Then
            ddlUser.SelectedIndex = 0
        End If
    End Sub
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim TN, TN_G, TN_P, TN_O As TreeNode
        TN = TreeView1.Nodes(0)
        For Each TN_G In TN.ChildNodes
            For Each TN_P In TN_G.ChildNodes
                For Each TN_O In TN_P.ChildNodes
                    If TN_O.Checked Then
                        PinnaclePlus.Security.P_Users_Page_Option_IU(TN_O.Value, ddlUser.SelectedItem.Value)
                    Else
                        PinnaclePlus.Security.P_Users_Page_Option_Del(TN_O.Value, ddlUser.SelectedItem.Value)
                    End If
                Next
            Next
        Next
    End Sub
    Private Sub ddlModule_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlUser.SelectedIndexChanged
        FillTree()
    End Sub

    Private Sub TreeView1_SelectedNodeChanged(sender As Object, e As EventArgs) Handles TreeView1.SelectedNodeChanged
        Dim sas As String
    End Sub
End Class