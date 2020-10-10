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
    [Authorize]
    public class ProfileController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        // GET: Profile
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var store = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(store);
            var user = userManager.FindById(userId);

            ProfileDetails profileD = new ProfileDetails
            {
                UserId = user.Id,
                UserName = user.FirstName + " " + user.LastName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password_hint = user.Password_hint
                
                
                
            };
            return View(profileD);
        }
    }
}