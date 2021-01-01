using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Server.Models; 

namespace Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        [HttpGet]
        public List<ChatUser> Get()
        {
            return Program.Users;
        }
        
        [HttpPost("[controller]/token")]
        public IActionResult Token(string username, string password)
        {
            var identity = CreateIdentity(username, password);

            if (identity == null)
                return BadRequest();

            var issueTime = DateTime.UtcNow;
            var token = new JwtSecurityToken(
                issuer: TokenOptions.ISSUER,
                audience: TokenOptions.AUDIENCE,
                notBefore: issueTime,
                claims: identity.Claims,
                expires: issueTime.Add(TimeSpan.FromMinutes(TokenOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(TokenOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);

            var response = new {access_token = encodedToken, username = identity.Name};
            return Json(response);
        }

        private ClaimsIdentity CreateIdentity(string username, string password)
        {
            var user = Program.Users.FirstOrDefault(x => x.Username == username && x.Password == password);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Username),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
                };

                var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            return null;
        }
    }
}