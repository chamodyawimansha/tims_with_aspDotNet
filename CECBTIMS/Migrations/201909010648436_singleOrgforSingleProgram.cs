namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class singleOrgforSingleProgram : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProgramArrangements", "OrganizerId", "dbo.Organizers");
            DropForeignKey("dbo.ProgramArrangements", "ProgramId", "dbo.Programs");
            DropIndex("dbo.ProgramArrangements", new[] { "ProgramId" });
            DropIndex("dbo.ProgramArrangements", new[] { "OrganizerId" });
            AddColumn("dbo.Programs", "OrganizerId", c => c.Int());
            CreateIndex("dbo.Programs", "OrganizerId");
            AddForeignKey("dbo.Programs", "OrganizerId", "dbo.Organizers", "Id");
            DropTable("dbo.ProgramArrangements");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProgramArrangements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProgramId = c.Int(nullable: false),
                        OrganizerId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        CreatedBy = c.String(),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Programs", "OrganizerId", "dbo.Organizers");
            DropIndex("dbo.Programs", new[] { "OrganizerId" });
            DropColumn("dbo.Programs", "OrganizerId");
            CreateIndex("dbo.ProgramArrangements", "OrganizerId");
            CreateIndex("dbo.ProgramArrangements", "ProgramId");
            AddForeignKey("dbo.ProgramArrangements", "ProgramId", "dbo.Programs", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ProgramArrangements", "OrganizerId", "dbo.Organizers", "Id", cascadeDelete: true);
        }
    }
}
