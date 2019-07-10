namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class programModelWithoutRelationships : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Programs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        ProgramType = c.Int(nullable: false),
                        TargetGroup = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        ApplicationClosingDateTime = c.DateTime(nullable: false),
                        Brochure = c.String(),
                        EmploymentNature = c.Int(),
                        EmployeeCategory = c.Int(),
                        Venue = c.String(),
                        EndDate = c.DateTime(),
                        NotifiedBy = c.String(),
                        NotifiedOn = c.DateTime(),
                        Requirements = c.String(),
                        ProgramHours = c.Int(),
                        DurationInDays = c.Int(),
                        DurationInMonths = c.Int(),
                        Department = c.String(),
                        CreatedBy = c.String(),
                        UpdatedBy = c.String(),
                        UpdatedAt = c.DateTime(),
                        CreatedAt = c.DateTime(nullable: false),
                        Currency = c.Int(nullable: false),
                        ProgramFee = c.Double(),
                        RegistrationFee = c.Double(),
                        PerPersonFee = c.Double(),
                        NoShowFee = c.Double(),
                        MemberFee = c.Double(),
                        NonMemberFee = c.Double(),
                        StudentFee = c.Double(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Programs");
        }
    }
}
