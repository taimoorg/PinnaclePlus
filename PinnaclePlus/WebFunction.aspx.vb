Imports System.Web.Services

Public Class GetPolyRet
    Public Property PolyList As List(Of String)
    Public Property Color As String
    Public Property LineWidth As Integer

End Class
Public Class glabel
    Public Property text As String
    Public Property color As String
    Sub New(vtext, vcolor)
        text = vtext
        color = vcolor
    End Sub

End Class
Public Class PinSVG
    Public Property path As String
    Public Property fillColor As String
    Public Property fillOpacity As Integer
    Public Property labelOrigin As Gpoint
    Public Property strokeColor As String
    Public Property strokeWeight As Integer
    Public Property scale As Integer
End Class
Public Class Gpoint
    Public Property x As Integer
    Public Property y As Integer
    Sub New(vx, vy)
        x = vx
        y = vy
    End Sub
End Class

Public Class WebFunction
    Inherits System.Web.UI.Page
    <WebMethod()> _
    Public Shared Function Add_Missing_Stop(AM_Order_ID As Integer, AM_MS_ID As Integer, AM_Is_Pickup As Boolean) As String
        PinnaclePlus.SQLData.Dispatch.P_Add_Missing_Stop(AM_MS_ID, AM_Order_ID, AM_Is_Pickup)
        Return "True"
    End Function
    <WebMethod()> _
    Public Shared Function StayALive() As String
        'HttpContext.Current.Session.
        Return "True"
    End Function
    <WebMethod()> _
    Public Shared Function updateSort(MIN_ID As Integer, SortOrder As String) As String
        Dim A() As String
        Dim Order() As String
        A = SortOrder.Split("&")
        For i = 0 To A.Length - 1
            Order = A(i).Split("=")
            PinnaclePlus.SQLData.GeneralOperations.ExecuteNONQuery(String.Format("update T_Manifest_Stop set Seq={0} where MS_ID={1}", i + 1, Order(1)), "", "")
        Next
        PinnaclePlus.Google.GoogleApi.RouteOptimizationDistance(MIN_ID, False)
        Return "True"
    End Function
    <WebMethod()> _
    Public Shared Function UpdateServiceTime(MSO_ID As Integer, ServiceTime As Object) As String
        If ServiceTime Is Nothing Then
            ServiceTime = DBNull.Value
        End If
        PinnaclePlus.SQLData.Dispatch.P_Manifest_Stop_Order_Update_Delivery_Time(MSO_ID, ServiceTime)
        Return "True"
    End Function
    <WebMethod()> _
    Public Shared Function GetStopTimeFrame(MS_ID As Integer) As StopTimeFrame
        Dim StopInfo As New StopTimeFrame
        Dim DR As DataRow

        DR = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleRow(String.Format("select MS_ID ,isnull(User_Value_Cascade,0) as User_Value_Cascade,dbo.F_Get_FromUTC_With_MS_ID(User_StartTime,MS_ID) as TW_Start,TimeWindow from T_Manifest_Stop MS where MS_ID={0}", MS_ID))
        With StopInfo
            .MS_ID = DR.Item("MS_ID")
            If IsDBNull(DR.Item("TW_Start")) Then
                .Start_Date = ""
                .Start_Time = ""
            Else
                .Start_Date = String.Format("{0}/{1}/{2}", CDate(DR.Item("TW_Start")).Month, CDate(DR.Item("TW_Start")).Day, CDate(DR.Item("TW_Start")).Year)
                .Start_Time = String.Format("{0}:{1}", Right("0" + CDate(DR.Item("TW_Start")).Hour.ToString, 2), Left(CDate(DR.Item("TW_Start")).Minute.ToString + "0", 2))
            End If

            .TimeWindow = DR.Item("TimeWindow")
            .User_Value_Cascade = DR.Item("User_Value_Cascade")
        End With

        Return StopInfo
    End Function
    <WebMethod()> _
    Public Shared Function GetStopDelivery_Time(MSO_ID As Integer) As Object
        Dim DR As DataRow
        DR = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleRow(String.Format("select Delivery_Time from T_Manifest_Stop_Order where MSO_ID={0}", MSO_ID))
        Return DR.Item(0)
    End Function
    <WebMethod()> _
    Public Shared Function UpdateTimeWindow(MS_ID As Integer, TimeFrame As Object, StartTime As Object, User_Value_Cascade As Object) As String
        If TimeFrame Is Nothing Then
            TimeFrame = DBNull.Value
        End If
        If User_Value_Cascade Is Nothing Then
            User_Value_Cascade = DBNull.Value
        End If
        If StartTime Is Nothing Then
            StartTime = DBNull.Value
        End If
        PinnaclePlus.SQLData.Dispatch.P_Manifest_Stop_TimeWindow_Update(MS_ID, TimeFrame, StartTime, User_Value_Cascade)
        Return "True"
    End Function
    <WebMethod()> _
    Public Shared Function Route_Optimization(MIN_ID As Integer, Route_Optimization_Type As Integer) As String
        Dim MIN_Count As Integer = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleValue(String.Format("select count(MS_ID) from T_Manifest_Stop where MIN_ID={0}", MIN_ID))
        If Route_Optimization_Type = 1 And MIN_Count > 23 Then
            Return "Manifest has more than 23 stops, try Shortest Distance."
        ElseIf Route_Optimization_Type = 1 Then
            PinnaclePlus.Google.GoogleApi.RouteOptimization(MIN_ID, True)
        Else
            PinnaclePlus.Google.GoogleApi.RouteOptimizationDistance(MIN_ID, True)
        End If
        Return ""
    End Function
    <WebMethod()> _
    Public Shared Function Ignore_Address_Check(M_ID As String) As String
        Dim Address As String
        Dim City As String
        Dim State As String
        Dim Zip As String
        Dim Lat, Lng As String
        Dim DT As DataTable
        Dim A() As String
        Dim B() As String
        Dim C() As String
        Dim Pic_Del As String = ""
        A = M_ID.Split("~")
        B = A(1).Split(",")
        C = B(0).Split("-")
        DT = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect(String.Format("select * from MetroPolitanNavProduction.[dbo].[V_Order_PPlus] where OrderNo='{0}'", C(0)))
        If C(1) = 1 Then
            Pic_Del = "Pic"

        Else
            Pic_Del = "Del"

        End If
        Address = DT.Rows(0).Item(String.Format("Address1_{0}", Pic_Del))
        City = DT.Rows(0).Item(String.Format("City_{0}", Pic_Del))
        State = DT.Rows(0).Item(String.Format("State_{0}", Pic_Del))
        Zip = DT.Rows(0).Item(String.Format("Zip_{0}", Pic_Del))
        C = DT.Rows(0).Item(String.Format("Latlong_{0}", Pic_Del)).ToString.Split(",")
        Lat = C(0)
        Lng = C(1)

        PinnaclePlus.SQLData.Dispatch.P_Address_IU(0, Address, City, State, Zip, Lat, Lng, "ignore")
        Return "1"
    End Function
    <WebMethod()> _
    Public Shared Function GetInfo(M_ID As String, MIN_ID As Integer, SEL_MIN_ID As Integer) As String
        Dim StrRet As String = ""
        Dim DT As DataTable
        Dim DR, DRManSel, DRMan As DataRow
        Dim OrdStr As String = "'0'"
        Dim Temp As String
        Dim A() As String
        Dim B() As String
        Dim C() As String
        Dim Pic_Del As String = ""
        Dim is_pickup As Boolean
        Dim StrHtml As String = ""
        Dim LinkTxt As String
        Dim MIN As String
        Dim OrdersNotPresentInMin As New List(Of OrderMissingFromStop)
        Dim ObjOrderMissingFromStop As OrderMissingFromStop
        Dim IsMissingOrder As Boolean
        DRManSel = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleRow(String.Format("Select cast(isnull(Locked,0) as bit) as Locked,Status from T_Manifest where MIN_ID={0}", SEL_MIN_ID))
        DRMan = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleRow(String.Format("Select cast(isnull(Locked,0) as bit) as Locked,Status from T_Manifest where MIN_ID={0}", MIN_ID))
        If MIN_ID = 0 Then
            If SEL_MIN_ID > 0 Then
                If Not DRManSel.Item("Locked") Then
                    If IsDBNull(DRManSel.Item("Status")) Then
                        MIN = String.Format("<a href=""javascript:Insert_Stop_dialog('{1}');"" title=""Select Manifest"">Insert Stop To Manifest No {0}</a>", SEL_MIN_ID, M_ID)
                    End If

                End If
            End If

        Else
            MIN = String.Format("Manifest ID: <a href=""javascript:sel_manifest({0});"" title=""Select Manifest"">{0}</a>", MIN_ID)
        End If

        A = M_ID.Split("~")
        If CStr(A(0)) = "" Then
            MIN = String.Format("<a href=""javascript:Ignore_Address_Check('{0}');"" title=""Ignore Address Check"">Ignore Address Check</a>", M_ID)
        End If
        If CStr(A(0)) = "0" Then
            DR = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleRow(String.Format("select * from T_Hub where HUB_CODE='{0}'", A(1)))
            StrRet = String.Format("{0}<br/> {1}, {2} {3} {4}<hr/>", DR.Item("Name"), DR.Item("Address1"), DR.Item("City"), DR.Item("State"), DR.Item("Zip"))
            Return StrRet
        Else


            B = A(1).Split(",")
            If A(0).Trim <> "" Then
                DR = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleRow(String.Format("select [Address],City,[State],Zip from T_Address where ADD_ID={0}", A(0)))
                StrRet = String.Format("{0}, {1}, {2} {3} {4}<hr/>", DR.Item("Address"), DR.Item("City"), DR.Item("State"), DR.Item("Zip"), MIN)
            End If
            For Each Temp In B
                C = Temp.Split("-")
                OrdStr = String.Format("{0},'{1}'", OrdStr, C(0))
                ObjOrderMissingFromStop = New OrderMissingFromStop
                ObjOrderMissingFromStop.Order_ID = C(0)
                OrdersNotPresentInMin.Add(ObjOrderMissingFromStop)
            Next
            DT = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect(String.Format("select Order_ID,MS_ID,(select Seq from T_Manifest_Stop where MS_ID=mso.MS_ID) as Seq from T_Manifest_Stop_Order mso where MS_ID in (select MS_ID from T_Manifest_Stop where MIN_ID={0}) and Order_ID in({1})", MIN_ID, OrdStr.Replace("'0',", "").Replace("'", "")))
            For i = 0 To DT.Rows.Count - 1
                For j = 0 To OrdersNotPresentInMin.Count - 1
                    OrdersNotPresentInMin(j).MS_ID = DT.Rows(i).Item("MS_ID")
                    OrdersNotPresentInMin(j).Seq = DT.Rows(i).Item("Seq")
                    If OrdersNotPresentInMin(j).Order_ID = DT.Rows(i).Item("Order_ID") Then
                        OrdersNotPresentInMin(j).Delete = True
                    End If
                Next
            Next
            DT = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect(String.Format("select * from MetroPolitanNavProduction.[dbo].[V_Order_PPlus] where OrderNo in ({0})", OrdStr))
            For i = 0 To DT.Rows.Count - 1
                For Each Temp In B
                    C = Temp.Split("-")
                    If C(0) = DT.Rows(i).Item("OrderNo") Then
                        If C(1) = 1 Then
                            Pic_Del = "Pic"
                            is_pickup = True
                        Else
                            Pic_Del = "Del"
                            is_pickup = False
                        End If
                        Exit For
                    End If
                Next
                If A(0).Trim = "" And i = 0 Then
                    StrRet = String.Format("{0}, {1}, {2} {3} {4}<hr/>", DT.Rows(i).Item(String.Format("Address1_{0}", Pic_Del)), DT.Rows(i).Item(String.Format("City_{0}", Pic_Del)), DT.Rows(i).Item(String.Format("State_{0}", Pic_Del)), DT.Rows(i).Item(String.Format("Zip_{0}", Pic_Del)), MIN)
                End If
                LinkTxt = "http://main.metropolitanwarehouse.com/Order/OrderDetail?eqs=" + PinnacleFunction.URLEncrypt.EncryptDesToHex(String.Format("OrderId={0}", DT.Rows(i).Item("OrderNo")), "MetroCryptoUSA07306#?+")
                StrHtml = String.Format("{0}<div  >", StrHtml)
                StrHtml = String.Format("{0}{1}<br/>", StrHtml, DT.Rows(i).Item(String.Format("Name_{0}", Pic_Del)))
                IsMissingOrder = False
                For j = 0 To OrdersNotPresentInMin.Count - 1
                    If (Not OrdersNotPresentInMin(j).Delete) And MIN_ID > 0 Then
                        If OrdersNotPresentInMin(j).Order_ID = DT.Rows(i).Item("OrderNo") Then
                            IsMissingOrder = True
                            ObjOrderMissingFromStop = OrdersNotPresentInMin(j)
                            Exit For
                        End If
                    End If
                Next
                If IsMissingOrder Then
                    If IsDBNull(DRMan.Item("Status")) Then
                        StrHtml = String.Format("{0}Order No:<a href=""{1}"" target=""_blank"">{2}</a> <img src='icon/alert.gif'/> {3} <a href=""javascript:Add_Missing_Stop({2},{4},{5});"" title=""Select Manifest"">Add to Manifest:<b>{6}</b> Stop:<b>{7}</b></a><br/>", _
                                                StrHtml, _
                                                LinkTxt, _
                                                DT.Rows(i).Item("OrderNo"), _
                                                IIf(is_pickup, "Pickup", "Delivery"), _
                                                ObjOrderMissingFromStop.MS_ID, _
                                                is_pickup.ToString.ToLower, _
                                                MIN_ID, _
                                                ObjOrderMissingFromStop.Seq)
                    End If
                Else
                    StrHtml = String.Format("{0}Order No:<a href=""{1}"" target=""_blank"">{2}</a> {3}<br/>", StrHtml, LinkTxt, DT.Rows(i).Item("OrderNo"), IIf(is_pickup, "Pickup", "Delivery"))
                End If


        StrHtml = String.Format("{0}Quantity:{1}; Weight: {2}; Cu Ft:{3}<br/>", StrHtml, DT.Rows(i).Item("Qty"), Math.Round(DT.Rows(i).Item("Wt"), 2), Math.Round(DT.Rows(i).Item("CuFts"), 0))
        StrHtml = String.Format("{0}{1}", StrHtml, DT.Rows(i).Item("SpInstructions"))
        If i = DT.Rows.Count - 1 Then
            StrHtml = String.Format("{0}</div>", StrHtml)
        Else
            StrHtml = String.Format("{0}</div><hr/>", StrHtml)
        End If
            Next
            Return String.Format("{0}{1}", StrRet, StrHtml)
        End If

    End Function
    <System.Web.Services.WebMethod()> _
    Public Shared Sub GetnuVizz()
        Dim NV As New PinnaclePlus.nuVizz.NuvizzAletrs
        NV.StartFTP()
    End Sub
    
    <System.Web.Services.WebMethod()> _
    Public Shared Function GetPoly(MIN_ID) As GetPolyRet
        Dim Ret As New GetPolyRet
        Dim DT As DataTable
        'Dim Temp As List(Of LatLng)
        Dim Final As New List(Of String)
        DT = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect(String.Format("select PolyLine,(select Color from T_Manifest where ms.MIN_ID=MIN_ID) as color from T_Manifest_Stop ms where MIN_ID={0} and PolyLine is not null   order by Seq", MIN_ID))
        For i = 0 To DT.Rows.Count - 1
            'Temp = New List(Of LatLng)
            'Temp = PinnaclePlus.Google.GoogleApi.decodePolyline(DT.Rows(i).Item("PolyLine"))
            Final.Add(DT.Rows(i).Item("PolyLine"))
        Next
        Ret.PolyList = Final
        If DT.Rows.Count > 0 Then
            If Not IsDBNull(DT.Rows(0).Item("color")) Then
                Ret.Color = DT.Rows(0).Item("color")
            Else
                Ret.Color = "#FF0000"
            End If
        Else
            Ret.Color = "#FF0000"
        End If
        Ret.LineWidth = 5
        Return Ret
    End Function
    <System.Web.Services.WebMethod()> _
    Public Shared Function GetHomes() As List(Of MarkerData)
        Dim DT As DataTable
        Dim MData As MarkerData
        Dim MarkerLst As New List(Of MarkerData)
        DT = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect(String.Format("Select * from T_Hub where HUB_CODE in (select HUB_CODE from T_Users_Hub where USER_ID='{0}')", PinnaclePlus.Security.CurrentUser.ID))
        For i = 0 To DT.Rows.Count - 1
            MData = New MarkerData
            With MData
                .lat = DT.Rows(i).Item("Lat")
                .lng = DT.Rows(i).Item("Lng")
                .M_ID = DT.Rows(i).Item("HUB_CODE")
                .pin_type = 0
                .title = String.Format("{0}({1})\n{2}, {3}", DT.Rows(i).Item("Name"), DT.Rows(i).Item("HUB_CODE"), DT.Rows(i).Item("Address1"), DT.Rows(i).Item("City"), DT.Rows(i).Item("State"))
                .icon = "icon/hub1.png"
                .Sel_Icon = ""
                .clickable = False
                .MIN_ID = -1
            End With
            MarkerLst.Add(MData)
        Next
        Return MarkerLst
    End Function
    <System.Web.Services.WebMethod()> _
    Public Shared Function GetMarkers(Date_ As String, DateTo As String, Hub As String) As List(Of MarkerData)
        Dim Pin As New PinSVG
        Dim StartDate As DateTime
        If Not IsDate(Date_) Then
            Return Nothing
        End If
        StartDate = Date_
        If Not IsDate(DateTo) Then
            DateTo = Date_
        End If
        Dim Diff As Integer
        Dim ManString As String
        Dim gAddress As PinnaclePlus.Google.GooglePlace
        Diff = DateDiff(DateInterval.Day, CDate(Date_), CDate(DateTo))

        Dim MData As MarkerData
        Dim MarkerLst As New List(Of MarkerData)
        Dim DT As DataTable

        For DateCounter = 0 To Diff


            DT = PinnaclePlus.SQLData.Dispatch.P_Stops_Get(Date_, Hub, StartDate)
            For i = 0 To DT.Rows.Count - 1
                If IsDBNull(DT.Rows(i).Item("ADD_ID")) Then
                    'If DT.Rows(i).Item("OrderNo") = 230661 Then
                    '    Dim XXX As String
                    '    XXX = ""
                    'End If
                    gAddress = PinnaclePlus.Google.GoogleApi.CheckAddress(DT.Rows(i).Item("Address_"), DT.Rows(i).Item("City"), DT.Rows(i).Item("State_"), DT.Rows(i).Item("Zip"))
                    If gAddress IsNot Nothing Then
                        DT.Rows(i).Item("ADD_ID") = gAddress.Add_ID
                        DT.Rows(i).Item("Lat") = gAddress.Lati
                        DT.Rows(i).Item("Lng") = gAddress.Longi
                    Else
                        If (Not IsDBNull(DT.Rows(i).Item("lat_in"))) And (Not IsDBNull(DT.Rows(i).Item("lng_in"))) Then
                            DT.Rows(i).Item("Lat") = DT.Rows(i).Item("lat_in")
                            DT.Rows(i).Item("Lng") = DT.Rows(i).Item("lng_in")
                        End If
                    End If
                End If
            Next
            For i = 0 To DT.Rows.Count - 1
                If (Not IsDBNull(DT.Rows(i).Item("lat"))) And (Not IsDBNull(DT.Rows(i).Item("lng"))) Then


                    '    Dim XXX As String
                    '    XXX = ""
                    'End If
                    ManString = ""
                    If Not Address_ZipFound(DT.Rows(i), MarkerLst) Then
                        'If DT.Rows(i).Item("OrderNo") = 195988 Or DT.Rows(i).Item("OrderNo") = 197493 Then
                        '    Dim XXXa As String
                        '    XXXa = ""
                        'End If
                        MData = New MarkerData

                        With MData

                            If IsDBNull(DT.Rows(i).Item("ADD_ID")) Then
                                .icon = "icon/invalid.png"
                                .Sel_Icon = "icon/invalid.png"
                                .clickable = False

                                .ADD_ID = String.Format("NULL{0}", Guid.NewGuid.ToString.ToLower.Replace("-", ""))
                            Else
                                .ADD_ID = DT.Rows(i).Item("ADD_ID")
                                If DT.Rows(i).Item("MIN_ID") > 0 Then
                                    Pin = New PinSVG
                                    Pin.path = "M 0,0 C -2,-20 -10,-22 -10,-30 A 10,10 0 1,1 10,-30 C 10,-22 2,-20 0,0 z"
                                    Pin.fillColor = DT.Rows(i).Item("Color")
                                    Pin.fillOpacity = 1
                                    Pin.labelOrigin = New Gpoint(0, -28)
                                    Pin.strokeColor = InvertColor(DT.Rows(i).Item("Color"))
                                    Pin.strokeWeight = 1
                                    Pin.scale = 1
                                    .icon = Pin
                                    .Sel_Icon = "icon/10-Sel.png"
                                    .clickable = False
                                    ManString = String.Format("Manifest ID: {0}", DT.Rows(i).Item("MIN_ID"))
                                Else
                                    .icon = "icon/00.png"
                                    .Sel_Icon = "icon/00-Sel.png"
                                    .clickable = True
                                End If

                            End If
                            If IsDBNull(DT.Rows(i).Item("Seq")) Then
                                .label = Format(CDate(Date_), "dd")
                            Else
                                .label = New glabel(DT.Rows(i).Item("Seq"), InvertColor(DT.Rows(i).Item("Color")))
                            End If

                            .lat = DT.Rows(i).Item("Lat")
                            .lng = DT.Rows(i).Item("Lng")

                            .M_ID = String.Format("{0}~{1}-{2}", DT.Rows(i).Item("ADD_ID"), DT.Rows(i).Item("OrderNo"), DT.Rows(i).Item("is_pickup"))
                            '.Address = DT.Rows(i).Item("Address_")
                            .Zip = DT.Rows(i).Item("Zip")
                            .State_ = DT.Rows(i).Item("State_")
                            .pin_type = 1


                            .title = String.Format("{0}, {1}, {2}\n{3}", DT.Rows(i).Item("Address_"), DT.Rows(i).Item("City"), DT.Rows(i).Item("Zip"), DT.Rows(i).Item("Name"))
                            If DT.Rows(i).Item("is_pickup") Then
                                .OrderCount_pic = 1
                                .ItemCount_pic = DT.Rows(i).Item("Qty")
                                .Wt_pic = DT.Rows(i).Item("Wt")
                                .Cubes_pic = DT.Rows(i).Item("CuFts")
                            Else
                                .OrderCount_del = 1
                                .ItemCount_del = DT.Rows(i).Item("Qty")
                                .Wt_del = DT.Rows(i).Item("Wt")
                                .Cubes_del = DT.Rows(i).Item("CuFts")
                            End If

                            .MIN_ID = DT.Rows(i).Item("MIN_ID")
                        End With
                        MarkerLst.Add(MData)
                    End If
                Else
                    Throw New System.Exception(String.Format("Lat Long Not Found for Order {0}", DT.Rows(i).Item("OrderNo")))
                End If
            Next
            Date_ = CDate(Date_).AddDays(1)

        Next
        'Dim StrTest As String = ""
        'For Each m In MarkerLst
        '    StrTest = StrTest & m.Info
        'Next

        Return MarkerLst

    End Function
    Private Shared Function Address_ZipFound(OrderRow As DataRow, ByRef Arr As List(Of MarkerData)) As Boolean
        Dim Pin As PinSVG
        Dim CorrectMIN_ID As Integer
        Dim ADD_ID As String
        If IsDBNull(OrderRow.Item("ADD_ID")) Then
            ADD_ID = ""
        Else
            ADD_ID = OrderRow.Item("ADD_ID")
        End If

        For Each m In Arr
            If m.pin_type <> 0 Then
                If m.ADD_ID = ADD_ID Then
                    If OrderRow.Item("MIN_ID") = m.MIN_ID Then
                        If Not IsDBNull(OrderRow.Item("ADD_ID")) Then
                            If OrderRow.Item("MIN_ID") > 0 Then
                                Pin = New PinSVG
                                Pin.path = "M 0,0 C -2,-20 -10,-22 -10,-30 A 10,10 0 1,1 10,-30 C 10,-22 2,-20 0,0 z"
                                Pin.fillColor = OrderRow.Item("Color")
                                Pin.fillOpacity = 1
                                Pin.labelOrigin = New Gpoint(0, -28)
                                Pin.strokeColor = InvertColor(OrderRow.Item("Color"))
                                Pin.strokeWeight = 2
                                Pin.scale = 1
                                m.icon = Pin
                                m.Sel_Icon = "icon/11.png"
                            Else
                                m.icon = "icon/01.png"
                                m.Sel_Icon = "icon/01-Sel.png"
                            End If
                        End If
                        'M_ID=Address_ID~OrderNo-Is_PickUp,OrderNo-Is_PickUp:Address_ID~OrderNo-Is_PickUp,OrderNo-Is_PickUp
                        m.M_ID = String.Format("{0},{1}-{2}", m.M_ID, OrderRow.Item("OrderNo"), OrderRow.Item("is_pickup"))
                        If OrderRow.Item("is_pickup") Then
                            m.OrderCount_pic = m.OrderCount_pic + 1
                            m.ItemCount_pic = m.ItemCount_pic + OrderRow.Item("Qty")
                            m.Wt_pic = m.Wt_pic + OrderRow.Item("Wt")
                            m.Cubes_pic = m.Cubes_pic + OrderRow.Item("CuFts")
                        Else
                            m.OrderCount_del = m.OrderCount_del + 1
                            m.ItemCount_del = m.ItemCount_del + OrderRow.Item("Qty")
                            m.Wt_del = m.Wt_del + OrderRow.Item("Wt")
                            m.Cubes_del = m.Cubes_del + OrderRow.Item("CuFts")
                        End If

                        Return True
                        Exit Function
                    Else
                        '10998~190246-0


                        m.icon = "icon/02.gif"
                        m.Sel_Icon = "icon/02.gif"
                        If m.MIN_ID > OrderRow.Item("MIN_ID") Then
                            CorrectMIN_ID = m.MIN_ID
                        Else
                            CorrectMIN_ID = OrderRow.Item("MIN_ID")
                        End If
                        If m.MIN_ID <> CorrectMIN_ID Then
                            m.MIN_ID = CorrectMIN_ID
                        End If
                        If (TypeOf m.label Is glabel) Then
                            If CType(m.label, glabel).text = "" Then
                                If Not IsDBNull(OrderRow.Item("Seq")) Then
                                    CType(m.label, glabel).text = OrderRow.Item("Seq")
                                End If

                            End If
                        Else
                            If m.label = "" Then
                                If Not IsDBNull(OrderRow.Item("Seq")) Then
                                    m.label = OrderRow.Item("Seq")
                                End If

                            End If

                        End If

                        'M_ID=Address_ID~OrderNo-Is_PickUp,OrderNo-Is_PickUp:Address_ID~OrderNo-Is_PickUp,OrderNo-Is_PickUp
                        m.M_ID = String.Format("{0},{1}-{2}", m.M_ID, OrderRow.Item("OrderNo"), OrderRow.Item("is_pickup"))
                        If OrderRow.Item("is_pickup") Then
                            m.OrderCount_pic = m.OrderCount_pic + 1
                            m.ItemCount_pic = m.ItemCount_pic + OrderRow.Item("Qty")
                            m.Wt_pic = m.Wt_pic + OrderRow.Item("Wt")
                            m.Cubes_pic = m.Cubes_pic + OrderRow.Item("CuFts")
                        Else
                            m.OrderCount_del = m.OrderCount_del + 1
                            m.ItemCount_del = m.ItemCount_del + OrderRow.Item("Qty")
                            m.Wt_del = m.Wt_del + OrderRow.Item("Wt")
                            m.Cubes_del = m.Cubes_del + OrderRow.Item("CuFts")
                        End If
                        Return True
                        Exit Function
                    End If
                End If
            End If
        Next
        Return False
    End Function



End Class