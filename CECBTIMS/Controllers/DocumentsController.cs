using System.Threading.Tasks;
using System.Web.Mvc;
using CECBTIMS.DAL;
using CECBTIMS.Models.Document;


namespace CECBTIMS.Controllers
{
    public class DocumentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // variable names in the document will be like this get_today, get_program_title, get_program_end_date, get_program_start_date


        /*
         * Show list of documents each program have
         */

        /**
         * show document generation form
         */

        /**
         *
         */


        /**
         * static document gen
         */

        public async Task<ActionResult> Generate()
        {
//            var lc = new LocalApprovalLetter(await db.Programs.FindAsync(1));
            var program = await db.Programs.FindAsync(1);

            var tc = new EmployeesController();

            if (program == null) return Content("Hello");


            var lc = new LocalApprovalLetter(program, tc.GetTrainees(program.Id));
            lc.Create();

            return Content("Hello");
        }

    }
}