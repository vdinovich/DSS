using DTS.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DTS.Controllers
{
    public class HomeController : Controller
    {
        MyContext db = new MyContext();
        static string notsel;
        public static int[] counts;
        public static List<string> strs;
        public static string path { get; set; }
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
                 department = new string[] { "Nursing", "Nursing Admin", "Admin", "Programs", "Food Service", "Maintainence", "Housekeeping", "Laundry", "Other" },
                 resolved = new string[] { "Yes", "No", "Ongoing" },
                 ministry = new string[] { "Yes", "No" },
                 category = new string[] { "GoodNews", "Compliments" },
                 department2 = new string[] {"All","Nursing","Housekeeping","Laundry","Maintenance","Dietary","Recriation","Administration","Individual(s)",
                                            "Physio", "Hairdresser", "Physician","Foot care", "Dental", "Other", "Yes", "No"},
                 source = new string[] { "Let's Connect", "Card", "Email", "Letter", "Verbal", "Other" },
                 receiveFrom = new string[] { "Resident", "Family", "Supplier", "SSO", "Manager", "Leadership", "Tour", "Other" },
                 picture = new string[] { "Yes", "No" },
                 risk_list = new string[] { "Adverse Event", "Environmental", "Infortmation Technology", "Insurance", "Legal", "Near Miss", "Serious Adverse Event",
                     "Vendor", "Workplace Harrasement", "Other" },
                 visitAgency = new string[] { "MOH", "Fire", "TSSA", "Public Health", "PCQO", "QR Health Authority", "CNS", "Public Health - MHO", "Public Health - EHO", "Other" },
                 visitnumbers = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
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
            list9 = new SelectList(department);
            list10 = new SelectList(resolved);
            list11 = new SelectList(ministry);

            // for GoodNews table:
            list12 = new SelectList(category);
            list13 = new SelectList(department2);
            list14 = new SelectList(source);
            list15 = new SelectList(receiveFrom);
            list16 = new SelectList(picture);

            // for Community Risks:
            list17 = new SelectList(risk_list);

            // for Visit Agency:
            list18 = new SelectList(visitAgency);
            list19 = new SelectList(visitnumbers);
        }

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

        [HttpGet]
        public ActionResult SignIN()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignIN(Users user)
        {
            string login = user.Login;
            string password = user.Password;
            List<Users> users = db.Users.ToList(); //takes all users from users table in a db and packs it into list users
            bool flag = false;
            for (int i = 0; i < users.Count(); i++)
            {
                if (users[i].Login == login && users[i].Password == password)
                {
                    flag = true;
                    return RedirectToAction("../Home/Index");
                }
            }
            if (!flag) ViewBag.incorrect = "Incorrect Login or Password...Please try again!";
            return View();
        }

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
                    ViewBag.ResultMsg = "Your record was added successfuly!";

                    return View();
                }
            }
            return View();
        }

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
                        fname = Path.Combine(Server.MapPath("~/Uploaded_Files/"), fname);
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
            if (!flag) return Json("There is nothing to delete...Please upload any file!");
            else return RedirectToAction("../Home/AllFiles");
        }

        public ActionResult WOR_Tabs()
        {
            WorTabs tabs = new WorTabs();
            tabs.ListForms = GetFormNames();

            return View(tabs);
        }

        [HttpPost]
        public ActionResult WOR_Tabs(WorTabs Value)
        {
            DateTime start = DateTime.MinValue, end = DateTime.MinValue;
            string errorMsg = string.Empty;
            if (Value != null && Value.Name != null)
            {
                string btnName = Request.Params
                      .Cast<string>()
                      .Where(p => p.StartsWith("btn"))
                      .Select(p => p.Substring("btn".Length))
                      .First();

                if (btnName.Equals("-list"))
                {
                    string name = Value.Name;
                    return GoToSelectFormList($"../Home/GoToSelectFormList/{name}");
                }
                else if(btnName.Equals("-insert"))
                {
                    int id = int.Parse(Value.Name);
                    return RedirectToAction($"../Home/GoToSelectForm/{id}");
                }
                else if (btnName.Equals("-export"))
                {
                    start = Value.Start;
                    end = Value.End;
                    if (start != DateTime.MinValue && end != DateTime.MinValue)
                    {
                        TablesContainer.list1 = (from ent in db.Critical_Incidents where ent.Date >= start && ent.Date <= end select ent).ToList();
                        if (TablesContainer.list1.Count != 0)
                        {
                            string msg = new STREAM().WriteTo_CSV(TablesContainer.list1);
                        }
                        else 
                        { 
                            ViewBag.ErrorMsg = errorMsg = "Nothing found to your choice dates..";
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
                else if(btnName.Equals("-summary"))
                {
                    start = Value.Start;
                    end = Value.End;
                    int id = int.Parse(Value.Name);
                    var tbl_list = GetTableById(id).ToArray().ToList();
                    Type type = tbl_list[1].GetType();
                    string entity = type.Name;
                    if (!entity.Equals(string.Empty))
                    {
                        ViewBag.TableName = entity;
                    }

                    switch (entity)
                    {
                        case "Critical_Incidents":
                            TablesContainer.list1 = (from ent in db.Critical_Incidents where ent.Date >= start && ent.Date <= end select ent).ToList();
                            var ci = db.CI_Category_Types.ToList();

                            var names = new List<string>();
                            foreach (var n in ci)
                                names.Add(n.Name);

                            counts = new int[ci.Count]; // for each parameter CI_Category_Type set up count

                            for(int i = 0; i < counts.Length; i++)
                                counts[i] = 0;
                            var g = TablesContainer.list1.GroupBy(i => i.CI_Category_Type);

                            strs = new List<string>();
                            foreach (var group in g)
                            {
                                strs.Add($"{names[group.Key - 1]}\t-\t{group.Count()}" );
                            }

                            #region Count of all found records:
                            foreach (var i in TablesContainer.list1)
                            {
                                if (i.Brief_Description != null) TablesContainer.c1++;
                                if (i.Care_Plan_Updated != null) TablesContainer.c2++;
                                if (i.CIS_Initiated != null) TablesContainer.c3++;
                                if (i.CI_Category_Type != 0) TablesContainer.c4++;
                                if (i.CI_Form_Number != null) TablesContainer.c5++;
                                if (i.Date != DateTime.MinValue) TablesContainer.c6++;
                                if (i.File_Complete != null) TablesContainer.c7++;
                                if (i.Follow_Up_Amendments != null) TablesContainer.c8++;
                                if (i.Location != 0) TablesContainer.c9++;
                                if (i.MOHLTC_Follow_Up != null) TablesContainer.c10++;
                                if (i.MOH_Notified != null) TablesContainer.c11++;
                                if (i.POAS_Notified != null) TablesContainer.c12++;
                                if (i.Police_Notified != null) TablesContainer.c13++;
                                if (i.Quality_Improvement_Actions != null) TablesContainer.c14++;
                                if (i.Risk_Locked != null) TablesContainer.c15++;
                            }

                            TablesContainer.count_arr.AddRange(new int[] {
                            TablesContainer.c1++,TablesContainer.c2++,TablesContainer.c3++,TablesContainer.c4++,TablesContainer.c5++,
                            TablesContainer.c6++,TablesContainer.c7++,TablesContainer.c8++,TablesContainer.c9++,TablesContainer.c10++,
                            TablesContainer.c11++,TablesContainer.c12++,TablesContainer.c13++,TablesContainer.c14++,TablesContainer.c15++
                            });
                            #endregion

                            return RedirectToAction($"../Statistics/{entity}");                  
                        case "Complaint":
                            List<Complaint> list2 = (from ent in db.Complaints where ent.DateReceived >= start && ent.DateReceived <= end select ent).ToList();
                            return RedirectToAction($"../Statistics/{entity}"); 
                        case "Good_News":
                            TablesContainer.list3 = (from ent in db.Good_News where ent.DateNews >= start && ent.DateNews <= end select ent).ToList();
                            return RedirectToAction($"../Statistics/{entity}");
                        case "Emergency_Prep":
                            //TablesContainer.list4 = (from ent in db.Emergency_Prep where ent.D >= start && ent.DateNews <= end select ent).ToList();
                            return RedirectToAction($"../Statistics/{entity}");
                        case "Community_Risks":
                            TablesContainer.list5 = (from ent in db.Community_Risks where ent.Date >= start && ent.Date <= end select ent).ToList();
                            return RedirectToAction($"../Statistics/{entity}");
                        case "Visits_Others":
                            TablesContainer.list6 = (from ent in db.Visits_Others where ent.Date_of_Visit >= start && ent.Date_of_Visit <= end select ent).ToList();
                            return RedirectToAction($"../Statistics/{entity}");
                        case "Privacy_Breaches":
                            TablesContainer.list7 = (from ent in db.Privacy_Breaches where ent.Date_Breach_Reported >= start && ent.Date_Breach_Reported <= end select ent).ToList();
                            return RedirectToAction($"../Statistics/{entity}");
                        case "Privacy_Complaints":
                            TablesContainer.list8 = (from ent in db.Privacy_Complaints where ent.Date_Complain_Received >= start && ent.Date_Complain_Received <= end select ent).ToList();
                            return RedirectToAction($"../Statistics/{entity}");
                        case "Education":
                            //TablesContainer.list9 = (from ent in db.Educations where ent. >= start && ent.DateNews <= end select ent).ToList();
                            return RedirectToAction($"../Statistics/{entity}");
                        case "Labour_Relations":
                            TablesContainer.list10 = (from ent in db.Relations where ent.Date >= start && ent.Date <= end select ent).ToList();
                            return RedirectToAction($"../Statistics/{entity}");
                        case "Immunization":
                            //TablesContainer.list11 = (from ent in db.Immunizations where ent. >= start && ent.DateNews <= end select ent).ToList();
                            return RedirectToAction($"../Statistics/{entity}");
                        case "Outbreaks":
                            TablesContainer.list12 = (from ent in db.Outbreaks where ent.Date_Declared >= start && ent.Date_Declared <= end select ent).ToList();
                            return RedirectToAction($"../Statistics/{entity}");
                        case "WSIB":
                            TablesContainer.list13 = (from ent in db.WSIBs where ent.Date_Accident >= start && ent.Date_Accident <= end select ent).ToList();
                            return RedirectToAction($"../Statistics/{entity}");
                        case "Not_WSIBs":
                            TablesContainer.list14 = (from ent in db.Not_WSIBs where ent.Date_of_Incident >= start && ent.Date_of_Incident <= end select ent).ToList();
                            return RedirectToAction($"../Statistics/{entity}");
                    }
                }
            }
            else  //  if you don't selected anything from list
            {
                ViewBag.ErrorMsg = errorMsg = "Please select something from the list on the left side";
                WorTabs tabs = new WorTabs();
                tabs.ListForms = GetFormNames();
                return View(tabs);
            }

            return RedirectToAction("../Home/WOR_Tabs");
        }

        #region All Forms:
        SelectList GetFormNames()
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
                        var tbl1 = db.Critical_Incidents.ToList();
                        tableList.AddRange(tbl1);
                        break;
                    case 2:
                        var tbl2 = db.Complaints.ToList();
                        tableList.AddRange(tbl2);
                        break;
                    case 3:
                        var tbl3 = db.Good_News.ToList();
                        tableList.AddRange(tbl3);
                        break;
                    case 4:
                        var tbl4 = db.Emergency_Prep.ToList();
                        tableList.AddRange(tbl4);
                        break;
                    case 5:
                        var tbl5 = db.Community_Risks.ToList();
                        tableList.AddRange(tbl5);
                        break;
                    case 6:
                        var tbl6 = db.Visits_Others.ToList();
                        tableList.AddRange(tbl6);
                        break;
                    case 7:
                        var tbl7 = db.Privacy_Breaches.ToList();
                        tableList.AddRange(tbl7);
                        break;
                    case 8:
                        var tbl8 = db.Privacy_Complaints.ToList();
                        tableList.AddRange(tbl8);
                        break;
                    case 9:
                        var tbl9 = db.Educations.ToList();
                        tableList.AddRange(tbl9);
                        break;
                    case 10:
                        var tbl10 = db.Relations.ToList();
                        tableList.AddRange(tbl10);
                        break;
                    case 11:
                        var tbl11 = db.Immunizations.ToList();
                        tableList.AddRange(tbl11);
                        break;
                    case 12:
                        var tbl12 = db.Outbreaks.ToList();
                        tableList.AddRange(tbl12);
                        break;
                    case 13:
                        var tbl13 = db.WSIBs.ToList();
                        tableList.AddRange(tbl13);
                        break;
                    case 14:
                        var tbl14 = db.Not_WSIBs.ToList();
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
            else if ((entity.Location != 0) && entity.Awards_Details == null || entity.Awards_Received == null || entity.Category == null || entity.Community_Inititives == null ||
                entity.Compliment == null || entity.DateNews == null || entity.Department == null ||
                entity.Description_Complim == null || entity.Growth == false || entity.NameAwards == null || entity.Passion == false ||
                entity.ReceivedFrom == null || entity.Respect == false || entity.Responsibility == false || entity.SourceCompliment == null ||
                entity.Spot_Awards == null || entity.Teamwork == false)
            {
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