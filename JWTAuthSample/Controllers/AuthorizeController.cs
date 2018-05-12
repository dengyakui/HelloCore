using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JWTAuthSample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.IdentityModel.Tokens;

namespace JWTAuthSample.Controllers
{

    public class AuthorizeController : Controller
    {
        private readonly JwtSettings _jwtSettings;
        public AuthorizeController(IOptions<JwtSettings> option)
        {
            _jwtSettings = option.Value;
        }
        // GET

        public IActionResult Index()
        {
            return Content("Ok");
        }

        [HttpPost]
        public IActionResult Token(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!(model.UserName == "jesse" && model.Password == "123456"))
            {
                return BadRequest();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "jesse"),
                new Claim(ClaimTypes.Role, "admin")
            };

            var credentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_jwtSettings.Issuer, _jwtSettings.Audience, claims, DateTime.Now, DateTime.Now.AddMinutes(30),credentials);
            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}