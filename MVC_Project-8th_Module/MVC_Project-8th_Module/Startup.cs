using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVC_Project_8th_Module.Startup))]
namespace MVC_Project_8th_Module
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
