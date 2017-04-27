Imports System.IO
Imports OfficeOpenXml

Public Class reports
    Inherits System.Web.UI.Page
    Private Sub FillPara()
        litError.Visible = False
        Dim DR As DataRow
        Dim DT As DataTable
        DT = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect(String.Format("select * from T_Report_Para where REP_ID={0} order by Order_", Rep_Value.Value))
        gvData.DataSource = DT
        gvData.DataBind()
        GridView1.DataSource = Nothing
        GridView1.DataBind()
        DR = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleRow(String.Format("select [REP_ID],[Name],[Query],[Description],isnull(Is_PP,0) as Is_PP  from T_Report where REP_ID={0}", Rep_Value.Value))
        txtSearch.Text = DR.Item("Name")

        litError.Text = ""
        litError.Visible = False
    End Sub
    Public Function ExportToExcel(FileName As String, SheetName As String, data As DataTable) As Boolean
        Dim pck As ExcelPackage
        pck = New ExcelPackage()

        Try

            Dim ws = pck.Workbook.Worksheets.Add(SheetName)
            ws.Cells("A1").LoadFromDataTable(data, True)
            Dim excel = pck.GetAsByteArray()

            HttpContext.Current.Response.ClearHeaders()
            HttpContext.Current.Response.ClearContent()
            HttpContext.Current.Response.Clear() 'really clear it :-p
            HttpContext.Current.Response.BufferOutput = False
            HttpContext.Current.Response.ContentType = "application/octet-stream"
            HttpContext.Current.Response.AddHeader("cache-control", "max-age=0")
            HttpContext.Current.Response.AddHeader("Pragma", "public")
            HttpContext.Current.Response.AddHeader("Content-Length", excel.Length.ToString())
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;  filename=""" & FileName & ".xlsx""")

            HttpContext.Current.Response.BinaryWrite(excel)

            HttpContext.Current.Response.[End]()

            Return True
        Catch
            Return False
        Finally
            pck.Dispose()

        End Try

    End Function
    Private Function SetOrders(StrOrder As String, Seperation_Logic_Is_Text As Boolean) As String
        StrOrder = StrOrder.Replace("'", "")
        StrOrder = StrOrder.Replace(",", Chr(13))
        StrOrder = StrOrder.Replace(" ", Chr(13))
        StrOrder = StrOrder.Replace(Chr(10), Chr(13))
        Dim StrRet As String = ""
        Dim OrderNo As String
        Dim A()
        A = StrOrder.Split(Chr(13))
        For Each OrderNo In A
            If OrderNo.Trim <> "" Then


                If StrRet = "" Then
                    If Seperation_Logic_Is_Text Then
                        StrRet = String.Format("'{0}'", OrderNo.Trim)
                    Else
                        StrRet = String.Format("{0}", OrderNo.Trim)
                    End If

                Else
                    If Seperation_Logic_Is_Text Then
                        StrRet = String.Format("{0},'{1}'", StrRet, OrderNo.Trim)
                    Else
                        StrRet = String.Format("{0},{1}", StrRet, OrderNo.Trim)
                    End If
                End If
            End If
        Next
        If Not Seperation_Logic_Is_Text Then

        End If
        Return StrRet
    End Function
    Private Function SetTrack(StrOrder As String, Seperation_Logic_Is_Text As Boolean) As String
        StrOrder = StrOrder.Replace("'", "")
        StrOrder = StrOrder.Replace(",", Chr(13))
        StrOrder = StrOrder.Replace(" ", Chr(13))
        StrOrder = StrOrder.Replace(Chr(10), Chr(13))
        Dim StrRet As String = ""
        Dim OrderNo As String
        Dim A()
        A = StrOrder.Split(Chr(13))
        For Each OrderNo In A
            If OrderNo.Trim <> "" Then
                If StrRet = "" Then
                    If Seperation_Logic_Is_Text Then
                        StrRet = String.Format("'{0}'", OrderNo.Trim.PadLeft(12, "0"))
                    Else
                        StrRet = String.Format("{0}", OrderNo.Trim.PadLeft(12, "0"))
                    End If
                Else
                    If Seperation_Logic_Is_Text Then
                        StrRet = String.Format("{0},'{1}'", StrRet, OrderNo.Trim.PadLeft(12, "0"))
                    Else
                        StrRet = String.Format("{0},{1}", StrRet, OrderNo.Trim.PadLeft(12, "0"))
                    End If

                End If
            End If
        Next
        Return StrRet
    End Function
    Private Sub gvData_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvData.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim DT As DataTable
            Dim rowView As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim DR As DataRow
            Dim ParaValue As Object
            Dim L_RP_ID As Integer = rowView("RP_ID")
            DR = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleRow(String.Format("select * from T_Report_Para where RP_ID={0}", L_RP_ID))
            Dim txt As TextBox
            Dim FileUL As FileUpload
            Dim DDl As DropDownList
            ParaValue = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleValue(String.Format("select Value_ from T_Users_Report_Para where [USER_ID]='{0}' and RP_ID={1}", PinnaclePlus.Security.CurrentUser.ID, L_RP_ID))
            If ParaValue Is Nothing Then
                ParaValue = ""
            End If
            txt = e.Row.Cells(2).FindControl("txtPara")
            txt.Enabled = True
            DDl = e.Row.Cells(2).FindControl("ddlPara")
            txt.Visible = True
            DDl.Visible = False
            FileUL = e.Row.Cells(2).FindControl("FileUpload1")
            txt.Width = New System.Web.UI.WebControls.Unit(DR.Item("Width"), UnitType.Pixel)
            If Not IsDBNull(DR.Item("Rows_")) Then
                txt.TextMode = TextBoxMode.MultiLine
                txt.Rows = DR.Item("Rows_")
            End If
            If DR.Item("Para_Type") = PinnaclePlusGlobals.Report_Para_Type.GeneralText Then
                txt.Text = ParaValue
            ElseIf DR.Item("Para_Type") = PinnaclePlusGlobals.Report_Para_Type.Date_ Then
                txt.CssClass = "cal"
                txt.Text = ParaValue
            ElseIf DR.Item("Para_Type") = PinnaclePlusGlobals.Report_Para_Type.OrderNo Then
                txt.Text = ParaValue
            ElseIf DR.Item("Para_Type") = PinnaclePlusGlobals.Report_Para_Type.OrderNo_Outer Then
                txt.Text = ParaValue
            ElseIf DR.Item("Para_Type") = PinnaclePlusGlobals.Report_Para_Type.TrackingNo Then
                txt.Text = ParaValue
            ElseIf DR.Item("Para_Type") = PinnaclePlusGlobals.Report_Para_Type.TrackingNo_Outer Then
                txt.Text = ParaValue
            ElseIf DR.Item("Para_Type") = PinnaclePlusGlobals.Report_Para_Type.ClientList Then
                txt.CssClass = "ClientList"
                txt.Text = ParaValue
            ElseIf DR.Item("Para_Type") = PinnaclePlusGlobals.Report_Para_Type.State_List Then
                txt.CssClass = "StateList"
                txt.Text = ParaValue
            ElseIf DR.Item("Para_Type") = PinnaclePlusGlobals.Report_Para_Type.Hub_List Then
                DDl.Visible = True
                DDl.Width = New System.Web.UI.WebControls.Unit(DR.Item("Width"), UnitType.Pixel)
                txt.Visible = False
                DT = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectPinnacle("Select Code,[Wharehouse Name] as name from [Metropolitan$TR Warehouse Hub] with(nolock) where [Active Status]=1")
                DDl.Items.Clear()
                Dim LI As ListItem
                Dim LIAll As New ListItem
                LIAll.Text = "All"
                LIAll.Value = "''"
                DDl.Items.Add(LIAll)
                For i = 0 To DT.Rows.Count - 1
                    LI = New ListItem
                    If DT.Rows(i).Item("Code") = ParaValue Then
                        LI.Selected = True
                    End If
                    LI.Text = String.Format("{0} - {1}", DT.Rows(i).Item("name"), DT.Rows(i).Item("Code"))
                    LI.Value = String.Format("'{0}'", DT.Rows(i).Item("Code"))
                    LIAll.Value = String.Format("{0},'{1}'", LIAll.Value, DT.Rows(i).Item("Code"))
                    DDl.Items.Add(LI)
                Next
            ElseIf DR.Item("Para_Type") = PinnaclePlusGlobals.Report_Para_Type.CheckBox Then
            ElseIf DR.Item("Para_Type") = PinnaclePlusGlobals.Report_Para_Type.Cur_User Then
                txt.Text = PinnaclePlus.Security.CurrentUser.ID
                txt.Enabled = False
            ElseIf DR.Item("Para_Type") = PinnaclePlusGlobals.Report_Para_Type.File Then
                FileUL.Visible = True
                txt.Visible = False
                DDl.Visible = False
            End If
        End If
    End Sub

    Private Sub btnView_Click(sender As Object, e As EventArgs) Handles btnView.Click
        ViewXL(False)
    End Sub
    Private Sub btnXL_Click(sender As Object, e As EventArgs) Handles btnXL.Click
        ViewXL(True)
    End Sub
    Private Sub ViewXL(XL As Boolean)
        Try

            Dim Query As String
            Dim DR As DataRow
            Dim DT As DataTable
            Dim Txt As TextBox
            Dim FileUL As FileUpload
            Dim RawTxt As String = ""
            Dim DDL As DropDownList
            Dim ParaValue As String = ""
            Dim Dat As Date
            DR = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleRow(String.Format("select [REP_ID],[Name],[Query],[Description],isnull(Is_PP,0) as Is_PP  from T_Report where REP_ID={0}", Rep_Value.Value))
            Query = DR.Item("Query")
            DT = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect(String.Format("select * from T_Report_Para where REP_ID={0} order by Order_", Rep_Value.Value))
            For i = 0 To DT.Rows.Count - 1
                Txt = gvData.Rows(i).Cells(2).FindControl("txtPara")
                DDL = gvData.Rows(i).Cells(2).FindControl("ddlPara")
                FileUL = gvData.Rows(i).Cells(2).FindControl("FileUpload1")
                If DT.Rows(i).Item("Para_Type") = PinnaclePlusGlobals.Report_Para_Type.GeneralText Then
                    RawTxt = Txt.Text
                    ParaValue = Txt.Text
                ElseIf DT.Rows(i).Item("Para_Type") = PinnaclePlusGlobals.Report_Para_Type.OrderNo Then
                    RawTxt = Txt.Text
                    Txt.Text = SetOrders(Txt.Text, True)
                    ParaValue = Txt.Text
                ElseIf DT.Rows(i).Item("Para_Type") = PinnaclePlusGlobals.Report_Para_Type.OrderNo_Outer Then
                    RawTxt = Txt.Text
                    Txt.Text = String.Format("{1}{0}{1}", SetOrders(Txt.Text, False), "'")
                    ParaValue = Txt.Text
                ElseIf DT.Rows(i).Item("Para_Type") = PinnaclePlusGlobals.Report_Para_Type.TrackingNo Then
                    RawTxt = Txt.Text
                    Txt.Text = SetTrack(Txt.Text, True)
                    ParaValue = Txt.Text
                ElseIf DT.Rows(i).Item("Para_Type") = PinnaclePlusGlobals.Report_Para_Type.TrackingNo_Outer Then
                    RawTxt = Txt.Text
                    Txt.Text = String.Format("{1}{0}{1}", SetTrack(Txt.Text, False), "'")
                    ParaValue = Txt.Text
                ElseIf DT.Rows(i).Item("Para_Type") = PinnaclePlusGlobals.Report_Para_Type.ClientList Then
                    RawTxt = Txt.Text
                    ParaValue = SetClientNos(Txt.Text)
                ElseIf DT.Rows(i).Item("Para_Type") = PinnaclePlusGlobals.Report_Para_Type.State_List Then
                    RawTxt = Txt.Text
                    ParaValue = SetStateNos(Txt.Text)
                ElseIf DT.Rows(i).Item("Para_Type") = PinnaclePlusGlobals.Report_Para_Type.Date_ Then
                    RawTxt = Txt.Text
                    Dat = CDate(Txt.Text)
                    ParaValue = String.Format("{3}{0}-{1}-{2}{3}", Dat.Year, Dat.Month, Dat.Day, "'")
                ElseIf DT.Rows(i).Item("Para_Type") = PinnaclePlusGlobals.Report_Para_Type.Hub_List Then
                    RawTxt = DDL.SelectedItem.Value
                    ParaValue = DDL.SelectedItem.Value
                ElseIf DT.Rows(i).Item("Para_Type") = PinnaclePlusGlobals.Report_Para_Type.CheckBox Then
                    'RawTxt = DDL.SelectedItem.Value
                    'ParaValue = DDL.SelectedItem.Value
                ElseIf DT.Rows(i).Item("Para_Type") = PinnaclePlusGlobals.Report_Para_Type.Cur_User Then
                    RawTxt = Txt.Text
                    ParaValue = RawTxt
                ElseIf DT.Rows(i).Item("Para_Type") = PinnaclePlusGlobals.Report_Para_Type.File Then
                    If FileUL.HasFile Then
                        Using reader = New StreamReader(FileUL.FileContent)
                            Dim value As String = reader.ReadToEnd()
                            RawTxt = value.Replace(Chr(13), "~").Replace(Chr(10), "")
                        End Using
                    Else
                        RawTxt = ""
                    End If

                    ParaValue = RawTxt
                End If
                PinnaclePlus.SQLData.Reports.P_Users_Report_Para_IU(PinnaclePlus.Security.CurrentUser.ID, DT.Rows(i).Item("RP_ID"), RawTxt)
                Query = Query.Replace("{" & i & "}", ParaValue)
            Next
            If DR.Item("Is_PP") Then
                DT = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect(Query)
            Else
                DT = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectPinnacle(Query)
            End If

            If XL Then
                ExportToExcel(DR.Item("Name"), "Sheet1", DT)
            Else
                GridView1.DataSource = DT
                GridView1.DataBind()
            End If
            PinnaclePlus.SQLData.Reports.P_Users_Report_UseCount(PinnaclePlus.Security.CurrentUser.ID, Rep_Value.Value)
        Catch ex As Exception
            litError.Text = ex.Message.ToString
            litError.Visible = True
        End Try
    End Sub
    Private Function SetClientNos(StrClients As String) As String
        'URBAN OUTFITTERS/ ANTHROPOLOGIE~C100108, BRUNSCHWIG & FILS~C100018, 
        Dim StrRet As String = ""
        Dim ClientNo As String
        Dim A(), B()
        A = StrClients.Split(",")
        For Each ClientNo In A
            If ClientNo.Trim <> "" Then
                B = ClientNo.Split("~")
                If CStr(B(1)).Trim <> "" Then

                    If StrRet = "" Then
                        StrRet = String.Format("'{0}'", CStr(B(1)).Trim)
                    Else
                        StrRet = String.Format("{0},'{1}'", StrRet, CStr(B(1)).Trim)
                    End If
                End If
            End If
        Next
        Return StrRet
    End Function
    Private Function SetStateNos(StrStates As String) As String

        Dim StrRet As String = ""
        Dim ClientNo As String
        Dim A(), B()
        A = StrStates.Split(",")
        For Each ClientNo In A
            If ClientNo.Trim <> "" Then
                B = ClientNo.Split(":")
                If CStr(B(1)).Trim <> "" Then

                    If StrRet = "" Then
                        StrRet = String.Format("'{0}'", CStr(B(1)).Trim)
                    Else
                        StrRet = String.Format("{0},'{1}'", StrRet, CStr(B(1)).Trim)
                    End If
                End If
            End If
        Next
        Return StrRet
    End Function

    Private Sub btnGo_Click(sender As Object, e As EventArgs) Handles btnGo.Click
        FillPara()
    End Sub
End Class