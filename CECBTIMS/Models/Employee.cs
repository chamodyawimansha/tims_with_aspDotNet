using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CECBTIMS.Models.Enums;

namespace CECBTIMS.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public System.Guid EmployeeId { get; set; }
        public string EPFNo { get; set; }
        public Title? Title { get; set; }
        public string NameWithInitial { get; set; }
        public string FullName { get; set; }
        public string NIC { get; set; }
        public string WorkSpaceName { get; set; }
        public string WorkSpaceType { get; set; }
        public string DesignationName { get; set; }
        public string Grade { get; set; }
        public RecruitmentType? EmployeeRecruitmentType { get; set; } // Nature Of Appointment
        public EmployeeStatus? EmpStatus { get; set; }
        public DateTime? DateOfAppointment { get; set; }
        public DateTime? DateOfJoint { get; set; }
        public string TypeOfContract { get; set; }
        public string OfficeEmail { get; set; }
        public string MobileNumber { get; set; }
        public string PrivateEmail { get; set; }
    }
}