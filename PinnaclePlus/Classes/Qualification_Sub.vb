Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Namespace PinnaclePlus.SQLData
    Public Class Qualification_Sub
        Sub New()
        End Sub
        Public Shared Function P_Qualification_Sub_IU(QS_ID As Integer, QUA_ID As Integer, Name As String) As Integer
            Dim database As Database = DatabaseFactory.CreateDatabase()
            Dim DT As DataTable
            DT = CType(database.ExecuteDataSet("P_Qualification_Sub_IU", QS_ID, QUA_ID, Name, PinnaclePlus.Security.CurrentUser.ID, PinnaclePlus.Security.CurrentUser.IP_Address), System.Data.DataSet).Tables(0)
            Return DT.Rows(0).Item(0)
        End Function
        Public Shared Function P_Qualification_Sub_Del(QS_ID As Integer) As Integer
            Dim database As Database = DatabaseFactory.CreateDatabase()
            Dim DT As DataTable
            DT = CType(database.ExecuteDataSet("P_Qualification_Sub_Del", QS_ID, PinnaclePlus.Security.CurrentUser.ID, PinnaclePlus.Security.CurrentUser.IP_Address), System.Data.DataSet).Tables(0)
            Return DT.Rows(0).Item(0)
        End Function
    End Class



End Namespace
