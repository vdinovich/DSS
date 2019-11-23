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
    }
}