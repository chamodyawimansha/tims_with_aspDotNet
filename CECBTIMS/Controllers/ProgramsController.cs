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
                return new System.Web.Mvc.HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var program = await db.Programs.FindAsync(id);
            if (program == null)
            {
                return HttpNotFound();
            }

            return View(program);
        }

        // GET: Programs/Create
        public ActionResult Create(int? programType)
        {
            if (programType == null || !(programType >= 1) || !(programType <= 4))
            {
                return View($"Select");
            }

            ViewBag.ProgramType = programType;

            return View($"Create");
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
                return new System.Web.Mvc.HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Program program = await db.Programs.FindAsync(id);
            if (program == null)
            {
                return HttpNotFound();
            }

            ViewBag.ProgramType = program.ProgramType;

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
                Program deletedP = new Program();
                TryUpdateModel(deletedP, fieldsToBind);
                ModelState.AddModelError(string.Empty, "Unable to save changes. The Program was deleted by another user.");

                return View(deletedP);
            }

            if (TryUpdateModel(programToUpdate, fieldsToBind))
            {
                try
                {
                    db.Entry(programToUpdate).OriginalValues["RowVersion"] = rowVersion;
                    await db.SaveChangesAsync();

                    return RedirectToAction($"Details", new {id = id});
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var clientValues = (Program)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();

                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty,
                            "Unable to save changes. The department was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Program)databaseEntry.ToObject();
                    
                        ModelState.AddModelError(string.Empty, "The Program you attempted to edit was modified by another user after you got the original values. The edit operation was canceled and the current values in the database have been displayed. If you still want to edit this record, click the Save button again. Otherwise click the Back to List hyperlink.");
                        programToUpdate.RowVersion = databaseValues.RowVersion;
                    }
                }
                catch (RetryLimitExceededException  error)
                {
//                    ModelState.AddModelError(error.Message);
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }

            }

            return View(programToUpdate);
        }

        // GET: Programs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new System.Web.Mvc.HttpStatusCodeResult(HttpStatusCode.BadRequest);
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
