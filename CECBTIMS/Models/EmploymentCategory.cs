using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CECBTIMS.Models.Enums;

namespace CECBTIMS.Models
{
    public class EmploymentCategory : BaseCols
    {
        public int Id { get; set; }
        public EmployeeCategory EmpCategory { get; set; }
        public int ProgramId { get; set; }

        public virtual Program Program { get; set; }
        
    }
}