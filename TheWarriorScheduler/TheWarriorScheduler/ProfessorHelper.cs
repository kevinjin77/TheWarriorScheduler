using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace TheWarriorScheduler
{
    public static class ProfessorHelper
    {
        private static string[] parseName(string name)
        {
            string[] values = name.Split(',');
            if (values[1].Contains(' '))
            {
                string[] firstName = values[1].Split(' ');
                values[1] = firstName[0];
            }
            return values;
        }

        public static float getRating(string name)
        {
            //ProfRating myRating = new ProfRating();
            if (name == "")
            {
                return 0;
            }
            string[] parsedName = parseName(name);
            using (WebClient wc = new WebClient())
            {
                string request = "http://www.ratemyprofessors.com/find/professor/?&page=1&sid=1490&queryoption=TEACHER&queryBy=teacherName" + $"&query={parsedName[1]}+{parsedName[0]}";
                var response = wc.DownloadString(request);
                var responseJSON = new JavaScriptSerializer().Deserialize<ProfessorResponse>(response);
                if (responseJSON.professors.Count == 0)
                {
                    request = "http://www.ratemyprofessors.com/find/professor/?&page=1&sid=1490&queryoption=TEACHER&queryBy=teacherName" + $"&query={parsedName[0]}";
                    response = wc.DownloadString(request);
                    responseJSON = new JavaScriptSerializer().Deserialize<ProfessorResponse>(response);
                    if (responseJSON.professors.Count == 0)
                    {
                        return 0;
                    }
                    foreach (Professor p in responseJSON.professors)
                    {
                        if (p.tFname.StartsWith(parsedName[1][0].ToString()))
                        {
                            return float.Parse(p.overall_rating, CultureInfo.InvariantCulture.NumberFormat);
                        } 
                    }
                    return 0;
                }
                //myRating.rating = float.Parse(responseJSON.professors[0].overall_rating, CultureInfo.InvariantCulture.NumberFormat);
                //myRating.num_students = responseJSON.professors[0].tNumRatings;
                //return myRating;
                return float.Parse(responseJSON.professors[0].overall_rating, CultureInfo.InvariantCulture.NumberFormat);
            }          
        }  
    }

    //public class ProfRating
    //{
    //    public float rating = 0;
    //    public int num_students = 0;
    //}
}
