using DTS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DTS.Controllers
{
    public class DeleteController : Controller
    {
        MyContext db = new MyContext();

        [HttpGet]
        public ActionResult Incident_Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var delete = db.Critical_Incidents.SingleOrDefault(e => e.id == id);
            if (delete == null) return HttpNotFound();

            return View(delete);
        }


        [HttpPost]
        public ActionResult Incident_Delete(int id)
        {
            var delete = db.Critical_Incidents.SingleOrDefault(l => l.id == id);
            db.Critical_Incidents.Remove(delete ?? throw new InvalidOperationException());
            db.SaveChanges();

            return RedirectToAction("../Select/Select_Incidents");
        }

        [HttpGet]
        public ActionResult Labour_Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Labour_Relations delete = db.Relations.SingleOrDefault(e => e.Id == id);
            if (delete == null) return HttpNotFound();

            return View(delete);
        }


        [HttpPost]
        public ActionResult Labour_Delete(int id)
        {
            Labour_Relations delete = db.Relations.SingleOrDefault(l => l.Id == id);
            db.Relations.Remove(delete ?? throw new InvalidOperationException());
            db.SaveChanges();

            return RedirectToAction("../Select/Select_Labour");
        }

        [HttpGet]
        public ActionResult Community_Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var delete = db.Community_Risks.SingleOrDefault(e => e.Id == id);
            if (delete == null) return HttpNotFound();

            return View(delete);
        }


        [HttpPost]
        public ActionResult Community_Delete(int id)
        {
            var delete = db.Community_Risks.SingleOrDefault(l => l.Id == id);
            db.Community_Risks.Remove(delete ?? throw new InvalidOperationException());
            db.SaveChanges();

            return RedirectToAction("../Select/Select_Community");
        }

        [HttpGet]
        public ActionResult User_Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var delete = db.Users.SingleOrDefault(e => e.Id == id);
            if (delete == null) return HttpNotFound();

            return View(delete);
        }


        [HttpPost]
        public ActionResult User_Delete(int id)
        {
            var delete = db.Users.SingleOrDefault(l => l.Id == id);
            db.Users.Remove(delete ?? throw new InvalidOperationException());
            db.SaveChanges();

            return RedirectToAction("../Select/Select_Users");
        }
    }
}