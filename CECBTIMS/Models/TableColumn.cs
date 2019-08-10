using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CECBTIMS.Models
{
    public enum TableColumnName{
        // column names
    }

    public class TableColumn : BaseCols
    {
        public int Id { get; set; }
        public int TimsFileId { get; set; }
        public TableColumnName ColumnName { get; set; }

        public virtual TimsFile TimsFile { get; set; }
    }
}


//table column enums
