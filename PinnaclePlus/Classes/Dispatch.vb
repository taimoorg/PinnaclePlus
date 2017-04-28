Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Namespace PinnaclePlus.SQLData
    Public Class Dispatch
        Sub New()
        End Sub
        Public Shared Function P_Stops_Get(Date_ As DateTime, Hub As String, StartDate As DateTime) As DataTable
            Dim DS As New DataSet
            Dim objDatabase As Database
            Dim cmd As System.Data.Common.DbCommand
            objDatabase = DatabaseFactory.CreateDatabase()
            cmd = objDatabase.GetStoredProcCommand("P_Stops_Get", Date_, Hub, StartDate)
            cmd.CommandTimeout = 300
            objDatabase.LoadDataSet(cmd, DS, "tmp")
            'Return CType(objDatabase.ExecuteDataSet("P_Stops_Get", Date_, Hub, StartDate), DataSet).Tables(0)
            Return DS.Tables(0)
        End Function
        Public Shared Function P_Manifest_Stop_Order_GetByStopID(MS_ID As Integer) As DataTable
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            Return CType(objDatabase.ExecuteDataSet("P_Manifest_Stop_Order_GetByStopID", MS_ID), DataSet).Tables(0)
        End Function
        Public Shared Function P_Manifest_Stop_Order_GetByMIN_ID(MIN_ID As Integer) As DataTable
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            Return CType(objDatabase.ExecuteDataSet("P_Manifest_Stop_Order_GetByMIN_ID", MIN_ID), DataSet).Tables(0)
        End Function
        Public Shared Function P_Manifest_Stop_Order_GetByMIN_ID_UTC(MIN_ID As Integer) As DataTable
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            Return CType(objDatabase.ExecuteDataSet("P_Manifest_Stop_Order_GetByMIN_ID_UTC", MIN_ID), DataSet).Tables(0)
        End Function
        Public Shared Function P_Manifest_Stop_Order_Event_Doc_GetPending() As DataTable
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            Return CType(objDatabase.ExecuteDataSet("P_Manifest_Stop_Order_Event_Doc_GetPending"), DataSet).Tables(0)
        End Function
        Public Shared Function P_Manifest_IU(MIN_ID As Integer, Hub As String, DRIVER_ID As Integer, CO_DRIVER_ID1 As Object, CO_DRIVER_ID2 As Object, TRUCK_ID As Integer, StartDate As Date, ActualStartDate As Object, Running_Hours As Integer, Delivery_Time As Integer, TimeWindow As Integer, Notes As String, Is_Multiday As Boolean, Color As String, Det_Data As String) As Integer
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            If ActualStartDate = "0" Then
                ActualStartDate = DBNull.Value
            End If
            Return CType(objDatabase.ExecuteDataSet("P_Manifest_IU", MIN_ID, Hub, DRIVER_ID, IIf(CO_DRIVER_ID1 = 0, DBNull.Value, CO_DRIVER_ID1), IIf(CO_DRIVER_ID2 = 0, DBNull.Value, CO_DRIVER_ID2), TRUCK_ID, StartDate, ActualStartDate, Running_Hours, Delivery_Time, TimeWindow, Notes, Is_Multiday, Color, Det_Data), DataSet).Tables(0).Rows(0).Item(0)
        End Function
        Public Shared Function P_Address_IU(ADD_ID As Integer, Address As String, City As String, State As String, Zip As String, Lat As String, Lng As String, PlaceID As String) As Integer
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            Return CType(objDatabase.ExecuteDataSet("P_Address_IU", ADD_ID, Address, City, State, Zip, Lat, Lng, PlaceID), DataSet).Tables(0).Rows(0).Item(0)
        End Function
        Public Shared Function P_Manifest_Stop_Insert(StopStr As String, Seq_MS_ID As Integer, After As Boolean) As Integer
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            Return CType(objDatabase.ExecuteDataSet("P_Manifest_Stop_Insert", StopStr, Seq_MS_ID, After), DataSet).Tables(0).Rows(0).Item(0)
        End Function
        Public Shared Function P_Manifest_Lock(MIN_ID As Integer) As Boolean
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            Return CType(objDatabase.ExecuteDataSet("P_Manifest_Lock", MIN_ID), DataSet).Tables(0).Rows(0).Item(0)
        End Function
        Public Shared Sub P_Manifest_Stop_MoveDown(MS_ID As Integer)
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            objDatabase.ExecuteNonQuery("P_Manifest_Stop_MoveDown", MS_ID)
        End Sub
        Public Shared Sub P_Manifest_Stop_MoveUp(MS_ID As Integer)
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            objDatabase.ExecuteNonQuery("P_Manifest_Stop_MoveUp", MS_ID)
        End Sub
        Public Shared Sub P_Manifest_Stop_Del(MS_ID As Integer)
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            objDatabase.ExecuteNonQuery("P_Manifest_Stop_Del", MS_ID)
        End Sub
        Public Shared Sub P_Manifest_Stop_DayBreak(MS_ID As Integer)
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            objDatabase.ExecuteNonQuery("P_Manifest_Stop_DayBreak", MS_ID)
        End Sub
        Public Shared Sub P_Manifest_Del(MIN_ID As Integer)
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            objDatabase.ExecuteNonQuery("P_Manifest_Del", MIN_ID)
        End Sub
        Public Shared Sub P_UpdateTimeWindow(MIN_ID As Integer)
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            objDatabase.ExecuteNonQuery("P_UpdateTimeWindow", MIN_ID)
        End Sub
        Public Shared Sub P_Manifest_Update_DirectionDate(MIN_ID As Integer, Distance As Integer, Duration As Integer, Polyline As String)
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            objDatabase.ExecuteNonQuery("P_Manifest_Update_DirectionDate", MIN_ID, Distance, Duration, Polyline)
        End Sub
        Public Shared Sub P_Manifest_Stop_DirectionDate(MS_ID As Integer, Seq As Integer, Distance As Integer, Duration As Integer, PolyLine As String)
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            objDatabase.ExecuteNonQuery("P_Manifest_Stop_DirectionDate", MS_ID, Seq, Distance, Duration, PolyLine)
        End Sub
        Public Shared Sub P_Manifest_Update_nuVizz(MIN_ID As Integer, Status As Integer, Actual_Start_Time As Object, Actual_End_Time As Object, Actual_Driver As String, Actual_Driver_ID As String, Actual_Driver_Email As String)
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            objDatabase.ExecuteNonQuery("P_Manifest_Update_nuVizz", MIN_ID, Status, Actual_Start_Time, Actual_End_Time, Actual_Driver, Actual_Driver_ID, Actual_Driver_Email)
        End Sub
        Public Shared Sub P_Manifest_Event_IU(ME_ID As Integer, MIN_ID As Integer, EventId As Integer, EventName As String, ReportedDTTM As DateTime, Lat As String, Lng As String, XML_File As String)
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            objDatabase.ExecuteNonQuery("P_Manifest_Event_IU", ME_ID, MIN_ID, EventId, EventName, ReportedDTTM, Lat, Lng, XML_File)
        End Sub
        Public Shared Sub P_Manifest_Stop_Order_Survey_IU(MSO_ID As Integer, Attribute As String, Value As String, Description As String, XML_File As String)
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            objDatabase.ExecuteNonQuery("P_Manifest_Stop_Order_Survey_IU", MSO_ID, Attribute, Value, Description, XML_File)
        End Sub
        Public Shared Sub P_Manifest_Stop_Order_nuVizz(MSO_ID As Integer, DeliveryStartDTTM As Object, DeliveryEndDTTM As Object, ActualDuration As Object)
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            objDatabase.ExecuteNonQuery("P_Manifest_Stop_Order_nuVizz", MSO_ID, DeliveryStartDTTM, DeliveryEndDTTM, ActualDuration)
        End Sub
        Public Shared Sub P_Manifest_Stop_Order_Detail_IU(MSO_ID As Integer, BarCode As String, ConfirmedQuantity As Object, ExceptionCode As String, ExceptionText As String)
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            objDatabase.ExecuteNonQuery("P_Manifest_Stop_Order_Detail_IU", MSO_ID, BarCode, ConfirmedQuantity, ExceptionCode, ExceptionText)
        End Sub
        Public Shared Function P_Manifest_Stop_Order_Event_IU(MSOE_ID As Integer, MSO_ID As Integer, EventId As Integer, EventName As String, ReportedDTTM As DateTime, Lat As Object, Lng As Object, XML_File As String) As Integer
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            Return CType(objDatabase.ExecuteDataSet("P_Manifest_Stop_Order_Event_IU", MSOE_ID, MSO_ID, EventId, EventName, ReportedDTTM, Lat, Lng, XML_File), DataSet).Tables(0).Rows(0).Item(0)
        End Function
        Public Shared Function P_Manifest_Stop_Order_Event_Doc_IU(MSOED_ID As Integer, MSOE_ID As Integer, DocumentName As String, DocumentType As String, DocumentExtType As String, DocumentData As String, StopSigneeName As String, StopSigneeEmail As String) As Integer
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            Return CType(objDatabase.ExecuteDataSet("P_Manifest_Stop_Order_Event_Doc_IU", MSOED_ID, MSOE_ID, DocumentName, DocumentType, DocumentExtType, DocumentData, StopSigneeName, StopSigneeEmail), DataSet).Tables(0).Rows(0).Item(0)
        End Function
        Public Shared Function P_Manifest_Stop_Order_Exception_IU(MSOC_ID As Integer, MSO_ID As Integer, Exception_Type As String, Exception_Text As String, Comment_Type As String, Comments As String) As Integer
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            Return CType(objDatabase.ExecuteDataSet("P_Manifest_Stop_Order_Exception_IU", MSOC_ID, MSO_ID, Exception_Type, Exception_Text, Comment_Type, Comments), DataSet).Tables(0).Rows(0).Item(0)
        End Function
        Public Shared Sub P_Manifest_Stop_TimeWindow_Update(MS_ID As Integer, TimeFrame As Object, StartTime As Object, User_Value_Cascade As Object)
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            objDatabase.ExecuteNonQuery("P_Manifest_Stop_TimeWindow_Update", MS_ID, StartTime, TimeFrame, User_Value_Cascade)
        End Sub
        Public Shared Sub P_Manifest_Stop_Order_Update_Delivery_Time(MSO_ID As Integer, NewTime As Object)
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            objDatabase.ExecuteNonQuery("P_Manifest_Stop_Order_Update_Delivery_Time", MSO_ID, NewTime)
        End Sub
        Public Shared Sub P_Users_Report_Para_IU(USER_ID As String, RP_ID As Integer, Value_ As String)
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            objDatabase.ExecuteNonQuery("P_Users_Report_Para_IU", USER_ID, RP_ID, Value_)
        End Sub
        Public Shared Sub P_Users_Report_UseCount(USER_ID As String, REP_ID As Integer)
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            objDatabase.ExecuteNonQuery("P_Users_Report_UseCount", USER_ID, REP_ID)
        End Sub
        Public Shared Sub P_Add_Missing_Stop(MS_ID As Integer, Order_ID As Integer, Is_Pickup As Boolean)
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            objDatabase.ExecuteNonQuery("P_Add_Missing_Stop", MS_ID, Order_ID, Is_Pickup)
        End Sub
        Public Shared Sub P_Manifest_Stop_OrderUpSideDow(MIN_ID As Integer)
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            objDatabase.ExecuteNonQuery("P_Manifest_Stop_OrderUpSideDow", MIN_ID)
        End Sub
        Public Shared Sub P_Attach_Proof(MSOED_ID As Integer, OrderNo As String, Driver_ID As Integer, Hub As String, DateTime As DateTime, FileName As String, Path As String, is_pickup As Boolean, StopSigneeName As String)
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            objDatabase.ExecuteNonQuery("P_Attach_Proof", MSOED_ID, OrderNo, Driver_ID, Hub, DateTime, FileName, Path, is_pickup, StopSigneeName)
        End Sub
        Public Shared Sub P_Attach_Image(MSOED_ID As Integer, OrderNo As String, FileName As String, Path As String, Description As String, is_private As Boolean, DocType As String)
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            objDatabase.ExecuteNonQuery("P_Attach_Image", MSOED_ID, OrderNo, FileName, Path, Description, is_private, DocType)
        End Sub
        Public Shared Function P_OrderCompleted_WithException(startDate As Date, endDate As Date) As DataTable
            Dim objDatabase As Database
            Dim DS As New DataSet
            Dim cmd As System.Data.Common.DbCommand
            objDatabase = DatabaseFactory.CreateDatabase()
            cmd = objDatabase.GetStoredProcCommand("P_OrderCompleted_WithException", startDate, endDate)
            cmd.CommandTimeout = 30
            objDatabase.LoadDataSet(cmd, DS, "tmp")
            'Return CType(objDatabase.ExecuteDataSet("P_Stops_Get", Date_, Hub, StartDate), DataSet).Tables(0)
            Return DS.Tables(0)
        End Function
        Public Shared Function HasException_GeneratedAgainstManifest(StopNumber As Integer) As Boolean
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            Return CType(objDatabase.ExecuteDataSet("HasException_GeneratedAgainstManifest", StopNumber), DataSet).Tables(0).Rows(0).Item(0)
        End Function
        Public Shared Function P_ExceptionAgainst_Menifest(MSO_ID As Integer) As DataTable
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            Dim DS As New DataSet
            Dim cmd As System.Data.Common.DbCommand
            cmd = objDatabase.GetStoredProcCommand("P_ExceptionAgainst_Menifest", MSO_ID)
            cmd.CommandTimeout = 30
            objDatabase.LoadDataSet(cmd, DS, "tmp")
            Return DS.Tables(0)

            'Return CType(objDatabase.ExecuteDataSet("P_ExceptionAgainst_Menifest", MSO_ID), DataSet).Tables(0).Rows(0)
        End Function
        Public Shared Function P_DetailsAgainst_Menifest(MSO_ID As Integer) As DataTable
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            Dim DS As New DataSet
            Dim cmd As System.Data.Common.DbCommand
            cmd = objDatabase.GetStoredProcCommand("P_DetailsAgainst_Menifest", MSO_ID)
            cmd.CommandTimeout = 30
            objDatabase.LoadDataSet(cmd, DS, "tmp")
            Return DS.Tables(0)

            'Return CType(objDatabase.ExecuteDataSet("P_ExceptionAgainst_Menifest", MSO_ID), DataSet).Tables(0).Rows(0)
        End Function
    End Class

End Namespace
