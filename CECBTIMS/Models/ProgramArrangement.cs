using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CECBTIMS.Models
{
    public class ProgramArrangement
    {
        public int Id { get; set; }
        public int OrganizerId { get; set; }
        public int ProgramId { get; set; }
        [
            Required,
            DatabaseGenerated(DatabaseGeneratedOption.Computed)
        ]
        public DateTime CreatedAt { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } // For optimistic concurrency;

        public virtual Organizer Organizer { get; set; }
        public virtual Program Program { get; set; }
    }
}