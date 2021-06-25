using LoginAPIAngular.Models;
using LoginAPIAngular.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;


namespace LoginAPIAngular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserService service;
        private readonly IConfiguration configuration;

        public readonly IEnumerable<User> users;

        public LoginController(IConfiguration configuration,IUserService service)
        {
            this.configuration = configuration;
            this.service = service;
            users = service.allUsers();
        }

        private string GenerateWebToken(User userInfo)
        {
            
            var user = users.Where(x => x.Username == userInfo.Username && x.Password == userInfo.Password).SingleOrDefault();
            if(user == null)
            {
                return null;
            }
            var signingKey = Convert.FromBase64String(configuration["Jwt:Key"]);
            var expiryDuration = int.Parse(configuration["Jwt:ExpiryDuration"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = null,
                Audience = null,
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(expiryDuration),
                Subject = new ClaimsIdentity(new List<Claim> {
                    new Claim("userid", user.Id.ToString()),
                    new Claim("roles", user.Role),
                    new Claim("Usernames", user.Username.ToString())

                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(signingKey),SecurityAlgorithms.HmacSha256Signature)

            };

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtTokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            var token = jwtTokenHandler.WriteToken(jwtToken);
            return token;

        }

        [HttpPost]
        public IActionResult Login([FromBody] User user)
        {

            var jwtToken = GenerateWebToken(user);
            if(jwtToken == null)
            {
                return Unauthorized();
            }
            return Ok(jwtToken);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()  
        {
            var currentUser = HttpContext.User.Claims.Where(x => x.Type == "userid").SingleOrDefault();

            
            return Ok("connected " + currentUser.Value);
        }





    }
}
