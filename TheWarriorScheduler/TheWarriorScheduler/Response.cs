using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWarriorScheduler
{
    public class Method
    {
        public string disclaimer { get; set; }
        public string license { get; set; }
    }

    public class Meta
    {
        public int requests { get; set; }
        public int timestamp { get; set; }
        public int status { get; set; }
        public string message { get; set; }
        public int method_id { get; set; }
        public Method method { get; set; }
    }

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

    public class Location
    {
        public string building { get; set; }
        public string room { get; set; }
    }

    public class Class
    {
        public Date date { get; set; }
        public Location location { get; set; }
        public List<object> instructors { get; set; }
    }

    public class Course
    {
        public string subject { get; set; }
        public string catalog_number { get; set; }
        public double units { get; set; }
        public string title { get; set; }
        public string note { get; set; }
        public int class_number { get; set; }
        public string section { get; set; }
        public string campus { get; set; }
        public int associated_class { get; set; }
        public string related_component_1 { get; set; }
        public string related_component_2 { get; set; }
        public int enrollment_capacity { get; set; }
        public int enrollment_total { get; set; }
        public int waiting_capacity { get; set; }
        public int waiting_total { get; set; }
        public object topic { get; set; }
        public List<object> reserves { get; set; }
        public List<Class> classes { get; set; }
        public List<object> held_with { get; set; }
        public int term { get; set; }
        public string academic_level { get; set; }
        public string last_updated { get; set; }
    }

    public class Response
    {
        public Meta meta { get; set; }
        public List<Course> data { get; set; }
    }
}
