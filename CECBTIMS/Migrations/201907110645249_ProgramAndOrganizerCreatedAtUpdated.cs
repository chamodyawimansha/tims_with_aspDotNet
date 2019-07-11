namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProgramAndOrganizerCreatedAtUpdated : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Programs", "CreatedAt", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Programs", "CreatedAt", c => c.DateTime(nullable: false));
        }
    }
}
