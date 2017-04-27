Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Namespace PinnaclePlus.SQLData
    Public Class Module_Category_Level
        Sub New()
        End Sub
        Public Shared Sub P_Module_Category_Level_IU(MS_ID As Integer, SC_ID As Integer, Q_Level As Integer, Q_Per As Integer)
            Dim database As Database = DatabaseFactory.CreateDatabase()
            database.ExecuteNonQuery("P_Module_Category_Level_IU", MS_ID, SC_ID, Q_Level, Q_Per, PinnaclePlus.Security.CurrentUser.ID, PinnaclePlus.Security.CurrentUser.IP_Address)
        End Sub
        Public Shared Sub P_Module_Category_Level_Del_If_Exists(MS_ID As Integer, SC_ID As Integer)
            Dim database As Database = DatabaseFactory.CreateDatabase()
            database.ExecuteNonQuery("P_Module_Category_Level_Del_If_Exists", MS_ID, SC_ID, PinnaclePlus.Security.CurrentUser.ID, PinnaclePlus.Security.CurrentUser.IP_Address)
        End Sub
    End Class

End Namespace
