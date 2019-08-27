namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdontKnowWhatTheHackIsGoinOn : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(),
                        EmployeeId = c.Guid(nullable: false),
                        EPFNo = c.String(),
                        Title = c.Int(),
                        NameWithInitial = c.String(),
                        FullName = c.String(),
                        NIC = c.String(),
                        WorkSpaceName = c.String(),
                        WorkSpaceType = c.String(),
                        DesignationName = c.String(),
                        EmployeeRecruitmentType = c.Int(),
                        EmpStatus = c.Int(),
                        DateOfAppointment = c.DateTime(),
                        DateOfJoint = c.DateTime(),
                        NatureOfAppointment = c.String(),
                        TypeOfContract = c.String(),
                        OfficeEmail = c.String(),
                        MobileNumber = c.String(),
                        PrivateEmail = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Employees");
        }
    }
}
