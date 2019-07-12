using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CECBTIMS.Models
{
    public class ProgramResourcePersons : BaseCols
    {
        public int Id { get; set; }
        public int ResourcePersonId { get; set; }
        public int ProgramId { get; set; }

        public virtual ResourcePerson ResourcePerson { get; set; }
        public virtual Program Program { get; set; }
    }
}