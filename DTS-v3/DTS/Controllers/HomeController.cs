namespace DTS.Controllers
{
    using System;
    using System.IO;
    using DTS.Models;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Collections;
    using static System.String;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public enum Role { Admin, User }

    public class HomeController : Controller
    {
        #region Fields:
        MyContext db = new MyContext();
        static string notsel;
        public static int[] counts;
        public static List<string> strs, strN;
        public static string path;
        public static int Id_Location { get; set; }
        List<CI_Category_Type> categories;
        List<Care_Community> communities;
        List<Department> departments;
        List<Position> positions;
        public static SelectList
            list2, list, list3, list4, list5, list6, list7, list8, list9, list10,
            list11, list12, list13, list14, list15, list16, list17, list18, list19, list20; //needed for front end drop down list
        List<object> both;
        string[] SelectYesNo = new string[] { "Yes", "No" },
                 visit = new string[] { "Visit", "Phone Call" },
                 written = new string[] { "Verbal", "Written" },
                 direct = new string[] { "Direct", "Corporate", "Both" },
                 resident = new string[] { "Resident", "Family", "Visitor", "Stuff", "Other" },
                 resolved = new string[] { "Yes", "No", "Ongoing" },
                 ministry = new string[] { "Yes", "No" },
                 categoryGoodNews = new string[] { "GoodNews", "Compliments" },
                 departmentGoodNews = new string[] { "Nursing", "Nursing Admin", "Admin", "Programs", "Food Service", "Maintainence", "Housekeeping", "Laundry", "Other" },
                 sourceGoodNews = new string[] { "Let's Connect", "Card", "Email", "Letter", "Verbal", "Other" },
                 department2 = new string[] {"All","Nursing","Housekeeping","Laundry","Maintenance","Dietary","Recriation","Administration","Individual(s)",
                                            "Physio", "Hairdresser", "Physician","Foot care", "Dental", "Other", "Yes", "No"},
                 receiveFrom = new string[] { "Resident", "Family", "Supplier", "SSO", "Manager", "Leadership", "Tour", "Other" },
                 picture = new string[] { "Yes", "No" },
                 risk_list = new string[] { "Adverse Event", "Environmental", "Infortmation Technology", "Insurance", "Legal", "Near Miss", "Serious Adverse Event",
                     "Vendor", "Workplace Harrasement", "Other" },
                 visitAgency = new string[] { "MOH", "Fire", "TSSA", "Public Health", "PCQO", "QR Health Authority", "CNS", "Public Health - MHO", "Public Health - EHO", "Other" },
                 visitnumbers = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
        #endregion

        #region Constructor:
        public HomeController()
        {
            communities = db.Care_Communities.ToList();
            list = new SelectList(communities, "Id", "Name");

            positions = db.Positions.ToList();
            list2 = new SelectList(positions, "Id", "Name");

            categories = db.CI_Category_Types.ToList();
            list3 = new SelectList(categories, "Id", "Name");

            list4 = new SelectList(SelectYesNo);  // For some attributes table Clritical Incident
            list5 = new SelectList(visit);        // for one attribute MOHLTC_Follow_Up the same table

            // for the Complaints table:
            list6 = new SelectList(written);
            list7 = new SelectList(direct);
            list8 = new SelectList(resident);
            list9 = new SelectList(departmentGoodNews);
            list10 = new SelectList(resolved);
            list11 = new SelectList(ministry);

            // for GoodNews table:
            list12 = new SelectList(categoryGoodNews);
            list13 = new SelectList(department2);
            list14 = new SelectList(sourceGoodNews);
            list15 = new SelectList(receiveFrom);
            list16 = new SelectList(picture);

            // for Community Risks:
            list17 = new SelectList(risk_list);

            // for Visit Agency:
            list18 = new SelectList(visitAgency);
            list19 = new SelectList(visitnumbers);

            // for Departments:
            departments = db.Departments.ToList();
            list20 = new SelectList(departments, "Id", "Name");
        }
        #endregion

        #region Logout(clear all static members):
        public ActionResult Logout()
        {
            ClearAllStatic();
            checkView = "none";
            return RedirectToAction("../Home/SignIN");
        }
        #endregion

        #region Clear all list for new search(for range):
        void ClearAllStatic()
        {
            checkRepead = false;
            foundSummary1 = new List<CriticalIncidentSummary>();
            allSummary1 = new List<IncidentSummaryAll>();
            foundSummary2 = new List<ComplaintsSummary>();
            allSummary2 = new List<ComplaintsSummaryAll>();
            locList = new List<string>();
            ll1 = new List<Critical_Incidents>();
            ll2 = new List<Critical_Incidents>();
            ll3 = new List<Critical_Incidents>();
            ll4 = new List<Critical_Incidents>();
            ll5 = new List<Critical_Incidents>();
            ll6 = new List<Critical_Incidents>();
            ll7 = new List<Critical_Incidents>();
            ll8 = new List<Critical_Incidents>();
            ll9 = new List<Critical_Incidents>();
            ll10 = new List<Critical_Incidents>();
            ll11 = new List<Critical_Incidents>();
            p1 = p2 = p3 = p4 = p5 = p6 = p7 = p8 = p9 = p10 = p11 = p12 = p13 = p14 = p15 = p16 = p17 = p18 = p18 = p19 = p20 = p21 = p22 =
                p23 = p24 = p25 = p26 = p27 = 0;
        }
        #endregion

        #region Complaints(Insert):
        public ActionResult Complaint_Insert()
        {
            object[] objs = new object[] { list, list3, list4, list5, list6, list7, list8, list9, list10, list11 };
            ViewBag.locations = objs;
            return View();
        }

        [HttpPost]
        public ActionResult Complaint_Insert(Complaint entity)
        {
            object[] objs = new object[] { list, list3, list4, list5, list6, list7, list8, list9, list10, list11 };
            ViewBag.locations = objs;
            if (entity.DateReceived == null && entity.Location == 0 && entity.WritenOrVerbal == null && entity.Receive_Directly == null &&
               entity.FromResident == null && entity.ResidentName == null && entity.Department == null && entity.BriefDescription == null &&
               entity.IsAdministration == false && entity.CareServices == false && entity.PalliativeCare == false && entity.Dietary == false && entity.Housekeeping == false &&
               entity.Laundry == false && entity.Maintenance == false && entity.Programs == false && entity.Physician == false && entity.Beautician == false &&
               entity.FootCare == false && entity.DentalCare == false && entity.Physio == false && entity.Other == false && entity.MOHLTCNotified == null && entity.CopyToVP == null &&
               entity.ResponseSent == null && entity.ActionToken == null && entity.Resolved == null && entity.MinistryVisit == null)
            {
                ViewBag.Empty = "All fields have to be filled.";
                return View();
            }
            else if (entity.DateReceived == null || entity.Location == 0 || entity.WritenOrVerbal == null || entity.Receive_Directly == null ||
               entity.FromResident == null || entity.ResidentName == null || entity.Department == null || entity.BriefDescription == null ||
               entity.IsAdministration == false || entity.CareServices == false || entity.PalliativeCare == false || entity.Dietary == false || entity.Housekeeping == false ||
               entity.Laundry == false || entity.Maintenance == false || entity.Programs == false || entity.Physician == false || entity.Beautician == false ||
               entity.FootCare == false || entity.DentalCare == false || entity.Physio == false || entity.Other == false || entity.MOHLTCNotified == null || entity.CopyToVP == null ||
               entity.ResponseSent == null || entity.ActionToken == null || entity.Resolved == null || entity.MinistryVisit == null)
            {

                try
                {
                    Id_Location = entity.Location;
                    db.Complaints.Add(entity);
                    db.SaveChanges();

                    return RedirectToAction("../Select/Select_Complaints");
                }
                catch (Exception ex) { return Json("Error occurred. Error details: " + ex.Message); }
                // ViewBag.Empty = "Some fields are empty. Please fill it out and try again!";
                // return View();
            }
            else
            {
                try
                {
                    Id_Location = entity.Location;
                    db.Complaints.Add(entity);
                    db.SaveChanges();

                    return RedirectToAction("../Select/Select_Complaints");
                }
                catch (Exception ex) { return HttpNotFound(ex.Message); }
            }
        }
        #endregion

        #region Sign In:
        [HttpGet]
        public ActionResult SignIN()
        {
            return View();
        }

        static Role role;
        public static bool isAdmin;
        [HttpPost]
        public ActionResult SignIN(Users user)
        {
            checkView = "none";
            ClearAllStatic();
            string login = user.Login;
            string password = user.Password;
            List<Users> users = db.Users.ToList(); //takes all users from users table in a db and packs it into list users
            bool flag = false;
            for (int i = 0; i < users.Count(); i++)
            {
                if (users[i].Login == login && users[i].Password == password && users[i].Role.Equals("User"))
                {
                    role = Role.User;
                    isAdmin = false;
                    flag = true;
                    return RedirectToAction("../Home/Index");
                }
                else if (users[i].Login == login && users[i].Password == password && users[i].Role.Equals("Admin"))
                {
                    role = Role.Admin;
                    isAdmin = true;
                    flag = true;
                    return RedirectToAction("../Home/WOR_Tabs");
                }
            }
            if (!flag) ViewBag.incorrect = "Incorrect Login or Password...Please try again!";
            return View();
        }
        #endregion

        #region Index:
        [HttpGet]
        public ActionResult Index()
        {
            both = new List<object> { list, list2 };
            ViewBag.listing = both;
            var mainModel = new Sign_in_Main();
            return View(mainModel);
        }

        [HttpPost]
        public ActionResult Index(Sign_in_Main main)
        {
            if (main.Id == 0)
            {
                if (main.User_Name == null || main.Position == null || main.Week == 0 || main.Current_Date == DateTime.MinValue)
                {
                    if (main.Care_Community_Centre == null ||
                        main.Position == null || main.User_Name == null) // if nothing was selected in a Location drop-down list
                    {
                        notsel = "This field is required. Please fill it in.";
                        both = new List<object> { list, list2 };
                        ViewBag.listing = both;
                        ViewBag.EmptyRequired = notsel;
                    }
                    else//if(main.User_Name == null || main.Position == null || main.Week == 0 || main.Current_Date == DateTime.MinValue)
                    {
                        Id_Location = int.Parse(main.Care_Community_Centre); //we get an id (converted into int) from the list
                        both = new List<object> { list, list2 }; //prepare two lists for viewbag for later use in drop-down lists
                        ViewBag.listing = both;
                        main.Care_Community_Centre = list.Where(p => p.Value == main.Care_Community_Centre).First().Text; //retrieves value from the list

                        main.Position = list2.Where(p => p.Value == main.Position).First().Text;
                        main.Date_Entred = DateTime.Now;
                        db.Sign_in_Mains.Add(main);
                        db.SaveChanges();

                        main.Care_Community_Centre = null;
                        main.Position = null;
                        main.Week = 0;
                        main.User_Name = null;
                        ViewBag.ResultMsg = "Your record was added successfuly!";
                        return RedirectToAction("../Home/WOR_Tabs");
                    }
                }
                else
                {
                    Id_Location = int.Parse(main.Care_Community_Centre);
                    both = new List<object> { list, list2 };
                    ViewBag.listing = both;
                    main.Care_Community_Centre = list.Where(p => p.Value == main.Care_Community_Centre).First().Text;
                    main.Position = list2.Where(p => p.Value == main.Position).First().Text;
                    main.Date_Entred = DateTime.Now;
                    db.Sign_in_Mains.Add(main);
                    db.SaveChanges();

                    main.Care_Community_Centre = null;
                    main.Position = null;
                    main.Week = 0;
                    main.User_Name = null;
                    //ViewBag.listing = both;
                    ViewBag.ResultMsg = "Your record was successfuly added!";

                    return View();
                }
            }
            return View();
        }
        #endregion

        #region Upload files:
        [HttpPost]
        public ActionResult Uploded()
        {// Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        // Get the complete folder path and store the file inside it.  
                        path = Path.Combine(Server.MapPath($"~/Uploaded_Files/{fname}"));
                        file.SaveAs(path);
                    }
                    // Returns message that successfully uploaded  
                    return Json("Your file was uploaded successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("There was no file selected. Please try again.");
            }
        }

        public ActionResult AllFiles()
        {
            List<string> names = new List<string>();
            path = Server.MapPath("~/Uploaded_Files");
            string[] files_names = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            //if(files_names == null || files_names.Length == 0)
            //{
            //    ViewBag.Empty = "There is no uploaded file here. Please go to the upload area.";
            //    return View();
            //}

            for (int i = 0; i < files_names.Length; i++)
                names.Add(Path.GetFileName(files_names[i]));

            return View(names);
        }
        #endregion

        #region Delete file:
        public ActionResult DeleteFile(string item)
        {
            bool flag = false;
            if (item != null)
            {
                flag = true;
                string path = Path.Combine(Server.MapPath($"~/Uploaded_Files/{item}"));
                System.IO.File.Delete(path);
            }
            if (!flag) return Json("There is nothing to delete...Please upload a file first.");
            else return RedirectToAction("../Home/AllFiles");
        }
        #endregion

        #region WOR Tabs(Get):
        public static int num_tbl;
        public static string checkView = "none";
        public static bool b = false;
        static List<IncidentSummaryAll> allSummary1 = new List<IncidentSummaryAll>();
        static List<ComplaintsSummaryAll> allSummary2 = new List<ComplaintsSummaryAll>();
        public static List<Activities> formList = default;
        [HttpGet]
        public ActionResult WOR_Tabs()
        {
            ViewBag.AllCI = db.CI_Category_Types.ToList();
            ViewBag.AllLocs = db.Care_Communities.ToList();

            if (w_without != null) 
                mirrorWout = w_without;

            WorTabs tabs = null;
            try
            {
                formList = db.Activities.ToList();
            }
            catch { throw new Exception(); }

            { ViewBag.Activities = formList; }

            if (role == Role.Admin)
                ViewBag.Welcome = Role.Admin;
            else if (role == Role.User)
                ViewBag.Welcome = Role.User;
            if (Id_Location == 0)
            {
                { ViewBag.List = TablesContainer.list1; }
                { ViewBag.List = TablesContainer.list2; }
                { ViewBag.List = TablesContainer.list3; }
                { ViewBag.List = TablesContainer.list4; }
                { ViewBag.List = TablesContainer.list5; }
                { ViewBag.List = TablesContainer.list6; }
                { ViewBag.List = TablesContainer.list7; }
                { ViewBag.List = TablesContainer.list8; }
                { ViewBag.List = TablesContainer.list9; }
                { ViewBag.List = TablesContainer.list10; }
                { ViewBag.List = TablesContainer.list13; }
                { ViewBag.List = TablesContainer.list14; }
            }
            else
            {
                { ViewBag.List = TablesContainer.list1 = db.Critical_Incidents.Where(l => l.Location == Id_Location).ToList(); }
                { ViewBag.List = TablesContainer.list2 = db.Complaints.Where(l => l.Location == Id_Location).ToList(); }
                { ViewBag.List = TablesContainer.list3 = db.Good_News.Where(l => l.Location == Id_Location).ToList(); }
                { ViewBag.List = TablesContainer.list4 = db.Emergency_Prep.Where(l => l.Location == Id_Location).ToList(); }
                { ViewBag.List = TablesContainer.list5 = db.Community_Risks.Where(l => l.Location == Id_Location).ToList(); }
                { ViewBag.List = TablesContainer.list6 = db.Visits_Others.Where(l => l.Location == Id_Location).ToList(); }
                { ViewBag.List = TablesContainer.list7 = db.Privacy_Breaches.Where(l => l.Location == Id_Location).ToList(); }
                { ViewBag.List = TablesContainer.list8 = db.Privacy_Complaints.Where(l => l.Location == Id_Location).ToList(); }
                { ViewBag.List = TablesContainer.list9 = db.Educations.Where(l => l.Location == Id_Location).ToList(); }
                { ViewBag.List = TablesContainer.list10 = db.Relations.Where(l => l.Location == Id_Location).ToList(); }
                { ViewBag.List = TablesContainer.list13 = db.WSIBs.Where(l => l.Location == Id_Location).ToList(); }
                { ViewBag.List = TablesContainer.list14 = db.Not_WSIBs.Where(l => l.Location == Id_Location).ToList(); }
            }
            if (b)
            {
                switch (num_tbl)
                {
                    case 1:
                        var arr1 = TablesContainer.COUNT;

                        {
                            ViewBag.Count = arr1;
                        }

                        {
                            ViewBag.GN_Found = foundSummary1;
                        }

                        {
                            ViewBag.ObjName = "Critical Incidents";
                        }

                        {
                            ViewBag.Entity = "Critical_Incidents";
                        }

                        { ViewBag.IsAdmin = isAdmin; }
                        { ViewBag.TotalSummary = allSummary1; }
                        { ViewBag.Check1 = isEmpty; }
                        { ViewBag.Check = checkView; }

                        {
                            ViewBag.Locations = locList;
                        }

                        { ViewBag.EmptLocation = b; }

                        {
                            if (role == Role.Admin) ViewBag.LocInfo = "All";
                            else ViewBag.LocInfo = db.Care_Communities.Find(Id_Location).Name;
                        }

                        tabs = new WorTabs();
                        tabs.ListForms = GetFormNames();

                        return View(tabs);
                    case 2:
                        var arr2 = TablesContainer.COUNT;
                        {
                            ViewBag.Count = arr2;
                        }

                        {
                            ViewBag.GN_Found = strN;
                        }

                        {
                            ViewBag.ObjName = "Complaints";
                        }

                        {
                            ViewBag.Entity = "Complaints";
                        }

                        { ViewBag.TotalSummary = allSummary1; }
                        { ViewBag.Check1 = isEmpty; }
                        { ViewBag.Check = checkView; }

                        {
                            ViewBag.Locations = locList;
                        }

                        { ViewBag.EmptLocation = b; }

                        {
                            if (role == Role.Admin) ViewBag.LocInfo = "All";
                            else ViewBag.LocInfo = db.Care_Communities.Find(Id_Location).Name;
                        }
                        ViewBag.Check = checkView;
                        tabs = new WorTabs();
                        tabs.ListForms = GetFormNames();
                        return View(tabs);
                    case 3:

                        {
                            ViewBag.Count = TablesContainer.COUNT;
                        }

                        {
                            ViewBag.GN_Found = strN;
                        }

                        {
                            ViewBag.ObjName = "Good News";
                        }

                        {
                            ViewBag.Entity = "Good_News";
                        }
                        {
                            if (role == Role.Admin) ViewBag.LocInfo = "All";
                            else ViewBag.LocInfo = db.Care_Communities.Find(Id_Location).Name;
                        }
                        /// for other statistic details...
                        ViewBag.Check = checkView;
                        tabs = new WorTabs();
                        tabs.ListForms = GetFormNames();
                        return View(tabs);
                    case 4:

                        {
                            ViewBag.Count = TablesContainer.COUNT;
                        }

                        {
                            ViewBag.GN_Found = strN;
                        }

                        {
                            ViewBag.ObjName = "Emergency Prep";
                        }

                        {
                            ViewBag.Entity = "Emergency_Prep";
                        }
                        {
                            if (role == Role.Admin) ViewBag.LocInfo = "All";
                            else ViewBag.LocInfo = db.Care_Communities.Find(Id_Location).Name;
                        }
                        /// for other statistic details...
                        ViewBag.Check = checkView;
                        tabs = new WorTabs();
                        tabs.ListForms = GetFormNames();
                        return View(tabs);
                    case 6:

                        {
                            ViewBag.Count = TablesContainer.COUNT;
                        }

                        {
                            ViewBag.GN_Found = strN;
                        }

                        {
                            ViewBag.ObjName = "Visits_Others";
                        }

                        {
                            ViewBag.Entity = "Visits_Others";
                        }
                        {
                            if (role == Role.Admin) ViewBag.LocInfo = "All";
                            else ViewBag.LocInfo = db.Care_Communities.Find(Id_Location).Name;
                        }
                        ViewBag.Check = checkView;
                        tabs = new WorTabs();
                        tabs.ListForms = GetFormNames();
                        return View(tabs);
                    case 5:

                        {
                            ViewBag.Count = TablesContainer.COUNT;
                        }

                        {
                            ViewBag.GN_Found = strN;
                        }

                        {
                            ViewBag.ObjName = "Community_Risks";
                        }

                        {
                            ViewBag.Entity = "Community_Risks";
                        }
                        {
                            if (role == Role.Admin) ViewBag.LocInfo = "All";
                            else ViewBag.LocInfo = db.Care_Communities.Find(Id_Location).Name;
                        }
                        ViewBag.Check = checkView;
                        tabs = new WorTabs();
                        tabs.ListForms = GetFormNames();
                        return View(tabs);
                    case 7:

                        {
                            ViewBag.Count = TablesContainer.COUNT;
                        }

                        {
                            ViewBag.GN_Found = strN;
                        }

                        {
                            ViewBag.ObjName = "Privacy_Breaches";
                        }

                        {
                            ViewBag.Entity = "Privacy_Breaches";
                        }
                        {
                            if (role == Role.Admin) ViewBag.LocInfo = "All";
                            else ViewBag.LocInfo = db.Care_Communities.Find(Id_Location).Name;
                        }
                        ViewBag.Check = checkView;
                        tabs = new WorTabs();
                        tabs.ListForms = GetFormNames();
                        return View(tabs);
                    case 8:

                        {
                            ViewBag.Count = TablesContainer.COUNT;
                        }

                        {
                            ViewBag.GN_Found = strN;
                        }

                        {
                            ViewBag.ObjName = "Privacy_Complaints";
                        }

                        {
                            ViewBag.Entity = "Privacy_Complaints";
                        }
                        {
                            if (role == Role.Admin) ViewBag.LocInfo = "All";
                            else ViewBag.LocInfo = db.Care_Communities.Find(Id_Location).Name;
                        }
                        ViewBag.Check = checkView;
                        tabs = new WorTabs();
                        tabs.ListForms = GetFormNames();
                        return View(tabs);
                    case 10:

                        {
                            ViewBag.Count = TablesContainer.COUNT;
                        }

                        {
                            ViewBag.GN_Found = strN;
                        }

                        {
                            ViewBag.ObjName = "Labour_Relations";
                        }

                        {
                            ViewBag.Entity = "Labour_Relations";
                        }
                        {
                            if (role == Role.Admin) ViewBag.LocInfo = "All";
                            else ViewBag.LocInfo = db.Care_Communities.Find(Id_Location).Name;
                        }
                        ViewBag.Check = checkView;
                        tabs = new WorTabs();
                        tabs.ListForms = GetFormNames();
                        return View(tabs);
                    case 13:

                        {
                            ViewBag.Count = TablesContainer.COUNT;
                        }

                        {
                            ViewBag.GN_Found = strN;
                        }

                        {
                            ViewBag.ObjName = "WSIB";
                        }

                        {
                            ViewBag.Entity = "WSIB";
                        }
                        {
                            if (role == Role.Admin) ViewBag.LocInfo = "All";
                            else ViewBag.LocInfo = db.Care_Communities.Find(Id_Location).Name;
                        }
                        ViewBag.Check = checkView;
                        tabs = new WorTabs();
                        tabs.ListForms = GetFormNames();
                        return View(tabs);
                    case 14:

                        {
                            ViewBag.Count = TablesContainer.COUNT;
                        }

                        {
                            ViewBag.GN_Found = strN;
                        }

                        {
                            ViewBag.ObjName = "Not_WSIB";
                        }

                        {
                            ViewBag.Entity = "Not_WSIB";
                        }
                        {
                            if (role == Role.Admin) ViewBag.LocInfo = "All";
                            else ViewBag.LocInfo = db.Care_Communities.Find(Id_Location).Name;
                        }
                        ViewBag.Check = checkView;
                        tabs = new WorTabs();
                        tabs.ListForms = GetFormNames();
                        return View(tabs);

                        /// for other statistic details...
                }
            }

            tabs = new WorTabs();
            tabs.ListForms = GetFormNames();
            return View(tabs);
        }
        #endregion

        #region WOR Tabs(Post):
        static string model_name, rememberFormName = string.Empty, w_without = string.Empty;
        static string radioNameForms = string.Empty;
        static bool isEmpty = false, checkRepead = false;
        static List<string> locList = new List<string>();
        static List<CriticalIncidentSummary> foundSummary1 = new List<CriticalIncidentSummary>();
        static List<ComplaintsSummary> foundSummary2 = new List<ComplaintsSummary>();
        static List<Critical_Incidents> ll1, ll2, ll3, ll4, ll5, ll6, ll7, ll8, ll9, ll10, ll11;
        static int p1 = 0, p2 = 0, p3 = 0, p4 = 0, p5 = 0, p6 = 0, p7 = 0, p8 = 0, p9 = 0, p10 = 0, p11 = 0, p12 = 0, p13 = 0,
            p14 = 0, p15 = 0, p16 = 0, p17 = 0, p18 = 0, p19 = 0, p20 = 0, p21 = 0, p22 = 0, p23 = 0, p24 = 0, p25 = 0, p26 = 0, p27 = 0;
        List<int> cnt = new List<int>();
        int cnt1 = 0, cnt2 = 0, cnt3 = 0, cnt4 = 0, cnt5 = 0, cnt6 = 0, cnt7 = 0, cnt8 = 0, cnt9 = 0, cnt10 = 0, cnt11 = 0;
        static string radioName = null, mirrorWout = null;
        [HttpPost]
        public ActionResult WOR_Tabs(WorTabs Value)
        {
            ViewBag.AllCI = db.CI_Category_Types.ToList();
            ViewBag.AllLocs = db.Care_Communities.ToList();
            try
            {
                w_without = Request.Form["range"];
                if (w_without == null)
                    w_without = mirrorWout;

                if (radioNameForms == string.Empty)
                    radioNameForms = Request.Form["formRadio"];
                Value.Name = radioNameForms;
            }
            catch { }
            ViewBag.ListCI = list3;
            if (role == Role.Admin)
                ViewBag.Welcome = Role.Admin;
            else if (role == Role.User)
                ViewBag.Welcome = Role.User;
            ViewBag.IsAdmin = isAdmin;
            DateTime start = DateTime.MinValue, end = DateTime.MinValue;
            string errorMsg = string.Empty;
            // With Range:
            if (Value != null && Value.Name != null && w_without != "-without"/* && w_without != "-filter"*/)  // If we select anythng from the listbox
            {
                string btnName = Request.Params
                      .Cast<string>()
                      .Where(p => p.StartsWith("btn"))
                      .Select(p => p.Substring("btn".Length))
                      .First();

                #region For Showing List (with Range):
                if (btnName.Equals("-list") || btnName.Equals("-upSort") || btnName.Equals("-downSort"))
                {
                    {
                        ViewBag.Check = "list";
                    }
                    checkView = "list";
                    {
                        ViewBag.Tbl = Value.Name;
                    }
                    ViewBag.Check = checkView;
                    string name = Value.Name;

                    start = Value.Start;
                    end = Value.End;
                    if (start != DateTime.MinValue && end != DateTime.MinValue)
                    {
                        if (role == Role.Admin)
                        {
                            ViewBag.Welcome = Role.Admin;
                            switch (Value.Name)
                            {
                                case "1":
                                    List<Critical_Incidents> lst1 = (from c in db.Critical_Incidents where c.Date >=  start && c.Date <= end select c).ToList();
                                    TablesContainer.list1 = lst1.OrderBy(x => x.Date).ToList();
                                    if (btnName.Equals("-upSort"))
                                    {
                                        TablesContainer.list1 = lst1.OrderBy(x => x.Date).ToList();
                                    }
                                    else if (btnName.Equals("-downSort"))
                                    {
                                        TablesContainer.list1 = lst1.OrderByDescending(x => x.Date).ToList();
                                    }
                                    if (TablesContainer.list1.Count != 0 || TablesContainer.list1 != null)
                                    {
                                        { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                        { ViewBag.CiNames = STREAM.GetCINames().ToArray(); }
                                        ViewBag.List = TablesContainer.list1;
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                case "2":
                                    ViewBag.Department = list20;
                                    var lst2 = (from ent in db.Complaints where ent.DateReceived >= start && ent.DateReceived <= end select ent).ToList();
                                    TablesContainer.list2 = lst2.OrderBy(x => x.DateReceived).ToList();
                                    if (TablesContainer.list2.Count != 0 || TablesContainer.list2 != null)
                                    {
                                        { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                        ViewBag.List = TablesContainer.list2;
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                case "3":  // Good_News table\
                                    var lst3 = (from ent in db.Good_News where ent.DateNews >= start && ent.DateNews <= end select ent).ToList();
                                    TablesContainer.list3 = lst3.OrderBy(x => x.DateNews).ToList();
                                    if (TablesContainer.list3.Count != 0 || TablesContainer.list3 != null)
                                    {
                                        { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                        ViewBag.List = TablesContainer.list3;
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                case "4":
                                    var lst4 = db.Emergency_Prep.ToList();

                                    if (TablesContainer.list3.Count != 0 || TablesContainer.list3 != null)
                                    {
                                        { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                        ViewBag.List = lst4;
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                case "5":
                                    var lst5 = db.Community_Risks.ToList();
                                    TablesContainer.list5 = (from ent in lst5 where ent.Date >= start && ent.Date <= end select ent).ToList();
                                    if (TablesContainer.list5.Count != 0 || TablesContainer.list5 != null)
                                    {
                                        { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                        ViewBag.List = TablesContainer.list5;
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                case "6":
                                    var lst6 = db.Visits_Others.ToList();
                                    TablesContainer.list6 = (from ent in lst6 where ent.Date_of_Visit >= start && ent.Date_of_Visit <= end select ent).ToList();
                                    if (TablesContainer.list6.Count != 0 || TablesContainer.list6 != null)
                                    {
                                        { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                        ViewBag.List = TablesContainer.list6;
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                case "7":
                                    var lst7 = db.Privacy_Breaches.ToList();
                                    TablesContainer.list7 = (from ent in lst7 where ent.Date_Breach_Occurred >= start && ent.Date_Breach_Occurred <= end select ent).ToList();
                                    if (TablesContainer.list7.Count != 0 || TablesContainer.list7 != null)
                                    {
                                        { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                        ViewBag.List = TablesContainer.list7;
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                case "8":
                                    var lst8 = db.Privacy_Complaints.ToList();
                                    TablesContainer.list8 = (from ent in lst8 where ent.Date_Complain_Received >= start && ent.Date_Complain_Received <= end select ent).ToList();
                                    if (TablesContainer.list8.Count != 0 || TablesContainer.list8 != null)
                                    {
                                        { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                        ViewBag.List = TablesContainer.list8;
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                case "9":
                                    TablesContainer.list9 = db.Educations.ToList();
                                    if (TablesContainer.list9.Count != 0 || TablesContainer.list9 != null)
                                    {
                                        { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                        ViewBag.List = TablesContainer.list9;
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                case "10":
                                    var lst10 = db.Relations.ToList();
                                    TablesContainer.list10 = (from ent in lst10 where ent.Date >= start && ent.Date <= end select ent).ToList();
                                    if (TablesContainer.list10.Count != 0 || TablesContainer.list11 != null)
                                    {
                                        { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                        ViewBag.List = TablesContainer.list10;
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                case "11":
                                    TablesContainer.list11 = db.Immunizations.ToList();
                                    if (TablesContainer.list11.Count != 0 || TablesContainer.list11 != null)
                                    {
                                        { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                        ViewBag.List = TablesContainer.list11;
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                case "12":
                                    TablesContainer.list12 = db.Outbreaks.ToList();
                                    if (TablesContainer.list12.Count != 0 || TablesContainer.list12 != null)
                                    {
                                        { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                        ViewBag.List = TablesContainer.list12;
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                case "13":
                                    TablesContainer.list13 = db.WSIBs.ToList();
                                    if (TablesContainer.list13.Count != 0 || TablesContainer.list13 != null)
                                    {
                                        { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                        ViewBag.List = TablesContainer.list13;
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                case "14":
                                    TablesContainer.list14 = db.Not_WSIBs.ToList();
                                    if (TablesContainer.list14.Count != 0 || TablesContainer.list14 != null)
                                    {
                                        { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                        ViewBag.List = TablesContainer.list14;
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                            }
                        }
                        else if (role == Role.User)
                        {
                            ViewBag.Welcome = Role.User;
                            switch (Value.Name)
                            {
                                case "1":
                                    var lstLoc = db.Critical_Incidents.Where(x => x.Location == Id_Location);
                                    List<Critical_Incidents> lst1 = (from ent in lstLoc where ent.Date >= start && ent.Date <= end select ent).ToList();
                                    TablesContainer.list1 = lst1.OrderBy(x => x.Date).ToList();
                                    if (btnName.Equals("-upSort"))
                                    {
                                        TablesContainer.list1 = lst1.OrderBy(x => x.Date).ToList();
                                    }
                                    else if (btnName.Equals("-downSort"))
                                    {
                                        TablesContainer.list1 = lst1.OrderByDescending(x => x.Date).ToList();
                                    }
                                    if (TablesContainer.list1.Count != 0 || TablesContainer.list1 != null)
                                    {
                                        { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                        { ViewBag.CiNames = STREAM.GetCINames().ToArray(); }
                                        ViewBag.List = TablesContainer.list1;
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();

                                        return View(tabs);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                case "2":
                                    var lst2 = db.Complaints.Where(i => i.Location == Id_Location);
                                    TablesContainer.list2 = (from ent in lst2 where ent.DateReceived >= start && ent.DateReceived <= end select ent).ToList();
                                    if (TablesContainer.list2.Count != 0 || TablesContainer.list2 != null)
                                    {
                                        { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                        ViewBag.List = TablesContainer.list2;
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                case "3":  // Good_News table\
                                    var lst3 = db.Good_News.Where(i => i.Location == Id_Location).ToList();
                                    TablesContainer.list3 = (from ent in lst3 where ent.DateNews >= start && ent.DateNews <= end select ent).ToList();
                                    if (TablesContainer.list3.Count != 0 || TablesContainer.list3 != null)
                                    {
                                        { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                        ViewBag.List = TablesContainer.list3;
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                case "4":
                                    var lst4 = db.Emergency_Prep.Where(i => i.Location == Id_Location).ToList();

                                    if (TablesContainer.list3.Count != 0 || TablesContainer.list3 != null)
                                    {
                                        { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                        ViewBag.List = lst4;
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                case "5":
                                    var lst5 = db.Community_Risks.Where(i => i.Location == Id_Location).ToList();
                                    TablesContainer.list5 = (from ent in lst5 where ent.Date >= start && ent.Date <= end select ent).ToList();
                                    if (TablesContainer.list5.Count != 0 || TablesContainer.list5 != null)
                                    {
                                        { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                        ViewBag.List = TablesContainer.list5;
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                case "6":
                                    var lst6 = db.Visits_Others.Where(i => i.Location == Id_Location).ToList();
                                    TablesContainer.list6 = (from ent in lst6 where ent.Date_of_Visit >= start && ent.Date_of_Visit <= end select ent).ToList();
                                    if (TablesContainer.list6.Count != 0 || TablesContainer.list6 != null)
                                    {
                                        { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                        ViewBag.List = TablesContainer.list6;
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                case "7":
                                    var lst7 = db.Privacy_Breaches.Where(i => i.Location == Id_Location).ToList();
                                    TablesContainer.list7 = (from ent in lst7 where ent.Date_Breach_Occurred >= start && ent.Date_Breach_Occurred <= end select ent).ToList();
                                    if (TablesContainer.list7.Count != 0 || TablesContainer.list7 != null)
                                    {
                                        { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                        ViewBag.List = TablesContainer.list7;
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                case "8":
                                    var lst8 = db.Privacy_Complaints.Where(i => i.Location == Id_Location).ToList();
                                    TablesContainer.list8 = (from ent in lst8 where ent.Date_Complain_Received >= start && ent.Date_Complain_Received <= end select ent).ToList();
                                    if (TablesContainer.list8.Count != 0 || TablesContainer.list8 != null)
                                    {
                                        { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                        ViewBag.List = TablesContainer.list8;
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                case "9":
                                    TablesContainer.list9 = db.Educations.Where(i => i.Location == Id_Location).ToList();
                                    if (TablesContainer.list9.Count != 0 || TablesContainer.list9 != null)
                                    {
                                        { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                        ViewBag.List = TablesContainer.list9;
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                case "10":
                                    var lst10 = db.Relations.Where(i => i.Location == Id_Location).ToList();
                                    TablesContainer.list10 = (from ent in lst10 where ent.Date >= start && ent.Date <= end select ent).ToList();
                                    if (TablesContainer.list10.Count != 0 || TablesContainer.list11 != null)
                                    {
                                        { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                        ViewBag.List = TablesContainer.list10;
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                case "11":
                                    TablesContainer.list11 = db.Immunizations.Where(i => i.Location == Id_Location).ToList();
                                    if (TablesContainer.list11.Count != 0 || TablesContainer.list11 != null)
                                    {
                                        { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                        ViewBag.List = TablesContainer.list11;
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                case "12":
                                    TablesContainer.list12 = db.Outbreaks.Where(i => i.Location == Id_Location).ToList();
                                    if (TablesContainer.list12.Count != 0 || TablesContainer.list12 != null)
                                    {
                                        { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                        ViewBag.List = TablesContainer.list12;
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                case "13":
                                    TablesContainer.list13 = db.WSIBs.Where(i => i.Location == Id_Location).ToList();
                                    if (TablesContainer.list13.Count != 0 || TablesContainer.list13 != null)
                                    {
                                        { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                        ViewBag.List = TablesContainer.list13;
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                case "14":
                                    TablesContainer.list14 = db.Not_WSIBs.Where(i => i.Location == Id_Location).ToList();
                                    if (TablesContainer.list14.Count != 0 || TablesContainer.list14 != null)
                                    {
                                        { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                        ViewBag.List = TablesContainer.list14;
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                            }
                        }
                    }
                    else
                    {
                        if (Id_Location == 0)
                        {
                            switch (Value.Name)
                            {
                                case "1":
                                    List<Critical_Incidents> lst1 = db.Critical_Incidents.ToList();
                                    TablesContainer.list1 = lst1.OrderBy(x => x.Date).ToList();
                                    if (btnName.Equals("-upSort"))
                                    {
                                        TablesContainer.list1 = lst1.OrderBy(x => x.Date).ToList();
                                    }
                                    else if (btnName.Equals("-downSort"))
                                    {
                                        TablesContainer.list1 = lst1.OrderByDescending(x => x.Date).ToList();
                                    }
                                    if (TablesContainer.list1.Count != 0 || TablesContainer.list1 != null)
                                    {
                                        { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                        ViewBag.List = TablesContainer.list1;
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                case "2":
                                    var lst2 = db.Complaints.Where(i => i.Location == Id_Location);
                                    var var = TablesContainer.list2 = (from ent in lst2 where ent.DateReceived >= start && ent.DateReceived <= end select ent).ToList();
                                    if (var == null)
                                    {
                                        ViewBag.List = "The table is EMPTY!";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    if (TablesContainer.list2.Count != 0 || TablesContainer.list2 != null)
                                    {
                                        { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                        ViewBag.List = TablesContainer.list2;
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                case "3":  // Good_News table
                                    var lst3 = db.Good_News.Where(i => i.Location == Id_Location);
                                    TablesContainer.list3 = (from ent in lst3 where ent.DateNews >= start && ent.DateNews <= end select ent).ToList();
                                    if (TablesContainer.list3.Count != 0 || TablesContainer.list3 != null)
                                    {
                                        { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                        ViewBag.List = TablesContainer.list3;
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                            }
                        }
                        else
                        {
                            switch (Value.Name)
                            {
                                case "1": // CriticalIncidents tbl
                                    var lst1 = db.Critical_Incidents.Where(i => i.Location == Id_Location);
                                    TablesContainer.list1 = (from ent in lst1 where ent.Date >= start && ent.Date <= end select ent).ToList();
                                    TablesContainer.list1 = lst1.OrderBy(x => x.Date).ToList();
                                    if (btnName.Equals("-upSort"))
                                    {
                                        TablesContainer.list1 = lst1.OrderBy(x => x.Date).ToList();
                                    }
                                    else if (btnName.Equals("-downSort"))
                                    {
                                        TablesContainer.list1 = lst1.OrderByDescending(x => x.Date).ToList();
                                    }
                                    if (TablesContainer.list1.Count != 0 || TablesContainer.list1 != null)
                                    {
                                        { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                        ViewBag.List = TablesContainer.list1;
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                case "2": // Complients tbl
                                    var lst2 = db.Complaints.Where(i => i.Location == Id_Location);
                                    TablesContainer.list2 = (from ent in lst2 where ent.DateReceived >= start && ent.DateReceived <= end select ent).ToList();
                                    if (TablesContainer.list2.Count != 0)
                                    {
                                        { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                        ViewBag.List = TablesContainer.list2;
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                case "3":  // Good_News table
                                    var lst3 = db.Good_News.Where(i => i.Location == Id_Location);
                                    TablesContainer.list3 = (from ent in lst3 where ent.DateNews >= start && ent.DateNews <= end select ent).ToList();
                                    if (TablesContainer.list3.Count != 0 || TablesContainer.list3 != null)
                                    {
                                        { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                        ViewBag.List = TablesContainer.list3;
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                            }
                        }
                    }
                }
                #endregion

                #region For Inserted (with Range):
                else if (btnName.Equals("-insert"))
                {
                    checkView = "insert";
                    ViewBag.Check = checkView;
                    int id = int.Parse(Value.Name);
                    return RedirectToAction($"../Home/GoToSelectForm/{id}");
                }
                #endregion

                #region For Export to .csv file (with Range):
                else if (btnName.Equals("-export"))
                {
                    start = Value.Start;
                    end = Value.End;
                    if (start != DateTime.MinValue && end != DateTime.MinValue && radioName != "-without")
                    {
                        //var query1 = (from ent in db.Good_News where ent.DateNews >= start && ent.DateNews <= end select ent).ToList();
                        int id = int.Parse(Value.Name);
                        var tbl_list = GetTableById(id).ToArray().ToList();
                        Type type = tbl_list[0].GetType();
                        string entity = type.Name;
                        object model = Searcher.FindObjByName(entity);
                        if (model.GetType() == typeof(Critical_Incidents))
                        {
                            List<Critical_Incidents> lst1 = default;
                            model_name = model.GetType().Name;
                            if(role == Role.Admin)
                            {
                                lst1 = db.Critical_Incidents.ToList();
                                TablesContainer.list1 = (from ent in lst1 where ent.Date >= start && ent.Date <= end select ent).ToList();
                            }
                            else
                            {
                                lst1 = db.Critical_Incidents.Where(i => i.Location == Id_Location).ToList();
                                TablesContainer.list1 = (from ent in lst1 where ent.Date >= start && ent.Date <= end select ent).ToList();
                            }
                            
                            // new STREAM().WriteToCSV(query1); // to be continue..
                            return RedirectToAction("../Home/ExportToCSV");
                        }
                        else if (model.GetType() == typeof(Good_News))
                        {
                            model_name = model.GetType().Name;
                            var lst3 = db.Good_News.Where(i => i.Location == Id_Location);
                            TablesContainer.list3 = (from ent in lst3 where ent.DateNews >= start && ent.DateNews <= end select ent).ToList();
                            return RedirectToAction("../Home/ExportToCSV");
                        }
                        else if (model.GetType() == typeof(Complaint))
                        {
                            model_name = model.GetType().Name;
                            var lst1 = db.Complaints.Where(i => i.Location == Id_Location);
                            TablesContainer.list2 = (from ent in lst1 where ent.DateReceived >= start && ent.DateReceived <= end select ent).ToList();
                            return RedirectToAction("../Home/ExportToCSV");
                        }
                        else if (model.GetType() == typeof(Community_Risks))
                        {
                            model_name = model.GetType().Name;
                            var lst1 = db.Community_Risks.Where(i => i.Location == Id_Location);
                            TablesContainer.list5 = (from ent in lst1 where ent.Date >= start && ent.Date <= end select ent).ToList();
                            return RedirectToAction("../Home/ExportToCSV");
                        }
                        else if (model.GetType() == typeof(Labour_Relations))
                        {
                            model_name = model.GetType().Name;
                            var lst1 = db.Relations.Where(i => i.Location == Id_Location);
                            TablesContainer.list10 = (from ent in lst1 where ent.Date >= start && ent.Date <= end select ent).ToList();
                            return RedirectToAction("../Home/ExportToCSV");
                        }
                        else if (model.GetType() == typeof(Emergency_Prep))
                        {
                            model_name = model.GetType().Name;
                            TablesContainer.list4 = db.Emergency_Prep.Where(i => i.Location == Id_Location).ToList();
                            return RedirectToAction("../Home/ExportToCSV");
                        }
                        else if (model.GetType() == typeof(Visits_Others))
                        {
                            model_name = model.GetType().Name;
                            TablesContainer.list6 = db.Visits_Others.Where(i => i.Location == Id_Location).ToList();
                            return RedirectToAction("../Home/ExportToCSV");
                        }
                        else if (model.GetType() == typeof(Privacy_Breaches))
                        {
                            model_name = model.GetType().Name;
                            TablesContainer.list7 = db.Privacy_Breaches.Where(i => i.Location == Id_Location).ToList();
                            return RedirectToAction("../Home/ExportToCSV");
                        }
                        else if (model.GetType() == typeof(Privacy_Complaints))
                        {
                            model_name = model.GetType().Name;
                            TablesContainer.list8 = db.Privacy_Complaints.Where(i => i.Location == Id_Location).ToList();
                            return RedirectToAction("../Home/ExportToCSV");
                        }
                        else if (model.GetType() == typeof(Education))
                        {
                            model_name = model.GetType().Name;
                            TablesContainer.list9 = db.Educations.Where(i => i.Location == Id_Location).ToList();
                            return RedirectToAction("../Home/ExportToCSV");
                        }
                        else if (model.GetType() == typeof(Labour_Relations))
                        {
                            model_name = model.GetType().Name;
                            TablesContainer.list10 = db.Relations.Where(i => i.Location == Id_Location).ToList();
                            return RedirectToAction("../Home/ExportToCSV");
                        }
                        else if (model.GetType() == typeof(Immunization))
                        {
                            model_name = model.GetType().Name;
                            TablesContainer.list11 = db.Immunizations.Where(i => i.Location == Id_Location).ToList();
                            return RedirectToAction("../Home/ExportToCSV");
                        }
                        else if (model.GetType() == typeof(Outbreaks))
                        {
                            model_name = model.GetType().Name;
                            TablesContainer.list12 = db.Outbreaks.Where(i => i.Location == Id_Location).ToList();
                            return RedirectToAction("../Home/ExportToCSV");
                        }
                        else if (model.GetType() == typeof(WSIB))
                        {
                            model_name = model.GetType().Name;
                            TablesContainer.list13 = db.WSIBs.Where(i => i.Location == Id_Location).ToList();
                            return RedirectToAction("../Home/ExportToCSV");
                        }
                        else if (model.GetType() == typeof(Not_WSIBs))
                        {
                            model_name = model.GetType().Name;
                            TablesContainer.list14 = db.Not_WSIBs.Where(i => i.Location == Id_Location).ToList();
                            return RedirectToAction("../Home/ExportToCSV");
                        }
                        else
                        {
                            ViewBag.ErrorMsg = errorMsg = "There was nothing found within the date range that was chosen.";
                            WorTabs tabs = new WorTabs();
                            tabs.ListForms = GetFormNames();
                            return View(tabs);
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMsg = errorMsg = "Please choose the dates from the list";
                        WorTabs tabs = new WorTabs();
                        tabs.ListForms = GetFormNames();
                        return View(tabs);
                    }
                }
                #endregion

                #region For Summary (with Range):
                else if (btnName.Equals("-summary"))
                {
                    checkView = "summary";
                    ViewBag.Check = checkView;
                    start = Value.Start;
                    end = Value.End;
                    int id = num_tbl = int.Parse(Value.Name);
                    if (id == 11) Id_Location = 1;
                    var tbl_list = GetTableById(id).ToArray().ToList();
                    Type type = tbl_list[0].GetType();
                    string entity = type.Name;
                    int cnt1 = 0, cnt2 = 0, cnt3 = 0, cnt4 = 0, cnt5 = 0, cnt6 = 0, cnt7 = 0, cnt8 = 0, cnt9 = 0, cnt10 = 0, cnt11 = 0;
                    if (!entity.Equals(string.Empty))
                    {
                        ViewBag.TableName = entity;
                    }
                    if (start == DateTime.MinValue && end == DateTime.MinValue)
                    {
                        ViewBag.ErrorMsg = "No date range was chosen. Please choose the date range.";
                        WorTabs tabs = new WorTabs();
                        tabs.ListForms = GetFormNames();
                        return View(tabs);
                    }
                    else if (start == DateTime.MinValue || end == DateTime.MinValue)
                    {
                        if (start == DateTime.MinValue)
                        {
                            ViewBag.ErrorMsg = "No date 'Start' was chosen. Please choose the 'Start' date.";
                            WorTabs tabs = new WorTabs();
                            tabs.ListForms = GetFormNames();
                            return View(tabs);
                        }
                        else
                        {
                            ViewBag.ErrorMsg = "No date 'End' was chosen. Please choose the 'End' date.";
                            WorTabs tabs = new WorTabs();
                            tabs.ListForms = GetFormNames();
                            return View(tabs);
                        }
                    }
                    else
                    {
                        #region Switch to show all object's Statistic:
                        switch (entity)
                        {
                            #region Critical_Incident:
                            case "Critical_Incidents":
                                ClearAllStatic();
                                List<Critical_Incidents> lst_locat1 = null;
                                if (role == Role.Admin) { lst_locat1 = db.Critical_Incidents.ToList(); }
                                else if (role == Role.User) 
                                    lst_locat1 = db.Critical_Incidents.Where(i => i.Location == Id_Location).ToList();
                                TablesContainer.list1 = (from ent in lst_locat1 where ent.Date >= start && ent.Date <= end select ent).ToList();
                                if (TablesContainer.list1.Count() == 0)
                                {
                                    { ViewBag.ObjName = entity; }
                                    ViewBag.ErrorMsg = errorMsg = "Please select a date range.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                                List<int> cnt = new List<int>();

                             
                                    foreach (var cc in TablesContainer.list1)
                                    {
                                        if (STREAM.GetLocNameById(cc.Location).Contains("Altamont Care Community"))
                                            cnt1++;
                                        else if (STREAM.GetLocNameById(cc.Location).Contains("Astoria Retirement Residence"))
                                            cnt2++;
                                        else if (STREAM.GetLocNameById(cc.Location).Contains("Barnswallow Place Care Community"))
                                            cnt3++;
                                        else if (STREAM.GetLocNameById(cc.Location).Contains("Bearbrook Retirement Residence"))
                                            cnt4++;
                                        else if (STREAM.GetLocNameById(cc.Location).Contains("Bloomington Cove Care Community"))
                                            cnt5++;
                                        else if (STREAM.GetLocNameById(cc.Location).Contains("Bradford Valley Care Community"))
                                            cnt6++;
                                        else if (STREAM.GetLocNameById(cc.Location).Contains("Brookside Lodge"))
                                            cnt7++;
                                        else if (STREAM.GetLocNameById(cc.Location).Contains("Woodbridge Vista"))
                                            cnt8++;
                                        else if (STREAM.GetLocNameById(cc.Location).Contains("Norfinch"))
                                            cnt9++;
                                        else if (STREAM.GetLocNameById(cc.Location).Contains("Rideau"))
                                            cnt10++;
                                        else if (STREAM.GetLocNameById(cc.Location).Contains("Villa da Vinci"))
                                            cnt11++;
                                    }

                                    CriticalIncidentSummary model = new CriticalIncidentSummary();
                                    var locDistinct = new HashSet<string>();
                                    var locId = new List<int>();
                                    foreach (var it in TablesContainer.list1)
                                    {
                                        Care_Community cc = db.Care_Communities.Find(it.Location);
                                        locDistinct.Add(cc.Name);
                                        locId.Add(cc.Id);
                                    }

                                    locList = locDistinct.ToList();

                                    #region Fill out lists ll1,ll2,ll3...ll11 existing locations:
                                    for (var i = 0; i < locList.Count; i++)
                                    {
                                        if (locList[i].Contains("Altamont Care Community"))
                                        {
                                            ll1 = TablesContainer.list1.Where
                                         (loc => STREAM.GetLocNameById(loc.Location) == "Altamont Care Community\r\n").ToList();
                                        }
                                        else if (locList[i].Contains("Astoria Retirement Residence"))
                                        {
                                            ll2 = TablesContainer.list1.Where
                                               (loc => STREAM.GetLocNameById(loc.Location) == "Astoria Retirement Residence\r\n").ToList();
                                        }
                                        else if (locList[i].Contains("Barnswallow Place Care Community"))
                                        {
                                            ll3 = TablesContainer.list1.Where
                                          (loc => STREAM.GetLocNameById(loc.Location) == "Barnswallow Place Care Community\r\n").ToList();
                                        }
                                        else if (locList[i].Contains("Bearbrook Retirement Residence"))
                                        {
                                            ll4 = TablesContainer.list1.Where
                                          (loc => STREAM.GetLocNameById(loc.Location) == "Bearbrook Retirement Residence\r\n").ToList();
                                        }
                                        else if (locList[i].Contains("Bloomington Cove Care Community"))
                                        {
                                            ll5 = TablesContainer.list1.Where
                                           (loc => STREAM.GetLocNameById(loc.Location) == "Bloomington Cove Care Community\r\n").ToList();
                                        }
                                        else if (locList[i].Contains("Bradford Valley Care Community"))
                                        {
                                            ll6 = TablesContainer.list1.Where
                                            (loc => STREAM.GetLocNameById(loc.Location) == "Bradford Valley Care Community\r\n").ToList();
                                        }
                                        else if (locList[i].Contains("Brookside Lodge"))
                                        {
                                            ll7 = TablesContainer.list1.Where
                                           (loc => STREAM.GetLocNameById(loc.Location) == "Brookside Lodge\r\n").ToList();
                                        }
                                        else if (locList[i].Contains("Woodbridge Vista"))
                                        {
                                            string retName = STREAM.GetLocNameById(8);
                                            ll8 = TablesContainer.list1.Where
                                            (loc => STREAM.GetLocNameById(loc.Location) == "Woodbridge Vista").ToList();
                                        }
                                        else if (locList[i].Contains("Norfinch"))
                                        {
                                            ll9 = TablesContainer.list1.Where
                                            (loc => STREAM.GetLocNameById(loc.Location) == "Norfinch\r\n").ToList();
                                        }
                                        else if (locList[i].Contains("Rideau"))
                                        {
                                            ll10 = TablesContainer.list1.Where
                                          (loc => STREAM.GetLocNameById(loc.Location) == "Rideau\r\n").ToList();
                                        }
                                        else if (locList[i].Contains("Villa da Vinci"))
                                        {
                                            ll11 = TablesContainer.list1.Where
                                            (loc => STREAM.GetLocNameById(loc.Location) == "Villa da Vinci\r\n").ToList();
                                        }
                                    }
                                    #endregion

                                    #region Add count location for each exist:
                                    for (var i = 0; i < locList.Count; i++)
                                    {
                                        if (locList[i].Contains("Altamont Care Community"))
                                            locList[i] = locList[i] + " - " + cnt1;
                                        else if (locList[i].Contains("Astoria Retirement Residence"))
                                            locList[i] = locList[i] + " - " + cnt2;
                                        else if (locList[i].Contains("Barnswallow Place Care Community"))
                                            locList[i] = locList[i] + " - " + cnt3;
                                        else if (locList[i].Contains("Bearbrook Retirement Residence"))
                                            locList[i] = locList[i] + " - " + cnt4;
                                        else if (locList[i].Contains("Bloomington Cove Care Community"))
                                            locList[i] = locList[i] + " - " + cnt5;
                                        else if (locList[i].Contains("Bradford Valley Care Community"))
                                            locList[i] = locList[i] + " - " + cnt6;
                                        else if (locList[i].Contains("Brookside Lodge"))
                                            locList[i] = locList[i] + " - " + cnt7;
                                        else if (locList[i].Contains("Woodbridge Vista"))
                                            locList[i] = locList[i] + " - " + cnt8;
                                        else if (locList[i].Contains("Norfinch"))
                                            locList[i] = locList[i] + " - " + cnt9;
                                        else if (locList[i].Contains("Rideau"))
                                            locList[i] = locList[i] + " - " + cnt10;
                                        else if (locList[i].Contains("Villa da Vinci"))
                                            locList[i] = locList[i] + " - " + cnt11;
                                    }
                                    #endregion

                                    locList.Sort(); // Sorted by alphanumeric

                                    if (TablesContainer.list1.Count() == 0)
                                    {
                                        { ViewBag.ObjName = entity; }
                                        ViewBag.ErrorMsg = errorMsg = "Please select a date range.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }

                                    TablesContainer.COUNT = TablesContainer.list1.Count;

                                    #region For the 1st Location:
                                    if (ll1 != null)
                                    {
                                        model.LocationName = locList.Find(i => i == "Altamont Care Community\r\n" + " - " + cnt1);
                                        var attr11 = ll1.GroupBy(i => i.MOHLTC_Follow_Up);
                                        if (attr11 != null)
                                        {
                                            foreach (var cc in attr11)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                                if (key == "NULL") continue;
                                                model.MOHLTC_Follow_Up += $"{key}\t - \t{cc.Count()}" + " | ";
                                                p1 += cc.Count();
                                            }
                                        }

                                        var attr10 = ll1.GroupBy(i => i.CIS_Initiated);
                                        if (attr10 != null)
                                        {
                                            foreach (var cc in attr10)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.CIS_Initiated += $"{key}\t - \t{cc.Count()}" + " | "; p2 += cc.Count();
                                            }
                                        }

                                        var attr7 = ll1.GroupBy(i => i.MOH_Notified);
                                        if (attr7 != null)
                                        {
                                            foreach (var cc in attr7)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.MOH_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p3 += cc.Count();
                                            }
                                        }

                                        var attr2 = ll1.GroupBy(i => i.POAS_Notified);
                                        if (attr2 != null)
                                        {
                                            foreach (var d in attr2)
                                            {
                                                string key = d.Key == null ? "NULL" : d.Key;
                                                if (key == "NULL") continue;
                                                model.POAS_Notified += $"{key}\t - \t{d.Count()}" + " | "; p4 += d.Count();
                                            }
                                        }

                                        var attr4 = ll1.GroupBy(i => i.Police_Notified);
                                        if (attr4 != null)
                                        {
                                            foreach (var cc in attr4)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Police_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p5 += cc.Count();
                                            }
                                        }

                                        var attr3 = ll1.GroupBy(i => i.Quality_Improvement_Actions);
                                        if (attr3 != null)
                                        {
                                            foreach (var cc in attr3)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Quality_Improvement_Actions += $"{key}\t - \t{cc.Count()}" + " | "; p6 += cc.Count();
                                            }
                                        }

                                        var attr8 = ll1.GroupBy(i => i.Risk_Locked);
                                        if (attr8 != null)
                                        {
                                            foreach (var cc in attr8)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Risk_Locked += $"{key}\t - \t{cc.Count()}" + " | "; p7 += cc.Count();
                                            }
                                        }

                                        var attr5 = ll1.GroupBy(i => i.Brief_Description);
                                        if (attr5 != null)
                                        {
                                            foreach (var cc in attr5)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Brief_Description += $"{key}\t - \t{cc.Count()}" + " | "; p8 += cc.Count();
                                            }
                                        }

                                        var attr1 = ll1.GroupBy(i => i.Care_Plan_Updated);
                                        if (attr1 != null)
                                        {
                                            foreach (var e in attr1)
                                            {
                                                string key = e.Key == null ? "NULL" : e.Key;
                                                if (key == "NULL") continue;
                                                model.Care_Plan_Updated += $"{key}\t - \t{e.Count()}" + " | "; p9 += e.Count();
                                            }
                                        }

                                        var attr13 = ll1.GroupBy(i => i.CI_Form_Number);
                                        if (attr13 != null)
                                        {
                                            int count = 0;
                                            foreach (var cc in attr13)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                count += cc.Count();
                                            }
                                            model.CI_Form_Number = $"All\t - \t{count}";
                                            p9 += count;
                                        }

                                        var attr00 = ll1.GroupBy(i => i.File_Complete);
                                        if (attr00 != null)
                                        {
                                            foreach (var cc in attr00)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.File_Complete += $"{key}\t - \t{cc.Count()}" + " | "; p11 += cc.Count();
                                            }
                                        }

                                        var attr111 = ll1.GroupBy(i => i.Follow_Up_Amendments);
                                        if (attr111 != null)
                                        {
                                            foreach (var cc in attr111)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Follow_Up_Amendments += $"{key}\t - \t{cc.Count()}" + " | "; p12 += cc.Count();
                                            }
                                        }
                                        foundSummary1.Add(model);
                                        model = new CriticalIncidentSummary();
                                    }
                                    #endregion

                                    #region 2nd Location:
                                    if (ll2 != null)
                                    {
                                        model.LocationName = locList.Find(i => i == "Astoria Retirement Residence\r\n" + " - " + cnt2);
                                        var attr11 = ll2.GroupBy(i => i.MOHLTC_Follow_Up);
                                        if (attr11 != null)
                                        {
                                            foreach (var cc in attr11)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                                if (key == "NULL") continue;
                                                model.MOHLTC_Follow_Up += $"{key}\t - \t{cc.Count()}" + " | "; p1 += cc.Count();
                                            }
                                        }

                                        var attr10 = ll2.GroupBy(i => i.CIS_Initiated);
                                        if (attr10 != null)
                                        {
                                            foreach (var cc in attr10)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.CIS_Initiated += $"{key}\t - \t{cc.Count()}" + " | "; p2 += cc.Count();
                                            }
                                        }

                                        var attr7 = ll2.GroupBy(i => i.MOH_Notified);
                                        if (attr7 != null)
                                        {
                                            foreach (var cc in attr7)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.MOH_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p3 += cc.Count();
                                            }
                                        }

                                        var attr2 = ll2.GroupBy(i => i.POAS_Notified);
                                        if (attr2 != null)
                                        {
                                            foreach (var d in attr2)
                                            {
                                                string key = d.Key == null ? "NULL" : d.Key;
                                                if (key == "NULL") continue;
                                                model.POAS_Notified += $"{key}\t - \t{d.Count()}" + " | "; p4 += d.Count();
                                            }
                                        }

                                        var attr4 = ll2.GroupBy(i => i.Police_Notified);
                                        if (attr4 != null)
                                        {
                                            foreach (var cc in attr4)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Police_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p5 += cc.Count();
                                            }
                                        }


                                        var attr3 = ll2.GroupBy(i => i.Quality_Improvement_Actions);
                                        if (attr3 != null)
                                        {
                                            foreach (var cc in attr3)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Quality_Improvement_Actions += $"{key}\t - \t{cc.Count()}" + " | "; p6 += cc.Count();
                                            }
                                        }

                                        var attr8 = ll2.GroupBy(i => i.Risk_Locked);
                                        if (attr8 != null)
                                        {
                                            foreach (var cc in attr8)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Risk_Locked += $"{key}\t - \t{cc.Count()}" + " | "; p7 += cc.Count();
                                            }
                                        }

                                        var attr5 = ll2.GroupBy(i => i.Brief_Description);
                                        if (attr5 != null)
                                        {
                                            foreach (var cc in attr5)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Brief_Description += $"{key}\t - \t{cc.Count()}" + " | "; p8 += cc.Count();
                                            }
                                        }

                                        var attr1 = ll2.GroupBy(i => i.Care_Plan_Updated);
                                        if (attr1 != null)
                                        {
                                            foreach (var e in attr1)
                                            {
                                                string key = e.Key == null ? "NULL" : e.Key;
                                                if (key == "NULL") continue;
                                                model.Care_Plan_Updated += $"{key}\t - \t{e.Count()}" + " | "; p9 += e.Count();
                                            }
                                        }

                                        var attr13 = ll2.GroupBy(i => i.CI_Form_Number);
                                        if (attr13 != null)
                                        {
                                            int count = 0;
                                            foreach (var cc in attr13)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                count += cc.Count();
                                            }
                                            model.CI_Form_Number = $"All\t - \t{count}"; p10 += count;
                                        }

                                        var attr00 = ll2.GroupBy(i => i.File_Complete);
                                        if (attr00 != null)
                                        {
                                            foreach (var cc in attr00)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.File_Complete += $"{key}\t - \t{cc.Count()}" + " | "; p11 += cc.Count();
                                            }
                                        }

                                        var attr111 = ll2.GroupBy(i => i.Follow_Up_Amendments);
                                        if (attr111 != null)
                                        {
                                            foreach (var cc in attr111)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Follow_Up_Amendments += $"{key}\t - \t{cc.Count()}" + " | "; p12 += cc.Count();
                                            }
                                        }
                                        foundSummary1.Add(model);
                                        model = new CriticalIncidentSummary();
                                    }
                                    #endregion

                                    #region 3rd Location:
                                    if (ll3 != null)
                                    {
                                        model.LocationName = locList.Find(i => i == "Barnswallow Place Care Community\r\n" + " - " + cnt3);
                                        var attr11 = ll3.GroupBy(i => i.MOHLTC_Follow_Up);
                                        if (attr11 != null)
                                        {
                                            foreach (var cc in attr11)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                                if (key == "NULL") continue;
                                                model.MOHLTC_Follow_Up += $"{key}\t - \t{cc.Count()}" + " | "; p1 += cc.Count();
                                            }
                                        }

                                        var attr10 = ll3.GroupBy(i => i.CIS_Initiated);
                                        if (attr10 != null)
                                        {
                                            foreach (var cc in attr10)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.CIS_Initiated += $"{key}\t - \t{cc.Count()}" + " | "; p2 += cc.Count();
                                            }
                                        }

                                        var attr7 = ll3.GroupBy(i => i.MOH_Notified);
                                        if (attr7 != null)
                                        {
                                            foreach (var cc in attr7)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.MOH_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p3 += cc.Count();
                                            }
                                        }

                                        var attr2 = ll3.GroupBy(i => i.POAS_Notified);
                                        if (attr2 != null)
                                        {
                                            foreach (var d in attr2)
                                            {
                                                string key = d.Key == null ? "NULL" : d.Key;
                                                if (key == "NULL") continue;
                                                model.POAS_Notified += $"{key}\t - \t{d.Count()}" + " | "; p4 += d.Count();
                                            }
                                        }

                                        var attr4 = ll3.GroupBy(i => i.Police_Notified);
                                        if (attr4 != null)
                                        {
                                            foreach (var cc in attr4)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Police_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p5 += cc.Count();
                                            }
                                        }


                                        var attr3 = ll3.GroupBy(i => i.Quality_Improvement_Actions);
                                        if (attr3 != null)
                                        {
                                            foreach (var cc in attr3)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Quality_Improvement_Actions += $"{key}\t - \t{cc.Count()}" + " | "; p6 += cc.Count();
                                            }
                                        }

                                        var attr8 = ll3.GroupBy(i => i.Risk_Locked);
                                        if (attr8 != null)
                                        {
                                            foreach (var cc in attr8)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Risk_Locked += $"{key}\t - \t{cc.Count()}" + " | "; p7 += cc.Count();
                                            }
                                        }

                                        var attr5 = ll3.GroupBy(i => i.Brief_Description);
                                        if (attr5 != null)
                                        {
                                            foreach (var cc in attr5)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Brief_Description += $"{key}\t - \t{cc.Count()}" + " | "; p8 += cc.Count();
                                            }
                                        }

                                        var attr1 = ll3.GroupBy(i => i.Care_Plan_Updated);
                                        if (attr1 != null)
                                        {
                                            foreach (var e in attr1)
                                            {
                                                string key = e.Key == null ? "NULL" : e.Key;
                                                if (key == "NULL") continue;
                                                model.Care_Plan_Updated += $"{key}\t - \t{e.Count()}" + " | "; p9 += e.Count();
                                            }
                                        }

                                        var attr13 = ll3.GroupBy(i => i.CI_Form_Number);
                                        if (attr13 != null)
                                        {
                                            int count = 0;
                                            foreach (var cc in attr13)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                count += cc.Count();
                                            }
                                            model.CI_Form_Number = $"All\t - \t{count}"; p10 += count;
                                        }

                                        var attr00 = ll3.GroupBy(i => i.File_Complete);
                                        if (attr00 != null)
                                        {
                                            foreach (var cc in attr00)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.File_Complete += $"{key}\t - \t{cc.Count()}" + " | "; p11 += cc.Count();
                                            }
                                        }

                                        var attr111 = ll3.GroupBy(i => i.Follow_Up_Amendments);
                                        if (attr111 != null)
                                        {
                                            foreach (var cc in attr111)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Follow_Up_Amendments += $"{key}\t - \t{cc.Count()}" + " | "; p12 += cc.Count();
                                            }
                                        }
                                        foundSummary1.Add(model);
                                        model = new CriticalIncidentSummary();
                                    }
                                    #endregion

                                    #region 4rd Location:
                                    if (ll4 != null)
                                    {
                                        model.LocationName = locList.Find(i => i == "Bearbrook Retirement Residence\r\n" + " - " + cnt4);
                                        var attr11 = ll4.GroupBy(i => i.MOHLTC_Follow_Up);
                                        if (attr11 != null)
                                        {
                                            foreach (var cc in attr11)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                                if (key == "NULL") continue;
                                                model.MOHLTC_Follow_Up += $"{key}\t - \t{cc.Count()}" + " | "; p1 += cc.Count();
                                            }
                                        }

                                        var attr10 = ll4.GroupBy(i => i.CIS_Initiated);
                                        if (attr10 != null)
                                        {
                                            foreach (var cc in attr10)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.CIS_Initiated += $"{key}\t - \t{cc.Count()}" + " | "; p2 += cc.Count();
                                            }
                                        }

                                        var attr7 = ll4.GroupBy(i => i.MOH_Notified);
                                        if (attr7 != null)
                                        {
                                            foreach (var cc in attr7)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.MOH_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p3 += cc.Count();
                                            }
                                        }

                                        var attr2 = ll4.GroupBy(i => i.POAS_Notified);
                                        if (attr2 != null)
                                        {
                                            foreach (var d in attr2)
                                            {
                                                string key = d.Key == null ? "NULL" : d.Key;
                                                if (key == "NULL") continue;
                                                model.POAS_Notified += $"{key}\t - \t{d.Count()}" + " | "; p4 += d.Count();
                                            }
                                        }

                                        var attr4 = ll4.GroupBy(i => i.Police_Notified);
                                        if (attr4 != null)
                                        {
                                            foreach (var cc in attr4)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Police_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p5 += cc.Count();
                                            }
                                        }

                                        var attr3 = ll4.GroupBy(i => i.Quality_Improvement_Actions);
                                        if (attr3 != null)
                                        {
                                            foreach (var cc in attr3)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Quality_Improvement_Actions += $"{key}\t - \t{cc.Count()}" + " | "; p6 += cc.Count();
                                            }
                                        }

                                        var attr8 = ll4.GroupBy(i => i.Risk_Locked);
                                        if (attr8 != null)
                                        {
                                            foreach (var cc in attr8)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Risk_Locked += $"{key}\t - \t{cc.Count()}" + " | "; p7 += cc.Count();
                                            }
                                        }

                                        var attr5 = ll4.GroupBy(i => i.Brief_Description);
                                        if (attr5 != null)
                                        {
                                            foreach (var cc in attr5)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Brief_Description += $"{key}\t - \t{cc.Count()}" + " | "; p8 += cc.Count();
                                            }
                                        }

                                        var attr1 = ll4.GroupBy(i => i.Care_Plan_Updated);
                                        if (attr1 != null)
                                        {
                                            foreach (var e in attr1)
                                            {
                                                string key = e.Key == null ? "NULL" : e.Key;
                                                if (key == "NULL") continue;
                                                model.Care_Plan_Updated += $"{key}\t - \t{e.Count()}" + " | "; p9 += e.Count();
                                            }
                                        }

                                        var attr13 = ll4.GroupBy(i => i.CI_Form_Number);
                                        if (attr13 != null)
                                        {
                                            int count = 0;
                                            foreach (var cc in attr13)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                count += cc.Count();
                                            }
                                            model.CI_Form_Number = $"All\t - \t{count}"; p10 += count;
                                        }

                                        var attr00 = ll4.GroupBy(i => i.File_Complete);
                                        if (attr00 != null)
                                        {
                                            foreach (var cc in attr00)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.File_Complete += $"{key}\t - \t{cc.Count()}" + " | "; p11 += cc.Count();
                                            }
                                        }

                                        var attr111 = ll4.GroupBy(i => i.Follow_Up_Amendments);
                                        if (attr111 != null)
                                        {
                                            foreach (var cc in attr111)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Follow_Up_Amendments += $"{key}\t - \t{cc.Count()}" + " | "; p12 += cc.Count();
                                            }
                                        }
                                        foundSummary1.Add(model);
                                        model = new CriticalIncidentSummary();
                                    }
                                    #endregion

                                    #region 5th Location:
                                    if (ll5 != null)
                                    {
                                        model.LocationName = locList.Find(i => i == "Bloomington Cove Care Community\r\n" + " - " + cnt5);
                                        var attr11 = ll5.GroupBy(i => i.MOHLTC_Follow_Up);
                                        if (attr11 != null)
                                        {
                                            foreach (var cc in attr11)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                                if (key == "NULL") continue;
                                                model.MOHLTC_Follow_Up += $"{key}\t - \t{cc.Count()}" + " | "; p1 += cc.Count();
                                            }
                                        }

                                        var attr10 = ll5.GroupBy(i => i.CIS_Initiated);
                                        if (attr10 != null)
                                        {
                                            foreach (var cc in attr10)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.CIS_Initiated += $"{key}\t - \t{cc.Count()}" + " | "; p2 += cc.Count();
                                            }
                                        }

                                        var attr7 = ll5.GroupBy(i => i.MOH_Notified);
                                        if (attr7 != null)
                                        {
                                            foreach (var cc in attr7)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.MOH_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p3 += cc.Count();
                                            }
                                        }

                                        var attr2 = ll5.GroupBy(i => i.POAS_Notified);
                                        if (attr2 != null)
                                        {
                                            foreach (var d in attr2)
                                            {
                                                string key = d.Key == null ? "NULL" : d.Key;
                                                if (key == "NULL") continue;
                                                model.POAS_Notified += $"{key}\t - \t{d.Count()}" + " | "; p4 += d.Count();
                                            }
                                        }

                                        var attr4 = ll5.GroupBy(i => i.Police_Notified);
                                        if (attr4 != null)
                                        {
                                            foreach (var cc in attr4)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Police_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p5 += cc.Count();
                                            }
                                        }


                                        var attr3 = ll5.GroupBy(i => i.Quality_Improvement_Actions);
                                        if (attr3 != null)
                                        {
                                            foreach (var cc in attr3)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Quality_Improvement_Actions += $"{key}\t - \t{cc.Count()}" + " | "; p6 += cc.Count();
                                            }
                                        }

                                        var attr8 = ll5.GroupBy(i => i.Risk_Locked);
                                        if (attr8 != null)
                                        {
                                            foreach (var cc in attr8)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Risk_Locked += $"{key}\t - \t{cc.Count()}" + " | "; p7 += cc.Count();
                                            }
                                        }

                                        var attr5 = ll5.GroupBy(i => i.Brief_Description);
                                        if (attr5 != null)
                                        {
                                            foreach (var cc in attr5)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Brief_Description += $"{key}\t - \t{cc.Count()}" + " | "; p8 += cc.Count();
                                            }
                                        }

                                        var attr1 = ll5.GroupBy(i => i.Care_Plan_Updated);
                                        if (attr1 != null)
                                        {
                                            foreach (var e in attr1)
                                            {
                                                string key = e.Key == null ? "NULL" : e.Key;
                                                if (key == "NULL") continue;
                                                model.Care_Plan_Updated += $"{key}\t - \t{e.Count()}" + " | "; p9 += e.Count();
                                            }
                                        }

                                        var attr13 = ll5.GroupBy(i => i.CI_Form_Number);
                                        if (attr13 != null)
                                        {
                                            int count = 0;
                                            foreach (var cc in attr13)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                count += cc.Count();
                                            }
                                            model.CI_Form_Number = $"All\t - \t{count}"; p10 += count;
                                        }

                                        var attr00 = ll5.GroupBy(i => i.File_Complete);
                                        if (attr00 != null)
                                        {
                                            foreach (var cc in attr00)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.File_Complete += $"{key}\t - \t{cc.Count()}" + " | "; p11 += cc.Count();
                                            }
                                        }

                                        var attr111 = ll5.GroupBy(i => i.Follow_Up_Amendments);
                                        if (attr111 != null)
                                        {
                                            foreach (var cc in attr111)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Follow_Up_Amendments += $"{key}\t - \t{cc.Count()}" + " | "; p12 += cc.Count();
                                            }
                                        }
                                        foundSummary1.Add(model);
                                        model = new CriticalIncidentSummary();
                                    }
                                    #endregion

                                    #region 6th Location:
                                    if (ll6 != null)
                                    {
                                        model.LocationName = locList.Find(i => i == "Bradford Valley Care Community\r\n" + " - " + cnt6);
                                        var attr11 = ll6.GroupBy(i => i.MOHLTC_Follow_Up);
                                        if (attr11 != null)
                                        {
                                            foreach (var cc in attr11)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                                if (key == "NULL") continue;
                                                model.MOHLTC_Follow_Up += $"{key}\t - \t{cc.Count()}" + " | "; p1 += cc.Count();
                                            }
                                        }

                                        var attr10 = ll6.GroupBy(i => i.CIS_Initiated);
                                        if (attr10 != null)
                                        {
                                            foreach (var cc in attr10)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.CIS_Initiated += $"{key}\t - \t{cc.Count()}" + " | "; p2 += cc.Count();
                                            }
                                        }

                                        var attr7 = ll6.GroupBy(i => i.MOH_Notified);
                                        if (attr7 != null)
                                        {
                                            foreach (var cc in attr7)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.MOH_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p3 += cc.Count();
                                            }
                                        }

                                        var attr2 = ll6.GroupBy(i => i.POAS_Notified);
                                        if (attr2 != null)
                                        {
                                            foreach (var d in attr2)
                                            {
                                                string key = d.Key == null ? "NULL" : d.Key;
                                                if (key == "NULL") continue;
                                                model.POAS_Notified += $"{key}\t - \t{d.Count()}" + " | "; p4 += d.Count();
                                            }
                                        }

                                        var attr4 = ll6.GroupBy(i => i.Police_Notified);
                                        if (attr4 != null)
                                        {
                                            foreach (var cc in attr4)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Police_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p5 += cc.Count();
                                            }
                                        }


                                        var attr3 = ll6.GroupBy(i => i.Quality_Improvement_Actions);
                                        if (attr3 != null)
                                        {
                                            foreach (var cc in attr3)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Quality_Improvement_Actions += $"{key}\t - \t{cc.Count()}" + " | "; p6 += cc.Count();
                                            }
                                        }

                                        var attr8 = ll6.GroupBy(i => i.Risk_Locked);
                                        if (attr8 != null)
                                        {
                                            foreach (var cc in attr8)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Risk_Locked += $"{key}\t - \t{cc.Count()}" + " | "; p7 += cc.Count();
                                            }
                                        }

                                        var attr5 = ll6.GroupBy(i => i.Brief_Description);
                                        if (attr5 != null)
                                        {
                                            foreach (var cc in attr5)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Brief_Description += $"{key}\t - \t{cc.Count()}" + " | "; p8 += cc.Count();
                                            }
                                        }

                                        var attr1 = ll6.GroupBy(i => i.Care_Plan_Updated);
                                        if (attr1 != null)
                                        {
                                            foreach (var e in attr1)
                                            {
                                                string key = e.Key == null ? "NULL" : e.Key;
                                                if (key == "NULL") continue;
                                                model.Care_Plan_Updated += $"{key}\t - \t{e.Count()}" + " | "; p9 += e.Count();
                                            }
                                        }

                                        var attr13 = ll6.GroupBy(i => i.CI_Form_Number);
                                        if (attr13 != null)
                                        {
                                            int count = 0;
                                            foreach (var cc in attr13)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                count += cc.Count();
                                            }
                                            model.CI_Form_Number = $"All\t - \t{count}"; p10 += count;
                                        }

                                        var attr00 = ll6.GroupBy(i => i.File_Complete);
                                        if (attr00 != null)
                                        {
                                            foreach (var cc in attr00)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.File_Complete += $"{key}\t - \t{cc.Count()}" + " | "; p11 += cc.Count();
                                            }
                                        }

                                        var attr111 = ll6.GroupBy(i => i.Follow_Up_Amendments);
                                        if (attr111 != null)
                                        {
                                            foreach (var cc in attr111)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Follow_Up_Amendments += $"{key}\t - \t{cc.Count()}" + " | "; p12 += cc.Count();
                                            }
                                        }
                                        foundSummary1.Add(model);
                                        model = new CriticalIncidentSummary();
                                    }
                                    #endregion

                                    #region 7th Location:
                                    if (ll7 != null)
                                    {
                                        model.LocationName = locList.Find(i => i == "Brookside Lodge\r\n" + " - " + cnt7);
                                        var attr11 = ll7.GroupBy(i => i.MOHLTC_Follow_Up);
                                        if (attr11 != null)
                                        {
                                            foreach (var cc in attr11)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                                if (key == "NULL") continue;
                                                model.MOHLTC_Follow_Up += $"{key}\t - \t{cc.Count()}" + " | "; p1 += cc.Count();
                                            }
                                        }

                                        var attr10 = ll7.GroupBy(i => i.CIS_Initiated);
                                        if (attr10 != null)
                                        {
                                            foreach (var cc in attr10)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.CIS_Initiated += $"{key}\t - \t{cc.Count()}" + " | "; p2 += cc.Count();
                                            }
                                        }

                                        var attr7 = ll7.GroupBy(i => i.MOH_Notified);
                                        if (attr7 != null)
                                        {
                                            foreach (var cc in attr7)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.MOH_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p3 += cc.Count();
                                            }
                                        }

                                        var attr2 = ll7.GroupBy(i => i.POAS_Notified);
                                        if (attr2 != null)
                                        {
                                            foreach (var d in attr2)
                                            {
                                                string key = d.Key == null ? "NULL" : d.Key;
                                                if (key == "NULL") continue;
                                                model.POAS_Notified += $"{key}\t - \t{d.Count()}" + " | "; p4 += d.Count();
                                            }
                                        }

                                        var attr4 = ll7.GroupBy(i => i.Police_Notified);
                                        if (attr4 != null)
                                        {
                                            foreach (var cc in attr4)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Police_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p5 += cc.Count();
                                            }
                                        }


                                        var attr3 = ll7.GroupBy(i => i.Quality_Improvement_Actions);
                                        if (attr3 != null)
                                        {
                                            foreach (var cc in attr3)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Quality_Improvement_Actions += $"{key}\t - \t{cc.Count()}" + " | "; p6 += cc.Count();
                                            }
                                        }

                                        var attr8 = ll7.GroupBy(i => i.Risk_Locked);
                                        if (attr8 != null)
                                        {
                                            foreach (var cc in attr8)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Risk_Locked += $"{key}\t - \t{cc.Count()}" + " | "; p7 += cc.Count();
                                            }
                                        }

                                        var attr5 = ll7.GroupBy(i => i.Brief_Description);
                                        if (attr5 != null)
                                        {
                                            foreach (var cc in attr5)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Brief_Description += $"{key}\t - \t{cc.Count()}" + " | "; p8 += cc.Count();
                                            }
                                        }

                                        var attr1 = ll7.GroupBy(i => i.Care_Plan_Updated);
                                        if (attr1 != null)
                                        {
                                            foreach (var e in attr1)
                                            {
                                                string key = e.Key == null ? "NULL" : e.Key;
                                                if (key == "NULL") continue;
                                                model.Care_Plan_Updated += $"{key}\t - \t{e.Count()}" + " | "; p9 += e.Count();
                                            }
                                        }

                                        var attr13 = ll7.GroupBy(i => i.CI_Form_Number);
                                        if (attr13 != null)
                                        {
                                            int count = 0;
                                            foreach (var cc in attr13)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                count += cc.Count();
                                            }
                                            model.CI_Form_Number = $"All\t - \t{count}"; p10 += count;
                                        }

                                        var attr00 = ll7.GroupBy(i => i.File_Complete);
                                        if (attr00 != null)
                                        {
                                            foreach (var cc in attr00)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.File_Complete += $"{key}\t - \t{cc.Count()}" + " | "; p11 += cc.Count();
                                            }
                                        }

                                        var attr111 = ll7.GroupBy(i => i.Follow_Up_Amendments);
                                        if (attr111 != null)
                                        {
                                            foreach (var cc in attr111)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Follow_Up_Amendments += $"{key}\t - \t{cc.Count()}" + " | "; p12 += cc.Count();
                                            }
                                        }
                                        foundSummary1.Add(model);
                                        model = new CriticalIncidentSummary();
                                    }
                                    #endregion

                                    #region 8th Location:
                                    if (ll8 != null)
                                    {
                                        int c = 0;
                                        model.LocationName = locList.Find(i => i == "Woodbridge Vista" + " - " + cnt8);
                                        var attr11 = ll8.GroupBy(i => i.MOHLTC_Follow_Up);
                                        if (attr11 != null)
                                        {

                                            foreach (var cc in attr11)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                                if (key == "NULL") continue;
                                                if (c > 1)
                                                {
                                                    model.MOHLTC_Follow_Up += $"{key}\t - \t{cc.Count()}" + " | "; p1 += cc.Count();
                                                }
                                                else { model.MOHLTC_Follow_Up += $"{key}\t - \t{cc.Count()}"; p1 += cc.Count(); }
                                                c++;
                                            }
                                        }
                                        c = 0;
                                        var attr10 = ll8.GroupBy(i => i.CIS_Initiated);
                                        if (attr10 != null)
                                        {
                                            foreach (var cc in attr10)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                if (c > 1)
                                                {
                                                    model.CIS_Initiated += $"{key}\t - \t{cc.Count()}" + " | "; p2 += cc.Count();
                                                }
                                                else { model.CIS_Initiated += $"{key}\t - \t{cc.Count()}"; p2 += cc.Count(); }
                                                c++;
                                            }
                                        }
                                        c = 0;
                                        var attr7 = ll8.GroupBy(i => i.MOH_Notified);
                                        if (attr7 != null)
                                        {
                                            foreach (var cc in attr7)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                if (c > 1)
                                                {
                                                    model.MOH_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p3 += cc.Count();
                                                }
                                                else { model.MOH_Notified += $"{key}\t - \t{cc.Count()}"; p3 += cc.Count(); }
                                                c++;
                                            }
                                        }
                                        c = 0;
                                        var attr2 = ll8.GroupBy(i => i.POAS_Notified);
                                        if (attr2 != null)
                                        {
                                            foreach (var d in attr2)
                                            {
                                                string key = d.Key == null ? "NULL" : d.Key;
                                                if (key == "NULL") continue;
                                                if (c > 1)
                                                {
                                                    model.POAS_Notified += $"{key}\t - \t{d.Count()}" + " | ";
                                                    p4 += d.Count();
                                                }
                                                else { model.POAS_Notified += $"{key}\t - \t{d.Count()}"; p4 += d.Count(); }
                                                c++;
                                            }
                                        }
                                        c = 0;
                                        var attr4 = ll8.GroupBy(i => i.Police_Notified);
                                        if (attr4 != null)
                                        {
                                            foreach (var cc in attr4)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                if (c > 1)
                                                {
                                                    model.Police_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p5 += cc.Count();
                                                }
                                                else { model.Police_Notified += $"{key}\t - \t{cc.Count()}"; p5 += cc.Count(); }
                                                c++;
                                            }
                                        }
                                        c = 0;
                                        var attr3 = ll8.GroupBy(i => i.Quality_Improvement_Actions);
                                        if (attr3 != null)
                                        {
                                            foreach (var cc in attr3)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                if (c > 1)
                                                {
                                                    model.Quality_Improvement_Actions += $"{key}\t - \t{cc.Count()}" + " | ";
                                                    p6 += cc.Count();
                                                }
                                                else { model.Quality_Improvement_Actions += $"{key}\t - \t{cc.Count()}"; p6 += cc.Count(); }
                                                c++;
                                            }
                                        }
                                        c = 0;
                                        var attr8 = ll8.GroupBy(i => i.Risk_Locked);
                                        if (attr8 != null)
                                        {
                                            foreach (var cc in attr8)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                if (c > 1)
                                                {
                                                    model.Risk_Locked += $"{key}\t - \t{cc.Count()}" + " | "; p7 += cc.Count();
                                                }
                                                else { model.Risk_Locked += $"{key}\t - \t{cc.Count()}"; p7 += cc.Count(); }
                                                c++;
                                            }
                                        }
                                        c = 0;
                                        var attr5 = ll8.GroupBy(i => i.Brief_Description);
                                        if (attr5 != null)
                                        {
                                            foreach (var cc in attr5)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                if (c > 1)
                                                {
                                                    model.Brief_Description += $"{key}\t - \t{cc.Count()}" + " | "; p8 += cc.Count();
                                                }
                                                else { model.Brief_Description += $"{key}\t - \t{cc.Count()}"; p8 += cc.Count(); }
                                                c++;
                                            }
                                        }
                                        c = 0;
                                        var attr1 = ll8.GroupBy(i => i.Care_Plan_Updated);
                                        if (attr1 != null)
                                        {
                                            foreach (var e in attr1)
                                            {
                                                string key = e.Key == null ? "NULL" : e.Key;
                                                if (key == "NULL") continue;
                                                if (c > 1)
                                                {
                                                    model.Care_Plan_Updated += $"{key}\t - \t{e.Count()}" + " | "; p9 += e.Count();
                                                }
                                                else { model.Care_Plan_Updated += $"{key}\t - \t{e.Count()}"; p9 += e.Count(); }
                                                c++;
                                            }
                                        }
                                        c = 0;
                                        var attr13 = ll8.GroupBy(i => i.CI_Form_Number);
                                        if (attr13 != null)
                                        {
                                            int count = 0;
                                            foreach (var cc in attr13)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                count += cc.Count();
                                            }
                                            if (c > 1)
                                            {
                                                model.CI_Form_Number += $"{count}" + " | "; p10 += count;
                                            }
                                            else { model.CI_Form_Number += $"{count}"; p10 += count; }
                                            c++;
                                        }
                                        c = 0;
                                        var attr00 = ll8.GroupBy(i => i.File_Complete);
                                        if (attr00 != null)
                                        {
                                            foreach (var cc in attr00)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                if (c > 1)
                                                {
                                                    model.File_Complete += $"{key}\t - \t{cc.Count()}" + " | "; p11 += cc.Count();
                                                }
                                                else { model.File_Complete += $"{key}\t - \t{cc.Count()}"; p11 += cc.Count(); }
                                                c++;
                                            }
                                        }
                                        c = 0;
                                        var attr111 = ll8.GroupBy(i => i.Follow_Up_Amendments);
                                        if (attr111 != null)
                                        {
                                            foreach (var cc in attr111)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                if (c > 1)
                                                {
                                                    model.Follow_Up_Amendments += $"{key}\t - \t{cc.Count()}" + " | "; p12 += cc.Count();
                                                }
                                                else { model.Follow_Up_Amendments += $"{key}\t - \t{cc.Count()}"; p12 += cc.Count(); }
                                                c++;
                                            }
                                        }
                                        c = 0;
                                        foundSummary1.Add(model);
                                        model = new CriticalIncidentSummary();
                                    }
                                    #endregion

                                    #region 9th Location:
                                    if (ll9 != null)
                                    {
                                        model.LocationName = locList.Find(i => i == "Brookside Lodge\r\n" + " - " + cnt9);
                                        var attr11 = ll9.GroupBy(i => i.MOHLTC_Follow_Up);
                                        if (attr11 != null)
                                        {
                                            foreach (var cc in attr11)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                                if (key == "NULL") continue;
                                                model.MOHLTC_Follow_Up += $"{key}\t - \t{cc.Count()}" + " | "; p1 += cc.Count();
                                            }
                                        }

                                        var attr10 = ll9.GroupBy(i => i.CIS_Initiated);
                                        if (attr10 != null)
                                        {
                                            foreach (var cc in attr10)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.CIS_Initiated += $"{key}\t - \t{cc.Count()}" + " | "; p2 += cc.Count();
                                            }
                                        }

                                        var attr7 = ll9.GroupBy(i => i.MOH_Notified);
                                        if (attr7 != null)
                                        {
                                            foreach (var cc in attr7)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.MOH_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p3 += cc.Count();
                                            }
                                        }

                                        var attr2 = ll9.GroupBy(i => i.POAS_Notified);
                                        if (attr2 != null)
                                        {
                                            foreach (var d in attr2)
                                            {
                                                string key = d.Key == null ? "NULL" : d.Key;
                                                if (key == "NULL") continue;
                                                model.POAS_Notified += $"{key}\t - \t{d.Count()}" + " | "; p4 += d.Count();
                                            }
                                        }

                                        var attr4 = ll9.GroupBy(i => i.Police_Notified);
                                        if (attr4 != null)
                                        {
                                            foreach (var cc in attr4)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Police_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p5 += cc.Count();
                                            }
                                        }


                                        var attr3 = ll9.GroupBy(i => i.Quality_Improvement_Actions);
                                        if (attr3 != null)
                                        {
                                            foreach (var cc in attr3)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Quality_Improvement_Actions += $"{key}\t - \t{cc.Count()}" + " | "; p6 += cc.Count();
                                            }
                                        }

                                        var attr8 = ll9.GroupBy(i => i.Risk_Locked);
                                        if (attr8 != null)
                                        {
                                            foreach (var cc in attr8)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Risk_Locked += $"{key}\t - \t{cc.Count()}" + " | "; p7 += cc.Count();
                                            }
                                        }

                                        var attr5 = ll9.GroupBy(i => i.Brief_Description);
                                        if (attr5 != null)
                                        {
                                            foreach (var cc in attr5)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Brief_Description += $"{key}\t - \t{cc.Count()}" + " | "; p8 += cc.Count();
                                            }
                                        }

                                        var attr1 = ll9.GroupBy(i => i.Care_Plan_Updated);
                                        if (attr1 != null)
                                        {
                                            foreach (var e in attr1)
                                            {
                                                string key = e.Key == null ? "NULL" : e.Key;
                                                if (key == "NULL") continue;
                                                model.Care_Plan_Updated += $"{key}\t - \t{e.Count()}" + " | "; p9 += e.Count();
                                            }
                                        }

                                        var attr13 = ll9.GroupBy(i => i.CI_Form_Number);
                                        if (attr13 != null)
                                        {
                                            int count = 0;
                                            foreach (var cc in attr13)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                count += cc.Count();
                                            }
                                            model.CI_Form_Number = $"All\t - \t{count}"; p10 += count;
                                        }

                                        var attr00 = ll9.GroupBy(i => i.File_Complete);
                                        if (attr00 != null)
                                        {
                                            foreach (var cc in attr00)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.File_Complete += $"{key}\t - \t{cc.Count()}" + " | "; p11 += cc.Count();
                                            }
                                        }

                                        var attr111 = ll9.GroupBy(i => i.Follow_Up_Amendments);
                                        if (attr111 != null)
                                        {
                                            foreach (var cc in attr111)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Follow_Up_Amendments += $"{key}\t - \t{cc.Count()}" + " | "; p12 += cc.Count();
                                            }
                                        }
                                        foundSummary1.Add(model);
                                        model = new CriticalIncidentSummary();
                                    }
                                    #endregion

                                    #region 10th Location:
                                    if (ll10 != null)
                                    {
                                        model.LocationName = locList.Find(i => i == "Rideau\r\n" + " - " + cnt10);
                                        var attr11 = ll10.GroupBy(i => i.MOHLTC_Follow_Up);
                                        if (attr11 != null)
                                        {
                                            foreach (var cc in attr11)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                                if (key == "NULL") continue;
                                                model.MOHLTC_Follow_Up += $"{key}\t - \t{cc.Count()}" + " | "; p1 += cc.Count();
                                            }
                                        }

                                        var attr10 = ll10.GroupBy(i => i.CIS_Initiated);
                                        if (attr10 != null)
                                        {
                                            foreach (var cc in attr10)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.CIS_Initiated += $"{key}\t - \t{cc.Count()}" + " | "; p2 += cc.Count();
                                            }
                                        }

                                        var attr7 = ll10.GroupBy(i => i.MOH_Notified);
                                        if (attr7 != null)
                                        {
                                            foreach (var cc in attr7)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.MOH_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p3 += cc.Count();
                                            }
                                        }

                                        var attr2 = ll10.GroupBy(i => i.POAS_Notified);
                                        if (attr2 != null)
                                        {
                                            foreach (var d in attr2)
                                            {
                                                string key = d.Key == null ? "NULL" : d.Key;
                                                if (key == "NULL") continue;
                                                model.POAS_Notified += $"{key}\t - \t{d.Count()}" + " | "; p4 += d.Count();
                                            }
                                        }

                                        var attr4 = ll10.GroupBy(i => i.Police_Notified);
                                        if (attr4 != null)
                                        {
                                            foreach (var cc in attr4)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Police_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p5 += cc.Count();
                                            }
                                        }


                                        var attr3 = ll10.GroupBy(i => i.Quality_Improvement_Actions);
                                        if (attr3 != null)
                                        {
                                            foreach (var cc in attr3)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Quality_Improvement_Actions += $"{key}\t - \t{cc.Count()}" + " | "; p6 += cc.Count();
                                            }
                                        }

                                        var attr8 = ll10.GroupBy(i => i.Risk_Locked);
                                        if (attr8 != null)
                                        {
                                            foreach (var cc in attr8)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Risk_Locked += $"{key}\t - \t{cc.Count()}" + " | "; p7 += cc.Count();
                                            }
                                        }

                                        var attr5 = ll10.GroupBy(i => i.Brief_Description);
                                        if (attr5 != null)
                                        {
                                            foreach (var cc in attr5)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Brief_Description += $"{key}\t - \t{cc.Count()}" + " | "; p8 += cc.Count();
                                            }
                                        }

                                        var attr1 = ll10.GroupBy(i => i.Care_Plan_Updated);
                                        if (attr1 != null)
                                        {
                                            foreach (var e in attr1)
                                            {
                                                string key = e.Key == null ? "NULL" : e.Key;
                                                if (key == "NULL") continue;
                                                model.Care_Plan_Updated += $"{key}\t - \t{e.Count()}" + " | "; p9 += e.Count();
                                            }
                                        }

                                        var attr13 = ll10.GroupBy(i => i.CI_Form_Number);
                                        if (attr13 != null)
                                        {
                                            int count = 0;
                                            foreach (var cc in attr13)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                count += cc.Count();
                                            }
                                            model.CI_Form_Number = $"All\t - \t{count}"; p10 += count;
                                        }

                                        var attr00 = ll10.GroupBy(i => i.File_Complete);
                                        if (attr00 != null)
                                        {
                                            foreach (var cc in attr00)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.File_Complete += $"{key}\t - \t{cc.Count()}" + " | "; p11 += cc.Count();
                                            }
                                        }

                                        var attr111 = ll10.GroupBy(i => i.Follow_Up_Amendments);
                                        if (attr111 != null)
                                        {
                                            foreach (var cc in attr111)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Follow_Up_Amendments += $"{key}\t - \t{cc.Count()}" + " | "; p12 += cc.Count();
                                            }
                                        }
                                        foundSummary1.Add(model);
                                        model = new CriticalIncidentSummary();
                                    }
                                    #endregion

                                    #region 11th Location:
                                    if (ll11 != null)
                                    {
                                        model.LocationName = locList.Find(i => i == "Villa da Vinci\r\n" + " - " + cnt11);
                                        var attr11 = ll11.GroupBy(i => i.MOHLTC_Follow_Up);
                                        if (attr11 != null)
                                        {
                                            foreach (var cc in attr11)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                                if (key == "NULL") continue;
                                                model.MOHLTC_Follow_Up += $"{key}\t - \t{cc.Count()}" + " | "; p1 += cc.Count();
                                            }
                                        }

                                        var attr10 = ll11.GroupBy(i => i.CIS_Initiated);
                                        if (attr10 != null)
                                        {
                                            foreach (var cc in attr10)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.CIS_Initiated += $"{key}\t - \t{cc.Count()}" + " | "; p2 += cc.Count();
                                            }
                                        }

                                        var attr7 = ll11.GroupBy(i => i.MOH_Notified);
                                        if (attr7 != null)
                                        {
                                            foreach (var cc in attr7)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.MOH_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p3 += cc.Count();
                                            }
                                        }

                                        var attr2 = ll11.GroupBy(i => i.POAS_Notified);
                                        if (attr2 != null)
                                        {
                                            foreach (var d in attr2)
                                            {
                                                string key = d.Key == null ? "NULL" : d.Key;
                                                if (key == "NULL") continue;
                                                model.POAS_Notified += $"{key}\t - \t{d.Count()}" + " | "; p4 += d.Count();
                                            }
                                        }

                                        var attr4 = ll11.GroupBy(i => i.Police_Notified);
                                        if (attr4 != null)
                                        {
                                            foreach (var cc in attr4)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Police_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p5 += cc.Count();
                                            }
                                        }


                                        var attr3 = ll11.GroupBy(i => i.Quality_Improvement_Actions);
                                        if (attr3 != null)
                                        {
                                            foreach (var cc in attr3)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Quality_Improvement_Actions += $"{key}\t - \t{cc.Count()}" + " | "; p6 += cc.Count();
                                            }
                                        }

                                        var attr8 = ll11.GroupBy(i => i.Risk_Locked);
                                        if (attr8 != null)
                                        {
                                            foreach (var cc in attr8)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Risk_Locked += $"{key}\t - \t{cc.Count()}" + " | "; p7 += cc.Count();
                                            }
                                        }

                                        var attr5 = ll11.GroupBy(i => i.Brief_Description);
                                        if (attr5 != null)
                                        {
                                            foreach (var cc in attr5)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Brief_Description += $"{key}\t - \t{cc.Count()}" + " | "; p8 += cc.Count();
                                            }
                                        }

                                        var attr1 = ll11.GroupBy(i => i.Care_Plan_Updated);
                                        if (attr1 != null)
                                        {
                                            foreach (var e in attr1)
                                            {
                                                string key = e.Key == null ? "NULL" : e.Key;
                                                if (key == "NULL") continue;
                                                model.Care_Plan_Updated += $"{key}\t - \t{e.Count()}" + " | "; p9 += e.Count();
                                            }
                                        }

                                        var attr13 = ll11.GroupBy(i => i.CI_Form_Number);
                                        if (attr13 != null)
                                        {
                                            int count = 0;
                                            foreach (var cc in attr13)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                count += cc.Count();
                                            }
                                            model.CI_Form_Number = $"All\t - \t{count}"; p10 += count;
                                        }

                                        var attr00 = ll11.GroupBy(i => i.File_Complete);
                                        if (attr00 != null)
                                        {
                                            foreach (var cc in attr00)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.File_Complete += $"{key}\t - \t{cc.Count()}" + " | "; p11 += cc.Count();
                                            }
                                        }

                                        var attr111 = ll11.GroupBy(i => i.Follow_Up_Amendments);
                                        if (attr111 != null)
                                        {
                                            foreach (var cc in attr111)
                                            {
                                                string key = cc.Key == null ? "NULL" : cc.Key;
                                                if (key == "NULL") continue;
                                                model.Follow_Up_Amendments += $"{key}\t - \t{cc.Count()}" + " | "; p12 += cc.Count();
                                            }
                                        }
                                        foundSummary1.Add(model);
                                        model = new CriticalIncidentSummary();
                                    }
                                    #endregion

                                    #region Add All Summary quantity on List:
                                    allSummary1.Add(new IncidentSummaryAll
                                    {
                                        MOHLTC_Follow_Up = p1,
                                        CIS_Initiated = p2,
                                        MOH_Notified = p3,
                                        POAS_Notified = p4,
                                        Police_Notified = p5,
                                        Quality_Improvement_Actions = p6,
                                        Risk_Locked = p7,
                                        Brief_Description = p8,
                                        Care_Plan_Updated = p9,
                                        CI_Form_Number = p10,
                                        File_Complete = p11,
                                        Follow_Up_Amendments = p12
                                    });
                                #endregion

                                #region Create viewBags:
                                { ViewBag.TotalSummary = allSummary1; }

                                b = true;
                                if (foundSummary1.Count == 0) { b = false; ViewBag.EmptLocation = b; }

                                {
                                    ViewBag.Count = TablesContainer.COUNT;
                                }

                                {
                                    ViewBag.GN_Found = foundSummary1;
                                }

                                {
                                    ViewBag.Entity = "Critical_Incidents";
                                }

                                if (locList.Count != 0) isEmpty = true;

                                { ViewBag.Check1 = isEmpty; }

                                {
                                    ViewBag.Locations = locList;
                                }

                                {
                                    if (role == Role.Admin)
                                        ViewBag.LocInfo = db.Care_Communities.Find(1).Name;
                                    else ViewBag.LocInfo = db.Care_Communities.Find(Id_Location).Name;
                                }
                                #endregion
                                break;
                            #endregion

                            #region Complaints:
                            case "Complaint":
                                if (role == Role.Admin) TablesContainer.list2 = db.Complaints.ToList();
                                else TablesContainer.list2 = db.Complaints.Where(i => i.Location == Id_Location).ToList();
                                TablesContainer.list2 = (from ent in TablesContainer.list2 where ent.DateReceived >= start && ent.DateReceived <= end select ent).ToList();
                                if (TablesContainer.list2.Count() == 0)
                                {
                                    { ViewBag.ObjName = "Complaint"; }
                                    ViewBag.ErrorMsg = errorMsg = "Please select a date range.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                                TablesContainer.COUNT = TablesContainer.list2.Count;
                                strN = new List<string>();
                                var att1 = TablesContainer.list2.GroupBy(i => i.DateReceived);
                                if (att1 != null)
                                {
                                    
                                    foreach (var cc in att1)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att2 = TablesContainer.list2.GroupBy(i => i.WritenOrVerbal);
                                if (att2 != null)
                                {
                                    
                                    foreach (var cc in att2)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att3 = TablesContainer.list2.GroupBy(i => i.Receive_Directly);
                                if (att3 != null)
                                {
                                    
                                    foreach (var cc in att3)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att4 = TablesContainer.list2.GroupBy(i => i.FromResident);
                                if (att4 != null)
                                {
                                    
                                    foreach (var cc in att4)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att5 = TablesContainer.list2.GroupBy(i => i.ResidentName);
                                if (att5 != null)
                                {
                                    
                                    foreach (var cc in att5)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att6 = TablesContainer.list2.GroupBy(i => i.Department);
                                if (att6 != null)
                                {
                                    
                                    foreach (var cc in att6)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att7 = TablesContainer.list2.GroupBy(i => i.BriefDescription);
                                if (att7 != null)
                                {
                                    
                                    foreach (var cc in att7)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att8 = TablesContainer.list2.GroupBy(i => i.IsAdministration);
                                if (att8 != null)
                                {
                                    
                                    foreach (var cc in att8)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att9 = TablesContainer.list2.GroupBy(i => i.CareServices);
                                if (att9 != null)
                                {
                                    
                                    foreach (var cc in att9)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att10 = TablesContainer.list2.GroupBy(i => i.PalliativeCare);
                                if (att10 != null)
                                {
                                    
                                    foreach (var cc in att10)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att11 = TablesContainer.list2.GroupBy(i => i.Dietary);
                                if (att11 != null)
                                {
                                    
                                    foreach (var cc in att11)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att12 = TablesContainer.list2.GroupBy(i => i.Housekeeping);
                                if (att12 != null)
                                {
                                    
                                    foreach (var cc in att2)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att13 = TablesContainer.list2.GroupBy(i => i.Laundry);
                                if (att13 != null)
                                {
                                    
                                    foreach (var cc in att13)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att14 = TablesContainer.list2.GroupBy(i => i.Maintenance);
                                if (att14 != null)
                                {
                                    
                                    foreach (var cc in att14)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att15 = TablesContainer.list2.GroupBy(i => i.Programs);
                                if (att15 != null)
                                {
                                    strN.Add("Programs: ");
                                    foreach (var cc in att15)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att16 = TablesContainer.list2.GroupBy(i => i.Physician);
                                if (att16 != null)
                                {
                                    
                                    foreach (var cc in att16)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att17 = TablesContainer.list2.GroupBy(i => i.Beautician);
                                if (att17 != null)
                                {
                                    
                                    foreach (var cc in att17)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att18 = TablesContainer.list2.GroupBy(i => i.FootCare);
                                if (att18 != null)
                                {
                                    
                                    foreach (var cc in att18)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att19 = TablesContainer.list2.GroupBy(i => i.DentalCare);
                                if (att19 != null)
                                {
                                    
                                    foreach (var cc in att19)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att20 = TablesContainer.list2.GroupBy(i => i.Physio);
                                if (att20 != null)
                                {
                                    
                                    foreach (var cc in att20)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att21 = TablesContainer.list2.GroupBy(i => i.Other);
                                if (att21 != null)
                                {
                                    
                                    foreach (var cc in att21)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att22 = TablesContainer.list2.GroupBy(i => i.MOHLTCNotified);
                                if (att22 != null)
                                {
                                    
                                    foreach (var cc in att22)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att23 = TablesContainer.list2.GroupBy(i => i.CopyToVP);
                                if (att23 != null)
                                {
                                    
                                    foreach (var cc in att23)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att24 = TablesContainer.list2.GroupBy(i => i.ResponseSent);
                                if (att15 != null)
                                {
                                    
                                    foreach (var cc in att15)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att25 = TablesContainer.list2.GroupBy(i => i.ActionToken);
                                if (att25 != null)
                                {
                                    
                                    foreach (var cc in att25)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att26 = TablesContainer.list2.GroupBy(i => i.Resolved);
                                if (att26 != null)
                                {
                                    
                                    foreach (var cc in att26)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att27 = TablesContainer.list2.GroupBy(i => i.MinistryVisit);
                                if (att27 != null)
                                {
                                    
                                    foreach (var cc in att27)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                ///
                                b = true;

                                {
                                    ViewBag.Count = TablesContainer.COUNT;
                                }

                                {
                                    ViewBag.GN_Found = HomeController.strN;
                                }
                                ViewBag.Entity = "Complaints";
                                break;
                            #endregion

                            #region Good_News:
                            case "Good_News":
                                { ViewBag.ObjName = "Good_News"; }
                                var lst_locat3 = db.Good_News.Where(i => i.Location == Id_Location).ToList();
                                TablesContainer.list3 = (from ent in lst_locat3 where ent.DateNews >= start && ent.DateNews <= end select ent).ToList();
                                if (TablesContainer.list3.Count() == 0)
                                {
                                    ViewBag.ErrorMsg = errorMsg = "Please select a date range.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }

                                strN = new List<string>();
                                //var news = db.Good_News.ToList();
                                var a1 = TablesContainer.list3.GroupBy(i => i.DateNews);
                                if (a1 != null)
                                {
                                    strN.Add("Date News: ");
                                    foreach (var cc in a1)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var a2 = TablesContainer.list3.GroupBy(i => i.Department);
                                if (a2 != null)
                                {
                                    
                                    foreach (var cc in a2)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var a3 = TablesContainer.list3.GroupBy(i => i.SourceCompliment);
                                if (a3 != null)
                                {
                                    strN.Add("Source Compliment: ");
                                    foreach (var cc in a3)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var a4 = TablesContainer.list3.GroupBy(i => i.ReceivedFrom);
                                if (a4 != null)
                                {
                                    strN.Add("Received From: ");
                                    foreach (var cc in a4)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var a5 = TablesContainer.list3.GroupBy(i => i.Description_Complim);
                                if (a5 != null)
                                {
                                    strN.Add("Description Compliment: ");
                                    foreach (var cc in a5)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var a6 = TablesContainer.list3.GroupBy(i => i.Respect);
                                if (a6 != null)
                                {
                                    strN.Add("Respect: ");
                                    foreach (var cc in a6)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var a7 = TablesContainer.list3.GroupBy(i => i.Passion);
                                if (a7 != null)
                                {
                                    strN.Add("Passion: ");
                                    foreach (var cc in a7)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var a8 = TablesContainer.list3.GroupBy(i => i.Teamwork);
                                if (a8 != null)
                                {
                                    strN.Add("Teamwork: ");
                                    foreach (var cc in a8)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var a9 = TablesContainer.list3.GroupBy(i => i.Responsibility);
                                if (a9 != null)
                                {
                                    strN.Add("Responsibility: ");
                                    foreach (var cc in a9)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var a10 = TablesContainer.list3.GroupBy(i => i.Growth);
                                if (a10 != null)
                                {
                                    strN.Add("Growth: ");
                                    foreach (var cc in a10)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var a11 = TablesContainer.list3.GroupBy(i => i.Compliment);
                                if (a11 != null)
                                {
                                    strN.Add("Compliment: ");
                                    foreach (var cc in a11)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var a12 = TablesContainer.list3.GroupBy(i => i.Spot_Awards);
                                if (a12 != null)
                                {
                                    strN.Add("Spot Awards: ");
                                    foreach (var cc in a12)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var a13 = TablesContainer.list3.GroupBy(i => i.Awards_Details);
                                if (a13 != null)
                                {
                                    strN.Add("Awards Details: ");
                                    foreach (var cc in a13)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var a14 = TablesContainer.list3.GroupBy(i => i.NameAwards);
                                if (a14 != null)
                                {
                                    strN.Add("Name Awards: ");
                                    foreach (var cc in a14)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var a15 = TablesContainer.list3.GroupBy(i => i.Awards_Received);
                                if (a15 != null)
                                {
                                    strN.Add("Awards Received: ");
                                    foreach (var cc in a15)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var a16 = TablesContainer.list3.GroupBy(i => i.Community_Inititives);
                                if (a16 != null)
                                {
                                    strN.Add("Awards Received: ");
                                    foreach (var cc in a16)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                ///
                                b = true; TablesContainer.COUNT = TablesContainer.list3.Count;

                                {
                                    ViewBag.Count = TablesContainer.COUNT;
                                }

                                {
                                    ViewBag.GN_Found = HomeController.strN;
                                }

                                {
                                    ViewBag.Entity = "Good_News";
                                }
                                break;
                            #endregion

                            #region Emergency_Prep: 
                            case "Emergency_Prep":
                                { ViewBag.ObjName = "Emergency_Prep"; }
                                var lst_locat4 = db.Emergency_Prep.Where(i => i.Location == Id_Location).ToList();
                                TablesContainer.list4 = db.Emergency_Prep.ToList();
                                if (TablesContainer.list4.Count() == 0)
                                {
                                    ViewBag.ErrorMsg = errorMsg = "Please select a date range.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }

                                strN = new List<string>();
                                //var news = db.Good_News.ToList();
                                var e1 = TablesContainer.list4.GroupBy(i => i.Name);
                                if (e1 != null)
                                {
                                    strN.Add("Name: ");
                                    foreach (var cc in e1)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var e2 = TablesContainer.list4.GroupBy(i => i.Jan);
                                if (e2 != null)
                                {
                                    strN.Add("Jan: ");
                                    foreach (var cc in e2)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var e3 = TablesContainer.list4.GroupBy(i => i.Feb);
                                if (e3 != null)
                                {
                                    strN.Add("Feb: ");
                                    foreach (var cc in e3)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var e4 = TablesContainer.list4.GroupBy(i => i.Mar);
                                if (e4 != null)
                                {
                                    strN.Add("Mar: ");
                                    foreach (var cc in e4)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var e5 = TablesContainer.list4.GroupBy(i => i.Apr);
                                if (e5 != null)
                                {
                                    strN.Add("Apr: ");
                                    foreach (var cc in e5)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var e6 = TablesContainer.list4.GroupBy(i => i.May);
                                if (e6 != null)
                                {
                                    strN.Add("May: ");
                                    foreach (var cc in e6)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var e7 = TablesContainer.list4.GroupBy(i => i.Jun);
                                if (e7 != null)
                                {
                                    strN.Add("Jun: ");
                                    foreach (var cc in e7)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var e8 = TablesContainer.list4.GroupBy(i => i.Jul);
                                if (e8 != null)
                                {
                                    strN.Add("Jul: ");
                                    foreach (var cc in e8)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var e9 = TablesContainer.list4.GroupBy(i => i.Aug);
                                if (e9 != null)
                                {
                                    strN.Add("Aug: ");
                                    foreach (var cc in e9)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var e10 = TablesContainer.list4.GroupBy(i => i.Sep);
                                if (e10 != null)
                                {
                                    strN.Add("Sep: ");
                                    foreach (var cc in e10)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var e11 = TablesContainer.list4.GroupBy(i => i.Oct);
                                if (e11 != null)
                                {
                                    strN.Add("Oct: ");
                                    foreach (var cc in e11)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var e12 = TablesContainer.list4.GroupBy(i => i.Nov);
                                if (e12 != null)
                                {
                                    strN.Add("Nov: ");
                                    foreach (var cc in e12)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var e13 = TablesContainer.list4.GroupBy(i => i.Dec);
                                if (e13 != null)
                                {
                                    strN.Add("Dec: ");
                                    foreach (var cc in e13)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var e14 = TablesContainer.list4.GroupBy(i => i.Dec);
                                if (e13 != null)
                                {
                                    strN.Add("Dec: ");
                                    foreach (var cc in e13)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }
                                b = true; TablesContainer.COUNT = TablesContainer.list4.Count;

                                {
                                    ViewBag.Count = TablesContainer.COUNT;
                                }

                                {
                                    ViewBag.GN_Found = HomeController.strN;
                                }

                                {
                                    ViewBag.Entity = "Emergency_Prep";
                                }
                                break;
                            #endregion

                            #region Community_Risks: 
                            case "Community_Risks":
                                var lst_locat5 = db.Community_Risks.Where(i => i.Location == Id_Location).ToList();
                                TablesContainer.list5 = (from ent in lst_locat5 where ent.Date >= start && ent.Date <= end select ent).ToList();
                                { ViewBag.ObjName = "Community_Risks"; }
                                if (TablesContainer.list5.Count() == 0)
                                {
                                    ViewBag.ErrorMsg = errorMsg = "Please select a date range.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }

                                strN = new List<string>();
                                var c1 = TablesContainer.list5.GroupBy(i => i.Date);
                                if (c1 != null)
                                {
                                    strN.Add("Date: ");
                                    foreach (var cc in c1)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var c2 = TablesContainer.list5.GroupBy(i => i.Type_Of_Risk);
                                if (c2 != null)
                                {
                                    strN.Add("Type Of Risk: ");
                                    foreach (var cc in c2)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var c3 = TablesContainer.list5.GroupBy(i => i.Descriptions);
                                if (c3 != null)
                                {
                                    strN.Add("Descriptions: ");
                                    foreach (var cc in c3)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var c4 = TablesContainer.list5.GroupBy(i => i.Potential_Risk);
                                if (c4 != null)
                                {
                                    strN.Add("Potential Risk: ");
                                    foreach (var cc in c4)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var c5 = TablesContainer.list5.GroupBy(i => i.MOH_Visit);
                                if (c5 != null)
                                {
                                    strN.Add("MOH Visit: ");
                                    foreach (var cc in c5)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var c6 = TablesContainer.list5.GroupBy(i => i.Risk_Legal_Action);
                                if (c6 != null)
                                {
                                    strN.Add("Risk Legal Action: ");
                                    foreach (var cc in c6)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var c7 = TablesContainer.list5.GroupBy(i => i.Hot_Alert);
                                if (c7 != null)
                                {
                                    strN.Add("Hot Alert: ");
                                    foreach (var cc in c7)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var c8 = TablesContainer.list5.GroupBy(i => i.Status_Update);
                                if (c8 != null)
                                {
                                    strN.Add("Status Update: ");
                                    foreach (var cc in c8)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        else
                                        {
                                            strN.Add($"{key}\t - \t{cc.Count()}");
                                        }
                                    }
                                }

                                var c9 = TablesContainer.list5.GroupBy(i => i.Resolved);
                                if (c9 != null)
                                {
                                    
                                    foreach (var cc in c9)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        else
                                        {
                                            strN.Add($"{key}\t - \t{cc.Count()}");
                                        }
                                    }
                                }

                                b = true; TablesContainer.COUNT = TablesContainer.list5.Count;

                                {
                                    ViewBag.Count = TablesContainer.COUNT;
                                }

                                {
                                    ViewBag.GN_Found = HomeController.strN;
                                }

                                {
                                    ViewBag.Entity = "Community_Risks";
                                }
                                break;
                            #endregion

                            #region Visits_Others:
                            case "Visits_Others":
                                var lst_locat6 = db.Visits_Others.Where(i => i.Location == Id_Location).ToList();
                                TablesContainer.list6 = (from ent in lst_locat6 where ent.Date_of_Visit >= start && ent.Date_of_Visit <= end select ent).ToList();
                                { ViewBag.ObjName = "Visits_Others"; }
                                if (TablesContainer.list6.Count() == 0)
                                {
                                    ViewBag.ErrorMsg = errorMsg = "Please select a date range.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }

                                strN = new List<string>();
                                var v1 = TablesContainer.list6.GroupBy(i => i.Date_of_Visit);
                                if (v1 != null)
                                {
                                    strN.Add("Date of Visit: ");
                                    foreach (var cc in v1)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var v2 = TablesContainer.list6.GroupBy(i => i.Agency);
                                if (v2 != null)
                                {
                                    strN.Add("Agency: ");
                                    foreach (var cc in v2)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var v3 = TablesContainer.list6.GroupBy(i => i.Number_of_Findings);
                                if (v3 != null)
                                {
                                    strN.Add("Number of Findings: ");
                                    foreach (var cc in v3)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var v4 = TablesContainer.list6.GroupBy(i => i.Details_of_Findings);
                                if (v4 != null)
                                {
                                    strN.Add("Details of Findings: ");
                                    foreach (var cc in v4)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var v5 = TablesContainer.list6.GroupBy(i => i.Corrective_Actions);
                                if (v5 != null)
                                {
                                    strN.Add("Corrective Actions: ");
                                    foreach (var cc in v5)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var v6 = TablesContainer.list6.GroupBy(i => i.Report_Posted);
                                if (v6 != null)
                                {
                                    strN.Add("Report Posted: ");
                                    foreach (var cc in v6)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var v7 = TablesContainer.list6.GroupBy(i => i.LHIN_Letter_Received);
                                if (v7 != null)
                                {
                                    strN.Add("LHIN Letter Received: ");
                                    foreach (var cc in v7)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var v8 = TablesContainer.list6.GroupBy(i => i.PH_Letter_Received);
                                if (v8 != null)
                                {
                                    strN.Add("PH Letter Received: ");
                                    foreach (var cc in v8)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        else
                                        {
                                            strN.Add($"{key}\t - \t{cc.Count()}");
                                        }
                                    }
                                }

                                b = true; TablesContainer.COUNT = TablesContainer.list6.Count;

                                {
                                    ViewBag.Count = TablesContainer.COUNT;
                                }

                                {
                                    ViewBag.GN_Found = HomeController.strN;
                                }

                                {
                                    ViewBag.Entity = "Visits_Others";
                                }
                                break;
                            #endregion

                            #region Privacy Breaches:   
                            case "Privacy_Breaches":
                                TablesContainer.list7 = (from ent in db.Privacy_Breaches where ent.Date_Breach_Reported >= start && ent.Date_Breach_Reported <= end select ent).ToList();
                                { ViewBag.ObjName = "Visits_Others"; }
                                if (TablesContainer.list7.Count() == 0)
                                {
                                    ViewBag.ErrorMsg = errorMsg = "Please select a date range.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }

                                strN = new List<string>();
                                var pb1 = TablesContainer.list7.GroupBy(i => i.Date_Breach_Occurred);
                                if (pb1 != null)
                                {
                                    strN.Add("Date Breach Occured: ");
                                    foreach (var cc in pb1)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var pb2 = TablesContainer.list7.GroupBy(i => i.Date_Breach_Reported);
                                if (pb2 != null)
                                {
                                    strN.Add("Date Breach Reported: ");
                                    foreach (var cc in pb2)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var pb3 = TablesContainer.list7.GroupBy(i => i.Date_Breach_Reported_By);
                                if (pb3 != null)
                                {
                                    strN.Add("Date Breach Reported By: ");
                                    foreach (var cc in pb3)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var pb4 = TablesContainer.list7.GroupBy(i => i.Description_Outcome);
                                if (pb4 != null)
                                {
                                    strN.Add("Description Outcome: ");
                                    foreach (var cc in pb4)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var pb5 = TablesContainer.list7.GroupBy(i => i.Risk_Level);
                                if (pb5 != null)
                                {
                                    strN.Add("Risk Level: ");
                                    foreach (var cc in pb5)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var pb6 = TablesContainer.list7.GroupBy(i => i.Number_of_Individuals_Affected);
                                if (pb6 != null)
                                {
                                    strN.Add("Number of Individuals Affected: ");
                                    foreach (var cc in pb6)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var pb7 = TablesContainer.list7.GroupBy(i => i.Status);
                                if (pb7 != null)
                                {
                                    strN.Add("Status: ");
                                    foreach (var cc in pb7)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var pb8 = TablesContainer.list7.GroupBy(i => i.Type_of_Breach);
                                if (pb8 != null)
                                {
                                    strN.Add("Type of Breach: ");
                                    foreach (var cc in pb8)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        else
                                        {
                                            strN.Add($"{key}\t - \t{cc.Count()}");
                                        }
                                    }
                                }

                                var pb9 = TablesContainer.list7.GroupBy(i => i.Type_of_PHI_Involved);
                                if (pb9 != null)
                                {
                                    strN.Add("Type of PHI Involved: ");
                                    foreach (var cc in pb9)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        else
                                        {
                                            strN.Add($"{key}\t - \t{cc.Count()}");
                                        }
                                    }
                                }

                                b = true; TablesContainer.COUNT = TablesContainer.list7.Count;

                                {
                                    ViewBag.Count = TablesContainer.COUNT;
                                }

                                {
                                    ViewBag.GN_Found = HomeController.strN;
                                }

                                {
                                    ViewBag.Entity = "Privacy_Breaches";
                                }
                                break;
                            #endregion

                            #region Privacy Complaints  
                            case "Privacy_Complaints":
                                TablesContainer.list8 = (from ent in db.Privacy_Complaints where ent.Date_Complain_Received >= start && ent.Date_Complain_Received <= end select ent).ToList();
                                { ViewBag.ObjName = "Privacy_Complaints"; }
                                if (TablesContainer.list8.Count() == 0)
                                {
                                    ViewBag.ErrorMsg = errorMsg = "Please select a date range.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }

                                strN = new List<string>();
                                var pс1 = TablesContainer.list8.GroupBy(i => i.Is_Complaint_Resolved);
                                if (pс1 != null)
                                {
                                    strN.Add("Is Complaint Resolved: ");
                                    foreach (var cc in pс1)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var pс2 = TablesContainer.list8.GroupBy(i => i.Status);
                                if (pс2 != null)
                                {
                                    strN.Add("Status: ");
                                    foreach (var cc in pс2)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var pс3 = TablesContainer.list8.GroupBy(i => i.Type_of_Complaint);
                                if (pс3 != null)
                                {
                                    strN.Add("Type of Complaint: ");
                                    foreach (var cc in pс3)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var pс4 = TablesContainer.list8.GroupBy(i => i.Description_Outcome);
                                if (pс4 != null)
                                {
                                    strN.Add("Description Outcome: ");
                                    foreach (var cc in pс4)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var pс5 = TablesContainer.list8.GroupBy(i => i.Date_Complain_Received);
                                if (pс5 != null)
                                {
                                    strN.Add("Date Complain Received: ");
                                    foreach (var cc in pс5)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var pс6 = TablesContainer.list8.GroupBy(i => i.Complain_Filed_By);
                                if (pс6 != null)
                                {
                                    strN.Add("Complain Filed By: ");
                                    foreach (var cc in pс6)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                b = true; TablesContainer.COUNT = TablesContainer.list8.Count;

                                {
                                    ViewBag.Count = TablesContainer.COUNT;
                                }

                                {
                                    ViewBag.GN_Found = HomeController.strN;
                                }

                                {
                                    ViewBag.Entity = "Privacy_Complaints";
                                }
                                break;
                            #endregion

                            #region Education   
                            case "Education":
                                TablesContainer.list9 = (from ent in db.Educations where ent.DateStart >= start && ent.DateStart <= end select ent).ToList();
                                { ViewBag.ObjName = "Education"; }
                                if (TablesContainer.list9.Count() == 0)
                                {
                                    ViewBag.ErrorMsg = errorMsg = "Please select a date range.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }

                                strN = new List<string>();
                                var ed1 = TablesContainer.list9.GroupBy(i => i.Session_Name);
                                if (ed1 != null)
                                {
                                    strN.Add("Session Name: ");
                                    foreach (var cc in ed1)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var ed2 = TablesContainer.list9.GroupBy(i => i.Jan);
                                if (ed2 != null)
                                {
                                    strN.Add("Jan: ");
                                    foreach (var cc in ed2)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var ed3 = TablesContainer.list9.GroupBy(i => i.Feb);
                                if (ed3 != null)
                                {
                                    strN.Add("Feb: ");
                                    foreach (var cc in ed3)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var ed4 = TablesContainer.list9.GroupBy(i => i.Mar);
                                if (ed4 != null)
                                {
                                    strN.Add("Mar: ");
                                    foreach (var cc in ed4)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var ed5 = TablesContainer.list9.GroupBy(i => i.Apr);
                                if (ed5 != null)
                                {
                                    strN.Add("Apr: ");
                                    foreach (var cc in ed5)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var ed6 = TablesContainer.list9.GroupBy(i => i.May);
                                if (ed6 != null)
                                {
                                    strN.Add("May: ");
                                    foreach (var cc in ed6)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var ed7 = TablesContainer.list9.GroupBy(i => i.Jun);
                                if (ed7 != null)
                                {
                                    strN.Add("Jun: ");
                                    foreach (var cc in ed7)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var ed8 = TablesContainer.list9.GroupBy(i => i.Jul);
                                if (ed8 != null)
                                {
                                    strN.Add("Jul: ");
                                    foreach (var cc in ed8)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var ed9 = TablesContainer.list9.GroupBy(i => i.Aug);
                                if (ed9 != null)
                                {
                                    strN.Add("Aug: ");
                                    foreach (var cc in ed9)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var ed10 = TablesContainer.list9.GroupBy(i => i.Sep);
                                if (ed10 != null)
                                {
                                    strN.Add("Sep: ");
                                    foreach (var cc in ed10)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var ed11 = TablesContainer.list9.GroupBy(i => i.Oct);
                                if (ed11 != null)
                                {
                                    strN.Add("Oct: ");
                                    foreach (var cc in ed11)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var ed12 = TablesContainer.list9.GroupBy(i => i.Nov);
                                if (ed12 != null)
                                {
                                    strN.Add("Nov: ");
                                    foreach (var cc in ed12)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var ed13 = TablesContainer.list9.GroupBy(i => i.Oct);
                                if (ed13 != null)
                                {
                                    strN.Add("Oct: ");
                                    foreach (var cc in ed13)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var ed14 = TablesContainer.list9.GroupBy(i => i.Nov);
                                if (ed14 != null)
                                {
                                    strN.Add("Nov: ");
                                    foreach (var cc in ed14)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var ed16 = TablesContainer.list9.GroupBy(i => i.Total_Numb_Educ);
                                if (ed16 != null)
                                {
                                    strN.Add("Total Numb Educ: ");
                                    foreach (var cc in ed16)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var ed17 = TablesContainer.list9.GroupBy(i => i.Total_Numb_Eligible);
                                if (ed17 != null)
                                {
                                    strN.Add("Total Numb Eligible: ");
                                    foreach (var cc in ed17)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var ed18 = TablesContainer.list9.GroupBy(i => i.Approx_Per_Educated);
                                if (ed18 != null)
                                {
                                    strN.Add("Approx Per Educated: ");
                                    foreach (var cc in ed18)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                b = true; TablesContainer.COUNT = TablesContainer.list9.Count;

                                {
                                    ViewBag.Count = TablesContainer.COUNT;
                                }

                                {
                                    ViewBag.GN_Found = HomeController.strN;
                                }

                                {
                                    ViewBag.Entity = "Education";
                                }
                                break;
                            #endregion

                            #region Labour_Relations:
                            case "Labour_Relations":
                                var lst_locat10 = db.Relations.Where(i => i.Location == Id_Location);
                                TablesContainer.list10 = (from ent in lst_locat10 where ent.Date >= start && ent.Date <= end select ent).ToList();
                                { ViewBag.ObjName = "Community_Risks"; }
                                if (TablesContainer.list10.Count() == 0)
                                {
                                    ViewBag.ErrorMsg = errorMsg = "Please select a date range.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }

                                strN = new List<string>();
                                var l1 = TablesContainer.list10.GroupBy(i => i.Date);
                                if (l1 != null)
                                {
                                    strN.Add("Date: ");
                                    foreach (var cc in l1)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var l2 = TablesContainer.list10.GroupBy(i => i.Union);
                                if (l2 != null)
                                {
                                    strN.Add("Union: ");
                                    foreach (var cc in l2)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var l3 = TablesContainer.list10.GroupBy(i => i.Category);
                                if (l3 != null)
                                {
                                    strN.Add("Category: ");
                                    foreach (var cc in l3)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var l4 = TablesContainer.list10.GroupBy(i => i.Details);
                                if (l4 != null)
                                {
                                    strN.Add("Details: ");
                                    foreach (var cc in l4)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var l5 = TablesContainer.list10.GroupBy(i => i.Status);
                                if (l5 != null)
                                {
                                    strN.Add("Status: ");
                                    foreach (var cc in l5)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var l6 = TablesContainer.list10.GroupBy(i => i.Accruals);
                                if (l6 != null)
                                {
                                    strN.Add("Accruals: ");
                                    foreach (var cc in l6)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var l7 = TablesContainer.list10.GroupBy(i => i.Outcome);
                                if (l7 != null)
                                {
                                    strN.Add("Outcome: ");
                                    foreach (var cc in l7)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var l8 = TablesContainer.list10.GroupBy(i => i.Lessons_Learned);
                                if (l8 != null)
                                {
                                    strN.Add("Lessons Learned: ");
                                    foreach (var cc in l8)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        else
                                        {
                                            strN.Add($"{key}\t - \t{cc.Count()}");
                                        }
                                    }
                                }

                                b = true; TablesContainer.COUNT = TablesContainer.list10.Count;

                                {
                                    ViewBag.Count = TablesContainer.COUNT;
                                }

                                {
                                    ViewBag.GN_Found = HomeController.strN;
                                }

                                {
                                    ViewBag.Entity = "Labour_Relations";
                                }
                                break;
                            #endregion

                            #region Immunization 
                            case "Immunization":
                                var lst_locat11 = db.Community_Risks.Where(i => i.Location == Id_Location).ToList();
                                TablesContainer.list11 = db.Immunizations.ToList();
                                //(from ent in db.Immunizations where ent.Location == Id_Location select ent).ToList();
                                { ViewBag.ObjName = "Immunization"; }
                                if (TablesContainer.list11.Count() == 0)
                                {
                                    ViewBag.ErrorMsg = errorMsg = "Please select a date range.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }

                                strN = new List<string>();
                                var i1 = TablesContainer.list11.GroupBy(i => i.Numb_Res_Comm);
                                if (i1 != null)
                                {
                                    strN.Add("Numb Res Comm: ");
                                    foreach (var cc in i1)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var i2 = TablesContainer.list11.GroupBy(i => i.Numb_Res_Immun);
                                if (i2 != null)
                                {
                                    strN.Add("Numb Res Immun: ");
                                    foreach (var cc in i2)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var i3 = TablesContainer.list11.GroupBy(i => i.Numb_Res_Not_Immun);
                                if (i3 != null)
                                {
                                    strN.Add("Numb Res Not Immun: ");
                                    foreach (var cc in i3)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var i4 = TablesContainer.list11.GroupBy(i => i.Per_Res_Immun);
                                if (i4 != null)
                                {
                                    strN.Add("Per Res Immun: ");
                                    foreach (var cc in i4)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var i5 = TablesContainer.list11.GroupBy(i => i.Per_Res_Not_Immun);
                                if (i5 != null)
                                {
                                    strN.Add("Per Res Not Immun: ");
                                    foreach (var cc in i5)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                b = true; TablesContainer.COUNT = TablesContainer.list11.Count;

                                {
                                    ViewBag.Count = TablesContainer.COUNT;
                                }

                                {
                                    ViewBag.GN_Found = HomeController.strN;
                                }

                                {
                                    ViewBag.Entity = "Immunization";
                                }
                                break;
                            #endregion

                            #region Outbreaks    --none  
                            case "Outbreaks":
                                TablesContainer.list12 = (from ent in db.Outbreaks where ent.Date_Declared >= start && ent.Date_Declared <= end select ent).ToList();
                                return RedirectToAction($"../Statistics/{entity}");
                            #endregion

                            #region WSIB:
                            case "WSIB":
                                var lst_locat13 = db.WSIBs.Where(i => i.Location == Id_Location);
                                TablesContainer.list13 = (from ent in lst_locat13 where ent.Date_Accident >= start && ent.Date_Accident <= end select ent).ToList();
                                { ViewBag.ObjName = "WSIB"; }
                                if (TablesContainer.list13.Count() == 0)
                                {
                                    ViewBag.ErrorMsg = errorMsg = "Please select a date range.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }

                                strN = new List<string>();
                                var w1 = TablesContainer.list13.GroupBy(i => i.Date_Accident);
                                if (w1 != null)
                                {
                                    strN.Add("Date Accident: ");
                                    foreach (var cc in w1)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var w2 = TablesContainer.list13.GroupBy(i => i.Employee_Initials);
                                if (w2 != null)
                                {
                                    strN.Add("Employee Initials: ");
                                    foreach (var cc in w2)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var w3 = TablesContainer.list13.GroupBy(i => i.Accident_Cause);
                                if (w3 != null)
                                {
                                    strN.Add("Accident Cause: ");
                                    foreach (var cc in w3)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var w4 = TablesContainer.list13.GroupBy(i => i.Date_Duties);
                                if (w4 != null)
                                {
                                    strN.Add("Date Duties: ");
                                    foreach (var cc in w4)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var w5 = TablesContainer.list13.GroupBy(i => i.Date_Regular);
                                if (w5 != null)
                                {
                                    strN.Add("Date Regular: ");
                                    foreach (var cc in w5)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var w6 = TablesContainer.list13.GroupBy(i => i.Lost_Days);
                                if (w6 != null)
                                {
                                    strN.Add("Lost Days: ");
                                    foreach (var cc in w6)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var w7 = TablesContainer.list13.GroupBy(i => i.Modified_Days_Not_Shadowed);
                                if (w7 != null)
                                {
                                    strN.Add("Modified Days Not Shadowed: ");
                                    foreach (var cc in w7)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var w8 = TablesContainer.list13.GroupBy(i => i.Modified_Days_Shadowed);
                                if (w8 != null)
                                {
                                    strN.Add("Modified Days Shadowed: ");
                                    foreach (var cc in w8)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                b = true; TablesContainer.COUNT = TablesContainer.list10.Count;

                                {
                                    ViewBag.Count = TablesContainer.COUNT;
                                }

                                {
                                    ViewBag.GN_Found = HomeController.strN;
                                }

                                {
                                    ViewBag.Entity = "WSIB";
                                }
                                break;
                            #endregion

                            #region Not WSIB    
                            case "Not_WSIBs":
                                var lst_locat14 = db.Not_WSIBs.Where(i => i.Location == Id_Location);
                                TablesContainer.list14 = (from ent in lst_locat14 where ent.Date_of_Incident >= start && ent.Date_of_Incident <= end select ent).ToList();

                                { ViewBag.ObjName = "Not_WSIB"; }
                                if (TablesContainer.list13.Count() == 0)
                                {
                                    ViewBag.ErrorMsg = errorMsg = "Please select a date range.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }

                                strN = new List<string>();
                                var ww1 = TablesContainer.list14.GroupBy(i => i.Date_of_Incident);
                                if (ww1 != null)
                                {
                                    strN.Add("Date of Incident: ");
                                    foreach (var cc in ww1)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var ww2 = TablesContainer.list14.GroupBy(i => i.Employee_Initials);
                                if (ww2 != null)
                                {
                                    strN.Add("Employee Initials: ");
                                    foreach (var cc in ww2)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var ww3 = TablesContainer.list14.GroupBy(i => i.Position);
                                if (ww3 != null)
                                {
                                    strN.Add("Position: ");
                                    foreach (var cc in ww3)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var ww4 = TablesContainer.list14.GroupBy(i => i.Time_of_Incident);
                                if (ww4 != null)
                                {
                                    strN.Add("Time of Incident: ");
                                    foreach (var cc in ww4)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var ww5 = TablesContainer.list14.GroupBy(i => i.Shift);
                                if (ww5 != null)
                                {
                                    strN.Add("Shift : ");
                                    foreach (var cc in ww5)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var ww6 = TablesContainer.list14.GroupBy(i => i.Home_Area);
                                if (ww6 != null)
                                {
                                    strN.Add("Home Area: ");
                                    foreach (var cc in ww6)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var ww7 = TablesContainer.list14.GroupBy(i => i.Injury_Related);
                                if (ww7 != null)
                                {
                                    strN.Add("Injury Related: ");
                                    foreach (var cc in ww7)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var ww8 = TablesContainer.list14.GroupBy(i => i.Type_of_Injury);
                                if (ww8 != null)
                                {
                                    strN.Add("Type of Injury: ");
                                    foreach (var cc in ww8)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var ww9 = TablesContainer.list14.GroupBy(i => i.Details_of_Incident);
                                if (ww9 != null)
                                {
                                    strN.Add("Details of Incident: ");
                                    foreach (var cc in ww9)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                b = true; TablesContainer.COUNT = TablesContainer.list10.Count;

                                {
                                    ViewBag.Count = TablesContainer.COUNT;
                                }

                                {
                                    ViewBag.GN_Found = HomeController.strN;
                                }

                                {
                                    ViewBag.Entity = "WSIB";
                                }
                                break;
                                #endregion
                        }
                        #endregion
                    }
                }
                #endregion

                #region For Searching (with Range):
                else if (btnName.Equals("-search"))
                {
                    checkView = "search";
                    { ViewBag.Check = checkView; }
                    string searchName = Value.Id.ToString();
                    ViewBag.Check1 = "search" + Value.Name;

                    #region Critical Incidents Searching:
                    if (searchName != "0" && Value.Name == "1")
                    {
                        if (TablesContainer.list1.Count != 0)
                        {
                            //CI_Category_Type ci_found = db.CI_Category_Types.Where(n => n.Id == Value.Id).SingleOrDefault();
                            //if(Value.Id != 0)
                            //{
                            List<Critical_Incidents> found =
                                TablesContainer.list1.Where(ci => ci.CI_Category_Type == Value.Id).ToList();

                            { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                            ViewBag.List = found;
                            WorTabs tabs = new WorTabs();
                            tabs.ListForms = GetFormNames();
                            return View(tabs);
                            //}
                            //else
                            //{
                            //    ViewBag.ErrorMsg = $"There was nothing found for '{searchName}'.. Please try input correct CI Category Name!";
                            //    WorTabs tabs = new WorTabs();
                            //    tabs.ListForms = GetFormNames();
                            //    return View(tabs);
                            //}
                        }
                    }
                    #endregion

                    #region Complaints Searching:
                    else if (searchName != "0" && Value.Name == "2")
                    {
                        if (TablesContainer.list2.Count != 0)
                        {
                            List<Complaint> found =
                                TablesContainer.list2.Where(ci => ci.Department == Value.Id).ToList();

                            { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                            ViewBag.List = found;
                            WorTabs tabs = new WorTabs();
                            tabs.ListForms = GetFormNames();
                            return View(tabs);
                        }
                    }
                    #endregion
                }
                #endregion
            }
            // Without Range:
            else if (w_without == "-without" || w_without == "-filter")  // If we selected All With Datas:
            {
                #region Grab to get button names:
                string btnName = Request.Params
                     .Cast<string>()
                     .Where(p => p.StartsWith("btn"))
                     .Select(p => p.Substring("btn".Length))
                     .First();

                string filter = Value.Filter;
                #endregion

                #region For Showing List (Without Range):
                if (btnName.Equals("-list") || btnName.Equals("-upSort") || btnName.Equals("-downSort"))
                {
                    {
                        ViewBag.Check = "list";
                    }
                    checkView = "list";
                    {
                        ViewBag.Tbl = Value.Name;
                    }
                    ViewBag.IsAdmin = isAdmin;
                    ViewBag.Check = checkView;
                    string name = Value.Name;

                    start = Value.Start;
                    end = Value.End;
                    if (role == Role.Admin)
                    {
                        ViewBag.Welcome = Role.Admin;
                        switch (Value.Name)
                        {
                            case "1":
                                List<Critical_Incidents> lst1 = db.Critical_Incidents.ToList();
                                TablesContainer.list1 = lst1.OrderBy(x => x.Date).ToList();
                                if (btnName.Equals("-upSort"))
                                {
                                    TablesContainer.list1 = lst1.OrderBy(x => x.Date).ToList();
                                }
                                else if (btnName.Equals("-downSort"))
                                {
                                    TablesContainer.list1 = lst1.OrderByDescending(x => x.Date).ToList();
                                }

                                #region If We selected any item from dropdown Filter:
                                //if (filter != null)
                                //{
                                //    switch (filter)
                                //    {
                                //        case "1 month back":
                                //            DateTime oneBack1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, DateTime.Now.Day);
                                //            TablesContainer.list1 = (from it in db.Critical_Incidents
                                //                                     where it.Date <= DateTime.Now && it.Date >= oneBack1
                                //                                     select it).ToList();
                                //            ViewBag.CheckEmpty = isEmpty;
                                //            if (TablesContainer.list1.Count == 0)
                                //            {
                                //                ViewBag.CheckEmpty = isEmpty = true;
                                //                ViewBag.EmptyFilter = "Nothing was found... Try selecting another date.";
                                //            }
                                //            break;
                                //        case "2 month back":
                                //            DateTime oneBack2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, DateTime.Now.Day);
                                //            TablesContainer.list1 = (from it in db.Critical_Incidents
                                //                                     where it.Date <= DateTime.Now && it.Date >= oneBack2
                                //                                     select it).ToList();
                                //            ViewBag.CheckEmpty = isEmpty;
                                //            if (TablesContainer.list1.Count == 0)
                                //            {
                                //                ViewBag.CheckEmpty = isEmpty = true;
                                //                ViewBag.EmptyFilter = "Nothing was found... Try selecting another date.";
                                //            }
                                //            break;
                                //        case "3 month back":
                                //            DateTime oneBack3 = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, DateTime.Now.Day);
                                //            TablesContainer.list1 = (from it in db.Critical_Incidents
                                //                                     where it.Date <= DateTime.Now && it.Date >= oneBack3
                                //                                     select it).ToList();
                                //            ViewBag.CheckEmpty = isEmpty;
                                //            if (TablesContainer.list1.Count == 0)
                                //            {
                                //                ViewBag.CheckEmpty = isEmpty = true;
                                //                ViewBag.EmptyFilter = "Nothing was found... Try selecting another date.";
                                //            }
                                //            break;
                                //    }
                                //}
                                #endregion

                                if (TablesContainer.list1.Count != 0 || TablesContainer.list1 != null)
                                {
                                    { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                    { ViewBag.CiNames = STREAM.GetCINames().ToArray(); }
                                    ViewBag.List = TablesContainer.list1;
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                                else
                                {
                                    ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                            case "2":
                                ViewBag.Department = list20;
                                var lst2 = db.Complaints;
                                TablesContainer.list2 = db.Complaints.OrderBy(x => x.DateReceived).ToList();
                                if (TablesContainer.list2.Count != 0 || TablesContainer.list2 != null)
                                {
                                    { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                    ViewBag.List = TablesContainer.list2;
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                                else
                                {
                                    ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                            case "3":
                                var lst3 = db.Good_News.ToList();
                                TablesContainer.list3 = db.Good_News.OrderBy(x => x.DateNews).ToList();
                                if (TablesContainer.list3.Count != 0 || TablesContainer.list3 != null)
                                {
                                    { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                    ViewBag.List = TablesContainer.list3;
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                                else
                                {
                                    ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                            case "4":
                                var lst4 = db.Emergency_Prep.ToList();

                                if (TablesContainer.list3.Count != 0 || TablesContainer.list3 != null)
                                {
                                    { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                    ViewBag.List = lst4;
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                                else
                                {
                                    ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                            case "5":
                                TablesContainer.list5 = db.Community_Risks.ToList();
                                if (TablesContainer.list5.Count != 0 || TablesContainer.list5 != null)
                                {
                                    { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                    ViewBag.List = TablesContainer.list5;
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                                else
                                {
                                    ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                            case "6":
                                TablesContainer.list6 = db.Visits_Others.ToList();
                                if (TablesContainer.list6.Count != 0 || TablesContainer.list6 != null)
                                {
                                    { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                    ViewBag.List = TablesContainer.list6;
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                                else
                                {
                                    ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                            case "7":
                                TablesContainer.list7 = db.Privacy_Breaches.ToList();
                                if (TablesContainer.list7.Count != 0 || TablesContainer.list7 != null)
                                {
                                    { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                    ViewBag.List = TablesContainer.list7;
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                                else
                                {
                                    ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                            case "8":
                                TablesContainer.list8 = db.Privacy_Complaints.ToList();
                                if (TablesContainer.list8.Count != 0 || TablesContainer.list8 != null)
                                {
                                    { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                    ViewBag.List = TablesContainer.list8;
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                                else
                                {
                                    ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                            case "9":
                                TablesContainer.list9 = db.Educations.ToList();
                                if (TablesContainer.list9.Count != 0 || TablesContainer.list9 != null)
                                {
                                    { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                    ViewBag.List = TablesContainer.list9;
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                                else
                                {
                                    ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                            case "10":
                                TablesContainer.list10 = db.Relations.ToList();
                                if (TablesContainer.list10.Count != 0 || TablesContainer.list11 != null)
                                {
                                    { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                    ViewBag.List = TablesContainer.list10;
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                                else
                                {
                                    ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                            case "11":
                                TablesContainer.list11 = db.Immunizations.ToList();
                                if (TablesContainer.list11.Count != 0 || TablesContainer.list11 != null)
                                {
                                    { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                    ViewBag.List = TablesContainer.list11;
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                                else
                                {
                                    ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                            case "12":
                                TablesContainer.list12 = db.Outbreaks.ToList();
                                if (TablesContainer.list12.Count != 0 || TablesContainer.list12 != null)
                                {
                                    { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                    ViewBag.List = TablesContainer.list12;
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                                else
                                {
                                    ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                            case "13":
                                TablesContainer.list13 = db.WSIBs.ToList();
                                if (TablesContainer.list13.Count != 0 || TablesContainer.list13 != null)
                                {
                                    { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                    ViewBag.List = TablesContainer.list13;
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                                else
                                {
                                    ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                            case "14":
                                TablesContainer.list14 = db.Not_WSIBs.ToList();
                                if (TablesContainer.list14.Count != 0 || TablesContainer.list14 != null)
                                {
                                    { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                    ViewBag.List = TablesContainer.list14;
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                                else
                                {
                                    ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                        }
                    }
                    else if (role == Role.User)
                    {
                        ViewBag.Welcome = Role.User;
                        switch (Value.Name)
                        {
                            case "1":
                                List<Critical_Incidents> lst1 = (from c in db.Critical_Incidents where c.Location == Id_Location select c).ToList();
                                TablesContainer.list1 = lst1.OrderBy(x => x.Date).ToList();
                                if (btnName.Equals("-upSort"))
                                {
                                    TablesContainer.list1 = lst1.OrderBy(x => x.Date).ToList();
                                }
                                else if (btnName.Equals("-downSort"))
                                {
                                    TablesContainer.list1 = lst1.OrderByDescending(x => x.Date).ToList();
                                }
                                if (TablesContainer.list1.Count != 0 || TablesContainer.list1 != null)
                                {
                                    { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                    { ViewBag.CiNames = STREAM.GetCINames().ToArray(); }
                                    ViewBag.List = TablesContainer.list1;
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();

                                    return View(tabs);
                                }
                                else
                                {
                                    ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                            case "2":
                                ViewBag.Department = list20;
                                TablesContainer.list2 = db.Complaints.Where(i => i.Location == Id_Location).ToList();
                                if (TablesContainer.list2.Count != 0 || TablesContainer.list2 != null)
                                {
                                    { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                    ViewBag.List = TablesContainer.list2;
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                                else
                                {
                                    ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                            case "3":
                                TablesContainer.list3 = db.Good_News.Where(i => i.Location == Id_Location).ToList(); if (TablesContainer.list3.Count != 0 || TablesContainer.list3 != null)
                                {
                                    { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                    ViewBag.List = TablesContainer.list3;
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                                else
                                {
                                    ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                            case "4":
                                var lst4 = db.Emergency_Prep.Where(i => i.Location == Id_Location).ToList();
                                if (TablesContainer.list3.Count != 0 || TablesContainer.list3 != null)
                                {
                                    { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                    ViewBag.List = lst4;
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                                else
                                {
                                    ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                            case "5":
                                TablesContainer.list5 = db.Community_Risks.Where(i => i.Location == Id_Location).ToList();
                                if (TablesContainer.list5.Count != 0 || TablesContainer.list5 != null)
                                {
                                    { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                    ViewBag.List = TablesContainer.list5;
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                                else
                                {
                                    ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                            case "6":
                                TablesContainer.list6 = db.Visits_Others.Where(i => i.Location == Id_Location).ToList();
                                if (TablesContainer.list6.Count != 0 || TablesContainer.list6 != null)
                                {
                                    { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                    ViewBag.List = TablesContainer.list6;
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                                else
                                {
                                    ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                            case "7":
                                TablesContainer.list7 = db.Privacy_Breaches.Where(i => i.Location == Id_Location).ToList();
                                if (TablesContainer.list7.Count != 0 || TablesContainer.list7 != null)
                                {
                                    { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                    ViewBag.List = TablesContainer.list7;
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                                else
                                {
                                    ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                            case "8":
                                TablesContainer.list8 = db.Privacy_Complaints.Where(i => i.Location == Id_Location).ToList();
                                if (TablesContainer.list8.Count != 0 || TablesContainer.list8 != null)
                                {
                                    { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                    ViewBag.List = TablesContainer.list8;
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                                else
                                {
                                    ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                            case "9":
                                TablesContainer.list9 = db.Educations.Where(i => i.Location == Id_Location).ToList();
                                if (TablesContainer.list9.Count != 0 || TablesContainer.list9 != null)
                                {
                                    { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                    ViewBag.List = TablesContainer.list9;
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                                else
                                {
                                    ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                            case "10":
                                TablesContainer.list10 = db.Relations.Where(i => i.Location == Id_Location).ToList();
                                if (TablesContainer.list10.Count != 0 || TablesContainer.list11 != null)
                                {
                                    { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                    ViewBag.List = TablesContainer.list10;
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                                else
                                {
                                    ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                            case "11":
                                TablesContainer.list11 = db.Immunizations.Where(i => i.Location == Id_Location).ToList();
                                if (TablesContainer.list11.Count != 0 || TablesContainer.list11 != null)
                                {
                                    { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                    ViewBag.List = TablesContainer.list11;
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                                else
                                {
                                    ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                            case "12":
                                TablesContainer.list12 = db.Outbreaks.Where(i => i.Location == Id_Location).ToList();
                                if (TablesContainer.list12.Count != 0 || TablesContainer.list12 != null)
                                {
                                    { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                    ViewBag.List = TablesContainer.list12;
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                                else
                                {
                                    ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                            case "13":
                                TablesContainer.list13 = db.WSIBs.Where(i => i.Location == Id_Location).ToList();
                                if (TablesContainer.list13.Count != 0 || TablesContainer.list13 != null)
                                {
                                    { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                    ViewBag.List = TablesContainer.list13;
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                                else
                                {
                                    ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                            case "14":
                                TablesContainer.list14 = db.Not_WSIBs.Where(i => i.Location == Id_Location).ToList();
                                if (TablesContainer.list14.Count != 0 || TablesContainer.list14 != null)
                                {
                                    { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                    ViewBag.List = TablesContainer.list14;
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                                else
                                {
                                    ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                        }
                    }
                    else
                    {
                        if (Id_Location == 0)
                        {
                            switch (Value.Name)
                            {
                                case "1":
                                    TablesContainer.list1 = db.Critical_Incidents.Where(i => i.Location == Id_Location).ToList();
                                    if (TablesContainer.list1.Count != 0 || TablesContainer.list1 != null)
                                    {
                                        { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                        { ViewBag.CiNames = STREAM.GetCINames().ToArray(); }
                                        ViewBag.List = TablesContainer.list1;
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                case "2":
                                    var var = TablesContainer.list2 = db.Complaints.Where(i => i.Location == Id_Location).ToList();
                                    if (var == null)
                                    {
                                        ViewBag.List = "The table is EMPTY!";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    if (TablesContainer.list2.Count != 0 || TablesContainer.list2 != null)
                                    {
                                        { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                        ViewBag.List = TablesContainer.list2;
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                case "3":  // Good_News table
                                    TablesContainer.list3 = db.Good_News.Where(i => i.Location == Id_Location).ToList();
                                    if (TablesContainer.list3.Count != 0 || TablesContainer.list3 != null)
                                    {
                                        { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                        ViewBag.List = TablesContainer.list3;
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                            }
                        }
                        else
                        {
                            switch (Value.Name)
                            {
                                case "1":
                                    TablesContainer.list1 = db.Critical_Incidents.Where(i => i.Location == Id_Location).ToList();
                                    if (TablesContainer.list1.Count != 0 || TablesContainer.list1 != null)
                                    {
                                        { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                        { ViewBag.CiNames = STREAM.GetCINames().ToArray(); }
                                        ViewBag.List = TablesContainer.list1;
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                case "2":
                                    var var = TablesContainer.list2 = db.Complaints.Where(i => i.Location == Id_Location).ToList();
                                    if (var == null)
                                    {
                                        ViewBag.List = "The table is EMPTY!";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    if (TablesContainer.list2.Count != 0 || TablesContainer.list2 != null)
                                    {
                                        { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                        ViewBag.List = TablesContainer.list2;
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                case "3":  // Good_News table
                                    TablesContainer.list3 = db.Good_News.Where(i => i.Location == Id_Location).ToList();
                                    if (TablesContainer.list3.Count != 0 || TablesContainer.list3 != null)
                                    {
                                        { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
                                        ViewBag.List = TablesContainer.list3;
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMsg = errorMsg = "That range doesn't contain any records.";
                                        WorTabs tabs = new WorTabs();
                                        tabs.ListForms = GetFormNames();
                                        return View(tabs);
                                    }
                            }
                        }
                    }
                }

                #endregion

                #region For Export to .csv file ((Without Range))
                else if (btnName.Equals("-export"))
                {
                    start = Value.Start;
                    end = Value.End;
                    int id = 0;
                    try
                    {
                        id = int.Parse(Value.Name);
                    }
                    catch { }
                    var tbl_list = GetTableById(id).ToArray().ToList();
                    Type type = tbl_list[0].GetType();
                    string entity = type.Name;
                    object model = Searcher.FindObjByName(entity);
                    if (model.GetType() == typeof(Critical_Incidents))
                    {
                        model_name = model.GetType().Name;
                        if (role == Role.Admin)
                            TablesContainer.list1 = db.Critical_Incidents.ToList();
                        else
                            TablesContainer.list1 = db.Critical_Incidents.Where(i => i.Location == Id_Location).ToList();
                        // new STREAM().WriteToCSV(query1); // to be continue..
                        return RedirectToAction("../Home/ExportToCSV");
                    }
                    else if (model.GetType() == typeof(Good_News))
                    {
                        model_name = model.GetType().Name;
                        if (role == Role.Admin)
                            TablesContainer.list3 = db.Good_News.ToList();
                        else
                            TablesContainer.list3 = db.Good_News.Where(i => i.Location == Id_Location).ToList();
                        return RedirectToAction("../Home/ExportToCSV");
                    }
                    else if (model.GetType() == typeof(Complaint))
                    {
                        model_name = model.GetType().Name;
                        if (role == Role.Admin)
                            TablesContainer.list2 = db.Complaints.ToList();
                        if (role == Role.Admin)
                            TablesContainer.list2 = db.Complaints.Where(i => i.Location == Id_Location).ToList();
                        return RedirectToAction("../Home/ExportToCSV");
                    }
                    else if (model.GetType() == typeof(Community_Risks))
                    {
                        model_name = model.GetType().Name;
                        if (role == Role.Admin)
                            TablesContainer.list5 = db.Community_Risks.ToList();
                        else
                            TablesContainer.list5 = db.Community_Risks.Where(i => i.Location == Id_Location).ToList();
                        return RedirectToAction("../Home/ExportToCSV");
                    }
                    else if (model.GetType() == typeof(Labour_Relations))
                    {
                        model_name = model.GetType().Name;
                        if (role == Role.Admin)
                            TablesContainer.list10 = db.Relations.ToList();
                        else
                            TablesContainer.list10 = db.Relations.Where(i => i.Location == Id_Location).ToList();
                        return RedirectToAction("../Home/ExportToCSV");
                    }
                    else if (model.GetType() == typeof(Emergency_Prep))
                    {
                        model_name = model.GetType().Name;
                        if (role == Role.Admin)
                            TablesContainer.list4 = db.Emergency_Prep.ToList();
                        else
                            TablesContainer.list4 = db.Emergency_Prep.Where(i => i.Location == Id_Location).ToList();
                        return RedirectToAction("../Home/ExportToCSV");
                    }
                    else if (model.GetType() == typeof(Visits_Others))
                    {
                        model_name = model.GetType().Name;
                        if (role == Role.Admin)
                            TablesContainer.list6 = db.Visits_Others.ToList();
                        else
                            TablesContainer.list6 = db.Visits_Others.Where(i => i.Location == Id_Location).ToList();
                        return RedirectToAction("../Home/ExportToCSV");
                    }
                    else if (model.GetType() == typeof(Privacy_Breaches))
                    {
                        model_name = model.GetType().Name;
                        if (role == Role.Admin)
                            TablesContainer.list7 = db.Privacy_Breaches.ToList();
                        else
                            TablesContainer.list7 = db.Privacy_Breaches.Where(i => i.Location == Id_Location).ToList();
                        return RedirectToAction("../Home/ExportToCSV");
                    }
                    else if (model.GetType() == typeof(Privacy_Complaints))
                    {
                        model_name = model.GetType().Name;
                        if (role == Role.Admin)
                            TablesContainer.list8 = db.Privacy_Complaints.ToList();
                        else
                            TablesContainer.list8 = db.Privacy_Complaints.Where(i => i.Location == Id_Location).ToList();
                        return RedirectToAction("../Home/ExportToCSV");
                    }
                    else if (model.GetType() == typeof(Education))
                    {
                        model_name = model.GetType().Name;
                        if (role == Role.Admin)
                            TablesContainer.list9 = db.Educations.ToList();
                        else
                            TablesContainer.list9 = db.Educations.Where(i => i.Location == Id_Location).ToList();
                        return RedirectToAction("../Home/ExportToCSV");
                    }
                    else if (model.GetType() == typeof(Labour_Relations))
                    {
                        model_name = model.GetType().Name;
                        if (role == Role.Admin)
                            TablesContainer.list10 = db.Relations.ToList();
                        else
                            TablesContainer.list10 = db.Relations.Where(i => i.Location == Id_Location).ToList();
                        return RedirectToAction("../Home/ExportToCSV");
                    }
                    else if (model.GetType() == typeof(Immunization))
                    {
                        model_name = model.GetType().Name;
                        if (role == Role.Admin)
                            TablesContainer.list11 = db.Immunizations.ToList();
                        else
                            TablesContainer.list11 = db.Immunizations.Where(i => i.Location == Id_Location).ToList();
                        return RedirectToAction("../Home/ExportToCSV");
                    }
                    else if (model.GetType() == typeof(Outbreaks))
                    {
                        model_name = model.GetType().Name;
                        if (role == Role.Admin)
                            TablesContainer.list12 = db.Outbreaks.ToList();
                        else
                            TablesContainer.list12 = db.Outbreaks.Where(i => i.Location == Id_Location).ToList();
                        return RedirectToAction("../Home/ExportToCSV");
                    }
                    else if (model.GetType() == typeof(WSIB))
                    {
                        model_name = model.GetType().Name;
                        if (role == Role.Admin)
                            TablesContainer.list13 = db.WSIBs.ToList();
                        else
                            TablesContainer.list13 = db.WSIBs.Where(i => i.Location == Id_Location).ToList();
                        return RedirectToAction("../Home/ExportToCSV");
                    }
                    else if (model.GetType() == typeof(Not_WSIBs))
                    {
                        model_name = model.GetType().Name;
                        if (role == Role.Admin)
                            TablesContainer.list14 = db.Not_WSIBs.ToList();
                        else
                            TablesContainer.list14 = db.Not_WSIBs.Where(i => i.Location == Id_Location).ToList();
                        return RedirectToAction("../Home/ExportToCSV");
                    }
                    else
                    {
                        ViewBag.ErrorMsg = errorMsg = "There was nothing found within the date range that was chosen.";
                        WorTabs tabs = new WorTabs();
                        tabs.ListForms = GetFormNames();
                        return View(tabs);
                    }
                }
                #endregion

                #region For Summary:
                else if (btnName.Equals("-summary"))
                {
                    checkView = "summary";
                    ViewBag.Check = checkView;
                    start = Value.Start;
                    end = Value.End;
                    int id = 0;
                    try
                    {
                         id = num_tbl = int.Parse(Value.Name);
                    }
                    catch { }
                    if (id == 11) Id_Location = 1;
                    var tbl_list = GetTableById(id).ToArray().ToList();
                    Type type = tbl_list[0].GetType();
                    string entity = type.Name;
                    if (!entity.Equals(string.Empty))
                    {
                        ViewBag.TableName = entity;
                    }

                    #region Switch to show all object's Statistic:
                    switch (entity)
                    {
                        #region Critical_Incident:
                        case "Critical_Incidents":
                            ClearAllStatic();
                            if (role == Role.Admin) 
                                TablesContainer.list1 = db.Critical_Incidents.ToList();
                            else if(role == Role.User)
                                TablesContainer.list1 = db.Critical_Incidents.Where(i => i.Location == Id_Location).ToList();
                            // Accounting param name for Location:
                            
                            if (!checkRepead)
                            {
                                checkRepead = true;
                                foreach (var cc in TablesContainer.list1)
                                {
                                    if (STREAM.GetLocNameById(cc.Location).Contains("Altamont Care Community"))
                                        cnt1++;
                                    else if (STREAM.GetLocNameById(cc.Location).Contains("Astoria Retirement Residence"))
                                        cnt2++;
                                    else if (STREAM.GetLocNameById(cc.Location).Contains("Barnswallow Place Care Community"))
                                        cnt3++;
                                    else if (STREAM.GetLocNameById(cc.Location).Contains("Bearbrook Retirement Residence"))
                                        cnt4++;
                                    else if (STREAM.GetLocNameById(cc.Location).Contains("Bloomington Cove Care Community"))
                                        cnt5++;
                                    else if (STREAM.GetLocNameById(cc.Location).Contains("Bradford Valley Care Community"))
                                        cnt6++;
                                    else if (STREAM.GetLocNameById(cc.Location).Contains("Brookside Lodge"))
                                        cnt7++;
                                    else if (STREAM.GetLocNameById(cc.Location).Contains("Woodbridge Vista"))
                                        cnt8++;
                                    else if (STREAM.GetLocNameById(cc.Location).Contains("Norfinch"))
                                        cnt9++;
                                    else if (STREAM.GetLocNameById(cc.Location).Contains("Rideau"))
                                        cnt10++;
                                    else if (STREAM.GetLocNameById(cc.Location).Contains("Villa da Vinci"))
                                        cnt11++;
                                }

                                CriticalIncidentSummary model = new CriticalIncidentSummary();
                                var locDistinct = new HashSet<string>();
                                var locId = new List<int>();
                                foreach (var it in TablesContainer.list1)
                                {
                                    Care_Community cc = db.Care_Communities.Find(it.Location);
                                    locDistinct.Add(cc.Name);
                                    locId.Add(cc.Id);
                                }

                                locList = locDistinct.ToList();

                                #region Fill out lists ll1,ll2,ll3...ll11 existing locations:
                                for (var i = 0; i < locList.Count; i++)
                                {
                                    if (locList[i].Contains("Altamont Care Community"))
                                    {
                                        ll1 = TablesContainer.list1.Where
                                     (loc => STREAM.GetLocNameById(loc.Location) == "Altamont Care Community\r\n").ToList();
                                    }
                                    else if (locList[i].Contains("Astoria Retirement Residence"))
                                    {
                                        ll2 = TablesContainer.list1.Where
                                           (loc => STREAM.GetLocNameById(loc.Location) == "Astoria Retirement Residence\r\n").ToList();
                                    }
                                    else if (locList[i].Contains("Barnswallow Place Care Community"))
                                    {
                                        ll3 = TablesContainer.list1.Where
                                      (loc => STREAM.GetLocNameById(loc.Location) == "Barnswallow Place Care Community\r\n").ToList();
                                    }
                                    else if (locList[i].Contains("Bearbrook Retirement Residence"))
                                    {
                                        ll4 = TablesContainer.list1.Where
                                      (loc => STREAM.GetLocNameById(loc.Location) == "Bearbrook Retirement Residence\r\n").ToList();
                                    }
                                    else if (locList[i].Contains("Bloomington Cove Care Community"))
                                    {
                                        ll5 = TablesContainer.list1.Where
                                       (loc => STREAM.GetLocNameById(loc.Location) == "Bloomington Cove Care Community\r\n").ToList();
                                    }
                                    else if (locList[i].Contains("Bradford Valley Care Community"))
                                    {
                                        ll6 = TablesContainer.list1.Where
                                        (loc => STREAM.GetLocNameById(loc.Location) == "Bradford Valley Care Community\r\n").ToList();
                                    }
                                    else if (locList[i].Contains("Brookside Lodge"))
                                    {
                                        ll7 = TablesContainer.list1.Where
                                       (loc => STREAM.GetLocNameById(loc.Location) == "Brookside Lodge\r\n").ToList();
                                    }
                                    else if (locList[i].Contains("Woodbridge Vista"))
                                    {
                                        string retName = STREAM.GetLocNameById(8);
                                        ll8 = TablesContainer.list1.Where
                                        (loc => STREAM.GetLocNameById(loc.Location) == "Woodbridge Vista").ToList();
                                    }
                                    else if (locList[i].Contains("Norfinch"))
                                    {
                                        ll9 = TablesContainer.list1.Where
                                        (loc => STREAM.GetLocNameById(loc.Location) == "Norfinch\r\n").ToList();
                                    }
                                    else if (locList[i].Contains("Rideau"))
                                    {
                                        ll10 = TablesContainer.list1.Where
                                      (loc => STREAM.GetLocNameById(loc.Location) == "Rideau\r\n").ToList();
                                    }
                                    else if (locList[i].Contains("Villa da Vinci"))
                                    {
                                        ll11 = TablesContainer.list1.Where
                                        (loc => STREAM.GetLocNameById(loc.Location) == "Villa da Vinci\r\n").ToList();
                                    }
                                }
                                #endregion

                                #region Add count location for each exist:
                                for (var i = 0; i < locList.Count; i++)
                                {
                                    if (locList[i].Contains("Altamont Care Community"))
                                        locList[i] = locList[i] + " - " + cnt1;
                                    else if (locList[i].Contains("Astoria Retirement Residence"))
                                        locList[i] = locList[i] + " - " + cnt2;
                                    else if (locList[i].Contains("Barnswallow Place Care Community"))
                                        locList[i] = locList[i] + " - " + cnt3;
                                    else if (locList[i].Contains("Bearbrook Retirement Residence"))
                                        locList[i] = locList[i] + " - " + cnt4;
                                    else if (locList[i].Contains("Bloomington Cove Care Community"))
                                        locList[i] = locList[i] + " - " + cnt5;
                                    else if (locList[i].Contains("Bradford Valley Care Community"))
                                        locList[i] = locList[i] + " - " + cnt6;
                                    else if (locList[i].Contains("Brookside Lodge"))
                                        locList[i] = locList[i] + " - " + cnt7;
                                    else if (locList[i].Contains("Woodbridge Vista"))
                                        locList[i] = locList[i] + " - " + cnt8;
                                    else if (locList[i].Contains("Norfinch"))
                                        locList[i] = locList[i] + " - " + cnt9;
                                    else if (locList[i].Contains("Rideau"))
                                        locList[i] = locList[i] + " - " + cnt10;
                                    else if (locList[i].Contains("Villa da Vinci"))
                                        locList[i] = locList[i] + " - " + cnt11;
                                }
                                #endregion

                                locList.Sort(); // Sorted by alphanumeric

                                if (TablesContainer.list1.Count() == 0)
                                {
                                    { ViewBag.ObjName = entity; }
                                    ViewBag.ErrorMsg = errorMsg = "Please select a date range.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }

                                TablesContainer.COUNT = TablesContainer.list1.Count;

                                #region For the 1st Location:
                                if (ll1 != null)
                                {
                                    model.LocationName = locList.Find(i => i == "Altamont Care Community\r\n" + " - " + cnt1);
                                    var attr11 = ll1.GroupBy(i => i.MOHLTC_Follow_Up);
                                    if (attr11 != null)
                                    {
                                        foreach (var cc in attr11)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.MOHLTC_Follow_Up += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p1 += cc.Count();
                                        }
                                    }

                                    var attr10 = ll1.GroupBy(i => i.CIS_Initiated);
                                    if (attr10 != null)
                                    {
                                        foreach (var cc in attr10)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.CIS_Initiated += $"{key}\t - \t{cc.Count()}" + " | "; p2 += cc.Count();
                                        }
                                    }

                                    var attr7 = ll1.GroupBy(i => i.MOH_Notified);
                                    if (attr7 != null)
                                    {
                                        foreach (var cc in attr7)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.MOH_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p3 += cc.Count();
                                        }
                                    }

                                    var attr2 = ll1.GroupBy(i => i.POAS_Notified);
                                    if (attr2 != null)
                                    {
                                        foreach (var d in attr2)
                                        {
                                            string key = d.Key == null ? "NULL" : d.Key;
                                            if (key == "NULL") continue;
                                            model.POAS_Notified += $"{key}\t - \t{d.Count()}" + " | "; p4 += d.Count();
                                        }
                                    }

                                    var attr4 = ll1.GroupBy(i => i.Police_Notified);
                                    if (attr4 != null)
                                    {
                                        foreach (var cc in attr4)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Police_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p5 += cc.Count();
                                        }
                                    }


                                    var attr3 = ll1.GroupBy(i => i.Quality_Improvement_Actions);
                                    if (attr3 != null)
                                    {
                                        foreach (var cc in attr3)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Quality_Improvement_Actions += $"{key}\t - \t{cc.Count()}" + " | "; p6 += cc.Count();
                                        }
                                    }

                                    var attr8 = ll1.GroupBy(i => i.Risk_Locked);
                                    if (attr8 != null)
                                    {
                                        foreach (var cc in attr8)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Risk_Locked += $"{key}\t - \t{cc.Count()}" + " | "; p7 += cc.Count();
                                        }
                                    }

                                    var attr5 = ll1.GroupBy(i => i.Brief_Description);
                                    if (attr5 != null)
                                    {
                                        foreach (var cc in attr5)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Brief_Description += $"{key}\t - \t{cc.Count()}" + " | "; p8 += cc.Count();
                                        }
                                    }

                                    var attr1 = ll1.GroupBy(i => i.Care_Plan_Updated);
                                    if (attr1 != null)
                                    {
                                        foreach (var e in attr1)
                                        {
                                            string key = e.Key == null ? "NULL" : e.Key;
                                            if (key == "NULL") continue;
                                            model.Care_Plan_Updated += $"{key}\t - \t{e.Count()}" + " | "; p9 += e.Count();
                                        }
                                    }

                                    var attr13 = ll1.GroupBy(i => i.CI_Form_Number);
                                    if (attr13 != null)
                                    {
                                        int count = 0;
                                        foreach (var cc in attr13)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            count += cc.Count();
                                        }
                                        model.CI_Form_Number = $"All\t - \t{count}";
                                        p10 += count;
                                    }

                                    var attr00 = ll1.GroupBy(i => i.File_Complete);
                                    if (attr00 != null)
                                    {
                                        foreach (var cc in attr00)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.File_Complete += $"{key}\t - \t{cc.Count()}" + " | "; p11 += cc.Count();
                                        }
                                    }

                                    var attr111 = ll1.GroupBy(i => i.Follow_Up_Amendments);
                                    if (attr111 != null)
                                    {
                                        foreach (var cc in attr111)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Follow_Up_Amendments += $"{key}\t - \t{cc.Count()}" + " | "; p12 += cc.Count();
                                        }
                                    }
                                    foundSummary1.Add(model);
                                    model = new CriticalIncidentSummary();
                                }
                                #endregion

                                #region 2nd Location:
                                if (ll2 != null)
                                {
                                    model.LocationName = locList.Find(i => i == "Astoria Retirement Residence\r\n" + " - " + cnt2);
                                    var attr11 = ll2.GroupBy(i => i.MOHLTC_Follow_Up);
                                    if (attr11 != null)
                                    {
                                        foreach (var cc in attr11)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.MOHLTC_Follow_Up += $"{key}\t - \t{cc.Count()}" + " | "; p1 += cc.Count();
                                        }
                                    }

                                    var attr10 = ll2.GroupBy(i => i.CIS_Initiated);
                                    if (attr10 != null)
                                    {
                                        foreach (var cc in attr10)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.CIS_Initiated += $"{key}\t - \t{cc.Count()}" + " | "; p2 += cc.Count();
                                        }
                                    }

                                    var attr7 = ll2.GroupBy(i => i.MOH_Notified);
                                    if (attr7 != null)
                                    {
                                        foreach (var cc in attr7)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.MOH_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p3 += cc.Count();
                                        }
                                    }

                                    var attr2 = ll2.GroupBy(i => i.POAS_Notified);
                                    if (attr2 != null)
                                    {
                                        foreach (var d in attr2)
                                        {
                                            string key = d.Key == null ? "NULL" : d.Key;
                                            if (key == "NULL") continue;
                                            model.POAS_Notified += $"{key}\t - \t{d.Count()}" + " | "; p4 += d.Count();
                                        }
                                    }

                                    var attr4 = ll2.GroupBy(i => i.Police_Notified);
                                    if (attr4 != null)
                                    {
                                        foreach (var cc in attr4)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Police_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p5 += cc.Count();
                                        }
                                    }


                                    var attr3 = ll2.GroupBy(i => i.Quality_Improvement_Actions);
                                    if (attr3 != null)
                                    {
                                        foreach (var cc in attr3)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Quality_Improvement_Actions += $"{key}\t - \t{cc.Count()}" + " | "; p6 += cc.Count();
                                        }
                                    }

                                    var attr8 = ll2.GroupBy(i => i.Risk_Locked);
                                    if (attr8 != null)
                                    {
                                        foreach (var cc in attr8)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Risk_Locked += $"{key}\t - \t{cc.Count()}" + " | "; p7 += cc.Count();
                                        }
                                    }

                                    var attr5 = ll2.GroupBy(i => i.Brief_Description);
                                    if (attr5 != null)
                                    {
                                        foreach (var cc in attr5)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Brief_Description += $"{key}\t - \t{cc.Count()}" + " | "; p8 += cc.Count();
                                        }
                                    }

                                    var attr1 = ll2.GroupBy(i => i.Care_Plan_Updated);
                                    if (attr1 != null)
                                    {
                                        foreach (var e in attr1)
                                        {
                                            string key = e.Key == null ? "NULL" : e.Key;
                                            if (key == "NULL") continue;
                                            model.Care_Plan_Updated += $"{key}\t - \t{e.Count()}" + " | "; p9 += e.Count();
                                        }
                                    }

                                    var attr13 = ll2.GroupBy(i => i.CI_Form_Number);
                                    if (attr13 != null)
                                    {
                                        int count = 0;
                                        foreach (var cc in attr13)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            count += cc.Count();
                                        }
                                        model.CI_Form_Number = $"All\t - \t{count}"; p10 += count;
                                    }

                                    var attr00 = ll2.GroupBy(i => i.File_Complete);
                                    if (attr00 != null)
                                    {
                                        foreach (var cc in attr00)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.File_Complete += $"{key}\t - \t{cc.Count()}" + " | "; p11 += cc.Count();
                                        }
                                    }

                                    var attr111 = ll2.GroupBy(i => i.Follow_Up_Amendments);
                                    if (attr111 != null)
                                    {
                                        foreach (var cc in attr111)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Follow_Up_Amendments += $"{key}\t - \t{cc.Count()}" + " | "; p12 += cc.Count();
                                        }
                                    }
                                    foundSummary1.Add(model);
                                    model = new CriticalIncidentSummary();
                                }
                                #endregion

                                #region 3rd Location:
                                if (ll3 != null)
                                {
                                    model.LocationName = locList.Find(i => i == "Barnswallow Place Care Community\r\n" + " - " + cnt3);
                                    var attr11 = ll3.GroupBy(i => i.MOHLTC_Follow_Up);
                                    if (attr11 != null)
                                    {
                                        foreach (var cc in attr11)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.MOHLTC_Follow_Up += $"{key}\t - \t{cc.Count()}" + " | "; p1 += cc.Count();
                                        }
                                    }

                                    var attr10 = ll3.GroupBy(i => i.CIS_Initiated);
                                    if (attr10 != null)
                                    {
                                        foreach (var cc in attr10)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.CIS_Initiated += $"{key}\t - \t{cc.Count()}" + " | "; p2 += cc.Count();
                                        }
                                    }

                                    var attr7 = ll3.GroupBy(i => i.MOH_Notified);
                                    if (attr7 != null)
                                    {
                                        foreach (var cc in attr7)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.MOH_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p3 += cc.Count();
                                        }
                                    }

                                    var attr2 = ll3.GroupBy(i => i.POAS_Notified);
                                    if (attr2 != null)
                                    {
                                        foreach (var d in attr2)
                                        {
                                            string key = d.Key == null ? "NULL" : d.Key;
                                            if (key == "NULL") continue;
                                            model.POAS_Notified += $"{key}\t - \t{d.Count()}" + " | "; p4 += d.Count();
                                        }
                                    }

                                    var attr4 = ll3.GroupBy(i => i.Police_Notified);
                                    if (attr4 != null)
                                    {
                                        foreach (var cc in attr4)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Police_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p5 += cc.Count();
                                        }
                                    }


                                    var attr3 = ll3.GroupBy(i => i.Quality_Improvement_Actions);
                                    if (attr3 != null)
                                    {
                                        foreach (var cc in attr3)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Quality_Improvement_Actions += $"{key}\t - \t{cc.Count()}" + " | "; p6 += cc.Count();
                                        }
                                    }

                                    var attr8 = ll3.GroupBy(i => i.Risk_Locked);
                                    if (attr8 != null)
                                    {
                                        foreach (var cc in attr8)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Risk_Locked += $"{key}\t - \t{cc.Count()}" + " | "; p7 += cc.Count();
                                        }
                                    }

                                    var attr5 = ll3.GroupBy(i => i.Brief_Description);
                                    if (attr5 != null)
                                    {
                                        foreach (var cc in attr5)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Brief_Description += $"{key}\t - \t{cc.Count()}" + " | "; p8 += cc.Count();
                                        }
                                    }

                                    var attr1 = ll3.GroupBy(i => i.Care_Plan_Updated);
                                    if (attr1 != null)
                                    {
                                        foreach (var e in attr1)
                                        {
                                            string key = e.Key == null ? "NULL" : e.Key;
                                            if (key == "NULL") continue;
                                            model.Care_Plan_Updated += $"{key}\t - \t{e.Count()}" + " | "; p9 += e.Count();
                                        }
                                    }

                                    var attr13 = ll3.GroupBy(i => i.CI_Form_Number);
                                    if (attr13 != null)
                                    {
                                        int count = 0;
                                        foreach (var cc in attr13)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            count += cc.Count();
                                        }
                                        model.CI_Form_Number = $"All\t - \t{count}"; p10 += count;
                                    }

                                    var attr00 = ll3.GroupBy(i => i.File_Complete);
                                    if (attr00 != null)
                                    {
                                        foreach (var cc in attr00)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.File_Complete += $"{key}\t - \t{cc.Count()}" + " | "; p11 += cc.Count();
                                        }
                                    }

                                    var attr111 = ll3.GroupBy(i => i.Follow_Up_Amendments);
                                    if (attr111 != null)
                                    {
                                        foreach (var cc in attr111)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Follow_Up_Amendments += $"{key}\t - \t{cc.Count()}" + " | "; p12 += cc.Count();
                                        }
                                    }
                                    foundSummary1.Add(model);
                                    model = new CriticalIncidentSummary();
                                }
                                #endregion

                                #region 4rd Location:
                                if (ll4 != null)
                                {
                                    model.LocationName = locList.Find(i => i == "Bearbrook Retirement Residence\r\n" + " - " + cnt4);
                                    var attr11 = ll4.GroupBy(i => i.MOHLTC_Follow_Up);
                                    if (attr11 != null)
                                    {
                                        foreach (var cc in attr11)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.MOHLTC_Follow_Up += $"{key}\t - \t{cc.Count()}" + " | "; p1 += cc.Count();
                                        }
                                    }

                                    var attr10 = ll4.GroupBy(i => i.CIS_Initiated);
                                    if (attr10 != null)
                                    {
                                        foreach (var cc in attr10)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.CIS_Initiated += $"{key}\t - \t{cc.Count()}" + " | "; p2 += cc.Count();
                                        }
                                    }

                                    var attr7 = ll4.GroupBy(i => i.MOH_Notified);
                                    if (attr7 != null)
                                    {
                                        foreach (var cc in attr7)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.MOH_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p3 += cc.Count();
                                        }
                                    }

                                    var attr2 = ll4.GroupBy(i => i.POAS_Notified);
                                    if (attr2 != null)
                                    {
                                        foreach (var d in attr2)
                                        {
                                            string key = d.Key == null ? "NULL" : d.Key;
                                            if (key == "NULL") continue;
                                            model.POAS_Notified += $"{key}\t - \t{d.Count()}" + " | "; p4 += d.Count();
                                        }
                                    }

                                    var attr4 = ll4.GroupBy(i => i.Police_Notified);
                                    if (attr4 != null)
                                    {
                                        foreach (var cc in attr4)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Police_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p5 += cc.Count();
                                        }
                                    }


                                    var attr3 = ll4.GroupBy(i => i.Quality_Improvement_Actions);
                                    if (attr3 != null)
                                    {
                                        foreach (var cc in attr3)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Quality_Improvement_Actions += $"{key}\t - \t{cc.Count()}" + " | "; p6 += cc.Count();
                                        }
                                    }

                                    var attr8 = ll4.GroupBy(i => i.Risk_Locked);
                                    if (attr8 != null)
                                    {
                                        foreach (var cc in attr8)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Risk_Locked += $"{key}\t - \t{cc.Count()}" + " | "; p7 += cc.Count();
                                        }
                                    }

                                    var attr5 = ll4.GroupBy(i => i.Brief_Description);
                                    if (attr5 != null)
                                    {
                                        foreach (var cc in attr5)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Brief_Description += $"{key}\t - \t{cc.Count()}" + " | "; p8 += cc.Count();
                                        }
                                    }

                                    var attr1 = ll4.GroupBy(i => i.Care_Plan_Updated);
                                    if (attr1 != null)
                                    {
                                        foreach (var e in attr1)
                                        {
                                            string key = e.Key == null ? "NULL" : e.Key;
                                            if (key == "NULL") continue;
                                            model.Care_Plan_Updated += $"{key}\t - \t{e.Count()}" + " | "; p9 += e.Count();
                                        }
                                    }

                                    var attr13 = ll4.GroupBy(i => i.CI_Form_Number);
                                    if (attr13 != null)
                                    {
                                        int count = 0;
                                        foreach (var cc in attr13)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            count += cc.Count();
                                        }
                                        model.CI_Form_Number = $"All\t - \t{count}"; p10 += count;
                                    }

                                    var attr00 = ll4.GroupBy(i => i.File_Complete);
                                    if (attr00 != null)
                                    {
                                        foreach (var cc in attr00)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.File_Complete += $"{key}\t - \t{cc.Count()}" + " | "; p11 += cc.Count();
                                        }
                                    }

                                    var attr111 = ll4.GroupBy(i => i.Follow_Up_Amendments);
                                    if (attr111 != null)
                                    {
                                        foreach (var cc in attr111)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Follow_Up_Amendments += $"{key}\t - \t{cc.Count()}" + " | "; p12 += cc.Count();
                                        }
                                    }
                                    foundSummary1.Add(model);
                                    model = new CriticalIncidentSummary();
                                }
                                #endregion

                                #region 5th Location:
                                if (ll5 != null)
                                {
                                    model.LocationName = locList.Find(i => i == "Bloomington Cove Care Community\r\n" + " - " + cnt5);
                                    var attr11 = ll5.GroupBy(i => i.MOHLTC_Follow_Up);
                                    if (attr11 != null)
                                    {
                                        foreach (var cc in attr11)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.MOHLTC_Follow_Up += $"{key}\t - \t{cc.Count()}" + " | "; p1 += cc.Count();
                                        }
                                    }

                                    var attr10 = ll5.GroupBy(i => i.CIS_Initiated);
                                    if (attr10 != null)
                                    {
                                        foreach (var cc in attr10)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.CIS_Initiated += $"{key}\t - \t{cc.Count()}" + " | "; p2 += cc.Count();
                                        }
                                    }

                                    var attr7 = ll5.GroupBy(i => i.MOH_Notified);
                                    if (attr7 != null)
                                    {
                                        foreach (var cc in attr7)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.MOH_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p3 += cc.Count();
                                        }
                                    }

                                    var attr2 = ll5.GroupBy(i => i.POAS_Notified);
                                    if (attr2 != null)
                                    {
                                        foreach (var d in attr2)
                                        {
                                            string key = d.Key == null ? "NULL" : d.Key;
                                            if (key == "NULL") continue;
                                            model.POAS_Notified += $"{key}\t - \t{d.Count()}" + " | "; p4 += d.Count();
                                        }
                                    }

                                    var attr4 = ll5.GroupBy(i => i.Police_Notified);
                                    if (attr4 != null)
                                    {
                                        foreach (var cc in attr4)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Police_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p5 += cc.Count();
                                        }
                                    }


                                    var attr3 = ll5.GroupBy(i => i.Quality_Improvement_Actions);
                                    if (attr3 != null)
                                    {
                                        foreach (var cc in attr3)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Quality_Improvement_Actions += $"{key}\t - \t{cc.Count()}" + " | "; p6 += cc.Count();
                                        }
                                    }

                                    var attr8 = ll5.GroupBy(i => i.Risk_Locked);
                                    if (attr8 != null)
                                    {
                                        foreach (var cc in attr8)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Risk_Locked += $"{key}\t - \t{cc.Count()}" + " | "; p7 += cc.Count();
                                        }
                                    }

                                    var attr5 = ll5.GroupBy(i => i.Brief_Description);
                                    if (attr5 != null)
                                    {
                                        foreach (var cc in attr5)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Brief_Description += $"{key}\t - \t{cc.Count()}" + " | "; p8 += cc.Count();
                                        }
                                    }

                                    var attr1 = ll5.GroupBy(i => i.Care_Plan_Updated);
                                    if (attr1 != null)
                                    {
                                        foreach (var e in attr1)
                                        {
                                            string key = e.Key == null ? "NULL" : e.Key;
                                            if (key == "NULL") continue;
                                            model.Care_Plan_Updated += $"{key}\t - \t{e.Count()}" + " | "; p9 += e.Count();
                                        }
                                    }

                                    var attr13 = ll5.GroupBy(i => i.CI_Form_Number);
                                    if (attr13 != null)
                                    {
                                        int count = 0;
                                        foreach (var cc in attr13)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            count += cc.Count();
                                        }
                                        model.CI_Form_Number = $"All\t - \t{count}"; p10 += count;
                                    }

                                    var attr00 = ll5.GroupBy(i => i.File_Complete);
                                    if (attr00 != null)
                                    {
                                        foreach (var cc in attr00)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.File_Complete += $"{key}\t - \t{cc.Count()}" + " | "; p11 += cc.Count();
                                        }
                                    }

                                    var attr111 = ll5.GroupBy(i => i.Follow_Up_Amendments);
                                    if (attr111 != null)
                                    {
                                        foreach (var cc in attr111)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Follow_Up_Amendments += $"{key}\t - \t{cc.Count()}" + " | "; p12 += cc.Count();
                                        }
                                    }
                                    foundSummary1.Add(model);
                                    model = new CriticalIncidentSummary();
                                }
                                #endregion

                                #region 6th Location:
                                if (ll6 != null)
                                {
                                    model.LocationName = locList.Find(i => i == "Bradford Valley Care Community\r\n" + " - " + cnt6);
                                    var attr11 = ll6.GroupBy(i => i.MOHLTC_Follow_Up);
                                    if (attr11 != null)
                                    {
                                        foreach (var cc in attr11)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.MOHLTC_Follow_Up += $"{key}\t - \t{cc.Count()}" + " | "; p1 += cc.Count();
                                        }
                                    }

                                    var attr10 = ll6.GroupBy(i => i.CIS_Initiated);
                                    if (attr10 != null)
                                    {
                                        foreach (var cc in attr10)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.CIS_Initiated += $"{key}\t - \t{cc.Count()}" + " | "; p2 += cc.Count();
                                        }
                                    }

                                    var attr7 = ll6.GroupBy(i => i.MOH_Notified);
                                    if (attr7 != null)
                                    {
                                        foreach (var cc in attr7)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.MOH_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p3 += cc.Count();
                                        }
                                    }

                                    var attr2 = ll6.GroupBy(i => i.POAS_Notified);
                                    if (attr2 != null)
                                    {
                                        foreach (var d in attr2)
                                        {
                                            string key = d.Key == null ? "NULL" : d.Key;
                                            if (key == "NULL") continue;
                                            model.POAS_Notified += $"{key}\t - \t{d.Count()}" + " | "; p4 += d.Count();
                                        }
                                    }

                                    var attr4 = ll6.GroupBy(i => i.Police_Notified);
                                    if (attr4 != null)
                                    {
                                        foreach (var cc in attr4)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Police_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p5 += cc.Count();
                                        }
                                    }


                                    var attr3 = ll6.GroupBy(i => i.Quality_Improvement_Actions);
                                    if (attr3 != null)
                                    {
                                        foreach (var cc in attr3)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Quality_Improvement_Actions += $"{key}\t - \t{cc.Count()}" + " | "; p6 += cc.Count();
                                        }
                                    }

                                    var attr8 = ll6.GroupBy(i => i.Risk_Locked);
                                    if (attr8 != null)
                                    {
                                        foreach (var cc in attr8)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Risk_Locked += $"{key}\t - \t{cc.Count()}" + " | "; p7 += cc.Count();
                                        }
                                    }

                                    var attr5 = ll6.GroupBy(i => i.Brief_Description);
                                    if (attr5 != null)
                                    {
                                        foreach (var cc in attr5)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Brief_Description += $"{key}\t - \t{cc.Count()}" + " | "; p8 += cc.Count();
                                        }
                                    }

                                    var attr1 = ll6.GroupBy(i => i.Care_Plan_Updated);
                                    if (attr1 != null)
                                    {
                                        foreach (var e in attr1)
                                        {
                                            string key = e.Key == null ? "NULL" : e.Key;
                                            if (key == "NULL") continue;
                                            model.Care_Plan_Updated += $"{key}\t - \t{e.Count()}" + " | "; p9 += e.Count();
                                        }
                                    }

                                    var attr13 = ll6.GroupBy(i => i.CI_Form_Number);
                                    if (attr13 != null)
                                    {
                                        int count = 0;
                                        foreach (var cc in attr13)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            count += cc.Count();
                                        }
                                        model.CI_Form_Number = $"All\t - \t{count}"; p10 += count;
                                    }

                                    var attr00 = ll6.GroupBy(i => i.File_Complete);
                                    if (attr00 != null)
                                    {
                                        foreach (var cc in attr00)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.File_Complete += $"{key}\t - \t{cc.Count()}" + " | "; p11 += cc.Count();
                                        }
                                    }

                                    var attr111 = ll6.GroupBy(i => i.Follow_Up_Amendments);
                                    if (attr111 != null)
                                    {
                                        foreach (var cc in attr111)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Follow_Up_Amendments += $"{key}\t - \t{cc.Count()}" + " | "; p12 += cc.Count();
                                        }
                                    }
                                    foundSummary1.Add(model);
                                    model = new CriticalIncidentSummary();
                                }
                                #endregion

                                #region 7th Location:
                                if (ll7 != null)
                                {
                                    model.LocationName = locList.Find(i => i == "Brookside Lodge\r\n" + " - " + cnt7);
                                    var attr11 = ll7.GroupBy(i => i.MOHLTC_Follow_Up);
                                    if (attr11 != null)
                                    {
                                        foreach (var cc in attr11)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.MOHLTC_Follow_Up += $"{key}\t - \t{cc.Count()}" + " | "; p1 += cc.Count();
                                        }
                                    }

                                    var attr10 = ll7.GroupBy(i => i.CIS_Initiated);
                                    if (attr10 != null)
                                    {
                                        foreach (var cc in attr10)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.CIS_Initiated += $"{key}\t - \t{cc.Count()}" + " | "; p2 += cc.Count();
                                        }
                                    }

                                    var attr7 = ll7.GroupBy(i => i.MOH_Notified);
                                    if (attr7 != null)
                                    {
                                        foreach (var cc in attr7)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.MOH_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p3 += cc.Count();
                                        }
                                    }

                                    var attr2 = ll7.GroupBy(i => i.POAS_Notified);
                                    if (attr2 != null)
                                    {
                                        foreach (var d in attr2)
                                        {
                                            string key = d.Key == null ? "NULL" : d.Key;
                                            if (key == "NULL") continue;
                                            model.POAS_Notified += $"{key}\t - \t{d.Count()}" + " | "; p4 += d.Count();
                                        }
                                    }

                                    var attr4 = ll7.GroupBy(i => i.Police_Notified);
                                    if (attr4 != null)
                                    {
                                        foreach (var cc in attr4)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Police_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p5 += cc.Count();
                                        }
                                    }


                                    var attr3 = ll7.GroupBy(i => i.Quality_Improvement_Actions);
                                    if (attr3 != null)
                                    {
                                        foreach (var cc in attr3)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Quality_Improvement_Actions += $"{key}\t - \t{cc.Count()}" + " | "; p6 += cc.Count();
                                        }
                                    }

                                    var attr8 = ll7.GroupBy(i => i.Risk_Locked);
                                    if (attr8 != null)
                                    {
                                        foreach (var cc in attr8)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Risk_Locked += $"{key}\t - \t{cc.Count()}" + " | "; p7 += cc.Count();
                                        }
                                    }

                                    var attr5 = ll7.GroupBy(i => i.Brief_Description);
                                    if (attr5 != null)
                                    {
                                        foreach (var cc in attr5)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Brief_Description += $"{key}\t - \t{cc.Count()}" + " | "; p8 += cc.Count();
                                        }
                                    }

                                    var attr1 = ll7.GroupBy(i => i.Care_Plan_Updated);
                                    if (attr1 != null)
                                    {
                                        foreach (var e in attr1)
                                        {
                                            string key = e.Key == null ? "NULL" : e.Key;
                                            if (key == "NULL") continue;
                                            model.Care_Plan_Updated += $"{key}\t - \t{e.Count()}" + " | "; p9 += e.Count();
                                        }
                                    }

                                    var attr13 = ll7.GroupBy(i => i.CI_Form_Number);
                                    if (attr13 != null)
                                    {
                                        int count = 0;
                                        foreach (var cc in attr13)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            count += cc.Count();
                                        }
                                        model.CI_Form_Number = $"All\t - \t{count}"; p10 += count;
                                    }

                                    var attr00 = ll7.GroupBy(i => i.File_Complete);
                                    if (attr00 != null)
                                    {
                                        foreach (var cc in attr00)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.File_Complete += $"{key}\t - \t{cc.Count()}" + " | "; p11 += cc.Count();
                                        }
                                    }

                                    var attr111 = ll7.GroupBy(i => i.Follow_Up_Amendments);
                                    if (attr111 != null)
                                    {
                                        foreach (var cc in attr111)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Follow_Up_Amendments += $"{key}\t - \t{cc.Count()}" + " | "; p12 += cc.Count();
                                        }
                                    }
                                    foundSummary1.Add(model);
                                    model = new CriticalIncidentSummary();
                                }
                                #endregion

                                #region 8th Location:
                                if (ll8 != null)
                                {
                                    int c = 0;
                                    model.LocationName = locList.Find(i => i == "Woodbridge Vista" + " - " + cnt8);
                                    var attr11 = ll8.GroupBy(i => i.MOHLTC_Follow_Up);
                                    if (attr11 != null)
                                    {

                                        foreach (var cc in attr11)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            if (c > 1)
                                            {
                                                model.MOHLTC_Follow_Up += $"{key}\t - \t{cc.Count()}" + " | "; p1 += cc.Count();
                                            }
                                            else { model.MOHLTC_Follow_Up += $"{key}\t - \t{cc.Count()}"; p1 += cc.Count(); }
                                            c++;
                                        }
                                    }
                                    c = 0;
                                    var attr10 = ll8.GroupBy(i => i.CIS_Initiated);
                                    if (attr10 != null)
                                    {
                                        foreach (var cc in attr10)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            if (c > 1)
                                            {
                                                model.CIS_Initiated += $"{key}\t - \t{cc.Count()}" + " | "; p2 += cc.Count();
                                            }
                                            else { model.CIS_Initiated += $"{key}\t - \t{cc.Count()}"; p2 += cc.Count(); }
                                            c++;
                                        }
                                    }
                                    c = 0;
                                    var attr7 = ll8.GroupBy(i => i.MOH_Notified);
                                    if (attr7 != null)
                                    {
                                        foreach (var cc in attr7)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            if (c > 1)
                                            {
                                                model.MOH_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p3 += cc.Count();
                                            }
                                            else { model.MOH_Notified += $"{key}\t - \t{cc.Count()}"; p3 += cc.Count(); }
                                            c++;
                                        }
                                    }
                                    c = 0;
                                    var attr2 = ll8.GroupBy(i => i.POAS_Notified);
                                    if (attr2 != null)
                                    {
                                        foreach (var d in attr2)
                                        {
                                            string key = d.Key == null ? "NULL" : d.Key;
                                            if (key == "NULL") continue;
                                            if (c > 1)
                                            {
                                                model.POAS_Notified += $"{key}\t - \t{d.Count()}" + " | ";
                                                p4 += d.Count();
                                            }
                                            else { model.POAS_Notified += $"{key}\t - \t{d.Count()}"; p4 += d.Count(); }
                                            c++;
                                        }
                                    }
                                    c = 0;
                                    var attr4 = ll8.GroupBy(i => i.Police_Notified);
                                    if (attr4 != null)
                                    {
                                        foreach (var cc in attr4)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            if (c > 1)
                                            {
                                                model.Police_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p5 += cc.Count();
                                            }
                                            else { model.Police_Notified += $"{key}\t - \t{cc.Count()}"; p5 += cc.Count(); }
                                            c++;
                                        }
                                    }
                                    c = 0;
                                    var attr3 = ll8.GroupBy(i => i.Quality_Improvement_Actions);
                                    if (attr3 != null)
                                    {
                                        foreach (var cc in attr3)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            if (c > 1)
                                            {
                                                model.Quality_Improvement_Actions += $"{key}\t - \t{cc.Count()}" + " | ";
                                                p6 += cc.Count();
                                            }
                                            else { model.Quality_Improvement_Actions += $"{key}\t - \t{cc.Count()}"; p6 += cc.Count(); }
                                            c++;
                                        }
                                    }
                                    c = 0;
                                    var attr8 = ll8.GroupBy(i => i.Risk_Locked);
                                    if (attr8 != null)
                                    {
                                        foreach (var cc in attr8)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            if (c > 1)
                                            {
                                                model.Risk_Locked += $"{key}\t - \t{cc.Count()}" + " | "; p7 += cc.Count();
                                            }
                                            else { model.Risk_Locked += $"{key}\t - \t{cc.Count()}"; p7 += cc.Count(); }
                                            c++;
                                        }
                                    }
                                    c = 0;
                                    var attr5 = ll8.GroupBy(i => i.Brief_Description);
                                    if (attr5 != null)
                                    {
                                        foreach (var cc in attr5)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            if (c > 1)
                                            {
                                                model.Brief_Description += $"{key}\t - \t{cc.Count()}" + " | "; p8 += cc.Count();
                                            }
                                            else { model.Brief_Description += $"{key}\t - \t{cc.Count()}"; p8 += cc.Count(); }
                                            c++;
                                        }
                                    }
                                    c = 0;
                                    var attr1 = ll8.GroupBy(i => i.Care_Plan_Updated);
                                    if (attr1 != null)
                                    {
                                        foreach (var e in attr1)
                                        {
                                            string key = e.Key == null ? "NULL" : e.Key;
                                            if (key == "NULL") continue;
                                            if (c > 1)
                                            {
                                                model.Care_Plan_Updated += $"{key}\t - \t{e.Count()}" + " | "; p9 += e.Count();
                                            }
                                            else { model.Care_Plan_Updated += $"{key}\t - \t{e.Count()}"; p9 += e.Count(); }
                                            c++;
                                        }
                                    }
                                    c = 0;
                                    var attr13 = ll8.GroupBy(i => i.CI_Form_Number);
                                    if (attr13 != null)
                                    {
                                        int count = 0;
                                        foreach (var cc in attr13)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            count += cc.Count();
                                        }
                                        if (c > 1)
                                        {
                                            model.CI_Form_Number += $"{count}" + " | "; p10 += count;
                                        }
                                        else { model.CI_Form_Number += $"{count}"; p10 += count; }
                                        c++;
                                    }
                                    c = 0;
                                    var attr00 = ll8.GroupBy(i => i.File_Complete);
                                    if (attr00 != null)
                                    {
                                        foreach (var cc in attr00)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            if (c > 1)
                                            {
                                                model.File_Complete += $"{key}\t - \t{cc.Count()}" + " | "; p11 += cc.Count();
                                            }
                                            else { model.File_Complete += $"{key}\t - \t{cc.Count()}"; p11 += cc.Count(); }
                                            c++;
                                        }
                                    }
                                    c = 0;
                                    var attr111 = ll8.GroupBy(i => i.Follow_Up_Amendments);
                                    if (attr111 != null)
                                    {
                                        foreach (var cc in attr111)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            if (c > 1)
                                            {
                                                model.Follow_Up_Amendments += $"{key}\t - \t{cc.Count()}" + " | "; p12 += cc.Count();
                                            }
                                            else { model.Follow_Up_Amendments += $"{key}\t - \t{cc.Count()}"; p12 += cc.Count(); }
                                            c++;
                                        }
                                    }
                                    c = 0;
                                    foundSummary1.Add(model);
                                    model = new CriticalIncidentSummary();
                                }
                                #endregion

                                #region 9th Location:
                                if (ll9 != null)
                                {
                                    model.LocationName = locList.Find(i => i == "Brookside Lodge\r\n" + " - " + cnt9);
                                    var attr11 = ll9.GroupBy(i => i.MOHLTC_Follow_Up);
                                    if (attr11 != null)
                                    {
                                        foreach (var cc in attr11)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.MOHLTC_Follow_Up += $"{key}\t - \t{cc.Count()}" + " | "; p1 += cc.Count();
                                        }
                                    }

                                    var attr10 = ll9.GroupBy(i => i.CIS_Initiated);
                                    if (attr10 != null)
                                    {
                                        foreach (var cc in attr10)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.CIS_Initiated += $"{key}\t - \t{cc.Count()}" + " | "; p2 += cc.Count();
                                        }
                                    }

                                    var attr7 = ll9.GroupBy(i => i.MOH_Notified);
                                    if (attr7 != null)
                                    {
                                        foreach (var cc in attr7)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.MOH_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p3 += cc.Count();
                                        }
                                    }

                                    var attr2 = ll9.GroupBy(i => i.POAS_Notified);
                                    if (attr2 != null)
                                    {
                                        foreach (var d in attr2)
                                        {
                                            string key = d.Key == null ? "NULL" : d.Key;
                                            if (key == "NULL") continue;
                                            model.POAS_Notified += $"{key}\t - \t{d.Count()}" + " | "; p4 += d.Count();
                                        }
                                    }

                                    var attr4 = ll9.GroupBy(i => i.Police_Notified);
                                    if (attr4 != null)
                                    {
                                        foreach (var cc in attr4)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Police_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p5 += cc.Count();
                                        }
                                    }


                                    var attr3 = ll9.GroupBy(i => i.Quality_Improvement_Actions);
                                    if (attr3 != null)
                                    {
                                        foreach (var cc in attr3)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Quality_Improvement_Actions += $"{key}\t - \t{cc.Count()}" + " | "; p6 += cc.Count();
                                        }
                                    }

                                    var attr8 = ll9.GroupBy(i => i.Risk_Locked);
                                    if (attr8 != null)
                                    {
                                        foreach (var cc in attr8)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Risk_Locked += $"{key}\t - \t{cc.Count()}" + " | "; p7 += cc.Count();
                                        }
                                    }

                                    var attr5 = ll9.GroupBy(i => i.Brief_Description);
                                    if (attr5 != null)
                                    {
                                        foreach (var cc in attr5)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Brief_Description += $"{key}\t - \t{cc.Count()}" + " | "; p8 += cc.Count();
                                        }
                                    }

                                    var attr1 = ll9.GroupBy(i => i.Care_Plan_Updated);
                                    if (attr1 != null)
                                    {
                                        foreach (var e in attr1)
                                        {
                                            string key = e.Key == null ? "NULL" : e.Key;
                                            if (key == "NULL") continue;
                                            model.Care_Plan_Updated += $"{key}\t - \t{e.Count()}" + " | "; p9 += e.Count();
                                        }
                                    }

                                    var attr13 = ll9.GroupBy(i => i.CI_Form_Number);
                                    if (attr13 != null)
                                    {
                                        int count = 0;
                                        foreach (var cc in attr13)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            count += cc.Count();
                                        }
                                        model.CI_Form_Number = $"All\t - \t{count}"; p10 += count;
                                    }

                                    var attr00 = ll9.GroupBy(i => i.File_Complete);
                                    if (attr00 != null)
                                    {
                                        foreach (var cc in attr00)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.File_Complete += $"{key}\t - \t{cc.Count()}" + " | "; p11 += cc.Count();
                                        }
                                    }

                                    var attr111 = ll9.GroupBy(i => i.Follow_Up_Amendments);
                                    if (attr111 != null)
                                    {
                                        foreach (var cc in attr111)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Follow_Up_Amendments += $"{key}\t - \t{cc.Count()}" + " | "; p12 += cc.Count();
                                        }
                                    }
                                    foundSummary1.Add(model);
                                    model = new CriticalIncidentSummary();
                                }
                                #endregion

                                #region 10th Location:
                                if (ll10 != null)
                                {
                                    model.LocationName = locList.Find(i => i == "Rideau\r\n" + " - " + cnt10);
                                    var attr11 = ll10.GroupBy(i => i.MOHLTC_Follow_Up);
                                    if (attr11 != null)
                                    {
                                        foreach (var cc in attr11)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.MOHLTC_Follow_Up += $"{key}\t - \t{cc.Count()}" + " | "; p1 += cc.Count();
                                        }
                                    }

                                    var attr10 = ll10.GroupBy(i => i.CIS_Initiated);
                                    if (attr10 != null)
                                    {
                                        foreach (var cc in attr10)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.CIS_Initiated += $"{key}\t - \t{cc.Count()}" + " | "; p2 += cc.Count();
                                        }
                                    }

                                    var attr7 = ll10.GroupBy(i => i.MOH_Notified);
                                    if (attr7 != null)
                                    {
                                        foreach (var cc in attr7)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.MOH_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p3 += cc.Count();
                                        }
                                    }

                                    var attr2 = ll10.GroupBy(i => i.POAS_Notified);
                                    if (attr2 != null)
                                    {
                                        foreach (var d in attr2)
                                        {
                                            string key = d.Key == null ? "NULL" : d.Key;
                                            if (key == "NULL") continue;
                                            model.POAS_Notified += $"{key}\t - \t{d.Count()}" + " | "; p4 += d.Count();
                                        }
                                    }

                                    var attr4 = ll10.GroupBy(i => i.Police_Notified);
                                    if (attr4 != null)
                                    {
                                        foreach (var cc in attr4)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Police_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p5 += cc.Count();
                                        }
                                    }


                                    var attr3 = ll10.GroupBy(i => i.Quality_Improvement_Actions);
                                    if (attr3 != null)
                                    {
                                        foreach (var cc in attr3)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Quality_Improvement_Actions += $"{key}\t - \t{cc.Count()}" + " | "; p6 += cc.Count();
                                        }
                                    }

                                    var attr8 = ll10.GroupBy(i => i.Risk_Locked);
                                    if (attr8 != null)
                                    {
                                        foreach (var cc in attr8)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Risk_Locked += $"{key}\t - \t{cc.Count()}" + " | "; p7 += cc.Count();
                                        }
                                    }

                                    var attr5 = ll10.GroupBy(i => i.Brief_Description);
                                    if (attr5 != null)
                                    {
                                        foreach (var cc in attr5)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Brief_Description += $"{key}\t - \t{cc.Count()}" + " | "; p8 += cc.Count();
                                        }
                                    }

                                    var attr1 = ll10.GroupBy(i => i.Care_Plan_Updated);
                                    if (attr1 != null)
                                    {
                                        foreach (var e in attr1)
                                        {
                                            string key = e.Key == null ? "NULL" : e.Key;
                                            if (key == "NULL") continue;
                                            model.Care_Plan_Updated += $"{key}\t - \t{e.Count()}" + " | "; p9 += e.Count();
                                        }
                                    }

                                    var attr13 = ll10.GroupBy(i => i.CI_Form_Number);
                                    if (attr13 != null)
                                    {
                                        int count = 0;
                                        foreach (var cc in attr13)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            count += cc.Count();
                                        }
                                        model.CI_Form_Number = $"All\t - \t{count}"; p10 += count;
                                    }

                                    var attr00 = ll10.GroupBy(i => i.File_Complete);
                                    if (attr00 != null)
                                    {
                                        foreach (var cc in attr00)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.File_Complete += $"{key}\t - \t{cc.Count()}" + " | "; p11 += cc.Count();
                                        }
                                    }

                                    var attr111 = ll10.GroupBy(i => i.Follow_Up_Amendments);
                                    if (attr111 != null)
                                    {
                                        foreach (var cc in attr111)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Follow_Up_Amendments += $"{key}\t - \t{cc.Count()}" + " | "; p12 += cc.Count();
                                        }
                                    }
                                    foundSummary1.Add(model);
                                    model = new CriticalIncidentSummary();
                                }
                                #endregion

                                #region 11th Location:
                                if (ll11 != null)
                                {
                                    model.LocationName = locList.Find(i => i == "Villa da Vinci\r\n" + " - " + cnt11);
                                    var attr11 = ll11.GroupBy(i => i.MOHLTC_Follow_Up);
                                    if (attr11 != null)
                                    {
                                        foreach (var cc in attr11)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.MOHLTC_Follow_Up += $"{key}\t - \t{cc.Count()}" + " | "; p1 += cc.Count();
                                        }
                                    }

                                    var attr10 = ll11.GroupBy(i => i.CIS_Initiated);
                                    if (attr10 != null)
                                    {
                                        foreach (var cc in attr10)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.CIS_Initiated += $"{key}\t - \t{cc.Count()}" + " | "; p2 += cc.Count();
                                        }
                                    }

                                    var attr7 = ll11.GroupBy(i => i.MOH_Notified);
                                    if (attr7 != null)
                                    {
                                        foreach (var cc in attr7)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.MOH_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p3 += cc.Count();
                                        }
                                    }

                                    var attr2 = ll11.GroupBy(i => i.POAS_Notified);
                                    if (attr2 != null)
                                    {
                                        foreach (var d in attr2)
                                        {
                                            string key = d.Key == null ? "NULL" : d.Key;
                                            if (key == "NULL") continue;
                                            model.POAS_Notified += $"{key}\t - \t{d.Count()}" + " | "; p4 += d.Count();
                                        }
                                    }

                                    var attr4 = ll11.GroupBy(i => i.Police_Notified);
                                    if (attr4 != null)
                                    {
                                        foreach (var cc in attr4)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Police_Notified += $"{key}\t - \t{cc.Count()}" + " | "; p5 += cc.Count();
                                        }
                                    }


                                    var attr3 = ll11.GroupBy(i => i.Quality_Improvement_Actions);
                                    if (attr3 != null)
                                    {
                                        foreach (var cc in attr3)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Quality_Improvement_Actions += $"{key}\t - \t{cc.Count()}" + " | "; p6 += cc.Count();
                                        }
                                    }

                                    var attr8 = ll11.GroupBy(i => i.Risk_Locked);
                                    if (attr8 != null)
                                    {
                                        foreach (var cc in attr8)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Risk_Locked += $"{key}\t - \t{cc.Count()}" + " | "; p7 += cc.Count();
                                        }
                                    }

                                    var attr5 = ll11.GroupBy(i => i.Brief_Description);
                                    if (attr5 != null)
                                    {
                                        foreach (var cc in attr5)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Brief_Description += $"{key}\t - \t{cc.Count()}" + " | "; p8 += cc.Count();
                                        }
                                    }

                                    var attr1 = ll11.GroupBy(i => i.Care_Plan_Updated);
                                    if (attr1 != null)
                                    {
                                        foreach (var e in attr1)
                                        {
                                            string key = e.Key == null ? "NULL" : e.Key;
                                            if (key == "NULL") continue;
                                            model.Care_Plan_Updated += $"{key}\t - \t{e.Count()}" + " | "; p9 += e.Count();
                                        }
                                    }

                                    var attr13 = ll11.GroupBy(i => i.CI_Form_Number);
                                    if (attr13 != null)
                                    {
                                        int count = 0;
                                        foreach (var cc in attr13)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            count += cc.Count();
                                        }
                                        model.CI_Form_Number = $"All\t - \t{count}"; p10 += count;
                                    }

                                    var attr00 = ll11.GroupBy(i => i.File_Complete);
                                    if (attr00 != null)
                                    {
                                        foreach (var cc in attr00)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.File_Complete += $"{key}\t - \t{cc.Count()}" + " | "; p11 += cc.Count();
                                        }
                                    }

                                    var attr111 = ll11.GroupBy(i => i.Follow_Up_Amendments);
                                    if (attr111 != null)
                                    {
                                        foreach (var cc in attr111)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key;
                                            if (key == "NULL") continue;
                                            model.Follow_Up_Amendments += $"{key}\t - \t{cc.Count()}" + " | "; p12 += cc.Count();
                                        }
                                    }
                                    foundSummary1.Add(model);
                                    model = new CriticalIncidentSummary();
                                }
                                #endregion

                                #region Add All Summary quantity on List:
                                allSummary1.Add(new IncidentSummaryAll
                                {
                                    MOHLTC_Follow_Up = p1,
                                    CIS_Initiated = p2,
                                    MOH_Notified = p3,
                                    POAS_Notified = p4,
                                    Police_Notified = p5,
                                    Quality_Improvement_Actions = p6,
                                    Risk_Locked = p7,
                                    Brief_Description = p8,
                                    Care_Plan_Updated = p9,
                                    CI_Form_Number = p10,
                                    File_Complete = p11,
                                    Follow_Up_Amendments = p12
                                });
                                #endregion
                            }

                            #region Create ViewBAgs:
                            { ViewBag.TotalSummary = allSummary1; }

                            b = true;
                            if (foundSummary1.Count == 0) { b = false; ViewBag.EmptLocation = b; }

                            {
                                ViewBag.Count = TablesContainer.COUNT;
                            }

                            {
                                ViewBag.GN_Found = foundSummary1;
                            }

                            {
                                ViewBag.Entity = "Critical_Incidents";
                            }

                            if (locList.Count != 0) isEmpty = true;

                            { ViewBag.Check1 = isEmpty; }

                            {
                                ViewBag.Locations = locList;
                            }

                            {
                                if (role == Role.Admin)
                                 ViewBag.LocInfo = db.Care_Communities.Find(1).Name;
                                else ViewBag.LocInfo = db.Care_Communities.Find(Id_Location).Name;
                            }
                            #endregion
                            break;
                        #endregion

                        #region Complaints:
                        case "Complaint":
                            if (role == Role.Admin)
                                TablesContainer.list2 = db.Complaints.ToList();
                            else if (role == Role.User)
                                TablesContainer.list2 = db.Complaints.Where(i => i.Location == Id_Location).ToList();
                            ClearAllStatic();

                            if (!checkRepead)
                            {
                                checkRepead = true;

                                #region Accounting all location name:
                                foreach (var cc in TablesContainer.list1)
                                {
                                    if (STREAM.GetLocNameById(cc.Location).Contains("Altamont Care Community"))
                                        cnt1++;
                                    else if (STREAM.GetLocNameById(cc.Location).Contains("Astoria Retirement Residence"))
                                        cnt2++;
                                    else if (STREAM.GetLocNameById(cc.Location).Contains("Barnswallow Place Care Community"))
                                        cnt3++;
                                    else if (STREAM.GetLocNameById(cc.Location).Contains("Bearbrook Retirement Residence"))
                                        cnt4++;
                                    else if (STREAM.GetLocNameById(cc.Location).Contains("Bloomington Cove Care Community"))
                                        cnt5++;
                                    else if (STREAM.GetLocNameById(cc.Location).Contains("Bradford Valley Care Community"))
                                        cnt6++;
                                    else if (STREAM.GetLocNameById(cc.Location).Contains("Brookside Lodge"))
                                        cnt7++;
                                    else if (STREAM.GetLocNameById(cc.Location).Contains("Woodbridge Vista"))
                                        cnt8++;
                                    else if (STREAM.GetLocNameById(cc.Location).Contains("Norfinch"))
                                        cnt9++;
                                    else if (STREAM.GetLocNameById(cc.Location).Contains("Rideau"))
                                        cnt10++;
                                    else if (STREAM.GetLocNameById(cc.Location).Contains("Villa da Vinci"))
                                        cnt11++;
                                }
                                #endregion

                                ComplaintsSummary model = new ComplaintsSummary();
                                var locDistinct = new HashSet<string>();
                                var locId = new List<int>();
                                foreach (var it in TablesContainer.list1)
                                {
                                    Care_Community cc = db.Care_Communities.Find(it.Location);
                                    locDistinct.Add(cc.Name);
                                    locId.Add(cc.Id);
                                }

                                locList = locDistinct.ToList();

                                #region Fill out lists ll1,ll2,ll3...ll11 existing locations:
                                for (var i = 0; i < locList.Count; i++)
                                {
                                    if (locList[i].Contains("Altamont Care Community"))
                                    {
                                        ll1 = TablesContainer.list1.Where
                                     (loc => STREAM.GetLocNameById(loc.Location) == "Altamont Care Community\r\n").ToList();
                                    }
                                    else if (locList[i].Contains("Astoria Retirement Residence"))
                                    {
                                        ll2 = TablesContainer.list1.Where
                                           (loc => STREAM.GetLocNameById(loc.Location) == "Astoria Retirement Residence\r\n").ToList();
                                    }
                                    else if (locList[i].Contains("Barnswallow Place Care Community"))
                                    {
                                        ll3 = TablesContainer.list1.Where
                                      (loc => STREAM.GetLocNameById(loc.Location) == "Barnswallow Place Care Community\r\n").ToList();
                                    }
                                    else if (locList[i].Contains("Bearbrook Retirement Residence"))
                                    {
                                        ll4 = TablesContainer.list1.Where
                                      (loc => STREAM.GetLocNameById(loc.Location) == "Bearbrook Retirement Residence\r\n").ToList();
                                    }
                                    else if (locList[i].Contains("Bloomington Cove Care Community"))
                                    {
                                        ll5 = TablesContainer.list1.Where
                                       (loc => STREAM.GetLocNameById(loc.Location) == "Bloomington Cove Care Community\r\n").ToList();
                                    }
                                    else if (locList[i].Contains("Bradford Valley Care Community"))
                                    {
                                        ll6 = TablesContainer.list1.Where
                                        (loc => STREAM.GetLocNameById(loc.Location) == "Bradford Valley Care Community\r\n").ToList();
                                    }
                                    else if (locList[i].Contains("Brookside Lodge"))
                                    {
                                        ll7 = TablesContainer.list1.Where
                                       (loc => STREAM.GetLocNameById(loc.Location) == "Brookside Lodge\r\n").ToList();
                                    }
                                    else if (locList[i].Contains("Woodbridge Vista"))
                                    {
                                        string retName = STREAM.GetLocNameById(8);
                                        ll8 = TablesContainer.list1.Where
                                        (loc => STREAM.GetLocNameById(loc.Location) == "Woodbridge Vista").ToList();
                                    }
                                    else if (locList[i].Contains("Norfinch"))
                                    {
                                        ll9 = TablesContainer.list1.Where
                                        (loc => STREAM.GetLocNameById(loc.Location) == "Norfinch\r\n").ToList();
                                    }
                                    else if (locList[i].Contains("Rideau"))
                                    {
                                        ll10 = TablesContainer.list1.Where
                                      (loc => STREAM.GetLocNameById(loc.Location) == "Rideau\r\n").ToList();
                                    }
                                    else if (locList[i].Contains("Villa da Vinci"))
                                    {
                                        ll11 = TablesContainer.list1.Where
                                        (loc => STREAM.GetLocNameById(loc.Location) == "Villa da Vinci\r\n").ToList();
                                    }
                                }
                                #endregion

                                #region Add count location for each exist:
                                for (var i = 0; i < locList.Count; i++)
                                {
                                    if (locList[i].Contains("Altamont Care Community"))
                                        locList[i] = locList[i] + " - " + cnt1;
                                    else if (locList[i].Contains("Astoria Retirement Residence"))
                                        locList[i] = locList[i] + " - " + cnt2;
                                    else if (locList[i].Contains("Barnswallow Place Care Community"))
                                        locList[i] = locList[i] + " - " + cnt3;
                                    else if (locList[i].Contains("Bearbrook Retirement Residence"))
                                        locList[i] = locList[i] + " - " + cnt4;
                                    else if (locList[i].Contains("Bloomington Cove Care Community"))
                                        locList[i] = locList[i] + " - " + cnt5;
                                    else if (locList[i].Contains("Bradford Valley Care Community"))
                                        locList[i] = locList[i] + " - " + cnt6;
                                    else if (locList[i].Contains("Brookside Lodge"))
                                        locList[i] = locList[i] + " - " + cnt7;
                                    else if (locList[i].Contains("Woodbridge Vista"))
                                        locList[i] = locList[i] + " - " + cnt8;
                                    else if (locList[i].Contains("Norfinch"))
                                        locList[i] = locList[i] + " - " + cnt9;
                                    else if (locList[i].Contains("Rideau"))
                                        locList[i] = locList[i] + " - " + cnt10;
                                    else if (locList[i].Contains("Villa da Vinci"))
                                        locList[i] = locList[i] + " - " + cnt11;
                                }
                                #endregion

                                locList.Sort(); // Sorted by alphanumeric

                                if (TablesContainer.list2.Count() == 0)
                                {
                                    { ViewBag.ObjName = "Complaint"; }
                                    ViewBag.ErrorMsg = errorMsg = "Please select a date range.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                                TablesContainer.COUNT = TablesContainer.list2.Count;

                                #region Count all atributes to location:
                                #region For the 1st Location:
                                if(ll1 != null)
                                {
                                    model.LocationName = locList.Find(i => i == "Altamont Care Community\r\n" + " - " + cnt1);
                                    var att1 = TablesContainer.list2.GroupBy(i => i.DateReceived);
                                    if (att1 != null)
                                    {              
                                        foreach (var cc in att1)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.DateReceived += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p1 += cc.Count();
                                        }
                                    }

                                    var att2 = TablesContainer.list2.GroupBy(i => i.WritenOrVerbal);
                                    if (att2 != null)
                                    {
                                        foreach (var cc in att2)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.WritenOrVerbal += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p2 += cc.Count();
                                        }
                                    }

                                    var att3 = TablesContainer.list2.GroupBy(i => i.Receive_Directly);
                                    if (att3 != null)
                                    {
                                        foreach (var cc in att3)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.Receive_Directly += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p3 += cc.Count();
                                        }
                                    }

                                    var att4 = TablesContainer.list2.GroupBy(i => i.FromResident);
                                    if (att4 != null)
                                    {
                                        foreach (var cc in att4)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.FromResident += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p4 += cc.Count();
                                        }
                                    }

                                    var att5 = TablesContainer.list2.GroupBy(i => i.ResidentName);
                                    if (att5 != null)
                                    {
                                        foreach (var cc in att5)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.ResidentName += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p5 += cc.Count();
                                        }
                                    }

                                    var att6 = TablesContainer.list2.GroupBy(i => i.Department);
                                    if (att6 != null)
                                    {
                                        foreach (var cc in att6)
                                        {
                                            model.Department += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p6 += cc.Count();
                                        }
                                    }

                                    var att7 = TablesContainer.list2.GroupBy(i => i.BriefDescription);
                                    if (att7 != null)
                                    {
                                        foreach (var cc in att7)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.BriefDescription += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p7 += cc.Count();
                                        }
                                    }

                                    var att8 = TablesContainer.list2.GroupBy(i => i.IsAdministration);
                                    if (att8 != null)
                                    {
                                        foreach (var cc in att8)
                                        {
                                            model.BriefDescription += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p8 += cc.Count();
                                        }
                                    }

                                    var att9 = TablesContainer.list2.GroupBy(i => i.CareServices);
                                    if (att9 != null)
                                    {
                                        foreach (var cc in att9)
                                        {
                                            model.CareServices += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p9 += cc.Count();
                                        }
                                    }

                                    var att10 = TablesContainer.list2.GroupBy(i => i.PalliativeCare);
                                    if (att10 != null)
                                    {
                                        foreach (var cc in att10)
                                        {
                                            model.PalliativeCare += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p10 += cc.Count();
                                        }
                                    }

                                    var att11 = TablesContainer.list2.GroupBy(i => i.Dietary);
                                    if (att11 != null)
                                    {
                                        foreach (var cc in att11)
                                        {
                                            model.Dietary += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p11 += cc.Count();
                                        }
                                    }

                                    var att12 = TablesContainer.list2.GroupBy(i => i.Housekeeping);
                                    if (att12 != null)
                                    {
                                        foreach (var cc in att2)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.Housekeeping += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p12 += cc.Count();
                                        }
                                    }

                                    var att13 = TablesContainer.list2.GroupBy(i => i.Laundry);
                                    if (att13 != null)
                                    {
                                        foreach (var cc in att13)
                                        {
                                            model.Laundry += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p13 += cc.Count();
                                        }
                                    }

                                    var att14 = TablesContainer.list2.GroupBy(i => i.Maintenance);
                                    if (att14 != null)
                                    {
                                        foreach (var cc in att14)
                                        {
                                            model.Maintenance += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p14 += cc.Count();
                                        }
                                    }

                                    var att15 = TablesContainer.list2.GroupBy(i => i.Programs);
                                    if (att15 != null)
                                    {
                                        foreach (var cc in att15)
                                        {
                                            model.Programs += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p15 += cc.Count();
                                        }
                                    }

                                    var att16 = TablesContainer.list2.GroupBy(i => i.Physician);
                                    if (att16 != null)
                                    {
                                        foreach (var cc in att16)
                                        {
                                            model.Physician += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p16 += cc.Count();
                                        }
                                    }

                                    var att17 = TablesContainer.list2.GroupBy(i => i.Beautician);
                                    if (att17 != null)
                                    {
                                        foreach (var cc in att17)
                                        {
                                            model.Beautician += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p17 += cc.Count();
                                        }
                                    }

                                    var att18 = TablesContainer.list2.GroupBy(i => i.FootCare);
                                    if (att18 != null)
                                    {
                                        foreach (var cc in att18)
                                        {
                                            model.FootCare += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p18 += cc.Count();
                                        }
                                    }

                                    var att19 = TablesContainer.list2.GroupBy(i => i.DentalCare);
                                    if (att19 != null)
                                    {
                                        foreach (var cc in att19)
                                        {
                                            model.DentalCare += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p19 += cc.Count();
                                        }
                                    }

                                    var att20 = TablesContainer.list2.GroupBy(i => i.Physio);
                                    if (att20 != null)
                                    {
                                        foreach (var cc in att20)
                                        {
                                            model.Physio += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p20 += cc.Count();
                                        }
                                    }

                                    var att21 = TablesContainer.list2.GroupBy(i => i.Other);
                                    if (att21 != null)
                                    {
                                        foreach (var cc in att21)
                                        {
                                            model.Other += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p21 += cc.Count();
                                        }
                                    }

                                    var att22 = TablesContainer.list2.GroupBy(i => i.MOHLTCNotified);
                                    if (att22 != null)
                                    {
                                        foreach (var cc in att22)
                                        {
                                            model.MOHLTCNotified += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p22 += cc.Count();
                                        }
                                    }

                                    var att23 = TablesContainer.list2.GroupBy(i => i.CopyToVP);
                                    if (att23 != null)
                                    {
                                        foreach (var cc in att23)
                                        {
                                            model.CopyToVP += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p23 += cc.Count();
                                        }
                                    }

                                    var att24 = TablesContainer.list2.GroupBy(i => i.ResponseSent);
                                    if (att15 != null)
                                    {
                                        foreach (var cc in att15)
                                        {
                                            model.ResponseSent += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p24 += cc.Count();
                                        }
                                    }

                                    var att25 = TablesContainer.list2.GroupBy(i => i.ActionToken);
                                    if (att25 != null)
                                    {
                                        foreach (var cc in att25)
                                        {
                                            model.ActionToken += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p25 += cc.Count();
                                        }
                                    }

                                    var att26 = TablesContainer.list2.GroupBy(i => i.Resolved);
                                    if (att26 != null)
                                    {
                                        foreach (var cc in att26)
                                        {
                                            model.Resolved += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p26 += cc.Count();
                                        }
                                    }

                                    var att27 = TablesContainer.list2.GroupBy(i => i.MinistryVisit);
                                    if (att27 != null)
                                    {
                                        foreach (var cc in att27)
                                        {
                                            model.MinistryVisit += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p27 += cc.Count();
                                        }
                                    }
                                }

                                foundSummary2.Add(model);
                                model = new ComplaintsSummary();
                                #endregion

                                #region For the 2nd Location:
                                if (ll2 != null)
                                {
                                    model.LocationName = locList.Find(i => i == "Altamont Care Community\r\n" + " - " + cnt1);
                                    var att1 = TablesContainer.list2.GroupBy(i => i.DateReceived);
                                    if (att1 != null)
                                    {                                     
                                        foreach (var cc in att1)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.DateReceived += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p1 += cc.Count();
                                        }
                                    }

                                    var att2 = TablesContainer.list2.GroupBy(i => i.WritenOrVerbal);
                                    if (att2 != null)
                                    {                                       
                                        foreach (var cc in att2)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.WritenOrVerbal += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p2 += cc.Count();
                                        }
                                    }

                                    var att3 = TablesContainer.list2.GroupBy(i => i.Receive_Directly);
                                    if (att3 != null)
                                    {                                       
                                        foreach (var cc in att3)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.Receive_Directly += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p3 += cc.Count();
                                        }
                                    }

                                    var att4 = TablesContainer.list2.GroupBy(i => i.FromResident);
                                    if (att4 != null)
                                    {                                        
                                        foreach (var cc in att4)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.FromResident += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p4 += cc.Count();
                                        }
                                    }

                                    var att5 = TablesContainer.list2.GroupBy(i => i.ResidentName);
                                    if (att5 != null)
                                    {                                        
                                        foreach (var cc in att5)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.ResidentName += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p5 += cc.Count();
                                        }
                                    }

                                    var att6 = TablesContainer.list2.GroupBy(i => i.Department);
                                    if (att6 != null)
                                    {                                      
                                        foreach (var cc in att6)
                                        {
                                            model.Department += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p6 += cc.Count();
                                        }
                                    }

                                    var att7 = TablesContainer.list2.GroupBy(i => i.BriefDescription);
                                    if (att7 != null)
                                    {                                        
                                        foreach (var cc in att7)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.BriefDescription += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p7 += cc.Count();
                                        }
                                    }

                                    var att8 = TablesContainer.list2.GroupBy(i => i.IsAdministration);
                                    if (att8 != null)
                                    {                                     
                                        foreach (var cc in att8)
                                        {
                                            model.BriefDescription += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p8 += cc.Count();
                                        }
                                    }

                                    var att9 = TablesContainer.list2.GroupBy(i => i.CareServices);
                                    if (att9 != null)
                                    {                                      
                                        foreach (var cc in att9)
                                        {
                                            model.CareServices += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p9 += cc.Count();
                                        }
                                    }

                                    var att10 = TablesContainer.list2.GroupBy(i => i.PalliativeCare);
                                    if (att10 != null)
                                    {                                      
                                        foreach (var cc in att10)
                                        {
                                            model.PalliativeCare += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p10 += cc.Count();
                                        }
                                    }

                                    var att11 = TablesContainer.list2.GroupBy(i => i.Dietary);
                                    if (att11 != null)
                                    {                                      
                                        foreach (var cc in att11)
                                        {
                                            model.Dietary += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p11 += cc.Count();
                                        }
                                    }

                                    var att12 = TablesContainer.list2.GroupBy(i => i.Housekeeping);
                                    if (att12 != null)
                                    {                                      
                                        foreach (var cc in att2)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.Housekeeping += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p12 += cc.Count();
                                        }
                                    }

                                    var att13 = TablesContainer.list2.GroupBy(i => i.Laundry);
                                    if (att13 != null)
                                    {                                       
                                        foreach (var cc in att13)
                                        {
                                            model.Laundry += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p13 += cc.Count();
                                        }
                                    }

                                    var att14 = TablesContainer.list2.GroupBy(i => i.Maintenance);
                                    if (att14 != null)
                                    {                                       
                                        foreach (var cc in att14)
                                        {
                                            model.Maintenance += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p14 += cc.Count();
                                        }
                                    }

                                    var att15 = TablesContainer.list2.GroupBy(i => i.Programs);
                                    if (att15 != null)
                                    {
                                        foreach (var cc in att15)
                                        {
                                            model.Programs += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p15 += cc.Count();
                                        }
                                    }

                                    var att16 = TablesContainer.list2.GroupBy(i => i.Physician);
                                    if (att16 != null)
                                    {                                     
                                        foreach (var cc in att16)
                                        {
                                            model.Physician += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p16 += cc.Count();
                                        }
                                    }

                                    var att17 = TablesContainer.list2.GroupBy(i => i.Beautician);
                                    if (att17 != null)
                                    {                                       
                                        foreach (var cc in att17)
                                        {
                                            model.Beautician += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p17 += cc.Count();
                                        }
                                    }

                                    var att18 = TablesContainer.list2.GroupBy(i => i.FootCare);
                                    if (att18 != null)
                                    {                                       
                                        foreach (var cc in att18)
                                        {
                                            model.FootCare += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p18 += cc.Count();
                                        }
                                    }

                                    var att19 = TablesContainer.list2.GroupBy(i => i.DentalCare);
                                    if (att19 != null)
                                    {                                        
                                        foreach (var cc in att19)
                                        {
                                            model.DentalCare += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p19 += cc.Count();
                                        }
                                    }

                                    var att20 = TablesContainer.list2.GroupBy(i => i.Physio);
                                    if (att20 != null)
                                    {                                      
                                        foreach (var cc in att20)
                                        {
                                            model.Physio += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p20 += cc.Count();
                                        }
                                    }

                                    var att21 = TablesContainer.list2.GroupBy(i => i.Other);
                                    if (att21 != null)
                                    {                                       
                                        foreach (var cc in att21)
                                        {
                                            model.Other += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p21 += cc.Count();
                                        }
                                    }

                                    var att22 = TablesContainer.list2.GroupBy(i => i.MOHLTCNotified);
                                    if (att22 != null)
                                    {                                       
                                        foreach (var cc in att22)
                                        {
                                            model.MOHLTCNotified += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p22 += cc.Count();
                                        }
                                    }

                                    var att23 = TablesContainer.list2.GroupBy(i => i.CopyToVP);
                                    if (att23 != null)
                                    {                                       
                                        foreach (var cc in att23)
                                        {
                                            model.CopyToVP += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p23 += cc.Count();
                                        }
                                    }

                                    var att24 = TablesContainer.list2.GroupBy(i => i.ResponseSent);
                                    if (att15 != null)
                                    {                                       
                                        foreach (var cc in att15)
                                        {
                                            model.ResponseSent += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p24 += cc.Count();
                                        }
                                    }

                                    var att25 = TablesContainer.list2.GroupBy(i => i.ActionToken);
                                    if (att25 != null)
                                    {                                      
                                        foreach (var cc in att25)
                                        {
                                            model.ActionToken += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p25 += cc.Count();
                                        }
                                    }

                                    var att26 = TablesContainer.list2.GroupBy(i => i.Resolved);
                                    if (att26 != null)
                                    {                                      
                                        foreach (var cc in att26)
                                        {
                                            model.Resolved += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p26 += cc.Count();
                                        }
                                    }

                                    var att27 = TablesContainer.list2.GroupBy(i => i.MinistryVisit);
                                    if (att27 != null)
                                    {                                       
                                        foreach (var cc in att27)
                                        {
                                            model.MinistryVisit += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p27 += cc.Count();
                                        }
                                    }
                                }

                                foundSummary2.Add(model);
                                model = new ComplaintsSummary();
                                #endregion

                                #region For the 3rd Location:
                                if (ll3 != null)
                                {
                                    model.LocationName = locList.Find(i => i == "Altamont Care Community\r\n" + " - " + cnt1);
                                    var att1 = TablesContainer.list2.GroupBy(i => i.DateReceived);
                                    if (att1 != null)
                                    {
                                        
                                        foreach (var cc in att1)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.DateReceived += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p1 += cc.Count();
                                        }
                                    }

                                    var att2 = TablesContainer.list2.GroupBy(i => i.WritenOrVerbal);
                                    if (att2 != null)
                                    {
                                        
                                        foreach (var cc in att2)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.WritenOrVerbal += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p2 += cc.Count();
                                        }
                                    }

                                    var att3 = TablesContainer.list2.GroupBy(i => i.Receive_Directly);
                                    if (att3 != null)
                                    {
                                        
                                        foreach (var cc in att3)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.Receive_Directly += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p3 += cc.Count();
                                        }
                                    }

                                    var att4 = TablesContainer.list2.GroupBy(i => i.FromResident);
                                    if (att4 != null)
                                    {
                                        
                                        foreach (var cc in att4)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.FromResident += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p4 += cc.Count();
                                        }
                                    }

                                    var att5 = TablesContainer.list2.GroupBy(i => i.ResidentName);
                                    if (att5 != null)
                                    {
                                        
                                        foreach (var cc in att5)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.ResidentName += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p5 += cc.Count();
                                        }
                                    }

                                    var att6 = TablesContainer.list2.GroupBy(i => i.Department);
                                    if (att6 != null)
                                    {
                                        
                                        foreach (var cc in att6)
                                        {
                                            model.Department += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p6 += cc.Count();
                                        }
                                    }

                                    var att7 = TablesContainer.list2.GroupBy(i => i.BriefDescription);
                                    if (att7 != null)
                                    {
                                        
                                        foreach (var cc in att7)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.BriefDescription += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p7 += cc.Count();
                                        }
                                    }

                                    var att8 = TablesContainer.list2.GroupBy(i => i.IsAdministration);
                                    if (att8 != null)
                                    {
                                        
                                        foreach (var cc in att8)
                                        {
                                            model.BriefDescription += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p8 += cc.Count();
                                        }
                                    }

                                    var att9 = TablesContainer.list2.GroupBy(i => i.CareServices);
                                    if (att9 != null)
                                    {
                                        
                                        foreach (var cc in att9)
                                        {
                                            model.CareServices += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p9 += cc.Count();
                                        }
                                    }

                                    var att10 = TablesContainer.list2.GroupBy(i => i.PalliativeCare);
                                    if (att10 != null)
                                    {
                                        
                                        foreach (var cc in att10)
                                        {
                                            model.PalliativeCare += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p10 += cc.Count();
                                        }
                                    }

                                    var att11 = TablesContainer.list2.GroupBy(i => i.Dietary);
                                    if (att11 != null)
                                    {
                                        
                                        foreach (var cc in att11)
                                        {
                                            model.Dietary += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p11 += cc.Count();
                                        }
                                    }

                                    var att12 = TablesContainer.list2.GroupBy(i => i.Housekeeping);
                                    if (att12 != null)
                                    {
                                        
                                        foreach (var cc in att2)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.Housekeeping += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p12 += cc.Count();
                                        }
                                    }

                                    var att13 = TablesContainer.list2.GroupBy(i => i.Laundry);
                                    if (att13 != null)
                                    {
                                        
                                        foreach (var cc in att13)
                                        {
                                            model.Laundry += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p13 += cc.Count();
                                        }
                                    }

                                    var att14 = TablesContainer.list2.GroupBy(i => i.Maintenance);
                                    if (att14 != null)
                                    {
                                        
                                        foreach (var cc in att14)
                                        {
                                            model.Maintenance += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p14 += cc.Count();
                                        }
                                    }

                                    var att15 = TablesContainer.list2.GroupBy(i => i.Programs);
                                    if (att15 != null)
                                    {

                                        foreach (var cc in att15)
                                        {
                                            model.Programs += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p15 += cc.Count();
                                        }
                                    }

                                    var att16 = TablesContainer.list2.GroupBy(i => i.Physician);
                                    if (att16 != null)
                                    {
                                        
                                        foreach (var cc in att16)
                                        {
                                            model.Physician += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p16 += cc.Count();
                                        }
                                    }

                                    var att17 = TablesContainer.list2.GroupBy(i => i.Beautician);
                                    if (att17 != null)
                                    {
                                        
                                        foreach (var cc in att17)
                                        {
                                            model.Beautician += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p17 += cc.Count();
                                        }
                                    }

                                    var att18 = TablesContainer.list2.GroupBy(i => i.FootCare);
                                    if (att18 != null)
                                    {
                                        
                                        foreach (var cc in att18)
                                        {
                                            model.FootCare += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p18 += cc.Count();
                                        }
                                    }

                                    var att19 = TablesContainer.list2.GroupBy(i => i.DentalCare);
                                    if (att19 != null)
                                    {
                                        
                                        foreach (var cc in att19)
                                        {
                                            model.DentalCare += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p19 += cc.Count();
                                        }
                                    }

                                    var att20 = TablesContainer.list2.GroupBy(i => i.Physio);
                                    if (att20 != null)
                                    {
                                        
                                        foreach (var cc in att20)
                                        {
                                            model.Physio += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p20 += cc.Count();
                                        }
                                    }

                                    var att21 = TablesContainer.list2.GroupBy(i => i.Other);
                                    if (att21 != null)
                                    {
                                        
                                        foreach (var cc in att21)
                                        {
                                            model.Other += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p21 += cc.Count();
                                        }
                                    }

                                    var att22 = TablesContainer.list2.GroupBy(i => i.MOHLTCNotified);
                                    if (att22 != null)
                                    {
                                        
                                        foreach (var cc in att22)
                                        {
                                            model.MOHLTCNotified += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p22 += cc.Count();
                                        }
                                    }

                                    var att23 = TablesContainer.list2.GroupBy(i => i.CopyToVP);
                                    if (att23 != null)
                                    {
                                        
                                        foreach (var cc in att23)
                                        {
                                            model.CopyToVP += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p23 += cc.Count();
                                        }
                                    }

                                    var att24 = TablesContainer.list2.GroupBy(i => i.ResponseSent);
                                    if (att15 != null)
                                    {
                                        
                                        foreach (var cc in att15)
                                        {
                                            model.ResponseSent += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p24 += cc.Count();
                                        }
                                    }

                                    var att25 = TablesContainer.list2.GroupBy(i => i.ActionToken);
                                    if (att25 != null)
                                    {
                                        
                                        foreach (var cc in att25)
                                        {
                                            model.ActionToken += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p25 += cc.Count();
                                        }
                                    }

                                    var att26 = TablesContainer.list2.GroupBy(i => i.Resolved);
                                    if (att26 != null)
                                    {
                                        
                                        foreach (var cc in att26)
                                        {
                                            model.Resolved += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p26 += cc.Count();
                                        }
                                    }

                                    var att27 = TablesContainer.list2.GroupBy(i => i.MinistryVisit);
                                    if (att27 != null)
                                    {
                                        
                                        foreach (var cc in att27)
                                        {
                                            model.MinistryVisit += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p27 += cc.Count();
                                        }
                                    }
                                }

                                foundSummary2.Add(model);
                                model = new ComplaintsSummary();
                                #endregion

                                #region For the 4th Location:
                                if (ll4 != null)
                                {
                                    model.LocationName = locList.Find(i => i == "Altamont Care Community\r\n" + " - " + cnt1);
                                    var att1 = TablesContainer.list2.GroupBy(i => i.DateReceived);
                                    if (att1 != null)
                                    {
                                        
                                        foreach (var cc in att1)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.DateReceived += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p1 += cc.Count();
                                        }
                                    }

                                    var att2 = TablesContainer.list2.GroupBy(i => i.WritenOrVerbal);
                                    if (att2 != null)
                                    {
                                        
                                        foreach (var cc in att2)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.WritenOrVerbal += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p2 += cc.Count();
                                        }
                                    }

                                    var att3 = TablesContainer.list2.GroupBy(i => i.Receive_Directly);
                                    if (att3 != null)
                                    {
                                        
                                        foreach (var cc in att3)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.Receive_Directly += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p3 += cc.Count();
                                        }
                                    }

                                    var att4 = TablesContainer.list2.GroupBy(i => i.FromResident);
                                    if (att4 != null)
                                    {
                                        
                                        foreach (var cc in att4)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.FromResident += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p4 += cc.Count();
                                        }
                                    }

                                    var att5 = TablesContainer.list2.GroupBy(i => i.ResidentName);
                                    if (att5 != null)
                                    {
                                        
                                        foreach (var cc in att5)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.ResidentName += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p5 += cc.Count();
                                        }
                                    }

                                    var att6 = TablesContainer.list2.GroupBy(i => i.Department);
                                    if (att6 != null)
                                    {
                                        
                                        foreach (var cc in att6)
                                        {
                                            model.Department += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p6 += cc.Count();
                                        }
                                    }

                                    var att7 = TablesContainer.list2.GroupBy(i => i.BriefDescription);
                                    if (att7 != null)
                                    {
                                        
                                        foreach (var cc in att7)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.BriefDescription += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p7 += cc.Count();
                                        }
                                    }

                                    var att8 = TablesContainer.list2.GroupBy(i => i.IsAdministration);
                                    if (att8 != null)
                                    {
                                        
                                        foreach (var cc in att8)
                                        {
                                            model.BriefDescription += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p8 += cc.Count();
                                        }
                                    }

                                    var att9 = TablesContainer.list2.GroupBy(i => i.CareServices);
                                    if (att9 != null)
                                    {
                                        
                                        foreach (var cc in att9)
                                        {
                                            model.CareServices += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p9 += cc.Count();
                                        }
                                    }

                                    var att10 = TablesContainer.list2.GroupBy(i => i.PalliativeCare);
                                    if (att10 != null)
                                    {
                                        
                                        foreach (var cc in att10)
                                        {
                                            model.PalliativeCare += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p10 += cc.Count();
                                        }
                                    }

                                    var att11 = TablesContainer.list2.GroupBy(i => i.Dietary);
                                    if (att11 != null)
                                    {
                                        
                                        foreach (var cc in att11)
                                        {
                                            model.Dietary += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p11 += cc.Count();
                                        }
                                    }

                                    var att12 = TablesContainer.list2.GroupBy(i => i.Housekeeping);
                                    if (att12 != null)
                                    {
                                        
                                        foreach (var cc in att2)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.Housekeeping += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p12 += cc.Count();
                                        }
                                    }

                                    var att13 = TablesContainer.list2.GroupBy(i => i.Laundry);
                                    if (att13 != null)
                                    {
                                        
                                        foreach (var cc in att13)
                                        {
                                            model.Laundry += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p13 += cc.Count();
                                        }
                                    }

                                    var att14 = TablesContainer.list2.GroupBy(i => i.Maintenance);
                                    if (att14 != null)
                                    {
                                        
                                        foreach (var cc in att14)
                                        {
                                            model.Maintenance += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p14 += cc.Count();
                                        }
                                    }

                                    var att15 = TablesContainer.list2.GroupBy(i => i.Programs);
                                    if (att15 != null)
                                    {

                                        foreach (var cc in att15)
                                        {
                                            model.Programs += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p15 += cc.Count();
                                        }
                                    }

                                    var att16 = TablesContainer.list2.GroupBy(i => i.Physician);
                                    if (att16 != null)
                                    {
                                        
                                        foreach (var cc in att16)
                                        {
                                            model.Physician += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p16 += cc.Count();
                                        }
                                    }

                                    var att17 = TablesContainer.list2.GroupBy(i => i.Beautician);
                                    if (att17 != null)
                                    {
                                        
                                        foreach (var cc in att17)
                                        {
                                            model.Beautician += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p17 += cc.Count();
                                        }
                                    }

                                    var att18 = TablesContainer.list2.GroupBy(i => i.FootCare);
                                    if (att18 != null)
                                    {
                                        
                                        foreach (var cc in att18)
                                        {
                                            model.FootCare += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p18 += cc.Count();
                                        }
                                    }

                                    var att19 = TablesContainer.list2.GroupBy(i => i.DentalCare);
                                    if (att19 != null)
                                    {
                                        
                                        foreach (var cc in att19)
                                        {
                                            model.DentalCare += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p19 += cc.Count();
                                        }
                                    }

                                    var att20 = TablesContainer.list2.GroupBy(i => i.Physio);
                                    if (att20 != null)
                                    {
                                        
                                        foreach (var cc in att20)
                                        {
                                            model.Physio += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p20 += cc.Count();
                                        }
                                    }

                                    var att21 = TablesContainer.list2.GroupBy(i => i.Other);
                                    if (att21 != null)
                                    {
                                        
                                        foreach (var cc in att21)
                                        {
                                            model.Other += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p21 += cc.Count();
                                        }
                                    }

                                    var att22 = TablesContainer.list2.GroupBy(i => i.MOHLTCNotified);
                                    if (att22 != null)
                                    {
                                        
                                        foreach (var cc in att22)
                                        {
                                            model.MOHLTCNotified += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p22 += cc.Count();
                                        }
                                    }

                                    var att23 = TablesContainer.list2.GroupBy(i => i.CopyToVP);
                                    if (att23 != null)
                                    {
                                        
                                        foreach (var cc in att23)
                                        {
                                            model.CopyToVP += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p23 += cc.Count();
                                        }
                                    }

                                    var att24 = TablesContainer.list2.GroupBy(i => i.ResponseSent);
                                    if (att15 != null)
                                    {
                                        
                                        foreach (var cc in att15)
                                        {
                                            model.ResponseSent += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p24 += cc.Count();
                                        }
                                    }

                                    var att25 = TablesContainer.list2.GroupBy(i => i.ActionToken);
                                    if (att25 != null)
                                    {
                                        
                                        foreach (var cc in att25)
                                        {
                                            model.ActionToken += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p25 += cc.Count();
                                        }
                                    }

                                    var att26 = TablesContainer.list2.GroupBy(i => i.Resolved);
                                    if (att26 != null)
                                    {
                                        
                                        foreach (var cc in att26)
                                        {
                                            model.Resolved += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p26 += cc.Count();
                                        }
                                    }

                                    var att27 = TablesContainer.list2.GroupBy(i => i.MinistryVisit);
                                    if (att27 != null)
                                    {
                                        
                                        foreach (var cc in att27)
                                        {
                                            model.MinistryVisit += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p27 += cc.Count();
                                        }
                                    }
                                }

                                foundSummary2.Add(model);
                                model = new ComplaintsSummary();
                                #endregion

                                #region For the 5th Location:
                                if (ll5 != null)
                                {
                                    model.LocationName = locList.Find(i => i == "Altamont Care Community\r\n" + " - " + cnt1);
                                    var att1 = TablesContainer.list2.GroupBy(i => i.DateReceived);
                                    if (att1 != null)
                                    {
                                        
                                        foreach (var cc in att1)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.DateReceived += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p1 += cc.Count();
                                        }
                                    }

                                    var att2 = TablesContainer.list2.GroupBy(i => i.WritenOrVerbal);
                                    if (att2 != null)
                                    {
                                        
                                        foreach (var cc in att2)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.WritenOrVerbal += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p2 += cc.Count();
                                        }
                                    }

                                    var att3 = TablesContainer.list2.GroupBy(i => i.Receive_Directly);
                                    if (att3 != null)
                                    {
                                        
                                        foreach (var cc in att3)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.Receive_Directly += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p3 += cc.Count();
                                        }
                                    }

                                    var att4 = TablesContainer.list2.GroupBy(i => i.FromResident);
                                    if (att4 != null)
                                    {
                                        
                                        foreach (var cc in att4)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.FromResident += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p4 += cc.Count();
                                        }
                                    }

                                    var att5 = TablesContainer.list2.GroupBy(i => i.ResidentName);
                                    if (att5 != null)
                                    {
                                        
                                        foreach (var cc in att5)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.ResidentName += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p5 += cc.Count();
                                        }
                                    }

                                    var att6 = TablesContainer.list2.GroupBy(i => i.Department);
                                    if (att6 != null)
                                    {
                                        
                                        foreach (var cc in att6)
                                        {
                                            model.Department += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p6 += cc.Count();
                                        }
                                    }

                                    var att7 = TablesContainer.list2.GroupBy(i => i.BriefDescription);
                                    if (att7 != null)
                                    {
                                        
                                        foreach (var cc in att7)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.BriefDescription += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p7 += cc.Count();
                                        }
                                    }

                                    var att8 = TablesContainer.list2.GroupBy(i => i.IsAdministration);
                                    if (att8 != null)
                                    {
                                        
                                        foreach (var cc in att8)
                                        {
                                            model.BriefDescription += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p8 += cc.Count();
                                        }
                                    }

                                    var att9 = TablesContainer.list2.GroupBy(i => i.CareServices);
                                    if (att9 != null)
                                    {
                                        
                                        foreach (var cc in att9)
                                        {
                                            model.CareServices += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p9 += cc.Count();
                                        }
                                    }

                                    var att10 = TablesContainer.list2.GroupBy(i => i.PalliativeCare);
                                    if (att10 != null)
                                    {
                                        
                                        foreach (var cc in att10)
                                        {
                                            model.PalliativeCare += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p10 += cc.Count();
                                        }
                                    }

                                    var att11 = TablesContainer.list2.GroupBy(i => i.Dietary);
                                    if (att11 != null)
                                    {
                                        
                                        foreach (var cc in att11)
                                        {
                                            model.Dietary += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p11 += cc.Count();
                                        }
                                    }

                                    var att12 = TablesContainer.list2.GroupBy(i => i.Housekeeping);
                                    if (att12 != null)
                                    {
                                        
                                        foreach (var cc in att2)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.Housekeeping += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p12 += cc.Count();
                                        }
                                    }

                                    var att13 = TablesContainer.list2.GroupBy(i => i.Laundry);
                                    if (att13 != null)
                                    {
                                        
                                        foreach (var cc in att13)
                                        {
                                            model.Laundry += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p13 += cc.Count();
                                        }
                                    }

                                    var att14 = TablesContainer.list2.GroupBy(i => i.Maintenance);
                                    if (att14 != null)
                                    {
                                        
                                        foreach (var cc in att14)
                                        {
                                            model.Maintenance += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p14 += cc.Count();
                                        }
                                    }

                                    var att15 = TablesContainer.list2.GroupBy(i => i.Programs);
                                    if (att15 != null)
                                    {

                                        foreach (var cc in att15)
                                        {
                                            model.Programs += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p15 += cc.Count();
                                        }
                                    }

                                    var att16 = TablesContainer.list2.GroupBy(i => i.Physician);
                                    if (att16 != null)
                                    {
                                        
                                        foreach (var cc in att16)
                                        {
                                            model.Physician += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p16 += cc.Count();
                                        }
                                    }

                                    var att17 = TablesContainer.list2.GroupBy(i => i.Beautician);
                                    if (att17 != null)
                                    {
                                        
                                        foreach (var cc in att17)
                                        {
                                            model.Beautician += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p17 += cc.Count();
                                        }
                                    }

                                    var att18 = TablesContainer.list2.GroupBy(i => i.FootCare);
                                    if (att18 != null)
                                    {
                                        
                                        foreach (var cc in att18)
                                        {
                                            model.FootCare += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p18 += cc.Count();
                                        }
                                    }

                                    var att19 = TablesContainer.list2.GroupBy(i => i.DentalCare);
                                    if (att19 != null)
                                    {
                                        
                                        foreach (var cc in att19)
                                        {
                                            model.DentalCare += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p19 += cc.Count();
                                        }
                                    }

                                    var att20 = TablesContainer.list2.GroupBy(i => i.Physio);
                                    if (att20 != null)
                                    {
                                        
                                        foreach (var cc in att20)
                                        {
                                            model.Physio += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p20 += cc.Count();
                                        }
                                    }

                                    var att21 = TablesContainer.list2.GroupBy(i => i.Other);
                                    if (att21 != null)
                                    {
                                        
                                        foreach (var cc in att21)
                                        {
                                            model.Other += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p21 += cc.Count();
                                        }
                                    }

                                    var att22 = TablesContainer.list2.GroupBy(i => i.MOHLTCNotified);
                                    if (att22 != null)
                                    {
                                        
                                        foreach (var cc in att22)
                                        {
                                            model.MOHLTCNotified += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p22 += cc.Count();
                                        }
                                    }

                                    var att23 = TablesContainer.list2.GroupBy(i => i.CopyToVP);
                                    if (att23 != null)
                                    {
                                        
                                        foreach (var cc in att23)
                                        {
                                            model.CopyToVP += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p23 += cc.Count();
                                        }
                                    }

                                    var att24 = TablesContainer.list2.GroupBy(i => i.ResponseSent);
                                    if (att15 != null)
                                    {
                                        
                                        foreach (var cc in att15)
                                        {
                                            model.ResponseSent += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p24 += cc.Count();
                                        }
                                    }

                                    var att25 = TablesContainer.list2.GroupBy(i => i.ActionToken);
                                    if (att25 != null)
                                    {
                                        
                                        foreach (var cc in att25)
                                        {
                                            model.ActionToken += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p25 += cc.Count();
                                        }
                                    }

                                    var att26 = TablesContainer.list2.GroupBy(i => i.Resolved);
                                    if (att26 != null)
                                    {
                                        
                                        foreach (var cc in att26)
                                        {
                                            model.Resolved += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p26 += cc.Count();
                                        }
                                    }

                                    var att27 = TablesContainer.list2.GroupBy(i => i.MinistryVisit);
                                    if (att27 != null)
                                    {
                                        
                                        foreach (var cc in att27)
                                        {
                                            model.MinistryVisit += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p27 += cc.Count();
                                        }
                                    }
                                }

                                foundSummary2.Add(model);
                                model = new ComplaintsSummary();
                                #endregion

                                #region For the 6th Location:
                                if (ll6 != null)
                                {
                                    model.LocationName = locList.Find(i => i == "Altamont Care Community\r\n" + " - " + cnt1);
                                    var att1 = TablesContainer.list2.GroupBy(i => i.DateReceived);
                                    if (att1 != null)
                                    {
                                        
                                        foreach (var cc in att1)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.DateReceived += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p1 += cc.Count();
                                        }
                                    }

                                    var att2 = TablesContainer.list2.GroupBy(i => i.WritenOrVerbal);
                                    if (att2 != null)
                                    {
                                        
                                        foreach (var cc in att2)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.WritenOrVerbal += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p2 += cc.Count();
                                        }
                                    }

                                    var att3 = TablesContainer.list2.GroupBy(i => i.Receive_Directly);
                                    if (att3 != null)
                                    {
                                        
                                        foreach (var cc in att3)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.Receive_Directly += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p3 += cc.Count();
                                        }
                                    }

                                    var att4 = TablesContainer.list2.GroupBy(i => i.FromResident);
                                    if (att4 != null)
                                    {
                                        
                                        foreach (var cc in att4)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.FromResident += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p4 += cc.Count();
                                        }
                                    }

                                    var att5 = TablesContainer.list2.GroupBy(i => i.ResidentName);
                                    if (att5 != null)
                                    {
                                        
                                        foreach (var cc in att5)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.ResidentName += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p5 += cc.Count();
                                        }
                                    }

                                    var att6 = TablesContainer.list2.GroupBy(i => i.Department);
                                    if (att6 != null)
                                    {
                                        
                                        foreach (var cc in att6)
                                        {
                                            model.Department += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p6 += cc.Count();
                                        }
                                    }

                                    var att7 = TablesContainer.list2.GroupBy(i => i.BriefDescription);
                                    if (att7 != null)
                                    {
                                        
                                        foreach (var cc in att7)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.BriefDescription += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p7 += cc.Count();
                                        }
                                    }

                                    var att8 = TablesContainer.list2.GroupBy(i => i.IsAdministration);
                                    if (att8 != null)
                                    {
                                        
                                        foreach (var cc in att8)
                                        {
                                            model.BriefDescription += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p8 += cc.Count();
                                        }
                                    }

                                    var att9 = TablesContainer.list2.GroupBy(i => i.CareServices);
                                    if (att9 != null)
                                    {
                                        
                                        foreach (var cc in att9)
                                        {
                                            model.CareServices += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p9 += cc.Count();
                                        }
                                    }

                                    var att10 = TablesContainer.list2.GroupBy(i => i.PalliativeCare);
                                    if (att10 != null)
                                    {
                                        
                                        foreach (var cc in att10)
                                        {
                                            model.PalliativeCare += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p10 += cc.Count();
                                        }
                                    }

                                    var att11 = TablesContainer.list2.GroupBy(i => i.Dietary);
                                    if (att11 != null)
                                    {
                                        
                                        foreach (var cc in att11)
                                        {
                                            model.Dietary += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p11 += cc.Count();
                                        }
                                    }

                                    var att12 = TablesContainer.list2.GroupBy(i => i.Housekeeping);
                                    if (att12 != null)
                                    {
                                        
                                        foreach (var cc in att2)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.Housekeeping += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p12 += cc.Count();
                                        }
                                    }

                                    var att13 = TablesContainer.list2.GroupBy(i => i.Laundry);
                                    if (att13 != null)
                                    {
                                        
                                        foreach (var cc in att13)
                                        {
                                            model.Laundry += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p13 += cc.Count();
                                        }
                                    }

                                    var att14 = TablesContainer.list2.GroupBy(i => i.Maintenance);
                                    if (att14 != null)
                                    {
                                        
                                        foreach (var cc in att14)
                                        {
                                            model.Maintenance += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p14 += cc.Count();
                                        }
                                    }

                                    var att15 = TablesContainer.list2.GroupBy(i => i.Programs);
                                    if (att15 != null)
                                    {

                                        foreach (var cc in att15)
                                        {
                                            model.Programs += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p15 += cc.Count();
                                        }
                                    }

                                    var att16 = TablesContainer.list2.GroupBy(i => i.Physician);
                                    if (att16 != null)
                                    {
                                        
                                        foreach (var cc in att16)
                                        {
                                            model.Physician += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p16 += cc.Count();
                                        }
                                    }

                                    var att17 = TablesContainer.list2.GroupBy(i => i.Beautician);
                                    if (att17 != null)
                                    {
                                        
                                        foreach (var cc in att17)
                                        {
                                            model.Beautician += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p17 += cc.Count();
                                        }
                                    }

                                    var att18 = TablesContainer.list2.GroupBy(i => i.FootCare);
                                    if (att18 != null)
                                    {
                                        
                                        foreach (var cc in att18)
                                        {
                                            model.FootCare += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p18 += cc.Count();
                                        }
                                    }

                                    var att19 = TablesContainer.list2.GroupBy(i => i.DentalCare);
                                    if (att19 != null)
                                    {
                                        
                                        foreach (var cc in att19)
                                        {
                                            model.DentalCare += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p19 += cc.Count();
                                        }
                                    }

                                    var att20 = TablesContainer.list2.GroupBy(i => i.Physio);
                                    if (att20 != null)
                                    {
                                        
                                        foreach (var cc in att20)
                                        {
                                            model.Physio += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p20 += cc.Count();
                                        }
                                    }

                                    var att21 = TablesContainer.list2.GroupBy(i => i.Other);
                                    if (att21 != null)
                                    {
                                        
                                        foreach (var cc in att21)
                                        {
                                            model.Other += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p21 += cc.Count();
                                        }
                                    }

                                    var att22 = TablesContainer.list2.GroupBy(i => i.MOHLTCNotified);
                                    if (att22 != null)
                                    {
                                        
                                        foreach (var cc in att22)
                                        {
                                            model.MOHLTCNotified += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p22 += cc.Count();
                                        }
                                    }

                                    var att23 = TablesContainer.list2.GroupBy(i => i.CopyToVP);
                                    if (att23 != null)
                                    {
                                        
                                        foreach (var cc in att23)
                                        {
                                            model.CopyToVP += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p23 += cc.Count();
                                        }
                                    }

                                    var att24 = TablesContainer.list2.GroupBy(i => i.ResponseSent);
                                    if (att15 != null)
                                    {
                                        
                                        foreach (var cc in att15)
                                        {
                                            model.ResponseSent += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p24 += cc.Count();
                                        }
                                    }

                                    var att25 = TablesContainer.list2.GroupBy(i => i.ActionToken);
                                    if (att25 != null)
                                    {
                                        
                                        foreach (var cc in att25)
                                        {
                                            model.ActionToken += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p25 += cc.Count();
                                        }
                                    }

                                    var att26 = TablesContainer.list2.GroupBy(i => i.Resolved);
                                    if (att26 != null)
                                    {
                                        
                                        foreach (var cc in att26)
                                        {
                                            model.Resolved += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p26 += cc.Count();
                                        }
                                    }

                                    var att27 = TablesContainer.list2.GroupBy(i => i.MinistryVisit);
                                    if (att27 != null)
                                    {
                                        
                                        foreach (var cc in att27)
                                        {
                                            model.MinistryVisit += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p27 += cc.Count();
                                        }
                                    }
                                }

                                foundSummary2.Add(model);
                                model = new ComplaintsSummary();
                                #endregion

                                #region For the 7th Location:
                                if (ll7 != null)
                                {
                                    model.LocationName = locList.Find(i => i == "Altamont Care Community\r\n" + " - " + cnt1);
                                    var att1 = TablesContainer.list2.GroupBy(i => i.DateReceived);
                                    if (att1 != null)
                                    {
                                        
                                        foreach (var cc in att1)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.DateReceived += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p1 += cc.Count();
                                        }
                                    }

                                    var att2 = TablesContainer.list2.GroupBy(i => i.WritenOrVerbal);
                                    if (att2 != null)
                                    {
                                        
                                        foreach (var cc in att2)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.WritenOrVerbal += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p2 += cc.Count();
                                        }
                                    }

                                    var att3 = TablesContainer.list2.GroupBy(i => i.Receive_Directly);
                                    if (att3 != null)
                                    {
                                        
                                        foreach (var cc in att3)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.Receive_Directly += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p3 += cc.Count();
                                        }
                                    }

                                    var att4 = TablesContainer.list2.GroupBy(i => i.FromResident);
                                    if (att4 != null)
                                    {
                                        
                                        foreach (var cc in att4)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.FromResident += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p4 += cc.Count();
                                        }
                                    }

                                    var att5 = TablesContainer.list2.GroupBy(i => i.ResidentName);
                                    if (att5 != null)
                                    {
                                        
                                        foreach (var cc in att5)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.ResidentName += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p5 += cc.Count();
                                        }
                                    }

                                    var att6 = TablesContainer.list2.GroupBy(i => i.Department);
                                    if (att6 != null)
                                    {
                                        
                                        foreach (var cc in att6)
                                        {
                                            model.Department += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p6 += cc.Count();
                                        }
                                    }

                                    var att7 = TablesContainer.list2.GroupBy(i => i.BriefDescription);
                                    if (att7 != null)
                                    {
                                        
                                        foreach (var cc in att7)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.BriefDescription += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p7 += cc.Count();
                                        }
                                    }

                                    var att8 = TablesContainer.list2.GroupBy(i => i.IsAdministration);
                                    if (att8 != null)
                                    {
                                        
                                        foreach (var cc in att8)
                                        {
                                            model.BriefDescription += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p8 += cc.Count();
                                        }
                                    }

                                    var att9 = TablesContainer.list2.GroupBy(i => i.CareServices);
                                    if (att9 != null)
                                    {
                                        
                                        foreach (var cc in att9)
                                        {
                                            model.CareServices += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p9 += cc.Count();
                                        }
                                    }

                                    var att10 = TablesContainer.list2.GroupBy(i => i.PalliativeCare);
                                    if (att10 != null)
                                    {
                                        
                                        foreach (var cc in att10)
                                        {
                                            model.PalliativeCare += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p10 += cc.Count();
                                        }
                                    }

                                    var att11 = TablesContainer.list2.GroupBy(i => i.Dietary);
                                    if (att11 != null)
                                    {
                                        
                                        foreach (var cc in att11)
                                        {
                                            model.Dietary += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p11 += cc.Count();
                                        }
                                    }

                                    var att12 = TablesContainer.list2.GroupBy(i => i.Housekeeping);
                                    if (att12 != null)
                                    {
                                        
                                        foreach (var cc in att2)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.Housekeeping += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p12 += cc.Count();
                                        }
                                    }

                                    var att13 = TablesContainer.list2.GroupBy(i => i.Laundry);
                                    if (att13 != null)
                                    {
                                        
                                        foreach (var cc in att13)
                                        {
                                            model.Laundry += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p13 += cc.Count();
                                        }
                                    }

                                    var att14 = TablesContainer.list2.GroupBy(i => i.Maintenance);
                                    if (att14 != null)
                                    {
                                        
                                        foreach (var cc in att14)
                                        {
                                            model.Maintenance += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p14 += cc.Count();
                                        }
                                    }

                                    var att15 = TablesContainer.list2.GroupBy(i => i.Programs);
                                    if (att15 != null)
                                    {

                                        foreach (var cc in att15)
                                        {
                                            model.Programs += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p15 += cc.Count();
                                        }
                                    }

                                    var att16 = TablesContainer.list2.GroupBy(i => i.Physician);
                                    if (att16 != null)
                                    {
                                        
                                        foreach (var cc in att16)
                                        {
                                            model.Physician += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p16 += cc.Count();
                                        }
                                    }

                                    var att17 = TablesContainer.list2.GroupBy(i => i.Beautician);
                                    if (att17 != null)
                                    {
                                        
                                        foreach (var cc in att17)
                                        {
                                            model.Beautician += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p17 += cc.Count();
                                        }
                                    }

                                    var att18 = TablesContainer.list2.GroupBy(i => i.FootCare);
                                    if (att18 != null)
                                    {
                                        
                                        foreach (var cc in att18)
                                        {
                                            model.FootCare += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p18 += cc.Count();
                                        }
                                    }

                                    var att19 = TablesContainer.list2.GroupBy(i => i.DentalCare);
                                    if (att19 != null)
                                    {
                                        
                                        foreach (var cc in att19)
                                        {
                                            model.DentalCare += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p19 += cc.Count();
                                        }
                                    }

                                    var att20 = TablesContainer.list2.GroupBy(i => i.Physio);
                                    if (att20 != null)
                                    {
                                        
                                        foreach (var cc in att20)
                                        {
                                            model.Physio += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p20 += cc.Count();
                                        }
                                    }

                                    var att21 = TablesContainer.list2.GroupBy(i => i.Other);
                                    if (att21 != null)
                                    {
                                        
                                        foreach (var cc in att21)
                                        {
                                            model.Other += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p21 += cc.Count();
                                        }
                                    }

                                    var att22 = TablesContainer.list2.GroupBy(i => i.MOHLTCNotified);
                                    if (att22 != null)
                                    {
                                        
                                        foreach (var cc in att22)
                                        {
                                            model.MOHLTCNotified += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p22 += cc.Count();
                                        }
                                    }

                                    var att23 = TablesContainer.list2.GroupBy(i => i.CopyToVP);
                                    if (att23 != null)
                                    {
                                        
                                        foreach (var cc in att23)
                                        {
                                            model.CopyToVP += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p23 += cc.Count();
                                        }
                                    }

                                    var att24 = TablesContainer.list2.GroupBy(i => i.ResponseSent);
                                    if (att15 != null)
                                    {
                                        
                                        foreach (var cc in att15)
                                        {
                                            model.ResponseSent += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p24 += cc.Count();
                                        }
                                    }

                                    var att25 = TablesContainer.list2.GroupBy(i => i.ActionToken);
                                    if (att25 != null)
                                    {
                                        
                                        foreach (var cc in att25)
                                        {
                                            model.ActionToken += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p25 += cc.Count();
                                        }
                                    }

                                    var att26 = TablesContainer.list2.GroupBy(i => i.Resolved);
                                    if (att26 != null)
                                    {
                                        
                                        foreach (var cc in att26)
                                        {
                                            model.Resolved += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p26 += cc.Count();
                                        }
                                    }

                                    var att27 = TablesContainer.list2.GroupBy(i => i.MinistryVisit);
                                    if (att27 != null)
                                    {
                                        
                                        foreach (var cc in att27)
                                        {
                                            model.MinistryVisit += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p27 += cc.Count();
                                        }
                                    }
                                }

                                foundSummary2.Add(model);
                                model = new ComplaintsSummary();
                                #endregion

                                #region For the 8th Location:
                                if (ll8 != null)
                                {
                                    model.LocationName = locList.Find(i => i == "Altamont Care Community\r\n" + " - " + cnt1);
                                    var att1 = TablesContainer.list2.GroupBy(i => i.DateReceived);
                                    if (att1 != null)
                                    {
                                        
                                        foreach (var cc in att1)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.DateReceived += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p1 += cc.Count();
                                        }
                                    }

                                    var att2 = TablesContainer.list2.GroupBy(i => i.WritenOrVerbal);
                                    if (att2 != null)
                                    {
                                        
                                        foreach (var cc in att2)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.WritenOrVerbal += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p2 += cc.Count();
                                        }
                                    }

                                    var att3 = TablesContainer.list2.GroupBy(i => i.Receive_Directly);
                                    if (att3 != null)
                                    {
                                        
                                        foreach (var cc in att3)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.Receive_Directly += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p3 += cc.Count();
                                        }
                                    }

                                    var att4 = TablesContainer.list2.GroupBy(i => i.FromResident);
                                    if (att4 != null)
                                    {
                                        
                                        foreach (var cc in att4)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.FromResident += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p4 += cc.Count();
                                        }
                                    }

                                    var att5 = TablesContainer.list2.GroupBy(i => i.ResidentName);
                                    if (att5 != null)
                                    {
                                        
                                        foreach (var cc in att5)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.ResidentName += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p5 += cc.Count();
                                        }
                                    }

                                    var att6 = TablesContainer.list2.GroupBy(i => i.Department);
                                    if (att6 != null)
                                    {
                                        
                                        foreach (var cc in att6)
                                        {
                                            model.Department += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p6 += cc.Count();
                                        }
                                    }

                                    var att7 = TablesContainer.list2.GroupBy(i => i.BriefDescription);
                                    if (att7 != null)
                                    {
                                        
                                        foreach (var cc in att7)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.BriefDescription += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p7 += cc.Count();
                                        }
                                    }

                                    var att8 = TablesContainer.list2.GroupBy(i => i.IsAdministration);
                                    if (att8 != null)
                                    {
                                        
                                        foreach (var cc in att8)
                                        {
                                            model.BriefDescription += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p8 += cc.Count();
                                        }
                                    }

                                    var att9 = TablesContainer.list2.GroupBy(i => i.CareServices);
                                    if (att9 != null)
                                    {
                                        
                                        foreach (var cc in att9)
                                        {
                                            model.CareServices += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p9 += cc.Count();
                                        }
                                    }

                                    var att10 = TablesContainer.list2.GroupBy(i => i.PalliativeCare);
                                    if (att10 != null)
                                    {
                                        
                                        foreach (var cc in att10)
                                        {
                                            model.PalliativeCare += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p10 += cc.Count();
                                        }
                                    }

                                    var att11 = TablesContainer.list2.GroupBy(i => i.Dietary);
                                    if (att11 != null)
                                    {
                                        
                                        foreach (var cc in att11)
                                        {
                                            model.Dietary += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p11 += cc.Count();
                                        }
                                    }

                                    var att12 = TablesContainer.list2.GroupBy(i => i.Housekeeping);
                                    if (att12 != null)
                                    {
                                        
                                        foreach (var cc in att2)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.Housekeeping += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p12 += cc.Count();
                                        }
                                    }

                                    var att13 = TablesContainer.list2.GroupBy(i => i.Laundry);
                                    if (att13 != null)
                                    {
                                        
                                        foreach (var cc in att13)
                                        {
                                            model.Laundry += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p13 += cc.Count();
                                        }
                                    }

                                    var att14 = TablesContainer.list2.GroupBy(i => i.Maintenance);
                                    if (att14 != null)
                                    {
                                        
                                        foreach (var cc in att14)
                                        {
                                            model.Maintenance += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p14 += cc.Count();
                                        }
                                    }

                                    var att15 = TablesContainer.list2.GroupBy(i => i.Programs);
                                    if (att15 != null)
                                    {

                                        foreach (var cc in att15)
                                        {
                                            model.Programs += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p15 += cc.Count();
                                        }
                                    }

                                    var att16 = TablesContainer.list2.GroupBy(i => i.Physician);
                                    if (att16 != null)
                                    {
                                        
                                        foreach (var cc in att16)
                                        {
                                            model.Physician += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p16 += cc.Count();
                                        }
                                    }

                                    var att17 = TablesContainer.list2.GroupBy(i => i.Beautician);
                                    if (att17 != null)
                                    {
                                        
                                        foreach (var cc in att17)
                                        {
                                            model.Beautician += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p17 += cc.Count();
                                        }
                                    }

                                    var att18 = TablesContainer.list2.GroupBy(i => i.FootCare);
                                    if (att18 != null)
                                    {
                                        
                                        foreach (var cc in att18)
                                        {
                                            model.FootCare += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p18 += cc.Count();
                                        }
                                    }

                                    var att19 = TablesContainer.list2.GroupBy(i => i.DentalCare);
                                    if (att19 != null)
                                    {
                                        
                                        foreach (var cc in att19)
                                        {
                                            model.DentalCare += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p19 += cc.Count();
                                        }
                                    }

                                    var att20 = TablesContainer.list2.GroupBy(i => i.Physio);
                                    if (att20 != null)
                                    {
                                        
                                        foreach (var cc in att20)
                                        {
                                            model.Physio += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p20 += cc.Count();
                                        }
                                    }

                                    var att21 = TablesContainer.list2.GroupBy(i => i.Other);
                                    if (att21 != null)
                                    {
                                        
                                        foreach (var cc in att21)
                                        {
                                            model.Other += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p21 += cc.Count();
                                        }
                                    }

                                    var att22 = TablesContainer.list2.GroupBy(i => i.MOHLTCNotified);
                                    if (att22 != null)
                                    {
                                        
                                        foreach (var cc in att22)
                                        {
                                            model.MOHLTCNotified += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p22 += cc.Count();
                                        }
                                    }

                                    var att23 = TablesContainer.list2.GroupBy(i => i.CopyToVP);
                                    if (att23 != null)
                                    {
                                        
                                        foreach (var cc in att23)
                                        {
                                            model.CopyToVP += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p23 += cc.Count();
                                        }
                                    }

                                    var att24 = TablesContainer.list2.GroupBy(i => i.ResponseSent);
                                    if (att15 != null)
                                    {
                                        
                                        foreach (var cc in att15)
                                        {
                                            model.ResponseSent += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p24 += cc.Count();
                                        }
                                    }

                                    var att25 = TablesContainer.list2.GroupBy(i => i.ActionToken);
                                    if (att25 != null)
                                    {
                                        
                                        foreach (var cc in att25)
                                        {
                                            model.ActionToken += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p25 += cc.Count();
                                        }
                                    }

                                    var att26 = TablesContainer.list2.GroupBy(i => i.Resolved);
                                    if (att26 != null)
                                    {
                                        
                                        foreach (var cc in att26)
                                        {
                                            model.Resolved += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p26 += cc.Count();
                                        }
                                    }

                                    var att27 = TablesContainer.list2.GroupBy(i => i.MinistryVisit);
                                    if (att27 != null)
                                    {
                                        
                                        foreach (var cc in att27)
                                        {
                                            model.MinistryVisit += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p27 += cc.Count();
                                        }
                                    }
                                }

                                foundSummary2.Add(model);
                                model = new ComplaintsSummary();
                                #endregion

                                #region For the 9th Location:
                                if (ll9 != null)
                                {
                                    model.LocationName = locList.Find(i => i == "Altamont Care Community\r\n" + " - " + cnt1);
                                    var att1 = TablesContainer.list2.GroupBy(i => i.DateReceived);
                                    if (att1 != null)
                                    {
                                        
                                        foreach (var cc in att1)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.DateReceived += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p1 += cc.Count();
                                        }
                                    }

                                    var att2 = TablesContainer.list2.GroupBy(i => i.WritenOrVerbal);
                                    if (att2 != null)
                                    {
                                        
                                        foreach (var cc in att2)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.WritenOrVerbal += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p2 += cc.Count();
                                        }
                                    }

                                    var att3 = TablesContainer.list2.GroupBy(i => i.Receive_Directly);
                                    if (att3 != null)
                                    {
                                        
                                        foreach (var cc in att3)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.Receive_Directly += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p3 += cc.Count();
                                        }
                                    }

                                    var att4 = TablesContainer.list2.GroupBy(i => i.FromResident);
                                    if (att4 != null)
                                    {
                                        
                                        foreach (var cc in att4)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.FromResident += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p4 += cc.Count();
                                        }
                                    }

                                    var att5 = TablesContainer.list2.GroupBy(i => i.ResidentName);
                                    if (att5 != null)
                                    {
                                        
                                        foreach (var cc in att5)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.ResidentName += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p5 += cc.Count();
                                        }
                                    }

                                    var att6 = TablesContainer.list2.GroupBy(i => i.Department);
                                    if (att6 != null)
                                    {
                                        
                                        foreach (var cc in att6)
                                        {
                                            model.Department += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p6 += cc.Count();
                                        }
                                    }

                                    var att7 = TablesContainer.list2.GroupBy(i => i.BriefDescription);
                                    if (att7 != null)
                                    {
                                        
                                        foreach (var cc in att7)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.BriefDescription += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p7 += cc.Count();
                                        }
                                    }

                                    var att8 = TablesContainer.list2.GroupBy(i => i.IsAdministration);
                                    if (att8 != null)
                                    {
                                        
                                        foreach (var cc in att8)
                                        {
                                            model.BriefDescription += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p8 += cc.Count();
                                        }
                                    }

                                    var att9 = TablesContainer.list2.GroupBy(i => i.CareServices);
                                    if (att9 != null)
                                    {
                                        
                                        foreach (var cc in att9)
                                        {
                                            model.CareServices += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p9 += cc.Count();
                                        }
                                    }

                                    var att10 = TablesContainer.list2.GroupBy(i => i.PalliativeCare);
                                    if (att10 != null)
                                    {
                                        
                                        foreach (var cc in att10)
                                        {
                                            model.PalliativeCare += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p10 += cc.Count();
                                        }
                                    }

                                    var att11 = TablesContainer.list2.GroupBy(i => i.Dietary);
                                    if (att11 != null)
                                    {
                                        
                                        foreach (var cc in att11)
                                        {
                                            model.Dietary += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p11 += cc.Count();
                                        }
                                    }

                                    var att12 = TablesContainer.list2.GroupBy(i => i.Housekeeping);
                                    if (att12 != null)
                                    {
                                        
                                        foreach (var cc in att2)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.Housekeeping += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p12 += cc.Count();
                                        }
                                    }

                                    var att13 = TablesContainer.list2.GroupBy(i => i.Laundry);
                                    if (att13 != null)
                                    {
                                        
                                        foreach (var cc in att13)
                                        {
                                            model.Laundry += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p13 += cc.Count();
                                        }
                                    }

                                    var att14 = TablesContainer.list2.GroupBy(i => i.Maintenance);
                                    if (att14 != null)
                                    {
                                        
                                        foreach (var cc in att14)
                                        {
                                            model.Maintenance += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p14 += cc.Count();
                                        }
                                    }

                                    var att15 = TablesContainer.list2.GroupBy(i => i.Programs);
                                    if (att15 != null)
                                    {

                                        foreach (var cc in att15)
                                        {
                                            model.Programs += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p15 += cc.Count();
                                        }
                                    }

                                    var att16 = TablesContainer.list2.GroupBy(i => i.Physician);
                                    if (att16 != null)
                                    {
                                        
                                        foreach (var cc in att16)
                                        {
                                            model.Physician += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p16 += cc.Count();
                                        }
                                    }

                                    var att17 = TablesContainer.list2.GroupBy(i => i.Beautician);
                                    if (att17 != null)
                                    {
                                        
                                        foreach (var cc in att17)
                                        {
                                            model.Beautician += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p17 += cc.Count();
                                        }
                                    }

                                    var att18 = TablesContainer.list2.GroupBy(i => i.FootCare);
                                    if (att18 != null)
                                    {
                                        
                                        foreach (var cc in att18)
                                        {
                                            model.FootCare += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p18 += cc.Count();
                                        }
                                    }

                                    var att19 = TablesContainer.list2.GroupBy(i => i.DentalCare);
                                    if (att19 != null)
                                    {
                                        
                                        foreach (var cc in att19)
                                        {
                                            model.DentalCare += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p19 += cc.Count();
                                        }
                                    }

                                    var att20 = TablesContainer.list2.GroupBy(i => i.Physio);
                                    if (att20 != null)
                                    {
                                        
                                        foreach (var cc in att20)
                                        {
                                            model.Physio += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p20 += cc.Count();
                                        }
                                    }

                                    var att21 = TablesContainer.list2.GroupBy(i => i.Other);
                                    if (att21 != null)
                                    {
                                        
                                        foreach (var cc in att21)
                                        {
                                            model.Other += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p21 += cc.Count();
                                        }
                                    }

                                    var att22 = TablesContainer.list2.GroupBy(i => i.MOHLTCNotified);
                                    if (att22 != null)
                                    {
                                        
                                        foreach (var cc in att22)
                                        {
                                            model.MOHLTCNotified += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p22 += cc.Count();
                                        }
                                    }

                                    var att23 = TablesContainer.list2.GroupBy(i => i.CopyToVP);
                                    if (att23 != null)
                                    {
                                        
                                        foreach (var cc in att23)
                                        {
                                            model.CopyToVP += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p23 += cc.Count();
                                        }
                                    }

                                    var att24 = TablesContainer.list2.GroupBy(i => i.ResponseSent);
                                    if (att15 != null)
                                    {
                                        
                                        foreach (var cc in att15)
                                        {
                                            model.ResponseSent += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p24 += cc.Count();
                                        }
                                    }

                                    var att25 = TablesContainer.list2.GroupBy(i => i.ActionToken);
                                    if (att25 != null)
                                    {
                                        
                                        foreach (var cc in att25)
                                        {
                                            model.ActionToken += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p25 += cc.Count();
                                        }
                                    }

                                    var att26 = TablesContainer.list2.GroupBy(i => i.Resolved);
                                    if (att26 != null)
                                    {
                                        
                                        foreach (var cc in att26)
                                        {
                                            model.Resolved += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p26 += cc.Count();
                                        }
                                    }

                                    var att27 = TablesContainer.list2.GroupBy(i => i.MinistryVisit);
                                    if (att27 != null)
                                    {
                                        
                                        foreach (var cc in att27)
                                        {
                                            model.MinistryVisit += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p27 += cc.Count();
                                        }
                                    }
                                }

                                foundSummary2.Add(model);
                                model = new ComplaintsSummary();
                                #endregion

                                #region For the 10th Location:
                                if (ll10 != null)
                                {
                                    model.LocationName = locList.Find(i => i == "Altamont Care Community\r\n" + " - " + cnt1);
                                    var att1 = TablesContainer.list2.GroupBy(i => i.DateReceived);
                                    if (att1 != null)
                                    {
                                        
                                        foreach (var cc in att1)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.DateReceived += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p1 += cc.Count();
                                        }
                                    }

                                    var att2 = TablesContainer.list2.GroupBy(i => i.WritenOrVerbal);
                                    if (att2 != null)
                                    {
                                        
                                        foreach (var cc in att2)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.WritenOrVerbal += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p2 += cc.Count();
                                        }
                                    }

                                    var att3 = TablesContainer.list2.GroupBy(i => i.Receive_Directly);
                                    if (att3 != null)
                                    {
                                        
                                        foreach (var cc in att3)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.Receive_Directly += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p3 += cc.Count();
                                        }
                                    }

                                    var att4 = TablesContainer.list2.GroupBy(i => i.FromResident);
                                    if (att4 != null)
                                    {
                                        
                                        foreach (var cc in att4)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.FromResident += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p4 += cc.Count();
                                        }
                                    }

                                    var att5 = TablesContainer.list2.GroupBy(i => i.ResidentName);
                                    if (att5 != null)
                                    {
                                        
                                        foreach (var cc in att5)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.ResidentName += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p5 += cc.Count();
                                        }
                                    }

                                    var att6 = TablesContainer.list2.GroupBy(i => i.Department);
                                    if (att6 != null)
                                    {
                                        
                                        foreach (var cc in att6)
                                        {
                                            model.Department += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p6 += cc.Count();
                                        }
                                    }

                                    var att7 = TablesContainer.list2.GroupBy(i => i.BriefDescription);
                                    if (att7 != null)
                                    {
                                        
                                        foreach (var cc in att7)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.BriefDescription += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p7 += cc.Count();
                                        }
                                    }

                                    var att8 = TablesContainer.list2.GroupBy(i => i.IsAdministration);
                                    if (att8 != null)
                                    {
                                        
                                        foreach (var cc in att8)
                                        {
                                            model.BriefDescription += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p8 += cc.Count();
                                        }
                                    }

                                    var att9 = TablesContainer.list2.GroupBy(i => i.CareServices);
                                    if (att9 != null)
                                    {
                                        
                                        foreach (var cc in att9)
                                        {
                                            model.CareServices += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p9 += cc.Count();
                                        }
                                    }

                                    var att10 = TablesContainer.list2.GroupBy(i => i.PalliativeCare);
                                    if (att10 != null)
                                    {
                                        
                                        foreach (var cc in att10)
                                        {
                                            model.PalliativeCare += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p10 += cc.Count();
                                        }
                                    }

                                    var att11 = TablesContainer.list2.GroupBy(i => i.Dietary);
                                    if (att11 != null)
                                    {
                                        
                                        foreach (var cc in att11)
                                        {
                                            model.Dietary += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p11 += cc.Count();
                                        }
                                    }

                                    var att12 = TablesContainer.list2.GroupBy(i => i.Housekeeping);
                                    if (att12 != null)
                                    {
                                        
                                        foreach (var cc in att2)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.Housekeeping += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p12 += cc.Count();
                                        }
                                    }

                                    var att13 = TablesContainer.list2.GroupBy(i => i.Laundry);
                                    if (att13 != null)
                                    {
                                        
                                        foreach (var cc in att13)
                                        {
                                            model.Laundry += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p13 += cc.Count();
                                        }
                                    }

                                    var att14 = TablesContainer.list2.GroupBy(i => i.Maintenance);
                                    if (att14 != null)
                                    {
                                        
                                        foreach (var cc in att14)
                                        {
                                            model.Maintenance += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p14 += cc.Count();
                                        }
                                    }

                                    var att15 = TablesContainer.list2.GroupBy(i => i.Programs);
                                    if (att15 != null)
                                    {

                                        foreach (var cc in att15)
                                        {
                                            model.Programs += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p15 += cc.Count();
                                        }
                                    }

                                    var att16 = TablesContainer.list2.GroupBy(i => i.Physician);
                                    if (att16 != null)
                                    {
                                        
                                        foreach (var cc in att16)
                                        {
                                            model.Physician += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p16 += cc.Count();
                                        }
                                    }

                                    var att17 = TablesContainer.list2.GroupBy(i => i.Beautician);
                                    if (att17 != null)
                                    {
                                        
                                        foreach (var cc in att17)
                                        {
                                            model.Beautician += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p17 += cc.Count();
                                        }
                                    }

                                    var att18 = TablesContainer.list2.GroupBy(i => i.FootCare);
                                    if (att18 != null)
                                    {
                                        
                                        foreach (var cc in att18)
                                        {
                                            model.FootCare += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p18 += cc.Count();
                                        }
                                    }

                                    var att19 = TablesContainer.list2.GroupBy(i => i.DentalCare);
                                    if (att19 != null)
                                    {
                                        
                                        foreach (var cc in att19)
                                        {
                                            model.DentalCare += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p19 += cc.Count();
                                        }
                                    }

                                    var att20 = TablesContainer.list2.GroupBy(i => i.Physio);
                                    if (att20 != null)
                                    {
                                        
                                        foreach (var cc in att20)
                                        {
                                            model.Physio += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p20 += cc.Count();
                                        }
                                    }

                                    var att21 = TablesContainer.list2.GroupBy(i => i.Other);
                                    if (att21 != null)
                                    {
                                        
                                        foreach (var cc in att21)
                                        {
                                            model.Other += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p21 += cc.Count();
                                        }
                                    }

                                    var att22 = TablesContainer.list2.GroupBy(i => i.MOHLTCNotified);
                                    if (att22 != null)
                                    {
                                        
                                        foreach (var cc in att22)
                                        {
                                            model.MOHLTCNotified += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p22 += cc.Count();
                                        }
                                    }

                                    var att23 = TablesContainer.list2.GroupBy(i => i.CopyToVP);
                                    if (att23 != null)
                                    {
                                        
                                        foreach (var cc in att23)
                                        {
                                            model.CopyToVP += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p23 += cc.Count();
                                        }
                                    }

                                    var att24 = TablesContainer.list2.GroupBy(i => i.ResponseSent);
                                    if (att15 != null)
                                    {
                                        
                                        foreach (var cc in att15)
                                        {
                                            model.ResponseSent += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p24 += cc.Count();
                                        }
                                    }

                                    var att25 = TablesContainer.list2.GroupBy(i => i.ActionToken);
                                    if (att25 != null)
                                    {
                                        
                                        foreach (var cc in att25)
                                        {
                                            model.ActionToken += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p25 += cc.Count();
                                        }
                                    }

                                    var att26 = TablesContainer.list2.GroupBy(i => i.Resolved);
                                    if (att26 != null)
                                    {
                                        
                                        foreach (var cc in att26)
                                        {
                                            model.Resolved += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p26 += cc.Count();
                                        }
                                    }

                                    var att27 = TablesContainer.list2.GroupBy(i => i.MinistryVisit);
                                    if (att27 != null)
                                    {
                                        
                                        foreach (var cc in att27)
                                        {
                                            model.MinistryVisit += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p27 += cc.Count();
                                        }
                                    }
                                }

                                foundSummary2.Add(model);
                                model = new ComplaintsSummary();
                                #endregion

                                #region For the 11th Location:
                                if (ll11 != null)
                                {
                                    model.LocationName = locList.Find(i => i == "Altamont Care Community\r\n" + " - " + cnt1);
                                    var att1 = TablesContainer.list2.GroupBy(i => i.DateReceived);
                                    if (att1 != null)
                                    {
                                        
                                        foreach (var cc in att1)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.DateReceived += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p1 += cc.Count();
                                        }
                                    }

                                    var att2 = TablesContainer.list2.GroupBy(i => i.WritenOrVerbal);
                                    if (att2 != null)
                                    {
                                        
                                        foreach (var cc in att2)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.WritenOrVerbal += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p2 += cc.Count();
                                        }
                                    }

                                    var att3 = TablesContainer.list2.GroupBy(i => i.Receive_Directly);
                                    if (att3 != null)
                                    {
                                        
                                        foreach (var cc in att3)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.Receive_Directly += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p3 += cc.Count();
                                        }
                                    }

                                    var att4 = TablesContainer.list2.GroupBy(i => i.FromResident);
                                    if (att4 != null)
                                    {
                                        
                                        foreach (var cc in att4)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.FromResident += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p4 += cc.Count();
                                        }
                                    }

                                    var att5 = TablesContainer.list2.GroupBy(i => i.ResidentName);
                                    if (att5 != null)
                                    {
                                        
                                        foreach (var cc in att5)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.ResidentName += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p5 += cc.Count();
                                        }
                                    }

                                    var att6 = TablesContainer.list2.GroupBy(i => i.Department);
                                    if (att6 != null)
                                    {
                                        
                                        foreach (var cc in att6)
                                        {
                                            model.Department += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p6 += cc.Count();
                                        }
                                    }

                                    var att7 = TablesContainer.list2.GroupBy(i => i.BriefDescription);
                                    if (att7 != null)
                                    {
                                        
                                        foreach (var cc in att7)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.BriefDescription += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p7 += cc.Count();
                                        }
                                    }

                                    var att8 = TablesContainer.list2.GroupBy(i => i.IsAdministration);
                                    if (att8 != null)
                                    {
                                        
                                        foreach (var cc in att8)
                                        {
                                            model.BriefDescription += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p8 += cc.Count();
                                        }
                                    }

                                    var att9 = TablesContainer.list2.GroupBy(i => i.CareServices);
                                    if (att9 != null)
                                    {
                                        
                                        foreach (var cc in att9)
                                        {
                                            model.CareServices += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p9 += cc.Count();
                                        }
                                    }

                                    var att10 = TablesContainer.list2.GroupBy(i => i.PalliativeCare);
                                    if (att10 != null)
                                    {
                                        
                                        foreach (var cc in att10)
                                        {
                                            model.PalliativeCare += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p10 += cc.Count();
                                        }
                                    }

                                    var att11 = TablesContainer.list2.GroupBy(i => i.Dietary);
                                    if (att11 != null)
                                    {
                                        
                                        foreach (var cc in att11)
                                        {
                                            model.Dietary += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p11 += cc.Count();
                                        }
                                    }

                                    var att12 = TablesContainer.list2.GroupBy(i => i.Housekeeping);
                                    if (att12 != null)
                                    {
                                        
                                        foreach (var cc in att2)
                                        {
                                            string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                            if (key == "NULL") continue;
                                            model.Housekeeping += $"{key}\t - \t{cc.Count()}" + " | ";
                                            p12 += cc.Count();
                                        }
                                    }

                                    var att13 = TablesContainer.list2.GroupBy(i => i.Laundry);
                                    if (att13 != null)
                                    {
                                        
                                        foreach (var cc in att13)
                                        {
                                            model.Laundry += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p13 += cc.Count();
                                        }
                                    }

                                    var att14 = TablesContainer.list2.GroupBy(i => i.Maintenance);
                                    if (att14 != null)
                                    {
                                        
                                        foreach (var cc in att14)
                                        {
                                            model.Maintenance += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p14 += cc.Count();
                                        }
                                    }

                                    var att15 = TablesContainer.list2.GroupBy(i => i.Programs);
                                    if (att15 != null)
                                    {

                                        foreach (var cc in att15)
                                        {
                                            model.Programs += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p15 += cc.Count();
                                        }
                                    }

                                    var att16 = TablesContainer.list2.GroupBy(i => i.Physician);
                                    if (att16 != null)
                                    {
                                        
                                        foreach (var cc in att16)
                                        {
                                            model.Physician += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p16 += cc.Count();
                                        }
                                    }

                                    var att17 = TablesContainer.list2.GroupBy(i => i.Beautician);
                                    if (att17 != null)
                                    {
                                        
                                        foreach (var cc in att17)
                                        {
                                            model.Beautician += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p17 += cc.Count();
                                        }
                                    }

                                    var att18 = TablesContainer.list2.GroupBy(i => i.FootCare);
                                    if (att18 != null)
                                    {
                                        
                                        foreach (var cc in att18)
                                        {
                                            model.FootCare += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p18 += cc.Count();
                                        }
                                    }

                                    var att19 = TablesContainer.list2.GroupBy(i => i.DentalCare);
                                    if (att19 != null)
                                    {
                                        
                                        foreach (var cc in att19)
                                        {
                                            model.DentalCare += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p19 += cc.Count();
                                        }
                                    }

                                    var att20 = TablesContainer.list2.GroupBy(i => i.Physio);
                                    if (att20 != null)
                                    {
                                        
                                        foreach (var cc in att20)
                                        {
                                            model.Physio += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p20 += cc.Count();
                                        }
                                    }

                                    var att21 = TablesContainer.list2.GroupBy(i => i.Other);
                                    if (att21 != null)
                                    {
                                        
                                        foreach (var cc in att21)
                                        {
                                            model.Other += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p21 += cc.Count();
                                        }
                                    }

                                    var att22 = TablesContainer.list2.GroupBy(i => i.MOHLTCNotified);
                                    if (att22 != null)
                                    {
                                        
                                        foreach (var cc in att22)
                                        {
                                            model.MOHLTCNotified += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p22 += cc.Count();
                                        }
                                    }

                                    var att23 = TablesContainer.list2.GroupBy(i => i.CopyToVP);
                                    if (att23 != null)
                                    {
                                        
                                        foreach (var cc in att23)
                                        {
                                            model.CopyToVP += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p23 += cc.Count();
                                        }
                                    }

                                    var att24 = TablesContainer.list2.GroupBy(i => i.ResponseSent);
                                    if (att15 != null)
                                    {
                                        
                                        foreach (var cc in att15)
                                        {
                                            model.ResponseSent += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p24 += cc.Count();
                                        }
                                    }

                                    var att25 = TablesContainer.list2.GroupBy(i => i.ActionToken);
                                    if (att25 != null)
                                    {
                                        
                                        foreach (var cc in att25)
                                        {
                                            model.ActionToken += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p25 += cc.Count();
                                        }
                                    }

                                    var att26 = TablesContainer.list2.GroupBy(i => i.Resolved);
                                    if (att26 != null)
                                    {
                                        
                                        foreach (var cc in att26)
                                        {
                                            model.Resolved += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p26 += cc.Count();
                                        }
                                    }

                                    var att27 = TablesContainer.list2.GroupBy(i => i.MinistryVisit);
                                    if (att27 != null)
                                    {
                                        
                                        foreach (var cc in att27)
                                        {
                                            model.MinistryVisit += $"{cc.Key}\t - \t{cc.Count()}" + " | ";
                                            p27 += cc.Count();
                                        }
                                    }
                                }

                                foundSummary2.Add(model);
                                model = new ComplaintsSummary();
                                #endregion
                                #endregion

                                #region Add All Summary quantity on List:
                                allSummary2.Add(new ComplaintsSummaryAll
                                {
                                    ActionToken = p1,
                                    Beautician = p2,
                                    PalliativeCare = p3,
                                    ResidentName = p4,
                                    CareServices = p5,
                                    FromResident = p6,
                                    Receive_Directly = p7,
                                    WritenOrVerbal = p8,
                                    DateReceived = p9,
                                    BriefDescription = p10,
                                    CopyToVP = p11,
                                    Department = p12,
                                    DentalCare = p13,
                                    Dietary = p14,
                                    FootCare = p15,
                                    Laundry = p16,
                                    Maintenance = p17,
                                    Housekeeping = p18,
                                    Physician = p19,
                                    Programs = p20,
                                    MinistryVisit = p21,
                                    Resolved = p22,
                                    ResponseSent = p23,
                                    IsAdministration = p24,
                                    Physio = p25,
                                    MOHLTCNotified = p26,
                                    Other = p27
                                });
                                #endregion
                            }

                            #region Create ViewBags:
                            { ViewBag.TotalSummary = allSummary2; }

                            b = true;
                            if (foundSummary1.Count == 0) { b = false; ViewBag.EmptLocation = b; }

                            {
                                ViewBag.Count = TablesContainer.COUNT;
                            }

                            {
                                ViewBag.GN_Found = foundSummary2;
                            }

                            {
                                ViewBag.Entity = "Complaints";
                            }

                            if (locList.Count != 0) isEmpty = true;

                            { ViewBag.Check1 = isEmpty; }

                            {
                                ViewBag.Locations = locList;
                            }

                            {
                                if (role == Role.Admin)
                                    ViewBag.LocInfo = db.Care_Communities.Find(1).Name;
                                else ViewBag.LocInfo = db.Care_Communities.Find(Id_Location).Name;
                            }
                            #endregion                         
                            break;
                        #endregion

                        #region Good_News:
                        case "Good_News":
                            { ViewBag.ObjName = "Good_News"; }
                            TablesContainer.list3 = db.Good_News.ToList();
                            if (TablesContainer.list3.Count() == 0)
                            {
                                ViewBag.ErrorMsg = errorMsg = "Please select a date range.";
                                WorTabs tabs = new WorTabs();
                                tabs.ListForms = GetFormNames();
                                return View(tabs);
                            }

                            strN = new List<string>();
                            //var news = db.Good_News.ToList();
                            var a1 = TablesContainer.list3.GroupBy(i => i.DateNews);
                            if (a1 != null)
                            {
                                strN.Add("Date News: ");
                                foreach (var cc in a1)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var a2 = TablesContainer.list3.GroupBy(i => i.Department);
                            if (a2 != null)
                            {
                                
                                foreach (var cc in a2)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var a3 = TablesContainer.list3.GroupBy(i => i.SourceCompliment);
                            if (a3 != null)
                            {
                                strN.Add("Source Compliment: ");
                                foreach (var cc in a3)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var a4 = TablesContainer.list3.GroupBy(i => i.ReceivedFrom);
                            if (a4 != null)
                            {
                                strN.Add("Received From: ");
                                foreach (var cc in a4)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var a5 = TablesContainer.list3.GroupBy(i => i.Description_Complim);
                            if (a5 != null)
                            {
                                strN.Add("Description Compliment: ");
                                foreach (var cc in a5)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var a6 = TablesContainer.list3.GroupBy(i => i.Respect);
                            if (a6 != null)
                            {
                                strN.Add("Respect: ");
                                foreach (var cc in a6)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var a7 = TablesContainer.list3.GroupBy(i => i.Passion);
                            if (a7 != null)
                            {
                                strN.Add("Passion: ");
                                foreach (var cc in a7)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var a8 = TablesContainer.list3.GroupBy(i => i.Teamwork);
                            if (a8 != null)
                            {
                                strN.Add("Teamwork: ");
                                foreach (var cc in a8)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var a9 = TablesContainer.list3.GroupBy(i => i.Responsibility);
                            if (a9 != null)
                            {
                                strN.Add("Responsibility: ");
                                foreach (var cc in a9)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var a10 = TablesContainer.list3.GroupBy(i => i.Growth);
                            if (a10 != null)
                            {
                                strN.Add("Growth: ");
                                foreach (var cc in a10)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var a11 = TablesContainer.list3.GroupBy(i => i.Compliment);
                            if (a11 != null)
                            {
                                strN.Add("Compliment: ");
                                foreach (var cc in a11)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var a12 = TablesContainer.list3.GroupBy(i => i.Spot_Awards);
                            if (a12 != null)
                            {
                                strN.Add("Spot Awards: ");
                                foreach (var cc in a12)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var a13 = TablesContainer.list3.GroupBy(i => i.Awards_Details);
                            if (a13 != null)
                            {
                                strN.Add("Awards Details: ");
                                foreach (var cc in a13)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var a14 = TablesContainer.list3.GroupBy(i => i.NameAwards);
                            if (a14 != null)
                            {
                                strN.Add("Name Awards: ");
                                foreach (var cc in a14)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var a15 = TablesContainer.list3.GroupBy(i => i.Awards_Received);
                            if (a15 != null)
                            {
                                strN.Add("Awards Received: ");
                                foreach (var cc in a15)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var a16 = TablesContainer.list3.GroupBy(i => i.Community_Inititives);
                            if (a16 != null)
                            {
                                strN.Add("Awards Received: ");
                                foreach (var cc in a16)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            ///
                            b = true; TablesContainer.COUNT = TablesContainer.list3.Count;

                            {
                                ViewBag.Count = TablesContainer.COUNT;
                            }

                            {
                                ViewBag.GN_Found = HomeController.strN;
                            }

                            {
                                ViewBag.Entity = "Good_News";
                            }
                            break;
                        #endregion

                        #region Emergency_Prep: 
                        case "Emergency_Prep":
                            { ViewBag.ObjName = "Emergency_Prep"; }
                            TablesContainer.list4 = db.Emergency_Prep.ToList();
                            if (TablesContainer.list4.Count() == 0)
                            {
                                ViewBag.ErrorMsg = errorMsg = "Please select a date range.";
                                WorTabs tabs = new WorTabs();
                                tabs.ListForms = GetFormNames();
                                return View(tabs);
                            }

                            strN = new List<string>();
                            //var news = db.Good_News.ToList();
                            var e1 = TablesContainer.list4.GroupBy(i => i.Name);
                            if (e1 != null)
                            {
                                strN.Add("Name: ");
                                foreach (var cc in e1)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var e2 = TablesContainer.list4.GroupBy(i => i.Jan);
                            if (e2 != null)
                            {
                                strN.Add("Jan: ");
                                foreach (var cc in e2)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var e3 = TablesContainer.list4.GroupBy(i => i.Feb);
                            if (e3 != null)
                            {
                                strN.Add("Feb: ");
                                foreach (var cc in e3)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var e4 = TablesContainer.list4.GroupBy(i => i.Mar);
                            if (e4 != null)
                            {
                                strN.Add("Mar: ");
                                foreach (var cc in e4)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var e5 = TablesContainer.list4.GroupBy(i => i.Apr);
                            if (e5 != null)
                            {
                                strN.Add("Apr: ");
                                foreach (var cc in e5)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var e6 = TablesContainer.list4.GroupBy(i => i.May);
                            if (e6 != null)
                            {
                                strN.Add("May: ");
                                foreach (var cc in e6)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var e7 = TablesContainer.list4.GroupBy(i => i.Jun);
                            if (e7 != null)
                            {
                                strN.Add("Jun: ");
                                foreach (var cc in e7)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var e8 = TablesContainer.list4.GroupBy(i => i.Jul);
                            if (e8 != null)
                            {
                                strN.Add("Jul: ");
                                foreach (var cc in e8)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var e9 = TablesContainer.list4.GroupBy(i => i.Aug);
                            if (e9 != null)
                            {
                                strN.Add("Aug: ");
                                foreach (var cc in e9)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var e10 = TablesContainer.list4.GroupBy(i => i.Sep);
                            if (e10 != null)
                            {
                                strN.Add("Sep: ");
                                foreach (var cc in e10)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var e11 = TablesContainer.list4.GroupBy(i => i.Oct);
                            if (e11 != null)
                            {
                                strN.Add("Oct: ");
                                foreach (var cc in e11)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var e12 = TablesContainer.list4.GroupBy(i => i.Nov);
                            if (e12 != null)
                            {
                                strN.Add("Nov: ");
                                foreach (var cc in e12)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var e13 = TablesContainer.list4.GroupBy(i => i.Dec);
                            if (e13 != null)
                            {
                                strN.Add("Dec: ");
                                foreach (var cc in e13)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var e14 = TablesContainer.list4.GroupBy(i => i.Dec);
                            if (e13 != null)
                            {
                                strN.Add("Dec: ");
                                foreach (var cc in e13)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }
                            b = true; TablesContainer.COUNT = TablesContainer.list4.Count;

                            {
                                ViewBag.Count = TablesContainer.COUNT;
                            }

                            {
                                ViewBag.GN_Found = HomeController.strN;
                            }

                            {
                                ViewBag.Entity = "Emergency_Prep";
                            }
                            break;
                        #endregion

                        #region Community_Risks: 
                        case "Community_Risks":
                            TablesContainer.list5 = db.Community_Risks.ToList();
                            { ViewBag.ObjName = "Community_Risks"; }
                            if (TablesContainer.list5.Count() == 0)
                            {
                                ViewBag.ErrorMsg = errorMsg = "Please select a date range.";
                                WorTabs tabs = new WorTabs();
                                tabs.ListForms = GetFormNames();
                                return View(tabs);
                            }

                            strN = new List<string>();
                            var c1 = TablesContainer.list5.GroupBy(i => i.Date);
                            if (c1 != null)
                            {
                                strN.Add("Date: ");
                                foreach (var cc in c1)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var c2 = TablesContainer.list5.GroupBy(i => i.Type_Of_Risk);
                            if (c2 != null)
                            {
                                strN.Add("Type Of Risk: ");
                                foreach (var cc in c2)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var c3 = TablesContainer.list5.GroupBy(i => i.Descriptions);
                            if (c3 != null)
                            {
                                strN.Add("Descriptions: ");
                                foreach (var cc in c3)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var c4 = TablesContainer.list5.GroupBy(i => i.Potential_Risk);
                            if (c4 != null)
                            {
                                strN.Add("Potential Risk: ");
                                foreach (var cc in c4)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var c5 = TablesContainer.list5.GroupBy(i => i.MOH_Visit);
                            if (c5 != null)
                            {
                                strN.Add("MOH Visit: ");
                                foreach (var cc in c5)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var c6 = TablesContainer.list5.GroupBy(i => i.Risk_Legal_Action);
                            if (c6 != null)
                            {
                                strN.Add("Risk Legal Action: ");
                                foreach (var cc in c6)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var c7 = TablesContainer.list5.GroupBy(i => i.Hot_Alert);
                            if (c7 != null)
                            {
                                strN.Add("Hot Alert: ");
                                foreach (var cc in c7)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var c8 = TablesContainer.list5.GroupBy(i => i.Status_Update);
                            if (c8 != null)
                            {
                                strN.Add("Status Update: ");
                                foreach (var cc in c8)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    else
                                    {
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }
                            }

                            var c9 = TablesContainer.list5.GroupBy(i => i.Resolved);
                            if (c9 != null)
                            {
                                
                                foreach (var cc in c9)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    else
                                    {
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }
                            }

                            b = true; TablesContainer.COUNT = TablesContainer.list5.Count;

                            {
                                ViewBag.Count = TablesContainer.COUNT;
                            }

                            {
                                ViewBag.GN_Found = HomeController.strN;
                            }

                            {
                                ViewBag.Entity = "Community_Risks";
                            }
                            break;
                        #endregion

                        #region Visits_Others:
                        case "Visits_Others":
                            TablesContainer.list6 = db.Visits_Others.ToList();
                            { ViewBag.ObjName = "Visits_Others"; }
                            if (TablesContainer.list6.Count() == 0)
                            {
                                ViewBag.ErrorMsg = errorMsg = "Please select a date range.";
                                WorTabs tabs = new WorTabs();
                                tabs.ListForms = GetFormNames();
                                return View(tabs);
                            }

                            strN = new List<string>();
                            var v1 = TablesContainer.list6.GroupBy(i => i.Date_of_Visit);
                            if (v1 != null)
                            {
                                strN.Add("Date of Visit: ");
                                foreach (var cc in v1)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var v2 = TablesContainer.list6.GroupBy(i => i.Agency);
                            if (v2 != null)
                            {
                                strN.Add("Agency: ");
                                foreach (var cc in v2)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var v3 = TablesContainer.list6.GroupBy(i => i.Number_of_Findings);
                            if (v3 != null)
                            {
                                strN.Add("Number of Findings: ");
                                foreach (var cc in v3)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var v4 = TablesContainer.list6.GroupBy(i => i.Details_of_Findings);
                            if (v4 != null)
                            {
                                strN.Add("Details of Findings: ");
                                foreach (var cc in v4)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var v5 = TablesContainer.list6.GroupBy(i => i.Corrective_Actions);
                            if (v5 != null)
                            {
                                strN.Add("Corrective Actions: ");
                                foreach (var cc in v5)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var v6 = TablesContainer.list6.GroupBy(i => i.Report_Posted);
                            if (v6 != null)
                            {
                                strN.Add("Report Posted: ");
                                foreach (var cc in v6)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var v7 = TablesContainer.list6.GroupBy(i => i.LHIN_Letter_Received);
                            if (v7 != null)
                            {
                                strN.Add("LHIN Letter Received: ");
                                foreach (var cc in v7)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var v8 = TablesContainer.list6.GroupBy(i => i.PH_Letter_Received);
                            if (v8 != null)
                            {
                                strN.Add("PH Letter Received: ");
                                foreach (var cc in v8)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    else
                                    {
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }
                            }

                            b = true; TablesContainer.COUNT = TablesContainer.list6.Count;

                            {
                                ViewBag.Count = TablesContainer.COUNT;
                            }

                            {
                                ViewBag.GN_Found = HomeController.strN;
                            }

                            {
                                ViewBag.Entity = "Visits_Others";
                            }
                            break;
                        #endregion

                        #region Privacy Breaches:   
                        case "Privacy_Breaches":
                            TablesContainer.list7 = db.Privacy_Breaches.ToList();
                            { ViewBag.ObjName = "Visits_Others"; }
                            if (TablesContainer.list7.Count() == 0)
                            {
                                ViewBag.ErrorMsg = errorMsg = "Please select a date range.";
                                WorTabs tabs = new WorTabs();
                                tabs.ListForms = GetFormNames();
                                return View(tabs);
                            }

                            strN = new List<string>();
                            var pb1 = TablesContainer.list7.GroupBy(i => i.Date_Breach_Occurred);
                            if (pb1 != null)
                            {
                                strN.Add("Date Breach Occured: ");
                                foreach (var cc in pb1)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var pb2 = TablesContainer.list7.GroupBy(i => i.Date_Breach_Reported);
                            if (pb2 != null)
                            {
                                strN.Add("Date Breach Reported: ");
                                foreach (var cc in pb2)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var pb3 = TablesContainer.list7.GroupBy(i => i.Date_Breach_Reported_By);
                            if (pb3 != null)
                            {
                                strN.Add("Date Breach Reported By: ");
                                foreach (var cc in pb3)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var pb4 = TablesContainer.list7.GroupBy(i => i.Description_Outcome);
                            if (pb4 != null)
                            {
                                strN.Add("Description Outcome: ");
                                foreach (var cc in pb4)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var pb5 = TablesContainer.list7.GroupBy(i => i.Risk_Level);
                            if (pb5 != null)
                            {
                                strN.Add("Risk Level: ");
                                foreach (var cc in pb5)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var pb6 = TablesContainer.list7.GroupBy(i => i.Number_of_Individuals_Affected);
                            if (pb6 != null)
                            {
                                strN.Add("Number of Individuals Affected: ");
                                foreach (var cc in pb6)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var pb7 = TablesContainer.list7.GroupBy(i => i.Status);
                            if (pb7 != null)
                            {
                                strN.Add("Status: ");
                                foreach (var cc in pb7)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var pb8 = TablesContainer.list7.GroupBy(i => i.Type_of_Breach);
                            if (pb8 != null)
                            {
                                strN.Add("Type of Breach: ");
                                foreach (var cc in pb8)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    else
                                    {
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }
                            }

                            var pb9 = TablesContainer.list7.GroupBy(i => i.Type_of_PHI_Involved);
                            if (pb9 != null)
                            {
                                strN.Add("Type of PHI Involved: ");
                                foreach (var cc in pb9)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    else
                                    {
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }
                            }

                            b = true; TablesContainer.COUNT = TablesContainer.list7.Count;

                            {
                                ViewBag.Count = TablesContainer.COUNT;
                            }

                            {
                                ViewBag.GN_Found = HomeController.strN;
                            }

                            {
                                ViewBag.Entity = "Privacy_Breaches";
                            }
                            break;
                        #endregion

                        #region Privacy Complaints  
                        case "Privacy_Complaints":
                            TablesContainer.list8 = db.Privacy_Complaints.ToList();
                            { ViewBag.ObjName = "Privacy_Complaints"; }
                            if (TablesContainer.list8.Count() == 0)
                            {
                                ViewBag.ErrorMsg = errorMsg = "Please select a date range.";
                                WorTabs tabs = new WorTabs();
                                tabs.ListForms = GetFormNames();
                                return View(tabs);
                            }

                            strN = new List<string>();
                            var pс1 = TablesContainer.list8.GroupBy(i => i.Is_Complaint_Resolved);
                            if (pс1 != null)
                            {
                                strN.Add("Is Complaint Resolved: ");
                                foreach (var cc in pс1)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var pс2 = TablesContainer.list8.GroupBy(i => i.Status);
                            if (pс2 != null)
                            {
                                strN.Add("Status: ");
                                foreach (var cc in pс2)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var pс3 = TablesContainer.list8.GroupBy(i => i.Type_of_Complaint);
                            if (pс3 != null)
                            {
                                strN.Add("Type of Complaint: ");
                                foreach (var cc in pс3)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var pс4 = TablesContainer.list8.GroupBy(i => i.Description_Outcome);
                            if (pс4 != null)
                            {
                                strN.Add("Description Outcome: ");
                                foreach (var cc in pс4)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var pс5 = TablesContainer.list8.GroupBy(i => i.Date_Complain_Received);
                            if (pс5 != null)
                            {
                                strN.Add("Date Complain Received: ");
                                foreach (var cc in pс5)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var pс6 = TablesContainer.list8.GroupBy(i => i.Complain_Filed_By);
                            if (pс6 != null)
                            {
                                strN.Add("Complain Filed By: ");
                                foreach (var cc in pс6)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            b = true; TablesContainer.COUNT = TablesContainer.list8.Count;

                            {
                                ViewBag.Count = TablesContainer.COUNT;
                            }

                            {
                                ViewBag.GN_Found = HomeController.strN;
                            }

                            {
                                ViewBag.Entity = "Privacy_Complaints";
                            }
                            break;
                        #endregion

                        #region Education   
                        case "Education":
                            TablesContainer.list9 = db.Educations.ToList();
                            { ViewBag.ObjName = "Education"; }
                            if (TablesContainer.list9.Count() == 0)
                            {
                                ViewBag.ErrorMsg = errorMsg = "Please select a date range.";
                                WorTabs tabs = new WorTabs();
                                tabs.ListForms = GetFormNames();
                                return View(tabs);
                            }

                            strN = new List<string>();
                            var ed1 = TablesContainer.list9.GroupBy(i => i.Session_Name);
                            if (ed1 != null)
                            {
                                strN.Add("Session Name: ");
                                foreach (var cc in ed1)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var ed2 = TablesContainer.list9.GroupBy(i => i.Jan);
                            if (ed2 != null)
                            {
                                strN.Add("Jan: ");
                                foreach (var cc in ed2)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var ed3 = TablesContainer.list9.GroupBy(i => i.Feb);
                            if (ed3 != null)
                            {
                                strN.Add("Feb: ");
                                foreach (var cc in ed3)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var ed4 = TablesContainer.list9.GroupBy(i => i.Mar);
                            if (ed4 != null)
                            {
                                strN.Add("Mar: ");
                                foreach (var cc in ed4)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var ed5 = TablesContainer.list9.GroupBy(i => i.Apr);
                            if (ed5 != null)
                            {
                                strN.Add("Apr: ");
                                foreach (var cc in ed5)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var ed6 = TablesContainer.list9.GroupBy(i => i.May);
                            if (ed6 != null)
                            {
                                strN.Add("May: ");
                                foreach (var cc in ed6)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var ed7 = TablesContainer.list9.GroupBy(i => i.Jun);
                            if (ed7 != null)
                            {
                                strN.Add("Jun: ");
                                foreach (var cc in ed7)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var ed8 = TablesContainer.list9.GroupBy(i => i.Jul);
                            if (ed8 != null)
                            {
                                strN.Add("Jul: ");
                                foreach (var cc in ed8)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var ed9 = TablesContainer.list9.GroupBy(i => i.Aug);
                            if (ed9 != null)
                            {
                                strN.Add("Aug: ");
                                foreach (var cc in ed9)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var ed10 = TablesContainer.list9.GroupBy(i => i.Sep);
                            if (ed10 != null)
                            {
                                strN.Add("Sep: ");
                                foreach (var cc in ed10)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var ed11 = TablesContainer.list9.GroupBy(i => i.Oct);
                            if (ed11 != null)
                            {
                                strN.Add("Oct: ");
                                foreach (var cc in ed11)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var ed12 = TablesContainer.list9.GroupBy(i => i.Nov);
                            if (ed12 != null)
                            {
                                strN.Add("Nov: ");
                                foreach (var cc in ed12)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var ed13 = TablesContainer.list9.GroupBy(i => i.Oct);
                            if (ed13 != null)
                            {
                                strN.Add("Oct: ");
                                foreach (var cc in ed13)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var ed14 = TablesContainer.list9.GroupBy(i => i.Nov);
                            if (ed14 != null)
                            {
                                strN.Add("Nov: ");
                                foreach (var cc in ed14)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var ed16 = TablesContainer.list9.GroupBy(i => i.Total_Numb_Educ);
                            if (ed16 != null)
                            {
                                strN.Add("Total Numb Educ: ");
                                foreach (var cc in ed16)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var ed17 = TablesContainer.list9.GroupBy(i => i.Total_Numb_Eligible);
                            if (ed17 != null)
                            {
                                strN.Add("Total Numb Eligible: ");
                                foreach (var cc in ed17)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var ed18 = TablesContainer.list9.GroupBy(i => i.Approx_Per_Educated);
                            if (ed18 != null)
                            {
                                strN.Add("Approx Per Educated: ");
                                foreach (var cc in ed18)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            b = true; TablesContainer.COUNT = TablesContainer.list9.Count;

                            {
                                ViewBag.Count = TablesContainer.COUNT;
                            }

                            {
                                ViewBag.GN_Found = HomeController.strN;
                            }

                            {
                                ViewBag.Entity = "Education";
                            }
                            break;
                        #endregion

                        #region Labour_Relations:
                        case "Labour_Relations":
                            TablesContainer.list10 = db.Relations.ToList();
                            { ViewBag.ObjName = "Community_Risks"; }
                            if (TablesContainer.list10.Count() == 0)
                            {
                                ViewBag.ErrorMsg = errorMsg = "Please select a date range.";
                                WorTabs tabs = new WorTabs();
                                tabs.ListForms = GetFormNames();
                                return View(tabs);
                            }

                            strN = new List<string>();
                            var l1 = TablesContainer.list10.GroupBy(i => i.Date);
                            if (l1 != null)
                            {
                                strN.Add("Date: ");
                                foreach (var cc in l1)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var l2 = TablesContainer.list10.GroupBy(i => i.Union);
                            if (l2 != null)
                            {
                                strN.Add("Union: ");
                                foreach (var cc in l2)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var l3 = TablesContainer.list10.GroupBy(i => i.Category);
                            if (l3 != null)
                            {
                                strN.Add("Category: ");
                                foreach (var cc in l3)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var l4 = TablesContainer.list10.GroupBy(i => i.Details);
                            if (l4 != null)
                            {
                                strN.Add("Details: ");
                                foreach (var cc in l4)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var l5 = TablesContainer.list10.GroupBy(i => i.Status);
                            if (l5 != null)
                            {
                                strN.Add("Status: ");
                                foreach (var cc in l5)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var l6 = TablesContainer.list10.GroupBy(i => i.Accruals);
                            if (l6 != null)
                            {
                                strN.Add("Accruals: ");
                                foreach (var cc in l6)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var l7 = TablesContainer.list10.GroupBy(i => i.Outcome);
                            if (l7 != null)
                            {
                                strN.Add("Outcome: ");
                                foreach (var cc in l7)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var l8 = TablesContainer.list10.GroupBy(i => i.Lessons_Learned);
                            if (l8 != null)
                            {
                                strN.Add("Lessons Learned: ");
                                foreach (var cc in l8)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    else
                                    {
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }
                            }

                            b = true; TablesContainer.COUNT = TablesContainer.list10.Count;

                            {
                                ViewBag.Count = TablesContainer.COUNT;
                            }

                            {
                                ViewBag.GN_Found = HomeController.strN;
                            }

                            {
                                ViewBag.Entity = "Labour_Relations";
                            }
                            break;
                        #endregion

                        #region Immunization 
                        case "Immunization":
                            TablesContainer.list11 = db.Immunizations.ToList();
                            //(from ent in db.Immunizations where ent.Location == Id_Location select ent).ToList();
                            { ViewBag.ObjName = "Immunization"; }
                            if (TablesContainer.list11.Count() == 0)
                            {
                                ViewBag.ErrorMsg = errorMsg = "Please select a date range.";
                                WorTabs tabs = new WorTabs();
                                tabs.ListForms = GetFormNames();
                                return View(tabs);
                            }

                            strN = new List<string>();
                            var i1 = TablesContainer.list11.GroupBy(i => i.Numb_Res_Comm);
                            if (i1 != null)
                            {
                                strN.Add("Numb Res Comm: ");
                                foreach (var cc in i1)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var i2 = TablesContainer.list11.GroupBy(i => i.Numb_Res_Immun);
                            if (i2 != null)
                            {
                                strN.Add("Numb Res Immun: ");
                                foreach (var cc in i2)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var i3 = TablesContainer.list11.GroupBy(i => i.Numb_Res_Not_Immun);
                            if (i3 != null)
                            {
                                strN.Add("Numb Res Not Immun: ");
                                foreach (var cc in i3)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var i4 = TablesContainer.list11.GroupBy(i => i.Per_Res_Immun);
                            if (i4 != null)
                            {
                                strN.Add("Per Res Immun: ");
                                foreach (var cc in i4)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var i5 = TablesContainer.list11.GroupBy(i => i.Per_Res_Not_Immun);
                            if (i5 != null)
                            {
                                strN.Add("Per Res Not Immun: ");
                                foreach (var cc in i5)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            b = true; TablesContainer.COUNT = TablesContainer.list11.Count;

                            {
                                ViewBag.Count = TablesContainer.COUNT;
                            }

                            {
                                ViewBag.GN_Found = HomeController.strN;
                            }

                            {
                                ViewBag.Entity = "Immunization";
                            }
                            break;
                        #endregion

                        #region Outbreaks      
                        case "Outbreaks":
                            TablesContainer.list12 = db.Outbreaks.ToList();
                            { ViewBag.ObjName = "Outbreaks"; }
                            if (TablesContainer.list12.Count() == 0)
                            {
                                ViewBag.ErrorMsg = errorMsg = "Please select a date range.";
                                WorTabs tabs = new WorTabs();
                                tabs.ListForms = GetFormNames();
                                return View(tabs);
                            }

                            strN = new List<string>();
                            var w1 = TablesContainer.list12.GroupBy(i => i.CI_Report_Submitted);
                            if (w1 != null)
                            {
                                strN.Add("CI Report Submitted: ");
                                foreach (var cc in w1)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var w2 = TablesContainer.list12.GroupBy(i => i.Credit_for_Lost_Days);
                            if (w2 != null)
                            {
                                strN.Add("Credit for Lost Days: ");
                                foreach (var cc in w2)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var w3 = TablesContainer.list12.GroupBy(i => i.Date_Concluded);
                            if (w3 != null)
                            {
                                strN.Add("Date Concluded: ");
                                foreach (var cc in w3)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var w4 = TablesContainer.list12.GroupBy(i => i.Date_Declared);
                            if (w4 != null)
                            {
                                strN.Add("Date Declared: ");
                                foreach (var cc in w4)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var w5 = TablesContainer.list12.GroupBy(i => i.Deaths_Due);
                            if (w5 != null)
                            {
                                strN.Add("Deaths Due: ");
                                foreach (var cc in w5)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var w6 = TablesContainer.list12.GroupBy(i => i.Docs_Submitted_Finance);
                            if (w6 != null)
                            {
                                strN.Add("Docs Submitted Finance: ");
                                foreach (var cc in w6)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var w7 = TablesContainer.list12.GroupBy(i => i.Notify_MOL);
                            if (w7 != null)
                            {
                                strN.Add("Notify MOL: ");
                                foreach (var cc in w7)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var w8 = TablesContainer.list12.GroupBy(i => i.Strain_Identified);
                            if (w8 != null)
                            {
                                strN.Add("Strain Identified: ");
                                foreach (var cc in w8)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var w9 = TablesContainer.list12.GroupBy(i => i.Total_Days_Closed);
                            if (w9 != null)
                            {
                                strN.Add("Total Days Closed: ");
                                foreach (var cc in w8)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var w10 = TablesContainer.list12.GroupBy(i => i.Total_Residents_Affected);
                            if (w10 != null)
                            {
                                strN.Add("Total Residents Affected: ");
                                foreach (var cc in w10)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var w11 = TablesContainer.list12.GroupBy(i => i.Total_Staff_Affected);
                            if (w11 != null)
                            {
                                strN.Add("Total Staff Affected: ");
                                foreach (var cc in w11)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var w12 = TablesContainer.list12.GroupBy(i => i.Tracking_Sheet_Completed);
                            if (w12 != null)
                            {
                                strN.Add("Tracking Sheet Completed: ");
                                foreach (var cc in w12)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var w13 = TablesContainer.list12.GroupBy(i => i.Type_of_Outbreak);
                            if (w13 != null)
                            {
                                strN.Add("Type of Outbreak: ");
                                foreach (var cc in w13)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            b = true; TablesContainer.COUNT = TablesContainer.list12.Count;

                            {
                                ViewBag.Count = TablesContainer.COUNT;
                            }

                            {
                                ViewBag.GN_Found = HomeController.strN;
                            }

                            {
                                ViewBag.Entity = "Outbreaks";
                            }
                            break;
                        #endregion

                        #region WSIB:
                        case "WSIB":
                            TablesContainer.list13 = db.WSIBs.ToList();
                            { ViewBag.ObjName = "WSIB"; }
                            if (TablesContainer.list13.Count() == 0)
                            {
                                ViewBag.ErrorMsg = errorMsg = "Please select a date range.";
                                WorTabs tabs = new WorTabs();
                                tabs.ListForms = GetFormNames();
                                return View(tabs);
                            }

                            strN = new List<string>();
                            var ww1 = TablesContainer.list13.GroupBy(i => i.Date_Accident);
                            if (ww1 != null)
                            {
                                strN.Add("Date Accident: ");
                                foreach (var cc in ww1)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var ww2 = TablesContainer.list13.GroupBy(i => i.Employee_Initials);
                            if (ww2 != null)
                            {
                                strN.Add("Employee Initials: ");
                                foreach (var cc in ww2)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var ww3 = TablesContainer.list13.GroupBy(i => i.Accident_Cause);
                            if (ww3 != null)
                            {
                                strN.Add("Accident Cause: ");
                                foreach (var cc in ww3)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var ww4 = TablesContainer.list13.GroupBy(i => i.Date_Duties);
                            if (ww4 != null)
                            {
                                strN.Add("Date Duties: ");
                                foreach (var cc in ww4)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var ww5 = TablesContainer.list13.GroupBy(i => i.Date_Regular);
                            if (ww5 != null)
                            {
                                strN.Add("Date Regular: ");
                                foreach (var cc in ww5)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var ww6 = TablesContainer.list13.GroupBy(i => i.Lost_Days);
                            if (ww6 != null)
                            {
                                strN.Add("Lost Days: ");
                                foreach (var cc in ww6)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var ww7 = TablesContainer.list13.GroupBy(i => i.Modified_Days_Not_Shadowed);
                            if (ww7 != null)
                            {
                                strN.Add("Modified Days Not Shadowed: ");
                                foreach (var cc in ww7)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var ww8 = TablesContainer.list13.GroupBy(i => i.Modified_Days_Shadowed);
                            if (ww8 != null)
                            {
                                strN.Add("Modified Days Shadowed: ");
                                foreach (var cc in ww8)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            b = true; TablesContainer.COUNT = TablesContainer.list13.Count;

                            {
                                ViewBag.Count = TablesContainer.COUNT;
                            }

                            {
                                ViewBag.GN_Found = HomeController.strN;
                            }

                            {
                                ViewBag.Entity = "WSIB";
                            }
                            break;
                        #endregion

                        #region Not WSIB    
                        case "Not_WSIBs":
                            TablesContainer.list14 = db.Not_WSIBs.ToList();

                            { ViewBag.ObjName = "Not_WSIB"; }
                            if (TablesContainer.list14.Count() == 0)
                            {
                                ViewBag.ErrorMsg = errorMsg = "Please select a date range.";
                                WorTabs tabs = new WorTabs();
                                tabs.ListForms = GetFormNames();
                                return View(tabs);
                            }

                            strN = new List<string>();
                            var www1 = TablesContainer.list14.GroupBy(i => i.Date_of_Incident);
                            if (www1 != null)
                            {
                                strN.Add("Date of Incident: ");
                                foreach (var cc in www1)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var www2 = TablesContainer.list14.GroupBy(i => i.Employee_Initials);
                            if (www2 != null)
                            {
                                strN.Add("Employee Initials: ");
                                foreach (var cc in www2)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var www3 = TablesContainer.list14.GroupBy(i => i.Position);
                            if (www3 != null)
                            {
                                strN.Add("Position: ");
                                foreach (var cc in www3)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var www4 = TablesContainer.list14.GroupBy(i => i.Time_of_Incident);
                            if (www4 != null)
                            {
                                strN.Add("Time of Incident: ");
                                foreach (var cc in www4)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var www5 = TablesContainer.list14.GroupBy(i => i.Shift);
                            if (www5 != null)
                            {
                                strN.Add("Shift : ");
                                foreach (var cc in www5)
                                {
                                    string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                    if (key == "NULL") continue;
                                    strN.Add($"{key}\t - \t{cc.Count()}");
                                }
                            }

                            var www6 = TablesContainer.list14.GroupBy(i => i.Home_Area);
                            if (www6 != null)
                            {
                                strN.Add("Home Area: ");
                                foreach (var cc in www6)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var www7 = TablesContainer.list14.GroupBy(i => i.Injury_Related);
                            if (www7 != null)
                            {
                                strN.Add("Injury Related: ");
                                foreach (var cc in www7)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var www8 = TablesContainer.list14.GroupBy(i => i.Type_of_Injury);
                            if (www8 != null)
                            {
                                strN.Add("Type of Injury: ");
                                foreach (var cc in www8)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            var www9 = TablesContainer.list14.GroupBy(i => i.Details_of_Incident);
                            if (www9 != null)
                            {
                                strN.Add("Details of Incident: ");
                                foreach (var cc in www9)
                                {
                                    strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                }
                            }

                            b = true; TablesContainer.COUNT = TablesContainer.list14.Count;

                            {
                                ViewBag.Count = TablesContainer.COUNT;
                            }

                            {
                                ViewBag.GN_Found = HomeController.strN;
                            }

                            {
                                ViewBag.Entity = "WSIB";
                            }
                            break;
                            #endregion
                    }
                    #endregion
                }
                #endregion
            }

            #region if you didn't select anything from the list on the left
            else
            {
                ViewBag.ErrorMsg = errorMsg = "Please select a form from the list on the left.";
                WorTabs tabs = new WorTabs();
                tabs.ListForms = GetFormNames();
                return View(tabs);
            }
            #endregion

            return RedirectToAction("../Home/WOR_Tabs");
        }
        #endregion

        public ActionResult GoToListForm(object name)
        {
            var list = TablesContainer.list3;
            return View(list);
        }

        #region Export to CSV:
        [HttpGet]
        public FileResult ExportToCSV()
        {
            if (model_name.Equals("Critical_Incidents"))
            {
                var lst = TablesContainer.list1.ToList<object>();

                string[] names = typeof(Critical_Incidents).GetProperties().Select(property => property.Name).ToArray();


                lst.Insert(0, names.Where(x => x != names[0]).ToArray());

                #region Generate CSV

                StringBuilder sb = new StringBuilder();
                for (var i = 0; i < lst.Count; i++)
                {
                    if (i == 0)
                    {
                        string[] titles = (string[])lst[0];
                        foreach (string data in titles)
                        {
                            //Append data with comma(,) separator.
                            if (data == "Location")
                            {
                                string loc = "Location                   ";
                                sb.Append(loc + ',');
                            }
                            else
                                sb.Append(data + ',');
                        }
                    }
                    else
                    {
                        sb.Append(lst[i]);
                    }
                    //Append new line character.
                    sb.Append("\r\n");
                }

                #endregion

                #region Download CSV

                return File(Encoding.ASCII.GetBytes(sb.ToString()), "text/csv", "Critical_Incidents_NEW.csv");

                #endregion
            }
            else if (model_name.Equals("Good_News"))
            {
                var lst = TablesContainer.list3.ToList<object>();

                string[] names = typeof(Good_News).GetProperties().Select(property => property.Name).ToArray();

                lst.Insert(0, names.Where(x => x != names[0]).ToArray());

                #region Generate CSV

                StringBuilder sb = new StringBuilder();
                for (var i = 0; i < lst.Count; i++)
                {
                    if (i == 0)
                    {
                        string[] arrStudents = (string[])lst[0];
                        foreach (var data in arrStudents)
                        {
                            //Append data with comma(,) separator.
                            sb.Append(data + ',');
                        }
                    }
                    else
                    {
                        sb.Append(lst[i]);
                    }
                    //Append new line character.
                    sb.Append("\r\n");
                }

                #endregion

                #region Download CSV

                return File(Encoding.ASCII.GetBytes(sb.ToString()), "text/csv", "GoodNews_NEW.csv");

                #endregion
            }
            else if (model_name.Equals("Complaint"))
            {
                var lst = TablesContainer.list2.ToList<object>();

                string[] names = typeof(Complaint).GetProperties().Select(property => property.Name).ToArray();

                lst.Insert(0, names.Where(x => x != names[0]).ToArray());

                #region Generate CSV

                StringBuilder sb = new StringBuilder();
                for (var i = 0; i < lst.Count; i++)
                {
                    if (i == 0)
                    {
                        string[] arrStudents = (string[])lst[0];
                        foreach (var data in arrStudents)
                        {
                            //Append data with comma(,) separator.
                            sb.Append(data + ',');
                        }
                    }
                    else
                    {
                        sb.Append(lst[i]);
                    }
                    //Append new line character.
                    sb.Append("\r\n");
                }

                #endregion

                #region Download CSV

                return File(Encoding.ASCII.GetBytes(sb.ToString()), "text/csv", "Complaint.csv");

                #endregion
            }
            else if (model_name.Equals("Community_Risks"))
            {
                var lst = TablesContainer.list5.ToList<object>();

                string[] names = typeof(Community_Risks).GetProperties().Select(property => property.Name).ToArray();

                lst.Insert(0, names.Where(x => x != names[0]).ToArray());

                #region Generate CSV

                StringBuilder sb = new StringBuilder();
                for (var i = 0; i < lst.Count; i++)
                {
                    if (i == 0)
                    {
                        string[] arrStudents = (string[])lst[0];
                        foreach (var data in arrStudents)
                        {
                            //Append data with comma(,) separator.
                            sb.Append(data + ',');
                        }
                    }
                    else
                    {
                        sb.Append(lst[i]);
                    }
                    //Append new line character.
                    sb.Append("\r\n");
                }

                #endregion

                #region Download CSV

                return File(Encoding.ASCII.GetBytes(sb.ToString()), "text/csv", "Community_Risks.csv");

                #endregion
            }
            else if (model_name.Equals("Labour_Relations"))
            {
                var lst = TablesContainer.list10.ToList<object>();

                string[] names = typeof(Labour_Relations).GetProperties().Select(property => property.Name).ToArray();

                lst.Insert(0, names.Where(x => x != names[0]).ToArray());

                #region Generate CSV

                StringBuilder sb = new StringBuilder();
                for (var i = 0; i < lst.Count; i++)
                {
                    if (i == 0)
                    {
                        string[] arrStudents = (string[])lst[0];
                        foreach (var data in arrStudents)
                        {
                            //Append data with comma(,) separator.
                            sb.Append(data + ',');
                        }
                    }
                    else
                    {
                        sb.Append(lst[i]);
                    }
                    //Append new line character.
                    sb.Append("\r\n");
                }

                #endregion

                #region Download CSV

                return File(Encoding.ASCII.GetBytes(sb.ToString()), "text/csv", "Labour_Relations.csv");

                #endregion
            }
            else if (model_name.Equals("Emergency_Prep"))
            {
                var lst = TablesContainer.list4.ToList<object>();

                string[] names = typeof(Emergency_Prep).GetProperties().Select(property => property.Name).ToArray();

                lst.Insert(0, names.Where(x => x != names[0]).ToArray());

                #region Generate CSV

                StringBuilder sb = new StringBuilder();
                for (var i = 0; i < lst.Count; i++)
                {
                    if (i == 0)
                    {
                        string[] arrStudents = (string[])lst[0];
                        foreach (var data in arrStudents)
                        {
                            //Append data with comma(,) separator.
                            sb.Append(data + ',');
                        }
                    }
                    else
                    {
                        sb.Append(lst[i]);
                    }
                    //Append new line character.
                    sb.Append("\r\n");
                }

                #endregion

                #region Download CSV

                return File(Encoding.ASCII.GetBytes(sb.ToString()), "text/csv", "Emergency_Prep.csv");

                #endregion
            }
            else if (model_name.Equals("Visits_Others"))
            {
                var lst = TablesContainer.list6.ToList<object>();

                string[] names = typeof(Visits_Others).GetProperties().Select(property => property.Name).ToArray();

                lst.Insert(0, names.Where(x => x != names[0]).ToArray());

                #region Generate CSV

                StringBuilder sb = new StringBuilder();
                for (var i = 0; i < lst.Count; i++)
                {
                    if (i == 0)
                    {
                        string[] arrStudents = (string[])lst[0];
                        foreach (var data in arrStudents)
                        {
                            //Append data with comma(,) separator.
                            sb.Append(data + ',');
                        }
                    }
                    else
                    {
                        sb.Append(lst[i]);
                    }
                    //Append new line character.
                    sb.Append("\r\n");
                }

                #endregion

                #region Download CSV

                return File(Encoding.ASCII.GetBytes(sb.ToString()), "text/csv", "Visits_Others.csv");

                #endregion
            }
            else if (model_name.Equals("Privacy_Breaches"))
            {
                var lst = TablesContainer.list7.ToList<object>();

                string[] names = typeof(Privacy_Breaches).GetProperties().Select(property => property.Name).ToArray();

                lst.Insert(0, names.Where(x => x != names[0]).ToArray());

                #region Generate CSV

                StringBuilder sb = new StringBuilder();
                for (var i = 0; i < lst.Count; i++)
                {
                    if (i == 0)
                    {
                        string[] arrStudents = (string[])lst[0];
                        foreach (var data in arrStudents)
                        {
                            //Append data with comma(,) separator.
                            sb.Append(data + ',');
                        }
                    }
                    else
                    {
                        sb.Append(lst[i]);
                    }
                    //Append new line character.
                    sb.Append("\r\n");
                }

                #endregion

                #region Download CSV

                return File(Encoding.ASCII.GetBytes(sb.ToString()), "text/csv", "Privacy_Breaches.csv");

                #endregion
            }
            else if (model_name.Equals("Privacy_Complaints"))
            {
                var lst = TablesContainer.list8.ToList<object>();

                string[] names = typeof(Privacy_Complaints).GetProperties().Select(property => property.Name).ToArray();

                lst.Insert(0, names.Where(x => x != names[0]).ToArray());

                #region Generate CSV

                StringBuilder sb = new StringBuilder();
                for (var i = 0; i < lst.Count; i++)
                {
                    if (i == 0)
                    {
                        string[] arrStudents = (string[])lst[0];
                        foreach (var data in arrStudents)
                        {
                            //Append data with comma(,) separator.
                            sb.Append(data + ',');
                        }
                    }
                    else
                    {
                        sb.Append(lst[i]);
                    }
                    //Append new line character.
                    sb.Append("\r\n");
                }

                #endregion

                #region Download CSV

                return File(Encoding.ASCII.GetBytes(sb.ToString()), "text/csv", "Privacy_Complaints.csv");

                #endregion
            }
            else if (model_name.Equals("Education"))
            {
                var lst = TablesContainer.list9.ToList<object>();

                string[] names = typeof(Education).GetProperties().Select(property => property.Name).ToArray();

                lst.Insert(0, names.Where(x => x != names[0]).ToArray());

                #region Generate CSV

                StringBuilder sb = new StringBuilder();
                for (var i = 0; i < lst.Count; i++)
                {
                    if (i == 0)
                    {
                        string[] arrStudents = (string[])lst[0];
                        foreach (var data in arrStudents)
                        {
                            //Append data with comma(,) separator.
                            sb.Append(data + ',');
                        }
                    }
                    else
                    {
                        sb.Append(lst[i]);
                    }
                    //Append new line character.
                    sb.Append("\r\n");
                }

                #endregion

                #region Download CSV

                return File(Encoding.ASCII.GetBytes(sb.ToString()), "text/csv", "Educations.csv");

                #endregion
            }
            else if (model_name.Equals("Labour_Relations"))
            {
                var lst = TablesContainer.list10.ToList<object>();

                string[] names = typeof(Labour_Relations).GetProperties().Select(property => property.Name).ToArray();

                lst.Insert(0, names.Where(x => x != names[0]).ToArray());

                #region Generate CSV

                StringBuilder sb = new StringBuilder();
                for (var i = 0; i < lst.Count; i++)
                {
                    if (i == 0)
                    {
                        string[] arrStudents = (string[])lst[0];
                        foreach (var data in arrStudents)
                        {
                            //Append data with comma(,) separator.
                            sb.Append(data + ',');
                        }
                    }
                    else
                    {
                        sb.Append(lst[i]);
                    }
                    //Append new line character.
                    sb.Append("\r\n");
                }

                #endregion

                #region Download CSV

                return File(Encoding.ASCII.GetBytes(sb.ToString()), "text/csv", "Labour_Relations.csv");

                #endregion
            }
            else if (model_name.Equals("Immunization"))
            {
                var lst = TablesContainer.list11.ToList<object>();

                string[] names = typeof(Immunization).GetProperties().Select(property => property.Name).ToArray();

                lst.Insert(0, names.Where(x => x != names[0]).ToArray());

                #region Generate CSV

                StringBuilder sb = new StringBuilder();
                for (var i = 0; i < lst.Count; i++)
                {
                    if (i == 0)
                    {
                        string[] arrStudents = (string[])lst[0];
                        foreach (var data in arrStudents)
                        {
                            //Append data with comma(,) separator.
                            sb.Append(data + ',');
                        }
                    }
                    else
                    {
                        sb.Append(lst[i]);
                    }
                    //Append new line character.
                    sb.Append("\r\n");
                }

                #endregion

                #region Download CSV

                return File(Encoding.ASCII.GetBytes(sb.ToString()), "text/csv", "Immunizationbb.csv");

                #endregion
            }
            else if (model_name.Equals("Outbreaks"))
            {
                var lst = TablesContainer.list12.ToList<object>();

                string[] names = typeof(Outbreaks).GetProperties().Select(property => property.Name).ToArray();

                lst.Insert(0, names.Where(x => x != names[0]).ToArray());

                #region Generate CSV

                StringBuilder sb = new StringBuilder();
                for (var i = 0; i < lst.Count; i++)
                {
                    if (i == 0)
                    {
                        string[] arrStudents = (string[])lst[0];
                        foreach (var data in arrStudents)
                        {
                            //Append data with comma(,) separator.
                            sb.Append(data + ',');
                        }
                    }
                    else
                    {
                        sb.Append(lst[i]);
                    }
                    //Append new line character.
                    sb.Append("\r\n");
                }

                #endregion

                #region Download CSV

                return File(Encoding.ASCII.GetBytes(sb.ToString()), "text/csv", "Outbreaks.csv");

                #endregion
            }
            else if (model_name.Equals("WSIB"))
            {
                var lst = TablesContainer.list13.ToList<object>();

                string[] names = typeof(WSIB).GetProperties().Select(property => property.Name).ToArray();

                lst.Insert(0, names.Where(x => x != names[0]).ToArray());

                #region Generate CSV

                StringBuilder sb = new StringBuilder();
                for (var i = 0; i < lst.Count; i++)
                {
                    if (i == 0)
                    {
                        string[] arrStudents = (string[])lst[0];
                        foreach (var data in arrStudents)
                        {
                            //Append data with comma(,) separator.
                            sb.Append(data + ',');
                        }
                    }
                    else
                    {
                        sb.Append(lst[i]);
                    }
                    //Append new line character.
                    sb.Append("\r\n");
                }

                #endregion

                #region Download CSV

                return File(Encoding.ASCII.GetBytes(sb.ToString()), "text/csv", "WSIBs.csv");

                #endregion
            }
            else if (model_name.Equals("Not_WSIBs"))
            {
                var lst = TablesContainer.list14.ToList<object>();

                string[] names = typeof(Not_WSIBs).GetProperties().Select(property => property.Name).ToArray();

                lst.Insert(0, names.Where(x => x != names[0]).ToArray());

                #region Generate CSV

                StringBuilder sb = new StringBuilder();
                for (var i = 0; i < lst.Count; i++)
                {
                    if (i == 0)
                    {
                        string[] arrStudents = (string[])lst[0];
                        foreach (var data in arrStudents)
                        {
                            //Append data with comma(,) separator.
                            sb.Append(data + ',');
                        }
                    }
                    else
                    {
                        sb.Append(lst[i]);
                    }
                    //Append new line character.
                    sb.Append("\r\n");
                }

                #endregion

                #region Download CSV

                return File(Encoding.ASCII.GetBytes(sb.ToString()), "text/csv", "Not_WSIBs.csv");

                #endregion
            }
            return null;
        }
        #endregion

        #region All Forms:
        public static SelectList GetFormNames()
        {
            List<WorTabs> forms = new List<WorTabs>()
            {
                new WorTabs{Id=1, Name = "1. Critical Incidents"},
                new WorTabs{Id=2,Name ="2. Complaints"},
                new WorTabs { Id=3,Name = "3. Good News" },
                new WorTabs {Id=4, Name = "4. Emergency Prep" },
                new WorTabs {Id=5, Name = "5. Community Risks or Legal" },
                new WorTabs {Id=6, Name = "6. Visits by Other Agencies" },
                new WorTabs {Id=7, Name = "7a. Privacy Breaches" },
                new WorTabs {Id=8, Name = "7b. Privacy Complaints" },
                new WorTabs {Id=9, Name = "8. Education" },
                new WorTabs {Id=10, Name = "9. Labour Relations" },
                new WorTabs {Id=11, Name = "10. Immunization" },
                new WorTabs {Id=12, Name = "11. Outbreak" },
                new WorTabs {Id=13, Name = "12a. WSIB" },
                new WorTabs {Id=14, Name = "12b. Not WSIB" },
            };
            return new SelectList(forms, "Id", "Name");
        }
        #endregion

        #region Get Table by Id:
        /// <summary>
        /// For Summary handler
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ArrayList GetTableById(int id)
        {
            ArrayList tableList = new ArrayList();
            if (id != 0)
            {
                switch (id)
                {
                    case 1:
                        List<Critical_Incidents> tbl1 = null;
                        if (role == Role.Admin)
                            tbl1 = db.Critical_Incidents.ToList();
                        else
                            tbl1 = db.Critical_Incidents.Where(i => i.Location == Id_Location).ToList();
                        tableList.AddRange(tbl1);
                        break;
                    case 2:
                        List<Complaint> tbl2 = null;
                        if (role == Role.Admin)
                            tbl2 = db.Complaints.ToList();
                        else
                            tbl2 = db.Complaints.Where(i => i.Location == Id_Location).ToList();
                        tableList.AddRange(tbl2);
                        break;
                    case 3:
                        List<Good_News> tbl3 = null;
                        if (role == Role.Admin)
                            tbl3 = db.Good_News.ToList();
                        else
                            tbl3 = db.Good_News.Where(i => i.Location == Id_Location).ToList();
                        tableList.AddRange(tbl3);
                        break;
                    case 4:
                        List<Emergency_Prep> tbl4 = null;
                        if (role == Role.Admin)
                            tbl4 = db.Emergency_Prep.ToList();
                        else tbl4 = db.Emergency_Prep.Where(i => i.Location == Id_Location).ToList();
                        tableList.AddRange(tbl4);
                        break;
                    case 5:
                        List<Community_Risks> tbl5 = null;
                        if (role == Role.Admin)
                            tbl5 = db.Community_Risks.ToList();
                        else tbl5 = db.Community_Risks.Where(i => i.Location == Id_Location).ToList();
                        tableList.AddRange(tbl5);
                        break;
                    case 6:
                        List<Visits_Others> tbl6 = null;
                        if (role == Role.Admin)
                            tbl6 = db.Visits_Others.ToList();
                        else tbl6 = db.Visits_Others.Where(i => i.Location == Id_Location).ToList();
                        tableList.AddRange(tbl6);
                        break;
                    case 7:
                        List<Privacy_Breaches> tbl7 = null;
                        if (role == Role.Admin)
                            tbl7 = db.Privacy_Breaches.ToList();
                        else
                            tbl7 = db.Privacy_Breaches.Where(i => i.Location == Id_Location).ToList();
                        tableList.AddRange(tbl7);
                        break;
                    case 8:
                        List<Privacy_Complaints> tbl8 = null;
                        if (role == Role.Admin)
                            tbl8 = db.Privacy_Complaints.ToList();
                        else tbl8 = db.Privacy_Complaints.Where(i => i.Location == Id_Location).ToList();
                        tableList.AddRange(tbl8);
                        break;
                    case 9:
                        List<Education> tbl9 = null;
                        if (role == Role.Admin)
                            tbl9 = db.Educations.ToList();
                        else tbl9 = db.Educations.Where(i => i.Location == Id_Location).ToList();
                        tableList.AddRange(tbl9);
                        break;
                    case 10:
                        List<Labour_Relations> tbl10 = null;
                        if (role == Role.Admin)
                            tbl10 = db.Relations.ToList();
                        else tbl10 = db.Relations.Where(i => i.Location == Id_Location).ToList();
                        tableList.AddRange(tbl10);
                        break;
                    case 11:
                        List<Immunization> tbl11 = null;
                        if (role == Role.Admin)
                            tbl11 = db.Immunizations.ToList();
                        else tbl11 = db.Immunizations.Where(i => i.Location == Id_Location).ToList();
                        tableList.AddRange(tbl11);
                        break;
                    case 12:
                        List<Outbreaks> tbl12 = null;
                        if (role == Role.Admin)
                            tbl12 = db.Outbreaks.ToList();
                        else tbl12 = db.Outbreaks.Where(i => i.Location == Id_Location).ToList();
                        tableList.AddRange(tbl12);
                        break;
                    case 13:
                        List<WSIB> tbl13 = null;
                        if (role == Role.Admin)
                            tbl13 = db.WSIBs.Where(i => i.Location == Id_Location).ToList();
                        else tbl13 = db.WSIBs.Where(i => i.Location == Id_Location).ToList();
                        tableList.AddRange(tbl13);
                        break;
                    case 14:
                        List<Not_WSIBs> tbl14 = null;
                        if (role == Role.Admin)
                            tbl14 = db.Not_WSIBs.ToList();
                        else tbl14 = db.Not_WSIBs.Where(i => i.Location == Id_Location).ToList();
                        tableList.AddRange(tbl14);
                        break;
                }
            }
            return tableList;
        }

        public ActionResult GoToSelectFormList(string name)
        {
            var arr = name.Split(new char[] { '/' });
            string last = arr.Last();
            if (last != null)
            {
                switch (last)
                {
                    case "1":
                        return RedirectToAction("../Select/Select_Incidents");
                    case "2":
                        return RedirectToAction("../Select/Select_Complaints");
                    case "3":
                        return RedirectToAction("../Select/Select_GoodNews");
                    case "4":
                        return RedirectToAction("../Select/Select_Emergency_Prep");
                    case "5":
                        return RedirectToAction("../Select/Select_Community");
                    case "6":
                        return RedirectToAction("../Select/Select_Visits_Others");
                    case "7":
                        return RedirectToAction("../Select/Privacy_Breaches");
                    case "8":
                        return RedirectToAction("../Select/Privacy_Complaints");
                    case "9":
                        return RedirectToAction("../Select/Education_Select");
                    case "10":
                        return RedirectToAction("../Select/Select_Labour");
                    case "11":
                        return RedirectToAction("../Select/Select_Immunization");
                    case "12":
                        return RedirectToAction("../Select/Outbreaks");
                    case "13":
                        return RedirectToAction("../Select/Select_WSIB");
                    case "14":
                        return RedirectToAction("../Select/Select_Not_WSIB");
                }
            }
            return RedirectToAction("../Home/WOR_Tabs");
        }

        public ActionResult GoToSelectForm(int id)
        {
            if (id != 0)
            {
                switch (id)
                {
                    case 1:
                        return RedirectToAction("../Home/Insert");
                    case 2:
                        return RedirectToAction("../Home/Complaint_Insert");
                    case 3:
                        return RedirectToAction("../Home/GoodNews_Insert");
                    case 4:
                        return RedirectToAction("../Home/Emergency_Prep_Insert");
                    case 5:
                        return RedirectToAction("../Home/Community_Insert");
                    case 6:
                        return RedirectToAction("../Home/Visits_Others");
                    case 7:
                        return RedirectToAction("../Home/Privacy_Breaches");
                    case 8:
                        return RedirectToAction("../Home/Privacy_Complaints");
                    case 9:
                        return RedirectToAction("../Home/Education_Insert");
                    case 10:
                        return RedirectToAction("../Home/Labour_Insert");
                    case 11:
                        return RedirectToAction("../Home/Immunization_Insert");
                    case 12:
                        return RedirectToAction("../Home/Outbreaks");
                    case 13:
                        return RedirectToAction("../Home/WSIB");
                    case 14:
                        return RedirectToAction("../Home/Not_WSIB");
                }
            }
            return RedirectToAction("../Home/WOR_Tabs");
        }
        #endregion

        #region Insert (Critical Incidents):
        [HttpGet]
        public ActionResult Insert()
        {
            object[] objs = new object[] { list, list3, list4, list5 };
            ViewBag.locations = objs;
            return View();
        }

        [HttpPost]
        public ActionResult Insert(Critical_Incidents entity)
        {
            object[] objs = new object[] { list, list3, list4, list5 };
            ViewBag.locations = objs;
            if (entity.Brief_Description == null && entity.Care_Plan_Updated == null && entity.CIS_Initiated == null && entity.CI_Category_Type == 0 &&
               entity.CI_Form_Number == null && entity.Date == null && entity.File_Complete == null && entity.Follow_Up_Amendments == null &&
               entity.Location == 0 && entity.MOHLTC_Follow_Up == null && entity.MOH_Notified == null && entity.POAS_Notified == null && entity.Police_Notified == null &&
               entity.Quality_Improvement_Actions == null && entity.Risk_Locked == null)
            {
                ViewBag.Empty = "All fields have to be filled.";
                return View();
            }
            else if (entity.Brief_Description == null || entity.Care_Plan_Updated == null || entity.CIS_Initiated == null || entity.CI_Category_Type == 0 ||
              entity.CI_Form_Number == null || entity.Date == null || entity.File_Complete == null || entity.Follow_Up_Amendments == null ||
              entity.Location == 0 || entity.MOHLTC_Follow_Up == null || entity.MOH_Notified == null || entity.POAS_Notified == null || entity.Police_Notified == null ||
              entity.Quality_Improvement_Actions == null || entity.Risk_Locked == null)
            {

                try
                {
                    db.Critical_Incidents.Add(entity);
                    db.SaveChanges();
                    //msg_infos = ADO_NET_CRUD.Insert_Incident(entity);
                    return RedirectToAction("../Select/Select_Incidents");
                }
                catch (Exception ex) { return Json("An error has occurred. Error details: " + ex.Message); }
                // ViewBag.Empty = "Some fields are empty. Please fill it out and try again!";
                // return View();
            }
            else
            {
                try
                {
                    db.Critical_Incidents.Add(entity);
                    db.SaveChanges();
                    // msg_infos = ADO_NET_CRUD.Insert_Incident(entity);
                    return RedirectToAction("../Select/Select_Incidents");
                }
                catch (Exception ex) { return HttpNotFound(ex.Message); }
            }
        }
        #endregion

        #region Insert (Labour Insert):
        [HttpGet]
        public ActionResult Labour_Insert()
        {
            ViewBag.locations = list;
            return View();
        }

        [HttpPost]
        public ActionResult Labour_Insert(Labour_Relations entity)
        {
            ViewBag.locations = list;
            if (entity.Accruals == null && entity.Category == null && entity.Date == DateTime.MinValue && entity
                .Details == null && entity.Lessons_Learned == null && entity.Location == 0 && entity.Outcome == null && entity.Status == null &&
                entity.Union == null)
            {
                return View();
            }
            else if (entity.Accruals == null || entity.Category == null || entity.Date == DateTime.MinValue || entity
                          .Details == null || entity.Lessons_Learned == null || entity.Location == 0 || entity.Outcome == null || entity.Status == null ||
                          entity.Union == null)
            {
                db.Relations.Add(entity);
                db.SaveChanges();
                return RedirectToAction("../Select/Select_Labour");
            }
            else
            {
                db.Relations.Add(entity);
                db.SaveChanges();
                return RedirectToAction("../Select/Select_Labour");
            }
        }
        #endregion

        #region Insert (Community_Insert):
        [HttpGet]
        public ActionResult Community_Insert()
        {
            ViewBag.locations = new object[] { list, list17, list4 };
            return View();
        }

        [HttpPost]
        public ActionResult Community_Insert(Community_Risks entity)
        {
            ViewBag.locations = new object[] { list, list17, list4 };
            if (entity.Date == DateTime.MinValue && entity.Descriptions == null && entity.Hot_Alert == null &&
                entity.Location == 0 && entity.MOH_Visit == null && entity.Potential_Risk == null && entity.Resolved == null &&
                entity.Risk_Legal_Action == null && entity.Status_Update == null && entity.Type_Of_Risk == null)
            {
                //ViewBag.Empty = "All fields have to be filled.";
                return View();
            }
            else
            {
                db.Community_Risks.Add(entity);
                db.SaveChanges();
                return RedirectToAction("../Select/Select_Community");
            }
        }
        #endregion

        #region GoodNews Insert:
        [HttpGet]
        public ActionResult GoodNews_Insert()
        {
            ViewBag.locations = new object[] { list, list12, list13, list14, list15, list16 };
            return View();
        }

        [HttpPost]
        public ActionResult GoodNews_Insert(Good_News entity)
        {
            ViewBag.locations = new object[] { list, list12, list13, list14, list15, list16 };
            if (entity.Awards_Details == null && entity.Awards_Received == null && entity.Category == null && entity.Community_Inititives == null &&
                entity.Compliment == null && entity.DateNews == DateTime.MinValue && entity.Department == null && entity.Location == 0 &&
                entity.Description_Complim == null && entity.Growth == false && entity.NameAwards == null && entity.Passion == false &&
                entity.ReceivedFrom == null && entity.Respect == false && entity.Responsibility == false && entity.SourceCompliment == null &&
                entity.Spot_Awards == null && entity.Teamwork == false)
            {
                //ViewBag.Empty = "All fields have to be filled.";
                return View();
            }
            else if ((entity.DateNews == DateTime.MinValue) && entity.Awards_Details == null || entity.Awards_Received == null || entity.Category == null || entity.Community_Inititives == null ||
                entity.Compliment == null || entity.Location != 0 || entity.Department == null ||
                entity.Description_Complim == null || entity.Growth == false || entity.NameAwards == null || entity.Passion == false ||
                entity.ReceivedFrom == null || entity.Respect == false || entity.Responsibility == false || entity.SourceCompliment == null ||
                entity.Spot_Awards == null || entity.Teamwork == false)
            {
                return View();
            }
            else if ((entity.Location != 0) && entity.Awards_Details == null || entity.Awards_Received == null || entity.Category == null || entity.Community_Inititives == null ||
                entity.Compliment == null || entity.DateNews == DateTime.MinValue || entity.Department == null ||
                entity.Description_Complim == null || entity.Growth == false || entity.NameAwards == null || entity.Passion == false ||
                entity.ReceivedFrom == null || entity.Respect == false || entity.Responsibility == false || entity.SourceCompliment == null ||
                entity.Spot_Awards == null || entity.Teamwork == false)
            {
                //if(entity.DateNews == DateTime.MinValue) { entity.DateNews = DateTime.Now; }
                Id_Location = entity.Location;
                db.Good_News.Add(entity);
                int res = db.SaveChanges();
                return RedirectToAction("../Select/Select_GoodNews");
            }
            //if (entity != null)
            //{
            //    id_goodNews = entity.Location;
            //    db.Good_News.Add(entity);
            //    int res = db.SaveChanges();
            //    if (res == 1)
            //        success_nsg = "Your record was successfully saved!";
            //    else
            //        success_nsg = "Somthing went wrong...";
            //    return RedirectToAction("../Select/Select_GoodNews");
            //}
            else
                return View();
        }
        #endregion

        [HttpGet]
        public ActionResult Agency_Insert()
        {
            ViewBag.locations = new object[] { list, list18, list19, list4 };
            return View();
        }

        [HttpPost]
        public ActionResult Agency_Insert(Visits_Agency entity)
        {
            ViewBag.locations = new object[] { list, list18, list19, list4 };
            if (entity.Location == 0 && entity.Agency == null && entity.Corrective_Actions == null && entity.Date_of_Visit == DateTime.Now &&
                entity.Findings_Details == null && entity.Findings_number == null && entity.Report_Posted == null)
            {
                //ViewBag.Empty = "All fields have to be filled.";
                return View();
            }
            else
            {
                db.Visits_Agencies.Add(entity);
                db.SaveChanges();
                return RedirectToAction("../Select/Select_Agencies");
            }
        }

        [HttpGet]
        public ActionResult WSIB()
        {
            ViewBag.locations = list;
            return View();
        }

        [HttpPost]
        public ActionResult WSIB(WSIB entity)
        {
            ViewBag.locations = list;
            if (entity.Accident_Cause == null && entity.Date_Accident == DateTime.MinValue && entity.Date_Duties == DateTime.MinValue &&
                entity.Date_Regular == DateTime.MinValue && entity.Employee_Initials == null && entity.Form_7 == null &&
                entity.Location == 0 && entity.Lost_Days == 0 && entity.Modified_Days_Not_Shadowed == 0 && entity.Modified_Days_Shadowed == 0)
            {
                //ViewBag.Empty = "All fields have to be filled.";
                return View();
            }
            else if ((entity.Location != 0) || entity.Accident_Cause == null || entity.Date_Accident == null || entity.Date_Duties == null ||
                entity.Date_Regular == null || entity.Employee_Initials == null || entity.Form_7 == null ||
                entity.Lost_Days == 0 || entity.Modified_Days_Not_Shadowed == 0 || entity.Modified_Days_Shadowed == 0)
            {
                db.WSIBs.Add(entity);
                db.SaveChanges();
                return RedirectToAction("../Select/Select_WSIB");
            }

            return View();
        }

        [HttpGet]
        public ActionResult Not_WSIB()
        {
            ViewBag.locations = list;
            return View();
        }

        [HttpPost]
        public ActionResult Not_WSIB(Not_WSIBs entity)
        {
            ViewBag.locations = list;
            if (entity.Location == 0 && entity.Date_of_Incident == DateTime.MinValue && entity.Details_of_Incident == null && entity.Employee_Initials == null &&
                entity.Home_Area == null && entity.Injury_Related == null && entity.Location == 0 && entity.Position == null &&
                entity.Shift == null && entity.Time_of_Incident == null && entity.Type_of_Injury == null)
            {
                //ViewBag.Empty = "All fields have to be filled.";
                return View();
            }
            else if ((entity.Location != 0) && entity.Date_of_Incident == DateTime.MinValue || entity.Details_of_Incident == null || entity.Employee_Initials == null ||
                entity.Home_Area == null || entity.Injury_Related == null || entity.Location == 0 || entity.Position == null ||
                entity.Shift == null || entity.Time_of_Incident == null || entity.Type_of_Injury == null)
            {
                db.Not_WSIBs.Add(entity);
                db.SaveChanges();
                return RedirectToAction("../Select/Select_Not_WSIB");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Care_Community()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Care_Community(Care_Community entity)
        {
            ViewBag.locations = list;
            if (entity.Name == null)
            {
                //ViewBag.Empty = "All fields have to be filled.";
                return View();
            }
            else
            {
                db.Care_Communities.Add(entity);
                db.SaveChanges();
                return RedirectToAction("../Home/Index");
            }
        }

        [HttpGet]
        public ActionResult Add_Position()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add_Position(Position entity)
        {
            ViewBag.locations = list;
            if (entity.Name == null)
            {
                //ViewBag.Empty = "All fields have to be filled.";
                return View();
            }
            else
            {
                db.Positions.Add(entity);
                db.SaveChanges();
                return RedirectToAction("../Home/Index");
            }
        }

        [HttpGet]
        public ActionResult Visits_Others()
        {
            ViewBag.locations = new object[] { list, list18, list19, list4 };
            return View();
        }

        [HttpPost]
        public ActionResult Visits_Others(Visits_Others entity)
        {
            ViewBag.locations = new object[] { list, list18, list19, list4 };
            if (entity.Agency == null && entity.Corrective_Actions == null && entity.Date_of_Visit == DateTime.MinValue &&
                entity.Details_of_Findings == null && entity.LHIN_Letter_Received == null && entity.Location == 0 &&
                entity.Number_of_Findings == null && entity.PH_Letter_Received == null && entity.Report_Posted == null)
            {
                //ViewBag.Empty = "All fields have to be filled.";
                return View();
            }
            else if ((entity.Date_of_Visit != DateTime.MinValue &&
                      entity.Details_of_Findings != null && entity.Location != 0 &&
                      entity.Number_of_Findings != null)
                      && (entity.Agency == null || entity.Corrective_Actions == null || entity.LHIN_Letter_Received == null ||
                      entity.PH_Letter_Received == null || entity.Report_Posted == null))
            {
                db.Visits_Others.Add(entity);
                db.SaveChanges();
                return RedirectToAction("../Select/Select_Visits_Others");
            }
            return View();
        }

        public ActionResult Education()
        {
            ViewBag.locations = list;
            return View();
        }

        [HttpPost]
        public ActionResult Education(Education entity)
        {
            ViewBag.locations = list;
            if (entity.Approx_Per_Educated == 0 && entity.Apr == 0 && entity.Aug == 0 && entity.Dec == 0 && entity.Feb == 0 && entity.Jan == 0 &&
                entity.Jul == 0 && entity.Jun == 0 && entity.Location == 0 && entity.Mar == 0 && entity.May == 0 && entity.Nov == 0 && entity.Oct == 0 &&
                entity.Sep == 0 && entity.Session_Name == null && entity.Total_Numb_Educ == 0 && entity.Total_Numb_Eligible == 0)
            {
                //ViewBag.Empty = "All fields have to be filled.";
                return View();
            }
            else if ((entity.Location != 0) && entity.Approx_Per_Educated == 0 || entity.Apr == 0 || entity.Aug == 0 || entity.Dec == 0 || entity.Feb == 0 || entity.Jan == 0 ||
                entity.Jul == 0 || entity.Jun == 0 || entity.Location == 0 || entity.Mar == 0 || entity.May == 0 || entity.Nov == 0 || entity.Oct == 0 ||
                entity.Sep == 0 || entity.Session_Name == null || entity.Total_Numb_Educ == 0 || entity.Total_Numb_Eligible == 0)
            {
                db.Educations.Add(entity);
                db.SaveChanges();
                return RedirectToAction("../Select/Education_Select");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Outbreaks()
        {
            ViewBag.locations = list;
            return View();
        }

        [HttpPost]
        public ActionResult Outbreaks(Outbreaks entity)
        {
            ViewBag.locations = list;
            if (entity.CI_Report_Submitted == null && entity.Credit_for_Lost_Days == 0 && entity.Date_Concluded == DateTime.MinValue &&
                entity.Date_Declared == DateTime.MinValue && entity.Deaths_Due == 0 && entity.Docs_Submitted_Finance == null &&
                entity.Location == 0 && entity.Notify_MOL == null && entity.Strain_Identified == null && entity.Total_Days_Closed == 0 &&
                entity.Total_Residents_Affected == 0 && entity.Total_Staff_Affected == 0 && entity.Tracking_Sheet_Completed == null &&
                entity.Type_of_Outbreak == null)
            {
                //ViewBag.Empty = "All fields have to be filled.";
                return View();
            }
            else if ((entity.Location != 0) && entity.CI_Report_Submitted == null || entity.Credit_for_Lost_Days == 0 || entity.Date_Concluded == null ||
                entity.Date_Declared == null || entity.Deaths_Due == 0 || entity.Docs_Submitted_Finance == null ||
                entity.Notify_MOL == null || entity.Strain_Identified == null || entity.Total_Days_Closed == 0 ||
                entity.Total_Residents_Affected == 0 || entity.Total_Staff_Affected == 0 || entity.Tracking_Sheet_Completed == null ||
                entity.Type_of_Outbreak == null)
            {
                //entity.Date_Concluded = entity.Date_Declared = DateTime.MinValue;
                db.Outbreaks.Add(entity);
                db.SaveChanges();
                return RedirectToAction("../Select/Outbreaks");
            }
            return View();
        }

        public ActionResult Immunization_Insert()
        {
            ViewBag.locations = list;
            return View();
        }

        [HttpPost]
        public ActionResult Immunization_Insert(Immunization entity)
        {
            ViewBag.locations = list;
            if (entity.Location == 0 && entity.Numb_Res_Comm == null && entity.Numb_Res_Immun == null && entity.Numb_Res_Not_Immun == null &&
                entity.Per_Res_Immun == null && entity.Per_Res_Not_Immun == null)
                return View();
            else if ((entity.Location != 0) && entity.Numb_Res_Comm == null || entity.Numb_Res_Immun == null || entity.Numb_Res_Not_Immun == null ||
                entity.Per_Res_Immun == null || entity.Per_Res_Not_Immun == null)
            {
                db.Immunizations.Add(entity);
                db.SaveChanges();
                return RedirectToAction("../Select/Select_Immunization");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Privacy_Breaches()
        {
            ViewBag.locations = list;
            return View();
        }

        [HttpPost]
        public ActionResult Privacy_Breaches(Privacy_Breaches entity)
        {
            ViewBag.locations = list;
            if (entity.Date_Breach_Reported == DateTime.MinValue &&
                entity.Location == 0 &&
                entity.Number_of_Individuals_Affected == 0 &&
                entity.Risk_Level == null &&
                entity.Status == null &&
                entity.Type_of_Breach == null &&
                entity.Type_of_PHI_Involved == null &&
                entity.Date_Breach_Occurred == DateTime.MinValue &&
                entity.Date_Breach_Reported_By == null &&
                entity.Description_Outcome == null
                ) return View();
            else if ((entity.Location != 0 && entity.Date_Breach_Occurred != DateTime.MinValue) &&
                      entity.Number_of_Individuals_Affected == 0 ||
                entity.Risk_Level == null ||
                entity.Status == null ||
                entity.Type_of_Breach == null ||
                entity.Type_of_PHI_Involved == null ||
                entity.Date_Breach_Occurred == DateTime.MinValue ||
                entity.Date_Breach_Reported_By == null ||
                entity.Description_Outcome == null)
            {
                db.Privacy_Breaches.Add(entity);
                db.SaveChanges();
                return RedirectToAction("../Select/Privacy_Breaches");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Privacy_Complaints()
        {
            ViewBag.locations = list;
            return View();
        }

        [HttpPost]
        public ActionResult Privacy_Complaints(Privacy_Complaints entity)
        {

            ViewBag.locations = list;
            if (entity.Complain_Filed_By == null && entity.Date_Complain_Received == DateTime.MinValue && entity.Description_Outcome == null &&
                entity.Is_Complaint_Resolved == null && entity.Location == 0 && entity.Status == null && entity.Type_of_Complaint == null)
            {
                //ViewBag.Empty = "All fields have to be filled.";
                return View();
            }
            else if ((entity.Location != 0) && entity.Complain_Filed_By == null || entity.Date_Complain_Received == DateTime.MinValue || entity.Description_Outcome == null ||
                entity.Is_Complaint_Resolved == null || entity.Status == null || entity.Type_of_Complaint == null)
            {
                db.Privacy_Complaints.Add(entity);
                db.SaveChanges();
                return RedirectToAction("../Select/Privacy_Complaints");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Education_Insert()
        {
            ViewBag.locations = list;
            return View();
        }

        [HttpPost]
        public ActionResult Education_Insert(Education entity)
        {
            ViewBag.locations = list;
            if (entity.Apr == 0 && entity.Aug == 0 && entity.Dec == 0 && entity.Feb == 0 && entity.Jan == 0 &&
                entity.Jul == 0 && entity.Jun == 0 && entity.Location == 0 && entity.Mar == 0 && entity.May == 0 &&
                entity.Session_Name == null && entity.Nov == 0 && entity.Oct == 0 && entity.Sep == 0 && entity.Total_Numb_Educ == 0 &&
                entity.Total_Numb_Eligible == 0 && entity.Approx_Per_Educated == 0)
            {
                //ViewBag.Empty = "All fields have to be filled.";
                return View();
            }
            else
            {
                db.Educations.Add(entity);
                db.SaveChanges();
                return RedirectToAction("../Select/Education_Select");
            }
        }

        [HttpGet]
        public ActionResult Emergency_Prep_Insert()
        {
            ViewBag.locations = list;
            return View();
        }

        [HttpPost]
        public ActionResult Emergency_Prep_Insert(Emergency_Prep entity)
        {
            ViewBag.locations = list;
            if (entity.Apr == null && entity.Aug == null && entity.Dec == null && entity.Feb == null && entity.Jan == null &&
                entity.Jul == null && entity.Jun == null && entity.Location == 0 && entity.Mar == null && entity.May == null &&
                entity.Name == null && entity.Nov == null && entity.Oct == null && entity.Sep == null)
            {
                //ViewBag.Empty = "All fields have to be filled.";
                return View();
            }
            else
            {
                db.Emergency_Prep.Add(entity);
                db.SaveChanges();
                return RedirectToAction("../Select/Select_Emergency_Prep");
            }
        }
    }

    public class WorTabs
    {
        public int Id { get; set; }
        public string SearchBy { get; set; }
        public string Name { get; set; }
        [Required(ErrorMessage = "This field is required! Please fill it in.")]
        [DataType(DataType.Date)]
        public DateTime Start { get; set; }
        [Required(ErrorMessage = "This field is required! Please fill it in.")]
        [DataType(DataType.Date)]
        public DateTime End { get; set; }
        public SelectList ListForms { get; set; }
        public string Filter { get; set; }
        // For Radio:
        public string WithRadio { get; set; }
        public string WithoutRadio { get; set; }
        public string FilterRadio { get; set; }

        // For All Forms:
        public string Param1 { get; set; }
    }
}