using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTS.Models;

namespace DTS.Controllers
{
    public class RegisterController : Controller
    {
        MyContext db = new MyContext();

        [HttpGet]
        public ActionResult Register_New_User()
        {
            List<Care_Community> communities = db.Care_Communities.ToList();
            SelectList list = new SelectList(communities, "Id", "Name");

            List<Position> positions = db.Positions.ToList();
            SelectList list2 = new SelectList(positions, "Id", "Name");
            List<object> both = new List<object> { list, list2 };
            ViewBag.listing = both;

            return View();
        }

        [HttpPost]
        public ActionResult Register_New_User(Users user)
        {
            user.Date_Register = DateTime.Now;
            db.Users.Add(user);
            db.SaveChanges();

            Users u = db.Users.Where(w => w.First_Name == user.First_Name).FirstOrDefault();
            //Creating new table for registered user:
            //if (u != null) {
            //    db.Database.ExecuteSqlCommand
            //        ($"CREATE TABLE [dbo].[Sign_in_Main_{u.Id}] (" +
            //        "[id]      INT           IDENTITY(1, 1) NOT NULL," +
            //        "[care_community_centre] NVARCHAR(50) NULL," +
            //        "[user_name]             NVARCHAR(50) NULL," +
            //        "[position]              NVARCHAR(50) NULL," +
            //        "[current_date]          DATETIME      NULL," +
            //        "[week]                  INT           NULL," +
            //        "[date_entred]           DATETIME      NULL)");
            //}
            return RedirectToAction("../Select/Select_Users");
        }
    }
}