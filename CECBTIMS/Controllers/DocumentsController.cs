using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using CECBTIMS.DAL;
using CECBTIMS.Models;
using CECBTIMS.Models.Enums;
using CECBTIMS.ViewModels;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using PagedList.EntityFramework;
using Document = CECBTIMS.Models.Document;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using ParagraphProperties = DocumentFormat.OpenXml.Drawing.ParagraphProperties;
using Path = System.IO.Path;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using RunProperties = DocumentFormat.OpenXml.Drawing.RunProperties;
using Table = DocumentFormat.OpenXml.Wordprocessing.Table;
using Text = DocumentFormat.OpenXml.Drawing.Text;

namespace CECBTIMS.Controllers
{
    [Authorize]
    public class DocumentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Type _helperClass;
        private object _classInstance;

        // GET: Documents
        public async Task<ActionResult> Index(int? programId, Guid? employeeId, string searchString, int? countPerPage,
            int? page)
        {
            if (programId == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var pageSize = countPerPage ?? 5;
            var pageNumber = page ?? 1;

            var documents = from d in db.Documents
                select d;
            if (employeeId != null)
            {
                documents = documents.Where(d => (d.EmployeeId == employeeId));
                if (!string.IsNullOrEmpty(searchString))
                {
                    documents = documents.Where(d => d.Title.Contains(searchString));
                }

                documents = documents.OrderBy(o => o.CreatedAt);

                ViewBag.ProgramId = programId;
                ViewBag.EmployeeId = employeeId;
                ViewBag.PageNumber = pageNumber;
                ViewBag.serachParam = searchString;

                return View(await documents.ToPagedListAsync(pageNumber, pageSize));
            }

            documents = documents.Where(d => (d.ProgramId == programId));
            if (!string.IsNullOrEmpty(searchString))
            {
                documents = documents.Where(d => d.Title.Contains(searchString));
            }

            documents = documents.OrderBy(o => o.CreatedAt);

            ViewBag.ProgramId = programId;
            ViewBag.PageNumber = pageNumber;
            ViewBag.ProgramId = programId;

            return View(await documents.ToPagedListAsync(pageNumber, pageSize));
        }

        //Select the template
        public async Task<ActionResult> Select(int? programId, Guid? employeeId)
        {
            if (programId == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var program = await db.Programs.FindAsync(programId);
            if (program == null) return HttpNotFound();

            var templates = from t in db.Templates
                select t;
            // get the templates matches program type to selected program type
            templates = templates.Where(t => (t.ProgramType == program.ProgramType));
            ViewBag.ProgramId = programId;
            ViewBag.EmployeeId = employeeId;

            return View(await templates.ToListAsync());
        }

        // select columns
        public async Task<ActionResult> Create(int templateId, int programId, Guid? employeeId)
        {
            var template = await db.Templates.FindAsync(templateId);
            if (template == null) return HttpNotFound();


            //create New Document Name
            var documentTitle = template.Title + "-" + programId + "-" + DateTime.Now.ToString("MMddyyyyhhmmss");

            var program = await db.Programs.FindAsync(programId);
            if (program == null) return HttpNotFound();
            var details = "Generated Document For " + program.Title + " From " + template.Title + " template";
            if (employeeId != null)
            {
                details = "Generated Document For employee-" + employeeId + " From " + template.Title + " template";
            }

            var viewModel = new SelectConfirmViewModel
            {
                Template = template,
                Document = new Document()
                {
                    Title = documentTitle,
                    Details = details,
                    FileName = Guid.NewGuid() + "." + FileType.DOCX,
                    ProgramType = program.ProgramType,
                    FileType = FileType.DOCX,
                    FileMethod = FileMethod.Generate,
                    ProgramId = programId,
                    EmployeeId = employeeId,
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

        public async Task<ActionResult> Download(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var doc = await db.Documents.FindAsync(id);
            if(doc == null) return HttpNotFound();

            var path = Server.MapPath("~/Storage/gen/" + doc.FileName);

            var fileBytes = GetDocument(path);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, path);
        }

        private byte[] GetDocument(string path)
        {
            var fs = System.IO.File.OpenRead(path);
            var data = new byte[fs.Length];
            var br = fs.Read(data, 0, data.Length);
            if (br != fs.Length) throw new System.IO.IOException(path);

            return data;
        }


        private async Task<object> InstHelperClass(int programId, Guid? EmployeeId)
        {
            _helperClass = typeof(DocumentHelper);

            if (EmployeeId == null)
                return Activator.CreateInstance(
                    _helperClass,
                    await ProgramsController.GetProgram(programId),
                    await EmployeesController.GetTraineesAsync(programId)
                );

            var emp = await EmployeesController.FindEmployeeAsync((Guid) EmployeeId);

            if (emp != null)
            {
                return Activator.CreateInstance(
                    _helperClass,
                    await ProgramsController.GetProgram(programId),
                    new List<Employee> {emp}
                );
            }

            return Activator.CreateInstance(
                _helperClass,
                await ProgramsController.GetProgram(programId),
                await EmployeesController.GetTraineesAsync(programId)
            );
        }

        // POST: Documents1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            [Bind(Include =
                "Id,Title,Details,FileName,ProgramType,FileType,FileMethod,ProgramId,EmployeeId,DocumentNumber")]
            Document document, TableColumnName[] columns, int templateId, int programId)
        {
            var path = "";
            try
            {
                // document path of the newly created document from the template
                path = await GetDocumentPath(templateId, document.FileName);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", @"Creating a document from the selected template failed.(" + e + ")");
                return RedirectToAction($"Select", $"Documents", new {programId, employeeId = document.EmployeeId});
            }

            //instantiate helper class
            _classInstance = await InstHelperClass(programId, document.EmployeeId);
            /**
             * Process the table
             */
            try
            {
                ProcessVariables(path);
                ProcessLists(path);
                ProcessTables(path, columns, document.EmployeeId);
            }
            catch (Exception e)
            {
                DeleteDocument(path);
                ModelState.AddModelError("", @"Processing the Document failed.(" + e + ")");
                return RedirectToAction($"Select", $"Documents", new {programId, employeeId = document.EmployeeId});
            }

            if (ModelState.IsValid)
            {
                db.Documents.Add(document);
                await db.SaveChangesAsync();
                return RedirectToAction($"Index", $"Documents", new {programId, employeeId = document.EmployeeId});
            }

            ModelState.AddModelError("", @"Document Generation Failed.. Please try again");
            return RedirectToAction($"Select", $"Documents", new {programId, employeeId = document.EmployeeId});
        }

        private static void DeleteDocument(string path)
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
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

        private void ProcessLists(string path)
        {
            using (var wordDoc = WordprocessingDocument.Open(path, true))
            {
                var mainPart = wordDoc.MainDocumentPart;

                foreach (var item in DocumentHelper.DocumentListsList)
                {
                    var res = from p in mainPart.Document.Body.Descendants<Paragraph>()
                        where p.InnerText == item
                        select p;

                    if (!res.Any()) continue;
                    var method = _helperClass.GetMethod(item);
                    if (method == null) continue;

                    var rf = res.First();
                    rf.RemoveAllChildren<Run>();

                    rf.AppendChild(new Run((Paragraph) method.Invoke(_classInstance, null)));
                }
            }
        }

        private void ProcessTables(string path, TableColumnName[] columnNames, Guid? employeeId)
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

                    if (!res.Any()) continue;

                    var method = _helperClass.GetMethod(item);

                    if (method == null) continue;

                    var rf = res.First();

                    rf.RemoveAllChildren<Run>();
                    rf.AppendChild(
                        new Run((Table) method.Invoke(_classInstance, new object[] {columnNames, employeeId})));
                }
            }
        }

        private async Task<string> GetDocumentPath(int templateId, string newFileName)
        {
            var template = await db.Templates.FindAsync(templateId);

            if (template == null) return null;

            var path = Path.Combine(Server.MapPath("~/Storage/templates/"),
                Path.GetFileName(template.FileName + "." + template.FileType));

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
            var document = await db.Documents.FindAsync(id);

            if (document == null)
            {
                ModelState.AddModelError("", @"Document not found in the database.");
                return RedirectToAction($"Delete", $"Documents", new { id });
            }

            var path = Server.MapPath("~/Storage/gen/" + document.FileName);

            if (System.IO.File.Exists(path))
            {
                try
                {
                    System.IO.File.Delete(path);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", @"Document File Failed to delete from the database please try again later.("+e+")");
                    return RedirectToAction($"Delete", $"Documents", new { id });
                }

                db.Documents.Remove(document);
                await db.SaveChangesAsync();
                return RedirectToAction($"Index",$"Documents", new {programId = document.ProgramId, employeeId = document.EmployeeId});
            }

            ModelState.AddModelError("", @"Document Not Found in the System's Storage");
            return RedirectToAction($"Delete", $"Documents", new { id });
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