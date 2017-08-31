using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWarriorScheduler
{
    public static class ScheduleHelper
    {
        public static List<string> processDate(string weekdays)
        {
            int index = 0;
            List<string> dayList = new List<string>();
            if (weekdays == null)
            {
                return dayList;
            }
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

        private static bool isTimeConflict(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        {
            if ((DateTime.Compare(start1, end2) <= 0) && (DateTime.Compare(end1, start2) >= 0))
            {
                return true;
            }
            return false;
        }

        private static bool isConflict(Course c1, Course c2)
        {
            if (c1.type != "LEC" || c2.type != "LEC")
            {
                return true;
            }

            foreach (string day in c1.weekdays)
            {
                if (c2.weekdays.Contains(day) && isTimeConflict(c1.start_time, c1.end_time, c2.start_time, c2.end_time))
                {
                    return true;
                }
            }
            return false;
        }

        private static int dateToInt(DateTime date)
        {
            return date.Hour * 60 + date.Minute;
        }

        private static bool isScheduleValid(Schedule s1, bool earlyBirdFilter, bool nightFilter)
        {
            for (int i = 0; i < s1.Courses.Count - 1; ++i)
            {
                for (int j = i + 1; j < s1.Courses.Count; ++j)
                {
                    if (isConflict(s1.Courses[i], s1.Courses[j]))
                    {
                        return false;
                    }
                }
            }

            if (earlyBirdFilter)
            {
                foreach (Course c in s1.Courses)
                {
                    if (dateToInt(c.start_time) <= 510)
                    {
                        return false;
                    }
                }
            }

            if (nightFilter)
            {
                foreach (Course c in s1.Courses)
                {
                    if (dateToInt(c.start_time) >= 1080)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static List<Schedule> sortSchedules(List<Schedule> scheduleList)
        {
            List<Schedule> newList = scheduleList.OrderByDescending(x => x.Rating).ToList();
            return newList;
        }

        public static List<Schedule> generateSchedules(List<CourseList> responseList, bool earlyBirdFilter, bool nightFilter, List<string> lectureList)
        {
            int numLec, numTut, numLab, numTst, lecIndex, tutIndex, labIndex, tstIndex;
            numLec = numTut = numLab = numTst = 0;
            List<int> sizes = new List<int>();
            foreach (CourseList cList in responseList)
            {
                foreach (Course c in cList.data)
                {
                    if (c.type == "LEC")
                    {
                        ++numLec;
                    }
                    else if (c.type == "TUT")
                    {
                        ++numTut;
                    }
                    else if (c.type == "LAB")
                    {
                        ++numLab;
                    }
                    else if (c.type == "TST")
                    {
                        ++numTst;
                    }
                }
                lecIndex = 0;
                tutIndex = numLec;
                labIndex = tutIndex + numTut;
                tstIndex = labIndex + numLab;
                /*if (numLec > 0)
                {
                    sizes.Add(numLec);
                }
                if (numTut > 0)
                {
                    sizes.Add(numTut);
                }
                if (numLab > 0)
                {
                    sizes.Add(numLab);
                }
                if (numTst > 0)
                {
                    sizes.Add(numTst);
                }*/
                sizes.Add(cList.data.Count);
            }

            List<List<int>> initList = new List<List<int>>();
            for (int i = 0; i < sizes.Count; ++i)
            {
                if (sizes[i] - 1 == 0)
                {
                    initList.Add(new List<int> { 0 });
                }
                else
                {
                    initList.Add(Enumerable.Range(0, sizes[i] - 1).ToList());
                }
            }

            IEnumerable<IEnumerable<int>> cross = GetPermutations(initList);
            List<Schedule> result = new List<Schedule>();
            foreach (var item in cross)
            {
                Schedule mySchedule = new Schedule(lectureList);
                int count = 0;
                foreach (int num in item)
                {
                    mySchedule.Courses.Add(responseList[count].data[num]);
                    ++count;
                }
                if (isScheduleValid(mySchedule, earlyBirdFilter, nightFilter))
                {
                    result.Add(mySchedule);
                }
            }
            result = sortSchedules(result);
            return result;
        }

        private static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<IEnumerable<T>> listOfLists)
        {
            return listOfLists.Skip(1)
                .Aggregate(listOfLists.First()
                        .Select(c => new List<T>() { c }),
                    (previous, next) => previous
                        .SelectMany(p => next.Select(d => new List<T>(p) { d })));
        }
    }
}

/*public static List<Schedule> generateSchedules(List<CourseList> responseList, bool earlyBirdFilter, bool nightFilter, List<string> lectureList)
{
    int numLec, numTut, numLab, numTst, lecIndex, tutIndex, labIndex, tstIndex;
    List<int[]> initList = new List<int[]>();
    List<int> courseDivisions = new List<int>();
    List<int> sizes = new List<int>();
    foreach (CourseList cList in responseList)
    {
        numLec = numTut = numLab = numTst = 0;
        int divisions = 0;
        foreach (Course c in cList.data)
        {
            if (c.type == "LEC")
            {
                ++numLec;
            }
            else if (c.type == "TUT")
            {
                ++numTut;
            }
            else if (c.type == "LAB")
            {
                ++numLab;
            }
        }
        lecIndex = 0;
        tutIndex = numLec;
        labIndex = tutIndex + numTut;
        if (numLec > 0)
        {
            divisions++;
            sizes.Add(numLec);
            if (numLec == 1)
            {
                initList.Add(new int[] { lecIndex });
            }
            else
            {
                initList.Add(Enumerable.Range(lecIndex, numLec).ToArray());
            }
        }
        if (numTut > 0)
        {
            divisions++;
            sizes.Add(numTut);
            if (numTut == 1)
            {
                initList.Add(new int[] { tutIndex });
            }
            else
            {
                initList.Add(Enumerable.Range(tutIndex, numTut).ToArray());
            }
        }
        if (numLab > 0)
        {
            divisions++;
            sizes.Add(numLab);
            if (numLab == 1)
            {
                initList.Add(new int[] { labIndex });
            }
            else
            {
                initList.Add(Enumerable.Range(labIndex, numLab).ToArray());
            }
        }

        courseDivisions.Add(divisions);
        sizes.Add(cList.data.Count);
    }

    int[][] initArray = new int[sizes.Count][];
    for (int i = 0; i < sizes.Count; ++i)
    {
        if (sizes[i] - 1 == 0)
        {
            initArray[i] = new int[] { 0 };
        }
        else
        {
            initArray[i] = (Enumerable.Range(0, sizes[i] - 1).ToArray());
        }
    }

    var cross = new CartesianProduct<int>(initList.ToArray());
    List<Schedule> result = new List<Schedule>();
    foreach (var item in cross.Get())
    {
        Schedule mySchedule = new Schedule(lectureList);
        int count = 0;
        int courseCount = 0;
        foreach (int num in item)
        {
            mySchedule.Courses.Add(responseList[count].data[num]);
            ++courseCount;
            if (courseCount == courseDivisions[count])
            {
                ++count;
            }
        }
        if (isScheduleValid(mySchedule, earlyBirdFilter, nightFilter))
        {
            result.Add(mySchedule);
        }
    }
    result = sortSchedules(result);
    return result;
}*/