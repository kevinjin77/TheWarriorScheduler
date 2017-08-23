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
            if (index < weekdays.Length && weekdays[index] == 'T' && weekdays[index + 1] != 'h')
            {
                dayList.Add("T");
                index++;
            }
            if (index < weekdays.Length && weekdays[index] == 'W')
            {
                dayList.Add("W");
                index++;
            }
            if ((index + 1) < weekdays.Length && weekdays[index] == 'T' && weekdays[index + 1] == 'h')
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
    }
}
