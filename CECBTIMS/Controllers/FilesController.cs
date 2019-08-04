using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CECBTIMS.DAL;
using CECBTIMS.Models;
using CECBTIMS.Models.Enums;

namespace CECBTIMS.Controllers
{
    public class FilesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Files
        //        public async Task<ActionResult> Index()
        //        {
        //            var files = db.Files.Include(f => f.Program);
        //            return View(await files.ToListAsync());
        //        }


        //        public async Task<ActionResult> Details(Guid? id)
        //        {
        //            if (id == null)
        //            {
        //                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //            }
        //            TimsFile file = await db.Files.FindAsync(id);
        //            if (file == null)
        //            {
        //                return HttpNotFound();
        //            }
        //            return View(file);
        //        }

        public async Task<ActionResult> Download(Guid? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var file = await db.Files.FindAsync(id);

            if (file == null) return HttpNotFound();

            var path = Path.Combine(Server.MapPath("~/Storage"), Path.GetFileName(file.FileName) ?? throw new InvalidOperationException());

            if (System.IO.File.Exists(path))
            {

                return DownloadFile(path, file.FileName);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        }

        public FileResult DownloadFile(string path,string fileName)
        {
            return File(path, MimeMapping.GetMimeMapping(path), fileName);
        }

        public async Task<ActionResult> Upload(int? programId)
        {
            if (programId == null) return View();

            var program = await db.Programs.FindAsync(programId);
            if (program == null) return HttpNotFound();

            ViewBag.ProgramId = programId;
            return View();

        }

        
        [HttpPost, ActionName("Upload")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UploadFile(HttpPostedFileBase file,string title,string details,int? programId)
        {

            if (file == null || file.ContentLength <= 0 ) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            // get the file extension
            var fileExtension = Path.GetExtension(file.FileName.ToUpper())?.Replace(".", string.Empty);


            //check if the file type is supported
            if (!Enum.GetNames(typeof(FileType)).Contains(fileExtension))
            {
                ViewBag.Message = "File type is not supported";
                return View($"Upload"); 
            }

            // generate id
            var id = Guid.NewGuid();
            // generate a new file name
            var newFileName = id + "_" + file.FileName;
            //create new file object
            var newFile = new TimsFile
            {
                Id = id,
                Title = title,
                Details = details,
                FileName = newFileName,
                OriginalFileName = file.FileName,
                FileType = (FileType) Enum.Parse(typeof(FileType), fileExtension ?? throw new InvalidOperationException()),
                FileMethod = (FileMethod) 1, // upload
                ProgramId = programId,
            };
            // file path name
            var path = Path.Combine(Server.MapPath("~/Storage"), Path.GetFileName(newFileName) ?? throw new InvalidOperationException());

            try
            {
                // save file in the storage
                file.SaveAs(path);

                if (System.IO.File.Exists(path))
                {
                    //save the new file object in the database
                    db.Files.Add(newFile);
                    await db.SaveChangesAsync();
                    ViewBag.Message = "File uploaded Successfully";
                }








                //remove file from the storage if cant save on the db









            }
            catch (Exception ex)
            {
                ViewBag.Message = "File upload Failed:" + ex.Message.ToString();
            }

            
            return View($"Upload");
        }


        // GET: Files/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var file = await db.Files.FindAsync(id);
            if (file == null)
            {
                return HttpNotFound();
            }

            ViewBag.ProgramId = file.Program.Id;

            return View(file);
        }

        // POST: Files/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditFile(Guid? Id, HttpPostedFileBase file, int? programId, string title, string details, byte[] rowVersion)
        {
            // get the file object from the database
            var fileInDb = await db.Files.FindAsync(Id);

            if (fileInDb == null) return HttpNotFound();

            var newFileName = "";

            //check if the file is going to change
            if (file != null && !(file.ContentLength <= 0))
            {
                //get the new file ext
                var newFileExtension = Path.GetExtension(file.FileName.ToUpper())?.Replace(".", string.Empty);
                //check the new file ext is right
                if (!Enum.GetNames(typeof(FileType)).Contains(newFileExtension))
                {
                    ViewBag.Message = "File type is not supported";
                    return View($"Edit");
                }

                var currentFilepath = Path.Combine(Server.MapPath("~/Storage"), Path.GetFileName(fileInDb.FileName) ?? throw new InvalidOperationException());

                //remove the file if available in the storage
                if (System.IO.File.Exists(currentFilepath))
                {
                    System.IO.File.Delete(currentFilepath);
                }

                //new name
                newFileName = Id + "_" + file.FileName;

                var newFilePath = Path.Combine(Server.MapPath("~/Storage"), Path.GetFileName(newFileName) ?? throw new InvalidOperationException());
                // store file in the storage
                try
                {
                    file.SaveAs(newFilePath);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "File upload Failed:" + ex.Message.ToString();
                    return View($"Edit");
                }
         


            }

            // update details in the database
            



            // if file upload failed remove the data from the storage and notify to re upload

            return Content(programId.ToString());


        }
//
//        // GET: Files/Delete/5
//        public async Task<ActionResult> Delete(Guid? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            TimsFile file = await db.Files.FindAsync(id);
//            if (file == null)
//            {
//                return HttpNotFound();
//            }
//            return View(file);
//        }
//
//        // POST: Files/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> DeleteConfirmed(Guid id)
//        {
//            TimsFile file = await db.Files.FindAsync(id);
//            db.Files.Remove(file);
//            await db.SaveChangesAsync();
//            return RedirectToAction("Index");
//        }

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
