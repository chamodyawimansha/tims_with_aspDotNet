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
    public class CostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Costs/Create
        public async Task<ActionResult> Create(int? programId, string programTitle)
        {
            if (programId == null || programId == 0 || programTitle == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // check the program is exists in the database
            var currentProgram = await db.Programs.FindAsync(programId);
            if (currentProgram == null)
            {
                return HttpNotFound();
            }

            ViewBag.ProgramId = programId;
            ViewBag.ProgramTitle = programTitle;

            return View();
        }

        // POST: Costs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Name,Value,ProgramId")] Cost cost)
        {
            if (ModelState.IsValid)
            {
                db.Costs.Add(cost);
                await db.SaveChangesAsync();
            }

            return RedirectToAction($"Details", $"Programs", new { id = cost.ProgramId });
        }

        // GET: Costs/Edit/5
        public async Task<ActionResult> Edit(int? id, int? programId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cost cost = await db.Costs.FindAsync(id);
            if (cost == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProgramId = programId;
            return View(cost);
        }

        // POST: Costs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Value,ProgramId, rowVersion")] Cost cost)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cost).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }


            return RedirectToAction($"Details", $"Programs", new { id = cost.ProgramId });
        }

        // POST: Costs/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            Cost cost = await db.Costs.FindAsync(id);
            db.Costs.Remove(cost);
            await db.SaveChangesAsync();
            return RedirectToAction($"Details", $"Programs", new { id = cost.ProgramId });
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
