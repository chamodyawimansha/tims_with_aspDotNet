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

namespace CECBTIMS.Controllers
{
    public class ProgramAssignmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProgramAssignments
//        public async Task<ActionResult> Index()
//        {
//            var programAssignments = db.ProgramAssignments.Include(p => p.Program);
//            return View(await programAssignments.ToListAsync());
//        }

        // GET: ProgramAssignments/Details/5
//        public async Task<ActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            ProgramAssignment programAssignment = await db.ProgramAssignments.FindAsync(id);
//            if (programAssignment == null)
//            {
//                return HttpNotFound();
//            }
//            return View(programAssignment);
//        }

        // GET: ProgramAssignments/Create
//        public ActionResult Create()
//        {
//            ViewBag.ProgramId = new SelectList(db.Programs, "Id", "Title");
//            return View();
//        }

        // POST: ProgramAssignments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EmployeeId,EmployeeVersionId,EPFNo,ProgramId")] ProgramAssignment programAssignment)
        {
            if (ModelState.IsValid)
            {
                db.ProgramAssignments.Add(programAssignment);
                await db.SaveChangesAsync();

                return RedirectToAction($"Index", $"Employees", new { programId = programAssignment.ProgramId });
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: ProgramAssignments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProgramAssignment programAssignment = await db.ProgramAssignments.FindAsync(id);
            if (programAssignment == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProgramId = new SelectList(db.Programs, "Id", "Title", programAssignment.ProgramId);
            return View(programAssignment);
        }

        // POST: ProgramAssignments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,EmployeeId,EmployeeVersionId,EPFNo,ProgramId,CreatedAt,UpdatedAt,CreatedBy,UpdatedBy,RowVersion")] ProgramAssignment programAssignment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(programAssignment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ProgramId = new SelectList(db.Programs, "Id", "Title", programAssignment.ProgramId);
            return View(programAssignment);
        }

        // GET: ProgramAssignments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProgramAssignment programAssignment = await db.ProgramAssignments.FindAsync(id);
            if (programAssignment == null)
            {
                return HttpNotFound();
            }
            return View(programAssignment);
        }

        // POST: ProgramAssignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ProgramAssignment programAssignment = await db.ProgramAssignments.FindAsync(id);
            db.ProgramAssignments.Remove(programAssignment);
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
