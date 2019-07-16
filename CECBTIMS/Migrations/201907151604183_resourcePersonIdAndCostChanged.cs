namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class resourcePersonIdAndCostChanged : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProgramResourcePersons", "ResourcePerson_Id", "dbo.ResourcePersons");
            DropIndex("dbo.ProgramResourcePersons", new[] { "ResourcePerson_Id" });
            DropColumn("dbo.ProgramResourcePersons", "ResourcePersonId");
            RenameColumn(table: "dbo.ProgramResourcePersons", name: "ResourcePerson_Id", newName: "ResourcePersonId");
            DropPrimaryKey("dbo.ResourcePersons");
            AlterColumn("dbo.ProgramResourcePersons", "ResourcePersonId", c => c.Int(nullable: false));
            AlterColumn("dbo.ResourcePersons", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.ResourcePersons", "Cost", c => c.Double(nullable: false));
            AddPrimaryKey("dbo.ResourcePersons", "Id");
            CreateIndex("dbo.ProgramResourcePersons", "ResourcePersonId");
            AddForeignKey("dbo.ProgramResourcePersons", "ResourcePersonId", "dbo.ResourcePersons", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProgramResourcePersons", "ResourcePersonId", "dbo.ResourcePersons");
            DropIndex("dbo.ProgramResourcePersons", new[] { "ResourcePersonId" });
            DropPrimaryKey("dbo.ResourcePersons");
            AlterColumn("dbo.ResourcePersons", "Cost", c => c.String());
            AlterColumn("dbo.ResourcePersons", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.ProgramResourcePersons", "ResourcePersonId", c => c.String(maxLength: 128));
            AddPrimaryKey("dbo.ResourcePersons", "Id");
            RenameColumn(table: "dbo.ProgramResourcePersons", name: "ResourcePersonId", newName: "ResourcePerson_Id");
            AddColumn("dbo.ProgramResourcePersons", "ResourcePersonId", c => c.Int(nullable: false));
            CreateIndex("dbo.ProgramResourcePersons", "ResourcePerson_Id");
            AddForeignKey("dbo.ProgramResourcePersons", "ResourcePerson_Id", "dbo.ResourcePersons", "Id");
        }
    }
}
