using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CECBTIMS.Models
{
    public class ResourcePerson : BaseCols
    {
        [Key] public int Id { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Designation { get; set; }
        [Required] public double Cost { get; set; }
        public int ProgramId { get; set; }

        public virtual Program Program { get; set; }
    }
}