using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DTS.Models;

namespace DTS.Controllers
{
    public class EditController : Controller
    {
        MyContext db = new MyContext();

        [HttpGet]
        public ActionResult Edit_Incidents(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Critical_Incidents incident = db.Critical_Incidents.Find(id);       // get Critical_Incidents object by id
            object[] objs = new object[] { HomeController.list, HomeController.list3 };    // using thwo list (Location & CI_Category_Type) from a static fields of HomeController class'
            ViewBag.locations = objs;        // add object of array on ViewBag
            if (incident == null) return HttpNotFound();
            //ViewBag.id = id;     // add 
            return View(incident);
        }


        [HttpPost]
        public ActionResult Edit_Incidents(Critical_Incidents incident)   // get updated Critical_Incidents object
        {
            if (ModelState.IsValid)
            {
                db = new MyContext();   // open connection
                db.Entry(incident).State = EntityState.Modified;    // Set property  updated object to 
                db.SaveChanges();    // Press Execute in C#
                return RedirectToAction("../Select/Select_Incidents"); // redirect to view List
            }

            return View(incident);   // else if our model state false(or something went wrong)
        }

        [HttpGet]
        public ActionResult EditEducation(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var founded = db.Educations.Find(id);
            ViewBag.locations = HomeController.list;
            if (founded == null) return HttpNotFound();
            return View(founded);
        }

        [HttpPost]
        public ActionResult EditEducation(Education entity)
        {
            if (ModelState.IsValid)
            {
                db = new MyContext();
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../Select/Education_Select");
            }

            return View(entity);
        }


        [HttpGet]
        public ActionResult Edit_Labour(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Labour_Relations labour = db.Relations.Find(id);
            ViewBag.locations = HomeController.list;

            if (labour == null) return HttpNotFound();

            return View(labour);
        }

        [HttpPost]
        public ActionResult Edit_Labour(Labour_Relations labour)
        {
            if (ModelState.IsValid)
            {
                db.Entry(labour).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../Select/Select_Labour");
            }

            return View(labour);
        }

        public ActionResult Edit_Community(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Community_Risks edit = db.Community_Risks.Find(id);
            ViewBag.locations = HomeController.list;

            if (edit == null) return HttpNotFound();

            return View(edit);
        }

        [HttpPost]
        public ActionResult Edit_Community(Community_Risks risk)
        {
            if (ModelState.IsValid)
            {
                db = new MyContext();
                db.Entry(risk).State = EntityState.Modified;
                int res = db.SaveChanges();
                return RedirectToAction("../Select/Select_Community");
            }

            return View(risk);
        }

        [HttpGet]
        public ActionResult Edit_User(int? id)
        {
            List<Care_Community> communities = db.Care_Communities.ToList();
            SelectList list = new SelectList(communities, "Id", "Name");

            List<Position> positions = db.Positions.ToList();
            SelectList list2 = new SelectList(positions, "Id", "Name");
            List<object> both = new List<object> { list, list2 };
            ViewBag.listing = both;

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var risk = db.Users.SingleOrDefault(e => e.Id == id);
            if (risk == null) return HttpNotFound();

            return View(risk);
        }


        [HttpPost]
        public ActionResult Edit_User(Users u)
        {
            if (ModelState.IsValid)
            {
                db = new MyContext();
                db.Entry(u).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../Select/Select_Users");
            }

            return View(u);
        }

        [HttpGet]
        public ActionResult Edit_GoodNews(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var founded = db.Good_News.Find(id);
            ViewBag.locations = HomeController.list;
            if (founded == null) return HttpNotFound();
            return View(founded);
        }


        [HttpPost]
        public ActionResult Edit_GoodNews(Good_News edit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(edit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../Select/Select_GoodNews");
            }

            return View();
        }


        [HttpGet]
        public ActionResult Edit_Agency(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var risk = db.Visits_Agencies.SingleOrDefault(e => e.Id == id);
            if (risk == null) return HttpNotFound();

            return View(risk);
        }
        
        [HttpPost]
        public ActionResult Edit_Agency(Visits_Agency agency)
        {
            if (ModelState.IsValid)
            {
                db = new MyContext();
                db.Entry(agency).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../Select/Select_Agencies");
            }

            return View(agency);
        }

        [HttpGet]
        public ActionResult Edit_WSIB(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var wsib = db.WSIBs.SingleOrDefault(e => e.Id == id);
            ViewBag.locations = HomeController.list;
            if (wsib == null) return HttpNotFound();

            return View(wsib);
        }

        [HttpPost]
        public ActionResult Edit_WSIB(WSIB entity)
        {
            if (ModelState.IsValid)
            {
                db = new MyContext();
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../Select/Select_WSIB");
            }

            return View();
        }

        [HttpGet]
        public ActionResult Edit_Not_WSIB(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var wsib = db.Not_WSIBs.SingleOrDefault(e => e.Id == id);
            ViewBag.locations = HomeController.list;
            if (wsib == null) return HttpNotFound();

            return View(wsib);
        }

        [HttpPost]
        public ActionResult Edit_Not_WSIB(Not_WSIBs entity)
        {
            if (ModelState.IsValid)
            {
                db = new MyContext();
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../Select/Select_Not_WSIB");
            }

            return View();
        }

        [HttpGet]
        public ActionResult Edit_Visits_Others(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var visit = db.Visits_Others.Find(id);
            ViewBag.locations = HomeController.list;
            if (visit == null) return HttpNotFound();

            return View(visit);
        }

        [HttpPost]
        public ActionResult Edit_Visits_Others(Visits_Others entity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../Select/Select_Visits_Others");
            }

            return View();
        }

        [HttpGet]
        public ActionResult Edit_Outbreaks(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var outbreaks = db.Outbreaks.SingleOrDefault(o => o.Id == id);
            if (outbreaks == null) return HttpNotFound();

            return View(outbreaks);
        }

        [HttpPost]
        public ActionResult Edit_Outbreaks(Outbreaks entity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../Select/Outbreaks");
            }

            return View();
        }

        [HttpGet]
        public ActionResult Edit_Privacy_Complaints(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var founded = db.Privacy_Complaints.Find(id);
            ViewBag.locations = HomeController.list;
            if (founded == null) return HttpNotFound();
            return View(founded);
        }

        [HttpPost]
        public ActionResult Edit_Privacy_Complaints(Privacy_Complaints edit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(edit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../Select/Privacy_Complaints");
            }

            return View();
        }

        [HttpGet]
        public ActionResult Edit_Complaints(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var founded = db.Complaints.Find(id);
            ViewBag.locations = HomeController.list;
            if (founded == null) return HttpNotFound();
            return View(founded);
        }

        [HttpPost]
        public ActionResult Edit_Complaints(Complaint edit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(edit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../Select/Select_Complaints");
            }

            return View();
        }

        [HttpGet]
        public ActionResult Edit_Privacy_Breaches(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            ViewBag.locations = HomeController.list;
            var founded = db.Privacy_Breaches.Find(id);
            if (founded == null) return HttpNotFound();
            return View(founded);
        }

        [HttpPost]
        public ActionResult Edit_Privacy_Breaches(Privacy_Breaches edit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(edit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../Select/Privacy_Breaches");
            }

            return View();
        }

        [HttpGet]
        public ActionResult Edit_Emergency(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            ViewBag.locations = HomeController.list;
            var founded = db.Emergency_Prep.Find(id);
            if (founded == null) return HttpNotFound();
            return View(founded);
        }

        [HttpPost]
        public ActionResult Edit_Emergency(Emergency_Prep edit)
        {

            if (ModelState.IsValid)
            {
                db.Entry(edit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../Select/Select_Emergency_Prep");
            }

            return View();
        }

        [HttpGet]
        public ActionResult Edit_Immun(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            ViewBag.locations = HomeController.list;
            var founded = db.Immunizations.Find(id);
            if (founded == null) return HttpNotFound();
            return View(founded);
        }

        [HttpPost]
        public ActionResult Edit_Immun(Immunization edit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(edit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../Select/Select_Immunization");
            }

            return View();
        }
    }
}