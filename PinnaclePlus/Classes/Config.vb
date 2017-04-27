Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Namespace gAdmin
    Public Class Config
        Sub New()
        End Sub
        Public Enum Config_Name
            Test_Setup_Time = 1000
            Test_Teardown_Time = 2000
            Test_Start_Time = 3000
            Test_End_Time = 4000
            Time_Interval = 5000

        End Enum

        Public Shared Function P_Config_Update(CONFIG_ID As Integer, C_Value As String) As Integer
            Dim database As Database = DatabaseFactory.CreateDatabase()
            Dim DT As DataTable
            DT = CType(database.ExecuteDataSet("P_Config_Update", CONFIG_ID, C_Value, PinnaclePlus.Security.CurrentUser.ID, PinnaclePlus.Security.CurrentUser.IP_Address), System.Data.DataSet).Tables(0)
            Return DT.Rows(0).Item(0)
        End Function

        Public Shared Function P_Config_Get(CONFIG_ID As Config_Name) As Object
            Dim database As Database = DatabaseFactory.CreateDatabase()
            Dim DT As DataTable
            DT = CType(database.ExecuteDataSet("P_Config_Get", CONFIG_ID), System.Data.DataSet).Tables(0)
            Return DT.Rows(0).Item(0)
        End Function

    End Class



End Namespace
