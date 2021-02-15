using DTS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DTS.Controllers
{
    public class HomeController : Controller
    {
        MyContext db = new MyContext();
        public static string msg_infos { get; set; } 
        public static int id_care_center { get; set; }
        List<Care_Community> communities;
        List<Position> positions;
        SelectList list2, list;
        List<object> both;

        public HomeController()
        {
            communities = db.Care_Communities.ToList();
            list = new SelectList(communities, "Id", "Name");

            positions = db.Positions.ToList();
            list2 = new SelectList(positions, "Id", "Name");
        }

        [HttpGet]
        public ActionResult SignIN()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignIN(Users user)
        {
            string login = user.Login;
            string password = user.Password;
            List<Users> users = db.Users.ToList();
            bool flag = false;
            for (int i = 0; i < users.Count(); i++)
            {
                if (users[i].Login == login && users[i].Password == password)
                {
                    flag = true;
                    return RedirectToAction("../Home/Index");
                }
            }
            if (!flag) { ViewBag.incorrect = "Incorrect Login or Password..Try again!"; }
            return View();
        }

        [HttpGet]
        public ActionResult Index()
        {
            both = new List<object> { list, list2 };
            ViewBag.listing = both;
            return View();
        }

        [HttpPost]
        public ActionResult Index(Sign_in_Main main)
        {
            if(main.Id == 0)
            {
                if (main.Care_Community_Centre == null) return RedirectToAction("Index");
                id_care_center = int.Parse(main.Care_Community_Centre);
                return RedirectToAction("WOR_Tabs");
            }
            both = new List<object> { list, list2 };
            main.Care_Community_Centre = list.Where(p => p.Value == main.Care_Community_Centre).First().Text;
            main.Position = list2.Where(p => p.Value == main.Position).First().Text;
            main.Date_Entred = DateTime.Now;
            db.Sign_in_Mains.Add(main);
            db.SaveChanges();

            main.Care_Community_Centre = null;
            main.Position = null;
            main.Week = 0;
            main.User_Name = null;
            ViewBag.listing = both;
            return RedirectToAction("Index");
        }

        public ActionResult WOR_Tabs()
        {
            ViewBag.id = id_care_center;
            return View();
        }

        [HttpGet]
        public ActionResult Insert()
        {
            ViewBag.locations = list;
            return View();
        }

        [HttpPost]
        public ActionResult Insert(Critical_Incidents entity)
        {
            if (entity.Brief_Description == null && entity.Care_Plan_Updated == null && entity.CIS_Initiated == null && entity.CI_Category_Type == null &&
               entity.CI_Form_Number == null && entity.Date == null && entity.File_Complete == null && entity.Follow_Up_Amendments == null &&
               entity.Location == null && entity.MOHLTC_Follow_Up == null && entity.MOH_Notified == null && entity.POAS_Notified == null && entity.Police_Notified == null &&
               entity.Quality_Improvement_Actions == null && entity.Risk_Locked == null)
            {
                ViewBag.Empty = "All fields have to be filled..";
                return View();
            }
            else if (entity.Brief_Description == null || entity.Care_Plan_Updated == null || entity.CIS_Initiated == null || entity.CI_Category_Type == null ||
              entity.CI_Form_Number == null || entity.Date == null || entity.File_Complete == null || entity.Follow_Up_Amendments == null ||
              entity.Location == null || entity.MOHLTC_Follow_Up == null || entity.MOH_Notified == null || entity.POAS_Notified == null || entity.Police_Notified == null ||
              entity.Quality_Improvement_Actions == null || entity.Risk_Locked == null)
            {
                ViewBag.Empty = "Some one field is Empty.. Try to Fill out it, and try again!";
                return View();
            }
            else
            {
                try
                {
                    //db.Critical_Incidents.Add(entity);
                    //db.SaveChanges();
                    msg_infos = ADO_NET_CRUD.Insert_Incident(entity);
                    return RedirectToAction("../Select/Select_Incidents");
                }
                catch(Exception ex) { return HttpNotFound(ex.Message); }
            }
        }

        [HttpGet]
        public ActionResult Labour_Insert()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Labour_Insert(Labour_Relations entity)
        {
            db.Relations.Add(entity);
            db.SaveChanges();
            return RedirectToAction("../Select/Select_Labour");
        }

        [HttpGet]
        public ActionResult Community_Insert()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Community_Insert(Community_Risks entity)
        {
            db.Community_Risks.Add(entity);
            db.SaveChanges();
            return RedirectToAction("../Select/Select_Community");
        }

        [HttpGet]
        public ActionResult GoodNews_Insert()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GoodNews_Insert(Good_News entity)
        {
            db.Good_News.Add(entity);
            db.SaveChanges();
            return RedirectToAction("../Select/Select_GoodNews");
        }

        [HttpGet]
        public ActionResult Agency_Insert()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Agency_Insert(Visits_Agency entity)
        {
            db.Visits_Agencies.Add(entity);
            db.SaveChanges();
            return RedirectToAction("../Select/Select_Agencies");
        }

        [HttpGet]
        public ActionResult WSIB()
        {
            return View();
        }

        [HttpPost]
        public ActionResult WSIB(WSIB entity)
        {
            db.WSIBs.Add(entity);
            db.SaveChanges();
            return RedirectToAction("../Select/Select_WSIB");

        }

        [HttpGet]
        public ActionResult Not_WSIB()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Not_WSIB(Not_WSIBs entity)
        {
            db.Not_WSIBs.Add(entity);
            db.SaveChanges();
            return RedirectToAction("../Select/Select_Not_WSIB");

        }

        [HttpGet]
        public ActionResult Care_Community()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Care_Community(Care_Community entity)
        {
            if (ModelState.IsValid)
            {
                db.Care_Communities.Add(entity);
                db.SaveChanges();
                return RedirectToAction("../Home/Index");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Add_Position()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add_Position(Position entity)
        {
            if (ModelState.IsValid)
            {
                db.Positions.Add(entity);
                db.SaveChanges();
                return RedirectToAction("../Home/Index");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Visits_Others()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Visits_Others(Visits_Others entity)
        {
            db.Visits_Others.Add(entity);
            db.SaveChanges();
            return RedirectToAction("../Select/Select_Visits_Others");
        }

        [HttpGet]
        public ActionResult Outbreaks()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Outbreaks(Outbreaks entity)
        {
            db.Outbreaks.Add(entity);
            db.SaveChanges();
            return RedirectToAction("../Home/WOR_Tabs");
            //return RedirectToAction("../Select/Select_Outbreaks");
        }

    }
}