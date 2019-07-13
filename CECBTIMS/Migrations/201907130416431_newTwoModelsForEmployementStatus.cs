namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newTwoModelsForEmployementStatus : DbMigration
    {
        public override void Up()
        {
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
            
            AlterColumn("dbo.Programs", "ProgramHours", c => c.Byte());
            AlterColumn("dbo.Programs", "DurationInDays", c => c.Byte());
            AlterColumn("dbo.Programs", "DurationInMonths", c => c.Byte());
            AlterColumn("dbo.Programs", "RegistrationFee", c => c.Single());
            AlterColumn("dbo.Programs", "PerPersonFee", c => c.Single());
            AlterColumn("dbo.Programs", "NoShowFee", c => c.Single());
            AlterColumn("dbo.Programs", "MemberFee", c => c.Single());
            AlterColumn("dbo.Programs", "NonMemberFee", c => c.Single());
            AlterColumn("dbo.Programs", "StudentFee", c => c.Single());
            DropColumn("dbo.Programs", "EmploymentNature");
            DropColumn("dbo.Programs", "EmployeeCategory");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Programs", "EmployeeCategory", c => c.Int());
            AddColumn("dbo.Programs", "EmploymentNature", c => c.Int());
            DropForeignKey("dbo.ProgramEmploymentNatures", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.ProgramEmploymentCategories", "ProgramId", "dbo.Programs");
            DropIndex("dbo.ProgramEmploymentNatures", new[] { "ProgramId" });
            DropIndex("dbo.ProgramEmploymentCategories", new[] { "ProgramId" });
            AlterColumn("dbo.Programs", "StudentFee", c => c.Double());
            AlterColumn("dbo.Programs", "NonMemberFee", c => c.Double());
            AlterColumn("dbo.Programs", "MemberFee", c => c.Double());
            AlterColumn("dbo.Programs", "NoShowFee", c => c.Double());
            AlterColumn("dbo.Programs", "PerPersonFee", c => c.Double());
            AlterColumn("dbo.Programs", "RegistrationFee", c => c.Double());
            AlterColumn("dbo.Programs", "DurationInMonths", c => c.Int());
            AlterColumn("dbo.Programs", "DurationInDays", c => c.Int());
            AlterColumn("dbo.Programs", "ProgramHours", c => c.Int());
            DropTable("dbo.ProgramEmploymentNatures");
            DropTable("dbo.ProgramEmploymentCategories");
        }
    }
}
