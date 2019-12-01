using DTS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DTS.Controllers
{
    public class HomeController : Controller
    {
        MyContext db = new MyContext();

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
            for (int i = 0; i< users.Count(); i++)
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

        public ActionResult Index()
        {
            List<Care_Community> communities = db.Care_Communities.ToList();
            SelectList list = new SelectList(communities, "Id", "Name");

            List<Position> positions = db.Positions.ToList();
            SelectList list2 = new SelectList(positions, "Id", "Name");
            List<object> both = new List<object> { list, list2 };
            ViewBag.listing = both;
            return View();
        }

        public ActionResult WOR_Tabs()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Insert()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Insert(Critical_Incidents entity)
        {
            db.Critical_Incidents.Add(entity);
            db.SaveChanges();
            return View(entity);
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
    }
}