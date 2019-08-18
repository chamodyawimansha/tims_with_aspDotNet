using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CECBTIMS.Models.Enums;

namespace CECBTIMS.Models
{

    public class TableColumn : BaseCols
    {
        public int Id { get; set; }
        public int TimsFileId { get; set; }
        public TableColumnName ColumnName { get; set; }

        public virtual Brochure TimsFile { get; set; }
    }
}


//table column enums
