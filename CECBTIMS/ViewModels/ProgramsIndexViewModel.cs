using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CECBTIMS.Models;

namespace CECBTIMS.ViewModels
{
    public class ProgramsIndexViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [Display(Name = "Program Type")]
        public ProgramType ProgramType { get; set; }
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "Application Closing On")]
        public DateTime ApplicationClosingDateTime { get; set; }
        [Display(Name = "Created On")]
        public DateTime? CreatedOn { get; set; }
    }
}