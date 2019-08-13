namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class programTypeaddedToFiles : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TimsFiles", "ProgramType", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TimsFiles", "ProgramType");
        }
    }
}
