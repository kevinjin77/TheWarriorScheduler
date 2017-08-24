﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

// TODO:
// Add Rating System for generated schedule:
//   - Professor Rating (RateMyProf)
//   - Distance Rating (Based on distance between buildings)
//   - Gap Rating (Based on number of small gaps)
//   - Lunch Rating (Based on time allocated for lunch)
//   - Early Bird/Night Owl


namespace TheWarriorScheduler
{
    class Program
    {
        static HttpClient client = new HttpClient();

        static void Main(string[] args)
        {
            RunAsync().Wait();
        }

        static async Task RunAsync()
        {
            string apiKey = "a0fa5a0445627c840d18a3cf30d89995";
            string term = "1179";
            List<CourseList> responseList = new List<CourseList>();
            Console.WriteLine("How many courses are you taking this term?");
            int numCourses = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine($"\nPlease enter the names of your {numCourses} courses:");
            Console.WriteLine("Each course should have the format of Subject ClassNumber. For example, \"CS 245\".");
            for (int i = 0; i < numCourses; ++i)
            {
                try
                {
                    var arguments = Console.ReadLine().Split(' ');
                    StringBuilder requestString = new StringBuilder("https://api.uwaterloo.ca/v2/terms/");
                    requestString.Append($"{term}/{arguments[0]}/{arguments[1]}/schedule.json?key={apiKey}");
                    var data = await GetContentAsync(requestString.ToString());
                    var dataJSON = new JavaScriptSerializer().Deserialize<CourseList>(data);
                    if (dataJSON.data.Count == 0)
                    {
                        Console.WriteLine($"\n{arguments[0]} {arguments[1]} does not exist/is not being offered this term! Please enter a valid course:");
                        i--;
                    }
                    else
                    {
                        responseList.Add(dataJSON);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"\nPlease enter a course with the valid format:");
                    i--;
                }
            }

            ScheduleHelper schedules = new ScheduleHelper();
            List<Schedule> resulter = new List<Schedule>();
            resulter = schedules.generateSchedules(responseList);
            bool test = schedules.isConflict(responseList[0].data[1], responseList[0].data[2]);

            //List<int> myList = new List<int>(new int[] { 3, 4, 9, 12 });
            //List<List<int>> result = schedules.generateCombinations(myList);

            //ScheduleHelper schedules = responseList.generateAllSchedules();
            //PrintSchedules(schedules);

            Console.WriteLine(responseList[0].data[0].type);
        }

        static async Task<string> GetContentAsync(string url)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string product = null;
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsStringAsync();
            }
            return product;
        }

        //public static List<List<int>> GetSubsetsOfSizeK(List<int> lInputSet, int k)
        //{
        //    List<List<int>> lSubsets = new List<List<int>>();
        //    GetSubsetsOfSizeK_rec(lInputSet, k, 0, new List<int>(), lSubsets);
        //    return lSubsets;
        //}

        //public static void GetSubsetsOfSizeK_rec(List<int> lInputSet, int k, int i, List<int> lCurrSet, List<List<int>> lSubsets)
        //{
        //    if (lCurrSet.Count == k)
        //    {
        //        lSubsets.Add(lCurrSet);
        //        return;
        //    }

        //    if (i >= lInputSet.Count)
        //        return;

        //    List<int> lWith = new List<int>(lCurrSet);
        //    List<int> lWithout = new List<int>(lCurrSet);
        //    lWith.Add(lInputSet[i++]);

        //    GetSubsetsOfSizeK_rec(lInputSet, k, i, lWith, lSubsets);
        //    GetSubsetsOfSizeK_rec(lInputSet, k, i, lWithout, lSubsets);
        //}
    }
}