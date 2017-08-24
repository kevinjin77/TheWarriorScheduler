using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWarriorScheduler
{
    class ScheduleHelper
    {
        private List<string> processDate(string weekdays)
        {
            int index = 0;
            List<string> dayList = new List<string>();
            if (index < weekdays.Length && weekdays[index] == 'M')
            {
                dayList.Add("M");
                index++;
            }
            if (index < weekdays.Length && weekdays[index] == 'T' && (index + 1 == weekdays.Length ? true : weekdays[index + 1] != 'h'))
            {
                dayList.Add("T");
                index++;
            }
            if (index < weekdays.Length && weekdays[index] == 'W')
            {
                dayList.Add("W");
                index++;
            }
            if ((index + 1) < weekdays.Length && weekdays[index] == 'T' && (index + 1 == weekdays.Length ? true : weekdays[index + 1] != 'h'))
            {
                dayList.Add("Th");
                index += 2;
            }
            if (index < weekdays.Length && weekdays[index] == 'F')
            {
                dayList.Add("F");
                index++;
            }

            return dayList;
        }

        private bool isTimeConflict(string start1, string end1, string start2, string end2)
        {
            DateTime startTime1 = DateTime.ParseExact(start1, "HH:mm", null);
            DateTime endTime1 = DateTime.ParseExact(end1, "HH:mm", null);
            DateTime startTime2 = DateTime.ParseExact(start2, "HH:mm", null);
            DateTime endTime2 = DateTime.ParseExact(end2, "HH:mm", null);

            if ((DateTime.Compare(startTime1, endTime2) <= 0) && (DateTime.Compare(endTime1, startTime2) >= 0))
            {
                return true;
            }
            return false;
        }

        public bool isConflict(Course c1, Course c2)
        {
            if (c1.type != "LEC" || c2.type != "LEC")
            {
                return true;
            }
            Date c1Date = c1.classes[0].date;
            Date c2Date = c2.classes[0].date;
            List<string> c1List = processDate(c1Date.weekdays);
            List<string> c2List = processDate(c2Date.weekdays);

            foreach(string day in c1List)
            {
                if (c2List.Contains(day) && isTimeConflict(c1Date.start_time, c1Date.end_time, c2Date.start_time, c2Date.end_time))
                {
                    return true;
                }
            }
            return false;
        }

        public bool isScheduleValid(Schedule s1)
        {
            for (int i = 0; i < s1.Courses.Count - 1; ++i)
            {
                for (int j = i + 1; j < s1.Courses.Count; ++j)
                {
                    if(isConflict(s1.Courses[i], s1.Courses[j]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public List<Schedule> generateSchedules(List<CourseList> responseList)
        {
            List<int> sizes = new List<int>();
            foreach (CourseList cList in responseList)
            {
                sizes.Add(cList.data.Count);
            }
            int[][] initArray = new int[sizes.Count][];
            for (int i = 0; i < sizes.Count; ++i)
            {
                initArray[i] = (Enumerable.Range(0, sizes[i] - 1).ToArray());
            }

            var cross = new CartesianProduct<int>(initArray);
            List<Schedule> result = new List<Schedule>();
            foreach (var item in cross.Get())
            {
                Schedule mySchedule = new Schedule();
                //List<int> myList = new List<int>();
                int count = 0;
                foreach (int num in item)
                {
                    mySchedule.Courses.Add(responseList[count].data[num]);
                    ++count;
                    //myList.Add(num);
                }
                if (isScheduleValid(mySchedule))
                {
                    mySchedule.printSchedule();
                    result.Add(mySchedule);
                }
            }
            return result;
        }
    }
}
