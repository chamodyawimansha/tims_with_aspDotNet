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
using CECBTIMS.DAL;
using CECBTIMS.Models;
using CECBTIMS.Models.Enums;

namespace CECBTIMS.Controllers
{
    public class BrochuresController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Brochures/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brochure brochure = await db.Brochures.FindAsync(id);
            if (brochure == null)
            {
                return HttpNotFound();
            }
            return View(brochure);
        }

        // GET: Brochures/Create
        public async Task<ActionResult> Create(int? programId)
        {
            if(programId == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var program = await db.Programs.FindAsync(programId);
            if (program == null)
            {
                return HttpNotFound();
            }

            ViewBag.ProgramId = programId;

            return View();
        }

        // POST: Brochures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Title,Details, ProgramId")] Brochure brochure, HttpPostedFileBase file)
        {
            // check if the template file is present
            if (file == null || file.ContentLength <= 0)
            {
                ModelState.AddModelError("", @"Please select a brochure to upload");
                return View(brochure);
            }
            //get the file extension
            var fileExtension = Path.GetExtension(file.FileName.ToUpper())?.Replace(".", string.Empty);
            //check if the file type is supported
            if (!Enum.GetNames(typeof(FileType)).Contains(fileExtension))
            {
                ModelState.AddModelError("", @"Selected brochure file type is not supported.");
                return View(brochure);
            }

            // Generate a new File name
            var fileName = Guid.NewGuid().ToString();
            //create the file path
            var path = Path.Combine(Server.MapPath("~/Storage/brochures/"), Path.GetFileName(fileName + "." + fileExtension));
            try
            {
                // save file in the storage
                file.SaveAs(path);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", @"File upload Failed:" + ex.Message);
                return View(brochure);
            }

            if (!ModelState.IsValid) return View(brochure);

            if (!System.IO.File.Exists(path))
            {
                ModelState.AddModelError("", @"File Upload Failed. Try again.");
                return View(brochure);
            }

            brochure.Details = "ProgramBrochure";
            brochure.FileMethod = FileMethod.Upload;
            brochure.FileType = (FileType)Enum.Parse(typeof(FileType), fileExtension);
            brochure.OriginalFileName = file.FileName;
            brochure.FileName = fileName;
            //save the template details in the database
            try
            {
                db.Brochures.Add(brochure);
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                ModelState.AddModelError("", @"File Upload Failed. Try again. "+ e);
                return View(brochure);
            }

            return RedirectToAction($"Details", $"Programs", new{id = brochure.ProgramId});
        }

        // GET: Brochures/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brochure brochure = await db.Brochures.FindAsync(id);
            if (brochure == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProgramId = new SelectList(db.Programs, "Id", "Title", brochure.ProgramId);
            return View(brochure);
        }

        // POST: Brochures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Details,FileName,OriginalFileName,FileType,FileMethod,ProgramId,CreatedAt,UpdatedAt,CreatedBy,UpdatedBy,RowVersion")] Brochure brochure)
        {
            if (ModelState.IsValid)
            {
                db.Entry(brochure).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ProgramId = new SelectList(db.Programs, "Id", "Title", brochure.ProgramId);
            return View(brochure);
        }

        // GET: Brochures/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brochure brochure = await db.Brochures.FindAsync(id);
            if (brochure == null)
            {
                return HttpNotFound();
            }
            return View(brochure);
        }

        // POST: Brochures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Brochure brochure = await db.Brochures.FindAsync(id);
            db.Brochures.Remove(brochure);
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
