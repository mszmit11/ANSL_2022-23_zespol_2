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

            var result = await _userManager.CreateAsync(newUser, userRegisterData.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, Enums.Roles.User.ToString());
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login userLoginData)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            //logika logująca - metoda async
            
            var result = await _signInManager.PasswordSignInAsync(userLoginData.userName, userLoginData.Password, false, false); // zapamiętywanie, zablokowywanie
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError(string.Empty, "Nieprawidłowe dane logowania.");
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
