Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Namespace PinnaclePlus.SQLData
    Public Class Reports
        Sub New()
        End Sub

        Public Shared Function P_Report_IU(REP_ID As Integer, Name As String, Query As String, Des As String, IsPP As Boolean) As Integer
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            Return CType(objDatabase.ExecuteDataSet("P_Report_IU", REP_ID, Name, Query, Des, IsPP), DataSet).Tables(0).Rows(0).Item(0)
        End Function
        Public Shared Sub P_Users_Report_ID(User_ID As String, REP_ID As Integer, Has As Boolean)
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            objDatabase.ExecuteNonQuery("P_Users_Report_ID", User_ID, REP_ID, Has)
        End Sub
        Public Shared Sub P_Report_Para_IU(RP_ID As Integer, REP_ID As Integer, Name As String, Description As String, Order_ As Integer, Width As Integer, Rows_ As Object, Para_Type As Integer)

            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            objDatabase.ExecuteNonQuery("P_Report_Para_IU", RP_ID, REP_ID, Name, Description, Order_, Width, Rows_, Para_Type)
        End Sub
        Public Shared Sub P_Report_Para_Del(RP_ID As Integer)

            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            objDatabase.ExecuteNonQuery("P_Report_Para_Del", RP_ID)
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

        Public Shared Function P_Report_AutoComplete(SearchText As String) As DataTable
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            Return CType(objDatabase.ExecuteDataSet("P_Report_AutoComplete", SearchText), DataSet).Tables(0)
        End Function
    End Class
End Namespace
