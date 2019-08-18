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
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Details,FileName,ProgramType,FileType,FileMethod,OriginalFileName,HasConfigurableTable,CreatedAt,UpdatedAt,CreatedBy,UpdatedBy,RowVersion")] Template template)
        {
            if (ModelState.IsValid)
            {
                db.Templates.Add(template);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(template);
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
