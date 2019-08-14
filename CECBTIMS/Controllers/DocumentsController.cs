using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CECBTIMS.DAL;
using CECBTIMS.Models;
using CECBTIMS.Models.Enums;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Wordprocessing;
using BottomBorder = DocumentFormat.OpenXml.Wordprocessing.BottomBorder;
using InsideHorizontalBorder = DocumentFormat.OpenXml.Wordprocessing.InsideHorizontalBorder;
using InsideVerticalBorder = DocumentFormat.OpenXml.Wordprocessing.InsideVerticalBorder;
using LeftBorder = DocumentFormat.OpenXml.Wordprocessing.LeftBorder;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using Path = System.IO.Path;
using RightBorder = DocumentFormat.OpenXml.Wordprocessing.RightBorder;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using RunProperties = DocumentFormat.OpenXml.Wordprocessing.RunProperties;
using Table = DocumentFormat.OpenXml.Wordprocessing.Table;
using TableCell = DocumentFormat.OpenXml.Wordprocessing.TableCell;
using TableProperties = DocumentFormat.OpenXml.Wordprocessing.TableProperties;
using TableRow = DocumentFormat.OpenXml.Wordprocessing.TableRow;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;
using TopBorder = DocumentFormat.OpenXml.Wordprocessing.TopBorder;


namespace CECBTIMS.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        public static readonly string[] TraineeTableColumns = new[]
        {
            "Title",
            "Name With Title",
            "Full Name",
            "Name Short",
            "Employee Id",
            "Epf No",
            "Nic",
            "Work Space Name",
            "Designation Name",
            "Employee Recruitment Type",
            "Employment Status",
            "Date Of Appointment",
            "Date Of Joint",
            "Date Of Confrimation",
            "Experience in CECB",
            "Member / Non-Member",
            "Details of Foreign Training Participated",
            "Details of Foreign Field Vists Participated",
            "Remarks of Foreign Training Participated",
            "Nature Of Appointment",
            "Recommendation",
            "Name,Designation and grade",
            "Type Of Contract",
            "Office Email",
            "Mobile Number",
            "Private Email"

        };

        private readonly string[] _varList =
        {
            "YEAR",
            "TODAY",
            "PROGRAMTITLE",
            "ORGANISEDBY",
            "STARTDATE",
            "VENUE",
            "MEMBERFEE",
            "NONMEMBERFEE",
            "STUDENTFEE",
        };

        private readonly string[] _bookMarkList =
        {
            "AgendaListBookMark" /*numbered list of names*/,
            "AgendaDetailsTableBookMark" /*A table with details*/,
            "TraineeDetailsTableBookMark" /*A table with details*/,
            "ResourcePersonsListBookMark" /*numbered list of names*/,
            "ResourcePersonsDetailsTableBookMark" /*A table with details*/,
        };

        private string _destinationFileName;
        private string _destinationFile;
        private Type _helperClass;
        private object _classInstance;
        
        /**
         * Show the index page with documents related to a program
         */
        public async Task<ActionResult> Index(int? programId)
        {
            if (programId == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var documents = new List<TimsFile>();
            // get the program
            var program = await _db.Programs.FindAsync(programId);
            if (program == null) return View(documents);

            // get the generated documents equal to the selected program
            var docs = program.Files;
            documents.AddRange(docs.Where(item => item.FileMethod == FileMethod.Generate));

            //get list of templates equal to the selected program type
            var templateList = new List<TimsFile>();
            var fileList = await _db.Files.ToListAsync();
            templateList.AddRange(fileList.Where(item => item.ProgramType == program.ProgramType));
            // assign the template list to a view bag variable
            ViewBag.TemplateList = templateList;
            ViewBag.Program = program;

            return View(documents);
        }

        //redirect to column select
        public async Task<ActionResult> Generate(Guid? templateId, int? programId, string details, ProgramType ProgramType)
        {
            // validate request
            if (templateId == null || programId == null || string.IsNullOrWhiteSpace(details)) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //get the selected template
            var templateFile = await _db.Files.FindAsync(templateId);
            if (templateFile == null) return HttpNotFound();
            var program = await _db.Programs.FindAsync(programId);
            if (program == null) return HttpNotFound();
            // check if the template has trainee information table

            ViewBag.DocTitle = "Generated "+templateFile.Title;
            ViewBag.Details = "GeneratedDocument";
            ViewBag.Program = program;

            return View("ColumnSelect", templateFile);
        }
        

        /**
         * Copy the word template to a new file
         */
        private void ProcessTemplate(TimsFile file)
        {
            /**
             * get the document template from the storage
             */
            var template = System.IO.Path.Combine(Server.MapPath("~/Storage"),
                System.IO.Path.GetFileName(file.FileName) ?? throw new InvalidOperationException());
            /**
             * Generate a new file name and select a path for the new document
             */
            this._destinationFileName = Guid.NewGuid().ToString("N") + ".docx";
            this._destinationFile = Server.MapPath("~/Storage/gen/" + _destinationFileName);
            /**
             * Copy the template with the new name for processing.
             */
            System.IO.File.Copy(template, _destinationFile, false);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GenerateFile(Guid? id, int programId, int? employeeId, string title, string details,
            bool download, string[] columnNames)
        {
            /**
             * Validate request
             */
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (await _db.Programs.FindAsync(programId) == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            /**
             * select the file record in the database. return bad request if not available
             */
            var file = await _db.Files.FindAsync(id);
            if (file == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            /**
             * Process the template file
             */
            this.ProcessTemplate(file);
            /**
             * Process the document
             */
            this.ProcessVariables(programId, employeeId);
            /**
             * Process the table
             */
            this.ProcessTables();
            /**
             * if download true download the file and return to the page, otherwise return
             */
            if (download == true) return Download();
            /**
             * Save the created document details in the database
             */
//            this.Save(title,details,programId);


            //return to page
            return Content("page returned");
        }

        private void InstantiateHelperClass(int programId)
        {
            _helperClass = typeof(DocumentHelper);
            _classInstance = Activator.CreateInstance(_helperClass, programId);
        }

        /**
         * Replace variables in the document
         */
        private void ProcessVariables(int programId, int? employeeId)
        {
            using (var wordDoc = WordprocessingDocument.Open(_destinationFile, true))
            {
                string docText = null;
                /**
                 * store the document in a string variable
                 */
                using (var sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }

                /**
                 * Instantiate the document helper class with parameters
                 */
                this.InstantiateHelperClass(programId);
                /**
                 * Replacing the values in the template
                 */
                foreach (var var in _varList)
                {
                    if (!docText.Contains("VAR" + var)) continue;
                    var method = _helperClass.GetMethod(Helpers.FigureVarName(var));

                    docText = method != null
                        ? new Regex("VAR" + var).Replace(docText, (string) method.Invoke(_classInstance, null))
                        : new Regex("VAR" + var).Replace(docText, "Null");
                }

                /**
                 * write the changes to the file
                 */
                using (var sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }
            }
        }

        /**
         * Insert the data tables after the DataTableBookMark
         */
        private void ProcessTables()
        {
            /**
            * Add the table after the bookmark
            */
            using (var doc = WordprocessingDocument.Open(_destinationFile, true))
            {
                var mainPart = doc.MainDocumentPart;




                /**
                 * Trainee details table
                 */
//                TraineeDetailsTableBookMark




               /**
                * Add agenda table to the document
                */
               var agendaBookmark = FindBookMark(mainPart, "AgendaListBookMark");

                if (agendaBookmark != null)
                {
                    var parent = agendaBookmark.Parent; // bookmark's parent element
                    // insert after bookmark parent
                    parent.InsertAfterSelf(ProcessAgendaDetailsTable());

                    //rename the bookmark text value
//                    AgendaListBookMark
                }

                doc.Close();
            }
        }

        private Table ProcessAgendaDetailsTable()
        {

            var table = new Table();
            /**
             * Add styles to the table
             */
            this.SetTableStyle(table);
            /**
             * add column names to the table
             */
            table.Append(AddTitleRow(new[] { "No", "Topic", "Duration" }));
            /**
             * Get the list of agendas
             */
            var method = _helperClass.GetMethod("GetAgendaTable");
            var agendas = (List<Agenda>)method.Invoke(_classInstance, null);
            /**
             * Add agenda data to the table
             */
            var i = 1;
            foreach (var item in agendas)
            {
                var row = new TableRow();
                row.Append(CreateCell(i.ToString()));
                row.Append(CreateCell(item.Name));
                row.Append(CreateCell(item.From+" hrs - "+item.To+" hrs"));
                i++;
                table.Append(row);
            }

            return table;
        }


        //        private 

        private Table ProcessAgendaList()
        {
            return new Table();
        }

        /**
         * find a given bookmark from the document
         */
        private BookmarkStart FindBookMark(MainDocumentPart mainPart, string bookmarkName)
        {
            var bookmark = from d in mainPart.Document.Body.Descendants<BookmarkStart>()
                where d.Name == bookmarkName
                select d;

            var bmk = bookmark.SingleOrDefault();

            return bmk ?? null;
        }
        /**
         * Add column header to a table
         */
        private TableRow AddTitleRow(string[] columnNames)
        {
            if (columnNames == null) throw new ArgumentNullException(nameof(columnNames));

            var row = new TableRow();

            foreach (var col in columnNames)
            {
                row.Append(CreateCell(col));
            }

            return row;
        }














        private Table ProcessTraineeDetailsTable()
        {
            return new Table();
        }

        private Table ProcessResourcePersonsList()
        {
            return new Table();
        }

        private Table ProcessResourcePersonsDetailsTable()
        {
            return new Table();
        }

        /**
         * Create Data table
         */
        private Table CreateTable(string[] columnNames)
        {
            /**
           * Create the table
           */
            var table = new Table();
            var row = new TableRow();

            /**
             * Add styles to the table
             */
            this.SetTableStyle(table);

            /**
             * Add the column names
             */
            foreach (var col in columnNames)
            {
                row.Append(CreateCell(col));
            }

            table.Append(row);

            /**
             * Add Table Data
             */

            var method = _helperClass.GetMethod("GetEmployess");
            var traineeList = (List<Employee>) method.Invoke(_classInstance, null);

            // convert tl to object list
            foreach (var item in traineeList)
            {
                row = new TableRow();

                foreach (var m in columnNames)
                {
                    var methodName = _helperClass.GetMethod(DocumentHelper.ToFunctionName(m));
                    if (methodName != null)
                    {
                        row.Append(CreateCell((string) methodName.Invoke(_classInstance, new object[] {item})));
                    }
                    else
                    {
                        row.Append(CreateCell("Null"));
                    }
                }

                table.Append(row);
            }

            return table;
        }

        private static TableCell CreateCell(string text)
        {
            return new TableCell(new Paragraph(new Run(new Text(text))));
        }

        private void SetTableStyle(OpenXmlElement table)
        {
            var properties = new TableProperties();

            //table borders
            var borders = new TableBorders();

            borders.TopBorder = new TopBorder() {Val = new EnumValue<BorderValues>(BorderValues.Single)};
            borders.BottomBorder = new BottomBorder() {Val = new EnumValue<BorderValues>(BorderValues.Single)};
            borders.LeftBorder = new LeftBorder() {Val = new EnumValue<BorderValues>(BorderValues.Single)};
            borders.RightBorder = new RightBorder() {Val = new EnumValue<BorderValues>(BorderValues.Single)};
            borders.InsideHorizontalBorder = new InsideHorizontalBorder() {Val = BorderValues.Single};
            borders.InsideVerticalBorder = new InsideVerticalBorder() {Val = BorderValues.Single};

            properties.Append(borders);

            //set the table width to page width
            TableWidth tableWidth = new TableWidth() {Width = "5000", Type = TableWidthUnitValues.Pct};
            properties.Append(tableWidth);

            //add properties to table
            table.Append(properties);
        }


        // save generated file in th database
        private bool Save(string title, string details, int programId)
        {
            //create new file object
            var newFile = new TimsFile
            {
                Id = Guid.NewGuid(),
                Title = title,
                Details = details,
                FileName = _destinationFileName,
                OriginalFileName = _destinationFileName,
                FileType = (FileType) Enum.Parse(typeof(FileType), "DOCX" ?? throw new InvalidOperationException()),
                FileMethod = FileMethod.Generate, // generate
                ProgramId = programId,
            };
            // file path name
            var path = Path.Combine(Server.MapPath("~/Storage/gen"),
                Path.GetFileName(_destinationFileName) ?? throw new InvalidOperationException());

            try
            {
                if (System.IO.File.Exists(path))
                {
                    //save the new file object in the database
                    _db.Files.Add(newFile);
                    _db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }

        private FileResult Download()
        {
            return File(_destinationFile, MimeMapping.GetMimeMapping(_destinationFile), _destinationFileName);
        }
    }
}