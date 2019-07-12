using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CECBTIMS.Models
{
    public class Cost : BaseCols
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public int ProgramId { get; set; }

        public virtual Program Program { get; set; }
    }
}