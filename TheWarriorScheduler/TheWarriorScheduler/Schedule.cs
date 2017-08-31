using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWarriorScheduler
{
    public class Schedule
    {
        public Schedule(List<string> lectureList)
        {
            LectureList = lectureList;
        }

        public void printSchedule()
        {
            foreach (Course c in Courses)
            {
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"test.txt", true))
                {
                    file.WriteLine($"{c.subject} {c.catalog_number}, {c.section}: {c.classes[0].date.weekdays} {c.classes[0].date.start_time} - {c.classes[0].date.end_time} "
                    + (c.classes[0].instructors.Count == 0 ? "???" : c.instructor) + $" {c.classroom} {c.instructor_rating}");
                }
                Console.WriteLine($"{c.subject} {c.catalog_number}, {c.section}: { c.classes[0].date.weekdays} {c.classes[0].date.start_time} - {c.classes[0].date.end_time} "
                    + (c.classes[0].instructors.Count == 0 ? "???" : c.instructor) + $" {c.classroom} {c.instructor_rating}");
                //Courses.Add(c);
            }
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"test.txt", true))
            {
                file.WriteLine($"Gap: {gapRating}, Lunch: {lunchRating}, Professor: {professorRating}, Distance: {distanceRating}, Proximity: {proximityRating}, Overall: {Rating}");
                file.WriteLine("\n");
            }
            Console.WriteLine($"Gap: {gapRating}, Lunch: {lunchRating}, Professor: {professorRating}, Distance: {distanceRating}, Proximity: {proximityRating}, Overall: {Rating}");
            //Console.WriteLine(String.Join(",", s.calculateGapRating()));
            //Console.WriteLine(String.Join(",", s.calculateDistanceRating()));
            Console.WriteLine("\n");
        }

        private List<Course> processSchedule(string weekday)
        {
            List<Course> listCourse = new List<Course>();
            foreach (Course c in Courses)
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

            //times = times.OrderBy(x => x.TimeOfDay).ToList();

            for (int i = 1; i < times.Count - 1; i += 2)
            {
                int gap = Convert.ToInt32(Math.Abs(times[i].Subtract(times[i + 1]).TotalMinutes));
                gapList.Add(gap);
            }
        }

        private int calculateGapRating()
        {
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

        private double getLunchRating(List<DateTime> times)
        {
            //times = times.OrderBy(x => x.TimeOfDay).ToList();

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

        private double calculateLunchRating()
        {
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

        private double calculateDistanceRatingByCourseList(List<Course> courseList)
        {
            double result = 0;
            if (courseList.Count > 1)
            {
                for (int i = 0; i < courseList.Count - 1; ++i)
                {
                    int gap = Convert.ToInt32(Math.Abs(courseList[i].end_time.Subtract(courseList[i + 1].start_time).TotalMinutes));
                    if (gap <= 10)
                    {
                        int seconds = LocationHelper.distanceInSeconds(courseList[i].building, courseList[i + 1].building);
                        if (seconds >= 200)
                        {
                            result -= (seconds - 200);
                        }
                    }
                }
            }
            return result / 60;
        }

        private double calculateDistanceRating()
        {
            double result = calculateDistanceRatingByCourseList(mCourses)
                + calculateDistanceRatingByCourseList(tCourses)
                + calculateDistanceRatingByCourseList(wCourses)
                + calculateDistanceRatingByCourseList(thCourses)
                + calculateDistanceRatingByCourseList(fCourses);
            return result;
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
            foreach (Course c in Courses)
            {
                sum += (c.instructor_rating != 0) ? c.instructor_rating : 2.5f;
                //if (c.instructor_rating != 0)
                //{    
                //sum += c.instructor_rating.rating * c.instructor_rating.num_students;
                //numStudents += c.instructor_rating.num_students;
                //}
            }
            return sum / Courses.Count;
        }

        private int calculateProximityRating(List<string> lectureList)
        {
            int count = 0;
            if (lectureList == null)
            {
                return count;
            }
            for (int i = 0; i < lectureList.Count; ++i)
            {
                if (Courses[i].section == lectureList[i])
                {
                    ++count;
                }
            }
            return count;
        }

        public ScheduleStats ComputeStats()
        {
            ScheduleStats stats = new ScheduleStats();

            return stats;
        }

        public List<string> LectureList { get; set; }

        /*private List<Course> _mCourses;
        private List<Course> mCourses
        { get { return _mCourses != null ? _mCourses : _mCourses = sortCoursesByTime(processSchedule("M")); } }
        private List<Course> _tCourses;
        private List<Course> tCourses
        { get { return _tCourses != null ? _tCourses : _tCourses = sortCoursesByTime(processSchedule("T")); } }
        private List<Course> _wCourses;
        private List<Course> wCourses
        { get { return _wCourses != null ? _wCourses : _wCourses = sortCoursesByTime(processSchedule("W")); } }
        private List<Course> _thCourses;
        private List<Course> thCourses
        { get { return _thCourses != null ? _thCourses : _thCourses = sortCoursesByTime(processSchedule("Th")); } }
        private List<Course> _fCourses;
        private List<Course> fCourses
        { get { return _fCourses != null ? _fCourses : _fCourses = sortCoursesByTime(processSchedule("F")); } }

        private List<DateTime> _mTimes;
        private List<DateTime> mTimes
        { get { return _mTimes != null ? _mTimes : _mTimes = toTimes(mCourses); } }
        private List<DateTime> _tTimes;
        private List<DateTime> tTimes
        { get { return _tTimes != null ? _tTimes : _tTimes = toTimes(tCourses); } }
        private List<DateTime> _wTimes;
        private List<DateTime> wTimes
        { get { return _wTimes != null ? _wTimes : _wTimes = toTimes(wCourses); } }
        private List<DateTime> _thTimes;
        private List<DateTime> thTimes
        { get { return _thTimes != null ? _thTimes : _thTimes = toTimes(thCourses); } }
        private List<DateTime> _fTimes;
        private List<DateTime> fTimes
        { get { return _fTimes != null ? _fTimes : _fTimes = toTimes(fCourses); } }

        private int _gapRating = -1;
        public int gapRating
        { get { return _gapRating != -1 ? _gapRating : _gapRating = calculateGapRating(); } }

        private int _lunchRating = -1;
        public int lunchRating
        { get { return _lunchRating != -1 ? _lunchRating : _lunchRating = calculateLunchRating(); } }

        private float _professorRating = -1;
        public float professorRating
        { get { return _professorRating != -1 ? _professorRating : _professorRating = calculateProfessorRating() * 20; } }

        private double _distanceRating = -1;
        public double distanceRating
        { get { return _distanceRating != -1 ? _distanceRating : _distanceRating = calculateDistanceRating(); } }

        private double _proximityRating = -1;
        public double proximityRating
        { get { return _proximityRating != -1 ? _proximityRating : _proximityRating = calculateProximityRating(LectureList); } }

        private double _Rating = -1;
        public double Rating
        { get { return _Rating != -1 ? _Rating : 
                    _Rating = Math.Round(gapRating + lunchRating + professorRating + distanceRating + proximityRating, 2); } }

        public List<Course> Courses = new List<Course>();*/

        private List<Course> mCourses
        { get { return sortCoursesByTime(processSchedule("M")); } }
        private List<Course> tCourses
        { get { return sortCoursesByTime(processSchedule("T")); } }
        private List<Course> wCourses
        { get { return sortCoursesByTime(processSchedule("W")); } }
        private List<Course> thCourses
        { get { return sortCoursesByTime(processSchedule("Th")); } }
        private List<Course> fCourses
        { get { return sortCoursesByTime(processSchedule("F")); } }

        private List<DateTime> mTimes
        { get { return toTimes(mCourses); } }
        private List<DateTime> tTimes
        { get { return toTimes(tCourses); } }
        private List<DateTime> wTimes
        { get { return toTimes(wCourses); } }
        private List<DateTime> thTimes
        { get { return toTimes(thCourses); } }
        private List<DateTime> fTimes
        { get { return toTimes(fCourses); } }

        public int gapRating
        { get { return calculateGapRating(); } }

        public double lunchRating
        { get { return calculateLunchRating() / 5; } }

        public float professorRating
        { get { return calculateProfessorRating() * 20; } }

        public double distanceRating
        { get { return calculateDistanceRating(); } }

        public double proximityRating
        { get { return calculateProximityRating(LectureList) * 5; } }

        public double Rating
        {
            get
            { return Math.Round(gapRating + lunchRating + professorRating + distanceRating + proximityRating, 2); }
        }

        public List<Course> Courses = new List<Course>();
    }
}
