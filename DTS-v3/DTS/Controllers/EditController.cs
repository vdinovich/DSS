﻿using System;
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
            ViewBag.id = id;
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

        [HttpGet]
        public ActionResult Edit_WSIB(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            WSIB wsib = db.WSIBs.SingleOrDefault(e => e.Id == id);
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
            Not_WSIBs wsib = db.Not_WSIBs.SingleOrDefault(e => e.Id == id);
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
            Visits_Others visit = db.Visits_Others.SingleOrDefault(o => o.Id == id);
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
            Outbreaks outbreaks = db.Outbreaks.SingleOrDefault(o => o.Id == id);
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

        public ActionResult Edit_Complaints(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Complaint founded = db.Complaints.Find(id);
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
    }
}