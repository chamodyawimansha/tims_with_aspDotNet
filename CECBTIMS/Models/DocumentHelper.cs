using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.EntitySql;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CECBTIMS.Controllers;
using CECBTIMS.DAL;
using CECBTIMS.Models.Enums;

namespace CECBTIMS.Models
{
    public class DocumentHelper
    {
        private readonly Program _program;
        private readonly List<Employee> _traineeList;

        private ApplicationDbContext db = new ApplicationDbContext();
        private CECB_ERPEntities cecb_db = new CECB_ERPEntities();

        public DocumentHelper(int programId)
        {
            _program = GetProgram(programId);
            _traineeList = GetTrainees();
        }

        /**
         * get the current program
         */
        private Program GetProgram(int programId)
        {
            return db.Programs.Find(programId);
        }

        /**
         * Get the current program's trainee list
         */
        private List<Employee> GetTrainees()
        {
            var programAssignments = _program.ProgramAssignments;
            var traineeList = new List<Employee>();

//            if (programAssignments == null) return traineeList = null;

            foreach (var item in programAssignments)
            {
                var employee = EmployeesController.FindEmployee(item.EmployeeVersionId);
                if (employee != null)
                {
                    traineeList.Add(employee);
                }
            }
            
            return traineeList;
        }

        // return the file number
        public string GetFilenumber()
        {
            return "";
        }
        //returns current year
        public string GetYear()
        {
            return DateTime.Now.ToString("yyyy");
        }
        //returns today
        public string GetToday()
        {
            return DateTime.Now.ToString("yyyy.MM.dd");
        }
        //returns the program title
        public string GetProgramtitle()
        {
            return _program.Title;
        }
        // get organisers of the program
        public string GetOrganisedby()
        {
            return _program.ProgramArrangements.Aggregate("", (current, item) => current == "" ? item.Organizer.Name : current + ", " + item.Organizer.Name);
        }
        //returns program start date
        public string GetStartdate()
        {
            return _program.StartDate.ToString("yyyy.MM.dd");
        }
        //returns venue of the program
        public string GetVenue()
        {
            return _program.Venue;
        }
        // retuns member fee
        public string GetMemberfee()
        {
            return _program.MemberFee.ToString();
        }
        // returns non member fee
        public string GetNonmemberfee()
        {
            return _program.NonMemberFee.ToString();
        }
        // retuns student fee
        public string GetStudentfee()
        {
            return _program.StudentFee.ToString();
        }

        public List<Employee> GetEmployess()
        {
            return this._traineeList;
        }
        /**
         * function to get trainee data
         */    

        public string GetEmployeeId(Employee employee)
        {
            return employee.EmployeeId.ToString();
        }
        public string GetEPFNo(Employee employee)
        {
            return employee.EPFNo;
        }
        public string GetTitle(Employee employee)
        {
            return employee.Title.ToString();
        }
        public string GetName(Employee employee)
        {
            return employee.FullName;
        }
        public string GetNameShort(Employee employee)
        {
            return employee.NameWithInitial;
        }
        public string GetNameWithTitle(Employee employee)
        {
            return employee.Title + ". " +employee.NameWithInitial;
        }
        public string GetNIC(Employee employee)
        {
            return employee.NIC;
        }






        public string GetWorkSpaceName(Employee employee)
        {
            return employee.FullName;
        }

        public string GetDesignationName(Employee employee)
        {
            return employee.FullName;
        }

        public string GetEmployeeRecruitmentType(Employee employee)
        {
            return employee.FullName;
        }
        public string GetEmpStatus(Employee employee)
        {
            return employee.FullName;
        }
        public string GetDateOfAppointment(Employee employee)
        {
            return employee.FullName;
        }
        public string GetTypeOfContract(Employee employee)
        {
            return employee.FullName;
        }
        public string GetOfficeEmail(Employee employee)
        {
            return employee.FullName;
        }

        public string GetMobileNumber(Employee employee)
        {
            return employee.FullName;
        }

        public string GetPrivateEmail(Employee employee)
        {
            return employee.FullName;
        }


    }
}