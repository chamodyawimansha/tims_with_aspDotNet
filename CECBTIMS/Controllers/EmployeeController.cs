using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using CECBTIMS.DAL;
using CECBTIMS.Models;

namespace CECBTIMS.Controllers
{
    public class EmployeeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private CECB_ERPEntities cecb_db = new CECB_ERPEntities();

        // GET: Employee/Details
        public async Task<ActionResult> Details(string method, string q)
        {
            if (string.IsNullOrEmpty(method) || string.IsNullOrEmpty(q))
            {
                return View($"Details");
            }

            var employees = from em in db.Employees
                            select em;

            switch (method)
            {
                case "EPFNo":
                    employees = employees.Where(em => em.Cecb.EPFNo.Contains(q));
                    break;
                case "NIC":
                    employees = employees.Where(em => em.Cecb.NIC.Contains(q));
                    break;
                default:
                    employees = employees.Where(em => em.Cecb.FullName.Contains(q));
                    break;
            }

            return View($"Details", await employees.ToListAsync());

        }

        /**
         * Get More Details from the cecb_db
         */
        public async Task<ActionResult> MoreDetails(Guid? id)
        {
            var query = from emp in cecb_db.cmn_EmployeeVersion
                        join wks in cecb_db.cmn_WorkSpace on emp.WorkSpaceId equals wks.WorkSpaceId
                        join wkst in cecb_db.cmn_WorkSpaceType on wks.WorkSpaceTypeId equals wkst.WorkSpaceTypeId
                        join dsg in cecb_db.hrm_Designation on emp.DesignationId equals dsg.DesignationId
                        join dsgc in cecb_db.hrm_DesignationCategory on dsg.DesignationGroupId equals dsgc.DesignationCategoryId
                        where emp.EmployeeId == id && emp.IsActive
                        select new
                        {
                            employee = emp,
                            workSpace = wks,
                            workSpaceType = wkst,
                            designation = dsg,
                            designationCategory = dsgc
                        };

            var data = await query.FirstAsync();

            var employee = new Employee()
            {
                EmployeeId = data.employee.EmployeeId,
                EPFNo = data.employee.EPFNo,
                Title = (Title)int.Parse(data.employee.Title),
                NameWithInitial = data.employee.NameWithInitial,
                FullName = data.employee.FullName,
                NIC = data.employee.NIC,
                WorkSpaceName = data.workSpace.WorkSpaceName,
                DesignationName = data.designation.DesignationName,
                EmployeeRecruitmentType = (RecruitmentType)int.Parse(data.employee.EmployeeRecruitmentType),
                EmpStatus = (EmployeeStatus)data.employee.EmpStatus,
                DateOfAppointment = data.employee.DateOfAppointment,
                TypeOfContract = data.employee.TypeOfContract,
                OfficeEmail = data.employee.OfficeEmail,
                MobileNumber = data.employee.MobileNumber,
                PrivateEmail = data.employee.PrivateEmail
            };

            return View($"Details", employee);
        }

        //            https://www.guru99.com/c-sharp-serialization.html


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                cecb_db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}