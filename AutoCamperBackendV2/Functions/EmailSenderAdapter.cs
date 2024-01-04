using System.Net;
using System.Net.Mail;

namespace AutoCamperBackendV2.Functions
{


    public class EmailSenderAdapter : IEmailSender
    {
        public void SendEmail(string recipientEmail, string emailSubject, string emailBody, string attachmentFilePath)
        {
            using (var message = new MailMessage("REDACTED", recipientEmail)
            {
                Subject = emailSubject,
                Body = emailBody,
                IsBodyHtml = true
            })
            {
                // Attach the PDF file
                if (!string.IsNullOrEmpty(attachmentFilePath))
                {
                    Attachment attachment = new Attachment(attachmentFilePath);
                    message.Attachments.Add(attachment);
                }

                var smtpClient = new SmtpClient("REDACTED")
                {
                    Port = 587,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    EnableSsl = true,
                    Credentials = new NetworkCredential("REDACTED", "REDACTED")
                };

                smtpClient.Send(message);
            }
        }
    }
}



