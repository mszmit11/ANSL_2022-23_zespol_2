﻿using BOOKING.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BOOKING.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        //przykladowe uzycie route
        [Route("polityka-prywatnosci")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //nowa sciezka z widokiem, mozna przekierowac do innego controllera
        [Route("testowy-route/{name}")]
        public IActionResult Produkt(string name) 
        {
            return View(); 
        }

        //przekierowanie na privacy
        public IActionResult Redirect()
        {
            return RedirectToAction("Privacy");
        }
    }
}