namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tableColumnModelAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TableColumns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TimsFileId = c.Int(nullable: false),
                        ColumnName = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        CreatedBy = c.String(),
                        UpdatedBy = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        TimsFile_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TimsFiles", t => t.TimsFile_Id)
                .Index(t => t.TimsFile_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TableColumns", "TimsFile_Id", "dbo.TimsFiles");
            DropIndex("dbo.TableColumns", new[] { "TimsFile_Id" });
            DropTable("dbo.TableColumns");
        }
    }
}
