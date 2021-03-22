namespace DTS.Controllers
{
    using System;
    using System.Data;
    using System.IO;
    using System.Text;
    using System.Web.Mvc;
    using DTS.Models;
    using iTextSharp.text.pdf;
    using iTextSharp.text.pdf.parser;

    public class PdfClass
    {
        public string Text { get; set; } // Text contains the word is looking for
    }

    public class AdminLTEController : Controller
    {
        string text = ""; // text contains all of the pdf file

        public ActionResult Index()
        {
            return View(); //review later
        }

        public ActionResult _View()
        {
            return View(); //review later
        }

        [HttpGet]
        public ActionResult SearchByWord() //renders the search page and a button
        {
            ViewBag.Check = true;
            return View();
        }

        [HttpPost]
        public ActionResult SearchByWord(PdfClass obj) // by clicking a button, enter the code
        {
            bool check = false;
            string word = obj.Text;
            if (word == null)
            {
                if (word == null) ViewBag.Check = check;
                ViewBag.EmptyText = "Please input any word/s into the textbox and try again!";
                return View();
            }
            else
            {
                text = GetPDFText();
                int count = (text.Length - text.Replace(word, "").Length) / word.Length;
                ViewBag.FoundedText = $"The search found word - '{word}' and it is present {count} times."; 
            }

            return View();
        }

        public ActionResult Pdf_Viewer() //shows the whole pdf file on one page
        {
            text = GetPDFText();
            ViewBag.Text = text;
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"> The path pdf file </param>
        /// <returns> text of the pdf file </returns>
        string GetPDFText(string path = "~/Uploaded_Files/Altamont Inspection.pdf") //method that uploads a pdf file
        {
            string absolPath = System.IO.Path.Combine(Server.MapPath(path));
            return PDFText(absolPath); //calls the main PDFText method
        }
    
        /// <summary>
        /// Using PdfReader extention
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string PDFText(string path) //the main method that retrieves the word we are searching for
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