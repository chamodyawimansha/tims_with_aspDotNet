using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CECBTIMS.Models
{
    public class EmploymentCategory : BaseCols
    {
        public int Id { get; set; }
        public EmploymentCategory EmpCategory { get; set; }
        public int ProgramId { get; set; }

        public virtual Program Program { get; set; }
        
    }
}