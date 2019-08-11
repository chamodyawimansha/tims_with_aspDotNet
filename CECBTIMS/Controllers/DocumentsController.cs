using DocumentFormat.OpenXml.Packaging;
using System;
using System.Data;
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

        //only for development
        private readonly CECB_ERPEntities _db2 = new CECB_ERPEntities();


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

        private string _destinationFileName;
        private string _destinationFile;
        private Type _helperClass;
        private object _classInstance;
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


        public async Task<ActionResult> Generate(Guid? id, int programId, int? employeeId, string title, string details,
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
            this.ProcessTable();
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
        private void ProcessTable()
        {
            /**
            * Add the table after the bookmark
            */
            using (var doc = WordprocessingDocument.Open(_destinationFile, true))
            {
                var mainPart = doc.MainDocumentPart;
                // Find the table bookmark from the document
                var res = from bm in mainPart.Document.Body.Descendants<BookmarkStart>()
                    where bm.Name == "DataTableBookMark"
                    select bm;

                var bookmark = res.SingleOrDefault();
                if (bookmark != null)
                {
                    var parent = bookmark.Parent; // bookmark's parent element
                    // insert after bookmark parent
                    parent.InsertAfterSelf(CreateTable(new[] {"Name", "Age", "Birth"}));
                }

                // close saves all parts and closes the document
                doc.Close();
            }
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

            //add first row with title  

//            Column names and function names should be same in order to fill the table
            /**
             * Add the column names
             */
            foreach (var col in columnNames)
            {
                row.Append(CreateCell(col));
            }

            table.Append(row);


            //add content rows
            for (int rowNumber = 1; rowNumber <= 5; rowNumber++)
            {
                row = new TableRow();

                row.Append(CreateCell("A" + rowNumber.ToString()));
                row.Append(CreateCell("B" + rowNumber.ToString()));
                row.Append(CreateCell("C" + rowNumber.ToString()));

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