using DTS.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DTS.Controllers
{
    public enum Role { Admin, User}

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
        List<Position> positions;
        public static SelectList list2, list, list3, list4, list5, list6, list7, list8, list9, list10, list11, list12, list13, list14, list15, list16, list17, list18, list19; //needed for front end drop down list
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
        }
        #endregion

        public ActionResult Complaint_Insert()
        {
            object[] objs = new object[] { list, list3, list4, list5, list6, list7, list8, list9, list10, list11 };
            ViewBag.locations = objs;
            return View();
        }

        #region Complaints(Insert):
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

        [HttpGet]
        public ActionResult SignIN()
        {
            return View();
        }

        static Role role;
        [HttpPost]
        public ActionResult SignIN(Users user)
        {
            string login = user.Login;
            string password = user.Password;
            List<Users> users = db.Users.ToList(); //takes all users from users table in a db and packs it into list users
            bool flag = false;
            for (int i = 0; i < users.Count(); i++)
            {
                if (users[i].Login == login && users[i].Password == password && users[i].Role.Equals("User"))
                {
                    role = Role.User;
                    flag = true;
                    return RedirectToAction("../Home/Index");
                }
                else if (users[i].Login == login && users[i].Password == password && users[i].Role.Equals("Admin"))
                {
                    role = Role.Admin;
                    flag = true;
                    return RedirectToAction("../Home/WOR_Tabs");
                }
            }
            if (!flag) ViewBag.incorrect = "Incorrect Login or Password...Please try again!";
            return View();
        }

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
                    
                        file.SaveAs(fname);
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
        #endregion

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

        public static int num_tbl;
        public static string checkView = "none";
        public static bool b = false;
        [HttpGet]
        public ActionResult WOR_Tabs()
        {
            WorTabs tabs = null;
            { ViewBag.Names = STREAM.GetLocNames().ToArray(); }
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
                            ViewBag.GN_Found = strN;
                        }

                        {
                            ViewBag.ObjName = "Critical Incidents";
                        }

                        {
                            ViewBag.Entity = "Critical_Incidents";
                        }

                        {
                            ViewBag.LocInfo = db.Care_Communities.Find(Id_Location).Name;
                        }
                        ViewBag.Check = checkView;
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
                        {
                            ViewBag.LocInfo = db.Care_Communities.Find(Id_Location).Name;
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
                            ViewBag.LocInfo = db.Care_Communities.Find(Id_Location).Name;
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
                            ViewBag.LocInfo = db.Emergency_Prep.Find(Id_Location).Name;
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
                            ViewBag.LocInfo = db.Care_Communities.Find(Id_Location).Name;
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
                            ViewBag.LocInfo = db.Care_Communities.Find(Id_Location).Name;
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
                            ViewBag.LocInfo = db.Care_Communities.Find(Id_Location).Name;
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
                            ViewBag.LocInfo = db.Care_Communities.Find(Id_Location).Name;
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
                            ViewBag.LocInfo = db.Care_Communities.Find(Id_Location).Name;
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

        static string model_name;
        [HttpPost]
        public ActionResult WOR_Tabs(WorTabs Value)
        {
            DateTime start = DateTime.MinValue, end = DateTime.MinValue;
            string errorMsg = string.Empty;
            if (Value != null && Value.Name != null)  // If we select anythng from the listbox
            {
                string btnName = Request.Params
                      .Cast<string>()
                      .Where(p => p.StartsWith("btn"))
                      .Select(p => p.Substring("btn".Length))
                      .First();

                #region For Showing List:
                if (btnName.Equals("-list"))
                {
                    {
                        ViewBag.Check = "list";
                        //ViewBag.Tbl = Value.Name;
                        //ViewBag.List = db.Complaints.ToList();
                        //WorTabs tabs = new WorTabs();
                        //tabs.ListForms = GetFormNames();
                        //return View(tabs);
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
                                    var lst1 = db.Critical_Incidents;
                                    TablesContainer.list1 = (from ent in lst1 where ent.Date >= start && ent.Date <= end select ent).ToList();
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
                                    var lst2 = db.Complaints;
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
                                    var lst3 = db.Good_News.ToList();
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
                                    var lst1 = db.Critical_Incidents.Where(i => i.Location == Id_Location);
                                    TablesContainer.list1 = (from ent in lst1 where ent.Date >= start && ent.Date <= end select ent).ToList();
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
                                    var lst = db.Critical_Incidents.Where(i => i.Location == Id_Location);
                                    TablesContainer.list1 = (from ent in lst where ent.Date >= start && ent.Date <= end select ent).ToList();
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
                                    if(var == null)
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
                    //return GoToSelectFormList($"../Home/GoToSelectFormList/{name}");
                }
                #endregion

                #region For Inserted:
                else if (btnName.Equals("-insert"))
                {
                    checkView = "insert";
                    ViewBag.Check = checkView;
                    int id = int.Parse(Value.Name);
                    return RedirectToAction($"../Home/GoToSelectForm/{id}");
                }
                #endregion

                #region For Export to .csv file
                else if (btnName.Equals("-export"))
                 {
                    start = Value.Start;
                    end = Value.End;
                    if (start != DateTime.MinValue && end != DateTime.MinValue)
                    {
                        //var query1 = (from ent in db.Good_News where ent.DateNews >= start && ent.DateNews <= end select ent).ToList();
                        int id = int.Parse(Value.Name);
                        var tbl_list = GetTableById(id).ToArray().ToList();
                        Type type = tbl_list[0].GetType();
                        string entity = type.Name;
                        object model = Searcher.FindObjByName(entity);
                        if (model.GetType() == typeof(Critical_Incidents))
                        {
                            model_name = model.GetType().Name;
                            var lst1 = db.Critical_Incidents.Where(i => i.Location == Id_Location);
                            TablesContainer.list1 = (from ent in lst1 where ent.Date >= start && ent.Date <= end select ent).ToList();
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

                #region For Summary:
                else if (btnName.Equals("-summary"))
                {
                    checkView = "summary";
                    ViewBag.Check = checkView;
                    start = Value.Start;
                    end = Value.End;
                    int id = num_tbl = int.Parse(Value.Name);
                    var tbl_list = GetTableById(id).ToArray().ToList();
                    Type type = tbl_list[0].GetType();
                    string entity = type.Name;
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
                                var lst_locat1 = db.Critical_Incidents.Where(i => i.Location == Id_Location).ToList();
                                TablesContainer.list1 = (from ent in lst_locat1 where ent.Date >= start && ent.Date <= end select ent).ToList();
                                if (TablesContainer.list1.Count() == 0)
                                {
                                    { ViewBag.ObjName = entity; }
                                    ViewBag.ErrorMsg = errorMsg = "Please select a date range.";
                                    WorTabs tabs = new WorTabs();
                                    tabs.ListForms = GetFormNames();
                                    return View(tabs);
                                }
                                TablesContainer.COUNT = TablesContainer.list1.Count;
                                strN = new List<string>();

                                var attr11 = TablesContainer.list1.GroupBy(i => i.Date);
                                if (attr11 != null)
                                {
                                    strN.Add("Date: ");
                                    foreach (var cc in attr11)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var attr10 = TablesContainer.list1.GroupBy(i => i.CI_Form_Number);
                                if (attr10 != null)
                                {
                                    strN.Add("CI Form Number: ");
                                    foreach (var cc in attr10)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key;
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var attr7 = TablesContainer.list1.GroupBy(i => i.Brief_Description);
                                if (attr7 != null)
                                {
                                    strN.Add("Brief Description: ");
                                    foreach (var cc in attr7)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key;
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var attr2 = TablesContainer.list1.GroupBy(i => i.MOH_Notified);
                                if (attr2 != null)
                                {
                                    strN.Add("MOH Notified: ");
                                    foreach (var d in attr2)
                                    {
                                        string key = d.Key == null ? "NULL" : d.Key;
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{d.Count()}");
                                    }
                                }

                                var attr4 = TablesContainer.list1.GroupBy(i => i.Police_Notified);
                                if (attr4 != null)
                                {
                                    strN.Add("Police Notified: ");
                                    foreach (var cc in attr4)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key;
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }


                                var attr3 = TablesContainer.list1.GroupBy(i => i.POAS_Notified);
                                if (attr3 != null)
                                {
                                    strN.Add("POAS Notified: ");
                                    foreach (var cc in attr3)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key;
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var attr8 = TablesContainer.list1.GroupBy(i => i.Care_Plan_Updated);
                                if (attr8 != null)
                                {
                                    strN.Add("Care Plan Updated: ");
                                    foreach (var cc in attr8)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key;
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var attr5 = TablesContainer.list1.GroupBy(i => i.Quality_Improvement_Actions);
                                if (attr5 != null)
                                {
                                    strN.Add("Quality Improvement Actions: ");
                                    foreach (var cc in attr5)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key;
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var attr1 = TablesContainer.list1.GroupBy(i => i.MOHLTC_Follow_Up);
                                if (attr1 != null)
                                {
                                    strN.Add("MOHLTC Follow Up: ");
                                    foreach (var e in attr1)
                                    {
                                        string key = e.Key == null ? "NULL" : e.Key;
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{e.Count()}");
                                    }
                                }

                                var attr9 = TablesContainer.list1.GroupBy(i => i.CIS_Initiated);
                                if (attr9 != null)
                                {
                                    strN.Add("CIS Initiated: ");
                                    foreach (var cc in attr9)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key;
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var attr13 = TablesContainer.list1.GroupBy(i => i.Follow_Up_Amendments);
                                if (attr13 != null)
                                {
                                    strN.Add("Follow Up Amendments: ");
                                    foreach (var cc in attr13)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key;
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var attr6 = TablesContainer.list1.GroupBy(i => i.Risk_Locked);
                                if (attr6 != null)
                                {
                                    strN.Add("Risk Locked: ");
                                    foreach (var cc in attr6)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key;
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var attr12 = TablesContainer.list1.GroupBy(i => i.File_Complete);
                                if (attr12 != null)
                                {
                                    strN.Add("File Complete: ");
                                    foreach (var cc in attr12)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key;
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                b = true;

                                {
                                    ViewBag.Count = TablesContainer.COUNT;
                                }

                                {
                                    ViewBag.GN_Found = HomeController.strN;
                                }
                                {
                                    ViewBag.Entity = "Critical_Incidents";
                                }

                                {
                                    ViewBag.LocInfo = db.Care_Communities.Find(Id_Location).Name;
                                }
                                //var ci = db.CI_Category_Types.ToList();

                                //var names = new List<string>();
                                //foreach (var n in ci)
                                //    names.Add(n.Name);

                                //counts = new int[ci.Count]; // for each parameter CI_Category_Type set up count

                                //for(int i = 0; i < counts.Length; i++)
                                //    counts[i] = 0;

                                //var g = TablesContainer.list1.GroupBy(i => i.CI_Category_Type);

                                //strs = new List<string>();
                                //foreach (var group in g)
                                //{
                                //    strs.Add($"{names[group.Key - 1]}\t-\t{group.Count()}" );
                                //}

                                //ViewBag.Entity = entity;

                                #region Count of all found records:
                                //foreach (var i in TablesContainer.list1)
                                //{
                                //    if (i.Brief_Description != null) TablesContainer.c1++;
                                //    if (i.Care_Plan_Updated != null) TablesContainer.c2++;
                                //    if (i.CIS_Initiated != null) TablesContainer.c3++;
                                //    if (i.CI_Category_Type != 0) TablesContainer.c4++;
                                //    if (i.CI_Form_Number != null) TablesContainer.c5++;
                                //    if (i.Date != DateTime.MinValue) TablesContainer.c6++;
                                //    if (i.File_Complete != null) TablesContainer.c7++;
                                //    if (i.Follow_Up_Amendments != null) TablesContainer.c8++;
                                //    if (i.Location != 0) TablesContainer.c9++;
                                //    if (i.MOHLTC_Follow_Up != null) TablesContainer.c10++;
                                //    if (i.MOH_Notified != null) TablesContainer.c11++;
                                //    if (i.POAS_Notified != null) TablesContainer.c12++;
                                //    if (i.Police_Notified != null) TablesContainer.c13++;
                                //    if (i.Quality_Improvement_Actions != null) TablesContainer.c14++;
                                //    if (i.Risk_Locked != null) TablesContainer.c15++;
                                //}

                                //TablesContainer.count_arr.AddRange(new int[] {
                                //TablesContainer.c1++,TablesContainer.c2++,TablesContainer.c3++,TablesContainer.c4++,TablesContainer.c5++,
                                //TablesContainer.c6++,TablesContainer.c7++,TablesContainer.c8++,TablesContainer.c9++,TablesContainer.c10++,
                                //TablesContainer.c11++,TablesContainer.c12++,TablesContainer.c13++,TablesContainer.c14++,TablesContainer.c15++
                                //});
                                #endregion

                                break;
                            #endregion

                            #region Complaints:
                            case "Complaint":
                                var lst_locat2 = db.Complaints.Where(i => i.Location == Id_Location).ToList();
                                TablesContainer.list2 = (from ent in lst_locat2 where ent.DateReceived >= start && ent.DateReceived <= end select ent).ToList();
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
                                    strN.Add("Date Received: ");
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
                                    strN.Add("Writen Or Verbal: ");
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
                                    strN.Add("Receive Directly: ");
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
                                    strN.Add("From Resident: ");
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
                                    strN.Add("Resident Name: ");
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
                                    strN.Add("Department: ");
                                    foreach (var cc in att6)
                                    {
                                        string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        if (key == "NULL") continue;
                                        strN.Add($"{key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att7 = TablesContainer.list2.GroupBy(i => i.BriefDescription);
                                if (att7 != null)
                                {
                                    strN.Add("Brief Description: ");
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
                                    strN.Add("Is Administration: ");
                                    foreach (var cc in att8)
                                    {
                                        //string key = cc.Key == null ? "NULL" : cc.Key.ToString();
                                        //if (key == "NULL") continue;
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att9 = TablesContainer.list2.GroupBy(i => i.CareServices);
                                if (att9 != null)
                                {
                                    strN.Add("Care Services: ");
                                    foreach (var cc in att9)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att10 = TablesContainer.list2.GroupBy(i => i.PalliativeCare);
                                if (att10 != null)
                                {
                                    strN.Add("Palliative Care: ");
                                    foreach (var cc in att10)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att11 = TablesContainer.list2.GroupBy(i => i.Dietary);
                                if (att11 != null)
                                {
                                    strN.Add("Dietary: ");
                                    foreach (var cc in att11)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att12 = TablesContainer.list2.GroupBy(i => i.Housekeeping);
                                if (att12 != null)
                                {
                                    strN.Add("Housekeeping: ");
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
                                    strN.Add("Laundry: ");
                                    foreach (var cc in att13)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att14 = TablesContainer.list2.GroupBy(i => i.Maintenance);
                                if (att14 != null)
                                {
                                    strN.Add("Maintenance: ");
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
                                    strN.Add("Physician: ");
                                    foreach (var cc in att16)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att17 = TablesContainer.list2.GroupBy(i => i.Beautician);
                                if (att17 != null)
                                {
                                    strN.Add("Beautician: ");
                                    foreach (var cc in att17)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att18 = TablesContainer.list2.GroupBy(i => i.FootCare);
                                if (att18 != null)
                                {
                                    strN.Add("Foot Care: ");
                                    foreach (var cc in att18)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att19 = TablesContainer.list2.GroupBy(i => i.DentalCare);
                                if (att19 != null)
                                {
                                    strN.Add("Dental Care: ");
                                    foreach (var cc in att19)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att20 = TablesContainer.list2.GroupBy(i => i.Physio);
                                if (att20 != null)
                                {
                                    strN.Add("Physio: ");
                                    foreach (var cc in att20)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att21 = TablesContainer.list2.GroupBy(i => i.Other);
                                if (att21 != null)
                                {
                                    strN.Add("Other: ");
                                    foreach (var cc in att21)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att22 = TablesContainer.list2.GroupBy(i => i.MOHLTCNotified);
                                if (att22 != null)
                                {
                                    strN.Add("MOHLTC Notified: ");
                                    foreach (var cc in att22)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att23 = TablesContainer.list2.GroupBy(i => i.CopyToVP);
                                if (att23 != null)
                                {
                                    strN.Add("Copy To VP: ");
                                    foreach (var cc in att23)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att24 = TablesContainer.list2.GroupBy(i => i.ResponseSent);
                                if (att15 != null)
                                {
                                    strN.Add("Response Sent: ");
                                    foreach (var cc in att15)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att25 = TablesContainer.list2.GroupBy(i => i.ActionToken);
                                if (att25 != null)
                                {
                                    strN.Add("Action Token: ");
                                    foreach (var cc in att25)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att26 = TablesContainer.list2.GroupBy(i => i.Resolved);
                                if (att26 != null)
                                {
                                    strN.Add("Resolved: ");
                                    foreach (var cc in att26)
                                    {
                                        strN.Add($"{cc.Key}\t - \t{cc.Count()}");
                                    }
                                }

                                var att27 = TablesContainer.list2.GroupBy(i => i.MinistryVisit);
                                if (att27 != null)
                                {
                                    strN.Add("Ministry Visit: ");
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
                                    strN.Add("Department: ");
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
                                    strN.Add("Resolved: ");
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

                            #region Privacy Breaches:   --none
                            case "Privacy_Breaches":
                                TablesContainer.list7 = (from ent in db.Privacy_Breaches where ent.Date_Breach_Reported >= start && ent.Date_Breach_Reported <= end select ent).ToList();
                                return RedirectToAction($"../Statistics/{entity}");
                            #endregion

                            #region Privacy Complaints  --none
                            case "Privacy_Complaints":
                                TablesContainer.list8 = (from ent in db.Privacy_Complaints where ent.Date_Complain_Received >= start && ent.Date_Complain_Received <= end select ent).ToList();
                                return RedirectToAction($"../Statistics/{entity}");
                            #endregion

                            #region Education    --none
                            case "Education":
                                //TablesContainer.list9 = (from ent in db.Educations where ent. >= start && ent.DateNews <= end select ent).ToList();
                                return RedirectToAction($"../Statistics/{entity}");
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

                            #region Immunization --none
                            case "Immunization":
                                //TablesContainer.list11 = (from ent in db.Immunizations where ent. >= start && ent.DateNews <= end select ent).ToList();
                                return RedirectToAction($"../Statistics/{entity}");
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
                        var tbl1 = db.Critical_Incidents.Where(i => i.Location == Id_Location).ToList();
                        tableList.AddRange(tbl1);
                        break;
                    case 2:
                        var tbl2 = db.Complaints.Where(i => i.Location == Id_Location).ToList();
                        tableList.AddRange(tbl2);
                        break;
                    case 3:
                        var tbl3 = db.Good_News.Where(i => i.Location == Id_Location).ToList();
                        tableList.AddRange(tbl3);
                        break;
                    case 4:
                        var tbl4 = db.Emergency_Prep.Where(i => i.Location == Id_Location).ToList();
                        tableList.AddRange(tbl4);
                        break;
                    case 5:
                        var tbl5 = db.Community_Risks.Where(i => i.Location == Id_Location).ToList();
                        tableList.AddRange(tbl5);
                        break;
                    case 6:
                        var tbl6 = db.Visits_Others.Where(i => i.Location == Id_Location).ToList();
                        tableList.AddRange(tbl6);
                        break;
                    case 7:
                        var tbl7 = db.Privacy_Breaches.Where(i => i.Location == Id_Location).ToList();
                        tableList.AddRange(tbl7);
                        break;
                    case 8:
                        var tbl8 = db.Privacy_Complaints.Where(i => i.Location == Id_Location).ToList();
                        tableList.AddRange(tbl8);
                        break;
                    case 9:
                        var tbl9 = db.Educations.Where(i => i.Location == Id_Location).ToList();
                        tableList.AddRange(tbl9);
                        break;
                    case 10:
                        var tbl10 = db.Relations.Where(i => i.Location == Id_Location).ToList();
                        tableList.AddRange(tbl10);
                        break;
                    case 11:
                        var tbl11 = db.Immunizations.Where(i => i.Location == Id_Location).ToList();
                        tableList.AddRange(tbl11);
                        break;
                    case 12:
                        var tbl12 = db.Outbreaks.Where(i => i.Location == Id_Location).ToList();
                        tableList.AddRange(tbl12);
                        break;
                    case 13:
                        var tbl13 = db.WSIBs.Where(i => i.Location == Id_Location).ToList();
                        tableList.AddRange(tbl13);
                        break;
                    case 14:
                        var tbl14 = db.Not_WSIBs.Where(i => i.Location == Id_Location).ToList();
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
            if(id != 0)
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
                entity.Compliment == null || entity.Location != 0|| entity.Department == null ||
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
        public string Name { get; set; }
        [Required(ErrorMessage = "This field is required! Please fill it in.")]
        [DataType(DataType.Date)]
        public DateTime Start { get; set; }
        [Required(ErrorMessage = "This field is required! Please fill it in.")]
        [DataType(DataType.Date)]
        public DateTime End { get; set; }
        public SelectList ListForms { get; set; }
    }
}