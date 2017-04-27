Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Namespace PinnaclePlus.SQLData
    Public Class Questions
        Sub New()
        End Sub
        Public Shared Function P_Question_IU(QUE_ID As Integer, MS_ID As Integer, Is_Active As Boolean, Q_Level As Integer, Q_Text As String, Q_Guid As String) As Integer
            Dim database As Database = DatabaseFactory.CreateDatabase()
            Dim DT As DataTable
            DT = CType(database.ExecuteDataSet("P_Question_IU", QUE_ID, MS_ID, Is_Active, Q_Level, Q_Text, Q_Guid, PinnaclePlus.Security.CurrentUser.ID, PinnaclePlus.Security.CurrentUser.IP_Address), System.Data.DataSet).Tables(0)
            Return DT.Rows(0).Item(0)

        End Function
        Public Shared Sub P_Question_Choice_Del_If_Exists(QC_ID As Integer)
            Dim database As Database = DatabaseFactory.CreateDatabase()
            database.ExecuteNonQuery("P_Question_Choice_Del_If_Exists", QC_ID, PinnaclePlus.Security.CurrentUser.ID, PinnaclePlus.Security.CurrentUser.IP_Address)
        End Sub
        Public Shared Function P_Question_Del(QUE_ID As Integer) As Integer
            Dim database As Database = DatabaseFactory.CreateDatabase()
            Dim DT As DataTable
            DT = CType(database.ExecuteDataSet("P_Question_Del", QUE_ID, PinnaclePlus.Security.CurrentUser.ID, PinnaclePlus.Security.CurrentUser.IP_Address), System.Data.DataSet).Tables(0)
            Return DT.Rows(0).Item(0)
        End Function
        Public Shared Sub P_Question_Choice_Mark_Correct(QC_ID As Integer)
            Dim database As Database = DatabaseFactory.CreateDatabase()
            database.ExecuteNonQuery("P_Question_Choice_Mark_Correct", QC_ID, PinnaclePlus.Security.CurrentUser.ID, PinnaclePlus.Security.CurrentUser.IP_Address)
        End Sub
        Public Shared Function P_Question_Choice_IU(QC_ID As Integer, QUE_ID As Integer, C_Text As String, C_Order As Integer) As Integer
            Dim database As Database = DatabaseFactory.CreateDatabase()
            Dim DT As DataTable
            DT = CType(database.ExecuteDataSet("P_Question_Choice_IU", QC_ID, QUE_ID, C_Text, C_Order, PinnaclePlus.Security.CurrentUser.ID, PinnaclePlus.Security.CurrentUser.IP_Address), System.Data.DataSet).Tables(0)
            Return DT.Rows(0).Item(0)
        End Function

    End Class

End Namespace
