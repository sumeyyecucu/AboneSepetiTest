using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using TestCase.API.Extensions;
using TestCase.Business.IServices;
using TestCase.Contracts.RequestModels.Auth;
using TestCase.Entity.Entities;


namespace TestCase.API.Controllers;

[ApiController]
[Route("api/auth")]

public class AuthController(IAuthService authService, IPasswordHasher passwordHasher) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    
    [HttpPost("registerUser")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] Register register)
    {
        return Ok(await _authService.AddEntityAsync(register, "User"));
    }
    [HttpPost("registerAdmin")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterAdmin([FromBody] Register register)
    {
        return Ok(await _authService.AddEntityAsync(register, "Admin"));
    }

    [HttpPost("login")]
 
    public async Task<IActionResult> Login([FromBody] Login login)
    {
        return Ok(await _authService.Login(login));
    }

    [HttpPost("refreshToken")]
    [Authorize]
    public async Task<IActionResult> RefreshToken()
    {
        var userId = User.GetUserId();
        if (userId == null)
        {
            return NotFound("User not found");
        }
        var response=await _authService.RefreshToken(userId);
        return Ok(response);

    }

    [HttpGet("getAllUser")]
    [Authorize(Roles="Admin")]
    public async Task<IActionResult> GetAllUser()
    {
        var users=await _authService.GetAllEntityAsync();
        return Ok(users);
    }

    [HttpPut("updatePassword")]
    [Authorize(Roles="User")]
    public async Task<IActionResult> UpdatePassword(ChangePassword changePassword)
    {
       
        var userId = User.GetUserId();
        var objectId = new ObjectId(userId);
        var status=await _authService.UpdatePassword(userId,changePassword);
        if (status)
        {
            return Ok("Password updated");
        }
        return BadRequest("Password update failed");
    }

}