using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CECBTIMS.Models
{
    public class DefaultColumn : BaseCols
    {
        public int Id { get; set; }
        public string ColumnName { get; set; }
        public int TemplateId { get; set; }

        public Template Template { get; set; }
    }
}