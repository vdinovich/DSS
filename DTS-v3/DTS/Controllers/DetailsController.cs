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
            var entity = db.Critical_Incidents.SingleOrDefault(rel => rel.id == id);
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
    }
}
