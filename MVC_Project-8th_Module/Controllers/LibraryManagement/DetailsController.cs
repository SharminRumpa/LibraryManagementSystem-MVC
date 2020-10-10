using MVC_Project_8th_Module.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Project_8th_Module.Controllers.LibraryManagement
{
    public class DetailsController : Controller
    {
        private LibraryManagementBDEntities db = new LibraryManagementBDEntities();
        // GET: Details
        public ActionResult Index(int? FacultyID, int? DepartmentID)
        {
            var singleSelectList = new SelectedDetailsModel();

            singleSelectList.Faculties = db.Faculties.ToList();

            if (FacultyID == null)
            {
                if (Session["FacultyID"] != null)
                {
                    FacultyID = Convert.ToInt32(Session["FacultyID"].ToString());
                }
            }

            if (FacultyID != null)
            {

                Session["FacultyID"] = FacultyID;
                singleSelectList.Departments = db.Departments.Where(w => w.FacultyID == FacultyID.Value).ToList();


            }

            if (DepartmentID != null)
            {

                singleSelectList.Books = db.Books.Where(w => w.DepartmentID == DepartmentID.Value).ToList();

            }

            return View(singleSelectList);
        }
    }
}