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
                    + (c.classes[0].instructors.Count == 0 ? " " : c.instructor) + $" {c.classroom} {c.instructor_rating}");
                s.Courses.Add(c);
            }
            //using (System.IO.StreamWriter file =
            //new System.IO.StreamWriter(@"C:\Users\Kevin\Documents\test.txt", true))
            //{
            //    file.WriteLine($"{s.gapRating}" + "!\n");
            //}
            Console.WriteLine($"Gap: {s.gapRating}, Lunch: {s.lunchRating}, Professor: {s.professorRating}, Overall: {s.Rating}");
            //Console.WriteLine(String.Join(",", s.calculateGapRating()));
            //Console.WriteLine(String.Join(",", s.calculateDistanceRating()));
            Console.WriteLine("\n");
        }

        private List<Course> processSchedule(string weekday)
        {
            List<Course> listCourse = new List<Course>();
            foreach (Course c in this.Courses)
            {
                if (c.weekdays.Contains(weekday))
                {
                    listCourse.Add(c);
                }
            }
            return listCourse;
        }

        private List<DateTime> toTimes(List<Course> courseList)
        {
            List<DateTime> timeList = new List<DateTime>();
            foreach(Course c in courseList)
            {
                timeList.Add(c.start_time);
                timeList.Add(c.end_time);
            }
            return timeList;
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
            List<int> gapList = new List<int>();
            getTimeGaps(this.mTimes, gapList);
            getTimeGaps(this.tTimes, gapList);
            getTimeGaps(this.wTimes, gapList);
            getTimeGaps(this.thTimes, gapList);
            getTimeGaps(this.fTimes, gapList);

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
            return getLunchRating(this.mTimes) + getLunchRating(this.tTimes) + getLunchRating(this.wTimes)
                + getLunchRating(this.thTimes) + getLunchRating(this.fTimes);
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
            List<int> gapList = new List<int>();
            getDistances(sortCoursesByTime(this.mCourses), gapList);
            getDistances(sortCoursesByTime(this.tCourses), gapList);
            getDistances(sortCoursesByTime(this.wCourses), gapList);
            getDistances(sortCoursesByTime(this.thCourses), gapList);
            getDistances(sortCoursesByTime(this.fCourses), gapList);

            return gapList;
        }

        private List<Course> sortCoursesByTime(List<Course> courseList)
        {
            List<Course> newList = courseList.OrderBy(x => x.start_time.Hour).ToList();
            return newList;
        }

        private float calculateProfessorRating()
        {
            float sum = 0;
            //int numStudents = 0;
            foreach (Course c in this.Courses)
            {
                sum += (c.instructor_rating != 0) ? c.instructor_rating : 2.5f;
                //if (c.instructor_rating != 0)
                //{    
                //sum += c.instructor_rating.rating * c.instructor_rating.num_students;
                //numStudents += c.instructor_rating.num_students;
                //}
            }
            return sum / this.Courses.Count;
        }

        public ScheduleStats ComputeStats()
        {
            ScheduleStats stats = new ScheduleStats();

            return stats;
        }

        private List<Course> mCourses
        { get { return processSchedule("M"); } }
        private List<Course> tCourses
        { get { return processSchedule("T"); } }
        private List<Course> wCourses
        { get { return processSchedule("W"); } }
        private List<Course> thCourses
        { get { return processSchedule("Th"); } }
        private List<Course> fCourses
        { get { return processSchedule("F"); } }

        private List<DateTime> mTimes
        { get { return toTimes(this.mCourses); } }
        private List<DateTime> tTimes
        { get { return toTimes(this.tCourses); } }
        private List<DateTime> wTimes
        { get { return toTimes(this.wCourses); } }
        private List<DateTime> thTimes
        { get { return toTimes(this.thCourses); } }
        private List<DateTime> fTimes
        { get { return toTimes(this.fCourses); } }

        public int gapRating
        { get { return this.calculateGapRating(); } }

        public int lunchRating
        { get { return this.calculateLunchRating(); } }

        public float professorRating
        { get { return this.calculateProfessorRating() * 20; } }

        public double Rating
        { get { return Math.Round(this.gapRating + this.lunchRating + this.professorRating, 2); } }

        public List<Course> Courses = new List<Course>();
    }
}
