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
            Critical_Incidents incident = db.Critical_Incidents.SingleOrDefault(e => e.id == id);
            if (incident == null) return HttpNotFound();

            return View(incident);
        }


        [HttpPost]
        public ActionResult Edit_Incidents(Critical_Incidents incident)
        {
            if (ModelState.IsValid)
            {
                db = new MyContext();
                db.Entry(incident).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../Select/Select_Incidents");
            }

            return View(incident);
        }

        [HttpGet]
        public ActionResult Edit_Labour(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Labour_Relations labour = db.Relations.SingleOrDefault(e => e.Id == id);
            if (labour == null) return HttpNotFound();

            return View(labour);
        }

        [HttpPost]
        public ActionResult Edit_Labour(Labour_Relations labour)
        {
            if (ModelState.IsValid)
            {
                db = new MyContext();
                db.Entry(labour).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../Select/Select_Labour");
            }

            return View(labour);
        }

        [HttpGet]
        public ActionResult Edit_Community(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Community_Risks risk = db.Community_Risks.SingleOrDefault(e => e.Id == id);
            if (risk == null) return HttpNotFound();

            return View(risk);
        }


        [HttpPost]
        public ActionResult Edit_Community(Community_Risks risk)
        {
            if (ModelState.IsValid)
            {
                db = new MyContext();
                db.Entry(risk).State = EntityState.Modified;
                db.SaveChanges();
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
            var risk = db.Good_News.SingleOrDefault(e => e.Id == id);
            if (risk == null) return HttpNotFound();

            return View(risk);
        }


        [HttpPost]
        public ActionResult Edit_GoodNews(Good_News news)
        {
            if (ModelState.IsValid)
            {
                db = new MyContext();
                db.Entry(news).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../Select/Select_GoodNews");
            }

            return View(news);
        }


        [HttpGet]
        public ActionResult Edit_Agency(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Visits_Agency risk = db.Visits_Agencies.SingleOrDefault(e => e.Id == id);
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
    }
}