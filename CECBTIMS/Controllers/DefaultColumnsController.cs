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
        public async Task<ActionResult> Index(Guid? id)
        {
            if(id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //get the template
            var template = await db.Files.FindAsync(id);
            if (template == null) return HttpNotFound();
            //return default column
            
            return View(template);
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
            return View();
        }

        // POST: DefaultColumns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,TimsFileId,ColumnName")] DefaultColumn defaultColumn)
        {
            if (ModelState.IsValid)
            {
                db.DefaultColumns.Add(defaultColumn);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

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
            return View(defaultColumn);
        }

        // POST: DefaultColumns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,TimsFileId,ColumnName")] DefaultColumn defaultColumn)
        {
            if (ModelState.IsValid)
            {
                db.Entry(defaultColumn).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
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
