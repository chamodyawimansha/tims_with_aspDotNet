using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CECBTIMS.Models
{
    public class EmploymentNature
    {
        public int Id { get; set; }
        public EmploymentNature EmpCategory { get; set; }
        public int ProgramId { get; set; }

        public virtual Program Program { get; set; }
    }
}