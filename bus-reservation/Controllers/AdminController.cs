using Microsoft.AspNetCore.Mvc;
using bus_reservation.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace bus_reservation.Controllers
{
		[Authorize(Roles = "Admin")]
	public class AdminController : Controller
	{
		private readonly BusReservationContext _context;

		public AdminController(BusReservationContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			return View();
		}

        [HttpGet]
        public IActionResult AddEmployee()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEmployee(Employee employee)
        {
            if (ModelState.IsValid)
            {
                var hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<Employee>();
                employee.Password = hasher.HashPassword(employee, employee.Password);

                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(EmployeeDetail));
            }
            return View(employee);
        }

        [HttpGet]
		public async Task<IActionResult> EditEmployee(int id)
		{
			var employee = await _context.Employees.FindAsync(id);
			if (employee == null)
			{
				return NotFound();
			}
			return View(employee);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditEmployee(int id, Employee employee)
		{
			if (id != employee.EmployeeId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(employee);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!EmployeeExists(employee.EmployeeId))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(EmployeeDetail));
			}
			return View(employee);
		}

		[HttpGet]
		public async Task<IActionResult> DeleteEmployee(int id)
		{
			var employee = await _context.Employees.FindAsync(id);
			if (employee == null)
			{
				return NotFound();
			}
			return View(employee);
		}

		[HttpPost, ActionName("DeleteEmployee")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteEmployeeConfirmed(int id)
		{
			var employee = await _context.Employees.FindAsync(id);
			if (employee != null)
			{
				_context.Employees.Remove(employee);
				await _context.SaveChangesAsync();
			}
			return RedirectToAction(nameof(EmployeeDetail));
		}

		[HttpGet]
		public async Task<IActionResult> EmployeeDetail()
		{
			var employees = await _context.Employees.ToListAsync();
			return View(employees);
		}

		private bool EmployeeExists(int id)
		{
			return _context.Employees.Any(e => e.EmployeeId == id);
		}

		[HttpGet]
public IActionResult AddBus()
{
    ViewData["BusTypes"] = new SelectList(_context.BusTypes, "BusTypeId", "BusTypeName");
    ViewData["Routes"] = new SelectList(_context.Routes, "RouteId", "RouteName");
    return View();
}


		[HttpPost]
		[ValidateAntiForgeryToken]

		public IActionResult AddBus(Bus bus)
		{
			_context.Buses.Add(bus);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}
		
		[HttpGet]
        public async Task<IActionResult> EditBus(int id)
        {
            var bus = await _context.Buses.FindAsync(id);
            if (bus == null)
            {
                return NotFound();
            }

           
            ViewBag.BusTypeId = new SelectList(await _context.BusTypes.ToListAsync(), "BusTypeId", "BusTypeName");
          
            ViewBag.RouteId = new SelectList(_context.Routes, "RouteId", "RouteName");

            return View(bus);
        }


        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditBus(int id, Bus bus)
		{
			if (id != bus.BusId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(bus);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!BusExists(bus.BusId))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Details));
			}

            ViewBag.BusTypeId = new SelectList(await _context.BusTypes.ToListAsync(), "BusTypeId", "BusTypeName");
            ViewBag.RouteId = new SelectList(_context.Routes, "RouteId", "RouteName");
            return View(bus);
		}

		[HttpGet]
		public async Task<IActionResult> DeleteBus(int id)
		{
			var bus = await _context.Buses
				.Include(b => b.BusType)
				.FirstOrDefaultAsync(b => b.BusId == id);

			if (bus == null)
			{
				return NotFound();
			}
			return View(bus);
		}

		[HttpPost, ActionName("DeleteBus")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteBusConfirmed(int id)
		{
			var bus = await _context.Buses.FindAsync(id);
			if (bus != null)
			{
				_context.Buses.Remove(bus);
				await _context.SaveChangesAsync();
			}
			return RedirectToAction(nameof(Details));
		}

		[HttpGet]
        public async Task<IActionResult> Details()
        {
            var buses = await _context.Buses.Include(b => b.BusType).Include(b => b.Route).ToListAsync();
            return View(buses);
        }




        private bool BusExists(int id)
		{
			return _context.Buses.Any(e => e.BusId == id);
		}

        [HttpGet]
        public IActionResult AddRoute()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRoute(Models.Route route)
        {
            if (ModelState.IsValid)
            {
                _context.Routes.Add(route);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(RouteDetails)); 
            }
            return View(route);
        }

		[HttpGet]
		public async Task<IActionResult> RouteDetails()
		{
			var routes = await _context.Routes.ToListAsync();
			return View(routes);
		}

        [HttpGet]
        public async Task<IActionResult> EditRoute(int id)
        {
            var route = await _context.Routes.FindAsync(id);
            if (route == null)
            {
                return NotFound();
            }
            return View(route);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RouteEdit(int id, Models.Route route)
        {
            if (id != route.RouteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(route);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RouteExists(route.RouteId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(RouteDetails));
            }
            return View(route);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRoute(int id)
        {
            var route = await _context.Routes.FindAsync(id);
            if (route == null)
            {
                return NotFound();
            }
            return View(route);
        }

        [HttpPost, ActionName("DeleteRoute")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRouteConfirmed(int id)
        {
            var route = await _context.Routes.FindAsync(id);
            if (route != null)
            {
                _context.Routes.Remove(route);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(RouteDetails));
        }

        private bool RouteExists(int id)
        {
            return _context.Routes.Any(e => e.RouteId == id);
        }


    }
}
