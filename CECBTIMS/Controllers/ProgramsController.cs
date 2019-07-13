using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
    public class ProgramsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Programs
        public async Task<ActionResult> Index(string search, int? rowCount, string sortOrder, string currentFilter, int? page, int? countPerPage)
        {
            /**
             *
             *
             * NEED SORTING
             *
             *
             */
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


            var programs = from p in db.Programs
                select p;
            if (!String.IsNullOrEmpty(search))
            {
                programs = programs.Where(p => p.Title.Contains(search));
            }

            programs = programs.OrderBy(s => s.Title);

            var pageSize = countPerPage ?? 5;
            var pageNumber = page ?? 1;


            return View(await programs.ToPagedListAsync(pageNumber, pageSize));
        }

        // GET: Programs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Program program = await db.Programs.FindAsync(id);
            if (program == null)
            {
                return HttpNotFound();
            }
            return View(program);
        }

        // GET: Programs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Programs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,ProgramType,StartDate,ApplicationClosingDate,ApplicationClosingTime,Brochure,Venue,EndDate,NotifiedBy,NotifiedOn,ProgramHours,DurationInDays,DurationInMonths,Department,Currency,ProgramFee,RegistrationFee,PerPersonFee,NoShowFee,MemberFee,NonMemberFee,StudentFee")] Program program)
        {

            if (ModelState.IsValid)
            {
                db.Programs.Add(program);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(program);
        }

        // GET: Programs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Program program = await db.Programs.FindAsync(id);
            if (program == null)
            {
                return HttpNotFound();
            }
            return View(program);
        }

        // POST: Programs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //        [HttpPost]
        //        [ValidateAntiForgeryToken]
        //        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,ProgramType,StartDate,ApplicationClosingDate,ApplicationClosingTime,Brochure,Venue,EndDate,NotifiedBy,NotifiedOn,ProgramHours,DurationInDays,DurationInMonths,Department,Currency,ProgramFee,RegistrationFee,PerPersonFee,NoShowFee,MemberFee,NonMemberFee,StudentFee,RowVersion")] Program program)
        //        {
        //            if (ModelState.IsValid)
        //            {
        //                db.Entry(program).State = EntityState.Modified;
        //                await db.SaveChangesAsync();
        //                return RedirectToAction("Index");
        //            }
        //            return View(program);
        //            
        //        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, byte[] rowVersion)
        {

            string[] fieldsToBind = new string[]
            {
                "Title", "ProgramType", "StartDate", "ApplicationClosingDate", "ApplicationClosingTime", "Brochure",
                "Venue", "EndDate", "NotifiedBy", "NotifiedOn", "ProgramHours", "DurationInDays", "DurationInMonths",
                "Department", "Currency", "ProgramFee", "RegistrationFee", "PerPersonFee", "NoShowFee", "MemberFee",
                "NonMemberFee", "StudentFee", "RowVersion"
            };

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var programToUpdate = await db.Programs.FindAsync(id);
            if (programToUpdate == null)
            {
//                Department deletedDepartment = new Department();
//                TryUpdateModel(deletedDepartment, fieldsToBind);
//                ModelState.AddModelError(string.Empty,
//                    "Unable to save changes. The department was deleted by another user.");
//                ViewBag.InstructorID = new SelectList(db.Instructors, "ID", "FullName", deletedDepartment.InstructorID);
//                return View(deletedDepartment);
                return Content("Program Not Found");
            }

//            if (TryUpdateModel(departmentToUpdate, fieldsToBind))
//            {
//                try
//                {
//                    db.Entry(departmentToUpdate).OriginalValues["RowVersion"] = rowVersion;
//                    await db.SaveChangesAsync();
//
//                    return RedirectToAction("Index");
//                }
//                catch (DbUpdateConcurrencyException ex)
//                {
//                    var entry = ex.Entries.Single();
//                    var clientValues = (Department)entry.Entity;
//                    var databaseEntry = entry.GetDatabaseValues();
//                    if (databaseEntry == null)
//                    {
//                        ModelState.AddModelError(string.Empty,
//                            "Unable to save changes. The department was deleted by another user.");
//                    }
//                    else
//                    {
//                        var databaseValues = (Department)databaseEntry.ToObject();
//
//                        if (databaseValues.Name != clientValues.Name)
//                            ModelState.AddModelError("Name", "Current value: "
//                                + databaseValues.Name);
//                        if (databaseValues.Budget != clientValues.Budget)
//                            ModelState.AddModelError("Budget", "Current value: "
//                                + String.Format("{0:c}", databaseValues.Budget));
//                        if (databaseValues.StartDate != clientValues.StartDate)
//                            ModelState.AddModelError("StartDate", "Current value: "
//                                + String.Format("{0:d}", databaseValues.StartDate));
//                        if (databaseValues.InstructorID != clientValues.InstructorID)
//                            ModelState.AddModelError("InstructorID", "Current value: "
//                                + db.Instructors.Find(databaseValues.InstructorID).FullName);
//                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
//                            + "was modified by another user after you got the original value. The "
//                            + "edit operation was canceled and the current values in the database "
//                            + "have been displayed. If you still want to edit this record, click "
//                            + "the Save button again. Otherwise click the Back to List hyperlink.");
//                        departmentToUpdate.RowVersion = databaseValues.RowVersion;
//                    }
//                }
            return Content("Program Found");

        }

        // GET: Programs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Program program = await db.Programs.FindAsync(id);
            if (program == null)
            {
                return HttpNotFound();
            }
            return View(program);
        }

        // POST: Programs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Program program = await db.Programs.FindAsync(id);
            db.Programs.Remove(program);
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
