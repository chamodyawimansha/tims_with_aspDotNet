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
            if (!roleManager.RoleExists("Admin"))
            {

                // Admin Role    
                var role = new IdentityRole {Name = "Admin" };
                roleManager.Create(role);

                var user = new ApplicationUser {UserName = "Admin", Email = "Admin@timsadmin.com"};
                const string defaultPassword = "123456789";

                var chkUser = userManager.Create(user, defaultPassword);
                //Add default User to Role Admin    
                if (chkUser.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Admin");
                }
            }
            // creating Creating Employee role     
            if (roleManager.RoleExists("user")) return;
            {
                var role = new IdentityRole {Name = "user"};
                roleManager.Create(role);
            }
//
//            change the models and controllers according to the web site
//                delete the record from the database
//            https://www.c-sharpcorner.com/UploadFile/asmabegam/Asp-Net-mvc-5-security-and-creating-user-role/
        }
    }
}