namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EnumChangeEmpCat : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EmploymentCategories", "EmpCategory_Id", "dbo.EmploymentCategories");
            DropIndex("dbo.EmploymentCategories", new[] { "EmpCategory_Id" });
            AddColumn("dbo.EmploymentCategories", "EmpCategory", c => c.Int(nullable: false));
            DropColumn("dbo.EmploymentCategories", "EmpCategory_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EmploymentCategories", "EmpCategory_Id", c => c.Int());
            DropColumn("dbo.EmploymentCategories", "EmpCategory");
            CreateIndex("dbo.EmploymentCategories", "EmpCategory_Id");
            AddForeignKey("dbo.EmploymentCategories", "EmpCategory_Id", "dbo.EmploymentCategories", "Id");
        }
    }
}
