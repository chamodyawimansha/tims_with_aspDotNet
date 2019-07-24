using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CECBTIMS.Models.Enums;

namespace CECBTIMS.Models
{
    public class File : BaseCols
    {
        public Guid Id { get; set; }
        [StringLength(255)]
        public string Title { get; set; }
        [StringLength(255)]
        public string Details { get; set; }
        [StringLength(255)]
        public string FileName { get; set; }
        [StringLength(255)]
        public string OriginalFileName { get; set; }
        public FileType FileType { get; set; }
        public FileMethod FileMethod { get; set; }

        public int? ProgramId { get; set; }
        public virtual Program Program { get; set; }


    }
}