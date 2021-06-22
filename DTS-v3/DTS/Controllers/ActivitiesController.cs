namespace DTS.Controllers
{
    using DTS.Models;
    using System.Web.Mvc;
    using System.Collections.Generic;

    public class ActivitiesController : Controller
    {
        static string locSelected;
        private List<Activities> _activitiesList = new List<Activities>();

        [HttpPost]
        public ActionResult SelectLoc(object val)
        {
            locSelected = val.ToString();
            return RedirectToAction("WOR_Tabs", "Home");
        }

        [HttpGet]
        public ActionResult Add()
        {
            var activityId = int.Parse(RouteData.Values["id"].ToString());
            string parentActivityDescription;

            using (var activitiesDb = new MyContext())
            {
                var parentActivity = activitiesDb.Activities.Find(activityId);
                parentActivityDescription = parentActivity?.ActivityDescription ?? "No parent activity";
            }

            ViewBag.ParentActivity = activityId;
            ViewBag.ParentActivityDescription = parentActivityDescription;

            return View();
        }

        [HttpPost]
        public ActionResult Add(Activities activities)
        {
            try
            {
                var activity = new Activities()
                {
                    ParentActivityID = activities.ParentActivityID,
                    ActivityDescription = activities.ActivityDescription,
                    StartDateTime = activities.StartDateTime,
                    EndDateTime = activities.EndDateTime
                };

                using (var activitiesDb = new MyContext())
                {
                    var parentActivity = activitiesDb.Activities.Find(activity.ParentActivityID);
                    if (parentActivity == null && activities.ActivityID != 0)
                    {
                        return RedirectToAction("Index");
                    }
                    activitiesDb.Activities.Add(activity);
                    activitiesDb.SaveChanges();
                }
                return RedirectToAction("WOR_Tabs", "Home");
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("ParentActivityId", ex.Message);
                return View("Add", activities);
            }
        }
    }
}