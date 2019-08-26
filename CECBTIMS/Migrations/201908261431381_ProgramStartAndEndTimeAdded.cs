namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProgramStartAndEndTimeAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Programs", "StartTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Programs", "EndTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Programs", "EndTime");
            DropColumn("dbo.Programs", "StartTime");
        }
    }
}
