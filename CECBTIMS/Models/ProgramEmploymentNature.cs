using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CECBTIMS.Models
{
    public class ProgramEmploymentNature : BaseCols
    {
        public int Id { get; set; }
        public int ProgramId { get; set; }
        public EmploymentNature EmploymentNature { get; set; }

        public Program Program { get; set; }
    }
}