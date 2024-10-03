using bus_reservation.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Diagnostics;

namespace bus_reservation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BusReservationContext _context; 

       
        public HomeController(ILogger<HomeController> logger, BusReservationContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Faqs()
        {
            return View();
        }

        public IActionResult Signup()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public IActionResult SubmitEnquiry(Enquiry enquiry)
        {
            
                _context.Enquiries.Add(enquiry);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Your response has been recorded.";
                return RedirectToAction("Contact"); 
            

        }



        public IActionResult EnquirySuccess()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
