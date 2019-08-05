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
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        // GET: Categories/Create
        public ActionResult Create(int? programId)
        {

            if(programId == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.ProgramId = programId;


            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ProgramId,EmpCategory")] EmploymentCategory employmentCategory)
        {
            if (!ModelState.IsValid) return View(employmentCategory);

            db.EmploymentCategories.Add(employmentCategory);
            await db.SaveChangesAsync();

            return RedirectToAction("Details", $"Programs", new { id = employmentCategory.ProgramId });

        }

        // GET: Categories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmploymentCategory employmentCategory = await db.EmploymentCategories.FindAsync(id);
            if (employmentCategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProgramId = new SelectList(db.Programs, "Id", "Title", employmentCategory.ProgramId);
            return View(employmentCategory);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ProgramId,CreatedAt,UpdatedAt,CreatedBy,UpdatedBy,RowVersion")] EmploymentCategory employmentCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employmentCategory).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ProgramId = new SelectList(db.Programs, "Id", "Title", employmentCategory.ProgramId);
            return View(employmentCategory);
        }

        // GET: Categories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmploymentCategory employmentCategory = await db.EmploymentCategories.FindAsync(id);
            if (employmentCategory == null)
            {
                return HttpNotFound();
            }
            return View(employmentCategory);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            EmploymentCategory employmentCategory = await db.EmploymentCategories.FindAsync(id);
            db.EmploymentCategories.Remove(employmentCategory);
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
