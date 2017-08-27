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
            Schedule s = new Schedule();
            foreach(Course c in Courses)
            {
                //using (System.IO.StreamWriter file =
                //new System.IO.StreamWriter(@"C:\Users\Kevin\Documents\test.txt", true))
                //{
                //    file.WriteLine($"{c.subject} {c.catalog_number}, {c.section}: {c.classes[0].date.weekdays} {c.classes[0].date.start_time} - {c.classes[0].date.end_time} "
                //    + (c.classes[0].instructors.Count == 0 ? " " : c.instructor) + $" {c.classroom}");
                //}
                Console.WriteLine($"{c.subject} {c.catalog_number}, {c.section}: {c.classes[0].date.weekdays} {c.classes[0].date.start_time} - {c.classes[0].date.end_time} "
                    + (c.classes[0].instructors.Count == 0 ? " " : c.instructor) + $" {c.classroom}");
                s.Courses.Add(c);
            }
            //using (System.IO.StreamWriter file =
            //new System.IO.StreamWriter(@"C:\Users\Kevin\Documents\test.txt", true))
            //{
            //    file.WriteLine($"{s.gapRating}" + "!\n");
            //}
            Console.WriteLine($"Gap: {s.gapRating}, Lunch: {s.lunchRating}, Overall: {s.Rating}");
            //Console.WriteLine(String.Join(",", s.calculateGapRating()));
            //Console.WriteLine(String.Join(",", s.calculateDistanceRating()));
            Console.WriteLine("\n");
        }

        private void getTimeGaps(List<DateTime> times, List<int> gapList)
        {
            if (times.Count <= 2)
            {
                return;
            }

            times = times.OrderBy(x => x.TimeOfDay).ToList();

            for (int i = 1; i < times.Count - 1; i += 2)
            {
                int gap = Convert.ToInt32(Math.Abs(times[i].Subtract(times[i + 1]).TotalMinutes));
                gapList.Add(gap);
            }
        }

        private int calculateGapRating()
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
            getTimeGaps(mTimes, gapList);
            getTimeGaps(tTimes, gapList);
            getTimeGaps(wTimes, gapList);
            getTimeGaps(thTimes, gapList);
            getTimeGaps(fTimes, gapList);

            int count = 0;
            foreach (int num in gapList)
            {
                if (num > 10 && num <= 70)
                {
                    --count;
                }
            }
            return count;
        }

        private int dateToInt(DateTime date)
        {
            return date.Hour * 60 + date.Minute;
        }

        private int getLunchRating(List<DateTime> times)
        {
            times = times.OrderBy(x => x.TimeOfDay).ToList();

            if ((times.Count == 0) || (dateToInt(times[0]) >= 720) || (dateToInt(times[times.Count - 1]) <= 810))
            {
                return 2;
            }

            int max = 0;
            for (int i = 1; i < times.Count - 1; i += 2)
            {
                int start = dateToInt(times[i]);
                int end = dateToInt(times[i + 1]);
                if (start <= 840 && 690 <= end)
                {
                    if (end - start <= 30)
                    {
                        max = Math.Max(0, max);
                    }
                    else if (end - start <= 70)
                    {
                        max = Math.Max(1, max);
                    }
                    else
                    {
                        max = Math.Max(2, max);
                    }
                }
            }
            return max;
        }

        private int calculateLunchRating()
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

            return getLunchRating(mTimes) + getLunchRating(tTimes) + getLunchRating(wTimes)
                + getLunchRating(thTimes) + getLunchRating(fTimes);
        }

        private void getDistances(List<Course> courseList, List<int> gapList)
        {
            if (courseList.Count <= 1)
            {
                return;
            }

            for (int i = 0; i < courseList.Count - 1; ++i)
            {
                int seconds = LocationHelper.distanceInSeconds(courseList[i].building, courseList[i + 1].building);
                gapList.Add(seconds);
            }
        }

        private List<int> calculateDistanceRating()
        {
            List<Course> mCourses = new List<Course>();
            List<Course> tCourses = new List<Course>();
            List<Course> wCourses = new List<Course>();
            List<Course> thCourses = new List<Course>();
            List<Course> fCourses = new List<Course>();

            foreach(Course c in this.Courses)
            {
                if (c.weekdays.Contains("M"))
                {
                    mCourses.Add(c);
                }
                if (c.weekdays.Contains("T"))
                {
                    tCourses.Add(c);
                }
                if (c.weekdays.Contains("W"))
                {
                    wCourses.Add(c);
                }
                if (c.weekdays.Contains("Th"))
                {
                    thCourses.Add(c);
                }
                if (c.weekdays.Contains("F"))
                {
                    fCourses.Add(c);
                }
            }

            List<int> gapList = new List<int>();
            getDistances(sortCoursesByTime(mCourses), gapList);
            getDistances(sortCoursesByTime(tCourses), gapList);
            getDistances(sortCoursesByTime(wCourses), gapList);
            getDistances(sortCoursesByTime(thCourses), gapList);
            getDistances(sortCoursesByTime(fCourses), gapList);

            return gapList;
        }

        private List<Course> sortCoursesByTime (List<Course> courseList)
        {
            List<Course> newList = courseList.OrderBy(x => x.start_time.Hour).ToList();
            return newList;
        }

        public ScheduleStats ComputeStats()
        {
            ScheduleStats stats = new ScheduleStats();

            return stats;
        }

        /*private int scaleRating()
        {

        }*/

        public int gapRating
        {
            get
            {
                return this.calculateGapRating();
            }
        }

        public int lunchRating
        {
            get
            {
                return this.calculateLunchRating();
            }
        }

        public int Rating
        {
            get
            {
                return this.gapRating + this.lunchRating;
            }
        }

        public List<Course> Courses = new List<Course>();
    }
}
