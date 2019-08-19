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
    public class DefaultColumnsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DefaultColumns
        public async Task<ActionResult> Index()
        {
            var defaultColumns = db.DefaultColumns.Include(d => d.Template);
            return View(await defaultColumns.ToListAsync());
        }

        // GET: DefaultColumns/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DefaultColumn defaultColumn = await db.DefaultColumns.FindAsync(id);
            if (defaultColumn == null)
            {
                return HttpNotFound();
            }
            return View(defaultColumn);
        }

        // GET: DefaultColumns/Create
        public ActionResult Create()
        {
            ViewBag.TemplateId = new SelectList(db.Templates, "Id", "Title");
            return View();
        }

        // POST: DefaultColumns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ColumnName,TemplateId,CreatedAt,UpdatedAt,CreatedBy,UpdatedBy,RowVersion")] DefaultColumn defaultColumn)
        {
            if (ModelState.IsValid)
            {
                db.DefaultColumns.Add(defaultColumn);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.TemplateId = new SelectList(db.Templates, "Id", "Title", defaultColumn.TemplateId);
            return View(defaultColumn);
        }

        // GET: DefaultColumns/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DefaultColumn defaultColumn = await db.DefaultColumns.FindAsync(id);
            if (defaultColumn == null)
            {
                return HttpNotFound();
            }
            ViewBag.TemplateId = new SelectList(db.Templates, "Id", "Title", defaultColumn.TemplateId);
            return View(defaultColumn);
        }

        // POST: DefaultColumns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ColumnName,TemplateId,CreatedAt,UpdatedAt,CreatedBy,UpdatedBy,RowVersion")] DefaultColumn defaultColumn)
        {
            if (ModelState.IsValid)
            {
                db.Entry(defaultColumn).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.TemplateId = new SelectList(db.Templates, "Id", "Title", defaultColumn.TemplateId);
            return View(defaultColumn);
        }

        // GET: DefaultColumns/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DefaultColumn defaultColumn = await db.DefaultColumns.FindAsync(id);
            if (defaultColumn == null)
            {
                return HttpNotFound();
            }
            return View(defaultColumn);
        }

        // POST: DefaultColumns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            DefaultColumn defaultColumn = await db.DefaultColumns.FindAsync(id);
            db.DefaultColumns.Remove(defaultColumn);
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
