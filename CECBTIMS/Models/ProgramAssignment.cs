using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using CECBTIMS.Models.Enums;

namespace CECBTIMS.Models
{
    public class ProgramAssignment : BaseCols
    {
        public int Id { get; set; }
        public System.Guid EmployeeId { get; set; }
        public System.Guid EmployeeVersionId { get; set; }
        public string EPFNo { get; set; }
        public MemberType MemberType { get; set; }
        [ForeignKey("Program")]
        public int ProgramId { get; set; }


        public virtual Program Program { get; set; }

    }
}