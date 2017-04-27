Imports System.IO
Imports Rebex.Net
Namespace PinnaclePlus.nuVizz

    Public Class NuvizzAletrs
        Public Sub DO_PODs()
            Dim DocType, Description As String
            Dim FileName As String
            Dim DT As DataTable
            Dim FInfo As FileInfo
            DT = PinnaclePlus.SQLData.Dispatch.P_Manifest_Stop_Order_Event_Doc_GetPending()
            For i = 0 To DT.Rows.Count - 1
                Try

                    Select Case DT.Rows(i).Item("DocumentName")
                        Case "Delivery Receipt"
                            FileName = String.Format("/Content/ProofOfDelivery/{0}/POD{1}{2}.{3}", DT.Rows(i).Item("Order_ID"), Format(DT.Rows(i).Item("ReportedDTTM"), "ddMMyyyyHHmmss"), Format(DateTime.Now, "fff"), DT.Rows(i).Item("DocumentExtType"))
                            FInfo = New FileInfo(String.Format("{0}{1}", My.Settings.Pinnacle_File_Path, FileName))
                            CreateFile(DT.Rows(i).Item("DocumentData"), FInfo)
                            PinnaclePlus.SQLData.Dispatch.P_Attach_Proof(DT.Rows(i).Item("MSOED_ID"), DT.Rows(i).Item("Order_ID"), DT.Rows(i).Item("Driver_ID"), DT.Rows(i).Item("Hub"), DT.Rows(i).Item("ReportedDTTM"), FInfo.Name, FileName, DT.Rows(i).Item("Is_Pickup"), DT.Rows(i).Item("StopSigneeName"))
                        Case "Bill of Lading"
                            FileName = String.Format("/Content/ProofOfPickup/{0}/POP{1}{2}.{3}", DT.Rows(i).Item("Order_ID"), Format(DT.Rows(i).Item("ReportedDTTM"), "ddMMyyyyHHmmss"), Format(DateTime.Now, "fff"), DT.Rows(i).Item("DocumentExtType"))
                            FInfo = New FileInfo(String.Format("{0}{1}", My.Settings.Pinnacle_File_Path, FileName))
                            CreateFile(DT.Rows(i).Item("DocumentData"), FInfo)
                            PinnaclePlus.SQLData.Dispatch.P_Attach_Proof(DT.Rows(i).Item("MSOED_ID"), DT.Rows(i).Item("Order_ID"), DT.Rows(i).Item("Driver_ID"), DT.Rows(i).Item("Hub"), DT.Rows(i).Item("ReportedDTTM"), FInfo.Name, FileName, DT.Rows(i).Item("Is_Pickup"), DT.Rows(i).Item("StopSigneeName"))
                        Case Else
                            FileName = String.Format("/Content/Document/{0}/{1}.{2}", DT.Rows(i).Item("Order_ID"), Guid.NewGuid.ToString.ToLower, DT.Rows(i).Item("DocumentExtType"))
                            FInfo = New FileInfo(String.Format("{0}{1}", My.Settings.Pinnacle_File_Path, FileName))
                            CreateFile(DT.Rows(i).Item("DocumentData"), FInfo)
                            If DT.Rows(i).Item("DocumentExtType") = "pdf" Then
                                DocType = "10000"
                            Else
                                DocType = "40000"
                            End If
                            Description = String.Format("[{0}] {1}", DT.Rows(i).Item("A_Driver"), DT.Rows(i).Item("DocumentName"))
                            PinnaclePlus.SQLData.Dispatch.P_Attach_Image(DT.Rows(i).Item("MSOED_ID"), DT.Rows(i).Item("Order_ID"), FInfo.Name, FileName, Description, False, DocType)
                    End Select
                Catch ex As Exception
                    PinnaclePlus.SQLData.GeneralOperations.ExecuteNONQuery(String.Format("update [dbo].[T_Manifest_Stop_Order_Event_Doc] set P_Done=0,P_Error='{0}' where MSOED_ID={1}", ex.Message.Replace("'", "''"), DT.Rows(i).Item("MSOED_ID")), "", "")
                End Try
            Next
        End Sub
        Private Sub CreateFile(b64str As String, FInfo As FileInfo)

            If Not FInfo.Directory.Exists Then
                FInfo.Directory.Create()
            End If
            Dim binaryData() As Byte = Convert.FromBase64String(b64str)

            ' Assuming it's a jpg :)
            Dim fs As New FileStream(FInfo.FullName, FileMode.CreateNew)

            ' write it out
            fs.Write(binaryData, 0, binaryData.Length)

            ' close it down.
            fs.Close()
        End Sub
        Public Sub StartFTP()
            Dim client As New Rebex.Net.Sftp
            Dim StrFile As String

            If CType(HttpContext.Current.Application.Item("GetnuVizzRunning"), Object) Is Nothing Then
                HttpContext.Current.Application.Add("GetnuVizzRunning", False)
            End If
            If HttpContext.Current.Application.Item("GetnuVizzRunning") Then
                Exit Sub
            End If
            HttpContext.Current.Application.Item("GetnuVizzRunning") = True
            client.Connect("sftp.nuvizzapps.com", 2201)
            client.Login("metrowhprod", "M3tr0Pr0d")
            client.ChangeDirectory("/download")
            Dim list As SftpItemCollection = client.GetList()
            Dim item As SftpItem
            For Each item In list
                If item.IsFile Then
                    StrFile = String.Format("{0}{1}", My.Settings.nuVizz_xml_path, item.Name)
                    client.GetFile(item.Name, StrFile)
                    StrFile = String.Format("/download/done/{0}", item.Name)
                    client.Rename(String.Format("/download/{0}", item.Name), StrFile)
                End If
            Next item
            client.Disconnect()
            ProcessXMLFiles()
            DO_PODs()
            HttpContext.Current.Application.Item("GetnuVizzRunning") = False
        End Sub
        Private Sub ProcessXMLFiles()
            Dim DIr As System.IO.DirectoryInfo
            Dim DS As DataSet
            Dim DI As New System.IO.DirectoryInfo(My.Settings.nuVizz_xml_path)
            Dim FIs() As System.IO.FileInfo
            Dim FI As System.IO.FileInfo
            Dim DTTest As DataTable
            Dim EventCode As String
            FIs = DI.GetFiles()
            For Each FI In FIs

                If FI.Extension = ".xml" Then
                    DS = New DataSet
                    Try
                        DS.ReadXml(FI.FullName)
                    Catch ex1 As System.Xml.XmlException
                        DIr = New System.IO.DirectoryInfo(String.Format("{0}error\System.Xml.XmlException\", My.Settings.nuVizz_xml_path))
                        If Not DIr.Exists Then
                            DIr.Create()
                        End If
                        FI.MoveTo(String.Format("{0}\{1}", DIr.FullName, FI.Name))
                        Continue For
                    Catch ex As Exception
                        DIr = New System.IO.DirectoryInfo(String.Format("{0}error\Exception\", My.Settings.nuVizz_xml_path))
                        If Not DIr.Exists Then
                            DIr.Create()
                        End If
                        FI.MoveTo(String.Format("{0}\{1}", DIr.FullName, FI.Name))
                        Continue For
                    End Try

                    DTTest = DS.Tables("DeliverItLoadOut")
                    If DTTest Is Nothing Then
                        DTTest = DS.Tables("DeliverItStopDItoHost")
                    End If
                    EventCode = DTTest.Rows(0).Item("TriggeredByEvent")

                    Select Case EventCode

                        Case 7004
                            Try
                                Process_LoadIni_7004(DS, FI.Name)
                                FI.MoveTo(String.Format("{0}done\{1}", My.Settings.nuVizz_xml_path, FI.Name))
                            Catch ex As Exception
                                DIr = New System.IO.DirectoryInfo(String.Format("{0}error\Process_LoadIni_7004\", My.Settings.nuVizz_xml_path))
                                If Not DIr.Exists Then
                                    DIr.Create()
                                End If
                                FI.MoveTo(String.Format("{0}\{1}", DIr.FullName, FI.Name))
                                Dim fPath = FI.FullName.Replace(".xml", ".txt")
                                Dim afile As New IO.StreamWriter(fPath, True)
                                afile.WriteLine(ex.Message)
                                afile.Close()
                                Continue For
                            End Try
                        Case 7005
                            Try
                                Process_LoadComplete_7005(DS, FI.Name)
                                FI.MoveTo(String.Format("{0}done\{1}", My.Settings.nuVizz_xml_path, FI.Name))
                            Catch ex As Exception
                                DIr = New System.IO.DirectoryInfo(String.Format("{0}error\Process_LoadComplete_7005\", My.Settings.nuVizz_xml_path))
                                If Not DIr.Exists Then
                                    DIr.Create()
                                End If
                                FI.MoveTo(String.Format("{0}\{1}", DIr.FullName, FI.Name))
                                Dim fPath = FI.FullName.Replace(".xml", ".txt")
                                Dim afile As New IO.StreamWriter(fPath, True)
                                afile.WriteLine(ex.Message)
                                afile.Close()
                                Continue For
                            End Try
                        Case 7008
                            Try
                                Process_StopDeparture_7008(DS, FI.Name)
                                FI.MoveTo(String.Format("{0}done\{1}", My.Settings.nuVizz_xml_path, FI.Name))
                            Catch ex As Exception
                                DIr = New System.IO.DirectoryInfo(String.Format("{0}error\Process_StopDeparture_7008\", My.Settings.nuVizz_xml_path))
                                If Not DIr.Exists Then
                                    DIr.Create()
                                End If
                                FI.MoveTo(String.Format("{0}\{1}", DIr.FullName, FI.Name))
                                Dim fPath = FI.FullName.Replace(".xml", ".txt")
                                Dim afile As New IO.StreamWriter(fPath, True)
                                afile.WriteLine(ex.Message)
                                afile.Close()
                                Continue For
                            End Try
                        Case 7007
                            Try
                                Process_STOP_ARRIVAL_7007(DS, FI.Name)
                                FI.MoveTo(String.Format("{0}done\{1}", My.Settings.nuVizz_xml_path, FI.Name))
                            Catch ex As Exception
                                DIr = New System.IO.DirectoryInfo(String.Format("{0}error\Process_STOP_ARRIVAL_7007\", My.Settings.nuVizz_xml_path))
                                If Not DIr.Exists Then
                                    DIr.Create()
                                End If
                                FI.MoveTo(String.Format("{0}\{1}", DIr.FullName, FI.Name))
                                Dim fPath = FI.FullName.Replace(".xml", ".txt")
                                Dim afile As New IO.StreamWriter(fPath, True)
                                afile.WriteLine(ex.Message)
                                afile.Close()
                                Continue For
                            End Try
                    End Select
                End If
            Next
        End Sub
        Private Sub Process_STOP_ARRIVAL_7007(DS As DataSet, XML_File As String)
            Dim MSO_ID, MSOE_ID As Integer

            Dim TestRows() As DataRow

            MSO_ID = DS.Tables("Stop").Rows(0).Item("StopNbr")



            'Events and Docs
            If DS.Tables("Events") IsNot Nothing Then


                Dim Events_ID As Integer
                TestRows = DS.Tables("Events").Select("Stop_id=0")
                If TestRows.Count > 0 Then
                    Events_ID = TestRows(0).Item("Events_ID")
                    TestRows = DS.Tables("Event").Select(String.Format("Events_ID={0}", Events_ID))
                    For Each DR In TestRows
                        MSOE_ID = PinnaclePlus.SQLData.Dispatch.P_Manifest_Stop_Order_Event_IU(0, MSO_ID, DR.Item("EventId"), DR.Item("EventName"), DR.Item("ReportedDTTM").ToString.Replace("Z", ""), DR.Item("Latitude"), DR.Item("Longitude"), XML_File)
                    Next
                End If
            End If
            'End of Events and Docs
        End Sub
        Private Sub Process_StopDeparture_7008(DS As DataSet, XML_File As String)
            Dim MSO_ID, MSOE_ID As Integer

            Dim TestRows(), TestRows1(), TestRows2(), TestRows3(), RowsDocs(), RowsDoc() As DataRow
            Dim TestTable As DataTable
            Dim TestRow, TestRow1, TestRow2, TestRow3, DR, RowDocs, RowDoc As DataRow
            MSO_ID = DS.Tables("Stop").Rows(0).Item("StopNbr")
            Dim Duration As Object
            Duration = DS.Tables("Stop").Rows(0).Item("ActualDuration")
            If Duration.ToString.Trim = "" Then
                Duration = DBNull.Value
            Else
                Duration = CInt(Duration.ToString.ToLower.Replace("mins", "").Trim)
            End If
            Dim DeliveryStartDTTM, DeliveryEndDTTM As Object
            If DS.Tables("Stop").Columns.Contains("DeliveryStartDTTM") Then
                DeliveryStartDTTM = DS.Tables("Stop").Rows(0).Item("DeliveryStartDTTM").ToString.Replace("Z", "")
            Else
                DeliveryStartDTTM = DBNull.Value
            End If
            If DS.Tables("Stop").Columns.Contains("DeliveryEndDTTM") Then
                DeliveryEndDTTM = DS.Tables("Stop").Rows(0).Item("DeliveryEndDTTM").ToString.Replace("Z", "")
            Else
                DeliveryEndDTTM = DBNull.Value
            End If

            PinnaclePlus.SQLData.Dispatch.P_Manifest_Stop_Order_nuVizz(MSO_ID, DeliveryStartDTTM, DeliveryEndDTTM, Duration)
            'Event_ID->Documents.Documents_ID->Document.


            'Stop Details
            If DS.Tables("StopDetail") IsNot Nothing Then
                Dim ExceptionCode, ExceptionText As String
                Dim ConfirmQty As Object
                For i = 0 To DS.Tables("StopDetail").Rows.Count - 1
                    If DS.Tables("ConfirmedQuantity") IsNot Nothing Then

                        TestRows = DS.Tables("ConfirmedQuantity").Select(String.Format("StopDetail_id={0}", DS.Tables("StopDetail").Rows(i).Item("StopDetail_id")))
                        If TestRows.Count = 1 Then
                            ConfirmQty = CInt(TestRows(0).Item("ConfirmedQuantity_Text"))
                        Else
                            ConfirmQty = DBNull.Value
                        End If
                    Else
                        ConfirmQty = DBNull.Value
                    End If
                    If DS.Tables("ExceptionReason") IsNot Nothing Then
                        TestRows = DS.Tables("ExceptionReason").Select(String.Format("StopDetail_id={0}", DS.Tables("StopDetail").Rows(i).Item("StopDetail_id")))
                        If TestRows.Count = 1 Then
                            ExceptionCode = TestRows(0).Item("Code")
                            ExceptionText = TestRows(0).Item("ExceptionReason_Text")
                        Else
                            ExceptionCode = ""
                            ExceptionText = ""
                        End If
                    Else
                        ExceptionCode = ""
                        ExceptionText = ""
                    End If
                    PinnaclePlus.SQLData.Dispatch.P_Manifest_Stop_Order_Detail_IU(MSO_ID, DS.Tables("StopDetail").Rows(i).Item("ProductIdentifier"), ConfirmQty, ExceptionCode, ExceptionText)
                Next
            End If
            'End of Stop Details


            'Stop Exception
            If DS.Tables("StopExceptions") IsNot Nothing Then
                Dim Comment_Type, Comment_Text As String
                TestRows = DS.Tables("StopExceptions").Select("Stop_id=0")
                For Each TestRow In TestRows
                    TestRows1 = DS.Tables("StopException").Select(String.Format("StopExceptions_ID={0}", TestRow.Item("StopExceptions_ID")))
                    For Each TestRow1 In TestRows1
                        TestRows2 = DS.Tables("Exception").Select(String.Format("StopException_ID={0}", TestRow1.Item("StopException_ID")))
                        For Each TestRow2 In TestRows2
                            If DS.Tables("Comment") Is Nothing Then
                                PinnaclePlus.SQLData.Dispatch.P_Manifest_Stop_Order_Exception_IU(0, MSO_ID, TestRow2.Item("Code"), TestRow2.Item("Exception_Text"), "", "")
                            Else
                                If DS.Tables("Comment").Columns.Contains("StopException_ID") Then
                                    TestRows3 = DS.Tables("Comment").Select(String.Format("StopException_ID={0}", TestRow1.Item("StopException_ID")))
                                    If TestRows3.Count = 1 Then
                                        Comment_Type = TestRows3(0).Item("Type")
                                        Comment_Text = TestRows3(0).Item("Comment_Text")
                                        PinnaclePlus.SQLData.Dispatch.P_Manifest_Stop_Order_Exception_IU(0, MSO_ID, TestRow2.Item("Code"), TestRow2.Item("Exception_Text"), Comment_Type, Comment_Text)
                                    Else
                                        PinnaclePlus.SQLData.Dispatch.P_Manifest_Stop_Order_Exception_IU(0, MSO_ID, TestRow2.Item("Code"), TestRow2.Item("Exception_Text"), "", "")
                                    End If
                                Else
                                    PinnaclePlus.SQLData.Dispatch.P_Manifest_Stop_Order_Exception_IU(0, MSO_ID, TestRow2.Item("Code"), TestRow2.Item("Exception_Text"), "", "")
                                End If


                            End If

                        Next
                    Next
                Next
            End If
            'End of Stop Exception


            If DS.Tables("Attribute") IsNot Nothing Then

                For i = 0 To DS.Tables("Attribute").Rows.Count - 1
                    PinnaclePlus.SQLData.Dispatch.P_Manifest_Stop_Order_Survey_IU(MSO_ID, DS.Tables("Attribute").Rows(i).Item("Name"), DS.Tables("Attribute").Rows(i).Item("Value"), DS.Tables("Attribute").Rows(i).Item("Description"), XML_File)
                Next
            End If

            'Events and Docs
            If DS.Tables("Events") IsNot Nothing Then


                Dim Events_ID As Integer
                TestRows = DS.Tables("Events").Select("Stop_id=0")
                If TestRows.Count > 0 Then
                    Events_ID = TestRows(0).Item("Events_ID")
                    TestRows = DS.Tables("Event").Select(String.Format("Events_ID={0}", Events_ID))
                    For Each DR In TestRows
                        MSOE_ID = PinnaclePlus.SQLData.Dispatch.P_Manifest_Stop_Order_Event_IU(0, MSO_ID, DR.Item("EventId"), DR.Item("EventName"), DR.Item("ReportedDTTM").ToString.Replace("Z", ""), DR.Item("Latitude"), DR.Item("Longitude"), XML_File)
                        TestTable = DS.Tables("Documents")
                        If TestTable IsNot Nothing Then
                            RowsDocs = TestTable.Select(String.Format("Event_Id={0}", DR.Item("Event_Id")))
                            For Each RowDocs In RowsDocs
                                TestTable = DS.Tables("Document")
                                If TestTable IsNot Nothing Then
                                    RowsDoc = DS.Tables("Document").Select(String.Format("Documents_ID={0}", RowDocs.Item("Documents_ID")))
                                    For Each RowDoc In RowsDoc
                                        PinnaclePlus.SQLData.Dispatch.P_Manifest_Stop_Order_Event_Doc_IU(0, MSOE_ID, RowDoc.Item("DocumentName"), RowDoc.Item("DocumentType"), RowDoc.Item("DocumentExtType"), RowDoc.Item("DocumentData"), RowDoc.Item("StopSigneeName"), "")
                                    Next
                                End If
                            Next
                        End If
                    Next
                End If
            End If
            'End of Events and Docs
        End Sub
        Private Sub Process_LoadComplete_7005(DS As DataSet, XML_File As String)
            Dim MIN_ID As Integer
            Dim Status As Integer
            Dim StartTime As Object
            Dim EndTime As Object
            Dim DriverName As String
            Dim DriverID As String
            Dim DriverEmail As String
            Dim TestRow() As DataRow
            Dim DR As DataRow
            MIN_ID = DS.Tables("LoadHeader").Rows(0).Item("LoadNbr")
            Status = DS.Tables("LoadHeader").Rows(0).Item("Status")
            StartTime = Replace(DS.Tables("LoadHeader").Rows(0).Item("ActualStartDTTM"), "Z", "")
            EndTime = Replace(DS.Tables("LoadHeader").Rows(0).Item("ActualEndDTTM"), "Z", "")
            DriverName = DS.Tables("Driver").Rows(0).Item("DriverName")
            DriverID = DS.Tables("Driver").Rows(0).Item("DriverCDL")

            DriverEmail = DS.Tables("Driver").Rows(0).Item("DriverEmail")
            PinnaclePlus.SQLData.Dispatch.P_Manifest_Update_nuVizz(MIN_ID, Status, StartTime, EndTime, DriverName, DriverID, DriverEmail)
            'Dim Events_ID As Integer
            'TestRow = DS.Tables("Events").Select("LoadHeader_Id=0")
            'If TestRow.Count > 0 Then
            '    Events_ID = TestRow(0).Item("Events_ID")
            '    TestRow = DS.Tables("Event").Select(String.Format("Events_ID={0} and EventId in (7004)", Events_ID))
            '    For Each DR In TestRow
            '        PinnaclePlus.SQLData.Dispatch.P_Manifest_Event_IU(0, MIN_ID, DR.Item("EventId"), DR.Item("EventName"), DR.Item("ReportedDTTM").ToString.Replace("Z", ""), DR.Item("Latitude"), DR.Item("Longitude"), XML_File)
            '    Next
            'End If
        End Sub
        Private Sub Process_LoadIni_7004(DS As DataSet, XML_File As String)
            Dim MIN_ID As Integer
            Dim Status As Integer
            Dim StartTime As DateTime
            Dim DriverName As String
            Dim DriverID As String
            Dim DriverEmail As String
            Dim TestRow() As DataRow
            Dim DR As DataRow
            MIN_ID = DS.Tables("LoadHeader").Rows(0).Item("LoadNbr")
            Status = DS.Tables("LoadHeader").Rows(0).Item("Status")
            StartTime = Replace(DS.Tables("LoadHeader").Rows(0).Item("ActualStartDTTM"), "Z", "")
            DriverName = DS.Tables("Driver").Rows(0).Item("DriverName")
            DriverID = DS.Tables("Driver").Rows(0).Item("DriverCDL")

            DriverEmail = DS.Tables("Driver").Rows(0).Item("DriverEmail")
            PinnaclePlus.SQLData.Dispatch.P_Manifest_Update_nuVizz(MIN_ID, Status, StartTime, DBNull.Value, DriverName, DriverID, DriverEmail)
            Dim Events_ID As Integer
            TestRow = DS.Tables("Events").Select("LoadHeader_Id=0")
            If TestRow.Count > 0 Then
                Events_ID = TestRow(0).Item("Events_ID")
                TestRow = DS.Tables("Event").Select(String.Format("Events_ID={0} and EventId in (7004)", Events_ID))
                For Each DR In TestRow
                    PinnaclePlus.SQLData.Dispatch.P_Manifest_Event_IU(0, MIN_ID, DR.Item("EventId"), DR.Item("EventName"), DR.Item("ReportedDTTM").ToString.Replace("Z", ""), DR.Item("Latitude"), DR.Item("Longitude"), XML_File)
                Next
            End If
        End Sub
    End Class
End Namespace