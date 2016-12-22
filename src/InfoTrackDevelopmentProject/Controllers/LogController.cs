using InfoTrackDevelopmentProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace InfoTrackDevelopmentProject.Controllers
{
    public class LogController : Controller
    {
        // GET: /<controller>/
        //This handles opening the log
        public ViewResult LogView()
        {
            //gives the list of searches to the view
            return View(SearchRepository.Searches);
        }
    }
}
