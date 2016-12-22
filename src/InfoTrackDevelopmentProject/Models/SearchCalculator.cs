using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InfoTrackDevelopmentProject.Models
{
    public class SearchCalculator
    {
        //This function checks the data to see which entries have the required URL
        public static string Calculator(string urlToLookFor, string paramToSearch, string searchEngine)
        {
            Regex regex;
            string searchUrl;
            switch (searchEngine)
            {
                case "Google":
                    //Each entry in google is wrapped in a div <div class="g">...</div>. This regular expression is used to find these in the larger html file
                    regex = new Regex("<div class=\"g\">(.*?)</div>");
                    //The google search url that we need to use
                    searchUrl = "http://www.google.com/search?num=100&q=" + paramToSearch.Replace(" ", "+");
                    break;
                case "Bing":
                    //The entries in bing have class "b_algo"
                    regex = new Regex("<li class=\"b_algo\"(.*?)</li>");
                    searchUrl = "http://www.bing.com/search?count=100&q=" + paramToSearch.Replace(" ", "+");
                    break;
                default:
                    //defaults to the google case
                    regex = new Regex("<div class=\"g\">(.*?)</div>");
                    searchUrl = "http://www.google.com/search?num=100&q=" + paramToSearch.Replace(" ", "+");
                    break;
            }

            //The google search url that we need to use

            var html = GetData(searchUrl).Result;

            //Gets the entries from the data
            var matches = regex.Matches(html).Cast<Match>().ToList();

            Debug.WriteLine(matches.Count );
            //Creates a list of the entries where the url has a match
            var indexes = matches.Select((x, i) => new { i, x })
                .Where(x => x.ToString().Contains(urlToLookFor))
                .Select(x => x.i + 1)
                .ToList();

            //Returns 0 if there were no matches, else it returns a string of everywhere that matched
            return indexes.Count == 0 ? "0" : string.Join(", ", indexes.ToArray());
        }

        //This method gets the data from any search
        public static async Task<string> GetData(string searchUrl)
        {
            using (var httpClient = new HttpClient())
            using (var response = await httpClient.GetAsync(searchUrl))
            using (var content = response.Content)
            {
                var html = await content.ReadAsStringAsync();
                return html;
            }
        }
    }
}
