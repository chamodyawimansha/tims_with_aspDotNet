using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CECBTIMS.Models
{
    public class Payment : BaseCols
    {
        public int Id { get; set; }
        public int ProgramId { get; set; }
        public Guid WorkSpaceId { get; set; }
        public string Title { get; set; }
        public double value { get; set; }
        public int? ChequeNo { get; set; }
        public string ChequeFile { get; set; }
        public virtual Program Program { get; set; }

//        they need to know payment details
//        section payments how many employees per section
//            what need to pay by sections
//            who payed and balance what need to pay
//
//
//            payment Controllers 
//            select Program type > next > selectProgram > show pyaments with program trainee sections 
// find and select is good

        // if program id not present execute above procedure

    }
}