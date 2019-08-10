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
using Path = System.IO.Path;


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