using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using CECBTIMS.DAL;
using CECBTIMS.Models;
using CECBTIMS.Models.Document;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Ajax.Utilities;


namespace CECBTIMS.Controllers
{
    public class DocumentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Type _helperClass;
        private object _classInstance;

        private void InstantiateHelperClass()
        {

        }


        public async Task<ActionResult> Process(string option, int programId, Guid? employeeId)
        {
            if (option.Equals("ProgramDocument"))
            {
                await ProcessProgramDocument(programId);

            }else if (option.Equals("EmployeeDocument"))
            {
                return Content("Trainee");
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        private async Task<ActionResult> ProcessProgramDocument(int? programId)
        {
            var path = ProcessTemplate();

            IDictionary<string, BookmarkStart> bookmarkMap = new Dictionary<string, BookmarkStart>();

            using (var wordDoc = WordprocessingDocument.Open(path, true))
            {   
                // get all the bookmarks from the document
                foreach (var bookmarkStart in wordDoc.MainDocumentPart.RootElement.Descendants<BookmarkStart>())
                {
                    bookmarkMap[bookmarkStart.Name] = bookmarkStart;
                }
                // loop the bookmarks
                foreach (var bookmarkStart in bookmarkMap.Values)
                {


                    // process variable bookmarks 

                    // process list bookmarks

                    //process table bookmarks

//
//                    YearVar
//                        TraineeTable
//                            ProgramList


                    var bookmarkText = bookmarkStart.NextSibling<Run>();

                    if (bookmarkText != null)
                    {
                        bookmarkText.GetFirstChild<Text>().Text = bookmarkText.InnerText;
                    }
                }

            }

            return Content("Hello");
        }

        private string ProcessTemplate()
        {

            var template = Server.MapPath("~/Storage/templates/ApprovelLetter.docx");
            var tPath = Server.MapPath("~/Storage/gen/"+new Guid()+".docx");
            System.IO.File.Copy(template, tPath, true);

            return tPath;


            //            /**
            //             * get the document template from the storage
            //             */
            //            var template = System.IO.Path.Combine(Server.MapPath("~/Storage"),
            //                System.IO.Path.GetFileName(file.FileName) ?? throw new InvalidOperationException());
            //            /**
            //             * Generate a new file name and select a path for the new document
            //             */
            //            this._destinationFileName = Guid.NewGuid().ToString("N") + ".docx";
            //            this._destinationFile = Server.MapPath("~/Storage/gen/" + _destinationFileName);
            //            /**
            //             * Copy the template with the new name for processing.
            //             */
            //            System.IO.File.Copy(template, _destinationFile, false);
        }



    }
}