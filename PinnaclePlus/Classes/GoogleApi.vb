Imports System.Net
Imports System.IO
Imports System.Security.Cryptography
Namespace PinnaclePlus.Google
    Public Class GetDistanceInfo
        Public Property Distance As Integer
        Public Property Duration As Integer
        Public Property Poly As String
    End Class
    Public Class GoogleApi
        Sub New()
        End Sub
        Private Shared Function SignURL(ByVal url As String) As String
            Dim keyString As String = "7NZLKUTjxb0erhZtdU3Aomn2GAw="
            Dim encoding As ASCIIEncoding = New ASCIIEncoding()

            'URL-safe decoding
            Dim privateKeyBytes As Byte() = Convert.FromBase64String(keyString.Replace("-", "+").Replace("_", "/"))

            Dim objURI As Uri = New Uri(url)
            Dim encodedPathAndQueryBytes As Byte() = encoding.GetBytes(objURI.LocalPath & objURI.Query)

            'compute the hash
            Dim algorithm As HMACSHA1 = New HMACSHA1(privateKeyBytes)
            Dim hash As Byte() = algorithm.ComputeHash(encodedPathAndQueryBytes)

            'convert the bytes to string and make url-safe by replacing '+' and '/' characters
            Dim signature As String = Convert.ToBase64String(hash).Replace("+", "-").Replace("/", "_")

            'Add the signature to the existing URI.
            Return objURI.Scheme & "://" & objURI.Host & objURI.LocalPath & objURI.Query & "&signature=" & signature
        End Function

        Public Shared Function CheckAddress(ByVal Address1 As String, City As String, State As String, Zip As String) As GooglePlace
            Dim DR As DataRow
            Dim LL As GooglePlace
            Dim ZipID, State_ID As Integer
            Dim PlaceID As String
            Dim ZipRows() As DataRow
            Dim ZipGoogle, StateGoolge As String
            DR = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleRow(String.Format("select ADD_ID,Lat, Lng from T_Address where LOWER([Address])='{0}' and Zip='{1}'", Address1.ToLower.Trim, Zip))
            If DR Is Nothing Then

                Dim url As String = SignURL(String.Format("https://maps.google.com/maps/api/geocode/xml?address={0}&channel=pp_Address&client=gme-metropolitanwarehouse", String.Format("{0}, {1}, {2} {3}", Address1.Replace("#", ""), City, State, Zip)))
                Dim request As WebRequest = WebRequest.Create(url)
                Using response As WebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
                    Using reader As New StreamReader(response.GetResponseStream(), Encoding.UTF8)
                        Dim dsResult As New DataSet()

                        dsResult.ReadXml(reader)
                        If dsResult.Tables("GeocodeResponse").Rows(0).Item("Status") = "OK" Then
                            Dim location As DataRow = dsResult.Tables("location").Select("geometry_id=0")(0)
                            ZipRows = dsResult.Tables("type").Select("type_Text='postal_code' and address_component_id is not null")
                            If ZipRows.Count > 0 Then
                                ZipID = ZipRows(0).Item("address_component_id")
                                State_ID = dsResult.Tables("type").Select("type_Text='administrative_area_level_1'")(0).Item("address_component_id")

                                ZipGoogle = dsResult.Tables("address_component").Select("address_component_id=" & ZipID)(0).Item("short_name")
                                StateGoolge = dsResult.Tables("address_component").Select("address_component_id=" & State_ID)(0).Item("short_name")
                                PlaceID = dsResult.Tables("result").Rows(0).Item("place_id")
                                If Zip = ZipGoogle And State = StateGoolge Then
                                    LL = New GooglePlace
                                    LL.Lati = location.Item("lat")
                                    LL.Longi = location.Item("lng")
                                    LL.Add_ID = PinnaclePlus.SQLData.Dispatch.P_Address_IU(0, Address1.Trim, City.Trim, State.Trim, Zip.Trim, LL.Lati, LL.Longi, PlaceID)
                                    Return LL
                                Else
                                    Return Nothing
                                End If
                            Else
                                Return Nothing
                            End If
                        Else
                            Return Nothing
                        End If
                    End Using
                End Using
            Else
                LL = New GooglePlace
                LL.Lati = DR.Item("Lat")
                LL.Longi = DR.Item("Lng")
                LL.Add_ID = DR.Item("ADD_ID")
                Return LL
            End If
        End Function
        Public Shared Function RouteOptimization(MIN_ID As Integer, Optimize As Boolean) As String
            'googleID=gme-metropolitanwarehouse
            Dim DT As DataTable
            Dim Legs, waypoints_index As DataTable
            Dim OrigninLatLong As String
            Dim WayPoints As String = ""
            Dim GogURL As String
            Dim MS_ID As Integer
            Dim Distance As Integer
            Dim duration As Integer
            OrigninLatLong = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleValue(String.Format("select lat +','+ lng from T_Hub where HUB_CODE in (select Hub from T_Manifest where MIN_ID={0})", MIN_ID))
            'waypoints=Charlestown,MA|Lexington,MA
            DT = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect(String.Format("select * from V_Manifest_Stop where MIN_ID={0}  order by Seq", MIN_ID))
            If Optimize Then
                WayPoints = String.Format("{0}{1}", WayPoints, "optimize:true|")
            End If
            For i = 0 To DT.Rows.Count - 1
                If i = 0 Then
                    WayPoints = String.Format("{0}{1},{2}", WayPoints, DT.Rows(i).Item("lat"), DT.Rows(i).Item("lng"))
                Else
                    WayPoints = String.Format("{0}|{1},{2}", WayPoints, DT.Rows(i).Item("lat"), DT.Rows(i).Item("lng"))
                End If
                'If i = 0 Then
                '    WayPoints = String.Format("{0}{1},{2} {3}", WayPoints, DT.Rows(i).Item("Address"), DT.Rows(i).Item("State"), DT.Rows(i).Item("Zip"))
                'Else
                '    WayPoints = String.Format("{0}|{1},{2} {3}", WayPoints, DT.Rows(i).Item("Address"), DT.Rows(i).Item("State"), DT.Rows(i).Item("Zip"))
                'End If
            Next


            GogURL = SignURL(String.Format("https://maps.googleapis.com/maps/api/directions/xml?origin={0}&destination={0}&waypoints={1}&channel=pp_Route&client=gme-metropolitanwarehouse", OrigninLatLong, WayPoints))

            '&key=AIzaSyDM7-igdY9_vzj7-1ASLXQmLHPxNDB3OHE

            Dim request As WebRequest = WebRequest.Create(GogURL)
            Using response As WebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
                Using reader As New StreamReader(response.GetResponseStream(), Encoding.UTF8)
                    Dim dsResult As New DataSet()
                    dsResult.ReadXml(reader)
                    If dsResult.Tables("DirectionsResponse").Rows(0).Item("Status") = "OK" Then
                        Legs = dsResult.Tables("leg")
                        waypoints_index = dsResult.Tables("waypoint_index")
                        For i = 0 To waypoints_index.Rows.Count - 1
                            MS_ID = DT.Rows(waypoints_index.Rows(i).Item("waypoint_index_Text")).Item("MS_ID")
                            Distance = dsResult.Tables("distance").Select(String.Format("leg_id={0}", Legs.Rows(i).Item("leg_id")))(0).Item("value")
                            duration = dsResult.Tables("duration").Select(String.Format("leg_id={0}", Legs.Rows(i).Item("leg_id")))(0).Item("value")
                            PinnaclePlus.SQLData.GeneralOperations.ExecuteNONQuery(String.Format("update T_Manifest_Stop set Seq={0}, Distance={1}, Duration={2} where MS_ID={3}", i + 1, Distance, duration, MS_ID), "", "")
                        Next
                        Distance = dsResult.Tables("distance").Select(String.Format("leg_id={0}", waypoints_index.Rows.Count))(0).Item("value")
                        duration = dsResult.Tables("duration").Select(String.Format("leg_id={0}", waypoints_index.Rows.Count))(0).Item("value")
                        PinnaclePlus.SQLData.Dispatch.P_Manifest_Update_DirectionDate(MIN_ID, Distance, duration, dsResult.Tables("overview_polyline").Rows(0).Item("points"))
                        PinnaclePlus.SQLData.Dispatch.P_UpdateTimeWindow(MIN_ID)
                        Return ""
                    Else
                        PinnaclePlus.SQLData.Dispatch.P_UpdateTimeWindow(MIN_ID)
                        Return dsResult.Tables("DirectionsResponse").Rows(0).Item("error_message")
                    End If
                End Using
            End Using

        End Function
        Public Shared Function GetDistance(StartLatlong As LatLng, EndLatLon As LatLng) As Object
            Dim Ret As New GetDistanceInfo
            Dim GogURL As String
            Dim request As WebRequest
            Dim curLatLong, StartingLatLong As String
            StartingLatLong = String.Format("{0},{1}", StartLatlong.lat, StartLatlong.lng)
            curLatLong = String.Format("{0},{1}", EndLatLon.lat, EndLatLon.lng)
            GogURL = SignURL(String.Format("https://maps.googleapis.com/maps/api/directions/xml?origin={0}&destination={1}&channel=pp_Route_Distance&client=gme-metropolitanwarehouse", StartingLatLong, curLatLong))
            request = System.Net.WebRequest.Create(GogURL)
            Using response As WebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
                Using reader As New StreamReader(response.GetResponseStream(), Encoding.UTF8)
                    Dim dsResult As New DataSet()
                    dsResult.ReadXml(reader)
                    If dsResult.Tables("DirectionsResponse").Rows(0).Item("Status") = "OK" Then
                        Ret.Distance = dsResult.Tables("distance").Select("leg_id=0")(0).Item("value")
                        Ret.Duration = dsResult.Tables("duration").Select("leg_id=0")(0).Item("value")
                        Ret.Poly = dsResult.Tables("overview_polyline").Rows(0).Item("points")
                    Else
                        Return dsResult.Tables("DirectionsResponse").Rows(0).Item("error_message")
                    End If
                End Using
            End Using
            Return Ret
        End Function

        Public Shared Function RouteOptimizationDistance(MIN_ID As Integer, Optimize As Boolean) As String
            'googleID=gme-metropolitanwarehouse
            Dim PendingList As New List(Of My_Stop)
            Dim FinalList As New List(Of My_Stop)
            Dim DT As DataTable
            Dim OrigninLatLong As String
            Dim WayPoints As String = ""
            Dim GogURL As String
            Dim StartingLatLong, curLatLong As String
            Dim request As WebRequest
            Dim SmallStop As My_Stop
            OrigninLatLong = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleValue(String.Format("select lat +','+ lng from T_Hub where HUB_CODE in (select Hub from T_Manifest where MIN_ID={0})", MIN_ID))
            'waypoints=Charlestown,MA|Lexington,MA
            DT = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect(String.Format("select * from V_Manifest_Stop where MIN_ID={0}  order by Seq", MIN_ID))
            If DT.Rows(0).Item("Locked") Then
                Exit Function
            End If
            StartingLatLong = OrigninLatLong
            If Optimize Then
                For i = 0 To DT.Rows.Count - 1
                    Dim ObjStop As New My_Stop
                    With ObjStop
                        .MS_ID = DT.Rows(i).Item("MS_ID")
                        .Lati = DT.Rows(i).Item("lat")
                        .Longi = DT.Rows(i).Item("lng")
                        .address = DT.Rows(i).Item("Address")
                    End With
                    PendingList.Add(ObjStop)
                Next
                While PendingList.Count > 0
                    For i = 0 To PendingList.Count - 1
                        curLatLong = String.Format("{0},{1}", PendingList.Item(i).Lati, PendingList.Item(i).Longi)
                        GogURL = SignURL(String.Format("https://maps.googleapis.com/maps/api/directions/xml?origin={0}&destination={1}&channel=pp_Route_Distance&client=gme-metropolitanwarehouse", StartingLatLong, curLatLong))
                        request = System.Net.WebRequest.Create(GogURL)
                        Using response As WebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
                            Using reader As New StreamReader(response.GetResponseStream(), Encoding.UTF8)
                                Dim dsResult As New DataSet()
                                dsResult.ReadXml(reader)
                                If dsResult.Tables("DirectionsResponse").Rows(0).Item("Status") = "OK" Then
                                    PendingList.Item(i).Distance = dsResult.Tables("distance").Select("leg_id=0")(0).Item("value")
                                    PendingList.Item(i).Duration = dsResult.Tables("duration").Select("leg_id=0")(0).Item("value")
                                    PendingList.Item(i).Poly = dsResult.Tables("overview_polyline").Rows(0).Item("points")
                                Else
                                    Return dsResult.Tables("DirectionsResponse").Rows(0).Item("error_message")
                                End If
                            End Using
                        End Using
                    Next
                    SmallStop = GetSmallestStop(PendingList)
                    FinalList.Add(SmallStop)
                    StartingLatLong = String.Format("{0},{1}", SmallStop.Lati, SmallStop.Longi)
                End While
                For i = 0 To FinalList.Count - 1
                    PinnaclePlus.SQLData.Dispatch.P_Manifest_Stop_DirectionDate(FinalList(i).MS_ID, i + 1, FinalList(i).Distance, FinalList(i).Duration, FinalList(i).Poly)
                Next
            Else
                For i = 0 To DT.Rows.Count - 1
                    curLatLong = String.Format("{0},{1}", DT.Rows(i).Item("lat"), DT.Rows(i).Item("lng"))
                    GogURL = SignURL(String.Format("https://maps.googleapis.com/maps/api/directions/xml?origin={0}&destination={1}&channel=pp_Route_Distance&client=gme-metropolitanwarehouse", StartingLatLong, curLatLong))
                    request = System.Net.WebRequest.Create(GogURL)
                    Using response As WebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
                        Using reader As New StreamReader(response.GetResponseStream(), Encoding.UTF8)
                            Dim dsResult As New DataSet()
                            dsResult.ReadXml(reader)
                            If dsResult.Tables("DirectionsResponse").Rows(0).Item("Status") = "OK" Then
                                PinnaclePlus.SQLData.Dispatch.P_Manifest_Stop_DirectionDate(DT.Rows(i).Item("MS_ID"), i + 1, dsResult.Tables("distance").Select("leg_id=0")(0).Item("value"), dsResult.Tables("duration").Select("leg_id=0")(0).Item("value"), dsResult.Tables("overview_polyline").Rows(0).Item("points"))
                            Else
                                Return dsResult.Tables("DirectionsResponse").Rows(0).Item("error_message")
                            End If
                        End Using
                    End Using
                    StartingLatLong = curLatLong
                Next
            End If

            curLatLong = OrigninLatLong
            GogURL = SignURL(String.Format("https://maps.googleapis.com/maps/api/directions/xml?origin={0}&destination={1}&channel=pp_Route_Distance&client=gme-metropolitanwarehouse", StartingLatLong, curLatLong))
            request = System.Net.WebRequest.Create(GogURL)
            Using response As WebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
                Using reader As New StreamReader(response.GetResponseStream(), Encoding.UTF8)
                    Dim dsResult As New DataSet()
                    dsResult.ReadXml(reader)
                    If dsResult.Tables("DirectionsResponse").Rows(0).Item("Status") = "OK" Then
                        PinnaclePlus.SQLData.Dispatch.P_Manifest_Update_DirectionDate(MIN_ID, dsResult.Tables("distance").Select("leg_id=0")(0).Item("value"), dsResult.Tables("duration").Select("leg_id=0")(0).Item("value"), dsResult.Tables("overview_polyline").Rows(0).Item("points"))
                    Else
                        Return dsResult.Tables("DirectionsResponse").Rows(0).Item("error_message")
                    End If
                End Using
            End Using
            PinnaclePlus.SQLData.Dispatch.P_UpdateTimeWindow(MIN_ID)

            Return ""
        End Function
        Public Shared Function decodePolyline(ByVal polyline As String) As List(Of LatLng)
            If polyline Is Nothing OrElse polyline = "" Then Return Nothing

            Dim polylinechars As Char() = polyline.ToCharArray()
            Dim points As New List(Of LatLng)
            Dim currentLat As Integer = 0
            Dim ObjLatLng As LatLng
            Dim currentLng As Integer = 0
            Dim next5bits As Integer
            Dim sum As Integer
            Dim shifter As Integer
            Dim index As Integer = 0

            While index < polylinechars.Length
                ' calculate next latitude
                sum = 0
                shifter = 0
                Do
                    index += 1
                    next5bits = AscW(polylinechars(index - 1)) - 63
                    sum = sum Or (next5bits And 31) << shifter
                    shifter += 5
                Loop While next5bits >= 32 AndAlso index < polylinechars.Length

                If index >= polylinechars.Length Then
                    Exit While
                End If

                currentLat += If((sum And 1) = 1, Not (sum >> 1), (sum >> 1))

                'calculate next longitude
                sum = 0
                shifter = 0
                Do
                    index += 1
                    next5bits = AscW(polylinechars(index - 1)) - 63
                    sum = sum Or (next5bits And 31) << shifter
                    shifter += 5
                Loop While next5bits >= 32 AndAlso index < polylinechars.Length

                If index >= polylinechars.Length AndAlso next5bits >= 32 Then
                    Exit While
                End If

                currentLng += If((sum And 1) = 1, Not (sum >> 1), (sum >> 1))
                ObjLatLng = New LatLng
                ObjLatLng.lat = Convert.ToDouble(currentLat) / 100000.0
                ObjLatLng.lng = Convert.ToDouble(currentLng) / 100000.0
                points.Add(ObjLatLng)
            End While

            Return points
        End Function
        Private Shared Function GetSmallestStop(Arr As List(Of My_Stop)) As My_Stop
            Dim Ret As New My_Stop
            Ret.Distance = 1000000000
            Dim objStop As My_Stop
            For Each objStop In Arr
                If objStop.Distance < Ret.Distance Then
                    Ret = objStop
                End If
            Next
            Arr.Remove(Ret)
            Return Ret
        End Function
    End Class
    Public Class My_Stop
        Public Property MS_ID As Integer
        Public Property Lati As String
        Public Property Longi As String
        Public Property Distance As Integer
        Public Property Duration As Integer
        Public Property Poly As String
        Public Property address As String

    End Class

    Public Class GooglePlace
        Public Property Lati As String
        Public Property Longi As String
        Public Property Add_ID As Integer
    End Class

End Namespace
