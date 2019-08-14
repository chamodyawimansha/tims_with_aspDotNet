namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FilesHasTableREquired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TimsFiles", "HasTraineeTable", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TimsFiles", "HasTraineeTable", c => c.Boolean());
        }
    }
}
