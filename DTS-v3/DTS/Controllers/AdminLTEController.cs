namespace DTS.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using DTS.Models;
    using iTextSharp.text.pdf;
    using iTextSharp.text.pdf.parser;

    public class AdminLTEController : Controller
    {
        string text = ""; // the variable contains all contents of the pdf file
        static SelectList selList, selFilenames;
        List<Search_Word> listWords;
        MyContext db;
        List<Search_Word> fnames;
        static string path;

        public AdminLTEController()
        {
            db = new MyContext();
            listWords = db.Search_Words.ToList();
            selList = new SelectList(listWords, "Id", "Word");
        }

        public ActionResult Index()
        {
            return View(); //review later for refactoring
        }

        public ActionResult _View()
        {
            return View(); //review later for refactoring
        }

        [HttpGet]
        public ActionResult SearchByWord() //renders the search page and a search button
        {
            List<string> names = new List<string>();
            string path = Server.MapPath("~/Uploaded_Files");
            string[] files_names = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            List<Search_Word> fnames = new List<Search_Word>();
            for (int i = 0; i < files_names.Length; i++)
            {
                var o = new Search_Word();
                o.FileName = files_names[i];
                fnames.Add(o);
                names.Add(System.IO.Path.GetFileName(files_names[i]));
            }
            selFilenames = new SelectList(fnames, "FileName", "FileName");
            if (files_names != null || files_names.Length != 0)
            {
                object[] obgs = new object[] { selFilenames, selList };
                ViewBag.DropDown = obgs;
            }
            ViewBag.Check = true; //bool variable is true to render the page
            return View();
        }

        [HttpPost]
        public ActionResult SearchByWord(Search_Word obj) // a method to click a button and enter word/s
        {
            object[] obgs = new object[] { selFilenames, selList };
            ViewBag.DropDown = obgs;
            bool check = false;
            string word = obj.CustomersWord; //the variable takes in the searched word/s

            if((obj.FileName != null) && obj.Word == null && obj.CustomersWord == null)
            {
                path = obj.FileName;
                return RedirectToAction("../AdminLTE/Pdf_Viewer");
            }

            if (word == null) // if drop-down list doesn't selected
            {
                if(obj.Word != null)
                {
                    var found = db.Search_Words.Find(int.Parse(obj.Word));
                    string selWord = found.Word;
                    if (obj.FileName != null)
                    {
                        text = GetPDFText(obj.FileName);
                        int count = (text.Length - text.Replace(selWord, "").Length) / selWord.Length;
                        if (count == 0) ViewBag.FoundText = "The search has resulted in no word/s mataches.";
                        ViewBag.FoundText = $"The search found word/s: '{selWord}' and it is present in the text {count} time/s.";
                    }
                    else // if file name doesnt selected
                    {
                        ViewBag.FoundText = $"Please select a file from a menu!";
                    }
                }
                else // if custumer word & drop-down word wont selected
                {
                    if (word == null) ViewBag.Check = check;
                    ViewBag.EmptyText = "Please input any word/s into the textbox and try again!";
                    return View();
                }           
            }
            else // if we want to search by input of word
            {             
                text = GetPDFText(); //transfers all pdf contents to text from GetPDFText method
                int count = (text.Length - text.Replace(word, "").Length) / word.Length;
                if (count == 0) ViewBag.FoundText = "The search has resulted in no word/s mataches.";
                ViewBag.FoundText = $"The search found word/s: '{word}' and it is present in the text {count} time/s."; 
            }

            return View();
        }

        string IgnoreUpperChar(string word)
        {
            return null; //permanently
        }

        /// <summary>
        /// View shown pdf content
        /// </summary>
        /// <returns> Action </returns>
        public ActionResult Pdf_Viewer() //shows the whole pdf file on one page
        {
            text = GetPDFText(path); //text grabs all file contents via GetPDFText method
            ViewBag.Text = text; //renders the file contents on the page
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"> The path pdf file </param>
        /// <returns> text of the pdf file </returns>
        string GetPDFText(string path = "~/Uploaded_Files/Altamont Inspection.pdf") //gets the contents of the uploaded pdf file via a path
        {
            if (path.Equals("~/Uploaded_Files/Altamont Inspection.pdf"))
            {
                string absolPath = System.IO.Path.Combine(Server.MapPath(path)); //the absolute path is received
                return PDFText(absolPath); //calls the main PDFText method
            }
            else
            {
                return PDFText(path);
            }
        }
    
        /// <summary>
        /// Using PdfReader extention
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string PDFText(string path) //the main method that retrieves the word we are searching for
        {
            PdfReader reader = new PdfReader(path);   // PdfReader is a service class from iText library
            string text = string.Empty;
            for (int page = 1; page <= reader.NumberOfPages; page++)
            {
                text = text +"\n\n" + PdfTextExtractor.GetTextFromPage(reader, page);
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