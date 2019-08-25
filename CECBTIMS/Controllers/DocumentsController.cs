using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using CECBTIMS.DAL;
using CECBTIMS.Models;
using CECBTIMS.Models.Enums;
using CECBTIMS.ViewModels;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Ajax.Utilities;
using Document = CECBTIMS.Models.Document;

namespace CECBTIMS.Controllers
{
    public class DocumentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Type _helperClass;
        private object _classInstance;

        // GET: Documents
        public async Task<ActionResult> Index(int? programId)
        {
            if (programId == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var program = await db.Programs.FindAsync(programId);

            if (program == null) return new HttpNotFoundResult();

            ViewBag.Program = program;

            return View(program.Documents);
        }

        //Select the template
        public async Task<ActionResult> Select(int? programId)
        {
            if (programId == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var program = await db.Programs.FindAsync(programId);
            if (program == null) return HttpNotFound();

            var templates = from t in db.Templates
                select t;
            // get the templates matches program type to selected program type
            templates = templates.Where(t => (t.ProgramType == program.ProgramType));
            ViewBag.ProgramId = programId;

            return View(await templates.ToListAsync());
        }

        // select columns
        public async Task<ActionResult> Create(int templateId, int programId)
        {
            var template = await db.Templates.FindAsync(templateId);
            if (template == null) return HttpNotFound();


            //create New Document Name
            var documentTitle = template.Title + "-" + programId + "-" + DateTime.Now.ToString("MMddyyyyhhmmss");

            var program = await db.Programs.FindAsync(programId);
            if (program == null) return HttpNotFound();

            var viewModel = new SelectConfirmViewModel
            {
                Template = template,
                Document = new Document()
                {
                    Title = documentTitle,
                    Details = "Generated Document For " + program.Title + " From " + template.Title + " template",
                    FileName = Guid.NewGuid() + "." + FileType.DOCX,
                    ProgramType = program.ProgramType,
                    FileType = FileType.DOCX,
                    FileMethod = FileMethod.Generate,
                    ProgramId = programId,
                    DocumentNumber = 000000000 //get document number here
                }
            };

            ViewBag.ProgramId = programId;


            return View(viewModel);
        }

        // GET: Documents/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var document = await db.Documents.FindAsync(id);
            if (document == null)
            {
                return HttpNotFound();
            }

            return View(document);
        }

        private async Task<object> InstHelperClass(int programId)
        {
            _helperClass = typeof(DocumentHelper);
            return Activator.CreateInstance(_helperClass, await ProgramsController.GetProgram(programId));
        }
        
        // POST: Documents1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Details,FileName,ProgramType,FileType,FileMethod,ProgramId,DocumentNumber")] Document document, TableColumnName[] columns, int templateId, int programId)
        {

            // document path of the newly created document from the template
            var path = await GetDocumentPath(templateId, document.FileName);

            //instantiate helper class
            _classInstance = await InstHelperClass(programId);

            //get document class
            // replace variables

            var method = _helperClass.GetMethod("test");


            ProcessVariables(path);
            return Content(ProcessTables(path, columns));
            // add tables


            //            return Content(ProcessBookmarkVariables(path).ToString());



            return Content(templateId.ToString());




            //generate thw word document
            //save the document in the storage
            // save the data in the database



            if (ModelState.IsValid)
            {
                db.Documents.Add(document);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ProgramId = new SelectList(db.Programs, "Id", "Title", document.ProgramId);
            return View(document);
        }

        private void ProcessVariables(string path)
        {
            using (var wordDoc = WordprocessingDocument.Open(path, true))
            {
                string docText = null;
                // read the entire document and store the text to a variable
                using (var sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }

                foreach (var item in DocumentHelper.DocumentVariableList)
                {
                    if (!docText.Contains(item)) continue;
                    
                    var method = _helperClass.GetMethod(item);
                
                    docText = method != null
                        ? new Regex(item).Replace(docText, (string) method.Invoke(_classInstance, null))
                        : new Regex(item).Replace(docText, "Null");
                }

                using (var sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }
                
            }

        }

        private string ProcessTables(string path, TableColumnName[] columnNames)
        {
            using (var wordDoc = WordprocessingDocument.Open(path, true))
            {
                var mainPart = wordDoc.MainDocumentPart;


                foreach (var item in DocumentHelper.DocumentTableList)
                {
                    //find the paragraphs
                    var res = from p in mainPart.Document.Body.Descendants<Paragraph>()
                        where p.InnerText == item
                        select p;

                    var method = _helperClass.GetMethod(item);

                    if (method != null)
                    {
                        var rf = res.First();

                        rf.RemoveAllChildren<Run>();
                        rf.AppendChild(new Run((Table)method.Invoke(_classInstance, new object[] { columnNames })));

                    }
     

                }


                return null;

            }
        }


        //        private int ProcessBookmarkVariables(string path)
        //        {
        //            using (var wordDoc = WordprocessingDocument.Open(path, true))
        //            {
        //
        //                var bookmarks = GetBookMarks(wordDoc);
        //
        //
        //                foreach (BookmarkStart bookmarkStart in bookmarks.Values)
        //                {
        //                    var bookmarkText = bookmarkStart.NextSibling<Run>();
        //                    if (bookmarkText != null)
        //                    {
        //                        bookmarkText.GetFirstChild<Text>().Text = "blah";
        //                    }
        //                }
        //
        //                return bookmarks.Count;
        //            }
        //        }


        private IDictionary<string, BookmarkStart> GetBookMarks(WordprocessingDocument wordDoc)
        {

            var bookmarkMap = new Dictionary<string, BookmarkStart>();
            // get all the bookmarks
            foreach (var bookmarkStart in wordDoc.MainDocumentPart.RootElement.Descendants<BookmarkStart>())
            {
                bookmarkMap[bookmarkStart.Name] = bookmarkStart;
            }

            return bookmarkMap;
        }




        private async Task<string> GetDocumentPath(int templateId, string newFileName)
        {
            var template = await db.Templates.FindAsync(templateId);

            if (template == null) return null;

            var path = Path.Combine(Server.MapPath("~/Storage/templates/"), Path.GetFileName(template.FileName + "." + template.FileType));

            var templateDestination = Server.MapPath("~/Storage/gen/" + newFileName);

            if (System.IO.File.Exists(path))
            {
                if (!System.IO.File.Exists(templateDestination))
                {
                    System.IO.File.Copy(path, templateDestination, false);
                }
            }
            
            return System.IO.File.Exists(templateDestination) ? templateDestination : null;
        }















        // GET: Documents1/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Document document = await db.Documents.FindAsync(id);
            if (document == null)
            {
                return HttpNotFound();
            }

            ViewBag.ProgramId = new SelectList(db.Programs, "Id", "Title", document.ProgramId);
            return View(document);
        }

        // POST: Documents1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            [Bind(Include =
                "Id,Title,Details,FileName,ProgramType,FileType,FileMethod,ProgramId,CreatedAt,UpdatedAt,CreatedBy,UpdatedBy,RowVersion")]
            Document document)
        {
            if (ModelState.IsValid)
            {
                db.Entry(document).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ProgramId = new SelectList(db.Programs, "Id", "Title", document.ProgramId);
            return View(document);
        }

        // GET: Documents1/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Document document = await db.Documents.FindAsync(id);
            if (document == null)
            {
                return HttpNotFound();
            }

            return View(document);
        }

        // POST: Documents1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Document document = await db.Documents.FindAsync(id);
            db.Documents.Remove(document);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}