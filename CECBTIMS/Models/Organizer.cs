using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CECBTIMS.Models
{
    public class Organizer : BaseCols
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        
        public virtual ICollection<ProgramArrangement> ProgramArrangement { get; set; }// organizer can have multiple programs;
    }
}