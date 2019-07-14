using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CECBTIMS.Models
{
    public class TargetGroup : BaseCols
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int ProgramId { get; set; }
        public virtual Program Program { get; set; }

    }
}