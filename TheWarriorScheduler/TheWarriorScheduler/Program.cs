using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace TheWarriorScheduler
{
    class Program
    {
        static HttpClient client = new HttpClient();

        static void Main(string[] args)
        {
            //Console.ReadLine
            RunAsync().Wait();
        }

        static async Task RunAsync()
        {
            string apiKey = "a0fa5a0445627c840d18a3cf30d89995";
            string term = "1179";
            List<Response> reponseList = new List<Response>();
            Console.WriteLine("How many courses are you taking this term?");
            int numCourses = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine($"\nPlease enter the names of your {numCourses} courses:");
            Console.WriteLine("Each course should have the format of Subject ClassNumber. For example, \"CS 245\".");
            for (int i = 0; i < numCourses; ++i)
            {
                //courseList.Add(Console.ReadLine());
                var arguments = Console.ReadLine().Split(' ');
                StringBuilder requestString = new StringBuilder("https://api.uwaterloo.ca/v2/courses/");
                requestString.Append($"{arguments[0]}/{arguments[1]}/schedule.json?term={term}&key={apiKey}");
                var data = await GetContentAsync(requestString.ToString());
                var dataJSON = new JavaScriptSerializer().Deserialize<Response>(data);
                if (dataJSON.data.Count == 0)
                {
                    Console.WriteLine($"{arguments[0]} {arguments[1]} does not exist/is not being offered this term! Please enter a valid course.\n");
                    i--;
                } else
                {
                    reponseList.Add(dataJSON);
                }
            }

            //var data = await GetContentAsync("https://api.uwaterloo.ca/v2/courses/CS/245/schedule.json?term=1171&key=a0fa5a0445627c840d18a3cf30d89995");
            //var test = new JavaScriptSerializer().Deserialize<Response>(data);
            //Console.WriteLine(test.data[0].subject);
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