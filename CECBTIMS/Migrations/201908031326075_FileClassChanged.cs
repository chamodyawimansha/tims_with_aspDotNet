namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileClassChanged : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Files", newName: "TimsFiles");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.TimsFiles", newName: "Files");
        }
    }
}
