namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmpCatAndEmpNat : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ProgramEmploymentCategories", newName: "EmploymentCategories");
            DropIndex("dbo.ProgramEmploymentNatures", new[] { "ProgramId" });
            CreateTable(
                "dbo.EmploymentNatures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmpNatures = c.Int(nullable: false),
                        ProgramId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Programs", t => t.ProgramId, cascadeDelete: true)
                .Index(t => t.ProgramId);
            
            AddColumn("dbo.EmploymentCategories", "EmpCategory", c => c.Int(nullable: false));
            DropColumn("dbo.EmploymentCategories", "EmployeeCategory");
            DropTable("dbo.ProgramEmploymentNatures");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.EmploymentCategories", "EmployeeCategory", c => c.Int(nullable: false));
            DropForeignKey("dbo.EmploymentNatures", "ProgramId", "dbo.Programs");
            DropIndex("dbo.EmploymentNatures", new[] { "ProgramId" });
            DropColumn("dbo.EmploymentCategories", "EmpCategory");
            DropTable("dbo.EmploymentNatures");
            CreateIndex("dbo.ProgramEmploymentNatures", "ProgramId");
            RenameTable(name: "dbo.EmploymentCategories", newName: "ProgramEmploymentCategories");
        }
    }
}
