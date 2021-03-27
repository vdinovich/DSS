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
        MyContext db = new MyContext();   // Open database connection via EF code first

        [HttpGet]
        public ActionResult Incident_Delete(int? id) //can check if null by using int; won't work w/o ?
        {
            if (id == null) 
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var delete = db.Critical_Incidents.SingleOrDefault(e => e.id == id);   // get CriticalIncident object by id using lamda expression
            Care_Community name1 = db.Care_Communities.Find(delete.Location);      // get Care_Community(Location) object by id location  CriticalIncident foreign key
            CI_Category_Type name2 = db.CI_Category_Types.Find(delete.CI_Category_Type);  // get CI_Category_Type object by id CI_Category_Type  CriticalIncident foreign key
            var arr = new string[] { name1.Name, name2.Name };        // create string array and put two elements - Location.Name & CI_Category_Type.Name both model
            ViewBag.list = arr;   // add array to ViewBag
            if (delete == null) return HttpNotFound(); 

            return View(delete);  
        }


        [HttpPost]
        public ActionResult Incident_Delete(int id)
        {
            var delete = db.Critical_Incidents.SingleOrDefault(l => l.id == id);   // get Critical_Incidents of model by id if it already exist, and null if there doesn't have
            db.Critical_Incidents.Remove(delete ?? throw new InvalidOperationException());      // remove found entity from DbSet collection EF
            db.SaveChanges();            // remove found entity from table

            return RedirectToAction("../Select/Select_Incidents");  /// Redirect on the List Select_Incidents
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
        public ActionResult Education_Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var delete = db.Educations.SingleOrDefault(e => e.Id == id);
            Care_Community name1 = db.Care_Communities.Find(delete.Location);
            ViewBag.list = name1.Name;
            if (delete == null) return HttpNotFound();

            return View(delete);
        }

        [HttpPost]
        public ActionResult Education_Delete(int id)
        {
            var delete = db.Educations.SingleOrDefault(l => l.Id == id);
            db.Educations.Remove(delete ?? throw new InvalidOperationException());
            db.SaveChanges();

            return RedirectToAction("../Select/Education_Select");
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
            Care_Community n = db.Care_Communities.Find(delete.Location);
            ViewBag.list = n.Name;
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
            Care_Community n = db.Care_Communities.Find(delete.Location);
            ViewBag.list = n.Name;
            if (delete == null) return HttpNotFound();

            return View(delete);
        }

        [HttpPost]
        public ActionResult Privacy_Breaches_Delete(int id)
        {
            var delete = db.Privacy_Breaches.SingleOrDefault(l => l.Id == id);
            db.Privacy_Breaches.Remove(delete ?? throw new InvalidOperationException());
            db.SaveChanges();

            return RedirectToAction("../Select/Privacy_Breaches");
        }

        [HttpGet]
        public ActionResult Emergency_Prep_Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var delete = db.Emergency_Prep.SingleOrDefault(e => e.Id == id);
            Care_Community n = db.Care_Communities.Find(delete.Location);
            ViewBag.list = n.Name;
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
            Care_Community name = db.Care_Communities.Find(delete.Location);
            ViewBag.list = name.Name;
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

        [HttpGet]
        public ActionResult Privacy_Complaints_Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var delete = db.Privacy_Complaints.SingleOrDefault(e => e.id == id);
            Care_Community name = db.Care_Communities.Find(delete.Location);
            ViewBag.list = name.Name;
            if (delete == null) return HttpNotFound();

            return View(delete);
        }

        [HttpPost]
        public ActionResult Privacy_Complaints_Delete(int id)
        {

            var delete = db.Privacy_Complaints.SingleOrDefault(l => l.id == id);
            db.Privacy_Complaints.Remove(delete ?? throw new InvalidOperationException());
            db.SaveChanges();

            return RedirectToAction("../Select/Privacy_Complaints");
        }

        [HttpGet]
        public ActionResult Emergency_Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var delete = db.Emergency_Prep.SingleOrDefault(e => e.Id == id);
            Care_Community name = db.Care_Communities.Find(delete.Location);
            ViewBag.list = name.Name;
            if (delete == null) return HttpNotFound();

            return View(delete);
        }

        [HttpPost]
        public ActionResult Emergency_Delete(int id)
        {
            var delete = db.Emergency_Prep.SingleOrDefault(l => l.Id == id);
            db.Emergency_Prep.Remove(delete ?? throw new InvalidOperationException());
            db.SaveChanges();

            return RedirectToAction("../Select/Select_Emergency_Prep");
        }

        [HttpGet]
        public ActionResult Immun_Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var delete = db.Immunizations.SingleOrDefault(e => e.Id == id);
            Care_Community name = db.Care_Communities.Find(delete.Location);
            ViewBag.list = name.Name;
            if (delete == null) return HttpNotFound();

            return View(delete);
        }

        [HttpPost]
        public ActionResult Immun_Delete(int id)
        {
            var delete = db.Immunizations.SingleOrDefault(l => l.Id == id);
            db.Immunizations.Remove(delete ?? throw new InvalidOperationException());
            db.SaveChanges();

            return RedirectToAction("../Select/Select_Immunization");
        }

        public ActionResult Not_WSIB_Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var delete = db.Not_WSIBs.SingleOrDefault(e => e.Id == id);
            Care_Community name = db.Care_Communities.Find(delete.Location);
            ViewBag.list = name.Name;
            if (delete == null) return HttpNotFound();

            return View(delete);
        }

        [HttpPost]
        public ActionResult Not_WSIB_Delete(int id)
        {
            var delete = db.Not_WSIBs.SingleOrDefault(l => l.Id == id);
            db.Not_WSIBs.Remove(delete ?? throw new InvalidOperationException());
            db.SaveChanges();

            return RedirectToAction("../Select/Select_Not_WSIB");
        }
    }
}