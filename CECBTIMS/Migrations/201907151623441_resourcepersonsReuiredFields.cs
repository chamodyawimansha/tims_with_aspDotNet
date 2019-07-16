namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class resourcepersonsReuiredFields : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ResourcePersons", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.ResourcePersons", "Designation", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ResourcePersons", "Designation", c => c.String());
            AlterColumn("dbo.ResourcePersons", "Name", c => c.String());
        }
    }
}
