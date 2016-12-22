using System;

namespace InfoTrackDevelopmentProject.Models
{
    //class for the data stored each time we search for an item.
    public class SearchItem
    {
        public string Url { get; set; }
        public string SearchQuery { get; set; }
        public int Id { get; set; }
        public string Result { get; set; }
        public DateTime Time { get; set; }
        public string SearchEngine { get; set; }
    }

    public enum SearchEngines
    {
        Google = 0,
        Bing = 1
    }
}
