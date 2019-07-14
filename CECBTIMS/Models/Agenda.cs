using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CECBTIMS.Models
{
    public class Agenda : BaseCols
    {
        public int Id { get; set; }
        [
            Required,
        ]
        public string Name { get; set; }
        [
            Required,
            DataType(DataType.Time)
        ]
        public DateTime From { get; set; }
        [
            Required,
            DataType(DataType.Time)
        ]
        public DateTime To { get; set; }
        public int ProgramId { get; set; }

        public virtual Program Program { get; set; }
    }
}