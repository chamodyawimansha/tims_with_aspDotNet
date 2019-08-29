using System;
using System.Collections.Generic;
using System.Linq;
using CECBTIMS.Models.Enums;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;


namespace CECBTIMS.Models
{
    public class DocumentHelper
    {
        private readonly Program _program;
        private readonly List<Employee> _employees;

        private const string FontFamily = "Times New Roman";

        public static readonly string[] DocumentVariableList =
        {
            "GetDocumentNo",
            "GetYear",
            "GetToday",
            "GetProgramTitle",
            "GetOrganisedBy",
            "GetTargetGroup",
            "GetApplicationClosingDate",
            "GetApplicationClosingTime",
            "GetStartDate",
            "GetStartEndTime", // start time with am pm To end time 
            "GetVenue",
            "GetMemberFee",
            "GetNonMemberFee",
            "GetStudentFee",
            "GetProgramFee",
            "GetTrainingManagerName",
            "GetEmployeeName",
            "GetEmployeeDesignation",
            "GetDurationInMonths",
            "GetStartMonthAndYear"
        };

        public static string[] DocumentTableList =
        {
            "GetTraineeInformationTable"
        };

        public DocumentHelper(Program program)
        {
            _program = program;
        }

        public DocumentHelper(Program program, List<Employee> employees)
        {
            _program = program;
            _employees = employees;
        }


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
            return DateTime.Now.ToString("yyyy/MM/dd");
        }

        public string GetProgramTitle()
        {
            return _program.Title;
        }

        public string GetOrganisedBy()
        {
            return _program.ProgramArrangements.Aggregate("",
                (current, item) => current == "" ? item.Organizer.Name : current + ", " + item.Organizer.Name);
        }

        public string GetTargetGroup()
        {
            return _program.TargetGroups.Aggregate("",
                (current, item) => current == "" ? item.Name : current + ", " + item.Name);
        }

        public string GetApplicationClosingDate()
        {
            return _program.ApplicationClosingDate.ToString("D");
        }

        public string GetApplicationClosingTime()
        {
            return _program.ApplicationClosingTime.ToString("HH:mm tt");
        }

        public string GetStartDate()
        {
            return _program.StartDate.ToString("D");
        }

        public string GetStartEndTime()
        {
            return _program.StartTime.ToString("h:mm tt") + " To " + _program.EndTime.ToString("h:mm tt");
        }

        public string GetVenue()
        {
            return _program.Venue;
        }

        public string GetMemberFee()
        {
            return "Rs." + _program.MemberFee + "/-";
        }

        public string GetNonMemberFee()
        {
            return "Rs." + _program.NonMemberFee + "/-";
        }

        public string GetStudentFee()
        {
            return "Rs." + _program.StudentFee + "/-";
        }

        public string GetProgramFee()
        {
            return _program != null ? "Rs." + _program.ProgramFee + "/-" : "Null";
        }

        public string GetTrainingManagerName()
        {
            return "Eng. LCK Karunarathna";
        }

        public string GetStartMonthAndYear()
        {
            return _program.StartDate.ToString("Y");
        }

        public string GetEmployeeName()
        {
            var _employee = _employees.FirstOrDefault();
            return _employee != null
                ? (_employee.Title != null
                    ? _employee.Title + ". " + ToProperName(_employee.NameWithInitial)
                    : ToProperName(_employee.NameWithInitial))
                : "Null";
        }

        public string GetEmployeeDesignation()
        {
            var _employee = _employees.FirstOrDefault();
            return _employee != null ? _employee.DesignationName : "Null";
        }

        public string GetDurationInMonths()
        {
            return _program != null ? _program.DurationInMonths + " Months" : "Null";
        }


        public Text GetNo(Employee emp)
        {
            return new Text("");
        }

        public Text GetName(Employee emp)
        {
            return new Text(ToProperName(emp.NameWithInitial));
        }

        public Text GetTitle(Employee emp)
        {
            return new Text(emp.Title != null ? emp.Title.ToString() : "Null");
        }

        public Text GetFullName(Employee emp)
        {
            return new Text(emp.FullName);
        }

        public Text GetNameWithTitle(Employee emp)
        {
            return new Text(emp.Title != null
                ? emp.Title + ". " + ToProperName(emp.NameWithInitial)
                : ToProperName(emp.NameWithInitial));
        }

        public Text GetDesignation(Employee emp)
        {
            return new Text(emp.DesignationName);
        }

        public Text GetNameDesignationAndGrade(Employee emp)
        {
            var name = emp.Title != null
                ? emp.Title + ". " + ToProperName(emp.NameWithInitial)
                : ToProperName(emp.NameWithInitial);
            var designation = emp.DesignationName;
            var grade = emp.Grade;

            var t = new Text();


            return new Text(name + " - " + designation + " (" + grade + ")");
        }


        public Text GetGrade(Employee emp)
        {
            return new Text(emp.Grade);
        }

        public Text GetNic(Employee emp)
        {
            return new Text(emp.EPFNo);
        }

        public Text GetWorkspaceName(Employee emp)
        {
            return new Text(emp.WorkSpaceName);
        }

        public Text GetNatureOfAppointment(Employee emp)
        {
            return new Text(emp.NatureOfAppointment);
        }

        public Text GetRecommendation(Employee emp)
        {
            return new Text((emp.WorkSpaceType).Replace("Unit", "") + "(" + emp.WorkSpaceName + ")");
        }

        public Text GetDateOfJoined(Employee emp)
        {
            if (emp.DateOfJoint != null)
            {
                return new Text(((DateTime) emp.DateOfJoint).ToString("yyyy/MM/dd"));
            }

            return emp.DateOfAppointment != null
                ? new Text(((DateTime) emp.DateOfAppointment).ToString("yyyy/MM/dd"))
                : new Text("null");
        }

        public Text GetExperienceInCecb(Employee emp)
        {
            var startDate = emp.DateOfJoint ?? emp.DateOfAppointment;
            var today = DateTime.Today;

            if (startDate == null) return new Text("null");

            var diffInDays = (today.Subtract((DateTime) startDate)).Days;

            var months = (diffInDays / 30);
            var years = (int) months / 12;

            var restMonths = months % 12;

            return new Text(years.ToString("D2") + "Y " + restMonths.ToString("D2") + "M");
        }

        public Text GetEmail(Employee emp)
        {
            return new Text(emp.PrivateEmail);
        }

        public Text GetContactNo(Employee emp)
        {
            return new Text(emp.MobileNumber);
        }

        public Text GetPassportNo(Employee emp)
        {
            return new Text();
        }

        public Text GetDateOfServiceConfirmation(Employee emp)
        {
            return new Text(emp.DateOfAppointment.ToString());
        }

//        public Text GetIncidentalAllowance(Employee emp)
//        {
//            var costs = _program.Costs;
//
//            if (_program.EndDate != null)
//            {
//                var dateCount = int._program.StartDate.Subtract((DateTime) _program.EndDate);
//                
//                foreach (var cost in costs)
//                {
//                    if (cost.Name.ToLower().Contains("incidental"))
//                    {
//                        return new Text(_program.Currency.ToString() + cost.Value));
//                    }
//                }
//            }
//
//
//
//
//
//            return new Text("Set End Date");
//        }

//        public Text GetWarmClothAllowance(Employee emp)
//        {
//
//        }

        public Text GetMemberNonMember(Employee emp)
        {
            var pas = _program.ProgramAssignments;

            foreach (var item in pas)
            {
                if (item.EmployeeId != emp.EmployeeId) continue;
                switch (item.MemberType.ToString())
                {
                    case ("Member"):
                        return new Text("Member");
                    case ("NonMember"):
                        return new Text("Non-Member");
                    default:
                        return new Text("Student");
                }
            }

            return new Text();
        }

        public Text GetRemarks(Employee emp)
        {
            return new Text();
        }

        public Text GetRemarksOnRelevancyToTheProgram()
        {
            return new Text();
        }

        public Text GetDetailsOfForeignTrainingParticipated()
        {
            return new Text();
        }

        public Text GetDetailsOfForeignVisitsParticipated()
        {
            return new Text();
        }


        private static string ToProperName(string name)
        {
            var nameParts = name.Split(null);
            var newName = "";

            for (var i = 1; i < nameParts.Length; i++)
            {
                if (!nameParts[i].Equals("."))
                {
                    newName += nameParts[i] + $" ";
                }
            }

            return newName += ToUpperFirstLetter(nameParts[0].ToLower());
        }

        private static string ToUpperFirstLetter(string source)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;
            var letters = source.ToCharArray();
            letters[0] = char.ToUpper(letters[0]);

            return new string(letters);
        }

        public Table GetTraineeInformationTable(TableColumnName[] columnNames, Guid? employeeId)
        {
            var table = new Table();
            var titleRow = new TableRow();
            this.SetTableStyle(table);

            // add Table Column Names
            foreach (var col in columnNames)
            {
                var tc = new TableCell();
                var tcpParagraph = new Paragraph();
                // Vertical align center
                tc.Append(new TableCellProperties(new TableCellVerticalAlignment()
                    {Val = TableVerticalAlignmentValues.Center}));

                //text center
                tcpParagraph.Append(new ParagraphProperties
                    {Justification = new Justification() {Val = JustificationValues.Center}});
                var run = new Run();

                run.Append(
                    new RunProperties(
                        new FontSize() {Val = "24"},
                        new RunFonts() {Ascii = FontFamily},
                        new Bold() {Val = OnOffValue.FromBoolean(true)}
                    )
                );

                run.Append(new Text(ToColumnName(col.ToString())));

                tcpParagraph.Append(run);
                tc.Append(tcpParagraph);

                titleRow.Append(tc);
            }

            table.Append(titleRow);

            var thisType = this.GetType();

            //Add data to the table
            foreach (var emp in _employees)
            {
                var dataRow = new TableRow();

                foreach (var col in columnNames)
                {
                    var tc = new TableCell();
                    var tcpParagraph = new Paragraph();
                    // Vertical align center
                    tc.Append(new TableCellProperties(new TableCellVerticalAlignment()
                        {Val = TableVerticalAlignmentValues.Center}));
                    var run = new Run();

                    run.Append(
                        new RunProperties(
                            new FontSize() {Val = "22"},
                            new RunFonts() {Ascii = FontFamily}
                        )
                    );

                    var theMethod = thisType.GetMethod(ToFunctionName(col.ToString()));

                    if (theMethod != null)
                    {
                        run.Append((Text) theMethod.Invoke(this, new object[] {emp}));
                    }
                    else
                    {
                        run.Append(new Text(
                            "Null"
                        ));
                    }


                    tcpParagraph.Append(run);
                    tc.Append(tcpParagraph);

                    dataRow.Append(tc);
                }

                table.Append(dataRow);
            }


            return table;
        }

        private string ToFunctionName(string name)
        {
            var nameParts = name.Split('_');

            return nameParts.Aggregate("Get", (current, item) => current + (item + ""));
        }

        private string ToColumnName(string name)
        {
            var nameParts = name.Split('_');

            return nameParts.Aggregate("", (current, item) => current + (item + " "));
        }

        private void SetTableStyle(OpenXmlElement table)
        {
            var properties = new TableProperties();

            //table borders
            var borders = new TableBorders
            {
                TopBorder = new TopBorder() {Val = new EnumValue<BorderValues>(BorderValues.Single)},
                BottomBorder = new BottomBorder() {Val = new EnumValue<BorderValues>(BorderValues.Single)},
                LeftBorder = new LeftBorder() {Val = new EnumValue<BorderValues>(BorderValues.Single)},
                RightBorder = new RightBorder() {Val = new EnumValue<BorderValues>(BorderValues.Single)},
                InsideHorizontalBorder = new InsideHorizontalBorder() {Val = BorderValues.Single},
                InsideVerticalBorder = new InsideVerticalBorder() {Val = BorderValues.Single}
            };

            properties.Append(borders);
            //set the table width to page width
            var tableWidth = new TableWidth() {Width = "5000", Type = TableWidthUnitValues.Pct};
            properties.Append(tableWidth);
            //add properties to table
            table.Append(properties);
        }
    }
}