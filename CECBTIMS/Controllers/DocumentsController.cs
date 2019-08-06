using DocumentFormat.OpenXml.Packaging;
using OpenXmlPowerTools;
using System;
using System.Data;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CECBTIMS.DAL;
using CECBTIMS.Models;
using DocumentFormat.OpenXml;


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

            //// filepath is a string which contains the path where the new document has to be created
            var sourceFile = Server.MapPath("~/Storage/a0a8e2fb-bb6c-4ad9-8d6b-4102a01e9506_Approvel letter.docx");

            var destinationFile = Server.MapPath("~/Storage/Approvel_letter_edited.docx");

            System.IO.File.Copy(sourceFile, destinationFile, true);

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(destinationFile, true))
            {
                SimplifyMarkupSettings settings = new SimplifyMarkupSettings
                {
                    NormalizeXml = true, // Merges Run's in a paragraph with similar formatting

                };
                MarkupSimplifier.SimplifyMarkup(doc, settings);

                string docText = null;
                using (var sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }

                var regexText = new Regex("{year}");
                docText = regexText.Replace(docText, Helpers.GetYear());

                using (var sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }
            }

            //            using (var doc = WordprocessingDocument.Open(destinationFile, true))
            //            {
            //                var body = doc.MainDocumentPart.Document.Body;
            //                var paras = body.Elements<Paragraph>();
            //
            //                foreach (var para in paras)
            //                {
            //                    foreach (var run in para.Elements<Run>())
            //                    {
            //                        foreach (var text in run.Elements<Text>())
            //                        {
            //                            if (text.Text.Contains("${year}"))
            //                            {
            //                                text.Text = text.Text.Replace("${year}", Helpers.GetYear());
            //                            }
            //
            ////                            if (text.Text.Contains("${today}"))
            ////                            {
            ////                                text.Text = text.Text.Replace("${today}", Helpers.GetToday());
            ////                            }
            ////
            ////                            if (text.Text.Contains("${program_title}"))
            ////                            {
            ////                                text.Text = text.Text.Replace("${program_title}", Helpers.GetToday());
            ////                            }
            //
            //                        }
            //                    }
            //                }
            //            }


            return DownloadDoc(destinationFile, "Approvel_letter_edited.docx");



                //            var dt = new DataTable();
                //            dt.Columns.Add("ID");
                //            dt.Columns.Add("Name");
                //            dt.Columns.Add("Sex");
                //            dt.Rows.Add(1, "Tom", "male");
                //            dt.Rows.Add(2, "Jim", "male");
                //            dt.Rows.Add(3, "LiSa", "female");
                //            dt.Rows.Add(4, "LiLi", "female");
                //
                //
                //            var path = Server.MapPath("~/Storage/a0a8e2fb-bb6c-4ad9-8d6b-4102a01e9506_Approvel letter.docx");
                //            var resultPath = Server.MapPath("~/Storage/OpenXmlExample.docx");
                //
                //
                //
                //            var document = WordprocessingDocument.Open(path, true);
                //
                //            var mainPart = document.MainDocumentPart;
                //
                //            var res = from mark in mainPart.Document.Body.Descendants<BookmarkStart>()
                //                where mark.Name == "DataTableBookMark"
                //                      select mark;
                //
                //
                //            var bookmark = res.SingleOrDefault();
                //
                //            if (bookmark != null)
                //            {
                //                var parent = bookmark.Parent;
                //
                //                DocumentFormat.OpenXml.Wordprocessing.Table table = new DocumentFormat.OpenXml.Wordprocessing.Table();
                //                TableProperties tblProp = new DocumentFormat.OpenXml.Wordprocessing.TableProperties(
                //                    new TableBorders(
                //                        new Border()
                //                        {
                //                            Val = new DocumentFormat.OpenXml.EnumValue<BorderValues>(BorderValues.DotDash),
                //                            Size = 24
                //                        }
                //                    )
                //                );
                //
                //                table.AppendChild<TableProperties>(tblProp);
                //                for (int i = 0; i < dt.Rows.Count; i++)
                //                {
                //                    DocumentFormat.OpenXml.Wordprocessing.TableRow tr = new DocumentFormat.OpenXml.Wordprocessing.TableRow();
                //                    for (int j = 0; j < dt.Columns.Count; j++)
                //                    {
                //
                //                        DocumentFormat.OpenXml.Wordprocessing.TableCell tc = new DocumentFormat.OpenXml.Wordprocessing.TableCell();
                //                        tc.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "240" }));
                //                        tc.Append(new Paragraph(new Run(new Text(dt.Rows[i][j].ToString()))));
                //                        tr.Append(tc);
                //                    }
                //                    table.Append(tr);
                //                }
                //
                //                parent.InsertAfterSelf(table);
                //
                //            }
                //
                //
                //
                //

            return Content("Hello");




//            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//
//            c
//
//            if (file == null) return HttpNotFound();
//
//            var path = Path.Combine(Server.MapPath("~/Storage"), Path.GetFileName(file.FileName) ?? throw new InvalidOperationException());
//
//            if (System.IO.File.Exists(path))
//            {
//
////                return DownloadFile(path, file.FileName);
//            }
//
//            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        }


        public FileResult DownloadDoc(string path, string fileName)
        {
            return File(path, MimeMapping.GetMimeMapping(path), fileName);
        }
    }
}