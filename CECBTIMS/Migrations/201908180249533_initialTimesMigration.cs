namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialTimesMigration : DbMigration
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
                        CreatedBy = c.String(),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Programs", t => t.ProgramId, cascadeDelete: true)
                .Index(t => t.ProgramId);
            
            CreateTable(
                "dbo.Programs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 255),
                        ProgramType = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
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
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        CreatedBy = c.String(),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
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
                        CreatedBy = c.String(),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Programs", t => t.ProgramId, cascadeDelete: true)
                .Index(t => t.ProgramId);
            
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
                        CreatedBy = c.String(),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Programs", t => t.ProgramId, cascadeDelete: true)
                .Index(t => t.ProgramId);
            
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
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        CreatedBy = c.String(),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Programs", t => t.ProgramId, cascadeDelete: true)
                .Index(t => t.ProgramId);
            
            CreateTable(
                "dbo.EmploymentCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmpCategory = c.Int(nullable: false),
                        ProgramId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        CreatedBy = c.String(),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Programs", t => t.ProgramId, cascadeDelete: true)
                .Index(t => t.ProgramId);
            
            CreateTable(
                "dbo.EmploymentNatures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmpNature = c.Int(nullable: false),
                        ProgramId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Programs", t => t.ProgramId, cascadeDelete: true)
                .Index(t => t.ProgramId);
            
            CreateTable(
                "dbo.ProgramArrangements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProgramId = c.Int(nullable: false),
                        OrganizerId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        CreatedBy = c.String(),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizers", t => t.OrganizerId, cascadeDelete: true)
                .ForeignKey("dbo.Programs", t => t.ProgramId, cascadeDelete: true)
                .Index(t => t.ProgramId)
                .Index(t => t.OrganizerId);
            
            CreateTable(
                "dbo.Organizers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        CreatedBy = c.String(),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProgramAssignments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Guid(nullable: false),
                        EmployeeVersionId = c.Guid(nullable: false),
                        EPFNo = c.String(),
                        ProgramId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        CreatedBy = c.String(),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Programs", t => t.ProgramId, cascadeDelete: true)
                .Index(t => t.ProgramId);
            
            CreateTable(
                "dbo.Requirements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ProgramId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        CreatedBy = c.String(),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Programs", t => t.ProgramId, cascadeDelete: true)
                .Index(t => t.ProgramId);
            
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
                        CreatedBy = c.String(),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Programs", t => t.ProgramId, cascadeDelete: true)
                .Index(t => t.ProgramId);
            
            CreateTable(
                "dbo.TargetGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ProgramId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        CreatedBy = c.String(),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Programs", t => t.ProgramId, cascadeDelete: true)
                .Index(t => t.ProgramId);
            
            CreateTable(
                "dbo.Templates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Details = c.String(),
                        FileName = c.String(),
                        ProgramType = c.Int(nullable: false),
                        FileType = c.Int(nullable: false),
                        FileMethod = c.Int(nullable: false),
                        OriginalFileName = c.String(),
                        HasConfigurableTable = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        CreatedBy = c.String(),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DefaultColumns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ColumnName = c.String(),
                        TemplateId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        CreatedBy = c.String(),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Templates", t => t.TemplateId, cascadeDelete: true)
                .Index(t => t.TemplateId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DefaultColumns", "TemplateId", "dbo.Templates");
            DropForeignKey("dbo.TargetGroups", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.ResourcePersons", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.Requirements", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.ProgramAssignments", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.ProgramArrangements", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.ProgramArrangements", "OrganizerId", "dbo.Organizers");
            DropForeignKey("dbo.EmploymentNatures", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.EmploymentCategories", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.Documents", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.Costs", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.Brochures", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.Agenda", "ProgramId", "dbo.Programs");
            DropIndex("dbo.DefaultColumns", new[] { "TemplateId" });
            DropIndex("dbo.TargetGroups", new[] { "ProgramId" });
            DropIndex("dbo.ResourcePersons", new[] { "ProgramId" });
            DropIndex("dbo.Requirements", new[] { "ProgramId" });
            DropIndex("dbo.ProgramAssignments", new[] { "ProgramId" });
            DropIndex("dbo.ProgramArrangements", new[] { "OrganizerId" });
            DropIndex("dbo.ProgramArrangements", new[] { "ProgramId" });
            DropIndex("dbo.EmploymentNatures", new[] { "ProgramId" });
            DropIndex("dbo.EmploymentCategories", new[] { "ProgramId" });
            DropIndex("dbo.Documents", new[] { "ProgramId" });
            DropIndex("dbo.Costs", new[] { "ProgramId" });
            DropIndex("dbo.Brochures", new[] { "ProgramId" });
            DropIndex("dbo.Agenda", new[] { "ProgramId" });
            DropTable("dbo.DefaultColumns");
            DropTable("dbo.Templates");
            DropTable("dbo.TargetGroups");
            DropTable("dbo.ResourcePersons");
            DropTable("dbo.Requirements");
            DropTable("dbo.ProgramAssignments");
            DropTable("dbo.Organizers");
            DropTable("dbo.ProgramArrangements");
            DropTable("dbo.EmploymentNatures");
            DropTable("dbo.EmploymentCategories");
            DropTable("dbo.Documents");
            DropTable("dbo.Costs");
            DropTable("dbo.Brochures");
            DropTable("dbo.Programs");
            DropTable("dbo.Agenda");
        }
    }
}
