using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CECBTIMS.Controllers;


namespace CECBTIMS.Models
{
    public class DocumentHelper
    {

        private readonly Program _program;

        public static readonly string[] DocumentVariableList = {
            "GetYear",
            "GetToday",
            "GetProgramTitle",
            "GetOrganisedBy",
            "GetStartDate",
            "GetVenue",
            "GetMemberFee",
            "GetNonMemberFee",
            "GetStudentFee",
            "GetTrainingManagerName",

        };

        public static string[] DocumentTableList =
        {
            "GetTraineeInformationTable"
        };

        private string[] _tableColumns;


        public DocumentHelper(Program program) => _program = program;

        public DocumentHelper(Program program, string[] tableColumns) => _tableColumns = tableColumns;


        public string GetDocumentNo()
        {
            return "";
        }

        public string GetYear()
        {
            return DateTime.Now.ToString("yyyy");
        }

        public string GetToday()
        {
            return DateTime.Now.ToString("yyyy/mm/dd");
        }

        public string GetProgramTitle()
        {
            return _program.Title;
        }

        public string GetOrganisedBy()
        {
            return _program.ProgramArrangements.Aggregate("", (current, item) => current == "" ? item.Organizer.Name : current + ", " + item.Organizer.Name);
        }

        public string GetStartDate()
        {
            return _program.StartDate.ToString("D");
        }

        public string GetVenue()
        {
            return _program.Venue;
        }

        public string GetMemberFee()
        {
            return "Rs."+_program.MemberFee+"/-";
        }

        public string GetNonMemberFee()
        {
            return "Rs." + _program.NonMemberFee + "/-";
        }

        public string GetStudentFee()
        {
            return "Rs." + _program.StudentFee + "/-";
        }

        public string GetTrainingManagerName()
        {
            return "Eng. LCK Karunarathna";
        }

        public string GetTraineeInformationTable()
        {
            return "null";
        }













        //        // get organisers of the program
        //        public static string GetOrganisedBy(Program program)
        //        {
        //            return program.ProgramArrangements.Aggregate("", (current, item) => current == "" ? item.Organizer.Name : current + ", " + item.Organizer.Na me);
        //        }
        //
        //        public static string GetTargetGroup(Program program)
        //        {
        //            return program.TargetGroups.Aggregate("", (current, item) => current == "" ? item.Name : current + ", " + item.Name);
        //        }
        //
        //        public static string GetTargetGroupList(Program program)
        //        {
        //            return program.TargetGroups.Aggregate("", (current, item) => current == "" ? item.Name : current + ", " + item.Name);
        //        }
        //
        //        public static string GetTraineeManagerName()
        //        {
        //            return "Eng.L C K Karunaratne";
        //        }
        //
        //        public static string ToFunctionName(string name)
        //        {
        //            name = name.ToLower();
        //            var nameParts = name.Split(null);
        //            return nameParts.Aggregate("Get", (current, item) => current + (item.First().ToString().ToUpper() + item.Substring(1)));
        //        }
        //
        //        private static string ToUpperFirstLetter(string source)
        //        {
        //            if (string.IsNullOrEmpty(source))
        //                return string.Empty;
        //            var letters = source.ToCharArray();
        //            letters[0] = char.ToUpper(letters[0]);
        //
        //            return new string(letters);
        //        }
        //
        //        private static string ToProperName(string name)
        //        {
        //            var nameParts = name.Split(null);
        //            var newName = "";
        //
        //            for (var i = 1; i < nameParts.Length; i++)
        //            {
        //                if (!nameParts[i].Equals("."))
        //                {
        //                    newName += nameParts[i] + $" ";
        //                }
        //            }
        //
        //            return newName += ToUpperFirstLetter(nameParts[0].ToLower());
        //        }
        //
        //        public static string GetName(Employee trainee)
        //        {
        //            return trainee.Title != null  ? trainee.Title+". "+ToProperName(trainee.NameWithInitial) : ToProperName(trainee.NameWithInitial);
        //        }
        //
        //        public static string GetDesignation(Employee trainee)
        //        {
        //            return trainee.DesignationName;
        //        }
        //
        //        public static string GetNatureOfAppointment(Employee trainee)
        //        {
        //            return trainee.EmployeeRecruitmentType.ToString();
        //        }
        //
        //        public static string GetRecommendation(Employee trainee)
        //        {
        //            return trainee.WorkSpaceType;
        //        }
        //
        //        public static string GetDurationOfTheCourse(Program program)
        //        {
        //            return program.DurationInMonths.ToString();
        //        }
        //
        //        public static string GetCourseFee(Program program)
        //        {
        //            return program.ProgramFee.ToString();
        //        }


    }
}