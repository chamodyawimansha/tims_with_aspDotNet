using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace CECBTIMS.Models.Document
{
    /**
     * The class for Local Program circulation document
     */
    public class LocalCirculate
    {
        /**
         * Create a new document
         */
        private static string CreateNewDocument(string fileName)
        {
            //create new file path
            var path = HttpContext.Current.Server.MapPath("~/Storage/gen" + fileName + ".docx");
            //create new document
            using (var doc = WordprocessingDocument.Create(path, WordprocessingDocumentType.Document))
            {
                // Add a main document part. 
                doc.AddMainDocumentPart();
            }

            // add default styles and return the file path
            return path;
        }

        /**
         * Add default styles to the new empty document
         */
        private static string AddDefaultStyles(string path)
        {
            if (!File.Exists(path)) return path;

            using (var doc = WordprocessingDocument.Open(path, true))
            {

                var runProp = new RunProperties();

                var defaultFont = new RunFonts {Ascii = "Arial"};
                var defaultFontSize = new FontSize {Val = new StringValue("11")};
                runProp.Append(defaultFont);
                runProp.Append(defaultFontSize);

                var r = doc.MainDocumentPart.Document.Descendants<Run>().First();
                r.PrependChild(runProp);
                doc.MainDocumentPart.Document.Save();
            }

            return path;

        }

        public static void Create()
        {
            //create a new empty word document
            var path = CreateNewDocument(Guid.NewGuid().ToString());

            // open the document to add content
            using (var doc = WordprocessingDocument.Open(path, true))
            {
                // Add a main document part. 
                var mainPart = doc.MainDocumentPart;

                // Create the document structure and add some text.
                mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document();


                var body = mainPart.Document.AppendChild(new Body());


                HeaderParagraph(body);


            }
        }


        private static void HeaderParagraph(OpenXmlElement body)
        {
            var para = body.AppendChild(new Paragraph());
            var run = para.AppendChild(new Run());
            
            run.AppendChild(SetFileNumber());
            run.AppendChild(SetCurrentDate());
        }

        /**
         * File number and current date
         */

        private static Text SetFileNumber()
        {
            return new Text()
            {
                Text = "This is the File Number",
            };
        }

        private static Text SetCurrentDate()
        {
            return new Text()
            {
                Text = "Date: "+DateTime.Now.ToString("yyyy.MM.dd"),
            };
        }
    }
}