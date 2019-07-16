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
using Microsoft.Ajax.Utilities;
using PagedList.EntityFramework;

namespace CECBTIMS.Controllers
{
    public class OrganizersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Organizers
        public async Task<ActionResult> Index(int? programId, string search, int? rowCount, string sortOrder, string currentFilter, int? page, int? countPerPage)
        {
            if (programId == null || programId == 0)
            {
                ViewBag.ProgramId = null;
            }

            //check the availability oif the program before adding the relationship

            ViewBag.CurrentSort = sortOrder;
            ViewBag.serachParam = search != "" ? search : null;


            if (search != null)
            {
                page = 1;
            }
            else
            {
                search = currentFilter;
            }

            ViewBag.CurrentFilter = search;
            // entry count for entry count selector
            ViewBag.entryCount = countPerPage ?? 5;


            var orgs = from o in db.Organizers
                select o;
            if (!String.IsNullOrEmpty(search))
            {
                orgs = orgs.Where(p => p.Name.Contains(search));
            }

            orgs = orgs.OrderBy(s => s.Name);

            var pageSize = countPerPage ?? 5;
            var pageNumber = page ?? 1;


            return View(await orgs.ToPagedListAsync(pageNumber, pageSize));
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
        public async Task<ActionResult> Create(int? programId,string programTitle)
        {
            if (programId == null || programTitle == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.ProgramTitle = programTitle;
            ViewBag.ProgramId = programId;
            ViewBag.OrgsList = await db.Organizers.ToListAsync();

            return View();
        }

        // POST: Organizers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Name")] Organizer organizer, int? programId)
        {
            
            if (ModelState.IsValid)
            {
                db.Organizers.Add(organizer);
                await db.SaveChangesAsync();
            }

            return RedirectToAction($"CreateGet", $"ProgramArrangements", new { ProgramId = programId, OrganizerId = organizer.Id });
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
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,RowVersion")] Organizer organizer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(organizer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(organizer);
        }

        // POST: Organizers/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            Organizer organizer = await db.Organizers.FindAsync(id);
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
