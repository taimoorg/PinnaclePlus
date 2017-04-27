Imports Newtonsoft.Json
Imports System.Net
Imports System.IO
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Namespace PinnaclePlus.nuVizz
    Public Class nuVizzAPI
        Sub New()
        End Sub
        Private Shared Function SetTimeFrom(Dt As DateTime) As DateTime
            Return Dt
        End Function
        Public Shared Function GetStop(Stop_number As String) As nuVizzStop
            Dim nvstop As nuVizzStop
            Dim strURL As String
            Dim myWebReq As HttpWebRequest
            Dim myWebResp As HttpWebResponse
            Dim objStream As Stream
            Dim encoding As New System.Text.UTF8Encoding
            Dim Jstr As String
            Dim sr As StreamReader
            'getData__1 = getData__1 & strJSON

            strURL = String.Format("{0}/deliverit/webservices/api/stop/{1}?stopNbr={2}", My.Settings.nuVizzApiURL, My.Settings.nuVizzIntegrationCompanyCode, Stop_number)

            'myWebReq.GetResponse.ToString()
            myWebReq = DirectCast(WebRequest.Create(strURL), HttpWebRequest)


            'myWebReq.ContentLength = data.Le
            myWebReq.Method = "GET"
            myWebReq.KeepAlive = True
            Dim autorization As String = My.Settings.nuVizzIntegrationUser + ":" + My.Settings.nuVizzIntegrationUserPassword
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
            Jstr = Left(Jstr, Jstr.Length - 1)
            Jstr = Right(Jstr, Jstr.Length - 1)

            myWebResp.Close()
            'System.Threading.Thread.Sleep(1000)

            myWebReq = Nothing
            If Jstr.Trim = "" Then
                nvstop = Nothing
            Else
                nvstop = JsonConvert.DeserializeObject(Of PinnaclePlus.nuVizz.nuVizzStop)(Jstr)
            End If


            Return nvstop
        End Function
        
        Public Shared Function GetXmlLoad(MIN_ID As Integer) As String
            Dim RetXML As String = ""
            Dim DT, DT_Detail As DataTable
            Dim DRManifest As DataRow
            Dim DTTM As DateTime
            Dim Pic_Del As String

            DRManifest = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleRow(String.Format("select *,	[dbo].[F_Manifest_Get_CuTfs](MIN_ID) as Tot_CuTfs,[dbo].[F_Manifest_Get_Qty](MIN_ID) as Tot_Qty,[dbo].[F_Manifest_Get_Wt](MIN_ID) as Tot_Wt from V_Manifest where MIN_ID={0}", MIN_ID))


            RetXML = "<?xml version=""1.0"" encoding=""UTF-8""?>"
            RetXML = String.Format("{0}<di:DeliverItLoad xsi:schemaLocation=""http://www.nuvizzards.com/schemas/deliverit/Loads NuvizzDeliveritLoads.xsd "" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:di=""http://www.nuvizzards.com/schemas/deliverit/Loads"">", RetXML)
            RetXML = String.Format("{0}<di:DocumentID>123456</di:DocumentID>", RetXML)
            RetXML = String.Format("{0}<di:CompanyCode>{1}</di:CompanyCode>", RetXML, My.Settings.nuVizzIntegrationCompanyCode)
            RetXML = String.Format("{0}<di:Loads>", RetXML)
            RetXML = String.Format("{0}<di:Load>", RetXML)
            RetXML = String.Format("{0}<di:LoadHeader>", RetXML)
            RetXML = String.Format("{0}<di:Function>03</di:Function>", RetXML)
            RetXML = String.Format("{0}<di:LoadNbr>{1}</di:LoadNbr>", RetXML, DRManifest.Item("MIN_ID"))
            'RetXML = String.Format("{0}<di:Origin>di:Origin</di:Origin>", RetXML, DRManifest.Item("Name"))
            RetXML = String.Format("{0}<di:OriginName>{1}</di:OriginName>", RetXML, DRManifest.Item("Name"))
            RetXML = String.Format("{0}<di:OriginAddr1>{1}</di:OriginAddr1>", RetXML, DRManifest.Item("Address1"))
            'RetXML = String.Format("{0}<di:OriginAddr2>di:OriginAddr2</di:OriginAddr2>", RetXML)
            RetXML = String.Format("{0}<di:OriginCity>{1}</di:OriginCity>", RetXML, DRManifest.Item("City"))
            RetXML = String.Format("{0}<di:OriginState>{1}</di:OriginState>", RetXML, DRManifest.Item("State"))
            RetXML = String.Format("{0}<di:OriginZip>{1}</di:OriginZip>", RetXML, DRManifest.Item("Zip"))
            RetXML = String.Format("{0}<di:OriginCountry>US</di:OriginCountry>", RetXML)
            RetXML = String.Format("{0}<di:OriginLatitude>{1}</di:OriginLatitude>", RetXML, DRManifest.Item("Lat"))
            RetXML = String.Format("{0}<di:OriginLongitude>{1}</di:OriginLongitude>", RetXML, DRManifest.Item("Lng"))
            'RetXML = String.Format("{0}<di:OriginPhoneNumber/>", RetXML)
            'RetXML = String.Format("{0}<di:TractorNbr>di:TractorNbr</di:TractorNbr>", RetXML)
            'RetXML = String.Format("{0}<di:TrailerNbr>di:TrailerNbr</di:TrailerNbr>", RetXML)
            'RetXML = String.Format("{0}<di:TrailerType>di:TrailerType</di:TrailerType>", RetXML)
            'RetXML = String.Format("{0}<di:TrailerSize>di:TrailerSize</di:TrailerSize>", RetXML)
            'RetXML = String.Format("{0}<di:PRONbr>di:PRONbr</di:PRONbr>", RetXML)
            'RetXML = String.Format("{0}<di:MasterBOL>di:MasterBOL</di:MasterBOL>", RetXML)
            'RetXML = String.Format("{0}<di:Reference>di:Reference</di:Reference>", RetXML)
            RetXML = String.Format("{0}<di:EarliestStartDTTM>{1}-{2}-{3}T{4}:{5}:{6}</di:EarliestStartDTTM>", RetXML, CDate(DRManifest.Item("WorkStart_UTC")).Year, CDate(DRManifest.Item("WorkStart_UTC")).Month, CDate(DRManifest.Item("WorkStart_UTC")).Day, CDate(DRManifest.Item("WorkStart_UTC")).Hour, CDate(DRManifest.Item("WorkStart_UTC")).Minute, CDate(DRManifest.Item("WorkStart_UTC")).Second)
            'RetXML = String.Format("{0}<di:LatestStartDTTM>2001-12-31T12:00:00</di:LatestStartDTTM>", RetXML)
            RetXML = String.Format("{0}<di:Weight UOM=""Lbs"">{1}</di:Weight>", RetXML, DRManifest.Item("Tot_Wt"))
            RetXML = String.Format("{0}<di:Volume UOM=""Cu. Ft."">{1}</di:Volume>", RetXML, DRManifest.Item("Tot_CuTfs"))
            RetXML = String.Format("{0}<di:TotalPallets>{1}</di:TotalPallets>", RetXML, DRManifest.Item("Tot_Qty"))
            RetXML = String.Format("{0}<di:TotalCartons>{1}</di:TotalCartons>", RetXML, DRManifest.Item("Tot_Qty"))
            'RetXML = String.Format("{0}<di:SealNbr>di:SealNbr</di:SealNbr>", RetXML)
            'RetXML = String.Format("{0}<di:StopSeqOrder>0</di:StopSeqOrder>", RetXML)
            'RetXML = String.Format("{0}<di:Comments>", RetXML)
            'RetXML = String.Format("{0}<di:Comment Type="""">di:Comment</di:Comment>", RetXML)
            'RetXML = String.Format("{0}</di:Comments>", RetXML) 
            'RetXML = String.Format("{0}<di:Documents>", RetXML)
            'RetXML = String.Format("{0}<di:Document>", RetXML)
            'RetXML = String.Format("{0}<di:DocumentName>Document Name</di:DocumentName>", RetXML)
            'RetXML = String.Format("{0}<di:DocumentURL>file:filename.pdf</di:DocumentURL>", RetXML)
            'RetXML = String.Format("{0}<di:DispositionType>01/02/03/04</di:DispositionType>", RetXML)
            'RetXML = String.Format("{0}<di:Reference>Existing document</di:Reference>", RetXML)
            'RetXML = String.Format("{0}<di:Description>Existing document</di:Description>", RetXML)
            'RetXML = String.Format("{0}</di:Document>", RetXML)
            'RetXML = String.Format("{0}</di:Documents>", RetXML)
            RetXML = String.Format("{0}</di:LoadHeader>", RetXML)
            DT = PinnaclePlus.SQLData.Dispatch.P_Manifest_Stop_Order_GetByMIN_ID_UTC(MIN_ID)
            RetXML = String.Format("{0}<di:Stops>", RetXML)
            For i = 0 To DT.Rows.Count - 1
                If DT.Rows(i).Item("Is_Pickup") Then
                    Pic_Del = "Pic"
                Else
                    Pic_Del = "Del"
                End If
                RetXML = String.Format("{0}<di:Stop>", RetXML)
                'RetXML = String.Format("{0}<di:Function>01</di:Function>", RetXML)
                RetXML = String.Format("{0}<di:StopNbr>{1}</di:StopNbr>", RetXML, DT.Rows(i).Item("MSO_ID"))
                RetXML = String.Format("{0}<di:StopSeq>{1}</di:StopSeq>", RetXML, i + 1)


                RetXML = String.Format("{0}<di:ShipmentNbr>{1}-{2}</di:ShipmentNbr>", RetXML, DT.Rows(i).Item("Order_ID"), DT.Rows(i).Item("Source_"))
                RetXML = String.Format("{0}<di:BOL>{1}</di:BOL>", RetXML, DRManifest.Item("Hub").ToString.Trim)
                RetXML = String.Format("{0}<di:SignatureRequired>false</di:SignatureRequired>", RetXML)
                RetXML = String.Format("{0}<di:ShipToName>{1}</di:ShipToName>", RetXML, Left(String.Format("{1}-{0}", DT.Rows(i).Item(String.Format("Name_{0}", Pic_Del)).ToString.Trim.Replace("<", "").Replace(">", ""), DT.Rows(i).Item("Order_ID")), 50))
                RetXML = String.Format("{0}<di:ShipToAddr1>{1}</di:ShipToAddr1>", RetXML, DT.Rows(i).Item(String.Format("Address1_{0}", Pic_Del)).ToString.Trim)
                RetXML = String.Format("{0}<di:ShipToAddr2>{1}</di:ShipToAddr2>", RetXML, DT.Rows(i).Item(String.Format("Address2_{0}", Pic_Del)).ToString.Trim)
                RetXML = String.Format("{0}<di:ShipToCity>{1}</di:ShipToCity>", RetXML, DT.Rows(i).Item(String.Format("City_{0}", Pic_Del)).ToString.Trim)
                RetXML = String.Format("{0}<di:ShipToState>{1}</di:ShipToState>", RetXML, DT.Rows(i).Item(String.Format("State_{0}", Pic_Del)).ToString.Trim)
                RetXML = String.Format("{0}<di:ShipToZip>{1}</di:ShipToZip>", RetXML, DT.Rows(i).Item(String.Format("Zip_{0}", Pic_Del)).ToString.Trim)
                RetXML = String.Format("{0}<di:ShipToCountry>US</di:ShipToCountry>", RetXML)
                RetXML = String.Format("{0}<di:BillToName>{1}</di:BillToName>", RetXML, DT.Rows(i).Item("Company_Bill").ToString.Trim)
                RetXML = String.Format("{0}<di:BillToAddr1>{1}</di:BillToAddr1>", RetXML, Left(DT.Rows(i).Item("Address1_Bill").ToString.Trim, 31))
                RetXML = String.Format("{0}<di:BillToAddr2>{1}</di:BillToAddr2>", RetXML, Left(DT.Rows(i).Item("Address2_Bill").ToString.Trim, 31))
                RetXML = String.Format("{0}<di:BillToCity>{1}</di:BillToCity>", RetXML, DT.Rows(i).Item("City_Bill").ToString.Trim)
                RetXML = String.Format("{0}<di:BillToState>{1}</di:BillToState>", RetXML, DT.Rows(i).Item("State_Bill").ToString.Trim)
                RetXML = String.Format("{0}<di:BillToZip>{1}</di:BillToZip>", RetXML, DT.Rows(i).Item("Zip_Bill").ToString.Trim)
                RetXML = String.Format("{0}<di:BillToCountry>US</di:BillToCountry>", RetXML)

                DTTM = SetTimeFrom(DT.Rows(i).Item("TW_Start"))
                RetXML = String.Format("{0}<di:EarliestStartDTTM>{1}-{2}-{3}T{4}:{5}:{6}</di:EarliestStartDTTM>", RetXML, DTTM.Year, DTTM.Month, DTTM.Day, DTTM.Hour, DTTM.Minute, DTTM.Second)
                DTTM = SetTimeFrom(DT.Rows(i).Item("TW_End"))

                RetXML = String.Format("{0}<di:LatestStartDTTM>{1}-{2}-{3}T{4}:{5}:{6}</di:LatestStartDTTM>", RetXML, DTTM.Year, DTTM.Month, DTTM.Day, DTTM.Hour, DTTM.Minute, DTTM.Second)
                RetXML = String.Format("{0}<di:EstimatedDuration>{1}</di:EstimatedDuration>", RetXML, DT.Rows(i).Item("Delivery_Time"))
                RetXML = String.Format("{0}<di:Weight UOM=""Lbs"">{1}</di:Weight>", RetXML, DT.Rows(i).Item("Wt").ToString.Trim)
                RetXML = String.Format("{0}<di:Volume UOM=""Cu. Ft."">{1}</di:Volume>", RetXML, DT.Rows(i).Item("CuFts").ToString.Trim)

                '<di:TotalPallets>0</di:TotalPallets>
                '<di:TotalCartons>0</di:TotalCartons>
                '<di:SealNbr>di:SealNbr</di:SealNbr>
                '<di:FreightTerms>di:FreightTerms</di:FreightTerms>
                If DT.Rows(i).Item(String.Format("Phone_{0}", Pic_Del)).ToString.Trim <> "" Then
                    RetXML = String.Format("{0}<di:PhoneNumber>{1}</di:PhoneNumber>", RetXML, DT.Rows(i).Item(String.Format("Phone_{0}", Pic_Del)).ToString.Trim)
                End If

                '<di:FaxNumber/>
                '</>
                RetXML = String.Format("{0}<di:Email>{1}</di:Email>", RetXML, DT.Rows(i).Item(String.Format("Email_{0}", Pic_Del)).ToString.Trim)
                '<di:ServiceCategory/>
                '<di:AccountNumber/>
                If DT.Rows(i).Item("Is_Pickup") Then
                    RetXML = String.Format("{0}<di:StopType>01</di:StopType>", RetXML)
                Else
                    RetXML = String.Format("{0}<di:StopType>02</di:StopType>", RetXML)
                End If
                '<di:Latitude/>
                '<di:Longitude/>
                RetXML = String.Format("{0}<di:WorkType>CustomerSurvey</di:WorkType>", RetXML)
                If DT.Rows(i).Item(String.Format("Cell_{0}", Pic_Del)).ToString.Trim <> "" Then
                    RetXML = String.Format("{0}<di:SMSNumber>{1}</di:SMSNumber>", RetXML, DT.Rows(i).Item(String.Format("Cell_{0}", Pic_Del)).ToString.Trim)
                End If
                '<di:StopPriority/>
                '-<di:Comments>
                '<di:Comment Type="">di:Comment</di:Comment>
                '</di:Comments>

                RetXML = String.Format("{0}<di:Documents>", RetXML)
                RetXML = String.Format("{0}<di:Document>", RetXML)
                If DT.Rows(i).Item("Is_Pickup") Then
                    RetXML = String.Format("{0}<di:DocumentName>Bill of Lading</di:DocumentName>", RetXML)
                Else
                    RetXML = String.Format("{0}<di:DocumentName>Delivery Receipt</di:DocumentName>", RetXML)
                End If

                RetXML = String.Format("{0}<di:DocumentType>03</di:DocumentType>", RetXML)
                RetXML = String.Format("{0}<di:DocumentExtType>pdf</di:DocumentExtType>", RetXML)
                RetXML = String.Format("{0}<di:DocumentURL></di:DocumentURL>", RetXML)
                If DT.Rows(i).Item("Is_Pickup") Then
                    RetXML = String.Format("{0}<di:DocumentData>{1}</di:DocumentData>", RetXML, GetBOLBase64(DT.Rows(i).Item("Order_ID"), DT.Rows(i).Item("Source_"), MIN_ID, DT.Rows(i).Item("Seq")))
                Else
                    RetXML = String.Format("{0}<di:DocumentData>{1}</di:DocumentData>", RetXML, GetDLBase64(DT.Rows(i).Item("Order_ID"), DT.Rows(i).Item("Source_"), MIN_ID, DT.Rows(i).Item("Seq")))
                End If

                RetXML = String.Format("{0}<di:DispositionType>01</di:DispositionType>", RetXML)
                '<di:Reference>Existing document</di:Reference>
                '<di:Description>Existing document</di:Description>
                RetXML = String.Format("{0}</di:Document>", RetXML)
                RetXML = String.Format("{0}</di:Documents>", RetXML)
                DT_Detail = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelect(String.Format("select [Item Tracking No],[Item Description],[Item Quantity] as Qty,[Weight],[Dimensions],[Dimension Unit],[Cu_Ft_],[Amount] from [MetroPolitanNavProduction].[dbo].[Metropolitan$Sales Line Link] where [Sales Order No]='{0}' and [Document Type]=1   --group by [Item Description]      ,[Weight]      ,[Dimensions]      ,[Dimension Unit]      ,[Cu_Ft_]      ,[Amount]", DT.Rows(i).Item("Order_ID")))
                RetXML = String.Format("{0}<di:StopDetails>", RetXML)

                For j = 0 To DT_Detail.Rows.Count - 1
                    RetXML = String.Format("{0}<di:StopDetail>", RetXML)
                    RetXML = String.Format("{0}<di:StopDetailSeq>{1}</di:StopDetailSeq>", RetXML, j)
                    RetXML = String.Format("{0}<di:Product>{1}</di:Product>", RetXML, Left(DT_Detail.Rows(j).Item("Item Description"), 100))
                    If DT_Detail.Rows(j).Item("Item Description") = "" Then
                        Return String.Format("ERR:Order No {0} Has no item description", DT.Rows(i).Item("Order_ID"))
                        Exit Function
                    End If
                    RetXML = String.Format("{0}<di:ProductIdentifier>{1}</di:ProductIdentifier>", RetXML, DT_Detail.Rows(j).Item("Item Tracking No"))
                    'RetXML = String.Format("{0}<di:OriginalLineNumber>1</di:OriginalLineNumber>", RetXML)
                    RetXML = String.Format("{0}<di:Quantity UOM=""Units"">1</di:Quantity>", RetXML)
                    RetXML = String.Format("{0}<di:Weight UOM=""Lbs"">{1}</di:Weight>", RetXML, DT_Detail.Rows(j).Item("Weight"))
                    RetXML = String.Format("{0}<di:Volume UOM=""Cu. Ft."">{1}</di:Volume>", RetXML, DT_Detail.Rows(j).Item("Cu_Ft_"))
                    'RetXML = String.Format("{0}<di:ServiceCategory/>", RetXML) 
                    'RetXML = String.Format("{0}<di:AccountNumber/>", RetXML) 
                    'RetXML = String.Format("{0}<di:ReferenceText/>", RetXML) 
                    'RetXML = String.Format("{0}<di:LineType/>", RetXML) 
                    RetXML = String.Format("{0}</di:StopDetail>", RetXML)
                Next
                RetXML = String.Format("{0}</di:StopDetails>", RetXML)
                'RetXML = String.Format("{0}<di:StopAssignment>", RetXML)
                'RetXML = String.Format("{0}<di:CarrierCode>METRO</di:CarrierCode>", RetXML)
                'RetXML = String.Format("{0}</di:StopAssignment>", RetXML)
                RetXML = String.Format("{0}</di:Stop>", RetXML)
            Next
            RetXML = String.Format("{0}</di:Stops>", RetXML)



            RetXML = String.Format("{0}<di:LoadAssignment>", RetXML)
            RetXML = String.Format("{0}<di:LoadSeq>0</di:LoadSeq>", RetXML)
            RetXML = String.Format("{0}<di:SequenceOverridable>false</di:SequenceOverridable>", RetXML)
            'RetXML = String.Format("{0}<di:StopGeofenceRadius>300</di:StopGeofenceRadius>", RetXML)
            If DRManifest.Item("Hub").ToString.Trim <> "NJ-PA" Then
                RetXML = String.Format("{0}<di:CarrierCode>{1}</di:CarrierCode>", RetXML, DRManifest.Item("nuVizz_Code").ToString.Trim)
            End If
            RetXML = String.Format("{0}<di:DriverCDL>{1}</di:DriverCDL>", RetXML, DRManifest.Item("DRIVER_ID"))
            'RetXML = String.Format("{0}<di:DriverEmail>driver@abc.com</di:DriverEmail>", RetXML)
            'RetXML = String.Format("{0}<di:DriverName>driverusername</di:DriverName>", RetXML)
            RetXML = String.Format("{0}</di:LoadAssignment>", RetXML)
            RetXML = String.Format("{0}</di:Load>", RetXML)
            RetXML = String.Format("{0}</di:Loads>", RetXML)
            RetXML = String.Format("{0}</di:DeliverItLoad>", RetXML)
            Return RetXML
        End Function
        Public Shared Function GetXmlLoadDelete(MIN_ID As Integer) As String
            Dim RetXML As String = ""
            Dim DT, DT_Detail As DataTable
            Dim DRManifest As DataRow
            Dim DTTM As DateTime
            Dim Pic_Del As String

            DRManifest = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleRow(String.Format("select *,	[dbo].[F_Manifest_Get_CuTfs](MIN_ID) as Tot_CuTfs,[dbo].[F_Manifest_Get_Qty](MIN_ID) as Tot_Qty,[dbo].[F_Manifest_Get_Wt](MIN_ID) as Tot_Wt from V_Manifest where MIN_ID={0}", MIN_ID))


            RetXML = "<?xml version=""1.0"" encoding=""UTF-8""?>"
            RetXML = String.Format("{0}<di:DeliverItLoad xsi:schemaLocation=""http://www.nuvizzards.com/schemas/deliverit/Loads NuvizzDeliveritLoads.xsd "" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:di=""http://www.nuvizzards.com/schemas/deliverit/Loads"">", RetXML)
            RetXML = String.Format("{0}<di:DocumentID>123456</di:DocumentID>", RetXML)
            RetXML = String.Format("{0}<di:CompanyCode>{1}</di:CompanyCode>", RetXML, My.Settings.nuVizzIntegrationCompanyCode)
            RetXML = String.Format("{0}<di:Loads>", RetXML)
            RetXML = String.Format("{0}<di:Load>", RetXML)
            RetXML = String.Format("{0}<di:LoadHeader>", RetXML)
            RetXML = String.Format("{0}<di:Function>02</di:Function>", RetXML)
            RetXML = String.Format("{0}<di:LoadNbr>{1}</di:LoadNbr>", RetXML, DRManifest.Item("MIN_ID"))
            'RetXML = String.Format("{0}<di:Origin>di:Origin</di:Origin>", RetXML, DRManifest.Item("Name"))
            RetXML = String.Format("{0}<di:OriginName>{1}</di:OriginName>", RetXML, DRManifest.Item("Name"))
            RetXML = String.Format("{0}<di:OriginAddr1>{1}</di:OriginAddr1>", RetXML, DRManifest.Item("Address1"))
            'RetXML = String.Format("{0}<di:OriginAddr2>di:OriginAddr2</di:OriginAddr2>", RetXML)
            RetXML = String.Format("{0}<di:OriginCity>{1}</di:OriginCity>", RetXML, DRManifest.Item("City"))
            RetXML = String.Format("{0}<di:OriginState>{1}</di:OriginState>", RetXML, DRManifest.Item("State"))
            RetXML = String.Format("{0}<di:OriginZip>{1}</di:OriginZip>", RetXML, DRManifest.Item("Zip"))
            RetXML = String.Format("{0}<di:OriginCountry>US</di:OriginCountry>", RetXML)
            RetXML = String.Format("{0}<di:OriginLatitude>{1}</di:OriginLatitude>", RetXML, DRManifest.Item("Lat"))
            RetXML = String.Format("{0}<di:OriginLongitude>{1}</di:OriginLongitude>", RetXML, DRManifest.Item("Lng"))
            'RetXML = String.Format("{0}<di:OriginPhoneNumber/>", RetXML)
            'RetXML = String.Format("{0}<di:TractorNbr>di:TractorNbr</di:TractorNbr>", RetXML)
            'RetXML = String.Format("{0}<di:TrailerNbr>di:TrailerNbr</di:TrailerNbr>", RetXML)
            'RetXML = String.Format("{0}<di:TrailerType>di:TrailerType</di:TrailerType>", RetXML)
            'RetXML = String.Format("{0}<di:TrailerSize>di:TrailerSize</di:TrailerSize>", RetXML)
            'RetXML = String.Format("{0}<di:PRONbr>di:PRONbr</di:PRONbr>", RetXML)
            'RetXML = String.Format("{0}<di:MasterBOL>di:MasterBOL</di:MasterBOL>", RetXML)
            'RetXML = String.Format("{0}<di:Reference>di:Reference</di:Reference>", RetXML)
            RetXML = String.Format("{0}<di:EarliestStartDTTM>{1}-{2}-{3}T{4}:{5}:{6}</di:EarliestStartDTTM>", RetXML, CDate(Now.Date).Year, CDate(Now.Date).Month, CDate(Now.Date).Day, CDate(Now.Date).Hour, CDate(Now.Date).Minute, CDate(Now.Date).Second)
            'RetXML = String.Format("{0}<di:LatestStartDTTM>2001-12-31T12:00:00</di:LatestStartDTTM>", RetXML)
            RetXML = String.Format("{0}<di:Weight UOM=""Lbs"">{1}</di:Weight>", RetXML, DRManifest.Item("Tot_Wt"))
            RetXML = String.Format("{0}<di:Volume UOM=""Cu. Ft."">{1}</di:Volume>", RetXML, DRManifest.Item("Tot_CuTfs"))
            RetXML = String.Format("{0}<di:TotalPallets>{1}</di:TotalPallets>", RetXML, DRManifest.Item("Tot_Qty"))
            RetXML = String.Format("{0}<di:TotalCartons>{1}</di:TotalCartons>", RetXML, DRManifest.Item("Tot_Qty"))
            'RetXML = String.Format("{0}<di:SealNbr>di:SealNbr</di:SealNbr>", RetXML)
            'RetXML = String.Format("{0}<di:StopSeqOrder>0</di:StopSeqOrder>", RetXML)
            'RetXML = String.Format("{0}<di:Comments>", RetXML)
            'RetXML = String.Format("{0}<di:Comment Type="""">di:Comment</di:Comment>", RetXML)
            'RetXML = String.Format("{0}</di:Comments>", RetXML) 
            'RetXML = String.Format("{0}<di:Documents>", RetXML)
            'RetXML = String.Format("{0}<di:Document>", RetXML)
            'RetXML = String.Format("{0}<di:DocumentName>Document Name</di:DocumentName>", RetXML)
            'RetXML = String.Format("{0}<di:DocumentURL>file:filename.pdf</di:DocumentURL>", RetXML)
            'RetXML = String.Format("{0}<di:DispositionType>01/02/03/04</di:DispositionType>", RetXML)
            'RetXML = String.Format("{0}<di:Reference>Existing document</di:Reference>", RetXML)
            'RetXML = String.Format("{0}<di:Description>Existing document</di:Description>", RetXML)
            'RetXML = String.Format("{0}</di:Document>", RetXML)
            'RetXML = String.Format("{0}</di:Documents>", RetXML)
            RetXML = String.Format("{0}</di:LoadHeader>", RetXML)
            
            RetXML = String.Format("{0}</di:Load>", RetXML)
            RetXML = String.Format("{0}</di:Loads>", RetXML)
            RetXML = String.Format("{0}</di:DeliverItLoad>", RetXML)
            Return RetXML
        End Function
        Public Shared Function ExportLoad(MIN_ID As Integer, IsDelete As Boolean) As String
            Dim XMLTxt As String

            Dim RetDS As New DataSet
            Dim ResponceXML As String
            Dim strURL As String
            Dim myWebReq As HttpWebRequest
            Dim myWebResp As HttpWebResponse
            Dim encoding As New System.Text.UTF8Encoding
            Dim objStream As Stream
            Dim sr As StreamReader
            Dim A() As String
            If IsDelete Then
                XMLTxt = GetXmlLoadDelete(MIN_ID)
            Else

                XMLTxt = GetXmlLoad(MIN_ID)
            End If
            A = XMLTxt.Split(":")
            If A(0) = "ERR" Then
                Return A(1)
                Exit Function
            End If
            'getData__1 = getData__1 & strJSON


            strURL = String.Format("{0}/deliverit/webservices/api/load/{1}", My.Settings.nuVizzApiURL, My.Settings.nuVizzIntegrationCompanyCode)

            'myWebReq.GetResponse.ToString()

            myWebReq = DirectCast(WebRequest.Create(strURL), HttpWebRequest)
            myWebReq.ContentType = "application/xml;"

            'myWebReq.ContentLength = data.Le
            myWebReq.Method = "POST"
            myWebReq.KeepAlive = True
            Dim autorization As String = My.Settings.nuVizzIntegrationUser + ":" + My.Settings.nuVizzIntegrationUserPassword
            Dim binaryAuthorization As Byte() = System.Text.Encoding.UTF8.GetBytes(autorization)
            autorization = Convert.ToBase64String(binaryAuthorization)
            autorization = "Basic " + autorization
            myWebReq.Headers.Add("AUTHORIZATION", autorization)


            If myWebReq.Proxy IsNot Nothing Then
                myWebReq.Proxy.Credentials = CredentialCache.DefaultCredentials
            End If
            Using myStream As Stream = myWebReq.GetRequestStream()

                'myStream = myWebReq.GetRequestStream()
                Dim data As Byte() = encoding.GetBytes(Replace(XMLTxt, "&", ""))
                If data.Length > 0 Then
                    myStream.Write(data, 0, data.Length)
                    myStream.Close()
                End If
                Try
                    myWebResp = DirectCast(myWebReq.GetResponse(), HttpWebResponse)
                    objStream = myWebResp.GetResponseStream()
                    sr = New StreamReader(objStream)
                    ResponceXML = sr.ReadToEnd()


                    Dim xmlSR As System.IO.StringReader = New System.IO.StringReader(ResponceXML)

                    RetDS.ReadXml(xmlSR)
                    If RetDS.Tables("DeliverItLoadResponse").Rows(0).Item("Status") = 10 Then
                        PinnaclePlus.SQLData.GeneralOperations.ExecuteNONQuery(String.Format("update T_Manifest set ExportednuVizz=1 where MIN_ID={0}", MIN_ID), "", "")
                        Return ""
                    Else
                        Return RetDS.Tables("Error").Rows(0).Item("Error_Text")
                    End If

                    myWebResp.Close()
                    myWebReq = Nothing
                Catch wex As WebException
                    'If wex.Response IsNot Nothing Then
                    '    myWebResp = DirectCast(wex.Response, HttpWebResponse)
                    '    objStream = myWebResp.GetResponseStream()
                    '    sr = New StreamReader(objStream)
                    '    JsonStr = sr.ReadToEnd()
                    '    Dim Objerr As PinnaclePlus.onfleet.ErrorResponse = JsonConvert.DeserializeObject(Of PinnaclePlus.onfleet.ErrorResponse)(JsonStr)
                    '    PinnaclePlus.SQLData.GeneralOperations.ExecuteNONQuery(String.Format("update T_Manifest_Order set of_error='{0}' where MO_ID={1}", Objerr.message.message, DrLocal.Item("MO_ID")), "", "")
                    'End If
                    Return wex.Message
                Catch ex As Exception

                End Try
            End Using

        End Function
        Public Shared Sub ExportStopsToNuvizz(XMLTxt As String)
            Dim ResponceXML As String
            Dim strURL As String
            Dim myWebReq As HttpWebRequest
            Dim myWebResp As HttpWebResponse
            Dim encoding As New System.Text.UTF8Encoding
            Dim objStream As Stream
            Dim sr As StreamReader
            'getData__1 = getData__1 & strJSON


            strURL = String.Format("{0}/deliverit/webservices/api/stop/{1}", My.Settings.nuVizzApiURL, My.Settings.nuVizzIntegrationCompanyCode)

            'myWebReq.GetResponse.ToString()

            myWebReq = DirectCast(WebRequest.Create(strURL), HttpWebRequest)
            myWebReq.ContentType = "application/xml;"

            'myWebReq.ContentLength = data.Le
            myWebReq.Method = "POST"
            myWebReq.KeepAlive = True
            Dim autorization As String = My.Settings.nuVizzIntegrationUser + ":" + My.Settings.nuVizzIntegrationUserPassword
            Dim binaryAuthorization As Byte() = System.Text.Encoding.UTF8.GetBytes(autorization)
            autorization = Convert.ToBase64String(binaryAuthorization)
            autorization = "Basic " + autorization
            myWebReq.Headers.Add("AUTHORIZATION", autorization)


            If myWebReq.Proxy IsNot Nothing Then
                myWebReq.Proxy.Credentials = CredentialCache.DefaultCredentials
            End If
            Using myStream As Stream = myWebReq.GetRequestStream()

                'myStream = myWebReq.GetRequestStream()
                Dim data As Byte() = encoding.GetBytes(XMLTxt)
                If data.Length > 0 Then
                    myStream.Write(data, 0, data.Length)
                    myStream.Close()
                End If
                Try
                    myWebResp = DirectCast(myWebReq.GetResponse(), HttpWebResponse)
                    objStream = myWebResp.GetResponseStream()
                    sr = New StreamReader(objStream)
                    ResponceXML = sr.ReadToEnd()
                    'If myWebResp.StatusCode = HttpStatusCode.OK Then
                    '    ObjTask = JsonConvert.DeserializeObject(Of PinnaclePlus.onfleet.OnfleetTask)(JsonStr)
                    '    PinnaclePlus.SQLData.GeneralOperations.ExecuteNONQuery(String.Format("update T_Manifest_Order set of_id='{0}',of_error='',of_State={2} where MO_ID={1}", ObjTask.id, DrLocal.Item("MO_ID"), ObjTask.state), "", "")
                    'End If
                    'myWebResp.Close()
                    'myWebReq = Nothing
                Catch wex As WebException
                    'If wex.Response IsNot Nothing Then
                    '    myWebResp = DirectCast(wex.Response, HttpWebResponse)
                    '    objStream = myWebResp.GetResponseStream()
                    '    sr = New StreamReader(objStream)
                    '    JsonStr = sr.ReadToEnd()
                    '    Dim Objerr As PinnaclePlus.onfleet.ErrorResponse = JsonConvert.DeserializeObject(Of PinnaclePlus.onfleet.ErrorResponse)(JsonStr)
                    '    PinnaclePlus.SQLData.GeneralOperations.ExecuteNONQuery(String.Format("update T_Manifest_Order set of_error='{0}' where MO_ID={1}", Objerr.message.message, DrLocal.Item("MO_ID")), "", "")
                    'End If
                Catch ex As Exception

                End Try
            End Using
            System.Threading.Thread.Sleep(75)
        End Sub
        Public Shared Function GetBOLBase64(ByVal OrderNo As Integer, Source_ As Integer, MIN_ID As Integer, Seq As String) As String
            Dim retstr As String
            'Dim FileName As String
            'FileName = HttpContext.Current.Server.MapPath(String.Format("~/challan_Std/Fee_Challan_{0}_{1}.pdf", A_ID, ID_Code))

            Dim Rpt As New cr_bol
            'Dim path1 As String = My.Application.Info.DirectoryPath '' path
            '' for normal ADO Concept
            '' The current DataSource is an ADO.
            '' change path of the database
            Rpt.DataSourceConnections.Item(0). _
                    SetConnection(GetDBLoginFromConnectionString("Data Source"), GetDBLoginFromConnectionString("Initial Catalog"), False)
            '' if password is given then give the password
            '' if not give it will ask at runtime
            Rpt.DataSourceConnections.Item(0).SetLogon(GetDBLoginFromConnectionString("User ID"), GetDBLoginFromConnectionString("Password"))
            Rpt.SetParameterValue("P_MIN_ID", MIN_ID)
            Rpt.SetParameterValue("P_Seq", Seq)
            Rpt.RecordSelectionFormula = String.Format("{{V_Order.OrderNo}}='{0}' and {{V_Order.Source_}}={1}", OrderNo, Source_)


            If Rpt.HasSavedData Then
                Rpt.Refresh()
            End If

            Dim s As System.IO.MemoryStream = Rpt.ExportToStream(ExportFormatType.PortableDocFormat)
            retstr = Convert.ToBase64String(s.ToArray)
            s.Dispose()
            Rpt.Dispose()
            Return retstr
            'will generate in the app foot folder

        End Function
        Public Shared Function GetDLBase64(ByVal OrderNo As Integer, Source_ As Integer, MIN_ID As Integer, Seq As String) As String
            Dim retstr As String
            'Dim FileName As String
            'FileName = HttpContext.Current.Server.MapPath(String.Format("~/challan_Std/Fee_Challan_{0}_{1}.pdf", A_ID, ID_Code))

            Dim Rpt As New cr_Ticket
            'Dim path1 As String = My.Application.Info.DirectoryPath '' path
            '' for normal ADO Concept
            '' The current DataSource is an ADO.
            '' change path of the database
            Rpt.DataSourceConnections.Item(0). _
                    SetConnection(GetDBLoginFromConnectionString("Data Source"), GetDBLoginFromConnectionString("Initial Catalog"), False)
            '' if password is given then give the password
            '' if not give it will ask at runtime
            Rpt.DataSourceConnections.Item(0).SetLogon(GetDBLoginFromConnectionString("User ID"), GetDBLoginFromConnectionString("Password"))
            Rpt.SetParameterValue("P_MIN_ID", MIN_ID)
            Rpt.SetParameterValue("P_Seq", Seq)

            Rpt.RecordSelectionFormula = String.Format("{{V_Order.OrderNo}}='{0}' and {{V_Order.Source_}}={1}", OrderNo, Source_)


            If Rpt.HasSavedData Then
                Rpt.Refresh()
            End If

            Dim s As System.IO.MemoryStream = Rpt.ExportToStream(ExportFormatType.PortableDocFormat)
            retstr = Convert.ToBase64String(s.ToArray)
            s.Dispose()
            Rpt.Dispose()
            Return retstr
            'will generate in the app foot folder

        End Function
    End Class
End Namespace
