using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using CECBTIMS.DAL;
using CECBTIMS.Models;

namespace CECBTIMS.Controllers
{
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        // GET: Categories/Create
        public ActionResult Create(int? programId)
        {

            if(programId == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.ProgramId = programId;
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ProgramId,EmpCategory")] EmploymentCategory employmentCategory)
        {
            if (!ModelState.IsValid) return View(employmentCategory);

            db.EmploymentCategories.Add(employmentCategory);
            await db.SaveChangesAsync();

            return RedirectToAction("Details", $"Programs", new { id = employmentCategory.ProgramId });

        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id, int programId)
        {
            
            var employmentCategory = await db.EmploymentCategories.FindAsync(id);
            db.EmploymentCategories.Remove(employmentCategory);

            await db.SaveChangesAsync();

            return RedirectToAction("Details", $"Programs", new {id = programId });
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
