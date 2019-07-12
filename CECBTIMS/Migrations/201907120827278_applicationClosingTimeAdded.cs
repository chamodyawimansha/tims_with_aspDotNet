namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class applicationClosingTimeAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Programs", "ApplicationClosingDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Programs", "ApplicationClosingTime", c => c.Byte(nullable: false));
            DropColumn("dbo.Programs", "ApplicationClosingDateTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Programs", "ApplicationClosingDateTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Programs", "ApplicationClosingTime");
            DropColumn("dbo.Programs", "ApplicationClosingDate");
        }
    }
}
