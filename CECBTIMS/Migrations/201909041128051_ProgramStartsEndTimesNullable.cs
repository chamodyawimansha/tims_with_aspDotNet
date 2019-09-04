namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProgramStartsEndTimesNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Programs", "StartTime", c => c.DateTime());
            AlterColumn("dbo.Programs", "EndTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Programs", "EndTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Programs", "StartTime", c => c.DateTime(nullable: false));
        }
    }
}
