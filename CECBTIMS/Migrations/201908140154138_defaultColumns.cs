namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class defaultColumns : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TableColumns", "TimsFile_Id", "dbo.TimsFiles");
            DropIndex("dbo.TableColumns", new[] { "TimsFile_Id" });
            CreateTable(
                "dbo.DefaultColumns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TimsFileId = c.Int(nullable: false),
                        ColumnName = c.Int(nullable: false),
                        TimsFile_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TimsFiles", t => t.TimsFile_Id)
                .Index(t => t.TimsFile_Id);
            
            AddColumn("dbo.TimsFiles", "HasTraineeTable", c => c.Boolean());
            DropTable("dbo.TableColumns");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.DefaultColumns", "TimsFile_Id", "dbo.TimsFiles");
            DropIndex("dbo.DefaultColumns", new[] { "TimsFile_Id" });
            DropColumn("dbo.TimsFiles", "HasTraineeTable");
            DropTable("dbo.DefaultColumns");
            CreateIndex("dbo.TableColumns", "TimsFile_Id");
            AddForeignKey("dbo.TableColumns", "TimsFile_Id", "dbo.TimsFiles", "Id");
        }
    }
}
