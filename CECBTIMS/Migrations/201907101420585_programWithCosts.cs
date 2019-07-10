namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class programWithCosts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Costs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.Double(nullable: false),
                        ProgramId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Programs", t => t.ProgramId, cascadeDelete: true)
                .Index(t => t.ProgramId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Costs", "ProgramId", "dbo.Programs");
            DropIndex("dbo.Costs", new[] { "ProgramId" });
            DropTable("dbo.Costs");
        }
    }
}
