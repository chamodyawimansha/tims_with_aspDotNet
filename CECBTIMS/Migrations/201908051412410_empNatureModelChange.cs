namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class empNatureModelChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmploymentNatures", "EmpNature", c => c.Int(nullable: false));
            DropColumn("dbo.EmploymentNatures", "EmpNatures");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EmploymentNatures", "EmpNatures", c => c.Int(nullable: false));
            DropColumn("dbo.EmploymentNatures", "EmpNature");
        }
    }
}
