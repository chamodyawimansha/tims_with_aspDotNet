using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CECBTIMS.DAL;
using CECBTIMS.Models;

namespace CECBTIMS.Controllers
{
    [Authorize]
    public class DefaultColumnsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DefaultColumns
        public async Task<ActionResult> Index(int? templateId)
        {
            if (templateId == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var template = await db.Templates.FindAsync(templateId);

            if(template == null) return HttpNotFound();

            ViewBag.Template = template;

            return View(template.DefaultColumns);
        }
        
        // GET: DefaultColumns/Create
        public async Task<ActionResult> Create(int? templateId)
        {
            if(templateId == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var template = await db.Templates.FindAsync(templateId);

            if(template == null) return HttpNotFound();

            ViewBag.TemplateId = templateId;
            return View();
        }

        // POST: DefaultColumns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ColumnName,TemplateId")] DefaultColumn defaultColumn)
        {

            if (ModelState.IsValid)
            {
                db.DefaultColumns.Add(defaultColumn);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { templateId = defaultColumn.TemplateId});
            }

            ViewBag.TemplateId = defaultColumn.TemplateId;
            return View(defaultColumn);
        }

        // POST: DefaultColumns/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            var defaultColumn = await db.DefaultColumns.FindAsync(id);

            db.DefaultColumns.Remove(defaultColumn);
            await db.SaveChangesAsync();

            return RedirectToAction("Index", new{templateId = defaultColumn.TemplateId});
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
