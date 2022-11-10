using BOOKING.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BOOKING.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<UserModel> _userManager; //rejestracja
        private readonly SignInManager<UserModel> _signInManager; //logowanie

        public AccountController(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Register userRegisterData)
        {
            if (!ModelState.IsValid)
            {
                return View(userRegisterData);
            }

            //logika rejestrujaca - metoda asynchroniczna
            var newUser = new UserModel
            {
                Email = userRegisterData.Email,
                UserName = userRegisterData.userName
            };

            //funkcjonalność hash'owania - identity core
            await _userManager.CreateAsync(newUser, userRegisterData.Password);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login userLoginData)
        {
            if (!ModelState.IsValid)
            {
                return View(userLoginData);
            }

            //logika logująca - metoda async
            await _signInManager.PasswordSignInAsync(userLoginData.userName, userLoginData.Password, false, false); // zapamiętywanie, zablokowywanie

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
