using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

/**
 * Base Program model. other 4 program types inherit this model
 */
namespace CECBTIMS.Models
{
    public class Program : BaseCols
    {
        public int Id { get; set; }

        public string Title { get; set; }
        [Display(Name = "Program Type")]
        public ProgramType ProgramType { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Application Closing Date")]
        public DateTime ApplicationClosingDate { get; set; }
        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Application Closing Time")]
        public DateTime ApplicationClosingTime { get; set; }
        public string Brochure { get; set; } // File upload: File Name
        public string Venue { get; set; } // Not For PostGrad
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }
        [Display(Name = "Notified By")]
        public string NotifiedBy { get; set; } //  For Foreign
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Notified On")]
        public DateTime? NotifiedOn { get; set; } //  For Foreign
        [Display(Name = "Program Hours")]
        public byte? ProgramHours { get; set; } // For Local Program
        [Display(Name = "Duration In Days")]
        public byte? DurationInDays { get; set; }
        [Display(Name = "Duration In Months")]
        public byte? DurationInMonths { get; set; }
        public string Department { get; set; }
        public Currency Currency { get; set; }
        [Display(Name = "Program Fee")]
        public double? ProgramFee { get; set; }
        [Display(Name = "Registration Fee")]
        public float? RegistrationFee { get; set; }
        [Display(Name = "Per Person Fee")]
        public float? PerPersonFee { get; set; }
        [Display(Name = "No Show Fee")]
        public float? NoShowFee { get; set; }
        [Display(Name = "Member Fee")]
        public float? MemberFee { get; set; }
        [Display(Name = "None Member Fee")]
        public float? NonMemberFee { get; set; }
        [Display(Name = "Student Fee")]
        public float? StudentFee { get; set; }


        public virtual ICollection<Agenda> Agendas { get; set; }
        public virtual ICollection<TargetGroup> TargetGroups { get; set; }
        public virtual ICollection<ResourcePerson> ResourcePersons { get; set; }
        public virtual ICollection<ProgramArrangement> Organizers { get; set; }
        public virtual ICollection<Cost> Costs { get; set; }
        public virtual ICollection<Requirement> Requirements { get; set; }
        public virtual ICollection<ProgramEmploymentCategory> ProgramEmploymentCategories { get; set; }
        public virtual ICollection<ProgramEmploymentNature> ProgramEmploymentNatures { get; set; }
    }
}