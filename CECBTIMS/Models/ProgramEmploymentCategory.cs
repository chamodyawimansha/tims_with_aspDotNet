using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CECBTIMS.Models
{
    public class ProgramEmploymentCategory : BaseCols
    {
        public int Id { get; set; }
        public int ProgramId { get; set; }
        public EmployeeCategory EmployeeCategory { get; set; }

        public Program Program { get; set; }
    }
}