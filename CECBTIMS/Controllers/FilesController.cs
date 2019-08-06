using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
        public async Task<ActionResult> Index()
        {
            var files = db.Files.Include(f => f.Program);
            return View(await files.ToListAsync());
        }


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

        public async Task<ActionResult> Upload(int? programId, string details, string message)
        {

            if (string.IsNullOrWhiteSpace(details)) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (programId != null)
            {
                var program = await db.Programs.FindAsync(programId);
                if (program == null) return HttpNotFound();
            }

            ViewBag.Message = message;
            ViewBag.Details = details;
            ViewBag.ProgramId = programId;

            return View();

        }


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
            
            if (details.Equals("DocumentTemplate"))
            {
          
                if (!fileExtension.Equals("DOCX"))
                {
                    return RedirectToAction($"Upload", new
                    {
                        details = "DocumentTemplate",
                        message = "File type is not supported as a Document Template. Only DOCX are supported"
                });
                }
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
            
            return View(file);
        }

        // POST: Files/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditFile(Guid id, HttpPostedFileBase file, string title, byte[] rowVersion)
        {

            // get the to be update file object from the database
            var fileInDb = await db.Files.FindAsync(id);

            // check the file is not empty
            if (fileInDb == null)
            {
                var deletedFile = new TimsFile();
                TryUpdateModel(deletedFile);
                ModelState.AddModelError(string.Empty, "Unable to save changes. The File was deleted by another user.");

                return View(deletedFile);
            }

            var newFileName = "";
            var newOriginalFileName = "";
            var newFileExtension = "";
            
            //check if the file is going to change
            if (file != null)
            {
                //get the new file ext
                newFileExtension = Path.GetExtension(file.FileName.ToUpper())?.Replace(".", string.Empty);
                //check the new file ext is right
                if (!Enum.GetNames(typeof(FileType)).Contains(newFileExtension))
                {
                    ViewBag.Message = "File type is not supported";
                    return View($"Edit");
                }

                var currentFilepath = Path.Combine(Server.MapPath("~/Storage"),
                    Path.GetFileName(fileInDb.FileName) ?? throw new InvalidOperationException());

                //remove the file if available in the storage
                if (System.IO.File.Exists(currentFilepath))
                {
                    System.IO.File.Delete(currentFilepath);
                }

                //new name
                newOriginalFileName = file.FileName;
                newFileName = id + "_" + file.FileName;

                var newFilePath = Path.Combine(Server.MapPath("~/Storage"),
                    Path.GetFileName(newFileName) ?? throw new InvalidOperationException());
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
            fileInDb.Title = title;
            fileInDb.FileName = newFileName == "" ? fileInDb.FileName : newFileName;
            fileInDb.OriginalFileName = newOriginalFileName == "" ? fileInDb.OriginalFileName : newOriginalFileName;
            fileInDb.FileType = newFileExtension == ""
                ? fileInDb.FileType
                : (FileType)Enum.Parse(typeof(FileType), newFileExtension ?? throw new InvalidOperationException()); ;
            fileInDb.UpdatedAt = DateTime.Now;
            fileInDb.RowVersion = rowVersion;

            if (TryUpdateModel(fileInDb))
            {
                try
                {
                    db.Entry(fileInDb).OriginalValues["RowVersion"] = rowVersion;
                    await db.SaveChangesAsync();

                    ViewBag.Message = "File Updated";
                    return View(fileInDb);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var clientValues = (TimsFile) entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();

                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty,
                            "Unable to save changes. The File was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (TimsFile) databaseEntry.ToObject();
                        
                        if (databaseValues.Title != clientValues.Title)
                        {
                            ModelState.AddModelError("Title", "Current value: " + databaseValues.Title);
                        }

                        if (databaseValues.OriginalFileName != clientValues.OriginalFileName)
                        {
                            ModelState.AddModelError("OriginalFileName", "Current value: " + databaseValues.OriginalFileName);
                        }
                        

                        ModelState.AddModelError(string.Empty,
                            "The File you attempted to edit was modified by another user after you got the original values. The edit operation was canceled and the current values in the database have been displayed. If you still want to edit this record, click the Save button again. Otherwise click the Back Link.");

                        fileInDb.RowVersion = databaseValues.RowVersion;
                    }
                }
                catch (RetryLimitExceededException /*error*/)
                {
                    //ModelState.AddModelError(error.Message);
                    ModelState.AddModelError("",
                        "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }





                // if file upload failed remove the data from the storage and notify to re upload
                






            }

            return View(fileInDb);
        }

        // GET: Files/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
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
            return View(file);
        }

         // POST: Files/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            var file = await db.Files.FindAsync(id);
            if(file == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //remove file from the storage 
            var filepath = Path.Combine(Server.MapPath("~/Storage"),
                Path.GetFileName(file.FileName) ?? throw new InvalidOperationException());

            if (System.IO.File.Exists(filepath))
            {
                System.IO.File.Delete(filepath);
            }
            // make sure the file delete from the storage
            if (System.IO.File.Exists(filepath))
            {
                ViewBag.Message = "File couldn't Delete from the Storage. ";
                return View($"Delete");
            }

            // remove file object from the database
            db.Files.Remove(file);
            await db.SaveChangesAsync();

            ViewBag.Message = "File deleted";
            return View($"Delete", new TimsFile());
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
