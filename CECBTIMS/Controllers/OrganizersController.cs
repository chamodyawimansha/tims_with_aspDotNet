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
using PagedList.EntityFramework;

namespace CECBTIMS.Controllers
{
    [Authorize]
    public class OrganizersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Organizers
        public async Task<ActionResult> Index(int? programId, string searchString, int? countPerPage, int? page)
        {
            var Orgs = from o in db.Organizers
                select o;

            if (!string.IsNullOrEmpty(searchString))
            {
                Orgs = Orgs.Where(o => o.Name.Contains(searchString));
            }

            Orgs = Orgs.OrderBy(o => o.Name);

            var pageSize = countPerPage ?? 5;
            var pageNumber = page ?? 1;
            ViewBag.PageNumber = pageNumber;
            ViewBag.ProgramId = programId;

            return View(await Orgs.ToPagedListAsync(pageNumber, pageSize));
        }

        // GET: Organizers/Details/5
        public async Task<ActionResult> Details(int? id, int? programId)
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

            ViewBag.ProgramId = programId;

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
            [Bind(Include = "Id,Name,CreatedAt,UpdatedAt,CreatedByApplicationUserId,UpdatedBy,RowVersion")]
            Organizer organizer, int? programId)
        {
            if (!ModelState.IsValid) return View(organizer);

            db.Organizers.Add(organizer);
            await db.SaveChangesAsync();

            return RedirectToAction($"Index");
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
            [Bind(Include = "Id,Name,CreatedAt,UpdatedAt,CreatedByApplicationUserId,UpdatedBy,RowVersion")]
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