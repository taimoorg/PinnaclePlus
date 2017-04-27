Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Namespace PinnaclePlus.SQLData
    Public Class Locations
        Sub New()
        End Sub
        Public Shared Function P_Location_IU(LOC_ID As Integer, Name As String, address As String, Contacts As String) As Integer
            Dim database As Database = DatabaseFactory.CreateDatabase()
            Dim DT As DataTable
            DT = CType(database.ExecuteDataSet("P_Location_IU", LOC_ID, Name, address, Contacts, PinnaclePlus.Security.CurrentUser.ID, PinnaclePlus.Security.CurrentUser.IP_Address), System.Data.DataSet).Tables(0)
            Return DT.Rows(0).Item(0)
        End Function
        Public Shared Function P_Location_Del(LOC_ID As Integer) As Integer
            Dim database As Database = DatabaseFactory.CreateDatabase()
            Dim DT As DataTable
            DT = CType(database.ExecuteDataSet("P_Location_Del", LOC_ID, PinnaclePlus.Security.CurrentUser.ID, PinnaclePlus.Security.CurrentUser.IP_Address), System.Data.DataSet).Tables(0)
            Return DT.Rows(0).Item(0)
        End Function
    End Class



End Namespace
