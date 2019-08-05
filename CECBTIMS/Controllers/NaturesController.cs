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
    public class NaturesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Natures
        public async Task<ActionResult> Index()
        {
            var employmentNatures = db.EmploymentNatures.Include(e => e.Program);
            return View(await employmentNatures.ToListAsync());
        }

        // GET: Natures/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmploymentNature employmentNature = await db.EmploymentNatures.FindAsync(id);
            if (employmentNature == null)
            {
                return HttpNotFound();
            }
            return View(employmentNature);
        }

        // GET: Natures/Create
        public ActionResult Create()
        {
            ViewBag.ProgramId = new SelectList(db.Programs, "Id", "Title");
            return View();
        }

        // POST: Natures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ProgramId")] EmploymentNature employmentNature)
        {
            if (ModelState.IsValid)
            {
                db.EmploymentNatures.Add(employmentNature);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ProgramId = new SelectList(db.Programs, "Id", "Title", employmentNature.ProgramId);
            return View(employmentNature);
        }

        // GET: Natures/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmploymentNature employmentNature = await db.EmploymentNatures.FindAsync(id);
            if (employmentNature == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProgramId = new SelectList(db.Programs, "Id", "Title", employmentNature.ProgramId);
            return View(employmentNature);
        }

        // POST: Natures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ProgramId")] EmploymentNature employmentNature)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employmentNature).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ProgramId = new SelectList(db.Programs, "Id", "Title", employmentNature.ProgramId);
            return View(employmentNature);
        }

        // GET: Natures/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmploymentNature employmentNature = await db.EmploymentNatures.FindAsync(id);
            if (employmentNature == null)
            {
                return HttpNotFound();
            }
            return View(employmentNature);
        }

        // POST: Natures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            EmploymentNature employmentNature = await db.EmploymentNatures.FindAsync(id);
            db.EmploymentNatures.Remove(employmentNature);
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
