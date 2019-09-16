using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CECBTIMS.Models.Enums;

namespace CECBTIMS.Models
{
    public class EmploymentNature : BaseCols
    {
        public int Id { get; set; }
        public EmploymentNatures EmpNature { get; set; }
        public int ProgramId { get; set; }

        public virtual Program Program { get; set; }
    }
}