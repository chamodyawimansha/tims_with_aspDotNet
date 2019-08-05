namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OldEmpCatAndEmpNatRemoved : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProgramEmploymentCategories", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.ProgramEmploymentNatures", "EmploymentNature_Id", "dbo.EmploymentNatures");
            DropForeignKey("dbo.ProgramEmploymentNatures", "ProgramId", "dbo.Programs");
            DropIndex("dbo.ProgramEmploymentCategories", new[] { "ProgramId" });
            DropIndex("dbo.ProgramEmploymentNatures", new[] { "ProgramId" });
            DropIndex("dbo.ProgramEmploymentNatures", new[] { "EmploymentNature_Id" });
            RenameColumn(table: "dbo.EmploymentNatures", name: "EmpCategory_Id", newName: "EmpNature_Id");
            RenameIndex(table: "dbo.EmploymentNatures", name: "IX_EmpCategory_Id", newName: "IX_EmpNature_Id");
            DropTable("dbo.ProgramEmploymentCategories");
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
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        CreatedBy = c.String(),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        EmploymentNature_Id = c.Int(),
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
                .PrimaryKey(t => t.Id);
            
            RenameIndex(table: "dbo.EmploymentNatures", name: "IX_EmpNature_Id", newName: "IX_EmpCategory_Id");
            RenameColumn(table: "dbo.EmploymentNatures", name: "EmpNature_Id", newName: "EmpCategory_Id");
            CreateIndex("dbo.ProgramEmploymentNatures", "EmploymentNature_Id");
            CreateIndex("dbo.ProgramEmploymentNatures", "ProgramId");
            CreateIndex("dbo.ProgramEmploymentCategories", "ProgramId");
            AddForeignKey("dbo.ProgramEmploymentNatures", "ProgramId", "dbo.Programs", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ProgramEmploymentNatures", "EmploymentNature_Id", "dbo.EmploymentNatures", "Id");
            AddForeignKey("dbo.ProgramEmploymentCategories", "ProgramId", "dbo.Programs", "Id", cascadeDelete: true);
        }
    }
}
