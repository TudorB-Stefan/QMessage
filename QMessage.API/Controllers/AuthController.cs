using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QMessage.Core.DTOs;
using QMessage.Core.Entities;

namespace QMessage.API.Controllers;

[ApiController]
[Route("api/Controller")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthController(UserManager<User> userManager, SignInManager<User> signInManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        var user = new User
        {
            UserName = model.Username,
            DisplayName = model.DisplayName, 
            Email = model.Email, 
            ProfilePictureUrl  = model.ProfilePictureUrl
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        if(!result.Succeeded) return BadRequest(result.Errors);
        await _signInManager.SignInAsync(user, false);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if(user == null) return Unauthorized();
        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        if(!result.Succeeded) return Unauthorized();
        var token = GenerateJwtToken(user);
        return Ok();
    }    
    [Authorize]
    [HttpGet("user-info")]
    public async Task<ActionResult> GetUserInfo()
    {
        if(User.Identity?.IsAuthenticated == false) return NoContent();
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var user = await _signInManager.UserManager.FindByEmailAsync(email);
        
        return Ok(new
        {
            user.DisplayName,
            user.UserName,
            user.Email,
            user.ProfilePictureUrl,
            user.Id
        });
    }
    [Authorize]
    [HttpGet("auth-status")]
    public ActionResult AuthStatus()
    {
        return Ok(new
        {
            IsAuthenticated = User.Identity?.IsAuthenticated ?? false
        });
    }
    
    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:ExpireMinutes"])),
            signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}