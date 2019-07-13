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
        public  ProgramType ProgramType { get; set; }
        [
            Required,
            DataType(DataType.Date),
            DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)
        ]
        public DateTime StartDate { get; set; }
        [
            Required,
            DataType(DataType.Date),
            DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)
        ]
        public DateTime ApplicationClosingDate { get; set; }
        [
            Required,
            DataType(DataType.Time)
        ]
        public DateTime ApplicationClosingTime { get; set; }
        public string Brochure { get; set; } // File upload: File Name
        public string Venue { get; set; } // Not For PostGrad
        [
            DataType(DataType.Date),
            DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)
        ]
        public DateTime? EndDate { get; set; }
        public string NotifiedBy { get; set; } //  For Foreign
        [
            DataType(DataType.Date),
            DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)
        ]
        public DateTime? NotifiedOn { get; set; } //  For Foreign

        public byte? ProgramHours { get; set; } // For Local Program
        public byte? DurationInDays { get; set; }
        public byte? DurationInMonths { get; set; }
        public string Department { get; set; }

        public Currency Currency { get; set; }
        public double? ProgramFee { get; set; }
        public float? RegistrationFee { get; set; }
        public float? PerPersonFee { get; set; }
        public float? NoShowFee { get; set; }
        public float? MemberFee { get; set; }
        public float? NonMemberFee { get; set; }
        public float? StudentFee { get; set; }


        public virtual ICollection<TargetGroup> TargetGroups { get; set; }
        public virtual ICollection<ProgramResourcePersons> ResourcePersons { get; set; } 
        public virtual ICollection<ProgramArrangement> Organizer { get; set; } 
        public virtual ICollection<Cost> Costs { get; set; }
        public virtual ICollection<Requirement> Requirements { get; set; }
        public virtual ICollection<ProgramEmploymentCategory> ProgramEmploymentCategories { get; set; }
        public virtual ICollection<ProgramEmploymentNature> ProgramEmploymentNatures { get; set; }
    }
}

//$table->bigIncrements('id');
//$table->string ('program_id')->unique();
//$table->string ('program_title');
//$table->string ('organised_by_id');
//$table->string ('target_group');
//$table->timestamp('start_date')->useCurrent = true;
//$table->integer('duration');
//$table->timestamp('application_closing_date_time')->useCurrent = true;
//$table->string ('nature_of_the_employment');//['permanent', 'fixed', 'contract']
//$table->string ('employee_category');//,['technical', 'non-technical', 'both']
//$table->string ('venue');
//$table->string ('duration_by');
//$table->float ('program_fee')->nullable();
//$table->float ('non_member_fee')->nullable();
//$table->float ('member_fee')->nullable();
//$table->float ('student_fee')->nullable();
//$table->string ('brochure_url')->nullable();
//$table->string ('created_by');
//$table->string ('updated_by')->nullable();
//$table->timestamps();
//Program Hours



//Foregin
//$table->bigIncrements('id');
//$table->string ('program_id')->unique()->index();
//$table->string ('program_title');
//$table->string ('program_type');
//$table->string ('organised_by_id');
//$table->string ('notified_by');
//$table->date('notified_on');
//$table->string ('target_group');
//$table->string ('nature_of_the_employment');
//$table->string ('employee_category');
//$table->string ('venue');
//$table->enum('currency',['usd', 'euro', 'gbp', 'lkr']);
//$table->float ('program_fee')->nullable();
//$table->date('start_date');
//$table->date('end_date');
//$table->string ('application_closing_date_time');
//$table->string ('duration');
//$table->text('other_costs')->nullable();
//$table->string ('brochure_url')->nullable();
//$table->string ('created_by');
//$table->string ('updated_by')->nullable();
//$table->timestamps();

//INHOUSE
//$table->bigIncrements('id');
//$table->string ('program_id')->unique();
//$table->text('program_title');
//$table->string ('target_group');
//$table->string ('organised_by_id');
//$table->string ('nature_of_the_employment');
//$table->string ('employee_category');
//$table->string ('venue');
//$table->timestamp('start_date')->useCurrent = true;
//$table->time('end_time')->useCurrent = true;
//$table->timestamp('application_closing_date_time')->useCurrent = true;
//$table->float ('no_show_fee')->nullable();
//$table->float ('per_person_fee')->nullable();
//$table->text('resource_person'); //person name, designation, and cost
//$table->text('other_costs')->nullable();
//$table->float ('hours')->nullable();;
//$table->string ('brochure_url')->nullable();
//$table->string ('created_by');
//$table->string ('updated_by')->nullable();
//$table->timestamps();


//POSTGRAD
//$table->bigIncrements('id');
//$table->string ('program_id')->unique();
//$table->string ('program_title');
//$table->string ('organised_by_id');
//$table->string ('department');
//$table->text('requirements');
//$table->text('target_group');
//$table->timestamp('start_date');
//$table->string ('duration'); //months
//$table->timestamp('application_closing_date_time')->useCurrent = true;
//$table->float ('registration_fees')->nullable();;
//$table->text('other_costs')->nullable();;
//$table->string ('brochure_url')->nullable();
//$table->string ('created_by');
//$table->string ('updated_by')->nullable();
//$table->timestamps();