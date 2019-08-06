using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CECBTIMS.Models
{
    public class Helpers
    {

        public static string FileNumber()
        {
            return "";
        }
        //returns current year
        public static string GetYear()
        {
            return DateTime.Now.ToString("yyyy");
        }
        //returns today
        public static string GetToday()
        {
            return DateTime.Now.ToString("yyyy.mm.dd");
        }
        // returns the programs title.. require program id
        public static string GetProgramTitle()
        {
            return "Hello World";
        }

    }
}