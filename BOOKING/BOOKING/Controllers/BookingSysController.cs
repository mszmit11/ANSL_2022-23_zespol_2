using BOOKING.Models;
using BOOKING.Services;
using BOOKING.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BOOKING.Controllers
{
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
    }
}
