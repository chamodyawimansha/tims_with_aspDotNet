namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class templateSomeReuired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Templates", "Title", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Templates", "Title", c => c.String());
        }
    }
}
