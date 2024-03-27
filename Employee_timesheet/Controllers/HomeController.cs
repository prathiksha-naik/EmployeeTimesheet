using Employee_timesheet.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;


namespace Employee_timesheet.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
       

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;         
        }

        [Authorize]
        public IActionResult Index()
        {        
            return View();        
        }      

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }
        public IActionResult ManagerProfile()
        {
            return View();
        }
        public IActionResult Calender()
        {
            return View();
        }

        public IActionResult GetCalendarEvents()

        {

            var events = new List<CalendarEvent>

            {

                new CalendarEvent { Title = "New Year", Start = new DateTime(DateTime.Now.Year, 1, 1).ToString("yyyy-MM-dd"), Description = "New Year  - New Year celebrations " },

                new CalendarEvent { Title = "Republic Day", Start = new DateTime(DateTime.Now.Year, 1, 26).ToString("yyyy-MM-dd"), Description = "Republic Day - Republic Day of India Celebrated every year " },

                new CalendarEvent { Title = "May Day ", Start = new DateTime(DateTime.Now.Year, 5, 1).ToString("yyyy-MM-dd"), Description = "May Day" },

                new CalendarEvent { Title = "Bakrid/ Eid al Adha", Start = new DateTime(DateTime.Now.Year, 6, 29).ToString("yyyy-MM-dd"), Description = "Bakrid/ Eid al Adha" },

                new CalendarEvent { Title = "Bakrid/ Eid al Adha", Start = new DateTime(DateTime.Now.Year, 6, 29).ToString("yyyy-MM-dd"), Description = "Bakrid/ Eid al Adha" },

                new CalendarEvent { Title = "Independence Day", Start = new DateTime(DateTime.Now.Year, 8, 15).ToString("yyyy-MM-dd"), Description = "Independence Day" },

                new CalendarEvent { Title = "Ganesh Chaturthi", Start = new DateTime(DateTime.Now.Year, 9, 19).ToString("yyyy-MM-dd"), Description = "Ganesh Chaturthi" },

                new CalendarEvent { Title = "Gandhi Jayanthi", Start = new DateTime(DateTime.Now.Year, 10, 2).ToString("yyyy-MM-dd"), Description = "Gandhi Jayanthi - Birth Anniversary of Mahatma Gandhi" },

                new CalendarEvent { Title = "Vijaya Dashmi", Start = new DateTime(DateTime.Now.Year, 10, 24).ToString("yyyy-MM-dd"), Description = "Vijaya Dashmi" },

                new CalendarEvent { Title = "Karnataka Rajyotsava", Start = new DateTime(DateTime.Now.Year, 11, 1).ToString("yyyy-MM-dd"), Description = "Karnataka Rajyotsava" },

                new CalendarEvent { Title = "Christmas Day", Start = new DateTime(DateTime.Now.Year, 12, 25).ToString("yyyy-MM-dd"), Description = "Christmas Day" },

               // Add other events here

            };



            return Json(events);

        }

        public class CalendarEvent

        {

            public string Title { get; set; }

            public string Start { get; set; }

            public string Description { get; set; }

        }
    

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }






        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}

        [Route("Home/Error")]
        public IActionResult Error()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            ViewBag.ExceptionPath = exceptionDetails.Path;
            ViewBag.ExceptionMessage = exceptionDetails.Error.Message;
            ViewBag.Stacktrace = exceptionDetails.Error.StackTrace;

            if (exceptionDetails.Error is DbUpdateException)
            {
                ViewBag.ExceptionMessage = "A database error occurred. Please contact support for assistance.";
            }
            else
            {
                ViewBag.ExceptionMessage = "An error occurred while processing your request. ";
            }

            _logger.LogError(exceptionDetails.Error, "An error occurred at {ExceptionPath},stack trace{StackTrace}", exceptionDetails.Path, exceptionDetails.Error.StackTrace);
            return View("Error");
        }

    }
}