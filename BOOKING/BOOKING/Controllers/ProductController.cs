using BOOKING.Models;
using BOOKING.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BOOKING.Controllers
{
    public class ProductController : Controller
    {
        private readonly IBookingService _bookingService;
        public ProductController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }
        public IActionResult Index()
        {
            return View();
        }
        //wyswietlanie danych
        public IActionResult Product()
        {
            var product = new Product
            {
                Id = 1,
                Category = "Pokój",
                Description = "Hotel XYZ",
                Name = "Hotel XYZ"
            };

            return View(product);
        }
        //wyswietlanie danych lista
        public IActionResult List()
        {

            var productList = _bookingService.GetAll();
            /*
            {
                new Product
                {
                    Id = 1,
                    Category = "Pokój",
                    Description = "Opis Hotel XYZ",
                    Name = "Pokoj XYZ"
                },
                new Product
                {
                    Id = 2,
                    Category = "Domek",
                    Description = "Opis domku",
                    Name = "Domek XYZ"
                },
                new Product
                {
                    Id = 3,
                    Category = "Nocleg",
                    Description = "Opis noclegu",
                    Name = "Nocleg XYZ"
                }
            };*/

            return View(productList);
        }

        //Sposoby przekazywania danych
        public IActionResult Data()
        {
            //przekazane raz w danym cyklu request
            ViewBag.Name = "Hotel";
            ViewData["Nazwisko"] = "Kowalski";
            //2 cykle request, do formularzy np
            TempData["imie"] = "Jan";
            return View();
        }
    }
}
