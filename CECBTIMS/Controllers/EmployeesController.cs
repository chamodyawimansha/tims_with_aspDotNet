using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using CECBTIMS.DAL;
using CECBTIMS.Models;
using CECBTIMS.Models.Enums;
using CECBTIMS.ViewModels;
using PagedList;
using PagedList.EntityFramework;

namespace CECBTIMS.Controllers
{
    public class EmployeesController : Controller
    {
        private CECB_ERPEntities db = new CECB_ERPEntities();
        private ApplicationDbContext default_db = new ApplicationDbContext();
        private static CECB_ERPEntities dbs = new CECB_ERPEntities();
        private static ApplicationDbContext default_dbs = new ApplicationDbContext();

        public async Task<ActionResult> Index(string searchString, int? countPerPage,
            int? page)
        {
            
            var pageSize = countPerPage ?? 5;
            var pageNumber = page ?? 1;

            var employees = from em in db.cmn_EmployeeVersion
                select em;

            if (!string.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(e => e.FullName.Contains(searchString));
            }

            employees = employees.OrderBy(o => o.EPFNo);

            var empList = await employees.ToListAsync();
            
            var empPureList = empList.Select(employee => new Employee
                {
                    EmployeeId = employee.EmployeeVersionId,
                    EPFNo = employee.EPFNo,
                    Title = int.TryParse(employee.Title, out var t) ? (Title)t : Title.Null,
                    NameWithInitial = employee.NameWithInitial,
                    FullName = employee.FullName,
                    NIC = employee.NIC,
                    WorkSpaceName = employee.cmn_WorkSpace != null ? employee.cmn_WorkSpace.WorkSpaceName : "Null",
                    WorkSpaceType = employee.cmn_WorkSpace != null
                        ? employee.cmn_WorkSpace.cmn_WorkSpaceType.WorkSpaceTypeName
                        : "Null",
                    DesignationName = employee.hrm_Designation != null ? employee.hrm_Designation.DesignationName : "Null",
                    EmployeeRecruitmentType = int.TryParse(employee.EmployeeRecruitmentType, out var rt)
                        ? (RecruitmentType)rt
                        : RecruitmentType.Null,
                    EmpStatus = (EmployeeStatus)employee.EmpStatus,
                    Grade = employee.hrm_Grade != null ? employee.hrm_Grade.GradeName : "Null",
                    DateOfAppointment = employee.DateOfAppointment,
                    DateOfJoint = employee.EffectiveDate,
                    TypeOfContract = employee.TypeOfContract,
                    OfficeEmail = employee.OfficeEmail,
                    MobileNumber = employee.MobileNumber,
                    PrivateEmail = employee.PrivateEmail
                })
                .ToList();

            ViewBag.PageNumber = pageNumber;
            ViewBag.serachParam = searchString;


            return View(empPureList.ToPagedList(pageNumber, pageSize));

        }
        public async Task<ActionResult> Trainees(int? programId)
        {
            if (programId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //get the program
            var program = await default_db.Programs.FindAsync(programId);

            if (program == null)
            {
                return new HttpNotFoundResult();
            }

            // get program assignments
            var programAssignments =
                await default_db.ProgramAssignments.Where(p => p.ProgramId == programId).ToListAsync();

            var trainees = new List<cmn_EmployeeVersion>();

            foreach (var item in programAssignments)
            {
                // get trainee data from the cecb database
                trainees.Add(await db.cmn_EmployeeVersion.FindAsync(item.EmployeeVersionId));
            }

            //
            //            get the employee ids from the program assignments
            //                get the inforamtio from the cecb db for each one
            //                    send it to index page
            //                        show program title
            //                        employee count
            //
            //                            full name
            //                                

            ViewBag.Program = program;
            return View(trainees);
        }

        // GET: Employee/Details
        public async Task<ActionResult> Details(Guid? id, int? programId)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var employee = await FindEmployeeAsync((Guid)id);

            if (employee == null) return HttpNotFound();

            ViewBag.ProgramId = programId;

            return View(new EmployeeDetailsViewModel()
            {
                Programs = await GetTraineeHistory((Guid)id),
                Employee = employee
            });

        }

        private async Task<List<Program>> GetTraineeHistory(Guid id)
        {
            var ps = from p in default_db.ProgramAssignments
                select p;
            ps = ps.Where(p => p.EmployeeVersionId == id);

            var psr = await ps.ToListAsync();

            var pp = psr.Select(item => item.Program).ToList();

            return (pp);
        }

        //Get
        public ActionResult Find(int? programId)
        {
            if (programId == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            ViewBag.ProgramId = programId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Find(string method, int programId, string searchString)
        {
            var employees = from em in db.cmn_EmployeeVersion
                select em;

            switch (method)
            {
                case "EPFNo":
                    employees = employees.Where(em => em.EPFNo.Contains(searchString));
                    break;
                case "NIC":
                    employees = employees.Where(em => em.NIC.Contains(searchString));
                    break;
                default:
                    employees = employees.Where(em => em.FullName.Contains(searchString));
                    break;
            }

            var empVersionList = await employees.ToListAsync();

            var empList = empVersionList.Select(employee => new Employee
                {
                    EmployeeId = employee.EmployeeVersionId,
                    EPFNo = employee.EPFNo,
                    Title = int.TryParse(employee.Title, out var t) ? (Title) t : Title.Null,
                    NameWithInitial = employee.NameWithInitial,
                    FullName = employee.FullName,
                    NIC = employee.NIC,
                    WorkSpaceName = employee.cmn_WorkSpace != null ? employee.cmn_WorkSpace.WorkSpaceName : "Null",
                    WorkSpaceType = employee.cmn_WorkSpace != null
                        ? employee.cmn_WorkSpace.cmn_WorkSpaceType.WorkSpaceTypeName
                        : "Null",
                    DesignationName = employee.hrm_Designation != null ? employee.hrm_Designation.DesignationName : "Null",
                    EmployeeRecruitmentType = int.TryParse(employee.EmployeeRecruitmentType, out var rt)
                        ? (RecruitmentType) rt
                        : RecruitmentType.Null,
                    EmpStatus = (EmployeeStatus) employee.EmpStatus,
                    Grade = employee.hrm_Grade != null ? employee.hrm_Grade.GradeName : "Null",
                    DateOfAppointment = employee.DateOfAppointment,
                    DateOfJoint = employee.EffectiveDate,
                    TypeOfContract = employee.TypeOfContract,
                    OfficeEmail = employee.OfficeEmail,
                    MobileNumber = employee.MobileNumber,
                    PrivateEmail = employee.PrivateEmail
                })
                .ToList();

            ViewBag.ProgramId = programId;

            return View(empList);
        }

        internal static async Task<List<Employee>> GetTraineesAsync(int programId)
        {
            // get program assignments
            var programAssignments =
                await default_dbs.ProgramAssignments.Where(p => p.ProgramId == programId).ToListAsync();

            var trainees = new List<Employee>();

            foreach (var item in programAssignments)
            {
                // get trainee data from the cecb database
                trainees.Add(await FindEmployeeAsync(item.EmployeeVersionId));
            }

            return trainees;
        }

        /**
         * Need employee version id and return converted Employee
         */
        internal static async Task<Employee> FindEmployeeAsync(Guid id)
        {
//            var employee = dbs.cmn_EmployeeVersion.First(vid => vid.EmployeeVersionId == id);
            var employee = await dbs.cmn_EmployeeVersion.FindAsync(id);
            
            if (employee == null) return new Employee();
            return new Employee
            {
                EmployeeId = employee.EmployeeVersionId,
                EPFNo = employee.EPFNo,
                Title = int.TryParse(employee.Title, out var t) ? (Title) t : Title.Null,
                NameWithInitial = employee.NameWithInitial,
                FullName = employee.FullName,
                NIC = employee.NIC,
                WorkSpaceName = employee.cmn_WorkSpace != null ? employee.cmn_WorkSpace.WorkSpaceName : "Null",
                WorkSpaceType = employee.cmn_WorkSpace != null
                    ? employee.cmn_WorkSpace.cmn_WorkSpaceType.WorkSpaceTypeName
                    : "Null",
                DesignationName = employee.hrm_Designation != null ? employee.hrm_Designation.DesignationName : "Null",
                EmployeeRecruitmentType = int.TryParse(employee.EmployeeRecruitmentType, out var rt)
                    ? (RecruitmentType) rt
                    : RecruitmentType.Null,
                EmpStatus = (EmployeeStatus) employee.EmpStatus,
                DateOfAppointment = employee.DateOfAppointment,
                Grade = employee.hrm_Grade != null ? employee.hrm_Grade.GradeName : "Null",
                DateOfJoint = employee.EffectiveDate,
                TypeOfContract = employee.TypeOfContract,
                OfficeEmail = employee.OfficeEmail,
                MobileNumber = employee.MobileNumber,
                PrivateEmail = employee.PrivateEmail
            };
        }

        internal static Employee FindEmployee(Guid id)
        {
            var employee = dbs.cmn_EmployeeVersion.Find(id);

            if (employee == null) return new Employee();
            return new Employee
            {
                EmployeeId = employee.EmployeeVersionId,
                EPFNo = employee.EPFNo,
                Title = int.TryParse(employee.Title, out var t) ? (Title)t : Title.Null,
                NameWithInitial = employee.NameWithInitial,
                FullName = employee.FullName,
                NIC = employee.NIC,
                WorkSpaceName = employee.cmn_WorkSpace != null ? employee.cmn_WorkSpace.WorkSpaceName : "Null",
                WorkSpaceType = employee.cmn_WorkSpace != null
                    ? employee.cmn_WorkSpace.cmn_WorkSpaceType.WorkSpaceTypeName
                    : "Null",
                DesignationName = employee.hrm_Designation != null ? employee.hrm_Designation.DesignationName : "Null",
                EmployeeRecruitmentType = int.TryParse(employee.EmployeeRecruitmentType, out var rt)
                    ? (RecruitmentType)rt
                    : RecruitmentType.Null,
                EmpStatus = (EmployeeStatus)employee.EmpStatus,
                DateOfAppointment = employee.DateOfAppointment,
                Grade = employee.hrm_Grade != null ? employee.hrm_Grade.GradeName : "Null",
                DateOfJoint = employee.EffectiveDate,
                TypeOfContract = employee.TypeOfContract,
                OfficeEmail = employee.OfficeEmail,
                MobileNumber = employee.MobileNumber,
                PrivateEmail = employee.PrivateEmail
            };
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