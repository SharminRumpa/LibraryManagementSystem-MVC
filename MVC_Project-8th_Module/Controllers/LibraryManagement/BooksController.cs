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
    public class BooksController : Controller
    {
        private LibraryManagementBDEntities db = new LibraryManagementBDEntities();

        // GET: Books
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

            var book = from s in db.Books.Include(b => b.Author).Include(b => b.Department).Include(b => b.Publisher) select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                book = book.Where(a => a.BookTitle.ToUpper().StartsWith(searchString.ToUpper())
                || a.Author.AuthorName.ToUpper().StartsWith(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    book = book.OrderByDescending(s => s.Department.DepartmentName);
                    break;
                case "Date":
                    book = book.OrderBy(s => s.BookTitle);
                    break;
                default:
                    book = book.OrderBy(s => s.NoOfPage);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            //var books = db.Books.Include(b => b.Author).Include(b => b.Department).Include(b => b.Publisher);
            return View(book.ToPagedList(pageNumber, pageSize));
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "AuthorName");
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "DepartmentName");
            ViewBag.PublisherID = new SelectList(db.Publishers, "PublisherID", "PublisherName");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookID,BookTitle,ISBN,AuthorID,DepartmentID,NoOfPage,PublisherID,ImageUrl")] Book book, HttpPostedFileBase ImagefileCreate)
        {
            if (ModelState.IsValid)
            {
                ImagefileCreate.SaveAs(Server.MapPath("~/Images/Books") + "/" + ImagefileCreate.FileName);
                string filePath = "~/Images/Books/" + ImagefileCreate.FileName;
                book.ImageUrl = filePath;
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "AuthorName", book.AuthorID);
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "DepartmentName", book.DepartmentID);
            ViewBag.PublisherID = new SelectList(db.Publishers, "PublisherID", "PublisherName", book.PublisherID);
            return View(book);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "AuthorName", book.AuthorID);
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "DepartmentName", book.DepartmentID);
            ViewBag.PublisherID = new SelectList(db.Publishers, "PublisherID", "PublisherName", book.PublisherID);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookID,BookTitle,ISBN,AuthorID,DepartmentID,NoOfPage,PublisherID,ImageUrl")] Book book, HttpPostedFileBase ImageFileCreate)
        {
            if (ModelState.IsValid)
            {
                //System.IO.File.Delete(Server.MapPath(book.ImageUrl));
                ImageFileCreate.SaveAs(Server.MapPath("~/Images/Books") + "/" + ImageFileCreate.FileName);
                string filePath = "~/Images/Books/" + ImageFileCreate.FileName;
                book.ImageUrl = filePath;
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "AuthorName", book.AuthorID);
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "DepartmentName", book.DepartmentID);
            ViewBag.PublisherID = new SelectList(db.Publishers, "PublisherID", "PublisherName", book.PublisherID);
            return View(book);
        }

        [Authorize(Roles = "Admin")]
        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            System.IO.File.Delete(Server.MapPath(book.ImageUrl));
            db.Books.Remove(book);
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
