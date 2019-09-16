namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationUserAddedToBaseCols : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Agenda", "CreatedByApplicationUserId", c => c.String(maxLength: 128));
            AddColumn("dbo.Programs", "CreatedByApplicationUserId", c => c.String(maxLength: 128));
            AddColumn("dbo.Brochures", "CreatedByApplicationUserId", c => c.String(maxLength: 128));
            AddColumn("dbo.Costs", "CreatedByApplicationUserId", c => c.String(maxLength: 128));
            AddColumn("dbo.Documents", "CreatedByApplicationUserId", c => c.String(maxLength: 128));
            AddColumn("dbo.EmploymentCategories", "CreatedByApplicationUserId", c => c.String(maxLength: 128));
            AddColumn("dbo.EmploymentNatures", "CreatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.EmploymentNatures", "UpdatedAt", c => c.DateTime());
            AddColumn("dbo.EmploymentNatures", "CreatedByApplicationUserId", c => c.String(maxLength: 128));
            AddColumn("dbo.EmploymentNatures", "UpdatedBy", c => c.String());
            AddColumn("dbo.EmploymentNatures", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Organizers", "CreatedByApplicationUserId", c => c.String(maxLength: 128));
            AddColumn("dbo.Payments", "CreatedByApplicationUserId", c => c.String(maxLength: 128));
            AddColumn("dbo.ProgramAssignments", "CreatedByApplicationUserId", c => c.String(maxLength: 128));
            AddColumn("dbo.Requirements", "CreatedByApplicationUserId", c => c.String(maxLength: 128));
            AddColumn("dbo.ResourcePersons", "CreatedByApplicationUserId", c => c.String(maxLength: 128));
            AddColumn("dbo.TargetGroups", "CreatedByApplicationUserId", c => c.String(maxLength: 128));
            AddColumn("dbo.DefaultColumns", "CreatedByApplicationUserId", c => c.String(maxLength: 128));
            AddColumn("dbo.Templates", "CreatedByApplicationUserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Agenda", "CreatedByApplicationUserId");
            CreateIndex("dbo.Brochures", "CreatedByApplicationUserId");
            CreateIndex("dbo.Programs", "CreatedByApplicationUserId");
            CreateIndex("dbo.Costs", "CreatedByApplicationUserId");
            CreateIndex("dbo.Documents", "CreatedByApplicationUserId");
            CreateIndex("dbo.EmploymentCategories", "CreatedByApplicationUserId");
            CreateIndex("dbo.EmploymentNatures", "CreatedByApplicationUserId");
            CreateIndex("dbo.Organizers", "CreatedByApplicationUserId");
            CreateIndex("dbo.Payments", "CreatedByApplicationUserId");
            CreateIndex("dbo.ProgramAssignments", "CreatedByApplicationUserId");
            CreateIndex("dbo.Requirements", "CreatedByApplicationUserId");
            CreateIndex("dbo.ResourcePersons", "CreatedByApplicationUserId");
            CreateIndex("dbo.TargetGroups", "CreatedByApplicationUserId");
            CreateIndex("dbo.DefaultColumns", "CreatedByApplicationUserId");
            CreateIndex("dbo.Templates", "CreatedByApplicationUserId");
            AddForeignKey("dbo.Agenda", "CreatedByApplicationUserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Brochures", "CreatedByApplicationUserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Costs", "CreatedByApplicationUserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Programs", "CreatedByApplicationUserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Documents", "CreatedByApplicationUserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.EmploymentCategories", "CreatedByApplicationUserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.EmploymentNatures", "CreatedByApplicationUserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Organizers", "CreatedByApplicationUserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Payments", "CreatedByApplicationUserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.ProgramAssignments", "CreatedByApplicationUserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Requirements", "CreatedByApplicationUserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.ResourcePersons", "CreatedByApplicationUserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.TargetGroups", "CreatedByApplicationUserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.DefaultColumns", "CreatedByApplicationUserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Templates", "CreatedByApplicationUserId", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Agenda", "CreatedBy");
            DropColumn("dbo.Programs", "CreatedBy");
            DropColumn("dbo.Brochures", "CreatedBy");
            DropColumn("dbo.Costs", "CreatedBy");
            DropColumn("dbo.Documents", "CreatedBy");
            DropColumn("dbo.EmploymentCategories", "CreatedBy");
            DropColumn("dbo.Organizers", "CreatedBy");
            DropColumn("dbo.Payments", "CreatedBy");
            DropColumn("dbo.ProgramAssignments", "CreatedBy");
            DropColumn("dbo.Requirements", "CreatedBy");
            DropColumn("dbo.ResourcePersons", "CreatedBy");
            DropColumn("dbo.TargetGroups", "CreatedBy");
            DropColumn("dbo.DefaultColumns", "CreatedBy");
            DropColumn("dbo.Templates", "CreatedBy");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Templates", "CreatedBy", c => c.String());
            AddColumn("dbo.DefaultColumns", "CreatedBy", c => c.String());
            AddColumn("dbo.TargetGroups", "CreatedBy", c => c.String());
            AddColumn("dbo.ResourcePersons", "CreatedBy", c => c.String());
            AddColumn("dbo.Requirements", "CreatedBy", c => c.String());
            AddColumn("dbo.ProgramAssignments", "CreatedBy", c => c.String());
            AddColumn("dbo.Payments", "CreatedBy", c => c.String());
            AddColumn("dbo.Organizers", "CreatedBy", c => c.String());
            AddColumn("dbo.EmploymentCategories", "CreatedBy", c => c.String());
            AddColumn("dbo.Documents", "CreatedBy", c => c.String());
            AddColumn("dbo.Costs", "CreatedBy", c => c.String());
            AddColumn("dbo.Brochures", "CreatedBy", c => c.String());
            AddColumn("dbo.Programs", "CreatedBy", c => c.String());
            AddColumn("dbo.Agenda", "CreatedBy", c => c.String());
            DropForeignKey("dbo.Templates", "CreatedByApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.DefaultColumns", "CreatedByApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.TargetGroups", "CreatedByApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ResourcePersons", "CreatedByApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Requirements", "CreatedByApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProgramAssignments", "CreatedByApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Payments", "CreatedByApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Organizers", "CreatedByApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.EmploymentNatures", "CreatedByApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.EmploymentCategories", "CreatedByApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Documents", "CreatedByApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Programs", "CreatedByApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Costs", "CreatedByApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Brochures", "CreatedByApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Agenda", "CreatedByApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Templates", new[] { "CreatedByApplicationUserId" });
            DropIndex("dbo.DefaultColumns", new[] { "CreatedByApplicationUserId" });
            DropIndex("dbo.TargetGroups", new[] { "CreatedByApplicationUserId" });
            DropIndex("dbo.ResourcePersons", new[] { "CreatedByApplicationUserId" });
            DropIndex("dbo.Requirements", new[] { "CreatedByApplicationUserId" });
            DropIndex("dbo.ProgramAssignments", new[] { "CreatedByApplicationUserId" });
            DropIndex("dbo.Payments", new[] { "CreatedByApplicationUserId" });
            DropIndex("dbo.Organizers", new[] { "CreatedByApplicationUserId" });
            DropIndex("dbo.EmploymentNatures", new[] { "CreatedByApplicationUserId" });
            DropIndex("dbo.EmploymentCategories", new[] { "CreatedByApplicationUserId" });
            DropIndex("dbo.Documents", new[] { "CreatedByApplicationUserId" });
            DropIndex("dbo.Costs", new[] { "CreatedByApplicationUserId" });
            DropIndex("dbo.Programs", new[] { "CreatedByApplicationUserId" });
            DropIndex("dbo.Brochures", new[] { "CreatedByApplicationUserId" });
            DropIndex("dbo.Agenda", new[] { "CreatedByApplicationUserId" });
            DropColumn("dbo.Templates", "CreatedByApplicationUserId");
            DropColumn("dbo.DefaultColumns", "CreatedByApplicationUserId");
            DropColumn("dbo.TargetGroups", "CreatedByApplicationUserId");
            DropColumn("dbo.ResourcePersons", "CreatedByApplicationUserId");
            DropColumn("dbo.Requirements", "CreatedByApplicationUserId");
            DropColumn("dbo.ProgramAssignments", "CreatedByApplicationUserId");
            DropColumn("dbo.Payments", "CreatedByApplicationUserId");
            DropColumn("dbo.Organizers", "CreatedByApplicationUserId");
            DropColumn("dbo.EmploymentNatures", "RowVersion");
            DropColumn("dbo.EmploymentNatures", "UpdatedBy");
            DropColumn("dbo.EmploymentNatures", "CreatedByApplicationUserId");
            DropColumn("dbo.EmploymentNatures", "UpdatedAt");
            DropColumn("dbo.EmploymentNatures", "CreatedAt");
            DropColumn("dbo.EmploymentCategories", "CreatedByApplicationUserId");
            DropColumn("dbo.Documents", "CreatedByApplicationUserId");
            DropColumn("dbo.Costs", "CreatedByApplicationUserId");
            DropColumn("dbo.Brochures", "CreatedByApplicationUserId");
            DropColumn("dbo.Programs", "CreatedByApplicationUserId");
            DropColumn("dbo.Agenda", "CreatedByApplicationUserId");
        }
    }
}
