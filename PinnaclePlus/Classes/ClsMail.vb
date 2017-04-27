Imports System.Net.Mail
Public Class ClsMail
    Sub New()

    End Sub
    Public Sub SendEmail(ByVal Msg As String, ByVal to_address As String, ByVal msg_Subject As String, Optional ByVal Attachments As ArrayList = Nothing)
        '  create the mail message
        Dim mail As MailMessage = New MailMessage()
        Dim st
        Dim ma As System.Net.Mail.Attachment
        'set the addresses

        'mail.From = New MailAddress("noreply@ist.edu.pk", "IST Online Admission Support")
        mail.From = New MailAddress("noreply@ist.edu.pk", "IST Islamabad")
        mail.ReplyToList.Add("admissions@ist.edu.pk")
        mail.To.Add(to_address)
        mail.Bcc.Add("mnaseer@ist.edu.pk")
        'set the content
        mail.Subject = msg_Subject
        mail.Body = Msg
        mail.IsBodyHtml = True
        'mail.IsBodyHtml = False

        If Not (Attachments Is Nothing) Then
            For Each st In Attachments
                ma = New System.Net.Mail.Attachment(st)
                mail.Attachments.Add(ma)
            Next
        End If

        'send the mas
        'SmtpClient smtp = new SmtpClient("smtp.mail.yahoo.com");
        Dim aCred As System.Net.NetworkCredential = New System.Net.NetworkCredential("noreply@ist.edu.pk", "abc!!123A11")
        'Dim aCred As System.Net.NetworkCredential = New System.Net.NetworkCredential("be_admissions@ist.edu.pk", "nccsbs22")

        'there  smtp mail hosting address for emicor

        Dim smtp As SmtpClient = New SmtpClient("203.124.44.20")
        'Dim smtp As SmtpClient = New SmtpClient("smtp.gmail.com")
        smtp.DeliveryMethod = SmtpDeliveryMethod.Network
        smtp.UseDefaultCredentials = False
        smtp.EnableSsl = False
        'smtp.EnableSsl = True
        smtp.Port = 25
        smtp.Credentials = aCred
        smtp.Send(mail) '  create the mail message
    End Sub
End Class
