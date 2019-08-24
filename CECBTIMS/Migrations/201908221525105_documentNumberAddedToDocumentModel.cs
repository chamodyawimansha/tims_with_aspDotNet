namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class documentNumberAddedToDocumentModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "DocumentNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documents", "DocumentNumber");
        }
    }
}
