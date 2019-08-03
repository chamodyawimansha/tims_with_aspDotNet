﻿using System;
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

            if (file == null || file.ContentLength <= 0) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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
//        public async Task<ActionResult> Edit(Guid? id)
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
//            ViewBag.ProgramId = new SelectList(db.Programs, "Id", "Title", file.ProgramId);
//            return View(file);
//        }

        // POST: Files/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Details,FileName,OriginalFileName,FileType,FileMethod,ProgramId,CreatedAt,UpdatedAt,CreatedBy,UpdatedBy,RowVersion")] TimsFile file)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(file).State = EntityState.Modified;
//                await db.SaveChangesAsync();
//                return RedirectToAction("Index");
//            }
//            ViewBag.ProgramId = new SelectList(db.Programs, "Id", "Title", file.ProgramId);
//            return View(file);
//        }
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
