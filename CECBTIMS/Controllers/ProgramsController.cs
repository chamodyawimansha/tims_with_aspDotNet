using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
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
        public async Task<ActionResult> Index(string sortOrder, int? countPerPage, string currentFilter, string searchString, int? page)
        {
            
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.ClosingSortParm = sortOrder == "ClosingDate" ? "closingdate_desc" : "ClosingDate";
            ViewBag.CreatedSortParm = sortOrder == "CreatedDate" ? "createddate_desc" : "CreatedDate";
            ViewBag.TypeSortParm = sortOrder == "Type" ? "type_desc" : "Type";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var programs = from s in db.Programs
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                programs = programs.Where(p => p.Title.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "title_desc":
                    programs = programs.OrderByDescending(s => s.Title);
                    break;
                case "Date":
                    programs = programs.OrderBy(s => s.StartDate);
                    break;
                case "date_desc":
                    programs = programs.OrderByDescending(s => s.StartDate);
                    break;
                case "ClosingDate":
                    programs = programs.OrderBy(s => s.ApplicationClosingDate);
                    break;
                case "closingdate_desc":
                    programs = programs.OrderByDescending(s => s.ApplicationClosingDate);
                    break;
                case "CreatedDate":
                    programs = programs.OrderBy(s => s.CreatedAt);
                    break;
                case "createddate_desc":
                    programs = programs.OrderByDescending(s => s.CreatedAt);
                    break;
                case "Type":
                    programs = programs.OrderBy(s => s.ProgramType);
                    break;
                case "type_desc":
                    programs = programs.OrderByDescending(s => s.ProgramType);
                    break;
                default:  
                    programs = programs.OrderBy(s => s.Title);
                    break;
            }

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
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,ProgramType,StartDate,ApplicationClosingDate,ApplicationClosingTime,Venue,EndDate,NotifiedBy,NotifiedOn,ProgramHours,DurationInDays,DurationInMonths,Department,Currency,ProgramFee,RegistrationFee,PerPersonFee,NoShowFee,MemberFee,NonMemberFee,StudentFee")] Program program)
        {
            if (ModelState.IsValid)
            {
                db.Programs.Add(program);
                await db.SaveChangesAsync();
                return RedirectToAction($"Index");
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

            ViewBag.ProgramType = (int)program.ProgramType;

            return View(program);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, byte[] rowVersion)
        {

            string[] fieldsToBind = new string[]
            {
                "Title", "ProgramType", "StartDate", "ApplicationClosingDate", "ApplicationClosingTime",
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

                    return RedirectToAction($"Details", new {id});
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

                        if (databaseValues.Title != clientValues.Title)
                        {
                            ModelState.AddModelError("Title", "Current value: " + databaseValues.Title);
                        }
                        if (databaseValues.StartDate != clientValues.StartDate)
                        {
                            ModelState.AddModelError("StartDate", "Current value: " + databaseValues.StartDate);
                        }
                        if (databaseValues.ApplicationClosingDate != clientValues.ApplicationClosingDate)
                        {
                            ModelState.AddModelError("ApplicationClosingDate", "Current value: " + databaseValues.ApplicationClosingDate);
                        }
                        if (databaseValues.ApplicationClosingTime != clientValues.ApplicationClosingTime)
                        {
                            ModelState.AddModelError("ApplicationClosingTime", "Current value: " + databaseValues.ApplicationClosingTime);
                        }
                        if (databaseValues.Venue != clientValues.Venue)
                        {
                            ModelState.AddModelError("Venue", "Current value: " + databaseValues.Venue);
                        }
                        if (databaseValues.EndDate != clientValues.EndDate)
                        {
                            ModelState.AddModelError("EndDate", "Current value: " + databaseValues.EndDate);
                        }
                        if (databaseValues.NotifiedBy != clientValues.NotifiedBy)
                        {
                            ModelState.AddModelError("NotifiedBy", "Current value: " + databaseValues.NotifiedBy);
                        }
                        if (databaseValues.NotifiedOn != clientValues.NotifiedOn)
                        {
                            ModelState.AddModelError("NotifiedOn", "Current value: " + databaseValues.NotifiedOn);
                        }
                        if (databaseValues.ProgramHours != clientValues.ProgramHours)
                        {
                            ModelState.AddModelError("ProgramHours", "Current value: " + databaseValues.ProgramHours);
                        }
                        if (databaseValues.DurationInDays != clientValues.DurationInDays)
                        {
                            ModelState.AddModelError("DurationInDays", "Current value: " + databaseValues.DurationInDays);
                        }
                        if (databaseValues.DurationInMonths != clientValues.DurationInMonths)
                        {
                            ModelState.AddModelError("DurationInMonths", "Current value: " + databaseValues.DurationInMonths);
                        }
                        if (databaseValues.Department != clientValues.Department)
                        {
                            ModelState.AddModelError("Department", "Current value: " + databaseValues.Department);
                        }
                        if (databaseValues.Currency != clientValues.Currency)
                        {
                            ModelState.AddModelError("Currency", "Current value: " + databaseValues.Currency);
                        }
                        if (!databaseValues.ProgramFee.Equals(clientValues.ProgramFee))
                        {
                            ModelState.AddModelError("ProgramFee", "Current value: " + databaseValues.ProgramFee);
                        }
                        if (!databaseValues.RegistrationFee.Equals(clientValues.RegistrationFee))
                        {
                            ModelState.AddModelError("RegistrationFee", "Current value: " + databaseValues.RegistrationFee);
                        }
                        if (!databaseValues.PerPersonFee.Equals(clientValues.PerPersonFee))
                        {
                            ModelState.AddModelError("PerPersonFee", "Current value: " + databaseValues.PerPersonFee);
                        }
                        if (!databaseValues.NoShowFee.Equals(clientValues.NoShowFee))
                        {
                            ModelState.AddModelError("NoShowFee", "Current value: " + databaseValues.NoShowFee);
                        }
                        if (!databaseValues.MemberFee.Equals(clientValues.MemberFee))
                        {
                            ModelState.AddModelError("MemberFee", "Current value: " + databaseValues.MemberFee);
                        }
                        if (!databaseValues.NonMemberFee.Equals(clientValues.NonMemberFee))
                        {
                            ModelState.AddModelError("NonMemberFee", "Current value: " + databaseValues.NonMemberFee);
                        }
                        if (!databaseValues.StudentFee.Equals(clientValues.StudentFee))
                        {
                            ModelState.AddModelError("StudentFee", "Current value: " + databaseValues.StudentFee);
                        }


                        ModelState.AddModelError(string.Empty, "The Program you attempted to edit was modified by another user after you got the original values. The edit operation was canceled and the current values in the database have been displayed. If you still want to edit this record, click the Save button again. Otherwise click the Back Link.");
                        programToUpdate.RowVersion = databaseValues.RowVersion;
                    }
                }
                catch (RetryLimitExceededException  /*error*/)
                {
//                    ModelState.AddModelError(error.Message);
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }

            }

            ViewBag.ProgramType = (int)programToUpdate.ProgramType;
            return View(programToUpdate);
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
            if (program == null)
            {
                return HttpNotFound();
            }

            db.Programs.Remove(program);
            await db.SaveChangesAsync();

            return RedirectToAction("Index");

            // Delete the raltionships
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
