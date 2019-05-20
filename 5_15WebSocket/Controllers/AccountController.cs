using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using _5_18WebSocket.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace _5_15WebSocket.Controllers
{
    public class AccountController : Controller
    {
       
        private string _connectionString;
        private Manager _mgr;

        public AccountController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
            _mgr = new Manager(_connectionString);
        }
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SigningUp(User user, string password)
        {
                        _mgr.AddUser(user, password);
            return Redirect("/");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoggingIn(string email, string password)
        {
            var user = _mgr.Login(email, password);
            if (user == null)
            {
                TempData["message"] = "Invalid login attempt";
                return Redirect("/account/login");
            }

            var claims = new List<Claim>
                {
                    new Claim("user", email)
                };
            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();
            return Redirect("/home/index");
        }
    }
}