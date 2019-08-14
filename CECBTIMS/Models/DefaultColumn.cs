using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CECBTIMS.Models.Enums;

namespace CECBTIMS.Models
{
    public class DefaultColumn
    {
        public int Id { get; set; }
        public int TimsFileId { get; set; }
        public TableColumnName ColumnName { get; set; }

        public virtual TimsFile TimsFile { get; set; }
    }
}


/**
 *
 * if file id don't have in default column save default columns for first time
 * edit default columns
 *      
 *
 *
 * /
