namespace CECBTIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProAssAndEmp : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProgramArrangements", "ProgramId", "dbo.Programs");
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
            
            AddColumn("dbo.ProgramArrangements", "Program_Id", c => c.Int());
            AddColumn("dbo.ProgramArrangements", "Program_Id1", c => c.Int());
            CreateIndex("dbo.ProgramArrangements", "Program_Id");
            CreateIndex("dbo.ProgramArrangements", "Program_Id1");
            AddForeignKey("dbo.ProgramArrangements", "Program_Id1", "dbo.Programs", "Id");
            AddForeignKey("dbo.ProgramArrangements", "Program_Id", "dbo.Programs", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProgramArrangements", "Program_Id", "dbo.Programs");
            DropForeignKey("dbo.ProgramArrangements", "Program_Id1", "dbo.Programs");
            DropIndex("dbo.ProgramArrangements", new[] { "Program_Id1" });
            DropIndex("dbo.ProgramArrangements", new[] { "Program_Id" });
            DropColumn("dbo.ProgramArrangements", "Program_Id1");
            DropColumn("dbo.ProgramArrangements", "Program_Id");
            DropTable("dbo.Employees");
            AddForeignKey("dbo.ProgramArrangements", "ProgramId", "dbo.Programs", "Id", cascadeDelete: true);
        }
    }
}
