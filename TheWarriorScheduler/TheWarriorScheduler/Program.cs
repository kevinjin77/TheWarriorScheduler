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
//   - Professor Rating (Need a way to get ratings off of RateMyProf)
//   - Distance Rating (Based on distance between buildings using API to get lat/longitude and then using Google Maps)
//   - Gap Rating (Based on number of small gaps)
//   - Lunch Rating (Based on time allocated for lunch)
//   - Early Bird/Night Owl (No 8:30 Classes, if possible)
//   - Proximity Rating (Least number of changes from current schedule, if given one)
//   - Scale Individual Ratings out of 100, scale overall ratings based on importance (Professor Rating > Gap Rating)

//   - Include TUT, TST in Schedule
//   - Accomodate ENG Classes (One Lecture, Multiple Classes)
//   - Show Open Classes Only (Or Red Dot if class if full, eventually take reserves into account"


namespace TheWarriorScheduler
{
    class Program
    {
        static HttpClient client = new HttpClient();

        static void Main(string[] args)
        {
            int test = LocationHelper.distanceInSeconds("AL", "MC");
            string uwApiKey = "a0fa5a0445627c840d18a3cf30d89995";
            string googleApiKey = "AIzaSyCcO39He0FpIJctRGX8O5xEq5mZntYKZLk";
            string term = "1179";
            List<CourseList> responseList = new List<CourseList>();

            Console.WriteLine("How many courses are you taking this term?");
            int numCourses = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Are you OK with 8:30 classes? (Y/N)");
            string earlyBird = Console.ReadLine().ToUpper();
            bool earlyBirdFilter = (earlyBird == "Y") ? false : true;

            Console.WriteLine("Are you OK with night classes? (Y/N)");
            string night = Console.ReadLine().ToUpper();
            bool nightFilter = (night == "Y") ? false : true;

            Console.WriteLine($"\nPlease enter the names of your {numCourses} courses:");
            Console.WriteLine("Each course should have the format of Subject ClassNumber. For example, \"CS 245\".");
            for (int i = 0; i < numCourses; ++i)
            {
                try
                {
                    var arguments = Console.ReadLine().Split(' ');
                    StringBuilder requestString = new StringBuilder("https://api.uwaterloo.ca/v2/terms/");
                    requestString.Append($"{term}/{arguments[0]}/{arguments[1]}/schedule.json?key={uwApiKey}");
                    var data = GetContentAsync(requestString.ToString()).Result;
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
                    Console.WriteLine(e);
                    Console.WriteLine($"\nPlease enter a course with the valid format:");
                    i--;
                }
            }

            List<Schedule> resulter = new List<Schedule>();
            resulter = ScheduleHelper.generateSchedules(responseList, earlyBirdFilter, nightFilter);
            for (int i = 0; i < resulter.Count; ++i)
            {
                resulter[i].printSchedule();
            }
            ScheduleStats stats = resulter[0].ComputeStats();
            Console.ReadLine();
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
    }
}