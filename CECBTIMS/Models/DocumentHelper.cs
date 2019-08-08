using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CECBTIMS.DAL;

namespace CECBTIMS.Models
{
    public class DocumentHelper
    {

        private readonly Program _program;
//        private readonly Program _Employee;

        private ApplicationDbContext db = new ApplicationDbContext();


        public DocumentHelper(int programId)
        {
            _program = GetProgram(programId);
        }

        public DocumentHelper(int programId, int employeeId)
        {
            //get program function
            //get employee function
        }

        private Program GetProgram(int programId)
        {
            return db.Programs.Find(programId);
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

    }
}