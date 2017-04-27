Imports Newtonsoft.Json
Imports System.Net
Imports System.IO
Public Class getof
    Inherits System.Web.UI.Page

    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '    'GetAllOrders()
    '    GetTeams()
    '    GetWorkers()

    'End Sub
    'Private Sub GetWorkers()
    '    Dim OrderNO As Long
    '    Dim DT As DataTable
    '    Dim strURL As String
    '    Dim myWebReq As HttpWebRequest
    '    Dim myWebResp As HttpWebResponse
    '    Dim objStream As Stream
    '    Dim Workers() As PinnaclePlus.onfleet.Worker
    '    Dim Worker As PinnaclePlus.onfleet.Worker
    '    Dim encoding As New System.Text.UTF8Encoding
    '    Dim Teams() As String
    '    Dim Team As String
    '    Dim Jstr As String
    '    Dim sr As StreamReader
    '    'getData__1 = getData__1 & strJSON

    '    strURL = "https://onfleet.com/api/v2/workers"

    '    'myWebReq.GetResponse.ToString()
    '    myWebReq = DirectCast(WebRequest.Create(strURL), HttpWebRequest)
    '    myWebReq.ContentType = "application/json; charset=utf-8"

    '    'myWebReq.ContentLength = data.Le
    '    myWebReq.Method = "GET"
    '    myWebReq.KeepAlive = True
    '    Dim autorization As String = My.Settings.OF_API_Key
    '    Dim binaryAuthorization As Byte() = System.Text.Encoding.UTF8.GetBytes(autorization)
    '    autorization = Convert.ToBase64String(binaryAuthorization)
    '    autorization = "Basic " + autorization
    '    myWebReq.Headers.Add("AUTHORIZATION", autorization)


    '    If myWebReq.Proxy IsNot Nothing Then
    '        myWebReq.Proxy.Credentials = CredentialCache.DefaultCredentials
    '    End If


    '    'myStream = myWebReq.GetRequestStream()
    '    myWebResp = DirectCast(myWebReq.GetResponse(), HttpWebResponse)
    '    objStream = myWebResp.GetResponseStream()
    '    Dim reader As New StreamReader(objStream)
    '    Jstr = reader.ReadToEnd()


    '    myWebResp.Close()
    '    'System.Threading.Thread.Sleep(1000)

    '    myWebReq = Nothing

    '    Workers = JsonConvert.DeserializeObject(Of PinnaclePlus.onfleet.Worker())(Jstr)
    '    'Try
    '    For Each Worker In Workers
    '        PinnaclePlus.OrderBatch.P_Worker_IU(Worker.id, Worker.name)
    '        PinnaclePlus.OrderBatch.P_Worker_Team_Delete(Worker.id)
    '        For Each Team In Worker.teams
    '            PinnaclePlus.OrderBatch.P_Worker_Team_IU(Worker.id, Team)
    '        Next
    '    Next

    '    'PinnaclePlus.SQLData.GeneralOperations.ExecuteNONQuery(String.Format("update T_Batch_Orders set of_id='{0}',worker_id='{1}',short_id='{2}' where Order_ID={3}", obj.id, obj.worker, obj.shortId, OrderNO), "", "")
    '    'Catch ex As Exception

    '    'End Try
    '    'System.Threading.Thread.Sleep(500)


    'End Sub
    'Private Sub GetTeams()
    '    Dim OrderNO As Long
    '    Dim DT As DataTable
    '    Dim strURL As String
    '    Dim myWebReq As HttpWebRequest
    '    Dim myWebResp As HttpWebResponse
    '    Dim objStream As Stream
    '    Dim Teams() As PinnaclePlus.onfleet.Team
    '    Dim Team As PinnaclePlus.onfleet.Team
    '    Dim encoding As New System.Text.UTF8Encoding
    '    Dim Jstr As String
    '    Dim sr As StreamReader
    '    'getData__1 = getData__1 & strJSON

    '    strURL = "https://onfleet.com/api/v2/teams"

    '    'myWebReq.GetResponse.ToString()
    '    myWebReq = DirectCast(WebRequest.Create(strURL), HttpWebRequest)
    '    myWebReq.ContentType = "application/json; charset=utf-8"

    '    'myWebReq.ContentLength = data.Le
    '    myWebReq.Method = "GET"
    '    myWebReq.KeepAlive = True
    '    Dim autorization As String = My.Settings.OF_API_Key
    '    Dim binaryAuthorization As Byte() = System.Text.Encoding.UTF8.GetBytes(autorization)
    '    autorization = Convert.ToBase64String(binaryAuthorization)
    '    autorization = "Basic " + autorization
    '    myWebReq.Headers.Add("AUTHORIZATION", autorization)


    '    If myWebReq.Proxy IsNot Nothing Then
    '        myWebReq.Proxy.Credentials = CredentialCache.DefaultCredentials
    '    End If


    '    'myStream = myWebReq.GetRequestStream()
    '    myWebResp = DirectCast(myWebReq.GetResponse(), HttpWebResponse)
    '    objStream = myWebResp.GetResponseStream()
    '    Dim reader As New StreamReader(objStream)
    '    Jstr = reader.ReadToEnd()


    '    myWebResp.Close()
    '    'System.Threading.Thread.Sleep(1000)

    '    myWebReq = Nothing

    '    Teams = JsonConvert.DeserializeObject(Of PinnaclePlus.onfleet.Team())(Jstr)
    '    'Try
    '    For Each Team In Teams
    '        PinnaclePlus.OrderBatch.P_Teams_IU(Team.id, Team.hub, Team.name)
    '    Next

    '    'PinnaclePlus.SQLData.GeneralOperations.ExecuteNONQuery(String.Format("update T_Batch_Orders set of_id='{0}',worker_id='{1}',short_id='{2}' where Order_ID={3}", obj.id, obj.worker, obj.shortId, OrderNO), "", "")
    '    'Catch ex As Exception

    '    'End Try
    '    'System.Threading.Thread.Sleep(500)


    'End Sub
    'Private Sub GetAllOrders()
    '    Dim OrderNO As Long
    '    Dim DT As DataTable
    '    Dim strURL As String
    '    Dim myWebReq As HttpWebRequest
    '    Dim myWebResp As HttpWebResponse
    '    Dim objStream As Stream
    '    Dim encoding As New System.Text.UTF8Encoding
    '    Dim Jstr As String
    '    Dim sr As StreamReader
    '    'getData__1 = getData__1 & strJSON

    '    DT = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect("select * from Temp_Sohrt where s_id not in (select short_id from T_Batch_Orders  where short_id is not null)")
    '    For i = 0 To DT.Rows.Count - 1

    '        strURL = "https://onfleet.com/api/v2/tasks/shortId/{0}"
    '        strURL = String.Format(strURL, DT.Rows(i).Item(0))
    '        'myWebReq.GetResponse.ToString()
    '        myWebReq = DirectCast(WebRequest.Create(strURL), HttpWebRequest)
    '        myWebReq.ContentType = "application/json; charset=utf-8"

    '        'myWebReq.ContentLength = data.Le
    '        myWebReq.Method = "GET"
    '        myWebReq.KeepAlive = True
    '        Dim autorization As String = "3b373b28998434e97c18c24658584dc5"
    '        Dim binaryAuthorization As Byte() = System.Text.Encoding.UTF8.GetBytes(autorization)
    '        autorization = Convert.ToBase64String(binaryAuthorization)
    '        autorization = "Basic " + autorization
    '        myWebReq.Headers.Add("AUTHORIZATION", autorization)


    '        If myWebReq.Proxy IsNot Nothing Then
    '            myWebReq.Proxy.Credentials = CredentialCache.DefaultCredentials
    '        End If


    '        'myStream = myWebReq.GetRequestStream()
    '        myWebResp = DirectCast(myWebReq.GetResponse(), HttpWebResponse)
    '        objStream = myWebResp.GetResponseStream()
    '        Dim reader As New StreamReader(objStream)
    '        Jstr = reader.ReadToEnd()


    '        myWebResp.Close()
    '        'System.Threading.Thread.Sleep(1000)

    '        myWebReq = Nothing
    '        Dim obj As PinnaclePlus.onfleet.OnfleetTask
    '        obj = JsonConvert.DeserializeObject(Of PinnaclePlus.onfleet.OnfleetTask)(Jstr)
    '        'Try


    '        OrderNO = obj.notes.Substring(0, 17).Substring(10, 7)
    '        PinnaclePlus.SQLData.GeneralOperations.ExecuteNONQuery(String.Format("update T_Batch_Orders set of_id='{0}',worker_id='{1}',short_id='{2}' where Order_ID={3}", obj.id, obj.worker, obj.shortId, OrderNO), "", "")
    '        'Catch ex As Exception

    '        'End Try
    '        'System.Threading.Thread.Sleep(500)
    '    Next

    'End Sub

End Class