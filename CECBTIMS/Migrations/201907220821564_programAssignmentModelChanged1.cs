namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class programAssignmentModelChanged1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProgramAssignments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Guid(nullable: false),
                        EmployeeVersionId = c.Guid(nullable: false),
                        EPFNo = c.String(),
                        ProgramId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        CreatedBy = c.String(),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Programs", t => t.ProgramId, cascadeDelete: true)
                .Index(t => t.ProgramId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProgramAssignments", "ProgramId", "dbo.Programs");
            DropIndex("dbo.ProgramAssignments", new[] { "ProgramId" });
            DropTable("dbo.ProgramAssignments");
        }
    }
}
