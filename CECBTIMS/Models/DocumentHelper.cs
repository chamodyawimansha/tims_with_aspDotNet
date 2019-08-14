using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core.Common.EntitySql;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using CECBTIMS.Controllers;
using CECBTIMS.DAL;
using CECBTIMS.Models.Enums;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.Ajax.Utilities;

namespace CECBTIMS.Models
{
    public class DocumentHelper
    {
        private readonly Program _program;
        private readonly List<Employee> _traineeList;

        public DocumentHelper(int programId)
        {
            _program = GetProgram(programId);
            _traineeList = GetTrainees();
        }

        /**
         * get the current program
         */
        private static Program GetProgram(int programId)
        {
            return ProgramsController.FindProgram(programId);
        }


        public static string ToFunctionName(string name)
        {
            name = name.ToLower();
            var nameParts = name.Split(null);

            return nameParts.Aggregate("Get", (current, item) => current + (item.First().ToString().ToUpper() + item.Substring(1)));
        }

        /**
         * Get the current program's trainee list
         */
        private List<Employee> GetTrainees()
        {
            var programAssignments = _program.ProgramAssignments;
            var traineeList = new List<Employee>();

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

        public List<Employee> GetEmployess()
        {
            return this._traineeList;
        }

        // return the file number
        public string GetFileNumber()
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
        public string GetProgramTitle()
        {
            return _program.Title;
        }
        //returns program start date
        public string GetStartDate()
        {
            return _program.StartDate.ToString("yyyy.MM.dd");
        }
        public string GetApplicationClosingDate()
        {
            return _program.ApplicationClosingDate.ToString("yyyy.MM.dd");
        }
        public string GetApplicationClosingTime()
        {
            return _program.ApplicationClosingDate.ToString("HH:mm");
        }
        //returns venue of the program
        public string GetVenue()
        {
            return _program.Venue;
        }

        public string GetEndDate()
        {
            return _program.EndDate?.ToString("yyyy.MM.dd");
        }

        public string GetNotifiedBy()
        {
            return _program.NotifiedBy;
        }

        public string GetNotifiedOn()
        {
            return _program.NotifiedOn?.ToString("HH:mm");
        }

        public string GetProgramHours()
        {
            return _program.ProgramHours.ToString();
        }

        public string GetDurationInDays()
        {
            return _program.DurationInDays.ToString();
        }

        public string GetDurationInMonths()
        {
            return _program.DurationInMonths.ToString();
        }
        public string Department()
        {
            return _program.Department.ToString();
        }
        public string Currency()
        {
            return _program.Currency.ToString();
        }
        public string ProgramFee()
        {
            return _program.ProgramFee.ToString();
        }
        public string RegistrationFee()
        {
            return _program.RegistrationFee.ToString();
        }
        public string PerPersonFee()
        {
            return _program.PerPersonFee.ToString();
        }
        public string NoShowFee()
        {
            return _program.NoShowFee.ToString();
        }
        public string MemberFee()
        {
            return _program.MemberFee.ToString();
        }
        public string NonMemberFee()
        {
            return _program.NonMemberFee.ToString();
        }
        public string StudentFee()
        {
            return _program.StudentFee.ToString();
        }

        // returns agenda titles : title1, title2, title3
        public string GetAgendaTitle()
        {
            var agenda = "";
            return _program.Agendas == null ? agenda : _program.Agendas.Aggregate(agenda, (current, item) => current + (current != "" ? ", " + item.Name : item.Name));
        }

        public string[] GetAgendaTitleList()
        {
            var agendaTitles = new string[]{};
            if (_program.Agendas == null) return agendaTitles;
            var i = 0;
            foreach (var item in _program.Agendas)
            {
                agendaTitles[i] = item.Name;
                i++;
            }

            return agendaTitles;

        }

        public List<Agenda> GetAgendaTable()
        {
            return _program.Agendas.ToList();
        }
        
        public List<ResourcePerson> GetResourcePersonsTable()
        {
            return _program.ResourcePersons.ToList();
        }
//
//        Costs
//            costs List<>
//
//            Requirements
//        Requirements list
//
//        EmploymentCategories
//
//            EmploymentNatures



        // get organisers of the program
        public string GetOrganisedBy()
        {
            return _program.ProgramArrangements.Aggregate("", (current, item) => current == "" ? item.Organizer.Name : current + ", " + item.Organizer.Name);
        }


        // retuns member fee
        public string GetMemberFee()
        {
            return _program.MemberFee.ToString();
        }
        // returns non member fee
        public string GetNonmemberFee()
        {
            return _program.NonMemberFee.ToString();
        }
        // retuns student fee
        public string GetStudentFee()
        {
            return _program.StudentFee.ToString();
        }



        /**
         * function to get trainee data
         */
        public string GetEmployeeId(Employee employee)
        {
            return employee.EmployeeId.ToString();
        }
        public string GetEpfNo(Employee employee)
        {
            return employee.EPFNo;
        }
        public string GetTitle(Employee employee)
        {
            return employee.Title.ToString();
        }
        public string GetFullName(Employee employee)
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
        public string GetNic(Employee employee)
        {
            return employee.NIC;
        }
        public string GetWorkSpaceName(Employee employee)
        {
            return employee.WorkSpaceName;
        }
        public string GetDesignationName(Employee employee)
        {
            return employee.DesignationName;
        }
        public string GetEmployeeRecruitmentType(Employee employee)
        {
            return employee.EmployeeRecruitmentType.ToString();
        }
        public string GetEmpStatus(Employee employee)
        {
            return employee.EmpStatus.ToString();
        }
        public string GetDateOfAppointment(Employee employee)
        {
            return employee.DateOfAppointment.ToString();
        }
        public string GetTypeOfContract(Employee employee)
        {
            return employee.TypeOfContract;
        }
        public string GetOfficeEmail(Employee employee)
        {
            return employee.OfficeEmail;
        }
        public string GetMobileNumber(Employee employee)
        {
            return employee.MobileNumber;
        }
        public string GetPrivateEmail(Employee employee)
        {
            return employee.PrivateEmail;
        }
    }
}