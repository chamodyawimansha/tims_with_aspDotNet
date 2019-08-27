namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MemberTypeAddedTothePrograAssignment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProgramAssignments", "MemberType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProgramAssignments", "MemberType");
        }
    }
}
