namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class programModelMigration : DbMigration
    {
        public override void Up()
        {
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
                "dbo.Programs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        ProgramType = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        ApplicationClosingDate = c.DateTime(nullable: false),
                        ApplicationClosingTime = c.Byte(nullable: false),
                        Brochure = c.String(),
                        EmploymentNature = c.Int(),
                        EmployeeCategory = c.Int(),
                        Venue = c.String(),
                        EndDate = c.DateTime(),
                        NotifiedBy = c.String(),
                        NotifiedOn = c.DateTime(),
                        ProgramHours = c.Int(),
                        DurationInDays = c.Int(),
                        DurationInMonths = c.Int(),
                        Department = c.String(),
                        Currency = c.Int(nullable: false),
                        ProgramFee = c.Double(),
                        RegistrationFee = c.Double(),
                        PerPersonFee = c.Double(),
                        NoShowFee = c.Double(),
                        MemberFee = c.Double(),
                        NonMemberFee = c.Double(),
                        StudentFee = c.Double(),
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
                "dbo.Requirements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.Int(nullable: false),
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
                "dbo.ProgramResourcePersons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ResourcePersonId = c.Int(nullable: false),
                        ProgramId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        CreatedBy = c.String(),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        ResourcePerson_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Programs", t => t.ProgramId, cascadeDelete: true)
                .ForeignKey("dbo.ResourcePersons", t => t.ResourcePerson_Id)
                .Index(t => t.ProgramId)
                .Index(t => t.ResourcePerson_Id);
            
            CreateTable(
                "dbo.ResourcePersons",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Designation = c.String(),
                        Cost = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        CreatedBy = c.String(),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
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
            DropForeignKey("dbo.ProgramResourcePersons", "ResourcePerson_Id", "dbo.ResourcePersons");
            DropForeignKey("dbo.ProgramResourcePersons", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.Requirements", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.ProgramArrangements", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.Costs", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.ProgramArrangements", "OrganizerId", "dbo.Organizers");
            DropIndex("dbo.TargetGroups", new[] { "ProgramId" });
            DropIndex("dbo.ProgramResourcePersons", new[] { "ResourcePerson_Id" });
            DropIndex("dbo.ProgramResourcePersons", new[] { "ProgramId" });
            DropIndex("dbo.Requirements", new[] { "ProgramId" });
            DropIndex("dbo.Costs", new[] { "ProgramId" });
            DropIndex("dbo.ProgramArrangements", new[] { "OrganizerId" });
            DropIndex("dbo.ProgramArrangements", new[] { "ProgramId" });
            DropTable("dbo.TargetGroups");
            DropTable("dbo.ResourcePersons");
            DropTable("dbo.ProgramResourcePersons");
            DropTable("dbo.Requirements");
            DropTable("dbo.Costs");
            DropTable("dbo.Programs");
            DropTable("dbo.ProgramArrangements");
            DropTable("dbo.Organizers");
        }
    }
}
