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
            foreach (BuildingObj b in buildingsCache)
            {
                if (b.name == building)
                {
                    return b.coords;
                }
            }

            string oldBuilding = building;

            while (true)
            {
                try
                {
                    StringBuilder requestString = new StringBuilder("https://api.uwaterloo.ca/v2/buildings/");
                    requestString.Append($"{building}.json?key={uwApiKey}");
                    var response = GetContentAsync(requestString.ToString()).Result;
                    var responseJSON = new JavaScriptSerializer().Deserialize<Building>(response);
                    Coordinates coords = new Coordinates();
                    coords.latitude = responseJSON.data.latitude;
                    coords.longitude = responseJSON.data.longitude;

                    BuildingObj myBuilding = new BuildingObj();
                    myBuilding.name = oldBuilding;
                    myBuilding.coords = coords;
                    buildingsCache.Add(myBuilding);

                    return coords;
                }
                catch (InvalidOperationException e)
                {
                    if (building.Contains("SJ"))
                    {
                        oldBuilding = building;
                        building = "STJ";       // Handle St. Jerome's
                    }
                    else
                    {
                        building = "MC";        // Default to MC
                    }
                }
            }     
        }

        public static int distanceInSeconds(string building1, string building2) {
            foreach (Directions d in directionsCache)
            {
                if ((d.building1 == building1 && d.building2 == building2) || (d.building1 == building2 && d.building2 == building1))
                {
                    return d.seconds;
                }
            }

            string lat1 = getCoords(building1).latitude.ToString();
            string long1 = getCoords(building1).longitude.ToString();
            string lat2 = getCoords(building2).latitude.ToString();
            string long2 = getCoords(building2).longitude.ToString();
            StringBuilder requestString = new StringBuilder("https://maps.googleapis.com/maps/api/directions/json?");
            requestString.Append($"origin={lat1},{long1}&destination={lat2},{long2}&mode=walking&key={googleApiKey}");
            var response = GetContentAsync(requestString.ToString()).Result;
            var responseJSON = new JavaScriptSerializer().Deserialize<GoogleResponse>(response);

            Directions myDirections = new Directions();
            myDirections.building1 = building1;
            myDirections.building2 = building2;
            myDirections.seconds = responseJSON.routes[0].legs[0].duration.value;
            directionsCache.Add(myDirections);

            return myDirections.seconds;
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

        public class BuildingObj
        {
            public string name;
            internal Coordinates coords;
        }

        public class Directions
        {
            public string building1;
            public string building2;
            public int seconds;
        }

        public static List<BuildingObj> buildingsCache = new List<BuildingObj>();
        public static List<Directions> directionsCache = new List<Directions>();
    }
}
