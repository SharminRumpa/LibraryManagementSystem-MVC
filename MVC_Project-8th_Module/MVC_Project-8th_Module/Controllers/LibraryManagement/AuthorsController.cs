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
    public class AuthorsController : Controller
    {
        private LibraryManagementBDEntities db = new LibraryManagementBDEntities();

        // GET: Authors
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else 
            { searchString = currentFilter; 
            }
            ViewBag.CurrentFilter = searchString;

            var author = from a in db.Authors select a;

            if (!String.IsNullOrEmpty(searchString)) 
            { 
                author = author.Where(a=> a.AuthorName.ToUpper().StartsWith(searchString.ToUpper()) 
                || a.DescripTion.ToUpper().StartsWith (searchString.ToUpper())); 
            }

            switch (sortOrder)
            {
                case "date_desc":
                    author = author.OrderBy(a => a.AuthorName);
                    break;
                case "name_desc":
                    author = author.OrderByDescending(a => a.DescripTion);
                    break;
                default:
                    author = author.OrderBy(a => a.ImageUrl);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(author.ToPagedList(pageNumber, pageSize));
        }

        // GET: Authors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // GET: Authors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AuthorID,AuthorName,DescripTion,ImageUrl")] Author author, HttpPostedFileBase ImageFileCreate)
        {
            if (ModelState.IsValid)
            {
                ImageFileCreate.SaveAs(Server.MapPath("~/Images/Author") + "/" + ImageFileCreate.FileName);
                string filePath = "~/Images/Author/" + ImageFileCreate.FileName;
                author.ImageUrl = filePath;
                db.Authors.Add(author);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(author);
        }

        // GET: Authors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AuthorID,AuthorName,DescripTion,ImageUrl")] Author author, HttpPostedFileBase ImageFileCreate)
        {
            if (ModelState.IsValid)
            {
                System.IO.File.Delete(Server.MapPath(author.ImageUrl));
                ImageFileCreate.SaveAs(Server.MapPath("~/Images/Author") + "/" + ImageFileCreate.FileName);
                string filePath = "~/Images/Author/" + ImageFileCreate.FileName;
                author.ImageUrl = filePath;
                db.Entry(author).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(author);
        }

        [Authorize(Roles = "Admin")]
        // GET: Authors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Author author = db.Authors.Find(id);
            System.IO.File.Delete(Server.MapPath(author.ImageUrl));
            db.Authors.Remove(author);
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
