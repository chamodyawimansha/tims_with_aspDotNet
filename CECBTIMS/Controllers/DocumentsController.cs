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

        
        public async Task<ActionResult> Generate(Guid? id)
        {

            string[] VarList =
            {
                "YEAR",
                "TODAY",
                "PROGRAMTITLE",

            };

            //// filepath is a string which contains the path where the new document has to be created
            var sourceFile = Server.MapPath("~/Storage/a0a8e2fb-bb6c-4ad9-8d6b-4102a01e9506_Approvel letter.docx");

            var destinationFile = Server.MapPath("~/Storage/Approvel_letter_edited.docx");

            System.IO.File.Copy(sourceFile, destinationFile, true);




            using (var wordDoc = WordprocessingDocument.Open(destinationFile, true))
            {
            
                string docText = null;
                using (var sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }

                foreach (var var in VarList)
                {
                    if (docText.Contains("VAR" + var))
                    {
                        docText = new Regex("VAR" + var).Replace(docText, Helpers.CallMethod(var, null));
                    }

                }

                // call method from variable names
                

                using (var sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }

            }


            return DownloadDoc(destinationFile, "Approvel_letter_edited.docx");




            return Content("Hello");




//            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//
//            c
//
//            if (file == null) return HttpNotFound();
//
//            var path = Path.Combine(Server.MapPath("~/Storage"), Path.GetFileName(file.FileName) ?? throw new InvalidOperationException());
//
//            if (System.IO.File.Exists(path))
//            {
//
////                return DownloadFile(path, file.FileName);
//            }
//
//            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        }


        public FileResult DownloadDoc(string path, string fileName)
        {
            return File(path, MimeMapping.GetMimeMapping(path), fileName);
        }
    }
}