using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CECBTIMS.Models.Enums;

namespace CECBTIMS.Models
{
    public class Template : BaseCols
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public string FileName { get; set; }
        public ProgramType ProgramType { get; set; }
        public FileType FileType { get; set; }
        public FileMethod FileMethod { get; set; } = FileMethod.Upload;
        public string OriginalFileName { get; set; }
        public bool HasConfigurableTable { get; set; } = false;

        public virtual ICollection<DefaultColumn> DefaultColumns { get; set; }
    }
}