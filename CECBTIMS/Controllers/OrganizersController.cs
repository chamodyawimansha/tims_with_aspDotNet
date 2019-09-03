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
    public class OrganizersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Organizers
        public async Task<ActionResult> Index(int? programId)
        {
            ViewBag.ProgramId = programId;

            return View(await db.Organizers.ToListAsync());
        }

        // GET: Organizers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Organizer organizer = await db.Organizers.FindAsync(id);
            if (organizer == null)
            {
                return HttpNotFound();
            }

            return View(organizer);
        }

        // GET: Organizers/Create
        public ActionResult Create(int? programId)
        {
            ViewBag.ProgramId = programId;
            return View();
        }

        // POST: Organizers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            [Bind(Include = "Id,Name,CreatedAt,UpdatedAt,CreatedBy,UpdatedBy,RowVersion")]
            Organizer organizer, int? programId)
        {
            if (!ModelState.IsValid) return View(organizer);

            db.Organizers.Add(organizer);
            await db.SaveChangesAsync();
            return programId == null ? RedirectToAction($"Index") : RedirectToAction("SelectOrg", "Programs", new {programId, orgId = organizer.Id });
        }

        // GET: Organizers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Organizer organizer = await db.Organizers.FindAsync(id);
            if (organizer == null)
            {
                return HttpNotFound();
            }

            return View(organizer);
        }

        // POST: Organizers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            [Bind(Include = "Id,Name,CreatedAt,UpdatedAt,CreatedBy,UpdatedBy,RowVersion")]
            Organizer organizer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(organizer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(organizer);
        }

        // GET: Organizers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var organizer = await db.Organizers.FindAsync(id);
            if (organizer == null)
            {
                return HttpNotFound();
            }
            
            return View(organizer);
        }

        // POST: Organizers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var organizer = await db.Organizers.FindAsync(id);

            if(organizer.Programs.Any())
            {
                ModelState.AddModelError("", @"Selected Organiser has Training Programs");
                View(organizer);
            } 

            db.Organizers.Remove(organizer);
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