using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CECBTIMS.Models;
using CECBTIMS.Models.Enums;

namespace CECBTIMS.Controllers
{
    public class FilesController : Controller
    {
        private CECB_ERPEntities db = new CECB_ERPEntities();

        // GET: Files
//        public async Task<ActionResult> Index()
//        {
//            var files = db.Files.Include(f => f.Program);
//            return View(await files.ToListAsync());
//        }

        // GET: Files/Details/5
//        public async Task<ActionResult> Details(Guid? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            File file = await db.Files.FindAsync(id);
//            if (file == null)
//            {
//                return HttpNotFound();
//            }
//            return View(file);
//        }

        // GET: Files/Create
//        public ActionResult Create()
//        {
//            ViewBag.ProgramId = new SelectList(db.Programs, "Id", "Title");
//            return View();
//        }

        // POST: Files/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Details,FileName,OriginalFileName,FileType,FileMethod,ProgramId,CreatedAt,UpdatedAt,CreatedBy,UpdatedBy,RowVersion")] File file)
//        {
//            if (ModelState.IsValid)
//            {
//                file.Id = Guid.NewGuid();
//                db.Files.Add(file);
//                await db.SaveChangesAsync();
//                return RedirectToAction("Index");
//            }
//
//            ViewBag.ProgramId = new SelectList(db.Programs, "Id", "Title", file.ProgramId);
//            return View(file);
//        }

        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost, ActionName("Upload")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UploadFile(HttpPostedFileBase file,string title,string details,int? programId)
        {
            if (file == null || file.ContentLength <= 0) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var fileExtension = Path.GetExtension(file.FileName)?.Replace(".", string.Empty);

            //check if the file type is supported
            if (!Enum.GetNames(typeof(FileType)).Contains(fileExtension))
            {
                ViewBag.Message = "File type is not supported";
                return View($"Upload");
            }

            generateanewnamebefore store in the database



            var path = Path.Combine(Server.MapPath("~/Storage"), Path.GetFileName(file.FileName) ?? throw new InvalidOperationException());

            try
            {
                file.SaveAs(path);

                ViewBag.Message = "File uploaded successfully";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "ERROR:" + ex.Message.ToString();
            }

            return View($"Upload");
        }


        // GET: Files/Edit/5
//        public async Task<ActionResult> Edit(Guid? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            File file = await db.Files.FindAsync(id);
//            if (file == null)
//            {
//                return HttpNotFound();
//            }
//            ViewBag.ProgramId = new SelectList(db.Programs, "Id", "Title", file.ProgramId);
//            return View(file);
//        }

        // POST: Files/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Details,FileName,OriginalFileName,FileType,FileMethod,ProgramId,CreatedAt,UpdatedAt,CreatedBy,UpdatedBy,RowVersion")] File file)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(file).State = EntityState.Modified;
//                await db.SaveChangesAsync();
//                return RedirectToAction("Index");
//            }
//            ViewBag.ProgramId = new SelectList(db.Programs, "Id", "Title", file.ProgramId);
//            return View(file);
//        }
//
//        // GET: Files/Delete/5
//        public async Task<ActionResult> Delete(Guid? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            File file = await db.Files.FindAsync(id);
//            if (file == null)
//            {
//                return HttpNotFound();
//            }
//            return View(file);
//        }
//
//        // POST: Files/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> DeleteConfirmed(Guid id)
//        {
//            File file = await db.Files.FindAsync(id);
//            db.Files.Remove(file);
//            await db.SaveChangesAsync();
//            return RedirectToAction("Index");
//        }

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
