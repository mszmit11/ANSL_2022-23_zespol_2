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
using System.Net;
using BOOKING.CloudStorage;

namespace BOOKING.Controllers
{
    [Authorize] // tylko po zalogowaniu
    public class BookingSysController : Controller
    {
        //dependency injection
        private readonly IBookingService _bookingService;
        private readonly UserManager<UserModel> _userManager;
        private readonly ICloudStorage _cloudStorage;
        private readonly DbBooking _dbBooking;

        public BookingSysController(IBookingService bookingService, UserManager<UserModel> userManager, ICloudStorage cloudStorage, DbBooking dbBooking)
        {
            _bookingService = bookingService;
            _userManager = userManager;
            _cloudStorage = cloudStorage;
            _dbBooking = dbBooking;
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([Bind("Id,Name,Description,Category,Locality,startDate,endDate,ImageUrl,ImageFile")]Product body)
        {
            if (ModelState.IsValid)
            {
                if (body.ImageFile != null)
                {
                    await UploadFile(body);
                }

                _dbBooking.Products.Add(body);
                await _dbBooking.SaveChangesAsync();
                return RedirectToAction("List");
            }
            return View(body);
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

        private async Task UploadFile(Product body)
        {
            string fileNameForStorage = FormFileName(body.Name, body.ImageFile.FileName);
            if (body.Name != null && body.Locality != null && body.Category != null)
            {
                body.ImageUrl = await _cloudStorage.UploadFileAsync(body.ImageFile, fileNameForStorage);
                body.ImageStorageName = fileNameForStorage;
            }
        }

        private static string FormFileName(string title, string fileName)
        {
            var fileExtension = Path.GetExtension(fileName);
            var fileNameForStorage = $"{title}-{DateTime.Now.ToString("yyyyMMddHHmmss")}{fileExtension}";
            return fileNameForStorage;
        }

    }
}
