using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CECBTIMS.Controllers;
using CECBTIMS.Models.Enums;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using BottomBorder = DocumentFormat.OpenXml.Wordprocessing.BottomBorder;
using Break = DocumentFormat.OpenXml.Wordprocessing.Break;
using InsideHorizontalBorder = DocumentFormat.OpenXml.Wordprocessing.InsideHorizontalBorder;
using InsideVerticalBorder = DocumentFormat.OpenXml.Wordprocessing.InsideVerticalBorder;
using LeftBorder = DocumentFormat.OpenXml.Wordprocessing.LeftBorder;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using ParagraphProperties = DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties;
using RightBorder = DocumentFormat.OpenXml.Wordprocessing.RightBorder;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using RunProperties = DocumentFormat.OpenXml.Wordprocessing.RunProperties;
using Table = DocumentFormat.OpenXml.Wordprocessing.Table;
using TableCell = DocumentFormat.OpenXml.Wordprocessing.TableCell;
using TableCellProperties = DocumentFormat.OpenXml.Wordprocessing.TableCellProperties;
using TableProperties = DocumentFormat.OpenXml.Wordprocessing.TableProperties;
using TableRow = DocumentFormat.OpenXml.Wordprocessing.TableRow;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;
using TopBorder = DocumentFormat.OpenXml.Wordprocessing.TopBorder;


namespace CECBTIMS.Models
{
    [Authorize]
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
            "GetStartMonthAndYear",
            "GetStartTime",
            "GetPerHeadCost",
            "GetEmployeeCount",
            "GetEmployeeCost",
            "GetResourcePersonCost",
            "GetProgramTotalCost",
            "GetRecipient", // the recipient for a single employee
            "GetRegistrationFee",
            "GetPostGradTotalCost",
            "GetExaminationFees",
            "GetBlankDate",
            "GetDepartment",
            "GetStartAndEndYear", //2019/2020
            "GetNotifiedOn",
            "GetStartYear",
            "GetDurationInDays",
            "GetEndDate",


            "GetIncidentalAllowance",
            "GetWarmClothAllowance",
            "GetEmployeeRecommendation",
            "GetTotalIncidentalAllowance",
        };

        public static string[] DocumentTableList =
        {
            "GetTraineeInformationTable",
            "GetAgendaDetailsTable",
            "GetResourcePersonsTable"
        };

        public static string[] DocumentListsList =
        {
            "GetAgendaList",
            "GetRecipientsList",
            "GetAgendaSubjectsList",
            "GetResourcePersonsList",
            "GetTraineeList",
            "GetDetailedTraineeList",
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
            return DateTime.Now.ToString("dd.mm.yyyy");
        }

        public string GetProgramTitle()
        {
            return _program.Title;
        }

        public string GetOrganisedBy()
        {
            return _program.Organizer != null ? _program.Organizer.Name : "Null";
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

        public string GetEndDate()
        {
            return _program.EndDate != null ? ((DateTime) _program.EndDate).ToString("D") : "YYYY/DD/MM";
        }

        public string GetStartEndTime()
        {
            var startTime = _program.StartTime?.ToString("h:mm tt");
            var endTime = _program.EndTime?.ToString("h:mm tt");

            return startTime + " To " + endTime;
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
            return _program != null ? "Rs." + _program.ProgramFee?.ToString("N") + "/-" : "Null";
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

        public string GetStartYear()
        {
            return _program.StartDate.ToString("yyyy");
        }

        public string GetDurationInDays()
        {
            return _program != null ? _program.DurationInDays + " Days" : "Null";
        }

        public string GetStartTime()
        {
            return _program.StartTime?.ToString("h:mm tt");
        }

        public string GetPerHeadCost()
        {
            return Convert.ToSingle((_program.PerPersonFee)).ToString("N");
        }

        public string GetEmployeeCount()
        {
            return _program.ProgramAssignments.Count.ToString();
        }

        public string GetEmployeeCost()
        {
            return Convert.ToSingle(_program.PerPersonFee * _program.ProgramAssignments.Count).ToString("N");
        }

        public string GetResourcePersonCost()
        {
            var cost = _program.ResourcePersons.Sum(item => item.Cost);

            return cost.ToString("N");
        }

        public string GetProgramTotalCost()
        {
            var rCost = _program.ResourcePersons.Sum(item => item.Cost);
            var eCost = (Convert.ToSingle(_program.PerPersonFee * _program.ProgramAssignments.Count));

            return (rCost + eCost).ToString("N");
        }

        public string GetDepartment()
        {
            return _program.Department;
        }

        public string GetStartAndEndYear()
        {
            var start = _program.StartDate.ToString("yyyy");
            var end = _program.EndDate != null
                ? ((DateTime) _program.EndDate).ToString("yyyy")
                : DateTime.Now.ToString("yyyy");

            return start + "/" + end;
        }

        public string GetRegistrationFee()
        {
            return (Convert.ToSingle(_program.RegistrationFee.ToString())).ToString("N");
        }

        public string GetPostGradTotalCost()
        {
            var cf = _program.ProgramFee ?? 0.0;
            var regc = Convert.ToSingle(_program.RegistrationFee);
            var exac = 0.0;

            foreach (var item in _program.Costs)
            {
                if (item.Name.Contains("Examination Fees"))
                {
                    exac = item.Value;
                }
            }

            return (cf + regc + exac).ToString("N");
        }

        public string GetExaminationFees()
        {
            foreach (var item in _program.Costs)
            {
                if (item.Name.Contains("Examination Fees"))
                {
                    return item.Value.ToString("N");
                }
            }

            return "No Examination Fees";
        }

        public string GetRecipient()
        {
            var emp = _employees.First();
            return emp.WorkSpaceType.Replace("Unit", "") + "(" + emp.WorkSpaceName + ")";
        }

        public string GetBlankDate()
        {
            return "YYYY/MM/DD";
        }

        public string GetNotifiedOn()
        {
            return _program.NotifiedOn != null ? ((DateTime) _program.NotifiedOn).ToString("D") : "yyyy.mm.dd";
        }

        public string GetEmployeeRecommendation()
        {
            var emp = _employees.FirstOrDefault();
            if (emp != null) return (emp.WorkSpaceType).Replace("Unit", "") + "(" + emp.WorkSpaceName + ")";

            return "Null";
        }

        public string GetTotalIncidentalAllowance()
        {
            foreach (var item in _program.Costs)
            {
                if (!item.Name.Contains("Incidental allowance")) continue;
                var ia = 0.0;
                ia = item.Value;

                if (_program.DurationInDays != null)
                {
                    ia *= (int) _program.DurationInDays;
                }

                return GetCurrencyMark(_program.Currency) + ia.ToString("N");
            }

            return "Null";
        }

        public string GetWarmClothAllowance()
        {
            foreach (var item in _program.Costs)
            {
                if (!item.Name.Contains("Warm cloth allowance")) continue;
                var wca = 0.0;
                wca = item.Value;
                return GetCurrencyMark(_program.Currency) + wca.ToString("N");
            }

            return "Null";
        }

        public string GetIncidentalAllowance()
        {
            foreach (var item in _program.Costs)
            {
                if (!item.Name.Contains("Incidental allowance")) continue;

                var ia = 0.0;

                ia = item.Value;

                return GetCurrencyMark(_program.Currency) + ia.ToString("N");
            }

            return "Null";
        }

        private static string GetCurrencyMark(Currency c)
        {
            switch (c.ToString())
            {
                case "USD":
                    return "$";
                case "Euro":
                    return "€";
                case "GBP":
                    return "£";
                case "Yuan":
                    return "¥";
                default:
                    return "Rs";
            }
        }

        public Paragraph GetName(Employee emp)
        {
            return WithDefaults(new Text(ToProperName(emp.NameWithInitial)));
        }

        public Paragraph GetTitle(Employee emp)
        {
            return WithDefaults(new Text(emp.Title != null ? emp.Title.ToString() : "Null"));
        }

        public Paragraph GetFullName(Employee emp)
        {
            return WithDefaults(new Text(emp.FullName));
        }

        public Paragraph GetNameWithTitle(Employee emp)
        {
            return WithDefaults(new Text(emp.Title != null
                ? emp.Title + ". " + ToProperName(emp.NameWithInitial)
                : ToProperName(emp.NameWithInitial)));
        }

        public Paragraph GetDesignation(Employee emp)
        {
            return WithTextCenter(new Text(emp.DesignationName));
        }

        public Paragraph GetNameDesignationAndGrade(Employee emp)
        {
            var p = new Paragraph();
            var r = new Run();
            r.Append(DefaultStyle());

            r.Append(new Text(emp.Title != null
                ? emp.Title + ". " + ToProperName(emp.NameWithInitial)
                : ToProperName(emp.NameWithInitial)));
            r.Append(new Break());
            r.Append(new Text(emp.DesignationName));
            r.Append(new Break());
            r.Append(new Text(emp.Grade));

            p.Append(r);

            return p;
        }


        public Paragraph GetGrade(Employee emp)
        {
            return WithTextCenter(new Text(emp.Grade));
        }

        public Paragraph GetNic(Employee emp)
        {
            return WithTextCenter(new Text(emp.EPFNo));
        }

        public Paragraph GetWorkspaceName(Employee emp)
        {
            return WithTextCenter(new Text(emp.WorkSpaceName));
        }

        public Paragraph GetNatureOfAppointment(Employee emp)
        {
            return WithTextCenter(new Text(emp.EmployeeRecruitmentType.ToString()));
        }

        public Paragraph GetRecommendation(Employee emp)
        {
            return WithTextCenter(new Text((emp.WorkSpaceType).Replace("Unit", "") + "(" + emp.WorkSpaceName + ")"));
        }

        public Paragraph GetDateOfJoined(Employee emp)
        {
            if (emp.DateOfJoint != null)
            {
                return WithTextCenter(new Text(((DateTime) emp.DateOfJoint).ToString("yyyy/MM/dd")));
            }

            return WithTextCenter(emp.DateOfAppointment != null
                ? new Text(((DateTime) emp.DateOfAppointment).ToString("yyyy/MM/dd"))
                : new Text("null"));
        }

        public Paragraph GetDateOfAppointment(Employee emp)
        {
            return WithTextCenter(emp.DateOfAppointment != null
                ? new Text(((DateTime) emp.DateOfAppointment).ToString("yyyy/MM/dd"))
                : new Text("null"));
        }

        public Paragraph GetExperienceInCecb(Employee emp)
        {
            var startDate = emp.DateOfJoint ?? emp.DateOfAppointment;
            var today = DateTime.Today;

            if (startDate == null) return WithTextCenter(new Text("null"));

            var diffInDays = (today.Subtract((DateTime) startDate)).Days;

            var months = (diffInDays / 30);
            var years = (int) months / 12;

            var restMonths = months % 12;

            return WithTextCenter(new Text(years.ToString("D2") + "Y " + restMonths.ToString("D2") + "M"));
        }

        public Paragraph GetEmail(Employee emp)
        {
            return WithTextCenter(new Text(emp.PrivateEmail));
        }

        public Paragraph GetContactNo(Employee emp)
        {
            return WithTextCenter(new Text(emp.MobileNumber));
        }

        public Paragraph GetPassportNo(Employee emp)
        {
            return WithTextCenter(new Text());
        }

        public Paragraph GetDateOfServiceConfirmation(Employee emp)
        {
            return emp.DateOfAppointment != null
                ? WithTextCenter(new Text(((DateTime) emp.DateOfAppointment).ToString("yyyy.mm.dd")))
                : WithTextCenter(new Text("YYYY.MM.DD"));
        }

        public Paragraph GetMemberNonMember(Employee emp)
        {
            var pas = _program.ProgramAssignments;

            foreach (var item in pas)
            {
                if (item.EmployeeId != emp.EmployeeId) continue;
                switch (item.MemberType.ToString())
                {
                    case ("Member"):
                        return WithTextCenter(new Text("Member"));
                    case ("NonMember"):
                        return WithTextCenter(new Text("Non-Member"));
                    default:
                        return WithTextCenter(new Text("Student"));
                }
            }

            return WithTextCenter(new Text());
        }

        public Paragraph GetRemarks(Employee emp)
        {
            return new Paragraph();
        }

        public Paragraph GetRemarksOnRelevancyToTheProgram(Employee emp)
        {
            return new Paragraph();
        }

        public Paragraph GetDetailsOfForeignTrainingParticipated(Employee emp)
        {
            return new Paragraph();
        }

        public Paragraph GetDetailsOfForeignVisitsParticipated(Employee emp)
        {
            return new Paragraph();
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

                tcpParagraph.Append(new ParagraphProperties
                    {Justification = new Justification() {Val = JustificationValues.Center}});

                tc.Append(new TableCellProperties(new TableCellVerticalAlignment()
                    {Val = TableVerticalAlignmentValues.Center}));

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
            var i = 1;
            //Add data to the table
            foreach (var emp in _employees)
            {
                var dataRow = new TableRow();

                foreach (var col in columnNames)
                {
                    var tc = new TableCell();

                    tc.Append(new TableCellProperties(new TableCellVerticalAlignment()
                        {Val = TableVerticalAlignmentValues.Center}));

                    if (col == TableColumnName.No)
                    {
                        tc.Append(WithDefaults(new Text(i.ToString("D2"))));
                        i++;
                        dataRow.Append(tc);
                        continue;
                    }

                    var theMethod = thisType.GetMethod(ToFunctionName(col.ToString()));

                    if (theMethod != null)
                    {
                        tc.Append((Paragraph) theMethod.Invoke(this, new object[] {emp}));
                    }
                    else
                    {
                        tc.Append(NullParagraph());
                    }

                    dataRow.Append(tc);
                }

                table.Append(dataRow);
            }


            return table;
        }

        public Table GetAgendaDetailsTable(TableColumnName[] columnNames, Guid? employeeId)
        {
            var table = new Table();
            var titleRow = new TableRow();
            this.SetTableStyle(table);

            var cols = new[] {"No", "Topic", "Duration"};

            foreach (var col in cols)
            {
                var tc = new TableCell();
                var tcpParagraph = new Paragraph();
                // Vertical align center
                tcpParagraph.Append(new ParagraphProperties
                    {Justification = new Justification() {Val = JustificationValues.Center}});
                tc.Append(new TableCellProperties(new TableCellVerticalAlignment()
                    {Val = TableVerticalAlignmentValues.Center}));
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
            var i = 1;
            foreach (var ag in _program.Agendas)
            {
                var dataRow = new TableRow();
                foreach (var col in cols)
                {
                    var tc = new TableCell();
                    tc.Append(new TableCellProperties(new TableCellVerticalAlignment()
                        {Val = TableVerticalAlignmentValues.Center}));

                    if (col.Equals("No"))
                    {
                        tc.Append(WithDefaults(new Text(i.ToString("D2"))));
                        i++;
                        dataRow.Append(tc);
                        continue;
                    }

                    var theMethod = thisType.GetMethod(ToFunctionName(col));
                    if (theMethod != null)
                    {
                        tc.Append((Paragraph) theMethod.Invoke(this, new object[] {ag}));
                    }
                    else
                    {
                        tc.Append(NullParagraph());
                    }

                    dataRow.Append(tc);
                }

                table.Append(dataRow);
            }

            return table;
        }

        public Table GetResourcePersonsTable(TableColumnName[] columnNames, Guid? employeeId)
        {
            var table = new Table();
            var titleRow = new TableRow();
            this.SetTableStyle(table);

            var cols = new[] {"No", "Resource_Person_Name", "Description"};

            foreach (var col in cols)
            {
                var tc = new TableCell();
                var tcpParagraph = new Paragraph();
                // Vertical align center
                tcpParagraph.Append(new ParagraphProperties
                    {Justification = new Justification() {Val = JustificationValues.Center}});
                tc.Append(new TableCellProperties(new TableCellVerticalAlignment()
                    {Val = TableVerticalAlignmentValues.Center}));
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
            var i = 1;
            foreach (var rp in _program.ResourcePersons)
            {
                var dataRow = new TableRow();
                foreach (var col in cols)
                {
                    var tc = new TableCell();
                    tc.Append(new TableCellProperties(new TableCellVerticalAlignment()
                        {Val = TableVerticalAlignmentValues.Center}));

                    if (col.Equals("No"))
                    {
                        tc.Append(WithDefaults(new Text(i.ToString("D2"))));
                        i++;
                        dataRow.Append(tc);
                        continue;
                    }

                    var theMethod = thisType.GetMethod(ToFunctionName(col));
                    if (theMethod != null)
                    {
                        tc.Append((Paragraph) theMethod.Invoke(this, new object[] {rp}));
                    }
                    else
                    {
                        tc.Append(NullParagraph());
                    }

                    dataRow.Append(tc);
                }

                table.Append(dataRow);
            }

            return table;
        }

        public Paragraph GetTopic(Agenda agenda)
        {
            return WithDefaults(new Text(agenda.Name));
        }

        public Paragraph GetResourcePersonName(ResourcePerson rp)
        {
            return WithDefaults(new Text(rp.Name));
        }

        public Paragraph GetDescription(ResourcePerson rp)
        {
            return WithDefaults(new Text(rp.Designation));
        }

        public Paragraph GetDuration(Agenda agenda)
        {
            return WithDefaults(new Text(agenda.From.ToString("t") + " hrs - " + agenda.To.ToString("t") + " hrs"));
        }

        private RunProperties DefaultStyle()
        {
            return new RunProperties(
                new FontSize() {Val = "24"},
                new RunFonts() {Ascii = FontFamily}
            );
        }

        private ParagraphProperties CenterStyle()
        {
            return new ParagraphProperties
                {Justification = new Justification() {Val = JustificationValues.Center}};
        }

        private Paragraph WithDefaults(Text text)
        {
            var p = new Paragraph();
            var r = new Run();
            r.Append(DefaultStyle());
            r.AppendChild(text);
            p.Append(r);
            return p;
        }

        private Paragraph WithTextCenter(Text text)
        {
            var p = new Paragraph();
            p.Append(CenterStyle());
            var r = new Run();
            r.Append(DefaultStyle());
            r.AppendChild(text);
            p.Append(r);
            return p;
        }

        private Paragraph NullParagraph()
        {
            return WithTextCenter(new Text("Null"));
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

        public Paragraph GetAgendaList()
        {
            var p = new Paragraph();
            p.Append(new ParagraphProperties
                {Justification = new Justification() {Val = JustificationValues.Left}});

            for (var i = 0; i < _program.Agendas.Count; i++)
            {
                var r = new Run();
                r.Append(new RunProperties(
                    new FontSize() {Val = "24"},
                    new RunFonts() {Ascii = FontFamily},
                    new Bold() {Val = OnOffValue.FromBoolean(true)}
                ));

                r.AppendChild(new Text()
                {
                    Text = "     " + (i + 1).ToString("D2") + ".  " + _program.Agendas.ToArray()[i].Name + ".",
                    Space = SpaceProcessingModeValues.Preserve
                });

                p.AppendChild(r);

                p.Append(new Break());
            }

            return p;
        }

        public Paragraph GetTraineeList()
        {
            var p = new Paragraph();
            p.Append(new ParagraphProperties
                {Justification = new Justification() {Val = JustificationValues.Left}});
            var i = 1;
            foreach (var item in _employees)
            {
                var r = new Run();
                r.Append(new RunProperties(
                    new FontSize() {Val = "24"},
                    new RunFonts() {Ascii = FontFamily}
                ));
                r.AppendChild(new Text()
                {
                    Text = "          " + (i).ToString("D2") + ".    " + item.Title + ". " +
                           ToProperName(item.NameWithInitial) + " -     " + item.DesignationName + ".",
                    Space = SpaceProcessingModeValues.Preserve
                });

                p.AppendChild(r);
                i++;
                p.Append(new Break());
            }

            return p;
        }

        public Paragraph GetDetailedTraineeList()
        {
            var p = new Paragraph();
            p.Append(new ParagraphProperties
                {Justification = new Justification() {Val = JustificationValues.Left}});
            var i = 1;
            foreach (var item in _employees)
            {
                var r = new Run();
                r.Append(new RunProperties(
                    new FontSize() {Val = "24"},
                    new RunFonts() {Ascii = FontFamily}
                ));
                r.AppendChild(new Text()
                {
                    Text = "          " + (i).ToString("D2") + ".    " + item.Title + ". " +
                           ToProperName(item.NameWithInitial) + " -     " + item.DesignationName + "   -   " +
                           (item.WorkSpaceType.Replace("Unit", "") + "(" + item.WorkSpaceName + ")"),
                    Space = SpaceProcessingModeValues.Preserve
                });

                p.AppendChild(r);
                i++;
                p.Append(new Break());
            }

            return p;
        }

        public Paragraph GetRecipientsList()
        {
            var assignments = GetRecipients();

            var p = new Paragraph();
            p.Append(new ParagraphProperties
                {Justification = new Justification() {Val = JustificationValues.Left}});

            foreach (var item in assignments)
            {
                var r = new Run();
                r.Append(new RunProperties(
                    new FontSize() {Val = "22"},
                    new RunFonts() {Ascii = FontFamily}
                ));

                r.AppendChild(new Text()
                {
                    Text = item,
                    Space = SpaceProcessingModeValues.Preserve
                });

                p.AppendChild(r);

                p.Append(new Break());
            }

            return p;
        }

        public Paragraph GetAgendaSubjectsList()
        {
            var p = new Paragraph();
            p.Append(new ParagraphProperties
                {Justification = new Justification() {Val = JustificationValues.Left}});

            foreach (var item in _program.Agendas)
            {
                var r = new Run();
                r.Append(new RunProperties(
                    new FontSize() {Val = "24"},
                    new RunFonts() {Ascii = FontFamily},
                    new Bold() {Val = OnOffValue.FromBoolean(true)}
                ));

                r.AppendChild(new Text()
                {
                    Text = item.Name,
                    Space = SpaceProcessingModeValues.Preserve
                });

                p.AppendChild(r);

                p.Append(new Break());
            }

            return p;
        }

        public Paragraph GetResourcePersonsList()
        {
            var p = new Paragraph();
            p.Append(new ParagraphProperties
                {Justification = new Justification() {Val = JustificationValues.Left}});

            foreach (var item in _program.ResourcePersons)
            {
                var r = new Run();
                r.Append(new RunProperties(
                    new FontSize() {Val = "22"},
                    new RunFonts() {Ascii = FontFamily},
                    new Bold() {Val = OnOffValue.FromBoolean(true)}
                ));

                r.AppendChild(new Text()
                {
                    Text = "                         " + item.Name + " - " + item.Designation,
                    Space = SpaceProcessingModeValues.Preserve
                });

                p.AppendChild(r);

                p.Append(new Break());
            }

            return p;
        }

        private IEnumerable<string> GetRecipients()
        {
            return _employees.Select(item => item.WorkSpaceType.Replace("Unit", "") + "(" + item.WorkSpaceName + ")")
                .ToList();
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