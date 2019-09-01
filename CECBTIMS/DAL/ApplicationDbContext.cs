using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.ModelConfiguration.Conventions;
using CECBTIMS.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CECBTIMS.DAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
//            RelationshipSet dont work
//            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Program> Programs { get; set; }


        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<CECBTIMS.Models.Organizer> Organizers { get; set; }

        public System.Data.Entity.DbSet<CECBTIMS.Models.TargetGroup> TargetGroups { get; set; }

        protected new void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            
            modelBuilder.Entity<TargetGroup>()
                .HasRequired<Program>(s => s.Program)
                .WithMany(g => g.TargetGroups)
                .HasForeignKey<int>(s => s.ProgramId);

        }

        public System.Data.Entity.DbSet<CECBTIMS.Models.Cost> Costs { get; set; }

        public System.Data.Entity.DbSet<CECBTIMS.Models.Requirement> Requirements { get; set; }

        public System.Data.Entity.DbSet<CECBTIMS.Models.Agenda> Agenda { get; set; }

        public System.Data.Entity.DbSet<CECBTIMS.Models.ResourcePerson> ResourcePersons { get; set; }

        //        public System.Data.Entity.DbSet<CECBTIMS.Models.Employee> Employees { get; set; }

        public System.Data.Entity.DbSet<CECBTIMS.Models.ProgramAssignment> ProgramAssignments { get; set; }

        public System.Data.Entity.DbSet<CECBTIMS.Models.EmploymentCategory> EmploymentCategories { get; set; }

        public System.Data.Entity.DbSet<CECBTIMS.Models.EmploymentNature> EmploymentNatures { get; set; }

        public System.Data.Entity.DbSet<CECBTIMS.Models.Template> Templates { get; set; }

        public System.Data.Entity.DbSet<CECBTIMS.Models.Document> Documents { get; set; }

        public System.Data.Entity.DbSet<CECBTIMS.Models.DefaultColumn> DefaultColumns { get; set; }
    }
}