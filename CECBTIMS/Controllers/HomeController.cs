using System.Web.Mvc;

namespace CECBTIMS.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return RedirectToAction("Index", "Programs");
        }

        public ActionResult Contact()
        {
            return RedirectToAction("Index", "Programs");
        }
    }
}





