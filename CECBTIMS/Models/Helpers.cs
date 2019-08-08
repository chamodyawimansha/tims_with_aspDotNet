using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using CECBTIMS.DAL;

namespace CECBTIMS.Models
{
    public class Helpers
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // return the file number
        public string FileNumber(object[] param)
        {
            return "";
        }
        //returns current year
        public string GetYear(object[] param)
        {
            return DateTime.Now.ToString("yyyy");
        }
        //returns today
        public string GetToday(object[] param)
        {
            return DateTime.Now.ToString("yyyy.mm.dd");
        }
        // returns the programs title.. require program id
        public string GetProgramtitle(object[] param)
        {
            if (param == null) throw new ArgumentNullException(nameof(param));

            var program = getProgram(Convert.ToInt32(param.FirstOrDefault()));

            return program.Title;
        }
        // org1, org2
        public string GetOrganisedby(object[] param)
        {
            if (param == null) throw new ArgumentNullException(nameof(param));

            var program = getProgram(Convert.ToInt32(param.FirstOrDefault()));

            return program.ProgramArrangements.Aggregate("", (current, item) => current == "" ? item.Organizer.Name : current + ", " + item.Organizer.Name);
        }





        private Program getProgram(int id)
        {
            return db.Programs.Find(id);
        }

        public static string FigureVarName(string name)
        {
            var var = name.ToLower();
            var = var.First().ToString().ToUpper() + var.Substring(1);

            return "Get" + var;
        }

        public static string CallMethod(string methodName, int programId)
        {

            var type = typeof(DocumentHelper);
            var method = type.GetMethod(FigureVarName(methodName));
            
            var classInstance = Activator.CreateInstance(type, programId);

//            object id = Convert.ToInt32(programId);
            
            if (method != null)
            {
                return (string) method.Invoke(null, null);
//                return (string) method.Invoke(classInstance, new object[] { new[] { id }});
            }
            
            return "null";
        }

    }
}