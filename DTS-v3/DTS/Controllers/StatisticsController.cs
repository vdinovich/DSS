namespace DTS.Controllers
{
    using DTS.Models;
    using System.Web.Mvc;

    public class StatisticsController : Controller
    {
        public ActionResult Critical_Incidents()
        {
            var arr = TablesContainer.count_arr;
            TablesContainer.c1 = TablesContainer.c2 = TablesContainer.c3 = TablesContainer.c4 = TablesContainer.c5 = TablesContainer.c6 = TablesContainer.c7 = TablesContainer.c8 =
                TablesContainer.c9 = TablesContainer.c10 = TablesContainer.c11 = TablesContainer.c12 = TablesContainer.c13 = TablesContainer.c14 = TablesContainer.c15 = 0;
            return View(arr);
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