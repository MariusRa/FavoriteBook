using FavoriteBook.Models;
using FavoriteBook.viewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace FavoriteBook.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userService;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<User> _logger;
        
        public AuthController(UserManager<User> userService, SignInManager<User> signInManager, ILogger<User> logger)
        {
            _userService = userService;
            _signInManager = signInManager;
            _logger = logger;
        }

        public IActionResult Index() => RedirectToAction("Registration");
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid && (model.Password == model.ConfirmPassword))
            {
                var newUser = new User() { Email = model.Email, UserName = model.Name };
                var result = await _userService.CreateAsync(newUser, model.Password);

                if (result.Succeeded)
                {
                    if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUsers", "Administration");
                    }

                    await _signInManager.SignInAsync(newUser, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                TempData["Errors"] = result.Errors.First().Description;
            }
            return View(model);
        }

        public IActionResult Login(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var myUser = await _userService.FindByEmailAsync(model.Email);

                if (myUser != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(myUser.UserName, model.Password, false, false);

                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }
                        else
                        {
                            
                            _logger.LogInformation("User logged in.");
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                ViewBag.Message = "Email or password is wrong!";
            }

            
            return View(model);
        }

        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out");

            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult Error()
        {
            return View();
        }

    } 
}
