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
    public class Organizer
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int CreatedBy { get; set; }
        [
            Required, 
            DatabaseGenerated(DatabaseGeneratedOption.Computed)
        ]
        public DateTime CreatedAt { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } // For optimistic concurrency;

        public virtual ICollection<Program> Programs { get; set; } // organizer can have multiple programs;
        public virtual ICollection<ProgramArrangement> ProgramArrangement { get; set; }

    }

    public class ProgramArrangement
    {
        public int Id { get; set; }
        public int OrganizerId { get; set; }
        public int ProgramId { get; set; }
        [
            Required,
            DatabaseGenerated(DatabaseGeneratedOption.Computed)
        ]
        public DateTime CreatedAt { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } // For optimistic concurrency;

        public virtual Organizer Organizer { get; set; }
        public virtual Program Program { get; set; }
    }

    public class ResourcePerson
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Cost { get; set; }
        [
            Required,
            DatabaseGenerated(DatabaseGeneratedOption.Computed)
        ]
        public DateTime CreatedAt { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } // For optimistic concurrency;

        public virtual ICollection<ProgramResourcePersons> Programs { get; set; } // resource person can multiple programs
    }

    public class ProgramResourcePersons
    {
        public int Id { get; set; }
        public int ResourcePersonId { get; set; }
        public int ProgramId { get; set; }
        [
            Required,
            DatabaseGenerated(DatabaseGeneratedOption.Computed)
        ]
        public DateTime CreatedAt { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } // For optimistic concurrency;

        public virtual ResourcePerson ResourcePerson { get; set; }
        public virtual Program Program { get; set; }
    }

    public class Cost
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public int ProgramId { get; set; }
        [
            Required,
            DatabaseGenerated(DatabaseGeneratedOption.Computed)
        ]
        public DateTime CreatedAt { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } // For optimistic concurrency;

        public virtual Program Program { get; set; }
    }

    public enum ProgramType
    {
        Local = 1,
        Foreign = 2,
        InHouse = 3,
        PostGraduation = 4
    }

    public enum EmploymentNature
    {
        Permanent = 1, 
        Fixed = 2,
        Contract = 3,
        PermanentAndConfirm = 4,

    }

    public enum EmployeeCategory
    {
        Technical = 1 ,
        NonTechnical = 2,
        TechnicalAndNonTechnical = 3
    }

    public enum Currency
    {
        USD = 1,
        Euro = 2,
        GBP = 3,
        LKR = 4
    }

    public class Program
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public  ProgramType ProgramType { get; set; }
        public string TargetGroup { get; set; } // Multiple target groups.. serialized array
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
        public DateTime ApplicationClosingDateTime { get; set; }
        public string Brochure { get; set; } // File upload: File Name


        /**
         * Not Required
         */
        public EmploymentNature? EmploymentNature { get; set; }
        public EmployeeCategory? EmployeeCategory { get; set; }
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

        public string Requirements { get; set; } // serialized Array, For PostGrad

        public int? ProgramHours { get; set; } // For Local Program
        public int? DurationInDays { get; set; }
        public int? DurationInMonths { get; set; }
        public string Department { get; set; }


        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [
            Required,
            DataType(DataType.Date),
            DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true),
            DatabaseGenerated(DatabaseGeneratedOption.Computed),
        ]
        public DateTime? CreatedAt { get; set; }

        public Currency Currency { get; set; }
        public  double? ProgramFee { get; set; }
        public double? RegistrationFee { get; set; }
        public double? PerPersonFee { get; set; }
        public double? NoShowFee { get; set; }
        public double? MemberFee { get; set; }
        public double? NonMemberFee { get; set; }
        public double? StudentFee { get; set; }


        [Timestamp]
        public byte[] RowVersion { get; set; } // For optimistic concurrency;

        public virtual ICollection<ProgramResourcePersons> ResourcePersons { get; set; } // A program can have multiple resource persons
        public virtual ProgramArrangement Organizer { get; set; } // Program have only one Organizer;
        public virtual ICollection<Cost> Costs { get; set; }
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