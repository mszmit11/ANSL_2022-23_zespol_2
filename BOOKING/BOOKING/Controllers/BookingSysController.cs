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
using BOOKING.Data;

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
        [Authorize(Roles = "User")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Add([Bind("Id,Name,Description,Category,Locality,startDate,endDate,ImageUrl,ImageFiles, Price, FullAddress, Firm, Number")] Product body)
        {
            if (ModelState.IsValid)
            {
                if (body.ImageFiles != null && body.ImageFiles.Any())
                {
                    await UploadFiles(body, body.ImageFiles);
                }
                body.CustomerId = _userManager.GetUserId(HttpContext.User);
                _dbBooking.Products.Add(body);
                await _dbBooking.SaveChangesAsync();
                return RedirectToAction("List");
            }
            return View(body);
        }

        [HttpGet]
        [Authorize(Roles = "User")]
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
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = _bookingService.Get(id);
            if(product.ImageStorageNames != null)
            {
                foreach(var name in product.ImageStorageNames)
                {
                    await _cloudStorage.DeleteFileAsync(name.Name);
                }
            }
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
        [Authorize(Roles = "User")]
        public IActionResult Reservation(int id)
        {
            TempData["ProductId2"] = TempData["ProductId"];
            TempData.Keep();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "User")]
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
        [Authorize(Roles = "Admin")]
        public IActionResult ReservationInfo()
        {
            var reservations = _bookingService.GetAllReservations();
            return View(reservations);
        }

        private async Task UploadFiles(Product body, IFormFile[] files)
        {


            if (body.Name != null && body.Locality != null && body.Category != null)
            {
                foreach (var file in files)
                {
                    string fileNameForStorage = FormFileName(body.Name, file.FileName);
                    var imageUrls = new[] { new ImageUrls() { Url = await _cloudStorage.UploadFileAsync(file, fileNameForStorage) } };
                    var names = new[] { new ImageStorageNames() { Name = fileNameForStorage } };
                    body.ImageUrls.AddRange(imageUrls);
                    body.ImageStorageNames!.AddRange(names);
                    //body.ImageUrls!.Add(await _cloudStorage.UploadFileAsync(file, fileNameForStorage));
                    //fileNames.Add(fileNameForStorage);
                }

            }
        }

        private static string FormFileName(string name, string fileName)
        {
            var fileExtension = Path.GetExtension(fileName);
            var fileNameForStorage = $"{name}-{DateTime.Now.ToString("yyyyMMddHHmmss")}{fileExtension}";
            return fileNameForStorage;
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public IActionResult MyProducts()
        {

            var products = _bookingService.GetAll();
            var userID = _userManager.GetUserId(HttpContext.User);
            var myProducts = products.Where(s => s.CustomerId!.Contains(userID));

            ViewData["Product"] = myProducts;
            ViewData["Reservations"] = _bookingService.GetAllReservations();
            return View("List");
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public IActionResult MyReservations()
        {

            var reservations = _bookingService.GetAllReservations();
            var userID = _userManager.GetUserId(HttpContext.User);
            var myReservations = reservations.Where(s => s.CustomerId!.Contains(userID));
            
            return View("ReservationInfo", myReservations);
        }



    }
}
