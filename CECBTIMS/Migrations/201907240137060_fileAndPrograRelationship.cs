namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fileAndPrograRelationship : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Files", "ProgramId", c => c.Int());
            AlterColumn("dbo.Programs", "Title", c => c.String(maxLength: 255));
            AlterColumn("dbo.Programs", "Venue", c => c.String(maxLength: 255));
            AlterColumn("dbo.Programs", "NotifiedBy", c => c.String(maxLength: 255));
            CreateIndex("dbo.Files", "ProgramId");
            AddForeignKey("dbo.Files", "ProgramId", "dbo.Programs", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Files", "ProgramId", "dbo.Programs");
            DropIndex("dbo.Files", new[] { "ProgramId" });
            AlterColumn("dbo.Programs", "NotifiedBy", c => c.String());
            AlterColumn("dbo.Programs", "Venue", c => c.String());
            AlterColumn("dbo.Programs", "Title", c => c.String());
            DropColumn("dbo.Files", "ProgramId");
        }
    }
}
