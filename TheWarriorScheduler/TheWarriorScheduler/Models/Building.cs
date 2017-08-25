using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWarriorScheduler
{
    public class Data
    {
        public string building_id { get; set; }
        public string building_code { get; set; }
        public string building_name { get; set; }
        public object building_parent { get; set; }
        public List<string> alternate_names { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public List<object> building_sections { get; set; }
        public string youtube_vid { get; set; }
        public string streamable_vid { get; set; }
        public List<object> building_outline { get; set; }
    }

    public class Building
    {
        public Data data { get; set; }
    }
}
