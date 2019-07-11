using System.ComponentModel.DataAnnotations;

namespace CECBTIMS.Models
{
    public enum ProgramType
    {
        [Display(Name = "Local Program")]
        Local = 1,
        [Display(Name = "Foreign Program")]
        Foreign = 2,
        [Display(Name = "In-House Program")]
        InHouse = 3,
        [Display(Name = "Post Graduation Program")]
        PostGraduation = 4
    }
}