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
        private const string FontFamily = "Times New Roman";
        private const string ParagraphFontSize = "24";
        private const string TitleFontSize = "24";

        // 0 = organizer, 1 = date(14th august 2018), 2 = Venue, 
        private const string FirstParagraph =
            "{0} has announced the Annual Sessions which will be held on {1} at {2}. Please circulate the attached details of the programme among the officers (from permanent cadre) in your Project/Sites.";

        //after first paragraph target group
        // 0 = Application closing Time, 1 = Application closing date(09th August 2018)
        private const string SecondParagraph =
            "Those who are willing to attend should send or email (cecb.training@gmail.com) duly filled Training Request Form together with your recommendation on or before {0} on {1} to reach the Training Unit.";

        // 0 = member fee, 1 = non - member fee, 2 = student fee
        private const string ThirdParagraph =
            "Please note that the registration fee of Rs.{0}/- (per member), Rs.{1}/- (per non-member), Rs.{2}/- (per Student Members) per Participant should be paid by the respective AGM sections.";

        private const string ForthParagraph = "Late requests will not be accepted";

        private readonly string[] RecipientList = new[]
        {
            "Corp. AGM (Con)",
            "Corp.AGM (EPC) /AGM (SP-2)",
            "CEO (CESL)",
            "AGM (Central)",
            "AGM (NC &North)",
            "AGM (P)",
            "AGM (Consultancy-East)",
            "AGM(D1)",
            "AGM (DHQC)",
            "AGM(D2)",
            "AGM (E& M)",
            "AGM(NRM & LS)",
            "AGM(Northern Roads)",
            "AGM(SP - 1)",
            "AGM(WRP)",
            "AGM (D3)",
            "AGM (Architecture)",
            "AGM (EPC - WP1)",
            "AGM (EPC -WP2)",
            "AGM(EPC -NC)",
            "AGM (EPC -Uva)",
            "AGM(EPC - Sabaragamuwa)",
            "AGM (EPC - SP)",
            "AGM (EPC - East)",
            "AGM (EPC - CP)",
            "AGM (EPC - SE)",
            "AGM (EPC-North)",
            "Actg.DGM (WP2)",
            "Actg. DGM (SP)",
            "Actg. DGM (SE)",
            "DGM (C & QS)",
            "DGM (HR & Admin.)",
            "DGM (IT)",
            "DGM (Finance)",
            "Senior Consultant (Business Promotion)",
            "HOD (Transport)",
            "HOD (Legal Unit)",
            "HOD (Supplies Unit)",
            "Progress Monitoring Unit",
            "CDO"
        };

        private Program program;
        private List<Employee> TraineeList = new List<Employee>();

        public LocalCirculate(Program program)
        {
            this.program = program;
        }

        public LocalCirculate(Program program, List<Employee> traineeList)
        {
            this.program = program;
            this.TraineeList = traineeList;
        }

        /**
         * Create a new document
         */
        private string CreateNewDocument(string fileName)
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
        private string AddDefaultStyles(string path)
        {
            if (!File.Exists(path)) return path;

            using (var doc = WordprocessingDocument.Open(path, true))
            {
                var runProp = new RunProperties();

                var defaultFont = new RunFonts {Ascii = FontFamily};
                var defaultFontSize = new FontSize {Val = new StringValue("12")};
                runProp.Append(defaultFont);
                runProp.Append(defaultFontSize);

                var r = doc.MainDocumentPart.Document.Descendants<Run>().First();
                r.PrependChild(runProp);
                doc.MainDocumentPart.Document.Save();
            }

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
                body.Append(SetRecipientParagraph());
                body.Append(SetProgramTitleParagraph());
                body.Append(SetFirstParagraph());
                body.Append(SetTargetGroupParagraph());
                body.Append(SetSecondParagraph());
                body.Append(SetThirdParagraph());
                body.Append(new Paragraph());
                body.Append(SetNameParagraph());
            }
        }

        /**
         * Create header paragraph
         */
        private Paragraph HeaderParagraph()
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
        private Text SetFileNumber()
        {
            return new Text()
            {
                Text = "CB/TRU/LOC/" + DateTime.Today.ToString("yyyy") +
                       " - 00                                                                                        ", //unique number for this year
                Space = SpaceProcessingModeValues.Preserve
            };
        }

        private Text SetCurrentDate()
        {
            return new Text()
            {
                Text = "Date: " + DateTime.Now.ToString("yyyy.MM.dd"),
            };
        }

        private Paragraph SetRecipientParagraph()
        {
            var p = new Paragraph();

            var pp = new ParagraphProperties
            {
                Justification = new Justification() {Val = JustificationValues.Left}
            };
            p.Append(pp);
            var r = new Run();
            // add font family
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

            var rPr = new RunProperties(
                new RunFonts()
                {
                    Ascii = FontFamily,
                });
            // set text to bold
            rPr.Append(new Bold()
            {
                Val = OnOffValue.FromBoolean(true),
            });
            //set font size to 12
            rPr.Append(new FontSize()
            {
                Val = TitleFontSize,
            });

            r.Append(rPr);

            r.Append(new Text()
            {
                Text = this.program.Title,
            });

            p.Append(r);
            return p;
        }

        private Paragraph SetFirstParagraph()
        {
            var p = new Paragraph();
            var pp = new ParagraphProperties
            {
                Justification = new Justification() {Val = JustificationValues.Both}
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
            r.Append(new Text()
            {
                Text = string.Format(FirstParagraph, DocumentHelper.GetOrganisedBy(program),
                    program.StartDate.ToString("D"), program.Venue)
            });

            p.Append(r);

            return p;
        }

        private Paragraph SetTargetGroupParagraph()
        {
            var p = new Paragraph();
            var pp = new ParagraphProperties
            {
                Justification = new Justification() {Val = JustificationValues.Both}
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
            // set text to bold
            rPr.Append(new Bold()
            {
                Val = OnOffValue.FromBoolean(true),
            });
            r.Append(rPr);
            r.Append(new Text()
            {
                Text = string.Format("Target Group: " + DocumentHelper.GetTargetGroup(program))
            });

            p.Append(r);

            return p;
        }

        private Paragraph SetSecondParagraph()
        {
            var p = new Paragraph();
            var pp = new ParagraphProperties
            {
                Justification = new Justification() {Val = JustificationValues.Both}
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
            r.Append(new Text()
            {
                Text = string.Format(SecondParagraph, program.ApplicationClosingTime.ToString("h:mm tt"),
                    program.ApplicationClosingDate.ToString("D"))
            });

            p.Append(r);

            return p;
        }

        private Paragraph SetThirdParagraph()
        {
            var p = new Paragraph();
            var pp = new ParagraphProperties
            {
                Justification = new Justification() {Val = JustificationValues.Both}
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
            r.Append(new Text()
            {
                Text = string.Format(ThirdParagraph, program.MemberFee.ToString(), program.NonMemberFee.ToString(),
                    program.StudentFee.ToString())
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