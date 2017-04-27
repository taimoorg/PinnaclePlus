Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Namespace PinnaclePlus
    Public Class OrderBatch
        Sub New()
        End Sub
        

        Public Shared Sub P_Batch_Orders_Update(BO_ID As Integer, Address1 As String, Zip As String, Lati As String, Longi As String)

            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            objDatabase.ExecuteNonQuery("P_Batch_Orders_Update", BO_ID, Address1.ToLower.Trim, Zip, Lati, Longi)
        End Sub
        Public Shared Sub P_Pull_Sheet_Insert(OrderNo As Integer, Barcode As String, Description As String, LocationCode As String, Location As String, Warehouse As String, Status_ As String)
            '
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            objDatabase.ExecuteNonQuery("P_Pull_Sheet_Insert", OrderNo, Barcode, Description, LocationCode, Location, Warehouse, Status_)
        End Sub

        Public Shared Function P_Order_Get_ByDate(ByVal Date_ As Date, ByVal Hub As String, Refresh As Boolean) As DataTable
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            Return CType(objDatabase.ExecuteDataSet("P_Order_Get_ByDate", Date_, Hub, Refresh), System.Data.DataSet).Tables(0)
        End Function
        Public Shared Function P_Order_Get_ByUnAttached_Count(ByVal Date_ As Date, ByVal Hub As String) As Integer
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            Return CType(objDatabase.ExecuteDataSet("P_Order_Get_ByUnAttached_Count", Date_, Hub), System.Data.DataSet).Tables(0).Rows(0).Item(0)
        End Function

        Public Shared Function P_Batch_Orders_Get_ByDate_Exported(ByVal Date_ As Date, ByVal Hub As String) As DataTable
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            Return CType(objDatabase.ExecuteDataSet("P_Batch_Orders_Get_ByDate_Exported", Date_, Hub), System.Data.DataSet).Tables(0)
        End Function
        Public Shared Function P_Batch_Orders_Get_ByDate_FirstUnassigned(ByVal Date_ As Date, ByVal Hub As String) As DataRow
            Dim DT As DataTable
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            DT = CType(objDatabase.ExecuteDataSet("P_Batch_Orders_Get_ByDate_FirstUnassigned", Date_, Hub), System.Data.DataSet).Tables(0)
            If DT.Rows.Count = 0 Then
                Return Nothing
            Else
                Return DT.Rows(0)
            End If
        End Function
        Public Shared Function P_Batch_Orders_Get_ByDate_UnassignedCount(ByVal Date_ As Date, ByVal Hub As String) As Integer
            Dim DT As DataTable
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            DT = CType(objDatabase.ExecuteDataSet("P_Batch_Orders_Get_ByDate_UnassignedCount", Date_, Hub), System.Data.DataSet).Tables(0)
            If DT.Rows.Count = 0 Then
                Return 0
            Else
                Return DT.Rows(0).Item(0)
            End If
        End Function

        Public Shared Function P_Batch_Orders_Get_ByDate_NotExported(ByVal Date_ As Date, ByVal Hub As String) As DataTable
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            Return CType(objDatabase.ExecuteDataSet("P_Batch_Orders_Get_ByDate_NotExported", Date_, Hub), System.Data.DataSet).Tables(0)
        End Function
        Public Shared Function P_Batch_Orders_Get_ByDate(ByVal Date_ As Date, ByVal Hub As String) As DataTable
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            Return CType(objDatabase.ExecuteDataSet("P_Batch_Orders_Get_ByDate", Date_, Hub), System.Data.DataSet).Tables(0)
        End Function
        Public Shared Function P_Get_Orders_by_Date(ByVal Date_ As Date, ByVal Hub As String) As DataTable
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase("SqlStrPin")
            Return CType(objDatabase.ExecuteDataSet("P_Get_Orders_by_Date", Date_, Hub), System.Data.DataSet).Tables(0)
        End Function
        Public Shared Function T_Manifest_Order_Del(MO_ID As Integer) As Integer
            Dim database As Database = DatabaseFactory.CreateDatabase()
            Dim DT As DataTable
            DT = CType(database.ExecuteDataSet("T_Manifest_Order_Del", MO_ID, PinnaclePlus.Security.CurrentUser.ID, PinnaclePlus.Security.CurrentUser.IP_Address), System.Data.DataSet).Tables(0)
            Return DT.Rows(0).Item(0)
        End Function
        Public Shared Sub P_Manifest_Clear_Stops(MO_ID As Integer)
            Dim database As Database = DatabaseFactory.CreateDatabase()
            database.ExecuteNonQuery("P_Manifest_Clear_Stops", MO_ID)
        End Sub

        Public Shared Sub P_Manifest_Create(ByVal Date_ As Date, ByVal Hub As String)
            Dim database As Database = DatabaseFactory.CreateDatabase()
            database.ExecuteNonQuery("P_Manifest_Create", Date_, Hub)
        End Sub
        Public Shared Sub P_Order_Sync(ByVal Date_ As Date, ByVal Hub As String)
            Dim database As Database = DatabaseFactory.CreateDatabase()


            Dim dcCommand As System.Data.Common.DbCommand
            dcCommand = database.GetStoredProcCommand("P_Order_Sync", Date_, Hub)
            dcCommand.CommandTimeout = 200
            database.ExecuteDataSet(dcCommand)
        End Sub
    End Class



End Namespace
