namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MemberTypeNotREquired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProgramAssignments", "MemberType", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProgramAssignments", "MemberType", c => c.Int(nullable: false));
        }
    }
}
