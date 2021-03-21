namespace DTS.Controllers
{
    using System;
    using System.Data;
    using System.IO;
    using System.Text;
    using System.Web.Mvc;
    using iTextSharp.text.pdf;
    using iTextSharp.text.pdf.parser;

    public class PdfClass
    {
        public string Text { get; set; }
    }

    public class AdminLTEController : Controller
    {
        string text = "";

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _View()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SearchByWord()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SearchByWord(PdfClass word)
        {
            return View();
        }

        public ActionResult Pdf_Viewer()
        {
            string absolPath = System.IO.Path.Combine(Server.MapPath("~/Uploaded_Files/Altamont Inspection.pdf"));
            text = PDFText(absolPath);

            ViewBag.Text = text;
            return View();
        }
    
        public string PDFText(string path)
        {
            PdfReader reader = new PdfReader(path);
            string text = string.Empty;
            for (int page = 1; page <= reader.NumberOfPages; page++)
            {
                text += PdfTextExtractor.GetTextFromPage(reader, page);
            }
            reader.Close();
            return text;
        }

        //public static string ReadPdfFile(object Filename, DataTable ReadLibray)
        //{
        //    PdfReader reader2 = new PdfReader((string)Filename);
        //    string strText = string.Empty;

        //    for (int page = 1; page <= reader2.NumberOfPages; page++)
        //    {
        //        ITextExtractionStrategy its = new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy();
        //        PdfReader reader = new PdfReader((string)Filename);
        //        String s = PdfTextExtractor.GetTextFromPage(reader, page, its);

        //        s = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(s)));
        //        strText = strText + s;
        //        reader.Close();
        //    }
        //    return strText;
        //}
    }
}