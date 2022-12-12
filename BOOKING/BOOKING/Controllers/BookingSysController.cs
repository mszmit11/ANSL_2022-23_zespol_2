using BOOKING.Models;
using BOOKING.Services;
using BOOKING.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Globalization;
using System.Xml.Linq;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BOOKING.Controllers
{
    [Authorize] // tylko po zalogowaniu
    public class BookingSysController : Controller
    {
        //dependency injection
        private readonly IBookingService _bookingService;
        private readonly UserManager<UserModel> _userManager;

        public BookingSysController(IBookingService bookingService, UserManager<UserModel> userManager)
        {
            _bookingService = bookingService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Product body)
        {
            //walidacja
            if (!ModelState.IsValid)
            {
                return View(body);
            }
            
            var id = _bookingService.Save(body);

            TempData["ProductId"] = id;
            //logika do zapisu
            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult List()
        {
            var products = _bookingService.GetAll();
            return View(products);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var product = _bookingService.Get(id);
            TempData["ProductId"] = id;
            TempData["ProductEndDay"] = product.endDate.Day.ToString();
            TempData["ProductEndMonth"] = product.endDate.Month.ToString();
            TempData["ProductEndYear"] = product.endDate.Year.ToString();
            return View(product);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _bookingService.Delete(id);
            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult Search([FromQuery] string location, [FromQuery] DateTime start, [FromQuery] DateTime end, [FromQuery] string cat)
        {
            if(_bookingService == null)
            {
                return Problem("Brak dostępnych pokoi.");
            }
            var products = _bookingService.GetAll();

            if (!String.IsNullOrEmpty(location))
            {
                var result  = products.Where(s => s.Locality.ToLower()!.Contains(location.ToLower()) && s.Category!.Contains(cat)
                    && s.startDate! <= start && s.endDate! >= end);
                return View("List", result);
            }
            return NotFound();
            
        }

        [HttpGet]
        public IActionResult Reservation()
        {
            TempData["ProductId2"] = TempData["ProductId"];
            return View();
        }

        [HttpPost]
        public IActionResult Reservation(Reservation body)
        {
            //walidacja
            if (!ModelState.IsValid)
            {
                return View(body);
            }
            var product = _bookingService.Get((int)TempData["ProductId2"]);
            body.ProductId = product.Id;
            //body.CustomerId = HttpContext.User.FindFirstValue("UserID");
            body.CustomerId = _userManager.GetUserId(HttpContext.User);
            var id = _bookingService.SaveReservation(body);

            return RedirectToAction("ReservationInfo");
        }

        [HttpGet]
        public IActionResult ReservationInfo()
        {
            var reservations = _bookingService.GetAllReservations();
            return View(reservations);
        }

    }
}
