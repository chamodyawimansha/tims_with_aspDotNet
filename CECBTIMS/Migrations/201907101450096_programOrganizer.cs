namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class programOrganizer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProgramArrangements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProgramId = c.Int(nullable: false),
                        OrganizerId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizers", t => t.OrganizerId, cascadeDelete: true)
                .ForeignKey("dbo.Programs", t => t.ProgramId, cascadeDelete: true)
                .Index(t => t.ProgramId)
                .Index(t => t.OrganizerId);
            
            CreateTable(
                "dbo.Organizers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProgramArrangements", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.ProgramArrangements", "OrganizerId", "dbo.Organizers");
            DropIndex("dbo.ProgramArrangements", new[] { "OrganizerId" });
            DropIndex("dbo.ProgramArrangements", new[] { "ProgramId" });
            DropTable("dbo.Organizers");
            DropTable("dbo.ProgramArrangements");
        }
    }
}
