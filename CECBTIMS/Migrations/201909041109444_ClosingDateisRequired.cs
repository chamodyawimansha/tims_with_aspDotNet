namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClosingDateisRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Programs", "ApplicationClosingTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Programs", "ApplicationClosingTime", c => c.DateTime());
        }
    }
}
