using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace CECBTIMS.Models.Document
{
    public class LocalLongApprovalLetterPOne
    {
        private const string FontFamily = "Times New Roman";
        private const string ParagraphFontSize = "24";
        private const string TitleFontSize = "24";

        private Program _program;
        private Employee _trainee;

        public LocalLongApprovalLetterPOne(Program program, Employee trainee)
        {
            this._program = program;
            this._trainee = trainee;
        }

        private readonly string[] RecipientList =
        {
            "Chairman",
            "General Manager",
            "Corp. AGM (Consultancy)",
            "Corp. AGM (EPC)"
        };

        // 0 = Organizer , 1 = start date (May 2019)
        private readonly string _firstParagraph =
            "{0} has announced the above programme which will be conducted the consecutive Saturdays or Sundays commencing from {1}.";

        //0 = Name with title, 1 = designation, 
        private readonly string _secondParagraph =
            "The {0} - {1} has requested to follow the same with the recommendation of respective DGM.";

        private readonly string _thirdParagraph = "The details of the programme are as follows.";
        private readonly string _fourthParagraph = "(special note here)";

        /**
 * Create a new document
 */
        private static string CreateNewDocument(string fileName)
        {
            //create new file path
            var path = HttpContext.Current.Server.MapPath("~/Storage/gen/" + fileName + ".docx");
            //create new document
            using (var doc = WordprocessingDocument.Create(path, WordprocessingDocumentType.Document))
            {
                // Add a main document part. 
                doc.AddMainDocumentPart();
            }

            // add default styles and return the file path
            return path;
        }

        public void Create()
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


                body.Append(HeaderParagraph());
                body.Append(new Paragraph());
                body.Append(SetRecipientParagraph());
                body.Append(SetProgramTitleParagraph());
                body.Append(SetFirstParagraph());
                body.Append(SetSecondParagraph());
                body.Append(SetThirdParagraph());
                body.Append(SetProgramDetailsList());
                body.Append(new Paragraph());
                body.Append(SetFourthParagraph());
                body.Append(new Paragraph());
                body.Append(new Paragraph());
                body.Append(SetNameParagraph());
            }
        }

        /**
         * Create header paragraph
         */
        private static Paragraph HeaderParagraph()
        {
            var p = new Paragraph();

            var pp = new ParagraphProperties
            {
                Justification = new Justification() {Val = JustificationValues.Left}
            };

            // Add paragraph properties to your paragraph
            p.Append(pp);

            // Run
            var r = new Run();

            var rPr = new RunProperties(
                new RunFonts()
                {
                    Ascii = FontFamily
                });
            //set font size to 12
            rPr.Append(new FontSize()
            {
                Val = ParagraphFontSize,
            });


            r.Append(rPr);

            r.Append(SetFileNumber());
            r.Append(SetCurrentDate());

            p.Append(r);
            return p;
        }

        /**
         * File number
         */
        private static Text SetFileNumber()
        {
            return new Text()
            {
                Text = "CB/TRU/LOC/" + DateTime.Today.ToString("yyyy") +
                       " - 00                                                                                        ", //unique number for this year
                Space = SpaceProcessingModeValues.Preserve
            };
        }

        private static Text SetCurrentDate()
        {
            return new Text()
            {
                Text = "Date: " + DateTime.Now.ToString("yyyy.MM.dd"),
            };
        }

        private Paragraph SetRecipientParagraph()
        {
            var p = new Paragraph();

            var pp = new ParagraphProperties {Justification = new Justification() {Val = JustificationValues.Left}};
            p.Append(pp);
            var r = new Run();
            // add font family
            var rPr = new RunProperties(new RunFonts() {Ascii = FontFamily});
            //set font size to 12
            rPr.Append(new FontSize() {Val = ParagraphFontSize});


            r.Append(rPr);

            foreach (var item in RecipientList)
            {
                r.AppendChild(new Text(item));
                r.AppendChild(new Break());
            }

            p.Append(r);
            return p;
        }

        private Paragraph SetProgramTitleParagraph()
        {
            var p = new Paragraph();

            var pp = new ParagraphProperties
            {
                Justification = new Justification() {Val = JustificationValues.Both}
            };
            p.Append(pp);

            var r = new Run();

            var rPr = new RunProperties(new RunFonts() {Ascii = FontFamily});
            // text bold
            rPr.Append(new Bold() {Val = OnOffValue.FromBoolean(true)});
            // text underline
            rPr.Append(new Underline() {Val = UnderlineValues.Single});
            // font size
            rPr.Append(new FontSize() {Val = TitleFontSize});

            r.Append(rPr);

            r.Append(new Text()
            {
                Text = this._program.Title,
            });

            p.Append(r);
            return p;
        }

        private Paragraph SetFirstParagraph()
        {
            var p = new Paragraph();
            var pp = new ParagraphProperties {Justification = new Justification() {Val = JustificationValues.Both}};
            p.Append(pp);

            var r = new Run();
            var rPr = new RunProperties(
                new RunFonts()
                {
                    Ascii = FontFamily,
                });
            //set font size to 12
            rPr.Append(new FontSize()
            {
                Val = ParagraphFontSize,
            });
            r.Append(rPr);
            r.Append(new Text()
            {
                Text = string.Format(_firstParagraph, DocumentHelper.GetOrganisedBy(_program),
                    _program.StartDate.ToString("Y"))
            });

            p.Append(r);

            return p;
        }

        private Paragraph SetSecondParagraph()
        {
            var p = new Paragraph();
            var pp = new ParagraphProperties {Justification = new Justification() {Val = JustificationValues.Both}};
            p.Append(pp);

            var r = new Run();
            var rPr = new RunProperties(
                new RunFonts()
                {
                    Ascii = FontFamily,
                });
            //set font size to 12
            rPr.Append(new FontSize()
            {
                Val = ParagraphFontSize,
            });
            r.Append(rPr);
            r.Append(new Text()
            {
                Text = string.Format(_secondParagraph, DocumentHelper.GetName(_trainee),
                    DocumentHelper.GetDesignation(_trainee))
            });

            p.Append(r);

            return p;
        }

        private Paragraph SetThirdParagraph()
        {
            var p = new Paragraph();
            var pp = new ParagraphProperties {Justification = new Justification() {Val = JustificationValues.Both}};
            p.Append(pp);

            var r = new Run();
            var rPr = new RunProperties(
                new RunFonts()
                {
                    Ascii = FontFamily,
                });
            //set font size to 12
            rPr.Append(new FontSize()
            {
                Val = ParagraphFontSize,
            });
            r.Append(rPr);
            r.Append(new Text()
            {
                Text = _thirdParagraph
            });

            p.Append(r);

            return p;
        }

        private Paragraph SetProgramDetailsList()
        {
            var p = new Paragraph();
            var pp = new ParagraphProperties {Justification = new Justification() {Val = JustificationValues.Left}};
            p.Append(pp);

            var r = new Run();
            var rPr = new RunProperties(
                new RunFonts()
                {
                    Ascii = FontFamily,
                });
            //set font size to 12
            rPr.Append(new FontSize()
            {
                Val = ParagraphFontSize,
            });
            r.Append(rPr);
            r.Append(new Text()
            {
                Text = "          Duration of the Course      - " + DocumentHelper.GetDurationOfTheCourse(_program) +
                       " (on consecutive Saturdays or Sundays)",
                Space = SpaceProcessingModeValues.Preserve
            });
            r.Append(new Break());
            r.Append(new Text()
            {
                Text = "          Course Fee                         - Rs. " + DocumentHelper.GetCourseFee(_program) +
                       "/-",
                Space = SpaceProcessingModeValues.Preserve
            });

            p.Append(r);

            return p;
        }

        private Paragraph SetFourthParagraph()
        {
            var p = new Paragraph();
            var pp = new ParagraphProperties {Justification = new Justification() {Val = JustificationValues.Both}};
            p.Append(pp);

            var r = new Run();
            var rPr = new RunProperties(
                new RunFonts()
                {
                    Ascii = FontFamily,
                });
            //set font size to 12
            rPr.Append(new FontSize()
            {
                Val = ParagraphFontSize,
            });
            r.Append(rPr);
            r.Append(new Text()
            {
                Text = _fourthParagraph
            });

            p.Append(r);

            return p;
        }

        private Paragraph SetNameParagraph()
        {
            var p = new Paragraph();
            var pp = new ParagraphProperties
            {
                Justification = new Justification() {Val = JustificationValues.Left}
            };
            p.Append(pp);

            var r = new Run();
            var rPr = new RunProperties(
                new RunFonts()
                {
                    Ascii = FontFamily,
                });
            //set font size to 12
            rPr.Append(new FontSize()
            {
                Val = ParagraphFontSize,
            });
            r.Append(rPr);
            r.AppendChild(new Text()
            {
                Text = "........................................."
            });
            r.AppendChild(new Break());
            r.AppendChild(new Text()
            {
                Text = DocumentHelper.GetTraineeManagerName()
            });

            r.AppendChild(new Break());

            r.AppendChild(new Text()
            {
                Text = "Training Manager"
            });

            p.Append(r);

            return p;
        }
    }
}