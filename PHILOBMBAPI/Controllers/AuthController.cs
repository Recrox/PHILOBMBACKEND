using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PHILOBMBusiness.Services.Interfaces;
using PHILOBMCore.Models;
using System.Security.Claims;

namespace PHILOBMBAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IJwtService _jwtService;
    private readonly IUserService _userService; 

    public AuthController(IJwtService jwtService, IUserService userService)
    {
        _jwtService = jwtService;
        _userService = userService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        //var user = await _userService.AuthenticateAsync(loginRequest.Username, loginRequest.Password);
        var user = new User
        {
            Id = 1,
            Username = "Arno",
            Password = "Password",
            Role = Roles.Admin,
        };

        if (user == null)
        {
            return Unauthorized("Invalid credentials");
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role) // Assurez-vous que `user.Role` existe
        };

        var identity = new ClaimsIdentity(claims);
        var token = _jwtService.GenerateToken(identity);

        return Ok(new { token });
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpGet("admin")]
    public IActionResult AdminOnlyEndpoint()
    {
        return Ok("This is an Admin-only endpoint.");
    }

}

