﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DTS.Controllers
{
    public class AdminLTEController : Controller
    {
        // GET: AdminLTE
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _View()
        {
            return View();
        }

        public ActionResult Pdf_Viewer(string text)
        {
            ViewBag.Text = HomeController.text;
            return View();
        }
    }
}