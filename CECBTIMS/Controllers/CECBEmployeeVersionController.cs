using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using CECBTIMS.Models;

namespace CECBTIMS.Controllers
{   
    public class CECBEmployeeVersionController : Controller
    {
        private CECB_ERPEntities db = new CECB_ERPEntities();
        
        // GET: Employee/Details
        public ActionResult Details()
        {
            return View($"Details");
        }

        /**
         *
         * Find Employees in the CECB ERP Database
         * 
         */
        public async Task<ActionResult> Find(string method, string q)
        {
            //            FullName, NIC, EPFNo

            if (string.IsNullOrWhiteSpace(method) || string.IsNullOrWhiteSpace(q))
            {
                return new System.Web.Mvc.HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var employees = await
                (from emp in db.cmn_EmployeeVersion
                 join wks in db.cmn_WorkSpace on emp.WorkSpaceId equals wks.WorkSpaceId
                 join wkst in db.cmn_WorkSpaceType on wks.WorkSpaceTypeId equals wkst.WorkSpaceTypeId
                 join dsg in db.hrm_Designation on emp.DesignationId equals dsg.DesignationId
                 join dsgc in db.hrm_DesignationCategory on dsg.DesignationCategoryId equals dsgc.DesignationCategoryId
                 where emp.NIC == q && emp.IsActive
                 select new Employee()
                 {
                     EmployeeId = emp.EmployeeId,
                     EPFNo = emp.EPFNo,
                     Title = (Title)int.Parse(emp.Title),
                     NameWithInitial = emp.NameWithInitial,
                     FullName = emp.FullName,
                     NIC = emp.NIC,
                     WorkSpaceName = wks.WorkSpaceName,
                     DesignationName = dsg.DesignationName,
                     EmployeeRecruitmentType = (RecruitmentType)int.Parse(emp.EmployeeRecruitmentType),
                     EmpStatus = (EmployeeStatus)emp.EmpStatus,
                     DateOfAppointment = emp.DateOfAppointment,
                     TypeOfContract = emp.TypeOfContract,
                     OfficeEmail = emp.OfficeEmail,
                     MobileNumber = emp.MobileNumber,
                     PrivateEmail = emp.PrivateEmail

                 }).ToListAsync();


            return View($"Details", employees);


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
