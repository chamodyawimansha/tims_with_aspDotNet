namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class programOrgs : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProgramArrangements", "Program_Id", "dbo.Programs");
            DropForeignKey("dbo.ProgramArrangements", "Program_Id1", "dbo.Programs");
            DropIndex("dbo.ProgramArrangements", new[] { "ProgramId" });
            DropIndex("dbo.ProgramArrangements", new[] { "Program_Id" });
            DropIndex("dbo.ProgramArrangements", new[] { "Program_Id1" });
            DropColumn("dbo.ProgramArrangements", "ProgramId");
            RenameColumn(table: "dbo.ProgramArrangements", name: "Program_Id1", newName: "ProgramId");
            AlterColumn("dbo.ProgramArrangements", "ProgramId", c => c.Int(nullable: false));
            CreateIndex("dbo.ProgramArrangements", "ProgramId");
            AddForeignKey("dbo.ProgramArrangements", "ProgramId", "dbo.Programs", "Id", cascadeDelete: true);
            DropColumn("dbo.ProgramArrangements", "Program_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProgramArrangements", "Program_Id", c => c.Int());
            DropForeignKey("dbo.ProgramArrangements", "ProgramId", "dbo.Programs");
            DropIndex("dbo.ProgramArrangements", new[] { "ProgramId" });
            AlterColumn("dbo.ProgramArrangements", "ProgramId", c => c.Int());
            RenameColumn(table: "dbo.ProgramArrangements", name: "ProgramId", newName: "Program_Id1");
            AddColumn("dbo.ProgramArrangements", "ProgramId", c => c.Int(nullable: false));
            CreateIndex("dbo.ProgramArrangements", "Program_Id1");
            CreateIndex("dbo.ProgramArrangements", "Program_Id");
            CreateIndex("dbo.ProgramArrangements", "ProgramId");
            AddForeignKey("dbo.ProgramArrangements", "Program_Id1", "dbo.Programs", "Id");
            AddForeignKey("dbo.ProgramArrangements", "Program_Id", "dbo.Programs", "Id");
        }
    }
}
