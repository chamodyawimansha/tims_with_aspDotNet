using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CECBTIMS.Models
{
    public class Cost
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public int ProgramId { get; set; }
        [
            Required,
            DatabaseGenerated(DatabaseGeneratedOption.Computed)
        ]
        public DateTime CreatedAt { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } // For optimistic concurrency;

        public virtual Program Program { get; set; }
    }
}