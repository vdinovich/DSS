namespace DTS.Controllers
{
    using DTS.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class StatisticsController : Controller
    {
        MyContext db;

        public StatisticsController() => db = new MyContext();

        public ActionResult Critical_Incidents()
        {
            var arr = TablesContainer.list1.Count;

            {
               ViewBag.Count = arr; 
            }

            {
                ViewBag.CI_Found = HomeController.strs;
            }
            //TablesContainer.c1 = TablesContainer.c2 = TablesContainer.c3 = TablesContainer.c4 = TablesContainer.c5 = TablesContainer.c6 = TablesContainer.c7 = TablesContainer.c8 =
            //    TablesContainer.c9 = TablesContainer.c10 = TablesContainer.c11 = TablesContainer.c12 = TablesContainer.c13 = TablesContainer.c14 = TablesContainer.c15 = 0;
            //var list = db.CI_Category_Types.ToList();
            //var arrNames = new List<string>();
            //foreach (var name in list)
            //    arrNames.Add(name.Name);
            //ViewBag.CI = arrNames;
            return View();
        }

        public ActionResult Complaint()
        {
            return View();
        }

        public ActionResult Good_News()
        {
            return View();
        }

        public ActionResult Emergency_Prep()
        {
            return View();
        }

        public ActionResult Community_Risks()
        {
            return View();
        }

        public ActionResult Visits_Others()
        {
            return View();
        }

        public ActionResult Privacy_Breaches()
        {
            return View();
        }

        public ActionResult Privacy_Complaints()
        {
            return View();
        }

        public ActionResult Education()
        {
            return View();
        }

        public ActionResult Labour_Relations()
        {
            return View();
        }

        public ActionResult Immunization()
        {
            return View();
        }

        public ActionResult Outbreaks()
        {
            return View();
        }

        public ActionResult WSIB()
        {
            return View();
        }

        public ActionResult Not_WSIBs()
        {
            return View();
        }
    }
}