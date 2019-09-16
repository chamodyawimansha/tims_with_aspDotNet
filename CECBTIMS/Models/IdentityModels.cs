using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CECBTIMS.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Agenda> Agendas { get; set; }
        public virtual ICollection<Brochure> Brochures { get; set; }
        public virtual ICollection<Cost> Costs { get; set; }
        public virtual ICollection<DefaultColumn>  DefaultColumns { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<EmploymentCategory> EmploymentCategories { get; set; }
        public virtual ICollection<EmploymentNature> EmploymentNatures { get; set; }
        public virtual ICollection<Organizer> Organizers { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Program> Programs { get; set; }
        public virtual ICollection<ProgramAssignment> ProgramAssignments { get; set; }
        public virtual ICollection<Requirement> Requirements { get; set; }
        public virtual ICollection<ResourcePerson> ResourcePersons { get; set; }
        public virtual ICollection<TargetGroup> TargetGroups { get; set; }
        public virtual ICollection<Template> Templates { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}