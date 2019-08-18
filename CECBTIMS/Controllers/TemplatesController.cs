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
using Microsoft.Ajax.Utilities;

namespace CECBTIMS.Controllers
{
    public class TemplatesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Templates
        public async Task<ActionResult> Index()
        {
            return View(await db.Templates.ToListAsync());
        }

        // GET: Templates/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Template template = await db.Templates.FindAsync(id);
            if (template == null)
            {
                return HttpNotFound();
            }
            return View(template);
        }

        // GET: Templates/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Templates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Title,ProgramType,HasConfigurableTable")] Template template, HttpPostedFileBase file)
        {
            // check if the select program type is a right one
            if (!Enum.GetNames(typeof(ProgramType)).Contains(template.ProgramType.ToString()))
            {
                ModelState.AddModelError("", @"Please select a Program Type for the Template");
                return View(template);
            }
            // check if the template file is present
            if (file == null || file.ContentLength <= 0)
            {
                ModelState.AddModelError("", @"Please select a template to upload");
                return View(template);
            }
            //get the file extension
            var fileExtension = Path.GetExtension(file.FileName.ToUpper())?.Replace(".", string.Empty);
            //check if the file type is supported
            if (!Enum.GetNames(typeof(FileType)).Contains(fileExtension))
            {
                ModelState.AddModelError("", @"Selected Template file type is not supported.");
                return View(template);
            }

            // Generate a new File name
            var fileName = Guid.NewGuid().ToString();
            //create the file path
            var path = Path.Combine(Server.MapPath("~/Storage/templates/"), Path.GetFileName(fileName+"."+fileExtension));
            try
            {
                // save file in the storage
                file.SaveAs(path);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("",@"File upload Failed:" + ex.Message);
                return View(template);
            }

            if (!ModelState.IsValid) return View(template);

            if (!System.IO.File.Exists(path))
            {
                ModelState.AddModelError("", @"File Upload Failed. Try again.");
                return View(template);
            }

            template.Details = "DocumentTemplate";
            template.FileMethod = FileMethod.Upload;
            template.FileType = (FileType)Enum.Parse(typeof(FileType), fileExtension);
            template.OriginalFileName = file.FileName;
            template.FileName = fileName;
            //save the template details in the database

            try
            {
                db.Templates.Add(template);
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                /***
                 *
                 * Delete the file from storage here
                 * and return with message
                 */
            }

            return RedirectToAction($"Index");

        }

        // GET: Templates/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Template template = await db.Templates.FindAsync(id);
            if (template == null)
            {
                return HttpNotFound();
            }
            return View(template);
        }

        // POST: Templates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Details,FileName,ProgramType,FileType,FileMethod,OriginalFileName,HasConfigurableTable,CreatedAt,UpdatedAt,CreatedBy,UpdatedBy,RowVersion")] Template template)
        {
            if (ModelState.IsValid)
            {
                db.Entry(template).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(template);
        }

        // GET: Templates/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Template template = await db.Templates.FindAsync(id);
            if (template == null)
            {
                return HttpNotFound();
            }
            return View(template);
        }

        // POST: Templates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Template template = await db.Templates.FindAsync(id);
            db.Templates.Remove(template);
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
