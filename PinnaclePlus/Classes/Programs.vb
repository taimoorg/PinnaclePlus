Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Namespace PinnaclePlus.SQLData
    Public Class Programs
        Sub New()
        End Sub
        Public Shared Function P_Program_IU(PRO_ID As Integer, DEPT_ID As Integer, Offered As Boolean, Adm_Type As String, Name As String, Small_Name As String, Sections As Integer) As Integer
            Dim database As Database = DatabaseFactory.CreateDatabase()
            Dim DT As DataTable
            DT = CType(database.ExecuteDataSet("P_Program_IU", PRO_ID, DEPT_ID, Offered, Adm_Type, Name, Small_Name, Sections, PinnaclePlus.Security.CurrentUser.ID, PinnaclePlus.Security.CurrentUser.IP_Address), System.Data.DataSet).Tables(0)
            Return DT.Rows(0).Item(0)
        End Function
        Public Shared Function P_Program_Del(PRO_ID As Integer) As Integer
            Dim database As Database = DatabaseFactory.CreateDatabase()
            Dim DT As DataTable
            DT = CType(database.ExecuteDataSet("P_Program_Del", PRO_ID, PinnaclePlus.Security.CurrentUser.ID, PinnaclePlus.Security.CurrentUser.IP_Address), System.Data.DataSet).Tables(0)
            Return DT.Rows(0).Item(0)
        End Function
        Public Shared Function P_Program_ToggleOffered(PRO_ID As Integer) As Integer
            Dim database As Database = DatabaseFactory.CreateDatabase()
            Dim DT As DataTable
            DT = CType(database.ExecuteDataSet("P_Program_ToggleOffered", PRO_ID, PinnaclePlus.Security.CurrentUser.ID, PinnaclePlus.Security.CurrentUser.IP_Address), System.Data.DataSet).Tables(0)
            Return DT.Rows(0).Item(0)
        End Function
        Public Shared Sub P_Program_Eligibility_Toggle(PRO_ID As Integer, QS_ID As Integer)
            Dim database As Database = DatabaseFactory.CreateDatabase()

            database.ExecuteNonQuery("P_Program_Eligibility_Toggle", PRO_ID, QS_ID, PinnaclePlus.Security.CurrentUser.ID, PinnaclePlus.Security.CurrentUser.IP_Address)

        End Sub


    End Class



End Namespace
