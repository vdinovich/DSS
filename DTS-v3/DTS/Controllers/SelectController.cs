using DTS.Models;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using System;

namespace DTS.Controllers
{
    public class SelectController : Controller
    {
        MyContext db = new MyContext();

        public ActionResult Select_Incidents()
        {
            bool flag;
            int id_loc = HomeController.Id_Location;  /// Get id location from Sign_In_Main page HomeControllers'
            IEnumerable<Critical_Incidents> list = db.Critical_Incidents.Where(l => l.Location == id_loc); // get list of records from Critical_Incidents(Location) 
            IEnumerable<CI_Category_Type> ll = db.CI_Category_Types;
            if (list.Count() == 0)
            {
                ViewBag.err = flag = false;
                ViewBag.emptyMsg = "The form - Critical Incidents is empty.. Please fill it out!";
                return View();
            }
            else
            {
                ViewBag.err = flag = true;
                Care_Community cc = db.Care_Communities.Find(id_loc);
                List<string> nms = new List<string>();
                List<Critical_Incidents> kk = list.ToList();
                for (int i = 0; i < kk.Count; i++)
                {
                    int id = kk[i].CI_Category_Type;
                    string name = db.CI_Category_Types.Find(id).Name;
                    nms.Add(name);
                    ViewBag.List1 = nms;
                }

                ViewBag.list = cc.Name;
                return View(list);
                Care_Community c = db.Care_Communities.Find(id_loc);
                ViewBag.list = c.Name;
                ViewBag.err = flag = true;
                return View(list);
            }
        }

        public ActionResult Select_Complaints()
        {
            bool flag;
            int id_loc = HomeController.Id_Location;
            IEnumerable<Complaint> list = db.Complaints.Where(l => l.Location == id_loc);
            if (list.Count() == 0)
            {
                ViewBag.err = flag = false;
                ViewBag.emptyMsg = "The form - Critical Incidents is empty.. Please fill it out!";
                return View();
            }
            else
            {
                ViewBag.err = flag = true;
                Care_Community cc = db.Care_Communities.Find(id_loc);
                ViewBag.list = cc.Name;
                return View(list);
            }
        }

        public ActionResult Select_Labour()
        {
            bool flag;
            int id_loc = HomeController.Id_Location;
            IEnumerable<Labour_Relations> list = db.Relations.Where(l => l.Location == id_loc);
            if (list.Count() == 0)
            {
                ViewBag.err = flag = false;
                ViewBag.emptyMsg = "The form - Labour Relations is empty.. Please fill it out!";
                return View();
            }
            else
            {
                ViewBag.err = flag = true;
                Care_Community cc = db.Care_Communities.Find(id_loc);

                ViewBag.list = cc.Name;
                return View(list);

                Care_Community c = db.Care_Communities.Find(id_loc);
                ViewBag.list = c.Name;
                ViewBag.err = flag = true;
                return View(list);
            }
        }

        public ActionResult Select_Community()
        {
            bool flag;
            int id_loc = HomeController.Id_Location;
            var list = db.Community_Risks.Where(l => l.Location == id_loc).ToList();
            if (list.Count() == 0)
            {
                ViewBag.err = flag = false;
                ViewBag.emptyMsg = "The form - Community Risks is empty.. Please fill it out!";
                return View();
            }
            else
            {
                ViewBag.err = flag = true;
                Care_Community cc = db.Care_Communities.Find(id_loc);

                ViewBag.list = cc.Name;
                return View(list);
            }
        }

        public ActionResult Select_Users()
        {
            var position = from user in db.Users
                           join posit in db.Positions on user.Position equals posit.Id
                           select new { user.Position, posit.Name };
            var care_community = from u in db.Users
                                 join care in db.Care_Communities on u.Care_Community equals care.Id
                                 select new { u.Care_Community, care.Name };

            List<string> list = new List<string>();
            foreach (var i in position)
                list.Add(i.Name);
            foreach (var i in care_community)
                list.Add(i.Name);

            ViewBag.pos_care = list;

            IEnumerable<Users> users = db.Users;
            return View(users);
        }

        public ActionResult Select_GoodNews()
        {
            bool flag;
            int id_loc = HomeController.Id_Location;
            IEnumerable<Good_News> list = db.Good_News.Where(l => l.Location == id_loc);
            ViewBag.err = flag = true;
            Care_Community cc = db.Care_Communities.Find(id_loc);
            ViewBag.list = cc.Name;
            return View(list);
            //if (list.Count() == 0)
            //{
            //    ViewBag.err = flag = false;
            //    ViewBag.emptyMsg = "The form - Good News is empty.. Please fill it out!";
            //    return View();
            //}
            //else
            //{
            //    ViewBag.err = flag = true;
            //    Care_Community cc = db.Care_Communities.Find(id_loc);
            //    ViewBag.list = cc.Name;
            //    return View(list);
            //}
        }

        public ActionResult Select_Agencies()
        {
            IEnumerable<Visits_Agency> list = db.Visits_Agencies;
            return View(list);
        }

        public ActionResult Select_WSIB()
        {

            bool flag;
            int id_loc = HomeController.Id_Location;
            var list = db.WSIBs.Where(l => l.Location == id_loc).ToList();
            if (list.Count() == 0)
            {
                ViewBag.err = flag = false;
                ViewBag.emptyMsg = "The form - WSIB is empty.. Please fill it out!";
                return View();
            }
            else
            {
                ViewBag.err = flag = true;
                Care_Community cc = db.Care_Communities.Find(id_loc);

                ViewBag.list = cc.Name;
                return View(list);

                Care_Community c = db.Care_Communities.Find(id_loc);
                ViewBag.list = c.Name;
                ViewBag.err = flag = true;
                return View(list);
            }
        }

        public ActionResult Select_Not_WSIB()
        {
            bool flag;
            int id_loc = HomeController.Id_Location;
            IEnumerable<Not_WSIBs> list = db.Not_WSIBs.Where(l => l.Location == id_loc);
            if (list.Count() == 0)
            {
                ViewBag.err = flag = false;
                ViewBag.emptyMsg = "The form - NOT WSIB is empty.. Please fill it out!";
                return View();
            }
            else
            {
                ViewBag.err = flag = true;
                Care_Community cc = db.Care_Communities.Find(id_loc);

                ViewBag.list = cc.Name;
                return View(list);

                Care_Community c = db.Care_Communities.Find(id_loc);
                ViewBag.list = c.Name;
                ViewBag.err = flag = true;
                return View(list);
            }
        }

        public ActionResult Select_Visits_Others()
        {

            bool flag;
            int id_loc = HomeController.Id_Location;
            var list = db.Visits_Others.Where(l => l.Location == id_loc).ToList();
            if (list.Count() == 0)
            {
                ViewBag.err = flag = false;
                ViewBag.emptyMsg = "The form - Visits by Others is empty.. Please fill it out!";
                return View();
            }
            else
            {
                ViewBag.err = flag = true;
                Care_Community cc = db.Care_Communities.Find(id_loc);

                ViewBag.list = cc.Name;
                return View(list);


            }
        }

        public ActionResult Outbreaks()
        {
   
            bool flag;
         int id_loc = HomeController.Id_Location;
            var list = db.Outbreaks.Where(l => l.Location == id_loc).ToList();
            if (list.Count() == 0)
            {
                ViewBag.err = flag = false;
                ViewBag.emptyMsg = "The form - Outbreakes is empty.. Please fill it out!";
                return View();
            }
            else
            {
                ViewBag.err = flag = true;
                Care_Community cc = db.Care_Communities.Find(id_loc);

                ViewBag.list = cc.Name;
           
                foreach(var it in list)
                {
                    if(it.Date_Concluded == DateTime.MinValue || it.Date_Declared == DateTime.MinValue)
                    {
                        if (it.Date_Concluded == DateTime.MinValue)
                            it.Date_Concluded = null;
                        else if(it.Date_Declared == DateTime.MinValue) it.Date_Declared = null;
                    }
                    else if(it.Date_Concluded == DateTime.MinValue && it.Date_Declared == DateTime.MinValue)
                    {
                        it.Date_Concluded = null;
                        it.Date_Declared = null;
                    }
                }
                return View(list);
            }
        }

        public ActionResult Select_Immunization()
        {
            bool flag;
            int id_loc = HomeController.Id_Location;
            IEnumerable<Immunization> list = db.Immunizations.Where(l => l.Location == id_loc);
            if (list.Count() == 0)
            {
                ViewBag.err = flag = false;
                ViewBag.emptyMsg = "The form - Immunization is empty.. Please fill it out!";
                return View();
            }
            else
            {
                ViewBag.err = flag = true;
                Care_Community cc = db.Care_Communities.Find(id_loc);

                ViewBag.list = cc.Name;
                return View(list);

         
            }
        }
        public ActionResult Privacy_Breaches()
        {

            bool flag;
            int id_loc = HomeController.Id_Location;
            IEnumerable<Privacy_Breaches> list = db.Privacy_Breaches.Where(l => l.Location == id_loc);
            if (list.Count() == 0)
            {
                ViewBag.err = flag = false;
                ViewBag.emptyMsg = "The form - Privacy Breaches is empty.. Please fill it out!";
                return View();
            }
            else
            {

                ViewBag.err = flag = true;
                Care_Community cc = db.Care_Communities.Find(id_loc);

                ViewBag.list = cc.Name;
                return View(list);

             
            }
        }

        public ActionResult Privacy_Complaints()
        {

            bool flag;
            int id_loc = HomeController.Id_Location;
            List<Privacy_Complaints> list = db.Privacy_Complaints.Where(l => l.Location == id_loc).ToList();
            if (list.Count() == 0)
            {
                ViewBag.err = flag = false;
                ViewBag.emptyMsg = "The form - Privacy Complaints is empty.. Please fill it out!";
                return View();
            }
            else
            {
                ViewBag.err = flag = true;
                Care_Community cc = db.Care_Communities.Find(id_loc);

                ViewBag.list = cc.Name;
                return View(list);

              
            }
        }

        public ActionResult Education_Select()
        {

            bool flag;
            int id_loc = HomeController.Id_Location;
            IEnumerable<Education> list = db.Educations.Where(l => l.Location == id_loc);
            if (list.Count() == 0)
            {
                ViewBag.err = flag = false;
                ViewBag.emptyMsg = "The form - Education is empty.. Please fill it out!";
                return View();
            }
            else
            {
                ViewBag.err = flag = true;
                Care_Community cc = db.Care_Communities.Find(id_loc);
                ViewBag.list = cc.Name;
                return View(list);
            }
        }
        public ActionResult Select_Emergency_Prep()
        {
            bool flag;
            int id_loc = HomeController.Id_Location;
            IEnumerable<Emergency_Prep> list = db.Emergency_Prep.Where(l => l.Location == id_loc);
            if (list.Count() == 0)
            {
                ViewBag.err = flag = false;
                ViewBag.emptyMsg = "The form - Education is empty.. Please fill it out!";
                return View();
            }
            else
            {
                ViewBag.err = flag = true;
                Care_Community cc = db.Care_Communities.Find(id_loc);
                ViewBag.list = cc.Name;
                return View(list);
            }
        }
    }
}
