﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CECBTIMS.Models
{
    public class Requirement : BaseCols
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProgramId { get; set; }

        public virtual Program Program { get; set; }
    }
}