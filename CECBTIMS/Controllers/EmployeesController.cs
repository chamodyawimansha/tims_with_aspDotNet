using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using CECBTIMS.DAL;
using CECBTIMS.Models;
using CECBTIMS.Models.Enums;

namespace CECBTIMS.Controllers
{
    public class EmployeesController : Controller
    {
        private CECB_ERPEntities db = new CECB_ERPEntities();
        private ApplicationDbContext default_db = new ApplicationDbContext();

        public async Task<ActionResult> Index(int? programId)
        {
//
//            get the employee ids from the program assignments
//                get the inforamtio from the cecb db for each one
//                    send it to index page
//                        show program title
//                        employee count
//
//                            full name
//                                


            return Content("Saved" + programId.ToString());
        }

        // GET: Employee/Details
            public async Task<ActionResult> Details(string method, string q, int? programId)
        {
            ViewBag.ProgramId = null;

            if (programId != null)
            {
                //check the program available in the database and the assign the program id to viewbag
                Program program = await default_db.Programs.FindAsync(programId);
                ViewBag.ProgramId = program != null ? programId : null;
            }

            if (string.IsNullOrEmpty(method) || string.IsNullOrEmpty(q))
            {
                return View($"Details");
            }

            var employees = from em in db.cmn_EmployeeVersion
                            select em;

            switch (method)
            {
                case "EPFNo":
                    employees = employees.Where(em => em.EPFNo.Contains(q));
                    break;
                case "NIC":
                    employees = employees.Where(em => em.NIC.Contains(q));
                    break;
                default:
                    employees = employees.Where(em => em.FullName.Contains(q));
                    break;
            }

            return View($"Details", await employees.ToListAsync());

        }

        /**
         * Get More Details from the db
         */
        public async Task<ActionResult> MoreDetails(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var employee = await db.cmn_EmployeeVersion.FindAsync(id);

            if (employee == null)
            {
                return HttpNotFound();
            }

            return View($"Single",employee);

        }

        //            https://www.guru99.com/c-sharp-serialization.html


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