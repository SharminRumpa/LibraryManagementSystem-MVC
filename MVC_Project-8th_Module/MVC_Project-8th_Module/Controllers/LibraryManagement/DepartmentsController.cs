using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC_Project_8th_Module.Models;
using PagedList;

namespace MVC_Project_8th_Module.Controllers.LibraryManagement
{
    public class DepartmentsController : Controller
    {
        private LibraryManagementBDEntities db = new LibraryManagementBDEntities();

        // GET: Departments
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var departments = from d in db.Departments.Include(d => d.Faculty) select d;

            if (!String.IsNullOrEmpty(searchString))
            {
                departments = departments.Where(a => a.DepartmentName.ToUpper().StartsWith(searchString.ToUpper())
                || a.Faculty.FacultyName.ToUpper().StartsWith(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    departments = departments.OrderBy(a => a.DepartmentName);
                    break;
                default:
                    departments = departments.OrderByDescending(a => a.Faculty.FacultyName);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            //var departments = db.Departments.Include(d => d.Faculty);
            //var dep = departments.ToPagedList(pageNumber, pageSize);
            return View(departments.ToPagedList(pageNumber, pageSize));
            //return View(dep);
        }

        // GET: Departments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // GET: Departments/Create
        public ActionResult Create()
        {
            ViewBag.FacultyID = new SelectList(db.Faculties, "FacultyID", "FacultyName");
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DepartmentID,DepartmentName,FacultyID")] Department department)
        {
            if (ModelState.IsValid)
            {
                db.Departments.Add(department);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FacultyID = new SelectList(db.Faculties, "FacultyID", "FacultyName", department.FacultyID);
            return View(department);
        }

        // GET: Departments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            ViewBag.FacultyID = new SelectList(db.Faculties, "FacultyID", "FacultyName", department.FacultyID);
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DepartmentID,DepartmentName,FacultyID")] Department department)
        {
            if (ModelState.IsValid)
            {
                db.Entry(department).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FacultyID = new SelectList(db.Faculties, "FacultyID", "FacultyName", department.FacultyID);
            return View(department);
        }

        [Authorize(Roles = "Admin")]
        // GET: Departments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Department department = db.Departments.Find(id);
            db.Departments.Remove(department);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
