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
            IEnumerable<Users> list = db.Users;
            return View(list);
        }

        public ActionResult Select_GoodNews()
        {
            IEnumerable<Good_News> list = db.Good_News;
            return View(list);
        }        
    }
}
