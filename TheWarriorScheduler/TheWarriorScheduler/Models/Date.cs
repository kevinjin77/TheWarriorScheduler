using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWarriorScheduler
{
    public class Date
    {
        public string start_time { get; set; }
        public string end_time { get; set; }
        public string weekdays { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public bool is_tba { get; set; }
        public bool is_cancelled { get; set; }
        public bool is_closed { get; set; }
    }
}
