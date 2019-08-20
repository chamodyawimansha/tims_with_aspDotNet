using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CECBTIMS.Models.Enums;
using DocumentFormat.OpenXml.Spreadsheet;

namespace CECBTIMS.Models
{
    public class DefaultColumn : BaseCols
    {
        public int Id { get; set; }
        public TableColumnName ColumnName { get; set; }
        public int TemplateId { get; set; }

        public Template Template { get; set; }
    }
}