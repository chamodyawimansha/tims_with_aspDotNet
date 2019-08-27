namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployeeIdAddedForDocument : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "EmployeeId", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documents", "EmployeeId");
        }
    }
}
