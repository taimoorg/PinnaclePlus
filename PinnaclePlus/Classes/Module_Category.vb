Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Namespace PinnaclePlus.SQLData
    Public Class Module_Category
        Sub New()
        End Sub
        Public Shared Function P_Module_Category_IU(MC_ID As Integer, MOD_ID As Integer, SC_ID As Integer, Mcq_Count As Integer, Test_Time As Integer, Essay_Count As Object, Essay_Time As Object, Tot_Marks As Object) As Integer
            Dim database As Database = DatabaseFactory.CreateDatabase()
            Dim DT As DataTable
            DT = CType(database.ExecuteDataSet("P_Module_Category_IU", MC_ID, MOD_ID, SC_ID, Mcq_Count, Test_Time, Essay_Count, Essay_Time, Tot_Marks, PinnaclePlus.Security.CurrentUser.ID, PinnaclePlus.Security.CurrentUser.IP_Address), System.Data.DataSet).Tables(0)
            Return DT.Rows(0).Item(0)
        End Function
        Public Shared Function P_Module_Category_Del(MC_ID As Integer) As Integer
            Dim database As Database = DatabaseFactory.CreateDatabase()
            Dim DT As DataTable
            DT = CType(database.ExecuteDataSet("P_Module_Category_Del", MC_ID, PinnaclePlus.Security.CurrentUser.ID, PinnaclePlus.Security.CurrentUser.IP_Address), System.Data.DataSet).Tables(0)
            Return DT.Rows(0).Item(0)
        End Function
    End Class



End Namespace
