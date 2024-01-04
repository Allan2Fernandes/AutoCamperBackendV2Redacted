using AutoCamperBackend.Controllers;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Org.BouncyCastle.Cms;

namespace AutoCamperBackendV2.Functions
{
    public class PdfGenerator : PdfBuilder
    {
        public PdfGenerator() { }

        public string CreatePDF(string Title, string InvoiceDate, string Recipient, string InvoiceContent)
        {
            string FileName = string.Format("Invoice.pdf");
            FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.None);
            Document doc = new Document();
            using (PdfWriter writer = PdfWriter.GetInstance(doc, fs))
            {
                doc.Open();
                AddTitle(doc, Title);
                AddInvoiceDate(doc, InvoiceDate);
                AddRecipient(doc, Recipient);
                AddInvoiceContent(doc, InvoiceContent);
                doc.Close();
            }

            return FileName;

        }
        public void InsertCustomParagraphIntoDocument(Document doc, string text, Font.FontFamily fontFamily, BaseColor Color, int FontSize, int Alignment) //0 is left, 1 is center, 2 is right
        {
            Font CustomFont = new Font(fontFamily);
            CustomFont.Color = Color;
            CustomFont.Size = FontSize;
            Paragraph CustomParagraph = new Paragraph(text, CustomFont);
            CustomParagraph.Alignment = Alignment;
            doc.Add(CustomParagraph);
        }
        public void AddTitle(Document doc, string Title)
        {
            //string Title = string.Format("Invoice");
            InsertCustomParagraphIntoDocument(doc, Title, Font.FontFamily.COURIER, BaseColor.DARK_GRAY, 20, Element.ALIGN_CENTER);
        }
        public void AddInvoiceDate(Document doc, string InvoiceDate)
        {
            InsertCustomParagraphIntoDocument(doc, InvoiceDate, Font.FontFamily.COURIER, BaseColor.DARK_GRAY, 16, Element.ALIGN_RIGHT);
        }
        public void AddRecipient(Document doc, string Recipient) 
        {
            //string Recipient = string.Format("Congratulations " + " your Booking has been approved.");
            InsertCustomParagraphIntoDocument(doc, Recipient, Font.FontFamily.COURIER, BaseColor.DARK_GRAY, 16, Element.ALIGN_CENTER);
        }
        public void AddInvoiceContent(Document doc, string InvoiceContent)
        {
            //string Content = string.Format("\n Space Owner: " + "DB SPACE OWNER FIELD" + "\n Booking Owner: " + "\n Duration: " + "\n Net Price: ");
            InsertCustomParagraphIntoDocument(doc, InvoiceContent, Font.FontFamily.COURIER, BaseColor.DARK_GRAY, 14, Element.ALIGN_LEFT);

        }

    }
}
