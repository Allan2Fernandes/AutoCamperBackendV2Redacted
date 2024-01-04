using iTextSharp.text;

namespace AutoCamperBackendV2.Functions
{
    public interface PdfBuilder
    {
        public string CreatePDF(string Title, string InvoiceDate, string Recipient, string InvoiceContent);

        public void AddTitle(Document doc, string Title);

        public void AddInvoiceDate(Document doc, string InvoiceDate);

        public void AddRecipient(Document doc, string Recipient);

        public void AddInvoiceContent(Document doc, string InvoiceContent);
    }
}
