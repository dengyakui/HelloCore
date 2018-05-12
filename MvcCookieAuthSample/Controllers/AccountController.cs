using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace MvcCookieAuthSample.Controllers
{
    public class AccountController : Controller
    {

        public async Task<IActionResult> MakeLogin()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "jesse"),
                new Claim(ClaimTypes.Role, "admin")
            };
            await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)));
            return Content("ok");
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Content("log out");
        }
    }
}