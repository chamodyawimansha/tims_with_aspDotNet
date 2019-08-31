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
using CECBTIMS.Models.Enums;
using Microsoft.Ajax.Utilities;

namespace CECBTIMS.Controllers
{
    public class ProgramAssignmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        public async Task<ActionResult> Select(Guid? employeeId, int? programId)
        {
            if (employeeId == null || programId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var employee = await EmployeesController.FindEmployee((Guid) employeeId);

            var program = await ProgramsController.GetProgram((int) programId);

            if(program == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.Program = program;
            ViewBag.MemberType = await FindMemberType((Guid)employeeId);
            
            return View(employee);
        }


        public async Task<MemberType> FindMemberType(Guid employeeId, int programId)
        {
            //get the organizers of the selected program
            var org = (from item in (await db.Programs.FindAsync(programId))?.ProgramArrangements select item.Organizer).ToList();


            var employee = from em in db.ProgramAssignments
                select em;
            //find the selected employee program assignment records
            employee = employee.Where(em => em.EmployeeVersionId == employeeId);
            var employeeHisory = await employee.ToListAsync();




            var recentRecord = employeeHisory.First();
            //find the max date
            foreach (var item in employeeHisory.Where(item => item.CreatedAt > recentRecord.CreatedAt))
            {
                recentRecord = item;
            }




            // find it the institutes matches





            return recentRecord.MemberType;

        }

        // POST: ProgramAssignments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EmployeeId,EmployeeVersionId,EPFNo,MemberType,ProgramId")] ProgramAssignment programAssignment)
        {
            if (!ModelState.IsValid) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            db.ProgramAssignments.Add(programAssignment);
            await db.SaveChangesAsync();

            return RedirectToAction($"Index", $"Employees", new { programId = programAssignment.ProgramId });

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
