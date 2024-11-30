using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NET8ASP.Data.AppDbContext;
using NET8ASP.Models.Domain;
using NET8ASP.Models.ViewModels;
using System.Security.Claims;

namespace NET8ASP.Controllers
{
    public class AuthController : Controller
    {
        private AppDbContext db;
        public AuthController(AppDbContext context)
        {
            db = context;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            var userModel = new UserViewModel
            {
                User = new User()
            };
            return View(userModel);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string login, string password)
        {
            var user = db.Users.Include(u => u.Role).FirstOrDefault(u => u.Login == login && u.Password == password);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Fio),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim("Login", user.Login),
                    new Claim("Role", user.Role.Id.ToString())
                };
                var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                    new ClaimsPrincipal(claimIdentity), authProperties);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Неверный логин или пароль";
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.User == null)
                {
                    model.User = new User();
                }
                var newUser = new User
                {
                    Fio = model.User.Fio,
                    Phone = model.User.Phone,
                    Login = model.User.Login,
                    Password = model.User.Password,
                    RoleId = 2
                };
                await db.Users.AddAsync(newUser);
                await db.SaveChangesAsync();

                return RedirectToAction("Login", "Auth");
            }
            else
            {
                ViewBag.Error = "Пожалуйста, заполните все поля корректно.";
                return View(model);
            }
        }
    }
}
