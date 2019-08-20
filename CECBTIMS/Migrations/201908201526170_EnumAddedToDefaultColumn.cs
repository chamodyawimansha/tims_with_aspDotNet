namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EnumAddedToDefaultColumn : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DefaultColumns", "ColumnName", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DefaultColumns", "ColumnName", c => c.String());
        }
    }
}
