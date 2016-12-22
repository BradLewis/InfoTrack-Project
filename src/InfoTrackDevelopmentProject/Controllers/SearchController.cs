using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InfoTrackDevelopmentProject.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace InfoTrackDevelopmentProject.Controllers
{
    public class SearchController : Controller
    {

        public SearchController()
        {
            //On first load, this reads the json file.
            if (SearchRepository.JsonRead) return;
            SearchRepository.ReadJson();
            SearchRepository.JsonRead = true;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        //This checks the inputs and performs the search etc
        public ActionResult FindSearch(SearchItem obj, string search)
        {
            if (obj.SearchQuery == null || obj.Url == null)
            {
                //If one of the boxes is empty then we don't perform the search
                ViewBag.Message = "Please enter both a query to search and a URL to search for.";
            }
            else
            {
                try
                {
                    //Check the url in lowercase
                    obj.Url = obj.Url.ToLower();
                    //Performs the search and gets the positions of the links that match
                    var result = SearchCalculator.Calculator(obj.Url, obj.SearchQuery, obj.SearchEngine);
                    //Creates new SearchItem
                    var item = new SearchItem
                    {
                        Url = obj.Url,
                        SearchQuery = obj.SearchQuery,
                        SearchEngine = obj.SearchEngine,
                        Id = 0,
                        Result = result,
                        Time = DateTime.Now
                    };
                    SearchRepository.Add(item);
                    //Passes the result to the view
                    ViewBag.Message = string.Format("From searching \"{0}\", we found the url \"{1}\" at results {2} ",
                        obj.SearchQuery, obj.Url, result);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    ViewBag.Messgae = "Something went wrong, please try again!";
                }
            }

            return View("Index");
        }
        
    }
}
