using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace CECBTIMS.Models
{
    public class Helpers
    {
        // return the file number
        public string FileNumber()
        {
            return "";
        }
        //returns current year
        public string GetYear()
        {
            return DateTime.Now.ToString("yyyy");
        }
        //returns today
        public string GetToday()
        {
            return DateTime.Now.ToString("yyyy.mm.dd");
        }
        // returns the programs title.. require program id
        public string GetProgramtitle()
        {
            return "Hello World";
        }

        private static string FigureVarName(string name)
        {
            var var = name.ToLower();
            var = var.First().ToString().ToUpper() + var.Substring(1);

            return "Get" + var;
        }

        public static string CallMethod(string methodVar, int? programId)
        {

            var type = typeof(Helpers);
            var method = type.GetMethod(FigureVarName(methodVar));

            var c = new Helpers();

            if (method != null)
            {
                return (string)method.Invoke(c, null);

            }

            return null;
        }

    }
}