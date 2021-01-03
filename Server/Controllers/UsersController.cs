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
    /// <summary>
    /// Controller for managing user authentication.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        /// <summary>
        /// GET request
        /// </summary>
        /// <returns>List of all registered users</returns>
        [HttpGet]
        public List<ChatUser> Get()
        {
            return Program.Users;
        }
        
        /// <summary>
        /// POST request for JWT authentication.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>JSON Bearer token for provided user credentials.</returns>
        [HttpPost("/[controller]/token")]
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

        /// <summary>
        /// Function for creating a ClaimsIdentity object from username and password.
        /// If user with provided credentials does not exist, creates it.
        /// If a new user tries to use an already existing username, returns null 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private ClaimsIdentity CreateIdentity(string username, string password)
        {
            var user = Program.Users.FirstOrDefault(x => x.Username == username && x.Password == password);
            
            if (user == null && !(Program.Users.Any(x => x.Username == username)))
            {
                user = new ChatUser {Username = username, Password = password, Role = "user"};
                Program.Users.Add(user);
            }
            
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