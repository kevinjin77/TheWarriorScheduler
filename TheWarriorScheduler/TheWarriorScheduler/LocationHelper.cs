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
    public static class LocationHelper
    {
        static HttpClient client = new HttpClient();
        internal class Coordinates
        {
            internal double latitude;
            internal double longitude;
        }
        private static string uwApiKey = "a0fa5a0445627c840d18a3cf30d89995";
        private static string googleApiKey = "AIzaSyCcO39He0FpIJctRGX8O5xEq5mZntYKZLk";

        private static Coordinates getCoords(string building)
        {
            StringBuilder requestString = new StringBuilder("https://api.uwaterloo.ca/v2/buildings/");
            requestString.Append($"{building}.json?key={uwApiKey}");
            var response = GetContentAsync(requestString.ToString()).Result;
            var responseJSON = new JavaScriptSerializer().Deserialize<Building>(response);
            Coordinates coords = new Coordinates();
            coords.latitude = responseJSON.data.latitude;
            coords.longitude = responseJSON.data.longitude;
            return coords;
        }

        public static int distanceInSeconds(string building1, string building2) {
            string lat1 = getCoords(building1).latitude.ToString();
            string long1 = getCoords(building1).longitude.ToString();
            string lat2 = getCoords(building2).latitude.ToString();
            string long2 = getCoords(building2).longitude.ToString();
            StringBuilder requestString = new StringBuilder("https://maps.googleapis.com/maps/api/directions/json?");
            requestString.Append($"origin={lat1},{long1}&destination={lat2},{long2}&mode=walking&key={googleApiKey}");
            var response = GetContentAsync(requestString.ToString()).Result;
            var responseJSON = new JavaScriptSerializer().Deserialize<GoogleResponse>(response);
            return responseJSON.routes[0].legs[0].duration.value;
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
