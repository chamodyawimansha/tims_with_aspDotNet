using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.IO;
using CECBTIMS.DAL;


namespace CECBTIMS.Controllers
{
    public class DocumentsController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Document
        public ActionResult Index()
        {
            return Content("Hello");
        }
        
        public async Task<ActionResult> Generate(Guid? id)
        {

//            Template name
//                program id
//                    employee id















            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var file = await db.Files.FindAsync(id);

            if (file == null) return HttpNotFound();

            var path = Path.Combine(Server.MapPath("~/Storage"), Path.GetFileName(file.FileName) ?? throw new InvalidOperationException());

            if (System.IO.File.Exists(path))
            {

//                return DownloadFile(path, file.FileName);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        }


        public FileResult DownloadDoc(string path, string fileName)
        {
            return File(path, MimeMapping.GetMimeMapping(path), fileName);
        }
    }
}