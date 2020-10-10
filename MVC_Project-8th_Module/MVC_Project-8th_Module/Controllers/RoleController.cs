using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MVC_Project_8th_Module.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Project_8th_Module.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Role
        public ActionResult Index()
        {
            var roleList = db.Roles.ToList();
            return View(roleList);
        }
        [HttpGet]
        public ActionResult Create()
        {
            
            var role = new IdentityRole();
            return View(role);
        }
        [HttpPost]
        public ActionResult Create(IdentityRole identity)
        {
            db.Roles.Add(identity);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(string Name)
        {
            if (HttpContext.User.IsInRole("Admin"))
            {

                ApplicationDbContext db = new ApplicationDbContext();
                ApplicationDbContext dbRole = new ApplicationDbContext();

                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(dbRole));
                var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(db));

                try
                {
                    ICollection<IdentityUserRole> IdentityUserRoleList = roleManager.FindByName(Name).Users;

                    foreach (IdentityUserRole iur in IdentityUserRoleList)
                    {
                        ApplicationUser au = db.Users.Find(new object[] { iur.UserId });
                        db.Users.Remove(au);
                        db.SaveChanges();

                    }

                }
                catch(Exception ex)
                {

                }
                
                IdentityRole IdentRoleName = dbRole.Roles.Where(w => w.Name == Name).FirstOrDefault();
                dbRole.Roles.Remove(IdentRoleName);
                dbRole.SaveChanges();
                return RedirectToAction("Index", "Role");
            }
            else
            {
                return RedirectToAction("Index", "Role");
            }

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