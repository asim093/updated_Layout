using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bus_reservation.Controllers
{
    		[Authorize(Roles = "Employee")]

    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
