using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Project_8th_Module.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "MVC Project On Relational Database. My Database about Library Management System.";

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        [Authorize]
        public ActionResult Vision()
        {
            ViewBag.Message = "SR Library Vision";

            return View();
            
        }
    }
}