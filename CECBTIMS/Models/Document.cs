using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CECBTIMS.Models.Enums;

namespace CECBTIMS.Models
{
    public class Document : BaseCols
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public string FileName { get; set; }
        public ProgramType ProgramType { get; set; }
        public FileType FileType { get; set; }
        public FileMethod FileMethod { get; set; } = FileMethod.Generate;
        public int ProgramId { get; set; }
        public int DocumentNumber { get; set; } // document count in one year

        public virtual Program Program { get; set; }
    }
}