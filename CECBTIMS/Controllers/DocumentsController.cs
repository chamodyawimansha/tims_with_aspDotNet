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

        public async Task<ActionResult> Generate(Guid? id, int programId, int? employeeId, string title, string details, bool download, string[] columnNames)
        {
            /**
             * Validate request
             */
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (await _db.Programs.FindAsync(programId) == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            /**
             * select the file record in the database. return bad request if not available
             */
            var file = await _db.Files.FindAsync(id);
            if(file == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            /**
             * get the document template from the storage
             */
            var template = System.IO.Path.Combine(Server.MapPath("~/Storage"), System.IO.Path.GetFileName(file.FileName) ?? throw new InvalidOperationException());
            /**
             * Generate a new file name and select a path for the new document
             */
            var destinationFileName = Guid.NewGuid().ToString("N") + ".docx";
            var destinationFile = Server.MapPath("~/Storage/gen/"+ destinationFileName);
            /**
             * Copy the template with the new name for processing.
             */
            System.IO.File.Copy(template, destinationFile, false);
            /**
             * Process the document
             */
             Process(destinationFile, destinationFileName, programId, employeeId, title, details);
            /**
             * if download true download the file and return to the page, otherwise return
             */
            if (download == true) return Download(destinationFile, destinationFileName);


            //return to page
            return Content("page returned");
        }

        // process the document generation
        private void Process(string destinationFile,string destinationFileName, int programId, int? employeeId, string title, string details)
        {

            ProcessTable(destinationFile);

            return;

            using (var wordDoc = WordprocessingDocument.Open(destinationFile, true))
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
                var helperClass = typeof(DocumentHelper);
                var classInstance = Activator.CreateInstance(helperClass, programId);
                if (employeeId != null) classInstance = Activator.CreateInstance(helperClass, programId, employeeId);
                /**
                 * Replacing the values in the template
                 */
                foreach (var var in _varList)
                {
                    if (!docText.Contains("VAR" + var)) continue;
                    var method = helperClass.GetMethod(Helpers.FigureVarName(var));

                    docText = method != null ? new Regex("VAR" + var).Replace(docText, (string)method.Invoke(classInstance, null)) : new Regex("VAR" + var).Replace(docText, "Null");
                }


                /**
                 * Process table values
                 */

                // default columns
                // user chooses the columns otherwise default
                    // add default columns table
                    // add has table checkbox to the template upload
                    // name or choose from previouis default columns in the upload
                
                // choose the coummns before generate
                //process the table
                // add table to the bookmark in the template




                //create table from const column names



                /**
                 * write the changes to the file
                 */
                using (var sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }
                /**
                 * Save the document in the database
                 */
                Save(title, details, destinationFileName,programId);
            }
        }

        private static void ProcessTable(string destinationFile)
        {
            /**
             * Create the table
             */
            Table table = null;
            TableRow row = null;

            //create table
            table = new Table();

            SetTableStyle(table);

            //add first row with title            
            row = new TableRow();
            row.Append(CreateCell("Column A"));
            row.Append(CreateCell("Column B"));
            row.Append(CreateCell("Column C"));

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

//            //add table to body
//            var body = new Body(table);
//            var document = new Document(body);
//
//            //create file
//            using (var file = WordprocessingDocument.Create(destinationFile, WordprocessingDocumentType.Document))
//            {
//                file.AddMainDocumentPart();
//                file.MainDocumentPart.Document = document;
//                file.MainDocumentPart.Document.Save();
//            }


            /**
             * Add the table after the bookmark
             */

            using (var doc = WordprocessingDocument.Open(destinationFile, true))
            {
                var mainPart = doc.MainDocumentPart;
                // Find the table bookmark from the document
                var res = from bm in mainPart.Document.Body.Descendants<BookmarkStart>()
                          where bm.Name == "DataTableBookMark"
                          select bm;

                var bookmark = res.SingleOrDefault();
                if (bookmark != null)
                {
                    var parent = bookmark.Parent;   // bookmark's parent element
                    // insert after bookmark parent
                    parent.InsertAfterSelf(table);
                }

                // close saves all parts and closes the document
                doc.Close();
            }


        }

        private static TableCell CreateCell(string text)
        {
            return new TableCell(new Paragraph(new Run(new Text(text))));
        }

        private static void SetTableStyle(Table table)
        {
            TableProperties properties = new TableProperties();

            //table borders
            TableBorders borders = new TableBorders();

            borders.TopBorder = new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single) };
            borders.BottomBorder = new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single) };
            borders.LeftBorder = new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single) };
            borders.RightBorder = new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single) };
            borders.InsideHorizontalBorder = new InsideHorizontalBorder() { Val = BorderValues.Single };
            borders.InsideVerticalBorder = new InsideVerticalBorder() { Val = BorderValues.Single };

            properties.Append(borders);

            //set the table width to page width
            TableWidth tableWidth = new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct };
            properties.Append(tableWidth);

            //add properties to table
            table.Append(properties);
        }


        // save generated file in th database
        private bool Save(string title, string details, string newFileName, int programId)
        {
            //create new file object
            var newFile = new TimsFile
            {
                Id = Guid.NewGuid(),
                Title = title,
                Details = details,
                FileName = newFileName,
                OriginalFileName = newFileName,
                FileType = (FileType) Enum.Parse(typeof(FileType), "DOCX" ?? throw new InvalidOperationException()),
                FileMethod = FileMethod.Generate, // generate
                ProgramId = programId,
            };
            // file path name
            var path = Path.Combine(Server.MapPath("~/Storage/gen"),
                Path.GetFileName(newFileName) ?? throw new InvalidOperationException());

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

        private FileResult Download(string path, string fileName)
        {
            return File(path, MimeMapping.GetMimeMapping(path), fileName);
        }
    }
}