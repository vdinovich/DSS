using DTS.Models;
using System;
using System.Collections.Generic;
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
        public static string path { get; set; }
        public static int Id_Location { get; set; }
        List<CI_Category_Type> categories;
        List<Care_Community> communities;
        List<Position> positions;
        public static SelectList list2, list, list3, list4, list5, list6, list7, list8, list9, list10,list11, list12,list13,list14,list15,list16; //needed for front end drop down list
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
                 picture = new string[] { "Yes", "No" };
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
            ViewBag.id = Id_Location;
            ViewBag.idComplaints = Id_Location;
            return View();
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
            ViewBag.locations = list;
            return View();
        }

        [HttpPost]
        public ActionResult Community_Insert(Community_Risks entity)
        {
            ViewBag.locations = list;
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
            else if((entity.Location != 0)&& entity.Awards_Details == null|| entity.Awards_Received == null|| entity.Category == null|| entity.Community_Inititives == null||
                entity.Compliment == null|| entity.DateNews == null|| entity.Department == null|| 
                entity.Description_Complim == null|| entity.Growth == false|| entity.NameAwards == null|| entity.Passion == false||
                entity.ReceivedFrom == null|| entity.Respect == false|| entity.Responsibility == false|| entity.SourceCompliment == null ||
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
            return View();
        }

        [HttpPost]
        public ActionResult Agency_Insert(Visits_Agency entity)
        {
            ViewBag.locations = list;
            if (entity.Agency == null && entity.Corrective_Actions == null && entity.Date_of_Visit == DateTime.Now &&
                entity.Findings_Details == null && entity.Findings_number == 0 && entity.Report_Posted == null)
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
            ViewBag.locations = list;
            return View();
        }

        [HttpPost]
        public ActionResult Visits_Others(Visits_Others entity)
        {
            ViewBag.locations = list;
            if (entity.Agency == null && entity.Corrective_Actions == null && entity.Date_of_Visit == DateTime.MinValue &&
                entity.Details_of_Findings == null && entity.LHIN_Letter_Received == null && entity.Location == 0 &&
                entity.Number_of_Findings == 0 && entity.PH_Letter_Received == null && entity.Report_Posted == null)
            {
                //ViewBag.Empty = "All fields have to be filled.";
                return View();
            }
            else if ((entity.Date_of_Visit != DateTime.MinValue &&
                      entity.Details_of_Findings != null && entity.Location != 0 &&
                      entity.Number_of_Findings != 0) && entity.Agency == null || entity.Corrective_Actions == null || entity.LHIN_Letter_Received == null ||
                      entity.PH_Letter_Received == null || entity.Report_Posted == null)
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
}