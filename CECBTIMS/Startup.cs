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
            createRolesAndUsers();
        }

        //create roles and admin account for the first time
        private static void createRolesAndUsers()
        {
            var context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            // In Startup iam creating first Admin Role and creating a default Admin User     
            if (!roleManager.RoleExists("Administrator"))
            {
                // Admin Role    
                var role = new IdentityRole { Name = "Administrator" };
                roleManager.Create(role);

                var user = new ApplicationUser { UserName = "admin", Email = "admin@timsadmin.com" };

                var chkUser = userManager.Create(user, "password");
                //Add default User to Role Admin    
                if (chkUser.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Administrator");
                }
            }
            // creating Creating Employee role     
            if (roleManager.RoleExists("User")) return;
            {
                var role = new IdentityRole { Name = "User" };
                roleManager.Create(role);
            }
        }
    }
}