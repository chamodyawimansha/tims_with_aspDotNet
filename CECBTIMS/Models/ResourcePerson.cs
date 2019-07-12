using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CECBTIMS.Models
{
    public class ResourcePerson : BaseCols
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Cost { get; set; }

        public virtual ICollection<ProgramResourcePersons> Programs { get; set; } // resource person can multiple programs
    }
}