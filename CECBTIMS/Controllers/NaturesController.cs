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
using Microsoft.AspNet.Identity;

namespace CECBTIMS.Controllers
{
    [Authorize]
    public class NaturesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: Natures/Create
        public ActionResult Create(int? programId)
        {

            if (programId == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.ProgramId = programId;

            return View();
        }

        // POST: Natures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ProgramId,EmpNature")] EmploymentNature employmentNature)
        {

            if (!ModelState.IsValid) return View(employmentNature);

            employmentNature.ApplicationUserId = User.Identity.GetUserId();
            db.EmploymentNatures.Add(employmentNature);
            await db.SaveChangesAsync();

            return RedirectToAction("Details", $"Programs", new { id = employmentNature.ProgramId });
        }

        // POST: Natures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id, int programId)
        {

            var employmentNature = await db.EmploymentNatures.FindAsync(id);
            db.EmploymentNatures.Remove(employmentNature ?? throw new InvalidOperationException());

            await db.SaveChangesAsync();

            return RedirectToAction("Details", $"Programs", new { id = programId });
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
