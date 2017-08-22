using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWarriorScheduler
{
    public class Course
    {
        public int ClassNumber { get; set; }
        public int Section { get; set; }
        public bool IsFull { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime Endtime { get; set; }
        public string Location { get; set; }
        public string Instructor { get; set; }
    }
}
