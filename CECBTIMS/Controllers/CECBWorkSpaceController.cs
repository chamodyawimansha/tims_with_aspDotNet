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
    public class CECBWorkSpaceController : Controller
    {
        private CECB_ERPEntities db = new CECB_ERPEntities();

        // GET: CECBWorkSpace
        public async Task<ActionResult> Index()
        {
            var cmn_WorkSpace = db.cmn_WorkSpace.Include(c => c.cmn_WorkSpace2);
            return View(await cmn_WorkSpace.ToListAsync());
        }

        // GET: CECBWorkSpace/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new System.Web.Mvc.HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cmn_WorkSpace cmn_WorkSpace = await db.cmn_WorkSpace.FindAsync(id);
            if (cmn_WorkSpace == null)
            {
                return HttpNotFound();
            }
            return View(cmn_WorkSpace);
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
