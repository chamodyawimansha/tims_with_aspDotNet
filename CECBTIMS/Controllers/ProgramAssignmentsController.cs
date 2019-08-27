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


        public ActionResult Select()
        {

        }
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

        // POST: ProgramAssignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid? EmployeeVersionId, int? programId)
        {
            if (EmployeeVersionId == null || programId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var programAssignment = await db.ProgramAssignments.Where(a => a.EmployeeVersionId == EmployeeVersionId).FirstAsync();
            db.ProgramAssignments.Remove(programAssignment);
            await db.SaveChangesAsync();

            //send success message
            return RedirectToAction($"Index",$"Employees", new{programId});
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
