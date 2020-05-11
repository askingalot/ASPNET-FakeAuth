using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FakeAuth.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FakeAuth.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            if (login.Username != "foo@bar.baz")
            {
                return Unauthorized();
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("__NOT_A_REAL_KEY__"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var issuer = "http://localhost";
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, login.Username),
                new Claim(JwtRegisteredClaimNames.Email, login.Username),
                new Claim(ClaimTypes.Name, login.Username),
            };
            var token = new JwtSecurityToken(issuer, issuer,
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: creds);

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
    }
}