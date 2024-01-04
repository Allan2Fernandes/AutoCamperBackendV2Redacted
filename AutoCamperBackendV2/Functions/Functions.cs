using System.Text;
using System;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Net;

namespace AutoCamperBackendV2.Functions
{
    public class Functions
    {

        public static string EncryptPassword(string RawPassword)
        {
            // Create a new instance of SHA-256
            using (SHA256 sha256 = SHA256.Create())
            {
                // Convert the input string to bytes
                byte[] inputBytes = Encoding.UTF8.GetBytes(RawPassword);

                // Compute the hash
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                // Convert the hash bytes to a hexadecimal string
                string hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                return hashString;
            }
        }

        public static bool IsListempty<T>(List<T> ListToTest)
        {
            return ListToTest.Count == 0;
        }

        //public static void SendEmail(string RecipientEmail, string EmailSubject, string EmailBody, string attachmentFilePath)
        //{
        //    using (var message = new MailMessage("parkinpeace@gmail.com", RecipientEmail)
        //    {
        //        Subject = EmailSubject,
        //        Body = EmailBody,
        //        IsBodyHtml = true // Set this to true to indicate that the body contains HTML content
        //    })
        //    {
        //        // Attach the PDF file
        //        if (!string.IsNullOrEmpty(attachmentFilePath))
        //        {
        //            Attachment attachment = new Attachment(attachmentFilePath);
        //            message.Attachments.Add(attachment);
        //        }

        //        var smtpClient = new SmtpClient("smtp.gmail.com")
        //        {
        //            Port = 587,
        //            DeliveryMethod = SmtpDeliveryMethod.Network,
        //            UseDefaultCredentials = false,
        //            EnableSsl = true,
        //            Credentials = new NetworkCredential("parkinpeace@gmail.com", "efxp jled xhip inkg")
        //        };

        //        smtpClient.Send(message);
        //    }
        //}


    }
}
