using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            "GetRecipientsList"
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
            return _program.Organizer.Name;
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

        public string GetStartTime()
        {
            return _program.StartTime.ToString("h:mm tt");
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
            return WithTextCenter(new Text("This removed"));
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
            return WithTextCenter(new Text(emp.DateOfAppointment.ToString()));
        }

        //        public Run GetIncidentalAllowance(Employee emp)
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

        public Paragraph GetRemarksOnRelevancyToTheProgram()
        {
            return new Paragraph();
        }

        public Paragraph GetDetailsOfForeignTrainingParticipated()
        {
            return new Paragraph();
        }

        public Paragraph GetDetailsOfForeignVisitsParticipated()
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

            var cols = new[]{"No", "Topic", "Duration"};

            foreach (var col in cols)
            {
                var tc = new TableCell();
                var tcpParagraph = new Paragraph();
                // Vertical align center
                tcpParagraph.Append(new ParagraphProperties
                    { Justification = new Justification() { Val = JustificationValues.Center } });
                tc.Append(new TableCellProperties(new TableCellVerticalAlignment()
                    { Val = TableVerticalAlignmentValues.Center }));
                var run = new Run();
                run.Append(
                    new RunProperties(
                        new FontSize() { Val = "24" },
                        new RunFonts() { Ascii = FontFamily },
                        new Bold() { Val = OnOffValue.FromBoolean(true) }
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
                        { Val = TableVerticalAlignmentValues.Center }));

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
                        tc.Append((Paragraph)theMethod.Invoke(this, new object[] { ag }));
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

            var cols = new[] { "No", "Resource_Person_Name", "Description" };

            foreach (var col in cols)
            {
                var tc = new TableCell();
                var tcpParagraph = new Paragraph();
                // Vertical align center
                tcpParagraph.Append(new ParagraphProperties
                    { Justification = new Justification() { Val = JustificationValues.Center } });
                tc.Append(new TableCellProperties(new TableCellVerticalAlignment()
                    { Val = TableVerticalAlignmentValues.Center }));
                var run = new Run();
                run.Append(
                    new RunProperties(
                        new FontSize() { Val = "24" },
                        new RunFonts() { Ascii = FontFamily },
                        new Bold() { Val = OnOffValue.FromBoolean(true) }
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
                        { Val = TableVerticalAlignmentValues.Center }));

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
                        tc.Append((Paragraph)theMethod.Invoke(this, new object[] { rp }));
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
                { Justification = new Justification() { Val = JustificationValues.Left }});

            for (var i = 0; i < _program.Agendas.Count; i++)
            {
                var r = new Run();
                r.Append(new RunProperties(
                    new FontSize() { Val = "24" },
                    new RunFonts() { Ascii = FontFamily },
                    new Bold() { Val = OnOffValue.FromBoolean(true) }
                ));

                r.AppendChild(new Text()
                {
                    Text = "     " + (i + 1).ToString("D2") + ".  " + _program.Agendas.ToArray()[i].Name+".",
                    Space = SpaceProcessingModeValues.Preserve
                });

                p.AppendChild(r);

                p.Append(new Break());
            }

            return p;
        }

        public Paragraph GetRecipientsList()
        {
            var assignments = GetRecipients(_program);

            var p = new Paragraph();
            p.Append(new ParagraphProperties
                { Justification = new Justification() { Val = JustificationValues.Left } });

            foreach (var item in assignments)
            {
                var r = new Run();
                r.Append(new RunProperties(
                    new FontSize() { Val = "22" },
                    new RunFonts() { Ascii = FontFamily }
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

        private static IEnumerable<string> GetRecipients(Program program)
        {
            var recipients = new List<string>();
            var assignments = program.ProgramAssignments;
   
            foreach (var item in assignments)
            {
                var emp = EmployeesController.FindEmployee(item.EmployeeVersionId);
                recipients.Add(emp.WorkSpaceType.Replace("Unit", "")+"("+emp.WorkSpaceName+")");
            }

            return recipients;

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