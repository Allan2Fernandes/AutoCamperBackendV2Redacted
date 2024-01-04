namespace AutoCamperBackendV2.Functions
{
    public interface IEmailSender
    {
        void SendEmail(string recipientEmail, string emailSubject, string emailBody, string attachmentFilePath);
    }
}
