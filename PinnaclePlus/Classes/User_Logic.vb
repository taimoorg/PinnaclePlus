Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports System.Security.Cryptography
Imports System.Web.Security
Imports System.IO

Namespace PinnaclePlus

    Public Class Security
        Public Shared Function EncriptURL(stringToEncrypt As String, encryptionKey As String) As String
            Dim key() As Byte = {}
            Dim iv() As Byte = {&H8, &H7, &H6, &H5, &H1, &H2, &H3, &H4}
            Dim inputByteArray() As Byte
            Dim outputByteArray() As Byte

            Try

                key = Encoding.UTF8.GetBytes(encryptionKey.Substring(0, 8))
                Dim des As DESCryptoServiceProvider = New DESCryptoServiceProvider()
                inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt)
                Dim MS As MemoryStream = New MemoryStream()
                Dim cs As CryptoStream = New CryptoStream(MS, des.CreateEncryptor(key, iv), CryptoStreamMode.Write)
                cs.Write(inputByteArray, 0, inputByteArray.Length)
                cs.FlushFinalBlock()
                outputByteArray = MS.ToArray()
                Dim retstr As String = Bytes_To_String2(outputByteArray)
                Return retstr

            Catch

                Return (String.Empty)

            End Try
        End Function
        Private Shared Function Bytes_To_String2(ByVal bytes_Input As Byte()) As String
            Dim strTemp As New StringBuilder(bytes_Input.Length * 2)
            For Each b As Byte In bytes_Input
                strTemp.Append(Conversion.Hex(b))
            Next
            Return strTemp.ToString()
        End Function
        Public Shared Function EncodePassword(passFormat As Byte, passtext As String, passwordSalt As String) As String

            If (passFormat.Equals(0)) Then ' passwordFormat="Clear" (0)
                Return passtext
            Else
                Dim Key() As Byte
                Dim IV() As Byte
                Dim bytePASS() As Byte = Encoding.Unicode.GetBytes(passtext)
                Dim byteSALT() As Byte = Convert.FromBase64String(passwordSalt)
                Dim Lenarr = (byteSALT.Length + bytePASS.Length) - 1
                Dim byteRESULT(Lenarr) As Byte

                System.Buffer.BlockCopy(byteSALT, 0, byteRESULT, 0, byteSALT.Length)
                System.Buffer.BlockCopy(bytePASS, 0, byteRESULT, byteSALT.Length, bytePASS.Length)

                Dim tdes As New TripleDESCryptoServiceProvider()
                tdes.KeySize = 128
                Key = UTF8Encoding.UTF8.GetBytes("0123456789ABCDEF")
                IV = UTF8Encoding.UTF8.GetBytes("ABCDEFGH")
                tdes.Key = Key
                tdes.IV = IV
                Dim cTransform As ICryptoTransform = tdes.CreateEncryptor()
                Dim resultArray() As Byte = cTransform.TransformFinalBlock(byteRESULT, 0, byteRESULT.Length)
                Return Convert.ToBase64String(resultArray, 0, resultArray.Length)
            End If
        End Function



        Public Shared Function DecodePassword(Password As String, PW_Salt As String) As String
            Dim PasswordDecode() As Byte
            Dim Salt() As Byte
            Dim RetVal
            Dim tdes As New TripleDESCryptoServiceProvider()
            tdes.KeySize = 128
            tdes.Key = UTF8Encoding.UTF8.GetBytes("0123456789ABCDEF")
            tdes.IV = UTF8Encoding.UTF8.GetBytes("ABCDEFGH")
            Dim toEncryptArray() As Byte = Convert.FromBase64String(Password)
            Dim cTransform1 As ICryptoTransform = tdes.CreateDecryptor()
            PasswordDecode = cTransform1.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length)
            Salt = Convert.FromBase64String(PW_Salt)
            RetVal = Encoding.Unicode.GetString(PasswordDecode).Remove(0, Encoding.Unicode.GetString(Salt).Length)
            Return RetVal
        End Function
        Public Shared Function GetUserByID(ID As String) As Admin_User
            Dim T As New Admin_User
            Dim DR As DataRow
            DR = PinnaclePlus.SQLData.GeneralOperations.ExecuteSelectSingleRow(String.Format("select * from T_Users where USER_ID='{0}'", ID.ToUpper))
            If DR Is Nothing Then
                Return Nothing
            Else
                With T
                    .ID = ID
                    .S_KEY = DR.Item("S_Key")
                    .Full_Name = DR.Item("Full_Name")
                    .Is_Admin = DR.Item("Is_Admin")
                    .Is_Blocked = DR.Item("Is_Blocked")
                    .IP_Address = HttpContext.Current.Request.UserHostAddress
                End With
                Return T
            End If
        End Function
        Public Shared Sub P_Users_Page_Option_IU(PO_ID As Integer, USER_ID As String)
            Dim database As Database = DatabaseFactory.CreateDatabase()
            database.ExecuteNonQuery("P_Users_Page_Option_IU", PO_ID, USER_ID, CurrentUser.ID, CurrentUser.IP_Address)
        End Sub
        Public Shared Sub P_Users_IU(USER_ID As String, Full_Name As String, Is_Admin As Boolean)
            Dim database As Database = DatabaseFactory.CreateDatabase()
            database.ExecuteNonQuery("P_Users_IU", USER_ID, Full_Name, Is_Admin, CurrentUser.ID, CurrentUser.IP_Address)
        End Sub
        Public Shared Function P_Users_Del(USER_ID As String) As Integer
            Dim database As Database = DatabaseFactory.CreateDatabase()
            Dim DT As DataTable
            DT = CType(database.ExecuteDataSet("P_Users_Del", USER_ID, CurrentUser.ID, CurrentUser.IP_Address), DataSet).Tables(0)
            Return DT.Rows(0).Item(0)
        End Function

        Public Shared Sub P_Users_Page_Option_Del(PO_ID As Integer, USER_ID As String)
            Dim database As Database = DatabaseFactory.CreateDatabase()
            database.ExecuteNonQuery("P_Users_Page_Option_Del", PO_ID, USER_ID, CurrentUser.ID, CurrentUser.IP_Address)
        End Sub
        Public Shared Sub P_User_Hub_Toggle(HUB_ID As String, USERID As String)
            Dim database As Database = DatabaseFactory.CreateDatabase()
            database.ExecuteNonQuery("P_User_Hub_Toggle", HUB_ID, USERID, CurrentUser.ID, CurrentUser.IP_Address)
        End Sub

        Public Shared Function P_Page_Group_By_User(User_ID As String) As DataTable
            Dim database As Database = DatabaseFactory.CreateDatabase()
            Return CType(database.ExecuteDataSet("P_Page_Group_By_User", User_ID), System.Data.DataSet).Tables(0)

        End Function
        Public Shared Function P_Page_Get_All_Sibling(User_ID As String, url As String) As DataTable
            Dim database As Database = DatabaseFactory.CreateDatabase()
            url = url.Replace("/", "")
            Return CType(database.ExecuteDataSet("P_Page_Get_All_Sibling", User_ID, url), System.Data.DataSet).Tables(0)

        End Function

        Public Shared Function P_Has_Rights(PO_ID As Integer) As Boolean
            Dim database As Database = DatabaseFactory.CreateDatabase()
            Return CType(database.ExecuteDataSet("P_Has_Rights", CurrentUser.ID, PO_ID), System.Data.DataSet).Tables(0).Rows(0).Item(0)
        End Function

        Public Shared ReadOnly Property CurrentUser() As Admin_User

            Get
                Return HttpContext.Current.Session("CurrentUserAdmin")
            End Get

        End Property
    End Class
End Namespace
