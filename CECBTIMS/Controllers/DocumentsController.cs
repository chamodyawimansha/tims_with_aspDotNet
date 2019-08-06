using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CECBTIMS.Controllers
{
    public class DocumentsController : Controller
    {
        // GET: Document
        public ActionResult Index()
        {
            return Content("Hello");
        }
    }
}