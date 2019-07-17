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
    public class TargetGroupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TargetGroups/Create
        public ActionResult Create(int? programId, string title)
        {

            if (programId == null)
            {
                return new System.Web.Mvc.HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.ProgramId = programId;
            ViewBag.programTitle = title;

            return View();
        }
         
        // POST: TargetGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Name,ProgramId")] TargetGroup targetGroup)
        {
            if (ModelState.IsValid)
            {
                db.TargetGroups.Add(targetGroup);
                await db.SaveChangesAsync();
            }

            return RedirectToAction("Details", "Programs",new{id = targetGroup.ProgramId});
        }

        // GET: TargetGroups/Edit/5
        public async Task<ActionResult> Edit(int? id, string title)
        {
            if (id == null)
            {
                return new System.Web.Mvc.HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TargetGroup targetGroup = await db.TargetGroups.FindAsync(id);
            if (targetGroup == null)
            {
                return HttpNotFound();
            }

            ViewBag.programTitle = title;

            return View(targetGroup);
        }

        // POST: TargetGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,ProgramId,RowVersion")] TargetGroup targetGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(targetGroup).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Details", "Programs", new { id = targetGroup.ProgramId });
        }

        // GET: TargetGroups/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new System.Web.Mvc.HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TargetGroup targetGroup = await db.TargetGroups.FindAsync(id);
            if (targetGroup == null)
            {
                return HttpNotFound();
            }
            return View(targetGroup);
        }

        // POST: TargetGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id,int ProgramId)
        {
            TargetGroup targetGroup = await db.TargetGroups.FindAsync(id);
            db.TargetGroups.Remove(targetGroup);
            await db.SaveChangesAsync();
            return RedirectToAction("Details", "Programs", new { id = ProgramId });
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
