using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AboneSepeti
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly Services services;
        public UserController(Services _services)
        {
            services = _services;
        }
        [Authorize("User")]
        [HttpPost("register-user")] //Sadece User rolüne sahip kullanıcılar erişebilir
        public async Task<IActionResult> RegisterUser([FromBody] Register dto)
        {
            return Ok(await services.Register(dto, "User"));
        }
        [Authorize("Admin")] //Sadece Admin rolüne sahip kullanıcılar erişebilir
        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] Register dto)
            {
                return Ok(await services.Register(dto, "Admin"));
            }


        [Authorize] //Her iki rol de erişebilir
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
              ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (!string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not found");
            }
            var log = await services.LoginUser(login);
            return Ok(log);

        }
        [Authorize] //Her iki rol de erişebilir
        [HttpPost("refresh_token")]
        public async Task<IActionResult> RefreshToken()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
              ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not found");
            }

            var log = await services.RefreshToken(userId);
            return Ok(log);

        }
        [Authorize("Admin")] //Sadece Admin rolüne sahip kullanıcılar erişebilir
        [HttpGet("get_users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await services.GetUsers();
            return Ok(users);
        }
        
        
    }
}