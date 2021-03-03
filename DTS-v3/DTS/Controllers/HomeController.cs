using DTS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DTS.Controllers
{
    public class HomeController : Controller
    {
        MyContext db = new MyContext();
        static string notsel;
        public static string path { get; set; }
        public static string msg_infos { get; set; }
        public static int id_care_center { get; set; }
        public static int id_labor_relation { get; set; }
        public static int id_community_risk { get; set; }
        public static int id_complaints { get; set; }
        public static int id_goodNews { get; set; }
        public static string success_nsg = string.Empty;
        public static int id_visit_order{ get; set; }
        public static int id_outbrakes { get; set; }
        public static int id_wsib { get; set; }
        List<CI_Category_Type> categories;
        List<Care_Community> communities;
        List<Position> positions;
        SelectList list2, list, list3;
        List<object> both;

        public HomeController()
        {
            communities = db.Care_Communities.ToList();
            list = new SelectList(communities, "Id", "Name");

            positions = db.Positions.ToList();
            list2 = new SelectList(positions, "Id", "Name");

            categories = db.CI_Category_Types.ToList();
            list3 = new SelectList(categories, "Id", "Name");
        }

        public ActionResult Complaint_Insert()
        {
            ViewBag.locations = list;
            return View();
        }

        [HttpPost]
        public ActionResult Complaint_Insert(Complaint c)
        {
            if (c != null)
            {
                id_complaints = c.Location;
                db.Complaints.Add(c);
                int res = db.SaveChanges();
                if (res == 1)
                    success_nsg = "Your record was successfully saved!";
                else
                    success_nsg = "Something went wrong...";
                return RedirectToAction("../Select/Select_Complaints");
            }
            return View();
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
                if (main.Care_Community_Centre == null)
                {
                    notsel = "You have to select a Care Community from a drop-down list";
                    ViewBag.not_selected = notsel;
                    return RedirectToAction("Index");
                }
                id_care_center = id_complaints = 
                id_visit_order = id_outbrakes = id_wsib = int.Parse(main.Care_Community_Centre);

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

        [HttpPost]
        public ActionResult Uploded(HttpPostedFileBase file)
        {
            if (file != null)
            {
                try
                {
                    if (file.ContentLength > 0)
                    {
                        string _FileName = Path.GetFileName(file.FileName);
                        path = Path.Combine(Server.MapPath("~/Uploaded_Files"), _FileName);
                        file.SaveAs(path);
                    }
                    ViewBag.Message = "File Uploaded Successfully!!";
                    return RedirectToAction("Index");
                }
                catch
                {
                    ViewBag.Message = "File upload failed!!";
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult AllFiles()
        {
            List<string> names = new List<string>();
            path = Server.MapPath("~/Uploaded_Files");
            string[] files_names = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            for (int i = 0; i < files_names.Length; i++)
                names.Add(Path.GetFileName(files_names[i]));

            return View(names);
        }

        [HttpPost]
        public ActionResult AllFiles(string item)
        {
            return View();
        }

        public ActionResult WOR_Tabs()
        {
            ViewBag.id = id_care_center;
            ViewBag.idComplaints = id_complaints;
            return View();
        }

        [HttpGet]
        public ActionResult Insert()
        {
            object[] objs = new object[] { list, list3 };
            ViewBag.locations = objs;
            return View();
        }

        [HttpPost]
        public ActionResult Insert(Critical_Incidents entity)
        {
            if (entity.Brief_Description == null && entity.Care_Plan_Updated == null && entity.CIS_Initiated == null && entity.CI_Category_Type == 0 &&
               entity.CI_Form_Number == null && entity.Date == null && entity.File_Complete == null && entity.Follow_Up_Amendments == null &&
               entity.Location == null && entity.MOHLTC_Follow_Up == null && entity.MOH_Notified == null && entity.POAS_Notified == null && entity.Police_Notified == null &&
               entity.Quality_Improvement_Actions == null && entity.Risk_Locked == null)
            {
                ViewBag.Empty = "All fields have to be filled.";
                return View();
            }
            else if (entity.Brief_Description == null || entity.Care_Plan_Updated == null || entity.CIS_Initiated == null || entity.CI_Category_Type == 0 ||
              entity.CI_Form_Number == null || entity.Date == null || entity.File_Complete == null || entity.Follow_Up_Amendments == null ||
              entity.Location == null || entity.MOHLTC_Follow_Up == null || entity.MOH_Notified == null || entity.POAS_Notified == null || entity.Police_Notified == null ||
              entity.Quality_Improvement_Actions == null || entity.Risk_Locked == null)
            {
                ViewBag.Empty = "Some fields are empty. Please fill it out and try again!";
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
            ViewBag.locations = list;
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
            ViewBag.locations = list;
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
            ViewBag.locations = list;
            return View();
        }

        [HttpPost]
        public ActionResult GoodNews_Insert(Good_News entity)
        {
            if (entity != null)
            {
                id_goodNews = entity.Location;
                db.Good_News.Add(entity);
                int res = db.SaveChanges();
                if (res == 1)
                    success_nsg = "Your record was successfully saved!";
                else
                    success_nsg = "Somthing went wrong...";
                return RedirectToAction("../Select/Select_GoodNews");
            }
            return View();
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
            ViewBag.locations = list;
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
            ViewBag.locations = list;
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
            ViewBag.locations = list;
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
            ViewBag.locations = list;
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

        public ActionResult Immunization_Insert()
        {
            ViewBag.locations = list;
            return View();
        }

        [HttpGet]
        public ActionResult Privacy_Breaches()
        {
            ViewBag.locations = list;
            return View();
        }

        [HttpPost]
        public ActionResult Privacy_Breaches(Privacy_Breaches entity)
        {
            db.Privacy_Breaches.Add(entity);
            db.SaveChanges();
            return RedirectToAction("../Select/Privacy_Breaches");
        }

        [HttpGet]
        public ActionResult Privacy_Complaints()
        {
            ViewBag.locations = list;
            return View();
        }

        [HttpPost]
        public ActionResult Privacy_Complaints(Privacy_Complaints entity)
        {
            db.Privacy_Complaints.Add(entity);
            db.SaveChanges();
            return RedirectToAction("../Select/Privacy_Complaints");
        }

        [HttpGet]
        public ActionResult Education_Insert()
        {
            ViewBag.locations = list;
            return View();
        }

        [HttpPost]
        public ActionResult Education_Insert(Education entity)
        {
            db.Education.Add(entity);
            db.SaveChanges();
            return RedirectToAction("../Select/Education_Select");
        }
    }
}