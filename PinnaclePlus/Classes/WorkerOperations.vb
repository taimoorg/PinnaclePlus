Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Namespace PinnaclePlus
    Public Class WorkerOperations
        Sub New()
        End Sub
        
        'Public Shared Sub P_Teams_IU(TEAM_ID As String, HUB_ID As Object, Name As String)

        '    Dim objDatabase As Database
        '    objDatabase = DatabaseFactory.CreateDatabase()

        '    objDatabase.ExecuteNonQuery("P_Teams_IU", TEAM_ID, HUB_ID, Name)
        'End Sub
        Public Shared Function P_Worker_IU(First_Name As String, Last_Name As String, Phone As String, Hub As String) As Integer
            Dim WORKER_ID As String
            Dim DT As DataTable
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            DT = CType(objDatabase.ExecuteDataSet("P_Worker_IU", First_Name, Last_Name, Phone, Hub), DataSet).Tables(0)
            If DT.Rows.Count = 0 Then
                WORKER_ID = 0
            Else
                WORKER_ID = DT.Rows(0).Item(0)
            End If
            Return WORKER_ID
        End Function
        'Public Shared Sub P_Worker_Team_Delete(Worker_ID As String)

        '    Dim objDatabase As Database
        '    objDatabase = DatabaseFactory.CreateDatabase()
        '    objDatabase.ExecuteNonQuery("P_Worker_Team_Delete", Worker_ID)
        'End Sub
        'Public Shared Sub P_Worker_Team_IU(Worker_ID As String, Team_ID As String)

        '    Dim objDatabase As Database
        '    objDatabase = DatabaseFactory.CreateDatabase()
        '    objDatabase.ExecuteNonQuery("P_Worker_Team_IU", Worker_ID, Team_ID)
        'End Sub

    End Class



End Namespace
