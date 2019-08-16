using System;
using System.Collections.Generic;
using System.Linq;
using CECBTIMS.Controllers;


namespace CECBTIMS.Models
{
    public class DocumentHelper
    {
        // get organisers of the program
        public static string GetOrganisedBy(Program program)
        {
            return program.ProgramArrangements.Aggregate("", (current, item) => current == "" ? item.Organizer.Name : current + ", " + item.Organizer.Name);
        }

        public static string GetTargetGroup(Program program)
        {
            return program.TargetGroups.Aggregate("", (current, item) => current == "" ? item.Name : current + ", " + item.Name);
        }

        public static string GetTargetGroupList(Program program)
        {
            return program.TargetGroups.Aggregate("", (current, item) => current == "" ? item.Name : current + ", " + item.Name);
        }

        public static string GetTraineeManagerName()
        {
            return "Eng.L C K Karunaratne";
        }

    }
}