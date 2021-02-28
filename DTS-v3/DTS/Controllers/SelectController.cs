using DTS.Models;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

namespace DTS.Controllers
{
    public class SelectController : Controller
    {
        MyContext db = new MyContext();

        public ActionResult Select_Incidents()
        {
            string info = HomeController.msg_infos, msg2 =  HomeController.success_nsg;
            bool flag;
            int id_loc = HomeController.id_care_center;
            IEnumerable<Critical_Incidents> list = db.Critical_Incidents.Where(l => l.Location == id_loc);
            IEnumerable<CI_Category_Type> ll = db.CI_Category_Types;
            if(list.Count() == 0)
            {
                ViewBag.err = flag = false;
                ViewBag.emptyMsg = "The form - Critical Incidents is empty.. Please fill it out!";
                return View();
            }
            else
            {
                if (info != null || info != "" || info != string.Empty)
                {
                    ViewBag.info_insert = info;
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
                }
                Care_Community c = db.Care_Communities.Find(id_loc);
                ViewBag.list = c.Name;
                ViewBag.err = flag = true;
                return View(list);
            }
        }

        public ActionResult Select_Complaints()
        {
            string info = HomeController.success_nsg;
            bool flag;
            int id_loc = HomeController.id_complaints;
            IEnumerable<Complaint> list = db.Complaints.Where(l => l.Location == id_loc);
            if (list.Count() == 0)
            {
                ViewBag.err = flag = false;
                ViewBag.emptyMsg = "The form - Critical Incidents is empty.. Please fill it out!";
                return View();
            }
            else
            {
                if (info != null || info != "" || info != string.Empty)
                {
                    ViewBag.info_insert = info;
                    ViewBag.err = flag = true;
                    Care_Community cc = db.Care_Communities.Find(id_loc);
                    ViewBag.list = cc.Name;
                    return View(list);
                }
                Care_Community c = db.Care_Communities.Find(id_loc);
                ViewBag.list = c.Name;
                ViewBag.err = flag = true;
                return View();
            }
        }

        public ActionResult Select_Labour()
        {
            string info = HomeController.msg_infos, msg2 = HomeController.success_nsg;
            bool flag;
            int id_loc = HomeController.id_outbrakes;
            IEnumerable<Labour_Relations> list = db.Relations.Where(l => l.Location == id_loc);
            if (list.Count() == 0)
            {
                ViewBag.err = flag = false;
                ViewBag.emptyMsg = "The form - Labour Relations is empty.. Please fill it out!";
                return View();
            }
            else
            {
                if (info != null || info != "" || info != string.Empty)
                {
                    ViewBag.info_insert = info;
                    ViewBag.err = flag = true;
                    Care_Community cc = db.Care_Communities.Find(id_loc);

                    ViewBag.list = cc.Name;
                    return View(list);
                }
                Care_Community c = db.Care_Communities.Find(id_loc);
                ViewBag.list = c.Name;
                ViewBag.err = flag = true;
                return View(list);
            }
        }

        public ActionResult Select_Community()
        {
            IEnumerable<Community_Risks> list = db.Community_Risks;
            return View(list);
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

            IEnumerable<Users>  users = db.Users;
            return View(users);
        }

        public ActionResult Select_GoodNews()
        {
            string  msg2 = HomeController.success_nsg;
            bool flag;
            int id_loc = HomeController.id_care_center;
            IEnumerable<Good_News> list = db.Good_News.Where(l => l.Location == id_loc);
            if (list.Count() == 0)
            {
                ViewBag.err = flag = false;
                ViewBag.emptyMsg = "The form - Good News is empty.. Please fill it out!";
                return View();
            }
            else
            {
                if (msg2 != null || msg2 != "" || msg2 != string.Empty)
                {
                    ViewBag.info_insert = msg2;
                    ViewBag.err = flag = true;
                    Care_Community cc = db.Care_Communities.Find(id_loc);
                    ViewBag.list = cc.Name;
                    return View(list);
                }
                Care_Community c = db.Care_Communities.Find(id_loc);
                ViewBag.list = c.Name;
            }
            return View();
        }        

        public ActionResult Select_Agencies()
        {
            IEnumerable<Visits_Agency> list = db.Visits_Agencies;
            return View(list);
        }

        public ActionResult Select_WSIB()
        {
            string info = HomeController.msg_infos, msg2 = HomeController.success_nsg;
            bool flag;
            int id_loc = HomeController.id_wsib;
            IEnumerable<WSIB> list = db.WSIBs.Where(l => l.Location == id_loc);
            if (list.Count() == 0)
            {
                ViewBag.err = flag = false;
                ViewBag.emptyMsg = "The form - WSIB is empty.. Please fill it out!";
                return View();
            }
            else
            {
                if (info != null || info != "" || info != string.Empty)
                {
                    ViewBag.info_insert = info;
                    ViewBag.err = flag = true;
                    Care_Community cc = db.Care_Communities.Find(id_loc);

                    ViewBag.list = cc.Name;
                    return View(list);
                }
                Care_Community c = db.Care_Communities.Find(id_loc);
                ViewBag.list = c.Name;
                ViewBag.err = flag = true;
                return View(list);
            }
        }

        public ActionResult Select_Not_WSIB()
        {
            string info = HomeController.msg_infos, msg2 = HomeController.success_nsg;
            bool flag;
            int id_loc = HomeController.id_wsib;
            IEnumerable<Not_WSIBs> list = db.Not_WSIBs.Where(l => l.Location == id_loc);
            if (list.Count() == 0)
            {
                ViewBag.err = flag = false;
                ViewBag.emptyMsg = "The form - NOT WSIB is empty.. Please fill it out!";
                return View();
            }
            else
            {
                if (info != null || info != "" || info != string.Empty)
                {
                    ViewBag.info_insert = info;
                    ViewBag.err = flag = true;
                    Care_Community cc = db.Care_Communities.Find(id_loc);

                    ViewBag.list = cc.Name;
                    return View(list);
                }
                Care_Community c = db.Care_Communities.Find(id_loc);
                ViewBag.list = c.Name;
                ViewBag.err = flag = true;
                return View(list);
            }
        }

        public ActionResult Select_Visits_Others()
        {
            string info = HomeController.msg_infos, msg2 = HomeController.success_nsg;
            bool flag;
            int id_loc = HomeController.id_visit_order;
            IEnumerable<Visits_Others> list = db.Visits_Others.Where(l => l.Location == id_loc);
            if (list.Count() == 0)
            {
                ViewBag.err = flag = false;
                ViewBag.emptyMsg = "The form - Visits by Others is empty.. Please fill it out!";
                return View();
            }
            else
            {
                if (info != null || info != "" || info != string.Empty)
                {
                    ViewBag.info_insert = info;
                    ViewBag.err = flag = true;
                    Care_Community cc = db.Care_Communities.Find(id_loc);

                    ViewBag.list = cc.Name;
                    return View(list);
                }
                Care_Community c = db.Care_Communities.Find(id_loc);
                ViewBag.list = c.Name;
                ViewBag.err = flag = true;
                return View(list);
            }
        }

        public ActionResult Outbreaks()
        {
            string info = HomeController.msg_infos, msg2 = HomeController.success_nsg;
            bool flag;
            int id_loc = HomeController.id_outbrakes;
            IEnumerable<Outbreaks> list = db.Outbreaks.Where(l => l.Location == id_loc);
            if (list.Count() == 0)
            {
                ViewBag.err = flag = false;
                ViewBag.emptyMsg = "The form - Outbreakes is empty.. Please fill it out!";
                return View();
            }
            else
            {
                if (info != null || info != "" || info != string.Empty)
                {
                    ViewBag.info_insert = info;
                    ViewBag.err = flag = true;
                    Care_Community cc = db.Care_Communities.Find(id_loc);

                    ViewBag.list = cc.Name;
                    return View(list);
                }
                Care_Community c = db.Care_Communities.Find(id_loc);
                ViewBag.list = c.Name;
                ViewBag.err = flag = true;
                return View(list);
            }
        }

        public ActionResult Select_Immunization()
        {
            string info = HomeController.msg_infos, msg2 = HomeController.success_nsg;
            bool flag;
            int id_loc = HomeController.id_outbrakes;
            IEnumerable<Immunization> list = db.Immunizations.Where(l => l.Location == id_loc);
            if (list.Count() == 0)
            {
                ViewBag.err = flag = false;
                ViewBag.emptyMsg = "The form - Immunization is empty.. Please fill it out!";
                return View();
            }
            else
            {
                if (info != null || info != "" || info != string.Empty)
                {
                    ViewBag.info_insert = info;
                    ViewBag.err = flag = true;
                    Care_Community cc = db.Care_Communities.Find(id_loc);

                    ViewBag.list = cc.Name;
                    return View(list);
                }
                Care_Community c = db.Care_Communities.Find(id_loc);
                ViewBag.list = c.Name;
                ViewBag.err = flag = true;
                return View(list);
            }
        }
    }
}
