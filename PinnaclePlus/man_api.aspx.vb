Imports System.Web.Services
Imports System.Data.SqlClient
Public Class SaveManRetInfo
    Public Property Err As String
    Public Property MIN_ID As Integer
End Class

Public Class ReportData
    Public Property Rep_ID As Integer
    Public Property Rep_Name As String
End Class
Public Class GetManDisplayDataInfo
    Public Property Err As String
    Public Property Man_Str As String
    Public Property Seq_Str As String
    Public Property Insert_Str As String
    Public Property Man_info As String
    Public Property Man_Locked As Boolean
    Public Property NotEditable As Boolean
    Public Property nv_Done As Boolean
End Class
Public Class man_api
    Inherits System.Web.UI.Page
    <WebMethod()> _
    Public Shared Function InsertStop(MIN_ID As Integer, MS_ID As Integer, Seq As Integer, BeforeAfter As Integer, StopStr As String) As Integer
        'Dim MaxSeq As Integer
        'Dim Dr, Dr_Before, Dr_After As DataRow
        'Dim Before_MS, After_MS, This_MS As Integer
        'Seq = Seq + 1
        'Dim Before_LatLong As New LatLng, After_LatLong As New LatLng, This_LatLong As New LatLng

        'If (Seq = 1 And BeforeAfter = 1) Or (MaxSeq = Seq And BeforeAfter = 2) Then
        '    
        'End If
        'If Seq = 1 And BeforeAfter = 1 Then
        '    Before_LatLong.lat = Dr.Item("Lat")
        '    Before_LatLong.lng = Dr.Item("Lng")
        'ElseIf MaxSeq = Seq And BeforeAfter = 2 Then
        '    After_LatLong.lat = Dr.Item("Lat")
        '    After_LatLong.lng = Dr.Item("Lng")
        'Else
        '    If BeforeAfter = 1 Then
        '        Dr_Before = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleRow(String.Format("select MS_ID,Lat,Long from V_Manifest_Stop where MIN_ID={0} and Seq={1}", MIN_ID, Seq - 1))
        '        Dr_After = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleRow(String.Format("select MS_ID,Lat,Long from V_Manifest_Stop where MS_ID={0}", MS_ID))
        '    Else
        '        Dr_Before = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleRow(String.Format("select MS_ID,Lat,Long from V_Manifest_Stop where MS_ID={0}", MS_ID))
        '        Dr_After = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleRow(String.Format("select MS_ID,Lat,Long from V_Manifest_Stop where MIN_ID={0} and Seq={1}", MIN_ID, Seq + 1))
        '    End If
        'End If

        'Dim Dr_Before, Dr_After, Dr, DrTemp As DataRow
        'Dim StartLatlong As New LatLng, EndLatLon As New LatLng
        'Dr = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleRow(String.Format("select MS_ID,Lat,Long from V_Manifest_Stop where MS_ID={0}", NewMS_ID))
        'Dr_Before = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleRow(String.Format("select MS_ID,Lat,Long from V_Manifest_Stop where MIN_ID={0} and Seq={1}", MIN_ID, Dr.Item("Seq") - 1))
        'Dr_After = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleRow(String.Format("select MS_ID,Lat,Long from V_Manifest_Stop where MIN_ID={0} and Seq={1}", MIN_ID, Dr.Item("Seq") + 1))
        'If Dr_Before Is Nothing Then
        '    DrTemp = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleRow(String.Format("select Lat,Lng from T_Hub where HUB_CODE=(select Hub from T_Manifest where MIN_ID={0})", MIN_ID))
        '    StartLatlong.lat = DrTemp.Item("Lat")
        '    StartLatlong.lng = DrTemp.Item("lng")
        'End If
        Dim NewMS_ID As Integer
        NewMS_ID = PinnaclePlus.SQLData.Dispatch.P_Manifest_Stop_Insert(StopStr, MS_ID, IIf(BeforeAfter = 1, False, True))
        PinnaclePlus.Google.GoogleApi.RouteOptimizationDistance(MIN_ID, False)
    End Function

    <WebMethod()> _
    Public Shared Function LockMan(MIN_ID As Integer) As Boolean
        Return PinnaclePlus.SQLData.Dispatch.P_Manifest_Lock(MIN_ID)
    End Function
    <WebMethod()> _
    Public Shared Function SaveMan(Man As ManifestInfo) As SaveManRetInfo
        Dim Ret As New SaveManRetInfo
        Ret.Err = ""
        With Man
            Ret.MIN_ID = PinnaclePlus.SQLData.Dispatch.P_Manifest_IU(.MIN_ID, .Hub, .DRIVER_ID, .CO_DRIVER_ID1, .CO_DRIVER_ID2, .TRUCK_ID, .StartDate, .ActualStartDate, .Running_Hours, .Delivery_Time, .TimeWindow, .Notes, .Is_Multiday, .Color, .Stops)
            If .MIN_ID = 0 Then
                Ret.Err = PinnaclePlus.Google.GoogleApi.RouteOptimizationDistance(Ret.MIN_ID, False)
            ElseIf .Stops <> "" Then
                Ret.Err = PinnaclePlus.Google.GoogleApi.RouteOptimizationDistance(Ret.MIN_ID, False)
            Else
                PinnaclePlus.SQLData.Dispatch.P_UpdateTimeWindow(Ret.MIN_ID)
            End If
        End With
        Return Ret
    End Function
    <WebMethod()> _
    Public Shared Function DelMan(MIN_ID) As String
        Dim Err As String
        Dim IsExported As Boolean
        IsExported = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleValue(String.Format("select isnull(ExportednuVizz,0) from T_Manifest where MIN_ID={0}", MIN_ID))
        If IsExported Then
            Err = PinnaclePlus.nuVizz.nuVizzAPI.ExportLoad(MIN_ID, True)
        Else
            Err = ""
        End If
        If Err = "" Then
            PinnaclePlus.SQLData.Dispatch.P_Manifest_Del(MIN_ID)
        Else
            Return Err
        End If

        Return Err
    End Function
    <WebMethod()> _
    Public Shared Function DelStop(MS_ID As String, MIN_ID As Integer) As Integer
        Dim A() As String
        Dim ms As String
        A = MS_ID.Split(",")
        For Each ms In A
            PinnaclePlus.SQLData.Dispatch.P_Manifest_Stop_Del(ms)
        Next

        PinnaclePlus.Google.GoogleApi.RouteOptimizationDistance(MIN_ID, False)
        Return 1
    End Function
    <WebMethod()> _
    Public Shared Function ToggleDayBreak(MS_ID As Integer, MIN_ID As Integer) As Integer
        PinnaclePlus.SQLData.Dispatch.P_Manifest_Stop_DayBreak(MS_ID)
        PinnaclePlus.SQLData.Dispatch.P_UpdateTimeWindow(MIN_ID)
        Return 1
    End Function


    <WebMethod()> _
    Public Shared Function Export_NV(MIN_ID) As String
        Return PinnaclePlus.nuVizz.nuVizzAPI.ExportLoad(MIN_ID, False)

    End Function
    <WebMethod()> _
    Public Shared Function InvertSeq(MIN_ID) As Integer
        PinnaclePlus.SQLData.Dispatch.P_Manifest_Stop_OrderUpSideDow(MIN_ID)
        PinnaclePlus.Google.GoogleApi.RouteOptimizationDistance(MIN_ID, False)
        Return 1
    End Function

    <WebMethod()> _
    Public Shared Function FillManList(Date_ As Date, Hub As String, Man_ToSelect As Integer) As String
        Dim LiTText As String
        Dim ForColor As String

        If Not IsDate(Date_) Then
            Return ""
        End If
        Dim DT_LIst As DataTable
        DT_LIst = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect(String.Format("select * from V_Manifest where Hub='{0}' and cast(StartDate as date)='{1}-{2}-{3}' order by MIN_ID", Hub, CDate(Date_).Year, CDate(Date_).Month, CDate(Date_).Day))
        If Man_ToSelect = 0 Then
            LiTText = "<option value=""0"" selected=""selected"">Create New Manifest</option>"
        Else
            LiTText = "<option value=""0"">Create New Manifest</option>"
        End If


        For i = 0 To DT_LIst.Rows.Count - 1
            If IsDBNull(DT_LIst.Rows(i).Item("Color")) Then
                ForColor = "#000000"
            Else
                ForColor = InvertColor(DT_LIst.Rows(i).Item("Color"))
            End If
            If DT_LIst.Rows(i).Item("MIN_ID") = Man_ToSelect Then
                LiTText = String.Format("{0}<option value=""{1}"" selected=""selected"" style=""background:{3};color:{4}"">{2}</option>", LiTText, DT_LIst.Rows(i).Item("MIN_ID"), String.Format("{0}-{1}-{2}", DT_LIst.Rows(i).Item("MIN_ID"), DT_LIst.Rows(i).Item("DriverName"), DT_LIst.Rows(i).Item("TruckName")), DT_LIst.Rows(i).Item("Color"), ForColor)
            Else
                LiTText = String.Format("{0}<option value=""{1}"" style=""background:{3};color:{4}"">{2}</option>", LiTText, DT_LIst.Rows(i).Item("MIN_ID"), String.Format("{0}-{1}-{2}", DT_LIst.Rows(i).Item("MIN_ID"), DT_LIst.Rows(i).Item("DriverName"), DT_LIst.Rows(i).Item("TruckName")), DT_LIst.Rows(i).Item("Color"), ForColor)
            End If
        Next
        Return LiTText
    End Function
    Private Shared Sub RefreshManData(State As Object)
        Try
            HttpContext.Current.Session.Item("ManCach") = PinnaclePlus.SQLData.Dispatch.P_Manifest_Stop_Order_GetByMIN_ID(HttpContext.Current.Session.Item("ManCachID"))
        Catch ex As Exception

        End Try

    End Sub
    <WebMethod()> _
    Public Shared Function GetManDisplayData(MIN_ID As Integer) As GetManDisplayDataInfo
        Dim OrdDate As Object
        Dim ErrMsg As String
        Dim Tbl As String
        Dim OrderCount_pic As Integer
        Dim ItemCount_pic As Integer
        Dim Wt_pic As Integer
        Dim Cubes_pic As Integer
        Dim Rev_pic As Integer

        Dim OrderCount_del As Integer
        Dim ItemCount_del As Integer
        Dim Wt_del As Integer
        Dim Cubes_del As Integer
        Dim Rev_Del As Integer

        Dim ETA = New Date(1978, 12, 27)
        Dim Ret As New GetManDisplayDataInfo
        Dim StrSeq As String = ""
        Ret.Err = ""
        Ret.Man_Str = ""
        Ret.Seq_Str = ""
        Ret.NotEditable = False
        Dim LiTText As String
        If MIN_ID = "0" Then
            Return Ret
            Exit Function
        End If
        Dim DT As DataTable
        Dim DrMan As DataRow
        Dim DT_Orders As DataTable
        Dim OrdersRows() As DataRow
        DrMan = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleRow(String.Format("select *,dbo.F_Manifest_Get_Miles(MIN_ID) as Miles from V_Manifest where MIN_ID={0}", MIN_ID))
        'If HttpContext.Current.Session.Item("ManCach") Is Nothing Then
        DT_Orders = PinnaclePlus.SQLData.Dispatch.P_Manifest_Stop_Order_GetByMIN_ID(MIN_ID)
        'HttpContext.Current.Session.Add("ManCach", DT_Orders)
        'HttpContext.Current.Session.Add("ManCachID", MIN_ID)
        'ElseIf HttpContext.Current.Session.Item("ManCachID") <> MIN_ID Then
        'DT_Orders = PinnaclePlus.SQLData.Dispatch.P_Manifest_Stop_Order_GetByMIN_ID(MIN_ID)
        'HttpContext.Current.Session.Item("ManCach") = DT_Orders
        'HttpContext.Current.Session.Item("ManCachID") = MIN_ID
        'Else
        'DT_Orders = HttpContext.Current.Session.Item("ManCach")
        'System.Threading.ThreadPool.QueueUserWorkItem(AddressOf RefreshManData)
        'End If

        Ret.nv_Done = DrMan.Item("ExportednuVizz")
        If Not IsDBNull(DrMan.Item("Status")) Then
            Ret.NotEditable = True
        End If
        If IsDBNull(DrMan.Item("Locked")) Then
            Ret.Man_Locked = False
        Else
            Ret.Man_Locked = DrMan.Item("Locked")
        End If
        LiTText = ""
        Ret.Err = ""
        DT = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect(String.Format("select * from V_Manifest_Stop where MIN_ID={0} order by Seq", MIN_ID))
        StrSeq = "<ul id=""sortable"">"
        Dim Insert_Str As String = ""
        LiTText = String.Format("{0}<table style=""width:100%;border-spacing: 0;border-collapse:collapse;border:solid 1px #000000"">", LiTText)
        For i = 0 To DT.Rows.Count - 1
            If DrMan.Item("is_Multiday") Then
                If ETA <> DateSerial(CDate(DT.Rows(i).Item("ETA")).Year, CDate(DT.Rows(i).Item("ETA")).Month, CDate(DT.Rows(i).Item("ETA")).Day) Then
                    LiTText = String.Format("{0}<tr><td style=""text-align: center; border: solid 1px #000000; background-color: #cccccc;font-weight:bold;"">", LiTText)
                    LiTText = String.Format("{0}{1}", LiTText, Format(DT.Rows(i).Item("ETA"), "MM/dd/yy"))
                    LiTText = String.Format("{0}</td></tr>", LiTText)
                End If
            End If
            ETA = DateSerial(CDate(DT.Rows(i).Item("ETA")).Year, CDate(DT.Rows(i).Item("ETA")).Month, CDate(DT.Rows(i).Item("ETA")).Day)
            LiTText = String.Format("{0}<tr><td style=""text-align:left;border: solid 1px #000000;"">", LiTText)
            LiTText = String.Format("{0}<div class=""selectable_old"" >", LiTText)
            LiTText = String.Format("{0}<span class='numberCircle' title=""Stop Sequence"">{1}</span> <span class='numberCirclemi' title=""Distance in Miles"">{2}</span>", LiTText, DT.Rows(i).Item("Seq"), DT.Rows(i).Item("Distance_Mi"))

            LiTText = String.Format("{0}{1}, {2}, {3} {4}", LiTText, DT.Rows(i).Item("Address"), DT.Rows(i).Item("City"), DT.Rows(i).Item("State"), DT.Rows(i).Item("Zip"))

            LiTText = String.Format("{0}<span style=""margin-left:auto;float:right;"">", LiTText)
            LiTText = String.Format("{0}<a href=""javascript:GoTo({1},{2});"" title=""Zoom on this stop""><img src=""icon/pin.png""  /></a>", LiTText, DT.Rows(i).Item("lat"), DT.Rows(i).Item("lng"))
            If IsDBNull(DT.Rows(i).Item("Status")) Then
                'LiTText = String.Format("{0}<a href=""javascript:DelStop({1});"" title=""Delete Stop""><img src=""icon/cross.gif""  /></a>", LiTText, DT.Rows(i).Item("MS_ID"))
                LiTText = String.Format("{0}<input type=""checkbox"" class=""MS_ID_DEL"" value=""{1}"" id=""MS_ID_DEL{1}"">", LiTText, DT.Rows(i).Item("MS_ID"))
            End If
            LiTText = String.Format("{0}</span>", LiTText)
            If IsDBNull(DT.Rows(i).Item("Status")) Then
                LiTText = String.Format("{0}<div style=""width:100%"">ETA:<b>{1}</b>; Win:<a href=""javascript:changeTimeFrame({4});""><b>{2}-{3}</b></a></div>", _
                                        LiTText, _
                                        Format(DT.Rows(i).Item("ETA"), "MM/dd/yy hh:mm tt"), _
                                        Format(DT.Rows(i).Item("TW_Start"), "hh:mm tt"), _
                                        Format(DT.Rows(i).Item("TW_End"), "hh:mm tt"), _
                                        DT.Rows(i).Item("MS_ID"))
            Else
                LiTText = String.Format("{0}<div style=""width:100%"">ETA:<b>{1}</b>; Win:<b>{2}-{3}</b></div>", _
                                        LiTText, _
                                        Format(DT.Rows(i).Item("ETA"), "MM/dd/yy hh:mm tt"), _
                                        Format(DT.Rows(i).Item("TW_Start"), "hh:mm tt"), _
                                        Format(DT.Rows(i).Item("TW_End"), "hh:mm tt"))
            End If



            'DT_Orders = PinnaclePlus.SQLData.Dispatch.P_Manifest_Stop_Order_GetByStopID(DT.Rows(i).Item("MS_ID"))
            'For j = 0 To DT_Orders.Rows.Count - 1

            '    If DT_Orders.Rows(j).Item("Is_Pickup") Then
            '        OrdDate = DT_Orders.Rows(j).Item("Sch_Date_Pic")
            '    Else
            '        OrdDate = DT_Orders.Rows(j).Item("Sch_Date_Del")
            '    End If
            '    If Not IsDBNull(OrdDate) Then
            '        OrdDate = DateSerial(OrdDate.Year, OrdDate.Month, OrdDate.Day)
            '        ErrMsg = ""
            '        If ETA <> OrdDate Then
            '            ErrMsg = String.Format("Order is Rescheduled to {0}", Format(OrdDate, "MM/dd/yy"))
            '            Ret.Err = "Orders in this manifest are  rescheduled"
            '        End If
            '    Else
            '        ErrMsg = "Order Sechdule Date Removed"
            '        Ret.Err = "Orders in this manifest are  rescheduled"
            '    End If
            '    LiTText = String.Format("{0}{1}", LiTText, GetInfoManifest(DT_Orders.Rows(j), DT.Rows(i).Item("Status"), ErrMsg))

            '    If DT_Orders.Rows(j).Item("is_pickup") Then
            '        OrderCount_pic = OrderCount_pic + 1
            '        ItemCount_pic = ItemCount_pic + DT_Orders.Rows(j).Item("Qty")
            '        Wt_pic = Wt_pic + DT_Orders.Rows(j).Item("Wt")
            '        Cubes_pic = Cubes_pic + DT_Orders.Rows(j).Item("CuFts")
            '        Rev_pic = Rev_pic + DT_Orders.Rows(j).Item("Est_Revenue")
            '    Else
            '        OrderCount_del = OrderCount_del + 1
            '        ItemCount_del = ItemCount_del + DT_Orders.Rows(j).Item("Qty")
            '        Wt_del = Wt_del + DT_Orders.Rows(j).Item("Wt")
            '        Cubes_del = Cubes_del + DT_Orders.Rows(j).Item("CuFts")
            '        Rev_Del = Rev_Del + DT_Orders.Rows(j).Item("Est_Revenue")
            '    End If
            'Next

            OrdersRows = DT_Orders.Select(String.Format("MS_ID={0}", DT.Rows(i).Item("MS_ID")))
            For j = 0 To OrdersRows.Length - 1

                If OrdersRows(j).Item("Is_Pickup") Then
                    OrdDate = OrdersRows(j).Item("Sch_Date_Pic")
                Else
                    OrdDate = OrdersRows(j).Item("Sch_Date_Del")
                End If
                If Not IsDBNull(OrdDate) Then
                    OrdDate = DateSerial(OrdDate.Year, OrdDate.Month, OrdDate.Day)
                    ErrMsg = ""
                    If ETA <> OrdDate Then
                        ErrMsg = String.Format("Order is Rescheduled to {0}", Format(OrdDate, "MM/dd/yy"))
                        Ret.Err = "Orders in this manifest are  rescheduled"
                    End If
                Else
                    ErrMsg = "Order Sechdule Date Removed"
                    Ret.Err = "Orders in this manifest are  rescheduled"
                End If
                LiTText = String.Format("{0}{1}", LiTText, GetInfoManifest(OrdersRows(j), DT.Rows(i).Item("Status"), ErrMsg))

                If OrdersRows(j).Item("is_pickup") Then
                    OrderCount_pic = OrderCount_pic + 1
                    ItemCount_pic = ItemCount_pic + OrdersRows(j).Item("Qty")
                    Wt_pic = Wt_pic + OrdersRows(j).Item("Wt")
                    Cubes_pic = Cubes_pic + OrdersRows(j).Item("CuFts")
                    Rev_pic = Rev_pic + OrdersRows(j).Item("Est_Revenue")
                Else
                    OrderCount_del = OrderCount_del + 1
                    ItemCount_del = ItemCount_del + OrdersRows(j).Item("Qty")
                    Wt_del = Wt_del + OrdersRows(j).Item("Wt")
                    Cubes_del = Cubes_del + OrdersRows(j).Item("CuFts")
                    Rev_Del = Rev_Del + OrdersRows(j).Item("Est_Revenue")
                End If
            Next


            LiTText = String.Format("{0}</div>", LiTText)
            LiTText = String.Format("{0}</td></tr>", LiTText)
            If DrMan.Item("is_Multiday") And DT.Rows(i).Item("Seq") <> DT.Rows.Count Then
                If DT.Rows(i).Item("Day_Break") Then
                    LiTText = String.Format("{0}<tr><td><div style=""text-align:center;""><a href=""javascript:ToggleDayBreak({1});"">Do Not Day Break Here</a></div></td></tr>", LiTText, DT.Rows(i).Item("MS_ID"))
                Else
                    LiTText = String.Format("{0}<tr><td><div style=""text-align:center;""><a href=""javascript:ToggleDayBreak({1});"">Day Break Here</a></div></td></tr>", LiTText, DT.Rows(i).Item("MS_ID"))
                End If
            End If
            ''Seq Change
            StrSeq = String.Format("{0}<li id=""set_{1}"" class=""ui-state-default"">", StrSeq, DT.Rows(i).Item("MS_ID"))
            StrSeq = String.Format("{0}<span class=""numberCircle"" title=""Stop Sequence"">{1}</span> <span class=""numberCirclemi"" title=""Distance in Miles"">{2}</span>", StrSeq, DT.Rows(i).Item("Seq"), DT.Rows(i).Item("Distance_Mi"))
            StrSeq = String.Format("{0}{1}, {2}, {3} {4}", StrSeq, DT.Rows(i).Item("Address"), DT.Rows(i).Item("City"), DT.Rows(i).Item("State"), DT.Rows(i).Item("Zip"))

            StrSeq = String.Format("{0}</li>", StrSeq)
            Insert_Str = String.Format("{0}<option value={1}>{6} - {2}, {3}, {4} {5}</option>", Insert_Str, DT.Rows(i).Item("MS_ID"), DT.Rows(i).Item("Address"), DT.Rows(i).Item("City"), DT.Rows(i).Item("State"), DT.Rows(i).Item("Zip"), DT.Rows(i).Item("Seq"))
            '' 
        Next
        LiTText = String.Format("{0}</table>", LiTText)
        StrSeq = String.Format("{0}</ul>", StrSeq)

        If Not IsDBNull(DrMan.Item("Status")) Then
            StrSeq = ""
        End If
        Tbl = "<table style='width:100%;font-weight:none;font-size:8pt;border-spacing: 0;border-collapse:collapse;'>"
        Tbl = String.Format("{0}<tr><td style='border: solid 1px #000000;text-align: center;'>Stops: {1} Miles: {2}</td><td style='text-align: center;border: solid 1px #000000;'>Revenue</td><td style='text-align: center;border: solid 1px #000000;'>Orders</td><td style='text-align: center;border: solid 1px #000000;'>Items</td><td style='text-align: center;border: solid 1px #000000;'>Wt</td><td style='text-align: center;border: solid 1px #000000;'>CuFts</td><tr>", Tbl, DT.Rows.Count, DrMan.Item("Miles"))
        Tbl = String.Format("{0}<tr><td style='border: solid 1px #000000;'>Pickup</td><td style='text-align: center;border: solid 1px #000000;'>{1} $</td><td style='text-align: center;border: solid 1px #000000;'>{2}</td><td style='text-align: center;border: solid 1px #000000;'>{3}</td><td style='text-align: center;border: solid 1px #000000;'>{4}</td><td style='text-align: center;border: solid 1px #000000;'>{5}</td><tr>", Tbl, Rev_pic, OrderCount_pic, ItemCount_pic, Wt_pic, Cubes_pic)
        Tbl = String.Format("{0}<tr><td style='border: solid 1px #000000;'>Delivery</td><td style='text-align: center;border: solid 1px #000000;'>{1} $</td><td style='text-align: center;border: solid 1px #000000;'>{2}</td><td style='text-align: center;border: solid 1px #000000;'>{3}</td><td style='text-align: center;border: solid 1px #000000;'>{4}</td><td style='text-align: center;border: solid 1px #000000;'>{5}</td><tr>", Tbl, Rev_Del, OrderCount_del, ItemCount_del, Wt_del, Cubes_del)
        Tbl = String.Format("{0}<tr><td style='border: solid 1px #000000;'>Total</td><td style='text-align: center;border: solid 1px #000000;'>{1} $</td><td style='text-align: center;border: solid 1px #000000;'>{2}</td><td style='text-align: center;border: solid 1px #000000;'>{3}</td><td style='text-align: center;border: solid 1px #000000;'>{4}</td><td style='text-align: center;border: solid 1px #000000;'>{5}</td><tr>", Tbl, (Rev_pic + Rev_Del), (OrderCount_del + OrderCount_pic), (ItemCount_del + ItemCount_pic), (Wt_del + Wt_pic), (Cubes_del + Cubes_pic))

        Tbl = String.Format("{0}</table>", Tbl)
        Ret.Man_info = Tbl
        Ret.Man_Str = LiTText
        Ret.Seq_Str = StrSeq
        Ret.Insert_Str = Insert_Str
        Return Ret
    End Function
    Private Shared Function GetInfoManifest(OrderRow As DataRow, MIN_Status As Object, ErrMsg As String) As String
        Dim BGColor As String

        Dim StrHtml As String
        Dim LinkTxt As String
        Dim Pic_Del As String
        If OrderRow.Item("Is_Pickup") Then
            Pic_Del = "Pic"
        Else
            Pic_Del = "Del"
        End If
        LinkTxt = "http://main.metropolitanwarehouse.com/Order/OrderDetail?eqs=" + PinnacleFunction.URLEncrypt.EncryptDesToHex(String.Format("OrderId={0}", OrderRow.Item("OrderNo")), "MetroCryptoUSA07306#?+")
        If ErrMsg <> "" Then
            BGColor = "background-color:#fc7e7e;"
        Else
            BGColor = ""
        End If

        StrHtml = String.Format("<span style='border-top :solid 1px #eeeeee;display:block;width: 100%;{0}'>", BGColor)
        If ErrMsg <> "" Then
            StrHtml = String.Format("{0}<span style='text-align:center;display:block;width: 100%; background-color:#ff0000;color:#ffffff;font-weight:bold;'>", StrHtml)
            StrHtml = String.Format("{0}{1}", StrHtml, ErrMsg)
            StrHtml = String.Format("{0}</span>", StrHtml)
        End If

        StrHtml = String.Format("{0}{1}, {2}<br/>", StrHtml, OrderRow.Item(String.Format("Name_{0}", Pic_Del)), OrderRow.Item(String.Format("Address2_{0}", Pic_Del)))
        StrHtml = String.Format("{0}Order No:<a href=""{1}"" target=""_blank"">{2}</a> <b>{3}</b> Type:<b>{4}{5} {6}</b>; <br/>", _
                                StrHtml, LinkTxt, _
                                OrderRow.Item("OrderNo"), _
                                IIf(OrderRow.Item("is_pickup"), "Pickup", "Delivery"), _
                                OrderRow.Item(String.Format("Type_Code_{0}", Pic_Del)), _
                                IIf(OrderRow.Item(String.Format("Sub_Type_Code_{0}", Pic_Del)) = "", "", String.Format("-{0}", OrderRow.Item(String.Format("Sub_Type_Code_{0}", Pic_Del)))), _
                                IIf(String.Format("{0}-{1}", OrderRow.Item(String.Format("Req_Hrs_From_{0}", Pic_Del)), OrderRow.Item(String.Format("Req_Hrs_To_{0}", Pic_Del))).Trim = "-", "", String.Format("{0}-{1}", OrderRow.Item(String.Format("Req_Hrs_From_{0}", Pic_Del)), OrderRow.Item(String.Format("Req_Hrs_To_{0}", Pic_Del)))) _
                                )
        StrHtml = String.Format("{0}Qty:<b>{1}</b>; Wt:<b>{2}</b>lbs; Vol:<b>{3}</b>CuFt; ", StrHtml, OrderRow.Item("Qty"), Math.Round(OrderRow.Item("Wt"), 0), Math.Round(OrderRow.Item("CuFts"), 0))
        If IsDBNull(MIN_Status) Then
            StrHtml = String.Format("{0}Srv Time:<a href=""javascript:changeServiceTime({1});""><b>{2}</b> Min </a>;", _
                                    StrHtml, _
                                    OrderRow.Item("MSO_ID"), _
                                    OrderRow.Item("Delivery_Time"))
        Else
            StrHtml = String.Format("{0}Srv Time:<b>{1}</b> Min;", _
                                    StrHtml, _
                                    OrderRow.Item("Delivery_Time"))
        End If
        StrHtml = String.Format("{0}Revenue:<b>{1} $</b> ", StrHtml, Math.Round(OrderRow.Item("Est_Revenue"), 2))
        StrHtml = String.Format("{0}<br/>{1}", StrHtml, OrderRow.Item("SpInstructions"))
        StrHtml = String.Format("{0}</span>", StrHtml)
        Return StrHtml
    End Function
    <WebMethod()> _
    Public Shared Function GetMan(Start_Date As Date, Hub As String, MIN_ID As Integer) As ManifestInfo
        Dim DT_List As DataTable
        Dim LiTText As String = ""
        Dim DateList As New List(Of Date)
        Dim disabled As String = "disabled"
        Dim DR As DataRow
        Dim Man As New ManifestInfo
        DR = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleRow(String.Format("select * from V_Manifest where MIN_ID={0}", MIN_ID))
        If MIN_ID = 0 Then
            Man.DRIVER_ID = 0
            Man.TRUCK_ID = 0
            Man.CO_DRIVER_ID1 = 0
            Man.CO_DRIVER_ID2 = 0
            Man.Running_Hours = 8
            Man.StartDate = "08:00"
            Man.TimeWindow = 240
            Man.Delivery_Time = 15
            Man.Color = GetColor()
            Man.Is_Multiday = False
            Man.ActualStartDate = CDate("1978-12-27")
            Man.Notes = ""
        Else
            Man.DRIVER_ID = DR.Item("DRIVER_ID")
            Man.TRUCK_ID = DR.Item("TRUCK_ID")
            If IsDBNull(DR.Item("CO_DRIVER_ID1")) Then
                Man.CO_DRIVER_ID1 = 0
            Else
                Man.CO_DRIVER_ID1 = DR.Item("CO_DRIVER_ID1")
            End If

            If IsDBNull(DR.Item("CO_DRIVER_ID2")) Then
                Man.CO_DRIVER_ID2 = 0
            Else
                Man.CO_DRIVER_ID2 = DR.Item("CO_DRIVER_ID2")
            End If

            Man.Running_Hours = DR.Item("Running_Hours")
            Man.StartDate = Format(DR.Item("StartDate"), "hh:mm")
            Man.TimeWindow = DR.Item("TimeWindow")
            Man.Delivery_Time = DR.Item("Delivery_Time")
            If IsDBNull(DR.Item("Color")) Then
                Man.Color = GetColor()
            Else
                Man.Color = DR.Item("Color")
            End If
            If IsDBNull(DR.Item("Is_Multiday")) Then
                Man.Is_Multiday = False
            Else
                Man.Is_Multiday = DR.Item("Is_Multiday")
            End If
            If IsDBNull(DR.Item("ActualStartDate")) Then
                Man.ActualStartDate = CDate("1978-12-27")
            Else
                Man.ActualStartDate = DR.Item("ActualStartDate")
            End If
            Man.Notes = DR.Item("Notes")
        End If

        DT_List = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect(String.Format("select [Driver Id], [Name],dbo.F_Is_Driver_Available([Driver Id],'{0}','{1}-{2}-{3}',{4}) as Is_Available from [MetroPolitanNavProduction].[dbo].[Metropolitan$Driver] where [Type]=10000 and [Default Location]='{0}'", _
                                                                                Hub, _
                                                                                CDate(Start_Date).Year, _
                                                                                CDate(Start_Date).Month, _
                                                                                CDate(Start_Date).Day, _
                                                                                MIN_ID _
                                                                            ))
        LiTText = ""
        For i = 0 To DT_List.Rows.Count - 1
            If DT_List.Rows(i).Item("Is_Available") Then
                disabled = ""
            Else
                disabled = "disabled"
            End If
            If Man.DRIVER_ID = DT_List.Rows(i).Item("Driver Id") Then
                LiTText = String.Format("{0}<option value=""{1}"" selected=""selected"" {3}>{2}</option>", LiTText, DT_List.Rows(i).Item("Driver Id"), DT_List.Rows(i).Item("Name"), disabled)
            Else
                LiTText = String.Format("{0}<option value=""{1}"" {3}>{2}</option>", LiTText, DT_List.Rows(i).Item("Driver Id"), DT_List.Rows(i).Item("Name"), disabled)
            End If
        Next
        Man.Drivers = LiTText



        DT_List = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect(String.Format("select [Entry No], [Code] from [MetroPolitanNavProduction].[dbo].[Metropolitan$Truck Location] where [Default Location]='{0}'", Hub))
        LiTText = ""
        For i = 0 To DT_List.Rows.Count - 1
            If Man.TRUCK_ID = DT_List.Rows(i).Item("Entry No") Then
                LiTText = String.Format("{0}<option value=""{1}"" selected=""selected"">{2}</option>", LiTText, DT_List.Rows(i).Item("Entry No"), DT_List.Rows(i).Item("Code"))
            Else
                LiTText = String.Format("{0}<option value=""{1}"">{2}</option>", LiTText, DT_List.Rows(i).Item("Entry No"), DT_List.Rows(i).Item("Code"))
            End If
        Next
        Man.Trucks = LiTText


        DT_List = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect(String.Format("select [Driver Id], [Name] from [MetroPolitanNavProduction].[dbo].[Metropolitan$Driver] where [Type]=20000 and [Default Location]='{0}'", Hub))
        LiTText = ""
        For i = 0 To DT_List.Rows.Count - 1

            If DT_List.Rows(i).Item("Driver Id") = Man.CO_DRIVER_ID1 Then
                LiTText = String.Format("{0}<option value=""{1}"" selected=""selected"">{2}</option>", LiTText, DT_List.Rows(i).Item("Driver Id"), DT_List.Rows(i).Item("Name"))
            Else
                LiTText = String.Format("{0}<option value=""{1}"">{2}</option>", LiTText, DT_List.Rows(i).Item("Driver Id"), DT_List.Rows(i).Item("Name"))
            End If
        Next
        Man.CODrivers1 = String.Format("<option value=""0"">Select Co Driver 1</option>{0}", LiTText)



        LiTText = ""
        For i = 0 To DT_List.Rows.Count - 1
            If DT_List.Rows(i).Item("Driver Id") = Man.CO_DRIVER_ID2 Then
                LiTText = String.Format("{0}<option value=""{1}"" selected=""selected"">{2}</option>", LiTText, DT_List.Rows(i).Item("Driver Id"), DT_List.Rows(i).Item("Name"))
            Else
                LiTText = String.Format("{0}<option value=""{1}"">{2}</option>", LiTText, DT_List.Rows(i).Item("Driver Id"), DT_List.Rows(i).Item("Name"))
            End If
        Next
        Man.CODrivers2 = String.Format("<option value=""0"">Select Co Driver 2</option>{0}", LiTText)
        For i = 0 To 8
            If Not (Start_Date.AddDays(i).DayOfWeek = DayOfWeek.Sunday) Then
                DateList.Add(Start_Date.AddDays(i))
            End If

        Next
        LiTText = ""
        For i = 1 To DateList.Count - 1

            If DateList(i) = CDate(Man.ActualStartDate) Then
                LiTText = String.Format("{0}<option value=""{1}/{2}/{3}"" selected=""selected"">Start on {4} {1}/{2}/{3}</option>", LiTText, DateList(i).Month, DateList(i).Day, DateList(i).Year, DateList(i).ToString("dddd"))
            Else
                LiTText = String.Format("{0}<option value=""{1}/{2}/{3}"">Start on {4} {1}/{2}/{3}</option>", LiTText, DateList(i).Month, DateList(i).Day, DateList(i).Year, DateList(i).ToString("dddd"))
            End If
        Next
        Man.MultidayList = String.Format("<option value=""0"">Start on {4} {1}/{2}/{3}</option>{0}", LiTText, DateList(0).Month, DateList(0).Day, DateList(0).Year, DateList(0).ToString("dddd"))


        Return Man
    End Function
    Private Shared Function GetColor() As String
        Dim ColorArr As New ArrayList
        ColorArr.Add("#B0171F")
        ColorArr.Add("#DC143C")
        ColorArr.Add("#FFB6C1")
        ColorArr.Add("#CD8C95")
        ColorArr.Add("#8B5F65")
        ColorArr.Add("#DB7093")
        ColorArr.Add("#FF82AB")
        ColorArr.Add("#EE799F")
        ColorArr.Add("#CD6889")
        ColorArr.Add("#8B475D")
        ColorArr.Add("#CDC1C5")
        ColorArr.Add("#8B8386")
        ColorArr.Add("#EE3A8C")
        ColorArr.Add("#CD3278")
        ColorArr.Add("#8B2252")
        ColorArr.Add("#8B3A62")
        ColorArr.Add("#FF1493")
        ColorArr.Add("#EE1289")
        ColorArr.Add("#CD1076")
        ColorArr.Add("#8B0A50")
        ColorArr.Add("#FF34B3")
        ColorArr.Add("#EE30A7")
        ColorArr.Add("#CD2990")
        ColorArr.Add("#8B1C62")
        ColorArr.Add("#C71585")
        ColorArr.Add("#D02090")
        ColorArr.Add("#DA70D6")
        ColorArr.Add("#FF83FA")
        ColorArr.Add("#EE7AE9")
        ColorArr.Add("#CD69C9")
        ColorArr.Add("#8B4789")
        ColorArr.Add("#D8BFD8")
        ColorArr.Add("#FFE1FF")
        ColorArr.Add("#EED2EE")
        ColorArr.Add("#CDB5CD")
        ColorArr.Add("#8B7B8B")
        ColorArr.Add("#FFBBFF")
        ColorArr.Add("#EEAEEE")
        ColorArr.Add("#CD96CD")
        ColorArr.Add("#8B668B")
        ColorArr.Add("#DDA0DD")
        ColorArr.Add("#EE82EE")
        ColorArr.Add("#FF00FF")
        ColorArr.Add("#EE00EE")
        ColorArr.Add("#CD00CD")
        ColorArr.Add("#8B008B")
        ColorArr.Add("#BA55D3")
        ColorArr.Add("#E066FF")
        ColorArr.Add("#D15FEE")
        ColorArr.Add("#B452CD")
        ColorArr.Add("#7A378B")
        ColorArr.Add("#9400D3")
        ColorArr.Add("#9932CC")
        ColorArr.Add("#BF3EFF")
        ColorArr.Add("#B23AEE")
        ColorArr.Add("#9A32CD")
        ColorArr.Add("#68228B")
        ColorArr.Add("#4B0082")
        ColorArr.Add("#8A2BE2")
        ColorArr.Add("#9B30FF")
        ColorArr.Add("#912CEE")
        ColorArr.Add("#7D26CD")
        ColorArr.Add("#551A8B")
        ColorArr.Add("#9370DB")
        ColorArr.Add("#AB82FF")
        ColorArr.Add("#9F79EE")
        ColorArr.Add("#8968CD")
        ColorArr.Add("#5D478B")
        ColorArr.Add("#483D8B")
        ColorArr.Add("#8470FF")
        ColorArr.Add("#7B68EE")
        ColorArr.Add("#6A5ACD")
        ColorArr.Add("#836FFF")
        ColorArr.Add("#7A67EE")
        ColorArr.Add("#6959CD")
        ColorArr.Add("#473C8B")
        ColorArr.Add("#0000FF")
        ColorArr.Add("#0000EE")
        ColorArr.Add("#0000CD")
        ColorArr.Add("#191970")
        ColorArr.Add("#3D59AB")
        ColorArr.Add("#4169E1")
        ColorArr.Add("#4876FF")
        ColorArr.Add("#436EEE")
        ColorArr.Add("#3A5FCD")
        ColorArr.Add("#27408B")
        ColorArr.Add("#6495ED")
        ColorArr.Add("#B0C4DE")
        ColorArr.Add("#CAE1FF")
        ColorArr.Add("#BCD2EE")
        ColorArr.Add("#A2B5CD")
        ColorArr.Add("#6E7B8B")
        ColorArr.Add("#778899")
        ColorArr.Add("#708090")
        ColorArr.Add("#C6E2FF")
        ColorArr.Add("#B9D3EE")
        ColorArr.Add("#9FB6CD")
        ColorArr.Add("#6C7B8B")
        ColorArr.Add("#1E90FF")
        ColorArr.Add("#1C86EE")
        ColorArr.Add("#1874CD")
        ColorArr.Add("#104E8B")
        ColorArr.Add("#4682B4")
        ColorArr.Add("#63B8FF")
        ColorArr.Add("#5CACEE")
        ColorArr.Add("#4F94CD")
        ColorArr.Add("#36648B")
        ColorArr.Add("#87CEFA")
        ColorArr.Add("#B0E2FF")
        ColorArr.Add("#A4D3EE")
        ColorArr.Add("#8DB6CD")
        ColorArr.Add("#607B8B")
        ColorArr.Add("#87CEFF")
        ColorArr.Add("#7EC0EE")
        ColorArr.Add("#6CA6CD")
        ColorArr.Add("#4A708B")
        ColorArr.Add("#87CEEB")
        ColorArr.Add("#00BFFF")
        ColorArr.Add("#00B2EE")
        ColorArr.Add("#009ACD")
        ColorArr.Add("#00688B")
        ColorArr.Add("#33A1C9")
        ColorArr.Add("#ADD8E6")
        ColorArr.Add("#BFEFFF")
        ColorArr.Add("#B2DFEE")
        ColorArr.Add("#9AC0CD")
        ColorArr.Add("#68838B")
        ColorArr.Add("#B0E0E6")
        ColorArr.Add("#98F5FF")
        ColorArr.Add("#8EE5EE")
        ColorArr.Add("#7AC5CD")
        ColorArr.Add("#53868B")
        ColorArr.Add("#00F5FF")
        ColorArr.Add("#00E5EE")
        ColorArr.Add("#00C5CD")
        ColorArr.Add("#00868B")
        ColorArr.Add("#5F9EA0")
        ColorArr.Add("#00CED1")
        ColorArr.Add("#F0FFFF")
        ColorArr.Add("#E0EEEE")
        ColorArr.Add("#C1CDCD")
        ColorArr.Add("#838B8B")
        ColorArr.Add("#E0FFFF")
        ColorArr.Add("#D1EEEE")
        ColorArr.Add("#B4CDCD")
        ColorArr.Add("#7A8B8B")
        ColorArr.Add("#BBFFFF")
        ColorArr.Add("#AEEEEE")
        ColorArr.Add("#96CDCD")
        ColorArr.Add("#668B8B")
        ColorArr.Add("#2F4F4F")
        ColorArr.Add("#97FFFF")
        ColorArr.Add("#8DEEEE")
        ColorArr.Add("#79CDCD")
        ColorArr.Add("#528B8B")
        ColorArr.Add("#00FFFF")
        ColorArr.Add("#00EEEE")
        ColorArr.Add("#00CDCD")
        ColorArr.Add("#008B8B")
        ColorArr.Add("#008080")
        ColorArr.Add("#48D1CC")
        ColorArr.Add("#20B2AA")
        ColorArr.Add("#03A89E")
        ColorArr.Add("#40E0D0")
        ColorArr.Add("#808A87")
        ColorArr.Add("#00C78C")
        ColorArr.Add("#7FFFD4")
        ColorArr.Add("#76EEC6")
        ColorArr.Add("#66CDAA")
        ColorArr.Add("#458B74")
        ColorArr.Add("#00FA9A")
        ColorArr.Add("#00FF7F")
        ColorArr.Add("#00EE76")
        ColorArr.Add("#00CD66")
        ColorArr.Add("#008B45")
        ColorArr.Add("#3CB371")
        ColorArr.Add("#54FF9F")
        ColorArr.Add("#4EEE94")
        ColorArr.Add("#43CD80")
        ColorArr.Add("#2E8B57")
        ColorArr.Add("#00C957")
        ColorArr.Add("#BDFCC9")
        ColorArr.Add("#3D9140")
        ColorArr.Add("#8FBC8F")
        ColorArr.Add("#C1FFC1")
        ColorArr.Add("#B4EEB4")
        ColorArr.Add("#9BCD9B")
        ColorArr.Add("#698B69")
        ColorArr.Add("#98FB98")
        ColorArr.Add("#9AFF9A")
        ColorArr.Add("#90EE90")
        ColorArr.Add("#7CCD7C")
        ColorArr.Add("#548B54")
        ColorArr.Add("#32CD32")
        ColorArr.Add("#228B22")
        ColorArr.Add("#00FF00")
        ColorArr.Add("#308014")
        ColorArr.Add("#7CFC00")
        ColorArr.Add("#7FFF00")
        ColorArr.Add("#76EE00")
        ColorArr.Add("#66CD00")
        ColorArr.Add("#458B00")
        ColorArr.Add("#ADFF2F")
        ColorArr.Add("#CAFF70")
        ColorArr.Add("#BCEE68")
        ColorArr.Add("#A2CD5A")
        ColorArr.Add("#6E8B3D")
        ColorArr.Add("#556B2F")
        ColorArr.Add("#6B8E23")
        ColorArr.Add("#C0FF3E")
        ColorArr.Add("#B3EE3A")
        ColorArr.Add("#9ACD32")
        ColorArr.Add("#698B22")
        ColorArr.Add("#CDCDC1")
        ColorArr.Add("#8B8B83")
        ColorArr.Add("#F5F5DC")
        ColorArr.Add("#FFFFE0")
        ColorArr.Add("#EEEED1")
        ColorArr.Add("#CDCDB4")
        ColorArr.Add("#8B8B7A")
        ColorArr.Add("#FAFAD2")
        ColorArr.Add("#FFFF00")
        ColorArr.Add("#EEEE00")
        ColorArr.Add("#CDCD00")
        ColorArr.Add("#808069")
        ColorArr.Add("#808000")
        ColorArr.Add("#BDB76B")
        ColorArr.Add("#FFF68F")
        ColorArr.Add("#EEE685")
        ColorArr.Add("#CDC673")
        ColorArr.Add("#8B864E")
        ColorArr.Add("#F0E68C")
        ColorArr.Add("#EEE8AA")
        ColorArr.Add("#FFFACD")
        ColorArr.Add("#EEE9BF")
        ColorArr.Add("#CDC9A5")
        ColorArr.Add("#8B8970")
        ColorArr.Add("#FFEC8B")
        ColorArr.Add("#EEDC82")
        ColorArr.Add("#CDBE70")
        ColorArr.Add("#8B814C")
        ColorArr.Add("#E3CF57")
        ColorArr.Add("#FFD700")
        ColorArr.Add("#EEC900")
        ColorArr.Add("#CDAD00")
        ColorArr.Add("#8B7500")
        ColorArr.Add("#FFF8DC")
        ColorArr.Add("#EEE8CD")
        ColorArr.Add("#CDC8B1")
        ColorArr.Add("#8B8878")
        ColorArr.Add("#DAA520")
        ColorArr.Add("#FFC125")
        ColorArr.Add("#EEB422")
        ColorArr.Add("#CD9B1D")
        ColorArr.Add("#8B6914")
        ColorArr.Add("#B8860B")
        ColorArr.Add("#FFB90F")
        ColorArr.Add("#EEAD0E")
        ColorArr.Add("#CD950C")
        ColorArr.Add("#8B6508")
        ColorArr.Add("#FFA500")
        ColorArr.Add("#EE9A00")
        ColorArr.Add("#CD8500")
        ColorArr.Add("#8B5A00")
        ColorArr.Add("#FFFAF0")
        ColorArr.Add("#FDF5E6")
        ColorArr.Add("#F5DEB3")
        ColorArr.Add("#FFE7BA")
        ColorArr.Add("#EED8AE")
        ColorArr.Add("#CDBA96")
        ColorArr.Add("#8B7E66")
        ColorArr.Add("#FFE4B5")
        ColorArr.Add("#FFEFD5")
        ColorArr.Add("#FFEBCD")
        ColorArr.Add("#FFDEAD")
        ColorArr.Add("#EECFA1")
        ColorArr.Add("#CDB38B")
        ColorArr.Add("#8B795E")
        ColorArr.Add("#FCE6C9")
        ColorArr.Add("#D2B48C")
        ColorArr.Add("#9C661F")
        ColorArr.Add("#FF9912")
        ColorArr.Add("#FAEBD7")
        ColorArr.Add("#FFEFDB")
        ColorArr.Add("#EEDFCC")
        ColorArr.Add("#CDC0B0")
        ColorArr.Add("#8B8378")
        ColorArr.Add("#DEB887")
        ColorArr.Add("#FFD39B")
        ColorArr.Add("#EEC591")
        ColorArr.Add("#CDAA7D")
        ColorArr.Add("#8B7355")
        ColorArr.Add("#FFE4C4")
        ColorArr.Add("#EED5B7")
        ColorArr.Add("#CDB79E")
        ColorArr.Add("#8B7D6B")
        ColorArr.Add("#E3A869")
        ColorArr.Add("#ED9121")
        ColorArr.Add("#FF8C00")
        ColorArr.Add("#FF7F00")
        ColorArr.Add("#EE7600")
        ColorArr.Add("#CD6600")
        ColorArr.Add("#8B4500")
        ColorArr.Add("#FF8000")
        ColorArr.Add("#FFA54F")
        ColorArr.Add("#EE9A49")
        ColorArr.Add("#CD853F")
        ColorArr.Add("#8B5A2B")
        ColorArr.Add("#CDAF95")
        ColorArr.Add("#8B7765")
        ColorArr.Add("#8B8682")
        ColorArr.Add("#C76114")
        ColorArr.Add("#D2691E")
        ColorArr.Add("#FF7F24")
        ColorArr.Add("#EE7621")
        ColorArr.Add("#CD661D")
        ColorArr.Add("#8B4513")
        ColorArr.Add("#292421")
        ColorArr.Add("#FF7D40")
        ColorArr.Add("#FF6103")
        ColorArr.Add("#8A360F")
        ColorArr.Add("#A0522D")
        ColorArr.Add("#FF8247")
        ColorArr.Add("#EE7942")
        ColorArr.Add("#CD6839")
        ColorArr.Add("#8B4726")
        ColorArr.Add("#FFA07A")
        ColorArr.Add("#EE9572")
        ColorArr.Add("#CD8162")
        ColorArr.Add("#8B5742")
        ColorArr.Add("#FF7F50")
        ColorArr.Add("#FF4500")
        ColorArr.Add("#EE4000")
        ColorArr.Add("#CD3700")
        ColorArr.Add("#8B2500")
        ColorArr.Add("#5E2612")
        ColorArr.Add("#E9967A")
        ColorArr.Add("#FF8C69")
        ColorArr.Add("#EE8262")
        ColorArr.Add("#CD7054")
        ColorArr.Add("#8B4C39")
        ColorArr.Add("#CD5B45")
        ColorArr.Add("#8B3E2F")
        ColorArr.Add("#CD4F39")
        ColorArr.Add("#FA8072")
        ColorArr.Add("#CDB7B5")
        ColorArr.Add("#CDC9C9")
        ColorArr.Add("#8B8989")
        ColorArr.Add("#CD9B9B")
        ColorArr.Add("#8B6969")
        ColorArr.Add("#CD5C5C")
        ColorArr.Add("#EE6363")
        ColorArr.Add("#8B3A3A")
        ColorArr.Add("#CD5555")
        ColorArr.Add("#A52A2A")
        ColorArr.Add("#FF4040")
        ColorArr.Add("#8B2323")
        ColorArr.Add("#B22222")
        ColorArr.Add("#FF3030")
        ColorArr.Add("#8B1A1A")
        ColorArr.Add("#CD0000")
        ColorArr.Add("#8E388E")
        ColorArr.Add("#7171C6")
        ColorArr.Add("#7D9EC0")
        ColorArr.Add("#388E8E")
        ColorArr.Add("#71C671")
        ColorArr.Add("#8E8E38")
        ColorArr.Add("#C5C1AA")
        ColorArr.Add("#C67171")
        ColorArr.Add("#555555")
        Dim rg As New Random
        Return ColorArr.Item(rg.Next(0, ColorArr.Count - 1))
    End Function

    <WebMethod()> _
    Public Shared Function AutoComplete(SearchText As String) As List(Of ReportData)
        Dim DT As DataTable
        Dim RepoList As New List(Of ReportData)
        Dim obj As New ReportData

        DT = PinnaclePlus.SQLData.Reports.P_Report_AutoComplete(SearchText)
        For i = 0 To DT.Rows.Count - 1
            obj = New ReportData
            obj.Rep_ID = DT.Rows(i).Item("Rep_ID")
            obj.Rep_Name = DT.Rows(i).Item("Name")
            RepoList.Add(obj)
        Next
        Return RepoList
    End Function

End Class