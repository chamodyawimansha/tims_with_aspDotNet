namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class resourcePersonAndProgramRelationshipChanged : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProgramResourcePersons", "ResourcePersonId", "dbo.ResourcePersons");
            DropIndex("dbo.ProgramResourcePersons", new[] { "ResourcePersonId" });
            DropIndex("dbo.ProgramResourcePersons", new[] { "ProgramId" });
            AddColumn("dbo.ResourcePersons", "ProgramId", c => c.Int(nullable: false));
            CreateIndex("dbo.ResourcePersons", "ProgramId");
            DropTable("dbo.ProgramResourcePersons");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProgramResourcePersons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ResourcePersonId = c.Int(nullable: false),
                        ProgramId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        CreatedBy = c.String(),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            DropIndex("dbo.ResourcePersons", new[] { "ProgramId" });
            DropColumn("dbo.ResourcePersons", "ProgramId");
            CreateIndex("dbo.ProgramResourcePersons", "ProgramId");
            CreateIndex("dbo.ProgramResourcePersons", "ResourcePersonId");
            AddForeignKey("dbo.ProgramResourcePersons", "ResourcePersonId", "dbo.ResourcePersons", "Id", cascadeDelete: true);
        }
    }
}
