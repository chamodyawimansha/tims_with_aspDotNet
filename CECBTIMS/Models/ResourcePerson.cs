using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CECBTIMS.Models
{
    public class ResourcePerson
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Cost { get; set; }
        [
            Required,
            DatabaseGenerated(DatabaseGeneratedOption.Computed)
        ]
        public DateTime CreatedAt { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } // For optimistic concurrency;

        public virtual ICollection<ProgramResourcePersons> Programs { get; set; } // resource person can multiple programs
    }
}