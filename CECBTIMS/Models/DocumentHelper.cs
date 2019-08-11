using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.EntitySql;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CECBTIMS.Controllers;
using CECBTIMS.DAL;

namespace CECBTIMS.Models
{
    public class DocumentHelper
    {
        private readonly Program _program;
        private readonly List<Employee> _TraineeList;

        private ApplicationDbContext db = new ApplicationDbContext();
        private CECB_ERPEntities cecb_db = new CECB_ERPEntities();

        public DocumentHelper(int programId)
        {
            _program = GetProgram(programId);
            _TraineeList = GetTrainees();
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
            List<Employee> traineeList = null;

            if (programAssignments == null) return traineeList = null;

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


        //Test function
        public string GetName()
        {
            // this is a array cant get one
            // add getter method to return the list of trainees
            return _TraineeList.;
        }
        public string GetAge()
        {
            return "24";
        }



        /**
         * function to get trainee data
         */

        public string GetName(Employee employee)
        {

        }


    }
}