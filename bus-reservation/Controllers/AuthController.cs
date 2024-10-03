using bus_reservation.Models;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Security.Claims;
using System.Threading.Tasks;

namespace bus_reservation.Controllers
{
    public class AuthController : Controller
    {
        private readonly BusReservationContext db;

        public AuthController(BusReservationContext _db)
        {
            db = _db;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string Email, string Password)
        {

            var checkUser = db.Employees.FirstOrDefault(a => a.EmployeeEmail == Email);
            var checkadmin = db.Admins.FirstOrDefault(a => a.Email == Email);

            if (checkUser != null)
            {
                var hasher = new PasswordHasher<string>();
                var verifyPass = hasher.VerifyHashedPassword(Email, checkUser.Password, Password);

                if (verifyPass == PasswordVerificationResult.Success)
                {
                    var identity = new ClaimsIdentity(new[]
                    {
                new Claim(ClaimTypes.Role, "Employee") // Replace "User" with dynamic role if you have roles
            },
                    CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    HttpContext.Session.SetInt32("UserID", checkUser.EmployeeId);
                    HttpContext.Session.SetString("UserEmail", checkUser.EmployeeEmail);

                    return RedirectToAction("Index", "Employee"); // Redirect to the appropriate controller/action
                }
                else
                {
                    ViewBag.msg = "Invalid Credentials";
                    return View();
                }
            }
            else if (checkadmin != null)
            {
                //var hasher = new PasswordHasher<string>();
                //var verifyPass = hasher.VerifyHashedPassword(Email, checkadmin.Password, Password);

                if (checkadmin.Password == Password)
                {
                    var identity = new ClaimsIdentity(new[]
                    {
                new Claim(ClaimTypes.Role, "Admin") // Replace "User" with dynamic role if you have roles
            }, CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    HttpContext.Session.SetInt32("UserID", checkadmin.Id);
                    HttpContext.Session.SetString("UserEmail", checkadmin.Email);

                    return RedirectToAction("Index", "Admin"); // Redirect to the appropriate controller/action
                }
                else
                {
                    ViewBag.msg = "Invalid Credentials";
                    return View();
                }
            }
            else
            {
                ViewBag.msg = "Invalid User";
                return View();
            }
        }

    }
}