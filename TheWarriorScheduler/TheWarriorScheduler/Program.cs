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
            string api_key = "a0fa5a0445627c840d18a3cf30d89995";
            //Console.ReadLine
            RunAsync().Wait();
        }

        static async Task RunAsync()
        {
            var data = await GetContentAsync("https://api.uwaterloo.ca/v2/courses/CS/245/schedule.json?term=1171&key=a0fa5a0445627c840d18a3cf30d89995");
            List<Course> Courselist;
            var example2Model = new JavaScriptSerializer().Deserialize<CourseList>(data);
            Console.WriteLine(data);
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