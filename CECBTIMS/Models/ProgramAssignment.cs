using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CECBTIMS.Models
{
    public class ProgramAssignment : BaseCols
    {
        public int Id { get; set; }
        public System.Guid EmployeeId { get; set; }
        public System.Guid EmployeeVersionId { get; set; }
        public string EPFNo { get; set; }
        public int ProgramId { get; set; }


        public virtual Program Program { get; set; }

    }
}