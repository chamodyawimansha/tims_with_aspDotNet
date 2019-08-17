using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace CECBTIMS.Models.Document
{
    public class LocalApprovalLetter
    {
        private Program program;
        private List<Employee> TraineeList = new List<Employee>();
        private readonly bool _hasConfigurableTraineeTable = true;

        private readonly string[] RecipientList =
        {
            "General Manager"
        };

        // 0 = organised_by, 1 = start_date, 2 = venue
        private const string FirstParagraph = "{0} has announced the Annual Sessions which will be held on {1} at {2}.";

        private const string SecondParagraph =
            "The following officers have applied to attend with the recommendation of respective AGMs.";

        private const string FontFamily = "Times New Roman";
        private const string ParagraphFontSize = "24";
        private const string TitleFontSize = "24";

        public LocalApprovalLetter(Program program)
        {
            this.program = program;
        }

        public LocalApprovalLetter(Program program, List<Employee> traineeList)
        {
            this.program = program;
            this.TraineeList = traineeList;
        }

        public bool GetHasTraineeTable()
        {
            return _hasConfigurableTraineeTable;
        }

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
                body.Append(new Paragraph());
                body.Append(SetSecondParagraph());
                body.Append(TraineeInformationTable(new[] {"Hello","second Col", "Third Col"}));
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
                Text = this.program.Title,
            });

            p.Append(r);
            return p;
        }

        private Paragraph SetFirstParagraph()
        {
            var p = new Paragraph();
            var pp = new ParagraphProperties { Justification = new Justification() {Val = JustificationValues.Both} };
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

        private static Paragraph SetSecondParagraph()
        {
            var p = new Paragraph();
            var pp = new ParagraphProperties {Justification = new Justification() {Val = JustificationValues.Both}};
            p.Append(pp);

            var r = new Run();
            var rPr = new RunProperties(new RunFonts() {Ascii = FontFamily});
            rPr.Append(new FontSize() {Val = ParagraphFontSize});
            r.Append(rPr);
            r.Append(new Text() {Text = SecondParagraph});

            p.Append(r);

            return p;
        }

        private Table TraineeInformationTable(string[] columnNames)
        {
            if (columnNames == null) throw new ArgumentNullException(nameof(columnNames));

            // column row
            var table = new Table();
            var titleRow = new TableRow();
            this.SetTableStyle(table);
            //add table columns
            foreach (var col in columnNames)
            {
                var tc = new TableCell();
                var tcpParagraph = new Paragraph();
                // Vertical align center
                tc.Append(new TableCellProperties(new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }));
                
                //text center
                tcpParagraph.Append(new ParagraphProperties { Justification = new Justification() { Val = JustificationValues.Center } });
                var run = new Run();

                run.Append(
                    new RunProperties(
                        new FontSize() { Val = "24" }, 
                        new RunFonts() { Ascii = FontFamily }, 
                        new Bold() { Val = OnOffValue.FromBoolean(true) }
                        )
                    );

                run.Append(new Text(col));

                tcpParagraph.Append(run);
                tc.Append(tcpParagraph);

                titleRow.Append(tc);
            }

            table.Append(titleRow);

            var dataRow = new TableRow();


            dataRow.Append(CreateCell("Test"));


            table.Append(dataRow);

            return table;
        }

        private static TableCell CreateCell(string text)
        {
            return new TableCell(new Paragraph(new Run(new Text(text))));
        }

        private void SetTableStyle(OpenXmlElement table)
        {
            var properties = new TableProperties();

            //table borders
            var borders = new TableBorders
            {
                TopBorder = new TopBorder() {Val = new EnumValue<BorderValues>(BorderValues.Single)},
                BottomBorder = new BottomBorder() {Val = new EnumValue<BorderValues>(BorderValues.Single)},
                LeftBorder = new LeftBorder() {Val = new EnumValue<BorderValues>(BorderValues.Single)},
                RightBorder = new RightBorder() {Val = new EnumValue<BorderValues>(BorderValues.Single)},
                InsideHorizontalBorder = new InsideHorizontalBorder() {Val = BorderValues.Single},
                InsideVerticalBorder = new InsideVerticalBorder() {Val = BorderValues.Single}
            };

            properties.Append(borders);
            //set the table width to page width
            var tableWidth = new TableWidth() {Width = "5000", Type = TableWidthUnitValues.Pct};
            properties.Append(tableWidth);
            //add properties to table
            table.Append(properties);
        }
    }
}