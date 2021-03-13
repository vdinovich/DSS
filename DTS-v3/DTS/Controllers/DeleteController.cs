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
            Care_Community name1 = db.Care_Communities.Find(delete.Location);
            CI_Category_Type name2 = db.CI_Category_Types.Find(delete.CI_Category_Type);
            var arr = new string[] { name1.Name, name2.Name };
            ViewBag.list = arr;
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
            Care_Community name1 = db.Care_Communities.Find(delete.Location);
            ViewBag.list = name1.Name;
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
            Care_Community name1 = db.Care_Communities.Find(delete.Location);
            ViewBag.list = name1.Name;
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

        [HttpGet]
        public ActionResult GoodNews_Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var delete = db.Good_News.SingleOrDefault(e => e.Id == id);
            Care_Community c = db.Care_Communities.Find(delete.Location);
            ViewBag.list = c.Name;
            if (delete == null) return HttpNotFound();

            return View(delete);
        }


        [HttpPost]
        public ActionResult GoodNews_Delete(int id)
        {
            var delete = db.Good_News.SingleOrDefault(l => l.Id == id);
            db.Good_News.Remove(delete ?? throw new InvalidOperationException());
            db.SaveChanges();

            return RedirectToAction("../Select/Select_GoodNews");
        }

        [HttpGet]
        public ActionResult Agency_Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var delete = db.Visits_Agencies.SingleOrDefault(e => e.Id == id);
            if (delete == null) return HttpNotFound();

            return View(delete);
        }


        [HttpPost]
        public ActionResult Agency_Delete(int id)
        {
            var delete = db.Visits_Agencies.SingleOrDefault(l => l.Id == id);
            db.Visits_Agencies.Remove(delete ?? throw new InvalidOperationException());
            db.SaveChanges();

            return RedirectToAction("../Select/Select_Agencies");
        }

        [HttpGet]
        public ActionResult WSIB_Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var delete = db.WSIBs.SingleOrDefault(e => e.Id == id);
            if (delete == null) return HttpNotFound();

            return View(delete);
        }


        [HttpPost]
        public ActionResult WSIB_Delete(int id)
        {
            var delete = db.WSIBs.SingleOrDefault(l => l.Id == id);
            db.WSIBs.Remove(delete ?? throw new InvalidOperationException());
            db.SaveChanges();

            return RedirectToAction("../Select/Select_WSIB");
        }

        [HttpGet]
        public ActionResult Visits_Others_Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var delete = db.Visits_Others.SingleOrDefault(e => e.Id == id);
            Care_Community n = db.Care_Communities.Find(delete.Location);
            ViewBag.list = n.Name;
            if (delete == null) return HttpNotFound();

            return View(delete);
        }


        [HttpPost]
        public ActionResult Visits_Others_Delete(int id)
        {
            Visits_Others delete = db.Visits_Others.SingleOrDefault(l => l.Id == id);
            db.Visits_Others.Remove(delete ?? throw new InvalidOperationException());
            db.SaveChanges();

            return RedirectToAction("../Select/Select_Visits_Others");
        }

        [HttpGet]
        public ActionResult Outbreaks_Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var delete = db.Outbreaks.SingleOrDefault(e => e.Id == id);
            if (delete == null) return HttpNotFound();

            return View(delete);
        }


        [HttpPost]
        public ActionResult Outbreaks_Delete(int id)
        {
            Outbreaks delete = db.Outbreaks.SingleOrDefault(l => l.Id == id);
            db.Outbreaks.Remove(delete ?? throw new InvalidOperationException());
            db.SaveChanges();

            return RedirectToAction("../Select/Outbreaks");
        }

        [HttpGet]
        public ActionResult Privacy_Breaches_Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var delete = db.Privacy_Breaches.SingleOrDefault(e => e.Id == id);
            if (delete == null) return HttpNotFound();

            return View(delete);
        }

        [HttpPost]
        public ActionResult Privacy_Breaches_Delete(int id)
        {
            Outbreaks delete = db.Outbreaks.SingleOrDefault(l => l.Id == id);
            db.Outbreaks.Remove(delete ?? throw new InvalidOperationException());
            db.SaveChanges();

            return RedirectToAction("../Select/Outbreaks");
        }

        [HttpGet]
        public ActionResult Emergency_Prep_Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var delete = db.Emergency_Prep.SingleOrDefault(e => e.Id == id);
            if (delete == null) return HttpNotFound();

            return View(delete);
        }

        [HttpPost]
        public ActionResult Emergency_Prep_Delete(int id)
        {
            var delete = db.Outbreaks.SingleOrDefault(l => l.Id == id);
            db.Outbreaks.Remove(delete ?? throw new InvalidOperationException());
            db.SaveChanges();

            return RedirectToAction("../Select/Emergency_Prep");
        }

        [HttpGet]
        public ActionResult Complaints_Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var delete = db.Complaints.SingleOrDefault(e => e.Id == id);
            Care_Community name1 = db.Care_Communities.Find(delete.Location);
            ViewBag.list = name1.Name;
            if (delete == null) return HttpNotFound();

            return View(delete);
        }

        [HttpPost]
        public ActionResult Complaints_Delete(int id)
        {
            var delete = db.Complaints.SingleOrDefault(l => l.Id == id);
            db.Complaints.Remove(delete ?? throw new InvalidOperationException());
            db.SaveChanges();

            return RedirectToAction("../Select/Select_Complaints");
        }
    }
}