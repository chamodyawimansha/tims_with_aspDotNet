using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CECBTIMS.Models
{
    public class Organizer
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int CreatedBy { get; set; }
        [
            Required, 
            DatabaseGenerated(DatabaseGeneratedOption.Computed)
        ]
        public DateTime CreatedAt { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } // For optimistic concurrency;

        public virtual ICollection<ProgramArrangement> ProgramArrangement { get; set; }// organizer can have multiple programs;
    }
}