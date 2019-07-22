namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class programAssignmentModel : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Employees");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        EmployeeId = c.Guid(nullable: false),
                        EPFNo = c.String(),
                        Title = c.Int(),
                        NameWithInitial = c.String(),
                        FullName = c.String(),
                        NIC = c.String(),
                        WorkSpaceName = c.String(),
                        DesignationName = c.String(),
                        EmployeeRecruitmentType = c.Int(),
                        EmpStatus = c.Int(),
                        DateOfAppointment = c.DateTime(),
                        TypeOfContract = c.String(),
                        OfficeEmail = c.String(),
                        MobileNumber = c.String(),
                        PrivateEmail = c.String(),
                    })
                .PrimaryKey(t => t.EmployeeId);
            
        }
    }
}
