namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialTimsMigration : DbMigration
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
                        Title = c.String(),
                        ProgramType = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        ApplicationClosingDate = c.DateTime(nullable: false),
                        ApplicationClosingTime = c.DateTime(nullable: false),
                        Brochure = c.String(),
                        Venue = c.String(),
                        EndDate = c.DateTime(),
                        NotifiedBy = c.String(),
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
                "dbo.ProgramEmploymentCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProgramId = c.Int(nullable: false),
                        EmployeeCategory = c.Int(nullable: false),
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
                "dbo.ProgramEmploymentNatures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProgramId = c.Int(nullable: false),
                        EmploymentNature = c.Int(nullable: false),
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TargetGroups", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.ResourcePersons", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.Requirements", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.ProgramEmploymentNatures", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.ProgramEmploymentCategories", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.ProgramArrangements", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.ProgramArrangements", "OrganizerId", "dbo.Organizers");
            DropForeignKey("dbo.Costs", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.Agenda", "ProgramId", "dbo.Programs");
            DropIndex("dbo.TargetGroups", new[] { "ProgramId" });
            DropIndex("dbo.ResourcePersons", new[] { "ProgramId" });
            DropIndex("dbo.Requirements", new[] { "ProgramId" });
            DropIndex("dbo.ProgramEmploymentNatures", new[] { "ProgramId" });
            DropIndex("dbo.ProgramEmploymentCategories", new[] { "ProgramId" });
            DropIndex("dbo.ProgramArrangements", new[] { "OrganizerId" });
            DropIndex("dbo.ProgramArrangements", new[] { "ProgramId" });
            DropIndex("dbo.Costs", new[] { "ProgramId" });
            DropIndex("dbo.Agenda", new[] { "ProgramId" });
            DropTable("dbo.TargetGroups");
            DropTable("dbo.ResourcePersons");
            DropTable("dbo.Requirements");
            DropTable("dbo.ProgramEmploymentNatures");
            DropTable("dbo.ProgramEmploymentCategories");
            DropTable("dbo.Organizers");
            DropTable("dbo.ProgramArrangements");
            DropTable("dbo.Costs");
            DropTable("dbo.Programs");
            DropTable("dbo.Agenda");
        }
    }
}
