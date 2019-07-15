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
    public class ResourcePersonsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ResourcePersons
        public async Task<ActionResult> Index()
        {
            return View(await db.ResourcePersons.ToListAsync());
        }

        // GET: ResourcePersons/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResourcePerson resourcePerson = await db.ResourcePersons.FindAsync(id);
            if (resourcePerson == null)
            {
                return HttpNotFound();
            }
            return View(resourcePerson);
        }

        // GET: ResourcePersons/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ResourcePersons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Designation,Cost,CreatedAt,UpdatedAt,CreatedBy,UpdatedBy,RowVersion")] ResourcePerson resourcePerson)
        {
            if (ModelState.IsValid)
            {
                db.ResourcePersons.Add(resourcePerson);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(resourcePerson);
        }

        // GET: ResourcePersons/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResourcePerson resourcePerson = await db.ResourcePersons.FindAsync(id);
            if (resourcePerson == null)
            {
                return HttpNotFound();
            }
            return View(resourcePerson);
        }

        // POST: ResourcePersons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Designation,Cost,CreatedAt,UpdatedAt,CreatedBy,UpdatedBy,RowVersion")] ResourcePerson resourcePerson)
        {
            if (ModelState.IsValid)
            {
                db.Entry(resourcePerson).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(resourcePerson);
        }

        // GET: ResourcePersons/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResourcePerson resourcePerson = await db.ResourcePersons.FindAsync(id);
            if (resourcePerson == null)
            {
                return HttpNotFound();
            }
            return View(resourcePerson);
        }

        // POST: ResourcePersons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            ResourcePerson resourcePerson = await db.ResourcePersons.FindAsync(id);
            db.ResourcePersons.Remove(resourcePerson);
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
