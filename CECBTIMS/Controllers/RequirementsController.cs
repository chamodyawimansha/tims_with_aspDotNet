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
    public class RequirementsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Requirements
        public async Task<ActionResult> Index()
        {
            var requirements = db.Requirements.Include(r => r.Program);
            return View(await requirements.ToListAsync());
        }

        // GET: Requirements/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Requirement requirement = await db.Requirements.FindAsync(id);
            if (requirement == null)
            {
                return HttpNotFound();
            }
            return View(requirement);
        }

        // GET: Requirements/Create
        public ActionResult Create(int? programId, string programTitle)
        {
            if (programId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.programId = programId;
            ViewBag.programTitle = programTitle;

            return View();
        }

        // POST: Requirements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Name,ProgramId")] Requirement requirement)
        {
            if (ModelState.IsValid)
            {
                db.Requirements.Add(requirement);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ProgramId = new SelectList(db.Programs, "Id", "Title", requirement.ProgramId);
            return View(requirement);
        }

        // GET: Requirements/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Requirement requirement = await db.Requirements.FindAsync(id);
            if (requirement == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProgramId = new SelectList(db.Programs, "Id", "Title", requirement.ProgramId);
            return View(requirement);
        }

        // POST: Requirements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,ProgramId,CreatedAt,UpdatedAt,CreatedBy,UpdatedBy,RowVersion")] Requirement requirement)
        {
            if (ModelState.IsValid)
            {
                db.Entry(requirement).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ProgramId = new SelectList(db.Programs, "Id", "Title", requirement.ProgramId);
            return View(requirement);
        }

        // GET: Requirements/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Requirement requirement = await db.Requirements.FindAsync(id);
            if (requirement == null)
            {
                return HttpNotFound();
            }
            return View(requirement);
        }

        // POST: Requirements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Requirement requirement = await db.Requirements.FindAsync(id);
            db.Requirements.Remove(requirement);
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
