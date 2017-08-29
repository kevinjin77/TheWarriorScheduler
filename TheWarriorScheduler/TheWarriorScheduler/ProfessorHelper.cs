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
            Rating myRating = new Rating();
            if (name == "")
            {
                myRating.professor = name;
                myRating.rating = 0;
                ratingsCache.Add(myRating);
                return 0;
            }

            foreach (Rating r in ratingsCache)
            {
                if (r.professor == name)
                {
                    return r.rating;
                }
            }

            string[] parsedName = parseName(name);
            using (WebClient wc = new WebClient())
            {
                int tries = 5;
                while (tries >= 0)
                {
                    try
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
                                myRating.professor = name;
                                myRating.rating = 0;
                                ratingsCache.Add(myRating);
                                return 0;
                            }
                            foreach (Professor p in responseJSON.professors)
                            {
                                if (p.tFname.StartsWith(parsedName[1][0].ToString()))
                                {
                                    myRating.professor = name;
                                    myRating.rating = float.Parse(p.overall_rating, CultureInfo.InvariantCulture.NumberFormat);
                                    ratingsCache.Add(myRating);
                                    return myRating.rating;
                                }
                            }
                            myRating.professor = name;
                            myRating.rating = 0;
                            ratingsCache.Add(myRating);
                            return 0;
                        }
                        myRating.professor = name;
                        try
                        {
                            myRating.rating = float.Parse(responseJSON.professors[0].overall_rating, CultureInfo.InvariantCulture.NumberFormat);
                        }
                        catch (Exception e)
                        {
                            myRating.rating = 0;
                        }

                        ratingsCache.Add(myRating);
                        return myRating.rating;
                    }
                    catch (ArgumentException e)
                    {
                        --tries;
                    }
                    catch (Exception e)
                    {
                        return 0;
                    }
                }
                return 0;
            }
        }

        public class Rating
        {
            public string professor;
            public float rating;
        }

        public static List<Rating> ratingsCache = new List<Rating>();
    }
}
