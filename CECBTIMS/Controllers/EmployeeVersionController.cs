using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CECBTIMS.Models;

namespace CECBTIMS.Controllers
{
    public class EmployeeVersionController : Controller
    {
        private CECB_ERPEntities db = new CECB_ERPEntities();

        // GET: EmployeeVersion
        public async Task<ActionResult> Index()
        {
            var cmn_EmployeeVersion = db.cmn_EmployeeVersion.Include(c => c.cmn_Employee).Include(c => c.hrm_Designation).Include(c => c.cmn_WorkSpace).Include(c => c.hrm_DesignationCategory).Include(c => c.hrm_Grade);
            return View(await cmn_EmployeeVersion.ToListAsync());
        }

        // GET: EmployeeVersion/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cmn_EmployeeVersion cmn_EmployeeVersion = await db.cmn_EmployeeVersion.FindAsync(id);
            if (cmn_EmployeeVersion == null)
            {
                return HttpNotFound();
            }
            return View(cmn_EmployeeVersion);
        }

        // GET: EmployeeVersion/Create
        public ActionResult Create()
        {
            ViewBag.EmployeeId = new SelectList(db.cmn_Employee, "EmployeeId", "EmployeeCode");
            ViewBag.DesignationId = new SelectList(db.hrm_Designation, "DesignationId", "DesignationName");
            ViewBag.WorkSpaceId = new SelectList(db.cmn_WorkSpace, "WorkSpaceId", "WorkSpaceCode");
            ViewBag.DesignationCategoryId = new SelectList(db.hrm_DesignationCategory, "DesignationCategoryId", "DesignationCategoryName");
            ViewBag.GradeId = new SelectList(db.hrm_Grade, "GradeId", "GradeName");
            return View();
        }

        // POST: EmployeeVersion/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EmployeeVersionId,EmployeeId,EmployeeCode,EPFNo,Title,NameWithInitial,FullName,NIC,Gender,WorkSpaceId,DesignationId,Religion,BloodGroup,DateOfBirth,BasicSalary,CivilStatus,HomeAddress,ContactAddress,LandphoneNumber,MobileNumber,PrivateEmail,NextOfKin,NextOfKinRelationShip,EmployeeRecruitmentType,DateOfAppointment,TypeOfContract,DataStatus,IsActive,BusinessUnitId,OrganizationId,CreatedDateTime,CreatedUserId,UpdatedDateTime,UpdatedUserId,OfficeEmail,PersonalFileNo,InitialBasicSalary,GradeId,AGMWorkSpaceId,DGMWorkSpaceId,OfficeEmail2,EmergencyContactNumber,EmergencyContactNumber2,EmergencyContactAddress,DateOfRetainment,Race,EmpStatus,Initial,Name,ImageUrl,DateOfExpiry,DesignationCategoryId,IncrementDate,SubSectionId,EffectiveDate,ResignedDate,Version,PhysicalLocationId,SNameWithInitial,SHomeAddress")] cmn_EmployeeVersion cmn_EmployeeVersion)
        {
            if (ModelState.IsValid)
            {
                cmn_EmployeeVersion.EmployeeVersionId = Guid.NewGuid();
                db.cmn_EmployeeVersion.Add(cmn_EmployeeVersion);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeId = new SelectList(db.cmn_Employee, "EmployeeId", "EmployeeCode", cmn_EmployeeVersion.EmployeeId);
            ViewBag.DesignationId = new SelectList(db.hrm_Designation, "DesignationId", "DesignationName", cmn_EmployeeVersion.DesignationId);
            ViewBag.WorkSpaceId = new SelectList(db.cmn_WorkSpace, "WorkSpaceId", "WorkSpaceCode", cmn_EmployeeVersion.WorkSpaceId);
            ViewBag.DesignationCategoryId = new SelectList(db.hrm_DesignationCategory, "DesignationCategoryId", "DesignationCategoryName", cmn_EmployeeVersion.DesignationCategoryId);
            ViewBag.GradeId = new SelectList(db.hrm_Grade, "GradeId", "GradeName", cmn_EmployeeVersion.GradeId);
            return View(cmn_EmployeeVersion);
        }

        // GET: EmployeeVersion/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cmn_EmployeeVersion cmn_EmployeeVersion = await db.cmn_EmployeeVersion.FindAsync(id);
            if (cmn_EmployeeVersion == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeId = new SelectList(db.cmn_Employee, "EmployeeId", "EmployeeCode", cmn_EmployeeVersion.EmployeeId);
            ViewBag.DesignationId = new SelectList(db.hrm_Designation, "DesignationId", "DesignationName", cmn_EmployeeVersion.DesignationId);
            ViewBag.WorkSpaceId = new SelectList(db.cmn_WorkSpace, "WorkSpaceId", "WorkSpaceCode", cmn_EmployeeVersion.WorkSpaceId);
            ViewBag.DesignationCategoryId = new SelectList(db.hrm_DesignationCategory, "DesignationCategoryId", "DesignationCategoryName", cmn_EmployeeVersion.DesignationCategoryId);
            ViewBag.GradeId = new SelectList(db.hrm_Grade, "GradeId", "GradeName", cmn_EmployeeVersion.GradeId);
            return View(cmn_EmployeeVersion);
        }

        // POST: EmployeeVersion/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EmployeeVersionId,EmployeeId,EmployeeCode,EPFNo,Title,NameWithInitial,FullName,NIC,Gender,WorkSpaceId,DesignationId,Religion,BloodGroup,DateOfBirth,BasicSalary,CivilStatus,HomeAddress,ContactAddress,LandphoneNumber,MobileNumber,PrivateEmail,NextOfKin,NextOfKinRelationShip,EmployeeRecruitmentType,DateOfAppointment,TypeOfContract,DataStatus,IsActive,BusinessUnitId,OrganizationId,CreatedDateTime,CreatedUserId,UpdatedDateTime,UpdatedUserId,OfficeEmail,PersonalFileNo,InitialBasicSalary,GradeId,AGMWorkSpaceId,DGMWorkSpaceId,OfficeEmail2,EmergencyContactNumber,EmergencyContactNumber2,EmergencyContactAddress,DateOfRetainment,Race,EmpStatus,Initial,Name,ImageUrl,DateOfExpiry,DesignationCategoryId,IncrementDate,SubSectionId,EffectiveDate,ResignedDate,Version,PhysicalLocationId,SNameWithInitial,SHomeAddress")] cmn_EmployeeVersion cmn_EmployeeVersion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cmn_EmployeeVersion).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeId = new SelectList(db.cmn_Employee, "EmployeeId", "EmployeeCode", cmn_EmployeeVersion.EmployeeId);
            ViewBag.DesignationId = new SelectList(db.hrm_Designation, "DesignationId", "DesignationName", cmn_EmployeeVersion.DesignationId);
            ViewBag.WorkSpaceId = new SelectList(db.cmn_WorkSpace, "WorkSpaceId", "WorkSpaceCode", cmn_EmployeeVersion.WorkSpaceId);
            ViewBag.DesignationCategoryId = new SelectList(db.hrm_DesignationCategory, "DesignationCategoryId", "DesignationCategoryName", cmn_EmployeeVersion.DesignationCategoryId);
            ViewBag.GradeId = new SelectList(db.hrm_Grade, "GradeId", "GradeName", cmn_EmployeeVersion.GradeId);
            return View(cmn_EmployeeVersion);
        }

        // GET: EmployeeVersion/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cmn_EmployeeVersion cmn_EmployeeVersion = await db.cmn_EmployeeVersion.FindAsync(id);
            if (cmn_EmployeeVersion == null)
            {
                return HttpNotFound();
            }
            return View(cmn_EmployeeVersion);
        }

        // POST: EmployeeVersion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            cmn_EmployeeVersion cmn_EmployeeVersion = await db.cmn_EmployeeVersion.FindAsync(id);
            db.cmn_EmployeeVersion.Remove(cmn_EmployeeVersion);
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
