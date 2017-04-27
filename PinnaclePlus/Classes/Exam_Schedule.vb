Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Namespace PinnaclePlus.SQLData
    Public Class Exam_Schedule
        Sub New()
        End Sub
        Public Shared Function P_Exam_Schedule_IU(ES_ID As Integer, LOC_ID As Integer, MOD_ID As Integer, S_Date As Object, Seats As Integer) As Integer
            Dim database As Database = DatabaseFactory.CreateDatabase()
            Dim DT As DataTable
            DT = CType(database.ExecuteDataSet("P_Exam_Schedule_IU", ES_ID, LOC_ID, MOD_ID, S_Date, Seats, PinnaclePlus.Security.CurrentUser.ID, PinnaclePlus.Security.CurrentUser.IP_Address), System.Data.DataSet).Tables(0)
            Return DT.Rows(0).Item(0)
        End Function
        Public Shared Function P_Module_Del(MOD_ID As Integer) As Integer
            Dim database As Database = DatabaseFactory.CreateDatabase()
            Dim DT As DataTable
            DT = CType(database.ExecuteDataSet("P_Module_Del", MOD_ID, PinnaclePlus.Security.CurrentUser.ID, PinnaclePlus.Security.CurrentUser.IP_Address), System.Data.DataSet).Tables(0)
            Return DT.Rows(0).Item(0)
        End Function
    End Class



End Namespace
