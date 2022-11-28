using BOOKING.Models;
using BOOKING.Services;
using BOOKING.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Xml.Linq;

namespace BOOKING.Controllers
{
    [Authorize] // tylko po zalogowaniu
    public class BookingSysController : Controller
    {
        //dependency injection
        private readonly IBookingService _bookingService;
        public BookingSysController(IBookingService bookingService)
        {
            _bookingService = bookingService;
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
            return View(product);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _bookingService.Delete(id);
            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult FilteredList([FromQuery] string location)
        {
            if(_bookingService == null)
            {
                return Problem("Brak dostępnych pokoi.");
            }
            var products = _bookingService.GetAll();

            if (!String.IsNullOrEmpty(location))
            {
                var result  = products.Where(s => s.Locality!.Contains(location)).OrderByDescending(x=>x.Id);
                return View("List", result);
            }
            return NotFound();
            
        }

    }
}
