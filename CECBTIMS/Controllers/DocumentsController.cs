using System.Threading.Tasks;
using System.Web.Mvc;
using CECBTIMS.Models.Document;


namespace CECBTIMS.Controllers
{
    public class DocumentsController : Controller
    {

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
            LocalCirculate.Create();

            return Content("Hello");
        }

    }
}