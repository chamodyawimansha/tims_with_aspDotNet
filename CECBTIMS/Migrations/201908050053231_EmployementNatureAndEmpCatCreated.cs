namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployementNatureAndEmpCatCreated : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmploymentCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProgramId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        CreatedBy = c.String(),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        EmpCategory_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EmploymentCategories", t => t.EmpCategory_Id)
                .ForeignKey("dbo.Programs", t => t.ProgramId, cascadeDelete: true)
                .Index(t => t.ProgramId)
                .Index(t => t.EmpCategory_Id);
            
            CreateTable(
                "dbo.EmploymentNatures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProgramId = c.Int(nullable: false),
                        EmpCategory_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EmploymentNatures", t => t.EmpCategory_Id)
                .ForeignKey("dbo.Programs", t => t.ProgramId, cascadeDelete: true)
                .Index(t => t.ProgramId)
                .Index(t => t.EmpCategory_Id);
            
            AddColumn("dbo.ProgramEmploymentNatures", "EmploymentNature_Id", c => c.Int());
            CreateIndex("dbo.ProgramEmploymentNatures", "EmploymentNature_Id");
            AddForeignKey("dbo.ProgramEmploymentNatures", "EmploymentNature_Id", "dbo.EmploymentNatures", "Id");
            DropColumn("dbo.ProgramEmploymentNatures", "EmploymentNature");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProgramEmploymentNatures", "EmploymentNature", c => c.Int(nullable: false));
            DropForeignKey("dbo.ProgramEmploymentNatures", "EmploymentNature_Id", "dbo.EmploymentNatures");
            DropForeignKey("dbo.EmploymentNatures", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.EmploymentNatures", "EmpCategory_Id", "dbo.EmploymentNatures");
            DropForeignKey("dbo.EmploymentCategories", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.EmploymentCategories", "EmpCategory_Id", "dbo.EmploymentCategories");
            DropIndex("dbo.ProgramEmploymentNatures", new[] { "EmploymentNature_Id" });
            DropIndex("dbo.EmploymentNatures", new[] { "EmpCategory_Id" });
            DropIndex("dbo.EmploymentNatures", new[] { "ProgramId" });
            DropIndex("dbo.EmploymentCategories", new[] { "EmpCategory_Id" });
            DropIndex("dbo.EmploymentCategories", new[] { "ProgramId" });
            DropColumn("dbo.ProgramEmploymentNatures", "EmploymentNature_Id");
            DropTable("dbo.EmploymentNatures");
            DropTable("dbo.EmploymentCategories");
        }
    }
}
