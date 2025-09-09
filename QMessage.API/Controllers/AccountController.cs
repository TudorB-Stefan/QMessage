using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using QMessage.Core.DTOs;
using QMessage.Core.Entities;
using QMessage.Core.Interfaces;
using QMessage.API.Extensions;
using QMessage.Infrastructure.Data;

namespace QMessage.API.Controllers;


[Microsoft.AspNetCore.Components.Route("api/[controller]")]
[ApiController]
public class AccountController(UserManager<User> userManager, ITokenService tokenService) : ControllerBase 
{
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        var user = new User
        {
            UserName = registerDto.UserName,
            Email = registerDto.Email,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            ImageUrl = registerDto.ImageUrl
        };
        var result = await userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            return ValidationProblem();
        }
        return user.ToDto(tokenService);
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await userManager.FindByEmailAsync(loginDto.Email);
        if (user == null) return Unauthorized("Invalid email or password");
        var result = await userManager.CheckPasswordAsync(user, loginDto.Password);
        if (!result) return Unauthorized("Invalid email or password");
        return user.ToDto(tokenService);
    }
}