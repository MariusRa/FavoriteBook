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
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<User> _logger;
        
        public AuthController(UserManager<User> userService, SignInManager<User> signInManager, ILogger<User> logger, RoleManager<IdentityRole> roleManager)
        {
            _userService = userService;
            _signInManager = signInManager;
            _logger = logger;
            _roleManager = roleManager;
        }

        public IActionResult Index() => RedirectToAction("Registration");
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            if (_userService.Users.Count() > 0)
            {
                if (_userService.Users.Count() == 1)
                {
                    if (ModelState.IsValid && (model.Password == model.ConfirmPassword))
                    {
                        var newUser = new User() { Email = model.Email, UserName = model.Name };
                        var result = await _userService.CreateAsync(newUser, model.Password);

                        if (result.Succeeded)
                        {
                            var identityRole = new IdentityRole
                            {
                                Name = "User"
                            };

                            var roleAdmin = await _roleManager.CreateAsync(identityRole);

                            await _userService.AddToRoleAsync(newUser, identityRole.Name);

                            if (_signInManager.IsSignedIn(User))
                            {
                                return RedirectToAction("Index", "Home");
                            }

                            await _signInManager.SignInAsync(newUser, isPersistent: false);
                            return RedirectToAction("Index", "Home");
                        }
                        TempData["Errors"] = result.Errors.First().Description;
                    }
                }
                else
                {
                    if (ModelState.IsValid && (model.Password == model.ConfirmPassword))
                    {
                        var newUser = new User() { Email = model.Email, UserName = model.Name };
                        var result = await _userService.CreateAsync(newUser, model.Password);

                        if (result.Succeeded)
                        {
                            await _userService.AddToRoleAsync(newUser, "User");

                            if (_signInManager.IsSignedIn(User))
                            {
                                return RedirectToAction("Index", "Home");
                            }

                            await _signInManager.SignInAsync(newUser, isPersistent: false);
                            return RedirectToAction("Index", "Home");
                        }
                        TempData["Errors"] = result.Errors.First().Description;
                    }
                }
            }
            else
            {
                if (ModelState.IsValid && (model.Password == model.ConfirmPassword))
                {
                    var newUser = new User() { Email = model.Email, UserName = model.Name };
                    var result = await _userService.CreateAsync(newUser, model.Password);

                    if (result.Succeeded)
                    {
                        var identityRole = new IdentityRole
                        {
                            Name = "Admin"
                        };

                        var roleAdmin = await _roleManager.CreateAsync(identityRole);

                        await _userService.AddToRoleAsync(newUser, identityRole.Name);
                                           
                        if (_signInManager.IsSignedIn(User) && await _userService.IsInRoleAsync(newUser, "Admin"))
                        {
                            return RedirectToAction("ListBooks", "Book");
                        }

                        await _signInManager.SignInAsync(newUser, isPersistent: false);
                        return RedirectToAction("ListBooks", "Book");
                    }
                    TempData["Errors"] = result.Errors.First().Description;
                }
            }
            return View(model);
        }

        public IActionResult Login(string returnUrl = null)
        {
            _logger.LogInformation("User trying logged in.");

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
                        if (await _userService.IsInRoleAsync(myUser, "Admin"))
                        {
                            _logger.LogInformation("Admin logged in.");
                            return RedirectToAction("ListBooks", "Book");
                        }
                        else if (!string.IsNullOrEmpty(model.ReturnUrl))
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
