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
    public class TargetGroupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TargetGroups
        public async Task<ActionResult> Index()
        {
            var targetGroups = db.TargetGroups.Include(t => t.Program);
            return View(await targetGroups.ToListAsync());
        }

        // GET: TargetGroups/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TargetGroup targetGroup = await db.TargetGroups.FindAsync(id);
            if (targetGroup == null)
            {
                return HttpNotFound();
            }
            return View(targetGroup);
        }

        // GET: TargetGroups/Create
        public ActionResult Create(int? id)
        {
//            ViewBag.ProgramId = new SelectList(db.Programs, "Id", "Title");
            ViewBag.ProgramId = id;
            return View();
        }
         
        // POST: TargetGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,ProgramId,CreatedAt,UpdatedAt,CreatedBy,UpdatedBy,RowVersion")] TargetGroup targetGroup)
        {
            if (ModelState.IsValid)
            {
                db.TargetGroups.Add(targetGroup);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ProgramId = new SelectList(db.Programs, "Id", "Title", targetGroup.ProgramId);
            return View(targetGroup);
        }

        // GET: TargetGroups/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TargetGroup targetGroup = await db.TargetGroups.FindAsync(id);
            if (targetGroup == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProgramId = new SelectList(db.Programs, "Id", "Title", targetGroup.ProgramId);
            return View(targetGroup);
        }

        // POST: TargetGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,ProgramId,CreatedAt,UpdatedAt,CreatedBy,UpdatedBy,RowVersion")] TargetGroup targetGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(targetGroup).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ProgramId = new SelectList(db.Programs, "Id", "Title", targetGroup.ProgramId);
            return View(targetGroup);
        }

        // GET: TargetGroups/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TargetGroup targetGroup = await db.TargetGroups.FindAsync(id);
            if (targetGroup == null)
            {
                return HttpNotFound();
            }
            return View(targetGroup);
        }

        // POST: TargetGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TargetGroup targetGroup = await db.TargetGroups.FindAsync(id);
            db.TargetGroups.Remove(targetGroup);
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
