Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports iTextSharp.text.pdf
Module PinnaclePlusGlobals
    Public Admission_Close As Date
    Public Admission_Status As Integer
    Public Session_Name As String
    Public Hostel_Seats As Integer

    Public Sub GetDocument(MSOED_ID As Integer)
        Dim DR As DataRow

        DR = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleRow(String.Format("select * from T_Manifest_Stop_Order_Event_Doc where MSOED_ID={0}", MSOED_ID))
        Dim binaryData() As Byte = Convert.FromBase64String(DR.Item("DocumentData"))
        If DR.Item("DocumentExtType") = "pdf" Then
            HttpContext.Current.Response.ContentType = "application/pdf"
        Else
            HttpContext.Current.Response.ContentType = String.Format("image/{0}", DR.Item("DocumentExtType"))
        End If
        HttpContext.Current.Response.ContentType = "application/octet-stream"
        HttpContext.Current.Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.{1}", DR.Item("DocumentName"), DR.Item("DocumentExtType")))
        HttpContext.Current.Response.OutputStream.Write(binaryData, 0, binaryData.Length)
        HttpContext.Current.Response.Flush()
    End Sub
    Public Enum Report_Para_Type
        GeneralText = 10
        OrderNo = 20
        OrderNo_Outer = 21
        TrackingNo = 30
        TrackingNo_Outer = 31
        Date_ = 40
        ClientList = 50
        CheckBox = 60
        Hub_List = 70
        State_List = 80
        Cur_User = 90
        File = 100
    End Enum
    Public Function InvertColor(OldColor As String) As String
        Dim MyColor As System.Drawing.Color
        MyColor = System.Drawing.ColorTranslator.FromHtml(OldColor)

        MyColor = System.Drawing.Color.FromArgb(MyColor.ToArgb() Xor &HFFFFFF)
        Return System.Drawing.ColorTranslator.ToHtml(MyColor)
    End Function
    Public Function GetUnixTime(DT As Date) As Object
        Dim Ret As Object
        Ret = (DT - New DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
        Return Ret
    End Function
    Public Function GetDateFromUnixTime(UnixTime As Long) As Date
        Dim DD As Date = New DateTime(1970, 1, 1, 0, 0, 0)
        DD = DD.AddMilliseconds(UnixTime)
        Return DD
    End Function
    Public Function SetCellNumber(Cell As String) As String
        Dim Ret As String
        Ret = Cell.Replace("-", "")
        Ret = Ret.Substring(1, Ret.Length - 1)
        Ret = String.Format("+92{0}", Ret)
        Return Ret
    End Function
    Public Function GetDBLoginFromConnectionString(Part As String) As String
        Dim A(), B() As String
        Dim NV_Pare As String
        A = ConfigurationManager.ConnectionStrings("SqlStr").ConnectionString.Split(";")
        For Each NV_Pare In A
            B = NV_Pare.Split("=")
            If B(0).ToUpper.Trim = Part.ToUpper.Trim Then
                Return B(1)
            End If
        Next
        Return Nothing
    End Function
    Public Sub GetTaskSheetXL(ByVal Date_ As DateTime, Hub As String)

        'Dim FileName As String
        'FileName = HttpContext.Current.Server.MapPath(String.Format("~/challan_Std/Fee_Challan_{0}_{1}.pdf", A_ID, ID_Code))

        Using Rpt As New TaskSheetReport1
            'Dim path1 As String = My.Application.Info.DirectoryPath '' path
            '' for normal ADO Concept
            '' The current DataSource is an ADO.
            '' change path of the database
            Rpt.DataSourceConnections.Item(0). _
                    SetConnection(GetDBLoginFromConnectionString("Data Source"), GetDBLoginFromConnectionString("Initial Catalog"), False)
            '' if password is given then give the password
            '' if not give it will ask at runtime
            Rpt.DataSourceConnections.Item(0).SetLogon(GetDBLoginFromConnectionString("User ID"), GetDBLoginFromConnectionString("Password"))
            Rpt.SetParameterValue("P_Add_Ticket", False)
            Rpt.RecordSelectionFormula = String.Format("cdate({{V_RPT_ManSheet.StartDate}})=datetime(""{0}"") and {{V_RPT_ManSheet.Hub}}=""{1}""", Date_.AddHours(0).ToString, Hub)
            '
            If Rpt.HasSavedData Then
                Rpt.Refresh()
            End If
            Rpt.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.ExcelWorkbook, HttpContext.Current.Response, True, String.Format("Manifest-{0}-{1}-{2}-{3}", Hub, Date_.Year, Date_.Month, Date_.Day))
        End Using
    End Sub

    Public Sub GetTaskSheet(ByVal Date_ As DateTime, Hub As String, Optional add_Ticket As Boolean = True)

        'Dim FileName As String
        'FileName = HttpContext.Current.Server.MapPath(String.Format("~/challan_Std/Fee_Challan_{0}_{1}.pdf", A_ID, ID_Code))

        Using Rpt As New TaskSheetReport1


            'Dim path1 As String = My.Application.Info.DirectoryPath '' path
            '' for normal ADO Concept
            '' The current DataSource is an ADO.
            '' change path of the database
            Rpt.DataSourceConnections.Item(0). _
                    SetConnection(GetDBLoginFromConnectionString("Data Source"), GetDBLoginFromConnectionString("Initial Catalog"), False)
            '' if password is given then give the password
            '' if not give it will ask at runtime
            Rpt.DataSourceConnections.Item(0).SetLogon(GetDBLoginFromConnectionString("User ID"), GetDBLoginFromConnectionString("Password"))
            Rpt.SetParameterValue("P_Add_Ticket", add_Ticket)
            Rpt.RecordSelectionFormula = String.Format("cdate({{V_RPT_ManSheet.StartDate}})=datetime(""{0}"") and {{V_RPT_ManSheet.Hub}}=""{1}""", Date_.AddHours(0).ToString, Hub)
            If Rpt.HasSavedData Then
                Rpt.Refresh()
            End If

            Rpt.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, True, String.Format("Manifest-{0}-{1}-{2}-{3}", Hub, Date_.Year, Date_.Month, Date_.Day))
        End Using
    End Sub
    Public Sub GetTaskSheetNew(ByVal Date_ As DateTime, Hub As String, Optional add_Ticket As Boolean = True)
        Dim Arr As New List(Of System.IO.MemoryStream)
        Dim Barr() As Byte
        Dim Strm As System.IO.MemoryStream
        'Dim FileName As String
        'FileName = HttpContext.Current.Server.MapPath(String.Format("~/challan_Std/Fee_Challan_{0}_{1}.pdf", A_ID, ID_Code))
        
        Dim DT As DataTable
        DT = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect(String.Format("select MIN_ID from T_Manifest where [Hub]='{0}' and  cast([StartDate] as date)=cast('{1}' as date)", Hub, Date_))
        For i = 0 To DT.Rows.Count - 1
            Using Rpt As New TaskSheetReport1
                'Dim path1 As String = My.Application.Info.DirectoryPath '' path
                '' for normal ADO Concept
                '' The current DataSource is an ADO.
                '' change path of the database
                Rpt.DataSourceConnections.Item(0). _
                        SetConnection(GetDBLoginFromConnectionString("Data Source"), GetDBLoginFromConnectionString("Initial Catalog"), False)
                '' if password is given then give the password
                '' if not give it will ask at runtime
                Rpt.DataSourceConnections.Item(0).SetLogon(GetDBLoginFromConnectionString("User ID"), GetDBLoginFromConnectionString("Password"))
                Rpt.SetParameterValue("P_Add_Ticket", add_Ticket)
                Rpt.RecordSelectionFormula = String.Format("{{V_RPT_ManSheet.MIN_ID}}={0}", DT.Rows(i).Item("MIN_ID"))
                If Rpt.HasSavedData Then
                    Rpt.Refresh()
                End If
                Strm = Rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat)
                Arr.Add(Strm)
                'HttpContext.Current.Response, True, 
            End Using
        Next

        Dim finalStream As New System.IO.MemoryStream
        Dim Copy As New PdfCopyFields(finalStream)
        
        For Each Strm In Arr
            Copy.AddDocument(New PdfReader(Strm))
            Strm.Dispose()
        Next
        Copy.Close()
        Barr = finalStream.ToArray()
        finalStream.Close()
        HttpContext.Current.Response.Clear()
        HttpContext.Current.Response.ContentType = "application/force-download"
        HttpContext.Current.Response.AddHeader("content-disposition", String.Format("attachment; filename={0}.pdf", String.Format("Manifest-{0}-{1}-{2}-{3}", Hub, Date_.Year, Date_.Month, Date_.Day)))
        HttpContext.Current.Response.BinaryWrite(Barr)
        HttpContext.Current.Response.End()

    End Sub
    Public Sub GetTaskSheetForMin(ByVal MIN_ID As Integer, Hub As String, Optional add_Ticket As Boolean = True)

        'Dim FileName As String
        'FileName = HttpContext.Current.Server.MapPath(String.Format("~/challan_Std/Fee_Challan_{0}_{1}.pdf", A_ID, ID_Code))

        Using Rpt As New TaskSheetReport1


            'Dim path1 As String = My.Application.Info.DirectoryPath '' path
            '' for normal ADO Concept
            '' The current DataSource is an ADO.
            '' change path of the database
            Rpt.DataSourceConnections.Item(0). _
                    SetConnection(GetDBLoginFromConnectionString("Data Source"), GetDBLoginFromConnectionString("Initial Catalog"), False)
            '' if password is given then give the password
            '' if not give it will ask at runtime
            Rpt.DataSourceConnections.Item(0).SetLogon(GetDBLoginFromConnectionString("User ID"), GetDBLoginFromConnectionString("Password"))

            Rpt.SetParameterValue("P_Add_Ticket", add_Ticket)
            Rpt.RecordSelectionFormula = String.Format("{{V_RPT_ManSheet.MIN_ID}}={0}", MIN_ID)
            '

            If Rpt.HasSavedData Then
                Rpt.Refresh()
            End If

            Rpt.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, True, String.Format("Manifest-{0}-{1}", Hub, MIN_ID))
        End Using
    End Sub


    Public Sub GetPullSheet(ByVal Date_ As Date, Hub As String)
        'RefreshPull(Date_, Hub)
        'Dim FileName As String
        'FileName = HttpContext.Current.Server.MapPath(String.Format("~/challan_Std/Fee_Challan_{0}_{1}.pdf", A_ID, ID_Code))

        Using Rpt As New rpt_Pull
            'Dim path1 As String = My.Application.Info.DirectoryPath '' path
            '' for normal ADO Concept
            '' The current DataSource is an ADO.
            '' change path of the database
            Rpt.DataSourceConnections.Item(0). _
                    SetConnection(GetDBLoginFromConnectionString("Data Source"), GetDBLoginFromConnectionString("Initial Catalog"), False)
            '' if password is given then give the password
            '' if not give it will ask at runtime
            Rpt.DataSourceConnections.Item(0).SetLogon(GetDBLoginFromConnectionString("User ID"), GetDBLoginFromConnectionString("Password"))


            Rpt.RecordSelectionFormula = String.Format("cdate({{V_RPT_ManSheet.StartDate}})=datetime(""{0}"") and {{V_RPT_ManSheet.Hub}}=""{1}""", Date_.AddHours(0).ToString, Hub)


            If Rpt.HasSavedData Then
                Rpt.Refresh()
            End If

            Rpt.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, True, String.Format("PullSheet-{0}-{1}-{2}-{3}", Hub, Date_.Year, Date_.Month, Date_.Day))
        End Using
    End Sub
    Public Sub GetPullSheetMin(ByVal MIN_ID As Integer, Hub As String)
        'RefreshPull(Date_, Hub)
        'Dim FileName As String
        'FileName = HttpContext.Current.Server.MapPath(String.Format("~/challan_Std/Fee_Challan_{0}_{1}.pdf", A_ID, ID_Code))

        Using Rpt As New rpt_Pull

            'Dim path1 As String = My.Application.Info.DirectoryPath '' path
            '' for normal ADO Concept
            '' The current DataSource is an ADO.
            '' change path of the database
            Rpt.DataSourceConnections.Item(0). _
                    SetConnection(GetDBLoginFromConnectionString("Data Source"), GetDBLoginFromConnectionString("Initial Catalog"), False)
            '' if password is given then give the password
            '' if not give it will ask at runtime
            Rpt.DataSourceConnections.Item(0).SetLogon(GetDBLoginFromConnectionString("User ID"), GetDBLoginFromConnectionString("Password"))


            Rpt.RecordSelectionFormula = String.Format("{{V_RPT_ManSheet.MIN_ID}}={0}", MIN_ID)


            If Rpt.HasSavedData Then
                Rpt.Refresh()
            End If

            Rpt.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, True, String.Format("PullSheet-{0}-{1}", Hub, MIN_ID))
        End Using
    End Sub
    'Private Sub RefreshPull(ByVal Date_ As Date, Hub As String)
    '    Dim DT, DT_PIn As DataTable
    '    Dim OrderStr As String
    '    DT = PinnaclePlus.OrderBatch.P_Batch_Orders_Get_ByDate(Date_, "")
    '    OrderStr = "'0'"
    '    For i = 0 To DT.Rows.Count - 1
    '        OrderStr = String.Format("{0},'{1}'", OrderStr, DT.Rows(i).Item("Order_ID"))
    '    Next
    '    DT_PIn = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectPinnacle(String.Format("select * from V_PullSheet where OrderNo in ({0})", OrderStr))
    '    PinnaclePlus.SQLData.GeneralOperations.ExecuteNONQuery(String.Format("delete from T_Pull_Sheet where OrderNo  in ({0})", OrderStr), "", "")
    '    For i = 0 To DT_PIn.Rows.Count - 1
    '        PinnaclePlus.OrderBatch.P_Pull_Sheet_Insert(DT_PIn.Rows(i).Item("OrderNo"), DT_PIn.Rows(i).Item("Barcode"), DT_PIn.Rows(i).Item("Item_Description"), DT_PIn.Rows(i).Item("LocationCode"), DT_PIn.Rows(i).Item("Location"), DT_PIn.Rows(i).Item("Warehouse"), DT_PIn.Rows(i).Item("Status"))
    '    Next
    'End Sub
    Public Function GetNullVal(Nvl As Object) As Object
        If IsDBNull(Nvl) Then
            Return Nothing
        Else
            Return Nvl
        End If

    End Function

End Module
