using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CECBTIMS.Controllers;


namespace CECBTIMS.Models
{
    public class DocumentHelper
    {

        private Program _program;


        public DocumentHelper(Program program) => this._program = program;


        public string test()
        {
            return _program.Title;
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