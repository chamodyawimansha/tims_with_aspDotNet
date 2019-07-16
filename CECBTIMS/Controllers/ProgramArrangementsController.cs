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
    public class ProgramArrangementsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProgramArrangements
        public async Task<ActionResult> Index()
        {
            var programArrangements = db.ProgramArrangements.Include(p => p.Organizer).Include(p => p.Program);
            return View(await programArrangements.ToListAsync());
        }

        // GET: ProgramArrangements/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProgramArrangement programArrangement = await db.ProgramArrangements.FindAsync(id);
            if (programArrangement == null)
            {
                return HttpNotFound();
            }
            return View(programArrangement);
        }

        // GET: ProgramArrangements/CreateGet
        public async Task<ActionResult> CreateGet([Bind(Include = "ProgramId,OrganizerId")] ProgramArrangement programArrangement)
        {
            //check the ids available
            if (programArrangement.ProgramId == 0 || programArrangement.OrganizerId == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // check the program and org are exists in the database
            var currentProgram = await db.Programs.FindAsync(programArrangement.ProgramId);
            var currentOrg = await db.Organizers.FindAsync(programArrangement.OrganizerId);

            if (currentProgram == null || currentOrg == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                db.ProgramArrangements.Add(programArrangement);
                await db.SaveChangesAsync();
            }

            return RedirectToAction($"Details", $"Programs", new { id = programArrangement.ProgramId });
        }

        // POST: ProgramArrangements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ProgramId,OrganizerId")] ProgramArrangement programArrangement)
        {
            if (ModelState.IsValid)
            {
                db.ProgramArrangements.Add(programArrangement);
                await db.SaveChangesAsync();
            }

            return RedirectToAction($"Details", $"Programs", new { id = programArrangement.ProgramId});

        }

        // GET: ProgramArrangements/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProgramArrangement programArrangement = await db.ProgramArrangements.FindAsync(id);
            if (programArrangement == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrganizerId = new SelectList(db.Organizers, "Id", "Name", programArrangement.OrganizerId);
            ViewBag.ProgramId = new SelectList(db.Programs, "Id", "Title", programArrangement.ProgramId);
            return View(programArrangement);
        }

        // POST: ProgramArrangements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ProgramId,OrganizerId,CreatedAt,UpdatedAt,CreatedBy,UpdatedBy,RowVersion")] ProgramArrangement programArrangement)
        {
            if (ModelState.IsValid)
            {
                db.Entry(programArrangement).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.OrganizerId = new SelectList(db.Organizers, "Id", "Name", programArrangement.OrganizerId);
            ViewBag.ProgramId = new SelectList(db.Programs, "Id", "Title", programArrangement.ProgramId);
            return View(programArrangement);
        }

        // GET: ProgramArrangements/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProgramArrangement programArrangement = await db.ProgramArrangements.FindAsync(id);
            if (programArrangement == null)
            {
                return HttpNotFound();
            }
            return View(programArrangement);
        }

        // POST: ProgramArrangements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int? id, int programId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProgramArrangement programArrangement = await db.ProgramArrangements.FindAsync(id);

            if (programArrangement == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            db.ProgramArrangements.Remove(programArrangement);
            await db.SaveChangesAsync();

            return RedirectToAction($"Details", $"Programs", new { id = programId });


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
