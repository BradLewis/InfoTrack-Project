using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InfoTrackDevelopmentProject.Models
{
    //This class handles reading and storing the data
    public class SearchRepository
    { 
        //Each search needs to have a unique integer id
        private static int _id = 1;

        //This keeps track of whether or not the json file has been read
        public static bool JsonRead = false;

        //A list to store ever search
        public static List<SearchItem> Searches = new List<SearchItem>();

        //Path to the json file that stores all the searches
        private static string filePath = "Data\\searches.json";

        //The json data in string format
        private static string _json = File.ReadAllText(filePath);

        //This method handles adding new searches
        public static void Add(SearchItem item)
        {
            //Items that do not have an idea are defaulted to having id = 0, this if statement checks that, and gives them a new id if needed
            if (item.Id == 0)
            {
                item.Id = _id;
                _id++;
            }
            //Adds the item to the list
            Searches.Add(item);
            //Updates the json file
            WriteJson();
        }

        //This method handles writing to the json file
        public static void WriteJson()
        {
            //Serialises the string into json
            var jsonData = JsonConvert.SerializeObject(Searches, Formatting.Indented);

            //Writes the data
            File.WriteAllText(filePath, jsonData);
        }

        //this method handles reading the json file
        public static void ReadJson()
        {
            //Reads the json string
            var searches = JToken.Parse(_json);

            //Gets all the necessary data for each entry and creates a new SearchItem object, adding the object to the list
            foreach (var cat in searches.Children())
            {
                var url = cat.SelectToken("Url").ToObject<string>();
                var searchQuery = cat.SelectToken("SearchQuery").ToObject<string>();
                var id = cat.SelectToken("Id").ToObject<int>();
                var result = cat.SelectToken("Result").ToObject<string>();
                var time = cat.SelectToken("Time").ToObject<DateTime>();
                Add(new SearchItem { Url = url, SearchQuery = searchQuery, Id = id, Result = result, Time = time});
                if (id >= _id)
                {
                    //updates the next unqiue id.
                    _id = id + 1;
                }
            }
        }
    }
}
