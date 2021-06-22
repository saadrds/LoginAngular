using LoginAPIAngular.Models;
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
using System.Threading.Tasks;

namespace LoginAPIAngular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public readonly IEnumerable<User> users = new List<User>
        {
            new User{Id = 1, Username = "saad", Password="yes please", Role ="Admin" },
            new User{Id = 2, Username = "safae", Password="yes please1", Role ="invited" }
        };

        public LoginController(IConfiguration configuration)
        {
            this.configuration = configuration;
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
