Imports Newtonsoft.Json
Imports System.Net
Imports System.IO
Namespace PinnaclePlus.onfleet
    Public Class OnfleetAPI
        Sub New()
        End Sub
        Public Shared Function CreateDestination(UnparsedAddress As String) As String
            Dim ObjDest As Destination
            Dim Ret As String = ""
            Dim strURL As String
            Dim myWebReq As HttpWebRequest
            Dim myWebResp As HttpWebResponse
            Dim encoding As New System.Text.UTF8Encoding
            Dim objStream As Stream
            Dim sr As StreamReader
            'getData__1 = getData__1 & strJSON
            Dim JsonStr As String = String.Format("{{""address"":{{""unparsed"":""{0}""}}}}", UnparsedAddress)

            strURL = "https://onfleet.com/api/v2/destinations"

            'myWebReq.GetResponse.ToString()

            myWebReq = DirectCast(WebRequest.Create(strURL), HttpWebRequest)
            myWebReq.ContentType = "application/json; charset=utf-8"

            'myWebReq.ContentLength = data.Le
            myWebReq.Method = "POST"
            myWebReq.KeepAlive = True
            Dim autorization As String = "" 'My.Settings.nv_xml_path
            Dim binaryAuthorization As Byte() = System.Text.Encoding.UTF8.GetBytes(autorization)
            autorization = Convert.ToBase64String(binaryAuthorization)
            autorization = "Basic " + autorization
            myWebReq.Headers.Add("AUTHORIZATION", autorization)


            If myWebReq.Proxy IsNot Nothing Then
                myWebReq.Proxy.Credentials = CredentialCache.DefaultCredentials
            End If
            Using myStream As Stream = myWebReq.GetRequestStream()

                'myStream = myWebReq.GetRequestStream()
                Dim data As Byte() = encoding.GetBytes(JsonStr)
                If data.Length > 0 Then
                    myStream.Write(data, 0, data.Length)
                    myStream.Close()
                End If
                Try
                    myWebResp = DirectCast(myWebReq.GetResponse(), HttpWebResponse)
                    objStream = myWebResp.GetResponseStream()
                    sr = New StreamReader(objStream)
                    JsonStr = sr.ReadToEnd()
                    If myWebResp.StatusCode = HttpStatusCode.OK Then
                        ObjDest = JsonConvert.DeserializeObject(Of PinnaclePlus.onfleet.Destination)(JsonStr)
                        Ret = ObjDest.id
                    End If

                Catch wex As WebException
                    If wex.Response IsNot Nothing Then
                        myWebResp = DirectCast(wex.Response, HttpWebResponse)
                        objStream = myWebResp.GetResponseStream()
                        sr = New StreamReader(objStream)
                        JsonStr = sr.ReadToEnd()
                        Dim Objerr As PinnaclePlus.onfleet.ErrorResponse = JsonConvert.DeserializeObject(Of PinnaclePlus.onfleet.ErrorResponse)(JsonStr)
                        Ret = String.Format("ERROR:{0}", Objerr.message.message)
                    End If
                Catch ex As Exception
                    Ret = "ERROR:Error creating destination,"
                Finally

                    myWebResp.Close()
                    myWebReq = Nothing
                End Try
            End Using
            Return Ret
        End Function
        Public Shared Function GetWorker(WorkerID As String) As Worker
            Dim Worker As Worker
            Dim strURL As String
            Dim myWebReq As HttpWebRequest
            Dim myWebResp As HttpWebResponse
            Dim objStream As Stream
            Dim encoding As New System.Text.UTF8Encoding
            Dim Jstr As String
            Dim sr As StreamReader
            'getData__1 = getData__1 & strJSON

            strURL = String.Format("https://onfleet.com/api/v2/workers/{0}", WorkerID)

            'myWebReq.GetResponse.ToString()
            myWebReq = DirectCast(WebRequest.Create(strURL), HttpWebRequest)
            myWebReq.ContentType = "application/json; charset=utf-8"

            'myWebReq.ContentLength = data.Le
            myWebReq.Method = "GET"
            myWebReq.KeepAlive = True
            Dim autorization As String = "" ' My.Settings.nv_xml_path
            Dim binaryAuthorization As Byte() = System.Text.Encoding.UTF8.GetBytes(autorization)
            autorization = Convert.ToBase64String(binaryAuthorization)
            autorization = "Basic " + autorization
            myWebReq.Headers.Add("AUTHORIZATION", autorization)


            If myWebReq.Proxy IsNot Nothing Then
                myWebReq.Proxy.Credentials = CredentialCache.DefaultCredentials
            End If


            'myStream = myWebReq.GetRequestStream()
            myWebResp = DirectCast(myWebReq.GetResponse(), HttpWebResponse)
            objStream = myWebResp.GetResponseStream()
            Dim reader As New StreamReader(objStream)
            Jstr = reader.ReadToEnd()


            myWebResp.Close()
            'System.Threading.Thread.Sleep(1000)

            myWebReq = Nothing

            Worker = JsonConvert.DeserializeObject(Of PinnaclePlus.onfleet.Worker)(Jstr)
            Return Worker
        End Function
        Public Shared Function GetTask(TaskID As String) As OnfleetTask
            Dim strURL As String
            Dim myWebReq As HttpWebRequest
            Dim myWebResp As HttpWebResponse
            Dim objStream As Stream
            Dim encoding As New System.Text.UTF8Encoding
            Dim Jstr As String
            Dim sr As StreamReader
            'getData__1 = getData__1 & strJSON


            strURL = String.Format("https://onfleet.com/api/v2/tasks/{0}", TaskID)
            'myWebReq.GetResponse.ToString()
            myWebReq = DirectCast(WebRequest.Create(strURL), HttpWebRequest)
            myWebReq.ContentType = "application/json; charset=utf-8"

            'myWebReq.ContentLength = data.Le
            myWebReq.Method = "GET"
            myWebReq.KeepAlive = True
            Dim autorization As String = "" 'My.Settings.nv_xml_path
            Dim binaryAuthorization As Byte() = System.Text.Encoding.UTF8.GetBytes(autorization)
            autorization = Convert.ToBase64String(binaryAuthorization)
            autorization = "Basic " + autorization
            myWebReq.Headers.Add("AUTHORIZATION", autorization)


            If myWebReq.Proxy IsNot Nothing Then
                myWebReq.Proxy.Credentials = CredentialCache.DefaultCredentials
            End If

            Try
                'myStream = myWebReq.GetRequestStream()
                myWebResp = DirectCast(myWebReq.GetResponse(), HttpWebResponse)
                objStream = myWebResp.GetResponseStream()
                Dim reader As New StreamReader(objStream)
                Jstr = reader.ReadToEnd()


                myWebResp.Close()
                'System.Threading.Thread.Sleep(1000)

                myWebReq = Nothing
                Dim obj As PinnaclePlus.onfleet.OnfleetTask
                obj = JsonConvert.DeserializeObject(Of PinnaclePlus.onfleet.OnfleetTask)(Jstr)
                Return obj


            Catch wex As WebException
                If wex.Response IsNot Nothing Then

                    myWebResp = DirectCast(wex.Response, HttpWebResponse)
                    objStream = myWebResp.GetResponseStream()
                    sr = New StreamReader(objStream)
                    Jstr = sr.ReadToEnd()
                    Return Nothing

                    'Dim Objerr As PinnaclePlus.onfleet.ErrorResponse = JsonConvert.DeserializeObject(Of PinnaclePlus.onfleet.ErrorResponse)(Jstr)

                End If
            Catch ex As Exception

            End Try
            'System.Threading.Thread.Sleep(500)

        End Function

        Public Shared Function GetTeam(TeamID As String) As Team
            Dim objTeam As Team
            Dim strURL As String
            Dim myWebReq As HttpWebRequest
            Dim myWebResp As HttpWebResponse
            Dim objStream As Stream
            Dim encoding As New System.Text.UTF8Encoding
            Dim Jstr As String
            Dim sr As StreamReader
            'getData__1 = getData__1 & strJSON

            strURL = String.Format("https://onfleet.com/api/v2/teams/{0}", TeamID)

            'myWebReq.GetResponse.ToString()
            myWebReq = DirectCast(WebRequest.Create(strURL), HttpWebRequest)
            myWebReq.ContentType = "application/json; charset=utf-8"

            'myWebReq.ContentLength = data.Le
            myWebReq.Method = "GET"
            myWebReq.KeepAlive = True
            Dim autorization As String = "" 'My.Settings.nv_xml_path
            Dim binaryAuthorization As Byte() = System.Text.Encoding.UTF8.GetBytes(autorization)
            autorization = Convert.ToBase64String(binaryAuthorization)
            autorization = "Basic " + autorization
            myWebReq.Headers.Add("AUTHORIZATION", autorization)


            If myWebReq.Proxy IsNot Nothing Then
                myWebReq.Proxy.Credentials = CredentialCache.DefaultCredentials
            End If


            'myStream = myWebReq.GetRequestStream()
            myWebResp = DirectCast(myWebReq.GetResponse(), HttpWebResponse)
            objStream = myWebResp.GetResponseStream()
            Dim reader As New StreamReader(objStream)
            Jstr = reader.ReadToEnd()


            myWebResp.Close()
            'System.Threading.Thread.Sleep(1000)

            myWebReq = Nothing

            objTeam = JsonConvert.DeserializeObject(Of PinnaclePlus.onfleet.Team)(Jstr)
            Return objTeam
        End Function
    End Class



End Namespace
