namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class programWithResourcePersonRelationship : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProgramResourcePersons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ResourcePersonId = c.Int(nullable: false),
                        ProgramId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        ResourcePerson_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Programs", t => t.ProgramId, cascadeDelete: true)
                .ForeignKey("dbo.ResourcePersons", t => t.ResourcePerson_Id)
                .Index(t => t.ProgramId)
                .Index(t => t.ResourcePerson_Id);
            
            CreateTable(
                "dbo.ResourcePersons",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Designation = c.String(),
                        Cost = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProgramResourcePersons", "ResourcePerson_Id", "dbo.ResourcePersons");
            DropForeignKey("dbo.ProgramResourcePersons", "ProgramId", "dbo.Programs");
            DropIndex("dbo.ProgramResourcePersons", new[] { "ResourcePerson_Id" });
            DropIndex("dbo.ProgramResourcePersons", new[] { "ProgramId" });
            DropTable("dbo.ResourcePersons");
            DropTable("dbo.ProgramResourcePersons");
        }
    }
}
