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
            DataType(DataType.Date),
            DisplayFormat(DataFormatString = "{0:yyyy-MM-dd | HH:mm}", ApplyFormatInEditMode = true)
        ]
        public DateTime CreatedAt
        {
            get
            {
                return this.createdAt.HasValue
                    ? this.createdAt.Value
                    : DateTime.Now;
            }

            set { this.createdAt = value; }
        }

        private DateTime? createdAt = null;

        [Timestamp]
        public byte[] RowVersion { get; set; } // For optimistic concurrency;

        public virtual ICollection<ProgramArrangement> ProgramArrangement { get; set; }// organizer can have multiple programs;
    }
}