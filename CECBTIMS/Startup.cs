using CECBTIMS.Controllers;
using CECBTIMS.DAL;
using CECBTIMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CECBTIMS.Startup))]

namespace CECBTIMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            AccountController.createRolesAndUsers();
        }
    }
}