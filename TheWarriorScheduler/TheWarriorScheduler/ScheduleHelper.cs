using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWarriorScheduler
{
    public class ScheduleHelper
    {
        public List<string> processDate(string weekdays)
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
            if ((index + 1) < weekdays.Length && weekdays[index] == 'T' && (index + 1 == weekdays.Length ? true : weekdays[index + 1] == 'h'))
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

        private bool isTimeConflict(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        {
            if ((DateTime.Compare(start1, end2) <= 0) && (DateTime.Compare(end1, start2) >= 0))
            {
                return true;
            }
            return false;
        }

        private bool isConflict(Course c1, Course c2)
        {
            if (c1.type != "LEC" || c2.type != "LEC")
            {
                return true;
            }

            foreach(string day in c1.weekdays)
            {
                if (c2.weekdays.Contains(day) && isTimeConflict(c1.start_time, c1.end_time, c2.start_time, c2.end_time))
                {
                    return true;
                }
            }
            return false;
        }

        private bool isScheduleValid(Schedule s1)
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
