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
    public class ProgramsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Programs
        public async Task<ActionResult> Index(string search, int? rowCount, string sortOrder, string currentFilter, int? page)
        {
            // dont get all the columns
            //Pages
            //Sorting
            // Program index, Program Type, Title, Start Date, Application Closing date, Program Organiser, Action links
            // Search field
            //Load count 5, 10, 50, 100

            ViewBag.CurrentSort = sortOrder;
            ViewBag.serachParam = search != "" ? search : null;
            ViewBag.TitleSortParm = "new";
            ViewBag.TypeSortParm = "";
            ViewBag.StartDateSortParm = "";
            ViewBag.ClosingDateSortParm = "";
            ViewBag.CreatedDateSortParm = "";


            var programs = from p in db.Programs
                select p;
            if (!String.IsNullOrEmpty(search))
            {
                programs = programs.Where(p => p.Title.Contains(search));
            }

            return View(await programs.ToArrayAsync());
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
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,ProgramType,StartDate,ApplicationClosingDate,ApplicationClosingTime,Brochure,EmploymentNature,EmployeeCategory,Venue,EndDate,NotifiedBy,NotifiedOn,ProgramHours,DurationInDays,DurationInMonths,Department,Currency,ProgramFee,RegistrationFee,PerPersonFee,NoShowFee,MemberFee,NonMemberFee,StudentFee,CreatedAt,UpdatedAt,CreatedBy,UpdatedBy,RowVersion")] Program program)
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,ProgramType,StartDate,ApplicationClosingDate,ApplicationClosingTime,Brochure,EmploymentNature,EmployeeCategory,Venue,EndDate,NotifiedBy,NotifiedOn,ProgramHours,DurationInDays,DurationInMonths,Department,Currency,ProgramFee,RegistrationFee,PerPersonFee,NoShowFee,MemberFee,NonMemberFee,StudentFee,CreatedAt,UpdatedAt,CreatedBy,UpdatedBy,RowVersion")] Program program)
        {
            if (ModelState.IsValid)
            {
                db.Entry(program).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(program);
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
