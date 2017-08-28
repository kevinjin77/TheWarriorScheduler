using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWarriorScheduler
{
    public class Professor
    {
        public string tDept { get; set; }
        public string tSid { get; set; }
        public string institution_name { get; set; }
        public string tFname { get; set; }
        public string tMiddlename { get; set; }
        public string tLname { get; set; }
        public int tid { get; set; }
        public int tNumRatings { get; set; }
        public string rating_class { get; set; }
        public string contentType { get; set; }
        public string categoryType { get; set; }
        public string overall_rating { get; set; }
    }

    public class ProfessorResponse
    {
        public List<Professor> professors { get; set; }
        public int searchResultsTotal { get; set; }
        public int remaining { get; set; }
        public string type { get; set; }
    }
}
