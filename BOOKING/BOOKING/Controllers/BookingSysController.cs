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
using Newtonsoft.Json;

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
            var reservations = _bookingService.GetAllReservations();
            ViewData["Product"] = products;
            ViewData["Reservations"] = reservations;
            return View();
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var product = _bookingService.Get(id);
            TempData["ProductId"] = id;
            string date = product.endDate.ToString("yyyy-MM-dd");
            TempData["ProductDate"] = date;
            var reservations = _bookingService.GetReservationId(id);
            ViewData["Product"] = product;
            ViewData["Reservations"] = reservations;
            return View();
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
                var result = products.Where(s => s.Locality.ToLower()!.Contains(location.ToLower()) && s.Category!.Contains(cat)
                    && s.startDate! <= start && s.endDate! >= end);
                ViewData["Product"] = result;
                ViewData["Reservations"] = _bookingService.GetAllReservations();
                return View("List");
            }
            return NotFound();
            
        }

        [HttpGet]
        public IActionResult Reservation(int id)
        {
            TempData["ProductId2"] = TempData["ProductId"];
            TempData.Keep();
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
