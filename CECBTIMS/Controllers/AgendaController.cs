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
    [Authorize]
    public class AgendaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: Agenda/Create
        public ActionResult Create(int? programId, string programTitle)
        {
            ViewBag.ProgramTitle = programTitle;
            ViewBag.ProgramId = programId;

            return View();
        }

        // POST: Agenda/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,From,To,ProgramId")]
            Agenda agenda)
        {
            if (ModelState.IsValid)
            {
                db.Agenda.Add(agenda);
                await db.SaveChangesAsync();
            }

            return RedirectToAction($"Details", $"Programs", new {id = agenda.ProgramId});
        }

        // GET: Agenda/Edit/5
        public async Task<ActionResult> Edit(int? id, int? programId, string programTitle)
        {
            if (id == null || programId == null) return new System.Web.Mvc.HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var agenda = await db.Agenda.FindAsync(id);
            if (agenda == null) return HttpNotFound();

            ViewBag.ProgramId = programId;
            ViewBag.ProgramTitle = programTitle;

            return View(agenda);
        }

        // POST: Agenda/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,From,To,ProgramId,RowVersion")]
            Agenda agenda)
        {
            if (ModelState.IsValid)
            {
                db.Entry(agenda).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }


            return RedirectToAction($"Details", $"Programs", new {id = agenda.ProgramId});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, int programId)
        {
            var agenda = await db.Agenda.FindAsync(id);
            db.Agenda.Remove(agenda);
            await db.SaveChangesAsync();

            return RedirectToAction($"Details", $"Programs", new {id = programId});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}