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

        private void createRolesAndUsers()
        {
            var context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // In Startup iam creating first Admin Role and creating a default Admin User     
            if (!roleManager.RoleExists("Manager"))
            {

                // first we create Manager Role    
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole {Name = "Manager" };
                roleManager.Create(role);

                var user = new ApplicationUser {UserName = "Manager", Email = "Manager@cecbtims.com"};
                const string defaultPassword = "A@Z200711";

                var chkUser = userManager.Create(user, defaultPassword);
                //Add default User to Role Admin    
                if (chkUser.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Manager");
                }
            }
            // creating Creating Employee role     
            if (roleManager.RoleExists("Employee")) return;
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole {Name = "Employee"};
                roleManager.Create(role);
            }

        }
    }
}