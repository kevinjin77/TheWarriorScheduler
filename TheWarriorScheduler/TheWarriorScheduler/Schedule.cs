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
        private void getGaps(List<DateTime> times, List<int> gapList)
        {
            if (times.Count <= 2)
            {
                return;
            }

            times = times.OrderBy(x => x.TimeOfDay).ToList();

            for(int i = 1; i < times.Count - 1; i += 2)
            {
                int gap = Convert.ToInt32(Math.Abs(times[i].Subtract(times[i + 1]).TotalMinutes));
                gapList.Add(gap);
            }
        }
        public int calculateGapRating()
        {
            List<DateTime> mTimes = new List<DateTime>();
            List<DateTime> tTimes = new List<DateTime>();
            List<DateTime> wTimes = new List<DateTime>();
            List<DateTime> thTimes = new List<DateTime>();
            List<DateTime> fTimes = new List<DateTime>();
            foreach (Course c in this.Courses)
            {
                if (c.weekdays.Contains("M"))
                {
                    mTimes.Add(c.start_time);
                    mTimes.Add(c.end_time);
                }
                if (c.weekdays.Contains("T"))
                {
                    tTimes.Add(c.start_time);
                    tTimes.Add(c.end_time);
                }
                if (c.weekdays.Contains("W"))
                {
                    wTimes.Add(c.start_time);
                    wTimes.Add(c.end_time);
                }
                if (c.weekdays.Contains("Th"))
                {
                    thTimes.Add(c.start_time);
                    thTimes.Add(c.end_time);
                }
                if (c.weekdays.Contains("F"))
                {
                    fTimes.Add(c.start_time);
                    fTimes.Add(c.end_time);
                }
            }
            List<int> gapList = new List<int>();
            getGaps(mTimes, gapList);
            getGaps(tTimes, gapList);
            getGaps(wTimes, gapList);
            getGaps(thTimes, gapList);
            getGaps(fTimes, gapList);
            return 2;
        }
        public ScheduleStats ComputeStats()
        {
            ScheduleStats stats = new ScheduleStats();

            return stats;
        }
        public int Rating { get; set; }
        public List<Course> Courses = new List<Course>();
    }
}
