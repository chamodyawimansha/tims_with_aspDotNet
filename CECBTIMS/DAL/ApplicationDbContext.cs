using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using CECBTIMS.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CECBTIMS.DAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Program> Programs { get; set; }

//        public DbSet<ResourcePerson> ResourcePersons { get; set; }
//        public DbSet<ProgramResourcePersons> ProgramResourcePersons { get; set; }
//        public DbSet<Cost> Costs { get; set; }
        //        public DbSet<Organizer> Organizers { get; set; }
        //        public DbSet<ProgramArrangement> ProgramArrangements { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<CECBTIMS.Models.Organizer> Organizers { get; set; }

        public System.Data.Entity.DbSet<CECBTIMS.Models.TargetGroup> TargetGroups { get; set; }

        //        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //        {
        //                Not Working with user models
        //            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        //
        //            modelBuilder.Entity<Program>().HasMany(c => c.Instructors).WithMany(i => i.Courses)
        //                .Map(t => t.MapLeftKey("CourseID")
        //                    .MapRightKey("InstructorID")
        //                    .ToTable("CourseInstructor"));
        //        }
    }
}