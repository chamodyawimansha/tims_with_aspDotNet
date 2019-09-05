using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CECBTIMS.Models;

namespace CECBTIMS.ViewModels
{
    public class EmployeeDetailsViewModel
    {
        public Employee Employee { get; set; }
        public List<Program> Programs { get; set; }
    }
}