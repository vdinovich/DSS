using DTS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DTS.Controllers
{
    public class DetailsController : Controller
    {
        MyContext db = new MyContext();

        public ActionResult Incidents_Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Critical_Incidents entity = db.Critical_Incidents.SingleOrDefault(rel => rel.id == id);
            Care_Community name1 = db.Care_Communities.Find(entity.Location);
            CI_Category_Type name2 = db.CI_Category_Types.Find(entity.CI_Category_Type);
            var arr = new string[] { name1.Name, name2.Name };
            ViewBag.list = arr;
            if (entity == null)
                return HttpNotFound();
            return View(entity);
        }

        public ActionResult Labour_Details(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var entity = db.Relations.SingleOrDefault(rel => rel.Id == id);
            Care_Community name1 = db.Care_Communities.Find(entity.Location);
            ViewBag.list = name1.Name;
            if (entity == null)
                return HttpNotFound();
            return View(entity);
        }

        public ActionResult Community_Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var entity = db.Community_Risks.SingleOrDefault(rel => rel.Id == id);
            Care_Community name1 = db.Care_Communities.Find(entity.Location);
            ViewBag.list = name1.Name;
            if (entity == null)
                return HttpNotFound();
            return View(entity);
        }

        public ActionResult User_Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var entity = db.Users.SingleOrDefault(rel => rel.Id == id);
            if (entity == null)
                return HttpNotFound();
            return View(entity);
        }

        public ActionResult GoodNews_Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var entity = db.Good_News.SingleOrDefault(rel => rel.Id == id);
            Care_Community c = db.Care_Communities.Find(entity.Location);
            ViewBag.list = c.Name;
            if (entity == null)
                return HttpNotFound();
            return View(entity);
        }

        public ActionResult Agency_Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var entity = db.Visits_Agencies.SingleOrDefault(rel => rel.Id == id);
            if (entity == null)
                return HttpNotFound();
            return View(entity);
        }

        public ActionResult WSIB_Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WSIB entity = db.WSIBs.SingleOrDefault(w => w.Id == id);
            if (entity == null)
                return HttpNotFound();
            return View(entity);
        }

        public ActionResult Visits_Others_Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Visits_Others entity = db.Visits_Others.SingleOrDefault(w => w.Id == id);
            Care_Community n = db.Care_Communities.Find(entity.Location);
            ViewBag.list = n.Name;
            if (entity == null)
                return HttpNotFound();
            return View(entity);
        }

        public ActionResult EducationDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var entity = db.Educations.SingleOrDefault(w => w.Id == id);
            Care_Community n = db.Care_Communities.Find(entity.Location);
            ViewBag.list = n.Name;
            if (entity == null)
                return HttpNotFound();
            return View(entity);
        }

        public ActionResult Outbreaks(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Outbreaks entity = db.Outbreaks.SingleOrDefault(w => w.Id == id);
            if (entity == null)
                return HttpNotFound();
            return View(entity);
        }

        public ActionResult Privacy_Breaches(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Outbreaks entity = db.Outbreaks.SingleOrDefault(w => w.Id == id);
            if (entity == null)
                return HttpNotFound();
            return View(entity);
        }

        public ActionResult Complaints_Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var entity = db.Complaints.SingleOrDefault(w => w.Id == id);
            Care_Community name = db.Care_Communities.Find(entity.Location);
            ViewBag.list = name.Name;
            if (entity == null)
                return HttpNotFound();
            return View(entity);
        }

        public ActionResult Privacy_Complaints_Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var entity = db.Privacy_Complaints.SingleOrDefault(w => w.id == id);
            Care_Community name = db.Care_Communities.Find(entity.Location);
            ViewBag.list = name.Name;
            if (entity == null)
                return HttpNotFound();
            return View(entity);
        }

        public ActionResult Emergency_Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var entity = db.Emergency_Prep.SingleOrDefault(w => w.Id == id);
            Care_Community name = db.Care_Communities.Find(entity.Location);
            ViewBag.list = name.Name;
            if (entity == null)
                return HttpNotFound();
            return View(entity);
        }

        public ActionResult Privacy_Breaches_Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var entity = db.Privacy_Breaches.SingleOrDefault(w => w.Id == id);
            Care_Community name = db.Care_Communities.Find(entity.Location);
            ViewBag.list = name.Name;
            if (entity == null)
                return HttpNotFound();
            return View(entity);
        }

        public ActionResult Immun_Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var entity = db.Immunizations.SingleOrDefault(w => w.Id == id);
            Care_Community name = db.Care_Communities.Find(entity.Location);
            ViewBag.list = name.Name;
            if (entity == null)
                return HttpNotFound();
            return View(entity);
        }

        [HttpGet]
        public ActionResult Not_WSIB_Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var entity = db.Not_WSIBs.SingleOrDefault(w => w.Id == id);
            Care_Community name = db.Care_Communities.Find(entity.Location);
            ViewBag.list = name.Name;
            if (entity == null)
                return HttpNotFound();
            return View(entity);
        }
    }
}
