using DTS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DTS.Controllers
{
    public class SelectController : Controller
    {
        MyContext db = new MyContext();
        public ActionResult Select_Incidents()
        {
            IEnumerable<Critical_Incidents> list = db.Critical_Incidents;
            return View(list);
        }

        public ActionResult Select_Labour()
        {
            IEnumerable<Labour_Relations> list = db.Relations;
            return View(list);
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
            IEnumerable<Good_News> list = db.Good_News;
            return View(list);
        }        
    }
}
