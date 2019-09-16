using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using CECBTIMS.DAL;
using CECBTIMS.Models;
using Microsoft.AspNet.Identity;

namespace CECBTIMS.Controllers
{
    [Authorize]
    public class RequirementsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: Requirements/Create
        public ActionResult Create(int? programId, string programTitle)
        {
            if (programId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.programId = programId;
            ViewBag.programTitle = programTitle;

            return View();
        }

        // POST: Requirements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Name,ProgramId")] Requirement requirement)
        {
            if (ModelState.IsValid)
            {
                requirement.ApplicationUserId = User.Identity.GetUserId();
                db.Requirements.Add(requirement);
                await db.SaveChangesAsync();
            }
            
            return RedirectToAction($"Details", $"Programs", new { id = requirement.ProgramId });
        }

        // POST: Requirements/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, int? ProgramId)
        {
            Requirement requirement = await db.Requirements.FindAsync(id);
            db.Requirements.Remove(requirement);
            await db.SaveChangesAsync();

            return RedirectToAction($"Details", $"Programs", new { id = ProgramId });
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
