using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWarriorScheduler
{
    public class Schedule
    {
        public void printSchedule()
        {
            foreach(Course c in Courses)
            {
                Console.WriteLine($"{c.subject} {c.catalog_number}, {c.section}: {c.classes[0].date.weekdays} {c.classes[0].date.start_time} - {c.classes[0].date.end_time} " + (c.classes[0].instructors.Count == 0 ? " " : c.classes[0].instructors[0]));
            }
            Console.WriteLine("\n");
        }
        public int Rating { get; set; }
        public List<Course> Courses = new List<Course>();
    }
}
