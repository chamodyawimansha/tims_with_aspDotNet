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
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing;


namespace CECBTIMS.Controllers
{
    public class DocumentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Document
        public ActionResult Index()
        {
            return Content("Hello");
        }


        public async Task<ActionResult> Generate(Guid? id, int programId)
        {

            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var file = await db.Files.FindAsync(id);

            if(file == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if(await db.Programs.FindAsync(programId) == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var template = System.IO.Path.Combine(Server.MapPath("~/Storage"), System.IO.Path.GetFileName(file.FileName) ?? throw new InvalidOperationException());
            
            //get date time in mili seconds and generate file name
            var destinationFileName = Guid.NewGuid().ToString("N") + ".docx";

            var destinationFile = Server.MapPath("~/Storage/gen/"+ destinationFileName);

            System.IO.File.Copy(template, destinationFile, false);

            //integer objects
            string[] VarList =
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

            //// filepath is a string which contains the path where the new document has to be created
            
            using (var wordDoc = WordprocessingDocument.Open(destinationFile, true))
            {
                string docText = null;
                using (var sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }



                var helperClass = typeof(DocumentHelper);

                var classInstance = Activator.CreateInstance(helperClass, programId);


                // replace variables in the document
                foreach (var var in VarList)
                {
                    if (docText.Contains("VAR" + var))
                    {
                        var method = helperClass.GetMethod(Helpers.FigureVarName(var));

                        docText = method != null ? new Regex("VAR" + var).Replace(docText, (string)method.Invoke(classInstance, null)) : new Regex("VAR" + var).Replace(docText, "Null");
                    }
                }

                using (var sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }
            }

            return DownloadDoc(destinationFile, destinationFileName);


        }


        public FileResult DownloadDoc(string path, string fileName)
        {
            return File(path, MimeMapping.GetMimeMapping(path), fileName);
        }
    }
}