using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CECBTIMS.Models
{
    public class ProgramArrangement : BaseCols
    {
        public int Id { get; set; }
        public int ProgramId { get; set; }
        public int OrganizerId { get; set; }

        public virtual Program Program { get; set; }
        public virtual Organizer Organizer { get; set; }
    }
}