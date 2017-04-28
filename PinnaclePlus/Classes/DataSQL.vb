Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Namespace PinnaclePlus.SQLData
    Public Class GeneralOperations
        Sub New()
        End Sub

        Public Shared Function ExecuteSelect(ByVal Query As String) As System.Data.DataTable
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            Return CType(objDatabase.ExecuteDataSet(System.Data.CommandType.Text, Query), System.Data.DataSet).Tables(0)
        End Function

        Public Shared Function ExecuteSelectPinnacle(ByVal Query As String) As System.Data.DataTable
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase("SqlStrPin")
            Dim dcCommand As System.Data.Common.DbCommand
            dcCommand = objDatabase.GetSqlStringCommand(Query)
            dcCommand.CommandTimeout = 2000
            Return CType(objDatabase.ExecuteDataSet(dcCommand), System.Data.DataSet).Tables(0)
        End Function
        Public Shared Function ExecuteNONQuery(ByVal Query As String, Log_Type As String, RecordStatus As String, Optional A_ID As Integer = 0) As Integer
            Dim objDatabase As Database
            Dim Ret As Integer
            objDatabase = DatabaseFactory.CreateDatabase()
            Ret = objDatabase.ExecuteNonQuery(System.Data.CommandType.Text, Query)
            If Log_Type <> "" Then
                P_Applicant_Log(A_ID, Log_Type, Query, RecordStatus)
            End If

            Return Ret
        End Function
        Public Shared Function ExecuteSelectSingleRow(ByVal Query As String) As System.Data.DataRow
            Dim objDatabase As Database
            Dim DT As DataTable
            objDatabase = DatabaseFactory.CreateDatabase()
            DT = CType(objDatabase.ExecuteDataSet(System.Data.CommandType.Text, Query), System.Data.DataSet).Tables(0)
            If DT.Rows.Count = 0 Then
                Return Nothing
            Else
                Return DT.Rows(0)
            End If
        End Function

        Public Shared Function ExecuteSelectSingleRowPinnacle(ByVal Query As String) As System.Data.DataRow
            Dim objDatabase As Database
            Dim DT As DataTable
            objDatabase = DatabaseFactory.CreateDatabase("SqlStrPin")
            DT = CType(objDatabase.ExecuteDataSet(System.Data.CommandType.Text, Query), System.Data.DataSet).Tables(0)
            If DT.Rows.Count = 0 Then
                Return Nothing
            Else
                Return DT.Rows(0)
            End If
        End Function

        Public Shared Function ExecuteSelectSingleValue(ByVal Query As String) As Object
            Dim DT As DataTable
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            DT = CType(objDatabase.ExecuteDataSet(System.Data.CommandType.Text, Query), System.Data.DataSet).Tables(0)
            If DT.Rows.Count = 0 Then
                Return Nothing
            Else
                Return DT.Rows(0).Item(0)
            End If
        End Function
        Public Shared Sub P_Applicant_Log(ByVal A_ID As Integer, Log_Type As String, ByVal L_Sql As String, Record_State As String)
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            If PinnaclePlus.Security.CurrentUser Is Nothing Then
                objDatabase.ExecuteNonQuery("P_Applicant_Log", A_ID, "admin", HttpContext.Current.Request.UserHostAddress, Log_Type, L_Sql, Record_State)
            Else
                objDatabase.ExecuteNonQuery("P_Applicant_Log", A_ID, PinnaclePlus.Security.CurrentUser.ID, HttpContext.Current.Request.UserHostAddress, Log_Type, L_Sql, Record_State)
            End If

        End Sub
        
        Public Shared Sub P_Applicant_Rec_Change(ByVal A_ID As Integer, ByVal Rec_PRO As Integer)
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            objDatabase.ExecuteNonQuery("P_Applicant_Rec_Change", A_ID, Rec_PRO)
        End Sub
        Public Shared Function GetRecordStatus(Query As String) As String
            Dim Str As String
            Dim DT As DataTable
            Str = ""
            Dim Col As DataColumn
            DT = ExecuteSelect(Query)
            If DT.Rows.Count = 0 Then
                Str = "No Row"
            Else
                For Each Col In DT.Columns
                    If IsDBNull(DT.Rows(0).Item(Col.ColumnName)) Then
                        Str = String.Format("{0}{1}={2} | ", Str, Col.ColumnName, "null")
                    Else
                        Str = String.Format("{0}{1}={2} | ", Str, Col.ColumnName, DT.Rows(0).Item(Col.ColumnName))
                    End If
                Next
            End If
            Return Str
        End Function
        Public Shared Function CreateAccount(Adm_Type As String, Email As String, Name As String, Cnic As String, Cell As String, QS_ID As String) As Integer
            Dim DT As DataTable
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            DT = CType(objDatabase.ExecuteDataSet("P_Applicant_Insert", Adm_Type, Email, Name, Cnic.Replace("-", ""), Cell, QS_ID, HttpContext.Current.Request.UserHostAddress), System.Data.DataSet).Tables(0)
            Return DT.Rows(0).Item("New_ID")
        End Function
        Public Shared Sub P_Applicant_Doc_Insert(A_ID As Integer)
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase()
            objDatabase.ExecuteNonQuery("P_Applicant_Doc_Insert", A_ID)
        End Sub
        Public Shared Sub Metro_AddOrderGeneralComment(OrderID As String, Comment As String)
            Dim objDatabase As Database
            objDatabase = DatabaseFactory.CreateDatabase("SqlStrPin")
            objDatabase.ExecuteNonQuery("Metro_AddOrderGeneralComment", OrderID, Comment, True, "1978", 10000, 1)
        End Sub
        'Farrukh Procedure for Report Rights
      
    End Class
End Namespace
