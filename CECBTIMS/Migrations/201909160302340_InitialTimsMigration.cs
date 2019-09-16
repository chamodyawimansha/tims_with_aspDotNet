namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialTimsMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Agenda",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        From = c.DateTime(nullable: false),
                        To = c.DateTime(nullable: false),
                        ProgramId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        ApplicationUserId = c.String(maxLength: 128),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Programs", t => t.ProgramId, cascadeDelete: true)
                .Index(t => t.ProgramId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.Brochures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 255),
                        Details = c.String(maxLength: 255),
                        FileName = c.String(maxLength: 255),
                        OriginalFileName = c.String(maxLength: 255),
                        FileType = c.Int(nullable: false),
                        FileMethod = c.Int(nullable: false),
                        ProgramId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        ApplicationUserId = c.String(maxLength: 128),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Programs", t => t.ProgramId, cascadeDelete: true)
                .Index(t => t.ProgramId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.Programs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 255),
                        ProgramType = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        StartTime = c.DateTime(),
                        EndTime = c.DateTime(),
                        ApplicationClosingDate = c.DateTime(nullable: false),
                        ApplicationClosingTime = c.DateTime(nullable: false),
                        Venue = c.String(maxLength: 255),
                        EndDate = c.DateTime(),
                        NotifiedBy = c.String(maxLength: 255),
                        NotifiedOn = c.DateTime(),
                        ProgramHours = c.Byte(),
                        DurationInDays = c.Byte(),
                        DurationInMonths = c.Byte(),
                        Department = c.String(),
                        Currency = c.Int(nullable: false),
                        ProgramFee = c.Double(),
                        RegistrationFee = c.Single(),
                        PerPersonFee = c.Single(),
                        NoShowFee = c.Single(),
                        MemberFee = c.Single(),
                        NonMemberFee = c.Single(),
                        StudentFee = c.Single(),
                        OrganizerId = c.Int(),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        ApplicationUserId = c.String(maxLength: 128),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Organizers", t => t.OrganizerId)
                .Index(t => t.OrganizerId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.Costs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.Double(nullable: false),
                        ProgramId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        ApplicationUserId = c.String(maxLength: 128),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Programs", t => t.ProgramId, cascadeDelete: true)
                .Index(t => t.ProgramId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Details = c.String(),
                        FileName = c.String(),
                        ProgramType = c.Int(nullable: false),
                        FileType = c.Int(nullable: false),
                        FileMethod = c.Int(nullable: false),
                        ProgramId = c.Int(nullable: false),
                        EmployeeId = c.Guid(),
                        DocumentNumber = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        ApplicationUserId = c.String(maxLength: 128),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Programs", t => t.ProgramId, cascadeDelete: true)
                .Index(t => t.ProgramId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.EmploymentCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmpCategory = c.Int(nullable: false),
                        ProgramId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        ApplicationUserId = c.String(maxLength: 128),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Programs", t => t.ProgramId, cascadeDelete: true)
                .Index(t => t.ProgramId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.EmploymentNatures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmpNature = c.Int(nullable: false),
                        ProgramId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        ApplicationUserId = c.String(maxLength: 128),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Programs", t => t.ProgramId, cascadeDelete: true)
                .Index(t => t.ProgramId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.Organizers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        ApplicationUserId = c.String(maxLength: 128),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProgramId = c.Int(nullable: false),
                        WorkSpaceId = c.Guid(nullable: false),
                        Title = c.String(),
                        value = c.Double(nullable: false),
                        ChequeNo = c.Int(),
                        ChequeFile = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        ApplicationUserId = c.String(maxLength: 128),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Programs", t => t.ProgramId, cascadeDelete: true)
                .Index(t => t.ProgramId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.ProgramAssignments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Guid(nullable: false),
                        EmployeeVersionId = c.Guid(nullable: false),
                        EPFNo = c.String(),
                        MemberType = c.Int(nullable: false),
                        ProgramId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        ApplicationUserId = c.String(maxLength: 128),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Programs", t => t.ProgramId, cascadeDelete: true)
                .Index(t => t.ProgramId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.Requirements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ProgramId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        ApplicationUserId = c.String(maxLength: 128),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Programs", t => t.ProgramId, cascadeDelete: true)
                .Index(t => t.ProgramId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.ResourcePersons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Designation = c.String(nullable: false),
                        Cost = c.Double(nullable: false),
                        ProgramId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        ApplicationUserId = c.String(maxLength: 128),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Programs", t => t.ProgramId, cascadeDelete: true)
                .Index(t => t.ProgramId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.TargetGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ProgramId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        ApplicationUserId = c.String(maxLength: 128),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Programs", t => t.ProgramId, cascadeDelete: true)
                .Index(t => t.ProgramId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.DefaultColumns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ColumnName = c.Int(nullable: false),
                        TemplateId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        ApplicationUserId = c.String(maxLength: 128),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Templates", t => t.TemplateId, cascadeDelete: true)
                .Index(t => t.TemplateId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.Templates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Details = c.String(),
                        FileName = c.String(),
                        ProgramType = c.Int(nullable: false),
                        FileType = c.Int(nullable: false),
                        FileMethod = c.Int(nullable: false),
                        OriginalFileName = c.String(),
                        HasConfigurableTable = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        ApplicationUserId = c.String(maxLength: 128),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DefaultColumns", "TemplateId", "dbo.Templates");
            DropForeignKey("dbo.Templates", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.DefaultColumns", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.TargetGroups", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.TargetGroups", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ResourcePersons", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.ResourcePersons", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Requirements", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.Requirements", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProgramAssignments", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.ProgramAssignments", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Payments", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.Payments", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Programs", "OrganizerId", "dbo.Organizers");
            DropForeignKey("dbo.Organizers", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.EmploymentNatures", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.EmploymentNatures", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.EmploymentCategories", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.EmploymentCategories", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Documents", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.Documents", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Programs", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Costs", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.Costs", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Brochures", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.Agenda", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.Brochures", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Agenda", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Templates", new[] { "ApplicationUserId" });
            DropIndex("dbo.DefaultColumns", new[] { "ApplicationUserId" });
            DropIndex("dbo.DefaultColumns", new[] { "TemplateId" });
            DropIndex("dbo.TargetGroups", new[] { "ApplicationUserId" });
            DropIndex("dbo.TargetGroups", new[] { "ProgramId" });
            DropIndex("dbo.ResourcePersons", new[] { "ApplicationUserId" });
            DropIndex("dbo.ResourcePersons", new[] { "ProgramId" });
            DropIndex("dbo.Requirements", new[] { "ApplicationUserId" });
            DropIndex("dbo.Requirements", new[] { "ProgramId" });
            DropIndex("dbo.ProgramAssignments", new[] { "ApplicationUserId" });
            DropIndex("dbo.ProgramAssignments", new[] { "ProgramId" });
            DropIndex("dbo.Payments", new[] { "ApplicationUserId" });
            DropIndex("dbo.Payments", new[] { "ProgramId" });
            DropIndex("dbo.Organizers", new[] { "ApplicationUserId" });
            DropIndex("dbo.EmploymentNatures", new[] { "ApplicationUserId" });
            DropIndex("dbo.EmploymentNatures", new[] { "ProgramId" });
            DropIndex("dbo.EmploymentCategories", new[] { "ApplicationUserId" });
            DropIndex("dbo.EmploymentCategories", new[] { "ProgramId" });
            DropIndex("dbo.Documents", new[] { "ApplicationUserId" });
            DropIndex("dbo.Documents", new[] { "ProgramId" });
            DropIndex("dbo.Costs", new[] { "ApplicationUserId" });
            DropIndex("dbo.Costs", new[] { "ProgramId" });
            DropIndex("dbo.Programs", new[] { "ApplicationUserId" });
            DropIndex("dbo.Programs", new[] { "OrganizerId" });
            DropIndex("dbo.Brochures", new[] { "ApplicationUserId" });
            DropIndex("dbo.Brochures", new[] { "ProgramId" });
            DropIndex("dbo.Agenda", new[] { "ApplicationUserId" });
            DropIndex("dbo.Agenda", new[] { "ProgramId" });
            DropTable("dbo.Templates");
            DropTable("dbo.DefaultColumns");
            DropTable("dbo.TargetGroups");
            DropTable("dbo.ResourcePersons");
            DropTable("dbo.Requirements");
            DropTable("dbo.ProgramAssignments");
            DropTable("dbo.Payments");
            DropTable("dbo.Organizers");
            DropTable("dbo.EmploymentNatures");
            DropTable("dbo.EmploymentCategories");
            DropTable("dbo.Documents");
            DropTable("dbo.Costs");
            DropTable("dbo.Programs");
            DropTable("dbo.Brochures");
            DropTable("dbo.Agenda");
        }
    }
}
