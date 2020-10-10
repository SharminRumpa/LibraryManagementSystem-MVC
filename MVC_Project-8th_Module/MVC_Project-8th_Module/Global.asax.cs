using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MVC_Project_8th_Module.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MVC_Project_8th_Module
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            ApplicationDbContext db = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));



            IdentityRole idenName = new IdentityRole();
            idenName.Name = "Admin";

            // ApplicationRoleManager.Create()

            IdentityResult r = roleManager.Create(idenName);
            if (r.Succeeded)
            {
                createDefaultRollAndUser();
            }

        }


        public async void createDefaultRollAndUser()
        {
            try
            {
                ApplicationDbContext context = new ApplicationDbContext();

                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                var user = new ApplicationUser { UserName = "sr@gmail.com", Email = "sr@gmail.com" };
                var result = await UserManager.CreateAsync(user, "sr1234");
                if (result.Succeeded)
                {
                    var userIdInstance = UserManager.Users.Where(w => w.UserName == "sr@gmail.com").FirstOrDefault();

                    UserManager.AddToRole(userIdInstance.Id, "Admin");

                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
