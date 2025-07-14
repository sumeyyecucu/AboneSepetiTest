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
        private readonly UserServices services;
        public UserController(UserServices _services)
        {
            services = _services;
        }

        [HttpPost("register-user")]
        public async Task<IActionResult> RegisterUser([FromBody] Register dto)
        {
            return Ok(await services.Register(dto, "User"));
        }

        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] Register dto)
        {
            return Ok(await services.Register(dto, "Admin"));
        }

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

        [Authorize(Roles = "Admin")] // Sadece Admin rolü erişebilir
        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var users=await services.GetAllUser();
            return Ok(users);
        }

        [Authorize(Roles = "User")] // Sadece User rolü erişebilir
        [HttpPut("update")]
        public async Task <IActionResult> UpdatePassword([FromBody] UpdatePassword updatePassword)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
              ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not found");
            }

            await services.UpdatePassword(userId, updatePassword);
            return Ok("Parola güncellendi");
        }
       
        
        
    }
}