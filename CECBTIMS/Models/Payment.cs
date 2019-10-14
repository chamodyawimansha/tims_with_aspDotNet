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
        
    }
}
//
//fix payments 
//    need to konw what sections need to pay